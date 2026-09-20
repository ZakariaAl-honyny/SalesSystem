using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Accounting;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    public partial class frmListAccountCategories : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private List<AccountCategoryDto> _allCategories = new();
        private List<AccountCategoryDto> _filteredCategories = new();

        private static readonly Dictionary<string, Func<AccountCategoryDto, string, bool>> _filterMap =
            new()
            {
                { "الاسم",    (c, s) => c.Name.Contains(s) },
                { "الوصف",    (c, s) => (c.Description ?? "").Contains(s) },
            };

        private bool _includeInactive = false;

        public frmListAccountCategories(IServiceProvider serviceProvider)
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
                new { Name = "Id",           Header = "المعرف",      Fill = 5,  Min = 60  },
                new { Name = "Name",         Header = "الاسم",       Fill = 25, Min = 200 },
                new { Name = "Description",  Header = "الوصف",       Fill = 30, Min = 250 },
                new { Name = "IsActive",     Header = "نشط",         Fill = 5,  Min = 60  },
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

        private async void _RefreshCategories()
        {
            Cursor = dgvCategories.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IAccountCategoryApiService>().GetAllAsync();
                if (result.IsSuccess && result.Value != null)
                    _allCategories = result.Value.ToList();
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _allCategories = new();
                }
                else
                    _allCategories = new();

                _filteredCategories = _allCategories.ToList();
                _ApplyInactiveFilter();
                _PopulateGrid();
                _UpdateRecordsCount();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading account categories.");
                MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvCategories.Cursor = Cursors.Default;
            }
        }

        private void _PopulateGrid()
        {
            dgvCategories.AutoGenerateColumns = false;
            dgvCategories.Rows.Clear();

            foreach (var c in _filteredCategories)
            {
                dgvCategories.Rows.Add(
                    c.Id,
                    c.Name,
                    c.Description ?? "",
                    c.IsActive ? "نعم" : "لا"
                );
            }
        }

        private void _ApplyInactiveFilter()
        {
            if (!_includeInactive)
                _filteredCategories = _filteredCategories.Where(c => c.IsActive).ToList();
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshCategories();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = _filteredCategories.Count.ToString();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedCategoryId()
        {
            if (dgvCategories.CurrentRow == null) return -1;
            return (int)dgvCategories.CurrentRow.Cells[0].Value;
        }

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListAccountCategories_Load(object sender, EventArgs e)
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

            _filteredCategories = _allCategories.ToList();
            _ApplyInactiveFilter();
            _PopulateGrid();
            _UpdateRecordsCount();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterMap.TryGetValue(cbFilterBy.Text, out var filterFunc))
            {
                _filteredCategories = _allCategories.ToList();
            }
            else
            {
                _filteredCategories = _allCategories.Where(c => filterFunc(c, filterText)).ToList();
            }

            _ApplyInactiveFilter();
            _PopulateGrid();
            _UpdateRecordsCount();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            if (char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "لا يُسمح بإدخال علامات الترقيم.");
            }
        }

        private void chkIncludeInactive_CheckedChanged(object sender, EventArgs e)
        {
            _includeInactive = chkIncludeInactive.Checked;
            _filteredCategories = _allCategories.ToList();
            _ApplyInactiveFilter();
            _PopulateGrid();
            _UpdateRecordsCount();
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvCategories_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedCategoryId();
            if (id > 0)
            {
                var frm = _serviceProvider.GetRequiredService<frmAddUpdateAccountCategory>();
                frm._SetUpdateMode(id);
                _OpenForm(frm);
            }
        }

        private void cmsCategories_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsCategories.Enabled = dgvCategories.Rows.Count > 0 && dgvCategories.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateAccountCategory>());
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateAccountCategory>());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateAccountCategory>();
            frm._SetUpdateMode(_GetSelectedCategoryId());
            _OpenForm(frm);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateAccountCategory>();
            frm._SetUpdateMode(_GetSelectedCategoryId());
            _OpenForm(frm);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        // ── Delete ─────────────────────────────────────────────────────────────────

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedCategoryId();
            if (id <= 0) return;

            if (MessageBox.Show($"هل أنت متأكد من حذف تصنيف الحسابات رقم [{id}]؟",
                    "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IAccountCategoryApiService>().DeleteAsync(id);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم حذف التصنيف بنجاح.", "تم الحذف",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل حذف التصنيف.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting account category {Id}.", id);
                MessageBox.Show("حدث خطأ في حذف التصنيف. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
