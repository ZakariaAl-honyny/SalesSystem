using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL expense service (in-process, no HTTP).
/// </summary>
public class DirectExpenseService : IExpenseApiService
{
    private readonly BizInterfaces.IExpenseService _bll;
    private readonly ISessionService _session;

    public DirectExpenseService(BizInterfaces.IExpenseService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<ExpenseDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(search, from, to, 1, int.MaxValue, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<ExpenseDto>>.Success(
                    result.Value?.Items.ToList() ?? new List<ExpenseDto>())
                : Result<IReadOnlyList<ExpenseDto>>.Failure(
                    result.Error ?? "تعذر تحميل المصروفات.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectExpenseService.GetAllAsync failed");
            return Result<IReadOnlyList<ExpenseDto>>.Failure("حدث خطأ غير متوقع أثناء تحميل المصروفات.");
        }
    }

    public async Task<Result<ExpenseDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectExpenseService.GetByIdAsync failed for id {Id}", id);
            return Result<ExpenseDto>.Failure("حدث خطأ غير متوقع أثناء تحميل المصروف.");
        }
    }

    public async Task<Result<ExpenseDto>> CreateAsync(CreateExpenseRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectExpenseService.CreateAsync failed");
            return Result<ExpenseDto>.Failure("حدث خطأ غير متوقع أثناء إضافة المصروف.");
        }
    }

    public async Task<Result<ExpenseDto>> UpdateAsync(int id, UpdateExpenseRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectExpenseService.UpdateAsync failed for id {Id}", id);
            return Result<ExpenseDto>.Failure("حدث خطأ غير متوقع أثناء تعديل المصروف.");
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.DeleteAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectExpenseService.DeleteAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء حذف المصروف.");
        }
    }

    public async Task<Result<ExpenseDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.PostAsync(id, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectExpenseService.PostAsync failed for id {Id}", id);
            return Result<ExpenseDto>.Failure("حدث خطأ غير متوقع أثناء ترحيل المصروف.");
        }
    }

    public async Task<Result<ExpenseDto>> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CancelAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectExpenseService.CancelAsync failed for id {Id}", id);
            return Result<ExpenseDto>.Failure("حدث خطأ غير متوقع أثناء إلغاء المصروف.");
        }
    }
}
