using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Units;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Units.Forms;

public partial class frmAddUpdateUnit : Form
{
    private readonly IServiceProvider _serviceProvider;
    private UnitDto? _existingUnit;

    public frmAddUpdateUnit(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.RightToLeft = RightToLeft.Yes;
    }

    public void LoadData(UnitDto? existingUnit)
    {
        _existingUnit = existingUnit;

        if (_existingUnit != null)
        {
            this.Text = "تعديل وحدة";
            lblMode.Text = "تعديل وحدة";
            txtName.Text = _existingUnit.Name;
            txtSymbol.Text = _existingUnit.Symbol;
            chkIsActive.Checked = _existingUnit.IsActive;
            chkIsActive.Visible = true;
        }
        else
        {
            this.Text = "إضافة وحدة جديدة";
            txtName.Clear();
            txtSymbol.Clear();
            chkIsActive.Visible = false;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الوحدة مطلوب.");
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
            if (_existingUnit == null)
            {
                var request = new CreateUnitRequest(txtName.Text, txtSymbol.Text);
                var result = await _serviceProvider.GetRequiredService<IUnitApiService>().CreateAsync(request);
                if (result.IsSuccess)
                {
                    _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تمت الإضافة بنجاح");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error!);
            }
            else
            {
                var request = new UpdateUnitRequest(txtName.Text, txtSymbol.Text, chkIsActive.Checked);
                var result = await _serviceProvider.GetRequiredService<IUnitApiService>().UpdateAsync(_existingUnit.Id, request);
                if (result.IsSuccess)
                {
                    _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم التعديل بنجاح");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error!);
            }
        }
        catch (Exception ex)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void lblSymbol_Click(object sender, EventArgs e)
    {

    }
}
