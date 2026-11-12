using Serilog;
using SalesSystem.DTO;
using ClosedXML.Excel;
using SalesSystem.Desktop.Controls.Common;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmInventoryReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "تقرير المخزون";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    private IReadOnlyList<WarehouseDto> _warehouses = new List<WarehouseDto>();

    public frmInventoryReport(IServiceProvider serviceProvider)
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
        this.Load += async (s, e) =>
        {
            await LoadWarehousesAsync();
            await LoadReportAsync();
        };
        ReportUiStandardizer.ApplyForm(this);
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

    private async Task LoadWarehousesAsync()
    {
        try
        {
            var whResult = await _serviceProvider.GetRequiredService<IWarehouseDesktopService>().GetAllAsync(true);
            if (whResult.IsSuccess)
            {
                _warehouses = whResult.Value;
                cmbWarehouse.Items.Clear();
                cmbWarehouse.Items.Add(new ComboBoxItem { Text = "جميع المستودعات", Value = null });
                foreach (var wh in _warehouses)
                {
                    cmbWarehouse.Items.Add(new ComboBoxItem { Text = wh.Name, Value = wh.Id });
                }
                cmbWarehouse.SelectedIndex = 0;
            }
            else
            {
                _serviceProvider.GetRequiredService<INotificationService>().ShowError(whResult.Error ?? "حدث خطأ في تحميل قائمة المستودعات");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل قائمة المستودعات");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء تحميل قائمة المستودعات.");
        }
    }

    private async Task LoadReportAsync()
    {
        try
        {
            ShowLoading(true);
            int? warehouseId = GetSelectedWarehouseId();
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetStockReportAsync(warehouseId);

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
            Log.Error(ex, "حدث خطأ في تحميل تقرير المخزون");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل التقرير. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private int? GetSelectedWarehouseId()
    {
        if (cmbWarehouse.SelectedItem is ComboBoxItem item)
        {
            return item.Value as int?;
        }
        return null;
    }

    private void UpdateSummary(List<StockReportDto> list)
    {
        lblRecordsCount.Text = list.Count.ToString();

        if (list.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        var totalQty = list.Sum(x => x.CurrentStock);
        var lowStockCount = list.Count(x => x.CurrentStock < x.ReorderLevel);

        lblSummary.Text = $"عدد الأصناف: {list.Count} | إجمالي الكمية: {totalQty:N3} | أقل من الحد الأدنى: {lowStockCount}";
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        var hides = new[] { "ProductId", "WarehouseId" };
        foreach (var h in hides)
        {
            if (dgvReport.Columns.Contains(h)) dgvReport.Columns[h].Visible = false;
        }

        SetHeader("ProductCode", "كود المنتج");
        SetHeader("ProductName", "اسم المنتج");
        SetHeader("UnitName", "الوحدة");
        SetHeader("WarehouseName", "المستودع");
        SetHeader("CurrentStock", "الكمية");
        SetHeader("ReorderLevel", "الحد الأدنى");

        if (dgvReport.Columns.Contains("CurrentStock"))
            dgvReport.Columns["CurrentStock"].DefaultCellStyle.Format = "N3";

        if (dgvReport.Columns.Contains("ReorderLevel"))
            dgvReport.Columns["ReorderLevel"].DefaultCellStyle.Format = "N3";
    }

    private void DgvReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex >= 0 && dgvReport.Columns[e.ColumnIndex].Name == "CurrentStock")
        {
            var row = dgvReport.Rows[e.RowIndex];
            if (row.DataBoundItem is StockReportDto item && item.CurrentStock < item.ReorderLevel)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 200, 200);
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
            }
        }
    }

    private void SetHeader(string col, string text)
    {
        if (dgvReport.Columns.Contains(col)) dgvReport.Columns[col].HeaderText = text;
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<StockReportDto> data || data.Count == 0)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("تقرير المخزون");

            var headers = new[] { "كود المنتج", "اسم المنتج", "الوحدة", "المستودع", "الكمية", "الحد الأدنى" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];
                worksheet.Cell(row + 2, 1).Value = item.ProductCode ?? "-";
                worksheet.Cell(row + 2, 2).Value = item.ProductName;
                worksheet.Cell(row + 2, 3).Value = item.UnitName ?? "-";
                worksheet.Cell(row + 2, 4).Value = item.WarehouseName;
                worksheet.Cell(row + 2, 5).Value = item.CurrentStock;
                worksheet.Cell(row + 2, 6).Value = item.ReorderLevel;

                // Highlight low stock rows
                if (item.CurrentStock < item.ReorderLevel)
                {
                    var range = worksheet.Range(row + 2, 1, row + 2, 6);
                    range.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 200, 200);
                    range.Style.Font.FontColor = XLColor.Red;
                }
            }

            worksheet.Columns().AdjustToContents();

            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"تقرير_المخزون_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(dialog.FileName);
                _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير المخزون إلى Excel");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show)
    {
        loadingPanel.Visible = show;
        this.Enabled = !show;
    }
}
