using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Expenses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

public partial class frmAddUpdateExpense : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;
    private List<CashBoxDto> _cashBoxes = new();

    public frmAddUpdateExpense(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.Text = "إضافة مصروف جديد";
        lblMode.Text = "إضافة مصروف جديد";
        _ = LoadComboBoxesAsync();
    }

    public frmAddUpdateExpense(int id, IServiceProvider serviceProvider) : this(serviceProvider)
    {
        _editingId = id;
        this.Text = "تعديل مصروف";
        lblMode.Text = "تعديل مصروف";
        _ = LoadExpenseAsync();
    }

    private async Task LoadComboBoxesAsync()
    {
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

    private async Task LoadExpenseAsync()
    {
        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().GetByIdAsync(_editingId);
        Cursor = Cursors.Default;

        if (result.IsSuccess && result.Value != null)
        {
            var ex = result.Value;
            dtpDate.Value = ex.ExpenseDate;
            numAmount.Value = ex.Amount;
            txtNotes.Text = ex.Notes ?? "";
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل في تحميل بيانات المصروف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
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

        if (string.IsNullOrWhiteSpace(txtNotes.Text))
        {
            errorProvider.SetError(txtNotes, "بيان المصروف مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtNotes, "");
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

            if (_isEditMode)
            {
                var request = new UpdateExpenseRequest(
                    ExpenseDate: dtpDate.Value,
                    ExpenseAccountId: 0,
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().UpdateAsync(_editingId, request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث المصروف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var request = new CreateExpenseRequest(
                    ExpenseDate: dtpDate.Value,
                    ExpenseAccountId: 0,
                    CashBoxId: cashBoxId,
                    Amount: numAmount.Value,
                    Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<IExpenseApiService>().CreateAsync(request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إضافة المصروف بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة المصروف", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    /// Sets the editor to Update mode for the given expense ID.
    /// Called after DI resolution (parameterless constructor).
    /// </summary>
    public void _SetUpdateMode(int id)
    {
        _editingId = id;
        this.Text = "تعديل مصروف";
        lblMode.Text = "تعديل مصروف";
        _ = LoadExpenseAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
