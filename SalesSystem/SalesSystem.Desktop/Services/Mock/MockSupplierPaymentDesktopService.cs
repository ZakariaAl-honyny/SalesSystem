using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة سندات صرف الموردين — تعيد بيانات من MockSupplierPaymentsData.
/// </summary>
/// <remarks>
/// RULE-N167 — تكافؤ الرفض: مبلغ صفر أو سالب · مبلغ أكبر من المتبقي للمورد ·
/// ترحيل سند مرحّل أو ملغى · إلغاء سند ملغى.
/// RULE-004 — <see cref="CancelAsync"/> لا يحذف السند: يسجّل إلغاءه فيبقى ظاهراً في القائمة.
/// «المتبقي للمورد» في وضع التجربة = مجموع <c>RemainingAmount</c> لفواتير الشراء غير الملغاة
/// (<c>MockPurchasesData.Purchases</c>)، أو متبقي الفاتورة المرتبطة بالسند إن وُجد —
/// لأن <c>SupplierDto.CurrentBalance</c> ثابت على صفر في هذه الطبقة.
/// </remarks>
public class MockSupplierPaymentDesktopService : ISupplierPaymentDesktopService
{
    /// <summary>
    /// حالة السند في وضع التجربة — <c>SupplierPaymentDto</c> لا يحمل حقلاً للحالة،
    /// فتُتتبَّع هنا في الذاكرة (تُنقل إلى <c>MockSupplierPaymentsData</c> عند أول مساس بها).
    /// </summary>
    private enum PaymentState : byte
    {
        Posted = 1,
        Cancelled = 2
    }

    private static readonly Dictionary<int, PaymentState> _states = new();

    public Task<Result<IReadOnlyList<SupplierPaymentDto>>> GetAllAsync(int? supplierId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        IEnumerable<SupplierPaymentDto> query = MockSupplierPaymentsData.Payments;

        if (supplierId.HasValue)
            query = query.Where(p => p.SupplierId == supplierId.Value);

        if (from.HasValue)
            query = query.Where(p => p.PaymentDate >= DateOnly.FromDateTime(from.Value));

        if (to.HasValue)
            query = query.Where(p => p.PaymentDate <= DateOnly.FromDateTime(to.Value));

        // RULE-004 — السند الملغى يبقى في القائمة (الإلغاء حالة لا حذف).
        return Task.FromResult(Result<IReadOnlyList<SupplierPaymentDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<SupplierPaymentDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var payment = MockSupplierPaymentsData.Payments.FirstOrDefault(p => p.Id == id);
        return payment is null
            ? Task.FromResult(Result<SupplierPaymentDto>.Failure("سند الصرف غير موجود"))
            : Task.FromResult(Result<SupplierPaymentDto>.Success(payment));
    }

    public Task<Result<SupplierPaymentDto>> CreateAsync(CreateSupplierPaymentRequest r, CancellationToken ct = default)
    {
        try
        {
            // RULE-N167 — المبلغ أكبر من صفر.
            if (r.Amount <= 0m)
                return Task.FromResult(Result<SupplierPaymentDto>.Failure("المبلغ يجب أن يكون أكبر من صفر"));

            var supplier = MockSuppliersData.Suppliers.FirstOrDefault(s => s.Id == r.SupplierId);
            if (supplier is null)
                return Task.FromResult(Result<SupplierPaymentDto>.Failure("المورد غير موجود"));

            // RULE-N167 — لا سداد أكبر من المتبقي للمورد (أو من متبقي الفاتورة إن رُبط السند بها).
            if (r.Amount > _RemainingFor(r.SupplierId, r.PurchaseInvoiceId))
                return Task.FromResult(Result<SupplierPaymentDto>.Failure("المبلغ أكبر من المتبقي للمورد"));

            var nextId = MockSupplierPaymentsData.Payments.Select(p => p.Id).DefaultIfEmpty(0).Max() + 1;
            var nextNo = MockSupplierPaymentsData.Payments
                .Select(p => int.TryParse(p.PaymentNo, out int no) ? no : 0)
                .DefaultIfEmpty(0)
                .Max() + 1;

            // TODO: CashBoxId / CashBoxName / Status / Currency / CreatedAt / PostedAt / CancelledAt
            //       removed from SupplierPaymentDto — السند لا يحمل صندوقاً أو حالة في هذا الإصدار.
            var payment = new SupplierPaymentDto(
                Id: nextId,
                PaymentNo: nextNo.ToString(),
                SupplierId: r.SupplierId,
                SupplierName: supplier.Name,
                Amount: r.Amount,
                PaymentMethod: (byte)r.PaymentMethod,
                PaymentDate: r.PaymentDate.HasValue
                    ? DateOnly.FromDateTime(r.PaymentDate.Value)
                    : DateOnly.FromDateTime(DateTime.UtcNow),
                PurchaseInvoiceId: r.PurchaseInvoiceId,
                Notes: r.Notes,
                BranchId: r.BranchId ?? 1);

            MockSupplierPaymentsData.Payments.Add(payment);
            return Task.FromResult(Result<SupplierPaymentDto>.Success(payment));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<SupplierPaymentDto>.Failure("تعذر حفظ سند الصرف"));
        }
    }

