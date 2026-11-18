using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Models;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

/// <summary>
/// شاشة مرتجعات المبيعات المدمجة (Master) — تحلّ محل <c>frmSalesReturn</c> + قائمة المرتجعات:
/// محرر المستند (رأس + شبكة سطور + إجمالي) وأوامر الشاشة وصندوق تصفية وشبكة عرض وشريط ملخص
/// في شاشة واحدة، بالنمط المُشهَد <c>frmProductMaster</c> (<c>F-004</c> · <c>D-01</c> · <c>D-02</c>).
/// <para>
/// <b>لا وضع تعديل (<c>D-05</c>):</b> عقد <see cref="ISalesReturnDesktopService"/> يعرّض
/// <c>Create</c> · <c>Post</c> · <c>Cancel</c> فقط — لا <c>UpdateAsync</c>. فاختيار مرتجع قائم
/// = <see cref="EntryMode.Editing"/> <b>للقراءة فقط</b> مع «ترحيل» (مسودة) و«إلغاء المستند»،
/// ولا يوجد زر حفظ يوهم بالتعديل.
/// </para>
/// <para>
/// <b>السلوك محفوظ من المصدر:</b> العميل (بعنصر «عميل نقدي») والتاريخ والمستودع والملاحظات،
/// وسطور بالباركود/التحرير داخل الخلية، ومساران للحفظ («حفظ مسودة» ⇒ <c>CreateAsync</c>
/// و«ترحيل» ⇒ <c>CreateAsync</c> ثم <c>PostAsync</c>)، ومنع تكرار المستند عبر <c>_createdId</c>.
/// </para>
/// <para>
/// <b>المكان الواحد (<c>RULE-N169</c>…<c>RULE-N172</c>):</b> التنقل والثيم والتغذية والتصفية
/// والرسائل والتنسيق ونصوص المستند من بيوتها القائمة — لا نسخة محلية من أي سلوك مشترك.
/// </para>
/// </summary>
public partial class frmSalesReturnMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── العقود المستهلَكة حرفياً (RULE-N03/N04) ────────────────────────────────
    private ISalesReturnDesktopService ReturnService => _serviceProvider.GetRequiredService<ISalesReturnDesktopService>();
    private ICustomerDesktopService CustomerService => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
    private IProductDesktopService ProductService => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService WarehouseService => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── حالة المستند (RULE-019 — Draft = 1 · Posted = 2 · Cancelled = 3) ───────
    private const byte StatusDraft = 1;
    private const byte StatusPosted = 2;
    private const byte StatusCancelled = 3;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( مرتجعات المبيعات ))";

    /// <summary>
    /// معرّف عنصر «عميل نقدي» في قائمة العملاء. **ليس صفراً** لأن
    /// <see cref="RequiredFieldHelper"/> يعدّ الصفر الرقمي في القائمة سطراً نائباً فارغاً —
    /// والعميل النقدي اختيار مشروع (يُرسَل <c>CustomerId = null</c>)، فلا يجوز أن يُرفض كحقل فارغ.
    /// </summary>
    private const int CashCustomerId = -1;

    // ── حالة المحرر ────────────────────────────────────────────────────────────
    private readonly BindingList<InvoiceLineItem> _lines = new();
    private readonly List<ProductDto> _allProducts = new();

    /// <summary>المرتجع المحمَّل في المحرر (null = لا مستند — وضع العرض أو إضافة جديدة).</summary>
    private SalesReturnDto? _invoice;

    /// <summary>
    /// معرّف المرتجع الذي أنشأته الشاشة فعلاً (أو المُحمَّل من القائمة): يمنع إنشاء مستند ثانٍ
    /// عند إعادة الضغط على «حفظ/ترحيل»، ويُصفَّر في <see cref="_ResetForm"/>.
    /// </summary>
    private int? _createdId;

    /// <summary>حالة المستند المحمَّل — أساس تفعيل «ترحيل» و«إلغاء المستند».</summary>
    private byte _loadedStatus = StatusDraft;

    /// <summary>يمنع معالجة تحرير الخلية أثناء تعبئة السطور برمجياً.</summary>
    private bool _isBinding;

    /// <summary>يمنع انطلاق تصفية أثناء تهيئة أدوات التصفية في المُنشئ/التحميل.</summary>
    private bool _isInitializing = true;

    // ── بيانات القائمة + التصفية ────────────────────────────────────────────────
    private List<SalesReturnDto> _allReturns = new();
    private List<SalesReturnDto> _filteredReturns = new();

    // RULE-N148: الوضع الافتراضي لأدوات التصفية («هذا الشهر») — يُلتقط مرة عند الإنشاء.
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    /// <summary>حالة الشاشة (آلة الحالة) — تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>.</summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N43/RULE-N44).</summary>
    private enum EntryMode
    {
        /// <summary>عرض فقط: المحرر والسطور معطّلة حتى يضغط المستخدم «جديد» أو يحدّد مستنداً (RULE-N149).</summary>
        Browsing,

        /// <summary>إضافة مرتجع جديد: الرأس والسطور مفعّلة، و«حفظ مسودة»/«ترحيل» متاحان.</summary>
        Adding,

        /// <summary>مرتجع قائم: عرض للقراءة فقط (لا <c>UpdateAsync</c>) مع «ترحيل»/«إلغاء المستند» (D-05).</summary>
        Editing
    }

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmSalesReturnMaster()
    {
        InitializeComponent();
    }

    public frmSalesReturnMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        // RULE-N191/N193 — Enter داخل لوحة المحرر يمشي بترتيب العرض ثم يُسلِّم التركيز لزر الوضع بلا تنفيذ.
        // استدعاء **ثنائي الوسيط فقط**: الأحادي يحجز الحقول قبله فيصير «الأول يفوز» صامتاً.
        KeyboardHelper.WireEnterAsTab(pnlFields, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);                                                       // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));  // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45

        // RULE-N43 — الأزرار المعطّلة تظهر رمادية باهتة (لا Enabled = false فقط).
        KeyboardHelper.WireButtonDisabledVisual(btnSaveDraft, btnPost, btnCancelDocument, btnAddItem, btnRemoveItem, btnClearFilters);

        // RULE-N47 — الاختصارات الموحّدة: F2 جديد · F4 ترحيل · F5 تحديث · Delete حذف السطر · Esc مسح التصفية/إلغاء.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F4, _PostIfAllowed),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDeleteLine),
            (Keys.Escape, _GlobalEscape));

        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);                                        // RULE-N147

        // RULE-N180…N183 + RULE-N194 — تغذية الحقول المطلوبة حيّاً (أيقونة ✗ + رسالة + ومضة رفض).
        // ترتيب الاستدعاء إلزامي: **بعد** WireFocusHighlight (ترتيب اشتراك Leave) و**قبل** الثيم (RULE-N183).
        // الشاشة تُعلن قواعدها فقط — لا أيقونة ولا لون هنا.
        RequiredFieldHelper.Attach(this, errorProvider,
            new(cmbCustomer, "يرجى اختيار العميل"),
            new(cmbWarehouse, "يرجى اختيار المستودع"));

        _LoadStatusOptions();      // نصوص الحالة من البيت الواحد DocumentTextHelper (RULE-N169)
        _SetupToolTips();          // RULE-N47 — التلميحات مع اختصاراتها

        FilterPanelHelper.Attach(this, _serviceProvider);   // يتبنّى btnToggleFilters المُعلَن في المصمّم

        _SetupLineGrid();          // RULE-N22a — سلوك شبكة السطور تعلنه الشاشة (RULE-N52)

        ApplyUiTheme();            // ينتهي بـ GreenThemeHelper.Apply(this) كآخر سطر (RULE-N50)
    }

    // ── الثيم والتلميحات ────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53): كل لون هنا **مُعلَن بقيمته الحرفية** في
    /// <c>frmSalesReturnMaster.Designer.cs</c> (RULE-N134/N140) — والمصمّم هو المرجع البصري.
    /// </summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);      // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvItems);       // RULE-N51/N52 — سلوك الشبكة تعلنه الشاشة
        GreenThemeHelper.ApplyGrid(dgvMain);

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblTotals.ForeColor = GreenThemeHelper.TextStrong;
        lblStatusValue.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this);               // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الأزرار والحقول مع اختصاراتها (RULE-N47) — التلميح المُعلَن في المصمّم (RULE-N145).</summary>
    private void _SetupToolTips()
    {
        toolTip.SetToolTip(btnNew, "مرتجع مبيعات جديد (F2)");
        toolTip.SetToolTip(btnSaveDraft, "حفظ المرتجع كمسودة — لا يُنشأ مستند ثانٍ بعد الحفظ");
        toolTip.SetToolTip(btnPost, "ترحيل المرتجع وزيادة المخزون (F4)");
        toolTip.SetToolTip(btnCancelDocument, "إلغاء المستند المحدد (مسودة أو مرحّل)");
        toolTip.SetToolTip(btnRefresh, "إعادة تحميل البيانات وإرجاع التصفية للافتراضي (F5)");
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");

        toolTip.SetToolTip(btnAddItem, "إضافة سطر صنف جديد للشبكة");
        toolTip.SetToolTip(btnRemoveItem, "حذف السطر المحدد من الشبكة (Delete)");
        toolTip.SetToolTip(dgvItems, "أدخل الباركود في عمود «الكود/الباركود» ثم الكمية والسعر — Enter لإدخال سطر");

        toolTip.SetToolTip(dtpDate, "تاريخ المرتجع");
        toolTip.SetToolTip(cmbCustomer, "العميل صاحب المرتجع (إلزامي — «عميل نقدي» مسموح)");
        toolTip.SetToolTip(cmbWarehouse, "المستودع الذي يدخل إليه المرتجع (إلزامي)");
        toolTip.SetToolTip(txtNotes, "ملاحظات الإرجاع");

        toolTip.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
        toolTip.SetToolTip(cbStatus, "تصفية بحالة المستند");
        toolTip.SetToolTip(chkOnlyCancelled, "إظهار المرتجعات الملغاة فقط");
        toolTip.SetToolTip(btnRangeToday, "تصفية مرتجعات اليوم فقط");
        toolTip.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip.SetToolTip(btnRange30, "آخر 30 يوماً");
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
    private async void frmSalesReturnMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // تصفية بحالتها الافتراضية (RULE-N148)
            _isInitializing = false;       // من هنا تصبح التصفية حيّة

            await _LoadLookupsAsync();     // العملاء/المستودعات (RULE-N33: DisplayMember لازم لكائنات) + الأصناف
            await _LoadReturnsAsync();     // يبني الشبكة ويطبّق التصفية والملخص

            _ResetForm();                  // المحرر معطّل + وضع العرض (RULE-N149)
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة مرتجعات المبيعات");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            _isInitializing = false;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// تحميل القوائم المنسدلة والأصناف. <c>includeInactive: true</c> لأن مرتجعاً قديماً لعميل/مستودع
    /// مُعطَّل يجب أن يُربط بجهته الصحيحة لا بأول عنصر في القائمة.
    /// </summary>
    private async Task _LoadLookupsAsync()
    {
        var warehouseResult = await WarehouseService.GetAllAsync(includeInactive: true);

        if (warehouseResult.IsSuccess && warehouseResult.Value != null)
        {
            cmbWarehouse.DisplayMember = "Name";   // RULE-N33 — كائنات بلا DisplayMember تُعرض كنص النوع
            cmbWarehouse.ValueMember = "Id";       // WarehouseDto.Id = short (يُقرأ بـ is short لا بتحويل صريح)
            cmbWarehouse.DataSource = warehouseResult.Value.ToList();

            if (cmbWarehouse.SelectedIndex < 0 && cmbWarehouse.Items.Count > 0)
                cmbWarehouse.SelectedIndex = 0;
        }
        else
        {
            clsDialogs.Error(warehouseResult.Error ?? "تعذر تحميل قائمة المستودعات.");
        }

        var customerResult = await CustomerService.GetAllAsync(includeInactive: true);

        if (customerResult.IsSuccess && customerResult.Value != null)
        {
            var customers = customerResult.Value.ToList();

            // نص «عميل نقدي» من بيته الواحد (RULE-N169) لا حرفياً داخل الشاشة.
            customers.Insert(0, new CustomerDto(CashCustomerId, DocumentTextHelper.CashCustomerName, "", "", "", "", 0, true, 0));

            cmbCustomer.DisplayMember = "Name";
            cmbCustomer.ValueMember = "Id";
            cmbCustomer.DataSource = customers;

            if (cmbCustomer.SelectedIndex < 0 && cmbCustomer.Items.Count > 0)
                cmbCustomer.SelectedIndex = 0;   // العميل النقدي افتراضياً — كما في الشاشة المصدر
        }
        else
        {
            clsDialogs.Error(customerResult.Error ?? "تعذر تحميل قائمة العملاء.");
        }

        var productResult = await ProductService.GetAllAsync();

        if (productResult.IsSuccess && productResult.Value != null)
        {
            _allProducts.Clear();
            _allProducts.AddRange(productResult.Value);
        }
        else
        {
            clsDialogs.Error(productResult.Error ?? "تعذر تحميل قائمة الأصناف.");
        }
    }

    /// <summary>
    /// تحميل المرتجعات من الخدمة. **لا تُفرَّغ الذاكرة عند فشل التحميل** — التفريغ داخل فرع
    /// <c>IsSuccess</c> فقط (الخطأ لا يمحو البيانات المعروضة).
    /// </summary>
    private async Task _LoadReturnsAsync()
    {
        var result = await ReturnService.GetAllAsync();

        if (result.IsSuccess && result.Value != null)
        {
            _allReturns = result.Value.ToList();
        }
        else
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة مرتجعات المبيعات.");
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

    /// <summary>RULE-N47: Esc داخل صندوق التصفية = مسحها، وإلا إلغاء تحرير الخلية، وإلا الرجوع لوضع العرض.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        // لا إلغاء للمستند أثناء تحرير خلية داخل الشبكة: التحرير أولاً (RULE-N22a).
        if (dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            return;
        }

        _ResetForm();
    }

    /// <summary>F4 = ترحيل — من زر ظاهر ومفعّل فقط (RULE-N47)، ولا ترحيل من داخل شبكة السطور (RULE-N153).</summary>
    private void _PostIfAllowed()
    {
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return;
        if (btnPost is { Visible: true, Enabled: true }) btnPost.PerformClick();
    }

    /// <summary>Delete: حذف السطر المحدد — من داخل شبكة السطور فقط وبلا تحرير جارٍ.</summary>
    private void _GlobalDeleteLine()
    {
        if (dgvItems.IsCurrentCellInEditMode) return;
        if (dgvItems.ContainsFocus && btnRemoveItem.Enabled) btnRemoveItem.PerformClick();
    }

    /// <summary>
    /// التصفية الفورية: النطاق الزمني (بمقارنة <c>.Date</c> — شامل اليوم) + الحالة + «الملغاة فقط» +
    /// نص التصفية على الحقل المختار. لا يوجد زر بحث (RULE-N59)، والمقارنة بـ <c>SelectedIndex</c>
    /// لا بنص عربي (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_isInitializing) return;

        IEnumerable<SalesReturnDto> query = _allReturns;

        // 1) النطاق الزمني — نطاق معكوس = نُبادل الطرفين بدل إسقاط النتائج.
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to)
            (from, to) = (to, from);

        query = query.Where(x => x.ReturnDate.Date >= from && x.ReturnDate.Date <= to);

        // 2) الحالة — 0 = الكل، 1 = مسودة، 2 = مرحّل، 3 = ملغى.
        byte? status = _GetStatusFilterValue();
        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        // 3) «الملغاة فقط» (RULE-N58).
        if (chkOnlyCancelled.Checked)
            query = query.Where(x => x.Status == StatusCancelled);

        // 4) نص التصفية على الحقل المختار (أعمدة معروفة فقط — RULE-N60).
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            query = cbFilterBy.SelectedIndex switch
            {
                1 => query.Where(x => x.ReturnNo.ToString().Contains(text, StringComparison.Ordinal)),
                2 => query.Where(x => (x.Notes ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)
                                   || (x.ReturnReason ?? string.Empty).Contains(text, StringComparison.OrdinalIgnoreCase)),
                3 => _FilterByAmount(query, text),
                _ => query.Where(x => DocumentTextHelper.CashCustomerNameOr(x.CustomerName)
                                        .Contains(text, StringComparison.OrdinalIgnoreCase)),
            };
        }

        _filteredReturns = query.ToList();
        _PopulateGrid();
        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    /// <summary>
    /// تصفية المبلغ: قراءة النص بقارئ الأرقام الواحد <c>clsFormat.TryParseMoney</c> (يتكفّل بالفاصل
    /// الحيّ والأرقام العربية) — لا طرح فواصل يدوياً ولا <c>decimal.TryParse</c> محلي (RULE-N169).
    /// </summary>
    private static IEnumerable<SalesReturnDto> _FilterByAmount(IEnumerable<SalesReturnDto> query, string text)
    {
        if (clsFormat.TryParseMoney(text, out decimal amount))
            return query.Where(x => x.TotalAmount == amount);

        return query.Where(x => clsFormat.Money(x.TotalAmount).Contains(text, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>قيمة الحالة المختارة — null = الكل.</summary>
    private byte? _GetStatusFilterValue()
    {
        if (cbStatus.SelectedIndex <= 0)
            return null;

        return (byte)cbStatus.SelectedIndex;   // 1 مسودة · 2 مرحّل · 3 ملغى
    }

    /// <summary>
    /// تعبئة شبكة العرض يدوياً (لا AutoGenerateColumns — الأعمدة معلنة في المصمّم).
    /// المعرّف يُمرَّر في عموده المخفي ويُقرأ لاحقاً عبر <see cref="GridRowHelper"/> فقط (RULE-N179).
    /// </summary>
    private void _PopulateGrid()
    {
        dgvMain.Rows.Clear();

        foreach (var document in _filteredReturns)
        {
            dgvMain.Rows.Add(
                document.Id,
                document.ReturnNo,
                clsFormat.DateToShort(document.ReturnDate),
                DocumentTextHelper.CashCustomerNameOr(document.CustomerName),
                clsFormat.Money(document.TotalAmount),
                DocumentTextHelper.StatusText(document.Status));   // البيت الواحد لنص الحالة (RULE-N169)
        }

        // صف «لا توجد بيانات» — طبقة نصية فوق الشبكة عند فراغ النتيجة.
        lblEmpty.Visible = _filteredReturns.Count == 0;

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

        foreach (var document in _filteredReturns)
        {
            total += document.TotalAmount;

            if (document.Status == StatusPosted) posted++;
            else if (document.Status == StatusCancelled) cancelled++;
            else if (document.Status == StatusDraft) draft++;
        }

        lblSummary.Text =
            $"المعروض: {clsFormat.Number(_filteredReturns.Count)} من {clsFormat.Number(_allReturns.Count)} — " +
            $"الإجمالي: {clsFormat.Money(total)} · مسودة: {clsFormat.Number(draft)} · " +
            $"مرحّل: {clsFormat.Number(posted)} · ملغي: {clsFormat.Number(cancelled)}";
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
            Log.Error(ex, "خطأ أثناء اختيار مرتجع من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات المرتجع المحدد. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>النقر المزدوج: عرض المستند في المحرر ثم نقل التركيز إلى شبكة سطوره للاطّلاع.</summary>
    private async void dgvMain_DoubleClick(object? sender, EventArgs e)
    {
        if (dgvMain.CurrentRow is null) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadSelectedRowAsync();

            if (_mode == EntryMode.Editing && dgvItems.Rows.Count > 0)
                dgvItems.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح المرتجع المحدد");
            clsDialogs.Error("تعذر عرض بيانات المرتجع المحدد. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>يحمّل السطر المحدد في المحرر (عرض للقراءة — D-05) بالقراءة عبر <see cref="GridRowHelper"/>.</summary>
    private async Task _LoadSelectedRowAsync()
    {
        if (dgvMain.CurrentRow is not { } row) return;
        if (!GridRowHelper.TryGetInt(row, "Id", out int id) || id <= 0) return;

        await _FillFormAsync(id);
    }

    /// <summary>تعبئة المحرر من الخدمة ثم ضبط الوضع والأزرار (اختيار مستند = عرض للقراءة — D-05).</summary>
    private async Task _FillFormAsync(int returnId)
    {
        var result = await ReturnService.GetByIdAsync(returnId);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات المرتجع.");
            return;
        }

        var document = result.Value;

        _invoice = document;
        _createdId = document.Id;               // لا إنشاء ثانٍ من مستند محمَّل
        _loadedStatus = document.Status;
        _mode = EntryMode.Editing;

        txtReturnNo.Text = document.ReturnNo.ToString();
        dtpDate.Value = document.ReturnDate == default ? DateTime.Today : document.ReturnDate;
        _SelectCustomer(document.CustomerId);
        _SelectWarehouse(document.WarehouseId);
        txtNotes.Text = document.Notes ?? string.Empty;
        lblStatusValue.Text = DocumentTextHelper.StatusText(document.Status);

        _isBinding = true;
        try
        {
            if (dgvItems.IsCurrentCellInEditMode) dgvItems.CancelEdit();   // لا تفريغ لقائمة أثناء تحرير خلية

            _lines.Clear();

            foreach (var item in document.Items)
            {
                var product = _allProducts.FirstOrDefault(p => p.Id == item.ProductId);

                _lines.Add(new InvoiceLineItem
                {
                    ProductId = item.ProductId,
                    ProductCode = product?.Barcode ?? string.Empty,
                    Barcode = product?.Barcode ?? string.Empty,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    DiscountAmount = item.DiscountAmount
                });
            }
        }
        finally
        {
            _isBinding = false;
        }

        _RecalculateTotals();
        _ApplyModeState();
    }

    /// <summary>اختيار العميل المحفوظ في القائمة المنسدلة (صفر أو أقل = «عميل نقدي»).</summary>
    private void _SelectCustomer(int customerId)
    {
        cmbCustomer.SelectedValue = customerId > 0 ? customerId : CashCustomerId;

        // عميل غير موجود في القائمة (بيانات قديمة) ⇒ لا اختيار، بدل إسناد «عميل نقدي» خطأً.
        if (cmbCustomer.SelectedValue is null)
            cmbCustomer.SelectedIndex = -1;
    }

    /// <summary>اختيار المستودع المحفوظ — <c>WarehouseDto.Id</c> من نوع <c>short</c> فلا تحويل صريح قد يرمي.</summary>
    private void _SelectWarehouse(int warehouseId)
    {
        if (warehouseId is > 0 and <= short.MaxValue)
            cmbWarehouse.SelectedValue = (short)warehouseId;

        // مستودع غير موجود في القائمة ⇒ لا اختيار، بدل الإسناد إلى أول مستودع خطأً.
        if (cmbWarehouse.SelectedValue is null)
            cmbWarehouse.SelectedIndex = -1;
    }

    private void _SelectGridRow(int returnId)
    {
        foreach (DataGridViewRow row in dgvMain.Rows)
        {
            if (GridRowHelper.TryGetInt(row, "Id", out int id) && id == returnId)
            {
                row.Selected = true;

                if (GridRowHelper.Cell(row, "ReturnNo") is { } cell)
                    dgvMain.CurrentCell = cell;

                break;
            }
        }
    }

    // ── آلة الحالة (RULE-N43 / RULE-N44 / RULE-N149…N152 · D-05) ───────────────

    /// <summary>
    /// آلة الحالة المركزية: تفعيل/تعطيل حقول المحرر وسطوره، وحالة كل زر، ونص شريط الوضع.
    /// تُستدعى بعد **كل** انتقال: تحميل · «جديد» · اختيار مستند · حفظ · ترحيل · إلغاء · تحديث · Esc.
    /// </summary>
    private void _ApplyModeState()
    {
        bool adding = _mode == EntryMode.Adding && _IsEditable();
        bool hasDocument = _mode == EntryMode.Editing && _invoice is not null;

        if (_mode == EntryMode.Browsing) RequiredFieldHelper.Clear(this);   // لا علامات مطلوبة على حقول معطّلة

        _SetEditorEnabled(adding);

        // RULE-N22a / RULE-N52 — سلوك شبكة السطور تعلنه الشاشة: التحرير والسطر الجديد للإضافة فقط.
        dgvItems.ReadOnly = !adding;
        dgvItems.AllowUserToAddRows = adding;

        // آلة حالة الأزرار (RULE-N43): «جديد/حفظ مسودة/ترحيل» في الإضافة، و«ترحيل/إلغاء» على المستند المحدد.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;

        btnSaveDraft.Enabled = adding;
        btnPost.Enabled = adding || (hasDocument && _IsPostable());
        btnCancelDocument.Enabled = hasDocument && _loadedStatus != StatusCancelled;

        btnAddItem.Enabled = adding;
        btnRemoveItem.Enabled = adding;

        _UpdateModeHeader();

        // RULE-N147 — الزر الافتراضي للشاشة يتبع الوضع الحالي (يُحدَّث بعد كل انتقال حالة).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147):
    /// عرض ⇒ «جديد» · إضافة ⇒ «حفظ مسودة» · مستند قائم ⇒ <c>null</c> (D-05: لا حفظ على مستند للقراءة فقط).
    /// ويُرجع <c>null</c> دائماً ما دام التركيز داخل شبكة السطور أو محرّر خليتها (RULE-N153).
    /// يُقرأ عند كل ضغطة وعند كل انتقال حالة (لا يُخزَّن).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        // RULE-N153 — الشبكة تملك Enter: إدخال سطر لا حفظ/ترحيل، ولا نقر لزر الوضع من داخلها.
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return null;

        return _mode switch
        {
            EntryMode.Adding => btnSaveDraft is { Visible: true, Enabled: true } ? btnSaveDraft : null,
            EntryMode.Editing => null,   // D-05 — لا إجراء حفظ على مستند قائم
            _ => btnNew
        };
    }

    /// <summary>
    /// يزامن <see cref="Form.AcceptButton"/> مع الزر الأساسي للوضع الحالي (RULE-N147)، ويُفرّغه ما دام
    /// التركيز داخل شبكة السطور حتى لا يخطف <c>Enter</c> من تحرير الخلية (RULE-N153).
    /// </summary>
    private void _SyncAcceptButton() => AcceptButton = _PrimaryActionButton();

    /// <summary>هل يمكن ترحيل المستند المحمَّل؟ المسودة تُرحَّل، والمرحّل/الملغى لا.</summary>
    private bool _IsPostable() => _loadedStatus == StatusDraft;

    /// <summary>
    /// هل بيانات المحرر قابلة للتحرير؟ **للجديد فقط**: العقد لا يحوي <c>UpdateAsync</c>،
    /// فتحرير مستند محفوظ لا يمكن إرساله ⇒ يُقفل بدل أن يُعرض ولا يُحفظ (<c>D-05</c>).
    /// </summary>
    private bool _IsEditable() => _invoice is null;

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض/العرض للقراءة وتُفعَّل في الإضافة فقط.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        dtpDate, cmbCustomer, cmbWarehouse, txtNotes
    };

    /// <summary>تفعيل أو تعطيل كل حقول الرأس دفعة واحدة (رقم المرتجع يبقى للقراءة دائماً).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» · «إضافة مرتجع جديد» ·
    /// «عرض مرتجع رقم: N — مسودة/مرحل/ملغي (للقراءة فقط)».
    /// </summary>
    private void _UpdateModeHeader()
    {
        string state = _mode switch
        {
            EntryMode.Adding => "إضافة مرتجع مبيعات جديد",
            EntryMode.Editing when _invoice is not null =>
                $"عرض مرتجع رقم: {_invoice?.ReturnNo} — {DocumentTextHelper.StatusText(_loadedStatus)} (للقراءة فقط)",
            EntryMode.Editing => "عرض مرتجع (للقراءة فقط)",
            _ => "وضع العرض"
        };

        lblMode.Text = $"{FormTitle} — {state}";
        Text = $"مرتجعات المبيعات — {state}";
    }

    // ── إعادة الضبط / إدخال جديد ────────────────────────────────────────────────

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض**: تفريغ المحرر والسطور، تعطيل كل حقول الإدخال،
    /// وإتاحة «جديد» فقط (RULE-N149).
    /// </summary>
    private void _ResetForm()
    {
        _invoice = null;
        _createdId = null;
        _loadedStatus = StatusDraft;
        _mode = EntryMode.Browsing;

        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);

        txtReturnNo.Text = "جديد";
        dtpDate.Value = DateTime.Today;
        txtNotes.Clear();
        lblStatusValue.Text = "—";

        _isBinding = true;
        try
        {
            if (dgvItems.IsCurrentCellInEditMode) dgvItems.CancelEdit();   // لا تفريغ لقائمة أثناء تحرير خلية

            _lines.Clear();
        }
        finally
        {
            _isBinding = false;
        }

        _RecalculateTotals();

        // العميل النقدي والمستودع الأول افتراضياً — كما في الشاشة المصدر.
        if (cmbCustomer.Items.Count > 0) cmbCustomer.SelectedIndex = 0;
        if (cmbWarehouse.Items.Count > 0) cmbWarehouse.SelectedIndex = 0;

        dgvMain.ClearSelection();

        _ApplyModeState();
        btnNew.Focus();
    }

    /// <summary>بدء إدخال مرتجع جديد (زر «جديد» / F2): تفريغ ثم **تفعيل** الرأس والسطور (RULE-N151).</summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        cmbCustomer.Focus();
    }

    // ── الأوامر ─────────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    private async void btnSaveDraft_Click(object? sender, EventArgs e) => await _SaveAsync(post: false);

    private async void btnPost_Click(object? sender, EventArgs e) => await _SaveAsync(post: true);

    /// <summary>
    /// حفظ المرتجع أو ترحيله:
    /// <paramref name="post"/> = false ⇒ «حفظ مسودة» (<c>CreateAsync</c>) ·
    /// true ⇒ إنشاء (إن لم يوجد) ثم <c>PostAsync</c> على نفس المعرّف.
    /// <para>
    /// <b>لا مستند مكرر:</b> بعد نجاح الإنشاء يُثبَّت المعرّف في <c>_createdId</c>، فأي إعادة ضغط
    /// تُرحّل المستند نفسه أو تُرفض عربياً — ولا يُنشأ مرتجع ثانٍ (العقد بلا <c>UpdateAsync</c>).
    /// </para>
    /// </summary>
    private async Task _SaveAsync(bool post)
    {
        // المستند مُنشأ سابقاً (أو مُحمَّل من القائمة): لا إنشاء ثانٍ — رفض عربي صريح بدل مستند مكرر.
        if (!post && _createdId is not null)
        {
            clsDialogs.Info("هذا المرتجع محفوظ مسبقاً — لا يُنشأ مستند ثانٍ. استخدم «ترحيل» لترحيل المسودة نفسها.",
                "المرتجع محفوظ");
            return;
        }

        bool retryPost = _mode == EntryMode.Editing && post && _createdId is not null && _IsPostable();

        // D-05 — لا حفظ/ترحيل إلا من وضع الإضافة، أو إعادة محاولة ترحيل مسودة أُنشئت فعلاً.
        if (_mode != EntryMode.Adding && !retryPost) return;

        // حاجز الأمان: لا يُبنى طلب من مستند مرحّل أو ملغى.
        if (_invoice is not null && _invoice.Status != StatusDraft)
        {
            clsDialogs.Warning("لا يمكن حفظ أو ترحيل مرتجع مرحّل أو ملغى.");
            return;
        }

        // الحقول المطلوبة: الحكم والعَلم في البيت الواحد RequiredFieldHelper (RULE-N172/RULE-N194).
        if (!RequiredFieldHelper.Validate(this))
        {
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
            return;
        }

        if (_createdId is null && !_ValidateLines()) return;

        if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم زيادة المخزون وتقليل رصيد العميل.")) return;

        Cursor = Cursors.WaitCursor;
        btnSaveDraft.Enabled = false;
        btnPost.Enabled = false;

        try
        {
            int documentId;

            if (_createdId is int existingId)
            {
                // إعادة محاولة الترحيل: نفس المستند لا نسخة جديدة.
                documentId = existingId;
            }
            else
            {
                var createResult = await ReturnService.CreateAsync(_BuildCreateRequest());

                if (!createResult.IsSuccess || createResult.Value is null)
                {
                    clsDialogs.Error(createResult.Error ?? "تعذر حفظ المرتجع.");
                    return;
                }

                // نُثبّت المستند المُنشأ: المعرّف يمنع الإنشاء المكرر، والحالة تصير «للقراءة فقط» (D-05).
                _createdId = createResult.Value.Id;
                _invoice = createResult.Value;
                _loadedStatus = createResult.Value.Status;
                _mode = EntryMode.Editing;

                txtReturnNo.Text = createResult.Value.ReturnNo.ToString();
                lblStatusValue.Text = DocumentTextHelper.StatusText(createResult.Value.Status);

                documentId = createResult.Value.Id;
                _ApplyModeState();
            }

            if (post)
            {
                var postResult = await ReturnService.PostAsync(documentId);

                if (!postResult.IsSuccess)
                {
                    // لا إغلاق عند فشل الترحيل: المرتجع محفوظ كمسودة وإعادة المحاولة تُرحّله نفسه.
                    await _ReloadDocumentAsync(documentId);

                    clsDialogs.Warning(
                        $"أُنشئ المرتجع كمسودة برقم {documentId} وتعذر ترحيله.\n" +
                        "لم يُنشأ مستند جديد: أعد الضغط على «ترحيل» (F4) لترحيل نفس المسودة.\n" +
                        "سطور المرتجع للقراءة الآن لأنها محفوظة كمسودة.",
                        "لم يُرحَّل المرتجع");
                    return;
                }

                clsDialogs.Success("تم ترحيل المرتجع.");

                // أثر مخزني فعلي من الخدمة — إشعار الشاشات المعتمدة على المخزون.
                foreach (var line in _lines.Where(l => l.ProductId > 0))
                    EventBus.Publish(new StockChangedMessage(line.ProductId));
            }
            else
            {
                clsDialogs.Success("تم حفظ المرتجع كمسودة.");
            }

            EventBus.Publish(new SalesReturnChangedMessage(documentId));   // البيت الواحد لرسالة التغيير

            await _ReloadDocumentAsync(documentId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ مرتجع المبيعات (ترحيل: {Post})", post);
            clsDialogs.Error("تعذر حفظ المرتجع. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>إعادة تحميل القائمة والمستند معاً وإبقاء سطره محدداً (تحديث تلقائي — RULE-N151).</summary>
    private async Task _ReloadDocumentAsync(int documentId)
    {
        await _LoadReturnsAsync();
        await _FillFormAsync(documentId);
        _SelectGridRow(documentId);
    }

    /// <summary>بناء طلب الإنشاء من المحرر (المال <c>decimal</c> — RULE-001).</summary>
    private CreateSalesReturnRequest _BuildCreateRequest()
    {
        var items = _lines
            .Where(line => line.ProductId > 0)
            .Select(line => new ReturnItemRequest(
                SalesInvoiceLineId: 0,
                ProductId: line.ProductId,
                ProductUnitId: line.ProductUnitId > 0 ? line.ProductUnitId : 1,
                Quantity: line.Quantity,
                UnitPrice: line.UnitPrice,
                Amount: line.LineTotal,
                DiscountAmount: line.DiscountAmount,
                Mode: 1,
                Notes: null))
            .ToList();

        int customerId = cmbCustomer.SelectedValue is int selectedCustomer && selectedCustomer > 0 ? selectedCustomer : 0;

        // WarehouseDto.Id من نوع short ⇒ القراءة بـ is short لا بتحويل صريح قد يرمي.
        short warehouseId = cmbWarehouse.SelectedValue is short selectedWarehouse ? selectedWarehouse : (short)0;

        return new CreateSalesReturnRequest(
            SalesInvoiceId: null,
            CustomerId: customerId > 0 ? customerId : (int?)null,   // «عميل نقدي» يُرسَل null
            WarehouseId: warehouseId,
            ReturnDate: dtpDate.Value,
            ReturnReason: null,
            Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
            Items: items);
    }

    /// <summary>
    /// إلغاء المستند المحدد (مسودة أو مرحّل) بتأكيد صريح. المرتجع الملغى يُرفض عربياً قبل نداء الخدمة
    /// (تكافؤ رفض الخدمة — RULE-N167).
    /// </summary>
    private async void btnCancelDocument_Click(object? sender, EventArgs e)
    {
        if (_invoice is not { } document)
        {
            clsDialogs.Warning("يرجى اختيار مرتجع من القائمة أولاً.");
            return;
        }

        if (_loadedStatus == StatusCancelled)
        {
            clsDialogs.Warning("المرتجع ملغي بالفعل.");
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من إلغاء المرتجع رقم \"{document.ReturnNo}\"؟"))
            return;

        bool reversesStock = _loadedStatus == StatusPosted;

        Cursor = Cursors.WaitCursor;
        btnCancelDocument.Enabled = false;

        try
        {
            var result = await ReturnService.CancelAsync(document.Id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم إلغاء المرتجع.");

                // إلغاء مرحّل يردّ أثر المخزون (تقييد الخدمة) — فأبلغ الشاشات المعتمدة على المخزون بنفس رسالتها.
                if (reversesStock)
                {
                    foreach (var line in document.Items)
                        EventBus.Publish(new StockChangedMessage(line.ProductId));
                }

                EventBus.Publish(new SalesReturnChangedMessage(document.Id));

                await _ReloadDocumentAsync(document.Id);
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر إلغاء المرتجع.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إلغاء المرتجع {ReturnId}", document.Id);
            clsDialogs.Error("تعذر إلغاء المرتجع. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← العملاء/المستودعات/الأصناف ← المرتجعات ← المحرر ووضع العرض.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            _ClearFilterInputs();
            await _LoadLookupsAsync();
            await _LoadReturnsAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث بيانات مرتجعات المبيعات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // ── السطور: تحقق · إضافة · إزالة · إعادة حساب ──────────────────────────────

    /// <summary>
    /// تحقق السطور: صنف واحد على الأقل، وكل سطر مرتبط بصنف، والكمية أكبر من صفر، والسعر غير سالب.
    /// <para>
    /// هذه **قاعدة عمل مالية** لا قاعدة «حقل مطلوب فارغ» (تلك ملك <see cref="RequiredFieldHelper"/> —
    /// RULE-N172). والرفض تجميعة عربية واحدة من بيوتها: <c>errorProvider</c> + <c>clsInputFeedback</c> +
    /// <c>clsDialogs</c>.
    /// </para>
    /// </summary>
    private bool _ValidateLines()
    {
        if (_lines.Count == 0 || _lines.All(line => line.ProductId <= 0))
            return _RejectLines("يجب إضافة صنف واحد على الأقل للإرجاع");

        // سطر فيه كود/كمية بلا صنف محلول: يُسقط من الطلب بصمت فيفسد أن «الإجمالي على الشاشة ≠ المحفوظ».
        var unresolved = _lines.FirstOrDefault(line => line.ProductId <= 0 &&
            (!string.IsNullOrWhiteSpace(line.ProductCode) || line.Quantity != 0m || line.UnitPrice != 0m));

        if (unresolved is not null)
            return _RejectLines($"السطر «{unresolved.ProductCode}» غير مرتبط بصنف مسجَّل — صحّح الكود/الباركود أو احذف السطر");

        // كمية صفر/سالب تُفسد المخزون ورصيد العميل — ترفض عربياً قبل نداء الخدمة (تكافؤ رفض الخدمة).
        var zeroQuantity = _lines.FirstOrDefault(line => line.ProductId > 0 && line.Quantity <= 0m);
        if (zeroQuantity is not null)
            return _RejectLines($"الكمية يجب أن تكون أكبر من صفر ({zeroQuantity.ProductName})");

        var negativePrice = _lines.FirstOrDefault(line => line.ProductId > 0 && line.UnitPrice < 0m);
        if (negativePrice is not null)
            return _RejectLines($"سعر الوحدة لا يمكن أن يكون سالباً ({negativePrice.ProductName})");

        errorProvider.SetError(dgvItems, string.Empty);   // لا يبقى تلميح رفض قديم بعد تصحيح السطور
        return true;
    }

    /// <summary>تجميعة رفض السطور (أيقونة ✗ + رسالة + ومضة) — مكان واحد داخل الشاشة.</summary>
    private bool _RejectLines(string message)
    {
        errorProvider.SetError(dgvItems, message);
        clsInputFeedback.ShowRejectTooltip(dgvItems, message, toolTip);
        clsDialogs.Warning(message);
        dgvItems.Focus();
        return false;
    }

    private void btnAddItem_Click(object? sender, EventArgs e)
    {
        if (_mode != EntryMode.Adding) return;

        _lines.Add(new InvoiceLineItem());
        dgvItems.Focus();
    }

    private void btnRemoveItem_Click(object? sender, EventArgs e)
    {
        if (_mode != EntryMode.Adding) return;
        if (dgvItems.CurrentRow?.DataBoundItem is not InvoiceLineItem line) return;

        _lines.Remove(line);
        _RecalculateTotals();
    }

    /// <summary>
    /// سلوك شبكة السطور (RULE-N22a): تحرير داخل الخلية وسطر جديد مملوكان للشاشة،
    /// و<see cref="Form.AcceptButton"/> يُفرَّغ ما دام التركيز داخلها (RULE-N153).
    /// </summary>
    private void _SetupLineGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.DataSource = _lines;

        // الحالة الابتدائية = وضع العرض: سطور للقراءة بلا سطر إدخال (RULE-N149/N52).
        dgvItems.ReadOnly = true;
        dgvItems.AllowUserToAddRows = false;

        // الأعمدة الرقمية بتنسيق بيت التنسيق الواحد (RULE-N169) — كميات N3 ومبالغ N2.
        colQuantity.DefaultCellStyle.Format = clsFormat.QuantityPattern;
        colUnitPrice.DefaultCellStyle.Format = clsFormat.MoneyPattern;
        colLineTotal.DefaultCellStyle.Format = clsFormat.MoneyPattern;

        dgvItems.CellEndEdit += dgvItems_CellEndEdit;

        // قيمة خلية غير قابلة للتحويل لا تُسقط الشاشة (تُترك للتحقق العربي عند الحفظ).
        dgvItems.DataError += (_, e) => e.ThrowException = false;

        // RULE-N153 — لا يبقى Enter مرتبطاً بزر الوضع ما دام التركيز داخل شبكة السطور.
        dgvItems.Enter += (_, _) => _SyncAcceptButton();
        dgvItems.Leave += (_, _) => _SyncAcceptButton();
    }

    /// <summary>
    /// إعادة حساب السطر ومجاميعه عند انتهاء تحرير خلية: حلّ الباركود إلى صنف، ثم
    /// <see cref="InvoiceLineItem.LineTotal"/> وإجمالي المستند.
    /// </summary>
    private void dgvItems_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isBinding || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (_mode != EntryMode.Adding) return;

        var row = dgvItems.Rows[e.RowIndex];

        // سطر الإدخال الجديد: قد يكون محرّك الربط أضافه للقائمة أو لم يُضفه بعد — الحالتان مغطّاتان.
        if (row.DataBoundItem is not InvoiceLineItem line)
        {
            if (e.RowIndex < _lines.Count) return;

            _CreateLineFromRow(row);
            _RecalculateTotals();
            return;
        }

        if (dgvItems.Columns[e.ColumnIndex].Name == "colBarcode")
            _ApplyBarcodeLookup(line);

        _RecalculateTotals();
        dgvItems.InvalidateRow(e.RowIndex);
    }

    /// <summary>إنشاء سطر من صف الإدخال الجديد مباشرةً (عندما لا يكون محرّك الربط قد أضافه بعد).</summary>
    private void _CreateLineFromRow(DataGridViewRow row)
    {
        string barcode = (row.Cells[colBarcode.Index].Value?.ToString() ?? string.Empty).Trim();

        if (barcode.Length == 0) return;   // سطر جديد فارغ — لا شيء يُضاف

        var product = _FindProductByBarcode(barcode);

        if (product is null)
        {
            // لا يُضاف سطر بلا صنف: إبلاغ المستخدم بدل الإسقاط الصامت.
            clsDialogs.Warning("لم يتم العثور على صنف بهذا الباركود — لم تُضَف السطر.");
            return;
        }

        object? quantityCell = row.Cells[colQuantity.Index].Value;

        // الخلية غير المُدخَلة (null · DBNull · فراغ) ⇒ الافتراضي 1؛ أما الصفر المكتوب فيبقى صفراً ويُرفض عند الحفظ.
        decimal quantity = quantityCell is null or DBNull || string.IsNullOrWhiteSpace(quantityCell.ToString())
            ? 1m
            : _CellQuantity(row);

        _lines.Add(new InvoiceLineItem
        {
            ProductId = product.Id,
            ProductCode = product.Barcode ?? barcode,
            Barcode = product.Barcode ?? barcode,
            ProductName = product.Name,
            Quantity = quantity,
            UnitPrice = _CellMoney(row)
        });
    }

    /// <summary>حلّ الباركود المكتوب في سطر قائم إلى صنف مسجَّل (الكمية الافتراضية 1 عند تركها صفراً).</summary>
    private void _ApplyBarcodeLookup(InvoiceLineItem line)
    {
        string code = (line.ProductCode ?? string.Empty).Trim();

        if (code.Length == 0) return;

        var product = _FindProductByBarcode(code);

        if (product is null) return;   // يُرفض عند الحفظ برسالة عربية: «السطر غير مرتبط بصنف مسجَّل»

        line.ProductId = product.Id;
        line.Barcode = product.Barcode ?? code;
        line.ProductName = product.Name;

        if (line.Quantity <= 0m) line.Quantity = 1m;
    }

    /// <summary>البحث عن صنف بالباركود — مطابقة تامة غير حسّاسة لحالة الأحرف.</summary>
    private ProductDto? _FindProductByBarcode(string barcode)
        => _allProducts.FirstOrDefault(product =>
            !string.IsNullOrWhiteSpace(product.Barcode)
            && string.Equals(product.Barcode, barcode, StringComparison.OrdinalIgnoreCase));

    /// <summary>الكمية المكتوبة في الخلية — قراءة محروسة عبر بيت التنسيق (RULE-N169).</summary>
    private decimal _CellQuantity(DataGridViewRow row)
        => clsFormat.TryParseQuantity(row.Cells[colQuantity.Index].Value?.ToString(), out decimal value) ? value : 0m;

    /// <summary>السعر المكتوب في الخلية — قراءة محروسة عبر بيت التنسيق (RULE-N169).</summary>
    private decimal _CellMoney(DataGridViewRow row)
        => clsFormat.TryParseMoney(row.Cells[colUnitPrice.Index].Value?.ToString(), out decimal value) ? value : 0m;

    /// <summary>
    /// إجمالي المرتجع المعروض = مجموع السطور التي ستُرسَل فعلاً (ProductId &gt; 0)،
    /// فلا يخالف المعروضُ ما يُحفظ (RULE-N167).
    /// </summary>
    private void _RecalculateTotals()
    {
        decimal total = _lines.Where(line => line.ProductId > 0).Sum(line => line.LineTotal);

        lblTotals.Text = $"الإجمالي: {clsFormat.Money(total)}";
    }

    // ── قائمة السياق (RULE-N23) ─────────────────────────────────────────────────

    /// <summary>RULE-N23: العناصر تستدعي نفس أزرار الشاشة — لا منطق مكرر.</summary>
    private async void cmsView_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadSelectedRowAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء عرض المرتجع من قائمة السياق");
            clsDialogs.Error("تعذر عرض بيانات المرتجع المحدد. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
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
        cmsRefresh.Enabled = btnRefresh.Enabled;
    }
}
