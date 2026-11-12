using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using SalesSystem.Desktop.Printing.Models;
using SalesSystem.Desktop.Printing.Extensions;

namespace SalesSystem.Desktop.Printing.Core;

public class InvoicePrinter : IInvoicePrinter
{
    private InvoicePrintDto? _invoice;
    private IEnumerable<InvoiceItemPrintDto>? _items;
    private InvoiceTotalsPrintDto? _totals;
    private StoreInfoPrintDto? _storeInfo;

    public void PrintPreview(InvoicePrintDto invoice, IEnumerable<InvoiceItemPrintDto> items, InvoiceTotalsPrintDto totals, StoreInfoPrintDto storeInfo)
    {
        _invoice = invoice;
        _items = items;
        _totals = totals;
        _storeInfo = storeInfo;

        using var pd = new PrintDocument();
        pd.PrintPage += Pd_PrintPage;

        using var preview = new PrintPreviewDialog();
        preview.Document = pd;
        preview.WindowState = FormWindowState.Maximized;
        preview.ShowDialog();
    }

    public void PrintSummaryPreview(InvoicePrintDto invoice, IEnumerable<InvoiceItemPrintDto> items, InvoiceTotalsPrintDto totals, StoreInfoPrintDto storeInfo)
    {
        _invoice = invoice;
        _items = items as IList<InvoiceItemPrintDto> ?? items.ToList();
        _totals = totals;
        _storeInfo = storeInfo;

        using var pd = new PrintDocument();
        pd.PrintPage += Pd_SummaryPrintPage;

        using var preview = new PrintPreviewDialog();
        preview.Document = pd;
        preview.WindowState = FormWindowState.Maximized;
        preview.ShowDialog();
    }

    public void Print(InvoicePrintDto invoice, IEnumerable<InvoiceItemPrintDto> items, InvoiceTotalsPrintDto totals, StoreInfoPrintDto storeInfo, string? printerName = null)
    {
        _invoice = invoice;
        _items = items;
        _totals = totals;
        _storeInfo = storeInfo;

        using var pd = new PrintDocument();
        pd.PrintPage += Pd_PrintPage;
        if (!string.IsNullOrEmpty(printerName))
        {
            pd.PrinterSettings.PrinterName = printerName;
        }
        pd.Print();
    }

    // ------------------------------------------------------------------
    // Shared drawing helpers (used by both the normal and summary layout)
    // ------------------------------------------------------------------

    private static (Font Title, Font Header, Font Body, Font Bold) CreateFonts() => (
        new Font("Segoe UI", 16, FontStyle.Bold),
        new Font("Segoe UI", 12, FontStyle.Bold),
        new Font("Segoe UI", 10),
        new Font("Segoe UI", 10, FontStyle.Bold));

    private void DrawStoreHeader(Graphics g, RectangleF margin, ref float y,
        (Font Title, Font Header, Font Body, Font Bold) fonts)
    {
        var logo = PrintHelper.LoadLogo(_storeInfo!.LogoPath);
        if (logo != null)
        {
            g.DrawImage(logo, margin.Right - 100, y, 100, 100);
        }

        g.DrawString(_storeInfo.StoreName, fonts.Title, Brushes.Black, new RectangleF(margin.Left, y, margin.Width - 110, 35), PrintHelper.RTLFormatRight);
        y += 35;
        g.DrawString(_storeInfo.Address, fonts.Body, Brushes.Black, new RectangleF(margin.Left, y, margin.Width - 110, 20), PrintHelper.RTLFormatRight);
        y += 20;
        g.DrawString($"هاتف: {_storeInfo.Phone}", fonts.Body, Brushes.Black, new RectangleF(margin.Left, y, margin.Width - 110, 20), PrintHelper.RTLFormatRight);
        y += 25;

        if (y < margin.Top + 100) y = margin.Top + 110;

        PrintHelper.DrawLine(g, Pens.Black, margin.Left, y, margin.Right);
        y += 10;
    }

