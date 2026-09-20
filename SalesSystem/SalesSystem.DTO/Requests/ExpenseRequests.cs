namespace SalesSystem.DTO.Requests;

public record CreateExpenseRequest(
    DateTime ExpenseDate,
    int ExpenseAccountId,
    int CashBoxId,
    decimal Amount,
    string? Notes,
    short? BranchId = null
);

public record UpdateExpenseRequest(
    DateTime ExpenseDate,
    int ExpenseAccountId,
    int CashBoxId,
    decimal Amount,
    string? Notes
);
