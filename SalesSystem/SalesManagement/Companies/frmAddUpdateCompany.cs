using Microsoft.VisualBasic.ApplicationServices;
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

namespace SalesManagement.Companies
{
    public partial class frmAddUpdateCompany : System.Windows.Forms.Form
    {

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int? UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _CompanyID = -1;
        private clsCompany _Company = null!;

        public frmAddUpdateCompany()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateCompany(int CompanyID)
        {
            InitializeComponent();

            _CompanyID = CompanyID;
            _Mode = enMode.Update;
        }

        #region Methods

        private void _ResetDefualtValues()
        {

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة شركة جديد";
                this.Text = "إضافة شركة جديد";
                _Company = new clsCompany();

            }
            else
            {
                lblTitle.Text = "تعديل شركة";
                this.Text = "تعديل شركة";
                btnSave.Text = "تعديل";

                btnSave.Enabled = true;

            }

            txtCompanyID.Text = "0";
            txtCompanyName.Text = "";
            txtOtherDetails.Text = "";

        }

        private void _LoadData()
        {

            _Company = clsCompany.Find(_CompanyID);

            if (_Company == null)
            {
                MessageBox.Show("هذهي الشاشة سوف يتم اغلاقها اجبارين بسبب انه لا يوجد شركة بهذا الرقم = " + _CompanyID);
                this.Close();

                return;
            }

            txtCompanyID.Text = _Company.CompanyID.ToString();
            txtCompanyName.Text = _Company.CompanyName;
            txtCompanyPhone.Text = _Company.CompanyPhone;
            txtCompanyAddress.Text = _Company.Address;
            txtOtherDetails.Text = _Company.OtherDetails;

        }

        #endregion

        #region Events
        private void frmAddUpdateCompany_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }


        #endregion



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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {

                MessageBox.Show("بعض الحقول المطلوبة فارغة لا يمكنك الاستمرار, قم تأشير على العلامة الحمراء رمعرفة الخطاء", "خطاء البيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }


            _Company.CompanyID = Convert.ToInt32(txtCompanyID.Text);
            _Company.CompanyName = txtCompanyName.Text.Trim().Replace("'", "");
            _Company.CompanyPhone = txtCompanyPhone.Text.Trim().Replace("'", "");
            _Company.Address = txtCompanyAddress.Text.Trim().Replace("'", "");
            _Company.OtherDetails = txtOtherDetails.Text.Trim().Replace("'", "");



            if (_Company.Save())
            {
                txtCompanyID.Text = _Company.CompanyID.ToString();

                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل شركة";
                this.Text = "تعديل شركة";
                btnSave.Text = "تعديل";

                MessageBox.Show("تم حفظ بيانات بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _CompanyID);

            }
            else
                MessageBox.Show("لم تم حفظ بيانات", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void frmAddUpdateCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

        }
    }
}
