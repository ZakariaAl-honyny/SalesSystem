// *********************************************************************
//  DateTimeExtensions.cs
//  ملحق للتعامل مع التواريخ حسب التقويم العربي (السبت أول الأسبوع)
// *********************************************************************

namespace SalesSystem.Common.Helpers;

/// <summary>
/// ملحق لدوال التاريخ والوقت، متوافق مع التقويم العربي حيث
/// يبدأ الأسبوع يوم السبت وينتهي يوم الجمعة.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// يحسب تاريخ أول يوم في الأسبوع الذي يقع فيه التاريخ المحدد.
    /// </summary>
    /// <param name="date">التاريخ المرجعي.</param>
    /// <param name="startOfWeek">أول أيام الأسبوع (السبت افتراضياً).</param>
    /// <returns>تاريخ أول يوم في الأسبوع.</returns>
    /// <example>
    /// <code>
    /// var wednesday = new DateTime(2026, 7, 22); // الأربعاء
    /// var start = wednesday.StartOfWeek(); // السبت 2026-07-18
    /// </code>
    /// </example>
    public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Saturday)
    {
        int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
        return date.AddDays(-diff).Date;
    }

    /// <summary>
    /// يحسب تاريخ آخر يوم في الأسبوع الذي يقع فيه التاريخ المحدد.
    /// </summary>
    /// <param name="date">التاريخ المرجعي.</param>
    /// <param name="endOfWeek">آخر أيام الأسبوع (الجمعة افتراضياً).</param>
    /// <returns>تاريخ آخر يوم في الأسبوع.</returns>
    /// <example>
    /// <code>
    /// var wednesday = new DateTime(2026, 7, 22); // الأربعاء
    /// var end = wednesday.EndOfWeek(); // الجمعة 2026-07-24
    /// </code>
    /// </example>
    public static DateTime EndOfWeek(this DateTime date, DayOfWeek endOfWeek = DayOfWeek.Friday)
    {
        var start = date.StartOfWeek();
        return start.AddDays(6).Date;
    }

    /// <summary>
    /// يحسب تاريخ أول يوم في الشهر الذي يقع فيه التاريخ المحدد.
    /// </summary>
    /// <param name="date">التاريخ المرجعي.</param>
    /// <returns>تاريخ أول يوم من الشهر مع ضبط الوقت إلى 00:00:00.</returns>
    /// <example>
    /// <code>
    /// var midMonth = new DateTime(2026, 7, 15);
    /// var first = midMonth.StartOfMonth(); // 2026-07-01
    /// </code>
    /// </example>
    public static DateTime StartOfMonth(this DateTime date)
        => new(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Local);

    /// <summary>
    /// يحسب تاريخ آخر يوم في الشهر الذي يقع فيه التاريخ المحدد.
    /// </summary>
    /// <param name="date">التاريخ المرجعي.</param>
    /// <returns>تاريخ آخر يوم من الشهر.</returns>
    /// <example>
    /// <code>
    /// var midMonth = new DateTime(2026, 7, 15);
    /// var last = midMonth.EndOfMonth(); // 2026-07-31
    /// </code>
    /// </example>
    public static DateTime EndOfMonth(this DateTime date)
        => date.StartOfMonth().AddMonths(1).AddDays(-1);

    /// <summary>
    /// للتحقق مما إذا كان التاريخ يقع في عطلة نهاية الأسبوع (الجمعة أو السبت).
    /// </summary>
    /// <param name="date">التاريخ المراد فحصه.</param>
    /// <returns>true إذا كان اليوم جمعة أو سبت.</returns>
    /// <example>
    /// <code>
    /// var friday = new DateTime(2026, 7, 24);
    /// bool isWeekend = friday.IsWeekend(); // true
    /// </code>
    /// </example>
    public static bool IsWeekend(this DateTime date)
        => date.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday;

    /// <summary>
    /// للتحقق مما إذا كان التاريخ يوافق يوم الجمعة.
    /// </summary>
    /// <param name="date">التاريخ المراد فحصه.</param>
    /// <returns>true إذا كان اليوم جمعة.</returns>
    public static bool IsFriday(this DateTime date)
        => date.DayOfWeek == DayOfWeek.Friday;

    /// <summary>
    /// للتحقق مما إذا كان التاريخ يوافق يوم السبت.
    /// </summary>
    /// <param name="date">التاريخ المراد فحصه.</param>
    /// <returns>true إذا كان اليوم سبت.</returns>
    public static bool IsSaturday(this DateTime date)
        => date.DayOfWeek == DayOfWeek.Saturday;
}

