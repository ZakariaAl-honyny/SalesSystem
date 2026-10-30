using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة دليل الحسابات — تعيد بيانات من MockAccountsData.
/// </summary>
public class MockAccountApiService : IAccountApiService
{
    public Task<Result<IReadOnlyList<AccountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default)
    {
        IEnumerable<AccountDto> query = MockAccountsData.Accounts;

        if (!includeInactive)
            query = query.Where(a => a.IsActive);

        return Task.FromResult(Result<IReadOnlyList<AccountDto>>.Success(query.OrderBy(a => a.AccountCode).ToList().AsReadOnly()));
    }

    public Task<Result<AccountDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var account = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        return account is null
            ? Task.FromResult(Result<AccountDto>.Failure("الحساب غير موجود"))
            : Task.FromResult(Result<AccountDto>.Success(account));
    }

    public Task<Result<IReadOnlyList<AccountTreeNodeDto>>> GetTreeAsync(CancellationToken ct = default)
    {
        var all = MockAccountsData.Accounts;
        var roots = all.Where(a => a.ParentId == null && a.IsActive)
            .OrderBy(a => a.AccountCode)
            .Select(a => BuildTreeNode(a, all))
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<AccountTreeNodeDto>>.Success(roots.AsReadOnly()));
    }

    private static AccountTreeNodeDto BuildTreeNode(AccountDto account, IReadOnlyList<AccountDto> all)
    {
        return new AccountTreeNodeDto(
            account.Id,
            account.AccountCode,
            account.NameAr,
            account.Nature,
            account.IsLeaf,
            account.CategoryId,
            account.Level,
            account.ColorCode,
            account.Description,
            all.Where(c => c.ParentId == account.Id && c.IsActive)
                .OrderBy(c => c.AccountCode)
                .Select(c => BuildTreeNode(c, all))
                .ToList()
        );
    }

    public Task<Result<int>> CreateAsync(CreateAccountRequest request, CancellationToken ct = default)
    {
        var nextId = MockAccountsData.Accounts.Count > 0
            ? MockAccountsData.Accounts.Max(a => a.Id) + 1
            : 1;

        // Determine level from parent
        byte level = 1;
        string? parentCode = null;
        string? parentName = null;
        if (request.ParentId.HasValue)
        {
            var parent = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == request.ParentId.Value);
            if (parent is not null)
            {
                level = (byte)(parent.Level + 1);
                parentCode = parent.AccountCode;
                parentName = parent.NameAr;
            }
        }

        var account = new AccountDto(
            Id: nextId,
            AccountCode: $"{parentCode ?? ""}{nextId:D2}",
            NameAr: request.NameAr,
            NameEn: request.NameEn,
            Nature: request.Nature,
            IsLeaf: request.IsLeaf,
            ParentId: request.ParentId,
            ParentAccountName: parentName,
            IsSystem: request.IsSystem,
            IsActive: true,
            CategoryId: request.CategoryId,
            Level: level,
            Description: request.Description,
            ColorCode: null,
            Notes: request.Notes);
        // TODO: ParentAccountCode / FinancialStatement / TreePath / AllowCostCenter / CurrentBalance
        //       removed from AccountDto — القيم تُشتق من شجرة الحسابات عند الحاجة.

        MockAccountsData.Accounts.Add(account);
        return Task.FromResult(Result<int>.Success(account.Id));
    }

    public Task<Result> UpdateAsync(int id, UpdateAccountRequest request, CancellationToken ct = default)
    {
        var existing = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الحساب غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result.Failure("لا يمكن تعديل حساب نظامي"));

        var updated = existing with
        {
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            Nature = request.Nature,
            IsLeaf = request.IsLeaf,
            ParentId = request.ParentId,
            CategoryId = request.CategoryId,
            Description = request.Description,
            Notes = request.Notes
        };

        var index = MockAccountsData.Accounts.IndexOf(existing);
        MockAccountsData.Accounts[index] = updated;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الحساب غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب نظامي"));

        var hasChildren = MockAccountsData.Accounts.Any(a => a.ParentId == id && a.IsActive);
        if (hasChildren)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب رئيسي لديه حسابات فرعية"));

        var index = MockAccountsData.Accounts.IndexOf(existing);
        MockAccountsData.Accounts[index] = existing with { IsActive = false };
        return Task.FromResult(Result.Success());
    }

    public Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("الحساب غير موجود"));

        if (existing.IsSystem)
            return Task.FromResult(Result.Failure("لا يمكن حذف حساب نظامي"));

        MockAccountsData.Accounts.Remove(existing);
        return Task.FromResult(Result.Success());
    }
}
