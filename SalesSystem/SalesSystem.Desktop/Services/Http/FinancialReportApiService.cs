using System.Web;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Services.Http;

public sealed class FinancialReportApiService : IFinancialReportApiService
{
    private readonly HttpClientService _http;
    private const string BasePath = "api/v1/financial-reports";

    public FinancialReportApiService(HttpClientService http) => _http = http;

    public async Task<Result<IReadOnlyList<IncomeStatementDto>>> GetIncomeStatementAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<IncomeStatementDto>($"{BasePath}/income-statement?{query}", ct);
    }

    public async Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(DateTime from, DateTime to, int? cashBoxId, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        if (cashBoxId.HasValue) query["cashBoxId"] = cashBoxId.Value.ToString();
        return await _http.GetAsync<CashFlowReportDto>($"{BasePath}/cash-flow?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<VatReportDto>>> GetVatReportAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<VatReportDto>($"{BasePath}/vat-report?{query}", ct);
    }

    public async Task<Result<BalanceSheetDto>> GetBalanceSheetAsync(DateTime asOfDate, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["asOfDate"] = asOfDate.ToString("yyyy-MM-dd");
        return await _http.GetAsync<BalanceSheetDto>($"{BasePath}/balance-sheet?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<TrialBalanceDto>>> GetTrialBalanceAsync(DateTime asOfDate, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["asOfDate"] = asOfDate.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<TrialBalanceDto>($"{BasePath}/trial-balance?{query}", ct);
    }

    public async Task<Result<AccountLedgerDto>> GetGeneralLedgerAsync(int accountId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetAsync<AccountLedgerDto>($"{BasePath}/general-ledger/{accountId}?{query}", ct);
    }

    public async Task<Result<WorkingCapitalSummaryDto>> GetWorkingCapitalAsync(DateTime asOfDate, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["asOfDate"] = asOfDate.ToString("yyyy-MM-dd");
        return await _http.GetAsync<WorkingCapitalSummaryDto>($"{BasePath}/working-capital?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<AccountBalanceReportDto>>> GetAccountBalancesAsync(byte? accountType, string? search, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (accountType.HasValue) query["accountType"] = accountType.Value.ToString();
        if (!string.IsNullOrEmpty(search)) query["search"] = search;
        return await _http.GetListAsync<AccountBalanceReportDto>($"{BasePath}/account-balances?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<AccountStatementDto>>> GetCustomerAccountStatementAsync(int customerId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<AccountStatementDto>($"{BasePath}/account-statement/customer/{customerId}?{query}", ct);
    }

    public async Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierAccountStatementAsync(int supplierId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["from"] = from.ToString("yyyy-MM-dd");
        query["to"] = to.ToString("yyyy-MM-dd");
        return await _http.GetListAsync<AccountStatementDto>($"{BasePath}/account-statement/supplier/{supplierId}?{query}", ct);
    }
}