    public Task<Result<SupplierPaymentDto>> PostAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockSupplierPaymentsData.Payments.FirstOrDefault(p => p.Id == id);
            if (existing is null)
                return Task.FromResult(Result<SupplierPaymentDto>.Failure("سند الصرف غير موجود"));

            // RULE-N167 — لا ترحيل لسند ملغى، ولا ترحيل مزدوج لسند مرحّل.
            if (_states.TryGetValue(id, out var state))
            {
                if (state == PaymentState.Cancelled)
                    return Task.FromResult(Result<SupplierPaymentDto>.Failure("لا يمكن ترحيل سند ملغى"));

                if (state == PaymentState.Posted)
                    return Task.FromResult(Result<SupplierPaymentDto>.Failure("السند مرحّل بالفعل"));
            }

            _states[id] = PaymentState.Posted;
            return Task.FromResult(Result<SupplierPaymentDto>.Success(existing));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<SupplierPaymentDto>.Failure("تعذر ترحيل سند الصرف"));
        }
    }

    public Task<Result<SupplierPaymentDto>> CancelAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var existing = MockSupplierPaymentsData.Payments.FirstOrDefault(p => p.Id == id);
            if (existing is null)
                return Task.FromResult(Result<SupplierPaymentDto>.Failure("سند الصرف غير موجود"));

            // RULE-N167 — لا إلغاء مزدوج.
            if (_states.TryGetValue(id, out var state) && state == PaymentState.Cancelled)
                return Task.FromResult(Result<SupplierPaymentDto>.Failure("السند ملغى بالفعل"));

            // RULE-004 — الإلغاء حالة لا حذف: السند يبقى في البيانات ويظل ظاهراً في القائمة.
            _states[id] = PaymentState.Cancelled;
            return Task.FromResult(Result<SupplierPaymentDto>.Success(existing));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<SupplierPaymentDto>.Failure("تعذر إلغاء سند الصرف"));
        }
    }

    /// <summary>
    /// RULE-N167 — المتبقي: متبقي الفاتورة المرتبطة إن وُجدت، وإلا مجموع متبقي فواتير المورد
    /// غير الملغاة (مسودة + مرحّلة) من بيانات المشتريات التجريبية.
    /// </summary>
    private static decimal _RemainingFor(int supplierId, int? purchaseInvoiceId)
    {
        if (purchaseInvoiceId is int invoiceId)
        {
            var invoice = MockPurchasesData.Purchases
                .FirstOrDefault(i => i.Id == invoiceId && i.SupplierId == supplierId && i.Status != 3);

            return invoice?.RemainingAmount ?? 0m;
        }

        return MockPurchasesData.Purchases
            .Where(i => i.SupplierId == supplierId && i.Status != 3)
            .Sum(i => i.RemainingAmount);
    }
}
