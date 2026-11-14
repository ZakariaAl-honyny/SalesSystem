// -----------------------------------------------------------------------------
// ColorSuggestionHelper.cs — المساعد الذكي لاقتراح الألوان (RULE-N54a)
//
// يستهلكه شاشة «ألوان النظام» فقط: يقترح ألواناً **مُتحقَّقة التباين** لكل مفتاح
// حتى لا يقبل المستخدم لوناً ترفضه الخدمة الوهمية بعد ذلك (WCAG 2.1).
//
// قواعد ملزمة:
//   • لا شيء هنا يُنشئ عنصر واجهة ولا يقرأ قاعدة بيانات — بيانات صرفة (N-Layer بسيط).
//   • القيم الحيّة والافتراضية وخريطة التباين كلها من ThemePalette وحده
//     (RULE-N54a: ممنوع تكرار المفالتيح أو الافتراضيات في أي ملف آخر).
//   • كل لون تُعيده For/LadderFor يمرّ عبر IsAcceptable ⇒ «الاقتراح لا يُرفض أبداً».
//   • لا تُرمى استثناءات: أي خطأ غير متوقع يُعيد قائمة فارغة (الشاشة تبقى تعمل).
// -----------------------------------------------------------------------------

using System.Drawing;

namespace SalesSystem.Desktop.Helpers;

/// <summary>اقتراح لون باسم عربي مقروء — وحدة عرض واحدة في شاشة ألوان النظام.</summary>
/// <param name="ArabicLabel">التسمية العربية الظاهرة (مثل «أخضر هادئ»).</param>
/// <param name="Color">اللون المقترح (مُتحقَّق التباين لمفتاحه).</param>
public sealed record ColorSuggestion(string ArabicLabel, Color Color);

/// <summary>
/// اقتراحات ألوان جاهزة ومُتحقَّقة لكل مفتاح في <see cref="ThemePalette"/>:
/// لوحات مرشّحة حسب دور المفتاح (خلفية · زر · نص · تحديد · حالة تعطيل)، ثم حلقة
/// تدرّج (فاتح ← غامق) حول أفضل اقتراح، ثم تسمية عربية لأقرب لون معروف.
/// <para>
/// كل اقتراح يمرّ على <see cref="IsAcceptable"/> مقابل الشريك الحيّ للمفتاح من
/// <see cref="ThemePalette.ContrastPartnerColor"/> و<see cref="ThemePalette.MinimumContrastFor"/>،
/// فلا يُعرض على المستخدم لون سترفضه الخدمة الوهمية.
/// </para>
/// </summary>
public static class ColorSuggestionHelper
{
    /// <summary>حد الاقتراحات المسمّاة لكل مفتاح.</summary>
    private const int MaxSuggestions = 8;

    /// <summary>أقل عدد خطوات لاعتبار التدرّج مفيداً.</summary>
    private const int MinLadderSteps = 2;

    /// <summary>متوسط القنوات الذي فوقه يُعدّ اللون «فاتحاً» فيُبنى تدرّجه نحو الغامق.</summary>
    private const int LightBaseAverage = 200;

    // ═════════════════════════════════════════════════════════════════
    //  لوحات المرشّحين (الموطن الوحيد لاقتراحات الألوان — لا تُنسخ في أي شاشة)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>خلفيات هادئة (ناعمة، لا تُتعب العين في مساحات كبيرة).</summary>
    private static readonly ColorSuggestion[] BackgroundPool =
    {
        new("أخضر هادئ",       Color.FromArgb(235, 245, 235)),
        new("أزرق فاتح",       Color.FromArgb(222, 235, 247)),
        new("تركوازي فاتح",    Color.FromArgb(224, 242, 241)),
        new("رملي",            Color.FromArgb(250, 244, 227)),
        new("أبيض مائل",       Color.FromArgb(249, 250, 251)),
        new("لافندر فاتح",     Color.FromArgb(238, 235, 248)),
        new("أصفر فاتح",       Color.FromArgb(255, 251, 230)),
        new("رمادي فاتح",      Color.FromArgb(240, 242, 245)),
    };

    /// <summary>ألوان حيّة (أزرار وشريط عنوان): مشبعة لكن غير صارخة.</summary>
    private static readonly ColorSuggestion[] VividPool =
    {
        new("أخضر زمردي",      Color.FromArgb(16, 150, 96)),
        new("أزرق احترافي",    Color.FromArgb(13, 110, 253)),
        new("تركوازي",         Color.FromArgb(23, 162, 184)),
        new("نيلي",            Color.FromArgb(67, 56, 202)),
        new("برتقالي",         Color.FromArgb(234, 88, 12)),
        new("أحمر هادئ",       Color.FromArgb(220, 53, 69)),
        new("أصفر كهرماني",    Color.FromArgb(255, 193, 7)),
        new("رمادي فولاذي",    Color.FromArgb(100, 116, 139)),
    };

