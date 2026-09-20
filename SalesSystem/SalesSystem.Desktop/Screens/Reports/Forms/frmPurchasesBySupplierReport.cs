using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using ClosedXML.Excel;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Business.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmPurchasesBySupplierReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "المشتريات حسب المورد";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmPurchasesBySupplierReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
    }

    private void frmPurchasesBySupplierReport_Load(object sender, EventArgs e)
    {
        _ = LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IPurchaseReportService>().GetPurchasesBySupplierAsync(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
            if (result.IsSuccess && result.Value != null)
            {
                _bindingSource.DataSource = result.Value.ToList();
                UpdateSummary(result.Value.ToList());
                FormatGrid();
            }
            else { MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل المشتريات حسب المورد"); MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل التقرير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<PurchasesBySupplierDto> list)
    {
        if (list.Count == 0) { lblRecordsCount.Text = "لا توجد بيانات"; return; }
        var total = list.Sum(x => x.TotalAmount);
        var due = list.Sum(x => x.DueAmount);
        lblRecordsCount.Text = $"عدد الموردين: {list.Count} | الإجمالي: {total:N2} | المتبقي: {due:N2}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("SupplierName", "اسم المورد"); SetHeader("InvoiceCount", "عدد الفواتير"); SetHeader("TotalAmount", "الإجمالي");
        SetHeader("PaidAmount", "المدفوع"); SetHeader("DueAmount", "المتبقي");
        foreach (var c in new[] { "TotalAmount", "PaidAmount", "DueAmount" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource == null) { MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        try
        {
            using var wb = new XLWorkbook(); var ws = wb.Worksheets.Add("المشتريات حسب المورد");
            var headers = new[] { "اسم المورد", "عدد الفواتير", "الإجمالي", "المدفوع", "المتبقي" };
            for (int i = 0; i < headers.Length; i++) { ws.Cell(1, i + 1).Value = headers[i]; ws.Cell(1, i + 1).Style.Font.Bold = true; }
            ws.Columns().AdjustToContents();
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"مشتريات_حسب_المورد_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK) { wb.SaveAs(dlg.FileName); MessageBox.Show("تم تصدير التقرير بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير المشتريات حسب المورد"); MessageBox.Show("حدث خطأ أثناء التصدير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void ShowLoading(bool show) { 
        //loadingPanel.Visible = show; this.Enabled = !show;
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
