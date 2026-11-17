// -----------------------------------------------------------------------------
// frmAccounts.cs — دليل الحسابات (حقول الحساب أعلى · شجرة يسار · شبكة يمين · ملخص أسفل)
//
// • الشكل (RULE-N135…N138 + الشاشة المرجعية frmProductMaster — TASK-011):
//     «شريط العنوان» ← «حقول الحساب» (pnlFields · ثلاثة أعمدة) ← «أوامر الحسابات» ←
//     «بحث وتصفية» ← [شجرة | شبكة] ← «الإجماليات» — ولا صندوق داخل صندوق.
//
// • العناصر القابلة للتضمين (RULE-N169 — لا نسخة ثانية من منطقها هنا):
//     ctrlTreeOfAccounts   عرض شجري للقراءة فقط + اختيار سريع (بلا أزرار ولا تصفية:
//                          العمليات كلها من شريط الشاشة، RULE-N100).
//     ctrlCrudToolbar      الأزرار الملوّنة وآلة حالتها.
//   وبيانات الحساب (حقول الحساب · خيارات الأنواع · معاينة الكود · حكم «مطلوب» · التحقق)
//   صارت **داخل الشاشة** في pnlFields (TASK-010) — بلا مكوّن وسيط — بثلاثة أعمدة
//   وسطر صحّة بيانات سفلي، مطابقةً لتخطيط pnlFields في frmProductMaster (TASK-011).
//
// • **الشجرة تقود الشبكة** (حسب مواصفة المالك docs/acounts.md): النقر على عقدة يملأ المحرّر
//   **وفي الوقت نفسه** يصفّي الشبكة على الحساب وفروعه (أو على الطبيعة كاملة عند عقدة طبيعة).
//   التصفية تُطبَّق **قبل** فتح السجل، لأن السطر يجب أن يكون موجوداً في الشبكة كي يُحدَّد فيها.
//   معرفات الفروع تأتي من الشجرة نفسها (GetSubtreeIds/GetNatureIds) — لا حساب هرمي هنا.
//
// • آلة الحالة واحدة `_ApplyModeState()` (النظير RULE-N150): هي **المكان الوحيد** الذي
//   يُغذّي الشريط المشترك (SetMode/HasSelection/AllowDelete) ويضبط Enabled لأزرار الشاشة
//   **وحقول الحساب** — ولا زر حفظ محلي بعد TASK-011.
//
// • الوضع: Browsing (فتح/بعد الحفظ/إلغاء الإضافة) · Adding («+ إضافة») · Editing (اختيار
//   حساب من الشجرة أو الشبكة). حقول الحساب تتبع نفس الآلة (§2.36).
//
// • الاختصارات: F2 = إضافة · F3 = تعديل المحدد · F5 = تحديث كامل · Delete = تعطيل ·
//   Esc = مسح التصفية (داخل صندوقها) / إلغاء الإضافة-التعديل / إغلاق (RULE-N47).
//   ولا زر أساسي للنموذج (AcceptButton = null): أزرار الشريط المشترك لا تُنقَر بـEnter
//   أبداً — فلا حفظ صامت من داخل حقول المحرّر أو شبكة السطور (RULE-N153/RULE-N193).
//
// • RULE-N205: `catch` واحد لكل حدّ فعلي — معالجات `async void` عبر `_RunSafeAsync`،
//   ومعالج الحذف النهائي بنفسه، و`finally` تُعيد المؤشر والعلم دائماً.
// -----------------------------------------------------------------------------

using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Helpers;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Accounting.Controls;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// شاشة دليل الحسابات: شجرة هرمية + محرّر حساب + شبكة عرض/تصفية، وشريط أوامر موحّد.
/// <para>
/// كل القراءات والكتابات تمرّ عبر <see cref="IAccountDesktopService"/> من حزمة الخدمات
/// المحقونة (RULE-N05/N06) — لا DbContext ولا HTTP ولا أي طبقة أدنى.
/// </para>
/// </summary>
public partial class frmAccounts : Form
{
    // ── رسائل عربية (RULE-N28) ────────────────────────────────────────────────
    private const string LoadFailedMessage = "تعذر تحميل دليل الحسابات. يرجى إعادة المحاولة.";
    private const string LoadUnexpectedMessage = "حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.";
    private const string RefreshFailedMessage = "تعذر تحديث البيانات. يرجى إعادة المحاولة.";
    private const string SaveFailedMessage = "حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.";
    private const string DeleteFailedMessage = "حدث خطأ أثناء التعطيل. يرجى إعادة المحاولة.";
    private const string PermanentDeleteFailedMessage = "حدث خطأ أثناء الحذف النهائي. يرجى إعادة المحاولة.";
    private const string SelectFirstMessage = "يرجى اختيار حساب من الشجرة أو الشبكة أولاً.";
    private const string SystemAccountMessage =
        "هذا حساب نظامي — لا يمكن تعطيله أو تعديله أو حذفه من هذه الشاشة.";
    private const string NameRequiredMessage = "اسم الحساب مطلوب";
    private const string NatureRequiredMessage = "يجب اختيار نوع الحساب";
    private const string NormalBalanceRequiredMessage = "يجب اختيار الحساب الختامي (طبيعة الرصيد)";
    private const string ParentCycleMessage = "لا يمكن اختيار الحساب نفسه أو أحد فروعه كحساب أب";
    private const string GroupToPostingWarningMessage =
        "تحويل حساب تجميعي إلى حساب حركة يُرفض إذا كان له حسابات فرعية";

    /// <summary>نص الخيار الجذري: لا أب ⇒ حساب رئيسي (كوده يولّده النظام عند الحفظ).</summary>
    private const string RootParentText = "(بلا أب — حساب رئيسي)";

    private readonly IServiceProvider _serviceProvider = null!;

    private IAccountDesktopService AccountService => _serviceProvider.GetRequiredService<IAccountDesktopService>();

    // ── العناصر القابلة للتضمين (معلنة في المصمّم — RULE-N145) ─────────────────
    private ctrlCrudToolbar Toolbar => ctrlCrudToolbar1;

    private ctrlTreeOfAccounts Tree => ctrlTreeOfAccounts1;

    // ── State ────────────────────────────────────────────────────────────────
    private readonly List<AccountDto> _accounts = new();
    private readonly Dictionary<int, AccountDto> _accountsById = new();

    private DataView _accountsView = new();
    private int _totalCount;

    /// <summary>الحساب المعروض/المعدَّل (<c>null</c> = لا سجل محدد).</summary>
    private int? _editId;

    /// <summary>وضع الشاشة الوحيد: عرض · إضافة · تعديل (§2.36).</summary>
    private CrudMode _mode = CrudMode.Browsing;

    /// <summary>عملية جارية (حفظ/تعطيل/تحديث) — يُعطّل كل ما قد يُطلق عملية أخرى.</summary>
    private bool _actionButtonsBusy;

    /// <summary>مزامنة تحديد (شبكة ↔ شجرة ↔ محرّر): تُكتم أحداث التحديد العابرة.</summary>
    private bool _syncingSelection;

    /// <summary>
    /// معرّفات تصفية الشجرة المختارة — <c>null</c> = لا تصفية عقدة. تُملأ من الشجرة
    /// (GetSubtreeIds للحساب، GetNatureIds للطبيعة) وتُركَّب مع بقية أدوات التصفية.
    /// </summary>
    private HashSet<int>? _treeFilterIds;

    /// <summary>وصف تصفية العقدة لشريط الملخص («الكود — الاسم» أو «طبيعة: …») — <c>null</c> = بلا تصفية عقدة.</summary>
    private string? _treeFilterLabel;

    /// <summary>
    /// تعبئة/إعادة ضبط جارية: تُكتم معاينة كود الحساب وأحداث القوائم حتى لا تُطلق طلبات عابرة.
    /// </summary>
    private bool _binding;

    /// <summary>حارس الردود المتأخرة لمعاينة الكود: تُقبل نتيجة **آخر** أب مطلوب فقط.</summary>
    private int? _pendingChildCodeParentId;

    /// <summary>
    /// أب طلبته الشجرة («إضافة فرعي») قبل بناء القائمة — يُحفظ فلا يضيع إن كانت القائمة
    /// فارغة وقت الطلب، ويُقرأ عند كل بناء للقائمة. يُصفَّر عند اختيار المستخدم أباً بنفسه.
    /// </summary>
    private int? _requestedParentId;

    /// <summary>الحساب المعروض في المحرّر (من <see cref="_editId"/> وفهرس واحد — لا حالة ثانية).</summary>
    private AccountDto? _CurrentAccount
        => _editId is int id && _accountsById.TryGetValue(id, out var account) ? account : null;

