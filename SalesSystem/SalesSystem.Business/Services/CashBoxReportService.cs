using Microsoft.Extensions.Logging;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Common.Enums;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Models;

namespace SalesSystem.Business.Services;

/// <summary>
/// Service for cash box summary reports.
/// Summary is computed from operational documents: sales, receipts, payments, and expenses.
/// </summary>
public class CashBoxReportService : ICashBoxReportService
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CashBoxReportService> _logger;

    public CashBoxReportService(IUnitOfWork uow, ILogger<CashBoxReportService> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    /// <summary>
    /// Gets cash box summary showing balance information per cash box.
    /// Computed from operational documents as of the specified date.
    /// </summary>
    public async Task<Result<List<CashBoxSummaryDto>>> GetCashBoxSummaryAsync(
        DateTime? asOfDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var effectiveDate = asOfDate ?? DateTime.UtcNow.Date;
            _logger.LogInformation("Generating cash box summary as of {Date}", effectiveDate);

            // Get all cash boxes (including inactive for historical reporting)
            var cashBoxes = await _uow.CashBoxes.ToListAsync(ct: ct);
            if (cashBoxes.Count == 0)
            {
                _logger.LogInformation("No cash boxes found for summary report");
                return Result<List<CashBoxSummaryDto>>.Success(new List<CashBoxSummaryDto>());
            }

            var results = new List<CashBoxSummaryDto>();

            foreach (var box in cashBoxes)
            {
                var boxId = box.Id;

                // + Cash sales: Posted invoices where CashBoxId matches, PaymentType == Cash, up to effectiveDate
                var cashSales = await _uow.SalesInvoices.ToListAsync(
                    si => si.CashBoxId == boxId
                       && si.Status == InvoiceStatus.Posted
                       && si.PaymentType == PaymentType.Cash
                       && si.InvoiceDate <= effectiveDate,
                    ct: ct);
                var cashSalesTotal = cashSales.Sum(si => si.PaidAmount);

                // + Customer receipts: Posted receipts where CashBoxId matches, up to effectiveDate
                var customerReceipts = await _uow.CustomerReceipts.ToListAsync(
                    cr => cr.CashBoxId == boxId
                       && cr.Status == InvoiceStatus.Posted
                       && cr.ReceiptDate <= effectiveDate,
                    ct: ct);
                var receiptsTotal = customerReceipts.Sum(cr => cr.Amount);

                // - Supplier payments: Posted payments where CashBoxId matches, up to effectiveDate
                var supplierPayments = await _uow.SupplierPayments.ToListAsync(
                    sp => sp.CashBoxId == boxId
                       && sp.Status == InvoiceStatus.Posted
                       && sp.PaymentDate <= DateOnly.FromDateTime(effectiveDate),
                    ct: ct);
                var paymentsTotal = supplierPayments.Sum(sp => sp.Amount);

                // - Expenses: Posted expenses where CashBoxId matches, up to effectiveDate
                var expenses = await _uow.Expenses.ToListAsync(
                    e => e.CashBoxId == boxId
                       && e.Status == InvoiceStatus.Posted
                       && e.ExpenseDate <= effectiveDate,
                    ct: ct);
                var expensesTotal = expenses.Sum(e => e.Amount);

                var totalIncome = cashSalesTotal + receiptsTotal;
                var totalExpense = paymentsTotal + expensesTotal;
                var netBalance = totalIncome - totalExpense;

                results.Add(new CashBoxSummaryDto(
                    CashBoxId: boxId,
                    CashBoxName: box.Name,
                    TotalIncome: totalIncome,
                    TotalExpense: totalExpense,
                    NetBalance: netBalance));

                _logger.LogDebug(
                    "CashBox {Id} ({Name}): Income={Income}, Expense={Expense}, Balance={Balance}",
                    boxId, box.Name, totalIncome, totalExpense, netBalance);
            }

            _logger.LogInformation("Cash box summary generated: {Count} boxes", results.Count);
            return Result<List<CashBoxSummaryDto>>.Success(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating cash box summary");
            return Result<List<CashBoxSummaryDto>>.Failure(
                "حدث خطأ أثناء إنشاء ملخص الصناديق النقدية");
        }
    }
}
