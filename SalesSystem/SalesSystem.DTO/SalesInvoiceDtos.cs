namespace SalesSystem.DTO;

public record SalesInvoiceDto(
    int Id,
    int InvoiceNo,
    int CustomerId,
    string? CustomerName,
    int WarehouseId,
    string WarehouseName,
    DateTime InvoiceDate,
    byte PaymentType,
    decimal SubTotal,
    decimal DiscountAmount,
    byte DiscountType,
    decimal? DiscountRate,
    decimal TaxAmount,
    decimal OtherCharges,
    decimal NetTotal,
    decimal PaidAmount,
    decimal RemainingAmount,
    decimal? CostInBaseCurrency,
    string? Notes,
    byte Status,
    int? TaxId,
    string? TaxName,
    decimal? TaxRate,
    int? CashBoxId,
    string? CashBoxName,
    IReadOnlyList<SalesInvoiceLineDto> Items,
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

public record SalesInvoiceLineDto(int Id, int ProductId, string ProductName,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineTotal,
    int ProductUnitId,
    byte DiscountType = 0,
    decimal? DiscountRate = null,
    decimal DiscountAmount = 0,
    decimal? CostInBaseCurrency = null,
    decimal UnitCost = 0,
    decimal ProfitAmount = 0)
{
    // Backward-compatible properties
    public string? ProductCode => null;
}
