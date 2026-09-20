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

namespace SalesManagement.Stores
{
    public partial class frmAddUpdateStore : System.Windows.Forms.Form
    {

        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int UserID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _StoreID = -1;
        private clsStore _Store = null!;

        public frmAddUpdateStore()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateStore(int StoreID)
        {
            InitializeComponent();

            _StoreID = StoreID;
            _Mode = enMode.Update;
        }



        #region Methods

        private void _FillStoresIntoComoboBox()
        {
            DataView dtStores = clsStore.GetAllEmployees();

            foreach (DataRow row in dtStores.Table.Rows)
            {
                cbxEmployees.Items.Add(row["FullName"]);
            }

        }

        private void _ResetDefualtValues()
        {
            _FillStoresIntoComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "إضافة مخزن جديد";
                this.Text = "إضافة مخزن جديد";
                _Store = new clsStore();

            }
            else
            {
                lblTitle.Text = "تعديل مخزن";
                this.Text = "تعديل مخزن";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل  وإغلاق";

                btnSave.Enabled = true;

            }

            txtStoreID.Text = "0";
            txtStoreName.Text = "";
            txtStoreDetails.Text = "";


            cbxEmployees.SelectedIndex = cbxEmployees.FindString("أختر أحد الموظفين");

        }

        private void _LoadData()
        {

            _Store = clsStore.Find(_StoreID);

            if (_Store == null)
            {
                MessageBox.Show("هذهي الشاشة سوف يتم اغلاقها اجبارين بسبب انه لا يوجد مخزن بهذا الرقم = " + _StoreID);
                this.Close();

                return;
            }

            txtStoreID.Text = _Store.StoreID.ToString();
            txtStoreName.Text = _Store.StoreName;
            txtStorePhone.Text = _Store.StorePhone;
            txtStoreAddress.Text = _Store.StoreAddress;
            txtStoreDetails.Text = _Store.StoreDetails;


            //this will select the manager in the combobox.
            cbxEmployees.SelectedIndex = cbxEmployees.FindString(clsStore.FindEmployeeFullNameByEmployeeID(_Store.StoreManagerID));

        }

        #endregion

        #region Events
        private void frmAddUpdateStore_Load(object sender, EventArgs e)
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


            int? StoreManagerID = clsStore.FindEmployeeIDByEmployeeFullName(cbxEmployees.Text);

            _Store.StoreID = Convert.ToInt32(txtStoreID.Text);
            _Store.StoreName = txtStoreName.Text.Trim().Replace("'", "");
            _Store.StoreAddress = txtStoreAddress.Text.Trim().Replace("'", "");
            _Store.StorePhone = txtStorePhone.Text.Trim().Replace("'", "");
            _Store.StoreDetails = txtStoreDetails.Text.Trim().Replace("'", "");
            _Store.StoreManagerID = StoreManagerID;


            if (_Store.Save())
            {
                txtStoreID.Text = _Store.StoreID.ToString();

                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "تعديل مخزن";
                this.Text = "تعديل مخزن";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل  وإغلاق";

                MessageBox.Show("تم حفظ بيانات النصنيف بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to caller Form
                DataBack?.Invoke(this, _StoreID);

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

        private void frmAddUpdateStore_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

        }
    }
}
