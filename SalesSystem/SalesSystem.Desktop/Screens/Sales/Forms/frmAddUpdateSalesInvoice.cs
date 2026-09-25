using Serilog;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Common.Enums;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Models;

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Printing;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.DTO.Responses;
using SalesSystem.Models.Accounting.Enums;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

public partial class frmAddUpdateSalesInvoice : Form
{
    // ── DI ───────────────────────────────────────────────────────────
    private readonly IServiceProvider _serviceProvider;

    // ── Data ─────────────────────────────────────────────────────────
    private SalesInvoiceDto? _invoice;
    private BindingList<InvoiceLineItemViewModel> _lines = new();
    private List<ProductDto> _allProducts = new();
    private Dictionary<int, List<ProductUnitDto>> _productUnitsCache = new();
    private List<AccountDto> _bankAccounts = new();
    private List<TaxDto> _taxes = new();
    private bool _isUpdating;
    private Guid? _editingLineKey = null;
    private InvoiceLineItemViewModel? _editingOriginalItem = null;

    // ── Lazy Service Accessors ──────────────────────────────────────
    private ISalesService _salesService => _serviceProvider.GetRequiredService<ISalesService>();
    private ICustomerService _customerApi => _serviceProvider.GetRequiredService<ICustomerService>();
    private IProductService _productApi => _serviceProvider.GetRequiredService<IProductService>();
    private IWarehouseService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseService>();
    private IProductUnitService _productUnitApi => _serviceProvider.GetRequiredService<IProductUnitService>();
    private IInvoicePrinter _invoicePrinter => _serviceProvider.GetRequiredService<IInvoicePrinter>();
    private IReceiptPrinter _receiptPrinter => _serviceProvider.GetRequiredService<IReceiptPrinter>();
    private IStoreSettingsService _settingsApi => _serviceProvider.GetRequiredService<IStoreSettingsService>();
    private IAccountService _accountApi => _serviceProvider.GetRequiredService<IAccountService>();
    private ITaxService _taxApi => _serviceProvider.GetRequiredService<ITaxService>();
    private IInventoryService _inventoryApi => _serviceProvider.GetRequiredService<IInventoryService>();

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
        ApplyStyles();
        ThemeHelper.ApplyDataGridViewStyle(dgvItems);
        dgvItems.AutoGenerateColumns = false;
        dgvItems.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvItems.DataSource = _lines;

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
        txtBankAmount.Enter += NumericTextBox_Enter;
        txtExpenses.Enter += NumericTextBox_Enter;
        txtNotes.Enter += NumericTextBox_Enter;
        numInvoiceDiscount.Enter += NumericTextBox_Enter;

        // Header/Footer combos — Enter moves to next field
        cmbCustomer.KeyDown += FieldEnter_KeyDown;
        cmbWarehouse.KeyDown += FieldEnter_KeyDown;
        cmbPaymentType.KeyDown += FieldEnter_KeyDown;
        cmbBank.KeyDown += FieldEnter_KeyDown;
        cmbTax.KeyDown += FieldEnter_KeyDown;
        cmbAccount.KeyDown += FieldEnter_KeyDown;
        cmbDiscountType.KeyDown += FieldEnter_KeyDown;
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

        // Context Menu
        tsmiEditRow.Click += (s, e) => { if (dgvItems.CurrentRow?.Index >= 0 && dgvItems.CurrentRow.Index < _lines.Count) LoadRowForEditing(dgvItems.CurrentRow.Index); };
        tsmiCancelEdit.Click += (s, e) => CancelEdit();
        tsmiDeleteRow.Click += (s, e) => { if (dgvItems.CurrentRow?.Index >= 0) DeleteSelectedRow(); };

        // Discount type changed
        cmbDiscountType.SelectedIndexChanged += OnCalculateTotals;

        // Tax checkbox — when checked (شاملة/inclusive), disable cmbTax since values don't change
        chkApplyTax.CheckedChanged += chkApplyTax_CheckedChanged;

        // Expenses numeric validation
        txtExpenses.KeyPress += NumericTextBox_KeyPress;

        // Payment area — Enter triggers next field
        txtPaid.KeyDown += FieldEnter_KeyDown;
        txtBankAmount.KeyDown += FieldEnter_KeyDown;

        // Notes / date — Enter triggers next field
        txtNotes.KeyDown += FieldEnter_KeyDown;
        dtpDate.KeyDown += FieldEnter_KeyDown;

        // Unified focus indicator for all input controls
        ThemeHelper.WireFocusIndicator(
            txtBarcode, cmbUnit, txtQuantity, txtUnitPrice, txtLineDiscount,  // Entry panel
            txtPaid, txtBankAmount, txtExpenses, txtNotes,                     // Payment / notes
            cmbCustomer, cmbWarehouse, cmbPaymentType,                         // Header combos
            cmbBank, cmbTax, cmbAccount, cmbDiscountType,                      // Footer combos
            numInvoiceDiscount, dtpDate                                        // Numeric / date
        );
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

            cmbDiscountType.SelectedIndex = 0;

