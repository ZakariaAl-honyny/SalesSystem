using System.Globalization;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.DTO;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// <b>بيت واحد لأثر المخزون في وضع التجربة (RULE-N169 / RULE-N172):</b> تُستهلك من
/// فاتورة البيع · فاتورة الشراء · مرتجع البيع · مرتجع الشراء (ومحاضر الجرد وتسويات المخزون
/// وسندات التحويل) — ولا تُنسخ نسخة ثانية في أي منها.
/// <para>
/// <b>الثابت الملزم (RULE-N167):</b> رصيد بيانات التجربة يعكس المستندات
/// <b>المرحّلة وغير الملغاة</b> فقط — الحفظ/التعديل كمسودة لا يحرّك رصيداً،
/// والحركة تقع مرة واحدة عند الترحيل، وتُعكس مرة واحدة عند إلغاء مستند مرحّل.
/// </para>
/// <para>
/// <b>كل تغيير رصيد يُسجَّل حركة (AGENTS.md §2.7):</b> <see cref="Apply"/> لم يبقَ يكتب
/// <c>CurrentStock</c> وحده — يُلحق سطر حركة في <see cref="MockInventoryData.Movements"/>
/// فتُرى ترحيلات المالك نفسها في شاشة حركات المخزون (<c>frmListInventoryTransactions</c>).
/// ترتيب الكتابة مقصود: <b>الحركة أولاً ثم الرصيد</b> — فلا يبقى رصيد تغيّر بلا سطر حركة.
/// </para>
/// <para>
/// <b>الدفعات لا تشيخ وحدها (RULE-N167):</b> كل خصم يستهلك
/// <see cref="MockInventoryData.Batches"/> للصنف في المخزن نفسه <b>الأقدم أولاً</b>
/// (<c>QuantityRemaining -= المأخوذ</c>، بدون سالب)، وكل إضافة تترك الدفعات كما هي
/// (إنشاء دفعة شأن مستند الشراء في المرحلة ب).
/// </para>
/// <para>
/// هذا الصنف هو <b>الموضع الوحيد</b> الذي يكتب <c>MockData.Products.CurrentStock</c>
/// و<c>InventoryBatchDto.QuantityRemaining</c> معاً (RULE-N171).
/// </para>
/// </summary>
internal static class MockStockLedger
{
    /// <summary>نص رفض نقص المخزون — مصدر واحد لكل الخدمات التي تحرّك الرصيد (نفس نص رفض BLL في المرحلة ب).</summary>
    internal const string ShortageMessage = "الكمية المطلوبة أكبر من المتاح في المخزن";

    /// <summary>اسم مستخدم بيانات التجربة الذي تُنسب إليه الحركات المولَّدة (مطابق للمستخدم المزروع في الحركات الأولى).</summary>
    private const string MockUserName = "مدير النظام";

    /// <summary>مجاميع الكميات لكل صنف من سطور مستند (يجمع السطور المتكررة لنفس الصنف).</summary>
    internal static Dictionary<int, decimal> QuantitiesByProduct(IEnumerable<(int ProductId, decimal Quantity)> lines)
    {
        var result = new Dictionary<int, decimal>();
        foreach (var line in lines)
            result[line.ProductId] = (result.TryGetValue(line.ProductId, out var qty) ? qty : 0m) + line.Quantity;

        return result;
    }

    /// <summary>
    /// هل يكفي الرصيد لكل الأصناف المطلوبة؟ يُستدعى **قبل** أي خصم حتى لا تحدث حركة جزئية.
    /// صنف بلا رصيد في بيانات التجربة لا يُقارن (لا رصيد معروف له).
    /// </summary>
    internal static bool HasSufficientStock(IReadOnlyDictionary<int, decimal> quantities)
    {
        foreach (var (productId, quantity) in quantities)
        {
            if (quantity <= 0m)
                continue;

            var product = MockData.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
                continue;

            if (quantity > product.CurrentStock)
                return false;
        }

        return true;
    }

