using SalesSystem.DTO.Enums;

namespace SalesSystem.DTO.Requests;

public record CreateCustomerPaymentRequest(
    int CustomerId,
    decimal Amount,
    PaymentType PaymentType,
    DateTime PaymentDate,
    int? SalesInvoiceId,
    string? Notes);
