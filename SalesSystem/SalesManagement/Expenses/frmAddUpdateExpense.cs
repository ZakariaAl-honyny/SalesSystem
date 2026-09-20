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

namespace SalesManagement.Expenses
{
    public partial class frmAddUpdateExpense : System.Windows.Forms.Form
    {

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int? UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _ExpenseID = -1;
        private clsExpense _Expense = null!;

        public frmAddUpdateExpense()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateExpense(int ExpenseID)
        {
            InitializeComponent();

            _ExpenseID = ExpenseID;
            _Mode = enMode.Update;
        }

        #region Methods

        private void _ResetDefualtValues()
        {

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة مصروف جديد";
                this.Text = "إضافة مصروف جديد";
                _Expense = new clsExpense();

            }
            else
            {
                lblTitle.Text = "تعديل مصروف";
                this.Text = "تعديل مصروف";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                btnSave.Enabled = true;

            }

            txtExpenseID.Text = "0";
            txtExpenseName.Text = "";
            txtExpenseDescription.Text = "";

        }

        private void _LoadData()
        {

            _Expense = clsExpense.FindByExpenseID(_ExpenseID);

            if (_Expense == null)
            {
                MessageBox.Show("هذهي الشاشة سوف يتم اغلاقها اجبارين بسبب انه لا يوجد مصروف بهذا الرقم = " + _ExpenseID);
                this.Close();

                return;
            }


            txtExpenseID.Text = _Expense.ExpenseID.ToString();
            txtExpenseName.Text = _Expense.ExpenseName;
            txtExpenseDescription.Text = _Expense.ExpenseDescription;

        }

        #endregion

        #region Events
        private void frmAddUpdateExpense_Load(object sender, EventArgs e)
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


            _Expense.ExpenseID = Convert.ToInt32(txtExpenseID.Text);
            _Expense.ExpenseName = txtExpenseName.Text.Trim().Replace("'", "");
            _Expense.ExpenseDescription = txtExpenseDescription.Text.Trim().Replace("'", "");



            if (_Expense.Save())
            {
                txtExpenseID.Text = _Expense.ExpenseID.ToString();

                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل مصروف";
                this.Text = "تعديل مصروف";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                MessageBox.Show("تم حفظ بيانات النصنيف بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _ExpenseID);

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

        private void frmAddUpdateExpense_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

        }
    }
}
