namespace SalesSystem.DTO;

public record SalesReturnDto(
    int Id,
    int ReturnNo,
    int WarehouseId,
    string WarehouseName,
    int CustomerId,
    string CustomerName,
    int? SalesInvoiceId,
    DateTime ReturnDate,
    decimal SubTotal,
    decimal DiscountAmount,
    decimal TotalAmount,
    decimal ReturnedDiscountAmount,
    decimal ReturnedChargeAmount,
    string? Notes,
    string? ReturnReason,
    byte Status,
    int? CashBoxId,
    string? CashBoxName,
    decimal RefundAmount,
    IReadOnlyList<SalesReturnItemDto> Items)
{
    public string StatusDisplay => Status switch
    {
        1 => "مسودة",
        2 => "تم الترحيل",
        3 => "ملغي",
        _ => "غير معروف"
    };
}

public record SalesReturnItemDto(int Id, int ProductId, int ProductUnitId, string ProductName,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    decimal LineTotal,
    byte Mode);
