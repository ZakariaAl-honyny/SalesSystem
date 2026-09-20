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
            var result = await _serviceProvider.GetRequiredService<IFinancialReportService>().GetBalanceSheetAsync(dtpAsOf.Value.Date);
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
            else { MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل رأس المال العامل"); MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل التقرير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(WorkingCapitalSummaryDto report)
    {
        lblSummary.Text = $"الأصول المتداولة: {report.CurrentAssets:N2} | الخصوم المتداولة: {report.CurrentLiabilities:N2} | رأس المال العامل: {report.WorkingCapital:N2} | النسبة: {report.CurrentRatio:N2}";
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
        if (_bindingSource.DataSource == null) { MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        try
        {
            using var wb = new XLWorkbook(); var ws = wb.Worksheets.Add("رأس المال العامل");
            var headers = new[] { "كود الحساب", "اسم الحساب", "الرصيد", "النوع" };
            for (int i = 0; i < headers.Length; i++) { ws.Cell(1, i + 1).Value = headers[i]; ws.Cell(1, i + 1).Style.Font.Bold = true; }
            ws.Columns().AdjustToContents();
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"رأس_المال_العامل_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK) { wb.SaveAs(dlg.FileName); MessageBox.Show("تم تصدير التقرير بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير رأس المال العامل"); MessageBox.Show("حدث خطأ أثناء التصدير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void ShowLoading(bool show) { loadingPanel.Visible = show; this.Enabled = !show; }

    private void label1_Click(object sender, EventArgs e)
    {

    }
}
