using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية لمراكز التكلفة — 10 مراكز (3 رئيسي + 7 فرعي) مستوحاة من INITIAL_COST_CENTERS.
/// </summary>
internal static class MockCostCentersData
{
    public static List<CostCenterDto> CostCenters { get; } = new()
    {
        // ═══════════════════════════════════════════════════════════════════
        // المستوى 1 — مراكز رئيسية
        // ═══════════════════════════════════════════════════════════════════

        // ── 1: مراكز الإدارة العامة والخدمات ──
        new CostCenterDto(
            Id: 1,
            Code: "1",
            Name: "مراكز الإدارة العامة والخدمات",
            NameEn: "General & Administrative Centers",
            ParentCode: null,
            Level: 1,
            IsLeaf: false,
            Budget: 800000m,
            ManagerName: null,
            Status: 1,
            Notes: "مراكز التكلفة التابعة للإدارة العامة"
        ),

        // ── 2: مراكز الفروع والمنافذ التشغيلية ──
        new CostCenterDto(
            Id: 2,
            Code: "2",
            Name: "مراكز الفروع والمنافذ التشغيلية",
            NameEn: "Operational Branches Centers",
            ParentCode: null,
            Level: 1,
            IsLeaf: false,
            Budget: 2500000m,
            ManagerName: null,
            Status: 1,
            Notes: "مراكز الإيرادات والمصروفات المباشرة للفروع"
        ),

        // ── 3: مراكز النقل وسلسلة التوريد ──
        new CostCenterDto(
            Id: 3,
            Code: "3",
            Name: "مراكز النقل وسلسلة التوريد",
            NameEn: "Logistics & Supply Chain",
            ParentCode: null,
            Level: 1,
            IsLeaf: false,
            Budget: 900000m,
            ManagerName: null,
            Status: 1,
            Notes: "مركبات التوزيع ومستودعات التخزين المركزية"
        ),

        // ═══════════════════════════════════════════════════════════════════
        // المستوى 2 — مراكز فرعية
        // ═══════════════════════════════════════════════════════════════════

        // ── 101: الموارد البشرية ──
        new CostCenterDto(
            Id: 4,
            Code: "101",
            Name: "إدارة الموارد البشرية والشؤون الإدارية",
            NameEn: "HR & Administration",
            ParentCode: "1",
            Level: 2,
            IsLeaf: true,
            Budget: 350000m,
            ManagerName: "أ. خالد الوادعي",
            Status: 1,
            Notes: null
        ),

        // ── 102: تقنية المعلومات ──
        new CostCenterDto(
            Id: 5,
            Code: "102",
            Name: "إدارة تكنولوجيا المعلومات ونقاط البيع IT",
            NameEn: "IT & POS Systems",
            ParentCode: "1",
            Level: 2,
            IsLeaf: true,
            Budget: 450000m,
            ManagerName: "م. أحمد الشامي",
            Status: 1,
            Notes: null
        ),

        // ── 201: فرع حدة ──
        new CostCenterDto(
            Id: 6,
            Code: "201",
            Name: "فرع حدة الرئيسي (صنعاء)",
            NameEn: "Hadda Main Branch",
            ParentCode: "2",
            Level: 2,
            IsLeaf: true,
            Budget: 1200000m,
            ManagerName: "فهد الحميري",
            Status: 1,
            Notes: null
        ),

        // ── 202: فرع التحرير ──
        new CostCenterDto(
            Id: 7,
            Code: "202",
            Name: "فرع التحرير التجاري (صنعاء)",
            NameEn: "Al-Tahreer Branch",
            ParentCode: "2",
            Level: 2,
            IsLeaf: true,
            Budget: 800000m,
            ManagerName: "طارق الأصبحي",
            Status: 1,
            Notes: null
        ),

        // ── 203: فرع الستين ──
        new CostCenterDto(
            Id: 8,
            Code: "203",
            Name: "فرع الستين والمستودع السريع",
            NameEn: "Al-Sitteen Branch & Depot",
            ParentCode: "2",
            Level: 2,
            IsLeaf: true,
            Budget: 500000m,
            ManagerName: "سامي القدسي",
            Status: 1,
            Notes: null
        ),

        // ── 301: أسطول التوزيع ──
        new CostCenterDto(
            Id: 9,
            Code: "301",
            Name: "أسطول شاحنات التوزيع الخفيف",
            NameEn: "Light Delivery Fleet",
            ParentCode: "3",
            Level: 2,
            IsLeaf: true,
            Budget: 550000m,
            ManagerName: "جمال الصبري",
            Status: 1,
            Notes: null
        ),

        // ── 302: المستودع المركزي ──
        new CostCenterDto(
            Id: 10,
            Code: "302",
            Name: "المستودع المركزي للبضائع الجافة",
            NameEn: "Central Dry Goods Warehouse",
            ParentCode: "3",
            Level: 2,
            IsLeaf: true,
            Budget: 350000m,
            ManagerName: "هشام اليافعي",
            Status: 1,
            Notes: null
        ),
    };
}
