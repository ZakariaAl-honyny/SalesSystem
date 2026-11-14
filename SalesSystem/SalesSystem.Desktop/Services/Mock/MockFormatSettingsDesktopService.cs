using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة إعدادات التنسيق — المخزن الوحيد في المرحلة أ (Mock · UI &amp; UX QA).
/// <para>
/// <b>تكافؤ المحاكاة (RULE-N167):</b> ترفض ما سترفضه طبقة الأعمال في المرحلة ب:
/// طلب فارغ · مفتاح مفقود · مفتاح غير معروف · قيمة غير صالحة · تعارض بين حقلين.
/// هذه القواعد — وحدودها — تعيش في <c>clsFormat.ValidateAll</c> وحدها، والتطبيق الذرّي في <c>clsFormat.TryApplyAll</c> (RULE-N169…N172)،
/// فلا تُنسخ هنا قاعدة ولا عتبة. كل رفض برسالة عربية عبر <see cref="Result.Failure"/>،
/// والحفظ لا يُطبَّق جزئياً أبداً: تُتحقّق المجموعة أولاً ثم تُطبَّق دفعة واحدة.
/// </para>
/// </summary>
public sealed class MockFormatSettingsDesktopService : IFormatSettingsDesktopService
{
    public Task<Result<FormatSettings>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var snapshot = clsFormat.Snapshot();
            var values = new Dictionary<string, string>(StringComparer.Ordinal);

            // التعريفات هي المصدر الوحيد لقائمة المفاتيح — لا قائمة مفاتيح ثانية هنا.
            foreach (var definition in clsFormat.Definitions)
            {
                values[definition.Key] = snapshot.TryGetValue(definition.Key, out var value)
                    ? value
                    : definition.DefaultValue;
            }

            return Task.FromResult(Result<FormatSettings>.Success(new FormatSettings(values)));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قراءة إعدادات التنسيق (محاكاة)");
            return Task.FromResult(Result<FormatSettings>.Failure(
                "تعذر تحميل إعدادات التنسيق. يرجى إعادة المحاولة."));
        }
    }

    public Task<Result> SaveAsync(FormatSettings settings, CancellationToken ct = default)
    {
        try
        {
            // (1) لا شيء للحفظ
            if (settings is null || settings.Values is null || settings.Values.Count == 0)
                return Task.FromResult(Result.Failure("لا توجد إعدادات تنسيق للحفظ."));

            // (2)+(3) التحقق والتطبيق الذرّي معاً — الدفعة كلها أو لا شيء، وترتيب المفاتيح لا يهم (البيت الواحد)
            string? error = clsFormat.TryApplyAll(settings.Values);
            if (error is not null)
                return Task.FromResult(Result.Failure(error));

            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ إعدادات التنسيق (محاكاة)");
            return Task.FromResult(Result.Failure("تعذر حفظ إعدادات التنسيق. يرجى إعادة المحاولة."));
        }
    }

    public Task<Result> ResetToDefaultsAsync(CancellationToken ct = default)
    {
        try
        {
            clsFormat.ResetToDefaults();
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر استعادة إعدادات التنسيق الافتراضية (محاكاة)");
            return Task.FromResult(Result.Failure(
                "تعذر استعادة إعدادات التنسيق الافتراضية. يرجى إعادة المحاولة."));
        }
    }
}
