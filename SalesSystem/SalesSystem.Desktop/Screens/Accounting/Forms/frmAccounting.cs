using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    /// <summary>
    /// وحدة الحسابات — نموذج واجهة (UI Shape) ببيانات تجريبية Mock محلية فقط.
    /// لا يقوم بأي اتصال بالـ API أو قاعدة البيانات في هذه المرحلة (UI-First).
    /// المعامل serviceProvider محجوز وفق نمط RULE-N03/N04 لربط
    /// IAccountApiService / IJournalEntryApiService عند اكتمال المرحلة الخلفية.
    /// </summary>
    public partial class frmAccounting : Form
    {
        // ── Mock data (in-memory — UI-first prototype) ─────────────────────────────
        private readonly List<AccountRow> _accounts = new();
        private readonly List<JournalEntryRow> _journalEntries = new();
        private readonly List<CostCenterRow> _costCenters = new();
        private readonly List<FiscalPeriodRow> _fiscalPeriods = new();

        public frmAccounting(IServiceProvider serviceProvider)
        {
            // serviceProvider: محجوز لربط خدمات الـ API عند اكتمال المرحلة الخلفية (UI-First).
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmAccounting_Load(object sender, EventArgs e)
        {
            try
            {
                _BuildMockAccounts();
                _BuildMockJournalEntries();
                _BuildMockCostCenters();
                _BuildMockFiscalPeriods();

                _SetupAccountsGrid();
                _SetupJournalGrid();
                _SetupCostCentersGrid();
                _SetupPeriodsGrid();

                _ApplyAccountsFilter();
                _ApplyJournalFilter();
                _PopulateCostCenters();
                _PopulateFiscalPeriods();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading mock accounting data.");
                MessageBox.Show("حدث خطأ أثناء تحميل البيانات التجريبية. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => frmAccounting_Load(sender, e);

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupAccountsGrid()
        {
            dgvAccounts.AutoGenerateColumns = false;
            dgvAccounts.Columns.Clear();

            _AddGridColumn(dgvAccounts, "Code", "الكود", 12, 90);
            _AddGridColumn(dgvAccounts, "Name", "الاسم", 30, 320);
            _AddGridColumn(dgvAccounts, "Type", "النوع", 10, 90);
            _AddGridColumn(dgvAccounts, "Balance", "الرصيد", 12, 130);
        }

        private void _SetupJournalGrid()
        {
            dgvJournalEntries.AutoGenerateColumns = false;
            dgvJournalEntries.Columns.Clear();

            _AddGridColumn(dgvJournalEntries, "Number", "رقم القيد", 14, 110);
            _AddGridColumn(dgvJournalEntries, "Date", "التاريخ", 10, 100);
            _AddGridColumn(dgvJournalEntries, "Description", "البيان", 30, 320);
            _AddGridColumn(dgvJournalEntries, "Debit", "المدين", 12, 120);
            _AddGridColumn(dgvJournalEntries, "Credit", "الدائن", 12, 120);
            _AddGridColumn(dgvJournalEntries, "Status", "الحالة", 8, 90);
        }

        private void _SetupCostCentersGrid()
        {
            dgvCostCenters.AutoGenerateColumns = false;
            dgvCostCenters.Columns.Clear();

            _AddGridColumn(dgvCostCenters, "Code", "الكود", 12, 90);
            _AddGridColumn(dgvCostCenters, "Name", "الاسم", 26, 260);
            _AddGridColumn(dgvCostCenters, "Budget", "الموازنة", 14, 130);
            _AddGridColumn(dgvCostCenters, "Responsible", "المسؤول", 18, 160);
            _AddGridColumn(dgvCostCenters, "Status", "الحالة", 10, 90);
        }

        private void _SetupPeriodsGrid()
        {
            dgvFiscalPeriods.AutoGenerateColumns = false;
            dgvFiscalPeriods.Columns.Clear();

            _AddGridColumn(dgvFiscalPeriods, "Year", "السنة", 12, 90);
            _AddGridColumn(dgvFiscalPeriods, "StartDate", "تاريخ البدء", 16, 130);
            _AddGridColumn(dgvFiscalPeriods, "EndDate", "تاريخ النهاية", 16, 130);
            _AddGridColumn(dgvFiscalPeriods, "Status", "الحالة", 12, 100);
        }

        private static void _AddGridColumn(DataGridView grid, string name, string header, int fill, int min)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name,
                FillWeight = fill,
                MinimumWidth = min,
                ReadOnly = true,
            });
        }

        // ── Data Population (filtered views) ───────────────────────────────────────

        private void _ApplyAccountsFilter()
        {
            var term = txtAccountsSearch.Text.Trim();
            var mode = cbFilterBy.SelectedItem?.ToString() ?? "بالاسم";

            var filtered = _accounts.AsEnumerable();
            if (!string.IsNullOrEmpty(term))
            {
                filtered = mode == "بالكود"
                    ? filtered.Where(a => a.Code.Contains(term))
                    : filtered.Where(a => a.Name.Contains(term));
            }

            var result = filtered.ToList();

            dgvAccounts.Rows.Clear();
            foreach (var a in result)
            {
                // مستوى 2 (حساب فرعي) يُسبق بشرطة لإظهار الشجرة (أب ← ابن)
                string displayName = a.Level > 1 ? "— " + a.Name : a.Name;

                int idx = dgvAccounts.Rows.Add(a.Code, displayName, a.Type, a.Balance.ToString("N2"));

                if (a.Level == 1)
                {
                    dgvAccounts.Rows[idx].DefaultCellStyle.Font =
                        new Font("Tahoma", 10F, FontStyle.Bold);
                }
            }

            lblAccountsCount.Text = $"[{result.Count}]";
        }

        private void _ApplyJournalFilter()
        {
            var term = txtJvSearch.Text.Trim();
            var from = dtpFrom.Value.Date;
            var to = dtpTo.Value.Date;

            var filtered = _journalEntries
                .Where(j => j.Date.Date >= from && j.Date.Date <= to)
                .Where(j => string.IsNullOrEmpty(term)
                            || j.Number.Contains(term)
                            || j.Description.Contains(term))
                .ToList();

            dgvJournalEntries.Rows.Clear();
            foreach (var j in filtered)
            {
                dgvJournalEntries.Rows.Add(
                    j.Number,
                    j.Date.ToString("yyyy/MM/dd"),
                    j.Description,
                    j.Debit.ToString("N2"),
                    j.Credit.ToString("N2"),
                    j.Status);
            }

            decimal totalDebit = filtered.Sum(j => j.Debit);
            decimal totalCredit = filtered.Sum(j => j.Credit);

            lblTotalDebitVal.Text = totalDebit.ToString("N2");
            lblTotalCreditVal.Text = totalCredit.ToString("N2");

            bool balanced = totalDebit == totalCredit;
            lblBalanceStatus.Text = balanced ? "متوازن ✓" : "غير متوازن ✗";
            lblBalanceStatus.ForeColor = balanced ? Color.Green : Color.Firebrick;

            lblJvCount.Text = $"[{filtered.Count}]";
        }

        private void _PopulateCostCenters()
        {
            dgvCostCenters.Rows.Clear();
            foreach (var c in _costCenters)
            {
                dgvCostCenters.Rows.Add(
                    c.Code,
                    c.Name,
                    c.Budget.ToString("N2"),
                    c.Responsible,
                    c.Status);
            }

            lblCostCount.Text = $"[{_costCenters.Count}]";
        }

        private void _PopulateFiscalPeriods()
        {
            dgvFiscalPeriods.Rows.Clear();
            foreach (var p in _fiscalPeriods)
            {
                dgvFiscalPeriods.Rows.Add(
                    p.Year,
                    p.StartDate.ToString("yyyy/MM/dd"),
                    p.EndDate.ToString("yyyy/MM/dd"),
                    p.Status);
            }

            lblPeriodsCount.Text = $"[{_fiscalPeriods.Count}]";
        }

        // ── Filter Events ──────────────────────────────────────────────────────────

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
            => _ApplyAccountsFilter();

        private void txtAccountsSearch_TextChanged(object sender, EventArgs e)
            => _ApplyAccountsFilter();

        private void txtJvSearch_TextChanged(object sender, EventArgs e)
            => _ApplyJournalFilter();

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
            => _ApplyJournalFilter();

        private void dtpTo_ValueChanged(object sender, EventArgs e)
            => _ApplyJournalFilter();

        // ── Mock Data Builders ──────────────────────────────────────────────────────
        // شجرة حسابات يمنية مبسطة: أصول / خصوم / إيرادات / مصروفات.

        private void _BuildMockAccounts()
        {
            _accounts.Clear();

            // الأصول
            _accounts.Add(new AccountRow { Code = "1101", Name = "الأصول", Type = "أصل", Balance = 470000.00m, Level = 1 });
            _accounts.Add(new AccountRow { Code = "1101-01", Name = "الصندوق الرئيسي", Type = "أصل", Balance = 350000.00m, Level = 2 });
            _accounts.Add(new AccountRow { Code = "1101-02", Name = "الصندوق الفرعي", Type = "أصل", Balance = 120000.00m, Level = 2 });
            _accounts.Add(new AccountRow { Code = "1102", Name = "البنوك", Type = "أصل", Balance = 1850000.00m, Level = 1 });
            _accounts.Add(new AccountRow { Code = "1102-01", Name = "البنك الأهلي اليمني", Type = "أصل", Balance = 1250000.00m, Level = 2 });
            _accounts.Add(new AccountRow { Code = "1102-02", Name = "بنك الكريمي للتمويل الأصغر", Type = "أصل", Balance = 600000.00m, Level = 2 });
            _accounts.Add(new AccountRow { Code = "1301", Name = "المخزون", Type = "أصل", Balance = 2400000.00m, Level = 1 });
            _accounts.Add(new AccountRow { Code = "1301-01", Name = "مخزون البضاعة", Type = "أصل", Balance = 2400000.00m, Level = 2 });

            // الخصوم
            _accounts.Add(new AccountRow { Code = "2101", Name = "الموردون", Type = "خصم", Balance = 750000.00m, Level = 1 });
            _accounts.Add(new AccountRow { Code = "2101-01", Name = "شركة النور للتوريدات", Type = "خصم", Balance = 500000.00m, Level = 2 });

            // الإيرادات
            _accounts.Add(new AccountRow { Code = "4101", Name = "المبيعات", Type = "إيراد", Balance = 3200000.00m, Level = 1 });
            _accounts.Add(new AccountRow { Code = "4101-01", Name = "مبيعات نقدية", Type = "إيراد", Balance = 2100000.00m, Level = 2 });
            _accounts.Add(new AccountRow { Code = "4101-02", Name = "مبيعات آجلة", Type = "إيراد", Balance = 1100000.00m, Level = 2 });

            // المصروفات
            _accounts.Add(new AccountRow { Code = "5101", Name = "المصروفات", Type = "مصروف", Balance = 640000.00m, Level = 1 });
            _accounts.Add(new AccountRow { Code = "5101-01", Name = "الإيجار", Type = "مصروف", Balance = 360000.00m, Level = 2 });
            _accounts.Add(new AccountRow { Code = "5101-02", Name = "الرواتب والأجور", Type = "مصروف", Balance = 280000.00m, Level = 2 });
        }

        // قيود اليومية — كل قيد متوازن (المدين = الدائن).
        private void _BuildMockJournalEntries()
        {
            _journalEntries.Clear();

            _journalEntries.Add(new JournalEntryRow { Number = "2026-0001", Date = new DateTime(2026, 9, 1), Description = "سند قبض — مبيعات نقدية للعميل أحمد عبدالله", Debit = 150000.00m, Credit = 150000.00m, Status = "مرحّل" });
            _journalEntries.Add(new JournalEntryRow { Number = "2026-0002", Date = new DateTime(2026, 9, 3), Description = "قيد مشتريات آجلة من شركة النور للتوريدات", Debit = 320000.00m, Credit = 320000.00m, Status = "مرحّل" });
            _journalEntries.Add(new JournalEntryRow { Number = "2026-0003", Date = new DateTime(2026, 9, 5), Description = "صرف إيجار المحل — شهر سبتمبر", Debit = 360000.00m, Credit = 360000.00m, Status = "مرحّل" });
            _journalEntries.Add(new JournalEntryRow { Number = "2026-0004", Date = new DateTime(2026, 9, 8), Description = "سداد دفعة للموردين بشيك من البنك الأهلي", Debit = 200000.00m, Credit = 200000.00m, Status = "مرحّل" });
            _journalEntries.Add(new JournalEntryRow { Number = "2026-0005", Date = new DateTime(2026, 9, 10), Description = "صرف رواتب الموظفين — سبتمبر 2026", Debit = 280000.00m, Credit = 280000.00m, Status = "مسودة" });
            _journalEntries.Add(new JournalEntryRow { Number = "2026-0006", Date = new DateTime(2026, 9, 12), Description = "تسوية فرق جرد المخزون — نهاية الفترة", Debit = 45000.00m, Credit = 45000.00m, Status = "مسودة" });
        }

        private void _BuildMockCostCenters()
        {
            _costCenters.Clear();

            _costCenters.Add(new CostCenterRow { Code = "CC-01", Name = "الفرع الرئيسي", Budget = 15000000.00m, Responsible = "أحمد العريقي", Status = "نشط" });
            _costCenters.Add(new CostCenterRow { Code = "CC-02", Name = "فرع التواهي", Budget = 8000000.00m, Responsible = "محمد الحمادي", Status = "نشط" });
            _costCenters.Add(new CostCenterRow { Code = "CC-03", Name = "إدارة المبيعات والتسويق", Budget = 5000000.00m, Responsible = "سامي القباطي", Status = "نشط" });
            _costCenters.Add(new CostCenterRow { Code = "CC-04", Name = "المشتريات والتوريد", Budget = 4500000.00m, Responsible = "خالد العنسي", Status = "موقوف" });
        }

        private void _BuildMockFiscalPeriods()
        {
            _fiscalPeriods.Clear();

            _fiscalPeriods.Add(new FiscalPeriodRow { Year = 2025, StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 12, 31), Status = "مقفلة" });
            _fiscalPeriods.Add(new FiscalPeriodRow { Year = 2026, StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 12, 31), Status = "مفتوحة" });
        }

        // ── Mock Row Models ────────────────────────────────────────────────────────

        private sealed class AccountRow
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public decimal Balance { get; set; }
            public int Level { get; set; }
        }

        private sealed class JournalEntryRow
        {
            public string Number { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public string Description { get; set; } = string.Empty;
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        private sealed class CostCenterRow
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public decimal Budget { get; set; }
            public string Responsible { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
        }

        private sealed class FiscalPeriodRow
        {
            public int Year { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Status { get; set; } = string.Empty;
        }
    }
}