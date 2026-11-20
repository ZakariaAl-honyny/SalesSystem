using Serilog;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
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
using SalesSystem.Desktop.Products.Forms;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

public partial class frmAddUpdatePurchaseInvoice : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IPurchaseInvoiceDesktopService _purchaseApi => _serviceProvider.GetRequiredService<IPurchaseInvoiceDesktopService>();
    private IPurchaseReturnDesktopService _returnApi => _serviceProvider.GetRequiredService<IPurchaseReturnDesktopService>();
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

    // ── وضع المستند (TASK-018) — الشاشة نفسها لفاتورة الشراء ومرتجع الشراء ──────────────────
    private PurchaseDocumentMode _documentMode = PurchaseDocumentMode.Purchase;
    private PurchaseReturnDto? _returnDoc;        // المرتجع المحمَّل/المُنشأ في وضع المرتجع
    private int? _createdReturnId;                // يمنع إنشاء مرتجع ثانٍ عند إعادة محاولة الترحيل
    private bool _isApplyingMode;                 // حارس إعادة الدخول على معالجات زرّي الوضع

    /// <summary>وضع المستند الحالي — يقرأه المستدعي عند الحاجة (TASK-018).</summary>
    public PurchaseDocumentMode DocumentMode => _documentMode;

    /// <summary>
    /// هل بيانات المستند قابلة للتحرير؟ فاتورة الشراء: جديدة أو مسودة (السلوك القائم حرفياً).
    /// مرتجع الشراء: جديد فقط — العقد <see cref="IPurchaseReturnDesktopService"/> بلا <c>UpdateAsync</c>
    /// فتحرير مستند محفوظ لا يُرسَل (D-5).
    /// </summary>
    private bool IsEditable => _documentMode == PurchaseDocumentMode.PurchaseReturn
        ? _returnDoc == null
        : _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;

    /// <summary>حارس إعادة الدخول على الحفظ: يمنع نقراً مزدوجاً أو تحرير سطر أثناء الإرسال.</summary>
    private bool _isSaving = false;

    public frmAddUpdatePurchaseInvoice(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // ── لوحة «إضافة صنف» أعلى الشبكة (TASK-014 · بيت واحد ctrlLineEntryPanel) ──────────────
        // الشاشة **تُسند مُفوَّداتها فقط** ولا تكتب سطراً من منطق الإدخال: الباركود والوحدات والدمج
        // وحدّ الخصم وإعادة التركيز كلها في المكوّن (RULE-N169…RULE-N172). المكوّن يعرف «كيف يُدخل
        // سطر»، والشاشة تعرف «من أين يأتي الصنف وكيف يُحسب المال».
        ctrlLineEntry.Lines = _lines;
        ctrlLineEntry.PriceCaption = "سعر التكلفة:";
        // سعر الشراء = ProductUnitDto.CostPrice. ‏ProductDto نفسه يحمل سعري بيع/شراء مرميّين على 0m
        // (مقيس) — فالسعر يُقرأ من الوحدة حصراً وإلا مُلئ كل سطر بسعر صفر.
        ctrlLineEntry.PriceSelector = u => u.CostPrice;
        // حكم «هل يُسمح بالإدخال» بيت واحد في الشاشة (IsEditable) — يتبع وضع المستند حيّاً فلا
        // نسخة ثانية من شرط «مسودة/مرتجع جديد» هنا (RULE-N172) ولا حاجة لإعادة إسناده عند تغيير الوضع.
        ctrlLineEntry.IsEditable = () => IsEditable;
        ctrlLineEntry.ResolveBarcodeAsync = FindProductByBarcodeAsync;
        ctrlLineEntry.LoadUnitsAsync = async id =>
            (await _productApi.GetProductUnitsAsync(id)).Value ?? Array.Empty<ProductUnitDto>();
        ctrlLineEntry.PickProductAsync = PickProductAsync;
        ctrlLineEntry.TotalsChanged = CalculateTotals;

        // في فاتورة الشراء سعر اللوحة هو التكلفة نفسها: تُزامن دائماً وإلا رفض الحفظ (UnitCost <= 0)
        // أو رُحّل السعر القديم صامتاً بعد تعديل سطر مسودة (مراجعة TASK-014 — الحاجز #1/#2).
        // المكوّن يُطلق LineCommitted على المسارات الثلاثة كلها — الإضافة · تعديل/تحديث سطر محمَّل ·
        // دمج الكمية · والمسح السريع (+1) — و`CommittedLine` يحمل السطر المقصود، فالاشتراك الواحد يكفي.
        ctrlLineEntry.LineCommitted += (_, _) =>
        {
            if (ctrlLineEntry.CommittedLine is { } line)
                line.UnitCost = line.UnitPrice;
        };

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (4 من 6).
        // RULE-N191 — Enter داخل رأس الفاتورة (gbInvoiceInfo) يمشي بترتيب العرض حقلاً بحقل ثم «يُركِّز»
        // زر الحفظ عند آخر حقل بلا تنفيذ. حقول المجاميع خارج النطاق فتبقى على SelectNextControl كما هي،
        // وشبكة السطور ليست حقل إدخال لهذا الـHelper فتبقى Enter ملكاً لها (RULE-N153 / RULE-N22a).
        KeyboardHelper.WireEnterAsTab(gbInvoiceInfo, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45

        // RULE-N180…N183 — تغذية الحقول المطلوبة حيّاً: أيقونة ✗ + رسالة عربية + ومضة رفض تعود إلى لون
        // المصمّم تلقائياً. إلزامي: **بعد** WireFocusHighlight (ترتيب Leave يحسم مَن يرسم الخلفية أخيراً)
        // و**قبل** ApplyUiTheme وما فيه من WireDisabledVisuals. الشاشة تُعلن قواعدها فقط — لا أيقونة
        // ولا لون ولا استعادة خلفية هنا (RULE-N169…RULE-N172).
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbSupplier, "يرجى اختيار المورد"),
            new RequiredField(cmbWarehouse, "يرجى اختيار المستودع"));

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(
            btnRemoveItem, btnSaveDraft, btnPost, btnCancelInvoice, btnPrintA4, btnClose, btnPickInvoice);

        // RULE-N47 — الاختصارات الموحّدة على الأزرار الموجودة فعلاً في الشاشة.
        // F2 = «سطر صنف جديد» ⇒ إعادة المؤشر إلى باركود لوحة الإضافة (لا زر «إضافة صنف» بعد اليوم).
        // **F4 أُزيل من هنا عمداً:**WireGlobalShortcuts يعمل على Form.KeyDown مع KeyPreview فيبتلع
        // المفتاح قبل حقل الباركود، فيُقتل F4 = منتقي الأصناف في المكوّن. الترحيل يبقى على زرّه.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, ctrlLineEntry.FocusBarcode),             // جديد = سطر صنف جديد
            (Keys.F3, () => btnSaveDraft.PerformClick()),      // حفظ/عرض المسودة
            (Keys.F5, _RefreshAsync),                          // تحديث
            (Keys.Delete, _GlobalDelete),                      // حذف السطر المحدد
            (Keys.Escape, _GlobalEscape));                     // خروج

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي (Enter داخل حقل = الحقل التالي — RULE-N40).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        _SyncAcceptButton();   // الزر الافتراضي الابتدائي = الزر الأساسي الحالي

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;

        SetupGrid();
        ApplyUiTheme();   // ينتهي بالثيم الأخضر (RULE-N50)
    }

    public void LoadData(PurchaseInvoiceDto? invoice)
    {
        // TASK-018 — نسخة معاد استخدامها من الحاوية لا يجوز أن تبقى في وضع المرتجع أو على مرتجع سابق.
        _documentMode = PurchaseDocumentMode.Purchase;
        _returnDoc = null;
        _createdReturnId = null;

        _invoice = invoice;
        ApplyUiTheme();
        _ApplyDocumentMode();   // الراديو + رؤية الحقول + العنوان + الحالة + الإجماليات
    }

    /// <summary>
    /// يفتح الشاشة في وضع «مرتجع شراء» على المستند المطلوب (أو على مرتجع جديد حين يكون <c>null</c>).
    /// يُستدعى قبل إظهار النموذج — والربط يحدث في <see cref="OnLoad"/> بعد تحميل القوائم.
    /// <para>
    /// <c>PurchaseReturnDto</c> يحمل سطوره (<c>Items</c>) فلا حاجة إلى نداء <c>GetByIdAsync</c>
    /// على خدمة الفواتير.
    /// </para>
    /// </summary>
    public void LoadAsReturn(PurchaseReturnDto? purchaseReturn)
    {
        _documentMode = PurchaseDocumentMode.PurchaseReturn;
        _returnDoc = purchaseReturn;
        _createdReturnId = purchaseReturn?.Id;
        _invoice = null;

        ApplyUiTheme();
        _ApplyDocumentMode();
    }

    /// <summary>
    /// يبدأ مستنداً جديداً بالوضع المطلوب — يُستدعى من القائمة (مرتجع شراء) قبل العرض أو على نموذج مفتوح.
    /// لا تأكيد هنا: الطلب صريح من المستخدم، والتحرير غير المحفوظ يخصّ مستنداً آخر يبدأ من جديد.
    /// </summary>
    public void StartNewDocument(PurchaseDocumentMode mode)
    {
        _invoice = null;
        _returnDoc = null;
        _createdReturnId = null;
        _documentMode = mode;

        ResetForm();
        _ApplyDocumentMode();
        UpdateUIState();
        CalculateTotals();

        if (IsHandleCreated) ctrlLineEntry.FocusBarcode();
    }

    /// <summary>
    /// نفس <see cref="StartNewDocument"/> لكن بتأكيد عربي حين يوجد مستند أو سطور غير محفوظة.
    /// يعود <c>false</c> عند إلغاء المستخدم فيبقى المستند الحالي كما هو.
    /// </summary>
    public bool TryStartNewDocument(PurchaseDocumentMode mode)
    {
        bool holdsDocument = _invoice is not null || _returnDoc is not null;
        bool holdsLines = _lines.Any(l => l.ProductId > 0);

        if ((holdsDocument || holdsLines) &&
            !clsDialogs.Confirm("سيبدأ مستند جديد وسيتم فقدان غير المحفوظ. هل تريد المتابعة؟"))
            return false;

        StartNewDocument(mode);
        return true;
    }

    // ═════════════════════════════════════════════════════════════════
    //  DOCUMENT MODE (TASK-018) — زرّا اختيار: فاتورة شراء / مرتجع شراء
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// حقول الفاتورة التي لا معنى لها في المرتجع (نوع الدفع · المدفوع · المتبقي · خصم الترويسة ·
    /// طباعة الفاتورة) — تُخفى في وضع المرتجع وتظهر في وضع الفاتورة (D-3 / RULE-N167: الواجهة لا تكذب).
    /// </summary>
    private Control[] PurchaseOnlyControls => new Control[]
    {
        lblPaymentType, cmbPaymentType,
        lblPaid, txtPaid,
        lblDue, lblDueVal,
        lblInvoiceDiscount, numInvoiceDiscount,
        btnPrintA4
    };

    /// <summary>
    /// يطبّق الوضع الحالي كاملاً: زرّا الاختيار ← إظهار/إخفاء الحقول ← العنوان ← الحالة ← الإجماليات.
    /// كل انتقال يمرّ من هنا وحده (RULE-N150) فلا تتنازع الحالة بين المعالجات.
    /// </summary>
    private void _ApplyDocumentMode()
    {
        _isApplyingMode = true;
        try
        {
            rbtnModePurchase.Checked = _documentMode == PurchaseDocumentMode.Purchase;
            rbtnModeReturn.Checked = _documentMode == PurchaseDocumentMode.PurchaseReturn;
        }
        finally
        {
            _isApplyingMode = false;
        }

        bool isReturn = _documentMode == PurchaseDocumentMode.PurchaseReturn;
        foreach (var control in PurchaseOnlyControls)
            control.Visible = !isReturn;

        _UpdateModeHeader();   // شريط الوضع + عنوان النافذة
        UpdateUIState();       // الأزرار وحدود التحرير حسب الوضع
        CalculateTotals();     // الإجماليات: خصم/مدفوع في الفاتورة فقط
    }

    /// <summary>
    /// تبديل نوع المستند من زرّي الاختيار. مستند قائم أو سطور غير محفوظة ⇒ تأكيد عربي،
    /// والإلغاء يُعيد الاختيار السابق ولا يفقد سطراً.
    /// </summary>
    private void DocumentMode_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isApplyingMode) return;

        var requested = rbtnModeReturn.Checked
            ? PurchaseDocumentMode.PurchaseReturn
            : PurchaseDocumentMode.Purchase;
        if (requested == _documentMode) return;

        bool holdsDocument = _invoice is not null || _returnDoc is not null;
        bool holdsLines = _lines.Any(l => l.ProductId > 0);
        if ((holdsDocument || holdsLines) &&
            !clsDialogs.Confirm("تغيير نوع المستند سيبدأ مستنداً جديداً ويفقد غير المحفوظ. هل تريد المتابعة؟"))
        {
            _ApplyDocumentMode();   // إعادة الاختيار إلى الوضع القائم بلا فقدان سطر
            return;
        }

        _invoice = null;
        _returnDoc = null;
        _createdReturnId = null;
        _documentMode = requested;

        if (dgvItems.IsCurrentCellInEditMode) dgvItems.CancelEdit();   // لا تبقى خلية قيد التحرير على سطور ستُمسح
        ResetForm();
        _ApplyDocumentMode();

        if (ctrlLineEntry.Enabled) ctrlLineEntry.FocusBarcode();
    }

    // ── الاختصارات العامة (RULE-N47) ─────────────────────────────────────────

    /// <summary>Delete: حذف السطر المحدد — يعمل فقط عندما تكون الشبكة هي صاحبة التركيز،
    /// ولا يحذف أثناء تحرير خلية داخلها (RULE-N22a — نفس حارس معالج Esc).</summary>
    private void _GlobalDelete()
    {
        if (dgvItems.IsCurrentCellInEditMode) return;

        if (dgvItems.ContainsFocus && btnRemoveItem.Enabled) btnRemoveItem.PerformClick();
    }

    /// <summary>
    /// Esc: يُلغي أولاً ما هو جارٍ (تحرير خلية · سطر في لوحة الإضافة)، وإلا يُغلق النموذج (RULE-N47).
    /// <para>
    /// <b>لماذا يُعالَج Esc هنا؟</b> ‏<see cref="KeyboardHelper.WireGlobalShortcuts"/> يعمل على
    /// <c>Form.KeyDown</c> مع <c>KeyPreview</c> فيسبق حقلَ الباركود — ولو لم يُعالَج هنا لأغلق Esc
    /// الفاتورة بدل إلغاء السطر (نفس سبب إزالة F4 من قائمة الاختصارات).
    /// </para>
    /// </summary>
    private void _GlobalEscape()
    {
        if (dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            return;
        }

        // Esc داخل لوحة الإضافة = إلغاء السطر الجاري وتفريغها (نمط الشاشة المرجعية) — لا إغلاق.
        if (ctrlLineEntry.ContainsFocus)
        {
            ctrlLineEntry.CancelEdit();
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

    /// <summary>
    /// مؤشر الوضع في شريط العنوان (RULE-N44) — يتبع الوضع الحالي: فاتورة شراء أو مرتجع شراء،
    /// جديد أو محفوظ. عنوان النافذة من نفس المصدر فلا نسخة ثانية للعنوان في مكان آخر.
    /// </summary>
    private void _UpdateModeHeader()
    {
        bool isReturn = _documentMode == PurchaseDocumentMode.PurchaseReturn;

        lblMode.Text = isReturn
            ? (_returnDoc == null
                ? "(( مرتجع مشتريات جديد ))"
                : $"(( مرتجع مشتريات — {_returnDoc.ReturnNo} ))")
            : (_invoice == null
                ? "(( فاتورة مشتريات جديدة ))"
                : $"(( فاتورة مشتريات — {_invoice.InvoiceNo} ))");

        this.Text = isReturn
            ? (_returnDoc == null ? "مرتجع مشتريات جديد" : $"مرتجع مشتريات - {_returnDoc.ReturnNo}")
            : (_invoice == null ? "فاتورة مشتريات جديدة" : $"فاتورة مشتريات - {_invoice.InvoiceNo}");

        _SyncAcceptButton();
    }

    /// <summary>
    /// الزر الأساسي الذي يستقبله <c>Enter</c> (RULE-N147) — تعريف واحد في الشاشة يُقرأ عند كل ضغطة:
    /// وضع التحرير (فاتورة جديدة/مسودة أو مرتجع جديد) ⇒ «حفظ مسودة» · وضع العرض (مرحّلة/ملغاة أو
    /// مرتجع محفوظ) ⇒ <c>null</c> (بلا أي حفظ أو ترحيل).
    /// <para>
    /// <b>الترحيل ليس زر Enter أبداً:</b> يبقى على زرّه لأنه يخصم/يزيد المخزون ويقفل المستند (§2.4) —
    /// فالمفتاح العابر لا يُرحّل مستنداً مالياً.
    /// </para>
    /// <para>
    /// حارس إلزامي (RULE-N153): التركيز داخل شبكة السطور (أو خلية قيد التحرير) ⇒ <c>null</c>، فـ<c>Enter</c>
    /// هناك يبقى تنقّلاً/إدخالاً داخل الشبكة ولا يُنفّذ زر الوضع أبداً.
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        // RULE-N153 — الشبكة تملك Enter: إدخال سطر لا حفظ/ترحيل، ولا نقر لزر الوضع من داخلها.
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return null;

        // لوحة الإضافة تحمّل سطراً للتعديل: Enter هناك يُثبّت السطر ولا يُنفّذ زر الوضع.
        if (ctrlLineEntry.IsEditing) return null;

        // غير قابل للتحرير (مرحّلة/ملغاة · مرتجع محفوظ) ⇒ لا إجراء لـEnter.
        if (!IsEditable) return null;

        // أثناء الحفظ الجاري الزر معطّل ⇒ لا Enter مزدوج.
        return btnSaveDraft is { Visible: true, Enabled: true } ? btnSaveDraft : null;
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
        // عنوان النافذة يملكه _UpdateModeHeader وحده (RULE-N44/RULE-N172) — لا نسخة ثانية هنا.

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

            if (_documentMode == PurchaseDocumentMode.PurchaseReturn)
            {
                // وضع المرتجع: الـDTO يحمل سطوره، فيُربط هنا مباشرة — لا نداء GetByIdAsync على
                // خدمة الفواتير ولا ربط قبل اكتمال القوائم (وإلا أُسند المورد/المستودع الخطأ).
                if (_returnDoc is not null) BindReturn();
                else ResetForm();
            }
            else if (_invoice is not null)
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

            // الراديو + رؤية الحقول + العنوان + الحالة + الإجماليات — من مصدر واحد (RULE-N150).
            _ApplyDocumentMode();
            ApplyUiTheme();

            // RULE-N22a — الشبكة عرض/اختيار فقط: الإدخال كله في لوحة الإضافة أعلاه، فصفوفها لا تُضاف
            // من الشبكة (AllowUserToAddRows/ReadOnly مضبوطان في SetupGrid) ولا تحرير خلية فيها.
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
                    // اسم الوحدة يُعرض في عمود «الوحدة» — لا يبقى اختيار الوحدة في اللوحة غير مرئي في الشبكة.
                    UnitName = item.ProductUnitName,
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

    /// <summary>
    /// ربط مرتجع شراء محمَّل بالشاشة (نظير <see cref="BindInvoice"/>). الـDTO لا يحمل كود الصنف،
    /// فيبقى <c>ProductCode</c> فارغاً، واسم الوحدة يأتي من <c>PurchaseReturnItemDto.ProductUnitName</c>.
    /// <para>
    /// سعر السطر = <c>UnitCost</c> نفسه (<c>UnitPrice</c>) لأن <c>LineTotal</c> تُحسب من <c>UnitPrice</c>
    /// و<i>_FirstLineError</i> يتحقق من <c>UnitCost</c> — فلا سطر محمَّل بتكلفة صفر يُرفض عند الترحيل.
    /// </para>
    /// </summary>
    private void BindReturn()
    {
        _isUpdating = true;
        try
        {
            // مرتجع محفوظ: معرّفه يمنع إنشاء نسخة ثانية عند إعادة محاولة الترحيل.
            _createdReturnId = _returnDoc!.Id;
            lblInvoiceNo.Text = _returnDoc.ReturnNo.ToString();
            dtpDate.Value = _returnDoc.ReturnDate.ToDateTime(TimeOnly.MinValue);
            cmbSupplier.SelectedValue = _returnDoc.SupplierId;
            // WarehouseDto.Id من نوع short — الإسناد بقيمة int لا يطابق أي عنصر فيُفرغ المستودع المحفوظ.
            cmbWarehouse.SelectedValue = Convert.ToInt16(_returnDoc.WarehouseId);
            txtNotes.Text = _returnDoc.Notes ?? string.Empty;

            _lines.Clear();
            foreach (var item in _returnDoc.Items)
            {
                _lines.Add(new InvoiceLineItem
                {
                    ProductId = item.ProductId,
                    ProductCode = string.Empty,   // غير موجود في PurchaseReturnItemDto
                    ProductName = item.ProductName,
                    ProductUnitId = item.ProductUnitId,
                    UnitName = item.ProductUnitName,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                    UnitPrice = item.UnitCost,
                    DiscountAmount = 0m,
                    ConversionFactor = 1m
                });
            }
        }
        finally
        {
            _isUpdating = false;
        }

        _UpdateModeHeader();
    }

    /// <summary>
    /// إعادة ضبط المحرر لمستند جديد — <b>يتبع الوضع الحالي</b>: حقول الفاتورة (نوع الدفع · خصم
    /// الترويسة · المدفوع) لا تُعاد تهيئتها في وضع المرتجع لأنها مخفيّة وعقد المرتجع لا يحملها.
    /// </summary>
    private void ResetForm()
    {
        bool isReturn = _documentMode == PurchaseDocumentMode.PurchaseReturn;

        // مستند جديد ⇒ لا مستند مرتجع سابق ولا معرّف محفوظ يُرحَّل بدل الجديد (لا تكرار للمستند).
        _returnDoc = null;
        _createdReturnId = null;

        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;

        if (!isReturn)
        {
            cmbPaymentType.SelectedIndex = 0;
            numInvoiceDiscount.Value = 0;
            txtPaid.Text = "0";
        }

        _lines.Clear();
        txtNotes.Clear();

        // RULE-N183 — مسار إعادة الضبط يزيل أي علامة «مطلوب» موروثة من محاولة حفظ سابقة.
        RequiredFieldHelper.Clear(this);

        // مستند جديد ⇒ لوحة الإضافة تبدأ من باركود فارغ — فلا يبقى سطر نصف مُدخل من مستند سابق.
        ctrlLineEntry.ResetPanel();
        ctrlLineEntry.FocusBarcode();

        _UpdateModeHeader();
    }

    /// <summary>
    /// عند الظهور: المؤشر على باركود لوحة الإضافة — أول ما يفعله المستخدم. (<c>OnLoad</c> يسبق الظهور
    /// فلا ينجح <c>Focus</c> فيه، ولذلك يقع الطلب هنا.)
    /// </summary>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        ctrlLineEntry.FocusBarcode();
    }

    /// <summary>
    /// إعداد الشبكة: <b>عرض واختيار فقط</b> — الإدخال كله في لوحة «إضافة صنف» أعلى الشاشة
    /// (TASK-014)، فلا تحرير داخل الخلية ولا صف إدخال جديد هنا. أعمدة الشبكة تُقرأ من
    /// <see cref="InvoiceLineItem"/> فقط، ولا تحمل الشاشة أي منطق بحث/دمج للأصناف.
    /// </summary>
    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Clear();
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 250, ReadOnly = true });
        // عمود الوحدة: اللوحة تتيح اختيار وحدة الصنف، وبدون هذا العمود يبقى الاختيار غير مرئي في الشبكة.
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColUnit", HeaderText = "الوحدة", DataPropertyName = "UnitName", Width = 90, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80 });
        // عمود التكلفة: يُعرض من UnitCost، والاشتراك في المُنشئ يُزامن UnitCost = UnitPrice عند كل
        // تأكيد سطر — فما تراه في الشبكة هو نفسه ما يُرسل في الطلب (لا صفر ولا سعر قديم).
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCost", HeaderText = "التكلفة", DataPropertyName = "UnitCost", Width = 100 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDisc", HeaderText = "خصم", DataPropertyName = "DiscountAmount", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 120, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { ForeColor = GreenThemeHelper.TextStrong, Font = new Font(dgvItems.Font, FontStyle.Bold) } });

        // RULE-N169: الدلالة تُعلَن (مبلغ مالي) والنمط يأتي من clsFormat الحيّ عبر البيت الواحد — لا "N2" مكتوبة هنا.
        GridFormatHelper.ApplyColumns(dgvItems, GridFormatKind.Money, "ColTotal");

        // RULE-N22 — شبكة عرض: لا صف إدخال، ولا تحرير، واختيار بمستوى الخلية/الصف المفرد.
        dgvItems.AllowUserToAddRows = false;
        dgvItems.ReadOnly = true;
        dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;

        dgvItems.DataSource = _lines;

        // النقر المزدوج يحمّل السطر في اللوحة (نمط frmAddUpdateSalesInvoice) — لا تحرير داخل الخلية.
        dgvItems.CellDoubleClick += DgvItems_CellDoubleClick;

        // RULE-N153 — Enter داخل شبكة السطور تنقّل ولا ينفّذ الزر الأساسي أبداً:
        // AcceptButton يُفرَّغ عند دخول الشبكة ويُعاد عند مغادرتها.
        dgvItems.Enter += (_, _) => _SyncAcceptButton();
        dgvItems.Leave += (_, _) => _SyncAcceptButton();
    }

    /// <summary>
    /// نقر مزدوج على سطر ⇒ تحميله في لوحة «إضافة صنف» لتعديله، بـ«تحديث السطر» و<c>Esc</c> للإلغاء.
    /// الحكم على «هل التعديل مسموح» يأتي من <c>ctrlLineEntry.Enabled</c> نفسه الذي يضبطه
    /// <see cref="UpdateUIState"/> — فلا نسخة ثانية من شرط «مسودة» هنا (RULE-N172).
    /// </summary>
    private void DgvItems_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= _lines.Count) return;
        if (_isSaving || !ctrlLineEntry.Enabled) return;

        ctrlLineEntry.BeginEdit(_lines[e.RowIndex]);
    }

    // ── الربط مع لوحة «إضافة صنف» (المكوّن المشترك) ─────────────────────────

    /// <summary>
    /// بحث الباركود للوحة الإضافة: <b>الخدمة أولاً</b> (تُرجع الصنف وأي تعديل حديث)، ثم قائمة
    /// الأصناف المحمَّلة كاحتياط عند فشل النداء أو لصنف معطّل. السعر لا يأتي من هنا — <c>ProductDto</c>
    /// يحمل سعري بيع/شراء مرميّين على <c>0m</c> (مقيس)، والسعر من <see cref="ProductUnitDto.CostPrice"/>.
    /// </summary>
    private async Task<ProductDto?> FindProductByBarcodeAsync(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode)) return null;

        var result = await _productApi.GetByBarcodeAsync(barcode.Trim());
        if (result.IsSuccess && result.Value is { IsActive: true }) return result.Value;

        // احتياط: باركود غير موجود في الخدمة (أو صنف معطّل) ⇒ من القائمة المحمَّلة، بلا حساسية لحالة الأحرف.
        return _allProducts.FirstOrDefault(
            p => string.Equals(p.Barcode?.Trim(), barcode.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// منتقي الأصناف (F4 أو باركود غير مسجّل) — <see cref="frmProductSelection"/> القائم (RULE-N169)،
    /// ويُمرَّر المستودع المختار فيُظهر أرصدة ذلك المخزن. الإلغاء يعود <c>null</c> بلا أي تغيير.
    /// </summary>
    private Task<PickedProduct?> PickProductAsync()
    {
        var dialog = _serviceProvider.GetRequiredService<frmProductSelection>();
        if (_SelectedWarehouseId > 0) dialog.WarehouseId = Convert.ToInt16(_SelectedWarehouseId);

        if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct is null)
            return Task.FromResult<PickedProduct?>(null);

        return Task.FromResult<PickedProduct?>(
            new PickedProduct(dialog.SelectedProduct, dialog.SelectedUnit));
    }

    // ── أحداث الحقول (كما كانت مربوطة في المصمّم) ────────────────────────────

    private void numInvoiceDiscount_ValueChanged(object? sender, EventArgs e) => CalculateTotals();
    private void txtPaid_TextChanged(object? sender, EventArgs e) => CalculateTotals();

    private void CalculateTotals()
    {
        if (_isUpdating) return;
        decimal subTotal = _lines.Sum(x => x.LineTotal);
        int itemCount = _lines.Count(l => l.ProductId > 0);
        decimal totalQuantity = _lines.Where(l => l.ProductId > 0).Sum(l => l.Quantity);

        // وضع المرتجع: العقد لا يحمل خصم ترويسة ولا مدفوعاً (D-3) — فالمجاميع تُقرأ من السطور
        // وحدها، والحقول المخفية لا تُلمس ولا تدخل الحساب (RULE-N167).
        if (_documentMode == PurchaseDocumentMode.PurchaseReturn)
        {
            lblSubTotalVal.Text = clsFormat.Money(subTotal);
            lblTotalVal.Text = clsFormat.Money(subTotal);
            // شريط الملخّص: المرتجع بلا خصم ترويسة وبلا مدفوع (D-3) ⇒ المدفوع صفر والمتبقي = الإجمالي.
            lblQuickInfo.Text = $"عدد الأصناف: {clsFormat.Number(itemCount)} | إجمالي الكمية: {clsFormat.Quantity(totalQuantity)} | الإجمالي: {clsFormat.Money(subTotal)} | المدفوع: {clsFormat.Money(0m)} | المتبقي: {clsFormat.Money(subTotal)}";
            return;
        }

        decimal invoiceDiscount = numInvoiceDiscount.Value;
        decimal paidAmount = txtPaid.DecimalValue;

        // قرار المالك: MVP V1 بلا ضريبة على مستندات الشراء — الإجمالي = الفرعي - خصم الفاتورة.
        decimal totalAmount = subTotal - invoiceDiscount;
        decimal dueAmount = totalAmount - paidAmount;

        lblSubTotalVal.Text = clsFormat.Money(subTotal);
        lblTotalVal.Text = clsFormat.Money(totalAmount);
        lblDueVal.Text = clsFormat.Money(dueAmount);
        lblDueVal.ForeColor = dueAmount > 0 ? GreenThemeHelper.StatusNegative : GreenThemeHelper.StatusPositive;
        lblQuickInfo.Text = $"عدد الأصناف: {clsFormat.Number(itemCount)} | إجمالي الكمية: {clsFormat.Quantity(totalQuantity)} | الإجمالي: {clsFormat.Money(totalAmount)} | المدفوع: {clsFormat.Money(paidAmount)} | المتبقي: {clsFormat.Money(dueAmount)}";
    }

    private void UpdateUIState()
    {
        bool isReturn = _documentMode == PurchaseDocumentMode.PurchaseReturn;
        bool editable = IsEditable;

        gbInvoiceInfo.Enabled = editable;
        // رأس الفاتورة وإجمالياتها يتبعان حالة التحرير: معطّلان على مستند مرحّل/ملغى.
        gbTotals.Enabled = editable;

        // لوحة «إضافة صنف» هي مسار الإدخال الوحيد: تعطيلها يقفل الإدخال كله على مستند مرحّل/ملغى
        // أو مرتجع محفوظ، وResetPanel يُلغي أي سطر نصف مُدخل فلا يبقى سطر لا يمكن حفظه.
        ctrlLineEntry.Enabled = editable;
        if (!editable)
        {
            ctrlLineEntry.ResetPanel();
            // RULE-N183 — الانتقال إلى وضع العرض يزيل علامات «مطلوب» عن الحقول المعطّلة.
            RequiredFieldHelper.Clear(this);
        }

        btnRemoveItem.Enabled = editable;

        if (isReturn)
        {
            // المرتجع: «حفظ» للجديد فقط، و«ترحيل» للجديد وللمسودة المحفوظة، و«إلغاء» للمرحّل فقط،
            // و«اختيار فاتورة» للجديد فقط (يملأ الرأس والسطور فلا معنى له على مستند محفوظ).
            bool hasReturn = _returnDoc is not null;
            bool returnIsDraft = hasReturn && _returnDoc!.Status == (byte)InvoiceStatus.Draft;

            btnSaveDraft.Visible = btnSaveDraft.Enabled = editable;
            btnPost.Visible = btnPost.Enabled = !hasReturn || returnIsDraft;
            btnCancelInvoice.Visible = btnCancelInvoice.Enabled =
                hasReturn && _returnDoc!.Status == (byte)InvoiceStatus.Posted;
            btnPickInvoice.Visible = btnPickInvoice.Enabled = editable;
        }
        else
        {
            // الإخفاء وحده لا يمنع اختصار F3 — التعطيل إلزامي (RULE-N43/N152)، ويُفعَّل فقط في وضع التحرير.
            btnSaveDraft.Visible = btnSaveDraft.Enabled = editable;
            btnPost.Visible = btnPost.Enabled = editable;
            btnCancelInvoice.Visible = btnCancelInvoice.Enabled =
                _invoice != null && _invoice.Status == (byte)InvoiceStatus.Posted;
            // «اختيار فاتورة» يخصّ المرتجع وحده — مخفي في وضع الفاتورة (D-4).
            btnPickInvoice.Visible = false;
            btnPickInvoice.Enabled = false;
        }

        // RULE-N171/N172: نص الحالة من البيت الواحد DocumentTextHelper (1..3 — RULE-019)؛
        // و«جديد» حين لا يوجد مستند بعد.
        byte? activeStatus = isReturn ? _returnDoc?.Status : _invoice?.Status;

        lblStatus.Text = activeStatus is null
            ? "جديد"
            : DocumentTextHelper.StatusText(activeStatus.Value);

        // ألوان الحالة من ثوابت ThemeHelper (لا أرقام لون داخل الشاشة) — مطابقة لقيمة المصمّم.
        lblStatus.BackColor = activeStatus switch
        {
            1 => ThemeHelper.Neutral,
            2 => ThemeHelper.Success,
            3 => ThemeHelper.Danger,
            _ => ThemeHelper.Primary
        };

        // RULE-N147 — الزر الذي يستقبله Enter يتبع الوضع الحالي (بلا حفظ/ترحيل من شبكة السطور).
        _SyncAcceptButton();
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

    /// <summary>
    /// تحقق الشاشة — قسمان لا يُخلطان:
    /// <para>
    /// (1) <b>الحقول المطلوبة</b> (المورد · المستودع): الحكم والوسم والنقل إلى أول ناقص في البيت الواحد
    /// <see cref="RequiredFieldHelper"/> (RULE-N172 / RULE-N180…N183) — يُوسَم كل ناقص (أيقونة ✗ + الرسالة
    /// + ومضة الرفض) بلا نسخة ثانية من حكم «فارغ» هنا، وبالرسالتين العربيتين نفسيهما المعلنتين في
    /// <c>RequiredFieldHelper.Attach</c> بالمُنشئ. والحقل المعطّل (مستند مرحّل/ملغى) لا يُوسَم (RULE-N154).
    /// </para>
    /// <para>
    /// (2) <b>قواعد الأسطر</b> (RULE-001 / RULE-002): سطر صنف واحد على الأقل · وحدة الصنف · كمية &gt; صفر ·
    /// تكلفة &gt; صفر — <b>تبقى كما هي حرفياً</b> بالرسالة والتلميح نفسهما وبالترتيب نفسه، وتُستهلك في
    /// <b>الوضعين</b> (فاتورة ومرتجع) بلا نسخة ثانية (D-6).
    /// </para>
    /// <para>
    /// (3) <b>قواعد مال الفاتورة وحدها</b> (خصم الترويسة · المدفوع): تُنفَّذ في وضع «فاتورة شراء» فقط،
    /// لأن الحقلين مخفيّان في وضع المرتجع وعقد المرتجع لا يحملهما (D-3).
    /// </para>
    /// </summary>
    private bool _ValidateRangeRules()
    {
        bool isValid = true;

        // (1) الحقول المطلوبة — التقييم والوسم والنقل إلى أول ناقص من البيت الواحد (لا نسخة ثانية هنا).
        if (!RequiredFieldHelper.Validate(this))
        {
            isValid = false;
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
        }

        // (2) قواعد الأسطر — لم تُمسّ (تُنفَّذ في الوضعين).
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

        // (3) قواعد المال الخاصة بفاتورة الشراء وحدها: خصم الترويسة والمدفوع. الحقلان مخفيّان في وضع
        // المرتجع وعقد المرتجع لا يحملهما (D-3) ⇒ لا حكم عليهما هناك — وRequiredFieldHelper و
        // _FirstLineError أعلاه يُنفَّذان في الوضعين بلا تغيير (RULE-N167: الواجهة لا تكذب).
        if (_documentMode == PurchaseDocumentMode.Purchase)
        {
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
        }

        if (!isValid)
        {
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    /// <summary>
    /// أول خطأ في سطور الأصناف أو <c>null</c> — رسالة واحدة محددة لكل حالة
    /// (تكافؤ الرفض: لا صنف مجهول · لا وحدة · لا كمية صفر · لا تكلفة صفر — <c>RULE-N167</c>).
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

            // LEP-1 (مقيس: 4 مواضع في 3 شاشات): كان الحفظ يسقط بصمت إلى الوحدة رقم 1 عند غياب
            // ProductUnitId — أي وحدة صنف آخر. الآن الوحدة تُختار في لوحة الإضافة فعلياً، والمتبقي رفض.
            if (line.ProductUnitId <= 0)
                return $"السطر {rowNo}: لم تُحدَّد وحدة الصنف — أعد اختياره من لوحة الإضافة.";

            if (line.Quantity <= 0m)
                return $"السطر {rowNo}: الكمية يجب أن تكون أكبر من صفر.";

            if (line.UnitCost <= 0m)
                return $"السطر {rowNo}: التكلفة يجب أن تكون أكبر من صفر.";
        }

        return null;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        // TASK-018 — الشاشة تحمل وضعين: عقد المرتجعات بلا UpdateAsync وله تحققه وطلبه وإشعاراته،
        // فالحفظ يتفرّع من سطر واحد هنا (لا نسخة ثانية من آلة الحالة).
        if (_documentMode == PurchaseDocumentMode.PurchaseReturn)
        {
            await SaveReturnAsync(post);
            return;
        }

        // حارس المسودة: فاتورة غير مسودة (مرحّلة/ملغاة) لا تُحفظ ولا تُرحّل من أي مسار — زر مخفي
        // لا يمنع اختصار F3، وهذا الحارس يقطعه قبل أي تحقق أو طلب للخدمة.
        if (_invoice is not null && _invoice.Status != (byte)InvoiceStatus.Draft)
        {
            clsDialogs.Warning("لا يمكن تعديل أو إعادة حفظ فاتورة غير مسودة.");
            return;
        }

        // الحقول المطلوبة (المورد/المستودع) في البيت الواحد RequiredFieldHelper، والمال/الأسطر في
        // _ValidateRangeRules — لا دالة تحقق ثانية في الشاشة (RULE-N172).
        if (!_ValidateRangeRules()) return;

        if (post && !clsDialogs.Confirm("هل أنت متأكد من ترحيل فاتورة المشتريات؟ سيتم زيادة المخزون.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        _isSaving = true;
        Cursor = Cursors.WaitCursor;
        try
        {
            // خصم السطر مبلغٌ ثابت: DiscountType.Amount + DiscountRate = المبلغ نفسه (بلا أي حساب)،
            // فيتساوى المحفوظ مع ما يعرضه اللصق والشبكة. الأسطر بلا خصم تبقى على الافتراضيات (null/null).
            // الوحدة تُرسل من السطر كما اختارها المستخدم في لوحة الإضافة — **لا سقوط صامت إلى الوحدة 1**
            // (LEP-1): غيابها رُفض قبل الوصول إلى هنا في _FirstLineError.
            var items = _lines
                .Where(l => l.ProductId > 0)
                .Select(l => l.DiscountAmount > 0m
                    ? new CreatePurchaseInvoiceLineRequest(
                        l.ProductId, l.ProductUnitId, l.Quantity, l.UnitCost,
                        SalesSystem.DTO.Enums.DiscountType.Amount, l.DiscountAmount)
                    : new CreatePurchaseInvoiceLineRequest(
                        l.ProductId, l.ProductUnitId, l.Quantity, l.UnitCost))
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
                txtPaid.DecimalValue,
                txtNotes.Text,
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
            _isSaving = false;
            // استعادة الحالة حسب الوضع: المسودة/الجديدة فقط تُعيد تفعيل أزرار الحفظ والترحيل.
            bool editable = IsEditable;
            btnSaveDraft.Enabled = btnPost.Enabled = editable;
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private async void btnCancelInvoice_Click(object? sender, EventArgs e)
    {
        // TASK-018 — الإلغاء يتبع وضع المستند: المرتجع بعقده (_returnApi.CancelAsync).
        if (_documentMode == PurchaseDocumentMode.PurchaseReturn)
        {
            await CancelReturnAsync();
            return;
        }

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

    // ═════════════════════════════════════════════════════════════════
    //  RETURN MODE (TASK-018) — مرتجع الشراء: عقد IPurchaseReturnDesktopService
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// حفظ/ترحيل مرتجع الشراء. لا <c>UpdateAsync</c> في العقد ⇒ المرتجع يُنشأ مرة واحدة ويُرحَّل بمعرّفه
    /// (<see cref="_createdReturnId"/> يمنع مستنداً ثانياً عند إعادة محاولة الترحيل)، ثم تُغلق الشاشة
    /// بـ<see cref="DialogResult.OK"/> كما كانت شاشة المرتجع القديمة تفعل.
    /// </summary>
    private async Task SaveReturnAsync(bool post)
    {
        // (0) حالة المستند: مرحَّل/ملغي ⇒ لا شيء؛ مسودة محفوظة ⇒ «ترحيل» فقط.
        if (_returnDoc is not null && _returnDoc.Status != (byte)InvoiceStatus.Draft)
        {
            clsDialogs.Info("المرتجع مرحّل أو ملغي — لا يمكن تعديله أو ترحيله مرة أخرى.");
            return;
        }

        if (!post && _returnDoc is not null)
        {
            clsDialogs.Info("المرتجع محفوظ كمسودة ولا يقبل التعديل — استخدم «ترحيل» لترحيله.");
            return;
        }

        // (1) الحقول المطلوبة (المورد · المستودع) — الحكم والوسم والنقل في البيت الواحد RequiredFieldHelper.
        if (!RequiredFieldHelper.Validate(this))
        {
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
            return;
        }

        // (2) قواعد الأسطر — نفس _FirstLineError المستهلكة في وضع الفاتورة (RULE-N172: لا نسخة ثانية
        // من حكم «وحدة · كمية > 0 · تكلفة > 0»).
        string? lineError = _FirstLineError();
        if (lineError is not null)
        {
            errorProvider.SetError(dgvItems, lineError);
            clsInputFeedback.ShowRejectTooltip(dgvItems, lineError, toolTip);
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
            return;
        }
        errorProvider.SetError(dgvItems, string.Empty);

        // (3) تأكيد الترحيل — الترحيل يقلّل المخزون ورصيد المورد فلا يُنفَّذ بمفتاح عابر.
        if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم تقليل المخزون وتقليل رصيد المورد.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        _isSaving = true;
        Cursor = Cursors.WaitCursor;
        try
        {
            int documentId;

            if (_createdReturnId is int existingReturnId)
            {
                // لا مستند مكرر: إن نجح الإنشاء سابقاً وفشل الترحيل، تُرحَّل المسودة نفسها.
                documentId = existingReturnId;
            }
            else
            {
                // الوحدة تُرسل من السطر كما اختارها المستخدم — **لا سقوط صامت إلى الوحدة 1**
                // (غيابها رُفض قبل الوصول إلى هنا في _FirstLineError).
                var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreatePurchaseReturnItemRequest(
                    PurchaseInvoiceLineId: 0,
                    ProductId: l.ProductId,
                    ProductUnitId: l.ProductUnitId,
                    Quantity: l.Quantity,
                    UnitCost: l.UnitCost,
                    Amount: l.LineTotal)).ToList();

                // CreatePurchaseReturnRequest.WarehouseId من نوع int (بخلاف المبيعات short).
                var request = new CreatePurchaseReturnRequest(
                    null,
                    _SelectedSupplierId,
                    _SelectedWarehouseId,
                    dtpDate.Value,
                    null,
                    null,
                    txtNotes.Text,
                    items);

                var result = await _returnApi.CreateAsync(request);
                if (!result.IsSuccess || result.Value is not { } created)
                {
                    clsDialogs.Error(result.Error ?? "تعذر حفظ مرتجع الشراء.");
                    return;
                }

                _createdReturnId = created.Id;
                _returnDoc = created;
                lblInvoiceNo.Text = created.ReturnNo.ToString();
                documentId = created.Id;

                // المستند أصبح محفوظاً ⇒ يقفل التحرير ويبقى «ترحيل» للمسودة (RULE-N150: مصدر واحد).
                UpdateUIState();
            }

            if (post)
            {
                var postResult = await _returnApi.PostAsync(documentId);
                if (!postResult.IsSuccess)
                {
                    // لا إغلاق عند فشل الترحيل: المرتجع محفوظ كمسودة، وإعادة المحاولة تُرحّل نفسه.
                    clsDialogs.Warning(
                        $"أُنشئ المرتجع كمسودة برقم {_createdReturnId} وتعذر ترحيله. " +
                        "أعد الضغط على «ترحيل» لترحيل نفس المسودة.",
                        "لم يُرحَّل المرتجع");
                    return;
                }

                clsDialogs.Success("تم الترحيل");
                foreach (var line in _lines.Where(l => l.ProductId > 0))
                    _eventBus.Publish(new StockChangedMessage(line.ProductId));
            }
            else clsDialogs.Success("تم الحفظ");

            _eventBus.Publish(new PurchaseReturnChangedMessage(documentId));
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ مرتجع المشتريات");
            clsDialogs.Error("خطأ في حفظ المرتجع. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            _isSaving = false;

            // الحفظ الناجح يغلق النموذج ويُدمّره — لا تُلمس عناصره بعد الإغلاق.
            if (!IsDisposed)
            {
                Cursor = Cursors.Default;
                UpdateUIState();
            }
        }
    }

    /// <summary>إلغاء مرتجع شراء مرحَّل — بالعقد الخاص بالمرتجعات (<c>_returnApi.CancelAsync</c>).</summary>
    private async Task CancelReturnAsync()
    {
        if (_returnDoc is null) return;
        if (!clsDialogs.Confirm("هل أنت متأكد من إلغاء المرتجع؟")) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            var result = await _returnApi.CancelAsync(_returnDoc.Id);
            if (!result.IsSuccess)
            {
                // لا نجاح وهمي — المرتجع لم يُلغَ فعلاً، تبقى الشاشة مفتوحة برسالة الخدمة العربية.
                clsDialogs.Error(result.Error ?? "تعذر إلغاء المرتجع.");
                return;
            }

            clsDialogs.Success("تم إلغاء المرتجع");
            _eventBus.Publish(new PurchaseReturnChangedMessage(_returnDoc.Id));
            foreach (var item in _returnDoc.Items)
                _eventBus.Publish(new StockChangedMessage(item.ProductId));

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء إلغاء مرتجع المشتريات");
            clsDialogs.Error("حدث خطأ أثناء الإلغاء. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// «اختيار فاتورة» (وضع المرتجع وحده): يفتح منتقي فواتير الشراء القائم ويُعيد الفاتورة بسطورها،
    /// فيُملأ رأس المرتجع (المورد والمستودع) وسطوره منها ليعدّل المستخدم الكميات ثم يُرحّل.
    /// <para>
    /// <b>لا يُمسّ تاريخ المرتجع</b> ولا ملاحظاته — تاريخ الإرجاع لا تاريخ الفاتورة. و<b>لا حساب مالي
    /// هنا</b>: كل قيمة من الـDTO كما هي، والمجاميع تُعاد بـ<see cref="CalculateTotals"/> القائم.
    /// </para>
    /// </summary>
    private void btnPickInvoice_Click(object? sender, EventArgs e)
    {
        // الزر مخفي في وضع الفاتورة واختصاراته لا تشمله — حارس صريح حتى لا يُعبَّأ مرتجع من فاتورة
        // والشاشة تعمل على مستند شراء.
        if (_documentMode != PurchaseDocumentMode.PurchaseReturn) return;
        if (_isSaving) return;

        // لا استبدال صامت: السطور ستُمسح فعلاً ⇒ تأكيد صريح أولاً (clsDialogs.Confirm افتراضه «لا»).
        if (_lines.Any(l => l.ProductId > 0) &&
            !clsDialogs.Confirm("لديك سطور في المرتجع — سيتم استبدالها ببيانات الفاتورة. هل تريد المتابعة؟"))
            return;

        try
        {
            Cursor = Cursors.WaitCursor;

            var dialog = _serviceProvider.GetRequiredService<frmPurchaseInvoiceSelection>();
            if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedInvoice is not { } invoice) return;

            // (1) الرأس: المورد والمستودع من الفاتورة (التاريخ والملاحظات كما هي).
            if (invoice.SupplierId > 0) cmbSupplier.SelectedValue = invoice.SupplierId;
            // WarehouseDto.Id من نوع short — الإسناد بقيمة int لا يطابق أي عنصر فيُفرغ المستودع.
            cmbWarehouse.SelectedValue = Convert.ToInt16(invoice.WarehouseId);

            // (2) السطور: سطر لكل بند، وسعر السطر = تكلفة الفاتورة (LineTotal = كمية × سعر − خصم)،
            // وUnitCost يُملأ أيضاً حتى يمر _FirstLineError (تكلفة > 0) في الوضعين.
            _lines.Clear();
            foreach (var item in invoice.Items)
            {
                _lines.Add(new InvoiceLineItem
                {
                    ProductId = item.ProductId,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProductUnitId = item.ProductUnitId,
                    UnitName = item.ProductUnitName,
                    Quantity = item.Quantity,
                    UnitCost = item.UnitCost,
                    UnitPrice = item.UnitCost,
                    DiscountAmount = item.DiscountAmount,
                    ConversionFactor = 1m
                });
            }

            // علامة رفض قديمة على الشبكة (من محاولة حفظ بلا سطور) لم تعد صحيحة بعد التعبئة.
            errorProvider.SetError(dgvItems, string.Empty);

            CalculateTotals();   // المسار القائم — لا جمع مالي في هذا المعالج

            if (ctrlLineEntry.Enabled) ctrlLineEntry.FocusBarcode();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تعبئة مرتجع المشتريات من الفاتورة المختارة");
            clsDialogs.Error("تعذر تعبئة المرتجع من الفاتورة المختارة. يرجى إعادة المحاولة.");
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }
}
