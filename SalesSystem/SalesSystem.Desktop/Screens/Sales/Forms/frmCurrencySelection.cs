namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة اختيار العملة لفاتورة البيع — نموذج أولي (UI-first prototype) ببيانات تجريبية داخل الذاكرة،
/// بدون أي اعتماد على API أو قاعدة بيانات. تُعرض كـ modal عبر ShowDialog().
/// </summary>
public partial class frmCurrencySelection : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════
    private sealed class MockCurrency
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsLocal { get; set; }
        public decimal SystemRate { get; set; }   // سعر الصرف الرسمي (ريال يمني لكل وحدة واحدة)
        public decimal MinRate { get; set; }      // الحد الأدنى للنطاق المسموح
        public decimal MaxRate { get; set; }      // الحد الأقصى للنطاق المسموح
        public override string ToString() => Name;
    }

    private readonly List<MockCurrency> _currencies = new();
    private readonly Dictionary<MockCurrency, Panel> _cardByCurrency = new();
    private readonly Dictionary<MockCurrency, RadioButton> _radioByCurrency = new();
    private MockCurrency? _selected;

    public frmCurrencySelection(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _ = serviceProvider;
        InitializeComponent();
        BuildMockCurrencies();
        BuildCards();
        SelectCurrency(_currencies[0]); // الافتراضي: العملة المحلية
    }

    // ═══════════════════════════════════════════════
    // نتيجة الاختيار للمتصل (نموذج أولي)
    // ═══════════════════════════════════════════════
    /// <summary>رمز العملة المحددة، مثل: YER / USD / SAR.</summary>
    public string SelectedCurrencyCode => _selected?.Code ?? "";

    /// <summary>اسم العملة المحددة بالعربية.</summary>
    public string SelectedCurrencyName => _selected?.Name ?? "";

    /// <summary>سعر الصرف المعتمد بالريال اليمني (يساوي 1 للعملة المحلية).</summary>
    public decimal ExchangeRateToLocal => _selected is { IsLocal: false } ? numExchangeRate.Value : 1m;

    // ═══════════════════════════════════════════════
    // بناء البيانات التجريبية (3 عملات)
    // ═══════════════════════════════════════════════
    private void BuildMockCurrencies()
    {
        _currencies.Clear();

        // العملة المحلية — معامل 1 بدون تحويل
        _currencies.Add(new MockCurrency
        {
            Code = "YER", Name = "ريال يمني", IsLocal = true,
            SystemRate = 1m, MinRate = 1m, MaxRate = 1m
        });

        // دولار أمريكي — سعر رسمي 250 ريال يمني، النطاق المسموح 240–260
        _currencies.Add(new MockCurrency
        {
            Code = "USD", Name = "دولار أمريكي", IsLocal = false,
            SystemRate = 250m, MinRate = 240m, MaxRate = 260m
        });

        // ريال سعودي — سعر رسمي 66 ريال يمني، النطاق المسموح 63–69
        _currencies.Add(new MockCurrency
        {
            Code = "SAR", Name = "ريال سعودي", IsLocal = false,
            SystemRate = 66m, MinRate = 63m, MaxRate = 69m
        });
    }

    // ═══════════════════════════════════════════════
    // بطاقات العملات القابلة للاختيار (RadioButton + كود + سعر)
    // ═══════════════════════════════════════════════
    private void BuildCards()
    {
        flpCurrencies.Controls.Clear();
        _cardByCurrency.Clear();
        _radioByCurrency.Clear();

        foreach (var cur in _currencies)
        {
            var card = new Panel
            {
                Width = 136,
                Height = 82,
                Margin = new Padding(0, 0, 6, 0),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = cur
            };

            var rdo = new RadioButton
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = cur.IsLocal ? $"{cur.Name} (محلي)" : cur.Name,
                Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(33, 43, 54),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                Tag = cur
            };

            var lblCode = new Label
            {
                Dock = DockStyle.Top,
                Height = 20,
                Text = cur.Code,
                Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(100, 110, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                Tag = cur
            };

            var lblRate = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                Text = cur.IsLocal ? "1.00" : cur.SystemRate.ToString("N2"),
                Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(21, 87, 36),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand,
                Tag = cur
            };

            // ترتيب البطاقة: اسم العملة أعلى، ثم الكود، والسعر في الأسفل
            card.Controls.Add(lblRate);
            card.Controls.Add(lblCode);
            card.Controls.Add(rdo);

            // اختيار العملة بالنقر على أي جزء من البطاقة
            rdo.Click += (s, e) => SelectCurrency(cur);
            card.Click += (s, e) => SelectCurrency(cur);
            lblCode.Click += (s, e) => SelectCurrency(cur);
            lblRate.Click += (s, e) => SelectCurrency(cur);

            tip.SetToolTip(card,
                cur.IsLocal
                    ? "العملة المحلية للفواتير — بدون تحويل"
                    : $"1 {cur.Name} = {cur.SystemRate:N2} ريال يمني (السعر الرسمي)");

            _cardByCurrency[cur] = card;
            _radioByCurrency[cur] = rdo;
            flpCurrencies.Controls.Add(card);
        }
    }

    /// <summary>تحديد عملة وتحديث تمييز البطاقات (اختيار واحد فقط).</summary>
    private void SelectCurrency(MockCurrency cur)
    {
        _selected = cur;

        foreach (var kv in _radioByCurrency)
        {
            bool isSelected = kv.Key == cur;
            kv.Value.Checked = isSelected;
            _cardByCurrency[kv.Key].BackColor = isSelected
                ? Color.FromArgb(214, 232, 252)
                : Color.White;
        }

        UpdateRatePanel();
    }

    // ═══════════════════════════════════════════════
    // لوحة سعر الصرف الديناميكية + ملاحظة التقييد + التحقق
    // ═══════════════════════════════════════════════
    private void UpdateRatePanel()
    {
        if (_selected == null) return;

        if (_selected.IsLocal)
        {
            // العملة المحلية: إخفاء لوحة سعر الصرف بالكامل
            pnlRate.Visible = false;
            lblValidation.Visible = false;
            lblNote.Text = "سيتم تقييد مبالغ الفاتورة بالريال اليمني (العملة المحلية) دون الحاجة إلى تحويل.";
        }
        else
        {
            // عملة أجنبية: إظهار السعر الرسمي + النطاق المسموح
            pnlRate.Visible = true;
            lblRateTitle.Text = $"سعر الصرف — {_selected.Name} ({_selected.Code}) → ريال يمني (YER)";
            numExchangeRate.Value = _selected.SystemRate;
            lblRateRange.Text = $"النطاق المسموح: {_selected.MinRate:0.##} – {_selected.MaxRate:0.##}";
            lblNote.Text = $"سيتم تقييد مبالغ الفاتورة بعملة «{_selected.Name} ({_selected.Code})» " +
                           "وتحويل المعادل المحلي بالريال اليمني.";
            ValidateRate();
        }
    }

    /// <summary>إظهار/إخفاء شريط التنبيه الأحمر عند خروج سعر الصرف عن النطاق المسموح.</summary>
    private void ValidateRate()
    {
        if (_selected == null || _selected.IsLocal)
        {
            lblValidation.Visible = false;
            return;
        }

        decimal value = numExchangeRate.Value;
        bool inRange = value >= _selected.MinRate && value <= _selected.MaxRate;

        lblValidation.Visible = !inRange;
        lblValidation.Text = inRange
            ? ""
            : $"تنبيه: سعر الصرف ({value:N2}) خارج النطاق المسموح " +
              $"({_selected.MinRate:0.##} – {_selected.MaxRate:0.##}).";
    }

    // ═══════════════════════════════════════════════
    // الأحداث
    // ═══════════════════════════════════════════════
    private void numExchangeRate_ValueChanged(object? sender, EventArgs e) => ValidateRate();

    private void btnRestoreRate_Click(object? sender, EventArgs e)
    {
        if (_selected == null || _selected.IsLocal) return;

        numExchangeRate.Value = _selected.SystemRate;
        MessageBox.Show($"تم استعادة سعر الصرف الرسمي ({_selected.SystemRate:N2}).",
            "استعادة السعر الرسمي", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        if (_selected == null) return;

        if (!_selected.IsLocal)
        {
            ValidateRate();
            if (lblValidation.Visible)
            {
                MessageBox.Show("سعر الصرف المدخل خارج النطاق المسموح.\nيرجى تصحيحه أو استعادة السعر الرسمي.",
                    "اختيار العملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numExchangeRate.Focus();
                return;
            }
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}