            if (_invoice != null)
            {
                BindInvoice();
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
            MessageBox.Show("حدث خطأ أثناء تحميل الشاشة. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    public void LoadData(SalesInvoiceDto? invoice)
    {
        _invoice = invoice;
        ApplyStyles();
    }

    // ═════════════════════════════════════════════════════════════════
    //  STYLES
    // ═════════════════════════════════════════════════════════════════

    private void ApplyStyles()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        lblMode.Text = _invoice == null ? "فاتورة بيع جديدة" : $"فاتورة بيع - {_invoice.InvoiceNo}";

        ThemeHelper.ApplyButtonStyle(btnSaveDraft, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnPost, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancelInvoice, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        ApplyUiPolish();
    }

    /// <summary>
    /// Visual polish applied on top of the base theme — totals emphasis,
    /// entry-panel buttons, quick-info bar and group headers.
    /// </summary>
    private void ApplyUiPolish()
    {
        // ── Entry panel buttons ──
        ThemeHelper.ApplyButtonStyle(btnAddLine, ThemeHelper.ButtonType.Success);
        btnAddLine.Text = "➕ إضافة";
        btnAddLine.BackColor = Color.FromArgb(25, 135, 84);
        ThemeHelper.ApplyButtonStyle(btnCancelLine, ThemeHelper.ButtonType.Warning);
        btnCancelLine.ForeColor = Color.FromArgb(60, 45, 5); // readable text on amber
        btnCancelLine.Enabled = false;

        // ── Print / cancel invoice buttons ──
        ThemeHelper.ApplyButtonStyle(btnPrintA4, ThemeHelper.ButtonType.Secondary);
        btnPrintA4.ForeColor = ThemeHelper.TextPrimary;
        ThemeHelper.ApplyButtonStyle(btnPrintReceipt, ThemeHelper.ButtonType.Secondary);
        btnPrintReceipt.ForeColor = ThemeHelper.TextPrimary;

        // ── Group headers ──
        foreach (var gb in new[] { gbInvoiceInfo, gbEntry, gbItems, gbTotals })
        {
            gb.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gb.ForeColor = ThemeHelper.TextPrimary;
        }

        // ── Totals area emphasis ──
        lblTotalVal.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblTotalVal.ForeColor = ThemeHelper.Primary;
        lblDueVal.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblSubTotalVal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblDiscountAmountVal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblLineTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblLineTotal.ForeColor = ThemeHelper.Primary;

        // ── Quick info bar ──
        lblQuickInfo.BackColor = Color.FromArgb(248, 249, 250);
        lblQuickInfo.ForeColor = ThemeHelper.TextDarkGray;
        lblQuickInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblQuickInfo.Padding = new Padding(8, 6, 8, 6);

        // ── Status badge ──
        lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblStatus.ForeColor = Color.White;
        lblStatus.Padding = new Padding(10, 4, 10, 4);
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        lblStatus.AutoSize = true;
    }

    // ═════════════════════════════════════════════════════════════════
    //  DATA LOADING
    // ═════════════════════════════════════════════════════════════════

    private async Task LoadLookups()
    {
        try
        {
            // Customers — must be selected, no default
            var customersRes = await _customerApi.GetAllAsync(null, 1, int.MaxValue);
            if (!customersRes.IsSuccess || customersRes.Value == null)
                throw new InvalidOperationException(customersRes.Error ?? "فشل تحميل قائمة العملاء");
            var list = customersRes.Value.Items.ToList();
            list.Insert(0, new CustomerDto(0, "--- اختر عميل ---", null, null, null, null, 0, true, 0));
            cmbCustomer.DataSource = list;
            cmbCustomer.DisplayMember = "Name";
            cmbCustomer.ValueMember = "Id";
            cmbCustomer.SelectedIndex = 0; // "--- اختر عميل ---"

            // Warehouses
            var warehouseRes = await _warehouseApi.GetAllAsync(null, 1, int.MaxValue);
            if (!warehouseRes.IsSuccess || warehouseRes.Value == null)
                throw new InvalidOperationException(warehouseRes.Error ?? "فشل تحميل قائمة المستودعات");
            cmbWarehouse.DataSource = warehouseRes.Value.Items;
            cmbWarehouse.DisplayMember = "Name";
            cmbWarehouse.ValueMember = "Id";

            if (_invoice == null)
            {
                var def = warehouseRes.Value.Items.FirstOrDefault(w => w.IsDefault);
                if (def != null) cmbWarehouse.SelectedValue = def.Id;
            }

            // Products (for barcode lookup)
            var productRes = await _productApi.GetAllAsync(null, null, 1, int.MaxValue);
            if (productRes.IsSuccess && productRes.Value != null)
            {
                _allProducts = productRes.Value.Items.ToList();
            }

            // Taxes — load from DB
            var taxesRes = await _taxApi.GetAllAsync(default);
            _taxes = taxesRes.IsSuccess && taxesRes.Value != null ? taxesRes.Value : [];
            var activeTaxes = _taxes.Where(t => t.IsActive).ToList();
            cmbTax.DisplayMember = "Name";
            cmbTax.ValueMember = "Id";
            cmbTax.DataSource = activeTaxes;
            if (activeTaxes.Count > 0)
            {
                var defaultTax = activeTaxes.FirstOrDefault(t => t.IsDefault) ?? activeTaxes[0];
                cmbTax.SelectedValue = defaultTax.Id;
            }
            cmbTax.Enabled = false; // disabled until checkbox is checked

            // Payment type default
            cmbPaymentType.SelectedIndex = 0;

            // Banks from ChartOfAccounts — only accounts under "1102" (البنوك)
            var assetAccounts = await _accountApi.GetByTypeAsync(AccountType.Asset, default);
            _bankAccounts = assetAccounts.IsSuccess
                ? assetAccounts.Value.Where(a => a.IsLeaf && !a.IsSystem
                    && a.AccountCode.StartsWith("1102")).ToList()
                : new List<AccountDto>();

            cmbBank.DataSource = _bankAccounts;
            cmbBank.DisplayMember = "NameAr";
            cmbBank.ValueMember = "Id";
            if (_bankAccounts.Count > 0) cmbBank.SelectedIndex = 0;

            // Store/Inventory accounts for cmbAccount (المخزون = 1104 prefix)
            var storeAccounts = assetAccounts.IsSuccess
                ? assetAccounts.Value.Where(a => a.IsLeaf && !a.IsSystem
                    && a.AccountCode.StartsWith("1104")).ToList()
                : new List<AccountDto>();
            cmbAccount.DataSource = storeAccounts;
            cmbAccount.DisplayMember = "NameAr";
            cmbAccount.ValueMember = "Id";
            if (storeAccounts.Count > 0) cmbAccount.SelectedIndex = 0;

            // Auto-select store account when warehouse changes
            cmbWarehouse.SelectedIndexChanged += cmbWarehouse_SelectedIndexChanged;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل القوائم");
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task<List<ProductUnitDto>> LoadProductUnitsAsync(int productId)
    {
        if (_productUnitsCache.TryGetValue(productId, out var cached))
            return cached;

        try
        {
            var unitsResult = await _productUnitApi.GetByProductIdAsync(productId, default);
            if (unitsResult.IsSuccess && unitsResult.Value != null)
            {
                _productUnitsCache[productId] = unitsResult.Value;
                return unitsResult.Value;
            }
            return [];
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل وحدات المنتج {ProductId}", productId);
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
        => MessageBox.Show("هذا المنتج مضاف مسبقاً للقائمة.", "تنبيه",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);

    /// <summary>
    /// Quantity of a product already on the invoice, excluding the row currently being edited.
    /// </summary>
    private decimal GetExistingQtyForProduct(int productId)
        => _lines.Where(l => l.ProductId == productId
            && (_editingLineKey == null || l.LineKey != _editingLineKey.Value))
            .Sum(l => l.Quantity);

    /// <summary>
    /// Early stock validation — blocks the line before it is added if the
    /// requested quantity exceeds available stock in the selected warehouse.
    /// Returns true (allow) when stock cannot be verified — server validates at post time.
    /// </summary>
    private async Task<bool> IsStockSufficientAsync(int productId, decimal totalRequestedQty)
    {
        if (totalRequestedQty <= 0) return true;
        if (cmbWarehouse.SelectedValue is not short wid || wid <= 0) return true;

        var stockRes = await _inventoryApi.GetStockAsync(productId, wid, default);
        if (!stockRes.IsSuccess) return true; // can't verify here — server decides at posting

        decimal available = stockRes.Value;
        if (totalRequestedQty <= available) return true;

        MessageBox.Show(
            $"الكمية غير كافية في المخزون.\nالمتوفرة: {available:G} — المطلوبة: {totalRequestedQty:G}",
            "المخزون غير كافٍ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        cmbPaymentType.SelectedIndex = _invoice.PaymentType - 1;

        // Restore discount type and value
        var discType = (SalesSystem.DTO.Enums.DiscountType)_invoice.DiscountType;
        cmbDiscountType.SelectedIndex = discType == SalesSystem.DTO.Enums.DiscountType.Percentage ? 1 : 0;
        numInvoiceDiscount.Value = discType == SalesSystem.DTO.Enums.DiscountType.Percentage
            ? (_invoice.DiscountRate ?? 0m)
            : _invoice.DiscountAmount;

        // Restore tax — if invoice has a TaxId, tax was applied
        bool hasTax = _invoice.TaxId.HasValue && _invoice.TaxId.Value > 0;
        chkApplyTax.Checked = hasTax;
        if (hasTax && _taxes.Any(t => t.Id == _invoice.TaxId!.Value))
            cmbTax.SelectedValue = _invoice.TaxId!.Value;
        cmbTax.Enabled = !chkApplyTax.Checked;

        // Restore payment amounts — split PaidAmount into cash vs bank
        decimal totalPaid = _invoice.PaidAmount;
        if (_invoice.PaymentType == 3) // Mixed — keep paid amount as-is, bank is separate
        {
            txtPaid.Text = totalPaid.ToString("N2");
            txtBankAmount.Text = "0";
        }
        else if (_invoice.PaymentType == 2) // Credit — paid should be 0
        {
            txtPaid.Text = "0";
            txtBankAmount.Text = "0";
        }
        else // Cash
        {
            txtPaid.Text = totalPaid.ToString("N2");
            txtBankAmount.Text = "0";
        }

        txtExpenses.Text = _invoice.OtherCharges.ToString("N2");
        txtNotes.Text = _invoice.Notes;

        // Build lines with ProductUnitId for correct unit restore
        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItemViewModel
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode ?? "",
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                UnitName = "", // will be loaded async
                ProductUnitId = item.ProductUnitId,
                Barcode = item.ProductCode ?? ""
            });
        }
        UpdatePaymentTypeUI();
        CalculateTotals();

        // Load units async AFTER _isUpdating is still true (prevents premature events)
        _ = LoadAllUnitsForLinesAsync();
        _isUpdating = false;
    }

    private async Task LoadAllUnitsForLinesAsync()
    {
        foreach (var line in _lines.Where(l => l.ProductId > 0))
        {
            if (string.IsNullOrEmpty(line.UnitName))
            {
                var (_, savedUnit) = await GetUnitsWithDefaultAsync(line.ProductId, line.ProductUnitId);
                line.UnitName = savedUnit?.UnitName ?? "";
            }
        }
        dgvItems.Refresh();
    }

    private void ResetForm()
    {
        lblMode.Text = "فاتورة بيع جديدة";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        cmbCustomer.SelectedIndex = 0; // "--- اختر عميل ---"
        cmbPaymentType.SelectedIndex = 0;
        _lines.Clear();
        cmbDiscountType.SelectedIndex = 0;
        numInvoiceDiscount.Value = 0;
        txtPaid.Text = "0";
        txtNotes.Clear();
        txtBankAmount.Text = "0";
        txtExpenses.Text = "0";
        chkApplyTax.Checked = false;
        cmbTax.Enabled = true;
        lblLineTotal.Text = "0.00";
        ClearEntryPanel();
    }

    // ═════════════════════════════════════════════════════════════════
    //  ENTRY PANEL — BARCODE LOOKUP
    // ═════════════════════════════════════════════════════════════════

    private async void txtBarcode_KeyDown(object? sender, KeyEventArgs e)
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
            MessageBox.Show("حدث خطأ أثناء البحث عن المنتج.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void FillProductDetails(ProductDto product)
    {
        if (!_editingLineKey.HasValue && IsDuplicateProduct(product.Id))
        {
            WarnDuplicateProduct();
            txtBarcode.Clear();
            txtBarcode.Focus();
            return;
        }

        // Load units
        var (units, baseUnit) = await GetUnitsWithDefaultAsync(product.Id);

        // Display product name
        lblProductName.Text = product.Name;

        // Populate unit combo
        cmbUnit.DisplayMember = "UnitName";
        cmbUnit.ValueMember = "Id";
        cmbUnit.DataSource = new List<ProductUnitDto>(units);
        if (baseUnit != null)
            cmbUnit.SelectedValue = baseUnit.Id;

        // Store product ID in tag
        cmbUnit.Tag = product.Id;

        // Pre-fill defaults for new rows
        if (!_editingLineKey.HasValue)
        {
            txtUnitPrice.Clear();
            txtQuantity.Text = "1";
        }

        // Focus unit combo
        cmbUnit.Focus();
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

        // If invoked from DGV (F3 while on a DGV cell), apply product to that DGV row
        if (_dgvSearchReturnRow >= 0 && _dgvSearchReturnRow < dgvItems.Rows.Count)
        {
            await ApplyProductToDgvRowAsync(_dgvSearchReturnRow, product);
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

        FillProductDetails(product);
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
                MessageBox.Show("الرجاء اختيار منتج أولاً (مسح باركود أو F4).", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBarcode.Focus();
                return;
            }

            if (cmbUnit.SelectedValue == null || Convert.ToInt32(cmbUnit.SelectedValue) <= 0)
            {
                MessageBox.Show("الرجاء اختيار وحدة للمنتج.", "بيانات غير مكتملة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (!decimal.TryParse(txtQuantity.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show("الرجاء إدخال كمية صحيحة (أكبر من صفر).", "بيانات غير صحيحة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

    private async void txtUnitPrice_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("الرجاء إدخال سعر صحيح.", "بيانات غير صحيحة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

    private async void txtLineDiscount_KeyDown(object? sender, KeyEventArgs e)
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
        ClearEntryPanel();
        foreach (DataGridViewRow r in dgvItems.Rows)
            r.DefaultCellStyle.BackColor = Color.White;
        btnAddLine.Text = "➕ إضافة";
        btnAddLine.BackColor = Color.FromArgb(25, 135, 84);
        gbEntry.Text = "إدخال الأصناف";
    }

    private void RecalculateLineTotal(object? sender, EventArgs? e)
    {
        decimal qty = decimal.TryParse(txtQuantity.Text, out decimal q) ? q : 0m;
        decimal price = decimal.TryParse(txtUnitPrice.Text, out decimal p) ? p : 0m;
        decimal discount = decimal.TryParse(txtLineDiscount.Text, out decimal d) ? d : 0m;
        decimal total = (qty * price) - discount;
        if (total < 0m) total = 0m;
        lblLineTotal.Text = total.ToString("N2");
    }

    private async void btnAddLine_Click(object? sender, EventArgs? e)
    {
        await AddOrUpdateRowAsync();
    }

    private void btnCancelLine_Click(object? sender, EventArgs? e)
    {
        CancelEdit();
    }

    private async Task AddOrUpdateRowAsync()
    {
        if (!ValidateEntryFields()) return;

        int productId = Convert.ToInt32(cmbUnit.Tag!);
        int productUnitId = Convert.ToInt32(cmbUnit.SelectedValue!);
        decimal quantity = decimal.Parse(txtQuantity.Text);
        decimal unitPrice = decimal.TryParse(txtUnitPrice.Text, out decimal up) ? up : 0m;
        decimal lineDiscount = decimal.TryParse(txtLineDiscount.Text, out decimal ld) ? ld : 0m;
        string barcode = txtBarcode.Text.Trim();
        string productName = lblProductName.Text;
        string unitName = cmbUnit.Text;

        // ── MERGE QUANTITY: if product already exists, add qty to existing row ──
        if (!_editingLineKey.HasValue)
        {
            var existingRow = _lines.FirstOrDefault(l => l.ProductId == productId && l.ProductUnitId == productUnitId);
            if (existingRow != null)
            {
                decimal newQty = existingRow.Quantity + quantity;
                decimal totalRequested = GetExistingQtyForProduct(productId) - existingRow.Quantity + newQty;
                if (!await IsStockSufficientAsync(productId, totalRequested))
                {
                    txtQuantity.Focus();
                    txtQuantity.SelectAll();
                    return;
                }
                existingRow.Quantity = newQty;
                // Update discount only if non-zero entered
                if (lineDiscount > 0m) existingRow.LineDiscount = lineDiscount;
                ResetEntryState();
                CalculateTotals();
                txtBarcode.Focus();
                return;
            }
        }

        // ── Early stock validation ──
        decimal totalReq = GetExistingQtyForProduct(productId) + quantity;
        if (!await IsStockSufficientAsync(productId, totalReq))
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
                row.Quantity = quantity;
                row.UnitPrice = unitPrice;
                row.LineDiscount = lineDiscount;
            }
        }
        else
        {
            // ── Add new row ──
            _lines.Add(new InvoiceLineItemViewModel
            {
                ProductId = productId,
                ProductCode = barcode,
                Barcode = barcode,
                ProductName = productName,
                UnitName = unitName,
                ProductUnitId = productUnitId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                LineDiscount = lineDiscount
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
                row.Quantity = _editingOriginalItem.Quantity;
                row.UnitPrice = _editingOriginalItem.UnitPrice;
            }
        }

        ResetEntryState();
        txtBarcode.Focus();
    }

    private void ClearEntryPanel()
    {
        txtBarcode.Clear();
        lblProductName.Text = "";
        cmbUnit.DataSource = null;
        cmbUnit.Tag = null;
        txtQuantity.Text = "1";
        txtUnitPrice.Clear();
        txtLineDiscount.Text = "0";
        lblLineTotal.Text = "0.00";
    }

    private bool ValidateEntryFields()
    {
        if (cmbUnit.Tag == null || Convert.ToInt32(cmbUnit.Tag) <= 0)
        {
            MessageBox.Show("الرجاء اختيار منتج أولاً (مسح باركود أو F4).", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtBarcode.Focus();
            return false;
        }

        if (cmbUnit.SelectedValue == null || Convert.ToInt32(cmbUnit.SelectedValue) <= 0)
        {
            MessageBox.Show("الرجاء اختيار الوحدة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbUnit.Focus();
            return false;
        }

        if (!decimal.TryParse(txtQuantity.Text, out decimal qty) || qty <= 0)
        {
            MessageBox.Show("الرجاء إدخال كمية صحيحة (أكبر من صفر).", "بيانات غير صحيحة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtQuantity.Focus();
            txtQuantity.SelectAll();
            return false;
        }

        if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price < 0)
        {
            MessageBox.Show("الرجاء إدخال سعر صحيح.", "بيانات غير صحيحة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        e.Row.Cells[colQuantity.Index].Value = 1m;
        e.Row.Cells[colUnitPrice.Index].Value = 0m;
        e.Row.Cells[colLineTotal.Index].Value = 0m;
    }

    private async void dgvItems_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= dgvItems.Rows.Count) return;

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
                cmb.BackColor = Color.FromArgb(255, 255, 180); // Visible yellow focus
            }
        }
        else if (dgvItems.CurrentCell.ColumnIndex == colQuantity.Index || dgvItems.CurrentCell.ColumnIndex == colUnitPrice.Index)
        {
            if (e.Control is TextBox txt)
            {
                txt.KeyPress -= NumericTextBox_KeyPress;
                txt.KeyPress += NumericTextBox_KeyPress;
            }
        }
    }

    private async void BarcodeCell_KeyDown(object? sender, KeyEventArgs e)
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
                MessageBox.Show("لم يتم العثور على منتج بهذا الباركود.", "بحث",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            await ApplyProductToDgvRowAsync(rowIndex, dialog.SelectedProduct);
        }
    }

    private async Task ApplyProductToDgvRowAsync(int rowIndex, ProductDto product)
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

        // ── Early stock validation — includes the row's CURRENT quantity ──
        decimal existingQty = rowIndex < _lines.Count ? _lines[rowIndex].Quantity : 0m;
        decimal requestedQty = existingQty > 0 ? existingQty : 1m;
        decimal otherRowsQty = _lines.Where(l => l.ProductId == product.Id
            && !(rowIndex < _lines.Count && l.LineKey == _lines[rowIndex].LineKey))
            .Sum(l => l.Quantity);
        if (!await IsStockSufficientAsync(product.Id, otherRowsQty + requestedQty))
            return;

        InvoiceLineItemViewModel line;
        if (rowIndex < _lines.Count)
        {
            line = _lines[rowIndex];
        }
        else
        {
            line = new InvoiceLineItemViewModel();
            _lines.Add(line);
        }

        line.ProductId = product.Id;
        line.ProductCode = product.Barcode ?? "";
        line.Barcode = product.Barcode ?? "";
        line.ProductName = product.Name;
        line.UnitName = baseUnit?.UnitName ?? product.UnitName ?? "";
        line.ProductUnitId = baseUnit?.Id ?? 0;
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

            var line = new InvoiceLineItemViewModel
            {
                ProductId = product.Id,
                ProductCode = product.Barcode ?? "",
                Barcode = product.Barcode ?? "",
                ProductName = product.Name,
                Quantity = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value ?? 1m),
                UnitPrice = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colUnitPrice.Index].Value ?? 0m),
                UnitName = dgvItems.Rows[e.RowIndex].Cells[colUnit.Index].Value?.ToString() ?? ""
            };

            // ── Early stock validation for new inline row ──
            if (!await IsStockSufficientAsync(product.Id, GetExistingQtyForProduct(product.Id) + line.Quantity))
                return;

            _lines.Add(line);
            dgvItems.Rows[e.RowIndex].Cells[colLineTotal.Index].Value = line.LineTotal;
            CalculateTotals();
            return;
        }

        var existingLine = _lines[e.RowIndex];

        // Always sync ALL editable values from DGV back to _lines
        decimal previousQty = existingLine.Quantity;
        if (dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value != null)
            existingLine.Quantity = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value);

        // ── Early stock validation on direct quantity edit ──
        if (existingLine.Quantity != previousQty)
        {
            decimal otherRowsQty = GetExistingQtyForProduct(existingLine.ProductId);
            bool sufficient = await IsStockSufficientAsync(existingLine.ProductId, otherRowsQty + existingLine.Quantity);
            if (!sufficient)
            {
                existingLine.Quantity = previousQty; // revert to previous quantity
                dgvItems.Rows[e.RowIndex].Cells[colQuantity.Index].Value = previousQty;
                CalculateTotals();
                return;
            }
        }

        if (dgvItems.Rows[e.RowIndex].Cells[colUnitPrice.Index].Value != null)
            existingLine.UnitPrice = Convert.ToDecimal(dgvItems.Rows[e.RowIndex].Cells[colUnitPrice.Index].Value);

        // Always sync UnitName — ComboBox value might have changed regardless of which column was edited
        var unitVal = dgvItems.Rows[e.RowIndex].Cells[colUnit.Index].Value?.ToString();
        if (!string.IsNullOrEmpty(unitVal))
            existingLine.UnitName = unitVal;

        // Update displayed LineTotal
        dgvItems.Rows[e.RowIndex].Cells[colLineTotal.Index].Value = existingLine.LineTotal;

        CalculateTotals();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في إنهاء تحرير خلية الفاتورة");
        }
    }

