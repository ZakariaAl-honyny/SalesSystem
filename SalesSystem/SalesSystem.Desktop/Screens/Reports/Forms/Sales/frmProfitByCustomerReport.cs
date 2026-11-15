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

public partial class frmProfitByCustomerReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;

    // العنوان «الربح حسب العميل» مملوك لقائمة التقارير (ReportNavigationHelper.Catalog في Helpers/ — للقراءة فقط هنا)،
    // وعقد البيانات SalesByCustomerDto لا يحمل حقل ربح لكل عميل؛ فلا يُخترع حقل ولا ربح محلي:
    // الشاشة تُظهر النقص في lblContractNote وتعرض المستحق (DueAmount) بصدق في شريط الإجماليات.
    public string GetReportName() => "الربح حسب العميل";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmProfitByCustomerReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
    }

    private void frmProfitByCustomerReport_Load(object sender, EventArgs e)
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
            var result = await _serviceProvider.GetRequiredService<ISalesReportDesktopService>().GetSalesByCustomerAsync(dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value));
            if (result.IsSuccess && result.Value != null)
            {
                _bindingSource.DataSource = result.Value.ToList();
                UpdateSummary(result.Value.ToList());
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل الربح حسب العميل"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally
        {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<SalesByCustomerDto> list)
    {
        if (list.Count == 0) { lblRecordsCount.Text = "لا توجد بيانات"; return; }

        // عقد البيانات SalesByCustomerDto لا يحمل حقل ربح لكل عميل، فالمعروض هو
        // «المستحق» (DueAmount) منقولاً بصدق — لا ربحاً محسوباً بمعادلة محلية
        // (كان الحساب الميت profit = TotalAmount - PaidAmount مرفوضاً: المستحق ليس ربحاً).
        var sales = list.Sum(x => x.TotalAmount);
        var due = list.Sum(x => x.DueAmount);
        // RULE-N179 — كل مبلغ من البيت الواحد clsFormat
        lblRecordsCount.Text = $"إجمالي المبيعات: {clsFormat.Money(sales)} | المستحق: {clsFormat.Money(due)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("CustomerName", "العميل"); SetHeader("TotalAmount", "المبيعات"); SetHeader("PaidAmount", "المدفوع");
        SetHeader("DueAmount", "المستحق"); SetHeader("InvoiceCount", "عدد الفواتير");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Money, "TotalAmount", "PaidAmount", "DueAmount");
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"ربح_حسب_العميل_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "الربح حسب العميل", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير الربح حسب العميل"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
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

