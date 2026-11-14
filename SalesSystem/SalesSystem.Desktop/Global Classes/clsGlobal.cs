using System;
using System.Windows.Forms;

namespace SalesSystem.Desktop.GlobalClasses
{
    /// <summary>
    /// أدوات عامة على مستوى النماذج — البيت الواحد لفتح أي نموذج من داخل نموذج آخر
    /// (<c>RULE-N132</c>): لا <c>ShowDialog</c> مخصّص، ولا <c>new frmXxx()</c>، ولا تكرار للنسخة المفتوحة.
    /// </summary>
    internal static class clsGlobal
    {
        /// <summary>النافذة الأب (MDI) التي تُستضاف فيها النماذج الفرعية — يضبطها <c>frmMain</c>.</summary>
        public static Form? frmMdiMain = null;

        /// <summary>
        /// يفتح النموذج إن لم يكن مفتوحاً، أو يُنشّط نسخته المفتوحة بدل إنشاء نسخة ثانية.
        /// لا يرمي استثناءً أبداً: أي فشل يُسجَّل ويُترك للمستخدم بلا رسالة إزعاج.
        /// </summary>
        /// <param name="form">النموذج المطلوب فتحه (يُحلّ من حاوية DI عبر <c>GetRequiredService</c>).</param>
        public static void _OpenForm(Form form)
        {
            if (form is null || form.IsDisposed) return;

            try
            {
                Form? existing = FindOpen(form.Name);
                if (existing is not null && !ReferenceEquals(existing, form))
                {
                    ActivateExisting(existing);
                    return;
                }

                AttachToMdiHost(form);
                form.Show();
            }
            catch (Exception ex)
            {
                // الفتح لا يُفشل الشاشة المستدعية — تسجيل صامت بلا رسالة للمستخدم.
                Serilog.Log.Error(ex, "تعذر فتح النموذج {Form}", form.Name);
            }
        }

        /// <summary>يبحث عن نسخة مفتوحة بنفس اسم النموذج — مع تجاهل شاشة الدخول والشاشة الرئيسية.</summary>
        private static Form? FindOpen(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            // عند البحث عن شاشة هيكلية (frmLogin/frmMain) نطابق اسمها، وإلا تُستثنى من المطابقة كي لا تُعدّ نسخاً مكرّرة.
            bool searchShellForm = IsShellForm(name);

            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is null || openForm.IsDisposed) continue;
                if (!searchShellForm && IsShellForm(openForm.Name)) continue;
                if (string.Equals(openForm.Name, name, StringComparison.Ordinal)) return openForm;
            }

            return null;
        }

        /// <summary>يُعيد النافذة المفتوحة إلى المقدمة بدل فتح نسخة جديدة منها.</summary>
        private static void ActivateExisting(Form openForm)
        {
            if (openForm.MdiParent is { IsDisposed: false } parent &&
                parent.WindowState == FormWindowState.Minimized)
            {
                parent.WindowState = FormWindowState.Normal;
            }

            openForm.WindowState = FormWindowState.Normal;
            openForm.Activate();
            openForm.BringToFront();
        }

        /// <summary>يُسند النموذج إلى النافذة الأب ويأخذ أيقونتها — فقط إن كانت النافذة الأب صالحة.</summary>
        private static void AttachToMdiHost(Form form)
        {
            Form? host = frmMdiMain;

            if (host is not { IsDisposed: false } || IsShellForm(form.Name)) return;

            try
            {
                if (host.Icon is not null) form.Icon = host.Icon;
                form.MdiParent = host;
            }
            catch (Exception ex)
            {
                // إسناد MDI غير حرج — النموذج يُفتح مستقلاً إن فشل الإسناد.
                Serilog.Log.Error(ex, "تعذر إسناد النموذج {Form} إلى النافذة الرئيسية", form.Name);
            }
        }

        /// <summary>شاشات هيكلية لا تُعدّ نسخاً مكرّرة ولا تُستضاف كأبناء MDI.</summary>
        private static bool IsShellForm(string? name)
            => string.Equals(name, "frmLogin", StringComparison.Ordinal)
            || string.Equals(name, "frmMain", StringComparison.Ordinal);
    }
}