    /// <summary>
    /// يطبّق الأثر على الأرصدة دفعة واحدة، ويسجّل **حركة مخزون لكل صنف تغيّر رصيده فعلاً**:
    /// <c>direction = -1</c> خصم (بيع · مرتجع شراء) و<c>+1</c> إضافة (شراء · مرتجع بيع).
    /// </summary>
    /// <param name="quantities">مجموع الكمية لكل صنف (من <see cref="QuantitiesByProduct"/>).</param>
    /// <param name="direction">اتجاه الأثر: <c>-1</c> خارج · <c>+1</c> داخل.</param>
    /// <param name="type">
    /// نوع الحركة الواقعي — كل مستدعٍ يمرّر نوع مستنده الحقيقي
    /// (<see cref="InventoryTransactionType.Sale"/> · <see cref="InventoryTransactionType.Purchase"/> ·
    /// المرتجعان · <see cref="InventoryTransactionType.Count"/> · التحويلان). والافتراضي
    /// <see cref="InventoryTransactionType.Adjustment"/> باقٍ للاستدعاء القديم بلا سياق فقط.
    /// </param>
    /// <param name="warehouseId">المخزن — <c>0</c> يعني: المخزن النشط الأول.</param>
    /// <param name="reference">رقم المستند المرجعي: يُحفظ نصاً في البيان، وكمعرّف رقمي إن كان رقماً.</param>
    /// <param name="date">تاريخ الحركة — الافتراضي «الآن».</param>
    internal static void Apply(
        IReadOnlyDictionary<int, decimal> quantities,
        int direction,
        InventoryTransactionType type = InventoryTransactionType.Adjustment,
        short warehouseId = 0,
        string? reference = null,
        DateTime? date = null)
    {
        if (quantities is null || quantities.Count == 0)
            return;

        short resolvedWarehouseId = ResolveWarehouseId(warehouseId);
        string? warehouseName = MockData.Warehouses.FirstOrDefault(w => w.Id == resolvedWarehouseId)?.Name;
        DateTime movedAt = date ?? DateTime.Now;

        foreach (var (productId, quantity) in quantities)
        {
            decimal change = direction * quantity;
            if (change == 0m)
                continue;

            var product = MockData.Products.FirstOrDefault(p => p.Id == productId);
            if (product is null)
                continue; // لا رصيد في بيانات التجربة لهذا الصنف

            decimal newBalance = product.CurrentStock + change;

            // الحركة أولاً ثم الرصيد: لو تعثّر السطر لا يكون الرصيد قد تحرّك (§2.7)
            AppendMovement(product, change, newBalance, type, resolvedWarehouseId, warehouseName, reference, movedAt);

            // الدفعات تشيخ مع الرصيد: الخصم يستهلك أقدم الدفعات، والإضافة **لا** تنشئ دفعة
            // (إنشاء دفعة مستندُ شراء في المرحلة ب — تجربةٌ لا تُختلق هنا — RULE-N167).
            if (change < 0m)
                ConsumeBatches(productId, resolvedWarehouseId, -change);

            var index = MockData.Products.IndexOf(product);
            MockData.Products[index] = product with { CurrentStock = newBalance };
        }
    }

    /// <summary>
    /// يستهلك <paramref name="quantity"/> من دفعات (الصنف، المخزن) **الأقدم أولاً** (FIFO: بترتيب
    /// إدخال الدفعة، وهو ترتيب وصولها في بيانات التجربة).
    /// <para>
    /// القواعد الملزمة: <c>QuantityRemaining -= المأخوذ</c> لكل دفعة بالترتيب، والتوقف عند
    /// انتهاء الكمية المطلوبة، و<b>لا سالب أبداً</b> (الدفعة المنتهية تبقى <c>0</c> ولا تُحذف).
    /// </para>
    /// <para>
    /// إن نفدت كل الدفعات وبقيت كمية غير مُخصَّصة (صنف بلا دفعات مثل السكر، أو رصيده الأكبر في
    /// مخزن آخر) فلا يُرفض شيء هنا: الحارس الملزم للرصيد هو
    /// <see cref="HasSufficientStock"/> قبل النداء، ورصيد المحاكاة على مستوى الصنف لا المخزن.
    /// </para>
    /// </summary>
    private static void ConsumeBatches(int productId, short warehouseId, decimal quantity)
    {
        decimal remaining = quantity;
        if (remaining <= 0m)
            return;

        var batches = MockInventoryData.Batches
            .Where(b => b.ProductId == productId
                        && b.QuantityRemaining > 0m
                        && (warehouseId <= 0 || b.WarehouseId == warehouseId))
            .OrderBy(b => b.Id)   // الأقدم أولاً
            .ToList();

        foreach (var batch in batches)
        {
            if (remaining <= 0m)
                break;

            decimal taken = Math.Min(batch.QuantityRemaining, remaining);
            if (taken <= 0m)
                continue;

            int index = MockInventoryData.Batches.IndexOf(batch);
            MockInventoryData.Batches[index] = batch with { QuantityRemaining = batch.QuantityRemaining - taken };
            remaining -= taken;
        }
    }

