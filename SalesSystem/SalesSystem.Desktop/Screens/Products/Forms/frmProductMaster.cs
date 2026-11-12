using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
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

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmProductMaster()
    {
        InitializeComponent();
    }

    public frmProductMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this);

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        _WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnManageUnits);
    }

    /// <summary>
    /// هل الحقول المطلوبة مكتملة؟ (اسم الصنف + المجموعة + الوحدة الرئيسية عند الإضافة).
    /// </summary>
    private bool _RequiredFieldsValid()
    {
        bool nameOk = !string.IsNullOrWhiteSpace(txtName.Text);
        bool categoryOk = cmbCategory.SelectedValue is int categoryId && categoryId > 0;

        // الوحدة الرئيسية مطلوبة فقط عند إضافة صنف جديد (في التعديل تُعرض وحدات الصنف للقراءة فقط).
        bool mainUnitOk = _editId is not null
            || (cmbMainUnit.SelectedValue is int unitId && unitId > 0);

        return nameOk && categoryOk && mainUnitOk;
    }

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    private void _FocusFirstMissingField(bool edit)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الصنف مطلوب");
            txtName.Focus();
            return;
        }

        if (cmbCategory.SelectedValue is not int categoryId || categoryId <= 0)
        {
            errorProvider.SetError(cmbCategory, "يرجى اختيار مجموعة الصنف");
            cmbCategory.Focus();
            return;
        }

        if (!edit && (cmbMainUnit.SelectedValue is not int unitId || unitId <= 0))
        {
            errorProvider.SetError(cmbMainUnit, "يرجى اختيار الوحدة الرئيسية");
            cmbMainUnit.Focus();
            return;
        }
    }

    /// <summary>تحديث يدوي لقائمة الأصناف مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadProductsAsync();
            _ResetForm(); // تفريغ الحقول + وضع الاستعداد بعد التحديث
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث البيانات");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
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

            ApplyGreenTheme(); // الثيم الأخضر آخر خطوة في مسار التحميل (RULE-N50)
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

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.
    /// </summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblTitle);
        GreenThemeHelper.ApplyGrid(dgvProducts);
        GreenThemeHelper.Apply(this); // ← الأخير دائماً
    }

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
        table.Columns.Add("Id", typeof(int)).Caption = "الرقم";
        table.Columns.Add("Name", typeof(string)).Caption = "اسم الصنف";
        table.Columns.Add("ForeignName", typeof(string)).Caption = "الاسم الأجنبي";
        table.Columns.Add("CategoryName", typeof(string)).Caption = "مجموعة الصنف";
        table.Columns.Add("Barcode", typeof(string)).Caption = "الباركود";
        table.Columns.Add("CurrentStock", typeof(decimal)).Caption = "المخزون";
        table.Columns.Add("ReorderLevel", typeof(decimal)).Caption = "نقطة إعادة الطلب";
        table.Columns.Add("TrackExpiry", typeof(bool)).Caption = "ينتهي؟";
        table.Columns.Add("ExpiryAlertDays", typeof(short)).Caption = "نقطة التنبيه";
        table.Columns.Add("LocationInStore", typeof(string)).Caption = "العنوان في المخزن";
        table.Columns.Add("Description", typeof(string)).Caption = "ملاحظات";
        table.Columns.Add("IsActive", typeof(bool)).Caption = "الحالة";

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
                        p.ForeignName ?? "",
                        p.CategoryName ?? "",
                        p.Barcode ?? "",
                        p.CurrentStock,
                        p.ReorderLevel,
                        p.TrackExpiry,
                        p.ExpiryAlertDays,
                        p.LocationInStore ?? "",
                        p.Description ?? "",
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
            _ApplyGridHeaders();
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يستخدم DataGridView عنوان <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً بعد إسناد <c>DataSource</c>.
    /// </summary>
    private static readonly Dictionary<string, string> _gridHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Id"] = "الرقم",
        ["Name"] = "اسم الصنف",
        ["ForeignName"] = "الاسم الأجنبي",
        ["CategoryName"] = "مجموعة الصنف",
        ["Barcode"] = "الباركود",
        ["CurrentStock"] = "المخزون",
        ["ReorderLevel"] = "نقطة إعادة الطلب",
        ["TrackExpiry"] = "ينتهي؟",
        ["ExpiryAlertDays"] = "نقطة التنبيه",
        ["LocationInStore"] = "العنوان في المخزن",
        ["Description"] = "ملاحظات",
        ["IsActive"] = "الحالة"
    };

    /// <summary>تطبيق العناوين العربية على أعمدة شبكة الأصناف بعد ربط البيانات.</summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvProducts.Columns)
        {
            if (_gridHeaders.TryGetValue(column.DataPropertyName, out var headerText))
                column.HeaderText = headerText;
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

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>
    /// يمنع إدخال علامات الترقيم (مثل ' و % و [ ]) التي تفسد فلتر البحث DataView.RowFilter.
    /// </summary>
    private void txtSearch_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(txtSearch, "لا يُسمح بإدخال علامات الترقيم.");
        }
    }

    private void _ApplyFilter()
    {
        if (_productsView is null) return;

        string text = txtSearch.Text.Trim();

        var conditions = new List<string>();

        // إخفاء غير النشط ما لم يُفَعَّل خيار "عرض غير النشط"
        if (!chkShowInactive.Checked)
            conditions.Add("IsActive = true");

        if (!string.IsNullOrEmpty(text))
            conditions.Add(
                $"(Name LIKE '%{text}%' OR CategoryName LIKE '%{text}%' OR Barcode LIKE '%{text}%' " +
                $"OR CONVERT(Id, 'System.String') LIKE '{text}%')");

        _productsView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
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

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        // إدارة الوحدات والأسعار تحتاج صنفاً محفوظاً → تُتاح في وضع التعديل فقط.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;
        btnManageUnits.Enabled = true;

        txtCode.Text = p.Id.ToString();
        txtName.Text = p.Name;
        txtForeignName.Text = p.ForeignName ?? "";
        txtLocationInStore.Text = p.LocationInStore ?? "";
        txtNotes.Text = p.Description ?? "";
        numExpiryAlertDays.Value = _Clamp(numExpiryAlertDays, p.ExpiryAlertDays ?? 0);

        // الباركود الرئيسي للصنف يُخزَّن في ProductDto.Barcode.
        txtMainUnitBarcode.Text = p.Barcode ?? "";

        chkTrackExpiry.Checked = p.TrackExpiry;
        numReorderLevel.Value = _Clamp(numReorderLevel, p.ReorderLevel);

        _SelectCategory(p.CategoryId);

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

                // توحيد ربط الوحدة الرئيسية بين وضعي الإضافة والتعديل:
                // دائماً DisplayMember = "Name" و ValueMember = "Id" على قائمة UnitDto.
                var unitOptions = units
                    .Select(u => new UnitDto(u.UnitId, u.UnitName ?? "", null, true))
                    .ToList();

                cmbMainUnit.DisplayMember = "Name";
                cmbMainUnit.ValueMember = "Id";
                cmbMainUnit.DataSource = unitOptions;
                cmbMainUnit.Enabled = false;    // للعرض فقط في وضع التعديل

                var baseUnit = units.FirstOrDefault(u => u.IsBaseUnit) ?? units[0];
                cmbMainUnit.SelectedValue = baseUnit.UnitId;   // رقم الوحدة (متوافق مع وضع الإضافة)

                await _LoadSalePriceAsync(baseUnit.Id);        // رقم وحدة الصنف لتحميل السعر
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

    // ── Reset ──────────────────────────────────────────────────────────

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الإضافة متاح، وزرّي التعديل والحذف معطّلان لعدم وجود سجل محدد.
        // إدارة الوحدات والأسعار معطّلة أيضاً حتى يُحفظ الصنف أولاً.
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;
        btnManageUnits.Enabled = false;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtForeignName.Clear();
        txtLocationInStore.Clear();
        txtNotes.Clear();
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
        cmbCategory.Focus();
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

    /// <summary>زر الخروج: إغلاق شاشة دليل الأصناف.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>زر الإضافة: يُضيف صنفاً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveProductAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث الصنف المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveProductAsync(edit: true);

    /// <summary>
    /// حفظ الصنف: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveProductAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقول المطلوبة الناقصة: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_RequiredFieldsValid())
        {
            _FocusFirstMissingField(edit);
            return;
        }

        if (!_ValidateForm()) return;
        if (cmbCategory.SelectedValue is not int categoryId || categoryId <= 0) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = txtName.Text.Trim();
            if (MessageBox.Show($"هل أنت متأكد من تعديل الصنف \"{editName}\"؟",
                    "تأكيد التعديل", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
        btnEdit.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string? description = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();
            string? foreignName = string.IsNullOrWhiteSpace(txtForeignName.Text) ? null : txtForeignName.Text.Trim();
            string? locationInStore = string.IsNullOrWhiteSpace(txtLocationInStore.Text) ? null : txtLocationInStore.Text.Trim();
            short? expiryAlertDays = numExpiryAlertDays.Value > 0 ? (short?)numExpiryAlertDays.Value : null;
            string? barcode = string.IsNullOrWhiteSpace(txtMainUnitBarcode.Text)
                ? null
                : txtMainUnitBarcode.Text.Trim();

            Result<ProductDto> result;

            if (edit)
            {
                var request = new UpdateProductRequest(
                    name,
                    categoryId,
                    description,
                    numReorderLevel.Value,
                    chkTrackExpiry.Checked,
                    null,
                    barcode,
                    ForeignName: foreignName,
                    LocationInStore: locationInStore,
                    ExpiryAlertDays: expiryAlertDays,
                    IsActive: true);

                result = await ProductService.UpdateAsync(_editId!.Value, request);
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
                    barcode,
                    ForeignName: foreignName,
                    LocationInStore: locationInStore,
                    ExpiryAlertDays: expiryAlertDays);

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

                MessageBox.Show(edit ? "تم تعديل الصنف بنجاح." : "تمت إضافة الصنف بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). النصوص ثابتة في المصمم.
                _editId = result.Value.Id;
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnManageUnits.Enabled = true;

                await _LoadProductsAsync();
                _SelectGridRow(result.Value.Id);

                // بعد الإضافة: تحويل حقل الوحدة الرئيسية إلى عرض وحدات الصنف المضافة (للقراءة فقط).
                if (!edit)
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
            // استعادة حالة الأزرار حسب الوضع الحالي للنموذج.
            if (_editId is null)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
            }

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
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زرّا الحذف والتعديل معطّلين.
            btnDelete.Enabled = _editId.HasValue;
            btnEdit.Enabled = _editId.HasValue;
            btnManageUnits.Enabled = _editId.HasValue;
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

    /// <summary>
    /// يمنح الأزرار شكلاً واضحاً عند تعطيلها (خلفية رمادية + نص باهت) بدلاً من المظهر الافتراضي غير الواضح.
    /// </summary>
    private static void _WireButtonDisabledVisual(params Button[] buttons)
    {
        foreach (var btn in buttons)
        {
            var normalBack = btn.BackColor;
            var normalFore = btn.ForeColor;

            void Apply()
            {
                btn.BackColor = btn.Enabled ? normalBack : Color.FromArgb(224, 224, 224);
                btn.ForeColor = btn.Enabled ? normalFore : Color.FromArgb(150, 150, 150);
            }

            btn.EnabledChanged += (s, e) => Apply();
            Apply();
        }
    }

    // ── Barcode Scanner ───────────────────────────────────────────────────────

    /// <summary>
    /// Enter من قارئ الباركود: إن كان الباركود مسجّلاً لصنف موجود يُحمَّل الصنف للتعديل،
    /// وإن كان جديداً يبقى الباركود في الحقل وينتقل المؤشر إلى اسم الصنف (إضافة سريعة).
    /// </summary>
    private async void txtMainUnitBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        string code = txtMainUnitBarcode.Text.Trim();

        if (string.IsNullOrEmpty(code))
        {
            txtName.Focus();
            return;
        }

        try
        {
            var existing = _productsById.Values.FirstOrDefault(p =>
                string.Equals(p.Barcode, code, StringComparison.OrdinalIgnoreCase));

            if (existing is not null)
            {
                Cursor = Cursors.WaitCursor;
                await _FillFormAsync(existing.Id);
                _SelectGridRow(existing.Id);
                txtMainUnitBarcode.Focus();
                return;
            }

            // باركود غير مسجّل → إضافة سريعة: يبقى الباركود كما هو وننتقل لاسم الصنف.
            txtName.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء البحث عن الباركود {Barcode}", code);
            MessageBox.Show("تعذر البحث عن الباركود. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void dgvProducts_ChangeUICues(object sender, UICuesEventArgs e)
    {

    }
}
