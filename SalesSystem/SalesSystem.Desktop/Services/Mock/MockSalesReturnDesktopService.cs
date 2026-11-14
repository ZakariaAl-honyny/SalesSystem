using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة مرتجعات المبيعات — تعيد بيانات من MockReturnsData.
/// <para>
/// <b>تكافؤ المحاكاة (RULE-N167):</b> ترفض ما سترفضه طبقة الأعمال في المرحلة ب —
/// كمية إرجاع غير موجبة · كمية إرجاع أكبر من الكمية المباعة (ناقص ما سبق إرجاعه)
/// — برسائل عربية عبر <see cref="Result.Failure"/>، مع بقاء رفض الترحيل/الإلغاء القائم.
/// </para>
/// <para>
/// <b>أثر المخزون:</b> الثابت الملزم — رصيد بيانات التجربة يعكس المرتجعات
/// <b>المرحّلة وغير الملغاة</b> فقط: حفظ المرتجع كمسودة (<c>Status = 1</c>) لا يعيد أي بضاعة،
/// و<see cref="PostAsync"/> هو ما يُعيد الكميات إلى المخزن، و<see cref="CancelAsync"/> يخصمها
/// مرة أخرى فقط إن كان المرتجع مرحّلاً.
/// </para>
/// </summary>
public class MockSalesReturnDesktopService : ISalesReturnDesktopService
{
    // ═══════════════════════════════════════════════
    // رسائل الرفض العربية — نص واحد لكل قاعدة (RULE-N169)
    // ═══════════════════════════════════════════════
    private const string ZeroQuantityMessage = "الكمية يجب أن تكون أكبر من صفر";
    private const string ReturnExceedsSoldMessage = "كمية الإرجاع أكبر من الكمية المباعة";

    /// <summary>نص واحد مشترك لأي رفض بسبب نقص المخزون (بيت الأثر الواحد — RULE-N169).</summary>
    private const string InsufficientStockMessage = MockStockLedger.ShortageMessage;

    private const string UnexpectedErrorMessage = "تعذر إتمام العملية في وضع التجربة. يرجى إعادة المحاولة.";

