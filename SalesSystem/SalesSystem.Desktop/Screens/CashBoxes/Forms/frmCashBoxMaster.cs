using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

/// <summary>
/// شاشة موحّدة (Master Form) لدليل الصناديق.
/// تجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// وتعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmCashBoxMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private ICashBoxApiService CashBoxService => _serviceProvider.GetRequiredService<ICashBoxApiService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, CashBoxDto> _cashBoxesById = new();
    private DataTable _cashBoxesTable = new();
    private DataView _cashBoxesView = new();

    /// <summary>null = إضافة صندوق جديد، &gt; 0 = تعديل صندوق قائم.</summary>
    private int? _editId;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmCashBoxMaster()
    {
        InitializeComponent();
    }

    public frmCashBoxMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnRefresh);

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
                     txtSearch, cbFilterBy, cbStatus, txtName, txtDescription
                 })
        {
            if (input.Focused) return true;
        }

        return false;
    }

    /// <summary>هل الحقل المطلوب (اسم الصندوق) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// يعيد false إذا كان هناك حقل ناقص.
    /// </summary>
    private bool _FocusFirstMissingField(bool edit)
    {
        // لا توجد حقول مطلوبة تختلف بين وضعي الإضافة والتعديل في هذا الدليل.
        _ = edit;

        if (!string.IsNullOrWhiteSpace(txtName.Text)) return true;

        errorProvider.SetError(txtName, "اسم الصندوق مطلوب");
        txtName.Focus();
        return false;
    }

    /// <summary>تحديث يدوي لقائمة الصناديق مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCashBoxesAsync();
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

    /// <summary>زر الخروج: إغلاق شاشة دليل الصناديق.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmCashBoxMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCashBoxesAsync();
            _ResetForm();
            ApplyGreenTheme(); // RULE-N50 — آخر خطوة في مسار التحميل
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل الصناديق");
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
        GreenThemeHelper.ApplyGrid(dgvCashBoxes);

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه)
        dgvCashBoxes.ReadOnly = true;
        dgvCashBoxes.AllowUserToAddRows = false;
        dgvCashBoxes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCashBoxes.MultiSelect = false;

        lblSummary.ForeColor = GreenThemeHelper.TextStrong;   // RULE-N53

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadCashBoxesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("AccountId", typeof(int));
        table.Columns.Add("AccountName", typeof(string));
        table.Columns.Add("BranchName", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await CashBoxService.GetAllAsync();

            _cashBoxesById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var c in result.Value)
                {
                    _cashBoxesById[c.Id] = c;
                    table.Rows.Add(
                        c.Id,
                        c.Name,
                        c.Description ?? "",
                        c.AccountId,
                        c.AccountName ?? "",
                        c.BranchName ?? "",
                        c.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة الصناديق.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة الصناديق");
            MessageBox.Show("تعذر تحميل قائمة الصناديق. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _cashBoxesTable = table;
            _cashBoxesView = new DataView(_cashBoxesTable);
            dgvCashBoxes.DataSource = _cashBoxesView;
            _ApplyGridHeaders();
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا يعتمد DataGridView على <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvCashBoxes.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم الصندوق",
                "Name" => "اسم الصندوق",
                "Description" => "الوصف",
                "AccountName" => "الحساب",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvCashBoxes.Rows)
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

    private void chkOnlyWithAccount_CheckedChanged(object? sender, EventArgs e) => _ApplyFilter();

    /// <summary>Enter يطبّق التصفية فوراً · Esc يمسح كل أدوات التصفية (RULE-N59).</summary>
    private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            _ApplyFilter();
            e.Handled = true;
            e.SuppressKeyPress = true;
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
        chkOnlyWithAccount.Checked = false;

        _ApplyFilter();
    }

    /// <summary>
    /// التصفية الفورية (RULE-N59/N60): كل الحالات تُبنى من أسماء أعمدة معروفة،
    /// والاختيار يُقرأ بالمؤشر لا بالنص العربي، والقيمة تُهرَّب قبل بنائها في RowFilter.
    /// </summary>
    private void _ApplyFilter()
    {
        if (_cashBoxesView is null || _cashBoxesView.Table is null) return;
        if (txtSearch is null || cbFilterBy is null || cbStatus is null
            || chkShowInactive is null || chkOnlyWithAccount is null)
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

        // ── البحث النصي: عمود واحد فقط يحدده cbFilterBy ──
        if (text.Length > 0)
            conditions.Add(RowFilterHelper.Like(_FilterColumn(), text));

        // ── «المربوط بالحسابات فقط» ──
        if (chkOnlyWithAccount.Checked)
            conditions.Add("AccountId > 0");

        // RULE-N60: القيمة مُهرَّبة، وإن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
        try
        {
            _cashBoxesView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تطبيق تصفية دليل الصناديق — أُبقيت التصفية السابقة كما هي");
        }

        _ApplyRowColoring();
        _UpdateSummary();
    }

    /// <summary>عمود البحث المقابل لاختيار «تصفية بـ» — أسماء أعمدة ثابتة داخل الكود (RULE-N60).</summary>
    private string _FilterColumn() => cbFilterBy.SelectedIndex switch
    {
        1 => "AccountName",
        2 => "BranchName",
        3 => "CONVERT(Id, 'System.String')",
        _ => "Name"
    };

    /// <summary>
    /// شريط الملخص (RULE-N61): المعروض من الإجمالي + النشط/المعطّل،
    /// ويُعاد حسابه بعد كل تصفية وكل تحميل.
    /// ملاحظة: `CashBoxDto` لا يحمل رصيداً (الرصيد على الحساب المرتبط فقط)،
    /// لذا لا يُعرض «إجمالي الأرصدة» هنا.
    /// </summary>
    private void _UpdateSummary()
    {
        if (lblSummary is null || _cashBoxesView?.Table is null) return;

        int total = _cashBoxesTable.Rows.Count;
        int shown = _cashBoxesView.Count;

        int active = 0, inactive = 0;

        foreach (DataRowView row in _cashBoxesView)
        {
            if (row["IsActive"] is bool isActive && isActive) active++;
            else inactive++;
        }

        lblSummary.Text =
            $"المعروض: {shown} من {total} — نشط: {active} · معطّل: {inactive}";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvCashBoxes_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvCashBoxes.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار صندوق من الشبكة");
            MessageBox.Show("تعذر عرض بيانات الصندوق المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int cashBoxId)
    {
        if (!_cashBoxesById.TryGetValue(cashBoxId, out var c)) return;

        _editId = cashBoxId;

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;

        errorProvider.Clear();

        txtCode.Text = cashBoxId.ToString();
        txtName.Text = c.Name;
        txtDescription.Text = c.Description ?? "";

        _UpdateModeHeader();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل الصناديق )) — تعديل صندوق"
            : "(( دليل الصناديق )) — إضافة صندوق جديد";

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الإضافة متاح، وزرّي التعديل والحذف معطّلان لعدم وجود سجل محدد.
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtDescription.Clear();

        dgvCashBoxes.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الصندوق مطلوب");
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

    /// <summary>زر الإضافة: يُضيف صندوقاً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث الصندوق المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ الصندوق: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقول المطلوبة الناقصة: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
        if (!_FocusFirstMissingField(edit))
            return;

        if (!_RequiredFieldsValid()) return;
        if (!_ValidateForm()) return;

        // رسالة تأكيد قبل التعديل (مثل رسالة تأكيد الحذف).
        if (edit)
        {
            string editName = txtName.Text.Trim();
            if (MessageBox.Show($"هل أنت متأكد من تعديل الصندوق \"{editName}\"؟",
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

            Result<CashBoxDto> result;

            if (edit)
            {
                var request = new UpdateCashBoxRequest(name, description);
                result = await CashBoxService.UpdateAsync(_editId!.Value, request);
            }
            else
            {
                // AccountId = null → الخدمة تنشئ الحساب المحاسبي المرتبط تلقائياً.
                var request = new CreateCashBoxRequest(name, AccountId: null, Description: description);
                result = await CashBoxService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                MessageBox.Show(edit ? "تم تعديل الصندوق بنجاح." : "تمت إضافة الصندوق بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). نصوص الأزرار ثابتة في المصمم.
                _editId = result.Value.Id;
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                await _LoadCashBoxesAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ الصندوق.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الصندوق");
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
            MessageBox.Show("يرجى اختيار صندوق من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) الصندوق \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await CashBoxService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل الصندوق بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
                await _LoadCashBoxesAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل الصندوق.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل الصندوق {CashBoxId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زرّا التعديل والحذف معطّلين.
            btnSave.Enabled = _editId is null;
            btnEdit.Enabled = _editId.HasValue;
            btnDelete.Enabled = _editId.HasValue;
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int cashBoxId)
    {
        foreach (DataGridViewRow row in dgvCashBoxes.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == cashBoxId)
            {
                row.Selected = true;
                dgvCashBoxes.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }
}
