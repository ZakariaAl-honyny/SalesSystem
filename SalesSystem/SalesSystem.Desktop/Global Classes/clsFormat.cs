using System;
using System.Globalization;

namespace SalesSystem.Desktop.GlobalClasses
{
    /// <summary>
    /// تنسيق وقراءة التواريخ والأرقام والمبالغ — البيت الواحد لكل تنسيق يُعرض في الشاشات
    /// (<c>RULE-N169</c>): مبالغ <c>N2</c> · كميات <c>N3</c> · أعداد <c>N0</c> · كل الأرقام بثقافة ثابتة.
    /// </summary>
    public static class clsFormat
    {
        /// <summary>تاريخ قصير للعرض بالشكل <c>dd/MMM/yyyy</c>.</summary>
        public static string DateToShort(DateTime Dt1)
        {
            return Dt1.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>تاريخ قصير بالصيغة الرقمية dd/MM/yyyy (ثابت الثقافة).</summary>
        public static string DateToDayMonthYear(DateTime dt) => dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        /// <summary>تاريخ ووقت للعرض بالشكل <c>dd/MM/yyyy HH:mm</c>.</summary>
        public static string DateToLong(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        /// <summary>مبلغ مالي بمنزلتين عشريتين (<c>decimal(18,2)</c>).</summary>
        public static string Money(decimal value)
        {
            return value.ToString("N2", CultureInfo.InvariantCulture);
        }

        /// <summary>كمية بثلاث منازل عشرية (<c>decimal(18,3)</c>).</summary>
        public static string Quantity(decimal value)
        {
            return value.ToString("N3", CultureInfo.InvariantCulture);
        }

        /// <summary>عدد صحيح بلا منازل عشرية (عدد النتائج والإحصاءات).</summary>
        public static string Number(decimal value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        /// <summary>نسبة مئوية بمنزلتين عشريتين متبوعة بعلامة <c>%</c>.</summary>
        public static string Percent(decimal value)
        {
            return value.ToString("N2", CultureInfo.InvariantCulture) + "%";
        }

        /// <summary>يقرأ مبلغاً من نص المستخدم — يُعيد <c>false</c> للنص الفارغ أو غير الرقمي.</summary>
        public static bool TryParseMoney(string? text, out decimal value)
        {
            return TryParseCore(text, out value);
        }

        /// <summary>يقرأ كمية من نص المستخدم — يُعيد <c>false</c> للنص الفارغ أو غير الرقمي.</summary>
        public static bool TryParseQuantity(string? text, out decimal value)
        {
            return TryParseCore(text, out value);
        }

        /// <summary>القارئ الوحيد للأرقام: ثقافة ثابتة حتى لا تتغير النتيجة حسب إعدادات الجهاز.</summary>
        private static bool TryParseCore(string? text, out decimal value)
        {
            value = 0m;

            if (string.IsNullOrWhiteSpace(text)) return false;

            return decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }
    }
}
