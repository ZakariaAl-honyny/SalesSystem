using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

public interface IFinancialReportApiService
{
    Task<Result<IReadOnlyList<IncomeStatementDto>>> GetIncomeStatementAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(DateTime from, DateTime to, int? cashBoxId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<VatReportDto>>> GetVatReportAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<BalanceSheetDto>> GetBalanceSheetAsync(DateTime asOfDate, CancellationToken ct = default);
    Task<Result<IReadOnlyList<TrialBalanceDto>>> GetTrialBalanceAsync(DateTime asOfDate, CancellationToken ct = default);
    Task<Result<AccountLedgerDto>> GetGeneralLedgerAsync(int accountId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<WorkingCapitalSummaryDto>> GetWorkingCapitalAsync(DateTime asOfDate, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountBalanceReportDto>>> GetAccountBalancesAsync(byte? accountType = null, string? search = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountStatementDto>>> GetCustomerAccountStatementAsync(int customerId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierAccountStatementAsync(int supplierId, DateTime from, DateTime to, CancellationToken ct = default);
}
