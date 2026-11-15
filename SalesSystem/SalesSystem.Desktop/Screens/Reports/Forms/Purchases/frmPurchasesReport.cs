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

public partial class frmPurchasesReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "تقرير المشتريات";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmPurchasesReport(IServiceProvider serviceProvider)
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
            // نهاية اليوم شاملة: ReportRepository.cs:50,59 يطبّع الحد بنفسه إلى to.Date.AddDays(1).AddTicks(-1)، فالقيمة نهاية آخر لحظة في اليوم مطابقة له ولا تُسقط اليوم الأخير.
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetPurchasesAsync(dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value));

            if (result.IsSuccess)
            {
                var list = result.Value!.ToList();
                _bindingSource.DataSource = list;
                UpdateSummary(list);
                FormatGrid();
            }
            else
            {
                clsDialogs.Error(result.Error!);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل تقرير المشتريات");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<PurchaseReportDto> list)
    {
        lblRecordsCount.Text = clsFormat.Number(list.Count);

        if (list.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        var totalPurchases = list.Sum(x => x.TotalAmount);
        var totalPaid = list.Sum(x => x.PaidAmount);
        var totalDue = list.Sum(x => x.DueAmount);

        lblSummary.Text = $"عدد الفواتير: {clsFormat.Number(list.Count)} | إجمالي المشتريات: {clsFormat.Money(totalPurchases)} | المدفوع: {clsFormat.Money(totalPaid)} | المتبقي: {clsFormat.Money(totalDue)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        // أسماء الحقول كما في PurchaseReportDto: المعرّف هو Id (لا InvoiceId)، ورقم الفاتورة يُعرض عبر InvoiceNo.
        var hides = new[] { "Id", "SubTotal", "DiscountAmount", "TaxAmount" };
        foreach (var h in hides)
        {
            if (dgvReport.Columns.Contains(h)) dgvReport.Columns[h].Visible = false;
        }

        SetHeader("InvoiceNo", "رقم الفاتورة");
        SetHeader("InvoiceDate", "التاريخ");
        SetHeader("SupplierName", "المورد");
        SetHeader("TotalAmount", "الإجمالي");
        SetHeader("PaidAmount", "المدفوع");
        SetHeader("DueAmount", "المتبقي");

        foreach (var col in new[] { "TotalAmount", "PaidAmount", "DueAmount" })
        {
            if (dgvReport.Columns.Contains(col))
            {
                dgvReport.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        // RULE-N169 — تنسيق المبالغ والتاريخ من البيت الواحد بدل معالج CellFormatting محلي.
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Money, "TotalAmount", "PaidAmount", "DueAmount");
        GridFormatHelper.ApplyColumn(dgvReport, "InvoiceDate", GridFormatKind.DateShort);
    }

    private void SetHeader(string col, string text) => ReportUiStandardizer.SetHeader(dgvReport, col, text);

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<PurchaseReportDto> data || data.Count == 0)
        {
            clsDialogs.Warning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"تقرير_المشتريات_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "تقرير المشتريات", dialog.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير المشتريات إلى Excel");
            clsDialogs.Error("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show) => ReportUiStandardizer.ShowLoading(this, loadingPanel, show);
}
