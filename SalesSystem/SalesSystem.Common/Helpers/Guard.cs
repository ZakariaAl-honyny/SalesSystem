namespace SalesSystem.Common.Helpers;

/// <summary>
/// مجموعة من دوال التحقق (Guard clauses) لتوحيد عملية فحص المعاملات والبيانات.
/// توفر هذه الدوال أسلوبين:
///   1. Exception-style — للتحقق من أخطاء المبرمج أو البنية التحتية (تطرح استثناء).
///   2. Result-style — للتحقق من قواعد العمل ومدخلات المستخدم (ترجع Result).
/// </summary>
public static class Guard
{
    // ═══════════════════════════════════════════════════════════
    //  Exception-style (Fail-fast)
    //  تُستخدم للتحقق من أخطاء التطوير والبنية التحتية
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// يتحقق من أن القيمة ليست null.
    /// في حال كانت null يتم طرح ArgumentNullException.
    /// </summary>
    /// <param name="value">القيمة المراد فحصها.</param>
    /// <param name="argumentName">اسم المتغير (لرسالة الخطأ).</param>
    public static void AgainstNull(object? value, string argumentName)
    {
        if (value is null)
            throw new ArgumentNullException(argumentName, $"{argumentName} لا يمكن أن يكون فارغاً.");
    }

    /// <summary>
    /// يتحقق من أن النص ليس فارغاً أو يتكون من مسافات فقط.
    /// في حال كان فارغاً يتم طرح ArgumentException.
    /// </summary>
    /// <param name="value">النص المراد فحصه.</param>
    /// <param name="argumentName">اسم المتغير (لرسالة الخطأ).</param>
    public static void AgainstNullOrEmpty(string? value, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{argumentName} لا يمكن أن يكون فارغاً.", argumentName);
    }

    /// <summary>
    /// يتحقق من أن القيمة الرقمية ليست سالبة.
    /// في حال كانت سالبة يتم طرح ArgumentOutOfRangeException.
    /// </summary>
    /// <param name="value">القيمة الرقمية المراد فحصها.</param>
    /// <param name="argumentName">اسم المتغير (لرسالة الخطأ).</param>
    public static void AgainstNegative(decimal value, string argumentName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} لا يمكن أن يكون أقل من الصفر.");
    }

    /// <summary>
    /// يتحقق من أن القيمة الرقمية لا تساوي صفراً.
    /// في حال كانت صفراً يتم طرح ArgumentOutOfRangeException.
    /// </summary>
    /// <param name="value">القيمة الرقمية المراد فحصها.</param>
    /// <param name="argumentName">اسم المتغير (لرسالة الخطأ).</param>
    public static void AgainstZero(decimal value, string argumentName)
    {
        if (value == 0)
            throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} لا يمكن أن يكون صفراً.");
    }

    /// <summary>
    /// يتحقق من أن القيمة الرقمية موجبة (أكبر من صفر).
    /// في حال كانت ≤ صفر يتم طرح ArgumentOutOfRangeException.
    /// </summary>
    /// <param name="value">القيمة الرقمية المراد فحصها.</param>
    /// <param name="argumentName">اسم المتغير (لرسالة الخطأ).</param>
    public static void AgainstNegativeOrZero(decimal value, string argumentName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} يجب أن يكون أكبر من الصفر.");
    }
}
