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

public partial class frmPurchaseReturn : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IPurchaseReturnDesktopService _returnApi => _serviceProvider.GetRequiredService<IPurchaseReturnDesktopService>();
    private ISupplierDesktopService _supplierApi => _serviceProvider.GetRequiredService<ISupplierDesktopService>();
    private IProductDesktopService _productApi => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private INotificationService _notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();

    private PurchaseReturnDto? _invoice;
    private BindingList<InvoiceLineItem> _lines = new();
    private List<ProductDto> _allProducts = new();
    private bool _isUpdating = false;

    public frmPurchaseReturn(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (4 من 6).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnAddItem, btnRemoveItem, btnSaveDraft, btnPost, btnClose);

        // RULE-N47 — الاختصارات الموحّدة على الأزرار الموجودة فعلاً في الشاشة.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnAddItem.PerformClick()),        // جديد = سطر صنف جديد
            (Keys.F3, () => btnSaveDraft.PerformClick()),      // حفظ/عرض المسودة
            (Keys.F4, () => btnPost.PerformClick()),           // ترحيل
            (Keys.F5, _RefreshAsync),                          // تحديث
            (Keys.Delete, _GlobalDelete),                      // حذف السطر المحدد
            (Keys.Escape, _GlobalEscape));                     // خروج

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي (يُقرأ عند كل ضغطة لا مرة واحدة).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;

        SetupGrid();
        ApplyUiTheme();   // ينتهي بالثيم الأخضر (RULE-N50)
    }

    public void LoadData(PurchaseReturnDto? invoice)
    {
        _invoice = invoice;
        if (_invoice != null) BindInvoice();
        else ResetForm();
        UpdateUIState();
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

    /// <summary>
    /// الزر الأساسي الذي ينفّذه <c>Enter</c> في الوضع الحالي (RULE-N147):
    /// مرتجع جديد أو مسودة قابلة للتحرير ⇒ «ترحيل» (ولا يُرحَّل شيء قبل تأكيد المستخدم في نافذة التأكيد)،
    /// والوضع المقروء (مرحّل/ملغي) ⇒ لا زر أساسي فلا يفعل Enter شيئاً.
    /// <para>
    /// ويُعاد <c>null</c> إلزامياً عندما يكون التركيز داخل شبكة الأصناف حتى يبقى <c>Enter</c> فيها
    /// انتقالاً بين السطور/إتمام تحرير الخلية ولا يُرحّل المرتجع أثناء إدخال الأصناف (RULE-N22a).
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        // حماية شبكة إدخال السطور — قبل أي اعتبار آخر.
        if (dgvItems.ContainsFocus) return null;

        // وضع العرض (مرتجع مرحّل أو ملغي): الشاشة للقراءة فقط ⇒ لا زر أساسي.
        if (_invoice != null && _invoice.Status != (byte)InvoiceStatus.Draft) return null;

        return btnPost.Enabled ? btnPost : null;
    }

    /// <summary>F5: إعادة تحميل القوائم وإعادة حساب الإجماليات (RULE-N47).</summary>
    private async void _RefreshAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            // نحفظ الاختيارات قبل إعادة الربط حتى لا تتغير بيانات المرتجع الحالي.
            int supplierId = Convert.ToInt32(cmbSupplier.SelectedValue ?? 0);
            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0);

            await LoadLookups();

            if (supplierId > 0) cmbSupplier.SelectedValue = supplierId;
            if (warehouseId > 0) cmbWarehouse.SelectedValue = warehouseId;

            CalculateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحديث بيانات مرتجع المشتريات");
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
            ? "(( مرتجع مشتريات جديد ))"
            : $"(( مرتجع مشتريات — {_invoice.ReturnNo} ))";
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
            await LoadLookups();
            if (_invoice == null) ResetForm(); // Default state
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
    }

    private async Task LoadLookups()
    {
        try
        {
            var warehouseRes = await _warehouseApi.GetAllAsync();
            if (warehouseRes.IsSuccess)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
            }

            var supplierRes = await _supplierApi.GetAllAsync();
            if (supplierRes.IsSuccess)
            {
                cmbSupplier.DataSource = supplierRes.Value;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.ValueMember = "Id";
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
        this.Text = $"مرتجع مشتريات - {_invoice!.ReturnNo}";
        lblInvoiceNo.Text = _invoice.ReturnNo.ToString();
        dtpDate.Value = _invoice.ReturnDate.ToDateTime(TimeOnly.MinValue);
        cmbSupplier.SelectedValue = _invoice.SupplierId;
        cmbWarehouse.SelectedValue = _invoice.WarehouseId;
        txtNotes.Text = _invoice.Notes;

        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItem {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitCost,
                DiscountAmount = 0m
            });
        }
        _isUpdating = false;
        CalculateTotals();

        _UpdateModeHeader();
    }

    private void ResetForm()
    {
        this.Text = "مرتجع مشتريات جديد";
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
        bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
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

        // RULE-N147 — الزر الافتراضي لـ Enter يتبع الوضع (مرحّل/ملغي ⇒ لا زر أساسي).
        AcceptButton = _PrimaryActionButton();

        _UpdateModeHeader();
    }

    private void btnAddItem_Click(object sender, EventArgs e) { _lines.Add(new InvoiceLineItem()); dgvItems.Focus(); }
    private void btnRemoveItem_Click(object sender, EventArgs e) { if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItem line) { _lines.Remove(line); CalculateTotals(); } }

    private async void btnSaveDraft_Click(object sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object sender, EventArgs e) => await SaveInvoiceAsync(true);

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbSupplier.SelectedValue == null || (int)cmbSupplier.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbSupplier, "يرجى اختيار المورد");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbSupplier, "");
        }

        if (cmbWarehouse.SelectedValue == null || (int)cmbWarehouse.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbWarehouse, "يرجى اختيار المستودع");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbWarehouse, "");
        }

        if (!_lines.Any(l => l.ProductId > 0))
        {
            errorProvider.SetError(dgvItems, "يجب إضافة صنف واحد على الأقل للإرجاع");
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

        if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم تقليل المخزون وتقليل رصيد المورد.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try {
            var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreatePurchaseReturnItemRequest(
                0, l.ProductId, 1, l.Quantity, l.UnitCost, l.LineTotal)).ToList();
            int supplierId = Convert.ToInt32(cmbSupplier.SelectedValue ?? 0);
            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0);
            var request = new CreatePurchaseReturnRequest(
                null,
                supplierId,
                warehouseId,
                dtpDate.Value,
                null,
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
                _eventBus.Publish(new PurchaseReturnChangedMessage(result.Value.Id));
                this.DialogResult = DialogResult.OK; this.Close();
            } else _notification.ShowError(result.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ مرتجع المشتريات");
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
