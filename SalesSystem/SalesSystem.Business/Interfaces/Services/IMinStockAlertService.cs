using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Service for monitoring inventory stock levels and batch expiry alerts.
/// Provides low-stock warnings and expiry notifications for the dashboard and notifications module.
/// All methods return Result{T} per RULE-006.
/// </summary>
public interface IMinStockAlertService
{
    /// <summary>
    /// Gets all products that are below their reorder level (min stock threshold)
    /// across all warehouses, or filtered by a specific warehouse.
    /// </summary>
    Task<Result<List<LowStockProductDto>>> GetLowStockProductsAsync(short? warehouseId, CancellationToken ct);

    /// <summary>
    /// Gets products whose inventory batches are expiring within the specified number of days.
    /// Optionally filtered by warehouse.
    /// </summary>
    Task<Result<List<ExpiringBatchDto>>> GetExpiringBatchesAsync(int thresholdDays, short? warehouseId, CancellationToken ct);

    /// <summary>
    /// Gets the count of products that are below their reorder level (for dashboard badges and notifications).
    /// </summary>
    Task<Result<int>> GetLowStockCountAsync(CancellationToken ct);
}

// ─── DTOs ────────────────────────────────────────────────────────────────

/// <summary>
/// A product whose current stock is below its configured reorder level.
/// </summary>
public record LowStockProductDto(
    int ProductId,
    string? ProductName,
    string? Barcode,
    decimal CurrentStock,
    decimal ReorderLevel,
    short WarehouseId,
    string? WarehouseName,
    string? CategoryName
);

/// <summary>
/// An inventory batch that is expiring within the configured threshold.
/// </summary>
public record ExpiringBatchDto(
    int BatchId,
    string? BatchNo,
    int ProductId,
    string? ProductName,
    decimal QuantityRemaining,
    DateOnly ExpiryDate,
    short WarehouseId,
    string? WarehouseName,
    int DaysUntilExpiry
);
