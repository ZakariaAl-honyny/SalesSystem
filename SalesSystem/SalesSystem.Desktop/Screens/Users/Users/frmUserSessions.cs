using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Controls.Users;

    public partial class frmUserSessions : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dt = new();

        public frmUserSessions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

    private async void frmUserSessions_Load(object sender, EventArgs e)
    {
        try
        {
            await _LoadDataAsync();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل النموذج");
            System.Windows.Forms.MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }

    private async Task _LoadDataAsync()
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            int? userId = string.IsNullOrWhiteSpace(txtUserId.Text) ? null : int.TryParse(txtUserId.Text, out var uid) ? uid : null;
            bool includeRevoked = chkIncludeRevoked.Checked;

            var result = await _serviceProvider.GetRequiredService<IUserSessionApiService>().GetAllAsync(userId, includeRevoked);

            if (result.IsSuccess && result.Value != null)
            {
                _dt = new DataTable();
                _dt.Columns.Add("Id", typeof(long));
                _dt.Columns.Add("رقم المستخدم", typeof(int));
                _dt.Columns.Add("اسم المستخدم", typeof(string));
                _dt.Columns.Add("الجهاز", typeof(string));
                _dt.Columns.Add("العنوان", typeof(string));
                _dt.Columns.Add("تاريخ البدء", typeof(DateTime));
                _dt.Columns.Add("آخر نشاط", typeof(DateTime));
                _dt.Columns.Add("ينتهي في", typeof(DateTime));
                _dt.Columns.Add("الحالة", typeof(string));

                foreach (var item in result.Value)
                {
                    _dt.Rows.Add(item.Id, item.UserId, item.UserName ?? "—",
                        item.DeviceName ?? "—", item.IpAddress ?? "—",
                        item.CreatedAt, item.LastActivityAt, item.ExpiresAt,
                        item.StatusDisplay);
                }

                _BindGrid();
                lblRecordCount.Text = $"إجمالي الجلسات: {result.Value.Count}";
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل تحميل الجلسات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private DataView? _dv;
    private void _BindGrid()
    {
        _dv = new DataView(_dt);
        dgvSessions.DataSource = _dv;

        if (dgvSessions.Columns.Contains("Id"))
            dgvSessions.Columns["Id"].Visible = false;

        dgvSessions.AutoResizeColumns();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_dv == null) return;
        string search = txtSearch.Text.Trim();
        if (string.IsNullOrWhiteSpace(search))
            _dv.RowFilter = null;
        else
            _dv.RowFilter = $"اسم المستخدم LIKE '%{search.Replace("'", "''")}%'";
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
        => await _LoadDataAsync();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void chkIncludeRevoked_CheckedChanged(object sender, EventArgs e)
        => _ = _LoadDataAsync();

    private async void btnRevoke_Click(object sender, EventArgs e)
    {
        if (dgvSessions.CurrentRow == null || dgvSessions.CurrentRow.Cells["Id"].Value == null)
        {
            MessageBox.Show("الرجاء تحديد جلسة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        long sessionId = Convert.ToInt64(dgvSessions.CurrentRow.Cells["Id"].Value);
        string userName = dgvSessions.CurrentRow.Cells["اسم المستخدم"].Value?.ToString() ?? "";

        var confirm = MessageBox.Show($"هل أنت متأكد من إلغاء جلسة المستخدم {userName}؟",
            "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        Cursor = Cursors.WaitCursor;

        try
        {
            var result = await _serviceProvider.GetRequiredService<IUserSessionApiService>().RevokeAsync(sessionId);
            if (result.IsSuccess)
            {
                MessageBox.Show("تم إلغاء الجلسة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await _LoadDataAsync();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في إلغاء الجلسة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
}
