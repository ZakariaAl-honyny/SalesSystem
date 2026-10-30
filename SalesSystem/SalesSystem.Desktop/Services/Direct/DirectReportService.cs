using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لتقارير المبيعات والمخزون — يستدعي خدمات طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectReportService : IReportApiService
{
    private readonly BizInterfaces.IReportService _reports;

    public DirectReportService(BizInterfaces.IReportService reports)
    {
        _reports = reports;
    }

    public async Task<Result<IReadOnlyList<SalesReportDto>>> GetSalesAsync(
        DateTime? from = null, DateTime? to = null, int? customerId = null, CancellationToken ct = default)
    {
        try
        {
            // BLL requires non-null dates and supports warehouse filtering only.
            // customerId has no equivalent in the BLL report (filtered client-side).
            var r = await _reports.GetSalesReportAsync(null, from ?? DateTime.MinValue, to ?? DateTime.MaxValue, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<SalesReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<SalesReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<SalesReportDto>>.Failure("حدث خطأ أثناء جلب تقرير المبيعات");
        }
    }

    public async Task<Result<IReadOnlyList<PurchaseReportDto>>> GetPurchasesAsync(
        DateTime? from = null, DateTime? to = null, int? supplierId = null, CancellationToken ct = default)
    {
        try
        {
            // BLL requires non-null dates; supplierId has no equivalent in the BLL report.
            var r = await _reports.GetPurchasesReportAsync(null, from ?? DateTime.MinValue, to ?? DateTime.MaxValue, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<PurchaseReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<PurchaseReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<PurchaseReportDto>>.Failure("حدث خطأ أثناء جلب تقرير المشتريات");
        }
    }

    public async Task<Result<IReadOnlyList<StockReportDto>>> GetStockReportAsync(
        int? warehouseId = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetStockReportAsync(warehouseId, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<StockReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<StockReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<StockReportDto>>.Failure("حدث خطأ أثناء جلب تقرير المخزون");
        }
    }

    public async Task<Result<IReadOnlyList<CustomerBalanceReportDto>>> GetCustomerBalancesAsync(
        int? customerId = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetCustomerBalancesReportAsync(customerId, ct);
            if (!r.IsSuccess)
                return Result<IReadOnlyList<CustomerBalanceReportDto>>.Failure(r.Error!, r.ErrorCode);

            var items = r.Value!
                .Select(x => new CustomerBalanceReportDto(
                    Id: x.CustomerId,
                    Name: x.CustomerName,
                    Phone: null,              // TODO: لا يوجد حقل هاتف في CustomerFinancialBalanceDto
                    GroupName: null,          // TODO: لا يوجد حقل مجموعة في CustomerFinancialBalanceDto
                    CurrentBalance: x.CurrentBalance,
                    CreditLimit: 0m,          // TODO: لا يوجد حد ائتمان في CustomerFinancialBalanceDto
                    BalanceStatus: x.CurrentBalance > 0m ? "مدين" : x.CurrentBalance < 0m ? "دائن" : "متوازن"))
                .ToList();

            return Result<IReadOnlyList<CustomerBalanceReportDto>>.Success(items);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<CustomerBalanceReportDto>>.Failure("حدث خطأ أثناء جلب أرصدة العملاء");
        }
    }

    public async Task<Result<IReadOnlyList<SupplierBalanceReportDto>>> GetSupplierBalancesAsync(
        int? supplierId = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetSupplierBalancesReportAsync(supplierId, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<SupplierBalanceReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<SupplierBalanceReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<SupplierBalanceReportDto>>.Failure("حدث خطأ أثناء جلب أرصدة الموردين");
        }
    }

    public async Task<Result<IReadOnlyList<ProductMovementReportDto>>> GetMovementsAsync(
        int? productId = null, int? warehouseId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        try
        {
            // BLL signature takes a non-nullable productId (0 = no product filter).
            var r = await _reports.GetProductMovementsReportAsync(productId ?? 0, from, to, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<ProductMovementReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<ProductMovementReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<ProductMovementReportDto>>.Failure("حدث خطأ أثناء جلب تقرير حركة المنتجات");
        }
    }

    public async Task<Result<IReadOnlyList<LowStockReportDto>>> GetLowStockAsync(CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetLowStockReportAsync(null, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<LowStockReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<LowStockReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<LowStockReportDto>>.Failure("حدث خطأ أثناء جلب تقرير المخزون المنخفض");
        }
    }

    public async Task<Result<IReadOnlyList<ExpiredProductDto>>> GetExpiredProductsAsync(
        int thresholdDays = 0, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetExpiredProductsReportAsync(thresholdDays, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<ExpiredProductDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<ExpiredProductDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<ExpiredProductDto>>.Failure("حدث خطأ أثناء جلب تقرير المنتجات منتهية الصلاحية");
        }
    }

    public async Task<Result<IReadOnlyList<StockBalanceReportDto>>> GetStockBalanceAsync(
        int? warehouseId = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetStockBalanceReportAsync(warehouseId, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<StockBalanceReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<StockBalanceReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<StockBalanceReportDto>>.Failure("حدث خطأ أثناء جلب تقرير أرصدة المخزون");
        }
    }

    public async Task<Result<IReadOnlyList<WarehouseMovementReportDto>>> GetWarehouseMovementsAsync(
        int? warehouseId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetWarehouseMovementsAsync(warehouseId, from, to, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<WarehouseMovementReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<WarehouseMovementReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<WarehouseMovementReportDto>>.Failure("حدث خطأ أثناء جلب تقرير حركة المستودعات");
        }
    }

    public async Task<Result<IReadOnlyList<DetailedStockLedgerDto>>> GetDetailedStockLedgerAsync(
        int? productId = null, int? warehouseId = null, DateTime? from = null, DateTime? to = null,
        CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetDetailedStockLedgerAsync(productId, warehouseId, from, to, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<DetailedStockLedgerDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<DetailedStockLedgerDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<DetailedStockLedgerDto>>.Failure("حدث خطأ أثناء جلب دفتر حركة المخزون التفصيلي");
        }
    }

    public async Task<Result<IReadOnlyList<ReturnsReportDto>>> GetReturnsReportAsync(
        string? returnType = null, DateTime? from = null, DateTime? to = null, int? productId = null,
        CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetReturnsReportAsync(returnType, from, to, productId, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<ReturnsReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<ReturnsReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<ReturnsReportDto>>.Failure("حدث خطأ أثناء جلب تقرير المرتجعات");
        }
    }

    public async Task<Result<IReadOnlyList<AgingReportDto>>> GetAgingReportAsync(
        string partyType = "Customers", int? partyId = null, CancellationToken ct = default)
    {
        try
        {
            var r = await _reports.GetAgingReportAsync(partyType, partyId, ct);
            return r.IsSuccess
                ? Result<IReadOnlyList<AgingReportDto>>.Success(r.Value!.ToList())
                : Result<IReadOnlyList<AgingReportDto>>.Failure(r.Error!, r.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<AgingReportDto>>.Failure("حدث خطأ أثناء جلب تقرير الأعمار");
        }
    }
}
