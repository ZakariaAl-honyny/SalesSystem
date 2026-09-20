using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Taxes.Forms;

public partial class frmListTaxes : Form
{
    private readonly IServiceProvider _serviceProvider;
    private DataTable _dt = new();
    private ToolTip _toolTip = new();

    public frmListTaxes(IServiceProvider serviceProvider)
    {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
            _toolTip.SetToolTip(chkIncludeInactive, "عرض الضرائب غير النشطة (المحذوفة)");
    }

    private async void frmListTaxes_Load(object sender, EventArgs e)
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
            bool includeInactive = chkIncludeInactive.Checked;
            var result = await _serviceProvider.GetRequiredService<ITaxesApiService>().GetAllAsync(includeInactive);

            if (result.IsSuccess && result.Value != null)
            {
                _dt = new DataTable();
                _dt.Columns.Add("Id", typeof(int));
                _dt.Columns.Add("الاسم", typeof(string));
                _dt.Columns.Add("الرمز", typeof(string));
                _dt.Columns.Add("النسبة", typeof(decimal));
                _dt.Columns.Add("النوع", typeof(string));
                _dt.Columns.Add("افتراضي", typeof(string));
                _dt.Columns.Add("نشط", typeof(string));

                foreach (var item in result.Value)
                {
                    string typeName = item.TaxType switch
                    {
                        1 => "نسبة مئوية",
                        2 => "مبلغ ثابت",
                        _ => "أخرى"
                    };

                    _dt.Rows.Add(item.Id, item.Name, item.Code, item.Rate,
                        typeName, item.IsDefault ? "نعم" : "لا",
                        item.IsActive ? "نعم" : "لا");
                }

                _BindGrid();
                lblRecordCount.Text = $"إجمالي الضرائب: {result.Value.Count}";
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل تحميل الضرائب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("خطأ: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Cursor = Cursors.Default;
    }

    private DataView? _dv;
    private void _BindGrid()
    {
        _dv = new DataView(_dt);
        dgvTaxes.DataSource = _dv;

        if (dgvTaxes.Columns.Contains("Id"))
            dgvTaxes.Columns["Id"].Visible = false;

        dgvTaxes.AutoResizeColumns();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        if (_dv == null) return;
        string search = txtSearch.Text.Trim();
        if (string.IsNullOrWhiteSpace(search))
            _dv.RowFilter = null;
        else
            _dv.RowFilter = $"الاسم LIKE '%{search.Replace("'", "''")}%' OR الرمز LIKE '%{search.Replace("'", "''")}%'";
    }

    private async void btnRefresh_Click(object sender, EventArgs e)
        => await _LoadDataAsync();

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void chkIncludeInactive_CheckedChanged(object sender, EventArgs e)
        => _ = _LoadDataAsync();

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateTax>();
        if (editor.ShowDialog() == DialogResult.OK)
            await _LoadDataAsync();
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        if (dgvTaxes.CurrentRow == null || dgvTaxes.CurrentRow.Cells["Id"].Value == null)
        {
            MessageBox.Show("الرجاء تحديد ضريبة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int id = Convert.ToInt32(dgvTaxes.CurrentRow.Cells["Id"].Value);
        var editor = _serviceProvider.GetRequiredService<frmAddUpdateTax>();
        editor._SetUpdateMode(id);
        if (editor.ShowDialog() == DialogResult.OK)
            await _LoadDataAsync();
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        if (dgvTaxes.CurrentRow == null || dgvTaxes.CurrentRow.Cells["Id"].Value == null)
        {
            MessageBox.Show("الرجاء تحديد ضريبة", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int id = Convert.ToInt32(dgvTaxes.CurrentRow.Cells["Id"].Value);
        string name = dgvTaxes.CurrentRow.Cells["الاسم"].Value?.ToString() ?? "";

        var confirm = MessageBox.Show($"هل أنت متأكد من حذف الضريبة {name}؟",
            "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        Cursor = Cursors.WaitCursor;
        var result = await _serviceProvider.GetRequiredService<ITaxesApiService>().DeleteAsync(id);
        Cursor = Cursors.Default;

        if (result.IsSuccess)
        {
            MessageBox.Show("تم حذف الضريبة بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await _LoadDataAsync();
        }
        else
        {
            MessageBox.Show(result.Error ?? "فشل حذف الضريبة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void dgvTaxes_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex < 0) return;
        btnEdit_Click(sender, e);
    }
}
