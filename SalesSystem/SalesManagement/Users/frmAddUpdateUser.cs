using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using SalesManagement.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.Users
{
    public partial class frmAddUpdateUser : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _UserID = -1;
        private clsUser _User;

        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _Mode = enMode.Update;
        }


        private void _ResetDefualtValues()
        {

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة مستخدم جديد";
                this.Text = "إضافة مستخدم جديد";
                _User = new clsUser();

                tpLoginInfo.Enabled = false;

                ctrlEmployeeCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "تعديل بيانات المستخدم";
                this.Text = "تعديل بيانات المستخدم";

                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;

            }

            lblUserID.Text = "N/A";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;

        }

        private void _LoadData()
        {

            _User = clsUser.FindByUserID(_UserID);
            ctrlEmployeeCardWithFilter1.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show("This form will be closed because No User with ID = " + _UserID);
                this.Close();

                return;
            }

            //the following code will not be executed if the user was not found
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            chkIsActive.Checked = _User.isActive;
            ctrlEmployeeCardWithFilter1.LoadEmployeeInfo(_User.EmployeeID);

            //selection LoginInfo TabPage
            //tcUserInfo.SelectedIndex = 1;
        }


        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {

                MessageBox.Show("بعض الحقول المطلوبة فارغة لا يمكنك الاستمرار, قم تأشير على العلامة الحمراء رمعرفة الخطاء", "خطاء البيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            //if (MessageBox.Show("Are you sure you want to save data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{

            _User.EmployeeID = ctrlEmployeeCardWithFilter1.EmployeeID;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = clsUtil.ComputeHash(txtPassword.Text.Trim());
            _User.isActive = chkIsActive.Checked;

            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                ctrlEmployeeCardWithFilter1.FilterEnabled = false;
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل بيانات المستخدم";
                this.Text = "تعديل بيانات المستخدم";

                MessageBox.Show("User data saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _UserID);

            }
            else
                MessageBox.Show("Error: user Data Is not saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            //}


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNextLoginInfo_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }

            //incase add new mode
            if (ctrlEmployeeCardWithFilter1.EmployeeID != -1)
            {

                if (clsUser.IsUserExistByEmployeeID(ctrlEmployeeCardWithFilter1.EmployeeID))
                {
                    MessageBox.Show("Selected Employee already has a user account!, choose another one", "Select Another Employee", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ctrlEmployeeCardWithFilter1.FilterFocus();
                }

                else
                {
                    btnSave.Enabled = true;
                    btnSaveAndClose.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                    txtUserName.Focus();

                }

            }

            else

            {
                MessageBox.Show("Place select a Employee ", "Not Found Employee",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlEmployeeCardWithFilter1.FilterFocus();

            }

        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                //txtFirstName.Focus();
                errorProvider1.SetError(txtUserName, "username can't be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            }

            if (_Mode == enMode.AddNew)
            {
                if (clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "user name is used  by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
            }
            else
            {
                //incase update UserName 
                if (txtUserName.Text.Trim() != _User.UserName)
                {
                    if (clsUser.isUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "user name is used  by another user");
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    }
                }
            }


        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password can't be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
            ;
        }

        private void btnPrevEmployeeInfo_Click(object sender, EventArgs e)
        {
            tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpEmployeeInfo"];
        }

        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlEmployeeCardWithFilter1.FilterFocus();
        }

        private void ctrlEmployeeCardWithFilter1_OnEmployeeSelected(object sender, clsEmployee e)
        {
            //after find Employee this method call
            //write your code here...
            if (ctrlEmployeeCardWithFilter1.EmployeeID == -1)
            {
                btnSave.Enabled = false;
                btnSaveAndClose.Enabled = false;
                tpLoginInfo.Enabled = false;
            }

        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSave_Click(null!, null!);
            btnClose_Click(null!, null!);
        }

        private void frmAddUpdateUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
