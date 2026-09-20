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
using SalesSystem.Desktop.Screens.Returns;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

public partial class frmSalesReturn : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISalesReturnApiService _returnApi => _serviceProvider.GetRequiredService<ISalesReturnApiService>();
    private ICustomerApiService _customerApi => _serviceProvider.GetRequiredService<ICustomerApiService>();
    private IProductApiService _productApi => _serviceProvider.GetRequiredService<IProductApiService>();
    private IWarehouseApiService _warehouseApi => _serviceProvider.GetRequiredService<IWarehouseApiService>();
    private INotificationService _notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus _eventBus => _serviceProvider.GetRequiredService<IEventBus>();
    
    private SalesReturnDto? _invoice;
    private BindingList<InvoiceLineItemViewModel> _lines = new();
    private List<ProductDto> _allProducts = new();
    private bool _isUpdating = false;

    public frmSalesReturn(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;
        SetupGrid();
    }

    public void LoadData(SalesReturnDto? invoice)
    {
        _invoice = invoice;
        if (_invoice != null) BindInvoice();
        else ResetForm();
        UpdateUIState();
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            await LoadLookups();
            if (_invoice == null) ResetForm();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل النموذج");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadLookups()
    {
        try
        {
            var warehouseRes = await _warehouseApi.GetAllAsync();
            if (warehouseRes.IsSuccess)
            {
                cmbWarehouse.DataSource = warehouseRes.Value;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "Id";
            }

            var customerRes = await _customerApi.GetAllAsync();
            if (customerRes.IsSuccess)
            {
                var list = customerRes.Value!.ToList();
                list.Insert(0, new CustomerDto(0, "عميل نقدي", "", "", "", "", 0, true, 0));
                cmbCustomer.DataSource = list;
                cmbCustomer.DisplayMember = "Name";
                cmbCustomer.ValueMember = "Id";
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
        this.Text = $"مرتجع مبيعات - {_invoice!.ReturnNo}";
        lblInvoiceNo.Text = _invoice.ReturnNo.ToString();
        dtpDate.Value = _invoice.ReturnDate;
        cmbCustomer.SelectedValue = _invoice.CustomerId;
        cmbWarehouse.SelectedValue = _invoice.WarehouseId;
        txtNotes.Text = _invoice.Notes;

        _lines.Clear();
        foreach (var item in _invoice.Items)
        {
            _lines.Add(new InvoiceLineItemViewModel {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountAmount = item.DiscountAmount
            });
        }
        _isUpdating = false;
        CalculateTotals();
    }

    private void ResetForm()
    {
        this.Text = "مرتجع مبيعات جديد";
        lblInvoiceNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        _lines.Clear();
        txtNotes.Clear();
    }

    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 250, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 80 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "UnitPrice", Width = 100 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "LineTotal", Width = 100, ReadOnly = true });
        dgvItems.DataSource = _lines;
        dgvItems.CellEndEdit += (s, e) => {
            if (_isUpdating || e.RowIndex < 0) return;
            var row = _lines[e.RowIndex];
            if (dgvItems.Columns[e.ColumnIndex].Name == "ColCode") {
                var p = _allProducts.FirstOrDefault(x => x.Barcode != null && x.Barcode == row.ProductCode);
                if (p != null) { row.ProductId = p.Id; row.ProductName = p.Name; if (row.Quantity == 0) row.Quantity = 1; }
            }
            CalculateTotals();
            dgvItems.Refresh();
        };
    }

    private void CalculateTotals()
    {
        decimal total = _lines.Sum(x => x.LineTotal);
        lblTotalVal.Text = total.ToString("N2");
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
        lblStatus.Text = _invoice?.Status switch { 1 => "مسودة", 2 => "مرحل", 3 => "ملغي", _ => "جديد" };
    }

    private void btnAddItem_Click(object sender, EventArgs e) { _lines.Add(new InvoiceLineItemViewModel()); dgvItems.Focus(); }
    private void btnRemoveItem_Click(object sender, EventArgs e) { if (dgvItems.CurrentRow?.DataBoundItem is InvoiceLineItemViewModel line) { _lines.Remove(line); CalculateTotals(); } }

    private async void btnSaveDraft_Click(object sender, EventArgs e) => await SaveInvoiceAsync(false);
    private async void btnPost_Click(object sender, EventArgs e) => await SaveInvoiceAsync(true);

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbCustomer.SelectedValue == null || (int)cmbCustomer.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbCustomer, "يرجى اختيار العميل");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCustomer, "");
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
            errorProvider.SetError(dgvItems, "يجب إضافة صنف واحد على الأقل للإرجاع");
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

        if (post && MessageBox.Show("ترحيل المرتجع؟ سيتم زيادة المخزون وتقليل رصيد العميل.", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        try {
            var items = _lines.Where(l => l.ProductId > 0).Select(l => new ReturnItemRequest(
                0, l.ProductId, 1, l.Quantity, l.UnitPrice, l.LineTotal, l.DiscountAmount, 1, null)).ToList();
            int customerIdVal = Convert.ToInt32(cmbCustomer.SelectedValue ?? 0);
            short warehouseIdVal = Convert.ToInt16(cmbWarehouse.SelectedValue ?? 0);
            var request = new CreateSalesReturnRequest(
                null,
                customerIdVal > 0 ? customerIdVal : null,
                warehouseIdVal,
                dtpDate.Value,
                null,
                txtNotes.Text,
                items);
            
            var result = await _returnApi.CreateAsync(request);
            if (result.IsSuccess) {
                if (post) {
                    var postRes = await _returnApi.PostAsync(result.Value.Id);
                    if (postRes.IsSuccess) { _notification.ShowSuccess("تم الترحيل"); foreach (var item in items) _eventBus.Publish(new StockChangedMessage(item.ProductId)); }
                    else _notification.ShowError(postRes.Error!);
                } else _notification.ShowSuccess("تم الحفظ");
                _eventBus.Publish(new SalesReturnChangedMessage(result.Value.Id));
                this.DialogResult = DialogResult.OK; this.Close();
            } else _notification.ShowError(result.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ مرتجع المبيعات");
            _notification.ShowError("خطأ في حفظ المرتجع. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();
}
