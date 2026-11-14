using System;
using System.Windows.Forms;

namespace SalesSystem.Desktop.GlobalClasses
{
    /// <summary>
    /// البيت الواحد لكل رسالة <see cref="MessageBox"/> عربية في النظام (<c>RULE-N169</c>):
    /// العناوين والأيقونات ثابتة، ولا تصل تفاصيل الاستثناء إلى المستخدم أبداً (<c>RULE-N28</c>/<c>RULE-N29</c>).
    /// وهو الملف الوحيد في الحل كله المسموح له بمناداة <see cref="MessageBox.Show(string)"/> — كل الشاشات تستهلك أعضاءه.
    /// </summary>
    public static class clsDialogs
    {
        // العناوين العربية الثابتة — تُستهلك أيضاً من clsInputFeedback (تلميحات الحقول) فلا تُكرَّر.
        internal const string InfoTitle = "معلومة";
        internal const string SuccessTitle = "تم بنجاح";
        internal const string WarningTitle = "تنبيه";
        internal const string ErrorTitle = "خطأ";
        internal const string ConfirmTitle = "تأكيد";

        /// <summary>
        /// عنوان تأكيد التعديل — يُستهلك في سؤال «هل أنت متأكد من تعديل …؟» الذي افتراضه «نعم»
        /// (يُختلف عن <see cref="Confirm"/> الافتراضي «لا» لأنه لا يحمي من إجراء خطير).
        /// </summary>
        internal const string ConfirmEditTitle = "تأكيد التعديل";

        /// <summary>رسالة معلومة للمستخدم بعنوان «معلومة».</summary>
        public static void Info(string message)
        {
            Show(message, InfoTitle, MessageBoxIcon.Information);
        }

        /// <summary>رسالة معلومة بعنوان مخصص — حين لا يناسب العنوان الثابت سياق الشاشة.</summary>
        public static void Info(string message, string title)
        {
            Show(message, title, MessageBoxIcon.Information);
        }

        /// <summary>رسالة نجاح بعد إتمام عملية (حفظ · حذف · إعادة تنشيط) بعنوان «تم بنجاح».</summary>
        public static void Success(string message)
        {
            Show(message, SuccessTitle, MessageBoxIcon.Information);
        }

        /// <summary>رسالة نجاح بعنوان مخصص.</summary>
        public static void Success(string message, string title)
        {
            Show(message, title, MessageBoxIcon.Information);
        }

        /// <summary>رسالة تنبيه (بيانات ناقصة · تحقق من الإدخال) بعنوان «تنبيه».</summary>
        public static void Warning(string message)
        {
            Show(message, WarningTitle, MessageBoxIcon.Warning);
        }

        /// <summary>رسالة تنبيه بعنوان مخصص.</summary>
        public static void Warning(string message, string title)
        {
            Show(message, title, MessageBoxIcon.Warning);
        }

        /// <summary>رسالة خطأ بنص عربي فقط — بلا تفاصيل تقنية، بعنوان «خطأ».</summary>
        public static void Error(string message)
        {
            Show(message, ErrorTitle, MessageBoxIcon.Error);
        }

        /// <summary>رسالة خطأ بعنوان مخصص — بلا تفاصيل تقنية أيضاً.</summary>
        public static void Error(string message, string title)
        {
            Show(message, title, MessageBoxIcon.Error);
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

        /// <summary>تأكيد نعم/لا بعنوان «تأكيد» — الافتراضي «لا» كي لا يُنفَّذ إجراء خطير بضغطة Enter عابرة.</summary>
        public static bool Confirm(string message)
        {
            return Ask(message, ConfirmTitle, MessageBoxDefaultButton.Button2);
        }

        /// <summary>تأكيد نعم/لا بعنوان مخصص — الافتراضي يبقى «لا».</summary>
        public static bool Confirm(string message, string title)
        {
            return Ask(message, title, MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// تأكيد تعديل سجل («هل أنت متأكد من تعديل …؟») بعنوان «تأكيد التعديل» — الافتراضي هنا «نعم»
        /// لأن التعديل إجراء روتيني لا خطير، بخلاف <see cref="Confirm"/> الافتراضي «لا».
        /// </summary>
        public static bool ConfirmEdit(string message)
        {
            return Ask(message, ConfirmEditTitle, MessageBoxDefaultButton.Button1);
        }

        /// <summary>العرض الوحيد لكل رسالة إخبارية: عنوان + أيقونة صريحة + اتجاه عربي.</summary>
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

        /// <summary>العرض الوحيد لكل سؤال نعم/لا (تأكيد أو تأكيد تعديل): افتراضي الزر يُمرَّر صريحاً.</summary>
        private static bool Ask(string message, string title, MessageBoxDefaultButton defaultButton)
        {
            DialogResult result = MessageBox.Show(
                message ?? string.Empty,
                title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                defaultButton,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            return result == DialogResult.Yes;
        }
    }
}
