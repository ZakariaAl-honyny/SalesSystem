using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class CustomerReceiptApiService : ICustomerReceiptApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/customer-receipts";

    public CustomerReceiptApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<CustomerReceiptDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
        => await _http.GetListAsync<CustomerReceiptDto>($"{BasePath}?includeInactive={includeInactive.ToString().ToLower()}", ct);

    public async Task<Result<CustomerReceiptDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetAsync<CustomerReceiptDto>($"{BasePath}/{id}", ct);

    public async Task<Result<CustomerReceiptDto>> CreateAsync(CreateCustomerReceiptRequest request, CancellationToken ct = default)
        => await _http.PostAsync<CustomerReceiptDto>(BasePath, request, ct);

    public async Task<Result<CustomerReceiptDto>> UpdateAsync(int id, UpdateCustomerReceiptRequest request, CancellationToken ct = default)
        => await _http.PutAsync<CustomerReceiptDto>($"{BasePath}/{id}", request, ct);

    public async Task<Result<CustomerReceiptDto>> PostAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync<CustomerReceiptDto>($"{BasePath}/{id}/post", null, ct);

    public async Task<Result> CancelAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync($"{BasePath}/{id}/cancel", null, ct);
}
