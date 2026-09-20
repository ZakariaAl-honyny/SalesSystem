using System.ComponentModel;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// شاشة المسح المتواصل بالباركود — إدخال فاتورة مبيعات عن طريق ماسح الليزر (Laser Scanner).
/// نسخة أولية (UI shape / UI-first prototype) ببيانات تجريبية داخل الذاكرة فقط —
/// لا يوجد أي اتصال بالـ API أو قاعدة البيانات في هذا النموذج.
/// 
/// سلوك المسح المتواصل (مترجم من مرجع ويب):
///  - تركيز تلقائي دائم على حقل الباركود بعد كل مسحة.
///  - كل مسحة لنفس الصنف تزيد الكمية +1 (دمج تلقائي بدون صف مكرر).
///  - باركود غير معروف → وميض أحمر + رسالة خطأ عربية + إعادة تركيز وتحديد (لا يقاطع سير المسح).
/// </summary>
public partial class frmSalesContinuousScanner : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (في الذاكرة فقط)
    // ═══════════════════════════════════════════════
    private sealed class MockProduct
    {
        public string Barcode { get; init; } = "";
        public string Name { get; init; } = "";
        public string Unit { get; init; } = "";
        public decimal Price { get; init; }
        public override string ToString() => $"{Name} ({Unit})";
    }

    private static readonly MockProduct[] MockProducts =
    {
        new() { Barcode = "102030", Name = "شاي ليبتون 250غ",   Unit = "قطعة",   Price = 500m },
        new() { Barcode = "102031", Name = "زيت نباتي 1 لتر",  Unit = "زجاجة",  Price = 1500m },
        new() { Barcode = "102032", Name = "أرز بسمتي 5 كغ",   Unit = "كيس",     Price = 3500m },
        new() { Barcode = "102033", Name = "سكر أبيض 1 كغ",    Unit = "كيس",     Price = 800m },
        new() { Barcode = "102034", Name = "دقيق فاخر 2 كغ",   Unit = "كيس",     Price = 1200m },
        new() { Barcode = "102035", Name = "معجون طماطم 400غ", Unit = "علبة",    Price = 600m },
    };

    private readonly BindingList<CartLine> _cart = new();

    private readonly System.Windows.Forms.Timer _flashTimer;

    public frmSalesContinuousScanner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider; // محجوز لربط خدمات API لاحقاً (نمط الحقن عبر DI)

        _flashTimer = new System.Windows.Forms.Timer();
        _flashTimer.Tick += _flashTimer_Tick;

        InitializeComponent();
        SetupGrid();
        FocusScanner();
    }

    // ═══════════════════════════════════════════════
    // التهيئة
    // ═══════════════════════════════════════════════
    private void SetupGrid()
    {
        dgvCart.AutoGenerateColumns = false;
        dgvCart.Columns.Clear();

        dgvCart.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColLineNo", HeaderText = "#", DataPropertyName = "LineNo", Width = 40,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvCart.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColName", HeaderText = "اسم الصنف", DataPropertyName = "ProductName", Width = 220
        });
        dgvCart.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColBarcode", HeaderText = "الباركود", DataPropertyName = "Barcode", Width = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvCart.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "0.###", Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvCart.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "UnitPrice", Width = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvCart.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleCenter }
        });

        dgvCart.DataSource = _cart;
    }

    private void frmSalesContinuousScanner_Shown(object? sender, EventArgs e) => FocusScanner();

    /// <summary>تركيز دائم على حقل الباركود مع تحديد النص — جاهز للمسحة التالية.</summary>
    private void FocusScanner()
    {
        txtBarcode.Clear();
        txtBarcode.Focus();
        txtBarcode.SelectAll();
    }

    // ═══════════════════════════════════════════════
    // منطق المسح — Merge بنفس الباركود (+1 لكل مسحة)
    // ═══════════════════════════════════════════════
    private void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        HandleScan(txtBarcode.Text.Trim());
    }

    private void btnDemo_Click(object? sender, EventArgs e)
    {
        if (sender is Button b && b.Tag is string code)
            HandleScan(code);
    }

    private void HandleScan(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
        {
            FocusScanner();
            return;
        }

        var product = MockProducts.FirstOrDefault(p => p.Barcode == barcode);
        if (product is null)
        {
            FlashScanError(barcode);
            return;
        }

        AddOrMerge(product);
        UpdateLastScanCard(product);
        FlashSuccess();
        FocusScanner();
    }

    /// <summary>إضافة سطر جديد أو زيادة كمية السطر الموجود (دمج — بدون صف مكرر).</summary>
    private void AddOrMerge(MockProduct product)
    {
        var existing = _cart.FirstOrDefault(l => l.Barcode == product.Barcode);
        if (existing is not null)
        {
            existing.Quantity += 1m; // كل مسحة = +1
        }
        else
        {
            _cart.Add(new CartLine
            {
                LineNo = _cart.Count + 1,
                ProductName = product.Name,
                Barcode = product.Barcode,
                Quantity = 1m,
                UnitPrice = product.Price,
            });
        }

        RecalcTotals();

        // تحديد آخر سطر لإبراز الإضافة (أو السطر المدمج)
        var target = existing ?? _cart[^1];
        int index = _cart.IndexOf(target);
        if (index >= 0)
        {
            dgvCart.ClearSelection();
            dgvCart.Rows[index].Selected = true;
            dgvCart.CurrentCell = dgvCart.Rows[index].Cells[1];
        }
    }

    private void UpdateLastScanCard(MockProduct product)
    {
        lblLastScanInfo.Text = $"{product.Name} — باركود {product.Barcode} — {DateTime.Now:HH:mm:ss}";
        lblLastScanTitle.Text = "آخر مسح ناجح";
    }

    private void RenumberLines()
    {
        for (int i = 0; i < _cart.Count; i++)
            _cart[i].LineNo = i + 1;
    }

    /// <summary>
    /// المجموع = مجموع بنود السلة (Quantity × UnitPrice).
    /// الصافي = المجموع (بدون خصم أو ضريبة في هذه النسخة التجريبية).
    /// </summary>
    private void RecalcTotals()
    {
        decimal subTotal = _cart.Sum(l => l.LineTotal);
        decimal netTotal = subTotal;

        lblSubTotalVal.Text = subTotal.ToString("N2");
        lblNetTotalVal.Text = netTotal.ToString("N2");
    }

    // ═══════════════════════════════════════════════
    // وميض النجاح / الخطأ (غير حاجب لسير المسح)
    // ═══════════════════════════════════════════════
    private void FlashSuccess()
    {
        pnlLastScan.BackColor = Color.FromArgb(25, 135, 84);       // أخضر مشبع
        lblLastScanTitle.ForeColor = Color.White;
        lblLastScanInfo.ForeColor = Color.White;
        _flashTimer.Interval = 900;
        _flashTimer.Start();
    }

    private void FlashScanError(string barcode)
    {
        pnlScanBox.BackColor = Color.FromArgb(220, 53, 69);        // أحمر
        lblScanError.Text = $"الباركود «{barcode}» غير معروف — حاول مرة أخرى";
        lblScanError.Visible = true;
        System.Media.SystemSounds.Exclamation.Play();
        _flashTimer.Interval = 1400;
        _flashTimer.Start();

        FocusScanner(); // إعادة تركيز + تحديد النص الخاطئ لمسحه بسرعة
    }

    private void _flashTimer_Tick(object? sender, EventArgs e)
    {
        _flashTimer.Stop();

        // استعادة الألوان الطبيعية
        pnlScanBox.BackColor = Color.FromArgb(25, 135, 84);
        lblScanError.Visible = false;
        pnlLastScan.BackColor = Color.FromArgb(211, 240, 222);
        lblLastScanTitle.ForeColor = Color.FromArgb(21, 87, 36);
        lblLastScanInfo.ForeColor = Color.FromArgb(21, 87, 36);
    }

    // ═══════════════════════════════════════════════
    // الحفظ / الإغلاق / اختصارات لوحة المفاتيح
    // ═══════════════════════════════════════════════
    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("السلة فارغة. امسح صنفاً واحداً على الأقل.", "حفظ واعتماد",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            FocusScanner();
            return;
        }

        MessageBox.Show($"تم اعتماد الفاتورة بنجاح (وضع تجريبي) — الصافي {lblNetTotalVal.Text}.",
            "حفظ واعتماد", MessageBoxButtons.OK, MessageBoxIcon.Information);

        _cart.Clear();
        RecalcTotals();
        lblLastScanInfo.Text = "—";
        FocusScanner();
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void frmSalesContinuousScanner_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.F2:
                FocusScanner();
                e.Handled = true;
                break;
            case Keys.F9:
                btnSave.PerformClick();
                e.Handled = true;
                break;
            case Keys.Delete when dgvCart.CurrentRow?.DataBoundItem is CartLine line:
                _cart.Remove(line);
                RenumberLines();
                RecalcTotals();
                e.Handled = true;
                break;
        }
    }

    // ═══════════════════════════════════════════════
    // سطر السلة (إعلام بالتغيير لتنعيم تحديث الشبكة)
    // ═══════════════════════════════════════════════
    private sealed class CartLine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _lineNo;
        private string _productName = string.Empty;
        private string _barcode = string.Empty;
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

        public string Barcode
        {
            get => _barcode;
            set { _barcode = value; OnPropertyChanged(nameof(Barcode)); }
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