    private void dgvItems_KeyDown(object? sender, KeyEventArgs e)
    {
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
    }

    private static Image? _trashIcon;
    private static Image GetTrashIcon()
    {
        if (_trashIcon != null) return _trashIcon;

        var bmp = new Bitmap(20, 20);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using var pen = new Pen(Color.FromArgb(220, 53, 69), 2f);

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
        if (e.RowIndex < 0 || e.ColumnIndex != colDelete.Index) return;
        if (e.RowIndex >= _lines.Count) return;
        if (dgvItems.IsCurrentCellInEditMode) return;

        _lines.RemoveAt(e.RowIndex);
        CalculateTotals();
    }

    private void dgvItems_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
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
    //  Editable order: Barcode(1) → Unit(3) → Quantity(4) → Price(5) → Quick Input Barcode
    // ═════════════════════════════════════════════════════════════════

    private static readonly int[] EditableDgvColumns = { 1, 3, 4, 5 }; // Barcode, Unit, Quantity, Price
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
            txtPaid.Focus();
            BeginInvoke(() => txtPaid.SelectAll());
            return true;
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
        if (keyData == Keys.Escape && dgvItems.Focused && dgvItems.IsCurrentCellInEditMode)
        {
            dgvItems.CancelEdit();
            return true;
        }

