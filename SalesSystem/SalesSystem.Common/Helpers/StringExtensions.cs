// *********************************************************************
//  StringExtensions.cs
//  ملحق لتحسين التعامل مع النصوص في النظام
// *********************************************************************

namespace SalesSystem.Common.Helpers;

/// <summary>
/// ملحق للتعامل مع السلاسل النصية، يوفّر دوال مساعدة للتحقق والتحويل.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// للتحقق مما إذا كانت القيمة فارغة أو تتكون من مسافات بيضاء فقط.
    /// </summary>
    /// <param name="value">النص المراد فحصه.</param>
    /// <returns>true إذا كانت القيمة null أو فارغة أو تحتوي على مسافات بيضاء فقط.</returns>
    /// <example>
    /// <code>
    /// string? name = null;
    /// bool empty = name.IsNullOrWhiteSpace(); // true
    /// </code>
    /// </example>
    public static bool IsNullOrWhiteSpace(this string? value)
        => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// للتحقق مما إذا كانت القيمة تحتوي على نص غير فارغ.
    /// </summary>
    /// <param name="value">النص المراد فحصه.</param>
    /// <returns>true إذا كانت القيمة تحتوي على نص غير فارغ.</returns>
    /// <example>
    /// <code>
    /// string? name = "أحمد";
    /// bool has = name.HasValue(); // true
    /// </code>
    /// </example>
    public static bool HasValue(this string? value)
        => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// يقص النص إلى الطول المحدد مع إزالة الزيادة.
    /// </summary>
    /// <param name="value">النص المراد قصّه.</param>
    /// <param name="maxLength">أقصى طول مسموح به.</param>
    /// <returns>نص مقصوص إذا تجاوز الطول المسموح، أو النص الأصلي إذا كان ضمن الحدّ.</returns>
    /// <example>
    /// <code>
    /// string longText = "نص طويل جداً";
    /// string truncated = longText.Truncate(4); // "نص ط"
    /// </code>
    /// </example>
    public static string Truncate(this string? value, int maxLength)
        => value?.Length <= maxLength ? value : value?[..maxLength] ?? string.Empty;

    /// <summary>
    /// يحول النص الفارغ إلى null لتسهيل التخزين في قاعدة البيانات.
    /// </summary>
    /// <param name="value">النص المراد تحويله.</param>
    /// <returns>null إذا كان النص فارغاً، وإلا النص الأصلي.</returns>
    /// <example>
    /// <code>
    /// string? result = "   ".ToNullIfEmpty(); // null
    /// </code>
    /// </example>
    public static string? ToNullIfEmpty(this string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    /// <summary>
    /// يعيد قيمة افتراضية إذا كان النص فارغاً.
    /// </summary>
    /// <param name="value">النص المراد فحصه.</param>
    /// <param name="defaultValue">القيمة الافتراضية عند الفراغ.</param>
    /// <returns>القيمة الافتراضية إذا كان النص فارغاً، وإلا النص الأصلي.</returns>
    /// <example>
    /// <code>
    /// string? empty = null;
    /// string result = empty.DefaultIfEmpty("افتراضي"); // "افتراضي"
    /// </code>
    /// </example>
    public static string DefaultIfEmpty(this string? value, string defaultValue)
        => string.IsNullOrWhiteSpace(value) ? defaultValue : value;
}

