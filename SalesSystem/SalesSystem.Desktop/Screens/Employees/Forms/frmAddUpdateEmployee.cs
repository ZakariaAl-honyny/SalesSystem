using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Employees.Forms;

public partial class frmAddUpdateEmployee : Form
{
    private readonly IServiceProvider _serviceProvider;
    private EmployeeMockData? _existingEmployee;

    public frmAddUpdateEmployee(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    /// <summary>
    /// Loads the employee being edited, or null for add mode. (UI shape — mock data.)
    /// </summary>
    public void LoadData(EmployeeMockData? existingEmployee)
    {
        _existingEmployee = existingEmployee;

        if (_existingEmployee != null)
        {
            this.Text = "تعديل موظف";
            lblMode.Text = "تعديل موظف";
            txtName.Text = _existingEmployee.Name;
            txtEmployeeNumber.Text = _existingEmployee.Id.ToString(CultureInfo.InvariantCulture);
            txtEmployeeNumber.ReadOnly = true;
            int jobTitleIndex = cmbJobTitle.FindStringExact(_existingEmployee.JobTitle);
            cmbJobTitle.SelectedIndex = jobTitleIndex >= 0 ? jobTitleIndex : 0;
            txtPhone.Text = _existingEmployee.Phone;
            txtNationalId.Text = _existingEmployee.NationalId;
            txtBaseSalary.Text = _existingEmployee.BaseSalary.ToString("N2", CultureInfo.InvariantCulture);
            txtCommissionRate.Text = _existingEmployee.CommissionRate.ToString("N2", CultureInfo.InvariantCulture);
            dtpHireDate.Value = _existingEmployee.HireDate;
            chkActive.Checked = _existingEmployee.IsActive;
            string linkedUser = string.IsNullOrEmpty(_existingEmployee.LinkedUser) ? "بدون" : _existingEmployee.LinkedUser;
            int linkedUserIndex = cmbLinkedUser.FindStringExact(linkedUser);
            cmbLinkedUser.SelectedIndex = linkedUserIndex >= 0 ? linkedUserIndex : 0;
            txtNotes.Text = _existingEmployee.Notes;
        }
        else
        {
            this.Text = "إضافة موظف";
            lblMode.Text = "إضافة موظف";
            txtName.Clear();
            txtEmployeeNumber.Text = EmployeeMockStore.NextId.ToString(CultureInfo.InvariantCulture);
            txtEmployeeNumber.ReadOnly = false;
            cmbJobTitle.SelectedIndex = 0;
            txtPhone.Clear();
            txtNationalId.Clear();
            txtBaseSalary.Text = "0.00";
            txtCommissionRate.Text = "0.00";
            dtpHireDate.Value = DateTime.Today;
            chkActive.Checked = true;
            cmbLinkedUser.SelectedIndex = 0; // "بدون"
            txtNotes.Clear();
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الموظف مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (!int.TryParse(txtEmployeeNumber.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
        {
            errorProvider.SetError(txtEmployeeNumber, "رقم الموظف يجب أن يكون رقماً صحيحاً.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtEmployeeNumber, "");
        }

        if (!decimal.TryParse(txtBaseSalary.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
        {
            errorProvider.SetError(txtBaseSalary, "الراتب الأساسي يجب أن يكون رقماً.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtBaseSalary, "");
        }

        if (!decimal.TryParse(txtCommissionRate.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
        {
            errorProvider.SetError(txtCommissionRate, "نسبة العمولة يجب أن تكون رقماً.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtCommissionRate, "");
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        btnSave.Enabled = false;
        try
        {
            var name = txtName.Text.Trim();
            int employeeNumber = int.Parse(txtEmployeeNumber.Text, CultureInfo.InvariantCulture);
            string jobTitle = cmbJobTitle.SelectedItem?.ToString() ?? string.Empty;
            string phone = txtPhone.Text.Trim();
            string nationalId = txtNationalId.Text.Trim();
            decimal baseSalary = decimal.Parse(txtBaseSalary.Text, NumberStyles.Number, CultureInfo.InvariantCulture);
            decimal commissionRate = decimal.Parse(txtCommissionRate.Text, NumberStyles.Number, CultureInfo.InvariantCulture);
            DateTime hireDate = dtpHireDate.Value.Date;
            bool isActive = chkActive.Checked;
            string linkedUser = cmbLinkedUser.SelectedItem?.ToString() == "بدون"
                ? string.Empty
                : cmbLinkedUser.SelectedItem?.ToString() ?? string.Empty;
            string notes = txtNotes.Text.Trim();

            if (_existingEmployee == null)
            {
                EmployeeMockStore.Employees.Add(new EmployeeMockData
                {
                    Id = employeeNumber,
                    Name = name,
                    JobTitle = jobTitle,
                    Phone = phone,
                    NationalId = nationalId,
                    BaseSalary = baseSalary,
                    CommissionRate = commissionRate,
                    HireDate = hireDate,
                    IsActive = isActive,
                    LinkedUser = linkedUser,
                    Notes = notes,
                });
            }
            else
            {
                _existingEmployee.Id = employeeNumber;
                _existingEmployee.Name = name;
                _existingEmployee.JobTitle = jobTitle;
                _existingEmployee.Phone = phone;
                _existingEmployee.NationalId = nationalId;
                _existingEmployee.BaseSalary = baseSalary;
                _existingEmployee.CommissionRate = commissionRate;
                _existingEmployee.HireDate = hireDate;
                _existingEmployee.IsActive = isActive;
                _existingEmployee.LinkedUser = linkedUser;
                _existingEmployee.Notes = notes;
            }

            MessageBox.Show("تم حفظ بيانات الموظف بنجاح.", "تم الحفظ",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء الحفظ: " + ex.Message, "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
        => this.Close();

    // ── Input Restriction (UI shape) ────────────────────────────────────────────

    private void txtEmployeeNumber_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void txtDecimal_KeyPress(object sender, KeyPressEventArgs e)
    {
        var txt = (TextBox)sender;
        if (char.IsControl(e.KeyChar)) return;

        if (e.KeyChar == '.' && !txt.Text.Contains('.')) return;

        if (!char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }
}