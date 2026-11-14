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
using SalesSystem.Desktop.Printing;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Purchases;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

public partial class frmPurchaseInvoice : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IPurchaseInvoiceDesktopService _purchaseApi => _serviceProvider.GetRequiredService<IPurchaseInvoiceDesktopService>();
    private ISupplierDesktopService _supplierApi => _serviceProvider.GetRequiredService<ISupplierDesktopService>();
    private IProductDesktopService _productApi => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private INotificationService _notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private IInvoicePrinter _invoicePrinter => _serviceProvider.GetRequiredService<IInvoicePrinter>();
    private ISettingsDesktopService _settingsApi => _serviceProvider.GetRequiredService<ISettingsDesktopService>();

    private PurchaseInvoiceDto? _invoice;
    private BindingList<InvoiceLineItem> _lines = new();
    private List<ProductDto> _allProducts = new();
    private bool _isUpdating = false;
    private bool _tafqeetAttached = false;

    public frmPurchaseInvoice(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (4 من 6).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(
            btnAddItem, btnRemoveItem, btnSaveDraft, btnPost, btnCancelInvoice, btnPrintA4, btnClose);

        // RULE-N47 — الاختصارات الموحّدة على الأزرار الموجودة فعلاً في الشاشة.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddItem.PerformClick()),        // جديد = سطر صنف جديد
            (Keys.F3, () => btnSaveDraft.PerformClick()),      // حفظ/عرض المسودة
            (Keys.F4, () => btnPost.PerformClick()),           // ترحيل
            (Keys.F5, _RefreshAsync),                          // تحديث
            (Keys.Delete, _GlobalDelete),                      // حذف السطر المحدد
            (Keys.Escape, _GlobalEscape));                     // خروج

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي (Enter داخل حقل = الحقل التالي — RULE-N40).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;

        SetupGrid();
        ApplyUiTheme();   // ينتهي بالثيم الأخضر (RULE-N50)
    }

    public void LoadData(PurchaseInvoiceDto? invoice)
    {
        _invoice = invoice;
        ApplyUiTheme();
        _UpdateModeHeader();
    }

    // ── الاختصارات العامة (RULE-N47) ─────────────────────────────────────────

    /// <summary>Delete: حذف السطر المحدد — يعمل فقط عندما تكون الشبكة هي صاحبة التركيز،
    /// ولا يحذف أثناء تحرير خلية داخلها (RULE-N22a — نفس حارس معالج Esc).</summary>
    private void _GlobalDelete()
    {
        if (dgvItems.IsCurrentCellInEditMode) return;

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

    /// <summary>F5: إعادة تحميل القوائم وإعادة حساب الإجماليات (RULE-N47).</summary>
    private async void _RefreshAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            // نحفظ الاختيارات قبل إعادة الربط حتى لا تتغير بيانات الفاتورة الحالية.
            int supplierId = Convert.ToInt32(cmbSupplier.SelectedValue ?? 0);
            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0);

            await LoadLookups();

            if (supplierId > 0) cmbSupplier.SelectedValue = supplierId;
            if (warehouseId > 0) cmbWarehouse.SelectedValue = warehouseId;

            CalculateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحديث بيانات فاتورة المشتريات");
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
            ? "(( فاتورة مشتريات جديدة ))"
            : $"(( فاتورة مشتريات — {_invoice.InvoiceNo} ))";

        _SyncAcceptButton();
    }

    /// <summary>
    /// الزر الأساسي الذي يستقبله <c>Enter</c> (RULE-N147):
    /// وضع الإدخال/المسودة ⇒ «حفظ مسودة» · وضع العرض (مرحّلة/ملغاة) ⇒ لا زر أساسي (بلا أي حفظ أو ترحيل).
    /// <para>
    /// حارس إلزامي: التركيز داخل شبكة السطور ⇒ <c>null</c>، فـ<c>Enter</c> هناك يبقى تنقّلاً/تحريراً
    /// داخل الشبكة ولا يُرحّل الفاتورة أبداً من الشبكة.
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return null;

        bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
        return editable ? btnSaveDraft : null;
    }

    /// <summary>
    /// يزامن <see cref="Form.AcceptButton"/> مع الزر الأساسي للوضع الحالي، فلا يظل Enter مرتبطاً بزر
    /// لم يعد صالحاً بعد تغيّر الوضع (RULE-N147).
    /// </summary>
    private void _SyncAcceptButton() => AcceptButton = _PrimaryActionButton();

    /// <summary>المظهر: ThemeHelper أولاً ثم الثيم الأخضر أخيراً (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        this.Text = _invoice == null ? "فاتورة مشتريات جديدة" : $"فاتورة مشتريات - {_invoice.InvoiceNo}";

        // الأزرار: ألوانها وخط Tahoma معلنة حرفياً في المصمّم بقيم مطابقة لثوابت الثيم
        // (RULE-N134 / RULE-N144) — فلا يُعاد تنسيقها وقت التشغيل، وتبقى حالة التعطيل
        // المرسومة بـ WireButtonDisabledVisual (RULE-N43) سليمة دون أن يمحوها الثيم.

        GreenThemeHelper.ApplyHeader(lblMode);   // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvItems);    // RULE-N51/N52 — لا يلمس ReadOnly/SelectionMode

        GreenThemeHelper.Apply(this);            // ← الأخير دائماً (RULE-N50)

        // RULE-N97 — التفقيط الحيّ على حقول المال (مرة واحدة لكل نموذج).
        if (!_tafqeetAttached)
        {
            _tafqeetAttached = true;
            LiveTafqeetHelper.Attach(toolTip,
                (txtPaid, "المبلغ المدفوع", LiveTafqeetHelper.ReadMode.Money),
                (numInvoiceDiscount, "خصم الفاتورة", LiveTafqeetHelper.ReadMode.Money));
        }
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            await LoadLookups();

            if (_invoice != null) BindInvoice();
            else ResetForm();

            UpdateUIState();
            CalculateTotals();
            ApplyUiTheme();

            // RULE-N22a — الشبكة شبكة إدخال: التحرير والصف الجديد مملوكان للشاشة (RULE-N52).
            dgvItems.AllowUserToAddRows = true;
            dgvItems.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل النموذج");
            clsDialogs.Error("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.");
        }
    }

    private async Task LoadLookups()
    {
        try
        {
            var supplierRes = await _supplierApi.GetAllAsync();
            if (supplierRes.IsSuccess)
            {
                cmbSupplier.DataSource = supplierRes.Value;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.ValueMember = "Id";
            }

            var warehouseRes = await _warehouseApi.GetAllAsync();
            if (warehouseRes.IsSuccess)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
                if (_invoice == null)
                {
                    var def = warehouseRes.Value!.FirstOrDefault(w => w.IsDefault);
                    if (def != null) cmbWarehouse.SelectedValue = def.Id;
                }
            }

            var productRes = await _productApi.GetAllAsync();
            if (productRes.IsSuccess) _allProducts = productRes.Value!.ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل البيانات");
            clsDialogs.Error("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.");
        }
    }

    private void BindInvoice()
    {
        _isUpdating = true;
        this.Text = $"فاتورة مشتريات - {_invoice!.InvoiceNo}";
        lblInvoiceNo.Text = _invoice.InvoiceNo.ToString();
        dtpDate.Value = _invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue);
        cmbSupplier.SelectedValue = _invoice.SupplierId;
        cmbWarehouse.SelectedValue = _invoice.WarehouseId;
        cmbPaymentType.SelectedIndex = (byte)_invoice.PaymentType - 1;
        numInvoiceDiscount.Value = _invoice.DiscountAmount;
        txtPaid.Text = clsFormat.Money(_invoice.PaidAmount);
        txtNotes.Text = _invoice.Notes;

        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItem {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                UnitPrice = item.UnitCost,      // عرض فقط: تُبقي LineTotal صحيحة عند فتح فاتورة محفوظة
                DiscountAmount = item.DiscountAmount
            });
        }
        _isUpdating = false;

        _UpdateModeHeader();
    }

    private void ResetForm()
    {
        this.Text = "فاتورة مشتريات جديدة";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        cmbPaymentType.SelectedIndex = 0;
        _lines.Clear();
        numInvoiceDiscount.Value = 0;
        txtPaid.Text = "0";
        txtNotes.Clear();

        _UpdateModeHeader();
    }

    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Clear();
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 250, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCost", HeaderText = "التكلفة", DataPropertyName = "UnitCost", Width = 100 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDisc", HeaderText = "خصم", DataPropertyName = "DiscountAmount", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 120, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", ForeColor = GreenThemeHelper.TextStrong, Font = new Font(dgvItems.Font, FontStyle.Bold) } });

        dgvItems.DataSource = _lines;
        dgvItems.CellEndEdit += DgvItems_CellEndEdit;
    }

    // ── أحداث الحقول (كما كانت مربوطة في المصمّم) ────────────────────────────

    private void numInvoiceDiscount_ValueChanged(object? sender, EventArgs e) => CalculateTotals();
    private void txtPaid_TextChanged(object? sender, EventArgs e) => CalculateTotals();

    private void DgvItems_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isUpdating || e.RowIndex < 0) return;
        var row = _lines[e.RowIndex];
        var colName = dgvItems.Columns[e.ColumnIndex].Name;

        if (colName == "ColCode")
        {
            var product = _allProducts.FirstOrDefault(p => p.Barcode == row.ProductCode);
            if (product != null)
            {
                row.ProductId = product.Id;
                row.ProductName = product.Name;
                row.UnitCost = product.PurchasePrice;
                row.UnitPrice = row.UnitCost;   // عرض فقط: LineTotal محسوبة من UnitPrice (لا تغيير في الطلب المحفوظ)
                if (row.Quantity == 0) row.Quantity = 1;
            }
        }
        else if (colName == "ColCost")
        {
            // تحرير عمود التكلفة: مزامنة حقل السعر لعرض LineTotal صحيحة (عرض فقط).
            row.UnitPrice = row.UnitCost;
        }

        CalculateTotals();
        dgvItems.Refresh();
    }

    private void CalculateTotals()
    {
        if (_isUpdating) return;
        decimal subTotal = _lines.Sum(x => x.LineTotal);
        decimal invoiceDiscount = numInvoiceDiscount.Value;
        decimal paidAmount = txtPaid.DecimalValue;

        // قرار المالك: MVP V1 بلا ضريبة على مستندات الشراء — الإجمالي = الفرعي - خصم الفاتورة.
        decimal totalAmount = subTotal - invoiceDiscount;
        decimal dueAmount = totalAmount - paidAmount;

        lblSubTotalVal.Text = clsFormat.Money(subTotal);
        lblTotalVal.Text = clsFormat.Money(totalAmount);
        lblDueVal.Text = clsFormat.Money(dueAmount);
        lblDueVal.ForeColor = dueAmount > 0 ? Color.Red : Color.Green;
    }

    private void UpdateUIState()
    {
        bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
        pnlHeader.Enabled = editable;
        dgvItems.ReadOnly = !editable;
        pnlFooter.Enabled = editable;
        btnAddItem.Enabled = editable;
        btnRemoveItem.Enabled = editable;
        btnSaveDraft.Visible = editable;
        btnPost.Visible = editable;
        btnCancelInvoice.Visible = _invoice != null && _invoice.Status == (byte)InvoiceStatus.Posted;

        lblStatus.Text = _invoice?.Status switch { 1 => "مسودة", 2 => "مرحل", 3 => "ملغي", _ => "جديد" };

        // ألوان الحالة من ثوابت ThemeHelper (لا أرقام لون داخل الشاشة) — مطابقة لقيمة المصمّم.
        lblStatus.BackColor = _invoice?.Status switch
        {
            1 => ThemeHelper.Neutral,
            2 => ThemeHelper.Success,
            3 => ThemeHelper.Danger,
            _ => ThemeHelper.Primary
        };

        // RULE-N147 — الزر الذي يستقبله Enter يتبع الوضع الحالي (بلا حفظ/ترحيل من شبكة السطور).
        _SyncAcceptButton();
    }

    private void btnAddItem_Click(object? sender, EventArgs e)
    {
        _lines.Add(new InvoiceLineItem());
        dgvItems.Focus();
        dgvItems.CurrentCell = dgvItems.Rows[_lines.Count - 1].Cells[0];
    }

    private void btnRemoveItem_Click(object? sender, EventArgs e)
    {
        if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItem line)
        {
            _lines.Remove(line);
            CalculateTotals();
        }
    }

    private async void btnSaveDraft_Click(object? sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object? sender, EventArgs e) => await SaveInvoiceAsync(true);

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbSupplier.SelectedValue == null || (int)cmbSupplier.SelectedValue <= 0)
        {
            const string supplierMessage = "يرجى اختيار المورد";
            errorProvider.SetError(cmbSupplier, supplierMessage);
            // GAP-08b — كل رفض errorProvider مقرون بتغذية بصرية من البيت الواحد (RULE-N169).
            clsInputFeedback.ShowRejectTooltip(cmbSupplier, supplierMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbSupplier, "");
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
            const string itemsMessage = "يجب إضافة صنف واحد على الأقل للفاتورة";
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
        if (!_ValidateForm()) return;

        if (post && !clsDialogs.Confirm("هل أنت متأكد من ترحيل فاتورة المشتريات؟ سيتم زيادة المخزون.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        try
        {
            var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreatePurchaseInvoiceLineRequest(l.ProductId, 1, l.Quantity, l.UnitCost)).ToList();
            var request = new CreatePurchaseInvoiceRequest(
                (int)(cmbWarehouse.SelectedValue ?? 0),
                (int)(cmbSupplier.SelectedValue ?? 0),
                null,
                dtpDate.Value,
                (SalesSystem.DTO.Enums.PaymentType)(cmbPaymentType.SelectedIndex + 1),
                numInvoiceDiscount.Value,
                null,
                null,
                0m,
                0m,
                txtPaid.DecimalValue,
                txtNotes.Text,
                null,
                null,
                items
            );

            var result = await _purchaseApi.CreateAsync(request);
            if (result.IsSuccess)
            {
                if (post)
                {
                    var postRes = await _purchaseApi.PostAsync(result.Value.Id);
                    if (postRes.IsSuccess)
                    {
                        _notification.ShowSuccess("تم الترحيل بنجاح");
                        foreach (var item in items) _eventBus.Publish(new StockChangedMessage(item.ProductId));
                    }
                    else _notification.ShowError("تم الحفظ ولكن فشل الترحيل: " + postRes.Error);
                }
                else _notification.ShowSuccess("تم الحفظ بنجاح");

                _eventBus.Publish(new PurchaseInvoiceChangedMessage(result.Value.Id));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else _notification.ShowError(result.Error!);
        }
        catch (Exception ex) 
        { 
            Log.Error(ex, "حدث خطأ أثناء حفظ فاتورة المشتريات");
            _notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني."); 
        }
        finally { btnSaveDraft.Enabled = btnPost.Enabled = true; }
    }

    private async void btnCancelInvoice_Click(object? sender, EventArgs e)
    {
        if (_invoice == null) return;
        if (!clsDialogs.Confirm("هل أنت متأكد من إلغاء الفاتورة؟ سيتم عكس المخزون.")) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            var result = await _purchaseApi.CancelAsync(_invoice.Id);
            if (result.IsSuccess)
            {
                _notification.ShowSuccess("تم الإلغاء بنجاح");
                _eventBus.Publish(new PurchaseInvoiceChangedMessage(_invoice.Id));
                foreach (var item in _invoice.Items) _eventBus.Publish(new StockChangedMessage(item.ProductId));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else _notification.ShowError(result.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء إلغاء فاتورة المشتريات");
            _notification.ShowError("حدث خطأ أثناء الإلغاء. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnPrintA4_Click(object? sender, EventArgs e)
    {
        if (_invoice == null)
        {
            clsDialogs.Info("احفظ الفاتورة أولاً قبل الطباعة.");
            return;
        }

        try
        {
            Cursor = Cursors.WaitCursor;
            var settingsRes = await _settingsApi.GetSettingsAsync();
            if (!settingsRes.IsSuccess)
            {
                _notification.ShowError("فشل تحميل إعدادات المتجر: " + settingsRes.Error);
                return;
            }

            var storeInfo = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(settingsRes.Value);
            var printInvoice = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(_invoice);
            var printItems = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDtos(_invoice.Items);
            var printTotals = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToTotalsPrintDto(_invoice);

            _invoicePrinter.PrintPreview(printInvoice, printItems, printTotals, storeInfo);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء طباعة فاتورة المشتريات A4");
            _notification.ShowError("حدث خطأ أثناء الطباعة. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => this.Close();
}
