using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmSalesByCategoryReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "المبيعات حسب التصنيف";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmSalesByCategoryReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
    }

    private void frmSalesByCategoryReport_Load(object sender, EventArgs e)
    {
        _ = LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        // RULE-N169 — حكم نطاق التاريخ من بيته الواحد clsValidation، والرفض بصيغة عربية واحدة.
        if (!clsValidation.IsDateRangeValid(dtpFrom.Value, dtpTo.Value))
        {
            clsDialogs.Warning(clsValidation.DateRangeErrorMessage);
            return;
        }

        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<ISalesReportDesktopService>().GetSalesByCategoryAsync(dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value));
            if (result.IsSuccess && result.Value != null)
            {
                _bindingSource.DataSource = result.Value.ToList();
                UpdateSummary(result.Value.ToList());
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل المبيعات حسب التصنيف"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<SalesByCategoryDto> list)
    {
        if (list.Count == 0) { lblRecordsCount.Text = "لا توجد بيانات"; return; }
        var total = list.Sum(x => x.TotalAmount);
        // RULE-N179 — كل عدد ومبلغ من البيت الواحد clsFormat
        lblRecordsCount.Text = $"عدد التصنيفات: {clsFormat.Number(list.Count)} | الإجمالي: {clsFormat.Money(total)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("CategoryName", "التصنيف"); SetHeader("InvoiceCount", "عدد الفواتير"); SetHeader("TotalAmount", "الإجمالي");
        GridFormatHelper.ApplyColumn(dgvReport, "TotalAmount", GridFormatKind.Money);
    }

    private void SetHeader(string col, string text) => ReportUiStandardizer.SetHeader(dgvReport, col, text);

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"مبيعات_حسب_التصنيف_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "المبيعات حسب التصنيف", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير المبيعات حسب التصنيف"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show) => ReportUiStandardizer.ShowLoading(this, loadingPanel, show);

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
