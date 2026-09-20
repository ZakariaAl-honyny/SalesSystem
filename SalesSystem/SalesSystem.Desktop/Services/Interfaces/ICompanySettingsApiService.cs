using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ICompanySettingsApiService
{
    Task<Result<CompanySettingsDto>> GetAsync(CancellationToken ct = default);
    Task<Result<CompanySettingsDto>> UpdateAsync(UpdateCompanySettingsRequest request, CancellationToken ct = default);
}
