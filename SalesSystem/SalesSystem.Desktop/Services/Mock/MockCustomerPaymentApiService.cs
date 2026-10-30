using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة دفعات العملاء — بيانات في الذاكرة بدون قاعدة بيانات.
/// تُستخدم للوحدات التي لا يوجد لها تنفيذ خلفي (BLL) بعد.
/// </summary>
public class MockCustomerPaymentApiService : ICustomerPaymentApiService
{
    /// <summary>
    /// الدفعات في الذاكرة — تُخزَّن مع معرّف العميل لأن الـ DTO لا يحمله.
    /// </summary>
    private static readonly List<(int CustomerId, CustomerPaymentDto Payment)> Payments = new();

    public Task<Result<IReadOnlyList<CustomerPaymentDto>>> GetAllAsync(int? customerId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        IEnumerable<(int CustomerId, CustomerPaymentDto Payment)> query = Payments;

        if (customerId.HasValue)
            query = query.Where(p => p.CustomerId == customerId.Value);

        if (from.HasValue)
            query = query.Where(p => p.Payment.ReceiptDate.Date >= from.Value.Date);

        if (to.HasValue)
            query = query.Where(p => p.Payment.ReceiptDate.Date <= to.Value.Date);

        var result = query
            .OrderBy(p => p.Payment.ReceiptDate)
            .Select(p => p.Payment)
            .ToList()
            .AsReadOnly();

        return Task.FromResult(Result<IReadOnlyList<CustomerPaymentDto>>.Success(result));
    }

    public Task<Result<CustomerPaymentDto>> CreateAsync(CreateCustomerPaymentRequest r, CancellationToken ct = default)
    {
        if (r.Amount <= 0)
            return Task.FromResult(Result<CustomerPaymentDto>.Failure("المبلغ يجب أن يكون أكبر من صفر", "INVALID_AMOUNT"));

        var nextId = Payments.Count > 0 ? Payments.Max(p => p.Payment.Id) + 1 : 1;
        var nextNo = Payments.Count > 0 ? Payments.Max(p => p.Payment.ReceiptNo) + 1 : 1;

        // جلب اسم العميل من البيانات التجريبية
        var customer = MockData.Customers.FirstOrDefault(c => c.Id == r.CustomerId);
        if (customer is null)
            return Task.FromResult(Result<CustomerPaymentDto>.Failure("العميل غير موجود", "CUSTOMER_NOT_FOUND"));

        var payment = new CustomerPaymentDto(
            Id: nextId,
            ReceiptNo: nextNo,
            CustomerName: customer.Name,
            Amount: r.Amount,
            ReceiptDate: r.PaymentDate == default ? DateTime.UtcNow : r.PaymentDate,
            Notes: r.Notes);

        Payments.Add((r.CustomerId, payment));
        return Task.FromResult(Result<CustomerPaymentDto>.Success(payment));
    }
}