        // ── Esc: Cancel entry panel edit mode ──
        if (keyData == Keys.Escape && _editingLineKey.HasValue)
        {
            CancelEdit();
            return true;
        }

        // ── Enter: DGV keyboard navigation engine ──
        if (keyData == Keys.Enter && dgvItems.Focused && dgvItems.CurrentCell != null)
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

    private async void LoadRowForEditing(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= _lines.Count) return;

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

        cmbUnit.DisplayMember = "UnitName";
        cmbUnit.ValueMember = "Id";
        cmbUnit.DataSource = new List<ProductUnitDto>(units);
        if (selectedUnit != null)
            cmbUnit.SelectedValue = selectedUnit.Id;
        else if (units.Count > 0)
            cmbUnit.SelectedIndex = 0;
        cmbUnit.Tag = productId;

        txtQuantity.Text = row.Quantity.ToString("G");
        txtUnitPrice.Text = row.UnitPrice.ToString("G");
        RecalculateLineTotal(null, null);

        // Set editing mode
        btnAddLine.Text = "✏️ تعديل";
        btnAddLine.BackColor = Color.FromArgb(255, 193, 7);
        btnCancelLine.Enabled = true;
        tsmiCancelEdit.Enabled = true;
        gbEntry.Text = $"تعديل السطر #{rowIndex + 1} — {productName}";

