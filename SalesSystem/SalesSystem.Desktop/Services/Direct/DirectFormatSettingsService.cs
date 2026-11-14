using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة إعدادات التنسيق — ينادي <see cref="BizInterfaces.IStoreSettingsService"/> داخل نفس العملية.
/// <para>
/// يُخزّن كل إعداد تحت المفتاح <c>"UI.Format." + clsFormat.Key</c> في جدول <c>SystemSettings</c>،
/// ويُعيد بناء القاموس الحيّ من <see cref="clsFormat.Definitions"/> عند القراءة (القيمة المحفوظة إن وُجدت،
/// وإلا الافتراضي من التعريف). لا يحتوي على أي منطق أعمال — يتحقّق عبر <see cref="clsFormat.ValidateAll"/>
/// ويطبّق عبر <see cref="clsFormat.TryApplyAll"/> في البيت الواحد فقط (<c>RULE-N11</c>/<c>RULE-N112</c>).
/// </para>
/// </summary>
public sealed class DirectFormatSettingsService : IFormatSettingsDesktopService
{
    /// <summary>بادئة مفاتيح إعدادات التنسيق في جدول <c>SystemSettings</c> — المصدر الوحيد لهذه البادئة.</summary>
    private const string KeyPrefix = "UI.Format.";

    private readonly BizInterfaces.IStoreSettingsService _bll;

    public DirectFormatSettingsService(BizInterfaces.IStoreSettingsService bll)
    {
        _bll = bll;
    }

    /// <inheritdoc />
    public async Task<Result<FormatSettings>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllSystemSettingsAsync(ct);
            if (!result.IsSuccess)
            {
                return Result<FormatSettings>.Failure(
                    result.Error ?? "تعذر تحميل إعدادات التنسيق.",
                    result.ErrorCode);
            }

            var persisted = result.Value ?? new Dictionary<string, string>(StringComparer.Ordinal);
            var values = new Dictionary<string, string>(clsFormat.Definitions.Count, StringComparer.Ordinal);

            foreach (var definition in clsFormat.Definitions)
            {
                string storageKey = KeyPrefix + definition.Key;

                values[definition.Key] = persisted.TryGetValue(storageKey, out var saved)
                    && !string.IsNullOrWhiteSpace(saved)
                        ? saved
                        : definition.DefaultValue;
            }

            return Result<FormatSettings>.Success(new FormatSettings(values));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قراءة إعدادات التنسيق (Direct)");
            return Result<FormatSettings>.Failure("تعذر تحميل إعدادات التنسيق. يرجى إعادة المحاولة.");
        }
    }

    /// <inheritdoc />
    public async Task<Result> SaveAsync(FormatSettings settings, CancellationToken ct = default)
    {
        try
        {
            if (settings is null || settings.Values is null || settings.Values.Count == 0)
                return Result.Failure("لا توجد إعدادات تنسيق للحفظ.");

            // (1) التحقق في البيت الواحد — لا إعادة تنفيذ للتحقق هنا
            string? validationError = clsFormat.ValidateAll(settings.Values);
            if (validationError is not null)
                return Result.Failure(validationError);

            // (2) بناء القاموس بالبادئة وحفظه في قاعدة البيانات
            var storage = new Dictionary<string, string>(settings.Values.Count, StringComparer.Ordinal);
            foreach (var pair in settings.Values)
                storage[KeyPrefix + pair.Key] = pair.Value;

            var persistResult = await _bll.UpdateSystemSettingsAsync(storage, ct);
            if (!persistResult.IsSuccess)
            {
                return Result.Failure(
                    persistResult.Error ?? "تعذر حفظ إعدادات التنسيق.",
                    persistResult.ErrorCode);
            }

            // (3) التطبيق على الحالة الحيّة فقط بعد نجاح الحفظ
            string? applyError = clsFormat.TryApplyAll(settings.Values);
            if (applyError is not null)
                return Result.Failure(applyError);

            return Result.Success();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ إعدادات التنسيق (Direct)");
            return Result.Failure("تعذر حفظ إعدادات التنسيق. يرجى إعادة المحاولة.");
        }
    }

    /// <inheritdoc />
    public async Task<Result> ResetToDefaultsAsync(CancellationToken ct = default)
    {
        try
        {
            var storage = new Dictionary<string, string>(clsFormat.Definitions.Count, StringComparer.Ordinal);
            foreach (var definition in clsFormat.Definitions)
                storage[KeyPrefix + definition.Key] = definition.DefaultValue;

            var persistResult = await _bll.UpdateSystemSettingsAsync(storage, ct);
            if (!persistResult.IsSuccess)
            {
                return Result.Failure(
                    persistResult.Error ?? "تعذر استعادة إعدادات التنسيق الافتراضية.",
                    persistResult.ErrorCode);
            }

            clsFormat.ResetToDefaults();
            return Result.Success();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر استعادة إعدادات التنسيق الافتراضية (Direct)");
            return Result.Failure("تعذر استعادة إعدادات التنسيق الافتراضية. يرجى إعادة المحاولة.");
        }
    }
}
