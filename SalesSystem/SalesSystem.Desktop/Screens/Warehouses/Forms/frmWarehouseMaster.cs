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

    public frmWarehouseMaster(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmWarehouseMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnSetDefault.Click += btnSetDefault_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvWarehouses.CellClick += dgvWarehouses_CellClick;
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

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string? phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            string? address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
            string? notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            Result<WarehouseDto> result;

            if (_editId is int id)
            {
                var request = new UpdateWarehouseRequest(name, phone, address, notes, IsActive: true);
                result = await WarehouseService.UpdateAsync(id, request);
            }
            else
            {
                var request = new CreateWarehouseRequest(name, phone, address, notes);
                result = await WarehouseService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                EventBus.Publish(new WarehouseChangedMessage(result.Value.Id));

                MessageBox.Show(wasEditing ? "تم تعديل المخزن بنجاح." : "تمت إضافة المخزن بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
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
            btnSave.Enabled = true;
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
            btnDelete.Enabled = true;
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
