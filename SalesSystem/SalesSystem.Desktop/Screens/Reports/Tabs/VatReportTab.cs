using Serilog;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Common;
using ClosedXML.Excel;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SalesSystem.Desktop.Screens.Reports.Tabs;

public class VatReportTab : UserControl, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "تقرير ضريبة القيمة المضافة";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();
    private DateTimePicker dtpFrom = null!;
    private DateTimePicker dtpTo = null!;
    private Button btnRefresh = null!;
    private Button btnExport = null!;
    private DataGridView dgvReport = null!;
    private Label lblSummary = null!;
    private Panel loadingPanel = null!;

    public VatReportTab(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        this.RightToLeft = RightToLeft.Yes;
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true; dgvReport.AllowUserToAddRows = false; dgvReport.BackgroundColor = Color.White;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvReport.AutoGenerateColumns = true;
    }

    protected override async void OnLoad(EventArgs e) { base.OnLoad(e); await LoadReportAsync(); }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IFinancialReportApiService>().GetVatReportAsync(dtpFrom.Value.Date, dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
            if (result.IsSuccess && result.Value != null)
            {
                var list = result.Value.ToList();
                _bindingSource.DataSource = list;
                UpdateSummary(list);
                FormatGrid();
            }
            else { _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error!); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل تقرير الضريبة"); _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<VatReportDto> list)
    {
        if (list.Count == 0) { lblSummary.Text = "لا توجد بيانات"; return; }
        var total = list.Sum(x => x.TaxAmount);
        lblSummary.Text = $"عدد الفواتير: {list.Count} | إجمالي الضريبة: {total:N2}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("InvoiceNumber", "رقم الفاتورة"); SetHeader("InvoiceDate", "التاريخ"); SetHeader("PartyName", "الطرف");
        SetHeader("TaxableAmount", "الضريبية"); SetHeader("TaxRate", "النسبة"); SetHeader("TaxAmount", "الضريبة");
        foreach (var c in new[] { "TaxableAmount", "TaxRate", "TaxAmount" })
            if (dgvReport.Columns.Contains(c)) dgvReport.Columns[c].DefaultCellStyle.Format = "N2";
    }

    private void SetHeader(string col, string text) { if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text; }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource == null) { _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("لا توجد بيانات للتصدير"); return; }
        try
        {
            using var wb = new XLWorkbook(); var ws = wb.Worksheets.Add("تقرير الضريبة");
            var headers = new[] { "رقم الفاتورة", "التاريخ", "الطرف", "الضريبية", "النسبة", "الضريبة" };
            for (int i = 0; i < headers.Length; i++) { ws.Cell(1, i + 1).Value = headers[i]; ws.Cell(1, i + 1).Style.Font.Bold = true; }
            ws.Columns().AdjustToContents();
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"تقرير_الضريبة_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK) { wb.SaveAs(dlg.FileName); _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم تصدير التقرير بنجاح"); }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير تقرير الضريبة"); _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء التصدير."); }
    }

    private void ShowLoading(bool show) { loadingPanel.Visible = show; this.Enabled = !show; }

    private void InitializeComponent()
    {
        this.Dock = DockStyle.Fill; this.BackColor = Color.White;
        var ml = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(0), Margin = new Padding(0) };
        ml.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F)); ml.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); ml.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        var tp = new Panel { Dock = DockStyle.Fill }; ThemeHelper.ApplyToolbarStyle(tp);
        var tb = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
        btnRefresh = new Button { Text = "تحديث", Margin = new Padding(8, 0, 8, 0) }; ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Secondary); btnRefresh.Click += async (_, _) => await LoadReportAsync();
        btnExport = new Button { Text = "تصدير Excel", Margin = new Padding(8, 0, 15, 0) }; ThemeHelper.ApplyButtonStyle(btnExport, ThemeHelper.ButtonType.Ghost); btnExport.ForeColor = Color.FromArgb(46, 125, 50); btnExport.Click += async (_, _) => await ExportToExcelAsync();
        dtpTo = new DateTimePicker { Width = 110, Margin = new Padding(8, 10, 8, 0) }; dtpTo.Format = DateTimePickerFormat.Short; dtpTo.Value = DateTime.Today;
        var lblTo = new Label { Text = "إلى:", AutoSize = true, Margin = new Padding(0, 15, 0, 0), Font = new Font("Segoe UI", 9F) };
        dtpFrom = new DateTimePicker { Width = 110, Margin = new Padding(8, 10, 8, 0) }; dtpFrom.Format = DateTimePickerFormat.Short; dtpFrom.Value = DateTime.Today.AddMonths(-1);
        var lblFrom = new Label { Text = "من:", AutoSize = true, Margin = new Padding(0, 15, 0, 0), Font = new Font("Segoe UI", 9F) };
        tb.Controls.AddRange(new Control[] { btnRefresh, btnExport, dtpTo, lblTo, dtpFrom, lblFrom }); tp.Controls.Add(tb);
        dgvReport = new DataGridView { Dock = DockStyle.Fill }; ThemeHelper.ApplyDataGridViewStyle(dgvReport);
        lblSummary = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Text = "جاهز", Font = new Font("Segoe UI", 9F), ForeColor = ThemeHelper.TextSecondary, BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(10, 0, 10, 0), Margin = new Padding(0) };
        loadingPanel = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = Color.FromArgb(100, Color.White) };
        var ll = new Label { Text = "جارٍ التحميل...", Font = new Font("Segoe UI", 12F, FontStyle.Bold), AutoSize = true, ForeColor = ThemeHelper.Primary };
        ll.Location = new Point((Width - ll.Width) / 2, (Height - ll.Height) / 2); loadingPanel.Controls.Add(ll);
        ml.Controls.Add(tp, 0, 0); ml.Controls.Add(dgvReport, 0, 1); ml.Controls.Add(lblSummary, 0, 2);
        this.Controls.Add(ml); this.Controls.Add(loadingPanel);
    }
}
