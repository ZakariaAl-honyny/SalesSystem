using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة التقارير المالية الباقية في V1 — أربعة تقارير فقط:
/// <b>قائمة الدخل</b> · <b>التدفق النقدي</b> ·
/// <b>كشف حساب العميل</b> · <b>كشف حساب المورد</b>.
/// </summary>
/// <remarks>
/// أُخرجت الوحدة المحاسبية من V1 (قرار المالك 2026-11-17 — «remove all Accounting code»):
/// لا دليل حسابات · لا قيود يومية · لا ميزان مراجعة · لا دفتر أستاذ · لا ميزانية عمومية ·
/// لا أرصدة حسابات · لا رأس مال عامل ولا كشوف حسابات دفترية.
/// كل رقم هنا يُشتق من المستندات التشغيلية نفسها — فواتير البيع والشراء والمرتجعات والمصروفات
/// وسندات القبض والصرف وصندوقها النقدي — لا من قيد مزدوج ولا من دليل حسابات.
/// لا تلمس قاعدة البيانات ولا أي اتصال شبكي ولا ترمي استثناءً.
/// </remarks>
public class MockFinancialReportDesktopService : IFinancialReportDesktopService
{
    /// <summary>المستند المرحّل فقط (Draft = 1, Posted = 2, Cancelled = 3).</summary>
    private const byte Posted = 2;

    // ─── مجموعات التدفق النقدي ───
    private const string SalesCollectionGroup = "تحصيلات فواتير البيع";
    private const string SalesRefundGroup = "مردودات نقدية للعملاء";
    private const string PurchasePaymentGroup = "مدفوعات فواتير الشراء";
    private const string ExpenseCashGroup = "مصروفات نقدية";

    // ─── تسميات قائمة الدخل ───
    private const string RevenueCategory = "إيرادات";
    private const string CogsCategory = "تكلفة المبيعات";
    private const string ExpenseCategory = "مصروفات";
    private const string SalesRevenueLine = "مبيعات الفواتير المرحّلة";
    private const string SalesReturnsLine = "مردودات المبيعات";
    private const string CogsLine = "تكلفة البضاعة المباعة";
    private const string OperatingExpensesLine = "المصروفات التشغيلية المرحّلة";
    private const string RevenueTotalLabel = "إجمالي الإيرادات";
    private const string ExpenseTotalLabel = "إجمالي المصروفات";
    private const string NetProfitCategory = "أرباح";
    private const string NetLossCategory = "خسائر";
    private const string NetProfitLine = "صافي الربح";
    private const string NetLossLine = "صافي الخسارة";

    // ─── رسائل عربية ───
    private const string DateRangeError = "تاريخ البداية يجب أن يكون قبل تاريخ النهاية";
    private const string CashBoxNotFound = "الصندوق غير موجود";
    private const string InvalidCustomer = "معرف العميل غير صالح";
    private const string InvalidSupplier = "معرف المورد غير صالح";
    private const string CustomerNotFound = "العميل غير موجود";
    private const string SupplierNotFound = "المورد غير موجود";
    private const string IncomeStatementError = "تعذر تحميل قائمة الدخل. حاول مرة أخرى.";
    private const string CashFlowError = "تعذر تحميل تقرير التدفق النقدي. حاول مرة أخرى.";
    private const string CustomerStatementError = "تعذر تحميل كشف حساب العميل. حاول مرة أخرى.";
    private const string SupplierStatementError = "تعذر تحميل كشف حساب المورد. حاول مرة أخرى.";

    /// <summary>حركة نقدية واحدة: مبلغ داخل أو خارج الصندوق، مع مجموعة العرض والصندوق.</summary>
    private sealed record CashMovement(
        DateTime Date,
        string Group,
        decimal CashIn,
        decimal CashOut,
        int? CashBoxId);

    // ═══════════════════════════════════════════════
    // 1) قائمة الدخل — من مستندات البيع والمرتجعات والمصروفات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<IncomeStatementDto>>> GetIncomeStatementAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<IncomeStatementDto>>.Failure(DateRangeError));

