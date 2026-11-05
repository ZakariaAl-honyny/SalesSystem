namespace SalesSystem.DTO;

public record SupplierPaymentDto(
    int Id,
    string PaymentNo,
    int SupplierId,
    string SupplierName,
    decimal Amount,
    byte PaymentMethod,
    DateOnly PaymentDate,
    int? PurchaseInvoiceId,
    string? Notes,
    short BranchId = 0)
{
    public string PaymentTypeDisplay => PaymentMethod switch
    {
        1 => "نقدي",
        2 => "آجل",
        3 => "مختلط",
        _ => "غير معروف"
    };
}
