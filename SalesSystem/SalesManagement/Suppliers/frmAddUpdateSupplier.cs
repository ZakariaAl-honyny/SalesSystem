using SalesManagement.GlobalClasses;
using SalesManagement.Busniess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SalesManagement.Suppliers
{
    public partial class frmAddUpdateSupplier : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int SupplierID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _SupplierID = -1;
        private clsSupplier _Supplier;

        public frmAddUpdateSupplier()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateSupplier(int SupplierID)
        {
            InitializeComponent();

            _SupplierID = SupplierID;
            _Mode = enMode.Update;
        }


        private void _ResetDefualtValues()
        {


            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة المورد جديد";
                this.Text = "إضافة المورد جديد";
                _Supplier = new clsSupplier();

                tpSupplierInfo.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "تعديل بيانات الالمورد";
                this.Text = "تعديل بيانات الالمورد";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                tpSupplierInfo.Enabled = true;
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;

            }


            lblSupplierID.Text = "N/A";
            txtOwnerName.Clear();
            txtOwnerPhone.Clear();
            txtManagerName.Clear();
            txtManagerPhone.Clear();

        }

        private void _LoadData()
        {

            _Supplier = clsSupplier.Find(_SupplierID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_Supplier == null)
            {
                MessageBox.Show($"هذهي الشاشهة سوف يتم أغلاقها أجباريا لأن الالمورد الذي رقمة المعرف = {_SupplierID} غير موجود.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }

            lblSupplierID.Text = _Supplier.SupplierID.ToString();
            txtOwnerName.Text = _Supplier.OwnerName;
            txtOwnerPhone.Text = _Supplier.OwnerPhone;
            txtManagerName.Text = _Supplier.ManagerName;
            txtManagerPhone.Text = _Supplier.ManagerPhone;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_Supplier.PersonID);

            //selection SupplierInfo TabPage
            //tcSupplierInfo.SelectedIndex = 1;
        }


        private void frmAddUpdateSupplier_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {

                MessageBox.Show("بعض الحقول المطلوبة فارغة لا يمكنك الاستمرار, قم تأشير على العلامة الحمراء رمعرفة الخطاء", "خطاء البيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            //if (MessageBox.Show("Are you sure you want to save data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            _Supplier.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _Supplier.OwnerName = txtOwnerName.Text.Trim();
            _Supplier.OwnerPhone = txtOwnerPhone.Text.Trim();
            _Supplier.ManagerName = txtManagerName.Text.Trim();
            _Supplier.ManagerPhone = txtManagerPhone.Text.Trim();


            if (_Supplier.Save())
            {
                lblSupplierID.Text = _Supplier.SupplierID.ToString();
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل بيانات الالمورد";
                this.Text = "تعديل بيانات الالمورد";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                MessageBox.Show("تم حفظ البيانات بنجاح", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _SupplierID);

            }
            else
                MessageBox.Show("خطاء لم يتم حفظ البيانات", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);


            //}


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNextSupplierInfo_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;
                tpSupplierInfo.Enabled = true;
                tcSupplierInfo.SelectedTab = tcSupplierInfo.TabPages["tpSupplierInfo"];
                return;
            }

            //incase add new mode
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                if (clsSupplier.isSupplierExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected person already has a Supplier account!, choose another one", "Select Another Person",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }

                else
                {
                    btnSave.Enabled = true;
                    btnSaveAndClose.Enabled = true;
                    tpSupplierInfo.Enabled = true;
                    tcSupplierInfo.SelectedTab = tcSupplierInfo.TabPages["tpSupplierInfo"];

                }

            }

            else

            {
                MessageBox.Show("Place select a person ", "Not Found Person",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }

        }

        private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "هذا الحقل مطلوب");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }
        }

        private void btnPrevPersonInfo_Click(object sender, EventArgs e)
        {
            tcSupplierInfo.SelectedTab = tcSupplierInfo.TabPages["tpPersonInfo"];
        }

        private void frmAddUpdateSupplier_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(object sender, clsPerson e)
        {
            //after find person this method call
            //write your code here...
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                btnSave.Enabled = false;
                btnSaveAndClose.Enabled = false;
                tpSupplierInfo.Enabled = false;
            }
        }

        private void cbxSupplierState_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSave_Click(null!, null!);
            btnClose_Click(null!, null!);
        }

        private void frmAddUpdateSupplier_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
