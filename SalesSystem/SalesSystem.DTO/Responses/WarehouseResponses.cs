namespace SalesSystem.DTO.Responses;

/// <summary>
/// Response for Warehouse entity — smallint PK, no AccountId/Type/ManagerName per schema.
/// </summary>
public record WarehouseResponse(
    short Id,
    string Name,
    string? Phone,
    string? Address,
    string? Notes,
    bool IsActive);

/// <summary>
/// Simplified stock summary response.
/// </summary>
public record WarehouseStockSummaryResponse(
    int ProductId,
    string ProductName,
    decimal Quantity,
    decimal AvgCost);

/// <summary>
/// Response for InventoryTransaction entity.
/// </summary>
public record InventoryTransactionResponse(
    int Id,
    int TransactionNo,
    DateTime TransactionDate,
    byte TransactionType,
    short WarehouseId,
    string? WarehouseName,
    int? ReferenceId,
    byte? ReferenceType,
    string? Notes,
    byte Status,
    List<InventoryTransactionLineResponse>? Lines);

/// <summary>
/// A single line within an InventoryTransaction.
/// </summary>
public record InventoryTransactionLineResponse(
    int Id,
    int ProductId,
    string? ProductName,
    int ProductUnitId,
    string? ProductUnitName,
    decimal Quantity,
    decimal UnitCost,
    decimal TotalCost,
    int? BatchId);
