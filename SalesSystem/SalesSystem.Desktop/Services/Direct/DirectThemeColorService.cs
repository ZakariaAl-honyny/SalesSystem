using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة ألوان النظام — ينادي <see cref="BizInterfaces.IStoreSettingsService"/> داخل نفس العملية.
/// <para>
/// يُخزّن كل لون تحت المفتاح <c>"UI.Color." + ThemePalette.Key</c> في جدول <c>SystemSettings</c>،
/// ويُعيد بناء القاموس الحيّ من <see cref="ThemePalette.Definitions"/> عند القراءة (القيمة المحفوظة إن وُجدت،
/// وإلا الافتراضي من اللوحة). لا يحتوي على أي منطق أعمال — يتحقّق عبر <see cref="ThemePalette.ValidateAll"/>
/// ويطبّق عبر <see cref="ThemePalette.TryApplyAll"/> في البيت الواحد فقط (<c>RULE-N11</c>/<c>RULE-N112</c>).
/// </para>
/// </summary>
public sealed class DirectThemeColorService : IThemeColorDesktopService
{
    /// <summary>بادئة مفاتيح الألوان في جدول <c>SystemSettings</c> — المصدر الوحيد لهذه البادئة.</summary>
    private const string KeyPrefix = "UI.Color.";

    private readonly BizInterfaces.IStoreSettingsService _bll;

    public DirectThemeColorService(BizInterfaces.IStoreSettingsService bll)
    {
        _bll = bll;
    }

    /// <inheritdoc />
    public async Task<Result<ThemeColorSettings>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllSystemSettingsAsync(ct);
            if (!result.IsSuccess)
            {
                return Result<ThemeColorSettings>.Failure(
                    result.Error ?? "تعذر تحميل ألوان النظام.",
                    result.ErrorCode);
            }

            var persisted = result.Value ?? new Dictionary<string, string>(StringComparer.Ordinal);
            var colors = new Dictionary<string, string>(ThemePalette.Definitions.Count, StringComparer.Ordinal);

            foreach (var definition in ThemePalette.Definitions)
            {
                string storageKey = KeyPrefix + definition.Key;

                // القيمة المحفوظة تُقرأ عبر TryParseHex: صف قديم أو معدَّل يدوياً بقيمة ليست
                // لوناً حقيقياً يعود إلى افتراضي اللوحة (وغياب المفتاح = الافتراضي، كما كان).
                colors[definition.Key] = persisted.TryGetValue(storageKey, out var saved)
                    && ThemePalette.TryParseHex(saved, out var savedColor)
                        ? ThemePalette.ToHex(savedColor)
                        : ThemePalette.ToHex(definition.DefaultValue);
            }

            return Result<ThemeColorSettings>.Success(new ThemeColorSettings(colors));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر قراءة ألوان النظام (Direct)");
            return Result<ThemeColorSettings>.Failure("تعذر تحميل ألوان النظام. يرجى إعادة المحاولة.");
        }
    }

    /// <inheritdoc />
    public async Task<Result> SaveAsync(ThemeColorSettings settings, CancellationToken ct = default)
    {
        try
        {
            if (settings is null || settings.Colors is null || settings.Colors.Count == 0)
                return Result.Failure("لا توجد ألوان للحفظ.");

            // (1) التحقق في البيت الواحد — لا إعادة تنفيذ للتحقق هنا
            string? validationError = ThemePalette.ValidateAll(settings.Colors);
            if (validationError is not null)
                return Result.Failure(validationError);

            // (2) بناء القاموس بالبادئة وحفظه في قاعدة البيانات
            var storage = new Dictionary<string, string>(settings.Colors.Count, StringComparer.Ordinal);
            foreach (var pair in settings.Colors)
                storage[KeyPrefix + pair.Key] = pair.Value;

            var persistResult = await _bll.UpdateSystemSettingsAsync(storage, ct);
            if (!persistResult.IsSuccess)
            {
                return Result.Failure(
                    persistResult.Error ?? "تعذر حفظ ألوان النظام.",
                    persistResult.ErrorCode);
            }

            // (3) التطبيق على الحالة الحيّة فقط بعد نجاح الحفظ — العقد string? من البيت الواحد
            string? applyError = ThemePalette.TryApplyAll(settings.Colors);
            if (applyError is not null)
                return Result.Failure(applyError);

            return Result.Success();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ ألوان النظام (Direct)");
            return Result.Failure("تعذر حفظ ألوان النظام. يرجى إعادة المحاولة.");
        }
    }

    /// <inheritdoc />
    public async Task<Result> ResetToDefaultsAsync(CancellationToken ct = default)
    {
        try
        {
            var storage = new Dictionary<string, string>(ThemePalette.Definitions.Count, StringComparer.Ordinal);
            foreach (var definition in ThemePalette.Definitions)
                storage[KeyPrefix + definition.Key] = ThemePalette.ToHex(definition.DefaultValue);

            var persistResult = await _bll.UpdateSystemSettingsAsync(storage, ct);
            if (!persistResult.IsSuccess)
            {
                return Result.Failure(
                    persistResult.Error ?? "تعذر استعادة الألوان الافتراضية.",
                    persistResult.ErrorCode);
            }

            ThemePalette.ResetToDefaults();
            return Result.Success();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر استعادة ألوان النظام الافتراضية (Direct)");
            return Result.Failure("تعذر استعادة الألوان الافتراضية. يرجى إعادة المحاولة.");
        }
    }
}
