using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.PaymentVouchers;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.PaymentVouchers.Forms;

public partial class frmAddUpdatePaymentVoucher : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;
    private List<SupplierDto> _suppliers = new();
    private List<CashBoxDto> _cashBoxes = new();

    public frmAddUpdatePaymentVoucher()
    {
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    public frmAddUpdatePaymentVoucher(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
        this.Text = "سند صرف جديد";
        lblMode.Text = "سند صرف جديد";
        _ = LoadComboBoxesAsync();
    }

    public frmAddUpdatePaymentVoucher(IServiceProvider serviceProvider, int id) : this(serviceProvider)
    {
        _editingId = id;
        this.Text = "تعديل سند صرف";
        lblMode.Text = "تعديل سند صرف";
        _ = LoadVoucherAsync();
    }

    private async Task LoadComboBoxesAsync()
    {
        var supplierResult = await _serviceProvider.GetRequiredService<ISupplierApiService>().GetAllAsync();
        if (supplierResult.IsSuccess && supplierResult.Value != null)
        {
            _suppliers = supplierResult.Value.Where(s => s.IsActive).ToList();
            cmbSupplier.DataSource = _suppliers.Select(s => new { s.Id, s.Name }).ToList();
            cmbSupplier.DisplayMember = "Name";
            cmbSupplier.ValueMember = "Id";
            cmbSupplier.SelectedIndex = -1;
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

    private async Task LoadVoucherAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<IPaymentVoucherApiService>().GetByIdAsync(_editingId);

            if (result.IsSuccess && result.Value != null)
            {
                var v = result.Value;
                cmbCashBox.SelectedValue = v.CashBoxId;
                numAmount.Value = v.TotalAmount;
                dtpDate.Value = v.VoucherDate;
                txtNotes.Text = v.Notes ?? "";
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات السند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء تحميل بيانات السند: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

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
            int cashBoxId = (int)cmbCashBox.SelectedValue;
            int accountId = 0;

            if (_isEditMode)
            {
                var request = new UpdatePaymentVoucherRequest(
                    VoucherDate: dtpDate.Value,
                    Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<IPaymentVoucherApiService>().UpdateAsync(_editingId, request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث سند الصرف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var request = new CreatePaymentVoucherRequest(
                    VoucherDate: dtpDate.Value,
                    CashBoxId: cashBoxId,
                    AccountId: accountId,
                    TotalAmount: numAmount.Value,
                    Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<IPaymentVoucherApiService>().CreateAsync(request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إضافة سند الصرف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة السند", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء حفظ سند الصرف: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Sets the editor to Update mode for the given voucher ID.
    /// Called after DI resolution (parameterless constructor).
    /// </summary>
    public void _SetUpdateMode(int id)
    {
        _editingId = id;
        this.Text = "تعديل سند صرف";
        lblMode.Text = "تعديل سند صرف";
        _ = LoadVoucherAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
