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

public partial class frmSupplierBalanceReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "كشف حساب موردين";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmSupplierBalanceReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        this.RightToLeft = RightToLeft.Yes;
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.BackgroundColor = Color.White;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.AutoGenerateColumns = true;
        dgvReport.CellFormatting += DgvReport_CellFormatting;
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
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetSupplierBalancesAsync(null);

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
            Log.Error(ex, "خطأ في تحميل تقرير أرصدة الموردين");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<SupplierBalanceReportDto> list)
    {
        lblRecordsCount.Text = clsFormat.Number(list.Count);
        if (list.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        var totalBalance = list.Sum(x => x.CurrentBalance);
        lblSummary.Text = $"عدد الموردين: {clsFormat.Number(list.Count)} | الرصيد الكلي: {clsFormat.Money(totalBalance)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        var hides = new[] { "SupplierId", "Phone" };
        foreach (var h in hides)
        {
            if (dgvReport.Columns.Contains(h)) dgvReport.Columns[h].Visible = false;
        }

        SetHeader("SupplierCode", "كود المورد");
        SetHeader("SupplierName", "اسم المورد");
        SetHeader("OpeningBalance", "الرصيد الافتتاحي");
        SetHeader("TotalPurchases", "إجمالي المشتريات");
        SetHeader("TotalPayments", "إجمالي المدفوعات");
        SetHeader("CurrentBalance", "الرصيد الحالي");

        foreach (var col in new[] { "OpeningBalance", "TotalPurchases", "TotalPayments", "CurrentBalance" })
        {
            if (dgvReport.Columns.Contains(col))
            {
                dgvReport.Columns[col].DefaultCellStyle.Format = "N2";
                dgvReport.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }
    }

    private void DgvReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex >= 0 && dgvReport.Columns[e.ColumnIndex].Name == "CurrentBalance")
        {
            var row = dgvReport.Rows[e.RowIndex];
            if (row.DataBoundItem is SupplierBalanceReportDto item)
            {
                if (item.CurrentBalance > 0)
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
                }
                else if (item.CurrentBalance < 0)
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
            }
        }
    }

    private void SetHeader(string col, string text)
    {
        if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text;
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<SupplierBalanceReportDto> data || data.Count == 0)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"كشف_حساب_موردين_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "كشف حساب موردين", dialog.FileName);
                _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تصدير تقرير أرصدة الموردين إلى Excel");
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
