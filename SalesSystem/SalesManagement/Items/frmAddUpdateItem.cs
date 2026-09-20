using Microsoft.Identity.Client;
using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using SalesManagement.Properties;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SalesManagement.Items
{
    public partial class frmAddUpdateItem : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int ItemID);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        int _ItemID = -1;
        clsItem _Item;

        public frmAddUpdateItem()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;

        }

        public frmAddUpdateItem(int ItemID)
        {
            InitializeComponent();

            _ItemID = ItemID;
            _Mode = enMode.Update;

        }

        //void _SetDefaultImage()
        //{
        //    if (pbItemImage.ImageLocation == null)
        //    {
        //        if (rbMale.Checked)
        //            pbItemImage.Image = Resources.Male_512;
        //        else
        //            pbItemImage.Image = Resources.Female_512;
        //    }
        //}

        private void _FillCompaniesIntoComoboBox()
        {
            DataView dtUnits = clsCompany.GetAllCompanies();

            foreach (DataRow row in dtUnits.Table.Rows)
            {
                cbxCompanies.Items.Add(row["CompanyName"]);
            }

        }

        private void _FillCategoriesIntoComoboBox()
        {
            DataView dtUnits = clsCategory.GetAllCategories();

            foreach (DataRow row in dtUnits.Table.Rows)
            {
                cbxCategories.Items.Add(row["CategoryName"]);
            }

        }

        private void _FillStoresIntoComoboBox()
        {
            DataView dtUnits = clsStore.GetAllStores();

            foreach (DataRow row in dtUnits.Table.Rows)
            {
                cbxStores.Items.Add(row["StoreName"]);
            }

        }

        private void _FillUnitsIntoComoboBox()
        {
            DataView dtUnits = clsUnit.GetAllUnits();

            foreach (DataRow row in dtUnits.Table.Rows)
            {
                cbxUnits.Items.Add(row["UnitName"]);
            }

        }

        private void _LoadData()
        {

            _Item = clsItem.Find(_ItemID);

            if (_Item == null)
            {
                MessageBox.Show("No Item with ID = " + _ItemID, "Item Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            //the following code will not be executed if the Item was not found
            lblItemID.Text = _ItemID.ToString();
            //this will select the default value in the combobox.
            cbxCategories.SelectedIndex = cbxCategories.FindString(clsCategory.FindByCategoryID(_Item.CategoryID).CategoryName);
            cbxCompanies.SelectedIndex = cbxCompanies.FindString(clsCompany.Find(_Item.CompanyID).CompanyName);
            cbxStores.SelectedIndex = cbxStores.FindString(clsStore.Find(_Item.StoreID).StoreName);
            cbxUnits.SelectedIndex = cbxUnits.FindString(clsUnit.FindByUnitID(_Item.UnitID).UnitName);
            txtItemName.Text = _Item.ItemName;
            txtMinmumQty.Text = _Item.MinimumQuantity.ToString();
            chbxIsActiveBuy.Checked = _Item.IsActiveBuy;
            chbxIsActiveSale.Checked = _Item.IsActiveSale;
            chbxPreventFraction.Checked = _Item.PreventFraction;
            chbxPreventDiscount.Checked = _Item.PreventDiscount;
            txtBuyPrice.Text = _Item.BuyPrice.ToString();
            txtSalePrice.Text = _Item.SalePrice.ToString();
            txtItemCode.Text = _Item.ItemCode;

            if (_Item.ImagePath != "")
                pbItemImage.ImageLocation = _Item.ImagePath;
            else
                pbItemImage.ImageLocation = null;

            //show remove control is there image 
            llRemoveImage.Visible = (_Item.ImagePath != "");

        }

        private void _ResetDefualtValues()
        {

            _FillCompaniesIntoComoboBox();
            _FillCategoriesIntoComoboBox();
            _FillStoresIntoComoboBox();
            _FillUnitsIntoComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "إضافة  شخص جديد";
                this.Text = "إضافة  شخص جديد";
                _Item = new clsItem();
            }
            else
            {
                lblMode.Text = "تعديل بيانات الصنف";
                this.Text = "تعديل بيانات الصنف";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";
            }


            llRemoveImage.Visible = (pbItemImage.ImageLocation != null);
            //_SetDefaultImage();

            //this will be select the default country to Yemen
            //cbxUnits.SelectedIndex = cbxUnits.FindString(clsUnit.Find("Yemen").CountryName);
            //cbxUnits.SelectedIndex = cbxUnits.FindString(clsUnit.Find("Yemen").CountryName);
            //cbxUnits.SelectedIndex = cbxUnits.FindString(clsUnit.Find("Yemen").CountryName);

            lblItemID.Text = "N/A";
            txtItemName.Clear();
            txtMinmumQty.Text = "0";
            chbxIsActiveBuy.Checked = true;
            chbxIsActiveSale.Checked = true;
            chbxPreventFraction.Checked = true;
            chbxPreventDiscount.Checked = true;
            txtBuyPrice.Text = "0";
            txtSalePrice.Text = "0";
            txtItemCode.Clear();
        }

        private void frmAddUpdateItem_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private bool _HandleItemImage()
        {

            //when image is change
            if (_Item.ImagePath != pbItemImage.ImageLocation)
            {
                //Here we first delete the old image 

                if (_Item.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Item.ImagePath);
                    }
                    catch (IOException ex)
                    {
                        clsGlobal.LoggingErrorEvent($"An error occurred: {ex.Message}");
                    }
                }


                if (pbItemImage.ImageLocation != null)
                {
                    //Here we save the new image in the progect folder after that load NewImagePath into pbItemalImage.ImageLocation
                    string SourceImageFile = pbItemImage.ImageLocation;

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbItemImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"لايمكنني نسخ الصورة الجديدة", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                }

            }

            return true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {

                MessageBox.Show("بعض الحقول المطلوبة فارغة لا يمكنك الاستمرار, قم تأشير على العلامة الحمراء رمعرفة الخطاء", "خطاء البيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;

            }

            //if (MessageBox.Show("Are you sure you want to save Item information?", "Confirm",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{

            if (!_HandleItemImage())
                return;

            int? CompanyID = clsCompany.Find(cbxCompanies.Text).CompanyID;
            int CategoryID = clsCategory.FindByCategoryName(cbxCategories.Text).CategoryID;
            int StoreID = clsStore.Find(cbxStores.Text).StoreID;
            int UnitID = clsUnit.FindByUnitName(cbxUnits.Text).UnitID;
            //_Item.ItemID = Convert.ToInt32(lblItemID.Text);

            _Item.CompanyID = CompanyID;
            _Item.CategoryID = CategoryID;
            _Item.StoreID = StoreID;
            _Item.UnitID = UnitID;
            _Item.ItemCode = txtItemCode.Text.Trim();
            _Item.ItemName = txtItemName.Text.Trim();
            _Item.MinimumQuantity = Convert.ToInt32(txtMinmumQty.Text);
            _Item.IsActiveBuy = chbxIsActiveBuy.Checked;
            _Item.IsActiveSale = chbxIsActiveSale.Checked;
            _Item.PreventFraction = chbxPreventFraction.Checked;
            _Item.PreventDiscount = chbxPreventDiscount.Checked;
            _Item.BuyPrice = Convert.ToInt32(txtBuyPrice.Text);
            _Item.SalePrice = Convert.ToInt32(txtSalePrice.Text);

            if (pbItemImage.ImageLocation != null)
                _Item.ImagePath = pbItemImage.ImageLocation;
            else
                _Item.ImagePath = "";

            if (_Item.Save())
            {
                //change form mode to update
                _Mode = enMode.Update;

                lblMode.Text = "تعديل بيانات الصنف";
                this.Text = "تعديل بيانات الصنف";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";

                lblItemID.Text = _Item.ItemID.ToString();

                MessageBox.Show("تم حفظ البيانات بنجاح", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Item.ItemID);
            }
            else
                MessageBox.Show("خطاء لم يتم حفظ البيانات", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //}

        }


        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            //_SetDefaultImage();
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            //_SetDefaultImage();
        }
        private void llOpenFileDialog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                string selectedFilePath = openFileDialog1.FileName;

                pbItemImage.ImageLocation = selectedFilePath;

                llRemoveImage.Visible = true;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbItemImage.ImageLocation = null;

            //_SetDefaultImage();

            llRemoveImage.Visible = false;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void txtAllowOnlyNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void txtBuyPrice_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtBuyPrice.Text))
            {
                if (!chkStopProfitRate.Checked)
                {
                    float ProfitRate = Convert.ToSingle(nudProfitRate.Text) / 100;
                    float Profit = Convert.ToSingle(txtBuyPrice.Text) * ProfitRate;
                    txtSalePrice.Text = (Convert.ToSingle(txtBuyPrice.Text) + Profit).ToString();
                }
                else
                {
                    txtSalePrice.Text = txtBuyPrice.Text;
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void nudProfitRate_ValueChanged(object sender, EventArgs e)
        {
            txtBuyPrice_TextChanged(null!, null!);
        }

        private void nudProfitRate_MouseDown(object sender, MouseEventArgs e)
        {
            txtBuyPrice_TextChanged(null!, null!);
        }

        private void nudProfitRate_MouseUp(object sender, MouseEventArgs e)
        {
            txtBuyPrice_TextChanged(null!, null!);
        }

        private void chbxStopProfitRate_CheckedChanged(object sender, EventArgs e)
        {
            txtBuyPrice_TextChanged(null!, null!);
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSave_Click(null!, null!);
            btnClose_Click(null!, null!);
        }

        private void frmAddUpdateItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
