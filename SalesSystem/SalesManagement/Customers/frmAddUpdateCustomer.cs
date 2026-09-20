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

namespace SalesManagement.Customers
{
    public partial class frmAddUpdateCustomer : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int CustomerID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _CustomerID = -1;
        private clsCustomer _Customer;

        public frmAddUpdateCustomer()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateCustomer(int CustomerID)
        {
            InitializeComponent();

            _CustomerID = CustomerID;
            _Mode = enMode.Update;
        }


        private void _ResetDefualtValues()
        {


            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة موظف جديد";
                this.Text = "إضافة موظف جديد";
                _Customer = new clsCustomer();

                tpCustomerInfo.Enabled = false;
                btnSave.Enabled = false;
                btnSaveAndClose.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "تعديل بيانات العميل";
                this.Text = "تعديل بيانات العميل";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                tpCustomerInfo.Enabled = true;
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;

            }


            lblCustomerID.Text = "N/A";
            dtpStartDate.Value = DateTime.Now;
            txtOtherDetails.Clear();

        }

        private void _LoadData()
        {

            _Customer = clsCustomer.Find(_CustomerID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_Customer == null)
            {
                MessageBox.Show($"هذهي الشاشهة سوف يتم أغلاقها أجباريا لأن العميل الذي رقمة المعرف = {_CustomerID} غير موجود.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }

            lblCustomerID.Text = _Customer.CustomerID.ToString();
            dtpStartDate.Value = DateTime.Parse(clsFormat.DateToShort(_Customer.StartDate));
            txtOtherDetails.Text = _Customer.OtherDetails;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_Customer.PersonID);

            //selection CustomerInfo TabPage
            //tcCustomerInfo.SelectedIndex = 1;
        }


        private void frmAddUpdateCustomer_Load(object sender, EventArgs e)
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

            _Customer.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _Customer.StartDate = dtpStartDate.Value;
            _Customer.OtherDetails = txtOtherDetails.Text;

            if (_Customer.Save())
            {
                lblCustomerID.Text = _Customer.CustomerID.ToString();
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل بيانات العميل";
                this.Text = "تعديل بيانات العميل";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";


                MessageBox.Show("تم حفظ البيانات بنجاح", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _CustomerID);

            }
            else
                MessageBox.Show("خطاء لم يتم حفظ البيانات", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);


            //}


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNextCustomerInfo_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;
                tpCustomerInfo.Enabled = true;
                tcCustomerInfo.SelectedTab = tcCustomerInfo.TabPages["tpCustomerInfo"];
                return;
            }

            //incase add new mode
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                if (clsCustomer.isCustomerExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected person already has a Customer account!, choose another one", "Select Another Person",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }

                else
                {
                    btnSave.Enabled = true;
                    btnSaveAndClose.Enabled = true;
                    tpCustomerInfo.Enabled = true;
                    tcCustomerInfo.SelectedTab = tcCustomerInfo.TabPages["tpCustomerInfo"];

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
            tcCustomerInfo.SelectedTab = tcCustomerInfo.TabPages["tpPersonInfo"];
        }

        private void frmAddUpdateCustomer_Activated(object sender, EventArgs e)
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
                tpCustomerInfo.Enabled = false;
            }
        }

        private void cbxCustomerState_SelectedIndexChanged(object sender, EventArgs e)
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

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void frmAddUpdateCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

        }
    }
}
