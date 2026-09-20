using Serilog;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Requests;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Taxes.Forms;

public partial class frmAddUpdateTax : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int? _taxId;

        public frmAddUpdateTax()
        {
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        public frmAddUpdateTax(IServiceProvider serviceProvider) : this()
    {
        _serviceProvider = serviceProvider;
    }

    public frmAddUpdateTax(IServiceProvider serviceProvider, int taxId) : this(serviceProvider)
    {
        _taxId = taxId;
        this.Text = "تعديل الضريبة";
        lblMode.Text = "تعديل الضريبة";
        _LoadTaxData();
    }

    private async void _LoadTaxData()
    {
        if (_taxId == null) return;

        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<ITaxesApiService>().GetAllAsync(includeInactive: true);
        Cursor = Cursors.Default;

        if (result.IsSuccess && result.Value != null)
        {
            var tax = result.Value.FirstOrDefault(t => t.Id == _taxId.Value);
            if (tax != null)
            {
                txtName.Text = tax.Name;
                txtCode.Text = tax.Code;
                numRate.Value = (decimal)tax.Rate;
                cboTaxType.SelectedIndex = tax.TaxType == 2 ? 1 : 0;
                chkIsDefault.Checked = tax.IsDefault;
            }
            else
            {
                MessageBox.Show("الضريبة غير موجودة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الضريبة مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (string.IsNullOrWhiteSpace(txtCode.Text))
        {
            errorProvider.SetError(txtCode, "رمز الضريبة مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtCode, "");
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

        string name = txtName.Text.Trim();
        string code = txtCode.Text.Trim();
        decimal rate = numRate.Value;
        byte taxType = (byte)(cboTaxType.SelectedIndex == 1 ? 2 : 1);
        bool isDefault = chkIsDefault.Checked;

        btnSave.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try
        {
            if (_taxId == null)
            {
                var request = new CreateTaxRequest(name, code, rate, taxType, isDefault);
                var result = await _serviceProvider.GetRequiredService<ITaxesApiService>().CreateAsync(request);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إنشاء الضريبة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إنشاء الضريبة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var request = new UpdateTaxRequest(name, code, rate, taxType, isDefault, IsActive: true);
                var result = await _serviceProvider.GetRequiredService<ITaxesApiService>().UpdateAsync(_taxId.Value, request);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث الضريبة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث الضريبة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ بيانات الضريبة");
            MessageBox.Show("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Sets the editor to Update mode for the given tax ID.
    /// Called after DI resolution (parameterless constructor).
    /// </summary>
    public void _SetUpdateMode(int taxId)
    {
        _taxId = taxId;
        this.Text = "تعديل الضريبة";
        lblMode.Text = "تعديل الضريبة";
        _LoadTaxData();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
