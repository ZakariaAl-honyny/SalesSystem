using System.Data;
using System.Globalization;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

/// <summary>
/// نافذة "قيد يومي جديد" — نموذج واجهة (UI Shape) يعمل ببيانات وهمية في الذاكرة
/// دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// شبكة بنود قابلة للتحرير مع حساب فوري لمجاميع المدين والدائن والفرق،
/// وحفظ كمسودة أو ترحيل مع التحقق من التوازن.
/// </summary>
public partial class frmJournalEntryNew : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── بيانات وهمية في الذاكرة (Mock) ────────────────────────────────────────

    private sealed class AccountOption
    {
        public required string Code { get; init; }
        public required string Name { get; init; }
        public string Display => $"{Code} - {Name}";
        public override string ToString() => Display;
    }

    private static readonly AccountOption[] Accounts =
    {
        new() { Code = "1101", Name = "الصندوق الرئيسي" },
        new() { Code = "1102", Name = "البنك" },
        new() { Code = "1103", Name = "الصندوق الفرعي" },
        new() { Code = "1201", Name = "العملاء" },
        new() { Code = "1301", Name = "المخزون" },
        new() { Code = "2101", Name = "الموردون" },
        new() { Code = "2201", Name = "الرواتب المستحقة" },
        new() { Code = "2301", Name = "الضرائب المستحقة" },
        new() { Code = "3101", Name = "رأس المال" },
        new() { Code = "4101", Name = "المبيعات" },
        new() { Code = "5101", Name = "المصروفات" },
        new() { Code = "6101", Name = "إيرادات أخرى" },
    };

    private static readonly string[] CostCenters = { "", "CC-01", "CC-02", "CC-03", "CC-04" };

    // جدول البنود في الذاكرة (يرتبط بالشبكة مباشرة)
    private DataTable _dtLines = new();

    public frmJournalEntryNew(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnPost.PerformClick()));
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        dtpDate.Value = DateTime.Today;
        _SetupLinesGrid();
        _RecalcTotals();
    }

    // ── إعداد شبكة البنود ─────────────────────────────────────────────────────

    private void _SetupLinesGrid()
    {
        _dtLines = new DataTable();
        _dtLines.Columns.Add("AccountCode", typeof(string));
        _dtLines.Columns.Add("Debit", typeof(decimal));
        _dtLines.Columns.Add("Credit", typeof(decimal));
        _dtLines.Columns.Add("CostCenter", typeof(string));
        _dtLines.Columns.Add("Notes", typeof(string));

        // عمود الحساب — قائمة منسدلة من الحسابات الوهمية
        colAccount.DataSource = Accounts;
        colAccount.DisplayMember = "Display";
        colAccount.ValueMember = "Code";

        // عمود مركز التكلفة — يسمح بتركه فارغاً
        colCostCenter.DataSource = CostCenters;

        dgvLines.AutoGenerateColumns = false;
        dgvLines.DataSource = new DataView(_dtLines);
    }

    // ── حساب المجاميع والتوازن ────────────────────────────────────────────────

    private void _RecalcTotals()
    {
        decimal totalDebit = 0m, totalCredit = 0m;

        foreach (DataRow row in _dtLines.Rows)
        {
            if (row.RowState == DataRowState.Deleted) continue;
            totalDebit += _RowDecimal(row, "Debit");
            totalCredit += _RowDecimal(row, "Credit");
        }

        decimal diff = totalDebit - totalCredit;
        bool balanced = Math.Abs(diff) < 0.01m;

        lblTotalDebit.Text = totalDebit.ToString("N2", CultureInfo.CurrentCulture);
        lblTotalCredit.Text = totalCredit.ToString("N2", CultureInfo.CurrentCulture);
        lblDiff.Text = diff.ToString("N2", CultureInfo.CurrentCulture);

        if (balanced)
        {
            lblBalanceStatus.Text = "متوازن ✓";
            lblBalanceStatus.ForeColor = Color.FromArgb(25, 135, 84);
        }
        else
        {
            lblBalanceStatus.Text = $"غير متوازن ✗ — الفرق: {diff:N2}";
            lblBalanceStatus.ForeColor = Color.Firebrick;
        }
    }

    private static decimal _RowDecimal(DataRow row, string column)
        => row[column] is DBNull ? 0m : Convert.ToDecimal(row[column], CultureInfo.CurrentCulture);

    // ── أحداث الشبكة ──────────────────────────────────────────────────────────

    private void dgvLines_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        _RecalcTotals();
    }

    private void dgvLines_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        // التزام القوائم المنسدلة فوراً حتى يُحدَّث التوازن لحظياً
        if (!dgvLines.IsCurrentCellDirty) return;

        int col = dgvLines.CurrentCell?.ColumnIndex ?? -1;
        if (col == colAccount.Index || col == colCostCenter.Index)
            dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void dgvLines_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        // يُسمح بترك الخلايا فارغة (الأصفار تُعامل كـ 0)
        e.ThrowException = false;
    }

    private void dgvLines_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (e.ColumnIndex != colDebit.Index && e.ColumnIndex != colCredit.Index) return;

        string text = (e.FormattedValue as string)?.Trim() ?? "";

        // الخلية الفارغة مقبولة وتُعامل كصفر
        if (text.Length == 0) return;

        if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal value) || value < 0)
        {
            e.Cancel = true;
            dgvLines.Rows[e.RowIndex].ErrorText = "أدخل مبلغاً صحيحاً أكبر من أو يساوي صفر";
        }
        else
        {
            dgvLines.Rows[e.RowIndex].ErrorText = "";
        }
    }

    private void tsmiDeleteRow_Click(object? sender, EventArgs e)
    {
        if (dgvLines.CurrentRow == null || dgvLines.CurrentRow.IsNewRow) return;

        if (dgvLines.CurrentRow.DataBoundItem is DataRowView rowView)
            rowView.Row.Delete();

        _RecalcTotals();
    }

    // ── التحقق من صحة البيانات ────────────────────────────────────────────────

    /// <summary>عدد البنود الصالحة: حساب محدد + مبلغ (مدين أو دائن) أكبر من صفر.</summary>
    private int _ValidLineCount()
    {
        int count = 0;

        foreach (DataRow row in _dtLines.Rows)
        {
            if (row.RowState == DataRowState.Deleted) continue;

            string code = row["AccountCode"] as string ?? "";
            decimal debit = _RowDecimal(row, "Debit");
            decimal credit = _RowDecimal(row, "Credit");

            if (code.Length > 0 && (debit > 0 || credit > 0))
                count++;
        }

        return count;
    }

    private (decimal Debit, decimal Credit) _Totals()
    {
        decimal debit = 0m, credit = 0m;

        foreach (DataRow row in _dtLines.Rows)
        {
            if (row.RowState == DataRowState.Deleted) continue;
            debit += _RowDecimal(row, "Debit");
            credit += _RowDecimal(row, "Credit");
        }

        return (debit, credit);
    }

    private bool _ValidateLines(bool requireBalance)
    {
        if (_ValidLineCount() < 2)
        {
            MessageBox.Show("يجب إدخال سطرين على الأقل بحساب ومبلغ صحيحين.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (requireBalance)
        {
            var (debit, credit) = _Totals();
            if (Math.Abs(debit - credit) >= 0.01m)
            {
                MessageBox.Show(
                    $"القيد غير متوازن: مجموع المدين ({debit:N2}) لا يساوي مجموع الدائن ({credit:N2}).\n" +
                    "يرجى تصحيح المبالغ قبل الترحيل.",
                    "قيد غير متوازن", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        return true;
    }

    // ── الأزرار ───────────────────────────────────────────────────────────────

    private void btnSaveDraft_Click(object? sender, EventArgs e)
    {
        if (!_ValidateLines(requireBalance: false)) return;

        MessageBox.Show(
            "تم حفظ القيد كمسودة بنجاح (وضع تجريبي).\n" +
            $"رقم القيد: جديد | التاريخ: {dtpDate.Value:yyyy/MM/dd}",
            "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnPost_Click(object? sender, EventArgs e)
    {
        if (!_ValidateLines(requireBalance: true)) return;

        var (debit, credit) = _Totals();

        MessageBox.Show(
            $"تم ترحيل القيد اليومي بنجاح (وضع تجريبي).\n" +
            $"التاريخ: {dtpDate.Value:yyyy/MM/dd} | المبلغ: {debit:N2}",
            "تم الترحيل", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
        => Close();
}