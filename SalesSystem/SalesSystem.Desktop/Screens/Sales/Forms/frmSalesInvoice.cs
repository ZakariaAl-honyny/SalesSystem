using Serilog;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Models;
using SalesSystem.Desktop.Messaging.Messages;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Printing;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

public partial class frmSalesInvoice : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISalesInvoiceApiService _salesApi => _serviceProvider.GetRequiredService<ISalesInvoiceApiService>();
    private ICustomerApiService _customerApi => _serviceProvider.GetRequiredService<ICustomerApiService>();
    private IProductApiService _productApi => _serviceProvider.GetRequiredService<IProductApiService>();
    private IWarehouseApiService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseApiService>();
    private INotificationService _notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private IInvoicePrinter _invoicePrinter => _serviceProvider.GetRequiredService<IInvoicePrinter>();
    private IReceiptPrinter _receiptPrinter => _serviceProvider.GetRequiredService<IReceiptPrinter>();
    private ISettingsApiService _settingsApi => _serviceProvider.GetRequiredService<ISettingsApiService>();
    private ITaxesApiService _taxesApi => _serviceProvider.GetRequiredService<ITaxesApiService>();
    
    private SalesInvoiceDto? _invoice;
    private BindingList<InvoiceLineItemViewModel> _lines = new();
    private List<ProductDto> _allProducts = new();
    private List<TaxDto> _taxes = new();
    private Dictionary<int, decimal> _salePriceByProductId = new();
    private Dictionary<int, int> _baseUnitIdByProductId = new();
    private bool _isUpdating = false;
    private Button btnPrintA4 = null!;
    private Button btnPrintReceipt = null!;

    public frmSalesInvoice(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
        ApplyStyles();
        SetupGrid();
        // Ensure print buttons exist before any UI state updates that access them
        SetupPrintingButtons();
    }

    public void LoadData(SalesInvoiceDto? invoice)
    {
        _invoice = invoice;
        ApplyStyles(); // Refresh title
    }

    private void ApplyStyles()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        this.Text = _invoice == null ? "فاتورة مبيعات جديدة" : $"فاتورة مبيعات - {_invoice.InvoiceNo}";
        
        ThemeHelper.ApplyButtonStyle(btnAddItem, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRemoveItem, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnSaveDraft, ThemeHelper.ButtonType.Primary);
        ThemeHelper.ApplyButtonStyle(btnPost, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnCancelInvoice, ThemeHelper.ButtonType.Danger);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            await LoadLookups();

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
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل النموذج");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Printer buttons moved to top with null! initialization

    private void SetupPrintingButtons()
    {
        btnPrintA4 = new Button 
        { 
            Text = "طباعة A4", 
            Width = 100, 
            Height = 30, 
            Location = new Point(10, 10),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left
        };
        btnPrintA4.Click += btnPrintA4_Click;

        btnPrintReceipt = new Button 
        { 
            Text = "طباعة وصل", 
            Width = 100, 
            Height = 30, 
            Location = new Point(115, 10),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left
        };
        btnPrintReceipt.Click += btnPrintReceipt_Click;
        
        if (pnlFooter != null) 
        {
            pnlFooter.Controls.Add(btnPrintA4);
            pnlFooter.Controls.Add(btnPrintReceipt);
        }
        else 
        {
            this.Controls.Add(btnPrintA4);
            this.Controls.Add(btnPrintReceipt);
        }
    }

    private async Task LoadLookups()
    {
        try
        {
            var customersRes = await _customerApi.GetAllAsync();
            if (customersRes.IsSuccess && customersRes.Value != null)
            {
                var list = customersRes.Value.ToList();
                list.Insert(0, new CustomerDto(0, "عميل نقدي", "", "", "", "", 0, true, 0));
                cmbCustomer.DataSource = list;
                cmbCustomer.DisplayMember = "Name";
                cmbCustomer.ValueMember = "Id";
            }

            var warehouseRes = await _warehouseApi.GetAllAsync();
            if (warehouseRes.IsSuccess)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
                
                if (_invoice == null)
                {
                    var def = warehouseRes.Value!.FirstOrDefault(w => w.IsDefault);
                    if (def != null) cmbWarehouse.SelectedValue = def.Id;
                }
            }

            var productRes = await _productApi.GetAllAsync();
            if (productRes.IsSuccess)
            {
                _allProducts = productRes.Value!.ToList();

                // Resolve effective base-unit sale prices for all products
                _salePriceByProductId.Clear();
                _baseUnitIdByProductId.Clear();
                foreach (var p in _allProducts)
                {
                    var unitsRes = await _productApi.GetProductUnitsAsync(p.Id);
                    var baseUnit = unitsRes.Value?.FirstOrDefault(u => u.IsBaseUnit)
                                   ?? unitsRes.Value?.FirstOrDefault();
                    if (baseUnit is null) continue;
                    _baseUnitIdByProductId[p.Id] = baseUnit.Id;

                    var pricesRes = await _productApi.GetProductPricesByUnitAsync(baseUnit.Id);
                    var effective = pricesRes.Value?.FirstOrDefault(pp => pp.IsCurrentlyEffective);
                    if (effective is not null)
                        _salePriceByProductId[p.Id] = effective.Price;
                }
            }

            var taxesRes = await _taxesApi.GetAllAsync();
            if (taxesRes.IsSuccess && taxesRes.Value != null)
            {
                _taxes = taxesRes.Value.ToList();
                cmbTax.DataSource = _taxes;
                cmbTax.DisplayMember = "Name";
                cmbTax.ValueMember = "Id";

                if (_invoice == null)
                {
                    var defaultTax = _taxes.FirstOrDefault(t => t.IsDefault) ?? _taxes.FirstOrDefault();
                    if (defaultTax != null)
                    {
                        cmbTax.SelectedValue = defaultTax.Id;
                        numTaxRate.Value = defaultTax.Rate;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل البيانات");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BindInvoice()
    {
        _isUpdating = true;
        this.Text = $"فاتورة مبيعات - {_invoice!.InvoiceNo}";
        lblInvoiceNo.Text = _invoice.InvoiceNo.ToString();
        dtpDate.Value = _invoice.InvoiceDate;
        cmbCustomer.SelectedValue = _invoice.CustomerId;
        cmbWarehouse.SelectedValue = (short)_invoice.WarehouseId;
        cmbPaymentType.SelectedIndex = (byte)_invoice.PaymentType - 1;
        numInvoiceDiscount.Value = _invoice.DiscountAmount;
        if (_invoice.TaxId.HasValue)
            cmbTax.SelectedValue = _invoice.TaxId.Value;
        numTaxRate.Value = _invoice.TaxRate ?? 15;
        txtPaid.Text = _invoice.PaidAmount.ToString("N2");
        txtNotes.Text = _invoice.Notes;

        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItemViewModel
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount
            });
        }
        _isUpdating = false;
    }

    private void ResetForm()
    {
        this.Text = "فاتورة مبيعات جديدة";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        cmbPaymentType.SelectedIndex = 0;
        _lines.Clear();
        numInvoiceDiscount.Value = 0;
        txtPaid.Text = "0";
        txtNotes.Clear();
        if (_taxes.Count > 0)
        {
            var defaultTax = _taxes.FirstOrDefault(t => t.IsDefault) ?? _taxes.First();
            cmbTax.SelectedValue = defaultTax.Id;
            numTaxRate.Value = defaultTax.Rate;
        }
    }

    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Clear();
        
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 250, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "UnitPrice", Width = 100 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDisc", HeaderText = "خصم", DataPropertyName = "DiscountAmount", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 120, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", ForeColor = Color.Blue, Font = new Font(dgvItems.Font, FontStyle.Bold) } });

        dgvItems.DataSource = _lines;
        dgvItems.CellEndEdit += DgvItems_CellEndEdit;
    }

    private void DgvItems_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (_isUpdating || e.RowIndex < 0) return;

        var row = _lines[e.RowIndex];
        var colName = dgvItems.Columns[e.ColumnIndex].Name;

        if (colName == "ColCode")
        {
            var product = _allProducts.FirstOrDefault(p => p.Barcode == row.ProductCode);
            if (product != null)
            {
                row.ProductId = product.Id;
                row.ProductName = product.Name;
                if (_salePriceByProductId.TryGetValue(product.Id, out var price))
                    row.UnitPrice = price;
                if (_baseUnitIdByProductId.TryGetValue(product.Id, out var unitId))
                    row.ProductUnitId = unitId;
                if (row.Barcode is null) row.Barcode = product.Barcode;
                if (row.Quantity == 0) row.Quantity = 1;
            }
        }

        CalculateTotals();
        dgvItems.Refresh();
    }

    private void CalculateTotals()
    {
        if (_isUpdating) return;

        decimal subTotal = _lines.Sum(x => x.LineTotal);
        decimal taxRate = numTaxRate.Value;
        decimal invoiceDiscount = numInvoiceDiscount.Value;
        decimal paidAmount = txtPaid.DecimalValue;

        // Server canonical formula (tax-exclusive): NetTotal = SubTotal - InvoiceDiscount + TaxAmount + OtherCharges
        decimal taxAmount = subTotal * (taxRate / 100m);
        decimal totalAmount = subTotal - invoiceDiscount + taxAmount;
        decimal dueAmount = totalAmount - paidAmount;

        lblSubTotalVal.Text = subTotal.ToString("N2");
        lblTaxAmountVal.Text = taxAmount.ToString("N2");
        lblTotalVal.Text = totalAmount.ToString("N2");
        lblDueVal.Text = dueAmount.ToString("N2");
        
        lblDueVal.ForeColor = dueAmount > 0 ? Color.Red : Color.Green;
    }

    private void cmbTax_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbTax.SelectedItem is TaxDto tax)
        {
            numTaxRate.Value = tax.Rate;
        }
    }

    private void UpdateUIState()
    {
        bool editable = _invoice == null || _invoice.Status == (byte)InvoiceStatus.Draft;
        
        pnlHeader.Enabled = editable;
        dgvItems.ReadOnly = !editable;
        pnlFooter.Enabled = editable;
        btnAddItem.Enabled = editable;
        btnRemoveItem.Enabled = editable;
        
        btnSaveDraft.Visible = editable;
        btnPost.Visible = editable;
        btnCancelInvoice.Visible = _invoice != null && _invoice.Status == (byte)InvoiceStatus.Posted;
        
        lblStatus.Text = _invoice?.Status switch {
            (byte)InvoiceStatus.Draft => "مسودة",
            (byte)InvoiceStatus.Posted => "مرحل",
            (byte)InvoiceStatus.Cancelled => "ملغي",
            _ => "جديد"
        };
        
        lblStatus.BackColor = _invoice?.Status switch {
            (byte)InvoiceStatus.Draft => Color.Gray,
            (byte)InvoiceStatus.Posted => Color.Green,
            (byte)InvoiceStatus.Cancelled => Color.Red,
            _ => Color.Blue
        };
        
        if (btnPrintA4 != null) btnPrintA4.Visible = _invoice != null;
        if (btnPrintReceipt != null) btnPrintReceipt.Visible = _invoice != null;
    }

    private void btnAddItem_Click(object? sender, EventArgs e)
    {
        _lines.Add(new InvoiceLineItemViewModel());
        dgvItems.Focus();
        dgvItems.CurrentCell = dgvItems.Rows[_lines.Count - 1].Cells[0];
    }

    private void btnRemoveItem_Click(object? sender, EventArgs e)
    {
        if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItemViewModel line)
        {
            _lines.Remove(line);
            CalculateTotals();
        }
    }

    private async void btnSaveDraft_Click(object? sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object? sender, EventArgs e) => await SaveInvoiceAsync(true);

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbCustomer.SelectedValue == null || (int)cmbCustomer.SelectedValue < 0)
        {
            errorProvider.SetError(cmbCustomer, "يرجى اختيار العميل");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCustomer, "");
        }

        if (!_lines.Any(l => l.ProductId > 0))
        {
            errorProvider.SetError(dgvItems, "يجب إضافة صنف واحد على الأقل للفاتورة");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(dgvItems, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private async Task SaveInvoiceAsync(bool post)
    {
        if (!_ValidateForm()) return;

        if (post && MessageBox.Show("هل أنت متأكد من ترحيل الفاتورة؟ لن تتمكن من تعديلها لاحقاً.", "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        try
        {
            var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreateSalesInvoiceLineRequest(l.ProductId, l.Quantity, l.UnitPrice, l.ProductUnitId == 0 ? 1 : l.ProductUnitId)).ToList();
            
            var request = new CreateSalesInvoiceRequest(
                Convert.ToInt32(cmbWarehouse.SelectedValue ?? 0),
                null,
                Convert.ToInt32(cmbCustomer.SelectedValue ?? 0),
                null,
                dtpDate.Value,
                (SalesSystem.DTO.Enums.PaymentType)(cmbPaymentType.SelectedIndex + 1),
                numInvoiceDiscount.Value,
                decimal.Parse(lblTaxAmountVal.Text),
                0m,
                txtPaid.DecimalValue,
                txtNotes.Text,
                cmbTax.SelectedValue is int taxId ? taxId : null,
                items
            );

            Result<SalesInvoiceDto> result;
            if (_invoice?.Id > 0)
                result = await _salesApi.UpdateAsync(_invoice.Id, request);
            else
                result = await _salesApi.CreateAsync(request);
            if (result.IsSuccess && result.Value is not null)
            {
                if (post)
                {
                    var postRes = await _salesApi.PostAsync(result.Value.Id);
                    if (postRes.IsSuccess)
                    {
                        _notification.ShowSuccess("تم الترحيل بنجاح");
                        // Publish StockChangedMessage for each item as requested in T027
                        foreach (var item in items)
                        {
                            _eventBus.Publish(new StockChangedMessage(item.ProductId));
                        }
                    }
                    else _notification.ShowError("تم الحفظ كمسودة ولكن فشل الترحيل: " + postRes.Error);
                }
                else _notification.ShowSuccess("تم الحفظ بنجاح");

                _eventBus.Publish(new SaleInvoiceChangedMessage(result.Value.Id));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else _notification.ShowError(result.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ فاتورة المبيعات");
            _notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            btnSaveDraft.Enabled = btnPost.Enabled = true;
        }
    }

    private async void btnCancelInvoice_Click(object? sender, EventArgs e)
    {
        if (_invoice == null) return;
        if (MessageBox.Show("هل أنت متأكد من إلغاء هذه الفاتورة؟", "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        var result = await _salesApi.CancelAsync(_invoice.Id);
        if (result.IsSuccess)
        {
            _notification.ShowSuccess("تم الإلغاء بنجاح");
            _eventBus.Publish(new SaleInvoiceChangedMessage(_invoice.Id));
            // Publish StockChangedMessage for each item on cancel too
            foreach (var item in _invoice.Items)
            {
                _eventBus.Publish(new StockChangedMessage(item.ProductId));
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else _notification.ShowError(result.Error!);
    }

    private async void btnPrintA4_Click(object? sender, EventArgs e)
    {
        if (_invoice == null) return;

        try
        {
            var settingsRes = await _settingsApi.GetSettingsAsync();
            if (!settingsRes.IsSuccess)
            {
                _notification.ShowError("فشل تحميل إعدادات المتجر: " + settingsRes.Error);
                return;
            }

            var storeInfo = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(settingsRes.Value);
            var printInvoice = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(_invoice);
            var printItems = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDtos(_invoice.Items);
            var printTotals = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToTotalsPrintDto(_invoice);

            _invoicePrinter.PrintPreview(printInvoice, printItems, printTotals, storeInfo);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء طباعة فاتورة A4");
            _notification.ShowError("حدث خطأ أثناء الطباعة. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private async void btnPrintReceipt_Click(object? sender, EventArgs e)
    {
        if (_invoice == null) return;

        try
        {
            var settingsRes = await _settingsApi.GetSettingsAsync();
            if (!settingsRes.IsSuccess)
            {
                _notification.ShowError("فشل تحميل إعدادات المتجر: " + settingsRes.Error);
                return;
            }

            var storeInfo = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(settingsRes.Value);
            var printInvoice = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDto(_invoice);
            var printItems = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToPrintDtos(_invoice.Items);
            var printTotals = SalesSystem.Desktop.Printing.Extensions.PrintDtoExtensions.ToTotalsPrintDto(_invoice);

            _receiptPrinter.PrintPreview(printInvoice, printItems, printTotals, storeInfo);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء طباعة وصل المبيعات");
            _notification.ShowError("حدث خطأ أثناء الطباعة. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => this.Close();
}
