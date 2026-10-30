using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة الموردين — يستدعي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// </summary>
public class DirectSupplierService : ISupplierApiService
{
    private readonly BizInterfaces.ISupplierService _bll;
    private readonly ISessionService _session;

    public DirectSupplierService(BizInterfaces.ISupplierService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<SupplierDto>>> GetAllAsync(
        string? search = null, bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // PagedResult<SupplierDto> → IReadOnlyList<SupplierDto>
            var r = await _bll.GetAllAsync(search, 1, int.MaxValue, includeInactive, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<SupplierDto>>.Success(r.Value!.Items.ToList())
                : Result<IReadOnlyList<SupplierDto>>.Failure(r.Error ?? "تعذر تحميل قائمة الموردين", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<SupplierDto>>.Failure("تعذر تحميل قائمة الموردين", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<SupplierDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<SupplierDto>.Failure("تعذر تحميل بيانات المورد", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<SupplierDto>> CreateAsync(CreateSupplierRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, UserId, ct);
        }
        catch (Exception)
        {
            return Result<SupplierDto>.Failure("تعذر إضافة المورد", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<SupplierDto>> UpdateAsync(int id, UpdateSupplierRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, r, UserId, ct);
        }
        catch (Exception)
        {
            return Result<SupplierDto>.Failure("تعذر تحديث بيانات المورد", ErrorCodes.InvalidOperation);
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
            return Result.Failure("تعذر تعطيل المورد", ErrorCodes.InvalidOperation);
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
            return Result.Failure("تعذر إعادة تفعيل المورد", ErrorCodes.InvalidOperation);
        }
    }
}
