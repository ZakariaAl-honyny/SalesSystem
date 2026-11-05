using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Repositories;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Models;
using SalesSystem.Common.Enums;

namespace SalesSystem.Business.Services;

/// <summary>
/// Inventory management service using the v4.10+ schema:
/// InventoryTransactions (immutable, no Status lifecycle) and WarehouseTransfers (Draft/Posted/Cancelled).
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly IUnitOfWork _uow;
    private readonly IDocumentSequenceService _sequenceService;
    private readonly ISystemSettingsRepository _systemSettingsRepo;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IUnitOfWork uow,
        IDocumentSequenceService sequenceService,
        ISystemSettingsRepository systemSettingsRepo,
        ILogger<InventoryService> logger)
    {
        _uow = uow;
        _sequenceService = sequenceService;
        _systemSettingsRepo = systemSettingsRepo;
        _logger = logger;
    }

    #region Foundational Stock Methods

    public async Task<Result<decimal>> GetStockAsync(int productId, int warehouseId, CancellationToken ct)
    {
        var stock = await _uow.WarehouseStocks.FirstOrDefaultAsync(
            ws => ws.WarehouseId == warehouseId && ws.ProductId == productId, ct);

        if (stock == null)
        {
            _logger.LogWarning("Stock record not found for Product {ProductId} in Warehouse {WarehouseId}", productId, warehouseId);
            return Result<decimal>.Failure("لم يتم العثور على سجل مخزون لهذا المنتج في هذا المستودع");
        }

        return Result<decimal>.Success(stock.Quantity);
    }

    public async Task<Result> ValidateStockAsync(int productId, int warehouseId, decimal requiredQty, bool allowNegativeStock = false, CancellationToken ct = default)
    {
        if (allowNegativeStock)
            return Result.Success();

        var stockResult = await GetStockAsync(productId, warehouseId, ct);
        if (!stockResult.IsSuccess) return stockResult;

        if (stockResult.Value < requiredQty)
        {
            _logger.LogWarning("Insufficient stock for Product {ProductId} in Warehouse {WarehouseId}. Available: {Available}, Required: {Required}",
                productId, warehouseId, stockResult.Value, requiredQty);
            return Result.Failure($"المخزون غير كافٍ للمنتج {productId}: المتوفر {stockResult.Value}، المطلوب {requiredQty}");
        }

        return Result.Success();
    }

    public async Task<Result> IncreaseStockAsync(
        int productId, int warehouseId, decimal quantity,
        decimal? unitCost = null, int? userId = null,
        CancellationToken ct = default)
    {
        var stock = await _uow.WarehouseStocks.FirstOrDefaultAsync(
            ws => ws.WarehouseId == warehouseId && ws.ProductId == productId, ct);

        if (stock == null)
        {
            stock = WarehouseStock.Create((short)warehouseId, productId, 0, userId);
            await _uow.WarehouseStocks.AddAsync(stock, ct);
        }
        else
        {
            stock.IncreaseQuantity(quantity);
        }

        _logger.LogInformation("Stock Increased: Product {ProductId}, Warehouse {WarehouseId}, Qty +{Quantity}",
            productId, warehouseId, quantity);
        return Result.Success();
    }

    public async Task<Result> DecreaseStockAsync(
        int productId, int warehouseId, decimal quantity,
        decimal? unitCost = null, int? userId = null,
        CancellationToken ct = default)
    {
        var stock = await _uow.WarehouseStocks.FirstOrDefaultAsync(
            ws => ws.WarehouseId == warehouseId && ws.ProductId == productId, ct);

        if (stock == null)
        {
            _logger.LogWarning("Cannot decrease stock: record not found for Product {ProductId} in Warehouse {WarehouseId}",
                productId, warehouseId);
            return Result.Failure("سجل المخزون غير موجود");
        }

        stock.DecreaseQuantity(quantity);
        _logger.LogInformation("Stock Decreased: Product {ProductId}, Warehouse {WarehouseId}, Qty -{Quantity}",
            productId, warehouseId, quantity);
        return Result.Success();
    }

    #endregion

    #region Warehouse Stock Query

    public async Task<Result<IEnumerable<WarehouseStockDto>>> GetWarehouseStockAsync(
        int warehouseId, string? search, CancellationToken ct)
    {
        Expression<Func<WarehouseStock, bool>> predicate = s =>
            s.WarehouseId == warehouseId &&
            (string.IsNullOrWhiteSpace(search) || s.Product!.Name.Contains(search));

        var items = await _uow.WarehouseStocks.ToListAsync(predicate, null, ct, false, "Product");

        var dtos = items.Select(s => new WarehouseStockDto(
            s.WarehouseId,
            s.Warehouse?.Name,
            s.ProductId,
            s.Product?.Name ?? "غير معروف",
            null,
            s.Quantity,
            0m // AvgCost no longer stored on WarehouseStock
        ));

        return Result<IEnumerable<WarehouseStockDto>>.Success(dtos);
    }

    public async Task<Result<PagedResult<WarehouseStockDto>>> GetWarehouseStocksAsync(
        int? warehouseId, int? productId, int page, int pageSize, CancellationToken ct)
    {
        Expression<Func<WarehouseStock, bool>> predicate = s =>
            (!warehouseId.HasValue || s.WarehouseId == warehouseId.Value) &&
            (!productId.HasValue || s.ProductId == productId.Value);

        var totalItems = await _uow.WarehouseStocks.CountAsync(predicate, ct);
        var items = await _uow.WarehouseStocks.ToListAsync(
            predicate,
            q => q.OrderBy(s => s.Warehouse!.Name).ThenBy(s => s.Product!.Name)
                  .Skip((page - 1) * pageSize).Take(pageSize),
            ct, false, "Warehouse", "Product");

        var dtos = items.Select(s => new WarehouseStockDto(
            s.WarehouseId,
            s.Warehouse?.Name,
            s.ProductId,
            s.Product?.Name ?? "غير معروف",
            null,
            s.Quantity,
            0m
        )).ToList();

        return Result<PagedResult<WarehouseStockDto>>.Success(
            PagedResult<WarehouseStockDto>.Create(dtos, totalItems, page, pageSize));
    }

    #endregion

    #region Inventory Transaction (immutable — written once, no Status)

    public async Task<Result<InventoryTransactionDto>> CreateTransactionAsync(
        CreateInventoryTransactionRequest request, int userId, CancellationToken ct)
    {
        if (request.Lines == null || request.Lines.Count == 0)
            return Result<InventoryTransactionDto>.Failure("يجب إضافة أصناف للمعاملة");

        var movementType = (InventoryTransactionType)request.MovementType;

        // Auto-generate TransactionNo if not provided
        var transactionNo = request.TransactionNo;
        if (string.IsNullOrWhiteSpace(transactionNo))
        {
            var seqResult = await _sequenceService.GetNextIntAsync("InventoryTransaction", ct);
            if (!seqResult.IsSuccess)
                return Result<InventoryTransactionDto>.Failure("فشل في توليد رقم المعاملة");
            transactionNo = seqResult.Value.ToString("D6");
        }

        var tx = InventoryTransaction.Create(
            transactionNo,
            movementType,
            request.WarehouseId,
            request.ReferenceType.HasValue ? (InventoryReferenceType?)request.ReferenceType.Value : null,
            request.ReferenceId,
            request.Notes,
            userId);

        // Process lines
        foreach (var lineReq in request.Lines)
        {
            var line = InventoryTransactionLine.Create(
                tx.Id,
                lineReq.ProductUnitId,
                lineReq.Quantity,
                lineReq.UnitCost,
                lineReq.BatchNo,
                lineReq.ExpiryDate,
                lineReq.WarehouseId);
            tx.AddLine(line);

            // Update stock immediately (transaction is immutable — no Draft/Posted lifecycle)
            var warehouseId = lineReq.WarehouseId ?? request.WarehouseId;
            var productUnit = await _uow.ProductUnits.GetByIdAsync(lineReq.ProductUnitId, ct);
            if (productUnit == null)
                return Result<InventoryTransactionDto>.Failure($"وحدة المنتج {lineReq.ProductUnitId} غير موجودة");

            if (IsOutgoingMovement(movementType))
            {
                await DecreaseStockAsync(productUnit.ProductId, warehouseId, lineReq.Quantity, lineReq.UnitCost, userId, ct);
            }
            else
            {
                await IncreaseStockAsync(productUnit.ProductId, warehouseId, lineReq.Quantity, lineReq.UnitCost, userId, ct);
            }
        }

        await _uow.InventoryTransactions.AddAsync(tx, ct);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Inventory Transaction created: #{No}, Type: {Type}",
            tx.TransactionNo, tx.MovementType);

        return await GetTransactionByIdAsync(tx.Id, ct);
    }

    /// <summary>
    /// InventoryTransactions are immutable — creation IS posting. This is a no-op returning the existing transaction.
    /// </summary>
    public async Task<Result<InventoryTransactionDto>> PostTransactionAsync(
        int transactionId, int userId, CancellationToken ct)
    {
        return await GetTransactionByIdAsync(transactionId, ct);
    }

    /// <summary>
    /// InventoryTransactions are immutable and cannot be cancelled.
    /// </summary>
    public async Task<Result<InventoryTransactionDto>> CancelTransactionAsync(
        int transactionId, int userId, CancellationToken ct)
    {
        return Result<InventoryTransactionDto>.Failure("المعاملة غير قابلة للإلغاء — المعاملات غير قابلة للتعديل");
    }

    public async Task<Result<InventoryTransactionDto>> GetTransactionByIdAsync(int id, CancellationToken ct)
    {
        var tx = await _uow.InventoryTransactions.FirstOrDefaultAsync(
            t => t.Id == id, ct, "Warehouse", "Lines");

        if (tx == null)
            return Result<InventoryTransactionDto>.Failure("المعاملة غير موجودة", ErrorCodes.NotFound);

        return Result<InventoryTransactionDto>.Success(MapTransactionToDto(tx));
    }

    public async Task<Result<PagedResult<InventoryTransactionDto>>> GetAllTransactionsAsync(
        int? warehouseId, int? movementType, int page, int pageSize, CancellationToken ct)
    {
        Expression<Func<InventoryTransaction, bool>> predicate = t =>
            (!warehouseId.HasValue || t.WarehouseId == warehouseId.Value) &&
            (!movementType.HasValue || (int)t.MovementType == movementType.Value);

        var totalItems = await _uow.InventoryTransactions.CountAsync(predicate, ct);
        var items = await _uow.InventoryTransactions.ToListAsync(
            predicate,
            q => q.OrderByDescending(t => t.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize),
            ct, false, "Warehouse");

        var dtos = items.Select(MapTransactionToDto).ToList();

        return Result<PagedResult<InventoryTransactionDto>>.Success(
            PagedResult<InventoryTransactionDto>.Create(dtos, totalItems, page, pageSize));
    }

    #endregion

    #region Movement History

    public async Task<Result<PagedResult<InventoryTransactionDto>>> GetMovementsAsync(
        int? productId, int? warehouseId, int? movementType, int page, int pageSize, CancellationToken ct)
    {
        // Reuse GetAllTransactionsAsync
        return await GetAllTransactionsAsync(warehouseId, movementType, page, pageSize, ct);
    }

    #endregion

    #region Mapping Helpers

    private static InventoryTransactionDto MapTransactionToDto(InventoryTransaction tx)
    {
        return new InventoryTransactionDto(
            tx.Id,
            tx.TransactionNo,
            (byte)tx.MovementType,
            tx.WarehouseId,
            tx.Warehouse?.Name ?? "غير معروف",
            tx.ReferenceId,
            tx.ReferenceType.HasValue ? (byte?)tx.ReferenceType.Value : null,
            tx.Notes,
            tx.CreatedAt,
            tx.CreatedByUserId,
            tx.Lines.Select(l => new InventoryTransactionLineDto(
                l.Id,
                l.InventoryTransactionId,
                l.ProductUnitId,
                null,
                l.Quantity,
                l.UnitCost,
                l.BatchNo,
                l.ExpiryDate,
                l.WarehouseId
            )).ToList()
        );
    }

    private static bool IsOutgoingMovement(InventoryTransactionType type) => type switch
    {
        InventoryTransactionType.Sale => true,
        InventoryTransactionType.PurchaseReturn => true,
        InventoryTransactionType.TransferOut => true,
        InventoryTransactionType.Damage => true,
        InventoryTransactionType.Adjustment => true,
        InventoryTransactionType.InternalIssue => true,
        _ => false
    };

    #endregion
}
