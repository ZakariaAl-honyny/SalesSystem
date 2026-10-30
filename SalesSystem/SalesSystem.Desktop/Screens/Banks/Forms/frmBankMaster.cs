using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Banks.Forms;

/// <summary>
/// شاشة موحّدة (Master Form) لدليل البنوك.
/// تجمع: بحث مباشر + شبكة + نموذج إدخال/تعديل في شاشة واحدة،
/// وتعتمد على <c>_editId</c> (null = إضافة، &gt; 0 = تعديل).
/// </summary>
public partial class frmBankMaster : Form
{
    private readonly IServiceProvider _serviceProvider;

    private IBankApiService BankService => _serviceProvider.GetRequiredService<IBankApiService>();

    // ── State ────────────────────────────────────────────────────────────────
    private readonly Dictionary<int, BankDto> _banksById = new();
    private DataTable _banksTable = new();
    private DataView _banksView = new();

    /// <summary>null = إضافة بنك جديد، &gt; 0 = تعديل بنك قائم.</summary>
    private int? _editId;

    /// <summary>يُفعَّل عند محاولة حفظ ناقصة — يُعطّل زر الحفظ حتى تكتمل الحقول المطلوبة.</summary>
    private bool _validationFailed;

    /// <summary>منشئ التصميم — يستخدمه مصمم Visual Studio فقط (بدون خدمات DI).</summary>
    private frmBankMaster()
    {
        InitializeComponent();
    }

    public frmBankMaster(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        _WireEvents();
    }

    // ── Event wiring (the Designer defines controls only) ─────────────────────

    private void _WireEvents()
    {
        Load += frmBankMaster_Load;

        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnDelete.Click += btnDelete_Click;
        btnRefresh.Click += btnRefresh_Click;

        txtSearch.TextChanged += txtSearch_TextChanged;
        dgvBanks.CellClick += dgvBanks_CellClick;

        // التحقق الحيّ: يُفعَّل زر الحفظ افتراضياً، ويُعطَّل فقط بعد محاولة حفظ ناقصة
        // ثم يُعاد تفعيله تلقائياً عند إدخال اسم البنك.
        txtName.TextChanged += (s, e) => _UpdateSaveEnabled();
    }

    /// <summary>هل الحقل المطلوب (اسم البنك) مكتمل؟</summary>
    private bool _RequiredFieldsValid()
        => !string.IsNullOrWhiteSpace(txtName.Text);

