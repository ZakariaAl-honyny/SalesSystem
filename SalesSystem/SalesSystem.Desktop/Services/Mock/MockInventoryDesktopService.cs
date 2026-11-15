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

    /// <summary>
    /// حركات المخزون من بيت البيانات الواحد <see cref="MockInventoryData.Movements"/> (RULE-N169) —
    /// الخدمة <b>قارئة فقط</b>: تصفية بالصنف/المخزن/نوع الحركة ثم ترقيم الصفحات، الأحدث أولاً.
    /// </summary>
    /// <remarks>
    /// التصفية الزمنية تُطبَّق في الشاشة على <c>DataView</c> (لا معامل تاريخ في العقد)،
    /// والفلترة بالصنف تُحلّ من وحدة الصنف في البنود لأن الحركة تحمل وحدات لا أصنافاً.
    /// </remarks>
    public Task<Result<IReadOnlyList<InventoryTransactionDto>>> GetMovementsAsync(
        int? productId = null, int? warehouseId = null, int? movementType = null,
        int page = 1, int pageSize = 50, CancellationToken ct = default)
    {
        try
        {
            IEnumerable<InventoryTransactionDto> query = MockInventoryData.Movements;

            if (productId.HasValue)
            {
                int wantedProductId = productId.Value;
                query = query.Where(t => t.Lines is { Count: > 0 } && t.Lines.Any(line =>
                    MockData.ProductUnits.FirstOrDefault(u => u.Id == line.ProductUnitId)?.ProductId == wantedProductId));
            }

            if (warehouseId.HasValue)
                query = query.Where(t => t.WarehouseId == warehouseId.Value);

            if (movementType.HasValue)
                query = query.Where(t => t.MovementType == movementType.Value);

            int size = pageSize > 0 ? pageSize : 50;
            int pageNumber = page > 0 ? page : 1;

            var rows = query
                .OrderByDescending(t => t.CreatedAt)
                .ThenByDescending(t => t.Id)
                .Skip((pageNumber - 1) * size)
                .Take(size)
                .ToList()
                .AsReadOnly();

            return Task.FromResult(Result<IReadOnlyList<InventoryTransactionDto>>.Success(rows));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<IReadOnlyList<InventoryTransactionDto>>.Failure("تعذر تحميل حركات المخزون"));
        }
    }

    public Task<Result<InventoryTransactionDto>> CreateInventoryTransactionAsync(
        CreateInventoryTransactionRequest request, CancellationToken ct = default)
        => Task.FromResult(Result<InventoryTransactionDto>.Failure(UnsupportedMessage, ErrorCodes.InvalidOperation));

    public Task<Result<InventoryTransactionDto>> PostInventoryTransactionAsync(int id, CancellationToken ct = default)
        => Task.FromResult(Result<InventoryTransactionDto>.Failure(UnsupportedMessage, ErrorCodes.InvalidOperation));

    public Task<Result<InventoryTransactionDto>> CancelInventoryTransactionAsync(int id, CancellationToken ct = default)
        => Task.FromResult(Result<InventoryTransactionDto>.Failure(UnsupportedMessage, ErrorCodes.InvalidOperation));

    /// <summary>
    /// حركة واحدة بالمعرّف من بيت البيانات الواحد <see cref="MockInventoryData.Movements"/> —
    /// يستهلكها نقر «التفاصيل» المزدوج في شاشة حركات المخزون.
    /// </summary>
    public Task<Result<InventoryTransactionDto>> GetInventoryTransactionByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var transaction = MockInventoryData.Movements.FirstOrDefault(t => t.Id == id);

            return transaction is null
                ? Task.FromResult(Result<InventoryTransactionDto>.Failure("حركة المخزون غير موجودة"))
                : Task.FromResult(Result<InventoryTransactionDto>.Success(transaction));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<InventoryTransactionDto>.Failure("تعذر تحميل حركة المخزون"));
        }
    }
}
