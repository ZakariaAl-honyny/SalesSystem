using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// Desktop wrapper over the BLL supplier-payment service (in-process, no HTTP).
/// </summary>
public class DirectSupplierPaymentService : ISupplierPaymentApiService
{
    private readonly BizInterfaces.ISupplierPaymentService _bll;
    private readonly ISessionService _session;

    public DirectSupplierPaymentService(BizInterfaces.ISupplierPaymentService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<SupplierPaymentDto>>> GetAllAsync(
        int? supplierId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        try
        {
            // BLL filters by search text only; supplier filtering is applied client-side on the returned set.
            var result = await _bll.GetAllAsync(null, from, to, 1, int.MaxValue, ct);
            if (!result.IsSuccess)
                return Result<IReadOnlyList<SupplierPaymentDto>>.Failure(
                    result.Error ?? "تعذر تحميل مدفوعات الموردين.", result.ErrorCode);

            var items = result.Value?.Items.ToList() ?? new List<SupplierPaymentDto>();
            if (supplierId.HasValue)
                items = items.Where(p => p.SupplierId == supplierId.Value).ToList();

            return Result<IReadOnlyList<SupplierPaymentDto>>.Success(items);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectSupplierPaymentService.GetAllAsync failed");
            return Result<IReadOnlyList<SupplierPaymentDto>>.Failure(
                "حدث خطأ غير متوقع أثناء تحميل مدفوعات الموردين.");
        }
    }

    public async Task<Result<SupplierPaymentDto>> CreateAsync(CreateSupplierPaymentRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "DirectSupplierPaymentService.CreateAsync failed");
            return Result<SupplierPaymentDto>.Failure("حدث خطأ غير متوقع أثناء إضافة دفعة المورد.");
        }
    }
}
