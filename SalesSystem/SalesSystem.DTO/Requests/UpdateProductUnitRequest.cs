namespace SalesSystem.DTO.Requests;

public record UpdateProductUnitRequest(
    short UnitId,
    string? Barcode = null,
    decimal SalePrice = 0,
    decimal? WholesalePrice = null,
    decimal CostPrice = 0,
    decimal? MaxDiscountAmount = null
);
