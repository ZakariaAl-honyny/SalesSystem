using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Customers.Forms;

/// <summary>
/// النموذج المرجعي (Master Form) لدليل العملاء.
/// يجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// ويعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmCustomerMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private ICustomerApiService CustomerService => _serviceProvider.GetRequiredService<ICustomerApiService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, CustomerDto> _customersById = new();
    private DataTable _customersTable = new();
    private DataView _customersView = new();

    /// <summary>null = إضافة عميل جديد، &gt; 0 = تعديل عميل قائم.</summary>
    private int? _editId;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmCustomerMaster()
    {
        InitializeComponent();
    }

    public frmCustomerMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // الأزرق الفاتح للحقل النشط
        KeyboardHelper.WireArrowNavigation(this); // أسهم RTL: يمين=رجوع، يسار=تالي

        // أزرار الوضع (إضافة/تعديل/حذف) تتغير حالة تفعيلها حسب السجل المحدد،
        // لذا نحتاج مظهراً رمادياً واضحاً عند التعطيل.
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnEdit, btnDelete, btnReactivate);
    }

    // ── Theme ─────────────────────────────────────────────────────────────────

    /// <summary>الثيم الأخضر (RULE-N50…RULE-N53) — يُستدعى أخيراً بعد ThemeHelper ليردّ الألوان.</summary>
    private void ApplyGreenTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvCustomers);

        // RULE-N22/RULE-N52: سلوك شبكة العرض تعلنه الشاشة نفسها — ApplyGrid لا يلمسه.
        dgvCustomers.ReadOnly = true;
        dgvCustomers.AllowUserToAddRows = false;
        dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvCustomers.MultiSelect = false;

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    // ── Validation ────────────────────────────────────────────────────────────

    /// <summary>هل الحقل المطلوب (اسم العميل) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    /// <param name="edit">false = إضافة عميل جديد، true = تعديل عميل قائم.</param>
    /// <returns>false إذا كان هناك حقل مطلوب ناقص، و true إذا كانت الحقول مكتملة.</returns>
    private bool _FocusFirstMissingField(bool edit)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم العميل مطلوب");
            txtName.Focus();
            return false;
        }

        return true;
    }

    /// <summary>تحديث يدوي لقائمة العملاء مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCustomersAsync();
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

    private async void frmCustomerMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadCustomersAsync();
            _ResetForm();
            ApplyGreenTheme();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل العملاء");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadCustomersAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("TaxNumber", typeof(string));
        table.Columns.Add("CreditLimit", typeof(decimal));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await CustomerService.GetAllAsync(includeInactive: true);

            _customersById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var c in result.Value)
                {
                    _customersById[c.Id] = c;
                    table.Rows.Add(
                        c.Id,
                        c.Name,
                        c.Phone ?? "",
                        c.Email ?? "",
                        c.TaxNumber ?? "",
                        c.CreditLimit,
                        c.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة العملاء.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة العملاء");
            MessageBox.Show("تعذر تحميل قائمة العملاء. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _customersTable = table;
            _customersView = new DataView(_customersTable);
            dgvCustomers.DataSource = _customersView;
            _ApplyGridHeaders();
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    /// <summary>
    /// عناوين أعمدة الشبكة بالعربية.
    /// لا تُعرض <c>DataColumn.Caption</c> عند الربط بـ <see cref="DataView"/>،
    /// لذا تُضبط <c>HeaderText</c> صراحةً لكل عمود ظاهر بعد إسناد <c>DataSource</c>.
    /// </summary>
    private void _ApplyGridHeaders()
    {
        foreach (DataGridViewColumn column in dgvCustomers.Columns)
        {
            column.HeaderText = column.DataPropertyName switch
            {
                "Id" => "رقم العميل",
                "Name" => "اسم العميل",
                "Phone" => "الهاتف",
                "Email" => "البريد",
                "TaxNumber" => "الرقم الضريبي",
                "CreditLimit" => "حد الائتمان",
                "IsActive" => "نشط",
                _ => column.HeaderText
            };
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvCustomers.Rows)
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

    private void chkShowInactive_CheckedChanged(object? sender, EventArgs e)
    {
        _ApplyFilter();
        _ApplyRowColoring();
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

    private void _ApplyFilter()
    {
        if (_customersView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        var conditions = new List<string>();

        // إخفاء غير النشط ما لم يُفَعَّل خيار "عرض غير النشط"
        if (!chkShowInactive.Checked)
            conditions.Add("IsActive = true");

        if (!string.IsNullOrEmpty(text))
        {
            conditions.Add(
                $"(Name LIKE '%{text}%' OR Phone LIKE '%{text}%' OR Email LIKE '%{text}%' " +
                $"OR TaxNumber LIKE '%{text}%' " +
                $"OR CONVERT(Id, 'System.String') LIKE '{text}%')");
        }

        _customersView.RowFilter = conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private async void dgvCustomers_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvCustomers.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            await _FillFormAsync(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار عميل من الشبكة");
            MessageBox.Show("تعذر عرض بيانات العميل المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task _FillFormAsync(int customerId)
    {
        if (!_customersById.TryGetValue(customerId, out var c)) return;

        // تحديث البيانات من الخدمة، مع الاعتماد على الذاكرة المحلية عند التعذر
        try
        {
            var fresh = await CustomerService.GetByIdAsync(customerId);
            if (fresh.IsSuccess && fresh.Value != null)
                c = fresh.Value;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر تحديث بيانات العميل {CustomerId} من الخدمة", customerId);
        }

        _editId = c.Id;

        // ضمان أن حالة التنشيط المحسوبة للأزرار مأخوذة من أحدث نسخة من السجل.
        _customersById[c.Id] = c;

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)،
        // وأزرار التعطيل/إعادة التنشيط تُضبط حسب حالة التنشيط (RULE-N92).
        btnSave.Enabled = false;
        _UpdateActionButtons();

        txtId.Text = c.Id.ToString();
        txtName.Text = c.Name;
        txtPhone.Text = c.Phone ?? "";
        txtEmail.Text = c.Email ?? "";
        txtAddress.Text = c.Address ?? "";
        txtTaxNumber.Text = c.TaxNumber ?? "";
        numCreditLimit.Value = _Clamp(numCreditLimit, c.CreditLimit);
        txtNotes.Text = c.Notes ?? "";

        _UpdateModeHeader();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل العملاء )) — تعديل عميل"
            : "(( دليل العملاء )) — إضافة عميل جديد";

    /// <summary>هل يوجد إجراء غير متزامن قيد التنفيذ؟ (يمنع تكرار النقر ويُعطّل أزرار الإجراءات مؤقتاً).</summary>
    private bool _actionButtonsBusy;

    /// <summary>
    /// آلة حالة أزرار الإجراءات (RULE-N43 + RULE-N92):
    /// التعديل يحتاج سجلاً محدداً، والتعطيل للنشط فقط، وإعادة التنشيط لغير النشط فقط.
    /// </summary>
    private void _UpdateActionButtons()
    {
        bool hasSelection = _editId.HasValue;
        bool isActive = hasSelection && _customersById.TryGetValue(_editId!.Value, out var c) && c.IsActive;

        btnEdit.Enabled = hasSelection && !_actionButtonsBusy;
        btnDelete.Enabled = hasSelection && isActive && !_actionButtonsBusy;        // التعطيل للنشط فقط
        btnReactivate.Enabled = hasSelection && !isActive && !_actionButtonsBusy;   // RULE-N92
    }

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الإضافة متاح، وبقية أزرار الإجراءات معطّلة لعدم وجود سجل محدد.
        btnSave.Enabled = true;
        _UpdateActionButtons();

        errorProvider.Clear();

        txtId.Clear();
        txtName.Clear();
        txtPhone.Clear();
        txtEmail.Clear();
        txtAddress.Clear();
        txtTaxNumber.Clear();
        txtNotes.Clear();
        numCreditLimit.Value = 0;

        dgvCustomers.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();
    }

    // ── Validation (الرسائل) ─────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم العميل مطلوب");
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

    /// <summary>زر الخروج: إغلاق شاشة دليل العملاء.</summary>
    private void btnClose_Click(object? sender, EventArgs e) => Close();

    /// <summary>زر الإضافة: يُضيف عميلاً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث العميل المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ العميل: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
    /// </summary>
    private async Task _SaveAsync(bool edit)
    {
        // التعديل بدون سجل محدد لا معنى له.
        if (edit && _editId is null) return;

        // الحقول المطلوبة الناقصة: نقل المؤشر إلى أول حقل ناقص مع أيقونة خطأ (الأزرار تبقى مفعّلة).
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
            if (MessageBox.Show($"هل أنت متأكد من تعديل العميل \"{editName}\"؟",
                    "تأكيد التعديل", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;
        _actionButtonsBusy = true;
        _UpdateActionButtons();

        try
        {
            string name = txtName.Text.Trim();
            string? phone = _ToNullIfEmpty(txtPhone.Text);
            string? email = _ToNullIfEmpty(txtEmail.Text);
            string? address = _ToNullIfEmpty(txtAddress.Text);
            string? taxNumber = _ToNullIfEmpty(txtTaxNumber.Text);
            string? notes = _ToNullIfEmpty(txtNotes.Text);
            decimal creditLimit = numCreditLimit.Value;

            Result<CustomerDto> result;

            if (edit)
            {
                int id = _editId!.Value;

                // الحفاظ على حالة التنشيط الحالية للعميل أثناء التعديل
                bool isActive = _customersById.TryGetValue(id, out var current) && current.IsActive;

                var request = new UpdateCustomerRequest(
                    name,
                    phone,
                    email,
                    address,
                    taxNumber,
                    notes,
                    creditLimit,
                    isActive);

                result = await CustomerService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateCustomerRequest(
                    name,
                    phone,
                    email,
                    address,
                    taxNumber,
                    notes,
                    creditLimit);

                result = await CustomerService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new CustomerChangedMessage(result.Value.Id));

                MessageBox.Show(edit ? "تم تعديل العميل بنجاح." : "تمت إضافة العميل بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). النصوص ثابتة في المصمم.
                _editId = result.Value.Id;
                btnSave.Enabled = false;
                _UpdateActionButtons();

                await _LoadCustomersAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ العميل.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ العميل");
            MessageBox.Show("حدث خطأ أثناء الحفظ. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // استعادة حالة الأزرار حسب الوضع الحالي للنموذج.
            _actionButtonsBusy = false;
            btnSave.Enabled = _editId is null;
            _UpdateActionButtons();

            Cursor = Cursors.Default;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار عميل من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) العميل \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _UpdateActionButtons();

        try
        {
            var result = await CustomerService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل العميل بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new CustomerChangedMessage(id));
                _ResetForm();
                await _LoadCustomersAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل العميل.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل العميل {CustomerId}", id);
            MessageBox.Show("حدث خطأ أثناء الحذف. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // بعد الحذف الناجح يعود النموذج إلى وضع الإضافة → يبقى زرّا التعديل والحذف معطّلين.
            _actionButtonsBusy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// إعادة تنشيط العميل المعطَّل (RULE-N90…RULE-N94):
    /// الزر مفعّل فقط على سطر غير نشط، والتنفيذ داخل الخدمة (BLL) داخل معاملة.
    /// </summary>
    private async void btnReactivate_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار عميل معطّل من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل تريد إعادة تنشيط العميل \"{name}\"؟",
                "تأكيد إعادة التنشيط", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        _actionButtonsBusy = true;
        _UpdateActionButtons();

        try
        {
            var result = await CustomerService.ReactivateAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم إعادة تنشيط العميل بنجاح.", "تم إعادة التنشيط",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new CustomerChangedMessage(id));

                await _LoadCustomersAsync();

                // RULE-N94: يبقى السطر المُعاد تنشيطه محدداً في الشبكة (يعود لونه أبيض ويُفعَّل زر التعطيل)
                // مع الإبقاء على بيانات العميل في الحقول، ليرى المستخدم نتيجة العملية فوراً.
                _editId = id;
                _SelectGridRow(id);
                _UpdateModeHeader();
                _UpdateActionButtons();
            }
            else
            {
                MessageBox.Show($"تعذر إعادة تنشيط العميل.\n{result.Error}",
                    "فشل إعادة التنشيط", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء إعادة تنشيط العميل {CustomerId}", id);
            MessageBox.Show("حدث خطأ أثناء إعادة التنشيط. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _actionButtonsBusy = false;
            _UpdateActionButtons();
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int customerId)
    {
        foreach (DataGridViewRow row in dgvCustomers.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == customerId)
            {
                row.Selected = true;
                dgvCustomers.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }

    private static string? _ToNullIfEmpty(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static decimal _Clamp(NumericUpDown control, decimal value)
        => Math.Min(Math.Max(value, control.Minimum), control.Maximum);
}
