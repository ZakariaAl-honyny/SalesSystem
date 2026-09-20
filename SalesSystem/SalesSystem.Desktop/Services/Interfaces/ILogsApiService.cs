using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ILogsApiService
{
    Task<Result<IReadOnlyList<SystemLogDto>>> GetAllAsync(byte? level = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
}
