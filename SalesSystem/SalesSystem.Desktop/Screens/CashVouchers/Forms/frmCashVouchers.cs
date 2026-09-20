using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

/// <summary>
/// شاشة سندات القبض والصرف والخزينة — نموذج واجهة (UI Shape) يعمل ببيانات وهمية
/// في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// </summary>
public partial class frmCashVouchers : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<CashVoucherItem> _allVouchers = new();
    private List<CashVoucherItem> _filteredVouchers = new();

    public frmCashVouchers(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── نموذج البيانات (بيانات وهمية) ─────────────────────────────────────────

    private sealed class CashVoucherItem
    {
        public int Id { get; set; }
        public string VoucherNo { get; set; } = "";
        public string Type { get; set; } = "";        // قبض / صرف / تحويل
        public DateTime Date { get; set; }
        public string Party { get; set; } = "";       // الجهة
        public string Account { get; set; } = "";     // الحساب
        public decimal Amount { get; set; }           // المبلغ (ريال يمني)
        public string Method { get; set; } = "";      // نقد / بنك / محفظة
        public string Status { get; set; } = "";      // مرحل / مسودة / ملغي
    }

    // ── تحميل البيانات الوهمية ────────────────────────────────────────────────

    private void frmCashVouchers_Load(object sender, EventArgs e)
    {
        _SetupGridColumns();
        _LoadMockData();
        _ApplyFilters();
    }

    private void _LoadMockData()
    {
        _allVouchers = new List<CashVoucherItem>
        {
            new() { Id = 1, VoucherNo = "قبض-2026-0001", Type = "قبض", Date = new DateTime(2026, 9, 1),  Party = "أحمد محمد",     Account = "الصندوق الرئيسي (111)",  Amount = 150000m, Method = "نقد",    Status = "مرحل" },
            new() { Id = 2, VoucherNo = "قبض-2026-0002", Type = "قبض", Date = new DateTime(2026, 9, 3),  Party = "شركة النور",    Account = "بنك (112)",              Amount = 480000m, Method = "بنك",    Status = "مرحل" },
            new() { Id = 3, VoucherNo = "قبض-2026-0003", Type = "قبض", Date = new DateTime(2026, 9, 5),  Party = "محمد السالمي",  Account = "محفظة (113)",            Amount = 95000m,  Method = "محفظة", Status = "مسودة" },
            new() { Id = 4, VoucherNo = "صرف-2026-0001", Type = "صرف", Date = new DateTime(2026, 9, 2),  Party = "شركة النور",    Account = "الصندوق الرئيسي (111)",  Amount = 320000m, Method = "نقد",    Status = "مرحل" },
            new() { Id = 5, VoucherNo = "صرف-2026-0002", Type = "صرف", Date = new DateTime(2026, 9, 6),  Party = "مؤسسة الخدمات", Account = "بنك (112)",              Amount = 210000m, Method = "بنك",    Status = "مرحل" },
            new() { Id = 6, VoucherNo = "صرف-2026-0003", Type = "صرف", Date = new DateTime(2026, 9, 8),  Party = "أحمد محمد",     Account = "الصندوق الرئيسي (111)",  Amount = 45000m,  Method = "نقد",    Status = "مسودة" },
            new() { Id = 7, VoucherNo = "تحويل-2026-0001", Type = "تحويل", Date = new DateTime(2026, 9, 4), Party = "تحويل داخلي", Account = "الصندوق الرئيسي (111) ← بنك (112)", Amount = 100000m, Method = "بنك", Status = "مرحل" },
            new() { Id = 8, VoucherNo = "صرف-2026-0004", Type = "صرف", Date = new DateTime(2026, 9, 7),  Party = "شركة الكهرباء", Account = "الصندوق الرئيسي (111)",  Amount = 60000m,  Method = "نقد",    Status = "ملغي" },
        };

        cbType.SelectedIndex = 0;    // الكل
        cbMethod.SelectedIndex = 0;  // الكل
        cbStatus.SelectedIndex = 0;  // الكل
    }

    // ── إعداد أعمدة الشبكة ────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvVouchers.AutoGenerateColumns = false;
        dgvVouchers.Columns.Clear();

        dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn { Name = "VoucherNo", HeaderText = "رقم السند", FillWeight = 10, MinimumWidth = 110, ReadOnly = true });
        dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type",      HeaderText = "النوع",     FillWeight = 8,  MinimumWidth = 70,  ReadOnly = true });
        dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date",      HeaderText = "التاريخ",   FillWeight = 11, MinimumWidth = 100, ReadOnly = true });
        dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Party",     HeaderText = "الجهة",     FillWeight = 16, MinimumWidth = 130, ReadOnly = true });
        dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Account",   HeaderText = "الحساب",    FillWeight = 17, MinimumWidth = 140, ReadOnly = true });
        dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Amount",    HeaderText = "المبلغ",    FillWeight = 12, MinimumWidth = 100, ReadOnly = true });
        dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status",    HeaderText = "الحالة",    FillWeight = 9,  MinimumWidth = 80,  ReadOnly = true });

        // أزرار الإجراءات (معاينة / طباعة / عرض / إلغاء)
        dgvVouchers.Columns.Add(new DataGridViewButtonColumn { Name = "ColPreview", HeaderText = "معاينة", FillWeight = 9,  MinimumWidth = 70, Text = "معاينة", UseColumnTextForButtonValue = true, FlatStyle = FlatStyle.Flat });
        dgvVouchers.Columns.Add(new DataGridViewButtonColumn { Name = "ColPrint",   HeaderText = "طباعة",  FillWeight = 8,  MinimumWidth = 65, Text = "طباعة",  UseColumnTextForButtonValue = true, FlatStyle = FlatStyle.Flat });
        dgvVouchers.Columns.Add(new DataGridViewButtonColumn { Name = "ColView",    HeaderText = "عرض",     FillWeight = 8,  MinimumWidth = 65, Text = "عرض",     UseColumnTextForButtonValue = true, FlatStyle = FlatStyle.Flat });
        dgvVouchers.Columns.Add(new DataGridViewButtonColumn { Name = "ColCancel",  HeaderText = "إلغاء",   FillWeight = 9,  MinimumWidth = 70, Text = "إلغاء",   UseColumnTextForButtonValue = true, FlatStyle = FlatStyle.Flat });

        // تنسيق داكن لأزرار الإجراءات
        foreach (DataGridViewColumn col in dgvVouchers.Columns)
        {
            if (col is DataGridViewButtonColumn btnCol)
            {
                btnCol.DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(56, 56, 56),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(0, 120, 215),
                    SelectionForeColor = Color.White,
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
                };
            }
        }
    }

    // ── الفلترة في الذاكرة ────────────────────────────────────────────────────

    private void _ApplyFilters()
    {
        IEnumerable<CashVoucherItem> query = _allVouchers;

        string search = txtSearch.Text.Trim();
        if (search.Length > 0)
        {
            query = query.Where(v =>
                v.VoucherNo.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                v.Party.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                v.Account.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (cbType.SelectedIndex > 0)
            query = query.Where(v => v.Type == cbType.SelectedItem?.ToString());

        if (cbMethod.SelectedIndex > 0)
            query = query.Where(v => v.Method == cbMethod.SelectedItem?.ToString());

        if (cbStatus.SelectedIndex > 0)
            query = query.Where(v => v.Status == cbStatus.SelectedItem?.ToString());

        _filteredVouchers = query.ToList();
        _PopulateGrid();
        _UpdateStats();
    }

    private void _PopulateGrid()
    {
        dgvVouchers.Rows.Clear();

        foreach (var v in _filteredVouchers)
        {
            dgvVouchers.Rows.Add(
                v.VoucherNo,
                v.Type,
                v.Date.ToString("dd/MM/yyyy"),
                v.Party,
                v.Account,
                v.Amount.ToString("N0"),
                v.Status
            );
        }
    }

    private void _UpdateStats()
    {
        // الإحصائيات تُحسب من جميع السندات غير الملغاة
        var active = _allVouchers.Where(v => v.Status != "ملغي").ToList();

        decimal receipts = active.Where(v => v.Type == "قبض").Sum(v => v.Amount);
        decimal payments = active.Where(v => v.Type == "صرف").Sum(v => v.Amount);
        decimal net = receipts - payments;

        cardReceiptsValue.Text = "+ " + receipts.ToString("N0") + " YER";
        cardPaymentsValue.Text = "- " + payments.ToString("N0") + " YER";
        cardNetValue.Text = (net >= 0 ? "" : "- ") + Math.Abs(net).ToString("N0") + " YER";

        lblRecordsCount.Text = _filteredVouchers.Count.ToString();
    }

    // ── مساعدات ───────────────────────────────────────────────────────────────

    private CashVoucherItem? _GetItemAt(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _filteredVouchers.Count) return null;
        return _filteredVouchers[rowIndex];
    }

    private CashVoucherItem? _GetSelectedItem()
    {
        if (dgvVouchers.CurrentRow == null || dgvVouchers.CurrentRow.Index < 0) return null;
        return _GetItemAt(dgvVouchers.CurrentRow.Index);
    }

    private void _ShowPreview(CashVoucherItem item)
    {
        MessageBox.Show(
            $"معاينة سند [{item.VoucherNo}] — {item.Type}\nالمبلغ: {item.Amount.ToString("N0")} YER\n(وضع تجريبي — ستُعرض شاشة المعاينة في النسخة الكاملة)",
            "معاينة السند", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void _ShowPrint(CashVoucherItem item)
    {
        MessageBox.Show(
            $"طباعة سند [{item.VoucherNo}] — {item.Type}\n(وضع تجريبي — سيتم إرسال السند إلى الطابعة في النسخة الكاملة)",
            "طباعة السند", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void _ShowDetails(CashVoucherItem item)
    {
        string details =
            $"رقم السند: {item.VoucherNo}\n" +
            $"النوع: {item.Type}\n" +
            $"التاريخ: {item.Date:dd/MM/yyyy}\n" +
            $"الجهة: {item.Party}\n" +
            $"الحساب: {item.Account}\n" +
            $"طريقة الدفع: {item.Method}\n" +
            $"المبلغ: {item.Amount.ToString("N0")} YER\n" +
            $"الحالة: {item.Status}";

        MessageBox.Show(details, "تفاصيل السند", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void _CancelVoucher(CashVoucherItem item)
    {
        if (item.Status == "ملغي")
        {
            MessageBox.Show("السند ملغي بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show(
                $"هل أنت متأكد من إلغاء السند [{item.VoucherNo}]؟",
                "تأكيد الإلغاء",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        item.Status = "ملغي";
        _ApplyFilters();
        MessageBox.Show("تم إلغاء السند بنجاح (وضع تجريبي).", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAdd_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "سيتم فتح شاشة إضافة سند جديد (قبض / صرف / تحويل).\n(وضع تجريبي — البيانات المعروضة وهمية)",
            "إضافة سند", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void cbMethod_SelectedIndexChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void dgvVouchers_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var item = _GetItemAt(e.RowIndex);
        if (item == null) return;

        switch (dgvVouchers.Columns[e.ColumnIndex].Name)
        {
            case "ColPreview":
                _ShowPreview(item);
                break;
            case "ColPrint":
                _ShowPrint(item);
                break;
            case "ColView":
                _ShowDetails(item);
                break;
            case "ColCancel":
                _CancelVoucher(item);
                break;
        }
    }

    private void dgvVouchers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var item = _GetItemAt(e.RowIndex);
        if (item == null) return;

        bool cancelled = item.Status == "ملغي";
        string colName = dgvVouchers.Columns[e.ColumnIndex].Name;

        switch (colName)
        {
            case "Type":
                e.CellStyle.ForeColor = item.Type switch
                {
                    "قبض" => Color.FromArgb(102, 214, 118),
                    "صرف" => Color.FromArgb(255, 110, 110),
                    "تحويل" => Color.FromArgb(255, 208, 104),
                    _ => Color.WhiteSmoke
                };
                e.CellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
                break;

            case "Amount":
                e.CellStyle.ForeColor = item.Type switch
                {
                    "قبض" => Color.FromArgb(102, 214, 118),
                    "صرف" => Color.FromArgb(255, 110, 110),
                    "تحويل" => Color.FromArgb(94, 168, 235),
                    _ => Color.WhiteSmoke
                };
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                e.CellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
                break;

            case "Status":
                e.CellStyle.ForeColor = item.Status switch
                {
                    "مرحل" => Color.FromArgb(102, 214, 118),
                    "مسودة" => Color.FromArgb(255, 208, 104),
                    "ملغي" => Color.FromArgb(160, 160, 160),
                    _ => Color.WhiteSmoke
                };
                e.CellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
                break;

            case "ColPreview":
            case "ColPrint":
            case "ColView":
            case "ColCancel":
                // أزرار الإجراءات: تُخفَّت للسندات الملغاة ويُعطَّل زر الإلغاء
                e.CellStyle.BackColor = cancelled ? Color.FromArgb(48, 48, 48) : Color.FromArgb(56, 56, 56);
                e.CellStyle.ForeColor = cancelled ? Color.FromArgb(130, 130, 130) : Color.White;
                if (colName == "ColCancel" && cancelled)
                    e.CellStyle.ForeColor = Color.FromArgb(95, 95, 95);
                break;

            default:
                // إظهار بقية الأعمدة باهتة للسندات الملغاة
                if (cancelled)
                    e.CellStyle.ForeColor = Color.FromArgb(130, 130, 130);
                break;
        }
    }

    private void dgvVouchers_DoubleClick(object sender, EventArgs e)
    {
        var item = _GetSelectedItem();
        if (item != null) _ShowDetails(item);
    }

    private void cmsVouchers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsVouchers.Enabled = dgvVouchers.Rows.Count > 0 && dgvVouchers.CurrentRow != null;
    }

    private void previewToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var item = _GetSelectedItem();
        if (item != null) _ShowPreview(item);
    }

    private void printToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var item = _GetSelectedItem();
        if (item != null) _ShowPrint(item);
    }

    private void viewToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var item = _GetSelectedItem();
        if (item != null) _ShowDetails(item);
    }

    private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var item = _GetSelectedItem();
        if (item != null) _CancelVoucher(item);
    }
}