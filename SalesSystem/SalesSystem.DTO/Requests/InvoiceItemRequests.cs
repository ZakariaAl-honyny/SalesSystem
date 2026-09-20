namespace SalesSystem.DTO.Requests;

public record CreateSalesInvoiceItemRequest(
    int ProductId,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    string? Notes);

public record CreatePurchaseInvoiceItemRequest(
    int ProductId,
    decimal Quantity,
    decimal UnitCost,
    decimal DiscountAmount,
    string? Notes);
