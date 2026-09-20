using SalesSystem.Common.Enums;
using SalesSystem.Common.Enums.Permissions;

namespace SalesSystem.Common.Helpers;

/// <summary>
/// Helper methods for working with bitmask permission flags.
/// Supports the 4 permission categories: Sales, Inventory, Accounting, System.
/// </summary>
public static class PermissionHelper
{
    /// <summary>
    /// Checks whether a user's permissions mask grants a specific required permission.
    /// Super Admin (-1) automatically passes all checks.
    /// </summary>
    public static bool HasPermission(long userPermissions, long requiredPermission)
        => userPermissions == -1 || (userPermissions & requiredPermission) == requiredPermission;

    /// <summary>
    /// Returns true if the permissions mask represents a Super Admin (all bits set).
    /// </summary>
    public static bool IsSuperAdmin(long permissionsMask)
        => permissionsMask == -1;

    /// <summary>
    /// Combines multiple permission flags into a single bitmask via OR operation.
    /// </summary>
    public static long CombinePermissions(params long[] permissions)
    {
        long result = 0;
        foreach (var p in permissions)
            result |= p;
        return result;
    }

    /// <summary>
    /// Combines Sales, Inventory, Accounting, and System permissions into one unified mask.
    /// </summary>
    public static long BuildUnifiedMask(
        SalesPermissions sales = SalesPermissions.None,
        InventoryPermissions inventory = InventoryPermissions.None,
        AccountingPermissions accounting = AccountingPermissions.None,
        SystemPermissions system = SystemPermissions.None)
    {
        return (long)sales | (long)inventory | (long)accounting | (long)system;
    }
}
