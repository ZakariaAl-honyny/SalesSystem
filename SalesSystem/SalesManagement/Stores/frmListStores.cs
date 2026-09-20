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

namespace SalesManagement.Stores
{
    public partial class frmListStores : Form
    {

        private static DataView dvStores;

        public frmListStores()
        {
            InitializeComponent();
        }

        private int _StoreID_dgvStores()
        {
            return (int)dgvStores.CurrentRow.Cells[0].Value;
        }

        //private async Task _RefreshStoresList()
        //{

        //    //--------------
        //    //using DataView

        //    dvStores = await clsStore.GetAllStores();

        //    dgvStores.DataSource = dvStores;
        //    lblRecordsCount.Text = dvStores.Count.ToString();
        //}

        private void _RefreshStoresList()
        {

            //--------------
            //using DataView

            dvStores = clsStore.GetAllStores();

            dgvStores.DataSource = dvStores;
            lblRecordsCount.Text = dvStores.Count.ToString();
        }

        private void frmListStores_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshStoresList();
            _RefreshStoresList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");


            if (dgvStores.Rows.Count > 0)
            {
                dgvStores.Columns[0].HeaderText = "رقم المخزن";
                dgvStores.Columns[0].Width = 180;
                //dgvStores.ColumnHeadersHeight = 50;

                dgvStores.Columns[1].HeaderText = "رقم مدير المخزن";
                dgvStores.Columns[1].Width = 180;

                dgvStores.Columns[2].HeaderText = "اسم المخزن";
                dgvStores.Columns[2].Width = 180;

                dgvStores.Columns[3].HeaderText = "عنوان المخزن";
                dgvStores.Columns[3].Width = 180;

                dgvStores.Columns[4].HeaderText = "هاتف المخزن";
                dgvStores.Columns[4].Width = 180;

                dgvStores.Columns[5].HeaderText = "تفاصيل المخزن";
                dgvStores.Columns[5].Width = 180;
            }


        }

        private void btnAddNewStore_Click(object sender, EventArgs e)
        {
            frmAddUpdateStore frm1 = new frmAddUpdateStore();
            clsGlobal._OpenForm(frm1);

            frmListStores_Load(null!, null!);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {


                case "رقم المخزن":
                    FilterColumn = "StoreID";
                    break;
                case "رقم مدير المخزن":
                    FilterColumn = "StoreManagerID";
                    break;
                case "اسم المخزن":
                    FilterColumn = "StoreName";
                    break;
                case "عنوان المخزن":
                    FilterColumn = "StoreAddress";
                    break;
                case "هاتف المخزن":
                    FilterColumn = "StorePhone";
                    break;
                case "تفاصيل المخزن":
                    FilterColumn = "StoreDetails";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                dvStores.RowFilter = "";
                lblRecordsCount.Text = dvStores.Count.ToString();
                return;
            }
            if (dvStores.Count != 0)
            {


                if (FilterColumn == "StoreID")
                    dvStores.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    dvStores.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

                lblRecordsCount.Text = dvStores.Count.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListStores_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmShowStoreInfo frm1 = new frmShowStoreInfo(_StoreID_dgvStores());
            //clsGlobal._OpenForm(frm1);

            frmListStores_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateStore frm1 = new frmAddUpdateStore(_StoreID_dgvStores());
            clsGlobal._OpenForm(frm1);

            frmListStores_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateStore frm1 = new frmAddUpdateStore();
            clsGlobal._OpenForm(frm1);

            frmListStores_Load(null!, null!);
        }

        private void DeleteStore_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("هل أنت متأكد من حذف بيانات هذا المخزن؟", "تأكيد الحذف",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {
                //Perform Delele
                if (clsStore.DeleteStore(_StoreID_dgvStores()))
                {
                    MessageBox.Show("تم حذف البيانات بنجاح", "تم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListStores_Load(null!, null!);
                }

                else
                    MessageBox.Show("لم تم حذف البيانات.", "لم يتم الخذف", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvAllStores_DoubleClick(object sender, EventArgs e)
        {
            if (dgvStores.Rows.Count > 0)
            {
                frmAddUpdateStore frm1 = new frmAddUpdateStore(_StoreID_dgvStores());
                clsGlobal._OpenForm(frm1);

                frmListStores_Load(null!, null!);
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
            //we allow number incase Store id or Store id are selected.
            if (cbFilterBy.Text == "رقم المخزن" || cbFilterBy.Text == "رقم مدير المخزن")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsStores_Opening(object sender, CancelEventArgs e)
        {
            cmsStores.Enabled = (dgvStores.Rows.Count > 0);
        }

    }
}
