namespace SalesSystem.DTO.Requests;

public record CreateInventoryAdjustmentRequest(
    short WarehouseId,
    byte AdjustmentType,
    string? Reason = null,
    short? BranchId = null
);

public record AddInventoryAdjustmentLineRequest(
    int InventoryAdjustmentId,
    int ProductUnitId,
    decimal ExpectedQuantity,
    decimal ActualQuantity,
    decimal UnitCost
);
