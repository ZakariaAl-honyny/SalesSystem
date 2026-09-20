using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms
{
    public partial class frmAddUpdateProductCategory : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private CategoryDto? _existingCategory;

        public frmAddUpdateProductCategory()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmAddUpdateProductCategory(IServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        public void LoadData(CategoryDto? existingCategory)
        {
            _existingCategory = existingCategory;

            if (_existingCategory != null)
            {
                this.Text = "تعديل تصنيف منتج";
                lblMode.Text = "تعديل تصنيف منتج";
                txtNameAr.Text = _existingCategory.Name;
                txtNameEn.Text = "";
                txtDescription.Text = _existingCategory.Description ?? "";
                chkIsActive.Checked = _existingCategory.IsActive;
                chkIsActive.Visible = true;
            }
            else
            {
                this.Text = "إضافة تصنيف منتج جديد";
                lblMode.Text = "إضافة تصنيف منتج جديد";
                txtNameAr.Clear();
                txtNameEn.Clear();
                txtDescription.Clear();
                chkIsActive.Visible = false;
            }
        }

        private async void frmProductCategoryEditor_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadParentCategoriesAsync();
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل النموذج");
                System.Windows.Forms.MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private async Task LoadParentCategoriesAsync()
        {
            var result = await _serviceProvider.GetRequiredService<ICategoryApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                var categories = new List<CategoryDto>(result.Value);
                categories.Insert(0, new CategoryDto(0, "-- بدون تصنيف أب --", null, true));

                cmbParentCategory.DisplayMember = "Name";
                cmbParentCategory.ValueMember = "Id";
                cmbParentCategory.DataSource = categories;
            }
        }

        private bool _ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtNameAr.Text))
            {
                errorProvider.SetError(txtNameAr, "اسم التصنيف مطلوب");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtNameAr, "");
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
                string categoryName = txtNameAr.Text.Trim();

                if (_existingCategory == null)
                {
                    var request = new CreateCategoryRequest(categoryName, txtDescription.Text.Trim());
                    var result = await _serviceProvider.GetRequiredService<ICategoryApiService>().CreateAsync(request);
                    if (result.IsSuccess)
                    {
                        _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تمت الإضافة بنجاح");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error ?? "فشلت الإضافة");
                    }
                }
                else
                {
                    var request = new UpdateCategoryRequest(categoryName, txtDescription.Text.Trim(), chkIsActive.Checked);
                    var result = await _serviceProvider.GetRequiredService<ICategoryApiService>().UpdateAsync(_existingCategory.Id, request);
                    if (result.IsSuccess)
                    {
                        _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم التعديل بنجاح");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error ?? "فشل التعديل");
                    }
                }
            }
            catch (Exception)
            {
                _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
