namespace SalesSystem.DTO.Requests;

public record CreateInventoryCountRequest(
    short WarehouseId,
    string? Notes = null,
    short? BranchId = null
);

public record UpdateInventoryCountRequest(
    string? Notes = null
);

public record AddInventoryCountLineRequest(
    int InventoryCountId,
    int ProductUnitId,
    decimal ExpectedQuantity,
    decimal ActualQuantity,
    string? Notes = null
);
