using System.Data;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
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
    private IRoleApiService RoleService => _serviceProvider.GetRequiredService<IRoleApiService>();
    private IBranchApiService BranchService => _serviceProvider.GetRequiredService<IBranchApiService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, UserDto> _usersById = new();
    private DataTable _usersTable = new();
    private DataView _usersView = new();

    /// <summary>قائمة الأدوار المتاحة لاختيار دور المستخدم.</summary>
    private List<RoleDto> _roleLookup = new();

    /// <summary>قائمة الفروع المتاحة لاختيار فرع المستخدم.</summary>
    private List<BranchDto> _branchLookup = new();

    /// <summary>null = إضافة مستخدم جديد، &gt; 0 = تعديل مستخدم قائم.</summary>
    private int? _editId;

    /// <summary>يمنع إعادة التقييم أثناء تعبئة القوائم برمجياً.</summary>
    private bool _isBindingLookups;

    /// <summary>يُفعَّل عند محاولة حفظ ناقصة — يُعطّل زر الحفظ حتى تكتمل الحقول المطلوبة.</summary>
    private bool _validationFailed;

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
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmUserMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvUsers.CellClick += dgvUsers_CellClick;

        // التحقق الحيّ: يُفعَّل زر الحفظ افتراضياً، ويُعطَّل فقط بعد محاولة حفظ ناقصة
        // ثم يُعاد تفعيله تلقائياً عند إكمال الحقول المطلوبة.
        txtUserName.TextChanged += (s, e) => _UpdateSaveEnabled();
        cmbRole.SelectedIndexChanged += (s, e) => _UpdateSaveEnabled();
    }

    /// <summary>الدور المحدد (0 = لم يُحدَّد).</summary>
    private int _SelectedRoleId()
        => cmbRole.SelectedValue is int roleId ? roleId : 0;

    /// <summary>الفرع المحدد (0 = بدون فرع).</summary>
    private short? _SelectedBranchId()
        => cmbBranch.SelectedValue is short branchId && branchId > 0 ? branchId : null;

    /// <summary>
    /// هل الحقول المطلوبة مكتملة؟ (اسم المستخدم + الدور).
    /// الدور مطلوب لأن <c>CreateUserRequest.RoleId</c> إلزامي.
    /// </summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtUserName.Text) && _SelectedRoleId() > 0;

    /// <summary>أسماء الحقول المطلوبة الناقصة (بترتيب ظهورها في النموذج).</summary>
    private List<string> _MissingRequiredNames()
    {
        var missing = new List<string>();

        if (string.IsNullOrWhiteSpace(txtUserName.Text))
            missing.Add("اسم المستخدم");

        if (_SelectedRoleId() <= 0)
            missing.Add("الدور");

        return missing;
    }

    /// <summary>
    /// تحديث حالة زر الحفظ: يبدأ مفعّلاً، ويُعطَّل فقط بعد محاولة حفظ ناقصة،
    /// ثم يُعاد تفعيله تلقائياً عند إكمال الحقول المطلوبة.
    /// </summary>
    private void _UpdateSaveEnabled()
    {
        if (_isBindingLookups) return;

        if (_validationFailed)
        {
            // re-enable once the user has filled all required fields
            if (_RequiredFieldsValid())
                _validationFailed = false;
        }

        btnSave.Enabled = !_validationFailed;
    }

    /// <summary>تحديث يدوي لقائمة المستخدمين مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadRolesAsync();
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

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmUserMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadRolesAsync();
            await _LoadBranchesAsync();
            await _LoadUsersAsync();
            _ResetForm();
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

    // ── Lookups ───────────────────────────────────────────────────────────────

    private async Task _LoadRolesAsync()
    {
        try
        {
            var result = await RoleService.GetAllAsync(includeInactive: false);

            if (result.IsSuccess && result.Value != null)
            {
                _roleLookup = result.Value.ToList();
                _roleLookup.Insert(0, new RoleDto(0, "-- اختر الدور --", null, true, false));
            }
            else
            {
                _roleLookup = new List<RoleDto> { new RoleDto(0, "-- اختر الدور --", null, true, false) };

                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الأدوار.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _isBindingLookups = true;
            try
            {
                cmbRole.DisplayMember = "Name";
                cmbRole.ValueMember = "Id";
                cmbRole.DataSource = _roleLookup;
                cmbRole.SelectedIndex = 0;
            }
            finally
            {
                _isBindingLookups = false;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الأدوار");
            MessageBox.Show("تعذر تحميل قائمة الأدوار. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

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

    private void _SelectRole(int roleId)
    {
        // RoleDto.Id من نوع int → يجب تمرير int لمطابقة ValueMember.
        cmbRole.SelectedValue = roleId;

        if (cmbRole.SelectedValue is not int selected || selected != roleId)
            cmbRole.SelectedIndex = 0; // لم يتم العثور على الدور → العنصر الافتراضي
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
                        u.IsActive);
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
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvUsers.Rows)
        {
            bool isActive = row.Cells["IsActive"].Value is bool active && active;
            bool isLocked = row.Cells["IsLocked"].Value is bool locked && locked;

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

    private void _ApplyFilter()
    {
        if (_usersView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _usersView.RowFilter = "";
            return;
        }

        _usersView.RowFilter =
            $"UserName LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
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

        // وضع التعديل: زر الحفظ يُحدّث السجل المحدد، وزر الحذف متاح.
        btnSave.Text = "💾 تعديل";
        btnDelete.Enabled = true;

        errorProvider.Clear();

        txtCode.Text = userId.ToString();

        // UpdateUserRequest لا يحتوي على اسم المستخدم → الاسم للقراءة فقط في وضع التعديل.
        txtUserName.Text = u.UserName;
        txtUserName.ReadOnly = true;

        _isBindingLookups = true;
        try
        {
            _SelectRole(u.RoleId);
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

        _validationFailed = false;
        _UpdateSaveEnabled();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( إدارة المستخدمين )) — تعديل مستخدم"
            : "(( إدارة المستخدمين )) — إضافة مستخدم جديد";

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الحفظ يُضيف سجلاً جديداً، وزر الحذف معطّل لعدم وجود سجل محدد.
        btnSave.Text = "💾 إضافة";
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
            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;

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

        _validationFailed = false;
        _UpdateSaveEnabled();
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

        if (_SelectedRoleId() <= 0)
        {
            errorProvider.SetError(cmbRole, "يرجى اختيار دور المستخدم");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbRole, "");
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
        // الحقول المطلوبة الناقصة: رسالة عربية محددة + تعطيل زر الحفظ حتى تعبئتها.
        if (!_RequiredFieldsValid())
        {
            _validationFailed = true;
            _UpdateSaveEnabled();

            var missing = string.Join("، ", _MissingRequiredNames());
            MessageBox.Show("يرجى تعبئة الحقول المطلوبة: " + missing, "بيانات ناقصة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
                errorProvider.SetError(txtUserName, "اسم المستخدم مطلوب");
            if (_SelectedRoleId() <= 0)
                errorProvider.SetError(cmbRole, "يرجى اختيار دور المستخدم");

            return;
        }

        if (!_ValidateForm()) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string userName = txtUserName.Text.Trim();
            short roleId = (short)_SelectedRoleId();
            short? branchId = _SelectedBranchId();
            string? password = string.IsNullOrWhiteSpace(txtPassword.Text)
                ? null
                : txtPassword.Text;

            Result<UserDto> result;

            if (_editId is int id)
            {
                // UpdateUserRequest لا يحتوي على اسم المستخدم — يُحفظ عبر CreateUserRequest فقط.
                // TODO: لا يوجد DefaultCashBoxId في الشاشة → يُمرَّر null.
                var request = new UpdateUserRequest(
                    roleId,
                    branchId,
                    chkIsLocked.Checked,
                    chkIsActive.Checked,
                    password,
                    DefaultCashBoxId: null);

                result = await UserService.UpdateAsync(id, request);
            }
            else
            {
                // CreateUserRequest: كلمة المرور اختيارية — عند تركها فارغة يفرض النظام تغييرها عند أول دخول.
                var request = new CreateUserRequest(
                    userName,
                    roleId,
                    branchId,
                    password,
                    DefaultCashBoxId: null);

                result = await UserService.CreateAsync(request);
            }

            // TODO: إعادة تعيين كلمة المرور (IUserApiService.ResetPasswordAsync) غير منفذة في هذه الشاشة.

            if (result.IsSuccess && result.Value != null)
            {
                MessageBox.Show(wasEditing ? "تم تعديل المستخدم بنجاح." : "تمت إضافة المستخدم بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;

                // بعد الحفظ ينتقل النموذج إلى وضع التعديل (السجل محدد).
                btnSave.Text = "💾 تعديل";
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
            btnSave.Enabled = true;
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
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زر الحذف معطّلاً.
            btnDelete.Enabled = _editId.HasValue;
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int userId)
    {
        foreach (DataGridViewRow row in dgvUsers.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == userId)
            {
                row.Selected = true;
                dgvUsers.CurrentCell = row.Cells["UserName"];
                break;
            }
        }
    }
}
