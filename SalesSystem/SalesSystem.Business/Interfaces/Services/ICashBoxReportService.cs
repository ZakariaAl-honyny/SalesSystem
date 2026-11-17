using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Business.Interfaces.Services;

/// <summary>
/// Service for cash box summary reports.
/// Summary is computed from operational documents: sales, receipts, payments, and expenses.
/// </summary>
public interface ICashBoxReportService
{
    /// <summary>
    /// Gets cash box summary showing balance information per cash box.
    /// </summary>
    Task<Result<List<CashBoxSummaryDto>>> GetCashBoxSummaryAsync(DateTime? asOfDate = null, CancellationToken ct = default);
}
