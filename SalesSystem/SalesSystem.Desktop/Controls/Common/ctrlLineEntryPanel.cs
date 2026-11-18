using System.ComponentModel;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Models;
using SalesSystem.DTO;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Controls.Common;

/// <summary>عنصر اختاره المستخدم من منتقي الأصناف.</summary>
public sealed record PickedProduct(ProductDto Product, ProductUnitDto? Unit);

/// <summary>
/// لوحة «إضافة صنف» أعلى شبكة السطور — <b>بيت واحد</b> للشكل والسلوك معاً (TASK-014 ·
/// <c>RULE-N169</c>…<c>RULE-N172</c>): باركود ⇒ وحدة ⇒ كمية ⇒ سعر ⇒ خصم ⇒ <c>Enter</c>
/// يُضيف السطر ويعيد المؤشر إلى الباركود، بسلوك <c>frmAddUpdateSalesInvoice</c> نفسه.
/// <para>
/// <b>لماذا UserControl؟</b> السلوك نفسه مطلوب في المشتريات والمرتجعين؛ فنسخُه في كل شاشة
/// يصنع أربع نسخ من حكم مالي واحد (<c>RULE-N172</c>). الشاشات الثلاثة تُسند مُفوَّضاتها فقط
/// وتضع المكوّن في مصمّمها — ولا تحمل سطراً من منطق الإدخال.
/// </para>
/// <para>
/// <b>لا خدمة ولا DI:</b> المكوّن لا يعرف أي طبقة — يستقبل مُفوَّضات جاهزة من الحاضن
/// (<see cref="Lines"/> · <see cref="ResolveBarcodeAsync"/> · <see cref="LoadUnitsAsync"/>
/// · <see cref="PriceSelector"/> · <see cref="PickProductAsync"/>)، ويُستهلك كما يُستهلك
/// <c>MoneyTextBox</c> و<c>ctrlCrudToolbar</c> من مصمّم الشاشة (RULE-N05/RULE-N06).
/// </para>
/// <para>
/// <b>حدّ الخطأ (<c>RULE-N205</c>):</b> الحارس الوحيد على معالجات <c>async void</c>
/// (الباركود · سلسلة <c>Enter</c> · زر الإضافة) — ولا <c>try/catch</c> حول قراءة نص أو تحويل رقم.
/// </para>
/// </summary>
public partial class ctrlLineEntryPanel : UserControl
{
    /// <summary>نص زر الإضافة في وضع الإضافة (وضع التعديل: <see cref="UpdateButtonText"/>).</summary>
    private const string AddButtonText = "إضافة السطر";

    /// <summary>نص زر الإضافة في وضع تعديل سطر محمَّل.</summary>
    private const string UpdateButtonText = "تحديث السطر";

    /// <summary>الصنف المُعبَّأ حالياً من الباركود أو المنتقي.</summary>
    private ProductDto? _product;

    /// <summary><c>LineKey</c> السطر المحمَّل للتعديل — <c>null</c> يعني «سطر جديد».</summary>
    private Guid? _editingLineKey;

    /// <summary>حارس إعادة الدخول على الحفظ.</summary>
    private bool _busy;

    /// <summary>يمنع معالج تغيير الوحدة من محو سعر محمَّل (تعديل سطر) أثناء ملء القائمة.</summary>
    private bool _suppressUnitPrice;

    /// <summary>حالة إظهار حقل «الانتهاء» (يُقرأ منها ولو كان المكوّن نفسه مخفياً).</summary>
    private bool _showExpiry;

    public ctrlLineEntryPanel()
    {
        InitializeComponent();
        ApplyUiTheme();
        ResetPanel();
    }

    // ── عقد الحاضن (يُسند في مُنشئ الشاشة — لا مصمّم ولا DI) ─────────────────

    // WFO1000: واجهة وقت تشغيل لا خاصية مصمّم — المصمّم لا يعرضها ولا يُسلسلها.
    /// <summary>سطور المستند الحيّة — تُضاف وتُحدَّث من هنا في مكان واحد.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public BindingList<InvoiceLineItem>? Lines { get; set; }

    /// <summary>بحث الباركود: يُعيد الصنف أو <c>null</c> حين لا يوجد (فيُفتح المنتقي).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<string, Task<ProductDto?>>? ResolveBarcodeAsync { get; set; }

