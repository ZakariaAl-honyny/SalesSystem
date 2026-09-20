using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.CashBoxes;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

public partial class frmListCashBoxes : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<CashBoxDto> _allCashBoxes = new();
    private List<CashBoxDto> _filteredCashBoxes = new();

    public frmListCashBoxes(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    private void _SetupGridColumns()
    {
        dgvCashBoxes.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",          Header = "رقم الصندوق",  Fill = 8,  Min = 100 },
            new { Name = "Name",        Header = "اسم الصندوق",  Fill = 15, Min = 150 },
            new { Name = "AccountName", Header = "الحساب",        Fill = 15, Min = 150 },
            new { Name = "AccountCode", Header = "كود الحساب",    Fill = 10, Min = 100 },
            new { Name = "Description", Header = "الوصف",         Fill = 15, Min = 150 },
            new { Name = "IsActive",    Header = "نشط",           Fill = 8,  Min = 80  },
        };

        foreach (var col in columns)
        {
            dgvCashBoxes.Columns.Add(new DataGridViewTextBoxColumn
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
            var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                _allCashBoxes = result.Value.ToList();
            }
            else
            {
                _allCashBoxes = new List<CashBoxDto>();
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات الصناديق.", "خطأ في تحميل البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _filteredCashBoxes = _allCashBoxes.ToList();
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
        dgvCashBoxes.AutoGenerateColumns = false;
        dgvCashBoxes.Rows.Clear();

        foreach (var c in _filteredCashBoxes)
        {
            dgvCashBoxes.Rows.Add(
                c.Id,
                c.Name,
                c.AccountName ?? "",
                c.AccountCode ?? "",
                c.Description ?? "",
                c.IsActive ? "نعم" : "لا"
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
        lblRecordsCount.Text = _filteredCashBoxes.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvCashBoxes.CurrentRow == null) return -1;
        return (int)dgvCashBoxes.CurrentRow.Cells[0].Value;
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private void frmListCashBoxes_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateCashBox>());
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
            _filteredCashBoxes = _allCashBoxes.ToList();
        else
            _filteredCashBoxes = _allCashBoxes.Where(c =>
                c.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                (c.AccountName ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateCashBox>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void deactivateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id <= 0) return;

        var box = _allCashBoxes.FirstOrDefault(c => c.Id == id);
        if (box == null) return;

        if (MessageBox.Show(
                $"هل أنت متأكد من إلغاء تنشيط الصندوق [{box.Name}]؟",
                "تأكيد إلغاء التنشيط",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        _ = DeactivateAsync(id);
    }

    private async Task DeactivateAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().DeactivateAsync(id);
        Cursor = Cursors.Default;

        if (result.IsSuccess)
        {
            MessageBox.Show("تم إلغاء التنشيط بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _ReloadForm();
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في إلغاء التنشيط.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void dgvCashBoxes_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateCashBox>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void cmsCashBoxes_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsCashBoxes.Enabled = dgvCashBoxes.Rows.Count > 0 && dgvCashBoxes.CurrentRow != null;
    }

    private void btnTransactions_Click(object sender, EventArgs e)
    {
        clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCashBoxTransactions>());
    }

    private void btnDailyClosure_Click(object sender, EventArgs e)
    {
        clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmDailyClosure>());
    }

    private void showTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id <= 0) return;

        clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCashBoxTransactions>());
        _ReloadForm();
    }

    private void dailyClosureToolStripMenuItem_Click(object sender, EventArgs e)
    {
        clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmDailyClosure>());
        _ReloadForm();
    }
}
