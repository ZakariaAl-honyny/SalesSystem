using Serilog;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.Models;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens;

/// <summary>
/// شاشة تسجيل الدخول: يختار المستخدم **حسابه** من قائمة <c>cmbUsers</c> ثم يكتب كلمة مرور
/// ذلك الحساب في <c>txtPassword</c> — ولا يوجد أي دخول تلقائي في أي وضع.
/// <list type="bullet">
/// <item>قائمة المستخدمين تأتي من <see cref="IUserDesktopService.GetAllAsync"/> (بيانات تجريبية في وضع التجربة)،
/// والمقفل يظهر بلاحقة «— مقفل» ويبقى قابلاً للاختيار ليُرفض بسبب واضح.</item>
/// <item>المصادقة دائماً عبر <see cref="IAuthDesktopService.LoginAsync"/> — لا مقارنة محلية لكلمة المرور
/// ولا اختلاق جلسة في الواجهة (تكافؤ المحاكاة RULE-N167).</item>
/// </list>
/// </summary>
public partial class frmLogin : Form
{
    // ── رسائل التحقق داخل الشاشة (قصيرة وتظهر في lblError — بلا MessageBox) ──
    private const string UserRequiredMessage = "يرجى اختيار المستخدم.";
    private const string PasswordRequiredMessage = "يرجى إدخال كلمة المرور.";
    private const string UnexpectedErrorMessage = "تعذر إكمال عملية تسجيل الدخول. يرجى المحاولة مرة أخرى.";
    private const string LoadUsersFailedMessage = "تعذر تحميل قائمة المستخدمين.";

    private const string AdminUserName = "admin";
    private const string LockedSuffix = " — مقفل";

    private readonly IAuthDesktopService _authService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISessionService _sessionService;

    public frmLogin(
        IAuthDesktopService authService,
        IServiceProvider serviceProvider,
        ISessionService sessionService)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
        _sessionService = sessionService;
        InitializeComponent();

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
        this.AcceptButton = btnLogin;
        this.AutoValidate = AutoValidate.EnableAllowFocusChange;

        // تلميح وضع التجربة يظهر فقط حين يعمل التطبيق على البيانات الوهمية (§2.16).
        lblMockHint.Visible = IsMockMode;

        // Enter داخل حقل كلمة المرور = تسجيل الدخول (RULE-N42) — يُوصَّل **قبل** WireEnterAsTab.
        txtPassword.KeyDown += txtPassword_KeyDown;