    /// <summary>خيار نوع الحساب في التصفية **وفي المحرّر** — الربط بالقيمة لا بالنص العربي (RULE-N60).</summary>
    private sealed record NatureOption(byte Value, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>طبيعة الرصيد: <c>null</c> = «تلقائي» (تُشتق من النوع) · 1 مدين · 2 دائن.</summary>
    private sealed record NormalBalanceOption(byte? Value, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>الحساب الأب: <c>null</c> = حساب رئيسي بلا أب.</summary>
    private sealed record ParentOption(int? Id, string Text)
    {
        public override string ToString() => Text;
    }

    /// <summary>
    /// نوع الحساب كما يراه المستخدم — يُترجَم إلى <c>IsLeaf</c> في الخلفية:
    /// «رئيسي (تجميعي)» ⇒ <c>false</c> · «فرعي (حساب حركة نهائي)» ⇒ <c>true</c>.
    /// </summary>
    private sealed record AccountTypeOption(string Display, bool IsLeaf)
    {
        public override string ToString() => Display;
    }

    /// <summary>منشئ المصمم — لمصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmAccounts()
    {
        InitializeComponent();
    }

    public frmAccounts(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        // RULE-N40…RULE-N47 — لوحة المفاتيح: تنقل `Enter` واحد لكل الشاشة (ترتيب `TabIndex`
        // = ترتيب العرض: العمود الأيمن ثم الأوسط ثم الأيسر). لا زر وضع محلي بعد TASK-011
        // (زر «حفظ» المحرّر حُذف؛ الحفظ من الشريط المشترك).
        KeyboardHelper.WireEnterAsTab(this);

        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45
        KeyboardHelper.WireGlobalShortcuts(this,                                                        // RULE-N47
            (Keys.F2, () => _StartNewEntry(Tree.SelectedAccountId)),
            (Keys.F3, _StartEditFromSelection),
            (Keys.F5, () => _ = _RunSafeAsync(_RefreshAsync, RefreshFailedMessage)),
            (Keys.Delete, _HandleDeleteShortcut),
            (Keys.Escape, _HandleEscape));

        // RULE-N147: لا زر أساسي للنموذج — أزرار الوضع تعيش في الشريط المشترك الذي لا
        // يُعرِّض زرّه (خارج نطاق هذه الموجة)، و`_PrimaryActionButton` تُرجع null فيبقى
        // AcceptButton فارغاً: Enter لا يحفظ ولا يُرحّل من أي مكان (RULE-N153/RULE-N193).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N183: قاعدة «اسم الحساب مطلوب» تُوصَّل بعد WireFocusHighlight وقبل الثيم —
        // مرة واحدة، على النموذج نفسه (المحرّر صار جزءاً منه).
        _AttachRequiredFields();

        _LoadNatureOptions();
        _LoadNormalBalanceOptions();
        _LoadAccountTypeOptions();
        _LoadFilterOptions();
        _SetupToolTips();

        ApplyUiTheme();

        // RULE-N43: يُوصَّل بعد الثيم حتى يلتقط ألوان الأزرار النهائية.
        KeyboardHelper.WireButtonDisabledVisual(btnPermanentDelete, btnClearFilters);

        // ── المكوّنات القابلة للتضمين ─────────────────────────────────────────
        Tree.Attach(_serviceProvider);    // شجرة عرض للقراءة فقط

        Tree.AccountSelected += Tree_AccountSelected;

        Toolbar.AddRequested += Toolbar_AddRequested;
        Toolbar.EditRequested += Toolbar_EditRequested;
        Toolbar.SaveRequested += _OnSaveRequested;
        Toolbar.CancelEditRequested += _OnCancelRequested;
        Toolbar.DeleteRequested += Toolbar_DeleteRequested;
        Toolbar.RefreshRequested += Toolbar_RefreshRequested;
        Toolbar.CloseRequested += Toolbar_CloseRequested;

        FilterPanelHelper.Attach(this, _serviceProvider);

        _ApplyModeState();   // فتح الشاشة = وضع العرض (§2.36)
    }

    // ── Theme (RULE-N50…RULE-N54) ────────────────────────────────────────────

    /// <summary>الثيم الأخضر — ينتهي بـ <c>GreenThemeHelper.Apply(this)</c> كآخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this); // يفرض أبيض/Segoe UI ⇒ يُمحى لاحقاً

        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvAccounts);

        // RULE-N22/RULE-N52: سلوك الشبكة تعلنه الشاشة (ApplyGrid لا يلمسه).
        dgvAccounts.ReadOnly = true;
        dgvAccounts.AllowUserToAddRows = false;
        dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAccounts.MultiSelect = false;

        // طبقة «لا توجد بيانات» وسطر الملخص: النص المميّز من البيت الواحد.
        lblEmptyState.BackColor = Color.White;
        lblEmptyState.ForeColor = GreenThemeHelper.TextStrong;
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;

        // سطر صحّة البيانات أسفل حقول الحساب: لون الخطر من البيت الواحد (RULE-N169).
        lblValidationHint.ForeColor = GreenThemeHelper.StatusDanger;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Options / ToolTips ───────────────────────────────────────────────────

    /// <summary>خيارات التصفية (خيارات المحرّر في <c>_Load*Options</c> أدناه — نفس الشاشة).</summary>
    private void _LoadFilterOptions()
    {
        // أسماء الطبائع من البيت الواحد في الشجرة — لا قائمة ثانية للأسماء العربية (RULE-N172).
        cbNatureFilter.Items.Clear();
        cbNatureFilter.Items.Add(new NatureOption(0, "كل الأنواع"));

        for (byte nature = 1; nature <= 5; nature++)
            cbNatureFilter.Items.Add(new NatureOption(nature, ctrlTreeOfAccounts.NatureName(nature)));

        cbNatureFilter.SelectedIndex = 0;

        cbFilterBy.Items.Clear();
        cbFilterBy.Items.Add("الكود");
        cbFilterBy.Items.Add("الاسم العربي");
        cbFilterBy.Items.Add("الاسم الإنجليزي");
        cbFilterBy.SelectedIndex = 0;
    }

    /// <summary>RULE-N47: كل زر/أداة تحمل تلميحها — والتلميح المكرّر لا يُكتب مرتين.</summary>
    private void _SetupToolTips()
    {
        toolTip.SetToolTip(cbFilterBy, "عمود البحث: الكود أو الاسم العربي أو الاسم الإنجليزي");
        toolTip.SetToolTip(cbNatureFilter, "تصفية بطبيعة الحساب — بنفس أسماء عقد الشجرة");

        // تلميح واحد للزر الواحد: يبدأ بما في المصمّم ويُضيف أن تصفية عقدة الشجرة تُمسح معه
        // («مسح التصفية» و«تحديث» يرفعانها، فلا تبقى الشبكة محصورة في فرع).
        toolTip.SetToolTip(btnClearFilters,
            "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات — ويشمل تصفية عقدة الشجرة المختارة");

        toolTip.SetToolTip(dgvAccounts, "نقر مزدوج = فتح الحساب في المحرّر للتعديل");
    }

    // ═════════════════════════════════════════════════════════════════
    //  بيانات الحساب — المحرّر المدمج في الشاشة (TASK-010)
    //  كل ما كان في العنصر الوسيط: القوائم · التعبئة · الآباء · معاينة الكود · بناء الطلب
    //  · التحقق. لا عضو ثانٍ ولا حكم «مطلوب» ثانٍ (RULE-N172).
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يعبّئ «الحساب الأب» بالحسابات المؤهَّلة: حسابات المجموعة (غير الفرعية) والنشطة
    /// (ويبقى أب الحساب المعروض دائماً)، مع استثناء الحساب المطلوب تعديله وكل فروعه (منع الدوران).
    /// <para><b>لا يُعيد بناء <see cref="_accountsById"/></b> — مصدره الوحيد هو <c>_LoadAsync</c>.</para>
    /// </summary>
    /// <param name="accounts">قائمة الحسابات (من <c>GetAllAsync(includeInactive: true)</c>).</param>
    /// <param name="excludeId">الحساب المستثنى وفروعه — <c>null</c> في وضع الإضافة.</param>
    private void BindParents(IReadOnlyList<AccountDto> accounts, int? excludeId)
    {
        var previousParentId = _SelectedParentId();

        var excluded = _BuildExclusionSet(accounts, excludeId);

        // أب الحساب المعروض يبقى في القائمة ولو كان معطّلاً أو ورقياً: لو اختفى لظهر المحرّر
        // على «بلا أب» وحُفظ التعديل بالحساب في الجذر — فقدان صلة صامت.
        int? keepParentId = _CurrentAccount?.ParentId;

        var options = new List<ParentOption> { new(null, RootParentText) };

        foreach (var candidate in accounts
                     .Where(a => !excluded.Contains(a.Id))
                     .Where(a => a.Id == keepParentId || (!a.IsLeaf && a.IsActive))
                     .OrderBy(a => a.AccountCode, StringComparer.Ordinal))
        {
            options.Add(new ParentOption(candidate.Id, $"{candidate.AccountCode} — {candidate.NameAr}"));
        }

        var wasBinding = _binding;
        _binding = true;
        try
        {
            cbParent.DataSource = null;
            cbParent.DisplayMember = nameof(ParentOption.Text);
            cbParent.DataSource = options;
        }
        finally
        {
            _binding = wasBinding;
        }

        // الأب المطلوب من الشجرة يُستأنف إن كانت القائمة لم تكن جاهزة وقت الطلب (لم يختر المستخدم بعد).
        _SelectParent(previousParentId ?? _requestedParentId);
    }

    /// <summary>يعبّئ كل الحقول من الحساب المحدد؛ <c>null</c> ⇒ إعادة ضبط المحرّر.</summary>
    /// <param name="account">الحساب المعروض (يُتوقع وصوله من الشبكة/الشجرة).</param>
    private void LoadAccount(AccountDto? account)
    {
        if (account is null)
        {
            ResetEditor();
            return;
        }

        _pendingChildCodeParentId = null;   // يُبطل أي معاينة كود لا تزال في طريقها
        _requestedParentId = null;          // الأب يأتي من السجل المعروض لا من طلب سابق

        // إعادة بناء الآباء باستثناء الحساب وفروعه قبل تحديد أبيه الفعلي.
        BindParents(_accounts, account.Id);

        var wasBinding = _binding;
        _binding = true;
        try
        {
            txtAccountCode.Text = account.AccountCode;   // للقراءة فقط — الكود لا يُغيَّر بعد الإنشاء
            txtNameAr.Text = account.NameAr;
            txtNameEn.Text = account.NameEn ?? string.Empty;
            txtDescription.Text = account.Description ?? string.Empty;
            txtNotes.Text = account.Notes ?? string.Empty;

            _SelectNature(account.Nature);
            _SelectNormalBalance(account.NormalBalance);
            _SelectParent(account.ParentId);

            // نوع الحساب: القراءة من الـDTO إلى الخيار المطابق — مكان الربط الوحيد (داخل ← خيار).
            var accountTypeOption = cbAccountType.Items.OfType<AccountTypeOption>()
                .FirstOrDefault(o => o.IsLeaf == account.IsLeaf);
            if (accountTypeOption is not null) cbAccountType.SelectedItem = accountTypeOption;

            chkIsSystem.Checked = account.IsSystem;      // للعرض فقط — لا يُغيَّر بعد الإنشاء
            chkAllowCostCenter.Checked = account.AllowCostCenter;
        }
        finally
        {
            _binding = wasBinding;
        }

        _ClearValidation();
        _ApplyModeState();
    }

    /// <summary>
    /// يُفرّغ المحرّر ويُعيده إلى الحالة الافتراضية، ويحدّد الأب المطلوب إن مُرِّر،
    /// ثم يطلب معاينة كود الحساب الجديد في وضع الإضافة.
    /// </summary>
    /// <param name="preselectedParentId">أب يُختار مسبقاً (إضافة فرعي من الشجرة) — <c>null</c> = الجذر.</param>
    private void ResetEditor(int? preselectedParentId = null)
    {
        _pendingChildCodeParentId = null;

        var wasBinding = _binding;
        _binding = true;
        try
        {
            txtAccountCode.Clear();
            txtNameAr.Clear();
            txtNameEn.Clear();
            txtDescription.Clear();
            txtNotes.Clear();

            if (cbNature.Items.Count > 0) cbNature.SelectedIndex = 0;

            _SelectNormalBalance(0);          // «تلقائي (من نوع الحساب)»

            // الحساب الجديد يبدأ «رئيسي» (تجميعي): المستخدم يحوّله إلى «فرعي» عندما سيُرحَّل عليه.
            if (cbAccountType.Items.Count > 0) cbAccountType.SelectedIndex = 0;

            chkIsSystem.Checked = false;
            chkAllowCostCenter.Checked = false;
        }
        finally
        {
            _binding = wasBinding;
        }

        // إضافة جديدة: لا استثناء (excludeId = null) — كل الآباء المؤهَّلة متاحة.
        _requestedParentId = null;          // لا يُستأنف أب طُلب في إضافة سابقة
        BindParents(_accounts, null);

        _requestedParentId = preselectedParentId;

        if (preselectedParentId is int parentId) _SelectParent(parentId);

        _ClearValidation();
        _ApplyModeState();

        if (_mode == CrudMode.Adding) _ = RefreshCodePreviewAsync();

        if (_mode != CrudMode.Browsing && txtNameAr.CanFocus) txtNameAr.Focus();
    }

    /// <summary>
    /// يبني طلب **إنشاء** حساب من محتوى المحرّر — والتحقق كله هنا (بيت واحد).
    /// </summary>
    /// <param name="request">الطلب الجاهز للإرسال إلى عقد الخدمة (إنشاء حساب).</param>
    /// <returns><c>false</c> مع رسالة عربية في <c>lblValidationHint</c> ونقل التركيز لأول ناقص.</returns>
    private bool TryBuildCreate(out CreateAccountRequest request)
    {
        request = null!;

        if (!_ValidateEditor()) return false;

        // حصر نوع للمُصرِّف فقط (لا حكم ثانٍ): الحكم والرسالة العربية أعلاه في `_ValidateEditor`
        if (cbNature.SelectedItem is not NatureOption nature) return false;

        // نوع الحساب ⇐ IsLeaf: القراءة من الخيار المختار — المكان الوحيد للربط (خيار ← طلب).
        if (cbAccountType.SelectedItem is not AccountTypeOption accountType) return false;

        request = new CreateAccountRequest(
            NameAr: txtNameAr.Text.Trim(),
            NameEn: txtNameEn.Text.Trim().ToNullIfEmpty(),
            Nature: nature.Value,
            IsLeaf: accountType.IsLeaf,
            ParentId: _SelectedParentId(),
            // الحسابات النظامية تُنشأ من بذور/مزامنة النظام فقط، والخدمة تتجاهل هذا العلم —
            // فإرسال ما لا يُطبَّق نجاح كاذب (RULE-N167): القيمة ثابتة false.
            IsSystem: false,
            CategoryId: null,                       // لا حقل تصنيف في هذا المحرر
            Description: txtDescription.Text.Trim().ToNullIfEmpty(),
            Notes: txtNotes.Text.Trim().ToNullIfEmpty(),
            OpeningBalance: null,                   // لا حقل رصيد افتتاحي هنا (يُسجَّل بالقيد الافتتاحي)
            NormalBalance: _SelectedNormalBalance(),
            AllowCostCenter: chkAllowCostCenter.Checked);

        return true;
    }

    /// <summary>
    /// يبني طلب **تعديل** الحساب المعروض — والتحقق كله هنا (بيت واحد).
    /// </summary>
    /// <param name="request">الطلب الجاهز للإرسال إلى عقد الخدمة (تعديل حساب).</param>
    /// <returns><c>false</c> مع رسالة عربية ولا شيء غير ذلك (بلا صندوق رسالة).</returns>
    private bool TryBuildUpdate(out UpdateAccountRequest request)
    {
        request = null!;

        // الحساب النظامي يرفضه BLL — فلا يُبنى طلب لعملية مرفوضة (RULE-N167).
        if (_CurrentAccount is { IsSystem: true })
        {
            _SetValidationHint(SystemAccountMessage);
            return false;
        }

        if (!_ValidateEditor()) return false;

        // حصر نوع للمُصرِّف فقط (لا حكم ثانٍ): الحكم والرسالة العربية أعلاه في `_ValidateEditor`
        if (cbNature.SelectedItem is not NatureOption nature) return false;

        // نوع الحساب ⇐ IsLeaf: القراءة من الخيار المختار — المكان الوحيد للربط (خيار ← طلب).
        if (cbAccountType.SelectedItem is not AccountTypeOption accountType) return false;

        request = new UpdateAccountRequest(
            NameAr: txtNameAr.Text.Trim(),
            NameEn: txtNameEn.Text.Trim().ToNullIfEmpty(),
            Nature: nature.Value,
            IsLeaf: accountType.IsLeaf,
            ParentId: _SelectedParentId(),
            // التصنيف ليس له حقل هنا — يُمرَّر المحفوظ كما هو بدل تصفيره (فقدان صلة صامت).
            CategoryId: _CurrentAccount?.CategoryId,
            Description: txtDescription.Text.Trim().ToNullIfEmpty(),
            Notes: txtNotes.Text.Trim().ToNullIfEmpty(),
            NormalBalance: _SelectedNormalBalance(),
            AllowCostCenter: chkAllowCostCenter.Checked);

        return true;
    }

    /// <summary>
    /// معاينة «كود الابن التالي» للأب المختار في وضع الإضافة (RULE-N25/N26).
    /// للعرض فقط: الكود الحقيقي يولّده النظام عند الحفظ، ولا يُرسَل مع أي طلب.
    /// </summary>
    private async Task RefreshCodePreviewAsync()
    {
        if (IsDisposed || Disposing) return;
        if (!IsHandleCreated) return;                        // لا طلب قبل أن تصير للشاشة واجهة حيّة
        if (_binding || _mode != CrudMode.Adding) return;    // المعاينة تخصّ الإضافة وحدها

        int? parentId = _SelectedParentId();

        // «بلا أب — حساب رئيسي»: لا كود أبوي يُبنى عليه ⇒ يُفرَّغ الحقل ويُولّد النظام الكود عند الحفظ.
        if (parentId is null)
        {
            _pendingChildCodeParentId = null;
            txtAccountCode.Clear();
            return;
        }

        _pendingChildCodeParentId = parentId;   // حارس الردود المتأخرة: آخر أب مطلوب فقط

        var previousCursor = Cursor;
        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await AccountService.GetNextChildCodeAsync(parentId.Value);

            if (IsDisposed || Disposing) return;
            if (_pendingChildCodeParentId != parentId) return;   // ردّ متأخر لأب سابق — يُهمل

            // الرفض لا يُعرَض هنا: المعاينة للعرض فقط، ورسالة الرفض الحقيقية تظهر عند الحفظ.
            txtAccountCode.Text = result.IsSuccess && result.Value is not null
                ? result.Value.Code
                : string.Empty;
        }
        catch (Exception ex)
        {
            // الحدّ الوحيد لهذا المسار (RULE-N205)
            Log.Error(ex, "دليل الحسابات: تعذر معاينة كود الحساب للأب {ParentId}", parentId);

            if (!IsDisposed && !Disposing && _pendingChildCodeParentId == parentId)
                txtAccountCode.Clear();
        }
        finally
        {
            if (!IsDisposed && !Disposing) Cursor = previousCursor;
        }
    }

