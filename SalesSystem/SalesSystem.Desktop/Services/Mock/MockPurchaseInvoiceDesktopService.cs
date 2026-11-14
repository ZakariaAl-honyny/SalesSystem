using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Enums;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة فواتير المشتريات — تدعم CRUD + ترحيل + إلغاء.
/// <para>
/// <b>تكافؤ المحاكاة (RULE-N167):</b> ترفض ما سترفضه طبقة الأعمال في المرحلة ب —
/// فاتورة بلا أصناف · كمية غير موجبة · سعر تكلفة غير موجب · مدفوع أكبر من الإجمالي ·
/// تعديل/ترحيل فاتورة مرحلة — كلها برسائل عربية عبر <see cref="Result.Failure"/>،
/// ولا تُحفظ الفاتورة جزئياً (التحقق يسبق أي كتابة). الفرق الوحيد عن فاتورة البيع أن الشراء
/// لا يُرفض لعدم كفاية المخزون لأنه يزيده.
/// </para>
/// <para>
/// <b>أثر المخزون (RULE-N167):</b> الثابت الملزم — رصيد بيانات التجربة (<c>MockData.Products.CurrentStock</c>)
/// يعكس المستندات <b>المرحّلة وغير الملغاة</b> فقط: الحفظ كمسودة (<c>Status = 1</c>) لا يزيد أي رصيد،
/// والزيادة تقع <b>مرة واحدة</b> في <see cref="PostAsync"/> عند نجاح الانتقال مسودة → مرحلة،
/// و<see cref="CancelAsync"/> يخصم الكميات <b>مرة واحدة</b> فقط إن كانت الفاتورة مرحلة.
/// لا توجد في المحاكاة مجموعة حركات مخزون مسجّلة، فلا تُنشأ حركة وهمية هنا.
/// </para>
/// </summary>
public class MockPurchaseInvoiceDesktopService : IPurchaseInvoiceDesktopService
{
    // ═══════════════════════════════════════════════
    // رسائل الرفض العربية — نص واحد لكل قاعدة (RULE-N169)
    // ═══════════════════════════════════════════════
    private const string EmptyLinesMessage = "لا يمكن حفظ الفاتورة بدون أصناف";
    private const string ZeroQuantityMessage = "الكمية يجب أن تكون أكبر من صفر";
    private const string ZeroPriceMessage = "السعر يجب أن يكون أكبر من صفر";
    private const string PaidExceedsTotalMessage = "المبلغ المدفوع أكبر من إجمالي الفاتورة";

    /// <summary>نص واحد مشترك لأي رفض بسبب نقص المخزون (بيت الأثر الواحد — RULE-N169).</summary>
    private const string InsufficientStockMessage = MockStockLedger.ShortageMessage;

    private const string UnexpectedErrorMessage = "تعذر إتمام العملية في وضع التجربة. يرجى إعادة المحاولة.";