    /// <summary>نصوص على خلفيات: أبيض/أبيض مائل/فحمي/أخضر داكن/أزرق داكن/رمادي داكن/أسود.</summary>
    private static readonly ColorSuggestion[] TextPool =
    {
        new("أبيض",            Color.FromArgb(255, 255, 255)),
        new("أبيض مائل",       Color.FromArgb(250, 250, 250)),
        new("فحمي",            Color.FromArgb(33, 37, 41)),
        new("أخضر داكن",       Color.FromArgb(14, 82, 52)),
        new("أزرق داكن",       Color.FromArgb(12, 44, 84)),
        new("رمادي داكن",      Color.FromArgb(75, 85, 99)),
        new("أسود",            Color.FromArgb(0, 0, 0)),
    };

    /// <summary>ألوان تحديد ناعمة (أصفر/أزرق/أخضر/لافندر/رملي) تُبقي النص مقروءاً.</summary>
    private static readonly ColorSuggestion[] SelectionPool =
    {
        new("أصفر فاتح",       Color.FromArgb(255, 249, 196)),
        new("أزرق فاتح",       Color.FromArgb(222, 235, 247)),
        new("أخضر فاتح",       Color.FromArgb(226, 244, 232)),
        new("لافندر",          Color.FromArgb(235, 232, 248)),
        new("رملي",            Color.FromArgb(255, 244, 214)),
    };

    /// <summary>خلفيات العناصر المعطّلة: رماديات فاتحة فقط.</summary>
    private static readonly ColorSuggestion[] DisabledBackPool =
    {
        new("رمادي فاتح جداً",  Color.FromArgb(238, 238, 238)),
        new("رمادي فاتح",       Color.FromArgb(232, 232, 232)),
        new("رمادي مستعمل",    Color.FromArgb(224, 224, 224)),
    };

    /// <summary>نصوص العناصر المعطّلة: رماديات داكنة (تباين كافٍ مع الخلفية الرمادية).</summary>
    private static readonly ColorSuggestion[] DisabledForePool =
    {
        new("رمادي دخاني",     Color.FromArgb(128, 128, 128)),
        new("رمادي داكن",      Color.FromArgb(110, 110, 110)),
        new("فحمي هادئ",       Color.FromArgb(96, 96, 96)),
    };

    /// <summary>الألوان المعروفة بأسمائها العربية — تُستخدم في <see cref="ArabicNameFor"/>.</summary>
    private static readonly (string Name, Color Value)[] KnownNames =
    {
        ("أبيض",            Color.FromArgb(255, 255, 255)),
        ("أبيض مائل",       Color.FromArgb(249, 250, 251)),
        ("فضي",             Color.FromArgb(200, 200, 205)),
        ("رمادي فاتح",      Color.FromArgb(224, 224, 224)),
        ("رمادي",           Color.FromArgb(150, 150, 150)),
        ("رمادي داكن",      Color.FromArgb(96, 96, 96)),
        ("فحمي",            Color.FromArgb(33, 37, 41)),
        ("أسود",            Color.FromArgb(0, 0, 0)),
        ("أحمر",            Color.FromArgb(220, 53, 69)),
        ("قرمزي",           Color.FromArgb(153, 27, 30)),
        ("برتقالي",         Color.FromArgb(234, 88, 12)),
        ("كهرماني",         Color.FromArgb(255, 193, 7)),
        ("أصفر",            Color.FromArgb(255, 240, 150)),
        ("زيتوني",          Color.FromArgb(128, 128, 0)),
        ("أخضر فاتح",       Color.FromArgb(200, 235, 205)),
        ("أخضر",            Color.FromArgb(14, 165, 103)),
        ("أخضر زمردي",      Color.FromArgb(16, 150, 96)),
        ("أخضر داكن",       Color.FromArgb(14, 82, 52)),
        ("تركوازي",         Color.FromArgb(23, 162, 184)),
        ("سماوي",           Color.FromArgb(135, 206, 235)),
        ("أزرق فاتح",       Color.FromArgb(222, 235, 247)),
        ("أزرق",            Color.FromArgb(13, 110, 253)),
        ("أزرق داكن",       Color.FromArgb(12, 44, 84)),
        ("نيلي",            Color.FromArgb(67, 56, 202)),
        ("بنفسجي",          Color.FromArgb(139, 92, 246)),
        ("لافندر",          Color.FromArgb(235, 232, 248)),
        ("بني",             Color.FromArgb(120, 72, 40)),
        ("رملي",            Color.FromArgb(250, 244, 227)),
        ("وردي",            Color.FromArgb(244, 170, 200)),
    };

