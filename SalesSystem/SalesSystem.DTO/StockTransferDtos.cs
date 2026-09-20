namespace SalesSystem.DTO;

public record StockTransferDto(
    int Id,
    string TransferNo,
    DateTime TransferDate,
    int FromWarehouseId,
    int ToWarehouseId,
    string? Notes,
    byte Status,
    IReadOnlyList<StockTransferItemDto> Items)
{
    // Backward-compatible properties
    public string? SourceWarehouseName => null;
    public string? DestinationWarehouseName => null;
    public DateTime CreatedAt => TransferDate;
    public string StatusDisplay => Status switch
    {
        1 => "مسودة",
        2 => "تم الترحيل",
        3 => "ملغي",
        _ => "غير معروف"
    };
}

public record StockTransferItemDto(
    int ProductId,
    string? ProductCode,
    string ProductName,
    decimal Quantity);
