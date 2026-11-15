using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using InventoryTxType = SalesSystem.Common.Enums.InventoryTransactionType;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة محاضر الجرد الفعلي — تقرأ وتكتب في <see cref="MockInventoryData.Stocktakings"/>،
/// وكل أثر على الرصيد يمرّ عبر <see cref="MockStockLedger"/> وحده (RULE-N169 / RULE-N172).
/// </summary>
/// <remarks>
/// RULE-N167 — تكافؤ الرفض: محضر بلا أصناف · كمية مجرودة سالبة · ترحيل محضر مرحّل ·
/// إلغاء محضر ملغي (وإلغاء <b>المسودة مسموح</b> لأنه لا حركة رصيد لها) · عجز أو سحب أكبر من
/// الرصيد المتاح (<see cref="MockStockLedger.ShortageMessage"/>).
/// <para>
/// الثابت الملزم: المسودة لا تحرّك رصيداً، والترحيل يطبّق <b>الفرق فقط</b> (المجرود − الدفتري)
/// بإشارته، والإلغاء يعكس أثر <b>المرحّل</b> مرة واحدة (وإلغاء المسودة لا يعكس شيئاً). ولا حذف نهائي — الإلغاء حالة (RULE-004).
/// </para>
/// <para>
/// رصيد المحاكاة على مستوى الصنف (<c>ProductDto.CurrentStock</c>) لا على مستوى (صنف، مخزن) —
/// لأن <see cref="MockStockLedger"/> هو البيت الوحيد للرصيد، فلا تُنسخ هنا نسخة ثانية من الحساب.
/// </para>
/// </remarks>
public class MockStocktakingDesktopService : IStocktakingDesktopService
{
    public Task<Result<IReadOnlyList<StocktakingDto>>> GetAllAsync(bool includeCancelled = false, CancellationToken ct = default)
    {
        try
        {
            IEnumerable<StocktakingDto> query = MockInventoryData.Stocktakings;

            if (!includeCancelled)
                query = query.Where(s => s.Status != StocktakingStatus.Cancelled);

            return Task.FromResult(Result<IReadOnlyList<StocktakingDto>>.Success(
                query.OrderByDescending(s => s.Date).ToList().AsReadOnly()));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<IReadOnlyList<StocktakingDto>>.Failure("تعذر تحميل محاضر الجرد"));
        }
    }

    public Task<Result<StocktakingDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.Stocktakings.FirstOrDefault(s => s.Id == id);

            return existing is null
                ? Task.FromResult(Result<StocktakingDto>.Failure("محضر الجرد غير موجود"))
                : Task.FromResult(Result<StocktakingDto>.Success(existing));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<StocktakingDto>.Failure("تعذر تحميل محضر الجرد"));
        }
    }

    public Task<Result<StocktakingDto>> CreateAsync(CreateStocktakingRequest request, CancellationToken ct = default)
    {
        try
        {
            var requested = (request.Lines ?? Array.Empty<CreateStocktakingLineRequest>())
                .Select(l => (l.ProductId, l.CountedQuantity))
                .ToList();

            // RULE-N167 — التحقق **قبل** أي إنشاء أو كتابة.
            var refusal = _Validate(requested);
            if (refusal is not null)
                return Task.FromResult(Result<StocktakingDto>.Failure(refusal));

            var nextId = MockInventoryData.Stocktakings.Select(s => s.Id).DefaultIfEmpty(0).Max() + 1;
            var nextLineId = MockInventoryData.Stocktakings
                .SelectMany(s => s.Lines)
                .Select(l => l.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == request.WarehouseId);

            var lines = new List<StocktakingLineDto>(requested.Count);
            foreach (var line in request.Lines ?? Array.Empty<CreateStocktakingLineRequest>())
            {
                lines.Add(new StocktakingLineDto(
                    Id: nextLineId++,
                    StocktakingId: nextId,
                    ProductId: line.ProductId,
                    ProductName: _ProductNameOf(line.ProductId),
                    Barcode: _BarcodeOf(line.ProductId),
                    BookQuantity: line.BookQuantity,
                    CountedQuantity: line.CountedQuantity));
            }

            var document = new StocktakingDto(
                Id: nextId,
                DocumentNo: $"STK-2026-{nextId:D3}",
                WarehouseId: request.WarehouseId,
                WarehouseName: warehouse?.Name ?? $"مستودع {request.WarehouseId}",
                Date: request.Date,
                Notes: request.Notes,
                CommitteeHead: request.CommitteeHead,
                CommitteeMember: request.CommitteeMember,
                Storekeeper: request.Storekeeper,
                Status: StocktakingStatus.Draft,
                StatusName: "مسودة",
                CreatedAt: DateTime.Now,
                PostedAt: null,
                CancelledAt: null,
                Lines: lines);

            MockInventoryData.Stocktakings.Add(document);
            return Task.FromResult(Result<StocktakingDto>.Success(document));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<StocktakingDto>.Failure("تعذر حفظ مسودة محضر الجرد"));
        }
    }

    public Task<Result<StocktakingDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.Stocktakings.FirstOrDefault(s => s.Id == id);
            if (existing is null)
                return Task.FromResult(Result<StocktakingDto>.Failure("محضر الجرد غير موجود"));

            if (existing.Status == StocktakingStatus.Posted)
                return Task.FromResult(Result<StocktakingDto>.Failure("لا يمكن ترحيل جرد مرحّل"));

            if (existing.Status == StocktakingStatus.Cancelled)
                return Task.FromResult(Result<StocktakingDto>.Failure("لا يمكن ترحيل جرد ملغي"));

            var lines = existing.Lines ?? Array.Empty<StocktakingLineDto>();

            var refusal = _Validate(lines.Select(l => (l.ProductId, l.CountedQuantity)).ToList());
            if (refusal is not null)
                return Task.FromResult(Result<StocktakingDto>.Failure(refusal));

            // كل الأثر (عجز/فائض) يُحسب ويُتحقق منه **قبل** أي كتابة على الرصيد — فلا حركة جزئية عند الرفض.
            var (inbound, outbound) = _StockEffectOf(lines);

            // RULE-N167 + §2.7 — العجز لا يتجاوز الرصيد المتاح.
            if (!MockStockLedger.HasSufficientStock(outbound))
                return Task.FromResult(Result<StocktakingDto>.Failure(MockStockLedger.ShortageMessage));

            // أثر الترحيل: العجز (المجرود < الدفتري) يُخصم −1، والفائض (المجرود > الدفتري) يُضاف +1.
            // الحركتان تحملان سياقهما الحقيقي: نوع «جرد» + مخزن المحضر + رقمه + تاريخه (RULE-N167)
            MockStockLedger.Apply(outbound,
                -1,
                type: InventoryTxType.Count,
                warehouseId: (short)existing.WarehouseId,
                reference: existing.DocumentNo,
                date: existing.Date);

            MockStockLedger.Apply(inbound,
                +1,
                type: InventoryTxType.Count,
                warehouseId: (short)existing.WarehouseId,
                reference: existing.DocumentNo,
                date: existing.Date);

            var index = MockInventoryData.Stocktakings.IndexOf(existing);
            var posted = existing with
            {
                Status = StocktakingStatus.Posted,
                StatusName = "مرحّل",
                PostedAt = DateTime.Now
            };

            MockInventoryData.Stocktakings[index] = posted;
            return Task.FromResult(Result<StocktakingDto>.Success(posted));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<StocktakingDto>.Failure("تعذر ترحيل محضر الجرد"));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.Stocktakings.FirstOrDefault(s => s.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("محضر الجرد غير موجود"));

            // RULE-N167 — الإلغاء فعل على المستند لا على الرصيد: المسودة ليس لها حركة رصيد فتُلغى بلا عكس،
            // والمرحّل وحده يُعكس أثره، والملغاة سبق رفضها أعلاه. وهذا نفس مسار BLL المرجعي:
            // InventoryAdjustmentService.CancelAsync يعكس أثر «المرحّل» فقط ثم ينادي Cancel() الذي لا يشترط حالة،
            // وInventoryAdjustment.Cancel() لا يرفض إلا الملغاة — فلا يُرفض إلغاء مسودة.
            if (existing.Status == StocktakingStatus.Cancelled)
                return Task.FromResult(Result.Failure("محضر الجرد ملغي بالفعل"));

            if (existing.Status == StocktakingStatus.Posted)
            {
                var (inbound, outbound) = _StockEffectOf(existing.Lines ?? Array.Empty<StocktakingLineDto>());

                // عكس الأثر بعكس الإشارة: ما خُصم عند الترحيل يعود +1، وما أُضيف يُسحب −1.
                // §2.7 — سحب الفائض لا يجعل الرصيد سالباً، ويُتحقق قبل أي كتابة.
                if (!MockStockLedger.HasSufficientStock(inbound))
                    return Task.FromResult(Result.Failure(MockStockLedger.ShortageMessage));

                MockStockLedger.Apply(outbound,
                    +1,
                    type: InventoryTxType.Count,
                    warehouseId: (short)existing.WarehouseId,
                    reference: existing.DocumentNo,
                    date: existing.Date);

                MockStockLedger.Apply(inbound,
                    -1,
                    type: InventoryTxType.Count,
                    warehouseId: (short)existing.WarehouseId,
                    reference: existing.DocumentNo,
                    date: existing.Date);
            }

            var index = MockInventoryData.Stocktakings.IndexOf(existing);
            MockInventoryData.Stocktakings[index] = existing with
            {
                Status = StocktakingStatus.Cancelled,
                StatusName = "ملغي",
                CancelledAt = DateTime.Now
            };

            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure("تعذر إلغاء محضر الجرد"));
        }
    }

    /// <summary>
    /// RULE-N167 / RULE-N172 — قاعدة رفض واحدة يستهلكها <c>CreateAsync</c> و<c>PostAsync</c>:
    /// بلا أصناف · كمية مجرودة سالبة. تُعيد نص الرفض أو <c>null</c> عند القبول.
    /// </summary>
    private static string? _Validate(IReadOnlyCollection<(int ProductId, decimal CountedQuantity)> lines)
    {
        if (lines.Count == 0)
            return "لا يمكن حفظ الجرد بدون أصناف";

        // المجرود قد يكون صفراً (صنف نُفد فعلاً) لكن لا يكون سالباً أبداً.
        if (lines.Any(l => l.CountedQuantity < 0m))
            return "الكمية المجرودة يجب أن تكون أكبر من صفر أو تساويها";

        return null;
    }

    /// <summary>
    /// RULE-N172 — أثر المحضر على الرصيد في مكان واحد، يستهلكه <c>PostAsync</c> و<c>CancelAsync</c>:
    /// الإشارة = المجرود − الدفتري (موجب ⇒ داخل · سالب ⇒ خارج)، والتجميع عبر
    /// <see cref="MockStockLedger.QuantitiesByProduct"/> — لا تجميع ثانٍ هنا (RULE-N169).
    /// </summary>
    private static (Dictionary<int, decimal> Inbound, Dictionary<int, decimal> Outbound) _StockEffectOf(
        IReadOnlyList<StocktakingLineDto> lines)
    {
        var inbound = new List<(int ProductId, decimal Quantity)>();
        var outbound = new List<(int ProductId, decimal Quantity)>();

        foreach (var line in lines)
        {
            if (line.ProductId <= 0) continue;

            decimal signedQuantity = line.CountedQuantity - line.BookQuantity;

            if (signedQuantity > 0m)
                inbound.Add((line.ProductId, signedQuantity));
            else if (signedQuantity < 0m)
                outbound.Add((line.ProductId, -signedQuantity));
        }

        return (MockStockLedger.QuantitiesByProduct(inbound), MockStockLedger.QuantitiesByProduct(outbound));
    }

    /// <summary>اسم الصنف من بيانات التجربة (السطر يعرض الاسم لا المعرّف).</summary>
    private static string _ProductNameOf(int productId)
        => MockData.Products.FirstOrDefault(p => p.Id == productId)?.Name ?? $"صنف {productId}";

    /// <summary>باركود الصنف = باركود أول وحدة له تحمل باركوداً (وإلا فراغ).</summary>
    private static string? _BarcodeOf(int productId)
        => MockData.ProductUnits.FirstOrDefault(pu => pu.ProductId == productId && pu.Barcode != null)?.Barcode;
}
