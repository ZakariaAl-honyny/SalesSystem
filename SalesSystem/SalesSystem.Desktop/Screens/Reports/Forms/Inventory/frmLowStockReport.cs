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

public partial class frmLowStockReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "تنبيه المخزون المنخفض";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmLowStockReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        this.RightToLeft = RightToLeft.Yes;
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.AutoGenerateColumns = true;
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadReportAsync();
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();

    private void btnRefresh_Click(object? sender, EventArgs e) => _ = LoadReportAsync();

    private void btnExport_Click(object? sender, EventArgs e) => _ = ExportToExcelAsync();

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetLowStockAsync();

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
            Log.Error(ex, "حدث خطأ في تحميل تقرير التنبيهات للمخزون المنخفض");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<LowStockReportDto> list)
    {
        lblRecordsCount.Text = clsFormat.Number(list.Count);
        if (list.Count == 0)
        {
            lblSummary.Text = "جميع الأصناف فوق الحد الأدنى للمخزون";
            return;
        }

        var totalQty = list.Sum(x => x.Quantity);
        lblSummary.Text = $"عدد الأصناف أقل من الحد الأدنى: {clsFormat.Number(list.Count)} | إجمالي الكمية: {clsFormat.Quantity(totalQty)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        // الأعمدة المخفية: المعرّف · «كود المنتج» (يعيده الـDTO بقيمة null دائماً) · العمودان
        // الخامان المكرّران لنفس كمية «Quantity»/«ReorderLevel» المحسوبين · معامل التحويل (رقم تقني).
        var hides = new[] { "ProductId", "ProductCode", "CurrentRetailQty", "ReorderLevelRetailQty", "ConversionFactor" };
        foreach (var h in hides)
        {
            if (dgvReport.Columns.Contains(h)) dgvReport.Columns[h].Visible = false;
        }

        SetHeader("ProductName", "اسم المنتج");
        SetHeader("CategoryName", "التصنيف");
        SetHeader("UnitName", "الوحدة");
        SetHeader("WarehouseName", "المستودع");
        SetHeader("Quantity", "الكمية الحالية");
        SetHeader("ReorderLevel", "الحد الأدنى");
        SetHeader("DeficitRetailQty", "النقص");
        SetHeader("SuggestedWholesaleBoxes", "المقترح (وحدات كبرى)");
        SetHeader("SuggestedRetailRemainder", "المتبقي (وحدات صغرى)");
        SetHeader("WholesaleUnitName", "الوحدة الكبرى");
        SetHeader("RetailUnitName", "الوحدة الصغرى");

        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Quantity,
            "Quantity", "ReorderLevel", "DeficitRetailQty", "SuggestedWholesaleBoxes", "SuggestedRetailRemainder");

        // الكمية أقل من الحد الأدنى تُلوَّن حمراء — من البيت الواحد بدل معالج تلوين محلي (RULE-N169).
        GridFormatHelper.ApplyBalanceColouring(dgvReport, "Quantity", "ReorderLevel");
    }

    private void SetHeader(string col, string text)
    {
        if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text;
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<LowStockReportDto> data || data.Count == 0)
        {
            clsDialogs.Warning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"تنبيه_المخزون_المنخفض_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "تنبيه المخزون المنخفض", dialog.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير التنبيهات للمخزون المنخفض إلى Excel");
            clsDialogs.Error("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
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
