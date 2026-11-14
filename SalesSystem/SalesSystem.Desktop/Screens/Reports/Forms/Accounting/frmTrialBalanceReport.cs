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

public partial class frmTrialBalanceReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "ميزان المراجعة";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmTrialBalanceReport(IServiceProvider serviceProvider)
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
            var result = await _serviceProvider.GetRequiredService<IFinancialReportDesktopService>().GetTrialBalanceAsync(dtpAsOf.Value.Date);
            if (result.IsSuccess && result.Value != null)
            {
                _bindingSource.DataSource = result.Value.ToList();
                UpdateSummary(result.Value.ToList());
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل ميزان المراجعة"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<TrialBalanceDto> list)
    {
        if (list.Count == 0) { lblSummary.Text = "لا توجد بيانات"; return; }
        var totalDebit = list.Sum(x => x.ClosingDebit);
        var totalCredit = list.Sum(x => x.ClosingCredit);
        var balanced = Math.Abs(totalDebit - totalCredit) < 0.01m;
        lblSummary.Text = $"إجمالي المدين: {clsFormat.Money(totalDebit)} | إجمالي الدائن: {clsFormat.Money(totalCredit)} | {(balanced ? "متوازن" : "غير متوازن")}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("AccountCode", "كود الحساب"); SetHeader("AccountName", "اسم الحساب");
        SetHeader("OpeningDebit", "افتتاحي مدين"); SetHeader("OpeningCredit", "افتتاحي دائن");
        SetHeader("TransactionDebit", "حركات مدين"); SetHeader("TransactionCredit", "حركات دائن");
        SetHeader("ClosingDebit", "ختامي مدين"); SetHeader("ClosingCredit", "ختامي دائن");
        SetHeader("AccountTypeLabel", "النوع");
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"ميزان_المراجعة_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "ميزان المراجعة", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير ميزان المراجعة"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show)
    {
        if (show) loadingPanel.BringToFront();
        loadingPanel.Visible = show;
        this.Enabled = !show;
        this.UseWaitCursor = show;
    }
}
