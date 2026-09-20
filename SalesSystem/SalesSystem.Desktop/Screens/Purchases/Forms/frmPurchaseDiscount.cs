using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// نافذة "خصم المشتريات" — نموذج واجهة (UI Shape) يعمل ببيانات وهمية في الذاكرة فقط
/// دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تُعرض عبر ShowDialog وتُعيد إلى النموذج الأب قيمة الخصم ونوعه (مبلغ / نسبة)
/// ونسبة الضريبة والصافي النهائي عبر خصائص عامة بعد الموافقة.
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
        KeyboardHelper.WireEnterAsTab(this);

        LoadMockData();
        Recalculate();
    }

    // ═══════════════════════════════════════════════════════════
    // نتائج النافذة — يقرأها النموذج الأب بعد ShowDialog
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

    /// <summary>قيمة الخصم المحسوبة بالريال اليمني (مبلغ أو نسبة × المجموع الفرعي).</summary>
    public decimal DiscountAmount => IsPercentageDiscount
        ? Subtotal * (DiscountValue / 100m)
        : DiscountValue;

    /// <summary>مبلغ الضريبة المحسوبة على المجموع بعد الخصم.</summary>
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
            if (DiscountValue < 0)
            {
                MessageBox.Show("لا يمكن أن تكون قيمة الخصم سالبة.", "تحقق من القيم",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // منع خصم أكبر من المجموع الفرعي عند الخصم بالمبلغ
            if (!IsPercentageDiscount && DiscountValue > Subtotal)
            {
                MessageBox.Show("قيمة الخصم أكبر من المجموع الفرعي للفاتورة.", "تحقق من القيم",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // منع خصم بنسبة تتجاوز 100%
            if (IsPercentageDiscount && DiscountValue > 100m)
            {
                MessageBox.Show("نسبة الخصم لا يمكن أن تتجاوز 100%.", "تحقق من القيم",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // في النسخة النهائية تُعاد القيم إلى فاتورة الشراء عبر خصائص النافذة
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception)
        {
            // واجهة تجريبية: لا يوجد اتصال بالخادم بعد — رسالة عامة فقط
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