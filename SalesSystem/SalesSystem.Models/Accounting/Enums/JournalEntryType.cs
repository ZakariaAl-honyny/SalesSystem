namespace SalesSystem.Models.Accounting.Enums;

public enum JournalEntryType : byte
{
    Manual = 1,
    Sales = 2,
    Purchase = 3,
    Receipt = 4,
    Payment = 5,
    Inventory = 6,
    Adjustment = 7,
    SalesReturn = 8,
    PurchaseReturn = 9,
    Expense = 10,
    StockWriteOff = 11,
    Transfer = 12,
    OpeningBalance = 13,
    CustomerReceipt = 14,
    SupplierPayment = 15
}
