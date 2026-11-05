using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services;

/// <summary>
/// Simple in-memory session for the single-admin desktop app.
/// Holds the current user's identity only — no roles, no permission bitmasks.
/// </summary>
public static class SessionManager
{
    private static LoginResponse? _session;

    public static void Initialize(LoginResponse loginResponse) => _session = loginResponse;

    public static void Clear() => _session = null;

    public static bool IsLoggedIn => _session is not null;

    public static int UserId => _session?.UserId ?? 0;

    public static string UserName => _session?.UserName ?? string.Empty;

    /// <summary>نسخة MVP: مستخدم واحد (مدير النظام).</summary>
    public static bool IsAdmin => _session?.IsAdmin ?? true;

    public static short? BranchId => _session?.BranchId;
}
