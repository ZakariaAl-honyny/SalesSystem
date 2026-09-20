using SalesSystem.Common;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Http;

public sealed class BackupApiService : IBackupApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/backup";

    public BackupApiService(HttpClientService http) => _http = http;

    public async Task<Result<string>> CreateBackupAsync(CancellationToken ct = default)
    {
        return await _http.PostAsync<string>($"{BasePath}/create", null, ct);
    }

    public async Task<Result<List<string>>> GetBackupListAsync(CancellationToken ct = default)
    {
        var result = await _http.GetListAsync<string>($"{BasePath}/list", ct);
        if (result.IsSuccess && result.Value != null)
            return Result<List<string>>.Success(result.Value.ToList());
        return Result<List<string>>.Failure(result.Error ?? "فشل تحميل قائمة النسخ الاحتياطية");
    }

    public async Task<Result> RestoreBackupAsync(string fileName, CancellationToken ct = default)
    {
        return await _http.PostAsync($"{BasePath}/restore", new { fileName }, ct);
    }
}
