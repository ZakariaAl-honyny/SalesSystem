//using SalesManagement.Busniess;
//using SalesManagement.GlobalClasses;
//using SalesManagement.People;
//using SalesManagement.Properties;
//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.IO;
//using System.Windows.Forms;

//namespace SalesManagement.People.Controls
//{
//    public partial class ctrlPersonCard : UserControl
//    {

//        private clsPerson _Person;
//        private int _PersonID = -1;

//        public int PersonID { get { return _PersonID; } }
//        public clsPerson SelectedPersonInfo { get { return _Person; } }

//        public ctrlPersonCard()
//        {
//            InitializeComponent();
//        }

//        public void LoadPersonInfo(int PersonID)
//        {
//            _Person = clsPerson.Find(PersonID);

//            if (_Person == null)
//            {
//                ResetPersonInfo();
//                MessageBox.Show("ا يوجد شخص بهذا المعرف في النظام = " + PersonID.ToString(), "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            _FillPersonInfo();

//        }

//        public void LoadPersonInfoBy(string FullName)
//        {
//            _Person = clsPerson.FindByFullName(FullName);

//            if (_Person == null)
//            {
//                ResetPersonInfo();
//                MessageBox.Show("لا يوجد شخص بهذا الاسم في النظام = " + FullName.ToString(), "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            _FillPersonInfo();

//        }


//        public void LoadPersonInfoByNationalNo(string NationalNo)
//        {
//            _Person = clsPerson.Find(NationalNo);

//            if (_Person == null)
//            {
//                ResetPersonInfo();
//                MessageBox.Show("لا يوجد شخص بهذا الرقم الوطني في النظام = " + NationalNo.ToString(), "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            _FillPersonInfo();

//        }



//        private void _LoadPersonImage()
//        {
//            if (_Person.Gender == clsPerson.enGender.Male)
//            {
//                pbPersonImage.Image = Resources.Male_512;
//            }
//            else
//            {
//                pbPersonImage.Image = Resources.Female_512;
//            }


//            if (_Person.ImagePath != "")
//            {
//                if (File.Exists(_Person.ImagePath))
//                {
//                    pbPersonImage.ImageLocation = _Person.ImagePath;

//                }
//                else
//                    MessageBox.Show("Error: cann't found person image with this path " + _Person.ImagePath,
//                        "Error Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }

//        }

//        private void _FillPersonInfo()
//        {

//            llblEditPersonInfo.Enabled = true;
//            _PersonID = _Person.PersonID;
//            txtPersonID.Text = _Person.PersonID.ToString();
//            txtFullName.Text = _Person.FullName;
//            txtNationalNo.Text = _Person.NationalNo;
//            txtGender.Text = _Person.Gender == clsPerson.enGender.Male ? "ذكر" : "أنثى";
//            txtAddress.Text = _Person.Address;
//            txtDateOfBirth.Text = clsFormat.DateToShort(_Person.DateOfBirth);
//            txtPhone.Text = _Person.Phone;
//            txtCountry.Text = _Person.CountryInfo.CountryName;

//            if (_Person.Email != "")
//                txtEmail.Text = _Person.Email;
//            else
//                txtEmail.Text = "[????]";

//            _LoadPersonImage();

//        }

//        public void ResetPersonInfo()
//        {
//            _PersonID = -1;
//            txtPersonID.Text = "N/A";
//            txtNationalNo.Text = "N/A";
//            txtFullName.Text = "[????]";
//            txtGender.Text = "[????]";
//            txtEmail.Text = "[????]";
//            txtPhone.Text = "[????]";
//            txtDateOfBirth.Text = "[??/???/????]";
//            txtCountry.Text = "[????]";
//            txtAddress.Text = "[????]";
//            pbPersonImage.Image = Resources.Male_512;
//            llblEditPersonInfo.Enabled = false;

//        }

//        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
//        {
//            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
//            clsGlobal._OpenForm(frm);

//            //frm.ShowDialog();

//            //refresh
//            LoadPersonInfo(_PersonID);

