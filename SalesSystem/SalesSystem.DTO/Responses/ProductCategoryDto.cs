namespace SalesSystem.DTO.Responses;

public record ProductCategoryDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive
);
