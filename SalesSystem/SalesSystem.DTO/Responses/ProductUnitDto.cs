namespace SalesSystem.DTO.Responses;

public record ProductUnitDto(
    int Id,
    int ProductId,
    int UnitId,
    string? UnitName,
    decimal ConversionFactor,
    bool IsBaseUnit,
    string? Barcode,
    decimal SalePrice,
    decimal? WholesalePrice,
    decimal CostPrice,
    decimal? MaxDiscountAmount
);
