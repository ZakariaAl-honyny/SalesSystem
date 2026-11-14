namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// أداة موحّدة لتهريب قيم البحث قبل بنائها في <see cref="System.Data.DataView.RowFilter"/> (RULE-N60).
/// <para>
/// التهريب يُبطل محارف <c>DataView</c> الخاصة كي لا يستطيع نصّ ملصوق (لصق بـ Ctrl+V يتجاوز حجب
/// <c>KeyPress</c>) أن يُسقط الاستعلام بخطأ صيغة أو أن يُوسّع/يُحقن شرطاً في التصفية:
/// <list type="bullet">
/// <item>علامة الاقتباس <c>'</c> تُضاعف فتبقى قيمة نصية لا تُغلق الشرط.</item>
/// <item>حروف البدل <c>%</c> و <c>*</c> تُحوَّل إلى قيم حرفية فلا تُوسّع نطاق البحث.</item>
/// <item>القوسان <c>[</c> و <c>]</c> يُهرَّبان فلا يُفسَّر النص كمجموعة محارف أو كصيغة شاذة.</item>
/// </list>
/// </para>
/// <para>
/// لا تُقبل أسماء الأعمدة من إدخال المستخدم إطلاقاً: كلا الدالتين تأخذ اسم العمود من نصّ ثابت
/// داخل الشاشة (تبديل على <c>SelectedIndex</c>) — التهريب هنا للقيمة وحدها.
/// </para>
/// </summary>
public static class RowFilterHelper
{
    /// <summary>
    /// يُهرّب قيمة بحث واحدة لتُحقن داخل تعبير <see cref="System.Data.DataView.RowFilter"/> بأمان.
    /// </summary>
    /// <param name="value">قيمة المستخدم (قد تكون <c>null</c>).</param>
    /// <returns>القيمة مُهرَّبة، أو <see cref="string.Empty"/> عند <c>null</c> — لا تُعيد <c>null</c> أبداً.</returns>
    public static string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;

        // تمريرة واحدة على المحارف: الاستبدال المتسلسل (Replace ثم Replace) كان سيُهرّب
        // الأقواس التي أضافها الاستبدال السابق نفسه فيُفسد الناتج.
        var builder = new System.Text.StringBuilder(value.Length + 8);

