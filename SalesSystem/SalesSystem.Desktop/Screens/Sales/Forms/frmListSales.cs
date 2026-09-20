using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

public partial class frmListSales : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISalesInvoiceApiService _salesApi => _serviceProvider.GetRequiredService<ISalesInvoiceApiService>();

    private List<SalesInvoiceDto> _allInvoices = new();
    private List<SalesInvoiceDto> _filteredInvoices = new();
    private CancellationTokenSource? _refreshCts;

    public frmListSales(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _SetupGridColumns();
    }

    // ── Data Loading ───────────────────────────────────────────────────────────

    private async void _RefreshSalesList()
    {
        _refreshCts?.Cancel();
        _refreshCts?.Dispose();
        _refreshCts = new CancellationTokenSource();
        CancellationToken ct = _refreshCts.Token;

        Cursor = Cursors.WaitCursor;
        try
        {
            byte? status = cbStatus.SelectedIndex > 0 ? (byte?)cbStatus.SelectedIndex : null;
            var result = await _salesApi.GetAllAsync(
                null, dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1), status, ct);

            if (ct.IsCancellationRequested)
                return;

            if (result.IsSuccess && result.Value != null)
            {
                _allInvoices = result.Value.ToList();
            }
            else
            {
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error ?? "حدث خطأ في تحميل البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _allInvoices = new List<SalesInvoiceDto>();
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            if (ct.IsCancellationRequested)
                return;
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _allInvoices = new List<SalesInvoiceDto>();
        }
        finally
        {
            if (!ct.IsCancellationRequested && !IsDisposed)
            {
                _filteredInvoices = _allInvoices.ToList();
                _PopulateGrid();
                _UpdateRecordsCount();
                Cursor = Cursors.Default;
            }
        }
    }

    private void _PopulateGrid()
    {
        if (dgvSales.Columns.Count == 0) return;
        dgvSales.Rows.Clear();

        foreach (var inv in _filteredInvoices)
        {
            dgvSales.Rows.Add(
                inv.InvoiceNo,
                clsFormat.DateToShort(inv.InvoiceDate),
                inv.CustomerName ?? "عميل نقدي",
                inv.TotalAmount.ToString("N2"),
                inv.PaidAmount.ToString("N2"),
                inv.DueAmount.ToString("N2"),
                inv.Status switch { 1 => "مسودة", 2 => "مرحل", 3 => "ملغي", _ => "غير معروف" }
            );
        }
    }

    private void _ReloadForm()
    {
        _RefreshSalesList();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredInvoices.Count.ToString();
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvSales.Columns.Clear();

        dgvSales.Columns.Add("InvoiceNo", "رقم الفاتورة");
        dgvSales.Columns.Add("InvoiceDate", "التاريخ");
        dgvSales.Columns.Add("CustomerName", "العميل");
        dgvSales.Columns.Add("TotalAmount", "الإجمالي");
        dgvSales.Columns.Add("PaidAmount", "المدفوع");
        dgvSales.Columns.Add("DueAmount", "المتبقي");
        dgvSales.Columns.Add("StatusName", "الحالة");

        foreach (var col in dgvSales.Columns)
        {
            if (col is DataGridViewTextBoxColumn txtCol)
                txtCol.ReadOnly = true;
        }

        dgvSales.CellFormatting += dgvSales_CellFormatting;
    }

    private void dgvSales_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex == 6 && e.Value != null)
        {
            e.CellStyle.ForeColor = e.Value.ToString() switch
            {
                "مرحل" => Color.Green,
                "ملغي" => Color.Red,
                _ => Color.Blue
            };
            e.CellStyle.Font = new Font(dgvSales.Font, FontStyle.Bold);
        }
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private SalesInvoiceDto? _GetSelectedInvoice()
    {
        if (dgvSales.CurrentRow == null) return null;
        int rowIndex = dgvSales.CurrentRow.Index;
        if (rowIndex < 0 || rowIndex >= _filteredInvoices.Count) return null;
        return _filteredInvoices[rowIndex];
    }

    private void _OpenForm(Form form)
    {
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private async void _PostSelectedAsync()
    {
        var inv = _GetSelectedInvoice();
        if (inv == null) return;

        if (inv.Status != 1)
        {
            MessageBox.Show("يمكن ترحيل الفواتير المسودة فقط.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"هل تريد ترحيل الفاتورة رقم {inv.InvoiceNo}؟",
                "تأكيد الترحيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        try
        {
            var res = await _salesApi.PostAsync(inv.Id);
            if (res.IsSuccess)
            {
                MessageBox.Show("تم الترحيل بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshSalesList();
            }
            else
            {
                MessageBox.Show(res.Error ?? "حدث خطأ أثناء الترحيل.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء الترحيل: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void _CancelSelectedAsync()
    {
        var inv = _GetSelectedInvoice();
        if (inv == null) return;

        if (inv.Status == 3)
        {
            MessageBox.Show("الفاتورة ملغية بالفعل.", "تنبيه",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"هل تريد إلغاء الفاتورة رقم {inv.InvoiceNo}؟",
                "تأكيد الإلغاء", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            var res = await _salesApi.CancelAsync(inv.Id);
            if (res.IsSuccess)
            {
                MessageBox.Show("تم الإلغاء بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshSalesList();
            }
            else
            {
                MessageBox.Show(res.Error ?? "حدث خطأ أثناء الإلغاء.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء الإلغاء: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _refreshCts?.Cancel();
        _refreshCts?.Dispose();
        _refreshCts = null;
        base.OnFormClosed(e);
    }

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmListSales_Load(object sender, EventArgs e)
    {
        dtpFrom.Value = DateTime.Today.AddDays(-30);
        cbFilterBy.SelectedIndex = cbFilterBy.FindString("لا شيئ");
        cbStatus.SelectedIndex = 0;
        _ReloadForm();
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

        _filteredInvoices = _allInvoices.ToList();
        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtFilterValue.Text.Trim();

        if (string.IsNullOrEmpty(filterText) || cbFilterBy.Text == "لا شيئ")
        {
            _filteredInvoices = _allInvoices.ToList();
        }
        else
        {
            _filteredInvoices = new List<SalesInvoiceDto>();
            foreach (var inv in _allInvoices)
            {
                bool match = false;
                switch (cbFilterBy.Text)
                {
                    case "رقم الفاتورة":
                        match = inv.InvoiceNo.ToString().StartsWith(filterText);
                        break;
                    case "اسم العميل":
                        match = (inv.CustomerName ?? "عميل نقدي").Contains(filterText);
                        break;
                }
                if (match) _filteredInvoices.Add(inv);
            }
        }

        _PopulateGrid();
        _UpdateRecordsCount();
    }

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }

        bool isNumericFilter = cbFilterBy.Text == "رقم الفاتورة";

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

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        => _RefreshSalesList();

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
        => _RefreshSalesList();

    private void dtpTo_ValueChanged(object sender, EventArgs e)
        => _RefreshSalesList();

    // ── Grid Events ────────────────────────────────────────────────────────────

    private void dgvSales_DoubleClick(object sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv != null)
            btnView.PerformClick();
    }

    // ── Toolbar / Button Events ────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var editor = _serviceProvider.GetRequiredService<frmSalesInvoice>();
        editor.LoadData(null);
        _OpenForm(editor);
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv == null) return;

        var editor = _serviceProvider.GetRequiredService<frmSalesInvoice>();
        editor.LoadData(inv);
        _OpenForm(editor);
    }

    private void btnPost_Click(object sender, EventArgs e)
        => _PostSelectedAsync();

    private void btnCancel_Click(object sender, EventArgs e)
        => _CancelSelectedAsync();
}
