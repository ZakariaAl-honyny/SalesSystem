using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class UserApiService : IUserApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/users";

    public UserApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<UserDto>>> GetAllAsync(CancellationToken ct = default)
    {
        return await _http.GetListAsync<UserDto>(BasePath, ct);
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetAsync<UserDto>($"{BasePath}/{id}", ct);
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<UserDto>(BasePath, request, ct);
    }

    public async Task<Result<UserDto>> UpdateAsync(int id, UpdateUserRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync<UserDto>($"{BasePath}/{id}", request, ct);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        return await _http.DeleteAsync($"{BasePath}/{id}", ct);
    }

    public async Task<Result<ResetPasswordResponse>> ResetPasswordAsync(int id, CancellationToken ct = default)
    {
        return await _http.PostAsync<ResetPasswordResponse>($"{BasePath}/{id}/reset-password", null, ct);
    }
}

