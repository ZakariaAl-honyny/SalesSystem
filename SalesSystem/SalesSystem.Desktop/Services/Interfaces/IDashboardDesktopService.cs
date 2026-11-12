using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IDashboardDesktopService
{
    Task<Result<DashboardSummaryDto>> GetSummaryAsync(CancellationToken ct = default);
}

