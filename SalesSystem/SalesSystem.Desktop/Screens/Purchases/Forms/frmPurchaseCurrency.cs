using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// حوار «عملة المشتريات» — واجهة أولية ببيانات وهمية في الذاكرة (وضع التجربة / UI-First).
/// يُعرض عبر <c>ShowDialog</c> ويُعيد العملة المختارة وسعر الصرف المعتمد إلى النموذج الأب.
/// <para>RULE-N40…RULE-N47 / RULE-N50…RULE-N54 / RULE-N97 — التنقل والثيم والتفقيط الحيّ.</para>
/// </summary>
public partial class frmPurchaseCurrency : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ═══════════════════════════════════════════════════════════
    // بيانات تجريبية (Mock) — عملات المشتريات
    // قيم decimal داخل الذاكرة فقط — لا يوجد اتصال بقاعدة البيانات
    // ريال يمني (محلي) بمعامل 1 | دولار أمريكي ~250 | ريال سعودي ~66
    // ═══════════════════════════════════════════════════════════
    private enum CurrencyKey { Yer, Usd, Sar }

    private sealed class CurrencyOption
    {
        public CurrencyOption(string name, string code, bool isLocal, decimal officialRate, decimal rangePercent)
        {
            Name = name;
            Code = code;
            IsLocal = isLocal;
            OfficialRate = officialRate;
            RangePercent = rangePercent;
        }

        public string Name { get; }          // الاسم العربي
        public string Code { get; }          // رمز العملة
        public bool IsLocal { get; }         // عملة محلية؟
        public decimal OfficialRate { get; } // السعر الرسمي (1 عملة = X ريال يمني)
        public decimal RangePercent { get; } // النطاق المسموح % حول السعر الرسمي (0 للعملة المحلية)
    }

    private static readonly IReadOnlyDictionary<CurrencyKey, CurrencyOption> Currencies =
        new Dictionary<CurrencyKey, CurrencyOption>
        {
            [CurrencyKey.Yer] = new("الريال اليمني", "YER", true, 1.000m, 0m),
            [CurrencyKey.Usd] = new("الدولار الأمريكي", "USD", false, 250.00m, 5m),
            [CurrencyKey.Sar] = new("الريال السعودي", "SAR", false, 66.00m, 5m),
        };

    // ─── حالة الحوار الحالية ───
    private CurrencyKey _selected = CurrencyKey.Yer;
    private decimal _currentRate = 1.000m;
    private bool _isUpdatingUi; // يمنع التكرار عند تعيين قيم عناصر التحكم برمجياً

    public frmPurchaseCurrency(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N47 + RULE-N95 — الستة من KeyboardHelper في كل حوار إدخال.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0)); // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                  // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnOk, btnCancel, btnRestore);     // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                   // RULE-N47
            (Keys.Escape, () => DialogResult = DialogResult.Cancel));

        // RULE-N97 — التفقيط الحيّ على سعر الصرف.
        LiveTafqeetHelper.Attach(tipTafqeet,
            (numRate, "سعر الصرف", LiveTafqeetHelper.ReadMode.Money));

        ApplyUiTheme(); // RULE-N50

        // تهيئة الحالة الافتراضية: العملة المحلية (الريال اليمني)
        SelectCurrency(CurrencyKey.Yer);
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
        ThemeHelper.ApplyButtonStyle(btnRestore, ThemeHelper.ButtonType.Secondary);

        // ThemeHelper يفرض Segoe UI ويوسّع الأزرار ⇒ نثبّت Tahoma والأحجام (RULE-N21).
        foreach (var btn in new[] { btnOk, btnCancel })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        }

        btnOk.Size = new Size(140, 40);
        btnOk.ForeColor = Color.White;
        btnOk.Margin = new Padding(0, 0, 8, 0);
        btnCancel.Size = new Size(92, 40);
        btnCancel.Margin = new Padding(0);

        btnRestore.AutoSize = false;
        btnRestore.UseVisualStyleBackColor = false;
        btnRestore.MinimumSize = Size.Empty;
        btnRestore.Font = new Font("Tahoma", 9.5F, FontStyle.Bold);

        lblRangeStatus.ForeColor = ThemeHelper.Danger; // تنبيه أحمر صريح (RULE-N54: من الـHelper)

        toolTip.SetToolTip(btnOk, "تأكيد واعتماد العملة وسعر الصرف (Enter)");
        toolTip.SetToolTip(btnCancel, "إلغاء وإغلاق الحوار بلا اعتماد (Esc)");
        toolTip.SetToolTip(btnRestore, "استعادة السعر الرسمي للعملة المختارة");
        toolTip.SetToolTip(numRate, "سعر الصرف مقابل الريال اليمني — للعملات الأجنبية فقط");
        toolTip.SetToolTip(pnlYer, "الريال اليمني — العملة المحلية (معامل 1)");
        toolTip.SetToolTip(pnlUsd, "الدولار الأمريكي — السعر الرسمي 250.00 ر.ي");
        toolTip.SetToolTip(pnlSar, "الريال السعودي — السعر الرسمي 66.00 ر.ي");

        GreenThemeHelper.ApplyHeader(lblTitle);
        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ═══════════════════════════════════════════════════════════
    // نتائج الحوار — يقرأها النموذج الأب بعد ShowDialog
    // ═══════════════════════════════════════════════════════════
    public string SelectedCurrencyCode => Currencies[_selected].Code;
    public string SelectedCurrencyName => Currencies[_selected].Name;
    public bool IsLocalCurrency => Currencies[_selected].IsLocal;
    public decimal ExchangeRate => _currentRate;

    // ═══════════════════════════════════════════════════════════
    // اختيار العملة (بطاقات الراديو)
    // ═══════════════════════════════════════════════════════════
    private void rbYer_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingUi) return;
        if (rbYer.Checked) SelectCurrency(CurrencyKey.Yer);
    }

    private void rbUsd_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingUi) return;
        if (rbUsd.Checked) SelectCurrency(CurrencyKey.Usd);
    }

    private void rbSar_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingUi) return;
        if (rbSar.Checked) SelectCurrency(CurrencyKey.Sar);
    }

    private void SelectCurrency(CurrencyKey key)
    {
        var opt = Currencies[key];
        _selected = key;
        _currentRate = opt.OfficialRate;

        // منطقة سعر الصرف: متاحة للعملات الأجنبية فقط
        _isUpdatingUi = true;
        numRate.Value = opt.OfficialRate;
        _isUpdatingUi = false;

        numRate.Enabled = !opt.IsLocal;
        btnRestore.Enabled = !opt.IsLocal;
        lblRangeValue.Text = opt.IsLocal ? "—" : RangeText(opt);
        lblRangeStatus.Visible = false;

        UpdateCardSelection();
        UpdateCardBackColors();
        UpdateRateStatus();
        UpdateEquivalence();
    }

    private void UpdateCardSelection()
    {
        _isUpdatingUi = true;
        rbYer.Checked = _selected == CurrencyKey.Yer;
        rbUsd.Checked = _selected == CurrencyKey.Usd;
        rbSar.Checked = _selected == CurrencyKey.Sar;
        _isUpdatingUi = false;
    }

    /// <summary>
    /// تلوين البطاقة المختارة من ثوابت الثيم فقط (RULE-N54) — بلا قيمة لونية مكرّرة هنا.
    /// </summary>
    private void UpdateCardBackColors()
    {
        Color selected = GreenThemeHelper.SelectionBack;
        Color normal = Color.White;

        pnlYer.BackColor = _selected == CurrencyKey.Yer ? selected : normal;
        pnlUsd.BackColor = _selected == CurrencyKey.Usd ? selected : normal;
        pnlSar.BackColor = _selected == CurrencyKey.Sar ? selected : normal;
    }

    // ═══════════════════════════════════════════════════════════
    // سعر الصرف — تعديل فوري + نطاق مسموح + استعادة السعر الرسمي
    // ═══════════════════════════════════════════════════════════
    private void numRate_ValueChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingUi) return;
        _currentRate = numRate.Value;
        UpdateRateStatus();
        UpdateEquivalence();
    }

    private void btnRestore_Click(object? sender, EventArgs e)
    {
        // استعادة السعر الرسمي للعملة المختارة
        SelectCurrency(_selected);
    }

    private void UpdateRateStatus()
    {
        var opt = Currencies[_selected];
        lblRangeStatus.Visible = !opt.IsLocal && !IsWithinRange(opt, _currentRate);
    }

    private static bool IsWithinRange(CurrencyOption opt, decimal rate)
    {
        if (opt.IsLocal || opt.RangePercent == 0m) return true;
        decimal min = opt.OfficialRate * (1m - opt.RangePercent / 100m);
        decimal max = opt.OfficialRate * (1m + opt.RangePercent / 100m);
        return rate >= min && rate <= max;
    }

    private static string RangeText(CurrencyOption opt)
    {
        if (opt.IsLocal || opt.RangePercent == 0m) return "—";
        decimal min = opt.OfficialRate * (1m - opt.RangePercent / 100m);
        decimal max = opt.OfficialRate * (1m + opt.RangePercent / 100m);
        return $"{min:N2} – {max:N2}";
    }

    // ═══════════════════════════════════════════════════════════
    // ملاحظة المعادل — تتحدث فوراً عند تغيير العملة أو السعر
    // ═══════════════════════════════════════════════════════════
    private void UpdateEquivalence()
    {
        var opt = Currencies[_selected];
        if (opt.IsLocal)
        {
            lblEquivValue.Text = "عملة محلية — 1 ريال يمني = 1.000 ريال يمني (لا يتطلب تحويلاً)";
        }
        else
        {
            lblEquivValue.Text = $"1 {opt.Name} = {_currentRate:N2} ريال يمني";
        }
    }

    // ═══════════════════════════════════════════════════════════
    // الإجراءات — تأكيد واعتماد / إلغاء
    // ═══════════════════════════════════════════════════════════
    private void btnOk_Click(object? sender, EventArgs e)
    {
        try
        {
            errorProvider.SetError(numRate, "");

            var opt = Currencies[_selected];

            // للعملات الأجنبية: تحقق من النطاق المسموح قبل الاعتماد
            if (!opt.IsLocal && !IsWithinRange(opt, _currentRate))
            {
                errorProvider.SetError(numRate,
                    $"السعر المُدخل خارج النطاق المسموح ({RangeText(opt)}).");

                var confirm = MessageBox.Show(
                    $"السعر المُدخل ({_currentRate:N2}) خارج النطاق المسموح ({RangeText(opt)}).\nهل تريد الاعتماد بالسعر الرسمي ({opt.OfficialRate:N2})؟",
                    "سعر الصرف خارج النطاق",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    _currentRate = opt.OfficialRate;

                    _isUpdatingUi = true;
                    numRate.Value = opt.OfficialRate;
                    _isUpdatingUi = false;

                    errorProvider.SetError(numRate, "");
                }
                else
                {
                    numRate.Focus();
                    return; // يبقى المستخدم لتصحيح السعر
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "خطأ في حوار عملة المشتريات");
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
