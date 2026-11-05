namespace SalesSystem.DTO;

public record PurchaseReturnDto(
    int Id,
    int ReturnNo,
    int WarehouseId,
    string WarehouseName,
    int SupplierId,
    string SupplierName,
    int? PurchaseInvoiceId,
    bool LinkToInvoice,
    DateOnly ReturnDate,
    decimal SubTotal,
    decimal TotalAmount,
    decimal ReturnedDiscountAmount,
    decimal ReturnedTaxAmount,
    decimal ReturnedChargeAmount,
    short? TaxId,
    byte DiscountType,
    decimal? DiscountRate,
    string? Notes,
    byte Status, IReadOnlyList<PurchaseReturnItemDto> Items,
    short BranchId = 0)
{
    public string StatusDisplay => Status switch
    {
        1 => "مسودة",
        2 => "تم الترحيل",
        3 => "ملغي",
        _ => "غير معروف"
    };
}

public record PurchaseReturnItemDto(int Id, int ProductId, string ProductName,
    int ProductUnitId,
    string? ProductUnitName,
    decimal Quantity,
    decimal UnitCost,
    decimal LineTotal,
    int? PurchaseInvoiceLineId,
    decimal? CostInBaseCurrency);
