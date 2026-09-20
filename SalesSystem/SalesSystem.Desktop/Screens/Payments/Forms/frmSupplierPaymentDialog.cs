using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SalesSystem.DTO;
using SalesSystem.Common.Enums;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Screens.Payments;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms;

public partial class frmSupplierPaymentDialog : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISupplierPaymentApiService PaymentApi => _serviceProvider.GetRequiredService<ISupplierPaymentApiService>();
    private ISupplierApiService SupplierApi => _serviceProvider.GetRequiredService<ISupplierApiService>();
    private IPurchaseInvoiceApiService PurchaseApi => _serviceProvider.GetRequiredService<IPurchaseInvoiceApiService>();
    private INotificationService Notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();
    
    private List<SupplierDto> _suppliers = new();
    private decimal _currentBalance = 0;

    public frmSupplierPaymentDialog(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        
        btnSave.Click += btnSave_Click;
        btnCancel.Click += (s, e) => this.Close();
        cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        try
        {
            var res = await SupplierApi.GetAllAsync();
            if (res.IsSuccess)
            {
                _suppliers = res.Value?.ToList() ?? new List<SupplierDto>();
                cmbSupplier.DataSource = _suppliers;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.ValueMember = "Id";
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل قائمة الموردين في سند صرف");
            Notification.ShowError("حدث خطأ في تحميل البيانات.");
        }
    }

    private async void cmbSupplier_SelectedIndexChanged(object? sender, EventArgs e) => await OnSupplierChanged();

    private async Task OnSupplierChanged()
    {
        try
        {
            if (cmbSupplier.SelectedValue is not int supplierId) return;
            var supplier = _suppliers.FirstOrDefault(s => s.Id == supplierId);
            if (supplier != null)
            {
                _currentBalance = supplier.CurrentBalance;
                lblBalanceVal.Text = _currentBalance.ToString("N2");
                lblBalanceVal.ForeColor = _currentBalance > 0 ? Color.Red : Color.Blue;
            }

            // Load unpaid invoices
            var invoicesRes = await PurchaseApi.GetAllAsync(status: 2);
            if (invoicesRes.IsSuccess)
            {
                var list = invoicesRes.Value.Where(i => i.SupplierId == supplierId && i.DueAmount > 0).ToList();
                cmbInvoice.DataSource = list;
                cmbInvoice.DisplayMember = "InvoiceNo";
                cmbInvoice.ValueMember = "Id";
                cmbInvoice.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تغيير المورد في سند صرف");
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbSupplier.SelectedValue is not int)
        {
            errorProvider.SetError(cmbSupplier, "يرجى اختيار المورد.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbSupplier, "");
        }

        if (numAmount.Value <= 0)
        {
            errorProvider.SetError(numAmount, "المبلغ يجب أن يكون أكبر من صفر.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(numAmount, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private async void btnSave_Click(object? sender, EventArgs e) => await SaveAsync();

    private async Task SaveAsync()
    {
        if (!_ValidateForm()) return;

        int supplierId = (int)cmbSupplier.SelectedValue;
        decimal amount = numAmount.Value;

        if (amount > _currentBalance)
        {
            if (MessageBox.Show("المبلغ المدفوع أكبر من الرصيد. هل تريد المتابعة؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
        }

        try
        {
            int? invoiceId = (int?)cmbInvoice.SelectedValue;
            var request = new CreateSupplierPaymentRequest(supplierId, amount, SalesSystem.DTO.Enums.PaymentMethod.Cash, dtpDate.Value, invoiceId, null, txtNotes.Text);

            var res = await PaymentApi.CreateAsync(request);
            if (res.IsSuccess)
            {
                Notification.ShowSuccess("تم حفظ السداد بنجاح");
                EventBus.Publish(new SupplierPaymentChangedMessage(res.Value.Id));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else Notification.ShowError(res.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ سند الصرف");
            Notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
        }
    }
}
