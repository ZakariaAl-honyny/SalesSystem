using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Common;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Controls.Users;

    public partial class frmAuditLog : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dt = new();

        public frmAuditLog(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

    private async void frmAuditLog_Load(object sender, EventArgs e)
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
            var query = new AuditLogQuery
            {
                UserId = string.IsNullOrWhiteSpace(txtUserId.Text) ? null : int.TryParse(txtUserId.Text, out var uid) ? uid : null,
                Action = string.IsNullOrWhiteSpace(txtAction.Text) ? null : txtAction.Text.Trim(),
                EntityType = string.IsNullOrWhiteSpace(txtEntityType.Text) ? null : txtEntityType.Text.Trim(),
                From = chkDateFilter.Checked ? dtpFrom.Value.ToUniversalTime() : null,
                To = chkDateFilter.Checked ? dtpTo.Value.ToUniversalTime() : null,
                Page = 1,
                PageSize = 500
            };

            Result<PaginatedResult<AuditLogDto>> result;
            if (chkLoginHistory.Checked)
            {
                var loginResult = await _serviceProvider.GetRequiredService<IAuditLogApiService>().GetLoginHistoryAsync(query.UserId);
                if (loginResult.IsSuccess && loginResult.Value != null)
                {
                    var paginated = new PaginatedResult<AuditLogDto>(loginResult.Value, loginResult.Value.Count, 1, loginResult.Value.Count);
                    result = Result<PaginatedResult<AuditLogDto>>.Success(paginated);
                }
                else
                {
                    result = Result<PaginatedResult<AuditLogDto>>.Failure(loginResult.Error ?? "فشل تحميل سجل الدخول");
                }
            }
            else
            {
                result = await _serviceProvider.GetRequiredService<IAuditLogApiService>().QueryAsync(query);
            }

            if (result.IsSuccess && result.Value != null)
            {
                _dt = new DataTable();
                _dt.Columns.Add("Id", typeof(long));
                _dt.Columns.Add("المستخدم", typeof(string));
                _dt.Columns.Add("الإجراء", typeof(string));
                _dt.Columns.Add("الكيان", typeof(string));
                _dt.Columns.Add("معرف الكيان", typeof(int));
                _dt.Columns.Add("العنوان", typeof(string));
                _dt.Columns.Add("التاريخ", typeof(DateTime));

                foreach (var item in result.Value.Items)
                {
                    _dt.Rows.Add(item.Id, item.UserName ?? "—", item.Action,
                        item.EntityType ?? "—", item.EntityId, item.IpAddress ?? "—",
                        item.Timestamp);
                }

                _BindGrid();
                lblRecordCount.Text = $"إجمالي السجلات: {result.Value.TotalCount}";
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل تحميل سجل التدقيق", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        dgvLogs.DataSource = _dv;

        if (dgvLogs.Columns.Count > 0)
        {
            if (dgvLogs.Columns.Contains("Id"))
                dgvLogs.Columns["Id"].Visible = false;
        }

        dgvLogs.AutoResizeColumns();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_dv == null) return;
        string search = txtSearch.Text.Trim();
        if (string.IsNullOrWhiteSpace(search))
            _dv.RowFilter = null;
        else
            _dv.RowFilter = $"المستخدم LIKE '%{search.Replace("'", "''")}%' OR الإجراء LIKE '%{search.Replace("'", "''")}%'";
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
        => await _LoadDataAsync();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void chkDateFilter_CheckedChanged(object sender, EventArgs e)
    {
        dtpFrom.Enabled = chkDateFilter.Checked;
        dtpTo.Enabled = chkDateFilter.Checked;
    }

    private void chkLoginHistory_CheckedChanged(object sender, EventArgs e)
    {
        lblAction.Visible = !chkLoginHistory.Checked;
        txtAction.Visible = !chkLoginHistory.Checked;
        lblEntityType.Visible = !chkLoginHistory.Checked;
        txtEntityType.Visible = !chkLoginHistory.Checked;
    }

    private void dgvLogs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var row = dgvLogs.Rows[e.RowIndex];
        if (row.Cells["Id"].Value == null) return;

        long id = Convert.ToInt64(row.Cells["Id"].Value);
        var item = _dt.Select($"Id = {id}").FirstOrDefault();
        if (item == null) return;

        // Show details
        txtDetail.Text = $@"المستخدم: {item["المستخدم"]}
الإجراء: {item["الإجراء"]}
الكيان: {item["الكيان"]}
معرف الكيان: {item["معرف الكيان"]}
العنوان: {item["العنوان"]}
التاريخ: {item["التاريخ"]}";
    }
}
