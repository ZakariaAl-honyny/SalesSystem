using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using ClosedXML.Excel;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Business.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmAccountStatementReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "كشف حساب الطرف";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmAccountStatementReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
        this.Load += async (s, e) => await LoadReportAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnRefresh_Click(object? sender, EventArgs e)
    {
        _ = LoadReportAsync();
    }

    private void btnExport_Click(object? sender, EventArgs e)
    {
        _ = ExportToExcelAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            var partyId = (int)nudPartyId.Value;
            if (partyId <= 0) { lblSummary.Text = "أدخل رقم الطرف أولاً"; return; }
            IReadOnlyList<AccountStatementDto>? list = null;
            if (cmbPartyType.SelectedIndex == 0)
            {
                var result = await _serviceProvider.GetRequiredService<IFinancialReportService>().GetAccountStatementAsync(partyId, dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1), default);
                if (result.IsSuccess) list = result.Value;
                else { MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            else
            {
                var result = await _serviceProvider.GetRequiredService<IFinancialReportService>().GetSupplierStatementAsync(partyId, dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1), default);
                if (result.IsSuccess) list = result.Value;
                else { MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }
            if (list != null)
            {
                _bindingSource.DataSource = list.ToList();
                UpdateSummary(list.ToList());
                FormatGrid();
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل كشف حساب الطرف"); MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل التقرير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<AccountStatementDto> list)
    {
        if (list.Count == 0) { lblSummary.Text = "لا توجد بيانات"; return; }
        var debit = list.Sum(x => x.Debit);
        var credit = list.Sum(x => x.Credit);
        lblSummary.Text = $"عدد الحركات: {list.Count} | مدين: {debit:N2} | دائن: {credit:N2}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("Date", "التاريخ"); SetHeader("Description", "البيان"); SetHeader("ReferenceNumber", "رقم المرجع");
        SetHeader("Debit", "مدين"); SetHeader("Credit", "دائن"); SetHeader("Balance", "الرصيد");
        foreach (var c in new[] { "Debit", "Credit", "Balance" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource == null) { MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        try
        {
            using var wb = new XLWorkbook(); var ws = wb.Worksheets.Add("كشف حساب");
            var headers = new[] { "التاريخ", "البيان", "رقم المرجع", "مدين", "دائن", "الرصيد" };
            for (int i = 0; i < headers.Length; i++) { ws.Cell(1, i + 1).Value = headers[i]; ws.Cell(1, i + 1).Style.Font.Bold = true; }
            ws.Columns().AdjustToContents();
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"كشف_حساب_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK) { wb.SaveAs(dlg.FileName); MessageBox.Show("تم تصدير التقرير بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير كشف حساب"); MessageBox.Show("حدث خطأ أثناء التصدير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void ShowLoading(bool show) { loadingPanel.Visible = show; this.Enabled = !show; }

    private void loadingPanel_Paint(object sender, PaintEventArgs e)
    {

    }
}

