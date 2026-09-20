using Serilog;
using SalesSystem.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Screens.Units.Forms;
using SalesSystem.Desktop.Screens.Products;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Categories.Forms;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.Screens.Inventory.Forms;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Products.Forms;

public partial class frmAddUpdateProduct : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IProductApiService ProductService => _serviceProvider.GetRequiredService<IProductApiService>();
    private ICategoryApiService CategoryService => _serviceProvider.GetRequiredService<ICategoryApiService>();
    private IUnitApiService UnitService => _serviceProvider.GetRequiredService<IUnitApiService>();
    private IInventoryBatchApiService BatchService => _serviceProvider.GetRequiredService<IInventoryBatchApiService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private INotificationService Notification => _serviceProvider.GetRequiredService<INotificationService>();
    private int? _editId;

    private IDisposable? _catSubscription;
    private IDisposable? _unitSubscription;

    private List<ProductUnitDto> _productUnits = new();

    public frmAddUpdateProduct(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    public void LoadData(int? editId)
    {
        _editId = editId;
        lblMode.Text = _editId.HasValue ? "تعديل منتج" : "إضافة منتج جديد";
        this.Text = _editId.HasValue ? "تعديل منتج" : "إضافة منتج جديد";
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            await LoadCombosAsync();

            if (_editId.HasValue)
            {
                await LoadProductAsync(_editId.Value);
                await LoadProductUnitsAsync(_editId.Value);
                await LoadPricesAsync();
                await LoadBatchesAsync();
            }

            _catSubscription = EventBus.Subscribe<CategoryChangedMessage>(async _ => await LoadCombosAsync());
            _unitSubscription = EventBus.Subscribe<UnitChangedMessage>(async _ => await LoadCombosAsync());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل نموذج المنتج");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadCombosAsync()
    {
        try
        {
            var cats = await CategoryService.GetAllAsync();
            if (cats.IsSuccess)
            {
                var currentVal = cmbCategory.SelectedValue;
                cmbCategory.DataSource = cats.Value;
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "Id";
                if (currentVal != null) cmbCategory.SelectedValue = currentVal;
            }

            var units = await UnitService.GetAllAsync();
            if (units.IsSuccess)
            {
                var currentVal = cmbUnit.SelectedValue;
                cmbUnit.DataSource = units.Value;
                cmbUnit.DisplayMember = "Name";
                cmbUnit.ValueMember = "Id";
                if (currentVal != null) cmbUnit.SelectedValue = currentVal;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل القوائم المنسدلة للمنتج");
            MessageBox.Show("حدث خطأ في تحميل البيانات الأساسية.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadProductAsync(int id)
    {
        try
        {
            var result = await ProductService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                var p = result.Value;
                txtCode.Text = p.Id.ToString();
                txtBarcode.Text = p.Barcode ?? "";
                txtName.Text = p.Name;
                if (cmbCategory.DataSource != null)
                    cmbCategory.SelectedValue = p.CategoryId;
                if (cmbUnit.DataSource != null)
                    cmbUnit.SelectedValue = p.UnitId > 0 ? p.UnitId : (object?)null;
                numReorderLevel.Value = p.ReorderLevel;
                chkTrackExpiry.Checked = p.TrackExpiry;
                txtDescription.Text = p.Description ?? "";
            }
            else
            {
                MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل بيانات المنتج المعرف {ProductId}", id);
            MessageBox.Show("حدث خطأ في تحميل بيانات المنتج.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private async Task LoadProductUnitsAsync(int productId)
    {
        try
        {
            var result = await ProductService.GetProductUnitsAsync(productId);
            if (result.IsSuccess && result.Value != null)
            {
                _productUnits = result.Value.ToList();
                PopulateUnitsGrid();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل وحدات المنتج");
        }
    }

    private void PopulateUnitsGrid()
    {
        if (dgvUnits == null) return;

        var dt = new System.Data.DataTable();
        dt.Columns.Add("UnitName", typeof(string));
        dt.Columns.Add("IsBaseUnit", typeof(string));
        dt.Columns.Add("Factor", typeof(decimal));

        foreach (var u in _productUnits)
        {
            dt.Rows.Add(u.UnitName ?? "", u.IsBaseUnit ? "نعم" : "لا", u.ConversionFactor);
        }

        dgvUnits.DataSource = dt;
    }

    private async Task LoadPricesAsync()
    {
        try
        {
            var allPrices = new List<ProductPriceDto>();
            foreach (var unit in _productUnits)
            {
                var result = await ProductService.GetProductPricesByUnitAsync(unit.Id);
                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var price in result.Value)
                    {
                        allPrices.Add(price);
                    }
                }
            }

            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("UnitName", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("EffectiveFrom", typeof(string));
            dt.Columns.Add("EffectiveTo", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            foreach (var p in allPrices)
            {
                dt.Rows.Add(
                    p.Id,
                    p.ProductUnitName ?? "",
                    p.Price,
                    p.EffectiveFrom.ToString("yyyy-MM-dd"),
                    p.EffectiveTo?.ToString("yyyy-MM-dd") ?? "دائم",
                    p.IsCurrentlyEffective ? "ساري" : "منتهي");
            }

            dgvPrices.DataSource = dt;
            if (dgvPrices.Columns["Id"] is { } idCol)
                idCol.Visible = false;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل أسعار المنتج");
        }
    }

    private async Task LoadBatchesAsync()
    {
        try
        {
            if (!_editId.HasValue) return;

            var result = await BatchService.GetByProductAsync(_editId.Value);
            if (result.IsSuccess && result.Value != null)
            {
                var dt = new System.Data.DataTable();
                dt.Columns.Add("BatchNo", typeof(string));
                dt.Columns.Add("WarehouseName", typeof(string));
                dt.Columns.Add("QuantityRemaining", typeof(decimal));
                dt.Columns.Add("UnitCost", typeof(decimal));
                dt.Columns.Add("ExpiryDate", typeof(string));
                dt.Columns.Add("ExpiryStatus", typeof(string));

                foreach (var b in result.Value)
                {
                    dt.Rows.Add(
                        b.BatchNo,
                        b.WarehouseName ?? "",
                        b.QuantityRemaining,
                        b.UnitCost,
                        b.ExpiryDate?.ToString("yyyy-MM-dd") ?? "—",
                        b.ExpiryStatus);
                }

                dgvBatches.DataSource = dt;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل بيانات المخزون");
        }
    }

    // ── Validation ───────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المنتج مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (cmbCategory.SelectedValue is not int)
        {
            errorProvider.SetError(cmbCategory, "يرجى اختيار التصنيف");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCategory, "");
        }

        if (cmbUnit.SelectedValue is not int)
        {
            errorProvider.SetError(cmbUnit, "يرجى اختيار الوحدة الأساسية");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbUnit, "");
        }

        if (numReorderLevel.Value < 0)
        {
            errorProvider.SetError(numReorderLevel, "حد إعادة الطلب يجب أن يكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numReorderLevel, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── Save ─────────────────────────────────────────────────────────────────

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        if (cmbCategory.SelectedValue is not int categoryId)
            return;

        Result<ProductDto> result;
        try
        {
            Cursor = Cursors.WaitCursor;

            if (_editId.HasValue)
            {
                var updateRequest = new UpdateProductRequest(
                    txtName.Text,
                    categoryId,
                    txtDescription.Text,
                    numReorderLevel.Value,
                    chkTrackExpiry.Checked,
                    null,
                    txtBarcode.Text,
                    true
                );
                result = await ProductService.UpdateAsync(_editId.Value, updateRequest);
            }
            else
            {
                var createRequest = new CreateProductRequest(
                    txtName.Text,
                    categoryId,
                    txtDescription.Text,
                    numReorderLevel.Value,
                    chkTrackExpiry.Checked,
                    null,
                    txtBarcode.Text
                );
                result = await ProductService.CreateAsync(createRequest);
            }

            if (result.IsSuccess)
            {
                Notification.ShowSuccess("تم حفظ المنتج بنجاح");
                EventBus.Publish(new ProductChangedMessage(result.Value!.Id));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                Notification.ShowError(result.Error!);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ المنتج");
            Notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Event Handlers ───────────────────────────────────────────────────────

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        this.Close();
    }

    private void btnAddCategory_Click(object? sender, EventArgs e)
    {
        var dialog = _serviceProvider.GetRequiredService<frmAddUpdateProductCategory>();
        dialog.ShowDialog();
    }

    private void btnAddUnit_Click(object? sender, EventArgs e)
    {
        var dialog = _serviceProvider.GetRequiredService<frmAddUpdateUnit>();
        dialog.ShowDialog();
    }

    private void btnOpenPricesScreen_Click(object? sender, EventArgs e)
    {
        if (!_editId.HasValue)
        {
            Notification.ShowWarning("يرجى حفظ المنتج أولاً قبل إدارة الأسعار.");
            return;
        }

        var dialog = _serviceProvider.GetRequiredService<frmProductPrices>();
        dialog.ShowDialog(this);
        _ = LoadPricesAsync();
        _ = LoadProductUnitsAsync(_editId.Value);
    }

    private void btnOpenUnitsScreen_Click(object? sender, EventArgs e)
    {
        if (!_editId.HasValue)
        {
            Notification.ShowWarning("يرجى حفظ المنتج أولاً قبل إدارة الوحدات.");
            return;
        }

        var dialog = _serviceProvider.GetRequiredService<frmProductUnitsList>();
        dialog.ShowDialog(this);
        _ = LoadProductUnitsAsync(_editId.Value);
    }

    private async void btnRefreshBatches_Click(object? sender, EventArgs e)
    {
        if (_editId.HasValue)
        {
            await LoadBatchesAsync();
        }
    }

    private void btnViewStock_Click(object? sender, EventArgs e)
    {
        if (!_editId.HasValue)
        {
            Notification.ShowWarning("يرجى حفظ المنتج أولاً قبل عرض المخزون.");
            return;
        }

        var dialog = _serviceProvider.GetRequiredService<frmListInventoryBatches>();
        dialog.ShowDialog(this);
        _ = LoadBatchesAsync();
    }

}
