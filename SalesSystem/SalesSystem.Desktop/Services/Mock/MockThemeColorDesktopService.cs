using System.Drawing;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة ألوان النظام — المخزن الوحيد في المرحلة أ (Mock · UI &amp; UX QA).
/// <para>
/// <b>تكافؤ المحاكاة (RULE-N167):</b> ترفض ما سترفضه طبقة الأعمال في المرحلة ب:
/// طلب فارغ · مفتاح مفقود · مفتاح غير معروف · قيمة ليست بصيغة <c>#RRGGBB</c> ·
/// تباين ضعيف بين النص وخلفيته.
/// </para>
/// <para>
/// <b>لا قاعدة تحقق هنا (RULE-N169 · RULE-N172):</b> الفحوص الخمسة ورسائلها العربية
/// وترتيبها وصيغة <c>#RRGGBB</c> وقواعد التباين تعيش في بيتها الواحد
/// <see cref="ThemePalette"/> (<see cref="ThemePalette.ValidateAll"/> /
/// <see cref="ThemePalette.TryApplyAll"/>)؛ هذه الخدمة تستهلكها فقط — فلا نسخة ثانية
/// من القاعدة في أي تنفيذ. والحفظ لا يُطبَّق جزئياً أبداً: تُتحقّق كل المفاتيح أولاً
/// ثم تُطبَّق دفعة واحدة.
/// </para>
/// <para>
/// لا يُستدعى <c>ThemePalette.ApplyToOpenForms()</c> هنا — الشاشة تفعل ذلك بعد نجاح الحفظ.
/// </para>
/// </summary>
public sealed class MockThemeColorDesktopService : IThemeColorDesktopService
{
    public Task<Result<ThemeColorSettings>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var snapshot = ThemePalette.Snapshot();
            var colors = new Dictionary<string, string>(StringComparer.Ordinal);

            // التعريفات هي المصدر الوحيد للقائمة (23 مفتاحاً في ست مجموعات:
            // الشاشة الرئيسية · الأزرار · الصناديق والأشرطة · الشبكة · التحديد والحقل النشط · حالات العرض)
            // — لا قائمة ثانية هنا.
            foreach (var definition in ThemePalette.Definitions)
            {
                Color color;
                if (!snapshot.TryGetValue(definition.Key, out color)
                    && !ThemePalette.TryGet(definition.Key, out color))
                {
                    color = definition.DefaultValue;
                }

                colors[definition.Key] = ThemePalette.ToHex(color);
            }

            return Task.FromResult(Result<ThemeColorSettings>.Success(new ThemeColorSettings(colors)));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قراءة ألوان النظام (محاكاة)");
            return Task.FromResult(Result<ThemeColorSettings>.Failure(
                "تعذر تحميل ألوان النظام. يرجى إعادة المحاولة."));
        }
    }

    public Task<Result> SaveAsync(ThemeColorSettings settings, CancellationToken ct = default)
    {
        try
        {
            var colors = settings?.Colors;

            // (1) كل قواعد التحقق (الطلب الفارغ · المفتاح المفقود · المفتاح المجهول ·
            //     الصيغة · التباين) في بيتها الواحد ThemePalette — لا قاعدة هنا.
            // (2) التطبيق ذرّي: لا يُطبَّق أي لون إن كانت قيمة واحدة غير صالحة،
            //     ورسالة الرفض العربية تأتي من البيت الواحد نفسه.
            string? applyError = ThemePalette.TryApplyAll(colors);
            if (applyError is not null)
                return Task.FromResult(Result.Failure(applyError));

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ ألوان النظام (محاكاة)");
            return Task.FromResult(Result.Failure("تعذر حفظ ألوان النظام. يرجى إعادة المحاولة."));
        }
    }

    public Task<Result> ResetToDefaultsAsync(CancellationToken ct = default)
    {
        try
        {
            ThemePalette.ResetToDefaults();
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر استعادة ألوان النظام الافتراضية (محاكاة)");
            return Task.FromResult(Result.Failure("تعذر استعادة الألوان الافتراضية. يرجى إعادة المحاولة."));
        }
    }
}
