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

namespace SalesManagement.Delegates.Controls
{
    public partial class ctrlDelegateCard : UserControl
    {

        clsDelegate _Delegate;
        int _DelegateId = -1;

        public int DelegateID
        {
            get { return _DelegateId; }
        }
        public clsDelegate SelectedDelegateInfo
        {
            get { return _Delegate; }
        }

        public ctrlDelegateCard()
        {
            InitializeComponent();
        }

        public void ResetDelegateInfo()
        {
            _DelegateId = -1;
            ctrlPersonCard1.ResetPersonInfo();
            txtDelegateID.Text = "N/A";
        }

        private void _FillDelegateInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_Delegate.PersonID);

            txtDelegateID.Text = _Delegate.DelegateID.ToString();

        }

        public void LoadDelegateInfoByDelegateID(int DelegateID)
        {
            _DelegateId = DelegateID;
            _Delegate = clsDelegate.Find(DelegateID);

            if (_Delegate == null)
            {
                ResetDelegateInfo();
                MessageBox.Show($"لا يوجد موظف يملك رقم = {DelegateID} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _FillDelegateInfo();

        }

        public void LoadDelegateInfoByFullName(string FullName)
        {
            clsPerson clsPerson = clsPerson.FindByFullName(FullName);

            if (clsPerson == null)
            {
                ResetDelegateInfo();
                MessageBox.Show($"لا يوجد شخص بهذا لأسم = {FullName} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _Delegate = clsDelegate.FindByPersonID(clsPerson.PersonID);

            if (_Delegate == null)
            {
                ResetDelegateInfo();
                MessageBox.Show($"لا يوجد موظف بهذا لأسم = {FullName} ", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            _DelegateId = _Delegate!.DelegateID;
            _FillDelegateInfo();

        }


        private void ctrlDelegateCard_Load(object sender, EventArgs e)
        {
            ResetDelegateInfo();
        }

        private void llEditDelegateInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdateDelegate frm1 = new frmAddUpdateDelegate(_DelegateId);

            frm1.DataBack += DataBackEvent;

            clsGlobal._OpenForm(frm1);


            //frm1.ShowDialog();
        }


        private void DataBackEvent(object sender, int DelegateID)
        {
            LoadDelegateInfoByDelegateID(DelegateID);
        }

    }
}
