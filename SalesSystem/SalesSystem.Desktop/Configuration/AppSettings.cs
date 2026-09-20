namespace SalesSystem.Desktop.Configuration;

public sealed class AppSettings
{
    public bool UseMockData { get; set; }
    public static AppSettings Current { get; set; } = new();
}
