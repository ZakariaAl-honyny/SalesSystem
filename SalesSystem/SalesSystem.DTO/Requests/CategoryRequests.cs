namespace SalesSystem.DTO.Requests;

public record CreateCategoryRequest(string Name, string? Description, short? BranchId = null);
public record UpdateCategoryRequest(string Name, string? Description, bool IsActive);
