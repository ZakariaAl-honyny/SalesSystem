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

namespace SalesManagement.Users
{
    public partial class frmListUsers : Form
    {

        private static DataView _dvUsers;

        public frmListUsers()
        {
            InitializeComponent();
        }

        private int _UserID_dgvUsers()
        {
            return (int)dgvUsers.CurrentRow.Cells[0].Value;
        }

        private void _RefreshUsersList()
        {

            //--------------
            //using DataView

            _dvUsers = clsUser.GetUsers();

            dgvUsers.DataSource = _dvUsers;
            lblRecordsCount.Text = _dvUsers.Count.ToString();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            //Task _ = _RefreshUsersList();
            _RefreshUsersList();

            //select the default Filter
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");

            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "رقم المستخدم";
                dgvUsers.Columns[0].Width = 100;

                dgvUsers.Columns[1].HeaderText = "رقم الموظف";
                dgvUsers.Columns[1].Width = 100;

                dgvUsers.Columns[2].HeaderText = "الاسم الكامل";
                dgvUsers.Columns[2].Width = 250;

                dgvUsers.Columns[3].HeaderText = "اسم المستخدم";
                dgvUsers.Columns[3].Width = 100;

                dgvUsers.Columns[4].HeaderText = "حالة الحساب";
                dgvUsers.Columns[4].Width = 100;
            }


        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "حالة الحساب")
            {
                //select the default value
                cbIsActive.SelectedIndex = cbIsActive.FindString("الكل");

                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();

            }
            else
            {
                //Reset to default value for getting all Users data
                if (cbIsActive.Text != "الكل")
                {
                    //select the default value
                    cbIsActive.SelectedIndex = cbIsActive.FindString("الكل");
                }

                txtFilterValue.Visible = (cbFilterBy.Text != "لا شيئ");
                cbIsActive.Visible = false;

                txtFilterValue.Clear();
                txtFilterValue.Focus();

            }

        }

        private void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm1 = new frmAddUpdateUser();
            clsGlobal._OpenForm(frm1);

            frmListUsers_Load(null!, null!);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dvUsers == null!)
                return;

            //if (_dvUsers.Count == 0)
            //    return;

            if (cbIsActive.Text == "الكل")
            {
                //Reset Filter
                _dvUsers.RowFilter = "";
                lblRecordsCount.Text = _dvUsers.Count.ToString();
                return;
            }

            if (cbIsActive.Text == "فعال")
                _dvUsers.RowFilter = "IsActive = 1";
            else
                _dvUsers.RowFilter = "IsActive = 0";

            lblRecordsCount.Text = _dvUsers.Count.ToString();

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "لا شيئ";
            //filter by cloumn name as is in the database
            switch (cbFilterBy.Text)
            {
                case "رقم المستخدم":
                    FilterColumn = "UserID";
                    break;
                case "رقم الموظف":
                    FilterColumn = "EmployeeID";
                    break;
                case "اسم المستخدم":
                    FilterColumn = "UserName";
                    break;
                case "الاسم الكامل":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "لا شيئ";
                    break;
            }

            //Reset filter if there is nothing or FilterColumn is لا شيئ
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "لا شيئ")
            {
                _dvUsers.RowFilter = "";
                lblRecordsCount.Text = _dvUsers.Count.ToString();
                return;
            }


            if (_dvUsers.Count != 0)
            {

                if (FilterColumn == "UserID" || FilterColumn == "EmployeeID")
                    _dvUsers.RowFilter = String.Format("{0} = {1}", FilterColumn, txtFilterValue.Text.Trim());
                else
                    _dvUsers.RowFilter = String.Format("{0} LIKE '%{1}%' ", FilterColumn, txtFilterValue.Text.Trim());


                lblRecordsCount.Text = _dvUsers.Count.ToString();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void refresh_toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmListUsers_Load(null!, null!);
        }

        private void showDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUserInfo frm1 = new frmShowUserInfo(_UserID_dgvUsers());
            clsGlobal._OpenForm(frm1);

            frmListUsers_Load(null!, null!);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm1 = new frmAddUpdateUser(_UserID_dgvUsers());
            clsGlobal._OpenForm(frm1);

            frmListUsers_Load(null!, null!);
        }

        private void addNewPeraonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm1 = new frmAddUpdateUser();
            clsGlobal._OpenForm(frm1);

            frmListUsers_Load(null!, null!);
        }

        private void DeleteEmployee_toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete user with id [" +
                _UserID_dgvUsers() + "]", "Confirm Delete",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.OK)

            {

                //Perform Delele
                if (clsUser.DeleteUser(_UserID_dgvUsers()))
                {
                    MessageBox.Show("User Deleted Successfully.", "Deletion Successfull",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListUsers_Load(null!, null!);
                }

                else
                    MessageBox.Show("User does not deleted.", "Deletion Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            if (dgvUsers.Rows.Count > 0)
            {
                frmShowUserInfo frm = new frmShowUserInfo(_UserID_dgvUsers());
                frm.ShowDialog();
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase رقم المستخدم or رقم الموظف are selected.
            if (cbFilterBy.Text == "رقم المستخدم" || cbFilterBy.Text == "رقم الموظف")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            else
                e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void cmsUsers_Opening(object sender, CancelEventArgs e)
        {
            cmsUsers.Enabled = (dgvUsers.Rows.Count > 0);
        }
    }
}
