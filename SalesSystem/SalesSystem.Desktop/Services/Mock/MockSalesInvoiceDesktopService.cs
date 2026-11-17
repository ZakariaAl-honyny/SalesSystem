using System.Globalization;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Enums;
using SalesSystem.DTO.Requests;
using InventoryTxType = SalesSystem.Common.Enums.InventoryTransactionType;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة فواتير البيع — تدعم CRUD + ترحيل + إلغاء.
/// <para>
/// <b>تكافؤ المحاكاة (RULE-N167):</b> ترفض ما سترفضه طبقة الأعمال في المرحلة ب —
/// فاتورة بلا أصناف · كمية غير موجبة · سعر غير موجب · مدفوع أكبر من الإجمالي ·
/// كمية تتجاوز المخزون المتاح · تعديل/ترحيل فاتورة مرحلة — كلها برسائل عربية عبر <see cref="Result.Failure"/>،
/// ولا تُحفظ الفاتورة جزئياً (التحقق يسبق أي كتابة).
/// </para>
/// <para>
/// <b>أثر المخزون (RULE-N167):</b> الثابت الملزم — رصيد بيانات التجربة (<c>MockData.Products.CurrentStock</c>)
/// يعكس المستندات <b>المرحّلة وغير الملغاة</b> فقط: الحفظ كمسودة (<c>Status = 1</c>) لا يحرّك أي رصيد،
/// والخصم يقع <b>مرة واحدة</b> في <see cref="PostAsync"/> عند نجاح الانتقال مسودة → مرحلة،
/// و<see cref="CancelAsync"/> يعيد الكميات <b>مرة واحدة</b> فقط إن كانت الفاتورة مرحلة
/// (المسودة لم تخصم شيئاً فلا يُعاد لها شيء). وكل حركة تُسجَّل في سجل الحركات الواحد
/// (<c>MockStockLedger</c>) بنوع «بيع» ومخزن الفاتورة ورقمها — فتُقرأ في شاشة الحركات والتقارير.
/// </para>
/// </summary>
public class MockSalesInvoiceDesktopService : ISalesInvoiceDesktopService
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

    public Task<Result<IReadOnlyList<SalesInvoiceDto>>> GetAllAsync(
        string? search = null, DateTime? from = null, DateTime? to = null,
        byte? status = null, CancellationToken ct = default)
    {
        IEnumerable<SalesInvoiceDto> query = MockData.Invoices;

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (from.HasValue)
            query = query.Where(i => i.InvoiceDate >= from.Value);

        if (to.HasValue)
            query = query.Where(i => i.InvoiceDate <= to.Value.Date.AddDays(1).AddTicks(-1));

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(i =>
                i.InvoiceNo.ToString().Contains(s) ||
                (i.CustomerName != null && i.CustomerName.Contains(s, StringComparison.OrdinalIgnoreCase)) ||
                (i.Notes != null && i.Notes.Contains(s, StringComparison.OrdinalIgnoreCase)));
        }

        return Task.FromResult(Result<IReadOnlyList<SalesInvoiceDto>>.Success(
            query.OrderByDescending(i => i.InvoiceDate).ThenByDescending(i => i.InvoiceNo).ToList().AsReadOnly()));
    }

    public Task<Result<SalesInvoiceDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var invoice = MockData.Invoices.FirstOrDefault(i => i.Id == id);
        return invoice is null
            ? Task.FromResult(Result<SalesInvoiceDto>.Failure("الفاتورة غير موجودة"))
            : Task.FromResult(Result<SalesInvoiceDto>.Success(invoice));
    }

    public Task<Result<SalesInvoiceDto>> CreateAsync(CreateSalesInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            // ── تكافؤ المحاكاة: كل رفض طبقة الأعمال قبل أي كتابة (RULE-N167) ──
            string? validationError = ValidateInvoice(r);
            if (validationError is not null)
                return Task.FromResult(Result<SalesInvoiceDto>.Failure(validationError));

            var nextId = MockData.Invoices.Count == 0 ? 1 : MockData.Invoices.Max(i => i.Id) + 1;
            var nextInvoiceNo = MockData.Invoices.Count == 0 ? 1001 : MockData.Invoices.Max(i => i.InvoiceNo) + 1;
            var customerName = MockData.Customers.FirstOrDefault(c => c.Id == r.CustomerId)?.Name
                ?? (r.CustomerId == 0 ? "عميل نقدي" : null);
            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);
            var cashBoxName = r.CashBoxId.HasValue
                ? MockData.CashBoxes.FirstOrDefault(c => c.Id == r.CashBoxId.Value)?.Name
                : null;

            var items = BuildItems(r.Items);

            decimal subTotal = items.Sum(i => i.LineTotal);
            decimal netTotal = ComputeNetTotal(r);
            decimal remaining = netTotal - r.PaidAmount;

            var invoice = new SalesInvoiceDto(
                Id: nextId, InvoiceNo: nextInvoiceNo, CustomerId: r.CustomerId,
                CustomerName: customerName, WarehouseId: r.WarehouseId,
                WarehouseName: warehouse?.Name ?? "مخزن غير معروف",
                InvoiceDate: r.InvoiceDate ?? DateTime.Today,
                PaymentType: (byte)r.PaymentType, SubTotal: subTotal,
                DiscountAmount: r.DiscountAmount, DiscountType: (byte)r.DiscountType,
                DiscountRate: r.DiscountRate, TaxAmount: r.TaxAmount,
                OtherCharges: r.OtherCharges, NetTotal: netTotal,
                PaidAmount: r.PaidAmount, RemainingAmount: remaining,
                Notes: r.Notes, Status: 1,
                TaxId: r.TaxId, TaxName: null, TaxRate: null,
                CashBoxId: r.CashBoxId, CashBoxName: cashBoxName, Items: items);

            MockData.Invoices.Add(invoice);

            // لا أثر مخزني عند الحفظ: المسودة (Status = 1) لا تحرّك رصيداً — الخصم في PostAsync وحده.

            return Task.FromResult(Result<SalesInvoiceDto>.Success(invoice));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر حفظ فاتورة البيع (محاكاة)");
            return Task.FromResult(Result<SalesInvoiceDto>.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result<SalesInvoiceDto>> UpdateAsync(int id, CreateSalesInvoiceRequest r, CancellationToken ct = default)
    {
        try
        {
            var existing = MockData.Invoices.FirstOrDefault(i => i.Id == id);
            if (existing is null)
                return Task.FromResult(Result<SalesInvoiceDto>.Failure("الفاتورة غير موجودة"));

            if (existing.Status == 2)
                return Task.FromResult(Result<SalesInvoiceDto>.Failure("لا يمكن تعديل فاتورة مرحلة"));
            if (existing.Status == 3)
                return Task.FromResult(Result<SalesInvoiceDto>.Failure("لا يمكن تعديل فاتورة ملغية"));

            string? validationError = ValidateInvoice(r);
            if (validationError is not null)
                return Task.FromResult(Result<SalesInvoiceDto>.Failure(validationError));

            var customerName = MockData.Customers.FirstOrDefault(c => c.Id == r.CustomerId)?.Name
                ?? (r.CustomerId == 0 ? "عميل نقدي" : null);
            var warehouse = MockData.Warehouses.FirstOrDefault(w => w.Id == (short)r.WarehouseId);
            var cashBoxName = r.CashBoxId.HasValue
                ? MockData.CashBoxes.FirstOrDefault(c => c.Id == r.CashBoxId.Value)?.Name
                : null;

            var items = BuildItems(r.Items);

            decimal subTotal = items.Sum(i => i.LineTotal);
            decimal netTotal = ComputeNetTotal(r);
            decimal remaining = netTotal - r.PaidAmount;

            var updated = existing with
            {
                CustomerId = r.CustomerId,
                CustomerName = customerName,
                WarehouseId = r.WarehouseId,
                WarehouseName = warehouse?.Name ?? existing.WarehouseName,
                InvoiceDate = r.InvoiceDate ?? existing.InvoiceDate,
                PaymentType = (byte)r.PaymentType,
                SubTotal = subTotal,
                DiscountAmount = r.DiscountAmount,
                DiscountType = (byte)r.DiscountType,
                DiscountRate = r.DiscountRate,
                TaxAmount = r.TaxAmount,
                OtherCharges = r.OtherCharges,
                NetTotal = netTotal,
                PaidAmount = r.PaidAmount,
                RemainingAmount = remaining,
                Notes = r.Notes,
                TaxId = r.TaxId,
                CashBoxId = r.CashBoxId,
                CashBoxName = cashBoxName,
                Items = items
            };

            var index = MockData.Invoices.IndexOf(existing);
            MockData.Invoices[index] = updated;

            // لا أثر مخزني عند التعديل: المسودة لا تحرّك رصيداً، والمرحّلة غير قابلة للتعديل أصلاً.

            return Task.FromResult(Result<SalesInvoiceDto>.Success(updated));
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تعديل فاتورة البيع (محاكاة)");
            return Task.FromResult(Result<SalesInvoiceDto>.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockData.Invoices.FirstOrDefault(i => i.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("الفاتورة غير موجودة"));

            if (existing.Status == 2)
                return Task.FromResult(Result.Failure("الفاتورة مرحلة بالفعل"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("لا يمكن ترحيل فاتورة ملغية"));

            var index = MockData.Invoices.IndexOf(existing);

            // ── التحقق أولاً ثم الحركة: لا خصم جزئي — كل الأصناف تُفحص قبل أي تغيير (RULE-N167) ──
            var quantities = QuantitiesOf(existing);
            if (!MockStockLedger.HasSufficientStock(quantities))
                return Task.FromResult(Result.Failure(InsufficientStockMessage));

            // الترحيل هو النقطة الوحيدة التي تُحرّك المخزون (Draft → Posted)
            // الحركة تحمل سياقها الحقيقي: نوع «بيع» + مخزن الفاتورة + رقمها + تاريخها (RULE-N167)
            MockStockLedger.Apply(quantities,
                direction: -1,
                type: InventoryTxType.Sale,
                warehouseId: (short)existing.WarehouseId,
                reference: existing.InvoiceNo.ToString(CultureInfo.InvariantCulture),
                date: existing.InvoiceDate);

            MockData.Invoices[index] = existing with { Status = 2 };
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر ترحيل فاتورة البيع (محاكاة)");
            return Task.FromResult(Result.Failure(UnexpectedErrorMessage));
        }
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockData.Invoices.FirstOrDefault(i => i.Id == id);
            if (existing is null)
                return Task.FromResult(Result.Failure("الفاتورة غير موجودة"));

            if (existing.Status == 3)
                return Task.FromResult(Result.Failure("الفاتورة ملغية بالفعل"));

            var index = MockData.Invoices.IndexOf(existing);

            // الإلغاء يعكس الأثر مرة واحدة فقط: المرحّلة (2) خصمت عند الترحيل فتُعاد كمياتها،
            // والمسودة (1) لم تخصم شيئاً فلا يُضاف لها شيء.
            // القيد العكسي يُسجَّل بنوع «تسوية» لأنه النوع الذي يحفظ إشارة الكمية في السجل،
            // فيبقى اتجاه الحركة (إعادة 45 إلى المخزن) مقروءاً في تقرير الحركة — مع رقم الفاتورة.
            if (existing.Status == 2)
                MockStockLedger.Apply(QuantitiesOf(existing),
                    direction: +1,
                    type: InventoryTxType.Adjustment,
                    warehouseId: (short)existing.WarehouseId,
                    reference: existing.InvoiceNo.ToString(CultureInfo.InvariantCulture),
                    date: existing.InvoiceDate);

            MockData.Invoices[index] = existing with { Status = 3 };
            return Task.FromResult(Result.Success());
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر إلغاء فاتورة البيع (محاكاة)");
            return Task.FromResult(Result.Failure(UnexpectedErrorMessage));
        }
    }

    // ═══════════════════════════════════════════════
    // بناء السطور والحسابات — مكان واحد للفورمولا (AGENTS §2.2)
    // ═══════════════════════════════════════════════
    private static List<SalesInvoiceLineDto> BuildItems(IEnumerable<CreateSalesInvoiceLineRequest> lines)
    {
        return lines.Select((line, idx) =>
        {
            var product = MockData.Products.FirstOrDefault(p => p.Id == line.ProductId);
            var productName = product?.Name ?? "منتج غير معروف";
            var productUnit = MockData.ProductUnits.FirstOrDefault(u => u.Id == line.ProductUnitId);

            decimal gross = line.Quantity * line.UnitPrice;

            // الخصم: مبلغ ثابت أو نسبة مئوية من إجمالي السطر — كلاهما مدعوم
            decimal disc = line.DiscountType == DiscountType.Percentage
                ? gross * (line.DiscountRate ?? 0) / 100m
                : line.DiscountRate ?? 0m;

            // حصر الخصم بين صفر وإجمالي السطر (بدون استثناءات)
            decimal maxDiscount = gross > 0m ? gross : 0m;
            disc = disc < 0m ? 0m : (disc > maxDiscount ? maxDiscount : disc);

            decimal lineTotal = gross - disc;

            return new SalesInvoiceLineDto(
                Id: idx + 1, ProductId: line.ProductId, ProductName: productName,
                Quantity: line.Quantity, UnitPrice: line.UnitPrice, LineTotal: lineTotal,
                ProductUnitId: line.ProductUnitId,
                DiscountType: (byte)line.DiscountType, DiscountRate: line.DiscountRate,
                DiscountAmount: disc,
                ExpiryDate: line.ExpiryDate,
                UnitName: productUnit?.UnitName,
                ProductCode: product?.Barcode);
        }).ToList();
    }

    /// <summary>NetTotal = SubTotal − InvoiceDiscount + TaxAmount + OtherCharges (AGENTS §2.2).</summary>
    private static decimal ComputeNetTotal(CreateSalesInvoiceRequest r)
    {
        decimal subTotal = BuildItems(r.Items).Sum(i => i.LineTotal);
        return subTotal - r.DiscountAmount + r.TaxAmount + r.OtherCharges;
    }

    /// <summary>
    /// رفض واحد يسبق الكتابة (RULE-N167): أصناف · كمية · سعر · مدفوع · مخزون.
    /// يعيد نص الرفض العربي أو <c>null</c> عند الصلاحية.
    /// <para>
    /// فحص المخزون هنا **إنذار مبكر** فقط (دفعة مسودة بكيمة غير متاحة تُرفض قبل الحفظ)،
    /// والفحص المرجعي الملزم يقع في <see cref="PostAsync"/> لأن الرصيد يتغير بين الحفظ والترحيل.
    /// </para>
    /// </summary>
    private static string? ValidateInvoice(CreateSalesInvoiceRequest r)
    {
        if (r.Items is null || r.Items.Count == 0)
            return EmptyLinesMessage;

        if (r.Items.Any(line => line.Quantity <= 0m))
            return ZeroQuantityMessage;

        if (r.Items.Any(line => line.UnitPrice <= 0m))
            return ZeroPriceMessage;

        if (r.PaidAmount > ComputeNetTotal(r))
            return PaidExceedsTotalMessage;

        var quantities = MockStockLedger.QuantitiesByProduct(r.Items.Select(line => (line.ProductId, line.Quantity)));
        if (!MockStockLedger.HasSufficientStock(quantities))
            return InsufficientStockMessage;

        return null;
    }

    /// <summary>مجاميع كميات هذه الفاتورة لكل صنف — تُستهلك في الترحيل والإلغاء.</summary>
    private static Dictionary<int, decimal> QuantitiesOf(SalesInvoiceDto invoice)
        => MockStockLedger.QuantitiesByProduct(
            (invoice.Items ?? Array.Empty<SalesInvoiceLineDto>()).Select(i => (i.ProductId, i.Quantity)));
}