    // ── التحقق: حكم «مطلوب» في بيته الواحد، وفحوص المحرّر هنا ──────────────────

    /// <summary>يوصّل قاعدة «اسم الحساب مطلوب» بالنموذج مرة واحدة (RULE-N183).</summary>
    private void _AttachRequiredFields()
    {
        RequiredFieldHelper.Attach(this, errorProvider, new RequiredField(txtNameAr, NameRequiredMessage));
    }

    /// <summary>
    /// يقيّم المحرر: الحقل المطلوب من البيت الواحد <see cref="RequiredFieldHelper"/>،
    /// وفحوص هذه الشاشة (النوع · طبيعة الرصيد · دورة الأب) هنا.
    /// </summary>
    private bool _ValidateEditor()
    {
        _SetValidationHint(null);

        if (!RequiredFieldHelper.Validate(this))
        {
            var missing = RequiredFieldHelper.FirstMissing(this);

            if (missing is not null)
            {
                missing.Focus();

                // الرسالة العربية نفسها التي وسمها البيت الواحد — لا نص ثانٍ هنا.
                var message = errorProvider.GetError(missing);
                _SetValidationHint(string.IsNullOrWhiteSpace(message) ? NameRequiredMessage : message);
            }
            else
            {
                _SetValidationHint(NameRequiredMessage);
            }

            return false;
        }

        // ── فحوص المحرر (ليست حكم «فارغ») ────────────────────────────────────
        errorProvider.SetError(cbNature, string.Empty);
        errorProvider.SetError(cbNormalBalance, string.Empty);
        errorProvider.SetError(cbParent, string.Empty);

        bool isValid = true;
        Control? firstInvalid = null;
        string? firstMessage = null;

        void Refuse(Control control, string message)
        {
            errorProvider.SetError(control, message);
            clsInputFeedback.ShowRejectTooltip(control, message, toolTip);

            isValid = false;
            firstInvalid ??= control;
            firstMessage ??= message;
        }

        // النوع وطبيعة الرصيد: قوائم مبنية بعناصر بلا ValueMember فيقيسها الـHelper فارغة
        // (SelectedValue) — فحكمها هنا صراحةً.
        if (cbNature.SelectedItem is not NatureOption)
            Refuse(cbNature, NatureRequiredMessage);

        if (cbNormalBalance.SelectedItem is not NormalBalanceOption)
            Refuse(cbNormalBalance, NormalBalanceRequiredMessage);

        // دورة الشجرة: الحساب نفسه أو أحد فروعه لا يكون أباً لنفسه.
        if (_CurrentAccount is { } current
            && _SelectedParentId() is int parentId
            && (parentId == current.Id || _IsDescendantOf(parentId, current.Id)))
        {
            Refuse(cbParent, ParentCycleMessage);
        }

        // «تجميعي ⇒ حركة» لا يُحكَم هنا: ممنوع فحص الأبناء في المحرّر (لا استدعاء خدمة)،
        // والتنبيه المعلوماتي يظهر عند تغيير النوع، والحكم النهائي للـBLL ونصّه العربي يُعرَض في الشاشة.

        if (!isValid)
        {
            firstInvalid?.Focus();
            _SetValidationHint(firstMessage);
        }

        return isValid;
    }

