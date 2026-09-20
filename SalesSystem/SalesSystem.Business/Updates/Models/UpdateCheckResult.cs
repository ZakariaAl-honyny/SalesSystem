namespace SalesSystem.Business.Updates.Models;

/// <summary>
/// Result of checking for available updates.
/// </summary>
public class UpdateCheckResult
{
    /// <summary>
    /// Whether a new version is available.
    /// </summary>
    public bool HasUpdate { get; set; }

    /// <summary>
    /// The latest available version string.
    /// </summary>
    public string LatestVersion { get; set; } = string.Empty;

    /// <summary>
    /// URL to download the update package.
    /// </summary>
    public string? DownloadUrl { get; set; }

    /// <summary>
    /// SHA256 checksum of the update package for verification.
    /// </summary>
    public string? ExpectedChecksum { get; set; }

    /// <summary>
    /// Release notes or description of the update.
    /// </summary>
    public string? ReleaseNotes { get; set; }

    /// <summary>
    /// Whether the update is mandatory.
    /// </summary>
    public bool IsMandatory { get; set; }

    /// <summary>
    /// Creates a "no update available" result.
    /// </summary>
    public static UpdateCheckResult NoUpdate() => new() { HasUpdate = false };

    /// <summary>
    /// Creates an "update available" result from the given update info.
    /// </summary>
    public static UpdateCheckResult Available(UpdateInfo info) => new()
    {
        HasUpdate = true,
        LatestVersion = info.LatestVersion ?? string.Empty,
        DownloadUrl = info.DownloadUrl,
        ExpectedChecksum = info.ExpectedChecksum,
        ReleaseNotes = info.Changelog != null
            ? string.Join("\n", info.Changelog)
            : info.ReleaseNotes ?? string.Empty,
        IsMandatory = info.IsBelowMinimumVersion(System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "0.0.0")
    };
}
