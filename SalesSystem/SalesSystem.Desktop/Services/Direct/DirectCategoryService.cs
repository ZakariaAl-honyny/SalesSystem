using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة تصنيفات المنتجات — يستدعي طبقة الأعمال داخل نفس العملية (بدون HTTP).
/// التصنيف في واجهة سطح المكتب (CategoryDto) يقابل ProductCategory في طبقة الأعمال.
/// </summary>
public class DirectCategoryService : ICategoryApiService
{
    private readonly BizInterfaces.IProductCategoryService _bll;
    private readonly ISessionService _session;

    public DirectCategoryService(BizInterfaces.IProductCategoryService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<CategoryDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            var r = await _bll.GetAllAsync(includeInactive, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<CategoryDto>>.Success(r.Value!.Select(MapToDto).ToList())
                : Result<IReadOnlyList<CategoryDto>>.Failure(r.Error ?? "تعذر تحميل قائمة التصنيفات", r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<CategoryDto>>.Failure("تعذر تحميل قائمة التصنيفات", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryRequest r, CancellationToken ct = default)
    {
        try
        {
            // TODO: CreateCategoryRequest.Description / BranchId لا يدعمهما CreateProductCategoryRequest حالياً.
            var bllRequest = new CreateProductCategoryRequest(r.Name);
            var result = await _bll.CreateAsync(bllRequest, ct);
            return result.IsSuccess
                ? Result<CategoryDto>.Success(MapToDto(result.Value!))
                : Result<CategoryDto>.Failure(result.Error ?? "تعذر إضافة التصنيف", result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<CategoryDto>.Failure("تعذر إضافة التصنيف", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryRequest r, CancellationToken ct = default)
    {
        try
        {
            // TODO: UpdateProductCategoryRequest يقبل الاسم فقط — الوصف (Description) لا يُمرَّر لطبقة الأعمال.
            var bllRequest = new UpdateProductCategoryRequest(r.Name);
            var result = await _bll.UpdateAsync(id, bllRequest, ct);
            if (!result.IsSuccess)
                return Result<CategoryDto>.Failure(result.Error ?? "تعذر تحديث التصنيف", result.ErrorCode);

            // مطابقة عقد الواجهة: خانة "نشط" تُنفَّذ عبر تعطيل/إعادة تفعيل التصنيف.
            if (r.IsActive != result.Value!.IsActive)
            {
                var toggle = r.IsActive
                    ? await _bll.ReactivateAsync(id, ct)
                    : await _bll.DeactivateAsync(id, ct);

                if (!toggle.IsSuccess)
                    return Result<CategoryDto>.Failure(toggle.Error ?? "تعذر تحديث حالة التصنيف", toggle.ErrorCode);

                var refreshed = await _bll.GetByIdAsync(id, ct);
                if (refreshed.IsSuccess)
                    return Result<CategoryDto>.Success(MapToDto(refreshed.Value!));

                var mapped = MapToDto(result.Value!);
                return Result<CategoryDto>.Success(mapped with { IsActive = r.IsActive });
            }

            return Result<CategoryDto>.Success(MapToDto(result.Value!));
        }
        catch (Exception)
        {
            return Result<CategoryDto>.Failure("تعذر تحديث التصنيف", ErrorCodes.InvalidOperation);
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.DeactivateAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر حذف التصنيف", ErrorCodes.InvalidOperation);
        }
    }

    // ─── مابر ────────────────────────────────────────────────────────────────

    private static CategoryDto MapToDto(ProductCategoryDto p) =>
        new(p.Id, p.Name, p.Description, p.IsActive);
}
