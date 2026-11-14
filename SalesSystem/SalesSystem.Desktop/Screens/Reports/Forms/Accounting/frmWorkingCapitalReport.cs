using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmWorkingCapitalReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "رأس المال العامل";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmWorkingCapitalReport(IServiceProvider serviceProvider)
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
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IFinancialReportDesktopService>().GetBalanceSheetAsync(dtpAsOf.Value.Date);
            if (result.IsSuccess && result.Value != null)
            {
                var bs = result.Value;
                var accounts = new List<WorkingCapitalAccountDto>();
                if (bs.Sections != null)
                    foreach (var section in bs.Sections)
                        if (section.Lines != null)
                            foreach (var line in section.Lines)
                                accounts.Add(new WorkingCapitalAccountDto(line.AccountName, line.AccountCode, line.Balance, section.Name));
                _bindingSource.DataSource = accounts;
                UpdateSummary(new WorkingCapitalSummaryDto(bs.TotalAssets, bs.TotalLiabilities, bs.TotalAssets - bs.TotalLiabilities, bs.TotalLiabilities > 0 ? bs.TotalAssets / bs.TotalLiabilities : 0, accounts));
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل رأس المال العامل"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(WorkingCapitalSummaryDto report)
    {
        // نسبة التداول معامل (أصول ÷ خصوم) لا نسبة مئوية — تُعرض بمنزلتين عبر منسّق المبالغ،
        // إذ لا يوجد منسّق «نسبة/معامل» في clsFormat بعد (RULE-N169: لا صيغة مكرّرة هنا).
        lblSummary.Text = $"الأصول المتداولة: {clsFormat.Money(report.CurrentAssets)} | الخصوم المتداولة: {clsFormat.Money(report.CurrentLiabilities)} | رأس المال العامل: {clsFormat.Money(report.WorkingCapital)} | نسبة التداول: {clsFormat.Money(report.CurrentRatio)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("AccountCode", "كود الحساب"); SetHeader("AccountName", "اسم الحساب"); SetHeader("Balance", "الرصيد"); SetHeader("Type", "النوع");
        if (dgvReport.Columns.Contains("Balance")) dgvReport.Columns["Balance"].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"رأس_المال_العامل_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "رأس المال العامل", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير رأس المال العامل"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show)
    {
        if (show) loadingPanel.BringToFront();
        loadingPanel.Visible = show;
        this.Enabled = !show;
        this.UseWaitCursor = show;
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }
}
