namespace SalesSystem.Desktop.Printing.Models;

public class InvoiceTotalsPrintDto
{
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }

    /// <summary>التفقيط (المبلغ كتابةً) — اختياري؛ null يعني عدم عرض سطر التفقيط.</summary>
    public string? Tafqit { get; set; }
}
