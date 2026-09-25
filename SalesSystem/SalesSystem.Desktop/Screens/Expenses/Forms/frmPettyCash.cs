using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

public partial class frmPettyCash : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<PettyCashRequestDto> _allRequests = new();
    private List<PettyCashRequestDto> _filteredRequests = new();

    private IPettyCashApiService ApiService
        => _serviceProvider.GetRequiredService<IPettyCashApiService>();

    public frmPettyCash(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ──────────────── Grid Setup ────────────────

    private void _SetupGridColumns()
    {
        dgvPettyCash.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",            Header = "الرقم",        Fill = 5,  Min = 60  },
            new { Name = "RequestNo",     Header = "رقم الطلب",    Fill = 7,  Min = 80  },
            new { Name = "Date",          Header = "التاريخ",      Fill = 9,  Min = 100 },
            new { Name = "EmployeeName",  Header = "الموظف",       Fill = 14, Min = 140 },
            new { Name = "Amount",        Header = "المبلغ",       Fill = 9,  Min = 100 },
            new { Name = "Reason",        Header = "السبب",        Fill = 16, Min = 160 },
            new { Name = "StatusDisplay", Header = "الحالة",       Fill = 9,  Min = 100 },
            new { Name = "ApprovedBy",    Header = "اعتمد بواسطة", Fill = 12, Min = 120 },
            new { Name = "Notes",         Header = "ملاحظات",      Fill = 14, Min = 140 },
        };

        foreach (var col in columns)
        {
            dgvPettyCash.Columns.Add(new DataGridViewTextBoxColumn
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

    // ──────────────── Data Loading ────────────────

    private async void _RefreshList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var result = await ApiService.GetAllAsync(includeInactive: true);
            if (result.IsSuccess && result.Value != null)
            {
                _allRequests = result.Value.ToList();
            }
            else
            {
                _allRequests = new List<PettyCashRequestDto>();
                MessageBox.Show(
                    result.Error ?? "فشل في تحميل بيانات العهد النثرية",
                    "خطأ في تحميل البيانات",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _filteredRequests = _allRequests.ToList();
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
        dgvPettyCash.AutoGenerateColumns = false;
        dgvPettyCash.Rows.Clear();

        foreach (var r in _filteredRequests)
        {
            int rowIndex = dgvPettyCash.Rows.Add(
                r.Id,
                r.RequestNo,
                r.Date.ToString("yyyy-MM-dd"),
                r.EmployeeName,
                r.Amount.ToString("N2"),
                r.Reason,
                r.StatusDisplay,
                r.ApprovedBy ?? "",
                r.Notes ?? ""
            );

            // تلوين الحالة
            var statusCell = dgvPettyCash.Rows[rowIndex].Cells["StatusDisplay"];
            switch (r.Status)
            {
                case 1: // Pending
                    statusCell.Style.ForeColor = Color.DarkOrange;
                    statusCell.Style.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    break;
                case 2: // Approved
                    statusCell.Style.ForeColor = Color.RoyalBlue;
                    statusCell.Style.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    break;
                case 3: // Disbursed
                    statusCell.Style.ForeColor = Color.Teal;
                    statusCell.Style.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    break;
                case 4: // Settled
                    statusCell.Style.ForeColor = Color.Green;
                    statusCell.Style.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    break;
                case 5: // Rejected
                    statusCell.Style.ForeColor = Color.Red;
                    statusCell.Style.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                    break;
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
        lblRecordsCount.Text = _filteredRequests.Count.ToString();
    }

    // ──────────────── Helpers ────────────────

    private int _GetSelectedId()
    {
        if (dgvPettyCash.CurrentRow == null) return -1;
        return (int)dgvPettyCash.CurrentRow.Cells["Id"].Value;
    }

    private byte _GetSelectedStatus()
    {
        var id = _GetSelectedId();
        if (id <= 0) return 0;
        var item = _filteredRequests.FirstOrDefault(x => x.Id == id);
        return item?.Status ?? 0;
    }

    // ──────────────── Events ────────────────

    private void frmPettyCash_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        var frm = _serviceProvider.GetRequiredService<frmAddUpdatePettyCash>();
        frm.ShowDialog();
        _ReloadForm();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
        {
            _filteredRequests = _allRequests.ToList();
        }
        else
        {
            _filteredRequests = _allRequests.Where(r =>
                r.RequestNo.ToString().Contains(filterText) ||
                r.EmployeeName.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                r.Reason.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                (r.ApprovedBy ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                (r.Notes ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            MessageBox.Show("لا يمكن تعديل طلب العهدة إلا في حالة قيد الانتظار.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var frm = _serviceProvider.GetRequiredService<frmAddUpdatePettyCash>();
        frm._SetUpdateMode(id);
        frm.ShowDialog();
        _ReloadForm();
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            MessageBox.Show("لا يمكن حذف طلب العهدة إلا في حالة قيد الانتظار.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من حذف هذا الطلب؟", "تأكيد الحذف",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        _ = DeleteAsync(id);
    }

    private async Task DeleteAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await ApiService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                MessageBox.Show("تم حذف طلب العهدة بنجاح.", "تم",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الحذف", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void dgvPettyCash_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdatePettyCash>();
            frm._SetUpdateMode(id);
            frm.ShowDialog();
            _ReloadForm();
        }
    }

    private void cmsPettyCash_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsPettyCash.Enabled = dgvPettyCash.Rows.Count > 0 && dgvPettyCash.CurrentRow != null;
    }
}
