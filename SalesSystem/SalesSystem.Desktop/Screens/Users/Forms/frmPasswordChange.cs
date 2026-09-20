using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Screens.Users;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Users.Forms;

    public partial class frmPasswordChange : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private int _UserId;
        private bool _isSelfService; // true when called from login (MustChangePassword flow)

        public frmPasswordChange(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

    private bool _ValidateForm()
    {
        bool isValid = true;

        // Current password is required for self-service (login flow)
        if (_isSelfService && string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
        {
            errorProvider.SetError(txtCurrentPassword, "كلمة المرور الحالية مطلوبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtCurrentPassword, "");
        }

        if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
        {
            errorProvider.SetError(txtNewPassword, "كلمة المرور الجديدة مطلوبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtNewPassword, "");
        }

        if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
        {
            errorProvider.SetError(txtConfirmPassword, "تأكيد كلمة المرور مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtConfirmPassword, "");
        }

        if (!string.IsNullOrWhiteSpace(txtNewPassword.Text) &&
            !string.IsNullOrWhiteSpace(txtConfirmPassword.Text) &&
            txtNewPassword.Text != txtConfirmPassword.Text)
        {
            errorProvider.SetError(txtConfirmPassword, "كلمة المرور غير متطابقة");
            isValid = false;
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm())
            return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            // Use the auth service to change password via API
            var authService = _serviceProvider.GetRequiredService<IAuthService>();
            var request = new ChangePasswordRequest(
                txtCurrentPassword.Text,
                txtNewPassword.Text,
                txtConfirmPassword.Text);

            var result = await authService.ChangePasswordAsync(request, _UserId);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تغيير كلمة المرور بنجاح.", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في تغيير كلمة المرور", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تغيير كلمة المرور للمستخدم {UserId}", _UserId);
            MessageBox.Show("حدث خطأ أثناء تغيير كلمة المرور.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    /// <summary>
    /// Sets the editor to Update mode for the given user ID.
    /// Called after DI resolution (parameterless constructor).
    /// </summary>
    public void _SetUpdateMode(int userId)
    {
        _UserId = userId;
        _isSelfService = false;
        lblMode.Text = "إعادة تعيين كلمة المرور";
        this.Text = "إعادة تعيين كلمة المرور";
        
    }

    /// <summary>
    /// Sets the editor to self-service mode (called from login when MustChangePassword is true).
    /// </summary>
    public void _SetSelfServiceMode(int userId)
    {
        _UserId = userId;
        _isSelfService = true;
        lblMode.Text = "تغيير كلمة المرور";
        this.Text = "تغيير كلمة المرور";
        lblCurrentPassword.Visible = true;
        txtCurrentPassword.Visible = true;
    }

    private void frmPasswordChange_Load(object sender, EventArgs e)
    {
        try
        {
            // Default to self-service if called from login
            if (_UserId > 0 && !_isSelfService)
            {
                // Called from list — admin reset mode
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ: " + ex.Message, "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
