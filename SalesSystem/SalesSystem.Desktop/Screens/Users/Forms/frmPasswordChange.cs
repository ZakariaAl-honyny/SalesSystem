using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Screens.Users.Forms;

/// <summary>
/// شاشة تغيير/إعادة تعيين كلمة المرور — تُفتح نمطياً (RULE-N09).
/// <para>
/// الحالة الافتراضية **خدمة ذاتية**: صفّ «كلمة المرور الحالية» ظاهر وإلزامي، والمستخدم يُقرأ من
/// <see cref="ISessionService"/> — فلا تُغيَّر كلمة المرور بلا إثبات الحالية، ولا يُرسَل
/// <c>userId = 0</c> أبداً (يُرفض محلياً برسالة عربية).
/// </para>
/// <para>
/// الاستثناء الموثّق: إعادة التعيين الإدارية عبر <see cref="_SetUpdateMode"/> — المدير لا يعرف
/// كلمة مرور غيره، فيُخفى الصفّ وتُعاد الصفوف المتبقية بلا فجوة.
/// </para>
/// لا تحتوي أي منطق تشفير: تُمرّر الطلب إلى <see cref="IAuthDesktopService"/> وحدها (RULE-N06).
/// </summary>
public partial class frmPasswordChange : Form
{
    // ── رسائل التحقق العربية — مصدر واحد داخل الشاشة (يُستهلك في التحقق وفي إبراز أول حقل ناقص) ──
    private const string CurrentRequiredMessage = "كلمة المرور الحالية مطلوبة";
    private const string NewRequiredMessage = "كلمة المرور الجديدة مطلوبة";
    private const string ConfirmRequiredMessage = "تأكيد كلمة المرور مطلوب";
    private const string MismatchMessage = "كلمة المرور غير متطابقة";
    private const string SamePasswordMessage = "كلمة المرور الجديدة يجب أن تختلف عن الحالية";
    private const string UnknownUserMessage = "تعذر تحديد المستخدم الحالي. يرجى إعادة تسجيل الدخول.";
    private const string UnknownTargetUserMessage = "تعذر تحديد المستخدم المطلوب تغيير كلمة مروره. يرجى إعادة المحاولة.";
    private const string ValidationSummaryMessage = "يرجى تصحيح الحقول المعلَّمة.";
    private const string SaveFailedMessage = "تعذر تغيير كلمة المرور. يرجى إعادة المحاولة.";

    /// <summary>
    /// رسالة حدّ الطول — تُبنى من البيت الواحد <see cref="clsValidation.PasswordMinLength"/>
    /// (سياسة موحّدة مع خدمة المصادقة — RULE-N36/RULE-N172)، فلا يُكتب الرقم حرفياً في الشاشة.
    /// </summary>
    private static string MinLengthMessage =>
        $"كلمة المرور الجديدة يجب ألا تقل عن {clsValidation.PasswordMinLength} خانات";

    private readonly IServiceProvider _serviceProvider;

    // ── مواضع المصمّم الأصلية للصفوف — تُلتقط مرة واحدة (لا أرقام تخطيط مكرّرة في المنطق) ──
    private Point _newPasswordOrigin;
    private Point _newPasswordLabelOrigin;
    private Point _confirmPasswordOrigin;
    private Point _confirmPasswordLabelOrigin;
    private int _groupHeight;
    private int _errorTop;

    private int _UserId;
    private bool _isSelfService = true;   // الافتراضي: خدمة ذاتية — كلمة المرور الحالية مطلوبة
    private bool _isBusy;                 // حارس إعادة الدخول أثناء الحفظ

