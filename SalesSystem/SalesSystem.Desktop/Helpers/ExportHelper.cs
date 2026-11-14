using System.Data;
using System.Globalization;
using System.Text;
using ClosedXML.Excel;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Helpers;

public static class ExportHelper
{
    public static void ExportToExcel(DataGridView dgv, string sheetName, string fileName)
    {
        if (dgv is null || dgv.IsDisposed || dgv.Disposing) return;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);

        // Headers
        int colIndex = 1;
        for (int i = 0; i < dgv.Columns.Count; i++)
        {
            if (!dgv.Columns[i].Visible) continue;
            worksheet.Cell(1, colIndex).Value = dgv.Columns[i].HeaderText;
            worksheet.Cell(1, colIndex).Style.Font.Bold = true;
            worksheet.Cell(1, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
            colIndex++;
        }

        int visibleColumns = colIndex - 1;

        // Data — typed values (number/date/bool) so Excel does not receive text
        for (int r = 0; r < dgv.Rows.Count; r++)
        {
            colIndex = 1;
            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                if (!dgv.Columns[c].Visible) continue;
                var val = dgv.Rows[r].Cells[c].Value;
                WriteValue(worksheet.Cell(r + 2, colIndex), val, dgv.Columns[c].DefaultCellStyle?.Format);
                colIndex++;
            }
        }

