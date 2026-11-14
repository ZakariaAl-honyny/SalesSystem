using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «طباعة باركود الأصناف» (نسخة UI-first ببيانات داخلية).
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر الملصق ← بيانات الملصق ← معاينة الملصق.
/// RULE-N42: Enter في حقل الباركود = بحث فوري، ويبقى التركيز في الحقل جاهزاً للمسح التالي.
/// </summary>
public partial class frmInventoryBarcode : Form
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>صنف تجريبي لمعاينة الواجهة.</summary>
    private sealed record MockProduct(int Id, string Name, string Barcode, decimal Price, string Unit);

    private static readonly List<MockProduct> MockProducts =
    [
        new(1,  "سكر أبيض 1 كغ",        "6291041500213", 12.50m, "كيس"),
        new(2,  "أرز مصري فاخر 5 كغ",   "6291041500220", 65.75m, "كيس"),
        new(3,  "زيت دوار الشمس 1 لتر", "6291041500237", 34.90m, "زجاجة"),
        new(4,  "شاي أوراق 250 غ",      "6291041500244", 18.25m, "علبة"),
        new(5,  "حليب مجفف 400 غ",      "6291041500251", 42.00m, "علبة"),
        new(6,  "دقيق فاخر 2 كغ",       "6291041500268", 28.60m, "كيس"),
        new(7,  "معكرونة 400 غ",        "6291041500275", 6.35m,  "عبوة"),
        new(8,  "صابون غسيل 1 كغ",      "6291041500282", 15.80m, "قطعة"),
    ];

    private static readonly string[] LabelSizes = ["صغير", "متوسط", "كبير"];

    private MockProduct? _selectedProduct;

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة الصنف المحدد (RULE-N44).</summary>
    private const string FormTitle = "طباعة باركود الأصناف";

    public frmInventoryBarcode(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnPrint, btnClose);

        // RULE-N47 — Esc يغلق الشاشة (لا «جديد/تحديث/ترحيل» على هذه الشاشة).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.Escape, () => { if (btnClose.Enabled) btnClose.PerformClick(); }));

        ApplyUiTheme();
    }

    // محجوز لربط خدمات API الحقيقية لاحقاً — النسخة الحالية تعمل ببيانات Mock فقط
    private IServiceProvider ServiceProvider => _serviceProvider;

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً (لا شبكة في هذه الشاشة).</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── التحميل والبيانات التجريبية ──────────────────────────────────────

    private void frmInventoryBarcode_Load(object? sender, EventArgs e)
    {
        try
        {
            cmbProduct.DisplayMember = nameof(MockProduct.Name);
            cmbProduct.ValueMember = nameof(MockProduct.Barcode);
            cmbProduct.DataSource = new List<MockProduct>(MockProducts);

            cmbLabelSize.Items.Clear();
            cmbLabelSize.Items.AddRange(LabelSizes);
            cmbLabelSize.SelectedIndex = 1; // متوسط

            nudLabelsCount.Minimum = 1;
            nudLabelsCount.Maximum = 1000;
            nudLabelsCount.Value = 10;

            if (cmbProduct.Items.Count > 0)
                cmbProduct.SelectedIndex = 0;

            // RULE-N42: التركيز يبدأ في حقل الباركود ليكون جاهزاً للمسح التالي.
            txtBarcode.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل بيانات طباعة الباركود");
            clsDialogs.Error("حدث خطأ أثناء تحميل البيانات.");
        }
    }

    // ── أحداث الواجهة ────────────────────────────────────────────────────

    private void cmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _selectedProduct = cmbProduct.SelectedItem as MockProduct;

        lblBarcodeValue.Text = _selectedProduct?.Barcode ?? "----------";
        pnlBarcodePreview.Invalidate();

        _UpdateModeHeader();
    }

    /// <summary>RULE-N44 — مؤشر الوضع: اسم الصنف المحدد أو «بلا تحديد».</summary>
    private void _UpdateModeHeader()
    {
        lblMode.Text = _selectedProduct is null
            ? FormTitle
            : $"{FormTitle} — {_selectedProduct.Name}";
    }

    // ── RULE-N42: بحث فوري بالباركود ─────────────────────────────────────

    private void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        _FindByBarcode();

        // WireEnterAsTab موصول بالحقل أيضاً وينقل التركيز للحقل التالي بعده — لذلك نُعيد التركيز
        // إلى حقل الباركود بعد انتهاء معالجة المفتاح ليبقى جاهزاً للمسح التالي (RULE-N42).
        BeginInvoke((Action)(() =>
        {
            if (txtBarcode.IsDisposed) return;
            txtBarcode.Focus();
            txtBarcode.SelectAll();
        }));
    }

    /// <summary>يبحث عن الصنف بالباركود: يُحدَّد فوراً إن وُجد، وإلا رسالة عربية ويُبقى النص للتعديل.</summary>
    private void _FindByBarcode()
    {
        string code = txtBarcode.Text.Trim();

        if (code.Length == 0)
        {
            errorProvider.SetError(txtBarcode, "أدخل أو امسح الباركود أولاً");
            return;
        }

        var product = MockProducts.FirstOrDefault(
            p => string.Equals(p.Barcode, code, StringComparison.OrdinalIgnoreCase));

        if (product is null)
        {
            errorProvider.SetError(txtBarcode, "لا يوجد صنف بهذا الباركود");
            clsDialogs.Warning($"لا يوجد صنف بهذا الباركود: {code}");
            return;
        }

        errorProvider.SetError(txtBarcode, "");

        // اختيار الصنف يرفع SelectedIndexChanged فتتحدث المعاينة وشريط العنوان.
        cmbProduct.SelectedItem = product;
        lblBarcodeValue.Text = product.Barcode;
        pnlBarcodePreview.Invalidate();

        txtBarcode.Clear();
    }

    private void btnPrint_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_selectedProduct is null)
            {
                clsDialogs.Warning("يرجى اختيار الصنف أولاً.");
                return;
            }

            int count = (int)nudLabelsCount.Value;
            if (count <= 0)
            {
                clsDialogs.Warning("يرجى إدخال عدد صحيح من الملصقات.");
                return;
            }

            string sizeName = cmbLabelSize.SelectedItem?.ToString() ?? "متوسط";

            Cursor = Cursors.WaitCursor;

            // نسخة تجريبية: محاكاة نجاح الطباعة بدون طابعة حقيقية
            clsDialogs.Success(
                $"تمت طباعة {count} ملصق باركود للصنف: {_selectedProduct.Name}\r\n" +
                $"الباركود: {_selectedProduct.Barcode} | حجم الملصق: {sizeName}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء طباعة الباركود");
            clsDialogs.Error("حدث خطأ أثناء تجهيز الطباعة.");
        }
        finally
        {
            Cursor = Cursors.Default;
            txtBarcode.Focus();
        }
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    // ── رسم معاينة الباركود (Mock) ───────────────────────────────────────

    private void pnlBarcodePreview_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(pnlBarcodePreview.BackColor);

        int width = pnlBarcodePreview.Width;
        int height = pnlBarcodePreview.Height;

        if (_selectedProduct is null)
        {
            using var hintFont = new Font("Tahoma", 11F, FontStyle.Bold);
            TextRenderer.DrawText(g, "اختر صنفاً لعرض الباركود", hintFont,
                new Rectangle(0, height / 2 - 14, width, 28),
                Color.Gray,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        // اسم الصنف (أعلى الملصق)
        using (var nameFont = new Font("Tahoma", 12F, FontStyle.Bold))
        {
            TextRenderer.DrawText(g, _selectedProduct.Name, nameFont,
                new Rectangle(6, 8, width - 12, 26),
                Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // السعر + الوحدة
        using (var priceFont = new Font("Tahoma", 9.5F))
        {
            TextRenderer.DrawText(g,
                $"السعر: {_selectedProduct.Price:0.00} | الوحدة: {_selectedProduct.Unit}",
                priceFont,
                new Rectangle(6, 36, width - 12, 20),
                Color.DimGray,
                TextFormatFlags.HorizontalCenter);
        }

        // أعمدة الباركود — مولّدة بشكل ثابت من رقم الباركود (Mock Barcode)
        int barsTop = 62;
        int barsBottom = height - 38;
        int barsHeight = Math.Max(20, barsBottom - barsTop);
        int left = 36;
        int right = width - 36;

        int seed = _selectedProduct.Barcode.Aggregate(0, (acc, ch) => acc * 31 + ch);
        var rng = new Random(seed);

        int x = left;
        while (x < right - 2)
        {
            int barWidth = rng.Next(1, 4);
            int gap = rng.Next(1, 4);
            if (x + barWidth > right - 2)
                barWidth = right - 2 - x;
            if (barWidth <= 0) break;

            g.FillRectangle(Brushes.Black, x, barsTop, barWidth, barsHeight);
            x += barWidth + gap;
        }

        // رقم الباركود أسفل الأعمدة
        using (var codeFont = new Font("Consolas", 13F, FontStyle.Bold))
        {
            TextRenderer.DrawText(g, _selectedProduct.Barcode, codeFont,
                new Rectangle(6, barsBottom + 6, width - 12, 24),
                Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
