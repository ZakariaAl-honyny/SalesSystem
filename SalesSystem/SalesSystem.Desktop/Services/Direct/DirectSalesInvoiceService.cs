using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة فواتير البيع — يفوّض كل المنطق إلى ISalesService في طبقة الأعمال (بدون HTTP).
/// </summary>
public class DirectSalesInvoiceService : ISalesInvoiceApiService
{
    private readonly BizInterfaces.ISalesService _bll;
    private readonly ISessionService _session;

    public DirectSalesInvoiceService(BizInterfaces.ISalesService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<SalesInvoiceDto>>> GetAllAsync(
        string? search = null,
        DateTime? from = null,
        DateTime? to = null,
        byte? status = null,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(
                customerId: null,
                status: status,
                search: search,
                from: from,
                to: to,
                page: 1,
                pageSize: int.MaxValue,
                includeInactive: false,
                ct: ct);

            return result.IsSuccess
                ? Result<IReadOnlyList<SalesInvoiceDto>>.Success(result.Value!.Items.ToList())
                : Result<IReadOnlyList<SalesInvoiceDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة فواتير البيع");
            return Result<IReadOnlyList<SalesInvoiceDto>>.Failure("تعذر تحميل قائمة فواتير البيع");
        }
    }

    public async Task<Result<SalesInvoiceDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل فاتورة البيع {InvoiceId}", id);
            return Result<SalesInvoiceDto>.Failure("تعذر تحميل فاتورة البيع");
        }
    }

    public async Task<Result<SalesInvoiceDto>> CreateAsync(CreateSalesInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إنشاء فاتورة البيع");
            return Result<SalesInvoiceDto>.Failure("تعذر إنشاء فاتورة البيع");
        }
    }

    public async Task<Result<SalesInvoiceDto>> UpdateAsync(int id, CreateSalesInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            // Create → Update: الحقول غير الموجودة في UpdateSalesInvoiceRequest (InvoiceNo, BranchId) لا تُحدَّث.
            var request = new UpdateSalesInvoiceRequest(
                WarehouseId: r.WarehouseId,
                CustomerId: r.CustomerId,
                InvoiceDate: r.InvoiceDate,
                PaymentType: r.PaymentType,
                DiscountAmount: r.DiscountAmount,
                TaxAmount: r.TaxAmount,
                OtherCharges: r.OtherCharges,
                PaidAmount: r.PaidAmount,
                CashBoxId: r.CashBoxId,
                Notes: r.Notes,
                TaxId: r.TaxId,
                Items: r.Items,
                DiscountType: r.DiscountType,
                DiscountRate: r.DiscountRate);

            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحديث فاتورة البيع {InvoiceId}", id);
            return Result<SalesInvoiceDto>.Failure("تعذر تحديث فاتورة البيع");
        }
    }

    public async Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.PostAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل ترحيل فاتورة البيع {InvoiceId}", id);
            return Result.Failure("تعذر ترحيل فاتورة البيع");
        }
    }

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.CancelAsync(id, UserId, ct);
            return result.IsSuccess
                ? Result.Success()
                : Result.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إلغاء فاتورة البيع {InvoiceId}", id);
            return Result.Failure("تعذر إلغاء فاتورة البيع");
        }
    }
}