        foreach (char c in value)
        {
            switch (c)
            {
                case '\'': builder.Append("''"); break;
                case '[':  builder.Append("[[]"); break;
                case ']':  builder.Append("[]]"); break;
                case '%':  builder.Append("[%]"); break;
                case '*':  builder.Append("[*]"); break;
                default:   builder.Append(c); break;
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// يبني شرط بحث «يحتوي» على عمود معروف: <c>column LIKE '%value%'</c> مع تهريب القيمة.
    /// </summary>
    /// <param name="columnName">اسم عمود ثابت داخل الكود — لا يُؤخذ من إدخال المستخدم.</param>
    /// <param name="value">قيمة المستخدم (قد تكون <c>null</c>).</param>
    public static string Like(string columnName, string? value)
        => $"{columnName} LIKE '%{Escape(value)}%'";

    /// <summary>
    /// يبني شرط البحث النصي على عمود واحد أو أكثر: عمود واحد ⇒ <see cref="Like"/> مباشرة،
    /// وأكثر من عمود ⇒ مجموعة «أو» بينها. قيمة فارغة ⇒ <see cref="string.Empty"/> (لا جزء تصفية).
    /// </summary>
    /// <param name="value">قيمة المستخدم (تُهرَّب مرة واحدة لكل عمود).</param>
    /// <param name="columns">أسماء أعمدة ثابتة داخل الكود — لا تُؤخذ من إدخال المستخدم.</param>
    public static string Text(string? value, params string[] columns)
    {
        if (string.IsNullOrEmpty(value) || columns.Length == 0) return string.Empty;
        if (columns.Length == 1) return Like(columns[0], value);

        var builder = new System.Text.StringBuilder("(");

        for (int i = 0; i < columns.Length; i++)
        {
            if (i > 0) builder.Append(" OR ");
            builder.Append(Like(columns[i], value));
        }

        return builder.Append(')').ToString();
    }

    /// <summary>شرط مساواة نصية مهرَّبة: <c>column = 'value'</c> — أو <see cref="string.Empty"/> عند قيمة فارغة.</summary>
    /// <param name="columnName">اسم عمود ثابت داخل الكود.</param>
    /// <param name="value">القيمة النصية (قد تكون <c>null</c> = «بلا تصفية»).</param>
    public static string EqualsText(string columnName, string? value)
        => string.IsNullOrEmpty(value) ? string.Empty : $"{columnName} = '{Escape(value)}'";

    /// <summary>شرط مساواة رقمية: <c>column = number</c> (بلا ثقافة جهاز).</summary>
    /// <param name="columnName">اسم عمود ثابت داخل الكود.</param>
    /// <param name="value">قيمة رقمية ثابتة من الكود (حالة/نوع حركة/فهرس اختيار).</param>
    public static string EqualsNumber(string columnName, long value)
        => $"{columnName} = {value.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

    /// <summary>
    /// نطاق زمني شامل على عمود تاريخ (<c>DateTime</c>): <c>column &gt;= #from# AND column &lt;= #to#</c>.
    /// يُهمَل وقت اليوم ويُبادَل الطرفان إذا كان النطاق معكوساً بدل إسقاط النتائج.
    /// </summary>
    /// <param name="columnName">اسم عمود ثابت داخل الكود.</param>
    /// <param name="from">بداية النطاق (يُؤخذ تاريخه فقط).</param>
    /// <param name="to">نهاية النطاق (يُؤخذ تاريخه فقط).</param>
    public static string DateRange(string columnName, DateTime from, DateTime to)
    {
        (from, to) = Normalize(from, to);
        return $"{columnName} >= #{FormatDate(from)}# AND {columnName} <= #{FormatDate(to)}#";
    }

    /// <summary>
    /// نطاق زمني على عمود نصي مخزَّن بصيغة <c>yyyy-MM-dd</c> (أو <c>yyyy-MM-dd HH:mm</c>):
    /// <c>column &gt;= 'from' AND column &lt;= 'to'</c> — ومع <paramref name="exclusiveEnd"/> تصبح النهاية
    /// <c>&lt; to+1</c> فتشمل كل ساعات اليوم الأخير.
    /// يُهمَل وقت اليوم ويُبادَل الطرفان إذا كان النطاق معكوساً.
    /// </summary>
    /// <param name="columnName">اسم عمود ثابت داخل الكود.</param>
    /// <param name="from">بداية النطاق (يُؤخذ تاريخه فقط).</param>
    /// <param name="to">نهاية النطاق (يُؤخذ تاريخه فقط).</param>
    /// <param name="exclusiveEnd">النهاية غير شاملة (يوم كامل بعد <paramref name="to"/>).</param>
    public static string DateRangeText(string columnName, DateTime from, DateTime to, bool exclusiveEnd = false)
    {
        (from, to) = Normalize(from, to);

        string upper = exclusiveEnd ? " < " : " <= ";
        return $"{columnName} >= '{FormatDate(from)}'{upper}'{FormatDate(to.AddDays(exclusiveEnd ? 1 : 0))}'";
    }

    /// <summary>
    /// الشرط النهائي لـ<c>DataView.RowFilter</c>: يتجاهل الأجزاء الفارغة ويجمع الباقي بـ <c>AND</c>،
    /// ويُعيد <see cref="string.Empty"/> (أي «بلا تصفية») إذا لم يبقَ جزء.
    /// </summary>
    /// <param name="parts">أجزاء التصفية (قد تحتوي <c>null</c>/فراغاً من الأجزاء الاختيارية).</param>
    public static string Build(IEnumerable<string?>? parts)
    {
        if (parts is null) return string.Empty;

        var builder = new System.Text.StringBuilder();

        foreach (string? part in parts)
        {
            if (string.IsNullOrWhiteSpace(part)) continue;

            if (builder.Length > 0) builder.Append(" AND ");
            builder.Append(part);
        }

        return builder.ToString();
    }

    /// <summary>يُهمل وقت اليوم ويضمن ترتيب الطرفين (النطاق المعكوس يُبادَل طرفاه).</summary>
    private static (DateTime From, DateTime To) Normalize(DateTime from, DateTime to)
    {
        from = from.Date;
        to = to.Date;
        return from > to ? (to, from) : (from, to);
    }

    /// <summary>صيغة تاريخ ثابتة لا تتأثر بثقافة الجهاز ولا بنوع التقويم.</summary>
    private static string FormatDate(DateTime date)
        => date.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
}
