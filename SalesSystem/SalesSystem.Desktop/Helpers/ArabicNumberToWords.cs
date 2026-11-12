// -----------------------------------------------------------------------------
// ArabicNumberToWords.cs — أداة تحويل المبالغ الرقمية إلى نص عربي (تفقيط)
//
// الاستخدام:
//     using SalesSystem.Desktop.Helpers;
//
//     string text = ArabicNumberToWords.ToWords(24500.50m);
//     // "أربعة وعشرون ألفاً وخمسمائة ريال يمني وخمسون فلساً فقط"
//
//     string usd = ArabicNumberToWords.ToWords(1500m, "دولار أمريكي", "سنت", appendOnly: false);
//     // "ألف وخمسمائة دولار أمريكي"
//
// ملاحظات:
//   * الأداة ثابتة (static) وعديمة الحالة (stateless) وآمنة للاستخدام من أكثر من
//     خيط تنفيذ في نفس الوقت (thread-safe) — لا تحتفظ بأي حالة بين الاستدعاءات.
//   * لا تعتمد على أي شيء خارج مكتبة .NET الأساسية (BCL) ولا على الثقافة الحالية
//     (InvariantCulture فقط)، لذا النتيجة واحدة على أي جهاز.
//   * تُستخدم في شاشة فاتورة البيع وفي قوالب الطباعة لعرض المبلغ كتابةً.
//   * المدى المدعوم: أقل من 1,000,000,000,000 (ترليون) — وما فوقه يُعاد كأرقام.
// -----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// أداة تفقيط: تحوّل المبلغ الرقمي إلى نص عربي فصيح مع اسم العملة والوحدة الفرعية.
/// </summary>
public static class ArabicNumberToWords
{
    /// <summary>الحد الأعلى المدعوم (حصراً): 1,000,000,000,000 — ما فوقه يُعاد كأرقام.</summary>
    private const decimal MaxSupported = 1_000_000_000_000m;

    /// <summary>الأعداد المفردة 0..19 (المذكر).</summary>
    private static readonly string[] Ones =
    [
        "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة",
        "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر",
        "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر"
    ];

    /// <summary>صيغ العشرات 20..90 (الفهارس 0 و1 غير مستخدمة).</summary>
    private static readonly string[] Tens =
    [
        "", "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون"
    ];

    /// <summary>صيغ المئات 100..900 (الفهرس صفر غير مستخدم).</summary>
    private static readonly string[] Hundreds =
    [
        "", "مئة", "مئتان", "ثلاثمئة", "أربعمئة", "خمسمئة", "ستمئة", "سبعمئة", "ثمانمئة", "تسعمئة"
    ];

    /// <summary>
    /// صيغ العدد المؤنث 3..10 — تُستخدم مع الأسماء المذكرة (مليون / مليار)
    /// وفق قاعدة مخالفة العدد للمعدود: "ثلاث ملايين"، "خمس مليارات".
    /// </summary>
    private static readonly string[] FeminineCount =
    [
        "", "", "", "ثلاث", "أربع", "خمس", "ست", "سبع", "ثمان", "تسع", "عشر"
    ];

    /// <summary>المفرد: ألف / مليون / مليار (الفهرس = مرتبة المجموعة).</summary>
    private static readonly string[] ScaleSingular = ["", "ألف", "مليون", "مليار"];

    /// <summary>المثنى: ألفان / مليونان / ملياران.</summary>
    private static readonly string[] ScaleDual = ["", "ألفان", "مليونان", "ملياران"];

    /// <summary>جمع القلة 3..10: آلاف / ملايين / مليارات.</summary>
    private static readonly string[] ScalePlural = ["", "آلاف", "ملايين", "مليارات"];

    /// <summary>التمييز المفرد المنصوب 11 وأكثر: ألفاً / مليوناً / ملياراً.</summary>
    private static readonly string[] ScaleAccusative = ["", "ألفاً", "مليوناً", "ملياراً"];

    /// <summary>جمع الوحدات الفرعية المعروفة (لتفادي صيغ جمع خاطئة).</summary>
    private static readonly Dictionary<string, string> SubUnitPlurals = new(StringComparer.Ordinal)
    {
        ["فلس"] = "فلوس",
        ["هللة"] = "هللات",
        ["قرش"] = "قروش",
        ["سنت"] = "سنتات",
        ["سنتيم"] = "سنتيمات",
        ["درهم"] = "دراهم",
        ["دينار"] = "دنانير",
        ["ليرة"] = "ليرات",
        ["جنيه"] = "جنيهات",
        ["ريال"] = "ريالات",
        ["بيسة"] = "بيسات"
    };

