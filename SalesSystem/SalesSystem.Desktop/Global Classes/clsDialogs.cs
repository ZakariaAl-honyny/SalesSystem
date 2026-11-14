using System;
using System.Windows.Forms;

namespace SalesSystem.Desktop.GlobalClasses
{
    /// <summary>
    /// البيت الواحد لكل رسالة <see cref="MessageBox"/> عربية في النظام (<c>RULE-N169</c>):
    /// العناوين والأيقونات ثابتة، ولا تصل تفاصيل الاستثناء إلى المستخدم أبداً (<c>RULE-N28</c>/<c>RULE-N29</c>).
    /// </summary>
    public static class clsDialogs
    {
        // العناوين العربية الثابتة — تُستهلك أيضاً من clsInputFeedback (تلميحات الحقول) فلا تُكرَّر.
        internal const string InfoTitle = "معلومة";
        internal const string SuccessTitle = "تم بنجاح";
        internal const string WarningTitle = "تنبيه";
        internal const string ErrorTitle = "خطأ";
        internal const string ConfirmTitle = "تأكيد";

        /// <summary>رسالة معلومة للمستخدم.</summary>
        public static void Info(string message)
        {
            Show(message, InfoTitle, MessageBoxIcon.Information);
        }

        /// <summary>رسالة نجاح بعد إتمام عملية (حفظ · حذف · إعادة تنشيط).</summary>
        public static void Success(string message)
        {
            Show(message, SuccessTitle, MessageBoxIcon.Information);
        }

        /// <summary>رسالة تنبيه (بيانات ناقصة · تحقق من الإدخال).</summary>
        public static void Warning(string message)
        {
            Show(message, WarningTitle, MessageBoxIcon.Warning);
        }

        /// <summary>رسالة خطأ بنص عربي فقط — بلا تفاصيل تقنية.</summary>
        public static void Error(string message)
        {
            Show(message, ErrorTitle, MessageBoxIcon.Error);
        }

        /// <summary>
        /// يسجّل الاستثناء في Serilog ويعرض <paramref name="userMessage"/> العربي فقط — <c>ex.Message</c> لا يصل للمستخدم.
        /// </summary>
        public static void Error(Exception ex, string userMessage)
        {
            try
            {
                Serilog.Log.Error(ex, "تعذر تنفيذ العملية: {UserMessage}", userMessage);
            }
            catch (Exception)
            {
                // فشل التسجيل لا يمنع عرض الرسالة العربية.
            }

            Show(userMessage, ErrorTitle, MessageBoxIcon.Error);
        }

        /// <summary>تأكيد نعم/لا — الافتراضي «لا» كي لا يُنفَّذ إجراء خطير بضغطة Enter عابرة.</summary>
        public static bool Confirm(string message)
        {
            DialogResult result = MessageBox.Show(
                message ?? string.Empty,
                ConfirmTitle,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            return result == DialogResult.Yes;
        }

        /// <summary>العرض الوحيد لكل رسالة: عنوان ثابت + أيقونة صريحة + اتجاه عربي.</summary>
        private static void Show(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(
                message ?? string.Empty,
                title,
                MessageBoxButtons.OK,
                icon,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
    }
}
