using System.ComponentModel;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// شاشة شريط الإدخال السريع (Rapid Bar) — نموذج واجهة تجريبي (UI shape / UI-first prototype)
/// ببيانات محلية في الذاكرة فقط. لا يوجد أي اتصال بالخادم أو قاعدة البيانات في هذا النموذج.
///
/// سير العمل:
///  - البحث الحي بالباركود / الاسم / الكود مع قائمة نتائج منسدلة (الاسم + الكود/الباركود + السعر + المخزون).
///  - إدراج الصنف في الجدول (القراءة فقط) عبر زر "إدراج" أو Enter.
///  - اختيار منتج من نافذة بحث منبثقة عبر [F3].
///  - إجماليات حية: المجموع الفرعي / إجمالي الخصم / الصافي.
/// </summary>
public partial class frmSalesRapidBar : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ================== بيانات تجريبية (في الذاكرة فقط) ==================

    private sealed class MockProduct
    {
        public MockProduct(string code, string barcode, string name, string[] units, decimal price, decimal stock)
        {
            Code = code;
            Barcode = barcode;
            Name = name;
            Units = units;
            Price = price;
            Stock = stock;
        }

        public string Code { get; }
        public string Barcode { get; }
        public string Name { get; }
        public string[] Units { get; }
        public decimal Price { get; }
        public decimal Stock { get; }
        public override string ToString() => $"{Name} — {Code}";
    }

    private static readonly MockProduct[] MockProducts =
    {
        new("C-1001", "6291041500213", "شاي ليبتون (100 كيس)",    new[] { "كرتون", "علبة" },          3500m, 120m),
        new("C-1002", "6291041500220", "أرز هندي بسمتي (10 كجم)", new[] { "كيس", "كيس صغير" },        9800m,  45m),
        new("C-1003", "6291041500237", "زيت نباتي (5 لتر)",       new[] { "عبوة", "كرتون" },          4200m,  60m),
        new("C-1004", "6291041500244", "سكر أبيض (1 كجم)",        new[] { "كيس", "كيس 25 كجم" },       850m, 200m),
        new("C-1005", "6291041500251", "حليب نيدو بودرة (1 كجم)", new[] { "علبة", "كرتون" },          2450m,  35m),
        new("C-1006", "6291041500268", "دقيق قمح فاخر (1 كجم)",   new[] { "كيس" },                    700m, 150m),
        new("C-1007", "6291041500275", "معجون طماطم (800 جم)",    new[] { "علبة", "كرتون" },           600m,  80m),
        new("C-1008", "6291041500282", "مياه معدنية (كرتون 12)",  new[] { "كرتون", "عبوة" },          1800m,  90m),
    };

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

    private static readonly MockCustomer[] MockCustomers =
    {
        new(1, "عميل نقدي", 0m),                 // حساب خالص
        new(2, "محمد أحمد الشرعبي", 25000m),     // عليه للمحل
        new(3, "فاطمة علي", -12000m),            // له دفعة مسبقة
        new(4, "علي حسن", 0m),                   // حساب خالص
    };

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

    // ================== حالة الشاشة ==================

    private readonly BindingList<CartLine> _cart = new();
    private readonly List<MockProduct> _matches = new();
    private MockProduct? _selectedProduct;
    private MockProduct? _pickerSelected;
    private int _invoiceSeq = 124;

    public frmSalesRapidBar(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider; // محجوز لربط خدمات API لاحقاً (نمط الحقن عبر DI)
        InitializeComponent();
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
        txtInvoiceNo.Text = $"SI-{_invoiceSeq:D6}";

        // العملاء (بيانات تجريبية)
        cmbCustomer.DataSource = new BindingList<MockCustomer>(MockCustomers.ToList());
        cmbCustomer.DisplayMember = "Name";
        cmbCustomer.SelectedIndex = 0;

        // صفان تجريبيان لتوضيح شكل الشاشة
        AddCartLine(MockProducts[0], 2m); // شاي ليبتون
        AddCartLine(MockProducts[2], 3m); // زيت نباتي
        RecalcTotals();

        ActiveControl = txtSearch; // تركيز تلقائي على حقل البحث السريع
        txtSearch.Focus();
    }

    // ================== البحث الحي (قائمة النتائج المنسدلة) ==================

    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        RefreshMatches();
    }

    private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            TryCommitSearch();
        }
        else if (e.KeyCode == Keys.Down && lstMatches.Visible)
        {
            lstMatches.Focus();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Escape)
        {
            HideMatches();
            e.Handled = true;
        }
    }

    private void txtSearch_Leave(object? sender, EventArgs e)
    {
        // إخفاء القائمة عند مغادرة الحقل ما لم يكن التركيز انتقل إليها
        if (!lstMatches.Focused) HideMatches();
    }

    private void RefreshMatches()
    {
        if (_settingSearchText) return;

        var query = txtSearch.Text.Trim();
        if (query.Length == 0)
        {
            HideMatches();
            return;
        }

        _matches.Clear();
        _matches.AddRange(MockProducts.Where(p =>
            p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Code.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            p.Barcode.Contains(query, StringComparison.OrdinalIgnoreCase)));

        if (_matches.Count == 0)
        {
            HideMatches();
            return;
        }

        lstMatches.Items.Clear();
        foreach (var p in _matches)
        {
            lstMatches.Items.Add($"{p.Name}  |  {p.Code} / {p.Barcode}  |  السعر: {p.Price:N2}  |  المخزون: {p.Stock:N0}");
        }

        PositionMatchesList();
        lstMatches.SelectedIndex = 0;
        lstMatches.Visible = true;
        lstMatches.BringToFront();
    }

    private void PositionMatchesList()
    {
        var screenPoint = txtSearch.PointToScreen(Point.Empty);
        var location = PointToClient(screenPoint);
        lstMatches.SetBounds(location.X, location.Y + txtSearch.Height + 2, txtSearch.Width, lstMatches.Height);
    }

    private void HideMatches()
    {
        lstMatches.Visible = false;
    }

    /// <summary>تأكيد البحث: مطابقة باركود تامة → إدراج فوري؛ وإلا تحديد أول نتيجة للانتقال إلى الكمية.</summary>
    private void TryCommitSearch()
    {
        var query = txtSearch.Text.Trim();
        if (query.Length == 0) return;

        // باركود كامل ممسوح → إدراج مباشر (أسرع مسار إدخال)
        var byBarcode = MockProducts.FirstOrDefault(p => p.Barcode == query);
        if (byBarcode is not null)
        {
            SelectMatch(byBarcode);
            InsertLine();
            txtSearch.Clear();
            txtSearch.Focus();
            return;
        }

        // النص يعرض بالفعل اسم المنتج المحدد → إعادة تأكيد نفس المنتج
        if (_selectedProduct is not null &&
            string.Equals(query, _selectedProduct.Name, StringComparison.OrdinalIgnoreCase))
        {
            SelectMatch(_selectedProduct);
            return;
        }

        // نتائج بحث → تحديد أول/محدد نتيجة والانتقال للكمية
        if (lstMatches.Visible && _matches.Count > 0)
        {
            int index = lstMatches.SelectedIndex >= 0 ? lstMatches.SelectedIndex : 0;
            if (index < _matches.Count)
            {
                SelectMatch(_matches[index]);
                return;
            }
        }

        MessageBox.Show("لا توجد نتائج مطابقة للبحث.", "البحث السريع",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtSearch.Focus();
    }

    private void lstMatches_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            CommitSelectedMatch();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            HideMatches();
            txtSearch.Focus();
            e.Handled = true;
        }
    }

    private void lstMatches_DoubleClick(object? sender, EventArgs e) => CommitSelectedMatch();

    private void lstMatches_Leave(object? sender, EventArgs e) => HideMatches();

    private void CommitSelectedMatch()
    {
        if (lstMatches.SelectedIndex < 0 || lstMatches.SelectedIndex >= _matches.Count) return;
        SelectMatch(_matches[lstMatches.SelectedIndex]);
    }

    // ================== اختيار المنتج وملء حقول الإدخال ==================

    private bool _settingSearchText;

    private void SelectMatch(MockProduct product)
    {
        _selectedProduct = product;

        _settingSearchText = true;
        txtSearch.Text = product.Name;
        _settingSearchText = false;
        HideMatches();

        cmbUnit.Items.Clear();
        foreach (var unit in product.Units) cmbUnit.Items.Add(unit);
        cmbUnit.SelectedIndex = 0;

        numPrice.Value = product.Price;
        numQty.Value = 1m;
        numQty.Focus();
    }

    private void btnPickProduct_Click(object? sender, EventArgs e) => ShowProductPicker();

    /// <summary>نافذة اختيار منتج [F3] — نافذة منبثقة مبنية برمجياً (بدون ملف إضافي) ببيانات التجربة.</summary>
    private void ShowProductPicker()
    {
        _pickerSelected = null;

        using var dlg = new Form
        {
            Text = "اختيار منتج [F3]",
            StartPosition = FormStartPosition.CenterParent,
            Size = new Size(700, 460),
            MinimumSize = new Size(620, 400),
            RightToLeft = RightToLeft.Yes,
            RightToLeftLayout = true,
            Font = new Font("Tahoma", 11F),
            BackColor = Color.WhiteSmoke,
            ShowInTaskbar = false,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
        };

        var txtSearch = new TextBox
        {
            Dock = DockStyle.Top,
            Height = 36,
            Font = new Font("Tahoma", 11F),
            PlaceholderText = "بحث بالاسم / الكود / الباركود...",
        };

        var btnOk = new Button { Text = "اختيار", BackColor = Color.FromArgb(25, 135, 84), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Height = 40, Width = 130, Font = new Font("Tahoma", 12F, FontStyle.Bold), Cursor = Cursors.Hand };
        var btnCancel = new Button { Text = "إلغاء", BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Height = 40, Width = 110, Font = new Font("Tahoma", 11F), Cursor = Cursors.Hand };
        var flpButtons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 56, FlowDirection = FlowDirection.RightToLeft, WrapContents = false, Padding = new Padding(10, 8, 10, 8) };
        flpButtons.Controls.Add(btnOk);
        flpButtons.Controls.Add(btnCancel);

        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            AutoGenerateColumns = false,
            RowTemplate = { Height = 32 },
            EnableHeadersVisualStyles = false,
        };
        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        grid.DefaultCellStyle.Font = new Font("Tahoma", 11F);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 242, 242);
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "cName", HeaderText = "اسم الصنف", DataPropertyName = "Name", Width = 270 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "cCode", HeaderText = "الكود", DataPropertyName = "Code", Width = 90 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "cBarcode", HeaderText = "الباركود", DataPropertyName = "Barcode", Width = 130 });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "cPrice", HeaderText = "السعر", DataPropertyName = "Price", Width = 110, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight } });
        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "cStock", HeaderText = "المخزون", DataPropertyName = "Stock", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });

        dlg.Controls.Add(grid);
        dlg.Controls.Add(txtSearch);
        dlg.Controls.Add(flpButtons);

        // مصدر بيانات (DataTable + DataView بنمط التصفية المعتاد في المشروع)
        var table = new DataTable();
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Code", typeof(string));
        table.Columns.Add("Barcode", typeof(string));
        table.Columns.Add("Price", typeof(decimal));
        table.Columns.Add("Stock", typeof(decimal));
        foreach (var p in MockProducts)
            table.Rows.Add(p.Name, p.Code, p.Barcode, p.Price, p.Stock);

        var view = new DataView(table);
        grid.DataSource = view;

        txtSearch.TextChanged += (_, _) =>
        {
            var q = txtSearch.Text.Trim();
            if (q.Length == 0)
            {
                view.RowFilter = string.Empty;
            }
            else
            {
                var safe = q.Replace("'", "''");
                view.RowFilter = $"Name LIKE '%{safe}%' OR Code LIKE '%{safe}%' OR Barcode LIKE '%{safe}%'";
            }
        };
        txtSearch.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnOk.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                dlg.Close();
            }
        };
        grid.DoubleClick += (_, _) => btnOk.PerformClick();
        btnOk.Click += (_, _) =>
        {
            if (grid.CurrentRow?.DataBoundItem is not DataRowView row)
            {
                MessageBox.Show("اختر صنفاً من القائمة أولاً.", "اختيار منتج",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var code = row["Code"] as string ?? string.Empty;
            _pickerSelected = MockProducts.FirstOrDefault(p => p.Code == code);
            dlg.DialogResult = DialogResult.OK;
        };
        btnCancel.Click += (_, _) => dlg.DialogResult = DialogResult.Cancel;
        grid.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) btnOk.PerformClick();
        };

        if (dlg.ShowDialog(this) == DialogResult.OK && _pickerSelected is not null)
        {
            SelectMatch(_pickerSelected);
        }
    }

    // ================== السلة والإدراج ==================

    private void AddCartLine(MockProduct product, decimal quantity)
    {
        var existing = _cart.FirstOrDefault(l => l.ProductName == product.Name && l.UnitName == product.Units[0]);
        if (existing is not null)
        {
            existing.Quantity += quantity;
            existing.UnitPrice = product.Price;
        }
        else
        {
            _cart.Add(new CartLine
            {
                LineNo = _cart.Count + 1,
                ProductName = product.Name,
                UnitName = product.Units[0],
                Quantity = quantity,
                UnitPrice = product.Price,
            });
        }

        RenumberLines();
    }

    private void btnInsert_Click(object? sender, EventArgs e) => InsertLine();

    private void numQty_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        InsertLine();
    }

    private void numPrice_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        InsertLine();
    }

    private void InsertLine()
    {
        if (_selectedProduct is null)
        {
            MessageBox.Show("اختر منتجاً أولاً من البحث أو من زر [F3].", "إدراج صنف",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtSearch.Focus();
            return;
        }

        decimal quantity = numQty.Value;
        decimal price = numPrice.Value;
        string unit = cmbUnit.SelectedItem as string ?? _selectedProduct.Units[0];

        var existing = _cart.FirstOrDefault(l => l.ProductName == _selectedProduct.Name && l.UnitName == unit);
        if (existing is not null)
        {
            existing.Quantity += quantity;
            existing.UnitPrice = price;
        }
        else
        {
            _cart.Add(new CartLine
            {
                LineNo = _cart.Count + 1,
                ProductName = _selectedProduct.Name,
                UnitName = unit,
                Quantity = quantity,
                UnitPrice = price,
            });
        }

        RenumberLines();
        RecalcTotals();

        // تحديد آخر صف لإظهار الإضافة والاستمرار بإدخال الكميات
        dgvCart.ClearSelection();
        if (_cart.Count > 0)
        {
            dgvCart.Rows[_cart.Count - 1].Selected = true;
            dgvCart.CurrentCell = dgvCart.Rows[_cart.Count - 1].Cells[0];
        }

        numQty.Value = 1m;
        numQty.Focus();
    }

    private void RenumberLines()
    {
        for (int i = 0; i < _cart.Count; i++)
        {
            _cart[i].LineNo = i + 1;
        }
    }

    // ================== الإجماليات ==================

    private void RecalcTotals()
    {
        decimal subTotal = _cart.Sum(l => l.LineTotal);
        decimal discount = numDiscount.Value;
        decimal netTotal = subTotal - discount;
        if (netTotal < 0) netTotal = 0m;

        lblSubTotalVal.Text = subTotal.ToString("N2");
        lblDiscountTotalVal.Text = discount.ToString("N2");
        lblNetTotalVal.Text = netTotal.ToString("N2");
    }

    private void numDiscount_ValueChanged(object? sender, EventArgs e) => RecalcTotals();

    private void cmbCustomer_SelectedIndexChanged(object? sender, EventArgs e) => UpdateBalanceBadge();

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

    // ================== الحفظ والإغلاق ==================

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("الفاتورة فارغة. أضف صنفاً واحداً على الأقل.", "حفظ واعتماد",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtSearch.Focus();
            return;
        }

        MessageBox.Show("تم اعتماد الفاتورة بنجاح (وضع تجريبي).", "حفظ واعتماد",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        _invoiceSeq++;
        txtInvoiceNo.Text = $"SI-{_invoiceSeq:D6}";
        _cart.Clear();
        _selectedProduct = null;
        cmbUnit.Items.Clear();
        numQty.Value = 1m;
        numPrice.Value = 0m;
        RecalcTotals();
        txtSearch.Clear();
        txtSearch.Focus();
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    private void frmSalesRapidBar_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F3)
        {
            ShowProductPicker(); // اختيار منتج [F3]
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete && dgvCart.CurrentRow?.DataBoundItem is CartLine line)
        {
            _cart.Remove(line);
            RenumberLines();
            RecalcTotals();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Escape)
        {
            HideMatches();
        }
    }
}