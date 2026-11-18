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
using SalesSystem.Desktop.Products.Forms;
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

    /// <summary>حارس إعادة الدخول على الحفظ: يمنع نقراً مزدوجاً أو تحرير سطر أثناء الإرسال.</summary>
    private bool _isSaving;

    /// <summary>
    /// معرّف المرتجع الذي أنشأته الشاشة فعلاً في محاولة سابقة. يمنع إنشاء مستند ثانٍ
    /// عندما ينجح الإنشاء ويفشل الترحيل ثم يعيد المستخدم المحاولة («ترحيل»)،
    /// فيُرحَّل المستند نفسه بدل تكرار المستند المالي. يُصفَّر في <see cref="ResetForm"/>.
    /// </summary>
    private int? _createdId;

    public frmPurchaseReturn(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // ── لوحة «إضافة صنف» أعلى الشبكة (TASK-014 · بيت واحد ctrlLineEntryPanel) ──────────────
        // الشاشة **تُسند مُفوَّداتها فقط** ولا تكتب سطراً من منطق الإدخال: الباركود والوحدات والدمج
        // وحدّ الخصم وإعادة التركيز كلها في المكوّن (RULE-N169…RULE-N172). المكوّن يعرف «كيف يُدخل
        // سطر»، والشاشة تعرف «من أين يأتي الصنف وكيف يُحسب المال».
        ctrlLineEntry.Lines = _lines;
        ctrlLineEntry.PriceCaption = "سعر التكلفة:";
        // سعر الشراء = ProductUnitDto.CostPrice. ProductDto نفسه يحمل سعري بيع/شراء مرميّين على 0m
        // (مقيس) — فالسعر يُقرأ من الوحدة حصراً وإلا مُلئ كل سطر بسعر صفر.
        ctrlLineEntry.PriceSelector = u => u.CostPrice;
        // حكم «هل يُسمح بالإدخال» بيت واحد في الشاشة: _IsEditable (العقد بلا UpdateAsync، فتحرير
        // مستند محفوظ لا يُرسَل — D-6) — فلا نسخة ثانية من الشرط هنا (RULE-N172).
        ctrlLineEntry.IsEditable = _IsEditable;
        ctrlLineEntry.ResolveBarcodeAsync = FindProductByBarcodeAsync;
        ctrlLineEntry.LoadUnitsAsync = async id =>
            (await _productApi.GetProductUnitsAsync(id)).Value ?? Array.Empty<ProductUnitDto>();
        ctrlLineEntry.PickProductAsync = PickProductAsync;
        ctrlLineEntry.TotalsChanged = CalculateTotals;

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (4 من 6).
        // RULE-N191 — Enter داخل «بيانات المرتجع» يمشي بترتيب العرض (صفاً صفاً من اليمين لليسار) ثم
        // يُركِّز زر الوضع الأساسي بلا تنفيذ (RULE-N147)؛ والحقول خارج النطاق (الملاحظات) تحتفظ
        // بسلوك SelectNextControl، وشبكة الأصناف تملك Enter بنفسها (RULE-N153).
        KeyboardHelper.WireEnterAsTab(pnlHeader, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b — لون الحقل النشط حيّاً من اللوحة
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45

        // RULE-N180…N183 — تغذية حقلي المرتجع المطلوبين حيّاً: أيقونة ✗ + رسالة + ومضة رفض تعود
        // إلى لون المصمّم عند مغادرة حقل مطلوب فارغ، وتزول فور صحّة القيمة. الحكم والوسم في البيت
        // الواحد RequiredFieldHelper — الشاشة تُعلن قواعدها فقط (RULE-N172).
        // ترتيب الاستدعاء إلزامي (RULE-N183): **بعد** WireFocusHighlight (ترتيب اشتراك Leave)
        // و**قبل** GreenThemeHelper.Apply في ApplyUiTheme (لا يلتقط مظهر التعطيل لوناً مؤقتاً).
        // الصيغة الصريحة new RequiredField(...) إلزامية — bare new(...) هنا CS8752.
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbSupplier, "يرجى اختيار المورد"),
            new RequiredField(cmbWarehouse, "يرجى اختيار المستودع"));

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnRemoveItem, btnSaveDraft, btnPost, btnClose);

        // RULE-N47 — الاختصارات الموحّدة على الأزرار الموجودة فعلاً في الشاشة.
        // F2 = «سطر صنف جديد» ⇒ إعادة المؤشر إلى باركود لوحة الإضافة (لا زر «إضافة صنف» بعد اليوم).
        // **F4 أُزيل من هنا عمداً:** WireGlobalShortcuts يعمل على Form.KeyDown مع KeyPreview فيبتلع
        // المفتاح قبل حقل الباركود، فيُقتل F4 = منتقي الأصناف في المكوّن. الترحيل يبقى على زرّه وحده.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, ctrlLineEntry.FocusBarcode),             // جديد = سطر صنف جديد
            (Keys.F3, _SaveDraftIfAllowed),                    // حفظ/عرض المسودة
            (Keys.F5, _RefreshAsync),                          // تحديث
            (Keys.Delete, _GlobalDelete),                      // حذف السطر المحدد
            (Keys.Escape, _GlobalEscape));                     // خروج

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي (يُقرأ عند كل ضغطة لا مرة واحدة).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        _UpdateAcceptButton();

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
    /// ولا يحذف والمؤشر داخل حقل إدخال (حقول لوحة «إضافة صنف») لأن Delete هناك يمسح نصاً.
    /// <para>
    /// حكم «المؤشر داخل حقل إدخال» من البيت الواحد <see cref="KeyboardHelper.IsTextInputFocused"/>
    /// بدل حارس محلي (RULE-N169).
    /// </para>
    /// </summary>
    private void _GlobalDelete()
    {
        if (KeyboardHelper.IsTextInputFocused(this)) return;

        if (dgvItems.ContainsFocus && btnRemoveItem.Enabled) btnRemoveItem.PerformClick();
    }

    /// <summary>
    /// Esc: يُلغي أولاً ما هو جارٍ (تحرير خلية · سطر في لوحة الإضافة)، وإلا يُغلق النموذج (RULE-N47).
    /// <para>
    /// <b>لماذا يُعالَج Esc هنا؟</b> <see cref="KeyboardHelper.WireGlobalShortcuts"/> يعمل على
    /// <c>Form.KeyDown</c> مع <c>KeyPreview</c> فيسبق حقول لوحة الإضافة — ولو لم يُعالَج هنا لأغلق Esc
    /// المرتجع بدل إلغاء السطر (نفس سبب إزالة F4 من قائمة الاختصارات).
    /// </para>
    /// </summary>
    private void _GlobalEscape()
    {
        if (dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            return;
        }

        // Esc داخل لوحة الإضافة أثناء تعديل سطر = إلغاء التعديل وتفريغ اللوحة — لا إغلاق للشاشة.
        if (ctrlLineEntry.ContainsFocus && ctrlLineEntry.IsEditing)
        {
            ctrlLineEntry.CancelEdit();
            return;
        }

        btnClose.PerformClick();
    }

    /// <summary>
    /// الزر الأساسي الذي ينفّذه <c>Enter</c> في الوضع الحالي (RULE-N147):
    /// مرتجع جديد أو مسودة قابلة للتحرير ⇒ «ترحيل» (ولا يُرحَّل شيء قبل تأكيد المستخدم في نافذة التأكيد)،
    /// والوضع المقروء (مرحّل/ملغي) ⇒ لا زر أساسي فلا يفعل Enter شيئاً.
    /// <para>
    /// ويُعاد <c>null</c> إلزامياً عندما يكون التركيز داخل شبكة الأصناف — أو حين تحمّل لوحة «إضافة صنف»
    /// سطراً للتعديل — حتى يبقى <c>Enter</c> إدخالاً للسطر ولا يُرحّل المرتجع وسط الإدخال
    /// (RULE-N153 / RULE-N22a). الشبكة ولوحة الإضافة ليستا حقلَي تنقل في <c>KeyboardHelper</c>.
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        // حماية شبكة إدخال السطور — قبل أي اعتبار آخر (RULE-N153).
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return null;

        // لوحة الإضافة تحمّل سطراً للتعديل: Enter هناك يُثبّت السطر ولا يُنفّذ زر الوضع.
        if (ctrlLineEntry.IsEditing) return null;

        // وضع العرض (مرتجع مرحّل أو ملغي): الشاشة للقراءة فقط ⇒ لا زر أساسي.
        if (_invoice != null && _invoice.Status != (byte)InvoiceStatus.Draft) return null;

        return btnPost is { Visible: true, Enabled: true } ? btnPost : null;
    }

    /// <summary>
    /// مزامنة الزر الافتراضي للنموذج (<c>AcceptButton</c>) مع الزر الأساسي للوضع (RULE-N147):
    /// بيت واحد للقراءة، يُستدعى عند كل انتقال حالة وعند دخول/مغادرة شبكة الأصناف (RULE-N153).
    /// </summary>
    private void _UpdateAcceptButton() => AcceptButton = _PrimaryActionButton();

    /// <summary>
    /// F3 = حفظ المسودة — لا يُفعَّل إلا من زر ظاهر ومفعَّل (RULE-N47).
    /// عند عرض مرتجع مرحَّل/ملغي يُخفى الزر، وButton.PerformClick() يفحص Enabled لا Visible
    /// ⇒ كان الاختصار يبني طلباً جديداً من سطور مرتجع مرحَّل (تكرار أثر مخزني/مالي).
    /// </summary>
    private void _SaveDraftIfAllowed()
    {
        if (btnSaveDraft is { Visible: true, Enabled: true }) btnSaveDraft.PerformClick();
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
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل النموذج");
            clsDialogs.Error("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// عند الظهور: المؤشر على باركود لوحة الإضافة — أول ما يفعله المستخدم. (<c>OnLoad</c> يسبق
    /// الظهور فلا ينجح <c>Focus</c> فيه، ولذلك يقع الطلب هنا.)
    /// </summary>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        if (ctrlLineEntry.Enabled) ctrlLineEntry.FocusBarcode();
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
                // الوحدة المحفوظة على السطر تُحمَل لتعود كما هي (لا تُستبدل بالوحدة الأساسية).
                ProductUnitId = item.ProductUnitId,
                // اسم الوحدة يُعرض في عمود «الوحدة» — لا يبقى اختيار الوحدة غير مرئي في الشبكة.
                UnitName = item.ProductUnitName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitCost,
                DiscountAmount = 0m
            });
        }
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

        // RULE-N183 — مسار إعادة الضبط يزيل أي علامة «مطلوب» موروثة من محاولة حفظ سابقة
        // (نفس نمط الشاشة المرجعية frmAddUpdateSalesInvoice — والوسم ملك RequiredFieldHelper وحده).
        RequiredFieldHelper.Clear(this);

        // مرتجع جديد ⇒ لوحة الإضافة تبدأ من باركود فارغ — فلا يبقى سطر نصف مُدخل من مستند سابق.
        ctrlLineEntry.ResetPanel();
        ctrlLineEntry.FocusBarcode();

        _UpdateModeHeader();
    }

    /// <summary>
    /// إعداد الشبكة: <b>عرض واختيار فقط</b> — الإدخال كله في لوحة «إضافة صنف» أعلى الشاشة
    /// (TASK-014)، فلا تحرير داخل الخلية ولا صف إدخال جديد هنا. أعمدة الشبكة تُقرأ من
    /// <see cref="InvoiceLineItem"/> فقط، ولا تحمل الشاشة أي منطق بحث/دمج للأصناف.
    /// </summary>
    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 250, ReadOnly = true });
        // عمود الوحدة: اللوحة تتيح اختيار وحدة الصنف، وبدون هذا العمود يبقى الاختيار غير مرئي في الشبكة.
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColUnit", HeaderText = "الوحدة", DataPropertyName = "UnitName", Width = 90, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "UnitPrice", Width = 100 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 100, ReadOnly = true });
        dgvItems.DataSource = _lines;

        // RULE-N22 — شبكة عرض: لا صف إدخال، ولا تحرير خلية، واختيار بمستوى الخلية.
        dgvItems.AllowUserToAddRows = false;
        dgvItems.ReadOnly = true;
        dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;

        // النقر المزدوج يحمّل السطر في لوحة الإضافة (نمط frmAddUpdateSalesInvoice) — لا تحرير خلية.
        dgvItems.CellDoubleClick += DgvItems_CellDoubleClick;

        // RULE-N153 — شبكة السطور تملك Enter: يُفرَّغ AcceptButton عند دخول الشبكة (فلا ينقر
        // Form.ProcessDialogKey زر «ترحيل» من داخل الإدخال) ويُعاد عند مغادرتها حسب الوضع الحالي.
        dgvItems.Enter += (s, e) => _UpdateAcceptButton();
        dgvItems.Leave += (s, e) => _UpdateAcceptButton();
    }

    /// <summary>
    /// نقر مزدوج على سطر ⇒ تحميله في لوحة «إضافة صنف» لتعديله، بـ«تحديث السطر» و<c>Esc</c> للإلغاء.
    /// حكم «هل التعديل مسموح» يأتي من <c>ctrlLineEntry.Enabled</c> نفسه الذي يضبطه
    /// <see cref="UpdateUIState"/> — فلا نسخة ثانية من شرط التحرير هنا (RULE-N172).
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
        if (cmbWarehouse.SelectedValue is short warehouseId && warehouseId > 0) dialog.WarehouseId = warehouseId;

        if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct is null)
            return Task.FromResult<PickedProduct?>(null);

        return Task.FromResult<PickedProduct?>(
            new PickedProduct(dialog.SelectedProduct, dialog.SelectedUnit));
    }

    // ── أحداث الحقول (كما كانت مربوطة في المصمّم) ────────────────────────────

    private void CalculateTotals()
    {
        // الإجمالي المعروض = مجموع السطور التي ستُرسَل فعلاً (ProductId > 0)؛ كان يجمع السطور
        // غير المرتبطة بصنف وهي تُسقط من الطلب ⇒ إجمالي على الشاشة يخالف ما يُحفظ (RULE-N167).
        decimal total = _lines.Where(x => x.ProductId > 0).Sum(x => x.LineTotal);
        lblSubTotalVal.Text = clsFormat.Money(total);
        lblTotalVal.Text = clsFormat.Money(total);
    }

    private void UpdateUIState()
    {
        // المحرر والسطور للمرتجع الجديد فقط (لا UpdateAsync في العقد ⇒ تحرير محفوظ لا يُرسَل).
        bool editable = _IsEditable();
        pnlHeader.Enabled = editable;

        // RULE-N183 — الانتقال إلى وضع العرض (مستند محفوظ مرحَّل/ملغي) يزيل علامات «مطلوب»
        // فلا تبقى أيقونة ✗ على حقل معطّل: مظهر التعطيل يملكه WireDisabledVisuals (RULE-N154)،
        // والوسم/إزالته ملك RequiredFieldHelper وحده — لا نسخة ثانية هنا (RULE-N172).
        if (!editable) RequiredFieldHelper.Clear(this);

        // الشبكة عرض/اختيار دائماً (SetupGrid): لا تُفتح للتحرير في أي وضع — الإدخال في اللوحة وحدها.
        pnlFooter.Enabled = editable;

        // لوحة «إضافة صنف» هي مسار الإدخال الوحيد: تعطيلها يقفل الإدخال كله على مستند محفوظ،
        // وResetPanel يُلغي أي سطر نصف مُدخل فلا يبقى على الشاشة سطر لا يمكن حفظه.
        ctrlLineEntry.Enabled = editable;
        if (!editable) ctrlLineEntry.ResetPanel();

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
        _UpdateAcceptButton();

        _UpdateModeHeader();
    }

    /// <summary>
    /// هل بيانات المرتجع قابلة للتحرير؟ للجديد فقط: العقد لا يحوي <c>UpdateAsync</c>، فتحرير مستند
    /// محفوظ (مسودة أو مرحَّل/ملغي) لا يمكن إرساله ⇒ يُقفل بدل أن يُعرض ولا يُحفظ.
    /// </summary>
    private bool _IsEditable() => _invoice == null;

    /// <summary>هل يمكن ترحيل المرتجع؟ الجديد يُنشأ ثم يُرحَّل، والمسودة المحفوظة تُرحَّل بمعرّفها.</summary>
    private bool _IsPostable() => _invoice?.Status is null or (byte)InvoiceStatus.Draft;

    private void btnRemoveItem_Click(object sender, EventArgs e) { if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItem line) { _lines.Remove(line); CalculateTotals(); } }

    private async void btnSaveDraft_Click(object sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object sender, EventArgs e) => await SaveInvoiceAsync(true);

    /// <summary>
    /// قواعد النطاق التي لا تُوصَف كـ«حقل مطلوب» (الحقلان المطلوبان — المورد والمستودع — يُعلنان في
    /// <c>RequiredFieldHelper.Attach</c> بالمُنشئ، والحكم والوسم هناك وحده — RULE-N172/RULE-N184):
    /// سطر واحد على الأقل للإرجاع · وحدة الصنف محدَّدة · كمية الإرجاع &gt; 0 · سعر الوحدة غير سالب ·
    /// كل سطر مرتبط بصنف مسجَّل.
    /// <para>
    /// <b>النصوص والأحكام منقولة حرفياً</b> من الفحص السابق — لا تغيير في الأثر المخزني/المالي:
    /// بلا هذا الفحص يُرسَل سطر بكمية صفر/سالب أو بكود غير محلول فيفسد المخزون ورصيد المورد بصمت.
    /// </para>
    /// </summary>
    private bool _ValidateRangeRules()
    {
        bool isValid = true;

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

        // LEP-1 (مقيس): كان الحفظ يُرسل الوحدة رقم 1 حرفياً لكل سطر — أي وحدة صنف آخر عند غياب
        // ProductUnitId. الآن الوحدة تُختار في لوحة الإضافة فعلياً، والمتبقي رفض برسالة تسمّي السطر.
        int missingUnitRow = 0;
        for (int i = 0; i < _lines.Count; i++)
        {
            if (_lines[i].ProductId > 0 && _lines[i].ProductUnitId <= 0) { missingUnitRow = i + 1; break; }
        }

        string? lineMessage = missingUnitRow > 0
            ? $"السطر {missingUnitRow}: لم تُحدَّد وحدة الصنف — أعد اختياره من لوحة الإضافة."
            : invalidQtyLine != null
                ? $"الكمية يجب أن تكون أكبر من صفر ({invalidQtyLine.ProductName})"
                : invalidPriceLine != null
                    ? $"سعر الوحدة لا يمكن أن يكون سالباً ({invalidPriceLine.ProductName})"
                    : unresolvedLine != null
                        ? $"السطر «{unresolvedLine.ProductCode}» غير مرتبط بصنف مسجَّل — صحّح الكود/الباركود أو احذف السطر"
                        : null;

        if (lineMessage is not null)
        {
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
            // الشاشة المرجعية تُقرِّب المؤشر إلى موضع الرفض؛ وكل أسباب الرفض هنا في شبكة السطور
            // ⇒ التركيز عليها (ودخولها يُفرِّغ AcceptButton — RULE-N153، فلا يبقى Enter على «ترحيل»).
            if (dgvItems.CanFocus) dgvItems.Focus();
            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
        }

        return isValid;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        // حاجز الأمان: لا يُبنى طلب جديد من مرتجع ليس مسودة (مرحَّل/ملغي) —
        // يحمي من إعادة الحفظ/الترحيل عبر الاختصار F3 أو أي مسار آخر.
        if (_invoice is not null && _invoice.Status != (byte)InvoiceStatus.Draft) return;

        // الحقول المطلوبة: الحكم والوسم في البيت الواحد RequiredFieldHelper (RULE-N172/RULE-N184) —
        // يُوسَم كل ناقص (أيقونة ✗ + ومضة رفض) ثم ينتقل المؤشر إلى أوله، بلا صندوق رسالة يحجب العمل.
        if (!RequiredFieldHelper.Validate(this))
        {
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
            return;
        }

        // قواعد السطور (وحدة الصنف/الكمية/السعر/ارتباط الصنف/سطر واحد على الأقل) تبقى في الشاشة بنصوصها الحرفية.
        if (!_ValidateRangeRules()) return;

        if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم تقليل المخزون وتقليل رصيد المورد.")) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        _isSaving = true;
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
                // **LEP-1:** الوحدة تُرسل من السطر كما اختارها المستخدم في لوحة الإضافة — لا سقوط
                // صامت إلى الوحدة رقم 1 (غيابها رُفض قبل الوصول إلى هنا في _ValidateRangeRules).
                var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreatePurchaseReturnItemRequest(
                    0, l.ProductId, l.ProductUnitId, l.Quantity, l.UnitPrice, l.LineTotal)).ToList();
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
                    // رسالة الخدمة أولاً، وبديل عربي عند غيابها — لا استدعاء بـnullable (CS8604)،
                    // وبنفس شكل الشاشة الشقيقة frmSalesReturn (RULE-N171).
                    clsDialogs.Error(result.Error ?? "تعذر حفظ مرتجع الشراء.");
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
                        "لم يُنشأ مستند جديد: أعد الضغط على «ترحيل» لترحيل نفس المسودة.\n" +
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
            _isSaving = false;
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
        btnRemoveItem.Enabled = false;
        ctrlLineEntry.Enabled = false;
        ctrlLineEntry.ResetPanel();
    }
}
