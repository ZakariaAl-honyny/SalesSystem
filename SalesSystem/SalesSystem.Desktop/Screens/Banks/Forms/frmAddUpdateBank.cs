using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Screens.Banks;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Banks.Forms;

public partial class frmAddUpdateBank : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;

    public frmAddUpdateBank(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.Text = "إضافة بنك جديد";
        lblMode.Text = "إضافة بنك جديد";
    }

    private async Task LoadBankAsync()
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<IBankApiService>().GetByIdAsync(_editingId);
        Cursor = Cursors.Default;

        if (result.IsSuccess && result.Value != null)
        {
            var bank = result.Value;
            txtName.Text = bank.Name;
            txtAccountNumber.Text = bank.AccountNumber ?? "";
            txtIban.Text = bank.Iban ?? "";
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في تحميل بيانات البنك", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم البنك مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
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
            if (_isEditMode)
            {
                var request = new UpdateBankRequest(
                    Name: txtName.Text.Trim(),
                    AccountNumber: string.IsNullOrWhiteSpace(txtAccountNumber.Text) ? null : txtAccountNumber.Text.Trim(),
                    Iban: string.IsNullOrWhiteSpace(txtIban.Text) ? null : txtIban.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<IBankApiService>().UpdateAsync(_editingId, request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث البنك بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث البنك", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var request = new CreateBankRequest(
                    AccountId: null,
                    Name: txtName.Text.Trim(),
                    AccountNumber: string.IsNullOrWhiteSpace(txtAccountNumber.Text) ? null : txtAccountNumber.Text.Trim(),
                    Iban: string.IsNullOrWhiteSpace(txtIban.Text) ? null : txtIban.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<IBankApiService>().CreateAsync(request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إضافة البنك بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة البنك", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    /// Sets the editor to Update mode for the given bank ID.
    /// Called after DI resolution (parameterless constructor).
    /// </summary>
    public void _SetUpdateMode(int id)
    {
        _editingId = id;
        this.Text = "تعديل بنك";
        lblMode.Text = "تعديل بنك";
        _ = LoadBankAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
