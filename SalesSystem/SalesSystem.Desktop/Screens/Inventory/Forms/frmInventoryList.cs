using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    public partial class frmInventoryList : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _movementsTable = new();
        private DataView _movementsView = new();

        public frmInventoryList(IServiceProvider serviceProvider)
        {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvMovements.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Date",           Header = "\u0627\u0644\u062A\u0627\u0631\u064A\u062E",          Fill = 12, Min = 150 },
                new { Name = "WarehouseName",  Header = "\u0627\u0644\u0645\u0633\u062A\u0648\u062F\u0639",     Fill = 12, Min = 130 },
                new { Name = "MovementType",   Header = "\u0646\u0648\u0639 \u0627\u0644\u062D\u0631\u0643\u0629", Fill = 12, Min = 120 },
                new { Name = "QuantityChange", Header = "\u0627\u0644\u0643\u0645\u064A\u0629",         Fill = 10, Min = 100 },
                new { Name = "QuantityAfter",  Header = "\u0627\u0644\u0631\u0635\u064A\u062F",       Fill = 10, Min = 100 },
                new { Name = "ReferenceNo",    Header = "\u0631\u0642\u0645 \u0627\u0644\u0645\u0631\u062C\u0639",   Fill = 10, Min = 120 },
            };

            foreach (var col in columns)
            {
                dgvMovements.Columns.Add(new DataGridViewTextBoxColumn
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

        // ── Data Loading ───────────────────────────────────────────────────────────

        private void _RefreshMovementsList()
        {
            Cursor = dgvMovements.Cursor = Cursors.WaitCursor;

            _movementsTable = new DataTable();
            _movementsTable.Columns.Add("Date", typeof(string));
            _movementsTable.Columns.Add("WarehouseName", typeof(string));
            _movementsTable.Columns.Add("MovementType", typeof(string));
            _movementsTable.Columns.Add("QuantityChange", typeof(decimal));
            _movementsTable.Columns.Add("QuantityAfter", typeof(decimal));
            _movementsTable.Columns.Add("ReferenceNo", typeof(string));

            var searchText = txtFilterValue.Text.Trim();
            var result = _serviceProvider.GetRequiredService<IReportApiService>().GetMovementsAsync().GetAwaiter().GetResult();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var m in result.Value)
                {
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        bool matchesWarehouse = m.WarehouseName?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        bool matchesRef = m.ReferenceNo?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;

                        if (!matchesWarehouse && !matchesRef)
                            continue;
                    }

                    _movementsTable.Rows.Add(
                        m.Date.ToString("yyyy-MM-dd HH:mm"),
                        m.WarehouseName ?? "",
                        m.MovementType ?? "",
                        m.QuantityChange,
                        m.QuantityAfter,
                        m.ReferenceNo ?? "");
                }
            }
            else if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error ?? "حدث خطأ في تحميل بيانات الحركات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _movementsView = new DataView(_movementsTable);
            dgvMovements.DataSource = _movementsView;
            lblRecordsCount.Text = _movementsView.Count.ToString();

            Cursor = dgvMovements.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshMovementsList();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmInventoryList_Load(object sender, EventArgs e)
        {
            _ReloadForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _RefreshMovementsList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            _RefreshMovementsList();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                _RefreshMovementsList();
            }
        }

        private void dgvMovements_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvMovements.Columns["QuantityChange"].Index && e.Value != null)
            {
                decimal qty = (decimal)e.Value;
                if (qty < 0)
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
                else if (qty > 0)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
            }
        }
    }
}