    private void DrawInvoiceHeader(Graphics g, RectangleF margin, ref float y, string title,
        (Font Title, Font Header, Font Body, Font Bold) fonts)
    {
        g.DrawString(title, fonts.Header, Brushes.Black, new RectangleF(margin.Left, y, margin.Width, 30), PrintHelper.RTLFormatCenter);
        y += 35;

        g.DrawString($"رقم الفاتورة: {_invoice!.InvoiceNumber}", fonts.Bold, Brushes.Black, new RectangleF(margin.Left, y, margin.Width, 20), PrintHelper.RTLFormatRight);
        y += 20;
        g.DrawString($"التاريخ: {_invoice.InvoiceDate:yyyy-MM-dd HH:mm}", fonts.Body, Brushes.Black, new RectangleF(margin.Left, y, margin.Width, 20), PrintHelper.RTLFormatRight);
        y += 20;
        g.DrawString($"العميل/المورد: {_invoice.CustomerOrSupplierName}", fonts.Body, Brushes.Black, new RectangleF(margin.Left, y, margin.Width, 20), PrintHelper.RTLFormatRight);
        y += 20;
        g.DrawString($"نوع الدفع: {_invoice.PaymentType.ToArabicString()}", fonts.Body, Brushes.Black, new RectangleF(margin.Left, y, margin.Width, 20), PrintHelper.RTLFormatRight);
        y += 30;
    }

    private static void DrawTotalRow(Graphics g, RectangleF margin, ref float y,
        string label, string value, Font labelFont, Font valueFont)
    {
        g.DrawString(label, labelFont, Brushes.Black, new RectangleF(margin.Left + 150, y, 150, 20), PrintHelper.RTLFormatRight);
        g.DrawString(value, valueFont, Brushes.Black, new RectangleF(margin.Left, y, 150, 20), PrintHelper.RTLFormatLeft);
        y += 20;
    }

    /// <summary>سطر التفقيط (المبلغ كتابةً) — يُرسم تحت كتلة الإجماليات ويُتجاهل تماماً عند غياب النص.</summary>
    private static void DrawTafqitLine(Graphics g, RectangleF margin, ref float y, Font font, InvoiceTotalsPrintDto totals)
    {
        if (totals == null || string.IsNullOrWhiteSpace(totals.Tafqit)) return;

        g.DrawString("التفقيط: " + totals.Tafqit, font, Brushes.Black,
            new RectangleF(margin.Left, y, margin.Width, 35), PrintHelper.RTLFormatRight);
        y += 35;
    }

    private static void DrawFooter(Graphics g, RectangleF margin, ref float y, Font footerFont)
    {
        g.DrawString("شكراً لتعاملكم معنا", footerFont, Brushes.Black, new RectangleF(margin.Left, y, margin.Width, 30), PrintHelper.RTLFormatCenter);
    }

    // ------------------------------------------------------------------
    // Normal A4 invoice
    // ------------------------------------------------------------------

