using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class TaxesApiService : ITaxesApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/taxes";

    public TaxesApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<TaxDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var path = $"{BasePath}?includeInactive={includeInactive.ToString().ToLower()}";
        return await _http.GetListAsync<TaxDto>(path, ct);
    }

    public async Task<Result<TaxDto>> CreateAsync(CreateTaxRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<TaxDto>(BasePath, request, ct);
    }

    public async Task<Result<TaxDto>> UpdateAsync(int id, UpdateTaxRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync<TaxDto>($"{BasePath}/{id}", request, ct);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/{id}", ct);
    }

    public async Task<Result> DeletePermanentlyAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/permanent/{id}", ct);
    }
}
