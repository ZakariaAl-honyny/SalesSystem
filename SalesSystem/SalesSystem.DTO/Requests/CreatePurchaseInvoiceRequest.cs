using SalesSystem.DTO.Enums;

namespace SalesSystem.DTO.Requests;

/// <summary>
/// طلب إنشاء فاتورة شراء جديدة — مع دعم نوع الخصم.
/// </summary>
public record CreatePurchaseInvoiceRequest(
    int WarehouseId,
    int SupplierId,
    int? InvoiceNo,
    DateTime? InvoiceDate,
    PaymentType PaymentType,
    decimal DiscountAmount,
    DiscountType? DiscountType,
    decimal? DiscountRate,
    decimal OtherCharges,
    decimal PaidAmount,
    string? Notes,
    string? AttachmentPath,
    List<CreatePurchaseInvoiceLineRequest> Items);

/// <summary>
/// طلب إنشاء بند في فاتورة الشراء — مع دعم الخصم.
/// </summary>
public record CreatePurchaseInvoiceLineRequest(
    int ProductId,
    int ProductUnitId,
    decimal Quantity,
    decimal UnitPrice,
    DiscountType? DiscountType = null,
    decimal? DiscountRate = null);
