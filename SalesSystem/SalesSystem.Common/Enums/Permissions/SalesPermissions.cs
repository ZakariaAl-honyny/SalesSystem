namespace SalesSystem.Common.Enums.Permissions;

/// <summary>
/// Sales module permission flags for bitmask authorization.
/// Each bit represents a discrete permission. All = -1 grants full access (Super Admin).
/// </summary>
[Flags]
public enum SalesPermissions : long
{
    None = 0,
    ViewInvoices = 1L << 0,
    CreateInvoices = 1L << 1,
    EditInvoices = 1L << 2,
    DeleteInvoices = 1L << 3,
    CancelInvoices = 1L << 4,
    PrintInvoices = 1L << 5,
    ViewReturns = 1L << 6,
    CreateReturns = 1L << 7,
    ViewReceipts = 1L << 8,
    CreateReceipts = 1L << 9,
    ManagePrices = 1L << 10,
    ManageDiscounts = 1L << 11,
    All = -1
}
