using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens.Users.Forms;

/// <summary>
/// شاشة تغيير/إعادة تعيين كلمة المرور — تُفتح نمطياً (RULE-N09).
/// لا تحتوي أي منطق تشفير: تُمرّر الطلب إلى <see cref="IAuthDesktopService"/> وحدها (RULE-N06).
/// </summary>
public partial class frmPasswordChange : Form
{
    /// <summary>الحد الأدنى لطول كلمة المرور الجديدة (سياسة موحّدة مع خدمة المصادقة — RULE-N36).</summary>
    private const int MinPasswordLength = 6;

    private readonly IServiceProvider _serviceProvider;
    private int _UserId;
    private bool _isSelfService; // true when called from login (MustChangePassword flow)

    public frmPasswordChange(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N47 — كل الستة من KeyboardHelper (RULE-N95)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46 — الحقل النشط
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45 — أسهم RTL
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnClose);                 // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F2, () => btnSave.PerformClick()),
            (Keys.Escape, () => btnClose.PerformClick()));
    }

    // ── التحقق ────────────────────────────────────────────────────────────────

    /// <summary>
    /// تحقق كامل قبل الحفظ: الحالية مطلوبة (في الخدمة الذاتية) · الجديدة ≥ الحد الأدنى ·
    /// الجديدة ≠ الحالية · التأكيد مطابق. الرسائل عربية عبر errorProvider (RULE-N28).
    /// </summary>
    private bool _ValidateForm()
    {
        bool isValid = true;

        // كلمة المرور الحالية مطلوبة في وضع الخدمة الذاتية فقط (شاشة الدخول).
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
        else if (txtNewPassword.Text.Length < MinPasswordLength)
        {
            errorProvider.SetError(txtNewPassword, $"كلمة المرور الجديدة يجب ألا تقل عن {MinPasswordLength} خانات");
            isValid = false;
        }
        else if (_isSelfService
                 && !string.IsNullOrWhiteSpace(txtCurrentPassword.Text)
                 && txtNewPassword.Text == txtCurrentPassword.Text)
        {
            errorProvider.SetError(txtNewPassword, "كلمة المرور الجديدة يجب أن تختلف عن الحالية");
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
        else if (!string.IsNullOrWhiteSpace(txtNewPassword.Text) &&
                 txtNewPassword.Text != txtConfirmPassword.Text)
        {
            errorProvider.SetError(txtConfirmPassword, "كلمة المرور غير متطابقة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtConfirmPassword, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    // ── الحفظ ─────────────────────────────────────────────────────────────────

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            // الخدمة هي المسؤولة عن التشفير والتحقق النهائي — الشاشة لا تعرف BCrypt إطلاقاً.
            var authService = _serviceProvider.GetRequiredService<IAuthDesktopService>();
            var request = new ChangePasswordRequest(
                txtCurrentPassword.Text,
                txtNewPassword.Text,
                txtConfirmPassword.Text);

            var result = await authService.ChangePasswordAsync(request, _UserId);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تغيير كلمة المرور بنجاح.");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "فشل في تغيير كلمة المرور.");
            }
        }
        catch (Exception ex)
        {
            // RULE-N29 — التفاصيل تُسجَّل ولا تُعرض للمستخدم
            Serilog.Log.Error(ex, "فشل تغيير كلمة المرور للمستخدم {UserId}", _UserId);
            clsDialogs.Error("تعذر تغيير كلمة المرور. يرجى إعادة المحاولة.");
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    // ── أوضاع الفتح ───────────────────────────────────────────────────────────

    /// <summary>
    /// Sets the editor to Update mode for the given user ID (فتح من الدليل — إعادة تعيين إدارية).
    /// </summary>
    public void _SetUpdateMode(int userId)
    {
        _UserId = userId;
        _isSelfService = false;
        lblMode.Text = "إعادة تعيين كلمة المرور";
        Text = "إعادة تعيين كلمة المرور";
        // إعادة التعيين الإدارية لا تطلب ولا تستهلك كلمة المرور الحالية.
        lblCurrentPassword.Visible = false;
        txtCurrentPassword.Visible = false;
    }

    /// <summary>
    /// Sets the editor to self-service mode (called from login when MustChangePassword is true).
    /// </summary>
    public void _SetSelfServiceMode(int userId)
    {
        _UserId = userId;
        _isSelfService = true;
        lblMode.Text = "تغيير كلمة المرور";
        Text = "تغيير كلمة المرور";
        lblCurrentPassword.Visible = true;
        txtCurrentPassword.Visible = true;
    }

    // ── التحميل والثيم ─────────────────────────────────────────────────────────

    private void frmPasswordChange_Load(object sender, EventArgs e)
    {
        try
        {
            ApplyUiTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
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
