using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Controls.Users;

    public partial class frmDocumentSequences : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private DataTable _dt = new();

        public frmDocumentSequences(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

    private async void frmDocumentSequences_Load(object sender, EventArgs e)
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
            var result = await _serviceProvider.GetRequiredService<IDocumentSequenceApiService>().GetAllAsync();

            if (result.IsSuccess && result.Value != null)
            {
                _dt = new DataTable();
                _dt.Columns.Add("Id", typeof(int));
                _dt.Columns.Add("نوع المستند", typeof(string));
                _dt.Columns.Add("الرقم الحالي", typeof(int));
                _dt.Columns.Add("الرقم الجديد", typeof(int));

                foreach (var item in result.Value)
                {
                    _dt.Rows.Add(item.Id, item.DocumentType, item.NextNumber, item.NextNumber);
                }

                _BindGrid();
                lblRecordCount.Text = $"إجمالي السلاسل: {result.Value.Count}";
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل تحميل سلاسل المستندات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        dgvSequences.DataSource = _dv;

        if (dgvSequences.Columns.Contains("Id"))
            dgvSequences.Columns["Id"].Visible = false;

        dgvSequences.AutoResizeColumns();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_dv == null) return;
        string search = txtSearch.Text.Trim();
        if (string.IsNullOrWhiteSpace(search))
            _dv.RowFilter = null;
        else
            _dv.RowFilter = $"نوع المستند LIKE '%{search.Replace("'", "''")}%'";
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
        => await _LoadDataAsync();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private async void btnSaveAll_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;

        try
        {
            foreach (DataRow row in _dt.Rows)
            {
                int id = Convert.ToInt32(row["Id"]);
                int currentNumber = Convert.ToInt32(row["الرقم الحالي"]);
                int newNumber = Convert.ToInt32(row["الرقم الجديد"]);

                if (newNumber != currentNumber)
                {
                    var request = new UpdateDocumentSequenceRequest(id, newNumber);
                    var result = await _serviceProvider.GetRequiredService<IDocumentSequenceApiService>().UpdateAsync(id, request);

                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"فشل تحديث {row["نوع المستند"]}: {result.Error}",
                            "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            MessageBox.Show("تم حفظ جميع التغييرات بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await _LoadDataAsync();
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

    private void dgvSequences_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
        if (dgvSequences.Columns[e.ColumnIndex].Name != "الرقم الجديد") return;

        var row = dgvSequences.Rows[e.RowIndex];
        if (row.Cells["الرقم الجديد"].Value == null) return;

        // Keep the in-memory DataTable in sync
        if (e.RowIndex < _dt.Rows.Count)
        {
            _dt.Rows[e.RowIndex]["الرقم الجديد"] = row.Cells["الرقم الجديد"].Value;
        }
    }
}
