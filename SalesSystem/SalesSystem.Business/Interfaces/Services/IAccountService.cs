using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Business.Interfaces.Services;

public interface IAccountService
{
    Task<Result<List<AccountTreeNodeDto>>> GetTreeAsync(CancellationToken ct = default);
    Task<Result<List<AccountDto>>> GetAllAsync(bool includeInactive = false, CancellationToken ct = default);
    Task<Result<AccountDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<List<AccountDto>>> GetByTypeAsync(AccountType type, CancellationToken ct = default);
    Task<Result<AccountDto>> CreateAsync(CreateAccountRequest request, int userId, CancellationToken ct = default);
    Task<Result<AccountDto>> UpdateAsync(int id, UpdateAccountRequest request, int userId, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    Task<Result> PermanentDeleteAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// يعيد معاينة كود الحساب الفرعي التالي تحت حساب أب (مع مستواه ومسار شجرته) قبل الحفظ.
    /// </summary>
    Task<Result<AccountCodePreviewDto>> GetNextChildCodeAsync(int parentId, CancellationToken ct = default);
}
