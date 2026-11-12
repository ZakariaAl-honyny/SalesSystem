using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المخزون — تعيد أرصدة من MockData وترفض العمليات غير المدعومة في وضع التجربة.
/// </summary>
public class MockInventoryDesktopService : IInventoryDesktopService
{
    private const string UnsupportedMessage = "هذه العملية غير مدعومة في وضع التجربة.";

    // ═══════════════════════════════════════════════
    // الأرصدة — Stock
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByWarehouseAsync(int warehouseId, CancellationToken ct = default)
    {
        var warehouseName = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)warehouseId)?.Name;

        var stocks = MockData.Products
            .Where(p => p.IsActive)
            .Select(p => new WarehouseStockDto(
                WarehouseId: warehouseId,
                WarehouseName: warehouseName,
                ProductId: p.Id,
                ProductName: p.Name,
                UnitName: MockData.ProductUnits.FirstOrDefault(u => u.ProductId == p.Id && u.IsBaseUnit)?.UnitName,
                Quantity: p.CurrentStock,
                AvgCost: 0m))
            .ToList()
            .AsReadOnly();

        return Task.FromResult(Result<IReadOnlyList<WarehouseStockDto>>.Success(stocks));
    }

    public Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByProductAsync(int productId, CancellationToken ct = default)
    {
        var product = MockData.Products.FirstOrDefault(p => p.Id == productId);
        if (product is null)
            return Task.FromResult(Result<IReadOnlyList<WarehouseStockDto>>.Failure("المنتج غير موجود"));

        var unitName = MockData.ProductUnits
            .FirstOrDefault(u => u.ProductId == productId && u.IsBaseUnit)?.UnitName;

        var stocks = MockData.Warehouses
            .Select(w => new WarehouseStockDto(
                WarehouseId: w.Id,
                WarehouseName: w.Name,
                ProductId: product.Id,
                ProductName: product.Name,
                UnitName: unitName,
                Quantity: product.CurrentStock,
                AvgCost: 0m))
            .ToList()
            .AsReadOnly();

        return Task.FromResult(Result<IReadOnlyList<WarehouseStockDto>>.Success(stocks));
    }

    // ═══════════════════════════════════════════════
    // حركات المخزون — Transactions
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<InventoryTransactionDto>>> GetMovementsAsync(
        int? productId = null, int? warehouseId = null, int? movementType = null,
        int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        // لا توجد حركات مخزون مسجّلة في بيانات التجربة — نعيد قائمة فارغة.
        IReadOnlyList<InventoryTransactionDto> empty = new List<InventoryTransactionDto>().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<InventoryTransactionDto>>.Success(empty));
    }

    public Task<Result<InventoryTransactionDto>> CreateInventoryTransactionAsync(
        CreateInventoryTransactionRequest request, CancellationToken ct = default)
        => Task.FromResult(Result<InventoryTransactionDto>.Failure(UnsupportedMessage, ErrorCodes.InvalidOperation));

    public Task<Result<InventoryTransactionDto>> PostInventoryTransactionAsync(int id, CancellationToken ct = default)
        => Task.FromResult(Result<InventoryTransactionDto>.Failure(UnsupportedMessage, ErrorCodes.InvalidOperation));

    public Task<Result<InventoryTransactionDto>> CancelInventoryTransactionAsync(int id, CancellationToken ct = default)
        => Task.FromResult(Result<InventoryTransactionDto>.Failure(UnsupportedMessage, ErrorCodes.InvalidOperation));

    public Task<Result<InventoryTransactionDto>> GetInventoryTransactionByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(Result<InventoryTransactionDto>.Failure(UnsupportedMessage, ErrorCodes.InvalidOperation));
}
