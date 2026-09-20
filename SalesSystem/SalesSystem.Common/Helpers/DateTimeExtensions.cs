// *********************************************************************
//  DateTimeExtensions.cs
//  ظ…ظ„ط­ظ‚ ظ„ظ„طھط¹ط§ظ…ظ„ ظ…ط¹ ط§ظ„طھظˆط§ط±ظٹط® ط­ط³ط¨ ط§ظ„طھظ‚ظˆظٹظ… ط§ظ„ط¹ط±ط¨ظٹ (ط§ظ„ط³ط¨طھ ط£ظˆظ„ ط§ظ„ط£ط³ط¨ظˆط¹)
// *********************************************************************

namespace SalesSystem.Common.Helpers;

/// <summary>
/// ظ…ظ„ط­ظ‚ ظ„ط¯ظˆط§ظ„ ط§ظ„طھط§ط±ظٹط® ظˆط§ظ„ظˆظ‚طھطŒ ظ…طھظˆط§ظپظ‚ ظ…ط¹ ط§ظ„طھظ‚ظˆظٹظ… ط§ظ„ط¹ط±ط¨ظٹ ط­ظٹط«
/// ظٹط¨ط¯ط£ ط§ظ„ط£ط³ط¨ظˆط¹ ظٹظˆظ… ط§ظ„ط³ط¨طھ ظˆظٹظ†طھظ‡ظٹ ظٹظˆظ… ط§ظ„ط¬ظ…ط¹ط©.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// ظٹط­ط³ط¨ طھط§ط±ظٹط® ط£ظˆظ„ ظٹظˆظ… ظپظٹ ط§ظ„ط£ط³ط¨ظˆط¹ ط§ظ„ط°ظٹ ظٹظ‚ط¹ ظپظٹظ‡ ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط­ط¯ط¯.
    /// </summary>
    /// <param name="date">ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط±ط¬ط¹ظٹ.</param>
    /// <param name="startOfWeek">ط£ظˆظ„ ط£ظٹط§ظ… ط§ظ„ط£ط³ط¨ظˆط¹ (ط§ظ„ط³ط¨طھ ط§ظپطھط±ط§ط¶ظٹط§ظ‹).</param>
    /// <returns>طھط§ط±ظٹط® ط£ظˆظ„ ظٹظˆظ… ظپظٹ ط§ظ„ط£ط³ط¨ظˆط¹.</returns>
    /// <example>
    /// <code>
    /// var wednesday = new DateTime(2026, 7, 22); // ط§ظ„ط£ط±ط¨ط¹ط§ط،
    /// var start = wednesday.StartOfWeek(); // ط§ظ„ط³ط¨طھ 2026-07-18
    /// </code>
    /// </example>
    public static DateTime StartOfWeek(this DateTime date, DayOfWeek startOfWeek = DayOfWeek.Saturday)
    {
        int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
        return date.AddDays(-diff).Date;
    }

    /// <summary>
    /// ظٹط­ط³ط¨ طھط§ط±ظٹط® ط¢ط®ط± ظٹظˆظ… ظپظٹ ط§ظ„ط£ط³ط¨ظˆط¹ ط§ظ„ط°ظٹ ظٹظ‚ط¹ ظپظٹظ‡ ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط­ط¯ط¯.
    /// </summary>
    /// <param name="date">ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط±ط¬ط¹ظٹ.</param>
    /// <param name="endOfWeek">ط¢ط®ط± ط£ظٹط§ظ… ط§ظ„ط£ط³ط¨ظˆط¹ (ط§ظ„ط¬ظ…ط¹ط© ط§ظپطھط±ط§ط¶ظٹط§ظ‹).</param>
    /// <returns>طھط§ط±ظٹط® ط¢ط®ط± ظٹظˆظ… ظپظٹ ط§ظ„ط£ط³ط¨ظˆط¹.</returns>
    /// <example>
    /// <code>
    /// var wednesday = new DateTime(2026, 7, 22); // ط§ظ„ط£ط±ط¨ط¹ط§ط،
    /// var end = wednesday.EndOfWeek(); // ط§ظ„ط¬ظ…ط¹ط© 2026-07-24
    /// </code>
    /// </example>
    public static DateTime EndOfWeek(this DateTime date, DayOfWeek endOfWeek = DayOfWeek.Friday)
    {
        var start = date.StartOfWeek();
        return start.AddDays(6).Date;
    }

    /// <summary>
    /// ظٹط­ط³ط¨ طھط§ط±ظٹط® ط£ظˆظ„ ظٹظˆظ… ظپظٹ ط§ظ„ط´ظ‡ط± ط§ظ„ط°ظٹ ظٹظ‚ط¹ ظپظٹظ‡ ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط­ط¯ط¯.
    /// </summary>
    /// <param name="date">ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط±ط¬ط¹ظٹ.</param>
    /// <returns>طھط§ط±ظٹط® ط£ظˆظ„ ظٹظˆظ… ظ…ظ† ط§ظ„ط´ظ‡ط± ظ…ط¹ ط¶ط¨ط· ط§ظ„ظˆظ‚طھ ط¥ظ„ظ‰ 00:00:00.</returns>
    /// <example>
    /// <code>
    /// var midMonth = new DateTime(2026, 7, 15);
    /// var first = midMonth.StartOfMonth(); // 2026-07-01
    /// </code>
    /// </example>
    public static DateTime StartOfMonth(this DateTime date)
        => new(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Local);

    /// <summary>
    /// ظٹط­ط³ط¨ طھط§ط±ظٹط® ط¢ط®ط± ظٹظˆظ… ظپظٹ ط§ظ„ط´ظ‡ط± ط§ظ„ط°ظٹ ظٹظ‚ط¹ ظپظٹظ‡ ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط­ط¯ط¯.
    /// </summary>
    /// <param name="date">ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط±ط¬ط¹ظٹ.</param>
    /// <returns>طھط§ط±ظٹط® ط¢ط®ط± ظٹظˆظ… ظ…ظ† ط§ظ„ط´ظ‡ط±.</returns>
    /// <example>
    /// <code>
    /// var midMonth = new DateTime(2026, 7, 15);
    /// var last = midMonth.EndOfMonth(); // 2026-07-31
    /// </code>
    /// </example>
    public static DateTime EndOfMonth(this DateTime date)
        => date.StartOfMonth().AddMonths(1).AddDays(-1);

    /// <summary>
    /// ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ…ط§ ط¥ط°ط§ ظƒط§ظ† ط§ظ„طھط§ط±ظٹط® ظٹظ‚ط¹ ظپظٹ ط¹ط·ظ„ط© ظ†ظ‡ط§ظٹط© ط§ظ„ط£ط³ط¨ظˆط¹ (ط§ظ„ط¬ظ…ط¹ط© ط£ظˆ ط§ظ„ط³ط¨طھ).
    /// </summary>
    /// <param name="date">ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡.</param>
    /// <returns>true ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظٹظˆظ… ط¬ظ…ط¹ط© ط£ظˆ ط³ط¨طھ.</returns>
    /// <example>
    /// <code>
    /// var friday = new DateTime(2026, 7, 24);
    /// bool isWeekend = friday.IsWeekend(); // true
    /// </code>
    /// </example>
    public static bool IsWeekend(this DateTime date)
        => date.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday;

    /// <summary>
    /// ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ…ط§ ط¥ط°ط§ ظƒط§ظ† ط§ظ„طھط§ط±ظٹط® ظٹظˆط§ظپظ‚ ظٹظˆظ… ط§ظ„ط¬ظ…ط¹ط©.
    /// </summary>
    /// <param name="date">ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡.</param>
    /// <returns>true ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظٹظˆظ… ط¬ظ…ط¹ط©.</returns>
    public static bool IsFriday(this DateTime date)
        => date.DayOfWeek == DayOfWeek.Friday;

    /// <summary>
    /// ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ…ط§ ط¥ط°ط§ ظƒط§ظ† ط§ظ„طھط§ط±ظٹط® ظٹظˆط§ظپظ‚ ظٹظˆظ… ط§ظ„ط³ط¨طھ.
    /// </summary>
    /// <param name="date">ط§ظ„طھط§ط±ظٹط® ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡.</param>
    /// <returns>true ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظٹظˆظ… ط³ط¨طھ.</returns>
    public static bool IsSaturday(this DateTime date)
        => date.DayOfWeek == DayOfWeek.Saturday;
}

