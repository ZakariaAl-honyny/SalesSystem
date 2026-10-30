using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للمخزون (دفعات + جرد + تسويات) — بيانات عربية واقعية.
/// </summary>
/// <remarks>
/// TODO: الحقول التالية أُزيلت من DTOs المخزون ولم تُعد موجودة:
/// InventoryBatchDto → ProductCode, ProductBarcode, ProductCategoryName, ProductUnitName, PurchaseInvoiceNo, SupplierName
/// InventoryCountDto → PostedAt, CancelledAt, CommitteeMembers, BranchId, BranchName
/// InventoryCountLineDto → ProductName, ProductCode, ProductId, UnitName, UnitCost, BatchNo
/// InventoryAdjustmentDto → ApprovedBy, BranchId, BranchName
/// InventoryAdjustmentLineDto → ProductName, ProductCode, ProductId, UnitName, BatchNo, Notes
/// </remarks>
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
            SupplierBatchNo: "KBS-2025-01-SUP"),

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
            SupplierBatchNo: null),

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
            SupplierBatchNo: "OIL-FAC-77"),

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
            SupplierBatchNo: null),

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
            SupplierBatchNo: "MLK-FAC-2025"),

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
            SupplierBatchNo: null)
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
                    Difference: -2.000m, Notes: "عجز — يُرجّح كسر أثناء التحميل"),
                new(Id: 2, InventoryCountId: 1, ProductUnitId: 2,
                    ProductUnitName: "زيت نباتي 1 لتر — زجاجة",
                    ExpectedQuantity: 50.000m, ActualQuantity: 50.000m,
                    Difference: 0m, Notes: null),
                new(Id: 3, InventoryCountId: 1, ProductUnitId: 4,
                    ProductUnitName: "سكر أبيض 1 كغ — كيس",
                    ExpectedQuantity: 200.000m, ActualQuantity: 203.000m,
                    Difference: 3.000m, Notes: "فائض — أكياس إضافية من دفعة غير مسجّلة"),
                new(Id: 4, InventoryCountId: 1, ProductUnitId: 8,
                    ProductUnitName: "حليب مجفف 900غ — علبة",
                    ExpectedQuantity: 30.000m, ActualQuantity: 28.000m,
                    Difference: -2.000m, Notes: "عجز — علب تالفة تم التخلص منها")
            }),

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
                    Difference: -1.000m, Notes: "عجز — كيس مفتوح للعرض"),
                new(Id: 6, InventoryCountId: 2, ProductUnitId: 6,
                    ProductUnitName: "معجون طماطم 400غ — علبة",
                    ExpectedQuantity: 90.000m, ActualQuantity: 92.000m,
                    Difference: 2.000m, Notes: "فائض — علب من عرض ترويجي")
            }),

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
            Lines: null)
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
                    UnitCost: 800m)
            }),

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
                    UnitCost: 1500m),
                new(Id: 3, InventoryAdjustmentId: 2, ProductUnitId: 8,
                    ProductUnitName: "حليب مجفف 900غ — علبة",
                    ExpectedQuantity: 30.000m, ActualQuantity: 28.000m,
                    UnitCost: 4500m)
            }),

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
                    UnitCost: 3500m),
                new(Id: 5, InventoryAdjustmentId: 3, ProductUnitId: 6,
                    ProductUnitName: "معجون طماطم 400غ — علبة",
                    ExpectedQuantity: 90.000m, ActualQuantity: 92.000m,
                    UnitCost: 600m)
            })
    };
}
