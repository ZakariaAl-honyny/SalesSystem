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

namespace SalesManagement.Revenues
{
    public partial class frmListRevenues : Form
    {

        private static DataView dvRevenues;

        public frmListRevenues()
        {
            InitializeComponent();
        }

        private int _RevenueID_dgvRevenues()
        {
            return (int)dgvRevenues.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshRevenuesList()
        //{

        //    //--------------
        //    //using DataView

        //    dvRevenues = await clsRevenue.GetAllRevenues();

        //    dgvRevenues.DataSource = dvRevenues;
        //    lblRecordsCount.Text = dvRevenues.Count.ToString();
        //}

        private void _RefreshRevenuesList()
        {

            //--------------
            //using DataView

            dvRevenues = clsRevenue.GetAllRevenues();

            dgvRevenues.DataSource = dvRevenues;
            lblRecordsCount.Text = dvRevenues.Count.ToString();
        }

        private void frmListRevenues_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshRevenuesList();
            _RefreshRevenuesList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvRevenues.Rows.Count > 0)
            {
                dgvRevenues.Columns[0].HeaderText = "رقم الايراد";
                dgvRevenues.Columns[0].Width = 180;
                //dgvRevenues.ColumnHeadersHeight = 50;

                dgvRevenues.Columns[1].HeaderText = "اسم الايراد";
                dgvRevenues.Columns[1].Width = 180;

                dgvRevenues.Columns[2].HeaderText = "تفاصيل الايراد";
                dgvRevenues.Columns[2].Width = 250;
            }


        }

        private void btnAddNewRevenue_Click(object sender, EventArgs e)
        {
            frmAddUpdateRevenue frm1 = new frmAddUpdateRevenue();
            clsGlobal._OpenForm(frm1);

            frmListRevenues_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم الايراد":
                    FilterColumn = "RevenueID";
                    break;
                case "اسم الايراد":
                    FilterColumn = "RevenueName";
                    break;
                case "تفاصيل الايراد":
                    FilterColumn = "RevenueDescription";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvRevenues.RowFilter = "";
                lblRecordsCount.Text = dvRevenues.Count.ToString();
                return;
            }
            if (dvRevenues.Count != 0)
            {


                if (FilterColumn == "RevenueID")
                    dvRevenues.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvRevenues.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvRevenues.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListRevenues_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmShowRevenueInfo frm1 = new frmShowRevenueInfo(_RevenueID_dgvRevenues());
            //clsGlobal._OpenForm(frm1);

            frmListRevenues_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateRevenue frm1 = new frmAddUpdateRevenue(_RevenueID_dgvRevenues());
            clsGlobal._OpenForm(frm1);

            frmListRevenues_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateRevenue frm1 = new frmAddUpdateRevenue();
            clsGlobal._OpenForm(frm1);

            frmListRevenues_Load(null!, null!);
        }

        private void DeleteRevenue_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا الايراد؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                //Perform Delele
                if (clsRevenue.DeleteRevenue(_RevenueID_dgvRevenues()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListRevenues_Load(null!, null!);
                }

                else
                    MessageBox.Show("لم تم حذف البيانات.", "لم يتم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvAllRevenues_DoubleClick(object sender, EventArgs e)
        {
            if (dgvRevenues.Rows.Count > 0)
            {
                frmAddUpdateRevenue frm1 = new frmAddUpdateRevenue(_RevenueID_dgvRevenues());
                clsGlobal._OpenForm(frm1);

                frmListRevenues_Load(null!, null!);
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
            //we allow number incase Revenue id or Revenue id are selected.
            if (cbFilterBy.Text == "رقم الايراد")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsRevenues_Opening(object sender, CancelEventArgs e)
        {
            cmsRevenues.Enabled = (dgvRevenues.Rows.Count > 0);
        }

    }
}
