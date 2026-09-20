using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class AccountApiService : IAccountApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/accounts";

    public AccountApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<AccountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        var path = $"{BasePath}?includeInactive={includeInactive.ToString().ToLower()}";
        return await _http.GetListAsync<AccountDto>(path, ct);
    }

    public async Task<Result<AccountDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<AccountDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<IReadOnlyList<AccountTreeNodeDto>>> GetTreeAsync(CancellationToken ct = default)
    {
        return await _http.GetListAsync<AccountTreeNodeDto>($"{BasePath}/tree", ct);
    }

    public async Task<Result<int>> CreateAsync(CreateAccountRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<int>(BasePath, request, ct);
    }

    public async Task<Result> UpdateAsync(int id, UpdateAccountRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync($"{BasePath}/{id}", request, ct);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/{id}", ct);
    }

    public async Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/permanent/{id}", ct);
    }
}
