using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.CustomerReceipts.Forms;

public partial class frmAddUpdateCustomerReceipt : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;
    private List<CustomerDto> _customers = new();
    private List<CashBoxDto> _cashBoxes = new();

    public frmAddUpdateCustomerReceipt(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.Text = "سند قبض جديد";
        lblMode.Text = "سند قبض جديد";
        _ = LoadComboBoxesAsync();
    }

    public frmAddUpdateCustomerReceipt(int id, IServiceProvider serviceProvider) : this(serviceProvider)
    {
        _editingId = id;
        this.Text = "تعديل سند قبض";
        lblMode.Text = "تعديل سند قبض";
        _ = LoadReceiptAsync();
    }

    private async Task LoadComboBoxesAsync()
    {
        var customerResult = await _serviceProvider.GetRequiredService<ICustomerApiService>().GetAllAsync();
        if (customerResult.IsSuccess && customerResult.Value != null)
        {
            _customers = customerResult.Value.Where(c => c.IsActive).ToList();
            cmbCustomer.DataSource = _customers.Select(c => new { c.Id, c.Name }).ToList();
            cmbCustomer.DisplayMember = "Name";
            cmbCustomer.ValueMember = "Id";
            cmbCustomer.SelectedIndex = -1;
        }

        var cashResult = await _serviceProvider.GetRequiredService<ICashBoxApiService>().GetAllAsync();
        if (cashResult.IsSuccess && cashResult.Value != null)
        {
            _cashBoxes = cashResult.Value.Where(b => b.IsActive).ToList();
            cmbCashBox.DataSource = _cashBoxes.Select(b => new { b.Id, b.Name }).ToList();
            cmbCashBox.DisplayMember = "Name";
            cmbCashBox.ValueMember = "Id";
            cmbCashBox.SelectedIndex = -1;
        }
    }

    private async Task LoadReceiptAsync()
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>().GetByIdAsync(_editingId);
        Cursor = Cursors.Default;

        if (result.IsSuccess && result.Value != null)
        {
            var r = result.Value;
            cmbCustomer.SelectedValue = r.CustomerId;
            cmbCashBox.SelectedValue = r.CashBoxId;
            numAmount.Value = r.Amount;
            dtpDate.Value = r.ReceiptDate;
            txtNotes.Text = r.Notes ?? "";
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في تحميل بيانات السند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbCustomer.SelectedValue == null)
        {
            errorProvider.SetError(cmbCustomer, "يرجى اختيار العميل.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCustomer, "");
        }

        if (cmbCashBox.SelectedValue == null)
        {
            errorProvider.SetError(cmbCashBox, "يرجى اختيار الصندوق.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbCashBox, "");
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

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            int customerId = (int)cmbCustomer.SelectedValue;
            int cashBoxId = (int)cmbCashBox.SelectedValue;

            if (_isEditMode)
            {
                var request = new UpdateCustomerReceiptRequest(
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>().UpdateAsync(_editingId, request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث سند القبض بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث السند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var request = new CreateCustomerReceiptRequest(
                    CustomerId: customerId,
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>().CreateAsync(request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إضافة سند القبض بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة السند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Sets the editor to Update mode for the given receipt ID.
    /// Called after DI resolution (parameterless constructor).
    /// </summary>
    public void _SetUpdateMode(int id)
    {
        _editingId = id;
        this.Text = "تعديل سند قبض";
        lblMode.Text = "تعديل سند قبض";
        _ = LoadReceiptAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
