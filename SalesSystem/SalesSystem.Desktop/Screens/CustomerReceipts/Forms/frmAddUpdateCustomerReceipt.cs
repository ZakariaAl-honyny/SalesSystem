using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.CustomerReceipts.Forms;

/// <summary>
/// شاشة إدخال سند القبض (إضافة/تعديل) — WinForms كلاسيكي، منطق الشاشة في المعالجات (RULE-N109).
/// البنية (§2.34): العنوان ← أوامر السند ← بيانات السند ← التفاصيل ← الإجماليات ← شريط الأسفل.
/// </summary>
public partial class frmAddUpdateCustomerReceipt : Form
{
    private readonly IServiceProvider _serviceProvider;

    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;
    private List<CustomerDto> _customers = new();
    private List<CashBoxDto> _cashBoxes = new();

    /// <summary>RULE-N04: الخدمات عبر خصائص مطبوعة النوع.</summary>
    private ICustomerReceiptDesktopService ReceiptService => _serviceProvider.GetRequiredService<ICustomerReceiptDesktopService>();
    private ICustomerDesktopService CustomerService => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
    private ICashBoxDesktopService CashBoxService => _serviceProvider.GetRequiredService<ICashBoxDesktopService>();

    public frmAddUpdateCustomerReceipt(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N95 — الستة من KeyboardHelper معاً في المُنشئ.
        KeyboardHelper.WireEnterAsTab(this);                                        // RULE-N40
        KeyboardHelper.WireSelectAllOnFocus(this);                                  // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));  // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnSave);                           // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F2, _StartNewEntry),
            (Keys.F4, () => { if (btnSave.Enabled) btnSave.PerformClick(); }),
            (Keys.F5, () => _ = _RefreshAsync()),
            (Keys.Escape, () => btnClose.PerformClick()));

        // RULE-N97 — التفقيط الحيّ كتلميح على حقل المبلغ.
        LiveTafqeetHelper.Attach(liveTip, (numAmount, "المبلغ", LiveTafqeetHelper.ReadMode.Money));

        _SetupToolTips();
        ApplyUiTheme();

