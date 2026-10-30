using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة دليل الحسابات — ينادي طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectAccountService : IAccountApiService
{
    private readonly BizInterfaces.IAccountService _bll;
    private readonly ISessionService _session;

    public DirectAccountService(BizInterfaces.IAccountService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<AccountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // ملاحظة: خدمة الأعمال لا تدعم تصفية غير النشط — المعامل includeInactive مُهمَل.
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<AccountDto>>.Success(result.Value!)
                : Result<IReadOnlyList<AccountDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<AccountDto>>.Failure("تعذر تحميل دليل الحسابات. حاول مرة أخرى.");
        }
    }

    public async Task<Result<AccountDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception)
        {
            return Result<AccountDto>.Failure("تعذر تحميل الحساب. حاول مرة أخرى.");
        }
    }

    public async Task<Result<IReadOnlyList<AccountTreeNodeDto>>> GetTreeAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetTreeAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<AccountTreeNodeDto>>.Success(result.Value!)
                : Result<IReadOnlyList<AccountTreeNodeDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<AccountTreeNodeDto>>.Failure("تعذر تحميل شجرة الحسابات. حاول مرة أخرى.");
        }
    }

    public async Task<Result<int>> CreateAsync(CreateAccountRequest request, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.CreateAsync(request, UserId, ct);
            if (!result.IsSuccess || result.Value is null)
                return Result<int>.Failure(result.Error ?? "تعذر إنشاء الحساب.", result.ErrorCode);

            // الواجهة تحتاج رقم الحساب فقط بعد الإنشاء.
            return Result<int>.Success(result.Value.Id);
        }
        catch (Exception)
        {
            return Result<int>.Failure("تعذر إنشاء الحساب. حاول مرة أخرى.");
        }
    }

    public async Task<Result> UpdateAsync(int id, UpdateAccountRequest request, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.UpdateAsync(id, request, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تحديث الحساب. حاول مرة أخرى.");
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
            return Result.Failure("تعذر حذف الحساب. حاول مرة أخرى.");
        }
    }

    public async Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.PermanentDeleteAsync(id, ct);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر حذف الحساب نهائياً. حاول مرة أخرى.");
        }
    }
}