//        }
//    }
//}
using SalesManagement.Busniess;
using SalesManagement.GlobalClasses;
using SalesManagement.People;
using SalesManagement.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace SalesManagement.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;
        private int _PersonID = -1;

        public int PersonID => _PersonID;
        public clsPerson SelectedPersonInfo => _Person;

        public event Action<int> OnPersonLoaded;

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int personID)
        {
            _Person = clsPerson.Find(personID);

            if (!_HandlePersonResult(_Person, $"لا يوجد شخص بهذا المعرف في النظام: {personID}"))
                return;

            _FillPersonInfo();
        }

        public void LoadPersonInfoByFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                _ShowWarning("يرجى إدخال اسم الشخص.");
                return;
            }

            _Person = clsPerson.FindByFullName(fullName);

            if (!_HandlePersonResult(_Person, $"لا يوجد شخص بهذا الاسم في النظام: {fullName}"))
                return;

            _FillPersonInfo();
        }

        public void LoadPersonInfoByNationalNo(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                _ShowWarning("يرجى إدخال الرقم الوطني.");
                return;
            }

            _Person = clsPerson.Find(nationalNo);

            if (!_HandlePersonResult(_Person, $"لا يوجد شخص بهذا الرقم الوطني في النظام: {nationalNo}"))
                return;

            _FillPersonInfo();
        }

        private bool _HandlePersonResult(clsPerson person, string errorMessage)
        {
            if (person != null)
                return true;

            ResetPersonInfo();
            _ShowError(errorMessage);
            return false;
        }

        private void _FillPersonInfo()
        {
            try
            {
                _PersonID = _Person.PersonID;
                llblEditPersonInfo.Enabled = true;

                txtPersonID.Text = _Person.PersonID.ToString();
                txtFullName.Text = _Person.FullName;
                txtNationalNo.Text = _Person.NationalNo;
                txtGender.Text = _Person.Gender == clsPerson.enGender.Male ? "ذكر" : "أنثى";
                txtDateOfBirth.Text = clsFormat.DateToShort(_Person.DateOfBirth);
                txtPhone.Text = _GetValueOrDefault(_Person.Phone);
                txtAddress.Text = _GetValueOrDefault(_Person.Address);
                txtEmail.Text = _GetValueOrDefault(_Person.Email);
                txtCountry.Text = _Person.CountryInfo?.CountryName ?? "غير محدد";

                _LoadPersonImage();

                OnPersonLoaded?.Invoke(_PersonID);
            }
            catch (Exception ex)
            {
                _ShowError($"حدث خطأ أثناء تحميل بيانات الشخص:\n{ex.Message}");
            }
        }

        private void _LoadPersonImage()
        {
            pbPersonImage.Image = _Person.Gender == clsPerson.enGender.Male
                ? Resources.Male_512
                : Resources.Female_512;

            if (string.IsNullOrWhiteSpace(_Person.ImagePath))
                return;

            if (File.Exists(_Person.ImagePath))
                pbPersonImage.ImageLocation = _Person.ImagePath;
            else
                _ShowError($"لا يمكن العثور على صورة الشخص في المسار:\n{_Person.ImagePath}");
        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            _Person = null;

            txtPersonID.Text = "N/A";
            txtNationalNo.Text = "N/A";
            txtFullName.Text = "غير محدد";
            txtGender.Text = "غير محدد";
            txtEmail.Text = "غير محدد";
            txtPhone.Text = "غير محدد";
            txtDateOfBirth.Text = "غير محدد";
            txtCountry.Text = "غير محدد";
            txtAddress.Text = "غير محدد";

            pbPersonImage.Image = Resources.Male_512;
            pbPersonImage.ImageLocation = null;

            llblEditPersonInfo.Enabled = false;
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_PersonID <= 0)
            {
                _ShowWarning("يرجى تحميل بيانات شخص أولاً.");
                return;
            }

            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            clsGlobal._OpenForm(frm);
            LoadPersonInfo(_PersonID);
        }

        private string _GetValueOrDefault(string value, string defaultValue = "غير محدد")
            => string.IsNullOrWhiteSpace(value) ? defaultValue : value;

        private void _ShowError(string message)
            => MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void _ShowWarning(string message)
            => MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
