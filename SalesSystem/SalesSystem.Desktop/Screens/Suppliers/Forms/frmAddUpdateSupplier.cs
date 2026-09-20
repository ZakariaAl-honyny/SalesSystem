using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Suppliers.Forms;

public partial class frmAddUpdateSupplier : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISupplierApiService ApiService => _serviceProvider.GetRequiredService<ISupplierApiService>();
    private INotificationService NotificationService => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private SupplierDto? _existingSupplier;

    public frmAddUpdateSupplier(IServiceProvider serviceProvider)
    {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
    }

    public void LoadData(SupplierDto? existingSupplier)
    {
        _existingSupplier = existingSupplier;
        if (_existingSupplier != null)
        {
            this.Text = "تعديل مورد";
            lblMode.Text = "تعديل مورد";
            txtName.Text = _existingSupplier.Name;
            txtPhone.Text = _existingSupplier.Phone;
            txtEmail.Text = _existingSupplier.Email;
            txtAddress.Text = _existingSupplier.Address;
            txtOpeningBalance.Enabled = false;
            chkIsActive.Checked = _existingSupplier.IsActive;
            chkIsActive.Visible = true;
        }
        else
        {
            this.Text = "إضافة مورد جديد";
            chkIsActive.Visible = false;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المورد مطلوب");
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
            if (_existingSupplier == null)
            {
                var request = new CreateSupplierRequest(
                    txtName.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtAddress.Text
                );
                var result = await ApiService.CreateAsync(request);
                if (result.IsSuccess)
                {
                    NotificationService.ShowSuccess("تمت الإضافة بنجاح");
                    EventBus.Publish(new SupplierChangedMessage(result.Value!.Id));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else NotificationService.ShowError(result.Error!);
            }
            else
            {
                var request = new UpdateSupplierRequest(
                    txtName.Text,
                    txtPhone.Text,
                    txtEmail.Text,
                    txtAddress.Text,
                    null,
                    null,
                    0,
                    null,
                    chkIsActive.Checked
                );
                var result = await ApiService.UpdateAsync(_existingSupplier.Id, request);
                if (result.IsSuccess)
                {
                    NotificationService.ShowSuccess("تم التعديل بنجاح");
                    EventBus.Publish(new SupplierChangedMessage(_existingSupplier.Id));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else NotificationService.ShowError(result.Error!);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ بيانات المورد");
            NotificationService.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();
}
