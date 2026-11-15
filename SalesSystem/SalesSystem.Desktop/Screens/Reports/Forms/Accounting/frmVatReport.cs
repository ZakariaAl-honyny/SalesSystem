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

public partial class frmVatReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "تقرير ضريبة القيمة المضافة";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmVatReport(IServiceProvider serviceProvider)
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
            var result = await _serviceProvider.GetRequiredService<IFinancialReportDesktopService>().GetVatReportAsync(dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value), default);
            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
                _bindingSource.DataSource = list;
                UpdateSummary(list);
                FormatGrid();
            }
            else { clsDialogs.Error(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل تقرير الضريبة"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<VatReportDto> list)
    {
        if (list.Count == 0) { lblSummary.Text = "لا توجد بيانات"; return; }
        var total = list.Sum(x => x.TaxAmount);
        lblSummary.Text = $"عدد الفواتير: {clsFormat.Number(list.Count)} | إجمالي الضريبة: {clsFormat.Money(total)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("InvoiceNumber", "رقم الفاتورة"); SetHeader("InvoiceDate", "التاريخ"); SetHeader("PartyName", "الطرف");
        SetHeader("TaxableAmount", "الضريبية"); SetHeader("TaxRate", "النسبة"); SetHeader("TaxAmount", "الضريبة");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Money, "TaxableAmount", "TaxAmount");
        // عمود التاريخ من البيت الواحد — لا صيغة ثقافة الجهاز (RULE-N169).
        GridFormatHelper.ApplyColumn(dgvReport, "InvoiceDate", GridFormatKind.DateShort);
        // TaxRate نسبة على مقياس 0..100 (المبلغ الضريبي ÷ الوعاء × 100) — لا «Money» ولا «Percent»
        // لأن نمط النسبة في clsFormat هو "P" و.NET تضربه بـ100 مرة ثانية فتظهر 15% كـ1,500%.
        GridFormatHelper.ApplyColumn(dgvReport, "TaxRate", GridFormatKind.Ratio);
    }

    private void SetHeader(string col, string text) => ReportUiStandardizer.SetHeader(dgvReport, col, text);

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"تقرير_الضريبة_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "تقرير الضريبة", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير تقرير الضريبة"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show) => ReportUiStandardizer.ShowLoading(this, loadingPanel, show);
}
