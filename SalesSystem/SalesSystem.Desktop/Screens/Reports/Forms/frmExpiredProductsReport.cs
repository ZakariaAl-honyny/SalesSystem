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

public partial class frmExpiredProductsReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "المنتجات منتهية الصلاحية";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmExpiredProductsReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
    }

    private async void frmExpiredProductsReport_Load(object sender, EventArgs e)
    {
        await LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IReportService>().GetExpiredProductsReportAsync(0, default);
            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
                _bindingSource.DataSource = list;
                lblRecordsCount.Text = list.Count.ToString();
                FormatGrid();
            }
            else { MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل المنتجات منتهية الصلاحية"); MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل التقرير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally { ShowLoading(false); }
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("ProductName", "المنتج"); SetHeader("CategoryName", "التصنيف"); SetHeader("WarehouseName", "المستودع");
        SetHeader("CurrentStock", "الكمية"); SetHeader("ExpirationDate", "تاريخ الانتهاء"); SetHeader("DaysExpired", "الأيام المنتهية");
        if (dgvReport.Columns.Contains("ProductId")) dgvReport.Columns["ProductId"].Visible = false;
        if (dgvReport.Columns.Contains("CurrentStock")) dgvReport.Columns["CurrentStock"].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource == null) { MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        try
        {
            using var wb = new XLWorkbook(); var ws = wb.Worksheets.Add("المنتجات منتهية الصلاحية");
            var headers = new[] { "المنتج", "التصنيف", "المستودع", "الكمية", "تاريخ الانتهاء", "الأيام المنتهية" };
            for (int i = 0; i < headers.Length; i++) { ws.Cell(1, i + 1).Value = headers[i]; ws.Cell(1, i + 1).Style.Font.Bold = true; }
            ws.Columns().AdjustToContents();
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"منتجات_منتهية_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK) { wb.SaveAs(dlg.FileName); MessageBox.Show("تم تصدير التقرير بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير المنتجات منتهية الصلاحية"); MessageBox.Show("حدث خطأ أثناء التصدير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void ShowLoading(bool show) { loadingPanel.Visible = show; this.Enabled = !show; }

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

