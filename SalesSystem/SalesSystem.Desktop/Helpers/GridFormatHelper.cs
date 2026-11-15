using System.Globalization;
using System.Runtime.CompilerServices;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Helpers;

/// <summary>
/// دلالة تنسيق عمود شبكة — تُقرأ من الشاشة مرة واحدة، فيُشتق منها نمط العرض
/// (<see cref="GridFormatHelper"/>) وعدد المنازل الحيّ عند التصدير (<c>ExportHelper</c>).
/// <para>
/// بديل معلن عن كتابة نص تنسيق حرفي (<c>"N2"</c>/<c>"N3"</c>) داخل الشاشات، فلا يبقى
/// «نوع العمود» لغزاً يُستنتج من نص التنسيق (<c>RULE-N169</c>).
/// </para>
/// </summary>
public enum GridFormatKind
{
    /// <summary>مبلغ مالي — منازل <c>clsFormat.MoneyDecimals</c>.</summary>
    Money = 0,

    /// <summary>كمية (<c>decimal(18,3)</c>) — منازل <c>clsFormat.QuantityDecimals</c>.</summary>
    Quantity = 1,

    /// <summary>عدد/إحصاء — منازل <c>clsFormat.NumberDecimals</c>.</summary>
    Number = 2,

    /// <summary>نسبة مئوية — منازل <c>clsFormat.PercentDecimals</c> مع علامة <c>%</c>.</summary>
    Percent = 3,

    /// <summary>معامل نسبة (1.85) — منازل <c>clsFormat.RatioDecimals</c> بلا ضرب بـ100 ولا تقريب.</summary>
    Ratio = 4,

    /// <summary>تاريخ قصير — الصيغة الحيّة <c>clsFormat.DateShortPattern</c>.</summary>
    DateShort = 5,

    /// <summary>تاريخ رقمي يوم/شهر/سنة — الصيغة الحيّة <c>clsFormat.DateDayMonthYearPattern</c>.</summary>
    DateDayMonthYear = 6,

    /// <summary>تاريخ ووقت — الصيغة الحيّة <c>clsFormat.DateLongPattern</c>.</summary>
    DateLong = 7
}

/// <summary>
/// البيت الواحد لتنسيق أعمدة <see cref="DataGridView"/> (<c>RULE-N169</c>…<c>RULE-N172</c>):
/// الشاشة تعلن <b>الدلالة</b> (<see cref="GridFormatKind"/>) لا نص التنسيق، فيُشتق النمط من
/// <c>clsFormat</c> الحيّ ويُسجَّل في العمود نفسه ليقرأه <c>ExportHelper</c> عند تصدير Excel.
/// <para>
/// ولا يلمس هذا الـHelper إلا <c>DefaultCellStyle.Format</c> في أعضاء التنسيق: لا <c>ReadOnly</c> ولا
/// <c>SelectionMode</c> ولا <c>AllowUserToAddRows</c> ولا <c>DataSource</c> ولا <c>Visible</c>
/// ولا عروض الأعمدة — كل شبكة تعلن سلوكها بنفسها (<c>RULE-N22</c>/<c>RULE-N52</c>).
/// الاستثناء الوحيد هو عضو التلوين <see cref="ApplyBalanceColouring"/>: يرسم <c>ForeColor</c> وحده
/// داخل معالج <c>CellFormatting</c> ولا يغيّر أي خاصية بنيوية في الشبكة ولا في عمودها.
/// </para>
/// <para>
/// العمود غير الموجود لا يرمي استثناءً ولا يُنشأ: شاشات التقارير تُعلن أعمدتها وقت التشغيل،
/// فتُتجاهل الأعمدة الغائبة بصمت ليصلح نفس سطر الشاشة لكل التقارير.
/// </para>
/// <para>
/// الدلالة تُحفظ في جدول ضعيف المراجع (<see cref="ConditionalWeakTable{TKey, TValue}"/>) لا في خاصية <c>Tag</c>
/// — الشاشات تستخدم <c>Tag</c> لأغراضها، ولا يُسرَّب عمود بعد التخلّص من شبكته.
/// </para>
/// </summary>
public static class GridFormatHelper
{
    // الجدول الضعيف (نفس نمط KeyboardHelper): مفتاحه العمود نفسه، فلا تبقى مراجع أعمدة بعد إغلاق شبكتها.
    private static readonly ConditionalWeakTable<DataGridViewColumn, object> _kinds = new();

    // أعمدة التلوين المسجَّلة لكل شبكة (اسم العمود → اسم عمود العتبة أو null) — تمنع تكرار المعالج.
    private static readonly ConditionalWeakTable<DataGridView, Dictionary<string, string?>> _balanceGrids = new();

