using Serilog;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.Models;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Services;

namespace SalesSystem.Desktop.Screens;

/// <summary>
/// شاشة تسجيل الدخول — نسخة MVP (متجر واحد / مستخدم واحد هو المدير المُهيأ).
/// <list type="bullet">
/// <item>وضع التجربة (Mock): تسجيل دخول تلقائي بالمدير دون أي إدخال من المستخدم.</item>
/// <item>الوضع الحقيقي (BLL + قاعدة البيانات): إدخال رمز رقمي PIN يُقرأ من إعداد النظام "AdminPin".</item>
/// </list>
/// </summary>
public partial class frmLogin : Form
{
    // ── بيانات المدير المُهيأ (MVP: مستخدم واحد فقط) ─────────────────────────
    private const int AdminUserId = 1;
    private const string AdminLoginName = "admin";
    private const string AdminDisplayName = "المدير";
    private const string AdminRoleName = "مدير النظام";

    /// <summary>مفتاح إعداد النظام الذي يحمل رمز الدخول الرقمي في الوضع الحقيقي.</summary>
    private const string AdminPinKey = "AdminPin";
    private const string DefaultAdminPin = "1234";

    private readonly IAuthService _authService;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISessionService _sessionService;

    public frmLogin(
        IAuthService authService,
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
    }

    // ── خصائص مساعدة ────────────────────────────────────────────────────────

    private static bool IsMockMode => AppSettings.Current.UseMockData;

    private ISettingsApiService SettingsService
        => _serviceProvider.GetRequiredService<ISettingsApiService>();

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
        // السماح بالأرقام ومفاتيح التحكم فقط (Backspace / Delete)
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            e.Handled = true;
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
            MessageBox.Show("حدث خطأ غير متوقع أثناء تسجيل الدخول. يرجى المحاولة مرة أخرى.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ? result.Value with { UserName = AdminDisplayName, RoleName = AdminRoleName }
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

            MessageBox.Show("تعذر تسجيل الدخول التلقائي. يرجى إدخال رمز الدخول (PIN).",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetLoading(false);
            Cursor = Cursors.Default;
        }
    }

    // ── الوضع الحقيقي: دخول برمز رقمي PIN ───────────────────────────────────

