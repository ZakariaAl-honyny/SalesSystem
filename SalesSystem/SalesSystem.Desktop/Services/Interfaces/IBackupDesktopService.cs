using SalesSystem.Common;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IBackupDesktopService
{
    Task<Result<string>> CreateBackupAsync(CancellationToken ct = default);
    Task<Result<List<string>>> GetBackupListAsync(CancellationToken ct = default);
    Task<Result> RestoreBackupAsync(string fileName, CancellationToken ct = default);
}
