using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة وحدات القياس — يستدعي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// </summary>
public class DirectUnitService : IUnitApiService
{
    private readonly BizInterfaces.IUnitService _bll;

    public DirectUnitService(BizInterfaces.IUnitService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<UnitDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // PagedResult<UnitDto> → IReadOnlyList<UnitDto>
            var r = await _bll.GetAllAsync(null, 1, int.MaxValue, includeInactive, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<UnitDto>>.Success(r.Value!.Items.ToList())
                : Result<IReadOnlyList<UnitDto>>.Failure(r.Error ?? "تعذر تحميل قائمة الوحدات", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<UnitDto>>.Failure("تعذر تحميل قائمة الوحدات", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<UnitDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<UnitDto>.Failure("تعذر تحميل بيانات الوحدة", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<UnitDto>> CreateAsync(CreateUnitRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, ct);
        }
        catch (Exception)
        {
            return Result<UnitDto>.Failure("تعذر إضافة الوحدة", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<UnitDto>> UpdateAsync(int id, UpdateUnitRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, r, ct);
        }
        catch (Exception)
        {
            return Result<UnitDto>.Failure("تعذر تحديث بيانات الوحدة", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.DeleteAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر حذف الوحدة", ErrorCodes.InvalidOperation);
        }
    }
}
