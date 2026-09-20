using SalesManagement.Busniess;
using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
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
    public partial class frmChangePassword : Form
    {
        int _UserID = -1;
        clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void _ResetDefualtValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Could not Find User with id = " + _UserID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;

            }
            ctrlUserCard1.LoadUserInfo(_UserID);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {

                MessageBox.Show("بعض الحقول المطلوبة فارغة لا يمكنك الاستمرار, قم تأشير على العلامة الحمراء رمعرفة الخطاء", "خطاء البيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            //if (MessageBox.Show("Are you sure you want to change password?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{

                _User.Password = clsUtil.ComputeHash(txtNewPassword.Text.Trim());

                if (_User.Save())
                {
                    MessageBox.Show("Password Changed Successfully.",
                       "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //refersh password
                    clsGlobal.CurrentUser.Password = txtNewPassword.Text.Trim();

                    _ResetDefualtValues();
                }
                else
                {
                    MessageBox.Show("An Erro Occured, Password did not change.",
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            //}

        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password can't be blank!");
                return;
            }

            if (clsUtil.ComputeHash(txtCurrentPassword.Text) != _User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password does not match your password");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, null);
            }


        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "New password can't be blank!");

            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, null);
            }

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {

            if (txtConfirmPassword.Text != txtNewPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirm Password does not match new password");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, null);
            }

        }

        private void ctrlUserCard1_Load(object sender, EventArgs e)
        {

        }
    }
}
