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

namespace SalesManagement.Delegates
{
    public partial class frmListDelegates : Form
    {

        private static DataView dvDelegates;

        public frmListDelegates()
        {
            InitializeComponent();
        }

        private int _DelegateID_dgvDelegates()
        {
            return (int)dgvDelegates.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshDelegatesList()
        //{

        //    //--------------
        //    //using DataView

        //    dvDelegates = await clsDelegate.GetAllDelegates();

        //    dgvDelegates.DataSource = dvDelegates;
        //    lblRecordsCount.Text = dvDelegates.Count.ToString();
        //}

        private void _RefreshDelegatesList()
        {

            //--------------
            //using DataView

            dvDelegates = clsDelegate.GetAllDelegates();

            dgvDelegates.DataSource = dvDelegates;
            lblRecordsCount.Text = dvDelegates.Count.ToString();
        }

        private void frmListDelegates_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshDelegatesList();
            _RefreshDelegatesList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvDelegates.Rows.Count > 0)
            {
                dgvDelegates.Columns[0].HeaderText = "رقم المندوب";
                dgvDelegates.Columns[0].Width = 180;
                //dgvDelegates.ColumnHeadersHeight = 50;

                dgvDelegates.Columns[1].HeaderText = "اسم المندوب";
                dgvDelegates.Columns[1].Width = 180;

            }


        }

        private void btnAddNewDelegate_Click(object sender, EventArgs e)
        {
            frmAddUpdateDelegate frm1 = new frmAddUpdateDelegate();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListDelegates_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم المندوب":
                    FilterColumn = "DelegateID";
                    break;
                case "اسم المندوب":
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
                dvDelegates.RowFilter = "";
                lblRecordsCount.Text = dvDelegates.Count.ToString();
                return;
            }
            if (dvDelegates.Count != 0)
            {


                if (FilterColumn == "DelegateID")
                    dvDelegates.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvDelegates.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvDelegates.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListDelegates_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowDelegateInfo frm1 = new frmShowDelegateInfo(_DelegateID_dgvDelegates());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListDelegates_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateDelegate frm1 = new frmAddUpdateDelegate(_DelegateID_dgvDelegates());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListDelegates_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateDelegate frm1 = new frmAddUpdateDelegate();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListDelegates_Load(null!, null!);
        }

        private void DeleteDelegate_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا المندوب؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {

                //Perform Delele
                if (clsDelegate.DeleteDelegate(_DelegateID_dgvDelegates()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListDelegates_Load(null!, null!);
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

        private void dgvAllDelegates_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDelegates.Rows.Count > 0)
            {
                frmShowDelegateInfo frm1 = new frmShowDelegateInfo(_DelegateID_dgvDelegates());
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
            //we allow number incase Delegate id or Delegate id are selected.
            if (cbFilterBy.Text == "رقم المندوب")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsDelegates_Opening(object sender, CancelEventArgs e)
        {
            cmsDelegates.Enabled = (dgvDelegates.Rows.Count > 0);
        }

        private void lblMode_Click(object sender, EventArgs e)
        {

        }
    }
}
