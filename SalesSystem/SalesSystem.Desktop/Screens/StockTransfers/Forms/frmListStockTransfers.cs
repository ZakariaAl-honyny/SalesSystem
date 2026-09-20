using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.StockTransfers.Forms
{
    public partial class frmListStockTransfers : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtTransfers = new();
        private DataView _dvTransfers = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "رقم التحويل", "TransferID" },
                { "مصدر",        "SourceWarehouseName" },
                { "وجهة",        "DestinationWarehouseName" },
                { "الحالة",      "StatusDisplay" },
            };

        public frmListStockTransfers(IServiceProvider serviceProvider)
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
                new { Name = "TransferID",              Header = "رقم التحويل",     Fill = 8,  Min = 80  },
                new { Name = "SourceWarehouseName",     Header = "المستودع المصدر",  Fill = 18, Min = 150 },
                new { Name = "DestinationWarehouseName", Header = "المستودع الوجهة", Fill = 18, Min = 150 },
                new { Name = "TransferDate",            Header = "تاريخ التحويل",   Fill = 12, Min = 130 },
                new { Name = "StatusDisplay",           Header = "الحالة",          Fill = 8,  Min = 80  },
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

        // ── Data Loading ───────────────────────────────────────────────────────────

        private async void _RefreshTransfersList()
        {
            Cursor = dgvTransfers.Cursor = Cursors.WaitCursor;
            try
            {
                _dtTransfers = new DataTable();
                _dtTransfers.Columns.Add("TransferID", typeof(int));
                _dtTransfers.Columns.Add("SourceWarehouseName", typeof(string));
                _dtTransfers.Columns.Add("DestinationWarehouseName", typeof(string));
                _dtTransfers.Columns.Add("TransferDate", typeof(string));
                _dtTransfers.Columns.Add("StatusDisplay", typeof(string));

                var result = await _serviceProvider.GetRequiredService<IStockTransferApiService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var t in result.Value)
                    {
                        _dtTransfers.Rows.Add(
                            t.Id,
                            t.SourceWarehouseName ?? "",
                            t.DestinationWarehouseName ?? "",
                            t.CreatedAt.ToString("yyyy/MM/dd HH:mm"),
                            t.StatusDisplay ?? ""
                        );
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل التحويلات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _dvTransfers = new DataView(_dtTransfers);
                dgvTransfers.DataSource = _dvTransfers;
                lblRecordsCount.Text = _dvTransfers.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات التحويلات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvTransfers.Cursor = Cursors.Default;
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshTransfersList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedTransferId()
        {
            if (dgvTransfers.CurrentRow == null) return -1;
            return Convert.ToInt32(dgvTransfers.CurrentRow.Cells[0].Value);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListStockTransfers_Load(object sender, EventArgs e)
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
            _dvTransfers.RowFilter = "";
            lblRecordsCount.Text = _dvTransfers.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _dvTransfers.RowFilter = "";
            }
            else if (columnName == "TransferID")
            {
                _dvTransfers.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _dvTransfers.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _dvTransfers.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "رقم التحويل";

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

        private void dgvTransfers_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedTransferId();
            if (id > 0) btnAdd_Click(sender, e);
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmStockTransfer>());
            _ReloadForm();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
            => btnAdd_Click(sender, e);

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
            => btnAdd_Click(sender, e);

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        private void cmsTransfers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsTransfers.Enabled = dgvTransfers.Rows.Count > 0 && dgvTransfers.CurrentRow != null;
        }
    }
}
