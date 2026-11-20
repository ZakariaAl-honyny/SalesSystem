namespace SalesSystem.DTO.Requests;

public record CreateProductCategoryRequest(string Name, string? Description);
public record UpdateProductCategoryRequest(string Name, string? Description);
