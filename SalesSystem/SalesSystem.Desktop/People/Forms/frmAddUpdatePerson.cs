using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.People.Forms
{
    public partial class frmAddUpdatePerson : Form
    {
        private readonly IServiceProvider _serviceProvider;
        public delegate void DataBackEventHandler(object sender, int personID);
        public event DataBackEventHandler? DataBack;

        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        private int _PersonID = -1;
        private PersonDto? _Person;

        public frmAddUpdatePerson(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int personID, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            _PersonID = personID;
            _Mode = enMode.Update;
        }

        public void _LoadData(int personID)
        {
            _PersonID = personID;
            _Mode = enMode.Update;
        }

        private void _SetDefaultImage()
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = rbMale.Checked ? SystemIcons.Application.ToBitmap() : SystemIcons.Application.ToBitmap();
        }

        private void _FillCountriesIntoComboBox()
        {
            cbxCountry.Items.Clear();
            cbxCountry.Items.Add("اليمن");
            cbxCountry.Items.Add("السعودية");
            cbxCountry.Items.Add("الإمارات");
            cbxCountry.Items.Add("مصر");
            cbxCountry.Items.Add("الأردن");
            cbxCountry.Items.Add("لبنان");
        }

        private async void _LoadData()
        {
            var result = await _serviceProvider.GetRequiredService<IPersonApiService>().GetByIdAsync(_PersonID);

            if (!result.IsSuccess || result.Value == null)
            {
                MessageBox.Show("لا يوجد شخص بهذا المعرف: " + _PersonID, "الشخص غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            _Person = result.Value;
            lblPersonID.Text = _PersonID.ToString();
            txtNationalNo.Text = _Person.NationalNo;
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName ?? "";
            txtLastName.Text = _Person.LastName;

            if (_Person.Gender == 1)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txtEmail.Text = _Person.Email ?? "";
            txtPhone.Text = _Person.Phone ?? "";
            txtAddress.Text = _Person.Address ?? "";
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (!string.IsNullOrWhiteSpace(_Person.ImagePath) && File.Exists(_Person.ImagePath))
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
                llRemoveImage.Visible = true;
            }

            int countryIndex = cbxCountry.FindString(_Person.CountryName ?? "");
            if (countryIndex >= 0)
                cbxCountry.SelectedIndex = countryIndex;
        }

        private void _ResetDefaultValues()
        {
            _FillCountriesIntoComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "إضافة شخص جديد";
                this.Text = "إضافة شخص جديد";
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

            cbxCountry.SelectedIndex = cbxCountry.FindString("اليمن");

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

        private void _SwitchToUpdateMode()
        {
            _Mode = enMode.Update;
            lblMode.Text = "تعديل بيانات الشخص";
            this.Text = "تعديل بيانات الشخص";
            btnSave.Text = "تعديل";
            btnSaveAndClose.Text = "تعديل وإغلاق";
            if (_Person != null)
                lblPersonID.Text = _Person.Id.ToString();
        }

        private async Task<bool> _SavePersonDataAsync()
        {
            byte gender = (byte)(rbMale.Checked ? 1 : 2);

            if (_Mode == enMode.AddNew)
            {
                var request = new CreatePersonRequest(
                    txtFirstName.Text.Trim(),
                    txtSecondName.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtThirdName.Text) ? null : txtThirdName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    txtNationalNo.Text.Trim(),
                    gender,
                    dtpDateOfBirth.Value,
                    string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                    pbPersonImage.ImageLocation,
                    1
                );

                var result = await _serviceProvider.GetRequiredService<IPersonApiService>().CreateAsync(request);

                if (result.IsSuccess && result.Value != null)
                {
                    _Person = result.Value;
                    _PersonID = result.Value.Id;
                    return true;
                }
                else
                {
                    MessageBox.Show(result.Error ?? "لم يتم حفظ البيانات.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                var request = new UpdatePersonRequest(
                    txtFirstName.Text.Trim(),
                    txtSecondName.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtThirdName.Text) ? null : txtThirdName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    txtNationalNo.Text.Trim(),
                    gender,
                    dtpDateOfBirth.Value,
                    string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                    pbPersonImage.ImageLocation,
                    1,
                    true
                );

                var result = await _serviceProvider.GetRequiredService<IPersonApiService>().UpdateAsync(_PersonID, request);

                if (result.IsSuccess && result.Value != null)
                {
                    _Person = result.Value;
                    return true;
                }
                else
                {
                    MessageBox.Show(result.Error ?? "لم يتم حفظ البيانات.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "الرقم الوطني مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errorProvider1.SetError(txtFirstName, "الاسم الأول مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtFirstName, "");
            }

            if (string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                errorProvider1.SetError(txtSecondName, "الاسم الثاني مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtSecondName, "");
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                errorProvider1.SetError(txtLastName, "الاسم الأخير مطلوب.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtLastName, "");
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains(".")))
            {
                errorProvider1.SetError(txtEmail, "صيغة البريد الإلكتروني غير صحيحة.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }

            if (!isValid)
            {
                MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateForm())
                return;

            if (await _SavePersonDataAsync())
            {
                _SwitchToUpdateMode();
                MessageBox.Show("تم حفظ البيانات بنجاح.", "تم الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _PersonID);
            }
        }

        private async void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            if (!_ValidateForm())
                return;

            if (await _SavePersonDataAsync())
            {
                DataBack?.Invoke(this, _PersonID);
                this.Close();
            }
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

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip((Control)sender, "لا يُسمح بإدخال علامات الترقيم.");
            }
        }

        private void frmAddUpdatePerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
