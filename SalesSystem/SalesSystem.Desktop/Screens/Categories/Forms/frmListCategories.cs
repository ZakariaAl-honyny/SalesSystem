using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Categories;

namespace SalesSystem.Desktop.Screens.Categories.Forms
{
    public partial class frmListCategories : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtCategories = new();
        private DataView _dvCategories = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "رقم التصنيف", "CategoryID" },
                { "اسم التصنيف", "CategoryName" },
            };

        public frmListCategories(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvCategories.Columns.Clear();

            var columns = new[]
            {
                new { Name = "CategoryID",   Header = "رقم التصنيف",  Fill = 8,  Min = 100 },
                new { Name = "CategoryName", Header = "اسم التصنيف",  Fill = 22, Min = 200 },
                new { Name = "Description",  Header = "الوصف",        Fill = 18, Min = 250 },
            };

            foreach (var col in columns)
            {
                dgvCategories.Columns.Add(new DataGridViewTextBoxColumn
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

        private async void _RefreshCategoriesList()
        {
            Cursor = dgvCategories.Cursor = Cursors.WaitCursor;

            try
            {
                _dtCategories = new DataTable();
                _dtCategories.Columns.Add("CategoryID", typeof(int));
                _dtCategories.Columns.Add("CategoryName", typeof(string));
                _dtCategories.Columns.Add("Description", typeof(string));

                var result = await _serviceProvider.GetRequiredService<ICategoryApiService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var c in result.Value)
                    {
                        _dtCategories.Rows.Add(c.Id, c.Name, c.Description ?? "");
                    }
                }

                _dvCategories = new DataView(_dtCategories);
                dgvCategories.DataSource = _dvCategories;
                lblRecordsCount.Text = _dvCategories.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات التصنيفات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor = dgvCategories.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshCategoriesList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedCategoryId()
        {
            if (dgvCategories.CurrentRow == null) return -1;
            return Convert.ToInt32(dgvCategories.CurrentRow.Cells[0].Value);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListCategories_Load(object sender, EventArgs e)
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
            _dvCategories.RowFilter = "";
            lblRecordsCount.Text = _dvCategories.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _dvCategories.RowFilter = "";
            }
            else if (columnName == "CategoryID")
            {
                _dvCategories.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _dvCategories.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _dvCategories.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "رقم التصنيف";

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

        private void dgvCategories_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedCategoryId();
            if (id > 0) _OpenEditDialog();
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<frmAddUpdateCategory>();
            clsGlobal._OpenForm(dlg);
            _ReloadForm();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
            => btnAdd_Click(sender, e);

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow == null) return;
            _OpenEditDialog();
        }

        private void _OpenEditDialog()
        {
            var dlg = _serviceProvider.GetRequiredService<frmAddUpdateCategory>();
            dlg.LoadData(new CategoryDto(
                _GetSelectedCategoryId(),
                dgvCategories.CurrentRow?.Cells["CategoryName"].Value?.ToString() ?? "",
                dgvCategories.CurrentRow?.Cells["Description"].Value?.ToString(),
                true
            ));
            clsGlobal._OpenForm(dlg);
            _ReloadForm();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        private void cmsCategories_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsCategories.Enabled = dgvCategories.Rows.Count > 0 && dgvCategories.CurrentRow != null;
        }
    }
}
