using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// نموذج إضافة مورد — واجهة تجريبية (UI-first prototype) تعمل ببيانات وهمية في الذاكرة
/// بدون اتصال بالخادم أو قاعدة البيانات. يُعرض عبر ShowDialog().
/// </summary>
public partial class frmAddSupplier : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── Mock data (UI-first prototype — no backend) ──────────────────────────
    // سجل الموردين في الذاكرة: يحاكي الحفظ خلال جلسة التشغيل دون قاعدة بيانات.
    private static readonly List<MockSupplier> MockSuppliers = new();

    private sealed record MockSupplier(
        string Name,
        string Phone,
        string Email,
        string Address,
        string TaxNumber,
        decimal CreditLimit,
        string Notes);

    // محجوز لربط خدمات الخادم الحقيقية لاحقاً (RULE-N03/RULE-N04) — الواجهة الحالية
    // لا تستدعي أي API وتعمل بالكامل ببيانات وهمية.
    private IServiceProvider ServiceProvider => _serviceProvider;

    public frmAddSupplier(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
    }

    /// <summary>التحقق الأساسي: الاسم والهاتف مطلوبان.</summary>
    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المورد مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (string.IsNullOrWhiteSpace(txtPhone.Text))
        {
            errorProvider.SetError(txtPhone, "رقم الهاتف مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtPhone, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        btnSave.Enabled = false;
        try
        {
            // Mock save: إضافة المورد إلى الذاكرة (بدون خادم أو قاعدة بيانات)
            MockSuppliers.Add(new MockSupplier(
                txtName.Text.Trim(),
                txtPhone.Text.Trim(),
                txtEmail.Text.Trim(),
                txtAddress.Text.Trim(),
                txtTaxNumber.Text.Trim(),
                nudCreditLimit.Value,
                txtNotes.Text.Trim()));

            Log.Information("تمت إضافة مورد (وضع تجريبي): {SupplierName}", txtName.Text.Trim());

            MessageBox.Show("تمت إضافة المورد بنجاح.", "إضافة مورد",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ بيانات المورد (وضع تجريبي)");
            MessageBox.Show("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}