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
using SalesSystem.Desktop.Screens.Purchases;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

public partial class frmPurchaseInvoice : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IPurchaseInvoiceApiService _purchaseApi => _serviceProvider.GetRequiredService<IPurchaseInvoiceApiService>();
    private ISupplierApiService _supplierApi => _serviceProvider.GetRequiredService<ISupplierApiService>();
    private IProductApiService _productApi => _serviceProvider.GetRequiredService<IProductApiService>();
    private IWarehouseApiService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseApiService>();
    private INotificationService _notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();
    private IInvoicePrinter _invoicePrinter => _serviceProvider.GetRequiredService<IInvoicePrinter>();
    private ISettingsApiService _settingsApi => _serviceProvider.GetRequiredService<ISettingsApiService>();
    
    private PurchaseInvoiceDto? _invoice;
    private BindingList<InvoiceLineItemViewModel> _lines = new();
    private List<ProductDto> _allProducts = new();
    private bool _isUpdating = false;
    private Button btnPrintA4 = null!;

    public frmPurchaseInvoice(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
        ApplyStyles();
        SetupGrid();
    }

    public void LoadData(PurchaseInvoiceDto? invoice)
    {
        _invoice = invoice;
        ApplyStyles();
    }

    private void ApplyStyles()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        this.Text = _invoice == null ? "فاتورة مشتريات جديدة" : $"فاتورة مشتريات - {_invoice.InvoiceNo}";
        
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

            if (_invoice != null) BindInvoice();
            else ResetForm();
            
            UpdateUIState();
            CalculateTotals();
            SetupPrintingButtons();
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
        
        if (pnlFooter != null) pnlFooter.Controls.Add(btnPrintA4);
        else this.Controls.Add(btnPrintA4);
    }

    private async Task LoadLookups()
    {
        try
        {
            var supplierRes = await _supplierApi.GetAllAsync();
            if (supplierRes.IsSuccess)
            {
                cmbSupplier.DataSource = supplierRes.Value;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.ValueMember = "Id";
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
            if (productRes.IsSuccess) _allProducts = productRes.Value!.ToList();
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
        this.Text = $"فاتورة مشتريات - {_invoice!.InvoiceNo}";
        lblInvoiceNo.Text = _invoice.InvoiceNo.ToString();
        dtpDate.Value = _invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue);
        cmbSupplier.SelectedValue = _invoice.SupplierId;
        cmbWarehouse.SelectedValue = _invoice.WarehouseId;
        cmbPaymentType.SelectedIndex = (byte)_invoice.PaymentType - 1;
        numInvoiceDiscount.Value = _invoice.DiscountAmount;
        numTaxRate.Value = 15;
        txtPaid.Text = _invoice.PaidAmount.ToString("N2");
        txtNotes.Text = _invoice.Notes;

        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItemViewModel {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitCost = item.UnitCost,
                DiscountAmount = item.DiscountAmount
            });
        }
        _isUpdating = false;
    }

    private void ResetForm()
    {
        this.Text = "فاتورة مشتريات جديدة";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        cmbPaymentType.SelectedIndex = 0;
        _lines.Clear();
        numInvoiceDiscount.Value = 0;
        txtPaid.Text = "0";
        txtNotes.Clear();
    }

    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Clear();
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 250, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCost", HeaderText = "التكلفة", DataPropertyName = "UnitCost", Width = 100 });
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
                row.UnitCost = product.PurchasePrice;
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
        bool isTaxInclusive = chkTaxInclusive.Checked;
        decimal invoiceDiscount = numInvoiceDiscount.Value;
        decimal paidAmount = txtPaid.DecimalValue;

        decimal taxAmount = isTaxInclusive ? subTotal - (subTotal / (1 + taxRate / 100m)) : subTotal * (taxRate / 100m);
        decimal totalAmount = subTotal - invoiceDiscount + (isTaxInclusive ? 0 : taxAmount);
        decimal dueAmount = totalAmount - paidAmount;

        lblSubTotalVal.Text = subTotal.ToString("N2");
        lblTaxAmountVal.Text = taxAmount.ToString("N2");
        lblTotalVal.Text = totalAmount.ToString("N2");
        lblDueVal.Text = dueAmount.ToString("N2");
        lblDueVal.ForeColor = dueAmount > 0 ? Color.Red : Color.Green;
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
        
        lblStatus.Text = _invoice?.Status switch { 1 => "مسودة", 2 => "مرحل", 3 => "ملغي", _ => "جديد" };
        lblStatus.BackColor = _invoice?.Status switch { 1 => Color.Gray, 2 => Color.Green, 3 => Color.Red, _ => Color.Blue };
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

        if (cmbSupplier.SelectedValue == null || (int)cmbSupplier.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbSupplier, "يرجى اختيار المورد");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbSupplier, "");
        }

        if (cmbWarehouse.SelectedValue == null || (int)cmbWarehouse.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbWarehouse, "يرجى اختيار المستودع");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbWarehouse, "");
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

        if (post && MessageBox.Show("هل أنت متأكد من ترحيل فاتورة المشتريات؟ سيتم زيادة المخزون.", "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        btnSaveDraft.Enabled = btnPost.Enabled = false;
        try
        {
            var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreatePurchaseInvoiceLineRequest(l.ProductId, 1, l.Quantity, l.UnitCost)).ToList();
            var request = new CreatePurchaseInvoiceRequest(
                (int)(cmbWarehouse.SelectedValue ?? 0),
                (int)(cmbSupplier.SelectedValue ?? 0),
                null,
                dtpDate.Value,
                (SalesSystem.DTO.Enums.PaymentType)(cmbPaymentType.SelectedIndex + 1),
                numInvoiceDiscount.Value,
                null,
                null,
                decimal.Parse(lblTaxAmountVal.Text),
                0m,
                txtPaid.DecimalValue,
                txtNotes.Text,
                null,
                null,
                items
            );

            var result = await _purchaseApi.CreateAsync(request);
            if (result.IsSuccess)
            {
                if (post)
                {
                    var postRes = await _purchaseApi.PostAsync(result.Value.Id);
                    if (postRes.IsSuccess)
                    {
                        _notification.ShowSuccess("تم الترحيل بنجاح");
                        foreach (var item in items) _eventBus.Publish(new StockChangedMessage(item.ProductId));
                    }
                    else _notification.ShowError("تم الحفظ ولكن فشل الترحيل: " + postRes.Error);
                }
                else _notification.ShowSuccess("تم الحفظ بنجاح");

                _eventBus.Publish(new PurchaseInvoiceChangedMessage(result.Value.Id));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else _notification.ShowError(result.Error!);
        }
        catch (Exception ex) 
        { 
            Log.Error(ex, "حدث خطأ أثناء حفظ فاتورة المشتريات");
            _notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني."); 
        }
        finally { btnSaveDraft.Enabled = btnPost.Enabled = true; }
    }

    private async void btnCancelInvoice_Click(object? sender, EventArgs e)
    {
        if (_invoice == null) return;
        if (MessageBox.Show("هل أنت متأكد من إلغاء الفاتورة؟ سيتم عكس المخزون.", "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        var result = await _purchaseApi.CancelAsync(_invoice.Id);
        if (result.IsSuccess)
        {
            _notification.ShowSuccess("تم الإلغاء بنجاح");
            _eventBus.Publish(new PurchaseInvoiceChangedMessage(_invoice.Id));
            foreach (var item in _invoice.Items) _eventBus.Publish(new StockChangedMessage(item.ProductId));
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
            Log.Error(ex, "حدث خطأ أثناء طباعة فاتورة المشتريات A4");
            _notification.ShowError("حدث خطأ أثناء الطباعة. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => this.Close();
}
