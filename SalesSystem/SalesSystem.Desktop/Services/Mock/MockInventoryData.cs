using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للمخزون (دفعات + جرد + تسويات) — بيانات عربية واقعية.
/// </summary>
internal static class MockInventoryData
{
    // ═══════════════════════════════════════════════
    // دفعات المخزون — Inventory Batches (FIFO/FEFO)
    // ═══════════════════════════════════════════════
    public static List<InventoryBatchDto> Batches { get; } = new()
    {
        // ── 1: شاي ليبتون — دفعة قديمة (مستهلكة جزئياً) ──
        new InventoryBatchDto(
            Id: 1,
            ProductId: 1,
            ProductName: "شاي ليبتون 250غ",
            PurchaseInvoiceId: 5001,
            PurchaseInvoiceLineId: 1,
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            BatchNo: "KBS-2025-01",
            QuantityReceived: 100m,
            QuantityRemaining: 45m,
            UnitCost: 1400m,
            ExpiryDate: new DateOnly(2026, 12, 31),
            SupplierBatchNo: "KBS-2025-01-SUP",
            ProductCode: "102030",
            ProductBarcode: "629100100101",
            ProductCategoryName: "مشروبات",
            ProductUnitName: "قطعة",
            PurchaseInvoiceNo: 5001,
            SupplierName: "مؤسسة كبوس اليمن"),

        // ── 2: شاي ليبتون — دفعة جديدة (سارية) ──
        new InventoryBatchDto(
            Id: 2,
            ProductId: 1,
            ProductName: "شاي ليبتون 250غ",
            PurchaseInvoiceId: 5003,
            PurchaseInvoiceLineId: 5,
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            BatchNo: "KBS-2026-03",
            QuantityReceived: 80m,
            QuantityRemaining: 80m,
            UnitCost: 1550m,
            ExpiryDate: new DateOnly(2027, 6, 30),
            SupplierBatchNo: null,
            ProductCode: "102030",
            ProductBarcode: "629100100101",
            ProductCategoryName: "مشروبات",
            ProductUnitName: "قطعة",
            PurchaseInvoiceNo: 5003,
            SupplierName: "مؤسسة كبوس اليمن"),

        // ── 3: زيت نباتي — دفعة كاملة (لم تُستهل) ──
        new InventoryBatchDto(
            Id: 3,
            ProductId: 2,
            ProductName: "زيت نباتي 1 لتر",
            PurchaseInvoiceId: 5002,
            PurchaseInvoiceLineId: 3,
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            BatchNo: "OIL-2026-A",
            QuantityReceived: 50m,
            QuantityRemaining: 50m,
            UnitCost: 1350m,
            ExpiryDate: new DateOnly(2027, 3, 15),
            SupplierBatchNo: "OIL-FAC-77",
            ProductCode: "102031",
            ProductBarcode: "629100100104",
            ProductCategoryName: "زيوت",
            ProductUnitName: "زجاجة",
            PurchaseInvoiceNo: 5002,
            SupplierName: "شركة عافية اليمن"),

        // ── 4: أرز بسمتي — دفعة مخزن المعرض ──
        new InventoryBatchDto(
            Id: 4,
            ProductId: 3,
            ProductName: "أرز بسمتي 5 كغ",
            PurchaseInvoiceId: 5004,
            PurchaseInvoiceLineId: 8,
            WarehouseId: 2,
            WarehouseName: "مخزن المعرض",
            BatchNo: "RICE-2026-02",
            QuantityReceived: 30m,
            QuantityRemaining: 12m,
            UnitCost: 3200m,
            ExpiryDate: new DateOnly(2027, 9, 1),
            SupplierBatchNo: null,
            ProductCode: "102032",
            ProductBarcode: "629100100106",
            ProductCategoryName: "حبوب",
            ProductUnitName: "كيس",
            PurchaseInvoiceNo: 5004,
            SupplierName: "مؤسسة الشعلان للتجارة"),

        // ── 5: حليب مجفف — دفعة منتهية الصلاحية ──
        new InventoryBatchDto(
            Id: 5,
            ProductId: 8,
            ProductName: "حليب مجفف 900غ",
            PurchaseInvoiceId: 5001,
            PurchaseInvoiceLineId: 12,
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            BatchNo: "MLK-EXP-01",
            QuantityReceived: 40m,
            QuantityRemaining: 8m,
            UnitCost: 4200m,
            ExpiryDate: new DateOnly(2026, 6, 1),
            SupplierBatchNo: "MLK-FAC-2025",
            ProductCode: "102037",
            ProductBarcode: "629100100112",
            ProductCategoryName: "ألبان",
            ProductUnitName: "علبة",
            PurchaseInvoiceNo: 5001,
            SupplierName: "شركة نستله اليمن"),

        // ── 6: مناديل ورقية — صنف بدون صلاحية (FIFO فقط) ──
        new InventoryBatchDto(
            Id: 6,
            ProductId: 9,
            ProductName: "مناديل ورقية 150 ورقة",
            PurchaseInvoiceId: 5005,
            PurchaseInvoiceLineId: 15,
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            BatchNo: "TISS-2026-Q1",
            QuantityReceived: 120m,
            QuantityRemaining: 95m,
            UnitCost: 380m,
            ExpiryDate: null,
            SupplierBatchNo: null,
            ProductCode: "102038",
            ProductBarcode: "629100100114",
            ProductCategoryName: "منظفات",
            ProductUnitName: "علبة",
            PurchaseInvoiceNo: 5005,
            SupplierName: "شركة الأورمان للورق")
    };

