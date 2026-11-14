using Serilog;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.Models;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens;

/// <summary>
/// شاشة تسجيل الدخول.
/// <list type="bullet">
/// <item>وضع التجربة (Mock): تسجيل دخول تلقائي بالمدير دون أي إدخال من المستخدم.</item>
/// <item>الوضع الحقيقي (BLL + قاعدة البيانات): مصادقة حقيقية عبر <see cref="IAuthDesktopService.LoginAsync"/>
/// (BCrypt + قفل الحساب بعد المحاولات الفاشلة + MustChangePassword) — لا مقارنة محلية لأي كلمة مرور.</item>
/// </list>
/// </summary>
public partial class frmLogin : Form
{
    // ── بيانات المدير المُهيأ (MVP: مستخدم واحد فقط) ─────────────────────────
    private const int AdminUserId = 1;
    private const string AdminLoginName = "admin";
    private const string AdminDisplayName = "المدير";

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

        // §2.21/2: Enter في حقل كلمة المرور = تسجيل الدخول فوراً.
        // يُوصَّل **قبل** WireEnterAsTab حتى لا يُحوِّل Enter إلى انتقال تركيز (RULE-N42).
        txtPin.KeyDown += txtPin_KeyDown;

        // RULE-N40…N46 — تنقل وتحديد القيمة وتظليل الحقل النشط
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireButtonDisabledVisual(btnLogin);                        // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.Escape, Close));           // RULE-N47

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

