using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using ClosedXML.Excel;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmSalesReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "تقرير المبيعات";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmSalesReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;

        // القيم الابتدائية للفترة كانت تُضبط في تهيئة التبويب — تبقى كما هي.
        dtpTo.Value = DateTime.Today;
        dtpFrom.Value = DateTime.Today.AddMonths(-1);

        this.Load += async (s, e) => await LoadReportAsync();

        ReportUiStandardizer.ApplyForm(this);
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
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetSalesAsync(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1));

            if (result.IsSuccess)
            {
                var list = result.Value!.ToList();
                _bindingSource.DataSource = list;
                UpdateSummary(list);
                FormatGrid();
            }
            else
            {
                _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error!);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل تقرير المبيعات");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<SalesReportDto> list)
    {
        lblRecordsCount.Text = list.Count.ToString();

        if (list.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        var totalSales = list.Sum(x => x.TotalAmount);
        var totalPaid = list.Sum(x => x.PaidAmount);
        var totalDue = list.Sum(x => x.DueAmount);

        lblSummary.Text = $"عدد الفواتير: {list.Count} | إجمالي المبيعات: {totalSales:N2} | المدفوع: {totalPaid:N2} | المتبقي: {totalDue:N2}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        var hides = new[] { "InvoiceId", "WarehouseName", "SubTotal", "DiscountAmount", "TaxAmount" };
        foreach (var h in hides)
        {
            if (dgvReport.Columns.Contains(h)) dgvReport.Columns[h].Visible = false;
        }

        SetHeader("InvoiceNo", "رقم الفاتورة");
        SetHeader("InvoiceDate", "التاريخ");
        SetHeader("CustomerName", "العميل");
        SetHeader("TotalAmount", "الإجمالي");
        SetHeader("PaidAmount", "المدفوع");
        SetHeader("DueAmount", "المتبقي");

        foreach (var col in new[] { "TotalAmount", "PaidAmount", "DueAmount" })
        {
            if (dgvReport.Columns.Contains(col))
            {
                dgvReport.Columns[col].DefaultCellStyle.Format = "N2";
                dgvReport.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        if (dgvReport.Columns.Contains("InvoiceDate"))
            dgvReport.Columns["InvoiceDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
    }

    private void SetHeader(string col, string text)
    {
        if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text;
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<SalesReportDto> data || data.Count == 0)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("تقرير المبيعات");

            // Headers
            var headers = new[] { "رقم الفاتورة", "التاريخ", "العميل", "الإجمالي", "المدفوع", "المتبقي" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Data
            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];
                worksheet.Cell(row + 2, 1).Value = item.InvoiceNo;
                worksheet.Cell(row + 2, 2).Value = item.InvoiceDate.ToString("yyyy-MM-dd");
                worksheet.Cell(row + 2, 3).Value = item.CustomerName ?? "-";
                worksheet.Cell(row + 2, 4).Value = item.TotalAmount;
                worksheet.Cell(row + 2, 5).Value = item.PaidAmount;
                worksheet.Cell(row + 2, 6).Value = item.DueAmount;
            }

            worksheet.Columns().AdjustToContents();

            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"تقرير_المبيعات_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(dialog.FileName);
                _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تصدير تقرير المبيعات إلى Excel");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show)
    {
        loadingPanel.Visible = show;
        this.Enabled = !show;
    }
}
