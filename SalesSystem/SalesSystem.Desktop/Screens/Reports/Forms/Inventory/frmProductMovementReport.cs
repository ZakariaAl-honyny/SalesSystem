using Serilog;
using SalesSystem.DTO;
using SalesSystem.Desktop.Screens.Reports;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.GlobalClasses;

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

        try
        {
            ShowLoading(true);
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetMovementsAsync(productId, null, dtpFrom.Value, dtpTo.Value);
            if (result.IsSuccess)
            {
                var data = result.Value.ToList();
                _bindingSource.DataSource = data;

                // عمود «قبل» غير مرتبط بالبيانات — يُملأ بقيمة رقمية بعد الربط حتى يخرج في التصدير
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.DataBoundItem is ProductMovementReportDto item)
                        row.Cells[colBefore.Name].Value = item.QuantityAfter - item.QuantityChange;
                }

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
        lblRecordsCount.Text = clsFormat.Number(data.Count);

        if (data.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            return;
        }

        var totalIn = data.Where(x => x.QuantityChange > 0).Sum(x => x.QuantityChange);
        var totalOut = data.Where(x => x.QuantityChange < 0).Sum(x => x.QuantityChange);

        lblSummary.Text = $"إجمالي الحركات: {clsFormat.Number(data.Count)} | الوارد: {clsFormat.Quantity(totalIn)} | الصادر: {clsFormat.Quantity(totalOut)}";
    }

    private void DgvReport_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;

        // عمود «قبل» غير مرتبط: يُحسب من (بعد - التغير)
        if (dgvReport.Columns[e.ColumnIndex].Name == "colBefore"
            && dgvReport.Rows[e.RowIndex].DataBoundItem is ProductMovementReportDto item)
        {
            e.Value = clsFormat.Quantity(item.QuantityAfter - item.QuantityChange);
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
            // عمود «قبل» غير مرتبط بالبيانات — يُملأ كقيمة رقمية قبل التصدير حتى لا يخرج فارغاً
            for (int i = 0; i < data.Count && i < dgvReport.Rows.Count; i++)
                dgvReport.Rows[i].Cells["colBefore"].Value = data[i].QuantityAfter - data[i].QuantityChange;

            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = $"حركة_منتج_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "حركة منتج", dialog.FileName);
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
        if (show) loadingPanel.BringToFront();
        loadingPanel.Visible = show;
        this.Enabled = !show;
        this.UseWaitCursor = show;
    }
}
