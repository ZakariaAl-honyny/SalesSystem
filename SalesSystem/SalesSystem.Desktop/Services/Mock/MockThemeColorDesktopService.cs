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
/// تباين ضعيف بين النص وخلفيته (قواعد التباين وحدوده من <c>ThemePalette</c> — RULE-N112).
/// كل الرفض برسالة عربية عبر <see cref="Result.Failure"/>،
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

    // ── التباين: العتبات تأتي من ThemePalette.MinimumContrastFor — لا ثوابت محلية ──

    public Task<Result<ThemeColorSettings>> GetAsync(CancellationToken ct = default)
    {
        try
        {
            var snapshot = ThemePalette.Snapshot();
            var colors = new Dictionary<string, string>(StringComparer.Ordinal);

            // التعريفات هي المصدر الوحيد للقائمة (24 مفتاحاً في ست مجموعات:
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

    // ── التباين (RULE-N112: القواعد والمفاتيح والحدود من ThemePalette فقط) ───

    /// <summary>
    /// يمرّ على كل تعريفات اللوحة ويطبّق **قاعدة اللوحة نفسها**: لكل مفتاح له حد أدنى
    /// (<see cref="ThemePalette.MinimumContrastFor"/>) وشريك (<see cref="ThemePalette.ContrastPartnerKey"/>)
    /// يُقاس التباين بـ <see cref="ThemePalette.ContrastRatioRaw"/> — لا قاعدة ولا عتبة محلية هنا.
    /// <para>
    /// <b>لماذا الطلب لا القيمة الحيّة:</b> قد تُغيّر المسودة مفتاح النص **و**شريكه الخلفي
    /// في نفس الحفظ، فلا يجوز الحكم على زوج (جديد، قديم) — يُقاس الزوج (جديد، جديد)
    /// من الطلب المقدَّم نفسه؛ والقيمة الحيّة تُستخدم فقط إذا لم يكن الشريك في الطلب
    /// (لا يحدث عملياً لأن كل المفاتيح إلزامية، لكنها تُبقي الدالة آمنة).
    /// </para>
    /// يرجع أول رسالة رفض عربية، أو <c>null</c> إن كانت كل الأزواج مقبولة.
    /// </summary>
    private static string? ValidateContrast(IReadOnlyDictionary<string, Color> colors)
    {
        foreach (var definition in ThemePalette.Definitions)
        {
            double minimum = ThemePalette.MinimumContrastFor(definition.Key);
            if (minimum <= 0) continue;                       // لا متطلب لهذا المفتاح

            if (!colors.TryGetValue(definition.Key, out var candidate)) continue;

            string? partnerKey = ThemePalette.ContrastPartnerKey(definition.Key);
            if (partnerKey is null) continue;                 // لا شريك لهذا المفتاح

            var partner = colors.TryGetValue(partnerKey, out var submitted)
                ? submitted                                   // ★ الزوج (جديد، جديد) — المسودة كاملة
                : ThemePalette.Get(partnerKey);               // احتياط: القيمة الحيّة

            if (ThemePalette.ContrastRatioRaw(candidate, partner) < minimum)
                return _ContrastMessage(definition.Key, definition.ArabicLabel);
        }

        return null;
    }

    /// <summary>
    /// الرسائل العربية الخمس الأصلية مربوطة بمفتاحها؛ وأي مفتاح آخر له متطلب تباين
    /// يأخذ رسالة عامة بنفس الصياغة (لا تُضاف رسالة لكل لون في الشاشة).
    /// </summary>
    private static string _ContrastMessage(string key, string arabicLabel) => key switch
    {
        ThemePalette.HeaderTextColor
            => "تباين نص شريط العنوان مع خلفيته ضعيف — اختر لوناً أغمق أو أفتح.",

        ThemePalette.GridHeaderTextColor
            => "تباين نص رؤوس الأعمدة مع خلفيتها ضعيف — اختر لوناً أغمق أو أفتح.",

        ThemePalette.GridCellTextColor
            => "تباين نص الخلايا مع خلفية الشبكة ضعيف — اختر لوناً أغمق أو أفتح.",

        ThemePalette.TextStrongColor
            => "تباين النص المميّز مع خلفية الشاشة ضعيف — اختر لوناً أغمق أو أفتح.",

        ThemePalette.DisabledForeColor
            => "تباين نص العنصر المعطّل مع خلفيته ضعيف.",

        _ => $"تباين «{arabicLabel}» مع اللون المرتبط به ضعيف — اختر لوناً أغمق أو أفتح."
    };
}
