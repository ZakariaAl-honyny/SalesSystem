using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Serilog;

namespace SalesSystem.Desktop.Screens.CustomerReceipts.Forms;

/// <summary>
/// شاشة سندات القبض المدمجة (<c>frm*Master</c>) — النمط المُشهَد لـ<c>frmProductMaster</c>
/// (<c>RULE-N100</c>/<c>RULE-N104</c> · <c>D-01</c>/<c>D-03</c>): محرر المستند + <c>gbFilter</c>
/// + <c>pnlSummary</c> + شبكة القائمة في شاشة واحدة، بدل <c>frmListCustomerReceipts</c> +
/// <c>frmAddUpdateCustomerReceipt</c>.
/// <para>
/// <b>العقد كما هو بلا تعديل (<c>D-04</c>):</b> <c>ICustomerReceiptDesktopService</c> —
/// <c>GetAllAsync</c> · <c>GetByIdAsync</c> · <c>CreateAsync</c> · <c>UpdateAsync</c> ·
/// <c>PostAsync</c> · <c>CancelAsync</c>. <b>لا <c>DeleteAsync</c></b> ⇒ لا زر حذف:
/// إلغاء المستند هو بديل الحذف (<c>RULE-004</c>).
/// </para>
/// <para>
/// <b>آلة الحالة (<c>RULE-N150</c>):</b> <see cref="EntryMode"/> + <c>_ApplyModeState()</c>
/// مركزية تُستدعى بعد كل انتقال. الحقول معطّلة في وضع العرض (<c>RULE-N149</c>)، والسند
/// المرحّل/الملغى **للعرض فقط** — لا حفظ ولا ترحيل، والإلغاء وحده متاح.
/// </para>
/// <para>
/// <b>حدّ الأخطاء واحد (<c>RULE-N205</c>):</b> <c>try/catch</c> في معالجات <c>async void</c>
/// فقط (استثناءها يُسقط التطبيق — <c>RULE-N25</c>)، والدوال <c>async Task</c> الخاصة
/// تُطلق الاستثناء إلى ذلك الحدّ ولا تكرّر حارساً. رسائل <c>Result</c> تُعرض عربية في مكانها.
/// </para>
/// </summary>
public partial class frmCRUDCustomerReceiptsMaster : Form
{
    private readonly IServiceProvider _serviceProvider = null!;

    // ── الخدمات عبر خصائص مطبوعة النوع (RULE-N03/RULE-N04) ────────────────────
    private ICustomerReceiptDesktopService ReceiptService => _serviceProvider.GetRequiredService<ICustomerReceiptDesktopService>();
    private ICustomerDesktopService CustomerService => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
    private ICashBoxDesktopService CashBoxService => _serviceProvider.GetRequiredService<ICashBoxDesktopService>();

    // ── الحالة ────────────────────────────────────────────────────────────────
    /// <summary>كل السندات كما وردت من الخدمة (مصدر التصفية والملخّص).</summary>
    private readonly List<CustomerReceiptDto> _allReceipts = new();

    /// <summary>نتيجة التصفية الحالية — ما تعرضه الشبكة.</summary>
    private List<CustomerReceiptDto> _filteredReceipts = new();

    /// <summary>قوائم الاختيار المحمَّلة (العملاء/الصناديق النشطة فقط).</summary>
    private List<CustomerDto> _customers = new();
    private List<CashBoxDto> _cashBoxes = new();

    /// <summary>null = لا سند محمَّل في المحرر.</summary>
    private int? _editId;

    /// <summary>حالة السند المحمَّل — أساس حظر الحفظ/الترحيل لغير المسودة.</summary>
    private byte _loadedStatus = StatusDraft;

    /// <summary>
    /// طريقة دفع السند المحمَّل: <see cref="UpdateCustomerReceiptRequest"/> يحملها وقيمتها
    /// الافتراضية «نقدي» (1)، فإرسال الافتراضي كان يحوّل سنداً غير نقدي إلى نقدي عند أي تعديل.
    /// </summary>
    private byte _loadedPaymentMethod = 1;

    /// <summary>تعليق التصفية الفورية أثناء التهيئة وإعادة الضبط البرمجية (RULE-N59).</summary>
    private bool _isInitializing = true;

    /// <summary>المدى الافتراضي وقت الإنشاء — يعيده «مسح التصفية» و«تحديث» (RULE-N148).</summary>
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>معرّف السند المطلوب تحديده عند الفتح من شاشة أخرى (تنقّل — RULE-N132).</summary>
    private int? _pendingSelectId;

