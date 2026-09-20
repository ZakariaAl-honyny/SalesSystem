using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services;

/// <summary>
/// Static session manager for the Desktop application.
/// Stores the current user's identity and permission bitmasks in memory.
/// Initialized after successful login — cleared on logout.
/// </summary>
public static class SessionManager
{
    private static LoginResponse? _loginResponse;

    /// <summary>
    /// Initialize the session with the response from a successful login.
    /// </summary>
    public static void Initialize(LoginResponse loginResponse)
    {
        _loginResponse = loginResponse;
    }

    /// <summary>
    /// Clear the current session (logout).
    /// </summary>
    public static void Clear()
    {
        _loginResponse = null;
    }

    // ── Identity Properties ──────────────────────────────────────────

    public static bool IsLoggedIn => _loginResponse is not null;
    public static int UserId => _loginResponse?.UserId ?? 0;
    public static string UserName => _loginResponse?.UserName ?? string.Empty;
    public static short RoleId => _loginResponse?.RoleId ?? 0;
    public static string? RoleName => _loginResponse?.RoleName;
    public static short? BranchId => _loginResponse?.BranchId;
    public static string Token => _loginResponse?.Token ?? string.Empty;
    public static DateTime ExpiresAt => _loginResponse?.ExpiresAt ?? DateTime.MinValue;
    public static bool MustChangePassword => _loginResponse?.MustChangePassword ?? false;
    public static long SalesPermissions => _loginResponse?.SalesPermissions ?? 0;
    public static long InventoryPermissions => _loginResponse?.InventoryPermissions ?? 0;
    public static long AccountingPermissions => _loginResponse?.AccountingPermissions ?? 0;
    public static long SystemPermissions => _loginResponse?.SystemPermissions ?? 0;

    // ── Permission Checks ────────────────────────────────────────────

    /// <summary>
    /// Returns true if the current user has ALL the given Sales permission bits set,
    /// or if the user is Super Admin (SalesPermissions == -1).
    /// </summary>
    public static bool HasSalesPermission(long requiredPermission)
    {
        if (_loginResponse is null) return false;
        return _loginResponse.SalesPermissions == -1
            || (_loginResponse.SalesPermissions & requiredPermission) == requiredPermission;
    }

    /// <summary>
    /// Returns true if the current user has ALL the given Inventory permission bits set,
    /// or if the user is Super Admin (InventoryPermissions == -1).
    /// </summary>
    public static bool HasInventoryPermission(long requiredPermission)
    {
        if (_loginResponse is null) return false;
        return _loginResponse.InventoryPermissions == -1
            || (_loginResponse.InventoryPermissions & requiredPermission) == requiredPermission;
    }

    /// <summary>
    /// Returns true if the current user has ALL the given Accounting permission bits set,
    /// or if the user is Super Admin (AccountingPermissions == -1).
    /// </summary>
    public static bool HasAccountingPermission(long requiredPermission)
    {
        if (_loginResponse is null) return false;
        return _loginResponse.AccountingPermissions == -1
            || (_loginResponse.AccountingPermissions & requiredPermission) == requiredPermission;
    }

    /// <summary>
    /// Returns true if the current user has ALL the given System permission bits set,
    /// or if the user is Super Admin (SystemPermissions == -1).
    /// </summary>
    public static bool HasSystemPermission(long requiredPermission)
    {
        if (_loginResponse is null) return false;
        return _loginResponse.SystemPermissions == -1
            || (_loginResponse.SystemPermissions & requiredPermission) == requiredPermission;
    }

    /// <summary>
    /// Returns true if the current user is a Super Admin
    /// (all SalesPermissions bits set to 1, i.e. value == -1).
    /// </summary>
    public static bool IsSuperAdmin => _loginResponse?.SalesPermissions == -1;

    /// <summary>
    /// Returns true if the token has expired.
    /// </summary>
    public static bool IsTokenExpired => DateTime.UtcNow >= ExpiresAt;
}
