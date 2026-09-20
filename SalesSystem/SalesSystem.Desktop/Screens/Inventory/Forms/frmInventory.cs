using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    public partial class frmInventory : Form
    {
        private readonly IServiceProvider _serviceProvider;

        private DataTable _productsTable = new();
        private DataView _productsView = new();
        private DataTable _countsTable = new();
        private DataView _countsView = new();
        private DataTable _transfersTable = new();
        private DataView _transfersView = new();

        // ── Status badge colors ─────────────────────────────────────────────────────

        private static readonly Dictionary<string, Color> _countStatusColors = new()
        {
            { "مسودة", Color.SlateGray },
            { "مكتمل", Color.SteelBlue },
            { "مرحّل", Color.ForestGreen },
        };

        private static readonly Dictionary<string, Color> _transferStatusColors = new()
        {
            { "مسودة", Color.SlateGray },
            { "قيد النقل", Color.Goldenrod },
            { "مستلم", Color.ForestGreen },
        };

        // ── Mock data models (UI-first prototype — no backend) ────────────────────

        private sealed record MockProduct(
            string Code, string Barcode, string Name, string Unit,
            string Warehouse, decimal Stock, decimal ReorderLevel, decimal Cost);

        private sealed record MockCount(
            string CountNo, string Date, string Warehouse, string Committee, string Status);

        private sealed record MockTransfer(
            string TransferNo, string Date, string From, string To, string Driver, string Status);

        public frmInventory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Mock Data Builders ─────────────────────────────────────────────────────

        private void _BuildProductsTable()
        {
            _productsTable = new DataTable();
            _productsTable.Columns.Add("Code", typeof(string));
            _productsTable.Columns.Add("Barcode", typeof(string));
            _productsTable.Columns.Add("Name", typeof(string));
            _productsTable.Columns.Add("Unit", typeof(string));
            _productsTable.Columns.Add("WarehouseName", typeof(string));
            _productsTable.Columns.Add("Stock", typeof(decimal));
            _productsTable.Columns.Add("ReorderLevel", typeof(decimal));
            _productsTable.Columns.Add("Cost", typeof(decimal));
            _productsTable.Columns.Add("StockStatus", typeof(string));

            var products = new[]
            {
                new MockProduct("P-1001", "6251001234567", "سكر أبيض 1 كجم",           "كيس",     "الرئيسي", 0m,    20m,  32.50m),
                new MockProduct("P-1002", "6251001234568", "شاي أوراق سائب 250 جم",     "عبوة",    "الرئيسي", 15m,   25m,  18.75m),
                new MockProduct("P-1003", "6251001234569", "زيت عباد الشمس 1 لتر",      "زجاجة",   "المعرض",  120m,  40m,  46.00m),
                new MockProduct("P-1004", "6251001234570", "أرز بسمتي 5 كجم",           "كيس",     "الرئيسي", 8m,    30m,  145.00m),
                new MockProduct("P-1005", "6251001234571", "دقيق أبيض فاخر 2 كجم",      "كيس",     "الرئيسي", 60m,   50m,  28.90m),
                new MockProduct("P-1006", "6251001234572", "حليب مجفف 2 كجم",           "علبة",    "المعرض",  0m,    15m,  95.00m),
                new MockProduct("P-1007", "6251001234573", "قهوة سريعة الذوبان 100 جم", "برطمان",  "الرئيسي", 45m,   20m,  22.40m),
                new MockProduct("P-1008", "6251001234574", "معجون طماطم 400 جم",        "علبة",    "المعرض",  18m,   35m,  12.75m),
                new MockProduct("P-1009", "6251001234575", "صابون سائل لليدين 500 مل",  "زجاجة",   "الرئيسي", 210m,  50m,  15.20m),
                new MockProduct("P-1010", "6251001234576", "مناديل ورقية 200 منديل",    "علبة",    "المعرض",  4m,    25m,  8.60m),
            };

            foreach (var p in products)
            {
                string status = p.Stock <= 0
                    ? "نفذ"
                    : p.Stock <= p.ReorderLevel ? "محدود" : "متوفر";

                _productsTable.Rows.Add(
                    p.Code, p.Barcode, p.Name, p.Unit, p.Warehouse,
                    p.Stock, p.ReorderLevel, p.Cost, status);
            }
        }

        private void _BuildCountsTable()
        {
            _countsTable = new DataTable();
            _countsTable.Columns.Add("CountNo", typeof(string));
            _countsTable.Columns.Add("CountDate", typeof(string));
            _countsTable.Columns.Add("WarehouseName", typeof(string));
            _countsTable.Columns.Add("Committee", typeof(string));
            _countsTable.Columns.Add("Status", typeof(string));

            var counts = new[]
            {
                new MockCount("INV-2026-001", "2026/09/05", "الرئيسي", "رئيس: أحمد محمود / عضو: خالد سعيد", "مرحّل"),
                new MockCount("INV-2026-002", "2026/09/12", "المعرض",  "رئيس: سامي حسن / عضو: عمر يوسف",    "مكتمل"),
                new MockCount("INV-2026-003", "2026/09/18", "الرئيسي", "رئيس: أحمد محمود / عضو: ليلى إبراهيم", "مسودة"),
            };

            foreach (var c in counts)
            {
                _countsTable.Rows.Add(c.CountNo, c.Date, c.Warehouse, c.Committee, c.Status);
            }
        }

        private void _BuildTransfersTable()
        {
            _transfersTable = new DataTable();
            _transfersTable.Columns.Add("TransferNo", typeof(string));
            _transfersTable.Columns.Add("TransferDate", typeof(string));
            _transfersTable.Columns.Add("FromWarehouse", typeof(string));
            _transfersTable.Columns.Add("ToWarehouse", typeof(string));
            _transfersTable.Columns.Add("DriverName", typeof(string));
            _transfersTable.Columns.Add("Status", typeof(string));

            var transfers = new[]
            {
                new MockTransfer("TR-2026-001", "2026/09/08", "الرئيسي", "المعرض",  "محمد علي",  "مستلم"),
                new MockTransfer("TR-2026-002", "2026/09/11", "المعرض",  "الرئيسي", "حسن فؤاد",  "قيد النقل"),
                new MockTransfer("TR-2026-003", "2026/09/15", "الرئيسي", "المعرض",  "محمد علي",  "مستلم"),
                new MockTransfer("TR-2026-004", "2026/09/19", "المعرض",  "الرئيسي", "—",         "مسودة"),
            };

            foreach (var t in transfers)
            {
                _transfersTable.Rows.Add(t.TransferNo, t.Date, t.From, t.To, t.Driver, t.Status);
            }
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupProductsGrid()
        {
            dgvProducts.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Code",          Header = "الكود",       Fill = 7,  Min = 70, Format = (string?)null },
                new { Name = "Name",          Header = "الاسم",       Fill = 16, Min = 150, Format = (string?)null },
                new { Name = "Unit",          Header = "الوحدة",      Fill = 7,  Min = 70, Format = (string?)null },
                new { Name = "WarehouseName", Header = "المستودع",    Fill = 9,  Min = 90, Format = (string?)null },
                new { Name = "Stock",         Header = "الرصيد",      Fill = 7,  Min = 70, Format = "0.###" },
                new { Name = "ReorderLevel",  Header = "حد الطلب",    Fill = 7,  Min = 70, Format = "0.###" },
                new { Name = "Cost",          Header = "التكلفة",     Fill = 8,  Min = 80, Format = "0.00" },
                new { Name = "StockStatus",   Header = "حالة الرصيد", Fill = 9,  Min = 90, Format = (string?)null },
            };

            foreach (var col in columns)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.Name,
                    MinimumWidth = col.Min,
                    FillWeight = col.Fill,
                    ReadOnly = true,
                };

                if (col.Format != null)
                    column.DefaultCellStyle.Format = col.Format;

                dgvProducts.Columns.Add(column);
            }
        }

        private void _SetupCountsGrid()
        {
            dgvCounts.Columns.Clear();

            var columns = new[]
            {
                new { Name = "CountNo",       Header = "رقم المحضر", Fill = 10, Min = 110 },
                new { Name = "CountDate",     Header = "التاريخ",     Fill = 10, Min = 100 },
                new { Name = "WarehouseName", Header = "المستودع",    Fill = 12, Min = 100 },
                new { Name = "Committee",     Header = "لجنة الجرد",  Fill = 34, Min = 260 },
                new { Name = "Status",        Header = "الحالة",      Fill = 10, Min = 80  },
            };

            foreach (var col in columns)
            {
                dgvCounts.Columns.Add(new DataGridViewTextBoxColumn
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

        private void _SetupTransfersGrid()
        {
            dgvTransfers.Columns.Clear();

            var columns = new[]
            {
                new { Name = "TransferNo",    Header = "رقم السند",  Fill = 10, Min = 110 },
                new { Name = "TransferDate",  Header = "التاريخ",    Fill = 10, Min = 100 },
                new { Name = "FromWarehouse", Header = "من مستودع",  Fill = 12, Min = 110 },
                new { Name = "ToWarehouse",   Header = "إلى مستودع", Fill = 12, Min = 110 },
                new { Name = "DriverName",    Header = "السائق",     Fill = 12, Min = 100 },
                new { Name = "Status",        Header = "الحالة",     Fill = 10, Min = 80  },
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
        }

        // ── Data Binding ───────────────────────────────────────────────────────────

        private void _RefreshProductsGrid()
        {
            _productsView = new DataView(_productsTable);
            dgvProducts.DataSource = _productsView;
            _ApplyProductsFilter();
        }

        private void _RefreshCountsGrid()
        {
            _countsView = new DataView(_countsTable);
            dgvCounts.DataSource = _countsView;
            lblCountsCount.Text = _countsView.Count.ToString();
        }

        private void _RefreshTransfersGrid()
        {
            _transfersView = new DataView(_transfersTable);
            dgvTransfers.DataSource = _transfersView;
            lblTransfersCount.Text = _transfersView.Count.ToString();
        }

        // ── In-memory Filter (Tab 1) ───────────────────────────────────────────────

        private void _ApplyProductsFilter()
        {
            if (_productsView.Table == null) return;

            var parts = new List<string>();

            string search = txtSearch.Text.Trim();
            if (search.Length > 0)
            {
                string s = search.Replace("'", "''");
                parts.Add($"(Name LIKE '%{s}%' OR Code LIKE '%{s}%' OR Barcode LIKE '%{s}%')");
            }

            if (cbWarehouseFilter.SelectedIndex >= 1)
                parts.Add($"WarehouseName = '{cbWarehouseFilter.Text}'");

            if (cbStockStatusFilter.SelectedIndex >= 1)
                parts.Add($"StockStatus = '{cbStockStatusFilter.Text}'");

            _productsView.RowFilter = parts.Count > 0 ? string.Join(" AND ", parts) : "";
            lblProductsCount.Text = _productsView.Count.ToString();
        }

        // ── Status Badge Helpers ───────────────────────────────────────────────────

        private static void _ApplyStatusBadge(DataGridView dgv, DataGridViewCellFormattingEventArgs e, Dictionary<string, Color> colors)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (!colors.TryGetValue(e.Value.ToString() ?? "", out var color)) return;

            e.CellStyle.BackColor = color;
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmInventory_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                _BuildProductsTable();
                _BuildCountsTable();
                _BuildTransfersTable();

                _SetupProductsGrid();
                _SetupCountsGrid();
                _SetupTransfersGrid();

                _RefreshProductsGrid();
                _RefreshCountsGrid();
                _RefreshTransfersGrid();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "خطأ أثناء تحميل بيانات المخزون التجريبية");
                MessageBox.Show("حدث خطأ أثناء تحميل بيانات المخزون.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        // ── Filter Events (Tab 1) ──────────────────────────────────────────────────

        private void txtSearch_TextChanged(object sender, EventArgs e)
            => _ApplyProductsFilter();

        private void cbWarehouseFilter_SelectedIndexChanged(object sender, EventArgs e)
            => _ApplyProductsFilter();

        private void cbStockStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
            => _ApplyProductsFilter();

        // ── Grid Badge Events ──────────────────────────────────────────────────────

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            var statusColumn = dgvProducts.Columns["StockStatus"];
            if (statusColumn == null || e.ColumnIndex != statusColumn.Index) return;

            Color? color = e.Value.ToString() switch
            {
                "نفذ"   => (Color?)Color.Firebrick,
                "محدود" => (Color?)Color.Goldenrod,
                "متوفر" => (Color?)Color.ForestGreen,
                _       => null,
            };

            if (color is null) return;

            e.CellStyle.BackColor = color.Value;
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.Font = new Font(dgvProducts.Font, FontStyle.Bold);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void dgvCounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => _ApplyStatusBadge(dgvCounts, e, _countStatusColors);

        private void dgvTransfers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => _ApplyStatusBadge(dgvTransfers, e, _transferStatusColors);
    }
}