using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.People.Forms;
using SalesSystem.Desktop.Services.Interfaces;

namespace SalesSystem.Desktop.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private PersonDto? _Person;
        private int _PersonID = -1;

        public int PersonID => _PersonID;
        public PersonDto? SelectedPersonInfo => _Person;

        public event Action<int>? OnPersonLoaded;

        public ctrlPersonCard()
        {
            _serviceProvider = Program.ServiceProvider!;
            InitializeComponent();
        }

        public ctrlPersonCard(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        public void LoadPersonInfo(int personID)
        {
            _LoadPersonDataAsync(personID);
        }

        public void LoadPersonInfoByNationalNo(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                _ShowWarning("يرجى إدخال الرقم الوطني.");
                return;
            }

            _LoadPersonByNationalNoAsync(nationalNo);
        }

        public void LoadPersonInfoByFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                _ShowWarning("يرجى إدخال اسم الشخص.");
                return;
            }

            _LoadPersonByFullNameAsync(fullName);
        }

        private async void _LoadPersonDataAsync(int personID)
        {
            var result = await _serviceProvider.GetRequiredService<IPersonApiService>().GetByIdAsync(personID);

            if (!result.IsSuccess || result.Value == null)
            {
                ResetPersonInfo();
                _ShowError($"لا يوجد شخص بهذا المعرف في النظام: {personID}");
                return;
            }

            _Person = result.Value;
            _FillPersonInfo();
        }

        private async void _LoadPersonByNationalNoAsync(string nationalNo)
        {
            var result = await _serviceProvider.GetRequiredService<IPersonApiService>().GetByNationalNoAsync(nationalNo);

            if (!result.IsSuccess || result.Value == null)
            {
                ResetPersonInfo();
                _ShowError($"لا يوجد شخص بهذا الرقم الوطني في النظام: {nationalNo}");
                return;
            }

            _Person = result.Value;
            _FillPersonInfo();
        }

        private async void _LoadPersonByFullNameAsync(string fullName)
        {
            var result = await _serviceProvider.GetRequiredService<IPersonApiService>().GetAllAsync(search: fullName);

            if (!result.IsSuccess || result.Value == null || result.Value.Count == 0)
            {
                ResetPersonInfo();
                _ShowError($"لا يوجد شخص بهذا الاسم في النظام: {fullName}");
                return;
            }

            _Person = result.Value[0];
            _FillPersonInfo();
        }

        private void _FillPersonInfo()
        {
            if (_Person == null) return;

            try
            {
                _PersonID = _Person.Id;
                llblEditPersonInfo.Enabled = true;

                txtPersonID.Text = _Person.Id.ToString();
                txtFullName.Text = _Person.FullName;
                txtNationalNo.Text = _Person.NationalNo;
                txtGender.Text = _Person.Gender == 1 ? "ذكر" : "أنثى";
                txtDateOfBirth.Text = clsFormat.DateToShort(_Person.DateOfBirth);
                txtPhone.Text = _GetValueOrDefault(_Person.Phone);
                txtAddress.Text = _GetValueOrDefault(_Person.Address);
                txtEmail.Text = _GetValueOrDefault(_Person.Email);
                txtCountry.Text = _GetValueOrDefault(_Person.CountryName, "غير محدد");

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
            if (_Person == null) return;

            if (_Person.Gender == 1)
                pbPersonImage.Image = SystemIcons.Application.ToBitmap();
            else
                pbPersonImage.Image = SystemIcons.Application.ToBitmap();

            if (string.IsNullOrWhiteSpace(_Person.ImagePath))
                return;

            if (File.Exists(_Person.ImagePath))
                pbPersonImage.ImageLocation = _Person.ImagePath;
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

            pbPersonImage.Image = SystemIcons.Application.ToBitmap();
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

            var frm = Program.ServiceProvider!.GetRequiredService<frmAddUpdatePerson>();
            frm._LoadData(_PersonID);
            clsGlobal._OpenForm(frm);
            LoadPersonInfo(_PersonID);
        }

        private string _GetValueOrDefault(string? value, string defaultValue = "غير محدد")
            => string.IsNullOrWhiteSpace(value) ? defaultValue : value;

        private void _ShowError(string message)
            => MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private void _ShowWarning(string message)
            => MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
