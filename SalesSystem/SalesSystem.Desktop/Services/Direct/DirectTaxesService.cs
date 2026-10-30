using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة الضرائب — يستدعي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// </summary>
public class DirectTaxesService : ITaxesApiService
{
    private readonly BizInterfaces.ITaxService _bll;

    public DirectTaxesService(BizInterfaces.ITaxService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<TaxDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            var r = await _bll.GetAllAsync(ct);
            if (!r.IsSuccess)
                return Result<IReadOnlyList<TaxDto>>.Failure(r.Error ?? "تعذر تحميل قائمة الضرائب", r.ErrorCode);

            // طبقة الأعمال تعيد الكل — تُطبَّق خانة "تضمين المعطلة" هنا لمطابقة عقد الواجهة.
            List<TaxDto> items = includeInactive
                ? r.Value!
                : r.Value!.Where(t => t.IsActive).ToList();

            return Result<IReadOnlyList<TaxDto>>.Success(items);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<TaxDto>>.Failure("تعذر تحميل قائمة الضرائب", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<TaxDto>> CreateAsync(CreateTaxRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, ct);
        }
        catch (Exception)
        {
            return Result<TaxDto>.Failure("تعذر إضافة الضريبة", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<TaxDto>> UpdateAsync(int id, UpdateTaxRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, ct);
        }
        catch (Exception)
        {
            return Result<TaxDto>.Failure("تعذر تحديث بيانات الضريبة", ErrorCodes.InvalidOperation);
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
            return Result.Failure("تعذر تعطيل الضريبة", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> DeletePermanentlyAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.PermanentDeleteAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر حذف الضريبة نهائياً", ErrorCodes.InvalidOperation);
        }
    }
}
