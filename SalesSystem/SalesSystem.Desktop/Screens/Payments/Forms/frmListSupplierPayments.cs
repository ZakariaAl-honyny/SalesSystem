using System.Data;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Payments;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms;

public partial class frmListSupplierPayments : Form
{
    /// <summary>لون تظليل الحقل النشط (RULE-N46).</summary>
    private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

    private readonly IServiceProvider _serviceProvider;

    /// <summary>جدول البيانات الكامل (بدون تصفية) — أساس شريط الملخص «من M» (RULE-N61).</summary>
    private DataTable _supplierTable;

    /// <summary>العرض المصفّى المرتبط بالشبكة (RULE-N24 — DataView.RowFilter).</summary>
    private DataView _supplierView;

    /// <summary>يمنع انطلاق التصفية قبل اكتمال تهيئة أدواتها (RULE-N59 — التصفية فورية بعد التهيئة).</summary>
    private bool _isInitializing = true;

    /// <summary>«من» الافتراضي الذي يعيد إليه «مسح التصفية» التاريخ (RULE-N148) — يُلتقط عند البناء.</summary>
    private DateTime _defaultFrom;

    /// <summary>«إلى» الافتراضي الذي يعيد إليه «مسح التصفية» التاريخ (RULE-N148) — يُلتقط عند البناء.</summary>
    private DateTime _defaultTo;

