using Serilog;
using SalesSystem.DTO;
using ClosedXML.Excel;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms;

public partial class frmProductMovementReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "حركة منتج";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();

    public frmProductMovementReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        this.RightToLeft = RightToLeft.Yes;
        dgvReport.AutoGenerateColumns = false;
        dgvReport.DataSource = _bindingSource;
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.CellFormatting += DgvReport_CellFormatting;
        this.Load += OnLoadHandler;
        ReportUiStandardizer.ApplyForm(this);
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

    private async void OnLoadHandler(object? sender, EventArgs e)
    {
        await LoadProductsAsync();
    }

    private async Task LoadProductsAsync()
    {
        try
        {
            var result = await _serviceProvider.GetRequiredService<IProductDesktopService>().GetAllAsync();
            if (result.IsSuccess)
            {
                var products = result.Value.ToList();
                products.Insert(0, new ProductDto(0, "-- اختر منتج --", 0, null, null, 0m, false, null, true));
                cmbProduct.DataSource = products;
                cmbProduct.DisplayMember = "Name";
                cmbProduct.ValueMember = "Id";
            }
            else
            {
                _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error ?? "حدث خطأ في تحميل قائمة المنتجات");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل قائمة المنتجات");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل قائمة المنتجات. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private async Task LoadReportAsync()
    {
        if (cmbProduct.SelectedValue is not int productId || productId == 0)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("يرجى اختيار منتج");
            return;
        }

        ShowLoading(true);
        try
        {
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetMovementsAsync(productId, null, dtpFrom.Value, dtpTo.Value);
            if (result.IsSuccess)
            {
                var data = result.Value.ToList();
                _bindingSource.DataSource = data;
                UpdateSummary(data);
            }
            else
            {
                _serviceProvider.GetRequiredService<INotificationService>().ShowError(result.Error ?? "حدث خطأ");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل تقرير حركة المنتج");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ غير متوقع أثناء تحميل البيانات. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void UpdateSummary(List<ProductMovementReportDto> data)
    {
        lblRecordsCount.Text = data.Count.ToString();

        if (data.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        var totalIn = data.Where(x => x.QuantityChange > 0).Sum(x => x.QuantityChange);
        var totalOut = data.Where(x => x.QuantityChange < 0).Sum(x => x.QuantityChange);

        lblSummary.Text = $"إجمالي الحركات: {data.Count} | الوارد: {totalIn:N3} | الصادر: {totalOut:N3}";
    }

    private void DgvReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // عمود «قبل» غير مرتبط: يُحسب من (بعد - التغير)
        if (dgvReport.Columns[e.ColumnIndex].Name == "colBefore"
            && dgvReport.Rows[e.RowIndex].DataBoundItem is ProductMovementReportDto item)
        {
            e.Value = (item.QuantityAfter - item.QuantityChange).ToString("N3");
            e.FormattingApplied = true;
        }
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<ProductMovementReportDto> data || data.Count == 0)
        {
            _serviceProvider.GetRequiredService<INotificationService>().ShowWarning("لا توجد بيانات للتصدير");
            return;
        }

        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("حركة منتج");

            var headers = new[] { "التاريخ", "النوع", "المخزن", "الكمية", "قبل", "بعد", "المرجع" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            for (int row = 0; row < data.Count; row++)
            {
                var item = data[row];
                worksheet.Cell(row + 2, 1).Value = item.Date;
                worksheet.Cell(row + 2, 1).Style.DateFormat.Format = "dd/MM/yyyy";
                worksheet.Cell(row + 2, 2).Value = item.MovementType;
                worksheet.Cell(row + 2, 3).Value = item.WarehouseName;
                worksheet.Cell(row + 2, 4).Value = item.QuantityChange;
                worksheet.Cell(row + 2, 5).Value = item.QuantityAfter - item.QuantityChange;
                worksheet.Cell(row + 2, 6).Value = item.QuantityAfter;
                worksheet.Cell(row + 2, 7).Value = item.ReferenceNo;
            }

            worksheet.Columns().AdjustToContents();

            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"حركة_منتج_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(dialog.FileName);
                _serviceProvider.GetRequiredService<INotificationService>().ShowSuccess("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير حركة المنتج إلى Excel");
            _serviceProvider.GetRequiredService<INotificationService>().ShowError("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show)
    {
        loadingPanel.Visible = show;
        this.Enabled = !show;
    }
}
