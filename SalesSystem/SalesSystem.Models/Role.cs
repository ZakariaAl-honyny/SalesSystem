using SalesSystem.Common;
using SalesSystem.Common.Exceptions;

namespace SalesSystem.Models;

/// <summary>
/// Represents a user role (e.g., Admin, Manager, Cashier).
/// Replaces the UserRole enum with a database-driven role entity.
/// Permissions are stored as bitmask fields grouped by category:
///   Sales, Inventory, Accounting, System.
/// A role with SalesPermissions = -1 (all bits set) is considered Super Admin.
///
/// 9 system roles seeded: Admin(1), Manager(2), Accountant(3), Treasurer(4),
/// Cashier(5), WarehouseSupervisor(6), SalesEmployee(7), Observer(8), BranchManager(9).
/// </summary>
public class Role : ActivatableEntity
{
    /// <summary>
    /// Schema: smallint PK (short).
    /// </summary>
    public new short Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    /// <summary>
    /// Indicates whether this is a system-protected role.
    /// System roles (Id 1–9) cannot be deleted.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Bitmask of sales-related permissions.
    /// -1 = all permissions (super admin for this category).
    /// </summary>
    public long SalesPermissions { get; private set; }

    /// <summary>
    /// Bitmask of inventory-related permissions.
    /// </summary>
    public long InventoryPermissions { get; private set; }

    /// <summary>
    /// Bitmask of accounting-related permissions.
    /// </summary>
    public long AccountingPermissions { get; private set; }

    /// <summary>
    /// Bitmask of system-related permissions.
    /// </summary>
    public long SystemPermissions { get; private set; }

    /// <summary>
    /// Returns true if this role has all sales permissions (super admin).
    /// </summary>
    public bool IsSuperAdmin => SalesPermissions == -1;

    protected Role() { } // EF Core

    /// <summary>
    /// Creates a new role with category-based permission bitmasks.
    /// </summary>
    public static Role Create(string name, string? description = null,
        long salesPermissions = 0, long inventoryPermissions = 0,
        long accountingPermissions = 0, long systemPermissions = 0,
        bool isSystem = false, int? createdByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم الدور مطلوب.");

        var role = new Role
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            SalesPermissions = salesPermissions,
            InventoryPermissions = inventoryPermissions,
            AccountingPermissions = accountingPermissions,
            SystemPermissions = systemPermissions,
            IsSystem = isSystem
        };
        role.SetCreatedBy(createdByUserId);
        return role;
    }

    /// <summary>
    /// Updates the role's name, description, and permission bitmasks.
    /// Only the provided permission categories are updated (nullable).
    /// </summary>
    public void Update(string name, string? description = null,
        long? salesPermissions = null, long? inventoryPermissions = null,
        long? accountingPermissions = null, long? systemPermissions = null,
        int? updatedByUserId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("اسم الدور مطلوب.");
        Name = name.Trim();
        Description = description?.Trim();
        if (salesPermissions.HasValue) SalesPermissions = salesPermissions.Value;
        if (inventoryPermissions.HasValue) InventoryPermissions = inventoryPermissions.Value;
        if (accountingPermissions.HasValue) AccountingPermissions = accountingPermissions.Value;
        if (systemPermissions.HasValue) SystemPermissions = systemPermissions.Value;
        SetUpdatedBy(updatedByUserId);
        UpdateTimestamp();
    }

    /// <summary>
    /// Sets the sales permissions bitmask.
    /// Use -1 to grant all permissions (super admin for this category).
    /// </summary>
    public void SetSalesPermissions(long mask)
    {
        SalesPermissions = mask;
        UpdateTimestamp();
    }

    /// <summary>
    /// Sets the inventory permissions bitmask.
    /// </summary>
    public void SetInventoryPermissions(long mask)
    {
        InventoryPermissions = mask;
        UpdateTimestamp();
    }

    /// <summary>
    /// Sets the accounting permissions bitmask.
    /// </summary>
    public void SetAccountingPermissions(long mask)
    {
        AccountingPermissions = mask;
        UpdateTimestamp();
    }

    /// <summary>
    /// Sets the system permissions bitmask.
    /// </summary>
    public void SetSystemPermissions(long mask)
    {
        SystemPermissions = mask;
        UpdateTimestamp();
    }

    /// <summary>
    /// Prevents deletion of system roles.
    /// </summary>
    public override void MarkAsDeleted()
    {
        if (IsSystem)
            throw new DomainException("لا يمكن حذف دور نظام — الدور محمي.");
        base.MarkAsDeleted();
    }
}
