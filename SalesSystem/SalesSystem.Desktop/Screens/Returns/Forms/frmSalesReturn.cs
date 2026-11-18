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
using SalesSystem.Desktop.Screens.Returns;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Products.Forms;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

public partial class frmSalesReturn : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISalesReturnDesktopService _returnApi => _serviceProvider.GetRequiredService<ISalesReturnDesktopService>();
    private ICustomerDesktopService _customerApi => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
    private IProductDesktopService _productApi => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    // RULE-N169: بيت الرسائل الواحد هو clsDialogs — لا خدمة إشعارات ثانية داخل الشاشة.
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();

    private SalesReturnDto? _invoice;
    private BindingList<InvoiceLineItem> _lines = new();
    private List<ProductDto> _allProducts = new();

    /// <summary>
    /// معرّف المرتجع الذي أنشأته الشاشة فعلاً في محاولة سابقة. يمنع إنشاء مستند ثانٍ
    /// عندما ينجح الإنشاء ويفشل الترحيل ثم يعيد المستخدم المحاولة («ترحيل»)،
    /// فيُرحَّل المستند نفسه بدل تكرار المستند المالي. يُصفَّر في <see cref="ResetForm"/>.
    /// </summary>
    private int? _createdId;

    /// <summary>حارس إعادة الدخول على الحفظ: يمنع نقراً مزدوجاً أو تحرير سطر أثناء الإرسال.</summary>
    private bool _isSaving;

    /// <summary>
    /// معرّف المستودع المختار (0 = بلا اختيار). <c>WarehouseDto.Id</c> من نوع <c>short</c>،
    /// فالقراءة بالنمط <c>is short</c> (لا التحويل الصريح الذي يرمي) — كما في بقية الشاشات.
    /// </summary>
    private int _SelectedWarehouseId => cmbWarehouse.SelectedValue is short warehouseId ? warehouseId : 0;

    public frmSalesReturn(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // ── لوحة «إضافة صنف» أعلى الشبكة (TASK-014 · البيت الواحد ctrlLineEntryPanel) ──────────────
        // الشاشة **تُسند مُفوَّداتها فقط** ولا تكتب سطراً من منطق الإدخال: الباركود والوحدات والدمج
        // وحدّ الخصم وإعادة التركيز كلها في المكوّن (RULE-N169…RULE-N172). المكوّن يعرف «كيف يُدخل
        // سطر»، والشاشة تعرف «من أين يأتي الصنف وكيف يُحسب المال».
        ctrlLineEntry.Lines = _lines;
        ctrlLineEntry.PriceCaption = "السعر:";
        // سعر البيع من ProductUnitDto.SalePrice حصراً: ProductDto يحمل سعري بيع/شراء مرميّين على 0m
        // (مقيس) — فالسعر يُقرأ من الوحدة وإلا مُلئ كل سطر بصفر.
        ctrlLineEntry.PriceSelector = u => u.SalePrice;
        ctrlLineEntry.IsEditable = () => _invoice is null || _invoice.Status == (byte)InvoiceStatus.Draft;
        ctrlLineEntry.ResolveBarcodeAsync = FindProductByBarcodeAsync;
        ctrlLineEntry.LoadUnitsAsync = async id =>
            (await _productApi.GetProductUnitsAsync(id)).Value ?? Array.Empty<ProductUnitDto>();
        ctrlLineEntry.PickProductAsync = PickProductAsync;
        ctrlLineEntry.TotalsChanged = CalculateTotals;

        // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط (4 من 6).
        // RULE-N191/N193 — Enter داخل حقول رأس المرتجع (gbInvoiceInfo) يمشي بترتيب العرض حقلاً بحقل،
        // وبعد آخر حقل يُركِّز زر الوضع (حفظ مسودة) بلا تنفيذ. الحقول خارج النطاق (الملاحظات) تبقى على
        // SelectNextControl كما كانت، ولوحة الإضافة تحمل سلسلة Enter الخاصة بها (RULE-N40 داخل المكوّن).
        KeyboardHelper.WireEnterAsTab(gbInvoiceInfo, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45

        // RULE-N180…N183 — تغذية الحقول المطلوبة حيّاً: أيقونة ✗ + رسالة عربية + ومضة رفض تعود إلى لون
        // المصمّم تلقائياً. إلزامي: **بعد** WireFocusHighlight (ترتيب Leave يحسم مَن يرسم الخلفية أخيراً)
        // و**قبل** الثيم. الشاشة تُعلن قواعدها فقط — لا أيقونة ولا لون ولا استعادة خلفية هنا.
        // «عميل نقدي» هو العنصر النائب في القائمة بمعرّف 0 ويُرسَل CustomerId = null ⇒ اختيار مشروع،
        // والمرفوض انعدام الاختيار وحده (SelectedValue = null) — نفس شرط تحقق الشاشة السابق حرفياً.
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbCustomer, "يرجى اختيار العميل",
                () => cmbCustomer.SelectedValue is null or DBNull),
            new RequiredField(cmbWarehouse, "يرجى اختيار المستودع"));

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnRemoveItem, btnSaveDraft, btnPost, btnClose);

        // RULE-N47 — الاختصارات الموحّدة على الأزرار الموجودة فعلاً في الشاشة.
        // F2 = «سطر صنف جديد» ⇒ إعادة المؤشر إلى باركود لوحة الإضافة (لا زر «إضافة صنف» بعد اليوم).
        // **F4 أُزيل من هنا عمداً:** WireGlobalShortcuts يعمل على Form.KeyDown مع KeyPreview فيبتلع
        // المفتاح (Handled = true) قبل حقل الباركود، فيُقتل F4 = منتقي الأصناف في المكوّن. الترحيل
        // يبقى على زرّه — والمفتاح العابر لا يُرحّل مستنداً مالياً على أي حال (RULE-N153).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, ctrlLineEntry.FocusBarcode),             // جديد = سطر صنف جديد
            (Keys.F3, _SaveDraftIfAllowed),                    // حفظ/عرض المسودة
            (Keys.F5, _RefreshAsync),                          // تحديث
            (Keys.Delete, _GlobalDelete),                      // حذف السطر المحدد
            (Keys.Escape, _GlobalEscape));                     // خروج / إلغاء السطر الجاري

        // RULE-N147 — Enter يُنفِّذ الزر الأساسي للوضع الحالي (مرتجع قابل للتحرير ⇒ «حفظ مسودة»)،
        // ولا يعمل داخل الشبكة ولا ولوحة الإضافة أثناء تحرير سطر.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;

        SetupGrid();
        ApplyUiTheme();   // ينتهي بالثيم الأخضر (RULE-N50)
    }

    public void LoadData(SalesReturnDto? invoice)
    {
        // لا يُربط المرتجع بالقوائم هنا: تُستدعى LoadData قبل Show() فلم تُملأ ComboBox بعد،
        // فيُسند العميل/المستودع الخطأ ثم يُحفظ المرتجع بجهة غير جهته. الربط في OnLoad بعد LoadLookups.
        _invoice = invoice;
        if (_invoice == null) ResetForm();
        UpdateUIState();
    }

    // ── الاختصارات العامة (RULE-N47) ─────────────────────────────────────────

    /// <summary>Delete: حذف السطر المحدد — يعمل فقط عندما تكون الشبكة هي صاحبة التركيز.</summary>
    private void _GlobalDelete()
    {
        // الحكم على «حقل إدخال» من بيته الواحد KeyboardHelper.IsTextInputFocused (RULE-N169/N171):
        // Delete داخل حقل نصي (الملاحظات · المرجع · حقول لوحة الإضافة) يخصّ النص ولا يحذف سطراً.
        if (KeyboardHelper.IsTextInputFocused(this)) return;

        if (dgvItems.ContainsFocus && btnRemoveItem.Enabled) btnRemoveItem.PerformClick();
    }

    /// <summary>
    /// Esc: يُلغي أولاً ما هو جارٍ (تحرير خلية · سطر في لوحة الإضافة)، وإلا يُغلق النموذج (RULE-N47).
    /// <para>
    /// <b>لماذا تُعالَج هنا؟</b> <see cref="KeyboardHelper.WireGlobalShortcuts"/> يعمل على
    /// <c>Form.KeyDown</c> مع <c>KeyPreview</c> فيسبق حقلَ الباركود — ولو لم يُعالَج هنا لأغلق Esc
    /// الشاشة بدل إلغاء السطر الجاري (نفس سبب إزالة F4 من قائمة الاختصارات).
    /// </para>
    /// </summary>
    private void _GlobalEscape()
    {
        if (dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            return;
        }

        // Esc داخل لوحة الإضافة أثناء تعديل سطر = إلغاء التعديل وتفريغ اللوحة (نمط الشاشة المرجعية).
        if (ctrlLineEntry.ContainsFocus && ctrlLineEntry.IsEditing)
        {
            ctrlLineEntry.CancelEdit();
            return;
        }

        btnClose.PerformClick();
    }

    /// <summary>
    /// F3 = حفظ المسودة — لا يُفعَّل إلا من زر ظاهر ومفعَّل (RULE-N47).
    /// عند عرض مرتجع مرحَّل يُخفى الزر فقط وتبقى قيمته مفعّلة، وButton.PerformClick()
    /// يفحص Enabled لا Visible ⇒ كان F3 يبني طلباً جديداً من سطور مرتجع مرحَّل.
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
            int customerId = Convert.ToInt32(cmbCustomer.SelectedValue ?? 0);
            short warehouseId = Convert.ToInt16(cmbWarehouse.SelectedValue ?? 0);

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
            if (!IsDisposed) Cursor = Cursors.Default;
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

            // الترتيب إلزامي: ربط المرتجع بعد اكتمال القوائم فقط — وإلا تُسند ComboBox الفارغة
            // العميل/المستودع الخطأ فيُحفظ المرتجع على جهة غير جهته.
            if (_invoice == null) ResetForm();
            else BindInvoice();
            ApplyUiTheme();

            // RULE-N22a — الشبكة عرض/اختيار فقط: الإدخال كله في لوحة «إضافة صنف» أعلاه، فصفوفها لا
            // تُضاف من الشبكة ولا تُحرَّر داخلها (AllowUserToAddRows/ReadOnly/SelectionMode في SetupGrid).
            UpdateUIState();
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
            Cursor = Cursors.WaitCursor;                 // RULE-N26 — مؤشر انتظار أثناء جلب القوائم

            // includeInactive: مرتجع قديم لعميل/مستودع مُعطَّل يجب أن يُربط بجهته الصحيحة،
            // وإلا بقي الاختيار على أول عنصر (عميل نقدي) فحُفظ على جهة غير جهته.
            var warehouseRes = await _warehouseApi.GetAllAsync(includeInactive: true);
            if (warehouseRes.IsSuccess)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
            }

            var customerRes = await _customerApi.GetAllAsync(includeInactive: true);
            if (customerRes.IsSuccess)
            {
                var list = customerRes.Value!.ToList();
                // نص «عميل نقدي» من بيته الواحد (RULE-N169) لا حرفياً داخل الشاشة.
                list.Insert(0, new CustomerDto(0, DocumentTextHelper.CashCustomerName, "", "", "", "", 0, true, 0));
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
        // المرتجع المحمَّل مستند قائم فعلاً: معرّفه يمنع إنشاء نسخة ثانية عند «حفظ/ترحيل»،
        // فكان فتح مسودة من القائمة ثم ترحيلها يُنشئ مرتجعاً جديداً ويترك المسودة القديمة معلّقة.
        _createdId = _invoice!.Id;

        this.Text = $"مرتجع مبيعات - {_invoice.ReturnNo}";
        lblInvoiceNo.Text = _invoice.ReturnNo.ToString();
        dtpDate.Value = _invoice.ReturnDate;
        cmbCustomer.SelectedValue = _invoice.CustomerId;
        // WarehouseDto.Id من نوع short ⇒ الإسناد بـ short وإلا لم تُطابق أي عنصر وبقي المستودع بلا اختيار.
        cmbWarehouse.SelectedValue = (short)_invoice.WarehouseId;
        txtNotes.Text = _invoice.Notes;

        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItem {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                // الوحدة المحفوظة تُحمَل كما هي: بها يُعاد الترحيل، وبها يُرفض سطر بلا وحدة (LEP-1).
                ProductUnitId = item.ProductUnitId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount
            });
        }
        CalculateTotals();

        _UpdateModeHeader();
    }

    private void ResetForm()
    {
        // مرتجع جديد ⇒ لا مستند مُنشأ سابقاً يُعاد ترحيله (RULE: لا تكرار للمستند).
        _createdId = null;
        this.Text = "مرتجع مبيعات جديد";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        _lines.Clear();
        txtNotes.Clear();

        // مرتجع جديد يبدأ بلا أي علامة «مطلوب» موروثة من محاولة حفظ سابقة (RULE-N183).
        RequiredFieldHelper.Clear(this);

        // لوحة الإضافة تبدأ من باركود فارغ — فلا يبقى سطر نصف مُدخل من مستند سابق.
        ctrlLineEntry.ResetPanel();
        ctrlLineEntry.FocusBarcode();

        _UpdateModeHeader();
    }

    /// <summary>
    /// إعداد الشبكة: <b>عرض واختيار فقط</b> — الإدخال كله في لوحة «إضافة صنف» أعلى الشاشة
    /// (TASK-014)، فلا تحرير داخل الخلية ولا صف إدخال جديد هنا، ولا بحث أصناف/دمج سطور في الشبكة
    /// (حكم مالي واحد في المكوّن وحده — RULE-N172).
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

        // RULE-N22 — شبكة عرض: لا صف إدخال، ولا تحرير، واختيار بمستوى الخلية (RULE-N52: الـHelper لا يلمسها).
        dgvItems.AllowUserToAddRows = false;
        dgvItems.ReadOnly = true;
        dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvItems.EditMode = DataGridViewEditMode.EditOnEnter;

        // النقر المزدوج يحمّل السطر في لوحة الإضافة (نمط frmAddUpdateSalesInvoice) — لا تحرير في الخلية.
        dgvItems.CellDoubleClick += DgvItems_CellDoubleClick;
    }

    /// <summary>
    /// نقر مزدوج على سطر ⇒ تحميله في لوحة «إضافة صنف» لتعديله، بـ«تحديث السطر» و<c>Esc</c> للإلغاء.
    /// الحكم على «هل التعديل مسموح» يأتي من <c>ctrlLineEntry.Enabled</c> نفسه الذي يضبطه
    /// <see cref="UpdateUIState"/> — فلا نسخة ثانية من شرط «قابل للتحرير» هنا (RULE-N172).
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
    /// يحمل سعري بيع/شراء مرميّين على <c>0m</c> (مقيس)، والسعر من <see cref="ProductUnitDto.SalePrice"/>.
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
        pnlFooter.Enabled = editable;

        // لوحة «إضافة صنف» هي مسار الإدخال الوحيد: تعطيلها يقفل الإدخال كله على مستند محفوظ،
        // والـResetPanel يُلغي أي سطر نصف مُدخل فلا يبقى على الشاشة سطر لا يمكن حفظه.
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

        _UpdateModeHeader();

        // RULE-N147 — الزر الافتراضي للشاشة يتبع الوضع الحالي (يُحدَّث بعد كل انتقال حالة).
        AcceptButton = _PrimaryActionButton();
    }

    /// <summary>
    /// هل بيانات المرتجع قابلة للتحرير؟ للجديد فقط: العقد لا يحوي <c>UpdateAsync</c>، فتحرير مستند
    /// محفوظ (مسودة أو مرحَّل/ملغي) لا يمكن إرساله ⇒ يُقفل بدل أن يُعرض ولا يُحفظ.
    /// </summary>
    private bool _IsEditable() => _invoice == null;

    /// <summary>هل يمكن ترحيل المرتجع؟ الجديد يُنشأ ثم يُرحَّل، والمسودة المحفوظة تُرحَّل بمعرّفها.</summary>
    private bool _IsPostable() => _invoice?.Status is null or (byte)InvoiceStatus.Draft;

    /// <summary>
    /// الزر الأساسي للوضع الحالي (RULE-N147): مرتجع جديد أو مسودة قابلة للتحرير ⇒ «حفظ مسودة».
    /// يُرجع <c>null</c> — أي لا إجراء لـEnter — في ثلاث حالات:
    /// (1) التركيز داخل شبكة الأصناف (أو محرّر خليتها): Enter هناك تنقّل داخل الشبكة لا تنفيذ زر
    ///     (حماية إلزامية — RULE-N153/RULE-N22a)،
    /// (2) لوحة الإضافة تحمّل سطراً للتعديل: Enter هناك يُثبّت السطر ولا ينفّذ زر الوضع،
    /// (3) مرتجع محفوظ (مسودة محمَّلة أو مرحَّل أو ملغي): لا إجراء حفظ عليه، والترحيل يبقى على زرّه
    ///     لأنه يغيّر المخزون ورصيد العميل ويحتاج تأكيداً مقصوداً (RULE-N153 — الترحيل المالي لا يُربط
    ///     بـEnter إطلاقاً، مطابقةً لشاشة فاتورة البيع المرجعية).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return null;
        if (ctrlLineEntry.IsEditing) return null;
        if (!_IsEditable()) return null;

        return btnSaveDraft is { Visible: true, Enabled: true } ? btnSaveDraft : null;
    }

    /// <summary>
    /// حماية Enter داخل شبكة الأصناف: الزر الافتراضي (<see cref="Form.AcceptButton"/>) لا يُنفَّذ إطلاقاً
    /// والتركيز داخل الشبكة — Enter هناك تنقّل داخل الشبكة فقط، فلا يُنشأ مرتجع بالخطأ.
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

    private void btnRemoveItem_Click(object sender, EventArgs e)
    {
        if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItem line)
        {
            _lines.Remove(line);
            CalculateTotals();
        }
    }

    private async void btnSaveDraft_Click(object sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object sender, EventArgs e) => await SaveInvoiceAsync(true);

    /// <summary>
    /// قواعد **النطاق** فقط — الأسطر (صنف مُسند · وحدة مُحدَّدة · كمية &gt; 0 · سعر غير سالب · سطر غير مرتبط):
    /// لا حكم «حقل مطلوب فارغ» هنا إطلاقاً، ذلك ملك <see cref="RequiredFieldHelper"/> وحده (RULE-N172):
    /// العميل والمستودع يُعلنان في <c>RequiredFieldHelper.Attach</c> بالمُنشئ ويُقيَّمان **قبل** هذه الدالة
    /// في <see cref="SaveInvoiceAsync"/> — كما في شاشة فاتورة البيع المرجعية.
    /// <para>
    /// كل قاعدة هنا باقية حرفياً كما كانت (رسالةً وشرطاً): سطر واحد على الأقل · كمية &gt; 0 ·
    /// سعر غير سالب · كود بلا صنف محلول — ويُضاف إليها **الوحدة** (LEP-1).
    /// </para>
    /// </summary>
    private bool _ValidateRangeRules()
    {
        // سطر واحد على الأقل مُسند لصنف مسجَّل — وإلا لا شيء يُرجَع.
        if (!_lines.Any(l => l.ProductId > 0))
        {
            const string itemsMessage = "يجب إضافة صنف واحد على الأقل للإرجاع";
            errorProvider.SetError(dgvItems, itemsMessage);
            clsInputFeedback.ShowRejectTooltip(dgvItems, itemsMessage, toolTip);   // GAP-08b — البيت الواحد
            dgvItems.Focus();
            clsDialogs.Warning(itemsMessage);
            return false;
        }

        errorProvider.SetError(dgvItems, "");

        // LEP-1 (مقيس: 4 مواضع في 3 شاشات): كان الطلب يُرسَل بوحدة ثابتة `1` — أي وحدة صنف آخر —
        // عند غياب ProductUnitId، فيُسجَّل المرتجع بوحدة غير وحدة الصنف. الآن الوحدة تُملأ من لوحة
        // الإضافة فعلياً، والمتبقي يُرفض برسالة عربية تُسمّي السطر بدل وحدة خاطئة صامتة.
        for (int i = 0; i < _lines.Count; i++)
        {
            var unitlessLine = _lines[i];
            if (unitlessLine.ProductId <= 0 || unitlessLine.ProductUnitId > 0) continue;

            string unitMessage = $"السطر {i + 1}: لم تُحدَّد وحدة الصنف — أعد اختياره من لوحة الإضافة.";
            errorProvider.SetError(dgvItems, unitMessage);
            clsInputFeedback.ShowRejectTooltip(dgvItems, unitMessage, toolTip);
            dgvItems.Focus();
            clsDialogs.Warning(unitMessage);
            return false;
        }

        // تحقق السطور: كمية > 0 وسعر وحدة غير سالب لكل سطر مُسنَد لصنف.
        // بلا هذا الفحص يُرسَل سطر بكمية صفر/سالب فيفسد المخزون ورصيد العميل بصمت.
        var invalidQtyLine = _lines.FirstOrDefault(l => l.ProductId > 0 && l.Quantity <= 0);
        var invalidPriceLine = _lines.FirstOrDefault(l => l.ProductId > 0 && l.UnitPrice < 0);

        // سطر فيه كود/كمية بلا صنف محلول: كان يُسقط من الطلب بصمت ويُحسب في الإجمالي.
        var unresolvedLine = _lines.FirstOrDefault(l => l.ProductId <= 0 &&
            (!string.IsNullOrWhiteSpace(l.ProductCode) || l.Quantity != 0m || l.UnitPrice != 0m));

        if (invalidQtyLine == null && invalidPriceLine == null && unresolvedLine == null) return true;

        string lineMessage = invalidQtyLine != null
            ? $"الكمية يجب أن تكون أكبر من صفر ({invalidQtyLine.ProductName})"
            : invalidPriceLine != null
                ? $"سعر الوحدة لا يمكن أن يكون سالباً ({invalidPriceLine.ProductName})"
                : $"السطر «{unresolvedLine!.ProductCode}» غير مرتبط بصنف مسجَّل — صحّح الكود/الباركود أو احذف السطر";

        errorProvider.SetError(dgvItems, lineMessage);
        clsInputFeedback.ShowRejectTooltip(dgvItems, lineMessage, toolTip);
        dgvItems.Focus();
        clsDialogs.Warning(lineMessage);
        return false;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        // حاجز الأمان: لا يُبنى طلب جديد من مرتجع ليس مسودة (مرحَّل/ملغي) —
        // يحمي من إعادة الحفظ/الترحيل عبر الاختصار F3 أو أي مسار آخر.
        if (_invoice is not null && _invoice.Status != (byte)InvoiceStatus.Draft) return;

        // ── ما قبل الحفظ (تحقق + تأكيد) داخل حارس واحد عند حدّ الخطأ (RULE-N205): المستدعي async void
        // فلا يجوز أن يفلت منه استثناء يسقط التطبيق، ولا يُكرَّر الحارس داخل قواعد النطاق ──
        try
        {
            errorProvider.Clear();

            // (1) الحقول المطلوبة — الحكم والعَلم في البيت الواحد RequiredFieldHelper (RULE-N172/RULE-N184):
            //     يُوسَم كل ناقص (أيقونة ✗ + ومضة رفض) ثم ينتقل المؤشر إلى أوله.
            if (!RequiredFieldHelper.Validate(this))
            {
                if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
                return;
            }

            // (2) قواعد النطاق (الأسطر: صنف · وحدة · كمية · سعر) — باقية حرفياً في _ValidateRangeRules.
            if (!_ValidateRangeRules()) return;

            if (post && !clsDialogs.Confirm("ترحيل المرتجع؟ سيتم زيادة المخزون وتقليل رصيد العميل.")) return;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء التحقق من مرتجع المبيعات قبل الحفظ");
            clsDialogs.Error("تعذر التحقق من بيانات المرتجع. يرجى إعادة المحاولة.");
            return;
        }

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
                // الوحدة تُرسل من السطر كما اختارها المستخدم في لوحة الإضافة — **لا سقوط صامت إلى
                // الوحدة 1** (LEP-1): غيابها رُفض قبل الوصول إلى هنا في _ValidateRangeRules.
                var items = _lines.Where(l => l.ProductId > 0).Select(l => new ReturnItemRequest(
                    0, l.ProductId, l.ProductUnitId, l.Quantity, l.UnitPrice, l.LineTotal, l.DiscountAmount, 1, null)).ToList();
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

            _eventBus.Publish(new SalesReturnChangedMessage(documentId));
            this.DialogResult = DialogResult.OK; this.Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ مرتجع المبيعات");
            clsDialogs.Error("خطأ في حفظ المرتجع. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            _isSaving = false;
            Cursor = Cursors.Default;

            // الحالة تُعاد من المصدر الواحد (RULE-N150): كان تفعيل الزرين هنا يُفعّل «حفظ مسودة» وهو
            // مخفي على مرتجع محفوظ — وButton.PerformClick() يفحص Enabled ولا يفحص Visible.
            // والحارس يمنع لمس الحالة بعد نجاح الحفظ وإغلاق النموذج (Close يُدمّر النموذج غير النمطي).
            if (!IsDisposed)
            {
                // بعد أي محاولة حفظ ناجحة: اللوحة تُفرَّغ والمؤشر يعود إلى الباركود للصنف التالي.
                ctrlLineEntry.ResetPanel();
                if (ctrlLineEntry.Enabled) ctrlLineEntry.FocusBarcode();
                UpdateUIState();
            }
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    /// <summary>
    /// يقفل سطور المرتجع بعد أن أُنشئ المستند فعلاً (وفشل ترحيله): فالبيانات المعروضة على الشاشة
    /// هي بالضبط ما ستُرحّله إعادة المحاولة — لا تعديل يُخالف المسودة المحفوظة.
    /// </summary>
    private void _LockAfterCreate()
    {
        // تعطيل المكوّن يقفل حقوله كلها ويُلغي أي سطر نصف مُدخل (OnEnabledChanged داخله).
        ctrlLineEntry.Enabled = false;
        ctrlLineEntry.ResetPanel();
        btnRemoveItem.Enabled = false;
    }
}
