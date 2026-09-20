using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Accounting;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    public partial class frmListAccounts : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private List<AccountDto> _allAccounts = new();
        private List<AccountDto> _filteredAccounts = new();

        private static readonly Dictionary<string, Func<AccountDto, string, bool>> _filterMap =
            new()
            {
                { "كود الحساب",        (a, s) => a.AccountCode.Contains(s) },
                { "اسم الحساب",        (a, s) => a.NameAr.Contains(s) },
                { "الطبيعة",           (a, s) => a.NatureDisplay.Contains(s) },
                { "المستوى",           (a, s) => a.LevelDisplay.Contains(s) },
            };

        public frmListAccounts(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvAccounts.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",              Header = "المعرف",         Fill = 5,  Min = 60  },
                new { Name = "AccountCode",     Header = "كود الحساب",     Fill = 10, Min = 100 },
                new { Name = "NameAr",          Header = "اسم الحساب",     Fill = 18, Min = 200 },
                new { Name = "NameEn",          Header = "الاسم (إنج)",    Fill = 12, Min = 140 },
                new { Name = "NatureDisplay",   Header = "الطبيعة",        Fill = 10, Min = 110 },
                new { Name = "LevelDisplay",    Header = "المستوى",        Fill = 10, Min = 100 },
                new { Name = "IsActive",        Header = "نشط",            Fill = 5,  Min = 60  },
                new { Name = "Description",     Header = "الوصف",          Fill = 15, Min = 160 },
            };

            foreach (var col in columns)
            {
                dgvAccounts.Columns.Add(new DataGridViewTextBoxColumn
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

        private async void _RefreshAccounts()
        {
            Cursor = dgvAccounts.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IAccountApiService>().GetAllAsync(includeInactive: true);
                if (result.IsSuccess && result.Value != null)
                    _allAccounts = result.Value.ToList();
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _allAccounts = new();
                }
                else
                    _allAccounts = new();

                _filteredAccounts = _allAccounts.ToList();
                _PopulateGrid();
                _UpdateRecordsCount();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading accounts.");
                MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvAccounts.Cursor = Cursors.Default;
            }
        }

        private void _PopulateGrid()
        {
            dgvAccounts.AutoGenerateColumns = false;
            dgvAccounts.Rows.Clear();

            foreach (var a in _filteredAccounts)
            {
                dgvAccounts.Rows.Add(
                    a.Id,
                    a.AccountCode,
                    a.NameAr,
                    a.NameEn ?? "",
                    a.NatureDisplay,
                    a.LevelDisplay,
                    a.IsActive ? "نعم" : "لا",
                    a.Description ?? ""
                );
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshAccounts();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = _filteredAccounts.Count.ToString();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedAccountId()
        {
            if (dgvAccounts.CurrentRow == null) return -1;
            return (int)dgvAccounts.CurrentRow.Cells[0].Value;
        }

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListAccounts_Load(object sender, EventArgs e)
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

            _filteredAccounts = _allAccounts.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterMap.TryGetValue(cbFilterBy.Text, out var filterFunc))
            {
                _filteredAccounts = _allAccounts.ToList();
            }
            else
            {
                _filteredAccounts = _allAccounts.Where(a => filterFunc(a, filterText)).ToList();
            }

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

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvAccounts_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedAccountId();
            if (id > 0)
            {
                var frm = _serviceProvider.GetRequiredService<frmAddUpdateAccount>();
                frm._SetUpdateMode(id);
                _OpenForm(frm);
            }
        }

        private void cmsAccounts_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsAccounts.Enabled = dgvAccounts.Rows.Count > 0 && dgvAccounts.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateAccount>());
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateAccount>());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateAccount>();
            frm._SetUpdateMode(_GetSelectedAccountId());
            _OpenForm(frm);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateAccount>();
            frm._SetUpdateMode(_GetSelectedAccountId());
            _OpenForm(frm);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        // ── Delete ─────────────────────────────────────────────────────────────────

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedAccountId();
            if (id <= 0) return;

            if (MessageBox.Show($"هل أنت متأكد من حذف الحساب رقم [{id}]؟",
                    "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IAccountApiService>().DeleteAsync(id);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم حذف الحساب بنجاح.", "تم الحذف",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل حذف الحساب.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting account {Id}.", id);
                MessageBox.Show("حدث خطأ في حذف الحساب. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