    /// <summary>هل الحساب <paramref name="accountId"/> فرع من <paramref name="ancestorId"/>؟ (حدّ دورات 64).</summary>
    private bool _IsDescendantOf(int accountId, int ancestorId)
    {
        var current = accountId;

        for (var guard = 0; guard < 64; guard++)
        {
            if (!_accountsById.TryGetValue(current, out var node) || node.ParentId is not int parentId)
                return false;

            if (parentId == ancestorId) return true;

            current = parentId;
        }

        return false;
    }

    /// <summary>يمسح أيقونات الخطأ وقواعد «مطلوب» وسطر التنبيه.</summary>
    private void _ClearValidation()
    {
        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);
        lblValidationHint.Text = string.Empty;
    }

    /// <summary>يكتب رسالة عربية في سطر «صحّة البيانات» (فارغ ⇒ يُفرَّغ السطر).</summary>
    private void _SetValidationHint(string? message)
    {
        lblValidationHint.Text = message ?? string.Empty;
    }

    // ── القوائم والتحديد (القيمة لا النص — RULE-N60) ──────────────────────────

    private void _LoadNatureOptions()
    {
        cbNature.Items.Clear();
        cbNature.Items.Add(new NatureOption(1, "أصول"));
        cbNature.Items.Add(new NatureOption(2, "خصوم"));
        cbNature.Items.Add(new NatureOption(3, "حقوق ملكية"));
        cbNature.Items.Add(new NatureOption(4, "إيرادات"));
        cbNature.Items.Add(new NatureOption(5, "مصروفات"));
        cbNature.SelectedIndex = 0;
    }

    /// <summary>
    /// طبيعة الرصيد: «تلقائي» أولاً لأنه الافتراضي — ويُصان بالدوران الحقيقي:
    /// القيمة <c>0</c> في الـDTO تعني «لم تُحدَّد» فتُشتق من النوع، وإرسال مدين/دائن مكانها
    /// يُغيّر سلوك الحساب صامتاً عند أول تعديل.
    /// </summary>
    private void _LoadNormalBalanceOptions()
    {
        cbNormalBalance.Items.Clear();
        cbNormalBalance.Items.Add(new NormalBalanceOption(null, "تلقائي (من نوع الحساب)"));
        cbNormalBalance.Items.Add(new NormalBalanceOption(1, "مدين"));
        cbNormalBalance.Items.Add(new NormalBalanceOption(2, "دائن"));
        cbNormalBalance.SelectedIndex = 0;
    }

    private void _SelectNature(byte nature)
    {
        var option = cbNature.Items.OfType<NatureOption>().FirstOrDefault(o => o.Value == nature);
        cbNature.SelectedIndex = option is null ? -1 : cbNature.Items.IndexOf(option);
    }

    /// <summary>
    /// «نوع الحساب» كما يراه المستخدم — خياران فقط يُترجَمان إلى <c>IsLeaf</c> في الخلفية.
    /// الترتيب مقصود: «رئيسي» أولاً (الافتراضي للحساب الجديد) و«فرعي» ثانياً.
    /// </summary>
    private void _LoadAccountTypeOptions()
    {
        var options = new List<AccountTypeOption>
        {
            new("رئيسي (تجميعي — لا يقبل قيوداً)", IsLeaf: false),
            new("فرعي (حساب حركة نهائي — يقبل القيود)", IsLeaf: true)
        };

        cbAccountType.DataSource = null;   // آمن للاستدعاء المتكرر: Items.Clear لا يعمل فوق DataSource
        cbAccountType.Items.Clear();
        cbAccountType.DisplayMember = nameof(AccountTypeOption.Display);
        cbAccountType.DataSource = options;
        cbAccountType.SelectedIndex = 0;
    }

    private void _SelectNormalBalance(byte stored)
    {
        byte? value = stored switch
        {
            1 => 1,
            2 => 2,
            _ => null
        };

        var option = cbNormalBalance.Items.OfType<NormalBalanceOption>().FirstOrDefault(o => o.Value == value)
                     ?? cbNormalBalance.Items.OfType<NormalBalanceOption>().FirstOrDefault(o => o.Value is null);

        if (option is not null) cbNormalBalance.SelectedItem = option;
    }

    /// <summary>يحدّد الأب المطلوب في القائمة، ويسقط إلى «بلا أب» إن لم يكن معروضاً.</summary>
    private void _SelectParent(int? parentId)
    {
        var wasBinding = _binding;
        _binding = true;
        try
        {
            if (cbParent.Items.Count == 0) return;

            var match = cbParent.Items.OfType<ParentOption>().FirstOrDefault(o => o.Id == parentId);
            cbParent.SelectedItem = match ?? cbParent.Items[0];
        }
        finally
        {
            _binding = wasBinding;
        }
    }

    private int? _SelectedParentId() => (cbParent.SelectedItem as ParentOption)?.Id;

    private byte? _SelectedNormalBalance() => (cbNormalBalance.SelectedItem as NormalBalanceOption)?.Value;

    /// <summary>معرّفات الحساب المستثنى وكل فروعه وأحفاده (حدّ دورات يمنع حلقة لا نهائية).</summary>
    private static HashSet<int> _BuildExclusionSet(IReadOnlyList<AccountDto> accounts, int? excludeId)
    {
        var excluded = new HashSet<int>();

        if (excludeId is not int rootId) return excluded;

        excluded.Add(rootId);

        var childrenByParent = accounts
            .Where(a => a.ParentId is not null)
            .ToLookup(a => a.ParentId!.Value);

        var queue = new Queue<int>();
        queue.Enqueue(rootId);

        for (var guard = 0; queue.Count > 0 && guard < 10_000; guard++)
        {
            var currentId = queue.Dequeue();

            foreach (var child in childrenByParent[currentId])
            {
                if (excluded.Add(child.Id)) queue.Enqueue(child.Id);
            }
        }

        return excluded;
    }

    // ── معالجات المحرّر ──────────────────────────────────────────────────────

    /// <summary>تغيّر الأب ⇒ معاينة كود الابن (وضع الإضافة فقط).</summary>
    private void cbParent_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_binding) return;

        _requestedParentId = null;   // اختيار المستخدم يلغي أي أب مطلوب سابقاً من الشجرة

        _ = RefreshCodePreviewAsync();
    }

    /// <summary>
    /// تغيّر «نوع الحساب»: تنبيه **معلوماتي** قبل الحفظ.
    /// <para>
    /// تحويل حساب تجميعي (محفوظ <c>IsLeaf = false</c>) إلى «فرعي» مسموح في الواجهة، لكن
    /// الخدمة ترفضه إن كان له حسابات فرعية — فنُظهر التنبيه مبكراً، و**لا نفحص الأبناء هنا**
    /// (لا استدعاء خدمة؛ RULE-N205 كما هو) والحكم النهائي للـBLL ونصّه العربي يُعرَض في الشاشة.
    /// </para>
    /// </summary>
    private void cbAccountType_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_binding) return;

        bool goesToPosting = (cbAccountType.SelectedItem as AccountTypeOption)?.IsLeaf == true;
        bool wasGroup = _CurrentAccount is { IsLeaf: false };

        if (goesToPosting && wasGroup)
        {
            _SetValidationHint(GroupToPostingWarningMessage);
            return;
        }

        // التراجع إلى «رئيسي» يرفع تنبيهنا فقط — ولا يمسح رسالة تحقق أخرى.
        if (lblValidationHint.Text == GroupToPostingWarningMessage) _SetValidationHint(null);
    }

    // ── Load ─────────────────────────────────────────────────────────────────

    private async void frmAccounts_Load(object? sender, EventArgs e)
        => await _RunSafeAsync(async () =>
        {
            await _LoadAsync();
            _ResetForm();
        }, LoadUnexpectedMessage);

    /// <summary>
    /// «تحديث» (F5) — تحديث **كامل** (RULE-N148): تُمسح أدوات التصفية أولاً، ثم تُعاد قراءة
    /// الحسابات (ومنها خيارات المحرّر والشجرة) وتُعاد الشبكة والملخص، ويُعاد التحديد
    /// والمحرّر إلى الحالة الافتراضية — ولا يقتصر على مسح التصفية.
    /// </summary>
    private async Task _RefreshAsync()
    {
        if (_actionButtonsBusy) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();
            _editId = null;
            _mode = CrudMode.Browsing;

            await _LoadAsync();
            _ResetForm();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ─────────────────────────────────────────────────────────

    /// <summary>
    /// يحمّل كل الحسابات (بما فيها غير النشطة) ثم يبني جدول العرض ويطبّق التصفية،
    /// ويُغذّي عنصري المحرّر والشجرة من نفس القراءة (لا قراءة ثانية للشاشة).
    /// </summary>
    private async Task _LoadAsync()
    {
        var result = await AccountService.GetAllAsync(includeInactive: true);

        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? LoadFailedMessage);
            return;
        }

        _accounts.Clear();
        _accountsById.Clear();

        foreach (var account in result.Value)
        {
            _accounts.Add(account);
            _accountsById[account.Id] = account;
        }

        var table = _BuildGridTable();
        _totalCount = table.Rows.Count;

        _syncingSelection = true;
        try
        {
            _accountsView = new DataView(table);
            dgvAccounts.DataSource = _accountsView;
        }
        finally
        {
            _syncingSelection = false;
        }

        // الدلائل: آباء المحرّر (واستثناء فروعه) + الشجرة — تحت حارس واحد حتى لا يُشعل
        // تحديث الشجرة حدث اختيار يفتح المحرّر أثناء التحميل.
        var wasSyncing = _syncingSelection;
        _syncingSelection = true;
        try
        {
            BindParents(_accounts, _editId);
            await Tree.RefreshAsync();
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }

        _ApplyFilter();

        if (_editId is int selectedId && _accountsById.ContainsKey(selectedId))
        {
            _SelectGridRow(selectedId);
            _SelectTreeRow(selectedId);
        }
        else
        {
            _editId = null;
            _ClearGridSelection();
        }

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>يبني جدول عرض الشاشة: أعمدة الشبكة الـ13 + أعمدة الحكم/التصفية — مصدر واحد.</summary>
    private DataTable _BuildGridTable()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("AccountCode", typeof(string));
        table.Columns.Add("NameAr", typeof(string));
        table.Columns.Add("NameArIndented", typeof(string));
        table.Columns.Add("NameEn", typeof(string));
        table.Columns.Add("NatureDisplay", typeof(string));
        table.Columns.Add("LevelDisplay", typeof(string));
        table.Columns.Add("ParentAccountName", typeof(string));
        table.Columns.Add("IsLeaf", typeof(bool));
        table.Columns.Add("IsSystem", typeof(bool));
        table.Columns.Add("IsActive", typeof(bool));
        table.Columns.Add("IsLeafText", typeof(string));
        table.Columns.Add("IsSystemText", typeof(string));
        table.Columns.Add("Status", typeof(string));
        table.Columns.Add("NatureValue", typeof(byte));
        table.Columns.Add("TreePath", typeof(string));            // مشتق في الـDTO — لا يُخزَّن
        table.Columns.Add("Balance", typeof(string));             // مُنسَّق بـ clsFormat.Money (البيت الواحد)
        table.Columns.Add("FinancialStatement", typeof(string));  // عربي جاهز من الـ DTO
        table.Columns.Add("Category", typeof(string));            // التصنيف أو «حساب مالي عام»

        foreach (var account in _accounts.OrderBy(a => a.AccountCode, StringComparer.Ordinal))
        {
            table.Rows.Add(
                account.Id,
                account.AccountCode,
                account.NameAr,
                _IndentedName(account),
                account.NameEn ?? string.Empty,
                account.NatureDisplay,      // عربي جاهز من الـ DTO — لا نُعيد تعيينه
                account.LevelDisplay,       // عربي جاهز من الـ DTO
                account.ParentAccountName ?? string.Empty,
                account.IsLeaf,
                account.IsSystem,
                account.IsActive,
                account.IsLeaf ? "نعم" : "لا",
                account.IsSystem ? "نعم" : "لا",
                account.IsActive ? "نشط" : "معطّل",
                account.Nature,
                account.TreePath,
                clsFormat.Money(account.Balance),   // RULE-N169: التنسيق من البيت الواحد لا من الشاشة
                account.FinancialStatementDisplay,  // عربي جاهز من الـ DTO
                account.CategoryLabel ?? "حساب مالي عام");
        }

        return table;
    }

    /// <summary>إبراز التسلسل الهرمي في عمود الاسم العربي دون TreeView (RULE: عرض هرمي).</summary>
    private static string _IndentedName(AccountDto account)
        => account.Level > 1
            ? new string(' ', (account.Level - 1) * 2) + "└ " + account.NameAr
            : account.NameAr;

    // ── آلة الحالة الوحيدة (§2.36 / نظير RULE-N150) ───────────────────────────

    /// <summary>
    /// المكان **الوحيد** الذي يُغذّي الشريط المشترك ويضبط تفعيل أزرار الشاشة
    /// (حذف نهائي · مسح التصفية · بنود قائمة السياق) **وحقول الحساب** — آلة واحدة
    /// لا ثانية (§2.36 / النظير RULE-N150).
    /// تُستدعى بعد كل انتقال: تحميل · إضافة · اختيار · حفظ · تعطيل · تحديث · تصفية.
    /// </summary>
    private void _ApplyModeState()
    {
        bool busy = _actionButtonsBusy;
        bool hasSelection = _editId is int selectedId && _accountsById.ContainsKey(selectedId);
        bool systemAccount = hasSelection && _accountsById[_editId!.Value].IsSystem;

        // ── المحرّر: حقول الحساب (§2.36) ──────────────────────────────────────
        bool browsing = _mode == CrudMode.Browsing;

        // الحساب النظامي للقراءة فقط دائماً: الخدمة ترفض تعديله، فلا تُفتح حقول لا يمكن حفظها.
        bool editable = !browsing && !systemAccount;

        txtAccountCode.Enabled = !browsing;   // للقراءة دائماً (ReadOnly) — يُظهر كود المعاينة أو المحفوظ
        txtNameAr.Enabled = editable;
        txtNameEn.Enabled = editable;
        cbParent.Enabled = editable;
        cbNature.Enabled = editable;
        cbNormalBalance.Enabled = editable;
        cbAccountType.Enabled = editable;
        chkAllowCostCenter.Enabled = editable;
        txtDescription.Enabled = editable;
        txtNotes.Enabled = editable;

        // «حساب نظامي»: لا يُنشأ من الواجهة (معطّل في الإضافة والعرض)، وفي التعديل للقراءة فقط
        // (AutoCheck = false يُبقي مظهره الطبيعي مع منع التغيير — وهو معنى «read-only» لمربع اختيار).
        chkIsSystem.AutoCheck = false;
        chkIsSystem.Enabled = _mode == CrudMode.Editing && !systemAccount;

        // أزرار الحفظ/الإلغاء المحلية حُذفت (TASK-011): الشريط المشترك يحملها
        // (`SaveRequested`/`CancelEditRequested` → `_OnSaveRequested`/`_OnCancelRequested`).

        // سطر التنبيه لسجل نظامي مفتوح — والمسح عند كل انتقال يملكه _ClearValidation
        // (فلا تمحو كتابةٌ في التصفية رسالةً لم يقرأها المستخدم).
        if (!browsing && systemAccount) _SetValidationHint(SystemAccountMessage);

        // الشريط المشترك: ثلاثة مدخلات فقط ويحكم هو على أزراره (RULE-N169).
        Toolbar.SetMode(_mode);
        Toolbar.HasSelection = hasSelection;
        Toolbar.AllowDelete = hasSelection && !systemAccount && !busy;
        Toolbar.ShowSave = true;
        Toolbar.ShowClose = true;

        // الصف الثانوي الخاص بالشاشة + قائمة سياق الشبكة: نفس المدخلات فلا حكم ثانٍ.
        btnPermanentDelete.Enabled = hasSelection && !systemAccount && !busy;
        cmsEdit.Enabled = hasSelection && !busy;
        cmsDelete.Enabled = hasSelection && !systemAccount && !busy;

        // RULE-N148: «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الافتراضي.
        btnClearFilters.Enabled = _HasActiveFilter() && !busy;

        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي لـ <c>Enter</c> (RULE-N147) — <c>null</c> بقصد: أزرار الوضع تعيش في
    /// <see cref="ctrlCrudToolbar"/> الذي لا يُعرِّض زرّه، وبقاء <c>AcceptButton</c> فارغاً
    /// يضمن أن <c>Enter</c> لا يحفظ ولا يُرحّل من أي حقل أو شبكة (RULE-N153/RULE-N193).
    /// والانتقال بين حقول الحساب بـ<c>Enter</c> يملكه استدعاء واحد لـ<see cref="KeyboardHelper"/>
    /// في المُنشئ (RULE-N191) — بلا زر وضع محلي.
    /// </summary>
    private Button? _PrimaryActionButton() => null;

    // ── Transitions — كل انتقال يمرّ من هنا ───────────────────────────────────

    /// <summary>
    /// «جديد» / F2 / «إضافة فرعي»: يفتح المحرّر للإضافة مع الأب المطلوب (أو المحدد في الشجرة).
    /// </summary>
    private void _StartNewEntry(int? parentId)
    {
        _editId = null;
        _mode = CrudMode.Adding;

        _ClearGridSelection();

        ResetEditor(parentId);

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>
    /// انتقال إلى «تعديل»/«عرض» السجل — المصدر الوحيد للتحديد من الشجرة **و** الشبكة.
    /// </summary>
    private void _OnRecordSelected(int accountId)
    {
        if (_syncingSelection) return;
        if (!_accountsById.TryGetValue(accountId, out var account)) return;

        _syncingSelection = true;
        try
        {
            _editId = account.Id;
            _mode = CrudMode.Editing;

            // بيانات السجل ثم الوضع (الترتيب يهمّ: تعبئة الحقول لا تركّز حقلاً، والوضع يفعّلها).
            LoadAccount(account);

            _SelectGridRow(account.Id);
            _SelectTreeRow(account.Id);
        }
        finally
        {
            _syncingSelection = false;
        }

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض**: بلا تحديد، أو مع السجل المطلوب (بعد الحفظ/الإلغاء).
    /// </summary>
    private void _ResetForm(int? selectId = null)
    {
        _editId = selectId;
        _mode = CrudMode.Browsing;

        if (selectId is int id)
        {
            _SelectGridRow(id);
            _SelectTreeRow(id);
        }
        else
        {
            _ClearGridSelection();
        }

        LoadAccount(_CurrentAccount);

        _UpdateModeHeader();
        _ApplyModeState();
    }

    /// <summary>RULE-N44: مؤشر الوضع أعلى الشاشة — يتحدث مع كل تغيير حالة.</summary>
    private void _UpdateModeHeader()
    {
        if (_mode == CrudMode.Adding)
        {
            // نفس أهلية خيارات «الحساب الأب» في المحرّر: تُذكر صلة الأب فقط إن كان الحساب
            // المحدد في الشجرة مؤهَّلاً فعلاً (غير ورقي ونشط) — وإلا فالإضافة إلى الجذر.
            lblMode.Text = Tree.SelectedAccountId is int parentId
                           && _accountsById.TryGetValue(parentId, out var parent)
                           && !parent.IsLeaf
                           && parent.IsActive
                ? $"دليل الحسابات — إضافة حساب فرعي تحت: {parent.NameAr}"
                : "دليل الحسابات — إضافة حساب جديد";
            return;
        }

        if (_editId is int id && _accountsById.TryGetValue(id, out var account))
        {
            lblMode.Text = account.IsSystem
                ? $"دليل الحسابات — حساب نظامي (عرض فقط): {account.NameAr}"
                : _mode == CrudMode.Editing
                    ? $"دليل الحسابات — تعديل الحساب: {account.NameAr}"
                    : $"دليل الحسابات — وضع العرض: {account.NameAr}";
            return;
        }

        lblMode.Text = "دليل الحسابات — وضع العرض";
    }

    // ── Grid ─────────────────────────────────────────────────────────────────

    private void dgvAccounts_SelectionChanged(object? sender, EventArgs e)
    {
        if (_syncingSelection) return;
        if (_SelectedAccountId() is not int id) return;
        if (_editId == id && _mode == CrudMode.Editing) return;

        _OnRecordSelected(id);
    }

    private void dgvAccounts_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (_SelectedAccountId() is int id)
        {
            _OnRecordSelected(id);
            txtNameAr.Focus();   // نقر مزدوج = تعديل مباشر (RULE §2.21 #14)
        }
    }

    private void dgvAccounts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvAccounts.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;

        // تصفير صريح قبل الحكم: الخلية قد تكون معاد استخدامها بعد إعادة الربط فيبقى لون حالة سابق
        e.CellStyle.BackColor = Color.Empty;
        e.CellStyle.ForeColor = Color.Empty;
        e.CellStyle.SelectionForeColor = Color.Empty;

        if (row["IsActive"] is bool isActive && !isActive)
        {
            e.CellStyle.BackColor = GreenThemeHelper.StatusDangerBack;
            e.CellStyle.ForeColor = GreenThemeHelper.StatusDanger;
            e.CellStyle.SelectionForeColor = GreenThemeHelper.StatusDanger;
        }
    }

    private int? _SelectedAccountId()
        => dgvAccounts.CurrentRow?.DataBoundItem is DataRowView row && row["Id"] is int id ? id : null;

    private void _SelectGridRow(int accountId)
    {
        foreach (DataGridViewRow row in dgvAccounts.Rows)
        {
            if (row.DataBoundItem is not DataRowView view || view["Id"] is not int rowId || rowId != accountId)
                continue;

            var wasSyncing = _syncingSelection;
            _syncingSelection = true;
            try
            {
                row.Selected = true;
                dgvAccounts.CurrentCell = row.Cells["colNameAr"];
            }
            finally
            {
                _syncingSelection = wasSyncing;
            }

            break;
        }
    }

    private void _ClearGridSelection()
    {
        var wasSyncing = _syncingSelection;
        _syncingSelection = true;
        try
        {
            dgvAccounts.ClearSelection();
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }
    }

    /// <summary>تحديد العقدة في الشجرة بلا إشعال حدث الاختيار (يمنع دورة شبكة ↔ شجرة).</summary>
    private void _SelectTreeRow(int accountId)
    {
        var wasSyncing = _syncingSelection;
        _syncingSelection = true;
        try
        {
            Tree.SelectAccount(accountId);
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }
    }

    // ── Filter (RULE-N56…RULE-N63) — تصفية فورية بلا زر بحث ─────────────────

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbNatureFilter_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyLeaf_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlySystem_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyParents_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Escape) return;

        _ClearAllFilters();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة التحميل.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>هل توجد تصفية تختلف عن الافتراضي؟ (RULE-N148 — حكم واحد لزر «مسح التصفية»)</summary>
    private bool _HasActiveFilter()
        => !string.IsNullOrWhiteSpace(txtFilterValue.Text)
           || cbFilterBy.SelectedIndex > 0
           || cbNatureFilter.SelectedIndex > 0
           || chkShowInactive.Checked
           || chkOnlyLeaf.Checked
           || chkOnlySystem.Checked
           || chkOnlyParents.Checked
           || _treeFilterIds is not null;   // تصفية عقدة الشجرة تُحتسب تصفيةً نشطة (RULE-N148)

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        var wasSyncing = _syncingSelection;
        _syncingSelection = true; // التصفية لا تلمس المحرر: نمنع مزامنة التحديد أثناء المسح
        try
        {
            // تصفية عقدة الشجرة تُمسح أولاً: الحالة أولاً (فلا تُقرأ في تطبيق وسيط) ثم اختيار
            // الشجرة، والحارس مرفوع فلا يهرب منه حدث اختيار يفتح المحرّر أو يعيد التصفية.
            _treeFilterIds = null;
            _treeFilterLabel = null;
            Tree.ClearSelection();

            txtFilterValue.Clear();

            if (cbFilterBy.Items.Count > 0) cbFilterBy.SelectedIndex = 0;       // «الكود»
            if (cbNatureFilter.Items.Count > 0) cbNatureFilter.SelectedIndex = 0; // «كل الأنواع»

            chkShowInactive.Checked = false;
            chkOnlyLeaf.Checked = false;
            chkOnlySystem.Checked = false;
            chkOnlyParents.Checked = false;
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }
    }

    /// <summary>يمسح كل أدوات التصفية ثم يعيد تطبيق التصفية على نفس البيانات (بلا إعادة تحميل).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
    }

    private void _ApplyFilter()
    {
        if (_accountsView.Table is null)
        {
            _UpdateSummary();
            _UpdateEmptyState();
            _ApplyModeState();
            return;
        }

        var conditions = new List<string>();

        if (!chkShowInactive.Checked) conditions.Add("IsActive = true");
        if (chkOnlyLeaf.Checked) conditions.Add("IsLeaf = true");
        if (chkOnlyParents.Checked) conditions.Add("IsLeaf = false");
        if (chkOnlySystem.Checked) conditions.Add("IsSystem = true");

        if (cbNatureFilter.SelectedItem is NatureOption { Value: > 0 } nature)
            conditions.Add($"NatureValue = {nature.Value}");

        // تصفية عقدة الشجرة: تُركَّب مع بقية الأدوات فتعمل التصفيتان معاً (RULE-N59).
        // الشرط على عمود Id الرقمي وحده — لا نص مستخدم هنا، ولا حاجة لتهريب.
        // الشجرة الفارغة (طبيعة بلا حسابات) تُظهر صفراً صراحةً بدل «كل الحسابات».
        if (_treeFilterIds is { } treeIds)
            conditions.Add(treeIds.Count == 0
                ? "Id = -1"
                : "Id IN (" + string.Join(",", treeIds) + ")");

        // RULE-N60: العمود يُحدَّد بالـ SelectedIndex لا بمقارنة نص عربي،
        // والقيمة تُهرَّب في RowFilterHelper لا داخل الشاشة (RULE-N112).
        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            string column = cbFilterBy.SelectedIndex switch
            {
                1 => "NameAr",
                2 => "NameEn",
                _ => "AccountCode"
            };

            conditions.Add(RowFilterHelper.Like(column, text));
        }

        var wasSyncing = _syncingSelection;
        _syncingSelection = true; // تغيير التصفية لا ينقل المحرر إلى سطر آخر
        try
        {
            _accountsView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : string.Empty;
        }
        finally
        {
            _syncingSelection = wasSyncing;
        }

        // RULE-N150: التصفية قد تُخفي السجل المعروض — فلا يبقى «تعديل/تعطيل» فعّالاً على
        // سجل غير معروض، ولا يبقى المحرّر على سجل خارج الشبكة.
        if (_editId is int editedId && !_IsRowVisible(editedId))
            _ResetForm();

        _UpdateSummary();
        _UpdateEmptyState();
        _ApplyModeState();
    }

    /// <summary>هل السجل <paramref name="accountId"/> معروض في الشبكة بعد التصفية الحالية؟</summary>
    private bool _IsRowVisible(int accountId)
    {
        foreach (DataGridViewRow row in dgvAccounts.Rows)
        {
            if (row.DataBoundItem is DataRowView view && view["Id"] is int rowId && rowId == accountId)
                return true;
        }

        return false;
    }

    /// <summary>RULE-N61: شريط الملخص — عدد المعروض من الإجمالي + توزيع الأنواع.</summary>
    private void _UpdateSummary()
    {
        int shown = _accountsView.Table is null ? 0 : _accountsView.Count;

        int assets = 0, liabilities = 0, equity = 0, revenue = 0, expenses = 0;

        if (_accountsView.Table is not null)
        {
            foreach (DataRowView row in _accountsView)
            {
                switch (row["NatureValue"] is byte nature ? nature : (byte)0)
                {
                    case 1: assets++; break;
                    case 2: liabilities++; break;
                    case 3: equity++; break;
                    case 4: revenue++; break;
                    case 5: expenses++; break;
                }
            }
        }

        // الأعداد تُنسَّق من البيت الواحد clsFormat.Number — لا إدراج خام في النص (RULE-N169)
        string summary =
            $"المعروض: {clsFormat.Number(shown)} من {clsFormat.Number(_totalCount)} — "
            + $"أصول: {clsFormat.Number(assets)} · خصوم: {clsFormat.Number(liabilities)} · "
            + $"حقوق ملكية: {clsFormat.Number(equity)} · إيرادات: {clsFormat.Number(revenue)} · "
            + $"مصروفات: {clsFormat.Number(expenses)}";

        // عند تصفية عقدة الشجرة يُذكر اسم العقدة صريحاً: «المعروض» وحده لا يقول أي فرع نراه.
        if (_treeFilterLabel is { Length: > 0 } treeLabel) summary += " — العقدة: " + treeLabel;

        lblSummary.Text = summary;
    }

    /// <summary>عند عدم وجود نتائج تُعرض طبقة «لا توجد بيانات» مكان جسم الشبكة.</summary>
    private void _UpdateEmptyState()
    {
        bool isEmpty = _accountsView.Table is null || _accountsView.Count == 0;

        lblEmptyState.Visible = isEmpty;
        if (isEmpty) lblEmptyState.BringToFront();
    }

    // ── Toolbar events — الشريط المشترك يرفع، والشاشة تنفّذ ───────────────────

    private void Toolbar_AddRequested(object? sender, EventArgs e) => _StartNewEntry(Tree.SelectedAccountId);

    /// <summary>«تعديل» — يُفعّل عند وجود سجل محدد في وضع العرض فقط.</summary>
    private void Toolbar_EditRequested(object? sender, EventArgs e) => _StartEditFromSelection();

    /// <summary>«حفظ» — من الشريط المشترك ومن الزر الداخلي للمحرّر (مسار واحد).</summary>
    private async void _OnSaveRequested(object? sender, EventArgs e)
        => await _RunSafeAsync(_SaveAsync, SaveFailedMessage);

    /// <summary>«إلغاء التعديل» — من الشريط المشترك ومن الزر الداخلي للمحرّر (مسار واحد).</summary>
    private void _OnCancelRequested(object? sender, EventArgs e) => _ResetForm(_editId);

    private async void Toolbar_DeleteRequested(object? sender, EventArgs e)
        => await _RunSafeAsync(_DeleteAsync, DeleteFailedMessage);

    private async void Toolbar_RefreshRequested(object? sender, EventArgs e)
        => await _RunSafeAsync(_RefreshAsync, RefreshFailedMessage);

    private void Toolbar_CloseRequested(object? sender, EventArgs e) => Close();

    /// <summary>F3 / «تعديل»: ينتقل إلى وضع التعديل للسجل المحدد.</summary>
    private void _StartEditFromSelection()
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning(SelectFirstMessage);
            return;
        }

        _OnRecordSelected(id);

        if (_editId == id && _mode == CrudMode.Editing)
        {
            // المحرّر أُعيد تحميله في وضع التعديل: تُركّز أولى حقوله للكتابة الفورية.
            txtNameAr.Focus();
        }
    }

    // ── Tree events — العمليات كلها من الشريط (RULE-N100) ─────────────────────

    private void Tree_AccountSelected(object? sender, AccountTreeNodeDto? node)
    {
        if (_syncingSelection) return;

        // عقدة حساب: تُصفّى الشبكة على الحساب وفروعه **قبل** فتح السجل — تحديد السطر في
        // الشبكة يحتاج سطراً موجوداً، ولو أُخِّر الفتح لذهب المؤشر إلى سجل غير معروض.
        if (node is not null)
        {
            _treeFilterIds = new HashSet<int>(Tree.GetSubtreeIds(node.Id));
            _treeFilterLabel = node.AccountCode + " — " + node.NameAr;

            _ApplyFilter();
            _OnRecordSelected(node.Id);
            return;
        }

        // عقدة طبيعة (لا حساب لها): تُصفّى الشبكة على الطبيعة كاملة، ولا سجل يُفتح.
        if (Tree.SelectedNature is byte nature)
        {
            _treeFilterIds = new HashSet<int>(Tree.GetNatureIds(nature));
            _treeFilterLabel = "طبيعة: " + ctrlTreeOfAccounts.NatureName(nature);

            _ApplyFilter();
            return;
        }

        // ذهب الاختيار (لا حساب ولا طبيعة): تُرفع تصفية العقدة وحدها بلا إعادة تحميل.
        if (_treeFilterIds is null && _treeFilterLabel is null) return;

        _treeFilterIds = null;
        _treeFilterLabel = null;
        _ApplyFilter();
    }

    // ── Save / Delete ────────────────────────────────────────────────────────

    /// <summary>
    /// حفظ الحساب: <c>Adding</c> ⇒ <c>CreateAsync</c>، و<c>Editing</c> ⇒ <c>UpdateAsync</c>.
    /// الطلب يُبنى في هذه الشاشة (<c>TryBuildCreate/TryBuildUpdate</c>) — التحقق كله هناك،
    /// و<c>AccountCode</c> يولّده النظام فلا يُرسَل من الشاشة.
    /// </summary>
    private async Task _SaveAsync()
    {
        // حماية من النقر المزدوج: لا ندخل الحفظ مرتين قبل أن يُصفَّر العلم في finally.
        if (_actionButtonsBusy) return;

        bool isUpdate = _mode == CrudMode.Editing && _editId is not null;

        // الحارسان «|| x is null» لتدفّق النوع عند المُصرِّف فقط: TryBuild* لا يُعيد true
        // إلا بطلب صالح غير فارغ — فلا تغيير في السلوك.
        CreateAccountRequest? createRequest = null;
        UpdateAccountRequest? updateRequest = null;

        if (isUpdate)
        {
            if (!TryBuildUpdate(out var update) || update is null) return;   // الرسالة والحقل في المحرّر
            updateRequest = update;
        }
        else
        {
            if (!TryBuildCreate(out var create) || create is null) return;
            createRequest = create;
        }

        int? targetId = isUpdate ? _editId : null;

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            int? savedId;

            if (updateRequest is not null && targetId is int id)
            {
                var result = await AccountService.UpdateAsync(id, updateRequest);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ التعديل.\n{result.Error}");
                    return;
                }

                savedId = id;
            }
            else if (createRequest is not null)
            {
                var result = await AccountService.CreateAsync(createRequest);

                if (!result.IsSuccess)
                {
                    clsDialogs.Error($"لم يتم حفظ الحساب.\n{result.Error}");
                    return;
                }

                savedId = result.Value;
            }
            else
            {
                // لا طلب صالح: الحارسان أعلاه يمنعان الوصول إلى هنا — لا شيء يُحفظ.
                return;
            }

            clsDialogs.Success(updateRequest is not null ? "تم تعديل الحساب بنجاح." : "تمت إضافة الحساب بنجاح.");

            _editId = savedId;      // إعادة التحميل تُحدد السجل المحفوظ في الشبكة والشجرة
            await _LoadAsync();
            _ResetForm(savedId);    // وضع العرض مع بقاء السجل المحفوظ معروضاً
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تعطيل الحساب (حذف ناعم) — الحسابات النظامية يرفضها BLL وتُعرَض رسالته.</summary>
    private async Task _DeleteAsync()
    {
        if (_actionButtonsBusy) return;

        if (_editId is not int id || !_accountsById.TryGetValue(id, out var account))
        {
            clsDialogs.Warning(SelectFirstMessage);
            return;
        }

        if (account.IsSystem)
        {
            // الحساب النظامي محمي في الخدمة نفسها: لا تعديل ولا تعطيل ولا حذف نهائي.
            clsDialogs.Warning(SystemAccountMessage);
            return;
        }

        if (!clsDialogs.Confirm($"هل أنت متأكد من تعطيل الحساب \"{account.NameAr}\"؟\n" +
                                "لن يظهر في القوائم بعد التعطيل، ويمكن إظهاره بخيار «إظهار غير النشط»."))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            var result = await AccountService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعطيل الحساب بنجاح.");

                _ResetForm();
                await _LoadAsync();
            }
            else
            {
                clsDialogs.Error($"لم يتم تعطيل الحساب.\n{result.Error}");
            }
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// حذف نهائي — تحذير أقوى، ويبقى الزر معطّلاً للحسابات النظامية من آلة الحالة.
    /// كل رفض من BLL يُعرَض نصّه العربي كما هو.
    /// </summary>
    private async void btnPermanentDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id || !_accountsById.TryGetValue(id, out var account))
        {
            clsDialogs.Warning(SelectFirstMessage);
            return;
        }

        if (account.IsSystem)
        {
            clsDialogs.Warning("لا يمكن حذف حساب نظامي نهائياً.");
            return;
        }

        if (!clsDialogs.Confirm($"تحذير: سيتم حذف الحساب \"{account.NameAr}\" نهائياً من قاعدة البيانات " +
                                "ولا يمكن التراجع عن هذه العملية.\n" +
                                "إن كان الحساب مستخدماً في قيود أو فواتير فسيرفض النظام الحذف.\n\n" +
                                "هل أنت متأكد من الحذف النهائي؟"))
        {
            return;
        }

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _ApplyModeState();

        try
        {
            var result = await AccountService.PermanentDeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم حذف الحساب نهائياً.");

                _ResetForm();
                await _LoadAsync();
            }
            else
            {
                clsDialogs.Error($"لم يتم حذف الحساب نهائياً.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            // حدّ الأمان لمعالج async void (RULE-N25/N205): رسالة عربية بلا نص الاستثناء.
            Log.Error(ex, "خطأ أثناء الحذف النهائي للحساب {AccountId}", id);
            clsDialogs.Error(PermanentDeleteFailedMessage);
        }
        finally
        {
            _actionButtonsBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    // ── Shortcuts ────────────────────────────────────────────────────────────

    /// <summary>Esc: داخل صندوق التصفية يمسحها، وفي وضع الإضافة/التعديل يلغي، وإلا يُغلق.</summary>
    private void _HandleEscape()
    {
        if (txtFilterValue.Focused)
        {
            _ClearAllFilters();
            return;
        }

        if (_mode != CrudMode.Browsing)
        {
            _ResetForm(_editId);
            return;
        }

        Close();
    }

    /// <summary>Delete يُعطّل الحساب فقط في وضع العرض وعندما لا يكون المؤشر داخل حقل نصي.</summary>
    private void _HandleDeleteShortcut()
    {
        // الفحص عبر بيت واحد (RULE-N169): ActiveControl يعيد الحاوية لا الحقل المركَّز نفسه.
        if (KeyboardHelper.IsTextInputFocused(this)) return;
        if (_mode != CrudMode.Browsing) return;

        _ = _RunSafeAsync(_DeleteAsync, DeleteFailedMessage);
    }

    // ── Context menu (RULE-N23) ──────────────────────────────────────────────

    private void cmsEdit_Click(object? sender, EventArgs e) => _StartEditFromSelection();

    private void cmsDelete_Click(object? sender, EventArgs e) => _ = _RunSafeAsync(_DeleteAsync, DeleteFailedMessage);

    private async void cmsRefresh_Click(object? sender, EventArgs e)
        => await _RunSafeAsync(_RefreshAsync, RefreshFailedMessage);

    // ── حدّ الأمان الوحيد للمعالجات غير المتزامنة (RULE-N205) ──────────────────

    /// <summary>
    /// ينفّذ عملية غير متزامنة ويعرض رسالة عربية واحدة عند فشلها — بدل تكرار
    /// <c>try/catch</c> في كل معالج. لا يرمي أبداً.
    /// </summary>
    private static async Task _RunSafeAsync(Func<Task> operation, string failureMessage)
    {
        try
        {
            await operation();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "دليل الحسابات: {FailureMessage}", failureMessage);
            clsDialogs.Error(failureMessage);
        }
    }
}
