using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmProductProfitabilityReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "أرباح الأصناف";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmProductProfitabilityReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
    }

    private void frmProductProfitabilityReport_Load(object sender, EventArgs e)
    {
        _ = LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<ISalesReportDesktopService>().GetSalesByProductAsync(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
            if (result.IsSuccess && result.Value != null)
            {
                _bindingSource.DataSource = result.Value.ToList();
                UpdateSummary(result.Value.ToList());
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل أرباح الأصناف"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<SalesByProductDto> list)
    {
        if (list.Count == 0) { lblRecordsCount.Text = "لا توجد بيانات"; return; }
        var sales = list.Sum(x => x.TotalAmount);
        var profit = list.Sum(x => x.TotalProfit);
        // RULE-N179 — كل مبلغ من البيت الواحد clsFormat
        lblRecordsCount.Text = $"إجمالي المبيعات: {clsFormat.Money(sales)} | صافي الربح: {clsFormat.Money(profit)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("ProductName", "المنتج"); SetHeader("Quantity", "الكمية"); SetHeader("TotalAmount", "المبيعات");
        SetHeader("TotalCost", "التكلفة"); SetHeader("TotalProfit", "الربح"); SetHeader("ProfitMargin", "نسبة الربح");
        foreach (var c in new[] { "Quantity" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N3";
        foreach (var c in new[] { "TotalAmount", "TotalCost", "TotalProfit", "ProfitMargin" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"أرباح_الأصناف_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "أرباح الأصناف", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير أرباح الأصناف"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show)
    {
        if (show) loadingPanel.BringToFront();
        loadingPanel.Visible = show;
        this.Enabled = !show;
        this.UseWaitCursor = show;
    }

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
}