    /// <summary>
    /// يُسجّل حركة واحدة بسطر واحد لصنف تغيّر رصيده — كل حقول
    /// <see cref="InventoryTransactionDto"/> و<see cref="InventoryTransactionLineDto"/> معبَّأة من بيانات التجربة.
    /// </summary>
    private static void AppendMovement(
        ProductDto product,
        decimal change,
        decimal newBalance,
        InventoryTransactionType type,
        short warehouseId,
        string? warehouseName,
        string? reference,
        DateTime movedAt)
    {
        // الوحدة الأساسية للصنف هي وحدة الحركة — ونفس الترتيب الذي تتبعه شاشات المخزون عند قراءة الوحدة
        var unit = MockData.ProductUnits.FirstOrDefault(u => u.ProductId == product.Id && u.IsBaseUnit)
                   ?? MockData.ProductUnits.FirstOrDefault(u => u.ProductId == product.Id);

        int movementId = NextMovementId();

        var line = new InventoryTransactionLineDto(
            Id: NextLineId(),
            InventoryTransactionId: movementId,
            ProductUnitId: unit?.Id ?? 0,
            ProductUnitName: unit?.UnitName,
            Quantity: SignedQuantity(change, type),
            UnitCost: unit?.CostPrice ?? 0m,
            BatchNo: null,
            ExpiryDate: null,
            WarehouseId: warehouseId);

        var movement = new InventoryTransactionDto(
            Id: movementId,
            TransactionNo: NextTransactionNo(),
            MovementType: (byte)type,
            WarehouseId: warehouseId,
            WarehouseName: warehouseName,
            ReferenceId: ParseReferenceId(reference),
            ReferenceType: ReferenceTypeOf(type),
            Notes: BuildNotes(product.Name, change, newBalance, reference),
            CreatedAt: movedAt,
            CreatedByUserName: MockUserName,
            Lines: new List<InventoryTransactionLineDto> { line });

        MockInventoryData.Movements.Add(movement);
    }

    /// <summary>المخزن المطلوب، أو المخزن النشط الأول حين لا يمرّر المستدعي مخزناً (<c>0</c>).</summary>
    private static short ResolveWarehouseId(short warehouseId)
    {
        if (warehouseId > 0)
            return warehouseId;

        var fallback = MockData.Warehouses.FirstOrDefault(w => w.IsActive) ?? MockData.Warehouses.FirstOrDefault();
        return fallback?.Id ?? (short)0;
    }

    /// <summary>
    /// إشارة الكمية في سطر الحركة — بنفس اصطلاح بيانات التجربة المزروعة: الجرد والتسوية
    /// تُحفظ بإشارتها (خصم = سالب، كما في الحركتين <c>000008</c>/<c>000014</c>)، وبقية الأنواع
    /// موجبة لأن الاتجاه يقرأه المستخدم من نوع الحركة (كما في حركات البيع المزروعة).
    /// </summary>
    private static decimal SignedQuantity(decimal change, InventoryTransactionType type)
        => KeepsSignedQuantity(type) ? change : Math.Abs(change);

    /// <summary>
    /// هل نوع الحركة يحفظ إشارة الكمية بنفسه؟ (الجرد والتسوية فقط)
    /// <para>
    /// <b>بيت واحد لقراءة الاتجاه (RULE-N169 / RULE-N171):</b> تُستهلك هنا وفي مولّد حركات
    /// التقارير — فلا تُكتب خريطة «نوع ← اتجاه» مرة ثانية في أي ملف.
    /// </para>
    /// </summary>
    internal static bool KeepsSignedQuantity(InventoryTransactionType type)
        => type is InventoryTransactionType.Adjustment or InventoryTransactionType.Count;

