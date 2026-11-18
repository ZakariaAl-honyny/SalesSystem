namespace SalesSystem.DTO;

public record CustomerDto(int Id, string Name, string? Phone, string? Email, string? Address,
    decimal CreditLimit, bool IsActive,
    int? CategoryId = null, string? Notes = null,
    decimal CurrentBalance = 0)
{
    public bool HasCreditLimit => CreditLimit > 0;

    // Backward-compatible properties
    public string Code => Id.ToString();
}
