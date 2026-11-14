using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Expenses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

public partial class frmListExpenses : Form
{
    /// <summary>RULE-N46 + RULE-N54b: لون الحقل النشط يُقرأ حيّاً من لوحة الثيم (خاصية لا حقل ثابت).</summary>
    private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor);

    private readonly IServiceProvider _serviceProvider;
    private List<ExpenseDto> _allExpenses = new();
    private List<ExpenseDto> _filteredExpenses = new();

    // RULE-N148: الوضع الافتراضي لأدوات التصفية («هذا الشهر») — يُلتقط مرة عند الإنشاء
    // وتُعاد إليه أدوات التصفية عند «مسح التصفية» وعند «تحديث».
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    // يبدأ true حتى لا تنطلق تصفية من داخل المنشئ قبل اكتمال تهيئة أدوات التصفية.
    private bool _isInitializing = true;

    public frmListExpenses(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        // RULE-N40…N46 — تنقل وتحديد وتظليل وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة.
        KeyboardHelper.WireButtonDisabledVisual(btnAddNew, btnRefresh, btnClose, btnClearFilters);

        // RULE-N47 — اختصارات الشاشة: F2 جديد · F5 تحديث · Delete حذف السطر · Esc مسح التصفية.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, () =>
            {
                if (dgvExpenses.ContainsFocus)
                    deleteToolStripMenuItem.PerformClick();
            }),
            (Keys.Escape, _GlobalEscape));

        // RULE-N147: Enter = الزر الأساسي للشاشة («جديد») — شاشة قائمة بلا آلة حالة إدخال (RULE-N101).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        _SetupToolTips();
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    /// <summary>RULE-N147: الزر الأساسي للشاشة — «جديد» (لا آلة حالة إدخال في شاشة القائمة).</summary>
    private Button? _PrimaryActionButton() => btnAddNew;

    /// <summary>تلميحات الأزرار مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnAddNew, "مصروف جديد (F2)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة من الخدمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");
        toolTip1.SetToolTip(btnRangeToday, "تصفية مصروفات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip1.SetToolTip(txtSearch, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
        toolTip1.SetToolTip(chkOnlyCancelled, "إظهار المصروفات الملغاة فقط");
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvExpenses);

        // RULE-N22: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvExpenses.ReadOnly = true;
        dgvExpenses.AllowUserToAddRows = false;
        dgvExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvExpenses.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    private void _SetupGridColumns()
    {
        dgvExpenses.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",              Header = "رقم المصروف",    Fill = 8,  Min = 100 },
            new { Name = "ExpenseNo",       Header = "رقم السند",       Fill = 8,  Min = 100 },
            new { Name = "ExpenseDate",     Header = "التاريخ",         Fill = 12, Min = 120 },
            new { Name = "ExpenseAccountName", Header = "حساب المصروف", Fill = 15, Min = 150 },
            new { Name = "CashBoxName",     Header = "الصندوق",         Fill = 12, Min = 120 },
            new { Name = "Amount",          Header = "المبلغ",          Fill = 10, Min = 100 },
            new { Name = "Notes",           Header = "الوصف",           Fill = 15, Min = 150 },
            new { Name = "StatusName",      Header = "الحالة",          Fill = 10, Min = 100 },
        };

        foreach (var col in columns)
        {
            dgvExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    // ── تحميل البيانات (F5) ─────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: معالج async void مع try/catch و Cursor.WaitCursor واستعادته في finally.</summary>
    private async void _RefreshList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseDesktopService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                _allExpenses = result.Value.ToList();
            }
            else
            {
                _allExpenses = new List<ExpenseDto>();
                clsDialogs.Error(result.Error ?? "فشل في تحميل بيانات المصروفات");
            }

            _ApplyFilter();

            // RULE-N148: إعادة التحميل تُصفّر الاختيار (الملخص يُعاد حسابه داخل _ApplyFilter).
            dgvExpenses.ClearSelection();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة المصروفات في شاشة الدليل");
            clsDialogs.Error("تعذر تحميل بيانات المصروفات. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                btnRefresh.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }

    // ── التصفية الفورية (RULE-N56…N61) ─────────────────────────────────────────

    /// <summary>
    /// التصفية الفورية: النطاق الزمني + الحالة + «الملغاة فقط» + الحقل المختار في «تصفية بـ».
    /// تُطبَّق عند كل تغيير — لا يوجد زر بحث (RULE-N59)، والمقارنة بـ SelectedIndex لا بالنص العربي (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing)
            return;

        IEnumerable<ExpenseDto> query = _allExpenses;

        // 1) النطاق الزمني — نطاق معكوس = نُبادل الطرفين بدل إسقاط النتائج.
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        query = query.Where(x => x.ExpenseDate.Date >= from && x.ExpenseDate.Date <= to);

        // 2) الحالة — 0 = الكل، 1 = مسودة، 2 = مرحّل، 3 = ملغى.
        byte? status = _GetStatusFilterValue();
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        // 3) «الملغاة فقط».
        if (chkOnlyCancelled.Checked)
            query = query.Where(x => x.Status == 3);

        // 4) نص التصفية على الحقل المختار.
        string text = txtSearch.Text.Trim();
        if (text.Length > 0)
        {
            query = cbFilterBy.SelectedIndex switch
            {
                1 => query.Where(x => x.ExpenseNo.ToString().Contains(text, StringComparison.Ordinal)),
                2 => query.Where(x => (x.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                3 => query.Where(x => (x.ExpenseAccountName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                4 => query.Where(x => (x.CashBoxName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                5 => _FilterByAmount(query, text),
                _ => query.Where(x => x.ExpenseNo.ToString().Contains(text, StringComparison.Ordinal)
                                   || (x.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
            };
        }

        _filteredExpenses = query.ToList();
        _PopulateGrid();
        _UpdateSummary();
        _UpdateClearFiltersState();   // RULE-N148: الزر مفعّل فقط عند وجود تصفية نشطة
    }

    /// <summary>تصفية المبلغ: مطابقة رقمية إن أمكن، وإلا مطابقة نصية منسّقة.</summary>
    private static IEnumerable<ExpenseDto> _FilterByAmount(IEnumerable<ExpenseDto> query, string text)
    {
        string normalized = text.Replace(",", string.Empty);

        if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
            return query.Where(x => x.Amount == amount
                                 || clsFormat.Money(x.Amount).Contains(text, StringComparison.OrdinalIgnoreCase));

        return query.Where(x => clsFormat.Money(x.Amount).Contains(text, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>قيمة الحالة المختارة — null = الكل.</summary>
    private byte? _GetStatusFilterValue()
    {
        if (cbStatus.SelectedIndex <= 0)
            return null;

        return (byte)cbStatus.SelectedIndex;   // 1 مسودة · 2 مرحّل · 3 ملغى
    }

    private void _PopulateGrid()
    {
        dgvExpenses.AutoGenerateColumns = false;
        dgvExpenses.Rows.Clear();

        foreach (var ex in _filteredExpenses)
        {
            dgvExpenses.Rows.Add(
                ex.Id,
                ex.ExpenseNo,
                clsFormat.DateToShort(ex.ExpenseDate),
                ex.ExpenseAccountName ?? "",
                ex.CashBoxName ?? "",
                clsFormat.Money(ex.Amount),
                ex.Notes ?? "",
                ex.StatusName ?? ""
            );
        }

        // سطر «لا توجد بيانات» — طبقة نصية فوق الشبكة عند فراغ النتيجة.
        lblEmpty.Visible = _filteredExpenses.Count == 0;

        if (lblEmpty.Visible)
            lblEmpty.BringToFront();
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض/الكل + الإجمالي N2 + عدّاد المرحّل والملغى.</summary>
    private void _UpdateSummary()
    {
        decimal total = 0m;
        int posted = 0;
        int cancelled = 0;

        foreach (var ex in _filteredExpenses)
        {
            total += ex.Amount;

            if (ex.Status == 2)
                posted++;
            else if (ex.Status == 3)
                cancelled++;
        }

        lblSummary.Text =
            $"المعروض: {_filteredExpenses.Count} من {_allExpenses.Count} — الإجمالي: {clsFormat.Money(total)} · مرحّل: {posted} · ملغى: {cancelled}";

        _UpdateRecordsCount();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredExpenses.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvExpenses.CurrentRow == null) return -1;
        if (dgvExpenses.CurrentRow.Cells[0].Value is not int id) return -1;
        return id;
    }

    private byte _GetSelectedStatus()
    {
        if (dgvExpenses.CurrentRow == null) return 0;
        var item = _filteredExpenses.FirstOrDefault(x => x.Id == _GetSelectedId());
        return item?.Status ?? 0;
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    // ── الأحداث ────────────────────────────────────────────────────────────────

    private void frmListExpenses_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            // الشاشة تفتح على الوضع الافتراضي لأدوات التصفية (RULE-N148).
            _ClearFilterInputs();
        }
        finally
        {
            _isInitializing = false;
        }

        _ReloadForm();
        _UpdateClearFiltersState();   // RULE-N148: حالة الزر بعد التحميل مباشرة
        ApplyGreenTheme();            // RULE-N50 — الأخير في مسار التحميل
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateExpense>());
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاع إعدادات التصفية للافتراضي (RULE-N148):
    /// أدوات التصفية ← (لا دلائل/قوائم منسدلة في هذه الشاشة) ← المصروفات من الخدمة ← الملخص والاختيار.
    /// «مسح التصفية» هو المسؤول عن مسح التصفية فقط بلا إعادة تحميل.
    /// </summary>
    private void btnRefresh_Click(object sender, EventArgs e)
    {
        _ClearFilterInputs();
        _ReloadForm();
    }

    // ── أحداث التصفية الفورية ──────────────────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    /// <summary>RULE-N59: Enter يطبّق التصفية فوراً (وهي مطبَّقة أصلاً عند كل كتابة).</summary>
    private void txtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
            return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

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

    private void chkOnlyCancelled_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object sender, EventArgs e)
        => _SetDateRangeThisMonth();

    private void btnRange30_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للبيانات (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
    {
        _ClearAllFilters();
        txtSearch.Focus();
    }

    // ── أدوات التصفية ──────────────────────────────────────────────────────────

    /// <summary>يضبط النطاق بلا إطلاق أحداث وسيطة ثم يُصفّي مرة واحدة.</summary>
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

    /// <summary>«هذا الشهر» = من أول الشهر الحالي حتى اليوم (زر النطاق السريع).</summary>
    private void _SetDateRangeThisMonth()
    {
        var today = DateTime.Today;
        dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
        dtpTo.Value = today;

        if (!_isInitializing)
            _ApplyFilter();
    }

    /// <summary>
    /// تفريغ أدوات التصفية وحدها إلى الوضع الافتراضي (بلا تطبيق ولا إعادة تحميل) — RULE-N148.
    /// تُستهلك من «مسح التصفية» و«تحديث» وعند فتح الشاشة.
    /// </summary>
    private void _ClearFilterInputs()
    {
        bool wasInitializing = _isInitializing;

        // منع تصفية وسيطة عند كل ضبط حقل — تُطبَّق مرة واحدة بعد التفريغ.
        _isInitializing = true;
        try
        {
            txtSearch.Clear();
            cbFilterBy.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            chkOnlyCancelled.Checked = false;
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = wasInitializing;
        }
    }

    /// <summary>RULE-N148: يمسح أدوات التصفية ثم يعيد تطبيق التصفية الافتراضية على البيانات المحمَّلة (بلا تحميل).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الوضع الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtSearch.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyCancelled.Checked
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    /// <summary>RULE-N47 + RULE-N59: Esc داخل صندوق التصفية = مسحها، وخارجه = إغلاق الشاشة.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        Close();
    }

    // ── قائمة السياق والإجراءات ────────────────────────────────────────────────

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateExpense>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void postToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            clsDialogs.Warning("لا يمكن ترحيل المصروف إلا في حالة مسودة.");
            return;
        }

        if (!clsDialogs.Confirm("هل أنت متأكد من ترحيل هذا المصروف؟"))
            return;

        _ = PostAsync(id);
    }

    private async Task PostAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseDesktopService>().PostAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم ترحيل المصروف بنجاح.");
                _ReloadForm();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "فشل في الترحيل");
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل المصروف {ExpenseId} من شاشة الدليل", id);
            clsDialogs.Error("تعذر ترحيل المصروف. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status == 3)
        {
            clsDialogs.Warning("السند ملغي بالفعل.");
            return;
        }

        if (!clsDialogs.Confirm("هل أنت متأكد من إلغاء هذا المصروف؟"))
            return;

        _ = CancelAsync(id);
    }

    private async Task CancelAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseDesktopService>().CancelAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم إلغاء المصروف بنجاح.");
                _ReloadForm();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "فشل في الإلغاء");
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء المصروف {ExpenseId} من شاشة الدليل", id);
            clsDialogs.Error("تعذر إلغاء المصروف. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            clsDialogs.Warning("لا يمكن حذف المصروف إلا في حالة مسودة.");
            return;
        }

        if (!clsDialogs.Confirm("هل أنت متأكد من حذف هذا المصروف؟"))
            return;

        _ = DeleteAsync(id);
    }

    private async Task DeleteAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseDesktopService>().DeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم حذف المصروف بنجاح.");
                _ReloadForm();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "فشل في الحذف");
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل حذف المصروف {ExpenseId} من شاشة الدليل", id);
            clsDialogs.Error("تعذر حذف المصروف. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private void dgvExpenses_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateExpense>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void cmsExpenses_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsExpenses.Enabled = dgvExpenses.Rows.Count > 0 && dgvExpenses.CurrentRow != null;
    }
}
