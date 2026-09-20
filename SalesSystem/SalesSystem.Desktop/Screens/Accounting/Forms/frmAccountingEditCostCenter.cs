using System;
using System.Windows.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    /// <summary>
    /// نافذة «تعديل مركز تكلفة» — نموذج تجريبي (UI-first prototype) ببيانات داخل الذاكرة فقط.
    /// لا تتصل بأي API أو قاعدة بيانات — تُعرض كـ modal عبر ShowDialog().
    /// البيانات معبأة مسبقاً من مركز تكلفة تجريبي واحد: CC-01.
    /// </summary>
    public partial class frmAccountingEditCostCenter : Form
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

        /// <summary>الكود الحالي لمركز التكلفة الجاري تعديله (ثابت في وضع التجربة).</summary>
        private const string _editingCode = "CC-01";

        /// <summary>الاسم الحالي لمركز التكلفة الجاري تعديله (يُعرض كقيمة افتراضية في الحقل).</summary>
        private const string _editingName = "مركز تكلفة المبيعات";

        public frmAccountingEditCostCenter(IServiceProvider serviceProvider)
        {
            // DI إلزامي لكل النماذج — يُستخدم عند ربط الواجهة بالخلفية الفعلية لاحقاً.
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            LoadLookups();

            ActiveControl = txtCostCenterName;
        }

        /// <summary>تعبئة قائمة المسؤولين وتحميل بيانات مركز التكلفة التجريبي (CC-01).</summary>
        private void LoadLookups()
        {
            cmbManager.Items.Clear();
            cmbManager.Items.AddRange(_employees);

            // تعبئة الحقول بقيم مركز التكلفة الجاري تعديله (وضع تجريبي).
            txtCostCenterName.Text = _editingName;
            txtCode.Text = _editingCode;                  // للقراءة فقط — لا يمكن تغييره.
            nudBudget.Value = 120000m;                    // الموازنة: 120,000.00
            chkIsActive.Checked = true;                   // نشط

            // اختيار المسؤول الحالي «أحمد محمد» من القائمة.
            int managerIndex = cmbManager.FindStringExact("أحمد محمد");
            cmbManager.SelectedIndex = managerIndex >= 0 ? managerIndex : 0;
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
            MessageBox.Show(
                $"تم تعديل مركز التكلفة بنجاح (وضع تجريبي).\r\n\r\n{summary}",
                "تعديل مركز تكلفة", MessageBoxButtons.OK, MessageBoxIcon.Information);

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