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

public partial class frmIncomeStatementReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "قائمة الدخل";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmIncomeStatementReport(IServiceProvider serviceProvider)
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
            var result = await _serviceProvider.GetRequiredService<IFinancialReportDesktopService>().GetIncomeStatementAsync(dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value), default);

            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
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
            Log.Error(ex, "خطأ في تحميل قائمة الدخل");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<IncomeStatementDto> list)
    {
        // T-43 (باك-إند مجمَّد — لا يُصلَح هنا): مصدر البيانات يصنّف السطور بنص عربي في Category لا بإشارة
        // المبلغ، وسطور الإجماليات تأتي داخل التصنيف نفسه؛ فجمع الإيرادات والمصروفات محلياً إمّا يضاعف
        // الإجماليات أو يخمّن إشارة غير موجودة. لذلك لا تُعرض مجاميع محسوبة هنا إطلاقاً (بدل أصفار كاذبة)،
        // ويُحال المستخدم إلى سطور الإجماليات التي يبنيها المصدر داخل الشبكة.
        lblRecordsCount.Text = list.Count == 0
            ? "لا توجد بيانات"
            : $"{clsFormat.Number(list.Count)} — المجاميع في سطور الإجماليات داخل الشبكة";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        SetHeader("Category", "التصنيف");
        SetHeader("Description", "البيان");
        SetHeader("Amount", "المبلغ");

        if (dgvReport.Columns.Contains("Amount"))
        {
            GridFormatHelper.ApplyColumn(dgvReport, "Amount", GridFormatKind.Money);
            dgvReport.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }
    }

    private void SetHeader(string col, string text)
    {
        if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text;
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<IncomeStatementDto> data || data.Count == 0)
        {
            clsDialogs.Warning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"قائمة_الدخل_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "قائمة الدخل", dialog.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تصدير قائمة الدخل");
            clsDialogs.Error("حدث خطأ أثناء التصدير.");
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

