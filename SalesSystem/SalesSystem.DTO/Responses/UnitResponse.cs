namespace SalesSystem.DTO.Responses;

public record UnitResponse(int Id, string Name, string Symbol, string? Description, bool IsActive);
