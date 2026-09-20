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

namespace SalesManagement.Suppliers
{
    public partial class frmListSuppliers : Form
    {

        private static DataView dvSuppliers;

        public frmListSuppliers()
        {
            InitializeComponent();
        }

        private int _SupplierID_dgvSuppliers()
        {
            return (int)dgvSuppliers.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshSuppliersList()
        //{

        //    //--------------
        //    //using DataView

        //    dvSuppliers = await clsSupplier.GetAllSuppliers();

        //    dgvSuppliers.DataSource = dvSuppliers;
        //    lblRecordsCount.Text = dvSuppliers.Count.ToString();
        //}

        private void _RefreshSuppliersList()
        {

            //--------------
            //using DataView

            dvSuppliers = clsSupplier.GetAllSuppliers();

            dgvSuppliers.DataSource = dvSuppliers;
            lblRecordsCount.Text = dvSuppliers.Count.ToString();
        }

        private void frmListSuppliers_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshSuppliersList();
            _RefreshSuppliersList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvSuppliers.Rows.Count > 0)
            {
                dgvSuppliers.Columns[0].HeaderText = "رقم المورد";
                dgvSuppliers.Columns[0].Width = 180;
                //dgvSuppliers.ColumnHeadersHeight = 50;

                dgvSuppliers.Columns[1].HeaderText = "اسم المورد";
                dgvSuppliers.Columns[1].Width = 180;

                dgvSuppliers.Columns[2].HeaderText = "اسم مالك المنتج";
                dgvSuppliers.Columns[2].Width = 250;

                dgvSuppliers.Columns[3].HeaderText = "هاتف المالك";
                dgvSuppliers.Columns[3].Width = 180;

                dgvSuppliers.Columns[4].HeaderText = "اسم مدير المنتج";
                dgvSuppliers.Columns[4].Width = 250;

                dgvSuppliers.Columns[5].HeaderText = "هاتف المدير";
                dgvSuppliers.Columns[5].Width = 180;

            }


        }

        private void btnAddNewSupplier_Click(object sender, EventArgs e)
        {
            frmAddUpdateSupplier frm1 = new frmAddUpdateSupplier();
            clsGlobal._OpenForm(frm1);

            frmListSuppliers_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم المورد":
                    FilterColumn = "SupplierID";
                    break;
                case "اسم المورد":
                    FilterColumn = "FullName";
                    break;
                case "اسم مالك المنتج":
                    FilterColumn = "OwnerName";
                    break;
                case "هاتف المالك":
                    FilterColumn = "OwnerPhone";
                    break;
                case "اسم مدير المنتج":
                    FilterColumn = "ManagerName";
                    break;
                case "هاتف المدير":
                    FilterColumn = "ManagerPhone";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvSuppliers.RowFilter = "";
                lblRecordsCount.Text = dvSuppliers.Count.ToString();
                return;
            }
            if (dvSuppliers.Count != 0)
            {


                if (FilterColumn == "SupplierID")
                    dvSuppliers.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvSuppliers.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvSuppliers.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListSuppliers_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowSupplierInfo frm1 = new frmShowSupplierInfo(_SupplierID_dgvSuppliers());
            clsGlobal._OpenForm(frm1);

            frmListSuppliers_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateSupplier frm1 = new frmAddUpdateSupplier(_SupplierID_dgvSuppliers());
            clsGlobal._OpenForm(frm1);

            frmListSuppliers_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateSupplier frm1 = new frmAddUpdateSupplier();
            clsGlobal._OpenForm(frm1);

            frmListSuppliers_Load(null!, null!);
        }

        private void DeleteSupplier_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا المورد؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {

                //Perform Delele
                if (clsSupplier.DeleteSupplier(_SupplierID_dgvSuppliers()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListSuppliers_Load(null!, null!);
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

        private void dgvAllSuppliers_DoubleClick(object sender, EventArgs e)
        {
            if (dgvSuppliers.Rows.Count > 0)
            {
                frmShowSupplierInfo frm = new frmShowSupplierInfo(_SupplierID_dgvSuppliers());
                frm.ShowDialog();
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
            //we allow number incase Supplier id or Supplier id are selected.
            if (cbFilterBy.Text == "رقم المورد")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsSuppliers_Opening(object sender, CancelEventArgs e)
        {
            cmsSuppliers.Enabled = (dgvSuppliers.Rows.Count > 0);
        }

        private void lblMode_Click(object sender, EventArgs e)
        {

        }
    }
}
