using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmListInventoryCounts : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _countsTable = new();
    private DataView _countsView = new();
    private InventoryCountDto? _selectedCount;

    public frmListInventoryCounts(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min)
    {
        return new DataGridViewTextBoxColumn
        {
            Name = name,
            HeaderText = header,
            DataPropertyName = name,
            MinimumWidth = min,
            FillWeight = fill,
            ReadOnly = true,
        };
    }

    private void _SetupGridColumns()
    {
        dgvCounts.Columns.Clear();

        dgvCounts.Columns.Add(_MakeColumn("Id", "رقم", 5, 60));
        dgvCounts.Columns.Add(_MakeColumn("CountNo", "رقم الجرد", 10, 120));
        dgvCounts.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 15, 150));
        dgvCounts.Columns.Add(_MakeColumn("StatusName", "الحالة", 8, 80));
        dgvCounts.Columns.Add(_MakeColumn("Notes", "ملاحظات", 20, 200));
        dgvCounts.Columns.Add(_MakeColumn("CreatedAt", "تاريخ الإنشاء", 12, 130));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    private void _RefreshList()
    {
        Cursor = dgvCounts.Cursor = Cursors.WaitCursor;

        _countsTable = new DataTable();
        _countsTable.Columns.Add("Id", typeof(int));
        _countsTable.Columns.Add("CountNo", typeof(string));
        _countsTable.Columns.Add("WarehouseName", typeof(string));
        _countsTable.Columns.Add("StatusName", typeof(string));
        _countsTable.Columns.Add("Notes", typeof(string));
        _countsTable.Columns.Add("CreatedAt", typeof(string));

        var searchText = txtFilterValue.Text.Trim();
        var api = _serviceProvider.GetRequiredService<IInventoryCountApiService>();
        var result = api.GetAllAsync(includeInactive: chkIncludeCancelled.Checked).GetAwaiter().GetResult();

        if (result.IsSuccess && result.Value is { Count: > 0 })
        {
            foreach (var c in result.Value)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    bool matchesWh = c.WarehouseName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchesNo = c.CountNo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchesNotes = c.Notes?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (!matchesWh && !matchesNo && !matchesNotes)
                        continue;
                }

                _countsTable.Rows.Add(
                    c.Id,
                    c.CountNo,
                    c.WarehouseName ?? "",
                    c.StatusName ?? "",
                    c.Notes ?? "",
                    c.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            }
        }
        else if (!result.IsSuccess)
        {
            MessageBox.Show(result.Error ?? "حدث خطأ في تحميل قائمة الجرد", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        _countsView = new DataView(_countsTable);
        dgvCounts.DataSource = _countsView;
        lblRecordsCount.Text = _countsView.Count.ToString();

        Cursor = dgvCounts.Cursor = Cursors.Default;
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    // ── Selection ──────────────────────────────────────────────────────────────────

    private void _SelectCount()
    {
        if (dgvCounts.SelectedRows.Count == 0) return;
        var row = dgvCounts.SelectedRows[0];
        if (row.DataBoundItem is DataRowView drv)
        {
            int id = Convert.ToInt32(drv["Id"]);
            var api = _serviceProvider.GetRequiredService<IInventoryCountApiService>();
            var result = api.GetByIdAsync(id).GetAwaiter().GetResult();
            _selectedCount = result.IsSuccess ? result.Value : null;
        }
    }

    // ── Actions ────────────────────────────────────────────────────────────────────

    private void _AddCount()
    {
        var editor = _serviceProvider.GetRequiredService<frmInventoryCountEditor>();
        editor.ShowDialog();
        _ReloadForm();
    }

    private void _ViewDetails()
    {
        if (dgvCounts.SelectedRows.Count == 0) return;
        _SelectCount();
        if (_selectedCount is null) return;

        var editor = _serviceProvider.GetRequiredService<frmInventoryCountEditor>();
        editor.LoadCount(_selectedCount.Id);
        editor.ShowDialog();
        _ReloadForm();
    }

    private void _PostCount()
    {
        if (dgvCounts.SelectedRows.Count == 0) return;
        _SelectCount();
        if (_selectedCount is null || _selectedCount.Status != 1) return;

        if (MessageBox.Show("هل أنت متأكد من ترحيل الجرد؟", "تأكيد الترحيل",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        var api = _serviceProvider.GetRequiredService<IInventoryCountApiService>();
        var result = api.PostAsync(_selectedCount.Id).GetAwaiter().GetResult();

        if (result.IsSuccess)
            MessageBox.Show("تم ترحيل الجرد بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show($"خطأ في الترحيل: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        _ReloadForm();
    }

    private void _CancelCount()
    {
        if (dgvCounts.SelectedRows.Count == 0) return;
        _SelectCount();
        if (_selectedCount is null || _selectedCount.Status != 2) return;

        if (MessageBox.Show("هل أنت متأكد من إلغاء الجرد؟ لا يمكن التراجع عن هذا الإجراء.",
            "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        var api = _serviceProvider.GetRequiredService<IInventoryCountApiService>();
        var result = api.CancelAsync(_selectedCount.Id).GetAwaiter().GetResult();

        if (result.IsSuccess)
            MessageBox.Show("تم إلغاء الجرد بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show($"خطأ في الإلغاء: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        _ReloadForm();
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmListInventoryCounts_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        _AddCount();
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        _ViewDetails();
    }

    private void btnPost_Click(object sender, EventArgs e)
    {
        _PostCount();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        _CancelCount();
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
        _RefreshList();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        txtFilterValue.Clear();
        _RefreshList();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            _RefreshList();
        }
    }

    private void chkIncludeCancelled_CheckedChanged(object sender, EventArgs e)
    {
        _RefreshList();
    }

    private void dgvCounts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        _ViewDetails();
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        var hit = dgvCounts.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0) return;
        dgvCounts.ClearSelection();
        dgvCounts.Rows[hit.RowIndex].Selected = true;
        contextMenuStrip.Show(dgvCounts, e.Location);
    }

    private void dgvCounts_MouseDown(object sender, MouseEventArgs e)
    {
        ShowContextMenu(e);
    }

    private void tsmiView_Click(object sender, EventArgs e) { _ViewDetails(); }
    private void tsmiPost_Click(object sender, EventArgs e) { _PostCount(); }
    private void tsmiCancel_Click(object sender, EventArgs e) { _CancelCount(); }
}
