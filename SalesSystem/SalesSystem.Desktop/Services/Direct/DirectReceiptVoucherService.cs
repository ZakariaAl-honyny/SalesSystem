using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL receipt-voucher service (in-process, no HTTP).
/// </summary>
public class DirectReceiptVoucherService : IReceiptVoucherApiService
{
    private readonly BizInterfaces.IReceiptVoucherService _bll;
    private readonly ISessionService _session;

    public DirectReceiptVoucherService(BizInterfaces.IReceiptVoucherService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<ReceiptVoucherDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(search, from, to, 1, int.MaxValue, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<ReceiptVoucherDto>>.Success(
                    result.Value?.Items.ToList() ?? new List<ReceiptVoucherDto>())
                : Result<IReadOnlyList<ReceiptVoucherDto>>.Failure(
                    result.Error ?? "تعذر تحميل سندات القبض.", result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectReceiptVoucherService.GetAllAsync failed");
            return Result<IReadOnlyList<ReceiptVoucherDto>>.Failure(
                "حدث خطأ غير متوقع أثناء تحميل سندات القبض.");
        }
    }

    public async Task<Result<ReceiptVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectReceiptVoucherService.GetByIdAsync failed for id {Id}", id);
            return Result<ReceiptVoucherDto>.Failure("حدث خطأ غير متوقع أثناء تحميل سند القبض.");
        }
    }

    public async Task<Result<ReceiptVoucherDto>> CreateAsync(CreateReceiptVoucherRequest request, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectReceiptVoucherService.CreateAsync failed");
            return Result<ReceiptVoucherDto>.Failure("حدث خطأ غير متوقع أثناء إضافة سند القبض.");
        }
    }

    public async Task<Result<ReceiptVoucherDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.PostAsync(id, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectReceiptVoucherService.PostAsync failed for id {Id}", id);
            return Result<ReceiptVoucherDto>.Failure("حدث خطأ غير متوقع أثناء ترحيل سند القبض.");
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
            Serilog.Log.Error(ex, "DirectReceiptVoucherService.CancelAsync failed for id {Id}", id);
            return Result.Failure("حدث خطأ غير متوقع أثناء إلغاء سند القبض.");
        }
    }
}
