using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Desktop.Services.Mock;

/// <summary>
/// محاكاة التقارير المالية — تُشتق كل الأرقام من دفتر أستاذ واحد مبني من مخازن بيانات التجربة:
/// <c>MockJournalEntriesData.EntryDetails</c> (القيود المرحّلة)، <c>MockData.Invoices</c> (فواتير البيع)،
/// <c>MockPurchasesData.Purchases</c> (فواتير الشراء)، <c>MockExpensesData.Expenses</c> (المصروفات)
/// و<c>MockReturnsData</c> (المرتجعات)، مع أسماء الحسابات من <c>MockAccountsData.Accounts</c>.
/// </summary>
/// <remarks>
/// التوازن مضمون بحكم البناء: كل حركة تُسجَّل بقيدين متساويين (طرف مدين وطرف دائن)، لذلك
/// ميزان المراجعة متوازن دائماً، والأصول = الخصوم + حقوق الملكية، وصافي الربح = الإيرادات − المصروفات.
/// لا تلمس قاعدة البيانات ولا أي اتصال شبكي ولا ترمي استثناءً.
/// </remarks>
public class MockFinancialReportDesktopService : IFinancialReportDesktopService
{
    /// <summary>المستند المرحّل فقط (Draft = 1, Posted = 2, Cancelled = 3).</summary>
    private const byte Posted = 2;

    // ─── أكواد الحسابات المستخدمة في الترحيل (من دليل الحسابات التجريبي) ───
    private const string CashCode = "1111";          // الصندوق الرئيسي
    private const string ReceivableCode = "113";     // حسابات العملاء والمدينون
    private const string InventoryCode = "114";      // بضاعة آخر المدة
    private const string PayableCode = "211";        // حسابات الموردين والدائنون
    private const string VatCode = "213";            // ضرائب مستحقة
    private const string SalesRevenueCode = "31";    // إيراد مبيعات البضاعة
    private const string CogsCode = "41";            // تكلفة البضاعة المباعة

    // ─── تسميات مجموعات التدفق النقدي ───
    private const string SalesCollectionGroup = "تحصيلات فواتير البيع";
    private const string SalesRefundGroup = "مردودات نقدية للعملاء";
    private const string PurchasePaymentGroup = "مدفوعات فواتير الشراء";
    private const string ExpenseCashGroup = "مصروفات نقدية";
    private const string JournalCashGroup = "قيود يومية";

    // ─── رسائل عربية ───
    private const string DateRangeError = "تاريخ البداية يجب أن يكون قبل تاريخ النهاية";
    private const string InvalidAccount = "معرف الحساب غير صالح";
    private const string AccountNotFound = "الحساب غير موجود";
    private const string InvalidCustomer = "معرف العميل غير صالح";
    private const string InvalidSupplier = "معرف المورد غير صالح";
    private const string CashBoxNotFound = "الصندوق غير موجود";
    private const string IncomeStatementError = "تعذر تحميل قائمة الدخل. حاول مرة أخرى.";
    private const string CashFlowError = "تعذر تحميل تقرير التدفق النقدي. حاول مرة أخرى.";
    private const string VatError = "تعذر تحميل تقرير الضريبة. حاول مرة أخرى.";
    private const string BalanceSheetError = "تعذر تحميل الميزانية العمومية. حاول مرة أخرى.";
    private const string TrialBalanceError = "تعذر تحميل ميزان المراجعة. حاول مرة أخرى.";
    private const string GeneralLedgerError = "تعذر تحميل دفتر الأستاذ. حاول مرة أخرى.";
    private const string WorkingCapitalError = "تعذر تحميل رأس المال العامل. حاول مرة أخرى.";
    private const string AccountBalancesError = "تعذر تحميل أرصدة الحسابات. حاول مرة أخرى.";
    private const string CustomerStatementError = "تعذر تحميل كشف حساب العميل. حاول مرة أخرى.";
    private const string SupplierStatementError = "تعذر تحميل كشف حساب المورد. حاول مرة أخرى.";

