using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;

namespace SalesSystem.Desktop.Screens.Payments.Forms;

/// <summary>
/// حوار سند الصرف للمورد — WinForms كلاسيكي، منطق الشاشة في المعالجات (RULE-N109).
/// البنية (§2.34): بيانات السند (gbPaymentInfo) ← شريط الأسفل بالأوامر، بلا gbFilter ولا pnlSummary.
/// </summary>
public partial class frmSupplierPaymentDialog : Form
{
    private readonly IServiceProvider _serviceProvider;

    private ISupplierPaymentDesktopService PaymentApi => _serviceProvider.GetRequiredService<ISupplierPaymentDesktopService>();
    private ISupplierDesktopService SupplierApi => _serviceProvider.GetRequiredService<ISupplierDesktopService>();
    private IPurchaseInvoiceDesktopService PurchaseApi => _serviceProvider.GetRequiredService<IPurchaseInvoiceDesktopService>();
    private INotificationService Notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    private List<SupplierDto> _suppliers = new();
    private decimal _currentBalance = 0;

    public frmSupplierPaymentDialog(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N95 — الستة من KeyboardHelper معاً في المُنشئ.
        KeyboardHelper.WireEnterAsTab(this);                                        // RULE-N40
        KeyboardHelper.WireSelectAllOnFocus(this);                                  // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));  // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnSave, btnCancel);                // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F2, _ResetForm),
            (Keys.F4, () => { if (btnSave.Enabled) btnSave.PerformClick(); }),
            (Keys.F5, () => _ = _RefreshAsync()),
            (Keys.Escape, () => btnCancel.PerformClick()));

        // RULE-N97 — التفقيط الحيّ كتلميح على حقل المبلغ.
        LiveTafqeetHelper.Attach(liveTip, (numAmount, "المبلغ المدفوع", LiveTafqeetHelper.ReadMode.Money));

