namespace SalesSystem.DTO;

public record PurchaseInvoiceDto(
    int Id,
    int InvoiceNo,
    int SupplierId,
    string SupplierName,
    int WarehouseId,
    string WarehouseName,
    DateOnly InvoiceDate,
    byte PaymentType,
    decimal SubTotal,
    decimal DiscountAmount,
    byte DiscountType,
    decimal? DiscountRate,
    decimal? CostInBaseCurrency,
    decimal TaxAmount,
    decimal OtherCharges,
    decimal NetTotal,
    decimal PaidAmount,
    decimal RemainingAmount,
    string? Notes,
    string? SupplierInvoiceNo,
    string? AttachmentPath,
    byte Status,
    int? TaxId,
    string? TaxName,
    decimal? TaxRate,
    int? CashBoxId,
    IReadOnlyList<PurchaseInvoiceLineDto> Items,
    short BranchId = 0)
{
    public string PaymentTypeDisplay => PaymentType switch
    {
        1 => "نقدي",
        2 => "آجل",
        3 => "مختلط",
        _ => "غير معروف"
    };

    public string StatusDisplay => Status switch
    {
        1 => "مسودة",
        2 => "تم الترحيل",
        3 => "ملغي",
        _ => "غير معروف"
    };

    public decimal TotalAmount => NetTotal;
    public decimal DueAmount => RemainingAmount;
}

public record PurchaseInvoiceLineDto(int Id, int ProductId, string ProductName,
    int ProductUnitId,
    string? ProductUnitName,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineTotal,
    decimal LandedUnitCost,
    byte DiscountType,
    decimal? DiscountRate,
    decimal DiscountAmount,
    decimal? CostInBaseCurrency,
    decimal AdditionalFeesAmount)
{
    // Backward-compatible properties
    public string? ProductCode => null;
    public decimal UnitCost => LandedUnitCost;
}
