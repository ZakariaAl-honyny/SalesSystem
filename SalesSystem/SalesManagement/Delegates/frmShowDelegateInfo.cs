using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.Delegates
{
    public partial class frmShowDelegateInfo : Form
    {
        int _DelegateID = -1;
        public frmShowDelegateInfo(int DelegateID)
        {
            InitializeComponent();
            _DelegateID = DelegateID;
        }

        private void frmShowDelegateInfo_Load(object sender, EventArgs e)
        {
            ctrlDelegateCard1.LoadDelegateInfoByDelegateID(_DelegateID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
