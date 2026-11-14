using Serilog;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Models;

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Printing;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.Screens.Returns.Forms;
using SalesSystem.DTO.Responses;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

public partial class frmAddUpdateSalesInvoice : Form
{
    // ── DI ───────────────────────────────────────────────────────────
    private readonly IServiceProvider _serviceProvider;

    // ── Data ─────────────────────────────────────────────────────────
    private SalesInvoiceDto? _invoice;
    private BindingList<InvoiceLineItem> _lines = new();
    private List<ProductDto> _allProducts = new();
    private Dictionary<int, List<ProductUnitDto>> _productUnitsCache = new();
    private List<AccountDto> _bankAccounts = new();
    private List<CashBoxDto> _cashBoxes = new();
    private bool _isUpdating;
    private Guid? _editingLineKey = null;
    private InvoiceLineItem? _editingOriginalItem = null;

    // ── رصيد المستودع الحالي بالوحدات الأساسية (productId → qty) ──
    private readonly Dictionary<int, decimal> _warehouseStockBase = new();
    private int _warehouseStockForWarehouseId = -1;   // -1 = غير محمّل
    private bool _warehouseStockAvailable;            // false = تعذّر التحقق ⇒ اسمح

    // ── Task 1/2/3/4 state ───────────────────────────────────────────
    private decimal _discountBeforeEdit;          // قيمة الخصم قبل التحرير (لاسترجاعها عند إدخال غير صالح)
    private int _stockLookupToken;                // حارس التسابق (race guard) لاستعلام المخزون

    /// <summary>الفاتورة المرحّلة أو الملغاة مقفلة تلقائياً — لا تعديل ولا إضافة سطور.</summary>
    private bool IsLocked => _invoice != null && _invoice.Status is 2 or 3;

    // ── Lazy Service Accessors ──────────────────────────────────────
    private ISalesInvoiceDesktopService _salesApi => _serviceProvider.GetRequiredService<ISalesInvoiceDesktopService>();
    private ICustomerDesktopService _customerApi => _serviceProvider.GetRequiredService<ICustomerDesktopService>();
    private IProductDesktopService _productApi => _serviceProvider.GetRequiredService<IProductDesktopService>();
    private IWarehouseDesktopService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();
    private IInvoicePrinter _invoicePrinter => _serviceProvider.GetRequiredService<IInvoicePrinter>();
    private IReceiptPrinter _receiptPrinter => _serviceProvider.GetRequiredService<IReceiptPrinter>();
    private ISettingsDesktopService _settingsApi => _serviceProvider.GetRequiredService<ISettingsDesktopService>();
    private IAccountDesktopService _accountApi => _serviceProvider.GetRequiredService<IAccountDesktopService>();
    private IInventoryDesktopService _inventoryApi => _serviceProvider.GetRequiredService<IInventoryDesktopService>();
    private ICashBoxDesktopService _cashBoxApi => _serviceProvider.GetRequiredService<ICashBoxDesktopService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();

    // ═════════════════════════════════════════════════════════════════
    //  CONSTRUCTOR
    // ═════════════════════════════════════════════════════════════════

    public frmAddUpdateSalesInvoice(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        KeyPreview = true;
        KeyDown += frmAddUpdateSalesInvoice_KeyDown;

        // تنقل لوحة المفاتيح الموحّد (RULE-N40…RULE-N47).
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireSelectAllOnFocus(this);
        KeyboardHelper.WireFocusHighlight(this, ThemePalette.DefaultFor(ThemePalette.ActiveFieldColor));   // RULE-N46 + RULE-N54b
        KeyboardHelper.WireArrowNavigation(this);                                    // RULE-N45 — أسهم RTL

        ApplyUiTheme();          // RULE-N50 — ينتهي بالثيم الأخضر حصراً
        ApplyGridBehaviour();    // RULE-N22a/N52 — سلوك شبكة السطور تعلنه الشاشة

        // RULE-N43 — آلة حالة الأزرار (بعد تثبيت ألوان المصمّم ليسجّل اللون السليم للاستعادة).
        KeyboardHelper.WireButtonDisabledVisual(
            btnSaveDraft, btnPost, btnCancelInvoice, btnCancelLine,
            btnAddLine, btnEditRow, btnDeleteRow, btnRefreshCatalog, btnPrintA4, btnPrintReceipt);

        // RULE-N47 — اختصارات الشاشة (F4 داخل حقول الأصناف يبقى «بحث عن صنف» — RULE-N42).
        KeyboardHelper.WireGlobalShortcuts(this,
            (Keys.F2, () => btnNew.PerformClick()),          // جديد
            (Keys.F4, HandlePostShortcut),                   // حفظ وترحيل
            (Keys.F5, () => btnRefreshCatalog.PerformClick()),// تحديث بيانات الأصناف
            (Keys.Delete, HandleCancelInvoiceShortcut),       // إلغاء الفاتورة
            (Keys.Escape, HandleEscapeShortcut));             // خروج

        // RULE-N147 — Enter = الزر الأساسي للوضع الحالي (داخل حقل إدخال = الحقل التالي — RULE-N40،
        // وداخل شبكة الأصناف يبقى Enter لإدخال السطر — RULE-N22a).
        KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
        _SyncAcceptButton();   // الزر الافتراضي الابتدائي = الزر الأساسي الحالي

        dgvItems.AutoGenerateColumns = false;
        dgvItems.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvItems.DataSource = _lines;

        // ترتيب أعمدة التحرير داخل الشبكة (#, باركود, وحدة, كمية, سعر, خصم, تاريخ الانتهاء)
        EditableDgvColumns = new[]
        {
            colBarcode.Index, colUnit.Index, colQuantity.Index, colUnitPrice.Index, colDiscount.Index, colExpiry.Index
        };

        // Enable DoubleBuffered for flicker-free rendering
        typeof(DataGridView).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(dgvItems, true, null);

        WireEvents();
    }

    // ═════════════════════════════════════════════════════════════════
    //  WIRE EVENTS (Designer handles button/value events; we add keyboard/DGV)
    // ═════════════════════════════════════════════════════════════════

    private void WireEvents()
    {
        // Entry Panel keyboard navigation
        txtBarcode.KeyDown += txtBarcode_KeyDown;
        cmbUnit.KeyDown += cmbUnit_KeyDown;
        txtQuantity.KeyDown += txtQuantity_KeyDown;
        txtUnitPrice.KeyDown += txtUnitPrice_KeyDown;
        txtLineDiscount.KeyDown += txtLineDiscount_KeyDown;
        dtpLineExpiry.KeyDown += dtpLineExpiry_KeyDown;

        // Live recalc
        txtQuantity.TextChanged += RecalculateLineTotal;
        txtUnitPrice.TextChanged += RecalculateLineTotal;
        txtLineDiscount.TextChanged += RecalculateLineTotal;

        // Numeric validation for discount
        txtLineDiscount.KeyPress += NumericTextBox_KeyPress;

        // Select-all on Enter
        txtBarcode.Enter += NumericTextBox_Enter;
        txtQuantity.Enter += NumericTextBox_Enter;
        txtUnitPrice.Enter += NumericTextBox_Enter;
        txtLineDiscount.Enter += NumericTextBox_Enter;
        txtPaid.Enter += NumericTextBox_Enter;
        //txtBankAmount.Enter += NumericTextBox_Enter;
        txtExpenses.Enter += NumericTextBox_Enter;
        txtNotes.Enter += NumericTextBox_Enter;
        numInvoiceDiscount.Enter += NumericTextBox_Enter;

        // Header/Footer combos — Enter moves to next field
        cmbCustomer.KeyDown += FieldEnter_KeyDown;
        cmbWarehouse.KeyDown += FieldEnter_KeyDown;
        cmbCashBox.KeyDown += FieldEnter_KeyDown;
        cmbDiscountType.KeyDown += FieldEnter_KeyDown;
        //cmbPaymentType.KeyDown += FieldEnter_KeyDown;
        //cmbBank.KeyDown += FieldEnter_KeyDown;
        //cmbTax.KeyDown += FieldEnter_KeyDown;
        cmbAccount.KeyDown += FieldEnter_KeyDown;
        //cmbDiscountType.KeyDown += FieldEnter_KeyDown;
        numInvoiceDiscount.KeyDown += FieldEnter_KeyDown;

        // DGV events (Inline Editing)
        dgvItems.DefaultValuesNeeded += dgvItems_DefaultValuesNeeded;
        dgvItems.CellBeginEdit += dgvItems_CellBeginEdit;
        dgvItems.CellEndEdit += dgvItems_CellEndEdit;
        dgvItems.EditingControlShowing += dgvItems_EditingControlShowing;
        dgvItems.CellEnter += dgvItems_CellEnter;
        dgvItems.UserDeletedRow += (s, e) => CalculateTotals();
        dgvItems.KeyDown += dgvItems_KeyDown;
        dgvItems.DataError += (s, e) => e.ThrowException = false;
        dgvItems.CellFormatting += dgvItems_CellFormatting;
        dgvItems.CellClick += dgvItems_CellClick;
        dgvItems.CellDoubleClick += dgvItems_CellDoubleClick;
        dgvItems.Scroll += (s, e) => HideCellExpiryPicker();
        dgvItems.Leave += (s, e) => HideCellExpiryPicker();

        // RULE-N147 — لا يبقى Enter مرتبطاً بزر الحفظ ما دام التركيز داخل شبكة الأصناف (RULE-N22a).
        dgvItems.Enter += (s, e) => _SyncAcceptButton();
        dgvItems.Leave += (s, e) => _SyncAcceptButton();

        // Context Menu
        tsmiEditRow.Click += (s, e) => { if (dgvItems.CurrentRow?.Index >= 0 && dgvItems.CurrentRow.Index < _lines.Count) LoadRowForEditing(dgvItems.CurrentRow.Index); };
        tsmiCancelEdit.Click += (s, e) => CancelEdit();
        tsmiDeleteRow.Click += (s, e) => { if (dgvItems.CurrentRow?.Index >= 0) DeleteSelectedRow(); };

        // Discount type changed — مبلغ / نسبة
        cmbDiscountType.SelectedIndexChanged += cmbDiscountType_SelectedIndexChanged;
        if (cmbDiscountType.SelectedIndex < 0) cmbDiscountType.SelectedIndex = 0; // "مبلغ" (default)

        // Tax checkbox — when checked (شاملة/inclusive), disable cmbTax since values don't change
        //chkApplyTax.CheckedChanged += chkApplyTax_CheckedChanged;

        // Expenses numeric validation
        txtExpenses.KeyPress += NumericTextBox_KeyPress;

        // Payment area — Enter triggers next field
        txtPaid.KeyDown += FieldEnter_KeyDown;
        //txtBankAmount.KeyDown += FieldEnter_KeyDown;

        // Notes / date — Enter triggers next field
        txtNotes.KeyDown += FieldEnter_KeyDown;
        dtpDate.KeyDown += FieldEnter_KeyDown;

        // Unified focus indicator for all input controls
        ThemeHelper.WireFocusIndicator(
            txtBarcode, cmbUnit, txtQuantity, txtUnitPrice, txtLineDiscount, dtpLineExpiry,  // Entry panel
            txtPaid, txtExpenses, txtNotes,                     // Payment / notes
            cmbCustomer, cmbWarehouse, cmbCashBox,              // Header combos
            cmbAccount, cmbDiscountType,                        // Footer combos
            numInvoiceDiscount, dtpDate                                        // Numeric / date
        );

        // ── المتاح في المخزون (لكل وحدة) — عند تغيير الوحدة أو المستودع ──
        cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
        cmbWarehouse.SelectedIndexChanged += cmbWarehouse_SelectedIndexChanged_RefreshAvailable;

        // ── تفقيط حيّ (RULE-N97): كل حقول المال في الشاشة ──
        // (المدفوع · الخصم · المصاريف · خصم السطر · السعر · الكمية)
        // الإجمالي والمتبقي حقول عرض (lblTafqitValue / lblQuickInfo) لا إدخال ⇒ يُفقيطان نصاً في CalculateTotals.
        LiveTafqeetHelper.Attach(
            liveTip,
            (txtQuantity, "الكمية", LiveTafqeetHelper.ReadMode.Quantity),
            (txtUnitPrice, "السعر", LiveTafqeetHelper.ReadMode.Money),
            (txtLineDiscount, "خصم السطر", LiveTafqeetHelper.ReadMode.Money),
            (txtPaid, "المدفوع", LiveTafqeetHelper.ReadMode.Money),
            (txtExpenses, "المصاريف", LiveTafqeetHelper.ReadMode.Money),
            (numInvoiceDiscount, "الخصم", LiveTafqeetHelper.ReadMode.Money));
    }

    // ═════════════════════════════════════════════════════════════════
    //  FORM LIFECYCLE
    // ═════════════════════════════════════════════════════════════════

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            Cursor = Cursors.WaitCursor;

            await LoadLookups();

            if (_invoice != null)
            {
                BindInvoice();
                // انتظار حلّ الوحدات ومعاملات التحويل قبل تمكين التحرير (يمنع حفظ كمية بمعامل 1 خطأً)
                await LoadAllUnitsForLinesAsync();
            }
            else
            {
                ResetForm();
            }

            UpdateUIState();
            CalculateTotals();

            bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
            if (editable)
            {
                txtBarcode.Focus();
            }
            else
            {
                btnClose.Focus();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحميل الشاشة");
            clsDialogs.Error("حدث خطأ أثناء تحميل الشاشة. يرجى المحاولة مرة أخرى.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    public void LoadData(SalesInvoiceDto? invoice)
    {
        _invoice = invoice;
        ApplyUiTheme();
        _SyncAcceptButton();   // RULE-N147 — الزر الافتراضي يتبع وضع الفاتورة المحمَّلة
    }

    // ═════════════════════════════════════════════════════════════════
    //  STYLES
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// تهيئة المظهر (RULE-N50): <see cref="ThemeHelper"/> يفرض خلفية بيضاء وخط Segoe UI،
    /// ثم يُفرض الثيم الأخضر <b>آخر خطوة حصراً</b> ليردّ ألوان المصمّم.
    /// وألوان المصمّم هي المرجع البصري — تُلتقط قبل ThemeHelper ثم تُعاد حرفياً (RULE-N134/N139/N140).
    /// </summary>
    private void ApplyUiTheme()
    {
        // (1) ألوان المصمّم حرفياً — تُلتقط قبل أن يفرض ThemeHelper أبيض/Segoe UI.
        var designerLook = new Dictionary<Button, (Color Back, Color Fore)>();
        foreach (var btn in ActionButtons)
            designerLook[btn] = (btn.BackColor, btn.ForeColor);

        // (2) ThemeHelper — أبيض/Segoe UI، ويُستدعى قبل الثيم الأخضر دائماً.
        ThemeHelper.ApplyMainFormStyle(this);
        lblMode.Text = _invoice == null ? "فاتورة بيع جديدة" : $"فاتورة بيع - {_invoice.InvoiceNo}";

        ThemeHelper.ApplyButtonStyle(btnSaveDraft, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnPost, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancelInvoice, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        // (3) استعادة شكل المصمّم على أزرار الشاشة (Tahoma 10 Bold · ارتفاع 36 · بلا AutoSize).
        foreach (var btn in ActionButtons)
        {
            btn.AutoSize = false;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn.Height = 36;
            btn.Padding = Padding.Empty;
            btn.UseVisualStyleBackColor = false;
            btn.Cursor = Cursors.Hand;
            btn.BackColor = designerLook[btn].Back;
            btn.ForeColor = designerLook[btn].Fore;
        }

        // (4) الثيم الأخضر — آخر خطوة حصراً (RULE-N50).
        GreenThemeHelper.ApplyHeader(lblMode);
        GreenThemeHelper.ApplyGrid(dgvItems);
        GreenThemeHelper.Apply(this);
    }

    /// <summary>أزرار الشاشة (شريطا الأوامر) — مصدر واحد لتوحيد الشكل وحفظ ألوان المصمّم.</summary>
    private Button[] ActionButtons => new[]
    {
        btnNew, btnSaveDraft, btnPost, btnPrintA4, btnPrintReceipt, btnSalesReturn,
        btnCancelInvoice, btnClose,
        btnAddLine, btnEditRow, btnDeleteRow, btnRefreshCatalog, btnCancelLine
    };

    /// <summary>
    /// سلوك شبكة الأصناف (RULE-N22a / RULE-N52): المساعد <see cref="GreenThemeHelper.ApplyGrid"/>
    /// لا يلمس <c>SelectionMode</c> ولا <c>MultiSelect</c> — الشاشة تُعلن سلوكها هنا:
    /// تحرير داخل الخلية (CellSelect) وإضافة سطر جديد مسموحة.
    /// </summary>
    private void ApplyGridBehaviour()
    {
        dgvItems.MultiSelect = false;
        dgvItems.SelectionMode = DataGridViewSelectionMode.CellSelect;   // تحرير داخل الخلية
    }

    // ═════════════════════════════════════════════════════════════════
    //  DATA LOADING
    // ═════════════════════════════════════════════════════════════════

    private async Task LoadLookups()
    {
        try
        {
            // Customers — must be selected, no default
            var customersRes = await _customerApi.GetAllAsync(null, includeInactive: true);
            if (!customersRes.IsSuccess || customersRes.Value == null)
                throw new InvalidOperationException(customersRes.Error ?? "فشل تحميل قائمة العملاء");
            var list = customersRes.Value.ToList();
            list.Insert(0, new CustomerDto(0, "--- اختر عميل ---", null, null, null, null, 0, true, 0));
            cmbCustomer.DataSource = list;
            cmbCustomer.DisplayMember = "Name";
            cmbCustomer.ValueMember = "Id";
            cmbCustomer.SelectedIndex = 0; // "--- اختر عميل ---"

            // فاتورة جديدة → العميل الافتراضي "عميل نقدي" (إن وُجد)، وإلا يبقى العنصر الأول.
            if (_invoice == null) SelectDefaultCashCustomer();

            // Warehouses
            var warehouseRes = await _warehouseApi.GetAllAsync(includeInactive: true);
            if (!warehouseRes.IsSuccess || warehouseRes.Value == null)
                throw new InvalidOperationException(warehouseRes.Error ?? "فشل تحميل قائمة المستودعات");
            cmbWarehouse.DataSource = warehouseRes.Value.ToList();
            cmbWarehouse.DisplayMember = "Name";
            cmbWarehouse.ValueMember = "Id";

            if (_invoice == null)
            {
                var def = warehouseRes.Value.FirstOrDefault(w => w.IsDefault);
                if (def != null) cmbWarehouse.SelectedValue = def.Id;
            }

            // الصناديق (نشطة فقط) — تُستخدم لتحديد صندوق قبض الفاتورة
            var cashBoxRes = await _cashBoxApi.GetAllAsync(default);
            if (cashBoxRes.IsSuccess && cashBoxRes.Value != null)
            {
                _cashBoxes = cashBoxRes.Value.Where(c => c.IsActive).ToList();
                cmbCashBox.DataSource = _cashBoxes.ToList();
                cmbCashBox.DisplayMember = "Name";
                cmbCashBox.ValueMember = "Id";
                if (_invoice == null && _cashBoxes.Count > 0) cmbCashBox.SelectedIndex = 0;
            }
            else
            {
                Log.Warning("تعذر تحميل قائمة الصناديق: {Error}", cashBoxRes.Error);
            }

            // Products (for barcode lookup)
            var productRes = await _productApi.GetAllAsync(null, null, includeInactive: false);
            if (productRes.IsSuccess && productRes.Value != null)
            {
                _allProducts = productRes.Value.ToList();
            }

            // الضرائب: أُزيلت وحدة الضرائب (V2) — لا قائمة ضرائب مرجعية، فالنسبة = 0
            // (يبقى الحقل معطلاً؛ قيمة TaxAmount في الطلب = 0 و TaxId = null).
            //cmbTax.Enabled = false;

            // Banks from ChartOfAccounts — only accounts under "1102" (البنوك)
            // (تصفية النوع "أصل" تتم هنا لأن GetAllAsync لا يفلتر بالنوع)
            var assetAccounts = await _accountApi.GetAllAsync(includeInactive: false);
            _bankAccounts = assetAccounts.IsSuccess
                ? assetAccounts.Value.Where(a => a.IsLeaf && !a.IsSystem
                    && a.Nature == (byte)SalesSystem.Models.Accounting.Enums.AccountType.Asset
                    && a.AccountCode.StartsWith("1102")).ToList()
                : new List<AccountDto>();

            //cmbBank.DataSource = _bankAccounts;
            //cmbBank.DisplayMember = "NameAr";
            //cmbBank.ValueMember = "Id";
            //if (_bankAccounts.Count > 0) cmbBank.SelectedIndex = 0;

            // Store/Inventory accounts for cmbAccount (المخزون = 1104 prefix)
            var storeAccounts = assetAccounts.IsSuccess
                ? assetAccounts.Value.Where(a => a.IsLeaf && !a.IsSystem
                    && a.Nature == (byte)SalesSystem.Models.Accounting.Enums.AccountType.Asset
                    && a.AccountCode.StartsWith("1104")).ToList()
                : new List<AccountDto>();
            cmbAccount.DataSource = storeAccounts;
            cmbAccount.DisplayMember = "NameAr";
            cmbAccount.ValueMember = "Id";
            if (storeAccounts.Count > 0) cmbAccount.SelectedIndex = 0;

            // Auto-select store account when warehouse changes
            // (إلغاء الاشتراك أولاً حتى لا يتكرر عند إعادة تحميل القوائم من زر "تحديث بيانات الأصناف")
            cmbWarehouse.SelectedIndexChanged -= cmbWarehouse_SelectedIndexChanged;
            cmbWarehouse.SelectedIndexChanged += cmbWarehouse_SelectedIndexChanged;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل القوائم");
            clsDialogs.Error("حدث خطأ أثناء تحميل البيانات. يرجى المحاولة مرة أخرى.");
        }
    }

    private async Task<List<ProductUnitDto>> LoadProductUnitsAsync(int productId)
    {
        if (_productUnitsCache.TryGetValue(productId, out var cached))
            return cached;

        try
        {
            // وحدات المنتج عبر واجهة Desktop (IProductDesktopService) — تعمل في الوضع الحقيقي ووضع التجربة
            var unitsResult = await _productApi.GetProductUnitsAsync(productId, default);
            if (unitsResult.IsSuccess && unitsResult.Value != null)
            {
                var units = unitsResult.Value.ToList();
                _productUnitsCache[productId] = units;
                return units;
            }

            // قائمة وحدات فارغة = حالة بيانات حقيقية (منتج بلا وحدات) — ليست خطأ
            Log.Warning("لا توجد وحدات للمنتج {ProductId}: {Error}", productId, unitsResult.Error);
            return [];
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "خطأ في تحميل وحدات المنتج {ProductId}", productId);
            return [];
        }
    }

    /// <summary>
    /// Loads product units and picks the best default:
    /// preferred unit id → base unit → first available.
    /// </summary>
    private async Task<(List<ProductUnitDto> Units, ProductUnitDto? Selected)> GetUnitsWithDefaultAsync(
        int productId, int preferredUnitId = 0, string? preferredUnitName = null)
    {
        var units = await LoadProductUnitsAsync(productId);
        var selected =
            (preferredUnitId > 0 ? units.FirstOrDefault(u => u.Id == preferredUnitId) : null)
            ?? (preferredUnitName != null ? units.FirstOrDefault(u => u.UnitName == preferredUnitName) : null)
            ?? units.FirstOrDefault(u => u.IsBaseUnit)
            ?? units.FirstOrDefault();
        return (units, selected);
    }

    /// <summary>
    /// Standard duplicate-product warning used by all entry paths.
    /// </summary>
    private void WarnDuplicateProduct()
        => clsDialogs.Warning("هذا المنتج مضاف مسبقاً للقائمة.");

    /// <summary>
    /// معامل تحويل الوحدة المختارة في لوحة الإدخال إلى الوحدة الأساسية (1 عند عدم التحديد).
    /// </summary>
    private decimal GetSelectedConversionFactor()
        => cmbUnit.SelectedItem is ProductUnitDto dto && dto.ConversionFactor > 0m
            ? dto.ConversionFactor
            : 1m;

    /// <summary>
    /// يضمن تحميل أرصدة المستودع المختار مرة واحدة ويعيد false عند تعذّر التحقق.
    /// </summary>
    private async Task<bool> EnsureWarehouseStockAsync(int warehouseId, CancellationToken ct = default)
    {
        if (warehouseId <= 0) return false;
        if (_warehouseStockForWarehouseId == warehouseId) return _warehouseStockAvailable;

        try
        {
            var res = await _inventoryApi.GetStockByWarehouseAsync(warehouseId, ct);
            _warehouseStockBase.Clear();

            if (res.IsSuccess && res.Value != null)
            {
                foreach (var dto in res.Value)
                    _warehouseStockBase[dto.ProductId] = dto.Quantity;

                _warehouseStockForWarehouseId = warehouseId;
                _warehouseStockAvailable = true;
                return true;
            }

            Log.Warning("تعذر تحميل أرصدة المستودع {WarehouseId}: {Error}", warehouseId, res.Error);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر تحميل أرصدة المستودع {WarehouseId}", warehouseId);
            _warehouseStockBase.Clear();
        }

        _warehouseStockForWarehouseId = warehouseId;
        _warehouseStockAvailable = false;
        return false;
    }

    /// <summary>
    /// التحقق من كفاية المخزون بوحدات أساسية:
    ///   neededBase = Σ(كميات السطور الأخرى لنفس المنتج × معامل تحويلها) + (الكمية الجديدة × معامل تحويلها)
    /// ويُقارن بالرصيد المخزني الأساسي للمستودع (من أرصدة IInventoryDesktopService.GetStockByWarehouseAsync).
    /// يُرجع true (السماح) عند تعذّر التحقق — الخادم يحكم عند الترحيل.
    /// </summary>
    private async Task<bool> IsStockSufficientAsync(
        int productId, decimal newQty, decimal newConversionFactor, Guid? excludeLineKey = null)
    {
        if (newQty <= 0) return true;
        if (newConversionFactor <= 0m) newConversionFactor = 1m;
        if (cmbWarehouse.SelectedValue is not short wid || wid <= 0) return true;

        // Σ الكميات الأخرى القائمة لنفس المنتج بالوحدات الأساسية
        decimal otherBase = _lines
            .Where(l => l.ProductId == productId
                && (excludeLineKey == null || l.LineKey != excludeLineKey.Value))
            .Sum(l => l.Quantity * (l.ConversionFactor > 0m ? l.ConversionFactor : 1m));

        decimal neededBase = otherBase + (newQty * newConversionFactor);

        // رصيد المستودع بالوحدات الأساسية (من كاش أرصدة المستودع المختار)
        if (!await EnsureWarehouseStockAsync(wid)) return true; // can't verify here — server decides at posting
        if (!_warehouseStockBase.TryGetValue(productId, out decimal availableBase)) return true;

        if (neededBase <= availableBase) return true;

        decimal availableInUnit = availableBase / newConversionFactor;

        clsDialogs.Warning(
            "الكمية غير كافية في المخزون." +
            $"\nالمتاح بالوحدة المختارة: {clsFormat.Quantity(availableInUnit)} — المطلوب: {clsFormat.Quantity(newQty)}" +
            $"\nالمتاح بالوحدة الأساسية: {clsFormat.Quantity(availableBase)} — المطلوب: {clsFormat.Quantity(neededBase)}");
        return false;
    }

    // ═════════════════════════════════════════════════════════════════
    //  INVOICE BINDING
    // ═════════════════════════════════════════════════════════════════

    private void BindInvoice()
    {
        _isUpdating = true;
        lblMode.Text = $"فاتورة بيع - {_invoice!.InvoiceNo}";
        lblInvoiceNo.Text = _invoice.InvoiceNo.ToString();
        dtpDate.Value = _invoice.InvoiceDate;
        if (_invoice.CustomerId > 0)
            cmbCustomer.SelectedValue = _invoice.CustomerId;
        cmbWarehouse.SelectedValue = Convert.ToInt16(_invoice.WarehouseId);
        if (_invoice.CashBoxId is int cashBoxId && cashBoxId > 0)
            cmbCashBox.SelectedValue = cashBoxId;
        //cmbPaymentType.SelectedIndex = _invoice.PaymentType - 1;

        // Restore discount type (مبلغ / نسبة) then its value
        var discType = (SalesSystem.DTO.Enums.DiscountType)_invoice.DiscountType;
        bool isPercentage = discType == SalesSystem.DTO.Enums.DiscountType.Percentage;
        cmbDiscountType.SelectedIndex = isPercentage ? 1 : 0;
        numInvoiceDiscount.Value = Math.Clamp(
            isPercentage ? (_invoice.DiscountRate ?? 0m) : _invoice.DiscountAmount,
            numInvoiceDiscount.Minimum, numInvoiceDiscount.Maximum);

        // Restore payment amounts — split PaidAmount into cash vs bank
        decimal totalPaid = _invoice.PaidAmount;
        if (_invoice.PaymentType == 3) // Mixed — keep paid amount as-is, bank is separate
        {
            txtPaid.Text = clsFormat.Money(totalPaid);
        }
        else if (_invoice.PaymentType == 2) // Credit — paid should be 0
        {
            txtPaid.Text = "0";
        }
        else // Cash
        {
            txtPaid.Text = clsFormat.Money(totalPaid);
        }

        txtExpenses.Text = clsFormat.Money(_invoice.OtherCharges);
        txtNotes.Text = _invoice.Notes;

        // Build lines with ProductUnitId for correct unit restore
        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItem
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode ?? "",
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                UnitName = "", // will be loaded async
                ProductUnitId = item.ProductUnitId,
                ConversionFactor = 1m, // يُصحَّح عند تحميل الوحدات (LoadAllUnitsForLinesAsync)
                Barcode = item.ProductCode ?? "",
                DiscountAmount = item.DiscountAmount, // خصم السطر (مبلغ) — يعيد إجمالي السطر كما حُفظ
                ExpiryDate = item.ExpiryDate          // تاريخ انتهاء السطر (اختياري)
            });
        }
        UpdatePaymentTypeUI();
        CalculateTotals();

