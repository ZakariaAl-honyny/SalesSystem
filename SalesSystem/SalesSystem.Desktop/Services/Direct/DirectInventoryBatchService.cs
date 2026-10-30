using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة دفعات المخزون — يفوّض كل المنطق إلى IInventoryBatchService في طبقة الأعمال.
/// </summary>
public class DirectInventoryBatchService : IInventoryBatchApiService
{
    private readonly BizInterfaces.IInventoryBatchService _bll;
    private readonly ISessionService _session;

    public DirectInventoryBatchService(BizInterfaces.IInventoryBatchService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<InventoryBatchDto>>> GetByProductAsync(
        int productId, int? warehouseId = null, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetByProductAsync(productId, warehouseId, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<InventoryBatchDto>>.Success(result.Value!)
                : Result<IReadOnlyList<InventoryBatchDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل دفعات صنف {ProductId}", productId);
            return Result<IReadOnlyList<InventoryBatchDto>>.Failure("تعذر تحميل دفعات الصنف");
        }
    }

    public async Task<Result<InventoryBatchDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل دفعة مخزون {BatchId}", id);
            return Result<InventoryBatchDto>.Failure("تعذر تحميل دفعة المخزون");
        }
    }

    public async Task<Result<InventoryBatchDto>> CreateAsync(
        CreateInventoryBatchRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إنشاء دفعة مخزون");
            return Result<InventoryBatchDto>.Failure("تعذر إنشاء دفعة المخزون");
        }
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.DeactivateAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء تنشيط دفعة المخزون {BatchId}", id);
            return Result.Failure("تعذر إلغاء تنشيط دفعة المخزون");
        }
    }
}
