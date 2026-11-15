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

public partial class frmDailyClosureReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "التقرير اليومي للصندوق";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmDailyClosureReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        _ShowDataScopeNotice();   // لافتة الصدق دائمة — تُضبط مرة واحدة قبل أي تحميل (RULE-N166)
        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
    }

    private void frmDailyClosureReport_Load(object sender, EventArgs e)
    {
        // بلا async void: لا استثناء غير ملحوظ يصل إلى Application.ThreadException (نمط التقارير المرجعي).
        _ = LoadReportAsync();
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);

            // لا عقد إقفال يومي بعد (T-41 مجمَّد) ⇒ نفس خدمة ملخص الصناديق. ولا يُصنع فلتر يومي على العميل.
            var result = await _serviceProvider.GetRequiredService<ICashBoxReportDesktopService>().GetCashBoxSummaryAsync();

            if (!result.IsSuccess)
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل التقرير اليومي للصندوق");
                return;
            }

            var list = result.Value?.ToList() ?? new List<CashBoxSummaryDto>();
            _bindingSource.DataSource = list;
            UpdateSummary(list);
            FormatGrid();
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل التقرير اليومي للصندوق"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    /// <summary>
    /// صدق نطاق البيانات (RULE-N166): لا يوجد عقد إقفال يومي في الباك-إند بعد، والخدمة المستدعاة
    /// هي ملخص الصناديق التراكمي حتى تاريخ اليوم — فتُعلن الشاشة ذلك صراحةً على شريط العنوان الموجود
    /// في المصمّم (بلا عنصر جديد — RULE-N145) **دائماً**: يُضبط في المُنشئ مرة واحدة، فلا يضيع في
    /// مسار الخطأ/الاستثناء ولا يعتمد على نجاح التحميل. واسم التقرير يبقى في شريط عنوان النافذة
    /// وفي شريط تنقّل التقارير.
    /// </summary>
    private void _ShowDataScopeNotice()
        => lblMode.Text = "تنبيه: هذا التقرير يعرض ملخص الصناديق الإجمالي حتى يُعتمد عقد الإقفال اليومي في الباك-إند.";

    /// <summary>
    /// شريط الإجماليات: العدد والتاريخ من البيت الواحد <c>clsFormat</c> (<c>RULE-N169</c>) بدل
    /// <c>ToString()</c> الخام — فيتبع منازل الأعداد والأرقام العربية ٠-٩ وصيغة التاريخ المحفوظة.
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
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"تقرير_يومي_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "التقرير اليومي", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير التقرير اليومي"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
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

