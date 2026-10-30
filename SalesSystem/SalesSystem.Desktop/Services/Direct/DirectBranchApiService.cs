using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL branch service (in-process, no HTTP).
/// </summary>
public class DirectBranchApiService : IBranchApiService
{
    private readonly BizInterfaces.IBranchService _bll;

    public DirectBranchApiService(BizInterfaces.IBranchService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<BranchDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(includeInactive, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<BranchDto>>.Success(
                    result.Value?.ToList() ?? new List<BranchDto>())
                : Result<IReadOnlyList<BranchDto>>.Failure(
                    result.Error ?? "تعذر تحميل الفروع.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBranchApiService.GetAllAsync failed");
            return Result<IReadOnlyList<BranchDto>>.Failure("حدث خطأ غير متوقع أثناء تحميل الفروع.");
        }
    }
}
