using System.Data;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Suppliers.Forms;

/// <summary>
/// النموذج المرجعي (Master Form) لدليل الموردين.
/// يجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// ويعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmSupplierMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private ISupplierApiService SupplierService => _serviceProvider.GetRequiredService<ISupplierApiService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, SupplierDto> _suppliersById = new();
    private DataTable _suppliersTable = new();
    private DataView _suppliersView = new();

    /// <summary>null = إضافة مورد جديد، &gt; 0 = تعديل مورد قائم.</summary>
    private int? _editId;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmSupplierMaster()
    {
        InitializeComponent();
    }

    public frmSupplierMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete);

        // الاختصارات الموحّدة (RULE-N47): F2 جديد · F5 تحديث · Delete تعطيل · Esc إلغاء/مسح التصفية.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Delete, _GlobalDelete),
            (Keys.Escape, _GlobalEscape));

        // RULE-N60: أدوات التصفية تُقرأ بالمؤشر — «الحالة: الكل» و«تصفية بـ: الاسم» افتراضياً.
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
                     txtSearch, cbFilterBy, cbStatus, txtName, txtPhone, txtEmail,
                     txtTaxNumber, txtAddress, txtNotes, numCreditLimit
                 })
        {
            if (input.Focused) return true;
        }

        return false;
    }

    // ── Theme ─────────────────────────────────────────────────────────────────

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvSuppliers);
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// يعيد <c>false</c> إذا كان هناك حقل مطلوب ناقص.
    /// </summary>
    private bool _FocusFirstMissingField(bool edit)
    {
        _ = edit; // الحقول المطلوبة متطابقة في وضعي الإضافة والتعديل.

        errorProvider.SetError(txtName, "");

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المورد مطلوب");
            txtName.Focus();
            return false;
        }

        return true;
    }

    /// <summary>تحديث يدوي لقائمة الموردين مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadSuppliersAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث البيانات");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmSupplierMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadSuppliersAsync();
            _ResetForm();
            ApplyGreenTheme();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الموردين");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadSuppliersAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("TaxNumber", typeof(string));
        table.Columns.Add("CreditLimit", typeof(decimal));
        table.Columns.Add("CurrentBalance", typeof(decimal));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await SupplierService.GetAllAsync(includeInactive: true);

            _suppliersById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var s in result.Value)
                {
                    _suppliersById[s.Id] = s;
                    table.Rows.Add(
                        s.Id,
                        s.Name,
                        s.Phone ?? "",
                        s.Email ?? "",
                        s.TaxNumber ?? "",
                        s.CreditLimit,
                        s.CurrentBalance,
                        s.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الموردين.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الموردين");
            MessageBox.Show("تعذر تحميل قائمة الموردين. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _suppliersTable = table;
            _suppliersView = new DataView(_suppliersTable);
            dgvSuppliers.DataSource = _suppliersView;
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
        foreach (DataGridViewColumn column in dgvSuppliers.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم المورد",
                "Name" => "اسم المورد",
                "Phone" => "الهاتف",
                "Email" => "البريد الإلكتروني",
                "TaxNumber" => "الرقم الضريبي",
                "CreditLimit" => "حد الائتمان",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
        }

        // عمود الرصيد الحالي: يُستخدم في التصفية والملخص فقط ولا يُعرض في الشبكة (RULE-N60).
        if (dgvSuppliers.Columns["CurrentBalance"] is { } balanceColumn)
        {
            balanceColumn.HeaderText = "الرصيد الحالي";
            balanceColumn.Visible = false;
        }
    }

    private void _ApplyRowColoring()
    {
        if (!dgvSuppliers.Columns.Contains("IsActive")) return;

        foreach (DataGridViewRow row in dgvSuppliers.Rows)
        {
            if (row.Cells["IsActive"].Value is bool isActive && isActive) continue;

            row.DefaultCellStyle.BackColor = Color.MistyRose;
            row.DefaultCellStyle.ForeColor = Color.DarkRed;
        }
    }

    // ── Search / Filter (DataView.RowFilter) ──────────────────────────────────

    private void txtSearch_TextChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbFilterBy_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyWithBalance_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    private void chkOnlyOverLimit_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

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
        chkOnlyWithBalance.Checked = false;
        chkOnlyOverLimit.Checked = false;

        _ApplyFilter();
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59/N60): كل الحالات تُبنى من أسماء أعمدة معروفة،
    /// والاختيار يُقرأ بالمؤشر لا بالنص العربي، والقيمة تُهرَّب قبل بنائها في RowFilter.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_suppliersView is null || _suppliersView.Table is null) return;
        if (txtSearch is null || cbFilterBy is null || cbStatus is null
            || chkShowInactive is null || chkOnlyWithBalance is null || chkOnlyOverLimit is null)
            return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

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
            conditions.Add($"{_FilterColumn()} LIKE '%{text}%'");

        // ── مربعات الاختيار المالية ──
        if (chkOnlyWithBalance.Checked)
            conditions.Add("CurrentBalance <> 0");

        if (chkOnlyOverLimit.Checked)
            conditions.Add("CurrentBalance > CreditLimit");

        _suppliersView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";

        _ApplyRowColoring();
        _UpdateSummary();
    }

    /// <summary>عمود البحث المقابل لاختيار «تصفية بـ» — أسماء أعمدة ثابتة داخل الكود (RULE-N60).</summary>
    private string _FilterColumn() => cbFilterBy.SelectedIndex switch
    {
        1 => "Phone",
        2 => "Email",
        3 => "TaxNumber",
        _ => "Name"
    };

    /// <summary>
    /// شريط الملخص (RULE-N61): المعروض من الإجمالي + النشط/المعطّل + إجمالي المستحقات،
    /// ويُعاد حسابه بعد كل تصفية وكل تحميل.
    /// </summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null || _suppliersView?.Table is null) return;

        int total = _suppliersTable.Rows.Count;
        int shown = _suppliersView.Count;

        int active = 0, inactive = 0;
        decimal due = 0m;

        foreach (DataRowView row in _suppliersView)
        {
            if (row["IsActive"] is bool isActive && isActive) active++;
            else inactive++;

            decimal current = _AsDecimal(row["CurrentBalance"]);
            if (current != 0m) due += current;
        }

        lblSummary.Text =
            $"المعروض: {shown} من {total} — نشط: {active} · معطّل: {inactive} · " +
            $"المستحقات: {due:N2}";
    }

    private static decimal _AsDecimal(object? value) => value is decimal d ? d : 0m;

    // ── Grid Selection ────────────────────────────────────────────────────────

    private async void dgvSuppliers_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvSuppliers.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            await _FillFormAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مورد من الشبكة");
            MessageBox.Show("تعذر عرض بيانات المورد المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task _FillFormAsync(int supplierId)
    {
        if (!_suppliersById.TryGetValue(supplierId, out var s)) return;

        // تحديث البيانات من الخدمة، مع الاعتماد على الذاكرة المحلية عند التعذر
        try
        {
            var fresh = await SupplierService.GetByIdAsync(supplierId);
            if (fresh.IsSuccess && fresh.Value != null)
                s = fresh.Value;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر تحديث بيانات المورد {SupplierId} من الخدمة", supplierId);
        }

        _editId = s.Id;

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;

        txtId.Text = s.Id.ToString();
        txtName.Text = s.Name;
        txtPhone.Text = s.Phone ?? "";
        txtEmail.Text = s.Email ?? "";
        txtAddress.Text = s.Address ?? "";
        txtTaxNumber.Text = s.TaxNumber ?? "";
        numCreditLimit.Value = _Clamp(numCreditLimit, s.CreditLimit);
        txtNotes.Text = s.Notes ?? "";

        _UpdateModeHeader();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل الموردين )) — تعديل مورد"
            : "(( دليل الموردين )) — إضافة مورد جديد";

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الإضافة متاح، وزرّي التعديل والحذف معطّلان لعدم وجود سجل محدد.
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;

        errorProvider.Clear();

        txtId.Clear();
        txtName.Clear();
        txtPhone.Clear();
        txtEmail.Clear();
        txtAddress.Clear();
        txtTaxNumber.Clear();
        txtNotes.Clear();
        numCreditLimit.Value = 0;

        dgvSuppliers.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المورد مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (numCreditLimit.Value < 0)
        {
            errorProvider.SetError(numCreditLimit, "حد الائتمان يجب أن يكون قيمة موجبة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numCreditLimit, "");
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

    /// <summary>زر الخروج: إغلاق شاشة دليل الموردين.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>زر الإضافة: يُضيف مورداً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المورد المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ المورد: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
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
            if (MessageBox.Show($"هل أنت متأكد من تعديل المورد \"{editName}\"؟",
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
            string? phone = _ToNullIfEmpty(txtPhone.Text);
            string? email = _ToNullIfEmpty(txtEmail.Text);
            string? address = _ToNullIfEmpty(txtAddress.Text);
            string? taxNumber = _ToNullIfEmpty(txtTaxNumber.Text);
            string? notes = _ToNullIfEmpty(txtNotes.Text);
            decimal creditLimit = numCreditLimit.Value;

            Result<SupplierDto> result;

            if (edit)
            {
                int id = _editId!.Value;

                // الحفاظ على حالة التنشيط الحالية للمورد أثناء التعديل
                bool isActive = _suppliersById.TryGetValue(id, out var current) && current.IsActive;

                var request = new UpdateSupplierRequest(
                    name,
                    phone,
                    email,
                    address,
                    taxNumber,
                    notes,
                    creditLimit,
                    CategoryId: null,
                    IsActive: isActive);

                result = await SupplierService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateSupplierRequest(
                    name,
                    phone,
                    email,
                    address,
                    taxNumber,
                    notes,
                    creditLimit);

                result = await SupplierService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new SupplierChangedMessage(result.Value.Id));

                MessageBox.Show(edit ? "تم تعديل المورد بنجاح." : "تمت إضافة المورد بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). النصوص ثابتة في المصمم.
                _editId = result.Value.Id;
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                await _LoadSuppliersAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ المورد.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ المورد");
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
            MessageBox.Show("يرجى اختيار مورد من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) المورد \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await SupplierService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل المورد بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new SupplierChangedMessage(id));
                _ResetForm();
                await _LoadSuppliersAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل المورد.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل المورد {SupplierId}", id);
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

    private void _SelectGridRow(int supplierId)
    {
        foreach (DataGridViewRow row in dgvSuppliers.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == supplierId)
            {
                row.Selected = true;
                dgvSuppliers.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }

    private static string? _ToNullIfEmpty(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal _Clamp(NumericUpDown control, decimal value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);
}
