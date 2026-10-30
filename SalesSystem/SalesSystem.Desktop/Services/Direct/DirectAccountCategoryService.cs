using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة تصنيفات الحسابات — ينادي طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectAccountCategoryService : IAccountCategoryApiService
{
    private readonly BizInterfaces.IAccountCategoryService _bll;

    public DirectAccountCategoryService(BizInterfaces.IAccountCategoryService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<AccountCategoryDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<AccountCategoryDto>>.Success(result.Value!)
                : Result<IReadOnlyList<AccountCategoryDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<AccountCategoryDto>>.Failure("تعذر تحميل تصنيفات الحسابات. حاول مرة أخرى.");
        }
    }

    public async Task<Result<AccountCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<AccountCategoryDto>.Failure("تعذر تحميل تصنيف الحساب. حاول مرة أخرى.");
        }
    }

    public async Task<Result<AccountCategoryDto>> CreateAsync(CreateAccountCategoryRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, ct);
        }
        catch (Exception)
        {
            return Result<AccountCategoryDto>.Failure("تعذر إنشاء تصنيف الحساب. حاول مرة أخرى.");
        }
    }

    public async Task<Result<AccountCategoryDto>> UpdateAsync(int id, UpdateAccountCategoryRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, ct);
        }
        catch (Exception)
        {
            return Result<AccountCategoryDto>.Failure("تعذر تحديث تصنيف الحساب. حاول مرة أخرى.");
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            // لا يوجد حذف فعلي — تعطيل التصنيف (RULE-004).
            return await _bll.DeactivateAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تعطيل تصنيف الحساب. حاول مرة أخرى.");
        }
    }
}
