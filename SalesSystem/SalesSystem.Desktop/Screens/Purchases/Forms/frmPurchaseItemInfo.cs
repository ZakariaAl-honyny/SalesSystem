namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// نافذة "معلومات صنف مشتريات" — نموذج واجهة (UI Shape) يعمل ببيانات وهمية في الذاكرة
/// دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تُعرض عبر ShowDialog وتعرض: بيانات الصنف، بطاقات أسعار الشراء المقترحة
/// (متوسط/آخر/أدنى — محسوبة من سجل المشتريات)، وسجل المشتريات السابقة للصنف.
/// </summary>
public partial class frmPurchaseItemInfo : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── بيانات تجريبية (Mock) — صنف المشتريات وسجل شرائه ─────────────────────

    private sealed record MockProduct(string Name, string Barcode, string Category, decimal CurrentStock, string Unit);

    private sealed record MockPurchase(DateTime Date, string Supplier, decimal Quantity, decimal UnitCost)
    {
        public decimal Total => Quantity * UnitCost;   // الإجمالي = الكمية × تكلفة الوحدة
    }

    private static readonly MockProduct MockItem = new(
        Name: "زيت نباتي 1 لتر",
        Barcode: "102031",
        Category: "زيوت ومواد غذائية",
        CurrentStock: 48.000m,          // رصيد كمي (3 خانات عشرية)
        Unit: "عبوة");

    private static readonly MockPurchase[] MockHistory =
    {
        new(new DateTime(2026, 8, 2),  "شركة النور للتجارة",         100.000m, 82.50m),
        new(new DateTime(2026, 8, 18), "مؤسسة الرافدين الغذائية",     50.000m, 86.00m),
        new(new DateTime(2026, 9, 1),  "شركة النور للتجارة",         120.000m, 80.00m),
        new(new DateTime(2026, 9, 9),  "مؤسسة الرافدين الغذائية",     40.000m, 74.00m),
        new(new DateTime(2026, 9, 15), "شركة البركة للمواد الغذائية",  80.000m, 78.50m),
    };

    public frmPurchaseItemInfo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        InitializeGridColumns();
        LoadMockData();
    }

    // ── تحميل البيانات التجريبية (Mock) ──────────────────────────────────────

    private void LoadMockData()
    {
        // رأس النافذة: اسم الصنف + الباركود + التصنيف + الرصيد الحالي
        lblProductName.Text = MockItem.Name;
        lblBarcodeValue.Text = MockItem.Barcode;
        lblCategoryValue.Text = MockItem.Category;
        lblStockValue.Text = $"{MockItem.CurrentStock:N3} {MockItem.Unit}";

        // سجل المشتريات (مرتب زمنياً)
        var ordered = MockHistory.OrderBy(p => p.Date).ToList();

        dgvHistory.Rows.Clear();
        foreach (var purchase in ordered)
        {
            dgvHistory.Rows.Add(
                purchase.Date.ToString("dd/MM/yyyy"),
                purchase.Supplier,
                purchase.Quantity.ToString("N3"),
                purchase.UnitCost.ToString("N2"),
                purchase.Total.ToString("N2"));
        }

        lblRecordsCount.Text = ordered.Count.ToString();

        // بطاقات أسعار الشراء المقترحة — محسوبة من سجل المشتريات
        decimal totalQuantity = ordered.Sum(p => p.Quantity);
        decimal totalCost = ordered.Sum(p => p.Total);

        decimal averageCost = totalQuantity > 0 ? totalCost / totalQuantity : 0m;  // متوسط مرجح بالكمية
        decimal lastCost = ordered.Last().UnitCost;                                 // أحدث سعر شراء
        decimal minCost = ordered.Min(p => p.UnitCost);                             // أدنى سعر شراء

        cardAvgValue.Text = averageCost.ToString("N2");
        cardLastValue.Text = lastCost.ToString("N2");
        cardMinValue.Text = minCost.ToString("N2");
    }

    // ── تنسيق الشبكة: إبراز عمود الإجمالي وأعمدة الكميات/التكلفة ─────────────

    private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvHistory.Rows.Count) return;

        switch (dgvHistory.Columns[e.ColumnIndex].Name)
        {
            case "ColQty":
            case "ColUnitCost":
                e.CellStyle.Font = new Font(dgvHistory.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(52, 73, 94);
                break;

            case "ColTotal":
                e.CellStyle.Font = new Font(dgvHistory.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.FromArgb(41, 128, 185);   // أزرق للمبالغ
                break;
        }
    }

    // ── إعداد أعمدة الشبكة ───────────────────────────────────────────────────

    private void InitializeGridColumns()
    {
        dgvHistory.AutoGenerateColumns = false;
        dgvHistory.Columns.Clear();

        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDate",     HeaderText = "التاريخ",       FillWeight = 16, MinimumWidth = 90,  ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColSupplier", HeaderText = "المورد",        FillWeight = 32, MinimumWidth = 140, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty",      HeaderText = "الكمية",        FillWeight = 16, MinimumWidth = 90,  ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColUnitCost", HeaderText = "تكلفة الوحدة",  FillWeight = 18, MinimumWidth = 100, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal",    HeaderText = "الإجمالي",     FillWeight = 18, MinimumWidth = 100, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
    }

    // ── أحداث الواجهة ────────────────────────────────────────────────────────

    private void btnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}