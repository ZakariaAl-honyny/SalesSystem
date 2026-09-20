namespace SalesSystem.DTO.Responses;

public record ProductUnitDto(
    int Id,
    int ProductId,
    int UnitId,
    string? UnitName,
    decimal ConversionFactor,
    bool IsBaseUnit
);
