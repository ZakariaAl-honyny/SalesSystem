using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmListInventoryAdjustments : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _adjustmentsTable = new();
    private DataView _adjustmentsView = new();
    private InventoryAdjustmentDto? _selectedAdjustment;

    public frmListInventoryAdjustments(IServiceProvider serviceProvider)
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
        dgvAdjustments.Columns.Clear();

        dgvAdjustments.Columns.Add(_MakeColumn("Id", "رقم التسوية", 5, 60));
        dgvAdjustments.Columns.Add(_MakeColumn("AdjustmentNo", "رقم المستند", 8, 100));
        dgvAdjustments.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 12, 130));
        dgvAdjustments.Columns.Add(_MakeColumn("AdjustmentTypeName", "نوع التسوية", 10, 100));
        dgvAdjustments.Columns.Add(_MakeColumn("StatusName", "الحالة", 8, 80));
        dgvAdjustments.Columns.Add(_MakeColumn("Reason", "السبب", 15, 150));
        dgvAdjustments.Columns.Add(_MakeColumn("CreatedAt", "تاريخ الإنشاء", 12, 130));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    private void _RefreshList()
    {
        Cursor = dgvAdjustments.Cursor = Cursors.WaitCursor;

        _adjustmentsTable = new DataTable();
        _adjustmentsTable.Columns.Add("Id", typeof(int));
        _adjustmentsTable.Columns.Add("AdjustmentNo", typeof(string));
        _adjustmentsTable.Columns.Add("WarehouseName", typeof(string));
        _adjustmentsTable.Columns.Add("AdjustmentTypeName", typeof(string));
        _adjustmentsTable.Columns.Add("StatusName", typeof(string));
        _adjustmentsTable.Columns.Add("Reason", typeof(string));
        _adjustmentsTable.Columns.Add("CreatedAt", typeof(string));

        var searchText = txtFilterValue.Text.Trim();
        var api = _serviceProvider.GetRequiredService<IInventoryAdjustmentApiService>();
        var result = api.GetAllAsync(includeInactive: chkIncludeCancelled.Checked).GetAwaiter().GetResult();

        if (result.IsSuccess && result.Value is { Count: > 0 })
        {
            foreach (var a in result.Value)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    bool matchesWarehouse = a.WarehouseName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchesReason = a.Reason?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchesNo = a.AdjustmentNo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (!matchesWarehouse && !matchesReason && !matchesNo)
                        continue;
                }

                _adjustmentsTable.Rows.Add(
                    a.Id,
                    a.AdjustmentNo,
                    a.WarehouseName ?? "",
                    a.AdjustmentTypeName ?? "",
                    a.StatusName ?? "",
                    a.Reason ?? "",
                    a.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            }
        }
        else if (!result.IsSuccess)
        {
            MessageBox.Show(result.Error ?? "حدث خطأ في تحميل قائمة التسويات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        _adjustmentsView = new DataView(_adjustmentsTable);
        dgvAdjustments.DataSource = _adjustmentsView;
        lblRecordsCount.Text = _adjustmentsView.Count.ToString();

        Cursor = dgvAdjustments.Cursor = Cursors.Default;
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    // ── Selection ──────────────────────────────────────────────────────────────────

    private void _SelectAdjustment()
    {
        if (dgvAdjustments.SelectedRows.Count == 0) return;
        var row = dgvAdjustments.SelectedRows[0];
        if (row.DataBoundItem is DataRowView drv)
        {
            int id = Convert.ToInt32(drv["Id"]);
            var api = _serviceProvider.GetRequiredService<IInventoryAdjustmentApiService>();
            var result = api.GetByIdAsync(id).GetAwaiter().GetResult();
            _selectedAdjustment = result.IsSuccess ? result.Value : null;
        }
    }

    // ── Actions ────────────────────────────────────────────────────────────────────

    private void _AddAdjustment()
    {
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateInventoryAdjustment>();
        editor.ShowDialog();
        _ReloadForm();
    }

    private void _ViewDetails()
    {
        if (dgvAdjustments.SelectedRows.Count == 0) return;
        _SelectAdjustment();
        if (_selectedAdjustment is null) return;

        var editor = _serviceProvider.GetRequiredService<frmAddUpdateInventoryAdjustment>();
        editor.LoadAdjustment(_selectedAdjustment.Id);
        editor.ShowDialog();
        _ReloadForm();
    }

    private void _PostAdjustment()
    {
        if (dgvAdjustments.SelectedRows.Count == 0) return;
        _SelectAdjustment();
        if (_selectedAdjustment is null || _selectedAdjustment.Status != 1) return; // Draft only

        if (MessageBox.Show("هل أنت متأكد من ترحيل تسوية المخزون؟", "تأكيد الترحيل",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        var api = _serviceProvider.GetRequiredService<IInventoryAdjustmentApiService>();
        var result = api.PostAsync(_selectedAdjustment.Id).GetAwaiter().GetResult();

        if (result.IsSuccess)
            MessageBox.Show("تم ترحيل تسوية المخزون بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show($"خطأ في الترحيل: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        _ReloadForm();
    }

    private void _CancelAdjustment()
    {
        if (dgvAdjustments.SelectedRows.Count == 0) return;
        _SelectAdjustment();
        if (_selectedAdjustment is null || _selectedAdjustment.Status != 2) return; // Posted only

        if (MessageBox.Show("هل أنت متأكد من إلغاء تسوية المخزون؟ لا يمكن التراجع عن هذا الإجراء.",
            "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        var api = _serviceProvider.GetRequiredService<IInventoryAdjustmentApiService>();
        var result = api.CancelAsync(_selectedAdjustment.Id).GetAwaiter().GetResult();

        if (result.IsSuccess)
            MessageBox.Show("تم إلغاء تسوية المخزون بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show($"خطأ في الإلغاء: {result.Error}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        _ReloadForm();
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmListInventoryAdjustments_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        _AddAdjustment();
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        _ViewDetails();
    }

    private void btnPost_Click(object sender, EventArgs e)
    {
        _PostAdjustment();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        _CancelAdjustment();
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

    private void dgvAdjustments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        _ViewDetails();
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        var hit = dgvAdjustments.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0) return;
        dgvAdjustments.ClearSelection();
        dgvAdjustments.Rows[hit.RowIndex].Selected = true;
        contextMenuStrip.Show(dgvAdjustments, e.Location);
    }

    private void dgvAdjustments_MouseDown(object sender, MouseEventArgs e)
    {
        ShowContextMenu(e);
    }

    private void tsmiView_Click(object sender, EventArgs e)
    {
        _ViewDetails();
    }

    private void tsmiPost_Click(object sender, EventArgs e)
    {
        _PostAdjustment();
    }

    private void tsmiCancel_Click(object sender, EventArgs e)
    {
        _CancelAdjustment();
    }
}