    private void Pd_PrintPage(object sender, PrintPageEventArgs e)
    {
        if (_invoice == null || _items == null || _totals == null || _storeInfo == null) return;

        var g = e.Graphics!;
        var margin = e.MarginBounds;
        float y = margin.Top;

        var (titleFont, headerFont, bodyFont, boldFont) = CreateFonts();

        // 1. Header (Store Info)
        DrawStoreHeader(g, margin, ref y, (titleFont, headerFont, bodyFont, boldFont));

        // 2. Invoice Meta Info
        DrawInvoiceHeader(g, margin, ref y, _invoice.TypeName, (titleFont, headerFont, bodyFont, boldFont));

        // 3. Items Table Header
        PrintHelper.DrawLine(g, Pens.Black, margin.Left, y, margin.Right);
        y += 5;

        g.DrawString("المنتج", boldFont, Brushes.Black, new RectangleF(margin.Right - 300, y, 300, 20), PrintHelper.RTLFormatRight);
        g.DrawString("الكمية", boldFont, Brushes.Black, new RectangleF(margin.Right - 400, y, 100, 20), PrintHelper.RTLFormatRight);
        g.DrawString("السعر", boldFont, Brushes.Black, new RectangleF(margin.Right - 500, y, 100, 20), PrintHelper.RTLFormatRight);
        g.DrawString("الخصم", boldFont, Brushes.Black, new RectangleF(margin.Right - 600, y, 100, 20), PrintHelper.RTLFormatRight);
        g.DrawString("الإجمالي", boldFont, Brushes.Black, new RectangleF(margin.Left, y, 100, 20), PrintHelper.RTLFormatLeft);

        y += 25;
        PrintHelper.DrawLine(g, Pens.Gray, margin.Left, y, margin.Right);
        y += 5;

        // 4. Items List
        foreach (var item in _items)
        {
            g.DrawString(item.ProductName, bodyFont, Brushes.Black, new RectangleF(margin.Right - 300, y, 300, 20), PrintHelper.RTLFormatRight);
            g.DrawString(PrintHelper.FormatQuantity(item.Quantity), bodyFont, Brushes.Black, new RectangleF(margin.Right - 400, y, 100, 20), PrintHelper.RTLFormatRight);
            g.DrawString(PrintHelper.FormatCurrency(item.UnitPrice), bodyFont, Brushes.Black, new RectangleF(margin.Right - 500, y, 100, 20), PrintHelper.RTLFormatRight);
            g.DrawString(PrintHelper.FormatCurrency(item.Discount), bodyFont, Brushes.Black, new RectangleF(margin.Right - 600, y, 100, 20), PrintHelper.RTLFormatRight);
            g.DrawString(PrintHelper.FormatCurrency(item.LineTotal), bodyFont, Brushes.Black, new RectangleF(margin.Left, y, 100, 20), PrintHelper.RTLFormatLeft);

            y += 20;
            if (y > margin.Bottom - 150) break;
        }

        y += 10;
        PrintHelper.DrawLine(g, Pens.Black, margin.Left, y, margin.Right);
        y += 10;

        // 5. Totals
        DrawTotalRow(g, margin, ref y, "إجمالي فرعي:", PrintHelper.FormatCurrency(_totals.SubTotal), boldFont, bodyFont);
        DrawTotalRow(g, margin, ref y, "الخصم:", PrintHelper.FormatCurrency(_totals.Discount), boldFont, bodyFont);
        DrawTotalRow(g, margin, ref y, "الضريبة:", PrintHelper.FormatCurrency(_totals.TaxAmount), boldFont, bodyFont);
        y += 5;
        PrintHelper.DrawLine(g, Pens.Black, margin.Left, y, margin.Left + 300);
        y += 5;
        g.DrawString("الإجمالي النهائي:", headerFont, Brushes.Black, new RectangleF(margin.Left + 150, y, 150, 30), PrintHelper.RTLFormatRight);
        g.DrawString(PrintHelper.FormatCurrency(_totals.TotalAmount), boldFont, Brushes.Black, new RectangleF(margin.Left, y, 150, 30), PrintHelper.RTLFormatLeft);

        y += 30;
        DrawTafqitLine(g, margin, ref y, bodyFont, _totals);

        y += 20;
        DrawFooter(g, margin, ref y, headerFont);
    }

    // ------------------------------------------------------------------
    // "فاتورة إجمالية" — compact summary invoice (quantities only)
    // ------------------------------------------------------------------

