using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة تقارير الشراء — يفوّض كل المنطق إلى IPurchaseReportService في طبقة الأعمال.
/// </summary>
public class DirectPurchaseReportService : IPurchaseReportApiService
{
    private readonly BizInterfaces.IPurchaseReportService _bll;

    public DirectPurchaseReportService(BizInterfaces.IPurchaseReportService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<PurchasesBySupplierDto>>> GetPurchasesBySupplierAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetPurchasesBySupplierAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<PurchasesBySupplierDto>>.Success(result.Value!)
                : Result<IReadOnlyList<PurchasesBySupplierDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل تقرير المشتريات حسب المورد");
            return Result<IReadOnlyList<PurchasesBySupplierDto>>.Failure("تعذر تحميل تقرير المشتريات حسب المورد");
        }
    }

    public async Task<Result<IReadOnlyList<PurchasesByProductDto>>> GetPurchasesByProductAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetPurchasesByProductAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<PurchasesByProductDto>>.Success(result.Value!)
                : Result<IReadOnlyList<PurchasesByProductDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل تقرير المشتريات حسب الصنف");
            return Result<IReadOnlyList<PurchasesByProductDto>>.Failure("تعذر تحميل تقرير المشتريات حسب الصنف");
        }
    }
}
