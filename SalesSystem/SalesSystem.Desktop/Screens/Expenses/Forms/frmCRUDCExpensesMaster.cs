using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Serilog;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

/// <summary>
/// شاشة المصروفات المدمجة (Master) — تحلّ محل <c>frmListExpenses</c> + <c>frmAddUpdateExpense</c>:
/// محرر المستند + أوامر الشاشة + صندوق تصفية + شبكة عرض + شريط ملخص في شاشة واحدة،
/// بالنمط المُشهَد <c>frmProductMaster</c> (<c>RULE-N100</c>…<c>RULE-N104</c> · <c>F-004</c>).
/// <para>
/// <b>آلة الحالة:</b> <see cref="EntryMode"/> ثلاثية (<c>Browsing</c> · <c>Adding</c> · <c>Editing</c>)
/// و<see cref="_ApplyModeState"/> مركزية تُستدعى بعد **كل** انتقال (<c>RULE-N150</c>).
/// </para>
/// <para>
/// <b>السلوك محفوظ حرفياً من المصدر:</b> الحقول والقواعد نفسها (الصندوق · البيان إلزاميان، والمبلغ
/// يجب أن يكون أكبر من صفر)، وتلميح التفقيط الحيّ على المبلغ، والمصروف **المرحّل/الملغى للعرض فقط**
/// (<c>_loadedStatus != StatusDraft</c>) تماماً كما كان في <c>frmAddUpdateExpense</c>.
/// </para>
/// <para>
/// <b>المكان الواحد:</b> التنقل والثيم والتغذية والتصفية والتفقيط والرسائل والتنسيق كلها من بيوتها
/// القائمة (<c>KeyboardHelper</c> · <c>GreenThemeHelper</c> · <c>RequiredFieldHelper</c> ·
/// <c>FilterPanelHelper</c> · <c>LiveTafqeetHelper</c> · <c>DocumentTextHelper</c> · <c>clsDialogs</c> ·
/// <c>clsFormat</c>) — لا نسخة محلية من أي سلوك مشترك (<c>RULE-N169</c>…<c>RULE-N172</c>).
/// </para>
/// </summary>
public partial class frmCRUDCExpensesMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── العقود المستهلَكة حرفياً (RULE-N03/N04) ────────────────────────────────
    private IExpenseDesktopService ExpenseService => _serviceProvider.GetRequiredService<IExpenseDesktopService>();
    private ICashBoxDesktopService CashBoxService => _serviceProvider.GetRequiredService<ICashBoxDesktopService>();

    // ── حالة المستند (RULE-019 — Draft = 1 · Posted = 2 · Cancelled = 3) ───────
    private const byte StatusDraft = 1;
    private const byte StatusPosted = 2;
    private const byte StatusCancelled = 3;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( إدارة المصروفات ))";

    /// <summary>المعرّف المحمَّل في المحرر — <c>null</c> = لا سجل محمَّل.</summary>
    private int? _editId;

    /// <summary>حالة المستند المحمَّل — أساس حظر الحفظ/الترحيل/الحذف (تكافؤ رفض الخدمة).</summary>
    private byte _loadedStatus = StatusDraft;

    /// <summary>حالة الشاشة (آلة الحالة) — تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>.</summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>معرّف المصروف المطلوب تحديده عند الفتح من شاشة أخرى (تنقّل — RULE-N132).</summary>
    private int? _pendingSelectId;

    /// <summary>هل انتهى التحميل الأول للبيانات؟ أساس تطبيق التحديد المطلوب فوراً أو بعده.</summary>
    private bool _dataLoaded;

    /// <summary>كل المصروفات كما وردت من الخدمة (المصدر الوحيد للتصفية والملخص).</summary>
    private readonly List<ExpenseDto> _allExpenses = new();

    /// <summary>المصروفات المعروضة بعد التصفية.</summary>
    private List<ExpenseDto> _filteredExpenses = new();

    /// <summary>فهرس المعرّف ← المصروف، لقراءة الحالة بلا نداء خدمة إضافي.</summary>
    private readonly Dictionary<int, ExpenseDto> _expensesById = new();

    // RULE-N148: الوضع الافتراضي لأدوات التصفية («هذا الشهر») — يُلتقط مرة عند الإنشاء.
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    /// <summary>يمنع انطلاق تصفية أثناء تهيئة أدوات التصفية في المُنشئ/التحميل.</summary>
    private bool _isInitializing = true;

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N43/RULE-N44).</summary>
    private enum EntryMode
    {
        /// <summary>عرض فقط: كل حقول الإدخال معطّلة حتى يضغط المستخدم «جديد» أو يحدّد سجلاً (RULE-N149).</summary>
        Browsing,

        /// <summary>إضافة مصروف جديد: الحقول مفعّلة وزر «حفظ» متاح فقط.</summary>
        Adding,

        /// <summary>سجل محدد: مسودة ⇒ قابل للتعديل · مرحّل/ملغى ⇒ عرض للقراءة فقط.</summary>
        Editing
    }

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmCRUDCExpensesMaster()
    {
        InitializeComponent();
    }

    public frmCRUDCExpensesMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        // RULE-N191/N193 — Enter داخل لوحة المحرر يمشي بترتيب العرض ثم يُسلِّم التركيز لزر الوضع بلا تنفيذ.
        // استدعاء **ثنائي الوسيط فقط**: الأحادي يحجز الحقول قبله فيصير «الأول يفوز» صامتاً.
        KeyboardHelper.WireEnterAsTab(pnlFields, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);                                                    // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                     // RULE-N45

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة بنص باهت (لا Enabled = false فقط).
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnPost, btnCancelDocument, btnClearFilters);

        // RULE-N47 — الاختصارات الموحّدة: F2 جديد · F4 ترحيل · F5 تحديث · Esc مسح التصفية/إلغاء المحرر.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F4, () => { if (btnPost.Enabled) btnPost.PerformClick(); }),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));

        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);                                     // RULE-N147

        // RULE-N180…N183 + RULE-N194 — تغذية الحقول المطلوبة حيّاً (أيقونة ✗ + رسالة + ومضة رفض).
        // ترتيب الاستدعاء إلزامي: **بعد** WireFocusHighlight (ترتيب اشتراك Leave) و**قبل** الثيم (RULE-N183).
        // القواعد هي نفسها المعلنة في frmAddUpdateExpense بنفس رسائله — وقاعدة «المبلغ > 0» ليست قاعدة
        // «حقل فارغ» فتبقى في مكانها أدناه (RULE-N172).
        RequiredFieldHelper.Attach(this, errorProvider,
            new(cmbCashBox, "يرجى اختيار الصندوق."),
            new(txtNotes, "بيان المصروف مطلوب."));

        // RULE-N97 — التفقيط الحيّ كتلميح على حقل المبلغ (التلميح المُعلَن في المصمّم — RULE-N145).
        LiveTafqeetHelper.Attach(liveTip, (numAmount, "المبلغ", LiveTafqeetHelper.ReadMode.Money));

        _LoadStatusOptions();     // نصوص الحالة من البيت الواحد DocumentTextHelper (RULE-N169)
        _SetupToolTips();         // RULE-N47 — التلميحات مع اختصاراتها

        FilterPanelHelper.Attach(this, _serviceProvider);   // يتبنّى btnToggleFilters المُعلَن في المصمّم

        ApplyUiTheme();           // ينتهي بـ GreenThemeHelper.Apply(this) كآخر سطر (RULE-N50)
    }

    // ── التنقّل من شاشة أخرى (RULE-N132 — تُفتح الشاشة داخل النافذة لا نمطياً) ──

    /// <summary>
    /// يطلب تحديد مصروف بالمعرّف عند فتح الشاشة من شاشة أخرى (مثل حركات الصندوق):
    /// إن كانت البيانات محمَّلة يُحدَّد السطر فوراً، وإلا يُحفظ الطلب ويُطبَّق بعد انتهاء التحميل الأول.
    /// التحديد يفعل ما يفعله نقر المستخدم تماماً — تعبئة المحرر وضبط الوضع: تعديل للمسودة،
    /// وعرض للقراءة للمرحّل/الملغى (<c>RULE-N150</c>/<c>RULE-N151</c>). المعرّف غير الموجب يُتجاهل.
    /// </summary>
    public void SelectRecord(int id)
    {
        if (id <= 0) return;

        _pendingSelectId = id;

        if (_dataLoaded) _ApplyPendingSelection();
    }

    /// <summary>
    /// يطبّق التحديد المطلوب على البيانات المحمَّلة. السجل غير الموجود لا يُظهر رسالة:
    /// التنقّل لا يُفشل الشاشة، والمستخدم يرى القائمة كما هي.
    /// </summary>
    private void _ApplyPendingSelection()
    {
        if (_pendingSelectId is not int id) return;

        _pendingSelectId = null;

        if (!_expensesById.TryGetValue(id, out ExpenseDto? expense)) return;

        _SelectGridRow(id);              // نفس السطر الذي يحدده نقر المستخدم
        _ApplyExpenseToEditor(expense);  // تعبئة المحرر + ضبط الوضع (تعديل/عرض)
    }

    // ── الثيم والتلميحات ────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53): كل لون هنا **مُعلَن بقيمته الحرفية** في
    /// <c>frmExpensesMaster.Designer.cs</c> (RULE-N134/N140) — والمصمّم هو المرجع البصري.
    /// </summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvMain);

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblNetTotalValue.ForeColor = GreenThemeHelper.TextStrong;
        lblStatusValue.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الأزرار والحقول مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip.SetToolTip(btnNew, "مصروف جديد (F2)");
        toolTip.SetToolTip(btnSave, "حفظ مصروف جديد");
        toolTip.SetToolTip(btnEdit, "تعديل المصروف المحدد — مسودة فقط");
        toolTip.SetToolTip(btnDelete, "حذف المصروف المحدد — مسودة غير مرحّلة فقط");
        toolTip.SetToolTip(btnPost, "ترحيل المصروف المحدد (F4)");
        toolTip.SetToolTip(btnCancelDocument, "إلغاء المستند المحدد");
        toolTip.SetToolTip(btnRefresh, "إعادة تحميل البيانات وإرجاع التصفية للافتراضي (F5)");
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");

        toolTip.SetToolTip(dtpDate, "تاريخ المصروف");
        toolTip.SetToolTip(cmbCashBox, "الصندوق الذي يتحمّل المصروف (إلزامي)");
        toolTip.SetToolTip(numAmount, "المبلغ — يظهر تفقيطه بالحروف تلقائياً (أكبر من صفر)");
        toolTip.SetToolTip(txtNotes, "بيان المصروف (إلزامي)");

        toolTip.SetToolTip(btnRangeToday, "تصفية مصروفات اليوم فقط");
        toolTip.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
        toolTip.SetToolTip(cbStatus, "تصفية بحالة المستند");
        toolTip.SetToolTip(chkOnlyCancelled, "إظهار المصروفات الملغاة فقط");
        toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
    }

    /// <summary>
    /// خيارات تصفية الحالة: «الكل» عنصر محايد محلي (ليس نص حالة)، وبقية النصوص من البيت الواحد
    /// <see cref="DocumentTextHelper.StatusOptions"/>. الترتيب «الكل» ثم 1 · 2 · 3 فيبقى
    /// <c>cbStatus.SelectedIndex</c> مساوياً لقيمة الحالة الرقمية في التصفية (RULE-N60).
    /// </summary>
    private void _LoadStatusOptions()
    {
        cbStatus.Items.Clear();
        cbStatus.Items.Add("الكل");   // عنصر محايد محلي — ليس نص حالة

        foreach (var option in DocumentTextHelper.StatusOptions)
            cbStatus.Items.Add(option.Text);

        cbStatus.SelectedIndex = 0;
    }

    // ── التحميل ─────────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26 — معالج async void مع try/catch و Cursor.WaitCursor واستعادته في finally.</summary>
    private async void frmExpensesMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // تصفية بحالتها الافتراضية (RULE-N148)
            _isInitializing = false;       // من هنا تصبح التصفية حيّة

            await _LoadCashBoxesAsync();   // القائمة المنسدلة (RULE-N33: DisplayMember لازم لكائنات)
            await _LoadExpensesAsync();    // يبني الشبكة ويطبّق التصفية والملخص

            _ResetForm();                  // المحرر معطّل + وضع العرض (RULE-N149)

            _dataLoaded = true;            // من هنا يمكن تحديد مصروف مطلوب من شاشة أخرى
            _ApplyPendingSelection();      // تنقّل من شاشة أخرى (RULE-N132)
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة المصروفات");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            _isInitializing = false;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تحميل الصناديق النشطة في القائمة المنسدلة (إلزامي: <c>DisplayMember</c> و<b>ValueMember</b>).</summary>
    private async Task _LoadCashBoxesAsync()
    {
        var result = await CashBoxService.GetAllAsync();

        if (result.IsSuccess && result.Value != null)
        {
            var cashBoxes = result.Value.Where(b => b.IsActive)
                .Select(b => new { b.Id, b.Name })
                .ToList();

            cmbCashBox.DisplayMember = "Name";
            cmbCashBox.ValueMember = "Id";
            cmbCashBox.DataSource = cashBoxes;
            cmbCashBox.SelectedIndex = -1;
        }
        else
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة الصناديق.");
        }
    }

    /// <summary>
    /// تحميل المصروفات من الخدمة. **لا تُفرَّغ الذاكرة عند فشل التحميل** — التفريغ داخل فرع
    /// <c>IsSuccess</c> فقط (RULE: الخطأ لا يمحو البيانات المعروضة).
    /// </summary>
    private async Task _LoadExpensesAsync()
    {
        var result = await ExpenseService.GetAllAsync();

        if (result.IsSuccess && result.Value != null)
        {
            _allExpenses.Clear();
            _expensesById.Clear();

            foreach (var expense in result.Value)
            {
                _allExpenses.Add(expense);
                _expensesById[expense.Id] = expense;
            }
        }
        else
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة المصروفات.");
        }

        _ApplyFilter();   // يعيد بناء الشبكة والملخص وحالة زر «مسح التصفية»
    }

    // ── التصفية الفورية (RULE-N24/N56…N61) ─────────────────────────────────────

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void txtFilterValue_TextChanged(object? sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpFrom_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object? sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void chkOnlyCancelled_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    /// <summary>RULE-N59: Enter يطبّق التصفية فوراً · Esc يمسح كل أدوات التصفية.</summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
        {
            _ApplyFilter();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        else if (e.KeyCode == Keys.Escape)
        {
            _ClearAllFilters();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }

    /// <summary>RULE-N148: «مسح التصفية» يمسح أدوات التصفية فقط بلا إعادة تحميل ولا لمس للمحرر.</summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    private void btnRangeToday_Click(object? sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object? sender, EventArgs e)
    {
        var today = DateTime.Today;
        _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object? sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

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

    /// <summary>تفريغ أدوات التصفية وحدها إلى الوضع الافتراضي (بلا تطبيق ولا تحميل) — RULE-N148.</summary>
    private void _ClearFilterInputs()
    {
        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            txtFilterValue.Clear();
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

    /// <summary>RULE-N148: يمسح أدوات التصفية ثم يعيد تطبيق التصفية الافتراضية على البيانات المحمَّلة.</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الوضع الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyCancelled.Checked
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    /// <summary>RULE-N47: Esc داخل صندوق التصفية = مسحها، وخارجه = إلغاء المحرر وإرجاع وضع العرض.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        _ResetForm();
    }

    /// <summary>
    /// التصفية الفورية: النطاق الزمني (بمقارنة <c>.Date</c> — شامل اليوم) + الحالة + «الملغاة فقط» +
    /// نص التصفية على الحقل المختار. لا يوجد زر بحث (RULE-N59)، والمقارنة بـ <c>SelectedIndex</c>
    /// لا بنص عربي (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing) return;

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
            query = query.Where(x => x.Status == StatusCancelled);

        // 4) نص التصفية على الحقل المختار.
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            query = cbFilterBy.SelectedIndex switch
            {
                1 => query.Where(x => x.ExpenseNo.ToString().Contains(text, StringComparison.Ordinal)),
                2 => query.Where(x => (x.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                3 => query.Where(x => (x.CashBoxName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                4 => _FilterByAmount(query, text),
                _ => query.Where(x => x.ExpenseNo.ToString().Contains(text, StringComparison.Ordinal)
                                   || (x.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
            };
        }

        _filteredExpenses = query.ToList();
        _PopulateGrid();
        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    /// <summary>
    /// تصفية المبلغ: قراءة النص بقارئ الأرقام الواحد <c>clsFormat.TryParseMoney</c> (يتكفّل بالفاصل
    /// الحيّ والأرقام العربية) — لا طرح فواصل يدوياً ولا <c>decimal.TryParse</c> محلي (RULE-N169).
    /// </summary>
    private static IEnumerable<ExpenseDto> _FilterByAmount(IEnumerable<ExpenseDto> query, string text)
    {
        if (clsFormat.TryParseMoney(text, out decimal amount))
            return query.Where(x => x.Amount == amount);

        return query.Where(x => clsFormat.Money(x.Amount).Contains(text, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>قيمة الحالة المختارة — null = الكل.</summary>
    private byte? _GetStatusFilterValue()
    {
        if (cbStatus.SelectedIndex <= 0)
            return null;

        return (byte)cbStatus.SelectedIndex;   // 1 مسودة · 2 مرحّل · 3 ملغى
    }

    /// <summary>
    /// تعبئة الشبكة يدوياً (لا AutoGenerateColumns — الأعمدة معلنة في المصمّم).
    /// المعرّف يُمرَّر في عموده المخفي ويُقرأ لاحقاً عبر <see cref="GridRowHelper"/> فقط (RULE-N179).
    /// </summary>
    private void _PopulateGrid()
    {
        dgvMain.Rows.Clear();

        foreach (var expense in _filteredExpenses)
        {
            dgvMain.Rows.Add(
                expense.Id,
                expense.ExpenseNo,
                clsFormat.DateToShort(expense.ExpenseDate),
                expense.CashBoxName ?? string.Empty,
                clsFormat.Money(expense.Amount),
                expense.Notes ?? string.Empty,
                string.IsNullOrWhiteSpace(expense.StatusName) ? DocumentTextHelper.StatusText(expense.Status) : expense.StatusName);
        }

        // صف «لا توجد بيانات» — طبقة نصية فوق الشبكة عند فراغ النتيجة.
        lblEmpty.Visible = _filteredExpenses.Count == 0;

        if (lblEmpty.Visible)
            lblEmpty.BringToFront();
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض/الكل + الإجمالي <c>N2</c> + عدّاد الحالات.</summary>
    private void _UpdateSummary()
    {
        decimal total = 0m;
        int draft = 0;
        int posted = 0;
        int cancelled = 0;

        foreach (var expense in _filteredExpenses)
        {
            total += expense.Amount;

            if (expense.Status == StatusPosted) posted++;
            else if (expense.Status == StatusCancelled) cancelled++;
            else if (expense.Status == StatusDraft) draft++;
        }

        lblSummary.Text =
            $"المعروض: {clsFormat.Number(_filteredExpenses.Count)} من {clsFormat.Number(_allExpenses.Count)} — " +
            $"الإجمالي: {clsFormat.Money(total)} · مسودة: {clsFormat.Number(draft)} · " +
            $"مرحّل: {clsFormat.Number(posted)} · ملغى: {clsFormat.Number(cancelled)}";
    }

    // ── اختيار سطر من الشبكة ────────────────────────────────────────────────────

    private async void dgvMain_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadSelectedRowAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مصروف من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات المصروف المحدد. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>النقر المزدوج: نقل التركيز إلى أول حقل في المحرر عندما يكون السجل قابلاً للتعديل.</summary>
    private void dgvMain_DoubleClick(object? sender, EventArgs e)
    {
        if (_mode == EntryMode.Browsing) return;

        if (_IsDraftLoaded()) cmbCashBox.Focus();
        else btnNew.Focus();
    }

    /// <summary>يحمّل السطر المحدد في المحرر (وضع التعديل) — القراءة بالحقل المنطقي عبر <see cref="GridRowHelper"/>.</summary>
    private async Task _LoadSelectedRowAsync()
    {
        if (dgvMain.CurrentRow is not { } row) return;
        if (!GridRowHelper.TryGetInt(row, "Id", out int id) || id <= 0) return;

        await _FillFormAsync(id);
    }

    /// <summary>تعبئة المحرر من الخدمة ثم ضبط الوضع والأزرار (اختيار سجل = وضع التعديل — RULE-N151).</summary>
    private async Task _FillFormAsync(int expenseId)
    {
        var result = await ExpenseService.GetByIdAsync(expenseId);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات المصروف.");
            return;
        }

        _ApplyExpenseToEditor(result.Value);
    }

    /// <summary>
    /// تعبئة حقول المحرر من نسخة المصروف ثم ضبط الوضع والأزرار — بيت واحد يخدم نقر المستخدم
    /// والتحديد القادم من شاشة أخرى (<see cref="SelectRecord"/>) فلا يتفرّع السلوك.
    /// </summary>
    private void _ApplyExpenseToEditor(ExpenseDto expense)
    {
        _expensesById[expense.Id] = expense;   // مزامنة الفهرس بالنسخة المحدَّثة (بعد نجاح القراءة فقط)
        _editId = expense.Id;
        _loadedStatus = expense.Status;
        _mode = EntryMode.Editing;

        txtExpenseNo.Text = expense.ExpenseNo.ToString();
        dtpDate.Value = expense.ExpenseDate;
        _SelectCashBox(expense.CashBoxId);
        numAmount.Value = NumericInputHelper.Clamp(numAmount, expense.Amount);
        txtNotes.Text = expense.Notes ?? string.Empty;

        _UpdateNetTotal();
        lblStatusValue.Text = string.IsNullOrWhiteSpace(expense.StatusName)
            ? DocumentTextHelper.StatusText(expense.Status)
            : expense.StatusName;

        _ApplyModeState();   // يفعّل/يعطّل الحقول والأزرار ويحدّث شريط الوضع
    }

    /// <summary>اختيار الصندوق المحفوظ في القائمة المنسدلة (بلا طرح استثناء عند غيابه).</summary>
    private void _SelectCashBox(int cashBoxId)
    {
        cmbCashBox.SelectedValue = cashBoxId;

        if (cmbCashBox.SelectedValue is not int selected || selected != cashBoxId)
            cmbCashBox.SelectedIndex = -1;
    }

    private void _SelectGridRow(int expenseId)
    {
        foreach (DataGridViewRow row in dgvMain.Rows)
        {
            if (GridRowHelper.TryGetInt(row, "Id", out int id) && id == expenseId)
            {
                row.Selected = true;

                if (GridRowHelper.Cell(row, "ExpenseNo") is { } cell)
                    dgvMain.CurrentCell = cell;

                break;
            }
        }
    }

    // ── آلة الحالة (RULE-N43 / RULE-N44 / RULE-N149…N152) ─────────────────────

    /// <summary>
    /// آلة الحالة المركزية: تفعيل/تعطيل حقول المحرر، وحالة كل زر، ونص شريط الوضع.
    /// تُستدعى بعد **كل** انتقال: تحميل · «جديد» · اختيار سجل · حفظ · تعديل · حذف · ترحيل · إلغاء · تحديث.
    /// </summary>
    private void _ApplyModeState()
    {
        bool canEdit = _mode == EntryMode.Adding || (_mode == EntryMode.Editing && _IsDraftLoaded());

        if (_mode == EntryMode.Browsing) RequiredFieldHelper.Clear(this);   // لا علامات مطلوبة على حقول معطّلة

        _SetEditorEnabled(canEdit);

        // آلة حالة الأزرار (RULE-N43): «جديد/حفظ» في الإضافة، «تعديل/حذف/ترحيل» على مسودة محددة فقط.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;

        btnSave.Enabled = _mode == EntryMode.Adding;
        btnEdit.Enabled = _mode == EntryMode.Editing && _IsDraftLoaded();
        btnDelete.Enabled = _mode == EntryMode.Editing && _IsDraftLoaded();
        btnPost.Enabled = _mode == EntryMode.Editing && _IsDraftLoaded();
        btnCancelDocument.Enabled = _mode == EntryMode.Editing && _editId is not null && _loadedStatus != StatusCancelled;

        _UpdateModeHeader();

        // RULE-N147: Enter يُنفِّذ الزر الأساسي للوضع الحالي (ويُبرزه كزر افتراضي للشاشة).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): عرض ⇒ «جديد» · إضافة ⇒ «حفظ» · تعديل مسودة ⇒ «تعديل»،
    /// والمستند المرحّل/الملغى ⇒ <c>null</c> كي لا ينفّذ Enter حفظاً مرفوضاً.
    /// يُقرأ عند كل ضغطة وعند كل انتقال حالة (لا يُخزَّن).
    /// </summary>
    private Button? _PrimaryActionButton() => _mode switch
    {
        EntryMode.Adding => btnSave,
        EntryMode.Editing => _IsDraftLoaded() ? btnEdit : null,
        _ => btnNew
    };

    /// <summary>هل السجل المحمَّل مسودة (قابل للتعديل/الترحيل/الحذف)؟</summary>
    private bool _IsDraftLoaded() => _editId is not null && _loadedStatus == StatusDraft;

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند مسودة محددة.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        dtpDate, cmbCashBox, numAmount, txtNotes
    };

    /// <summary>تفعيل أو تعطيل حقول المحرر دفعة واحدة (رقم المصروف يبقى للقراءة دائماً).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة مصروف جديد» · «تعديل مصروف رقم: N» ·
    /// «عرض مصروف رقم: N — مرحل/ملغي (غير قابل للتعديل)».
    /// </summary>
    private void _UpdateModeHeader()
    {
        string state = _mode switch
        {
            EntryMode.Adding => "إضافة مصروف جديد",
            EntryMode.Editing when _IsDraftLoaded() => $"تعديل مصروف رقم: {txtExpenseNo.Text}",
            EntryMode.Editing => $"عرض مصروف رقم: {txtExpenseNo.Text} — {DocumentTextHelper.StatusText(_loadedStatus)} (غير قابل للتعديل)",
            _ => "وضع العرض"
        };

        lblMode.Text = $"{FormTitle} — {state}";
        Text = $"إدارة المصروفات — {state}";
    }

    /// <summary>إجمالي المستند بصيغة <c>N2</c> (RULE-001).</summary>
    private void _UpdateNetTotal() => lblNetTotalValue.Text = clsFormat.Money(numAmount.Value);

    private void numAmount_ValueChanged(object? sender, EventArgs e) => _UpdateNetTotal();

    // ── إعادة الضبط / إدخال جديد ────────────────────────────────────────────────

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض**: تفريغ المحرر، تعطيل كل حقول الإدخال، وإتاحة «جديد» فقط (RULE-N149).
    /// </summary>
    private void _ResetForm()
    {
        _editId = null;
        _mode = EntryMode.Browsing;
        _loadedStatus = StatusDraft;

        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);

        txtExpenseNo.Clear();
        txtNotes.Clear();
        numAmount.Value = 0m;
        dtpDate.Value = DateTime.Today;
        lblStatusValue.Text = "—";

        if (cmbCashBox.Items.Count > 0)
            cmbCashBox.SelectedIndex = -1;

        _UpdateNetTotal();
        dgvMain.ClearSelection();

        _ApplyModeState();
        btnNew.Focus();
    }

    /// <summary>بدء إدخال مصروف جديد (زر «جديد» / F2): تفريغ ثم **تفعيل** الحقول (RULE-N151).</summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        cmbCashBox.Focus();
    }

    // ── الأوامر ─────────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    private async void btnSave_Click(object? sender, EventArgs e) => await _SaveAsync(edit: false);

    private async void btnEdit_Click(object? sender, EventArgs e) => await _SaveAsync(edit: true);

    /// <summary>
    /// حفظ المصروف: <paramref name="edit"/> = false ⇒ إضافة جديدة · true ⇒ تعديل المسودة المحددة.
    /// المصروف المرحّل/الملغى يُحظر حفظه مطابقةً لرفض الخدمة (RULE-N167) كي لا يُوهم المستخدم بحفظ لا يقع.
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        if (edit && _editId is null) return;

        if (edit && _loadedStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن حفظ مصروف مرحّل أو ملغى. المصروفات المرحّلة تُلغى ولا تُعدَّل.");
            return;
        }

        // الحقول المطلوبة: الحكم والعَلم في البيت الواحد RequiredFieldHelper (RULE-N172/RULE-N194) —
        // يُوسَم كل ناقص ثم ينتقل المؤشر إلى أوله.
        if (!RequiredFieldHelper.Validate(this))
        {
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
            return;
        }

        if (!_ValidateAmount()) return;
        if (!_TryGetCashBoxId(out int cashBoxId)) return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
        btnEdit.Enabled = false;

        try
        {
            string? notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            Result<ExpenseDto> result;

            if (edit)
            {
                int id = _editId!.Value;

                result = await ExpenseService.UpdateAsync(id,
                    new UpdateExpenseRequest(dtpDate.Value, cashBoxId, numAmount.Value, notes));

                if (result.IsSuccess) clsDialogs.Success("تم تحديث المصروف بنجاح.");
                else clsDialogs.Error(result.Error ?? "تعذر تحديث المصروف.");
            }
            else
            {
                result = await ExpenseService.CreateAsync(
                    new CreateExpenseRequest(dtpDate.Value, cashBoxId, numAmount.Value, notes));

                if (result.IsSuccess) clsDialogs.Success("تمت إضافة المصروف بنجاح.");
                else clsDialogs.Error(result.Error ?? "تعذر إضافة المصروف.");
            }

            if (result.IsSuccess)
            {
                // بعد الحفظ الناجح: تحديث الشبكة ثم إبقاء السجل المحفوظ محمَّلاً ومحدداً (تحديث تلقائي).
                int savedId = result.Value?.Id ?? _editId ?? 0;

                await _LoadExpensesAsync();

                if (savedId > 0)
                {
                    await _FillFormAsync(savedId);
                    _SelectGridRow(savedId);
                }
                else
                {
                    _ResetForm();
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ المصروف (وضع التعديل: {IsEdit})", edit);
            clsDialogs.Error("تعذر حفظ المصروف. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// حذف المصروف — مسودة غير مرحّلة فقط، بتأكيد صريح (<c>clsDialogs.Confirm</c>):
    /// المستند المرحّل لا يُحذف بل يُلغى (سلوك الشاشة المصدر وحكم الخدمة).
    /// </summary>
    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مصروف من القائمة أولاً.");
            return;
        }

        if (_loadedStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن حذف المصروف إلا في حالة مسودة.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من حذف المصروف رقم \"{txtExpenseNo.Text}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await ExpenseService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم حذف المصروف بنجاح.");

                await _LoadExpensesAsync();
                _ResetForm();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر حذف المصروف.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حذف المصروف {ExpenseId}", id);
            clsDialogs.Error("تعذر حذف المصروف. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>ترحيل المصروف — مسودة فقط، بتأكيد صريح، والأثر المالي من الخدمة (الشاشة لا تحسب).</summary>
    private async void btnPost_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مصروف من القائمة أولاً.");
            return;
        }

        if (_loadedStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن ترحيل المصروف إلا في حالة مسودة.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من ترحيل المصروف رقم \"{txtExpenseNo.Text}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnPost.Enabled = false;

        try
        {
            var result = await ExpenseService.PostAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم ترحيل المصروف بنجاح.");

                await _LoadExpensesAsync();
                await _FillFormAsync(id);   // المستند صار مرحّلاً ⇒ المحرر للعرض فقط
                _SelectGridRow(id);
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر ترحيل المصروف.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء ترحيل المصروف {ExpenseId}", id);
            clsDialogs.Error("تعذر ترحيل المصروف. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// إلغاء المستند المحدد (مسودة أو مرحّل) — بتأكيد صريح. المصروف الملغى بالفعل يُرفض عربياً
    /// قبل نداء الخدمة (تكافؤ رفض الخدمة).
    /// </summary>
    private async void btnCancelDocument_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مصروف من القائمة أولاً.");
            return;
        }

        if (_loadedStatus == StatusCancelled)
        {
            clsDialogs.Warning("السند ملغي بالفعل.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من إلغاء المصروف رقم \"{txtExpenseNo.Text}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnCancelDocument.Enabled = false;

        try
        {
            var result = await ExpenseService.CancelAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم إلغاء المصروف بنجاح.");

                await _LoadExpensesAsync();
                await _FillFormAsync(id);
                _SelectGridRow(id);
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر إلغاء المصروف.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إلغاء المصروف {ExpenseId}", id);
            clsDialogs.Error("تعذر إلغاء المصروف. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← الصناديق ← المصروفات ← المحرر ووضع العرض.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            _ClearFilterInputs();
            await _LoadCashBoxesAsync();
            await _LoadExpensesAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث بيانات المصروفات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // ── التحقق ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// قاعدة **المبلغ &gt; 0** وحدها — ليست قاعدة «حقل فارغ» فلا تنتقل إلى <see cref="RequiredFieldHelper"/>
    /// (RULE-N172: قواعد «مطلوب» في البيت الواحد، وقاعدة المدى/المال تبقى كما كانت في شاشة المصدر).
    /// </summary>
    private bool _ValidateAmount()
    {
        if (numAmount.Value > 0m)
        {
            errorProvider.SetError(numAmount, string.Empty);
            return true;
        }

        errorProvider.SetError(numAmount, "المبلغ يجب أن يكون أكبر من صفر.");
        clsInputFeedback.ShowRejectTooltip(numAmount, "المبلغ يجب أن يكون أكبر من صفر.", toolTip);
        numAmount.Focus();
        return false;
    }

    /// <summary>قراءة آمنة للصندوق المحدد (بلا تحويل صريح قد يرمي استثناء).</summary>
    private bool _TryGetCashBoxId(out int cashBoxId)
    {
        cashBoxId = 0;

        if (cmbCashBox.SelectedValue is int selected && selected > 0)
        {
            cashBoxId = selected;
            return true;
        }

        return false;
    }

    // ── قائمة السياق (RULE-N23) ─────────────────────────────────────────────────

    /// <summary>RULE-N23: العناصر تستدعي نفس أزرار الشاشة — لا منطق مكرر.</summary>
    private async void cmsView_Click(object? sender, EventArgs e)
    {
        try
        {
            if (btnEdit.Enabled)
            {
                btnEdit.PerformClick();
                return;
            }

            await _LoadSelectedRowAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء عرض المصروف من قائمة السياق");
            clsDialogs.Error("تعذر عرض بيانات المصروف المحدد. يرجى إعادة المحاولة.");
        }
    }

    private void cmsPost_Click(object? sender, EventArgs e)
    {
        if (btnPost.Enabled) btnPost.PerformClick();
    }

    private void cmsCancel_Click(object? sender, EventArgs e)
    {
        if (btnCancelDocument.Enabled) btnCancelDocument.PerformClick();
    }

    private void cmsDelete_Click(object? sender, EventArgs e)
    {
        if (btnDelete.Enabled) btnDelete.PerformClick();
    }

    private void cmsRefresh_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();

    /// <summary>
    /// قائمة السياق لا تُعرض بلا سطر محدد، وتُزامَن حالة العناصر مع أزرار الشاشة نفسها (RULE-N43).
    /// </summary>
    private void cmsRows_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (dgvMain.SelectedRows.Count == 0 || dgvMain.CurrentRow is null)
        {
            e.Cancel = true;
            return;
        }

        cmsView.Enabled = true;
        cmsPost.Enabled = btnPost.Enabled;
        cmsCancel.Enabled = btnCancelDocument.Enabled;
        cmsDelete.Enabled = btnDelete.Enabled;
        cmsRefresh.Enabled = btnRefresh.Enabled;
    }
}
