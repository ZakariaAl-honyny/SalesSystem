using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لسندات صرف الموردين — مستقلة للاختبار بدون API.
/// </summary>
internal static class MockSupplierPaymentsData
{
    public static List<SupplierPaymentDto> Payments { get; } = new()
    {
        // ── سند 1: مرحل — شيك بنكي لمجموعة هائل سعيد ──
        new SupplierPaymentDto(
            Id: 1,
            PaymentNo: "2001",
            SupplierId: 1,
            SupplierName: "مجموعة هائل سعيد أنعم وشركاه",
            Amount: 200000m,
            PaymentMethod: 3, // تحويل بنكي
            PaymentDate: new DateOnly(2026, 9, 1),
            PurchaseInvoiceId: null,
            Notes: "سداد دفعة من مستحقات فواتير الزيوت والمواد التموينية",
            BranchId: 1,
            CashBoxId: 2,
            CashBoxName: "بنك التضامن الإسلامي (حساب جاري)",
            Status: 2, // مرحل
            Currency: "YER",
            CreatedAt: new DateTime(2026, 9, 1, 10, 0, 0),
            PostedAt: new DateTime(2026, 9, 1, 10, 5, 0),
            CancelledAt: null),

        // ── سند 2: مسودة — نقداً لشركة مياه حدة ──
        new SupplierPaymentDto(
            Id: 2,
            PaymentNo: "2002",
            SupplierId: 2,
            SupplierName: "شركة مياه ومشروبات حدة المحدودة",
            Amount: 55000m,
            PaymentMethod: 1, // نقدي
            PaymentDate: new DateOnly(2026, 9, 5),
            PurchaseInvoiceId: null,
            Notes: "سداد قيمة توريد مياه معدنية للشهر الحالي",
            BranchId: 1,
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Status: 1, // مسودة
            Currency: "YER",
            CreatedAt: new DateTime(2026, 9, 5, 14, 30, 0),
            PostedAt: null,
            CancelledAt: null),

        // ── سند 3: مرحل — شيك لمؤسسة التضامن ──
        new SupplierPaymentDto(
            Id: 3,
            PaymentNo: "2003",
            SupplierId: 3,
            SupplierName: "مؤسسة التضامن لتجارة المواد الغذائية",
            Amount: 310000m,
            PaymentMethod: 2, // شيك
            PaymentDate: new DateOnly(2026, 9, 10),
            PurchaseInvoiceId: null,
            Notes: "سداد فاتورة أرز بسمتي وحبوب بموجب شيك رقم 440915",
            BranchId: 1,
            CashBoxId: 2,
            CashBoxName: "بنك التضامن الإسلامي (حساب جاري)",
            Status: 2, // مرحل
            Currency: "YER",
            CreatedAt: new DateTime(2026, 9, 10, 11, 0, 0),
            PostedAt: new DateTime(2026, 9, 10, 11, 10, 0),
            CancelledAt: null),

        // ── سند 4: ملغي — تحويل بنكي لشركة الكبوس ──
        new SupplierPaymentDto(
            Id: 4,
            PaymentNo: "2004",
            SupplierId: 4,
            SupplierName: "شركة الكبوس للتجارة والصناعة",
            Amount: 88000m,
            PaymentMethod: 3, // تحويل بنكي
            PaymentDate: new DateOnly(2026, 9, 15),
            PurchaseInvoiceId: null,
            Notes: "سداد فاتورة شاي وبهارات — تم الإلغاء بسبب خطأ في المبلغ",
            BranchId: 1,
            CashBoxId: 2,
            CashBoxName: "بنك التضامن الإسلامي (حساب جاري)",
            Status: 3, // ملغي
            Currency: "YER",
            CreatedAt: new DateTime(2026, 9, 15, 9, 0, 0),
            PostedAt: null,
            CancelledAt: new DateTime(2026, 9, 15, 16, 0, 0)),

        // ── سند 5: مسودة — نقداً لمجموعة هائل سعيد (دفعة ثانية) ──
        new SupplierPaymentDto(
            Id: 5,
            PaymentNo: "2005",
            SupplierId: 1,
            SupplierName: "مجموعة هائل سعيد أنعم وشركاه",
            Amount: 150000m,
            PaymentMethod: 1, // نقدي
            PaymentDate: new DateOnly(2026, 9, 20),
            PurchaseInvoiceId: null,
            Notes: "دفعة نقدية على الحساب الجاري — سداد جزئي",
            BranchId: 1,
            CashBoxId: 1,
            CashBoxName: "الصندوق الرئيسي (الخزينة المركزية)",
            Status: 1, // مسودة
            Currency: "YER",
            CreatedAt: new DateTime(2026, 9, 20, 13, 0, 0),
            PostedAt: null,
            CancelledAt: null)
    };
}
