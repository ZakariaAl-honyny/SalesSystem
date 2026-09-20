using System.Threading;
using System.Threading.Tasks;
using SalesSystem.Business.Updates.Models;
using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Updates;

/// <summary>
/// Service for checking, downloading, and installing application updates.
/// </summary>
public interface IUpdaterService
{
    /// <summary>
    /// Checks for available updates asynchronously.
    /// </summary>
    Task<Result<UpdateCheckResult>> CheckForUpdatesAsync(CancellationToken ct = default);

    /// <summary>
    /// Downloads the update package and returns the path to the downloaded file.
    /// </summary>
    Task<Result<string>> DownloadUpdateAsync(string downloadUrl, string expectedChecksum,
        IProgress<DownloadProgress> progress, CancellationToken ct = default);

    /// <summary>
    /// Launches the installer and signals the application to exit.
    /// </summary>
    Task<Result<bool>> LaunchInstallerAndExitAsync(string installerPath);

    /// <summary>
    /// Gets the current application version as a string.
    /// </summary>
    Result<string> GetCurrentVersion();

    /// <summary>
    /// Persists a skipped version so it is not offered again.
    /// </summary>
    Result SkipVersion(string version);

    /// <summary>
    /// Gets the currently skipped version (if any).
    /// </summary>
    Result<string> GetSkippedVersion();
}
