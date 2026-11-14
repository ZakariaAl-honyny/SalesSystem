using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// حوار «خصم المشتريات» — واجهة أولية ببيانات وهمية في الذاكرة (وضع التجربة / UI-First).
/// يُعرض عبر <c>ShowDialog</c> ويُعيد قيمة الخصم ونوعه (مبلغ / نسبة) ونسبة الضريبة والصافي النهائي.
/// <para>RULE-N40…RULE-N47 / RULE-N50…RULE-N54 / RULE-N97 — التنقل والثيم والتفقيط الحيّ.</para>
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
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnOk, btnCancel);                 // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.Escape, () => DialogResult = DialogResult.Cancel));

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
        toolTip.SetToolTip(cmbTax, "نسبة الضريبة المطبَّقة على المجموع بعد الخصم");

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

    /// <summary>نسبة الضريبة المختارة (0 / 5 / 15).</summary>
    public decimal TaxRate =>
        cmbTax.SelectedItem?.ToString() is { } s
        && decimal.TryParse(s.TrimEnd('%'), out var rate) ? rate : 0m;

    /// <summary>قيمة الخصم المحسوبة بالريال اليمني (مبلغ أو نسبة × المجموع الفرعي) — منطق محفوظ كما هو.</summary>
    public decimal DiscountAmount => IsPercentageDiscount
        ? Subtotal * (DiscountValue / 100m)
        : DiscountValue;

    /// <summary>مبلغ الضريبة المحسوبة على المجموع بعد الخصم — منطق محفوظ كما هو.</summary>
    public decimal TaxAmount => (Subtotal - DiscountAmount) * (TaxRate / 100m);

    /// <summary>
    /// الصافي النهائي = المجموع الفرعي − الخصم + الضريبة
    /// (نفس معادلة الفاتورة: NetTotal = SubTotal - InvoiceDiscount + TaxAmount + OtherCharges)
    /// </summary>
    public decimal NetTotal => Subtotal - DiscountAmount + TaxAmount;

    // ═══════════════════════════════════════════════════════════
    // تحميل البيانات التجريبية (Mock)
    // ═══════════════════════════════════════════════════════════
    private void LoadMockData()
    {
        rbAmount.Checked = true;       // الخصم الافتراضي: مبلغ ثابت
        numDiscount.Value = 10000.00m; // خصم مبلغ 10,000 ريال يمني
        cmbTax.Items.Clear();
        cmbTax.Items.Add("0%");
        cmbTax.Items.Add("5%");
        cmbTax.Items.Add("15%");
        cmbTax.SelectedIndex = 0;      // ضريبة 0% افتراضياً
    }

    // ═══════════════════════════════════════════════════════════
    // حساب فوري — المجموع الفرعي / الخصم / الضريبة / الصافي النهائي
    // ═══════════════════════════════════════════════════════════
    private void rbDiscountType_CheckedChanged(object? sender, EventArgs e)
    {
        // عند التحويل إلى النسبة المئوية تُعاد القيمة إلى حد معقول إن تجاوزت 100%
        if (rbPercent.Checked && numDiscount.Value > 100m)
            numDiscount.Value = 5m;

        Recalculate();
    }

    private void numDiscount_ValueChanged(object? sender, EventArgs e) => Recalculate();
    private void cmbTax_SelectedIndexChanged(object? sender, EventArgs e) => Recalculate();

    private void Recalculate()
    {
        lblSubtotalValue.Text = Subtotal.ToString("N2");
        lblDiscountValue.Text = DiscountAmount.ToString("N2");
        lblTaxValue.Text = TaxAmount.ToString("N2");
        lblNetValue.Text = NetTotal.ToString("N2");

        // إظهار الوحدة بجانب حقل القيمة: نسبة % أو مبلغ بالريال
        lblUnit.Text = IsPercentageDiscount ? "%" : "ريال";
    }

    // ═══════════════════════════════════════════════════════════
    // الإجراءات — موافق / إلغاء
    // ═══════════════════════════════════════════════════════════
    private void btnOk_Click(object? sender, EventArgs e)
    {
        try
        {
            errorProvider.SetError(numDiscount, "");

            if (DiscountValue < 0)
            {
                errorProvider.SetError(numDiscount, "لا يمكن أن تكون قيمة الخصم سالبة.");
                MessageBox.Show("لا يمكن أن تكون قيمة الخصم سالبة.", "تحقق من القيم",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numDiscount.Focus();
                return;
            }

            // منع خصم أكبر من المجموع الفرعي عند الخصم بالمبلغ
            if (!IsPercentageDiscount && DiscountValue > Subtotal)
            {
                errorProvider.SetError(numDiscount, "قيمة الخصم أكبر من المجموع الفرعي للفاتورة.");
                MessageBox.Show("قيمة الخصم أكبر من المجموع الفرعي للفاتورة.", "تحقق من القيم",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numDiscount.Focus();
                return;
            }

            // منع خصم بنسبة تتجاوز 100%
            if (IsPercentageDiscount && DiscountValue > 100m)
            {
                errorProvider.SetError(numDiscount, "نسبة الخصم لا يمكن أن تتجاوز 100%.");
                MessageBox.Show("نسبة الخصم لا يمكن أن تتجاوز 100%.", "تحقق من القيم",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numDiscount.Focus();
                return;
            }

            // في النسخة النهائية تُعاد القيم إلى فاتورة الشراء عبر خصائص الحوار
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "خطأ في حوار خصم المشتريات");
            MessageBox.Show("حدث خطأ غير متوقع. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
