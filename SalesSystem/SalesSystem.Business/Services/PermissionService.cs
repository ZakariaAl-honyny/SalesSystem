using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Services;

/// <summary>
/// Permission checking service for the single-admin system.
/// There is no role entity — the only account is the Super Admin, which is granted
/// full access (-1) across every permission module. All checks short-circuit to
/// "allowed" without a database lookup.
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(ILogger<PermissionService> logger)
    {
        _logger = logger;
    }

    // ═════════════════════════════════════════════════════════════════
    //  Permission code → bit value mapping (kept for the Desktop API
    //  surface and for future multi-user work).
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

    // ═════════════════════════════════════════════════════════════════
    //  Core permission check methods — single admin always has full access
    // ═════════════════════════════════════════════════════════════════

    /// <inheritdoc />
    public Task<bool> HasPermissionAsync(int userId, long requiredPermission, CancellationToken ct = default)
    {
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<long> GetPermissionsMaskAsync(int userId, CancellationToken ct = default)
    {
        // -1 = all bits set (super admin).
        return Task.FromResult(-1L);
    }

    /// <inheritdoc />
    public Task<Result<List<string>>> GetUserPermissionsAsync(int userId, CancellationToken ct = default)
    {
        var all = PermissionCodeToBit.Keys.OrderBy(x => x).ToList();
        return Task.FromResult(Result<List<string>>.Success(all));
    }

    /// <inheritdoc />
    public Task<bool> UserHasPermissionAsync(int userId, string permissionName, CancellationToken ct = default)
    {
        if (!PermissionCodeToBit.ContainsKey(permissionName))
        {
            _logger.LogWarning("Unknown permission code: {Permission}", permissionName);
        }
        return Task.FromResult(true);
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
    /// Returns all known permission code strings.
    /// </summary>
    public static IReadOnlyCollection<string> AllPermissionCodes => PermissionCodeToBit.Keys;
}
