namespace SalesSystem.DTO;

public record WarehouseDto(
    short Id,
    string Name,
    string? Phone,
    string? Address,
    string? Notes,
    bool IsActive,
    short? BranchId = null)
{
    // Backward-compatible properties
    public bool IsDefault => false;
    public string? Location => Address;
    public string Code => Id.ToString();
}

public record WarehouseStockDto(
    int WarehouseId,
    string? WarehouseName,
    int ProductId,
    string ProductName,
    string? UnitName,
    decimal Quantity,
    decimal AvgCost);
