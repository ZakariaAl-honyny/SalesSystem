namespace SalesSystem.DTO.Requests;

public record CreateRoleRequest(
    string Name,
    string? Description = null,
    long SalesPermissions = 0,
    long InventoryPermissions = 0,
    long AccountingPermissions = 0,
    long SystemPermissions = 0);

public record UpdateRoleRequest(
    string Name,
    string? Description = null,
    bool IsActive = true,
    long? SalesPermissions = null,
    long? InventoryPermissions = null,
    long? AccountingPermissions = null,
    long? SystemPermissions = null);

public record SetRolePermissionsRequest(
    long? SalesPermissions = null,
    long? InventoryPermissions = null,
    long? AccountingPermissions = null,
    long? SystemPermissions = null);
