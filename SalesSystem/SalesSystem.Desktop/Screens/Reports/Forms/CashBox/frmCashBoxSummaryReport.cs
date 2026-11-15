using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmCashBoxSummaryReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "ملخص الصناديق النقدية";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmCashBoxSummaryReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
    }

    private void frmCashBoxSummaryReport_Load(object sender, EventArgs e)
    {
        // بلا async void: لا استثناء غير ملحوظ يصل إلى Application.ThreadException (نمط التقارير المرجعي).
        _ = LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);

            // نطاق البيانات: العقد يمنح تاريخاً واحداً (asOfDate) لا مدى، وافتراضه DateTime.Today
            // في BLL وMock معاً ⇒ لا يوجد «نطاق مقلوب» هنا، ولا يُصنع فلتر على العميل.
            var result = await _serviceProvider.GetRequiredService<ICashBoxReportDesktopService>().GetCashBoxSummaryAsync();

            if (!result.IsSuccess)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل ملخص الصناديق النقدية");
                return;
            }

            var list = result.Value?.ToList() ?? new List<CashBoxSummaryDto>();
            _bindingSource.DataSource = list;
            UpdateSummary(list);
            FormatGrid();
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل ملخص الصناديق النقدية"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    /// <summary>
    /// شريط الإجماليات: العدد والتاريخ من البيت الواحد <c>clsFormat</c> (<c>RULE-N169</c>) فلا تُتجاهل
    /// منازل الأعداد ولا الأرقام العربية ٠-٩ ولا صيغة التاريخ المحفوظة — بدل <c>ToString()</c> الخام.
    /// ويُعلن نطاق البيانات صراحةً (تراكمي حتى تاريخ اليوم) لأن العقد <c>GetCashBoxSummaryAsync(asOfDate)</c>
    /// افتراضه <c>DateTime.Today</c> — فلا يقرأ المستخدم الرقم على أنه حركة يوم واحد.
    /// </summary>
    private void UpdateSummary(List<CashBoxSummaryDto> list)
        => lblRecordsCount.Text =
            $"عدد الصناديق: {clsFormat.Number(list.Count)} — حتى تاريخ: {clsFormat.DateToShort(DateTime.Today)}";

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("CashBoxName", "اسم الصندوق"); SetHeader("TotalIncome", "الإيرادات");
        SetHeader("TotalExpense", "المصروفات"); SetHeader("NetBalance", "الصافي");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Money, "TotalIncome", "TotalExpense", "NetBalance");
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"ملخص_الصناديق_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "ملخص الصناديق", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير ملخص الصناديق"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show)
    {
        // النموذج قد يكون أُغلق أثناء انتظار الخدمة؛ لمس عنصر متخلَّص منه يرمي
        // ObjectDisposedException من finally فيسقط التحميل.
        if (IsDisposed || Disposing) return;

        if (show) loadingPanel.BringToFront();
        loadingPanel.Visible = show;
        this.Enabled = !show;
        this.UseWaitCursor = show;
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

