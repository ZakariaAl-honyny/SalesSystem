// -----------------------------------------------------------------------------
// ctrlAccountEditor.cs — محرّر بيانات الحساب كعنصر قابل للتضمين (F-003 · المرحلة أ)
//
// • الغرض: اللوحة اليمنى في «دليل الحسابات» — بيانات الحساب + معاينة كوده +
//   الحقول الهرمية (الأب · نوع الحساب رئيسي/فرعي · طبيعة الرصيد · نظامي · مركز التكلفة) —
//   بلا شبكة وبلا شجرة: تُدمج إلى جانب ctrlTreeOfAccounts وأسفل/بجانب
//   الشبكة في frmAccounts، وتُستهلك وحدها حيث يلزم.
//
// • **العنصر يقرأ فقط ولا يكتب**: استدعاءاته الوحيدة
//   IAccountDesktopService.GetAllAsync (قائمة الحسابات الأب) و
//   GetNextChildCodeAsync (معاينة كود الابن). لا استدعاء إنشاء/تعديل/حذف هنا أبداً —
//   التحرير بيت الشاشة الحاضنة (RULE-N100)، وهذا العنصر **يبني الطلب فقط**
//   عبر TryBuildCreate/TryBuildUpdate والحاضن يرسله.
//
// • الأحداث الداخلية: «حفظ» ⇒ SaveRequested · «إلغاء» ⇒ CancelRequested (لا تنفيذ داخلي).
//
// • الوضع (§2.36): SetMode(CrudMode) + آلة حالة واحدة `_ApplyModeState()` — لا يُضبط
//   Enabled/Visible في أي معالج آخر. العرض = كل الحقول معطّلة وأزرار الإجراءات مخفية.
//
// • التحقق: حكم «مطلوب/فارغ» في بيته الواحد RequiredFieldHelper (RULE-N172/N180…N183)
//   — يُوصَّل إلى **النموذج الحاضن** في OnLoad (لأن Attach/Validate/FirstMissing تقبل Form)،
//   فيرث زر Enter الحجب (RULE-N194) والتغذية عند المغادرة بلا نسخة ثانية من الحكم.
//   وما لا يخصّ «الفراغ» (النوع · طبيعة الرصيد · دورة الأب · حساب له فروع) حكم هذه
//   الشاشة المُعلن، ورسائله عربية فقط وبلا صندوق رسالة (تظهر في lblValidationHint).
//
// • الحدود الوحيدة لـtry/catch (RULE-N205): تحميل قائمة الآباء · معاينة الكود.
//   و`finally` تُعيد المؤشر دائماً (RULE-N26). ولا صندوق رسالة في هذا العنصر.
// -----------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Helpers;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Controls;

/// <summary>
/// لوحة تحرير الحساب (بيانات الحساب + الحقول الهرمية) كعنصر قابل للتضمين.
/// <para><b>الاستخدام:</b> <see cref="Attach"/> مرة واحدة بعد الإنشاء، ثم
/// <see cref="BindParents"/> عند تحديث قائمة الحسابات، و<see cref="SetMode"/> +
/// <see cref="LoadAccount"/>/<see cref="ResetEditor"/> عند تغيّر الوضع أو السجل — والحاضن
/// يستهلك <see cref="SaveRequested"/>/<see cref="CancelRequested"/> ويرسل الطلب بنفسه.</para>
/// <para><b>لا يكتب:</b> لا استدعاء إنشاء/تعديل/حذف من هذا العنصر (RULE-N100).</para>
/// </summary>
public partial class ctrlAccountEditor : UserControl
{
    // ── رسائل عربية (RULE-N28) ────────────────────────────────────────────────
    private const string NameRequiredMessage = "اسم الحساب مطلوب";
    private const string NatureRequiredMessage = "يجب اختيار نوع الحساب";
    private const string NormalBalanceRequiredMessage = "يجب اختيار الحساب الختامي (طبيعة الرصيد)";
    private const string ParentCycleMessage = "لا يمكن اختيار الحساب نفسه أو أحد فروعه كحساب أب";
    private const string SystemAccountMessage = "هذا حساب نظامي — للعرض فقط ولا يُعدَّل من هذه الشاشة";
    private const string GroupToPostingWarningMessage =
        "تحويل حساب تجميعي إلى حساب حركة يُرفض إذا كان له حسابات فرعية";
    private const string NotAttachedMessage = "لم يتم تهيئة محرر الحساب — تعذر الوصول إلى خدمة دليل الحسابات.";
    private const string ParentsLoadFailedMessage = "تعذر تحميل قائمة الحسابات الأب.";
    private const string HostFormMissingMessage = "تعذر التحقق من الحقول المطلوبة — المحرر غير مُضمَّن في شاشة بعد.";

