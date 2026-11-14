// -----------------------------------------------------------------------------
// MockBackupData.cs — بيانات النسخ الاحتياطية في الذاكرة (المرحلة أ — Mock · UI & UX QA)
//
// الغرض: تُمكّن شاشة frmBackup من الفتح والعمل **بلا قاعدة بيانات إطلاقاً**
// (RULE-N173/RULE-N176): لا قرص، لا SQL، لا Business/**.
//
// الأسماء تُبنى بالاصطلاح الرسمي عبر بيتها الواحد BackupFileNameHelper (RULE-N169)،
// فما تراه الشاشة في الوضع التجريبي هو نفس شكل الأسماء الذي تنتجه طبقة الأعمال:
//     SalesSystemDb_{yyyyMMdd_HHmmss}.bak
//
// البيانات الأولية تغطي الحالات الثلاث المطلوبة في تكافؤ المحاكاة (RULE-N167):
//   • المسار السعيد: ست نسخ موزّعة على (اليوم · اليوم · أمس · الأسبوع الماضي ·
//     الشهر الماضي · قبل ثلاثة أشهر) — كلها بتاريخ مقروء.
//   • الاسم القديم غير المطابق للاصطلاح (backup-copy-old.bak) — يُظهر مسار «—» في عمود التاريخ.
//   • أثر الإنشاء: كل CreateBackupAsync يُضيف اسماً جديداً فتنمو القائمة على الشاشة.
//
// الترتيب: الأحدث أولاً (كما تتوقعه الشاشة وكما تعيده الخدمة الحقيقية).
// -----------------------------------------------------------------------------

using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// المخزن الوحيد في الذاكرة لقائمة أسماء النسخ الاحتياطية (النسخة الوهمية).
/// تعيش البيانات ما دام التطبيق، فتُغلق الشاشة وتُفتح وتظل النسخة المُنشأة موجودة.
/// </summary>
internal static class MockBackupData
{
    /// <summary>اسم قاعدة البيانات المستخدم في أسماء النسخ التجريبية.</summary>
    internal const string DatabaseName = "SalesSystemDb";

    /// <summary>اسم قديم لا يطابق الاصطلاح — لتغطية مسار «—» (RULE-N167).</summary>
    private const string LegacyFileName = "backup-copy-old.bak";

    private static readonly object Sync = new();

    private static readonly List<string> FileNames = BuildSeeds();

    /// <summary>لقطة من القائمة الحالية مرتبةً بالأحدث أولاً (نسخة مستقلة — لا تُعدَّل من الخارج).</summary>
    internal static List<string> Snapshot()
    {
        lock (Sync)
        {
            return SortNewestFirst(FileNames);
        }
    }

    /// <summary>يُضيف اسماً جديداً إلى المخزن (يُستهلك من إنشاء نسخة في الوضع التجريبي).</summary>
    internal static void Add(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return;

        lock (Sync)
        {
            FileNames.Add(fileName);
        }
    }

    /// <summary>هل الاسم موجود في المخزن؟ (يُستهلك في رفض استعادة اسم غير موجود — RULE-N167).</summary>
    internal static bool Contains(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return false;

        lock (Sync)
        {
            return FileNames.Contains(fileName, StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>يُنشئ اسماً جديداً بالاصطلاح الرسمي — اسم قاعدة المخزن في مكان واحد هنا.</summary>
    internal static string BuildFileName(DateTime createdAt)
        => BackupFileNameHelper.Build(DatabaseName, createdAt);

    // ─────────────────────────────────────────────────────────────────
    //  البذور والترتيب
    // ─────────────────────────────────────────────────────────────────

    private static List<string> BuildSeeds()
    {
        // اللحظات ماضية دائماً (لا نسخة «في المستقبل») وتغطي: اليوم · اليوم · أمس ·
        // الأسبوع الماضي · الشهر الماضي · نسخة أقدم.
        var now = DateTime.Now;

        var created =
            new[]
            {
                now,                    // اليوم — الأحدث
                now.AddMinutes(-45),    // اليوم (نسخة ثانية)
                now.AddDays(-1),        // أمس
                now.AddDays(-7),        // الأسبوع الماضي
                now.AddMonths(-1),      // الشهر الماضي
                now.AddMonths(-3)       // نسخة أقدم
            };

        var names = new List<string>(created.Length + 1);

        foreach (var stamp in created)
            names.Add(BackupFileNameHelper.Build(DatabaseName, stamp));

        names.Add(LegacyFileName);

        return SortNewestFirst(names);
    }

    /// <summary>
    /// الأحدث أولاً — التاريخ يُقرأ من البيت الواحد للاصطلاح، والاسم غير المطابق
    /// (بلا تاريخ) يبقى في الآخر بلا استثناء.
    /// </summary>
    private static List<string> SortNewestFirst(List<string> names)
    {
        var ordered = new List<(string Name, DateTime CreatedAt)>(names.Count);

        foreach (var name in names)
        {
            ordered.Add(BackupFileNameHelper.TryGetCreatedAt(name, out var createdAt)
                ? (name, createdAt)
                : (name, DateTime.MinValue));
        }

        return ordered
            .OrderByDescending(entry => entry.CreatedAt)
            .ThenBy(entry => entry.Name, StringComparer.Ordinal)
            .Select(entry => entry.Name)
            .ToList();
    }
}
