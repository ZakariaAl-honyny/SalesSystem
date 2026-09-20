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

namespace SalesManagement.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {

        clsUser _User;
        int _UserId = -1;

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void ResetUserInfo()
        {
            _UserId = -1;
            ctrlEmployeeCard1.ResetEmployeeInfo();
            txtUserID.Text = "N/A";
            txtUserName.Text = "[????]";
            txtIsActive.Text = "[????]";
        }

        private void _FillUserInfo()
        {

            ctrlEmployeeCard1.LoadEmployeeInfoByEmployeeID(_User.EmployeeID);

            txtUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtIsActive.Text = (_User.isActive == true ? "فعال" : "غير فعال");

        }

        public void LoadUserInfo(int UserID)
        {
            _UserId = UserID;
            _User = clsUser.FindByUserID(UserID);

            if (_User == null)
            {
                ResetUserInfo();
                MessageBox.Show($"لا يوجد مستخدم يملك رقم = {UserID} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _FillUserInfo();

        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {
            ResetUserInfo();
        }

        private void llEditUserInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdateUser frm1 = new frmAddUpdateUser(_UserId);

            frm1.DataBack += DataBackEvent;

            clsGlobal._OpenForm(frm1);
        }


        private void DataBackEvent(object sender, int UserID)
        {
            LoadUserInfo(UserID);
        }

    }
}
