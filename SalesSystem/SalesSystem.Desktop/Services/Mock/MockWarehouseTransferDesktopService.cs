using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using InventoryTxType = SalesSystem.Common.Enums.InventoryTransactionType;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة سندات التحويل المخزني — تقرأ وتكتب في <see cref="MockInventoryData.WarehouseTransfers"/>،
/// وكل أثر على الرصيد يمرّ عبر <see cref="MockStockLedger"/> وحده (RULE-N169 / RULE-N172).
/// </summary>
/// <remarks>
/// RULE-N167 — تكافؤ الرفض: سند بلا أصناف · كمية صفر أو سالبة · مصدر = هدف ·
/// ترحيل سند مرحّل · إلغاء سند غير مرحّل · نقص رصيد المصدر (<see cref="MockStockLedger.ShortageMessage"/>).
/// <para>
/// الثابت الملزم: الحفظ كمسودة لا يحرّك رصيداً، والترحيل يخفض المصدر ويرفع الهدف مرة واحدة،
/// والإلغاء يعكس الأثر مرة واحدة. ولا حذف نهائي — الإلغاء حالة (RULE-004).
/// </para>
/// <para>
/// رصيد المحاكاة على مستوى الصنف (<c>ProductDto.CurrentStock</c>) لا على مستوى (صنف، مخزن) —
/// لأن <see cref="MockStockLedger"/> هو البيت الوحيد للرصيد، فلا تُنسخ هنا نسخة ثانية من الحساب
/// (وفي المرحلة ب يُفصل الرصيد لكل مخزن في <c>WarehouseStocks</c>).
/// </para>
/// </remarks>
public class MockWarehouseTransferDesktopService : IWarehouseTransferDesktopService
{
    public Task<Result<IReadOnlyList<WarehouseTransferDto>>> GetAllAsync(bool includeCancelled = false, CancellationToken ct = default)
    {
        try
        {
            IEnumerable<WarehouseTransferDto> query = MockInventoryData.WarehouseTransfers;

            if (!includeCancelled)
                query = query.Where(t => t.Status != WarehouseTransferStatus.Cancelled);

            return Task.FromResult(Result<IReadOnlyList<WarehouseTransferDto>>.Success(
                query.OrderByDescending(t => t.Date).ToList().AsReadOnly()));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<IReadOnlyList<WarehouseTransferDto>>.Failure("تعذر تحميل سندات التحويل المخزني"));
        }
    }

    public Task<Result<WarehouseTransferDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.WarehouseTransfers.FirstOrDefault(t => t.Id == id);

            return existing is null
                ? Task.FromResult(Result<WarehouseTransferDto>.Failure("سند التحويل غير موجود"))
                : Task.FromResult(Result<WarehouseTransferDto>.Success(existing));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<WarehouseTransferDto>.Failure("تعذر تحميل سند التحويل"));
        }
    }

    public Task<Result<WarehouseTransferDto>> CreateAsync(CreateWarehouseTransferRequest request, CancellationToken ct = default)
    {
        try
        {
            var requested = (request.Lines ?? Array.Empty<CreateWarehouseTransferLineRequest>())
                .Select(l => (l.ProductId, l.Quantity))
                .ToList();

            // RULE-N167 — التحقق **قبل** أي إنشاء أو كتابة.
            var refusal = _Validate(request.FromWarehouseId, request.ToWarehouseId, requested);
            if (refusal is not null)
                return Task.FromResult(Result<WarehouseTransferDto>.Failure(refusal));

            var nextId = MockInventoryData.WarehouseTransfers.Select(t => t.Id).DefaultIfEmpty(0).Max() + 1;
            var nextLineId = MockInventoryData.WarehouseTransfers
                .SelectMany(t => t.Lines)
                .Select(l => l.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            var from = MockData.Warehouses.FirstOrDefault(w => w.Id == request.FromWarehouseId);
            var to = MockData.Warehouses.FirstOrDefault(w => w.Id == request.ToWarehouseId);

            var lines = new List<WarehouseTransferLineDto>(requested.Count);
            foreach (var line in request.Lines ?? Array.Empty<CreateWarehouseTransferLineRequest>())
            {
                lines.Add(new WarehouseTransferLineDto(
                    Id: nextLineId++,
                    WarehouseTransferId: nextId,
                    ProductId: line.ProductId,
                    ProductName: _ProductNameOf(line.ProductId),
                    Barcode: _BarcodeOf(line.ProductId),
                    BatchNo: line.BatchNo,
                    ExpiryDate: line.ExpiryDate,
                    Quantity: line.Quantity));
            }

            var document = new WarehouseTransferDto(
                Id: nextId,
                DocumentNo: $"TRF-2026-{nextId:D3}",
                FromWarehouseId: request.FromWarehouseId,
                FromWarehouseName: from?.Name ?? $"مستودع {request.FromWarehouseId}",
                ToWarehouseId: request.ToWarehouseId,
                ToWarehouseName: to?.Name ?? $"مستودع {request.ToWarehouseId}",
                Date: request.Date,
                Notes: request.Notes,
                Status: WarehouseTransferStatus.Draft,
                StatusName: "مسودة",
                CreatedAt: DateTime.Now,
                PostedAt: null,
                CancelledAt: null,
                Lines: lines);

            MockInventoryData.WarehouseTransfers.Add(document);
            return Task.FromResult(Result<WarehouseTransferDto>.Success(document));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<WarehouseTransferDto>.Failure("تعذر حفظ مسودة سند التحويل"));
        }
    }

    public Task<Result<WarehouseTransferDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.WarehouseTransfers.FirstOrDefault(t => t.Id == id);
            if (existing is null)
                return Task.FromResult(Result<WarehouseTransferDto>.Failure("سند التحويل غير موجود"));

            if (existing.Status == WarehouseTransferStatus.Posted)
                return Task.FromResult(Result<WarehouseTransferDto>.Failure("لا يمكن ترحيل تحويل مرحّل"));

            if (existing.Status == WarehouseTransferStatus.Cancelled)
                return Task.FromResult(Result<WarehouseTransferDto>.Failure("لا يمكن ترحيل تحويل ملغي"));

            var lines = existing.Lines ?? Array.Empty<WarehouseTransferLineDto>();

            var refusal = _Validate(existing.FromWarehouseId, existing.ToWarehouseId,
                lines.Select(l => (l.ProductId, l.Quantity)).ToList());
            if (refusal is not null)
                return Task.FromResult(Result<WarehouseTransferDto>.Failure(refusal));

            var quantities = MockStockLedger.QuantitiesByProduct(lines.Select(l => (l.ProductId, l.Quantity)));

            // RULE-N167 + §2.7 — رصيد المصدر يكفي، والتحقق **قبل** أي كتابة (لا حركة جزئية عند الرفض).
            if (!MockStockLedger.HasSufficientStock(quantities))
                return Task.FromResult(Result<WarehouseTransferDto>.Failure(MockStockLedger.ShortageMessage));

            // أثر الترحيل: المصدر −1 (صادر) ثم الهدف +1 (وارد) — MockStockLedger وحده يكتب الرصيد.
            // كل حركة على مخزنها الحقيقي ونوعها الحقيقي (لا تُسجَّل الحركتان على المخزن الأول):
            // المستدعيان يعطيان: صادر من «من مخزن» ووارد إلى «إلى مخزن» مع رقم السند وتاريخه.
            MockStockLedger.Apply(quantities,
                -1,
                type: InventoryTxType.TransferOut,
                warehouseId: (short)existing.FromWarehouseId,
                reference: existing.DocumentNo,
                date: existing.Date);

            MockStockLedger.Apply(quantities,
                +1,
                type: InventoryTxType.TransferIn,
                warehouseId: (short)existing.ToWarehouseId,
                reference: existing.DocumentNo,
                date: existing.Date);

            var index = MockInventoryData.WarehouseTransfers.IndexOf(existing);
            var posted = existing with
            {
                Status = WarehouseTransferStatus.Posted,
                StatusName = "مرحّل",
                PostedAt = DateTime.Now
            };

            MockInventoryData.WarehouseTransfers[index] = posted;
            return Task.FromResult(Result<WarehouseTransferDto>.Success(posted));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<WarehouseTransferDto>.Failure("تعذر ترحيل سند التحويل"));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockInventoryData.WarehouseTransfers.FirstOrDefault(t => t.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("سند التحويل غير موجود"));

            // RULE-N167 — الإلغاء للمرحّل فقط (المسودة لا تحرّك رصيداً فلا شيء يُلغى).
            if (existing.Status != WarehouseTransferStatus.Posted)
                return Task.FromResult(Result.Failure("لا يمكن إلغاء تحويل غير مرحّل"));

            var quantities = MockStockLedger.QuantitiesByProduct(
                (existing.Lines ?? Array.Empty<WarehouseTransferLineDto>()).Select(l => (l.ProductId, l.Quantity)));

            // عكس الأثر: ما خُصم من المصدر يعود +1 (وارد إلى المصدر)، وما أُضيف إلى الهدف يُسحب −1
            // (صادر من الهدف) — كل حركة على مخزنها الحقيقي وبالمعنى المعاكس لنوعها.
            // لا حارس نقص هنا لأن العكس متوازن (الصافي صفر على رصيد الصنف) فلا يمكن أن يجعله سالباً —
            // بخلاف إلغاء الجرد الذي يسحب فائضاً أُضيف فعلاً فيحتاج حارساً (§2.7).
            MockStockLedger.Apply(quantities,
                +1,
                type: InventoryTxType.TransferIn,
                warehouseId: (short)existing.FromWarehouseId,
                reference: existing.DocumentNo,
                date: existing.Date);

            MockStockLedger.Apply(quantities,
                -1,
                type: InventoryTxType.TransferOut,
                warehouseId: (short)existing.ToWarehouseId,
                reference: existing.DocumentNo,
                date: existing.Date);

            var index = MockInventoryData.WarehouseTransfers.IndexOf(existing);
            MockInventoryData.WarehouseTransfers[index] = existing with
            {
                Status = WarehouseTransferStatus.Cancelled,
                StatusName = "ملغي",
                CancelledAt = DateTime.Now
            };

            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure("تعذر إلغاء سند التحويل"));
        }
    }

    /// <summary>
    /// RULE-N167 / RULE-N172 — قاعدة رفض واحدة يستهلكها <c>CreateAsync</c> و<c>PostAsync</c>:
    /// بلا أصناف · كمية صفر أو سالبة · المخزن المصدر = المخزن الهدف.
    /// تُعيد نص الرفض أو <c>null</c> عند القبول.
    /// </summary>
    private static string? _Validate(int fromWarehouseId, int toWarehouseId,
        IReadOnlyCollection<(int ProductId, decimal Quantity)> lines)
    {
        if (lines.Count == 0)
            return "لا يمكن حفظ التحويل بدون أصناف";

        if (lines.Any(l => l.Quantity <= 0m))
            return "الكمية يجب أن تكون أكبر من صفر";

        if (fromWarehouseId == toWarehouseId)
            return "المخزن المصدر والمخزن الهدف يجب أن يكونا مختلفين";

        return null;
    }

    /// <summary>اسم الصنف من بيانات التجربة (السطر يعرض الاسم لا المعرّف).</summary>
    private static string _ProductNameOf(int productId)
        => MockData.Products.FirstOrDefault(p => p.Id == productId)?.Name ?? $"صنف {productId}";

    /// <summary>باركود الصنف = باركود أول وحدة له تحمل باركوداً (وإلا فراغ).</summary>
    private static string? _BarcodeOf(int productId)
        => MockData.ProductUnits.FirstOrDefault(pu => pu.ProductId == productId && pu.Barcode != null)?.Barcode;
}
