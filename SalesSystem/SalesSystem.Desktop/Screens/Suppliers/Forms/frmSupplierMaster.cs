using System.Data;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
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

        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmSupplierMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnEdit.Click += btnEdit_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvSuppliers.CellClick += dgvSuppliers_CellClick;
    }

    /// <summary>هل الحقل المطلوب (اسم المورد) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    private void _FocusFirstMissingField()
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المورد مطلوب");
            txtName.Focus();
        }
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
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvSuppliers.Rows)
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
        if (_suppliersView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _suppliersView.RowFilter = "";
            return;
        }

        _suppliersView.RowFilter =
            $"Name LIKE '%{text}%' OR Phone LIKE '%{text}%' OR Email LIKE '%{text}%' " +
            $"OR TaxNumber LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

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
        if (!_RequiredFieldsValid())
        {
            _FocusFirstMissingField();
            return;
        }

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
