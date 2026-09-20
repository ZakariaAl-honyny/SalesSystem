using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class AccountCategoryApiService : IAccountCategoryApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/account-categories";

    public AccountCategoryApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<AccountCategoryDto>>> GetAllAsync(CancellationToken ct = default)
    {
        return await _http.GetListAsync<AccountCategoryDto>(BasePath, ct);
    }

    public async Task<Result<AccountCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<AccountCategoryDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<AccountCategoryDto>> CreateAsync(CreateAccountCategoryRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<AccountCategoryDto>(BasePath, request, ct);
    }

    public async Task<Result<AccountCategoryDto>> UpdateAsync(int id, UpdateAccountCategoryRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync<AccountCategoryDto>($"{BasePath}/{id}", request, ct);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/{id}", ct);
    }
}
