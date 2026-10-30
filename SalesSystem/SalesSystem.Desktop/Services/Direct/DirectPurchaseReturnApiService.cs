using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة مرتجعات الشراء — يفوّض كل المنطق إلى IPurchaseReturnService في طبقة الأعمال.
/// </summary>
public class DirectPurchaseReturnApiService : IPurchaseReturnApiService
{
    private readonly BizInterfaces.IPurchaseReturnService _bll;
    private readonly ISessionService _session;

    public DirectPurchaseReturnApiService(BizInterfaces.IPurchaseReturnService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<PurchaseReturnDto>>> GetAllAsync(
        string? search = null,
        DateTime? from = null,
        DateTime? to = null,
        int? supplierId = null,
        CancellationToken ct = default)
    {
        try
        {
            // ملاحظة: خدمة الأعمال لا تدعم البحث النصي أو نطاق التاريخ — تُتجاهل هذه المعاملات.
            var result = await _bll.GetAllAsync(
                supplierId: supplierId,
                page: 1,
                pageSize: int.MaxValue,
                includeInactive: false,
                ct: ct);

            return result.IsSuccess
                ? Result<IReadOnlyList<PurchaseReturnDto>>.Success(result.Value!.Items.ToList())
                : Result<IReadOnlyList<PurchaseReturnDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة مرتجعات الشراء");
            return Result<IReadOnlyList<PurchaseReturnDto>>.Failure("تعذر تحميل قائمة مرتجعات الشراء");
        }
    }

    public async Task<Result<PurchaseReturnDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل مرتجع الشراء {ReturnId}", id);
            return Result<PurchaseReturnDto>.Failure("تعذر تحميل مرتجع الشراء");
        }
    }

    public async Task<Result<PurchaseReturnDto>> CreateAsync(CreatePurchaseReturnRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إنشاء مرتجع الشراء");
            return Result<PurchaseReturnDto>.Failure("تعذر إنشاء مرتجع الشراء");
        }
    }

    public async Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.PostAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل مرتجع الشراء {ReturnId}", id);
            return Result.Failure("تعذر ترحيل مرتجع الشراء");
        }
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.CancelAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء مرتجع الشراء {ReturnId}", id);
            return Result.Failure("تعذر إلغاء مرتجع الشراء");
        }
    }
}