    // ═══════════════════════════════════════════════
    // جرد المخزون — Inventory Counts (Stocktaking Sessions)
    // ═══════════════════════════════════════════════
    public static List<InventoryCountDto> Counts { get; } = new()
    {
        // ── 1: جرد ربع سنوي — المخزن الرئيسي — مرحّل ──
        new InventoryCountDto(
            Id: 1,
            CountNo: "ST-2026-001",
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            Status: 2,
            StatusName: "مرحّل",
            Notes: "جرد ربع سنوي أول — لجنة مكونة من 3 أعضاء",
            CreatedAt: new DateTime(2026, 8, 15, 9, 0, 0),
            CreatedByUserId: 1,
            Lines: new List<InventoryCountLineDto>
            {
                new(Id: 1, InventoryCountId: 1, ProductUnitId: 1,
                    ProductUnitName: "شاي ليبتون 250غ — قطعة",
                    ExpectedQuantity: 125.000m, ActualQuantity: 123.000m,
                    Difference: -2.000m, Notes: "عجز — يُرجّح كسر أثناء التحميل",
                    ProductName: "شاي ليبتون 250غ", ProductCode: "102030", ProductId: 1,
                    UnitName: "قطعة", UnitCost: 1500m, BatchNo: "KBS-2025-01"),
                new(Id: 2, InventoryCountId: 1, ProductUnitId: 2,
                    ProductUnitName: "زيت نباتي 1 لتر — زجاجة",
                    ExpectedQuantity: 50.000m, ActualQuantity: 50.000m,
                    Difference: 0m, Notes: null,
                    ProductName: "زيت نباتي 1 لتر", ProductCode: "102031", ProductId: 2,
                    UnitName: "زجاجة", UnitCost: 1500m, BatchNo: "OIL-2026-A"),
                new(Id: 3, InventoryCountId: 1, ProductUnitId: 4,
                    ProductUnitName: "سكر أبيض 1 كغ — كيس",
                    ExpectedQuantity: 200.000m, ActualQuantity: 203.000m,
                    Difference: 3.000m, Notes: "فائض — أكياس إضافية من دفعة غير مسجّلة",
                    ProductName: "سكر أبيض 1 كغ", ProductCode: "102033", ProductId: 4,
                    UnitName: "كيس", UnitCost: 800m, BatchNo: null),
                new(Id: 4, InventoryCountId: 1, ProductUnitId: 8,
                    ProductUnitName: "حليب مجفف 900غ — علبة",
                    ExpectedQuantity: 30.000m, ActualQuantity: 28.000m,
                    Difference: -2.000m, Notes: "عجز — علب تالفة تم التخلص منها",
                    ProductName: "حليب مجفف 900غ", ProductCode: "102037", ProductId: 8,
                    UnitName: "علبة", UnitCost: 4500m, BatchNo: "MLK-EXP-01")
            },
            PostedAt: new DateTime(2026, 8, 16, 14, 30, 0),
            CancelledAt: null,
            CommitteeMembers: "أ. أحمد الوصابي (رئيس اللجنة)، أ. محمد العماري (عضو)، أ. فؤاد الريمي (أمين المستودع)",
            BranchId: 1,
            BranchName: "الفرع الرئيسي — صنعاء"),

        // ── 2: جرد شهري — مخزن المعرض — مسودة ──
        new InventoryCountDto(
            Id: 2,
            CountNo: "ST-2026-002",
            WarehouseId: 2,
            WarehouseName: "مخزن المعرض",
            Status: 1,
            StatusName: "مسودة",
            Notes: "جرد شهري سبتمبر — تدقيق أصناف العرض",
            CreatedAt: new DateTime(2026, 9, 20, 10, 0, 0),
            CreatedByUserId: 2,
            Lines: new List<InventoryCountLineDto>
            {
                new(Id: 5, InventoryCountId: 2, ProductUnitId: 3,
                    ProductUnitName: "أرز بسمتي 5 كغ — كيس",
                    ExpectedQuantity: 12.000m, ActualQuantity: 11.000m,
                    Difference: -1.000m, Notes: "عجز — كيس مفتوح للعرض",
                    ProductName: "أرز بسمتي 5 كغ", ProductCode: "102032", ProductId: 3,
                    UnitName: "كيس", UnitCost: 3500m, BatchNo: "RICE-2026-02"),
                new(Id: 6, InventoryCountId: 2, ProductUnitId: 6,
                    ProductUnitName: "معجون طماطم 400غ — علبة",
                    ExpectedQuantity: 90.000m, ActualQuantity: 92.000m,
                    Difference: 2.000m, Notes: "فائض — علب من عرض ترويجي",
                    ProductName: "معجون طماطم 400غ", ProductCode: "102035", ProductId: 6,
                    UnitName: "علبة", UnitCost: 600m, BatchNo: null)
            },
            PostedAt: null,
            CancelledAt: null,
            CommitteeMembers: "أ. ياسر الحميري (رئيس اللجنة)، أ. سالم باوزير (عضو)",
            BranchId: 1,
            BranchName: "الفرع الرئيسي — صنعاء"),

        // ── 3: جرد استثنائي — تالف — ملغى ──
        new InventoryCountDto(
            Id: 3,
            CountNo: "ST-2026-003",
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            Status: 3,
            StatusName: "ملغي",
            Notes: "تم إلغاؤه بسبب خطأ في تحديد المستودع",
            CreatedAt: new DateTime(2026, 9, 10, 8, 0, 0),
            CreatedByUserId: 1,
            Lines: null,
            PostedAt: null,
            CancelledAt: new DateTime(2026, 9, 10, 9, 30, 0),
            CommitteeMembers: null,
            BranchId: 1,
            BranchName: "الفرع الرئيسي — صنعاء")
    };

