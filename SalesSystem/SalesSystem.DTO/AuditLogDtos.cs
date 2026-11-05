namespace SalesSystem.DTO;

public record AuditLogDto(long Id, int? UserId, string? UserName, string Action,
    string? EntityType, int? EntityId, string? OldValues, string? NewValues,
    string? ChangedColumns, string? IpAddress, DateTime Timestamp);

// ─── Audit ────────────────────────────────────────────

public record AuditLogQuery
{
    public int? UserId { get; init; }
    public string? Action { get; init; }
    public string? EntityType { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

// ─── System Log DTO ────────────────────────────────
public record SystemLogDto(
    long Id,
    byte Level,
    string Message,
    string? Exception,
    string? Source,
    string? ActionName,
    string? IpAddress,
    int? UserId,
    string? UserName,
    DateTime CreatedAt)
{
    public string LogLevel => Level switch
    {
        1 => "معلومات",
        2 => "تحذير",
        3 => "خطأ",
        4 => "حرج",
        _ => "غير معروف"
    };
}