        // تحميل الوحدات ومعاملات التحويل لا يتم هنا — يستدعيه المستدعي وينتظره
        // (LoadAllUnitsForLinesAsync) قبل تمكين التحرير، حتى لا يُحفظ سطر بمعامل 1 خطأً.
        _isUpdating = false;
    }

    private async Task LoadAllUnitsForLinesAsync()
    {
        foreach (var line in _lines.Where(l => l.ProductId > 0))
        {
            // حلّ الوحدة المحفوظة (by id ثم by name ثم الأساسية) وتثبيت معامل التحويل
            var (_, savedUnit) = await GetUnitsWithDefaultAsync(line.ProductId, line.ProductUnitId, line.UnitName);
            if (savedUnit == null) continue;

            if (string.IsNullOrEmpty(line.UnitName))
                line.UnitName = savedUnit.UnitName ?? "";
            if (line.ProductUnitId <= 0)
                line.ProductUnitId = savedUnit.Id;

            line.ConversionFactor = savedUnit.ConversionFactor > 0m ? savedUnit.ConversionFactor : 1m;
        }
        dgvItems.Refresh();
    }

    private void ResetForm()
    {
        // إعادة قراءة أرصدة المستودع عند إعادة تحميل البيانات
        _warehouseStockForWarehouseId = -1;
        _warehouseStockBase.Clear();

        lblMode.Text = "فاتورة بيع جديدة";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        SelectDefaultCashCustomer(); // "عميل نقدي" إن وُجد، وإلا "--- اختر عميل ---"
        //cmbPaymentType.SelectedIndex = 0;

        // الصندوق الافتراضي = أول صندوق نشط
        if (cmbCashBox.Items.Count > 0) cmbCashBox.SelectedIndex = 0;

        _lines.Clear();

        // الخصم: النوع الافتراضي "مبلغ" ثم تصفير القيمة
        cmbDiscountType.SelectedIndex = 0;
        numInvoiceDiscount.Value = 0;

        txtPaid.Text = "0";
        txtNotes.Clear();
        txtExpenses.Text = "0";
        lblLineTotal.Text = clsFormat.Money(0m);
        lblTafqitValue.Text = string.Empty;
        ClearEntryPanel();
    }

    /// <summary>
    /// يختار العميل الافتراضي "عميل نقدي" في قائمة العملاء، وإن لم يوجد يُبقي العنصر الأول
    /// ("--- اختر عميل ---"). يُستخدم عند فتح فاتورة جديدة فقط.
    /// </summary>
    private void SelectDefaultCashCustomer()
    {
        if (cmbCustomer.Items.Count == 0) return;

        for (int i = 0; i < cmbCustomer.Items.Count; i++)
        {
            if (cmbCustomer.Items[i] is CustomerDto customer
                && !string.IsNullOrWhiteSpace(customer.Name)
                && customer.Name.Contains("عميل نقدي", StringComparison.Ordinal))
            {
                cmbCustomer.SelectedIndex = i;
                return;
            }
        }

        cmbCustomer.SelectedIndex = 0;
    }

    // ═════════════════════════════════════════════════════════════════
    //  ENTRY PANEL — BARCODE LOOKUP
    // ═════════════════════════════════════════════════════════════════

    private async void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
    {
        try
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string barcode = txtBarcode.Text.Trim();

                if (string.IsNullOrWhiteSpace(barcode))
                {
                    await OpenProductSelectionPopupAsync();
                    return;
                }

                await LookupBarcodeAsync(barcode);
            }
            else if (e.KeyCode == Keys.F4)
            {
                e.SuppressKeyPress = true;
                await OpenProductSelectionPopupAsync();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                CancelEdit();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع في معالجة مفتاح الباركود");
            Cursor = Cursors.Default;
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    private async Task<ProductDto?> FindProductByBarcodeAsync(string barcode)
    {
        var result = await _productApi.GetByBarcodeAsync(barcode, default);
        if (result.IsSuccess && result.Value != null)
            return result.Value;

        return _allProducts.FirstOrDefault(p =>
            p.Barcode != null && p.Barcode.Equals(barcode, StringComparison.OrdinalIgnoreCase));
    }

    private async Task LookupBarcodeAsync(string barcode)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            var product = await FindProductByBarcodeAsync(barcode);

            if (product != null)
            {
                FillProductDetails(product);
            }
            else
            {
                // Barcode not found — auto-open search popup
                await OpenProductSelectionPopupAsync();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في البحث عن الباركود: {Barcode}", barcode);
            clsDialogs.Error("حدث خطأ أثناء البحث عن المنتج.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void FillProductDetails(ProductDto product, ProductUnitDto? preferredUnit = null)
    {
        try
        {
            if (!_editingLineKey.HasValue && IsDuplicateProduct(product.Id))
            {
                WarnDuplicateProduct();
                txtBarcode.Clear();
                txtBarcode.Focus();
                return;
            }

            // Load units (الوحدة المختارة في نافذة البحث أولاً، ثم الوحدة الأساسية)
            var (units, baseUnit) = await GetUnitsWithDefaultAsync(product.Id);
            var unitToSelect =
                (preferredUnit != null ? units.FirstOrDefault(u => u.Id == preferredUnit.Id) : null)
                ?? baseUnit;

            // Display product name
            lblProductName.Text = product.Name;
            txtItemCode.Text = product.Id.ToString();

            // Store product ID in tag (before DataSource → avoids stale availability lookups)
            cmbUnit.Tag = product.Id;

            // Populate unit combo
            cmbUnit.DisplayMember = "UnitName";
            cmbUnit.ValueMember = "Id";
            cmbUnit.DataSource = new List<ProductUnitDto>(units);
            if (unitToSelect != null)
                cmbUnit.SelectedValue = unitToSelect.Id; // يحرّك معالج تغيير الوحدة ⇒ تحديث المتاح/السعر

            // Pre-fill defaults for new rows
            if (!_editingLineKey.HasValue)
            {
                txtUnitPrice.Clear();
                txtQuantity.Text = "1";
            }

            // عرض المتاح في المخزون للوحدة المختارة
            _ = RefreshAvailableStockAsync();

            // Focus unit combo
            cmbUnit.Focus();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل بيانات المنتج");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    private async Task OpenProductSelectionPopupAsync()
    {
        var dialog = _serviceProvider.GetRequiredService<frmProductSelection>();
        if (cmbWarehouse.SelectedValue is short wid)
            dialog.WarehouseId = wid;
        if (dialog.ShowDialog(this) != DialogResult.OK || dialog.SelectedProduct == null)
        {
            // If we had a DGV return position, restore focus there even if cancelled
            ReturnFocusToDgvIfPending();
            return;
        }

        var product = dialog.SelectedProduct;
        var pickedUnit = dialog.SelectedUnit; // الوحدة المختارة في نافذة البحث (قد تكون null)

        // If invoked from DGV (F3 while on a DGV cell), apply product to that DGV row
        if (_dgvSearchReturnRow >= 0 && _dgvSearchReturnRow < dgvItems.Rows.Count)
        {
            await ApplyProductToDgvRowAsync(_dgvSearchReturnRow, product, pickedUnit);
            _dgvSearchReturnCol = -1;
            _dgvSearchReturnRow = -1;
            return;
        }

        // Otherwise, apply to Quick Input bar
        if (!_editingLineKey.HasValue && IsDuplicateProduct(product.Id))
        {
            WarnDuplicateProduct();
            return;
        }

        FillProductDetails(product, pickedUnit);
    }

    private void ReturnFocusToDgvIfPending()
    {
        if (_dgvSearchReturnRow >= 0 && _dgvSearchReturnRow < dgvItems.Rows.Count
            && _dgvSearchReturnCol >= 0 && _dgvSearchReturnCol < dgvItems.Columns.Count)
        {
            dgvItems.CurrentCell = dgvItems[_dgvSearchReturnCol, _dgvSearchReturnRow];
        }
        _dgvSearchReturnCol = -1;
        _dgvSearchReturnRow = -1;
    }

    // ═════════════════════════════════════════════════════════════════
    //  ENTRY PANEL — NAVIGATION
    // ═════════════════════════════════════════════════════════════════

    private void cmbUnit_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;

            if (cmbUnit.Tag == null || Convert.ToInt32(cmbUnit.Tag) <= 0)
            {
                clsDialogs.Warning("الرجاء اختيار منتج أولاً (مسح باركود أو F4).");
                txtBarcode.Focus();
                return;
            }

            if (cmbUnit.SelectedValue == null || Convert.ToInt32(cmbUnit.SelectedValue) <= 0)
            {
                clsDialogs.Warning("الرجاء اختيار وحدة للمنتج.");
                cmbUnit.Focus();
                return;
            }

            txtQuantity.Focus();
            BeginInvoke(() => txtQuantity.SelectAll());
        }
        else if (e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            CancelEdit();
        }
    }

    private void txtQuantity_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            if (!clsValidation.IsPositiveDecimal(txtQuantity.Text))   // GAP-08b — بيت التحقق الواحد (RULE-N169)
            {
                clsDialogs.Warning("الرجاء إدخال كمية صحيحة (أكبر من صفر).");
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return;
            }
            txtUnitPrice.Focus();
            BeginInvoke(() => txtUnitPrice.SelectAll());
        }
        else if (e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            CancelEdit();
        }
    }

    private void txtUnitPrice_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            if (!clsValidation.TryParseDecimal(txtUnitPrice.Text, out decimal price) || price < 0)
            {
                clsDialogs.Warning("الرجاء إدخال سعر صحيح.");
                txtUnitPrice.Focus();
                txtUnitPrice.SelectAll();
                return;
            }
            // Move to discount field
            txtLineDiscount.Focus();
            BeginInvoke(() => txtLineDiscount.SelectAll());
        }
        else if (e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            CancelEdit();
        }
    }

    private void txtLineDiscount_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            dtpLineExpiry.Focus();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            CancelEdit();
        }
    }

    /// <summary>
    /// Enter على تاريخ الانتهاء ⇒ إضافة/تعديل السطر، و Escape ⇒ إلغاء التحرير.
    /// </summary>
    private async void dtpLineExpiry_KeyDown(object? sender, KeyEventArgs e)
    {
        try
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await AddOrUpdateRowAsync();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                CancelEdit();
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع في معالجة مفتاح تاريخ الانتهاء");
            Cursor = Cursors.Default;
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  ENTRY PANEL — RECALC / ADD / UPDATE / CANCEL
    // ═════════════════════════════════════════════════════════════════

    private bool IsDuplicateProduct(int productId, Guid? excludeKey = null)
        => _lines.Any(l => l.ProductId == productId && (!excludeKey.HasValue || l.LineKey != excludeKey.Value));

    private void ResetEntryState()
    {
        _editingLineKey = null;
        _editingOriginalItem = null;
        tsmiCancelEdit.Enabled = false;
        HideCellExpiryPicker();
        ClearEntryPanel();
        dgvItems.Refresh(); // تحديث عرض تاريخ الانتهاء/الخصم بعد التعديل (الخصائص لا تُطلق إشعارًا)
        foreach (DataGridViewRow r in dgvItems.Rows)
            r.DefaultCellStyle.BackColor = Color.White;
        btnAddLine.Text = "➕ إضافة";
        btnAddLine.BackColor = ThemeHelper.Success;    // حالة الإضافة — نفس لون المصمّم (RULE-N134)
        btnCancelLine.Enabled = false;
        gbEntry.Text = "إضافة صنف";
    }

    private void RecalculateLineTotal(object? sender, EventArgs? e)
    {
        decimal qty = clsValidation.TryParseDecimal(txtQuantity.Text, out decimal q) ? q : 0m;
        decimal price = clsValidation.TryParseDecimal(txtUnitPrice.Text, out decimal p) ? p : 0m;
        decimal discount = clsValidation.TryParseDecimal(txtLineDiscount.Text, out decimal d) ? d : 0m;
        decimal total = (qty * price) - discount;
        if (total < 0m) total = 0m;
        lblLineTotal.Text = clsFormat.Money(total);
    }

    private async void btnAddLine_Click(object? sender, EventArgs? e)
    {
        try
        {
            await AddOrUpdateRowAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في إضافة/تعديل السطر");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    private void btnCancelLine_Click(object? sender, EventArgs? e)
    {
        CancelEdit();
    }

    /// <summary>
    /// "تعديل" — يحمّل السطر المحدد في شبكة الأصناف إلى لوحة الإدخال للتحرير.
    /// </summary>
    private void btnEditRow_Click(object? sender, EventArgs e)
    {
        try
        {
            if (IsLocked)
            {
                clsDialogs.Info("الفاتورة مقفلة (مرحّلة أو ملغاة) — لا يمكن تعديل الأصناف.");
                return;
            }

            int rowIndex = dgvItems.CurrentRow?.Index ?? -1;
            if (rowIndex < 0 || rowIndex >= _lines.Count)
            {
                clsDialogs.Info("اختر سطراً من الجدول أولاً.");
                return;
            }

            LoadRowForEditing(rowIndex);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تعديل السطر المحدد");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// "حذف" — يحذف السطر المحدد من قائمة أصناف الفاتورة بعد تأكيد.
    /// </summary>
    private void btnDeleteRow_Click(object? sender, EventArgs e)
    {
        try
        {
            if (IsLocked)
            {
                clsDialogs.Info("الفاتورة مقفلة (مرحّلة أو ملغاة) — لا يمكن تعديل الأصناف.");
                return;
            }

            if (dgvItems.CurrentRow is null || _lines.Count == 0)
            {
                clsDialogs.Info("اختر سطراً من الجدول أولاً.");
                return;
            }

            DeleteSelectedRow();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في حذف السطر المحدد");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    /// <summary>
    /// "تحديث بيانات الأصناف" — يعيد تحميل القوائم والأسعار وأرصدة المستودع (مسح الكاش).
    /// </summary>
    private async void btnRefreshCatalog_Click(object? sender, EventArgs e)
    {
        try
        {
            if (IsLocked) return;

            Cursor = Cursors.WaitCursor;

            _productUnitsCache.Clear();
            _warehouseStockForWarehouseId = -1;
            _warehouseStockBase.Clear();

            await LoadLookups();                 // إعادة تحميل العملاء/المستودعات/الصناديق/الأصناف/الحسابات

            // إعادة تطبيق اختيارات الفاتورة المفتوحة بعد إعادة تحميل القوائم (مسودة قيد التعديل)
            if (_invoice != null)
            {
                try
                {
                    if (cmbCustomer.DataSource != null && _invoice.CustomerId > 0)
                        cmbCustomer.SelectedValue = _invoice.CustomerId;
                    if (cmbWarehouse.DataSource != null && _invoice.WarehouseId > 0)
                        cmbWarehouse.SelectedValue = Convert.ToInt16(_invoice.WarehouseId);
                    if (cmbCashBox.DataSource != null && _invoice.CashBoxId is int cbId && cbId > 0)
                        cmbCashBox.SelectedValue = cbId;
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "تعذر إعادة تطبيق اختيارات الفاتورة بعد تحديث القوائم");
                }
            }

            await RefreshAvailableStockAsync();  // إعادة قراءة رصيد المستودع الحالي

            dgvItems.Refresh();
            clsDialogs.Success("تم تحديث بيانات الأصناف والأرصدة.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحديث بيانات الأصناف");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task AddOrUpdateRowAsync()
    {
        if (IsLocked)
        {
            clsDialogs.Info("الفاتورة مقفلة (مرحّلة أو ملغاة) — لا يمكن تعديل الأصناف.");
            return;
        }

        if (!ValidateEntryFields()) return;

        int productId = Convert.ToInt32(cmbUnit.Tag!);
        int productUnitId = Convert.ToInt32(cmbUnit.SelectedValue!);
        // القراءة من بيت التحقق نفسه الذي حكم في ValidateEntryFields (RULE-N169) —
        // فلا يمرّ حقل قرأه محرّك التحقق ويرفضه decimal.Parse بلغة أخرى.
        decimal quantity = clsValidation.TryParseDecimal(txtQuantity.Text, out decimal parsedQty) ? parsedQty : 0m;
        decimal unitPrice = clsValidation.TryParseDecimal(txtUnitPrice.Text, out decimal up) ? up : 0m;
        decimal lineDiscount = clsValidation.TryParseDecimal(txtLineDiscount.Text, out decimal ld) ? ld : 0m;
        decimal conversionFactor = GetSelectedConversionFactor();
        string barcode = txtBarcode.Text.Trim();
        string productName = lblProductName.Text;
        string unitName = cmbUnit.Text;
        DateTime? lineExpiry = dtpLineExpiry.Checked ? dtpLineExpiry.Value.Date : (DateTime?)null;

        // ── MERGE QUANTITY: if product already exists, add qty to existing row ──
        if (!_editingLineKey.HasValue)
        {
            var existingRow = _lines.FirstOrDefault(l => l.ProductId == productId && l.ProductUnitId == productUnitId);
            if (existingRow != null)
            {
                decimal newQty = existingRow.Quantity + quantity;
                existingRow.ConversionFactor = conversionFactor;
                if (!await IsStockSufficientAsync(productId, newQty, conversionFactor, existingRow.LineKey))
                {
                    txtQuantity.Focus();
                    txtQuantity.SelectAll();
                    return;
                }
                existingRow.Quantity = newQty;
                // Update discount only if non-zero entered
                if (lineDiscount > 0m) existingRow.DiscountAmount = lineDiscount;
                existingRow.ExpiryDate = lineExpiry;
                ResetEntryState();
                CalculateTotals();
                txtBarcode.Focus();
                return;
            }
        }

        // ── Early stock validation (وحدات أساسية) ──
        if (!await IsStockSufficientAsync(productId, quantity, conversionFactor, _editingLineKey))
        {
            txtQuantity.Focus();
            txtQuantity.SelectAll();
            return;
        }

        if (_editingLineKey.HasValue)
        {
            // ── Update existing row ──
            var row = _lines.FirstOrDefault(l => l.LineKey == _editingLineKey.Value);
            if (row != null)
            {
                row.ProductId = productId;
                row.ProductCode = barcode;
                row.Barcode = barcode;
                row.ProductName = productName;
                row.UnitName = unitName;
                row.ProductUnitId = productUnitId;
                row.ConversionFactor = conversionFactor;
                row.Quantity = quantity;
                row.UnitPrice = unitPrice;
                row.DiscountAmount = lineDiscount;
                row.ExpiryDate = lineExpiry;
            }
        }
        else
        {
            // ── Add new row ──
            _lines.Add(new InvoiceLineItem
            {
                ProductId = productId,
                ProductCode = barcode,
                Barcode = barcode,
                ProductName = productName,
                UnitName = unitName,
                ProductUnitId = productUnitId,
                ConversionFactor = conversionFactor,
                Quantity = quantity,
                UnitPrice = unitPrice,
                DiscountAmount = lineDiscount,
                ExpiryDate = lineExpiry
            });
        }

        ResetEntryState();
        CalculateTotals();
        txtBarcode.Focus();
    }

    private void CancelEdit()
    {
        if (_editingLineKey.HasValue && _editingOriginalItem != null)
        {
            var row = _lines.FirstOrDefault(l => l.LineKey == _editingLineKey.Value);
            if (row != null)
            {
                // Restore original item
                row.ProductId = _editingOriginalItem.ProductId;
                row.ProductCode = _editingOriginalItem.ProductCode;
                row.Barcode = _editingOriginalItem.Barcode;
                row.ProductName = _editingOriginalItem.ProductName;
                row.UnitName = _editingOriginalItem.UnitName;
                row.ProductUnitId = _editingOriginalItem.ProductUnitId;
                row.ConversionFactor = _editingOriginalItem.ConversionFactor;
                row.Quantity = _editingOriginalItem.Quantity;
                row.UnitPrice = _editingOriginalItem.UnitPrice;
                row.DiscountAmount = _editingOriginalItem.DiscountAmount;
                row.ExpiryDate = _editingOriginalItem.ExpiryDate;
            }
        }

        ResetEntryState();
        txtBarcode.Focus();
    }

    private void ClearEntryPanel()
    {
        txtBarcode.Clear();
        txtItemCode.Text = string.Empty;
        lblProductName.Text = "";
        cmbUnit.DataSource = null;
        cmbUnit.Tag = null;
        txtQuantity.Text = "1";
        txtUnitPrice.Clear();
        txtLineDiscount.Text = "0";
        lblLineTotal.Text = clsFormat.Money(0m);

        // تاريخ الانتهاء: غير محدد افتراضياً (بدون تاريخ انتهاء)
        dtpLineExpiry.Checked = false;
        dtpLineExpiry.Value = DateTime.Today;

        // إلغاء أي استعلام مخزون جارٍ + إعادة ضبط نص المتاح
        _stockLookupToken++;
        if (lblAvailable != null) lblAvailable.Text = "المتوفر في المخزن: —";

        // تفقيط حيّ — إخفاء التلميح عند بدء سطر جديد
        liveTip?.Hide(pnlEntry);
    }

    private bool ValidateEntryFields()
    {
        if (cmbUnit.Tag == null || Convert.ToInt32(cmbUnit.Tag) <= 0)
        {
            clsDialogs.Warning("الرجاء اختيار منتج أولاً (مسح باركود أو F4).");
            txtBarcode.Focus();
            return false;
        }

        if (cmbUnit.SelectedValue == null || Convert.ToInt32(cmbUnit.SelectedValue) <= 0)
        {
            clsDialogs.Warning("الرجاء اختيار الوحدة.");
            cmbUnit.Focus();
            return false;
        }

        if (!clsValidation.IsPositiveDecimal(txtQuantity.Text))   // GAP-08b — بيت التحقق الواحد (RULE-N169)
        {
            clsDialogs.Warning("الرجاء إدخال كمية صحيحة (أكبر من صفر).");
            txtQuantity.Focus();
            txtQuantity.SelectAll();
            return false;
        }

        if (!clsValidation.TryParseDecimal(txtUnitPrice.Text, out decimal price) || price < 0)
        {
            clsDialogs.Warning("الرجاء إدخال سعر صحيح.");
            txtUnitPrice.Focus();
            txtUnitPrice.SelectAll();
            return false;
        }

        return true;
    }

    // ═════════════════════════════════════════════════════════════════
    //  ENTRY PANEL — AUTO SELECT
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// Selects the current value when a field receives focus —
    /// typing overwrites immediately instead of appending.
    /// </summary>
    private void NumericTextBox_Enter(object? sender, EventArgs e)
    {
        switch (sender)
        {
            case TextBox txt:
                BeginInvoke(() => txt.SelectAll());
                break;
            case NumericUpDown num:
                BeginInvoke(() => num.Select(0, num.Text.Length));
                break;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  DGV — INLINE EDITING & EVENTS
    // ═════════════════════════════════════════════════════════════════

    private void dgvItems_DefaultValuesNeeded(object? sender, DataGridViewRowEventArgs e)
    {
        if (IsLocked) return; // الفاتورة مقفلة — لا إضافة سطور

        e.Row.Cells[colQuantity.Index].Value = 1m;
        e.Row.Cells[colUnitPrice.Index].Value = 0m;
        e.Row.Cells[colDiscount.Index].Value = 0m;
        e.Row.Cells[colLineTotal.Index].Value = 0m;
    }

    private async void dgvItems_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvItems.Rows.Count) return;

            // الفاتورة مقفلة (مرحّلة/ملغاة) — منع أي تحرير
            if (IsLocked)
            {
                e.Cancel = true;
                return;
            }

            // حفظ قيمة الخصم السابقة (لاسترجاعها عند إدخال غير صالح)
            if (e.ColumnIndex == colDiscount.Index && e.RowIndex < _lines.Count)
                _discountBeforeEdit = _lines[e.RowIndex].DiscountAmount;

            // If editing unit combo column — load units and set selected value
            if (e.ColumnIndex == colUnit.Index && e.RowIndex < _lines.Count)
            {
                var line = _lines[e.RowIndex];
                if (line.ProductId > 0)
                {
                    var units = await LoadProductUnitsAsync(line.ProductId);
                    var unitNames = units.Select(u => u.UnitName).ToList();
                    var comboCell = (DataGridViewComboBoxCell)dgvItems.Rows[e.RowIndex].Cells[colUnit.Index];
                    comboCell.DataSource = unitNames;

                    // Set value after DataSource is applied
                    if (!string.IsNullOrEmpty(line.UnitName) && unitNames.Contains(line.UnitName))
                        BeginInvoke(() => comboCell.Value = line.UnitName);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء بدء تحرير خلية في شبكة الأصناف");
            Cursor = Cursors.Default;
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    private void dgvItems_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
    {
        if (dgvItems.CurrentCell == null) return;

        if (dgvItems.CurrentCell.ColumnIndex == colBarcode.Index)
        {
            if (e.Control is TextBox txt)
            {
                txt.KeyDown -= BarcodeCell_KeyDown;
                txt.KeyDown += BarcodeCell_KeyDown;
            }
        }
        else if (dgvItems.CurrentCell.ColumnIndex == colUnit.Index)
        {
            if (e.Control is ComboBox cmb)
            {
                cmb.FlatStyle = FlatStyle.Flat;
                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb.BackColor = GreenThemeHelper.SelectionBack; // خلفية الخلية النشطة أثناء التحرير (RULE-N54)
            }
        }
        else if (dgvItems.CurrentCell.ColumnIndex == colQuantity.Index
              || dgvItems.CurrentCell.ColumnIndex == colUnitPrice.Index
              || dgvItems.CurrentCell.ColumnIndex == colDiscount.Index)
        {
            if (e.Control is TextBox txt)
            {
                txt.KeyPress -= NumericTextBox_KeyPress;
                txt.KeyPress += NumericTextBox_KeyPress;
            }
        }
        else if (dgvItems.CurrentCell.ColumnIndex == colExpiry.Index)
        {
            // محرر تاريخ الانتهاء: DateTimePicker (تنسيق yyyy/MM/dd) كعنصر تحرير عائم فوق الخلية
            ShowCellExpiryPicker(dgvItems.CurrentCell.RowIndex);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  محرر تاريخ الانتهاء داخل الشبكة (DateTimePicker فوق الخلية)
    // ═════════════════════════════════════════════════════════════════

    private DateTimePicker? _cellExpiryPicker;
    private bool _suspendCellExpiryCommit;

    private const string ExpiryDateFormat = "yyyy/MM/dd";

    private void EnsureCellExpiryPicker()
    {
        if (_cellExpiryPicker != null && !_cellExpiryPicker.IsDisposed) return;

        _cellExpiryPicker = new DateTimePicker
        {
            Name = "dtpCellExpiry",
            Format = DateTimePickerFormat.Custom,
            CustomFormat = ExpiryDateFormat,
            ShowCheckBox = true,
            Visible = false,
            Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0),
            RightToLeft = RightToLeft.Yes
        };
        _cellExpiryPicker.ValueChanged += CellExpiryPicker_ValueChanged;
        dgvItems.Controls.Add(_cellExpiryPicker);
    }

    /// <summary>يُظهر منتقي التاريخ فوق خلية تاريخ الانتهاء ويحمّل قيمتها الحالية.</summary>
    private void ShowCellExpiryPicker(int rowIndex)
    {
        if (rowIndex < 0 || IsLocked) return;

        EnsureCellExpiryPicker();
        var picker = _cellExpiryPicker!;

        DateTime? current = rowIndex < _lines.Count ? _lines[rowIndex].ExpiryDate : null;

        _suspendCellExpiryCommit = true;
        picker.Checked = current.HasValue;
        picker.Value = current ?? DateTime.Today;
        _suspendCellExpiryCommit = false;

        var cellRect = dgvItems.GetCellDisplayRectangle(colExpiry.Index, rowIndex, true);
        if (cellRect.Width <= 0 || cellRect.Height <= 0) return;

        picker.Bounds = new Rectangle(cellRect.X, cellRect.Y, Math.Max(cellRect.Width, 118), cellRect.Height);
        picker.Visible = true;
        picker.BringToFront();

        // بعد عرض محرر الشبكة الافتراضي — أعِد تقديم المنتقي وامنحه التركيز
        BeginInvoke(() =>
        {
            if (_cellExpiryPicker is { Visible: true, IsDisposed: false })
            {
                _cellExpiryPicker.BringToFront();
                _cellExpiryPicker.Focus();
            }
        });
    }

    private void HideCellExpiryPicker()
    {
        if (_cellExpiryPicker == null || _cellExpiryPicker.IsDisposed) return;
        _cellExpiryPicker.Visible = false;
    }

    private void CellExpiryPicker_ValueChanged(object? sender, EventArgs e)
    {
        if (_suspendCellExpiryCommit || _cellExpiryPicker == null) return;

        // غير محدد ⇒ بدون تاريخ انتهاء (null)
        CommitCellExpiry(_cellExpiryPicker.Checked ? _cellExpiryPicker.Value.Date : (DateTime?)null);
    }

    /// <summary>يكتب تاريخ الانتهاء في نموذج السطر وفي الخلية معاً.</summary>
    private void CommitCellExpiry(DateTime? expiry)
    {
        if (IsLocked) return;   // الفاتورة مقفلة — لا تعديل
        int rowIndex = dgvItems.CurrentCell?.RowIndex ?? -1;
        if (rowIndex < 0 || rowIndex >= _lines.Count) return;

        _lines[rowIndex].ExpiryDate = expiry;

        var cell = dgvItems.Rows[rowIndex].Cells[colExpiry.Index];
        var current = cell.Value is DateTime dt ? dt.Date : (DateTime?)null;
        if (current != expiry) cell.Value = expiry;
        dgvItems.InvalidateCell(cell);
    }

    /// <summary>تحليل نص تاريخ الانتهاء المكتوب يدوياً — الفراغ يعني "بدون تاريخ".</summary>
    private static DateTime? ParseExpiryText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text) || text.Trim() == "—") return null;

        string raw = text.Trim();
        string[] formats =
        {
            "yyyy/MM/dd", "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy"
        };

        if (DateTime.TryParseExact(raw, formats, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var exact))
            return exact.Date;

        return DateTime.TryParse(raw, out var parsed) ? parsed.Date : null;
    }

    private async void BarcodeCell_KeyDown(object? sender, KeyEventArgs e)
    {
        try
        {
            if (sender is not TextBox txt) return;

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string barcode = txt.Text.Trim();
                int rowIndex = dgvItems.CurrentCell?.RowIndex ?? -1;
                if (rowIndex < 0) return;

                if (string.IsNullOrWhiteSpace(barcode))
                {
                    await SelectProductForDgvRowAsync(rowIndex);
                }
                else
                {
                    await LookupBarcodeForDgvRowAsync(rowIndex, barcode);
                }
            }
            else if (e.KeyCode == Keys.F4)
            {
                e.SuppressKeyPress = true;
                int rowIndex = dgvItems.CurrentCell?.RowIndex ?? -1;
                if (rowIndex >= 0)
                {
                    await SelectProductForDgvRowAsync(rowIndex);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في إدخال الباركود داخل الشبكة");
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
    }

    private async Task LookupBarcodeForDgvRowAsync(int rowIndex, string barcode)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            var product = await FindProductByBarcodeAsync(barcode);

            if (product != null)
            {
                await ApplyProductToDgvRowAsync(rowIndex, product);
            }
            else
            {
                clsDialogs.Warning("لم يتم العثور على منتج بهذا الباركود.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في البحث عن الباركود في DGV: {Barcode}", barcode);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task SelectProductForDgvRowAsync(int rowIndex)
    {
        var dialog = _serviceProvider.GetRequiredService<frmProductSelection>();
        if (cmbWarehouse.SelectedValue is short wid)
            dialog.WarehouseId = wid;

        if (dialog.ShowDialog(this) == DialogResult.OK && dialog.SelectedProduct != null)
        {
            await ApplyProductToDgvRowAsync(rowIndex, dialog.SelectedProduct, dialog.SelectedUnit);
        }
    }

    private async Task ApplyProductToDgvRowAsync(int rowIndex, ProductDto product, ProductUnitDto? preferredUnit = null)
    {
        // Check duplicate: skip if editing the same row that already has this product
        if (rowIndex >= _lines.Count || _lines[rowIndex].ProductId != product.Id)
        {
            if (IsDuplicateProduct(product.Id, rowIndex < _lines.Count ? _lines[rowIndex].LineKey : null))
            {
                WarnDuplicateProduct();
                return;
            }
        }

        var (units, baseUnit) = await GetUnitsWithDefaultAsync(product.Id);

        // ── الوحدة المختارة في نافذة البحث (إن وُجدت) وإلا الوحدة الأساسية ──
        var preferred = preferredUnit != null ? units.FirstOrDefault(u => u.Id == preferredUnit.Id) : null;
        var chosenUnit = preferred ?? baseUnit;

        // ── Early stock validation (وحدات أساسية) — includes the row's CURRENT quantity ──
        decimal existingQty = rowIndex < _lines.Count ? _lines[rowIndex].Quantity : 0m;
        decimal requestedQty = existingQty > 0 ? existingQty : 1m;
        Guid? excludeKey = rowIndex < _lines.Count ? _lines[rowIndex].LineKey : null;
        decimal conversionFactor = chosenUnit != null && chosenUnit.ConversionFactor > 0m ? chosenUnit.ConversionFactor : 1m;

        if (!await IsStockSufficientAsync(product.Id, requestedQty, conversionFactor, excludeKey))
            return;

        InvoiceLineItem line;
        if (rowIndex < _lines.Count)
        {
            line = _lines[rowIndex];
        }
        else
        {
            line = new InvoiceLineItem();
            _lines.Add(line);
        }

        line.ProductId = product.Id;
        line.ProductCode = product.Barcode ?? "";
        line.Barcode = product.Barcode ?? "";
        line.ProductName = product.Name;
        line.UnitName = chosenUnit?.UnitName ?? product.UnitName ?? "";
        line.ProductUnitId = chosenUnit?.Id ?? 0;
        line.ConversionFactor = conversionFactor;
        if (line.UnitPrice <= 0m) line.UnitPrice = product.SalePrice > 0m ? product.SalePrice : 0m;
        if (line.Quantity <= 0) line.Quantity = 1m;

        // Set combo cell data source AND selected value
        if (rowIndex < dgvItems.Rows.Count)
        {
            var comboCell = (DataGridViewComboBoxCell)dgvItems.Rows[rowIndex].Cells[colUnit.Index];
            var unitNames = units.Select(u => u.UnitName).ToList();
            comboCell.DataSource = unitNames;
            if (!string.IsNullOrEmpty(line.UnitName) && unitNames.Contains(line.UnitName))
                comboCell.Value = line.UnitName;
        }

        CalculateTotals();
    }

    private async void dgvItems_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            if (IsLocked) return; // الفاتورة مقفلة — لا تحرير

            // ── خانة الخصم: تحقق/تقييد + إعادة حساب ──
            if (e.ColumnIndex == colDiscount.Index && e.RowIndex < _lines.Count)
            {
                ApplyDiscountCellEdit(e.RowIndex);
                return;
            }

            // ── خانة تاريخ الانتهاء: تثبيت القيمة في نموذج السطر (الفراغ ⇒ null) ──
            if (e.ColumnIndex == colExpiry.Index)
            {
                if (e.RowIndex >= 0 && e.RowIndex < _lines.Count)
                {
                    DateTime? expiry = _cellExpiryPicker is { Visible: true }
                        ? (_cellExpiryPicker.Checked ? _cellExpiryPicker.Value.Date : (DateTime?)null)
                        : ParseExpiryText(dgvItems.Rows[e.RowIndex].Cells[colExpiry.Index].Value?.ToString());

                    _lines[e.RowIndex].ExpiryDate = expiry;

                    var expiryCell = dgvItems.Rows[e.RowIndex].Cells[colExpiry.Index];
                    var expiryCurrent = expiryCell.Value is DateTime dt ? dt.Date : (DateTime?)null;
                    if (expiryCurrent != expiry) expiryCell.Value = expiry;
                    dgvItems.InvalidateCell(expiryCell);
                }

                HideCellExpiryPicker();
                return;
            }

            // Handle new row added by AllowUserToAddRows
            if (e.RowIndex >= _lines.Count)
            {
                var barcode = dgvItems.Rows[e.RowIndex].Cells[colBarcode.Index].Value?.ToString();
                if (string.IsNullOrWhiteSpace(barcode)) return;

                var product = _allProducts.FirstOrDefault(p =>
                    p.Barcode != null && p.Barcode.Equals(barcode, StringComparison.OrdinalIgnoreCase));
                if (product == null) return;

                if (IsDuplicateProduct(product.Id))
                {
                    WarnDuplicateProduct();
                    return;
                }

                // حلّ الوحدة ومعامل التحويل للسطر الجديد
                var lineUnits = await LoadProductUnitsAsync(product.Id);
                string lineUnitName = dgvItems.Rows[e.RowIndex].Cells[colUnit.Index].Value?.ToString() ?? "";
                var matchedUnit = lineUnits.FirstOrDefault(u => string.Equals(u.UnitName, lineUnitName, StringComparison.Ordinal))
                               ?? lineUnits.FirstOrDefault(u => u.IsBaseUnit)
                               ?? lineUnits.FirstOrDefault();

                var line = new InvoiceLineItem
                {
                    ProductId = product.Id,
                    ProductCode = product.Barcode ?? "",
                    Barcode = product.Barcode ?? "",
                    ProductName = product.Name,
                    Quantity = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value ?? 1m),
                    UnitPrice = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colUnitPrice.Index].Value ?? 0m),
                    UnitName = matchedUnit?.UnitName ?? lineUnitName,
                    ProductUnitId = matchedUnit?.Id ?? 0,
                    ConversionFactor = matchedUnit != null && matchedUnit.ConversionFactor > 0m ? matchedUnit.ConversionFactor : 1m,
                    DiscountAmount = TryParseCellDecimal(dgvItems.Rows[e.RowIndex].Cells[colDiscount.Index].Value),
                    ExpiryDate = ParseExpiryText(dgvItems.Rows[e.RowIndex].Cells[colExpiry.Index].Value?.ToString())
                };

                // ── Early stock validation for new inline row (وحدات أساسية) ──
                if (!await IsStockSufficientAsync(product.Id, line.Quantity, line.ConversionFactor))
                    return;

                _lines.Add(line);
                dgvItems.Rows[e.RowIndex].Cells[colLineTotal.Index].Value = line.LineTotal;
                CalculateTotals();
                return;
            }

            var existingLine = _lines[e.RowIndex];

            // 1) الوحدة (قد تكون تغيّرت) → مزامنة UnitName + ProductUnitId + معامل التحويل
            var unitVal = dgvItems.Rows[e.RowIndex].Cells[colUnit.Index].Value?.ToString();
            if (!string.IsNullOrEmpty(unitVal))
            {
                existingLine.UnitName = unitVal;
                if (_productUnitsCache.TryGetValue(existingLine.ProductId, out var cachedUnits))
                {
                    var matched = cachedUnits.FirstOrDefault(u => string.Equals(u.UnitName, unitVal, StringComparison.Ordinal));
                    if (matched != null)
                    {
                        existingLine.ProductUnitId = matched.Id;
                        existingLine.ConversionFactor = matched.ConversionFactor > 0m ? matched.ConversionFactor : 1m;
                    }
                }
            }

            // 2) الكمية + التحقق من المخزون بالوحدات الأساسية
            decimal previousQty = existingLine.Quantity;
            if (dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value != null)
                existingLine.Quantity = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value);

            // ── Early stock validation on direct quantity edit ──
            if (existingLine.Quantity != previousQty)
            {
                bool sufficient = await IsStockSufficientAsync(
                    existingLine.ProductId, existingLine.Quantity, existingLine.ConversionFactor, existingLine.LineKey);
                if (!sufficient)
                {
                    existingLine.Quantity = previousQty; // revert to previous quantity
                    dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value = previousQty;
                    CalculateTotals();
                    return;
                }
            }

            // 3) السعر
            if (dgvItems.Rows[e.RowIndex].Cells[colUnitPrice.Index].Value != null)
                existingLine.UnitPrice = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colUnitPrice.Index].Value);

            // 4) تقييد الخصم داخل (الكمية × السعر) بعد أي تغيير
            decimal maxDiscount = Math.Max(0m, existingLine.Quantity * existingLine.UnitPrice);
            if (existingLine.DiscountAmount > maxDiscount)
            {
                existingLine.DiscountAmount = maxDiscount;
                dgvItems.Rows[e.RowIndex].Cells[colDiscount.Index].Value = maxDiscount;
            }

            // Update displayed LineTotal
            dgvItems.Rows[e.RowIndex].Cells[colLineTotal.Index].Value = existingLine.LineTotal;

            CalculateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في إنهاء تحرير خلية الفاتورة");
        }
    }

    /// <summary>
    /// تحرير مباشر لخلية الخصم: قبول قيمة عشرية ≥ 0، تقييدها إلى (الكمية × السعر)،
    /// وإعادة الحساب وتنسيق الخلية N2 — مع استرجاع القيمة السابقة عند إدخال غير صالح.
    /// </summary>
    private void ApplyDiscountCellEdit(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _lines.Count) return;

        var line = _lines[rowIndex];
        var cell = dgvItems.Rows[rowIndex].Cells[colDiscount.Index];
        decimal maxDiscount = Math.Max(0m, line.Quantity * line.UnitPrice);

        string raw = (cell.Value?.ToString() ?? string.Empty).Trim();
        if (!clsValidation.TryParseDecimal(raw, out decimal value) || value < 0m)
        {
            line.DiscountAmount = Math.Max(0m, _discountBeforeEdit);
            cell.Value = line.DiscountAmount;
            cell.ToolTipText = "قيمة الخصم غير صحيحة — تم استرجاع القيمة السابقة";
            clsDialogs.Warning("قيمة الخصم غير صحيحة. يجب أن تكون رقماً أكبر من أو يساوي صفر.");
            CalculateTotals();
            return;
        }

        bool clamped = false;
        if (value > maxDiscount)
        {
            value = maxDiscount;
            clamped = true;
        }

        line.DiscountAmount = value;
        cell.Value = value; // التنسيق N2 يُطبَّق من نمط العمود
        cell.ToolTipText = clamped
            ? $"تم تقييد الخصم إلى {clsFormat.Money(maxDiscount)} (الكمية × السعر)"
            : string.Empty;

        if (clamped)
        {
            clsDialogs.Info($"لا يمكن أن يتجاوز الخصم قيمة السطر ({clsFormat.Money(maxDiscount)}).\nتم تقييد الخصم إلى {clsFormat.Money(maxDiscount)}.");
        }

        CalculateTotals();
    }

    /// <summary>قراءة رقم من قيمة خلية (تقبل الأرقام مع الفواصل) — عبر بيت القراءة الواحد.</summary>
    private static decimal TryParseCellDecimal(object? value)
        => value == null || value == DBNull.Value ? 0m
            : clsValidation.TryParseDecimal(value.ToString(), out decimal parsed) ? parsed : 0m;

    private void dgvItems_KeyDown(object? sender, KeyEventArgs e)
    {
        if (IsLocked) return;

        if (e.KeyCode == Keys.Delete && dgvItems.CurrentRow != null && !dgvItems.IsCurrentCellInEditMode && dgvItems.CurrentRow.Index >= 0 && dgvItems.CurrentRow.Index < _lines.Count)
        {
            e.SuppressKeyPress = true;
            DeleteSelectedRow();
        }
    }

    private void dgvItems_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex == colRowNum.Index && e.RowIndex >= 0)
        {
            e.Value = e.RowIndex + 1;
            e.FormattingApplied = true;
        }
        else if (e.ColumnIndex == colDelete.Index && e.RowIndex >= 0)
        {
            e.Value = GetTrashIcon();
            e.FormattingApplied = true;
        }
        else if (e.ColumnIndex == colUnit.Index && e.RowIndex >= 0 && e.RowIndex < _lines.Count)
        {
            // Display UnitName from model since we removed DataPropertyName
            e.Value = _lines[e.RowIndex].UnitName ?? "";
            e.FormattingApplied = true;
        }
        else if (e.ColumnIndex == colExpiry.Index && e.RowIndex >= 0)
        {
            // تاريخ الانتهاء: yyyy/MM/dd — و"‏—" عند عدم تحديد تاريخ
            var expiry = e.RowIndex < _lines.Count
                ? _lines[e.RowIndex].ExpiryDate
                : ParseExpiryText(e.Value?.ToString());

            e.Value = expiry.HasValue ? expiry.Value.ToString(ExpiryDateFormat) : "—";
            e.FormattingApplied = true;
        }
    }

    private static Image? _trashIcon;
    private static Image GetTrashIcon()
    {
        if (_trashIcon != null) return _trashIcon;

        var bmp = new Bitmap(20, 20);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using var pen = new Pen(ThemeHelper.Danger, 2f);   // نفس لون أزرار الحذف (RULE-N54)

        float cx = 10, top = 3, bot = 17;
        float lidW = 7, lidH = 2;

        g.DrawLine(pen, cx - lidW, top + lidH, cx + lidW, top + lidH);
        g.DrawRectangle(pen, cx - lidW + 1, top + lidH, (lidW * 2) - 2, bot - top - lidH);
        g.DrawLine(pen, cx - 3, top + lidH + 3, cx - 3, bot - 2);
        g.DrawLine(pen, cx, top + lidH + 3, cx, bot - 2);
        g.DrawLine(pen, cx + 3, top + lidH + 3, cx + 3, bot - 2);

        _trashIcon = bmp;
        return _trashIcon;
    }

    private void dgvItems_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (IsLocked) return; // الفاتورة مقفلة — لا حذف
        if (e.RowIndex < 0 || e.ColumnIndex != colDelete.Index) return;
        if (e.RowIndex >= _lines.Count) return;
        if (dgvItems.IsCurrentCellInEditMode) return;

        _lines.RemoveAt(e.RowIndex);
        CalculateTotals();
    }

    private void dgvItems_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (IsLocked) return; // الفاتورة مقفلة
        if (e.RowIndex < 0 || e.RowIndex >= _lines.Count) return;
        if (dgvItems.IsCurrentCellInEditMode) return;

        LoadRowForEditing(e.RowIndex);
    }

    private void dgvItems_CellEnter(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var col = dgvItems.Columns[e.ColumnIndex];
        if (col == null || col.ReadOnly) return;

        // Auto-select all text when entering TextBox cells
        // EditMode=EditOnEnter already handles entering edit mode
        if (dgvItems.EditingControl is TextBox txt)
        {
            BeginInvoke(() => txt.SelectAll());
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  DGV KEYBOARD NAVIGATION ENGINE
    //  Editable order: Barcode → Unit → Quantity → Price → Discount → Quick Input Barcode
    //  (يُبنى في المُنشئ من فهارس الأعمدة الفعلية — لا فهارس ثابتة)
    // ═════════════════════════════════════════════════════════════════

    private int[] EditableDgvColumns = Array.Empty<int>();
    private int _dgvSearchReturnCol = -1;
    private int _dgvSearchReturnRow = -1;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // ── Global shortcuts ──
        if (keyData == Keys.F1)
        {
            txtBarcode.Focus();
            BeginInvoke(() => txtBarcode.SelectAll());
            return true;
        }

        if (keyData == Keys.F3)
        {
            // If focused on DGV, store position to return after search
            if (dgvItems.Focused && dgvItems.CurrentCell != null)
            {
                _dgvSearchReturnCol = dgvItems.CurrentCell.ColumnIndex;
                _dgvSearchReturnRow = dgvItems.CurrentCell.RowIndex;
            }
            else
            {
                _dgvSearchReturnCol = -1;
                _dgvSearchReturnRow = -1;
            }
            _ = OpenProductSelectionPopupAsync();
            return true;
        }

        if (keyData == Keys.F5)
        {
            // F5 = تحديث بيانات الأصناف — يُنفَّذ عبر الاختصار الموحّد (RULE-N47)
            // (كان سابقاً ينقل التركيز إلى «المدفوع» — أُزيل لصالح معيار الاختصارات).
            return base.ProcessCmdKey(ref msg, keyData);
        }

        if (keyData == Keys.F12)
        {
            _ = SaveInvoiceAsync(false);
            return true;
        }

        // ── F2: Start editing current DGV cell ──
        if (keyData == Keys.F2 && dgvItems.Focused && dgvItems.CurrentCell != null)
        {
            var col = dgvItems.Columns[dgvItems.CurrentCell.ColumnIndex];
            if (col != null && !col.ReadOnly)
            {
                dgvItems.BeginEdit(false);
                if (dgvItems.EditingControl is TextBox txt)
                    BeginInvoke(() => txt.SelectAll());
            }
            return true;
        }

        // ── Esc: Cancel DGV inline edit ──
        if (keyData == Keys.Escape && (dgvItems.Focused || _cellExpiryPicker?.Visible == true) && dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            HideCellExpiryPicker();
            return true;
        }

        // ── Esc: Cancel entry panel edit mode ──
        if (keyData == Keys.Escape && _editingLineKey.HasValue)
        {
            CancelEdit();
            return true;
        }

        // ── Enter: DGV keyboard navigation engine ──
        if (keyData == Keys.Enter && (dgvItems.Focused || _cellExpiryPicker?.Visible == true) && dgvItems.CurrentCell != null)
        {
            int col = dgvItems.CurrentCell.ColumnIndex;
            int row = dgvItems.CurrentCell.RowIndex;
            if (row < 0) return base.ProcessCmdKey(ref msg, keyData);

            // Commit any active edit first
            if (dgvItems.IsCurrentCellInEditMode)
                dgvItems.EndEdit();

            // Find position of current column in the editable columns array
            int posInEditable = Array.IndexOf(EditableDgvColumns, col);

            if (posInEditable < 0)
            {
                // On a ReadOnly column — jump to first editable column of same row
                dgvItems.CurrentCell = dgvItems[EditableDgvColumns[0], row];
                return true;
            }

            // Move to next editable column in the same row
            if (posInEditable < EditableDgvColumns.Length - 1)
            {
                dgvItems.CurrentCell = dgvItems[EditableDgvColumns[posInEditable + 1], row];
            }
            else
            {
                // Was on last editable column (Price) → go back to Quick Input Barcode
                txtBarcode.Focus();
                BeginInvoke(() => txtBarcode.SelectAll());
            }
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// تمرير شبكة الأصناف بعجلة الماوس حتى لو لم تكن الشبكة هي العنصر النشط.
    /// (عندما تكون الشبكة نشطة تصل الرسالة إليها مباشرةً فلا تحدث مضاعفة.)
    /// </summary>
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        try
        {
            if (dgvItems is { IsDisposed: false, Visible: true } grid
                && grid.RowCount > 1
                && grid.ClientRectangle.Contains(grid.PointToClient(Cursor.Position)))
            {
                int lines = SystemInformation.MouseWheelScrollLines;
                if (lines <= 0) lines = 3;

                int notches = e.Delta / SystemInformation.MouseWheelScrollDelta;
                if (notches == 0) notches = e.Delta > 0 ? 1 : -1;

                int current = grid.FirstDisplayedScrollingRowIndex;
                if (current < 0) current = 0;

                int target = Math.Clamp(current - (notches * lines), 0, grid.RowCount - 1);
                if (target != grid.FirstDisplayedScrollingRowIndex)
                    grid.FirstDisplayedScrollingRowIndex = target;

                return; // تم التعامل مع العجلة داخل الشبكة
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر تمرير شبكة الأصناف بعجلة الماوس");
        }

        base.OnMouseWheel(e);
    }

    private async void LoadRowForEditing(int rowIndex)
    {
        try
        {
            if (IsLocked) return; // الفاتورة مقفلة — لا تعديل
            if (rowIndex < 0 || rowIndex >= _lines.Count) return;

            Cursor = Cursors.WaitCursor;

            var row = _lines[rowIndex];
            int productId = row.ProductId;
            string productName = row.ProductName ?? "";
            string barcode = row.Barcode ?? row.ProductCode ?? "";

            _editingOriginalItem = row.Clone();
            _editingLineKey = row.LineKey;

            // Load units (prefer saved unit id, then saved unit name)
            var (units, selectedUnit) = await GetUnitsWithDefaultAsync(productId, row.ProductUnitId, row.UnitName);

            // Fill entry panel
            txtBarcode.Text = barcode;
            lblProductName.Text = productName;
            txtItemCode.Text = productId.ToString();
            cmbUnit.Tag = productId; // قبل DataSource — يمنع استعلام مخزون قديم

            cmbUnit.DisplayMember = "UnitName";
            cmbUnit.ValueMember = "Id";
            cmbUnit.DataSource = new List<ProductUnitDto>(units);
            if (selectedUnit != null)
                cmbUnit.SelectedValue = selectedUnit.Id;
            else if (units.Count > 0)
                cmbUnit.SelectedIndex = 0;

            txtQuantity.Text = row.Quantity.ToString("G");
            txtUnitPrice.Text = row.UnitPrice.ToString("G");
            txtLineDiscount.Text = row.DiscountAmount.ToString("G");

            // تاريخ انتهاء السطر — يُعرض في لوحة الإدخال كما هو محفوظ
            dtpLineExpiry.Checked = row.ExpiryDate.HasValue;
            dtpLineExpiry.Value = row.ExpiryDate ?? DateTime.Today;

            RecalculateLineTotal(null, null);

            // تحديث المتاح في المخزون للوحدة المحفوظة
            _ = RefreshAvailableStockAsync();

            // Set editing mode
            btnAddLine.Text = "✏️ تعديل";
            btnAddLine.BackColor = ThemeHelper.Warning;    // وضع التعديل — نفس لون المصمّم (RULE-N134)
            btnCancelLine.Enabled = true;
            tsmiCancelEdit.Enabled = true;
            gbEntry.Text = $"تعديل السطر #{rowIndex + 1} — {productName}";

            foreach (DataGridViewRow r in dgvItems.Rows)
            {
                r.DefaultCellStyle.BackColor = Color.White;
            }
            dgvItems.Rows[rowIndex].DefaultCellStyle.BackColor = GreenThemeHelper.SelectionBack;

            txtBarcode.Focus();
            txtBarcode.SelectAll();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ غير متوقع أثناء تحميل السطر رقم {RowIndex} للتحرير", rowIndex);
            clsDialogs.Error("حدث خطأ غير متوقع. يرجى إعادة المحاولة.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void DeleteSelectedRow()
    {
        if (IsLocked) return; // الفاتورة مقفلة — لا حذف
        if (dgvItems.CurrentRow?.DataBoundItem is not InvoiceLineItem line)
            return;

        if (!clsDialogs.Confirm("هل أنت متأكد من حذف هذا الصنف من الفاتورة؟")) return;

        bool wasEditing = _editingLineKey == line.LineKey;
        _lines.Remove(line);

        if (wasEditing)
        {
            ResetEntryState();
        }

        CalculateTotals();
    }

    // ═════════════════════════════════════════════════════════════════
    //  BUTTON HANDLERS (Designer wires these to Click)
    // ═════════════════════════════════════════════════════════════════

    private async void btnSaveDraft_Click(object? sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object? sender, EventArgs e) => await SaveInvoiceAsync(true);

    private async void btnCancelInvoice_Click(object? sender, EventArgs e)
    {
        if (_invoice == null) return;
        if (!clsDialogs.Confirm("هل أنت متأكد من إلغاء هذه الفاتورة؟"))
            return;

        try
        {
            Cursor = Cursors.WaitCursor;
            await _salesApi.CancelAsync(_invoice.Id, default);
            clsDialogs.Success("تم إلغاء الفاتورة");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء إلغاء الفاتورة");
            clsDialogs.Error("حدث خطأ أثناء إلغاء الفاتورة. تم تسجيل التفاصيل.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnPrintA4_Click(object? sender, EventArgs e)
        => await PrintInvoiceAsync(_invoicePrinter, "A4", summary: chkPrintSummary.Checked);

    private async void btnPrintReceipt_Click(object? sender, EventArgs e)
        => await PrintInvoiceAsync(_receiptPrinter, "القصيرة");

    /// <summary>
    /// طباعة الفاتورة. عندما يكون الطابعة A4 (IInvoicePrinter) وخيار "طباعة فاتورة إجمالية" مفعّلاً
    /// تُطبع نسخة إجمالية مختصرة، وإلا تُطبع المعاينة العادية. الإيصال القصير لا يدعم النسخة الإجمالية.
    /// </summary>
    private async Task PrintInvoiceAsync(IPrinterService printer, string label, bool summary = false)
    {
        if (_invoice == null) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            var settingsRes = await _settingsApi.GetSettingsAsync();
            if (!settingsRes.IsSuccess)
            {
                clsDialogs.Error("فشل تحميل إعدادات الطباعة: " + settingsRes.Error);
                return;
            }

            // إعدادات المتجر تأتي dynamic من واجهة Desktop (StoreSettingsDto في الوضع الحقيقي،
            // ExpandoObject في وضع التجربة) — تُوحَّد إلى StoreSettingsDto قبل الطباعة.
            object? settingsValue = settingsRes.Value;
            var storeSettings = settingsValue as StoreSettingsDto ?? ToStoreSettings(settingsValue);

            var storeInfo = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(storeSettings);
            var printInvoice = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(_invoice);
            var printItems = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDtos(_invoice.Items);
            var printTotals = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToTotalsPrintDto(_invoice);

            if (summary && printer is IInvoicePrinter invoicePrinter)
            {
                // نسخة إجمالية مختصرة (بدون تفاصيل السطور)
                invoicePrinter.PrintSummaryPreview(printInvoice, printItems, printTotals, storeInfo);
                return;
            }

            printer.PrintPreview(printInvoice, printItems, printTotals, storeInfo);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في طباعة الفاتورة {Label}", label);
            clsDialogs.Error("حدث خطأ أثناء الطباعة. تم تسجيل التفاصيل في السجل.");
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// يوحّد إعدادات المتجر القادمة من واجهة Desktop إلى <see cref="StoreSettingsDto"/>
    /// (الوضع الحقيقي يعيد StoreSettingsDto، ووضع التجربة يعيد كائنًا ديناميكيًا).
    /// </summary>
    private static StoreSettingsDto ToStoreSettings(object? value)
    {
        if (value is StoreSettingsDto dto) return dto;

        if (value is IDictionary<string, object?> map)
        {
            object? Raw(string key) => map.TryGetValue(key, out var v) ? v : null;
            string? Text(string key) => Raw(key) as string;
            bool Flag(string key) => Raw(key) is bool b && b;
            decimal Number(string key) => Raw(key) is decimal d ? d : 0m;
            int Count(string key, int fallback) => Raw(key) is int i ? i : fallback;

            return new StoreSettingsDto(
                Id: 0,
                StoreName: Text("storeName") ?? string.Empty,
                Phone: Text("phone"),
                Address: Text("address"),
                LogoPath: Text("logoPath"),
                Email: Text("email"),
                DefaultTaxRate: Number("defaultTaxRate"),
                IsTaxEnabled: Flag("isTaxEnabled"),
                TaxNumber: Text("taxNumber"),
                EnableStockAlerts: Flag("enableStockAlerts"),
                AllowNegativeStock: Flag("allowNegativeStock"),
                InvoicePrefix: Text("invoicePrefix") ?? string.Empty,
                BackupPath: Text("backupPath"),
                BackupScheduleTime: Text("backupScheduleTime"),
                BackupRetentionDays: Count("backupRetentionDays", 30),
                UpdateServerUrl: Text("updateServerUrl"),
                SignaturePath: Text("signaturePath"));
        }

        return new StoreSettingsDto(0, string.Empty, null, null, null, null, 0m, false, null,
            false, false, string.Empty);
    }

    private void btnClose_Click(object? sender, EventArgs e) => this.Close();

    // ═════════════════════════════════════════════════════════════════
    //  NEW INVOICE / SALES RETURN
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// "فاتورة جديدة" — يتأكد من عدم فقدان سطور غير محفوظة ثم يبدأ فاتورة فارغة.
    /// </summary>
    private void btnNew_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!ConfirmDiscardUnsavedLines()) return;

            _invoice = null;
            ResetForm();
            UpdateUIState();
            CalculateTotals();

            txtBarcode.Focus();
            txtBarcode.SelectAll();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء بدء فاتورة بيع جديدة");
            clsDialogs.Error("حدث خطأ أثناء بدء فاتورة جديدة. يرجى المحاولة مرة أخرى.");
        }
    }

    /// <summary>
    /// تأكيد بدء فاتورة جديدة عند وجود سطور غير محفوظة.
    /// </summary>
    private bool ConfirmDiscardUnsavedLines()
    {
        if (!_lines.Any(l => l.ProductId > 0)) return true;

        return clsDialogs.Confirm("هل تريد بدء فاتورة جديدة؟ سيتم فقدان البيانات غير المحفوظة.");
    }

    // ═════════════════════════════════════════════════════════════════
    //  المتاح في المخزون (لكل وحدة) — TASK 2
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// يحدّث lblAvailable من رصيد المستودع (وحدات أساسية) بعد قسمته على معامل تحويل الوحدة المختارة.
    /// مثال: "المتوفر في المخزن: 12 كرتون (144 حبة)". يتجاهل النتائج القديمة (race guard).
    /// </summary>
    private async Task RefreshAvailableStockAsync()
    {
        if (lblAvailable == null || IsDisposed) return;

        int productId = cmbUnit.Tag != null ? Convert.ToInt32(cmbUnit.Tag) : 0;
        if (productId <= 0 || cmbWarehouse.SelectedValue is not short wid || wid <= 0)
        {
            lblAvailable.Text = "المتوفر في المخزن: —";
            return;
        }

        decimal factor = GetSelectedConversionFactor();
        string unitName = (cmbUnit.SelectedItem as ProductUnitDto)?.UnitName ?? string.Empty;
        if (string.IsNullOrWhiteSpace(unitName)) unitName = cmbUnit.Text;

        string baseUnitName = "الوحدة الأساسية";
        if (_productUnitsCache.TryGetValue(productId, out var units))
            baseUnitName = units.FirstOrDefault(u => u.IsBaseUnit)?.UnitName ?? baseUnitName;

        int token = ++_stockLookupToken;

        // رصيد المستودع بالوحدات الأساسية — يُحمَّل مرة واحدة لكل مستودع
        if (!await EnsureWarehouseStockAsync(wid))
        {
            if (token == _stockLookupToken) lblAvailable.Text = "المتوفر في المخزن: —";
            return;
        }
        if (!_warehouseStockBase.TryGetValue(productId, out decimal baseQty))
        {
            if (token == _stockLookupToken) lblAvailable.Text = "المتوفر في المخزن: —";
            return;
        }

        // ── تجاهل نتيجة قديمة (تغيّر المنتج/المستودع أثناء الاستعلام) ──
        if (token != _stockLookupToken) return;
        if (cmbUnit.Tag == null || Convert.ToInt32(cmbUnit.Tag) != productId) return;

        decimal inUnit = factor > 0m ? baseQty / factor : baseQty;
        string unitLabel = string.IsNullOrWhiteSpace(unitName) ? baseUnitName : unitName.Trim();
        lblAvailable.Text = $"المتوفر في المخزن: {clsFormat.Quantity(inUnit)} {unitLabel} ({clsFormat.Quantity(baseQty)} {baseUnitName})";
    }

    private async void cmbUnit_SelectedIndexChanged(object? sender, EventArgs e)
    {
        try
        {
            if (_isUpdating || IsDisposed) return;
            await RefreshAvailableStockAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث المتاح في المخزون بعد تغيير الوحدة");
        }
    }

    private async void cmbWarehouse_SelectedIndexChanged_RefreshAvailable(object? sender, EventArgs e)
    {
        try
        {
            if (_isUpdating || IsDisposed) return;
            await RefreshAvailableStockAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء تحديث المتاح في المخزون بعد تغيير المستودع");
        }
    }

    /// <summary>
    /// "مردود بيع" — يفتح شاشة مردود المبيعات من الحاوية (DI).
    /// </summary>
    private void btnSalesReturn_Click(object? sender, EventArgs e)
    {
        try
        {
            var form = _serviceProvider.GetRequiredService<frmSalesReturn>();
            form.ShowDialog(this);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء فتح شاشة مردود المبيعات");
            clsDialogs.Error("حدث خطأ أثناء فتح شاشة مردود المبيعات.");
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  SAVE LOGIC (PRESERVED)
    // ═════════════════════════════════════════════════════════════════

    private bool _ValidateForm()
    {
        errorProvider.Clear();
        var errors = new List<string>();

        // Customer must be selected (not the placeholder Id=0)
        if (cmbCustomer.SelectedValue == null || Convert.ToInt32(cmbCustomer.SelectedValue) <= 0)
        {
            errorProvider.SetError(cmbCustomer, "يجب اختيار العميل");
            clsInputFeedback.ShowRejectTooltip(cmbCustomer, "يجب اختيار العميل", tooltip);   // GAP-08b
            cmbCustomer.Focus();
            clsDialogs.Warning("يجب اختيار العميل");
            return false;
        }

        // Warehouse must be selected
        if (cmbWarehouse.SelectedValue == null || Convert.ToInt32(cmbWarehouse.SelectedValue) <= 0)
        {
            errorProvider.SetError(cmbWarehouse, "يجب اختيار المستودع");
            clsInputFeedback.ShowRejectTooltip(cmbWarehouse, "يجب اختيار المستودع", tooltip);   // GAP-08b
            cmbWarehouse.Focus();
            clsDialogs.Warning("يجب اختيار المستودع");
            return false;
        }

        // Must have at least one product
        if (!_lines.Any(l => l.ProductId > 0))
        {
            errorProvider.SetError(dgvItems, "لم يتم إضافة أي منتج إلى الفاتورة");
            clsInputFeedback.ShowRejectTooltip(dgvItems, "لم يتم إضافة أي منتج إلى الفاتورة", tooltip);   // GAP-08b
            dgvItems.Focus();
            clsDialogs.Warning("أضف منتج واحد على الأقل");
            return false;
        }

        // Validate each line
        foreach (var line in _lines.Where(l => l.ProductId > 0))
        {
            if (line.Quantity <= 0)
                errors.Add($"الكمية يجب أن تكون أكبر من صفر ({line.ProductName})");
            if (line.UnitPrice < 0)
                errors.Add($"السعر لا يمكن أن يكون سالباً ({line.ProductName})");
            if (string.IsNullOrEmpty(line.UnitName))
                errors.Add($"لم يتم اختيار وحدة ({line.ProductName})");
        }

        // Discount validation — يُحوَّل الخصم إلى مبلغ قبل المقارنة (يحترم مبلغ/نسبة)
        decimal subTotal = _lines.Where(l => l.ProductId > 0).Sum(l => l.LineTotal);
        decimal discountVal = ResolveInvoiceDiscount(subTotal).Amount;
        if (discountVal > subTotal && subTotal > 0)
            errors.Add("مبلغ الخصم لا يمكن أن يتجاوز إجمالي الفاتورة");

        // ── Payment validation ──
        bool isCash = rbtnPaymentTypeCash.Checked;     // نقدي
        bool isCredit = rbtnPaymentTypeCredit.Checked;   // آجل

        decimal paidCash = txtPaid.DecimalValue;
        decimal totalPaid = paidCash;

        // Real invoice total (tax + expenses) — shared threshold with server guard
        decimal totalAmount = ComputeTotalAmount();

        // Negative amounts
        if (paidCash < 0) { txtPaid.Focus(); errors.Add("المبلغ النقدي لا يمكن أن يكون سالباً"); }
        //if (paidBank < 0) { txtBankAmount.Focus(); errors.Add("المبلغ البنكي لا يمكن أن يكون سالباً"); }

        // ── Cash (نقدي): must be fully paid — no partial cash sales ──
        if (isCash)
        {
            if (totalPaid < totalAmount)
            {
                errorProvider.SetError(txtPaid, "المبلغ المدفوع أقل من الإجمالي");
                clsInputFeedback.ShowRejectTooltip(txtPaid, "المبلغ المدفوع أقل من الإجمالي", tooltip);   // GAP-08b
                txtPaid.Focus();
                clsDialogs.Warning($"البيع النقدي يتطلب دفع كامل المبلغ.\nالإجمالي: {clsFormat.Money(totalAmount)} — المدفوع: {clsFormat.Money(totalPaid)}\nالمتبقي: {clsFormat.Money(totalAmount - totalPaid)}");
                return false;
            }
            // Cash 
        }

        // ── Credit (آجل): العميل إلزامي لأن الباقي يُرحَّل إلى حسابه — الدفع الجزئي أو الصفر مسموح ──
        if (isCredit)
        {
            int creditCustomerId = cmbCustomer.SelectedValue == null ? 0 : Convert.ToInt32(cmbCustomer.SelectedValue);

            if (creditCustomerId <= 0 || cmbCustomer.SelectedIndex <= 0)
            {
                errorProvider.SetError(cmbCustomer, "البيع الآجل يتطلب اختيار عميل");
                clsInputFeedback.ShowRejectTooltip(cmbCustomer, "البيع الآجل يتطلب اختيار عميل", tooltip);   // GAP-08b
                cmbCustomer.Focus();
                errors.Add("البيع الآجل يتطلب اختيار عميل.");
            }
            else if (totalPaid > totalAmount)
            {
                errorProvider.SetError(txtPaid, "المبلغ المدفوع يتجاوز إجمالي الفاتورة");
                clsInputFeedback.ShowRejectTooltip(txtPaid, "المبلغ المدفوع يتجاوز إجمالي الفاتورة", tooltip);   // GAP-08b
                txtPaid.Focus();
                errors.Add("المبلغ المدفوع لا يمكن أن يتجاوز إجمالي الفاتورة في البيع الآجل.");
            }
            // المدفوع قد يكون صفراً (آجل كامل) أو جزئياً — لا يُمنع الحفظ عندما totalPaid < totalAmount
        }

        // Paid amount > total → warn (overpayment, allowed only on confirmation for cash/mixed edge cases)
        if (totalPaid > 0 && totalPaid > totalAmount && errors.Count == 0)
        {
            if (!clsDialogs.Confirm($"المبلغ المدفوع ({clsFormat.Money(totalPaid)}) أكبر من الإجمالي ({clsFormat.Money(totalAmount)}). هل تريد المتابعة؟"))
            {
                txtPaid.Focus();
                return false;
            }
        }

        if (errors.Count > 0)
        {
            clsDialogs.Warning(string.Join("\n", errors));
            return false;
        }

        return true;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        errorProvider.Clear();

        // الفاتورة مقفلة (مرحّلة/ملغاة) — لا حفظ ولا ترحيل
        if (IsLocked)
        {
            clsDialogs.Info("الفاتورة مقفلة (مرحّلة أو ملغاة) — لا يمكن تعديلها أو ترحيلها مرة أخرى.");
            return;
        }

        if (!_ValidateForm()) return;

        if (post && !clsDialogs.Confirm("هل أنت متأكد من ترحيل الفاتورة؟ لا يمكن تعديلها بعد الترحيل."))
            return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        try
        {
            Cursor = Cursors.WaitCursor;

            var items = _lines.Where(l => l.ProductId > 0).Select(l =>
            {
                // Use stored ProductUnitId if known; otherwise resolve from cache by name
                int productUnitId = 1;
                if (l.ProductUnitId > 0)
                {
                    productUnitId = l.ProductUnitId;
                }
                else if (_productUnitsCache.TryGetValue(l.ProductId, out var units))
                {
                    var match = units.FirstOrDefault(u => u.UnitName == l.UnitName) ?? units.FirstOrDefault();
                    if (match != null) productUnitId = match.Id;
                }

                // خصم السطر: النوع مبلغ ⇒ DiscountRate يحمل قيمة الخصم (كما في SalesInvoiceLine)
                return new CreateSalesInvoiceLineRequest(
                    l.ProductId,
                    l.Quantity,
                    l.UnitPrice,
                    productUnitId,
                    SalesSystem.DTO.Enums.DiscountType.Amount,
                    l.DiscountAmount > 0m ? l.DiscountAmount : (decimal?)null,
                    l.ExpiryDate);
            }).ToList();

            if (items.Count == 0)
            {
                clsDialogs.Warning("لا توجد منتجات صالحة للحفظ.");
                return;
            }

            decimal subTotal = _lines.Where(l => l.ProductId > 0).Sum(x => x.LineTotal);

            // الخصم: مبلغ ثابت أو نسبة من الإجمالي — يُرسَل كنوع + قيمة + مبلغ محسوب
            var discount = ResolveInvoiceDiscount(subTotal);
            var discountType = discount.Type;
            decimal discountValue = discount.Rate;
            decimal calculatedDiscountAmt = discount.Amount;

            decimal taxableAmount = subTotal - calculatedDiscountAmt;
            decimal reqTaxAmount = CalculateTaxAmount(taxableAmount);

            // ── Shared header values (used by both create & update) ──
            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0);
            int customerId = Convert.ToInt32(cmbCustomer.SelectedValue ?? 0);
            int? cashBoxId = cmbCashBox.SelectedIndex >= 0
                && cmbCashBox.SelectedValue is int selectedCashBoxId
                && selectedCashBoxId > 0
                    ? (int?)selectedCashBoxId
                    : null;
            var paymentType = (SalesSystem.DTO.Enums.PaymentType)(rbtnPaymentTypeCash.Checked == true ? SalesSystem.DTO.Enums.PaymentType.Cash : SalesSystem.DTO.Enums.PaymentType.Credit);
            decimal paidAmount = txtPaid.DecimalValue;
            decimal expenses = clsValidation.TryParseDecimal(txtExpenses.Text, out decimal exp2) ? exp2 : 0m;
            string notes = txtNotes.Text;
            int? taxId = null; // أُزيلت وحدة الضرائب (V2)

            // ── Create or Update? ──
            int invoiceId;
            bool isUpdate = _invoice != null && _invoice.Id > 0;

            // طلب واحد للإنشاء والتعديل — واجهة Desktop تستخدم CreateSalesInvoiceRequest في الحالتين
            var createRequest = new CreateSalesInvoiceRequest(
                WarehouseId: warehouseId,
                InvoiceNo: null,
                CustomerId: customerId,
                CashBoxId: cashBoxId,
                InvoiceDate: dtpDate.Value,
                PaymentType: paymentType,
                DiscountAmount: calculatedDiscountAmt,
                TaxAmount: reqTaxAmount,
                OtherCharges: expenses,
                PaidAmount: paidAmount,
                Notes: notes,
                TaxId: taxId,
                Items: items,
                DiscountType: discountType,
                DiscountRate: discountValue);

            if (isUpdate)
            {
                var updateResult = await _salesApi.UpdateAsync(_invoice!.Id, createRequest, default);
                if (!updateResult.IsSuccess)
                {
                    clsDialogs.Error(updateResult.Error ?? "فشل تعديل الفاتورة");
                    return;
                }
                invoiceId = _invoice.Id;
                _invoice = updateResult.Value;
            }
            else
            {
                var createResult = await _salesApi.CreateAsync(createRequest, default);
                if (!createResult.IsSuccess)
                {
                    clsDialogs.Error(createResult.Error ?? "فشل إنشاء الفاتورة");
                    return;
                }
                invoiceId = createResult.Value!.Id;
                _invoice = createResult.Value;
            }

            // ── Post if requested ──
            if (post)
            {
                // AutoPostInvoices may have already posted during CreateAsync —
                // only post when the invoice is still a Draft (avoids double-post error)
                bool alreadyPosted = _invoice != null && _invoice.Status != (byte)InvoiceStatus.Draft;

                if (alreadyPosted)
                {
                    clsDialogs.Success(isUpdate ? "تم تعديل وترحيل الفاتورة بنجاح" : "تم إنشاء وترحيل الفاتورة بنجاح");
                }
                else
                {
                    var postResult = await _salesApi.PostAsync(invoiceId, default);
                    if (!postResult.IsSuccess)
                    {
                        clsDialogs.Warning($"تم الحفظ كمسودة ولكن فشل الترحيل: {postResult.Error}");
                        // Refresh state from server — the invoice may still be a draft
                        var fresh = await _salesApi.GetByIdAsync(invoiceId, default);
                        if (fresh.IsSuccess) _invoice = fresh.Value;
                    }
                    else
                    {
                        // PostAsync يعيد Result (بلا قيمة) — نعيد تحميل الفاتورة لتحديث الحالة
                        // (المرحّل ⇒ IsLocked = true فيُقفل التعديل تلقائياً)
                        var posted = await _salesApi.GetByIdAsync(invoiceId, default);
                        if (posted.IsSuccess && posted.Value != null)
                        {
                            _invoice = posted.Value;
                        }
                        else
                        {
                            // الترحيل نجح على الخادم لكن تعذّر تحديث الحالة محلياً — لا نقفل الشاشة بشكل خاطئ
                            clsDialogs.Warning("تم ترحيل الفاتورة، لكن تعذر تحديث حالتها. أعد فتح الفاتورة من قائمة السجلات.");
                        }

                        clsDialogs.Success(isUpdate ? "تم تعديل وترحيل الفاتورة بنجاح" : "تم إنشاء وترحيل الفاتورة بنجاح");
                    }
                }
            }
            else
            {
                clsDialogs.Success(isUpdate ? "تم تعديل الفاتورة بنجاح" : "تم الحفظ كمسودة بنجاح");
            }

            // Update UI — stay open for further editing
            try { EventBus.Publish(new SaleInvoiceChangedMessage(invoiceId)); }
            catch (Exception ex) { Log.Warning(ex, "تعذر نشر حدث تغيير فاتورة البيع {InvoiceId}", invoiceId); }

            lblInvoiceNo.Text = _invoice.InvoiceNo.ToString();
            UpdateUIState();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الفاتورة");
            clsDialogs.Error("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل في السجل.");
        }
        finally
        {
            btnSaveDraft.Enabled = btnPost.Enabled = true;
            _SyncAcceptButton();   // RULE-N147 — يُعاد ربط Enter بالزر الأساسي بعد انتهاء الحفظ
            Cursor = Cursors.Default;
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  TOTALS (PRESERVED) — Designer wires to OnCalculateTotals
    // ═════════════════════════════════════════════════════════════════

    private void OnCalculateTotals(object? sender, EventArgs? e)
    {
        CalculateTotals();
    }

    /// <summary>
    /// تغيير نوع الخصم (مبلغ / نسبة): يضبط الحد الأقصى للحقل (100 للنسبة) ثم يعيد الحساب.
    /// </summary>
    private void cmbDiscountType_SelectedIndexChanged(object? sender, EventArgs e)
    {
        bool isPercentage = cmbDiscountType.SelectedIndex == 1;

        numInvoiceDiscount.Maximum = isPercentage ? 100m : 1_000_000m;
        if (numInvoiceDiscount.Value > numInvoiceDiscount.Maximum)
            numInvoiceDiscount.Value = numInvoiceDiscount.Maximum; // تقييد وليس خطأ

        CalculateTotals();
    }

    /// <summary>
    /// يحوّل قيمة حقل الخصم (مبلغ أو نسبة) إلى مبلغ خصم فعلي مع نوعه وقيمته،
    /// مقيَّداً داخل الحدود: (0..الإجمالي) للمبلغ و(0..100) للنسبة.
    /// </summary>
    private (decimal Amount, SalesSystem.DTO.Enums.DiscountType Type, decimal Rate) ResolveInvoiceDiscount(decimal subTotal)
    {
        if (subTotal < 0m) subTotal = 0m;

        decimal value = numInvoiceDiscount.Value;

        // نسبة → مبلغ من الإجمالي
        if (cmbDiscountType.SelectedIndex == 1)
        {
            value = Math.Clamp(value, 0m, 100m);
            decimal percentAmount = Math.Round(subTotal * value / 100m, 2, MidpointRounding.AwayFromZero);
            if (percentAmount > subTotal) percentAmount = subTotal;
            return (percentAmount, SalesSystem.DTO.Enums.DiscountType.Percentage, value);
        }

        // مبلغ ثابت
        value = Math.Clamp(value, 0m, subTotal);
        return (value, SalesSystem.DTO.Enums.DiscountType.Amount, value);
    }

    /// <summary>
    /// إجمالي الفاتورة الصافي المستخدم في التحقق من السداد (بوابة الدفع النقدي الكامل).
    /// يتبع دستور الحسابات نفسه المستخدم في <see cref="CalculateTotals"/>:
    ///   NetTotal = SubTotal - InvoiceDiscount + TaxAmount + OtherCharges
    /// والخصم يُحسب عبر <see cref="ResolveInvoiceDiscount"/> ليحترم النوع (مبلغ / نسبة).
    /// </summary>
    private decimal ComputeTotalAmount()
    {
        decimal subTotal = _lines.Where(l => l.ProductId > 0).Sum(x => x.LineTotal);

        // الخصم: مبلغ ثابت أو نسبة من الإجمالي — لا يُستخدم numInvoiceDiscount.Value مباشرة
        decimal invoiceDiscount = ResolveInvoiceDiscount(subTotal).Amount;

        decimal taxableAmount = subTotal - invoiceDiscount;
        decimal taxAmount = CalculateTaxAmount(taxableAmount);

        decimal expenses = clsValidation.TryParseDecimal(txtExpenses.Text, out decimal exp) ? exp : 0m;

        return taxableAmount + taxAmount + expenses;
    }

    private void CalculateTotals()
    {
        if (_isUpdating) return;

        decimal subTotal = _lines.Where(l => l.ProductId > 0).Sum(x => x.LineTotal);

        // الخصم: مبلغ ثابت أو نسبة — يُحوَّل دائماً إلى مبلغ قبل الحساب
        var discount = ResolveInvoiceDiscount(subTotal);
        decimal invoiceDiscount = discount.Amount;

        decimal taxableAmount = subTotal - invoiceDiscount;
        decimal taxAmount = CalculateTaxAmount(taxableAmount);
        decimal expenses = clsValidation.TryParseDecimal(txtExpenses.Text, out decimal exp) ? exp : 0m;

        // دستور الحسابات: NetTotal = SubTotal - InvoiceDiscount + Tax + OtherCharges
        decimal netTotal = subTotal - invoiceDiscount + taxAmount + expenses;

        decimal totalPaid = txtPaid.DecimalValue;
        decimal dueAmount = netTotal - totalPaid;

        // Update quick info bar (إجمالي الكمية يعرض بثلاث خانات عشرية كحد أقصى)
        int itemCount = _lines.Count(l => l.ProductId > 0);
        decimal totalQuantity = _lines.Where(l => l.ProductId > 0).Sum(l => l.Quantity);

        lblQuickInfo.Text =
            $"عدد الأصناف: {clsFormat.Number(itemCount)} | إجمالي الكمية: {clsFormat.Quantity(totalQuantity)} | الإجمالي: {clsFormat.Money(netTotal)} | المدفوع: {clsFormat.Money(totalPaid)} | المتبقي: {clsFormat.Money(dueAmount)}";

        // ── التفقيط: المبلغ الصافي كتابةً ──
        try
        {
            lblTafqitValue.Text = ArabicNumberToWords.ToWords(netTotal);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "تعذر توليد نص التفقيط للمبلغ {Amount}", netTotal);
            lblTafqitValue.Text = string.Empty;
        }
    }

    private decimal CalculateTaxAmount(decimal subTotal)
    {
        // أُزيلت وحدة الضرائب (V2) — لا توجد ضرائب معرّفة في النظام، لذا قيمة الضريبة = 0.
        // صيغة القانون المالي (إن أُعيد تفعيل الضرائب):
        //   شاملة  : TaxAmount = SubTotal - (SubTotal / (1 + rate))
        //   مضافة  : TaxAmount = SubTotal * rate
        _ = subTotal;
        return 0m;
    }

    private void UpdateUIState()
    {
        bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
        bool locked = IsLocked; // Posted / Cancelled → قفل تلقائي كامل

        gbInvoiceInfo.Enabled = editable;
        pnlEntry.Enabled = editable;
        gbTotals.Enabled = editable;

        // ── الشبكة: قراءة فقط عند القفل، ولا إضافة سطور ──
        dgvItems.ReadOnly = locked;
        dgvItems.AllowUserToAddRows = !locked;

        // ── أوامر الأصناف: تتعطل عند القفل ──
        btnAddLine.Enabled = editable;
        btnEditRow.Enabled = editable;
        btnDeleteRow.Enabled = editable;
        btnCancelLine.Enabled = editable && _editingLineKey.HasValue;
        btnRefreshCatalog.Enabled = editable;

        // ── مؤشر القفل (القفل تلقائي — لا زر) ──
        lblLockIndicator.Text = locked ? "🔒 مقفلة" : "✏ قابلة للتعديل";
        lblLockIndicator.BackColor = locked ? Color.MistyRose : Color.Honeydew;
        lblLockIndicator.ForeColor = locked ? Color.Firebrick : Color.DarkGreen;

        btnSaveDraft.Visible = editable;
        btnPost.Visible = editable;
        btnCancelInvoice.Visible = _invoice != null && _invoice.Status == (byte)InvoiceStatus.Posted;

        // ── قائمة السياق: تعطيل التعديل/الحذف عند القفل ──
        tsmiEditRow.Enabled = !locked;
        tsmiDeleteRow.Enabled = !locked;
        tsmiCancelEdit.Enabled = !locked && _editingLineKey.HasValue;

        lblStatus.Text = _invoice?.Status switch
        {
            (byte)InvoiceStatus.Draft => "مسودة",
            (byte)InvoiceStatus.Posted => "تم الترحيل — مقفلة",
            (byte)InvoiceStatus.Cancelled => "ملغاة",
            _ => "جديد"
        };

        lblStatus.BackColor = _invoice?.Status switch
        {
            (byte)InvoiceStatus.Draft => Color.Gray,
            (byte)InvoiceStatus.Posted => Color.Green,
            (byte)InvoiceStatus.Cancelled => Color.Red,
            _ => ThemeHelper.Primary   // «جديد» — نفس لون المصمّم (RULE-N134)
        };

        if (btnPrintA4 != null) btnPrintA4.Visible = _invoice != null;
        if (btnPrintReceipt != null) btnPrintReceipt.Visible = _invoice != null;
        if (chkPrintSummary != null) chkPrintSummary.Visible = _invoice != null;

        // RULE-N147 — الزر الافتراضي (Enter) يتبع الوضع الحالي بعد كل انتقال حالة
        // (تحميل · جديد · حفظ/ترحيل · قفل) — بلا آلة حالة إضافية.
        _SyncAcceptButton();
    }

    // ═════════════════════════════════════════════════════════════════
    //  VALIDATION HELPERS (PRESERVED)
    // ═════════════════════════════════════════════════════════════════

    private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
    {
        TextBox Temp = ((TextBox)sender);
        if (!clsValidation.IsRequired(Temp.Text))   // GAP-08b — بيت التحقق الواحد (RULE-N169)
        {
            e.Cancel = true;
            errorProvider.SetError(Temp, "هذا الحقل مطلوب");
            clsInputFeedback.ShowRejectTooltip(Temp, "هذا الحقل مطلوب", tooltip);   // GAP-08b
        }
        else
        {
            e.Cancel = false;
            errorProvider.SetError(Temp, null);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  KEYBOARD SHORTCUTS (Form-level)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// هل التركيز داخل حقل إدخال؟ عندها تُترك F4/Esc/Delete لسلوك الحقل نفسه
    /// (بحث عن صنف · إلغاء الحقل · حذف حرف) ولا تُسحب لاختصار الشاشة (RULE-N47).
    /// </summary>
    private bool IsInputFocused()
        => txtBarcode.Focused || txtItemCode.Focused || txtQuantity.Focused || txtUnitPrice.Focused
           || txtLineDiscount.Focused || txtPaid.Focused || txtExpenses.Focused || txtNotes.Focused
           || cmbUnit.Focused || cmbCustomer.Focused || cmbWarehouse.Focused || cmbCashBox.Focused
           || cmbAccount.Focused || cmbDiscountType.Focused || numInvoiceDiscount.Focused
           || dtpDate.Focused || dtpLineExpiry.Focused || dgvItems.ContainsFocus;

    /// <summary>F4 = حفظ وترحيل — لا يُسحب من «بحث عن صنف» داخل حقول الأصناف (RULE-N42).</summary>
    private void HandlePostShortcut()
    {
        if (IsInputFocused()) return;
        if (btnPost is { Visible: true, Enabled: true }) btnPost.PerformClick();
    }

    /// <summary>Delete = إلغاء الفاتورة المرحّلة — لا يُسحب من حذف سطر الشبكة (RULE-N22a).</summary>
    private void HandleCancelInvoiceShortcut()
    {
        if (IsInputFocused()) return;
        if (btnCancelInvoice is { Visible: true, Enabled: true }) btnCancelInvoice.PerformClick();
    }

    /// <summary>Esc = إلغاء التعديل الجاري، وإلا خروج — وداخل حقل إدخال يُترك للحقل (RULE-N47).</summary>
    private void HandleEscapeShortcut()
    {
        if (_editingLineKey.HasValue)
        {
            CancelEdit();
            return;
        }

        if (IsInputFocused()) return;

        // حماية من الفقدان: فاتورة جديدة فيها أصناف غير محفوظة ⇒ تأكيد قبل الإغلاق.
        if (_invoice == null && _lines.Any(l => l.ProductId > 0)
            && !clsDialogs.Confirm("سيتم إغلاق الشاشة وفقدان الأصناف غير المحفوظة. هل تريد المتابعة؟"))
            return;

        btnClose.PerformClick();
    }

    private void frmAddUpdateSalesInvoice_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            _ = SaveInvoiceAsync(true);
        }
        else if (e.Control && e.KeyCode == Keys.S)
        {
            e.SuppressKeyPress = true;
            _ = SaveInvoiceAsync(false);
        }
    }

    // ═════════════════════════════════════════════════════════════════
    //  ENTER = الزر الأساسي للوضع الحالي (RULE-N147)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// الزر الأساسي الذي يستقبله <c>Enter</c> (RULE-N147) — يُقرأ عند كل ضغطة لا مرة واحدة:
    /// فاتورة جديدة أو مسودة قابلة للتحرير ⇒ «حفظ كمسودة» (حفظ غير مُتلِف يبقيها قابلة للتعديل).
    /// <para>
    /// الترحيل لا يُربط بـ<c>Enter</c> إطلاقاً: يبقى على زرّه و<c>F4</c> لأنه يخصم المخزون ويُرحّل
    /// القيد المحاسبي ويقفل الفاتورة نهائياً (§2.4) — فالمفتاح العابر لا يُرحّل مستنداً مالياً.
    /// </para>
    /// <para>
    /// حارس إلزامي: التركيز داخل شبكة الأصناف (أو محرّر خليتها أو منتقي تاريخ الانتهاء المغمور فيها)
    /// ⇒ <c>null</c>، فيبقى <c>Enter</c> لإدخال السطر داخل الشبكة ولا يحفظ/يرحّل من الشبكة أبداً (RULE-N22a).
    /// </para>
    /// </summary>
    private Button? _PrimaryActionButton()
    {
        if (dgvItems.ContainsFocus || dgvItems.IsCurrentCellInEditMode) return null;

        // مرحّلة أو ملغاة (مقفلة) ⇒ لا إجراء لـEnter: لا حفظ ولا ترحيل ولا بدء فاتورة جديدة بالخطأ.
        if (IsLocked) return null;

        // أثناء الحفظ الجاري الزر معطّل ⇒ لا Enter مزدوج.
        return btnSaveDraft.Enabled ? btnSaveDraft : null;
    }

    /// <summary>
    /// يزامن <see cref="Form.AcceptButton"/> مع الزر الأساسي للوضع الحالي (RULE-N147)، ويُفرّغه ما دام
    /// التركيز داخل شبكة الأصناف حتى لا يخطف <c>Enter</c> من تحرير الخلية (RULE-N22a).
    /// </summary>
    private void _SyncAcceptButton() => AcceptButton = _PrimaryActionButton();

    // ═════════════════════════════════════════════════════════════════
    //  NAVIGATION (Enter → Next Field)
    // ═════════════════════════════════════════════════════════════════

    /// <summary>
    /// Enter in any header/footer field → move to next control (like Tab).
    /// </summary>
    private void FieldEnter_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            this.SelectNextControl(this.ActiveControl, true, true, true, true);
        }
    }

    /// <summary>
    /// When warehouse changes → auto-select the matching store account (1104).
    /// </summary>
    private void cmbWarehouse_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // تغيير المستودع ⇒ إعادة قراءة أرصدة المستودع الجديد
        _warehouseStockForWarehouseId = -1;

        if (_isUpdating) return;
        if (cmbAccount.DataSource is not List<AccountDto> accounts || accounts.Count == 0) return;

        string warehouseName = cmbWarehouse.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(warehouseName)) return;

        var match = accounts.FirstOrDefault(a => warehouseName.Contains(a.NameAr));
        if (match != null)
            cmbAccount.SelectedValue = match.Id;
    }

    // ═════════════════════════════════════════════════════════════════
    //  NUMERIC KEYPRESS
    // ═════════════════════════════════════════════════════════════════

    private void NumericTextBox_KeyPress(object? sender, KeyPressEventArgs e)
    {
        if (sender is not TextBox txt) return;

        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        {
            e.Handled = true;
        }

        if (e.KeyChar == '.' && txt.Text.IndexOf('.') > -1)
        {
            e.Handled = true;
        }
    }

    private void cmbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isUpdating) return;
        UpdatePaymentTypeUI();
        CalculateTotals();
    }

    private void UpdatePaymentTypeUI()
    {
        bool isCredit = rbtnPaymentTypeCredit.Checked;   // آجل
        bool isCash = rbtnPaymentTypeCash.Checked;     // نقدي

        if (isCredit)
        {
        }
        else if (isCash)
        {
        }
    }

    private void chkApplyTax_CheckedChanged(object? sender, EventArgs e)
    {
        // أُزيلت وحدة الضرائب (V2) — يبقى اختيار الضريبة معطلاً دائماً.
        //cmbTax.Enabled = false;
        CalculateTotals();
    }

    private void lblDueVal_Click(object sender, EventArgs e)
    {

    }

    private void pnlEntry_Paint(object sender, PaintEventArgs e)
    {

    }

    private void lblLockIndicator_Click(object sender, EventArgs e)
    {

    }

    private void gbTotals_Enter(object sender, EventArgs e)
    {

    }
}

