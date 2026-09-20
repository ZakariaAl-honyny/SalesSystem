// <copyright file="FileHelper.cs" company="SalesSystem">
// Copyright (c) SalesSystem. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace SalesSystem.Common.Helpers;

/// <summary>
/// ظپط¦ط© ظ…ط³ط§ط¹ط¯ط© ظ„ظ„طھط¹ط§ظ…ظ„ ظ…ط¹ ط¹ظ…ظ„ظٹط§طھ ط§ظ„ظ…ظ„ظپط§طھ ظˆط§ظ„ظ…ط¬ظ„ط¯ط§طھ ط¨ط´ظƒظ„ ط¢ظ…ظ† ظ…ط¹ ظ…ط¹ط§ظ„ط¬ط© ط§ظ„ط£ط®ط·ط§ط،.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// ظٹظ‚ط±ط£ ظ…ط­طھظˆظ‰ ط§ظ„ظ…ظ„ظپ ط§ظ„ظ†طµظٹ ط¨ط§ظ„ظƒط§ظ…ظ„. ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ…ط³ط§ط± ظپط§ط±ط؛ط§ظ‹ ط£ظˆ ط§ظ„ظ…ظ„ظپ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ ظٹظڈط±ط¬ط¹ <c>null</c>.
    /// </summary>
    /// <param name="filePath">ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„ ظ„ظ„ظ…ظ„ظپ ط§ظ„ظ…ط±ط§ط¯ ظ‚ط±ط§ط،طھظ‡.</param>
    /// <returns>ظ…ط­طھظˆظ‰ ط§ظ„ظ…ظ„ظپ ظƒظ†طµ ط£ظˆ <c>null</c> ظپظٹ ط­ط§ظ„ ط¹ط¯ظ… ظˆط¬ظˆط¯ ط§ظ„ظ…ظ„ظپ.</returns>
    public static string? ReadAllText(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return null;
        return File.ReadAllText(filePath);
    }

    /// <summary>
    /// ظٹظ‚ط±ط£ ظ…ط­طھظˆظ‰ ط§ظ„ظ…ظ„ظپ ط§ظ„ظ†طµظٹ ط¨ط§ظ„ظƒط§ظ…ظ„ ط¨ط´ظƒظ„ ط؛ظٹط± ظ…طھط²ط§ظ…ظ†.
    /// ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ…ط³ط§ط± ظپط§ط±ط؛ط§ظ‹ ط£ظˆ ط§ظ„ظ…ظ„ظپ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ ظٹظڈط±ط¬ط¹ <c>null</c>.
    /// </summary>
    /// <param name="filePath">ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„ ظ„ظ„ظ…ظ„ظپ ط§ظ„ظ…ط±ط§ط¯ ظ‚ط±ط§ط،طھظ‡.</param>
    /// <param name="ct">ط±ظ…ط² ط§ظ„ط¥ظ„ط؛ط§ط، ظ„ط¥ظ„ط؛ط§ط، ط§ظ„ط¹ظ…ظ„ظٹط©.</param>
    /// <returns>ظ…ط­طھظˆظ‰ ط§ظ„ظ…ظ„ظپ ظƒظ†طµ ط£ظˆ <c>null</c> ظپظٹ ط­ط§ظ„ ط¹ط¯ظ… ظˆط¬ظˆط¯ ط§ظ„ظ…ظ„ظپ.</returns>
    public static async Task<string?> ReadAllTextAsync(string? filePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return null;
        return await File.ReadAllTextAsync(filePath, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// ظٹط­ط§ظˆظ„ ظƒطھط§ط¨ط© ظ†طµ ط¥ظ„ظ‰ ظ…ظ„ظپ ظ…ط¹ ط¥ظ†ط´ط§ط، ط§ظ„ظ…ط¬ظ„ط¯ط§طھ ط§ظ„ظپط±ط¹ظٹط© طھظ„ظ‚ط§ط¦ظٹط§ظ‹ ط¹ظ†ط¯ ط§ظ„ط­ط§ط¬ط©.
    /// </summary>
    /// <param name="filePath">ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„ ظ„ظ„ظ…ظ„ظپ ط§ظ„ظ…ط±ط§ط¯ ط§ظ„ظƒطھط§ط¨ط© ط¥ظ„ظٹظ‡.</param>
    /// <param name="contents">ظ…ط­طھظˆظ‰ ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ ظƒطھط§ط¨طھظ‡.</param>
    /// <param name="logger">ظ…ظڈط³ط¬ظ„ ط§ظ„ط£ط®ط·ط§ط، ط§ط®طھظٹط§ط±ظٹ â€” ظٹظڈط³طھط®ط¯ظ… ظ„طھط³ط¬ظٹظ„ ط§ظ„ط§ط³طھط«ظ†ط§ط،ط§طھ ظپظٹ ط­ط§ظ„ ظپط´ظ„طھ ط§ظ„ط¹ظ…ظ„ظٹط©.</param>
    /// <returns><c>true</c> ط¥ط°ط§ طھظ…طھ ط§ظ„ظƒطھط§ط¨ط© ط¨ظ†ط¬ط§ط­طŒ ظˆط¥ظ„ط§ <c>false</c>.</returns>
    public static bool TryWriteAllText(string filePath, string contents, ILogger? logger = null)
    {
        try
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(filePath, contents);
            return true;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "ظپط´ظ„ ظپظٹ ظƒطھط§ط¨ط© ط§ظ„ظ…ظ„ظپ: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// ظٹط­ط§ظˆظ„ ظƒطھط§ط¨ط© ظ†طµ ط¥ظ„ظ‰ ظ…ظ„ظپ ط¨ط´ظƒظ„ ط؛ظٹط± ظ…طھط²ط§ظ…ظ† ظ…ط¹ ط¥ظ†ط´ط§ط، ط§ظ„ظ…ط¬ظ„ط¯ط§طھ ط§ظ„ظپط±ط¹ظٹط© طھظ„ظ‚ط§ط¦ظٹط§ظ‹.
    /// </summary>
    /// <param name="filePath">ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„ ظ„ظ„ظ…ظ„ظپ ط§ظ„ظ…ط±ط§ط¯ ط§ظ„ظƒطھط§ط¨ط© ط¥ظ„ظٹظ‡.</param>
    /// <param name="contents">ظ…ط­طھظˆظ‰ ط§ظ„ظ†طµ ط§ظ„ظ…ط±ط§ط¯ ظƒطھط§ط¨طھظ‡.</param>
    /// <param name="ct">ط±ظ…ط² ط§ظ„ط¥ظ„ط؛ط§ط، ظ„ط¥ظ„ط؛ط§ط، ط§ظ„ط¹ظ…ظ„ظٹط©.</param>
    /// <param name="logger">ظ…ظڈط³ط¬ظ„ ط§ظ„ط£ط®ط·ط§ط، ط§ط®طھظٹط§ط±ظٹ â€” ظٹظڈط³طھط®ط¯ظ… ظ„طھط³ط¬ظٹظ„ ط§ظ„ط§ط³طھط«ظ†ط§ط،ط§طھ ظپظٹ ط­ط§ظ„ ظپط´ظ„طھ ط§ظ„ط¹ظ…ظ„ظٹط©.</param>
    /// <returns><c>true</c> ط¥ط°ط§ طھظ…طھ ط§ظ„ظƒطھط§ط¨ط© ط¨ظ†ط¬ط§ط­طŒ ظˆط¥ظ„ط§ <c>false</c>.</returns>
    public static async Task<bool> TryWriteAllTextAsync(string filePath, string contents, CancellationToken ct = default, ILogger? logger = null)
    {
        try
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            await File.WriteAllTextAsync(filePath, contents, ct).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "ظپط´ظ„ ظپظٹ ظƒطھط§ط¨ط© ط§ظ„ظ…ظ„ظپ ط¨ط´ظƒظ„ ط؛ظٹط± ظ…طھط²ط§ظ…ظ†: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// ظٹط­ط§ظˆظ„ ط­ط°ظپ ظ…ظ„ظپ. ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ…ظ„ظپ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ ظٹط¹طھط¨ط± ط§ظ„ط­ط°ظپ ظ†ط§ط¬ط­ط§ظ‹.
    /// </summary>
    /// <param name="filePath">ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„ ظ„ظ„ظ…ظ„ظپ ط§ظ„ظ…ط±ط§ط¯ ط­ط°ظپظ‡.</param>
    /// <param name="logger">ظ…ظڈط³ط¬ظ„ ط§ظ„ط£ط®ط·ط§ط، ط§ط®طھظٹط§ط±ظٹ â€” ظٹظڈط³طھط®ط¯ظ… ظ„طھط³ط¬ظٹظ„ ط§ظ„ط§ط³طھط«ظ†ط§ط،ط§طھ ظپظٹ ط­ط§ظ„ ظپط´ظ„طھ ط§ظ„ط¹ظ…ظ„ظٹط©.</param>
    /// <returns><c>true</c> ط¥ط°ط§ طھظ… ط§ظ„ط­ط°ظپ ط¨ظ†ط¬ط§ط­ ط£ظˆ ط§ظ„ظ…ظ„ظپ ط؛ظٹط± ظ…ظˆط¬ظˆط¯طŒ ظˆط¥ظ„ط§ <c>false</c>.</returns>
    public static bool TryDelete(string? filePath, ILogger? logger = null)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                File.Delete(filePath);
            return true;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "ظپط´ظ„ ظپظٹ ط­ط°ظپ ط§ظ„ظ…ظ„ظپ: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// ظٹط­طµظ„ ط¹ظ„ظ‰ ط­ط¬ظ… ط§ظ„ظ…ظ„ظپ ط¨ط§ظ„ط¨ط§ظٹطھ. ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ…ط³ط§ط± ظپط§ط±ط؛ط§ظ‹ ط£ظˆ ط§ظ„ظ…ظ„ظپ ط؛ظٹط± ظ…ظˆط¬ظˆط¯ ظٹظڈط±ط¬ط¹ <c>null</c>.
    /// </summary>
    /// <param name="filePath">ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„ ظ„ظ„ظ…ظ„ظپ.</param>
    /// <returns>ط­ط¬ظ… ط§ظ„ظ…ظ„ظپ ط¨ط§ظ„ط¨ط§ظٹطھ ط£ظˆ <c>null</c> ظپظٹ ط­ط§ظ„ ط¹ط¯ظ… ظˆط¬ظˆط¯ ط§ظ„ظ…ظ„ظپ.</returns>
    public static long? GetFileSize(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return null;
        return new FileInfo(filePath).Length;
    }

    /// <summary>
    /// ظٹطھط£ظƒط¯ ظ…ظ† ظˆط¬ظˆط¯ ط§ظ„ظ…ط¬ظ„ط¯ ط§ظ„ظ…ط­ط¯ط¯طŒ ظˆط¥ظ† ظ„ظ… ظٹظƒظ† ظ…ظˆط¬ظˆط¯ط§ظ‹ ظٹظڈظ†ط´ط¦ظ‡ (ط¨ظ…ط§ ظپظٹ ط°ظ„ظƒ ط§ظ„ظ…ط¬ظ„ط¯ط§طھ ط§ظ„ظپط±ط¹ظٹط©).
    /// </summary>
    /// <param name="directoryPath">ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„ ظ„ظ„ظ…ط¬ظ„ط¯ ط§ظ„ظ…ط±ط§ط¯ ط§ظ„طھط£ظƒط¯ ظ…ظ† ظˆط¬ظˆط¯ظ‡.</param>
    /// <param name="logger">ظ…ظڈط³ط¬ظ„ ط§ظ„ط£ط®ط·ط§ط، ط§ط®طھظٹط§ط±ظٹ â€” ظٹظڈط³طھط®ط¯ظ… ظ„طھط³ط¬ظٹظ„ ط§ظ„ط§ط³طھط«ظ†ط§ط،ط§طھ ظپظٹ ط­ط§ظ„ ظپط´ظ„طھ ط§ظ„ط¹ظ…ظ„ظٹط©.</param>
    /// <returns><c>true</c> ط¥ط°ط§ طھظ… ط§ظ„طھط£ظƒط¯ ظ…ظ† ظˆط¬ظˆط¯ ط§ظ„ظ…ط¬ظ„ط¯ ط£ظˆ ط¥ظ†ط´ط§ط¤ظ‡ ط¨ظ†ط¬ط§ط­طŒ ظˆط¥ظ„ط§ <c>false</c>.</returns>
    public static bool EnsureDirectoryExists(string? directoryPath, ILogger? logger = null)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            return false;
        try
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            return true;
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "ظپط´ظ„ ظپظٹ ط¥ظ†ط´ط§ط، ط§ظ„ظ…ط¬ظ„ط¯: {DirectoryPath}", directoryPath);
            return false;
        }
    }

    /// <summary>
    /// ظٹظڈظ†ط¸ظپ ط§ط³ظ… ط§ظ„ظ…ظ„ظپ ظ…ظ† ط§ظ„ط£ط­ط±ظپ ط؛ظٹط± ط§ظ„طµط§ظ„ط­ط© ظ„ط£ظ†ط¸ظ…ط© ط§ظ„ظ…ظ„ظپط§طھ.
    /// ط¥ط°ط§ ظƒط§ظ†طھ ط§ظ„ظ†طھظٹط¬ط© ظپط§ط±ط؛ط© ط¨ط¹ط¯ ط§ظ„طھظ†ط¸ظٹظپ ظٹظڈط±ط¬ط¹ ط§ظ„ط§ط³ظ… ط§ظ„ط§ظپطھط±ط§ط¶ظٹ "file".
    /// </summary>
    /// <param name="fileName">ط§ط³ظ… ط§ظ„ظ…ظ„ظپ ط§ظ„ظ…ط±ط§ط¯ طھظ†ط¸ظٹظپظ‡.</param>
    /// <returns>ط§ط³ظ… ظ…ظ„ظپ طµط§ظ„ط­ ظ„ظ„ط§ط³طھط®ط¯ط§ظ… ظپظٹ ظ†ط¸ط§ظ… ط§ظ„ظ…ظ„ظپط§طھ.</returns>
    public static string SanitizeFileName(string fileName)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(fileName.Where(c => !invalid.Contains(c)).ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "file" : sanitized;
    }

    /// <summary>
    /// ظٹظڈظ†ط´ط¦ ط§ط³ظ…ط§ظ‹ ظپط±ظٹط¯ط§ظ‹ ظ„ظ„ظ…ظ„ظپ ط¯ط§ط®ظ„ ط§ظ„ظ…ط¬ظ„ط¯ ط§ظ„ظ…ط­ط¯ط¯ ط¨ط¥ط¶ط§ظپط© ط±ظ‚ظ… طھط³ظ„ط³ظ„ظٹ
    /// ط¥ط°ط§ ظƒط§ظ† ط§ظ„ظ…ظ„ظپ ظ…ظˆط¬ظˆط¯ط§ظ‹ ظ…ط³ط¨ظ‚ط§ظ‹ (ظ…ط«ط§ظ„: file_1.txt, file_2.txt).
    /// </summary>
    /// <param name="directory">ط§ظ„ظ…ط¬ظ„ط¯ ط§ظ„ط°ظٹ ط³ظٹطھظ… ط§ظ„طھط­ظ‚ظ‚ ظ…ظ† ظˆط¬ظˆط¯ ط§ظ„ظ…ظ„ظپ ظپظٹظ‡.
    /// ط¥ط°ط§ ظƒط§ظ† <c>null</c> ظٹظڈط³طھط®ط¯ظ… ط§ظ„ظ…ط¬ظ„ط¯ ط§ظ„ط­ط§ظ„ظٹ ظ„ظ„طھط·ط¨ظٹظ‚.</param>
    /// <param name="fileName">ط§ط³ظ… ط§ظ„ظ…ظ„ظپ ط§ظ„ط£طµظ„ظٹ ط§ظ„ظ…ط±ط§ط¯ ط¥ظ†ط´ط§ط، ظ†ط³ط®ط© ظپط±ظٹط¯ط© ظ…ظ†ظ‡.</param>
    /// <returns>ط§ط³ظ… ط§ظ„ظ…ظ„ظپ ط§ظ„ظپط±ظٹط¯ (ط¨ط¯ظˆظ† ط§ظ„ظ…ط³ط§ط± ط§ظ„ظƒط§ظ…ظ„).</returns>
    public static string GetUniqueFileName(string? directory, string fileName)
    {
        if (string.IsNullOrWhiteSpace(directory))
            directory = Directory.GetCurrentDirectory();

        var name = Path.GetFileNameWithoutExtension(fileName);
        var ext = Path.GetExtension(fileName);
        var fullPath = Path.Combine(directory, fileName);

        if (!File.Exists(fullPath))
            return fileName;

        int counter = 1;
        while (File.Exists(Path.Combine(directory, $"{name}_{counter}{ext}")))
            counter++;

        return $"{name}_{counter}{ext}";
    }
}