    // ═════════════════════════════════════════════════════════════════
    //  الواجهة العامة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// اقتراحات مسمّاة للمفتاح حسب دوره، **كلها مقبولة** مقابل الشريك الحيّ
    /// (بحد أقصى <see cref="MaxSuggestions"/>). تُعيد قائمة فارغة للمفتاح غير المتوقّع
    /// عند حدوث خطأ غير متوقع فقط.
    /// </summary>
    public static IReadOnlyList<ColorSuggestion> For(string key)
    {
        try
        {
            return Filter(key, PoolFor(key), MaxSuggestions);
        }
        catch
        {
            return Array.Empty<ColorSuggestion>();
        }
    }

    /// <summary>
    /// تدرّج من خمس خطوات حول <see cref="BestFor"/> (أو القيمة الحيّة إن لا اقتراح):
    /// فاتح ← الأساسي ← غامق، مع تسمية كل خطوة «أفتح ١/٢» · «الأساسي» · «أغمق ١/٢».
    /// تُعيد قائمة فارغة إذا لم ينجُ خطوتان على الأقل من التحقق.
    /// </summary>
    public static IReadOnlyList<ColorSuggestion> LadderFor(string key)
    {
        try
        {
            var baseColor = BestFor(key) ?? ThemePalette.Get(key);
            int baseAverage = AverageChannel(baseColor);

            var steps = BuildSteps(baseColor);
            var result = new List<ColorSuggestion>(steps.Length);
            var seen = new HashSet<int>();
            int lighter = 0;
            int darker = 0;

            foreach (var step in steps)
            {
                if (!seen.Add(step.ToArgb())) continue;
                if (!IsAcceptable(key, step)) continue;

                int average = AverageChannel(step);
                string label = average > baseAverage
                    ? $"أفتح {ToArabicDigit(++lighter)}"
                    : average < baseAverage
                        ? $"أغمق {ToArabicDigit(++darker)}"
                        : "الأساسي";

                result.Add(new ColorSuggestion(label, step));
            }

            return result.Count < MinLadderSteps ? Array.Empty<ColorSuggestion>() : result;
        }
        catch
        {
            return Array.Empty<ColorSuggestion>();
        }
    }

    /// <summary>أول اقتراح مقبول للمفتاح، أو <c>null</c> إن لا اقتراح (مفتاح بلا شرط لا يُعيد null).</summary>
    public static Color? BestFor(string key)
    {
        var suggestions = For(key);

        return suggestions.Count > 0 ? suggestions[0].Color : null;
    }

    /// <summary>
    /// هل يقبل المفتاح هذا اللون؟ المفتاح بلا شريك تباين ⇒ مقبول دائماً؛ وإلا فيجب
    /// أن تبلغ النسبة **غير المقرَّبة** (<see cref="ThemePalette.ContrastRatioRaw"/>)
    /// الحدَّ الأدنى للمفتاح — بنفس دقّة مسار الحفظ، فلا يُقترح لون سترفضه الخدمة
    /// الوهمية بسبب فرق تقريب (RULE-N54c).
    /// </summary>
    public static bool IsAcceptable(string key, Color color)
    {
        try
        {
            var partner = ThemePalette.ContrastPartnerColor(key);
            if (partner is null) return true;

            return ThemePalette.ContrastRatioRaw(color, partner.Value) >= ThemePalette.MinimumContrastFor(key);
        }
        catch
        {
            return true;
        }
    }

    /// <summary>
    /// لون النص المقروء فوق الخلفية <paramref name="back"/>: أبيض إذا بلغ تباينه
    /// <see cref="ThemePalette.BodyTextMinimumContrast"/> (نسبة **غير مقرَّبة**)، وإلا
    /// لون نص خلايا الشبكة الافتراضي من اللوحة (<see cref="ThemePalette.GridCellTextColor"/>).
    /// <para>
    /// الموطن الوحيد لهذا الحكم (RULE-N172): شاشة «ألوان النظام» تستدعيها بدل تكرار
    /// عتبة WCAG أو <c>Color.White</c> محلياً، ولا تُعاد صياغة معادلة التباين هنا —
    /// كل الأرقام تُقرأ من <see cref="ThemePalette"/>.
    /// </para>
    /// </summary>
    public static Color ReadableForeFor(Color back)
    {
        try
        {
            return ThemePalette.ContrastRatioRaw(back, Color.White) >= ThemePalette.BodyTextMinimumContrast
                ? Color.White
                : ThemePalette.DefaultFor(ThemePalette.GridCellTextColor);
        }
        catch
        {
            return ThemePalette.DefaultFor(ThemePalette.GridCellTextColor);
        }
    }

