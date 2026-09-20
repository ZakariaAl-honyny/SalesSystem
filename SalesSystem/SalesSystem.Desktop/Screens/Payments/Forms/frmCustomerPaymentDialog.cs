using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Screens.Payments;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Payments.Forms;

public partial class frmCustomerPaymentDialog : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ICustomerPaymentApiService PaymentApi => _serviceProvider.GetRequiredService<ICustomerPaymentApiService>();
    private ICustomerApiService CustomerApi => _serviceProvider.GetRequiredService<ICustomerApiService>();
    private ISalesInvoiceApiService SalesApi => _serviceProvider.GetRequiredService<ISalesInvoiceApiService>();
    private INotificationService Notification => _serviceProvider.GetRequiredService<INotificationService>();
    private IEventBus EventBus => _serviceProvider.GetRequiredService<IEventBus>();
    
    private List<CustomerDto> _customers = new();
    private decimal _currentBalance = 0;

    public frmCustomerPaymentDialog(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        
        btnSave.Click += btnSave_Click;
        btnCancel.Click += (s, e) => this.Close();
        cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        try
        {
            var res = await CustomerApi.GetAllAsync();
            if (res.IsSuccess)
            {
                _customers = res.Value?.ToList() ?? new List<CustomerDto>();
                cmbCustomer.DataSource = _customers;
                cmbCustomer.DisplayMember = "Name";
                cmbCustomer.ValueMember = "Id";
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل قائمة العملاء في سند قبض");
            Notification.ShowError("حدث خطأ في تحميل البيانات.");
        }
    }

    private async void cmbCustomer_SelectedIndexChanged(object? sender, EventArgs e) => await OnCustomerChanged();

    private async Task OnCustomerChanged()
    {
        try
        {
            if (cmbCustomer.SelectedValue is not int customerId) return;
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                _currentBalance = customer.CurrentBalance;
                lblBalanceVal.Text = _currentBalance.ToString("N2");
                lblBalanceVal.ForeColor = _currentBalance > 0 ? Color.Red : Color.Blue;
            }

            // Load unpaid invoices
            var invoicesRes = await SalesApi.GetAllAsync(status: 2); // Posted only
            if (invoicesRes.IsSuccess)
            {
                var list = invoicesRes.Value.Where(i => i.CustomerId == customerId && i.DueAmount > 0).ToList();
                cmbInvoice.DataSource = list;
                cmbInvoice.DisplayMember = "InvoiceNo";
                cmbInvoice.ValueMember = "Id";
                cmbInvoice.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تغيير العميل في سند قبض");
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbCustomer.SelectedValue is not int)
        {
            errorProvider.SetError(cmbCustomer, "يرجى اختيار العميل.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCustomer, "");
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

        int customerId = (int)cmbCustomer.SelectedValue;
        decimal amount = numAmount.Value;

        if (amount > _currentBalance)
        {
            if (MessageBox.Show("المبلغ المدفوع أكبر من الرصيد. هل تريد المتابعة؟", "تنبيه", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
        }

        try
        {
            int? invoiceId = (int?)cmbInvoice.SelectedValue;
            var request = new CreateCustomerPaymentRequest(customerId, amount, SalesSystem.DTO.Enums.PaymentType.Cash, dtpDate.Value, invoiceId, txtNotes.Text);

            var res = await PaymentApi.CreateAsync(request);
            if (res.IsSuccess)
            {
                Notification.ShowSuccess("تم حفظ السداد بنجاح");
                EventBus.Publish(new CustomerPaymentChangedMessage(res.Value.Id));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else Notification.ShowError(res.Error!);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في حفظ سند القبض");
            Notification.ShowError("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.");
        }
    }
}
