using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    /// <summary>
    /// نافذة «تعديل حساب» — نموذج تجريبي (UI-first prototype) ببيانات داخل الذاكرة فقط.
    /// لا تتصل بأي API أو قاعدة بيانات — تُعرض كـ modal عبر ShowDialog().
    /// تُظهر الحقول معبأة مسبقاً بحساب تجريبي (الصندوق الرئيسي 1101-01) تمهيداً لربطها بالخلفية لاحقاً.
    /// </summary>
    public partial class frmAccountingEditAccount : Form
    {
        private readonly IServiceProvider _serviceProvider;

        // ═══════════════════════════════════════════════════════════
        // Mock data — الحسابات الأب (بدون API / قاعدة بيانات)
        // ═══════════════════════════════════════════════════════════

        /// <summary>حساب أب في شجرة دليل الحسابات (وضع تجريبي).</summary>
        private sealed class MockParentAccount
        {
            public string Code { get; set; } = "";
            public string Name { get; set; } = "";
            public string NatureName { get; set; } = "";
            public int Level { get; set; } = 1;

            public string Display => $"{Code} — {Name}";
        }

        private readonly List<MockParentAccount> _parents = new()
        {
            new MockParentAccount { Code = "1101", Name = "النقد", NatureName = "أصل", Level = 1 },
            new MockParentAccount { Code = "1102", Name = "البنوك", NatureName = "أصل", Level = 1 },
            new MockParentAccount { Code = "1301", Name = "المخزون", NatureName = "أصل", Level = 1 },
            new MockParentAccount { Code = "2101", Name = "الموردون", NatureName = "خصم", Level = 1 },
            new MockParentAccount { Code = "4101", Name = "المبيعات", NatureName = "إيراد", Level = 1 },
            new MockParentAccount { Code = "5101", Name = "المصروفات", NatureName = "مصروف", Level = 1 },
        };

        /// <summary>الطبيعة المتاحة للحساب — تتم مطابقة فهرس الحساب التجريبي عليها.</summary>
        private static readonly string[] _natures = { "أصل", "خصم", "حقوق ملكية", "إيراد", "مصروف" };

        public frmAccountingEditAccount(IServiceProvider serviceProvider)
        {
            // DI إلزامي لكل النماذج — يُستخدم عند ربط الواجهة بالخلفية الفعلية لاحقاً.
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            LoadLookups();
            _LoadSampleAccount();

            ActiveControl = txtAccountName;
        }

        /// <summary>تعبئة قوائم الحسابات الأب وطبيعة الحساب من البيانات التجريبية.</summary>
        private void LoadLookups()
        {
            cmbParent.Items.Clear();
            foreach (var parent in _parents)
                cmbParent.Items.Add(parent);
            cmbParent.DisplayMember = nameof(MockParentAccount.Display);

            cmbNature.Items.Clear();
            cmbNature.Items.AddRange(_natures);
        }

        /// <summary>
        /// تعبئة الحقول بحساب تجريبي جاهز للتعديل:
        /// الصندوق الرئيسي — الكود 1101-01 — أصل — رصيد افتتاحي 250,000.00.
        /// </summary>
        private void _LoadSampleAccount()
        {
            txtAccountName.Text = "الصندوق الرئيسي";
            lblAccountCode.Text = "1101-01";

            int parentIndex = _parents.FindIndex(p => p.Code == "1101");
            cmbParent.SelectedIndex = parentIndex >= 0 ? parentIndex : 0;

            int natureIndex = Array.IndexOf(_natures, "أصل");
            cmbNature.SelectedIndex = natureIndex >= 0 ? natureIndex : 0;

            chkAllowPosting.Checked = true;
            numOpeningBalance.Value = 250000m;
            txtNotes.Text = "الصندوق الرئيسي للخزينة النقدية في الفرع الرئيسي.";
        }

        private MockParentAccount? SelectedParent => cmbParent.SelectedItem as MockParentAccount;

        // ═══════════════════════════════════════════════════════════
        // التحقق من صحة الإدخال
        // ═══════════════════════════════════════════════════════════

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtAccountName.Text))
            {
                errorProvider.SetError(txtAccountName, "اسم الحساب مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtAccountName, "");
            }

            if (string.IsNullOrWhiteSpace(cmbNature.Text))
            {
                errorProvider.SetError(cmbNature, "يرجى تحديد طبيعة الحساب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(cmbNature, "");
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

            var parent = SelectedParent;

            string summary =
                $"اسم الحساب: {txtAccountName.Text.Trim()}\r\n" +
                $"الكود: {lblAccountCode.Text}\r\n" +
                $"الحساب الأب: {parent?.Display}\r\n" +
                $"طبيعة الحساب: {cmbNature.Text.Trim()}\r\n" +
                $"يسمح بالترحيل: {(chkAllowPosting.Checked ? "نعم" : "لا")}\r\n" +
                $"الرصيد الافتتاحي: {numOpeningBalance.Value:N2}";

            // وضع تجريبي — لا يوجد اتصال بـ API بعد.
            MessageBox.Show(
                $"تم حفظ تعديلات الحساب بنجاح (وضع تجريبي).\r\n\r\n{summary}",
                "تعديل حساب", MessageBoxButtons.OK, MessageBoxIcon.Information);

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