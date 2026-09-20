using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

public partial class frmListInventoryBatches : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _batchesTable = new();
    private DataView _batchesView = new();

    public frmListInventoryBatches(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────────

    private DataGridViewColumn _MakeColumn(string name, string header, int fill, int min, string? format = null)
    {
        var col = new DataGridViewTextBoxColumn
        {
            Name = name, HeaderText = header, DataPropertyName = name,
            MinimumWidth = min, FillWeight = fill, ReadOnly = true,
        };
        if (format is not null) col.DefaultCellStyle.Format = format;
        return col;
    }

    private void _SetupGridColumns()
    {
        dgvBatches.Columns.Clear();

        dgvBatches.Columns.Add(_MakeColumn("Id", "رقم", 4, 50));
        dgvBatches.Columns.Add(_MakeColumn("ProductName", "المنتج", 18, 180));
        dgvBatches.Columns.Add(_MakeColumn("BatchNo", "رقم الدفعة", 10, 120));
        dgvBatches.Columns.Add(_MakeColumn("WarehouseName", "المستودع", 10, 120));
        dgvBatches.Columns.Add(_MakeColumn("QuantityReceived", "الكمية المستلمة", 10, 100, "N3"));
        dgvBatches.Columns.Add(_MakeColumn("QuantityRemaining", "الكمية المتبقية", 10, 100, "N3"));
        dgvBatches.Columns.Add(_MakeColumn("UnitCost", "تكلفة الوحدة", 8, 90, "N2"));
        dgvBatches.Columns.Add(_MakeColumn("TotalValue", "القيمة الإجمالية", 10, 100, "N2"));
        dgvBatches.Columns.Add(_MakeColumn("ExpiryDate", "تاريخ الانتهاء", 8, 90));
        dgvBatches.Columns.Add(_MakeColumn("ExpiryStatus", "حالة الصلاحية", 8, 80));
    }

    // ── Data Loading ───────────────────────────────────────────────────────────────

    private void _RefreshList()
    {
        Cursor = dgvBatches.Cursor = Cursors.WaitCursor;

        _batchesTable = new DataTable();
        _batchesTable.Columns.Add("Id", typeof(int));
        _batchesTable.Columns.Add("ProductName", typeof(string));
        _batchesTable.Columns.Add("BatchNo", typeof(string));
        _batchesTable.Columns.Add("WarehouseName", typeof(string));
        _batchesTable.Columns.Add("QuantityReceived", typeof(decimal));
        _batchesTable.Columns.Add("QuantityRemaining", typeof(decimal));
        _batchesTable.Columns.Add("UnitCost", typeof(decimal));
        _batchesTable.Columns.Add("TotalValue", typeof(decimal));
        _batchesTable.Columns.Add("ExpiryDate", typeof(string));
        _batchesTable.Columns.Add("ExpiryStatus", typeof(string));

        // Load batches by product or show all
        var api = _serviceProvider.GetRequiredService<IInventoryBatchApiService>();
        List<InventoryBatchDto> allBatches = new();

        int? filterProductId = null;
        if (cmbProduct.SelectedValue is int pid && pid > 0)
            filterProductId = pid;

        if (filterProductId.HasValue)
        {
            var result = api.GetByProductAsync(filterProductId.Value).GetAwaiter().GetResult();
            if (result.IsSuccess && result.Value is not null)
                allBatches.AddRange(result.Value);
        }
        else
        {
            // Load batches for all products via product list
            var productApi = _serviceProvider.GetRequiredService<IProductApiService>();
            var productsResult = productApi.GetAllAsync().GetAwaiter().GetResult();
            if (productsResult.IsSuccess && productsResult.Value is { Count: > 0 })
            {
                foreach (var prod in productsResult.Value.Take(50)) // Limit to avoid timeouts
                {
                    var result = api.GetByProductAsync(prod.Id).GetAwaiter().GetResult();
                    if (result.IsSuccess && result.Value is not null)
                        allBatches.AddRange(result.Value);
                }
            }
        }

        var searchText = txtFilterValue.Text.Trim();
        foreach (var b in allBatches)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                bool matchesProd = b.ProductName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                bool matchesWh = b.WarehouseName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                bool matchesBatch = b.BatchNo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                if (!matchesProd && !matchesWh && !matchesBatch)
                    continue;
            }

            _batchesTable.Rows.Add(
                b.Id,
                b.ProductName ?? "",
                b.BatchNo,
                b.WarehouseName ?? "",
                b.QuantityReceived,
                b.QuantityRemaining,
                b.UnitCost,
                b.TotalValue,
                b.ExpiryDate?.ToString("yyyy-MM-dd") ?? "-",
                b.ExpiryStatus);
        }

        _batchesView = new DataView(_batchesTable);
        dgvBatches.DataSource = _batchesView;
        lblRecordsCount.Text = _batchesView.Count.ToString();

        Cursor = dgvBatches.Cursor = Cursors.Default;
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _LoadProducts();
        _RefreshList();
    }

    // ── Lookup Data ────────────────────────────────────────────────────────────────

    private void _LoadProducts()
    {
        var api = _serviceProvider.GetRequiredService<IProductApiService>();
        var result = api.GetAllAsync().GetAwaiter().GetResult();
        if (result.IsSuccess && result.Value is { Count: > 0 })
        {
            var items = new List<KeyValuePair<int, string>>
            {
                new(0, "--- الجميع ---")
            };
            items.AddRange(result.Value.Select(p => new KeyValuePair<int, string>(p.Id, p.Name)));

            cmbProduct.DisplayMember = "Value";
            cmbProduct.ValueMember = "Key";
            cmbProduct.DataSource = items;
        }
    }

    // ── Cell Formatting ────────────────────────────────────────────────────────────

    private void dgvBatches_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;

        if (e.ColumnIndex == dgvBatches.Columns["ExpiryStatus"].Index && e.Value is string status)
        {
            if (status == "منتهي")
                e.CellStyle.ForeColor = Color.Red;
            else
                e.CellStyle.ForeColor = Color.Green;
        }
    }

    // ── Form Events ────────────────────────────────────────────────────────────────

    private void frmListInventoryBatches_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e) { Close(); }

    private void btnSearch_Click(object sender, EventArgs e) { _RefreshList(); }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        txtFilterValue.Clear();
        if (cmbProduct.Items.Count > 0) cmbProduct.SelectedIndex = 0;
        _RefreshList();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter) { e.Handled = true; _RefreshList(); }
    }

    private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        _RefreshList();
    }
}
