using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للعهد النثرية — 6 طلبات واقعية ببيانات عربية.
/// </summary>
internal static class MockPettyCashData
{
    public static List<PettyCashRequestDto> Requests { get; } = new()
    {
        // ── 1: طلب مُسوّى بالكامل — أحمد علي عبده ──
        new PettyCashRequestDto(
            Id: 1,
            RequestNo: 1001,
            Date: new DateTime(2026, 8, 10),
            EmployeeName: "أحمد علي عبده",
            EmployeeId: 1,
            Amount: 50000m,
            Reason: "شراء قرطاسية ومكتبية للمحل",
            Status: 4, // Settled
            ApprovedBy: "سامي عبدالملك الجوفي",
            DisbursedDate: new DateTime(2026, 8, 11),
            SettlementDate: new DateTime(2026, 8, 18),
            SettlementAmount: 47500m,
            RefundAmount: 2500m,
            Notes: "تم التسوية بالكامل — فواتير مرفقة"
        ),

        // ── 2: طلب مصروف بانتظار التسوية — محمد عبد الله الصلوي ──
        new PettyCashRequestDto(
            Id: 2,
            RequestNo: 1002,
            Date: new DateTime(2026, 9, 5),
            EmployeeName: "محمد عبد الله الصلوي",
            EmployeeId: 2,
            Amount: 30000m,
            Reason: "مصاريف بريد وشحن سريع للعملاء",
            Status: 3, // Disbursed
            ApprovedBy: "سامي عبدالملك الجوفي",
            DisbursedDate: new DateTime(2026, 9, 6),
            SettlementDate: null,
            SettlementAmount: null,
            RefundAmount: null,
            Notes: "بانتظار تقديم الفواتير والتسوية"
        ),

        // ── 3: طلب معتمد لم يُصرف بعد — خالد صالح القاضي ──
        new PettyCashRequestDto(
            Id: 3,
            RequestNo: 1003,
            Date: new DateTime(2026, 9, 15),
            EmployeeName: "خالد صالح القاضي",
            EmployeeId: 3,
            Amount: 20000m,
            Reason: "صيانة عاجلة لثلاجة العرض الرئيسية",
            Status: 2, // Approved
            ApprovedBy: "مدير النظام الأعلى (Admin)",
            DisbursedDate: null,
            SettlementDate: null,
            SettlementAmount: null,
            RefundAmount: null,
            Notes: "تم الاعتماد — بانتظار الصرف من الصندوق"
        ),

        // ── 4: طلب قيد الانتظار — ياسر طارق الوجيه ──
        new PettyCashRequestDto(
            Id: 4,
            RequestNo: 1004,
            Date: new DateTime(2026, 9, 18),
            EmployeeName: "ياسر طارق الوجيه",
            EmployeeId: 5,
            Amount: 15000m,
            Reason: "تعبئة وقود سيارة التوزيع",
            Status: 1, // Pending
            ApprovedBy: null,
            DisbursedDate: null,
            SettlementDate: null,
            SettlementAmount: null,
            RefundAmount: null,
            Notes: null
        ),

        // ── 5: طلب مرفوض — سامي عبدالملك المنصوري ──
        new PettyCashRequestDto(
            Id: 5,
            RequestNo: 1005,
            Date: new DateTime(2026, 9, 12),
            EmployeeName: "سامي عبدالملك المنصوري",
            EmployeeId: 4,
            Amount: 80000m,
            Reason: "شراء أدوات تنظيف ومواد معطرات",
            Status: 5, // Rejected
            ApprovedBy: null,
            DisbursedDate: null,
            SettlementDate: null,
            SettlementAmount: null,
            RefundAmount: null,
            Notes: "مرفوض — المبلغ مبالغ فيه، يرجى تقديم تقدير مناسب"
        ),

        // ── 6: طلب قيد الانتظار — أحمد علي عبده (عهدة ثانية) ──
        new PettyCashRequestDto(
            Id: 6,
            RequestNo: 1006,
            Date: new DateTime(2026, 9, 20),
            EmployeeName: "أحمد علي عبده",
            EmployeeId: 1,
            Amount: 10000m,
            Reason: "مصاريف ضيافة 방문 المندوبين",
            Status: 1, // Pending
            ApprovedBy: null,
            DisbursedDate: null,
            SettlementDate: null,
            SettlementAmount: null,
            RefundAmount: null,
            Notes: null
        ),
    };
}