        if (visibleColumns > 0) worksheet.Columns().AdjustToContents();
        workbook.SaveAs(fileName);
    }

    /// <summary>
    /// يكتب قيمة الخلية بنوعها الحقيقي (رقم/تاريخ/منطقي) بدل تحويل كل شيء إلى نص،
    /// ويطبّق تنسيق العرض المناسب للعمود. القيمة الفارغة تُترك فارغة ولا تُكتب كنص فارغ.
    /// </summary>
    private static void WriteValue(IXLCell cell, object? value, string? winFormsFormat)
    {
        switch (value)
        {
            case null:
            case DBNull:
                return; // خلية فارغة — كتابة "" تجعلها نصاً

            case decimal dec:
                cell.Value = dec;
                cell.Style.NumberFormat.Format = ResolveNumberFormat(winFormsFormat);
                return;

            case double dbl:
                cell.Value = dbl;
                cell.Style.NumberFormat.Format = ResolveNumberFormat(winFormsFormat);
                return;

            case float flt:
                cell.Value = (double)flt;
                cell.Style.NumberFormat.Format = ResolveNumberFormat(winFormsFormat);
                return;

            case int i:
                cell.Value = i;
                return;

            case long l:
                cell.Value = l;
                return;

            case short sh:
                cell.Value = (int)sh;
                return;

            case byte by:
                cell.Value = (int)by;
                return;

            case DateTime dt:
                cell.Value = dt;
                cell.Style.NumberFormat.Format = ResolveDateNumberFormat();
                return;

            case bool flag:
                cell.Value = flag;
                return;

            default:
                cell.Value = value?.ToString() ?? string.Empty;
                return;
        }
    }

    /// <summary>
    /// يحوّل تنسيق عمود WinForms (N2/F2/N3/F3…) إلى تنسيق Excel صالح — وفي الوقت نفسه يقرأ
    /// <b>عدد المنازل الحيّ</b> من إعدادات التنسيق (<c>clsFormat</c>) فلا يثبّت 2 أو 3 منازل (RULE-N179).
    /// تنسيق WinForms لا يُمرَّر إلى Excel مباشرة لأنه غير صالح هناك.
    /// </summary>
    private static string ResolveNumberFormat(string? winFormsFormat)
    {
        var format = (winFormsFormat ?? string.Empty).Trim().ToUpperInvariant();

        // نوع العمود (كمية/مبلغ) يحدّده تنسيق العمود، وعدد المنازل يأتي من الإعداد الحيّ لا من هنا.
        var decimalsKey = format is "N3" or "F3" ? clsFormat.QuantityDecimals : clsFormat.MoneyDecimals;
        var decimals = LiveDecimals(decimalsKey);

        // صيغة Excel تُخزَّن دائماً بفاصل آلاف «,» وفاصل كسر «.»، وExcel يعرضها بحسب لغة الجهاز؛
        // لذلك تُبنى من إعداد «فاصل الآلاف» الحيّ فقط، ولا تُبنى من «فاصل الكسر» الحيّ (رمز مختلفة الدلالة).
        var group = clsFormat.IsOn(clsFormat.Get(clsFormat.ThousandsSeparator)) ? "," : string.Empty;
        var body = "#" + group + "##0";

        return decimals <= 0 ? body : body + "." + new string('0', decimals);
    }

    /// <summary>
    /// عدد المنازل الحيّ لمفتاح تنسيق — يُقرأ من <c>clsFormat</c>، ومداه وافتراضيه من تعريفه الواحد
    /// (<c>clsFormat.Definitions</c>) بلا نسخة ثانية للمدى هنا (RULE-N169).
    /// </summary>
    private static int LiveDecimals(string key)
    {
        var definition = clsFormat.Definitions.FirstOrDefault(d => string.Equals(d.Key, key, StringComparison.Ordinal));

        if (!int.TryParse(clsFormat.Get(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var decimals))
        {
            decimals = definition is not null
                && int.TryParse(definition.DefaultValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var fallback)
                    ? fallback
                    : 0;
        }

        return definition is null ? decimals : Math.Clamp(decimals, definition.Min, definition.Max);
    }

    // احتياطي أخير — لا يقرأ clsFormat (GAP-20)
    private const string LastResortDateFormat = "yyyy-mm-dd";

    /// <summary>
    /// يبني صيغة تاريخ Excel من صيغة <c>clsFormat</c> («التاريخ القصير») بدل تثبيت صيغة محلية (RULE-N179):
    /// المسار الأول هو الصيغة الحيّة، ثم افتراضي <c>clsFormat</c> نفسه من تعريفه الواحد
    /// (<c>clsFormat.Definitions</c>)، ولا يُستخدم الثابت إلا إذا فشل التعيين في الحالتين معاً.
    /// </summary>
    private static string ResolveDateNumberFormat()
    {
        // (1) الصيغة الحيّة — المسار الأساسي
        var mapped = MapExcelDatePattern(clsFormat.Get(clsFormat.DateShortPattern));
        if (mapped is not null) return mapped;

        // (2) افتراضي clsFormat نفسه (من تعريفه الواحد) — لا قيمة ثانية خارج البيت
        var definition = clsFormat.Definitions.FirstOrDefault(
            d => string.Equals(d.Key, clsFormat.DateShortPattern, StringComparison.Ordinal));

        mapped = MapExcelDatePattern(definition?.DefaultValue);
        if (mapped is not null) return mapped;

        // (3) احتياطي أخير — لا يقرأ clsFormat (GAP-20)
        return LastResortDateFormat;
    }

    /// <summary>
    /// التعيين الوحيد من صيغة تاريخ <c>clsFormat</c> إلى صيغة Excel — <c>null</c> = لا مقابل موثوق.
    /// تُقبل رموز اليوم/الشهر/السنة فقط (d · M · y والفواصل)، وأي رمز آخر (وقت · نصوص · هروب) يُرجِع
    /// <c>null</c>. يُستهلك من <see cref="ResolveDateNumberFormat"/> في المسارين معاً فلا نسخة ثانية (RULE-N169).
    /// </summary>
    private static string? MapExcelDatePattern(string? pattern)
    {
        pattern = (pattern ?? string.Empty).Trim();
        if (pattern.Length == 0) return null;

        var excel = new StringBuilder(pattern.Length);
        var index = 0;

        while (index < pattern.Length)
        {
            var token = pattern[index];
            var length = 0;
            while (index < pattern.Length && pattern[index] == token)
            {
                index++;
                length++;
            }

            var mapped = (string?)null;

            if (token == 'd' && length is >= 1 and <= 4)
            {
                mapped = length switch { 1 => "d", 2 => "dd", 3 => "ddd", _ => "dddd" };
            }
            else if (token == 'M')
            {
                // اسم الشهر المختصر (MMM) لا مقابل موثوقاً له بلا كسر معناه ⇒ شهر رقمي بمنزلتين.
                mapped = length == 1 ? "m" : "mm";
            }
            else if (token == 'y' && length is 2 or 4)
            {
                mapped = length == 2 ? "yy" : "yyyy";
            }
            else if (token is '/' or '-' or '.' or ' ')
            {
                mapped = new string(token, length);
            }

            if (mapped is null) return null;

            excel.Append(mapped);
        }

        return excel.Length == 0 ? null : excel.ToString();
    }

    public static void ExportToCsv(DataGridView dgv, string fileName)
    {
        var sb = new StringBuilder();
        var headers = new List<string>();

        for (int i = 0; i < dgv.Columns.Count; i++)
        {
            if (!dgv.Columns[i].Visible) continue;
            headers.Add(EscapeCsv(dgv.Columns[i].HeaderText));
        }
        sb.AppendLine(string.Join(",", headers));

        for (int r = 0; r < dgv.Rows.Count; r++)
        {
            var rowValues = new List<string>();
            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                if (!dgv.Columns[c].Visible) continue;
                var val = dgv.Rows[r].Cells[c].Value;
                rowValues.Add(EscapeCsv(val?.ToString() ?? ""));
            }
            sb.AppendLine(string.Join(",", rowValues));
        }

        File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
    }

    private static string EscapeCsv(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        if (text.Contains(",") || text.Contains("\"") || text.Contains("\n") || text.Contains("\r"))
        {
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }
        return text;
    }
}

