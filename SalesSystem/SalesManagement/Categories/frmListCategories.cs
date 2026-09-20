using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using SalesManagement.Units;
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

namespace SalesManagement.Categories
{
    public partial class frmListCategories : Form
    {

        private static DataView dvCategories;

        public frmListCategories()
        {
            InitializeComponent();
        }

        private int _CategoryID_dgvCategories()
        {
            return (int)dgvCategories.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshCategoriesList()
        //{

        //    //--------------
        //    //using DataView

        //    dvCategories = await clsCategory.GetAllCategories();

        //    dgvCategories.DataSource = dvCategories;
        //    lblRecordsCount.Text = dvCategories.Count.ToString();
        //}

        private void _RefreshCategoriesList()
        {

            //--------------
            //using DataView

            dvCategories = clsCategory.GetAllCategories();

            dgvCategories.DataSource = dvCategories;
            lblRecordsCount.Text = dvCategories.Count.ToString();
        }

        private void frmListCategories_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshCategoriesList();
            _RefreshCategoriesList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvCategories.Rows.Count > 0)
            {
                dgvCategories.Columns[0].HeaderText = "رقم التصنيف";
                dgvCategories.Columns[0].Width = 180;
                //dgvCategories.ColumnHeadersHeight = 50;

                dgvCategories.Columns[1].HeaderText = "اسم التصنيف";
                dgvCategories.Columns[1].Width = 180;

                dgvCategories.Columns[2].HeaderText = "تفاصيل التصنيف";
                dgvCategories.Columns[2].Width = 250;
            }


        }

        private void btnAddNewCategory_Click(object sender, EventArgs e)
        {

            frmAddUpdateCategory frm1 = new frmAddUpdateCategory();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCategories_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم التصنيف":
                    FilterColumn = "CategoryID";
                    break;
                case "اسم التصنيف":
                    FilterColumn = "CategoryName";
                    break;
                case "تفاصيل التصنيف":
                    FilterColumn = "CategoryDescription";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvCategories.RowFilter = "";
                lblRecordsCount.Text = dvCategories.Count.ToString();
                return;
            }
            if (dvCategories.Count != 0)
            {


                if (FilterColumn == "CategoryID")
                    dvCategories.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvCategories.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvCategories.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListCategories_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmShowCategoryInfo frm1 = new frmShowCategoryInfo(_CategoryID_dgvCategories());
            //frm1.ShowDialog();

            frmListCategories_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddUpdateCategory frm1 = new frmAddUpdateCategory(_CategoryID_dgvCategories());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCategories_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddUpdateCategory frm1 = new frmAddUpdateCategory();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListCategories_Load(null!, null!);
        }

        private void DeleteCategory_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا التصنيف؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                //Perform Delele
                if (clsCategory.DeleteCategory(_CategoryID_dgvCategories()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListCategories_Load(null!, null!);
                }

                else
                    MessageBox.Show("لم تم حذف البيانات.", "لم يتم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvAllCategories_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCategories.Rows.Count > 0)
            {

                frmAddUpdateCategory frm1 = new frmAddUpdateCategory(_CategoryID_dgvCategories());
                clsGlobal._OpenForm(frm1);

                //frm1.ShowDialog();

                frmListCategories_Load(null!, null!);
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
            //we allow number incase Category id or Category id are selected.
            if (cbFilterBy.Text == "رقم التصنيف")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsCategories_Opening(object sender, CancelEventArgs e)
        {
            cmsCategories.Enabled = (dgvCategories.Rows.Count > 0);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
