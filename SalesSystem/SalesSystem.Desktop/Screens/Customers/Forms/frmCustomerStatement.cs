using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

/// <summary>
/// شاشة كشف حساب عميل — تعرض حركات العميل (فواتير بيع، مرتجعات، سندات قبض، قيود يومية)
/// مع رصيد جاري تراكمي وتحليل أعمار الديون.
/// البيانات تُجمع من خدمات المحاكاة (Mock Services) الموجودة مسبقاً.
/// </summary>
public partial class frmCustomerStatement : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── نموذج حركة الكشف ────────────────────────────────────────────────────────
    private sealed class StatementMovement
    {
        public DateTime Date { get; set; }
        public string SortKey => Date.ToString("yyyyMMddHHmmss");
        public string Type { get; set; } = "";
        public string Reference { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }

    // ── نموذج شريحة أعمار الديون ────────────────────────────────────────────────
    private sealed class AgingBucket
    {
        public string Label { get; set; } = "";
        public int Count { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
    }

    private List<CustomerDto> _customers = new();
    private List<SalesInvoiceDto> _allInvoices = new();
    private List<SalesReturnDto> _allReturns = new();
    private List<CustomerReceiptDto> _allReceipts = new();
    private List<JournalEntryListDto> _allJournalEntries = new();

    public frmCustomerStatement(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // ── تحميل الشاشة ───────────────────────────────────────────────────────────

    private async void frmCustomerStatement_Load(object sender, EventArgs e)
    {
        _SetupGridColumns();
        _SetupAgingGridColumns();
        await _LoadBaseDataAsync();

        dtpFrom.Value = new DateTime(2026, 9, 1);
        dtpTo.Value = DateTime.Today;
    }

    // ── تحميل البيانات الأساسية من خدمات المحاكاة ─────────────────────────────

    private async Task _LoadBaseDataAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            // تحميل العملاء
            var customerResult = await _serviceProvider.GetRequiredService<ICustomerApiService>()
                .GetAllAsync(includeInactive: false);
            if (customerResult.IsSuccess && customerResult.Value != null)
                _customers = customerResult.Value.ToList();

            // تحميل فواتير البيع
            var invoiceResult = await _serviceProvider.GetRequiredService<ISalesInvoiceApiService>()
                .GetAllAsync(status: 2); // المرحلة فقط
            if (invoiceResult.IsSuccess && invoiceResult.Value != null)
                _allInvoices = invoiceResult.Value.ToList();

            // تحميل مرتجعات المبيعات
            var returnResult = await _serviceProvider.GetRequiredService<ISalesReturnApiService>()
                .GetAllAsync();
            if (returnResult.IsSuccess && returnResult.Value != null)
                _allReturns = returnResult.Value.ToList();

            // تحميل سندات القبض
            var receiptResult = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>()
                .GetAllAsync();
            if (receiptResult.IsSuccess && receiptResult.Value != null)
                _allReceipts = receiptResult.Value.ToList();

            // تحميل القيود اليومية
            var journalResult = await _serviceProvider.GetRequiredService<IJournalEntryApiService>()
                .GetAllAsync();
            if (journalResult.IsSuccess && journalResult.Value != null)
                _allJournalEntries = journalResult.Value.ToList();

            // تعبئة قائمة العملاء
            cmbCustomer.Items.Clear();
            foreach (var c in _customers)
                cmbCustomer.Items.Add($"{c.Id} - {c.Name}");

            if (cmbCustomer.Items.Count > 0)
                cmbCustomer.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── إعداد أعمدة الشبكة الرئيسية ────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvStatement.AutoGenerateColumns = false;
        dgvStatement.Columns.Clear();

        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColDate", HeaderText = "التاريخ", FillWeight = 12, MinimumWidth = 90,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColType", HeaderText = "النوع", FillWeight = 11, MinimumWidth = 90,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColReference", HeaderText = "المرجع", FillWeight = 12, MinimumWidth = 100,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColDescription", HeaderText = "البيان", FillWeight = 20, MinimumWidth = 160,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColDebit", HeaderText = "مدين", FillWeight = 13, MinimumWidth = 90,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCredit", HeaderText = "دائن", FillWeight = 13, MinimumWidth = 90,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColBalance", HeaderText = "الرصيد", FillWeight = 14, MinimumWidth = 100,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
    }

    // ── إعداد أعمدة شبكة تحليل الأعمار ─────────────────────────────────────────

    private void _SetupAgingGridColumns()
    {
        dgvAging.AutoGenerateColumns = false;
        dgvAging.Columns.Clear();

        dgvAging.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColPeriod", HeaderText = "الفترة", FillWeight = 35, MinimumWidth = 200,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvAging.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColInvCount", HeaderText = "عدد الفواتير", FillWeight = 15, MinimumWidth = 90,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvAging.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColAmount", HeaderText = "المبلغ المستحق", FillWeight = 25, MinimumWidth = 120,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvAging.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColPercent", HeaderText = "النسبة", FillWeight = 15, MinimumWidth = 80,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
    }

    // ── تحديث الكشف ────────────────────────────────────────────────────────────

    private async void _RefreshStatement()
    {
        if (cmbCustomer.SelectedIndex < 0) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            var customer = _customers[cmbCustomer.SelectedIndex];
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date.AddDays(1).AddTicks(-1);

            // ── جمع الحركات الخام ──────────────────────────────────────────────
            var rawMovements = new List<StatementMovement>();

            // 1. فواتير البيع المرحلة لهذا العميل
            foreach (var inv in _allInvoices.Where(i => i.CustomerId == customer.Id))
            {
                if (inv.PaymentType == 2) // بيع آجل
                {
                    // المدين = إجمالي الفاتورة
                    rawMovements.Add(new StatementMovement
                    {
                        Date = inv.InvoiceDate,
                        Type = "فاتورة بيع",
                        Reference = inv.InvoiceNo.ToString(),
                        Description = $"فاتورة بيع آجلة [{inv.Items.Count} صنف]",
                        Debit = inv.NetTotal,
                        Credit = 0
                    });

                    // إذا دُفع جزء مع الفاتورة
                    if (inv.PaidAmount > 0)
                    {
                        rawMovements.Add(new StatementMovement
                        {
                            Date = inv.InvoiceDate,
                            Type = "دفعة مع الفاتورة",
                            Reference = $"DP-{inv.InvoiceNo}",
                            Description = $"دفعة نقدية مسددة فورياً مع الفاتورة {inv.InvoiceNo}",
                            Debit = 0,
                            Credit = inv.PaidAmount
                        });
                    }
                }
                else if (inv.PaymentType == 1) // بيع نقدي
                {
                    // بيع نقدي مرتبط بالعميل — مدين ودائن متساويان
                    rawMovements.Add(new StatementMovement
                    {
                        Date = inv.InvoiceDate,
                        Type = "فاتورة بيع نقدية",
                        Reference = inv.InvoiceNo.ToString(),
                        Description = "فاتورة بيع نقدية مسددة فوراً",
                        Debit = inv.NetTotal,
                        Credit = inv.NetTotal
                    });
                }
                else if (inv.PaymentType == 3) // مختلط
                {
                    rawMovements.Add(new StatementMovement
                    {
                        Date = inv.InvoiceDate,
                        Type = "فاتورة بيع",
                        Reference = inv.InvoiceNo.ToString(),
                        Description = $"فاتورة بيع — دفع مختلط [{inv.Items.Count} صنف]",
                        Debit = inv.NetTotal,
                        Credit = 0
                    });

                    if (inv.PaidAmount > 0)
                    {
                        rawMovements.Add(new StatementMovement
                        {
                            Date = inv.InvoiceDate,
                            Type = "دفعة مختلطة",
                            Reference = $"DP-{inv.InvoiceNo}",
                            Description = "جزء نقدي مدفوع مع الفاتورة",
                            Debit = 0,
                            Credit = inv.PaidAmount
                        });
                    }
                }
            }

            // 2. مرتجعات المبيعات المرحلة لهذا العميل
            foreach (var ret in _allReturns.Where(r => r.CustomerId == customer.Id && r.Status == 2))
            {
                rawMovements.Add(new StatementMovement
                {
                    Date = ret.ReturnDate,
                    Type = "مردود مبيعات",
                    Reference = ret.ReturnNo.ToString(),
                    Description = $"مردود مبيعات [{ret.Items.Count} صنف]",
                    Debit = 0,
                    Credit = ret.TotalAmount
                });
            }

            // 3. سندات القبض المرحلة لهذا العميل
            foreach (var receipt in _allReceipts.Where(r => r.CustomerId == customer.Id && r.Status == 2))
            {
                string payMethod = receipt.PaymentMethodName ?? "نقدي";
                rawMovements.Add(new StatementMovement
                {
                    Date = receipt.ReceiptDate,
                    Type = "سند قبض",
                    Reference = receipt.ReceiptNo.ToString(),
                    Description = $"سند قبض ({payMethod})",
                    Debit = 0,
                    Credit = receipt.Amount
                });
            }

            // 4. القيود اليومية المرحلة المرتبطة بحساب العميل
            foreach (var entry in _allJournalEntries.Where(e => e.Status == 2))
            {
                try
                {
                    var detailResult = await _serviceProvider.GetRequiredService<IJournalEntryApiService>()
                        .GetByIdAsync(entry.Id);
                    if (detailResult.IsSuccess && detailResult.Value != null)
                    {
                        foreach (var line in detailResult.Value.Lines)
                        {
                            if (line.AccountId == customer.AccountId)
                            {
                                rawMovements.Add(new StatementMovement
                                {
                                    Date = entry.EntryDate,
                                    Type = "قيد يومية",
                                    Reference = entry.EntryNumber,
                                    Description = line.Description ?? entry.Description,
                                    Debit = line.Debit,
                                    Credit = line.Credit
                                });
                            }
                        }
                    }
                }
                catch
                {
                    // تجاهل أخطاء القيود الفردية
                }
            }

            // ── ترتيب زمني ─────────────────────────────────────────────────────
            rawMovements = rawMovements.OrderBy(m => m.SortKey).ToList();

            // ── حساب الرصيد السابق (قبل الفترة) ───────────────────────────────
            decimal openingBalance = 0m;
            var periodMovements = new List<StatementMovement>();

            foreach (var m in rawMovements)
            {
                if (m.Date < from)
                {
                    openingBalance += m.Debit - m.Credit;
                }
                else if (m.Date <= to)
                {
                    periodMovements.Add(m);
                }
            }

            // ── ملء الشبكة مع الرصيد الجاري ────────────────────────────────────
            dgvStatement.Rows.Clear();

            // صف الرصيد السابق
            if (openingBalance != 0)
            {
                int rowIdx = dgvStatement.Rows.Add(
                    from.ToString("dd/MM/yyyy"),
                    "رصيد سابق",
                    "—",
                    "الرصيد المحمل من فترة سابقة",
                    "",
                    "",
                    openingBalance.ToString("N2"));
                dgvStatement.Rows[rowIdx].Tag = "Opening";
            }

            decimal running = openingBalance;
            decimal totalDebit = 0, totalCredit = 0;

            foreach (var m in periodMovements)
            {
                running += m.Debit - m.Credit;
                totalDebit += m.Debit;
                totalCredit += m.Credit;

                dgvStatement.Rows.Add(
                    m.Date.ToString("dd/MM/yyyy"),
                    m.Type,
                    m.Reference,
                    m.Description,
                    m.Debit > 0 ? m.Debit.ToString("N2") : "",
                    m.Credit > 0 ? m.Credit.ToString("N2") : "",
                    running.ToString("N2"));
            }

            // ── بطاقات الملخص ──────────────────────────────────────────────────
            cardOpeningValue.Text = openingBalance.ToString("N2");
            cardDebitValue.Text = totalDebit.ToString("N2");
            cardCreditValue.Text = totalCredit.ToString("N2");
            cardClosingValue.Text = running.ToString("N2");
            cardClosingValue.ForeColor = running > 0
                ? Color.FromArgb(192, 57, 43)   // مدين (عليه)
                : running < 0
                    ? Color.FromArgb(39, 174, 96) // دائن (له)
                    : Color.FromArgb(52, 73, 94);

            lblRecordsCount.Text = periodMovements.Count.ToString();

            // ── تحليل أعمار الديون ──────────────────────────────────────────────
            _CalculateAging(customer.Id, from);
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء إعداد كشف الحساب: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── حساب تحليل أعمار الديون ────────────────────────────────────────────────

    private void _CalculateAging(int customerId, DateTime asOfDate)
    {
        // فواتير بيع آجلةمرحلة بها مبلغ متبقي
        var creditInvoices = _allInvoices.Where(inv =>
            inv.CustomerId == customerId &&
            inv.PaymentType == 2 &&
            inv.Status == 2 &&
            inv.RemainingAmount > 0).ToList();

        var b0_30 = new AgingBucket { Label = "0 - 30 يوم (حالي)" };
        var b31_60 = new AgingBucket { Label = "31 - 60 يوم (متأخر أول)" };
        var b61_90 = new AgingBucket { Label = "61 - 90 يوم (متأخر ثانٍ)" };
        var b91Plus = new AgingBucket { Label = "+90 يوم (حرج)" };

        foreach (var inv in creditInvoices)
        {
            int days = (int)(asOfDate - inv.InvoiceDate).TotalDays;
            if (days < 0) days = 0;

            if (days <= 30)
            {
                b0_30.Count++;
                b0_30.Amount += inv.RemainingAmount;
            }
            else if (days <= 60)
            {
                b31_60.Count++;
                b31_60.Amount += inv.RemainingAmount;
            }
            else if (days <= 90)
            {
                b61_90.Count++;
                b61_90.Amount += inv.RemainingAmount;
            }
            else
            {
                b91Plus.Count++;
                b91Plus.Amount += inv.RemainingAmount;
            }
        }

        decimal totalOverdue = b0_30.Amount + b31_60.Amount + b61_90.Amount + b91Plus.Amount;

        b0_30.Percentage = totalOverdue > 0 ? Math.Round(b0_30.Amount / totalOverdue * 100, 1) : 0;
        b31_60.Percentage = totalOverdue > 0 ? Math.Round(b31_60.Amount / totalOverdue * 100, 1) : 0;
        b61_90.Percentage = totalOverdue > 0 ? Math.Round(b61_90.Amount / totalOverdue * 100, 1) : 0;
        b91Plus.Percentage = totalOverdue > 0 ? Math.Round(b91Plus.Amount / totalOverdue * 100, 1) : 0;

        var buckets = new[] { b0_30, b31_60, b61_90, b91Plus };

        dgvAging.Rows.Clear();
        foreach (var b in buckets)
        {
            dgvAging.Rows.Add(
                b.Label,
                b.Count.ToString(),
                b.Amount.ToString("N2"),
                b.Percentage.ToString("N1") + " %");
        }

        // صف الإجمالي
        int totalRow = dgvAging.Rows.Add(
            "الإجمالي",
            creditInvoices.Count.ToString(),
            totalOverdue.ToString("N2"),
            totalOverdue > 0 ? "100 %" : "0 %");
        dgvAging.Rows[totalRow].Tag = "Total";
        dgvAging.Rows[totalRow].DefaultCellStyle.Font = new Font(dgvAging.Font, FontStyle.Bold);
    }

    // ── تنسيق الشبكة ───────────────────────────────────────────────────────────

    private void dgvStatement_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvStatement.Rows.Count) return;

        bool isOpeningRow = dgvStatement.Rows[e.RowIndex].Tag as string == "Opening";

        if (isOpeningRow)
        {
            e.CellStyle.BackColor = Color.FromArgb(241, 242, 246);
            e.CellStyle.ForeColor = Color.FromArgb(108, 117, 125);
            e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
            return;
        }

        string type = dgvStatement.Rows[e.RowIndex].Cells["ColType"].Value?.ToString() ?? "";

        switch (dgvStatement.Columns[e.ColumnIndex].Name)
        {
            case "ColType":
                e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = type switch
                {
                    "فاتورة بيع" or "فاتورة بيع نقدية" => Color.FromArgb(41, 128, 185),   // أزرق
                    "سند قبض" or "دفعة مع الفاتورة" or "دفعة مختلطة" => Color.FromArgb(39, 174, 96),    // أخضر
                    "مردود مبيعات" => Color.FromArgb(211, 84, 0),     // برتقالي
                    "قيد يومية" => Color.FromArgb(142, 68, 173),      // بنفسجي
                    _ => Color.FromArgb(52, 73, 94)
                };
                break;

            case "ColDebit":
                if (e.Value is string debitText && debitText.Length > 0)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(192, 57, 43);
                    e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                }
                break;

            case "ColCredit":
                if (e.Value is string creditText && creditText.Length > 0)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                    e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                }
                break;

            case "ColBalance":
                if (decimal.TryParse(e.Value?.ToString(), out decimal balance))
                {
                    e.CellStyle.Font = new Font(dgvStatement.Font, FontStyle.Bold);
                    e.CellStyle.ForeColor = balance > 0
                        ? Color.FromArgb(192, 57, 43)   // مدين
                        : balance < 0
                            ? Color.FromArgb(39, 174, 96) // دائن
                            : Color.FromArgb(52, 73, 94);
                }
                break;
        }
    }

    private void dgvAging_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvAging.Rows.Count) return;

        if (dgvAging.Rows[e.RowIndex].Tag as string == "Total")
        {
            e.CellStyle.BackColor = Color.FromArgb(52, 73, 94);
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.Font = new Font(dgvAging.Font, FontStyle.Bold);
        }
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbCustomer.SelectedIndex >= 0 && cmbCustomer.SelectedIndex < _customers.Count)
        {
            var customer = _customers[cmbCustomer.SelectedIndex];
            lblCustomerInfo.Text = $"الرصيد الحالي: {customer.CurrentBalance:N2} | الحد الائتماني: {customer.CreditLimit:N2}";
        }
        _RefreshStatement();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
        => _RefreshStatement();

    private void dtpTo_ValueChanged(object sender, EventArgs e)
        => _RefreshStatement();

    private void btnRefresh_Click(object sender, EventArgs e)
        => _RefreshStatement();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