    public Task<Result<IReadOnlyList<SalesReturnDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        int? customerId = null, CancellationToken ct = default)
    {
        IEnumerable<SalesReturnDto> query = MockReturnsData.SalesReturns;

        if (customerId.HasValue)
            query = query.Where(r => r.CustomerId == customerId.Value);

        if (from.HasValue)
            query = query.Where(r => r.ReturnDate >= from.Value);

        if (to.HasValue)
            query = query.Where(r => r.ReturnDate <= to.Value.Date.AddDays(1).AddTicks(-1));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(r =>
                r.ReturnNo.ToString().Contains(s) ||
                (r.CustomerName != null && r.CustomerName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (r.ReturnReason != null && r.ReturnReason.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (r.Notes != null && r.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<SalesReturnDto>>.Success(
            query.OrderByDescending(r => r.ReturnDate).ThenByDescending(r => r.ReturnNo).ToList().AsReadOnly()));
    }

    public Task<Result<SalesReturnDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var item = MockReturnsData.SalesReturns.FirstOrDefault(r => r.Id == id);
        return item is null
            ? Task.FromResult(Result<SalesReturnDto>.Failure("مرتجع المبيعات غير موجود"))
            : Task.FromResult(Result<SalesReturnDto>.Success(item));
    }

    public Task<Result<SalesReturnDto>> CreateAsync(CreateSalesReturnRequest r, CancellationToken ct = default)
    {
        try
        {
            // ── تكافؤ المحاكاة: كل رفض طبقة الأعمال قبل أي كتابة (RULE-N167) ──
            string? validationError = ValidateReturn(r);
            if (validationError is not null)
                return Task.FromResult(Result<SalesReturnDto>.Failure(validationError));

            var nextId = MockReturnsData.SalesReturns.Count == 0
                ? 1
                : MockReturnsData.SalesReturns.Max(x => x.Id) + 1;

            var nextReturnNo = MockReturnsData.SalesReturns.Count == 0
                ? 1001
                : MockReturnsData.SalesReturns.Max(x => x.ReturnNo) + 1;

            var customerName = r.CustomerId.HasValue
                ? MockData.Customers.FirstOrDefault(c => c.Id == r.CustomerId.Value)?.Name ?? "عميل نقدي"
                : "عميل نقدي";

            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == r.WarehouseId);

            var items = (r.Items ?? new List<ReturnItemRequest>()).Select((line, idx) =>
            {
                var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "منتج غير معروف";
                decimal lineTotal = (line.Quantity * line.UnitPrice) - line.DiscountAmount;
                return new SalesReturnItemDto(
                    Id: idx + 1,
                    ProductId: line.ProductId,
                    ProductUnitId: line.ProductUnitId,
                    ProductName: productName,
                    Quantity: line.Quantity,
                    UnitPrice: line.UnitPrice,
                    DiscountAmount: line.DiscountAmount,
                    LineTotal: lineTotal,
                    Mode: line.Mode);
            }).ToList();

            decimal subTotal = items.Sum(i => i.LineTotal);

            var salesReturn = new SalesReturnDto(
                Id: nextId,
                ReturnNo: nextReturnNo,
                WarehouseId: r.WarehouseId,
                WarehouseName: warehouse?.Name ?? "مخزن غير معروف",
                CustomerId: r.CustomerId ?? 0,
                CustomerName: customerName,
                SalesInvoiceId: r.SalesInvoiceId,
                ReturnDate: r.ReturnDate ?? DateTime.Today,
                SubTotal: subTotal,
                TaxAmount: 0m,
                DiscountAmount: 0m,
                TotalAmount: subTotal,
                ReturnedDiscountAmount: 0m,
                ReturnedTaxAmount: 0m,
                ReturnedChargeAmount: 0m,
                TaxId: null,
                Notes: r.Notes,
                ReturnReason: r.ReturnReason,
                Status: 1,
                CashBoxId: null,
                CashBoxName: null,
                RefundAmount: 0m,
                Items: items.AsReadOnly(),
                BranchId: r.BranchId ?? 0);

            // TODO: Currency removed from SalesReturnDto — العملة تُحدد من إعدادات النظام.

            MockReturnsData.SalesReturns.Add(salesReturn);
            return Task.FromResult(Result<SalesReturnDto>.Success(salesReturn));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ مرتجع المبيعات (محاكاة)");
            return Task.FromResult(Result<SalesReturnDto>.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockReturnsData.SalesReturns.FirstOrDefault(r => r.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("مرتجع المبيعات غير موجود"));

            if (existing.Status == 2)
                return Task.FromResult(Result.Failure("المرتجع مرحّل بالفعل"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("لا يمكن ترحيل مرتجع ملغي"));

            var index = MockReturnsData.SalesReturns.IndexOf(existing);

            // الترحيل هو النقطة الوحيدة التي تُحرّك المخزون: بضاعة مرتجعة تعود إلى المخزن
            MockStockLedger.Apply(QuantitiesOf(existing), direction: +1);

            MockReturnsData.SalesReturns[index] = existing with { Status = 2, RefundAmount = existing.TotalAmount };
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر ترحيل مرتجع المبيعات (محاكاة)");
            return Task.FromResult(Result.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockReturnsData.SalesReturns.FirstOrDefault(r => r.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("مرتجع المبيعات غير موجود"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("المرتجع ملغي بالفعل"));

            var index = MockReturnsData.SalesReturns.IndexOf(existing);

            // الإلغاء يعكس الأثر مرة واحدة فقط على المرتجع المرحّل (البضاعة تخرج ثانية)؛
            // والمسودة لم تُدخل شيئاً فلا يُخصم لها شيء.
            if (existing.Status == 2)
            {
                var quantities = QuantitiesOf(existing);

                // تكافؤ الرفض: لا رصيد سالباً (CHECK (Quantity >= 0)) — يُفحص قبل أي حركة
                if (!MockStockLedger.HasSufficientStock(quantities))
                    return Task.FromResult(Result.Failure(InsufficientStockMessage));

                MockStockLedger.Apply(quantities, direction: -1);
            }

            MockReturnsData.SalesReturns[index] = existing with { Status = 3 };
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر إلغاء مرتجع المبيعات (محاكاة)");
            return Task.FromResult(Result.Failure(UnexpectedErrorMessage));
        }
    }

    // ═══════════════════════════════════════════════
    // التحقق — رفض واحد يسبق الكتابة (RULE-N167)
    // ═══════════════════════════════════════════════
    /// <summary>
    /// كمية الإرجاع يجب أن تكون &gt; صفر، وألا تتجاوز الكمية المباعة في الفاتورة المرجعية
    /// ناقص ما سبق إرجاعه منها (المرتجعات غير الملغاة فقط). يعيد نص الرفض أو <c>null</c>.
    /// </summary>
    private static string? ValidateReturn(CreateSalesReturnRequest r)
    {
        var items = r.Items ?? new List<ReturnItemRequest>();

        if (items.Any(line => line.Quantity <= 0m))
            return ZeroQuantityMessage;

        // مرتجع غير مرتبط بفاتورة بيع: لا كمية مباعة مرجعية لمقارنتها
        if (r.SalesInvoiceId is null)
            return null;

        var invoice = MockData.Invoices.FirstOrDefault(i => i.Id == r.SalesInvoiceId.Value);
        if (invoice is null)
            return null;

        var soldByProduct = ((IEnumerable<SalesInvoiceLineDto>?)invoice.Items ?? Array.Empty<SalesInvoiceLineDto>())
            .GroupBy(i => i.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

        var alreadyReturnedByProduct = MockReturnsData.SalesReturns
            .Where(x => x.SalesInvoiceId == r.SalesInvoiceId && x.Status != 3)
            .SelectMany(x => (IEnumerable<SalesReturnItemDto>?)x.Items ?? Array.Empty<SalesReturnItemDto>())
            .GroupBy(i => i.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

        foreach (var group in items.GroupBy(line => line.ProductId))
        {
            if (!soldByProduct.TryGetValue(group.Key, out var sold))
                return ReturnExceedsSoldMessage; // صنف لم يُبع في الفاتورة المرجعية

            decimal alreadyReturned = alreadyReturnedByProduct.TryGetValue(group.Key, out var qty) ? qty : 0m;

            if (group.Sum(line => line.Quantity) > sold - alreadyReturned)
                return ReturnExceedsSoldMessage;
        }

        return null;
    }

    /// <summary>مجاميع كميات هذا المرتجع لكل صنف — تُستهلك في الترحيل والإلغاء.</summary>
    private static Dictionary<int, decimal> QuantitiesOf(SalesReturnDto salesReturn)
        => MockStockLedger.QuantitiesByProduct(
            ((IEnumerable<SalesReturnItemDto>?)salesReturn.Items ?? Array.Empty<SalesReturnItemDto>())
                .Select(i => (i.ProductId, i.Quantity)));
}