        foreach (DataGridViewRow r in dgvItems.Rows)
        {
            r.DefaultCellStyle.BackColor = Color.White;
        }
        dgvItems.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 180);

        txtBarcode.Focus();
        txtBarcode.SelectAll();
    }

    private void DeleteSelectedRow()
    {
        if (dgvItems.CurrentRow?.DataBoundItem is not InvoiceLineItemViewModel line)
            return;

        var result = MessageBox.Show("هل أنت متأكد من حذف هذا الصنف من الفاتورة؟",
            "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result != DialogResult.Yes) return;

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
        if (MessageBox.Show("هل أنت متأكد من إلغاء هذه الفاتورة؟", "تأكيد الإلغاء",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            Cursor = Cursors.WaitCursor;
            await _salesService.CancelAsync(_invoice.Id, SessionManager.UserId, default);
            MessageBox.Show("تم إلغاء الفاتورة", "نجاح",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء إلغاء الفاتورة");
            MessageBox.Show("حدث خطأ أثناء إلغاء الفاتورة. تم تسجيل التفاصيل.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnPrintA4_Click(object? sender, EventArgs e)
        => await PrintInvoiceAsync(_invoicePrinter, "A4");

    private async void btnPrintReceipt_Click(object? sender, EventArgs e)
        => await PrintInvoiceAsync(_receiptPrinter, "القصيرة");

    private async Task PrintInvoiceAsync(IPrinterService printer, string label)
    {
        if (_invoice == null) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            var settingsRes = await _settingsApi.GetSettingsAsync();
            if (!settingsRes.IsSuccess)
            {
                MessageBox.Show("فشل تحميل إعدادات الطباعة: " + settingsRes.Error, "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var storeInfo = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(settingsRes.Value);
            var printInvoice = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(_invoice);
            var printItems = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDtos(_invoice.Items);
            var printTotals = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToTotalsPrintDto(_invoice);

            printer.PrintPreview(printInvoice, printItems, printTotals, storeInfo);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في طباعة الفاتورة {Label}", label);
            MessageBox.Show("حدث خطأ أثناء الطباعة. تم تسجيل التفاصيل في السجل.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => this.Close();

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
            cmbCustomer.Focus();
            MessageBox.Show("يجب اختيار العميل", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Warehouse must be selected
        if (cmbWarehouse.SelectedValue == null || Convert.ToInt32(cmbWarehouse.SelectedValue) <= 0)
        {
            errorProvider.SetError(cmbWarehouse, "يجب اختيار المستودع");
            cmbWarehouse.Focus();
            MessageBox.Show("يجب اختيار المستودع", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // Must have at least one product
        if (!_lines.Any(l => l.ProductId > 0))
        {
            errorProvider.SetError(dgvItems, "لم يتم إضافة أي منتج إلى الفاتورة");
            dgvItems.Focus();
            MessageBox.Show("أضف منتج واحد على الأقل", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // Discount validation
        decimal subTotal = _lines.Where(l => l.ProductId > 0).Sum(l => l.LineTotal);
        decimal discountVal = numInvoiceDiscount.Value;
        if (cmbDiscountType.SelectedIndex == 1 && discountVal > 100)
            errors.Add("نسبة الخصم لا يمكن أن تتجاوز 100%");
        if (cmbDiscountType.SelectedIndex == 0 && discountVal > subTotal && subTotal > 0)
            errors.Add("مبلغ الخصم لا يمكن أن يتجاوز إجمالي الفاتورة");

        // ── Payment validation ──
        int paymentIdx = cmbPaymentType.SelectedIndex;
        bool isCash = paymentIdx == 0;     // نقدي
        bool isCredit = paymentIdx == 1;   // آجل
        bool isMixed = paymentIdx == 2;    // مختلط

        decimal paidCash = txtPaid.DecimalValue;
        decimal paidBank = txtBankAmount.DecimalValue;
        decimal totalPaid = paidCash + paidBank;

        // Real invoice total (tax + expenses) — shared threshold with server guard
        decimal totalAmount = ComputeTotalAmount();

        // Negative amounts
        if (paidCash < 0) { txtPaid.Focus(); errors.Add("المبلغ النقدي لا يمكن أن يكون سالباً"); }
        if (paidBank < 0) { txtBankAmount.Focus(); errors.Add("المبلغ البنكي لا يمكن أن يكون سالباً"); }

        // ── Cash (نقدي): must be fully paid — no partial cash sales ──
        if (isCash)
        {
            if (totalPaid < totalAmount)
            {
                errorProvider.SetError(txtPaid, "المبلغ المدفوع أقل من الإجمالي");
                txtPaid.Focus();
                MessageBox.Show($"البيع النقدي يتطلب دفع كامل المبلغ.\nالإجمالي: {totalAmount:N2} — المدفوع: {totalPaid:N2}\nالمتبقي: {totalAmount - totalPaid:N2}",
                    "بيانات غير مكتملة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Cash + bank fields shouldn't mix — bank is disabled in cash mode anyway
        }

        // ── Credit (آجل): paid must be 0 ──
        if (isCredit && totalPaid > 0)
        {
            errors.Add("البيع الآجل — لا يُسمح بأي مبلغ مدفوع");
            txtPaid.Focus();
        }

        // ── Mixed (مختلط): needs at least one payment; bank requires account; cannot overpay silently ──
        if (isMixed)
        {
            if (paidBank <= 0 && paidCash <= 0)
            {
                errors.Add("البيع المختلط — أدخل مبلغ نقدي أو بنكي");
                txtPaid.Focus();
            }
            if (paidBank > 0 && (cmbBank.SelectedValue == null || Convert.ToInt32(cmbBank.SelectedValue) <= 0))
            {
                errorProvider.SetError(cmbBank, "يجب اختيار الحساب البنكي");
                cmbBank.Focus();
                errors.Add("يجب اختيار الحساب البنكي عند الدفع البنكي");
            }
            if (totalPaid >= totalAmount)
            {
                errors.Add("البيع المختلط — المدفوع يغطي كامل المبلغ، استخدم البيع النقدي بدلاً من ذلك");
                txtPaid.Focus();
            }
        }

        // Paid amount > total → warn (overpayment, allowed only on confirmation for cash/mixed edge cases)
        if (totalPaid > 0 && totalPaid > totalAmount && errors.Count == 0)
        {
            if (MessageBox.Show($"المبلغ المدفوع ({totalPaid:N2}) أكبر من الإجمالي ({totalAmount:N2}). هل تريد المتابعة؟",
                "تنبيه — دفعة زائدة", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                txtPaid.Focus();
                return false;
            }
        }

        if (errors.Count > 0)
        {
            MessageBox.Show(string.Join("\n", errors), "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        errorProvider.Clear();

        if (!_ValidateForm()) return;

        if (post && MessageBox.Show("هل أنت متأكد من ترحيل الفاتورة؟ لا يمكن تعديلها بعد الترحيل.", "تأكيد الترحيل",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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

                return new CreateSalesInvoiceLineRequest(
                    l.ProductId,
                    l.Quantity,
                    l.UnitPrice,
                    productUnitId,
                    SalesSystem.DTO.Enums.DiscountType.Amount,
                    null);
            }).ToList();

            if (items.Count == 0)
            {
                MessageBox.Show("لا توجد منتجات صالحة للحفظ.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal subTotal = _lines.Sum(x => x.LineTotal);
            var discountType = cmbDiscountType.SelectedIndex == 1
                ? SalesSystem.DTO.Enums.DiscountType.Percentage
                : SalesSystem.DTO.Enums.DiscountType.Amount;

            decimal discountValue = numInvoiceDiscount.Value;
            decimal calculatedDiscountAmt = discountType == SalesSystem.DTO.Enums.DiscountType.Amount
                ? discountValue
                : Math.Min(subTotal * (discountValue / 100m), subTotal);

            decimal taxableAmount = subTotal - calculatedDiscountAmt;
            decimal reqTaxAmount = CalculateTaxAmount(taxableAmount);

            // ── Shared header values (used by both create & update) ──
            int warehouseId = Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0);
            int customerId = Convert.ToInt32(cmbCustomer.SelectedValue ?? 0);
            var paymentType = (SalesSystem.DTO.Enums.PaymentType)(cmbPaymentType.SelectedIndex + 1);
            decimal paidAmount = txtPaid.DecimalValue + txtBankAmount.DecimalValue;
            decimal expenses = decimal.TryParse(txtExpenses.Text, out decimal exp2) ? exp2 : 0m;
            string notes = txtNotes.Text;
            int? taxId = chkApplyTax.Checked && cmbTax.SelectedValue is int tid ? tid : null;

            // ── Create or Update? ──
            int invoiceId;
            bool isUpdate = _invoice != null && _invoice.Id > 0;

            if (isUpdate)
            {
                var updateRequest = new UpdateSalesInvoiceRequest(
                    WarehouseId: warehouseId,
                    CustomerId: customerId,
                    InvoiceDate: dtpDate.Value,
                    PaymentType: paymentType,
                    DiscountAmount: calculatedDiscountAmt,
                    TaxAmount: reqTaxAmount,
                    OtherCharges: expenses,
                    PaidAmount: paidAmount,
                    CashBoxId: null,
                    Notes: notes,
                    TaxId: taxId,
                    Items: items,
                    DiscountType: discountType,
                    DiscountRate: discountValue);

                var updateResult = await _salesService.UpdateAsync(_invoice.Id, updateRequest, SessionManager.UserId, default);
                if (!updateResult.IsSuccess)
                {
                    MessageBox.Show(updateResult.Error ?? "فشل تعديل الفاتورة", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                invoiceId = _invoice.Id;
                _invoice = updateResult.Value;
            }
            else
            {
                var createRequest = new CreateSalesInvoiceRequest(
                    WarehouseId: warehouseId,
                    InvoiceNo: null,
                    CustomerId: customerId,
                    CashBoxId: null,
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

                var createResult = await _salesService.CreateAsync(createRequest, SessionManager.UserId, default);
                if (!createResult.IsSuccess)
                {
                    MessageBox.Show(createResult.Error ?? "فشل إنشاء الفاتورة", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show(isUpdate ? "تم تعديل وترحيل الفاتورة بنجاح" : "تم إنشاء وترحيل الفاتورة بنجاح", "نجاح",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var postResult = await _salesService.PostAsync(invoiceId, SessionManager.UserId, default);
                    if (!postResult.IsSuccess)
                    {
                        MessageBox.Show($"تم الحفظ كمسودة ولكن فشل الترحيل: {postResult.Error}", "تنبيه",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Refresh state from server — the invoice may still be a draft
                        var fresh = await _salesService.GetByIdAsync(invoiceId, default);
                        if (fresh.IsSuccess) _invoice = fresh.Value;
                    }
                    else
                    {
                        _invoice = postResult.Value ?? _invoice;
                        MessageBox.Show(isUpdate ? "تم تعديل وترحيل الفاتورة بنجاح" : "تم إنشاء وترحيل الفاتورة بنجاح", "نجاح",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show(isUpdate ? "تم تعديل الفاتورة بنجاح" : "تم الحفظ كمسودة بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Update UI — stay open for further editing
            lblInvoiceNo.Text = _invoice.InvoiceNo.ToString();
            lblMode.Text = $"فاتورة بيع - {_invoice.InvoiceNo}";
            UpdateUIState();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ أثناء حفظ الفاتورة");
            MessageBox.Show("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل في السجل.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSaveDraft.Enabled = btnPost.Enabled = true;
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
    /// Computes the invoice total the same way as CalculateTotals —
    /// shared by totals display and validation so thresholds always match.
    /// </summary>
    private decimal ComputeTotalAmount()
    {
        decimal subTotal = _lines.Where(l => l.ProductId > 0).Sum(x => x.LineTotal);
        decimal invoiceDiscount = cmbDiscountType.SelectedIndex == 1
            ? subTotal * (numInvoiceDiscount.Value / 100m)
            : numInvoiceDiscount.Value;
        invoiceDiscount = Math.Min(invoiceDiscount, subTotal);

        decimal taxableAmount = subTotal - invoiceDiscount;
        decimal taxAmount = CalculateTaxAmount(taxableAmount);
        bool isExclusive = !chkApplyTax.Checked; // مضافة = tax added on top
        decimal expenses = decimal.TryParse(txtExpenses.Text, out decimal exp) ? exp : 0m;

        return taxableAmount + (isExclusive ? taxAmount : 0m) + expenses;
    }

    private void CalculateTotals()
    {
        if (_isUpdating) return;

        decimal subTotal = _lines.Sum(x => x.LineTotal);
        decimal invoiceDiscount = 0m;

        if (cmbDiscountType.SelectedIndex == 0) // Amount
        {
            invoiceDiscount = numInvoiceDiscount.Value;
        }
        else if (cmbDiscountType.SelectedIndex == 1) // Percentage
        {
            invoiceDiscount = subTotal * (numInvoiceDiscount.Value / 100m);
        }

        invoiceDiscount = Math.Min(invoiceDiscount, subTotal);

        decimal paidAmountCash = txtPaid.DecimalValue;
        decimal paidAmountBank = txtBankAmount.DecimalValue;
        decimal totalPaid = paidAmountCash + paidAmountBank;

        // Get tax rate from selected tax
        decimal taxableAmount = subTotal - invoiceDiscount;
        decimal taxAmount = CalculateTaxAmount(taxableAmount);
        bool isInclusive = chkApplyTax.Checked;  // شاملة = tax embedded, cmbTax disabled
        bool isExclusive = !chkApplyTax.Checked; // مضافة = tax added on top, cmbTax enabled

        decimal expenses = decimal.TryParse(txtExpenses.Text, out decimal exp) ? exp : 0m;
        decimal totalAmount = taxableAmount + (isExclusive ? taxAmount : 0m) + expenses;
        decimal dueAmount = totalAmount - totalPaid;

        lblSubTotalVal.Text = subTotal.ToString("N2");
        lblDiscountAmountVal.Text = invoiceDiscount.ToString("N2");
        lblTotalVal.Text = totalAmount.ToString("N2");
        lblDueVal.Text = dueAmount.ToString("N2");

        lblDueVal.ForeColor = dueAmount > 0 ? Color.Red : Color.Green;

        // Update quick info bar
        int itemCount = _lines.Count(l => l.ProductId > 0);
        lblQuickInfo.Text = $"عدد الأصناف: {itemCount} | الإجمالي: {totalAmount:N2} | المدفوع: {totalPaid:N2} | المتبقي: {dueAmount:N2}";
    }

    private decimal CalculateTaxAmount(decimal subTotal)
    {
        if (cmbTax.SelectedValue is not int taxId) return 0m;

        var tax = _taxes.FirstOrDefault(t => t.Id == taxId);
        if (tax == null || tax.Rate <= 0) return 0m;

        decimal rate = tax.Rate / 100m;

        if (chkApplyTax.Checked)
        {
            // شاملة (Inclusive): Tax is embedded in the amount
            // TaxAmount = SubTotal - (SubTotal / (1 + rate))
            return subTotal - (subTotal / (1m + rate));
        }
        else
        {
            // مضافة (Exclusive): Tax is added on top
            // TaxAmount = SubTotal * rate
            return subTotal * rate;
        }
    }

    private void UpdateUIState()
    {
        bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;

        gbInvoiceInfo.Enabled = editable;
        pnlEntry.Enabled = editable;
        gbTotals.Enabled = editable;

        btnSaveDraft.Visible = editable;
        btnPost.Visible = editable;
        btnCancelInvoice.Visible = _invoice != null && _invoice.Status == (byte)InvoiceStatus.Posted;

        lblStatus.Text = _invoice?.Status switch
        {
            (byte)InvoiceStatus.Draft => "مسودة",
            (byte)InvoiceStatus.Posted => "تم الترحيل",
            (byte)InvoiceStatus.Cancelled => "ملغي",
            _ => "جديد"
        };

        lblStatus.BackColor = _invoice?.Status switch
        {
            (byte)InvoiceStatus.Draft => Color.Gray,
            (byte)InvoiceStatus.Posted => Color.Green,
            (byte)InvoiceStatus.Cancelled => Color.Red,
            _ => Color.Blue
        };

        if (btnPrintA4 != null) btnPrintA4.Visible = _invoice != null;
        if (btnPrintReceipt != null) btnPrintReceipt.Visible = _invoice != null;
    }

    // ═════════════════════════════════════════════════════════════════
    //  VALIDATION HELPERS (PRESERVED)
    // ═════════════════════════════════════════════════════════════════

    private void _ValidateEmptyTextBox(object sender, CancelEventArgs e)
    {
        TextBox Temp = ((TextBox)sender);
        if (string.IsNullOrEmpty(Temp.Text.Trim()))
        {
            e.Cancel = true;
            errorProvider.SetError(Temp, "هذا الحقل مطلوب");
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
        int idx = cmbPaymentType.SelectedIndex;
        bool isCredit = idx == 1;   // آجل
        bool isCash = idx == 0;     // نقدي
        bool isMixed = idx == 2;    // مختلط

        txtPaid.Enabled = !isCredit;
        txtBankAmount.Enabled = isMixed;
        cmbBank.Enabled = isMixed;

        if (isCredit)
        {
            txtPaid.Text = "0";
            txtBankAmount.Text = "0";
        }
        else if (isCash)
        {
            txtBankAmount.Text = "0";
        }
    }

    private void pnlButtons_Paint(object sender, PaintEventArgs e)
    {

    }

    private void chkApplyTax_CheckedChanged(object? sender, EventArgs e)
    {
        cmbTax.Enabled = !chkApplyTax.Checked;
        CalculateTotals();
    }

    private void lblDueVal_Click(object sender, EventArgs e)
    {

    }
}

