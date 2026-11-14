using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;

using DtoDiscountType = SalesSystem.DTO.Enums.DiscountType;
using DtoPaymentType = SalesSystem.DTO.Enums.PaymentType;

namespace SalesSystem.Desktop.Screens.POS.Forms;

/// <summary>
/// نقطة بيع تجزئة (§2.25 / RULE-N80…N85): اختيار سريع للأصناف + باركود ← سلة قابلة للتحرير
/// ← مجاميع وتفقيط ← دفع ← إنشاء وترحيل <c>SalesInvoice</c> ذرّياً.
/// الكتل مرتّبة ومجمّعة حسب §2.34: العنوان ← «أوامر البيع» ← «العميل والدفع» ← «سلة الأصناف»
/// ← «الإجماليات» ← الشريط السفلي (RULE-N135…N137) — بلا <c>gbFilter</c>/<c>pnlSummary</c>
/// لأن الشاشة بيع سريع (RULE-N81).
/// لا تصل الشاشة إلى قاعدة البيانات ولا إلى BLL مباشرةً؛ كل شيء عبر <c>I*DesktopService</c>
/// (Mock في وضع التجربة و Direct في الوضع الحقيقي) — RULE-N82.
/// </summary>
public partial class frmPOS : Form
{
    // ═════════════════════════════════════════════════════════════════
    //  الحالة
    // ═════════════════════════════════════════════════════════════════

    private readonly IServiceProvider _serviceProvider;

    /// <summary>سلة البيع — حالة عرض فقط (وليست بيانات تجريبية).</summary>
    private readonly BindingList<CartLine> _cart = new();

    /// <summary>أصناف الكتالوج المحمّلة من الخدمة.</summary>
    private readonly List<ProductDto> _allProducts = new();

    /// <summary>كاش وحدات البيع لكل صنف (تُجلب مرة واحدة لكل صنف).</summary>
    private readonly Dictionary<int, List<ProductUnitDto>> _unitsCache = new();

    private CartLine? _editingLine;      // السطر المحمّل في محرر pnlLine
    private string? _lastAddedInfo;      // آخر صنف أُضيف (يظهر في lblInvoiceInfo)
    private bool _autoPaid = true;       // هل المدفوع مُزامَن تلقائياً مع الصافي (بيع نقدي)؟
    private bool _isBusy;                // حارس ضد الحفظ المزدوج
    private int _updateDepth;            // حارس إعادة الدخول (re-entrancy)
    private int? _pendingInvoiceId;      // فاتورة أُنشئت ولم تُرحَّل بعد

    private bool IsUpdating => _updateDepth > 0;

