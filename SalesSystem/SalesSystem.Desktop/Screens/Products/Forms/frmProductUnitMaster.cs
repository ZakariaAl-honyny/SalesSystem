using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Enums;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
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

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmProductUnitMaster()
    {
        InitializeComponent();
    }

    public frmProductUnitMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this);                                  // RTL: يمين/أعلى = السابق، يسار/أسفل = التالي

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete);

        // اختصارات موحّدة (RULE-N47): F2 = جديد · F5 = تحديث · Delete = حذف السجل المحدد · Esc = مسح التصفية.
        // Delete لا يعمل أثناء الكتابة داخل حقل حتى لا يُحذف سجل بالخطأ.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, () =>
            {
                if (btnDelete.Enabled && !_IsTextEditingFocused())
                    btnDelete.PerformClick();
            }),
            (Keys.Escape, _ClearFilters));

        // الفهرس الافتراضي «الكل» = نفس سلوك البحث السابق (RULE-N60).
        _isBinding = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
        }
        finally
        {
            _isBinding = false;
        }
    }

    /// <summary>
    /// هل التركيز الحالي داخل حقل كتابة؟ (يُستخدم لحماية اختصار Delete من حذف سجل أثناء التصفية)
    /// </summary>
    private bool _IsTextEditingFocused()
    {
        Control? focus = ActiveControl;

        while (focus is ContainerControl container
               && container.ActiveControl is { } inner
               && !ReferenceEquals(inner, focus))
        {
            focus = inner;
        }

        return focus is TextBoxBase or NumericUpDown or ComboBox or IDataGridViewEditingControl;
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

    // ── Required fields / focus ───────────────────────────────────────────────

    /// <summary>هل حقول الإدخال المطلوبة مكتملة؟ (الوحدة، ومعامل التحويل عند الإضافة).</summary>
    private bool _RequiredFieldsValid(bool edit)
    {
        bool unitOk = cmbUnit.SelectedValue is int unitId && unitId > 0;

        // معامل التحويل مطلوب فقط عند الإضافة (لا يتغير بعد الإضافة).
        bool factorOk = edit || numFactor.Value > 1m;

        return unitOk && factorOk;
    }

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    private bool _FocusFirstMissingField(bool edit)
    {
        if (cmbUnit.SelectedValue is not int unitId || unitId <= 0)
        {
            errorProvider.SetError(cmbUnit, "يرجى اختيار الوحدة");
            cmbUnit.Focus();
            return false;
        }

        if (!edit && numFactor.Value <= 1m)
        {
            errorProvider.SetError(numFactor, "معامل التحويل يجب أن يكون أكبر من 1 للوحدات الفرعية");
            numFactor.Focus();
            return false;
        }

        return true;
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmProductUnitMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadUnitLookupAsync();
            await _LoadUnitsAsync();
            _ResetForm();

            ApplyGreenTheme(); // الثيم الأخضر آخر خطوة في مسار التحميل (RULE-N50)
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

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.
    /// </summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvUnits);

        // نص شريط الملخص: أخضر داكن مميّز (RULE-N53) — القيمة من الـ Helper لا من الشاشة (RULE-N54)
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً
    }

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
        table.Columns.Add("WholesalePrice", typeof(decimal));
        table.Columns.Add("CostPrice", typeof(decimal));
        table.Columns.Add("MaxDiscountAmount", typeof(decimal));
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
                        unit.WholesalePrice ?? 0m,
                        unit.CostPrice,
                        unit.MaxDiscountAmount ?? 0m,
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

            _ApplyGridHeaders();
            _ApplyFilter();
            _ApplyRowColoring();

            // الحفاظ على الوحدة المحددة إن كانت لا تزال موجودة
            if (_editId is int keepId && _unitsById.ContainsKey(keepId))
                _SelectGridRow(keepId);
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يستخدم DataGridView عنوان <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvUnits.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم الوحدة",
                "UnitName" => "الوحدة",
                "Barcode" => "الباركود",
                "ConversionFactor" => "معامل التحويل",
                "SalePrice" => "سعر البيع",
                "WholesalePrice" => "سعر الجملة",
                "CostPrice" => "سعر التكلفة",
                "MaxDiscountAmount" => "أقصى خصم",
                "IsBaseUnit" => "الوحدة الأساسية",
                _ => column.HeaderText
            };
        }
    }

    private void _ApplyRowColoring()
    {
        if (!dgvUnits.Columns.Contains("IsBaseUnit")) return;

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

    // ── Search / Filter (DataView.RowFilter — RULE-N56…RULE-N62) ───────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyBaseUnit_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyWithBarcode_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter في حقل التصفية يطبّق فوراً (RULE-N59).</summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        _ApplyFilter();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    /// <summary>
    /// يمنع إدخال علامات الترقيم (مثل ' و % و [ ]) التي تفسد فلتر البحث DataView.RowFilter.
    /// </summary>
    private void txtSearch_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(txtFilterValue, "لا يُسمح بإدخال علامات الترقيم.");
        }
    }

    /// <summary>
    /// تصفية فورية (بلا زر بحث): يُبنى الفلتر من أسماء أعمدة معروفة حسب اختيار
    /// <c>cbFilterBy</c>، وتُهرَّب القيم المدخلة (RULE-N60).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_unitsView is null) return;

        string text = _EscapeFilterValue(txtFilterValue.Text.Trim());

        var conditions = new List<string>();

        if (!string.IsNullOrEmpty(text))
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 1: // اسم الوحدة
                    conditions.Add($"UnitName LIKE '%{text}%'");
                    break;

                case 2: // الباركود
                    conditions.Add($"Barcode LIKE '%{text}%'");
                    break;

                case 3: // رقم الوحدة
                    conditions.Add($"CONVERT(Id, 'System.String') LIKE '{text}%'");
                    break;

                default: // الكل — نفس سلوك البحث السابق
                    conditions.Add($"(UnitName LIKE '%{text}%' OR Barcode LIKE '%{text}%' " +
                                   $"OR CONVERT(Id, 'System.String') LIKE '{text}%')");
                    break;
            }
        }

        if (chkOnlyBaseUnit.Checked)
            conditions.Add("IsBaseUnit = true");

        if (chkOnlyWithBarcode.Checked)
            conditions.Add("Barcode <> ''");

        _unitsView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";

        _UpdateSummary();
    }

    /// <summary>
    /// يهرّب قيمة التصفية قبل حقنها في <see cref="DataView.RowFilter"/>:
    /// حذف الأقواس المربعة (رموز خاصة في RowFilter) وتضعيف علامة الاقتباس (RULE-N60).
    /// </summary>
    private static string _EscapeFilterValue(string value)
        => value
            .Replace("[", "")
            .Replace("]", "")
            .Replace("'", "''");

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة كاملة (Esc — RULE-N59).</summary>
    private void _ClearFilters()
    {
        _isBinding = true;
        try
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Clear();
            chkOnlyBaseUnit.Checked = false;
            chkOnlyWithBarcode.Checked = false;
        }
        finally
        {
            _isBinding = false;
        }

        _ApplyFilter();
    }

    /// <summary>
    /// شريط الملخص (RULE-N61): عدد المعروض من الإجمالي + عدد الوحدات الأساسية والفرعية.
    /// يُعاد حسابه بعد كل تصفية وكل تحديث للبيانات.
    /// </summary>
    private void _UpdateSummary()
    {
        int shown = _unitsView?.Count ?? 0;
        int total = _unitsTable?.Rows.Count ?? 0;

        int baseUnits = 0;
        if (_unitsTable.Columns.Contains("IsBaseUnit"))
        {
            foreach (DataRow row in _unitsTable.Rows)
            {
                if (row["IsBaseUnit"] is bool isBase && isBase)
                    baseUnits++;
            }
        }

        lblSummary.Text = $"المعروض: {shown} من {total} — أساسية: {baseUnits} · فرعية: {total - baseUnits}";
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

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;

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

    // ── Mode header / Reset ───────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? $"(( وحدات الصنف )) — {_productName} — تعديل وحدة"
            : $"(( وحدات الصنف )) — {_productName} — إضافة وحدة جديدة";

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الإضافة متاح، وزرّا التعديل والحذف معطّلان لعدم وجود سجل محدد.
        // نصوص الأزرار ثابتة في المصمم — لا تُبدَّل حسب الوضع.
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;

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

        _isBinding = true;
        try
        {
            dgvUnits.ClearSelection();
        }
        finally
        {
            _isBinding = false;
        }

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

    /// <summary>زر الخروج: إغلاق شاشة وحدات الصنف.</summary>
    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    /// <summary>زر الإضافة: يُضيف وحدة جديدة دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث الوحدة المحددة فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>تحديث يدوي لقائمة وحدات الصنف مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadUnitsAsync();
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

    /// <summary>اسم الوحدة الحالية لعرضه في رسالة التأكيد.</summary>
    private string _CurrentUnitName()
        => _editId is int id && _unitsById.TryGetValue(id, out var unit)
            ? unit.UnitName ?? ""
            : cmbUnit.Text ?? "";

    /// <summary>
    /// حفظ وحدة الصنف: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقول المطلوبة الناقصة: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_RequiredFieldsValid(edit))
        {
            _FocusFirstMissingField(edit);
            return;
        }

        if (!_ValidateForm()) return;
        if (cmbUnit.SelectedValue is not int unitId || unitId <= 0) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = _CurrentUnitName();
            if (MessageBox.Show($"هل أنت متأكد من تعديل وحدة المنتج \"{editName}\"؟",
                    "تأكيد التعديل", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
        btnEdit.Enabled = false;

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

            if (edit)
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

                result = await ProductService.UpdateProductUnitAsync(_productId, _editId!.Value, request);
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
                MessageBox.Show(edit ? "تم تعديل وحدة الصنف بنجاح." : "تمت إضافة وحدة الصنف بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). النصوص ثابتة في المصمم.
                _editId = result.Value.Id;
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

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
            // استعادة حالة الأزرار حسب الوضع الحالي للنموذج.
            if (_editId is null)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }

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

        string unitName = _CurrentUnitName();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) وحدة المنتج \"{unitName}\"؟",
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
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زرّا الحذف والتعديل معطّلين.
            btnDelete.Enabled = _editId.HasValue;
            btnEdit.Enabled = _editId.HasValue;
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int productUnitId)
    {
        if (!dgvUnits.Columns.Contains("UnitName")) return;

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