    public frmListSupplierPayments(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        _supplierTable = new DataTable();
        _supplierView = new DataView();

        // النطاق الافتراضي لأدوات التصفية — أساس زر «مسح التصفية» (RULE-N148).
        _defaultFrom = dtpFrom.Value.Date;
        _defaultTo = dtpTo.Value.Date;

        // RULE-N40 / N41 / N46 / N45 — التنقل والتحديد وتظليل الحقل النشط وأسهم RTL
        KeyboardHelper.WireEnterAsTab(this);                                              // RULE-N40
        KeyboardHelper.WireSelectAllOnFocus(this);                                        // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, FocusColor);                              // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                         // RULE-N45

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة
        KeyboardHelper.WireButtonDisabledVisual(btnAddNew, btnRefresh, btnClearFilters);

        // RULE-N47 — اختصارات موحّدة: F2 جديد · F5 تحديث · Esc مسح التصفية
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));

        // RULE-N147 — Enter = الزر الأساسي لهذه الشاشة: زر «صرف جديد» (لا وضع إدخال في شاشة قائمة — RULE-N101).
        KeyboardHelper.WireModeEnter(this, () => btnAddNew);
        AcceptButton = btnAddNew;

        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    private ISupplierPaymentDesktopService PaymentApi
        => _serviceProvider.GetRequiredService<ISupplierPaymentDesktopService>();

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);              // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvSupplierPayments);    // RULE-N51

        // RULE-N22: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvSupplierPayments.ReadOnly = true;
        dgvSupplierPayments.AllowUserToAddRows = false;
        dgvSupplierPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSupplierPayments.MultiSelect = false;

        lblEmptyState.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الأزرار والحقول — تتضمن الاختصارات (RULE-N47).</summary>
    private void _ApplyToolTips()
    {
        toolTip1.SetToolTip(btnAddNew, "صرف جديد (F2)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");
        toolTip1.SetToolTip(txtFilterValue, "تصفية فورية — Enter يطبّق، Esc يمسح كل التصفية");
        toolTip1.SetToolTip(txtSearch, "بحث سريع باسم المورد — تصفية فورية");
        toolTip1.SetToolTip(btnRangeToday, "سندات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(dgvSupplierPayments, "انقر نقراً مزدوجاً لعرض السند");
    }

    private void _SetupGrid()
    {
        // لا تُولَّد أعمدة تلقائياً: أعمدة المساعدة (Notes / AmountText) للتصفية فقط.
        dgvSupplierPayments.AutoGenerateColumns = false;
        dgvSupplierPayments.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",             Header = "رقم السند",     Width = 80 },
            new { Name = "SupplierName",   Header = "اسم المورد",    Width = 200 },
            new { Name = "Amount",         Header = "المبلغ",        Width = 120 },
            new { Name = "PaymentDate",    Header = "التاريخ",       Width = 110 },
            new { Name = "PaymentMethod",  Header = "طريقة الدفع",   Width = 120 },
            new { Name = "ReferenceNo",    Header = "رقم المرجع",    Width = 100 },
        };

        foreach (var c in columns)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = c.Name,
                HeaderText = c.Header,
                DataPropertyName = c.Name,
                Width = c.Width,
                ReadOnly = true,
            };

            // RULE-001: المال يُعرض دائماً N2.
            if (c.Name == "Amount")
            {
                column.DefaultCellStyle.Format = "N2";
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgvSupplierPayments.Columns.Add(column);
        }
    }

    /// <summary>مخطط الجدول: أعمدة العرض + أعمدة مساعدة للتصفية (Notes / AmountText).</summary>
    private static DataTable _CreateEmptyTable()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("SupplierName", typeof(string));
        table.Columns.Add("Amount", typeof(decimal));
        table.Columns.Add("PaymentDate", typeof(string));
        table.Columns.Add("PaymentMethod", typeof(string));
        table.Columns.Add("ReferenceNo", typeof(string));
        table.Columns.Add("Notes", typeof(string));

        // عمود محسوب نصي للمبلغ ⇒ تصفية LIKE على «المبلغ» بلا تحويل داخل RowFilter.
        table.Columns.Add("AmountText", typeof(string), "Convert(Amount, 'System.String')");
        return table;
    }

    private async void _RefreshPayments()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var table = _CreateEmptyTable();

            var result = await PaymentApi.GetAllAsync();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var p in result.Value)
                {
                    var row = table.NewRow();
                    row["Id"] = p.Id;
                    row["SupplierName"] = p.SupplierName ?? string.Empty;
                    row["Amount"] = p.Amount;
                    row["PaymentDate"] = p.PaymentDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                    row["PaymentMethod"] = p.PaymentTypeDisplay;
                    row["ReferenceNo"] = p.PaymentNo ?? string.Empty;
                    row["Notes"] = p.Notes ?? string.Empty;
                    table.Rows.Add(row);
                }
            }
            else if (!result.IsSuccess)
            {
                clsDialogs.Error(result.Error ?? "حدث خطأ في تحميل مدفوعات الموردين");
            }

            _supplierTable = table;
            _supplierView = new DataView(_supplierTable);
            dgvSupplierPayments.DataSource = _supplierView;
            _ApplyFilter();

            // RULE-N148: بعد كل تحميل لا يبقى سطر محدَّد — الاختيار يبدأ من الصفر.
            dgvSupplierPayments.ClearSelection();
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "فشل تحميل مدفوعات الموردين في شاشة مدفوعات الموردين");
            clsDialogs.Error("حدث خطأ أثناء تحميل المدفوعات.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void _ReloadForm()
    {
        _SetupGrid();
        _RefreshPayments();
    }

    // ── التصفية (RULE-N56…RULE-N61) ─────────────────────────────────────────────

    /// <summary>تصفية فورية واحدة تجمع: البحث السريع + الحقل المحدد + النطاق الزمني.</summary>
    private void _ApplyFilter()
    {
        if (_isInitializing || _supplierView.Table == null)
            return;

        var clauses = new List<string>();

        // (1) البحث السريع باسم المورد — السلوك القائم لحقل txtSearch.
        string quick = txtSearch.Text.Trim();
        if (quick.Length > 0)
            clauses.Add(RowFilterHelper.Like("SupplierName", quick));

        // (2) الحقل المحدد في cbFilterBy + قيمته (RULE-N57/N60).
        string value = txtFilterValue.Text.Trim();
        if (value.Length > 0)
        {
            clauses.Add(cbFilterBy.SelectedIndex switch
            {
                1 => RowFilterHelper.Like("ReferenceNo", value),   // رقم السند
                2 => RowFilterHelper.Like("Notes", value),          // البيان
                3 => RowFilterHelper.Like("AmountText", value),     // المبلغ
                _ => RowFilterHelper.Like("SupplierName", value),   // المورد (الافتراضي)
            });
        }

        // (3) النطاق الزمني — التاريخ مخزّن نصياً بصيغة yyyy/MM/dd (قابل للمقارنة النصية).
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        clauses.Add($"PaymentDate >= '{from.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}'" +
                    $" AND PaymentDate <= '{to.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}'");

        try
        {
            _supplierView.RowFilter = string.Join(" AND ", clauses);
        }
        catch (Exception ex)
        {
            // RULE-N29: لا تفاصيل للمستخدم — نُسجّل ونُظهر كل البيانات.
            Serilog.Log.Error(ex, "تعذر تطبيق التصفية على مدفوعات الموردين");
            _supplierView.RowFilter = string.Empty;
        }

        _UpdateSummary();
        _UpdateEmptyState();
        _UpdateClearFiltersState();   // RULE-N148 — حالة زر «مسح التصفية» تتبع التصفية النشطة
    }

    /// <summary>شريط الملخص (RULE-N61): المعروض من الإجمالي + مجموع المبالغ N2.</summary>
    private void _UpdateSummary()
    {
        int shown = _supplierView?.Count ?? 0;
        int total = _supplierTable?.Rows.Count ?? 0;

        decimal totalAmount = 0m;
        if (_supplierView != null && _supplierView.Table != null)
        {
            for (int i = 0; i < shown; i++)
            {
                var row = _supplierView[i].Row;
                if (row["Amount"] is decimal amount)
                    totalAmount += amount;
            }
        }

        lblSummary.Text = $"المعروض: {shown} من {total} — الإجمالي: {clsFormat.Money(totalAmount)}";
        lblRecordsCount.Text = shown.ToString();
    }

    private void _UpdateEmptyState()
    {
        lblEmptyState.Visible = _supplierView == null || _supplierView.Count == 0;
        if (lblEmptyState.Visible)
            lblEmptyState.BringToFront();
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

    /// <summary>Esc: يمسح كل أدوات التصفية ويعيد الشبكة إلى وضعها الافتراضي (RULE-N59 / RULE-N148).</summary>
    private void _ClearFilters()
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل وبلا لمس البيانات (RULE-N148). «تحديث» هو المسؤول عن إعادة التحميل.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e) => _ClearFilters();

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;          // «المورد»
            txtFilterValue.Clear();
            txtSearch.Clear();
            dtpFrom.Value = _defaultFrom;          // النطاق الافتراضي لا «هذا الشهر»
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = false;
        }
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية بلا تحميل (RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        _ApplyFilter();
    }

    /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || !string.IsNullOrWhiteSpace(txtSearch.Text)
            || cbFilterBy.SelectedIndex > 0
            || dtpFrom.Value.Date != _defaultFrom
            || dtpTo.Value.Date != _defaultTo;
    }

    /// <summary>RULE-N47 + RULE-N59: Esc داخل صندوق التصفية = مسحها، وخارجه = إغلاق الشاشة.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearFilters();
            return;
        }

        Close();
    }

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmListSupplierPayments_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;   // «المورد»

            var today = DateTime.Today;

            // نطاق افتراضي يفتح بثلاثة أشهر حتى لا تظهر الشاشة فارغة.
            dtpFrom.Value = new DateTime(today.Year, today.Month, 1).AddMonths(-2);
            dtpTo.Value = today;

            // الافتراضي الذي يعيد إليه «مسح التصفية» = نطاق فتح الشاشة (RULE-N148).
            _defaultFrom = dtpFrom.Value.Date;
            _defaultTo = dtpTo.Value.Date;
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyToolTips();
        _ReloadForm();
        _UpdateClearFiltersState();   // RULE-N148 — لا تصفية نشطة عند الفتح ⇒ الزر معطّل
        ApplyGreenTheme();   // RULE-N50 — الأخير في مسار التحميل
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← إعادة قراءة المدفوعات ← إعادة بناء الأعمدة والملخص والاختيار. لا يمسح التصفية فقط.
    /// </summary>
    private void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            _ClearFilterInputs();   // التصفية تعود للافتراضي قبل إعادة التحميل
            _ReloadForm();          // إعادة بناء الأعمدة + قراءة البيانات (يعيد تطبيق التصفية والملخص)
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "فشل تحديث شاشة مدفوعات الموردين");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            // مؤشر الانتظار يديره _RefreshPayments — هنا حالة الزر فقط.
            _UpdateClearFiltersState();
        }
    }

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _serviceProvider.GetRequiredService<frmSupplierPaymentDialog>().ShowDialog();
        _RefreshPayments();
    }

    // ── Filter Events (تصفية فورية — RULE-N59) ─────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        txtFilterValue.Clear();     // تغيير الحقل يبدأ بقيمة فارغة
        txtFilterValue.Focus();
        _ApplyFilter();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        _ApplyFilter();
    }

    private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
    {
        // Enter يطبّق التصفية فوراً (التنقل للحقل التالي يتكفّل به WireEnterAsTab).
        if (e.KeyCode == Keys.Enter)
            _ApplyFilter();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        _ApplyFilter();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

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

    // ── Grid Events ────────────────────────────────────────────────────────────

    private void dgvSupplierPayments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || dgvSupplierPayments.CurrentRow == null)
            return;

        tsmiEdit_Click(sender, EventArgs.Empty);
    }

    // ── Context Menu ───────────────────────────────────────────────────────────

    private void cmsSupplierPayments_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRows = dgvSupplierPayments.Rows.Count > 0 && dgvSupplierPayments.CurrentRow != null;
        tsmiRefresh.Enabled = true;
        tsmiEdit.Enabled = hasRows;
        tsmiAdd.Enabled = true;
        cmsSupplierPayments.Enabled = hasRows;
    }

    private void tsmiRefresh_Click(object sender, EventArgs e)
    {
        // RULE-N148: «تحديث» = إعادة تحميل كل البيانات — لا مسح تصفية فقط.
        btnRefresh_Click(sender, e);
    }

    private void tsmiEdit_Click(object sender, EventArgs e)
    {
        if (dgvSupplierPayments.CurrentRow == null) return;
        btnAddNew_Click(sender, e);
    }

    private void tsmiAdd_Click(object sender, EventArgs e)
    {
        btnAddNew_Click(sender, e);
    }
}
