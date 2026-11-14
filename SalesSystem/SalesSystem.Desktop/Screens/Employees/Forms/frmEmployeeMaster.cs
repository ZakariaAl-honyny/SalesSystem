using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Employees.Forms;

/// <summary>
/// شاشة موحّدة (Master Form) لدليل الموظفين.
/// تجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// وتعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmEmployeeMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IEmployeeDesktopService EmployeeService => _serviceProvider.GetRequiredService<IEmployeeDesktopService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, EmployeeDto> _employeesById = new();
    private DataTable _employeesTable = new();
    private DataView _employeesView = new();

    /// <summary>السجل الأصلي للموظف المحدد — تُحفظ منه الحقول غير القابلة للتحرير في هذه الشاشة.</summary>
    private EmployeeDto? _selected;

    /// <summary>null = إضافة موظف جديد، &gt; 0 = تعديل موظف قائم.</summary>
    private int? _editId;

    /// <summary>حارس منع التكرار أثناء تنفيذ عملية إعادة التنشيط (RULE-N92).</summary>
    private bool _reactivateBusy;

    /// <summary>
    /// حالة الشاشة (آلة الحالة): تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>
    /// حيث كل حقول الإدخال **معطّلة**، ولا تُفعَّل إلا بزر «جديد» أو باختيار سجل من الشبكة (RULE-N149).
    /// </summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( دليل الموظفين ))";

    /// <summary>أوضاع شاشة الإدخال — آلة الحالة التي تحكم تفعيل الحقول والأزرار (RULE-N43/RULE-N44/RULE-N149).</summary>
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
    private frmEmployeeMaster()
    {
        InitializeComponent();
    }

    public frmEmployeeMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف) و«مسح التصفية» تتغير حالة تفعيلها حسب الوضع والسجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate, btnClearFilters); // RULE-N43

        // RULE-N47 — اختصارات موحّدة: F2 جديد · F5 تحديث · Esc مسح التصفية/إلغاء (RULE-N59).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, btnNew.PerformClick),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع الحالي: عرض ⇒ «جديد» · إضافة ⇒ «إضافة/حفظ» · تعديل ⇒ «تعديل» (RULE-N147).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N47 — اختصار كل زر في تلميحه
        tafqeetTip.SetToolTip(btnNew, "جديد (F2) — Esc يمسح التصفية");
        tafqeetTip.SetToolTip(btnSave, "إضافة موظف جديد");
        tafqeetTip.SetToolTip(btnEdit, "تعديل الموظف المحدد");
        tafqeetTip.SetToolTip(btnDelete, "حذف (تعطيل) الموظف المحدد");
        tafqeetTip.SetToolTip(btnReactivate, "إعادة تنشيط السجل المعطّل");
        tafqeetTip.SetToolTip(btnRefresh, "تحديث البيانات (F5)");
        tafqeetTip.SetToolTip(btnClose, "خروج من الشاشة");

        // RULE-N97 — التفقيط الحيّ على الحقل المالي
        LiveTafqeetHelper.Attach(tafqeetTip,
            (numBasicSalary, "الراتب الأساسي", LiveTafqeetHelper.ReadMode.Money));
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── Global Shortcuts ──────────────────────────────────────────────────────

    /// <summary>
    /// Esc: مسح كل التصفية إن كان المؤشر داخل صندوق التصفية، وإلا إلغاء المحرر
    /// والعودة إلى وضع العرض (RULE-N47 / RULE-N150).
    /// </summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        _ResetForm();
    }

    /// <summary>هل الحقل المطلوب (اسم الموظف) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    private void _FocusFirstMissingField(bool edit)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الموظف مطلوب");
            txtName.Focus();
            return;
        }

        // لا توجد حقول مطلوبة أخرى في هذه الشاشة (الباقي اختياري في وضعي الإضافة والتعديل).
        _ = edit;
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← قائمة الموظفين ← المحرر ووضع العرض. لا يُستخدم لمسح التصفية وحدها.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // التصفية تعود للافتراضي
            await _LoadEmployeesAsync();   // إعادة قراءة الموظفين (يعيد تطبيق التصفية والملخّص)
            _ResetForm();                  // تفريغ المحرر + وضع العرض + الحقول معطّلة
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث قائمة الموظفين");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmEmployeeMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadEmployeesAsync();
            _ResetForm();
            ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الموظفين");
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
        GreenThemeHelper.ApplyGrid(dgvEmployees);

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه)
        dgvEmployees.ReadOnly = true;
        dgvEmployees.AllowUserToAddRows = false;
        dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEmployees.MultiSelect = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadEmployeesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("JobTitle", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("NationalId", typeof(string));
        table.Columns.Add("BasicSalary", typeof(decimal));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await EmployeeService.GetAllAsync(includeInactive: true);

            _employeesById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var e in result.Value)
                {
                    _employeesById[e.Id] = e;
                    table.Rows.Add(
                        e.Id,
                        e.Name,
                        e.JobTitle ?? "",
                        e.Phone ?? "",
                        e.NationalId ?? "",
                        e.BasicSalary,
                        e.IsActive);
                }
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة الموظفين.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الموظفين");
            clsDialogs.Error("تعذر تحميل قائمة الموظفين. يرجى إعادة المحاولة.");
        }
        finally
        {
            _employeesTable = table;
            _employeesView = new DataView(_employeesTable);
            dgvEmployees.DataSource = _employeesView;
            _ApplyGridHeaders();
            _ApplyFilter(); // يشمل تلوين الأسطر + شريط الملخص (RULE-N61)
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يستخدم DataGridView عنوان <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvEmployees.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "الرقم",
                "Name" => "اسم الموظف",
                "JobTitle" => "المسمى الوظيفي",
                "Phone" => "الهاتف",
                "NationalId" => "الرقم الوطني",
                "BasicSalary" => "الراتب الأساسي",
                "IsActive" => "الحالة",
                _ => column.HeaderText
            };

            // أعمدة مساندة للتصفية والتلخيص فقط — لا تُعرض في الشبكة.
            column.Visible = column.DataPropertyName is not ("NationalId" or "BasicSalary");
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvEmployees.Rows)
        {
            bool isActive = row.Cells["IsActive"].Value is bool b && b;
            if (!isActive)
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
        }
    }

    // ── Search / Filter (DataView.RowFilter — RULE-N56…N63) ───────────────────

    // المعالجان الأصليان (txtSearch_TextChanged / txtSearch_KeyPress) محفوظان بالاسم،
    // لكنهما مربوطان الآن بالحقل القياسي txtFilterValue داخل gbFilter (RULE-N57).
    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkHasSalary_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>RULE-N59 — Enter في حقل التصفية يطبّق فوراً.</summary>
    private void txtFilterValue_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;
        _ApplyFilter();
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
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtFilterValue.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تطبيق ولا تحميل) — تُستهلك من «مسح» و«تحديث» وEsc.</summary>
    private void _ClearFilterInputs()
    {
        if (cbFilterBy.SelectedIndex != 0) cbFilterBy.SelectedIndex = 0;
        if (cbStatus.SelectedIndex != 0) cbStatus.SelectedIndex = 0;
        if (chkShowInactive.Checked) chkShowInactive.Checked = false;
        if (chkHasSalary.Checked) chkHasSalary.Checked = false;
        if (txtFilterValue.Text.Length > 0) txtFilterValue.Clear();
    }

    /// <summary>RULE-N59 — يمسح كل أدوات التصفية ويعيد القائمة كاملة إلى حالتها الافتراضية.</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();

        _ApplyFilter(); // ضمان التطبيق حتى لو لم يتغيّر شيء
    }

    /// <summary>RULE-N43 / RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية نشطة.</summary>
    private void _UpdateClearFiltersState()
    {
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex > 0
            || cbStatus.SelectedIndex > 0
            || chkShowInactive.Checked
            || chkHasSalary.Checked;
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59): تُطبَّق على <see cref="DataView.RowFilter"/> بأسماء أعمدة معروفة
    /// حسب <c>cbFilterBy.SelectedIndex</c> (RULE-N60) وبقيم مُهرَّبة، ثم تُحدَّث لوحة الملخص (RULE-N61).
    /// </summary>
    private void _ApplyFilter()
    {
        if (_employeesView is null || _employeesView.Table is null)
        {
            _UpdateSummary();
            _UpdateClearFiltersState();
            return;
        }

        var conditions = new List<string>();

        // (1) الحالة — cbStatus لها الأولوية، وإلا يحكمها مربع «إظهار غير النشط».
        switch (cbStatus.SelectedIndex)
        {
            case 1:
                conditions.Add("IsActive = true");
                break;

            case 2:
                conditions.Add("IsActive = false");
                break;

            default:
                if (!chkShowInactive.Checked)
                    conditions.Add("IsActive = true");
                break;
        }

        // (2) له راتب محدد — عمود حقيقي (BasicSalary)
        if (chkHasSalary.Checked)
            conditions.Add("BasicSalary > 0");

        // (3) البحث النصي على العمود المختار فقط
        string text = txtFilterValue.Text.Trim();

        if (text.Length > 0)
        {
            string column = cbFilterBy.SelectedIndex switch
            {
                1 => "Phone",
                2 => "JobTitle",
                3 => "NationalId",
                _ => "Name",
            };

            conditions.Add(RowFilterHelper.Like(column, text));
        }

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _employeesView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : string.Empty;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية دليل الموظفين — أُبقيت التصفية السابقة كما هي");
        }

        _ApplyRowColoring();
        _UpdateSummary();
        _UpdateClearFiltersState();
    }

    /// <summary>RULE-N61 — شريط الملخص: المعروض/الإجمالي + نشط/معطّل + إجمالي رواتب المعروض.</summary>
    private void _UpdateSummary()
    {
        int total = _employeesTable?.Rows.Count ?? 0;
        int shown = _employeesView?.Table is null ? 0 : _employeesView.Count;

        int active = 0;
        int inactive = 0;

        if (_employeesTable is not null)
        {
            foreach (DataRow row in _employeesTable.Rows)
            {
                if (row["IsActive"] is bool isActive && isActive) active++;
                else inactive++;
            }
        }

        decimal shownSalary = 0m;

        if (_employeesView?.Table is not null)
        {
            for (int i = 0; i < _employeesView.Count; i++)
            {
                if (_employeesView[i]["BasicSalary"] is decimal salary)
                    shownSalary += salary;
            }
        }

        lblSummary.Text =
            $"المعروض: {shown} من {total} — نشط: {active} · معطّل: {inactive}" +
            $" · إجمالي رواتب المعروض: {shownSalary:N2}";

        // «لا توجد بيانات» يُعلن في شريط الملخص بدل إضافة صف وهمي إلى الشبكة
        // (الصف الوهمي يُفسد التصدير والتصفية — RULE-N22 / RULE-N61).
        if (shown == 0) lblSummary.Text += " — لا توجد بيانات مطابقة";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvEmployees_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvEmployees.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار موظف من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات الموظف المحدد. يرجى إعادة المحاولة.");
        }
    }

    private void _FillForm(int employeeId)
    {
        if (!_employeesById.TryGetValue(employeeId, out var e)) return;

        _editId = employeeId;
        _selected = e;
        _mode = EntryMode.Editing;   // RULE-N150: اختيار سجل من الشبكة = وضع التعديل

        errorProvider.Clear();

        txtCode.Text = e.Code;
        txtName.Text = e.Name;
        txtJobTitle.Text = e.JobTitle ?? "";
        txtPhone.Text = e.Phone ?? "";
        txtNationalId.Text = e.NationalId ?? "";
        numBasicSalary.Value = _Clamp(numBasicSalary, e.BasicSalary);

        if (e.HireDate.HasValue)
        {
            dtpHireDate.Value = e.HireDate.Value;
            dtpHireDate.Checked = true;
        }
        else
        {
            dtpHireDate.Checked = false;
        }

        // بعد تعبئة الحقول: تفعيل الحقول + ضبط الأزرار + شريط الوضع + Enter الأساسي.
        _ApplyModeState();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    /// <summary>
    /// آلة الحالة المركزية للشاشة (RULE-N150): تفعيل/تعطيل حقول الإدخال، وحالة كل زر، ونص شريط الوضع.
    /// تُستدعى بعد **كل** انتقال حالة: تحميل · «جديد» · اختيار سجل · حفظ ناجح · حذف · «تحديث» · Esc.
    /// الحقول تبدأ معطّلة، ولا تُفعَّل إلا في وضعي الإضافة والتعديل (RULE-N149).
    /// </summary>
    private void _ApplyModeState()
    {
        bool editingSession = _mode != EntryMode.Browsing;

        _SetEditorEnabled(editingSession);

        // آلة حالة الأزرار (RULE-N43): «جديد/تحديث/خروج» متاحة دائماً،
        // «إضافة» في وضع الإضافة، «تعديل/حذف» عند وجود سجل محدد.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnSave.Enabled = _mode == EntryMode.Adding;
        btnEdit.Enabled = _mode == EntryMode.Editing;
        btnDelete.Enabled = _mode == EntryMode.Editing;

        _UpdateReactivateButton();   // RULE-N92: إعادة التنشيط للسجل المعطّل فقط

        _UpdateModeHeader();         // RULE-N44 / RULE-N151

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
    private Control[] _EditorInputs() => new Control[]
    {
        txtName, txtJobTitle, txtPhone, txtNationalId, numBasicSalary, dtpHireDate
    };

    /// <summary>تفعيل أو تعطيل كل حقول المحرر دفعة واحدة (txtCode يبقى للقراءة دائماً).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة موظف جديد» · «تعديل موظف: &lt;الاسم&gt;».
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — إضافة موظف جديد",
            EntryMode.Editing => $"{FormTitle} — تعديل موظف: {txtName.Text.Trim()}",
            _ => $"{FormTitle} — وضع العرض"
        };
    }

    /// <summary>
    /// آلة حالة زر «إعادة تنشيط» (RULE-N92): مفعّل فقط في وضع التعديل مع سجل **غير نشط** محدد،
    /// ومعطّل عند عدم وجود تحديد أو كون السجل نشطاً أو أثناء عملية جارية.
    /// </summary>
    private void _UpdateReactivateButton()
    {
        bool hasSelection = _editId.HasValue;

        bool isActive = hasSelection
            && _employeesById.TryGetValue(_editId!.Value, out var employee)
            && employee.IsActive;

        btnReactivate.Enabled =
            _mode == EntryMode.Editing && hasSelection && !isActive && !_reactivateBusy;   // RULE-N92
    }

    /// <summary>
    /// إعادة الشاشة إلى **وضع العرض** (الحالة الافتراضية): تفريغ الحقول، تعطيل كل حقول الإدخال،
    /// وإتاحة «جديد/تحديث/خروج» فقط (RULE-N149).
    /// </summary>
    private void _ResetForm()
    {
        _editId = null;
        _selected = null;
        _mode = EntryMode.Browsing;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtJobTitle.Clear();
        txtPhone.Clear();
        txtNationalId.Clear();
        numBasicSalary.Value = 0;

        dtpHireDate.Value = DateTime.Today;
        dtpHireDate.Checked = true;

        dgvEmployees.ClearSelection();

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>
    /// بدء إدخال موظف جديد (زر «جديد» / F2): تفريغ الحقول ثم **تفعيلها** وتحويل شريط الوضع
    /// إلى «إضافة موظف جديد» (RULE-N151).
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
            errorProvider.SetError(txtName, "اسم الموظف مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (numBasicSalary.Value < 0)
        {
            errorProvider.SetError(numBasicSalary, "الراتب الأساسي يجب أن يكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numBasicSalary, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    /// <summary>زر الخروج: إغلاق شاشة دليل الموظفين.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>
    /// بناء بطاقة الموظف من حقول الشاشة.
    /// الحقول غير الظاهرة (الملاحظات، الحساب المحاسبي، حساب المستخدم، نسبة العمولة)
    /// تُحفظ كما هي عند التعديل حتى لا تُفقد بياناتها.
    /// </summary>
    private EmployeeDto _BuildEmployeeDto()
    {
        string? NullIfEmpty(TextBox box)
            => string.IsNullOrWhiteSpace(box.Text) ? null : box.Text.Trim();

        return new EmployeeDto(
            Id: _selected?.Id ?? 0,
            Code: _selected?.Code ?? string.Empty,
            Name: txtName.Text.Trim(),
            JobTitle: NullIfEmpty(txtJobTitle),
            Phone: NullIfEmpty(txtPhone),
            NationalId: NullIfEmpty(txtNationalId),
            BasicSalary: numBasicSalary.Value,
            HireDate: dtpHireDate.Checked ? dtpHireDate.Value.Date : null,
            IsActive: _selected?.IsActive ?? true,
            Notes: _selected?.Notes,
            AccountCode: _selected?.AccountCode,
            LinkedUserId: _selected?.LinkedUserId,
            CommissionRate: _selected?.CommissionRate);
    }

    /// <summary>زر الإضافة: يُضيف موظفاً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث الموظف المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ الموظف: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقل المطلوب الناقص: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_RequiredFieldsValid())
        {
            _FocusFirstMissingField(edit);
            return;
        }

        if (!_ValidateForm()) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = txtName.Text.Trim();
            if (!clsDialogs.ConfirmEdit($"هل أنت متأكد من تعديل الموظف \"{editName}\"؟"))
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;   // حارس مؤقت أثناء العملية — يُستعاد في finally عبر _ApplyModeState
        btnEdit.Enabled = false;

        try
        {
            var request = _BuildEmployeeDto();

            Result<EmployeeDto> result;

            if (edit)
                result = await EmployeeService.UpdateAsync(_editId!.Value, request);
            else
                result = await EmployeeService.CreateAsync(request);

            if (result.IsSuccess && result.Value != null)
            {
                clsDialogs.Success(edit ? "تم تعديل الموظف بنجاح." : "تمت إضافة الموظف بنجاح.");

                // بعد الحفظ الناجح (إضافة أو تعديل): يصبح السجل محفوظاً ومحدداً → وضع التعديل،
                // ثم تحديث القائمة وإعادة تعبئة الحقول من البيانات المحفوظة (تحديث تلقائي — RULE-N151).
                _editId = result.Value.Id;
                _selected = result.Value;
                _mode = EntryMode.Editing;

                await _LoadEmployeesAsync();
                _FillForm(result.Value.Id);   // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _ApplyModeState();            // ضمان الحالة حتى لو لم يُعثر على السطر
                _SelectGridRow(result.Value.Id);
            }
            else
            {
                clsDialogs.Error($"لم يتم حفظ الموظف.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الموظف");
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
            clsDialogs.Warning("يرجى اختيار موظف من القائمة أولاً.");
            return;
        }

        string name = txtName.Text.Trim();

        if (!clsDialogs.Confirm($"هل أنت متأكد من حذف (تعطيل) الموظف \"{name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;   // حارس مؤقت أثناء العملية — يُستعاد في finally عبر _ApplyModeState

        try
        {
            var result = await EmployeeService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعطيل الموظف بنجاح.");

                // بعد الحذف: الرجوع إلى وضع العرض (الحقول معطّلة) ثم تحديث القائمة تلقائياً.
                _ResetForm();
                await _LoadEmployeesAsync();
                _ApplyModeState();   // يضمن بقاء الحقول معطّلة بعد إعادة التحميل
            }
            else
            {
                clsDialogs.Error($"لم يتم تعطيل الموظف.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الموظف {EmployeeId}", id);
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
    /// إعادة تنشيط الموظف المعطَّل (RULE-N90…RULE-N94 / RULE-N122):
    /// الزر مفعّل فقط على سجل غير نشط في وضع التعديل، والتنفيذ في الخدمة بلا منطق محلي.
    /// </summary>
    private async void btnReactivate_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار موظف معطّل من القائمة أولاً.");
            return;
        }

        if (!_employeesById.TryGetValue(id, out var employee))
        {
            clsDialogs.Warning("تعذر العثور على بيانات الموظف المحدد. يرجى تحديث القائمة.");
            return;
        }

        if (employee.IsActive)
        {
            clsDialogs.Warning("الموظف المحدد نشط بالفعل.");
            _UpdateReactivateButton();
            return;
        }

        if (_reactivateBusy) return;

        if (!clsDialogs.Confirm($"هل تريد إعادة تنشيط الموظف \"{employee.Name}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        _reactivateBusy = true;
        _UpdateReactivateButton();

        try
        {
            var result = await EmployeeService.ReactivateAsync(id);

            if (result.IsSuccess)
            {
                Log.Information("أُعيد تنشيط الموظف {EmployeeId} ({EmployeeName}) بنجاح", id, employee.Name);

                clsDialogs.Success("تم إعادة تنشيط الموظف بنجاح.");

                // RULE-N94: تحديث تلقائي للشبكة ثم إبقاء السطر المُعاد تنشيطه محدداً وإعادة تعبئة المحرر.
                await _LoadEmployeesAsync();
                _FillForm(id);
                _SelectGridRow(id);
                _ApplyModeState();   // السجل نشط بعد إعادة التنشيط → زر إعادة التنشيط معطّل
            }
            else
            {
                Log.Warning("تعذر إعادة تنشيط الموظف {EmployeeId}. السبب: {Error}", id, result.Error);

                clsDialogs.Error($"تعذر إعادة تنشيط الموظف.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تنشيط الموظف {EmployeeId}", id);
            clsDialogs.Error("حدث خطأ أثناء إعادة التنشيط. يرجى إعادة المحاولة.");
        }
        finally
        {
            _reactivateBusy = false;
            _ApplyModeState();
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int employeeId)
    {
        foreach (DataGridViewRow row in dgvEmployees.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == employeeId)
            {
                row.Selected = true;
                dgvEmployees.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }

    private static decimal _Clamp(NumericUpDown control, decimal value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);

    // ── Context Menu / Double click (RULE-N23) ────────────────────────────────

    /// <summary>النقر المزدوج على السطر = تحميل بياناته في المحرّر (نفس مسار النقر المفرد).</summary>
    private void dgvEmployees_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        dgvEmployees_CellClick(sender, e);
    }

    /// <summary>
    /// حالة بنود قائمة السياق قبل ظهورها (RULE-N23): بنود التعديل/التعطيل تُفعَّل فقط عند
    /// وجود سطر محدد، وتتبع آلة حالة الأزرار نفسها (RULE-N43) بلا منطق مكرر.
    /// </summary>
    private void cmsGrid_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRow = dgvEmployees.CurrentRow?.DataBoundItem is DataRowView;

        mnuEdit.Enabled = hasRow;
        mnuDeactivate.Enabled = hasRow && btnDelete.Enabled;
        mnuRefresh.Enabled = btnRefresh.Enabled;
        mnuExport.Enabled = dgvEmployees.Rows.Count > 0;
    }

    /// <summary>تعديل السطر المحدد = تحميله في المحرّر ثم نقل المؤشر إلى أول حقل.</summary>
    private void mnuEdit_Click(object? sender, EventArgs e)
    {
        if (dgvEmployees.CurrentRow is not DataGridViewRow row) return;

        dgvEmployees_CellClick(dgvEmployees, new DataGridViewCellEventArgs(0, row.Index));
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
            if (dgvEmployees.Rows.Count == 0)
            {
                clsDialogs.Warning("لا توجد بيانات معروضة للتصدير.");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"دليل_الموظفين_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير دليل الموظفين إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvEmployees, "الموظفون", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تصدير دليل الموظفين إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
