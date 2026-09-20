using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Warehouses.Forms
{
    public partial class frmListWarehouses : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtWarehouses = new();
        private DataView _dvWarehouses = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "الرقم",   "WarehouseID" },
                { "الاسم",   "Name" },
                { "الهاتف",  "Phone" },
                { "العنوان", "Address" },
            };

        public frmListWarehouses(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvWarehouses.Columns.Clear();

            var columns = new[]
            {
                new { Name = "WarehouseID", Header = "الرقم",    Fill = 8,  Min = 80  },
                new { Name = "Name",        Header = "الاسم",    Fill = 22, Min = 200 },
                new { Name = "Address",     Header = "العنوان",  Fill = 18, Min = 180 },
                new { Name = "Phone",       Header = "الهاتف",  Fill = 12, Min = 130 },
            };

            foreach (var col in columns)
            {
                dgvWarehouses.Columns.Add(new DataGridViewTextBoxColumn
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

        private async void _RefreshWarehousesList()
        {
            Cursor = dgvWarehouses.Cursor = Cursors.WaitCursor;

            try
            {
                _dtWarehouses = new DataTable();
                _dtWarehouses.Columns.Add("WarehouseID", typeof(int));
                _dtWarehouses.Columns.Add("Name", typeof(string));
                _dtWarehouses.Columns.Add("Address", typeof(string));
                _dtWarehouses.Columns.Add("Phone", typeof(string));

                var result = await _serviceProvider.GetRequiredService<IWarehouseApiService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var w in result.Value)
                    {
                        _dtWarehouses.Rows.Add(
                            (int)w.Id,
                            w.Name,
                            w.Address ?? "",
                            w.Phone ?? ""
                        );
                    }
                }

                _dvWarehouses = new DataView(_dtWarehouses);
                dgvWarehouses.DataSource = _dvWarehouses;
                lblRecordsCount.Text = _dvWarehouses.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات المستودعات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor = dgvWarehouses.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshWarehousesList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedWarehouseId()
        {
            if (dgvWarehouses.CurrentRow == null) return -1;
            return Convert.ToInt32(dgvWarehouses.CurrentRow.Cells[0].Value);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListWarehouses_Load(object sender, EventArgs e)
        {
            _ReloadForm();
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        // ── Filter Events ──────────────────────────────────────────────────────────

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.Text != "لا شيئ";
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
            _dvWarehouses.RowFilter = "";
            lblRecordsCount.Text = _dvWarehouses.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _dvWarehouses.RowFilter = "";
            }
            else if (columnName == "WarehouseID")
            {
                _dvWarehouses.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _dvWarehouses.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _dvWarehouses.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "الرقم";

            if (isNumericFilter && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "يُسمح فقط بإدخال الأرقام في هذا الحقل.");
                return;
            }

            if (!isNumericFilter && char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "لا يُسمح بإدخال علامات الترقيم.");
            }
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvWarehouses_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedWarehouseId();
            if (id > 0) btnEdit_Click(sender, e);
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var editor = _serviceProvider.GetRequiredService<frmAddUpdateWarehouse>();
            editor.LoadData(null);
            clsGlobal._OpenForm(editor);
            _ReloadForm();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedWarehouseId();
            if (id <= 0) return;

            try
            {
                var result = await _serviceProvider.GetRequiredService<IWarehouseApiService>().GetByIdAsync(id);
                if (result.IsSuccess && result.Value != null)
                {
                    var editor = _serviceProvider.GetRequiredService<frmAddUpdateWarehouse>();
                    editor.LoadData(result.Value);
                    clsGlobal._OpenForm(editor);
                    _ReloadForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح المستودع: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();
    }
}
