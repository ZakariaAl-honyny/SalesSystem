using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// حوار «معلومات صنف مشتريات» — نموذج واجهة (UI Shape) يعمل ببيانات وهمية في الذاكرة
/// دون أي اتصال بقاعدة البيانات (وضع التجربة / UI-First).
/// يعرض: بيانات الصنف · بطاقات أسعار الشراء المقترحة (متوسط/آخر/أدنى) · سجل المشتريات السابقة.
/// <para>RULE-N40…RULE-N47 / RULE-N50…RULE-N54 — التنقل والثيم الموحّد.</para>
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

        // RULE-N40…RULE-N47 + RULE-N95 — الستة من KeyboardHelper.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnClose);                         // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.Escape, () => DialogResult = DialogResult.Cancel));

        ApplyUiTheme(); // RULE-N50

        InitializeGridColumns();
        LoadMockData();
    }

    // ═══════════════════════════════════════════════════════════
    // الثيم (RULE-N50…RULE-N54)
    // ═══════════════════════════════════════════════════════════

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply</c> آخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        // ThemeHelper يفرض Segoe UI ويوسّع الأزرار ⇒ نثبّت Tahoma والأحجام (RULE-N21).
        btnClose.AutoSize = false;
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnClose.Size = new Size(120, 38);
        btnClose.ForeColor = Color.White;

        toolTip.SetToolTip(btnClose, "إغلاق نافذة معلومات الصنف (Esc)");
        toolTip.SetToolTip(dgvHistory, "سجل المشتريات السابقة — زر الفأرة الأيمن للتصدير أو التحديث");

        GreenThemeHelper.ApplyHeader(lblTitle);
        GreenThemeHelper.ApplyGrid(dgvHistory);

        // RULE-N22/RULE-N52 — سلوك شبكة العرض تعلنه الشاشة نفسها.
        dgvHistory.ReadOnly = true;
        dgvHistory.AllowUserToAddRows = false;
        dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvHistory.MultiSelect = false;

        emptyState.BringToFront();

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── تحميل البيانات التجريبية (Mock) ──────────────────────────────────────

    private void LoadMockData()
    {
        // رأس الحوار: اسم الصنف + الباركود + التصنيف + الرصيد الحالي
        lblProductName.Text = MockItem.Name;
        lblBarcodeValue.Text = MockItem.Barcode;
        lblCategoryValue.Text = MockItem.Category;
        lblStockValue.Text = $"{clsFormat.Quantity(MockItem.CurrentStock)} {MockItem.Unit}";

        // سجل المشتريات (مرتب زمنياً)
        var ordered = MockHistory.OrderBy(p => p.Date).ToList();

        dgvHistory.Rows.Clear();
        foreach (var purchase in ordered)
        {
            dgvHistory.Rows.Add(
                clsFormat.DateToDayMonthYear(purchase.Date),
                purchase.Supplier,
                clsFormat.Quantity(purchase.Quantity),
                clsFormat.Money(purchase.UnitCost),
                clsFormat.Money(purchase.Total));
        }

        lblRecordsCount.Text = clsFormat.Number(ordered.Count);

        // RULE-N23 — سطر «لا توجد بيانات» عند خلوّ الشبكة.
        emptyState.Visible = ordered.Count == 0;
        if (emptyState.Visible) emptyState.BringToFront();

        // بطاقات أسعار الشراء المقترحة — محسوبة من سجل المشتريات
        decimal totalQuantity = ordered.Sum(p => p.Quantity);
        decimal totalCost = ordered.Sum(p => p.Total);

        decimal averageCost = totalQuantity > 0 ? totalCost / totalQuantity : 0m;  // متوسط مرجح بالكمية
        decimal lastCost = ordered.Last().UnitCost;                                 // أحدث سعر شراء
        decimal minCost = ordered.Min(p => p.UnitCost);                             // أدنى سعر شراء

        cardAvgValue.Text = clsFormat.Money(averageCost);
        cardLastValue.Text = clsFormat.Money(lastCost);
        cardMinValue.Text = clsFormat.Money(minCost);
    }

    // ── تنسيق الشبكة: إبراز عمود الإجمالي وأعمدة الكميات/التكلفة ─────────────

    private void dgvHistory_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvHistory.Rows.Count) return;
        if (e.ColumnIndex < 0 || e.ColumnIndex >= dgvHistory.Columns.Count) return;

        switch (dgvHistory.Columns[e.ColumnIndex].Name)
        {
            case "ColQty":
            case "ColUnitCost":
                e.CellStyle.Font = new Font(dgvHistory.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = GreenThemeHelper.TextStrong;
                break;

            case "ColTotal":
                e.CellStyle.Font = new Font(dgvHistory.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = ThemeHelper.Primary;
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

    /// <summary>نقر مزدوج على سطر: عرض تفاصيل عملية الشراء (قراءة فقط).</summary>
    private void dgvHistory_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        try
        {
            var row = dgvHistory.Rows[e.RowIndex];

            clsDialogs.Info(
                $"التاريخ: {row.Cells["ColDate"].Value}\n" +
                $"المورد: {row.Cells["ColSupplier"].Value}\n" +
                $"الكمية: {row.Cells["ColQty"].Value}\n" +
                $"تكلفة الوحدة: {row.Cells["ColUnitCost"].Value}\n" +
                $"الإجمالي: {row.Cells["ColTotal"].Value}");
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر عرض تفاصيل عملية شراء سابقة");
        }
    }

    /// <summary>تصدير سجل المشتريات المعروض إلى Excel عبر <see cref="ExportHelper"/> (RULE-N112).</summary>
    private void tsmiExport_Click(object? sender, EventArgs e)
    {
        try
        {
            if (dgvHistory.Rows.Count == 0)
            {
                clsDialogs.Warning("لا توجد بيانات معروضة للتصدير.");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"سجل_مشتريات_الصنف_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير سجل المشتريات إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvHistory, "سجل المشتريات", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "خطأ أثناء تصدير سجل مشتريات الصنف إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    /// <summary>تحديث بيانات الحوار (وضع التجربة: إعادة بناء البيانات الوهمية).</summary>
    private void tsmiRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            LoadMockData();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحديث بيانات معلومات الصنف");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
