using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Enums;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Products.Forms;

/// <summary>
/// شاشة (Master) لإدارة وحدات صنف واحد وأسعار كل وحدة في نفس الشاشة.
/// تحل محل النموذج المنبثق القديم frmAddUpdateProductUnit.
/// </summary>
public partial class frmProductUnitMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IProductApiService ProductService => _serviceProvider.GetRequiredService<IProductApiService>();
    private IUnitApiService UnitService => _serviceProvider.GetRequiredService<IUnitApiService>();

    // ── State ────────────────────────────────────────────────────────────────
    private int _productId;
    private string _productName = string.Empty;

    private List<ProductUnitDto> _units = new();
    private List<ProductPriceDto> _prices = new();

    /// <summary>null = إضافة وحدة جديدة، &gt; 0 = تعديل وحدة قائمة.</summary>
    private int? _editUnitId;

    /// <summary>الوحدة المحددة في الشبكة (تُستخدم لإدارة أسعارها).</summary>
    private int? _selectedUnitId;

    /// <summary>يمنع إعادة التحميل أثناء تعبئة الحقول برمجياً.</summary>
    private bool _isBinding;

    public frmProductUnitMaster(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _WireEvents();
    }

    // ── Public API (يستدعيه frmProductMaster) ─────────────────────────────────

    /// <summary>تمرير الصنف المحدد من شاشة دليل الأصناف إلى هذه الشاشة.</summary>
    public void LoadData(int productId, string productName)
    {
        _productId = productId;
        _productName = productName ?? string.Empty;

        Text = "وحدات وأسعار الصنف";
        lblMode.Text = $"(( وحدات وأسعار الصنف )) — {_productName}";
        lblProduct.Text = $"الصنف: {_productName}  —  رقم: {_productId}";

        _ResetUnitForm();
        _ResetPriceForm();
    }

    // ── Event wiring ─────────────────────────────────────────────────────────

    private void _WireEvents()
    {
        Load += frmProductUnitMaster_Load;

        btnNewUnit.Click += btnNewUnit_Click;
        btnSaveUnit.Click += btnSaveUnit_Click;
        btnDeleteUnit.Click += btnDeleteUnit_Click;

        btnAddPrice.Click += btnAddPrice_Click;
        btnDeletePrice.Click += btnDeletePrice_Click;

        btnClose.Click += btnClose_Click;

        dgvUnits.SelectionChanged += dgvUnits_SelectionChanged;
    }

    // ── Grid setup ───────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvUnits.Columns.Clear();
        dgvUnits.AutoGenerateColumns = false;

        dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "رقم الوحدة",
            DataPropertyName = "Id",
            FillWeight = 10,
            MinimumWidth = 90,
            ReadOnly = true,
        });

        dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "UnitName",
            HeaderText = "الوحدة",
            DataPropertyName = "UnitName",
            FillWeight = 30,
            MinimumWidth = 140,
            ReadOnly = true,
        });

        dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ConversionFactor",
            HeaderText = "معامل التحويل",
            DataPropertyName = "ConversionFactor",
            FillWeight = 20,
            MinimumWidth = 120,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N3", Alignment = DataGridViewContentAlignment.MiddleCenter },
        });

        dgvUnits.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "IsBaseUnit",
            HeaderText = "الوحدة الأساسية",
            DataPropertyName = "IsBaseUnit",
            FillWeight = 18,
            MinimumWidth = 110,
            ReadOnly = true,
        });

        dgvPrices.Columns.Clear();
        dgvPrices.AutoGenerateColumns = false;

        dgvPrices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Id",
            HeaderText = "رقم السعر",
            DataPropertyName = "Id",
            FillWeight = 10,
            MinimumWidth = 80,
            ReadOnly = true,
        });

        dgvPrices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Price",
            HeaderText = "السعر",
            DataPropertyName = "Price",
            FillWeight = 22,
            MinimumWidth = 120,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleLeft },
        });

        dgvPrices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "EffectiveFrom",
            HeaderText = "ساري من",
            DataPropertyName = "EffectiveFrom",
            FillWeight = 22,
            MinimumWidth = 120,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter },
        });

        dgvPrices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "EffectiveTo",
            HeaderText = "ساري حتى",
            DataPropertyName = "EffectiveTo",
            FillWeight = 22,
            MinimumWidth = 120,
            ReadOnly = true,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy", Alignment = DataGridViewContentAlignment.MiddleCenter },
        });

        ApplyDevColumns();
    }

    /// <summary>يعرض أسماء أعمدة الشبكات في شريط سفلي — في وضع التجربة (Mock) فقط.</summary>
    private void ApplyDevColumns()
    {
        if (!SalesSystem.Desktop.Configuration.AppSettings.Current.UseMockData)
            return;

        var headers = new List<string>();
        foreach (DataGridView grid in new[] { dgvUnits, dgvPrices })
            foreach (DataGridViewColumn column in grid.Columns)
                headers.Add(column.HeaderText);

        lblDevColumns.Text = "الأعمدة: " + string.Join(" | ", headers);
        lblDevColumns.Visible = true;
    }

    // ── Form load ────────────────────────────────────────────────────────────

    private async void frmProductUnitMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _SetupGridColumns();
            await _LoadUnitsLookupAsync();
            await _LoadUnitsAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة وحدات وأسعار الصنف {ProductId}", _productId);
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Lookups ──────────────────────────────────────────────────────────────

    private async Task _LoadUnitsLookupAsync()
    {
        var result = await UnitService.GetAllAsync(includeInactive: true);

        if (result.IsSuccess && result.Value != null)
        {
            cmbUnit.DisplayMember = "Name";
            cmbUnit.ValueMember = "Id";
            cmbUnit.DataSource = result.Value.ToList();
            cmbUnit.SelectedIndex = -1;
        }
        else
        {
            MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الوحدات.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Data loading ─────────────────────────────────────────────────────────

    private async Task _LoadUnitsAsync()
    {
        var result = await ProductService.GetProductUnitsAsync(_productId);

        if (!result.IsSuccess || result.Value == null)
        {
            MessageBox.Show(result.Error ?? "تعذر تحميل وحدات الصنف.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _units = result.Value.OrderByDescending(u => u.IsBaseUnit).ThenBy(u => u.UnitName).ToList();

        // إعادة تحديد الوحدة السابقة إن وُجدت، وإلا أول وحدة في القائمة
        var keepId = _selectedUnitId ?? _units.FirstOrDefault()?.Id;

        _isBinding = true;
        try
        {
            dgvUnits.DataSource = null;
            dgvUnits.DataSource = _units;
            dgvUnits.ClearSelection();
        }
        finally
        {
            _isBinding = false;
        }

        _selectedUnitId = null;

        if (keepId is int id)
            _SelectUnitRow(id);

        if (_selectedUnitId is int selected)
        {
            var unit = _units.FirstOrDefault(u => u.Id == selected);
            if (unit != null)
                _FillUnitForm(unit);

            await _LoadPricesAsync(selected);
        }
        else
        {
            _prices = new List<ProductPriceDto>();
            dgvPrices.DataSource = null;
        }
    }

    private async Task _LoadPricesAsync(int productUnitId)
    {
        var result = await ProductService.GetProductPricesByUnitAsync(productUnitId);

        if (!result.IsSuccess || result.Value == null)
        {
            MessageBox.Show(result.Error ?? "تعذر تحميل أسعار الوحدة المحددة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _prices = result.Value.OrderByDescending(p => p.EffectiveFrom).ToList();

        _isBinding = true;
        try
        {
            dgvPrices.DataSource = null;
            dgvPrices.DataSource = _prices;
        }
        finally
        {
            _isBinding = false;
        }
    }

    // ── Grid selection ───────────────────────────────────────────────────────

    private async void dgvUnits_SelectionChanged(object? sender, EventArgs e)
    {
        if (_isBinding) return;

        try
        {
            if (dgvUnits.CurrentRow?.DataBoundItem is not ProductUnitDto unit) return;
            if (unit.Id == _selectedUnitId) return;

            _selectedUnitId = unit.Id;
            _FillUnitForm(unit);
            await _LoadPricesAsync(unit.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار وحدة من الشبكة");
            MessageBox.Show("تعذر عرض بيانات الوحدة المحددة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillUnitForm(ProductUnitDto unit)
    {
        _editUnitId = unit.Id;

        _isBinding = true;
        try
        {
            cmbUnit.SelectedValue = unit.UnitId;
            numFactor.Value = _Clamp(numFactor, unit.ConversionFactor);
            chkIsBaseUnit.Checked = unit.IsBaseUnit;
        }
        finally
        {
            _isBinding = false;
        }

        lblMode.Text = $"(( وحدات وأسعار الصنف )) — {_productName} — تعديل وحدة";
    }

    // ── Unit CRUD ────────────────────────────────────────────────────────────

    private void btnNewUnit_Click(object? sender, EventArgs e) => _ResetUnitForm();

    private async void btnSaveUnit_Click(object? sender, EventArgs e)
    {
        if (!_ValidateUnitForm()) return;

        if (cmbUnit.SelectedValue is not int unitId || unitId <= 0) return;

        bool isBase = chkIsBaseUnit.Checked;
        decimal factor = isBase ? 1m : numFactor.Value;
        bool wasEditing = _editUnitId.HasValue;

        Cursor = Cursors.WaitCursor;
        btnSaveUnit.Enabled = false;

        try
        {
            Result<ProductUnitDto> result;

            if (_editUnitId is int productUnitId)
            {
                // UpdateProductUnitRequest لا يحتوي إلا على UnitId — معامل التحويل
                // والوحدة الأساسية لا يمكن تعديلهما بعد الإضافة (قاعدة نطاق في BLL).
                result = await ProductService.UpdateProductUnitAsync(
                    _productId, productUnitId, new UpdateProductUnitRequest((short)unitId));

                if (result.IsSuccess)
                    MessageBox.Show("تم تعديل وحدة الصنف بنجاح.\nملاحظة: معامل التحويل والوحدة الأساسية لا يتم تعديلهما بعد الإضافة.",
                        "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var request = new AddProductUnitRequest((short)unitId, factor, isBase);
                result = await ProductService.AddProductUnitAsync(_productId, request);

                if (result.IsSuccess)
                    MessageBox.Show("تمت إضافة وحدة الصنف بنجاح.",
                        "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (result.IsSuccess && result.Value != null)
            {
                _selectedUnitId = result.Value.Id;
                await _LoadUnitsAsync();
                _SelectUnitRow(result.Value.Id);
                _ResetUnitForm();
                lblMode.Text = $"(( وحدات وأسعار الصنف )) — {_productName}";
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ وحدة الصنف.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ وحدة الصنف {ProductId}", _productId);
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSaveUnit.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async void btnDeleteUnit_Click(object? sender, EventArgs e)
    {
        if (_editUnitId is not int productUnitId)
        {
            MessageBox.Show("يرجى اختيار وحدة من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string unitName = dgvUnits.CurrentRow?.DataBoundItem is ProductUnitDto unit
            ? unit.UnitName ?? ""
            : "";

        if (MessageBox.Show($"هل أنت متأكد من حذف الوحدة \"{unitName}\" من هذا الصنف؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDeleteUnit.Enabled = false;

        try
        {
            var result = await ProductService.DeleteProductUnitAsync(_productId, productUnitId, DeleteStrategy.Permanent);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم حذف وحدة الصنف بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _selectedUnitId = null;
                _ResetUnitForm();
                await _LoadUnitsAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم حذف وحدة الصنف.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حذف وحدة الصنف {ProductUnitId}", productUnitId);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnDeleteUnit.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // ── Price CRUD ───────────────────────────────────────────────────────────

    private async void btnAddPrice_Click(object? sender, EventArgs e)
    {
        if (_selectedUnitId is not int productUnitId)
        {
            MessageBox.Show("يرجى اختيار وحدة من القائمة أولاً لإضافة سعر لها.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (numPrice.Value <= 0)
        {
            errorProvider.SetError(numPrice, "السعر يجب أن يكون أكبر من صفر");
            MessageBox.Show("يرجى إدخال سعر أكبر من صفر.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        errorProvider.SetError(numPrice, "");

        DateTime from = dtpEffectiveFrom.Value.Date;
        DateTime? to = dtpEffectiveTo.Checked ? dtpEffectiveTo.Value.Date : null;

        if (to.HasValue && to.Value < from)
        {
            errorProvider.SetError(dtpEffectiveTo, "تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
            MessageBox.Show("تاريخ نهاية السعر يجب أن يكون بعد تاريخ بدايته.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        errorProvider.SetError(dtpEffectiveTo, "");

        Cursor = Cursors.WaitCursor;
        btnAddPrice.Enabled = false;

        try
        {
            var request = new CreateProductPriceRequest(productUnitId, numPrice.Value, from, to);
            var result = await ProductService.CreateProductPriceAsync(request);

            if (result.IsSuccess)
            {
                MessageBox.Show("تمت إضافة سعر الوحدة بنجاح.", "تم الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetPriceForm();
                await _LoadPricesAsync(productUnitId);
            }
            else
            {
                MessageBox.Show($"لم يتم إضافة السعر.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إضافة سعر للوحدة {ProductUnitId}", productUnitId);
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnAddPrice.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async void btnDeletePrice_Click(object? sender, EventArgs e)
    {
        if (dgvPrices.CurrentRow?.DataBoundItem is not ProductPriceDto price)
        {
            MessageBox.Show("يرجى اختيار سعر من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"هل أنت متأكد من حذف السعر ({price.Price:N2})؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDeletePrice.Enabled = false;

        try
        {
            var result = await ProductService.DeleteProductPriceAsync(price.Id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم حذف السعر بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_selectedUnitId is int productUnitId)
                    await _LoadPricesAsync(productUnitId);
            }
            else
            {
                MessageBox.Show($"لم يتم حذف السعر.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حذف السعر {PriceId}", price.Id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnDeletePrice.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // ── Validation ───────────────────────────────────────────────────────────

    private bool _ValidateUnitForm()
    {
        bool isValid = true;

        if (cmbUnit.SelectedValue is not int unitId || unitId <= 0)
        {
            errorProvider.SetError(cmbUnit, "يرجى اختيار الوحدة");
            isValid = false;
        }
        else if (_units.Any(u => u.UnitId == unitId && u.Id != (_editUnitId ?? 0)))
        {
            errorProvider.SetError(cmbUnit, "هذه الوحدة مضافة بالفعل لهذا الصنف");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbUnit, "");
        }

        if (!chkIsBaseUnit.Checked && numFactor.Value <= 1m)
        {
            errorProvider.SetError(numFactor, "معامل التحويل يجب أن يكون أكبر من 1 للوحدات الفرعية");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numFactor, "");
        }

        if (chkIsBaseUnit.Checked && _units.Any(u => u.IsBaseUnit && u.Id != (_editUnitId ?? 0)))
        {
            errorProvider.SetError(chkIsBaseUnit, "يوجد بالفعل وحدة أساسية لهذا الصنف");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(chkIsBaseUnit, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── Reset ────────────────────────────────────────────────────────────────

    private void _ResetUnitForm()
    {
        _editUnitId = null;
        errorProvider.SetError(cmbUnit, "");

        _isBinding = true;
        try
        {
            if (cmbUnit.Items.Count > 0)
                cmbUnit.SelectedIndex = -1;

            numFactor.Value = 1m;
            chkIsBaseUnit.Checked = false;
        }
        finally
        {
            _isBinding = false;
        }

        lblMode.Text = $"(( وحدات وأسعار الصنف )) — {_productName}";
    }

    private void _ResetPriceForm()
    {
        numPrice.Value = 0m;
        dtpEffectiveFrom.Value = DateTime.Today;
        dtpEffectiveTo.Checked = false;
        dtpEffectiveTo.Value = DateTime.Today;

        errorProvider.SetError(numPrice, "");
        errorProvider.SetError(dtpEffectiveTo, "");
    }

    // ── Close ────────────────────────────────────────────────────────────────

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private void _SelectUnitRow(int productUnitId)
    {
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            if (row.DataBoundItem is ProductUnitDto unit && unit.Id == productUnitId)
            {
                // التعيين قبل التحديد حتى يتجاهل معالج SelectionChanged هذه الخطوة
                _selectedUnitId = unit.Id;
                row.Selected = true;
                dgvUnits.CurrentCell = row.Cells["UnitName"];
                break;
            }
        }
    }

    private static decimal _Clamp(NumericUpDown control, decimal value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);
}
