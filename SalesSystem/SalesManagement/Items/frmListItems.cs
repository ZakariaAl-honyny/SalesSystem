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

namespace SalesManagement.Items
{
    public partial class frmListItems : Form
    {

        private static DataView dvItems;

        public frmListItems()
        {
            InitializeComponent();
        }

        private int _ItemID_dgvItems()
        {
            return (int)dgvItems.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshItemsList()
        //{

        //    //--------------
        //    //using DataView

        //    dvItems = await clsItem.GetAllItems();

        //    dgvItems.DataSource = dvItems;
        //    lblRecordsCount.Text = dvItems.Count.ToString();
        //}

        private void _RefreshItemsList()
        {

            //--------------
            //using DataView

            dvItems = clsItem.GetAllItems();

            dgvItems.DataSource = dvItems;
            lblRecordsCount.Text = dvItems.Count.ToString();
        }

        private void frmListItems_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshItemsList();
            _RefreshItemsList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvItems.Rows.Count > 0)
            {
                dgvItems.Columns[0].HeaderText = "رقم الصنف";
                dgvItems.Columns[0].Width = 180;
                //dgvItems.ColumnHeadersHeight = 50;

                dgvItems.Columns[1].HeaderText = "اسم الصنف";
                dgvItems.Columns[1].Width = 180;

                dgvItems.Columns[2].HeaderText = "كود الصنف";
                dgvItems.Columns[2].Width = 180;

                dgvItems.Columns[3].HeaderText = "سعر الشراء";
                dgvItems.Columns[3].Width = 180;

                dgvItems.Columns[4].HeaderText = "سعر البيع";
                dgvItems.Columns[4].Width = 180;

                dgvItems.Columns[5].HeaderText = "التصنيف";
                dgvItems.Columns[5].Width = 180;

                dgvItems.Columns[6].HeaderText = "وحدة البيع";
                dgvItems.Columns[6].Width = 180;

                dgvItems.Columns[7].HeaderText = "مكان التخزين";
                dgvItems.Columns[7].Width = 180;

                dgvItems.Columns[8].HeaderText = "الشركة المصنعة";
                dgvItems.Columns[8].Width = 180;
            }


        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateItem frm1 = new frmAddUpdateItem();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListItems_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم الصنف":
                    FilterColumn = "ItemID";
                    break;
                case "اسم الصنف":
                    FilterColumn = "ItemName";
                    break;
                case "كود الصنف":
                    FilterColumn = "ItemCode";
                    break;
                case "سعر الشراء":
                    FilterColumn = "BuyPrice";
                    break;
                case "سعر البيع":
                    FilterColumn = "SalePrice";
                    break;
                case "التصنيف":
                    FilterColumn = "CategoryName";
                    break;
                case "وحدة البيع":
                    FilterColumn = "UnitName";
                    break;
                case "مكان التخزين":
                    FilterColumn = "StoreName";
                    break;
                case "الشركة المصنعة":
                    FilterColumn = "CompanyName";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvItems.RowFilter = "";
                lblRecordsCount.Text = dvItems.Count.ToString();
                return;
            }
            if (dvItems.Count != 0)
            {


                if (FilterColumn == "ItemID" || FilterColumn == "BuyPrice" || FilterColumn == "SalePrice")
                    dvItems.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvItems.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvItems.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListItems_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmShowItemInfo frm1 = new frmShowItemInfo(_ItemID_dgvItems());
            //frm1.ShowDialog();

            frmListItems_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateItem frm1 = new frmAddUpdateItem(_ItemID_dgvItems());
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListItems_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateItem frm1 = new frmAddUpdateItem();
            clsGlobal._OpenForm(frm1);

            //frm1.ShowDialog();

            frmListItems_Load(null!, null!);
        }

        private void DeleteItem_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا الصنف؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                //Perform Delele
                if (clsItem.DeleteItem(_ItemID_dgvItems()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListItems_Load(null!, null!);
                }

                else
                    MessageBox.Show("لم تم حذف البيانات.", "لم يتم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvAllItems_DoubleClick(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count > 0)
            {
                frmAddUpdateItem frm1 = new frmAddUpdateItem(_ItemID_dgvItems());
                clsGlobal._OpenForm(frm1);

                //frm1.ShowDialog();

                frmListItems_Load(null!, null!);
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
            //we allow number incase Item id or Item id are selected.
            if (cbFilterBy.Text == "رقم الصنف" || cbFilterBy.Text == "كود الصنف" || cbFilterBy.Text == "سعر الشراء" || cbFilterBy.Text == "سعر البيع")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsItems_Opening(object sender, CancelEventArgs e)
        {
            cmsItems.Enabled = (dgvItems.Rows.Count > 0);
        }

    }
}
