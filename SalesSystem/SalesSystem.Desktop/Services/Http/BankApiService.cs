using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class BankApiService : IBankApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/banks";

    public BankApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<BankDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["includeInactive"] = includeInactive.ToString().ToLower();
        return await _http.GetListAsync<BankDto>($"{BasePath}?{query}", ct);
    }

    public async Task<Result<BankDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetAsync<BankDto>($"{BasePath}/{id}", ct);

    public async Task<Result<BankDto>> CreateAsync(CreateBankRequest request, CancellationToken ct = default)
        => await _http.PostAsync<BankDto>(BasePath, request, ct);

    public async Task<Result<BankDto>> UpdateAsync(int id, UpdateBankRequest request, CancellationToken ct = default)
        => await _http.PutAsync<BankDto>($"{BasePath}/{id}", request, ct);

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
        => await _http.PutAsync<object>($"{BasePath}/{id}/deactivate", null, ct);
}
