// *********************************************************************
//  StringExtensions.cs
//  ظ…ظ„ط­ظ‚ ظ„طھط­ط³ظٹظ† ط§ظ„طھط¹ط§ظ…ظ„ ظ…ط¹ ط§ظ„ظ†طµظˆطµ ظپظٹ ط§ظ„ظ†ط¸ط§ظ…
// *********************************************************************

namespace SalesSystem.Common.Helpers;

/// <summary>
/// ظ…ظ„ط­ظ‚ ظ„ظ„طھط¹ط§ظ…ظ„ ظ…ط¹ ط§ظ„ط³ظ„ط§ط³ظ„ ط§ظ„ظ†طµظٹط©طŒ ظٹظˆظپظ‘ط± ط¯ظˆط§ظ„ ظ…ط³ط§ط¹ط¯ط© ظ„ظ„طھط­ظ‚ظ‚ ظˆط§ظ„طھط­ظˆظٹظ„.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ…ط§ ط¥ط°ط§ ظƒط§ظ†طھ ط§ظ„ظ‚ظٹظ…ط© ظپط§ط±ط؛ط© ط£ظˆ طھطھظƒظˆظ† ظ…ظ† ظ…ط³ط§ظپط§طھ ط¨ظٹط¶ط§ط، ظپظ‚ط·.
    /// </summary>
    /// <param name="value">ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡.</param>
    /// <returns>true ط¥ط°ط§ ظƒط§ظ†طھ ط§ظ„ظ‚ظٹظ…ط© null ط£ظˆ ظپط§ط±ط؛ط© ط£ظˆ طھط­طھظˆظٹ ط¹ظ„ظ‰ ظ…ط³ط§ظپط§طھ ط¨ظٹط¶ط§ط، ظپظ‚ط·.</returns>
    /// <example>
    /// <code>
    /// string? name = null;
    /// bool empty = name.IsNullOrWhiteSpace(); // true
    /// </code>
    /// </example>
    public static bool IsNullOrWhiteSpace(this string? value)
        => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ…ط§ ط¥ط°ط§ ظƒط§ظ†طھ ط§ظ„ظ‚ظٹظ…ط© طھط­طھظˆظٹ ط¹ظ„ظ‰ ظ†طµ ط؛ظٹط± ظپط§ط±ط؛.
    /// </summary>
    /// <param name="value">ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡.</param>
    /// <returns>true ط¥ط°ط§ ظƒط§ظ†طھ ط§ظ„ظ‚ظٹظ…ط© طھط­طھظˆظٹ ط¹ظ„ظ‰ ظ†طµ ط؛ظٹط± ظپط§ط±ط؛.</returns>
    /// <example>
    /// <code>
    /// string? name = "ط£ط­ظ…ط¯";
    /// bool has = name.HasValue(); // true
    /// </code>
    /// </example>
    public static bool HasValue(this string? value)
        => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// ظٹظ‚طµ ط§ظ„ظ†طµ ط¥ظ„ظ‰ ط§ظ„ط·ظˆظ„ ط§ظ„ظ…ط­ط¯ط¯ ظ…ط¹ ط¥ط²ط§ظ„ط© ط§ظ„ط²ظٹط§ط¯ط©.
    /// </summary>
    /// <param name="value">ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ ظ‚طµظ‘ظ‡.</param>
    /// <param name="maxLength">ط£ظ‚طµظ‰ ط·ظˆظ„ ظ…ط³ظ…ظˆط­ ط¨ظ‡.</param>
    /// <returns>ظ†طµ ظ…ظ‚طµظˆطµ ط¥ط°ط§ طھط¬ط§ظˆط² ط§ظ„ط·ظˆظ„ ط§ظ„ظ…ط³ظ…ظˆط­طŒ ط£ظˆ ط§ظ„ظ†طµ ط§ظ„ط£طµظ„ظٹ ط¥ط°ط§ ظƒط§ظ† ط¶ظ…ظ† ط§ظ„ط­ط¯ظ‘.</returns>
    /// <example>
    /// <code>
    /// string longText = "ظ†طµ ط·ظˆظٹظ„ ط¬ط¯ط§ظ‹";
    /// string truncated = longText.Truncate(4); // "ظ†طµ ط·"
    /// </code>
    /// </example>
    public static string Truncate(this string? value, int maxLength)
        => value?.Length <= maxLength ? value : value?[..maxLength] ?? string.Empty;

    /// <summary>
    /// ظٹط­ظˆظ„ ط§ظ„ظ†طµ ط§ظ„ظپط§ط±ط؛ ط¥ظ„ظ‰ null ظ„طھط³ظ‡ظٹظ„ ط§ظ„طھط®ط²ظٹظ† ظپظٹ ظ‚ط§ط¹ط¯ط© ط§ظ„ط¨ظٹط§ظ†ط§طھ.
    /// </summary>
    /// <param name="value">ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ طھط­ظˆظٹظ„ظ‡.</param>
    /// <returns>null ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ†طµ ظپط§ط±ط؛ط§ظ‹طŒ ظˆط¥ظ„ط§ ط§ظ„ظ†طµ ط§ظ„ط£طµظ„ظٹ.</returns>
    /// <example>
    /// <code>
    /// string? result = "   ".ToNullIfEmpty(); // null
    /// </code>
    /// </example>
    public static string? ToNullIfEmpty(this string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    /// <summary>
    /// ظٹط¹ظٹط¯ ظ‚ظٹظ…ط© ط§ظپطھط±ط§ط¶ظٹط© ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ†طµ ظپط§ط±ط؛ط§ظ‹.
    /// </summary>
    /// <param name="value">ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡.</param>
    /// <param name="defaultValue">ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط§ظپطھط±ط§ط¶ظٹط© ط¹ظ†ط¯ ط§ظ„ظپط±ط§ط؛.</param>
    /// <returns>ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط§ظپطھط±ط§ط¶ظٹط© ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ†طµ ظپط§ط±ط؛ط§ظ‹طŒ ظˆط¥ظ„ط§ ط§ظ„ظ†طµ ط§ظ„ط£طµظ„ظٹ.</returns>
    /// <example>
    /// <code>
    /// string? empty = null;
    /// string result = empty.DefaultIfEmpty("ط§ظپطھط±ط§ط¶ظٹ"); // "ط§ظپطھط±ط§ط¶ظٹ"
    /// </code>
    /// </example>
    public static string DefaultIfEmpty(this string? value, string defaultValue)
        => string.IsNullOrWhiteSpace(value) ? defaultValue : value;
}

