using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «طباعة باركود الأصناف» — معاينة ملصق الباركود لصنف من دليل الأصناف الحقيقي.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر الملصق ← بيانات الملصق ← معاينة الملصق.
/// RULE-N42: Enter في حقل الباركود = بحث فوري، ويبقى التركيز في الحقل جاهزاً للمسح التالي.
/// <para>
/// <b>مصدر البيانات:</b> <see cref="IProductDesktopService.GetAllAsync"/> لملء قائمة الأصناف،
/// و<see cref="IProductDesktopService.GetByBarcodeAsync"/> للبحث بالباركود (يشمل باركودات الوحدات).
/// لا يُولَّد ولا يُختلق أي باركود: الباركود المعروض هو المسجَّل في الدليل فعلاً.
/// </para>
/// <para>
/// <b>الطباعة:</b> لا توجد خدمة طباعة في هذه النسخة — الضغط على «طباعة الملصقات» يعرض رسالة عربية
/// صريحة بأن الطباعة غير موصولة، ولا يدّعي نجاحاً.
/// </para>
/// </summary>
public partial class frmInventoryBarcode : Form
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>الأصناف كما وردت من دليل الأصناف (مصدر قائمة الاختيار والبحث).</summary>
    private readonly List<ProductDto> _products = new();

    /// <summary>الصنف المحدد حالياً — <c>null</c> قبل التحميل أو عند إخفاقه.</summary>
    private ProductDto? _selectedProduct;

    /// <summary>الباركود المراد طباعته: باركود الصنف، أو باركود الوحدة الذي مُسح فعلاً.</summary>
    private string _labelBarcode = string.Empty;

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة الصنف المحدد (RULE-N44).</summary>
    private const string FormTitle = "طباعة باركود الأصناف";

    public frmInventoryBarcode(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N40…RULE-N46 — تنقل Enter/الأسهم + تحديد القيمة + تظليل الحقل النشط.
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnPrint, btnClose);

        // RULE-N47 — Esc يغلق الشاشة (لا «جديد/تحديث/ترحيل» على هذه الشاشة).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.Escape, () => { if (btnClose.Enabled) btnClose.PerformClick(); }));

        ApplyUiTheme();
    }

    // ── عقد الطبقة المكتبية (RULE-N04/N05) ─────────────────────────────

    /// <summary>عقد الأصناف — مصدر قائمة الملصقات والبحث بالباركود.</summary>
    private IProductDesktopService ProductService
        => _serviceProvider.GetRequiredService<IProductDesktopService>();

    // ── الثيم (RULE-N50…RULE-N54) ─────────────────────────────────────

    /// <summary>الثيم الأخضر — <c>GreenThemeHelper.Apply(this)</c> آخر سطر دائماً (لا شبكة في هذه الشاشة).</summary>
    private void ApplyUiTheme()
    {
        GreenThemeHelper.ApplyHeader(lblMode);

        GreenThemeHelper.Apply(this); // ← الأخير دائماً (RULE-N50)
    }

    // ── التحميل من دليل الأصناف ─────────────────────────────────────────

    private async void frmInventoryBarcode_Load(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            await _LoadProductsAsync();

            cmbLabelSize.SelectedIndex = 1;   // متوسط — العناصر معلنة في المصمّم

            // RULE-N42: التركيز يبدأ في حقل الباركود ليكون جاهزاً للمسح التالي.
            txtBarcode.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل بيانات طباعة الباركود");
            clsDialogs.Error("حدث خطأ أثناء تحميل البيانات.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>يقرأ الأصناف من العقد ويربطها بقائمة الاختيار — لا قائمة أصناف مُختلقة داخل الشاشة.</summary>
    private async Task _LoadProductsAsync()
    {
        var result = await ProductService.GetAllAsync();

        if (!result.IsSuccess || result.Value is null)
        {
            Log.Warning("تعذر تحميل أصناف ملصقات الباركود: {Error}", result.Error);

            _products.Clear();
            _selectedProduct = null;
            _labelBarcode = string.Empty;

            cmbProduct.DataSource = null;
            lblBarcodeValue.Text = "—";
            pnlBarcodePreview.Invalidate();
            _UpdateModeHeader();

            clsDialogs.Error(result.Error ?? "تعذّر تحميل قائمة الأصناف.");
            return;
        }

        _products.Clear();
        _products.AddRange(result.Value.OrderBy(p => p.Name));

        if (_products.Count == 0)
        {
            cmbProduct.DataSource = null;
            _selectedProduct = null;
            _labelBarcode = string.Empty;
            lblBarcodeValue.Text = "—";
            pnlBarcodePreview.Invalidate();
            _UpdateModeHeader();

            clsDialogs.Warning("لا توجد أصناف في دليل الأصناف بعد — أضف صنفاً أولاً.");
            return;
        }

        cmbProduct.DisplayMember = nameof(ProductDto.Name);
        cmbProduct.ValueMember = nameof(ProductDto.Id);
        cmbProduct.DataSource = new List<ProductDto>(_products);

        cmbProduct.SelectedIndex = 0;
    }

    // ── أحداث الواجهة ────────────────────────────────────────────────────

    private void cmbProduct_SelectedIndexChanged(object? sender, EventArgs e)
    {
        _selectedProduct = cmbProduct.SelectedItem as ProductDto;

        // الباركود المعروض هو المسجَّل في الدليل فقط — لا توليد ولا اختلاق.
        _labelBarcode = _selectedProduct?.Barcode ?? string.Empty;
        lblBarcodeValue.Text = string.IsNullOrWhiteSpace(_labelBarcode) ? "—" : _labelBarcode;

        errorProvider.SetError(lblBarcodeValue,
            string.IsNullOrWhiteSpace(_labelBarcode) ? "لا يوجد باركود مسجَّل لهذا الصنف" : "");

        pnlBarcodePreview.Invalidate();
        _UpdateModeHeader();
    }

    /// <summary>RULE-N44 — مؤشر الوضع: اسم الصنف المحدد أو «بلا تحديد».</summary>
    private void _UpdateModeHeader()
    {
        if (_selectedProduct is null)
        {
            lblMode.Text = _products.Count == 0
                ? $"{FormTitle} — لا توجد أصناف"
                : FormTitle;
            return;
        }

        lblMode.Text = $"{FormTitle} — {_selectedProduct.Name}";
    }

    // ── RULE-N42: بحث فوري بالباركود ─────────────────────────────────────

    private void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        _FindByBarcodeAsync();

        // WireEnterAsTab موصول بالحقل أيضاً وينقل التركيز للحقل التالي بعده — لذلك نُعيد التركيز
        // إلى حقل الباركود بعد انتهاء معالجة المفتاح ليبقى جاهزاً للمسح التالي (RULE-N42).
        BeginInvoke((Action)(() =>
        {
            if (txtBarcode.IsDisposed) return;
            txtBarcode.Focus();
            txtBarcode.SelectAll();
        }));
    }

    /// <summary>
    /// يبحث عن الصنف بالباركود عبر العقد: يُحدَّد فوراً إن وُجد، وإلا رسالة عربية ويُبقى النص للتعديل.
    /// </summary>
    private async void _FindByBarcodeAsync()
    {
        try
        {
            if (!clsValidation.IsRequired(txtBarcode.Text))
            {
                errorProvider.SetError(txtBarcode, "أدخل أو امسح الباركود أولاً");
                clsInputFeedback.ShowRejectTooltip(txtBarcode, "أدخل أو امسح الباركود أولاً", tooltip);
                return;
            }

            Cursor = Cursors.WaitCursor;

            string code = txtBarcode.Text.Trim();

            var result = await ProductService.GetByBarcodeAsync(code);
            if (!result.IsSuccess || result.Value is null)
            {
                errorProvider.SetError(txtBarcode, "لا يوجد صنف بهذا الباركود");
                clsInputFeedback.ShowRejectTooltip(txtBarcode, "لا يوجد صنف بهذا الباركود", tooltip);
                clsDialogs.Warning($"لا يوجد صنف بهذا الباركود: {code}");
                return;
            }

            errorProvider.SetError(txtBarcode, "");

            var product = result.Value;
            var listed = _products.FirstOrDefault(p => p.Id == product.Id);

            if (listed is not null)
            {
                // اختيار الصنف يرفع SelectedIndexChanged فتتحدث المعاينة وشريط العنوان.
                cmbProduct.SelectedItem = listed;
            }
            else
            {
                // صنف غير نشط لا يظهر في القائمة: يُعرض مباشرة مع تنبيه صريح.
                _selectedProduct = product;
                _UpdateModeHeader();

                clsDialogs.Warning("الصنف الممسوح غير نشط — لا يظهر في قائمة الأصناف.");
            }

            // الباركود المطبوع هو **الكود الذي مُسح فعلاً**: باركود الوحدة (كرتون/باكت…) يختلف عن
            // باركود الصنف، فاستبداله بباركود الصنف يطبع ملصقاً لا يطابق ما مسحه المستخدم.
            _labelBarcode = code;
            lblBarcodeValue.Text = code;
            errorProvider.SetError(lblBarcodeValue, "");
            pnlBarcodePreview.Invalidate();

            txtBarcode.Clear();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء البحث بالباركود في شاشة طباعة الملصقات");
            clsDialogs.Error("حدث خطأ أثناء البحث بالباركود. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
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

            if (string.IsNullOrWhiteSpace(_labelBarcode))
            {
                clsDialogs.Warning("لا يوجد باركود مسجَّل لهذا الصنف — أضف الباركود من دليل الأصناف أولاً.");
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

            // لا خدمة طباعة في هذه النسخة — رسالة صريحة بدل ادّعاء نجاح الطباعة.
            clsDialogs.Info(
                "معاينة الملصق جاهزة، لكن الطباعة الفعلية لم تُوصَّل بعد (لا توجد خدمة طباعة في هذه النسخة).\r\n" +
                $"الصنف: {_selectedProduct.Name}\r\n" +
                $"الباركود: {_labelBarcode}\r\n" +
                $"عدد الملصقات: {count} | حجم الملصق: {sizeName}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تجهيز معاينة الباركود");
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

    // ── رسم معاينة الباركود (من باركود الصنف الحقيقي) ─────────────────────

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
                GreenThemeHelper.StatusMutedSoft,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        if (string.IsNullOrWhiteSpace(_labelBarcode))
        {
            using var noBarcodeFont = new Font("Tahoma", 11F, FontStyle.Bold);
            TextRenderer.DrawText(g, "لا يوجد باركود مسجَّل لهذا الصنف", noBarcodeFont,
                new Rectangle(0, height / 2 - 14, width, 28),
                GreenThemeHelper.TextStrong,
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

        // المجموعة + الرصيد الحالي — حقول حقيقية من دليل الأصناف (ProductDto لا يحمل سعراً/وحدة)
        using (var infoFont = new Font("Tahoma", 9.5F))
        {
            string category = string.IsNullOrWhiteSpace(_selectedProduct.CategoryName)
                ? "—"
                : _selectedProduct.CategoryName;

            TextRenderer.DrawText(g,
                $"المجموعة: {category} | الرصيد الحالي: {clsFormat.Quantity(_selectedProduct.CurrentStock)}",
                infoFont,
                new Rectangle(6, 36, width - 12, 20),
                GreenThemeHelper.StatusMutedDeep,
                TextFormatFlags.HorizontalCenter);
        }

        // أعمدة الباركود — مولّدة بشكل ثابت من رقم الباركود المسجَّل (معاينة فقط)
        int barsTop = 62;
        int barsBottom = height - 38;
        int barsHeight = Math.Max(20, barsBottom - barsTop);
        int left = 36;
        int right = width - 36;

        int seed = _labelBarcode.Aggregate(0, (acc, ch) => acc * 31 + ch);
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
            TextRenderer.DrawText(g, _labelBarcode, codeFont,
                new Rectangle(6, barsBottom + 6, width - 12, 24),
                Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
