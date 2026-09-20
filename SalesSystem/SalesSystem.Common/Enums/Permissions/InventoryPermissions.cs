namespace SalesSystem.Common.Enums.Permissions;

/// <summary>
/// Inventory module permission flags for bitmask authorization.
/// Each bit represents a discrete permission. All = -1 grants full access (Super Admin).
/// </summary>
[Flags]
public enum InventoryPermissions : long
{
    None = 0,
    ViewProducts = 1L << 0,
    CreateProducts = 1L << 1,
    EditProducts = 1L << 2,
    ViewStock = 1L << 3,
    AdjustStock = 1L << 4,
    TransferStock = 1L << 5,
    CountStock = 1L << 6,
    ViewBatches = 1L << 7,
    ManageCategories = 1L << 8,
    ManageUnits = 1L << 9,
    All = -1
}
