using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using System.Data;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Suppliers.Forms
{
    public partial class frmListSuppliers : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _suppliersTable = new();
        private DataView _suppliersView = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "الاسم",       "Name" },
                { "رقم الهاتف",  "Phone" },
            };

        private static readonly HashSet<string> _numericColumns =
            new HashSet<string> { "الاسم", "رقم الهاتف" };

        public frmListSuppliers(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvSuppliers.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",          Header = "رقم المورد",     Fill = 8,  Min = 80  },
                new { Name = "Name",        Header = "اسم المورد",     Fill = 25, Min = 200 },
                new { Name = "Phone",       Header = "رقم الهاتف",     Fill = 15, Min = 150 },
                new { Name = "CreditLimit", Header = "حد الائتمان",    Fill = 12, Min = 120 },
                new { Name = "IsActive",    Header = "نشط",            Fill = 8,  Min = 80  },
            };

            foreach (var col in columns)
            {
                var isCheckBox = col.Name == "IsActive";

                if (isCheckBox)
                {
                    dgvSuppliers.Columns.Add(new DataGridViewCheckBoxColumn
                    {
                        Name = col.Name,
                        HeaderText = col.Header,
                        DataPropertyName = col.Name,
                        MinimumWidth = col.Min,
                        FillWeight = col.Fill,
                        ReadOnly = true,
                    });
                }
                else
                {
                    var cellStyle = col.Name == "CreditLimit"
                        ? new DataGridViewCellStyle { Format = "N2" }
                        : null;

                    dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = col.Name,
                        HeaderText = col.Header,
                        DataPropertyName = col.Name,
                        MinimumWidth = col.Min,
                        FillWeight = col.Fill,
                        ReadOnly = true,
                        DefaultCellStyle = cellStyle,
                    });
                }
            }

            // Color-code credit limit
            dgvSuppliers.CellFormatting += dgvSuppliers_CellFormatting;
        }

        private void dgvSuppliers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvSuppliers.Columns[e.ColumnIndex].DataPropertyName == "CreditLimit" && e.Value is decimal limit)
            {
                if (limit > 0)
                    e.CellStyle.ForeColor = Color.Red;
                else
                    e.CellStyle.ForeColor = Color.Gray;
                e.CellStyle.Font = new Font(dgvSuppliers.Font, FontStyle.Bold);
            }
        }

        // ── Data Loading ───────────────────────────────────────────────────────────

        private async void _RefreshSuppliersList()
        {
            Cursor = dgvSuppliers.Cursor = Cursors.WaitCursor;

            _suppliersTable = new DataTable();
            _suppliersTable.Columns.Add("Id", typeof(int));
            _suppliersTable.Columns.Add("Name", typeof(string));
            _suppliersTable.Columns.Add("Phone", typeof(string));
            _suppliersTable.Columns.Add("CreditLimit", typeof(decimal));
            _suppliersTable.Columns.Add("IsActive", typeof(bool));

            try
            {
                var result = await _serviceProvider.GetRequiredService<ISupplierApiService>().GetAllAsync(
                    search: null, includeInactive: chkShowInactive.Checked);

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var s in result.Value)
                    {
                        _suppliersTable.Rows.Add(
                            s.Id,
                            s.Name,
                            s.Phone ?? "",
                            s.CreditLimit,
                            s.IsActive);
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل قائمة الموردين", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "حدث خطأ في تحميل قائمة الموردين");
                MessageBox.Show("حدث خطأ في تحميل قائمة الموردين. تم تسجيل التفاصيل للدعم الفني.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _suppliersView = new DataView(_suppliersTable);
                dgvSuppliers.DataSource = _suppliersView;
                lblRecordsCount.Text = _suppliersView.Count.ToString();

                Cursor = dgvSuppliers.Cursor = Cursors.Default;
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshSuppliersList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedSupplierID()
        {
            if (dgvSuppliers.CurrentRow == null) return -1;
            return (int)dgvSuppliers.CurrentRow.Cells[0].Value;
        }

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListSuppliers_Load(object sender, EventArgs e)
        {
            _ReloadForm();
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");
            ApplyPermissions();
        }

        private void ApplyPermissions()
        {
            var session = _serviceProvider.GetRequiredService<Services.Interfaces.ISessionService>();
            bool canManage = session.Current?.Role <= Common.Enums.UserRole.Manager;
            btnAdd.Visible = canManage;
            editToolStripMenuItem.Visible = canManage;
            deactivateToolStripMenuItem.Visible = canManage;
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

            _suppliersView.RowFilter = "";
            lblRecordsCount.Text = _suppliersView.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _suppliersView.RowFilter = "";
            }
            else
            {
                _suppliersView.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _suppliersView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }
        }

        // ── Show Inactive ──────────────────────────────────────────────────────────

        private void chkShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            _RefreshSuppliersList();
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvSuppliers_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedSupplierID();
            if (id > 0)
                ShowEditor(id);
        }

        private void cmsSuppliers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasRows = dgvSuppliers.Rows.Count > 0 && dgvSuppliers.CurrentRow != null;
            cmsSuppliers.Enabled = hasRows;
            editToolStripMenuItem.Enabled = hasRows;
            deactivateToolStripMenuItem.Enabled = hasRows;

            if (hasRows)
            {
                int id = _GetSelectedSupplierID();
                bool isActive = false;
                foreach (DataRowView row in _suppliersView)
                {
                    if ((int)row["Id"] == id)
                    {
                        isActive = (bool)row["IsActive"];
                        break;
                    }
                }

                deactivateToolStripMenuItem.Text = isActive ? "تعطيل" : "تنشيط";
            }
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
            => ShowEditor(null);

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
            => ShowEditor(null);

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedSupplierID();
            if (id > 0) ShowEditor(id);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        private void deactivateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedSupplierID();
            if (id <= 0) return;

            string name = "";
            bool isActive = false;
            foreach (DataRowView row in _suppliersView)
            {
                if ((int)row["Id"] == id)
                {
                    name = row["Name"] as string ?? "";
                    isActive = (bool)row["IsActive"];
                    break;
                }
            }

            string action = isActive ? "تعطيل" : "تنشيط";

            if (MessageBox.Show(
                    $"هل أنت متأكد من {action} المورد '{name}'؟",
                    $"تأكيد {action}",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            _ToggleStatus(id, deactivate: isActive);
        }

        // ── Core Actions ───────────────────────────────────────────────────────────

        private async void _ToggleStatus(int supplierId, bool deactivate)
        {
            try
            {
                var res = deactivate
                    ? await _serviceProvider.GetRequiredService<ISupplierApiService>().DeactivateAsync(supplierId)
                    : await _serviceProvider.GetRequiredService<ISupplierApiService>().ReactivateAsync(supplierId);

                if (res.IsSuccess)
                {
                    string action = deactivate ? "تعطيل" : "تنشيط";
                    MessageBox.Show($"تم {action} المورد بنجاح.", "نجاح",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadForm();
                }
                else
                {
                    MessageBox.Show(res.Error ?? "حدث خطأ أثناء تحديث حالة المورد.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "حدث خطأ أثناء تغيير حالة المورد {SupplierId}", supplierId);
                MessageBox.Show("حدث خطأ غير متوقع. تم تسجيل التفاصيل للدعم الفني.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowEditor(int? supplierId)
        {
            var editor = _serviceProvider.GetRequiredService<frmAddUpdateSupplier>();

            if (supplierId.HasValue)
            {
                SupplierDto? supplier = null;
                foreach (DataRowView row in _suppliersView)
                {
                    if ((int)row["Id"] == supplierId.Value)
                    {
                        supplier = new SupplierDto(
                            supplierId.Value,
                            row["Name"] as string ?? "",
                            row["Phone"] as string,
                            null,
                            null,
                            null,
                            null,
                            row["CreditLimit"] is decimal d ? d : 0,
                            (bool)row["IsActive"],
                            0
                        );
                        break;
                    }
                }
                editor.LoadData(supplier);
            }
            else
            {
                editor.LoadData(null);
            }

            clsGlobal._OpenForm(editor);
            _ReloadForm();
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
