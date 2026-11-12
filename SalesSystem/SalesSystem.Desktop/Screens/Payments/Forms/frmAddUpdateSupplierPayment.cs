using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms;

public partial class frmAddUpdateSupplierPayment : Form
{
    public frmAddUpdateSupplierPayment()
    {
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);                                        // RULE-N40
        KeyboardHelper.WireSelectAllOnFocus(this);                                  // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    private void frmSupplierPaymentEditor_Load(object sender, EventArgs e)
    {
        ApplyGreenTheme();   // RULE-N50 — الأخير في مسار التحميل
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "إنشاء سند صرف جديد قيد التطوير. يرجى استخدام نافذة الصرف السريع من شاشة المدفوعات.",
            "تحت التطوير",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }
}
