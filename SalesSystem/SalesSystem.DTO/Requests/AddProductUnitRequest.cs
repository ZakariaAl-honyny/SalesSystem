namespace SalesSystem.DTO.Requests;

public record AddProductUnitRequest(
    short UnitId,
    decimal Factor,
    bool IsBaseUnit
);
