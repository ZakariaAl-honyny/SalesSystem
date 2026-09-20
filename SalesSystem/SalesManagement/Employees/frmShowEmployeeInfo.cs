using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.Employees
{
    public partial class frmShowEmployeeInfo : Form
    {
        int _EmployeeID = -1;
        public frmShowEmployeeInfo(int EmployeeID)
        {
            InitializeComponent();
            _EmployeeID = EmployeeID;
        }

        private void frmShowEmployeeInfo_Load(object sender, EventArgs e)
        {
            ctrlEmployeeCard1.LoadEmployeeInfoByEmployeeID(_EmployeeID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
