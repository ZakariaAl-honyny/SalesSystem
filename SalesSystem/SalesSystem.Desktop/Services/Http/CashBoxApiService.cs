using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class CashBoxApiService : ICashBoxApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/cash-boxes";

    public CashBoxApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<CashBoxDto>>> GetAllAsync(CancellationToken ct = default)
        => await _http.GetListAsync<CashBoxDto>(BasePath, ct);

    public async Task<Result<CashBoxDto>> GetByIdAsync(int id, CancellationToken ct = default)
        => await _http.GetAsync<CashBoxDto>($"{BasePath}/{id}", ct);

    public async Task<Result<CashBoxDto>> CreateAsync(CreateCashBoxRequest request, CancellationToken ct = default)
        => await _http.PostAsync<CashBoxDto>(BasePath, request, ct);

    public async Task<Result<CashBoxDto>> UpdateAsync(int id, UpdateCashBoxRequest request, CancellationToken ct = default)
        => await _http.PutAsync<CashBoxDto>($"{BasePath}/{id}", request, ct);

    public async Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
        => await _http.DeleteAsync($"{BasePath}/{id}", ct);

    public async Task<Result> TransferAsync(CashTransferRequest request, CancellationToken ct = default)
        => await _http.PostAsync($"{BasePath}/transfer", request, ct);
}
