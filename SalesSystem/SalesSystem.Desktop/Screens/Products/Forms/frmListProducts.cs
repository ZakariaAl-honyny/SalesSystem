using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using System.Data;
using SalesSystem.Desktop.Screens.Products;
using SalesSystem.Desktop.Screens.Products.Forms;
using SalesSystem.Desktop.Screens.Inventory.Forms;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms
{
    public partial class frmListProducts : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _productsTable = new();
        private DataView _productsView = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "كود المنتج", "Id" },
                { "اسم المنتج", "Name" },
                { "التصنيف",    "CategoryName" },
                { "الباركود",   "Barcode" },
                { "حالة المخزون", "StockStatus" },
            };

        public frmListProducts(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvProducts.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",           Header = "كود المنتج",    Fill = 7,  Min = 80 },
                new { Name = "Name",         Header = "اسم المنتج",    Fill = 18, Min = 200 },
                new { Name = "CategoryName", Header = "التصنيف",       Fill = 10, Min = 120 },
                new { Name = "Barcode",      Header = "الباركود",      Fill = 10, Min = 100 },
                new { Name = "StockStatus",  Header = "حالة المخزون",  Fill = 8,  Min = 80 },
                new { Name = "IsActive",     Header = "نشط",           Fill = 5,  Min = 60 },
            };

            foreach (var col in columns)
            {
                dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
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

        private async void _RefreshProductsList()
        {
            Cursor = dgvProducts.Cursor = Cursors.WaitCursor;
            try
            {
                _productsTable = new DataTable();
                _productsTable.Columns.Add("Id", typeof(int));
                _productsTable.Columns.Add("Name", typeof(string));
                _productsTable.Columns.Add("CategoryName", typeof(string));
                _productsTable.Columns.Add("Barcode", typeof(string));
                _productsTable.Columns.Add("StockStatus", typeof(string));
                _productsTable.Columns.Add("IsActive", typeof(bool));

                var result = await _serviceProvider.GetRequiredService<IProductApiService>().GetAllAsync();
                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var p in result.Value)
                    {
                        _productsTable.Rows.Add(
                            p.Id,
                            p.Name,
                            p.CategoryName ?? "",
                            p.Barcode ?? "",
                            p.StockStatusLabel,
                            p.IsActive);
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل المنتجات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _productsView = new DataView(_productsTable);
                dgvProducts.DataSource = _productsView;
                label1.Visible = _productsView.Count > 0;
                lblRecordsCount.Text = _productsView.Count > 0 ? _productsView.Count.ToString() : "لا توجد بيانات";

                // Apply row coloring after data binding
                _ApplyRowColoring();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "خطأ أثناء تحديث قائمة المنتجات");
                MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvProducts.Cursor = Cursors.Default;
            }
        }

        private void _ApplyRowColoring()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["StockStatus"].Value is string status)
                {
                    if (status == "نفذ")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else if (status == "محدود")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                        row.DefaultCellStyle.ForeColor = Color.SaddleBrown;
                    }
                }
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshProductsList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedProductId()
        {
            if (dgvProducts.CurrentRow == null) return -1;
            return (int)dgvProducts.CurrentRow.Cells[0].Value;
        }

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListProducts_Load(object sender, EventArgs e)
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

            _productsView.RowFilter = "";
            lblRecordsCount.Text = _productsView.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _productsView.RowFilter = "";
            }
            else if (columnName == "Id")
            {
                _productsView.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _productsView.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _productsView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "كود المنتج";

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

        private void dgvProducts_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedProductId();
            if (id > 0)
            {
                var editor = _serviceProvider.GetRequiredService<frmAddUpdateProduct>();
                editor.LoadData(id);
                _OpenForm(editor);
            }
        }

        private void cmsProducts_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsProducts.Enabled = dgvProducts.Rows.Count > 0 && dgvProducts.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
            => _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateProduct>());

        private void addNewProductToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateProduct>());

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedProductId();
            if (id > 0)
            {
                var editor = _serviceProvider.GetRequiredService<frmAddUpdateProduct>();
                editor.LoadData(id);
                _OpenForm(editor);
            }
        }

        private void viewStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedProductId();
            if (id > 0)
            {
                var dialog = _serviceProvider.GetRequiredService<frmListInventoryBatches>();
                dialog.ShowDialog();
                _ReloadForm();
            }
        }

        private void chkShowInactive_CheckedChanged(object sender, EventArgs e)
            => _ReloadForm();

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();
    }
}
