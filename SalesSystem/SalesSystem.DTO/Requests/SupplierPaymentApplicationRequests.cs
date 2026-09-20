namespace SalesSystem.DTO.Requests;

public record CreateSupplierPaymentApplicationRequest(
    int SupplierPaymentId,
    int PurchaseInvoiceId,
    decimal AppliedAmount);
