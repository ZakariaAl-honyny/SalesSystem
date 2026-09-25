using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة البنوك — تعيد بيانات من MockData.
/// </summary>
public class MockBankApiService : IBankApiService
{
    public Task<Result<IReadOnlyList<BankDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<BankDto> query = MockData.Banks;

        if (!includeInactive)
            query = query.Where(b => b.IsActive);

        return Task.FromResult(Result<IReadOnlyList<BankDto>>.Success(query.ToList().AsReadOnly()));
    }

    public Task<Result<BankDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var bank = MockData.Banks.FirstOrDefault(b => b.Id == id);
        return bank is null
            ? Task.FromResult(Result<BankDto>.Failure("البنك غير موجود"))
            : Task.FromResult(Result<BankDto>.Success(bank));
    }

    public Task<Result<BankDto>> CreateAsync(CreateBankRequest r, CancellationToken ct = default)
    {
        var nextId = MockData.Banks.Count > 0 ? MockData.Banks.Max(b => b.Id) + 1 : 1;
        var nextAccountId = MockData.Banks.Count > 0 ? MockData.Banks.Max(b => b.AccountId) + 1 : 100;
        var bank = new BankDto(
            Id: nextId,
            AccountId: nextAccountId,
            AccountName: r.Name,
            AccountCode: $"1112{nextId:D2}",
            Name: r.Name,
            AccountNumber: r.AccountNumber,
            Iban: r.Iban,
            IsActive: true,
            Currency: r.Currency ?? "YER",
            BranchId: r.BranchId,
            BranchName: null,
            ContactPerson: r.ContactPerson,
            ContactPhone: r.ContactPhone,
            OverdraftLimit: r.OverdraftLimit,
            OpeningBalance: r.OpeningBalance,
            CurrentBalance: r.OpeningBalance,
            Notes: r.Notes,
            LastReconciliationDate: null
        );
        MockData.Banks.Add(bank);
        return Task.FromResult(Result<BankDto>.Success(bank));
    }

    public Task<Result<BankDto>> UpdateAsync(int id, UpdateBankRequest r, CancellationToken ct = default)
    {
        var existing = MockData.Banks.FirstOrDefault(b => b.Id == id);
        if (existing is null)
            return Task.FromResult(Result<BankDto>.Failure("البنك غير موجود"));

        var updated = existing with
        {
            Name = r.Name,
            AccountNumber = r.AccountNumber,
            Iban = r.Iban,
            Currency = r.Currency ?? existing.Currency,
            ContactPerson = r.ContactPerson,
            ContactPhone = r.ContactPhone,
            OverdraftLimit = r.OverdraftLimit,
            Notes = r.Notes
        };

        var index = MockData.Banks.IndexOf(existing);
        MockData.Banks[index] = updated;
        return Task.FromResult(Result<BankDto>.Success(updated));
    }

    public Task<Result> DeactivateAsync(int id, CancellationToken ct = default)
    {
        var existing = MockData.Banks.FirstOrDefault(b => b.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("البنك غير موجود"));

        var index = MockData.Banks.IndexOf(existing);
        MockData.Banks[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }
}