    /// <summary>نص الخيار الجذري: لا أب ⇒ حساب رئيسي (كوده يولّده النظام عند الحفظ).</summary>
    private const string RootParentText = "(بلا أب — حساب رئيسي)";

    // ── الخدمة (RULE-N03 / RULE-N04) ───────────────────────────────────────────
    private IServiceProvider? _serviceProvider;

    private IAccountDesktopService Service => _serviceProvider!.GetRequiredService<IAccountDesktopService>();

    // ── الحالة ────────────────────────────────────────────────────────────────
    /// <summary>كل الحسابات المعروفة (آخر قراءة من الخدمة أو آخر <c>BindParents</c>) — لم يُرسَل منها شيء للخدمة.</summary>
    private readonly List<AccountDto> _allAccounts = [];

    /// <summary>فهرس المعرّف ⇒ الحساب: حكم «نظامي؟» و«دورة الأب» و«له فروع؟» من مصدر واحد.</summary>
    private readonly Dictionary<int, AccountDto> _accountsById = [];

    /// <summary>الحساب المعروض الآن (<c>null</c> = وضع إضافة) — يحدّد استثناء الأب والتصنيف المحفوظ.</summary>
    private AccountDto? _currentAccount;

    private CrudMode _mode = CrudMode.Browsing;

    /// <summary>تعبئة/إعادة ضبط جارية: تُكتم معاينة الكود وأحداث القوائم حتى لا تُطلق طلبات عابرة.</summary>
    private bool _binding;

    /// <summary>حارس الردود المتأخرة: تُقبل نتيجة **آخر** أب مطلوب فقط.</summary>
    private int? _pendingChildCodeParentId;

    /// <summary>
    /// أب طلبته الشاشة من <see cref="ResetEditor"/> قبل وصول قائمة الآباء — يُطبَّق بعد التحميل،
    /// فلا يضيع «إضافة فرعي» إذا سبق النقر قراءة القائمة.
    /// </summary>
    private int? _requestedParentId;

    /// <summary>هل وُصِّلت قاعدة الحقل المطلوب في النموذج الحاضن؟ (RULE-N183 — مرة واحدة).</summary>
    private bool _requiredFieldsAttached;

    /// <summary>هل بدأت قراءة قائمة الآباء؟ (قراءة واحدة لكل مزوّد خدمات).</summary>
    private bool _parentsLoadStarted;

    // ── خيارات القوائم (القيمة لا النص — RULE-N60) ─────────────────────────────

    /// <summary>نوع الحساب: 1 أصول · 2 خصوم · 3 حقوق ملكية · 4 إيرادات · 5 مصروفات.</summary>
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

    // ═════════════════════════════════════════════════════════════════
    //  الإنشاء
    // ═════════════════════════════════════════════════════════════════

