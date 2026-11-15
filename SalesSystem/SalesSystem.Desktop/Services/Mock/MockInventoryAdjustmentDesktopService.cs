using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة تسويات المخزون — تعيد بيانات من MockInventoryData.
/// </summary>
/// <remarks>
/// RULE-N167 — تكافؤ الرفض: تسوية بلا أصناف · بند بكمية صفر أو سالبة · خصم يتجاوز الرصيد المتاح ·
/// إلغاء تسوية مرحّلة سبق بيع كميتها المضافة (سحب لا يجعل الرصيد سالباً — <see cref="MockStockLedger.ShortageMessage"/>).
/// «الرصيد المتاح» في وضع التجربة = مجموع <c>QuantityRemaining</c> لدفعات الصنف في مخزن التسوية
/// (<c>MockInventoryData.Batches</c>) — وهو البديل الوحيد لجدول <c>WarehouseStocks</c> في هذه الطبقة.
/// </remarks>
public class MockInventoryAdjustmentDesktopService : IInventoryAdjustmentDesktopService
{
    /// <summary>نمط التسوية «إضافة» (AdjustmentType).</summary>
    private const byte AdditionType = 1;

    /// <summary>نمط التسوية «خصم» (AdjustmentType).</summary>
    private const byte DeductionType = 2;

    public Task<Result<IReadOnlyList<InventoryAdjustmentDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<InventoryAdjustmentDto> query = MockInventoryData.Adjustments;

        if (!includeInactive)
            query = query.Where(a => a.Status != 3); // Exclude Cancelled

        return Task.FromResult(Result<IReadOnlyList<InventoryAdjustmentDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<InventoryAdjustmentDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var adj = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
        return adj is null
            ? Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تسوية المخزون غير موجودة"))
            : Task.FromResult(Result<InventoryAdjustmentDto>.Success(adj));
    }

