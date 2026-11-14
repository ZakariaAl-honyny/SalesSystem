using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmListInventoryAdjustments : Form
{
    /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
    private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor);

    private readonly IServiceProvider _serviceProvider;

    private DataTable _adjustmentsTable = new();
    private DataView _adjustmentsView = new();
    private InventoryAdjustmentDto? _selectedAdjustment;

    // يبدأ true حتى لا تنطلق تصفية من داخل المنشئ قبل اكتمال تهيئة أدوات التصفية.
    private bool _isInitializing = true;

    /// <summary>RULE-N148: المدى الزمني الافتراضي وقت الإنشاء («هذا الشهر») — يعيده «مسح التصفية» و«تحديث».</summary>
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    public frmListInventoryAdjustments(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        // RULE-N148 — القيم الافتراضية لأدوات التصفية تُلتقط وقت الإنشاء: من أول الشهر الحالي حتى اليوم.
        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل وأسهم RTL.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, FocusColor);
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة (ومسح التصفية معطّل ما لم توجد تصفية نشطة).
        KeyboardHelper.WireButtonDisabledVisual(btnAdd, btnView, btnPost, btnCancelAdjustment, btnRefresh, btnClearFilters);

        // RULE-N47 — F2 جديد · F5 تحديث · Delete إلغاء · Esc مسح التصفية.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAdd.PerformClick()),
            (Keys.F5, _ReloadForm),
            (Keys.Delete, () =>
            {
                if (dgvAdjustments.ContainsFocus)
                    _CancelAdjustment();
            }),
            (Keys.Escape, _ClearAllFilters));

        // RULE-N147: Enter = الزر الأساسي للشاشة («تسوية جديدة») خارج حقول الإدخال.
        KeyboardHelper.WireModeEnter(this, () => btnAdd);
        AcceptButton = btnAdd;

        _SetupToolTips();
        ApplyUiTheme();
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    /// <summary>
    /// RULE-N147 + RULE-N40: «Enter» يُشغّل الزر الأساسي عبر <c>AcceptButton</c>،
    /// **إلا** داخل «بحث وتصفية» فيبقى سلوك العنصر نفسه (تطبيق التصفية فوراً / فتح القائمة / الحقل التالي)،
    /// وعلى أي زر = تنفيذ الزر نفسه. (AcceptButton في WinForms يسبق KeyDown ويلتقط Enter).
    /// </summary>
    protected override bool ProcessDialogKey(Keys keyData)
    {
        if (keyData == Keys.Enter && gbFilter.ContainsFocus && ActiveControl is not ButtonBase)
            return false;   // غير مُعالج ⇒ يمرّ إلى KeyDown فتعمل قواعد التصفية/التنقل القائمة.

        return base.ProcessDialogKey(keyData);
    }

    /// <summary>RULE-N04 — الخدمة تُحل من الحاوية ولا تُنشأ يدوياً.</summary>
    private IInventoryAdjustmentDesktopService DesktopService
        => _serviceProvider.GetRequiredService<IInventoryAdjustmentDesktopService>();

    // ── المظهر ─────────────────────────────────────────────────────────────────────

    /// <summary>RULE-N47 — تلميح لكل زر مع اختصاره.</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnAdd, "تسوية مخزون جديدة (F2)");
        toolTip1.SetToolTip(btnView, "عرض تفاصيل التسوية المحددة");
        toolTip1.SetToolTip(btnPost, "ترحيل التسوية المحددة (مسودة فقط)");
        toolTip1.SetToolTip(btnCancelAdjustment, "إلغاء ترحيل التسوية المحددة (Delete)");
        toolTip1.SetToolTip(btnRefresh, "تحديث القائمة من الخدمة (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة");
        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
        toolTip1.SetToolTip(cbStatus, "تصفية حسب حالة التسوية");
        toolTip1.SetToolTip(dtpFrom, "بداية النطاق الزمني لتاريخ الإنشاء");
        toolTip1.SetToolTip(dtpTo, "نهاية النطاق الزمني لتاريخ الإنشاء");
        toolTip1.SetToolTip(btnRangeToday, "تسويات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(chkOnlyCancelled, "إظهار التسويات الملغاة فقط");
        toolTip1.SetToolTip(chkIncludeCancelled, "تحميل التسويات الملغاة من الخدمة أيضاً");
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N54) — الترتيب إلزامي.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);      // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvAdjustments); // RULE-N51

        // RULE-N22: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
        dgvAdjustments.ReadOnly = true;
        dgvAdjustments.AllowUserToAddRows = false;
        dgvAdjustments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAdjustments.MultiSelect = false;

        GreenThemeHelper.Apply(this);               // ← الأخير دائماً (RULE-N50)
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            DataPropertyName = name,
            MinimumWidth = min,
            FillWeight = fill,
            ReadOnly = true,
        };
    }

    private void _SetupGridColumns()
    {
        dgvAdjustments.Columns.Clear();

        dgvAdjustments.Columns.Add(_MakeColumn("Id", "رقم التسوية", 5, 60));
        dgvAdjustments.Columns.Add(_MakeColumn("AdjustmentNo", "رقم المستند", 8, 100));
        dgvAdjustments.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 12, 130));
        dgvAdjustments.Columns.Add(_MakeColumn("AdjustmentTypeName", "نوع التسوية", 10, 100));
        dgvAdjustments.Columns.Add(_MakeColumn("StatusName", "الحالة", 8, 80));
        dgvAdjustments.Columns.Add(_MakeColumn("Reason", "السبب", 15, 150));
        dgvAdjustments.Columns.Add(_MakeColumn("CreatedAt", "تاريخ الإنشاء", 12, 130));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: معالج async void مع try/catch و WaitCursor واستعادته في finally.</summary>
    private async void _RefreshList()
    {
        Cursor = dgvAdjustments.Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            _adjustmentsTable = _CreateTable();
            _adjustmentsView = new DataView(_adjustmentsTable);

            var result = await DesktopService.GetAllAsync(includeInactive: chkIncludeCancelled.Checked);

            if (result.IsSuccess && result.Value is { Count: > 0 })
            {
                foreach (var a in result.Value)
                {
                    _adjustmentsTable.Rows.Add(
                        a.Id,
                        a.AdjustmentNo,
                        a.WarehouseName ?? "",
                        a.AdjustmentTypeName ?? "",
                        a.StatusName ?? "",
                        a.Reason ?? "",
                        a.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                        a.Status);
                }
            }
            else if (!result.IsSuccess)
            {
                clsDialogs.Error(result.Error ?? "حدث خطأ في تحميل قائمة التسويات");
            }

            dgvAdjustments.DataSource = _adjustmentsView;
            _ApplyFilter();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة تسويات المخزون في شاشة القائمة");
            clsDialogs.Error("تعذر تحميل قائمة تسويات المخزون. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                btnRefresh.Enabled = true;
                Cursor = dgvAdjustments.Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>جدول الذاكرة: الأعمدة المعروضة + عمود الحالة الخفي اللازم للتصفية والملخص.</summary>
    private static DataTable _CreateTable()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("AdjustmentNo", typeof(string));
        table.Columns.Add("WarehouseName", typeof(string));
        table.Columns.Add("AdjustmentTypeName", typeof(string));
        table.Columns.Add("StatusName", typeof(string));
        table.Columns.Add("Reason", typeof(string));
        table.Columns.Add("CreatedAt", typeof(string));   // "yyyy-MM-dd HH:mm" — مقارنة ISO قابلة للترتيب
        table.Columns.Add("Status", typeof(int));         // 1 مسودة · 2 مرحّل · 3 ملغى (لا يُعرض)
        return table;
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    // ── التصفية الفورية (RULE-N56…N61) ─────────────────────────────────────────────

    /// <summary>
    /// التصفية الفورية بالحالة + «الملغاة فقط» + النطاق الزمني + نص التصفية على الحقل المختار.
    /// لا يوجد زر بحث (RULE-N59)، والمقارنة بـ SelectedIndex لا بالنص العربي (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing || _adjustmentsView.Table is null)
            return;

        var parts = new List<string>();

        // 1) الحالة — 1 مسودة · 2 مرحّل · 3 ملغى (القيم الحقيقية لهذا الكيان).
        byte? status = _GetStatusFilterValue();
        if (status.HasValue)
            parts.Add($"Status = {status.Value}");

        // 2) «الملغاة فقط» — يحتاج عمود الحالة الموجود أصلاً.
        if (chkOnlyCancelled.Checked)
            parts.Add("Status = 3");

        // 3) النطاق الزمني على تاريخ الإنشاء (نطاق معكوس = نُبادل الطرفين).
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        parts.Add($"CreatedAt >= '{from:yyyy-MM-dd}' AND CreatedAt < '{to.AddDays(1):yyyy-MM-dd}'");

        // 4) نص التصفية على الحقل المختار.
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            // RULE-N60: القيمة تُهرَّب في RowFilterHelper، وأسماء الأعمدة ثابتة داخل الشاشة.
            string adjustmentNo = RowFilterHelper.Like("AdjustmentNo", text);
            string warehouse = RowFilterHelper.Like("WarehouseName", text);
            string reason = RowFilterHelper.Like("Reason", text);

            parts.Add(cbFilterBy.SelectedIndex switch
            {
                1 => adjustmentNo,
                2 => warehouse,
                3 => reason,
                _ => $"({adjustmentNo} OR {warehouse} OR {reason})",
            });
        }

        try
        {
            _adjustmentsView.RowFilter = string.Join(" AND ", parts);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تطبيق تصفية تسويات المخزون");
            _adjustmentsView.RowFilter = string.Empty;
        }

        _UpdateSummary();
        _UpdateButtonsState();
        _UpdateClearFiltersState();   // RULE-N148 — حالة زر «مسح التصفية» بعد كل تطبيق تصفية
    }

    /// <summary>قيمة الحالة المختارة — null = الكل.</summary>
    private byte? _GetStatusFilterValue()
    {
        if (cbStatus.SelectedIndex <= 0)
            return null;

        return (byte)cbStatus.SelectedIndex;   // 1 مسودة · 2 مرحّل · 3 ملغى
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض من الكل + عدّاد كل حالة.</summary>
    private void _UpdateSummary()
    {
        int draft = 0, posted = 0, cancelled = 0;

        foreach (DataRowView row in _adjustmentsView)
        {
            switch (Convert.ToInt32(row["Status"]))
            {
                case 1: draft++; break;
                case 2: posted++; break;
                case 3: cancelled++; break;
            }
        }

        int shown = _adjustmentsView.Count;
        int total = _adjustmentsTable.Rows.Count;

        lblSummary.Text =
            $"المعروض: {clsFormat.Number(shown)} من {clsFormat.Number(total)} — مسودات: {clsFormat.Number(draft)} · مُرحَّلة: {clsFormat.Number(posted)} · ملغاة: {clsFormat.Number(cancelled)}";

        lblRecordsCount.Text = clsFormat.Number(shown);

        // سطر «لا توجد بيانات» — طبقة نصية فوق الشبكة عند فراغ النتيجة (RULE-N22).
        bool empty = shown == 0;
        lblEmpty.Visible = empty;

        if (empty)
            lblEmpty.BringToFront();
    }

    /// <summary>RULE-N43: أزرار الإجراءات مفعّلة فقط عند وجود سجل محدد.</summary>
    private void _UpdateButtonsState()
    {
        bool hasSelection = dgvAdjustments.SelectedRows.Count > 0;

        btnView.Enabled = hasSelection;
        btnPost.Enabled = hasSelection;
        btnCancelAdjustment.Enabled = hasSelection;
    }

    // ── Selection ──────────────────────────────────────────────────────────────────

    private async Task _SelectAdjustmentAsync()
    {
        _selectedAdjustment = null;

        if (dgvAdjustments.SelectedRows.Count == 0)
            return;

        var row = dgvAdjustments.SelectedRows[0];
        if (row.DataBoundItem is not DataRowView drv)
            return;

        int id = Convert.ToInt32(drv["Id"]);
        var result = await DesktopService.GetByIdAsync(id);
        _selectedAdjustment = result.IsSuccess ? result.Value : null;
    }

    // ── Actions ────────────────────────────────────────────────────────────────────

    private void _AddAdjustment()
    {
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateInventoryAdjustment>();
        editor.ShowDialog();
        _ReloadForm();
    }

    private async void _ViewDetails()
    {
        try
        {
            if (dgvAdjustments.SelectedRows.Count == 0)
                return;

            Cursor = Cursors.WaitCursor;
            await _SelectAdjustmentAsync();

            if (_selectedAdjustment is null)
                return;

            var editor = _serviceProvider.GetRequiredService<frmAddUpdateInventoryAdjustment>();
            editor.LoadAdjustment(_selectedAdjustment.Id);
            editor.ShowDialog();
            _ReloadForm();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل فتح تفاصيل تسوية المخزون من شاشة القائمة");
            clsDialogs.Error("تعذر فتح تفاصيل التسوية. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private async void _PostAdjustment()
    {
        try
        {
            if (dgvAdjustments.SelectedRows.Count == 0)
                return;

            Cursor = Cursors.WaitCursor;
            await _SelectAdjustmentAsync();

            if (_selectedAdjustment is null || _selectedAdjustment.Status != 1)   // Draft only
                return;

            if (!clsDialogs.Confirm("هل أنت متأكد من ترحيل تسوية المخزون؟"))
                return;

            var result = await DesktopService.PostAsync(_selectedAdjustment.Id);

            if (result.IsSuccess)
                clsDialogs.Success("تم ترحيل تسوية المخزون بنجاح");
            else
                clsDialogs.Error($"خطأ في الترحيل: {result.Error}");

            _ReloadForm();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل تسوية المخزون {AdjustmentId} من شاشة القائمة", _selectedAdjustment?.Id);
            clsDialogs.Error("تعذر ترحيل تسوية المخزون. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    private async void _CancelAdjustment()
    {
        try
        {
            if (dgvAdjustments.SelectedRows.Count == 0)
                return;

            Cursor = Cursors.WaitCursor;
            await _SelectAdjustmentAsync();

            if (_selectedAdjustment is null || _selectedAdjustment.Status != 2)   // Posted only
                return;

            if (!clsDialogs.Confirm("هل أنت متأكد من إلغاء تسوية المخزون؟ لا يمكن التراجع عن هذا الإجراء."))
                return;

            var result = await DesktopService.CancelAsync(_selectedAdjustment.Id);

            if (result.IsSuccess)
                clsDialogs.Success("تم إلغاء تسوية المخزون بنجاح");
            else
                clsDialogs.Error($"خطأ في الإلغاء: {result.Error}");

            _ReloadForm();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء تسوية المخزون {AdjustmentId} من شاشة القائمة", _selectedAdjustment?.Id);
            clsDialogs.Error("تعذر إلغاء تسوية المخزون. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
                Cursor = Cursors.Default;
        }
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmListInventoryAdjustments_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        try
        {
            _ClearFilterInputs();               // التصفية تبدأ على القيم الافتراضية (RULE-N148)
            chkIncludeCancelled.Checked = false; // نطاق التحميل الافتراضي (بلا الملغاة)
        }
        finally
        {
            _isInitializing = false;
        }

        _ReloadForm();
        _UpdateClearFiltersState();   // RULE-N148 — الشاشة تُفتح على التصفية الافتراضية ⇒ الزر معطّل
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        _AddAdjustment();
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        _ViewDetails();
    }

    private void btnPost_Click(object sender, EventArgs e)
    {
        _PostAdjustment();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        _CancelAdjustment();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        _FullRefresh();
    }

    /// <summary>
    /// RULE-N148: «تحديث» (F5) = إعادة تحميل **كاملة**: تصفية افتراضية ← نطاق التحميل الافتراضي
    /// ← إعادة قراءة الشبكة من الخدمة ← ملخص وتحديد افتراضي.
    /// وليس بديلاً عن «مسح التصفية» الذي يمسح أدوات التصفية وحدها بلا إعادة تحميل.
    /// (هذه الشاشة لا تحتوي دليل مستودعات في المحرر — المستودع عمود عرض يأتي جاهزاً من الخدمة.)
    /// </summary>
    private void _FullRefresh()
    {
        if (_isInitializing)
            return;

        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            _ClearFilterInputs();               // أدوات التصفية تعود للافتراضي
            chkIncludeCancelled.Checked = false; // نطاق التحميل يعود للافتراضي
        }
        finally
        {
            _isInitializing = wasInitializing;
        }

        _selectedAdjustment = null;      // التحديد يعود إلى الافتراضي
        dgvAdjustments.ClearSelection();

        _ReloadForm();                   // إعادة قراءة البيانات ثم تطبيق التصفية والملخص
        _UpdateClearFiltersState();
    }

    // ── أحداث التصفية الفورية ──────────────────────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ApplyFilter();
    }

    /// <summary>RULE-N59: Enter يطبّق التصفية فوراً.</summary>
    private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
            return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            _ApplyFilter();
        }
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

    private void chkIncludeCancelled_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing)
            return;

        _ReloadForm();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today, DateTime.Today);
    }

    private void btnRangeMonth_Click(object sender, EventArgs e)
    {
        _SetDateRangeThisMonth();
    }

    private void btnRange30_Click(object sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);
    }

    // ── أدوات التصفية ──────────────────────────────────────────────────────────────

    /// <summary>يضبط النطاق بلا إطلاق أحداث وسيطة ثم يُصفّي مرة واحدة.</summary>
    private void _SetDateRange(DateTime from, DateTime to)
    {
        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            dtpFrom.Value = from;
            dtpTo.Value = to;
        }
        finally
        {
            _isInitializing = wasInitializing;
        }

        _ApplyFilter();
    }

    /// <summary>«هذا الشهر» = من أول الشهر الحالي حتى اليوم (الافتراضي ومسح Esc).</summary>
    private void _SetDateRangeThisMonth()
    {
        _SetDateRange(_defaultFrom, _defaultTo);
    }

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للبيانات (RULE-N148). «تحديث» هو المسؤول عن إعادة التحميل.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>
    /// تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح التصفية» و«تحديث» (RULE-N148).
    /// ملاحظة: «عرض الملغاة» ليس أداة تصفية بل نطاق تحميل من الخدمة (معالجه يُعيد التحميل)،
    /// فلا يُعاد ضبطه هنا حتى لا يُمسح بالزر ما لا يستطيع الزر إعادة تحميله بلا خدمة.
    /// </summary>
    private void _ClearFilterInputs()
    {
        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;           // «لا شيء» ⇒ البحث في كل الحقول
            txtFilterValue.Clear();
            cbStatus.SelectedIndex = 0;             // «الكل»
            chkOnlyCancelled.Checked = false;       // «الملغاة فقط»
            dtpFrom.Value = _defaultFrom;           // النطاق الافتراضي (هذا الشهر)
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = wasInitializing;
        }
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية بلا إعادة تحميل (RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
        txtFilterValue.Focus();
    }

    /// <summary>RULE-N43/RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyCancelled.Checked
            || !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    // ── الشبكة وقائمة السياق ───────────────────────────────────────────────────────

    private void dgvAdjustments_SelectionChanged(object sender, EventArgs e)
    {
        _UpdateButtonsState();
    }

    private void dgvAdjustments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        _ViewDetails();
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        var hit = dgvAdjustments.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0) return;
        dgvAdjustments.ClearSelection();
        dgvAdjustments.Rows[hit.RowIndex].Selected = true;
        contextMenuStrip.Show(dgvAdjustments, e.Location);
    }

    private void dgvAdjustments_MouseDown(object sender, MouseEventArgs e)
    {
        ShowContextMenu(e);
    }

    private void tsmiView_Click(object sender, EventArgs e)
    {
        _ViewDetails();
    }

    private void tsmiPost_Click(object sender, EventArgs e)
    {
        _PostAdjustment();
    }

    private void tsmiCancel_Click(object sender, EventArgs e)
    {
        _CancelAdjustment();
    }
}
