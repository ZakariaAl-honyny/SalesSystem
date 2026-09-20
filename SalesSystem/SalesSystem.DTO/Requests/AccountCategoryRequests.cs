namespace SalesSystem.DTO.Requests;

public record CreateAccountCategoryRequest(string Name, string? Description = null);
public record UpdateAccountCategoryRequest(string Name, string? Description = null);
