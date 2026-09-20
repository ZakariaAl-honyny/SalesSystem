namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// شاشة كشف حساب مورد — نموذج واجهة (UI Shape) يعمل ببيانات وهمية في الذاكرة
/// دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// الرصيد الجاري يُحسب تراكمياً حسب الترتيب الزمني:
/// فاتورة شراء = مدين (تزيد الرصيد) · دفع / مردود = دائن (تخفض الرصيد).
/// </summary>
public partial class frmSupplierStatement : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── نموذج حركة الكشف (بيانات وهمية في الذاكرة) ────────────────────────────

    private sealed class StatementLine
    {
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";        // فاتورة / دفع / مردود
        public string Reference { get; set; } = "";
        public decimal Debit { get; set; }            // مدين (فاتورة شراء)
        public decimal Credit { get; set; }           // دائن (دفع / مردود)
    }

    private readonly List<StatementLine> _allLines = new()
    {
        new() { Date = new DateTime(2026, 9, 1),  Type = "فاتورة", Reference = "مشتريات-2026-0001", Debit = 250000.00m },
        new() { Date = new DateTime(2026, 9, 3),  Type = "دفع",    Reference = "سند-صرف-2026-0142", Credit = 100000.00m },
        new() { Date = new DateTime(2026, 9, 7),  Type = "فاتورة", Reference = "مشتريات-2026-0005", Debit = 180000.00m },
        new() { Date = new DateTime(2026, 9, 10), Type = "مردود",  Reference = "مرتجع-2026-0002",    Credit = 25000.00m },
        new() { Date = new DateTime(2026, 9, 12), Type = "دفع",    Reference = "سند-صرف-2026-0150", Credit = 200000.00m },
        new() { Date = new DateTime(2026, 9, 15), Type = "فاتورة", Reference = "مشتريات-2026-0009", Debit = 320000.00m },
        new() { Date = new DateTime(2026, 9, 18), Type = "دفع",    Reference = "سند-صرف-2026-0161", Credit = 150000.00m },
        new() { Date = new DateTime(2026, 9, 20), Type = "فاتورة", Reference = "مشتريات-2026-0012", Debit = 95000.00m },
    };

    public frmSupplierStatement(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // ── تحميل الكشف (بيانات وهمية) ────────────────────────────────────────────

    private void frmSupplierStatement_Load(object sender, EventArgs e)
    {
        _SetupGridColumns();
        dtpFrom.Value = new DateTime(2026, 9, 1);
        dtpTo.Value = new DateTime(2026, 9, 30);
        _RefreshStatement();
    }

    private void _RefreshStatement()
    {
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        // الرصيد السابق = صافي حركات المورد قبل تاريخ البداية (يُحمَل من فترة سابقة)
        decimal opening = _allLines.Where(l => l.Date < from).Sum(l => l.Debit - l.Credit);

        var visible = _allLines
            .Where(l => l.Date >= from && l.Date <= to)
            .OrderBy(l => l.Date)
            .ToList();

        dgvStatement.Rows.Clear();

        // صف الرصيد السابق (افتتاحي) إن وُجد
        if (opening != 0)
        {
            int openingRow = dgvStatement.Rows.Add(
                from.ToString("dd/MM/yyyy"), "رصيد سابق", "—", "", "", opening.ToString("N2"));
            dgvStatement.Rows[openingRow].Tag = "Opening";
        }

        // الحركات مع الرصيد الجاري التراكمي
        decimal running = opening;
        foreach (var line in visible)
        {
            running += line.Debit - line.Credit;
            dgvStatement.Rows.Add(
                line.Date.ToString("dd/MM/yyyy"),
                line.Type,
                line.Reference,
                line.Debit > 0 ? line.Debit.ToString("N2") : "",
                line.Credit > 0 ? line.Credit.ToString("N2") : "",
                running.ToString("N2"));
        }

        // بطاقات الملخص
        decimal totalDebit = visible.Sum(l => l.Debit);
        decimal totalCredit = visible.Sum(l => l.Credit);
        decimal closing = opening + totalDebit - totalCredit;

        cardPurchasesValue.Text = totalDebit.ToString("N2");
        cardPaymentsValue.Text = totalCredit.ToString("N2");
        cardBalanceValue.Text = closing.ToString("N2");
        cardBalanceValue.ForeColor = closing < 0
            ? Color.FromArgb(192, 57, 43)
            : Color.FromArgb(52, 73, 94);

        lblRecordsCount.Text = visible.Count.ToString();
    }

    // ── إعداد أعمدة الشبكة ────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvStatement.AutoGenerateColumns = false;
        dgvStatement.Columns.Clear();

        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDate",      HeaderText = "التاريخ", FillWeight = 13, MinimumWidth = 90,  ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColType",      HeaderText = "النوع",   FillWeight = 11, MinimumWidth = 85,  ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColReference", HeaderText = "المرجع",  FillWeight = 17, MinimumWidth = 110, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDebit",     HeaderText = "مدين",    FillWeight = 14, MinimumWidth = 90,  ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCredit",    HeaderText = "دائن",    FillWeight = 14, MinimumWidth = 90,  ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvStatement.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColBalance",   HeaderText = "الرصيد",  FillWeight = 15, MinimumWidth = 100, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
    }

    // ── تنسيق الشبكة (ألوان النوع / المبالغ / الرصيد) ─────────────────────────

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
                    "فاتورة" => Color.FromArgb(41, 128, 185),   // أزرق
                    "دفع"    => Color.FromArgb(39, 174, 96),    // أخضر
                    "مردود"  => Color.FromArgb(211, 84, 0),     // برتقالي
                    _        => Color.FromArgb(52, 73, 94)
                };
                break;

            case "ColDebit":
                if (e.Value is string debitText && debitText.Length > 0)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(41, 128, 185);
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
                    e.CellStyle.ForeColor = balance < 0
                        ? Color.FromArgb(192, 57, 43)
                        : Color.FromArgb(52, 73, 94);
                }
                break;
        }
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
        => _RefreshStatement();

    private void dtpTo_ValueChanged(object sender, EventArgs e)
        => _RefreshStatement();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}