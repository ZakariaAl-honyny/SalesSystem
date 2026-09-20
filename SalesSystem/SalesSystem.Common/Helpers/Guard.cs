namespace SalesSystem.Common.Helpers;

/// <summary>
/// ظ…ط¬ظ…ظˆط¹ط© ظ…ظ† ط¯ظˆط§ظ„ ط§ظ„طھط­ظ‚ظ‚ (Guard clauses) ظ„طھظˆط­ظٹط¯ ط¹ظ…ظ„ظٹط© ظپط­طµ ط§ظ„ظ…ط¹ط§ظ…ظ„ط§طھ ظˆط§ظ„ط¨ظٹط§ظ†ط§طھ.
/// طھظˆظپط± ظ‡ط°ظ‡ ط§ظ„ط¯ظˆط§ظ„ ط£ط³ظ„ظˆط¨ظٹظ†:
///   1. Exception-style â€” ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ† ط£ط®ط·ط§ط، ط§ظ„ظ…ط¨ط±ظ…ط¬ ط£ظˆ ط§ظ„ط¨ظ†ظٹط© ط§ظ„طھط­طھظٹط© (طھط·ط±ط­ ط§ط³طھط«ظ†ط§ط،).
///   2. Result-style â€” ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ† ظ‚ظˆط§ط¹ط¯ ط§ظ„ط¹ظ…ظ„ ظˆظ…ط¯ط®ظ„ط§طھ ط§ظ„ظ…ط³طھط®ط¯ظ… (طھط±ط¬ط¹ Result).
/// </summary>
public static class Guard
{
    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ
    //  Exception-style (Fail-fast)
    //  طھظڈط³طھط®ط¯ظ… ظ„ظ„طھط­ظ‚ظ‚ ظ…ظ† ط£ط®ط·ط§ط، ط§ظ„طھط·ظˆظٹط± ظˆط§ظ„ط¨ظ†ظٹط© ط§ظ„طھط­طھظٹط©
    // â•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گâ•گ

    /// <summary>
    /// ظٹطھط­ظ‚ظ‚ ظ…ظ† ط£ظ† ط§ظ„ظ‚ظٹظ…ط© ظ„ظٹط³طھ null.
    /// ظپظٹ ط­ط§ظ„ ظƒط§ظ†طھ null ظٹطھظ… ط·ط±ط­ ArgumentNullException.
    /// </summary>
    /// <param name="value">ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡ط§.</param>
    /// <param name="argumentName">ط§ط³ظ… ط§ظ„ظ…طھط؛ظٹط± (ظ„ط±ط³ط§ظ„ط© ط§ظ„ط®ط·ط£).</param>
    public static void AgainstNull(object? value, string argumentName)
    {
        if (value is null)
            throw new ArgumentNullException(argumentName, $"{argumentName} ظ„ط§ ظٹظ…ظƒظ† ط£ظ† ظٹظƒظˆظ† ظپط§ط±ط؛ط§ظ‹.");
    }

    /// <summary>
    /// ظٹطھط­ظ‚ظ‚ ظ…ظ† ط£ظ† ط§ظ„ظ†طµ ظ„ظٹط³ ظپط§ط±ط؛ط§ظ‹ ط£ظˆ ظٹطھظƒظˆظ† ظ…ظ† ظ…ط³ط§ظپط§طھ ظپظ‚ط·.
    /// ظپظٹ ط­ط§ظ„ ظƒط§ظ† ظپط§ط±ط؛ط§ظ‹ ظٹطھظ… ط·ط±ط­ ArgumentException.
    /// </summary>
    /// <param name="value">ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡.</param>
    /// <param name="argumentName">ط§ط³ظ… ط§ظ„ظ…طھط؛ظٹط± (ظ„ط±ط³ط§ظ„ط© ط§ظ„ط®ط·ط£).</param>
    public static void AgainstNullOrEmpty(string? value, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{argumentName} ظ„ط§ ظٹظ…ظƒظ† ط£ظ† ظٹظƒظˆظ† ظپط§ط±ط؛ط§ظ‹.", argumentName);
    }

    /// <summary>
    /// ظٹطھط­ظ‚ظ‚ ظ…ظ† ط£ظ† ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط±ظ‚ظ…ظٹط© ظ„ظٹط³طھ ط³ط§ظ„ط¨ط©.
    /// ظپظٹ ط­ط§ظ„ ظƒط§ظ†طھ ط³ط§ظ„ط¨ط© ظٹطھظ… ط·ط±ط­ ArgumentOutOfRangeException.
    /// </summary>
    /// <param name="value">ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط±ظ‚ظ…ظٹط© ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡ط§.</param>
    /// <param name="argumentName">ط§ط³ظ… ط§ظ„ظ…طھط؛ظٹط± (ظ„ط±ط³ط§ظ„ط© ط§ظ„ط®ط·ط£).</param>
    public static void AgainstNegative(decimal value, string argumentName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} ظ„ط§ ظٹظ…ظƒظ† ط£ظ† ظٹظƒظˆظ† ط£ظ‚ظ„ ظ…ظ† ط§ظ„طµظپط±.");
    }

    /// <summary>
    /// ظٹطھط­ظ‚ظ‚ ظ…ظ† ط£ظ† ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط±ظ‚ظ…ظٹط© ظ„ط§ طھط³ط§ظˆظٹ طµظپط±ط§ظ‹.
    /// ظپظٹ ط­ط§ظ„ ظƒط§ظ†طھ طµظپط±ط§ظ‹ ظٹطھظ… ط·ط±ط­ ArgumentOutOfRangeException.
    /// </summary>
    /// <param name="value">ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط±ظ‚ظ…ظٹط© ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡ط§.</param>
    /// <param name="argumentName">ط§ط³ظ… ط§ظ„ظ…طھط؛ظٹط± (ظ„ط±ط³ط§ظ„ط© ط§ظ„ط®ط·ط£).</param>
    public static void AgainstZero(decimal value, string argumentName)
    {
        if (value == 0)
            throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} ظ„ط§ ظٹظ…ظƒظ† ط£ظ† ظٹظƒظˆظ† طµظپط±ط§ظ‹.");
    }

    /// <summary>
    /// ظٹطھط­ظ‚ظ‚ ظ…ظ† ط£ظ† ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط±ظ‚ظ…ظٹط© ظ…ظˆط¬ط¨ط© (ط£ظƒط¨ط± ظ…ظ† طµظپط±).
    /// ظپظٹ ط­ط§ظ„ ظƒط§ظ†طھ â‰¤ طµظپط± ظٹطھظ… ط·ط±ط­ ArgumentOutOfRangeException.
    /// </summary>
    /// <param name="value">ط§ظ„ظ‚ظٹظ…ط© ط§ظ„ط±ظ‚ظ…ظٹط© ط§ظ„ظ…ط±ط§ط¯ ظپط­طµظ‡ط§.</param>
    /// <param name="argumentName">ط§ط³ظ… ط§ظ„ظ…طھط؛ظٹط± (ظ„ط±ط³ط§ظ„ط© ط§ظ„ط®ط·ط£).</param>
    public static void AgainstNegativeOrZero(decimal value, string argumentName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} ظٹط¬ط¨ ط£ظ† ظٹظƒظˆظ† ط£ظƒط¨ط± ظ…ظ† ط§ظ„طµظپط±.");
    }
}
