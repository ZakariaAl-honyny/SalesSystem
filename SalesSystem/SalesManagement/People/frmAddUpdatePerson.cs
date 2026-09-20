using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using SalesManagement.Properties;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SalesManagement.People
{
    public partial class frmAddUpdatePerson : Form
    {
        // Delegate defines the signature of the method that will handle the event
        // (object sender = who fired it, int PersonID = the new/updated person's ID)
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // Event based on the delegate above — fired after a successful save
        // to send the saved PersonID back to the parent/caller form
        public event DataBackEventHandler DataBack;

        // Enum to track whether we are adding a new person or updating an existing one
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private int _PersonID = -1;
        private clsPerson _Person;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            _Mode = enMode.Update;
        }

        private void _SetDefaultImage()
        {
            // Only set the default gender image if no custom image is loaded
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = rbMale.Checked ? Resources.Male_512 : Resources.Female_512;
        }

        private void _FillCountriesIntoComboBox()
        {
            DataView dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Table.Rows)
                cbxCountry.Items.Add(row["CountryName"]);
        }

        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("لا يوجد شخص بهذا المعرف: " + _PersonID, "الشخص غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblPersonID.Text = _PersonID.ToString();
            txtNationalNo.Text = _Person.NationalNo;
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;

            if (_Person.Gender == clsPerson.enGender.Male)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            txtAddress.Text = _Person.Address;
            dtpDateOfBirth.Value = DateTime.Parse(clsFormat.DateToShort(_Person.DateOfBirth));

            pbPersonImage.ImageLocation = !string.IsNullOrWhiteSpace(_Person.ImagePath)
                ? _Person.ImagePath
                : null;

            // Show remove link only if there is an image loaded
            llRemoveImage.Visible = !string.IsNullOrWhiteSpace(_Person.ImagePath);

            // Find and select the person's nationality in the ComboBox
            cbxCountry.SelectedIndex = cbxCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
        }

        private void _ResetDefaultValues()
        {
            _FillCountriesIntoComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "إضافة شخص جديد";
                this.Text = "إضافة شخص جديد";
                _Person = new clsPerson();
            }
            else
            {
                lblMode.Text = "تعديل بيانات الشخص";
                this.Text = "تعديل بيانات الشخص";
                btnSave.Text = "تعديل";
                btnSaveAndClose.Text = "تعديل وإغلاق";
            }

            llRemoveImage.Visible = pbPersonImage.ImageLocation != null;
            _SetDefaultImage();

            // Select Yemen as the default country
            clsCountry country = clsCountry.Find("Yemen");
            if(country != null)
                cbxCountry.SelectedIndex = cbxCountry.FindString(country.CountryName);

            lblPersonID.Text = "N/A";
            txtNationalNo.Clear();
            txtFirstName.Clear();
            txtSecondName.Clear();
            txtThirdName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            rbMale.Checked = true;
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private bool _HandlePersonImage()
        {
            // Only process if the image was actually changed
            if (_Person.ImagePath == pbPersonImage.ImageLocation)
                return true;

            // Delete the old image file if it exists
            if (!string.IsNullOrWhiteSpace(_Person.ImagePath))
            {
                try
                {
                    File.Delete(_Person.ImagePath);
                }
                catch (IOException ex)
                {
                    clsGlobal.LoggingErrorEvent($"Failed to delete old image: {ex.Message}");
                }
            }

            if (pbPersonImage.ImageLocation == null)
                return true;

            // Copy the new image to the project images folder and update the path
            string sourceImageFile = pbPersonImage.ImageLocation;

            if (clsUtil.CopyImageToProjectImagesFolder(ref sourceImageFile))
            {
                pbPersonImage.ImageLocation = sourceImageFile;
                return true;
            }

            MessageBox.Show("لا يمكن نسخ الصورة الجديدة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void _SwitchToUpdateMode()
        {
            _Mode = enMode.Update;
            lblMode.Text = "تعديل بيانات الشخص";
            this.Text = "تعديل بيانات الشخص";
            btnSave.Text = "تعديل";
            btnSaveAndClose.Text = "تعديل وإغلاق";
            lblPersonID.Text = _Person.PersonID.ToString();
        }

        private void _SavePersonData()
        {
            int nationalityCountryID = clsCountry.Find(cbxCountry.Text).CountryID;

            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Gender = rbMale.Checked ? clsPerson.enGender.Male : clsPerson.enGender.Female;
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.NationalityCountryID = nationalityCountryID;
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.ImagePath = pbPersonImage.ImageLocation ?? string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("بعض الحقول المطلوبة فارغة، قم بتأشير العلامة الحمراء لمعرفة الخطأ.",
                    "خطأ - البيانات ناقصة", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
                return;

            _SavePersonData();

            if (_Person.Save())
            {
                _SwitchToUpdateMode();
                MessageBox.Show("تم حفظ البيانات بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Fire the DataBack event to notify the caller form with the saved PersonID
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("لم يتم حفظ البيانات.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSave_Click(null!, null!);
            btnClose_Click(null!, null!);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            _SetDefaultImage();
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            _SetDefaultImage();
        }

        private void llOpenFileDialog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.ImageLocation = openFileDialog1.FileName;
                llRemoveImage.Visible = true;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            _SetDefaultImage();
            llRemoveImage.Visible = false;
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            // Email is optional — skip validation if empty
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, null);
                return;
            }

            if (!clsValidatoin.ValidateEmail(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "صيغة البريد الإلكتروني غير صحيحة.");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }

        // Generic reusable validation method — attached to multiple TextBox Validating events
        private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "هذا الحقل مطلوب.");
            }
            else
            {
                errorProvider1.SetError(textBox, null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "هذا الحقل مطلوب.");
                return;
            }

            // Check for duplicate national number
            // In AddNew mode: always check
            // In Update mode: only check if the number was changed
            bool numberChanged = _Mode == enMode.Update && _Person.NationalNo != txtNationalNo.Text.Trim();
            bool shouldCheck = _Mode == enMode.AddNew || numberChanged;

            if (shouldCheck && clsPerson.isPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "الرقم الوطني محجوز لشخص آخر.");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Block punctuation characters from being typed
            e.Handled = char.IsPunctuation(e.KeyChar);
        }

        private void frmAddUpdatePerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }
    }
}