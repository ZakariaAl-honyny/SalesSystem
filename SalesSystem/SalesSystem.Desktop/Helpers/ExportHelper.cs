using System.Data;
using System.Text;
using ClosedXML.Excel;

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
                cell.Style.NumberFormat.Format = "yyyy-mm-dd";
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
    /// يحوّل تنسيق عمود WinForms (N2/F2/N3/F3…) إلى تنسيق Excel صالح —
    /// تنسيق WinForms لا يُمرَّر إلى Excel مباشرة لأنه غير صالح هناك.
    /// </summary>
    private static string ResolveNumberFormat(string? winFormsFormat)
    {
        var format = (winFormsFormat ?? string.Empty).Trim().ToUpperInvariant();
        return format is "N3" or "F3" ? "#,##0.000" : "#,##0.00";
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

