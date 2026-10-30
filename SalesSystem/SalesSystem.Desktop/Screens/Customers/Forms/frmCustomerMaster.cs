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

    public frmCustomerMaster(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmCustomerMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvCustomers.CellClick += dgvCustomers_CellClick;
    }

    // ── Grid Setup ────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvCustomers.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",          Header = "رقم العميل",  Fill = 8,  Min = 90  },
            new { Name = "Name",        Header = "اسم العميل",  Fill = 40, Min = 220 },
            new { Name = "Phone",       Header = "الهاتف",      Fill = 20, Min = 130 },
            new { Name = "CreditLimit", Header = "حد الائتمان", Fill = 20, Min = 130 },
            new { Name = "IsActive",    Header = "نشط",         Fill = 8,  Min = 60  },
        };

        foreach (var col in columns)
        {
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }

        ApplyDevColumns();
    }

    /// <summary>عرض أسماء أعمدة الشبكة — للتطوير فقط (وضع البيانات التجريبية).</summary>
    private void ApplyDevColumns()
    {
        if (SalesSystem.Desktop.Configuration.AppSettings.Current.UseMockData)
        {
            lblDevColumns.Text = "الأعمدة: " + string.Join(" | ", dgvCustomers.Columns.Cast<DataGridViewColumn>().Select(c => c.HeaderText));
            lblDevColumns.Visible = true;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmCustomerMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            _SetupGridColumns();
            await _LoadCustomersAsync();
            _ResetForm();
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
            _ApplyFilter();
            _ApplyRowColoring();
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

    private void _ApplyFilter()
    {
        if (_customersView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _customersView.RowFilter = "";
            return;
        }

        _customersView.RowFilter =
            $"Name LIKE '%{text}%' OR Phone LIKE '%{text}%' OR Email LIKE '%{text}%' " +
            $"OR TaxNumber LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
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

    private void _ResetForm()
    {
        _editId = null;

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

    // ── Validation ────────────────────────────────────────────────────────────

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

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

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

            if (_editId is int id)
            {
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

                MessageBox.Show(wasEditing ? "تم تعديل العميل بنجاح." : "تمت إضافة العميل بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;
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
            btnSave.Enabled = true;
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
        btnDelete.Enabled = false;

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
            btnDelete.Enabled = true;
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
