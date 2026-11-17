namespace SalesSystem.DTO.Responses;

public record ExpenseDto(
    int Id,
    int ExpenseNo,
    DateTime ExpenseDate,
    int CashBoxId,
    string? CashBoxName,
    decimal Amount,
    string? Notes,
    byte Status,
    string? StatusName,
    bool IsActive
);
