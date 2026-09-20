using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.Customers
{
    public partial class frmShowCustomerInfo : Form
    {
        int _CustomerID = -1;
        public frmShowCustomerInfo(int CustomerID)
        {
            InitializeComponent();
            _CustomerID = CustomerID;
        }

        private void frmShowCustomerInfo_Load(object sender, EventArgs e)
        {
            ctrlCustomerCard1.LoadCustomerInfoByCustomerID(_CustomerID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
