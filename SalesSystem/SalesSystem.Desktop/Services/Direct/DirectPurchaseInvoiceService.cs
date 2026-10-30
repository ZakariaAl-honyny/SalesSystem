using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة فواتير الشراء — يفوّض كل المنطق إلى IPurchaseService في طبقة الأعمال.
/// </summary>
public class DirectPurchaseInvoiceService : IPurchaseInvoiceApiService
{
    private readonly BizInterfaces.IPurchaseService _bll;
    private readonly ISessionService _session;

    public DirectPurchaseInvoiceService(BizInterfaces.IPurchaseService bll, ISessionService session)
    {
        _bll = bll;
        _session = session;
    }

    private int UserId => _session.Current?.UserId ?? 0;

    public async Task<Result<IReadOnlyList<PurchaseInvoiceDto>>> GetAllAsync(
        string? search = null,
        DateTime? from = null,
        DateTime? to = null,
        byte? status = null,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAllAsync(
                supplierId: null,
                status: status,
                search: search,
                from: from,
                to: to,
                page: 1,
                pageSize: int.MaxValue,
                includeInactive: false,
                ct: ct);

            return result.IsSuccess
                ? Result<IReadOnlyList<PurchaseInvoiceDto>>.Success(result.Value!.Items.ToList())
                : Result<IReadOnlyList<PurchaseInvoiceDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل قائمة فواتير الشراء");
            return Result<IReadOnlyList<PurchaseInvoiceDto>>.Failure("تعذر تحميل قائمة فواتير الشراء");
        }
    }

    public async Task<Result<PurchaseInvoiceDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetByIdAsync(id, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل فاتورة الشراء {InvoiceId}", id);
            return Result<PurchaseInvoiceDto>.Failure("تعذر تحميل فاتورة الشراء");
        }
    }

    public async Task<Result<PurchaseInvoiceDto>> CreateAsync(CreatePurchaseInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            return await _bll.CreateAsync(r, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل إنشاء فاتورة الشراء");
            return Result<PurchaseInvoiceDto>.Failure("تعذر إنشاء فاتورة الشراء");
        }
    }

    public async Task<Result<PurchaseInvoiceDto>> UpdateAsync(int id, CreatePurchaseInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            // Create → Update: الحقل InvoiceNo غير موجود في UpdatePurchaseInvoiceRequest ولا يُحدَّث.
            var request = new UpdatePurchaseInvoiceRequest(
                WarehouseId: r.WarehouseId,
                SupplierId: r.SupplierId,
                InvoiceDate: r.InvoiceDate,
                PaymentType: r.PaymentType,
                DiscountAmount: r.DiscountAmount,
                DiscountType: r.DiscountType,
                DiscountRate: r.DiscountRate,
                TaxAmount: r.TaxAmount,
                OtherCharges: r.OtherCharges,
                PaidAmount: r.PaidAmount,
                Notes: r.Notes,
                TaxId: r.TaxId,
                AttachmentPath: r.AttachmentPath,
                Items: r.Items);

            return await _bll.UpdateAsync(id, request, UserId, ct);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحديث فاتورة الشراء {InvoiceId}", id);
            return Result<PurchaseInvoiceDto>.Failure("تعذر تحديث فاتورة الشراء");
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
            Serilog.Log.Error(ex, "فشل ترحيل فاتورة الشراء {InvoiceId}", id);
            return Result.Failure("تعذر ترحيل فاتورة الشراء");
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
            Serilog.Log.Error(ex, "فشل إلغاء فاتورة الشراء {InvoiceId}", id);
            return Result.Failure("تعذر إلغاء فاتورة الشراء");
        }
    }
}
