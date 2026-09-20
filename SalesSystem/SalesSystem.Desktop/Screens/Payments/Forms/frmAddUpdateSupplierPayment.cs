using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms;

public partial class frmAddUpdateSupplierPayment : Form
{
    public frmAddUpdateSupplierPayment()
    {
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    private void frmSupplierPaymentEditor_Load(object sender, EventArgs e)
    {
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
