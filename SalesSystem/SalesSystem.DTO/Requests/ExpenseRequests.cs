namespace SalesSystem.DTO.Requests;

public record CreateExpenseRequest(
    DateTime ExpenseDate,
    int CashBoxId,
    decimal Amount,
    string? Notes
);

public record UpdateExpenseRequest(
    DateTime ExpenseDate,
    int CashBoxId,
    decimal Amount,
    string? Notes
);