    private void Pd_SummaryPrintPage(object sender, PrintPageEventArgs e)
    {
        if (_invoice == null || _items == null || _totals == null || _storeInfo == null) return;

        var g = e.Graphics!;
        var margin = e.MarginBounds;
        float y = margin.Top;

        var (titleFont, headerFont, bodyFont, boldFont) = CreateFonts();
        var fonts = (titleFont, headerFont, bodyFont, boldFont);
        var items = _items as IList<InvoiceItemPrintDto> ?? _items.ToList();

        // 1. Header (Store Info) — identical to the normal invoice
        DrawStoreHeader(g, margin, ref y, fonts);

        // 2. Sub-title + invoice header line (no, date, customer, payment type)
        DrawInvoiceHeader(g, margin, ref y, "فاتورة إجمالية", fonts);

        // 3. Items Table Header — # | الصنف | الوحدة | الكمية
        PrintHelper.DrawLine(g, Pens.Black, margin.Left, y, margin.Right);
        y += 5;

        g.DrawString("#", boldFont, Brushes.Black, new RectangleF(margin.Right - 60, y, 60, 20), PrintHelper.RTLFormatCenter);
        g.DrawString("الصنف", boldFont, Brushes.Black, new RectangleF(margin.Right - 460, y, 400, 20), PrintHelper.RTLFormatRight);
        g.DrawString("الوحدة", boldFont, Brushes.Black, new RectangleF(margin.Right - 600, y, 140, 20), PrintHelper.RTLFormatRight);
        g.DrawString("الكمية", boldFont, Brushes.Black, new RectangleF(margin.Right - 740, y, 140, 20), PrintHelper.RTLFormatRight);

        y += 25;
        PrintHelper.DrawLine(g, Pens.Gray, margin.Left, y, margin.Right);
        y += 5;

        // 4. Items List — quantities only (no price, no discount, no line total)
        int index = 1;
        foreach (var item in items)
        {
            g.DrawString(index.ToString(), bodyFont, Brushes.Black, new RectangleF(margin.Right - 60, y, 60, 20), PrintHelper.RTLFormatCenter);
            g.DrawString(item.ProductName, bodyFont, Brushes.Black, new RectangleF(margin.Right - 460, y, 400, 20), PrintHelper.RTLFormatRight);
            g.DrawString(string.IsNullOrWhiteSpace(item.UnitName) ? "—" : item.UnitName,
                bodyFont, Brushes.Black, new RectangleF(margin.Right - 600, y, 140, 20), PrintHelper.RTLFormatRight);
            g.DrawString(PrintHelper.FormatQuantity(item.Quantity), bodyFont, Brushes.Black, new RectangleF(margin.Right - 740, y, 140, 20), PrintHelper.RTLFormatRight);

            index++;
            y += 20;
            if (y > margin.Bottom - 200) break;
        }

        y += 10;
        PrintHelper.DrawLine(g, Pens.Black, margin.Left, y, margin.Right);
        y += 10;

        // 5. Compact totals block
        var itemCount = items.Count;
        var totalQuantity = items.Sum(i => i.Quantity);

        DrawTotalRow(g, margin, ref y, "عدد الأصناف:", itemCount.ToString("N0"), boldFont, bodyFont);
        DrawTotalRow(g, margin, ref y, "إجمالي الكمية:", PrintHelper.FormatQuantity(totalQuantity), boldFont, bodyFont);
        DrawTotalRow(g, margin, ref y, "الإجمالي:", PrintHelper.FormatCurrency(_totals.TotalAmount), boldFont, bodyFont);
        DrawTotalRow(g, margin, ref y, "المدفوع:", PrintHelper.FormatCurrency(_totals.PaidAmount), boldFont, bodyFont);
        DrawTotalRow(g, margin, ref y, "المتبقي:", PrintHelper.FormatCurrency(_totals.DueAmount), boldFont, bodyFont);

        // سطر التفقيط تحت كتلة الإجماليات (يُتجاهل عندما لا يتوفر النص).
        y += 5;
        DrawTafqitLine(g, margin, ref y, bodyFont, _totals);

        y += 20;
        DrawFooter(g, margin, ref y, headerFont);
    }
}
