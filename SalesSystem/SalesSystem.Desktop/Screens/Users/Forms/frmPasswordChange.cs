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
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45 — أسهم RTL
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnClose);                 // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F2, () => btnSave.PerformClick()),
            (Keys.Escape, () => btnClose.PerformClick()));
        KeyboardHelper.WireModeEnter(this, () => btnSave.Enabled ? btnSave : null);  // RULE-N147

        AcceptButton = btnSave;
        CancelButton = btnClose;
    }

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
    /// تحقق كامل بأسلوب <c>frmProductMaster</c>: يضبط/يمسح <c>errorProvider</c> لكل حقل بالترتيب،
    /// ويعيد النتيجة بلا أي MessageBox. قواعد التحقق نفسها لم تتغيّر — الإلزام يُقرأ من
    /// <see cref="clsValidation.IsRequired"/> وتلميح الرفض من <see cref="clsInputFeedback"/>.
    /// </summary>
    private bool _ValidateForm()
    {
        bool isValid = true;

        // كلمة المرور الحالية مطلوبة في وضع الخدمة الذاتية فقط (في الإعادة الإدارية الصف مخفي).
        if (_isSelfService && !clsValidation.IsRequired(txtCurrentPassword.Text))
        {
            _Reject(txtCurrentPassword, CurrentRequiredMessage);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtCurrentPassword, string.Empty);
        }

        if (!clsValidation.IsRequired(txtNewPassword.Text))
        {
            _Reject(txtNewPassword, NewRequiredMessage);
            isValid = false;
        }
        else if (txtNewPassword.Text.Length < clsValidation.PasswordMinLength)
        {
            _Reject(txtNewPassword, MinLengthMessage);
            isValid = false;
        }
        else if (_isSelfService && txtNewPassword.Text == txtCurrentPassword.Text)
        {
            _Reject(txtNewPassword, SamePasswordMessage);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtNewPassword, string.Empty);
        }

        if (!clsValidation.IsRequired(txtConfirmPassword.Text))
        {
            _Reject(txtConfirmPassword, ConfirmRequiredMessage);
            isValid = false;
        }
        else if (txtNewPassword.Text != txtConfirmPassword.Text)
        {
            _Reject(txtConfirmPassword, MismatchMessage);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtConfirmPassword, string.Empty);
        }

        return isValid;
    }

    /// <summary>
    /// ينقل المؤشر إلى **أول** حقل غير صالح بالترتيب البصري (الحالية ← الجديدة ← التأكيد) ويضع
    /// أيقونة الخطأ عليه — ويتخطّى حقل كلمة المرور الحالية في وضع الإعادة الإدارية لأنه مخفي.
    /// </summary>
    private void _FocusFirstInvalidField()
    {
        if (_isSelfService && !clsValidation.IsRequired(txtCurrentPassword.Text))
        {
            _Reject(txtCurrentPassword, CurrentRequiredMessage);
            txtCurrentPassword.Focus();
            return;
        }

        if (!clsValidation.IsRequired(txtNewPassword.Text))
        {
            _Reject(txtNewPassword, NewRequiredMessage);
            txtNewPassword.Focus();
            return;
        }

        if (txtNewPassword.Text.Length < clsValidation.PasswordMinLength)
        {
            _Reject(txtNewPassword, MinLengthMessage);
            txtNewPassword.Focus();
            return;
        }

        if (_isSelfService && txtNewPassword.Text == txtCurrentPassword.Text)
        {
            _Reject(txtNewPassword, SamePasswordMessage);
            txtNewPassword.Focus();
            return;
        }

        if (!clsValidation.IsRequired(txtConfirmPassword.Text))
        {
            _Reject(txtConfirmPassword, ConfirmRequiredMessage);
            txtConfirmPassword.Focus();
            return;
        }

        if (txtNewPassword.Text != txtConfirmPassword.Text)
        {
            _Reject(txtConfirmPassword, MismatchMessage);
            txtConfirmPassword.Focus();
        }
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

        // حارس الهوية: لا يُرسَل userId = 0 أبداً في وضع الخدمة الذاتية.
        if (_isSelfService && _UserId <= 0)
        {
            ShowError(UnknownUserMessage);
            _Reject(txtCurrentPassword, UnknownUserMessage);
            txtCurrentPassword.Focus();
            return;
        }

        if (!_ValidateForm())
        {
            _FocusFirstInvalidField();
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

            // رفض الخدمة (رسالة عربية): في الشاشة لا في صندوق رسالة، وتُفرَّغ حقول الإدخال.
            // الترتيب مقصود: التفريغ أولاً (يُطلق TextChanged الذي يمسح الخطأ) ثم الرسالة والأيقونة.
            string error = string.IsNullOrWhiteSpace(result.Error) ? SaveFailedMessage : result.Error;
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            ShowError(error);
            _Reject(txtNewPassword, error);
            txtNewPassword.Focus();
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
    /// </summary>
    public void _SetUpdateMode(int userId)
    {
        _isSelfService = false;
        _UserId = userId;
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

    /// <summary>تفريغ الحقول ومسح كل آثار الخطأ (يُستهلك عند تغيير الوضع).</summary>
    private void _ResetInputs()
    {
        txtCurrentPassword.Clear();
        txtNewPassword.Clear();
        txtConfirmPassword.Clear();
        errorProvider.Clear();
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
