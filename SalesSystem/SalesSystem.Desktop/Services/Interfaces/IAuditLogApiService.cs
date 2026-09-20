using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IAuditLogApiService
{
    Task<Result<PaginatedResult<AuditLogDto>>> QueryAsync(AuditLogQuery query, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AuditLogDto>>> GetLoginHistoryAsync(int? userId = null, CancellationToken ct = default);
}
