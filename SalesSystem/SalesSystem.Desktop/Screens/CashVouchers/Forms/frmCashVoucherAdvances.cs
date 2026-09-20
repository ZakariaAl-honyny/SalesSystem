using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

/// <summary>
/// نافذة سلف وعهد الموظفين — نموذج واجهة (UI Shape) يعمل ببيانات وهمية
/// في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض بطاقات ملخص (السلف المستحقة / العهد المفتوحة / عدد الموظفين)
/// وشريط فلترة وشبكة بالسلف والعهد، مع أزرار إضافة وإغلاق.
/// </summary>
public partial class frmCashVoucherAdvances : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<AdvanceItem> _allAdvances = new();
    private List<AdvanceItem> _filteredAdvances = new();

    public frmCashVoucherAdvances(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── نموذج البيانات (بيانات وهمية) ─────────────────────────────────────────

    private sealed class AdvanceItem
    {
        public int Id { get; set; }
        public string Employee { get; set; } = "";   // الموظف
        public string Type { get; set; } = "";        // سلفة / عهدة
        public DateTime Date { get; set; }            // التاريخ
        public decimal Amount { get; set; }           // المبلغ (ريال يمني)
        public string Account { get; set; } = "";     // الحساب (115 — سلف وعهد الموظفين)
        public string Status { get; set; } = "";      // مفتوحة / مسددة
    }

    // ── تحميل البيانات الوهمية ────────────────────────────────────────────────

    private void frmCashVoucherAdvances_Load(object sender, EventArgs e)
    {
        _SetupGridColumns();
        _LoadMockData();
        _ApplyFilters();
    }

    private void _LoadMockData()
    {
        _allAdvances = new List<AdvanceItem>
        {
            new() { Id = 1, Employee = "أحمد محمد", Type = "سلفة", Date = new DateTime(2026, 9, 5),  Amount = 50000m, Account = "سلف وعهد الموظفين (115)", Status = "مفتوحة" },
            new() { Id = 2, Employee = "أحمد محمد", Type = "عهدة", Date = new DateTime(2026, 9, 10), Amount = 20000m, Account = "سلف وعهد الموظفين (115)", Status = "مفتوحة" },
            new() { Id = 3, Employee = "فاطمة علي",  Type = "سلفة", Date = new DateTime(2026, 9, 2),  Amount = 35000m, Account = "سلف وعهد الموظفين (115)", Status = "مسددة" },
            new() { Id = 4, Employee = "خالد حسن",   Type = "سلفة", Date = new DateTime(2026, 8, 28), Amount = 75000m, Account = "سلف وعهد الموظفين (115)", Status = "مفتوحة" },
            new() { Id = 5, Employee = "سعاد صالح",  Type = "عهدة", Date = new DateTime(2026, 9, 12), Amount = 15000m, Account = "سلف وعهد الموظفين (115)", Status = "مفتوحة" },
            new() { Id = 6, Employee = "خالد حسن",   Type = "سلفة", Date = new DateTime(2026, 8, 15), Amount = 25000m, Account = "سلف وعهد الموظفين (115)", Status = "مسددة" },
        };

        cbStatus.SelectedIndex = 0; // الكل
    }

    // ── إعداد أعمدة الشبكة ────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvAdvances.AutoGenerateColumns = false;
        dgvAdvances.Columns.Clear();

        dgvAdvances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Employee", HeaderText = "الموظف", FillWeight = 20, MinimumWidth = 110, ReadOnly = true });
        dgvAdvances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type",     HeaderText = "النوع",   FillWeight = 10, MinimumWidth = 70,  ReadOnly = true });
        dgvAdvances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date",     HeaderText = "التاريخ", FillWeight = 12, MinimumWidth = 90,  ReadOnly = true });
        dgvAdvances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Amount",   HeaderText = "المبلغ",  FillWeight = 14, MinimumWidth = 90,  ReadOnly = true });
        dgvAdvances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Account",  HeaderText = "الحساب",  FillWeight = 16, MinimumWidth = 130, ReadOnly = true });
        dgvAdvances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status",   HeaderText = "الحالة",  FillWeight = 11, MinimumWidth = 80,  ReadOnly = true });
    }

    // ── الفلترة في الذاكرة ────────────────────────────────────────────────────

    private void _ApplyFilters()
    {
        IEnumerable<AdvanceItem> query = _allAdvances;

        string search = txtSearch.Text.Trim();
        if (search.Length > 0)
            query = query.Where(a => a.Employee.Contains(search, StringComparison.OrdinalIgnoreCase));

        // قائمة «الحالة» تعرض (الكل / سلفة / عهدة) وتفلتر حسب نوع السجل
        if (cbStatus.SelectedIndex > 0)
            query = query.Where(a => a.Type == cbStatus.SelectedItem?.ToString());

        _filteredAdvances = query.ToList();
        _PopulateGrid();
        _UpdateStats();
    }

    private void _PopulateGrid()
    {
        dgvAdvances.Rows.Clear();

        foreach (var a in _filteredAdvances)
        {
            dgvAdvances.Rows.Add(
                a.Employee,
                a.Type,
                a.Date.ToString("dd/MM/yyyy"),
                a.Amount.ToString("N0"),
                a.Account,
                a.Status
            );
        }
    }

    private void _UpdateStats()
    {
        // البطاقات تُحسب من جميع السجلات (بغض النظر عن الفلترة)
        decimal advancesDue = _allAdvances
            .Where(a => a.Type == "سلفة" && a.Status == "مفتوحة")
            .Sum(a => a.Amount);

        decimal trustsOpen = _allAdvances
            .Where(a => a.Type == "عهدة" && a.Status == "مفتوحة")
            .Sum(a => a.Amount);

        int employees = _allAdvances
            .Select(a => a.Employee)
            .Distinct()
            .Count();

        cardAdvancesValue.Text = advancesDue.ToString("N0") + " YER";
        cardTrustsValue.Text = trustsOpen.ToString("N0") + " YER";
        cardEmployeesValue.Text = employees.ToString();

        lblRecordsCount.Text = _filteredAdvances.Count.ToString();
    }

    // ── مساعدات ───────────────────────────────────────────────────────────────

    private AdvanceItem? _GetItemAt(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _filteredAdvances.Count) return null;
        return _filteredAdvances[rowIndex];
    }

    private AdvanceItem? _GetSelectedItem()
    {
        if (dgvAdvances.CurrentRow == null || dgvAdvances.CurrentRow.Index < 0) return null;
        return _GetItemAt(dgvAdvances.CurrentRow.Index);
    }

    private void _ShowDetails(AdvanceItem item)
    {
        string details =
            $"الموظف: {item.Employee}\n" +
            $"النوع: {item.Type}\n" +
            $"التاريخ: {item.Date:dd/MM/yyyy}\n" +
            $"المبلغ: {item.Amount.ToString("N0")} YER\n" +
            $"الحساب: {item.Account}\n" +
            $"الحالة: {item.Status}";

        MessageBox.Show(details, "تفاصيل سلفة / عهدة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAdd_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "سيتم فتح شاشة إضافة سلفة أو عهدة جديدة للموظف.\n(وضع تجريبي — البيانات المعروضة وهمية)",
            "إضافة سلفة / عهدة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void dgvAdvances_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var item = _GetItemAt(e.RowIndex);
        if (item == null) return;

        switch (dgvAdvances.Columns[e.ColumnIndex].Name)
        {
            case "Type":
                e.CellStyle.ForeColor = item.Type == "سلفة"
                    ? Color.FromArgb(255, 110, 110)
                    : Color.FromArgb(94, 168, 235);
                e.CellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
                break;

            case "Amount":
                e.CellStyle.ForeColor = item.Type == "سلفة"
                    ? Color.FromArgb(255, 110, 110)
                    : Color.FromArgb(94, 168, 235);
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                e.CellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
                break;

            case "Status":
                e.CellStyle.ForeColor = item.Status == "مفتوحة"
                    ? Color.FromArgb(102, 214, 118)
                    : Color.FromArgb(160, 160, 160);
                e.CellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
                break;

            case "Date":
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                break;
        }
    }

    private void dgvAdvances_DoubleClick(object sender, EventArgs e)
    {
        var item = _GetSelectedItem();
        if (item != null) _ShowDetails(item);
    }

    private void cmsAdvances_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsAdvances.Enabled = dgvAdvances.Rows.Count > 0 && dgvAdvances.CurrentRow != null;
    }

    private void viewToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var item = _GetSelectedItem();
        if (item != null) _ShowDetails(item);
    }
}