using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة العملاء — يستدعي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// </summary>
public class DirectCustomerService : ICustomerApiService
{
    private readonly BizInterfaces.ICustomerService _bll;
    private readonly ISessionService _session;

    public DirectCustomerService(BizInterfaces.ICustomerService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<CustomerDto>>> GetAllAsync(
        string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // PagedResult<CustomerDto> → IReadOnlyList<CustomerDto>
            var r = await _bll.GetAllAsync(search, 1, int.MaxValue, includeInactive, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<CustomerDto>>.Success(r.Value!.Items.ToList())
                : Result<IReadOnlyList<CustomerDto>>.Failure(r.Error ?? "تعذر تحميل قائمة العملاء", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<CustomerDto>>.Failure("تعذر تحميل قائمة العملاء", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<CustomerDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<CustomerDto>.Failure("تعذر تحميل بيانات العميل", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<CustomerDto>> CreateAsync(CreateCustomerRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, UserId, ct);
        }
        catch (Exception)
        {
            return Result<CustomerDto>.Failure("تعذر إضافة العميل", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, r, UserId, ct);
        }
        catch (Exception)
        {
            return Result<CustomerDto>.Failure("تعذر تحديث بيانات العميل", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.DeleteAsync(id, UserId, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تعطيل العميل", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> ReactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.ReactivateAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر إعادة تفعيل العميل", ErrorCodes.InvalidOperation);
        }
    }
}
