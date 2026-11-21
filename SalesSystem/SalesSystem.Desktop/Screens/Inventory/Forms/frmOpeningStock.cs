using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using Serilog;
using System.Data;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «بضاعة أول المدة» — مستند إدخال أرصدة افتتاحية بنمط شاشة فاتورة البيع (<c>frmAddUpdateSalesInvoice</c>):
/// العنوان ← أوامر المستند ← بيانات المستند ← إدخال صنف ← الأصناف ← الإجماليات
/// (<c>RULE-N79</c> / <c>RULE-N135…N138</c>).
/// <para>
/// التجربة: إدخال سريع متتابع — باركود ← صنف ← وحدة ← كمية ← تكلفة ← <c>Enter</c> يضيف السطر
/// ويعيد المؤشر إلى حقل الباركود؛ وبعد الترحيل تبقى الشاشة مفتوحة وتعود «مستنداً جديداً» (D4).
/// </para>
/// <para>
/// منطق الترحيل (تسوية افتتاحية + اعتمادها) محفوظ كما كان — الشاشة لا تلمس الباك-إند.
/// وكل سلوك مشترك يُستهلك من بيته الواحد: الثيم · التنقل · تغذية الحقول المطلوبة · التفقيط ·
/// تنسيق الأعمدة · الحوارات · التحقق (<c>RULE-N169…N172</c>).
/// </para>
/// </summary>
public partial class frmOpeningStock : Form
{
    // ── الحالة ──────────────────────────────────────────────────────

    private readonly IServiceScope _scope;

    private IServiceProvider ServiceProvider => _scope.ServiceProvider;

    private IProductDesktopService ProductApi => ServiceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService WarehouseApi => ServiceProvider.GetRequiredService<IWarehouseDesktopService>();
    private IInventoryAdjustmentDesktopService AdjustmentApi => ServiceProvider.GetRequiredService<IInventoryAdjustmentDesktopService>();

    /// <summary>أسطر المستند في الذاكرة (شبكة للقراءة فقط — التعديل من لوحة الإدخال، D1).</summary>
    private readonly DataTable _dtLines = new();

    /// <summary>كاش وحدات الأصناف: <c>ProductId</c> ⇒ وحداته.</summary>
    private readonly Dictionary<int, List<ProductUnitDto>> _productUnitsCache = [];

    /// <summary>الصنف المحمَّل في لوحة الإدخال للتعديل (0 = إضافة سطر جديد).</summary>
    private int _editingProductId;

    /// <summary>
    /// باركود الوحدة الأساسية للصنف المحمَّل — احتياطيّ حقل الباركود حين تكون الوحدة المختارة بلا باركود.
    /// (قرار المالك 2026-11-20: الباركود ملك الوحدة <c>ProductUnits.Barcode</c> لا الصنف؛ و<see cref="ProductDto.Barcode"/>
    /// قيمة مشتقة منه، فالمصدر هنا الوحدة نفسها.)
    /// </summary>
    private string _baseUnitBarcode = string.Empty;

    /// <summary>حارس نقر مزدوج على «حفظ وترحيل» أثناء عملية جارية.</summary>
    private bool _isSaving;

    /// <summary>نص شريط العنوان — يُحدَّث بعدّاد الأصناف (RULE-N44).</summary>
    private const string FormTitle = "بضاعة أول المدة";

    /// <summary>نص زر الإضافة الافتراضي (وضع الإضافة).</summary>
    private const string AddLineText = "إضافة السطر";

    /// <summary>نص زر الإضافة في وضع تعديل سطر محمَّل.</summary>
    private const string UpdateLineText = "تحديث السطر";

    public frmOpeningStock(IServiceScopeFactory scopeFactory)
    {
        InitializeComponent();
        _scope = scopeFactory.CreateScope();
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        KeyPreview = true;

        _SetupLinesData();

        // RULE-N193 — معالجات الحقول تُوصَّل **قبل** توصيل Enter (RULE-N191): معالج الحقل
        // (باركود · كمية · تكلفة) يستهلك Enter ويُعلن e.Handled، فيرى محرّك التنقل الحقل مستهلكاً
        // ولا ينقل التركيز أيضاً (وإلا نُفِّذ الأمران بضغطة واحدة). الأولوية للأول.
        _WireEntryEvents();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        // RULE-N191 — Enter داخل «بيانات المستند» (gbInfo) يمشي بترتيب العرض ثم يُركِّز زر الوضع بلا تنفيذ.
        // حقول لوحة الإدخال خارج النطاق فتبقى على سلوكها (Enter = بحث/تأكيد/إضافة سطر — RULE-N42).
        KeyboardHelper.WireEnterAsTab(gbInfo, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                                          // RULE-N45 — أسهم RTL

        // RULE-N43 — مظهر واضح للأزرار المعطّلة (رمادية بنص باهت) لا Enabled=false فقط.
        KeyboardHelper.WireButtonDisabledVisual(btnNew, btnSave, btnAddLine, btnCancelLine, btnClose);

        // RULE-N47 — اختصارات الشاشة: F2 جديد · F4 منتقي الأصناف داخل لوحة الإدخال / حفظ خارجها ·
        // Delete حذف السطر (الشبكة وحدها) · Esc إلغاء السطر ثم إغلاق.
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F4, _HandleF4),
            (Keys.Delete, _HandleDeleteLine),
            (Keys.Escape, _HandleEscape));

