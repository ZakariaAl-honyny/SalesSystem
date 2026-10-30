using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Screens.Categories.Forms;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Products.Forms;

/// <summary>
/// النموذج المرجعي (Master Form) لدليل الأصناف.
/// يجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// ويعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmProductMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IProductApiService ProductService => _serviceProvider.GetRequiredService<IProductApiService>();
    private ICategoryApiService CategoryService => _serviceProvider.GetRequiredService<ICategoryApiService>();
    private IUnitApiService UnitService => _serviceProvider.GetRequiredService<IUnitApiService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, ProductDto> _productsById = new();
    private DataTable _productsTable = new();
    private DataView _productsView = new();

    /// <summary>قائمة الوحدات المتاحة لاختيار الوحدة الرئيسية عند إضافة صنف جديد.</summary>
    private List<UnitDto> _unitLookup = new();

    /// <summary>null = إضافة صنف جديد، &gt; 0 = تعديل صنف قائم.</summary>
    private int? _editId;

    /// <summary>يمنع إعادة تحميل سعر الوحدة أثناء تعبئة النموذج برمجياً.</summary>
    private bool _isBindingMainUnit;

    public frmProductMaster(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmProductMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnManageUnits.Click += btnManageUnits_Click;
        btnAddCategory.Click += btnAddCategory_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvProducts.CellClick += dgvProducts_CellClick;
        cmbMainUnit.SelectedIndexChanged += cmbMainUnit_SelectedIndexChanged;
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmProductMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCategoriesAsync();
            await _LoadUnitLookupAsync();
            await _LoadProductsAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الأصناف");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Lookups ───────────────────────────────────────────────────────────────

    private async Task _LoadCategoriesAsync()
    {
        try
        {
            var selected = cmbCategory.SelectedValue as int?;

            var result = await CategoryService.GetAllAsync(includeInactive: true);
            if (result.IsSuccess && result.Value != null)
            {
                var categories = result.Value.ToList();
                categories.Insert(0, new CategoryDto(0, "-- اختر المجموعة --", null, true));

                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "Id";
                cmbCategory.DataSource = categories;

                if (selected is int id && id > 0)
                    _SelectCategory(id);
                else
                    cmbCategory.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل مجموعات الأصناف.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل مجموعات الأصناف");
            MessageBox.Show("تعذر تحميل مجموعات الأصناف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>تحميل قائمة الوحدات لاختيار الوحدة الرئيسية عند إضافة صنف جديد.</summary>
    private async Task _LoadUnitLookupAsync()
    {
        try
        {
            var result = await UnitService.GetAllAsync(includeInactive: false);

            if (result.IsSuccess && result.Value != null)
            {
                _unitLookup = result.Value.ToList();
            }
            else
            {
                _unitLookup = new List<UnitDto>();
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الوحدات.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            _unitLookup = new List<UnitDto>();
            Log.Error(ex, "خطأ أثناء تحميل قائمة الوحدات");
            MessageBox.Show("تعذر تحميل قائمة الوحدات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadProductsAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("CategoryName", typeof(string));
        table.Columns.Add("Barcode", typeof(string));
        table.Columns.Add("ReorderLevel", typeof(decimal));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await ProductService.GetAllAsync(includeInactive: true);

            _productsById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var p in result.Value)
                {
                    _productsById[p.Id] = p;
                    table.Rows.Add(
                        p.Id,
                        p.Name,
                        p.CategoryName ?? "",
                        p.Barcode ?? "",
                        p.ReorderLevel,
                        p.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الأصناف.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الأصناف");
            MessageBox.Show("تعذر تحميل قائمة الأصناف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _productsTable = table;
            _productsView = new DataView(_productsTable);
            dgvProducts.DataSource = _productsView;
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvProducts.Rows)
        {
            bool isActive = row.Cells["IsActive"].Value is bool b && b;
            if (!isActive)
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
        }
    }

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void _ApplyFilter()
    {
        if (_productsView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _productsView.RowFilter = "";
            return;
        }

        _productsView.RowFilter =
            $"Name LIKE '%{text}%' OR CategoryName LIKE '%{text}%' OR Barcode LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private async void dgvProducts_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvProducts.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            await _FillFormAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار صنف من الشبكة");
            MessageBox.Show("تعذر عرض بيانات الصنف المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task _FillFormAsync(int productId)
    {
        if (!_productsById.TryGetValue(productId, out var p)) return;

        _editId = p.Id;

        txtCode.Text = p.Id.ToString();
        txtName.Text = p.Name;

        // TODO: لا يوجد حقل مستقل للاسم الأجنبي في ProductDto — يُستخدم Description مؤقتاً.
        txtForeignName.Text = p.Description ?? "";

        // الباركود الرئيسي للصنف يُخزَّن في ProductDto.Barcode.
        txtMainUnitBarcode.Text = p.Barcode ?? "";

        // TODO: عنوان الصنف في المخزن غير موجود في ProductDto — يحتاج حقل LocationInStore.
        txtLocationInStore.Clear();

        chkTrackExpiry.Checked = p.TrackExpiry;
        numReorderLevel.Value = _Clamp(numReorderLevel, p.ReorderLevel);

        // TODO: نقطة التنبيه بالانتهاء غير موجودة في ProductDto — يحتاج حقل ExpiryAlertDays.
        numExpiryAlertDays.Value = 0;

        _SelectCategory(p.CategoryId);
        _UpdateModeHeader();

        await _LoadMainUnitAsync(p.Id);
    }

    /// <summary>
    /// إنشاء الوحدة الأساسية (الرئيسية) لصنف جديد مع سعر بيعها.
    /// أي تعثر هنا لا يُفشل حفظ الصنف — يُسجَّل الخطأ فقط.
    /// </summary>
    private async Task _CreateMainUnitAsync(int productId, int unitId)
    {
        try
        {
            // بيانات الوحدة الأساسية: الباركود والأسعار والخصم المسموح به
            // تُقرأ من حقول مجموعة (الوحدة الرئيسية) في هذه الشاشة.
            string? unitBarcode = string.IsNullOrWhiteSpace(txtMainUnitBarcode.Text)
                ? null
                : txtMainUnitBarcode.Text.Trim();

            decimal salePrice = numMainUnitSalePrice.Value;
            decimal costPrice = numMainUnitCostPrice.Value;
            decimal? maxDiscount = numMainUnitMaxDiscount.Value > 0m
                ? numMainUnitMaxDiscount.Value
                : null;

            var addUnit = await ProductService.AddProductUnitAsync(
                productId,
                new AddProductUnitRequest(
                    UnitId: (short)unitId,
                    Factor: 1m,
                    IsBaseUnit: true,
                    Barcode: unitBarcode,
                    SalePrice: salePrice,
                    WholesalePrice: null,
                    CostPrice: costPrice,
                    MaxDiscountAmount: maxDiscount));

            if (!addUnit.IsSuccess || addUnit.Value == null)
            {
                Log.Error("تعذر إنشاء الوحدة الأساسية للصنف {ProductId}. السبب: {Error}",
                    productId, addUnit.Error);
                return;
            }

            if (numMainUnitSalePrice.Value > 0)
            {
                var priceResult = await ProductService.CreateProductPriceAsync(
                    new CreateProductPriceRequest(addUnit.Value.Id, numMainUnitSalePrice.Value, DateTime.Today, null));

                if (!priceResult.IsSuccess)
                    Log.Error("تعذر حفظ سعر الوحدة الأساسية للصنف {ProductId}. السبب: {Error}",
                        productId, priceResult.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إنشاء الوحدة الأساسية للصنف {ProductId}", productId);
        }
    }

    private void _SelectCategory(int categoryId)
    {
        cmbCategory.SelectedValue = categoryId;

        if (cmbCategory.SelectedValue is not int selected || selected != categoryId)
            cmbCategory.SelectedIndex = 0; // لم يتم العثور على المجموعة → العنصر الافتراضي
    }

    // ── Main Unit / Price (read-only lookup) ──────────────────────────────────

    private async Task _LoadMainUnitAsync(int productId)
    {
        _isBindingMainUnit = true;
        try
        {
            var result = await ProductService.GetProductUnitsAsync(productId);

            if (result.IsSuccess && result.Value != null && result.Value.Count > 0)
            {
                var units = result.Value.ToList();

                cmbMainUnit.DisplayMember = "UnitName";
                cmbMainUnit.ValueMember = "Id";
                cmbMainUnit.DataSource = units;
                cmbMainUnit.Enabled = false;    // للعرض فقط في وضع التعديل

                var baseUnit = units.FirstOrDefault(u => u.IsBaseUnit) ?? units[0];
                cmbMainUnit.SelectedValue = baseUnit.Id;

                await _LoadSalePriceAsync(baseUnit.Id);
            }
            else
            {
                cmbMainUnit.DataSource = null;
                cmbMainUnit.Enabled = false;
                numMainUnitSalePrice.Value = 0;
                numMainUnitCostPrice.Value = 0;
                numMainUnitMaxDiscount.Value = 0;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل وحدات الصنف {ProductId}", productId);
        }
        finally
        {
            _isBindingMainUnit = false;
        }
    }

    private async Task _LoadSalePriceAsync(int productUnitId)
    {
        try
        {
            var result = await ProductService.GetProductPricesByUnitAsync(productUnitId);

            if (result.IsSuccess && result.Value != null)
            {
                var price = result.Value
                    .OrderByDescending(p => p.EffectiveFrom)
                    .FirstOrDefault();

                numMainUnitSalePrice.Value = _Clamp(numMainUnitSalePrice, price?.Price ?? 0m);
            }

            // TODO: سعر التكلفة والخصم المسموح به غير موجودين في ProductPriceDto.
            numMainUnitCostPrice.Value = 0;
            numMainUnitMaxDiscount.Value = 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل سعر الوحدة {ProductUnitId}", productUnitId);
        }
    }

    private async void cmbMainUnit_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBindingMainUnit) return;

        // في وضع الإضافة يكون SelectedValue هو رقم الوحدة (UnitDto.Id) وليس رقم وحدة الصنف،
        // لذلك لا يُبحث عن سعر إلا في وضع التعديل.
        if (_editId is not int) return;

        try
        {
            if (cmbMainUnit.SelectedValue is not int unitId || unitId <= 0) return;

            _isBindingMainUnit = true;
            await _LoadSalePriceAsync(unitId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تغيير وحدة الصنف");
            MessageBox.Show("تعذر تحميل سعر الوحدة المحددة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isBindingMainUnit = false;
        }
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل الأصناف )) — تعديل صنف"
            : "(( دليل الأصناف )) — إضافة صنف جديد";

    private void _ResetForm()
    {
        _editId = null;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtForeignName.Clear();
        txtLocationInStore.Clear();
        txtMainUnitBarcode.Clear();

        chkTrackExpiry.Checked = false;

        numReorderLevel.Value = 0;
        numExpiryAlertDays.Value = 0;
        numMainUnitCostPrice.Value = 0;
        numMainUnitSalePrice.Value = 0;
        numMainUnitMaxDiscount.Value = 0;
        errorProvider.SetError(cmbMainUnit, "");

        if (cmbCategory.Items.Count > 0)
            cmbCategory.SelectedIndex = 0;

        _isBindingMainUnit = true;
        try
        {
            // في وضع الإضافة تُربَط الوحدة الرئيسية بقائمة الوحدات (اختيار) لا بوحدات الصنف.
            cmbMainUnit.DisplayMember = "Name";
            cmbMainUnit.ValueMember = "Id";
            cmbMainUnit.DataSource = _unitLookup;
            cmbMainUnit.SelectedIndex = -1;
            cmbMainUnit.Enabled = true;
        }
        finally
        {
            _isBindingMainUnit = false;
        }

        dgvProducts.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الصنف مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (cmbCategory.SelectedValue is not int categoryId || categoryId <= 0)
        {
            errorProvider.SetError(cmbCategory, "يرجى اختيار مجموعة الصنف");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCategory, "");
        }

        if (numReorderLevel.Value < 0)
        {
            errorProvider.SetError(numReorderLevel, "نقطة إعادة الطلب يجب أن تكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numReorderLevel, "");
        }

        // عند إضافة صنف جديد يجب اختيار الوحدة الرئيسية (تُنشأ وحدتها الأساسية مع الحفظ).
        if (_editId is null)
        {
            if (cmbMainUnit.SelectedValue is not int unitId || unitId <= 0)
            {
                errorProvider.SetError(cmbMainUnit, "يرجى اختيار الوحدة الرئيسية");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(cmbMainUnit, "");
            }
        }
        else
        {
            errorProvider.SetError(cmbMainUnit, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _ResetForm();

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateForm()) return;
        if (cmbCategory.SelectedValue is not int categoryId || categoryId <= 0) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string description = txtForeignName.Text.Trim();
            string? barcode = string.IsNullOrWhiteSpace(txtMainUnitBarcode.Text)
                ? null
                : txtMainUnitBarcode.Text.Trim();

            Result<ProductDto> result;

            if (_editId is int id)
            {
                var request = new UpdateProductRequest(
                    name,
                    categoryId,
                    description,
                    numReorderLevel.Value,
                    chkTrackExpiry.Checked,
                    null,
                    barcode,
                    IsActive: true);

                result = await ProductService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateProductRequest(
                    name,
                    categoryId,
                    description,
                    numReorderLevel.Value,
                    chkTrackExpiry.Checked,
                    null,
                    barcode);

                // الوحدة المختارة من قائمة الوحدات في وضع الإضافة (UnitDto.Id)
                int? mainUnitId = cmbMainUnit.SelectedValue is int selectedUnitId && selectedUnitId > 0
                    ? selectedUnitId
                    : null;

                result = await ProductService.CreateAsync(request);

                // إنشاء الوحدة الأساسية للصنف الجديد مع سعر بيعها (لا يُفشل الحفظ عند تعثره).
                if (result.IsSuccess && result.Value != null && mainUnitId is int unitId)
                    await _CreateMainUnitAsync(result.Value.Id, unitId);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new ProductChangedMessage(result.Value.Id));

                MessageBox.Show(wasEditing ? "تم تعديل الصنف بنجاح." : "تمت إضافة الصنف بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
                await _LoadProductsAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();

                // بعد الإضافة: تحويل حقل الوحدة الرئيسية إلى عرض وحدات الصنف المضافة (للقراءة فقط).
                if (!wasEditing)
                    await _LoadMainUnitAsync(result.Value.Id);
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ الصنف.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الصنف");
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار صنف من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) الصنف \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await ProductService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل الصنف بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new ProductChangedMessage(id));
                _ResetForm();
                await _LoadProductsAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل الصنف.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الصنف {ProductId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnDelete.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async void btnManageUnits_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى حفظ الصنف أولاً قبل إدارة وحداته وأسعاره.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Cursor = Cursors.WaitCursor;
        try
        {
            var dialog = _serviceProvider.GetRequiredService<frmProductUnitMaster>();
            dialog.LoadData(id, txtName.Text.Trim());

            if (dialog.ShowDialog(this) == DialogResult.OK)
                await _LoadMainUnitAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح شاشة وحدات وأسعار الصنف");
            MessageBox.Show("تعذر فتح شاشة وحدات وأسعار الصنف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnAddCategory_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var dialog = _serviceProvider.GetRequiredService<frmCategoryMaster>();

            if (dialog.ShowDialog(this) == DialogResult.OK)
                await _LoadCategoriesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح شاشة مجموعات الأصناف");
            MessageBox.Show("تعذر فتح شاشة مجموعات الأصناف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int productId)
    {
        foreach (DataGridViewRow row in dgvProducts.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == productId)
            {
                row.Selected = true;
                dgvProducts.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }

    private static decimal _Clamp(NumericUpDown control, decimal value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);
}
