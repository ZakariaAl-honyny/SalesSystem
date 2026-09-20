namespace SalesSystem.DTO;

public record SalesInvoiceItemDto(
    int ProductId,
    string? ProductCode,
    string ProductName,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    decimal LineTotal);

public record PurchaseInvoiceItemDto(
    int ProductId,
    string? ProductCode,
    string ProductName,
    decimal Quantity,
    decimal UnitCost,
    decimal DiscountAmount,
    decimal LineTotal);
