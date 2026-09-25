using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    public partial class frmCostCenters : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtCostCenters = new();
        private DataView _dvCostCenters = new();

        private static readonly Dictionary<string, string> _filterColumns = new()
        {
            { "الكود",       "Code" },
            { "الاسم",       "Name" },
            { "المركز الأب", "ParentName" },
            { "المدير",      "ManagerName" },
        };

        public frmCostCenters(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvCostCenters.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",          Header = "الرقم",       Fill = 6,  Min = 60  },
                new { Name = "Code",        Header = "الكود",       Fill = 8,  Min = 80  },
                new { Name = "Name",        Header = "الاسم",       Fill = 22, Min = 200 },
                new { Name = "ParentName",  Header = "المركز الأب", Fill = 18, Min = 160 },
                new { Name = "Level",       Header = "المستوى",     Fill = 7,  Min = 70  },
                new { Name = "Budget",      Header = "الموازنة",    Fill = 12, Min = 120 },
                new { Name = "ManagerName", Header = "المدير",      Fill = 15, Min = 140 },
                new { Name = "StatusDisplay", Header = "الحالة",    Fill = 8,  Min = 80  },
            };

            foreach (var col in columns)
            {
                dgvCostCenters.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.Name,
                    MinimumWidth = col.Min,
                    FillWeight = col.Fill,
                    ReadOnly = true,
                });
            }

            // تنسيق عمود الموازنة كأرقام
            if (dgvCostCenters.Columns.Contains("Budget"))
            {
                dgvCostCenters.Columns["Budget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvCostCenters.Columns["Budget"].DefaultCellStyle.Format = "N0";
            }

            // تنسيق عمود المستوى
            if (dgvCostCenters.Columns.Contains("Level"))
            {
                dgvCostCenters.Columns["Level"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        // ── Data Loading ───────────────────────────────────────────────────────────

        private async void _RefreshCostCentersList()
        {
            Cursor = dgvCostCenters.Cursor = Cursors.WaitCursor;

            try
            {
                _dtCostCenters = new DataTable();
                _dtCostCenters.Columns.Add("Id", typeof(int));
                _dtCostCenters.Columns.Add("Code", typeof(string));
                _dtCostCenters.Columns.Add("Name", typeof(string));
                _dtCostCenters.Columns.Add("ParentName", typeof(string));
                _dtCostCenters.Columns.Add("Level", typeof(byte));
                _dtCostCenters.Columns.Add("Budget", typeof(decimal));
                _dtCostCenters.Columns.Add("ManagerName", typeof(string));
                _dtCostCenters.Columns.Add("StatusDisplay", typeof(string));

                var api = _serviceProvider.GetRequiredService<ICostCenterApiService>();
                var result = await api.GetAllAsync(includeInactive: true);

                if (result.IsSuccess && result.Value != null)
                {
                    // بناء قاموس الأسماء الأب لحل الاسم من الكود
                    var parentLookup = result.Value.ToDictionary(c => c.Code, c => c.Name);

                    foreach (var cc in result.Value)
                    {
                        string parentName = "";
                        if (cc.ParentCode != null && parentLookup.TryGetValue(cc.ParentCode, out var pName))
                            parentName = pName;

                        _dtCostCenters.Rows.Add(
                            cc.Id,
                            cc.Code,
                            cc.Name,
                            parentName,
                            cc.Level,
                            cc.Budget,
                            cc.ManagerName ?? "",
                            cc.StatusDisplay
                        );
                    }
                }

                _dvCostCenters = new DataView(_dtCostCenters);
                dgvCostCenters.DataSource = _dvCostCenters;
                lblRecordsCount.Text = _dvCostCenters.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات مراكز التكلفة: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor = dgvCostCenters.Cursor = Cursors.Default;
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshCostCentersList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedCostCenterId()
        {
            if (dgvCostCenters.CurrentRow == null) return -1;
            return Convert.ToInt32(dgvCostCenters.CurrentRow.Cells["Id"].Value);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmCostCenters_Load(object sender, EventArgs e)
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
            _dvCostCenters.RowFilter = "";
            lblRecordsCount.Text = _dvCostCenters.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _dvCostCenters.RowFilter = "";
            }
            else if (columnName == "Level")
            {
                _dvCostCenters.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _dvCostCenters.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _dvCostCenters.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "الكود" || cbFilterBy.Text == "المستوى";

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

        private void dgvCostCenters_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedCostCenterId();
            if (id > 0) btnEdit_Click(sender, e);
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // TODO: فتح نموذج إضافة مركز تكلفة جديد (frmAddUpdateCostCenter)
            MessageBox.Show("سيتم فتح نموذج إضافة مركز تكلفة جديد.",
                "إضافة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _ReloadForm();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedCostCenterId();
            if (id <= 0) return;

            try
            {
                var api = _serviceProvider.GetRequiredService<ICostCenterApiService>();
                var result = await api.GetByIdAsync(id);
                if (result.IsSuccess && result.Value != null)
                {
                    // TODO: فتح نموذج تعديل مركز التكلفة (frmAddUpdateCostCenter)
                    MessageBox.Show($"تعديل مركز التكلفة: {result.Value.Name}",
                        "تعديل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "مركز التكلفة غير موجود",
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في فتح مركز التكلفة: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedCostCenterId();
            if (id <= 0) return;

            var confirm = MessageBox.Show("هل أنت متأكد من تعطيل هذا المركز؟",
                "تأكيد التعطيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                var api = _serviceProvider.GetRequiredService<ICostCenterApiService>();
                var result = await api.DeleteAsync(id);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تعطيل مركز التكلفة بنجاح.",
                        "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل التعطيل",
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تعطيل مركز التكلفة: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();
    }
}
