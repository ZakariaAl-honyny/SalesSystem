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

        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmUserMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnEdit.Click += btnEdit_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvUsers.CellClick += dgvUsers_CellClick;
    }

    /// <summary>الفرع المحدد (0 = بدون فرع).</summary>
    private short? _SelectedBranchId()
        => cmbBranch.SelectedValue is short branchId && branchId > 0 ? branchId : null;

    /// <summary>
    /// هل الحقول المطلوبة مكتملة؟ (اسم المستخدم).
    /// </summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtUserName.Text);

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    private void _FocusFirstMissingField()
    {
        if (string.IsNullOrWhiteSpace(txtUserName.Text))
        {
            errorProvider.SetError(txtUserName, "اسم المستخدم مطلوب");
            txtUserName.Focus();
        }
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

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmUserMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
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
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveUserAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المستخدم المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveUserAsync(edit: true);

    /// <summary>
    /// حفظ المستخدم: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveUserAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقول المطلوبة الناقصة: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_RequiredFieldsValid())
        {
            _FocusFirstMissingField();
            return;
        }

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
            if (row.Cells["Id"].Value is int id && id == userId)
            {
                row.Selected = true;
                dgvUsers.CurrentCell = row.Cells["UserName"];
                break;
            }
        }
    }
}
