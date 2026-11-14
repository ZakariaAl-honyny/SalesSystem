using System.Globalization;
using SalesSystem.DTO;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Returns;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

public partial class frmListReturns : Form
{
    private const string CashCustomerName = "عميل نقدي";

    private readonly IServiceProvider _serviceProvider;
    private ISalesReturnDesktopService _salesReturnService => _serviceProvider.GetRequiredService<ISalesReturnDesktopService>();
    private IPurchaseReturnDesktopService _purchaseReturnService => _serviceProvider.GetRequiredService<IPurchaseReturnDesktopService>();

    // ── Data Stores ──────────────────────────────────────────────────────────
    private List<SalesReturnDto> _allSalesReturns = new();
    private List<SalesReturnDto> _filteredSalesReturns = new();
    private List<PurchaseReturnDto> _allPurchaseReturns = new();
    private List<PurchaseReturnDto> _filteredPurchaseReturns = new();

    /// <summary>يمنع تطبيق التصفية أثناء تهيئة أدوات `gbFilter`.</summary>
    private bool _isInitializing = true;

    // ── الحدود الافتراضية للتصفية (RULE-N148) ─────────────────────────────────
    // تُلتقط مرة واحدة عند الإنشاء، وهي مرجع زر «مسح التصفية» لمعرفة وجود تصفية نشطة.
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    public frmListReturns(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        // RULE-N148 — المدى الافتراضي «هذا الشهر» (يُلتقط قبل بناء الشاشة).
        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل وتحديد وتظليل الحقل النشط
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));
        KeyboardHelper.WireArrowNavigation(this);

        KeyboardHelper.WireButtonDisabledVisual(btnAdd, btnRefresh, btnClose, btnClearFilters);   // RULE-N43

        KeyboardHelper.WireGlobalShortcuts(this,                                  // RULE-N47
            (Keys.F2, () => btnAdd.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, () => { txtFilterValue.Clear(); _ApplyFilter(); }),
            (Keys.Escape, _GlobalEscape));

        // RULE-N147 — Enter = الزر الأساسي للشاشة («جديد») وخارج حقول الإدخال.
        KeyboardHelper.WireModeEnter(this, () => btnAdd);
        AcceptButton = btnAdd;

        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    /// <summary>RULE-N47 + RULE-N59: Esc داخل صندوق التصفية = مسحها، وخارجه = إغلاق الشاشة.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ResetFilters();
            return;
        }

        Close();
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — أخيراً بعد ThemeHelper.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);

        GreenThemeHelper.ApplyGrid(dgvSales);
        GreenThemeHelper.ApplyGrid(dgvPurchases);

        // RULE-N22 — الشبكتان للعرض فقط؛ نُعيد تأكيد سلوكهما (المساعد لا يلمس هذه الخصائص).
        dgvSales.ReadOnly = true;
        dgvSales.AllowUserToAddRows = false;
        dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSales.MultiSelect = false;

        dgvPurchases.ReadOnly = true;
        dgvPurchases.AllowUserToAddRows = false;
        dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPurchases.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير (RULE-N50)

        // RULE-N53/N54 — نص شريط الملخص من ثوابت المساعد، لا أرقام داخل الشاشة.
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblCountCaption.ForeColor = GreenThemeHelper.TextStrong;
        lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;
    }

    // ── Filter Initialization (RULE-N57/N58/N59) ──────────────────────────────

    /// <summary>القيم الابتدائية: بلا نص، الحالة «الكل»، والمدى «هذا الشهر».</summary>
    private void _InitializeFilters()
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;      // العميل/المورد
            cbStatus.SelectedIndex = 0;        // الكل
            chkOnlyCancelled.Checked = false;
            txtFilterValue.Clear();

            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = false;
        }
    }

    /// <summary>Esc أو «مسح التصفية» — يمسح كل أدوات التصفية ويعيد «هذا الشهر» (RULE-N59/N148).</summary>
    private void _ResetFilters()
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    private void _SetDateRange(DateTime from, DateTime to)
    {
        _isInitializing = true;
        try
        {
            dtpFrom.Value = from;
            dtpTo.Value = to;
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyFilter();
    }

    // ── Data Loading ─────────────────────────────────────────────────────────

    private async void _RefreshReturns()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var salesResult = await _salesReturnService.GetAllAsync();
            if (salesResult.IsSuccess && salesResult.Value != null)
            {
                _allSalesReturns = salesResult.Value.ToList();
            }
            else
            {
                if (!salesResult.IsSuccess)
                {
                    clsDialogs.Error(salesResult.Error ?? "حدث خطأ في تحميل مرتجعات المبيعات");
                }
                _allSalesReturns = new List<SalesReturnDto>();
            }

            var purchaseResult = await _purchaseReturnService.GetAllAsync();
            if (purchaseResult.IsSuccess && purchaseResult.Value != null)
            {
                _allPurchaseReturns = purchaseResult.Value.ToList();
            }
            else
            {
                if (!purchaseResult.IsSuccess)
                {
                    clsDialogs.Error(purchaseResult.Error ?? "حدث خطأ في تحميل مرتجعات المشتريات");
                }
                _allPurchaseReturns = new List<PurchaseReturnDto>();
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "حدث خطأ أثناء تحميل المرتجعات");
            clsDialogs.Error("حدث خطأ أثناء تحميل المرتجعات");
            _allSalesReturns = new List<SalesReturnDto>();
            _allPurchaseReturns = new List<PurchaseReturnDto>();
        }
        finally
        {
            _ApplyFilter();
            Cursor = Cursors.Default;
        }
    }

    // ── Filtering (RULE-N59/N60/N61) ──────────────────────────────────────────

    /// <summary>
    /// تصفية فورية واحدة تُطبَّق على الشبكتين معاً (مرتجع مبيعات + مرتجع مشتريات):
    /// المدى الزمني ثم الحالة ثم «الملغاة فقط» ثم النص حسب نوع التصفية. لا يوجد زر بحث.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing)
            return;

        if (dgvSales.Columns.Count == 0 || dgvPurchases.Columns.Count == 0)
            return;

        var fromDate = dtpFrom.Value.Date;
        var toDate = dtpTo.Value.Date;
        if (toDate < fromDate)
            (fromDate, toDate) = (toDate, fromDate);

        // (1) المدى الزمني — لكل شبكة حسب نوع تاريخها (PurchaseReturnDto.ReturnDate = DateOnly).
        IEnumerable<SalesReturnDto> sales = _allSalesReturns
            .Where(r => r.ReturnDate.Date >= fromDate && r.ReturnDate.Date <= toDate);

        IEnumerable<PurchaseReturnDto> purchases = _allPurchaseReturns
            .Where(r =>
            {
                var d = r.ReturnDate.ToDateTime(TimeOnly.MinValue).Date;
                return d >= fromDate && d <= toDate;
            });

        // (2) الحالة — بالـ SelectedIndex لا بنص عربي (RULE-N60): 0 الكل · 1 مرحّل(2) · 2 ملغى(3).
        int statusIndex = cbStatus.SelectedIndex;
        if (statusIndex == 1)
        {
            sales = sales.Where(r => r.Status == 2);
            purchases = purchases.Where(r => r.Status == 2);
        }
        else if (statusIndex == 2)
        {
            sales = sales.Where(r => r.Status == 3);
            purchases = purchases.Where(r => r.Status == 3);
        }

        // (3) «الملغاة فقط» (RULE-N58).
        if (chkOnlyCancelled.Checked)
        {
            sales = sales.Where(r => r.Status == 3);
            purchases = purchases.Where(r => r.Status == 3);
        }

        // (4) النص حسب نوع التصفية (RULE-N60 — أعمدة معروفة فقط).
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 0: // العميل / المورد
                    sales = sales.Where(r => _CustomerText(r).Contains(text, StringComparison.OrdinalIgnoreCase));
                    purchases = purchases.Where(r => (r.SupplierName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase));
                    break;

                case 1: // رقم الفاتورة (رقم المرتجع)
                    sales = sales.Where(r => r.ReturnNo.ToString(CultureInfo.InvariantCulture).StartsWith(text, StringComparison.Ordinal));
                    purchases = purchases.Where(r => r.ReturnNo.ToString(CultureInfo.InvariantCulture).StartsWith(text, StringComparison.Ordinal));
                    break;

                case 2: // البيان
                    sales = sales.Where(r =>
                        (r.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase) ||
                        (r.ReturnReason ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase));
                    purchases = purchases.Where(r => (r.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase));
                    break;

                case 3: // المبلغ
                    sales = sales.Where(r => r.TotalAmount.ToString("0.##", CultureInfo.InvariantCulture).Contains(text, StringComparison.Ordinal));
                    purchases = purchases.Where(r => r.TotalAmount.ToString("0.##", CultureInfo.InvariantCulture).Contains(text, StringComparison.Ordinal));
                    break;
            }
        }

        _filteredSalesReturns = sales.ToList();
        _filteredPurchaseReturns = purchases.ToList();

        _PopulateSalesGrid();
        _PopulatePurchaseGrid();
        _UpdateSummary();

        _UpdateClearFiltersState();   // RULE-N148: حالة زر «مسح التصفية» تتبع وجود تصفية نشطة
    }

    // ── مسح التصفية (RULE-N148) ────────────────────────────────────────────────

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
        => _ResetFilters();

    /// <summary>تفريغ أدوات التصفية وحدها إلى قيمها الافتراضية (RULE-N148).</summary>
    private void _ClearFilterInputs()
        => _InitializeFilters();

    /// <summary>يمسح كل أدوات التصفية ثم يعيد تطبيقها على البيانات المحمّلة — بلا إعادة تحميل (RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    /// <summary>RULE-N43/N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية مختلفة عن الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        if (btnClearFilters is null) return;

        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyCancelled.Checked
            || dtpFrom.Value.Date != _defaultFrom
            || dtpTo.Value.Date != _defaultTo;
    }

    private static string _CustomerText(SalesReturnDto r)
        => string.IsNullOrWhiteSpace(r.CustomerName) ? CashCustomerName : r.CustomerName;

    private void _PopulateSalesGrid()
    {
        dgvSales.Rows.Clear();

        foreach (var r in _filteredSalesReturns)
        {
            dgvSales.Rows.Add(
                r.ReturnNo,
                r.ReturnDate.ToString("yyyy-MM-dd"),
                _CustomerText(r),
                clsFormat.Money(r.TotalAmount),
                r.StatusDisplay
            );
        }

        lblNoSales.Visible = _filteredSalesReturns.Count == 0;
        if (lblNoSales.Visible)
            lblNoSales.BringToFront();
    }

    private void _PopulatePurchaseGrid()
    {
        dgvPurchases.Rows.Clear();

        foreach (var r in _filteredPurchaseReturns)
        {
            dgvPurchases.Rows.Add(
                r.ReturnNo,
                r.ReturnDate.ToString("yyyy-MM-dd"),
                r.SupplierName,
                clsFormat.Money(r.TotalAmount),
                r.StatusDisplay
            );
        }

        lblNoPurchases.Visible = _filteredPurchaseReturns.Count == 0;
        if (lblNoPurchases.Visible)
            lblNoPurchases.BringToFront();
    }

    private void _ReloadForm()
    {
        _SetupSalesGrid();
        _SetupPurchaseGrid();
        _RefreshReturns();
    }

    private void _UpdateRecordsCount()
    {
        int total = _filteredSalesReturns.Count + _filteredPurchaseReturns.Count;
        lblRecordsCount.Text = total.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>RULE-N61 — «المعروض: N من M — مرتجع مبيعات: X N2 · مرتجع مشتريات: Y N2».</summary>
    private void _UpdateSummary()
    {
        int shown = _filteredSalesReturns.Count + _filteredPurchaseReturns.Count;
        int total = _allSalesReturns.Count + _allPurchaseReturns.Count;

        decimal salesTotal = _filteredSalesReturns.Sum(r => r.TotalAmount);
        decimal purchaseTotal = _filteredPurchaseReturns.Sum(r => r.TotalAmount);

        lblSummary.Text =
            $"المعروض: {shown} من {total} — مرتجع مبيعات: {salesTotal.ToString("N2", CultureInfo.CurrentCulture)} · مرتجع مشتريات: {purchaseTotal.ToString("N2", CultureInfo.CurrentCulture)}";

        _UpdateRecordsCount();
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupSalesGrid()
    {
        dgvSales.Columns.Clear();

        dgvSales.Columns.Add("ReturnNo", "رقم المرتجع");
        dgvSales.Columns.Add("ReturnDate", "التاريخ");
        dgvSales.Columns.Add("CustomerName", "العميل");
        dgvSales.Columns.Add("TotalAmount", "الإجمالي");
        dgvSales.Columns.Add("StatusDisplay", "الحالة");

        _ApplyColumnWeights(dgvSales, 14F, 14F, 40F, 16F, 16F);
    }

    private void _SetupPurchaseGrid()
    {
        dgvPurchases.Columns.Clear();

        dgvPurchases.Columns.Add("ReturnNo", "رقم المرتجع");
        dgvPurchases.Columns.Add("ReturnDate", "التاريخ");
        dgvPurchases.Columns.Add("SupplierName", "المورد");
        dgvPurchases.Columns.Add("TotalAmount", "الإجمالي");
        dgvPurchases.Columns.Add("StatusDisplay", "الحالة");

        _ApplyColumnWeights(dgvPurchases, 14F, 14F, 40F, 16F, 16F);
    }

    /// <summary>
    /// أوزان الأعمدة (RULE-N22: عرض فقط) — تُضبط دائماً حتى قبل تعبئة أي سطر،
    /// لأن <c>AutoSizeColumnsMode.Fill</c> من <c>GreenThemeHelper.ApplyGrid</c> يوزّع بالوزن لا بالعرض.
    /// </summary>
    private static void _ApplyColumnWeights(DataGridView grid, params float[] weights)
    {
        int count = Math.Min(grid.Columns.Count, weights.Length);

        for (int i = 0; i < count; i++)
        {
            var column = grid.Columns[i];
            column.FillWeight = weights[i];
            column.MinimumWidth = 100;
            column.ReadOnly = true;
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }

    // ── Form Events ──────────────────────────────────────────────────────────

    private void frmListReturns_Load(object sender, EventArgs e)
    {
        ApplyGreenTheme();
        _InitializeFilters();
        _UpdateClearFiltersState();   // RULE-N148 — الشاشة تفتح بلا تصفية نشطة
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── Toolbar Events ─────────────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        int tabIndex = tabReturns.SelectedIndex;
        if (tabIndex == 0)
            OpenSalesReturnForm(null);
        else
            OpenPurchaseReturnForm(null);
    }

    /// <summary>
    /// RULE-N148 — «تحديث» إعادة تحميل كاملة: تفريغ التصفية ← إعادة تحميل بيانات الشاشة ←
    /// إعادة بناء الشبكة والملخص وتحديد الصف. ليس «مسح تصفية» فقط.
    /// </summary>
    private void btnRefresh_Click(object sender, EventArgs e)
    {
        _ClearFilterInputs();   // التصفية تعود للافتراضي
        _ReloadForm();          // الشبكة + الملخص + التحديد
    }

    // ── Filter Events (تصفية فورية — RULE-N59) ────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    /// <summary>Enter يطبّق التصفية فوراً (RULE-N59).</summary>
    private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
            return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void chkOnlyCancelled_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object sender, EventArgs e)
    {
        var today = DateTime.Today;
        _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    // ── Grid Events ─────────────────────────────────────────────────────────

    private void dgvSales_DoubleClick(object sender, EventArgs e)
    {
        if (dgvSales.CurrentRow == null) return;
        int index = dgvSales.CurrentRow.Index;
        if (index < 0 || index >= _filteredSalesReturns.Count) return;
        OpenSalesReturnForm(_filteredSalesReturns[index]);
    }

    private void dgvPurchases_DoubleClick(object sender, EventArgs e)
    {
        if (dgvPurchases.CurrentRow == null) return;
        int index = dgvPurchases.CurrentRow.Index;
        if (index < 0 || index >= _filteredPurchaseReturns.Count) return;
        OpenPurchaseReturnForm(_filteredPurchaseReturns[index]);
    }

    // ── Context Menu Events (RULE-N23) ───────────────────────────────────────

    private void tsmiOpen_Click(object sender, EventArgs e)
    {
        if (tabReturns.SelectedIndex == 0)
        {
            if (dgvSales.CurrentRow != null)
                dgvSales_DoubleClick(sender, e);
        }
        else
        {
            if (dgvPurchases.CurrentRow != null)
                dgvPurchases_DoubleClick(sender, e);
        }
    }

    private void tsmiRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    // ── Helpers ──────────────────────────────────────────────────────────────

    private void OpenSalesReturnForm(SalesReturnDto? dto)
    {
        var form = _serviceProvider.GetRequiredService<frmSalesReturn>();
        form.LoadData(dto);
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private void OpenPurchaseReturnForm(PurchaseReturnDto? dto)
    {
        var form = _serviceProvider.GetRequiredService<frmPurchaseReturn>();
        form.LoadData(dto);
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }
}
