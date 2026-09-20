using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.People.Forms
{
    public partial class frmListPeople : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private List<PersonDto> _allPeople = new();
        private List<PersonDto> _filteredPeople = new();

        private static readonly Dictionary<string, Func<PersonDto, string, bool>> _filterMap =
            new Dictionary<string, Func<PersonDto, string, bool>>
            {
                { "رقم الشخص",            (p, s) => p.Id.ToString().StartsWith(s) },
                { "رقم الوطني",           (p, s) => p.NationalNo.Contains(s) },
                { "رقم الهاتف",           (p, s) => (p.Phone ?? "").Contains(s) },
                { "الجنس",                (p, s) => p.GenderDisplay.Contains(s) },
                { "الأسم الاول",          (p, s) => p.FirstName.Contains(s) },
                { "الأسم الثاني",         (p, s) => p.SecondName.Contains(s) },
                { "الأسم الثالث",         (p, s) => (p.ThirdName ?? "").Contains(s) },
                { "الأسم الاخير - اللقب", (p, s) => p.LastName.Contains(s) },
                { "الجنسية",              (p, s) => (p.CountryName ?? "").Contains(s) },
                { "Email",                (p, s) => (p.Email ?? "").Contains(s) },
                { "Address",              (p, s) => (p.Address ?? "").Contains(s) },
            };

        private static readonly HashSet<string> _numericColumns =
            new HashSet<string> { "Id" };

        public frmListPeople(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
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

        private async void _RefreshPeopleList()
        {
            Cursor = dgvPeople.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IPersonApiService>().GetAllAsync();
                if (result.IsSuccess && result.Value != null)
                    _allPeople = result.Value.ToList();
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _allPeople = new List<PersonDto>();
                }
                else
                    _allPeople = new List<PersonDto>();

                _filteredPeople = _allPeople.ToList();
                _PopulateGrid();
                _UpdateRecordsCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ غير متوقع: " + ex.Message, "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvPeople.Cursor = Cursors.Default;
            }
        }

        private void _PopulateGrid()
        {
            dgvPeople.AutoGenerateColumns = false;

            dgvPeople.Rows.Clear();

            foreach (var p in _filteredPeople)
            {
                dgvPeople.Rows.Add(
                    p.Id,
                    p.NationalNo,
                    p.FirstName,
                    p.SecondName,
                    p.ThirdName ?? "",
                    p.LastName,
                    p.GenderDisplay,
                    clsFormat.DateToShort(p.DateOfBirth),
                    p.CountryName ?? "",
                    p.Phone ?? "",
                    p.Email ?? ""
                );
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshPeopleList();
        }

        private void _UpdateRecordsCount()
        {
            lblRecordsCount.Text = _filteredPeople.Count.ToString();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedPersonID()
        {
            if (dgvPeople.CurrentRow == null) return -1;
            return (int)dgvPeople.CurrentRow.Cells[0].Value;
        }

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

            _filteredPeople = _allPeople.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterMap.TryGetValue(cbFilterBy.Text, out var filterFunc))
            {
                _filteredPeople = _allPeople.ToList();
            }
            else
            {
                _filteredPeople = _allPeople.Where(p => filterFunc(p, filterText)).ToList();
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

            bool isNumericFilter = cbFilterBy.Text == "رقم الشخص";

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

        private void dgvAllPeople_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedPersonID();
            if (id <= 0) return;
            var frm = _serviceProvider.GetRequiredService<frmShowPersonInfo>();
            frm._LoadData(id);
            _OpenForm(frm);
        }

        private void cmsPeople_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsPeople.Enabled = dgvPeople.Rows.Count > 0 && dgvPeople.CurrentRow != null;
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAddNewPerson_Click(object sender, EventArgs e)
            => _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdatePerson>());

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdatePerson>());

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdatePerson>();
            frm._LoadData(_GetSelectedPersonID());
            _OpenForm(frm);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = _serviceProvider.GetRequiredService<frmShowPersonInfo>();
            frm._LoadData(_GetSelectedPersonID());
            _OpenForm(frm);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        // ── Delete ─────────────────────────────────────────────────────────────────

        private void DeletePerson_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = _GetSelectedPersonID();

            if (MessageBox.Show(
                    $"هل أنت متأكد من حذف بيانات الشخص رقم [{personID}]؟",
                    "تأكيد الحذف",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            // TODO: Call API delete endpoint when backend is ready
            // if (clsPerson.DeletePerson(personID))
            // {
            //     MessageBox.Show("تم حذف البيانات بنجاح.", "تم الحذف",
            //         MessageBoxButtons.OK, MessageBoxIcon.Information);
            //     _ReloadForm();
            // }
            // else
            // {
            //     MessageBox.Show("لم يتم حذف البيانات.", "فشل الحذف",
            //         MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }
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
