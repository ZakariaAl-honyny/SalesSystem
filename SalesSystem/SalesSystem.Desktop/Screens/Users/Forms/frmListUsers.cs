using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Users;

namespace SalesSystem.Desktop.Screens.Users.Forms
{
    public partial class frmListUsers : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dtUsers = new();
        private DataView _dvUsers = new();

        private static readonly Dictionary<string, string> _filterColumns =
            new Dictionary<string, string>
            {
                { "رقم المستخدم",   "UserID" },
                { "اسم المستخدم",   "UserName" },
                { "الحالة",         "StatusDisplay" },
                { "الدور",          "RoleDisplay" },
            };

        public frmListUsers(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvUsers.Columns.Clear();

            var columns = new[]
            {
                new { Name = "UserID",        Header = "رقم المستخدم",   Fill = 8,  Min = 100 },
                new { Name = "UserName",      Header = "اسم المستخدم",   Fill = 18, Min = 180 },
                new { Name = "RoleDisplay",   Header = "الدور",          Fill = 14, Min = 150 },
                new { Name = "BranchDisplay", Header = "الفرع",          Fill = 10, Min = 100 },
                new { Name = "StatusDisplay", Header = "الحالة",         Fill = 8,  Min = 100 },
            };

            foreach (var col in columns)
            {
                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.Name,
                    MinimumWidth = col.Min,
                    FillWeight = col.Fill,
                    ReadOnly = true,
                });
            }
        }

        // ── Data Loading ───────────────────────────────────────────────────────────

        private async void _RefreshUsersList()
        {
            Cursor = dgvUsers.Cursor = Cursors.WaitCursor;

            try
            {
                _dtUsers = new DataTable();
                _dtUsers.Columns.Add("UserID", typeof(int));
                _dtUsers.Columns.Add("UserName", typeof(string));
                _dtUsers.Columns.Add("RoleDisplay", typeof(string));
                _dtUsers.Columns.Add("BranchDisplay", typeof(string));
                _dtUsers.Columns.Add("StatusDisplay", typeof(string));
                _dtUsers.Columns.Add("IsActive", typeof(bool));

                var result = await _serviceProvider.GetRequiredService<IUserApiService>().GetAllAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    foreach (var u in result.Value)
                    {
                        string roleDisplay = u.RoleName ?? u.RoleId.ToString();
                        string branchDisplay = u.BranchId.HasValue ? u.BranchId.Value.ToString() : "---";
                        string statusDisplay = u.IsActive
                            ? (u.IsLocked ? "مقفل" : "نشط")
                            : "معطل";

                        _dtUsers.Rows.Add(
                            u.Id,
                            u.UserName,
                            roleDisplay,
                            branchDisplay,
                            statusDisplay,
                            u.IsActive
                        );
                    }
                }

                _dvUsers = new DataView(_dtUsers);
                dgvUsers.DataSource = _dvUsers;
                if (dgvUsers.Columns.Contains("IsActive"))
                    dgvUsers.Columns["IsActive"].Visible = false;
                lblRecordsCount.Text = _dvUsers.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل بيانات المستخدمين: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = dgvUsers.Cursor = Cursors.Default;
            }
        }

        private void _ReloadForm()
        {
            _SetupGridColumns();
            _RefreshUsersList();
        }

        // ── Helpers ────────────────────────────────────────────────────────────────

        private int _GetSelectedUserId()
        {
            if (dgvUsers.CurrentRow == null) return -1;
            return Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value);
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _ReloadForm();
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        // ── Filter Events ──────────────────────────────────────────────────────────

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.Text != "لا شيئ";
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Clear();
                txtFilterValue.Focus();
            }
            _dvUsers.RowFilter = "";
            lblRecordsCount.Text = _dvUsers.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text.Trim();

            if (string.IsNullOrEmpty(filterText) || !_filterColumns.TryGetValue(cbFilterBy.Text, out var columnName))
            {
                _dvUsers.RowFilter = "";
            }
            else if (columnName == "UserID")
            {
                _dvUsers.RowFilter = string.Format("CONVERT({0}, 'System.String') LIKE '{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }
            else
            {
                _dvUsers.RowFilter = string.Format("{0} LIKE '%{1}%'",
                    columnName, filterText.Replace("'", "''"));
            }

            lblRecordsCount.Text = _dvUsers.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                return;
            }

            bool isNumericFilter = cbFilterBy.Text == "رقم المستخدم";

            if (isNumericFilter && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "يُسمح فقط بإدخال الأرقام في هذا الحقل.");
                return;
            }

            if (!isNumericFilter && char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
                clsInputFeedback.ShowRejectTooltip(
                    txtFilterValue,
                    "لا يُسمح بإدخال علامات الترقيم.");
            }
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvUsers_DoubleClick(object sender, EventArgs e)
        {
            int userId = _GetSelectedUserId();
            if (userId <= 0) return;

            var editor = _serviceProvider.GetRequiredService<frmAddUpdateUser>();
            editor._SetUpdateMode(userId);
            if (editor.ShowDialog() == DialogResult.OK)
                _ReloadForm();
        }

        // ── Toolbar / Button Events ────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var editor = _serviceProvider.GetRequiredService<frmAddUpdateUser>();
            if (editor.ShowDialog() == DialogResult.OK)
                _ReloadForm();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
            => btnAdd_Click(sender, e);

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
            => dgvUsers_DoubleClick(sender, e);

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userId = _GetSelectedUserId();
            if (userId <= 0) return;

            var passwordForm = _serviceProvider.GetRequiredService<frmPasswordChange>();
            passwordForm._SetUpdateMode(userId);
            passwordForm.ShowDialog();
        }

        private async void resetPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userId = _GetSelectedUserId();
            if (userId <= 0) return;

            var confirm = MessageBox.Show("هل أنت متأكد من إعادة تعيين كلمة المرور لهذا المستخدم؟\nسيتم تعيين كلمة المرور إلى: 12345678",
                "تأكيد إعادة تعيين كلمة المرور", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                var result = await _serviceProvider.GetRequiredService<IUserApiService>().ResetPasswordAsync(userId);
                if (result.IsSuccess)
                {
                    MessageBox.Show($"تم إعادة تعيين كلمة المرور بنجاح.\nكلمة المرور الجديدة: 12345678\n\n{result.Value?.Message}",
                        "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إعادة تعيين كلمة المرور", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
            => _ReloadForm();

        private async void toggleStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userId = _GetSelectedUserId();
            if (userId <= 0) return;

            // Get the current user's active status from the hidden column
            var row = _dtUsers.AsEnumerable()
                .FirstOrDefault(r => Convert.ToInt32(r["UserID"]) == userId);
            if (row == null) return;

            bool currentlyActive = Convert.ToBoolean(row["IsActive"]);
            string newStatus = currentlyActive ? "تعطيل" : "تنشيط";

            // Prevent self-deactivation
            if (currentlyActive && userId == SessionManager.UserId)
            {
                MessageBox.Show("لا يمكنك تعطيل حسابك الخاص.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"هل أنت متأكد من {newStatus} هذا المستخدم؟",
                "تأكيد العملية",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes) return;

            Cursor = Cursors.WaitCursor;

            try
            {
                // Fetch current user data to preserve RoleId and BranchId
                var userResult = await _serviceProvider.GetRequiredService<IUserApiService>().GetByIdAsync(userId);
                if (!userResult.IsSuccess || userResult.Value == null)
                {
                    MessageBox.Show("تعذر الحصول على بيانات المستخدم.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var userDto = userResult.Value;

                // Prevent deactivation of admin accounts
                if (userDto.RoleId == 1)
                {
                    MessageBox.Show("لا يمكن تعطيل حساب مدير النظام.", "محظور",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = await _serviceProvider.GetRequiredService<IUserApiService>().UpdateAsync(
                    userId,
                    new UpdateUserRequest(
                        RoleId: userDto.RoleId,
                        BranchId: userDto.BranchId,
                        IsLocked: userDto.IsLocked,
                        IsActive: !currentlyActive));

                if (result.IsSuccess)
                {
                    _ReloadForm();
                    MessageBox.Show($"تم {newStatus} المستخدم بنجاح.", "نجاح",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ أثناء تحديث حالة المستخدم.", "خطأ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void cmsUsers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsUsers.Enabled = dgvUsers.Rows.Count > 0 && dgvUsers.CurrentRow != null;
        }
    }
}
