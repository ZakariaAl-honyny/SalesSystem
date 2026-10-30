using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لسندات صرف الموردين — مستقلة للاختبار بدون API.
/// TODO: CashBoxId / CashBoxName / Status / Currency / CreatedAt / PostedAt / CancelledAt
///       لم تعد موجودة في SupplierPaymentDto — البيانات التجريبية لم تعد تحمل الصندوق أو الحالة.
/// </summary>
internal static class MockSupplierPaymentsData
{
    public static List<SupplierPaymentDto> Payments { get; } = new()
    {
        // ── سند 1: شيك بنكي لمجموعة هائل سعيد ──
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
            BranchId: 1),

        // ── سند 2: نقداً لشركة مياه حدة ──
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
            BranchId: 1),

        // ── سند 3: شيك لمؤسسة التضامن ──
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
            BranchId: 1),

        // ── سند 4: تحويل بنكي لشركة الكبوس ──
        new SupplierPaymentDto(
            Id: 4,
            PaymentNo: "2004",
            SupplierId: 4,
            SupplierName: "شركة الكبوس للتجارة والصناعة",
            Amount: 88000m,
            PaymentMethod: 3, // تحويل بنكي
            PaymentDate: new DateOnly(2026, 9, 15),
            PurchaseInvoiceId: null,
            Notes: "سداد فاتورة شاي وبهارات",
            BranchId: 1),

        // ── سند 5: نقداً لمجموعة هائل سعيد (دفعة ثانية) ──
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
            BranchId: 1)
    };
}
