using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

/// <summary>
/// نافذة إضافة سند قبض / صرف جديد — نموذج واجهة (UI Shape) يعمل ببيانات وهمية
/// في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض تفقيط المبلغ بالحروف العربية بشكل مباشر أثناء إدخاله.
/// </summary>
public partial class frmCashVoucherNew : Form
{
    private readonly IServiceProvider _serviceProvider;
    private bool _isReceipt = true;

    // ── بيانات وهمية في الذاكرة (Mock) ────────────────────────────────────────

    private sealed class TreasuryAccount
    {
        public required string Code { get; init; }
        public required string Name { get; init; }
        public required decimal Balance { get; init; }

        public override string ToString() => $"{Name} {Code} (رصيد: {Balance:N0})";
    }

    private static readonly TreasuryAccount[] TreasuryAccounts =
    {
        new() { Code = "111", Name = "الصندوق الرئيسي", Balance = 250000m },
        new() { Code = "112", Name = "البنك",           Balance = 800000m },
        new() { Code = "113", Name = "المحفظة",         Balance = 95000m },
    };

    private static readonly string[] PaymentMethods = { "نقداً", "بنك ومحفظة" };

    private static readonly string[] PartyTypes = { "عميل", "مورد", "مصروف", "موظف" };

    // الحساب المقابل يعتمد على نوع الطرف
    private static readonly Dictionary<string, string[]> ContraAccountsByType = new()
    {
        ["عميل"]  = new[] { "2102 - ذمم عملاء" },
        ["مورد"]  = new[] { "1103 - ذمم موردين" },
        ["مصروف"] = new[] { "5101 - مصروفات متنوعة" },
        ["موظف"]  = new[] { "2141 - استحقاقات موظفين" },
    };

    private static readonly string[] CostCenters = { "CC-01", "CC-02", "CC-03", "CC-04" };

    // ألوان النوع: قبض (أخضر) / صرف (وردي)
    private static readonly Color ReceiptColor = Color.FromArgb(46, 204, 113);
    private static readonly Color PaymentColor = Color.FromArgb(231, 76, 60);

