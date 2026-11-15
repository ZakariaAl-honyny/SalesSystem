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
    // RULE-N169: بيت الرسائل الواحد هو clsDialogs — لا خدمة إشعارات ثانية داخل الشاشة.
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();

    private PurchaseReturnDto? _invoice;
    private BindingList<InvoiceLineItem> _lines = new();
    private List<ProductDto> _allProducts = new();
    private bool _isUpdating = false;

    /// <summary>
    /// معرّف المرتجع الذي أنشأته الشاشة فعلاً في محاولة سابقة. يمنع إنشاء مستند ثانٍ
    /// عندما ينجح الإنشاء ويفشل الترحيل ثم يعيد المستخدم المحاولة (F4/«ترحيل»)،
    /// فيُرحَّل المستند نفسه بدل تكرار المستند المالي. يُصفَّر في <see cref="ResetForm"/>.
    /// </summary>
    private int? _createdId;

    public frmPurchaseReturn(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (4 من 6).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
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
        // لا يُربط المرتجع بالقوائم هنا: تُستدعى LoadData قبل Show() فلم تُملأ ComboBox بعد،
        // فيُسند المورد/المستودع الخطأ ثم يُحفظ المرتجع بجهة غير جهته. الربط في OnLoad بعد LoadLookups.
        _invoice = invoice;
        if (_invoice == null) ResetForm();
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

        return btnPost is { Visible: true, Enabled: true } ? btnPost : null;
    }

    /// <summary>
    /// F3 = حفظ المسودة — لا يُفعَّل إلا من زر ظاهر ومفعَّل (RULE-N47).
    /// عند عرض مرتجع مرحَّل/ملغي يُخفى الزر، وButton.PerformClick() يفحص Enabled لا Visible
    /// ⇒ كان F3/F4 يبنيان طلباً جديداً من سطور مرتجع مرحَّل (تكرار أثر مخزني/مالي).
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
            // نوع القيمة المحفوظة = نوع خاصية ValueMember بالضبط (المستودع short): إسناد int إلى
            // قائمة قيمها short لا يطابق شيئاً فيُفرَّغ الاختيار — كان F5 يمسح المستودع المختار.
            int supplierId = Convert.ToInt32(cmbSupplier.SelectedValue ?? 0);
            short warehouseId = Convert.ToInt16(cmbWarehouse.SelectedValue ?? 0);

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

            // الترتيب إلزامي: ربط المرتجع بعد اكتمال القوائم فقط — وإلا تُسند ComboBox الفارغة
            // المورد/المستودع الخطأ فيُحفظ المرتجع على جهة غير جهته.
            if (_invoice == null) ResetForm(); // Default state
            else BindInvoice();
            ApplyUiTheme();

            // RULE-N22a — الشبكة شبكة إدخال: التحرير والصف الجديد مملوكان للشاشة (RULE-N52).
            // والتحرير للمرتجع الجديد فقط: العقد لا يحوي UpdateAsync، فتحرير مستند محفوظ يُعرض ولا يُرسَل.
            dgvItems.AllowUserToAddRows = _IsEditable();
            dgvItems.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            dgvItems.MultiSelect = true;
            dgvItems.ReadOnly = !_IsEditable();
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
            // includeInactive: مرتجع قديم لمورد/مستودع مُعطَّل يجب أن يُربط بجهته الصحيحة،
            // وإلا بقي الاختيار على أول عنصر فحُفظ على جهة غير جهته.
            var warehouseRes = await _warehouseApi.GetAllAsync(includeInactive: true);
            if (warehouseRes.IsSuccess)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
            }

            var supplierRes = await _supplierApi.GetAllAsync(includeInactive: true);
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

        // المرتجع المحمَّل مستند قائم فعلاً: معرّفه يمنع إنشاء نسخة ثانية عند «حفظ/ترحيل»،
        // فكان فتح مسودة من القائمة ثم ترحيلها يُنشئ مرتجعاً جديداً ويترك المسودة القديمة معلّقة.
        _createdId = _invoice!.Id;

        this.Text = $"مرتجع مشتريات - {_invoice.ReturnNo}";
        lblInvoiceNo.Text = _invoice.ReturnNo.ToString();
        dtpDate.Value = _invoice.ReturnDate.ToDateTime(TimeOnly.MinValue);
        cmbSupplier.SelectedValue = _invoice.SupplierId;
        // WarehouseDto.Id من نوع short ⇒ الإسناد بـ short وإلا لم تُطابق أي عنصر وبقي المستودع بلا اختيار.
        cmbWarehouse.SelectedValue = (short)_invoice.WarehouseId;
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
        // مرتجع جديد ⇒ لا مستند مُنشأ سابقاً يُعاد ترحيله (RULE: لا تكرار للمستند).
        _createdId = null;
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
            if (_isUpdating || e.RowIndex < 0 || e.RowIndex >= dgvItems.Rows.Count) return;

            // السطر يُقرأ من DataBoundItem لا بفهرس القائمة: صف الإدخال الجديد في الشبكة قد يكون
            // فهرسه خارج حدود _lines فيرمي ArgumentOutOfRangeException عند أول إدخال.
            if (dgvItems.Rows[e.RowIndex].DataBoundItem is not InvoiceLineItem row) return;

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
        // الإجمالي المعروض = مجموع السطور التي ستُرسَل فعلاً (ProductId > 0)؛ كان يجمع السطور
        // غير المرتبطة بصنف وهي تُسقط من الطلب ⇒ إجمالي على الشاشة يخالف ما يُحفظ (RULE-N167).
        decimal total = _lines.Where(x => x.ProductId > 0).Sum(x => x.LineTotal);
        lblTotalVal.Text = clsFormat.Money(total);
    }

    private void UpdateUIState()
    {
        // المحرر والسطور للمرتجع الجديد فقط (لا UpdateAsync في العقد ⇒ تحرير محفوظ لا يُرسَل).
        bool editable = _IsEditable();
        pnlHeader.Enabled = editable;
        dgvItems.ReadOnly = !editable;
        dgvItems.AllowUserToAddRows = editable;
        pnlFooter.Enabled = editable;
        btnAddItem.Enabled = editable;
        btnRemoveItem.Enabled = editable;
        btnSaveDraft.Visible = editable;
        btnSaveDraft.Enabled = editable;

        // «ترحيل» يبقى متاحاً للمسودة المحفوظة التي لم تُرحَّل: إعادة المحاولة تُرحّل نفس المعرّف (_createdId).
        // ولا يُترك الزر مفعّلاً وهو مخفي: Button.PerformClick() يفحص Enabled ولا يفحص Visible.
        bool postable = _IsPostable();
        btnPost.Visible = postable;
        btnPost.Enabled = postable;
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
            _ => ThemeHelper.Danger
        };

        // RULE-N147 — الزر الافتراضي لـ Enter يتبع الوضع (مرحّل/ملغي ⇒ لا زر أساسي).
        AcceptButton = _PrimaryActionButton();

        _UpdateModeHeader();
    }

    /// <summary>
    /// هل بيانات المرتجع قابلة للتحرير؟ للجديد فقط: العقد لا يحوي <c>UpdateAsync</c>، فتحرير مستند
    /// محفوظ (مسودة أو مرحَّل/ملغي) لا يمكن إرساله ⇒ يُقفل بدل أن يُعرض ولا يُحفظ.
    /// </summary>
    private bool _IsEditable() => _invoice == null;

    /// <summary>هل يمكن ترحيل المرتجع؟ الجديد يُنشأ ثم يُرحَّل، والمسودة المحفوظة تُرحَّل بمعرّفها.</summary>
    private bool _IsPostable() => _invoice?.Status is null or (byte)InvoiceStatus.Draft;

    private void btnAddItem_Click(object sender, EventArgs e) { _lines.Add(new InvoiceLineItem()); dgvItems.Focus(); }
    private void btnRemoveItem_Click(object sender, EventArgs e) { if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItem line) { _lines.Remove(line); CalculateTotals(); } }

    private async void btnSaveDraft_Click(object sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object sender, EventArgs e) => await SaveInvoiceAsync(true);

    private bool _ValidateForm()
    {
        bool isValid = true;

        // القراءة عبر Convert لا بالتحويل الصريح: SelectedValue قيمة مُعلَّبة (boxed) بنوع خاصية
        // المورد/المستودع، والمستودع Id من نوع short ⇒ (int) يرمي InvalidCastException.
        if (Convert.ToInt32(cmbSupplier.SelectedValue ?? 0) <= 0)
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

        if (Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0) <= 0)
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

        // تحقق السطور: كمية > 0 وسعر وحدة غير سالب لكل سطر مُسنَد لصنف.
        // بلا هذا الفحص يُرسَل سطر بكمية صفر/سالب فيفسد المخزون ورصيد المورد بصمت.
        var invalidQtyLine = _lines.FirstOrDefault(l => l.ProductId > 0 && l.Quantity <= 0);
        var invalidPriceLine = _lines.FirstOrDefault(l => l.ProductId > 0 && l.UnitPrice < 0);

        // سطر فيه كود/كمية بلا صنف محلول: كان يُسقط من الطلب بصمت ويُحسب في الإجمالي.
        var unresolvedLine = _lines.FirstOrDefault(l => l.ProductId <= 0 &&
            (!string.IsNullOrWhiteSpace(l.ProductCode) || l.Quantity != 0m || l.UnitPrice != 0m));

        if (invalidQtyLine != null || invalidPriceLine != null || unresolvedLine != null)
        {
            string lineMessage = invalidQtyLine != null
                ? $"الكمية يجب أن تكون أكبر من صفر ({invalidQtyLine.ProductName})"
                : invalidPriceLine != null
                    ? $"سعر الوحدة لا يمكن أن يكون سالباً ({invalidPriceLine.ProductName})"
                    : $"السطر «{unresolvedLine!.ProductCode}» غير مرتبط بصنف مسجَّل — صحّح الكود/الباركود أو احذف السطر";
            errorProvider.SetError(dgvItems, lineMessage);
            clsInputFeedback.ShowRejectTooltip(dgvItems, lineMessage, toolTip);
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvItems, "");   // لا يبقى تلميح رفض قديم بعد تصحيح السطور
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

        if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم تقليل المخزون وتقليل رصيد المورد.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try {
            // ⚠️ لا مستند مالي مكرر: إذا نجح الإنشاء في محاولة سابقة وفشل الترحيل،
            // فإعادة المحاولة تُرحّل المستند نفسه (_createdId) ولا تنشئ مرتجعاً ثانياً.
            int documentId;

            if (_createdId is int existingId)
            {
                documentId = existingId;
            }
            else
            {
                // سطر المرتجع: السعر المرسَل هو نفسه سعر وحدة السطر المعروض والمُتحقَّق منه
                // (UnitPrice، الذي يُملأ أيضاً من UnitCost عند تحميل مرتجع محفوظ) — كان
                // يُرسَل UnitCost وهو دائماً صفر فيُحفظ كل سطر بتكلفة صفر.
                var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreatePurchaseReturnItemRequest(
                    0, l.ProductId, 1, l.Quantity, l.UnitPrice, l.LineTotal)).ToList();
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
                if (!result.IsSuccess || result.Value is not { } created)
                {
                    clsDialogs.Error(result.Error);
                    return;
                }

                // نُثبّت المستند المُنشأ: المعرّف يمنع الإنشاء المكرر، والـDTO يُعرض في الشاشة.
                _createdId = created.Id;
                _invoice = created;
                lblInvoiceNo.Text = created.ReturnNo.ToString();
                UpdateUIState();
                documentId = created.Id;
            }

            if (post)
            {
                var postRes = await _returnApi.PostAsync(documentId);
                if (!postRes.IsSuccess)
                {
                    // لا إغلاق عند فشل الترحيل: المرتجع محفوظ كمسودة، وإعادة المحاولة تُرحّله.
                    _LockAfterCreate();
                    // رسالة واحدة فقط: التحذير التفصيلي أدناه هو ما يقرأه المستخدم (RULE-N169).
                    clsDialogs.Warning(
                        $"أُنشئ المرتجع كمسودة برقم {_createdId} وتعذر ترحيله.\n" +
                        "لم يُنشأ مستند جديد: أعد الضغط على «ترحيل» (F4) لترحيل نفس المسودة.\n" +
                        "سطور المرتجع مقفلة الآن لأنها محفوظة كمسودة.",
                        "لم يُرحَّل المرتجع");
                    return;
                }

                clsDialogs.Success("تم الترحيل");
                foreach (var line in _lines.Where(l => l.ProductId > 0))
                    _eventBus.Publish(new StockChangedMessage(line.ProductId));
            }
            else clsDialogs.Success("تم الحفظ");

            _eventBus.Publish(new PurchaseReturnChangedMessage(documentId));
            this.DialogResult = DialogResult.OK; this.Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ مرتجع المشتريات");
            clsDialogs.Error("خطأ في حفظ المرتجع. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            Cursor = Cursors.Default;

            // الحالة تُعاد من المصدر الواحد (RULE-N150): كان تفعيل الزرين هنا يُفعّل «حفظ مسودة» وهو
            // مخفي على مرتجع محفوظ — وButton.PerformClick() يفحص Enabled ولا يفحص Visible.
            // والحارس يمنع لمس الحالة بعد نجاح الحفظ وإغلاق النموذج (Close يُدمّر النموذج غير النمطي).
            if (!IsDisposed) UpdateUIState();
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    /// <summary>
    /// يقفل سطور المرتجع بعد أن أُنشئ المستند فعلاً (وفشل ترحيله): فالبيانات المعروضة على الشاشة
    /// هي بالضبط ما ستُرحّله إعادة المحاولة — لا تعديل يُخالف المسودة المحفوظة.
    /// </summary>
    private void _LockAfterCreate()
    {
        dgvItems.ReadOnly = true;
        dgvItems.AllowUserToAddRows = false;
        btnAddItem.Enabled = false;
        btnRemoveItem.Enabled = false;
    }
}
