namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة الخصم والمصروفات والدفع [F7] — تُفتح من شاشة البيع لتعديل خصم الفاتورة
/// وضريبتها ومصروفاتها الإضافية وتحديد طريقة الدفع (نقدي / آجل / دفع مجزأ).
/// نسخة أولى (UI-first prototype) ببيانات تجريبية داخل الذاكرة — بدون أي اعتماد على API أو قاعدة بيانات.
/// تُعرض كـ modal عبر ShowDialog().
/// </summary>
public partial class frmDiscountExpenses : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════
    /// <summary>المجموع الفرعي التجريبي للفاتورة (ريال يمني).</summary>
    private const decimal MockSubtotal = 50000m;

    /// <summary>حسابات المصروف التجريبية (رمز - اسم) مطابقة لنافذة الويب المرجعية.</summary>
    private static readonly (string Code, string Name)[] MockExpenseAccounts =
    {
        ("5101", "مصروفات متنوعة"),
        ("5102", "إيجار"),
        ("5103", "كهرباء"),
        ("5104", "نقل")
    };

    public frmDiscountExpenses(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _ = serviceProvider;
        InitializeComponent();
        LoadDefaults();
        Recalculate();
    }

    /// <summary>
    /// تعبئة القوائم المنسدلة بالقيم الافتراضية (تتم في الكود وليس في المصمم
    /// حتى لا تتفاعل الأحداث قبل اكتمال تهيئة النموذج).
    /// </summary>
    private void LoadDefaults()
    {
        cmbExpenseAccount.Items.Clear();
        foreach (var acc in MockExpenseAccounts)
            cmbExpenseAccount.Items.Add($"{acc.Code} - {acc.Name}");

        cmbExpenseAccount.SelectedIndex = 0;   // مصروفات متنوعة
        cmbTaxRate.SelectedIndex = 0;          // 0%
        cmbPaymentMethod.SelectedIndex = 0;    // نقدي
    }

    /// <summary>
    /// إظهار/إخفاء حقول الدفع ديناميكياً:
    /// «دفع مجزأ» → المدفوع نقد + بنك + محفظة، وإلا → حقل واحد «المبلغ المدفوع».
    /// </summary>
    private void UpdatePaymentVisibility()
    {
        bool split = cmbPaymentMethod.SelectedIndex == 2; // دفع مجزأ

        lblPaidSingle.Visible = !split;
        numPaidAmount.Visible = !split;

        lblPaidCash.Visible = split;
        numPaidCash.Visible = split;
        lblPaidBank.Visible = split;
        numPaidBank.Visible = split;
        lblPaidWallet.Visible = split;
        numPaidWallet.Visible = split;
    }

    /// <summary>حساب إجماليات الفاتورة فورياً بناءً على المدخلات الحالية.</summary>
    private (decimal Discount, decimal Tax, decimal Net, decimal Paid) ComputeTotals()
    {
        decimal subtotal = MockSubtotal;

        // الخصم: نسبة مئوية من المجموع الفرعي، أو مبلغ مباشر (لا يتجاوز المجموع).
        decimal discount = rbDiscountPercent.Checked
            ? subtotal * Math.Min(numDiscountValue.Value, 100m) / 100m
            : Math.Min(numDiscountValue.Value, subtotal);

        // الضريبة: تُحسب على الصافي بعد الخصم.
        decimal taxRate = 0m;
        if (cmbTaxRate.SelectedItem is string taxText &&
            taxText.EndsWith("%", StringComparison.Ordinal) &&
            decimal.TryParse(taxText[..^1], out decimal parsedRate))
        {
            taxRate = parsedRate;
        }
        decimal tax = (subtotal - discount) * taxRate / 100m;

        decimal expenses = numExpenseAmount.Value;
        decimal net = subtotal - discount + tax + expenses;

        // المدفوع: مجموع الحقول المجزأة عند «دفع مجزأ»، وإلا المبلغ المدفوع الواحد.
        decimal paid = cmbPaymentMethod.SelectedIndex == 2
            ? numPaidCash.Value + numPaidBank.Value + numPaidWallet.Value
            : numPaidAmount.Value;

        return (discount, tax, net, paid);
    }

    /// <summary>تحديث بطاقة الملخص (المجموع الفرعي / الخصم / الضريبة / المصروفات / الصافي / الباقي).</summary>
    private void Recalculate()
    {
        var (discount, tax, net, paid) = ComputeTotals();
        decimal remaining = net - paid;

        lblSummarySubtotalVal.Text = $"{MockSubtotal:N2} YER";
        lblSummaryDiscountVal.Text = $"-{discount:N2} YER";
        lblSummaryTaxVal.Text = $"+{tax:N2} YER";
        lblSummaryExpenseVal.Text = $"+{numExpenseAmount.Value:N2} YER";
        lblSummaryNetVal.Text = $"{net:N2} YER";
        lblSummaryRemainingVal.Text = $"{remaining:N2} YER";

        // الباقي بالأخضر عند السداد الكامل، وبالأحمر عند السداد الزائد.
        lblSummaryRemainingVal.ForeColor = remaining < 0
            ? Color.FromArgb(220, 53, 69)
            : Color.FromArgb(21, 87, 36);
    }

    // ═══════════════════════════════════════════════
    // الأحداث — إعادة حساب فورية عند أي تغيير
    // ═══════════════════════════════════════════════
    private void rbDiscountType_CheckedChanged(object? sender, EventArgs e) => Recalculate();

    private void numDiscountValue_ValueChanged(object? sender, EventArgs e) => Recalculate();

    private void cmbTaxRate_SelectedIndexChanged(object? sender, EventArgs e) => Recalculate();

    private void numExpenseAmount_ValueChanged(object? sender, EventArgs e) => Recalculate();

    private void cmbPaymentMethod_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdatePaymentVisibility();
        Recalculate();
    }

    private void numPaidAmount_ValueChanged(object? sender, EventArgs e) => Recalculate();

    private void numPaidCash_ValueChanged(object? sender, EventArgs e) => Recalculate();

    private void numPaidBank_ValueChanged(object? sender, EventArgs e) => Recalculate();

    private void numPaidWallet_ValueChanged(object? sender, EventArgs e) => Recalculate();

    // ═══════════════════════════════════════════════
    // الإجراءات — موافق / إلغاء / اختصارات
    // ═══════════════════════════════════════════════
    private void btnOk_Click(object? sender, EventArgs e)
    {
        var (_, _, net, paid) = ComputeTotals();

        if (paid > net)
        {
            MessageBox.Show("المبلغ المدفوع أكبر من الصافي النهائي. يرجى مراجعة قيم الدفع.",
                "تحقق من الدفع", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // في النسخة النهائية تُعاد القيم (الخصم، الضريبة، المصروفات، طريقة الدفع)
        // إلى فاتورة البيع عبر حدث DataBack / EventBus.
        DialogResult = DialogResult.OK;
        Close();
    }

    private void frmDiscountExpenses_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.F7:
                numDiscountValue.Focus();
                e.Handled = true;
                break;
        }
    }
}