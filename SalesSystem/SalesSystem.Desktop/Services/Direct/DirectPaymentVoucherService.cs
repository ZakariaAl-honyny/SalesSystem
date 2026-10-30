using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL payment-voucher service (in-process, no HTTP).
/// </summary>
public class DirectPaymentVoucherService : IPaymentVoucherApiService
{
    private readonly BizInterfaces.IPaymentVoucherService _bll;
    private readonly ISessionService _session;

    public DirectPaymentVoucherService(BizInterfaces.IPaymentVoucherService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<PaymentVoucherDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(search, from, to, 1, int.MaxValue, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<PaymentVoucherDto>>.Success(
                    result.Value?.Items.ToList() ?? new List<PaymentVoucherDto>())
                : Result<IReadOnlyList<PaymentVoucherDto>>.Failure(
                    result.Error ?? "تعذر تحميل سندات الدفع.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectPaymentVoucherService.GetAllAsync failed");
            return Result<IReadOnlyList<PaymentVoucherDto>>.Failure(
                "حدث خطأ غير متوقع أثناء تحميل سندات الدفع.");
        }
    }

    public async Task<Result<PaymentVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectPaymentVoucherService.GetByIdAsync failed for id {Id}", id);
            return Result<PaymentVoucherDto>.Failure("حدث خطأ غير متوقع أثناء تحميل سند الدفع.");
        }
    }

    public async Task<Result<PaymentVoucherDto>> CreateAsync(CreatePaymentVoucherRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectPaymentVoucherService.CreateAsync failed");
            return Result<PaymentVoucherDto>.Failure("حدث خطأ غير متوقع أثناء إضافة سند الدفع.");
        }
    }

    public async Task<Result<PaymentVoucherDto>> UpdateAsync(int id, UpdatePaymentVoucherRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectPaymentVoucherService.UpdateAsync failed for id {Id}", id);
            return Result<PaymentVoucherDto>.Failure("حدث خطأ غير متوقع أثناء تعديل سند الدفع.");
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
            Serilog.Log.Error(ex, "DirectPaymentVoucherService.DeleteAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء حذف سند الدفع.");
        }
    }

    public async Task<Result<PaymentVoucherDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.PostAsync(id, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectPaymentVoucherService.PostAsync failed for id {Id}", id);
            return Result<PaymentVoucherDto>.Failure("حدث خطأ غير متوقع أثناء ترحيل سند الدفع.");
        }
    }

    public async Task<Result<PaymentVoucherDto>> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CancelAsync(id, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectPaymentVoucherService.CancelAsync failed for id {Id}", id);
            return Result<PaymentVoucherDto>.Failure("حدث خطأ غير متوقع أثناء إلغاء سند الدفع.");
        }
    }
}