    /// <summary>يحوّل المبلغ إلى نص عربي. مثال: 24500.50 → "أربعة وعشرون ألفاً وخمسمائة ريال يمني وخمسون فلساً فقط"</summary>
    /// <param name="amount">المبلغ. القيم السالبة تُسبق بكلمة "سالب".</param>
    /// <param name="currency">اسم العملة كما يُكتب بعد الجزء الصحيح (افتراضي: ريال يمني).</param>
    /// <param name="subUnit">اسم الوحدة الفرعية (افتراضي: فلس).</param>
    /// <param name="appendOnly">عند <c>true</c> تُضاف كلمة "فقط" في نهاية النص.</param>
    /// <returns>النص العربي للمبلغ، أو الأرقام كما هي إذا تجاوز المدى المدعوم.</returns>
    public static string ToWords(decimal amount, string currency = "ريال يمني", string subUnit = "فلس", bool appendOnly = true)
    {
        // خارج المدى المدعوم (أو خطر تجاوز سعة decimal عند القيمة المطلقة) → إرجاع الأرقام.
        if (amount >= MaxSupported || amount <= -MaxSupported)
            return amount.ToString("0.##", CultureInfo.InvariantCulture);

        var isNegative = amount < 0m;
        var absolute = decimal.Round(decimal.Abs(amount), 2, MidpointRounding.AwayFromZero);

        if (absolute >= MaxSupported)
            return amount.ToString("0.##", CultureInfo.InvariantCulture);

        var whole = decimal.Truncate(absolute);
        var fraction = (int)decimal.Round((absolute - whole) * 100m, 0, MidpointRounding.AwayFromZero);

        var sb = new StringBuilder(128);

        if (isNegative)
            sb.Append("سالب ");

        if (whole == 0m)
            sb.Append("صفر");
        else
            AppendGrouped(sb, (long)whole);

        if (!string.IsNullOrWhiteSpace(currency))
            sb.Append(' ').Append(currency.Trim());

        if (fraction > 0)
        {
            sb.Append(" و");
            AppendFraction(sb, fraction, subUnit);
        }

        if (appendOnly)
            sb.Append(" فقط");

        return sb.ToString();
    }

    /// <summary>يكتب الجزء الصحيح (مجموعات من ثلاث خانات) مفصولة بـ "و".</summary>
    private static void AppendGrouped(StringBuilder sb, long value)
    {
        // [0] المليارات، [1] الملايين، [2] الآلاف، [3] الآحاد
        Span<int> groups = stackalloc int[4];
        groups[0] = (int)(value / 1_000_000_000L);
        groups[1] = (int)(value / 1_000_000L % 1000);
        groups[2] = (int)(value / 1_000L % 1000);
        groups[3] = (int)(value % 1000);

        var first = true;
        for (var i = 0; i < groups.Length; i++)
        {
            var group = groups[i];
            if (group == 0)
                continue;

            if (!first)
                sb.Append(" و");

            AppendGroupWithScale(sb, group, 3 - i);
            first = false;
        }
    }

    /// <summary>يكتب مجموعة (1..999) مع لاحقة المرتبة (آحاد/آلاف/ملايين/مليارات).</summary>
    private static void AppendGroupWithScale(StringBuilder sb, int group, int scale)
    {
        if (scale == 0)
        {
            AppendBelowThousand(sb, group);
            return;
        }

        if (group == 1)
        {
            sb.Append(ScaleSingular[scale]);
            return;
        }

        if (group == 2)
        {
            sb.Append(ScaleDual[scale]);
            return;
        }

        if (group <= 10)
        {
            // ألف مؤنثة → "ثلاثة آلاف" (عدد مذكر)، مليون/مليار مذكران → "ثلاث ملايين".
            sb.Append(scale == 1 ? Ones[group] : FeminineCount[group]);
            sb.Append(' ').Append(ScalePlural[scale]);
            return;
        }

        AppendBelowThousand(sb, group);
        sb.Append(' ').Append(ScaleAccusative[scale]);
    }

    /// <summary>يكتب عدداً بين 1 و999 (مئات + "و" + الباقي).</summary>
    private static void AppendBelowThousand(StringBuilder sb, int number)
    {
        if (number <= 0)
            return;

        var hundreds = number / 100;
        var rest = number % 100;

        if (hundreds > 0)
            sb.Append(Hundreds[hundreds]);

        if (rest > 0)
        {
            if (hundreds > 0)
                sb.Append(" و");

            AppendBelowHundred(sb, rest);
        }
    }

    /// <summary>يكتب عدداً بين 1 و99.</summary>
    private static void AppendBelowHundred(StringBuilder sb, int number)
    {
        if (number <= 0)
            return;

        if (number < 20)
        {
            sb.Append(Ones[number]);
            return;
        }

        var tens = number / 10;
        var unit = number % 10;

        if (unit > 0)
            sb.Append(Ones[unit]).Append(" و").Append(Tens[tens]);
        else
            sb.Append(Tens[tens]);
    }

    /// <summary>يكتب الجزء الكسري (1..99) مع صيغة الوحدة الفرعية المناسبة.</summary>
    private static void AppendFraction(StringBuilder sb, int fraction, string subUnit)
    {
        var unit = (subUnit ?? string.Empty).Trim();

        if (unit.Length == 0)
        {
            AppendBelowHundred(sb, fraction);
            return;
        }

        if (fraction == 1)
        {
            // "فلساً واحداً"
            sb.Append(unit).Append("اً واحداً");
            return;
        }

        if (fraction == 2)
        {
            // "فلسان"
            sb.Append(unit).Append("ان");
            return;
        }

        if (fraction <= 10)
        {
            // "ثلاثة فلوس"
            AppendBelowHundred(sb, fraction);
            sb.Append(' ').Append(GetSubUnitPlural(unit));
            return;
        }

        // 11..99 → تمييز مفرد منصوب: "خمسون فلساً"
        AppendBelowHundred(sb, fraction);
        sb.Append(' ').Append(unit).Append("اً");
    }

    /// <summary>يعيد جمع الوحدة الفرعية (من قاموس معروف أو بصيغة تقريبية).</summary>
    private static string GetSubUnitPlural(string subUnit)
    {
        if (SubUnitPlurals.TryGetValue(subUnit, out var known))
            return known;

        if (subUnit.EndsWith('ة') && subUnit.Length > 1)
            return subUnit[..^1] + "ات";

        return subUnit + "ات";
    }
}
