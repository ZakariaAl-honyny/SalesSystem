namespace SalesSystem.DTO.Requests;

public record CreateCustomerRequest(
    string Name,
    string? Phone,
    string? Email,
    string? Address,
    string? TaxNumber,
    string? Notes = null,
    decimal CreditLimit = 0,
    short? BranchId = null
);

public record UpdateCustomerRequest(
    string Name,
    string? Phone,
    string? Email,
    string? Address,
    string? TaxNumber,
    string? Notes = null,
    decimal CreditLimit = 0,
    bool IsActive = true
);
