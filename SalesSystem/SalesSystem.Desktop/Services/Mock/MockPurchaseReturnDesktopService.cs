using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Enums;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة مرتجعات المشتريات — تعيد بيانات من MockReturnsData.
/// <para>
/// <b>أثر المخزون (RULE-N167):</b> الثابت الملزم — رصيد بيانات التجربة يعكس المرتجعات
/// <b>المرحّلة وغير الملغاة</b> فقط: حفظ المرتجع كمسودة (<c>Status = 1</c>) لا يخصم أي رصيد،
/// و<see cref="PostAsync"/> هو ما يُخرج الكميات من المخزن (ولا يسمح برصيد سالب)،
/// و<see cref="CancelAsync"/> يعيدها مرة واحدة فقط إن كان المرتجع مرحّلاً.
/// </para>
/// </summary>
public class MockPurchaseReturnDesktopService : IPurchaseReturnDesktopService
{
    // RULE-N167 — نصوص الرفض العربية في مكان واحد داخل الملف (RULE-N169).
    private const string NotFoundError = "مرتجع المشتريات غير موجود";
    private const string ZeroQuantityError = "الكمية يجب أن تكون أكبر من صفر";
    private const string OverPurchasedError = "كمية الإرجاع أكبر من الكمية المشتراة";

    /// <summary>نص واحد مشترك لأي رفض بسبب نقص المخزون (بيت الأثر الواحد — RULE-N169).</summary>
    private const string InsufficientStockMessage = MockStockLedger.ShortageMessage;

    private const string UnexpectedError = "تعذر تنفيذ العملية على مرتجع المشتريات. يرجى إعادة المحاولة.";

