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

public partial class frmGeneralLedgerReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "دفتر الأستاذ العام";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmGeneralLedgerReport(IServiceProvider serviceProvider)
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
            var accountId = (int)nudAccountId.Value;
            if (accountId <= 0) { lblSummary.Text = "أدخل رقم الحساب أولاً"; return; }
            var result = await _serviceProvider.GetRequiredService<IFinancialReportDesktopService>().GetGeneralLedgerAsync(accountId, dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
            if (result.IsSuccess && result.Value != null)
            {
                var ledger = result.Value;
                var allItems = new List<object>();
                allItems.Add(new { Date = dtpFrom.Value.Date, EntryNumber = "", Description = $"الرصيد الافتتاحي: {ledger.AccountNameAr}", Debit = ledger.OpeningBalance, Credit = 0m, RunningBalance = ledger.OpeningBalance });
                foreach (var line in ledger.Lines)
                    allItems.Add(new { Date = line.Date, line.EntryNumber, line.Description, line.Debit, line.Credit, line.RunningBalance });
                allItems.Add(new { Date = dtpTo.Value.Date, EntryNumber = "", Description = "الإجمالي", Debit = ledger.TotalDebit, Credit = ledger.TotalCredit, RunningBalance = ledger.ClosingBalance });
                _bindingSource.DataSource = allItems;
                lblSummary.Text = $"الحساب: {ledger.AccountCode} - {ledger.AccountNameAr} | رصيد افتتاحي: {ledger.OpeningBalance:N2} | رصيد ختامي: {ledger.ClosingBalance:N2}";
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل دفتر الأستاذ العام"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("Date", "التاريخ"); SetHeader("EntryNumber", "رقم القيد"); SetHeader("Description", "البيان");
        SetHeader("Debit", "مدين"); SetHeader("Credit", "دائن"); SetHeader("RunningBalance", "الرصيد الجاري");
        foreach (var c in new[] { "Debit", "Credit", "RunningBalance" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"دفتر_الأستاذ_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "دفتر الأستاذ", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير دفتر الأستاذ"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show)
    {
        if (show) loadingPanel.BringToFront();
        loadingPanel.Visible = show;
        this.Enabled = !show;
        this.UseWaitCursor = show;
    }

    private void nudAccountId_ValueChanged(object sender, EventArgs e)
    {

    }
}

