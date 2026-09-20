using System.ComponentModel;
using Serilog;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Models;
using SalesSystem.DTO.Requests;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Screens.Users.Forms;

namespace SalesSystem.Desktop.Screens;

public partial class frmLogin : Form
{
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

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        errorProvider1.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    }

    private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
    {
        TextBox Temp = ((TextBox)sender);
        if (string.IsNullOrEmpty(Temp.Text.Trim()))
        {
            e.Cancel = true;
            errorProvider1.SetError(Temp, "هذا الحقل مطلوب");
        }
        else
        {
            e.Cancel = false;
            errorProvider1.SetError(Temp, null);
        }
    }

    private void txtUserName_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Enter)
            txtPassword.Focus();
    }

    private void txtPassword_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Enter)
            btnLogin.Focus();
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        await PerformLoginAsync();
    }

    private async Task PerformLoginAsync()
    {
        errorProvider1.Clear();

        if (!this.ValidateChildren())
        {
            MessageBox.Show("بعض الحقول المطلوبة فارغة لا يمكنك الاستمرار, قم تأشير على العلامة الحمراء رمعرفة الخطاء",
                "خطاء البيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                txtUserName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Focus();
                return;
            }

            return;
        }

        string username = txtUserName.Text.Trim();
        string password = txtPassword.Text.Trim();

        try
        {
            Cursor = Cursors.WaitCursor;
            SetLoading(true);
            var result = await _authService.LoginAsync(new LoginRequest(username, password));

            if (result.IsSuccess && result.Value != null)
            {
                // Initialize the static session manager
                SessionManager.Initialize(result.Value);

                // Initialize the DI session service so AuthTokenHandler adds the Bearer header
                _sessionService.SignIn(new UserSession
                {
                    UserId = result.Value.UserId,
                    UserName = result.Value.UserName,
                    FullName = result.Value.UserName,
                    Role = (UserRole)result.Value.RoleId,
                    Token = result.Value.Token
                });

                // Check if password must be changed
                if (result.Value.MustChangePassword)
                {
                    var passwordForm = _serviceProvider.GetRequiredService<frmPasswordChange>();
                    // Passwordless users (IsPasswordless = true) go through update mode
                    // (no current password required). Users with existing passwords
                    // entering MustChangePassword flow verify their current password.
                    if (result.Value.IsPasswordless)
                    {
                        passwordForm._SetUpdateMode(result.Value.UserId);
                    }
                    else
                    {
                        passwordForm._SetSelfServiceMode(result.Value.UserId);
                    }

                    if (passwordForm.ShowDialog(this) == DialogResult.Cancel)
                    {
                        // User cancelled password change — cannot proceed
                        SessionManager.Clear();
                        MessageBox.Show("يجب تغيير كلمة المرور قبل المتابعة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                var mainForm = _serviceProvider.GetRequiredService<frmMain>();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                string errorMessage = result.Error ?? "البيانات المدخلة غير صحيحة أو حدث خطأ في الخادم";
                MessageBox.Show(errorMessage, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "فشل عملية تسجيل الدخول لمستخدم: {Username}", username);
            MessageBox.Show("تعذر الاتصال بالخادم. يرجى التحقق من اتصال الإنترنت.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetLoading(false);
        }
    }

    private void SetLoading(bool isLoading)
    {
        btnLogin.Enabled = !isLoading;
        btnLogin.Text = isLoading ? "جاري الدخول..." : "دخول";
        txtUserName.Enabled = !isLoading;
        txtPassword.Enabled = !isLoading;
    }
}
