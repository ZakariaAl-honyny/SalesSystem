using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة المخزون — يفوّض كل المنطق إلى IInventoryService في طبقة الأعمال.
/// </summary>
public class DirectInventoryService : IInventoryApiService
{
    private readonly BizInterfaces.IInventoryService _bll;
    private readonly ISessionService _session;

    public DirectInventoryService(BizInterfaces.IInventoryService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByWarehouseAsync(
        int warehouseId, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetWarehouseStockAsync(warehouseId, search: null, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<WarehouseStockDto>>.Success(result.Value!.ToList())
                : Result<IReadOnlyList<WarehouseStockDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل أرصدة المخزون للمستودع {WarehouseId}", warehouseId);
            return Result<IReadOnlyList<WarehouseStockDto>>.Failure("تعذر تحميل أرصدة المخزون للمستودع");
        }
    }

    public async Task<Result<IReadOnlyList<WarehouseStockDto>>> GetStockByProductAsync(
        int productId, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetWarehouseStocksAsync(
                warehouseId: null,
                productId: productId,
                page: 1,
                pageSize: int.MaxValue,
                ct: ct);

            return result.IsSuccess
                ? Result<IReadOnlyList<WarehouseStockDto>>.Success(result.Value!.Items.ToList())
                : Result<IReadOnlyList<WarehouseStockDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل أرصدة المخزون للصنف {ProductId}", productId);
            return Result<IReadOnlyList<WarehouseStockDto>>.Failure("تعذر تحميل أرصدة المخزون للصنف");
        }
    }

    public async Task<Result<IReadOnlyList<InventoryTransactionDto>>> GetMovementsAsync(
        int? productId = null,
        int? warehouseId = null,
        int? movementType = null,
        int page = 1,
        int pageSize = 50,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetMovementsAsync(
                productId: productId,
                warehouseId: warehouseId,
                transactionType: movementType,
                page: page,
                pageSize: pageSize,
                ct: ct);

            return result.IsSuccess
                ? Result<IReadOnlyList<InventoryTransactionDto>>.Success(result.Value!.Items.ToList())
                : Result<IReadOnlyList<InventoryTransactionDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل حركات المخزون");
            return Result<IReadOnlyList<InventoryTransactionDto>>.Failure("تعذر تحميل حركات المخزون");
        }
    }

    public async Task<Result<InventoryTransactionDto>> CreateInventoryTransactionAsync(
        CreateInventoryTransactionRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateTransactionAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إنشاء حركة مخزون");
            return Result<InventoryTransactionDto>.Failure("تعذر إنشاء حركة المخزون");
        }
    }

    public async Task<Result<InventoryTransactionDto>> PostInventoryTransactionAsync(
        int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.PostTransactionAsync(id, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل حركة المخزون {TransactionId}", id);
            return Result<InventoryTransactionDto>.Failure("تعذر ترحيل حركة المخزون");
        }
    }

    public async Task<Result<InventoryTransactionDto>> CancelInventoryTransactionAsync(
        int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CancelTransactionAsync(id, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء حركة المخزون {TransactionId}", id);
            return Result<InventoryTransactionDto>.Failure("تعذر إلغاء حركة المخزون");
        }
    }

    public async Task<Result<InventoryTransactionDto>> GetInventoryTransactionByIdAsync(
        int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetTransactionByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل حركة المخزون {TransactionId}", id);
            return Result<InventoryTransactionDto>.Failure("تعذر تحميل حركة المخزون");
        }
    }
}
