using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Screens.Settings;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Settings.Forms
{
    public partial class frmCompanySettings : Form
    {
        private readonly IServiceProvider _serviceProvider;

        public frmCompanySettings()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmCompanySettings(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private async void frmCompanySettings_Load(object sender, EventArgs e)
        {
            try
            {
                await _LoadSettings();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل النموذج");
                System.Windows.Forms.MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private async Task _LoadSettings()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<ICompanySettingsApiService>().GetAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    var s = result.Value;
                    txtCompanyName.Text = s.CompanyName ?? "";
                    txtPhone.Text = s.Phone ?? "";
                    txtEmail.Text = s.Email ?? "";
                    txtAddress.Text = s.Address ?? "";
                    txtTaxNumber.Text = s.TaxNumber ?? "";
                    txtLogoPath.Text = s.LogoPath ?? "";
                }
                else
                {
                    MessageBox.Show(
                        "فشل في تحميل إعدادات الشركة: " + (result.Error ?? ""),
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل إعدادات الشركة: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!_ValidateForm())
                return;

            this.Cursor = Cursors.WaitCursor;
            btnSave.Enabled = false;
            try
            {
                var request = new UpdateCompanySettingsRequest(
                    txtCompanyName.Text.Trim(),
                    txtPhone.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtAddress.Text.Trim(),
                    txtTaxNumber.Text.Trim(),
                    txtLogoPath.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<ICompanySettingsApiService>().UpdateAsync(request);

                if (result.IsSuccess)
                {
                    MessageBox.Show(
                        "تم حفظ إعدادات الشركة بنجاح.",
                        "تم الحفظ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        "فشل في حفظ إعدادات الشركة: " + (result.Error ?? ""),
                        "خطأ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ إعدادات الشركة: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                errorProvider.SetError(txtCompanyName, "اسم الشركة مطلوب");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtCompanyName, "");
            }

            if (txtCompanyName.Text.Trim().Length > 200)
            {
                errorProvider.SetError(txtCompanyName, "اسم الشركة لا يمكن أن يتجاوز 200 حرف");
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return isValid;
        }

        private void btnBrowseLogo_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtLogoPath.Text = openFileDialog.FileName;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
