// <copyright file="FileHelper.cs" company="SalesSystem">
// Copyright (c) SalesSystem. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace SalesSystem.Common.Helpers;

/// <summary>
/// فئة مساعدة للتعامل مع عمليات الملفات والمجلدات بشكل آمن مع معالجة الأخطاء.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// يقرأ محتوى الملف النصي بالكامل. إذا كان المسار فارغاً أو الملف غير موجود يُرجع <c>null</c>.
    /// </summary>
    /// <param name="filePath">المسار الكامل للملف المراد قراءته.</param>
    /// <returns>محتوى الملف كنص أو <c>null</c> في حال عدم وجود الملف.</returns>
    public static string? ReadAllText(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return null;
        return File.ReadAllText(filePath);
    }

    /// <summary>
    /// يقرأ محتوى الملف النصي بالكامل بشكل غير متزامن.
    /// إذا كان المسار فارغاً أو الملف غير موجود يُرجع <c>null</c>.
    /// </summary>
    /// <param name="filePath">المسار الكامل للملف المراد قراءته.</param>
    /// <param name="ct">رمز الإلغاء لإلغاء العملية.</param>
    /// <returns>محتوى الملف كنص أو <c>null</c> في حال عدم وجود الملف.</returns>
    public static async Task<string?> ReadAllTextAsync(string? filePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return null;
        return await File.ReadAllTextAsync(filePath, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// يحاول كتابة نص إلى ملف مع إنشاء المجلدات الفرعية تلقائياً عند الحاجة.
    /// </summary>
    /// <param name="filePath">المسار الكامل للملف المراد الكتابة إليه.</param>
    /// <param name="contents">محتوى النص المراد كتابته.</param>
    /// <param name="logger">مُسجل الأخطاء اختياري — يُستخدم لتسجيل الاستثناءات في حال فشلت العملية.</param>
    /// <returns><c>true</c> إذا تمت الكتابة بنجاح، وإلا <c>false</c>.</returns>
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
            logger?.LogError(ex, "فشل في كتابة الملف: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// يحاول كتابة نص إلى ملف بشكل غير متزامن مع إنشاء المجلدات الفرعية تلقائياً.
    /// </summary>
    /// <param name="filePath">المسار الكامل للملف المراد الكتابة إليه.</param>
    /// <param name="contents">محتوى النص المراد كتابته.</param>
    /// <param name="ct">رمز الإلغاء لإلغاء العملية.</param>
    /// <param name="logger">مُسجل الأخطاء اختياري — يُستخدم لتسجيل الاستثناءات في حال فشلت العملية.</param>
    /// <returns><c>true</c> إذا تمت الكتابة بنجاح، وإلا <c>false</c>.</returns>
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
            logger?.LogError(ex, "فشل في كتابة الملف بشكل غير متزامن: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// يحاول حذف ملف. إذا كان الملف غير موجود يعتبر الحذف ناجحاً.
    /// </summary>
    /// <param name="filePath">المسار الكامل للملف المراد حذفه.</param>
    /// <param name="logger">مُسجل الأخطاء اختياري — يُستخدم لتسجيل الاستثناءات في حال فشلت العملية.</param>
    /// <returns><c>true</c> إذا تم الحذف بنجاح أو الملف غير موجود، وإلا <c>false</c>.</returns>
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
            logger?.LogError(ex, "فشل في حذف الملف: {FilePath}", filePath);
            return false;
        }
    }

    /// <summary>
    /// يحصل على حجم الملف بالبايت. إذا كان المسار فارغاً أو الملف غير موجود يُرجع <c>null</c>.
    /// </summary>
    /// <param name="filePath">المسار الكامل للملف.</param>
    /// <returns>حجم الملف بالبايت أو <c>null</c> في حال عدم وجود الملف.</returns>
    public static long? GetFileSize(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return null;
        return new FileInfo(filePath).Length;
    }

    /// <summary>
    /// يتأكد من وجود المجلد المحدد، وإن لم يكن موجوداً يُنشئه (بما في ذلك المجلدات الفرعية).
    /// </summary>
    /// <param name="directoryPath">المسار الكامل للمجلد المراد التأكد من وجوده.</param>
    /// <param name="logger">مُسجل الأخطاء اختياري — يُستخدم لتسجيل الاستثناءات في حال فشلت العملية.</param>
    /// <returns><c>true</c> إذا تم التأكد من وجود المجلد أو إنشاؤه بنجاح، وإلا <c>false</c>.</returns>
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
            logger?.LogError(ex, "فشل في إنشاء المجلد: {DirectoryPath}", directoryPath);
            return false;
        }
    }

    /// <summary>
    /// يُنظف اسم الملف من الأحرف غير الصالحة لأنظمة الملفات.
    /// إذا كانت النتيجة فارغة بعد التنظيف يُرجع الاسم الافتراضي "file".
    /// </summary>
    /// <param name="fileName">اسم الملف المراد تنظيفه.</param>
    /// <returns>اسم ملف صالح للاستخدام في نظام الملفات.</returns>
    public static string SanitizeFileName(string fileName)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(fileName.Where(c => !invalid.Contains(c)).ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "file" : sanitized;
    }

    /// <summary>
    /// يُنشئ اسماً فريداً للملف داخل المجلد المحدد بإضافة رقم تسلسلي
    /// إذا كان الملف موجوداً مسبقاً (مثال: file_1.txt, file_2.txt).
    /// </summary>
    /// <param name="directory">المجلد الذي سيتم التحقق من وجود الملف فيه.
    /// إذا كان <c>null</c> يُستخدم المجلد الحالي للتطبيق.</param>
    /// <param name="fileName">اسم الملف الأصلي المراد إنشاء نسخة فريدة منه.</param>
    /// <returns>اسم الملف الفريد (بدون المسار الكامل).</returns>
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

