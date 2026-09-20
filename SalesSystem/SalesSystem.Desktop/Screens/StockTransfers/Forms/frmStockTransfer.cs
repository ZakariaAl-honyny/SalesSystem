using Serilog;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Common.Enums;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Messaging.Messages;
using System.ComponentModel;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.StockTransfers.Forms;

public partial class frmStockTransfer : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IStockTransferApiService TransferApi => _serviceProvider.GetRequiredService<IStockTransferApiService>();
    private IProductApiService ProductApi => _serviceProvider.GetRequiredService<IProductApiService>();
    private IWarehouseApiService WarehouseApi => _serviceProvider.GetRequiredService<IWarehouseApiService>();
    private INotificationService Notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();
    
    private StockTransferDto? _transfer;
    private BindingList<TransferLineItemViewModel> _lines = new();
    private List<ProductDto> _allProducts = new();
    private bool _isUpdating = false;

    public frmStockTransfer(IServiceProvider serviceProvider)
    {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            SetupGrid();
    }

    public void LoadData(StockTransferDto? transfer)
    {
        _transfer = transfer;
        if (_transfer != null) BindTransfer();
        else ResetForm();
        UpdateUIState();
    }

    protected override async void OnLoad(EventArgs e)
    {
        try
        {
            base.OnLoad(e);
            await LoadLookups();
            if (_transfer == null) ResetForm();
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
            var warehouseRes = await WarehouseApi.GetAllAsync();
            if (warehouseRes.IsSuccess)
            {
                var list1 = warehouseRes.Value!.ToList();
                var list2 = warehouseRes.Value!.ToList();
                
                cmbSource.DataSource = list1;
                cmbSource.DisplayMember = "Name";
                cmbSource.ValueMember = "Id";

                cmbDest.DataSource = list2;
                cmbDest.DisplayMember = "Name";
                cmbDest.ValueMember = "Id";

                if (_transfer == null && list1.Count > 1) cmbDest.SelectedIndex = 1;
            }

            var productRes = await ProductApi.GetAllAsync();
            if (productRes.IsSuccess) _allProducts = productRes.Value!.ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل البيانات");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BindTransfer()
    {
        _isUpdating = true;
        this.Text = $"تحويل مخزني - {_transfer!.TransferNo}";
        lblTransferNo.Text = _transfer.TransferNo;
        dtpDate.Value = _transfer.TransferDate;
        cmbSource.SelectedValue = _transfer.FromWarehouseId;
        cmbDest.SelectedValue = _transfer.ToWarehouseId;
        txtNotes.Text = _transfer.Notes;

        _lines.Clear();
        foreach (var item in _transfer.Items)
        {
            _lines.Add(new TransferLineItemViewModel {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                Quantity = item.Quantity
            });
        }
        _isUpdating = false;
    }

    private void ResetForm()
    {
        this.Text = "تحويل مخزني جديد";
        lblTransferNo.Text = "جديد";
        dtpDate.Value = DateTime.Now;
        _lines.Clear();
        txtNotes.Clear();
    }

    private void SetupGrid()
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCode", HeaderText = "الكود/الباركود", DataPropertyName = "ProductCode", Width = 150 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "ProductName", Width = 300, ReadOnly = true });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", DataPropertyName = "Quantity", Width = 100 });
        dgvItems.DataSource = _lines;
        dgvItems.CellEndEdit += (s, e) => {
            if (_isUpdating || e.RowIndex < 0) return;
            var row = _lines[e.RowIndex];
            if (dgvItems.Columns[e.ColumnIndex].Name == "ColCode") {
                var p = _allProducts.FirstOrDefault(x => x.Barcode == row.ProductCode);
                if (p != null) { row.ProductId = p.Id; row.ProductName = p.Name; if (row.Quantity == 0) row.Quantity = 1; }
            }
            dgvItems.Refresh();
        };
    }

    private void UpdateUIState()
    {
        bool editable = _transfer == null || _transfer.Status == (byte)InvoiceStatus.Draft;
        pnlHeader.Enabled = editable;
        dgvItems.ReadOnly = !editable;
        pnlFooter.Enabled = editable;
        btnAddItem.Enabled = editable;
        btnRemoveItem.Enabled = editable;
        btnSaveDraft.Visible = editable;
        btnPost.Visible = editable;
        lblStatus.Text = _transfer?.Status switch { 1 => "مسودة", 2 => "مرحل", 3 => "ملغي", _ => "جديد" };
    }

    private void btnAddItem_Click(object sender, EventArgs e) { _lines.Add(new TransferLineItemViewModel()); dgvItems.Focus(); }
    private void btnRemoveItem_Click(object sender, EventArgs e) { if (dgvItems.CurrentRow?.DataBoundItem is TransferLineItemViewModel line) _lines.Remove(line); }

    private async void btnSaveDraft_Click(object sender, EventArgs e) => await SaveTransferAsync(false);
    private async void btnPost_Click(object sender, EventArgs e) => await SaveTransferAsync(true);

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbSource.SelectedValue == null || (int)cmbSource.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbSource, "يرجى اختيار المستودع المصدر");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbSource, "");
        }

        if (cmbDest.SelectedValue == null || (int)cmbDest.SelectedValue <= 0)
        {
            errorProvider.SetError(cmbDest, "يرجى اختيار المستودع الهدف");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbDest, "");
        }

        if (cmbSource.SelectedValue != null && cmbDest.SelectedValue != null &&
            (int)cmbSource.SelectedValue == (int)cmbDest.SelectedValue)
        {
            errorProvider.SetError(cmbDest, "لا يمكن التحويل لنفس المستودع");
            isValid = false;
        }
        else if (cmbDest.SelectedValue != null)
        {
            errorProvider.SetError(cmbDest, "");
        }

        if (!_lines.Any(l => l.ProductId > 0))
        {
            errorProvider.SetError(dgvItems, "يجب إضافة صنف واحد على الأقل");
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

    private async Task SaveTransferAsync(bool post)
    {
        if (!_ValidateForm()) return;

        int srcId = (int)cmbSource.SelectedValue;
        int dstId = (int)cmbDest.SelectedValue;
        if (post && MessageBox.Show("ترحيل التحويل؟ سيتم خصم الكميات من المصدر وإضافتها للوجهة.", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        try {
            var items = _lines.Where(l => l.ProductId > 0).Select(l => new CreateStockTransferItemRequest(l.ProductId, l.Quantity, null)).ToList();
            var request = new CreateStockTransferRequest(srcId, dstId, dtpDate.Value, txtNotes.Text, items);
            
            var result = await TransferApi.CreateAsync(request);
            if (result.IsSuccess) {
                if (post) {
                    var postRes = await TransferApi.PostAsync(result.Value.Id);
                    if (postRes.IsSuccess) { Notification.ShowSuccess("تم الترحيل"); foreach (var item in items) EventBus.Publish(new StockChangedMessage(item.ProductId)); }
                } else Notification.ShowSuccess("تم الحفظ");
                EventBus.Publish(new StockTransferChangedMessage(result.Value.Id));
                this.DialogResult = DialogResult.OK; this.Close();
            } else Notification.ShowError(result.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ التحويل المخزني");
            Notification.ShowError("خطأ في حفظ التحويل. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    public class TransferLineItemViewModel : INotifyPropertyChanged
    {
        public int ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        private decimal _quantity;
        public decimal Quantity { get => _quantity; set { _quantity = value; OnPropertyChanged(nameof(Quantity)); } }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
