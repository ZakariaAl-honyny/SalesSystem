using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL bank service (in-process, no HTTP).
/// </summary>
public class DirectBankService : IBankApiService
{
    private readonly BizInterfaces.IBankService _bll;
    private readonly ISessionService _session;

    public DirectBankService(BizInterfaces.IBankService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<BankDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // BLL exposes a single GetAllAsync without an includeInactive switch.
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<BankDto>>.Success(
                    result.Value?.ToList() ?? new List<BankDto>())
                : Result<IReadOnlyList<BankDto>>.Failure(
                    result.Error ?? "تعذر تحميل البنوك.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBankService.GetAllAsync failed");
            return Result<IReadOnlyList<BankDto>>.Failure("حدث خطأ غير متوقع أثناء تحميل البنوك.");
        }
    }

    public async Task<Result<BankDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBankService.GetByIdAsync failed for id {Id}", id);
            return Result<BankDto>.Failure("حدث خطأ غير متوقع أثناء تحميل البنك.");
        }
    }

    public async Task<Result<BankDto>> CreateAsync(CreateBankRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBankService.CreateAsync failed");
            return Result<BankDto>.Failure("حدث خطأ غير متوقع أثناء إضافة البنك.");
        }
    }

    public async Task<Result<BankDto>> UpdateAsync(int id, UpdateBankRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBankService.UpdateAsync failed for id {Id}", id);
            return Result<BankDto>.Failure("حدث خطأ غير متوقع أثناء تعديل البنك.");
        }
    }

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.DeactivateAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectBankService.DeactivateAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء تعطيل البنك.");
        }
    }
}