    /// <summary>هل انتهى التحميل الأول للبيانات؟ أساس تطبيق التحديد المطلوب فوراً أو بعده.</summary>
    private bool _dataLoaded;

    private const string FormTitle = "(( سندات القبض ))";

    private const byte StatusDraft = 1;
    private const byte StatusPosted = 2;
    private const byte StatusCancelled = 3;

    /// <summary>أوضاع الشاشة — آلة الحالة الواحدة (RULE-N150).</summary>
    private enum EntryMode
    {
        /// <summary>وضع العرض: كل الحقول معطّلة، والمتاح «جديد» فقط (RULE-N149).</summary>
        Browsing,

        /// <summary>إضافة سند جديد: الحقول مفعّلة والحفظ متاح.</summary>
        Adding,

        /// <summary>سند محمَّل من الشبكة: تعديل إن كان مسودة، وعرض فقط إن كان مرحّلاً/ملغى.</summary>
        Editing
    }

    /// <summary>منشئ التصميم — لمصمّم Visual Studio فقط (بلا خدمات DI).</summary>
    private frmCRUDCustomerReceiptsMaster()
    {
        InitializeComponent();
    }

    public frmCRUDCustomerReceiptsMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // تصفية افتراضية = هذا الشهر حتى اليوم (RULE-N59) — تُبنى قبل توصيل معالجات القيم.
        _defaultFrom = _StartOfCurrentMonth();
        _defaultTo = DateTime.Today;
        _LoadStatusOptions();
        _SetDateRange(_defaultFrom, _defaultTo);
        _isInitializing = false;

        // RULE-N191/RULE-N193 — Enter داخل لوحة المحرر يمشي بترتيب العرض ثم يُسلِّم التركيز
        // لزر الوضع الحالي بلا تنفيذ. الاستدعاء **ثنائي الوسيط** حصراً (لا معه الأحادي).
        KeyboardHelper.WireEnterAsTab(pnlFields, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);                                                   // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46/RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                    // RULE-N45