        // RULE-N40…N46 — تنقل وتحديد القيمة وتظليل الحقل النشط
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnLogin);                         // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.Escape, Close));            // RULE-N47

        // RULE-N147: Enter خارج حقول الإدخال = الزر الأساسي للشاشة («دخول»).
        KeyboardHelper.WireModeEnter(this, () => btnLogin.Enabled && btnLogin.Visible ? btnLogin : null);

        ApplyUiTheme();   // RULE-N50 — آخر خطوة في تهيئة المظهر
    }

    // ── المظهر (RULE-N50…RULE-N54) ───────────────────────────────────────────

    /// <summary>
    /// ثيم الشاشة: شريط عنوان أخضر وزر الدخول من لوحة الثيم، وتنتهي الدالة بـ
    /// <see cref="GreenThemeHelper.Apply(Form)"/> كآخر سطر (RULE-N50).
    /// <para>
    /// لا يُستدعى <see cref="ThemeHelper"/> هنا لأنه يفرض خط Segoe UI وخلفية بيضاء
    /// فيمحو الثيم الأخضر (RULE-N21/RULE-N50).
    /// </para>
    /// </summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblTitle);                 // RULE-N53

        btnLogin.BackColor = GreenThemeHelper.HeaderBackground; // RULE-N54 — لا قيم جديدة
        btnLogin.ForeColor = GreenThemeHelper.HeaderForeground;

        GreenThemeHelper.Apply(this);                           // ← الأخير دائماً (RULE-N50)
    }

    // ── خصائص مساعدة ────────────────────────────────────────────────────────

    private static bool IsMockMode => AppSettings.Current.UseMockData;

    private IUserDesktopService UserService => _serviceProvider.GetRequiredService<IUserDesktopService>();

    /// <summary>المستخدم المختار حالياً في القائمة، أو <c>null</c> إن لم يُختر شيء.</summary>
    private UserDto? SelectedUser => (cmbUsers.SelectedItem as UserChoice)?.User;

    // ── دورة حياة الشاشة ────────────────────────────────────────────────────

    /// <summary>
    /// بعد ظهور الشاشة: تحميل قائمة المستخدمين ثم وضع المؤشر في كلمة المرور
    /// (لأن المستخدم الافتراضي مُختار)، أو في القائمة إن تعذّر الاختيار. **لا دخول تلقائي**.
    /// </summary>
    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);

        // RULE-N25: معالج async void بلا try/catch يُسقط التطبيق عند أي خطأ غير متوقع.
        try
        {
            errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            await LoadUsersAsync();

            if (IsDisposed) return;

            if (SelectedUser is not null)
                txtPassword.Focus();
            else
                cmbUsers.Focus();
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Log.Error(ex, "تعذر تجهيز شاشة الدخول");

            if (!IsDisposed)
                clsDialogs.Error("تعذر تجهيز شاشة الدخول. يرجى إعادة المحاولة.");
        }
    }

    // ── تحميل قائمة المستخدمين ──────────────────────────────────────────────

    /// <summary>
    /// يملأ <c>cmbUsers</c> من <see cref="IUserDesktopService.GetAllAsync"/> ويختار «admin» إن وُجد،
    /// وإلا أول مستخدم. عند الفشل: رسالة عربية + تعطيل زر الدخول + إبقاء الشاشة قابلة للاستخدام.
    /// </summary>
    private async Task LoadUsersAsync()
    {
        bool loaded = false;

        try
        {
            Cursor = Cursors.WaitCursor;
            SetLoading(true);

            var result = await UserService.GetAllAsync();

            if (!result.IsSuccess || result.Value is null || result.Value.Count == 0)
            {
                if (result.IsSuccess)
                    Log.Warning("قائمة المستخدمين فارغة — لا يمكن تسجيل الدخول");
                else
                    Log.Warning("تعذر تحميل قائمة المستخدمين — رمز الخطأ: {ErrorCode}", result.ErrorCode);

                ShowError(string.IsNullOrWhiteSpace(result.Error) ? LoadUsersFailedMessage : result.Error!);
                return;
            }

            PopulateUsers(result.Value);
            loaded = true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل تحميل قائمة المستخدمين في شاشة الدخول");
            ShowError(LoadUsersFailedMessage);
        }
        finally
        {
            SetLoading(false);
            Cursor = Cursors.Default;

            // التعطيل يأتي بعد SetLoading لأنه يعيد تفعيل الزر (ترتيب مقصود).
            if (!loaded) DisableLoginAfterLoadFailure();
        }
    }

    /// <summary>يملأ القائمة ويختار «admin» ثم أول مستخدم — الاختيار لا يُفرض على المستخدم بعده.</summary>
    private void PopulateUsers(IReadOnlyList<UserDto> users)
    {
        cmbUsers.BeginUpdate();
        try
        {
            cmbUsers.Items.Clear();

            foreach (var user in users)
                cmbUsers.Items.Add(new UserChoice(user));
        }
        finally
        {
            cmbUsers.EndUpdate();
        }

        cmbUsers.SelectedIndex = FindPreferredUserIndex();
    }

    /// <summary>موضع «admin» في القائمة، وإلا الموضع الأول — <c>-1</c> حين تكون القائمة فارغة.</summary>
    private int FindPreferredUserIndex()
    {
        for (int i = 0; i < cmbUsers.Items.Count; i++)
        {
            if (cmbUsers.Items[i] is UserChoice choice
                && string.Equals(choice.User.UserName, AdminUserName, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return cmbUsers.Items.Count > 0 ? 0 : -1;
    }

    /// <summary>تعطيل الإدخال بعد فشل تحميل المستخدمين — الشاشة تبقى مفتوحة وقابلة للإغلاق (Esc).</summary>
    private void DisableLoginAfterLoadFailure()
    {
        cmbUsers.Enabled = false;
        txtPassword.Enabled = false;
        btnLogin.Enabled = false;
    }

    // ── أحداث الإدخال ───────────────────────────────────────────────────────

    /// <summary>اختيار مستخدم آخر يمسح رسالة الخطأ السابقة (تغذية راجعة فورية).</summary>
    private void cmbUsers_SelectedIndexChanged(object? sender, EventArgs e)
    {
        errorProvider1.SetError(cmbUsers, string.Empty);
        HideError();
    }

    /// <summary>الكتابة في كلمة المرور تمسح رسالة الخطأ السابقة.</summary>
    private void txtPassword_TextChanged(object? sender, EventArgs e)
    {
        errorProvider1.SetError(txtPassword, string.Empty);
        HideError();
    }

    private void txtPassword_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // الحقل «كلمة مرور» لا رمزاً رقمياً: لا نُقيّد المحتوى (حروف/أرقام/رموز)،
        // ونمنع فقط محارف التحكم غير المسموحة في TextBox أحادي السطر.
        if (char.IsControl(e.KeyChar) && e.KeyChar != '\b')
            e.Handled = true;
    }

    /// <summary>
    /// Enter في حقل كلمة المرور = «دخول» (RULE-N42). يُوصَّل قبل <see cref="KeyboardHelper.WireEnterAsTab"/>
    /// ليسبقه في التنفيذ، ويُؤجَّل الضغط رسالةً واحدة حتى ينتهي الـHelper من الانتقال إلى الحقل التالي
    /// (Enter داخل حقل = الحقل التالي — RULE-N40) فيبقى المؤشر على الحقل الناقص آخر الأمر.
    /// </summary>
    private void txtPassword_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
            return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        if (!btnLogin.Enabled || !btnLogin.Visible || !IsHandleCreated)
            return;

        BeginInvoke(new Action(() =>
        {
            if (!IsDisposed && btnLogin.Enabled && btnLogin.Visible)
                btnLogin.PerformClick();
        }));
    }

    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        try
        {
            await PerformLoginAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل معالج حدث تسجيل الدخول");
            clsDialogs.Error(UnexpectedErrorMessage);
        }
    }

    // ── تسجيل الدخول ────────────────────────────────────────────────────────

    /// <summary>
    /// التحقق المحلي أولاً (اختيار المستخدم + كلمة المرور إلزامية) ثم المصادقة عبر
    /// <see cref="IAuthDesktopService.LoginAsync"/>. الفشل يظهر في <c>lblError</c> + أيقونة على
    /// كلمة المرور مع تفريغها ونقل المؤشر إليها، ويُسجَّل في Serilog **باسم المستخدم فقط**
    /// (لا كلمة المرور ولا نص الاستثناء للمستخدم).
    /// </summary>
    private async Task PerformLoginAsync()
    {
        ClearErrors();

        if (!_FocusFirstInvalidField())
            return;

        var user = SelectedUser;
        if (user is null) return;   // دفاع مزدوج — لا يُبلغ عنه الأسلوب أعلاه

        // كلمة المرور تُقرأ كما هي (بلا Trim ولا تطبيع) حتى لا تتغيّر قيمتها.
        string password = txtPassword.Text;
        string userName = user.UserName;

        try
        {
            Cursor = Cursors.WaitCursor;
            SetLoading(true);

            var result = await _authService.LoginAsync(new LoginRequest(userName, password));

            if (!result.IsSuccess || result.Value is null)
            {
                string message = string.IsNullOrWhiteSpace(result.Error)
                    ? "تعذر تسجيل الدخول."
                    : result.Error!;

                // لا كلمة مرور في السجل ولا نص استثناء للمستخدم (RULE-N28/RULE-N29).
                Log.Warning("فشلت مصادقة المستخدم {UserName} — رمز الخطأ: {ErrorCode}",
                    userName, result.ErrorCode);

                // التفريغ أولاً لأن TextChanged يمسح رسالة الخطأ (تغذية راجعة فورية عند الكتابة)،
                // ثم تُعرض الرسالة وتوضع الأيقونة ويُنقل المؤشر — فلا تمحو نفسها.
                txtPassword.Clear();

                errorProvider1.SetError(txtPassword, message);
                clsInputFeedback.ShowRejectTooltip(txtPassword, message);   // GAP-08b — تلميح مرئي بجانب الحقل
                ShowError(message);

                txtPassword.Focus();
                return;
            }

            Log.Information("نجحت مصادقة المستخدم {UserId}", result.Value.UserId);

            CompleteSignIn(result.Value);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل عملية تسجيل الدخول");
            clsDialogs.Error(UnexpectedErrorMessage);
        }
        finally
        {
            SetLoading(false);
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// يفحص الحقلين بالترتيب البصري (المستخدم ثم كلمة المرور) — نمط <c>frmProductMaster</c>:
    /// عند أول حقل ناقص يضع أيقونة الخطأ عليه، ينقل المؤشر إليه، ويعرض رسالة قصيرة في
    /// <c>lblError</c> بلا أي MessageBox. يُعيد <c>true</c> إذا كان الإدخال مكتملاً.
    /// </summary>
    private bool _FocusFirstInvalidField()
    {
        if (SelectedUser is null)
        {
            errorProvider1.SetError(cmbUsers, UserRequiredMessage);
            clsInputFeedback.ShowRejectTooltip(cmbUsers, UserRequiredMessage);   // GAP-08b
            ShowError(UserRequiredMessage);
            cmbUsers.Focus();
            return false;
        }

        if (!clsValidation.IsRequired(txtPassword.Text))
        {
            errorProvider1.SetError(txtPassword, PasswordRequiredMessage);
            clsInputFeedback.ShowRejectTooltip(txtPassword, PasswordRequiredMessage);   // GAP-08b
            ShowError(PasswordRequiredMessage);
            txtPassword.Focus();
            return false;
        }

        return true;
    }

    // ── رسائل الخطأ داخل الشاشة ─────────────────────────────────────────────

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

    private void ClearErrors()
    {
        errorProvider1.Clear();
        HideError();
    }

    // ── إنهاء الدخول المشترك ────────────────────────────────────────────────

    /// <summary>
    /// يهيّئ الجلسة الثابتة (SessionManager) وجلسة DI (ISessionService) ثم يفتح frmMain.
    /// إذا كانت الاستجابة تُلزم بتغيير كلمة المرور فلا تُفتح frmMain قبل التغيير وإعادة المصادقة.
    /// </summary>
    private void CompleteSignIn(LoginResponse loginResponse)
    {
        if (loginResponse.MustChangePassword)
        {
            HandleMustChangePassword(loginResponse);
            return;
        }

        // Initialize the static session manager (permissions / identity)
        SessionManager.Initialize(loginResponse);

        // Initialize the DI session service used by Direct*Service wrappers
        _sessionService.SignIn(new UserSession
        {
            UserId = loginResponse.UserId,
            UserName = loginResponse.UserName,
            FullName = loginResponse.FullName,
            // الدور مشتق من الاستجابة القادمة من الطبقة التجارية (لا افتراض محلي بمدير)
            Role = loginResponse.IsAdmin ? UserRole.Admin : UserRole.Observer,
            Token = loginResponse.Token
        });

        var mainForm = _serviceProvider.GetRequiredService<frmMain>();
        mainForm.Show();
        this.Hide();
    }

    /// <summary>
    /// MustChangePassword == true: إلزام المستخدم بتغيير كلمة المرور عبر frmPasswordChange
    /// ثم طلب إعادة تسجيل الدخول بكلمة المرور الجديدة (لا تُفتح frmMain قبل ذلك).
    /// </summary>
    private void HandleMustChangePassword(LoginResponse loginResponse)
    {
        clsDialogs.Warning("يجب تغيير كلمة المرور قبل المتابعة.");

        try
        {
            using var changePasswordForm = _serviceProvider.GetRequiredService<frmPasswordChange>();
            changePasswordForm._SetSelfServiceMode(loginResponse.UserId);

            var dialogResult = changePasswordForm.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                Log.Information("غيّر المستخدم {UserId} كلمة المرور — إعادة المصادقة مطلوبة",
                    loginResponse.UserId);

                clsDialogs.Success("تم تغيير كلمة المرور بنجاح. يرجى تسجيل الدخول بكلمة المرور الجديدة.");
            }
            else
            {
                Log.Warning("أُلغيت عملية تغيير كلمة المرور الإلزامية للمستخدم {UserId}",
                    loginResponse.UserId);

                clsDialogs.Warning("لم يتم تغيير كلمة المرور، ولا يمكن المتابعة قبل تغييرها.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر فتح شاشة تغيير كلمة المرور الإلزامية للمستخدم {UserId}",
                loginResponse.UserId);

            clsDialogs.Error("تعذر فتح شاشة تغيير كلمة المرور. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            // إعادة المصادقة تكون بكلمة المرور الجديدة على نفس الشاشة
            txtPassword.Clear();
            txtPassword.Focus();
        }
    }

    // ── حالة التحميل ────────────────────────────────────────────────────────

    /// <summary>حالة الانتظار: تعطيل الإدخال + نص «جاري الدخول...» — ويُعاد كل شيء في <c>finally</c>.</summary>
    private void SetLoading(bool isLoading)
    {
        cmbUsers.Enabled = !isLoading;
        txtPassword.Enabled = !isLoading;
        btnLogin.Enabled = !isLoading;
        btnLogin.Text = isLoading ? "جاري الدخول..." : "دخول";
    }

    // ── عنصر قائمة اختيار المستخدم ──────────────────────────────────────────

    /// <summary>
    /// عنصر عرض القائمة (عارض نصّي بسيط — ليس كائناً مستقلاً عن الشاشة): يعرض اسم المستخدم،
    /// ويُظهر «— مقفل» للمقفل حتى يرى المستخدم سبب الرفض قبل المحاولة (RULE-N167).
    /// </summary>
    private sealed class UserChoice
    {
        public UserChoice(UserDto user) => User = user;

        public UserDto User { get; }

        public override string ToString() => User.IsLocked ? User.UserName + LockedSuffix : User.UserName;
    }
}