    public frmPOS(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        ApplyUiTheme();

        // RULE-N95 — الستة من KeyboardHelper (تنقل + تحديد + تظليل + أسهم + أزرار + اختصارات)
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));   // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                     // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnNew, btnSave, btnRemoveLine, btnApplyLine); // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                      // RULE-N47
            (Keys.F2, () => btnNew.PerformClick()),
            (Keys.F4, () => btnSave.PerformClick()),
            (Keys.F5, () => btnRefreshCatalog.PerformClick()),
            (Keys.Escape, () => btnClose.PerformClick()),
            (Keys.Delete, DeleteSelectedLineIfAllowed));

        // RULE-N147 — Enter = الزر الأساسي للشاشة («حفظ وترحيل»)، ولا يعمل داخل حقل أو شبكة
        // (Enter في الباركود = بحث فوري RULE-N42، وفي السلة = تنقّل بين السطور RULE-N80/N85).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        AcceptButton = _PrimaryActionButton();

        WireEvents();
        SetupGrid();
    }

    /// <summary>
    /// الزر الأساسي لـ <c>Enter</c> (RULE-N147): «حفظ وترحيل» — ويعود <c>null</c> عندما يكون
    /// التركيز داخل حقل الباركود أو شبكة السلة حتى يبقى <c>Enter</c> هناك للعنصر نفسه:
    /// بحث فوري بالباركود (RULE-N42) وتنقّل داخل السلة بلا ترحيل صامت (RULE-N80/N85).
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (txtBarcode.ContainsFocus || dgvCart.ContainsFocus) return null;

        return btnSave;
    }

    // ═════════════════════════════════════════════════════════════════
    //  الخدمات — كلها عبر الواجهات فقط (RULE-N03/N04/N05)
    // ═════════════════════════════════════════════════════════════════

    private IProductDesktopService ProductApi => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private ISalesInvoiceDesktopService SalesApi => _serviceProvider.GetRequiredService<ISalesInvoiceDesktopService>();
    private ICustomerDesktopService CustomerApi => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
    private IWarehouseDesktopService WarehouseApi => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private ICashBoxDesktopService CashBoxApi => _serviceProvider.GetRequiredService<ICashBoxDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ═════════════════════════════════════════════════════════════════
    //  المظهر (RULE-N50…N54)
    // ═════════════════════════════════════════════════════════════════

    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);

        ThemeHelper.ApplyButtonStyle(btnSave, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnNew, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnApplyLine, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnRemoveLine, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnRefreshCatalog, ThemeHelper.ButtonType.Secondary);

        // أزرار نقطة البيع أكبر من القياسي — تُضبط بعد ApplyButtonStyle (يفرض AutoSize/ارتفاع 35)
        foreach (var button in new[] { btnSave, btnNew, btnClose, btnApplyLine, btnRemoveLine, btnRefreshCatalog })
        {
            button.AutoSize = false;
            button.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        }

        btnSave.Size = new Size(202, 46);
        btnNew.Size = new Size(148, 46);
        btnClose.Size = new Size(126, 46);
        btnApplyLine.Size = new Size(100, 40);
        btnRemoveLine.Size = new Size(128, 40);
        btnRefreshCatalog.Size = new Size(142, 40);
        btnSave.Font = new Font("Tahoma", 12F, FontStyle.Bold);

        // RULE-N47 — كل زر يحمل اختصاره في ToolTip
        toolTip.SetToolTip(btnNew, "جديد — تفريغ السلة وبدء فاتورة جديدة [F2]");
        toolTip.SetToolTip(btnSave, "حفظ وترحيل الفاتورة [F4]");
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة [Esc]");
        toolTip.SetToolTip(btnRemoveLine, "حذف السطر المحدد [Delete]");
        toolTip.SetToolTip(btnApplyLine, "تطبيق الكمية/السعر/الخصم على السطر المحدد");
        toolTip.SetToolTip(btnRefreshCatalog, "إعادة تحميل قائمة الأصناف [F5]");
        toolTip.SetToolTip(txtBarcode, "امسح الباركود ثم اضغط Enter لإضافة الصنف");
        toolTip.SetToolTip(numQty, "كمية السطر المحدد (3 خانات عشرية)");
        toolTip.SetToolTip(numUnitPrice, "سعر بيع الوحدة للسطر المحدد");
        toolTip.SetToolTip(numLineDiscount, "خصم السطر المحدد (مبلغ)");

        // الألوان من الـ Helper فقط، وقيمها مطابقة حرفياً لما أُعلن في المصمّم
        // (RULE-N54 / RULE-N134 / RULE-N140…N144): المصمّم = وقت التشغيل.
        lblInvoiceInfo.BackColor = GreenThemeHelper.HeaderBackground;
        lblInvoiceInfo.ForeColor = GreenThemeHelper.HeaderForeground;
        lblSelectedProduct.ForeColor = GreenThemeHelper.TextStrong;
        lblNetTotal.ForeColor = GreenThemeHelper.TextStrong;
        lblTafqit.ForeColor = GreenThemeHelper.TextStrong;
        lblRemaining.ForeColor = GreenThemeHelper.TextStrong;

        // RULE-N50 — الترتيب الأخير إلزامي: العنوان ← الشبكة ← الشاشة كاملة
        GreenThemeHelper.ApplyHeader(lblMode);                                     // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvCart);                                       // RULE-N51/N52 — لا يمسّ ReadOnly/SelectionMode/AllowUserToAddRows (RULE-N83)
        GreenThemeHelper.Apply(this);                                              // RULE-N50 — الأخير دائماً
    }

    // ═════════════════════════════════════════════════════════════════
    //  التوصيل والتهيئة
    // ═════════════════════════════════════════════════════════════════

    private void SetupGrid()
    {
        dgvCart.AutoGenerateColumns = false;
        dgvCart.DataSource = _cart;
    }

    private void WireEvents()
    {
        txtBarcode.KeyDown += txtBarcode_KeyDown;

        txtProductSearch.TextChanged += txtProductSearch_TextChanged;
        cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
        btnRefreshCatalog.Click += btnRefreshCatalog_Click;

        dgvCart.SelectionChanged += dgvCart_SelectionChanged;
        dgvCart.CellEndEdit += dgvCart_CellEndEdit;
        dgvCart.CellValueChanged += dgvCart_CellValueChanged;
        dgvCart.DataError += dgvCart_DataError;

        btnApplyLine.Click += btnApplyLine_Click;
        btnRemoveLine.Click += btnRemoveLine_Click;
        btnNew.Click += btnNew_Click;
        btnSave.Click += btnSave_Click;
        btnClose.Click += btnClose_Click;

        numInvoiceDiscount.ValueChanged += InvoiceDiscount_ValueChanged;
        numPaid.ValueChanged += Paid_ValueChanged;
        rbtnCash.CheckedChanged += PaymentType_CheckedChanged;
        rbtnCredit.CheckedChanged += PaymentType_CheckedChanged;
        cbCustomer.SelectedIndexChanged += Lookup_SelectedIndexChanged;
        cbWarehouse.SelectedIndexChanged += Lookup_SelectedIndexChanged;
        cbCashBox.SelectedIndexChanged += Lookup_SelectedIndexChanged;

        // §2.20 بند 3 / RULE-N97 — تفقيط حيّ على الحقول الرقمية
        LiveTafqeetHelper.Attach(
            toolTip,
            (numQty, "الكمية", LiveTafqeetHelper.ReadMode.Quantity),
            (numUnitPrice, "سعر الوحدة", LiveTafqeetHelper.ReadMode.Money),
            (numLineDiscount, "خصم السطر", LiveTafqeetHelper.ReadMode.Money),
            (numInvoiceDiscount, "خصم الفاتورة", LiveTafqeetHelper.ReadMode.Money),
            (numPaid, "المدفوع", LiveTafqeetHelper.ReadMode.Money));
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            Cursor = Cursors.WaitCursor;

            await LoadCatalogAsync();
            await LoadLookupsAsync();

            UpdateInvoiceInfoLabel();
            RecalculateTotals();
            UpdateButtonState();

            ActiveControl = txtBarcode;
            txtBarcode.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحميل شاشة نقطة البيع");
            clsDialogs.Error("حدث خطأ أثناء تحميل شاشة نقطة البيع. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  تحميل البيانات (Mock/Real بنفس المسار — RULE-N82)
    // ═════════════════════════════════════════════════════════════════

    private async Task LoadCatalogAsync()
    {
        var result = await ProductApi.GetAllAsync();
        if (!result.IsSuccess || result.Value is null)
        {
            clsDialogs.Error(result.Error ?? "تعذر تحميل قائمة الأصناف.");
            return;
        }

        _allProducts.Clear();
        _allProducts.AddRange(result.Value.Where(p => p.IsActive));
        _unitsCache.Clear();

        BuildCategoryFilter();
        RenderProductCards();
    }

    /// <summary>يعيد بناء قائمة التصنيفات من الأصناف المحمّلة (تُربط بـ CategoryId — RULE-N60).</summary>
    private void BuildCategoryFilter()
    {
        var categories = new List<CategoryFilter> { new(0, "كل التصنيفات") };

        categories.AddRange(_allProducts
            .Where(p => p.CategoryId > 0)
            .GroupBy(p => p.CategoryId)
            .Select(g => new CategoryFilter(g.Key, g.First().CategoryName ?? $"تصنيف {g.Key}"))
            .OrderBy(c => c.Name));

        BeginUpdate();
        try
        {
            cbCategory.DisplayMember = nameof(CategoryFilter.Name);
            cbCategory.ValueMember = nameof(CategoryFilter.Id);
            cbCategory.DataSource = categories;
            cbCategory.SelectedIndex = 0;
        }
        finally
        {
            EndUpdate();
        }
    }

    private async Task LoadLookupsAsync()
    {
        // ── العملاء: «عميل نقدي» أولاً بمعرّف 0 ──
        // BLL يستبدل CustomerId <= 0 في البيع النقدي بـ DefaultCashCustomerId تلقائياً،
        // أما البيع الآجل فيتطلب عميلاً حقيقياً (تحقق صريح في ValidateBeforeSave).
        var customersResult = await CustomerApi.GetAllAsync();
        var customers = new List<CustomerDto> { new(0, "عميل نقدي", null, null, null, null, 0m, true, 0) };
        if (customersResult.IsSuccess && customersResult.Value is not null)
            customers.AddRange(customersResult.Value.Where(c => c.IsActive));

        BeginUpdate();
        try
        {
            cbCustomer.DisplayMember = nameof(CustomerDto.Name);
            cbCustomer.ValueMember = nameof(CustomerDto.Id);
            cbCustomer.DataSource = customers;
            cbCustomer.SelectedIndex = 0;
        }
        finally
        {
            EndUpdate();
        }

        // ── المخازن ──
        var warehousesResult = await WarehouseApi.GetAllAsync();
        if (warehousesResult.IsSuccess && warehousesResult.Value is not null)
        {
            var warehouses = warehousesResult.Value.Where(w => w.IsActive).ToList();
            BeginUpdate();
            try
            {
                cbWarehouse.DisplayMember = nameof(WarehouseDto.Name);
                cbWarehouse.ValueMember = nameof(WarehouseDto.Id);
                cbWarehouse.DataSource = warehouses;
                if (warehouses.Count > 0) cbWarehouse.SelectedIndex = 0;
            }
            finally
            {
                EndUpdate();
            }
        }

        // ── الصناديق النقدية ──
        var cashBoxesResult = await CashBoxApi.GetAllAsync();
        if (cashBoxesResult.IsSuccess && cashBoxesResult.Value is not null)
        {
            var cashBoxes = cashBoxesResult.Value.Where(c => c.IsActive).ToList();
            BeginUpdate();
            try
            {
                cbCashBox.DisplayMember = nameof(CashBoxDto.Name);
                cbCashBox.ValueMember = nameof(CashBoxDto.Id);
                cbCashBox.DataSource = cashBoxes;
                if (cashBoxes.Count > 0) cbCashBox.SelectedIndex = 0;
            }
            finally
            {
                EndUpdate();
            }
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  كروت الأصناف (الاختيار السريع — RULE-N81)
    // ═════════════════════════════════════════════════════════════════

    private void RenderProductCards()
    {
        if (IsUpdating) return;

        flpProducts.SuspendLayout();
        try
        {
            flpProducts.Controls.Clear();

            var search = txtProductSearch.Text.Trim();
            int? categoryId = cbCategory.SelectedValue is int id && id > 0 ? id : null;

            var matches = _allProducts.Where(p =>
                (categoryId is null || p.CategoryId == categoryId.Value) &&
                (search.Length == 0
                 || p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
                 || (p.Barcode ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase)
                 || (p.CategoryName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase)));

            foreach (var product in matches)
                flpProducts.Controls.Add(CreateProductCard(product));

            if (flpProducts.Controls.Count == 0)
            {
                flpProducts.Controls.Add(new Label
                {
                    Text = "لا توجد أصناف مطابقة للبحث.",
                    AutoSize = false,
                    Size = new Size(340, 40),
                    Margin = new Padding(4),
                    Font = new Font("Tahoma", 10F, FontStyle.Bold),
                    ForeColor = GreenThemeHelper.TextStrong,
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }
        }
        finally
        {
            flpProducts.ResumeLayout(true);
        }
    }

    private Panel CreateProductCard(ProductDto product)
    {
        var card = new Panel
        {
            Width = 344,
            Height = 68,
            Margin = new Padding(4),
            Padding = new Padding(4),
            BackColor = GreenThemeHelper.GroupBackground,
            Cursor = Cursors.Hand,
            Tag = product
        };

        var lblName = new Label
        {
            Text = product.Name,
            Font = new Font("Tahoma", 10F, FontStyle.Bold),
            ForeColor = GreenThemeHelper.TextStrong,
            Location = new Point(108, 4),
            Size = new Size(226, 24),
            TextAlign = ContentAlignment.MiddleRight,
            AutoEllipsis = true,
            Cursor = Cursors.Hand,
            Tag = product
        };

        var lblCode = new Label
        {
            Text = string.IsNullOrWhiteSpace(product.Barcode) ? "بلا باركود" : product.Barcode,
            Font = new Font("Tahoma", 8F),
            ForeColor = ThemeHelper.TextSecondary,
            Location = new Point(108, 27),
            Size = new Size(226, 17),
            TextAlign = ContentAlignment.MiddleRight,
            AutoEllipsis = true,
            Cursor = Cursors.Hand,
            Tag = product
        };

        var lblStock = new Label
        {
            Text = $"المتاح: {product.CurrentStock:0.###}",
            Font = new Font("Tahoma", 9F, FontStyle.Bold),
            ForeColor = GreenThemeHelper.TextStrong,
            Location = new Point(108, 45),
            Size = new Size(226, 18),
            TextAlign = ContentAlignment.MiddleRight,
            AutoEllipsis = true,
            Cursor = Cursors.Hand,
            Tag = product
        };

        var lblAdd = new Label
        {
            Text = "إضافة",
            Font = new Font("Tahoma", 9F, FontStyle.Bold),
            BackColor = GreenThemeHelper.HeaderBackground,
            ForeColor = GreenThemeHelper.HeaderForeground,
            Location = new Point(8, 14),
            Size = new Size(92, 40),
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            Tag = product
        };

        card.Controls.AddRange(new Control[] { lblName, lblCode, lblStock, lblAdd });

        card.Click += ProductCard_Click;
        foreach (Control child in card.Controls)
            child.Click += ProductCard_Click;

        return card;
    }

    private async void ProductCard_Click(object? sender, EventArgs e)
    {
        if ((sender as Control)?.Tag is not ProductDto product) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            await AddProductToCartAsync(product, null);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر إضافة الصنف من الكتالوج");
            clsDialogs.Error("تعذر إضافة الصنف. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
            txtBarcode.Focus();
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  الباركود (RULE-N42 / RULE-N80)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// Enter في حقل الباركود يعترضه <see cref="ProcessCmdKey"/> قبل وصوله إلى
    /// <c>WireEnterAsTab</c> حتى لا ينتقل التركيز بعيداً عن حقل السكانر.
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter && txtBarcode.Focused)
        {
            txtBarcode_KeyDown(txtBarcode, new KeyEventArgs(Keys.Enter));
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// RULE-N147 + RULE-N80: <c>AcceptButton</c> في WinForms يسبق <c>KeyDown</c> ويلتقط <c>Enter</c>،
    /// فنمنعه داخل حقل الباركود وشبكة السلة وبقية حقول الإدخال كي يبقى <c>Enter</c> هناك للعنصر نفسه
    /// (بحث باركود · تنقّل بين السطور · الحقل التالي — `RULE-N40`) ولا يُرحَّل بيع من داخل حقل.
    /// </summary>
    protected override bool ProcessDialogKey(Keys keyData)
    {
        if (keyData == Keys.Enter && (IsInputFocused() || dgvCart.ContainsFocus))
            return false;   // غير مُعالج ⇒ يمرّ إلى KeyDown فيُطبَّق سلوك العنصر الحالي

        return base.ProcessDialogKey(keyData);
    }

    private async void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        e.Handled = true;
        e.SuppressKeyPress = true;

        var code = txtBarcode.Text.Trim();
        if (code.Length == 0) return;

        try
        {
            Cursor = Cursors.WaitCursor;

            var result = await ProductApi.GetByBarcodeAsync(code);   // بحث فوري
            txtBarcode.Clear();

            if (!result.IsSuccess || result.Value is null)
            {
                clsDialogs.Warning(result.Error ?? "لا يوجد صنف بهذا الباركود.");
                return;
            }

            await AddProductToCartAsync(result.Value, code);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر البحث بالباركود");
            clsDialogs.Error("تعذر البحث بالباركود. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
            txtBarcode.Focus();                                      // التركيز يبقى في السكانر
        }
    }

    /// <summary>
    /// يضيف الصنف إلى السلة بوحدته الصحيحة: وحدة الباركود الممسوح، ثم الوحدة الأساسية، ثم الأولى.
    /// إن كان الصنف بالوحدة نفسها موجوداً ⇒ تُزاد الكمية بمقدار 1 بدل إضافة سطر مكرر.
    /// </summary>
    private async Task AddProductToCartAsync(ProductDto product, string? scannedBarcode)
    {
        var units = await GetUnitsAsync(product.Id);
        if (units.Count == 0)
        {
            clsDialogs.Warning($"لا توجد وحدة بيع معرّفة للصنف «{product.Name}».");
            return;
        }

        var unit = units.FirstOrDefault(u => !string.IsNullOrWhiteSpace(scannedBarcode)
                                             && string.Equals(u.Barcode, scannedBarcode, StringComparison.Ordinal))
                   ?? units.FirstOrDefault(u => u.IsBaseUnit)
                   ?? units[0];

        CartLine line;

        var existing = _cart.FirstOrDefault(l => l.ProductId == product.Id && l.ProductUnitId == unit.Id);
        if (existing is not null)
        {
            existing.Quantity += 1m;
            line = existing;
        }
        else
        {
            line = new CartLine
            {
                LineNo = _cart.Count + 1,
                ProductId = product.Id,
                ProductUnitId = unit.Id,
                Name = product.Name,
                UnitName = unit.UnitName ?? string.Empty,
                Barcode = unit.Barcode ?? product.Barcode ?? string.Empty,
                Quantity = 1m,
                UnitPrice = unit.SalePrice,
                Discount = 0m
            };

            _cart.Add(line);
        }

        RecalculateTotals();
        SelectCartRow(line);

        _lastAddedInfo = $"{product.Name} — {unit.UnitName} — {unit.SalePrice:N2}";
        UpdateInvoiceInfoLabel();
    }

    private async Task<List<ProductUnitDto>> GetUnitsAsync(int productId)
    {
        if (_unitsCache.TryGetValue(productId, out var cached))
            return cached;

        var result = await ProductApi.GetProductUnitsAsync(productId);
        var units = result.IsSuccess && result.Value is not null
            ? result.Value.ToList()
            : new List<ProductUnitDto>();

        _unitsCache[productId] = units;
        return units;
    }

    // ═════════════════════════════════════════════════════════════════
    //  السلة والمجاميع
    // ═════════════════════════════════════════════════════════════════

    private decimal ComputeSubTotal() => _cart.Sum(l => l.LineTotal);

    private decimal ComputeInvoiceDiscount()
    {
        var subTotal = ComputeSubTotal();
        return Math.Clamp(numInvoiceDiscount.Value, 0m, subTotal);
    }

    private decimal ComputeNetTotal() => ComputeSubTotal() - ComputeInvoiceDiscount();

    /// <summary>يعيد حساب المجاميع والتفقيط والباقي بشكل حيّ.</summary>
    private void RecalculateTotals()
    {
        if (IsUpdating) return;

        BeginUpdate();
        try
        {
            decimal subTotal = ComputeSubTotal();
            decimal invoiceDiscount = Math.Clamp(numInvoiceDiscount.Value, 0m, subTotal);
            decimal netTotal = subTotal - invoiceDiscount;

            lblSubTotal.Text = $"المجموع الفرعي: {subTotal:N2}";
            lblDiscountTotal.Text = $"إجمالي الخصم: {invoiceDiscount:N2}";
            lblNetTotal.Text = $"الصافي: {netTotal:N2}";

            // التفقيط الحيّ — نفس نمط شاشة الفاتورة المرجعية
            lblTafqit.Text = netTotal > 0m ? ArabicNumberToWords.ToWords(netTotal) : "—";

            // البيع النقدي: المدفوع يُزامَن مع الصافي ما لم يُعدّله المستخدم يدوياً
            if (rbtnCash.Checked && _autoPaid)
                numPaid.Value = Clamp(netTotal, numPaid);

            decimal paid = numPaid.Value;
            decimal remaining = netTotal - paid;
            bool cash = rbtnCash.Checked;

            lblRemaining.Text = cash ? $"الباقي: {remaining:N2}" : $"المتبقي: {remaining:N2}";
            lblRemaining.ForeColor = cash && remaining != 0m
                ? ThemeHelper.Danger
                : GreenThemeHelper.TextStrong;
        }
        finally
        {
            EndUpdate();
        }
    }

    private static decimal Clamp(decimal value, NumericUpDown nud)
        => Math.Min(Math.Max(value, nud.Minimum), nud.Maximum);

    private void SelectCartRow(CartLine line)
    {
        var index = _cart.IndexOf(line);
        if (index < 0) return;

        dgvCart.ClearSelection();
        dgvCart.CurrentCell = dgvCart.Rows[index].Cells[colQuantity.Index];
        dgvCart.Rows[index].Selected = true;
        LoadLineEditor(line);
    }

    private void RenumberLines()
    {
        for (var i = 0; i < _cart.Count; i++)
            _cart[i].LineNo = i + 1;
    }

    /// <summary>يفرّغ السلة ويعيد الشاشة لبيع جديد (RULE-N85).</summary>
    private void ClearCart(bool focusBarcode = true)
    {
        BeginUpdate();
        try
        {
            _cart.Clear();
            _editingLine = null;
            _lastAddedInfo = null;
            lblSelectedProduct.Text = "لم يُحدد سطر";
            numInvoiceDiscount.Value = 0m;
            _autoPaid = true;
            numPaid.Value = 0m;
            numQty.Value = 1m;
            numUnitPrice.Value = 0m;
            numLineDiscount.Value = 0m;
        }
        finally
        {
            EndUpdate();
        }

        errorProvider.Clear();                 // لا تبقى أيقونات تحقق من بيع سابق
        RecalculateTotals();
        UpdateButtonState();
        UpdateModeHeader();

        if (focusBarcode)
            txtBarcode.Focus();
    }

    // ═════════════════════════════════════════════════════════════════
    //  محرر السطر (pnlLine)
    // ═════════════════════════════════════════════════════════════════

    private void LoadLineEditor(CartLine? line)
    {
        _editingLine = line;

        BeginUpdate();
        try
        {
            if (line is null)
            {
                lblSelectedProduct.Text = "لم يُحدد سطر";
                numQty.Value = 1m;
                numUnitPrice.Value = 0m;
                numLineDiscount.Value = 0m;
            }
            else
            {
                lblSelectedProduct.Text = line.DisplayName;
                numQty.Value = Clamp(line.Quantity, numQty);
                numUnitPrice.Value = Clamp(line.UnitPrice, numUnitPrice);
                numLineDiscount.Value = Clamp(line.Discount, numLineDiscount);
            }
        }
        finally
        {
            EndUpdate();
        }

        UpdateModeHeader();
        UpdateButtonState();
    }

    private void UpdateModeHeader()
    {
        lblMode.Text = _editingLine is null
            ? "نقطة البيع — فاتورة جديدة"
            : "نقطة البيع — تعديل سطر";
    }

    private void UpdateButtonState()
    {
        bool hasLine = _editingLine is not null && _cart.Contains(_editingLine);

        btnRemoveLine.Enabled = hasLine;
        btnApplyLine.Enabled = hasLine;
        numQty.Enabled = hasLine;
        numUnitPrice.Enabled = hasLine;
        numLineDiscount.Enabled = hasLine;
    }

    private void btnApplyLine_Click(object? sender, EventArgs e)
    {
        if (_editingLine is null || !_cart.Contains(_editingLine))
        {
            clsDialogs.Warning("حدد سطراً في الشبكة أولاً.");
            return;
        }

        if (numQty.Value <= 0m)
        {
            clsDialogs.Warning("الكمية يجب أن تكون أكبر من صفر.");
            return;
        }

        _editingLine.Quantity = numQty.Value;
        _editingLine.UnitPrice = numUnitPrice.Value;
        _editingLine.Discount = numLineDiscount.Value;

        RefreshCartRow(_cart.IndexOf(_editingLine));
        RecalculateTotals();
        SelectCartRow(_editingLine);
    }

    private void btnRemoveLine_Click(object? sender, EventArgs e) => DeleteSelectedLine();

    /// <summary>
    /// Delete كاختصار على الشاشة: يُهمل داخل أي حقل إدخال خارج <c>dgvCart</c>
    /// (باركود · بحث · قوائم · حقول رقمية) فيبقى للحقل نفسه (حذف حرف) ولا يُسحب
    /// لحذف سطر السلة صامتاً — نمط <c>IsInputFocused</c> في frmAddUpdateSalesInvoice (RULE-N47).
    /// </summary>
    private void DeleteSelectedLineIfAllowed()
    {
        if (IsInputFocused()) return;
        DeleteSelectedLine();
    }

    /// <summary>
    /// هل التركيز داخل حقل إدخال خارج الشبكة؟ يُستخدم <see cref="Control.ContainsFocus"/>
    /// لأن الحقول الرقمية والقوائم قد تكون التركيز في محرّرها الداخلي لا فيها نفسها.
    /// </summary>
    private bool IsInputFocused()
        => txtBarcode.ContainsFocus || txtProductSearch.ContainsFocus
           || cbCategory.ContainsFocus || cbCustomer.ContainsFocus
           || cbWarehouse.ContainsFocus || cbCashBox.ContainsFocus
           || numQty.ContainsFocus || numUnitPrice.ContainsFocus || numLineDiscount.ContainsFocus
           || numInvoiceDiscount.ContainsFocus || numPaid.ContainsFocus;

    private void DeleteSelectedLine()
    {
        if (dgvCart.IsCurrentCellInEditMode) return;   // Delete داخل الخلية = حذف نص

        var line = _editingLine ?? dgvCart.CurrentRow?.DataBoundItem as CartLine;
        if (line is null) return;

        var index = _cart.IndexOf(line);
        if (index < 0) return;

        _cart.RemoveAt(index);
        RenumberLines();

        BeginUpdate();
        try
        {
            _editingLine = null;
            lblSelectedProduct.Text = "لم يُحدد سطر";
            numQty.Value = 1m;
            numUnitPrice.Value = 0m;
            numLineDiscount.Value = 0m;
        }
        finally
        {
            EndUpdate();
        }

        RecalculateTotals();

        if (_cart.Count > 0)
        {
            SelectCartRow(_cart[Math.Min(index, _cart.Count - 1)]);
            return;
        }

        UpdateModeHeader();
        UpdateButtonState();
    }

    // ═════════════════════════════════════════════════════════════════
    //  أحداث الشبكة (RULE-N83 — شبكة السلة قابلة للتحرير جزئياً)
    // ═════════════════════════════════════════════════════════════════

    private void dgvCart_SelectionChanged(object? sender, EventArgs e)
    {
        if (IsUpdating) return;

        var line = dgvCart.CurrentRow?.DataBoundItem as CartLine;
        if (!ReferenceEquals(line, _editingLine))
            LoadLineEditor(line);
    }

    private void dgvCart_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (IsUpdating) return;
        if (e.RowIndex < 0 || e.RowIndex >= _cart.Count || e.ColumnIndex < 0) return;

        var property = dgvCart.Columns[e.ColumnIndex].DataPropertyName;
        if (string.IsNullOrEmpty(property)) return;

        var line = _cart[e.RowIndex];
        var entered = dgvCart.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

        if (!TryReadDecimal(entered, out var value))
        {
            RefreshCartRow(e.RowIndex);
            return;
        }

        switch (property)
        {
            case nameof(CartLine.Quantity):
                line.Quantity = value;
                break;
            case nameof(CartLine.UnitPrice):
                line.UnitPrice = value;
                break;
            case nameof(CartLine.Discount):
                line.Discount = value;
                break;
            default:
                return;
        }

        RefreshCartRow(e.RowIndex);   // يعكس أي تقييد طبّقه النموذج على القيمة
        RecalculateTotals();

        if (ReferenceEquals(line, _editingLine))
        {
            BeginUpdate();
            try
            {
                numQty.Value = Clamp(line.Quantity, numQty);
                numUnitPrice.Value = Clamp(line.UnitPrice, numUnitPrice);
                numLineDiscount.Value = Clamp(line.Discount, numLineDiscount);
            }
            finally
            {
                EndUpdate();
            }
        }
    }

    private void dgvCart_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (IsUpdating) return;
        if (e.RowIndex < 0) return;

        RecalculateTotals();
    }

    private void dgvCart_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        // إدخال غير رقمي في خلية كمية/سعر/خصم: لا نُسقط الشاشة ولا نُظهر تفاصيل تقنية
        e.ThrowException = false;
        e.Cancel = false;
    }

    /// <summary>يعيد كتابة قيم السطر من النموذج إلى الخلايا (بعد أي تقييد) دون كسر الحارس.</summary>
    private void RefreshCartRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _cart.Count || rowIndex >= dgvCart.Rows.Count) return;

        var line = _cart[rowIndex];
        var cells = dgvCart.Rows[rowIndex].Cells;

        BeginUpdate();
        try
        {
            cells[colQuantity.Index].Value = line.Quantity;
            cells[colUnitPrice.Index].Value = line.UnitPrice;
            cells[colDiscount.Index].Value = line.Discount;
        }
        finally
        {
            EndUpdate();
        }
    }

    private static bool TryReadDecimal(object? raw, out decimal value)
    {
        value = 0m;

        switch (raw)
        {
            case null:
            case DBNull:
                return false;
            case decimal d:
                value = d;
                return true;
            case int i:
                value = i;
                return true;
            case long l:
                value = l;
                return true;
        }

        var text = raw.ToString();
        if (string.IsNullOrWhiteSpace(text)) return false;

        text = text.Trim().Replace("٫", ".").Replace(",", string.Empty);

        return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value)
               || decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out value);
    }

    // ═════════════════════════════════════════════════════════════════
    //  أحداث التصفية والدفع
    // ═════════════════════════════════════════════════════════════════

    private void txtProductSearch_TextChanged(object? sender, EventArgs e) => RenderProductCards();

    private void cbCategory_SelectedIndexChanged(object? sender, EventArgs e) => RenderProductCards();

    private async void btnRefreshCatalog_Click(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            await LoadCatalogAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "تعذر تحديث كتالوج الأصناف");
            clsDialogs.Error("تعذر تحديث قائمة الأصناف. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
            txtBarcode.Focus();
        }
    }

    private void InvoiceDiscount_ValueChanged(object? sender, EventArgs e)
    {
        if (IsUpdating) return;
        RecalculateTotals();
    }

    private void Paid_ValueChanged(object? sender, EventArgs e)
    {
        if (IsUpdating) return;

        _autoPaid = false;        // المستخدم عدّل المبلغ يدوياً
        RecalculateTotals();
    }

    private void PaymentType_CheckedChanged(object? sender, EventArgs e)
    {
        if (IsUpdating) return;

        BeginUpdate();
        try
        {
            if (rbtnCash.Checked)
            {
                _autoPaid = true;
                numPaid.Enabled = true;
            }
            else
            {
                _autoPaid = false;
                numPaid.Value = 0m;
                numPaid.Enabled = true;
            }
        }
        finally
        {
            EndUpdate();
        }

        RecalculateTotals();
    }

    private void Lookup_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (IsUpdating) return;
        UpdateInvoiceInfoLabel();
    }

    private void UpdateInvoiceInfoLabel()
    {
        var text =
            $"{DateTime.Today:yyyy/MM/dd}   |   المخزن: {cbWarehouse.Text}   |   الصندوق: {cbCashBox.Text}   |   العميل: {cbCustomer.Text}";

        if (!string.IsNullOrWhiteSpace(_lastAddedInfo))
            text += $"   |   آخر صنف: {_lastAddedInfo}";

        lblInvoiceInfo.Text = text;
    }

    // ═════════════════════════════════════════════════════════════════
    //  جديد / إغلاق
    // ═════════════════════════════════════════════════════════════════

    private void btnNew_Click(object? sender, EventArgs e)
    {
        // مسودة لم تُرحَّل: تحذير واضح قبل بدء بيع جديد — ولا يُحذف شيء من هذه الشاشة
        if (_pendingInvoiceId is int pendingDraft)
        {
            if (!clsDialogs.Confirm(
                    $"توجد فاتورة محفوظة كمسودة برقم {pendingDraft} لم تُرحَّل بعد. " +
                    "هل تريد إبقاءها كمسودة وبدء بيع جديد؟"))
            {
                return;   // «لا» — البقاء على الحالة الحالية
            }

            _pendingInvoiceId = null;   // تبقى مسودة في النظام (لا يوجد حذف من نقطة البيع)
        }

        if (_cart.Count > 0 &&
            !clsDialogs.Confirm("سيتم إفراغ سلة البيع وبدء فاتورة جديدة. هل تريد المتابعة؟"))
        {
            return;
        }

        ClearCart();
        UpdateInvoiceInfoLabel();
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        if (_cart.Count > 0 &&
            !clsDialogs.Confirm("توجد سلة غير محفوظة. هل تريد إغلاق الشاشة؟"))
        {
            return;
        }

        Close();
    }

    // ═════════════════════════════════════════════════════════════════
    //  حفظ وترحيل (RULE-N84/RULE-N85)
    // ═════════════════════════════════════════════════════════════════

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (_isBusy) return;

        try
        {
            // فاتورة أُنشئت ولم تُرحَّل بعد: لا نُنشئ ثانية — نُعيد الترحيل فقط (يمنع ازدواج المخزون/النقد/القيد)
            if (_pendingInvoiceId is int pendingInvoiceId)
            {
                _isBusy = true;
                Cursor = Cursors.WaitCursor;

                lblInvoiceInfo.Text = $"جارٍ ترحيل الفاتورة المحفوظة كمسودة رقم {pendingInvoiceId}…";

                var retryPost = await SalesApi.PostAsync(pendingInvoiceId);
                if (!retryPost.IsSuccess)
                {
                    clsDialogs.Warning(
                        $"الفاتورة رقم {pendingInvoiceId} محفوظة كمسودة ولم تُرحَّل بعد: {retryPost.Error}");
                    UpdateInvoiceInfoLabel();
                    return;
                }

                _pendingInvoiceId = null;
                EventBus.Publish(new SaleInvoiceChangedMessage(pendingInvoiceId)); // تحديث لوحة التحكم

                clsDialogs.Success($"تم ترحيل الفاتورة المحفوظة كمسودة رقم {pendingInvoiceId} بنجاح.");

                ClearCart();                                                   // RULE-N85 — الشاشة تبقى مفتوحة
                UpdateInvoiceInfoLabel();
                return;
            }

            if (!ValidateBeforeSave(out var warehouseId, out var cashBoxId, out var customerId,
                    out var paymentType, out var error))
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    clsDialogs.Warning(error);
                }

                return;
            }

            _isBusy = true;
            Cursor = Cursors.WaitCursor;

            var items = _cart.Select(line => new CreateSalesInvoiceLineRequest(
                ProductId: line.ProductId,
                Quantity: line.Quantity,
                UnitPrice: line.UnitPrice,
                ProductUnitId: line.ProductUnitId,
                DiscountType: DtoDiscountType.Amount,
                DiscountRate: line.Discount,
                ExpiryDate: null)).ToList();

            var request = new CreateSalesInvoiceRequest(
                WarehouseId: warehouseId,
                InvoiceNo: null,
                CustomerId: customerId,
                CashBoxId: cashBoxId,
                InvoiceDate: DateTime.Today,
                PaymentType: paymentType,
                DiscountAmount: ComputeInvoiceDiscount(),
                TaxAmount: 0m,
                OtherCharges: 0m,
                PaidAmount: numPaid.Value,
                Notes: null,
                TaxId: null,
                Items: items,
                DiscountType: DtoDiscountType.Amount,
                DiscountRate: null,
                BranchId: null);

            var createResult = await SalesApi.CreateAsync(request);
            if (!createResult.IsSuccess || createResult.Value is null)
            {
                clsDialogs.Error(createResult.Error ?? "تعذر إنشاء الفاتورة.");
                return;
            }

            var created = createResult.Value;
            _pendingInvoiceId = created.Id;                                // لم تُرحَّل بعد — نحمي من إعادة الإنشاء

            var postResult = await SalesApi.PostAsync(created.Id);
            if (!postResult.IsSuccess)
            {
                // لا نُسقط نجاح الإنشاء: الفاتورة محفوظة كمسودة، ونحتفظ بمعرّفها لإعادة الترحيل فقط
                clsDialogs.Warning(
                    $"تم إنشاء الفاتورة رقم {created.InvoiceNo} وحفظها كمسودة، لكن تعذر ترحيلها: {postResult.Error}");
                return;
            }

            _pendingInvoiceId = null;                                      // تم الترحيل — لا مسودة معلّقة

            EventBus.Publish(new SaleInvoiceChangedMessage(created.Id));   // تحديث لوحة التحكم

            clsDialogs.Success($"تم حفظ وترحيل الفاتورة رقم {created.InvoiceNo} بنجاح.");

            ClearCart();                                                   // RULE-N85 — الشاشة تبقى مفتوحة
            UpdateInvoiceInfoLabel();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ وترحيل فاتورة نقطة البيع");
            clsDialogs.Error("حدث خطأ أثناء حفظ الفاتورة. تم تسجيل التفاصيل في السجل.");
        }
        finally
        {
            Cursor = Cursors.Default;
            _isBusy = false;
            UpdateButtonState();
        }
    }

    /// <summary>
    /// تحقق عربي كامل قبل الحفظ. «عميل نقدي» (CustomerId = 0) مقبول في البيع النقدي
    /// لأن BLL يستبدله بـ DefaultCashCustomerId، ومرفوض في البيع الآجل.
    /// </summary>
    private bool ValidateBeforeSave(
        out int warehouseId,
        out int? cashBoxId,
        out int customerId,
        out DtoPaymentType paymentType,
        out string? error)
    {
        warehouseId = 0;
        cashBoxId = null;
        customerId = 0;
        paymentType = rbtnCash.Checked ? DtoPaymentType.Cash : DtoPaymentType.Credit;
        error = null;

        // رسائل عربية على الحقل المخالف (RULE-N28 + §2.21 بند 17)
        errorProvider.Clear();

        if (_cart.Count == 0)
        {
            error = "السلة فارغة. أضف صنفاً واحداً على الأقل.";
            errorProvider.SetError(dgvCart, error);
            return false;
        }

        warehouseId = cbWarehouse.SelectedValue is null ? 0 : Convert.ToInt32(cbWarehouse.SelectedValue);
        if (warehouseId <= 0)
        {
            error = "اختر المخزن أولاً.";
            errorProvider.SetError(cbWarehouse, error);
            return false;
        }

        cashBoxId = cbCashBox.SelectedValue is null ? null : Convert.ToInt32(cbCashBox.SelectedValue);
        if (cashBoxId is null or <= 0)
        {
            error = "اختر الصندوق النقدي أولاً.";
            errorProvider.SetError(cbCashBox, error);
            return false;
        }

        customerId = cbCustomer.SelectedValue is null ? 0 : Convert.ToInt32(cbCustomer.SelectedValue);

        foreach (var line in _cart)
        {
            if (line.Quantity <= 0m)
            {
                error = $"كمية الصنف «{line.Name}» يجب أن تكون أكبر من صفر.";
                errorProvider.SetError(dgvCart, error);
                return false;
            }

            if (line.UnitPrice < 0m)
            {
                error = $"سعر الصنف «{line.Name}» لا يمكن أن يكون سالباً.";
                errorProvider.SetError(dgvCart, error);
                return false;
            }

            if (line.Discount < 0m || line.Discount > line.Quantity * line.UnitPrice)
            {
                error = $"خصم الصنف «{line.Name}» يجب أن يكون بين صفر وقيمة السطر.";
                errorProvider.SetError(dgvCart, error);
                return false;
            }
        }

        decimal netTotal = ComputeNetTotal();

        if (paymentType == DtoPaymentType.Cash)
        {
            if (numPaid.Value < 0m || numPaid.Value > netTotal)
            {
                error = $"المبلغ المدفوع نقداً يجب أن يكون بين 0 و {netTotal:N2}.";
                errorProvider.SetError(numPaid, error);
                return false;
            }
        }
        else if (customerId <= 0)
        {
            error = "البيع الآجل يتطلب اختيار عميل.";
            errorProvider.SetError(cbCustomer, error);
            return false;
        }

        return true;
    }

    // ═════════════════════════════════════════════════════════════════
    //  حارس إعادة الدخول
    // ═════════════════════════════════════════════════════════════════

    private void BeginUpdate() => _updateDepth++;

    private void EndUpdate()
    {
        if (_updateDepth > 0) _updateDepth--;
    }

    // ═════════════════════════════════════════════════════════════════
    //  نماذج العرض الداخلية
    // ═════════════════════════════════════════════════════════════════

    /// <summary>عنصر تصنيف في قائمة التصفية (يُربط بالمعرّف لا بالنص — RULE-N60).</summary>
    private sealed record CategoryFilter(int Id, string Name);

    /// <summary>سطر سلة البيع — حالة عرض داخل الشاشة فقط.</summary>
    private sealed class CartLine : INotifyPropertyChanged
    {
        private int _lineNo;
        private decimal _quantity = 1m;
        private decimal _unitPrice;
        private decimal _discount;

        public int ProductId { get; init; }
        public int ProductUnitId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string UnitName { get; init; } = string.Empty;
        public string Barcode { get; init; } = string.Empty;

        public string DisplayName => string.IsNullOrWhiteSpace(UnitName) ? Name : $"{Name} ({UnitName})";

        public int LineNo
        {
            get => _lineNo;
            set
            {
                if (_lineNo == value) return;
                _lineNo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>الكمية decimal(18,3) — أي قيمة غير موجبة تُقيَّد إلى 0.001 (RULE-002).</summary>
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                var normalized = Math.Round(value <= 0m ? 0.001m : value, 3, MidpointRounding.AwayFromZero);
                if (_quantity == normalized) return;
                _quantity = normalized;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LineTotal));
            }
        }

        /// <summary>سعر الوحدة decimal(18,2) — لا يقبل السالب (RULE-001).</summary>
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                var normalized = Math.Round(value < 0m ? 0m : value, 2, MidpointRounding.AwayFromZero);
                if (_unitPrice == normalized) return;
                _unitPrice = normalized;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LineTotal));
            }
        }

        /// <summary>خصم السطر مبلغاً — مقيَّد بين صفر وقيمة السطر.</summary>
        public decimal Discount
        {
            get => _discount;
            set
            {
                var max = Quantity * UnitPrice;
                var normalized = Math.Round(Math.Clamp(value, 0m, max), 2, MidpointRounding.AwayFromZero);
                if (_discount == normalized) return;
                _discount = normalized;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LineTotal));
            }
        }

        /// <summary>إجمالي السطر: (الكمية × السعر) − الخصم.</summary>
        public decimal LineTotal => (Quantity * UnitPrice) - Discount;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
