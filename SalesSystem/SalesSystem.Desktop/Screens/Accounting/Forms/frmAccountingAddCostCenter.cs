using System;
using System.Windows.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    /// <summary>
    /// نافذة «إضافة مركز تكلفة» — نموذج تجريبي (UI-first prototype) ببيانات داخل الذاكرة فقط.
    /// لا تتصل بأي API أو قاعدة بيانات — تُعرض كـ modal عبر ShowDialog().
    /// الكود المقترح يتزايد تلقائياً (CC-01 ... CC-05) حسب عدد المراكز الافتراضية الموجودة.
    /// </summary>
    public partial class frmAccountingAddCostCenter : Form
    {
        private readonly IServiceProvider _serviceProvider;

        // ═══════════════════════════════════════════════════════════
        // Mock data — الموظفون المرشحون لمسؤولية مركز التكلفة (بدون API / قاعدة بيانات)
        // ═══════════════════════════════════════════════════════════

        /// <summary>أسماء الموظفين المتاحين لاختيار المسؤول (وضع تجريبي).</summary>
        private static readonly string[] _employees =
        {
            "أحمد محمد",
            "سارة علي",
            "خالد حسن",
            "فاطمة عمر",
            "محمود إبراهيم",
        };

        /// <summary>
        /// عدد مراكز التكلفة الموجودة سلفاً في النظام (وضع تجريبي).
        /// الكود المقترح عند فتح النافذة = CC-05 (أي 4 مراكز موجودة + 1).
        /// يزداد العداد بعد كل حفظ ناجح ليقترح الكود التالي تلقائياً في الفتحات اللاحقة.
        /// </summary>
        private static int _existingCount = 4;

        public frmAccountingAddCostCenter(IServiceProvider serviceProvider)
        {
            // DI إلزامي لكل النماذج — يُستخدم عند ربط الواجهة بالخلفية الفعلية لاحقاً.
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            LoadLookups();

            ActiveControl = txtCostCenterName;
        }

        /// <summary>تعبئة قائمة المسؤولين وتوليد الكود المقترح من البيانات التجريبية.</summary>
        private void LoadLookups()
        {
            cmbManager.Items.Clear();
            cmbManager.Items.AddRange(_employees);

            // اختيار أول مسؤول تلقائياً.
            if (cmbManager.Items.Count > 0)
                cmbManager.SelectedIndex = 0;

            // الكود المقترح تلقائياً — للقراءة فقط (CC-01 .. CC-05).
            txtCode.Text = $"CC-{_existingCount + 1:00}";
        }

        // ═══════════════════════════════════════════════════════════
        // التحقق من صحة الإدخال
        // ═══════════════════════════════════════════════════════════

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtCostCenterName.Text))
            {
                errorProvider.SetError(txtCostCenterName, "اسم مركز التكلفة مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtCostCenterName, "");
            }

            if (!isValid)
            {
                MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        // ═══════════════════════════════════════════════════════════
        // الأزرار
        // ═══════════════════════════════════════════════════════════

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateForm()) return;

            string summary =
                $"اسم مركز التكلفة: {txtCostCenterName.Text.Trim()}\r\n" +
                $"الكود: {txtCode.Text}\r\n" +
                $"الموازنة: {nudBudget.Value:N2}\r\n" +
                $"المسؤول: {cmbManager.Text}\r\n" +
                $"نشط: {(chkIsActive.Checked ? "نعم" : "لا")}";

            // وضع تجريبي — لا يوجد اتصال بـ API بعد.
            // زيادة العداد ليقترح الكود التالي تلقائياً في المرة القادمة.
            _existingCount++;

            MessageBox.Show(
                $"تمت إضافة مركز التكلفة بنجاح (وضع تجريبي).\r\n\r\n{summary}",
                "إضافة مركز تكلفة", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}