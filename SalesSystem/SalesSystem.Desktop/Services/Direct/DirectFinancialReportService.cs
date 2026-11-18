using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;

namespace SalesSystem.Desktop.Services.Direct;

/// <summary>
/// غلاف مباشر لخدمة التقارير المالية الباقية في V1 — ينادي طبقة الأعمال داخل نفس العملية.
/// أربعة تقارير فقط: قائمة الدخل · التدفق النقدي · كشف حساب العميل · كشف حساب المورد.
/// </summary>
/// <remarks>
/// كشوف تشغيلية مبنية على المستندات (فواتير · مرتجعات · سندات قبض · دفعات موردين):
/// قائمة الدخل · التدفق النقدي · كشف حساب العميل · كشف حساب المورّد.
/// </remarks>
public class DirectFinancialReportService : IFinancialReportDesktopService
{
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

    public async Task<Result<IReadOnlyList<AccountStatementDto>>> GetAccountStatementAsync(
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

    public async Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierStatementAsync(
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
}
