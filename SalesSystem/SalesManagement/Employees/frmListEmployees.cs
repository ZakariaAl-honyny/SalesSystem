using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace SalesManagement.Employees
{
    public partial class frmListEmployees : Form
    {
        private static DataView _dvEmployees;

        // Reusable tooltip — created once, used for all rejection messages
        private ToolTip _rejectToolTip;

        private static readonly Dictionary<string, string> _filterMap =
            new Dictionary<string, string>
            {
                { "رقم الموظف",    "EmployeeID"            },
                { "رقم الشخص",     "PersonID"              },
                { "أسم الموظف",    "FullName"              },
                { "مؤهل الموظف",   "EmployeeQualification" },
                { "حالة الموظف",   "EmployeeState"         },
            };

        private static readonly HashSet<string> _numericColumns =
            new HashSet<string> { "EmployeeID", "PersonID" };

        public frmListEmployees()
        {
            InitializeComponent();

            _rejectToolTip = new ToolTip
            {
                IsBalloon = true,
                ToolTipIcon = ToolTipIcon.Warning,
                ToolTipTitle = "تنبيه",
                UseAnimation = true,
                UseFading = true,
            };
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedEmployeeID()
            => (int)dgvEmployees.CurrentRow.Cells[0].Value;

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = _dvEmployees?.Count.ToString() ?? "0";
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvEmployees.Columns.Clear();

            var columns = new[]
            {
                new { Name = "EmployeeID",            Header = "رقم الموظف",  Fill = 10, Min = 120 },
                new { Name = "PersonID",               Header = "رقم الشخص",  Fill = 10, Min = 120 },
                new { Name = "FullName",               Header = "اسم الموظف", Fill = 30, Min = 200 },
                new { Name = "EmployeeQualification",  Header = "المؤهل",     Fill = 25, Min = 180 },
                new { Name = "EmployeeState",          Header = "الحالة",     Fill = 25, Min = 150 },
            };

            foreach (var col in columns)
            {
                dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn
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

        private void _RefreshEmployeesList()
        {
            Cursor = dgvEmployees.Cursor = Cursors.WaitCursor;

            _dvEmployees = clsEmployee.GetAllEmployees();

            dgvEmployees.AutoGenerateColumns = false;
            dgvEmployees.DataSource = _dvEmployees;

            _UpdateRecordsCount();

            Cursor = dgvEmployees.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshEmployeesList();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListEmployees_Load(object sender, EventArgs e)
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

            if (_dvEmployees != null)
            {
                _dvEmployees.RowFilter = "";
                _UpdateRecordsCount();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (_dvEmployees == null)
                return;

            string filterText = txtFilterValue.Text.Trim();

            if (!_filterMap.TryGetValue(cbFilterBy.Text, out string column) ||
                string.IsNullOrEmpty(filterText))
            {
                _dvEmployees.RowFilter = "";
                _UpdateRecordsCount();
                return;
            }

            if (_dvEmployees.Table.Columns.Count == 0 ||
                !_dvEmployees.Table.Columns.Contains(column))
            {
                _UpdateRecordsCount();
                return;
            }

            _dvEmployees.RowFilter = _numericColumns.Contains(column)
                ? $"{column} = {filterText}"
                : $"{column} LIKE '%{filterText}%'";

            _UpdateRecordsCount();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool isNumericFilter = cbFilterBy.Text == "رقم الموظف" ||
                                   cbFilterBy.Text == "رقم الشخص";

            if (isNumericFilter)
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;

                    // Sound + flash + tooltip telling the user only digits are allowed
                    clsInputFeedback.ShowRejectTooltip(
                        txtFilterValue,
                        "يُسمح فقط بإدخال الأرقام في هذا الحقل.",
                        _rejectToolTip);
                }
            }
            else
            {
                if (char.IsPunctuation(e.KeyChar))
                {
                    e.Handled = true;

                    clsInputFeedback.ShowRejectTooltip(
                        txtFilterValue,
                        "لا يُسمح بإدخال علامات الترقيم.",
                        _rejectToolTip);
                }
            }
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvAllEmployees_DoubleClick(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null)
                _OpenForm(new frmShowEmployeeInfo(_GetSelectedEmployeeID()));
        }

        private void cmsEmployees_Opening(object sender, CancelEventArgs e)
        {
            cmsEmployees.Enabled = dgvEmployees.Rows.Count > 0 && dgvEmployees.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAddNewEmployee_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdateEmployee());

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdateEmployee());

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdateEmployee(_GetSelectedEmployeeID()));

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmShowEmployeeInfo(_GetSelectedEmployeeID()));

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
            => _ReloadForm();

        // ── Delete ─────────────────────────────────────────────────────────────────

        private void DeleteEmployee_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int employeeID = _GetSelectedEmployeeID();

            if (MessageBox.Show(
                    $"هل أنت متأكد من حذف بيانات الموظف رقم [{employeeID}]؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            if (clsEmployee.DeleteEmployee(employeeID))
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

        // ── Future Features ────────────────────────────────────────────────────────

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
            => MessageBox.Show("إرسال البريد الإلكتروني سيكون متاحاً قريباً.", "قريباً",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void callPhoneToolStripMenuItem_Click(object sender, EventArgs e)
            => MessageBox.Show("خاصية الاتصال الهاتفي ستكون متاحة قريباً.", "قريباً",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}