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

namespace SalesSystem.Desktop.Screens.Categories.Forms;

/// <summary>
/// النموذج المدمج (Master Form) لدليل مجموعات الأصناف.
/// يجمع: بحث مباشر + شبكة + محرر إدخال/تعديل في شاشة واحدة،
/// ويعمل بآلة حالة واحدة (<see cref="EntryMode"/>) تبدأ دائماً بوضع العرض (RULE-N149).
/// </summary>
public partial class frmCategoryMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private ICategoryDesktopService CategoryService => _serviceProvider.GetRequiredService<ICategoryDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, CategoryDto> _categoriesById = new();
    private DataTable _categoriesTable = new();
    private DataView _categoriesView = new();

    /// <summary>null = لا يوجد سجل محدد، &gt; 0 = المجموعة المحددة (للتعديل والتعطيل).</summary>
    private int? _editId;

    /// <summary>
    /// حالة الشاشة (آلة الحالة): تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>
    /// حيث كل حقول الإدخال **معطّلة**، ولا تُفعَّل إلا بزر «جديد» أو باختيار مجموعة من الشبكة (RULE-N149).
    /// </summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>يمنع تكرار تنفيذ إعادة التنشيط ويُعطّل زرها مؤقتاً أثناء العملية (RULE-N92).</summary>
    private bool _reactivateBusy;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( دليل مجموعات الأصناف ))";

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N43/RULE-N44).</summary>
    private enum EntryMode
    {
        /// <summary>عرض فقط: كل حقول الإدخال معطّلة حتى يضغط المستخدم «جديد» أو يحدّد سجلاً (RULE-N149).</summary>
        Browsing,

        /// <summary>إضافة سجل جديد: الحقول مفعّلة وزر «إضافة» متاح فقط.</summary>
        Adding,

        /// <summary>تعديل سجل محدد: الحقول مفعّلة وأزرار «تعديل/حذف» متاحة.</summary>
        Editing
    }

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmCategoryMaster()
    {
        InitializeComponent();
    }

    public frmCategoryMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;

        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف/إعادة تنشيط) و«مسح التصفية» تتغير حالة تفعيلها حسب الوضع والسجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل (RULE-N43).
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate, btnClearFilters);

        // القيم الافتراضية لأدوات التصفية (RULE-N57/N60) — الاختيار بالمؤشر لا بالنص العربي.
        cbFilterBy.SelectedIndex = 0; // الاسم
        cbStatus.SelectedIndex = 0;   // الكل

        // RULE-N47 — اختصارات موحّدة: F2 جديد · F5 تحديث · Delete تعطيل · Esc مسح التصفية/إلغاء.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDelete),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع الحالي: عرض ⇒ «جديد» · إضافة ⇒ «إضافة» · تعديل ⇒ «تعديل» (RULE-N147).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // طيّ/إظهار صندوق التصفية (RULE-N63) — آخر سطر في المُنشئ.
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
        foreach (Control input in new Control[] { txtSearch, chkShowInactive, txtCode, txtName, txtDescription })
        {
            if (input.Focused) return true;
        }

        return false;
    }

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// يعيد <c>false</c> عند وجود حقل ناقص.
    /// </summary>
    private bool _FocusFirstMissingField(bool edit)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المجموعة مطلوب");
            txtName.Focus();
            return false;
        }

        errorProvider.SetError(txtName, "");
        return true;
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← قائمة المجموعات ← المحرر ووضع العرض.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // التصفية تعود للافتراضي
            await _LoadCategoriesAsync();  // إعادة قراءة المجموعات (يعيد تطبيق التصفية والملخّص)
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

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmCategoryMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCategoriesAsync();
            _ResetForm();      // الشاشة تفتح في وضع العرض (RULE-N149)
            ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل مجموعات الأصناف");
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
        GreenThemeHelper.ApplyGrid(dgvCategories);
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه)
        dgvCategories.ReadOnly = true;
        dgvCategories.AllowUserToAddRows = false;
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCategories.MultiSelect = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadCategoriesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await CategoryService.GetAllAsync(includeInactive: true);

            _categoriesById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var c in result.Value)
                {
                    _categoriesById[c.Id] = c;
                    table.Rows.Add(c.Id, c.Name, c.Description ?? "", c.IsActive);
                }
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة مجموعات الأصناف.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة مجموعات الأصناف");
            clsDialogs.Error("تعذر تحميل قائمة مجموعات الأصناف. يرجى إعادة المحاولة.");
        }
        finally
        {
            _categoriesTable = table;
            _categoriesView = new DataView(_categoriesTable);
            dgvCategories.DataSource = _categoriesView;
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
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvCategories.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم المجموعة",
                "Name" => "اسم المجموعة",
                "Description" => "الوصف",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvCategories.Rows)
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

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter يطبّق التصفية فوراً · Esc يمسح كل أدوات التصفية (RULE-N59).</summary>
    private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
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
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtSearch.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        if (txtSearch is null) return;

        txtSearch.Clear();
        if (cbFilterBy is not null) cbFilterBy.SelectedIndex = 0;
        if (cbStatus is not null) cbStatus.SelectedIndex = 0;
        if (chkShowInactive is not null) chkShowInactive.Checked = false;
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية (RULE-N59/N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        _ApplyFilter();
    }

    /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        if (btnClearFilters is null || txtSearch is null || cbStatus is null || chkShowInactive is null) return;

        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtSearch.Text)
            || cbStatus.SelectedIndex > 0
            || chkShowInactive.Checked;
    }

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

    /// <summary>
    /// عمود البحث المقابل لاختيار «تصفية بـ» — أسماء أعمدة ثابتة داخل الكود لا تُبنى من إدخال المستخدم (RULE-N60).
    /// </summary>
    private string _FilterColumn() => cbFilterBy.SelectedIndex switch
    {
        1 => "Description",
        2 => "CONVERT(Id, 'System.String')",
        _ => "Name"
    };

    /// <summary>
    /// التصفية الفورية (RULE-N59/N60): كل الشروط من أسماء أعمدة معروفة،
    /// والاختيار يُقرأ بالمؤشر لا بالنص العربي، والقيمة تُهرَّب قبل بنائها في RowFilter.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_categoriesView?.Table is null) return;
        if (txtSearch is null || cbFilterBy is null || cbStatus is null || chkShowInactive is null) return;

        string text = txtSearch.Text.Trim();

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

        // ── البحث النصي: عمود واحد فقط يحدده cbFilterBy ──
        if (text.Length > 0)
            conditions.Add(RowFilterHelper.Like(_FilterColumn(), text));

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _categoriesView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية دليل المجموعات — أُبقيت التصفية السابقة كما هي");
        }

        _ApplyRowColoring();
        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    /// <summary>
    /// شريط الملخص (RULE-N61): المعروض من الإجمالي + النشط/المعطّل،
    /// ويُعاد حسابه بعد كل تصفية وكل تحميل.
    /// </summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null || _categoriesView?.Table is null) return;

        int total = _categoriesTable.Rows.Count;
        int shown = _categoriesView.Count;

        int active = 0, inactive = 0;

        foreach (DataRowView row in _categoriesView)
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

    private void dgvCategories_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvCategories.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مجموعة أصناف من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات المجموعة المحددة. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>تعبئة المحرّر بالسجل المحدد وتحويل الشاشة إلى وضع التعديل (RULE-N151).</summary>
    private void _FillForm(int categoryId)
    {
        if (!_categoriesById.TryGetValue(categoryId, out var c)) return;

        _editId = c.Id;
        _mode = EntryMode.Editing;   // RULE-N44: اختيار مجموعة من الشبكة = وضع التعديل

        txtCode.Text = c.Id.ToString();
        txtName.Text = c.Name;
        txtDescription.Text = c.Description ?? "";

        errorProvider.Clear();

        _ApplyModeState();   // تفعيل الحقول + ضبط الأزرار + تحديث شريط الوضع
    }

    // ── Actions state (RULE-N43 / RULE-N44 / RULE-N149…N152) ──────────────────

    /// <summary>
    /// آلة الحالة المركزية للشاشة: تفعيل/تعطيل حقول الإدخال، وحالة كل زر، ونص شريط الوضع،
    /// والزر الافتراضي للشاشة (Enter). تُستدعى بعد **كل** انتقال حالة:
    /// تحميل · «جديد» · اختيار سجل · حفظ ناجح · حذف · «تحديث» · Esc (RULE-N150).
    /// </summary>
    private void _ApplyModeState()
    {
        bool editingSession = _mode != EntryMode.Browsing;

        _SetEditorEnabled(editingSession);

        // آلة حالة الأزرار (RULE-N43): «إضافة» في وضع الإضافة، «تعديل/حذف/إعادة تنشيط» عند وجود سجل محدد.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnSave.Enabled = _mode == EntryMode.Adding;
        btnEdit.Enabled = _mode == EntryMode.Editing;
        btnDelete.Enabled = _mode == EntryMode.Editing;

        _UpdateReactivateButton();    // RULE-N92: إعادة التنشيط للسجل المعطّل فقط
        _UpdateModeHeader();          // RULE-N44

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

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند تحديد سجل.</summary>
    private Control[] _EditorInputs() => new Control[] { txtName, txtDescription };

    /// <summary>تفعيل أو تعطيل كل حقول المحرر دفعة واحدة (txtCode يبقى للقراءة دائماً).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة مجموعة جديدة» · «تعديل مجموعة: &lt;الاسم&gt;».
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — إضافة مجموعة جديدة",
            EntryMode.Editing => $"{FormTitle} — تعديل مجموعة: {txtName.Text.Trim()}",
            _ => $"{FormTitle} — وضع العرض"
        };
    }

    /// <summary>
    /// آلة حالة زر «إعادة تنشيط» (RULE-N92): مفعّل فقط في وضع التعديل عند تحديد سجل **غير نشط**،
    /// ومعطّل عند عدم وجود تحديد أو عند كون السجل نشطاً أو أثناء عملية جارية.
    /// </summary>
    private void _UpdateReactivateButton()
    {
        bool hasSelection = _editId.HasValue;
        bool isActive = hasSelection
            && _categoriesById.TryGetValue(_editId!.Value, out var category)
            && category.IsActive;

        btnReactivate.Enabled = _mode == EntryMode.Editing && hasSelection && !isActive && !_reactivateBusy;   // RULE-N92
    }

    // ── Reset / New entry ─────────────────────────────────────────────────────

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
        txtDescription.Clear();

        dgvCategories.ClearSelection();

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>
    /// بدء إدخال مجموعة جديدة (زر «جديد» / F2): تفريغ الحقول ثم **تفعيلها** وتحويل شريط الوضع
    /// إلى «إضافة مجموعة جديدة» (RULE-N151).
    /// </summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المجموعة مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (txtName.Text.Trim().Length > 100)
        {
            errorProvider.SetError(txtName, "اسم المجموعة أطول من المسموح (100 حرف)");
            isValid = false;
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    /// <summary>زر الخروج: إغلاق شاشة دليل مجموعات الأصناف.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>زر الإضافة: يُضيف مجموعة جديدة دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveCategoryAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المجموعة المحددة فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveCategoryAsync(edit: true);

    /// <summary>
    /// حفظ المجموعة: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveCategoryAsync(bool edit)
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
            if (MessageBox.Show($"هل أنت متأكد من تعديل المجموعة \"{editName}\"؟",
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
            string? description = string.IsNullOrWhiteSpace(txtDescription.Text)
                ? null
                : txtDescription.Text.Trim();

            Result<CategoryDto> result;

            if (edit)
            {
                int id = _editId!.Value;

                // الحفاظ على حالة التنشيط الحالية للسجل أثناء التعديل
                // (مجموعة معطّلة لا تُعاد تنشيطها صامتاً بفعل الحفظ).
                bool isActive = _categoriesById.TryGetValue(id, out var current) && current.IsActive;

                var request = new UpdateCategoryRequest(name, description, isActive);
                result = await CategoryService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateCategoryRequest(name, description);
                result = await CategoryService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new CategoryChangedMessage(result.Value.Id));

                clsDialogs.Success(edit ? "تم تعديل المجموعة بنجاح." : "تمت إضافة المجموعة بنجاح.");

                // بعد الحفظ الناجح (إضافة أو تعديل): يصبح السجل محفوظاً ومحدداً → وضع التعديل،
                // ثم تحديث القائمة وإعادة تعبئة الحقول من البيانات المحفوظة (تحديث تلقائي).
                _editId = result.Value.Id;
                _mode = EntryMode.Editing;

                await _LoadCategoriesAsync();
                _FillForm(result.Value.Id);   // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _ApplyModeState();            // ضمان الحالة حتى لو لم يُعثر على السطر
                _SelectGridRow(result.Value.Id);
            }
            else
            {
                clsDialogs.Error($"لم يتم حفظ المجموعة.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ مجموعة الأصناف");
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
            clsDialogs.Warning("يرجى اختيار مجموعة من القائمة أولاً.");
            return;
        }

        string name = txtName.Text.Trim();

        if (!clsDialogs.Confirm($"هل أنت متأكد من حذف (تعطيل) المجموعة \"{name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await CategoryService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعطيل المجموعة بنجاح.");

                EventBus.Publish(new CategoryChangedMessage(id));

                // بعد الحذف: الرجوع إلى وضع العرض (الحقول معطّلة) ثم تحديث القائمة تلقائياً.
                _ResetForm();
                await _LoadCategoriesAsync();
                _ApplyModeState();   // يضمن بقاء الحقول معطّلة بعد إعادة التحميل
            }
            else
            {
                clsDialogs.Error($"لم يتم تعطيل المجموعة.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل مجموعة الأصناف {CategoryId}", id);
            clsDialogs.Error("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.");
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع العرض → «جديد» فقط هو المتاح.
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// إعادة تنشيط المجموعة المعطَّلة (RULE-N90…RULE-N94):
    /// الزر مفعّل فقط على سطر غير نشط، والتنفيذ داخل الخدمة (BLL) داخل معاملة.
    /// </summary>
    private async void btnReactivate_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مجموعة معطّلة من القائمة أولاً.");
            return;
        }

        if (!_categoriesById.TryGetValue(id, out var category))
        {
            clsDialogs.Warning("تعذر العثور على بيانات المجموعة المحددة. يرجى تحديث القائمة.");
            return;
        }

        // السجل نشط أصلاً → لا معنى لإعادة التنشيط (ويُعاد ضبط حالة الزر).
        if (category.IsActive)
        {
            clsDialogs.Warning("المجموعة المحددة نشطة بالفعل.");
            _UpdateReactivateButton();
            return;
        }

        if (_reactivateBusy) return;

        if (!clsDialogs.Confirm($"هل تريد إعادة تنشيط المجموعة \"{category.Name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        _reactivateBusy = true;
        _UpdateReactivateButton();

        try
        {
            var result = await CategoryService.ReactivateAsync(id);

            if (result.IsSuccess)
            {
                Log.Information("أُعيد تنشيط مجموعة الأصناف {CategoryId} ({CategoryName}) بنجاح",
                    id, category.Name);

                EventBus.Publish(new CategoryChangedMessage(id));

                await _LoadCategoriesAsync();   // تحديث تلقائي للشبكة والملخّص

                // RULE-N94: يبقى السطر المُعاد تنشيطه محدداً، وتُعاد تعبئة المحرّر
                // من البيانات المحدَّثة في وضع التعديل (تحديث تلقائي بلا إعادة فتح الشاشة).
                _editId = id;
                _FillForm(id);
                _SelectGridRow(id);
                _ApplyModeState();   // السجل نشط بعد إعادة التنشيط → زر إعادة التنشيط معطّل

                clsDialogs.Success("تم إعادة تنشيط المجموعة بنجاح.");
            }
            else
            {
                Log.Warning("تعذر إعادة تنشيط مجموعة الأصناف {CategoryId}. السبب: {Error}",
                    id, result.Error);

                clsDialogs.Error($"تعذر إعادة تنشيط المجموعة.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تنشيط مجموعة الأصناف {CategoryId}", id);
            clsDialogs.Error("حدث خطأ أثناء إعادة التنشيط. يرجى إعادة المحاولة.");
        }
        finally
        {
            _reactivateBusy = false;
            _ApplyModeState();   // يضمن تعطيل «إعادة تنشيط» بعد نجاح العملية (RULE-N92)
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int categoryId)
    {
        foreach (DataGridViewRow row in dgvCategories.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == categoryId)
            {
                row.Selected = true;
                dgvCategories.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }

    // ── Context Menu / Double click (RULE-N23) ────────────────────────────────

    /// <summary>النقر المزدوج على السطر = تحميل بياناته في المحرّر (نفس مسار النقر المفرد).</summary>
    private void dgvCategories_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        dgvCategories_CellClick(sender, e);
    }

    /// <summary>
    /// حالة بنود قائمة السياق قبل ظهورها (RULE-N23): بنود التعديل/التعطيل تُفعَّل فقط عند
    /// وجود سطر محدد، وتتبع آلة حالة الأزرار نفسها (RULE-N43) بلا منطق مكرر.
    /// </summary>
    private void cmsGrid_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRow = dgvCategories.CurrentRow?.DataBoundItem is DataRowView;

        mnuEdit.Enabled = hasRow;
        mnuDeactivate.Enabled = hasRow && btnDelete.Enabled;
        mnuRefresh.Enabled = btnRefresh.Enabled;
        mnuExport.Enabled = dgvCategories.Rows.Count > 0;
    }

    /// <summary>تعديل السطر المحدد = تحميله في المحرّر ثم نقل المؤشر إلى أول حقل.</summary>
    private void mnuEdit_Click(object? sender, EventArgs e)
    {
        if (dgvCategories.CurrentRow is not DataGridViewRow row) return;

        dgvCategories_CellClick(dgvCategories, new DataGridViewCellEventArgs(0, row.Index));
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
            if (dgvCategories.Rows.Count == 0)
            {
                clsDialogs.Warning("لا توجد بيانات معروضة للتصدير.");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"دليل_المجموعات_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير دليل مجموعات الأصناف إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvCategories, "المجموعات", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تصدير دليل مجموعات الأصناف إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
