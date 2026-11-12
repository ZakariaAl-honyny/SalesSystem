using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using ClosedXML.Excel;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmCustomerBalanceReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "كشف حساب عملاء";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    private IReadOnlyList<CustomerDto> _customers = new List<CustomerDto>();

    public frmCustomerBalanceReport(IServiceProvider serviceProvider)
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
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await LoadCustomersAsync();
        await LoadReportAsync();
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();

    private void btnRefresh_Click(object? sender, EventArgs e) => _ = LoadReportAsync();

    private void btnExport_Click(object? sender, EventArgs e) => _ = ExportToExcelAsync();

    private async Task LoadCustomersAsync()
    {
        try
        {
            var custResult = await _serviceProvider.GetRequiredService<ICustomerDesktopService>().GetAllAsync(null, true);
            if (custResult.IsSuccess)
            {
                _customers = custResult.Value;
                cmbCustomer.Items.Clear();
                cmbCustomer.Items.Add(new ComboBoxItem { Text = "جميع العملاء", Value = null });
                foreach (var cust in _customers)
                {
                    cmbCustomer.Items.Add(new ComboBoxItem { Text = cust.Name, Value = cust.Id });
                }
                cmbCustomer.SelectedIndex = 0;
            }
            else
            {
                _serviceProvider.GetRequiredService<INotificationService>().ShowError(custResult.Error ?? "حدث خطأ في تحميل قائمة العملاء");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل قائمة العملاء");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء تحميل قائمة العملاء.");
        }
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            int? customerId = GetSelectedCustomerId();
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetCustomerBalancesAsync(customerId);

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
            Log.Error(ex, "حدث خطأ في تحميل تقرير أرصدة العملاء");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private int? GetSelectedCustomerId()
    {
        if (cmbCustomer.SelectedItem is ComboBoxItem item)
        {
            return item.Value as int?;
        }
        return null;
    }

    private void UpdateSummary(List<CustomerBalanceReportDto> list)
    {
        lblRecordsCount.Text = list.Count.ToString();
        if (list.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        var totalBalance = list.Sum(x => x.CurrentBalance);
        var totalSales = list.Sum(x => x.TotalSales);
        var totalPayments = list.Sum(x => x.TotalPayments);

        lblSummary.Text = $"عدد العملاء: {list.Count} | إجمالي المبيعات: {totalSales:N2} | إجمالي المدفوعات: {totalPayments:N2} | الرصيد: {totalBalance:N2}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        var hides = new[] { "CustomerId", "Phone" };
        foreach (var h in hides)
        {
            if (dgvReport.Columns.Contains(h)) dgvReport.Columns[h].Visible = false;
        }

        SetHeader("CustomerCode", "كود العميل");
        SetHeader("CustomerName", "اسم العميل");
        SetHeader("OpeningBalance", "الرصيد الافتتاحي");
        SetHeader("TotalSales", "إجمالي المبيعات");
        SetHeader("TotalPayments", "إجمالي المدفوعات");
        SetHeader("CurrentBalance", "الرصيد الحالي");

        foreach (var col in new[] { "OpeningBalance", "TotalSales", "TotalPayments", "CurrentBalance" })
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
            if (row.DataBoundItem is CustomerBalanceReportDto item)
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
        if (_bindingSource.DataSource is not List<CustomerBalanceReportDto> data || data.Count == 0)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("كشف حساب عملاء");

            var headers = new[] { "كود العميل", "اسم العميل", "الرصيد الافتتاحي", "إجمالي المبيعات", "إجمالي المدفوعات", "الرصيد الحالي" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];
                worksheet.Cell(row + 2, 1).Value = item.CustomerCode ?? "-";
                worksheet.Cell(row + 2, 2).Value = item.CustomerName;
                worksheet.Cell(row + 2, 3).Value = item.OpeningBalance;
                worksheet.Cell(row + 2, 4).Value = item.TotalSales;
                worksheet.Cell(row + 2, 5).Value = item.TotalPayments;
                worksheet.Cell(row + 2, 6).Value = item.CurrentBalance;

                if (item.CurrentBalance > 0)
                {
                    var cell = worksheet.Cell(row + 2, 6);
                    cell.Style.Font.FontColor = XLColor.Red;
                }
            }

            worksheet.Columns().AdjustToContents();

            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"كشف_حساب_عملاء_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(dialog.FileName);
                _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير أرصدة العملاء إلى Excel");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show)
    {
        loadingPanel.Visible = show;
        this.Enabled = !show;
    }
}
