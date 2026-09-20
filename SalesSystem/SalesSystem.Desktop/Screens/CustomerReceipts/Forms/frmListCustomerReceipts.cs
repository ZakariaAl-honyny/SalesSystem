using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.CustomerReceipts.Forms;

public partial class frmListCustomerReceipts : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<CustomerReceiptDto> _allReceipts = new();
    private List<CustomerReceiptDto> _filteredReceipts = new();

    public frmListCustomerReceipts(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    private void _SetupGridColumns()
    {
        dgvReceipts.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",                Header = "رقم السند",     Fill = 8,  Min = 100 },
            new { Name = "ReceiptNo",         Header = "رقم السند",     Fill = 8,  Min = 100 },
            new { Name = "ReceiptDate",       Header = "التاريخ",       Fill = 12, Min = 120 },
            new { Name = "CustomerName",      Header = "العميل",        Fill = 15, Min = 150 },
            new { Name = "Amount",            Header = "المبلغ",        Fill = 10, Min = 100 },
            new { Name = "PaymentMethodName", Header = "طريقة الدفع",  Fill = 12, Min = 120 },
            new { Name = "CashBoxName",       Header = "الصندوق",       Fill = 12, Min = 120 },
            new { Name = "StatusName",        Header = "الحالة",        Fill = 10, Min = 100 },
        };

        foreach (var col in columns)
        {
            dgvReceipts.Columns.Add(new DataGridViewTextBoxColumn
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

    private async void _RefreshList()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;
        try
        {
            var result = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                _allReceipts = result.Value.ToList();
            }
            else
            {
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل سندات القبض", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _allReceipts = new List<CustomerReceiptDto>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء تحميل سندات القبض: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _filteredReceipts = _allReceipts.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateGrid()
    {
        dgvReceipts.AutoGenerateColumns = false;
        dgvReceipts.Rows.Clear();

        foreach (var r in _filteredReceipts)
        {
            dgvReceipts.Rows.Add(
                r.Id,
                r.ReceiptNo,
                r.ReceiptDate.ToString("yyyy-MM-dd"),
                r.CustomerName ?? "",
                r.Amount.ToString("N2"),
                r.PaymentMethodName ?? "",
                r.CashBoxName ?? "",
                r.StatusName ?? ""
            );
        }
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshList();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredReceipts.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvReceipts.CurrentRow == null) return -1;
        return (int)dgvReceipts.CurrentRow.Cells[0].Value;
    }

    private byte _GetSelectedStatus()
    {
        if (dgvReceipts.CurrentRow == null) return 0;
        var item = _filteredReceipts.FirstOrDefault(x => x.Id == _GetSelectedId());
        return item?.Status ?? 0;
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private void frmListCustomerReceipts_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        _OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>());
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
            _filteredReceipts = _allReceipts.ToList();
        else
            _filteredReceipts = _allReceipts.Where(r =>
                r.ReceiptNo.ToString().Contains(filterText) ||
                (r.CustomerName ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void postToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status != 1)
        {
            MessageBox.Show("لا يمكن ترحيل السند إلا في حالة مسودة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من ترحيل سند القبض؟", "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = PostAsync(id);
    }

    private async Task PostAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>().PostAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم ترحيل سند القبض بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الترحيل", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء الترحيل: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        byte status = _GetSelectedStatus();
        if (id <= 0) return;

        if (status == 3)
        {
            MessageBox.Show("السند ملغي بالفعل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show("هل أنت متأكد من إلغاء سند القبض؟", "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        _ = CancelAsync(id);
    }

    private async Task CancelAsync(int id)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var result = await _serviceProvider.GetRequiredService<ICustomerReceiptApiService>().CancelAsync(id);

            if (result.IsSuccess)
            {
                MessageBox.Show("تم إلغاء سند القبض بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReloadForm();
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في الإلغاء", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء الإلغاء: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void dgvReceipts_DoubleClick(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id > 0)
        {
            var frm = _serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>();
            frm._SetUpdateMode(id);
            _OpenForm(frm);
        }
    }

    private void cmsReceipts_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsReceipts.Enabled = dgvReceipts.Rows.Count > 0 && dgvReceipts.CurrentRow != null;
    }
}
