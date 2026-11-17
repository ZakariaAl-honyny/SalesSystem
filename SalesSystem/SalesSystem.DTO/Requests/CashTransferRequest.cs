namespace SalesSystem.DTO.Requests;

/// <summary>
/// Request to transfer cash between two cash boxes.
/// Inter-box transfer has no mechanism in V1: no voucher or journal-entry machinery exists
/// (accounting is deferred to V2), so the request is always refused.
/// </summary>
public record CashTransferRequest(
    int SourceCashBoxId,
    int DestinationCashBoxId,
    decimal Amount,
    string? Notes = null);