    private const string WorkingCapitalAssetLabel = "أصول متداولة";
    private const string WorkingCapitalLiabilityLabel = "خصوم متداولة";
    private const string CurrentAssetsSection = "الأصول المتداولة";
    private const string FixedAssetsSection = "الأصول الثابتة";
    private const string LiabilitiesSection = "الخصوم المتداولة";
    private const string EquitySection = "حقوق الملكية";
    private const string NetProfitLine = "صافي الربح (الخسارة) للسنة";
    private const string RevenueCategory = "إيرادات";
    private const string ExpenseCategory = "مصروفات";
    private const string RevenueTotalLabel = "إجمالي الإيرادات";
    private const string ExpenseTotalLabel = "إجمالي المصروفات";
    private const string NoCode = "-";

    // ═══════════════════════════════════════════════
    // 1) قائمة الدخل
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<IncomeStatementDto>>> GetIncomeStatementAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<IncomeStatementDto>>.Failure(DateRangeError));

        try
        {
            var nets = NetByAccount(PeriodLedger(from, to));
            var rows = new List<IncomeStatementDto>();

            var revenues = nets
                .Where(kv => NatureOf(kv.Key) == 4)
                .Select(kv => (Code: kv.Key, Amount: Money(-kv.Value)))
                .Where(r => r.Amount != 0m)
                .OrderByDescending(r => r.Amount)
                .ToList();

            foreach (var revenue in revenues)
                rows.Add(new IncomeStatementDto(RevenueCategory, AccountName(revenue.Code), revenue.Amount));

            var totalRevenue = Money(revenues.Sum(r => r.Amount));
            rows.Add(new IncomeStatementDto(RevenueCategory, RevenueTotalLabel, totalRevenue));

            var expenses = nets
                .Where(kv => NatureOf(kv.Key) == 5)
                .Select(kv => (Code: kv.Key, Amount: Money(kv.Value)))
                .Where(r => r.Amount != 0m)
                .OrderByDescending(r => r.Amount)
                .ToList();

            foreach (var expense in expenses)
                rows.Add(new IncomeStatementDto(ExpenseCategory, AccountName(expense.Code), expense.Amount));

            var totalExpenses = Money(expenses.Sum(r => r.Amount));
            rows.Add(new IncomeStatementDto(ExpenseCategory, ExpenseTotalLabel, totalExpenses));

            var netProfit = Money(totalRevenue - totalExpenses);
            rows.Add(new IncomeStatementDto(
                netProfit >= 0m ? "أرباح" : "خسائر",
                netProfit >= 0m ? "صافي الربح" : "صافي الخسارة",
                netProfit));

            return Task.FromResult(Result<IReadOnlyList<IncomeStatementDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<IncomeStatementDto>>.Failure(IncomeStatementError));
        }
    }

    // ═══════════════════════════════════════════════
    // 2) التدفق النقدي (حركة حسابات الصناديق 111*)
    // ═══════════════════════════════════════════════
    public Task<Result<CashFlowReportDto>> GetCashFlowReportAsync(
        DateTime from, DateTime to, int? cashBoxId = null, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<CashFlowReportDto>.Failure(DateRangeError));

        try
        {
            string? cashCode = null;
            if (cashBoxId.HasValue)
            {
                var cashBox = MockData.CashBoxes.FirstOrDefault(b => b.Id == cashBoxId.Value);
                if (cashBox is null)
                    return Task.FromResult(Result<CashFlowReportDto>.Failure(CashBoxNotFound));

                cashCode = string.IsNullOrWhiteSpace(cashBox.AccountCode) ? CashCode : cashBox.AccountCode!;
            }

            var cashPostings = BuildLedger()
                .Where(p => IsCashCode(p.AccountCode))
                .Where(p => cashCode is null || p.AccountCode == cashCode)
                .ToList();

            var before = cashPostings.Where(p => p.Date < from.Date).ToList();
            var period = cashPostings.Where(p => p.Date >= from.Date && p.Date <= to.Date).ToList();

            var openingBalance = Money(before.Sum(p => p.Debit - p.Credit));
            var totalIncome = Money(period.Sum(p => p.Debit));
            var totalExpense = Money(period.Sum(p => p.Credit));
            var netCashFlow = Money(totalIncome - totalExpense);

            var incomeItems = period
                .Where(p => p.Debit != 0m)
                .GroupBy(p => p.CashGroup ?? SalesCollectionGroup)
                .Select(g => new CashFlowItemDto(g.Key, Money(g.Sum(p => p.Debit))))
                .OrderByDescending(i => i.Amount)
                .ToList();

            var expenseItems = period
                .Where(p => p.Credit != 0m)
                .GroupBy(p => p.CashGroup ?? ExpenseCashGroup)
                .Select(g => new CashFlowItemDto(g.Key, Money(g.Sum(p => p.Credit))))
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
    // 3) تقرير ضريبة القيمة المضافة
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<VatReportDto>>> GetVatReportAsync(
        DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<VatReportDto>>.Failure(DateRangeError));

        try
        {
            var rows = new List<VatReportDto>();

            var sales = MockData.Invoices
                .Where(i => i.Status == Posted && i.TaxAmount > 0m)
                .Where(i => i.InvoiceDate.Date >= from.Date && i.InvoiceDate.Date <= to.Date)
                .OrderBy(i => i.InvoiceDate)
                .ToList();

            foreach (var invoice in sales)
            {
                var taxable = invoice.SubTotal - invoice.DiscountAmount;
                rows.Add(new VatReportDto(
                    invoice.InvoiceNo.ToString(),
                    invoice.InvoiceDate.Date,
                    invoice.CustomerName,
                    Money(taxable),
                    taxable > 0m ? Math.Round(invoice.TaxAmount / taxable * 100m, 2) : 0m,
                    Money(invoice.TaxAmount)));
            }

            var purchases = MockPurchasesData.Purchases
                .Where(p => p.Status == Posted && p.TaxAmount > 0m)
                .Where(p => p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date >= from.Date
                         && p.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date <= to.Date)
                .OrderBy(p => p.InvoiceDate)
                .ToList();

            foreach (var invoice in purchases)
            {
                var taxable = invoice.SubTotal - invoice.DiscountAmount;
                rows.Add(new VatReportDto(
                    invoice.InvoiceNo.ToString(),
                    invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue),
                    invoice.SupplierName,
                    Money(taxable),
                    taxable > 0m ? Math.Round(invoice.TaxAmount / taxable * 100m, 2) : 0m,
                    Money(invoice.TaxAmount)));
            }

            return Task.FromResult(Result<IReadOnlyList<VatReportDto>>.Success(
                rows.OrderBy(r => r.InvoiceDate).ToList()));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<VatReportDto>>.Failure(VatError));
        }
    }

    // ═══════════════════════════════════════════════
    // 4) الميزانية العمومية
    // ═══════════════════════════════════════════════
    public Task<Result<BalanceSheetDto>> GetBalanceSheetAsync(DateTime asOfDate, CancellationToken ct = default)
    {
        try
        {
            var nets = NetByAccount(BuildLedger().Where(p => p.Date <= asOfDate.Date));

            var currentAssets = BalanceLines(nets, code => NatureOf(code) == 1 && code.StartsWith("11"), creditPositive: false);
            var fixedAssets = BalanceLines(nets, code => NatureOf(code) == 1 && code.StartsWith("12"), creditPositive: false);
            var liabilities = BalanceLines(nets, code => NatureOf(code) == 2, creditPositive: true);
            var equity = BalanceLines(nets, code => NatureOf(code) == 3, creditPositive: true);

            var netProfit = NetProfitOf(nets);
            equity.Add(new BalanceSheetLineDto(NetProfitLine, NoCode, netProfit, Format(netProfit)));

            var sections = new List<BalanceSheetSectionDto>
            {
                BuildSection(CurrentAssetsSection, currentAssets),
                BuildSection(FixedAssetsSection, fixedAssets),
                BuildSection(LiabilitiesSection, liabilities),
                BuildSection(EquitySection, equity)
            };

            var totalAssets = Money(sections[0].Total + sections[1].Total);
            var totalLiabilities = sections[2].Total;
            var totalEquity = sections[3].Total;
            var totalLiabilitiesAndEquity = Money(totalLiabilities + totalEquity);

            var report = new BalanceSheetDto(
                TotalAssets: totalAssets,
                TotalLiabilities: totalLiabilities,
                TotalEquity: totalEquity,
                TotalLiabilitiesAndEquity: totalLiabilitiesAndEquity,
                IsBalanced: totalAssets == totalLiabilitiesAndEquity,
                Sections: sections);

            return Task.FromResult(Result<BalanceSheetDto>.Success(report));
        }
        catch
        {
            return Task.FromResult(Result<BalanceSheetDto>.Failure(BalanceSheetError));
        }
    }

    // ═══════════════════════════════════════════════
    // 5) ميزان المراجعة
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<TrialBalanceDto>>> GetTrialBalanceAsync(
        DateTime asOfDate, CancellationToken ct = default)
    {
        try
        {
            var postings = BuildLedger().Where(p => p.Date <= asOfDate.Date).ToList();

            var rows = postings
                .GroupBy(p => p.AccountCode)
                .Select(g =>
                {
                    var openingNet = g.Where(p => p.IsOpening).Sum(p => p.Debit - p.Credit);
                    var transactionDebit = Money(g.Where(p => !p.IsOpening).Sum(p => p.Debit));
                    var transactionCredit = Money(g.Where(p => !p.IsOpening).Sum(p => p.Credit));
                    var closingNet = Money(g.Sum(p => p.Debit - p.Credit));

                    return new TrialBalanceDto(
                        AccountCode: g.Key,
                        AccountName: AccountName(g.Key, g.First().AccountName),
                        OpeningDebit: openingNet > 0m ? Money(openingNet) : 0m,
                        OpeningCredit: openingNet < 0m ? Money(-openingNet) : 0m,
                        TransactionDebit: transactionDebit,
                        TransactionCredit: transactionCredit,
                        ClosingDebit: closingNet > 0m ? closingNet : 0m,
                        ClosingCredit: closingNet < 0m ? Money(-closingNet) : 0m,
                        AccountTypeLabel: NatureLabel(NatureOf(g.Key)));
                })
                .Where(r => r.TransactionDebit != 0m || r.TransactionCredit != 0m
                         || r.OpeningDebit != 0m || r.OpeningCredit != 0m)
                .OrderBy(r => r.AccountCode, StringComparer.Ordinal)
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<TrialBalanceDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<TrialBalanceDto>>.Failure(TrialBalanceError));
        }
    }

    // ═══════════════════════════════════════════════
    // 6) دفتر الأستاذ العام
    // ═══════════════════════════════════════════════
    public Task<Result<AccountLedgerDto>> GetGeneralLedgerAsync(
        int accountId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (accountId <= 0)
            return Task.FromResult(Result<AccountLedgerDto>.Failure(InvalidAccount));

        if (from.Date > to.Date)
            return Task.FromResult(Result<AccountLedgerDto>.Failure(DateRangeError));

        try
        {
            var account = MockAccountsData.Accounts.FirstOrDefault(a => a.Id == accountId);
            if (account is null)
                return Task.FromResult(Result<AccountLedgerDto>.Failure(AccountNotFound));

            var movements = BuildLedger()
                .Where(p => p.AccountCode == account.AccountCode)
                .ToList();

            var debitNormal = IsDebitNormal(account.Nature);

            decimal OpeningBalanceOf(IEnumerable<LedgerPosting> items)
                => Money(debitNormal
                    ? items.Sum(p => p.Debit - p.Credit)
                    : items.Sum(p => p.Credit - p.Debit));

            var openingBalance = OpeningBalanceOf(movements.Where(p => p.Date < from.Date));

            var running = openingBalance;
            var lines = new List<AccountLedgerLineDto>();
            foreach (var posting in movements
                         .Where(p => p.Date >= from.Date && p.Date <= to.Date)
                         .OrderBy(p => p.Date)
                         .ThenBy(p => p.EntryNumber, StringComparer.Ordinal))
            {
                running += debitNormal ? posting.Debit - posting.Credit : posting.Credit - posting.Debit;
                lines.Add(new AccountLedgerLineDto(
                    Date: posting.Date,
                    EntryNumber: posting.EntryNumber,
                    Description: posting.Description,
                    ReferenceNumber: null,
                    Debit: posting.Debit,
                    Credit: posting.Credit,
                    RunningBalance: Money(running)));
            }

            var ledger = new AccountLedgerDto(
                AccountCode: account.AccountCode,
                AccountNameAr: account.NameAr,
                OpeningBalance: openingBalance,
                Lines: lines,
                TotalDebit: Money(lines.Sum(l => l.Debit)),
                TotalCredit: Money(lines.Sum(l => l.Credit)),
                ClosingBalance: Money(running));

            return Task.FromResult(Result<AccountLedgerDto>.Success(ledger));
        }
        catch
        {
            return Task.FromResult(Result<AccountLedgerDto>.Failure(GeneralLedgerError));
        }
    }

    // ═══════════════════════════════════════════════
    // 7) رأس المال العامل
    // ═══════════════════════════════════════════════
    public Task<Result<WorkingCapitalSummaryDto>> GetWorkingCapitalAsync(
        DateTime asOfDate, CancellationToken ct = default)
    {
        try
        {
            var nets = NetByAccount(BuildLedger().Where(p => p.Date <= asOfDate.Date));

            var accounts = new List<WorkingCapitalAccountDto>();

            foreach (var kv in nets
                         .Where(kv => NatureOf(kv.Key) == 1 && kv.Key.StartsWith("11") && kv.Value != 0m)
                         .OrderBy(kv => kv.Key, StringComparer.Ordinal))
            {
                accounts.Add(new WorkingCapitalAccountDto(
                    AccountName(kv.Key), kv.Key, Money(kv.Value), WorkingCapitalAssetLabel));
            }

            foreach (var kv in nets
                         .Where(kv => NatureOf(kv.Key) == 2 && kv.Value != 0m)
                         .OrderBy(kv => kv.Key, StringComparer.Ordinal))
            {
                accounts.Add(new WorkingCapitalAccountDto(
                    AccountName(kv.Key), kv.Key, Money(-kv.Value), WorkingCapitalLiabilityLabel));
            }

            var currentAssets = Money(accounts
                .Where(a => a.Type == WorkingCapitalAssetLabel)
                .Sum(a => a.Balance));

            var currentLiabilities = Money(accounts
                .Where(a => a.Type == WorkingCapitalLiabilityLabel)
                .Sum(a => a.Balance));

            var summary = new WorkingCapitalSummaryDto(
                CurrentAssets: currentAssets,
                CurrentLiabilities: currentLiabilities,
                WorkingCapital: Money(currentAssets - currentLiabilities),
                CurrentRatio: currentLiabilities > 0m
                    ? Math.Round(currentAssets / currentLiabilities, 2)
                    : 0m,
                Accounts: accounts);

            return Task.FromResult(Result<WorkingCapitalSummaryDto>.Success(summary));
        }
        catch
        {
            return Task.FromResult(Result<WorkingCapitalSummaryDto>.Failure(WorkingCapitalError));
        }
    }

    // ═══════════════════════════════════════════════
    // 8) أرصدة الحسابات
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<AccountBalanceReportDto>>> GetAccountBalancesAsync(
        byte? accountType = null, string? search = null, CancellationToken ct = default)
    {
        try
        {
            var nets = NetByAccount(BuildLedger().Where(p => p.Date <= DateTime.Today));
            var term = search?.Trim();

            var rows = nets
                .Where(kv => kv.Value != 0m)
                .Where(kv => !accountType.HasValue || NatureOf(kv.Key) == accountType.Value)
                .Where(kv => string.IsNullOrWhiteSpace(term)
                          || kv.Key.Contains(term!, StringComparison.OrdinalIgnoreCase)
                          || AccountName(kv.Key).Contains(term!, StringComparison.OrdinalIgnoreCase))
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .Select(kv =>
                {
                    var nature = NatureOf(kv.Key);
                    return new AccountBalanceReportDto(
                        AccountCode: kv.Key,
                        AccountName: AccountName(kv.Key),
                        AccountTypeDisplay: NatureLabel(nature) ?? string.Empty,
                        Level: FindAccount(kv.Key)?.Level ?? 1,
                        DebitBalance: kv.Value > 0m ? Money(kv.Value) : 0m,
                        CreditBalance: kv.Value < 0m ? Money(-kv.Value) : 0m,
                        NetBalance: Money(kv.Value));
                })
                .ToList();

            return Task.FromResult(Result<IReadOnlyList<AccountBalanceReportDto>>.Success(rows));
        }
        catch
        {
            return Task.FromResult(Result<IReadOnlyList<AccountBalanceReportDto>>.Failure(AccountBalancesError));
        }
    }

    // ═══════════════════════════════════════════════
    // 9) كشف حساب العميل
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<AccountStatementDto>>> GetCustomerAccountStatementAsync(
        int customerId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (customerId <= 0)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(InvalidCustomer));

        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(DateRangeError));

        try
        {
            var entries = new List<AccountStatementDto>();

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
    // 10) كشف حساب المورد
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierAccountStatementAsync(
        int supplierId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        if (supplierId <= 0)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(InvalidSupplier));

        if (from.Date > to.Date)
            return Task.FromResult(Result<IReadOnlyList<AccountStatementDto>>.Failure(DateRangeError));

        try
        {
            var entries = new List<AccountStatementDto>();

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
    // 11–12) أسماء بديلة مطابقة لطبقة الأعمال
    // ═══════════════════════════════════════════════
    public Task<Result<IReadOnlyList<AccountStatementDto>>> GetAccountStatementAsync(
        int customerId, DateTime from, DateTime to, CancellationToken ct = default)
        => GetCustomerAccountStatementAsync(customerId, from, to, ct);

    public Task<Result<IReadOnlyList<AccountStatementDto>>> GetSupplierStatementAsync(
        int supplierId, DateTime from, DateTime to, CancellationToken ct = default)
        => GetSupplierAccountStatementAsync(supplierId, from, to, ct);

    // ═══════════════════════════════════════════════
    // بناء دفتر الأستاذ من مخازن بيانات التجربة
    // ═══════════════════════════════════════════════

    /// <summary>حركة واحدة داخل دفتر الأستاذ (طرف مدين أو دائن).</summary>
    private sealed record LedgerPosting(
        DateTime Date,
        string AccountCode,
        string AccountName,
        decimal Debit,
        decimal Credit,
        string EntryNumber,
        string Description,
        string? CashGroup,
        bool IsOpening);

    private static List<LedgerPosting> BuildLedger()
    {
        var postings = new List<LedgerPosting>();

        // ─── 1) القيود اليومية المرحّلة (كل قيد متوازن في بياناته) ───
        foreach (var entry in MockJournalEntriesData.EntryDetails.Where(e => e.Status == Posted))
        {
            var isOpening = entry.EntryTypeValue == JournalEntryType.OpeningBalance;

            foreach (var line in entry.Lines)
            {
                var code = line.AccountCode;
                if (string.IsNullOrWhiteSpace(code))
                    continue;

                postings.Add(new LedgerPosting(
                    Date: entry.EntryDate.Date,
                    AccountCode: code!,
                    AccountName: string.IsNullOrWhiteSpace(line.AccountName) ? AccountName(code!) : line.AccountName!,
                    Debit: Money(line.Debit),
                    Credit: Money(line.Credit),
                    EntryNumber: entry.EntryNumber,
                    Description: string.IsNullOrWhiteSpace(line.Description) ? entry.Description : line.Description!,
                    CashGroup: IsCashCode(code!) ? JournalCashGroup : null,
                    IsOpening: isOpening));
            }
        }

        // ─── 2) فواتير البيع المرحّلة ───
        foreach (var invoice in MockData.Invoices.Where(i => i.Status == Posted))
        {
            var date = invoice.InvoiceDate.Date;
            var reference = invoice.InvoiceNo.ToString();
            var revenue = invoice.NetTotal - invoice.TaxAmount;
            var cost = Money(invoice.Items.Sum(LineCost));

            Add(postings, date, ReceivableCode, invoice.NetTotal, 0m, reference,
                $"فاتورة بيع آجلة — {invoice.CustomerName}", null);
            Add(postings, date, SalesRevenueCode, 0m, revenue, reference,
                $"إيراد مبيعات — فاتورة {reference}", null);

            if (invoice.TaxAmount > 0m)
                Add(postings, date, VatCode, 0m, invoice.TaxAmount, reference,
                    $"ضريبة القيمة المضافة — فاتورة {reference}", null);

            if (cost > 0m)
            {
                Add(postings, date, CogsCode, cost, 0m, reference,
                    $"تكلفة البضاعة المباعة — فاتورة {reference}", null);
                Add(postings, date, InventoryCode, 0m, cost, reference,
                    $"صرف مخزون — فاتورة {reference}", null);
            }

            if (invoice.PaidAmount > 0m)
            {
                Add(postings, date, CashCode, invoice.PaidAmount, 0m, reference,
                    $"تحصيل نقدي — فاتورة {reference}", SalesCollectionGroup);
                Add(postings, date, ReceivableCode, 0m, invoice.PaidAmount, reference,
                    $"تسديد جزء من فاتورة {reference}", null);
            }
        }

        // ─── 3) فواتير الشراء المرحّلة ───
        foreach (var invoice in MockPurchasesData.Purchases.Where(p => p.Status == Posted))
        {
            var date = invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue).Date;
            var reference = invoice.InvoiceNo.ToString();
            var goods = invoice.NetTotal - invoice.TaxAmount;

            Add(postings, date, InventoryCode, goods, 0m, reference,
                $"توريد مخزون — فاتورة شراء {reference}", null);

            if (invoice.TaxAmount > 0m)
                Add(postings, date, VatCode, invoice.TaxAmount, 0m, reference,
                    $"ضريبة مشتريات — فاتورة {reference}", null);

            Add(postings, date, PayableCode, 0m, invoice.NetTotal, reference,
                $"فاتورة شراء آجلة — {invoice.SupplierName}", null);

            if (invoice.PaidAmount > 0m)
            {
                Add(postings, date, PayableCode, invoice.PaidAmount, 0m, reference,
                    $"سداد جزء من فاتورة شراء {reference}", null);
                Add(postings, date, CashCode, 0m, invoice.PaidAmount, reference,
                    $"دفعة لمورد — فاتورة {reference}", PurchasePaymentGroup);
            }
        }

        // ─── 4) المصروفات المرحّلة ───
        foreach (var expense in MockExpensesData.Expenses.Where(e => e.Status == Posted))
        {
            var code = AccountCodeById(expense.ExpenseAccountId) ?? "429";
            var cashAccount = CashAccountOf(expense.CashBoxId);

            Add(postings, expense.ExpenseDate.Date, code, expense.Amount, 0m,
                expense.ExpenseNo.ToString(),
                $"مصروف — {expense.ExpenseAccountName}", null);
            Add(postings, expense.ExpenseDate.Date, cashAccount, 0m, expense.Amount,
                expense.ExpenseNo.ToString(),
                $"صرف نقدي — {expense.ExpenseAccountName}", ExpenseCashGroup);
        }

        // ─── 5) مرتجعات البيع المرحّلة ───
        foreach (var salesReturn in MockReturnsData.SalesReturns.Where(r => r.Status == Posted))
        {
            var date = salesReturn.ReturnDate.Date;
            var reference = salesReturn.ReturnNo.ToString();

            Add(postings, date, SalesRevenueCode, salesReturn.TotalAmount, 0m, reference,
                $"مرتجع بيع — {salesReturn.CustomerName}", null);
            Add(postings, date, ReceivableCode, 0m, salesReturn.TotalAmount, reference,
                $"إلغاء مديونية مرتجع {reference}", null);

            if (salesReturn.RefundAmount > 0m)
            {
                Add(postings, date, ReceivableCode, salesReturn.RefundAmount, 0m, reference,
                    $"استحقاق رد نقدي — مرتجع {reference}", null);
                Add(postings, date, CashCode, 0m, salesReturn.RefundAmount, reference,
                    $"رد نقدي للعميل — مرتجع {reference}", SalesRefundGroup);
            }
        }

        // ─── 6) مرتجعات الشراء المرحّلة ───
        foreach (var purchaseReturn in MockReturnsData.PurchaseReturns.Where(r => r.Status == Posted))
        {
            var date = purchaseReturn.ReturnDate.ToDateTime(TimeOnly.MinValue).Date;
            var reference = purchaseReturn.ReturnNo.ToString();

            Add(postings, date, PayableCode, purchaseReturn.TotalAmount, 0m, reference,
                $"مرتجع شراء للمورد — {purchaseReturn.SupplierName}", null);
            Add(postings, date, InventoryCode, 0m, purchaseReturn.TotalAmount, reference,
                $"إخراج مخزون مرتجع {reference}", null);
        }

        return postings;
    }

    private static void Add(
        List<LedgerPosting> postings,
        DateTime date,
        string accountCode,
        decimal debit,
        decimal credit,
        string entryNumber,
        string description,
        string? cashGroup)
    {
        postings.Add(new LedgerPosting(
            Date: date,
            AccountCode: accountCode,
            AccountName: AccountName(accountCode),
            Debit: Money(debit),
            Credit: Money(credit),
            EntryNumber: entryNumber,
            Description: description,
            CashGroup: cashGroup,
            IsOpening: false));
    }

    // ═══════════════════════════════════════════════
    // مساعدات الحسابات والتجميع
    // ═══════════════════════════════════════════════

    private static IEnumerable<LedgerPosting> PeriodLedger(DateTime from, DateTime to)
        => BuildLedger().Where(p => p.Date >= from.Date && p.Date <= to.Date);

    private static Dictionary<string, decimal> NetByAccount(IEnumerable<LedgerPosting> postings)
    {
        var nets = new Dictionary<string, decimal>(StringComparer.Ordinal);

        foreach (var posting in postings)
        {
            var net = posting.Debit - posting.Credit;
            nets[posting.AccountCode] = nets.GetValueOrDefault(posting.AccountCode) + net;
        }

        return nets;
    }

    private static decimal NetProfitOf(IReadOnlyDictionary<string, decimal> nets)
    {
        decimal revenue = 0m, expenses = 0m;

        foreach (var (code, net) in nets)
        {
            switch (NatureOf(code))
            {
                case 4:
                    revenue += -net;
                    break;
                case 5:
                    expenses += net;
                    break;
            }
        }

        return Money(Money(revenue) - Money(expenses));
    }

    private static List<BalanceSheetLineDto> BalanceLines(
        IReadOnlyDictionary<string, decimal> nets, Func<string, bool> predicate, bool creditPositive)
        => nets
            .Where(kv => kv.Value != 0m && predicate(kv.Key))
            .OrderBy(kv => kv.Key, StringComparer.Ordinal)
            .Select(kv =>
            {
                var balance = creditPositive ? -kv.Value : kv.Value;
                return new BalanceSheetLineDto(AccountName(kv.Key), kv.Key, Money(balance), Format(balance));
            })
            .ToList();

    private static BalanceSheetSectionDto BuildSection(string name, List<BalanceSheetLineDto> lines)
    {
        var total = Money(lines.Sum(l => l.Balance));
        return new BalanceSheetSectionDto(name, total, Format(total), lines);
    }

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

    /// <summary>تكلفة سطر البيع = كمية السطر × تكلفة الوحدة (من وحدات الأصناف عند غياب التكلفة في السطر).</summary>
    private static decimal LineCost(SalesInvoiceLineDto line)
    {
        var unitCost = line.UnitCost;

        if (unitCost <= 0m)
        {
            var unit = MockData.ProductUnits.FirstOrDefault(u => u.Id == line.ProductUnitId);
            unitCost = unit is not null && unit.ProductId == line.ProductId
                ? unit.CostPrice
                : MockData.ProductUnits
                    .FirstOrDefault(u => u.ProductId == line.ProductId && u.IsBaseUnit)?.CostPrice ?? 0m;
        }

        return unitCost * line.Quantity;
    }

    private static string AccountName(string code, string? fallback = null)
    {
        var account = FindAccount(code);
        if (account is not null && !string.IsNullOrWhiteSpace(account.NameAr))
            return account.NameAr;

        if (!string.IsNullOrWhiteSpace(fallback))
            return fallback!;

        return $"حساب {code}";
    }

    private static AccountDto? FindAccount(string code)
        => MockAccountsData.Accounts.FirstOrDefault(a => a.AccountCode == code);

    private static int NatureOf(string code)
    {
        var account = FindAccount(code);
        if (account is not null)
            return account.Nature;

        if (code.StartsWith("11") || code.StartsWith("12")) return 1;
        if (code.StartsWith("21")) return 2;
        if (code.StartsWith("22")) return 3;
        if (code.StartsWith("3")) return 4;
        if (code.StartsWith("4")) return 5;

        return 1;
    }

    private static string? NatureLabel(int nature) => nature switch
    {
        1 => "أصل",
        2 => "خصم",
        3 => "حق ملكية",
        4 => "إيراد",
        5 => "مصروف",
        _ => null
    };

    private static bool IsDebitNormal(int nature) => nature is 1 or 5;

    private static bool IsCashCode(string code) => code.StartsWith("111");

    private static string? AccountCodeById(int accountId)
        => MockAccountsData.Accounts.FirstOrDefault(a => a.Id == accountId)?.AccountCode;

    private static string CashAccountOf(int cashBoxId)
    {
        var code = MockData.CashBoxes.FirstOrDefault(b => b.Id == cashBoxId)?.AccountCode;
        return string.IsNullOrWhiteSpace(code) ? CashCode : code!;
    }

    private static decimal Money(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

    // العرض من البيت الواحد (clsFormat) فلا منازل مثبّتة هنا — ويطابق ما سيفعله الباك-إند (GAP-11 / RULE-N167 / RULE-N179)
    private static string Format(decimal value) => clsFormat.Money(Money(value));
}
