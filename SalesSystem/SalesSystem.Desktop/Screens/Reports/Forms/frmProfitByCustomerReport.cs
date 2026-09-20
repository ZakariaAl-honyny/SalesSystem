using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using ClosedXML.Excel;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Business.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmProfitByCustomerReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "الربح حسب العميل";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmProfitByCustomerReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
    }

    private void frmProfitByCustomerReport_Load(object sender, EventArgs e)
    {
        _ = LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            //ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<ISalesReportService>().GetSalesByCustomerAsync(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
            if (result.IsSuccess && result.Value != null)
            {
                _bindingSource.DataSource = result.Value.ToList();
                UpdateSummary(result.Value.ToList());
                FormatGrid();
            }
            else { MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل الربح حسب العميل"); MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل التقرير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally
        {
            //ShowLoading(false);
        }
    }

    private void UpdateSummary(List<SalesByCustomerDto> list)
    {
        if (list.Count == 0) { lblRecordsCount.Text = "لا توجد بيانات"; return; }
        var sales = list.Sum(x => x.TotalAmount);
        var profit = list.Sum(x => x.TotalAmount - x.PaidAmount);
        lblRecordsCount.Text = $"إجمالي المبيعات: {sales:N2}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("CustomerName", "العميل"); SetHeader("TotalAmount", "المبيعات"); SetHeader("PaidAmount", "المدفوع");
        SetHeader("DueAmount", "المتبقي"); SetHeader("InvoiceCount", "عدد الفواتير");
        foreach (var c in new[] { "TotalAmount", "PaidAmount", "DueAmount" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource == null) { MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        try
        {
            using var wb = new XLWorkbook(); var ws = wb.Worksheets.Add("الربح حسب العميل");
            var headers = new[] { "العميل", "المبيعات", "التكلفة", "الربح", "نسبة الربح", "عدد الفواتير" };
            for (int i = 0; i < headers.Length; i++) { ws.Cell(1, i + 1).Value = headers[i]; ws.Cell(1, i + 1).Style.Font.Bold = true; }
            ws.Columns().AdjustToContents();
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"ربح_حسب_العميل_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK) { wb.SaveAs(dlg.FileName); MessageBox.Show("تم تصدير التقرير بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير الربح حسب العميل"); MessageBox.Show("حدث خطأ أثناء التصدير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    //private void ShowLoading(bool show) { loadingPanel.Visible = show; this.Enabled = !show; }

    private void btnRefresh_Click(object? sender, EventArgs e)
    {
        _ = LoadReportAsync();
    }

    private void btnExport_Click(object? sender, EventArgs e)
    {
        _ = ExportToExcelAsync();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void label3_Click(object sender, EventArgs e)
    {

    }
}

