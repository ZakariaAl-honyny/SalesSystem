using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ISalesReportApiService
{
    Task<Result<IReadOnlyList<SalesByCustomerDto>>> GetSalesByCustomerAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<SalesByProductDto>>> GetSalesByProductAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<SalesByCategoryDto>>> GetSalesByCategoryAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<DailySalesSummaryDto>>> GetDailySalesSummaryAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ProductProfitabilityDto>>> GetProductProfitabilityAsync(int? productId = null, int? categoryId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ProfitByCustomerDto>>> GetProfitByCustomerAsync(int? customerId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
}
