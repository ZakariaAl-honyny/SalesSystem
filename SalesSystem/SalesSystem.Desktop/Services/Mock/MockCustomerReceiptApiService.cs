using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة سندات قبض العملاء — تعيد بيانات من MockCustomerReceiptsData.
/// </summary>
public class MockCustomerReceiptApiService : ICustomerReceiptApiService
{
    public Task<Result<IReadOnlyList<CustomerReceiptDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<CustomerReceiptDto> query = MockCustomerReceiptsData.Receipts;

        if (!includeInactive)
            query = query.Where(r => r.IsActive);

        return Task.FromResult(Result<IReadOnlyList<CustomerReceiptDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<CustomerReceiptDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var receipt = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
        return receipt is null
            ? Task.FromResult(Result<CustomerReceiptDto>.Failure("سند القبض غير موجود"))
            : Task.FromResult(Result<CustomerReceiptDto>.Success(receipt));
    }

    public Task<Result<CustomerReceiptDto>> CreateAsync(CreateCustomerReceiptRequest request, CancellationToken ct = default)
    {
        var all = MockCustomerReceiptsData.Receipts;
        var nextId = all.Count > 0 ? all.Max(r => r.Id) + 1 : 1;
        var nextNo = all.Count > 0 ? all.Max(r => r.ReceiptNo) + 1 : 1001;

        // Resolve display names from MockData
        var customer = MockData.Customers.FirstOrDefault(c => c.Id == request.CustomerId);
        var cashBox = MockData.CashBoxes.FirstOrDefault(b => b.Id == request.CashBoxId);

        string paymentMethodName = request.PaymentMethod switch
        {
            1 => "نقدي",
            2 => "شيك",
            3 => "تحويل بنكي",
            4 => "بطاقة ائتمان",
            _ => "نقدي"
        };

        var receipt = new CustomerReceiptDto(
            Id: nextId,
            ReceiptNo: nextNo,
            ReceiptDate: DateTime.UtcNow,
            CustomerId: request.CustomerId,
            CustomerName: customer?.Name,
            CashBoxId: request.CashBoxId,
            CashBoxName: cashBox?.Name,
            Amount: request.Amount,
            PaymentMethod: request.PaymentMethod,
            PaymentMethodName: paymentMethodName,
            Notes: request.Notes,
            Status: 1, // Draft
            StatusName: "مسودة",
            PostedAt: null,
            Applications: new List<CustomerReceiptApplicationDto>(),
            IsActive: true
        );

        all.Add(receipt);
        return Task.FromResult(Result<CustomerReceiptDto>.Success(receipt));
    }

    public Task<Result<CustomerReceiptDto>> UpdateAsync(int id, UpdateCustomerReceiptRequest request, CancellationToken ct = default)
    {
        var existing = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CustomerReceiptDto>.Failure("سند القبض غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<CustomerReceiptDto>.Failure("لا يمكن تعديل سند قبض مرحّل أو ملغي"));

        var cashBox = MockData.CashBoxes.FirstOrDefault(b => b.Id == request.CashBoxId);

        string paymentMethodName = request.PaymentMethod switch
        {
            1 => "نقدي",
            2 => "شيك",
            3 => "تحويل بنكي",
            4 => "بطاقة ائتمان",
            _ => "نقدي"
        };

        var updated = existing with
        {
            CashBoxId = request.CashBoxId,
            CashBoxName = cashBox?.Name,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            PaymentMethodName = paymentMethodName,
            Notes = request.Notes
        };

        var index = MockCustomerReceiptsData.Receipts.IndexOf(existing);
        MockCustomerReceiptsData.Receipts[index] = updated;
        return Task.FromResult(Result<CustomerReceiptDto>.Success(updated));
    }

    public Task<Result<CustomerReceiptDto>> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CustomerReceiptDto>.Failure("سند القبض غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<CustomerReceiptDto>.Failure("لا يمكن ترحيل السند إلا في حالة المسودة"));

        var posted = existing with
        {
            Status = 2,
            StatusName = "مرحّل",
            PostedAt = DateTime.UtcNow
        };

        var index = MockCustomerReceiptsData.Receipts.IndexOf(existing);
        MockCustomerReceiptsData.Receipts[index] = posted;
        return Task.FromResult(Result<CustomerReceiptDto>.Success(posted));
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockCustomerReceiptsData.Receipts.FirstOrDefault(r => r.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("سند القبض غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("السند ملغي بالفعل"));

        var cancelled = existing with
        {
            Status = 3,
            StatusName = "ملغي"
        };

        var index = MockCustomerReceiptsData.Receipts.IndexOf(existing);
        MockCustomerReceiptsData.Receipts[index] = cancelled;
        return Task.FromResult(Result.Success());
    }
}