    /// <summary>تحميل وحدات الصنف من عقد الحاضن.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<int, Task<IReadOnlyList<ProductUnitDto>>>? LoadUnitsAsync { get; set; }

    /// <summary>سعر الوحدة: الشراء = CostPrice، البيع = SalePrice.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<ProductUnitDto, decimal>? PriceSelector { get; set; }

    /// <summary>فتح منتقي الأصناف وإرجاع ما اختاره المستخدم (أو <c>null</c> عند الإلغاء).</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<Task<PickedProduct?>>? PickProductAsync { get; set; }

    /// <summary>هل يُسمح بالإدخال الآن؟ (وضع الإضافة، المستند غير مرحّل) — الافتراضي: نعم دائماً.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<bool> IsEditable { get; set; } = static () => true;

    /// <summary>إعادة حساب مجاميع المستند بعد أي تغيير في السطور.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Action? TotalsChanged { get; set; }

    /// <summary>
    /// إظهار حقل «الانتهاء» — شاشة فاتورة البيع تُظهره للأصناف التي تُتبَّع بالصلاحية،
    /// وشاشات المشتريات والمرتجعين لا تمسّه فيبقى مخفياً (الافتراضي).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowExpiry
    {
        get => _showExpiry;
        set
        {
            _showExpiry = value;
            lblExpiryCaption.Visible = value;
            dtpExpiry.Visible = value;
        }
    }

    /// <summary>
    /// فحص إضافي قبل تثبيت السطر (كفاية المخزون…) يستقبل <b>السطر المرشَّح</b> كاملاً.
    /// <c>false</c> = لا تغيير ولا تفريغ ولا إبلاغ ولا تحريك تركيز — الشاشة تكون قالت كلمتها
    /// (رسالتها العربية وموضع تركيزها) بنفسها. <c>null</c> = لا فحص، والسطر يمرّ.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<InvoiceLineItem, Task<bool>>? BeforeCommit { get; set; }

    /// <summary>هل اللوحة تحمّل سطراً للتعديل؟</summary>
    public bool IsEditing => _editingLineKey.HasValue;

    /// <summary>يُطلقها الحفظ بعد إضافة سطر أو دمج كميته أو تحديثه في مكانه.</summary>
    public event EventHandler? LineCommitted;

    /// <summary>
    /// السطر الذي وقع عليه آخر إبلاغ (<see cref="LineCommitted"/>): السطر المُضاف أو المُحدَّث
    /// أو المزيدة كميته بالمسح السريع — يُقرأ من معالج الحدث (الحدث نفسه بلا حمولة EventArgs ثابتة).
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public InvoiceLineItem? CommittedLine { get; private set; }

    /// <summary>نص عنوان حقل السعر — شاشة الشراء تضبطه «سعر التكلفة:».</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PriceCaption
    {
        get => lblPriceCaption.Text;
        set => lblPriceCaption.Text = value;
    }

    // ── المظهر ───────────────────────────────────────────────────────────────

