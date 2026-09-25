using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة تحويلات الخزينة — تعيد بيانات من MockTreasuryTransfersData.
/// </summary>
public class MockTreasuryTransferApiService : ITreasuryTransferApiService
{
    public Task<Result<IReadOnlyList<TreasuryTransferDto>>> GetAllAsync(
        bool includeInactive = false,
        string? search = null,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default)
    {
        IEnumerable<TreasuryTransferDto> query = MockTreasuryTransfersData.Transfers;

        if (!includeInactive)
            query = query.Where(t => t.Status != 3);

        if (from.HasValue)
            query = query.Where(t => t.TransferDate >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.TransferDate <= to.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t =>
                t.TransferNo.ToString().Contains(search) ||
                (t.FromCashBoxName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (t.ToCashBoxName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (t.Notes ?? "").Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var result = query.OrderByDescending(t => t.TransferDate)
                          .ThenByDescending(t => t.Id)
                          .ToList()
                          .AsReadOnly();

        return Task.FromResult(Result<IReadOnlyList<TreasuryTransferDto>>.Success(result));
    }

    public Task<Result<TreasuryTransferDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var transfer = MockTreasuryTransfersData.Transfers.FirstOrDefault(t => t.Id == id);
        return transfer is null
            ? Task.FromResult(Result<TreasuryTransferDto>.Failure("التحويل غير موجود"))
            : Task.FromResult(Result<TreasuryTransferDto>.Success(transfer));
    }

    public Task<Result<TreasuryTransferDto>> CreateAsync(TreasuryTransferDto dto, CancellationToken ct = default)
    {
        if (dto.FromCashBoxId == dto.ToCashBoxId && dto.FromCashBoxId.HasValue)
            return Task.FromResult(Result<TreasuryTransferDto>.Failure("لا يمكن التحويل لنفس الصندوق"));

        if (dto.Amount <= 0)
            return Task.FromResult(Result<TreasuryTransferDto>.Failure("المبلغ يجب أن يكون أكبر من صفر"));

        var nextId = MockTreasuryTransfersData.Transfers.Count > 0
            ? MockTreasuryTransfersData.Transfers.Max(t => t.Id) + 1
            : 1;

        var nextNo = MockTreasuryTransfersData.Transfers.Count > 0
            ? MockTreasuryTransfersData.Transfers.Max(t => t.TransferNo) + 1
            : 3001;

        var transfer = dto with
        {
            Id = nextId,
            TransferNo = nextNo,
            Status = 1 // مسودة
        };

        MockTreasuryTransfersData.Transfers.Add(transfer);
        return Task.FromResult(Result<TreasuryTransferDto>.Success(transfer));
    }

    public Task<Result<TreasuryTransferDto>> UpdateAsync(int id, TreasuryTransferDto dto, CancellationToken ct = default)
    {
        var existing = MockTreasuryTransfersData.Transfers.FirstOrDefault(t => t.Id == id);
        if (existing is null)
            return Task.FromResult(Result<TreasuryTransferDto>.Failure("التحويل غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<TreasuryTransferDto>.Failure("لا يمكن تعديل التحويل إلا في حالة المسودة"));

        if (dto.Amount <= 0)
            return Task.FromResult(Result<TreasuryTransferDto>.Failure("المبلغ يجب أن يكون أكبر من صفر"));

        var updated = dto with { Id = id, TransferNo = existing.TransferNo, Status = 1 };
        var index = MockTreasuryTransfersData.Transfers.IndexOf(existing);
        MockTreasuryTransfersData.Transfers[index] = updated;

        return Task.FromResult(Result<TreasuryTransferDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockTreasuryTransfersData.Transfers.FirstOrDefault(t => t.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("التحويل غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result.Failure("لا يمكن حذف التحويل إلا في حالة المسودة"));

        MockTreasuryTransfersData.Transfers.Remove(existing);
        return Task.FromResult(Result.Success());
    }
}
