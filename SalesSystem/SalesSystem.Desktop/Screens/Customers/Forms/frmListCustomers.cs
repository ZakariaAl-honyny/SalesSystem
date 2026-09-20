using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using System.Data;
using SalesSystem.Desktop.Screens.Customers;
using SalesSystem.Desktop.Helpers;
 
namespace SalesSystem.Desktop.Screens.Customers.Forms
{
    public partial class frmListCustomers : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _customersTable = new();
        private DataView _customersView = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "رقم العميل", "Id" },
                { "اسم العميل", "Name" },
                { "رقم الهاتف", "Phone" },
            };

        public frmListCustomers(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvCustomers.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Id",         Header = "رقم العميل",      Fill = 8,  Min = 100 },
                new { Name = "Name",       Header = "اسم العميل",      Fill = 22, Min = 200 },
                new { Name = "Phone",      Header = "رقم الهاتف",      Fill = 12, Min = 130 },
                new { Name = "CreditLimit",Header = "الحد الائتماني",  Fill = 10, Min = 100 },
                new { Name = "IsActive",   Header = "نشط",             Fill = 5,  Min = 50 },
            };

            foreach (var col in columns)
            {
                dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
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

        private async void _RefreshCustomersList()
        {
            Cursor = dgvCustomers.Cursor = Cursors.WaitCursor;
            try
            {
                _customersTable = new DataTable();
                _customersTable.Columns.Add("Id", typeof(int));
                _customersTable.Columns.Add("Name", typeof(string));
                _customersTable.Columns.Add("Phone", typeof(string));
                _customersTable.Columns.Add("CreditLimit", typeof(decimal));
                _customersTable.Columns.Add("IsActive", typeof(bool));

                var result = await _serviceProvider.GetRequiredService<ICustomerApiService>().GetAllAsync(
                    includeInactive: chkShowInactive.Checked);

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var c in result.Value)
                    {
                        _customersTable.Rows.Add(
                            c.Id,
                            c.Name,
                            c.Phone ?? "",
                            c.CreditLimit,
                            c.IsActive);
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _customersView = new DataView(_customersTable);
                dgvCustomers.DataSource = _customersView;
                lblRecordsCount.Text = _customersView.Count.ToString();
                Cursor = dgvCustomers.Cursor = Cursors.Default;
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshCustomersList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedCustomerId()
        {
            if (dgvCustomers.CurrentRow == null) return -1;
            return (int)dgvCustomers.CurrentRow.Cells[0].Value;
        }

        private void _OpenForm(Form form)
        {
            clsGlobal._OpenForm(form);
            _ReloadForm();
        }

        private void _OpenEditor(CustomerDto? customer = null)
        {
            var form = _serviceProvider.GetRequiredService<frmAddUpdateCustomer>();
            form.LoadData(customer);
            form.ShowDialog();
            _ReloadForm();
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListCustomers_Load(object sender, EventArgs e)
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

            _customersView.RowFilter = "";
            lblRecordsCount.Text = _customersView.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _customersView.RowFilter = "";
            }
            else if (columnName == "Id")
            {
                _customersView.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _customersView.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _customersView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "رقم العميل";

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

        private void dgvCustomers_DoubleClick(object sender, EventArgs e)
        {
            int id = _GetSelectedCustomerId();
            if (id > 0)
                _OpenEditor(_GetSelectedCustomerDto());
        }

        private CustomerDto? _GetSelectedCustomerDto()
        {
            int id = _GetSelectedCustomerId();
            if (id <= 0) return null;

            // Search the DataView rows for the matching Id
            foreach (DataRowView row in _customersView)
            {
                if ((int)row["Id"] == id)
                {
                    return new CustomerDto(
                        id,
                        row["Name"] as string ?? "",
                        row["Phone"] as string,
                        null,
                        null,
                        null,
                        row["CreditLimit"] is decimal d ? d : 0,
                        row["IsActive"] is bool b && b,
                        0
                    );
                }
            }
            return null;
        }

        private void cmsCustomers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasRows = dgvCustomers.Rows.Count > 0 && dgvCustomers.CurrentRow != null;
            cmsCustomers.Enabled = hasRows;
            editToolStripMenuItem.Enabled = hasRows;
            deactivateToolStripMenuItem.Enabled = hasRows;
            reactivateToolStripMenuItem.Enabled = hasRows;

            if (hasRows)
            {
                int id = _GetSelectedCustomerId();
                bool isActive = false;
                foreach (DataRowView row in _customersView)
                {
                    if ((int)row["Id"] == id)
                    {
                        isActive = (bool)row["IsActive"];
                        break;
                    }
                }

                deactivateToolStripMenuItem.Visible = isActive;
                reactivateToolStripMenuItem.Visible = !isActive;
            }
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
            => _OpenEditor(null);

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
            => _OpenEditor(null);

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedCustomerId();
            if (id > 0)
                _OpenEditor(_GetSelectedCustomerDto());
        }

        private void chkShowInactive_CheckedChanged(object sender, EventArgs e)
            => _ReloadForm();

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        private async void deactivateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedCustomerId();
            if (id <= 0) return;

            string name = "";
            foreach (DataRowView row in _customersView)
            {
                if ((int)row["Id"] == id)
                {
                    name = row["Name"] as string ?? "";
                    break;
                }
            }

            if (MessageBox.Show($"هل أنت متأكد من تعطيل العميل {name}؟",
                    "تأكيد التعطيل", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            var result = await _serviceProvider.GetRequiredService<ICustomerApiService>().DeactivateAsync(id);
            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل العميل بنجاح.", "تم التعطيل",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل العميل.\n{result.Error}", "فشل التعطيل",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void reactivateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = _GetSelectedCustomerId();
            if (id <= 0) return;

            string name = "";
            foreach (DataRowView row in _customersView)
            {
                if ((int)row["Id"] == id)
                {
                    name = row["Name"] as string ?? "";
                    break;
                }
            }

            if (MessageBox.Show($"هل أنت متأكد من تنشيط العميل {name}؟",
                    "تأكيد التنشيط", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            var result = await _serviceProvider.GetRequiredService<ICustomerApiService>().ReactivateAsync(id);
            if (result.IsSuccess)
            {
                MessageBox.Show("تم تنشيط العميل بنجاح.", "تم التنشيط",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show($"لم يتم تنشيط العميل.\n{result.Error}", "فشل التنشيط",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