    /// <summary>
    /// يطبّق تنسيق دلالة واحدة على عمود واحد بالاسم. لا يفعل شيئاً بصمت إذا كانت الشبكة غير صالحة
    /// أو العمود غير معلن بعد (شاشات التقارير) — ولا يضيف عموداً.
    /// </summary>
    /// <param name="grid">الشبكة — <c>null</c> أو المتخلَّص منها يُتجاهل.</param>
    /// <param name="columnName">اسم العمود كما هو في المصمّم/البيانات (لا يُشتق من إدخال المستخدم).</param>
    /// <param name="kind">دلالة التنسيق.</param>
    public static void ApplyColumn(DataGridView grid, string columnName, GridFormatKind kind)
    {
        if (grid is null || grid.IsDisposed || grid.Disposing) return;
        if (string.IsNullOrWhiteSpace(columnName)) return;
        if (grid.Columns.Count == 0 || !grid.Columns.Contains(columnName)) return;

        var column = grid.Columns[columnName];
        if (column is null) return;

        Apply(column, kind);
    }

    /// <summary>
    /// يطبّق دلالة واحدة على مجموعة أعمدة بالاسم — تتجاهل كل عمود غائب بدل إسقاط الدفعة كلها.
    /// </summary>
    /// <param name="grid">الشبكة.</param>
    /// <param name="kind">دلالة التنسيق المشتركة للأعمدة.</param>
    /// <param name="columnNames">أسماء الأعمدة.</param>
    public static void ApplyColumns(DataGridView grid, GridFormatKind kind, params string[] columnNames)
    {
        if (grid is null || grid.IsDisposed || grid.Disposing) return;
        if (columnNames is null || columnNames.Length == 0) return;

        foreach (var columnName in columnNames)
            ApplyColumn(grid, columnName, kind);
    }

    /// <summary>
    /// يلوّن خانة عمود واحد حسب قيمة السطر — <b>البيت الواحد لتلوين «الرصيد»</b> في شاشات التقارير
    /// (<c>RULE-N169</c>…<c>RULE-N172</c>)، بدل نسخة محلية في كل شاشة.
    /// <para>
    /// الوضعان:
    /// <list type="bullet">
    /// <item><b>رصيد</b> (بلا <paramref name="compareToColumnName"/>): القيمة السالبة حمراء
    /// (<see cref="GreenThemeHelper.StatusDanger"/>) والموجبة خضراء (<see cref="GreenThemeHelper.StatusSuccess"/>) والصفر يعود للنمط الموروث.</item>
    /// <item><b>عتبة</b> (بـ<paramref name="compareToColumnName"/>): تُقارن الخانة بقيمة عمود آخر في نفس السطر
    /// (مثل «الكمية الحالية» مقابل «الحد الأدنى»)، وتُلوَّن حمراء عند النقصان فقط، وما عدا ذلك يعود للنمط الموروث
    /// فلا تُلوَّن كل خانات المخزون السليم بالأخضر.</item>
    /// </list>
    /// </para>
    /// <para>
    /// لا يفعل شيئاً بنيوياً: لا <c>ReadOnly</c> ولا <c>SelectionMode</c> ولا <c>DataSource</c> ولا <c>Visible</c>
    /// (<c>RULE-N52</c>)، ولا يضيف عموداً، ويُسجَّل مرة واحدة فقط لكل (شبكة · عمود) حتى لو نادته الشاشة بعد كل تحميل —
    /// فلا تتكرر المعالجات. الألوان من <see cref="GreenThemeHelper"/> فقط (لا قيمة لون حرفية — <c>RULE-N54</c>).
    /// </para>
    /// </summary>
    /// <param name="grid">الشبكة — <c>null</c> أو المتخلَّص منها يُتجاهل.</param>
    /// <param name="columnName">
    /// <b>اسم العمود</b> كما في <c>DataGridView.Columns</c> (لا اسم حقل البيانات — <c>RULE-N179</c>).
    /// </param>
    /// <param name="compareToColumnName">
    /// اسم عمود العتبة في نفس السطر — <c>null</c> يعني تلويناً حسب إشارة القيمة وحدها.
    /// </param>
    public static void ApplyBalanceColouring(DataGridView? grid, string columnName, string? compareToColumnName = null)
    {
        if (grid is null || grid.IsDisposed || grid.Disposing) return;
        if (string.IsNullOrWhiteSpace(columnName)) return;

        var registration = _balanceGrids.GetOrCreateValue(grid);
        if (!registration.TryAdd(columnName, string.IsNullOrWhiteSpace(compareToColumnName) ? null : compareToColumnName))
            return; // مسجَّل مسبقاً — لا معالج ثانٍ لنفس العمود

        // معالج واحد لكل شبكة، يُوصَّل مرة واحدة عند أول تسجيل.
        if (registration.Count == 1) grid.CellFormatting += OnBalanceCellFormatting;
    }

    /// <summary>
    /// يقرأ دلالة التنسيق المسجَّلة على عمود — <c>false</c> إذا لم تُسجَّل له دلالة
    /// (عمود نسّقته الشاشة بنص حرفي أو عمود بلا تنسيق أصلاً).
    /// </summary>
    /// <param name="column">العمود.</param>
    /// <param name="kind">الدلالة المقروءة عند النجاح.</param>
    public static bool TryGetKind(DataGridViewColumn column, out GridFormatKind kind)
    {
        kind = default;

        if (column is null) return false;
        if (!_kinds.TryGetValue(column, out var stored)) return false;
        if (stored is not GridFormatKind registered) return false;

        kind = registered;
        return true;
    }

