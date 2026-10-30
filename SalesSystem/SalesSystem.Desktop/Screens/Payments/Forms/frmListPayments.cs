using Microsoft.Extensions.DependencyInjection;
using System.Data;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Payments;
using SalesSystem.Desktop.CustomerReceipts.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    public partial class frmListPayments : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _customerTable = new();
        private DataView _customerView = new();
        private DataTable _supplierTable = new();
        private DataView _supplierView = new();

        public frmListPayments(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        private void _SetupCustomerGrid()
        {
            dgvCustomerPayments.Columns.Clear();

            var cols = new[]
            {
                new { Name = "ReceiptNo",     Header = "\u0631\u0642\u0645 \u0627\u0644\u0633\u0646\u062f",   Width = 100 },
                new { Name = "CustomerName",  Header = "\u0627\u0633\u0645 \u0627\u0644\u0639\u0645\u064a\u0644", Width = 200 },
                new { Name = "Amount",        Header = "\u0627\u0644\u0645\u0628\u0644\u063a",              Width = 120 },
                new { Name = "ReceiptDate",   Header = "\u0627\u0644\u062a\u0627\u0631\u064a\u062e",        Width = 110 },
                new { Name = "Notes",         Header = "\u0627\u0644\u0628\u064a\u0627\u0646",              Width = 150 },
            };

            foreach (var c in cols)
            {
                dgvCustomerPayments.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = c.Name,
                    HeaderText = c.Header,
                    DataPropertyName = c.Name,
                    Width = c.Width,
                    ReadOnly = true,
                });
            }
        }

        private void _SetupSupplierGrid()
        {
            dgvSupplierPayments.Columns.Clear();

            var cols = new[]
            {
                new { Name = "PaymentNo",     Header = "\u0631\u0642\u0645 \u0627\u0644\u0633\u0646\u062f",   Width = 100 },
                new { Name = "SupplierName",  Header = "\u0627\u0633\u0645 \u0627\u0644\u0645\u0648\u0631\u062f", Width = 200 },
                new { Name = "Amount",        Header = "\u0627\u0644\u0645\u0628\u0644\u063a",              Width = 120 },
                new { Name = "PaymentDate",   Header = "\u0627\u0644\u062a\u0627\u0631\u064a\u062e",        Width = 110 },
                new { Name = "Notes",         Header = "\u0627\u0644\u0628\u064a\u0627\u0646",              Width = 150 },
            };

            foreach (var c in cols)
            {
                dgvSupplierPayments.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = c.Name,
                    HeaderText = c.Header,
                    DataPropertyName = c.Name,
                    Width = c.Width,
                    ReadOnly = true,
                });
            }
        }

        private async void _RefreshCustomerPayments()
        {
            Cursor = dgvCustomerPayments.Cursor = Cursors.WaitCursor;
            try
            {
                _customerTable = new DataTable();
                _customerTable.Columns.Add("ReceiptNo", typeof(int));
                _customerTable.Columns.Add("CustomerName", typeof(string));
                _customerTable.Columns.Add("Amount", typeof(decimal));
                _customerTable.Columns.Add("ReceiptDate", typeof(string));
                _customerTable.Columns.Add("Notes", typeof(string));

                var result = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var p in result.Value)
                    {
                        _customerTable.Rows.Add(
                            p.ReceiptNo,
                            p.CustomerName ?? "",
                            p.Amount,
                            p.ReceiptDate.ToString("yyyy/MM/dd"),
                            p.Notes ?? "");
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل سندات القبض", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل سندات القبض: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _customerView = new DataView(_customerTable);
                dgvCustomerPayments.DataSource = _customerView;
                lblCustomerCount.Text = _customerView.Count.ToString();
                Cursor = dgvCustomerPayments.Cursor = Cursors.Default;
            }
        }

        private async void _RefreshSupplierPayments()
        {
            Cursor = dgvSupplierPayments.Cursor = Cursors.WaitCursor;
            try
            {
                _supplierTable = new DataTable();
                _supplierTable.Columns.Add("PaymentNo", typeof(string));
                _supplierTable.Columns.Add("SupplierName", typeof(string));
                _supplierTable.Columns.Add("Amount", typeof(decimal));
                _supplierTable.Columns.Add("PaymentDate", typeof(string));
                _supplierTable.Columns.Add("Notes", typeof(string));

                var result = await _serviceProvider.GetRequiredService<ISupplierPaymentApiService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var p in result.Value)
                    {
                        _supplierTable.Rows.Add(
                            p.PaymentNo ?? "",
                            p.SupplierName ?? "",
                            p.Amount,
                            p.PaymentDate.ToString("yyyy/MM/dd"),
                            p.Notes ?? "");
                    }
                }
                else if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل سندات الصرف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل سندات الصرف: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _supplierView = new DataView(_supplierTable);
                dgvSupplierPayments.DataSource = _supplierView;
                lblSupplierCount.Text = _supplierView.Count.ToString();
                Cursor = dgvSupplierPayments.Cursor = Cursors.Default;
            }
        }

        private void _RefreshAll()
        {
            _SetupCustomerGrid();
            _SetupSupplierGrid();
            _RefreshCustomerPayments();
            _RefreshSupplierPayments();
        }

        private void frmListPayments_Load(object sender, EventArgs e)
        {
            _RefreshAll();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbRefreshCustomer_Click(object sender, EventArgs e)
        {
            _RefreshCustomerPayments();
        }

        private void tsbRefreshSupplier_Click(object sender, EventArgs e)
        {
            _RefreshSupplierPayments();
        }

        private void btnAddCustomerPayment_Click(object sender, EventArgs e)
        {
            using var dialog = _serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>();
            dialog.ShowDialog(this);
            _RefreshCustomerPayments();
        }

        private void btnAddSupplierPayment_Click(object sender, EventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<frmSupplierPaymentDialog>();
            dialog.ShowDialog();
            _RefreshSupplierPayments();
        }

        private void cmsCustomerPayments_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasRows = dgvCustomerPayments.Rows.Count > 0 && dgvCustomerPayments.CurrentRow != null;
            tsmiCustomerRefresh.Enabled = true;
            tsmiCustomerEdit.Enabled = hasRows;
            tsmiCustomerAdd.Enabled = true;
            cmsCustomerPayments.Enabled = hasRows;
        }

        private void cmsSupplierPayments_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasRows = dgvSupplierPayments.Rows.Count > 0 && dgvSupplierPayments.CurrentRow != null;
            tsmiSupplierRefresh.Enabled = true;
            tsmiSupplierEdit.Enabled = hasRows;
            tsmiSupplierAdd.Enabled = true;
            cmsSupplierPayments.Enabled = hasRows;
        }

        private void tsmiCustomerRefresh_Click(object sender, EventArgs e)
        {
            _RefreshCustomerPayments();
        }

        private void tsmiCustomerEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomerPayments.CurrentRow == null) return;
            btnAddCustomerPayment_Click(sender, e);
        }

        private void tsmiCustomerAdd_Click(object sender, EventArgs e)
        {
            btnAddCustomerPayment_Click(sender, e);
        }

        private void tsmiSupplierRefresh_Click(object sender, EventArgs e)
        {
            _RefreshSupplierPayments();
        }

        private void tsmiSupplierEdit_Click(object sender, EventArgs e)
        {
            if (dgvSupplierPayments.CurrentRow == null) return;
            btnAddSupplierPayment_Click(sender, e);
        }

        private void tsmiSupplierAdd_Click(object sender, EventArgs e)
        {
            btnAddSupplierPayment_Click(sender, e);
        }
    }
}
