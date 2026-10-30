using SalesSystem.Common;
using SalesSystem.Common.Enums;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IProductApiService
{
    Task<Result<IReadOnlyList<ProductDto>>> GetAllAsync(string? search = null, int? categoryId = null, bool includeInactive = false, CancellationToken ct = default);
    Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<ProductDto>> GetByBarcodeAsync(string barcode, CancellationToken ct = default);
    Task<Result<ProductDto>> CreateAsync(CreateProductRequest r, CancellationToken ct = default);
    Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductRequest r, CancellationToken ct = default);
    Task<Result> DeactivateAsync(int id, CancellationToken ct = default);
    Task<Result> ReactivateAsync(int id, CancellationToken ct = default);

    // ── Product Units ────────────────────────────────────────────────────────
    Task<Result<IReadOnlyList<ProductUnitDto>>> GetProductUnitsAsync(int productId, CancellationToken ct = default);
    Task<Result<ProductUnitDto>> AddProductUnitAsync(int productId, AddProductUnitRequest r, CancellationToken ct = default);
    Task<Result<ProductUnitDto>> UpdateProductUnitAsync(int productId, int productUnitId, UpdateProductUnitRequest r, CancellationToken ct = default);
    Task<Result> DeleteProductUnitAsync(int productId, int productUnitId, DeleteStrategy strategy = DeleteStrategy.Permanent, CancellationToken ct = default);

    // ── Product Prices ───────────────────────────────────────────────────────
    Task<Result<IReadOnlyList<ProductPriceDto>>> GetProductPricesByUnitAsync(int productUnitId, CancellationToken ct = default);
    Task<Result<ProductPriceDto>> CreateProductPriceAsync(CreateProductPriceRequest r, CancellationToken ct = default);
    Task<Result> DeleteProductPriceAsync(int priceId, CancellationToken ct = default);
}

