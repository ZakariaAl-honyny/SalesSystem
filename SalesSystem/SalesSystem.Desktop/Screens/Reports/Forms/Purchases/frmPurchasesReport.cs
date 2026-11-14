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

        // RULE-N179: عرض الأرقام والتواريخ من البيت الواحد clsFormat — بلا صيغة حرفية في العمود.
        dgvReport.CellFormatting -= dgvReport_CellFormatting;
        dgvReport.CellFormatting += dgvReport_CellFormatting;

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
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetPurchasesAsync(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1));

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
            Log.Error(ex, "حدث خطأ في تحميل تقرير المشتريات");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
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

        var hides = new[] { "InvoiceId", "WarehouseName", "SubTotal", "DiscountAmount", "TaxAmount" };
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
    }

    /// <summary>
    /// RULE-N179 — كل رقم/تاريخ معروض في الشبكة يمرّ عبر البيت الواحد <see cref="clsFormat"/>،
    /// فلا تثبيت لصيغة حرفية في العمود. القيمة الأصلية (decimal/DateTime) تبقى في <c>Value</c>
    /// فيحتفظ الترتيب والتصدير بنوعيهما.
    /// </summary>
    private void dgvReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.Value is null or DBNull) return;

        string column = dgvReport.Columns[e.ColumnIndex].Name;

        if (e.Value is decimal amount && column is "TotalAmount" or "PaidAmount" or "DueAmount")
        {
            e.Value = clsFormat.Money(amount);
            e.FormattingApplied = true;
        }
        else if (e.Value is DateTime date && column == "InvoiceDate")
        {
            e.Value = clsFormat.DateToShort(date);
            e.FormattingApplied = true;
        }
    }

    private void SetHeader(string col, string text)
    {
        if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text;
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<PurchaseReportDto> data || data.Count == 0)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("لا توجد بيانات للتصدير");
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
                _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير المشتريات إلى Excel");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show)
    {
        if (show) loadingPanel.BringToFront();
        loadingPanel.Visible = show;
        this.Enabled = !show;
        this.UseWaitCursor = show;
    }
}
