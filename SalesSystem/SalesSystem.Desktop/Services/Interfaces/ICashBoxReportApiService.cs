using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface ICashBoxReportApiService
{
    Task<Result<IReadOnlyList<CashBoxSummaryDto>>> GetCashBoxSummaryAsync(DateTime? asOfDate = null, CancellationToken ct = default);
}
