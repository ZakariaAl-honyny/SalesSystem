using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Users;
using System.Diagnostics;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Users.Forms;

public partial class frmAddUpdateUser : Form
{
    private readonly IServiceProvider _serviceProvider;

    public enum enMode { AddNew = 0, Update = 1 }

    private enMode _Mode;
    private int _UserId = -1;
    private UserDto? _User;
    private List<RoleDto> _roles = new();
    private List<BranchDto> _branches = new();

    public frmAddUpdateUser(IServiceProvider serviceProvider)
    {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            _Mode = enMode.AddNew;
    }

    private void _ResetDefaultValues()
    {
        if (_Mode == enMode.AddNew)
        {
            lblMode.Text = "إضافة مستخدم جديد";
            this.Text = "إضافة مستخدم جديد";
            btnSave.Text = "إضافة";
            chkIsActive.Checked = true;
            chkMustChangePassword.Checked = true;
            lblUserId.Text = "جديد";
            txtUserName.Clear();
            chkIsActive.Visible = false;
        }
        else
        {
            lblMode.Text = "تعديل بيانات المستخدم";
            this.Text = "تعديل بيانات المستخدم";
            btnSave.Text = "تعديل";
            chkIsActive.Visible = true;
        }
    }

    private async void _LoadData()
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IUserApiService>().GetByIdAsync(_UserId);

            if (!result.IsSuccess || result.Value == null)
            {
                MessageBox.Show("لا يوجد مستخدم بهذا المعرف: " + _UserId.ToString(), "المستخدم غير موجود",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            _User = result.Value;
            lblUserId.Text = _UserId.ToString();
            txtUserName.Text = _User.UserName;
            chkIsActive.Checked = _User.IsActive;
            chkMustChangePassword.Checked = _User.MustChangePassword;

            //await Task.Delay(500);

            // Set RoleId in combo
            if (cbRoleId.Items.Count > 0)
                cbRoleId.SelectedValue = Convert.ToInt32(_User.RoleId);

            // Set BranchId in combo (0 = "--- بدون فرع ---")
            if (cbBranchId.Items.Count > 0)
            {
                cbBranchId.SelectedValue = _User.BranchId ?? (short)0;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ في تحميل بيانات المستخدم: " + ex.Message, "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void frmUserEditor_Load(object sender, EventArgs e)
    {
        try
        {
            await _LoadLookupsAsync();
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ في تحميل النموذج: " + ex.Message, "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task _LoadLookupsAsync()
    {
        // Load roles
        var rolesResult = await _serviceProvider.GetRequiredService<IRoleApiService>().GetAllAsync(includeInactive: false);
        if (rolesResult.IsSuccess && rolesResult.Value != null)
        {
            _roles = rolesResult.Value.OrderBy(r => r.Id).ToList();
            _roles.Insert(0, new RoleDto(0, "--- بدون دور ---", "",false,false));
            cbRoleId.DisplayMember = "Name";
            cbRoleId.ValueMember = "Id";
            cbRoleId.DataSource = new List<RoleDto>(_roles);
        }

        // Load branches (if service available)
        try
        {
            var branchService = _serviceProvider.GetService<IBranchApiService>();
            if (branchService is not null)
            {
                var branchResult = await branchService.GetAllAsync();
                if (branchResult.IsSuccess && branchResult.Value != null)
                {
                    _branches = branchResult.Value.Where(b => b.IsActive).ToList();
                    _branches.Insert(0, new BranchDto((short)0, "--- بدون فرع ---"));
                    cbBranchId.DisplayMember = "Name";
                    cbBranchId.ValueMember = "Id";
                    cbBranchId.DataSource = new List<BranchDto>(_branches);
                }
            }
        }
        catch
        {
            // Branch service not available — leave combo empty
        }
    }

    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtUserName.Text))
        {
            errorProvider.SetError(txtUserName, "اسم المستخدم مطلوب");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtUserName, "");
        }

        if (cbRoleId.SelectedItem == null || cbRoleId.SelectedIndex <= 0)
        {
            errorProvider.SetError(cbRoleId, "يرجى اختيار الصلاحية");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cbRoleId, "");
        }

        if (cbBranchId.SelectedItem == null)
        {
            errorProvider.SetError(cbBranchId, "يرجى اختيار الفرع");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(cbBranchId, "");
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
        if (!_ValidateForm())
            return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            short roleId = Convert.ToInt16(cbRoleId.SelectedValue ?? 1);
            short? branchId = null;
            if (cbBranchId.SelectedValue is short branchVal && branchVal > 0)
                branchId = branchVal;

            if (_Mode == enMode.AddNew)
            {
                var request = new CreateUserRequest(
                    txtUserName.Text.Trim(),
                    RoleId: roleId,
                    BranchId: branchId,
                    Password: "12345678",
                    DefaultCashBoxId: null);

                var result = await _serviceProvider.GetRequiredService<IUserApiService>().CreateAsync(request);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تمت إضافة المستخدم بنجاح.", "تم",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة المستخدم", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var request = new UpdateUserRequest(
                    RoleId: roleId,
                    BranchId: branchId,
                    IsLocked: null,
                    Password: null,
                    DefaultCashBoxId: null);

                var result = await _serviceProvider.GetRequiredService<IUserApiService>().UpdateAsync(_UserId, request);

                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث بيانات المستخدم بنجاح.", "تم",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث المستخدم", "خطأ",
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
    {
        this.Close();
    }

    /// <summary>
    /// Sets the editor to Update mode for the given user ID.
    /// Called after DI resolution (parameterless constructor).
    /// </summary>
    public void _SetUpdateMode(int userId)
    {
        _UserId = userId;
        _Mode = enMode.Update;
    }

    private void cbRoleId_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
