using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    /// <summary>
    /// نافذة «إضافة حساب» — نموذج تجريبي (UI-first prototype) ببيانات داخل الذاكرة فقط.
    /// لا تتصل بأي API أو قاعدة بيانات — تُعرض كـ modal عبر ShowDialog().
    /// تُظهر سلوك اقتراح الكود تلقائياً من الحساب الأب، واقتراح طبيعة الحساب مع إمكانية تعديلها يدوياً.
    /// </summary>
    public partial class frmAccountingAddAccount : Form
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

        /// <summary>
        /// عدّاد الأبناء الافتراضي لكل أب — لتوليد اللاحقة التالية في الكود المقترح.
        /// مثال: الأب 1101 لديه 5 أبناء فيحصل على اللاحقة 06.
        /// </summary>
        private static readonly Dictionary<string, int> _existingChildrenCount = new()
        {
            ["1101"] = 5,   // النقد      → 1101-06
            ["1102"] = 5,   // البنوك     → 1102-06
            ["1301"] = 3,   // المخزون    → 1301-04
            ["2101"] = 5,   // الموردون   → 2101-06
            ["4101"] = 4,   // المبيعات   → 4101-05
            ["5101"] = 2,   // المصروفات  → 5101-03
        };

        private static readonly string[] _natures = { "أصل", "خصم", "حقوق ملكية", "إيراد", "مصروف" };
        private static readonly string[] _levels = { "المستوى 1", "المستوى 2", "المستوى 3", "المستوى 4" };

        public frmAccountingAddAccount(IServiceProvider serviceProvider)
        {
            // DI إلزامي لكل النماذج — يُستخدم عند ربط الواجهة بالخلفية الفعلية لاحقاً.
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            LoadLookups();

            // اختيار أول حساب أب تلقائياً → يفعّل اقتراح الطبيعة والمستوى والكود.
            if (cmbParent.Items.Count > 0)
                cmbParent.SelectedIndex = 0;

            ActiveControl = txtAccountName;
        }

        /// <summary>تعبئة قوائم الحسابات الأب والطبيعة والمستوى من البيانات التجريبية.</summary>
        private void LoadLookups()
        {
            cmbParent.Items.Clear();
            foreach (var parent in _parents)
                cmbParent.Items.Add(parent);
            cmbParent.DisplayMember = nameof(MockParentAccount.Display);

            cmbNature.Items.Clear();
            cmbNature.Items.AddRange(_natures);

            cmbLevel.Items.Clear();
            cmbLevel.Items.AddRange(_levels);
        }

        private MockParentAccount? SelectedParent => cmbParent.SelectedItem as MockParentAccount;

        // ═══════════════════════════════════════════════════════════
        // الاقتراح التلقائي عند تغيير الحساب الأب
        // ═══════════════════════════════════════════════════════════

        private void cmbParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            var parent = SelectedParent;
            if (parent == null) return;

            // اقتراح طبيعة الحساب من الأب (قابلة للتعديل يدوياً).
            int natureIndex = Array.IndexOf(_natures, parent.NatureName);
            if (natureIndex >= 0)
                cmbNature.SelectedIndex = natureIndex;

            // اقتراح المستوى = مستوى الأب + 1 (مستوى الأبناء).
            int childLevel = Math.Clamp(parent.Level + 1, 1, 4);
            cmbLevel.SelectedIndex = childLevel - 1;

            // توليد الكود المقترح من الأب + اللاحقة التالية.
            UpdateSuggestedCode();
        }

        /// <summary>توليد الكود المقترح تلقائياً: كود الأب + «-» + اللاحقة التالية (مثال: 1101-06).</summary>
        private void UpdateSuggestedCode()
        {
            var parent = SelectedParent;
            if (parent == null)
            {
                lblSuggestedCode.Text = "—";
                return;
            }

            int count = _existingChildrenCount.TryGetValue(parent.Code, out int children) ? children : 0;
            lblSuggestedCode.Text = $"{parent.Code}-{(count + 1):00}";
        }

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
                $"الحساب الأب: {parent?.Display}\r\n" +
                $"طبيعة الحساب: {cmbNature.Text.Trim()}\r\n" +
                $"المستوى: {cmbLevel.Text}\r\n" +
                $"يسمح بالترحيل: {(chkAllowPosting.Checked ? "نعم" : "لا")}\r\n" +
                $"الكود المقترح: {lblSuggestedCode.Text}";

            // وضع تجريبي — لا يوجد اتصال بـ API بعد.
            MessageBox.Show(
                $"تمت إضافة الحساب بنجاح (وضع تجريبي).\r\n\r\n{summary}",
                "إضافة حساب", MessageBoxButtons.OK, MessageBoxIcon.Information);

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