        try
        {
            // بند الإيراد: صافي مبيعات الفواتير المرحّلة (SubTotal − الخصم)
            var sales = MockData.Invoices
                .Where(i => i.Status == Posted)
                .Where(i => i.InvoiceDate.Date >= from.Date && i.InvoiceDate.Date <= to.Date)
                .ToList();

            var revenue = Money(sales.Sum(i => i.SubTotal - i.DiscountAmount));

            var salesReturns = MockReturnsData.SalesReturns
                .Where(r => r.Status == Posted)
                .Where(r => r.ReturnDate.Date >= from.Date && r.ReturnDate.Date <= to.Date)
                .ToList();

            var salesReturnsAmount = Money(salesReturns.Sum(r => r.TotalAmount));
            var totalRevenue = Money(revenue - salesReturnsAmount);

            // ─── تكلفة المبيعات: تكلفة سطور الفواتير المرحّلة ناقص تكلفة البضاعة المرتجعة ───
            var cogs = Money(
                sales.SelectMany(i => i.Items).Sum(LineCost) -
                salesReturns.SelectMany(r => r.Items).Sum(UnitCostOfLine));

            // ─── المصروفات التشغيلية المرحّلة ───
            var expenses = Money(MockExpensesData.Expenses
                .Where(e => e.Status == Posted)
                .Where(e => e.ExpenseDate.Date >= from.Date && e.ExpenseDate.Date <= to.Date)
                .Sum(e => e.Amount));

            var netResult = Money(totalRevenue - cogs - expenses);

            var rows = new List<IncomeStatementDto>
            {
                new(RevenueCategory, SalesRevenueLine, revenue)
            };

            if (salesReturnsAmount != 0m)
                rows.Add(new IncomeStatementDto(RevenueCategory, SalesReturnsLine, -salesReturnsAmount));

            rows.Add(new IncomeStatementDto(RevenueCategory, RevenueTotalLabel, totalRevenue));
            rows.Add(new IncomeStatementDto(CogsCategory, CogsLine, cogs));
            rows.Add(new IncomeStatementDto(ExpenseCategory, OperatingExpensesLine, expenses));
            rows.Add(new IncomeStatementDto(ExpenseCategory, ExpenseTotalLabel, expenses));

            rows.Add(netResult >= 0m
                ? new IncomeStatementDto(NetProfitCategory, NetProfitLine, netResult)
                : new IncomeStatementDto(NetLossCategory, NetLossLine, netResult));

            return Task.FromResult(Result<IReadOnlyList<IncomeStatementDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<IncomeStatementDto>>.Failure(IncomeStatementError));
        }
    }

    // ═══════════════════════════════════════════════
    // 2) التدفق النقدي (حركة الصندوق التشغيلي)
    // ═══════════════════════════════════════════════
    public Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(
        DateTime from, DateTime to, int? cashBoxId = null, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<CashFlowReportDto>.Failure(DateRangeError));

        try
        {
            if (cashBoxId.HasValue && MockData.CashBoxes.All(b => b.Id != cashBoxId.Value))
                return Task.FromResult(Result<CashFlowReportDto>.Failure(CashBoxNotFound));

            var movements = BuildCashMovements()
                .Where(m => cashBoxId is null || m.CashBoxId == cashBoxId)
                .ToList();

            var before = movements.Where(m => m.Date < from.Date).ToList();
            var period = movements.Where(m => m.Date >= from.Date && m.Date <= to.Date).ToList();

            var openingBalance = Money(before.Sum(m => m.CashIn - m.CashOut));
            var totalIncome = Money(period.Sum(m => m.CashIn));
            var totalExpense = Money(period.Sum(m => m.CashOut));
            var netCashFlow = Money(totalIncome - totalExpense);

            var incomeItems = period
                .Where(m => m.CashIn != 0m)
                .GroupBy(m => m.Group)
                .Select(g => new CashFlowItemDto(g.Key, Money(g.Sum(m => m.CashIn))))
                .OrderByDescending(i => i.Amount)
                .ToList();

            var expenseItems = period
                .Where(m => m.CashOut != 0m)
                .GroupBy(m => m.Group)
                .Select(g => new CashFlowItemDto(g.Key, Money(g.Sum(m => m.CashOut))))
                .OrderByDescending(i => i.Amount)
                .ToList();

            var report = new CashFlowReportDto(
                OpeningBalance: openingBalance,
                TotalIncome: totalIncome,
                TotalExpense: totalExpense,
                NetCashFlow: netCashFlow,
                ClosingBalance: Money(openingBalance + netCashFlow),
                IncomeItems: incomeItems,
                ExpenseItems: expenseItems);

            return Task.FromResult(Result<CashFlowReportDto>.Success(report));
        }
        catch
        {
            return Task.FromResult(Result<CashFlowReportDto>.Failure(CashFlowError));
        }
    }

    // ═══════════════════════════════════════════════
    // 3) كشف حساب العميل — من مستنداته فقط
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<AccountStatementDto>>> GetAccountStatementAsync(
        int customerId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (customerId <= 0)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(InvalidCustomer));

        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(DateRangeError));

        try
        {
            if (MockData.Customers.All(c => c.Id != customerId))
                return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(CustomerNotFound));

            var entries = new List<AccountStatementDto>();

            // ─── مدين: فواتير البيع المرحّلة ───
            foreach (var invoice in MockData.Invoices
                         .Where(i => i.CustomerId == customerId && i.Status == Posted)
                         .Where(i => i.InvoiceDate.Date >= from.Date && i.InvoiceDate.Date <= to.Date))
            {
                entries.Add(new AccountStatementDto(
                    invoice.InvoiceDate.Date,
                    "فاتورة بيع",
                    invoice.InvoiceNo.ToString(),
                    Money(invoice.NetTotal),
                    0m,
                    0m));
            }

            // ─── دائن: مرتجعات البيع المرحّلة ───
            foreach (var salesReturn in MockReturnsData.SalesReturns
                         .Where(r => r.CustomerId == customerId && r.Status == Posted)
                         .Where(r => r.ReturnDate.Date >= from.Date && r.ReturnDate.Date <= to.Date))
            {
                entries.Add(new AccountStatementDto(
                    salesReturn.ReturnDate.Date,
                    "مرتجع بيع",
                    salesReturn.ReturnNo.ToString(),
                    0m,
                    Money(salesReturn.TotalAmount),
                    0m));
            }

            // ─── دائن: سندات القبض المرحّلة ───
            foreach (var receipt in MockCustomerReceiptsData.Receipts
                         .Where(r => r.CustomerId == customerId && r.Status == Posted && r.IsActive)
                         .Where(r => r.ReceiptDate.Date >= from.Date && r.ReceiptDate.Date <= to.Date))
            {
                entries.Add(new AccountStatementDto(
                    receipt.ReceiptDate.Date,
                    "سند قبض",
                    receipt.ReceiptNo.ToString(),
                    0m,
                    Money(receipt.Amount),
                    0m));
            }

            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Success(
                ApplyRunningBalance(entries, debitNormal: true)));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(CustomerStatementError));
        }
    }

    // ═══════════════════════════════════════════════
    // 4) كشف حساب المورد — من مستنداته فقط
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierStatementAsync(
        int supplierId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (supplierId <= 0)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(InvalidSupplier));

        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(DateRangeError));

        try
        {
            if (MockSuppliersData.Suppliers.All(s => s.Id != supplierId))
                return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(SupplierNotFound));

            var entries = new List<AccountStatementDto>();

            // ─── دائن: فواتير الشراء المرحّلة ───
            foreach (var invoice in MockPurchasesData.Purchases
                         .Where(p => p.SupplierId == supplierId && p.Status == Posted)
                         .Where(p => p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date >= from.Date
                                  && p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date <= to.Date))
            {
                entries.Add(new AccountStatementDto(
                    invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue),
                    "فاتورة شراء",
                    invoice.InvoiceNo.ToString(),
                    0m,
                    Money(invoice.NetTotal),
                    0m));
            }

            // ─── مدين: مرتجعات الشراء المرحّلة ───
            foreach (var purchaseReturn in MockReturnsData.PurchaseReturns
                         .Where(r => r.SupplierId == supplierId && r.Status == Posted)
                         .Where(r => r.ReturnDate.ToDateTime(TimeOnly.MinValue).Date >= from.Date
                                  && r.ReturnDate.ToDateTime(TimeOnly.MinValue).Date <= to.Date))
            {
                entries.Add(new AccountStatementDto(
                    purchaseReturn.ReturnDate.ToDateTime(TimeOnly.MinValue),
                    "مرتجع شراء",
                    purchaseReturn.ReturnNo.ToString(),
                    Money(purchaseReturn.TotalAmount),
                    0m,
                    0m));
            }

            // ─── مدين: سندات الصرف للمورد ───
            foreach (var payment in MockSupplierPaymentsData.Payments
                         .Where(p => p.SupplierId == supplierId)
                         .Where(p => p.PaymentDate.ToDateTime(TimeOnly.MinValue).Date >= from.Date
                                  && p.PaymentDate.ToDateTime(TimeOnly.MinValue).Date <= to.Date))
            {
                entries.Add(new AccountStatementDto(
                    payment.PaymentDate.ToDateTime(TimeOnly.MinValue),
                    "سند صرف للمورد",
                    payment.PaymentNo,
                    Money(payment.Amount),
                    0m,
                    0m));
            }

            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Success(
                ApplyRunningBalance(entries, debitNormal: false)));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(SupplierStatementError));
        }
    }

    // ═══════════════════════════════════════════════
    // بناء الحركات النقدية من مخازن بيانات التجربة
    // ═══════════════════════════════════════════════

    private static List<CashMovement> BuildCashMovements()
    {
        var movements = new List<CashMovement>();

        // ─── 1) تحصيلات فواتير البيع المرحّلة ───
        foreach (var invoice in MockData.Invoices.Where(i => i.Status == Posted))
        {
            if (invoice.PaidAmount <= 0m)
                continue;

            movements.Add(new CashMovement(
                Date: invoice.InvoiceDate.Date,
                Group: SalesCollectionGroup,
                CashIn: Money(invoice.PaidAmount),
                CashOut: 0m,
                CashBoxId: invoice.CashBoxId));
        }

        // ─── 2) مدفوعات فواتير الشراء المرحّلة ───
        foreach (var invoice in MockPurchasesData.Purchases.Where(p => p.Status == Posted))
        {
            if (invoice.PaidAmount <= 0m)
                continue;

            movements.Add(new CashMovement(
                Date: invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date,
                Group: PurchasePaymentGroup,
                CashIn: 0m,
                CashOut: Money(invoice.PaidAmount),
                CashBoxId: invoice.CashBoxId));
        }

        // ─── 3) المصروفات المرحّلة ───
        foreach (var expense in MockExpensesData.Expenses.Where(e => e.Status == Posted))
        {
            movements.Add(new CashMovement(
                Date: expense.ExpenseDate.Date,
                Group: ExpenseCashGroup,
                CashIn: 0m,
                CashOut: Money(expense.Amount),
                CashBoxId: expense.CashBoxId));
        }

        // ─── 4) المردودات النقدية لمرتجعات البيع المرحّلة ───
        foreach (var salesReturn in MockReturnsData.SalesReturns.Where(r => r.Status == Posted))
        {
            if (salesReturn.RefundAmount <= 0m)
                continue;

            movements.Add(new CashMovement(
                Date: salesReturn.ReturnDate.Date,
                Group: SalesRefundGroup,
                CashIn: 0m,
                CashOut: Money(salesReturn.RefundAmount),
                CashBoxId: salesReturn.CashBoxId));
        }

        return movements;
    }

    /// <summary>رصيد جارٍ لكل كشف: العميل مدين − دائن · المورد دائن − مدين (طبيعة الحساب من الطرف).</summary>
    private static List<AccountStatementDto> ApplyRunningBalance(
        List<AccountStatementDto> entries, bool debitNormal)
    {
        var ordered = entries
            .OrderBy(e => e.Date)
            .ThenByDescending(e => e.Debit > 0m)
            .ToList();

        decimal running = 0m;
        var result = new List<AccountStatementDto>(ordered.Count);

        foreach (var entry in ordered)
        {
            running += debitNormal ? entry.Debit - entry.Credit : entry.Credit - entry.Debit;
            result.Add(entry with { Balance = Money(running) });
        }

        return result;
    }

    // ═══════════════════════════════════════════════
    // تكلفة البضاعة — من تكلفة السطر أو من تكلفة وحدة الصنف
    // ═══════════════════════════════════════════════

    /// <summary>تكلفة سطر بيع = كمية السطر × تكلفة الوحدة (من السطر، وإلا من وحدات الأصناف).</summary>
    private static decimal LineCost(SalesInvoiceLineDto line)
    {
        var unitCost = line.UnitCost > 0m ? line.UnitCost : UnitCostOf(line.ProductId, line.ProductUnitId);
        return Money(unitCost * line.Quantity);
    }

    /// <summary>تكلفة سطر مرتجع بيع = الكمية × تكلفة وحدة الصنف في بيانات التجربة.</summary>
    private static decimal UnitCostOfLine(SalesReturnItemDto line)
        => Money(UnitCostOf(line.ProductId, line.ProductUnitId) * line.Quantity);

    private static decimal UnitCostOf(int productId, int productUnitId)
    {
        var unit = MockData.ProductUnits.FirstOrDefault(u => u.Id == productUnitId && u.ProductId == productId)
                   ?? MockData.ProductUnits.FirstOrDefault(u => u.ProductId == productId && u.IsBaseUnit);
        return unit?.CostPrice ?? 0m;
    }

    private static decimal Money(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
