using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// بيانات تجريبية للمخزون (دفعات + تسويات) — بيانات عربية واقعية.
/// </summary>
/// <remarks>
/// TODO: الحقول التالية أُزيلت من DTOs المخزون ولم تُعد موجودة:
/// InventoryBatchDto → ProductCode, ProductBarcode, ProductCategoryName, ProductUnitName, PurchaseInvoiceNo, SupplierName
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

    // ═══════════════════════════════════════════════
    // حركات المخزون — Inventory Transactions (Movements)
    // ═══════════════════════════════════════════════
    /// <remarks>
    /// بيت واحد لحركات المخزون في وضع التجربة (RULE-N169) — تُقرأ من
    /// <c>MockInventoryDesktopService.GetMovementsAsync</c> ولا تُشتق داخل الخدمة.
    /// <para>
    /// <b>اتساق البيانات (RULE-N167):</b> لكل (صنف، مخزن) يساوي صافي الحركات
    /// الكمية المتبقية في دفعات <see cref="Batches"/> للصنف نفسه في المخزن نفسه —
    /// شاي 125 · مناديل 95 · حليب 8 · زيت 50 · أرز 12 — والسكر 480 (بلا دفعات، يطابق رصيد الصنف).
    /// </para>
    /// <para>
    /// التواريخ نسبية إلى <c>DateTime.Today</c> لتبقى داخل نافذة آخر 30 يوماً
    /// (وداخل «هذا الشهر» في شاشة العرض)، والكميات في البنود موجبة دائماً عدا
    /// الجرد/التسوية فتُحفظ بإشارتها (خصم = سالب) كما تقرأها الشاشة.
    /// </para>
    /// </remarks>
    public static List<InventoryTransactionDto> Movements { get; } = new()
    {
        // ── 1: رصيد افتتاحي — سكر أبيض (المخزن الرئيسي) ──
        new InventoryTransactionDto(
            Id: 1,
            TransactionNo: "000001",
            MovementType: 10, // OpeningBalance
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: null,
            ReferenceType: null,
            Notes: "رصيد افتتاحي — سكر أبيض 1 كغ",
            CreatedAt: DateTime.Today.AddDays(-12).AddHours(9),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 1, InventoryTransactionId: 1, ProductUnitId: 4,
                    ProductUnitName: "سكر أبيض 1 كغ — كيس",
                    Quantity: 477.000m, UnitCost: 180m,
                    BatchNo: null, ExpiryDate: null, WarehouseId: 1)
            }),

        // ── 2: مشتريات — شاي ليبتون (الدفعة الأولى · KBS-2025-01) ──
        new InventoryTransactionDto(
            Id: 2,
            TransactionNo: "000002",
            MovementType: 1, // Purchase
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: 5001,
            ReferenceType: 1, // PurchaseInvoice
            Notes: "فاتورة شراء 5001 — شاي ليبتون 250غ",
            CreatedAt: DateTime.Today.AddDays(-12).AddHours(11),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 2, InventoryTransactionId: 2, ProductUnitId: 1,
                    ProductUnitName: "شاي ليبتون 250غ — قطعة",
                    Quantity: 100.000m, UnitCost: 120m,
                    BatchNo: "KBS-2025-01", ExpiryDate: new DateOnly(2026, 12, 31), WarehouseId: 1)
            }),

        // ── 3: مشتريات — مناديل ورقية (TISS-2026-Q1) ──
        new InventoryTransactionDto(
            Id: 3,
            TransactionNo: "000003",
            MovementType: 1, // Purchase
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: 5005,
            ReferenceType: 1, // PurchaseInvoice
            Notes: "فاتورة شراء 5005 — مناديل ورقية 150 ورقة",
            CreatedAt: DateTime.Today.AddDays(-11).AddHours(10),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 3, InventoryTransactionId: 3, ProductUnitId: 9,
                    ProductUnitName: "مناديل ورقية 150 ورقة — علبة",
                    Quantity: 120.000m, UnitCost: 170m,
                    BatchNo: "TISS-2026-Q1", ExpiryDate: null, WarehouseId: 1)
            }),

        // ── 4: مشتريات — حليب مجفف (MLK-EXP-01) ──
        new InventoryTransactionDto(
            Id: 4,
            TransactionNo: "000004",
            MovementType: 1, // Purchase
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: 5001,
            ReferenceType: 1, // PurchaseInvoice
            Notes: "فاتورة شراء 5001 — حليب مجفف 900غ",
            CreatedAt: DateTime.Today.AddDays(-10).AddHours(9).AddMinutes(30),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 4, InventoryTransactionId: 4, ProductUnitId: 8,
                    ProductUnitName: "حليب مجفف 900غ — علبة",
                    Quantity: 40.000m, UnitCost: 25m,
                    BatchNo: "MLK-EXP-01", ExpiryDate: new DateOnly(2026, 6, 1), WarehouseId: 1)
            }),

        // ── 5: مشتريات — زيت نباتي (OIL-2026-A) ──
        new InventoryTransactionDto(
            Id: 5,
            TransactionNo: "000005",
            MovementType: 1, // Purchase
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: 5002,
            ReferenceType: 1, // PurchaseInvoice
            Notes: "فاتورة شراء 5002 — زيت نباتي 1 لتر",
            CreatedAt: DateTime.Today.AddDays(-9).AddHours(14),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 5, InventoryTransactionId: 5, ProductUnitId: 2,
                    ProductUnitName: "زيت نباتي 1 لتر — زجاجة",
                    Quantity: 50.000m, UnitCost: 65m,
                    BatchNo: "OIL-2026-A", ExpiryDate: new DateOnly(2027, 3, 15), WarehouseId: 1)
            }),

        // ── 6: مشتريات — شاي ليبتون (الدفعة الثانية · KBS-2026-03) ──
        new InventoryTransactionDto(
            Id: 6,
            TransactionNo: "000006",
            MovementType: 1, // Purchase
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: 5003,
            ReferenceType: 1, // PurchaseInvoice
            Notes: "فاتورة شراء 5003 — شاي ليبتون 250غ (دفعة جديدة)",
            CreatedAt: DateTime.Today.AddDays(-8).AddHours(10),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 6, InventoryTransactionId: 6, ProductUnitId: 1,
                    ProductUnitName: "شاي ليبتون 250غ — قطعة",
                    Quantity: 80.000m, UnitCost: 120m,
                    BatchNo: "KBS-2026-03", ExpiryDate: new DateOnly(2027, 6, 30), WarehouseId: 1)
            }),

        // ── 7: مشتريات — أرز بسمتي (مخزن المعرض · RICE-2026-02) ──
        new InventoryTransactionDto(
            Id: 7,
            TransactionNo: "000007",
            MovementType: 1, // Purchase
            WarehouseId: 2,
            WarehouseName: "مخزن المعرض",
            ReferenceId: 5004,
            ReferenceType: 1, // PurchaseInvoice
            Notes: "فاتورة شراء 5004 — أرز بسمتي 5 كغ (مخزن المعرض)",
            CreatedAt: DateTime.Today.AddDays(-7).AddHours(11),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 7, InventoryTransactionId: 7, ProductUnitId: 3,
                    ProductUnitName: "أرز بسمتي 5 كغ — كيس",
                    Quantity: 30.000m, UnitCost: 50m,
                    BatchNo: "RICE-2026-02", ExpiryDate: new DateOnly(2027, 9, 1), WarehouseId: 2)
            }),

        // ── 8: تسوية — فائض جرد السكر (مرحّلة · ADJ-2026-001) ──
        new InventoryTransactionDto(
            Id: 8,
            TransactionNo: "000008",
            MovementType: 8, // Adjustment
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: 1,
            ReferenceType: 7, // Adjustment
            Notes: "تسوية فائض جرد ADJ-2026-001 — سكر أبيض 1 كغ",
            CreatedAt: DateTime.Today.AddDays(-6).AddHours(14).AddMinutes(30),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 8, InventoryTransactionId: 8, ProductUnitId: 4,
                    ProductUnitName: "سكر أبيض 1 كغ — كيس",
                    Quantity: 3.000m, UnitCost: 180m,
                    BatchNo: null, ExpiryDate: null, WarehouseId: 1)
            }),

        // ── 9: مبيعات — أرز بسمتي (مخزن المعرض) ──
        new InventoryTransactionDto(
            Id: 9,
            TransactionNo: "000009",
            MovementType: 3, // Sale
            WarehouseId: 2,
            WarehouseName: "مخزن المعرض",
            ReferenceId: null,
            ReferenceType: null,
            Notes: "صرف مبيعات — أرز بسمتي 5 كغ",
            CreatedAt: DateTime.Today.AddDays(-5).AddHours(16),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 9, InventoryTransactionId: 9, ProductUnitId: 3,
                    ProductUnitName: "أرز بسمتي 5 كغ — كيس",
                    Quantity: 18.000m, UnitCost: 50m,
                    BatchNo: "RICE-2026-02", ExpiryDate: new DateOnly(2027, 9, 1), WarehouseId: 2)
            }),

        // ── 10: مبيعات — شاي ليبتون ──
        new InventoryTransactionDto(
            Id: 10,
            TransactionNo: "000010",
            MovementType: 3, // Sale
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: null,
            ReferenceType: null,
            Notes: "صرف مبيعات — شاي ليبتون 250غ",
            CreatedAt: DateTime.Today.AddDays(-4).AddHours(11),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 10, InventoryTransactionId: 10, ProductUnitId: 1,
                    ProductUnitName: "شاي ليبتون 250غ — قطعة",
                    Quantity: 45.000m, UnitCost: 120m,
                    BatchNo: "KBS-2025-01", ExpiryDate: new DateOnly(2026, 12, 31), WarehouseId: 1)
            }),

        // ── 11: مبيعات — حليب مجفف ──
        new InventoryTransactionDto(
            Id: 11,
            TransactionNo: "000011",
            MovementType: 3, // Sale
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: null,
            ReferenceType: null,
            Notes: "صرف مبيعات — حليب مجفف 900غ",
            CreatedAt: DateTime.Today.AddDays(-3).AddHours(12).AddMinutes(30),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 11, InventoryTransactionId: 11, ProductUnitId: 8,
                    ProductUnitName: "حليب مجفف 900غ — علبة",
                    Quantity: 30.000m, UnitCost: 25m,
                    BatchNo: "MLK-EXP-01", ExpiryDate: new DateOnly(2026, 6, 1), WarehouseId: 1)
            }),

        // ── 12: مبيعات — مناديل ورقية ──
        new InventoryTransactionDto(
            Id: 12,
            TransactionNo: "000012",
            MovementType: 3, // Sale
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: null,
            ReferenceType: null,
            Notes: "صرف مبيعات — مناديل ورقية 150 ورقة",
            CreatedAt: DateTime.Today.AddDays(-2).AddHours(15),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 12, InventoryTransactionId: 12, ProductUnitId: 9,
                    ProductUnitName: "مناديل ورقية 150 ورقة — علبة",
                    Quantity: 25.000m, UnitCost: 170m,
                    BatchNo: "TISS-2026-Q1", ExpiryDate: null, WarehouseId: 1)
            }),

        // ── 13: مبيعات — شاي ليبتون (إتمام عجز الجرد) ──
        new InventoryTransactionDto(
            Id: 13,
            TransactionNo: "000013",
            MovementType: 3, // Sale
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: null,
            ReferenceType: null,
            Notes: "صرف مبيعات — شاي ليبتون 250غ",
            CreatedAt: DateTime.Today.AddDays(-1).AddHours(10),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 13, InventoryTransactionId: 13, ProductUnitId: 1,
                    ProductUnitName: "شاي ليبتون 250غ — قطعة",
                    Quantity: 8.000m, UnitCost: 120m,
                    BatchNo: "KBS-2025-01", ExpiryDate: new DateOnly(2026, 12, 31), WarehouseId: 1)
            }),

        // ── 14: تسوية — عجز جرد الشاي والحليب (مرحّلة · ADJ-2026-002) ──
        new InventoryTransactionDto(
            Id: 14,
            TransactionNo: "000014",
            MovementType: 8, // Adjustment
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            ReferenceId: 2,
            ReferenceType: 7, // Adjustment
            Notes: "تسوية عجز جرد ADJ-2026-002 — شاي ليبتون 250غ · حليب مجفف 900غ",
            CreatedAt: DateTime.Today.AddDays(-1).AddHours(16),
            CreatedByUserId: 1,
            Lines: new List<InventoryTransactionLineDto>
            {
                new(Id: 14, InventoryTransactionId: 14, ProductUnitId: 1,
                    ProductUnitName: "شاي ليبتون 250غ — قطعة",
                    Quantity: -2.000m, UnitCost: 120m,
                    BatchNo: "KBS-2025-01", ExpiryDate: new DateOnly(2026, 12, 31), WarehouseId: 1),
                new(Id: 15, InventoryTransactionId: 14, ProductUnitId: 8,
                    ProductUnitName: "حليب مجفف 900غ — علبة",
                    Quantity: -2.000m, UnitCost: 25m,
                    BatchNo: "MLK-EXP-01", ExpiryDate: new DateOnly(2026, 6, 1), WarehouseId: 1)
            })
    };

    // ═══════════════════════════════════════════════
    // محاضر الجرد الفعلي — Stocktakings (بيت واحد: يقرأه ويكتبه MockStocktakingDesktopService)
    // ═══════════════════════════════════════════════
    /// <remarks>
    /// RULE-N167 — الأربعة تغطي كل حالة يجب أن يراها المالك:
    /// (1) مرحّل بأثرين متعاكسين (فائض 3 سكر · عجز 2 شاي) · (2) مسودة قابلة للترحيل ·
    /// (3) ملغي يرفض الترحيل · (4) مسودة عجزها (15) أكبر من الرصيد المتاح (0) ⇒ تصل إلى
    /// رسالة الرفض <c>MockStockLedger.ShortageMessage</c> عند تجربة الترحيل.
    /// أثر المحاضر المرحّلة مسجَّل مسبقاً في الأرصدة والحركات (000008 · 000014) فلا يُعاد تطبيقه.
    /// </remarks>
    public static List<StocktakingDto> Stocktakings { get; } = new()
    {
        // ── 1: جرد ربع سنوي — مرحّل (فائض سكر 3 · عجز شاي 2) ──
        new StocktakingDto(
            Id: 1,
            DocumentNo: "STK-2026-001",
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            Date: new DateTime(2026, 8, 17),
            Notes: "جرد ربع سنوي — آب/أغسطس 2026",
            CommitteeHead: "أحمد محمد",
            CommitteeMember: "سارة علي",
            Storekeeper: "خالد عبدالله",
            Status: 2,
            StatusName: "مرحّل",
            CreatedAt: new DateTime(2026, 8, 17, 9, 0, 0),
            PostedAt: new DateTime(2026, 8, 17, 14, 0, 0),
            CancelledAt: null,
            Lines: new List<StocktakingLineDto>
            {
                new(Id: 1, StocktakingId: 1, ProductId: 4, ProductName: "سكر أبيض 1 كغ",
                    Barcode: null, BookQuantity: 477.000m, CountedQuantity: 480.000m),
                new(Id: 2, StocktakingId: 1, ProductId: 1, ProductName: "شاي ليبتون 250غ",
                    Barcode: "6200000000011", BookQuantity: 125.000m, CountedQuantity: 123.000m)
            }),

        // ── 2: جرد مخزن المعرض — مسودة (قابلة للترحيل) ──
        new StocktakingDto(
            Id: 2,
            DocumentNo: "STK-2026-002",
            WarehouseId: 2,
            WarehouseName: "مخزن المعرض",
            Date: new DateTime(2026, 9, 21),
            Notes: "جرد شهري — مخزن المعرض",
            CommitteeHead: "منى حسن",
            CommitteeMember: "عمر يوسف",
            Storekeeper: "خالد عبدالله",
            Status: 1,
            StatusName: "مسودة",
            CreatedAt: new DateTime(2026, 9, 21, 9, 0, 0),
            PostedAt: null,
            CancelledAt: null,
            Lines: new List<StocktakingLineDto>
            {
                new(Id: 3, StocktakingId: 2, ProductId: 3, ProductName: "أرز بسمتي 5 كغ",
                    Barcode: null, BookQuantity: 12.000m, CountedQuantity: 11.000m),
                new(Id: 4, StocktakingId: 2, ProductId: 6, ProductName: "معجون طماطم 400غ",
                    Barcode: null, BookQuantity: 90.000m, CountedQuantity: 92.000m)
            }),

        // ── 3: جرد أُلغي بعد الترحيل — يرفض الترحيل مرة ثانية ──
        new StocktakingDto(
            Id: 3,
            DocumentNo: "STK-2026-003",
            WarehouseId: 1,
            WarehouseName: "المخزن الرئيسي",
            Date: new DateTime(2026, 10, 5),
            Notes: "جرد أُلغي لخطأ في العدّ",
            CommitteeHead: "أحمد محمد",
            CommitteeMember: "منى حسن",
            Storekeeper: "خالد عبدالله",
            Status: 3,
            StatusName: "ملغي",
            CreatedAt: new DateTime(2026, 10, 5, 9, 0, 0),
            PostedAt: new DateTime(2026, 10, 5, 11, 0, 0),
            CancelledAt: new DateTime(2026, 10, 5, 12, 0, 0),
            Lines: new List<StocktakingLineDto>
            {
                new(Id: 5, StocktakingId: 3, ProductId: 9, ProductName: "مناديل ورقية 150 ورقة",
                    Barcode: null, BookQuantity: 95.000m, CountedQuantity: 90.000m)
            }),

        // ── 4: مسودة عجزها أكبر من المتاح (مياه نافدة · رصيد 0) — لبلوغ رسالة رفض النقص ──
        new StocktakingDto(
            Id: 4,
            DocumentNo: "STK-2026-004",
            WarehouseId: 3,
            WarehouseName: "مخزن الفرع - عدن",
            Date: new DateTime(2026, 11, 10),
            Notes: "جرد فرع عدن — يرفض الترحيل لنقص الرصيد",
            CommitteeHead: "عمر يوسف",
            CommitteeMember: "سارة علي",
            Storekeeper: "أحمد محمد",
            Status: 1,
            StatusName: "مسودة",
            CreatedAt: new DateTime(2026, 11, 10, 8, 30, 0),
            PostedAt: null,
            CancelledAt: null,
            Lines: new List<StocktakingLineDto>
            {
                new(Id: 6, StocktakingId: 4, ProductId: 12, ProductName: "مياه معدنية 1.5 لتر",
                    Barcode: "6200000012018", BookQuantity: 15.000m, CountedQuantity: 0.000m)
            })
    };

    // ═══════════════════════════════════════════════
    // سندات التحويل المخزني — Warehouse Transfers
    // ═══════════════════════════════════════════════
    /// <remarks>
    /// RULE-N167 — الأربعة تغطي كل حالة: (1) مرحّل بأثرين · (2) مسودة قابلة للترحيل ·
    /// (3) ملغي يرفض الترحيل · (4) مسودة كمية مصدرها (5) أكبر من رصيد الصنف (0) ⇒ رسالة رفض النقص.
    /// رصيد المحاكاة على مستوى الصنف لا المخزن، فالتحويل الداخلي لا يغيّر إجمالي الصنف،
    /// وأثر السندات المرحّلة مخزَّن أصلاً في الأرصدة فيُقرأ ولا يُعاد تطبيقه.
    /// </remarks>
    public static List<WarehouseTransferDto> WarehouseTransfers { get; } = new()
    {
        // ── 1: تحويل معتمد من الرئيسي إلى المعرض — مرحّل ──
        new WarehouseTransferDto(
            Id: 1,
            DocumentNo: "TRF-2026-001",
            FromWarehouseId: 1,
            FromWarehouseName: "المخزن الرئيسي",
            ToWarehouseId: 2,
            ToWarehouseName: "مخزن المعرض",
            Date: new DateTime(2026, 9, 10),
            Notes: "تغذية معرض العرض — أسبوع 37",
            Status: 2,
            StatusName: "مرحّل",
            CreatedAt: new DateTime(2026, 9, 10, 10, 0, 0),
            PostedAt: new DateTime(2026, 9, 10, 11, 30, 0),
            CancelledAt: null,
            Lines: new List<WarehouseTransferLineDto>
            {
                new(Id: 1, WarehouseTransferId: 1, ProductId: 1, ProductName: "شاي ليبتون 250غ",
                    Barcode: "6200000000011", BatchNo: "KBS-2026-03",
                    ExpiryDate: new DateOnly(2027, 6, 30), Quantity: 10.000m),
                new(Id: 2, WarehouseTransferId: 1, ProductId: 4, ProductName: "سكر أبيض 1 كغ",
                    Barcode: null, BatchNo: null, ExpiryDate: null, Quantity: 25.000m)
            }),

        // ── 2: تحويل إلى فرع عدن — مسودة (قابلة للترحيل) ──
        new WarehouseTransferDto(
            Id: 2,
            DocumentNo: "TRF-2026-002",
            FromWarehouseId: 1,
            FromWarehouseName: "المخزن الرئيسي",
            ToWarehouseId: 3,
            ToWarehouseName: "مخزن الفرع - عدن",
            Date: new DateTime(2026, 10, 12),
            Notes: "شحنة فرع عدن — بانتظار الترحيل",
            Status: 1,
            StatusName: "مسودة",
            CreatedAt: new DateTime(2026, 10, 12, 9, 15, 0),
            PostedAt: null,
            CancelledAt: null,
            Lines: new List<WarehouseTransferLineDto>
            {
                new(Id: 3, WarehouseTransferId: 2, ProductId: 9, ProductName: "مناديل ورقية 150 ورقة",
                    Barcode: null, BatchNo: "TISS-2026-Q1", ExpiryDate: null, Quantity: 20.000m),
                new(Id: 4, WarehouseTransferId: 2, ProductId: 2, ProductName: "زيت نباتي 1 لتر",
                    Barcode: "6200000000028", BatchNo: "OIL-2026-A",
                    ExpiryDate: new DateOnly(2027, 3, 15), Quantity: 5.000m)
            }),

        // ── 3: تحويل أُلغي بعد الترحيل — يرفض الترحيل مرة ثانية ──
        new WarehouseTransferDto(
            Id: 3,
            DocumentNo: "TRF-2026-003",
            FromWarehouseId: 2,
            FromWarehouseName: "مخزن المعرض",
            ToWarehouseId: 1,
            ToWarehouseName: "المخزن الرئيسي",
            Date: new DateTime(2026, 7, 3),
            Notes: "إرجاع أرز إلى المخزن الرئيسي — أُلغي",
            Status: 3,
            StatusName: "ملغي",
            CreatedAt: new DateTime(2026, 7, 3, 10, 0, 0),
            PostedAt: new DateTime(2026, 7, 3, 11, 0, 0),
            CancelledAt: new DateTime(2026, 7, 3, 16, 0, 0),
            Lines: new List<WarehouseTransferLineDto>
            {
                new(Id: 5, WarehouseTransferId: 3, ProductId: 3, ProductName: "أرز بسمتي 5 كغ",
                    Barcode: null, BatchNo: "RICE-2026-02",
                    ExpiryDate: new DateOnly(2027, 9, 1), Quantity: 4.000m)
            }),

        // ── 4: مسودة مصدرها نافد (مياه · رصيد 0) — لبلوغ رسالة رفض النقص ──
        new WarehouseTransferDto(
            Id: 4,
            DocumentNo: "TRF-2026-004",
            FromWarehouseId: 3,
            FromWarehouseName: "مخزن الفرع - عدن",
            ToWarehouseId: 1,
            ToWarehouseName: "المخزن الرئيسي",
            Date: new DateTime(2026, 11, 12),
            Notes: "تحويل يرفض الترحيل لنقص رصيد المصدر",
            Status: 1,
            StatusName: "مسودة",
            CreatedAt: new DateTime(2026, 11, 12, 8, 45, 0),
            PostedAt: null,
            CancelledAt: null,
            Lines: new List<WarehouseTransferLineDto>
            {
                new(Id: 6, WarehouseTransferId: 4, ProductId: 12, ProductName: "مياه معدنية 1.5 لتر",
                    Barcode: "6200000012018", BatchNo: null, ExpiryDate: null, Quantity: 5.000m)
            })
    };
}
