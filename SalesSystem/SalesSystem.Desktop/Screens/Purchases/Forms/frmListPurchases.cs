using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Purchases;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

public partial class frmListPurchases : Form
{
    private readonly IServiceProvider _serviceProvider;
    private IPurchaseInvoiceApiService _purchaseApi => _serviceProvider.GetRequiredService<IPurchaseInvoiceApiService>();

    private List<PurchaseInvoiceDto> _allInvoices = new();
    private List<PurchaseInvoiceDto> _filteredInvoices = new();

    public frmListPurchases(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Data Loading ───────────────────────────────────────────────────────────

    private async void _RefreshPurchasesList()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            byte? status = cbStatus.SelectedIndex > 0 ? (byte?)cbStatus.SelectedIndex : null;
            var result = await _purchaseApi.GetAllAsync(
                null, dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1), status);

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
                _allInvoices = new List<PurchaseInvoiceDto>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _allInvoices = new List<PurchaseInvoiceDto>();
        }
        finally
        {
            _filteredInvoices = _allInvoices.ToList();
            _PopulateGrid();
            _UpdateRecordsCount();
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateGrid()
    {
        dgvPurchases.Rows.Clear();

        foreach (var inv in _filteredInvoices)
        {
            dgvPurchases.Rows.Add(
                inv.InvoiceNo,
                clsFormat.DateToShort(inv.InvoiceDate.ToDateTime(TimeOnly.MinValue)),
                inv.SupplierName ?? "",
                inv.TotalAmount.ToString("N2"),
                inv.PaidAmount.ToString("N2"),
                inv.DueAmount.ToString("N2"),
                inv.Status switch { 1 => "مسودة", 2 => "مرحل", 3 => "ملغي", _ => "غير معروف" }
            );
        }
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _RefreshPurchasesList();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredInvoices.Count.ToString();
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvPurchases.Columns.Clear();

        dgvPurchases.Columns.Add("InvoiceNo", "رقم الفاتورة");
        dgvPurchases.Columns.Add("InvoiceDate", "التاريخ");
        dgvPurchases.Columns.Add("SupplierName", "المورد");
        dgvPurchases.Columns.Add("TotalAmount", "الإجمالي");
        dgvPurchases.Columns.Add("PaidAmount", "المدفوع");
        dgvPurchases.Columns.Add("DueAmount", "المتبقي");
        dgvPurchases.Columns.Add("StatusName", "الحالة");

        foreach (var col in dgvPurchases.Columns)
        {
            if (col is DataGridViewTextBoxColumn txtCol)
                txtCol.ReadOnly = true;
        }

        dgvPurchases.CellFormatting += dgvPurchases_CellFormatting;
    }

    private void dgvPurchases_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex == 6 && e.Value != null)
        {
            e.CellStyle.ForeColor = e.Value.ToString() switch
            {
                "مرحل" => Color.Green,
                "ملغي" => Color.Red,
                _ => Color.Blue
            };
            e.CellStyle.Font = new Font(dgvPurchases.Font, FontStyle.Bold);
        }
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private PurchaseInvoiceDto? _GetSelectedInvoice()
    {
        if (dgvPurchases.CurrentRow == null) return null;
        int rowIndex = dgvPurchases.CurrentRow.Index;
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
            var res = await _purchaseApi.PostAsync(inv.Id);
            if (res.IsSuccess)
            {
                MessageBox.Show("تم الترحيل بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshPurchasesList();
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
            var res = await _purchaseApi.CancelAsync(inv.Id);
            if (res.IsSuccess)
            {
                MessageBox.Show("تم الإلغاء بنجاح", "نجاح",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshPurchasesList();
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

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmListPurchases_Load(object sender, EventArgs e)
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
            _filteredInvoices = new List<PurchaseInvoiceDto>();
            foreach (var inv in _allInvoices)
            {
                bool match = false;
                switch (cbFilterBy.Text)
                {
                    case "رقم الفاتورة":
                        match = inv.InvoiceNo.ToString().StartsWith(filterText);
                        break;
                    case "اسم المورد":
                        match = (inv.SupplierName ?? "").Contains(filterText);
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
        => _RefreshPurchasesList();

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
        => _RefreshPurchasesList();

    private void dtpTo_ValueChanged(object sender, EventArgs e)
        => _RefreshPurchasesList();

    // ── Grid Events ────────────────────────────────────────────────────────────

    private void dgvPurchases_DoubleClick(object sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv != null)
            btnView.PerformClick();
    }

    // ── Toolbar / Button Events ────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        var editor = _serviceProvider.GetRequiredService<frmPurchaseInvoice>();
        editor.LoadData(null);
        _OpenForm(editor);
    }

    private void btnView_Click(object sender, EventArgs e)
    {
        var inv = _GetSelectedInvoice();
        if (inv == null) return;

        var editor = _serviceProvider.GetRequiredService<frmPurchaseInvoice>();
        editor.LoadData(inv);
        _OpenForm(editor);
    }

    private void btnPost_Click(object sender, EventArgs e)
        => _PostSelectedAsync();

    private void btnCancel_Click(object sender, EventArgs e)
        => _CancelSelectedAsync();
}
