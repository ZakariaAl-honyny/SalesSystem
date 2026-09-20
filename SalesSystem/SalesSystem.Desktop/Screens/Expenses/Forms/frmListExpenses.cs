using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Expenses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

public partial class frmListExpenses : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<ExpenseDto> _allExpenses = new();
    private List<ExpenseDto> _filteredExpenses = new();

    public frmListExpenses(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    private void _SetupGridColumns()
    {
        dgvExpenses.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",              Header = "رقم المصروف",    Fill = 8,  Min = 100 },
            new { Name = "ExpenseNo",       Header = "رقم السند",       Fill = 8,  Min = 100 },
            new { Name = "ExpenseDate",     Header = "التاريخ",         Fill = 12, Min = 120 },
            new { Name = "ExpenseAccountName", Header = "حساب المصروف", Fill = 15, Min = 150 },
            new { Name = "CashBoxName",     Header = "الصندوق",         Fill = 12, Min = 120 },
            new { Name = "Amount",          Header = "المبلغ",          Fill = 10, Min = 100 },
            new { Name = "Notes",           Header = "الوصف",           Fill = 15, Min = 150 },
            new { Name = "StatusName",      Header = "الحالة",          Fill = 10, Min = 100 },
        };

        foreach (var col in columns)
        {
            dgvExpenses.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    private async void _RefreshList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                _allExpenses = result.Value.ToList();
            }
            else
            {
                _allExpenses = new List<ExpenseDto>();
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات المصروفات", "خطأ في تحميل البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _filteredExpenses = _allExpenses.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
        }
        finally
        {
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateGrid()
    {
        dgvExpenses.AutoGenerateColumns = false;
        dgvExpenses.Rows.Clear();

        foreach (var ex in _filteredExpenses)
        {
            dgvExpenses.Rows.Add(
                ex.Id,
                ex.ExpenseNo,
                ex.ExpenseDate.ToString("yyyy-MM-dd"),
                ex.ExpenseAccountName ?? "",
                ex.CashBoxName ?? "",
                ex.Amount.ToString("N2"),
                ex.Notes ?? "",
                ex.StatusName ?? ""
            );
        }
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredExpenses.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvExpenses.CurrentRow == null) return -1;
        return (int)dgvExpenses.CurrentRow.Cells[0].Value;
    }

    private byte _GetSelectedStatus()
    {
        if (dgvExpenses.CurrentRow == null) return 0;
        var item = _filteredExpenses.FirstOrDefault(x => x.Id == _GetSelectedId());
        return item?.Status ?? 0;
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private void frmListExpenses_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateExpense>());
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
            _filteredExpenses = _allExpenses.ToList();
        else
            _filteredExpenses = _allExpenses.Where(ex =>
                ex.ExpenseNo.ToString().Contains(filterText) ||
                (ex.Notes ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateExpense>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void postToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            MessageBox.Show("لا يمكن ترحيل المصروف إلا في حالة مسودة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من ترحيل هذا المصروف؟", "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = PostAsync(id);
    }

    private async Task PostAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().PostAsync(id);
        Cursor = Cursors.Default;

        if (result.IsSuccess)
        {
            MessageBox.Show("تم ترحيل المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _ReloadForm();
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في الترحيل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status == 3)
        {
            MessageBox.Show("السند ملغي بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من إلغاء هذا المصروف؟", "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = CancelAsync(id);
    }

    private async Task CancelAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().CancelAsync(id);
        Cursor = Cursors.Default;

        if (result.IsSuccess)
        {
            MessageBox.Show("تم إلغاء المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _ReloadForm();
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في الإلغاء", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            MessageBox.Show("لا يمكن حذف المصروف إلا في حالة مسودة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من حذف هذا المصروف؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        _ = DeleteAsync(id);
    }

    private async Task DeleteAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().DeleteAsync(id);
        Cursor = Cursors.Default;

        if (result.IsSuccess)
        {
            MessageBox.Show("تم حذف المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _ReloadForm();
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في الحذف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void dgvExpenses_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateExpense>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void cmsExpenses_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsExpenses.Enabled = dgvExpenses.Rows.Count > 0 && dgvExpenses.CurrentRow != null;
    }
}
