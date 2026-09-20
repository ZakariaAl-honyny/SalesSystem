using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement.People
{
    public partial class frmShowPersonInfo : Form
    {
        int _PersonID = -1;
        public frmShowPersonInfo(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            ctrlPersonCard1.LoadPersonInfo(_PersonID);
        }

        public frmShowPersonInfo(string NationalNo)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonInfoByNationalNo(NationalNo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
