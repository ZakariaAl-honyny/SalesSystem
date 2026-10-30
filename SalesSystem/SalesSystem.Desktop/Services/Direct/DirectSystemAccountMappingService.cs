using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Models.Accounting.Enums;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة ربط الحسابات النظامية — ينادي طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectSystemAccountMappingService : ISystemAccountMappingApiService
{
    private readonly BizInterfaces.ISystemAccountService _bll;

    public DirectSystemAccountMappingService(BizInterfaces.ISystemAccountService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<SystemAccountMappingDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllMappingsAsync(branchId: null, ct: ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<SystemAccountMappingDto>>.Success(result.Value!)
                : Result<IReadOnlyList<SystemAccountMappingDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<SystemAccountMappingDto>>.Failure("تعذر تحميل ربط الحسابات النظامية. حاول مرة أخرى.");
        }
    }

    public async Task<Result<SystemAccountMappingDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            // خدمة الأعمال لا توفر جلباً بالمعرّف — نجلب الكل ونطابق المعرّف.
            var all = await _bll.GetAllMappingsAsync(branchId: null, ct: ct);
            if (!all.IsSuccess)
                return Result<SystemAccountMappingDto>.Failure(all.Error!, all.ErrorCode);

            var byId = all.Value?.FirstOrDefault(m => m.Id == id);
            if (byId is not null)
                return Result<SystemAccountMappingDto>.Success(byId);

            // احتياط: إذا كان المعرّف يمثّل قيمة SystemAccountKey نفسها.
            if (id > 0 && id <= byte.MaxValue && Enum.IsDefined(typeof(SystemAccountKey), (byte)id))
                return await _bll.GetMappingAsync((SystemAccountKey)id, branchId: null, ct: ct);

            return Result<SystemAccountMappingDto>.Failure("تعيين الحساب غير موجود.");
        }
        catch (Exception)
        {
            return Result<SystemAccountMappingDto>.Failure("تعذر تحميل تعيين الحساب. حاول مرة أخرى.");
        }
    }

    public async Task<Result> UpdateAsync(int id, UpdateSystemAccountMappingRequest request, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.UpdateMappingAsync(id, request, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result.Failure("تعذر تحديث تعيين الحساب. حاول مرة أخرى.");
        }
    }
}
