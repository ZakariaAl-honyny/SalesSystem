using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using System.ComponentModel;
//using SalesManagement.UtilityClasses;

namespace SalesManagement
{
    public partial class frmLogin : System.Windows.Forms.Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        #region Events
        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {

                MessageBox.Show("»⁄÷ «·ÕﬁÊ· «·„ÿ·Ê»… ›«—€… ·« Ì„ﬂ‰ﬂ «·«” „—«—, ﬁ„  √‘Ì— ⁄·Ï «·⁄·«„… «·Õ„—«¡ —„⁄—›… «·Œÿ«¡", "Œÿ«¡ «·»Ì«‰«  ‰«ﬁ’…",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            string Username = txtUserName.Text.Trim().Replace("'", "");
            string Password = clsUtil.ComputeHash(txtPassword.Text.Trim().Replace("'", ""));
            clsUser User = clsUser.FindByUserNameAndPassword(Username, Password);

            if (User != null)
            {

                //incase the user is not active
                if (!User.isActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your account is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //clsGlobal.LoggingErrorEvent($"UserID={User.UserID} account is not Active, Contact Admin.");
                    return;
                }

                clsGlobal.CurrentUser = User;
                this.Hide();
                frmMain frm = new frmMain(this);
                GlobalClasses.clsGlobal.frmMdiMain = frm;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("€·ÿ ›Ì √”„ «·„” Œœ„ «Ê ﬂ·„… «·”—", "€Ì— „ÊÃÊœ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //clsGlobal.LoggingWarningEvent("Invalid UserName or Password, Not Found");
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            //this.Close();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
        }


        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                txtPassword.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                btnLogin.Focus();
        }

        private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "Â–« «·Õﬁ· „ÿ·Ê»");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }
        }

        //public void _ValidateEmptyTextBox(string ErrorMessage,object sender, CancelEventArgs e)
        //{
        //    TextBox Temp = ((TextBox)sender);
        //    if (string.IsNullOrEmpty(Temp.Text.Trim()))
        //    {
        //        e.Cancel = true;
        //        errorProvider1.SetError(Temp, "Â–« «·Õﬁ· „ÿ·Ê»");
        //    }
        //    else
        //    {
        //        //e.Cancel = false;
        //        errorProvider1.SetError(Temp, null);
        //    }
        //}

        #endregion

    }
}
