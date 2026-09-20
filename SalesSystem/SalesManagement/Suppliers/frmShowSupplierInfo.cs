using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.Suppliers
{
    public partial class frmShowSupplierInfo : Form
    {
        int _SupplierID = -1;
        public frmShowSupplierInfo(int SupplierID)
        {
            InitializeComponent();
            _SupplierID = SupplierID;
        }

        private void frmShowSupplierInfo_Load(object sender, EventArgs e)
        {
            ctrlSupplierCard1.LoadSupplierInfoBySupplierID(_SupplierID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