    /// <summary>منشئ المصمم/الحاضن — بلا خدمات؛ الخدمة تأتي من <see cref="Attach"/>.</summary>
    public ctrlAccountEditor()
    {
        InitializeComponent();

        _LoadNatureOptions();
        _LoadNormalBalanceOptions();
        _LoadAccountTypeOptions();

        // RULE-N40…RULE-N46 — لوحة المفاتيح (WireGlobalShortcuts تخصّ النماذج ⇒ يملكها الحاضن).
        // RULE-N191/RULE-N193: نسخة الوسيطين تُعلن Enter مفتاحَ إدخال في PreviewKeyDown — وإلا
        // ابتلعه AcceptButton الحاضن (RULE-N147) قبل أن يصل KeyDown. والحقول تمشي بترتيب العرض
        // ثم تُسلِّم التركيز لزر «حفظ» بلا تنفيذ (RULE-N191).
        KeyboardHelper.WireEnterAsTab(gbAccountData, _PrimaryEditorButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                        // RULE-N45

        ApplyUiTheme();

        // RULE-N43 — بعد الثيم حتى يلتقط ألوان الأزرار النهائية.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnCancel);

        _ApplyModeState();   // فتح العنصر = وضع عرض معطّل (§2.36)
    }

    /// <summary>
    /// يُوصّل قاعدة «اسم الحساب مطلوب» بالنموذج الحاضن — بعد كل توصيلات المفاتيح
    /// (RULE-N183) ومرة واحدة فقط.
    /// <para>
    /// <b>لماذا النموذج الحاضن؟</b> لأن <see cref="RequiredFieldHelper"/> مفتاح سجله
    /// <see cref="Form"/> لا <see cref="UserControl"/>. فالعنصر يُعلن قاعدته على حاضنه
    /// بدل بناء حكم «فارغ» ثانٍ هنا (RULE-N172).
    /// </para>
    /// </summary>
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        _AttachRequiredFields();

        // قراءة الآباء تبدأ بعد إنشاء المقبض: يضمن سياق مزامنة الواجهة لاستكمال الـawait
        // (بدء القراءة من مُنشئ الحاضن قد يضع الاستكمال على خيط آخر ⇒ استثناء خيط متقاطع).
        _StartParentsLoadIfReady();
    }

    // ═════════════════════════════════════════════════════════════════
    //  الواجهة العامة (العقد المتفق عليه — لا يُغيَّر شكله)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يربط العنصر بالخدمات: قائمة الحسابات الأب ومعاينة الكود تقرآن من
    /// <see cref="IAccountDesktopService"/> وحده. آمن للاستدعاء المتكرر (لا إعادة تحميل
    /// إن كان المزوّد نفسه).
    /// </summary>
    /// <param name="serviceProvider">حاوية الخدمات القادمة من الشاشة الحاضنة.</param>
    public void Attach(IServiceProvider serviceProvider)
    {
        if (serviceProvider is null)
        {
            _SetValidationHint(NotAttachedMessage);
            return;
        }

        if (ReferenceEquals(_serviceProvider, serviceProvider))   // idempotent
        {
            _StartParentsLoadIfReady();   // لا ضرر: تبدأ القراءة فقط إن لم تكن بدأت والمقبض جاهز
            return;
        }

        _serviceProvider = serviceProvider;
        _allAccounts.Clear();
        _accountsById.Clear();
        _parentsLoadStarted = false;   // مزوّد جديد ⇒ قراءة جديدة عند أول فرصة آمنة

        _AttachRequiredFields();       // إن كان العنصر مُضمَّناً في نموذج أصلاً
        _StartParentsLoadIfReady();    // قراءة واحدة: قائمة الآباء
    }

    /// <summary>
    /// يبدأ قراءة قائمة الآباء **مرة واحدة** وحين يكون العنصر جاهزاً فعلاً (مقبض مُنشأ وخدمة
    /// موصولة) — فلا تبدأ قراءة من مُنشئ الحاضن قبل سياق مزامنة الواجهة.
    /// </summary>
    private void _StartParentsLoadIfReady()
    {
        if (_serviceProvider is null || _parentsLoadStarted || !IsHandleCreated) return;

        _parentsLoadStarted = true;
        _ = LoadParentsAsync();
    }

