using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
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

    private IEmployeeApiService EmployeeService => _serviceProvider.GetRequiredService<IEmployeeApiService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, EmployeeDto> _employeesById = new();
    private DataTable _employeesTable = new();
    private DataView _employeesView = new();

    /// <summary>السجل الأصلي للموظف المحدد — تُحفظ منه الحقول غير القابلة للتحرير في هذه الشاشة.</summary>
    private EmployeeDto? _selected;

    /// <summary>null = إضافة موظف جديد، &gt; 0 = تعديل موظف قائم.</summary>
    private int? _editId;

    /// <summary>يُفعَّل عند محاولة حفظ ناقصة — يُعطّل زر الحفظ حتى تكتمل الحقول المطلوبة.</summary>
    private bool _validationFailed;

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
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmEmployeeMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvEmployees.CellClick += dgvEmployees_CellClick;

        // التحقق الحيّ: يُفعَّل زر الحفظ افتراضياً، ويُعطَّل فقط بعد محاولة حفظ ناقصة
        // ثم يُعاد تفعيله تلقائياً عند إدخال اسم الموظف.
        txtName.TextChanged += (s, e) => _UpdateSaveEnabled();
    }

    /// <summary>هل الحقل المطلوب (اسم الموظف) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>أسماء الحقول المطلوبة الناقصة (اسم الموظف).</summary>
    private List<string> _MissingRequiredNames()
    {
        var missing = new List<string>();

        if (string.IsNullOrWhiteSpace(txtName.Text))
            missing.Add("اسم الموظف");

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

    /// <summary>تحديث يدوي لقائمة الموظفين مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadEmployeesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث قائمة الموظفين");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الموظفين");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadEmployeesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("JobTitle", typeof(string));
        table.Columns.Add("Phone", typeof(string));
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
                        e.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الموظفين.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الموظفين");
            MessageBox.Show("تعذر تحميل قائمة الموظفين. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _employeesTable = table;
            _employeesView = new DataView(_employeesTable);
            dgvEmployees.DataSource = _employeesView;
            _ApplyFilter();
            _ApplyRowColoring();
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

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void _ApplyFilter()
    {
        if (_employeesView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _employeesView.RowFilter = "";
            return;
        }

        _employeesView.RowFilter =
            $"Name LIKE '%{text}%' OR JobTitle LIKE '%{text}%' OR Phone LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
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
            MessageBox.Show("تعذر عرض بيانات الموظف المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int employeeId)
    {
        if (!_employeesById.TryGetValue(employeeId, out var e)) return;

        _editId = employeeId;
        _selected = e;

        // وضع التعديل: زر الحفظ يُحدّث السجل المحدد، وزر الحذف متاح.
        btnSave.Text = "💾 تعديل";
        btnDelete.Enabled = true;

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

        _UpdateModeHeader();

        _validationFailed = false;
        _UpdateSaveEnabled();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل الموظفين )) — تعديل موظف"
            : "(( دليل الموظفين )) — إضافة موظف جديد";

    private void _ResetForm()
    {
        _editId = null;
        _selected = null;

        // وضع الإضافة: زر الحفظ يُضيف سجلاً جديداً، وزر الحذف معطّل لعدم وجود سجل محدد.
        btnSave.Text = "💾 إضافة";
        btnDelete.Enabled = false;

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
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.",
                "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── Buttons ───────────────────────────────────────────────────────────────

    private void btnNew_Click(object? sender, EventArgs e) => _ResetForm();

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

            errorProvider.SetError(txtName, "اسم الموظف مطلوب");
            return;
        }

        if (!_ValidateForm()) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            var request = _BuildEmployeeDto();

            Result<EmployeeDto> result;

            if (_editId is int id)
                result = await EmployeeService.UpdateAsync(id, request);
            else
                result = await EmployeeService.CreateAsync(request);

            if (result.IsSuccess && result.Value != null)
            {
                MessageBox.Show(wasEditing ? "تم تعديل الموظف بنجاح." : "تمت إضافة الموظف بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
                _selected = result.Value;

                // بعد الحفظ ينتقل النموذج إلى وضع التعديل (السجل محدد).
                btnSave.Text = "💾 تعديل";
                btnDelete.Enabled = true;

                await _LoadEmployeesAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ الموظف.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الموظف");
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            _UpdateSaveEnabled();
            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار موظف من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) الموظف \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await EmployeeService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل الموظف بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
                await _LoadEmployeesAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل الموظف.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الموظف {EmployeeId}", id);
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
}
