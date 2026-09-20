using Serilog;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Warehouses.Forms;

public partial class frmAddUpdateWarehouse : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IWarehouseApiService ApiService => _serviceProvider.GetRequiredService<IWarehouseApiService>();
    private INotificationService NotificationService => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private WarehouseDto? _existingWarehouse;

    public frmAddUpdateWarehouse(IServiceProvider serviceProvider)
    {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
    }

    public void LoadData(WarehouseDto? existingWarehouse)
    {
        _existingWarehouse = existingWarehouse;
        
        if (_existingWarehouse != null)
        {
            this.Text = "تعديل مستودع";
            lblMode.Text = "تعديل مستودع";
            txtName.Text = _existingWarehouse.Name;
            txtLocation.Text = _existingWarehouse.Location;
            chkIsDefault.Checked = _existingWarehouse.IsDefault;
            chkIsActive.Checked = _existingWarehouse.IsActive;
            chkIsActive.Visible = true;
        }
        else
        {
            this.Text = "إضافة مستودع جديد";
            txtName.Clear();
            txtLocation.Clear();
            chkIsDefault.Checked = false;
            chkIsActive.Visible = false;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المستودع مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
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
            Result<WarehouseDto> result;
            if (_existingWarehouse == null)
            {
                var request = new CreateWarehouseRequest(
                    txtName.Text,
                    Phone: null,
                    Address: txtLocation.Text);
                result = await ApiService.CreateAsync(request);
            }
            else
            {
                var request = new UpdateWarehouseRequest(
                    txtName.Text,
                    Phone: _existingWarehouse.Phone,
                    Address: txtLocation.Text,
                    IsActive: chkIsActive.Checked);
                result = await ApiService.UpdateAsync(_existingWarehouse.Id, request);
            }

            if (result.IsSuccess)
            {
                NotificationService.ShowSuccess("تم الحفظ بنجاح");
                EventBus.Publish(new WarehouseChangedMessage(result.Value!.Id));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else 
            {
                NotificationService.ShowError(result.Error ?? "البيانات المدخلة غير صحيحة أو حدث خطأ في الخادم");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ بيانات المستودع");
            NotificationService.ShowError("خطأ في حفظ البيانات. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();
}
