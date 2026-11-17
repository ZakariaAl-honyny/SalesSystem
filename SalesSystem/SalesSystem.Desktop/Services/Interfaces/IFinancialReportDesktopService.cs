using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Interfaces;

/// <summary>
/// عقد التقارير المالية الباقية في V1 — خمسة تقارير فقط، وكلها مشتقة من المستندات التشغيلية:
/// قائمة الدخل · تقرير ضريبة القيمة المضافة · التدفق النقدي · كشف حساب العميل · كشف حساب المورد.
/// </summary>
/// <remarks>
/// أُخرجت الوحدة المحاسبية من V1 (قرار المالك 2026-11-17 — «remove all Accounting code»):
/// أُزيلت الميزانية العمومية · ميزان المراجعة · دفتر الأستاذ · رأس المال العامل · أرصدة الحسابات ·
/// وأسماء كشوف الحسابات البديلة. فلا حساب دفتر ولا قيد يومية في هذا العقد.
/// </remarks>
public interface IFinancialReportDesktopService
{
    Task<Result<IReadOnlyList<IncomeStatementDto>>> GetIncomeStatementAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<VatReportDto>>> GetVatReportAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(DateTime from, DateTime to, int? cashBoxId = null, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountStatementDto>>> GetAccountStatementAsync(int customerId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierStatementAsync(int supplierId, DateTime from, DateTime to, CancellationToken ct = default);
}
