namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// البيت الواحد لقراءة خلية/قيمة من سطر <see cref="DataGridView"/> باسم الحقل **المنطقي**
/// (<c>RULE-N169</c>…<c>RULE-N172</c>): يحلّ الخلية **بالاسم أولاً** ثم بـ <c>DataPropertyName</c>،
/// لأن المصمّم قد يسمّي العمود <c>colName</c>/<c>colIsActive</c> ويضع <c>DataPropertyName = "Name"/"IsActive"</c>
/// — و<see cref="DataGridViewCellCollection"/> يحلّ بالاسم فقط، فيرمي
/// <c>ArgumentException: Column named X cannot be found</c> فيسقط تحميل الشاشة كاملة.
/// <para>
/// وهو أيضاً البيت الواحد لـ**تلوين السطر بحسب حالة نشاطه**
/// (<see cref="ApplyInactiveRowStyle"/>): الشاشة تطلب «رمّد السطور المعطّلة» بسطر واحد،
/// فلا تقرأ خلية بالاسم الخطأ (سبب سقوط <c>frmCashBoxMaster</c> في
/// <c>Logs/AI_Error_Log_20261115.txt</c>) ولا يبقى سطر وردي عالق بعد إعادة التنشيط.
/// </para>
/// </summary>
/// <remarks>
/// <para>
/// <b>لا يرمي استثناءً أبداً</b> (على نمط أمان <c>RULE-N24</c>): سطر <c>null</c> أو عمود غائب أو
/// <c>DBNull</c> أو نوع قيمة غير متوقّع ⇒ القيمة الافتراضية الممرَّرة، لا انفجار في مسار التحميل.
/// </para>
/// <para>
/// <b>تصحيح لِما كان مكتوباً هنا سابقاً («لا ألوان في هذا الـHelper»):</b> بقيت القاعدة كما هي —
/// <b>لا لون حرفي واحد في هذا الملف</b> — لكن <see cref="ApplyInactiveRowStyle"/> يطبّق لونَي
/// السطر غير النشط <c>GreenThemeHelper.InactiveRowBack</c>/<c>InactiveRowFore</c> المستهلكَين من
/// بيت الألوان الواحد (RULE-N54/RULE-N169) ولا يكتب <c>Color.FromArgb</c> بنفسه.
/// </para>
/// <para>
/// لا رسائل ولا تسجيل ولا تنسيق رقمي: التنسيق ملك <see cref="GridFormatHelper"/>، والألوان ملك
/// <c>GreenThemeHelper</c> — هذا الـHelper يحلّ الخلية، ويُعيد قيمتها، ويطبّق نمط النشاط فقط.
/// </para>
/// </remarks>
public static class GridRowHelper
{
    /// <summary>
    /// يُعيد الخلية المطابقة للحقل: (1) عمود بالاسم <paramref name="field"/>،
    /// (2) أول عمود <c>DataPropertyName</c> مطابق (مقارنة ترتيبية حسّاسة لحالة الأحرف)، وإلا <c>null</c>.
    /// </summary>
    public static DataGridViewCell? Cell(DataGridViewRow? row, string field)
    {
        if (row is null || string.IsNullOrWhiteSpace(field)) return null;

        var grid = row.DataGridView;
        if (grid is null) return null;

        if (grid.Columns[field] is { } byName) return _CellAt(row, byName.Index);

        foreach (DataGridViewColumn column in grid.Columns)
        {
            if (string.Equals(column.DataPropertyName, field, StringComparison.Ordinal))
                return _CellAt(row, column.Index);
        }

        return null;
    }

    /// <summary>قيمة الخلية المحلولة، و<c>DBNull</c> تُعاد <c>null</c>.</summary>
    public static object? Value(DataGridViewRow? row, string field)
    {
        var value = Cell(row, field)?.Value;
        return value is DBNull ? null : value;
    }

    /// <summary>قيمة منطقية للحقل (عمود «نشط») — أي نوع آخر يُعيد <paramref name="fallback"/>.</summary>
    public static bool GetBool(DataGridViewRow? row, string field, bool fallback = false)
        => Value(row, field) is bool value ? value : fallback;

