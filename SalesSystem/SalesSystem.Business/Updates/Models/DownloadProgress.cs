namespace SalesSystem.Business.Updates.Models;

/// <summary>
/// Progress information for update download operations.
/// </summary>
public class DownloadProgress
{
    public DownloadProgress() { }

    public DownloadProgress(long bytesReceived, long totalBytes, double percentage, double speedKbps)
    {
        BytesReceived = bytesReceived;
        TotalBytes = totalBytes;
        Percentage = (int)percentage;
        SpeedKbps = speedKbps;
    }

    /// <summary>
    /// Total bytes to download.
    /// </summary>
    public long TotalBytes { get; set; }

    /// <summary>
    /// Bytes downloaded so far.
    /// </summary>
    public long BytesReceived { get; set; }

    /// <summary>
    /// Download progress as a percentage (0-100).
    /// </summary>
    public int Percentage { get; set; }

    /// <summary>
    /// Download speed in KB/s.
    /// </summary>
    public double SpeedKbps { get; set; }
}
