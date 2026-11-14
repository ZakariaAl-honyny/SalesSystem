using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SalesSystem.Desktop.GlobalClasses
{
    /// <summary>
    /// التحقق النصي المشترك — البيت الواحد لكل قواعد تحقق تستخدمها الشاشات (<c>RULE-N169</c>).
    /// كل الدوال آمنة مع <c>null</c> ولا ترمي استثناءً.
    /// </summary>
    public static class clsValidation
    {
        /// <summary>نمط البريد الإلكتروني المعتمد في النظام.</summary>
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$",
            RegexOptions.Compiled);

        /// <summary>هاتف: 7 إلى 15 رقماً مع علامة + اختيارية في البداية.</summary>
        private static readonly Regex PhoneRegex = new(@"^\+?\d{7,15}$", RegexOptions.Compiled);

        /// <summary>أرقام فقط بلا إشارة أو فاصل.</summary>
        private static readonly Regex DigitsRegex = new(@"^\d+$", RegexOptions.Compiled);

        /// <summary>يتحقق من صحة صيغة البريد الإلكتروني — <c>false</c> للنص الفارغ.</summary>
        public static bool ValidateEmail(string? emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress)) return false;

            return EmailRegex.IsMatch(emailAddress.Trim());
        }

        /// <summary>هل النص غير فارغ (بعد تجاهل المسافات)؟ — للحقول الإلزامية.</summary>
        public static bool IsRequired(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>هل النص أرقام صحيحة فقط (بلا إشارة أو نقطة عشرية)؟</summary>
        public static bool IsDigits(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            return DigitsRegex.IsMatch(value.Trim());
        }

        /// <summary>هل النص رقم هاتف صالح (7 إلى 15 رقماً مع <c>+</c> اختيارية)؟</summary>
        public static bool IsPhone(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            return PhoneRegex.IsMatch(value.Trim());
        }

        /// <summary>هل النص قيمة عشرية موجبة (أكبر من صفر)؟ — للمبالغ والكميات.</summary>
        public static bool IsPositiveDecimal(string? value)
        {
            return TryParseDecimal(value, out decimal parsed) && parsed > 0m;
        }

        /// <summary>يقرأ قيمة عشرية من النص بثقافة ثابتة — <c>false</c> للنص الفارغ أو غير الرقمي.</summary>
        public static bool TryParseDecimal(string? value, out decimal result)
            => clsFormat.TryParseMoney(value, out result);
    }
}
