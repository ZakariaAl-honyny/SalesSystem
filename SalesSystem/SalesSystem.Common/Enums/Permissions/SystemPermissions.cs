namespace SalesSystem.Common.Enums.Permissions;

/// <summary>
/// System administration permission flags for bitmask authorization.
/// Each bit represents a discrete permission. All = -1 grants full access (Super Admin).
/// </summary>
[Flags]
public enum SystemPermissions : long
{
    None = 0,
    ManageUsers = 1L << 0,
    ManageRoles = 1L << 1,
    ManageSettings = 1L << 2,
    ViewAuditLogs = 1L << 3,
    ManageBranches = 1L << 4,
    ManageBackups = 1L << 5,
    ManageFiscalYears = 1L << 6,
    ManageTaxes = 1L << 7,
    All = -1
}
