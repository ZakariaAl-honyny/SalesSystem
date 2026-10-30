using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Roles.Forms;

/// <summary>
/// شاشة موحّدة (Master Form) لدليل الأدوار.
/// تجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// وتعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmRoleMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IRoleApiService RoleService => _serviceProvider.GetRequiredService<IRoleApiService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, RoleDto> _rolesById = new();
    private DataTable _rolesTable = new();
    private DataView _rolesView = new();

    /// <summary>null = إضافة دور جديد، &gt; 0 = تعديل دور قائم.</summary>
    private int? _editId;

    /// <summary>الدور المحدد دور نظام (لا يُحذف) — يُعطَّل زر الحذف.</summary>
    private bool _isSystemRole;

    /// <summary>يُفعَّل عند محاولة حفظ ناقصة — يُعطّل زر الحفظ حتى تكتمل الحقول المطلوبة.</summary>
    private bool _validationFailed;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmRoleMaster()
    {
        InitializeComponent();
    }

    public frmRoleMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmRoleMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvRoles.CellClick += dgvRoles_CellClick;

        // التحقق الحيّ: يُفعَّل زر الحفظ افتراضياً، ويُعطَّل فقط بعد محاولة حفظ ناقصة
        // ثم يُعاد تفعيله تلقائياً عند إدخال اسم الدور.
        txtName.TextChanged += (s, e) => _UpdateSaveEnabled();
    }

    /// <summary>هل الحقل المطلوب (اسم الدور) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>أسماء الحقول المطلوبة الناقصة (اسم الدور).</summary>
    private List<string> _MissingRequiredNames()
    {
        var missing = new List<string>();

        if (string.IsNullOrWhiteSpace(txtName.Text))
            missing.Add("اسم الدور");

        return missing;
    }

    /// <summary>
    /// تحديث حالة زر الحفظ: يبدأ مفعّلاً، ويُعطَّل فقط بعد محاولة حفظ ناقصة،
    /// ثم يُعاد تفعيله تلقائياً عند إكمال الحقول المطلوبة.
    /// </summary>
    private void _UpdateSaveEnabled()
    {
        if (_validationFailed)
        {
            // re-enable once the user has filled all required fields
            if (_RequiredFieldsValid())
                _validationFailed = false;
        }

        btnSave.Enabled = !_validationFailed;
    }

    /// <summary>تحديث يدوي لقائمة الأدوار مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadRolesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث بيانات الأدوار");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmRoleMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadRolesAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الأدوار");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadRolesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await RoleService.GetAllAsync(includeInactive: true);

            _rolesById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var r in result.Value)
                {
                    _rolesById[r.Id] = r;
                    table.Rows.Add(
                        r.Id,
                        r.Name,
                        r.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الأدوار.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الأدوار");
            MessageBox.Show("تعذر تحميل قائمة الأدوار. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _rolesTable = table;
            _rolesView = new DataView(_rolesTable);
            dgvRoles.DataSource = _rolesView;
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvRoles.Rows)
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

    private void _ApplyFilter()
    {
        if (_rolesView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _rolesView.RowFilter = "";
            return;
        }

        _rolesView.RowFilter =
            $"Name LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvRoles_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvRoles.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار دور من الشبكة");
            MessageBox.Show("تعذر عرض بيانات الدور المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int roleId)
    {
        if (!_rolesById.TryGetValue(roleId, out var r)) return;

        _editId = roleId;
        _isSystemRole = r.IsSystem;

        // وضع التعديل: زر الحفظ يُحدّث السجل المحدد، وزر الحذف متاح (ما عدا أدوار النظام).
        btnSave.Text = "💾 تعديل";
        btnDelete.Enabled = !r.IsSystem;

        errorProvider.Clear();

        txtCode.Text = roleId.ToString();
        txtName.Text = r.Name;
        txtDescription.Text = r.Description ?? "";

        numSalesPermissions.Value = _Clamp(numSalesPermissions, r.SalesPermissions);
        numInventoryPermissions.Value = _Clamp(numInventoryPermissions, r.InventoryPermissions);
        numAccountingPermissions.Value = _Clamp(numAccountingPermissions, r.AccountingPermissions);
        numSystemPermissions.Value = _Clamp(numSystemPermissions, r.SystemPermissions);

        chkIsActive.Checked = r.IsActive;
        chkIsSystem.Checked = r.IsSystem;

        _UpdateModeHeader();

        _validationFailed = false;
        _UpdateSaveEnabled();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل الأدوار )) — تعديل دور"
            : "(( دليل الأدوار )) — إضافة دور جديد";

    private void _ResetForm()
    {
        _editId = null;
        _isSystemRole = false;

        // وضع الإضافة: زر الحفظ يُضيف سجلاً جديداً، وزر الحذف معطّل لعدم وجود سجل محدد.
        btnSave.Text = "💾 إضافة";
        btnDelete.Enabled = false;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtDescription.Clear();

        numSalesPermissions.Value = 0;
        numInventoryPermissions.Value = 0;
        numAccountingPermissions.Value = 0;
        numSystemPermissions.Value = 0;

        chkIsActive.Checked = true;
        chkIsSystem.Checked = false;

        dgvRoles.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();

        _validationFailed = false;
        _UpdateSaveEnabled();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الدور مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
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
        // الحقل المطلوب الناقص: رسالة عربية محددة + تعطيل زر الحفظ حتى تعبئته.
        if (!_RequiredFieldsValid())
        {
            _validationFailed = true;
            _UpdateSaveEnabled();

            var missing = string.Join("، ", _MissingRequiredNames());
            MessageBox.Show("يرجى تعبئة الحقول المطلوبة: " + missing, "بيانات ناقصة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            errorProvider.SetError(txtName, "اسم الدور مطلوب");
            return;
        }

        if (!_ValidateForm()) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string? description = string.IsNullOrWhiteSpace(txtDescription.Text)
                ? null
                : txtDescription.Text.Trim();

            long sales = (long)numSalesPermissions.Value;
            long inventory = (long)numInventoryPermissions.Value;
            long accounting = (long)numAccountingPermissions.Value;
            long system = (long)numSystemPermissions.Value;

            Result<RoleDto> result;

            if (_editId is int id)
            {
                var request = new UpdateRoleRequest(
                    name,
                    description,
                    chkIsActive.Checked,
                    sales,
                    inventory,
                    accounting,
                    system);

                result = await RoleService.UpdateAsync(id, request);
            }
            else
            {
                // CreateRoleRequest لا يحتوي على IsActive — الأدوار الجديدة تُنشأ نشطة.
                var request = new CreateRoleRequest(
                    name,
                    description,
                    sales,
                    inventory,
                    accounting,
                    system);

                result = await RoleService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                MessageBox.Show(wasEditing ? "تم تعديل الدور بنجاح." : "تمت إضافة الدور بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
                _isSystemRole = result.Value.IsSystem;

                // بعد الحفظ ينتقل النموذج إلى وضع التعديل (السجل محدد).
                btnSave.Text = "💾 تعديل";
                btnDelete.Enabled = !result.Value.IsSystem;

                await _LoadRolesAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ الدور.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الدور");
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
            MessageBox.Show("يرجى اختيار دور من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_isSystemRole)
        {
            MessageBox.Show("لا يمكن حذف أدوار النظام. يمكنك تعطيلها من خيار (نشط) بدلاً من ذلك.",
                "غير مسموح", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) الدور \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await RoleService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل الدور بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
                await _LoadRolesAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل الدور.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الدور {RoleId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زر الحذف معطّلاً.
            btnDelete.Enabled = _editId.HasValue && !_isSystemRole;
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int roleId)
    {
        foreach (DataGridViewRow row in dgvRoles.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == roleId)
            {
                row.Selected = true;
                dgvRoles.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }

    private static decimal _Clamp(NumericUpDown control, long value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);
}
