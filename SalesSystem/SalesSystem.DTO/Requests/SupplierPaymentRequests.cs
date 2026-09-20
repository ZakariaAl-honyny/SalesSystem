using SalesSystem.DTO.Enums;

namespace SalesSystem.DTO.Requests;

public record CreateSupplierPaymentRequest(
    int SupplierId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    DateTime? PaymentDate,
    int? PurchaseInvoiceId,
    int? CashBoxId,
    string? Notes = null,
    short? BranchId = null
);
