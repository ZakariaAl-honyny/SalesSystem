using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Users.Forms;

/// <summary>
/// شاشة موحّدة (Master Form) لإدارة المستخدمين.
/// تجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// وتعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmUserMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IUserDesktopService UserService => _serviceProvider.GetRequiredService<IUserDesktopService>();
    private IBranchDesktopService BranchService => _serviceProvider.GetRequiredService<IBranchDesktopService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, UserDto> _usersById = new();
    private DataTable _usersTable = new();
    private DataView _usersView = new();

    /// <summary>قائمة الفروع المتاحة لاختيار فرع المستخدم.</summary>
    private List<BranchDto> _branchLookup = new();

    /// <summary>null = إضافة مستخدم جديد، &gt; 0 = تعديل مستخدم قائم.</summary>
    private int? _editId;

    /// <summary>يمنع إعادة التقييم أثناء تعبئة القوائم برمجياً.</summary>
    private bool _isBindingLookups;

    /// <summary>يمنع تكرار نداء «إعادة التنشيط» أثناء تنفيذ العملية (RULE-N92).</summary>
    private bool _reactivateBusy;

    /// <summary>
    /// حالة الشاشة (آلة الحالة): تبدأ دائماً بوضع العرض <see cref="EntryMode.Browsing"/>
    /// حيث كل حقول الإدخال **معطّلة**، ولا تُفعَّل إلا بزر «جديد» أو باختيار سجل من الشبكة (RULE-N149).
    /// </summary>
    private EntryMode _mode = EntryMode.Browsing;

    /// <summary>نص شريط العنوان الأساسي، تُضاف إليه لاحقة الوضع الحالي.</summary>
    private const string FormTitle = "(( إدارة المستخدمين ))";

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
    private frmUserMaster()
    {
        InitializeComponent();
    }

    public frmUserMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف) و«مسح التصفية» تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate, btnClearFilters); // RULE-N43

        // الاختصارات الموحّدة (RULE-N47): F2 جديد · F5 تحديث · Delete تعطيل · Esc مسح التصفية/إلغاء.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDelete),
            (Keys.Escape, _GlobalEscape));

        // Enter = الزر الأساسي للوضع الحالي: عرض ⇒ «جديد» · إضافة ⇒ «إضافة/حفظ» · تعديل ⇒ «تعديل» (RULE-N147).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N60: أدوات التصفية تُقرأ بالمؤشر — «الحالة: الكل» و«تصفية بـ: اسم المستخدم» افتراضياً.
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
        FilterPanelHelper.Attach(this, _serviceProvider);

        // RULE-N149: الشاشة تفتح في وضع العرض — الحقول معطّلة من أول لحظة (حتى قبل Load).
        _ApplyModeState();
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
                     txtSearch, cbFilterBy, cbStatus, txtCode, txtUserName, txtPassword, cmbBranch
                 })
        {
            if (input.Focused) return true;
        }

        return false;
    }

    /// <summary>الفرع المحدد (0 = بدون فرع).</summary>
    private short? _SelectedBranchId()
        => cmbBranch.SelectedValue is short branchId && branchId > 0 ? branchId : null;

    /// <summary>
    /// يتحقق من الحقول المطلوبة (اسم المستخدم). عند نقص أي حقل ينقل المؤشر إليه
    /// ويعرض أيقونة الخطأ عليه ثم يعيد <c>false</c> — دون تعطيل أي زر.
    /// </summary>
    /// <remarks>
    /// اسم المستخدم مطلوب في وضعي الإضافة والتعديل، لذا لا يغيّر <paramref name="edit"/>
    /// قاعدة التحقق (أُبقي في التوقيع لتوحيد شاشات الإدخال).
    /// </remarks>
    private bool _FocusFirstMissingField(bool edit)
    {
        if (string.IsNullOrWhiteSpace(txtUserName.Text))
        {
            errorProvider.SetError(txtUserName, "اسم المستخدم مطلوب");
            txtUserName.Focus();
            return false;
        }

        errorProvider.SetError(txtUserName, "");
        return true;
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى إعداداتها الافتراضية (RULE-N148):
    /// أدوات التصفية ← الدلائل (الفروع) ← المستخدمون ← المحرر ووضع العرض.
    /// «مسح التصفية» وحده هو الذي يمسح التصفية دون إعادة تحميل.
    /// </summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _ClearFilterInputs();          // التصفية تعود للافتراضي
            await _LoadBranchesAsync();    // قد تتغيّر من شاشة الفروع
            await _LoadUsersAsync();       // إعادة قراءة المستخدمين (يعيد تطبيق التصفية والملخّص)
            _ResetForm();                  // تفريغ المحرر + وضع العرض + الحقول معطّلة
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث بيانات المستخدمين");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>زر الخروج: إغلاق شاشة إدارة المستخدمين.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmUserMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadBranchesAsync();
            await _LoadUsersAsync();
            _ResetForm();
            ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة المستخدمين");
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
        GreenThemeHelper.ApplyGrid(dgvUsers);
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        // RULE-N134/RULE-N92: لون زر «إعادة تنشيط» من ثوابت الثيم (مطابق لقيمة المصمّم).
        btnReactivate.BackColor = GreenThemeHelper.ActionInfoBackground;

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه)
        dgvUsers.ReadOnly = true;
        dgvUsers.AllowUserToAddRows = false;
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsers.MultiSelect = false;

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Lookups ───────────────────────────────────────────────────────────────

    private async Task _LoadBranchesAsync()
    {
        try
        {
            var result = await BranchService.GetAllAsync(includeInactive: false);

            if (result.IsSuccess && result.Value != null)
            {
                _branchLookup = result.Value.ToList();
            }
            else
            {
                _branchLookup = new List<BranchDto>();
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة الفروع.");
            }

            // عنصر افتراضي: بدون فرع (BranchId = null).
            _branchLookup.Insert(0, new BranchDto(0, "-- بدون فرع --"));

            _isBindingLookups = true;
            try
            {
                cmbBranch.DisplayMember = "Name";
                cmbBranch.ValueMember = "Id";
                cmbBranch.DataSource = _branchLookup;
                cmbBranch.SelectedIndex = 0;
            }
            finally
            {
                _isBindingLookups = false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الفروع");
            clsDialogs.Error("تعذر تحميل قائمة الفروع. يرجى إعادة المحاولة.");
        }
    }

    private void _SelectBranch(short? branchId)
    {
        cmbBranch.SelectedValue = (short)(branchId is > 0 ? branchId.Value : 0);

        if (cmbBranch.SelectedValue is not short selected || selected != (branchId ?? 0))
            cmbBranch.SelectedIndex = 0;
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadUsersAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("UserName", typeof(string));
        table.Columns.Add("IsLocked", typeof(bool));
        table.Columns.Add("IsActive", typeof(bool));
        table.Columns.Add("MustChangePassword", typeof(bool));   // عمود تصفية فقط (غير معروض في الشبكة)

        try
        {
            var result = await UserService.GetAllAsync();

            _usersById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var u in result.Value)
                {
                    _usersById[u.Id] = u;
                    table.Rows.Add(
                        u.Id,
                        u.UserName,
                        u.IsLocked,
                        u.IsActive,
                        u.MustChangePassword);
                }
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة المستخدمين.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة المستخدمين");
            clsDialogs.Error("تعذر تحميل قائمة المستخدمين. يرجى إعادة المحاولة.");
        }
        finally
        {
            _usersTable = table;
            _usersView = new DataView(_usersTable);
            dgvUsers.DataSource = _usersView;
            _ApplyGridHeaders();
            _ApplyFilter();   // يشمل تلوين الأسطر + شريط الملخص (RULE-N61)
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يعتمد على <c>DataColumn.Caption</c>، بل تُضبط <c>HeaderText</c> صراحةً
    /// لكل عمود ظاهر بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvUsers.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم المستخدم",
                "UserName" => "اسم المستخدم",
                "IsLocked" => "مقفل",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvUsers.Rows)
        {
            if (row.DataBoundItem is not DataRowView dataRow) continue;

            bool isActive = dataRow["IsActive"] is bool active && active;
            bool isLocked = dataRow["IsLocked"] is bool locked && locked;

            if (!isActive)
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
                row.DefaultCellStyle.ForeColor = Color.DarkRed;
            }
            else if (isLocked)
            {
                row.DefaultCellStyle.BackColor = Color.LemonChiffon;
                row.DefaultCellStyle.ForeColor = Color.DarkOrange;
            }
        }
    }

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyLocked_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyMustChangePassword_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

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
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
    /// بلا إعادة تحميل ولا لمس للمحرر (RULE-N148). «تحديث» هو المسؤول عن إعادة تحميل البيانات.
    /// </summary>
    private void btnClearFilters_Click(object? sender, EventArgs e)
    {
        _ClearAllFilters();
        txtSearch.Focus();
    }

    /// <summary>تفريغ أدوات التصفية وحدها (بلا تحميل) — تُستهلك من «مسح التصفية» و«تحديث».</summary>
    private void _ClearFilterInputs()
    {
        txtSearch.Clear();
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
        chkShowInactive.Checked = false;
        chkOnlyLocked.Checked = false;
        chkOnlyMustChangePassword.Checked = false;
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
        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtSearch.Text)
            || cbStatus.SelectedIndex > 0
            || cbFilterBy.SelectedIndex > 0
            || chkShowInactive.Checked
            || chkOnlyLocked.Checked
            || chkOnlyMustChangePassword.Checked;
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59/N60): كل الشروط تُبنى من أسماء أعمدة معروفة،
    /// والاختيار يُقرأ بالمؤشر لا بالنص العربي، والقيمة تُهرَّب قبل بنائها في RowFilter.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_usersView is null || _usersView.Table is null) return;
        if (txtSearch is null || cbFilterBy is null || cbStatus is null
            || chkShowInactive is null || chkOnlyLocked is null || chkOnlyMustChangePassword is null)
            return;

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

        // ── البحث النصي: عمود واحد فقط يحدده cbFilterBy (RULE-N60) ──
        if (text.Length > 0)
        {
            conditions.Add(cbFilterBy.SelectedIndex == 1
                ? $"CONVERT(Id, 'System.String') LIKE '{RowFilterHelper.Escape(text)}%'"   // رقم المستخدم
                : RowFilterHelper.Like("UserName", text));                                   // اسم المستخدم (الافتراضي)
        }

        // ── مربعات الاختيار: المقفلون / الملزمون بتغيير كلمة المرور ──
        if (chkOnlyLocked.Checked)
            conditions.Add("IsLocked = true");

        if (chkOnlyMustChangePassword.Checked)
            conditions.Add("MustChangePassword = true");

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _usersView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية دليل المستخدمين — أُبقيت التصفية السابقة كما هي");
        }

        _ApplyRowColoring();
        _UpdateSummary();
        _UpdateClearFiltersState();   // RULE-N148: زر «مسح التصفية» يتبع وجود تصفية نشطة
    }

    /// <summary>
    /// شريط الملخص (RULE-N61): المعروض من الإجمالي + النشط/المعطّل/المقفلون،
    /// ويُعاد حسابه بعد كل تصفية وكل تحميل.
    /// </summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null || _usersView?.Table is null) return;

        int total = _usersTable.Rows.Count;
        int shown = _usersView.Count;

        int active = 0, inactive = 0, locked = 0;

        foreach (DataRowView row in _usersView)
        {
            if (row["IsActive"] is bool isActive && isActive) active++;
            else inactive++;

            if (row["IsLocked"] is bool isLocked && isLocked) locked++;
        }

        lblSummary.Text =
            $"المعروض: {shown} من {total} — نشط: {active} · معطّل: {inactive} · مقفلون: {locked}";

        // «لا توجد بيانات» يُعلن في شريط الملخص بدل إضافة صف وهمي إلى الشبكة
        // (الصف الوهمي يُفسد التصدير والتصفية — RULE-N22 / RULE-N61).
        if (shown == 0) lblSummary.Text += " — لا توجد بيانات مطابقة";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvUsers_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvUsers.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مستخدم من الشبكة");
            clsDialogs.Error("تعذر عرض بيانات المستخدم المحدد. يرجى إعادة المحاولة.");
        }
    }

    private void _FillForm(int userId)
    {
        if (!_usersById.TryGetValue(userId, out var u)) return;

        _editId = userId;
        _mode = EntryMode.Editing;   // RULE-N151: اختيار سجل من الشبكة = وضع التعديل

        errorProvider.Clear();

        txtCode.Text = userId.ToString();

        // UpdateUserRequest لا يحتوي على اسم المستخدم → الاسم للقراءة فقط في وضع التعديل.
        txtUserName.Text = u.UserName;

        _isBindingLookups = true;
        try
        {
            _SelectBranch(u.BranchId);
        }
        finally
        {
            _isBindingLookups = false;
        }

        // كلمة المرور لا تُعرض أبداً — تُترك فارغة، ويُحدَّث بها فقط عند إدخال قيمة جديدة.
        txtPassword.Clear();

        chkIsLocked.Checked = u.IsLocked;
        chkIsActive.Checked = u.IsActive;
        chkMustChangePassword.Checked = u.MustChangePassword;

        // بعد التعبئة: تفعيل الحقول + ضبط الأزرار + شريط الوضع (RULE-N150).
        _ApplyModeState();
    }

    // ── Mode / Reset (RULE-N149 … RULE-N152) ──────────────────────────────────

    /// <summary>
    /// آلة الحالة المركزية للشاشة: تفعيل/تعطيل حقول الإدخال، وحالة كل زر، ونص شريط الوضع،
    /// والزر الافتراضي (Enter). تُستدعى بعد **كل** انتقال حالة:
    /// تحميل · «جديد» · اختيار سجل · حفظ ناجح · حذف · «تحديث» · Esc (RULE-N150).
    /// </summary>
    private void _ApplyModeState()
    {
        bool editingSession = _mode != EntryMode.Browsing;

        _SetEditorEnabled(editingSession);

        // اسم المستخدم غير قابل للتعديل بعد الإنشاء (UpdateUserRequest لا يحتوي عليه).
        txtUserName.ReadOnly = _mode == EntryMode.Editing;

        // «يجب تغيير كلمة المرور» للقراءة فقط في كل الأوضاع.
        chkMustChangePassword.Enabled = false;

        // آلة حالة الأزرار (RULE-N43): «جديد/حفظ» في الإضافة، «تعديل/حذف» عند وجود سجل محدد.
        btnNew.Enabled = true;
        btnRefresh.Enabled = true;
        btnClose.Enabled = true;
        btnSave.Enabled = _mode == EntryMode.Adding;
        btnEdit.Enabled = _mode == EntryMode.Editing;
        btnDelete.Enabled = _mode == EntryMode.Editing;

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
    /// آلة حالة زر «إعادة تنشيط» (RULE-N92): مفعّل فقط في وضع التعديل عندما يكون السجل المحدد
    /// **غير نشط**، ومعطّل عند عدم وجود تحديد أو كون السجل نشطاً أو أثناء عملية جارية.
    /// </summary>
    private void _UpdateReactivateButton()
    {
        bool isInactive = _mode == EntryMode.Editing
                          && _editId is int id
                          && _usersById.TryGetValue(id, out var user)
                          && !user.IsActive;

        btnReactivate.Enabled = isInactive && !_reactivateBusy;   // RULE-N92
    }

    /// <summary>حقول المحرر التي تُعطَّل في وضع العرض وتُفعَّل عند «جديد» أو عند تحديد سجل.</summary>
    private Control[] _EditorInputs() => new Control[]
    {
        txtUserName, txtPassword, cmbBranch, chkIsLocked, chkIsActive
    };

    /// <summary>تفعيل أو تعطيل كل حقول المحرر دفعة واحدة (txtCode يبقى للقراءة دائماً).</summary>
    private void _SetEditorEnabled(bool enabled)
    {
        foreach (var input in _EditorInputs())
            input.Enabled = enabled;
    }

    /// <summary>
    /// شريط الوضع (RULE-N44): «وضع العرض» افتراضياً · «إضافة مستخدم جديد» · «تعديل مستخدم: &lt;الاسم&gt;».
    /// </summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _mode switch
        {
            EntryMode.Adding => $"{FormTitle} — إضافة مستخدم جديد",
            EntryMode.Editing => $"{FormTitle} — تعديل مستخدم: {txtUserName.Text.Trim()}",
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
        txtUserName.Clear();
        txtUserName.ReadOnly = false;
        txtPassword.Clear();

        chkIsLocked.Checked = false;
        chkIsActive.Checked = true;
        chkMustChangePassword.Checked = false;

        _isBindingLookups = true;
        try
        {
            if (cmbBranch.Items.Count > 0)
                cmbBranch.SelectedIndex = 0;
        }
        finally
        {
            _isBindingLookups = false;
        }

        dgvUsers.ClearSelection();

        _ApplyModeState();   // الحقول معطّلة + «جديد» فقط + شريط الوضع = وضع العرض
        btnNew.Focus();
    }

    /// <summary>
    /// بدء إدخال مستخدم جديد (زر «جديد» / F2): تفريغ الحقول ثم **تفعيلها** وتحويل شريط الوضع
    /// إلى «إضافة مستخدم جديد» (RULE-N151).
    /// </summary>
    private void _StartNewEntry()
    {
        _ResetForm();

        _mode = EntryMode.Adding;
        _ApplyModeState();

        txtUserName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtUserName.Text))
        {
            errorProvider.SetError(txtUserName, "اسم المستخدم مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtUserName, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _StartNewEntry();

    /// <summary>زر الإضافة: يُضيف مستخدماً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المستخدم المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ المستخدم: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
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
            string editName = txtUserName.Text.Trim();
            if (!clsDialogs.ConfirmEdit($"هل أنت متأكد من تعديل المستخدم \"{editName}\"؟"))
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
        btnEdit.Enabled = false;

        try
        {
            string userName = txtUserName.Text.Trim();
            short? branchId = _SelectedBranchId();
            string? password = string.IsNullOrWhiteSpace(txtPassword.Text)
                ? null
                : txtPassword.Text;

            Result<UserDto> result;

            if (edit)
            {
                // UpdateUserRequest لا يحتوي على اسم المستخدم — يُحفظ عبر CreateUserRequest فقط.
                // TODO: لا يوجد DefaultCashBoxId في الشاشة → يُمرَّر null.
                var request = new UpdateUserRequest(
                    branchId,
                    chkIsLocked.Checked,
                    chkIsActive.Checked,
                    password,
                    DefaultCashBoxId: null);

                result = await UserService.UpdateAsync(_editId!.Value, request);
            }
            else
            {
                // CreateUserRequest: كلمة المرور اختيارية — عند تركها فارغة يفرض النظام تغييرها عند أول دخول.
                var request = new CreateUserRequest(
                    userName,
                    branchId,
                    password,
                    DefaultCashBoxId: null);

                result = await UserService.CreateAsync(request);
            }

            // TODO: إعادة تعيين كلمة المرور (IUserDesktopService.ResetPasswordAsync) غير منفذة في هذه الشاشة.

            if (result.IsSuccess && result.Value != null)
            {
                clsDialogs.Success(edit ? "تم تعديل المستخدم بنجاح." : "تمت إضافة المستخدم بنجاح.");

                // بعد الحفظ الناجح (إضافة أو تعديل): يصبح السجل محفوظاً ومحدداً → وضع التعديل،
                // ثم تحديث القائمة وإعادة تعبئة الحقول من البيانات المحفوظة (تحديث تلقائي — RULE-N151).
                _editId = result.Value.Id;
                _mode = EntryMode.Editing;

                await _LoadUsersAsync();
                _FillForm(result.Value.Id);   // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _ApplyModeState();            // ضمان الحالة حتى لو لم يُعثر على السطر
                _SelectGridRow(result.Value.Id);
            }
            else
            {
                clsDialogs.Error($"لم يتم حفظ المستخدم.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ المستخدم");
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
            clsDialogs.Warning("يرجى اختيار مستخدم من القائمة أولاً.");
            return;
        }

        string userName = txtUserName.Text.Trim();

        if (!clsDialogs.Confirm($"هل أنت متأكد من حذف (تعطيل) المستخدم \"{userName}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await UserService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                clsDialogs.Success("تم تعطيل المستخدم بنجاح.");

                // بعد الحذف: الرجوع إلى وضع العرض (الحقول معطّلة) ثم تحديث القائمة تلقائياً،
                // ويبقى السطر المعطّل محدداً إن كان معروضاً (RULE-N151).
                await _LoadUsersAsync();
                _ResetForm();
                _SelectGridRow(id);
            }
            else
            {
                clsDialogs.Error($"لم يتم تعطيل المستخدم.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل المستخدم {UserId}", id);
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
    /// إعادة تنشيط المستخدم المعطَّل (RULE-N90…RULE-N94): الزر مفعّل فقط على سجل غير نشط،
    /// والتنفيذ داخل الخدمة (BLL) داخل معاملة، ويُلغى قفل الحساب مع إعادة التنشيط.
    /// </summary>
    private async void btnReactivate_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            clsDialogs.Warning("يرجى اختيار مستخدم معطّل من القائمة أولاً.");
            return;
        }

        if (!_usersById.TryGetValue(id, out var user))
        {
            clsDialogs.Warning("تعذر العثور على بيانات المستخدم المحدد. يرجى تحديث القائمة.");
            return;
        }

        if (user.IsActive)
        {
            clsDialogs.Warning("المستخدم المحدد نشط بالفعل.");
            _UpdateReactivateButton();
            return;
        }

        if (_reactivateBusy) return;

        if (!clsDialogs.Confirm($"هل تريد إعادة تنشيط المستخدم \"{user.UserName}\"؟"))
            return;

        Cursor = Cursors.WaitCursor;
        _reactivateBusy = true;
        _UpdateReactivateButton();

        try
        {
            var result = await UserService.ReactivateAsync(id);

            if (result.IsSuccess)
            {
                Log.Information("أُعيد تنشيط المستخدم {UserId} بنجاح", id);

                await _LoadUsersAsync();

                // RULE-N94: يبقى السطر المُعاد تنشيطه محدداً، وتُعاد تعبئة الحقول من البيانات
                // المحدَّثة في وضع التعديل (تحديث تلقائي) — فتظهر حالة «نشط» والقفل ملغى.
                _editId = id;
                _mode = EntryMode.Editing;
                _FillForm(id);        // يعبّئ الحقول ويضبط الأزرار وشريط الوضع
                _SelectGridRow(id);

                clsDialogs.Success("تم إعادة تنشيط المستخدم بنجاح وإلغاء قفل الحساب إن كان مقفلاً.");
            }
            else
            {
                Log.Warning("تعذر إعادة تنشيط المستخدم {UserId}. السبب: {Error}", id, result.Error);

                clsDialogs.Error($"تعذر إعادة تنشيط المستخدم.\n{result.Error}");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تنشيط المستخدم {UserId}", id);
            clsDialogs.Error("حدث خطأ أثناء إعادة التنشيط. يرجى إعادة المحاولة.");
        }
        finally
        {
            _reactivateBusy = false;
            _ApplyModeState();   // السجل نشط بعد إعادة التنشيط → زر إعادة التنشيط معطّل (RULE-N92)
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int userId)
    {
        foreach (DataGridViewRow row in dgvUsers.Rows)
        {
            if (row.DataBoundItem is not DataRowView dataRow) continue;

            if (dataRow["Id"] is int id && id == userId)
            {
                row.Selected = true;
                dgvUsers.CurrentCell = row.Cells[0];
                break;
            }
        }
    }

    // ── Context Menu / Double click (RULE-N23) ────────────────────────────────

    /// <summary>النقر المزدوج على السطر = تحميل بياناته في المحرّر (نفس مسار النقر المفرد).</summary>
    private void dgvUsers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        dgvUsers_CellClick(sender, e);
    }

    /// <summary>
    /// حالة بنود قائمة السياق قبل ظهورها (RULE-N23): بنود التعديل/التعطيل تُفعَّل فقط عند
    /// وجود سطر محدد، وتتبع آلة حالة الأزرار نفسها (RULE-N43) بلا منطق مكرر.
    /// </summary>
    private void cmsGrid_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        bool hasRow = dgvUsers.CurrentRow?.DataBoundItem is DataRowView;

        mnuEdit.Enabled = hasRow;
        mnuDeactivate.Enabled = hasRow && btnDelete.Enabled;
        mnuRefresh.Enabled = btnRefresh.Enabled;
        mnuExport.Enabled = dgvUsers.Rows.Count > 0;
    }

    /// <summary>تعديل السطر المحدد = تحميله في المحرّر ثم نقل المؤشر إلى أول حقل.</summary>
    private void mnuEdit_Click(object? sender, EventArgs e)
    {
        if (dgvUsers.CurrentRow is not DataGridViewRow row) return;

        dgvUsers_CellClick(dgvUsers, new DataGridViewCellEventArgs(0, row.Index));
        txtUserName.Focus();
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
            if (dgvUsers.Rows.Count == 0)
            {
                clsDialogs.Warning("لا توجد بيانات معروضة للتصدير.");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"المستخدمون_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير قائمة المستخدمين إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvUsers, "المستخدمون", dialog.FileName);

            clsDialogs.Success("تم تصدير البيانات بنجاح.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تصدير قائمة المستخدمين إلى Excel");
            clsDialogs.Error("تعذر تصدير البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
