using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class ProductPriceApiService : IProductPriceApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/product-prices";

    public ProductPriceApiService(HttpClientService http) => _http = http;

    public async Task<Result<ProductPriceDto>> GetEffectivePriceAsync(int productUnitId, DateTime? effectiveDate = null, CancellationToken ct = default)
    {
        var url = $"{BasePath}/effective?productUnitId={productUnitId}";
        if (effectiveDate.HasValue)
            url += $"&effectiveDate={effectiveDate.Value:yyyy-MM-dd}";
        return await _http.GetAsync<ProductPriceDto>(url, ct);
    }

    public async Task<Result<ProductPriceDto>> GetEffectivePriceForInvoiceAsync(int productUnitId, CancellationToken ct = default)
        => await _http.GetAsync<ProductPriceDto>($"{BasePath}/effective/invoice?productUnitId={productUnitId}", ct);

    public async Task<Result<IReadOnlyList<ProductPriceDto>>> GetByProductUnitAsync(int productUnitId, CancellationToken ct = default)
        => await _http.GetListAsync<ProductPriceDto>($"{BasePath}/by-unit/{productUnitId}", ct);

    public async Task<Result<ProductPriceDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetAsync<ProductPriceDto>($"{BasePath}/{id}", ct);

    public async Task<Result<ProductPriceDto>> CreateAsync(CreateProductPriceRequest request, CancellationToken ct = default)
        => await _http.PostAsync<ProductPriceDto>(BasePath, request, ct);

    public async Task<Result<ProductPriceDto>> UpdateAsync(int id, UpdateProductPriceRequest request, CancellationToken ct = default)
        => await _http.PutAsync<ProductPriceDto>($"{BasePath}/{id}", request, ct);

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
        => await _http.DeleteAsync($"{BasePath}/{id}", ct);
}