        _SetupToolTips();
    }

    // ── الثيم والتلميحات ────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53): يبدأ بـ ThemeHelper وينتهي بـ GreenThemeHelper.Apply
    /// **كآخر سطر** — والقيم النهائية للأزرار مطابقة حرفياً لِما في *.Designer.cs (RULE-N134/N140).
    /// لا شريط عنوان (lblMode) ولا شبكة في هذا الحوار: لا ApplyHeader ولا ApplyGrid.
    /// </summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.ButtonType.Neutral);

        // يُعاد ضبط ما فرضه ThemeHelper (AutoSize/خط Segoe UI) إلى قيم المصمّم.
        foreach (var btn in new[] { btnSave, btnCancel })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        }

        btnSave.Size = new Size(140, 40);
        btnCancel.Size = new Size(136, 40);

        GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الأزرار والحقول مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnSave, "حفظ سند الصرف (F4)");
        toolTip1.SetToolTip(btnCancel, "إلغاء وإغلاق السند (Esc)");
        toolTip1.SetToolTip(cmbSupplier, "المورد الذي يُسدد له (إلزامي)");
        toolTip1.SetToolTip(cmbInvoice, "ربط السداد بفاتورة شراء غير مسددة (اختياري)");
        toolTip1.SetToolTip(numAmount, "المبلغ المدفوع — يظهر تفقيطه بالحروف تلقائياً (إلزامي)");
        toolTip1.SetToolTip(dtpDate, "تاريخ سند الصرف");
        toolTip1.SetToolTip(txtNotes, "ملاحظات السند (اختياري)");
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        ApplyUiTheme();   // RULE-N50 — الأخير في مسار التحميل
        await _LoadSuppliersAsync();
    }

    private async Task _LoadSuppliersAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var res = await SupplierApi.GetAllAsync();
            if (res.IsSuccess)
            {
                _suppliers = res.Value?.ToList() ?? new List<SupplierDto>();

                cmbSupplier.DataSource = _suppliers;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.ValueMember = "Id";
                cmbSupplier.SelectedIndex = -1;
            }
            else
            {
                Notification.ShowError(res.Error ?? "تعذر تحميل قائمة الموردين.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل قائمة الموردين في سند صرف");
            Notification.ShowError("حدث خطأ في تحميل البيانات.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    /// <summary>F5 — تحديث قائمة الموردين.</summary>
    private async Task _RefreshAsync()
    {
        if (IsDisposed) return;
        await _LoadSuppliersAsync();
    }

    /// <summary>F2 — تفريغ السند لعملية سداد جديدة.</summary>
    private void _ResetForm()
    {
        _currentBalance = 0m;

        cmbSupplier.SelectedIndex = -1;
        cmbInvoice.DataSource = null;
        numAmount.Value = 0m;
        dtpDate.Value = DateTime.Today;
        txtNotes.Clear();
        lblBalanceVal.Text = "0.00";
        lblBalanceVal.ForeColor = Color.Blue;
        errorProvider.Clear();

        cmbSupplier.Focus();
    }

    // ── التغيير والتحقق ────────────────────────────────────────────────────────

    private async void cmbSupplier_SelectedIndexChanged(object? sender, EventArgs e)
    {
        try
        {
            await OnSupplierChanged();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تغيير المورد في سند صرف");
        }
    }

    private async Task OnSupplierChanged()
    {
        if (cmbSupplier.SelectedValue is not int supplierId)
            return;

        var supplier = _suppliers.FirstOrDefault(s => s.Id == supplierId);
        if (supplier != null)
        {
            _currentBalance = supplier.CurrentBalance;
            lblBalanceVal.Text = _currentBalance.ToString("N2");
            lblBalanceVal.ForeColor = _currentBalance > 0 ? Color.Red : Color.Blue;
        }

        // تحميل فواتير الشراء غير المسددة للمورد (المرحّلة فقط).
        var invoicesRes = await PurchaseApi.GetAllAsync(status: 2);
        if (invoicesRes.IsSuccess && invoicesRes.Value != null)
        {
            var list = invoicesRes.Value.Where(i => i.SupplierId == supplierId && i.DueAmount > 0).ToList();
            cmbInvoice.DataSource = list;
            cmbInvoice.DisplayMember = "InvoiceNo";
            cmbInvoice.ValueMember = "Id";
            cmbInvoice.SelectedIndex = -1;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbSupplier.SelectedValue is not int)
        {
            errorProvider.SetError(cmbSupplier, "يرجى اختيار المورد.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbSupplier, string.Empty);
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
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── الحفظ ──────────────────────────────────────────────────────────────────

    private async void btnSave_Click(object? sender, EventArgs e) => await SaveAsync();

    /// <summary>RULE-N25/N26: async مع Cursor.WaitCursor واستعادته في finally.</summary>
    private async Task SaveAsync()
    {
        if (!_ValidateForm()) return;

        int supplierId = (int)cmbSupplier.SelectedValue;
        decimal amount = numAmount.Value;

        if (amount > _currentBalance)
        {
            if (MessageBox.Show("المبلغ المدفوع أكبر من الرصيد. هل تريد المتابعة؟", "تنبيه",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
        }

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            int? invoiceId = cmbInvoice.SelectedValue as int?;
            var request = new CreateSupplierPaymentRequest(
                supplierId, amount, SalesSystem.DTO.Enums.PaymentMethod.Cash, dtpDate.Value, invoiceId, null, txtNotes.Text);

            var res = await PaymentApi.CreateAsync(request);
            if (res.IsSuccess)
            {
                Notification.ShowSuccess("تم حفظ السداد بنجاح");
                EventBus.Publish(new SupplierPaymentChangedMessage(res.Value.Id));
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                Notification.ShowError(res.Error ?? "تعذر حفظ سند الصرف.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ سند الصرف");
            Notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
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

    private void btnCancel_Click(object? sender, EventArgs e) => Close();
}
