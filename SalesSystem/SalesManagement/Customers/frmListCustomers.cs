using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.Customers
{
    public partial class frmListCustomers : Form
    {

        private static DataView dvCustomers;

        public frmListCustomers()
        {
            InitializeComponent();
        }

        private int _CustomerID_dgvCustomers()
        {
            return (int)dgvCustomers.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshCustomersList()
        //{

        //    //--------------
        //    //using DataView

        //    dvCustomers = await clsCustomer.GetAllCustomers();

        //    dgvCustomers.DataSource = dvCustomers;
        //    lblRecordsCount.Text = dvCustomers.Count.ToString();
        //}

        private void _RefreshCustomersList()
        {

            //--------------
            //using DataView

            dvCustomers = clsCustomer.GetAllCustomers();

            dgvCustomers.DataSource = dvCustomers;
            lblRecordsCount.Text = dvCustomers.Count.ToString();
        }

        private void frmListCustomers_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshCustomersList();
            _RefreshCustomersList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvCustomers.Rows.Count > 0)
            {
                dgvCustomers.Columns[0].HeaderText = "رقم العميل";
                dgvCustomers.Columns[0].Width = 180;
                //dgvCustomers.ColumnHeadersHeight = 50;

                dgvCustomers.Columns[1].HeaderText = "اسم العميل";
                dgvCustomers.Columns[1].Width = 180;

                dgvCustomers.Columns[2].HeaderText = "تاريخ الحساب";
                dgvCustomers.Columns[2].Width = 250;

                dgvCustomers.Columns[3].HeaderText = "تفاصيل العميل";
                dgvCustomers.Columns[3].Width = 180;

            }


        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            frmAddUpdateCustomer frm1 = new frmAddUpdateCustomer();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCustomers_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم العميل":
                    FilterColumn = "CustomerID";
                    break;
                case "اسم العميل":
                    FilterColumn = "FullName";
                    break;
                case "تاريخ الحساب":
                    FilterColumn = "StartDate";
                    break;
                case "تفاصيل العميل":
                    FilterColumn = "OtherDetails";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvCustomers.RowFilter = "";
                lblRecordsCount.Text = dvCustomers.Count.ToString();
                return;
            }
            if (dvCustomers.Count != 0)
            {


                if (FilterColumn == "CustomerID")
                    dvCustomers.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvCustomers.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvCustomers.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListCustomers_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowCustomerInfo frm1 = new frmShowCustomerInfo(_CustomerID_dgvCustomers());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCustomers_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateCustomer frm1 = new frmAddUpdateCustomer(_CustomerID_dgvCustomers());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCustomers_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateCustomer frm1 = new frmAddUpdateCustomer();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCustomers_Load(null!, null!);
        }

        private void DeleteCustomer_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا العميل؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {

                //Perform Delele
                if (clsCustomer.DeleteCustomer(_CustomerID_dgvCustomers()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListCustomers_Load(null!, null!);
                }

                else
                    MessageBox.Show("لم تم حذف البيانات.", "لم يتم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Send email will be in the future",
                "will be soon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void callPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Call phone will be in the future",
                "will be soon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgvAllCustomers_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomers.Rows.Count > 0)
            {
                frmShowCustomerInfo frm1 = new frmShowCustomerInfo(_CustomerID_dgvCustomers());
                clsGlobal._OpenForm(frm1);

                //frm1.ShowDialog();
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "لا شيئ");
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase Customer id or Customer id are selected.
            if (cbFilterBy.Text == "رقم العميل" || cbFilterBy.Text == "اسم العميل")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsCustomers_Opening(object sender, CancelEventArgs e)
        {
            cmsCustomers.Enabled = (dgvCustomers.Rows.Count > 0);
        }

        private void lblMode_Click(object sender, EventArgs e)
        {

        }
    }
}
