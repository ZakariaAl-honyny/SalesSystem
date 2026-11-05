using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Permission checking service. In the single-admin system there is no role
/// entity — the only account has full access across every permission module.
/// This interface is kept so the Desktop can still ask "does this user have X?"
/// and always receive an affirmative answer for the seeded admin.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Checks if a user has a specific permission. Always true for the admin.
    /// </summary>
    Task<bool> HasPermissionAsync(int userId, long requiredPermission, CancellationToken ct = default);

    /// <summary>
    /// Gets the combined permissions mask for a user. Returns -1 (all bits set)
    /// for the admin.
    /// </summary>
    Task<long> GetPermissionsMaskAsync(int userId, CancellationToken ct = default);

    /// <summary>
    /// Gets the list of permission code strings for a user (for Desktop API).
    /// Returns all known permission codes for the admin.
    /// </summary>
    Task<Result<List<string>>> GetUserPermissionsAsync(int userId, CancellationToken ct = default);

    /// <summary>
    /// Checks if a user has a specific permission by code name (e.g., "Sales.View").
    /// Always true for the admin.
    /// </summary>
    Task<bool> UserHasPermissionAsync(int userId, string permissionName, CancellationToken ct = default);
}
