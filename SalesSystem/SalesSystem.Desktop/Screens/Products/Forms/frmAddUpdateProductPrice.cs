using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Products.Forms;

public partial class frmAddUpdateProductPrice : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IProductApiService ProductService => _serviceProvider.GetRequiredService<IProductApiService>();
    private INotificationService Notification => _serviceProvider.GetRequiredService<INotificationService>();

    private int _productId;
    private bool _isEditMode;
    private int _editPriceId;

    public frmAddUpdateProductPrice(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    public void LoadForProduct(int productId, int? editPriceId = null, List<ProductUnitDto>? availableUnits = null)
    {
        _productId = productId;
        _editPriceId = editPriceId ?? 0;
        _isEditMode = editPriceId.HasValue;

        if (_editPriceId > 0)
        {
            lblMode.Text = "تعديل السعر";
            this.Text = "تعديل السعر";
        }
        else
        {
            lblMode.Text = "إضافة سعر جديد";
            this.Text = "إضافة سعر جديد";
            dtpEffectiveFrom.Value = DateTime.Today;
            dtpEffectiveTo.Value = DateTime.Today.AddMonths(1);
        }

        if (availableUnits != null)
        {
            cmbProductUnit.DisplayMember = "UnitName";
            cmbProductUnit.ValueMember = "Id";
            cmbProductUnit.DataSource = new List<ProductUnitDto>(availableUnits);
        }
        else
        {
            _ = LoadUnitsAsync();
        }
    }

    private async Task LoadUnitsAsync()
    {
        try
        {
            var result = await ProductService.GetProductUnitsAsync(_productId);
            if (result.IsSuccess && result.Value != null)
            {
                cmbProductUnit.DisplayMember = "UnitName";
                cmbProductUnit.ValueMember = "Id";
                cmbProductUnit.DataSource = new List<ProductUnitDto>(result.Value);
            }
            else
            {
                Notification.ShowError("تعذر تحميل وحدات المنتج");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل وحدات المنتج لصفحة الأسعار");
            Notification.ShowError("حدث خطأ في تحميل بيانات الوحدات.");
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        this.Close();
    }

    private void chkNoEndDate_CheckedChanged(object? sender, EventArgs e)
    {
        dtpEffectiveTo.Enabled = !chkNoEndDate.Checked;
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (numPrice.Value <= 0)
        {
            errorProvider.SetError(numPrice, "السعر يجب أن يكون أكبر من صفر");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numPrice, "");
        }

        if (cmbProductUnit.SelectedValue == null || (int)cmbProductUnit.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbProductUnit, "يرجى اختيار الوحدة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbProductUnit, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        try
        {
            Cursor = Cursors.WaitCursor;

            var request = new CreateProductPriceRequest(
                ProductUnitId: (int)cmbProductUnit.SelectedValue,
                Price: numPrice.Value,
                EffectiveFrom: dtpEffectiveFrom.Value,
                EffectiveTo: chkNoEndDate.Checked ? null : dtpEffectiveTo.Value
            );

            var result = await ProductService.CreateProductPriceAsync(request);
            if (result.IsSuccess)
            {
                Notification.ShowSuccess("تم إضافة السعر بنجاح");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                Notification.ShowError(result.Error ?? "حدث خطأ أثناء حفظ السعر");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في حفظ سعر المنتج");
            Notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
