namespace SalesSystem.Desktop.Printing.Models;

public class InvoiceItemPrintDto
{
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; } // decimal(18,3)
    public decimal UnitPrice { get; set; } // decimal(18,2)
    public decimal Discount { get; set; } // decimal(18,2)
    public decimal LineTotal { get; set; } // decimal(18,2)

    /// <summary>اسم وحدة القياس (اختياري) — تُترك null عندما لا يتوفر اسم الوحدة في مصدر البيانات.</summary>
    public string? UnitName { get; set; }
}
