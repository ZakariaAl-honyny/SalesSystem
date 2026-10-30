using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// تنفيذ مباشر لخدمة تقارير البيع — يفوّض كل المنطق إلى ISalesReportService في طبقة الأعمال.
/// </summary>
public class DirectSalesReportService : ISalesReportApiService
{
    private readonly BizInterfaces.ISalesReportService _bll;

    public DirectSalesReportService(BizInterfaces.ISalesReportService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<SalesByCustomerDto>>> GetSalesByCustomerAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetSalesByCustomerAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<SalesByCustomerDto>>.Success(result.Value!)
                : Result<IReadOnlyList<SalesByCustomerDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل تقرير المبيعات حسب العميل");
            return Result<IReadOnlyList<SalesByCustomerDto>>.Failure("تعذر تحميل تقرير المبيعات حسب العميل");
        }
    }

    public async Task<Result<IReadOnlyList<SalesByProductDto>>> GetSalesByProductAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetSalesByProductAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<SalesByProductDto>>.Success(result.Value!)
                : Result<IReadOnlyList<SalesByProductDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل تقرير المبيعات حسب الصنف");
            return Result<IReadOnlyList<SalesByProductDto>>.Failure("تعذر تحميل تقرير المبيعات حسب الصنف");
        }
    }

    public async Task<Result<IReadOnlyList<SalesByCategoryDto>>> GetSalesByCategoryAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetSalesByCategoryAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<SalesByCategoryDto>>.Success(result.Value!)
                : Result<IReadOnlyList<SalesByCategoryDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل تقرير المبيعات حسب المجموعة");
            return Result<IReadOnlyList<SalesByCategoryDto>>.Failure("تعذر تحميل تقرير المبيعات حسب المجموعة");
        }
    }

    public async Task<Result<IReadOnlyList<DailySalesSummaryDto>>> GetDailySalesSummaryAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetDailySalesSummaryAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<DailySalesSummaryDto>>.Success(result.Value!)
                : Result<IReadOnlyList<DailySalesSummaryDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل تحميل الملخص اليومي للمبيعات");
            return Result<IReadOnlyList<DailySalesSummaryDto>>.Failure("تعذر تحميل الملخص اليومي للمبيعات");
        }
    }

    public Task<Result<IReadOnlyList<ProductProfitabilityDto>>> GetProductProfitabilityAsync(
        int? productId = null, int? categoryId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        // TODO: تقرير ربحية الأصناف غير مُنفّذ في طبقة الأعمال (ISalesReportService) بعد.
        return Task.FromResult(Result<IReadOnlyList<ProductProfitabilityDto>>
            .Failure("هذا التقرير غير متاح في الإصدار الحالي"));
    }

    public Task<Result<IReadOnlyList<ProfitByCustomerDto>>> GetProfitByCustomerAsync(
        int? customerId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        // TODO: تقرير الربحية حسب العميل غير مُنفّذ في طبقة الأعمال (ISalesReportService) بعد.
        return Task.FromResult(Result<IReadOnlyList<ProfitByCustomerDto>>
            .Failure("هذا التقرير غير متاح في الإصدار الحالي"));
    }
}
