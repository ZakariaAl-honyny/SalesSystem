namespace SalesSystem.DTO;

public record AccountStatementDto(
    DateTime Date,
    string Description,
    string ReferenceNumber,
    decimal Debit,
    decimal Credit,
    decimal Balance
);