        // RULE-N147/N153 — Enter = الزر الأساسي للوضع الحالي، ولا يعمل داخل شبكة السطور.
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);

        // RULE-N180…N183 — تغذية الحقول المطلوبة حيّاً (أيقونة ✗ + الرسالة العربية + ومضة الرفض).
        // موضع الاستدعاء إلزامي: **بعد** WireFocusHighlight (RULE-N183 — ترتيب اشتراك Leave يحسم مَن
        // يرسم الخلفية أخيراً) و**قبل** GreenThemeHelper.Apply في ApplyUiTheme أدناه.
        // الشاشة تُعلن قواعدها فقط — لا حكم «فارغ» ولا لون رفض داخل هذا الملف (RULE-N172/N181).
        RequiredFieldHelper.Attach(this, errorProvider,
            new RequiredField(cmbWarehouse, "يرجى اختيار المخزن"));

        // RULE-N153 — شبكة السطور تملك Enter: يُفرَّغ الزر الافتراضي داخلها ويُعاد عند مغادرتها.
        dgvLines.Enter += (_, _) => AcceptButton = _PrimaryActionButton();
        dgvLines.Leave += (_, _) => AcceptButton = _PrimaryActionButton();
        dgvLines.SelectionChanged += (_, _) => _UpdateButtonsState();

        // RULE-N97 — التفقيط الحيّ: كمية وتكلفة السطر + إجمالي المستند.
        LiveTafqeetHelper.Attach(liveTip,
            (txtQuantity, "الكمية", LiveTafqeetHelper.ReadMode.Quantity),
            (txtUnitCost, "تكلفة الوحدة", LiveTafqeetHelper.ReadMode.Money),
            (txtTotalCost, "إجمالي التكلفة", LiveTafqeetHelper.ReadMode.Money));

        ApplyUiTheme();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    }

    // ═════════════════════════════════════════════════════════════════
    //  الثيم (RULE-N50…RULE-N54)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>الثيم الموحّد — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvLines);

        // RULE-N134/N179 — تنسيق الأعمدة وقت التشغيل من البيت الواحد (يتبع إعدادات التنسيق الحيّة).
        GridFormatHelper.ApplyColumn(dgvLines, "colQuantity", GridFormatKind.Quantity);
        GridFormatHelper.ApplyColumns(dgvLines, GridFormatKind.Money, "colUnitCost", "colTotalCost");

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═════════════════════════════════════════════════════════════════
    //  الزر الأساسي + آلة الحالة (RULE-N43/N44/N147/N153)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// RULE-N147 — الزر الأساسي للوضع الحالي: «حفظ وترحيل» (زر حقيقي من المصمّم، وحالته يملكها
    /// <see cref="_UpdateButtonsState"/> فيتبعها <c>AcceptButton</c>).
    /// <para>
    /// <b>RULE-N153:</b> يرجع <c>null</c> عندما يكون التركيز داخل شبكة السطور (<c>dgvLines</c>) أو خليتها
    /// في وضع التحرير، فلا يُرحَّل المستند من داخل الشبكة أبداً — الترحيل المالي يبقى على زرّه و<kbd>F4</kbd>.
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvLines.ContainsFocus || dgvLines.IsCurrentCellInEditMode) return null;
        return _isSaving ? null : btnSave;
    }

    /// <summary>مؤشر الوضع: مستند جديد + عدد الأصناف المدخلة (RULE-N44).</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = $"{FormTitle} — مستند جديد (عدد الأصناف: {_dtLines.Rows.Count})";
    }

    /// <summary>
    /// حالة الأزرار وقوائم السياق: «حفظ» متاح دائماً (التحقق هو مَن يشرح النقص)،
    /// و«إلغاء السطر» في وضع التعديل فقط، وتحتيّ الشبكة على سطر محدَّد فقط.
    /// <para>
    /// <b>قفل إعادة الدخول:</b> ما دام المستند قيد الحفظ/الترحيل (<see cref="_isSaving"/>) تُعطَّل
    /// <b>كل</b> الأزرار وعناصر قائمة السياق التي يمكن أن تُعدّل المستند أو تُغادره — كانت الحلقة
    /// تنتظر <c>AddLineAsync</c> بينما <kbd>F2</kbd> يُفرّغ <c>_dtLines.Rows</c> و<kbd>Esc</kbd> يُغلق
    /// الشاشة فيُفسد التعداد المفتوح وتضيع رسالة العدد العربية الحقيقية.
    /// </para>
    /// </summary>
    private void _UpdateButtonsState()
    {
        bool hasSelectedLine = _dtLines.Rows.Count > 0 && dgvLines.CurrentRow is { IsNewRow: false };
        bool isIdle = !_isSaving;

        btnSave.Enabled = isIdle;
        btnNew.Enabled = isIdle;
        btnClose.Enabled = isIdle;
        btnPickProduct.Enabled = isIdle;
        btnAddLine.Enabled = isIdle;
        btnCancelLine.Enabled = isIdle && _editingProductId > 0;
        miEditLine.Enabled = isIdle && hasSelectedLine;
        miRemoveLine.Enabled = isIdle && hasSelectedLine;

        AcceptButton = _PrimaryActionButton();
    }

    // ═════════════════════════════════════════════════════════════════
    //  بيانات الشبكة + الإجماليات
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// جدول أسطر المستند — أسماء الأعمدة تطابق <c>DataPropertyName</c> في المصمّم،
    /// والقراءة منها لا من خلايا الشبكة (<c>RULE-N179</c>).
    /// </summary>
    private void _SetupLinesData()
    {
        _dtLines.Columns.Add("RowNum", typeof(int));
        _dtLines.Columns.Add("ProductId", typeof(int));
        _dtLines.Columns.Add("ProductUnitId", typeof(int));
        _dtLines.Columns.Add("Barcode", typeof(string));
        _dtLines.Columns.Add("ProductName", typeof(string));
        _dtLines.Columns.Add("UnitName", typeof(string));
        _dtLines.Columns.Add("Quantity", typeof(decimal));
        _dtLines.Columns.Add("UnitCost", typeof(decimal));
        _dtLines.Columns.Add("TotalCost", typeof(decimal));

        dgvLines.DataSource = new DataView(_dtLines);
    }

    /// <summary>إعادة حساب الأسطر (الترقيم + إجمالي السطر) وشريط الإجماليات وشريط الوضع.</summary>
    private void _UpdateTotals()
    {
        decimal totalQuantity = 0m;
        decimal totalCost = 0m;
        int rowNumber = 0;

        foreach (DataRow row in _dtLines.Rows)
        {
            decimal quantity = _DecimalOf(row, "Quantity");
            decimal unitCost = _DecimalOf(row, "UnitCost");
            decimal lineTotal = quantity * unitCost;

            row["RowNum"] = ++rowNumber;
            row["TotalCost"] = lineTotal;

            totalQuantity += quantity;
            totalCost += lineTotal;
        }

        lblItemCountValue.Text = clsFormat.Number(_dtLines.Rows.Count);
        lblTotalQtyValue.Text = clsFormat.Quantity(totalQuantity);
        txtTotalCost.Text = clsFormat.Money(totalCost);
        lblTafqitValue.Text = totalCost > 0m ? ArabicNumberToWords.ToWords(totalCost) : string.Empty;

        _UpdateButtonsState();
        _UpdateModeHeader();
    }

    /// <summary>
    /// قراءة عشرية من سطر جدول المستند: الخلية الفارغة (<c>DBNull</c>) تُقرأ صفراً بلا رمي —
    /// <c>Convert.ToDecimal</c> عليها كانت تُسقط الشاشة لو فرّغ المستخدم كمية أو تكلفة.
    /// </summary>
    private static decimal _DecimalOf(DataRow row, string column)
        => row[column] is decimal value ? value : 0m;

    /// <summary>سطر المستند الخاص بصنف ما (سطر واحد لكل صنف — D2)، أو <c>null</c>.</summary>
    private DataRow? _FindLineRow(int productId)
        => _dtLines.AsEnumerable().FirstOrDefault(r => r.Field<int>("ProductId") == productId);

    // ═════════════════════════════════════════════════════════════════
    //  التحميل
    // ═════════════════════════════════════════════════════════════════

    private async void frmOpeningStock_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadWarehousesAsync();

            dtpDate.Value = DateTime.Today;
            _UpdateTotals();

            txtBarcode.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل شاشة بضاعة أول المدة");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل الشاشة. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task _LoadWarehousesAsync()
    {
        var result = await WarehouseApi.GetAllAsync();

        if (!result.IsSuccess || result.Value == null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة المخازن.");
            return;
        }

        // سطر نائب بمعرّف صفر: يجعل «المخزن» حقلاً مطلوباً حقيقياً (البيت الواحد RequiredFieldHelper
        // يعدّ الصفر «فارغاً» — RULE-N181)، فلا يُختار مخزن بالنيابة عن المستخدم.
        var warehouses = new List<WarehouseDto> { new(0, "— اختر المخزن —", null, null, null, true) };
        warehouses.AddRange(result.Value);

        cmbWarehouse.DisplayMember = "Name";
        cmbWarehouse.ValueMember = "Id";
        cmbWarehouse.DataSource = warehouses;
        cmbWarehouse.SelectedIndex = 0;
    }

    // ═════════════════════════════════════════════════════════════════
    //  لوحة الإدخال — التوصيل
    // ═════════════════════════════════════════════════════════════════

    private void _WireEntryEvents()
    {
        txtBarcode.KeyDown += txtBarcode_KeyDown;
        txtQuantity.KeyDown += txtQuantity_KeyDown;
        txtUnitCost.KeyDown += txtUnitCost_KeyDown;

        // قرار المالك 2026-11-20: الباركود ملك الوحدة — تغيير الوحدة يتبع حقلَ الباركود بباركود الوحدة الجديدة.
        cmbUnit.SelectedIndexChanged += (_, _) => _SyncBarcodeField();

        txtQuantity.TextChanged += (_, _) => _RecalculateLineTotal();
        txtUnitCost.TextChanged += (_, _) => _RecalculateLineTotal();
    }

    /// <summary>الكمية × تكلفة الوحدة — معاينة إجمالي السطر قبل إضافته.</summary>
    private void _RecalculateLineTotal()
    {
        decimal quantity = clsValidation.TryParseDecimal(txtQuantity.Text, out decimal q) ? q : 0m;
        decimal unitCost = clsValidation.TryParseDecimal(txtUnitCost.Text, out decimal c) ? c : 0m;

        lblLineTotal.Text = clsFormat.Money(quantity * unitCost);
    }

    // ═════════════════════════════════════════════════════════════════
    //  لوحة الإدخال — الباركود والمنتقي
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// RULE-N42 — Enter في الباركود = بحث فوري: باركود معروف ⇒ تحميل الصنف؛ فارغ أو غير معروف ⇒
    /// منتقي الأصناف. و<see cref="KeyEventArgs.Handled"/> إلزامي (RULE-N191/N193) حتى لا ينقل
    /// محرّك التنقل التركيز أيضاً.
    /// </summary>
    private async void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        if (_isSaving) return;      // قفل إعادة الدخول — لا بحث/منتقي أثناء الحفظ

        e.Handled = true;
        e.SuppressKeyPress = true;

        try
        {
            await _ProcessBarcodeAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء معالجة باركود الصنف");
            Cursor = Cursors.Default;
            clsDialogs.Error("حدث خطأ أثناء البحث عن الصنف. يرجى إعادة المحاولة.");
        }
    }

    private async Task _ProcessBarcodeAsync()
    {
        string barcode = txtBarcode.Text.Trim();

        if (string.IsNullOrWhiteSpace(barcode))
        {
            await _PickProductAsync();
            return;
        }

        ProductDto? product = null;
        BarcodeResolutionDto? resolution = null;

        Cursor = Cursors.WaitCursor;
        try
        {
            // قرار المالك 2026-11-20: الباركود ملك الوحدة (ProductUnits.Barcode) لا الصنف.
            // يُحلّ الباركود إلى صنف + وحدة، ثم يُحمَّل الصنف بمعرّفه وتُختار الوحدة المحلولة.
            var resolved = await ProductApi.ResolveBarcodeAsync(barcode);
            if (resolved.IsSuccess) resolution = resolved.Value;

            if (resolution != null)
            {
                var productResult = await ProductApi.GetByIdAsync(resolution.ProductId);
                if (productResult.IsSuccess) product = productResult.Value;
            }
        }
        finally
        {
            Cursor = Cursors.Default;
        }

        if (product != null)
        {
            // وحدة الباركود الممسوح تُختار في قائمة الوحدة، وحقل الباركود يعرض باركودها المعتمد (لا نص المسح الخام).
            await _LoadProductForEntryAsync(product, resolution?.ProductUnitId);
            return;
        }

        // باركود غير معروف ⇒ منتقي الأصناف (نفس سلوك فاتورة البيع)
        await _PickProductAsync();
    }

    /// <summary>
    /// حقل الباركود يتبع الوحدة المختارة: باركود الوحدة، فإن كانت بلا باركود فباركود الوحدة الأساسية
    /// (نفس <see cref="ProductDto.Barcode"/> المشتق، لكن من مصدره — الوحدة نفسها)، ثم فراغ.
    /// يُستدعى عند تحميل صنف وعند كل تغيير للوحدة (<c>RULE-N42</c> — قرار المالك 2026-11-20).
    /// </summary>
    private void _SyncBarcodeField()
    {
        string barcode = (cmbUnit.SelectedItem as ProductUnitDto)?.Barcode?.Trim() ?? string.Empty;
        txtBarcode.Text = barcode.Length > 0 ? barcode : _baseUnitBarcode;
    }

    /// <summary>باركود الوحدة الأساسية من وحدات الصنف المحمَّلة (وإلا أول وحدة) — بلا استدعاء خدمة إضافي.</summary>
    private static string _BaseBarcodeOf(IReadOnlyList<ProductUnitDto> units)
        => (units.FirstOrDefault(u => u.IsBaseUnit) ?? units.FirstOrDefault())?.Barcode?.Trim() ?? string.Empty;

    /// <summary>
    /// منتقي الأصناف (D5) — <b>دون تمرير المخزن عمداً:</b> بضاعة أول المدة هي مَن يُثبّت أول كمية،
    /// و<see cref="frmProductSelection"/> يرفض الأصناف التي رصيدها صفر في مخزن مُمرَّر — فيُحجب
    /// بالضبط الصنف الذي جاء المستخدم ليُدخله. المخزن يُختار في «بيانات المستند» ويُستعمل عند الترحيل فقط.
    /// </summary>
    private async Task _PickProductAsync()
    {
        var dialog = ServiceProvider.GetRequiredService<frmProductSelection>();

        if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct == null)
        {
            txtBarcode.Focus();
            txtBarcode.SelectAll();
            return;
        }

        var product = dialog.SelectedProduct;

        await _LoadProductForEntryAsync(product, dialog.SelectedUnit?.Id);
    }

    private async void btnPickProduct_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول

        try
        {
            await _PickProductAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء اختيار صنف من القائمة");
            clsDialogs.Error("حدث خطأ أثناء اختيار الصنف. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// تحميل صنف إلى لوحة الإدخال: إن كان مضافاً مسبقاً يُحمَّل سطره للتعديل برسالة معلومات (D2 —
    /// لا دمج صامت ولا تكرار)؛ وإلا تُختار الوحدة (وحدة الباركود الممسوح، أو المختارة من المنتقي، أو
    /// الأساسية — <c>preferredUnitId</c>) ويُعبَّأ باركود تلك الوحدة والكمية = 1 ويستقر المؤشر على الكمية.
    /// </summary>
    private async Task _LoadProductForEntryAsync(ProductDto product, int? preferredUnitId)
    {
        var existing = _FindLineRow(product.Id);

        if (existing != null)
        {
            if (_editingProductId == product.Id)
            {
                // السطر نفسه محمَّل للتعديل بالفعل — لا إعادة تحميل ولا رسالة
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return;
            }

            await _LoadLineForEditingAsync(existing);
            clsDialogs.Info("هذا الصنف مضاف مسبقاً — تم تحميل السطر للتعديل.");
            return;
        }

        // مسح صنف جديد أثناء تعديل سطر آخر ⇒ إنهاء التعديل أولاً (لا دمج ولا فقدان صامت)
        if (_editingProductId > 0) _EndLineEdit();

        var units = await _GetUnitsAsync(product.Id);
        if (units.Count == 0)
        {
            // بلا وحدات لا يمكن تحديد الوحدة ⇒ سطر لا يُحفظ. كان يُضاف سطر بوحدة صفرية فيرفضه التحقق لاحقاً.
            clsDialogs.Warning("تعذر تحميل وحدات هذا الصنف — لا يمكن إضافته. راجع وحدات الصنف في دليل الأصناف.");
            _ClearEntryPanel();
            txtBarcode.Focus();
            return;
        }

        var unitToSelect = units.FirstOrDefault(u => u.Id == preferredUnitId)
                           ?? units.FirstOrDefault(u => u.IsBaseUnit)
                           ?? units[0];

        _baseUnitBarcode = _BaseBarcodeOf(units);

        lblProductName.Text = product.Name;
        cmbUnit.Tag = product.Id;
        cmbUnit.DisplayMember = "UnitName";
        cmbUnit.ValueMember = "Id";
        cmbUnit.DataSource = new List<ProductUnitDto>(units);
        cmbUnit.SelectedValue = unitToSelect.Id;

        _SyncBarcodeField();   // باركود الوحدة المختارة (قرار المالك 2026-11-20 — لا باركود للصنف)

        txtQuantity.Text = "1";
        txtUnitCost.Clear();
        _RecalculateLineTotal();

        txtQuantity.Focus();
        txtQuantity.SelectAll();
    }

    private async Task<List<ProductUnitDto>> _GetUnitsAsync(int productId)
    {
        if (_productUnitsCache.TryGetValue(productId, out var cached)) return cached;

        var result = await ProductApi.GetProductUnitsAsync(productId);
        if (!result.IsSuccess || result.Value == null)
        {
            Log.Warning("تعذر تحميل وحدات الصنف {ProductId}: {Error}", productId, result.Error);
            return [];
        }

        var units = result.Value.ToList();
        _productUnitsCache[productId] = units;
        return units;
    }

    // ═════════════════════════════════════════════════════════════════
    //  لوحة الإدخال — التنقل بين الحقول
    // ═════════════════════════════════════════════════════════════════

    /// <summary>Enter في الكمية ⇒ تحقق ثم الانتقال إلى تكلفة الوحدة.</summary>
    private void txtQuantity_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        if (!clsValidation.IsPositiveDecimal(txtQuantity.Text)) // GAP-08b — بيت التحقق الواحد (RULE-N169)
        {
            clsDialogs.Warning("الرجاء إدخال كمية صحيحة (أكبر من صفر).");
            txtQuantity.Focus();
            txtQuantity.SelectAll();
            return;
        }

        txtUnitCost.Focus();
        txtUnitCost.SelectAll();
    }

    /// <summary>Enter في تكلفة الوحدة ⇒ إضافة/تحديث السطر ثم العودة إلى الباركود.</summary>
    private void txtUnitCost_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        if (_isSaving) return;      // قفل إعادة الدخول — لا تعديل أسطر أثناء الحفظ

        e.Handled = true;
        e.SuppressKeyPress = true;

        _AddOrUpdateLine();
    }

    // ═════════════════════════════════════════════════════════════════
    //  لوحة الإدخال — التحقق (بنمط ValidateEntryFields في فاتورة البيع)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// تحقق لوحة الإدخال — نفس نمط <c>ValidateEntryFields</c> في <c>frmAddUpdateSalesInvoice</c>:
    /// صنف محدَّد · وحدة محدَّدة · كمية &gt; 0 (<see cref="clsValidation.IsPositiveDecimal"/>) ·
    /// تكلفة صالحة ≥ 0 (<see cref="clsValidation.TryParseDecimal"/>) — كل فشل = رسالة عربية + تركيز + تحديد.
    /// </summary>
    private bool _ValidateEntryFields()
    {
        if (cmbUnit.Tag is not int productId || productId <= 0)
        {
            clsDialogs.Warning("الرجاء اختيار منتج أولاً (مسح باركود أو F4).");
            txtBarcode.Focus();
            txtBarcode.SelectAll();
            return false;
        }

        if (cmbUnit.SelectedValue is not int unitId || unitId <= 0)
        {
            clsDialogs.Warning("الرجاء اختيار الوحدة.");
            cmbUnit.Focus();
            return false;
        }

        if (!clsValidation.IsPositiveDecimal(txtQuantity.Text))
        {
            clsDialogs.Warning("الرجاء إدخال كمية صحيحة (أكبر من صفر).");
            txtQuantity.Focus();
            txtQuantity.SelectAll();
            return false;
        }

        if (!clsValidation.TryParseDecimal(txtUnitCost.Text, out decimal unitCost) || unitCost < 0m)
        {
            clsDialogs.Warning("الرجاء إدخال تكلفة صحيحة (صفر أو أكثر).");
            txtUnitCost.Focus();
            txtUnitCost.SelectAll();
            return false;
        }

        return true;
    }

    // ═════════════════════════════════════════════════════════════════
    //  لوحة الإدخال — إضافة/تحديث/إلغاء السطر
    // ═════════════════════════════════════════════════════════════════

    private void btnAddLine_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول
        _AddOrUpdateLine();
    }

    private void btnCancelLine_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول

        _EndLineEdit();
        txtBarcode.Focus();
    }

    /// <summary>
    /// إضافة سطر جديد أو تحديث السطر المحمَّل، ثم — طلب المالك — تفريغ لوحة الإدخال وإعادة المؤشر
    /// إلى حقل الباركود (إدخال سريع متتابع).
    /// </summary>
    private void _AddOrUpdateLine()
    {
        if (!_ValidateEntryFields()) return;

        int productId = (int)cmbUnit.Tag!;
        int productUnitId = (int)cmbUnit.SelectedValue!;
        decimal quantity = clsValidation.TryParseDecimal(txtQuantity.Text, out decimal parsedQuantity) ? parsedQuantity : 0m;
        decimal unitCost = clsValidation.TryParseDecimal(txtUnitCost.Text, out decimal parsedCost) ? parsedCost : 0m;

        var row = _FindLineRow(_editingProductId > 0 ? _editingProductId : productId);

        if (row == null)
        {
            _dtLines.Rows.Add(
                0,                                                    // RowNum — يُرقَّم في _UpdateTotals
                productId,
                productUnitId,
                txtBarcode.Text.Trim(),
                lblProductName.Text,
                cmbUnit.Text,
                quantity,
                unitCost,
                0m);                                                  // TotalCost — يُحسب في _UpdateTotals
        }
        else
        {
            // باركود السطر المحفوظ يبقى كما هو: في وضع التعديل قد يكون المستخدم كتب نصاً آخر
            // في حقل الباركود بلا Enter، فلا يُكتب فوق باركود وحدة السطر المحمَّل (يتغيّر فقط عبر
            // بحث باركود ناجح أو تغيير الوحدة أو منتقي الأصناف — وكلها تُعيد تحميل/مزامنة السطر).
            row["ProductId"] = productId;
            row["ProductUnitId"] = productUnitId;
            row["ProductName"] = lblProductName.Text;
            row["UnitName"] = cmbUnit.Text;
            row["Quantity"] = quantity;
            row["UnitCost"] = unitCost;
        }

        _EndLineEdit();
        _UpdateTotals();
        txtBarcode.Focus();
    }

    /// <summary>خروج وضع التعديل: تفريغ اللوحة + إعادة نص زر الإضافة + رفع تظليل السطر.</summary>
    private void _EndLineEdit()
    {
        _editingProductId = 0;
        _ClearEntryPanel();
        btnAddLine.Text = AddLineText;
        _RefreshEditingRowHighlight();
        _UpdateButtonsState();
    }

    private void _ClearEntryPanel()
    {
        // يُفرَّغ قبل إسقاط مصدر الوحدة: معالج تغيير الوحدة يعيد كتابة حقل الباركود من هذا الاحتياطي.
        _baseUnitBarcode = string.Empty;

        txtBarcode.Clear();
        lblProductName.Text = string.Empty;
        cmbUnit.DataSource = null;
        cmbUnit.Tag = null;
        txtQuantity.Text = "1";
        txtUnitCost.Clear();
        lblLineTotal.Text = clsFormat.Money(0m);

        liveTip.Hide(pnlEntry);   // تفقيط حيّ — إخفاء التلميح عند بدء سطر جديد
    }

    /// <summary>
    /// تظليل السطر المحمَّل للتعديل بلون التحديد الحيّ من اللوحة، وإعادة وراثة نمط بقية الأسطر
    /// (<c>Color.Empty</c> — <c>RULE-N179</c>) فلا يبقى لون سطر بعد نهاية التعديل.
    /// </summary>
    private void _RefreshEditingRowHighlight()
    {
        foreach (DataGridViewRow gridRow in dgvLines.Rows)
        {
            bool isEditing = _editingProductId > 0
                && gridRow.DataBoundItem is DataRowView view
                && view.Row.Field<int>("ProductId") == _editingProductId;

            gridRow.DefaultCellStyle.BackColor = isEditing ? GreenThemeHelper.SelectionBack : Color.Empty;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  الشبكة — تحميل سطر للتعديل + حذف سطر
    // ═════════════════════════════════════════════════════════════════

    private void dgvLines_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول
        _EditSelectedLine();
    }

    private void miEditLine_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول
        _EditSelectedLine();
    }

    private void miRemoveLine_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول
        _DeleteSelectedLine();
    }

    private void miPickProduct_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول
        btnPickProduct.PerformClick();
    }

    /// <summary>حدّ واحد لمعالجة خطأ تحميل السطر (RULE-N205) — يُستهلك من النقر المزدوج وقائمة السياق.</summary>
    private async void _EditSelectedLine()
    {
        if (_isSaving) return;      // قفل إعادة الدخول
        if (dgvLines.CurrentRow?.DataBoundItem is not DataRowView view) return;

        Cursor = Cursors.WaitCursor;
        try
        {
            await _LoadLineForEditingAsync(view.Row);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل السطر للتعديل");
            clsDialogs.Error("حدث خطأ أثناء تحميل السطر. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>تحميل سطر المستند في لوحة الإدخال (D1 — الشبكة للقراءة فقط).</summary>
    private async Task _LoadLineForEditingAsync(DataRow row)
    {
        int productId = row.Field<int>("ProductId");
        var units = await _GetUnitsAsync(productId);

        _editingProductId = productId;
        _baseUnitBarcode = _BaseBarcodeOf(units);

        lblProductName.Text = row["ProductName"]?.ToString() ?? string.Empty;

        cmbUnit.Tag = productId;
        cmbUnit.DisplayMember = "UnitName";
        cmbUnit.ValueMember = "Id";
        cmbUnit.DataSource = new List<ProductUnitDto>(units);

        int unitId = row["ProductUnitId"] is int savedUnitId ? savedUnitId : 0;
        if (units.Any(u => u.Id == unitId)) cmbUnit.SelectedValue = unitId;
        else if (cmbUnit.Items.Count > 0) cmbUnit.SelectedIndex = 0;

        // باركود السطر المحفوظ يبقى كما حُفظ (قد يكون نصاً كتبه المستخدم) — ويُتبع بباركود الوحدة عند تغييرها.
        txtBarcode.Text = row["Barcode"]?.ToString() ?? string.Empty;

        txtQuantity.Text = _DecimalOf(row, "Quantity").ToString("G");
        txtUnitCost.Text = _DecimalOf(row, "UnitCost").ToString("G");
        _RecalculateLineTotal();

        btnAddLine.Text = UpdateLineText;
        _RefreshEditingRowHighlight();
        _UpdateButtonsState();

        txtBarcode.Focus();
        txtBarcode.SelectAll();
    }

    /// <summary>حذف السطر المحدد بعد تأكيد (قائمة السياق · <kbd>Delete</kbd> على الشبكة).</summary>
    private void _DeleteSelectedLine()
    {
        if (_isSaving) return;      // قفل إعادة الدخول — لا حذف أسطر داخل حلقة الترحيل

        if (dgvLines.CurrentRow?.DataBoundItem is not DataRowView view)
        {
            clsDialogs.Info("اختر سطراً من الجدول أولاً.");
            return;
        }

        int productId = view.Row.Field<int>("ProductId");
        if (!clsDialogs.Confirm("هل أنت متأكد من حذف هذا السطر من المستند؟")) return;

        bool wasEditing = _editingProductId == productId;
        _dtLines.Rows.Remove(view.Row);

        if (wasEditing) _EndLineEdit();

        _UpdateTotals();
    }

    // ═════════════════════════════════════════════════════════════════
    //  اختصارات الشاشة
    // ═════════════════════════════════════════════════════════════════

    /// <summary>F4 — داخل لوحة الإدخال: منتقي الأصناف؛ خارجها: حفظ وترحيل.</summary>
    private void _HandleF4()
    {
        if (_isSaving) return;      // قفل إعادة الدخول — لا منتقي ولا حفظ ثانٍ

        if (gbEntry.ContainsFocus)
        {
            if (btnPickProduct.Enabled) btnPickProduct.PerformClick();
            return;
        }

        if (btnSave.Enabled) btnSave.PerformClick();
    }

    /// <summary>Delete — حذف السطر المحدد، ولا يُسحب من أي حقل إدخال (RULE-N47).</summary>
    private void _HandleDeleteLine()
    {
        if (_isSaving) return;      // قفل إعادة الدخول
        if (!dgvLines.ContainsFocus) return;
        _DeleteSelectedLine();
    }

    /// <summary>Esc — إلغاء تعديل السطر إن كان جارياً، وإلا إغلاق الشاشة بعد تأكيد فقدان غير المرحَّل.</summary>
    private void _HandleEscape()
    {
        if (_isSaving) return;      // قفل إعادة الدخول — لا إغلاق أثناء الترحيل

        if (_editingProductId > 0)
        {
            _EndLineEdit();
            txtBarcode.Focus();
            return;
        }

        _CloseScreen();
    }

    // ═════════════════════════════════════════════════════════════════
    //  مستند جديد / إغلاق
    // ═════════════════════════════════════════════════════════════════

    private void btnNew_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;      // قفل إعادة الدخول — لا تفريغ للمستند داخل حلقة الترحيل

        if (_dtLines.Rows.Count > 0
            && !clsDialogs.Confirm("هل تريد بدء مستند جديد؟ سيتم فقدان الأصناف غير المرحّلة."))
            return;

        _ResetDocument(keepWarehouse: true);
    }

    private void btnClose_Click(object? sender, EventArgs e) => _CloseScreen();

    private void _CloseScreen()
    {
        if (_isSaving) return;      // قفل إعادة الدخول — لا إغلاق/تخلّص من النطاق أثناء الترحيل

        if (_dtLines.Rows.Count > 0
            && !clsDialogs.Confirm("سيتم إغلاق الشاشة وفقدان الأصناف غير المرحّلة. هل تريد المتابعة؟"))
            return;

        Close();
    }

    /// <summary>
    /// D4 — إعادة الشاشة إلى «مستند جديد» على نفس النموذج المفتوح: تفريغ الأسطر ولوحة الإدخال،
    /// إبقاء المخزن المختار، تاريخ اليوم، شريط الوضع «مستند جديد (عدد الأصناف: 0)»، والمؤشر على الباركود.
    /// </summary>
    private void _ResetDocument(bool keepWarehouse)
    {
        object? selectedWarehouse = keepWarehouse ? cmbWarehouse.SelectedValue : null;

        _editingProductId = 0;
        _dtLines.Rows.Clear();
        _ClearEntryPanel();
        btnAddLine.Text = AddLineText;

        dtpDate.Value = DateTime.Today;
        if (keepWarehouse && selectedWarehouse is short warehouseId)
            cmbWarehouse.SelectedValue = warehouseId;

        // لا تبقى علامة «مطلوب» موروثة من محاولة حفظ سابقة (RULE-N183).
        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);

        _UpdateTotals();
        txtBarcode.Focus();
    }

    // ═════════════════════════════════════════════════════════════════
    //  التحقق قبل الحفظ
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// بوابة الحفظ: (1) الحقول المطلوبة — الحكم والعَلم في البيت الواحد <see cref="RequiredFieldHelper"/>
    /// فلا نسخة ثانية لحكم «فارغ» هنا (RULE-N172/N184)، ثم (2) قواعد المستند في <see cref="_ValidateRangeRules"/>.
    /// </summary>
    private bool _ValidateBeforeSave()
    {
        errorProvider.Clear();
        RequiredFieldHelper.Clear(this);

        if (!RequiredFieldHelper.Validate(this))
        {
            // يُوسَم كل ناقص من الـHelper (أيقونة ✗ + الرسالة العربية + ومضة الرفض)، ثم ينتقل المؤشر إلى أوله.
            if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();

            clsDialogs.Warning("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.");
            return false;
        }

        return _ValidateRangeRules();
    }

    /// <summary>
    /// قواعد المستند — **منقولة حرفياً** من التحقق القديم (نفس الرسائل العربية ونفس ترتيب الفحص):
    /// سطر واحد على الأقل · وحدة صالحة لكل سطر · كمية أكبر من صفر — ويُضاف إليها منع الحفظ بسطر
    /// معلَّق في لوحة الإدخال لم يُضَف (حماية من الفقدان الصامت؛ لا وجود له في النسخة القديمة لأنها بلا لوحة إدخال).
    /// ولا حكم على «فارغ» هنا: المخزن حقل مطلوب يملكه <see cref="RequiredFieldHelper"/> وحده (RULE-N172).
    /// </summary>
    private bool _ValidateRangeRules()
    {
        if (_dtLines.Rows.Count == 0)
        {
            clsDialogs.Warning("يرجى إضافة منتج واحد على الأقل.");
            return false;
        }

        foreach (DataRow row in _dtLines.Rows)
        {
            if (row["ProductUnitId"] is not int productUnitId || productUnitId <= 0)
            {
                clsDialogs.Warning("يرجى اختيار الوحدة المناسبة لكل المنتجات.");
                return false;
            }

            if (_DecimalOf(row, "Quantity") <= 0m)
            {
                clsDialogs.Warning("الكمية يجب أن تكون أكبر من صفر.");
                return false;
            }
        }

        if (_editingProductId <= 0 && cmbUnit.Tag is int pendingProductId && pendingProductId > 0)
        {
            clsDialogs.Warning("هناك صنف في لوحة الإدخال لم يُضَف إلى المستند — أضِفه أو ألغِه أولاً.");
            txtUnitCost.Focus();
            return false;
        }

        return true;
    }

    // ═════════════════════════════════════════════════════════════════
    //  الحفظ والترحيل (المنطق محفوظ حرفياً + إعادة التهيئة D4)
    // ═════════════════════════════════════════════════════════════════

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_isSaving) return;

        if (!_ValidateBeforeSave()) return;

        if (!clsDialogs.Confirm("هل أنت متأكد من حفظ وترحيل المستند؟ لا يمكن تعديله بعد الترحيل.")) return;

        _isSaving = true;
        _UpdateButtonsState();
        Cursor = Cursors.WaitCursor;

        try
        {
            // نوع القراءة فقط (WarehouseDto.Id = short) — حكم «فارغ» يملكه RequiredFieldHelper وحده (RULE-N172).
            if (cmbWarehouse.SelectedValue is not short warehouseId) return;

            var createRequest = new CreateInventoryAdjustmentRequest(
                WarehouseId: warehouseId,
                AdjustmentType: 1); // Opening

            var createResult = await AdjustmentApi.CreateAsync(createRequest);
            if (!createResult.IsSuccess || createResult.Value == null)
            {
                clsDialogs.Error(createResult.Error ?? "حدث خطأ أثناء إنشاء التسوية.");
                return;
            }

            int adjustmentId = createResult.Value.Id;
            int successCount = 0;
            int failedCount = 0;

            // لقطة من الأسطر قبل الحلقة: أي تغيير مفاجئ للمصدر لا يُبطل التعداد (حزام أمان فوق قفل الأزرار)،
            // ورسالة المتعذر تُقرأ من عدد اللقطة نفسه فلا تصف المستند بحالة تغيّرت في منتصف الترحيل.
            var lines = _dtLines.Rows.Cast<DataRow>().ToList();
            int lineCount = lines.Count;

            foreach (DataRow row in lines)
            {
                int productUnitId = row["ProductUnitId"] is int unitId ? unitId : 0;
                decimal quantity = _DecimalOf(row, "Quantity");
                decimal unitCost = _DecimalOf(row, "UnitCost");

                if (productUnitId <= 0 || quantity <= 0) continue;

                var lineRequest = new AddInventoryAdjustmentLineRequest(
                    InventoryAdjustmentId: adjustmentId,
                    ProductUnitId: productUnitId,
                    ExpectedQuantity: 0m,
                    ActualQuantity: quantity,
                    UnitCost: unitCost);

                var lineResult = await AdjustmentApi.AddLineAsync(adjustmentId, lineRequest);
                if (lineResult.IsSuccess)
                    successCount++;
                else
                    failedCount++;   // لا تسقيط صامت: يُحسب ويُبلَّغ قبل الترحيل
            }

            if (successCount == 0)
            {
                clsDialogs.Error("فشل إضافة جميع المنتجات. تحقق من البيانات.");
                return;
            }

            if (failedCount > 0)
            {
                // لا ترحيل بمستند ناقص: تبقى التسوية مسودة ويُبلَّغ المستخدم بالعدد الحقيقي.
                clsDialogs.Error(
                    $"تعذر إضافة {clsFormat.Number(failedCount)} من {clsFormat.Number(lineCount)} صنفاً، ولم يُرحَّل المستند. راجع بيانات الأصناف المتعذرة وأعد المحاولة.");
                return;
            }

            var postResult = await AdjustmentApi.PostAsync(adjustmentId);
            if (!postResult.IsSuccess)
            {
                clsDialogs.Error(postResult.Error ?? "حدث خطأ أثناء ترحيل التسوية.");
                return;
            }

            string documentNo = string.IsNullOrWhiteSpace(postResult.Value?.AdjustmentNo)
                ? string.Empty
                : $" — رقم المستند: {postResult.Value!.AdjustmentNo}";

            clsDialogs.Success(
                $"تم حفظ وترحيل {clsFormat.Number(successCount)} منتج بنجاح كبضاعة أول المدة.{documentNo}");

            // D4 — الشاشة تبقى مفتوحة وتُعاد إلى مستند جديد والمؤشر على الباركود.
            _ResetDocument(keepWarehouse: true);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ الكميات الافتتاحية");
            clsDialogs.Error("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل.");
        }
        finally
        {
            Cursor = Cursors.Default;
            _isSaving = false;
            _UpdateButtonsState();
        }
    }
}
