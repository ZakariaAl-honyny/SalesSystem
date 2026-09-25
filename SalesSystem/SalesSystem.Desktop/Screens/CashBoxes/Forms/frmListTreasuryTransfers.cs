using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

public partial class frmListTreasuryTransfers : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<TreasuryTransferDto> _allTransfers = new();
    private List<TreasuryTransferDto> _filteredTransfers = new();

    private ITreasuryTransferApiService ApiService
        => _serviceProvider.GetRequiredService<ITreasuryTransferApiService>();

    public frmListTreasuryTransfers(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvTransfers.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",              Header = "المعرف",       Fill = 5,  Min = 60  },
            new { Name = "TransferNo",      Header = "رقم التحويل",  Fill = 8,  Min = 90  },
            new { Name = "TransferDate",    Header = "التاريخ",      Fill = 9,  Min = 100 },
            new { Name = "FromName",        Header = "من",           Fill = 14, Min = 150 },
            new { Name = "ToName",          Header = "إلى",          Fill = 14, Min = 150 },
            new { Name = "Amount",          Header = "المبلغ",       Fill = 9,  Min = 100 },
            new { Name = "Notes",           Header = "ملاحظات",      Fill = 15, Min = 140 },
            new { Name = "StatusDisplay",   Header = "الحالة",       Fill = 7,  Min = 80  },
        };

        foreach (var col in columns)
        {
            dgvTransfers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }

        // Hide Id column (used for lookup)
        if (dgvTransfers.Columns.Contains("Id"))
            dgvTransfers.Columns["Id"].Visible = false;

        // Format money column
        if (dgvTransfers.Columns.Contains("Amount"))
            dgvTransfers.Columns["Amount"].DefaultCellStyle.Format = "N2";
    }

    // ── Data Loading ───────────────────────────────────────────────────────────

    private async void _RefreshList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;
        try
        {
            var result = await ApiService.GetAllAsync(includeInactive: true);
            if (result.IsSuccess && result.Value != null)
            {
                _allTransfers = result.Value.ToList();
            }
            else
            {
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل تحويلات الخزينة",
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _allTransfers = new List<TreasuryTransferDto>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء تحميل تحويلات الخزينة: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _filteredTransfers = _allTransfers.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateGrid()
    {
        dgvTransfers.AutoGenerateColumns = false;
        dgvTransfers.Rows.Clear();

        foreach (var t in _filteredTransfers)
        {
            dgvTransfers.Rows.Add(
                t.Id,
                t.TransferNo,
                t.TransferDate.ToString("yyyy-MM-dd"),
                t.FromCashBoxName ?? "",
                t.ToCashBoxName ?? "",
                t.Amount.ToString("N2"),
                t.Notes ?? "",
                t.StatusDisplay
            );
        }

        // Color-code status cells
        foreach (DataGridViewRow row in dgvTransfers.Rows)
        {
            var statusCell = row.Cells["StatusDisplay"];
            if (statusCell?.Value is string status)
            {
                statusCell.Style.ForeColor = status switch
                {
                    "مسودة" => Color.DarkGoldenrod,
                    "مرحل"  => Color.DarkGreen,
                    "ملغي"  => Color.Gray,
                    _       => Color.Black
                };
            }
        }
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredTransfers.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvTransfers.CurrentRow == null) return -1;
        var val = dgvTransfers.CurrentRow.Cells["Id"]?.Value;
        return val != null ? Convert.ToInt32(val) : -1;
    }

    private byte _GetSelectedStatus()
    {
        int id = _GetSelectedId();
        if (id <= 0) return 0;
        var item = _allTransfers.FirstOrDefault(t => t.Id == id);
        return item?.Status ?? 0;
    }

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmListTreasuryTransfers_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        // Opens the existing single-transfer creation form (frmCashTransfer)
        using var frm = _serviceProvider.GetRequiredService<frmCashTransfer>();
        if (frm.ShowDialog(this) == DialogResult.OK)
            _ReloadForm();
    }

    // ── Search ─────────────────────────────────────────────────────────────────

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
        {
            _filteredTransfers = _allTransfers.ToList();
        }
        else
        {
            _filteredTransfers = _allTransfers.Where(t =>
                t.TransferNo.ToString().Contains(filterText) ||
                (t.FromCashBoxName ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                (t.ToCashBoxName ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                (t.Notes ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    // ── Context Menu ───────────────────────────────────────────────────────────

    private void dgvTransfers_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            // Future: open detail/edit form
            MessageBox.Show($"تفاصيل التحويل رقم {id}", "معلومات",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void viewDetailsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var transfer = _allTransfers.FirstOrDefault(t => t.Id == id);
            if (transfer != null)
            {
                MessageBox.Show(
                    $"رقم التحويل: {transfer.TransferNo}\n" +
                    $"التاريخ: {transfer.TransferDate:yyyy-MM-dd}\n" +
                    $"من: {transfer.FromCashBoxName}\n" +
                    $"إلى: {transfer.ToCashBoxName}\n" +
                    $"المبلغ: {transfer.Amount:N2}\n" +
                    $"الحالة: {transfer.StatusDisplay}\n" +
                    $"ملاحظات: {transfer.Notes ?? "—"}",
                    "تفاصيل التحويل", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void cmsTransfers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRows = dgvTransfers.Rows.Count > 0 && dgvTransfers.CurrentRow != null;
        viewDetailsToolStripMenuItem.Enabled = hasRows;
        refreshToolStripMenuItem.Enabled = true;
    }
}
