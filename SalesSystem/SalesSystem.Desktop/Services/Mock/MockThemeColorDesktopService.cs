using System.Drawing;
using System.Text.RegularExpressions;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة ألوان النظام — المخزن الوحيد في المرحلة أ (Mock · UI &amp; UX QA).
/// <para>
/// <b>تكافؤ المحاكاة (RULE-N167):</b> ترفض ما سترفضه طبقة الأعمال في المرحلة ب:
/// طلب فارغ · مفتاح مفقود · مفتاح غير معروف · قيمة ليست بصيغة <c>#RRGGBB</c> ·
/// تباين ضعيف بين النص وخلفيته (WCAG). كل الرفض برسالة عربية عبر <see cref="Result.Failure"/>،
/// والحفظ لا يُطبَّق جزئياً أبداً: تُتحقّق كل المفاتيح أولاً ثم تُطبَّق دفعة واحدة.
/// </para>
/// <para>
/// لا يُستدعى <c>ThemePalette.ApplyToOpenForms()</c> هنا — الشاشة تفعل ذلك بعد نجاح الحفظ.
/// </para>
/// </summary>
public sealed class MockThemeColorDesktopService : IThemeColorDesktopService
{
    /// <summary>الصيغة الوحيدة المقبولة لقيمة اللون.</summary>
    private static readonly Regex HexPattern = new("^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled);

    // ── حدود التباين (WCAG) — نفس ما ستفرضه طبقة الأعمال ────────────────────
    private const double MinHeadingContrast = 3.0;   // نص شريط العنوان / رؤوس الأعمدة
    private const double MinBodyContrast = 4.5;      // نص الخلايا / النص المميّز
    private const double MinDisabledContrast = 2.0;  // العنصر المعطّل

    public Task<Result<ThemeColorSettings>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var snapshot = ThemePalette.Snapshot();
            var colors = new Dictionary<string, string>(StringComparer.Ordinal);

            // التعريفات هي المصدر الوحيد للقائمة (23 مفتاحاً) — لا قائمة ثانية هنا.
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
            // (1) لا شيء للحفظ
            if (settings is null || settings.Colors is null || settings.Colors.Count == 0)
                return Task.FromResult(Result.Failure("لا توجد ألوان للحفظ."));

            var definitions = ThemePalette.Definitions;

            // (2) مفتاح من التعريفات مفقود من الطلب
            foreach (var definition in definitions)
            {
                if (!settings.Colors.TryGetValue(definition.Key, out var value)
                    || string.IsNullOrWhiteSpace(value))
                {
                    return Task.FromResult(Result.Failure(
                        $"اللون المطلوب غير موجود: {definition.ArabicLabel} ({definition.Key})."));
                }
            }

            // (3) مفتاح غير معروف (ليس من التعريفات)
            var known = new HashSet<string>(definitions.Select(d => d.Key), StringComparer.Ordinal);
            foreach (var key in settings.Colors.Keys)
            {
                if (!known.Contains(key))
                    return Task.FromResult(Result.Failure($"مفتاح لون غير معروف: {key}."));
            }

            // (4) قيمة ليست بصيغة #RRGGBB
            var parsed = new Dictionary<string, Color>(StringComparer.Ordinal);
            foreach (var definition in definitions)
            {
                string hex = settings.Colors[definition.Key];

                if (!HexPattern.IsMatch(hex) || !ThemePalette.TryParseHex(hex, out var color))
                {
                    return Task.FromResult(Result.Failure(
                        $"قيمة اللون غير صالحة لـ «{definition.ArabicLabel}». استخدم الصيغة #RRGGBB."));
                }

                parsed[definition.Key] = color;
            }

            // (5) تباين ضعيف بين النص وخلفيته
            string? contrastError = ValidateContrast(parsed);
            if (contrastError is not null)
                return Task.FromResult(Result.Failure(contrastError));

            // كل شيء صالح → تطبيق دفعة واحدة (بلا تطبيق جزئي)
            bool allApplied = true;
            foreach (var pair in parsed)
                allApplied &= ThemePalette.TrySet(pair.Key, pair.Value);

            if (!allApplied)
                return Task.FromResult(Result.Failure("تعذر حفظ بعض الألوان. يرجى إعادة المحاولة."));

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

    // ── التباين (WCAG relative luminance) ────────────────────────────────────

    /// <summary>يرجع أول رسالة رفض للتباين، أو <c>null</c> إن كانت كل الأزواج مقبولة.</summary>
    private static string? ValidateContrast(IReadOnlyDictionary<string, Color> colors)
    {
        if (ContrastRatio(colors, ThemePalette.HeaderTextColor, ThemePalette.HeaderBarColor) < MinHeadingContrast)
            return "تباين نص شريط العنوان مع خلفيته ضعيف — اختر لوناً أغمق أو أفتح.";

        if (ContrastRatio(colors, ThemePalette.GridHeaderTextColor, ThemePalette.GridHeaderColor) < MinHeadingContrast)
            return "تباين نص رؤوس الأعمدة مع خلفيتها ضعيف — اختر لوناً أغمق أو أفتح.";

        if (ContrastRatio(colors, ThemePalette.GridCellTextColor, ThemePalette.GridCellBackgroundColor) < MinBodyContrast)
            return "تباين نص الخلايا مع خلفية الشبكة ضعيف — اختر لوناً أغمق أو أفتح.";

        if (ContrastRatio(colors, ThemePalette.TextStrongColor, ThemePalette.FrmMainColor) < MinBodyContrast)
            return "تباين النص المميّز مع خلفية الشاشة ضعيف — اختر لوناً أغمق أو أفتح.";

        if (ContrastRatio(colors, ThemePalette.DisabledForeColor, ThemePalette.DisabledBackColor) < MinDisabledContrast)
            return "تباين نص العنصر المعطّل مع خلفيته ضعيف.";

        return null;
    }

    private static double ContrastRatio(IReadOnlyDictionary<string, Color> colors, string textKey, string backKey)
    {
        Color text = colors.TryGetValue(textKey, out var t) ? t : Color.Black;
        Color back = colors.TryGetValue(backKey, out var b) ? b : Color.White;

        double textLum = RelativeLuminance(text);
        double backLum = RelativeLuminance(back);

        double lighter = Math.Max(textLum, backLum);
        double darker = Math.Min(textLum, backLum);

        return (lighter + 0.05) / (darker + 0.05);
    }

    /// <summary>الإضاءة النسبية للون وفق WCAG 2.x (0 = أسود · 1 = أبيض).</summary>
    private static double RelativeLuminance(Color color)
        => 0.2126 * LinearChannel(color.R)
         + 0.7152 * LinearChannel(color.G)
         + 0.0722 * LinearChannel(color.B);

    private static double LinearChannel(byte value)
    {
        double channel = value / 255d;
        return channel <= 0.03928
            ? channel / 12.92
            : Math.Pow((channel + 0.055) / 1.055, 2.4);
    }
}
