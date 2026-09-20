namespace SalesSystem.DTO;

public record CustomerPaymentDto(
    int Id,
    int ReceiptNo,
    string? CustomerName,
    decimal Amount,
    DateTime ReceiptDate,
    string? Notes)
{
    // Backward-compatible properties
    public DateTime PaymentDate => ReceiptDate;
}
