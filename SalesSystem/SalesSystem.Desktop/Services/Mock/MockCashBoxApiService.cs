using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة الصناديق النقدية — تعيد بيانات من MockData.
/// </summary>
public class MockCashBoxApiService : ICashBoxApiService
{
    public Task<Result<IReadOnlyList<CashBoxDto>>> GetAllAsync(CancellationToken ct = default)
    {
        IEnumerable<CashBoxDto> query = MockData.CashBoxes;
        return Task.FromResult(Result<IReadOnlyList<CashBoxDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<CashBoxDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var box = MockData.CashBoxes.FirstOrDefault(c => c.Id == id);
        return box is null
            ? Task.FromResult(Result<CashBoxDto>.Failure("الصندوق النقدي غير موجود"))
            : Task.FromResult(Result<CashBoxDto>.Success(box));
    }

    public Task<Result<CashBoxDto>> CreateAsync(CreateCashBoxRequest r, CancellationToken ct = default)
    {
        var nextId = MockData.CashBoxes.Count > 0 ? MockData.CashBoxes.Max(c => c.Id) + 1 : 1;
        var nextAccountId = MockData.CashBoxes.Count > 0 ? MockData.CashBoxes.Max(c => c.AccountId) + 1 : 200;
        var box = new CashBoxDto(
            Id: nextId,
            Name: r.Name,
            AccountId: nextAccountId,
            AccountName: r.Name,
            AccountCode: $"1111{nextId:D2}",
            Description: r.Description,
            IsActive: true,
            BranchId: r.BranchId,
            BranchName: null,
            Code: r.Code,
            Currency: r.Currency ?? "YER",
            CashierName: null,
            CashierId: r.CashierId,
            MaxCashLimit: r.MaxCashLimit,
            OpeningBalance: r.OpeningBalance,
            CurrentBalance: r.OpeningBalance,
            LastMovementDate: null
        );
        MockData.CashBoxes.Add(box);
        return Task.FromResult(Result<CashBoxDto>.Success(box));
    }

    public Task<Result<CashBoxDto>> UpdateAsync(int id, UpdateCashBoxRequest r, CancellationToken ct = default)
    {
        var existing = MockData.CashBoxes.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result<CashBoxDto>.Failure("الصندوق النقدي غير موجود"));

        var updated = existing with
        {
            Name = r.Name,
            Description = r.Description,
            Code = r.Code ?? existing.Code,
            Currency = r.Currency ?? existing.Currency,
            CashierId = r.CashierId ?? existing.CashierId,
            MaxCashLimit = r.MaxCashLimit
        };

        var index = MockData.CashBoxes.IndexOf(existing);
        MockData.CashBoxes[index] = updated;
        return Task.FromResult(Result<CashBoxDto>.Success(updated));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.CashBoxes.FirstOrDefault(c => c.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الصندوق النقدي غير موجود"));

        var index = MockData.CashBoxes.IndexOf(existing);
        MockData.CashBoxes[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> TransferAsync(CashTransferRequest request, CancellationToken ct = default)
    {
        // محاكاة بسيطة للتحويل — لا حاجة لتحديث الأرصدة في وضع التجربة
        return Task.FromResult(Result.Success());
    }
}
