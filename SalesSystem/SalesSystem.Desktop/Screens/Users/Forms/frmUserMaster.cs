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

    private IUserApiService UserService => _serviceProvider.GetRequiredService<IUserApiService>();
    private IBranchApiService BranchService => _serviceProvider.GetRequiredService<IBranchApiService>();

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

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete);

        // الاختصارات الموحّدة (RULE-N47): F2 جديد · F5 تحديث · Delete تعطيل · Esc مسح التصفية/إلغاء.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDelete),
            (Keys.Escape, _GlobalEscape));

        // RULE-N60: أدوات التصفية تُقرأ بالمؤشر — «الحالة: الكل» و«تصفية بـ: اسم المستخدم» افتراضياً.
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
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

    /// <summary>تحديث يدوي لقائمة المستخدمين مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadBranchesAsync();
            await _LoadUsersAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث بيانات المستخدمين");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الفروع.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MessageBox.Show("تعذر تحميل قائمة الفروع. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة المستخدمين.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة المستخدمين");
            MessageBox.Show("تعذر تحميل قائمة المستخدمين. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية (RULE-N59).</summary>
    private void _ClearAllFilters()
    {
        txtSearch.Clear();
        cbStatus.SelectedIndex = 0;
        cbFilterBy.SelectedIndex = 0;
        chkShowInactive.Checked = false;
        chkOnlyLocked.Checked = false;
        chkOnlyMustChangePassword.Checked = false;

        _ApplyFilter();
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
            MessageBox.Show("تعذر عرض بيانات المستخدم المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int userId)
    {
        if (!_usersById.TryGetValue(userId, out var u)) return;

        _editId = userId;

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;

        errorProvider.Clear();

        txtCode.Text = userId.ToString();

        // UpdateUserRequest لا يحتوي على اسم المستخدم → الاسم للقراءة فقط في وضع التعديل.
        txtUserName.Text = u.UserName;
        txtUserName.ReadOnly = true;

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

        _UpdateModeHeader();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( إدارة المستخدمين )) — تعديل مستخدم"
            : "(( إدارة المستخدمين )) — إضافة مستخدم جديد";

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الإضافة متاح، وزرّي التعديل والحذف معطّلان لعدم وجود سجل محدد.
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;

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
        _UpdateModeHeader();
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
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _ResetForm();

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
            if (MessageBox.Show($"هل أنت متأكد من تعديل المستخدم \"{editName}\"؟",
                    "تأكيد التعديل", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
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

            // TODO: إعادة تعيين كلمة المرور (IUserApiService.ResetPasswordAsync) غير منفذة في هذه الشاشة.

            if (result.IsSuccess && result.Value != null)
            {
                MessageBox.Show(edit ? "تم تعديل المستخدم بنجاح." : "تمت إضافة المستخدم بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). النصوص ثابتة في المصمم.
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                await _LoadUsersAsync();
                _SelectGridRow(result.Value.Id);
                txtUserName.ReadOnly = true;
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ المستخدم.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ المستخدم");
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
            MessageBox.Show("يرجى اختيار مستخدم من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string userName = txtUserName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) المستخدم \"{userName}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await UserService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل المستخدم بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
                await _LoadUsersAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل المستخدم.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل المستخدم {UserId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زرّا التعديل والحذف معطّلين.
            btnDelete.Enabled = _editId.HasValue;
            btnEdit.Enabled = _editId.HasValue;
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
}