    // ── دورة حياة الشاشة ────────────────────────────────────────────────────

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        if (IsMockMode)
        {
            // وضع التجربة: إخفاء أدوات الإدخال والانتقال التلقائي إلى frmMain
            lblPin.Visible = false;
            txtPin.Visible = false;
            btnLogin.Visible = false;
            lblTitle.Text = "جاري تسجيل الدخول...";

            // BeginInvoke يضمن رسم الشاشة أولاً ثم تنفيذ الدخول التلقائي
            BeginInvoke(new Action(() => _ = AutoLoginAsync()));
        }
        else
        {
            lblTitle.Text = "برنامج إدارة المبيعات";
            SetPinModeVisible();
            txtPin.Focus();
        }
    }

    private void SetPinModeVisible()
    {
        lblPin.Visible = true;
        txtPin.Visible = true;
        btnLogin.Visible = true;
    }

    // ── أحداث الإدخال ───────────────────────────────────────────────────────

    private void txtPin_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // الحقل صار «كلمة مرور» لا رمزاً رقمياً: لا نُقيّد المحتوى (حروف/أرقام/رموز)،
        // ونمنع فقط محارف التحكم غير المسموحة في TextBox أحادي السطر.
        if (char.IsControl(e.KeyChar) && e.KeyChar != '\b')
            e.Handled = true;
    }

    /// <summary>
    /// Enter في حقل كلمة المرور = تسجيل الدخول مباشرة (بدل تحويله إلى انتقال تركيز).
    /// يُوصَّل قبل <see cref="KeyboardHelper.WireEnterAsTab"/> في المُنشئ ليسبقه في التنفيذ.
    /// </summary>
    private void txtPin_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
            return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        if (btnLogin.Enabled && btnLogin.Visible)
            btnLogin.PerformClick();
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            // في وضع التجربة يكفي زر الدخول لتسجيل الدخول التلقائي (وسيلة احتياطية)
            if (IsMockMode)
            {
                await AutoLoginAsync();
                return;
            }

            await PerformPinLoginAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل معالج حدث تسجيل الدخول");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى.");
        }
    }

    // ── وضع التجربة (Mock): دخول تلقائي ─────────────────────────────────────

    /// <summary>
    /// تسجيل دخول تلقائي بالمدير المُهيأ في وضع التجربة — بدون قاعدة بيانات
    /// (MockAuthService لا يلمس قاعدة البيانات إطلاقاً).
    /// </summary>
    private async Task AutoLoginAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            SetLoading(true);

            var result = await _authService.LoginAsync(new LoginRequest(AdminLoginName, string.Empty));

            var loginResponse = result.IsSuccess && result.Value is not null
                ? result.Value with { UserName = AdminDisplayName, IsAdmin = true }
                : CreateAdminLoginResponse();

            CompleteSignIn(loginResponse);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل تسجيل الدخول التلقائي في وضع التجربة");

            // احتياط: إظهار إدخال الرمز بدلاً من ترك المستخدم في شاشة فارغة
            lblTitle.Text = "برنامج إدارة المبيعات";
            SetPinModeVisible();
            txtPin.Focus();

            clsDialogs.Warning("تعذر تسجيل الدخول التلقائي. يرجى إدخال كلمة المرور.");
        }
        finally
        {
            SetLoading(false);
            Cursor = Cursors.Default;
        }
    }

    // ── الوضع الحقيقي: مصادقة حقيقية عبر BLL ─────────────────────────────────

    /// <summary>
    /// مصادقة حقيقية: تُمرَّر كلمة المرور المكتوبة إلى <see cref="IAuthDesktopService.LoginAsync"/>
    /// (BLL AuthService: BCrypt + عدّ المحاولات + القفل بعد 5 محاولات + MustChangePassword).
    /// لا مقارنة محلية ولا عدّاد محاولات في الواجهة — كل ذلك مسؤولية الطبقة التجارية.
    /// </summary>
    private async Task PerformPinLoginAsync()
    {
        errorProvider1.Clear();
        lblError.Visible = false;
        lblError.Text = string.Empty;

        // كلمة المرور تُقرأ كما هي (Trim فقط) — لا تطبيع ولا تحويل حتى لا تتغيّر قيمتها
        string password = txtPin.Text.Trim();

        if (string.IsNullOrWhiteSpace(password))
        {
            errorProvider1.SetError(txtPin, "هذا الحقل مطلوب");
            clsDialogs.Warning("يرجى إدخال كلمة المرور.");
            txtPin.Focus();
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            SetLoading(true);

            var result = await _authService.LoginAsync(new LoginRequest(AdminLoginName, password));

            if (!result.IsSuccess || result.Value is null)
            {
                // لا تُسجَّل كلمة المرور ولا يُعرض نص الاستثناء — رسالة الطبقة التجارية فقط
                Log.Warning("فشلت مصادقة المستخدم {UserName} — رمز الخطأ: {ErrorCode}",
                    AdminLoginName, result.ErrorCode);

                string message = string.IsNullOrWhiteSpace(result.Error)
                    ? "كلمة المرور غير صحيحة."
                    : result.Error!;

                errorProvider1.SetError(txtPin, message);
                lblError.Text = message;
                lblError.Visible = true;

                clsDialogs.Error(message);

                txtPin.Clear();
                txtPin.Focus();
                return;
            }

            var loginResponse = result.Value;
            Log.Information("نجحت مصادقة المستخدم {UserId}", loginResponse.UserId);

            CompleteSignIn(loginResponse);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل عملية تسجيل الدخول");
            clsDialogs.Error("تعذر إكمال عملية تسجيل الدخول. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            SetLoading(false);
            Cursor = Cursors.Default;
        }
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
            txtPin.Clear();
            txtPin.Focus();
        }
    }

    /// <summary>
    /// استجابة دخول المدير المُهيأ (UserId = 1) — تُستخدم كاحتياط في وضع التجربة
    /// فقط عند فشل مصادقة <c>MockAuthService</c>. لا تُستخدم في الوضع الحقيقي.
    /// </summary>
    private static LoginResponse CreateAdminLoginResponse() => new(
        UserId: AdminUserId,
        UserName: AdminDisplayName,
        IsAdmin: true,
        BranchId: null,
        Token: "local-admin-session",
        ExpiresAt: DateTime.UtcNow.AddHours(8),
        MustChangePassword: false,
        IsPasswordless: true);

    // ── حالة التحميل ────────────────────────────────────────────────────────

    private void SetLoading(bool isLoading)
    {
        btnLogin.Enabled = !isLoading;
        btnLogin.Text = isLoading ? "جاري الدخول..." : "دخول";
        txtPin.Enabled = !isLoading;
    }
}