    /// <summary>أقرب اسم عربي مقروء للون (مسافة إدراكية موزونة: الأخضر أثقل وزناً).</summary>
    public static string ArabicNameFor(Color color)
    {
        try
        {
            string nearest = "لون مخصّص";
            double nearestDistance = double.MaxValue;

            foreach (var known in KnownNames)
            {
                double distance = Distance(color, known.Value);
                if (distance >= nearestDistance) continue;

                nearestDistance = distance;
                nearest = known.Name;
            }

            return nearest;
        }
        catch
        {
            return "لون مخصّص";
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  الداخل
    // ═════════════════════════════════════════════════════════════════

    /// <summary>لوحة المرشّحين المناسبة لدور المفتاح (خلفية/زر/نص/تحديد/تعطيل).</summary>
    private static ColorSuggestion[] PoolFor(string key) => key switch
    {
        // رؤوس وأشرطة العنوان: ألوان حيّة يقابلها نص.
        ThemePalette.HeaderBarColor or ThemePalette.GridHeaderColor => VividPool,

        // نصوص: تُقاس على خلفيتها الحيّة.
        ThemePalette.HeaderTextColor
            or ThemePalette.GridHeaderTextColor
            or ThemePalette.GridCellTextColor
            or ThemePalette.TextStrongColor => TextPool,

        // التحديد والحقل النشط: ناعمة تُبقي النص مقروءاً.
        ThemePalette.SelectionRowColor or ThemePalette.ActiveFieldColor => SelectionPool,

        // حالات التعطيل: رمادي فاتح للخلفية، ورمادي داكن للنص.
        ThemePalette.DisabledBackColor => DisabledBackPool,
        ThemePalette.DisabledForeColor => DisabledForePool,

        // الأزرار السبعة: ألوان حيّة (لا شرط تباين على الخلفية نفسها).
        ThemePalette.BtnAddNewButtonColor
            or ThemePalette.BtnSaveButtonColor
            or ThemePalette.BtnDeleteButtonColor
            or ThemePalette.BtnCancelButtonColor
            or ThemePalette.BtnInfoButtonColor
            or ThemePalette.BtnWarningButtonColor
            or ThemePalette.BtnSecondaryButtonColor => VividPool,

        // كل الخلفيات الباقية (شاشة · أشرطة · صناديق · شبكة · أسطر متبادلة · خطوط).
        _ => BackgroundPool,
    };

    /// <summary>يُرشِّح لوحة مرشحين: إزالة التكرار + التحقق من التباين + سقف العدد.</summary>
    private static IReadOnlyList<ColorSuggestion> Filter(string key, ColorSuggestion[] pool, int max)
    {
        var result = new List<ColorSuggestion>(max);
        var seen = new HashSet<int>();

        foreach (var suggestion in pool)
        {
            if (result.Count >= max) break;
            if (!seen.Add(suggestion.Color.ToArgb())) continue;
            if (!IsAcceptable(key, suggestion.Color)) continue;

            result.Add(suggestion);
        }

        return result;
    }

    /// <summary>
    /// خمس خطوات تدرّج: حول لون فاتح (متوسط ≥ <see cref="LightBaseAverage"/>) ينزل
    /// تدريجياً نحو الغامق، وحول لون غامق/متوسط يصعد ثم ينزل.
    /// </summary>
    private static Color[] BuildSteps(Color baseColor)
        => AverageChannel(baseColor) >= LightBaseAverage
            ? new[]
            {
                baseColor,
                ControlPaint.Dark(baseColor, 0.14f),
                ControlPaint.Dark(baseColor, 0.30f),
                ControlPaint.Dark(baseColor, 0.46f),
                ControlPaint.Dark(baseColor, 0.62f),
            }
            : new[]
            {
                ControlPaint.Light(baseColor, 0.55f),
                ControlPaint.Light(baseColor, 0.28f),
                baseColor,
                ControlPaint.Dark(baseColor, 0.22f),
                ControlPaint.Dark(baseColor, 0.42f),
            };

    /// <summary>متوسط القنوات (0..255) — مقياس «فتح/غموض» بسيط للترتيب والتسمية.</summary>
    private static int AverageChannel(Color color) => (color.R + color.G + color.B) / 3;

    /// <summary>مسافة إدراكية موزونة بين لونين (الأخضر أثقل، ثم الأزرق، ثم الأحمر).</summary>
    private static double Distance(Color a, Color b)
    {
        int red = a.R - b.R;
        int green = a.G - b.G;
        int blue = a.B - b.B;

        return (2.0 * red * red) + (4.0 * green * green) + (3.0 * blue * blue);
    }

    /// <summary>رقم عربي-هندي للخطوات (١…٥) بلا حساسية للثقافة الحالية.</summary>
    private static string ToArabicDigit(int value) => value switch
    {
        1 => "١",
        2 => "٢",
        3 => "٣",
        4 => "٤",
        5 => "٥",
        _ => value.ToString(System.Globalization.CultureInfo.InvariantCulture),
    };
}