    /// <summary>يضبط وضع العنصر ويُعيد حساب تفعيل كل الحقول والأزرار الداخلية (§2.36).</summary>
    /// <param name="mode">وضع العرض · الإضافة · التعديل.</param>
    public void SetMode(CrudMode mode)
    {
        if (_mode == mode) return;

        _mode = mode;
        _ApplyModeState();

        // الدخول إلى «إضافة» يحتاج كوداً معروضاً فوراً (يُولّده النظام عند الحفظ).
        if (mode == CrudMode.Adding) _ = RefreshCodePreviewAsync();
    }

    /// <summary>
    /// يعبّئ «الحساب الأب» بالحسابات المؤهَّلة: حسابات المجموعة (غير الفرعية) والنشطة
    /// (ويبقى أب الحساب المعروض دائماً)، مع استثناء الحساب المطلوب تعديله وكل فروعه (منع الدوران).
    /// </summary>
    /// <param name="accounts">قائمة الحسابات (يُستحسن نتيجة <c>GetAllAsync(includeInactive: true)</c>).</param>
    /// <param name="excludeId">الحساب المستثنى وفروعه — <c>null</c> في وضع الإضافة.</param>
    public void BindParents(IReadOnlyList<AccountDto> accounts, int? excludeId)
    {
        var previousParentId = _SelectedParentId();

        _accountsById.Clear();
        foreach (var account in accounts) _accountsById[account.Id] = account;

        var excluded = _BuildExclusionSet(accounts, excludeId);

        // أب الحساب المعروض يبقى في القائمة ولو كان معطّلاً أو ورقياً: لو اختفى لظهر المحرر
        // على «بلا أب» وحُفظ التعديل بالحساب في الجذر — فقدان صلة صامت.
        int? keepParentId = _currentAccount?.ParentId;

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

        _SelectParent(previousParentId);
    }

