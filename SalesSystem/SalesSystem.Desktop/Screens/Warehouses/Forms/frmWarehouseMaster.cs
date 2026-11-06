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

namespace SalesSystem.Desktop.Screens.Warehouses.Forms;

/// <summary>
/// شاشة موحّدة (Master Form) لدليل المخازن.
/// تجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// وتعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmWarehouseMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IWarehouseApiService WarehouseService => _serviceProvider.GetRequiredService<IWarehouseApiService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, WarehouseDto> _warehousesById = new();
    private DataTable _warehousesTable = new();
    private DataView _warehousesView = new();

    /// <summary>null = إضافة مخزن جديد، &gt; 0 = تعديل مخزن قائم.</summary>
    private int? _editId;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmWarehouseMaster()
    {
        InitializeComponent();
    }

    public frmWarehouseMaster(IServiceProvider serviceProvider) : this()
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
        Load += frmWarehouseMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnEdit.Click += btnEdit_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;
        btnSetDefault.Click += btnSetDefault_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvWarehouses.CellClick += dgvWarehouses_CellClick;
    }

    /// <summary>هل الحقل المطلوب (اسم المخزن) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>
    /// ينقل المؤشر إلى أول حقل مطلوب فارغ ويعرض أيقونة الخطأ عليه (دون تعطيل أي زر).
    /// </summary>
    private void _FocusFirstMissingField()
    {
        if (!string.IsNullOrWhiteSpace(txtName.Text)) return;

        errorProvider.SetError(txtName, "اسم المخزن مطلوب");
        txtName.Focus();
    }

    /// <summary>تحديث يدوي لقائمة المخازن مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadWarehousesAsync();
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

    private async void frmWarehouseMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadWarehousesAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل المخازن");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadWarehousesAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("Address", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await WarehouseService.GetAllAsync(includeInactive: true);

            _warehousesById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var w in result.Value)
                {
                    _warehousesById[w.Id] = w;
                    table.Rows.Add(
                        (int)w.Id,
                        w.Name,
                        w.Phone ?? "",
                        w.Address ?? "",
                        w.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة المخازن.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة المخازن");
            MessageBox.Show("تعذر تحميل قائمة المخازن. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _warehousesTable = table;
            _warehousesView = new DataView(_warehousesTable);
            dgvWarehouses.DataSource = _warehousesView;
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvWarehouses.Rows)
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
        if (_warehousesView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _warehousesView.RowFilter = "";
            return;
        }

        _warehousesView.RowFilter =
            $"Name LIKE '%{text}%' OR Phone LIKE '%{text}%' OR Address LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvWarehouses_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvWarehouses.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار مخزن من الشبكة");
            MessageBox.Show("تعذر عرض بيانات المخزن المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int warehouseId)
    {
        if (!_warehousesById.TryGetValue(warehouseId, out var w)) return;

        _editId = warehouseId;

        // وضع التعديل: زر التعديل يُحدّث السجل المحدد، وزر الإضافة معطّل (السجل محدد)، وزر الحذف متاح.
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;

        errorProvider.Clear();

        txtCode.Text = warehouseId.ToString();
        txtName.Text = w.Name;
        txtPhone.Text = w.Phone ?? "";
        txtAddress.Text = w.Address ?? "";
        txtNotes.Text = w.Notes ?? "";

        _UpdateModeHeader();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل المخازن )) — تعديل مخزن"
            : "(( دليل المخازن )) — إضافة مخزن جديد";

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
        txtPhone.Clear();
        txtAddress.Clear();
        txtNotes.Clear();

        dgvWarehouses.ClearSelection();
        _UpdateModeHeader();
        txtName.Focus();
    }

    // ── Validation ────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم المخزن مطلوب");
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

    /// <summary>زر الإضافة: يُضيف مخزناً جديداً دائماً (لا يعتمد على الاختيار في الشبكة).</summary>
    private void btnSave_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: false);

    /// <summary>زر التعديل: يُحدّث المخزن المحدد فقط.</summary>
    private void btnEdit_Click(object? sender, EventArgs e) => _ = _SaveAsync(edit: true);

    /// <summary>
    /// حفظ المخزن: <paramref name="edit"/> = false → إضافة جديدة، true → تعديل السجل المحدد (<c>_editId</c>).
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
            if (MessageBox.Show($"هل أنت متأكد من تعديل المخزن \"{editName}\"؟",
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
            string? phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            string? address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
            string? notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            Result<WarehouseDto> result;

            if (edit)
            {
                var request = new UpdateWarehouseRequest(name, phone, address, notes, IsActive: true);
                result = await WarehouseService.UpdateAsync(_editId!.Value, request);
            }
            else
            {
                var request = new CreateWarehouseRequest(name, phone, address, notes);
                result = await WarehouseService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new WarehouseChangedMessage(result.Value.Id));

                MessageBox.Show(edit ? "تم تعديل المخزن بنجاح." : "تمت إضافة المخزن بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // بعد الحفظ الناجح يصبح السجل محدداً → وضع التعديل
                // (زر الإضافة معطّل، وزرّا التعديل والحذف متاحان). نصوص الأزرار ثابتة في المصمم.
                _editId = result.Value.Id;
                btnSave.Enabled = false;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;

                await _LoadWarehousesAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ المخزن.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ المخزن");
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
            MessageBox.Show("يرجى اختيار مخزن من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) المخزن \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            // لا توجد عملية حذف مخصصة في IWarehouseApiService — التعطيل يتم عبر Update مع IsActive = false.
            var current = await WarehouseService.GetByIdAsync(id);
            if (!current.IsSuccess || current.Value is null)
            {
                MessageBox.Show($"لم يتم تعطيل المخزن.\n{current.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var w = current.Value;
            var request = new UpdateWarehouseRequest(w.Name, w.Phone, w.Address, w.Notes, IsActive: false);

            var result = await WarehouseService.UpdateAsync(id, request);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل المخزن بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new WarehouseChangedMessage(id));
                _ResetForm();
                await _LoadWarehousesAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل المخزن.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل المخزن {WarehouseId}", id);
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

    private async void btnSetDefault_Click(object? sender, EventArgs e)
    {
        if (_editId is not int id)
        {
            MessageBox.Show("يرجى اختيار مخزن من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل تريد تعيين المخزن \"{name}\" كمخزن افتراضي؟",
                "تعيين مخزن افتراضي", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnSetDefault.Enabled = false;

        try
        {
            var result = await WarehouseService.SetDefaultAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعيين المخزن الافتراضي بنجاح.", "تم التنفيذ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                EventBus.Publish(new WarehouseChangedMessage(id));
            }
            else
            {
                MessageBox.Show($"لم يتم تعيين المخزن الافتراضي.\n{result.Error}",
                    "فشل التنفيذ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعيين المخزن الافتراضي {WarehouseId}", id);
            MessageBox.Show("حدث خطأ أثناء تعيين المخزن الافتراضي. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSetDefault.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void _SelectGridRow(int warehouseId)
    {
        foreach (DataGridViewRow row in dgvWarehouses.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == warehouseId)
            {
                row.Selected = true;
                dgvWarehouses.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }
}
