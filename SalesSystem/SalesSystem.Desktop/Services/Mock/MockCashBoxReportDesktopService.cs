using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة تقارير الصناديق النقدية — تُشتق الأرقام من مخازن بيانات التجربة فقط:
/// <c>MockData.CashBoxes</c> (الصناديق وأسماؤها)، <c>MockCustomerReceiptsData.Receipts</c>
/// (سندات القبض المرحّلة = الإيرادات) و<c>MockExpensesData.Expenses</c>
/// (المصروفات المرحّلة = النفقات) — نفس منطق خدمة التقارير في طبقة الأعمال.
/// لا تلمس قاعدة البيانات ولا أي اتصال شبكي ولا ترمي استثناءً.
/// </summary>
public class MockCashBoxReportDesktopService : ICashBoxReportDesktopService
{
    /// <summary>المستند المرحّل فقط (Draft = 1, Posted = 2, Cancelled = 3).</summary>
    private const byte Posted = 2;

    // ═══════════════════════════════════════════════
    // ملخص الصناديق النقدية حتى تاريخ محدد
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<CashBoxSummaryDto>>> GetCashBoxSummaryAsync(
        DateTime? asOfDate = null, CancellationToken ct = default)
    {
        var effectiveDate = (asOfDate ?? DateTime.Today).Date;

        var cashBoxes = MockData.CashBoxes;
        if (cashBoxes.Count == 0)
            return Task.FromResult(Result<IReadOnlyList<CashBoxSummaryDto>>.Success(
                new List<CashBoxSummaryDto>()));

        var boxIds = cashBoxes.Select(b => b.Id).ToList();

        // الإيرادات: سندات قبض العملاء المرحّلة حتى التاريخ المحدد
        var incomeByBox = MockCustomerReceiptsData.Receipts
            .Where(r => r.Status == Posted)
            .Where(r => r.IsActive)
            .Where(r => r.ReceiptDate.Date <= effectiveDate)
            .Where(r => boxIds.Contains(r.CashBoxId))
            .GroupBy(r => r.CashBoxId)
            .ToDictionary(g => g.Key, g => g.Sum(r => r.Amount));

        // النفقات: المصروفات المرحّلة حتى التاريخ المحدد
        var expenseByBox = MockExpensesData.Expenses
            .Where(e => e.Status == Posted)
            .Where(e => e.ExpenseDate.Date <= effectiveDate)
            .Where(e => boxIds.Contains(e.CashBoxId))
            .GroupBy(e => e.CashBoxId)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

        var list = cashBoxes
            .Select(box =>
            {
                var totalIncome = Money(incomeByBox.GetValueOrDefault(box.Id, 0m));
                var totalExpense = Money(expenseByBox.GetValueOrDefault(box.Id, 0m));

                return new CashBoxSummaryDto(
                    box.Id,
                    CashBoxName(box.Id, box.Name),
                    totalIncome,
                    totalExpense,
                    Money(totalIncome - totalExpense));
            })
            .OrderByDescending(d => d.NetBalance)
            .ToList();

        return Task.FromResult(Result<IReadOnlyList<CashBoxSummaryDto>>.Success(list));
    }

    // ═══════════════════════════════════════════════
    // مخازن البيانات المساعدة
    // ═══════════════════════════════════════════════

    private static string CashBoxName(int cashBoxId, string? fallback)
    {
        var name = MockData.CashBoxes.FirstOrDefault(b => b.Id == cashBoxId)?.Name;
        if (!string.IsNullOrWhiteSpace(name))
            return name;

        return !string.IsNullOrWhiteSpace(fallback) ? fallback! : "صندوق نقدي";
    }

    private static decimal Money(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
