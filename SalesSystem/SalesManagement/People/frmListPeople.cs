using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace SalesManagement.People
{
    public partial class frmListPeople : Form
    {
        private static DataView _dvAllPeople;
        private DataView _dvPeople;

        private readonly ToolTip _rejectToolTip;

        private static readonly Dictionary<string, string> _filterMap =
            new Dictionary<string, string>
            {
                { "رقم الشخص",            "PersonID"      },
                { "رقم الوطني",           "NationalNo"    },
                { "رقم الهاتف",           "Phone"         },
                { "الجنس",                "GenderCaption" },
                { "الأسم الاول",          "FirstName"     },
                { "الأسم الثاني",         "SecondName"    },
                { "الأسم الثالث",         "ThirdName"     },
                { "الأسم الاخير - اللقب", "LastName"      },
                { "الجنسية",              "CountryName"   },
                { "Email",                "Email"         },
                { "Address",              "Address"       },
            };

        // Columns that require exact numeric match instead of LIKE
        private static readonly HashSet<string> _numericColumns =
            new HashSet<string> { "PersonID" };

        public frmListPeople()
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

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvPeople.Columns.Clear();

            var columns = new[]
            {
                new { Name = "PersonID",      Header = "رقم الشخص",            Fill = 8,  Min = 120 },
                new { Name = "NationalNo",    Header = "الرقم الوطني",          Fill = 12, Min = 150 },
                new { Name = "FirstName",     Header = "الاسم الأول",           Fill = 10, Min = 130 },
                new { Name = "SecondName",    Header = "الاسم الثاني",          Fill = 10, Min = 130 },
                new { Name = "ThirdName",     Header = "الاسم الثالث",          Fill = 10, Min = 130 },
                new { Name = "LastName",      Header = "الاسم الأخير - اللقب",  Fill = 12, Min = 160 },
                new { Name = "GenderCaption", Header = "الجنس",                 Fill = 7,  Min = 100 },
                new { Name = "DateOfBirth",   Header = "تاريخ الميلاد",         Fill = 10, Min = 130 },
                new { Name = "CountryName",   Header = "الجنسية",               Fill = 10, Min = 130 },
                new { Name = "Phone",         Header = "رقم الهاتف",            Fill = 10, Min = 130 },
                new { Name = "Email",         Header = "البريد الإلكتروني",     Fill = 11, Min = 150 },
            };

            foreach (var col in columns)
            {
                dgvPeople.Columns.Add(new DataGridViewTextBoxColumn
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

        private void _RefreshPeopleList()
        {
            Cursor = dgvPeople.Cursor = Cursors.WaitCursor;

            _dvAllPeople = clsPerson.GetAllPeople();

            dgvPeople.AutoGenerateColumns = false;

            if (_dvAllPeople.Count > 0)
            {
                _dvPeople = _dvAllPeople
                    .ToTable(false,
                        "PersonID", "NationalNo",
                        "FirstName", "SecondName", "ThirdName", "LastName",
                        "GenderCaption", "DateOfBirth", "CountryName",
                        "Phone", "Email")
                    .DefaultView;

                dgvPeople.DataSource = _dvPeople;
            }
            else
            {
                dgvPeople.DataSource = null;
            }

            _UpdateRecordsCount();
            Cursor = dgvPeople.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshPeopleList();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = (_dvPeople?.Count ?? _dvAllPeople?.Count ?? 0).ToString();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedPersonID()
            => (int)dgvPeople.CurrentRow.Cells[0].Value;

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmManagePeople_Load(object sender, EventArgs e)
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

            if (_dvPeople != null)
            {
                _dvPeople.RowFilter = "";
                _UpdateRecordsCount();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (_dvPeople == null)
                return;

            string filterText = txtFilterValue.Text.Trim();

            if (!_filterMap.TryGetValue(cbFilterBy.Text, out string column) ||
                string.IsNullOrEmpty(filterText))
            {
                _dvPeople.RowFilter = "";
                _UpdateRecordsCount();
                return;
            }

            // Guard: skip if DataTable has no columns (empty DB result)
            if (_dvPeople.Table.Columns.Count == 0 ||
                !_dvPeople.Table.Columns.Contains(column))
            {
                _UpdateRecordsCount();
                return;
            }

            _dvPeople.RowFilter = _numericColumns.Contains(column)
                ? $"{column} = {filterText}"
                : $"{column} LIKE '{filterText}%'";

            _UpdateRecordsCount();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "رقم الشخص";

            if (isNumericFilter && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "يُسمح فقط بإدخال الأرقام في هذا الحقل.",
                    _rejectToolTip);
                return;
            }

            if (!isNumericFilter && char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "لا يُسمح بإدخال علامات الترقيم.",
                    _rejectToolTip);
            }
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvAllPeople_DoubleClick(object sender, EventArgs e)
        {
            if (dgvPeople.CurrentRow != null)
                _OpenForm(new frmShowPersonInfo(_GetSelectedPersonID()));
        }

        private void cmsPeople_Opening(object sender, CancelEventArgs e)
        {
            cmsPeople.Enabled = dgvPeople.Rows.Count > 0 && dgvPeople.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAddNewPerson_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdatePerson());

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdatePerson());

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmAddUpdatePerson(_GetSelectedPersonID()));

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(new frmShowPersonInfo(_GetSelectedPersonID()));

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        // ── Delete ─────────────────────────────────────────────────────────────────

        private void DeletePerson_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = _GetSelectedPersonID();

            if (MessageBox.Show(
                    $"هل أنت متأكد من حذف بيانات الشخص رقم [{personID}]؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            if (clsPerson.DeletePerson(personID))
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