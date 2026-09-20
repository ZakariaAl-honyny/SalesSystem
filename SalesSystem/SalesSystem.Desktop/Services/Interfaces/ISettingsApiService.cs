using SalesSystem.Common;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ISettingsApiService
{
    Task<Result<dynamic>> GetSettingsAsync(CancellationToken ct = default);
    Task<Result> UpdateSettingsAsync(object settings, CancellationToken ct = default);
    Task<Result<Dictionary<string, string>>> GetAllSystemSettingsAsync(CancellationToken ct = default);
    Task<Result> UpdateSystemSettingsAsync(Dictionary<string, string> settings, CancellationToken ct = default);
    void RefreshCache();
}
