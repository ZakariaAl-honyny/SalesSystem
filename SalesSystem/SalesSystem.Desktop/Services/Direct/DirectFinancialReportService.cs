using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة التقارير المالية — ينادي طبقة الأعمال داخل نفس العملية.
/// </summary>
public class DirectFinancialReportService : IFinancialReportApiService
{
    private const string NotAvailableMessage = "هذا التقرير غير متاح في الإصدار الحالي";

    private readonly BizInterfaces.IFinancialReportService _bll;

    public DirectFinancialReportService(BizInterfaces.IFinancialReportService bll)
    {
        _bll = bll;
    }

    public async Task<Result<IReadOnlyList<IncomeStatementDto>>> GetIncomeStatementAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetIncomeStatementAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<IncomeStatementDto>>.Success(result.Value!)
                : Result<IReadOnlyList<IncomeStatementDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<IncomeStatementDto>>.Failure("تعذر تحميل قائمة الدخل. حاول مرة أخرى.");
        }
    }

    public async Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(
        DateTime from, DateTime to, int? cashBoxId = null, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetCashFlowReportAsync(from, to, cashBoxId, ct);
        }
        catch (Exception)
        {
            return Result<CashFlowReportDto>.Failure("تعذر تحميل تقرير التدفق النقدي. حاول مرة أخرى.");
        }
    }

    public async Task<Result<IReadOnlyList<VatReportDto>>> GetVatReportAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetVatReportAsync(from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<VatReportDto>>.Success(result.Value!)
                : Result<IReadOnlyList<VatReportDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<VatReportDto>>.Failure("تعذر تحميل تقرير الضريبة. حاول مرة أخرى.");
        }
    }

    public async Task<Result<BalanceSheetDto>> GetBalanceSheetAsync(DateTime asOfDate, CancellationToken ct = default)
    {
        try
        {
            return await _bll.GetBalanceSheetAsync(asOfDate, ct);
        }
        catch (Exception)
        {
            return Result<BalanceSheetDto>.Failure("تعذر تحميل الميزانية العمومية. حاول مرة أخرى.");
        }
    }

    public async Task<Result<IReadOnlyList<TrialBalanceDto>>> GetTrialBalanceAsync(
        DateTime asOfDate, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetTrialBalanceAsync(asOfDate, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<TrialBalanceDto>>.Success(result.Value!)
                : Result<IReadOnlyList<TrialBalanceDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<TrialBalanceDto>>.Failure("تعذر تحميل ميزان المراجعة. حاول مرة أخرى.");
        }
    }

    public async Task<Result<AccountLedgerDto>> GetGeneralLedgerAsync(
        int accountId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetGeneralLedgerAsync(accountId, from, to, ct);
            return result.IsSuccess
                ? Result<AccountLedgerDto>.Success(MapToDesktop(result.Value!))
                : Result<AccountLedgerDto>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<AccountLedgerDto>.Failure("تعذر تحميل دفتر الأستاذ. حاول مرة أخرى.");
        }
    }

    public Task<Result<WorkingCapitalSummaryDto>> GetWorkingCapitalAsync(
        DateTime asOfDate, CancellationToken ct = default)
    {
        // TODO: غير مطبّق في طبقة الأعمال بعد.
        return Task.FromResult(Result<WorkingCapitalSummaryDto>.Failure(NotAvailableMessage));
    }

    public Task<Result<IReadOnlyList<AccountBalanceReportDto>>> GetAccountBalancesAsync(
        byte? accountType = null, string? search = null, CancellationToken ct = default)
    {
        // TODO: غير مطبّق في طبقة الأعمال بعد.
        return Task.FromResult(Result<IReadOnlyList<AccountBalanceReportDto>>.Failure(NotAvailableMessage));
    }

    public async Task<Result<IReadOnlyList<AccountStatementDto>>> GetCustomerAccountStatementAsync(
        int customerId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetAccountStatementAsync(customerId, from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<AccountStatementDto>>.Success(result.Value!)
                : Result<IReadOnlyList<AccountStatementDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<AccountStatementDto>>.Failure("تعذر تحميل كشف حساب العميل. حاول مرة أخرى.");
        }
    }

    public async Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierAccountStatementAsync(
        int supplierId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        try
        {
            var result = await _bll.GetSupplierStatementAsync(supplierId, from, to, ct);
            return result.IsSuccess
                ? Result<IReadOnlyList<AccountStatementDto>>.Success(result.Value!)
                : Result<IReadOnlyList<AccountStatementDto>>.Failure(result.Error!, result.ErrorCode);
        }
        catch (Exception)
        {
            return Result<IReadOnlyList<AccountStatementDto>>.Failure("تعذر تحميل كشف حساب المورد. حاول مرة أخرى.");
        }
    }

    // ─── Mapping: Business DTO → Desktop DTO ───────────────────────────────

    private static AccountLedgerDto MapToDesktop(GeneralLedgerDto src) => new(
        AccountCode: src.AccountCode,
        AccountNameAr: src.AccountName,
        OpeningBalance: src.OpeningBalance,
        Lines: src.Lines is null
            ? new List<AccountLedgerLineDto>()
            : src.Lines.Select(l => new AccountLedgerLineDto(
                Date: l.Date,
                EntryNumber: l.EntryNumber,
                Description: l.Description,
                ReferenceNumber: null, // TODO: سطر الأعمال لا يحتوي رقم مرجع
                Debit: l.Debit,
                Credit: l.Credit,
                RunningBalance: l.RunningBalance)).ToList(),
        TotalDebit: src.TotalDebit,
        TotalCredit: src.TotalCredit,
        ClosingBalance: src.ClosingBalance);
}
