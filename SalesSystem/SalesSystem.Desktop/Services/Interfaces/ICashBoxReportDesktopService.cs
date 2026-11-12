using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ICashBoxReportDesktopService
{
    Task<Result<IReadOnlyList<CashBoxSummaryDto>>> GetCashBoxSummaryAsync(DateTime? asOfDate = null, CancellationToken ct = default);
}