    private async Task PerformPinLoginAsync()
    {
        errorProvider1.Clear();
        lblError.Visible = false;
        lblError.Text = string.Empty;

        string pin = NormalizeDigits(txtPin.Text);

        if (string.IsNullOrWhiteSpace(pin))
        {
            errorProvider1.SetError(txtPin, "هذا الحقل مطلوب");
            MessageBox.Show("يرجى إدخال رمز الدخول (PIN).", "بيانات ناقصة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPin.Focus();
            return;
        }

        if (!pin.All(char.IsDigit))
        {
            errorProvider1.SetError(txtPin, "الرمز يجب أن يكون أرقاماً فقط");
            MessageBox.Show("الرمز يجب أن يكون أرقاماً فقط.", "بيانات غير صحيحة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPin.Clear();
            txtPin.Focus();
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            SetLoading(true);

            var expectedPin = await GetAdminPinAsync();

            if (!string.Equals(pin, expectedPin, StringComparison.Ordinal))
            {
                Log.Warning("محاولة دخول برمز PIN غير صحيح");

                lblError.Text = "الرمز غير صحيح";
                lblError.Visible = true;

                MessageBox.Show("الرمز غير صحيح", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPin.Clear();
                txtPin.Focus();
                return;
            }

            Log.Information("تسجيل دخول المدير بواسطة رمز PIN");
            CompleteSignIn(CreateAdminLoginResponse());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل عملية تسجيل الدخول برمز PIN");
            MessageBox.Show("تعذر إكمال عملية تسجيل الدخول. يرجى المحاولة مرة أخرى.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoading(false);
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// يقرأ رمز الدخول من إعدادات النظام ("AdminPin")، ويُهيّئ القيمة الافتراضية
    /// "1234" عند غيابها (أول تشغيل) حتى تصبح قابلة للتعديل من شاشة الإعدادات لاحقاً.
    /// </summary>
    private async Task<string> GetAdminPinAsync()
    {
        try
        {
            var result = await SettingsService.GetAllSystemSettingsAsync();

            if (result.IsSuccess && result.Value is not null
                && result.Value.TryGetValue(AdminPinKey, out var storedPin)
                && !string.IsNullOrWhiteSpace(storedPin))
            {
                var normalized = NormalizeDigits(storedPin);

                // إعداد تالف (حروف بدل أرقام) لا يُستخدم حتى لا تُقفل شاشة الدخول نهائياً
                if (normalized.All(char.IsDigit))
                    return normalized;

                Log.Warning("إعداد {Key} غير صالح (يجب أن يكون أرقاماً فقط) — سيتم استخدام القيمة الافتراضية", AdminPinKey);
                return DefaultAdminPin;
            }

            // القيمة غير موجودة → تهيئة الافتراضي (أفضل جهد، لا يعطّل الدخول عند الفشل)
            var seedResult = await SettingsService.UpdateSystemSettingsAsync(
                new Dictionary<string, string> { [AdminPinKey] = DefaultAdminPin });

            if (!seedResult.IsSuccess)
                Log.Warning("تعذر تهيئة إعداد {Key}: {Error}", AdminPinKey, seedResult.Error);

            return DefaultAdminPin;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر قراءة إعداد {Key} — سيتم استخدام القيمة الافتراضية", AdminPinKey);
            return DefaultAdminPin;
        }
    }

    /// <summary>يحوّل الأرقام العربية/الفارسية إلى أرقام لاتينية ويزيل المسافات.</summary>
    private static string NormalizeDigits(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var chars = value.Trim()
            .Where(c => !char.IsWhiteSpace(c))
            .Select(c => c switch
            {
                >= '\u0660' and <= '\u0669' => (char)('0' + (c - '\u0660')), // ٠-٩
                >= '\u06F0' and <= '\u06F9' => (char)('0' + (c - '\u06F0')), // ۰-۹
                _ => c
            });

        return new string(chars.ToArray());
    }

    // ── إنهاء الدخول المشترك ────────────────────────────────────────────────

    /// <summary>
    /// يهيّئ الجلسة الثابتة (SessionManager) وجلسة DI (ISessionService) ثم يفتح frmMain.
    /// نفس التدفق المستخدم سابقاً بعد نجاح المصادقة.
    /// </summary>
    private void CompleteSignIn(LoginResponse loginResponse)
    {
        // Initialize the static session manager (permissions / identity)
        SessionManager.Initialize(loginResponse);

        // Initialize the DI session service used by Direct*Service wrappers
        _sessionService.SignIn(new UserSession
        {
            UserId = loginResponse.UserId,
            UserName = loginResponse.UserName,
            FullName = loginResponse.FullName,
            Role = (UserRole)loginResponse.RoleId,
            Token = loginResponse.Token
        });

        // TODO (MVP): تم إسقاط خطوة تغيير كلمة المرور/الرمز في نسخة MVP.
        // عند إضافة شاشة إدارة رمز الدخول، أعد ربط frmPasswordChange هنا
        // عند loginResponse.MustChangePassword == true.

        var mainForm = _serviceProvider.GetRequiredService<frmMain>();
        mainForm.Show();
        this.Hide();
    }

    /// <summary>
    /// استجابة دخول المدير المُهيأ (UserId = 1) — تُستخدم في الوضع الحقيقي
    /// حيث لا تتوفر كلمة مرور، وفي حال فشل مصادقة وضع التجربة.
    /// </summary>
    private static LoginResponse CreateAdminLoginResponse() => new(
        UserId: AdminUserId,
        UserName: AdminDisplayName,
        RoleId: (short)UserRole.Admin,
        RoleName: AdminRoleName,
        BranchId: null,
        Token: "local-admin-session",
        ExpiresAt: DateTime.UtcNow.AddHours(8),
        MustChangePassword: false,
        IsPasswordless: true,
        SalesPermissions: -1,
        InventoryPermissions: -1,
        AccountingPermissions: -1,
        SystemPermissions: -1);

    // ── حالة التحميل ────────────────────────────────────────────────────────

    private void SetLoading(bool isLoading)
    {
        btnLogin.Enabled = !isLoading;
        btnLogin.Text = isLoading ? "جاري الدخول..." : "دخول";
        txtPin.Enabled = !isLoading;
    }
}
