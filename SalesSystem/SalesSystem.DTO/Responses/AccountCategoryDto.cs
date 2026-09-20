namespace SalesSystem.DTO.Responses;

public record AccountCategoryDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive
);
