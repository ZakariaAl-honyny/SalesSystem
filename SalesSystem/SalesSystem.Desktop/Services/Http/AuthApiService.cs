using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class AuthApiService : IAuthApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/auth";

    public AuthApiService(HttpClientService http) => _http = http;

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync<LoginResponse>($"{BasePath}/login", request, ct);
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/change-password", request, ct);
    }
}

