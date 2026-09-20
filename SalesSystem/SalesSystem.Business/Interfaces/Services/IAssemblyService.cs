using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Manages Bill of Materials (BOM) / product assemblies — component definitions,
/// cost roll-up, and circular reference validation.
/// All methods return Result{T} per RULE-006.
/// </summary>
public interface IAssemblyService
{
    /// <summary>
    /// Gets all BOM entries where the specified product is the finished product.
    /// Returns active components sorted by component product name.
    /// </summary>
    Task<Result<List<BillOfMaterialsDto>>> GetComponentsAsync(int finishedProductId, CancellationToken ct);

    /// <summary>
    /// Gets all BOM entries where the specified product is used as a component.
    /// Shows which assemblies consume this product.
    /// </summary>
    Task<Result<List<BillOfMaterialsDto>>> GetParentAssembliesAsync(int componentProductId, CancellationToken ct);

    /// <summary>
    /// Adds a component to a finished product's BOM.
    /// Validates both products exist, quantity > 0, and no circular reference.
    /// </summary>
    Task<Result<BillOfMaterialsDto>> AddComponentAsync(CreateBillOfMaterialsRequest request, CancellationToken ct);

    /// <summary>
    /// Updates the quantity required for an existing BOM entry.
    /// </summary>
    Task<Result<BillOfMaterialsDto>> UpdateComponentQuantityAsync(int id, decimal newQuantity, CancellationToken ct);

    /// <summary>
    /// Soft-deletes (deactivates) a BOM entry.
    /// </summary>
    Task<Result> RemoveComponentAsync(int id, CancellationToken ct);

    /// <summary>
    /// Calculates the total component cost to produce one unit of the finished product.
    /// Uses weighted average cost from InventoryBatches for each component × quantity required.
    /// Returns 0 if no stock exists for any component.
    /// </summary>
    Task<Result<decimal>> CalculateTotalCostAsync(int finishedProductId, short warehouseId, CancellationToken ct);

    /// <summary>
    /// Validates that adding this component won't create a direct circular reference (A→B→A).
    /// V1 — checks only direct circular references, not deep recursion.
    /// </summary>
    Task<Result<bool>> ValidateNoCircularReferenceAsync(int finishedProductId, int componentProductId, CancellationToken ct);
}

// ─── DTOs ────────────────────────────────────────────────────────────────

/// <summary>
/// Represents a single Bill of Materials entry — links a component to a finished product.
/// </summary>
public record BillOfMaterialsDto(
    int Id,
    int FinishedProductId,
    string? FinishedProductName,
    int ComponentProductId,
    string? ComponentProductName,
    decimal QuantityRequired,
    short? UnitId,
    string? UnitName,
    string? Notes,
    bool IsActive
);

/// <summary>
/// Request to add a new component to a finished product's BOM.
/// </summary>
public record CreateBillOfMaterialsRequest(
    int FinishedProductId,
    int ComponentProductId,
    decimal QuantityRequired,
    short? UnitId = null,
    string? Notes = null
);
