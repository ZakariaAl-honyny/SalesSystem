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

namespace SalesManagement.Delegates
{
    public partial class frmAddUpdateDelegate : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int DelegateID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _DelegateID = -1;
        private clsDelegate _Delegate;

        public frmAddUpdateDelegate()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateDelegate(int DelegateID)
        {
            InitializeComponent();

            _DelegateID = DelegateID;
            _Mode = enMode.Update;
        }


        private void _ResetDefualtValues()
        {


            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة موظف جديد";
                this.Text = "إضافة موظف جديد";
                _Delegate = new clsDelegate();

                tpDelegateInfo.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "تعديل بيانات المندوب";
                this.Text = "تعديل بيانات المندوب";

                tpDelegateInfo.Enabled = true;
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;

            }


            lblDelegateID.Text = "N/A";

        }

        private void _LoadData()
        {

            _Delegate = clsDelegate.Find(_DelegateID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_Delegate == null)
            {
                MessageBox.Show($"هذهي الشاشهة سوف يتم أغلاقها أجباريا لأن المندوب الذي رقمة المعرف = {_DelegateID} غير موجود.", "غير موجود", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }

            lblDelegateID.Text = _Delegate.DelegateID.ToString();
            ctrlPersonCardWithFilter1.LoadPersonInfo(_Delegate.PersonID);

            //selection DelegateInfo TabPage
            //tcDelegateInfo.SelectedIndex = 1;
        }


        private void frmAddUpdateDelegate_Load(object sender, EventArgs e)
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

            _Delegate.PersonID = ctrlPersonCardWithFilter1.PersonID;

            if (_Delegate.Save())
            {
                lblDelegateID.Text = _Delegate.DelegateID.ToString();
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل بيانات المندوب";
                this.Text = "تعديل بيانات المندوب";

                MessageBox.Show("تم حفظ البيانات بنجاح", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _DelegateID);

            }
            else
                MessageBox.Show("خطاء لم يتم حفظ البيانات", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);


            //}


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNextDelegateInfo_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                btnSaveAndClose.Enabled = true;
                tpDelegateInfo.Enabled = true;
                tcDelegateInfo.SelectedTab = tcDelegateInfo.TabPages["tpDelegateInfo"];
                return;
            }

            //incase add new mode
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                if (clsDelegate.isDelegateExistByPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected person already has a Delegate account!, choose another one", "Select Another Person", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }

                else
                {
                    btnSave.Enabled = true;
                    btnSaveAndClose.Enabled = true;
                    tpDelegateInfo.Enabled = true;
                    tcDelegateInfo.SelectedTab = tcDelegateInfo.TabPages["tpDelegateInfo"];

                }

            }

            else

            {
                MessageBox.Show("الرجاء أختير شخص.", "غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }

        }

        private void _ValidateEmptyComboBox(object sender, CancelEventArgs e)
        {
            ComboBox Temp = ((ComboBox)sender);
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
            tcDelegateInfo.SelectedTab = tcDelegateInfo.TabPages["tpPersonInfo"];
        }

        private void frmAddUpdateDelegate_Activated(object sender, EventArgs e)
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
                tpDelegateInfo.Enabled = false;
            }
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSave_Click(null!, null!);
            btnClose_Click(null!, null!);
        }

        private void frmAddUpdateDelegate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

        }
    }
}
