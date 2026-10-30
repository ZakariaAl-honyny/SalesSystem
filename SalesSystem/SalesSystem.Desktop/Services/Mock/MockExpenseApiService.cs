using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة خدمة المصروفات — تعيد بيانات من MockExpensesData.
/// </summary>
public class MockExpenseApiService : IExpenseApiService
{
    public Task<Result<IReadOnlyList<ExpenseDto>>> GetAllAsync(string? search = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        IEnumerable<ExpenseDto> query = MockExpensesData.Expenses;

        if (from.HasValue)
            query = query.Where(e => e.ExpenseDate >= from.Value);

        if (to.HasValue)
            query = query.Where(e => e.ExpenseDate <= to.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.ExpenseNo.ToString().Contains(search) ||
                (e.Notes ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (e.ExpenseAccountName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (e.CashBoxName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var result = query.OrderByDescending(e => e.ExpenseDate).ThenByDescending(e => e.Id).ToList().AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<ExpenseDto>>.Success(result));
    }

    public Task<Result<ExpenseDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var expense = MockExpensesData.Expenses.FirstOrDefault(e => e.Id == id);
        return expense is null
            ? Task.FromResult(Result<ExpenseDto>.Failure("المصروف غير موجود"))
            : Task.FromResult(Result<ExpenseDto>.Success(expense));
    }

    public Task<Result<ExpenseDto>> CreateAsync(CreateExpenseRequest request, CancellationToken ct = default)
    {
        var nextId = MockExpensesData.Expenses.Count > 0
            ? MockExpensesData.Expenses.Max(e => e.Id) + 1
            : 1;

        var nextExpenseNo = MockExpensesData.Expenses.Count > 0
            ? MockExpensesData.Expenses.Max(e => e.ExpenseNo) + 1
            : 1001;

        // Resolve account name from mock accounts
        var account = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == request.ExpenseAccountId);
        var cashBox = MockData.CashBoxes.FirstOrDefault(c => c.Id == request.CashBoxId);

        var expense = new ExpenseDto(
            Id: nextId,
            ExpenseNo: nextExpenseNo,
            ExpenseDate: request.ExpenseDate,
            ExpenseAccountId: request.ExpenseAccountId,
            ExpenseAccountName: account?.NameAr,
            CashBoxId: request.CashBoxId,
            CashBoxName: cashBox?.Name,
            Amount: request.Amount,
            Notes: request.Notes,
            Status: 1,
            StatusName: "مسودة",
            IsActive: true
            // TODO: Currency / ExpenseAccountCode / BranchName removed from ExpenseDto.
        );

        MockExpensesData.Expenses.Add(expense);
        return Task.FromResult(Result<ExpenseDto>.Success(expense));
    }

    public Task<Result<ExpenseDto>> UpdateAsync(int id, UpdateExpenseRequest request, CancellationToken ct = default)
    {
        var existing = MockExpensesData.Expenses.FirstOrDefault(e => e.Id == id);
        if (existing is null)
            return Task.FromResult(Result<ExpenseDto>.Failure("المصروف غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<ExpenseDto>.Failure("لا يمكن تعديل مصروف بعد ترحيله"));

        var account = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == request.ExpenseAccountId);
        var cashBox = MockData.CashBoxes.FirstOrDefault(c => c.Id == request.CashBoxId);

        var updated = existing with
        {
            ExpenseDate = request.ExpenseDate,
            ExpenseAccountId = request.ExpenseAccountId,
            ExpenseAccountName = account?.NameAr ?? existing.ExpenseAccountName,
            CashBoxId = request.CashBoxId,
            CashBoxName = cashBox?.Name ?? existing.CashBoxName,
            Amount = request.Amount,
            Notes = request.Notes
        };

        var index = MockExpensesData.Expenses.IndexOf(existing);
        MockExpensesData.Expenses[index] = updated;
        return Task.FromResult(Result<ExpenseDto>.Success(updated));
    }

    public Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = MockExpensesData.Expenses.FirstOrDefault(e => e.Id == id);
        if (existing is null)
            return Task.FromResult(Result.Failure("المصروف غير موجود"));

        if (existing.Status == 2)
            return Task.FromResult(Result.Failure("لا يمكن حذف مصروف مرحّل. قم بإلغائه أولاً."));

        var index = MockExpensesData.Expenses.IndexOf(existing);
        MockExpensesData.Expenses[index] = existing with
        {
            Status = 3,
            StatusName = "ملغي",
            IsActive = false
        };
        return Task.FromResult(Result.Success());
    }

    public Task<Result<ExpenseDto>> PostAsync(int id, CancellationToken ct = default)
    {
        var existing = MockExpensesData.Expenses.FirstOrDefault(e => e.Id == id);
        if (existing is null)
            return Task.FromResult(Result<ExpenseDto>.Failure("المصروف غير موجود"));

        if (existing.Status != 1)
            return Task.FromResult(Result<ExpenseDto>.Failure("فقط المصروفات المسودة يمكن ترحيلها."));

        var index = MockExpensesData.Expenses.IndexOf(existing);
        MockExpensesData.Expenses[index] = existing with
        {
            Status = 2,
            StatusName = "مرحّل"
        };

        return Task.FromResult(Result<ExpenseDto>.Success(MockExpensesData.Expenses[index]));
    }

    public Task<Result<ExpenseDto>> CancelAsync(int id, CancellationToken ct = default)
    {
        var existing = MockExpensesData.Expenses.FirstOrDefault(e => e.Id == id);
        if (existing is null)
            return Task.FromResult(Result<ExpenseDto>.Failure("المصروف غير موجود"));

        if (existing.Status == 3)
            return Task.FromResult(Result<ExpenseDto>.Failure("المصروف ملغي بالفعل."));

        var index = MockExpensesData.Expenses.IndexOf(existing);
        MockExpensesData.Expenses[index] = existing with
        {
            Status = 3,
            StatusName = "ملغي",
            IsActive = false
        };

        return Task.FromResult(Result<ExpenseDto>.Success(MockExpensesData.Expenses[index]));
    }
}
