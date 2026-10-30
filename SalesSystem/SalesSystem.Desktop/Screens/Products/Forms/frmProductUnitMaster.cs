using System.Data;
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
/// شاشة (Master) لإدارة وحدات صنف واحد في شاشة واحدة:
/// بحث مباشر + شبكة + نموذج إدخال/تعديل، مثل <see cref="frmProductMaster"/>.
/// تعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// كل وحدة تحمل باركودها وأسعارها (تجزئة/جملة/تكلفة) وأقصى خصم مسموح به.
/// </summary>
public partial class frmProductUnitMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IProductApiService ProductService => _serviceProvider.GetRequiredService<IProductApiService>();
    private IUnitApiService UnitService => _serviceProvider.GetRequiredService<IUnitApiService>();

    // ── State ────────────────────────────────────────────────────────────────
    private int _productId;
    private string _productName = string.Empty;

    private readonly Dictionary<int, ProductUnitDto> _unitsById = new();
    private DataTable _unitsTable = new();
    private DataView _unitsView = new();

    /// <summary>null = إضافة وحدة جديدة، &gt; 0 = تعديل وحدة قائمة.</summary>
    private int? _editId;

    /// <summary>يمنع معالجات الأحداث من إعادة تعبئة النموذج أثناء الربط البرمجي.</summary>
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

        Text = $"وحدات الصنف — {_productName}";
        lblProduct.Text = $"الصنف: {_productName}    —    رقم الصنف: {_productId}";

        _ResetForm();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmProductUnitMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnClose.Click += btnClose_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvUnits.CellClick += dgvUnits_CellClick;
        dgvUnits.SelectionChanged += dgvUnits_SelectionChanged;
    }

    // ── Grid Setup ────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvUnits.Columns.Clear();
        dgvUnits.AutoGenerateColumns = false;

        var columns = new[]
        {
            new { Name = "Id",               Header = "رقم الوحدة",      Fill = 8,  Min = 90,  Format = "" },
            new { Name = "UnitName",         Header = "الوحدة",         Fill = 18, Min = 130, Format = "" },
            new { Name = "Barcode",          Header = "الباركود",       Fill = 16, Min = 130, Format = "" },
            new { Name = "ConversionFactor", Header = "معامل التحويل",  Fill = 12, Min = 110, Format = "N3" },
            new { Name = "SalePrice",        Header = "سعر البيع",      Fill = 13, Min = 110, Format = "N2" },
            new { Name = "CostPrice",        Header = "سعر التكلفة",    Fill = 13, Min = 110, Format = "N2" },
        };

        foreach (var col in columns)
        {
            var cellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
            if (!string.IsNullOrEmpty(col.Format))
                cellStyle.Format = col.Format;

            dgvUnits.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
                DefaultCellStyle = cellStyle,
            });
        }

        dgvUnits.Columns.Add(new DataGridViewCheckBoxColumn
        {
            Name = "IsBaseUnit",
            HeaderText = "الوحدة الأساسية",
            DataPropertyName = "IsBaseUnit",
            MinimumWidth = 110,
            FillWeight = 10,
            ReadOnly = true,
        });

        ApplyDevColumns();
    }

    /// <summary>يعرض أسماء أعمدة الشبكة في شريط سفلي — في وضع التجربة (Mock) فقط.</summary>
    private void ApplyDevColumns()
    {
        if (!SalesSystem.Desktop.Configuration.AppSettings.Current.UseMockData)
            return;

        var headers = new List<string>();
        foreach (DataGridViewColumn column in dgvUnits.Columns)
            headers.Add(column.HeaderText);

        lblDevColumns.Text = "الأعمدة: " + string.Join(" | ", headers);
        lblDevColumns.Visible = true;
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmProductUnitMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _SetupGridColumns();
            await _LoadUnitLookupAsync();
            await _LoadUnitsAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة وحدات الصنف {ProductId}", _productId);
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Lookups ───────────────────────────────────────────────────────────────

    private async Task _LoadUnitLookupAsync()
    {
        try
        {
            var result = await UnitService.GetAllAsync(includeInactive: false);

            if (result.IsSuccess && result.Value != null)
            {
                _isBinding = true;
                try
                {
                    cmbUnit.DisplayMember = "Name";
                    cmbUnit.ValueMember = "Id";
                    cmbUnit.DataSource = result.Value.ToList();
                    cmbUnit.SelectedIndex = -1;
                }
                finally
                {
                    _isBinding = false;
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الوحدات.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الوحدات");
            MessageBox.Show("تعذر تحميل قائمة الوحدات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadUnitsAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("UnitName", typeof(string));
        table.Columns.Add("Barcode", typeof(string));
        table.Columns.Add("ConversionFactor", typeof(decimal));
        table.Columns.Add("SalePrice", typeof(decimal));
        table.Columns.Add("CostPrice", typeof(decimal));
        table.Columns.Add("IsBaseUnit", typeof(bool));

        try
        {
            var result = await ProductService.GetProductUnitsAsync(_productId);

            _unitsById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var unit in result.Value.OrderByDescending(u => u.IsBaseUnit).ThenBy(u => u.UnitName))
                {
                    _unitsById[unit.Id] = unit;
                    table.Rows.Add(
                        unit.Id,
                        unit.UnitName ?? "",
                        unit.Barcode ?? "",
                        unit.ConversionFactor,
                        unit.SalePrice,
                        unit.CostPrice,
                        unit.IsBaseUnit);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل وحدات الصنف.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل وحدات الصنف {ProductId}", _productId);
            MessageBox.Show("تعذر تحميل وحدات الصنف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _unitsTable = table;
            _unitsView = new DataView(_unitsTable);

            _isBinding = true;
            try
            {
                dgvUnits.DataSource = _unitsView;
                dgvUnits.ClearSelection();
            }
            finally
            {
                _isBinding = false;
            }

            _ApplyFilter();
            _ApplyRowColoring();

            // الحفاظ على الوحدة المحددة إن كانت لا تزال موجودة
            if (_editId is int keepId && _unitsById.ContainsKey(keepId))
                _SelectGridRow(keepId);
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            bool isBase = row.Cells["IsBaseUnit"].Value is bool b && b;
            if (isBase)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 205);
                row.DefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            }
        }
    }

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void _ApplyFilter()
    {
        if (_unitsView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _unitsView.RowFilter = "";
            return;
        }

        _unitsView.RowFilter =
            $"UnitName LIKE '%{text}%' OR Barcode LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private async void dgvUnits_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvUnits.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            await _FillFormAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار وحدة من الشبكة");
            MessageBox.Show("تعذر عرض بيانات الوحدة المحددة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void dgvUnits_SelectionChanged(object? sender, EventArgs e)
    {
        if (_isBinding) return;

        try
        {
            if (dgvUnits.CurrentRow?.DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;
            if (_editId == id) return;

            await _FillFormAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديد وحدة في الشبكة");
            MessageBox.Show("تعذر عرض بيانات الوحدة المحددة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Task _FillFormAsync(int productUnitId)
    {
        if (!_unitsById.TryGetValue(productUnitId, out var unit))
            return Task.CompletedTask;

        _editId = unit.Id;

        _isBinding = true;
        try
        {
            cmbUnit.SelectedValue = unit.UnitId;

            // الوحدة قد تكون غير موجودة في القائمة (معطّلة) — لا نُغيّر التحديد عندها
            if (cmbUnit.SelectedValue is not int selectedUnitId || selectedUnitId != unit.UnitId)
                cmbUnit.SelectedIndex = -1;

            txtBarcode.Text = unit.Barcode ?? "";
            numFactor.Value = _Clamp(numFactor, unit.ConversionFactor);
            numSalePrice.Value = _Clamp(numSalePrice, unit.SalePrice);
            numWholesalePrice.Value = _Clamp(numWholesalePrice, unit.WholesalePrice ?? 0m);
            numCostPrice.Value = _Clamp(numCostPrice, unit.CostPrice);
            numMaxDiscount.Value = _Clamp(numMaxDiscount, unit.MaxDiscountAmount ?? 0m);
        }
        finally
        {
            _isBinding = false;
        }

        errorProvider.Clear();
        _UpdateModeHeader();

        return Task.CompletedTask;
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? $"(( وحدات الصنف )) — {_productName} — تعديل وحدة"
            : $"(( وحدات الصنف )) — {_productName} — إضافة وحدة جديدة";

    private void _ResetForm()
    {
        _editId = null;
        errorProvider.Clear();

        _isBinding = true;
        try
        {
            if (cmbUnit.Items.Count > 0)
                cmbUnit.SelectedIndex = -1;

            txtBarcode.Clear();
            numFactor.Value = 1m;
            numSalePrice.Value = 0m;
            numWholesalePrice.Value = 0m;
            numCostPrice.Value = 0m;
            numMaxDiscount.Value = 0m;
        }
        finally
        {
            _isBinding = false;
        }

        dgvUnits.ClearSelection();
        _UpdateModeHeader();
        cmbUnit.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbUnit.SelectedValue is not int unitId || unitId <= 0)
        {
            errorProvider.SetError(cmbUnit, "يرجى اختيار الوحدة");
            isValid = false;
        }
        else if (_unitsById.Values.Any(u => u.UnitId == unitId && u.Id != (_editId ?? 0)))
        {
            errorProvider.SetError(cmbUnit, "هذه الوحدة مضافة بالفعل لهذا الصنف");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbUnit, "");
        }

        // كل وحدة تُضاف من هذه الشاشة هي وحدة فرعية (الأساسية تُنشأ من شاشة الصنف)
        // لذلك يجب أن يكون معامل التحويل أكبر من 1. لا يُتحقق عند التعديل لأن
        // معامل التحويل غير قابل للتعديل بعد الإضافة.
        if (_editId is null && numFactor.Value <= 1m)
        {
            errorProvider.SetError(numFactor, "معامل التحويل يجب أن يكون أكبر من 1 للوحدات الفرعية");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numFactor, "");
        }

        if (numSalePrice.Value < 0m)
        {
            errorProvider.SetError(numSalePrice, "سعر البيع يجب أن يكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numSalePrice, "");
        }

        if (numCostPrice.Value < 0m)
        {
            errorProvider.SetError(numCostPrice, "سعر التكلفة يجب أن يكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numCostPrice, "");
        }

        if (numMaxDiscount.Value < 0m)
        {
            errorProvider.SetError(numMaxDiscount, "أقصى خصم يجب أن يكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numMaxDiscount, "");
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
        if (cmbUnit.SelectedValue is not int unitId || unitId <= 0) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string? barcode = string.IsNullOrWhiteSpace(txtBarcode.Text)
                ? null
                : txtBarcode.Text.Trim();

            decimal salePrice = numSalePrice.Value;
            decimal costPrice = numCostPrice.Value;
            decimal? wholesalePrice = numWholesalePrice.Value > 0m
                ? numWholesalePrice.Value
                : null;
            decimal? maxDiscount = numMaxDiscount.Value > 0m
                ? numMaxDiscount.Value
                : null;

            Result<ProductUnitDto> result;

            if (_editId is int productUnitId)
            {
                // التعديل يشمل الوحدة والباركود والأسعار فقط
                // (معامل التحويل والوحدة الأساسية لا يتغيران بعد الإضافة).
                var request = new UpdateProductUnitRequest(
                    UnitId: (short)unitId,
                    Barcode: barcode,
                    SalePrice: salePrice,
                    WholesalePrice: wholesalePrice,
                    CostPrice: costPrice,
                    MaxDiscountAmount: maxDiscount);

                result = await ProductService.UpdateProductUnitAsync(_productId, productUnitId, request);
            }
            else
            {
                // الإضافة من هذه الشاشة تُنشئ وحدة فرعية دائماً؛
                // الوحدة الأساسية تُنشأ من شاشة دليل الأصناف.
                var request = new AddProductUnitRequest(
                    UnitId: (short)unitId,
                    Factor: numFactor.Value,
                    IsBaseUnit: false,
                    Barcode: barcode,
                    SalePrice: salePrice,
                    WholesalePrice: wholesalePrice,
                    CostPrice: costPrice,
                    MaxDiscountAmount: maxDiscount);

                result = await ProductService.AddProductUnitAsync(_productId, request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                MessageBox.Show(wasEditing ? "تم تعديل وحدة الصنف بنجاح." : "تمت إضافة وحدة الصنف بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
                await _LoadUnitsAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
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
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int productUnitId)
        {
            MessageBox.Show("يرجى اختيار وحدة من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_unitsById.TryGetValue(productUnitId, out var current) && current.IsBaseUnit)
        {
            MessageBox.Show("لا يمكن حذف الوحدة الأساسية للصنف. يمكنك تعديل أسعارها فقط.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string unitName = _unitsById.TryGetValue(productUnitId, out var unit)
            ? unit.UnitName ?? ""
            : "";

        if (MessageBox.Show($"هل أنت متأكد من حذف الوحدة \"{unitName}\" من هذا الصنف؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await ProductService.DeleteProductUnitAsync(
                _productId, productUnitId, DeleteStrategy.Permanent);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم حذف وحدة الصنف بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
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
            btnDelete.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int productUnitId)
    {
        foreach (DataGridViewRow row in dgvUnits.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == productUnitId)
            {
                _isBinding = true;
                try
                {
                    dgvUnits.ClearSelection();
                    row.Selected = true;
                    dgvUnits.CurrentCell = row.Cells["UnitName"];
                }
                finally
                {
                    _isBinding = false;
                }

                break;
            }
        }
    }

    private static decimal _Clamp(NumericUpDown control, decimal value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);
}
