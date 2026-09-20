using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.Models;

namespace SalesSystem.Business.Services;

/// <summary>
/// Monitors inventory stock levels and batch expiry alerts.
/// Provides low-stock warnings and expiry notifications for dashboard display.
///
/// Read-only service — does NOT modify any entities.
/// All methods return Result{T} per RULE-006.
/// </summary>
public class MinStockAlertService : IMinStockAlertService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<MinStockAlertService> _logger;

    public MinStockAlertService(IUnitOfWork uow, ILogger<MinStockAlertService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    // ─── Get Low Stock Products ──────────────────────────────────────

    public async Task<Result<List<LowStockProductDto>>> GetLowStockProductsAsync(
        short? warehouseId, CancellationToken ct)
    {
        try
        {
            // Build predicate: stock quantity is below product reorder level
            Expression<Func<WarehouseStock, bool>> predicate;
            if (warehouseId.HasValue)
            {
                var whId = warehouseId.Value;
                predicate = ws => ws.Quantity < ws.Product!.ReorderLevel
                                  && ws.Product.IsActive
                                  && ws.WarehouseId == whId;
            }
            else
            {
                predicate = ws => ws.Quantity < ws.Product!.ReorderLevel
                                  && ws.Product.IsActive;
            }

            var stocks = await _uow.WarehouseStocks.ToListAsync(
                predicate,
                q => q.OrderBy(ws => ws.Product != null ? ws.Product.Name : ""),
                ct: ct,
                includePaths: "Product,Warehouse,Product.ProductCategory");

            var dtos = stocks.Select(ws => new LowStockProductDto(
                ProductId: ws.ProductId,
                ProductName: ws.Product?.Name,
                Barcode: ws.Product?.Barcode,
                CurrentStock: ws.Quantity,
                ReorderLevel: ws.Product?.ReorderLevel ?? 0,
                WarehouseId: ws.WarehouseId,
                WarehouseName: ws.Warehouse?.Name,
                CategoryName: ws.Product?.ProductCategory?.Name
            )).ToList();

            _logger.LogInformation(
                "Low stock check completed: {Count} products below reorder level{WhSuffix}",
                dtos.Count,
                warehouseId.HasValue ? $" (Warehouse {warehouseId.Value})" : "");

            return Result<List<LowStockProductDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving low stock products");
            return Result<List<LowStockProductDto>>.Failure("حدث خطأ أثناء استرجاع المنتجات منخفضة المخزون");
        }
    }

    // ─── Get Expiring Batches ────────────────────────────────────────

    public async Task<Result<List<ExpiringBatchDto>>> GetExpiringBatchesAsync(
        int thresholdDays, short? warehouseId, CancellationToken ct)
    {
        try
        {
            if (thresholdDays <= 0)
                return Result<List<ExpiringBatchDto>>.Failure(
                    "عدد الأيام يجب أن يكون أكبر من الصفر");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var thresholdDate = today.AddDays(thresholdDays);

            // Build predicate: batch expiring within threshold days
            Expression<Func<InventoryBatch, bool>> predicate;
            if (warehouseId.HasValue)
            {
                var whId = warehouseId.Value;
                predicate = b => b.ExpiryDate.HasValue
                                 && b.ExpiryDate.Value >= today
                                 && b.ExpiryDate.Value <= thresholdDate
                                 && b.QuantityRemaining > 0
                                 && b.WarehouseId == whId;
            }
            else
            {
                predicate = b => b.ExpiryDate.HasValue
                                 && b.ExpiryDate.Value >= today
                                 && b.ExpiryDate.Value <= thresholdDate
                                 && b.QuantityRemaining > 0;
            }

            var batches = await _uow.InventoryBatches.ToListAsync(
                predicate,
                q => q.OrderBy(b => b.ExpiryDate.HasValue ? b.ExpiryDate.Value : DateOnly.MaxValue),
                ct: ct,
                includePaths: "Product,Warehouse");

            var dtos = batches.Select(b => new ExpiringBatchDto(
                BatchId: b.Id,
                BatchNo: b.BatchNo,
                ProductId: b.ProductId,
                ProductName: b.Product?.Name,
                QuantityRemaining: b.QuantityRemaining,
                ExpiryDate: b.ExpiryDate!.Value,
                WarehouseId: b.WarehouseId,
                WarehouseName: b.Warehouse?.Name,
                DaysUntilExpiry: b.ExpiryDate!.Value.DayNumber - today.DayNumber
            )).ToList();

            _logger.LogInformation(
                "Expiry check completed: {Count} batches expiring within {ThresholdDays} days{WhSuffix}",
                dtos.Count,
                thresholdDays,
                warehouseId.HasValue ? $" (Warehouse {warehouseId.Value})" : "");

            return Result<List<ExpiringBatchDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving expiring batches (threshold: {ThresholdDays} days)", thresholdDays);
            return Result<List<ExpiringBatchDto>>.Failure("حدث خطأ أثناء استرجاع الدفعات المنتهية");
        }
    }

    // ─── Get Low Stock Count ─────────────────────────────────────────

    public async Task<Result<int>> GetLowStockCountAsync(CancellationToken ct)
    {
        try
        {
            // Count distinct products that have at least one warehouse stock below reorder level
            var count = await _uow.WarehouseStocks.CountAsync(
                ws => ws.Quantity < ws.Product!.ReorderLevel
                      && ws.Product.IsActive,
                ct);

            _logger.LogInformation("Low stock count: {Count} products", count);

            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error counting low stock products");
            return Result<int>.Failure("حدث خطأ أثناء حساب عدد المنتجات منخفضة المخزون");
        }
    }
}
