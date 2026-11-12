namespace SalesSystem.DTO;

public record BranchDto(
    short Id,
    string Name,
    string? Address = null,
    string? Phone = null,
    bool IsActive = true);
