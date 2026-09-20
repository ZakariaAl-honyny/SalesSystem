using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.CashBoxes;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

public partial class frmCashBoxTransactions : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<CashBoxDto> _cashBoxes = new();

    public frmCashBoxTransactions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _SetupGridColumns();
        _ = LoadCashBoxesAsync();
    }

    private void _SetupGridColumns()
    {
        dgvTransactions.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",             Header = "رقم الحركة",    Fill = 8,  Min = 80  },
            new { Name = "Type",           Header = "النوع",          Fill = 10, Min = 100 },
            new { Name = "Amount",         Header = "المبلغ",         Fill = 10, Min = 100 },
            new { Name = "RunningBalance", Header = "الرصيد الجاري",  Fill = 10, Min = 110 },
            new { Name = "Description",    Header = "البيان",         Fill = 20, Min = 200 },
            new { Name = "CreatedAt",      Header = "التاريخ",        Fill = 12, Min = 130 },
        };

        foreach (var col in columns)
        {
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
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

    private async Task LoadCashBoxesAsync()
    {
        Cursor = Cursors.WaitCursor;

        var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().GetAllAsync();

        Cursor = Cursors.Default;

        if (result.IsSuccess && result.Value != null)
        {
            _cashBoxes = result.Value.Where(b => b.IsActive).ToList();

            cmbCashBox.DataSource = _cashBoxes.Select(b => new { b.Id, b.Name }).ToList();
            cmbCashBox.DisplayMember = "Name";
            cmbCashBox.ValueMember = "Id";
            cmbCashBox.SelectedIndex = -1;
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في تحميل قائمة الصناديق.", "خطأ في تحميل البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void _LoadTransactionsForCashBox(int cashBoxId)
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        dgvTransactions.Rows.Clear();

        Cursor = Cursors.Default;
        btnRefresh.Enabled = true;

        lblRecordsCount.Text = dgvTransactions.Rows.Count.ToString();
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();

        if (cmbCashBox.SelectedValue != null && cmbCashBox.SelectedValue is int boxId)
        {
            _LoadTransactionsForCashBox(boxId);
        }
    }

    private void frmCashBoxTransactions_Load(object sender, EventArgs e)
    {
        _SetupGridColumns();
    }

    private void cmbCashBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbCashBox.SelectedValue != null && cmbCashBox.SelectedValue is int boxId && boxId > 0)
        {
            _LoadTransactionsForCashBox(boxId);
        }
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void cmsTransactions_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsTransactions.Enabled = dgvTransactions.Rows.Count > 0 && dgvTransactions.CurrentRow != null;
    }

    private void dgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
}