    public Task<Result<InventoryAdjustmentDto>> CreateAsync(CreateInventoryAdjustmentRequest request, CancellationToken ct = default)
    {
        try
        {
            var nextId = MockInventoryData.Adjustments.Select(a => a.Id).DefaultIfEmpty(0).Max() + 1;
            var adjNo = $"ADJ-2026-{nextId:D3}";
            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == request.WarehouseId);

            string adjTypeName = request.AdjustmentType switch
            {
                1 => "إضافة",
                2 => "خصم",
                3 => "تصحيح",
                _ => "غير محدد"
            };

            var adj = new InventoryAdjustmentDto(
                Id: nextId,
                AdjustmentNo: adjNo,
                WarehouseId: request.WarehouseId,
                WarehouseName: warehouse?.Name ?? $"مستودع {request.WarehouseId}",
                AdjustmentType: request.AdjustmentType,
                AdjustmentTypeName: adjTypeName,
                Reason: request.Reason,
                Status: 1,
                StatusName: "مسودة",
                CreatedAt: DateTime.Now,
                CreatedByUserId: 1,
                PostedAt: null,
                CancelledAt: null,
                Lines: new List<InventoryAdjustmentLineDto>());
            // TODO: BranchId / BranchName removed from InventoryAdjustmentDto.

            // RULE-N167: رأس التسوية يُنشأ بلا بنود (البنود تُضاف بعد ذلك عبر AddLineAsync)،
            // لذلك لا يُرفض «الحفظ بلا أصناف» هنا — الرفض عند الترحيل في PostAsync.
            MockInventoryData.Adjustments.Add(adj);
            return Task.FromResult(Result<InventoryAdjustmentDto>.Success(adj));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تعذر حفظ مسودة تسوية المخزون"));
        }
    }

    public Task<Result<InventoryAdjustmentDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
            if (existing is null)
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تسوية المخزون غير موجودة"));

            if (existing.Status != 1)
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("فقط التسويات المسودة يمكن ترحيلها"));

            var lines = existing.Lines ?? new List<InventoryAdjustmentLineDto>();

            // RULE-N167 — لا تُحفظ/تُرحَّل تسوية بلا أصناف.
            if (lines.Count == 0)
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("لا يمكن حفظ التسوية بدون أصناف"));

            // RULE-N167 — كمية كل بند أكبر من صفر (ولا تُقبل كمية سالبة).
            if (lines.Any(l => _EffectiveQuantity(existing.AdjustmentType, l.ExpectedQuantity, l.ActualQuantity) <= 0m))
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("الكمية يجب أن تكون أكبر من صفر"));

            // RULE-N167 + §2.7 — لا خصم يتجاوز الرصيد المتاح في مخزن التسوية.
            // كل الأثر (خارج/داخل) يُحسب ويُتحقق منه **قبل** أي كتابة على الرصيد — فلا حركة جزئية عند الرفض.
            var (inboundQuantities, outboundQuantities) = _StockEffectOf(existing);

            if (_HasStockShortage(existing))
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("الكمية المخصومة أكبر من الرصيد المتاح"));

            // RULE-N167 + §2.7 — تحقّق ثانٍ من رصيد الصنف نفسه قبل الكتابة (لا رصيد سالب).
            if (!MockStockLedger.HasSufficientStock(outboundQuantities))
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("الكمية المخصومة أكبر من الرصيد المتاح"));

            var index = MockInventoryData.Adjustments.IndexOf(existing);
            var posted = existing with
            {
                Status = 2,
                StatusName = "مرحّل",
                PostedAt = DateTime.Now
            };

            // الأثر المخزني: MockStockLedger هو البيت الوحيد الذي يكتب CurrentStock (RULE-N171) —
            // خارج = −1 (خصم) ثم داخل = +1 (إضافة)، والسطور المتوازنة في التسوية الواحدة تُطبَّق مرة واحدة.
            MockStockLedger.Apply(outboundQuantities, -1);
            MockStockLedger.Apply(inboundQuantities, +1);

            MockInventoryData.Adjustments[index] = posted;
            return Task.FromResult(Result<InventoryAdjustmentDto>.Success(posted));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تعذر ترحيل تسوية المخزون"));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("تسوية المخزون غير موجودة"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("التسوية ملغاة بالفعل"));

            // RULE-N167 + RULE-004 — عكس أثر المخزون للمرحّل فقط: المسودة لم تحرّك رصيداً فلا شيء يُعكس،
            // والملغاة سبق رفضها أعلاه. ولا حذف نهائي — الإلغاء حالة لا حذف.
            if (existing.Status == 2)
            {
                var (inboundQuantities, outboundQuantities) = _StockEffectOf(existing);

                // RULE-N167 + §2.7 — سحب ما سبق إضافته عند الترحيل لا يجعل الرصيد سالباً إن بيعت الكمية بعده؛
                // والتحقق **قبل** أي كتابة على الرصيد (لا حركة جزئية عند الرفض)، بنفس نص الرفض الموحّد.
                if (!MockStockLedger.HasSufficientStock(inboundQuantities))
                    return Task.FromResult(Result.Failure(MockStockLedger.ShortageMessage));

                // عكس الإشارة: ما خُصم عند الترحيل يعود +1، وما أُضيف يُسحب −1 (MockStockLedger وحده يكتب الرصيد).
                MockStockLedger.Apply(outboundQuantities, +1);
                MockStockLedger.Apply(inboundQuantities, -1);
            }

            var index = MockInventoryData.Adjustments.IndexOf(existing);
            MockInventoryData.Adjustments[index] = existing with
            {
                Status = 3,
                StatusName = "ملغي",
                CancelledAt = DateTime.Now
            };
            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure("تعذر إلغاء تسوية المخزون"));
        }
    }

    public Task<Result<InventoryAdjustmentDto>> AddLineAsync(int id, AddInventoryAdjustmentLineRequest request, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.Adjustments.FirstOrDefault(a => a.Id == id);
            if (existing is null)
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تسوية المخزون غير موجودة"));

            if (existing.Status != 1)
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("لا يمكن إضافة أصناف لتسوية غير مسودة"));

            // RULE-N167 — كمية البند أكبر من صفر (ولا تُقبل كمية سالبة) حسب نوع التسوية.
            if (_EffectiveQuantity(existing.AdjustmentType, request.ExpectedQuantity, request.ActualQuantity) <= 0m)
                return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("الكمية يجب أن تكون أكبر من صفر"));

            var nextLineId = MockInventoryData.Adjustments
                .SelectMany(a => a.Lines ?? new List<InventoryAdjustmentLineDto>())
                .Select(l => l.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            var productUnit = MockData.ProductUnits.FirstOrDefault(pu => pu.Id == request.ProductUnitId);
            var productName = MockData.Products.FirstOrDefault(p => p.Id == productUnit?.ProductId)?.Name;

            var line = new InventoryAdjustmentLineDto(
                Id: nextLineId,
                InventoryAdjustmentId: id,
                ProductUnitId: request.ProductUnitId,
                ProductUnitName: $"{productName} — {productUnit?.UnitName ?? "وحدة"}",
                ExpectedQuantity: request.ExpectedQuantity,
                ActualQuantity: request.ActualQuantity,
                UnitCost: request.UnitCost);
            // TODO: ProductName / ProductId / UnitName removed from InventoryAdjustmentLineDto.

            var lines = existing.Lines is { Count: > 0 }
                ? new List<InventoryAdjustmentLineDto>(existing.Lines) { line }
                : new List<InventoryAdjustmentLineDto> { line };

            var index = MockInventoryData.Adjustments.IndexOf(existing);
            MockInventoryData.Adjustments[index] = existing with { Lines = lines };

            return Task.FromResult(Result<InventoryAdjustmentDto>.Success(MockInventoryData.Adjustments[index]));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<InventoryAdjustmentDto>.Failure("تعذر إضافة صنف إلى تسوية المخزون"));
        }
    }

    /// <summary>
    /// RULE-N167 — الكمية الفعّالة للبند حسب نوع التسوية:
    /// خصم = المتوقع − الفعلي · إضافة = الفعلي − المتوقع · تصحيح = الفرق المطلق.
    /// </summary>
    private static decimal _EffectiveQuantity(byte adjustmentType, decimal expectedQuantity, decimal actualQuantity)
        => adjustmentType switch
        {
            DeductionType => expectedQuantity - actualQuantity,
            AdditionType => actualQuantity - expectedQuantity,
            _ => Math.Abs(actualQuantity - expectedQuantity)
        };

    /// <summary>
    /// RULE-N172 — أثر التسوية على الرصيد في مكان واحد، يستهلكه <c>PostAsync</c> و<c>CancelAsync</c>:
    /// قاموس «داخل» (زيادة) وقاموس «خارج» (نقص) لكل صنف. الإشارة: إضافة = + · خصم = − ·
    /// تصحيح = إشارة الفرق (الفعلي − المتوقع). والتجميع عبر
    /// <see cref="MockStockLedger.QuantitiesByProduct"/> — لا تجميع ثانٍ هنا (RULE-N169).
    /// </summary>
    private static (Dictionary<int, decimal> Inbound, Dictionary<int, decimal> Outbound) _StockEffectOf(
        InventoryAdjustmentDto adjustment)
    {
        var inbound = new List<(int ProductId, decimal Quantity)>();
        var outbound = new List<(int ProductId, decimal Quantity)>();

        foreach (var line in adjustment.Lines ?? new List<InventoryAdjustmentLineDto>())
        {
            if (_ProductIdOf(line.ProductUnitId) is not int productId) continue;

            decimal signedQuantity = adjustment.AdjustmentType switch
            {
                AdditionType => _EffectiveQuantity(AdditionType, line.ExpectedQuantity, line.ActualQuantity),
                DeductionType => -_EffectiveQuantity(DeductionType, line.ExpectedQuantity, line.ActualQuantity),
                _ => line.ActualQuantity - line.ExpectedQuantity
            };

            if (signedQuantity > 0m)
                inbound.Add((productId, signedQuantity));
            else if (signedQuantity < 0m)
                outbound.Add((productId, -signedQuantity));
        }

        return (MockStockLedger.QuantitiesByProduct(inbound), MockStockLedger.QuantitiesByProduct(outbound));
    }

    /// <summary>
    /// RULE-N167 — هل يتجاوز مجموع النقص لكل صنف (خصم · أو تصحيح خافض) رصيده المتاح في مخزن التسوية؟
    /// </summary>
    private static bool _HasStockShortage(InventoryAdjustmentDto adjustment)
    {
        var (_, outbound) = _StockEffectOf(adjustment);

        return outbound.Any(pair => pair.Value > _AvailableStock(pair.Key, adjustment.WarehouseId));
    }

    /// <summary>معرّف الصنف من معرّف وحدة الصنف (البنود تحمل وحدة لا صنفاً).</summary>
    private static int? _ProductIdOf(int productUnitId)
        => MockData.ProductUnits.FirstOrDefault(pu => pu.Id == productUnitId)?.ProductId;

    /// <summary>الرصيد المتاح = مجموع الكمية المتبقية من دفعات الصنف في المخزن المحدد.</summary>
    private static decimal _AvailableStock(int productId, short warehouseId)
        => MockInventoryData.Batches
            .Where(b => b.ProductId == productId && b.WarehouseId == warehouseId)
            .Sum(b => b.QuantityRemaining);
}
