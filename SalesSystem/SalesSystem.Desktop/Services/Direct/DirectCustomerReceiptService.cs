using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL customer-receipt service (in-process, no HTTP).
/// </summary>
public class DirectCustomerReceiptService : ICustomerReceiptApiService
{
    private readonly BizInterfaces.ICustomerReceiptService _bll;
    private readonly ISessionService _session;

    public DirectCustomerReceiptService(BizInterfaces.ICustomerReceiptService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<CustomerReceiptDto>>> GetAllAsync(
        bool includeInactive = false, CancellationToken ct = default)
    {
        try
        {
            // BLL exposes a single GetAllAsync without an includeInactive switch.
            var result = await _bll.GetAllAsync(ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<CustomerReceiptDto>>.Success(
                    result.Value?.ToList() ?? new List<CustomerReceiptDto>())
                : Result<IReadOnlyList<CustomerReceiptDto>>.Failure(
                    result.Error ?? "تعذر تحميل سندات القبض.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCustomerReceiptService.GetAllAsync failed");
            return Result<IReadOnlyList<CustomerReceiptDto>>.Failure(
                "حدث خطأ غير متوقع أثناء تحميل سندات القبض.");
        }
    }

    public async Task<Result<CustomerReceiptDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCustomerReceiptService.GetByIdAsync failed for id {Id}", id);
            return Result<CustomerReceiptDto>.Failure("حدث خطأ غير متوقع أثناء تحميل سند القبض.");
        }
    }

    public async Task<Result<CustomerReceiptDto>> CreateAsync(CreateCustomerReceiptRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCustomerReceiptService.CreateAsync failed");
            return Result<CustomerReceiptDto>.Failure("حدث خطأ غير متوقع أثناء إضافة سند القبض.");
        }
    }

    public async Task<Result<CustomerReceiptDto>> UpdateAsync(int id, UpdateCustomerReceiptRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCustomerReceiptService.UpdateAsync failed for id {Id}", id);
            return Result<CustomerReceiptDto>.Failure("حدث خطأ غير متوقع أثناء تعديل سند القبض.");
        }
    }

    public async Task<Result<CustomerReceiptDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            // BLL PostAsync returns a non-generic Result — re-read the posted receipt for the DTO.
            var posted = await _bll.PostAsync(id, UserId, ct);
            if (!posted.IsSuccess)
                return Result<CustomerReceiptDto>.Failure(
                    posted.Error ?? "تعذر ترحيل سند القبض.", posted.ErrorCode);

            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCustomerReceiptService.PostAsync failed for id {Id}", id);
            return Result<CustomerReceiptDto>.Failure("حدث خطأ غير متوقع أثناء ترحيل سند القبض.");
        }
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CancelAsync(id, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectCustomerReceiptService.CancelAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء إلغاء سند القبض.");
        }
    }
}
