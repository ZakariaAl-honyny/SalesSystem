using System.ComponentModel;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace SalesSystem.Desktop.Screens.POS.Forms;

/// <summary>
/// شاشة نقطة البيع (POS) — نموذج واجهة تجريبي (UI shape) ببيانات محلية في الذاكرة فقط.
/// لا يوجد أي اتصال بالخادم أو قاعدة البيانات في هذا النموذج.
/// </summary>
public partial class frmPOS : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ================== بيانات تجريبية (في الذاكرة فقط) ==================

    private static readonly MockProduct[] MockProducts =
    {
        new("1001", "شاي ليبتون", "كرتون", 3500m),
        new("1002", "زيت نباتي (5 لتر)", "عبوة", 4200m),
        new("1003", "أرز هندي (10 كجم)", "كيس", 9800m),
        new("1004", "سكر (1 كجم)", "كيس", 850m),
        new("1005", "حليب نيدو (1 كجم)", "علبة", 2450m),
        new("1006", "دقيق قمح (1 كجم)", "كيس", 700m),
        new("1007", "معجون طماطم", "علبة", 600m),
        new("1008", "صابون غسيل", "قطعة", 450m),
        new("1009", "مياه معدنية (كرتون)", "كرتون", 1800m),
        new("1010", "جبنة مثلثات", "علبة", 1100m),
    };

    private static readonly MockCustomer[] MockCustomers =
    {
        new(1, "عميل نقدي", 0m),                 // حساب خالص
        new(2, "محمد أحمد الشرعبي", 25000m),     // عليه للمحل
        new(3, "فاطمة علي", -12000m),            // له دفعة مسبقة
        new(4, "علي حسن", 0m),                   // حساب خالص
    };

    private readonly BindingList<CartLine> _cart = new();

    public frmPOS(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider; // محجوز لربط خدمات API لاحقاً (نمط الحقن عبر DI)
        InitializeComponent();
        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
        SetupGrid();
        LoadMockData();
    }

    // ================== التهيئة ==================

    private void SetupGrid()
    {
        dgvCart.AutoGenerateColumns = false; // الأعمدة معرّفة في المصمم
        dgvCart.DataSource = _cart;
    }

    private void LoadMockData()
    {
        cmbCurrency.SelectedIndex = 0; // ريال يمني
        cmbPayMethod.SelectedIndex = 0; // نقدي

        // العملاء (بيانات تجريبية)
        cmbCustomer.DataSource = new BindingList<MockCustomer>(MockCustomers.ToList());
        cmbCustomer.DisplayMember = "Name";
        cmbCustomer.SelectedIndex = 0;

        // صفوف سلة تجريبية لتوضيح شكل الشاشة
        AddCartLine("1001", 2m); // شاي ليبتون
        AddCartLine("1002", 3m); // زيت نباتي
        AddCartLine("1003", 1m); // أرز هندي
        AddCartLine("1004", 5m); // سكر

        ActiveControl = txtBarcode; // تركيز تلقائي على حقل الباركود [F2]
        txtBarcode.Focus();
    }

    // ================== منطق السلة (نموذج تجريبي) ==================

    private void AddCartLine(string barcode, decimal quantity)
    {
        var product = MockProducts.FirstOrDefault(p => p.Barcode == barcode);
        if (product is null) return;

        var existing = _cart.FirstOrDefault(l => l.ProductName == product.Name);
        if (existing is not null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            _cart.Add(new CartLine
            {
                LineNo = _cart.Count + 1,
                ProductName = product.Name,
                UnitName = product.Unit,
                Quantity = quantity,
                UnitPrice = product.Price,
            });
        }
    }

    private void RenumberLines()
    {
        for (int i = 0; i < _cart.Count; i++)
        {
            _cart[i].LineNo = i + 1;
        }
    }

    private void UpdateBalanceBadge()
    {
        if (cmbCustomer.SelectedItem is not MockCustomer customer)
        {
            lblBalanceBadge.Text = "—";
            lblBalanceBadge.BackColor = Color.FromArgb(108, 117, 125);
            return;
        }

        if (customer.Balance > 0)
        {
            lblBalanceBadge.Text = $"عليه للمحل: {customer.Balance:N2}";
            lblBalanceBadge.BackColor = Color.FromArgb(220, 53, 69); // أحمر
        }
        else if (customer.Balance < 0)
        {
            lblBalanceBadge.Text = $"له دفعة مسبقة: {Math.Abs(customer.Balance):N2}";
            lblBalanceBadge.BackColor = Color.FromArgb(25, 135, 84); // أخضر
        }
        else
        {
            lblBalanceBadge.Text = "حساب خالص";
            lblBalanceBadge.BackColor = Color.FromArgb(108, 117, 125); // رمادي
        }
    }

    /// <summary>إعادة حساب الإجماليات — نسخة مبسطة (stub) لعرض شكل الشاشة.</summary>
    private void RecalcTotals()
    {
        decimal subTotal = _cart.Sum(l => l.LineTotal);
        decimal discount = numDiscount.Value;
        decimal taxRate = numTax.Value;
        decimal taxAmount = (subTotal - discount) * (taxRate / 100m);
        decimal netTotal = subTotal - discount + taxAmount;

        decimal paidCash = ParseMoney(txtPaidCash.Text);
        decimal paidBank = ParseMoney(txtPaidBank.Text);
        decimal paidWallet = ParseMoney(txtPaidWallet.Text);
        decimal paidTotal = paidCash + paidBank + paidWallet;
        decimal remaining = netTotal - paidTotal;

        // المستحق للمحل = دين العميل السابق (إن وُجد) + ما تبقى من هذه الفاتورة
        decimal dueToStore = 0m;
        if (cmbCustomer.SelectedItem is MockCustomer customer && customer.Balance > 0)
            dueToStore += customer.Balance;
        if (remaining > 0)
            dueToStore += remaining;

        lblSubTotalVal.Text = subTotal.ToString("N2");
        lblDiscountTotalVal.Text = discount.ToString("N2");
        lblNetTotalVal.Text = netTotal.ToString("N2");
        lblPaidTotalVal.Text = paidTotal.ToString("N2");
        lblRemainingVal.Text = remaining.ToString("N2");
        lblRemainingVal.ForeColor = remaining > 0 ? Color.FromArgb(220, 53, 69) : Color.FromArgb(25, 135, 84);
        lblDueToStoreVal.Text = dueToStore.ToString("N2");
    }

    private static decimal ParseMoney(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return 0m;
        // دعم الفاصلة العشرية العربية (٫) وفاصلة الآلاف
        var normalized = text.Trim().Replace('٫', '.').Replace(",", string.Empty);
        return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            ? value
            : 0m;
    }

    // ================== أحداث الواجهة ==================

    private void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;

        var code = txtBarcode.Text.Trim();
        if (code.Length == 0) return;

        var product = MockProducts.FirstOrDefault(p => p.Barcode == code);
        if (product is null)
        {
            MessageBox.Show("الصنف غير موجود في بيانات التجربة.", "بحث الباركود",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtBarcode.Clear();
            return;
        }

        AddCartLine(product.Barcode, 1m);
        txtBarcode.Clear();
        RecalcTotals();

        // تحديد آخر صف لإظهار الإضافة
        dgvCart.ClearSelection();
        if (_cart.Count > 0)
            dgvCart.Rows[_cart.Count - 1].Selected = true;
    }

    private void cmbCustomer_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateBalanceBadge();
        RecalcTotals();
    }

    private void MoneyField_TextChanged(object? sender, EventArgs e) => RecalcTotals();

    private void btnPending_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("لا توجد فواتير معلقة في وضع التجربة.", "فواتير معلقة [F4]",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("السلة فارغة. أضف صنفاً واحداً على الأقل.", "حفظ واعتماد",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show("تم اعتماد الفاتورة بنجاح (وضع تجريبي).", "حفظ واعتماد",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        _cart.Clear();
        RecalcTotals();
    }

    private void frmPOS_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F2)
        {
            txtBarcode.Focus(); // باركود [F2]
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.F4)
        {
            btnPending_Click(sender, EventArgs.Empty); // فواتير معلقة [F4]
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete && dgvCart.CurrentRow?.DataBoundItem is CartLine line)
        {
            _cart.Remove(line);
            RenumberLines();
            RecalcTotals();
            e.Handled = true;
        }
    }

    // ================== فئات البيانات التجريبية ==================

    private sealed class MockProduct
    {
        public MockProduct(string barcode, string name, string unit, decimal price)
        {
            Barcode = barcode;
            Name = name;
            Unit = unit;
            Price = price;
        }

        public string Barcode { get; }
        public string Name { get; }
        public string Unit { get; }
        public decimal Price { get; }
        public override string ToString() => $"{Name} ({Unit})";
    }

    private sealed class MockCustomer
    {
        public MockCustomer(int id, string name, decimal balance)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }

        public int Id { get; }
        public string Name { get; }
        public decimal Balance { get; }
        public override string ToString() => Name;
    }

    private sealed class CartLine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _lineNo;
        private string _productName = string.Empty;
        private string _unitName = string.Empty;
        private decimal _quantity;
        private decimal _unitPrice;

        public int LineNo
        {
            get => _lineNo;
            set { _lineNo = value; OnPropertyChanged(nameof(LineNo)); }
        }

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(nameof(ProductName)); }
        }

        public string UnitName
        {
            get => _unitName;
            set { _unitName = value; OnPropertyChanged(nameof(UnitName)); }
        }

        public decimal Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); OnPropertyChanged(nameof(LineTotal)); }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set { _unitPrice = value; OnPropertyChanged(nameof(UnitPrice)); OnPropertyChanged(nameof(LineTotal)); }
        }

        public decimal LineTotal => Quantity * UnitPrice;

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}