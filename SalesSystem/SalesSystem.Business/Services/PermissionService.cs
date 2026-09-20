using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Models;

namespace SalesSystem.Business.Services;

/// <summary>
/// Permission checking service using Role-based 4-category bitmasks.
/// Permissions are stored across 4 fields on the Role entity:
///   - SalesPermissions: Sales.View, Sales.Create, Sales.Edit, Sales.Delete, Sales.Cancel, Sales.Return, Sales.Print
///   - InventoryPermissions: Purchases.*, Inventory.*, Warehouse.*, Products.*, Suppliers.*
///   - AccountingPermissions: Reports.View, Accounting.*, Operations.*
///   - SystemPermissions: System.*, Roles.*, Audit.*, Backup.*, FiscalYear.*, Currencies.*, Customers.*
/// User permissions are determined by the user's assigned Role (User.RoleId -> Role).
/// A field value of -1 = all permissions for that category (super admin for that module).
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(IUnitOfWork uow, ILogger<PermissionService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    // ═════════════════════════════════════════════════════════════════
    //  Permission code → bit value mapping
    //  These must stay in sync with the codes seeded in DbSeeder.
    // ═════════════════════════════════════════════════════════════════

    private static readonly Dictionary<string, long> PermissionCodeToBit = new()
    {
        // ── Sales (bits 0-6) ──────────────────────────────────
        ["Sales.View"]      = 1L << 0,   // 1
        ["Sales.Create"]    = 1L << 1,   // 2
        ["Sales.Edit"]      = 1L << 2,   // 4
        ["Sales.Delete"]    = 1L << 3,   // 8
        ["Sales.Cancel"]    = 1L << 4,   // 16
        ["Sales.Return"]    = 1L << 5,   // 32
        ["Sales.Print"]     = 1L << 6,   // 64

        // ── Purchases (bits 7-12) ────────────────────────────
        ["Purchases.View"]  = 1L << 7,   // 128
        ["Purchases.Create"]= 1L << 8,   // 256
        ["Purchases.Edit"]  = 1L << 9,   // 512
        ["Purchases.Cancel"]= 1L << 10,  // 1024
        ["Purchases.Print"] = 1L << 11,  // 2048
        ["Purchases.Return"]= 1L << 12,  // 4096

        // ── Inventory (bits 13-17) ───────────────────────────
        ["Inventory.View"]      = 1L << 13,  // 8192
        ["Inventory.Transfer"]  = 1L << 14,  // 16384
        ["Inventory.Adjust"]    = 1L << 15,  // 32768
        ["Inventory.Count"]     = 1L << 16,  // 65536
        ["Warehouse.Manage"]    = 1L << 17,  // 131072

        // ── Customers (bits 18-21) ──────────────────────────
        ["Customers.View"]   = 1L << 18,  // 262144
        ["Customers.Create"] = 1L << 19,  // 524288
        ["Customers.Edit"]   = 1L << 20,  // 1048576
        ["Customers.Delete"] = 1L << 21,  // 2097152

        // ── Suppliers (bits 22-25) ──────────────────────────
        ["Suppliers.View"]   = 1L << 22,  // 4194304
        ["Suppliers.Create"] = 1L << 23,  // 8388608
        ["Suppliers.Edit"]   = 1L << 24,  // 16777216
        ["Suppliers.Delete"] = 1L << 25,  // 33554432

        // ── Products (bits 26-29) ───────────────────────────
        ["Products.View"]   = 1L << 26,  // 67108864
        ["Products.Create"] = 1L << 27,  // 134217728
        ["Products.Edit"]   = 1L << 28,  // 268435456
        ["Products.Delete"] = 1L << 29,  // 536870912

        // ── Reports / Accounting (bits 30-32) ───────────────
        ["Reports.View"]      = 1L << 30,  // 1073741824
        ["Accounting.View"]   = 1L << 31,  // 2147483648
        ["Accounting.Manage"] = 1L << 32,  // 4294967296

        // ── System (bits 33-35) ─────────────────────────────
        ["System.Settings"] = 1L << 33,  // 8589934592
        ["System.Users"]    = 1L << 34,  // 17179869184
        ["Roles.Manage"]    = 1L << 35,  // 34359738368

        // ── Operations (bits 36-38) ─────────────────────────
        ["Operations.Cashbox"]   = 1L << 36,  // 68719476736
        ["Operations.Banking"]   = 1L << 37,  // 137438953472
        ["Operations.Expenses"]  = 1L << 38,  // 274877906944

        // ── Audit (bit 39) ──────────────────────────────────
        ["Audit.Log"] = 1L << 39,  // 549755813888

        // ── Backup / Fiscal Year (bits 40-41) ───────────────
        ["Backup.Manage"]   = 1L << 40,  // 1099511627776
        ["FiscalYear.Manage"] = 1L << 41, // 2199023255552

        // ── Currencies (bits 44-45) ─────────────────────────
        ["Currencies.View"]   = 1L << 44,  // 17592186044416
        ["Currencies.Manage"] = 1L << 45,  // 35184372088832
    };

    private static readonly Dictionary<long, string> BitToPermissionCode;

    static PermissionService()
    {
        BitToPermissionCode = PermissionCodeToBit
            .GroupBy(kvp => kvp.Value)
            .ToDictionary(g => g.Key, g => g.First().Key);
    }

    // ═════════════════════════════════════════════════════════════════
    //  Permission module routing
    //  Maps each permission code to one of the 4 Role permission fields.
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// Determines which permission module (field on Role) a given code belongs to.
    /// </summary>
    private static string GetPermissionModule(string permissionCode) => permissionCode switch
    {
        "Sales.View" or "Sales.Create" or "Sales.Edit" or "Sales.Delete" or "Sales.Cancel" or "Sales.Return" or "Sales.Print" => "Sales",
        "Customers.View" or "Customers.Create" or "Customers.Edit" or "Customers.Delete" => "System",
        "Purchases.View" or "Purchases.Create" or "Purchases.Edit" or "Purchases.Cancel" or "Purchases.Print" or "Purchases.Return"
            or "Inventory.View" or "Inventory.Transfer" or "Inventory.Adjust" or "Inventory.Count" or "Warehouse.Manage"
            or "Products.View" or "Products.Create" or "Products.Edit" or "Products.Delete"
            or "Suppliers.View" or "Suppliers.Create" or "Suppliers.Edit" or "Suppliers.Delete" => "Inventory",
        "Reports.View" or "Accounting.View" or "Accounting.Manage"
            or "Operations.Cashbox" or "Operations.Banking" or "Operations.Expenses" => "Accounting",
        "System.Settings" or "System.Users" or "Roles.Manage" or "Audit.Log"
            or "Backup.Manage" or "FiscalYear.Manage"
            or "Currencies.View" or "Currencies.Manage" => "System",
        _ => "System"
    };

    /// <summary>
    /// Gets the appropriate permission bitmask field from the role for the given permission code.
    /// </summary>
    private static long GetRoleModuleMask(Role role, string permissionCode)
    {
        var module = GetPermissionModule(permissionCode);
        return module switch
        {
            "Sales" => role.SalesPermissions,
            "Inventory" => role.InventoryPermissions,
            "Accounting" => role.AccountingPermissions,
            "System" => role.SystemPermissions,
            _ => 0L
        };
    }

    // ═════════════════════════════════════════════════════════════════
    //  Core permission check methods
    // ═════════════════════════════════════════════════════════════════

    /// <inheritdoc />
    public async Task<bool> HasPermissionAsync(int userId, long requiredPermission, CancellationToken ct = default)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Id == userId, ct, "Role");
            if (user?.Role == null)
                return false;

            // Determine which module this permission bit belongs to by finding its code
            var permissionCode = BitToPermissionCode.GetValueOrDefault(requiredPermission);
            if (permissionCode == null)
                return false;

            var moduleMask = GetRoleModuleMask(user.Role, permissionCode);

            // -1 = all permissions for this module (super admin for category)
            if (moduleMask == -1)
                return true;

            return (moduleMask & requiredPermission) == requiredPermission;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check permission mask {Mask} for user {UserId}", requiredPermission, userId);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<long> GetPermissionsMaskAsync(int userId, CancellationToken ct = default)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Id == userId, ct, "Role");
            if (user?.Role == null)
                return 0;

            // For backward compatibility: combine all 4 masks into one
            return user.Role.SalesPermissions
                 | user.Role.InventoryPermissions
                 | user.Role.AccountingPermissions
                 | user.Role.SystemPermissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get permissions mask for user {UserId}", userId);
            return 0;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  Role permission management
    // ═════════════════════════════════════════════════════════════════

    /// <inheritdoc />
    public async Task<Result> SetRolePermissionsAsync(short roleId,
        long? salesPermissions = null, long? inventoryPermissions = null,
        long? accountingPermissions = null, long? systemPermissions = null,
        CancellationToken ct = default)
    {
        try
        {
            var role = await _uow.Roles.FirstOrDefaultIgnoreFiltersAsync(r => r.Id == roleId, ct);
            if (role is null)
                return Result.Failure("الدور غير موجود", ErrorCodes.NotFound);

            if (salesPermissions.HasValue) role.SetSalesPermissions(salesPermissions.Value);
            if (inventoryPermissions.HasValue) role.SetInventoryPermissions(inventoryPermissions.Value);
            if (accountingPermissions.HasValue) role.SetAccountingPermissions(accountingPermissions.Value);
            if (systemPermissions.HasValue) role.SetSystemPermissions(systemPermissions.Value);

            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting role permissions for role {RoleId}", roleId);
            return Result.Failure("حدث خطأ أثناء تحديث صلاحيات الدور.");
        }
    }

    /// <inheritdoc />
    public async Task<Result<List<RoleDto>>> GetAllRoleMasksAsync(CancellationToken ct = default)
    {
        try
        {
            var roles = await _uow.Roles.GetAllAsync(ct);
            var dtos = roles.Select(r => new RoleDto(
                r.Id, r.Name, r.Description, r.IsActive, r.IsSystem,
                r.SalesPermissions, r.InventoryPermissions,
                r.AccountingPermissions, r.SystemPermissions)).ToList();
            return Result<List<RoleDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all role masks");
            return Result<List<RoleDto>>.Failure("حدث خطأ أثناء جلب صلاحيات الأدوار.");
        }
    }

    /// <inheritdoc />
    public async Task<Result> AssignRoleToUserAsync(int userId, short roleId, CancellationToken ct = default)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
            if (user == null)
                return Result.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

            var role = await _uow.Roles.FirstOrDefaultAsync(r => r.Id == roleId, ct);
            if (role == null)
                return Result.Failure("الدور غير موجود", ErrorCodes.NotFound);

            // Set the user's RoleId directly (single-role model)
            user.SetRoleId(roleId);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Assigned role {RoleId} to user {UserId}", roleId, userId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign role {RoleId} to user {UserId}", roleId, userId);
            return Result.Failure("حدث خطأ أثناء تعيين الدور للمستخدم.");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  Permission code ↔ mask translation
    // ═════════════════════════════════════════════════════════════════

    /// <inheritdoc />
    public async Task<Result<List<string>>> GetUserPermissionsAsync(int userId, CancellationToken ct = default)
    {
        try
        {
            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Id == userId, ct, "Role");
            if (user?.Role == null)
                return Result<List<string>>.Failure("المستخدم غير موجود", ErrorCodes.NotFound);

            var role = user.Role;

            // Walk through each known permission code and check the appropriate module mask
            var granted = new List<string>();
            foreach (var (code, bitValue) in PermissionCodeToBit)
            {
                var moduleMask = GetRoleModuleMask(role, code);

                // -1 = all permissions (super admin for this module)
                if (moduleMask == -1 || (moduleMask & bitValue) == bitValue)
                    granted.Add(code);
            }

            return Result<List<string>>.Success(granted.OrderBy(x => x).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get permissions for user {UserId}", userId);
            return Result<List<string>>.Failure("حدث خطأ أثناء جلب صلاحيات المستخدم.");
        }
    }

    /// <inheritdoc />
    public async Task<bool> UserHasPermissionAsync(int userId, string permissionName, CancellationToken ct = default)
    {
        try
        {
            if (!PermissionCodeToBit.TryGetValue(permissionName, out var requiredBit))
            {
                _logger.LogWarning("Unknown permission code: {Permission}", permissionName);
                return false;
            }

            var user = await _uow.Users.FirstOrDefaultAsync(u => u.Id == userId, ct, "Role");
            if (user?.Role == null)
                return false;

            var moduleMask = GetRoleModuleMask(user.Role, permissionName);

            // -1 = all permissions for this module
            if (moduleMask == -1)
                return true;

            return (moduleMask & requiredBit) == requiredBit;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check permission {Permission} for user {UserId}", permissionName, userId);
            return false;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  Public helpers
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// Gets the bitmask value for a permission code string.
    /// Returns 0 if the code is unknown.
    /// </summary>
    public static long GetPermissionBit(string permissionCode)
    {
        return PermissionCodeToBit.TryGetValue(permissionCode, out var bit) ? bit : 0;
    }

    /// <summary>
    /// Gets the permission code string for a given bitmask value.
    /// Returns null if the value does not correspond to a single known permission.
    /// </summary>
    public static string? GetPermissionCode(long bitValue)
    {
        return BitToPermissionCode.TryGetValue(bitValue, out var code) ? code : null;
    }

    /// <summary>
    /// Returns all known permission code strings.
    /// </summary>
    public static IReadOnlyCollection<string> AllPermissionCodes => PermissionCodeToBit.Keys;
}
