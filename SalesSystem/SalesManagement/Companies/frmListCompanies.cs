using SalesManagement.Busniess;
using SalesManagement.Categories;
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

namespace SalesManagement.Companies
{
    public partial class frmListCompanies : Form
    {

        private static DataView dvCompanies;

        public frmListCompanies()
        {
            InitializeComponent();
        }

        private int _CompanyID_dgvCompanies()
        {
            return (int)dgvCompanies.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshCompaniesList()
        //{

        //    //--------------
        //    //using DataView

        //    dvCompanies = await clsCompany.GetAllCompanies();

        //    dgvCompanies.DataSource = dvCompanies;
        //    lblRecordsCount.Text = dvCompanies.Count.ToString();
        //}

        private void _RefreshCompaniesList()
        {

            //--------------
            //using DataView

            dvCompanies = clsCompany.GetAllCompanies();

            dgvCompanies.DataSource = dvCompanies;
            lblRecordsCount.Text = dvCompanies.Count.ToString();
        }

        private void frmListCompanies_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshCompaniesList();
            _RefreshCompaniesList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvCompanies.Rows.Count > 0)
            {
                dgvCompanies.Columns[0].HeaderText = "رقم الشركة";
                dgvCompanies.Columns[0].Width = 180;
                //dgvCompanies.ColumnHeadersHeight = 50;

                dgvCompanies.Columns[1].HeaderText = "اسم الشركة";
                dgvCompanies.Columns[1].Width = 180;

                dgvCompanies.Columns[2].HeaderText = "هاتف الشركة";
                dgvCompanies.Columns[2].Width = 180;

                dgvCompanies.Columns[3].HeaderText = "عنوان الشركة";
                dgvCompanies.Columns[3].Width = 180;

                dgvCompanies.Columns[4].HeaderText = "تفاصيل الشركة";
                dgvCompanies.Columns[4].Width = 180;
            }


        }

        private void btnAddNewCompany_Click(object sender, EventArgs e)
        {

            frmAddUpdateCompany frm1 = new frmAddUpdateCompany();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCompanies_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم الشركة":
                    FilterColumn = "CompanyID";
                    break;
                case "اسم الشركة":
                    FilterColumn = "CompanyName";
                    break;
                case "هاتف الشركة":
                    FilterColumn = "CompanyPhone";
                    break;
                case "عنوان الشركة":
                    FilterColumn = "Address";
                    break;
                case "تفاصيل الشركة":
                    FilterColumn = "OtherDetails";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvCompanies.RowFilter = "";
                lblRecordsCount.Text = dvCompanies.Count.ToString();
                return;
            }
            if (dvCompanies.Count != 0)
            {


                if (FilterColumn == "CompanyID")
                    dvCompanies.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvCompanies.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvCompanies.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListCompanies_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmShowCompanyInfo frm1 = new frmShowCompanyInfo(_CompanyID_dgvCompanies());
            //frm1.ShowDialog();

            frmListCompanies_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddUpdateCompany frm1 = new frmAddUpdateCompany(_CompanyID_dgvCompanies());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCompanies_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddUpdateCompany frm1 = new frmAddUpdateCompany();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCompanies_Load(null!, null!);
        }

        private void DeleteCompany_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا الشركة؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                //Perform Delele
                if (clsCompany.DeleteCompany(_CompanyID_dgvCompanies()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListCompanies_Load(null!, null!);
                }

                else
                    MessageBox.Show("لم تم حذف البيانات.", "لم يتم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvAllCompanies_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCompanies.Rows.Count > 0)
            {

                frmAddUpdateCompany frm1 = new frmAddUpdateCompany(_CompanyID_dgvCompanies());
                clsGlobal._OpenForm(frm1);

                //frm1.ShowDialog();

                frmListCompanies_Load(null!, null!);
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
            //we allow number incase Company id or Company id are selected.
            if (cbFilterBy.Text == "رقم الشركة")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsCompanies_Opening(object sender, CancelEventArgs e)
        {
            cmsCompanies.Enabled = (dgvCompanies.Rows.Count > 0);
        }

    }
}
