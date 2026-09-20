using SalesManagement.Busniess;
using SalesManagement.Companies;
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

namespace SalesManagement.Customers.Controls
{
    public partial class ctrlCustomerCard : UserControl
    {

        clsCustomer _Customer;
        int _CustomerId = -1;

        public int CustomerID
        {
            get { return _CustomerId; }
        }
        public clsCustomer SelectedCustomerInfo
        {
            get { return _Customer; }
        }

        public ctrlCustomerCard()
        {
            InitializeComponent();
        }

        public void ResetCustomerInfo()
        {
            _CustomerId = -1;
            ctrlPersonCard1.ResetPersonInfo();
            txtCustomerID.Text = "N/A";
            txtCustomerStartDate.Text = "[????]";
            txtCustomerOtherDetails.Text = "[????]";
        }

        private void _FillCustomerInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_Customer.PersonID);

            txtCustomerID.Text = _Customer.CustomerID.ToString();
            //txtCustomerStartDate.Text = GlobalClasses.clsFormat.DateToShort(_Customer.StartDate);
            //txtCustomerOtherDetails.Text = _Customer.OtherDetails;

        }

        public void LoadCustomerInfoByCustomerID(int CustomerID)
        {
            _CustomerId = CustomerID;
            _Customer = clsCustomer.Find(CustomerID);

            if (_Customer == null)
            {
                ResetCustomerInfo();
                MessageBox.Show($"لا يوجد موظف يملك رقم = {CustomerID} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _FillCustomerInfo();

        }

        public void LoadCustomerInfoByFullName(string FullName)
        {
            clsPerson clsPerson = clsPerson.FindByFullName(FullName);

            if (clsPerson == null)
            {
                ResetCustomerInfo();
                MessageBox.Show($"لا يوجد شخص بهذا لأسم = {FullName} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _Customer = clsCustomer.FindByPersonID(clsPerson.PersonID);

            if (_Customer == null)
            {
                ResetCustomerInfo();
                MessageBox.Show($"لا يوجد موظف بهذا لأسم = {FullName} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _CustomerId = _Customer!.CustomerID;
            _FillCustomerInfo();

        }


        private void ctrlCustomerCard_Load(object sender, EventArgs e)
        {
            ResetCustomerInfo();
        }

        private void llEditCustomerInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdateCustomer frm1 = new frmAddUpdateCustomer(_CustomerId);

            frm1.DataBack += DataBackEvent;

            clsGlobal._OpenForm(frm1);


            //frm1.ShowDialog();
        }


        private void DataBackEvent(object sender, int CustomerID)
        {
            LoadCustomerInfoByCustomerID(CustomerID);
        }

    }
}
