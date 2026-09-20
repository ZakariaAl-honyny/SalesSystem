using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Controls.Users;

    public partial class frmSystemLogs : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dt = new();

        public frmSystemLogs(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

    private async void frmSystemLogs_Load(object sender, EventArgs e)
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
            byte? level = null;
            if (cboLevel.SelectedItem != null)
            {
                string? selected = cboLevel.SelectedItem.ToString();
                level = selected switch
                {
                    "معلومات" => (byte)1,
                    "تحذير" => (byte)2,
                    "خطأ" => (byte)3,
                    "حرج" => (byte)4,
                    _ => null
                };
            }

            DateTime? from = chkDateFilter.Checked ? dtpFrom.Value.ToUniversalTime() : null;
            DateTime? to = chkDateFilter.Checked ? dtpTo.Value.ToUniversalTime() : null;

            var result = await _serviceProvider.GetRequiredService<ILogsApiService>().GetAllAsync(level, from, to);

            if (result.IsSuccess && result.Value != null)
            {
                _dt = new DataTable();
                _dt.Columns.Add("Id", typeof(long));
                _dt.Columns.Add("المستوى", typeof(string));
                _dt.Columns.Add("الرسالة", typeof(string));
                _dt.Columns.Add("المصدر", typeof(string));
                _dt.Columns.Add("المستخدم", typeof(string));
                _dt.Columns.Add("العنوان", typeof(string));
                _dt.Columns.Add("التاريخ", typeof(DateTime));

                foreach (var item in result.Value)
                {
                    _dt.Rows.Add(item.Id, item.LogLevel, item.Message,
                        item.Source ?? "—", item.UserName ?? "—", item.IpAddress ?? "—",
                        item.CreatedAt);
                }

                _BindGrid();
                lblRecordCount.Text = $"إجمالي السجلات: {result.Value.Count}";
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل تحميل سجل النظام", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        if (dgvLogs.Columns.Contains("Id"))
            dgvLogs.Columns["Id"].Visible = false;

        dgvLogs.AutoResizeColumns();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_dv == null) return;
        string search = txtSearch.Text.Trim();
        if (string.IsNullOrWhiteSpace(search))
            _dv.RowFilter = null;
        else
            _dv.RowFilter = $"الرسالة LIKE '%{search.Replace("'", "''")}%' OR المستخدم LIKE '%{search.Replace("'", "''")}%'";
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

    private void dgvLogs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = dgvLogs.Rows[e.RowIndex];
        if (row.Cells["Id"].Value == null) return;

        long id = Convert.ToInt64(row.Cells["Id"].Value);
        var item = _dt.Select($"Id = {id}").FirstOrDefault();
        if (item == null) return;

        txtDetail.Text = $@"المستوى: {item["المستوى"]}
الرسالة: {item["الرسالة"]}
المصدر: {item["المصدر"]}
المستخدم: {item["المستخدم"]}
العنوان: {item["العنوان"]}
التاريخ: {item["التاريخ"]}";
    }
}
