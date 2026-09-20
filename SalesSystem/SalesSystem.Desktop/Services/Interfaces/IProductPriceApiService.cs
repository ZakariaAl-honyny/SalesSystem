using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IProductPriceApiService
{
    Task<Result<ProductPriceDto>> GetEffectivePriceAsync(int productUnitId, DateTime? effectiveDate = null, CancellationToken ct = default);
    Task<Result<ProductPriceDto>> GetEffectivePriceForInvoiceAsync(int productUnitId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ProductPriceDto>>> GetByProductUnitAsync(int productUnitId, CancellationToken ct = default);
    Task<Result<ProductPriceDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<ProductPriceDto>> CreateAsync(CreateProductPriceRequest request, CancellationToken ct = default);
    Task<Result<ProductPriceDto>> UpdateAsync(int id, UpdateProductPriceRequest request, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
}
