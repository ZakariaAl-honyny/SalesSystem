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

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete);

        // RULE-N47 — اختصارات موحّدة: F2 جديد · F5 تحديث · Esc مسح كل التصفية (RULE-N59)
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, btnNew.PerformClick),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _ClearFilters));

        // RULE-N47 — اختصار كل زر في تلميحه
        tafqeetTip.SetToolTip(btnNew, "جديد (F2) — Esc يمسح التصفية");
        tafqeetTip.SetToolTip(btnSave, "إضافة موظف جديد");
        tafqeetTip.SetToolTip(btnEdit, "تعديل الموظف المحدد");
        tafqeetTip.SetToolTip(btnDelete, "حذف (تعطيل) الموظف المحدد");
        tafqeetTip.SetToolTip(btnRefresh, "تحديث البيانات (F5)");
        tafqeetTip.SetToolTip(btnClose, "خروج من الشاشة");

        // RULE-N97 — التفقيط الحيّ على الحقل المالي
        LiveTafqeetHelper.Attach(tafqeetTip,
            (numBasicSalary, "الراتب الأساسي", LiveTafqeetHelper.ReadMode.Money));
        FilterPanelHelper.Attach(this, _serviceProvider);
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

    /// <summary>تحديث يدوي لقائمة الموظفين مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadEmployeesAsync();
            _ResetForm(); // تفريغ الحقول + وضع الاستعداد بعد التحديث
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
            ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
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

    /// <summary>RULE-N59 — Esc يمسح كل أدوات التصفية ويعيد القائمة كاملة.</summary>
    private void _ClearFilters()
    {
        if (cbFilterBy.SelectedIndex != 0) cbFilterBy.SelectedIndex = 0;
        if (cbStatus.SelectedIndex != 0) cbStatus.SelectedIndex = 0;
        if (chkShowInactive.Checked) chkShowInactive.Checked = false;
        if (chkHasSalary.Checked) chkHasSalary.Checked = false;
        if (txtFilterValue.Text.Length > 0) txtFilterValue.Clear();

        _ApplyFilter(); // ضمان التطبيق حتى لو لم يتغيّر شيء
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

            conditions.Add($"{column} LIKE '%{_EscapeFilterValue(text)}%'");
        }

        _employeesView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : string.Empty;

        _ApplyRowColoring();
        _UpdateSummary();
    }

    /// <summary>تهريب قيمة البحث قبل حقنها في <see cref="DataView.RowFilter"/> (RULE-N60).</summary>
    private static string _EscapeFilterValue(string value)
        => value.Replace("'", "''").Replace("[", "[[]").Replace("*", "[*]").Replace("%", "[%]");

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

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
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

        // وضع الإضافة: زر الإضافة متاح، وزرّي التعديل والحذف معطّلان لعدم وجود سجل محدد.
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
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
            if (MessageBox.Show($"هل أنت متأكد من تعديل الموظف \"{editName}\"؟",
                    "تأكيد التعديل", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
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
                MessageBox.Show(edit ? "تم تعديل الموظف بنجاح." : "تمت إضافة الموظف بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
                _selected = result.Value;

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). النصوص ثابتة في المصمم.
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
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
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زرّا التعديل والحذف معطّلين.
            btnDelete.Enabled = _editId.HasValue;
            btnEdit.Enabled = _editId.HasValue;
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
