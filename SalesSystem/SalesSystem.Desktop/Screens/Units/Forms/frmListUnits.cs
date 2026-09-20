using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Units;

namespace SalesSystem.Desktop.Screens.Units.Forms
{
    public partial class frmListUnits : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtUnits = new();
        private DataView _dvUnits = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "الرقم", "UnitID" },
                { "الاسم", "Name" },
                { "الرمز", "Symbol" },
            };

        public frmListUnits(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvUnits.Columns.Clear();

            var columns = new[]
            {
                new { Name = "UnitID", Header = "الرقم", Fill = 8,  Min = 80  },
                new { Name = "Name",   Header = "الاسم", Fill = 22, Min = 200 },
                new { Name = "Symbol", Header = "الرمز", Fill = 10, Min = 100 },
            };

            foreach (var col in columns)
            {
                dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
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

        private async void _RefreshUnitsList()
        {
            Cursor = dgvUnits.Cursor = Cursors.WaitCursor;

            try
            {
                _dtUnits = new DataTable();
                _dtUnits.Columns.Add("UnitID", typeof(int));
                _dtUnits.Columns.Add("Name", typeof(string));
                _dtUnits.Columns.Add("Symbol", typeof(string));

                var result = await _serviceProvider.GetRequiredService<IUnitApiService>().GetAllAsync(false);

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var u in result.Value)
                    {
                        _dtUnits.Rows.Add(u.Id, u.Name, u.Symbol ?? "");
                    }
                }

                _dvUnits = new DataView(_dtUnits);
                dgvUnits.DataSource = _dvUnits;
                lblRecordsCount.Text = _dvUnits.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات الوحدات: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor = dgvUnits.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshUnitsList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedUnitId()
        {
            if (dgvUnits.CurrentRow == null) return -1;
            return Convert.ToInt32(dgvUnits.CurrentRow.Cells[0].Value);
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
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
            _dvUnits.RowFilter = "";
            lblRecordsCount.Text = _dvUnits.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _dvUnits.RowFilter = "";
            }
            else if (columnName == "UnitID")
            {
                _dvUnits.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _dvUnits.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _dvUnits.Count.ToString();
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

        private void dgvUnits_DoubleClick(object sender, EventArgs e)
            => btnEdit_Click(sender, e);

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = _serviceProvider.GetRequiredService<frmAddUpdateUnit>();
            dlg.LoadData(null);
            clsGlobal._OpenForm(dlg);
            _ReloadForm();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedUnitId();
            if (id <= 0) return;

            try
            {
                var result = await _serviceProvider.GetRequiredService<IUnitApiService>().GetByIdAsync(id);
                if (result.IsSuccess && result.Value != null)
                {
                    var dlg = _serviceProvider.GetRequiredService<frmAddUpdateUnit>();
                    dlg.LoadData(result.Value);
                    clsGlobal._OpenForm(dlg);
                    _ReloadForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح الوحدة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUnits.CurrentRow == null) return;
            int id = _GetSelectedUnitId();
            string name = dgvUnits.CurrentRow.Cells[1].Value?.ToString() ?? "";

            if (MessageBox.Show("هل تريد حذف/تعطيل الوحدة: " + name + "؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            var result = await _serviceProvider.GetRequiredService<IUnitApiService>().DeleteAsync(id);
            if (result.IsSuccess)
            {
                MessageBox.Show("تمت العملية بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشلت العملية", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();
    }
}