    /// <summary>يعبّئ كل الحقول من الحساب المحدد؛ <c>null</c> ⇒ إعادة ضبط المحرر.</summary>
    /// <param name="account">الحساب المعروض (يُتوقع وصوله من الشبكة/الشجرة).</param>
    public void LoadAccount(AccountDto? account)
    {
        if (account is null)
        {
            ResetEditor();
            return;
        }

        _currentAccount = account;
        _pendingChildCodeParentId = null;   // يُبطل أي معاينة كود لا تزال في طريقها
        _requestedParentId = null;          // الأب يأتي من السجل المعروض لا من طلب سابق

        // إعادة بناء الآباء باستثناء الحساب وفروعه قبل تحديد أبيه الفعلي.
        BindParents(_accountsById.Values.ToList(), account.Id);

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
    /// يُفرّغ المحرر ويُعيده إلى الحالة الافتراضية، ويحدّد الأب المطلوب إن مُرِّر،
    /// ثم يطلب معاينة كود الحساب الجديد في وضع الإضافة.
    /// </summary>
    /// <param name="preselectedParentId">أب يُختار مسبقاً (إضافة فرعي من الشجرة) — <c>null</c> = الجذر.</param>
    public void ResetEditor(int? preselectedParentId = null)
    {
        _currentAccount = null;
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
        BindParents(_accountsById.Values.ToList(), null);

        _requestedParentId = preselectedParentId;

        if (preselectedParentId is int parentId) _SelectParent(parentId);

        _ClearValidation();
        _ApplyModeState();

        if (_mode == CrudMode.Adding) _ = RefreshCodePreviewAsync();

        if (_mode != CrudMode.Browsing && txtNameAr.CanFocus) txtNameAr.Focus();
    }

    /// <summary>
    /// يبني طلب **إنشاء** حساب من محتوى المحرر — والتحقق كله هنا (بيت واحد).
    /// </summary>
    /// <param name="request">الطلب الجاهز للإرسال إلى عقد الخدمة (إنشاء حساب).</param>
    /// <returns><c>false</c> مع رسالة عربية في <c>lblValidationHint</c> ونقل التركيز لأول ناقص.</returns>
    public bool TryBuildCreate(out CreateAccountRequest request)
    {
        request = null!;

        if (!_ValidateEditor()) return false;

        // حصر نوع للمُصرِّف فقط (لا حكم ثانٍ): الحكم والرسالة العربية أعلاه في _ValidateEditor.
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
    public bool TryBuildUpdate(out UpdateAccountRequest request)
    {
        request = null!;

        // الحساب النظامي يرفضه BLL — فلا يُبنى طلب لعملية مرفوضة (RULE-N167).
        if (_currentAccount is { IsSystem: true })
        {
            _SetValidationHint(SystemAccountMessage);
            return false;
        }

        if (!_ValidateEditor()) return false;

        // حصر نوع للمُصرِّف فقط (لا حكم ثانٍ): الحكم والرسالة العربية أعلاه في _ValidateEditor.
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
            CategoryId: _currentAccount?.CategoryId,
            Description: txtDescription.Text.Trim().ToNullIfEmpty(),
            Notes: txtNotes.Text.Trim().ToNullIfEmpty(),
            NormalBalance: _SelectedNormalBalance(),
            AllowCostCenter: chkAllowCostCenter.Checked);

        return true;
    }

    /// <summary>«حفظ» الداخلي — العنصر يرفع فقط والحاضن يرسل الطلب.</summary>
    public event EventHandler? SaveRequested;

    /// <summary>«إلغاء» الداخلي — الحاضن يعيد الوضع إلى العرض.</summary>
    public event EventHandler? CancelRequested;

    // ═════════════════════════════════════════════════════════════════
    //  الخدمة — قراءات فقط (لا Create/Update/Delete هنا — RULE-N100)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يقرأ الحسابات (فيها غير النشط، ليبقى أب الحساب المعروض متاحاً) ويُعبّئ قائمة الآباء.
    /// حدّ واحد لـtry/catch (RULE-N205) — والخطأ يُكتب في سطر التنبيه، لا في صندوق.
    /// </summary>
    private async Task LoadParentsAsync()
    {
        if (IsDisposed || Disposing) return;

        if (_serviceProvider is null)
        {
            _SetValidationHint(NotAttachedMessage);
            return;
        }

        var previousCursor = Cursor;
        Cursor = Cursors.WaitCursor;   // RULE-N26

        try
        {
            var result = await Service.GetAllAsync(includeInactive: true);

            if (IsDisposed || Disposing) return;

            if (!result.IsSuccess || result.Value is null)
            {
                Log.Warning("محرر الحساب: تعذر تحميل قائمة الآباء — {Error}", result.Error);

                _allAccounts.Clear();
                BindParents([], _currentAccount?.Id);
                _SetValidationHint(result.Error ?? ParentsLoadFailedMessage);
                return;
            }

            _allAccounts.Clear();
            foreach (var account in result.Value) _allAccounts.Add(account);

            BindParents(_allAccounts, _currentAccount?.Id);

            // الأب بعد إعادة التحميل: أب السجل المعروض هو المرجع (وإلا ضاع تحديده لأن القائمة
            // كانت فارغة قبل القراءة)، وإن لم يكن هناك سجل ⇒ الأب الذي طلبته الشاشة («إضافة فرعي»).
            if (_currentAccount is { } currentAccount) _SelectParent(currentAccount.ParentId);
            else if (_requestedParentId is int requested) _SelectParent(requested);

            // وضع الإضافة يحتاج كوداً معروضاً الآن (كان الحقل فارغاً قبل وصول القائمة).
            if (_mode == CrudMode.Adding) _ = RefreshCodePreviewAsync();
        }
        catch (Exception ex)
        {
            // الحدّ الوحيد لهذا المسار (RULE-N205): رسالة عربية بلا نص الاستثناء (RULE-N28/N29)
            Log.Error(ex, "محرر الحساب: خطأ أثناء تحميل قائمة الحسابات الأب");

            if (IsDisposed || Disposing) return;

            _allAccounts.Clear();
            BindParents([], _currentAccount?.Id);
            _SetValidationHint(ParentsLoadFailedMessage);
        }
        finally
        {
            if (!IsDisposed && !Disposing) Cursor = previousCursor;   // RULE-N26 — الاستعادة دائماً
        }
    }

    /// <summary>
    /// معاينة «كود الابن التالي» للأب المختار في وضع الإضافة (RULE-N25/N26).
    /// للعرض فقط: الكود الحقيقي يولّده النظام عند الحفظ، ولا يُرسَل مع أي طلب.
    /// </summary>
    private async Task RefreshCodePreviewAsync()
    {
        if (IsDisposed || Disposing) return;
        if (!IsHandleCreated) return;                        // لا طلب قبل أن يصير للعنصر واجهة حيّة
        if (_binding || _mode != CrudMode.Adding) return;    // المعاينة تخصّ الإضافة وحدها

        int? parentId = _SelectedParentId();

        // «بلا أب — حساب رئيسي»: لا كود أبوي يُبنى عليه ⇒ يُفرَّغ الحقل ويُولّد النظام الكود عند الحفظ.
        if (parentId is null)
        {
            _pendingChildCodeParentId = null;
            txtAccountCode.Clear();
            return;
        }

        if (_serviceProvider is null) return;

        _pendingChildCodeParentId = parentId;   // حارس الردود المتأخرة: آخر أب مطلوب فقط

        var previousCursor = Cursor;
        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await Service.GetNextChildCodeAsync(parentId.Value);

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
            Log.Error(ex, "محرر الحساب: تعذر معاينة كود الحساب للأب {ParentId}", parentId);

            if (!IsDisposed && !Disposing && _pendingChildCodeParentId == parentId)
                txtAccountCode.Clear();
        }
        finally
        {
            if (!IsDisposed && !Disposing) Cursor = previousCursor;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  آلة الحالة الوحيدة (§2.36 / نظير RULE-N150)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// الحالة الوحيدة للحقول والأزرار الداخلية — تُستدعى بعد كل انتقال (تهيئة · وضع ·
    /// تحميل سجل · إعادة ضبط). لا يُضبط <c>Enabled</c>/<c>Visible</c> خارجها.
    /// </summary>
    private void _ApplyModeState()
    {
        bool browsing = _mode == CrudMode.Browsing;
        bool adding = _mode == CrudMode.Adding;

        // الحساب النظامي للقراءة فقط دائماً: الخدمة ترفض تعديله، فلا تُفتح حقول لا يمكن حفظها.
        bool systemAccount = _currentAccount?.IsSystem == true;
        bool editable = !browsing && !systemAccount;

        txtAccountCode.Enabled = !browsing;   // للقراءة دائماً (ReadOnly) — يُظهر كود المعاينة أو الكود المحفوظ
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

        // أزرار الإجراءات الداخلية: مخفية في وضع العرض (§2.36) وتتبع قابلية التحرير.
        btnSave.Visible = !browsing;
        btnSave.Enabled = editable;
        btnSave.Text = adding ? "حفظ الإضافة" : "حفظ التعديل";

        btnCancel.Visible = !browsing;
        btnCancel.Enabled = !browsing;

        if (!browsing) _SetValidationHint(systemAccount ? SystemAccountMessage : null);
    }

    /// <summary>الزر الأساسي لـEnter داخل المحرر: «حفظ» إن كان ظاهراً ومفعّلاً (RULE-N191) — تركيزاً فقط.</summary>
    private Button? _PrimaryEditorButton()
        => btnSave.Visible && btnSave.Enabled ? btnSave : null;

    // ═════════════════════════════════════════════════════════════════
    //  التحقق — حكم «مطلوب» في بيته الواحد، وفحوص المحرر هنا
    // ═════════════════════════════════════════════════════════════════

    /// <summary>يوصّل قاعدة «اسم الحساب مطلوب» بالنموذج الحاضن مرة واحدة (RULE-N183).</summary>
    private void _AttachRequiredFields()
    {
        if (_requiredFieldsAttached) return;

        var host = FindForm();
        if (host is null || host.IsDisposed) return;

        RequiredFieldHelper.Attach(host, errorProvider, new RequiredField(txtNameAr, NameRequiredMessage));
        _requiredFieldsAttached = true;
    }

    /// <summary>
    /// يقيّم المحرر: الحقل المطلوب من البيت الواحد <see cref="RequiredFieldHelper"/>،
    /// وفحوص هذه الشاشة (النوع · طبيعة الرصيد · دورة الأب · حساب له فروع) هنا.
    /// </summary>
    private bool _ValidateEditor()
    {
        _SetValidationHint(null);

        var host = FindForm();

        if (host is null || host.IsDisposed)
        {
            // لا حكم «فارغ» ثانٍ هنا (RULE-N172): بلا نموذج حاضن لا تُوصَّل القاعدة ⇒ رفض مغلق
            // برسالة عربية بدل السماح بحساب بلا اسم.
            _SetValidationHint(HostFormMissingMessage);
            return false;
        }

        _AttachRequiredFields();

        if (!RequiredFieldHelper.Validate(host))
        {
            var missing = RequiredFieldHelper.FirstMissing(host);

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
        if (_currentAccount is { } current
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

        if (_requiredFieldsAttached && FindForm() is Form host && !host.IsDisposed)
            RequiredFieldHelper.Clear(host);

        lblValidationHint.Text = string.Empty;
    }

    /// <summary>يكتب رسالة عربية في سطر «صحّة البيانات» (فارغ ⇒ يُفرَّغ السطر).</summary>
    private void _SetValidationHint(string? message)
    {
        lblValidationHint.Text = message ?? string.Empty;
    }

    // ═════════════════════════════════════════════════════════════════
    //  القوائم والتحديد (القيمة لا النص — RULE-N60)
    // ═════════════════════════════════════════════════════════════════

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

    // ═════════════════════════════════════════════════════════════════
    //  المعالجات — رفع الأحداث والتوصيل
    // ═════════════════════════════════════════════════════════════════

    /// <summary>تغيّر الأب ⇒ معاينة كود الابن (وضع الإضافة فقط).</summary>
    private void cbParent_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_binding) return;

        _requestedParentId = null;   // اختيار المستخدم يلغي أي أب مطلوب سابقاً من الشاشة

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
        bool wasGroup = _currentAccount is { IsLeaf: false };

        if (goesToPosting && wasGroup)
        {
            _SetValidationHint(GroupToPostingWarningMessage);
            return;
        }

        // التراجع إلى «رئيسي» يرفع تنبيهنا فقط — ولا يمسح رسالة تحقق أخرى.
        if (lblValidationHint.Text == GroupToPostingWarningMessage) _SetValidationHint(null);
    }

    private void btnSave_Click(object? sender, EventArgs e) => SaveRequested?.Invoke(this, EventArgs.Empty);

    private void btnCancel_Click(object? sender, EventArgs e) => CancelRequested?.Invoke(this, EventArgs.Empty);

    // ═════════════════════════════════════════════════════════════════
    //  المظهر والتنظيف
    // ═════════════════════════════════════════════════════════════════

    /// <summary>ثيم العنصر — ينتهي بـ<see cref="GreenThemeHelper.Apply(Control)"/> كآخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        // لونا الحالة من البيت الواحد (RULE-N169) — لا قيمة لون جديدة هنا.
        lblValidationHint.ForeColor = GreenThemeHelper.StatusDanger;
        gbAccountData.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this);   // آخر سطر دائماً (RULE-N50) — يُوصِّل مظهر التعطيل أيضاً (RULE-N154)
    }

    /// <summary>التنظيف (RULE-N30).</summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }

        base.Dispose(disposing);
    }
}
