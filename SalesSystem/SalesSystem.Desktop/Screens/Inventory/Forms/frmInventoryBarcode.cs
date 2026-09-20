using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// نموذج طباعة باركود الأصناف (نسخة تجريبية UI-first).
/// يعمل ببيانات Mock داخلية فقط — لا يوجد اتصال بالخلفية (API / قاعدة بيانات).
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

    public frmInventoryBarcode(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // محجوز لربط خدمات API الحقيقية لاحقاً — النسخة الحالية تعمل ببيانات Mock فقط
    private IServiceProvider ServiceProvider => _serviceProvider;

    // ── التحميل والبيانات التجريبية ──────────────────────────────────────

    private void frmInventoryBarcode_Load(object? sender, EventArgs e)
    {
        try
        {
            cmbProduct.DisplayMember = nameof(MockProduct.Name);
            cmbProduct.ValueMember = nameof(MockProduct.Barcode);
            cmbProduct.DataSource = new List<MockProduct>(MockProducts);

            cmbLabelSize.Items.AddRange(LabelSizes);
            cmbLabelSize.SelectedIndex = 1; // متوسط

            nudLabelsCount.Minimum = 1;
            nudLabelsCount.Maximum = 1000;
            nudLabelsCount.Value = 10;

            if (cmbProduct.Items.Count > 0)
                cmbProduct.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل بيانات طباعة الباركود");
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ── أحداث الواجهة ────────────────────────────────────────────────────

    private void cmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _selectedProduct = cmbProduct.SelectedItem as MockProduct;

        lblBarcodeValue.Text = _selectedProduct?.Barcode ?? "----------";
        pnlBarcodePreview.Invalidate();
    }

    private void btnPrint_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_selectedProduct is null)
            {
                MessageBox.Show("يرجى اختيار الصنف أولاً.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int count = (int)nudLabelsCount.Value;
            if (count <= 0)
            {
                MessageBox.Show("يرجى إدخال عدد صحيح من الملصقات.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sizeName = cmbLabelSize.SelectedItem?.ToString() ?? "متوسط";

            Cursor = Cursors.WaitCursor;

            // نسخة تجريبية: محاكاة نجاح الطباعة بدون طابعة حقيقية
            MessageBox.Show(
                $"تمت طباعة {count} ملصق باركود للصنف: {_selectedProduct.Name}\r\n" +
                $"الباركود: {_selectedProduct.Barcode} | حجم الملصق: {sizeName}",
                "طباعة ناجحة",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء طباعة الباركود");
            MessageBox.Show("حدث خطأ أثناء تجهيز الطباعة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
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
        using var rng = new Random(seed);

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