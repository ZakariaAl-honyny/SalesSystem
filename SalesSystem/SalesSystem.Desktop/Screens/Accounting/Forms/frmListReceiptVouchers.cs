using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Screens.Accounting;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Responses;
using Serilog;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Accounting.Forms;

public partial class frmListReceiptVouchers : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<ReceiptVoucherDto> _allVouchers;
    private List<ReceiptVoucherDto> _filteredVouchers;

    public frmListReceiptVouchers(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _allVouchers = new List<ReceiptVoucherDto>();
        _filteredVouchers = new List<ReceiptVoucherDto>();
    }

    private void _SetupGridColumns()
    {
        dgvVouchers.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",           Header = "رقم السند",    Fill = 8,  Min = 80 },
            new { Name = "VoucherNo",    Header = "رقم الإشارة",  Fill = 8,  Min = 80 },
            new { Name = "VoucherDate",  Header = "التاريخ",      Fill = 12, Min = 110 },
            new { Name = "AccountName",  Header = "الحساب",       Fill = 15, Min = 150 },
            new { Name = "TotalAmount",  Header = "المبلغ",       Fill = 10, Min = 100 },
            new { Name = "StatusDisplay",Header = "الحالة",       Fill = 10, Min = 100 },
        };

        foreach (var col in columns)
        {
            dgvVouchers.Columns.Add(new DataGridViewTextBoxColumn
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
            var result = await _serviceProvider.GetRequiredService<IReceiptVoucherApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
                _allVouchers = result.Value.ToList();
            else if (!result.IsSuccess)
            {
                MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _allVouchers = new List<ReceiptVoucherDto>();
            }
            else
                _allVouchers = new List<ReceiptVoucherDto>();

            _filteredVouchers = _allVouchers.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error loading receipt vouchers.");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateGrid()
    {
        dgvVouchers.AutoGenerateColumns = false;
        dgvVouchers.Rows.Clear();

        foreach (var v in _filteredVouchers)
        {
            string statusText = v.Status switch
            {
                1 => "مسودة",
                2 => "تم الترحيل",
                3 => "ملغي",
                _ => "غير معروف"
            };

            dgvVouchers.Rows.Add(
                v.Id,
                v.VoucherNo,
                v.VoucherDate.ToString("yyyy-MM-dd"),
                v.AccountName ?? "",
                v.TotalAmount.ToString("N2"),
                statusText
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
        lblRecordsCount.Text = _filteredVouchers.Count.ToString();
    }

    private int _GetSelectedId()
    {
        if (dgvVouchers.CurrentRow == null) return -1;
        return (int)dgvVouchers.CurrentRow.Cells[0].Value;
    }

    private byte _GetSelectedStatus()
    {
        if (dgvVouchers.CurrentRow == null) return 0;
        var item = _filteredVouchers.FirstOrDefault(x => x.Id == _GetSelectedId());
        return item?.Status ?? 0;
    }

    private void frmListReceiptVouchers_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnRefresh_Click(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
            _filteredVouchers = _allVouchers.ToList();
        else
            _filteredVouchers = _allVouchers.Where(v =>
                v.VoucherNo.ToString().Contains(filterText) ||
                (v.AccountName ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void cmsVouchers_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        cmsVouchers.Enabled = dgvVouchers.Rows.Count > 0 && dgvVouchers.CurrentRow != null;
    }

    private void viewToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedId();
        if (id <= 0) return;
        MessageBox.Show("عرض تفاصيل سند القبض قيد التطوير.", "تحت التطوير", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var result = await _serviceProvider.GetRequiredService<IReceiptVoucherApiService>().PostAsync(id);

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
            Log.Error(ex, "Error posting receipt voucher {Id}.", id);
            MessageBox.Show("حدث خطأ في ترحيل السند. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var result = await _serviceProvider.GetRequiredService<IReceiptVoucherApiService>().CancelAsync(id);

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
            Log.Error(ex, "Error cancelling receipt voucher {Id}.", id);
            MessageBox.Show("حدث خطأ في إلغاء السند. يرجى المحاولة مرة أخرى.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
