using DesktopSession = SalesSystem.Desktop.Services.Interfaces.ISessionService;

namespace SalesSystem.Desktop.Services;

/// <summary>
/// TASK-007 — supplies the acting user's identity to the DataAccess audit interceptor
/// straight from the desktop session (username text, never a read of the Users table).
/// </summary>
public sealed class DesktopCurrentUserProvider : SalesSystem.Common.ICurrentUserProvider
{
    private readonly DesktopSession _session;

    public DesktopCurrentUserProvider(DesktopSession session)
    {
        _session = session;
    }

    public int? UserId => _session.Current?.UserId;

    public string? UserName => string.IsNullOrWhiteSpace(_session.Current?.UserName)
        ? null
        : _session.Current!.UserName.Trim();
}
