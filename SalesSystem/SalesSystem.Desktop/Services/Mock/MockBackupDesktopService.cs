// -----------------------------------------------------------------------------
// MockBackupDesktopService.cs — النسخة الوهمية لعقد النسخ الاحتياطي (المرحلة أ)
//
// تُغلق فجوة التغطية المانعة في docs/MOCK-COVERAGE.md (RULE-N173/RULE-N178):
// شاشة frmBackup تُفتح وتُستخدم **بلا قاعدة بيانات إطلاقاً**.
//
// تكافؤ المحاكاة (RULE-N167) — ترفض ما ترفضه الخدمة الحقيقية/طبقة الأعمال:
//   • استعادة بلا اسم ملف            ⇒ «يجب تحديد ملف النسخة الاحتياطية.»
//   • استعادة اسم غير موجود في القائمة ⇒ «ملف النسخة الاحتياطية غير موجود.»
//   • اسم غير مطابق للاصطلاح         ⇒ لا يمنع الاستعادة (سلوك الخدمة الحقيقية:
//     الملف موجود ⇒ تُقبل الاستعادة)، لكن الشاشة تعرض تاريخه «—».
//   • تعذّر توليد اسم فريد (نفاد محاولات إزاحة الطابع الزمني)
//                                    ⇒ «تعذر إنشاء اسم نسخة فريد. يرجى إعادة المحاولة.»
//     ولا يُضاف أي اسم مكرّر إلى القائمة.
// والمسار السعيد: إنشاء نسخة تُضيف اسماً جديداً بتاريخ اللحظة الراهنة فتنمو القائمة.
//
// ما لا تفعله إطلاقاً: لا نظام ملفات، لا SQL، لا Business/**، ولا استثناء يفلت
// (RULE-N14 spirit) — كل خطأ يُسجَّل في Serilog ويُعاد Result فاشل برسالة عربية.
// -----------------------------------------------------------------------------

using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// تنفيذ وهمي (في الذاكرة) لعقد <see cref="IBackupDesktopService"/> — للوضع التجريبي فقط.
/// </summary>
public sealed class MockBackupDesktopService : IBackupDesktopService
{
    /// <summary>رسالة الرفض المشتركة — مطابقة لنص الخدمة الحقيقية.</summary>
    private const string MissingFileNameMessage = "يجب تحديد ملف النسخة الاحتياطية.";

    /// <summary>رسالة رفض استعادة اسم غير موجود — مطابقة لسلوك طبقة الأعمال.</summary>
    private const string UnknownFileNameMessage = "ملف النسخة الاحتياطية غير موجود.";

    /// <summary>رسالة رفض إنشاء نسخة باسم لا يمكن تمييزه (فشل كل محاولات إزاحة الطابع الزمني).</summary>
    private const string UnableToBuildUniqueNameMessage = "تعذر إنشاء اسم نسخة فريد. يرجى إعادة المحاولة.";

    /// <summary>عدد محاولات إزاحة الطابع الزمني ثانيةً عند تعارض الاسم.</summary>
    private const int MaxUniqueNameAttempts = 5;

    /// <summary>
    /// إنشاء نسخة احتياطية: يُضاف اسم جديد بالأصطلاح الرسمي وبتاريخ اللحظة الراهنة.
    /// </summary>
    /// <returns>اسم الملف الجديد (لا مسار كامل — الشاشة لا تستخدم القيمة كاسم ملف).</returns>
    public Task<Result<string>> CreateBackupAsync(CancellationToken ct = default)
    {
        try
        {
            var createdAt = DateTime.Now;
            var fileName = MockBackupData.BuildFileName(createdAt);

            // إنشاء نسختين في الثانية نفسها لا يُنتج صفّين بنفس الاسم: يُزحزح الطابع ثانية.
            for (var attempt = 0; attempt < MaxUniqueNameAttempts && MockBackupData.Contains(fileName); attempt++)
            {
                createdAt = createdAt.AddSeconds(1);
                fileName = MockBackupData.BuildFileName(createdAt);
            }

            // نفدت المحاولات وما زال الاسم مكرّراً ⇒ رفض عربي ولا يُضاف شيء (لا اسم مكرّر في القائمة).
            if (MockBackupData.Contains(fileName))
                return Task.FromResult(Result<string>.Failure(UnableToBuildUniqueNameMessage));

            MockBackupData.Add(fileName);

            return Task.FromResult(Result<string>.Success(fileName));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر إنشاء نسخة احتياطية (محاكاة)");
            return Task.FromResult(Result<string>.Failure("تعذر إنشاء النسخة الاحتياطية. يرجى إعادة المحاولة."));
        }
    }

    /// <summary>قائمة أسماء النسخ الاحتياطية المتاحة — الأحدث أولاً (بلا مسارات).</summary>
    public Task<Result<List<string>>> GetBackupListAsync(CancellationToken ct = default)
    {
        try
        {
            return Task.FromResult(Result<List<string>>.Success(MockBackupData.Snapshot()));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل قائمة النسخ الاحتياطية (محاكاة)");
            return Task.FromResult(Result<List<string>>.Failure(
                "تعذر تحميل قائمة النسخ الاحتياطية. يرجى إعادة المحاولة."));
        }
    }

    /// <summary>استعادة نسخة — ترفض الاسم الفارغ وغير الموجود، وتنجح لغيرهما.</summary>
    public Task<Result> RestoreBackupAsync(string fileName, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return Task.FromResult(Result.Failure(MissingFileNameMessage));

            if (!MockBackupData.Contains(fileName.Trim()))
                return Task.FromResult(Result.Failure(UnknownFileNameMessage));

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر استعادة النسخة الاحتياطية (محاكاة) — الملف {FileName}", fileName);
            return Task.FromResult(Result.Failure("تعذر استعادة النسخة الاحتياطية. يرجى إعادة المحاولة."));
        }
    }
}
