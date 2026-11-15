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
    // RULE-N169: بيت الرسائل الواحد هو clsDialogs — لا خدمة إشعارات ثانية داخل الشاشة.
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private IInvoicePrinter _invoicePrinter => _serviceProvider.GetRequiredService<IInvoicePrinter>();
    private ISettingsDesktopService _settingsApi => _serviceProvider.GetRequiredService<ISettingsDesktopService>();

    /// <summary>معرّف المورد المحدد (0 إذا لم يُحدَّد) — <c>SupplierDto.Id</c> من نوع <c>int</c>.</summary>
    private int _SelectedSupplierId => cmbSupplier.SelectedValue is int supplierId ? supplierId : 0;

    /// <summary>
    /// معرّف المستودع المحدد (0 إذا لم يُحدَّد).
    /// <para>
    /// <c>WarehouseDto.Id</c> من نوع <c>short</c>، فالتحويل الصريح <c>(int)cmbWarehouse.SelectedValue</c>
    /// يرمي <c>InvalidCastException</c> (فكّ صندوق <c>short</c> إلى <c>int</c> غير صالح) فيسقط الحفظ كله؛
    /// والقراءة هنا بالنمط <c>is short</c> كما في بقية الشاشات (<c>frmAddUpdateSalesInvoice</c> · <c>frmOpeningStock</c>).
    /// </para>
    /// </summary>
    private int _SelectedWarehouseId => cmbWarehouse.SelectedValue is short warehouseId ? warehouseId : 0;

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
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
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
            int supplierId = _SelectedSupplierId;
            int warehouseId = _SelectedWarehouseId;

            await LoadLookups();

            if (supplierId > 0) cmbSupplier.SelectedValue = supplierId;
            // WarehouseDto.Id من نوع short — الإسناد بقيمة int لا يطابق أي عنصر فيضيع المستودع بعد التحديث.
            if (warehouseId > 0) cmbWarehouse.SelectedValue = Convert.ToInt16(warehouseId);

            CalculateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحديث بيانات فاتورة المشتريات");
            clsDialogs.Error("تعذر تحديث البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
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
            Cursor = Cursors.WaitCursor;
            await LoadLookups();

            if (_invoice is not null)
            {
                // المستند الكامل يُقرأ بالمعرّف: شاشة القائمة قد تُمرّر رأس الفاتورة بلا سطور، وفتح مسودة
                // بلا سطور يجعل إعادة الحفظ تُسقط أصنافها (فقد بيانات)، وكذلك تُقرأ الحالة الحقيقية
                // فلا يُعدَّل مستند رُحّل من شاشة أخرى.
                var full = await _purchaseApi.GetByIdAsync(_invoice.Id);
                if (full.IsSuccess && full.Value is not null) _invoice = full.Value;
                else clsDialogs.Error(full.Error ?? "تعذر تحميل تفاصيل الفاتورة.");

                BindInvoice();
            }
            else ResetForm();

            UpdateUIState();
            CalculateTotals();
            ApplyUiTheme();

            // RULE-N22a — الشبكة شبكة إدخال: صف الإدخال الجديد يُتاح في وضع التحرير فقط
            // (UpdateUIState يضبطه حسب الحالة) والتحرير داخل الخلية مملوك للشاشة (RULE-N52).
            dgvItems.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل النموذج");
            clsDialogs.Error("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private async Task LoadLookups()
    {
        try
        {
            var supplierRes = await _supplierApi.GetAllAsync();
            if (supplierRes.IsSuccess && supplierRes.Value is not null)
            {
                cmbSupplier.DataSource = supplierRes.Value;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.ValueMember = "Id";
            }
            else
            {
                // نتيجة فاشلة لا تُهمَل: قائمة فارغة بلا سبب تُظهر للمستخدم «يرجى اختيار المورد» بلا تفسير.
                clsDialogs.Error(supplierRes.Error ?? "تعذر تحميل قائمة الموردين.");
            }

            var warehouseRes = await _warehouseApi.GetAllAsync();
            if (warehouseRes.IsSuccess && warehouseRes.Value is not null)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
                if (_invoice == null)
                {
                    var def = warehouseRes.Value.FirstOrDefault(w => w.IsDefault);
                    if (def != null) cmbWarehouse.SelectedValue = def.Id;
                }
            }
            else
            {
                clsDialogs.Error(warehouseRes.Error ?? "تعذر تحميل قائمة المستودعات.");
            }

            var productRes = await _productApi.GetAllAsync();
            if (productRes.IsSuccess && productRes.Value is not null) _allProducts = productRes.Value.ToList();
            else clsDialogs.Error(productRes.Error ?? "تعذر تحميل قائمة الأصناف.");
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
        try
        {
            this.Text = $"فاتورة مشتريات - {_invoice!.InvoiceNo}";
            lblInvoiceNo.Text = _invoice.InvoiceNo.ToString();
            dtpDate.Value = _invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue);
            cmbSupplier.SelectedValue = _invoice.SupplierId;
            // WarehouseDto.Id من نوع short — الإسناد بقيمة int لا يطابق أي عنصر فيُفرغ المستودع المحفوظ.
            cmbWarehouse.SelectedValue = Convert.ToInt16(_invoice.WarehouseId);
            cmbPaymentType.SelectedIndex = (byte)_invoice.PaymentType - 1;

            // سقف الحقل من المصمّم لا يكفي لخصم أكبر: نوسّعه قبل الإسناد بدل رمي ArgumentOutOfRangeException
            // أو قصّ الخصم بصمت (المعروض = المحفوظ).
            if (numInvoiceDiscount.Maximum < _invoice.DiscountAmount)
                numInvoiceDiscount.Maximum = _invoice.DiscountAmount;
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
                    // الوحدة المحفوظة على السطر تُحمَل لتعود كما هي عند إعادة الحفظ (لا تُستبدل بالوحدة الأساسية).
                    ProductUnitId = item.ProductUnitId,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitPrice,
                    UnitPrice = item.UnitPrice,    // ما يُرسل عند الحفظ هو سعر الوحدة — فيُعرض هو لا التكلفة المستقرّة
                    DiscountAmount = item.DiscountAmount
                });
            }
        }
        finally
        {
            _isUpdating = false;
        }

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
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 120, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { ForeColor = GreenThemeHelper.TextStrong, Font = new Font(dgvItems.Font, FontStyle.Bold) } });

        // RULE-N169: الدلالة تُعلَن (مبلغ مالي) والنمط يأتي من clsFormat الحيّ عبر البيت الواحد — لا "N2" مكتوبة هنا.
        GridFormatHelper.ApplyColumns(dgvItems, GridFormatKind.Money, "ColTotal");

        dgvItems.DataSource = _lines;
        dgvItems.CellEndEdit += DgvItems_CellEndEdit;

        // RULE-N153 — Enter داخل شبكة السطور تنقّل/تحرير ولا ينفّذ الزر الأساسي أبداً:
        // AcceptButton يُفرَّغ عند دخول الشبكة أو بدء تحرير خلية ويُعاد عند مغادرتها.
        dgvItems.Enter += (_, _) => _SyncAcceptButton();
        dgvItems.Leave += (_, _) => _SyncAcceptButton();
        dgvItems.CellBeginEdit += (_, _) => _SyncAcceptButton();
    }

    // ── أحداث الحقول (كما كانت مربوطة في المصمّم) ────────────────────────────

    private void numInvoiceDiscount_ValueChanged(object? sender, EventArgs e) => CalculateTotals();
    private void txtPaid_TextChanged(object? sender, EventArgs e) => CalculateTotals();

    private void DgvItems_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        // RULE-N153: بعد نهاية التحرير يعود Enter لمتابعة تنقّل الشبكة لا لتنفيذ الزر الأساسي.
        _SyncAcceptButton();

        if (_isUpdating || e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (e.RowIndex >= _lines.Count) return;   // صف الإدخال الجديد قبل إضافته للقائمة المربوطة

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
        lblDueVal.ForeColor = dueAmount > 0 ? GreenThemeHelper.StatusNegative : GreenThemeHelper.StatusPositive;
    }

    private void UpdateUIState()
    {
        bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
        pnlHeader.Enabled = editable;
        dgvItems.ReadOnly = !editable;
        // RULE-N22a: صف الإدخال الجديد يظهر في وضع التحرير فقط، فلا يُعرض صف فارغ على مستند مرحّل/ملغى.
        dgvItems.AllowUserToAddRows = editable;
        pnlFooter.Enabled = editable;
        btnAddItem.Enabled = editable;
        btnRemoveItem.Enabled = editable;
        // الإخفاء وحده لا يمنع الاختصارات (F3/F4) — التعطيل إلزامي (RULE-N43/N152)، ويُفعَّل فقط في وضع التحرير.
        btnSaveDraft.Visible = btnSaveDraft.Enabled = editable;
        btnPost.Visible = btnPost.Enabled = editable;
        btnCancelInvoice.Visible = btnCancelInvoice.Enabled = _invoice != null && _invoice.Status == (byte)InvoiceStatus.Posted;

        // RULE-N171/N172: نص الحالة من البيت الواحد DocumentTextHelper (1..3 — RULE-019)؛
        // و«جديد» حين لا يوجد مستند بعد.
        lblStatus.Text = _invoice is null
            ? "جديد"
            : DocumentTextHelper.StatusText(_invoice.Status);

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
        // RULE-N22a: قيمة الخلية الجاري تحريرها تُثبَّت أولاً، فلا تبقى كمية/تكلفة مكتوبة خارج ما يُرسل.
        dgvItems.EndEdit();
        dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);

        bool isValid = true;

        if (_SelectedSupplierId <= 0)
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

        if (_SelectedWarehouseId <= 0)
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

        // كل سطر ملموس يجب أن يشير إلى صنف وكمية وتكلفة صحيحة — لا سطر يُعرض ويُسقط بصمت من الطلب.
        string? lineError = _FirstLineError();
        if (lineError is not null)
        {
            errorProvider.SetError(dgvItems, lineError);
            clsInputFeedback.ShowRejectTooltip(dgvItems, lineError, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvItems, "");
        }

        // الحساب من **نفس** مجموعة الأسطر المرسلة (ProductId > 0) — فالمعروض في الشاشة = المحفوظ.
        decimal subTotal = _lines.Where(l => l.ProductId > 0).Sum(l => l.LineTotal);
        decimal invoiceDiscount = numInvoiceDiscount.Value;
        decimal netTotal = subTotal - invoiceDiscount;

        if (invoiceDiscount > subTotal)
        {
            const string discountMessage = "خصم الفاتورة أكبر من مجموع الأصناف.";
            errorProvider.SetError(numInvoiceDiscount, discountMessage);
            clsInputFeedback.ShowRejectTooltip(numInvoiceDiscount, discountMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numInvoiceDiscount, "");
        }

        if (txtPaid.DecimalValue > netTotal)
        {
            const string paidMessage = "المبلغ المدفوع أكبر من إجمالي الفاتورة.";
            errorProvider.SetError(txtPaid, paidMessage);
            clsInputFeedback.ShowRejectTooltip(txtPaid, paidMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtPaid, "");
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    /// <summary>
    /// أول خطأ في سطور الأصناف أو <c>null</c> — رسالة واحدة محددة لكل حالة
    /// (تكافؤ الرفض: لا صنف مجهول · لا كمية صفر · لا تكلفة صفر — <c>RULE-N167</c>).
    /// </summary>
    private string? _FirstLineError()
    {
        if (_lines.All(l => l.ProductId <= 0))
            return "يجب إضافة صنف واحد على الأقل للفاتورة";

        for (int i = 0; i < _lines.Count; i++)
        {
            var line = _lines[i];
            int rowNo = i + 1;

            // صف الإدخال الجديد الفارغ تماماً لا يمنع الحفظ ولا يُرسل.
            bool touched = line.ProductId > 0
                           || line.Quantity != 0m
                           || line.UnitCost != 0m
                           || line.DiscountAmount != 0m
                           || !string.IsNullOrWhiteSpace(line.ProductCode);
            if (!touched) continue;

            if (line.ProductId <= 0)
                return $"السطر {rowNo}: الصنف غير محدد — ابحث عن الصنف بالكود أو احذف السطر.";

            if (line.Quantity <= 0m)
                return $"السطر {rowNo}: الكمية يجب أن تكون أكبر من صفر.";

            if (line.UnitCost <= 0m)
                return $"السطر {rowNo}: التكلفة يجب أن تكون أكبر من صفر.";
        }

        return null;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        // حارس المسودة: فاتورة غير مسودة (مرحّلة/ملغاة) لا تُحفظ ولا تُرحّل من أي مسار — زر مخفي
        // لا يمنع اختصار F3/F4، وهذا الحارس يقطعه قبل أي تحقق أو طلب للخدمة.
        if (_invoice is not null && _invoice.Status != (byte)InvoiceStatus.Draft)
        {
            clsDialogs.Warning("لا يمكن تعديل أو إعادة حفظ فاتورة غير مسودة.");
            return;
        }

        if (!_ValidateForm()) return;

        if (post && !clsDialogs.Confirm("هل أنت متأكد من ترحيل فاتورة المشتريات؟ سيتم زيادة المخزون.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try
        {
            // خصم السطر مبلغٌ ثابت: DiscountType.Amount + DiscountRate = المبلغ نفسه (بلا أي حساب)،
            // فيتساوى المحفوظ مع ما يعرضه اللصق والشبكة. الأسطر بلا خصم تبقى على الافتراضيات (null/null).
            // والوحدة المحفوظة على السطر تُعاد كما هي؛ السطر الجديد بلا وحدة يأخذ الوحدة الأساسية (1).
            var items = _lines
                .Where(l => l.ProductId > 0)
                .Select(l => l.DiscountAmount > 0m
                    ? new CreatePurchaseInvoiceLineRequest(
                        l.ProductId, l.ProductUnitId > 0 ? l.ProductUnitId : 1, l.Quantity, l.UnitCost,
                        SalesSystem.DTO.Enums.DiscountType.Amount, l.DiscountAmount)
                    : new CreatePurchaseInvoiceLineRequest(
                        l.ProductId, l.ProductUnitId > 0 ? l.ProductUnitId : 1, l.Quantity, l.UnitCost))
                .ToList();
            var request = new CreatePurchaseInvoiceRequest(
                _SelectedWarehouseId,
                _SelectedSupplierId,
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

            // فاتورة جديدة ⇒ إنشاء · فاتورة محفوظة (مسودة) ⇒ تعديل نفس السجل فلا تُنشأ نسخة ثانية.
            int? existingId = _invoice?.Id;
            Result<PurchaseInvoiceDto> result;
            if (existingId.HasValue)
                result = await _purchaseApi.UpdateAsync(existingId.Value, request);
            else
                result = await _purchaseApi.CreateAsync(request);

            if (result.IsSuccess)
            {
                // المعرّف من التحديث أو من الخدمة عند الإنشاء — بلا فكّ مرجعي أعمى لقيمة قد تكون null.
                int invoiceId = existingId ?? result.Value?.Id ?? 0;

                if (post && invoiceId > 0)
                {
                    var postRes = await _purchaseApi.PostAsync(invoiceId);
                    if (postRes.IsSuccess)
                    {
                        clsDialogs.Success("تم الترحيل بنجاح");
                        foreach (var item in items) _eventBus.Publish(new StockChangedMessage(item.ProductId));
                    }
                    else clsDialogs.Error("تم الحفظ ولكن فشل الترحيل: " + (postRes.Error ?? "خطأ غير معروف"));
                }
                else if (post)
                {
                    clsDialogs.Warning("تم الحفظ كمسودة، لكن تعذر ترحيلها لأن الخدمة لم تُرجع رقم الفاتورة.");
                }
                else clsDialogs.Success("تم الحفظ بنجاح");

                _eventBus.Publish(new PurchaseInvoiceChangedMessage(invoiceId));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else clsDialogs.Error(result.Error);
        }
        catch (Exception ex) 
        { 
            Log.Error(ex, "حدث خطأ أثناء حفظ فاتورة المشتريات");
            clsDialogs.Error("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني."); 
        }
        finally
        {
            // استعادة الحالة حسب الوضع: المسودة/الجديدة فقط تُعيد تفعيل أزرار الحفظ والترحيل.
            bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
            btnSaveDraft.Enabled = btnPost.Enabled = editable;
            if (!IsDisposed) Cursor = Cursors.Default;
        }
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
                clsDialogs.Success("تم الإلغاء بنجاح");
                _eventBus.Publish(new PurchaseInvoiceChangedMessage(_invoice.Id));
                foreach (var item in _invoice.Items) _eventBus.Publish(new StockChangedMessage(item.ProductId));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else clsDialogs.Error(result.Error);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء إلغاء فاتورة المشتريات");
            clsDialogs.Error("حدث خطأ أثناء الإلغاء. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
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
                clsDialogs.Error("فشل تحميل إعدادات المتجر: " + settingsRes.Error);
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
            clsDialogs.Error("حدث خطأ أثناء الطباعة. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => this.Close();
}
