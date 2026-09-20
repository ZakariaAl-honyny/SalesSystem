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

namespace SalesManagement.Suppliers.Controls
{
    public partial class ctrlSupplierCard : UserControl
    {

        clsSupplier _Supplier;
        int _SupplierId = -1;

        public int SupplierID
        {
            get { return _SupplierId; }
        }
        public clsSupplier SelectedSupplierInfo
        {
            get { return _Supplier; }
        }

        public ctrlSupplierCard()
        {
            InitializeComponent();
        }

        public void ResetSupplierInfo()
        {
            _SupplierId = -1;
            ctrlPersonCard1.ResetPersonInfo();
            txtSupplierID.Text = "N/A";
            txtOwnerName.Text = "[????]";
            txtOwnerPhone.Text = "[????]";
            txtManagerName.Text = "[????]";
            txtManagerPhone.Text = "[????]";
        }

        private void _FillSupplierInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_Supplier.PersonID);

            txtSupplierID.Text = _Supplier.SupplierID.ToString();
            txtOwnerName.Text = _Supplier.OwnerName;
            txtOwnerPhone.Text = _Supplier.OwnerPhone;
            txtManagerName.Text = _Supplier.ManagerName;
            txtManagerPhone.Text = _Supplier.ManagerPhone;

        }

        public void LoadSupplierInfoBySupplierID(int SupplierID)
        {
            _SupplierId = SupplierID;
            _Supplier = clsSupplier.Find(SupplierID);

            if (_Supplier == null)
            {
                ResetSupplierInfo();
                MessageBox.Show($"لا يوجد موظف يملك رقم = {SupplierID} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _FillSupplierInfo();

        }

        public void LoadSupplierInfoByFullName(string FullName)
        {
            clsPerson clsPerson = clsPerson.FindByFullName(FullName);

            if (clsPerson == null)
            {
                ResetSupplierInfo();
                MessageBox.Show($"لا يوجد شخص بهذا لأسم = {FullName} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _Supplier = clsSupplier.FindByPersonID(clsPerson.PersonID);

            if (_Supplier == null)
            {
                ResetSupplierInfo();
                MessageBox.Show($"لا يوجد موظف بهذا لأسم = {FullName} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _SupplierId = _Supplier!.SupplierID;
            _FillSupplierInfo();

        }


        private void ctrlSupplierCard_Load(object sender, EventArgs e)
        {
            ResetSupplierInfo();
        }

        private void llEditSupplierInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdateSupplier frm1 = new frmAddUpdateSupplier(_SupplierId);

            frm1.DataBack += DataBackEvent;

            clsGlobal._OpenForm(frm1);
        }


        private void DataBackEvent(object sender, int SupplierID)
        {
            LoadSupplierInfoBySupplierID(SupplierID);
        }

    }
}
