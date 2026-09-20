using System.Data;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة «توزيع الأرباح» — عرض تفصيلي لربح كل صنف في فاتورة البيع مع إجماليات الفاتورة.
/// نموذج أولي (UI-first prototype) ببيانات تجريبية داخل الذاكرة، بدون أي اعتماد على API أو قاعدة بيانات.
/// تُعرض كـ modal عبر ShowDialog().
/// </summary>
public partial class frmSalesProfitBreakdown : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════
    /// <summary>بند ربح داخل الفاتورة — تُحسب كل المبالغ تلقائياً من (السعر - التكلفة) × الكمية.</summary>
    private sealed class MockProfitLine
    {
        public string Product { get; init; } = "";
        public string Unit { get; init; } = "";
        public decimal Quantity { get; init; }     // الكمية
        public decimal UnitCost { get; init; }     // تكلفة الوحدة
        public decimal UnitPrice { get; init; }    // سعر البيع للوحدة

        /// <summary>إجمالي تكلفة البند = الكمية × تكلفة الوحدة.</summary>
        public decimal LineCost => Quantity * UnitCost;

        /// <summary>إجمالي مبيعات البند = الكمية × سعر البيع.</summary>
        public decimal LineSales => Quantity * UnitPrice;

        /// <summary>ربح البند = المبيعات − التكلفة.</summary>
        public decimal LineProfit => LineSales - LineCost;

        /// <summary>هامش ربح البند % = الربح ÷ المبيعات.</summary>
        public decimal MarginPercent => LineSales == 0m ? 0m : LineProfit / LineSales * 100m;
    }

    /// <summary>فاتورة بيع تجريبية واحدة (5 أصناف): (الصنف، الوحدة، الكمية، التكلفة، سعر البيع).</summary>
    private static readonly (string Product, string Unit, decimal Qty, decimal Cost, decimal Price)[] MockInvoiceLines =
    {
        ("سكر أبيض 50 كجم",   "كيس",   10, 15000m, 17000m),
        ("أرز بسمتي 25 كجم",  "كيس",    8, 22000m, 24500m),
        ("زيت ذرة 5 لتر",     "عبوة",  20,  4200m,  4900m),
        ("دقيق قمح 10 كجم",   "كيس",   15,  6500m,  7200m),
        ("شاي أخضر 1 كجم",    "عبوة",  25,  2800m,  3400m),
    };

    private readonly List<MockProfitLine> _lines = new();

    // مراجع قيم بطاقات الملخص العلوية (تُبنى في الكود — تُحدَّث عند حساب الإجماليات)
    private Label? _lblCostVal;
    private Label? _lblSalesVal;
    private Label? _lblProfitVal;
    private Label? _lblMarginVal;

    public frmSalesProfitBreakdown(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _ = serviceProvider;
        InitializeComponent();
        BuildMockData();
        BuildMetricCards();
        BindGrid();
        UpdateTotals();
    }

    // ═══════════════════════════════════════════════
    // بناء البيانات التجريبية
    // ═══════════════════════════════════════════════
    private void BuildMockData()
    {
        _lines.Clear();
        foreach (var (product, unit, qty, cost, price) in MockInvoiceLines)
        {
            _lines.Add(new MockProfitLine
            {
                Product = product,
                Unit = unit,
                Quantity = qty,
                UnitCost = cost,
                UnitPrice = price
            });
        }
    }

    // ═══════════════════════════════════════════════
    // بطاقات الملخص العلوية (3 بطاقات)
    // ═══════════════════════════════════════════════
    /// <summary>بناء بطاقة ملخص واحدة: عنوان أعلى + قيمة كبيرة في المنتصف (+ سطر فرعي اختياري أسفل).</summary>
    private static Panel BuildMetricCard(string title, Color valueColor, string? subText = null)
    {
        var card = new Panel
        {
            Width = 200,
            Height = 88,
            Margin = new Padding(0, 0, 6, 0),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblTitle = new Label
        {
            Dock = DockStyle.Top,
            Height = 22,
            Text = title,
            Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = Color.FromArgb(100, 110, 120),
            TextAlign = ContentAlignment.MiddleCenter
        };

        var lblValue = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = valueColor,
            TextAlign = ContentAlignment.MiddleCenter
        };

        // ترتيب البطاقة: العنوان أعلى، القيمة تملأ المنتصف، والسطر الفرعي أسفل (إن وُجد)
        card.Controls.Add(lblValue);
        if (subText != null)
        {
            var lblSub = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                Text = subText,
                Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = valueColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblSub);
        }
        card.Controls.Add(lblTitle);

        return card;
    }

    private void BuildMetricCards()
    {
        flpCards.Controls.Clear();

        var costCard = BuildMetricCard("إجمالي التكلفة", Color.FromArgb(108, 117, 125));
        var salesCard = BuildMetricCard("صافي المبيعات", Color.FromArgb(13, 110, 253));
        var profitCard = BuildMetricCard("صافي الربح", Color.FromArgb(21, 87, 36), "هامش الربح: 0.00%");

        flpCards.Controls.Add(costCard);
        flpCards.Controls.Add(salesCard);
        flpCards.Controls.Add(profitCard);

        // الاحتفاظ بمراجع قيم البطاقات لتحديثها عند حساب الإجماليات
        _lblCostVal = FindCardValueLabel(costCard);
        _lblSalesVal = FindCardValueLabel(salesCard);
        _lblProfitVal = FindCardValueLabel(profitCard);
        _lblMarginVal = FindCardSubLabel(profitCard);

        tip.SetToolTip(costCard, "إجمالي تكلفة شراء الأصناف المباعة في الفاتورة");
        tip.SetToolTip(salesCard, "إجمالي قيمة مبيعات الفاتورة قبل خصم التكاليف");
        tip.SetToolTip(profitCard, "صافي الربح بعد خصم التكاليف — مع هامش الربح %");
    }

    /// <summary>إيجاد تسمية القيمة الرئيسية داخل البطاقة (التي تملأ المنتصف).</summary>
    private static Label FindCardValueLabel(Panel card)
        => card.Controls.OfType<Label>().First(l => l.Dock == DockStyle.Fill);

    /// <summary>إيجاد السطر الفرعي أسفل البطاقة (إن وُجد).</summary>
    private static Label? FindCardSubLabel(Panel card)
        => card.Controls.OfType<Label>().FirstOrDefault(l => l.Dock == DockStyle.Bottom);

    // ═══════════════════════════════════════════════
    // تعبئة الشبكة + الإجماليات
    // ═══════════════════════════════════════════════
    private void BindGrid()
    {
        var table = new DataTable();
        table.Columns.Add("Item", typeof(string));
        table.Columns.Add("Qty", typeof(decimal));
        table.Columns.Add("Cost", typeof(decimal));
        table.Columns.Add("Price", typeof(decimal));
        table.Columns.Add("Profit", typeof(decimal));
        table.Columns.Add("Margin", typeof(decimal));

        foreach (var line in _lines)
        {
            table.Rows.Add(
                $"{line.Product} — {line.Unit}",
                line.Quantity,
                line.LineCost,
                line.LineSales,
                line.LineProfit,
                line.MarginPercent);
        }

        dgvProfit.AutoGenerateColumns = false;
        dgvProfit.DataSource = table;
    }

    /// <summary>حساب إجماليات الفاتورة وتحديث البطاقات العلوية وشريط الإجماليات السفلي.</summary>
    private void UpdateTotals()
    {
        decimal totalCost = 0m;
        decimal totalSales = 0m;
        foreach (var line in _lines)
        {
            totalCost += line.LineCost;
            totalSales += line.LineSales;
        }

        decimal totalProfit = totalSales - totalCost;
        decimal margin = totalSales == 0m ? 0m : totalProfit / totalSales * 100m;

        // بطاقات الملخص العلوية
        if (_lblCostVal != null) _lblCostVal.Text = totalCost.ToString("N2");
        if (_lblSalesVal != null) _lblSalesVal.Text = totalSales.ToString("N2");
        if (_lblProfitVal != null) _lblProfitVal.Text = totalProfit.ToString("N2");
        if (_lblMarginVal != null) _lblMarginVal.Text = $"هامش الربح: {margin:0.##}%";

        // شريط الإجماليات السفلي
        lblTotalSalesVal.Text = totalSales.ToString("N2");
        lblTotalCostVal.Text = totalCost.ToString("N2");
        lblTotalProfitVal.Text = totalProfit.ToString("N2");
    }

    // ═══════════════════════════════════════════════
    // الأحداث
    // ═══════════════════════════════════════════════
    /// <summary>تنسيق عمود «الهامش %» — إضافة علامة النسبة المئوية إلى القيمة.</summary>
    private void dgvProfit_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex == colMargin.Index && e.RowIndex >= 0 && e.Value is decimal margin)
        {
            e.Value = $"{margin:0.##} %";
            e.FormattingApplied = true;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}