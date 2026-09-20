using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common.Enums.Permissions;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Roles.Forms;

public partial class frmAddUpdateRole : Form
{
    private readonly IServiceProvider _serviceProvider;
    private int? _roleId;
    private RoleDto? _role;
    private bool _isSystemRole;
    private bool _isLoading;

    public frmAddUpdateRole(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    public void _SetEditMode(int roleId)
    {
        _roleId = roleId;
        this.Text = "تعديل الدور";
        lblMode.Text = "تعديل الدور";
        _LoadRoleData();
    }

    private async void _LoadRoleData()
    {
        if (_roleId == null) return;

        Cursor = Cursors.WaitCursor;
        _isLoading = true;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IRoleApiService>().GetByIdAsync(_roleId.Value);
            if (result.IsSuccess && result.Value != null)
            {
                _role = result.Value;
                _isSystemRole = _role.IsSystem;

                txtName.Text = _role.Name;
                txtName.Enabled = !_isSystemRole;
                txtDescription.Text = _role.Description ?? "";

                LoadPermissionCheckboxes();

                if (_isSystemRole)
                {
                    btnSave.Enabled = false;
                    MessageBox.Show("هذا دور نظام ولا يمكن تعديل صلاحياته.", "دور نظام",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات الدور", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ في تحميل بيانات الدور: " + ex.Message, "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _isLoading = false;
            Cursor = Cursors.Default;
        }
    }

    private void LoadPermissionCheckboxes()
    {
        if (_role == null) return;

        SetCheckboxes(flpSales.Controls, (long)_role.SalesPermissions);
        SetCheckboxes(flpInventory.Controls, (long)_role.InventoryPermissions);
        SetCheckboxes(flpAccounting.Controls, (long)_role.AccountingPermissions);
        SetCheckboxes(flpSystem.Controls, (long)_role.SystemPermissions);
    }

    private void SetCheckboxes(Control.ControlCollection controls, long permissions)
    {
        foreach (Control ctrl in controls)
        {
            if (ctrl is CheckBox chk && chk.Tag is long bitValue && bitValue != 0)
            {
                chk.Checked = permissions == -1 || (permissions & bitValue) == bitValue;
                chk.Enabled = !_isSystemRole;
            }
        }
    }

    private long GetPermissionsFromCheckboxes(Control.ControlCollection controls)
    {
        long result = 0;
        foreach (Control ctrl in controls)
        {
            if (ctrl is CheckBox chk && chk.Tag is long bitValue && bitValue != 0 && chk.Checked)
            {
                result |= bitValue;
            }
        }
        return result;
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الصلاحية مطلوب");
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

        string name = txtName.Text.Trim();

        Cursor = Cursors.WaitCursor;

        try
        {
            var description = string.IsNullOrWhiteSpace(txtDescription.Text)
                ? null
                : txtDescription.Text.Trim();

            long salesPerms = GetPermissionsFromCheckboxes(flpSales.Controls);
            long inventoryPerms = GetPermissionsFromCheckboxes(flpInventory.Controls);
            long accountingPerms = GetPermissionsFromCheckboxes(flpAccounting.Controls);
            long systemPerms = GetPermissionsFromCheckboxes(flpSystem.Controls);

            if (_roleId == null)
            {
                var request = new CreateRoleRequest(name, description,
                    salesPerms, inventoryPerms, accountingPerms, systemPerms);
                var result = await _serviceProvider.GetRequiredService<IRoleApiService>().CreateAsync(request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إنشاء الدور بنجاح", "نجاح",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إنشاء الدور", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var request = new UpdateRoleRequest(name, description, IsActive: true,
                    salesPerms, inventoryPerms, accountingPerms, systemPerms);
                var result = await _serviceProvider.GetRequiredService<IRoleApiService>().UpdateAsync(_roleId.Value, request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث الدور بنجاح", "نجاح",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث الدور", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ في حفظ الدور: " + ex.Message, "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (_isLoading) return;

        var chkAll = sender as CheckBox;
        if (chkAll == null) return;

        // Find which groupbox this "select all" belongs to
        var parentPanel = chkAll.Parent; // FlowLayoutPanel
        if (parentPanel == null) return;

        bool isChecked = chkAll.Checked;
        foreach (Control ctrl in parentPanel.Controls)
        {
            if (ctrl is CheckBox chk && chk.Tag is long)
            {
                chk.Checked = isChecked;
            }
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void chkSalesCreateReceipts_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void chkSalesManagePrices_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void grpAccounting_Enter(object sender, EventArgs e)
    {

    }
}
