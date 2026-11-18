namespace SalesSystem.DTO;

public record SupplierDto(int Id, string Name, string? Phone, string? Email, string? Address,
    string? Notes, decimal CreditLimit, bool IsActive,
    int? CategoryId = null)
{
    public bool HasCreditLimit => CreditLimit > 0;

    // Backward-compatible properties
    public string Code => Id.ToString();
    public decimal CurrentBalance => 0m;
}