    public frmCashVoucherNew(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnSave.PerformClick()));
        _LoadMockLookups();
    }

    // ── تحميل القوائم الوهمية ─────────────────────────────────────────────────

    private void _LoadMockLookups()
    {
        cmbTreasury.Items.Clear();
        foreach (var acc in TreasuryAccounts)
            cmbTreasury.Items.Add(acc.ToString());

        cmbMethod.Items.AddRange(PaymentMethods);
        cmbPartyType.Items.AddRange(PartyTypes);
        cmbCostCenter.Items.AddRange(CostCenters);

        rbReceipt.Checked = true;                 // افتراضي: سند قبض
        dtpDate.Value = DateTime.Today;
        cmbMethod.SelectedIndex = 0;
        cmbTreasury.SelectedIndex = 0;
        cmbPartyType.SelectedIndex = 0;           // يعبّئ الحساب المقابل تلقائياً
        cmbCostCenter.SelectedIndex = 0;

        _ApplyTypeVisuals();
        _UpdateTafqit();
    }

    private void _LoadContraAccounts()
    {
        cmbContraAccount.Items.Clear();

        if (cmbPartyType.SelectedItem is not string type) return;
        if (!ContraAccountsByType.TryGetValue(type, out var accounts)) return;

        cmbContraAccount.Items.AddRange(accounts);
        cmbContraAccount.SelectedIndex = 0;
    }

    // ── التبديل بين قبض / صرف ─────────────────────────────────────────────────

    private void _ApplyTypeVisuals()
    {
        _isReceipt = rbReceipt.Checked;
        var color = _isReceipt ? ReceiptColor : PaymentColor;

        lblMode.Text = _isReceipt ? "سند قبض" : "سند صرف";
        Text = _isReceipt ? "سند قبض جديد" : "سند صرف جديد";
        lblAccent.BackColor = color;

        rbReceipt.BackColor = _isReceipt ? color : Color.White;
        rbReceipt.ForeColor = _isReceipt ? Color.White : Color.FromArgb(108, 117, 125);
        rbPayment.BackColor = !_isReceipt ? color : Color.White;
        rbPayment.ForeColor = !_isReceipt ? Color.White : Color.FromArgb(108, 117, 125);

        lblTafqit.ForeColor = color;
    }

    // ── تفقيط المبلغ بالحروف العربية (حتى الملايين) ───────────────────────────

    private static readonly string[] _ones =
        { "صفر", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة" };

    private static readonly string[] _teens =
        { "عشرة", "أحد عشر", "اثنا عشر", "ثلاثة عشر", "أربعة عشر", "خمسة عشر",
          "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };

    private static readonly string[] _tens =
        { "", "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };

    private static readonly string[] _hundreds =
        { "", "مائة", "مائتان", "ثلاثمائة", "أربعمائة", "خمسمائة",
          "ستمائة", "سبعمائة", "ثمانمائة", "تسعمائة" };

    private void _UpdateTafqit()
        => lblTafqit.Text = ArabicAmountToWords(numAmount.Value);

    /// <summary>تحويل مبلغ عشري إلى عبارة عربية، مثال: "مائة وخمسون ألف ريال يمني فقط".</summary>
    private static string ArabicAmountToWords(decimal amount)
    {
        decimal abs = Math.Abs(amount);
        long whole = (long)Math.Floor(abs);
        int fraction = (int)Math.Round((abs - whole) * 100);
        if (fraction >= 100) { whole++; fraction -= 100; }

        var parts = new List<string>();
        if (whole > 0)      parts.Add(_NumberToWords(whole) + " ريال يمني");
        if (fraction > 0)   parts.Add(_TwoDigits(fraction) + " فلساً");
        if (parts.Count == 0) parts.Add("صفر ريال يمني");

        return string.Join(" و", parts) + " فقط";
    }

    private static string _NumberToWords(long n)
    {
        if (n == 0) return "صفر";

        var parts = new List<string>();

        long millions  = n / 1_000_000;
        long thousands = (n % 1_000_000) / 1_000;
        long units     = n % 1_000;

        if (millions > 0)  parts.Add(_Millions(millions));
        if (thousands > 0) parts.Add(_Thousands(thousands));
        if (units > 0)     parts.Add(_ThreeDigits((int)units));

        return string.Join(" و", parts);
    }

    private static string _Thousands(long k)
    {
        if (k == 1) return "ألف";
        if (k == 2) return "ألفان";
        if (k <= 10) return _ThreeDigits((int)k) + " آلاف";
        return _ThreeDigits((int)k) + " ألف";
    }

    private static string _Millions(long m)
    {
        if (m == 1) return "مليون";
        if (m == 2) return "مليونان";
        if (m <= 10) return _ThreeDigits((int)m) + " ملايين";
        return _ThreeDigits((int)m) + " مليون";
    }

    private static string _ThreeDigits(int n)
    {
        if (n == 0) return "";

        int h = n / 100;
        int rest = n % 100;

        string result = h > 0 ? _hundreds[h] : "";
        if (rest > 0)
        {
            string restWords = _TwoDigits(rest);
            result = result.Length == 0 ? restWords : result + " و" + restWords;
        }

        return result;
    }

    private static string _TwoDigits(int n)
    {
        if (n >= 1 && n <= 9) return _ones[n];
        if (n == 10) return "عشرة";
        if (n >= 11 && n <= 19) return _teens[n - 10];

        int t = n / 10;
        int o = n % 10;
        if (o == 0) return _tens[t];
        return _ones[o] + " و" + _tens[t];   // مثال: خمسة وعشرون
    }

    // ── التحقق من صحة البيانات ────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbTreasury.SelectedIndex < 0)
        {
            errorProvider.SetError(cmbTreasury, "يرجى اختيار حساب الخزينة.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbTreasury, "");
        }

        if (cmbContraAccount.SelectedIndex < 0)
        {
            errorProvider.SetError(cmbContraAccount, "يرجى اختيار الحساب المقابل.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbContraAccount, "");
        }

        if (numAmount.Value <= 0)
        {
            errorProvider.SetError(numAmount, "المبلغ يجب أن يكون أكبر من صفر.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numAmount, "");
        }

        if (string.IsNullOrWhiteSpace(txtNotes.Text))
        {
            errorProvider.SetError(txtNotes, "بيان السند مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtNotes, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void rbReceipt_CheckedChanged(object sender, EventArgs e)
        => _ApplyTypeVisuals();

    private void rbPayment_CheckedChanged(object sender, EventArgs e)
        => _ApplyTypeVisuals();

    private void cmbPartyType_SelectedIndexChanged(object sender, EventArgs e)
        => _LoadContraAccounts();

    private void numAmount_ValueChanged(object sender, EventArgs e)
        => _UpdateTafqit();

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        string typeName = _isReceipt ? "القبض" : "الصرف";

        MessageBox.Show(
            $"تم حفظ سند {typeName} وترحيله وطباعته بنجاح (وضع تجريبي).\n" +
            $"المبلغ: {numAmount.Value:N2} ريال يمني",
            "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
        => Close();
}