    public Task<Result<IReadOnlyList<PurchaseReturnDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        int? supplierId = null, CancellationToken ct = default)
    {
        try
        {
            IEnumerable<PurchaseReturnDto> query = MockReturnsData.PurchaseReturns;

            if (supplierId.HasValue)
                query = query.Where(r => r.SupplierId == supplierId.Value);

            if (from.HasValue)
                query = query.Where(r => r.ReturnDate >= DateOnly.FromDateTime(from.Value));

            if (to.HasValue)
                query = query.Where(r => r.ReturnDate <= DateOnly.FromDateTime(to.Value));

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                query = query.Where(r =>
                    r.ReturnNo.ToString().Contains(s) ||
                    (r.SupplierName != null && r.SupplierName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                    (r.Notes != null && r.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
            }

            return Task.FromResult(Result<IReadOnlyList<PurchaseReturnDto>>.Success(
                query.OrderByDescending(r => r.ReturnDate).ThenByDescending(r => r.ReturnNo).ToList().AsReadOnly()));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<IReadOnlyList<PurchaseReturnDto>>.Failure(UnexpectedError));
        }
    }

    public Task<Result<PurchaseReturnDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var item = MockReturnsData.PurchaseReturns.FirstOrDefault(r => r.Id == id);
            return item is null
                ? Task.FromResult(Result<PurchaseReturnDto>.Failure(NotFoundError))
                : Task.FromResult(Result<PurchaseReturnDto>.Success(item));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<PurchaseReturnDto>.Failure(UnexpectedError));
        }
    }

    public Task<Result<PurchaseReturnDto>> CreateAsync(CreatePurchaseReturnRequest r, CancellationToken ct = default)
    {
        try
        {
            // RULE-N167 — الرفض نفسه الذي سيرفضه الباك-إند: كمية صفر/سالبة · إرجاع أكبر من المشترى.
            foreach (var line in r.Items)
            {
                if (line.Quantity <= 0m)
                    return Task.FromResult(Result<PurchaseReturnDto>.Failure(ZeroQuantityError));

                var purchasedQuantity = PurchasedQuantityOf(line);
                if (purchasedQuantity <= 0m)
                    continue; // لا يوجد بند مشترى مرجعي في البيانات الأولية ⇒ لا مقارنة

                if (ReturnedQuantityOf(line) + line.Quantity > purchasedQuantity)
                    return Task.FromResult(Result<PurchaseReturnDto>.Failure(OverPurchasedError));
            }

            var nextId = MockReturnsData.PurchaseReturns.Count == 0
                ? 1
                : MockReturnsData.PurchaseReturns.Max(x => x.Id) + 1;

            var nextReturnNo = MockReturnsData.PurchaseReturns.Count == 0
                ? 2001
                : MockReturnsData.PurchaseReturns.Max(x => x.ReturnNo) + 1;

            var supplierName = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId)?.Name
                ?? "مورد غير معروف";

            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

            var items = r.Items.Select((line, idx) =>
            {
                var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "منتج غير معروف";
                return new PurchaseReturnItemDto(
                    Id: nextId * 100 + idx + 1,
                    ProductId: line.ProductId,
                    ProductName: productName,
                    ProductUnitId: line.ProductUnitId,
                    ProductUnitName: null,
                    Quantity: line.Quantity,
                    UnitCost: line.UnitCost,
                    LineTotal: line.Amount,
                    PurchaseInvoiceLineId: line.PurchaseInvoiceLineId > 0 ? line.PurchaseInvoiceLineId : null,
                    CostInBaseCurrency: null);
            }).ToList();

            decimal totalAmount = items.Sum(i => i.LineTotal);
            DateOnly returnDate = r.ReturnDate.HasValue
                ? DateOnly.FromDateTime(r.ReturnDate.Value)
                : DateOnly.FromDateTime(DateTime.Today);

            var purchaseReturn = new PurchaseReturnDto(
                Id: nextId,
                ReturnNo: nextReturnNo,
                WarehouseId: r.WarehouseId,
                WarehouseName: warehouse?.Name ?? "مخزن غير معروف",
                SupplierId: r.SupplierId,
                SupplierName: supplierName,
                PurchaseInvoiceId: r.PurchaseInvoiceId,
                LinkToInvoice: r.PurchaseInvoiceId.HasValue,
                ReturnDate: returnDate,
                SubTotal: totalAmount,
                TotalAmount: totalAmount,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                DiscountType: (byte)(r.DiscountType ?? DiscountType.Amount),
                DiscountRate: r.DiscountRate,
                Notes: r.Notes,
                Status: 1,
                Items: items.AsReadOnly(),
                BranchId: r.BranchId ?? 0);

            // TODO: Currency removed from PurchaseReturnDto — العملة تُحدد من إعدادات النظام.

            MockReturnsData.PurchaseReturns.Add(purchaseReturn);
            return Task.FromResult(Result<PurchaseReturnDto>.Success(purchaseReturn));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<PurchaseReturnDto>.Failure(UnexpectedError));
        }
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockReturnsData.PurchaseReturns.FirstOrDefault(r => r.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure(NotFoundError));

            if (existing.Status == 2)
                return Task.FromResult(Result.Failure("المرتجع مرحّل بالفعل"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("لا يمكن ترحيل مرتجع ملغي"));

            var index = MockReturnsData.PurchaseReturns.IndexOf(existing);

            // التحقق أولاً ثم الحركة: لا خصم جزئي — الترحيل هو النقطة الوحيدة التي تُخرج بضاعة
            var quantities = QuantitiesOf(existing);
            if (!MockStockLedger.HasSufficientStock(quantities))
                return Task.FromResult(Result.Failure(InsufficientStockMessage));

            MockStockLedger.Apply(quantities, direction: -1);

            MockReturnsData.PurchaseReturns[index] = existing with { Status = 2 };
            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure(UnexpectedError));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockReturnsData.PurchaseReturns.FirstOrDefault(r => r.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure(NotFoundError));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("المرتجع ملغي بالفعل"));

            var index = MockReturnsData.PurchaseReturns.IndexOf(existing);

            // الإلغاء يعكس الأثر مرة واحدة فقط على المرتجع المرحّل (البضاعة تعود إلى المخزن)؛
            // والمسودة لم تُخرج شيئاً فلا يُضاف لها شيء.
            if (existing.Status == 2)
                MockStockLedger.Apply(QuantitiesOf(existing), direction: +1);

            MockReturnsData.PurchaseReturns[index] = existing with { Status = 3 };
            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure(UnexpectedError));
        }
    }

    // ═══════════════════════════════════════════════
    // مساعدات المقارنة مع فواتير المشتريات (RULE-N167)
    // ═══════════════════════════════════════════════

    /// <summary>الكمية المشتراة: من بند الفاتورة المرجعي إن حُدِّد، وإلا لكل بنود الصنف في فواتير المشتريات.</summary>
    private static decimal PurchasedQuantityOf(CreatePurchaseReturnItemRequest line)
    {
        var purchaseLines = MockPurchasesData.Purchases.SelectMany(p => p.Items);

        return line.PurchaseInvoiceLineId > 0
            ? purchaseLines.Where(l => l.Id == line.PurchaseInvoiceLineId).Sum(l => l.Quantity)
            : purchaseLines.Where(l => l.ProductId == line.ProductId).Sum(l => l.Quantity);
    }

    /// <summary>الكمية المرتجعة سابقاً في المرتجعات غير الملغاة (3 = ملغي).</summary>
    private static decimal ReturnedQuantityOf(CreatePurchaseReturnItemRequest line)
    {
        var returnedItems = MockReturnsData.PurchaseReturns
            .Where(r => r.Status != 3)
            .SelectMany(r => r.Items);

        return line.PurchaseInvoiceLineId > 0
            ? returnedItems.Where(i => i.PurchaseInvoiceLineId == line.PurchaseInvoiceLineId).Sum(i => i.Quantity)
            : returnedItems.Where(i => i.ProductId == line.ProductId).Sum(i => i.Quantity);
    }

    /// <summary>مجاميع كميات هذا المرتجع لكل صنف — تُستهلك في الترحيل والإلغاء.</summary>
    private static Dictionary<int, decimal> QuantitiesOf(PurchaseReturnDto purchaseReturn)
        => MockStockLedger.QuantitiesByProduct(
            ((IEnumerable<PurchaseReturnItemDto>?)purchaseReturn.Items ?? Array.Empty<PurchaseReturnItemDto>())
                .Select(i => (i.ProductId, i.Quantity)));
}