    // ═══════════════════════════════════════════════
    // تسويات المخزون — Inventory Adjustments
    // ═══════════════════════════════════════════════
    public static List<InventoryAdjustmentDto> Adjustments { get; } = new()
    {
        // ── 1: تسوية فائض جرد — مرحّلة ──
        new InventoryAdjustmentDto(
            Id: 1,
            AdjustmentNo: "ADJ-2026-001",
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            AdjustmentType: 1,
            AdjustmentTypeName: "إضافة",
            Reason: "تسوية فائض جرد ربع سنوي — أغسطس 2026",
            Status: 2,
            StatusName: "مرحّل",
            CreatedAt: new DateTime(2026, 8, 17, 10, 0, 0),
            CreatedByUserId: 1,
            PostedAt: new DateTime(2026, 8, 17, 14, 0, 0),
            CancelledAt: null,
            Lines: new List<InventoryAdjustmentLineDto>
            {
                new(Id: 1, InventoryAdjustmentId: 1, ProductUnitId: 4,
                    ProductUnitName: "سكر أبيض 1 كغ — كيس",
                    ExpectedQuantity: 200.000m, ActualQuantity: 203.000m,
                    UnitCost: 800m,
                    ProductName: "سكر أبيض 1 كغ", ProductCode: "102033", ProductId: 4,
                    UnitName: "كيس", BatchNo: null,
                    Notes: "فائض 3 أكياس — إضافة للمخزون")
            },
            ApprovedBy: "أ. أحمد الوصابي — مدير المخازن",
            BranchId: 1,
            BranchName: "الفرع الرئيسي — صنعاء"),

        // ── 2: تسوية عجز + تلف — مرحّلة ──
        new InventoryAdjustmentDto(
            Id: 2,
            AdjustmentNo: "ADJ-2026-002",
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            AdjustmentType: 2,
            AdjustmentTypeName: "خصم",
            Reason: "تسوية عجز جرد + تلف مخزني — أغسطس 2026",
            Status: 2,
            StatusName: "مرحّل",
            CreatedAt: new DateTime(2026, 8, 18, 11, 0, 0),
            CreatedByUserId: 1,
            PostedAt: new DateTime(2026, 8, 18, 15, 0, 0),
            CancelledAt: null,
            Lines: new List<InventoryAdjustmentLineDto>
            {
                new(Id: 2, InventoryAdjustmentId: 2, ProductUnitId: 1,
                    ProductUnitName: "شاي ليبتون 250غ — قطعة",
                    ExpectedQuantity: 125.000m, ActualQuantity: 123.000m,
                    UnitCost: 1500m,
                    ProductName: "شاي ليبتون 250غ", ProductCode: "102030", ProductId: 1,
                    UnitName: "قطعة", BatchNo: "KBS-2025-01",
                    Notes: "عجز 2 وحدة — كسر أثناء التحميل"),
                new(Id: 3, InventoryAdjustmentId: 2, ProductUnitId: 8,
                    ProductUnitName: "حليب مجفف 900غ — علبة",
                    ExpectedQuantity: 30.000m, ActualQuantity: 28.000m,
                    UnitCost: 4500m,
                    ProductName: "حليب مجفف 900غ", ProductCode: "102037", ProductId: 8,
                    UnitName: "علبة", BatchNo: "MLK-EXP-01",
                    Notes: "عجز 2 علبة — تالف ومنتهي الصلاحية")
            },
            ApprovedBy: "أ. أحمد الوصابي — مدير المخازن",
            BranchId: 1,
            BranchName: "الفرع الرئيسي — صنعاء"),

        // ── 3: تسوية تصحيح — مسودة ──
        new InventoryAdjustmentDto(
            Id: 3,
            AdjustmentNo: "ADJ-2026-003",
            WarehouseId: 2,
            WarehouseName: "مخزن المعرض",
            AdjustmentType: 3,
            AdjustmentTypeName: "تصحيح",
            Reason: "تصحيح خطأ إدخال — كمية أرز غير مسجّلة في النظام",
            Status: 1,
            StatusName: "مسودة",
            CreatedAt: new DateTime(2026, 9, 21, 9, 0, 0),
            CreatedByUserId: 2,
            PostedAt: null,
            CancelledAt: null,
            Lines: new List<InventoryAdjustmentLineDto>
            {
                new(Id: 4, InventoryAdjustmentId: 3, ProductUnitId: 3,
                    ProductUnitName: "أرز بسمتي 5 كغ — كيس",
                    ExpectedQuantity: 12.000m, ActualQuantity: 11.000m,
                    UnitCost: 3500m,
                    ProductName: "أرز بسمتي 5 كغ", ProductCode: "102032", ProductId: 3,
                    UnitName: "كيس", BatchNo: "RICE-2026-02",
                    Notes: "عجز كيس — عرض ترويجي لم يُسجَّل"),
                new(Id: 5, InventoryAdjustmentId: 3, ProductUnitId: 6,
                    ProductUnitName: "معجون طماطم 400غ — علبة",
                    ExpectedQuantity: 90.000m, ActualQuantity: 92.000m,
                    UnitCost: 600m,
                    ProductName: "معجون طماطم 400غ", ProductCode: "102035", ProductId: 6,
                    UnitName: "علبة", BatchNo: null,
                    Notes: "فائض 2 علبة — عرض ترويجي")
            },
            ApprovedBy: null,
            BranchId: 1,
            BranchName: "الفرع الرئيسي — صنعاء")
    };
}
