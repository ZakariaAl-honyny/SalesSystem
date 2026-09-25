namespace SalesSystem.Desktop.Models;

/// <summary>
/// بسيط لنقل أسطر الفاتورة من نماذج الإدخال السريع إلى محرر الفاتورة الرئيسي.
/// Simple DTO to transfer invoice lines from strategy input forms to the main invoice editor.
/// </summary>
public record SalesLineDto(
    int ProductId,
    string ProductName,
    decimal Quantity,
    string? UnitName,
    decimal UnitPrice,
    decimal Discount = 0m,
    string? ProductCode = null,
    string? Barcode = null
);
