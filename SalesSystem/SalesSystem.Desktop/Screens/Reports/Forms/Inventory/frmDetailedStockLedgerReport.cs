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

public partial class frmDetailedStockLedgerReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "دفتر الأستاذ التفصيلي للمخزون";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmDetailedStockLedgerReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
    }

    private async void frmDetailedStockLedgerReport_Load(object sender, EventArgs e)
    {
        await LoadReportAsync();
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
            // نهاية اليوم شاملة: ReportRepository.cs:376-377 يقارن CreatedAt (طابع زمني كامل) بـ to.Value.Date.AddDays(1).AddTicks(-1)، فتمرير .Date وحدها يُسقط حركات اليوم الأخير.
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetDetailedStockLedgerAsync(null, null, dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value), default);
            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
                _bindingSource.DataSource = list;
                UpdateSummary(list);
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل دفتر الأستاذ التفصيلي"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<DetailedStockLedgerDto> list)
    {
        lblRecordsCount.Text = clsFormat.Number(list.Count);
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("Date", "التاريخ"); SetHeader("ReferenceNo", "رقم المرجع"); SetHeader("ReferenceType", "نوع المرجع");
        SetHeader("MovementType", "نوع الحركة"); SetHeader("QuantityBefore", "قبل"); SetHeader("QuantityChange", "التغيير");
        SetHeader("QuantityAfter", "بعد"); SetHeader("UnitCost", "التكلفة"); SetHeader("TotalCost", "الإجمالي"); SetHeader("CreatedBy", "أنشأه");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Quantity, "QuantityBefore", "QuantityChange", "QuantityAfter");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Money, "UnitCost", "TotalCost");
        GridFormatHelper.ApplyColumn(dgvReport, "Date", GridFormatKind.DateDayMonthYear);
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"دفتر_الأستاذ_المخزون_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "دفتر الأستاذ التفصيلي", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير دفتر الأستاذ التفصيلي"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
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

