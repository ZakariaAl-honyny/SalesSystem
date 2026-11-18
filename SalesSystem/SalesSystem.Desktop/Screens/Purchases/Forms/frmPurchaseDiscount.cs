using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// حوار «خصم المشتريات» — واجهة أولية ببيانات وهمية في الذاكرة (وضع التجربة / UI-First).
/// يُعرض عبر <c>ShowDialog</c> ويُعيد قيمة الخصم ونوعه (مبلغ / نسبة) والصافي النهائي.
/// <para>RULE-N40…RULE-N47 / RULE-N50…RULE-N54 / RULE-N97 — التنقل والثيم والتفقيط الحيّ.</para>
/// <para>MVP V1 بلا ضريبة (أمر المالك 2026-11-18 — «remove each and every Tax columns»): لا نسبة ضريبة ولا مبلغ ضريبة، والصافي = الفرعي − الخصم.</para>
/// </summary>
public partial class frmPurchaseDiscount : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ═══════════════════════════════════════════════════════════
    // بيانات تجريبية (Mock) — مجموع فرعي افتراضي بالريال اليمني
    // (قيم decimal داخل الذاكرة فقط — لا يوجد اتصال بقاعدة البيانات)
    // ═══════════════════════════════════════════════════════════
    private const decimal MockSubtotal = 150000.00m; // المجموع الفرعي لفاتورة الشراء (ريال يمني)

    public frmPurchaseDiscount(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N47 + RULE-N95 — الستة من KeyboardHelper في كل حوار إدخال.
        // RULE-N191 — Enter يمشي بترتيب العرض داخل مجموعة «بيانات الخصم» (صفاً صفاً، ومن اليمين لليسار)
        // ثم يُركِّز زر «موافق» بلا تنفيذ؛ والواجهة ثنائية الوسيط وحدها تُعلن Enter مفتاح إدخال (RULE-N193).
        KeyboardHelper.WireEnterAsTab(grpDiscount, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor)); // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnOk, btnCancel);                 // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.Escape, () => DialogResult = DialogResult.Cancel));

        // RULE-N147 — الزر الافتراضي للحوار يُقرأ من نفس دالة الزر الأساسي التي يستدعيها WireEnterAsTab.
        AcceptButton = _PrimaryActionButton();

        // RULE-N97 — التفقيط الحيّ على قيمة الخصم.
        LiveTafqeetHelper.Attach(tipTafqeet,
            (numDiscount, "قيمة الخصم", LiveTafqeetHelper.ReadMode.Money));

        ApplyUiTheme(); // RULE-N50

        LoadMockData();
        Recalculate();
    }

    // ═══════════════════════════════════════════════════════════
    // الثيم (RULE-N50…RULE-N54)
    // ═══════════════════════════════════════════════════════════

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply</c> آخر سطر (RULE-N50).</summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        ThemeHelper.ApplyButtonStyle(btnOk, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancel, ThemeHelper.ButtonType.Secondary);

        // ThemeHelper يفرض Segoe UI ويوسّع الأزرار ⇒ نثبّت Tahoma والأحجام (RULE-N21).
        foreach (var btn in new[] { btnOk, btnCancel })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(140, 40);
        }

        btnOk.ForeColor = Color.White;
        btnOk.Margin = new Padding(0, 0, 8, 0);
        btnCancel.Size = new Size(92, 40);
        btnCancel.Margin = new Padding(0);

        toolTip.SetToolTip(btnOk, "موافق — اعتماد الخصم وإغلاق الحوار (Enter)");
        toolTip.SetToolTip(btnCancel, "إلغاء وإغلاق الحوار بلا حفظ (Esc)");
        toolTip.SetToolTip(numDiscount, "قيمة الخصم — مبلغ ثابت أو نسبة مئوية حسب النوع المحدد");

        GreenThemeHelper.ApplyHeader(lblTitle);
        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═══════════════════════════════════════════════════════════
    // نتائج الحوار — يقرأها النموذج الأب بعد ShowDialog
    // ═══════════════════════════════════════════════════════════
    public decimal Subtotal => MockSubtotal;

    /// <summary>نوع الخصم: true = نسبة مئوية، false = مبلغ ثابت.</summary>
    public bool IsPercentageDiscount => rbPercent.Checked;

    /// <summary>القيمة المدخلة (مبلغ أو نسبة).</summary>
    public decimal DiscountValue => numDiscount.Value;

    /// <summary>قيمة الخصم المحسوبة بالريال اليمني (مبلغ أو نسبة × المجموع الفرعي) — منطق محفوظ كما هو.</summary>
    public decimal DiscountAmount => IsPercentageDiscount
        ? Subtotal * (DiscountValue / 100m)
        : DiscountValue;

    /// <summary>
    /// الصافي النهائي = المجموع الفرعي − الخصم
    /// (نفس معادلة V1 بعد إزالة الضريبة: NetTotal = SubTotal - InvoiceDiscount + OtherCharges)
    /// </summary>
    public decimal NetTotal => Subtotal - DiscountAmount;

    // ═══════════════════════════════════════════════════════════
    // تحميل البيانات التجريبية (Mock)
    // ═══════════════════════════════════════════════════════════
    private void LoadMockData()
    {
        rbAmount.Checked = true;       // الخصم الافتراضي: مبلغ ثابت
        numDiscount.Value = 10000.00m; // خصم مبلغ 10,000 ريال يمني
    }

    // ═══════════════════════════════════════════════════════════
    // حساب فوري — المجموع الفرعي / الخصم / الصافي النهائي
    // ═══════════════════════════════════════════════════════════
    private void rbDiscountType_CheckedChanged(object? sender, EventArgs e)
    {
        // عند التحويل إلى النسبة المئوية تُعاد القيمة إلى حد معقول إن تجاوزت 100%
        if (rbPercent.Checked && numDiscount.Value > 100m)
            numDiscount.Value = 5m;

        Recalculate();
    }

    private void numDiscount_ValueChanged(object? sender, EventArgs e) => Recalculate();

    private void Recalculate()
    {
        lblSubtotalValue.Text = clsFormat.Money(Subtotal);
        lblDiscountValue.Text = clsFormat.Money(DiscountAmount);
        lblNetValue.Text = clsFormat.Money(NetTotal);

        // إظهار الوحدة بجانب حقل القيمة: نسبة % أو مبلغ بالريال
        lblUnit.Text = IsPercentageDiscount ? "%" : "ريال";
    }

    // ═══════════════════════════════════════════════════════════
    // الإجراءات — موافق / إلغاء
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// الزر الأساسي للحوار (RULE-N147): «موافق». تُقرأ عند كل ضغطة Enter وعند ضبط <c>AcceptButton</c>
    /// — ودالة واحدة يستهلكها الاثنان فلا نسخة ثانية للقرار (RULE-N169…RULE-N172).
    /// </summary>
    private Button? _PrimaryActionButton() => btnOk;

    private void btnOk_Click(object? sender, EventArgs e)
    {
        try
        {
            // MVP V1 بلا ضريبة ⇒ لا حقل مطلوب في الحوار بعد إزالة نسبة الضريبة،
            // فأُزيلت تغذية RequiredFieldHelper الميتة مع الحقل (RULE-N205: لا حارس بلا حقل يحرسه).

            // قواعد المدى (سالب / أكبر من المجموع الفرعي / أكبر من 100%) — منطق محفوظ كما هو.
            if (!_ValidateRangeRules()) return;

            // في النسخة النهائية تُعاد القيم إلى فاتورة الشراء عبر خصائص الحوار
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "خطأ في حوار خصم المشتريات");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// قواعد مدى قيمة الخصم — الرسائل العربية والترتيب محفوظة حرفياً كما كانت (RULE-002 · §2.2):
    /// خصم سالب مرفوض · مبلغ أكبر من المجموع الفرعي مرفوض · نسبة تتجاوز 100% مرفوضة.
    /// </summary>
    private bool _ValidateRangeRules()
    {
        errorProvider.SetError(numDiscount, "");

        if (DiscountValue < 0)
        {
            const string negativeDiscountMessage = "لا يمكن أن تكون قيمة الخصم سالبة.";

            // GAP-08b + RULE-N169: رفض واحد يقرن errorProvider بالتغذية البصرية بنفس النص.
            errorProvider.SetError(numDiscount, negativeDiscountMessage);
            clsInputFeedback.ShowRejectTooltip(numDiscount, negativeDiscountMessage, toolTip);
            clsDialogs.Warning(negativeDiscountMessage);
            numDiscount.Focus();
            return false;
        }

        // منع خصم أكبر من المجموع الفرعي عند الخصم بالمبلغ
        if (!IsPercentageDiscount && DiscountValue > Subtotal)
        {
            const string exceedsSubtotalMessage = "قيمة الخصم أكبر من المجموع الفرعي للفاتورة.";

            errorProvider.SetError(numDiscount, exceedsSubtotalMessage);
            clsInputFeedback.ShowRejectTooltip(numDiscount, exceedsSubtotalMessage, toolTip);
            clsDialogs.Warning(exceedsSubtotalMessage);
            numDiscount.Focus();
            return false;
        }

        // منع خصم بنسبة تتجاوز 100%
        if (IsPercentageDiscount && DiscountValue > 100m)
        {
            const string exceedsPercentMessage = "نسبة الخصم لا يمكن أن تتجاوز 100%.";

            errorProvider.SetError(numDiscount, exceedsPercentMessage);
            clsInputFeedback.ShowRejectTooltip(numDiscount, exceedsPercentMessage, toolTip);
            clsDialogs.Warning(exceedsPercentMessage);
            numDiscount.Focus();
            return false;
        }

        return true;
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
