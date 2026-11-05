namespace SalesSystem.DTO;

public record SupplierDto(int Id, string Name, string? Phone, string? Email, string? Address,
    string? TaxNumber, string? Notes, decimal CreditLimit, bool IsActive,
    int AccountId, string? AccountName = null, int? CategoryId = null, short BranchId = 0)
{
    public bool HasCreditLimit => CreditLimit > 0;

    // Backward-compatible properties
    public string Code => Id.ToString();
    public decimal CurrentBalance => 0m;
}
