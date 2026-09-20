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

namespace SalesManagement.Categories
{
    public partial class frmAddUpdateCategory : System.Windows.Forms.Form
    {

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int? UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _CategoryID = -1;
        private clsCategory _Category = null!;

        public frmAddUpdateCategory()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateCategory(int CategoryID)
        {
            InitializeComponent();

            _CategoryID = CategoryID;
            _Mode = enMode.Update;
        }

        #region Methods

        private void _ResetDefualtValues()
        {

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة تصنيف جديد";
                this.Text = "إضافة تصنيف جديد";
                _Category = new clsCategory();

            }
            else
            {
                lblTitle.Text = "تعديل تصنيف";
                this.Text = "تعديل تصنيف";
                btnSave.Text = "تعديل";

                btnSave.Enabled = true;

            }

            txtCategoryID.Text = "0";
            txtCategoryName.Text = "";
            txtCategoryDescription.Text = "";

        }

        private void _LoadData()
        {

            _Category = clsCategory.FindByCategoryID(_CategoryID);

            if (_Category == null)
            {
                MessageBox.Show("هذهي الشاشة سوف يتم اغلاقها اجبارين بسبب انه لا يوجد تصنيف بهذا الرقم = " + _CategoryID);
                this.Close();

                return;
            }


            txtCategoryID.Text = _Category.CategoryID.ToString();
            txtCategoryName.Text = _Category.CategoryName;
            txtCategoryDescription.Text = _Category.CategoryDescription;


        }

        #endregion

        #region Events
        private void frmAddUpdateCategory_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }


        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {

                MessageBox.Show("بعض الحقول المطلوبة فارغة لا يمكنك الاستمرار, قم تأشير على العلامة الحمراء رمعرفة الخطاء", "خطاء البيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }


            _Category.CategoryID = Convert.ToInt32(txtCategoryID.Text);
            _Category.CategoryName = txtCategoryName.Text.Trim().Replace("'", "");
            _Category.CategoryDescription = txtCategoryDescription.Text.Trim().Replace("'", "");



            if (_Category.Save())
            {
                txtCategoryID.Text = _Category.CategoryID.ToString();

                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل تصنيف";
                this.Text = "تعديل تصنيف";
                btnSave.Text = "تعديل";

                MessageBox.Show("تم حفظ بيانات النصنيف بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _CategoryID);

            }
            else
                MessageBox.Show("لم تم حفظ بيانات النصنيف", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);





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

        private void frmAddUpdateCategory_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

        }
    }
}