    /// <summary>
    /// المعالج الوحيد لتلوين الأعمدة المسجَّلة — آمن تماماً: أي خلل في قيمة أو عمود يترك الخانة بنمطها
    /// الموروث ولا يرمي استثناءً داخل دورة الرسم.
    /// </summary>
    private static void OnBalanceCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        try
        {
            if (sender is not DataGridView grid) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (!_balanceGrids.TryGetValue(grid, out var registration)) return;
            if (e.ColumnIndex >= grid.Columns.Count) return;

            var column = grid.Columns[e.ColumnIndex];
            if (column is null || !registration.TryGetValue(column.Name, out var compareToColumnName)) return;

            if (!TryReadDecimal(e.Value, out var value)) return;

            // وضع العتبة: أحمر عند النقصان عن عمود المقارنة فقط (مثل الكمية < الحد الأدنى).
            if (compareToColumnName is not null)
            {
                if (TryReadRowDecimal(grid, e.RowIndex, compareToColumnName, out var threshold) && value < threshold)
                    e.CellStyle.ForeColor = GreenThemeHelper.StatusDanger;
                else
                    RestoreInherited(e.CellStyle);

                return;
            }

            // وضع الرصيد: سالب أحمر · موجب أخضر · صفر يعود للموروث.
            if (value < 0m) e.CellStyle.ForeColor = GreenThemeHelper.StatusDanger;
            else if (value > 0m) e.CellStyle.ForeColor = GreenThemeHelper.StatusSuccess;
            else RestoreInherited(e.CellStyle);
        }
        catch (Exception)
        {
            // التلوين تجميلي: لا يُسقط الشاشة ولا يُظهر رسالة (RULE-N29 — لا تفاصيل تقنية للمستخدم).
        }
    }

    /// <summary>يعيد الخانة إلى نمطها الموروث (لا لون ولا خط محلي).</summary>
    private static void RestoreInherited(DataGridViewCellStyle style)
    {
        style.ForeColor = Color.Empty;
        style.Font = null;
    }

    /// <summary>يقرأ قيمة رقمية من قيمة خانة بأمان — <c>false</c> للنصوص والفارغ.</summary>
    private static bool TryReadDecimal(object? raw, out decimal value)
    {
        value = 0m;
        if (raw is null or DBNull) return false;
        if (raw is decimal direct) { value = direct; return true; }

        if (raw is IConvertible convertible)
        {
            try
            {
                value = convertible.ToDecimal(CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// يقرأ قيمة عمود آخر في نفس السطر بالاسم — والاسم <b>اسم عمود</b> لا اسم حقل بيانات (<c>RULE-N179</c>).
    /// </summary>
    private static bool TryReadRowDecimal(DataGridView grid, int rowIndex, string columnName, out decimal value)
    {
        value = 0m;
        if (rowIndex < 0 || rowIndex >= grid.Rows.Count) return false;
        if (!grid.Columns.Contains(columnName)) return false;

        var cell = grid.Rows[rowIndex].Cells[columnName];
        return cell is not null && TryReadDecimal(cell.Value, out value);
    }

    /// <summary>
    /// نمط العرض المعتمد لدلالة — كل نمط يأتي من <c>clsFormat</c> الحيّ (لا نص تنسيق مكتوب هنا).
    /// </summary>
    private static string PatternFor(GridFormatKind kind) => kind switch
    {
        GridFormatKind.Money => clsFormat.MoneyPattern,
        GridFormatKind.Quantity => clsFormat.QuantityPattern,
        GridFormatKind.Number => clsFormat.NumberPattern,
        GridFormatKind.Percent => clsFormat.PercentPattern,
        GridFormatKind.Ratio => clsFormat.RatioPattern,
        GridFormatKind.DateShort => clsFormat.Get(clsFormat.DateShortPattern),
        GridFormatKind.DateDayMonthYear => clsFormat.Get(clsFormat.DateDayMonthYearPattern),
        GridFormatKind.DateLong => clsFormat.Get(clsFormat.DateLongPattern),
        _ => string.Empty
    };

    /// <summary>
    /// التطبيق الوحيد: يضع النمط في <c>DefaultCellStyle.Format</c> ويسجّل الدلالة.
    /// لا يعدّل شيئاً آخر في العمود (<c>RULE-N52</c>).
    /// </summary>
    private static void Apply(DataGridViewColumn column, GridFormatKind kind)
    {
        var pattern = PatternFor(kind);

        // دلالة مجهولة/بلا نمط: لا نكتب تنسيقاً فارغاً يُلغي تنسيق المصمّم، ولا نسجّل دلالة كاذبة.
        if (pattern.Length == 0) return;

        column.DefaultCellStyle.Format = pattern;
        _kinds.AddOrUpdate(column, kind);
    }
}
