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

public partial class frmSalesByProductReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "المبيعات حسب المنتج";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmSalesByProductReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
        this.Load += async (s, e) => await LoadReportAsync();
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
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
        // RULE-N169 — حكم نطاق التاريخ من بيته الواحد clsValidation، والرفض بصيغة عربية واحدة.
        if (!clsValidation.IsDateRangeValid(dtpFrom.Value, dtpTo.Value))
        {
            clsDialogs.Warning(clsValidation.DateRangeErrorMessage);
            return;
        }

        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<ISalesReportDesktopService>().GetSalesByProductAsync(dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value));
            if (result.IsSuccess && result.Value != null)
            {
                _bindingSource.DataSource = result.Value.ToList();
                UpdateSummary(result.Value.ToList());
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل المبيعات حسب المنتج"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<SalesByProductDto> list)
    {
        // عدد السجلات يبقى في تسميته label1 «عدد السجلات:» — كان يبقى على «0» من المصمّم فيكذب على الشاشة.
        lblRecordsCount.Text = clsFormat.Number(list.Count);

        if (list.Count == 0) { lblSummary.Text = "لا توجد بيانات"; return; }
        var total = list.Sum(x => x.TotalAmount);
        var profit = list.Sum(x => x.TotalProfit);
        // RULE-N179 — كل عدد ومبلغ من البيت الواحد clsFormat
        lblSummary.Text = $"عدد المنتجات: {clsFormat.Number(list.Count)} | الإجمالي: {clsFormat.Money(total)} | الربح: {clsFormat.Money(profit)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("ProductName", "اسم المنتج"); SetHeader("Quantity", "الكمية"); SetHeader("TotalAmount", "إجمالي المبيعات");
        SetHeader("TotalCost", "التكلفة"); SetHeader("TotalProfit", "الربح"); SetHeader("ProfitMargin", "نسبة الربح");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Quantity, "Quantity");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Money, "TotalAmount", "TotalCost", "TotalProfit");
        // ProfitMargin نسبة على مقياس 0..100 (الربح ÷ المبيعات × 100) — «Ratio» لا «Money» ولا «Percent»
        // لأن نمط النسبة في clsFormat هو "P" و.NET تضربه بـ100 مرة ثانية.
        GridFormatHelper.ApplyColumn(dgvReport, "ProfitMargin", GridFormatKind.Ratio);
    }

    private void SetHeader(string col, string text) => ReportUiStandardizer.SetHeader(dgvReport, col, text);

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"مبيعات_حسب_المنتج_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "المبيعات حسب المنتج", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير المبيعات حسب المنتج"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show) => ReportUiStandardizer.ShowLoading(this, loadingPanel, show);
}
