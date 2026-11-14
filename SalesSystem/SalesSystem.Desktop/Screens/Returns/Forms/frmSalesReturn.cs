using Serilog;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Models;
using SalesSystem.Desktop.Messaging.Messages;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Returns;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

public partial class frmSalesReturn : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISalesReturnDesktopService _returnApi => _serviceProvider.GetRequiredService<ISalesReturnDesktopService>();
    private ICustomerDesktopService _customerApi => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
    private IProductDesktopService _productApi => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private INotificationService _notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();

    private SalesReturnDto? _invoice;
    private BindingList<InvoiceLineItem> _lines = new();
    private List<ProductDto> _allProducts = new();
    private bool _isUpdating = false;

    public frmSalesReturn(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (4 من 6).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnAddItem, btnRemoveItem, btnSaveDraft, btnPost, btnClose);

        // RULE-N47 — الاختصارات الموحّدة على الأزرار الموجودة فعلاً في الشاشة.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddItem.PerformClick()),        // جديد = سطر صنف جديد
            (Keys.F3, _SaveDraftIfAllowed),                    // حفظ/عرض المسودة
            (Keys.F4, _PostIfAllowed),                         // ترحيل
            (Keys.F5, _RefreshAsync),                          // تحديث
            (Keys.Delete, _GlobalDelete),                      // حذف السطر المحدد
            (Keys.Escape, _GlobalEscape));                     // خروج

        // RULE-N147 — Enter يُنفِّذ الزر الأساسي للوضع الحالي (مرتجع قابل للتحرير ⇒ «حفظ مسودة»)،
        // ولا يعمل داخل شبكة الأصناف: Enter هناك تنقّل/تحرير داخل الشبكة لا تنفيذ زر.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;

        SetupGrid();
        ApplyUiTheme();   // ينتهي بالثيم الأخضر (RULE-N50)
    }

    public void LoadData(SalesReturnDto? invoice)
    {
        _invoice = invoice;
        if (_invoice != null) BindInvoice();
        else ResetForm();
        UpdateUIState();
    }

    // ── الاختصارات العامة (RULE-N47) ─────────────────────────────────────────

    /// <summary>Delete: حذف السطر المحدد — يعمل فقط عندما تكون الشبكة هي صاحبة التركيز.</summary>
    private void _GlobalDelete()
    {
        if (dgvItems.ContainsFocus && btnRemoveItem.Enabled) btnRemoveItem.PerformClick();
    }

    /// <summary>Esc: يُلغي تحرير الخلية أولاً، وإلا يُغلق النموذج (RULE-N47).</summary>
    private void _GlobalEscape()
    {
        if (dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            return;
        }

        btnClose.PerformClick();
    }

    /// <summary>
    /// F3 = حفظ المسودة — لا يُفعَّل إلا من زر ظاهر ومفعَّل (RULE-N47).
    /// عند عرض مرتجع مرحَّل يُخفى الزر فقط وتبقى قيمته مفعّلة، وButton.PerformClick()
    /// يفحص Enabled لا Visible ⇒ كان F3/F4 يبنيان طلباً جديداً من سطور مرتجع مرحَّل.
    /// </summary>
    private void _SaveDraftIfAllowed()
    {
        if (btnSaveDraft is { Visible: true, Enabled: true }) btnSaveDraft.PerformClick();
    }

    /// <summary>F4 = ترحيل — نفس حارس الزر الظاهر والمفعَّل المذكور أعلاه (RULE-N47).</summary>
    private void _PostIfAllowed()
    {
        if (btnPost is { Visible: true, Enabled: true }) btnPost.PerformClick();
    }

    /// <summary>F5: إعادة تحميل القوائم وإعادة حساب الإجماليات (RULE-N47).</summary>
    private async void _RefreshAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            // نحفظ الاختيارات قبل إعادة الربط حتى لا تتغير بيانات المرتجع الحالي.
            int customerId = Convert.ToInt32(cmbCustomer.SelectedValue ?? 0);
            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0);

            await LoadLookups();

            if (customerId > 0) cmbCustomer.SelectedValue = customerId;
            if (warehouseId > 0) cmbWarehouse.SelectedValue = warehouseId;

            CalculateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحديث بيانات مرتجع المبيعات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>مؤشر الوضع في شريط العنوان (RULE-N44).</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _invoice == null
            ? "(( مرتجع مبيعات جديد ))"
            : $"(( مرتجع مبيعات — {_invoice.ReturnNo} ))";
    }

    /// <summary>المظهر: ThemeHelper أولاً ثم الثيم الأخضر أخيراً (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        // الأزرار: ألوانها وخط Tahoma معلنة حرفياً في المصمّم بقيم مطابقة لثوابت الثيم
        // (RULE-N134 / RULE-N144) — فلا يُعاد تنسيقها وقت التشغيل، وتبقى حالة التعطيل
        // المرسومة بـ WireButtonDisabledVisual (RULE-N43) سليمة دون أن يمحوها الثيم.

        GreenThemeHelper.ApplyHeader(lblMode);   // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvItems);    // RULE-N51/N52

        GreenThemeHelper.Apply(this);            // ← الأخير دائماً (RULE-N50)

        // RULE-N97 — لا يوجد حقل مالي في الشاشة بعد إسقاط حقول الضريبة (قرار المالك: MVP V1 بلا ضريبة).
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            Cursor = Cursors.WaitCursor;                 // RULE-N26 — تغذية بصرية أثناء تحميل القوائم
            await LoadLookups();
            if (_invoice == null) ResetForm();
            ApplyUiTheme();

            // RULE-N22a — الشبكة شبكة إدخال: التحرير والصف الجديد مملوكان للشاشة (RULE-N52).
            dgvItems.AllowUserToAddRows = true;
            dgvItems.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            dgvItems.MultiSelect = true;
            dgvItems.ReadOnly = _invoice != null && _invoice.Status != (byte)InvoiceStatus.Draft;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل النموذج");
            clsDialogs.Error("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;                    // RULE-N26 — استعادة المؤشر دائماً
        }
    }

    private async Task LoadLookups()
    {
        try
        {
            Cursor = Cursors.WaitCursor;                 // RULE-N26 — مؤشر انتظار أثناء جلب القوائم

            var warehouseRes = await _warehouseApi.GetAllAsync();
            if (warehouseRes.IsSuccess)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
            }

            var customerRes = await _customerApi.GetAllAsync();
            if (customerRes.IsSuccess)
            {
                var list = customerRes.Value!.ToList();
                list.Insert(0, new CustomerDto(0, "عميل نقدي", "", "", "", "", 0, true, 0));
                cmbCustomer.DataSource = list;
                cmbCustomer.DisplayMember = "Name";
                cmbCustomer.ValueMember = "Id";
            }

            var productRes = await _productApi.GetAllAsync();
            if (productRes.IsSuccess) _allProducts = productRes.Value!.ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل البيانات");
            clsDialogs.Error("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;                    // RULE-N26 — استعادة المؤشر في كل الحالات
        }
    }

    private void BindInvoice()
    {
        _isUpdating = true;
        this.Text = $"مرتجع مبيعات - {_invoice!.ReturnNo}";
        lblInvoiceNo.Text = _invoice.ReturnNo.ToString();
        dtpDate.Value = _invoice.ReturnDate;
        cmbCustomer.SelectedValue = _invoice.CustomerId;
        cmbWarehouse.SelectedValue = _invoice.WarehouseId;
        txtNotes.Text = _invoice.Notes;

        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItem {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount
            });
        }
        _isUpdating = false;
        CalculateTotals();

        _UpdateModeHeader();
    }

    private void ResetForm()
    {
        this.Text = "مرتجع مبيعات جديد";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        _lines.Clear();
        txtNotes.Clear();

        _UpdateModeHeader();
    }

    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 250, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "UnitPrice", Width = 100 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 100, ReadOnly = true });
        dgvItems.DataSource = _lines;
        dgvItems.CellEndEdit += (s, e) => {
            if (_isUpdating || e.RowIndex < 0) return;
            var row = _lines[e.RowIndex];
            if (dgvItems.Columns[e.ColumnIndex].Name == "ColCode") {
                var p = _allProducts.FirstOrDefault(x => x.Barcode != null && x.Barcode == row.ProductCode);
                if (p != null) { row.ProductId = p.Id; row.ProductName = p.Name; if (row.Quantity == 0) row.Quantity = 1; }
            }
            CalculateTotals();
            dgvItems.Refresh();
        };
    }

    // ── أحداث الحقول (كما كانت مربوطة في المصمّم) ────────────────────────────

    private void CalculateTotals()
    {
        decimal total = _lines.Sum(x => x.LineTotal);
        lblTotalVal.Text = clsFormat.Money(total);
    }

    private void UpdateUIState()
    {
        bool editable = _IsEditable();
        pnlHeader.Enabled = editable;
        dgvItems.ReadOnly = !editable;
        pnlFooter.Enabled = editable;
        btnAddItem.Enabled = editable;
        btnRemoveItem.Enabled = editable;
        btnSaveDraft.Visible = editable;
        btnPost.Visible = editable;
        lblStatus.Text = _invoice?.Status switch { 1 => "مسودة", 2 => "مرحل", 3 => "ملغي", _ => "جديد" };

        // ألوان الحالة من ثوابت ThemeHelper (لا أرقام لون داخل الشاشة) — مطابقة لقيمة المصمّم.
        lblStatus.BackColor = _invoice?.Status switch
        {
            1 => ThemeHelper.Neutral,
            2 => ThemeHelper.Success,
            3 => ThemeHelper.Danger,
            _ => ThemeHelper.Danger
        };

        _UpdateModeHeader();

        // RULE-N147 — الزر الافتراضي للشاشة يتبع الوضع الحالي (يُحدَّث بعد كل انتقال حالة).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>هل المرتجع قابل للتحرير؟ (جديد أو مسودة فقط — لا تعديل على مرحَّل/ملغي).</summary>
    private bool _IsEditable() => _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): مرتجع جديد أو مسودة قابلة للتحرير ⇒ «حفظ مسودة».
    /// يُرجع <c>null</c> — أي لا إجراء لـEnter — في حالتين:
    /// (1) التركيز داخل شبكة الأصناف: Enter هناك تنقّل/تحرير داخل الشبكة لا تنفيذ زر (حماية إلزامية)،
    /// (2) مرتجع مرحَّل أو ملغي: لا إجراء حفظ عليه، والترحيل يبقى F4 صريحاً لأنه يغيّر المخزون
    ///     ورصيد العميل ويحتاج تأكيداً مقصوداً.
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvItems.ContainsFocus) return null;
        if (!_IsEditable()) return null;

        return btnSaveDraft is { Visible: true, Enabled: true } ? btnSaveDraft : null;
    }

    /// <summary>
    /// حماية Enter داخل شبكة الأصناف: الزر الافتراضي (<see cref="Form.AcceptButton"/>) لا يُنفَّذ إطلاقاً
    /// والتركيز داخل الشبكة — Enter هناك تنقّل/تحرير داخل الشبكة فقط، فلا يُنشأ مرتجع بالخطأ.
    /// </summary>
    protected override bool ProcessDialogKey(Keys keyData)
    {
        if ((keyData == Keys.Enter || keyData == Keys.Return) && dgvItems.ContainsFocus)
        {
            var saved = AcceptButton;
            AcceptButton = null;
            try { return base.ProcessDialogKey(keyData); }
            finally { AcceptButton = saved; }
        }

        return base.ProcessDialogKey(keyData);
    }

    private void btnAddItem_Click(object sender, EventArgs e) { _lines.Add(new InvoiceLineItem()); dgvItems.Focus(); }
    private void btnRemoveItem_Click(object sender, EventArgs e) { if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItem line) { _lines.Remove(line); CalculateTotals(); } }

    private async void btnSaveDraft_Click(object sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object sender, EventArgs e) => await SaveInvoiceAsync(true);

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbCustomer.SelectedValue == null || (int)cmbCustomer.SelectedValue <= 0)
        {
            const string customerMessage = "يرجى اختيار العميل";
            errorProvider.SetError(cmbCustomer, customerMessage);
            // GAP-08b — كل رفض errorProvider مقرون بتغذية بصرية من البيت الواحد (RULE-N169).
            clsInputFeedback.ShowRejectTooltip(cmbCustomer, customerMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCustomer, "");
        }

        if (cmbWarehouse.SelectedValue == null || (int)cmbWarehouse.SelectedValue <= 0)
        {
            const string warehouseMessage = "يرجى اختيار المستودع";
            errorProvider.SetError(cmbWarehouse, warehouseMessage);
            clsInputFeedback.ShowRejectTooltip(cmbWarehouse, warehouseMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbWarehouse, "");
        }

        if (!_lines.Any(l => l.ProductId > 0))
        {
            const string itemsMessage = "يجب إضافة صنف واحد على الأقل للإرجاع";
            errorProvider.SetError(dgvItems, itemsMessage);
            clsInputFeedback.ShowRejectTooltip(dgvItems, itemsMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvItems, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        // حاجز الأمان: لا يُبنى طلب جديد من مرتجع ليس مسودة (مرحَّل/ملغي) —
        // يحمي من إعادة الحفظ/الترحيل عبر الاختصارات F3/F4 أو أي مسار آخر.
        if (_invoice is not null && _invoice.Status != (byte)InvoiceStatus.Draft) return;

        if (!_ValidateForm()) return;

        if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم زيادة المخزون وتقليل رصيد العميل.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try {
            var items = _lines.Where(l => l.ProductId > 0).Select(l => new ReturnItemRequest(
                0, l.ProductId, 1, l.Quantity, l.UnitPrice, l.LineTotal, l.DiscountAmount, 1, null)).ToList();
            int customerIdVal = Convert.ToInt32(cmbCustomer.SelectedValue ?? 0);
            short warehouseIdVal = Convert.ToInt16(cmbWarehouse.SelectedValue ?? 0);
            var request = new CreateSalesReturnRequest(
                null,
                customerIdVal > 0 ? customerIdVal : null,
                warehouseIdVal,
                dtpDate.Value,
                null,
                txtNotes.Text,
                items);
            
            var result = await _returnApi.CreateAsync(request);
            if (result.IsSuccess) {
                if (post) {
                    var postRes = await _returnApi.PostAsync(result.Value.Id);
                    if (postRes.IsSuccess) { _notification.ShowSuccess("تم الترحيل"); foreach (var item in items) _eventBus.Publish(new StockChangedMessage(item.ProductId)); }
                    else _notification.ShowError(postRes.Error!);
                } else _notification.ShowSuccess("تم الحفظ");
                _eventBus.Publish(new SalesReturnChangedMessage(result.Value.Id));
                this.DialogResult = DialogResult.OK; this.Close();
            } else _notification.ShowError(result.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ مرتجع المبيعات");
            _notification.ShowError("خطأ في حفظ المرتجع. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            Cursor = Cursors.Default;
            btnSaveDraft.Enabled = btnPost.Enabled = true;
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();
}
