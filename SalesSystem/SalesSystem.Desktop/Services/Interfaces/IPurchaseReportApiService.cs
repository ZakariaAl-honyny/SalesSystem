using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IPurchaseReportApiService
{
    Task<Result<IReadOnlyList<PurchasesBySupplierDto>>> GetPurchasesBySupplierAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<PurchasesByProductDto>>> GetPurchasesByProductAsync(DateTime from, DateTime to, CancellationToken ct = default);
}
