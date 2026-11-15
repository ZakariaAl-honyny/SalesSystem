using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.GlobalClasses;
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

    // يمنع إعادة التحميل أثناء تعبئة قائمة العملاء (SelectedIndexChanged الناتج عن التعبئة).
    private bool _suppressCustomerReload;

    public frmCustomerBalanceReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        this.RightToLeft = RightToLeft.Yes;
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.AutoGenerateColumns = true;

        // RULE-N59 — تصفية العميل فورية: كانت القائمة معروضة بلا معالج فلا يحدث شيء عند الاختيار.
        cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;

        ReportUiStandardizer.ApplyForm(this);
        ReportUiStandardizer.ApplyReferenceLook(this);
        ReportNavigationHelper.Attach(this, _serviceProvider);
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

    private void cmbCustomer_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressCustomerReload) return;
        _ = LoadReportAsync();   // LoadReportAsync يحمي نفسه بـ try/catch/finally
    }

    private async Task LoadCustomersAsync()
    {
        try
        {
            var custResult = await _serviceProvider.GetRequiredService<ICustomerDesktopService>().GetAllAsync(null, true);
            if (custResult.IsSuccess)
            {
                _customers = custResult.Value;

                _suppressCustomerReload = true;
                try
                {
                    cmbCustomer.Items.Clear();
                    cmbCustomer.Items.Add(new ComboBoxItem { Text = "جميع العملاء", Value = null });
                    foreach (var cust in _customers)
                    {
                        cmbCustomer.Items.Add(new ComboBoxItem { Text = cust.Name, Value = cust.Id });
                    }
                    cmbCustomer.SelectedIndex = 0;
                }
                finally
                {
                    _suppressCustomerReload = false;
                }
            }
            else
            {
                clsDialogs.Error(custResult.Error ?? "حدث خطأ في تحميل قائمة العملاء");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل قائمة العملاء");
            clsDialogs.Error("حدث خطأ أثناء تحميل قائمة العملاء.");
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
                clsDialogs.Error(result.Error!);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل تقرير أرصدة العملاء");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
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
        lblRecordsCount.Text = clsFormat.Number(list.Count);
        if (list.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        // الرصيد الحالي فقط مبلغ حقيقي في CustomerBalanceReportDto؛
        // OpeningBalance/TotalSales/TotalPayments خصائص توافق قديم تُعيد 0m دائماً،
        // فجمعها كان يُظهر «0.00» كإجمالي مبيعات/مدفوعات (رقم كاذب).
        var totalBalance = list.Sum(x => x.CurrentBalance);

        lblSummary.Text = $"عدد العملاء: {clsFormat.Number(list.Count)} | إجمالي الرصيد الحالي: {clsFormat.Money(totalBalance)}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        // أسماء الحقول كما في CustomerBalanceReportDto (المعرّف Id لا CustomerId)،
        // وتُخفى الأعمدة المكرّرة والقيم الثابتة صفراً بدل عرضها كأرقام حقيقية.
        var hides = new[] { "Id", "Phone", "Name", "OpeningBalance", "TotalSales", "TotalPayments" };
        foreach (var h in hides)
        {
            if (dgvReport.Columns.Contains(h)) dgvReport.Columns[h].Visible = false;
        }

        SetHeader("CustomerCode", "كود العميل");
        SetHeader("CustomerName", "اسم العميل");
        SetHeader("GroupName", "المجموعة");
        SetHeader("CreditLimit", "حد الائتمان");
        SetHeader("CurrentBalance", "الرصيد الحالي");
        SetHeader("BalanceStatus", "حالة الرصيد");

        foreach (var col in new[] { "CreditLimit", "CurrentBalance" })
        {
            if (dgvReport.Columns.Contains(col))
            {
                GridFormatHelper.ApplyColumn(dgvReport, col, GridFormatKind.Money);
                dgvReport.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        // سالب أحمر وموجب أخضر — من البيت الواحد بدل معالج تلوين محلي (RULE-N169).
        GridFormatHelper.ApplyBalanceColouring(dgvReport, "CurrentBalance");
    }

    private void SetHeader(string col, string text) => ReportUiStandardizer.SetHeader(dgvReport, col, text);

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<CustomerBalanceReportDto> data || data.Count == 0)
        {
            clsDialogs.Warning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"كشف_حساب_عملاء_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "كشف حساب عملاء", dialog.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير أرصدة العملاء إلى Excel");
            clsDialogs.Error("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show) => ReportUiStandardizer.ShowLoading(this, loadingPanel, show);
}
