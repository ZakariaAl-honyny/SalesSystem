using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

/// <summary>
/// شاشة إدخال مستند المصروف (إضافة/تعديل) — WinForms كلاسيكي، منطق الشاشة في المعالجات (RULE-N109).
/// البنية (§2.34): العنوان ← أوامر السند ← بيانات السند ← التفاصيل ← الإجماليات ← شريط الأسفل.
/// </summary>
public partial class frmAddUpdateExpense : Form
{
    private readonly IServiceProvider _serviceProvider;

    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;
    private List<CashBoxDto> _cashBoxes = new();

    /// <summary>RULE-N04: الخدمات عبر خصائص مطبوعة النوع، لا استدعاء مباشر لأي طبقة أخرى.</summary>
    private IExpenseDesktopService ExpenseService => _serviceProvider.GetRequiredService<IExpenseDesktopService>();
    private ICashBoxDesktopService CashBoxService => _serviceProvider.GetRequiredService<ICashBoxDesktopService>();

    public frmAddUpdateExpense(IServiceProvider serviceProvider)
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

        Text = "إضافة مصروف جديد";
        _UpdateModeHeader();
        _UpdateNetTotal();
        _ = LoadComboBoxesAsync();
    }

    /// <summary>منشئ التعديل: يُستخدم من شاشة الدليل بعد حل النموذج من الحاوية (RULE-N02).</summary>
    public frmAddUpdateExpense(int id, IServiceProvider serviceProvider) : this(serviceProvider)
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
        toolTip1.SetToolTip(btnSave, "حفظ المصروف (F4)");
        toolTip1.SetToolTip(btnClose, "إلغاء وإغلاق السند (Esc)");
        toolTip1.SetToolTip(dtpDate, "تاريخ المصروف");
        toolTip1.SetToolTip(cmbCashBox, "الصندوق الذي يتحمّل المصروف (إلزامي)");
        toolTip1.SetToolTip(numAmount, "المبلغ — يظهر تفقيطه بالحروف تلقائياً (إلزامي)");
        toolTip1.SetToolTip(txtNotes, "بيان المصروف (إلزامي)");
    }

    // ── مؤشر الوضع وحالة الأزرار ────────────────────────────────────────────────

    /// <summary>RULE-N44: مؤشر الوضع يتحدث مع كل تغيير للحالة.</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _isEditMode ? $"تعديل مصروف رقم: {_editingId}" : "إضافة مصروف جديد";
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
        cmbCashBox.SelectedIndex = -1;
        numAmount.Value = 0m;
        txtNotes.Clear();
        errorProvider.Clear();

        _UpdateModeHeader();
        _UpdateNetTotal();
        cmbCashBox.Focus();
    }

    // ── تحميل البيانات ──────────────────────────────────────────────────────────

    private async Task LoadComboBoxesAsync()
    {
        try
        {
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
                MessageBox.Show(cashResult.Error ?? "تعذر تحميل قائمة الصناديق.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل قائمة الصناديق في شاشة المصروف");
            MessageBox.Show("تعذر تحميل قائمة الصناديق. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadExpenseAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await ExpenseService.GetByIdAsync(_editingId);

            if (result.IsSuccess && result.Value != null)
            {
                var expense = result.Value;
                dtpDate.Value = expense.ExpenseDate;
                numAmount.Value = expense.Amount;
                txtNotes.Text = expense.Notes ?? string.Empty;
                _UpdateNetTotal();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات المصروف", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل المصروف {ExpenseId}", _editingId);
            MessageBox.Show("تعذر تحميل بيانات المصروف. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (_isEditMode) await LoadExpenseAsync();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحديث شاشة المصروف");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        if (!_TryGetCashBoxId(out _))
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

        if (string.IsNullOrWhiteSpace(txtNotes.Text))
        {
            errorProvider.SetError(txtNotes, "بيان المصروف مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtNotes, string.Empty);
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    /// <summary>قراءة آمنة للصندوق المحدد (بلا تحويل صريح قد يرمي استثناء).</summary>
    private bool _TryGetCashBoxId(out int cashBoxId)
    {
        cashBoxId = 0;

        if (cmbCashBox.SelectedValue is int selected && selected > 0)
        {
            cashBoxId = selected;
            return true;
        }

        return false;
    }

    // ── الحفظ ──────────────────────────────────────────────────────────────────

    /// <summary>RULE-N25/N26: async void + try/catch + Cursor.WaitCursor واستعادته في finally.</summary>
    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;
        if (!_TryGetCashBoxId(out int cashBoxId)) return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            string? notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            if (_isEditMode)
            {
                var request = new UpdateExpenseRequest(
                    ExpenseDate: dtpDate.Value,
                    ExpenseAccountId: 0,
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: notes);

                var result = await ExpenseService.UpdateAsync(_editingId, request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث المصروف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var request = new CreateExpenseRequest(
                    ExpenseDate: dtpDate.Value,
                    ExpenseAccountId: 0,
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: notes);

                var result = await ExpenseService.CreateAsync(request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إضافة المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة المصروف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "فشل حفظ المصروف (وضع التعديل: {IsEditMode})", _isEditMode);
            MessageBox.Show("تعذر حفظ المصروف. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        _ = LoadExpenseAsync();
    }

    private void numAmount_ValueChanged(object? sender, EventArgs e)
        => _UpdateNetTotal();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