    /// <summary>الثيم (RULE-N50): آخر خطوة دائماً هي <c>GreenThemeHelper.Apply</c> — ولا لون هنا.</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.Apply(this);
    }

    // ── العقد العام ──────────────────────────────────────────────────────────

    /// <summary>يعيد المؤشر إلى الباركود مع تحديد ما فيه — نهاية كل سطر وبدايته.</summary>
    public void FocusBarcode()
    {
        txtBarcode.Focus();
        txtBarcode.SelectAll();
    }

    /// <summary>
    /// يحمّل سطراً موجوداً في اللوحة لتعديله. <b>لا يُعدَّل السطر هنا:</b> التغيير يقع في
    /// الحفظ وحده، فـ<see cref="CancelEdit"/> لا يحتاج نسخة احتياطية.
    /// </summary>
    public void BeginEdit(InvoiceLineItem line)
    {
        if (line is null) return;

        _product = null;                       // الهوية تأتي من السطر نفسه حتى يمسح المستخدم باركوداً
        _editingLineKey = line.LineKey;

        txtBarcode.Text = line.Barcode ?? line.ProductCode ?? string.Empty;
        txtItemCode.Text = line.ProductCode ?? string.Empty;
        lblProductName.Text = line.ProductName ?? string.Empty;
        txtQuantity.Text = line.Quantity.ToString("0.###");
        txtPrice.Text = line.UnitPrice.ToString("0.###");
        txtDiscount.Text = line.DiscountAmount.ToString("0.###");
        dtpExpiry.Checked = line.ExpiryDate.HasValue;
        if (line.ExpiryDate.HasValue) dtpExpiry.Value = line.ExpiryDate.Value;

        btnAddLine.Text = UpdateButtonText;
        btnCancelLine.Enabled = true;

        txtQuantity.Focus();
        txtQuantity.SelectAll();

        // الوحدات تُحمَّل بلا حجب الواجهة؛ وقائمة الوحدات لا تمسّ سعر السطر المحمَّل (_suppressUnitPrice).
        _ = LoadUnitsForEditAsync(line);
    }

    /// <summary>يُلغي التعديل ويُفرّغ اللوحة ويعيد المؤشر إلى الباركود.</summary>
    public void CancelEdit()
    {
        ResetPanel();
        FocusBarcode();
    }

    /// <summary>يُفرّغ اللوحة إلى حالة سطر جديد (بلا لمس سطور المستند).</summary>
    public void ResetPanel()
    {
        txtBarcode.Clear();
        txtItemCode.Clear();
        lblProductName.Text = string.Empty;

        _suppressUnitPrice = true;
        try { cmbUnit.DataSource = null; }
        finally { _suppressUnitPrice = false; }

        txtQuantity.Text = "1";
        txtPrice.Clear();
        txtDiscount.Text = "0";
        dtpExpiry.Checked = false;
        lblLineTotal.Text = clsFormat.Money(0m);

        _product = null;
        _editingLineKey = null;

        btnAddLine.Text = AddButtonText;
        btnCancelLine.Enabled = false;
    }

    // ── التفعيل/التعطيل: تعطيل المكوّن يُعطّل كل حقوله (Control.Enabled) ──────

    /// <summary>عند تعطيل اللوحة يُلغى التعديل أولاً — فلا تبقى سطور نصف معدَّلة.</summary>
    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);

        if (!Enabled) CancelEdit();
    }

    // ── الحفظ ────────────────────────────────────────────────────────────────

    /// <summary>هل أسند الحاضن كل ما يحتاجه الإدخال؟ غير المسند = لا فعل (لا استثناء للمستخدم).</summary>
    private bool IsReady =>
        Lines is not null
        && ResolveBarcodeAsync is not null
        && LoadUnitsAsync is not null
        && PriceSelector is not null
        && PickProductAsync is not null;

    /// <summary>
    /// الحفظ الوحيد: تحقق ⇒ قصّ الخصم على قيمة السطر ⇒ <b>سطر مرشَّح</b> ⇒ فحص الحاضن
    /// (<see cref="BeforeCommit"/>) ⇒ <b>دمج كمية</b> لسطر بنفس (الصنف + الوحدة) ⇒ إضافة/تحديث
    /// ⇒ تفريغ اللوحة ⇒ إعادة المجاميع ⇒ باركود.
    /// </summary>
    private async Task AddOrUpdateAsync()
    {
        if (!IsReady) return;
        if (!IsEditable()) return;
        if (_busy) return;

        _busy = true;
        try
        {
            await CommitAsync();
        }
        finally
        {
            _busy = false;
        }
    }

    private async Task CommitAsync()
    {
        var lines = Lines!;

        var editingLine = _editingLineKey is { } key
            ? lines.FirstOrDefault(l => l.LineKey == key)
            : null;

        // في التعديل: السطر نفسه هو دليل الصنف (اللوحة لا تملك ProductDto للوهلة الأولى).
        if (_product is null && editingLine is null)
        {
            clsDialogs.Warning("اختر الصنف أولاً بالباركود أو بمفتاح F4.");
            FocusBarcode();
            return;
        }

        if (!EnsureUnitSelected()) return;
        if (!EnsureQuantity()) return;
        if (!EnsurePrice()) return;
        if (!EnsureDiscount()) return;

        // الحكم وقع في Ensure* أعلاه — والقراءة هنا من نفس بيت التحقق (RULE-N169).
        _ = clsValidation.TryParseDecimal(txtQuantity.Text, out decimal quantity);
        _ = clsValidation.TryParseDecimal(txtPrice.Text, out decimal price);
        _ = clsValidation.TryParseDecimal(txtDiscount.Text, out decimal discount);

        var unit = (ProductUnitDto)cmbUnit.SelectedItem!;
        int productId = _product?.Id ?? editingLine!.ProductId;
        string barcode = txtBarcode.Text.Trim();
        if (barcode.Length == 0)
            barcode = _product?.Barcode ?? editingLine?.Barcode ?? editingLine?.ProductCode ?? string.Empty;

        // خصم السطر لا يتجاوز قيمة السطر (الكمية × السعر) — نفس تقييد الشاشة المرجعية.
        decimal maxDiscount = Math.Max(0m, quantity * price);
        if (discount > maxDiscount)
        {
            discount = maxDiscount;
            txtDiscount.Text = discount.ToString("0.###");
            clsDialogs.Info(
                $"لا يمكن أن يتجاوز خصم السطر قيمة السطر ({clsFormat.Money(maxDiscount)}).\n" +
                $"تم تقييد الخصم إلى {clsFormat.Money(maxDiscount)}.");
        }

        // السطر المرشَّح: كل قيم السطر قبل أي مساس بالسطور — وهو نفسه ما يفحصه الحاضن وما يُثبَّت.
        var candidate = new InvoiceLineItem
        {
            ProductId = productId,
            ProductCode = barcode,
            Barcode = barcode,
            ProductName = lblProductName.Text,
            UnitName = unit.UnitName ?? string.Empty,
            ProductUnitId = unit.Id,
            ConversionFactor = unit.ConversionFactor > 0m ? unit.ConversionFactor : 1m,
            Quantity = quantity,
            UnitPrice = price,
            DiscountAmount = discount,
            ExpiryDate = dtpExpiry.Checked ? dtpExpiry.Value.Date : (DateTime?)null
        };

        // فحص الحاضن (كفاية المخزون…) **قبل** أي مساس: إرجاعه false = لا تغيير ولا تفريغ ولا إبلاغ،
        // واستثناءه يُلتقط عند حدّ المعالج (RULE-N205) فيُظهر الرسالة العربية العامة ويلغي الحفظ
        // كاملاً — فلا نصف تثبيت.
        var beforeCommit = BeforeCommit;
        if (beforeCommit is not null && !await beforeCommit(candidate)) return;

        // ── دمج الكمية: نفس (الصنف + الوحدة) ⇒ لا سطر ثانٍ ──
        if (editingLine is null)
        {
            var existing = lines.FirstOrDefault(
                l => l.ProductId == candidate.ProductId && l.ProductUnitId == candidate.ProductUnitId);
            if (existing is not null)
            {
                existing.Quantity += candidate.Quantity;
                if (candidate.DiscountAmount > 0m) existing.DiscountAmount = candidate.DiscountAmount;
                existing.ExpiryDate = candidate.ExpiryDate;

                NotifyCommitted(existing);
                return;
            }
        }

        InvoiceLineItem committedLine;

        if (editingLine is not null)
        {
            editingLine.ProductId = candidate.ProductId;
            editingLine.ProductCode = candidate.ProductCode;
            editingLine.Barcode = candidate.Barcode;
            editingLine.ProductName = candidate.ProductName;
            editingLine.UnitName = candidate.UnitName;
            editingLine.ProductUnitId = candidate.ProductUnitId;
            editingLine.ConversionFactor = candidate.ConversionFactor;
            editingLine.Quantity = candidate.Quantity;
            editingLine.UnitPrice = candidate.UnitPrice;
            editingLine.DiscountAmount = candidate.DiscountAmount;
            editingLine.ExpiryDate = candidate.ExpiryDate;
            committedLine = editingLine;
        }
        else
        {
            lines.Add(candidate);
            committedLine = candidate;
        }

        NotifyCommitted(committedLine);
    }

    /// <summary>
    /// نهاية أي حفظ ناجح: تسجيل السطر المُثبَّت ⇒ تفريغ اللوحة ⇒ إعادة المجاميع ⇒ باركود ⇒ إبلاغ الحاضن.
    /// </summary>
    private void NotifyCommitted(InvoiceLineItem line)
    {
        CommittedLine = line;
        ResetPanel();
        TotalsChanged?.Invoke();
        FocusBarcode();
        LineCommitted?.Invoke(this, EventArgs.Empty);
    }

    // ── الصنف والوحدات ───────────────────────────────────────────────────────

    private async Task PickAndApplyAsync()
    {
        var picked = await PickProductAsync!();
        if (picked is null) return;

        await ApplyResolvedProductAsync(picked.Product, picked.Unit, null);
    }

    /// <summary>
    /// طريق الإدخال الواحد после حلّ الصنف (باركود أو منتقي): تُحمَّل الوحدات **مرة واحدة**،
    /// ثم إما <b>المسار السريع</b> (+1 لسطر بنفس الصنف والوحدة) أو تعبئة اللوحة للمسار الكامل.
    /// </summary>
    private async Task ApplyResolvedProductAsync(ProductDto product, ProductUnitDto? preferredUnit, string? typedBarcode)
    {
        if (!IsReady) return;

        var units = await LoadUnitsCoreAsync(product.Id);
        if (txtBarcode.IsDisposed) return;

        var unitToSelect = PickUnit(units, preferredUnit);

        // ── المسار السريع (أمر المالك 2026-11-18): «إدخال الصنف نفسه مرة أخرى ⇒ الكمية +1» ──
        // لا تعبئة للوحة ولا رسالة ولا تمرير على BeforeCommit — والسطر يُبلَّغ عنه بالحدث
        // ويُعاد التركيز إلى الباركود جاهزاً للصنف التالي (تحقّق الشاشة عند الحفظ يبقى كما هو).
        if (!IsEditing && unitToSelect is not null && TryIncrementExisting(product, unitToSelect))
        {
            txtBarcode.Clear();
            txtBarcode.Focus();
            txtBarcode.SelectAll();
            return;
        }

        FillPanelWithProduct(product, units, unitToSelect, typedBarcode);
    }

    /// <summary>
    /// المسار السريع: سطر قائم بنفس (الصنف + الوحدة) ⇒ كميته +1 بالضبط، بلا رسالة وبلا لمس اللوحة.
    /// يُعيد <c>false</c> إن لم يوجد سطر مطابق ⇒ يمرّ الإدخال إلى المسار الكامل كما كان.
    /// <para>
    /// <b>لا يُنفّذ</b> <see cref="BeforeCommit"/>: لا يدخل المستندَ إلا وحدة واحدة، والفحص المسبق
    /// للشاشة (كفاية المخزون) يبقى على الحفظ، فلا تُقاطع عملية المسح برسالة وسط الإدخال السريع.
    /// </para>
    /// </summary>
    private bool TryIncrementExisting(ProductDto product, ProductUnitDto unit)
    {
        var lines = Lines;
        if (lines is null) return false;

        var existing = lines.FirstOrDefault(l => l.ProductId == product.Id && l.ProductUnitId == unit.Id);
        if (existing is null) return false;

        existing.Quantity += 1m;

        CommittedLine = existing;
        TotalsChanged?.Invoke();
        LineCommitted?.Invoke(this, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// وحدة الصنف المختارة: وحدة المنتقي إن أُرسلت، ثم الوحدة الأساسية، ثم الأولى —
    /// حكم واحد يستهلكه المساران (السريع والكامل) و<see cref="LoadUnitsForEditAsync"/>.
    /// </summary>
    private static ProductUnitDto? PickUnit(
        IReadOnlyList<ProductUnitDto> units,
        ProductUnitDto? preferredUnit,
        int? preferredUnitId = null)
        => (preferredUnit is not null ? units.FirstOrDefault(u => u.Id == preferredUnit.Id) : null)
           ?? (preferredUnitId is not null ? units.FirstOrDefault(u => u.Id == preferredUnitId.Value) : null)
           ?? units.FirstOrDefault(u => u.IsBaseUnit)
           ?? units.FirstOrDefault();

    /// <summary>تحميل وحدات الصنف مع مؤشر انتظار — نقطة واحدة، فلا وحدة تُطلب مرتين في إدخال واحد.</summary>
    private async Task<IReadOnlyList<ProductUnitDto>> LoadUnitsCoreAsync(int productId)
    {
        IReadOnlyList<ProductUnitDto> units = Array.Empty<ProductUnitDto>();

        var form = FindForm();
        if (form is not null) form.Cursor = Cursors.WaitCursor;
        try
        {
            units = await LoadUnitsAsync!(productId);
        }
        finally
        {
            if (form is not null && !form.IsDisposed) form.Cursor = Cursors.Default;
        }

        return units;
    }

    /// <summary>تعبئة اللوحة بالصنف: الاسم/الكود/الباركود/الكمية/السعر ثم مؤشر الكمية.</summary>
    private void FillPanelWithProduct(
        ProductDto product,
        IReadOnlyList<ProductUnitDto> units,
        ProductUnitDto? unitToSelect,
        string? typedBarcode)
    {
        FillUnitCombo(units, unitToSelect);

        _product = product;
        lblProductName.Text = product.Name;
        txtItemCode.Text = product.Barcode ?? string.Empty;
        txtBarcode.Text = product.Barcode ?? typedBarcode ?? string.Empty;
        txtQuantity.Text = "1";
        txtPrice.Text = unitToSelect is null
            ? string.Empty
            : PriceSelector!(unitToSelect).ToString("0.###");

        txtQuantity.Focus();
        txtQuantity.SelectAll();
    }

    /// <summary>ملء قائمة الوحدات واختيار وحدة (بلا لمس السعر المعروض — يُعاد ضبطه بعد الاستدعاء).</summary>
    private void FillUnitCombo(IReadOnlyList<ProductUnitDto> units, ProductUnitDto? selected)
    {
        _suppressUnitPrice = true;
        try
        {
            cmbUnit.DisplayMember = nameof(ProductUnitDto.UnitName);
            cmbUnit.ValueMember = nameof(ProductUnitDto.Id);
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.DataSource = new List<ProductUnitDto>(units);
            if (selected is not null) cmbUnit.SelectedValue = selected.Id;
        }
        finally
        {
            _suppressUnitPrice = false;
        }
    }

    /// <summary>تحميل وحدات سطر محمَّل للتعديل — بلا حجب الواجهة وبلا مسّ السعر/الكمية.</summary>
    private async Task LoadUnitsForEditAsync(InvoiceLineItem line)
    {
        var loader = LoadUnitsAsync;
        if (loader is null) return;

        try
        {
            var units = await loader(line.ProductId);
            if (cmbUnit.IsDisposed) return;

            FillUnitCombo(units, PickUnit(units, null, line.ProductUnitId));
        }
        catch (Exception)
        {
            // حدّ الخطأ لمسار بلا منتظِر: لا يُسقط الشاشة ولا يكشف تفاصيل الاستثناء.
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    // ── لوحة المفاتيح ────────────────────────────────────────────────────────

    /// <summary>
    /// <c>RULE-N193</c> — <c>Enter</c> يجب أن يُعلَن مفتاح إدخال على كل حقل، وإلا ابتلعه
    /// <c>AcceptButton</c> في <c>ProcessDialogKey</c> قبل وصوله إلى <c>KeyDown</c>.
    /// </summary>
    private void Entry_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

        // القائمة المنسدلة المفتوحة تملك Enter (تُثبّت العنصر وتُغلق القائمة)
        if (ReferenceEquals(sender, cmbUnit) && cmbUnit.DroppedDown) return;

        // تقويم الانتهاء: يُعلَن المفتاح فقط والحقل هو المركَّز — والنافذة المنبثقة للتقويم
        // نافذة مستقلة تملك Enter بنفسها (لا يوجد علم «منسدل» على DateTimePicker).
        if (ReferenceEquals(sender, dtpExpiry) && !dtpExpiry.Focused) return;

        e.IsInputKey = true;
    }

    private async void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (cmbUnit.DroppedDown) return;
        if (!IsReady) return;

        try
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                string barcode = txtBarcode.Text.Trim();
                if (barcode.Length == 0)
                {
                    await PickAndApplyAsync();
                    return;
                }

                var product = await ResolveBarcodeAsync!(barcode);
                if (product is null)
                {
                    // باركود غير مسجّل ⇒ منتقي الأصناف (نمط الشاشة المرجعية) لا تحذير نصف تعبئة
                    await PickAndApplyAsync();
                    return;
                }

                await ApplyResolvedProductAsync(product, null, barcode);
                return;
            }

            if (e.KeyCode == Keys.F4)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                await PickAndApplyAsync();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                CancelEdit();
            }
        }
        catch (Exception)
        {
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// سلسلة <c>Enter</c>: وحدة ⇒ كمية ⇒ سعر ⇒ خصم ⇒ (الانتهاء إن ظهر) ⇒ إضافة السطر.
    /// و<c>Esc</c> يُلغي.
    /// </summary>
    private async void Entry_KeyDown(object? sender, KeyEventArgs e)
    {
        if (cmbUnit.DroppedDown) return;
        // تقويم الانتهاء: لا يُعالَج Enter إلا والحقل هو المركَّز (المنبثقة تملك Enter بنفسها)
        if (ReferenceEquals(sender, dtpExpiry) && !dtpExpiry.Focused) return;
        if (!IsReady) return;

        try
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                CancelEdit();
                return;
            }

            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

            e.Handled = true;
            e.SuppressKeyPress = true;

            if (ReferenceEquals(sender, cmbUnit))
            {
                if (!EnsureUnitSelected()) return;
                FocusAndSelect(txtQuantity);
                return;
            }

            if (ReferenceEquals(sender, txtQuantity))
            {
                if (!EnsureQuantity()) return;
                FocusAndSelect(txtPrice);
                return;
            }

            if (ReferenceEquals(sender, txtPrice))
            {
                if (!EnsurePrice()) return;
                FocusAndSelect(txtDiscount);
                return;
            }

            if (ReferenceEquals(sender, txtDiscount))
            {
                if (!EnsureDiscount()) return;

                if (dtpExpiry.Visible)
                {
                    dtpExpiry.Focus();
                    return;
                }

                await AddOrUpdateAsync();
                return;
            }

            if (ReferenceEquals(sender, dtpExpiry))
            {
                await AddOrUpdateAsync();
            }
        }
        catch (Exception)
        {
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    // ── التحقق (حكم واحد لكل حقل — يُستهلك من السلسلة ومن الحفظ معاً) ─────────

    private bool EnsureUnitSelected()
    {
        if (cmbUnit.SelectedItem is ProductUnitDto) return true;

        clsDialogs.Warning("اختر وحدة الصنف.");
        cmbUnit.Focus();
        return false;
    }

    private bool EnsureQuantity()
    {
        if (clsValidation.TryParseDecimal(txtQuantity.Text, out decimal quantity) && quantity > 0m) return true;

        clsDialogs.Warning("الكمية يجب أن تكون أكبر من صفر.");
        txtQuantity.Focus();
        txtQuantity.SelectAll();
        return false;
    }

    private bool EnsurePrice()
    {
        if (clsValidation.TryParseDecimal(txtPrice.Text, out decimal price) && price >= 0m) return true;

        clsDialogs.Warning("السعر لا يمكن أن يكون سالباً.");
        txtPrice.Focus();
        txtPrice.SelectAll();
        return false;
    }

    private bool EnsureDiscount()
    {
        if (clsValidation.TryParseDecimal(txtDiscount.Text, out decimal discount) && discount >= 0m) return true;

        clsDialogs.Warning("الخصم لا يمكن أن يكون سالباً.");
        txtDiscount.Focus();
        txtDiscount.SelectAll();
        return false;
    }

    private static void FocusAndSelect(TextBox control)
    {
        control.Focus();
        control.SelectAll();
    }

    // ── المعاينة الحيّة والأزرار ─────────────────────────────────────────────

    private void cmbUnit_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressUnitPrice) return;
        if (cmbUnit.SelectedItem is not ProductUnitDto unit) return;

        var selector = PriceSelector;
        if (selector is null) return;

        txtPrice.Text = selector(unit).ToString("0.###");
    }

    /// <summary>معاينة إجمالي السطر: (كمية × سعر) − خصم، وما لا يُقرأ يُحسب صفراً.</summary>
    private void Entry_Amount_TextChanged(object? sender, EventArgs e)
    {
        _ = clsValidation.TryParseDecimal(txtQuantity.Text, out decimal quantity);
        _ = clsValidation.TryParseDecimal(txtPrice.Text, out decimal price);
        _ = clsValidation.TryParseDecimal(txtDiscount.Text, out decimal discount);

        lblLineTotal.Text = clsFormat.Money((quantity * price) - discount);
    }

    private async void btnAddLine_Click(object? sender, EventArgs e)
    {
        try
        {
            await AddOrUpdateAsync();
        }
        catch (Exception)
        {
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    private void btnCancelLine_Click(object? sender, EventArgs e) => CancelEdit();
}
