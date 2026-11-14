using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Warehouses.Forms;

/// <summary>
/// شاشة موحّدة (Master Form) لدليل المخازن.
/// تجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// وتعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmWarehouseMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IWarehouseDesktopService WarehouseService => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, WarehouseDto> _warehousesById = new();
    private DataTable _warehousesTable = new();
    private DataView _warehousesView = new();

    /// <summary>null = إضافة مخزن جديد، &gt; 0 = تعديل مخزن قائم.</summary>
    private int? _editId;

    /// <summary>
    /// حالة الشاشة (آلة الحالة): تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>
    /// حيث كل حقول الإدخال **معطّلة**، ولا تُفعَّل إلا بزر «جديد» أو باختيار سجل من الشبكة (RULE-N149).
    /// </summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي (RULE-N44).</summary>
    private const string FormTitle = "(( دليل المخازن ))";

    /// <summary>
    /// عملية إعادة تنشيط جارية (RULE-N92): يُعطّل زر «إعادة تنشيط» أثناء التنفيذ
    /// ويمنع الضغط المزدوج الذي يُرسل طلبين متتاليين.
    /// </summary>
    private bool _reactivateBusy;

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N149…N152).</summary>
    private enum EntryMode
    {
        /// <summary>عرض فقط: كل حقول الإدخال معطّلة حتى يضغط المستخدم «جديد» أو يحدّد سجلاً.</summary>
        Browsing,

        /// <summary>إضافة سجل جديد: الحقول مفعّلة وزر «إضافة» متاح فقط.</summary>
        Adding,

        /// <summary>تعديل سجل محدد: الحقول مفعّلة وأزرار «تعديل/حذف/تعيين افتراضي» متاحة.</summary>
        Editing
    }

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmWarehouseMaster()
    {
        InitializeComponent();
    }

    public frmWarehouseMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate, btnSetDefault, btnClearFilters); // RULE-N43

        // الاختصارات الموحّدة (RULE-N47): F2 جديد · F5 تحديث · Delete تعطيل · Esc مسح التصفية/إلغاء.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDelete),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع الحالي: عرض ⇒ «جديد» · إضافة ⇒ «إضافة/حفظ» · تعديل ⇒ «تعديل» (RULE-N147).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N60: أدوات التصفية تُقرأ بالمؤشر — «الحالة: الكل» و«تصفية بـ: الكل» افتراضياً.
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── Global Shortcuts ──────────────────────────────────────────────────────

    /// <summary>Delete: تعطيل السجل المحدد — يُتجاهل أثناء الكتابة داخل أي حقل إدخال.</summary>
    private void _GlobalDelete()
    {
        if (_IsTextInputFocused()) return;
        if (btnDelete.Enabled) btnDelete.PerformClick();
    }

    /// <summary>Esc: مسح كل التصفية إن كان المؤشر داخل صندوق التصفية، وإلا إلغاء النموذج (RULE-N47).</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        _ResetForm();
    }

    /// <summary>هل المؤشر داخل حقل إدخال؟ (لئلا يُنفَّذ اختصار Delete أثناء الكتابة).</summary>
    private bool _IsTextInputFocused()
    {
        foreach (Control input in new Control[]
                 {
                     txtFilterValue, cbFilterBy, cbStatus,
                     txtCode, txtName, txtPhone, txtAddress, txtNotes
                 })
        {
            if (input.Focused) return true;
        }

        return false;
    }

    // ── Validation ────────────────────────────────────────────────────────────

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    /// <returns><c>true</c> إذا كانت الحقول المطلوبة مكتملة، و<c>false</c> بعد إبراز أول حقل ناقص.</returns>
    private bool _FocusFirstMissingField(bool edit)
    {
        errorProvider.SetError(txtName, "");

        if (!string.IsNullOrWhiteSpace(txtName.Text)) return true;

        errorProvider.SetError(txtName, "اسم المخزن مطلوب");
        txtName.Focus();
        return false;
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← المخازن ← المحرر ووضع العرض. وليس بديلاً عن «مسح التصفية».
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // التصفية تعود للافتراضي
            await _LoadWarehousesAsync();  // إعادة قراءة المخازن (يعيد تطبيق التصفية والملخّص)
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

    /// <summary>زر الخروج: إغلاق شاشة دليل المخازن.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmWarehouseMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadWarehousesAsync();
            _ResetForm();
            ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل المخازن");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvWarehouses);
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه)
        dgvWarehouses.ReadOnly = true;
        dgvWarehouses.AllowUserToAddRows = false;
        dgvWarehouses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvWarehouses.MultiSelect = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadWarehousesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("Address", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await WarehouseService.GetAllAsync(includeInactive: true);

            _warehousesById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var w in result.Value)
                {
                    _warehousesById[w.Id] = w;
                    table.Rows.Add(
                        (int)w.Id,
                        w.Name,
                        w.Phone ?? "",
                        w.Address ?? "",
                        w.IsActive);
                }
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة المخازن.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة المخازن");
            clsDialogs.Error("تعذر تحميل قائمة المخازن. يرجى إعادة المحاولة.");
        }
        finally
        {
            _warehousesTable = table;
            _warehousesView = new DataView(_warehousesTable);
            dgvWarehouses.DataSource = _warehousesView;
            _ApplyGridHeaders();
            _ApplyFilter();   // التصفية + التلوين + شريط الملخص (RULE-N61)
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يستخدم DataGridView عنوان <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvWarehouses.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم المخزن",
                "Name" => "اسم المخزن",
                "Phone" => "الهاتف",
                "Address" => "العنوان",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvWarehouses.Rows)
        {
            if (row.DataBoundItem is not DataRowView dataRow) continue;

            bool isActive = dataRow["IsActive"] is bool b && b;
            if (isActive) continue;

            row.DefaultCellStyle.BackColor = Color.MistyRose;
            row.DefaultCellStyle.ForeColor = Color.DarkRed;
        }
    }

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtFilterValue_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter يطبّق التصفية فوراً · Esc يمسح كل أدوات التصفية (RULE-N59).</summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _ApplyFilter();
            e.Handled = true;
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
    private void txtFilterValue_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsPunctuation(e.KeyChar))
        {
            e.Handled = true;
            clsInputFeedback.ShowRejectTooltip(txtFilterValue, "لا يُسمح بإدخال علامات الترقيم.");
        }
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

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تحميل ولا لمس للمحرر) — تُستهلك من «مسح» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        txtFilterValue.Clear();
        chkShowInactive.Checked = false;
        cbFilterBy.SelectedIndex = 0;   // «الكل» = بلا تصفية
        cbStatus.SelectedIndex = 0;     // «الكل» = بلا تصفية
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية (RULE-N59).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        _ApplyFilter();
    }

    /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        if (btnClearFilters is null) return;

        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || chkShowInactive.Checked
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0;
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59/N60): كل الشروط تُبنى من أسماء أعمدة معروفة،
    /// والاختيار يُقرأ بالمؤشر لا بالنص العربي، والقيمة تُهرَّب قبل بنائها في RowFilter.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_warehousesView is null || _warehousesView.Table is null) return;
        if (txtFilterValue is null || cbFilterBy is null || cbStatus is null || chkShowInactive is null)
            return;

        string text = txtFilterValue.Text.Trim();

        var conditions = new List<string>();

        // ── الحالة: الكل / نشط / معطّل (cbStatus بالمؤشر) ──
        switch (cbStatus.SelectedIndex)
        {
            case 1: // نشط
                conditions.Add("IsActive = true");
                break;

            case 2: // معطّل — يُظهر المعطّل حتى لو لم يُفعَّل «عرض غير النشط»
                conditions.Add("IsActive = false");
                break;

            default: // الكل — يبقى «عرض غير النشط» هو الحاكم كما كان
                if (!chkShowInactive.Checked)
                    conditions.Add("IsActive = true");
                break;
        }

        // ── البحث النصي: عمود معروف واحد، أو مجموعة أعمدة ثابتة عند اختيار «الكل» ──
        if (text.Length > 0)
        {
            string? column = _FilterColumn();
            conditions.Add(column is null
                ? "(" + RowFilterHelper.Like("Name", text)
                    + " OR " + RowFilterHelper.Like("Address", text)
                    + " OR " + RowFilterHelper.Like("Phone", text) + ")"
                : RowFilterHelper.Like(column, text));
        }

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _warehousesView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية دليل المستودعات — أُبقيت التصفية السابقة كما هي");
        }

        _ApplyRowColoring();
        _UpdateSummary();
        _UpdateClearFiltersState();   // RULE-N148: حالة زر «مسح التصفية» تتبع وجود تصفية نشطة
    }

    /// <summary>عمود البحث المقابل لاختيار «تصفية بـ» — أسماء أعمدة ثابتة داخل الكود (RULE-N60).</summary>
    private string? _FilterColumn() => cbFilterBy.SelectedIndex switch
    {
        1 => "Name",
        2 => "Address",
        3 => "Phone",
        _ => null   // «الكل» — بحث في الاسم والعنوان والهاتف
    };

    /// <summary>
    /// شريط الملخص (RULE-N61): المعروض من الإجمالي + عدد النشط والمعطّل،
    /// ويُعاد حسابه بعد كل تصفية وكل تحميل.
    /// </summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null || _warehousesView?.Table is null) return;

        int total = _warehousesTable.Rows.Count;
        int shown = _warehousesView.Count;

        int active = 0, inactive = 0;

        foreach (DataRowView row in _warehousesView)
        {
            if (row["IsActive"] is bool isActive && isActive) active++;
            else inactive++;
        }

        lblSummary.Text = $"المعروض: {shown} من {total} — نشط: {active} · معطّل: {inactive}";

        // «لا توجد بيانات» يُعلن في شريط الملخص بدل إضافة صف وهمي إلى الشبكة
        // (الصف الوهمي يُفسد التصدير والتصفية — RULE-N22 / RULE-N61).
        if (shown == 0) lblSummary.Text += " — لا توجد بيانات مطابقة";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvWarehouses_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvWarehouses.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مخزن من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات المخزن المحدد. يرجى إعادة المحاولة.");
        }
    }

    private void _FillForm(int warehouseId)
    {
        if (!_warehousesById.TryGetValue(warehouseId, out var w)) return;

        _editId = warehouseId;
        _mode = EntryMode.Editing;   // RULE-N151: اختيار سجل من الشبكة = وضع التعديل

        errorProvider.Clear();

        txtCode.Text = warehouseId.ToString();
        txtName.Text = w.Name;
        txtPhone.Text = w.Phone ?? "";
        txtAddress.Text = w.Address ?? "";
        txtNotes.Text = w.Notes ?? "";

        // بعد تعبئة الحقول: تفعيلها + ضبط الأزرار + تحديث شريط الوضع.
        _ApplyModeState();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    /// <summary>
    /// آلة الحالة المركزية للشاشة (RULE-N150): تفعيل/تعطيل حقول الإدخال، وحالة كل زر، ونص شريط الوضع،
    /// والزر الافتراضي (Enter). تُستدعى بعد **كل** انتقال حالة: تحميل · «جديد» · اختيار سجل ·
    /// حفظ ناجح · تعطيل · «تحديث» · Esc.
    /// </summary>
    private void _ApplyModeState()
    {
        _SetEditorEnabled(_mode != EntryMode.Browsing);

        // آلة حالة الأزرار (RULE-N43): «جديد» في العرض، «إضافة» في الإضافة، «تعديل/حذف/افتراضي» عند وجود سجل محدد.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnSave.Enabled = _mode == EntryMode.Adding;
        btnEdit.Enabled = _mode == EntryMode.Editing;
        btnDelete.Enabled = _mode == EntryMode.Editing;
        btnSetDefault.Enabled = _mode == EntryMode.Editing;

        _UpdateReactivateButton();   // RULE-N92: إعادة التنشيط للسجل المعطّل فقط
        _UpdateModeHeader();         // RULE-N44

        // RULE-N147: Enter يُنفِّذ الزر الأساسي للوضع الحالي (ويُبرزه كزر افتراضي للشاشة).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): عرض ⇒ «جديد» · إضافة ⇒ «إضافة/حفظ» · تعديل ⇒ «تعديل».
    /// يُقرأ عند كل ضغطة Enter وعند كل انتقال حالة.
    /// </summary>
    private Button? _PrimaryActionButton() => _mode switch
    {
        EntryMode.Adding => btnSave,
        EntryMode.Editing => btnEdit,
        _ => btnNew
    };

    /// <summary>
    /// آلة حالة زر «إعادة تنشيط» (RULE-N92/RULE-N122): مفعّل فقط في وضع التعديل عند تحديد سجل
    /// **معطّل**، ولا عملية إعادة تنشيط جارية؛ ومعطّل في وضع العرض/الإضافة أو عند سجل نشط.
    /// </summary>
    private void _UpdateReactivateButton()
    {
        if (btnReactivate is null) return;

        bool hasSelection = _mode == EntryMode.Editing && _editId.HasValue;
        bool isActive = hasSelection
            && _warehousesById.TryGetValue(_editId!.Value, out var warehouse)
            && warehouse.IsActive;

        btnReactivate.Enabled = hasSelection && !isActive && !_reactivateBusy;   // RULE-N92
    }

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند تحديد سجل.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        txtName, txtPhone, txtAddress, txtNotes
    };

    /// <summary>تفعيل أو تعطيل كل حقول المحرر دفعة واحدة (txtCode يبقى للقراءة دائماً).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة مخزن جديد» · «تعديل مخزن: &lt;الاسم&gt;».
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — إضافة مخزن جديد",
            EntryMode.Editing => $"{FormTitle} — تعديل مخزن: {txtName.Text.Trim()}",
            _ => $"{FormTitle} — وضع العرض"
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

        txtCode.Clear();
        txtName.Clear();
        txtPhone.Clear();
        txtAddress.Clear();
        txtNotes.Clear();

        dgvWarehouses.ClearSelection();

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>
    /// بدء إدخال مخزن جديد (زر «جديد» / F2): تفريغ الحقول ثم **تفعيلها** وتحويل شريط الوضع
    /// إلى «إضافة مخزن جديد» (RULE-N151).
    /// </summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        txtName.Focus();
    }

    // ── Validation (مكمّل) ────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المخزن مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    /// <summary>زر الإضافة: يُضيف مخزناً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المخزن المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ المخزن: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقول المطلوبة الناقصة: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_FocusFirstMissingField(edit)) return;

        if (!_ValidateForm()) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = txtName.Text.Trim();
            if (!clsDialogs.ConfirmEdit($"هل أنت متأكد من تعديل المخزن \"{editName}\"؟"))
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
        btnEdit.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string? phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            string? address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
            string? notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            Result<WarehouseDto> result;

            if (edit)
            {
                // الحفاظ على حالة التنشيط الحالية للمخزن أثناء التعديل (RULE-N90/N92):
                // مخزن معطّل لا يُعاد تنشيطه صامتاً بفعل الحفظ — إعادة التنشيط من شاشتها المخصصة.
                bool isActive = _warehousesById.TryGetValue(_editId!.Value, out var current)
                    ? current.IsActive
                    : true;

                var request = new UpdateWarehouseRequest(name, phone, address, notes, IsActive: isActive);
                result = await WarehouseService.UpdateAsync(_editId!.Value, request);
            }
            else
            {
                var request = new CreateWarehouseRequest(name, phone, address, notes);
                result = await WarehouseService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new WarehouseChangedMessage(result.Value.Id));

                clsDialogs.Success(edit ? "تم تعديل المخزن بنجاح." : "تمت إضافة المخزن بنجاح.");

                // بعد الحفظ الناجح (إضافة أو تعديل): يصبح السجل محفوظاً ومحدداً → وضع التعديل،
                // ثم تحديث القائمة وإعادة تعبئة الحقول من البيانات المحفوظة (تحديث تلقائي — RULE-N151).
                _editId = result.Value.Id;
                _mode = EntryMode.Editing;

                await _LoadWarehousesAsync();
                _FillForm(result.Value.Id);   // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _ApplyModeState();            // ضمان الحالة حتى لو لم يُعثر على السطر
                _SelectGridRow(result.Value.Id);
            }
            else
            {
                clsDialogs.Error($"لم يتم حفظ المخزن.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ المخزن");
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
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مخزن من القائمة أولاً.");
            return;
        }

        string name = txtName.Text.Trim();

        if (!clsDialogs.Confirm($"هل أنت متأكد من حذف (تعطيل) المخزن \"{name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            // لا توجد عملية حذف مخصصة في IWarehouseDesktopService — التعطيل يتم عبر Update مع IsActive = false.
            var current = await WarehouseService.GetByIdAsync(id);
            if (!current.IsSuccess || current.Value is null)
            {
                clsDialogs.Error($"لم يتم تعطيل المخزن.\n{current.Error}");
                return;
            }

            var w = current.Value;
            var request = new UpdateWarehouseRequest(w.Name, w.Phone, w.Address, w.Notes, IsActive: false);

            var result = await WarehouseService.UpdateAsync(id, request);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعطيل المخزن بنجاح.");

                EventBus.Publish(new WarehouseChangedMessage(id));

                // بعد الحذف: الرجوع إلى وضع العرض (الحقول معطّلة) ثم تحديث القائمة تلقائياً.
                _ResetForm();
                await _LoadWarehousesAsync();
                _ApplyModeState();   // يضمن بقاء الحقول معطّلة بعد إعادة التحميل
            }
            else
            {
                clsDialogs.Error($"لم يتم تعطيل المخزن.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل المخزن {WarehouseId}", id);
            clsDialogs.Error("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.");
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع العرض → «جديد» فقط هو المتاح.
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    private async void btnSetDefault_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مخزن من القائمة أولاً.");
            return;
        }

        string name = txtName.Text.Trim();

        if (!clsDialogs.Confirm($"هل تريد تعيين المخزن \"{name}\" كمخزن افتراضي؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnSetDefault.Enabled = false;

        try
        {
            var result = await WarehouseService.SetDefaultAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعيين المخزن الافتراضي بنجاح.");

                EventBus.Publish(new WarehouseChangedMessage(id));
            }
            else
            {
                clsDialogs.Error($"لم يتم تعيين المخزن الافتراضي.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعيين المخزن الافتراضي {WarehouseId}", id);
            clsDialogs.Error("حدث خطأ أثناء تعيين المخزن الافتراضي. يرجى إعادة المحاولة.");
        }
        finally
        {
            _ApplyModeState();   // RULE-N150: الحالة تُستعاد من آلة الحالة المركزية
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// إعادة تنشيط المخزن المعطَّل (RULE-N90…RULE-N94): الزر مفعّل فقط على سطر غير نشط،
    /// والتنفيذ داخل الخدمة (BLL) داخل معاملة.
    /// </summary>
    private async void btnReactivate_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مخزن معطّل من القائمة أولاً.");
            return;
        }

        if (!_warehousesById.TryGetValue(id, out var warehouse))
        {
            clsDialogs.Warning("تعذر العثور على بيانات المخزن المحدد. يرجى تحديث القائمة.");
            return;
        }

        if (warehouse.IsActive)
        {
            clsDialogs.Warning("المخزن المحدد نشط بالفعل.");
            _ApplyModeState();
            return;
        }

        if (_reactivateBusy) return;

        if (!clsDialogs.Confirm($"هل تريد إعادة تنشيط المخزن \"{warehouse.Name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        _reactivateBusy = true;
        _UpdateReactivateButton();   // RULE-N43: تعطيل فوري أثناء التنفيذ

        try
        {
            var result = await WarehouseService.ReactivateAsync(id);

            if (result.IsSuccess)
            {
                Log.Information("أُعيد تنشيط المخزن {WarehouseId} ({WarehouseName}) بنجاح", id, warehouse.Name);

                EventBus.Publish(new WarehouseChangedMessage(id));

                await _LoadWarehousesAsync();

                // RULE-N94: يبقى السطر المُعاد تنشيطه محدداً وتُعاد تعبئة المحرر بالبيانات المحدَّثة.
                _FillForm(id);
                _SelectGridRow(id);
                _ApplyModeState();   // السجل نشط الآن ⇒ زر «إعادة تنشيط» معطّل

                clsDialogs.Success("تم إعادة تنشيط المخزن بنجاح.");
            }
            else
            {
                Log.Warning("تعذر إعادة تنشيط المخزن {WarehouseId}. السبب: {Error}", id, result.Error);

                clsDialogs.Error($"تعذر إعادة تنشيط المخزن.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تنشيط المخزن {WarehouseId}", id);
            clsDialogs.Error("حدث خطأ أثناء إعادة التنشيط. يرجى إعادة المحاولة.");
        }
        finally
        {
            _reactivateBusy = false;
            _ApplyModeState();   // RULE-N150: الحالة تُستعاد من آلة الحالة المركزية
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int warehouseId)
    {
        foreach (DataGridViewRow row in dgvWarehouses.Rows)
        {
            if (row.DataBoundItem is not DataRowView dataRow) continue;
            if (dataRow["Id"] is not int id || id != warehouseId) continue;

            row.Selected = true;

            // أعمدة الشبكة مُولَّدة تلقائياً من DataView (Id/Name/Phone/Address/IsActive)
            // — يُبحث العمود بالاسم بدل حقل ميّت كان null فيتحطّم المسار (RULE-N130).
            var nameColumn = dgvWarehouses.Columns["Name"];
            if (nameColumn is not null && row.Index >= 0)
                dgvWarehouses.CurrentCell = row.Cells[nameColumn.Index];

            break;
        }
    }

    // ── Context Menu / Double click (RULE-N23) ────────────────────────────────

    /// <summary>النقر المزدوج على السطر = تحميل بياناته في المحرّر (نفس مسار النقر المفرد).</summary>
    private void dgvWarehouses_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        dgvWarehouses_CellClick(sender, e);
    }

    /// <summary>
    /// حالة بنود قائمة السياق قبل ظهورها (RULE-N23): بنود التعديل/التعطيل تُفعَّل فقط عند
    /// وجود سطر محدد، وتتبع آلة حالة الأزرار نفسها (RULE-N43) بلا منطق مكرر.
    /// </summary>
    private void cmsGrid_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRow = dgvWarehouses.CurrentRow?.DataBoundItem is DataRowView;

        mnuEdit.Enabled = hasRow;
        mnuDeactivate.Enabled = hasRow && btnDelete.Enabled;
        mnuRefresh.Enabled = btnRefresh.Enabled;
        mnuExport.Enabled = dgvWarehouses.Rows.Count > 0;
    }

    /// <summary>تعديل السطر المحدد = تحميله في المحرّر ثم نقل المؤشر إلى أول حقل.</summary>
    private void mnuEdit_Click(object? sender, EventArgs e)
    {
        if (dgvWarehouses.CurrentRow is not DataGridViewRow row) return;

        dgvWarehouses_CellClick(dgvWarehouses, new DataGridViewCellEventArgs(0, row.Index));
        txtName.Focus();
    }

    /// <summary>تعطيل السطر المحدد — يستدعي معالج زر الحذف القائم (بلا منطق مكرر).</summary>
    private void mnuDeactivate_Click(object? sender, EventArgs e) => btnDelete.PerformClick();

    /// <summary>تحديث البيانات — يستدعي معالج زر التحديث القائم.</summary>
    private void mnuRefresh_Click(object? sender, EventArgs e) => btnRefresh.PerformClick();

    /// <summary>تصدير الشبكة المعروضة إلى Excel عبر <see cref="ExportHelper"/> (RULE-N112).</summary>
    private void mnuExport_Click(object? sender, EventArgs e) => _ExportGridToExcel();

    private void _ExportGridToExcel()
    {
        try
        {
            if (dgvWarehouses.Rows.Count == 0)
            {
                clsDialogs.Warning("لا توجد بيانات معروضة للتصدير.");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"دليل_المخازن_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير دليل المخازن إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvWarehouses, "المخازن", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تصدير دليل المخازن إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
