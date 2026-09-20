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

public partial class frmDetailedStockLedgerReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "دفتر الأستاذ التفصيلي للمخزون";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmDetailedStockLedgerReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
    }

    private async void frmDetailedStockLedgerReport_Load(object sender, EventArgs e)
    {
        await LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IReportService>().GetDetailedStockLedgerAsync(null, null, dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1), default);
            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
                _bindingSource.DataSource = list;
                UpdateSummary(list);
                FormatGrid();
            }
            else { MessageBox.Show(result.Error!, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل دفتر الأستاذ التفصيلي"); MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل التقرير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<DetailedStockLedgerDto> list)
    {
        if (list.Count == 0) { lblRecordsCount.Text = "0"; return; }
        lblRecordsCount.Text = list.Count.ToString();
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("Date", "التاريخ"); SetHeader("ReferenceNo", "رقم المرجع"); SetHeader("ReferenceType", "نوع المرجع");
        SetHeader("MovementType", "نوع الحركة"); SetHeader("QuantityBefore", "قبل"); SetHeader("QuantityChange", "التغيير");
        SetHeader("QuantityAfter", "بعد"); SetHeader("UnitCost", "التكلفة"); SetHeader("TotalCost", "الإجمالي"); SetHeader("CreatedBy", "أنشأه");
        foreach (var c in new[] { "QuantityBefore", "QuantityChange", "QuantityAfter", "UnitCost", "TotalCost" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource == null) { MessageBox.Show("لا توجد بيانات للتصدير", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        try
        {
            using var wb = new XLWorkbook(); var ws = wb.Worksheets.Add("دفتر الأستاذ التفصيلي");
            var headers = new[] { "التاريخ", "رقم المرجع", "نوع المرجع", "نوع الحركة", "قبل", "التغيير", "بعد", "التكلفة", "الإجمالي" };
            for (int i = 0; i < headers.Length; i++) { ws.Cell(1, i + 1).Value = headers[i]; ws.Cell(1, i + 1).Style.Font.Bold = true; }
            ws.Columns().AdjustToContents();
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"دفتر_الأستاذ_المخزون_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK) { wb.SaveAs(dlg.FileName); MessageBox.Show("تم تصدير التقرير بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير دفتر الأستاذ التفصيلي"); MessageBox.Show("حدث خطأ أثناء التصدير.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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