    public frmPasswordChange(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        _CaptureDesignerLayout();

        // الحالة الافتراضية = خدمة ذاتية، والمستخدم من الجلسة — فلا يبقى _UserId = 0 ولا يُغيَّر
        // شيء بلا إدخال كلمة المرور الحالية (العيب: كان الفتح من frmMain يترك الشاشة بلا وضع).
        _SetSelfServiceMode(SessionService.Current?.UserId ?? 0);

        // RULE-N40…RULE-N47 — كل الستة من KeyboardHelper (RULE-N95)
        // RULE-N191: Enter داخل صندوق كلمة المرور يمشي بترتيب العرض (الحالية ← الجديدة ← التأكيد)
        // ثم يُركِّز زر «حفظ» **بلا تنفيذ** — وEnter لا يُرسِل تغيير كلمة المرور من داخل الصندوق أبداً.
        // النطاق = gbPassword (حقول المصمّم وحدها)، فيبقى سلوك أي حقل آخر على SelectNextControl.
        KeyboardHelper.WireEnterAsTab(gbPassword, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45 — أسهم RTL
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnClose);                 // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F2, () => btnSave.PerformClick()),
            (Keys.Escape, () => btnClose.PerformClick()));
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);                   // RULE-N147 — يتخطّى حقول الإدخال بنفسه

        // RULE-N180…N183 — تغذية الحقول المطلوبة (الإلزام وحده؛ قواعد المدى تبقى في _ValidateRangeRules).
        // إلزامي أن يأتي **بعد** WireFocusHighlight (ترتيب اشتراك Leave) — والباكس الثلاثة TextBoxBase
        // بـ PasswordChar فتُعامل كأي حقل نصي عند الحكم على «فارغ»؛ الرسائل نفسها بلا تغيير.
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(txtCurrentPassword, CurrentRequiredMessage, () => _isSelfService),
            new RequiredField(txtNewPassword, NewRequiredMessage),
            new RequiredField(txtConfirmPassword, ConfirmRequiredMessage));

        AcceptButton = _PrimaryActionButton();   // مصدر واحد مع WireModeEnter/WireEnterAsTab (RULE-N147)
        CancelButton = btnClose;
    }

    /// <summary>
    /// الزر الأساسي للشاشة (RULE-N147/RULE-N191): «حفظ» وحده. يُقرأ عند كل ضغطة Enter وعند إسناد
    /// <c>AcceptButton</c>، ولا يُرجع <c>null</c> أثناء الحفظ لأن الـHelper يفحص <c>Enabled</c> بنفسه:
    /// <c>KeyboardHelper.MoveNextByDisplayOrder</c> و<c>WireModeEnter</c> كلاهما يشترط
    /// <c>primary.Enabled &amp;&amp; primary.Visible</c> — فلا نسخة ثانية من حارس «معطّل» في الشاشة (RULE-N172).
    /// </summary>
    private Button? _PrimaryActionButton() => btnSave;

    // ── الخدمات (RULE-N03/N04) ────────────────────────────────────────────────

    private IAuthDesktopService AuthService => _serviceProvider.GetRequiredService<IAuthDesktopService>();
    private ISessionService SessionService => _serviceProvider.GetRequiredService<ISessionService>();

    // ── التحقق ────────────────────────────────────────────────────────────────

    /// <summary>
    /// رفض حقل واحد: أيقونة/رسالة <c>errorProvider</c> **مقرونةً** بتغذية بصرية موحّدة من البيت الواحد
    /// <see cref="clsInputFeedback.ShowRejectTooltip"/> بنفس نص الرفض — فلا رفض صامت (GAP-08b / RULE-N169).
    /// </summary>
    private void _Reject(Control field, string message)
    {
        errorProvider.SetError(field, message);
        clsInputFeedback.ShowRejectTooltip(field, message);
    }

    /// <summary>
    /// قواعد **المدى** فقط — تبقى **حرفياً كما هي** (نفس الشروط ونفس الرسائل العربية ونفس
    /// الترتيب): حدّ الطول · اختلاف الجديدة عن الحالية (خدمة ذاتية) · تطابق الجديدة مع التأكيد.
    /// <para>
    /// <b>حكم «فارغ» ليس هنا:</b> الإلزام ملك <see cref="RequiredFieldHelper"/> وحده (PILOT-2 /
    /// RULE-N172) ويُقيَّم **قبل** هذه الدالة، فلا يُبلَّغ «قصيرة جداً» عن حقل فارغ.
    /// </para>
    /// <para>
    /// تُوسم كل مخالفة (لا تُقصّ) ثم يُركَّز أول حقل مخالف — نفس سلوك التحقق السابق بلا تغيير،
    /// والرفض يمرّ عبر <see cref="_Reject"/> (أيقونة + ومضة من البيت الواحد).
    /// </para>
    /// </summary>
    private bool _ValidateRangeRules()
    {
        Control? firstInvalid = null;
        bool isValid = true;

        if (txtNewPassword.Text.Length < clsValidation.PasswordMinLength)
        {
            _Reject(txtNewPassword, MinLengthMessage);
            firstInvalid ??= txtNewPassword;
            isValid = false;
        }
        else if (_isSelfService && txtNewPassword.Text == txtCurrentPassword.Text)
        {
            _Reject(txtNewPassword, SamePasswordMessage);
            firstInvalid ??= txtNewPassword;
            isValid = false;
        }

        if (txtNewPassword.Text != txtConfirmPassword.Text)
        {
            _Reject(txtConfirmPassword, MismatchMessage);
            firstInvalid ??= txtConfirmPassword;
            isValid = false;
        }

        firstInvalid?.Focus();
        return isValid;
    }

    // ── رسائل الخطأ داخل الشاشة (بلا MessageBox للتحقق — نمط frmLogin/frmProductMaster) ──

    private void ShowError(string message)
    {
        lblError.Text = message;
        lblError.Visible = true;
    }

    private void HideError()
    {
        lblError.Text = string.Empty;
        lblError.Visible = false;
    }

    /// <summary>
    /// «المسح عند الكتابة»: أول تعديل على أي حقل يمسح خطأه ويُخفي رسالة الشاشة — بلا إلحاح.
    /// </summary>
    private void _OnPasswordTextChanged(object? sender, EventArgs e)
    {
        if (sender is Control field)
            errorProvider.SetError(field, string.Empty);

        HideError();
    }

    // ── الحفظ ─────────────────────────────────────────────────────────────────

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_isBusy) return;

        // حارس الهوية: لا يُرسَل userId = 0 أبداً — في **الوضعين** (كان الحارس على الخدمة الذاتية
        // وحدها، فكانت الإعادة الإدارية تُرسل userId = 0 فتنتهي الشاشة إلى طريق مسدود بلا رسالة).
        if (_UserId <= 0)
        {
            var identityMessage = _isSelfService ? UnknownUserMessage : UnknownTargetUserMessage;
            var identityField = _isSelfService ? txtCurrentPassword : txtNewPassword;

            ShowError(identityMessage);
            _Reject(identityField, identityMessage);
            identityField.Focus();
            return;
        }

        // RULE-N180…N183 / RULE-N194: الحقول المطلوبة الفارغة تُوسَم وتُومض وتُرَكَّز من البيت الواحد
        // (أيقونة ✗ + ومضة + صوت، ثم تزول فور صحة القيمة) — لا حكم «فارغ» في الشاشة، وحفظ واحد بلا رنّات.
        if (!RequiredFieldHelper.Validate(this))
        {
            RequiredFieldHelper.FirstMissing(this)?.Focus();
            ShowError(ValidationSummaryMessage);
            return;
        }

        // قواعد المدى تبقى كما هي حرفياً (الطول · الاختلاف عن الحالية · التطابق) — الرسائل نفسها.
        if (!_ValidateRangeRules())
        {
            ShowError(ValidationSummaryMessage);
            return;
        }

        _isBusy = true;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            // الخدمة هي المسؤولة عن التشفير والتحقق النهائي — الشاشة لا تعرف BCrypt إطلاقاً.
            var request = new ChangePasswordRequest(
                txtCurrentPassword.Text,
                txtNewPassword.Text,
                txtConfirmPassword.Text);

            var result = await AuthService.ChangePasswordAsync(request, _UserId);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تغيير كلمة المرور بنجاح.");
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            // رفض الخدمة (رسالة عربية): في الشاشة لا في صندوق رسالة.
            // وفي وضع الخدمة الذاتية لا ترفض الخدمة إلا بسبب الهوية أو كلمة المرور الحالية — فكل قواعد
            // كلمة المرور الجديدة (الإلزام · الطول · التطابق · الاختلاف عن الحالية) تُتحقَّق محلياً قبل
            // الإرسال. لذلك تُوجَّه الأيقونة إلى كلمة المرور الحالية، وتبقى كلمتا المرور الجديدتان كما
            // هما: كانت الشاشة تُفرّغهما وتضع الأيقونة على «الجديدة» فيُعيد المستخدم كتابة حقلين صحيحين
            // بسبب خطأ في الحقل الأول — رفض مضلِّل وفقدان لما كتبه.
            string error = string.IsNullOrWhiteSpace(result.Error) ? SaveFailedMessage : result.Error;

            var rejectField = _isSelfService && txtCurrentPassword.Visible
                ? txtCurrentPassword
                : txtNewPassword;

            ShowError(error);
            _Reject(rejectField, error);
            rejectField.Focus();
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "فشل تغيير كلمة المرور للمستخدم {UserId}", _UserId);
            ShowError(SaveFailedMessage);
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
            _isBusy = false;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── أوضاع الفتح ───────────────────────────────────────────────────────────

    /// <summary>
    /// إعادة تعيين إدارية من دليل المستخدمين: تُخفى كلمة المرور الحالية (المدير لا يعرفها —
    /// استثناء موثّق) وتُعاد الصفوف المتبقية إلى أعلى بلا فجوة تخطيط.
    /// <para>
    /// الهدف هو المستخدم الحالي نفسه ⇒ الخدمة تعامل الطلب كخدمة ذاتية وتُلزم كلمة المرور الحالية،
    /// فإخفاء صفّها يجعل الشاشة طريقاً مسدوداً — يُفتح الوضع الذاتي بدلاً منه.
    /// والهدف غير الصالح (<paramref name="userId"/> ≤ 0) يبقى في الوضع الإداري بهوية 0 فيرفضه حارس
    /// الحفظ برسالة عربية، بدل إرسال <c>userId = 0</c> إلى الخدمة أو تحويل الطلب إلى تغيير كلمة مرور
    /// المستخدم الحالي (كتابة على الحساب الخطأ).
    /// </para>
    /// </summary>
    public void _SetUpdateMode(int userId)
    {
        if (userId > 0 && userId == (SessionService.Current?.UserId ?? 0))
        {
            _SetSelfServiceMode(userId);
            return;
        }

        _isSelfService = false;
        _UserId = Math.Max(userId, 0);
        lblMode.Text = "إعادة تعيين كلمة المرور";
        Text = "إعادة تعيين كلمة المرور";

        _SetCurrentPasswordRowVisible(false);
        _ResetInputs();
    }

    /// <summary>
    /// وضع الخدمة الذاتية (فتح من شاشة الدخول عند <c>MustChangePassword</c>، أو الحالة الافتراضية
    /// عند الفتح من <c>frmMain</c>): صفّ كلمة المرور الحالية ظاهر وإلزامي.
    /// </summary>
    public void _SetSelfServiceMode(int userId)
    {
        _isSelfService = true;
        _UserId = userId;
        lblMode.Text = "تغيير كلمة المرور";
        Text = "تغيير كلمة المرور";

        _SetCurrentPasswordRowVisible(true);
        _ResetInputs();
    }

    // ── تخطيط صف كلمة المرور الحالية ──────────────────────────────────────────

    /// <summary>
    /// يلتقط مواضع المصمّم مرة واحدة، فتُستعاد الصفوف بدقة بلا تكرار أرقام التخطيط في المنطق
    /// (وإن تغيّر المصمّم تبعها الكود — RULE-N134).
    /// </summary>
    private void _CaptureDesignerLayout()
    {
        _newPasswordOrigin = txtNewPassword.Location;
        _newPasswordLabelOrigin = lblNewPassword.Location;
        _confirmPasswordOrigin = txtConfirmPassword.Location;
        _confirmPasswordLabelOrigin = lblConfirmPassword.Location;
        _groupHeight = gbPassword.Height;
        _errorTop = lblError.Top;
    }

    /// <summary>
    /// إظهار/إخفاء الصف الأول (كلمة المرور الحالية + التلميح) ونقل الصفّين المتبقيين لأعلى عند
    /// الإخفاء، مع تقصير صندوق المجموعة وتقريب رسالة الخطأ — فلا تبقى فجوة مكان الصف المخفي.
    /// </summary>
    private void _SetCurrentPasswordRowVisible(bool visible)
    {
        lblHint.Visible = visible;
        lblCurrentPassword.Visible = visible;
        txtCurrentPassword.Visible = visible;

        // فرق صفّين متتاليين مقروء من المصمّم (58 بكسل) — لا رقم مكرّر هنا.
        int rowGap = _confirmPasswordOrigin.Y - _newPasswordOrigin.Y;
        int shift = visible ? 0 : -rowGap;

        txtNewPassword.Top = _newPasswordOrigin.Y + shift;
        lblNewPassword.Top = _newPasswordLabelOrigin.Y + shift;
        txtConfirmPassword.Top = _confirmPasswordOrigin.Y + shift;
        lblConfirmPassword.Top = _confirmPasswordLabelOrigin.Y + shift;
        gbPassword.Height = _groupHeight + shift;
        lblError.Top = _errorTop + shift;
    }

    /// <summary>تفريغ الحقول ومسح كل آثار الخطأ (يُستهلك عند تغيير الوضع) — RULE-N183 مع errorProvider.Clear.</summary>
    private void _ResetInputs()
    {
        txtCurrentPassword.Clear();
        txtNewPassword.Clear();
        txtConfirmPassword.Clear();
        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);   // لا علامة مطلوبة عالقة بعد إعادة ضبط الوضع
        HideError();
    }

    /// <summary>أول حقل ظاهر يُركَّز عليه (الحالية في الخدمة الذاتية، والجديدة في الإعادة الإدارية).</summary>
    private void _FocusFirstField()
    {
        if (_isSelfService && txtCurrentPassword.Visible)
            txtCurrentPassword.Focus();
        else
            txtNewPassword.Focus();
    }

    // ── التحميل والثيم ─────────────────────────────────────────────────────────

    private void frmPasswordChange_Load(object sender, EventArgs e)
    {
        try
        {
            ApplyUiTheme();     // RULE-N50 — آخر خطوة في مسار التحميل
            _FocusFirstField(); // المؤشر يبدأ على كلمة المرور الحالية
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "تعذر تجهيز شاشة تغيير كلمة المرور");
            clsDialogs.Error("تعذر تجهيز الشاشة. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستهلك من الـHelper فقط (RULE-N112).</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }
}
