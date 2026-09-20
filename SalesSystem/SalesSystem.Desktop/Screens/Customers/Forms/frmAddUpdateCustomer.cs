using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Screens.Customers;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

public partial class frmAddUpdateCustomer : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ICustomerApiService ApiService => _serviceProvider.GetRequiredService<ICustomerApiService>();
    private INotificationService NotificationService => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private CustomerDto? _existingCustomer;

    public frmAddUpdateCustomer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
    }

    public void LoadData(CustomerDto? existingCustomer)
    {
        _existingCustomer = existingCustomer;
        if (_existingCustomer != null)
        {
            this.Text = "تعديل عميل";
            lblMode.Text = "تعديل عميل";
            txtName.Text = _existingCustomer.Name;
            txtPhone.Text = _existingCustomer.Phone;
            txtEmail.Text = _existingCustomer.Email;
            txtAddress.Text = _existingCustomer.Address;
            txtOpeningBalance.Enabled = false;
            chkIsActive.Checked = _existingCustomer.IsActive;
            chkIsActive.Visible = true;
        }
        else
        {
            this.Text = "إضافة عميل جديد";
            chkIsActive.Visible = false;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم العميل مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (!string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            string email = txtEmail.Text.Trim();
            if (!email.Contains('@') || !email.Contains('.'))
            {
                errorProvider.SetError(txtEmail, "صيغة البريد الإلكتروني غير صحيحة (example@domain.com)");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtEmail, "");
            }
        }
        else
        {
            errorProvider.SetError(txtEmail, "");
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
        if (!_ValidateForm()) return;

        btnSave.Enabled = false;
        try
        {
            if (_existingCustomer == null)
            {
                var request = new CreateCustomerRequest(
                    txtName.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtAddress.Text,
                    null,
                    null,
                    0
                );
                var result = await ApiService.CreateAsync(request);
                if (result.IsSuccess)
                {
                    NotificationService.ShowSuccess("تمت الإضافة بنجاح");
                    EventBus.Publish(new CustomerChangedMessage(result.Value!.Id));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else NotificationService.ShowError(result.Error!);
            }
            else
            {
                var request = new UpdateCustomerRequest(
                    txtName.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtAddress.Text,
                    null,
                    null,
                    _existingCustomer.CreditLimit,
                    chkIsActive.Checked
                );
                var result = await ApiService.UpdateAsync(_existingCustomer.Id, request);
                if (result.IsSuccess)
                {
                    NotificationService.ShowSuccess("تم التعديل بنجاح");
                    EventBus.Publish(new CustomerChangedMessage(_existingCustomer.Id));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else NotificationService.ShowError(result.Error!);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ بيانات العميل");
            NotificationService.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void frmAddUpdateCustomer_Load(object sender, EventArgs e)
    {

    }
}
