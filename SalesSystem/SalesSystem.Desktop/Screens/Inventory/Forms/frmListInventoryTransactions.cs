using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Services.Interfaces;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

    public partial class frmListInventoryTransactions : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _transactionsTable = new();
    private DataView _transactionsView = new();

    public frmListInventoryTransactions(IServiceProvider serviceProvider)
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
            Name = name, HeaderText = header, DataPropertyName = name,
            MinimumWidth = min, FillWeight = fill, ReadOnly = true,
        };
    }

    private void _SetupGridColumns()
    {
        dgvTransactions.Columns.Clear();

        dgvTransactions.Columns.Add(_MakeColumn("Id", "رقم", 5, 60));
        dgvTransactions.Columns.Add(_MakeColumn("TransactionNo", "رقم الحركة", 8, 100));
        dgvTransactions.Columns.Add(_MakeColumn("MovementTypeDisplay", "نوع الحركة", 12, 120));
        dgvTransactions.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 12, 130));
        dgvTransactions.Columns.Add(_MakeColumn("CreatedAt", "التاريخ", 10, 130));
        dgvTransactions.Columns.Add(_MakeColumn("Notes", "ملاحظات", 15, 200));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    private void _RefreshList()
    {
        Cursor = dgvTransactions.Cursor = Cursors.WaitCursor;

        _transactionsTable = new DataTable();
        _transactionsTable.Columns.Add("Id", typeof(int));
        _transactionsTable.Columns.Add("TransactionNo", typeof(string));
        _transactionsTable.Columns.Add("MovementTypeDisplay", typeof(string));
        _transactionsTable.Columns.Add("WarehouseName", typeof(string));
        _transactionsTable.Columns.Add("CreatedAt", typeof(string));
        _transactionsTable.Columns.Add("Notes", typeof(string));

        var searchText = txtFilterValue.Text.Trim();
        var api = _serviceProvider.GetRequiredService<IInventoryApiService>();
        var result = api.GetMovementsAsync(pageSize: 500).GetAwaiter().GetResult();

        if (result.IsSuccess && result.Value is { Count: > 0 })
        {
            foreach (var t in result.Value)
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    bool matchesWh = t.WarehouseName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchesType = t.MovementTypeDisplay.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    bool matchesNo = t.TransactionNo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (!matchesWh && !matchesType && !matchesNo)
                        continue;
                }

                _transactionsTable.Rows.Add(
                    t.Id,
                    t.TransactionNo,
                    t.MovementTypeDisplay,
                    t.WarehouseName ?? "",
                    t.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                    t.Notes ?? "");
            }
        }
        else if (!result.IsSuccess)
        {
            MessageBox.Show(result.Error ?? "حدث خطأ في تحميل قائمة الحركات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        _transactionsView = new DataView(_transactionsTable);
        dgvTransactions.DataSource = _transactionsView;
        lblRecordsCount.Text = _transactionsView.Count.ToString();

        Cursor = dgvTransactions.Cursor = Cursors.Default;
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    private void _ViewDetails()
    {
        if (dgvTransactions.SelectedRows.Count == 0) return;
        var row = dgvTransactions.SelectedRows[0];
        if (row.DataBoundItem is not DataRowView drv) return;

        int id = Convert.ToInt32(drv["Id"]);
        var api = _serviceProvider.GetRequiredService<IInventoryApiService>();
        var result = api.GetInventoryTransactionByIdAsync(id).GetAwaiter().GetResult();

        if (!result.IsSuccess || result.Value is null)
        {
            MessageBox.Show("تعذر تحميل تفاصيل الحركة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var tx = result.Value;
        var details = new System.Text.StringBuilder();
        details.AppendLine($"رقم الحركة: {tx.TransactionNo}");
        details.AppendLine($"النوع: {tx.MovementTypeDisplay}");
        details.AppendLine($"المستودع: {tx.WarehouseName ?? "-"}");
        details.AppendLine($"التاريخ: {tx.CreatedAt:yyyy-MM-dd HH:mm}");
        details.AppendLine($"ملاحظات: {tx.Notes ?? "-"}");
        details.AppendLine($"--- البنود ({tx.Lines?.Count ?? 0}) ---");

        if (tx.Lines is { Count: > 0 })
        {
            foreach (var line in tx.Lines)
            {
                details.AppendLine($"- {line.ProductUnitName ?? "?"} | كمية: {line.Quantity:N3} | تكلفة: {line.UnitCost:N2}");
            }
        }

        MessageBox.Show(details.ToString(), "تفاصيل الحركة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmListInventoryTransactions_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e) { Close(); }

    private void btnSearch_Click(object sender, EventArgs e) { _RefreshList(); }

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

    private void dgvTransactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        _ViewDetails();
    }

    private void ShowContextMenu(MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        var hit = dgvTransactions.HitTest(e.X, e.Y);
        if (hit.RowIndex < 0) return;
        dgvTransactions.ClearSelection();
        dgvTransactions.Rows[hit.RowIndex].Selected = true;
        contextMenuStrip.Show(dgvTransactions, e.Location);
    }

    private void dgvTransactions_MouseDown(object sender, MouseEventArgs e)
    {
        ShowContextMenu(e);
    }

    private void tsmiView_Click(object sender, EventArgs e) { _ViewDetails(); }
}
