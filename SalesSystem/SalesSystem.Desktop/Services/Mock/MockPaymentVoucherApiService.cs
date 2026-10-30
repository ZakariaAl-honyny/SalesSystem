using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة سندات الصرف — تعيد بيانات من MockVouchersData.
/// </summary>
public class MockPaymentVoucherApiService : IPaymentVoucherApiService
{
    public Task<Result<IReadOnlyList<PaymentVoucherDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        IEnumerable<PaymentVoucherDto> query = MockVouchersData.Payments;

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(v =>
                v.VoucherNo.ToString().Contains(search) ||
                (v.AccountName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (v.CashBoxName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (v.Notes ?? "").Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (from.HasValue)
            query = query.Where(v => v.VoucherDate >= from.Value);

        if (to.HasValue)
            query = query.Where(v => v.VoucherDate <= to.Value);

        return Task.FromResult(Result<IReadOnlyList<PaymentVoucherDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<PaymentVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var voucher = MockVouchersData.Payments.FirstOrDefault(v => v.Id == id);
        return voucher is null
            ? Task.FromResult(Result<PaymentVoucherDto>.Failure("سند الصرف غير موجود"))
            : Task.FromResult(Result<PaymentVoucherDto>.Success(voucher));
    }

    public Task<Result<PaymentVoucherDto>> CreateAsync(CreatePaymentVoucherRequest request, CancellationToken ct = default)
    {
        var nextId = MockVouchersData.Payments.Count > 0 ? MockVouchersData.Payments.Max(v => v.Id) + 1 : 1;
        var nextNo = MockVouchersData.Payments.Count > 0 ? MockVouchersData.Payments.Max(v => v.VoucherNo) + 1 : 2001;

        var voucher = new PaymentVoucherDto(
            Id: nextId,
            VoucherNo: nextNo,
            VoucherDate: request.VoucherDate,
            CashBoxId: request.CashBoxId,
            CashBoxName: null,
            AccountId: request.AccountId,
            AccountName: null,
            TotalAmount: request.TotalAmount,
            Notes: request.Notes,
            Status: 1, // Draft
            CreatedAt: DateTime.UtcNow,
            PostedAt: null,
            CancelledAt: null
            // TODO: PaymentMethod / Currency removed from PaymentVoucherDto.
        );

        MockVouchersData.Payments.Add(voucher);
        return Task.FromResult(Result<PaymentVoucherDto>.Success(voucher));
    }

    public Task<Result<PaymentVoucherDto>> UpdateAsync(int id, UpdatePaymentVoucherRequest request, CancellationToken ct = default)
    {
        var existing = MockVouchersData.Payments.FirstOrDefault(v => v.Id == id);
        if (existing is null)
            return Task.FromResult(Result<PaymentVoucherDto>.Failure("سند الصرف غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<PaymentVoucherDto>.Failure("لا يمكن تعديل سند الصرف إلا في حالة المسودة"));

        var updated = existing with
        {
            VoucherDate = request.VoucherDate ?? existing.VoucherDate,
            Notes = request.Notes ?? existing.Notes
        };

        var index = MockVouchersData.Payments.IndexOf(existing);
        MockVouchersData.Payments[index] = updated;
        return Task.FromResult(Result<PaymentVoucherDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockVouchersData.Payments.FirstOrDefault(v => v.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("سند الصرف غير موجود"));

        if (existing.Status == 2)
            return Task.FromResult(Result.Failure("لا يمكن حذف سند صرف مرحّل. قم بإلغائه أولاً."));

        var cancelled = existing with { Status = 3, CancelledAt = DateTime.UtcNow };
        var index = MockVouchersData.Payments.IndexOf(existing);
        MockVouchersData.Payments[index] = cancelled;
        return Task.FromResult(Result.Success());
    }

    public Task<Result<PaymentVoucherDto>> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockVouchersData.Payments.FirstOrDefault(v => v.Id == id);
        if (existing is null)
            return Task.FromResult(Result<PaymentVoucherDto>.Failure("سند الصرف غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<PaymentVoucherDto>.Failure("لا يمكن ترحيل السند إلا في حالة المسودة"));

        var posted = existing with { Status = 2, PostedAt = DateTime.UtcNow };
        var index = MockVouchersData.Payments.IndexOf(existing);
        MockVouchersData.Payments[index] = posted;
        return Task.FromResult(Result<PaymentVoucherDto>.Success(posted));
    }

    public Task<Result<PaymentVoucherDto>> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockVouchersData.Payments.FirstOrDefault(v => v.Id == id);
        if (existing is null)
            return Task.FromResult(Result<PaymentVoucherDto>.Failure("سند الصرف غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result<PaymentVoucherDto>.Failure("السند ملغي بالفعل"));

        var cancelled = existing with { Status = 3, CancelledAt = DateTime.UtcNow };
        var index = MockVouchersData.Payments.IndexOf(existing);
        MockVouchersData.Payments[index] = cancelled;
        return Task.FromResult(Result<PaymentVoucherDto>.Success(cancelled));
    }
}
