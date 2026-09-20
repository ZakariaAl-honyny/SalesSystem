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
    public partial class frmShowUserInfo : Form
    {
        int _UserID = -1;
        public frmShowUserInfo(int userID)
        {
            InitializeComponent();
            _UserID = userID;
        }

        private void frmShowUserInfo_Load(object sender, EventArgs e)
        {
            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
