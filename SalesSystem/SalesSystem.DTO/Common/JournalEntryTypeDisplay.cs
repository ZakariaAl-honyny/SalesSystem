using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.DTO.Common;

/// <summary>
/// Provides Arabic display names for journal entry types.
/// Single source of truth for journal entry type labels.
/// </summary>
public static class JournalEntryTypeDisplay
{
    /// <summary>
    /// Gets the Arabic display name for a journal entry type.
    /// </summary>
    public static string GetArabicName(JournalEntryType entryType) => entryType switch
    {
        JournalEntryType.Manual => "يدوي",
        JournalEntryType.Sales => "مبيعات",
        JournalEntryType.Purchase => "مشتريات",
        JournalEntryType.Receipt => "سند قبض",
        JournalEntryType.Payment => "سند صرف",
        JournalEntryType.Inventory => "مخزون",
        JournalEntryType.Adjustment => "تسوية",
        JournalEntryType.SalesReturn => "مرتجع مبيعات",
        JournalEntryType.PurchaseReturn => "مرتجع مشتريات",
        JournalEntryType.Expense => "مصروف",
        JournalEntryType.StockWriteOff => "إعدام مخزون",
        JournalEntryType.Transfer => "تحويل",
        JournalEntryType.OpeningBalance => "رصيد افتتاحي",
        JournalEntryType.CustomerReceipt => "مقبوضات عميل",
        JournalEntryType.SupplierPayment => "مدفوعات مورد",
        _ => "غير معروف"
    };

    /// <summary>
    /// Gets all journal entry types with their Arabic names, in numeric order.
    /// Useful for binding to ComboBox controls.
    /// </summary>
    public static IReadOnlyList<(JournalEntryType Value, string ArabicName)> All { get; } = new List<(JournalEntryType, string)>
    {
        (JournalEntryType.Manual, "يدوي"),
        (JournalEntryType.Sales, "مبيعات"),
        (JournalEntryType.Purchase, "مشتريات"),
        (JournalEntryType.Receipt, "سند قبض"),
        (JournalEntryType.Payment, "سند صرف"),
        (JournalEntryType.Inventory, "مخزون"),
        (JournalEntryType.Adjustment, "تسوية"),
        (JournalEntryType.SalesReturn, "مرتجع مبيعات"),
        (JournalEntryType.PurchaseReturn, "مرتجع مشتريات"),
        (JournalEntryType.Expense, "مصروف"),
        (JournalEntryType.StockWriteOff, "إعدام مخزون"),
        (JournalEntryType.Transfer, "تحويل"),
        (JournalEntryType.OpeningBalance, "رصيد افتتاحي"),
        (JournalEntryType.CustomerReceipt, "مقبوضات عميل"),
        (JournalEntryType.SupplierPayment, "مدفوعات مورد")
    };
}
