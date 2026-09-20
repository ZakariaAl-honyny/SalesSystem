using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class CompanySettingsApiService : ICompanySettingsApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/company-settings";

    public CompanySettingsApiService(HttpClientService http) => _http = http;

    public async Task<Result<CompanySettingsDto>> GetAsync(CancellationToken ct = default)
    {
        return await _http.GetAsync<CompanySettingsDto>(BasePath, ct);
    }

    public async Task<Result<CompanySettingsDto>> UpdateAsync(UpdateCompanySettingsRequest request, CancellationToken ct = default)
    {
        return await _http.PutAsync<CompanySettingsDto>(BasePath, request, ct);
    }
}
