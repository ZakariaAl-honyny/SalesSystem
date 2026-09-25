using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لسندات قبض العملاء — مستوحاة من بيانات MockData.
/// </summary>
internal static class MockCustomerReceiptsData
{
    public static List<CustomerReceiptDto> Receipts { get; } = new()
    {
        // ─── سند 1: مرحل — أحمد محمد علي — نقدي ───
        new CustomerReceiptDto(
            Id: 1,
            ReceiptNo: 1001,
            ReceiptDate: new DateTime(2026, 9, 1),
            CustomerId: 1,
            CustomerName: "أحمد محمد علي",
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Amount: 45000m,
            PaymentMethod: 1, // Cash
            PaymentMethodName: "نقدي",
            Notes: "تحصيل دفعة نقدية من العميل أحمد محمد علي",
            Status: 2, // Posted
            StatusName: "مرحّل",
            PostedAt: new DateTime(2026, 9, 1, 10, 0, 0),
            Applications: new List<CustomerReceiptApplicationDto>
            {
                new(1, 1, 2, 1002, 45000m, true)
            },
            IsActive: true
        ),

        // ─── سند 2: مرحل — شركة النور للتجارة — تحويل بنكي ───
        new CustomerReceiptDto(
            Id: 2,
            ReceiptNo: 1002,
            ReceiptDate: new DateTime(2026, 9, 3),
            CustomerId: 2,
            CustomerName: "شركة النور للتجارة",
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Amount: 175000m,
            PaymentMethod: 3, // BankTransfer
            PaymentMethodName: "تحويل بنكي",
            Notes: "حوالة بنكية سداداً لفاتورة بيع جملة رقم 1002",
            Status: 2, // Posted
            StatusName: "مرحّل",
            PostedAt: new DateTime(2026, 9, 3, 14, 30, 0),
            Applications: new List<CustomerReceiptApplicationDto>
            {
                new(2, 2, 2, 1002, 175000m, true)
            },
            IsActive: true
        ),

        // ─── سند 3: مسودة — مؤسسة اليمن الحديثة — نقدي ───
        new CustomerReceiptDto(
            Id: 3,
            ReceiptNo: 1003,
            ReceiptDate: new DateTime(2026, 9, 10),
            CustomerId: 3,
            CustomerName: "مؤسسة اليمن الحديثة",
            CashBoxId: 2,
            CashBoxName: "درج كاشير نقطة البيع 1 (الصالة)",
            Amount: 80000m,
            PaymentMethod: 1, // Cash
            PaymentMethodName: "نقدي",
            Notes: "دفعة مقدمة على حساب فاتورة قيد التجهيز",
            Status: 1, // Draft
            StatusName: "مسودة",
            PostedAt: null,
            Applications: new List<CustomerReceiptApplicationDto>(),
            IsActive: true
        ),

        // ─── سند 4: ملغى — محمد عبدالرحمن السالمي — نقدي ───
        new CustomerReceiptDto(
            Id: 4,
            ReceiptNo: 1004,
            ReceiptDate: new DateTime(2026, 9, 12),
            CustomerId: 4,
            CustomerName: "محمد عبدالرحمن السالمي",
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Amount: 20000m,
            PaymentMethod: 1, // Cash
            PaymentMethodName: "نقدي",
            Notes: "سند ملغي — خطأ في التحصيل",
            Status: 3, // Cancelled
            StatusName: "ملغي",
            PostedAt: null,
            Applications: new List<CustomerReceiptApplicationDto>(),
            IsActive: true
        ),

        // ─── سند 5: مسودة — محلات البدر للمواد الغذائية — شيك ───
        new CustomerReceiptDto(
            Id: 5,
            ReceiptNo: 1005,
            ReceiptDate: new DateTime(2026, 9, 15),
            CustomerId: 6,
            CustomerName: "محلات البدر للمواد الغذائية",
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Amount: 200000m,
            PaymentMethod: 2, // Cheque
            PaymentMethodName: "شيك",
            Notes: "شيك بنكي رقم 550123 — سداد جزئي",
            Status: 1, // Draft
            StatusName: "مسودة",
            PostedAt: null,
            Applications: new List<CustomerReceiptApplicationDto>(),
            IsActive: true
        )
    };
}
