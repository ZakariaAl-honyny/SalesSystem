using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// عقد التقارير المالية الباقية في V1 — أربعة تقارير فقط، وكلها مشتقة من المستندات التشغيلية:
/// قائمة الدخل · التدفق النقدي · كشف حساب العميل · كشف حساب المورد.
/// </summary>
/// <remarks>
/// كشوف تشغيلية مبنية على المستندات (فواتير · مرتجعات · سندات قبض · دفعات موردين):
/// قائمة الدخل · التدفق النقدي · كشف حساب العميل · كشف حساب المورّد.
/// </remarks>
public interface IFinancialReportDesktopService
{
    Task<Result<IReadOnlyList<IncomeStatementDto>>> GetIncomeStatementAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(DateTime from, DateTime to, int? cashBoxId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountStatementDto>>> GetAccountStatementAsync(int customerId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierStatementAsync(int supplierId, DateTime from, DateTime to, CancellationToken ct = default);
}
