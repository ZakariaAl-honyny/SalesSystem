using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة سندات القبض — تعيد بيانات من MockVouchersData.
/// </summary>
public class MockReceiptVoucherApiService : IReceiptVoucherApiService
{
    public Task<Result<IReadOnlyList<ReceiptVoucherDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        IEnumerable<ReceiptVoucherDto> query = MockVouchersData.Receipts;

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

        return Task.FromResult(Result<IReadOnlyList<ReceiptVoucherDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<ReceiptVoucherDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var voucher = MockVouchersData.Receipts.FirstOrDefault(v => v.Id == id);
        return voucher is null
            ? Task.FromResult(Result<ReceiptVoucherDto>.Failure("سند القبض غير موجود"))
            : Task.FromResult(Result<ReceiptVoucherDto>.Success(voucher));
    }

    public Task<Result<ReceiptVoucherDto>> CreateAsync(CreateReceiptVoucherRequest request, CancellationToken ct = default)
    {
        var nextId = MockVouchersData.Receipts.Count > 0 ? MockVouchersData.Receipts.Max(v => v.Id) + 1 : 1;
        var nextNo = MockVouchersData.Receipts.Count > 0 ? MockVouchersData.Receipts.Max(v => v.VoucherNo) + 1 : 1001;

        var voucher = new ReceiptVoucherDto(
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
            // TODO: PaymentMethod / Currency removed from ReceiptVoucherDto.
        );

        MockVouchersData.Receipts.Add(voucher);
        return Task.FromResult(Result<ReceiptVoucherDto>.Success(voucher));
    }

    public Task<Result<ReceiptVoucherDto>> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockVouchersData.Receipts.FirstOrDefault(v => v.Id == id);
        if (existing is null)
            return Task.FromResult(Result<ReceiptVoucherDto>.Failure("سند القبض غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<ReceiptVoucherDto>.Failure("لا يمكن ترحيل السند إلا في حالة المسودة"));

        var posted = existing with { Status = 2, PostedAt = DateTime.UtcNow };
        var index = MockVouchersData.Receipts.IndexOf(existing);
        MockVouchersData.Receipts[index] = posted;
        return Task.FromResult(Result<ReceiptVoucherDto>.Success(posted));
    }

    public Task<Result> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockVouchersData.Receipts.FirstOrDefault(v => v.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("سند القبض غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result.Failure("السند ملغي بالفعل"));

        var cancelled = existing with { Status = 3, CancelledAt = DateTime.UtcNow };
        var index = MockVouchersData.Receipts.IndexOf(existing);
        MockVouchersData.Receipts[index] = cancelled;
        return Task.FromResult(Result.Success());
    }
}
