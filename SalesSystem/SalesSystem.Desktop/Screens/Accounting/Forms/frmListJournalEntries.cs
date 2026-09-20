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
    public partial class frmListJournalEntries : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private List<JournalEntryListDto> _allEntries = new();
        private List<JournalEntryListDto> _filteredEntries = new();

        private static readonly Dictionary<string, Func<JournalEntryListDto, string, bool>> _filterMap =
            new()
            {
                { "رقم القيد",           (e, s) => e.EntryNumber.Contains(s) },
                { "البيان",              (e, s) => e.Description.Contains(s) },
                { "نوع القيد",           (e, s) => e.EntryType.Contains(s) },
                { "المرجع",              (e, s) => (e.ReferenceNumber ?? "").Contains(s) },
                { "الحالة",              (e, s) => (e.StatusDisplay ?? "").Contains(s) },
            };

        private static readonly HashSet<string> _numericColumns =
            new() { };

        public frmListJournalEntries(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvJournalEntries.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",               Header = "المعرف",          Fill = 5,  Min = 60  },
                new { Name = "EntryNumber",      Header = "رقم القيد",       Fill = 12, Min = 130 },
                new { Name = "EntryDate",        Header = "التاريخ",         Fill = 10, Min = 100 },
                new { Name = "Description",      Header = "البيان",          Fill = 18, Min = 200 },
                new { Name = "EntryType",        Header = "نوع القيد",       Fill = 10, Min = 110 },
                new { Name = "StatusDisplay",    Header = "الحالة",          Fill = 8,  Min = 90  },
                new { Name = "TotalDebit",       Header = "مجموع المدين",    Fill = 12, Min = 120 },
                new { Name = "TotalCredit",      Header = "مجموع الدائن",    Fill = 12, Min = 120 },
                new { Name = "ReferenceNumber",  Header = "رقم المرجع",      Fill = 10, Min = 110 },
            };

            foreach (var col in columns)
            {
                dgvJournalEntries.Columns.Add(new DataGridViewTextBoxColumn
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

        private async void _RefreshJournalEntries()
        {
            Cursor = dgvJournalEntries.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().GetAllAsync();
                if (result.IsSuccess && result.Value != null)
                    _allEntries = result.Value.ToList();
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _allEntries = new();
                }
                else
                    _allEntries = new();

                _filteredEntries = _allEntries.ToList();
                _PopulateGrid();
                _UpdateRecordsCount();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error loading journal entries.");
                MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvJournalEntries.Cursor = Cursors.Default;
            }
        }

        private void _PopulateGrid()
        {
            dgvJournalEntries.AutoGenerateColumns = false;

            dgvJournalEntries.Rows.Clear();

            foreach (var e in _filteredEntries)
            {
                dgvJournalEntries.Rows.Add(
                    e.Id,
                    e.EntryNumber,
                    clsFormat.DateToShort(e.EntryDate),
                    e.Description,
                    e.EntryType,
                    e.StatusDisplay ?? "",
                    e.TotalDebit.ToString("N2"),
                    e.TotalCredit.ToString("N2"),
                    e.ReferenceNumber ?? ""
                );
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshJournalEntries();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = _filteredEntries.Count.ToString();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedEntryId()
        {
            if (dgvJournalEntries.CurrentRow == null) return -1;
            return (int)dgvJournalEntries.CurrentRow.Cells[0].Value;
        }

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListJournalEntries_Load(object sender, EventArgs e)
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

            _filteredEntries = _allEntries.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterMap.TryGetValue(cbFilterBy.Text, out var filterFunc))
            {
                _filteredEntries = _allEntries.ToList();
            }
            else
            {
                _filteredEntries = _allEntries.Where(je => filterFunc(je, filterText)).ToList();
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

        private void dgvJournalEntries_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedEntryId();
            if (id > 0)
            {
                var frm = _serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>();
                frm._SetUpdateMode(id);
                _OpenForm(frm);
            }
        }

        private void cmsJournalEntries_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsJournalEntries.Enabled = dgvJournalEntries.Rows.Count > 0 && dgvJournalEntries.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>());
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>();
            frm._SetUpdateMode(_GetSelectedEntryId());
            _OpenForm(frm);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateJournalEntry>();
            frm._SetUpdateMode(_GetSelectedEntryId());
            _OpenForm(frm);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        // ── Post / Cancel ─────────────────────────────────────────────────────────

        private async void postToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedEntryId();
            if (id <= 0) return;

            if (MessageBox.Show($"هل أنت متأكد من ترحيل القيد رقم [{id}]؟",
                    "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().PostAsync(id);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم ترحيل القيد بنجاح.", "تم الترحيل",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل الترحيل.", "خطأ في الترحيل",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error posting journal entry {Id}.", id);
                MessageBox.Show("حدث خطأ في ترحيل القيد. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedEntryId();
            if (id <= 0) return;

            if (MessageBox.Show($"هل أنت متأكد من إلغاء القيد رقم [{id}]؟",
                    "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IJournalEntryApiService>().CancelAsync(id);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إلغاء القيد بنجاح.", "تم الإلغاء",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل الإلغاء.", "خطأ في الإلغاء",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error cancelling journal entry {Id}.", id);
                MessageBox.Show("حدث خطأ في إلغاء القيد. يرجى المحاولة مرة أخرى.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