    /// <summary>أسماء الحقول المطلوبة الناقصة (اسم البنك).</summary>
    private List<string> _MissingRequiredNames()
    {
        var missing = new List<string>();

        if (string.IsNullOrWhiteSpace(txtName.Text))
            missing.Add("اسم البنك");

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

    /// <summary>تحديث يدوي لقائمة البنوك مع الإبقاء على نص البحث الحالي.</summary>
    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadBanksAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث قائمة البنوك");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Form Load ─────────────────────────────────────────────────────────────

    private async void frmBankMaster_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadBanksAsync();
            _ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل دليل البنوك");
            MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Data Loading ──────────────────────────────────────────────────────────

    private async Task _LoadBanksAsync()
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("AccountNumber", typeof(string));
        table.Columns.Add("Iban", typeof(string));
        table.Columns.Add("IsActive", typeof(bool));

        try
        {
            var result = await BankService.GetAllAsync(includeInactive: true);

            _banksById.Clear();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var b in result.Value)
                {
                    _banksById[b.Id] = b;
                    table.Rows.Add(
                        b.Id,
                        b.Name,
                        b.AccountNumber ?? "",
                        b.Iban ?? "",
                        b.IsActive);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "تعذر تحميل قائمة البنوك.",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل قائمة البنوك");
            MessageBox.Show("تعذر تحميل قائمة البنوك. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _banksTable = table;
            _banksView = new DataView(_banksTable);
            dgvBanks.DataSource = _banksView;
            _ApplyFilter();
            _ApplyRowColoring();
        }
    }

    private void _ApplyRowColoring()
    {
        foreach (DataGridViewRow row in dgvBanks.Rows)
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
        if (_banksView is null) return;

        string text = txtSearch.Text.Trim().Replace("'", "''");

        if (string.IsNullOrEmpty(text))
        {
            _banksView.RowFilter = "";
            return;
        }

        _banksView.RowFilter =
            $"Name LIKE '%{text}%' OR AccountNumber LIKE '%{text}%' OR Iban LIKE '%{text}%' " +
            $"OR CONVERT(Id, 'System.String') LIKE '{text}%'";
    }

    // ── Grid Selection ────────────────────────────────────────────────────────

    private void dgvBanks_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (dgvBanks.Rows[e.RowIndex].DataBoundItem is not DataRowView row) return;
            if (row["Id"] is not int id || id <= 0) return;

            _FillForm(id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار بنك من الشبكة");
            MessageBox.Show("تعذر عرض بيانات البنك المحدد. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void _FillForm(int bankId)
    {
        if (!_banksById.TryGetValue(bankId, out var b)) return;

        _editId = bankId;

        // وضع التعديل: زر الحفظ يُحدّث السجل المحدد، وزر الحذف متاح.
        btnSave.Text = "💾 تعديل";
        btnDelete.Enabled = true;

        errorProvider.Clear();

        txtCode.Text = bankId.ToString();
        txtName.Text = b.Name;
        txtAccountNumber.Text = b.AccountNumber ?? "";
        txtIban.Text = b.Iban ?? "";
        txtAccountName.Text = b.AccountName ?? "";

        _UpdateModeHeader();

        _validationFailed = false;
        _UpdateSaveEnabled();
    }

    // ── Mode / Reset ──────────────────────────────────────────────────────────

    private void _UpdateModeHeader()
        => lblMode.Text = _editId.HasValue
            ? "(( دليل البنوك )) — تعديل بنك"
            : "(( دليل البنوك )) — إضافة بنك جديد";

    private void _ResetForm()
    {
        _editId = null;

        // وضع الإضافة: زر الحفظ يُضيف سجلاً جديداً، وزر الحذف معطّل لعدم وجود سجل محدد.
        btnSave.Text = "💾 إضافة";
        btnDelete.Enabled = false;

        errorProvider.Clear();

        txtCode.Clear();
        txtName.Clear();
        txtAccountNumber.Clear();
        txtIban.Clear();
        txtAccountName.Clear();

        dgvBanks.ClearSelection();
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
            errorProvider.SetError(txtName, "اسم البنك مطلوب");
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

            errorProvider.SetError(txtName, "اسم البنك مطلوب");
            return;
        }

        if (!_ValidateForm()) return;

        bool wasEditing = _editId.HasValue;
        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string name = txtName.Text.Trim();
            string? accountNumber = string.IsNullOrWhiteSpace(txtAccountNumber.Text)
                ? null
                : txtAccountNumber.Text.Trim();
            string? iban = string.IsNullOrWhiteSpace(txtIban.Text)
                ? null
                : txtIban.Text.Trim();

            Result<BankDto> result;

            if (_editId is int id)
            {
                // AccountId غير قابل للتغيير بعد الإنشاء — لا يُرسَل في طلب التعديل.
                var request = new UpdateBankRequest(name, accountNumber, iban);
                result = await BankService.UpdateAsync(id, request);
            }
            else
            {
                // AccountId = null → الخدمة تنشئ الحساب المحاسبي المرتبط تلقائياً.
                var request = new CreateBankRequest(
                    AccountId: null,
                    Name: name,
                    AccountNumber: accountNumber,
                    Iban: iban,
                    BranchId: null);

                result = await BankService.CreateAsync(request);
            }

            if (result.IsSuccess && result.Value != null)
            {
                MessageBox.Show(wasEditing ? "تم تعديل البنك بنجاح." : "تمت إضافة البنك بنجاح.",
                    "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _editId = result.Value.Id;

                // بعد الحفظ ينتقل النموذج إلى وضع التعديل (السجل محدد).
                btnSave.Text = "💾 تعديل";
                btnDelete.Enabled = true;

                await _LoadBanksAsync();
                _SelectGridRow(result.Value.Id);
                _UpdateModeHeader();
            }
            else
            {
                MessageBox.Show($"لم يتم حفظ البنك.\n{result.Error}",
                    "فشل الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ البنك");
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
            MessageBox.Show("يرجى اختيار بنك من القائمة أولاً.",
                "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string name = txtName.Text.Trim();

        if (MessageBox.Show($"هل أنت متأكد من حذف (تعطيل) البنك \"{name}\"؟",
                "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        Cursor = Cursors.WaitCursor;
        btnDelete.Enabled = false;

        try
        {
            var result = await BankService.DeactivateAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم تعطيل البنك بنجاح.", "تم الحذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ResetForm();
                await _LoadBanksAsync();
            }
            else
            {
                MessageBox.Show($"لم يتم تعطيل البنك.\n{result.Error}",
                    "فشل الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تعطيل البنك {BankId}", id);
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

    private void _SelectGridRow(int bankId)
    {
        foreach (DataGridViewRow row in dgvBanks.Rows)
        {
            if (row.Cells["Id"].Value is int id && id == bankId)
            {
                row.Selected = true;
                dgvBanks.CurrentCell = row.Cells["Name"];
                break;
            }
        }
    }
}
