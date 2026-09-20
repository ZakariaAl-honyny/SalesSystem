using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Banks;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Banks.Forms;

public partial class frmListBanks : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<BankDto> _allBanks = new();
    private List<BankDto> _filteredBanks = new();

    public frmListBanks(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    private void _SetupGridColumns()
    {
        dgvBanks.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",            Header = "رقم البنك",    Fill = 8,  Min = 100 },
            new { Name = "Name",          Header = "اسم البنك",    Fill = 15, Min = 150 },
            new { Name = "AccountName",   Header = "الحساب",        Fill = 15, Min = 150 },
            new { Name = "AccountNumber", Header = "رقم الحساب",    Fill = 12, Min = 130 },
            new { Name = "Iban",          Header = "الآيبان",       Fill = 15, Min = 150 },
            new { Name = "IsActive",      Header = "نشط",           Fill = 8,  Min = 80  },
        };

        foreach (var col in columns)
        {
            dgvBanks.Columns.Add(new DataGridViewTextBoxColumn
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
            var result = await _serviceProvider.GetRequiredService<IBankApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                _allBanks = result.Value.ToList();
            }
            else
            {
                _allBanks = new List<BankDto>();
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات البنوك", "خطأ في تحميل البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _filteredBanks = _allBanks.ToList();
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
        dgvBanks.AutoGenerateColumns = false;
        dgvBanks.Rows.Clear();

        foreach (var b in _filteredBanks)
        {
            dgvBanks.Rows.Add(
                b.Id,
                b.Name,
                b.AccountName ?? "",
                b.AccountNumber ?? "",
                b.Iban ?? "",
                b.IsActive ? "نعم" : "لا"
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
        lblRecordsCount.Text = _filteredBanks.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvBanks.CurrentRow == null) return -1;
        return (int)dgvBanks.CurrentRow.Cells[0].Value;
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private void frmListBanks_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateBank>());
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
            _filteredBanks = _allBanks.ToList();
        else
            _filteredBanks = _allBanks.Where(b =>
                b.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                (b.AccountName ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateBank>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void deactivateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id <= 0) return;

        var bank = _allBanks.FirstOrDefault(b => b.Id == id);
        if (bank == null) return;

        if (MessageBox.Show(
                $"هل أنت متأكد من إلغاء تنشيط البنك [{bank.Name}]؟",
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
        var result = await _serviceProvider.GetRequiredService<IBankApiService>().DeactivateAsync(id);
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

    private void dgvBanks_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateBank>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void cmsBanks_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsBanks.Enabled = dgvBanks.Rows.Count > 0 && dgvBanks.CurrentRow != null;
    }
}
