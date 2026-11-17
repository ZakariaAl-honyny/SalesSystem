namespace SalesSystem.DTO.Requests;

/// <summary>
/// Request to create a new warehouse — a name plus optional phone, address and notes.
/// </summary>
public record CreateWarehouseRequest(
    string Name,
    string? Phone = null,
    string? Address = null,
    string? Notes = null);

/// <summary>
/// Request to update an existing warehouse.
/// </summary>
public record UpdateWarehouseRequest(
    string Name,
    string? Phone = null,
    string? Address = null,
    string? Notes = null,
    bool IsActive = true);

/// <summary>
/// Request to create a new inventory transaction.
/// </summary>
public record CreateInventoryTransactionRequest(
    string? TransactionNo,
    byte MovementType,
    short WarehouseId,
    byte? ReferenceType = null,
    int? ReferenceId = null,
    string? Notes = null,
    List<CreateInventoryTransactionLineRequest>? Lines = null);

/// <summary>
/// A single line in an inventory transaction.
/// </summary>
public record CreateInventoryTransactionLineRequest(
    int ProductUnitId,
    decimal Quantity,
    decimal UnitCost,
    string? BatchNo = null,
    DateOnly? ExpiryDate = null,
    short? WarehouseId = null);
