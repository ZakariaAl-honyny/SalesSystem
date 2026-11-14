// -----------------------------------------------------------------------------
// BackupFileNameHelper.cs — البيت الواحد لاصطلاح تسمية النسخ الاحتياطية (RULE-N169)
//
// الاصطلاح الوحيد في النظام (تنتجه طبقة الأعمال فعلياً):
//     {DatabaseName}_{yyyyMMdd_HHmmss}.bak      مثال: SalesSystemDb_20261114_153045.bak
//
// الاستهلاك:
//   • الشاشة (frmBackup) — تقرأ منه تاريخ النسخة لعرضه في العمود وفي الملخّص وفي
//     رسالة تأكيد الاستعادة، فلا تحتوي الشاشة أي نسخة من الاصطلاح ولا أي regex
//     ولا أي Substring على "_" (RULE-N169…RULE-N172).
//   • الخدمة الوهمية (MockBackupDesktopService) — تُنشئ به اسم نسخة جديد بنفس الاصطلاح.
//
// ما لا تفعله:
//   • لا تلمس نظام الملفات إطلاقاً (لا File/Directory) — نصوص فقط، فتعمل في الوضع
//     التجريبي بلا قرص ولا قاعدة بيانات.
//   • لا ترمي استثناءً أبداً: الاسم الذي لا يطابق الاصطلاح يُعيد false (RULE-N14).
//   • لا تنسّق التاريخ للعرض — التنسيق في مكانه الواحد clsFormat.DateToLong.
// -----------------------------------------------------------------------------

using System.Globalization;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// المعرفة الوحيدة باصطلاح اسم ملف النسخة الاحتياطية
/// <c>{DatabaseName}_{yyyyMMdd_HHmmss}.bak</c>: بناءً للاسم وقراءةً لتاريخ إنشائه.
/// </summary>
public static class BackupFileNameHelper
{
    /// <summary>امتداد ملفات النسخ الاحتياطية.</summary>
    public const string Extension = ".bak";

    /// <summary>نص بديل يُعرض مكان التاريخ حين لا يطابق اسم الملف الاصطلاح.</summary>
    public const string NoDateText = "—";

    /// <summary>اسم قاعدة بديل حين يُطلب بناء اسم بلا اسم قاعدة.</summary>
    public const string FallbackDatabaseName = "Database";

    /// <summary>صيغة الطابع الزمني — المصدر الوحيد (لا تُكتب في أي ملف آخر).</summary>
    private const string TimestampFormat = "yyyyMMdd_HHmmss";

    /// <summary>طول الطابع الزمني داخل الاسم: <c>yyyyMMdd_HHmmss</c> = 15 محرفاً.</summary>
    private const int TimestampLength = 15;

    /// <summary>الفاصل بين اسم قاعدة البيانات والطابع الزمني، وداخل الطابع نفسه.</summary>
    private const char Separator = '_';

    /// <summary>
    /// يبني اسم ملف نسخة احتياطية بالاصطلاح الرسمي.
    /// </summary>
    /// <param name="databaseName">اسم قاعدة البيانات (اسم فارغ ⇒ <see cref="FallbackDatabaseName"/>).</param>
    /// <param name="createdAt">لحظة إنشاء النسخة.</param>
    public static string Build(string? databaseName, DateTime createdAt)
    {
        var name = string.IsNullOrWhiteSpace(databaseName) ? FallbackDatabaseName : databaseName.Trim();

        return $"{name}{Separator}{createdAt.ToString(TimestampFormat, CultureInfo.InvariantCulture)}{Extension}";
    }

    /// <summary>
    /// يقرأ لحظة إنشاء النسخة من اسم الملف.
    /// </summary>
    /// <param name="fileName">اسم الملف (أو مسار كامل — يُهمل المجلد).</param>
    /// <param name="createdAt">اللحظة المقروءة، أو <c>default</c> عند عدم المطابقة.</param>
    /// <returns><c>true</c> إذا طابق الاسم الاصطلاح، وإلا <c>false</c> — بلا استثناء.</returns>
    public static bool TryGetCreatedAt(string? fileName, out DateTime createdAt)
    {
        createdAt = default;

        try
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;

            var name = Path.GetFileName(fileName.Trim());

            if (string.IsNullOrEmpty(name)) return false;
            if (!name.EndsWith(Extension, StringComparison.OrdinalIgnoreCase)) return false;

            var stem = name[..^Extension.Length];

            // اسم قاعدة (محرف واحد على الأقل) + فاصل + الطابع الزمني = 17 محرفاً على الأقل.
            if (stem.Length < TimestampLength + 2) return false;

            var separatorIndex = stem.Length - TimestampLength - 1;
            if (stem[separatorIndex] != Separator) return false;

            var timestamp = stem.Substring(separatorIndex + 1, TimestampLength);

            return DateTime.TryParseExact(
                timestamp,
                TimestampFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out createdAt);
        }
        catch (Exception ex)
        {
            // مسار غير صالح لا يُسقط شاشة — يُسجَّل ويُعدّ الاسم غير مطابق.
            Serilog.Log.Debug(ex, "تعذر قراءة تاريخ النسخة الاحتياطية من الاسم {FileName}", fileName);
            createdAt = default;
            return false;
        }
    }

    /// <summary>
    /// نص تاريخ النسخة للعرض (مصدر التنسيق الوحيد: <see cref="clsFormat.DateToLong"/>)
    /// أو <see cref="NoDateText"/> («—») حين لا يطابق الاسم الاصطلاح — بلا استثناء.
    /// </summary>
    /// <param name="fileName">اسم الملف (أو مسار كامل).</param>
    public static string GetDisplayDate(string? fileName)
        => TryGetCreatedAt(fileName, out var createdAt)
            ? clsFormat.DateToLong(createdAt)
            : NoDateText;
}
