using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.Expenses.Forms;

public partial class frmAddUpdatePettyCash : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;

    private IPettyCashApiService ApiService
        => _serviceProvider.GetRequiredService<IPettyCashApiService>();

    public frmAddUpdatePettyCash(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.Text = "إضافة طلب عهدة نثرية";
        lblMode.Text = "إضافة طلب عهدة نثرية";
    }

    /// <summary>
    /// ضبط وضع التعديل — يُستدعى بعد حل الـ DI.
    /// </summary>
    public void _SetUpdateMode(int id)
    {
        _editingId = id;
        this.Text = "تعديل طلب عهدة نثرية";
        lblMode.Text = "تعديل طلب عهدة نثرية";
        _ = LoadRequestAsync();
    }

    private async Task LoadRequestAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await ApiService.GetByIdAsync(_editingId);
            if (result.IsSuccess && result.Value != null)
            {
                var r = result.Value;
                dtpDate.Value = r.Date;
                txtEmployeeName.Text = r.EmployeeName;
                numAmount.Value = r.Amount;
                txtReason.Text = r.Reason;
                txtNotes.Text = r.Notes ?? "";
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات الطلب", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtEmployeeName.Text))
        {
            errorProvider.SetError(txtEmployeeName, "اسم الموظف مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtEmployeeName, "");
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

        if (string.IsNullOrWhiteSpace(txtReason.Text))
        {
            errorProvider.SetError(txtReason, "سبب طلب العهدة مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtReason, "");
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
            var dto = new PettyCashRequestDto(
                Id: _isEditMode ? _editingId : 0,
                RequestNo: 0,
                Date: dtpDate.Value,
                EmployeeName: txtEmployeeName.Text.Trim(),
                EmployeeId: null,
                Amount: numAmount.Value,
                Reason: txtReason.Text.Trim(),
                Status: 1, // Pending
                ApprovedBy: null,
                DisbursedDate: null,
                SettlementDate: null,
                SettlementAmount: null,
                RefundAmount: null,
                Notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim()
            );

            if (_isEditMode)
            {
                var result = await ApiService.UpdateAsync(_editingId, dto);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث طلب العهدة بنجاح.", "تم",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث الطلب", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var result = await ApiService.CreateAsync(dto);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إضافة طلب العهدة بنجاح.", "تم",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة الطلب", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}
