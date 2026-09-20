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

namespace SalesManagement.Revenues
{
    public partial class frmAddUpdateRevenue : System.Windows.Forms.Form
    {

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int? UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _RevenueID = -1;
        private clsRevenue _Revenue = null!;

        public frmAddUpdateRevenue()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateRevenue(int RevenueID)
        {
            InitializeComponent();

            _RevenueID = RevenueID;
            _Mode = enMode.Update;
        }

        #region Methods

        private void _ResetDefualtValues()
        {

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة ايراد جديد";
                this.Text = "إضافة ايراد جديد";
                _Revenue = new clsRevenue();

            }
            else
            {
                lblTitle.Text = "تعديل ايراد";
                this.Text = "تعديل ايراد";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                btnSave.Enabled = true;

            }

            txtRevenueID.Text = "0";
            txtRevenueName.Text = "";
            txtRevenueDescription.Text = "";

        }

        private void _LoadData()
        {

            _Revenue = clsRevenue.FindByRevenueID(_RevenueID);

            if (_Revenue == null)
            {
                MessageBox.Show("هذهي الشاشة سوف يتم اغلاقها اجبارين بسبب انه لا يوجد ايراد بهذا الرقم = " + _RevenueID);
                this.Close();

                return;
            }


            txtRevenueID.Text = _Revenue.RevenueID.ToString();
            txtRevenueName.Text = _Revenue.RevenueName;
            txtRevenueDescription.Text = _Revenue.RevenueDescription;

        }

        #endregion

        #region Events
        private void frmAddUpdateRevenue_Load(object sender, EventArgs e)
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


            _Revenue.RevenueID = Convert.ToInt32(txtRevenueID.Text);
            _Revenue.RevenueName = txtRevenueName.Text.Trim().Replace("'", "");
            _Revenue.RevenueDescription = txtRevenueDescription.Text.Trim().Replace("'", "");



            if (_Revenue.Save())
            {
                txtRevenueID.Text = _Revenue.RevenueID.ToString();

                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل ايراد";
                this.Text = "تعديل ايراد";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                MessageBox.Show("تم حفظ بيانات النصنيف بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _RevenueID);

            }
            else
                MessageBox.Show("لم تم حفظ بيانات النصنيف", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void frmAddUpdateRevenue_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

        }
    }
}
