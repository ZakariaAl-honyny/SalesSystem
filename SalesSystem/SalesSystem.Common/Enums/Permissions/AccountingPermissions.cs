namespace SalesSystem.Common.Enums.Permissions;

/// <summary>
/// Accounting module permission flags for bitmask authorization.
/// Each bit represents a discrete permission. All = -1 grants full access (Super Admin).
/// </summary>
[Flags]
public enum AccountingPermissions : long
{
    None = 0,
    ViewJournalEntries = 1L << 0,
    CreateJournalEntries = 1L << 1,
    ViewAccounts = 1L << 2,
    ManageAccounts = 1L << 3,
    ViewCashBoxes = 1L << 4,
    ManageCashBoxes = 1L << 5,
    ViewExpenses = 1L << 6,
    CreateExpenses = 1L << 7,
    ViewReceiptVouchers = 1L << 8,
    CreateReceiptVouchers = 1L << 9,
    ViewPaymentVouchers = 1L << 10,
    CreatePaymentVouchers = 1L << 11,
    ViewReports = 1L << 12,
    All = -1
}
