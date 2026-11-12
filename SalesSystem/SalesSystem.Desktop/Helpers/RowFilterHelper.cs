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
}
