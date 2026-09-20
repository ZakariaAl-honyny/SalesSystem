using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    /// <summary>
    /// نافذة «إنشاء حساب ذكي» — نموذج تجريبي (UI-first prototype) ببيانات داخل الذاكرة فقط.
    /// تعرض قوالب جاهزة (صندوق / بنك / محفظة / عميل / مورد) وتقترح الكود والحساب الأب تلقائياً.
    /// لا تتصل بأي API أو قاعدة بيانات — تُعرض كـ modal عبر ShowDialog().
    /// </summary>
    public partial class frmSmartAccountCreation : Form
    {
        private readonly IServiceProvider _serviceProvider;

        // ═══════════════════════════════════════════════════════════
        // قالب الحساب الذكي — Mock data (بدون API / قاعدة بيانات)
        // ═══════════════════════════════════════════════════════════
        private sealed class AccountTemplate
        {
            public string Name { get; set; } = "";
            public string Nature { get; set; } = "";          // أصل / خصم
            public string ParentCode { get; set; } = "";      // كود الحساب الأب
            public string ParentName { get; set; } = "";      // اسم الحساب الأب
            public string SuggestedCode { get; set; } = "";   // الكود المقترح
            public string Reason { get; set; } = "";          // شرح سبب اختيار القالب
        }

        private readonly List<(RadioButton Radio, AccountTemplate Template)> _templates = new();

        public frmSmartAccountCreation(IServiceProvider serviceProvider)
        {
            // DI إلزامي لكل النماذج — يُستخدم عند ربط الواجهة بالخلفية الفعلية لاحقاً.
            _serviceProvider = serviceProvider;
            InitializeComponent();
            BuildTemplates();
            RefreshTemplateInfo();
            ActiveControl = txtName;
        }

        /// <summary>القوالب الخمسة المدعومة مع الحساب الأب والكود المقترح والشرح.</summary>
        private void BuildTemplates()
        {
            _templates.Add((rbCashbox, new AccountTemplate
            {
                Name = "صندوق",
                Nature = "أصل",
                ParentCode = "1101",
                ParentName = "النقد وما في حكمه",
                SuggestedCode = "1101-05",
                Reason = "الصندوق: أصل يُستخدم لتتبع النقد المتوفر لدى المنشأة."
            }));

            _templates.Add((rbBank, new AccountTemplate
            {
                Name = "بنك",
                Nature = "أصل",
                ParentCode = "1102",
                ParentName = "الحسابات البنكية",
                SuggestedCode = "1102-05",
                Reason = "البنك: أصل يُستخدم لتتبع الأرصدة النقدية المحفوظة لدى البنوك."
            }));

            _templates.Add((rbWallet, new AccountTemplate
            {
                Name = "محفظة",
                Nature = "أصل",
                ParentCode = "1103",
                ParentName = "المحافظ الإلكترونية",
                SuggestedCode = "1103-05",
                Reason = "المحفظة: أصل يُستخدم لتتبع الأرصدة الرقمية في تطبيقات الدفع الإلكتروني."
            }));

            _templates.Add((rbCustomer, new AccountTemplate
            {
                Name = "عميل",
                Nature = "أصل",
                ParentCode = "1104",
                ParentName = "ذمم عملاء",
                SuggestedCode = "1104-05",
                Reason = "العميل: أصل (ذمم مدينة) يُستخدم لتتبع مستحقات العملاء لدى المنشأة."
            }));

            _templates.Add((rbSupplier, new AccountTemplate
            {
                Name = "مورد",
                Nature = "خصم",
                ParentCode = "2101",
                ParentName = "ذمم موردين",
                SuggestedCode = "2101-05",
                Reason = "المورد: خصم (ذمم دائنة) يُستخدم لتتبع مستحقات الموردين على المنشأة."
            }));

            // اختيار قالب → تحديث معلومات القالب فوراً.
            foreach (var (radio, _) in _templates)
                radio.CheckedChanged += (s, e) => RefreshTemplateInfo();
        }

        /// <summary>تحديث لوحة المعلومات (الطبيعة / الأب / الكود المقترح / الشرح) حسب القالب المختار.</summary>
        private void RefreshTemplateInfo()
        {
            foreach (var (radio, template) in _templates)
            {
                if (!radio.Checked) continue;

                lblNatureValue.Text = template.Nature;
                lblParentValue.Text = $"{template.ParentCode} — {template.ParentName}";
                lblCodeValue.Text = template.SuggestedCode;
                lblReason.Text = $"الشرح: {template.Reason}";
                return;
            }
        }

        private AccountTemplate GetSelectedTemplate()
        {
            foreach (var (radio, template) in _templates)
                if (radio.Checked)
                    return template;
            return _templates[0].Template;
        }

        // ═══════════════════════════════════════════════════════════
        // التحقق من صحة الإدخال
        // ═══════════════════════════════════════════════════════════
        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider.SetError(txtName, "اسم الحساب مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtName, "");
            }

            if (!isValid)
            {
                MessageBox.Show("يرجى إدخال اسم الحساب قبل الإنشاء.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        // ═══════════════════════════════════════════════════════════
        // الأزرار
        // ═══════════════════════════════════════════════════════════
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!_ValidateForm()) return;

            var template = GetSelectedTemplate();
            string accountName = txtName.Text.Trim();

            // وضع تجريبي — لا يوجد اتصال بـ API بعد.
            MessageBox.Show(
                $"تم إنشاء الحساب «{accountName}» بنجاح (وضع تجريبي).\r\n" +
                $"القالب: {template.Name}\r\n" +
                $"الحساب الأب: {template.ParentCode} — {template.ParentName}\r\n" +
                $"الكود المقترح: {template.SuggestedCode}",
                "إنشاء حساب ذكي", MessageBoxButtons.OK, MessageBoxIcon.Information);

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