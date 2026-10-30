using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة مرتجعات البيع — يفوّض كل المنطق إلى ISalesReturnService في طبقة الأعمال.
/// </summary>
public class DirectSalesReturnApiService : ISalesReturnApiService
{
    private readonly BizInterfaces.ISalesReturnService _bll;
    private readonly ISessionService _session;

    public DirectSalesReturnApiService(BizInterfaces.ISalesReturnService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<SalesReturnDto>>> GetAllAsync(
        string? search = null,
        DateTime? from = null,
        DateTime? to = null,
        int? customerId = null,
        CancellationToken ct = default)
    {
        try
        {
            // ملاحظة: خدمة الأعمال لا تدعم البحث النصي أو نطاق التاريخ — تُتجاهل هذه المعاملات.
            var result = await _bll.GetAllAsync(
                customerId: customerId,
                page: 1,
                pageSize: int.MaxValue,
                includeInactive: false,
                ct: ct);

            return result.IsSuccess
                ? Result<IReadOnlyList<SalesReturnDto>>.Success(result.Value!.Items.ToList())
                : Result<IReadOnlyList<SalesReturnDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة مرتجعات البيع");
            return Result<IReadOnlyList<SalesReturnDto>>.Failure("تعذر تحميل قائمة مرتجعات البيع");
        }
    }

    public async Task<Result<SalesReturnDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل مرتجع البيع {ReturnId}", id);
            return Result<SalesReturnDto>.Failure("تعذر تحميل مرتجع البيع");
        }
    }

    public async Task<Result<SalesReturnDto>> CreateAsync(CreateSalesReturnRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إنشاء مرتجع البيع");
            return Result<SalesReturnDto>.Failure("تعذر إنشاء مرتجع البيع");
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
            Serilog.Log.Error(ex, "فشل ترحيل مرتجع البيع {ReturnId}", id);
            return Result.Failure("تعذر ترحيل مرتجع البيع");
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
            Serilog.Log.Error(ex, "فشل إلغاء مرتجع البيع {ReturnId}", id);
            return Result.Failure("تعذر إلغاء مرتجع البيع");
        }
    }
}
