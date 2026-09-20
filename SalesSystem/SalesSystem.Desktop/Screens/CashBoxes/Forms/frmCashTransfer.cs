using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.CashBoxes;
using SalesSystem.Desktop.Helpers;
 
namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

public partial class frmCashTransfer : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<CashBoxDto> _cashBoxes = new();

    public frmCashTransfer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _ = LoadCashBoxesAsync();
    }

    private async Task LoadCashBoxesAsync()
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().GetAllAsync();
        Cursor = Cursors.Default;

        if (result.IsSuccess && result.Value != null)
        {
            _cashBoxes = result.Value.Where(b => b.IsActive).ToList();
            cmbSource.DataSource = _cashBoxes.Select(b => new { b.Id, b.Name }).ToList();
            cmbSource.DisplayMember = "Name";
            cmbSource.ValueMember = "Id";
            cmbSource.SelectedIndex = -1;

            cmbDestination.DataSource = _cashBoxes.Select(b => new { b.Id, b.Name }).ToList();
            cmbDestination.DisplayMember = "Name";
            cmbDestination.ValueMember = "Id";
            cmbDestination.SelectedIndex = -1;
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في تحميل قائمة الصناديق.", "خطأ في تحميل البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (cmbSource.SelectedValue == null)
        {
            errorProvider.SetError(cmbSource, "يرجى اختيار الصندوق المصدر.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbSource, "");
        }

        if (cmbDestination.SelectedValue == null)
        {
            errorProvider.SetError(cmbDestination, "يرجى اختيار الصندوق الهدف.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cmbDestination, "");
        }

        if (cmbSource.SelectedValue != null && cmbDestination.SelectedValue != null &&
            (int)cmbSource.SelectedValue == (int)cmbDestination.SelectedValue)
        {
            errorProvider.SetError(cmbDestination, "لا يمكن التحويل لنفس الصندوق.");
            isValid = false;
        }
        else if (cmbDestination.SelectedValue != null)
        {
            errorProvider.SetError(cmbDestination, "");
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

    private async void btnTransfer_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        Cursor = Cursors.WaitCursor;
        btnTransfer.Enabled = false;

        try
        {
            var request = new CashTransferRequest(
                (int)cmbSource.SelectedValue,
                (int)cmbDestination.SelectedValue,
                numAmount.Value,
                string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim());

            var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().TransferAsync(request);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم التحويل بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في التحويل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            btnTransfer.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
