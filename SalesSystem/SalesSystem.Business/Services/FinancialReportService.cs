using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.Models;
using SalesSystem.Common.Enums;

namespace SalesSystem.Business.Services;

public class FinancialReportService : IFinancialReportService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<FinancialReportService> _logger;

    public FinancialReportService(IUnitOfWork uow, ILogger<FinancialReportService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    // ─── Income Statement ──────────────────────────────────────────

    public async Task<Result<List<IncomeStatementDto>>> GetIncomeStatementAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        try
        {
            if (from > to)
                return Result<List<IncomeStatementDto>>.Failure("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");

            _logger.LogInformation("Generating income statement from {From} to {To}", from, to);

            var salesInvoices = await _uow.SalesInvoices.ToListAsync(
                si => si.Status == InvoiceStatus.Posted && si.InvoiceDate >= from && si.InvoiceDate <= to,
                ct: ct);

            var totalSales = salesInvoices.Sum(si => si.NetTotal);

            var purchaseInvoices = await _uow.PurchaseInvoices.ToListAsync(
                pi => pi.Status == InvoiceStatus.Posted && pi.InvoiceDate >= DateOnly.FromDateTime(from) && pi.InvoiceDate <= DateOnly.FromDateTime(to),
                ct: ct);

            var totalPurchases = purchaseInvoices.Sum(pi => pi.NetTotal);

            var netProfit = totalSales - totalPurchases;

            var items = new List<IncomeStatementDto>
            {
                new("إيرادات", "إيرادات المبيعات", totalSales),
                new("تكاليف", "تكلفة المشتريات", totalPurchases),
                new(netProfit >= 0 ? "أرباح" : "خسائر",
                    netProfit >= 0 ? "صافي الربح" : "صافي الخسارة",
                    Math.Abs(netProfit))
            };

            _logger.LogInformation("Income statement generated: Sales={TotalSales}, Purchases={TotalPurchases}, Net={NetProfit}",
                totalSales, totalPurchases, netProfit);

            return Result<List<IncomeStatementDto>>.Success(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating income statement");
            return Result<List<IncomeStatementDto>>.Failure("حدث خطأ أثناء إنشاء قائمة الدخل");
        }
    }

    // ─── Cash Flow Report ─────────────────────────────────────────────────

    public async Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(DateTime from, DateTime to, int? cashBoxId, CancellationToken ct)
    {
        try
        {
            if (from > to)
                return Result<CashFlowReportDto>.Failure("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");

            _logger.LogInformation("Generating cash flow report from {From} to {To}" +
                (cashBoxId.HasValue ? " for cash box {CashBoxId}" : ""),
                from, to, cashBoxId);

            // Opening balance = 0 (no vouchers/accounts to compute historical balance)
            decimal openingBalance = 0m;

            // ─── Period inflows: Customer Receipts ───────────────────────────
            var periodReceipts = await _uow.CustomerReceipts.ToListAsync(
                r => r.Status == InvoiceStatus.Posted
                  && r.ReceiptDate >= from && r.ReceiptDate <= to
                  && (!cashBoxId.HasValue || r.CashBoxId == cashBoxId.Value),
                ct: ct);

            // ─── Period inflows: Cash Sales Invoices (paid immediately) ─────
            var periodCashSales = await _uow.SalesInvoices.ToListAsync(
                si => si.Status == InvoiceStatus.Posted
                    && si.PaymentType == PaymentType.Cash
                    && si.InvoiceDate >= from && si.InvoiceDate <= to
                    && (!cashBoxId.HasValue || si.CashBoxId == cashBoxId.Value),
                ct: ct);

            // ─── Period outflows: Supplier Payments ──────────────────────────
            var periodPayments = await _uow.SupplierPayments.ToListAsync(
                p => p.Status == InvoiceStatus.Posted
                    && p.PaymentDate >= DateOnly.FromDateTime(from)
                    && p.PaymentDate <= DateOnly.FromDateTime(to)
                    && (!cashBoxId.HasValue || p.CashBoxId == cashBoxId.Value),
                ct: ct);

            // ─── Period outflows: Expenses ───────────────────────────────────
            var periodExpenses = await _uow.Expenses.ToListAsync(
                e => e.Status == InvoiceStatus.Posted
                    && e.ExpenseDate >= from && e.ExpenseDate <= to
                    && (!cashBoxId.HasValue || e.CashBoxId == cashBoxId.Value),
                ct: ct);

            var totalIncome = periodReceipts.Sum(r => r.Amount) + periodCashSales.Sum(si => si.PaidAmount);
            var totalExpense = periodPayments.Sum(p => p.Amount) + periodExpenses.Sum(e => e.Amount);
            var netCashFlow = totalIncome - totalExpense;
            var closingBalance = openingBalance + netCashFlow;

            // ─── Build categorized income items ─────────────────────────────────
            var incomeItems = new List<CashFlowItemDto>
            {
                new("إيرادات سندات القبض", periodReceipts.Sum(r => r.Amount)),
                new("مبيعات نقدية", periodCashSales.Sum(si => si.PaidAmount))
            };

            // ─── Build categorized expense items ────────────────────────────────
            var expenseItems = new List<CashFlowItemDto>
            {
                new("مدفوعات للموردين", periodPayments.Sum(p => p.Amount)),
                new("مصروفات", periodExpenses.Sum(e => e.Amount))
            };

            var dto = new CashFlowReportDto(
                OpeningBalance: openingBalance,
                TotalIncome: totalIncome,
                TotalExpense: totalExpense,
                NetCashFlow: netCashFlow,
                ClosingBalance: closingBalance,
                IncomeItems: incomeItems,
                ExpenseItems: expenseItems
            );

            _logger.LogInformation(
                "Cash flow report generated: Opening={OpeningBalance}, Income={TotalIncome}, Expense={TotalExpense}, Net={NetCashFlow}, Closing={ClosingBalance}",
                openingBalance, totalIncome, totalExpense, netCashFlow, closingBalance);

            return Result<CashFlowReportDto>.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating cash flow report");
            return Result<CashFlowReportDto>.Failure("حدث خطأ أثناء إنشاء تقرير التدفق النقدي");
        }
    }

    // ─── Account Statements ──────────────────────────────────────────────────────

    public async Task<Result<List<AccountStatementDto>>> GetAccountStatementAsync(int customerId, DateTime from, DateTime to, CancellationToken ct)
    {
        try
        {
            if (from > to)
                return Result<List<AccountStatementDto>>.Failure("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
            if (customerId <= 0)
                return Result<List<AccountStatementDto>>.Failure("معرف العميل غير صالح");

            _logger.LogInformation("Generating account statement for customer {CustomerId} from {From} to {To}", customerId, from, to);

            var entries = new List<AccountStatementDto>();

            var invoices = await _uow.SalesInvoices.ToListAsync(
                si => si.CustomerId == customerId && si.Status == InvoiceStatus.Posted
                   && si.InvoiceDate >= from && si.InvoiceDate <= to,
                q => q.OrderBy(si => si.InvoiceDate), ct: ct);

            foreach (var invoice in invoices)
            {
                entries.Add(new AccountStatementDto(invoice.InvoiceDate, "فاتورة بيع",
                    invoice.Id.ToString(), invoice.NetTotal, 0, 0));
            }

            entries = entries.OrderBy(e => e.Date).ThenByDescending(e => e.Debit > 0 ? 0 : 1).ToList();

            decimal runningBalance = 0;
            for (int i = 0; i < entries.Count; i++)
            {
                runningBalance += entries[i].Debit - entries[i].Credit;
                entries[i] = entries[i] with { Balance = runningBalance };
            }

            _logger.LogInformation("Account statement for customer {CustomerId}: {Count} entries, final balance {Balance}",
                customerId, entries.Count, runningBalance);

            return Result<List<AccountStatementDto>>.Success(entries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating account statement for customer {CustomerId}", customerId);
            return Result<List<AccountStatementDto>>.Failure("حدث خطأ أثناء إنشاء كشف حساب العميل");
        }
    }

    public async Task<Result<List<AccountStatementDto>>> GetSupplierStatementAsync(int supplierId, DateTime from, DateTime to, CancellationToken ct)
    {
        try
        {
            if (from > to)
                return Result<List<AccountStatementDto>>.Failure("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
            if (supplierId <= 0)
                return Result<List<AccountStatementDto>>.Failure("معرف المورد غير صالح");

            _logger.LogInformation("Generating account statement for supplier {SupplierId} from {From} to {To}", supplierId, from, to);

            var entries = new List<AccountStatementDto>();

            var invoices = await _uow.PurchaseInvoices.ToListAsync(
                pi => pi.SupplierId == supplierId && pi.Status == InvoiceStatus.Posted
                   && pi.InvoiceDate >= DateOnly.FromDateTime(from) && pi.InvoiceDate <= DateOnly.FromDateTime(to),
                q => q.OrderBy(pi => pi.InvoiceDate), ct: ct);

            foreach (var invoice in invoices)
            {
                entries.Add(new AccountStatementDto(invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue), "فاتورة شراء",
                    invoice.Id.ToString(), 0, invoice.NetTotal, 0));
            }

            var payments = await _uow.SupplierPayments.ToListAsync(
                sp => sp.SupplierId == supplierId && sp.PaymentDate >= DateOnly.FromDateTime(from) && sp.PaymentDate <= DateOnly.FromDateTime(to),
                q => q.OrderBy(sp => sp.PaymentDate), ct: ct);

            foreach (var payment in payments)
            {
                entries.Add(new AccountStatementDto(payment.PaymentDate.ToDateTime(TimeOnly.MinValue), "دفعة مورد",
                    payment.PaymentNo.ToString(), payment.Amount, 0, 0));
            }

            entries = entries.OrderBy(e => e.Date).ThenByDescending(e => e.Credit > 0 ? 0 : 1).ToList();

            decimal runningBalance = 0;
            for (int i = 0; i < entries.Count; i++)
            {
                runningBalance += entries[i].Credit - entries[i].Debit;
                entries[i] = entries[i] with { Balance = runningBalance };
            }

            _logger.LogInformation("Account statement for supplier {SupplierId}: {Count} entries, final balance {Balance}",
                supplierId, entries.Count, runningBalance);

            return Result<List<AccountStatementDto>>.Success(entries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating account statement for supplier {SupplierId}", supplierId);
            return Result<List<AccountStatementDto>>.Failure("حدث خطأ أثناء إنشاء كشف حساب المورد");
        }
    }
}
