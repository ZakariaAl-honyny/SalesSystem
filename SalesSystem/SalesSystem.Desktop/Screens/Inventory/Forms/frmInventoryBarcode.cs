using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Responses;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// «طباعة باركود الأصناف» — معاينة ملصق الباركود لصنف من دليل الأصناف الحقيقي.
/// الهيكل (RULE-N79 / RULE-N135…N138): العنوان ← أوامر الملصق ← بيانات الملصق ← معاينة الملصق.
/// RULE-N42: Enter في حقل الباركود = بحث فوري، ويبقى التركيز في الحقل جاهزاً للمسح التالي.
/// RULE-N191/N194/N180: Enter يمشي بترتيب العرض ثم يُسلِّم التركيز لزر الإجراء بلا تنفيذ،
/// والحقول المطلوبة تُغذّى من البيت الواحد <see cref="RequiredFieldHelper"/> (لا حكم محلي في الشاشة).
/// <para>
/// <b>مصدر البيانات:</b> <see cref="IProductDesktopService.GetAllAsync"/> لملء قائمة الأصناف،
/// و<see cref="IProductDesktopService.ResolveBarcodeAsync"/> للبحث بالباركود. الباركود لم يعد على
/// مستوى الصنف (قرار المالك 2026-11-20): مصدره الوحيد <c>ProductUnits.Barcode</c>، فالبحث يعيد
/// الصنف **ووحدته** (الكرتون/العلبة/الوحدة الأساسية) ومعامل تحويلها، والملصق يطبع باركود تلك الوحدة
/// نفسها ولا يُستبدل أبداً بباركود الوحدة الأساسية.
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

    /// <summary>الباركود المراد طباعته: باركود الوحدة المحلولة من المسح، أو باركود الوحدة الأساسية عند الاختيار اليدوي.</summary>
    private string _labelBarcode = string.Empty;

    /// <summary>
    /// نتيجة حلّ الباركود الممسوح — تحمل <b>الوحدة</b> التي يخصّها الباركود واسمها ومعامل تحويلها
    /// (مصدرها <c>ProductUnits.Barcode</c>؛ الصنف نفسه لا يملك باركوداً — قرار المالك 2026-11-20).
    /// تُصفَّر عند أي اختيار يدوي من القائمة كي لا تُنسَب وحدة مسحٍ سابق للصنف المختار الآن.
    /// </summary>
    private BarcodeResolutionDto? _resolution;

    /// <summary>نص شريط العنوان الأساسي — تُضاف إليه حالة الصنف المحدد (RULE-N44).</summary>
    private const string FormTitle = "طباعة باركود الأصناف";

    public frmInventoryBarcode(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N191 — Enter داخل «بيانات الملصق» يمشي بترتيب العرض (صفاً صفاً من اليمين لليسار) ثم
        // يُسلِّم التركيز لزر الإجراء بلا تنفيذ (RULE-N147). ومعالج الباركود في الشاشة مُوصَّل من
        // المصمّم **قبل** هذا السطر، فيرى Enter أولاً ويستهلكه (e.Handled) فلا ينقل الـHelper التركيز
        // أيضاً — تنفيذ الأمرين بضغطة واحدة هو نفس عيب PILOT-13 (RULE-N193 / RULE-N42).
        KeyboardHelper.WireEnterAsTab(gbInfo, _PrimaryActionButton);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.SelectionRowColor));
        KeyboardHelper.WireArrowNavigation(this);

        // RULE-N43 — مظهر واضح للأزرار المعطّلة.
        KeyboardHelper.WireButtonDisabledVisual(btnPrint, btnClose);

        // RULE-N47 — Esc يغلق الشاشة (لا «جديد/تحديث/ترحيل» على هذه الشاشة).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.Escape, () => { if (btnClose.Enabled) btnClose.PerformClick(); }
        ));

        // RULE-N180…N183 — الحقول المطلوبة في البيت الواحد: أيقونة ✗ + رسالة + ومضة رفض عند مغادرة
        // حقل مطلوب فارغ، و«الحقل الفارغ يحجب Enter» (RULE-N194) بلا أي حكم محلي في هذه الشاشة.
        // ترتيب الاستدعاء إلزامي: **بعد** WireFocusHighlight و**قبل** GreenThemeHelper.Apply (RULE-N183).
        RequiredFieldHelper.Attach(this, errorProvider,
            new(cmbProduct, "يرجى اختيار الصنف أولاً", () => _products.Count > 0),
            new(txtBarcode, "أدخل أو امسح الباركود أولاً"));

        // RULE-N147 — Enter يُبرز زر الإجراء الحالي (زر إجراء واحد في الشاشة: «طباعة الملصقات»).
        AcceptButton = _PrimaryActionButton();

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

    // ── الإجراء الأساسي (RULE-N147 / RULE-N191) ──────────────────────────

    /// <summary>
    /// الزر الأساسي للشاشة — يُقرأ عند كل ضغطة Enter وعند الاشتراك في <c>WireEnterAsTab</c>.
    /// هذه الشاشة لا تملك آلة أوضاع (عرض/إضافة/تعديل): إجراؤها واحد دائماً وهو «طباعة الملصقات»،
    /// و«إغلاق» ليس إجراءً أساسياً (Esc و<see cref="Form.CancelButton"/>).
    /// </summary>
    private Button? _PrimaryActionButton() => btnPrint;

    /// <summary>
    /// تحقق النطاقات — نفس الحكم الحرفي الذي كان مضمَّناً داخل <see cref="btnPrint_Click"/>
    /// (عدد الملصقات يجب أن يكون أكبر من صفر)، مستخرجاً إلى دالة واحدة بلا أي تغيير في الشرط ولا الرسالة.
    /// </summary>
    private bool _ValidateRangeRules()
    {
        int count = (int)nudLabelsCount.Value;
        if (count <= 0)
        {
            clsDialogs.Warning("يرجى إدخال عدد صحيح من الملصقات.");
            return false;
        }

        return true;
    }

    // ── التحميل من دليل الأصناف ─────────────────────────────────────────

    private async void frmInventoryBarcode_Load(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            await _LoadProductsAsync();

            // بداية نظيفة: لا علامة «مطلوب» معلّقة من محاولة سابقة (RULE-N183 — Clear مع كل إعادة ضبط).
            RequiredFieldHelper.Clear(this);

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

        // اختيار يدوي من القائمة = لا وحدة ممسوحة: تُصفَّر نتيجة الحلّ السابقة كي لا يبقى اسم وحدة/معامل
        // تحويل مسحٍ سابق معلَّقاً على صنف آخر (RULE-N167 — لا عرض مضلِّل).
        _resolution = null;

        // الباركود المعروض عند الاختيار اليدوي هو باركود الوحدة الأساسية المسجَّل في الدليل (ProductDto.Barcode
        // مشتقّ من ProductUnits ولا يُختلق)؛ وباركود أي وحدة أخرى يصل عبر البحث بالباركود وحده.
        _labelBarcode = _selectedProduct?.Barcode ?? string.Empty;
        lblBarcodeValue.Text = string.IsNullOrWhiteSpace(_labelBarcode) ? "—" : _labelBarcode;

        errorProvider.SetError(lblBarcodeValue,
            string.IsNullOrWhiteSpace(_labelBarcode) ? "لا يوجد باركود مسجَّل لهذا الصنف" : "");

        pnlBarcodePreview.Invalidate();
        _UpdateModeHeader();
    }

    /// <summary>
    /// RULE-N44 — مؤشر الوضع: اسم الصنف المحدد أو «بلا تحديد»، ويُضاف إليه عند المسح
    /// <b>وحدة الباركود المحلولة</b> واسمها ومعامل تحويلها (الباركود باركود وحدة لا باركود صنف).
    /// يُكتب في <c>lblMode</c> المُعلَن في المصمّم — لا عنصر جديد (RULE-N145).
    /// </summary>
    private void _UpdateModeHeader()
    {
        if (_selectedProduct is null)
        {
            lblMode.Text = _products.Count == 0
                ? $"{FormTitle} — لا توجد أصناف"
                : FormTitle;
            return;
        }

        var header = $"{FormTitle} — {_selectedProduct.Name}";

        if (_resolution is not null && !string.IsNullOrWhiteSpace(_resolution.UnitName))
            header += $" — الوحدة: {_resolution.UnitName} (معامل التحويل: {_UnitFactorText(_resolution)})";

        lblMode.Text = header;
    }

    /// <summary>معامل تحويل وحدة الباركود كنص عربي واحد يستهلكه شريط العنوان والمعاينة ورسالة الطباعة.</summary>
    private static string _UnitFactorText(BarcodeResolutionDto resolution)
        => clsFormat.Quantity(resolution.ConversionFactor);

    // ── RULE-N42: بحث فوري بالباركود ─────────────────────────────────────

    private void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        // استهلاك Enter هنا يُبطل نقل التركيز في KeyboardHelper.WireEnterAsTab(scope, …) (RULE-N193):
        // البحث يعمل ويبقى الحقل جاهزاً للمسح التالي، ولا يُنفَّذ أمران بضغطة واحدة.
        e.Handled = true;
        e.SuppressKeyPress = true;

        _FindByBarcodeAsync();
    }

    /// <summary>
    /// يبحث عن الصنف بالباركود عبر العقد: يُحدَّد فوراً إن وُجد، وإلا رسالة عربية ويُبقى النص للتعديل.
    /// </summary>
    private async void _FindByBarcodeAsync()
    {
        try
        {
            // «الباركود مطلوب» حكمه ووسمه في البيت الواحد RequiredFieldHelper (RULE-N172/RULE-N183):
            // أيقونة ✗ + الرسالة + ومضة الرفض من قواعد الشاشة المعلنة في المُنشئ — لا نسخة محلية.
            if (!RequiredFieldHelper.Validate(this))
            {
                if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
                return;
            }

            Cursor = Cursors.WaitCursor;

            string code = txtBarcode.Text.Trim();

            // المصدر الوحيد للباركود هو ProductUnits.Barcode — الصنف نفسه لا يملك باركوداً (قرار المالك
            // 2026-11-20). الحلّ يعيد الصنف **و** الوحدة التي يخصّها الكود واسمها ومعامل تحويلها، بدل
            // GetByBarcodeAsync الذي كان يعتمد على باركود الصنف المشتقّ من الوحدة الأساسية فقط.
            var result = await ProductService.ResolveBarcodeAsync(code);
            if (!result.IsSuccess || result.Value is null)
            {
                errorProvider.SetError(txtBarcode, "لا يوجد صنف بهذا الباركود");
                clsInputFeedback.ShowRejectTooltip(txtBarcode, "لا يوجد صنف بهذا الباركود", tooltip);
                clsDialogs.Warning($"لا يوجد صنف بهذا الباركود: {code}");
                return;
            }

            // نجاح البحث = القيمة موجودة: تُرفع علامة «مطلوب» من البيت الواحد (لا مسح محلي للأيقونة).
            RequiredFieldHelper.Clear(this);

            var resolution = result.Value;
            var listed = _products.FirstOrDefault(p => p.Id == resolution.ProductId);

            if (listed is not null)
            {
                // اختيار الصنف يرفع SelectedIndexChanged فتتحدث المعاينة وشريط العنوان — والتعبئة الصريحة
                // تُبقي الحالة صحيحة حتى إن كان الصنف مختاراً في القائمة أصلاً فلم يرتفع الحدث.
                cmbProduct.SelectedItem = listed;
                _selectedProduct = listed;
            }
            else
            {
                // صنف غير نشط لا يظهر في القائمة: يُجلب بالمعرّف ويُعرض مباشرة مع تنبيه صريح.
                var lookup = await ProductService.GetByIdAsync(resolution.ProductId);
                if (lookup.IsSuccess && lookup.Value is not null)
                {
                    _selectedProduct = lookup.Value;
                    clsDialogs.Warning("الصنف الممسوح غير نشط — لا يظهر في قائمة الأصناف.");
                }
                else
                {
                    _selectedProduct = null;
                    clsDialogs.Warning(lookup.Error ?? "تعذر تحميل بيانات الصنف الممسوح.");
                }
            }

            // الباركود المطبوع هو باركود **الوحدة التي يخصّها الكود الممسوح** كما أعاده الحلّ:
            // الكرتون/الباكت له باركود يختلف عن باركود الوحدة الأساسية، وطباعة باركود الأساس
            // تُخرج ملصقاً لا يطابق ما مسحه المستخدم. ووحدة الحلّ تُعرض باسمها ومعامل تحويلها.
            _resolution = resolution;
            _labelBarcode = string.IsNullOrWhiteSpace(resolution.Barcode) ? code : resolution.Barcode;
            lblBarcodeValue.Text = _labelBarcode;
            errorProvider.SetError(lblBarcodeValue, "");
            _UpdateModeHeader();
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
            // الحقول المطلوبة (الصنف + الباركود) حكمها ووسمها في البيت الواحد RequiredFieldHelper
            // (RULE-N172/RULE-N183): يُوسَم كل ناقص ثم ينتقل المؤشر إلى أوله — بلا نسخة محلية.
            if (!RequiredFieldHelper.Validate(this))
            {
                if (RequiredFieldHelper.FirstMissing(this) is Control firstMissing) firstMissing.Focus();
                return;
            }

            // شرطُ الطباعة المشتقّ (لا حقل إدخال): الصنف المحدد نفسه + باركوده المسجَّل في الدليل.
            // تغذيةُ حقلَي «الصنف/الباركود» عند نقصهما ملكُ RequiredFieldHelper أعلاه (RULE-N172/RULE-N183).
            var product = _selectedProduct;
            if (product is null)
            {
                clsDialogs.Warning("يرجى اختيار الصنف أولاً.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_labelBarcode))
            {
                clsDialogs.Warning("لا يوجد باركود مسجَّل لهذا الصنف — أضف الباركود من دليل الأصناف أولاً.");
                return;
            }

            if (!_ValidateRangeRules()) return;

            int count = (int)nudLabelsCount.Value;

            string sizeName = cmbLabelSize.SelectedItem?.ToString() ?? "متوسط";

            // وحدة الباركود الممسوح — تُذكر في المعاينة فقط عند مسح باركود وحدة معروفة (لا اختلاق).
            string unitLine = _resolution is not null && !string.IsNullOrWhiteSpace(_resolution.UnitName)
                ? $"وحدة الباركود: {_resolution.UnitName} (معامل التحويل: {_UnitFactorText(_resolution)})\r\n"
                : string.Empty;

            Cursor = Cursors.WaitCursor;

            // لا خدمة طباعة في هذه النسخة — رسالة صريحة بدل ادّعاء نجاح الطباعة.
            clsDialogs.Info(
                "معاينة الملصق جاهزة، لكن الطباعة الفعلية لم تُوصَّل بعد (لا توجد خدمة طباعة في هذه النسخة).\r\n" +
                $"الصنف: {product.Name}\r\n" +
                $"الباركود: {_labelBarcode}\r\n" +
                unitLine +
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

    // ── رسم معاينة الباركود (من باركود الوحدة المسجَّل فعلاً) ─────────────

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

        // المجموعة + الرصيد الحالي + وحدة الباركود المحلولة — حقول حقيقية من دليل الأصناف وحلّ الباركود
        using (var infoFont = new Font("Tahoma", 9.5F))
        {
            string category = string.IsNullOrWhiteSpace(_selectedProduct.CategoryName)
                ? "—"
                : _selectedProduct.CategoryName;

            string info =
                $"المجموعة: {category} | الرصيد الحالي: {clsFormat.Quantity(_selectedProduct.CurrentStock)}";

            // الباركود يخصّ وحدة: تُعرض الوحدة ومعامل تحويلها على الملصق نفسه (لا في عنصر منفصل — RULE-N145).
            if (_resolution is not null && !string.IsNullOrWhiteSpace(_resolution.UnitName))
                info += $" | الوحدة: {_resolution.UnitName} (معامل التحويل: {_UnitFactorText(_resolution)})";

            TextRenderer.DrawText(g,
                info,
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

    private void lblLabelsCount_Click(object sender, EventArgs e)
    {

    }
}
