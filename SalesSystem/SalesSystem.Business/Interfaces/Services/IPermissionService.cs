using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Service for Role-based permission checking using 4-category bitmasks.
/// Permissions are stored across 4 fields on the Role entity:
///   SalesPermissions, InventoryPermissions, AccountingPermissions, SystemPermissions.
/// User permissions come from their assigned Role (User.RoleId -> Role).
/// A field value of -1 = all permissions for that category (super admin for module).
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Checks if a user has a specific permission using bitwise AND.
    /// Routes the permission bit to the correct Role field based on the permission code.
    /// </summary>
    Task<bool> HasPermissionAsync(int userId, long requiredPermission, CancellationToken ct = default);

    /// <summary>
    /// Gets the combined permissions mask for a user (all 4 Role fields OR'd together).
    /// Returns 0 if the user does not exist.
    /// </summary>
    Task<long> GetPermissionsMaskAsync(int userId, CancellationToken ct = default);

    /// <summary>
    /// Sets per-module permission masks on a role. Only provided categories are updated.
    /// </summary>
    Task<Result> SetRolePermissionsAsync(short roleId,
        long? salesPermissions = null, long? inventoryPermissions = null,
        long? accountingPermissions = null, long? systemPermissions = null,
        CancellationToken ct = default);

    /// <summary>
    /// Gets all roles with their per-module permission masks.
    /// </summary>
    Task<Result<List<RoleDto>>> GetAllRoleMasksAsync(CancellationToken ct = default);

    /// <summary>
    /// Assigns a role to a user by setting User.RoleId (single-role model).
    /// </summary>
    Task<Result> AssignRoleToUserAsync(int userId, short roleId, CancellationToken ct = default);

    /// <summary>
    /// Gets the list of permission code strings for a user (for Desktop API).
    /// Checks the user's Role across all 4 permission fields.
    /// </summary>
    Task<Result<List<string>>> GetUserPermissionsAsync(int userId, CancellationToken ct = default);

    /// <summary>
    /// Checks if a user has a specific permission by code name (e.g., "Sales.View").
    /// Routes to the correct Role field based on the permission code.
    /// </summary>
    Task<bool> UserHasPermissionAsync(int userId, string permissionName, CancellationToken ct = default);
}
