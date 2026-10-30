using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL cash-box service (in-process, no HTTP).
/// </summary>
public class DirectCashBoxService : ICashBoxApiService
{
    private readonly BizInterfaces.ICashBoxService _bll;
    private readonly ISessionService _session;

    public DirectCashBoxService(BizInterfaces.ICashBoxService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<CashBoxDto>>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<CashBoxDto>>.Success(
                    result.Value?.ToList() ?? new List<CashBoxDto>())
                : Result<IReadOnlyList<CashBoxDto>>.Failure(
                    result.Error ?? "تعذر تحميل الصناديق.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCashBoxService.GetAllAsync failed");
            return Result<IReadOnlyList<CashBoxDto>>.Failure("حدث خطأ غير متوقع أثناء تحميل الصناديق.");
        }
    }

    public async Task<Result<CashBoxDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCashBoxService.GetByIdAsync failed for id {Id}", id);
            return Result<CashBoxDto>.Failure("حدث خطأ غير متوقع أثناء تحميل الصندوق.");
        }
    }

    public async Task<Result<CashBoxDto>> CreateAsync(CreateCashBoxRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCashBoxService.CreateAsync failed");
            return Result<CashBoxDto>.Failure("حدث خطأ غير متوقع أثناء إضافة الصندوق.");
        }
    }

    public async Task<Result<CashBoxDto>> UpdateAsync(int id, UpdateCashBoxRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCashBoxService.UpdateAsync failed for id {Id}", id);
            return Result<CashBoxDto>.Failure("حدث خطأ غير متوقع أثناء تعديل الصندوق.");
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
            Serilog.Log.Error(ex, "DirectCashBoxService.DeactivateAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء تعطيل الصندوق.");
        }
    }

    public async Task<Result> TransferAsync(CashTransferRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.TransferAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCashBoxService.TransferAsync failed");
            return Result.Failure("حدث خطأ غير متوقع أثناء تنفيذ التحويل بين الصناديق.");
        }
    }
}