        // مظهر التعطيل الموحّد لأزرار الوضع (RULE-N43/RULE-N154).
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnPost, btnCancelDocument, btnClearFilters);

        // الاختصارات الموحّدة (RULE-N47): F2 جديد · F5 تحديث · Esc إلغاء/مسح التصفية.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع الحالي (RULE-N147).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N180…N183 — الحقول المطلوبة تُعلَن هنا فقط، والتقييم/الأيقونة/الومضة في البيت الواحد.
        // ترتيب الاستدعاء ملزم: **بعد** WireFocusHighlight (RULE-N183).
        // العميل بلا شرط: يُعطَّل في وضع التعديل (العقد لا يحمل CustomerId) فتسقط قاعدته تلقائياً.
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbCustomer, "يرجى اختيار العميل."),
            new RequiredField(cmbCashBox, "يرجى اختيار الصندوق."));

        // RULE-N97 — التفقيط الحيّ كتلميح على حقل المبلغ (تلميح مستقل عن تلميحات الأزرار).
        LiveTafqeetHelper.Attach(liveTip, (numAmount, "المبلغ", LiveTafqeetHelper.ReadMode.Money));

        // الثيم الأخضر آخر كتلة (RULE-N50) — بعد RequiredFieldHelper (RULE-N183) وقبل توصيل التصفية.
        ApplyUiTheme();

        // طيّ/إظهار صندوق التصفية (RULE-N63) — يتبنّى btnToggleFilters المُعلَن في المصمّم (RULE-N145).
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── التنقّل من شاشة أخرى (RULE-N132 — تُفتح الشاشة داخل النافذة لا نمطياً) ──

    /// <summary>
    /// يطلب تحديد سند بالمعرّف عند فتح الشاشة من شاشة أخرى (مثل حركات الصندوق):
    /// إن كانت البيانات محمَّلة يُحدَّد السطر فوراً، وإلا يُحفظ الطلب ويُطبَّق بعد انتهاء التحميل الأول.
    /// التحديد يفعل ما يفعله نقر المستخدم تماماً — تعبئة المحرر وضبط الوضع: تعديل للسند المسودة،
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

        if (_allReceipts.FirstOrDefault(r => r.Id == id) is not { } receipt) return;

        _SelectGridRow(id);              // نفس السطر الذي يحدده نقر المستخدم (RULE-N179)
        _ApplyReceiptToEditor(receipt);  // تعبئة المحرر + ضبط الوضع (تعديل/عرض)
    }

    // ── الثيم ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N54): كل قيمة هنا مُعلَنة حرفياً في <c>*.Designer.cs</c>
    /// بالقيمة نفسها (RULE-N134/RULE-N140) — و<see cref="GreenThemeHelper.Apply(Form)"/> **آخر سطر**.
    /// </summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvMain);
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── التحميل ───────────────────────────────────────────────────────────────

    private async void frmCustomerReceiptsMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadLookupsAsync();
            await _LoadReceiptsAsync();
            _ResetForm();

            _dataLoaded = true;          // من هنا يمكن تحديد سند مطلوب من شاشة أخرى
            _ApplyPendingSelection();    // تنقّل من شاشة أخرى (RULE-N132)

            ApplyUiTheme();   // RULE-N50 — آخر خطوة في مسار التحميل
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة سندات القبض");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>«تحديث» (F5) — إعادة تحميل كل بيانات الشاشة وإرجاعها لافتراضياتها (RULE-N148).</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // التصفية تعود للافتراضي
            await _LoadLookupsAsync();     // العملاء/الصناديق قد تتغيّر من شاشاتها
            await _LoadReceiptsAsync();    // إعادة قراءة السندات (يعيد التصفية والملخّص)
            _ResetForm();                  // تفريغ المحرر + وضع العرض + الحقول معطّلة
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث شاشة سندات القبض");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تحميل قوائم العملاء والصناديق (النشطة فقط) وربطها بالكائنات لا بالنصوص.</summary>
    private async Task _LoadLookupsAsync()
    {
        var customers = await CustomerService.GetAllAsync(includeInactive: false);

        if (customers.IsSuccess && customers.Value is not null)
        {
            _customers = customers.Value.Where(c => c.IsActive).ToList();
            _BindCustomers();
        }
        else
        {
            clsDialogs.Error(customers.Error ?? "تعذر تحميل قائمة العملاء.");
        }

        var cashBoxes = await CashBoxService.GetAllAsync();

        if (cashBoxes.IsSuccess && cashBoxes.Value is not null)
        {
            _cashBoxes = cashBoxes.Value.Where(b => b.IsActive).ToList();
            _BindCashBoxes();
        }
        else
        {
            clsDialogs.Error(cashBoxes.Error ?? "تعذر تحميل قائمة الصناديق.");
        }
    }

    /// <summary>ربط العملاء: <c>DisplayMember</c> إلزامي لأن العناصر كائنات لا نصوص.</summary>
    private void _BindCustomers()
    {
        cmbCustomer.DisplayMember = "Name";
        cmbCustomer.ValueMember = "Id";
        cmbCustomer.DataSource = _customers.Select(c => new { c.Id, c.Name }).ToList();
        cmbCustomer.SelectedIndex = -1;
    }

    /// <summary>ربط الصناديق: <c>DisplayMember</c> إلزامي لأن العناصر كائنات لا نصوص.</summary>
    private void _BindCashBoxes()
    {
        cmbCashBox.DisplayMember = "Name";
        cmbCashBox.ValueMember = "Id";
        cmbCashBox.DataSource = _cashBoxes.Select(b => new { b.Id, b.Name }).ToList();
        cmbCashBox.SelectedIndex = -1;
    }

    /// <summary>
    /// قراءة السندات من الخدمة. لا تُفرَّغ الذاكرة إلا في فرع النجاح (RULE: لا تفريغ قبل <c>IsSuccess</c>).
    /// </summary>
    private async Task _LoadReceiptsAsync()
    {
        try
        {
            var result = await ReceiptService.GetAllAsync();

            if (result.IsSuccess && result.Value is not null)
            {
                _allReceipts.Clear();
                _allReceipts.AddRange(result.Value);
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل سندات القبض.");
            }
        }
        finally
        {
            _ApplyFilter();
        }
    }

    // ── التصفية (فورية — بلا زر بحث) ──────────────────────────────────────────

    /// <summary>
    /// RULE-N60: خيارات الحالة من البيت الواحد <see cref="DocumentTextHelper.StatusOptions"/> —
    /// والعنصر المحايد «الكل» وحده محلّي لأنه ليس نص حالة.
    /// </summary>
    private void _LoadStatusOptions()
    {
        cbStatus.Items.Clear();
        cbStatus.Items.Add("الكل");

        foreach (var option in DocumentTextHelper.StatusOptions)
            cbStatus.Items.Add(option.Text);

        cbStatus.SelectedIndex = 0;
    }

    /// <summary>قيمة الحالة المختارة (تُقرأ من البيت الواحد لا من ترتيب العنصر) — null = الكل.</summary>
    private byte? _GetStatusFilter()
    {
        int index = cbStatus.SelectedIndex;
        if (index <= 0 || index > DocumentTextHelper.StatusOptions.Count) return null;

        return DocumentTextHelper.StatusOptions[index - 1].Value;
    }

    private static DateTime _StartOfCurrentMonth()
    {
        var today = DateTime.Today;
        return new DateTime(today.Year, today.Month, 1);
    }

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
    }

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void dtpFrom_ValueChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void dtpTo_ValueChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyUnapplied_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void btnRangeToday_Click(object? sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today, DateTime.Today);
        _ApplyFilter();
    }

    private void btnRangeMonth_Click(object? sender, EventArgs e)
    {
        _SetDateRange(_StartOfCurrentMonth(), DateTime.Today);
        _ApplyFilter();
    }

    private void btnRange30_Click(object? sender, EventArgs e)
    {
        _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);
        _ApplyFilter();
    }

    /// <summary>RULE-N56…N60: تصفية فورية — الحالة + النطاق + غير المخصصة + البحث النصي.</summary>
    private void _ApplyFilter()
    {
        if (_isInitializing) return;

        IEnumerable<CustomerReceiptDto> query = _allReceipts;

        byte? status = _GetStatusFilter();
        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        // نطاق التاريخ شامل اليوم، ويُبدَّل الطرفان بدل إظهار رسالة مع كل تغيير (RULE-N59).
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        query = query.Where(r => r.ReceiptDate.Date >= from && r.ReceiptDate.Date <= to);

        if (chkOnlyUnapplied.Checked)
            query = query.Where(r => _GetUnappliedAmount(r) > 0m);

        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            int filterBy = cbFilterBy.SelectedIndex;
            query = query.Where(r => _MatchesText(r, filterBy, text));
        }

        _filteredReceipts = query.ToList();
        _PopulateGrid();
        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    private static bool _MatchesText(CustomerReceiptDto receipt, int filterBy, string text) => filterBy switch
    {
        1 => receipt.ReceiptNo.ToString().Contains(text, StringComparison.Ordinal),
        2 => (receipt.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase),
        3 => _MatchesAmount(receipt, text),
        _ => (receipt.CustomerName ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase),
    };

    /// <summary>
    /// تصفية المبلغ: قراءة النص بقارئ الأرقام الواحد <c>clsFormat.TryParseMoney</c>
    /// (الفاصل الحيّ والأرقام العربية) — لا نص تنسيق مكتوب في الشاشة (RULE-N169).
    /// </summary>
    private static bool _MatchesAmount(CustomerReceiptDto receipt, string text)
        => clsFormat.TryParseMoney(text, out decimal amount)
            ? receipt.Amount == amount || clsFormat.Money(receipt.Amount).Contains(text, StringComparison.Ordinal)
            : clsFormat.Money(receipt.Amount).Contains(text, StringComparison.Ordinal);

    private static decimal _GetAppliedAmount(CustomerReceiptDto receipt)
        => receipt.Applications?.Where(a => a.IsActive).Sum(a => a.AppliedAmount) ?? 0m;

    private static decimal _GetUnappliedAmount(CustomerReceiptDto receipt)
        => Math.Max(0m, receipt.Amount - _GetAppliedAmount(receipt));

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة لوضعها الافتراضي بلا إعادة تحميل
    /// وبلا لمس المحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        bool wasInitializing = _isInitializing;
        _isInitializing = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Clear();
            cbStatus.SelectedIndex = 0;
            chkOnlyUnapplied.Checked = false;
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = wasInitializing;
        }
    }

    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    /// <summary>RULE-N43/RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkOnlyUnapplied.Checked
            || !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    /// <summary>RULE-N47 + RULE-N59: Esc داخل صندوق التصفية = مسحها، وخارجه = إعادة الشاشة لوضع العرض.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        _ResetForm();
    }

    // ── الشبكة ────────────────────────────────────────────────────────────────

    private void _PopulateGrid()
    {
        dgvMain.Rows.Clear();

        foreach (var receipt in _filteredReceipts)
        {
            int rowIndex = dgvMain.Rows.Add(
                receipt.ReceiptNo,
                clsFormat.DateToDayMonthYear(receipt.ReceiptDate),
                receipt.CustomerName ?? string.Empty,
                clsFormat.Money(receipt.Amount),
                receipt.PaymentMethodName ?? string.Empty,
                receipt.CashBoxName ?? string.Empty,
                receipt.StatusName ?? DocumentTextHelper.StatusText(receipt.Status),
                receipt.Notes ?? string.Empty);

            // الربط بالمعرّف لا بترتيب السطر — القراءة من Tag لا من أسماء أعمدة منطقية (RULE-N179).
            dgvMain.Rows[rowIndex].Tag = receipt.Id;
        }

        _UpdateEmptyState();
    }

    /// <summary>RULE-N22: «لا توجد بيانات» يُعلن فوق الشبكة عند فراغ نتيجة التصفية.</summary>
    private void _UpdateEmptyState()
    {
        if (lblEmptyState is null) return;

        bool isEmpty = _filteredReceipts.Count == 0;

        lblEmptyState.Visible = isEmpty;
        if (isEmpty) lblEmptyState.BringToFront();
    }

    /// <summary>RULE-N61: شريط الملخص — المعروض/الإجمالي/غير المخصص/المرحّل/الملغى.</summary>
    private void _UpdateSummary()
    {
        int shown = _filteredReceipts.Count;
        decimal total = _filteredReceipts.Sum(r => r.Amount);
        decimal unapplied = _filteredReceipts.Sum(_GetUnappliedAmount);
        int posted = _filteredReceipts.Count(r => r.Status == StatusPosted);
        int cancelled = _filteredReceipts.Count(r => r.Status == StatusCancelled);

        lblSummary.Text =
            $"المعروض: {clsFormat.Number(shown)} من {clsFormat.Number(_allReceipts.Count)}"
            + $" — الإجمالي: {clsFormat.Money(total)}"
            + $" · غير المخصص: {clsFormat.Money(unapplied)}"
            + $" · مرحّل: {clsFormat.Number(posted)}"
            + $" · ملغى: {clsFormat.Number(cancelled)}";

        if (shown == 0) lblSummary.Text += " — لا توجد بيانات مطابقة";
    }

    /// <summary>تحديد سطر السجل بعد التحديث التلقائي — بلا قراءة خلية باسم حقل منطقي (RULE-N179).</summary>
    private void _SelectGridRow(int receiptId)
    {
        foreach (DataGridViewRow row in dgvMain.Rows)
        {
            if (row.Tag is not int rowId || rowId != receiptId) continue;

            row.Selected = true;
            if (GridRowHelper.Cell(row, "colReceiptNo") is { } cell) dgvMain.CurrentCell = cell;
            break;
        }
    }

    private async void dgvMain_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgvMain.Rows[e.RowIndex].Tag is not int receiptId || receiptId <= 0) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            await _FillFormAsync(receiptId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار سند قبض من الشبكة {ReceiptId}", receiptId);
            clsDialogs.Error("تعذر عرض بيانات السند المحدد. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>النقر المزدوج ينقل التركيز إلى أول إجراء متاح في المحرر (بعد تحميل السطر بنقرة).</summary>
    private void dgvMain_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (btnSave.Enabled) numAmount.Focus();
        else if (btnPost.Enabled) btnPost.Focus();
    }

    // ── تحميل السجل في المحرر ─────────────────────────────────────────────────

    /// <summary>جلب السند بالمعرّف من الخدمة ثم تعبئة المحرر (RULE-N04 — العقد فقط).</summary>
    private async Task _FillFormAsync(int receiptId)
    {
        var result = await ReceiptService.GetByIdAsync(receiptId);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات السند المحدد.");
            return;
        }

        _ApplyReceiptToEditor(result.Value);
    }

    private void _ApplyReceiptToEditor(CustomerReceiptDto receipt)
    {
        _editId = receipt.Id;
        _loadedStatus = receipt.Status;
        _loadedPaymentMethod = receipt.PaymentMethod;

        txtReceiptNo.Text = receipt.ReceiptNo.ToString();
        dtpDate.Value = receipt.ReceiptDate.Date;
        _SelectCustomer(receipt.CustomerId);
        _SelectCashBox(receipt.CashBoxId);
        numAmount.Value = NumericInputHelper.Clamp(numAmount, receipt.Amount);
        txtNotes.Text = receipt.Notes ?? string.Empty;
        lblPaymentMethodValue.Text = string.IsNullOrWhiteSpace(receipt.PaymentMethodName) ? "—" : receipt.PaymentMethodName;

        _mode = EntryMode.Editing;   // RULE-N44: اختيار سند = وضع التعديل (أو عرض للقراءة)
        _UpdateAmountValue();
        _ApplyModeState();
    }

    private void _SelectCustomer(int customerId)
    {
        cmbCustomer.SelectedValue = customerId;

        if (cmbCustomer.SelectedValue is not int selected || selected != customerId)
            cmbCustomer.SelectedIndex = -1;
    }

    private void _SelectCashBox(int cashBoxId)
    {
        cmbCashBox.SelectedValue = cashBoxId;

        if (cmbCashBox.SelectedValue is not int selected || selected != cashBoxId)
            cmbCashBox.SelectedIndex = -1;
    }

    /// <summary>إجمالي السند بصيغة <c>N2</c> (RULE-001) — القراءة عبر <c>clsFormat</c> لا تنسيق محلي.</summary>
    private void _UpdateAmountValue() => lblAmountValue.Text = clsFormat.Money(numAmount.Value);

    private void numAmount_ValueChanged(object? sender, EventArgs e) => _UpdateAmountValue();

    // ── آلة الحالة (RULE-N150) ────────────────────────────────────────────────

    /// <summary>
    /// آلة الحالة المركزية: تفعيل/تعطيل حقول المحرر، وحالة كل زر، ونص شريط الوضع،
    /// و<see cref="Form.AcceptButton"/>. تُستدعى بعد **كل** انتقال: تحميل · جديد · اختيار سند ·
    /// حفظ · ترحيل · إلغاء · تحديث · Esc — ولا يُضبط <c>Enabled</c> متفرقاً في المعالجات.
    /// </summary>
    private void _ApplyModeState()
    {
        bool editingSession = _mode != EntryMode.Browsing;

        // سند قابل للتحرير: إضافة جديدة، أو تعديل سند ما زال مسودة (المرحّل/الملغى للعرض فقط).
        bool editable = _mode == EntryMode.Adding
            || (_mode == EntryMode.Editing && _loadedStatus == StatusDraft);

        if (!editingSession) RequiredFieldHelper.Clear(this);   // وضع العرض: لا علامات على حقول معطّلة

        _SetEditorEnabled(editable);

        if (editable)
        {
            // عقد التعديل لا يحمل CustomerId ولا ReceiptDate ⇒ يُقفلان في التعديل
            // فلا يوهم المستخدم بتغيير لا يُرسَل ولا يُحفظ (سلوك الشاشة المصدر).
            cmbCustomer.Enabled = _mode == EntryMode.Adding;
            dtpDate.Enabled = _mode == EntryMode.Adding;
        }

        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnSave.Enabled = editable;
        btnPost.Enabled = _editId is not null && _loadedStatus == StatusDraft;
        btnCancelDocument.Enabled = _editId is not null && _loadedStatus != StatusCancelled;

        btnSave.Text = _mode == EntryMode.Adding ? "💾 إضافة" : "💾 حفظ";

        _UpdateModeHeader();                 // RULE-N44
        AcceptButton = _PrimaryActionButton(); // RULE-N147
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): عرض ⇒ «جديد» · إضافة ⇒ «حفظ» ·
    /// تعديل سند مسودة ⇒ «حفظ» · تعديل سند مرحّل/ملغى ⇒ <c>null</c> (لا حفظ على مستند غير مسودة).
    /// </summary>
    private Button? _PrimaryActionButton() => _mode switch
    {
        EntryMode.Adding => btnSave,
        EntryMode.Editing => _loadedStatus == StatusDraft ? btnSave : null,
        _ => btnNew
    };

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند تحرير مسودة.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        dtpDate, cmbCustomer, cmbCashBox, numAmount, txtNotes
    };

    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>شريط الوضع (RULE-N44): وضع العرض · سند جديد · تعديل رقم … · عرض (غير قابل للتعديل).</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — سند قبض جديد",
            EntryMode.Editing when _loadedStatus == StatusDraft
                => $"{FormTitle} — تعديل سند قبض رقم: {txtReceiptNo.Text}",
            EntryMode.Editing
                => $"{FormTitle} — عرض سند قبض رقم: {txtReceiptNo.Text} — {DocumentTextHelper.StatusText(_loadedStatus)} (غير قابل للتعديل)",
            _ => $"{FormTitle} — وضع العرض"
        };
    }

    // ── إعادة الضبط / سند جديد ────────────────────────────────────────────────

    /// <summary>إعادة الشاشة إلى وضع العرض الافتراضي: تفريغ المحرر + تعطيل الحقول (RULE-N149).</summary>
    private void _ResetForm()
    {
        _editId = null;
        _mode = EntryMode.Browsing;
        _loadedStatus = StatusDraft;
        _loadedPaymentMethod = 1;

        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);

        txtReceiptNo.Clear();
        dtpDate.Value = DateTime.Today;
        numAmount.Value = 0m;
        txtNotes.Clear();
        lblPaymentMethodValue.Text = "—";

        if (cmbCustomer.Items.Count > 0) cmbCustomer.SelectedIndex = -1;
        if (cmbCashBox.Items.Count > 0) cmbCashBox.SelectedIndex = -1;

        _UpdateAmountValue();
        dgvMain.ClearSelection();

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>بدء سند جديد (زر «جديد» / F2): تفريغ ثم تفعيل الحقول (RULE-N149).</summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        cmbCustomer.Focus();
    }

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── الحفظ (إضافة/تعديل) ───────────────────────────────────────────────────

    /// <summary>
    /// حفظ السند: وضع الإضافة ⇒ <c>CreateAsync</c>؛ وضع التعديل (سند مسودة) ⇒ <c>UpdateAsync</c>.
    /// <para>
    /// <b>قواعد المال والتحقق:</b> الحكم على الحقول المطلوبة في البيت الواحد
    /// <see cref="RequiredFieldHelper"/>، وقاعدة «المبلغ &gt; صفر» (RULE-001) تبقى هنا لأنها
    /// ليست حكم «مطلوب» — وبها ومضة الرفض من <c>clsInputFeedback</c> وحدها.
    /// </para>
    /// </summary>
    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_mode == EntryMode.Browsing) return;

        if (_mode == EntryMode.Editing && _loadedStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن حفظ سند قبض مرحّل أو ملغى. السندات المرحّلة تُلغى ولا تُعدَّل.");
            return;
        }

        if (!_ValidateEditor()) return;

        if (_TryGetSelectedId(cmbCashBox) is not int cashBoxId) return;   // الرسالة ظهرت من RequiredFieldHelper

        bool isNew = _mode == EntryMode.Adding;
        int? customerId = _TryGetSelectedId(cmbCustomer);
        if (isNew && customerId is null) return;

        if (!isNew && !clsDialogs.ConfirmEdit($"هل أنت متأكد من تعديل سند القبض رقم {txtReceiptNo.Text}؟"))
            return;

        string? notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();
        decimal amount = numAmount.Value;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            if (isNew)
            {
                var created = await ReceiptService.CreateAsync(
                    new CreateCustomerReceiptRequest(customerId!.Value, cashBoxId, amount, notes));

                if (created.IsSuccess && created.Value is not null)
                {
                    clsDialogs.Success("تمت إضافة سند القبض بنجاح.");
                    await _ReloadAndSelectAsync(created.Value.Id);
                }
                else
                {
                    clsDialogs.Error($"لم يتم إضافة سند القبض.\n{created.Error}");
                }
            }
            else
            {
                int id = _editId!.Value;

                // طريقة الدفع المحمَّلة تُعاد كما هي (العقد قيمته الافتراضية «نقدي»).
                var updated = await ReceiptService.UpdateAsync(id,
                    new UpdateCustomerReceiptRequest(cashBoxId, amount, notes, _loadedPaymentMethod));

                if (updated.IsSuccess)
                {
                    clsDialogs.Success("تم تحديث سند القبض بنجاح.");
                    await _ReloadAndSelectAsync(id);
                }
                else
                {
                    clsDialogs.Error($"لم يتم تحديث سند القبض.\n{updated.Error}");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ سند القبض (وضع الإضافة: {IsNew})", isNew);
            clsDialogs.Error("تعذر حفظ سند القبض. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// تحقق المحرر: (1) الحقول المطلوبة من البيت الواحد، (2) قاعدة المال «المبلغ &gt; صفر».
    /// </summary>
    private bool _ValidateEditor()
    {
        bool isValid = RequiredFieldHelper.Validate(this);

        if (!isValid && RequiredFieldHelper.FirstMissing(this) is { } firstMissing)
            firstMissing.Focus();

        // قاعدة المال (RULE-001) — ليست حكم «مطلوب»، فلا تُعلَن في RequiredFieldHelper (RULE-N172).
        if (numAmount.Value <= 0m)
        {
            errorProvider.SetError(numAmount, "المبلغ يجب أن يكون أكبر من صفر.");
            clsInputFeedback.ShowRejectTooltip(numAmount, "المبلغ يجب أن يكون أكبر من صفر.", liveTip);

            if (isValid) numAmount.Focus();
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numAmount, string.Empty);
        }

        return isValid;
    }

    // ── الترحيل وإلغاء المستند ────────────────────────────────────────────────

    /// <summary>ترحيل السند المسودة عبر العقد — الشاشة لا تحسب أثراً مالياً، تُظهر <c>Result</c> فقط.</summary>
    private async void btnPost_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار سند من القائمة أولاً.");
            return;
        }

        if (_loadedStatus != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن ترحيل السند إلا في حالة مسودة.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من ترحيل سند القبض رقم {txtReceiptNo.Text}؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnPost.Enabled = false;

        try
        {
            var result = await ReceiptService.PostAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم ترحيل سند القبض بنجاح.");
                await _ReloadAndSelectAsync(id);
            }
            else
            {
                clsDialogs.Error($"لم يتم ترحيل سند القبض.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء ترحيل سند القبض {ReceiptId}", id);
            clsDialogs.Error("تعذر ترحيل سند القبض. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>إلغاء المستند عبر العقد — بديل الحذف (العقد لا يحمل <c>DeleteAsync</c>).</summary>
    private async void btnCancelDocument_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار سند من القائمة أولاً.");
            return;
        }

        if (_loadedStatus == StatusCancelled)
        {
            clsDialogs.Warning("السند ملغي بالفعل.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من إلغاء سند القبض رقم {txtReceiptNo.Text}؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnCancelDocument.Enabled = false;

        try
        {
            var result = await ReceiptService.CancelAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم إلغاء سند القبض بنجاح.");
                await _ReloadAndSelectAsync(id);
            }
            else
            {
                clsDialogs.Error($"لم يتم إلغاء سند القبض.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إلغاء سند القبض {ReceiptId}", id);
            clsDialogs.Error("تعذر إلغاء سند القبض. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تحديث تلقائي بعد أي كتابة ناجحة: إعادة قراءة الشبكة + إعادة تعبئة المحرر + تحديد السطر.</summary>
    private async Task _ReloadAndSelectAsync(int receiptId)
    {
        await _LoadReceiptsAsync();
        await _FillFormAsync(receiptId);
        _SelectGridRow(receiptId);
        _ApplyModeState();
    }

    // ── قائمة السياق (RULE-N23) ───────────────────────────────────────────────

    /// <summary>عناصر القائمة تستدعي نفس أزرار الشاشة — لا منطق مكرر.</summary>
    private void cmsPost_Click(object? sender, EventArgs e)
    {
        if (btnPost.Enabled) btnPost.PerformClick();
    }

    private void cmsCancelDocument_Click(object? sender, EventArgs e)
    {
        if (btnCancelDocument.Enabled) btnCancelDocument.PerformClick();
    }

    private void cmsRefresh_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();

    private async void cmsEdit_Click(object? sender, EventArgs e)
    {
        if (dgvMain.CurrentRow?.Tag is not int receiptId || receiptId <= 0) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            await _FillFormAsync(receiptId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تحميل السند المحدد {ReceiptId}", receiptId);
            clsDialogs.Error("تعذر تحميل بيانات السند المحدد. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>القائمة لا تُعرض بلا سطر محدد، وعناصرها تُزامَن مع أزرار الشاشة (RULE-N43).</summary>
    private void cmsRows_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        var row = dgvMain.CurrentRow;

        if (row is null || dgvMain.SelectedRows.Count == 0)
        {
            e.Cancel = true;
            return;
        }

        cmsEdit.Enabled = row.Tag is int;
        cmsPost.Enabled = btnPost.Enabled;
        cmsCancelDocument.Enabled = btnCancelDocument.Enabled;
        cmsRefresh.Enabled = btnRefresh.Enabled;
    }

    // ── أدوات ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// معرّف العنصر المختار في قائمة منسدلة (بلا تحويل صريح قد يرمي على معرّف <c>short</c>).
    /// </summary>
    private static int? _TryGetSelectedId(ComboBox combo) => combo.SelectedValue switch
    {
        int id => id > 0 ? id : null,
        short shortId => shortId > 0 ? shortId : null,
        byte byteId => byteId > 0 ? byteId : null,
        long longId when longId is > 0 and <= int.MaxValue => (int)longId,
        _ => null
    };
}
