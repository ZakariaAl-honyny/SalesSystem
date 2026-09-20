using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using SalesManagement.Unit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace SalesManagement.Units
{
    public partial class frmListUnits : Form
    {
        private static DataView _dvUnits;

        private static readonly Dictionary<string, string> _filterMap =
            new Dictionary<string, string>
            {
                { "رقم الوحدة",    "UnitID"          },
                { "اسم الوحدة",    "UnitName"        },
                { "تفاصيل الوحدة", "UnitDescription" },
            };

        public frmListUnits()
        {
            InitializeComponent();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedUnitID()
            => (int)dgvUnits.CurrentRow.Cells[0].Value;

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            // Clear any auto-generated columns, then rebuild manually.
            // This guarantees headers always appear — even when DataSource is empty.
            dgvUnits.Columns.Clear();

            var columns = new[]
            {
                new { Name = "UnitID",          Header = "رقم الوحدة",    Fill = 15, Min = 100 },
                new { Name = "UnitName",         Header = "اسم الوحدة",    Fill = 30, Min = 180 },
                new { Name = "UnitDescription",  Header = "تفاصيل الوحدة", Fill = 55, Min = 250 },
            };

            foreach (var col in columns)
            {
                dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.Name,    // ربط الاسم بحقل الـ DataView
                    MinimumWidth = col.Min,
                    FillWeight = col.Fill,
                    ReadOnly = true,
                });
            }
        }

        // ── Data Loading ───────────────────────────────────────────────────────────

        private void _RefreshUnitsList()
        {
            Cursor = dgvUnits.Cursor = Cursors.WaitCursor;

            _dvUnits = clsUnit.GetAllUnits();

            // Must be false — columns are managed manually in _SetupGridColumns.
            // If true, DataSource will auto-create duplicate/unnamed columns.
            dgvUnits.AutoGenerateColumns = false;
            dgvUnits.DataSource = _dvUnits;

            _UpdateRecordsCount();

            Cursor = dgvUnits.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            // Setup columns BEFORE binding data so headers exist from the start
            _SetupGridColumns();
            _RefreshUnitsList();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = _dvUnits?.Count.ToString() ?? "0";
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListUnits_Load(object sender, EventArgs e)
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
            txtFilterValue.Clear();

            if (txtFilterValue.Visible)
                txtFilterValue.Focus();

            if (_dvUnits != null)
            {
                _dvUnits.RowFilter = "";
                _UpdateRecordsCount();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (_dvUnits == null)
                return;

            string filterText = txtFilterValue.Text.Trim();

            // Clear filter if no column selected or input is empty
            if (!_filterMap.TryGetValue(cbFilterBy.Text, out string column) ||
                string.IsNullOrEmpty(filterText))
            {
                _dvUnits.RowFilter = "";
                _UpdateRecordsCount();
                return;
            }

            // Guard: if DataTable has no columns (empty result from DB),
            // skip filtering — there is nothing to filter against
            if (_dvUnits.Table.Columns.Count == 0 ||
                !_dvUnits.Table.Columns.Contains(column))
            {
                _UpdateRecordsCount();
                return;
            }

            _dvUnits.RowFilter = column == "UnitID"
                ? $"{column} = {filterText}"
                : $"{column} LIKE '%{filterText}%'";

            _UpdateRecordsCount();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "رقم الوحدة")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvAllUnits_DoubleClick(object sender, EventArgs e)
        {
            if (dgvUnits.CurrentRow != null)
                _OpenForm(new frmAddUpdateUnit(_GetSelectedUnitID()));
        }

        private void cmsUnits_Opening(object sender, CancelEventArgs e)
        {
            cmsUnits.Enabled = dgvUnits.Rows.Count > 0 && dgvUnits.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAddNewUnit_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdateUnit());

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdateUnit());

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdateUnit(_GetSelectedUnitID()));

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
            => _ReloadForm();

        // ── Delete ─────────────────────────────────────────────────────────────────

        private void DeleteUnit_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int unitID = _GetSelectedUnitID();

            if (MessageBox.Show(
                    $"هل أنت متأكد من حذف بيانات الوحدة رقم [{unitID}]؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            if (clsUnit.DeleteUnit(unitID))
            {
                MessageBox.Show("تم حذف البيانات بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show("لم يتم حذف البيانات.", "فشل الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}