using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة تسويات المخزون — يفوّض كل المنطق إلى IInventoryAdjustmentService في طبقة الأعمال.
/// </summary>
public class DirectInventoryAdjustmentService : IInventoryAdjustmentApiService
{
    private readonly BizInterfaces.IInventoryAdjustmentService _bll;
    private readonly ISessionService _session;

    public DirectInventoryAdjustmentService(BizInterfaces.IInventoryAdjustmentService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<InventoryAdjustmentDto>>> GetAllAsync(
        bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // ملاحظة: خدمة الأعمال تُرجع كل التسويات (بما فيها الملغاة) — الفلترة مسؤولية الشاشة.
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<InventoryAdjustmentDto>>.Success(result.Value!)
                : Result<IReadOnlyList<InventoryAdjustmentDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة تسويات المخزون");
            return Result<IReadOnlyList<InventoryAdjustmentDto>>.Failure("تعذر تحميل قائمة تسويات المخزون");
        }
    }

    public async Task<Result<InventoryAdjustmentDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل تسوية المخزون {AdjustmentId}", id);
            return Result<InventoryAdjustmentDto>.Failure("تعذر تحميل تسوية المخزون");
        }
    }

    public async Task<Result<InventoryAdjustmentDto>> CreateAsync(
        CreateInventoryAdjustmentRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إنشاء تسوية مخزون");
            return Result<InventoryAdjustmentDto>.Failure("تعذر إنشاء تسوية المخزون");
        }
    }

    public async Task<Result<InventoryAdjustmentDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.PostAsync(id, UserId, ct);
            if (!result.IsSuccess)
                return Result<InventoryAdjustmentDto>.Failure(result.Error!, result.ErrorCode);

            // خدمة الأعمال تُرجع Result فقط — نُعيد التسوية محدّثة بعد الترحيل.
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل تسوية المخزون {AdjustmentId}", id);
            return Result<InventoryAdjustmentDto>.Failure("تعذر ترحيل تسوية المخزون");
        }
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CancelAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء تسوية المخزون {AdjustmentId}", id);
            return Result.Failure("تعذر إلغاء تسوية المخزون");
        }
    }

    public async Task<Result<InventoryAdjustmentDto>> AddLineAsync(
        int id, AddInventoryAdjustmentLineRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.AddLineAsync(adjustmentId: id, request, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إضافة صنف لتسوية المخزون {AdjustmentId}", id);
            return Result<InventoryAdjustmentDto>.Failure("تعذر إضافة صنف لتسوية المخزون");
        }
    }
}
