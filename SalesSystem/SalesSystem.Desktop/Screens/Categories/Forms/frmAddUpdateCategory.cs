using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Categories;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Categories.Forms;

public partial class frmAddUpdateCategory : Form
{
    private readonly IServiceProvider _serviceProvider;
    private CategoryDto? _existingCategory;

    public frmAddUpdateCategory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.RightToLeft = RightToLeft.Yes;
    }

    public void LoadData(CategoryDto? existingCategory)
    {
        _existingCategory = existingCategory;

        if (_existingCategory != null)
        {
            this.Text = "تعديل تصنيف";
            lblMode.Text = "تعديل تصنيف";
            txtName.Text = _existingCategory.Name;
            txtDescription.Text = _existingCategory.Description;
            chkIsActive.Checked = _existingCategory.IsActive;
            chkIsActive.Visible = true;
        }
        else
        {
            this.Text = "إضافة تصنيف جديد";
            txtName.Clear();
            txtDescription.Clear();
            chkIsActive.Visible = false;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم التصنيف مطلوب.");
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
            if (_existingCategory == null)
            {
                var request = new CreateCategoryRequest(txtName.Text, txtDescription.Text);
                var result = await _serviceProvider.GetRequiredService<ICategoryApiService>().CreateAsync(request);
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
                var request = new UpdateCategoryRequest(txtName.Text, txtDescription.Text, chkIsActive.Checked);
                var result = await _serviceProvider.GetRequiredService<ICategoryApiService>().UpdateAsync(_existingCategory.Id, request);
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
}