    /// <summary>هل الحركة تُخرج بضاعة من المخزن؟ (تُستخدم لاستنتاج الإشارة حين يحمل النوع اتجاهه).</summary>
    internal static bool IsOutbound(InventoryTransactionType type)
        => type is InventoryTransactionType.Sale
                or InventoryTransactionType.PurchaseReturn
                or InventoryTransactionType.TransferOut
                or InventoryTransactionType.Damage
                or InventoryTransactionType.InternalIssue;

    /// <summary>
    /// التغيّر الموقّع لسطر حركة <b>مخزَّن</b>: يحفظ الإشارة للجرد/التسوية، ويستنتجها من نوع
    /// الحركة لبقية الأنواع (هذا هو عكس <see cref="SignedQuantity"/> تماماً).
    /// </summary>
    internal static decimal SignedChangeOf(decimal storedQuantity, InventoryTransactionType type)
    {
        if (KeepsSignedQuantity(type))
            return storedQuantity;

        return IsOutbound(type) ? -Math.Abs(storedQuantity) : Math.Abs(storedQuantity);
    }

    /// <summary>نوع المستند المرجعي المكافئ لنوع الحركة — القراءة من تعداد النظام الواحد <see cref="InventoryReferenceType"/>.</summary>
    private static byte? ReferenceTypeOf(InventoryTransactionType type) => type switch
    {
        InventoryTransactionType.Purchase => (byte)InventoryReferenceType.PurchaseInvoice,
        InventoryTransactionType.Sale => (byte)InventoryReferenceType.SalesInvoice,
        InventoryTransactionType.PurchaseReturn => (byte)InventoryReferenceType.PurchaseReturn,
        InventoryTransactionType.SaleReturn => (byte)InventoryReferenceType.SalesReturn,
        InventoryTransactionType.TransferOut or InventoryTransactionType.TransferIn => (byte)InventoryReferenceType.Transfer,
        InventoryTransactionType.Count => (byte)InventoryReferenceType.Count,
        InventoryTransactionType.Adjustment => (byte)InventoryReferenceType.Adjustment,
        _ => null
    };

    /// <summary>المعرّف الرقمي للمستند من رقمه النصي — والنص يبقى كاملاً في البيان.</summary>
    private static int? ParseReferenceId(string? reference)
        => int.TryParse(reference?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var id)
            ? id
            : null;

    /// <summary>بيان الحركة: الصنف والاتجاه والرصيد الناتج ورقم المستند — بأرقام التنسيق الحيّة من <see cref="clsFormat"/>.</summary>
    private static string BuildNotes(string productName, decimal change, decimal newBalance, string? reference)
    {
        string directionText = change > 0m ? "إضافة" : "خصم";
        string referenceText = string.IsNullOrWhiteSpace(reference) ? string.Empty : $" · المستند {reference.Trim()}";

        return $"{productName} — {directionText} {clsFormat.Quantity(Math.Abs(change))} · الرصيد بعد الحركة {clsFormat.Quantity(newBalance)}{referenceText}";
    }

    /// <summary>معرّف الحركة التالي — يواصل تسلسل الحركات المزروعة (1..14).</summary>
    private static int NextMovementId()
    {
        int max = 0;
        foreach (var movement in MockInventoryData.Movements)
            if (movement.Id > max)
                max = movement.Id;

        return max + 1;
    }

    /// <summary>معرّف سطر الحركة التالي — يواصل تسلسل سطور الحركات المزروعة عبر كل الحركات.</summary>
    private static int NextLineId()
    {
        int max = 0;
        foreach (var movement in MockInventoryData.Movements)
            foreach (var line in movement.Lines)
                if (line.Id > max)
                    max = line.Id;

        return max + 1;
    }

    /// <summary>يواصل تسلسل أرقام الحركات المزروعة (<c>000001</c>…) بصيغة ستة أرقام <c>D6</c>.</summary>
    private static string NextTransactionNo()
    {
        int max = 0;
        foreach (var movement in MockInventoryData.Movements)
            if (int.TryParse(movement.TransactionNo, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number) &&
                number > max)
                max = number;

        return (max + 1).ToString("D6", CultureInfo.InvariantCulture);
    }
}
