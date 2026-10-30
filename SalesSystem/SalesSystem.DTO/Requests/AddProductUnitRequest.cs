namespace SalesSystem.DTO.Requests;

public record AddProductUnitRequest(
    short UnitId,
    decimal Factor,
    bool IsBaseUnit,
    string? Barcode = null,
    decimal SalePrice = 0,
    decimal? WholesalePrice = null,
    decimal CostPrice = 0,
    decimal? MaxDiscountAmount = null
);
