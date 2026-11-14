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

    private IProductDesktopService ProductService => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IUnitDesktopService UnitService => _serviceProvider.GetRequiredService<IUnitDesktopService>();

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

    /// <summary>
    /// حالة الشاشة (آلة الحالة): تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>
    /// حيث كل حقول الإدخال **معطّلة**، ولا تُفعَّل إلا بزر «جديد» أو باختيار سجل من الشبكة (RULE-N149).
    /// </summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( وحدات الصنف ))";

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N43/RULE-N44/RULE-N150).</summary>
    private enum EntryMode
    {
        /// <summary>عرض فقط: كل حقول الإدخال معطّلة حتى يضغط المستخدم «جديد» أو يحدّد سجلاً (RULE-N149).</summary>
        Browsing,

        /// <summary>إضافة وحدة جديدة: الحقول مفعّلة وزر «إضافة» متاح فقط.</summary>
        Adding,

        /// <summary>تعديل وحدة محددة: الحقول مفعّلة وأزرار «تعديل/حذف» متاحة.</summary>
        Editing
    }

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

        // أزرار الوضع (إضافة/تعديل/حذف) وزر «مسح التصفية» تتغير حالة تفعيلها،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnClearFilters); // RULE-N43

        // اختصارات موحّدة (RULE-N47): F2 = جديد · F5 = تحديث · Delete = حذف السجل المحدد · Esc = مسح التصفية/إلغاء.
        // Delete لا يعمل أثناء الكتابة داخل حقل حتى لا يُحذف سجل بالخطأ.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, () =>
            {
                if (btnDelete.Enabled && !_IsTextEditingFocused())
                    btnDelete.PerformClick();
            }),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع الحالي: عرض ⇒ «جديد» · إضافة ⇒ «إضافة» · تعديل ⇒ «تعديل» (RULE-N147).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

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
        FilterPanelHelper.Attach(this, _serviceProvider);
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

    /// <summary>Esc: مسح كل التصفية إن كان المؤشر داخل صندوق التصفية، وإلا إلغاء النموذج والعودة لوضع العرض (RULE-N47).</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        _ResetForm();   // ينهي الإدخال ويعيد الحقول إلى وضع العرض
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
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
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

        GreenThemeHelper.Apply(this); // الثيم الأخضر آخر سطر دائماً (RULE-N50)
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
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة الوحدات.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الوحدات");
            clsDialogs.Error("تعذر تحميل قائمة الوحدات. يرجى إعادة المحاولة.");
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
                clsDialogs.Error(result.Error ?? "تعذر تحميل وحدات الصنف.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل وحدات الصنف {ProductId}", _productId);
            clsDialogs.Error("تعذر تحميل وحدات الصنف. يرجى إعادة المحاولة.");
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

    /// <summary>Enter في حقل التصفية يطبّق فوراً · Esc يمسح كل أدوات التصفية (RULE-N59).</summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _ApplyFilter();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        else if (e.KeyCode == Keys.Escape)
        {
            _ClearAllFilters();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
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

        string text = txtFilterValue.Text.Trim();

        var conditions = new List<string>();

        if (!string.IsNullOrEmpty(text))
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 1: // اسم الوحدة
                    conditions.Add(RowFilterHelper.Like("UnitName", text));
                    break;

                case 2: // الباركود
                    conditions.Add(RowFilterHelper.Like("Barcode", text));
                    break;

                case 3: // رقم الوحدة — بحث بادئي على الرقم (نفس السلوك السابق، بقيمة مُهرَّبة)
                    conditions.Add($"CONVERT(Id, 'System.String') LIKE '{RowFilterHelper.Escape(text)}%'");
                    break;

                default: // الكل — نفس سلوك البحث السابق
                    conditions.Add("(" + RowFilterHelper.Like("UnitName", text)
                                   + " OR " + RowFilterHelper.Like("Barcode", text)
                                   + " OR CONVERT(Id, 'System.String') LIKE '" + RowFilterHelper.Escape(text) + "%')");
                    break;
            }
        }

        if (chkOnlyBaseUnit.Checked)
            conditions.Add("IsBaseUnit = true");

        if (chkOnlyWithBarcode.Checked)
            conditions.Add("Barcode <> ''");

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _unitsView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية وحدات الصنف — أُبقيت التصفية السابقة كما هي");
        }

        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث» (RULE-N148).</summary>
    private void _ClearFilterInputs()
    {
        txtFilterValue.Clear();
        chkOnlyBaseUnit.Checked = false;
        chkOnlyWithBarcode.Checked = false;
        cbFilterBy.SelectedIndex = 0;
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة كاملة (Esc — RULE-N59/RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        _ApplyFilter();
    }

    /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || chkOnlyBaseUnit.Checked
            || chkOnlyWithBarcode.Checked
            || cbFilterBy.SelectedIndex > 0;
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
            clsDialogs.Error("تعذر عرض بيانات الوحدة المحددة. يرجى إعادة المحاولة.");
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
            clsDialogs.Error("تعذر عرض بيانات الوحدة المحددة. يرجى إعادة المحاولة.");
        }
    }

    private Task _FillFormAsync(int productUnitId)
    {
        if (!_unitsById.TryGetValue(productUnitId, out var unit))
            return Task.CompletedTask;

        _editId = unit.Id;
        _mode = EntryMode.Editing;   // RULE-N151: اختيار سجل من الشبكة = وضع التعديل

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

        // بعد تعبئة الحقول: تفعيلها + ضبط الأزرار + تحديث شريط الوضع (آلة حالة واحدة).
        _ApplyModeState();
        return Task.CompletedTask;
    }

    // ── Actions state (RULE-N43 / RULE-N44 / RULE-N150) ───────────────────────

    /// <summary>
    /// آلة الحالة المركزية للشاشة: تفعيل/تعطيل حقول الإدخال، وحالة كل زر، ونص شريط الوضع،
    /// والزر الافتراضي للشاشة (`AcceptButton`). تُستدعى بعد **كل** انتقال حالة:
    /// تحميل · «جديد» · اختيار سجل · حفظ ناجح · حذف · «تحديث» · Esc (RULE-N150).
    /// الحقول تبدأ معطّلة، ولا تُفعَّل إلا في وضعي الإضافة والتعديل (RULE-N149).
    /// </summary>
    private void _ApplyModeState()
    {
        bool editingSession = _mode != EntryMode.Browsing;

        _SetEditorEnabled(editingSession);

        // معامل التحويل يُعدَّل في وضع الإضافة فقط (لا يتغيّر بعد الإضافة) — للعرض عند التعديل.
        numFactor.Enabled = _mode == EntryMode.Adding;

        // آلة حالة الأزرار (RULE-N43): «جديد/إضافة» في الإضافة، «تعديل/حذف» عند وجود سجل محدد.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnSave.Enabled = _mode == EntryMode.Adding;
        btnEdit.Enabled = _mode == EntryMode.Editing;
        btnDelete.Enabled = _mode == EntryMode.Editing;

        _UpdateModeHeader();   // RULE-N44

        // RULE-N147: Enter يُنفِّذ الزر الأساسي للوضع الحالي (ويُبرزه كزر افتراضي للشاشة).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): عرض ⇒ «جديد» · إضافة ⇒ «إضافة» · تعديل ⇒ «تعديل».
    /// يُقرأ عند كل ضغطة Enter وعند كل انتقال حالة.
    /// </summary>
    private Button? _PrimaryActionButton() => _mode switch
    {
        EntryMode.Adding => btnSave,
        EntryMode.Editing => btnEdit,
        _ => btnNew
    };

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند تحديد سجل.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        cmbUnit, txtBarcode, numFactor,
        numSalePrice, numWholesalePrice, numCostPrice, numMaxDiscount
    };

    /// <summary>تفعيل أو تعطيل كل حقول المحرر دفعة واحدة.</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    // ── Mode header / Reset ───────────────────────────────────────────────────

    /// <summary>عنوان الشاشة الأساسي مع اسم الصنف (إن كان معروفاً).</summary>
    private string _Title => string.IsNullOrWhiteSpace(_productName)
        ? FormTitle
        : $"{FormTitle} — {_productName}";

    private string _CurrentUnitName()
        => _editId is int id && _unitsById.TryGetValue(id, out var unit)
            ? unit.UnitName ?? ""
            : cmbUnit.Text ?? "";

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة وحدة جديدة» · «تعديل وحدة: &lt;الاسم&gt;».
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{_Title} — إضافة وحدة جديدة",
            EntryMode.Editing => $"{_Title} — تعديل وحدة: {_CurrentUnitName()}",
            _ => $"{_Title} — وضع العرض"
        };
    }

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض** (الحالة الافتراضية): تفريغ الحقول، تعطيل كل حقول الإدخال،
    /// وإتاحة زر «جديد» فقط (RULE-N149).
    /// </summary>
    private void _ResetForm()
    {
        _editId = null;
        _mode = EntryMode.Browsing;

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

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>
    /// بدء إدخال وحدة جديدة (زر «جديد» / F2): تفريغ الحقول ثم **تفعيلها** وتحويل شريط الوضع
    /// إلى «إضافة وحدة جديدة» (RULE-N151).
    /// </summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

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
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

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

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← الدلائل (الوحدات) ← وحدات الصنف ← المحرر ووضع العرض.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // التصفية تعود للافتراضي
            await _LoadUnitLookupAsync();  // قد تتغيّر من شاشة الوحدات
            await _LoadUnitsAsync();       // إعادة قراءة وحدات الصنف (يعيد تطبيق التصفية والملخّص)
            _ResetForm();                  // تفريغ المحرر + وضع العرض + الحقول معطّلة
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث البيانات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

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
            if (!clsDialogs.ConfirmEdit($"هل أنت متأكد من تعديل وحدة المنتج \"{editName}\"؟"))
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
                clsDialogs.Success(edit ? "تم تعديل وحدة الصنف بنجاح." : "تمت إضافة وحدة الصنف بنجاح.");

                // بعد الحفظ الناجح (إضافة أو تعديل): يصبح السجل محفوظاً ومحدداً → وضع التعديل،
                // ثم تحديث الشبكة وإعادة تعبئة الحقول من البيانات المحفوظة (تحديث تلقائي — RULE-N151).
                _editId = result.Value.Id;
                _mode = EntryMode.Editing;

                await _LoadUnitsAsync();
                await _FillFormAsync(result.Value.Id);   // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _ApplyModeState();                       // ضمان الحالة حتى لو لم يُعثر على السطر
                _SelectGridRow(result.Value.Id);
            }
            else
            {
                clsDialogs.Error($"لم يتم حفظ وحدة الصنف.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ وحدة الصنف {ProductId}", _productId);
            clsDialogs.Error("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.");
        }
        finally
        {
            // استعادة الحالة الكاملة (حقول + أزرار + شريط الوضع) حسب الوضع الحالي للنموذج.
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int productUnitId)
        {
            clsDialogs.Warning("يرجى اختيار وحدة من القائمة أولاً.");
            return;
        }

        if (_unitsById.TryGetValue(productUnitId, out var current) && current.IsBaseUnit)
        {
            clsDialogs.Warning("لا يمكن حذف الوحدة الأساسية للصنف. يمكنك تعديل أسعارها فقط.");
            return;
        }

        string unitName = _CurrentUnitName();

        if (!clsDialogs.Confirm($"هل أنت متأكد من حذف (تعطيل) وحدة المنتج \"{unitName}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await ProductService.DeleteProductUnitAsync(
                _productId, productUnitId, DeleteStrategy.Permanent);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم حذف وحدة الصنف بنجاح.");

                // بعد الحذف: الرجوع إلى وضع العرض (الحقول معطّلة) ثم تحديث الشبكة تلقائياً.
                _ResetForm();
                await _LoadUnitsAsync();
                _ApplyModeState();   // يضمن بقاء الحقول معطّلة بعد إعادة التحميل
            }
            else
            {
                clsDialogs.Error($"لم يتم حذف وحدة الصنف.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حذف وحدة الصنف {ProductUnitId}", productUnitId);
            clsDialogs.Error("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.");
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع العرض → «جديد» فقط هو المتاح.
            _ApplyModeState();
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
