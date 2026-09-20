using System;

namespace SalesSystem.Business.Updates.Models;

/// <summary>
/// Represents version information from a version.json update check endpoint.
/// </summary>
public class UpdateInfo
{
    /// <summary>
    /// Latest available version string.
    /// </summary>
    public string? LatestVersion { get; set; }

    /// <summary>
    /// Minimum required version (for force updates).
    /// </summary>
    public string? MinimumVersion { get; set; }

    /// <summary>
    /// URL to download the update package.
    /// </summary>
    public string? DownloadUrl { get; set; }

    /// <summary>
    /// SHA256 checksum of the update package.
    /// </summary>
    public string? ExpectedChecksum { get; set; }

    /// <summary>
    /// Release notes for this version.
    /// </summary>
    public string? ReleaseNotes { get; set; }

    /// <summary>
    /// Whether the update is mandatory (force update).
    /// </summary>
    public bool IsForceUpdate { get; set; }

    /// <summary>
    /// Release date of the update as a string (used by GitHub updater).
    /// </summary>
    public string? ReleaseDate { get; set; }

    /// <summary>
    /// SHA256 checksum property alias used by GitHub updater.
    /// </summary>
    public string? ChecksumSHA256 { get => ExpectedChecksum; set => ExpectedChecksum = value; }

    /// <summary>
    /// Minimum required version alias used by GitHub updater.
    /// </summary>
    public string? MinimumRequiredVersion { get => MinimumVersion; set => MinimumVersion = value; }

    /// <summary>
    /// Whether the update is critical (used by GitHub updater).
    /// </summary>
    public bool IsCritical { get; set; }

    /// <summary>
    /// Changelog as a list of strings (used by GitHub updater).
    /// </summary>
    public List<string>? Changelog { get; set; }

    /// <summary>
    /// Determines if an update is available compared to the current version.
    /// </summary>
    public bool IsUpdateAvailable(string currentVersion)
    {
        if (string.IsNullOrWhiteSpace(LatestVersion))
            return false;

        if (Version.TryParse(currentVersion, out var current) &&
            Version.TryParse(LatestVersion, out var latest))
        {
            return latest > current;
        }

        return string.Compare(LatestVersion, currentVersion, StringComparison.OrdinalIgnoreCase) > 0;
    }

    /// <summary>
    /// Determines if the specified current version is below the minimum required version.
    /// </summary>
    public bool IsBelowMinimumVersion(string? currentVersion)
    {
        if (string.IsNullOrWhiteSpace(MinimumVersion) || string.IsNullOrWhiteSpace(currentVersion))
            return IsForceUpdate;

        if (Version.TryParse(currentVersion, out var current) &&
            Version.TryParse(MinimumVersion, out var minimum))
        {
            return current < minimum;
        }

        return IsForceUpdate;
    }
}