    public Task<Result<IReadOnlyList<PurchaseInvoiceDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        byte? status = null, CancellationToken ct = default)
    {
        IEnumerable<PurchaseInvoiceDto> query = MockPurchasesData.Purchases;

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (from.HasValue)
        {
            var fromDate = DateOnly.FromDateTime(from.Value);
            query = query.Where(i => i.InvoiceDate >= fromDate);
        }

        if (to.HasValue)
        {
            var toDate = DateOnly.FromDateTime(to.Value);
            query = query.Where(i => i.InvoiceDate <= toDate);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(i =>
                i.InvoiceNo.ToString().Contains(s) ||
                (i.SupplierName != null && i.SupplierName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (i.Notes != null && i.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<PurchaseInvoiceDto>>.Success(
            query.OrderByDescending(i => i.InvoiceDate).ThenByDescending(i => i.InvoiceNo).ToList().AsReadOnly()));
    }

    public Task<Result<PurchaseInvoiceDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var invoice = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
        return invoice is null
            ? Task.FromResult(Result<PurchaseInvoiceDto>.Failure("فاتورة المشتريات غير موجودة"))
            : Task.FromResult(Result<PurchaseInvoiceDto>.Success(invoice));
    }

    public Task<Result<PurchaseInvoiceDto>> CreateAsync(CreatePurchaseInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            // ── تكافؤ المحاكاة: كل رفض طبقة الأعمال قبل أي كتابة (RULE-N167) ──
            string? validationError = ValidateInvoice(r);
            if (validationError is not null)
                return Task.FromResult(Result<PurchaseInvoiceDto>.Failure(validationError));

            var nextId = MockPurchasesData.Purchases.Count == 0 ? 1 : MockPurchasesData.Purchases.Max(i => i.Id) + 1;
            var nextInvoiceNo = MockPurchasesData.Purchases.Count == 0 ? 5001 : MockPurchasesData.Purchases.Max(i => i.InvoiceNo) + 1;
            var supplierName = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId)?.Name
                ?? "مورد غير معروف";
            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

            var items = BuildItems(r.Items);

            decimal subTotal = items.Sum(i => i.LineTotal);
            decimal netTotal = ComputeNetTotal(r);
            decimal remaining = netTotal - r.PaidAmount;

            var invoice = new PurchaseInvoiceDto(
                Id: nextId, InvoiceNo: nextInvoiceNo,
                SupplierId: r.SupplierId, SupplierName: supplierName,
                WarehouseId: r.WarehouseId,
                WarehouseName: warehouse?.Name ?? "مخزن غير معروف",
                InvoiceDate: DateOnly.FromDateTime(r.InvoiceDate ?? DateTime.Today),
                PaymentType: (byte)r.PaymentType,
                SubTotal: subTotal, DiscountAmount: r.DiscountAmount,
                DiscountType: (byte)(r.DiscountType ?? 0), DiscountRate: r.DiscountRate,
                CostInBaseCurrency: null,
                TaxAmount: r.TaxAmount, OtherCharges: r.OtherCharges,
                NetTotal: netTotal, PaidAmount: r.PaidAmount, RemainingAmount: remaining,
                Notes: r.Notes, SupplierInvoiceNo: null,
                AttachmentPath: r.AttachmentPath, Status: 1,
                TaxId: r.TaxId, TaxName: null, TaxRate: null,
                CashBoxId: null, Items: items);

            MockPurchasesData.Purchases.Add(invoice);

            // لا أثر مخزني عند الحفظ: المسودة (Status = 1) لا تحرّك رصيداً — الزيادة في PostAsync وحده.

            return Task.FromResult(Result<PurchaseInvoiceDto>.Success(invoice));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ فاتورة المشتريات (محاكاة)");
            return Task.FromResult(Result<PurchaseInvoiceDto>.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result<PurchaseInvoiceDto>> UpdateAsync(int id, CreatePurchaseInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            var existing = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
            if (existing is null)
                return Task.FromResult(Result<PurchaseInvoiceDto>.Failure("فاتورة المشتريات غير موجودة"));

            if (existing.Status == 2)
                return Task.FromResult(Result<PurchaseInvoiceDto>.Failure("لا يمكن تعديل فاتورة مرحلة"));
            if (existing.Status == 3)
                return Task.FromResult(Result<PurchaseInvoiceDto>.Failure("لا يمكن تعديل فاتورة ملغية"));

            string? validationError = ValidateInvoice(r);
            if (validationError is not null)
                return Task.FromResult(Result<PurchaseInvoiceDto>.Failure(validationError));

            var supplierName = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId)?.Name
                ?? existing.SupplierName;
            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);

            var items = BuildItems(r.Items);

            decimal subTotal = items.Sum(i => i.LineTotal);
            decimal netTotal = ComputeNetTotal(r);
            decimal remaining = netTotal - r.PaidAmount;

            var updated = existing with
            {
                SupplierId = r.SupplierId,
                SupplierName = supplierName,
                WarehouseId = r.WarehouseId,
                WarehouseName = warehouse?.Name ?? existing.WarehouseName,
                InvoiceDate = r.InvoiceDate.HasValue ? DateOnly.FromDateTime(r.InvoiceDate.Value) : existing.InvoiceDate,
                PaymentType = (byte)r.PaymentType,
                SubTotal = subTotal,
                DiscountAmount = r.DiscountAmount,
                DiscountType = (byte)(r.DiscountType ?? 0),
                TaxAmount = r.TaxAmount,
                OtherCharges = r.OtherCharges,
                NetTotal = netTotal,
                PaidAmount = r.PaidAmount,
                RemainingAmount = remaining,
                Notes = r.Notes,
                TaxId = r.TaxId,
                AttachmentPath = r.AttachmentPath,
                Items = items
            };

            var index = MockPurchasesData.Purchases.IndexOf(existing);
            MockPurchasesData.Purchases[index] = updated;

            // لا أثر مخزني عند التعديل: المسودة لا تحرّك رصيداً، والمرحّلة غير قابلة للتعديل أصلاً.

            return Task.FromResult(Result<PurchaseInvoiceDto>.Success(updated));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تعديل فاتورة المشتريات (محاكاة)");
            return Task.FromResult(Result<PurchaseInvoiceDto>.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("فاتورة المشتريات غير موجودة"));

            if (existing.Status == 2)
                return Task.FromResult(Result.Failure("الفاتورة مرحلة بالفعل"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("لا يمكن ترحيل فاتورة ملغية"));

            var index = MockPurchasesData.Purchases.IndexOf(existing);

            // الترحيل هو النقطة الوحيدة التي تُحرّك المخزون (Draft → Posted): إضافة الكميات المشتراة
            MockStockLedger.Apply(QuantitiesOf(existing), direction: +1);

            MockPurchasesData.Purchases[index] = existing with { Status = 2 };
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر ترحيل فاتورة المشتريات (محاكاة)");
            return Task.FromResult(Result.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockPurchasesData.Purchases.FirstOrDefault(i => i.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("فاتورة المشتريات غير موجودة"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("الفاتورة ملغية بالفعل"));

            var index = MockPurchasesData.Purchases.IndexOf(existing);

            // الإلغاء يعكس الأثر مرة واحدة فقط: المرحّلة (2) أضافت عند الترحيل فتُخصم كمياتها الآن،
            // والمسودة (1) لم تحرّك شيئاً فلا يُخصم لها شيء.
            if (existing.Status == 2)
            {
                var quantities = QuantitiesOf(existing);

                // تكافؤ الرفض: لا رصيد سالباً (CHECK (Quantity >= 0)) — يُفحص قبل أي حركة
                if (!MockStockLedger.HasSufficientStock(quantities))
                    return Task.FromResult(Result.Failure(InsufficientStockMessage));

                MockStockLedger.Apply(quantities, direction: -1);
            }

            MockPurchasesData.Purchases[index] = existing with { Status = 3 };
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر إلغاء فاتورة المشتريات (محاكاة)");
            return Task.FromResult(Result.Failure(UnexpectedErrorMessage));
        }
    }

    // ═══════════════════════════════════════════════
    // بناء السطور والحسابات — مكان واحد للفورمولا (AGENTS §2.2)
    // ═══════════════════════════════════════════════
    private static List<PurchaseInvoiceLineDto> BuildItems(IEnumerable<CreatePurchaseInvoiceLineRequest> lines)
    {
        return lines.Select((line, idx) =>
        {
            var productName = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId)?.Name ?? "منتج غير معروف";
            decimal lineDiscount = line.DiscountType == DiscountType.Percentage && line.DiscountRate.HasValue
                ? line.Quantity * line.UnitPrice * line.DiscountRate.Value / 100m
                : line.DiscountRate ?? 0m;
            decimal lineTotal = (line.Quantity * line.UnitPrice) - lineDiscount;
            return new PurchaseInvoiceLineDto(
                Id: idx + 1, ProductId: line.ProductId, ProductName: productName,
                ProductUnitId: line.ProductUnitId, ProductUnitName: null,
                Quantity: line.Quantity, UnitPrice: line.UnitPrice, LineTotal: lineTotal,
                LandedUnitCost: line.UnitPrice,
                DiscountType: (byte)(line.DiscountType ?? 0), DiscountRate: line.DiscountRate,
                DiscountAmount: lineDiscount,
                CostInBaseCurrency: null, AdditionalFeesAmount: 0m);
        }).ToList();
    }

    /// <summary>NetTotal = SubTotal − InvoiceDiscount + TaxAmount + OtherCharges (AGENTS §2.2).</summary>
    private static decimal ComputeNetTotal(CreatePurchaseInvoiceRequest r)
    {
        decimal subTotal = BuildItems(r.Items).Sum(i => i.LineTotal);
        return subTotal - r.DiscountAmount + r.TaxAmount + r.OtherCharges;
    }

    /// <summary>
    /// رفض واحد يسبق الكتابة (RULE-N167): أصناف · كمية · سعر التكلفة · مدفوع.
    /// يعيد نص الرفض العربي أو <c>null</c> عند الصلاحية. الشراء لا يُرفض لعدم كفاية المخزون.
    /// </summary>
    private static string? ValidateInvoice(CreatePurchaseInvoiceRequest r)
    {
        if (r.Items is null || r.Items.Count == 0)
            return EmptyLinesMessage;

        if (r.Items.Any(line => line.Quantity <= 0m))
            return ZeroQuantityMessage;

        if (r.Items.Any(line => line.UnitPrice <= 0m))
            return ZeroPriceMessage;

        if (r.PaidAmount > ComputeNetTotal(r))
            return PaidExceedsTotalMessage;

        return null;
    }

    /// <summary>مجاميع كميات هذه الفاتورة لكل صنف — تُستهلك في الترحيل والإلغاء.</summary>
    private static Dictionary<int, decimal> QuantitiesOf(PurchaseInvoiceDto invoice)
        => MockStockLedger.QuantitiesByProduct(
            (invoice.Items ?? Array.Empty<PurchaseInvoiceLineDto>()).Select(i => (i.ProductId, i.Quantity)));
}