    /// <summary>رقم صحيح للحقل (معرّف السجل) — يُعيد <c>false</c> إن غاب العمود أو كانت القيمة غير رقمية.</summary>
    public static bool TryGetInt(DataGridViewRow? row, string field, out int value)
    {
        value = 0;

        switch (Value(row, field))
        {
            case int i:
                value = i;
                return true;
            case short s:
                value = s;
                return true;
            case long l when l is >= int.MinValue and <= int.MaxValue:
                value = (int)l;
                return true;
            case decimal d when d == decimal.Truncate(d) && d >= int.MinValue && d <= int.MaxValue:
                value = (int)d;
                return true;
            case string text when int.TryParse(text, out var parsed):
                value = parsed;
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// قيمة عشرية للحقل — المال <c>decimal</c> (<c>RULE-001</c>)؛ أي نوع غير رقمي يُعيد <paramref name="fallback"/>.
    /// </summary>
    public static decimal GetDecimal(DataGridViewRow? row, string field, decimal fallback = 0m)
        => Value(row, field) switch
        {
            decimal d => d,
            int i => i,
            long l => l,
            short s => s,
            byte b => b,
            string text when decimal.TryParse(text, out var parsed) => parsed,
            _ => fallback
        };

    /// <summary>نص الحقل — <c>null</c> أو غياب العمود يُعيد <paramref name="fallback"/>.</summary>
    public static string GetText(DataGridViewRow? row, string field, string fallback = "")
        => Value(row, field) switch
        {
            null => fallback,
            string text => text,
            var other => other.ToString() ?? fallback
        };

    // ═════════════════════════════════════════════════════════════════
    //  ApplyInactiveRowStyle — تلوين السطر بحسب حالة نشاطه
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يُرمّد سطور السجلات **غير النشطة** في الشبكة، ويُعيد بقية السطور إلى وراثتها الطبيعية.
    /// تُستهلك من كل شاشة دليل بدل <c>_ApplyRowColoring</c> المحلي (RULE-N169/RULE-N172).
    /// <para>
    /// <b>الحلّ بالحقل المنطقي لا بالاسم الخام:</b> يعتمد على <see cref="Cell"/> نفسه
    /// (اسم العمود أولاً ثم <c>DataPropertyName</c>) — فالشاشة التي عمودها <c>colIsActive</c>
    /// لا تنفجر بـ <c>Column named IsActive cannot be found</c>، ولا يُكتب حلّ ثانٍ للحقل.
    /// </para>
    /// <para>
    /// <b>ترميم الوراثة أولاً ثم الترميد:</b> كل سطر يُعاد إلى <see cref="Color.Empty"/> /
    /// <c>Font = null</c> قبل الحكم عليه، فيمحو لوناً سابقاً (وردي قديم أو ترميد تحميل سابق) —
    /// وهو ما يعيد السطر إلى خلفية الشبكة بعد إعادة التنشيط.
    /// </para>
    /// <para>
    /// الألوان تُوضع على <see cref="DataGridViewRow.DefaultCellStyle"/> لأن <see cref="DataGridViewRow"/>
    /// لا يملك <c>BackColor</c>/<c>ForeColor</c>/<c>Font</c> على مستوى السطر، ولأنه الموضع نفسه
    /// الذي كانت تكتب فيه الشاشات اللون الوردي — فرمّه هنا يمحوه فعلاً.
    /// </para>
    /// </summary>
    /// <param name="grid">الشبكة — <c>null</c> أو المتخلَّص منها أو بلا أعمدة ⇒ لا شيء.</param>
    /// <param name="activeField">الحقل المنطقي لحالة النشاط (اسم عمود أو <c>DataPropertyName</c>).</param>
    public static void ApplyInactiveRowStyle(DataGridView? grid, string activeField = "IsActive")
    {
        if (grid is null || grid.IsDisposed || grid.Disposing) return;
        if (grid.Columns.Count == 0 || string.IsNullOrWhiteSpace(activeField)) return;

        // حارس الحقل الغائب: لا عمود مطابق ⇒ **لا نلمس سطراً واحداً** (لا رمي، ولا ترميد
        // أسطر نشطة بالخطأ). الحلّ نفسه لا يُعاد كتابته هنا: نستأمن Cell على أول سطر بيانات،
        // فهو يحلّ بالاسم أولاً ثم بـ DataPropertyName، والحلّ واحد لكل سطور الشبكة.
        DataGridViewRow? probe = null;
        foreach (DataGridViewRow candidate in grid.Rows)
        {
            if (candidate.IsNewRow) continue;
            probe = candidate;
            break;
        }

        if (probe is null || Cell(probe, activeField) is null) return;

        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.IsNewRow) continue;

            var style = row.DefaultCellStyle;

            // (1) ترميم الوراثة أولاً — يمحو أي لون عالق على السطر.
            style.BackColor = Color.Empty;
            style.ForeColor = Color.Empty;
            style.SelectionForeColor = Color.Empty;
            style.Font = null;

            // (2) الترميد للسجل المعطّل فقط، بألوان البيت الواحد (لا لون حرفي في هذا الملف).
            if (GetBool(row, activeField, true)) continue;

            style.BackColor = GreenThemeHelper.InactiveRowBack;
            style.ForeColor = GreenThemeHelper.InactiveRowFore;
            style.SelectionForeColor = GreenThemeHelper.InactiveRowFore;
        }
    }

    /// <summary>وصول آمن إلى الخلية بالفهرس (حدود مجموعة الخلايا محروسة فلا يرمي).</summary>
    private static DataGridViewCell? _CellAt(DataGridViewRow row, int columnIndex)
        => columnIndex >= 0 && columnIndex < row.Cells.Count ? row.Cells[columnIndex] : null;
}