        Text = "سند قبض جديد";
        _UpdateModeHeader();
        _UpdateNetTotal();
        _ = LoadComboBoxesAsync();
    }

    /// <summary>منشئ التعديل: يُستخدم من شاشة الدليل بعد حل النموذج من الحاوية (RULE-N02).</summary>
    public frmAddUpdateCustomerReceipt(int id, IServiceProvider serviceProvider) : this(serviceProvider)
    {
        _SetUpdateMode(id);
    }

    // ── الثيم والتلميحات ────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53): يبدأ بـ ThemeHelper وينتهي بـ GreenThemeHelper.Apply
    /// **كآخر سطر** — والقيم النهائية للأزرار هي نفس القيم الحرفية في *.Designer.cs (RULE-N134/N140).
    /// لا DataGridView في هذه الشاشة (شاشة إدخال مستند) فلا استدعاء لـ ApplyGrid.
    /// </summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        // يُعاد ضبط ما فرضه ThemeHelper (خط Segoe UI/حجم مرن) إلى قيم المصمّم حرفياً.
        foreach (var btn in new[] { btnSave, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(140, 40);
        }

        GreenThemeHelper.ApplyHeader(lblMode);                    // RULE-N53
        lblNetTotalAmount.ForeColor = GreenThemeHelper.TextStrong;
        GreenThemeHelper.Apply(this);                             // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الأزرار والحقول مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnSave, "حفظ سند القبض (F4)");
        toolTip1.SetToolTip(btnClose, "إلغاء وإغلاق السند (Esc)");
        toolTip1.SetToolTip(dtpDate, "تاريخ سند القبض");
        toolTip1.SetToolTip(cmbCustomer, "العميل الذي يُحصَّل منه المبلغ (إلزامي)");
        toolTip1.SetToolTip(cmbCashBox, "الصندوق الذي يُودَع فيه المبلغ (إلزامي)");
        toolTip1.SetToolTip(numAmount, "المبلغ — يظهر تفقيطه بالحروف تلقائياً (إلزامي)");
        toolTip1.SetToolTip(txtNotes, "بيان سند القبض (اختياري)");
    }

    // ── مؤشر الوضع وحالة الأزرار ────────────────────────────────────────────────

    /// <summary>RULE-N44: مؤشر الوضع يتحدث مع كل تغيير للحالة.</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _isEditMode ? $"تعديل سند قبض رقم: {_editingId}" : "سند قبض جديد";
        Text = lblMode.Text;
    }

    /// <summary>إجمالي السند بصيغة N2 (RULE-001).</summary>
    private void _UpdateNetTotal()
        => lblNetTotalAmount.Text = clsFormat.Money(numAmount.Value);

    /// <summary>F2 — سند جديد: تفريغ الحقول والعودة إلى وضع الإضافة.</summary>
    private void _StartNewEntry()
    {
        _editingId = -1;

        dtpDate.Value = DateTime.Today;
        cmbCustomer.SelectedIndex = -1;
        cmbCashBox.SelectedIndex = -1;
        numAmount.Value = 0m;
        txtNotes.Clear();
        errorProvider.Clear();

        _UpdateModeHeader();
        _UpdateNetTotal();
        cmbCustomer.Focus();
    }

    // ── تحميل البيانات ──────────────────────────────────────────────────────────

    private async Task LoadComboBoxesAsync()
    {
        try
        {
            var customerResult = await CustomerService.GetAllAsync();
            if (customerResult.IsSuccess && customerResult.Value != null)
            {
                _customers = customerResult.Value.Where(c => c.IsActive).ToList();
                cmbCustomer.DataSource = _customers.Select(c => new { c.Id, c.Name }).ToList();
                cmbCustomer.DisplayMember = "Name";
                cmbCustomer.ValueMember = "Id";
                cmbCustomer.SelectedIndex = -1;
            }
            else
            {
                clsDialogs.Error(customerResult.Error ?? "تعذر تحميل قائمة العملاء.");
            }

            var cashResult = await CashBoxService.GetAllAsync();
            if (cashResult.IsSuccess && cashResult.Value != null)
            {
                _cashBoxes = cashResult.Value.Where(b => b.IsActive).ToList();
                cmbCashBox.DataSource = _cashBoxes.Select(b => new { b.Id, b.Name }).ToList();
                cmbCashBox.DisplayMember = "Name";
                cmbCashBox.ValueMember = "Id";
                cmbCashBox.SelectedIndex = -1;
            }
            else
            {
                clsDialogs.Error(cashResult.Error ?? "تعذر تحميل قائمة الصناديق.");
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل قوائم سند القبض");
            clsDialogs.Error("تعذر تحميل القوائم. يرجى إعادة المحاولة.");
        }
    }

    private async Task LoadReceiptAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await ReceiptService.GetByIdAsync(_editingId);

            if (result.IsSuccess && result.Value != null)
            {
                var receipt = result.Value;
                cmbCustomer.SelectedValue = receipt.CustomerId;
                cmbCashBox.SelectedValue = receipt.CashBoxId;
                numAmount.Value = receipt.Amount;
                dtpDate.Value = receipt.ReceiptDate;
                txtNotes.Text = receipt.Notes ?? string.Empty;
                _UpdateNetTotal();
            }
            else
            {
                clsDialogs.Error(result.Error ?? "فشل في تحميل بيانات السند");
                Close();
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل سند القبض {ReceiptId}", _editingId);
            clsDialogs.Error("تعذر تحميل بيانات السند. يرجى إعادة المحاولة.");
            Close();
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    /// <summary>F5 — تحديث القوائم والسجل الحالي بلا إغلاق الشاشة.</summary>
    private async Task _RefreshAsync()
    {
        if (IsDisposed) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            await LoadComboBoxesAsync();
            if (_isEditMode) await LoadReceiptAsync();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحديث شاشة سند القبض");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    // ── التحقق ─────────────────────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (!_TryGetSelectedId(cmbCustomer, out _))
        {
            errorProvider.SetError(cmbCustomer, "يرجى اختيار العميل.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCustomer, string.Empty);
        }

        if (!_TryGetSelectedId(cmbCashBox, out _))
        {
            errorProvider.SetError(cmbCashBox, "يرجى اختيار الصندوق.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCashBox, string.Empty);
        }

        if (numAmount.Value <= 0)
        {
            errorProvider.SetError(numAmount, "المبلغ يجب أن يكون أكبر من صفر.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numAmount, string.Empty);
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    /// <summary>قراءة آمنة للمعرّف المحدد في قائمة (بلا تحويل صريح قد يرمي استثناء).</summary>
    private static bool _TryGetSelectedId(ComboBox combo, out int id)
    {
        id = 0;

        if (combo.SelectedValue is int selected && selected > 0)
        {
            id = selected;
            return true;
        }

        return false;
    }

    // ── الحفظ ──────────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: async void + try/catch + Cursor.WaitCursor واستعادته في finally.</summary>
    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;
        if (!_TryGetSelectedId(cmbCustomer, out int customerId)) return;
        if (!_TryGetSelectedId(cmbCashBox, out int cashBoxId)) return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string? notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            if (_isEditMode)
            {
                var request = new UpdateCustomerReceiptRequest(
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: notes);

                var result = await ReceiptService.UpdateAsync(_editingId, request);
                if (result.IsSuccess)
                {
                    clsDialogs.Success("تم تحديث سند القبض بنجاح.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    clsDialogs.Error(result.Error ?? "فشل في تحديث السند");
                }
            }
            else
            {
                var request = new CreateCustomerReceiptRequest(
                    CustomerId: customerId,
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: notes);

                var result = await ReceiptService.CreateAsync(request);
                if (result.IsSuccess)
                {
                    clsDialogs.Success("تم إضافة سند القبض بنجاح.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    clsDialogs.Error(result.Error ?? "فشل في إضافة السند");
                }
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل حفظ سند القبض (وضع التعديل: {IsEditMode})", _isEditMode);
            clsDialogs.Error("تعذر حفظ السند. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed)
            {
                btnSave.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>
    /// يضبط المحرر على وضع التعديل لسجل محدد — يُستدعى من شاشة الدليل بعد حل النموذج من DI.
    /// </summary>
    public void _SetUpdateMode(int id)
    {
        _editingId = id;
        _UpdateModeHeader();
        _ = LoadReceiptAsync();
    }

    private void numAmount_ValueChanged(object? sender, EventArgs e)
        => _UpdateNetTotal();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
