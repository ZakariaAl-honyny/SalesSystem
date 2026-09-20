using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

/// <summary>
/// نافذة إلغاء سند قبض / صرف — نموذج واجهة (UI Shape) يعمل ببيانات وهمية
/// في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// إجراء حاسم لا يمكن التراجع عنه: يتطلب اختيار سبب الإلغاء قبل التأكيد،
/// ثم يعرض رسالة نجاح ويُغلق بنتيجة DialogResult.OK.
/// </summary>
public partial class frmCashVoucherVoid : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── بيانات السند الوهمية (Mock) — نفس سند العرض رقم 1003 ──────────────────

    private const string MockVoucherNo = "1003";       // رقم السند
    private const string MockVoucherType = "قبض";      // قبض / صرف
    private const decimal MockVoucherAmount = 150000m; // المبلغ (ريال يمني)

    // الأسباب الشائعة للإلغاء
    private static readonly string[] CommonVoidReasons =
    {
        "خطأ في الإدخال",
        "سند مكرر",
        "طلب العميل",
        "أخرى (بيان حر)",
    };

    public frmCashVoucherVoid(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnConfirm.PerformClick()));
        _LoadMockReasons();
    }

    // ── تحميل أسباب الإلغاء الوهمية ───────────────────────────────────────────

    private void _LoadMockReasons()
    {
        cmbReason.Items.Clear();
        cmbReason.Items.Add("اختر سبب الإلغاء…");

        foreach (var reason in CommonVoidReasons)
            cmbReason.Items.Add(reason);

        cmbReason.SelectedIndex = 0;
    }

    // ── التحقق من صحة البيانات ────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        // سبب الإلغاء مطلوب (العنصر 0 هو مجرد تلميح "اختر سبب الإلغاء…")
        if (cmbReason.SelectedIndex <= 0)
        {
            errorProvider.SetError(cmbReason, "سبب الإلغاء مطلوب.");
            MessageBox.Show("يرجى اختيار سبب الإلغاء.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        errorProvider.SetError(cmbReason, "");

        // عند اختيار «أخرى» يجب كتابة تفاصيل السبب في البيان الحر
        if (cmbReason.Text == "أخرى (بيان حر)" && string.IsNullOrWhiteSpace(txtReasonDetails.Text))
        {
            errorProvider.SetError(txtReasonDetails, "يرجى توضيح السبب في البيان الحر.");
            MessageBox.Show("عند اختيار «أخرى» يجب كتابة تفاصيل السبب.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        errorProvider.SetError(txtReasonDetails, "");
        return true;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnConfirm_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        var reason = cmbReason.Text;
        var details = txtReasonDetails.Text.Trim();

        var message =
            $"تم إلغاء السند رقم {MockVoucherNo} — {MockVoucherType} — " +
            $"{MockVoucherAmount:N0} ريال يمني بنجاح.\n" +
            $"سبب الإلغاء: {reason}" +
            (details.Length > 0 ? $"\nالتفاصيل: {details}" : "") +
            "\n\n(وضع تجريبي — تمت المحاكاة دون اتصال بالخادم)";

        MessageBox.Show(message, "تم الإلغاء",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
        => Close();
}