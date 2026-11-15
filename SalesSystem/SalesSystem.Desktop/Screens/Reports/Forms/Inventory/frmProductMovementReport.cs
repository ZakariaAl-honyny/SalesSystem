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
                clsDialogs.Error(result.Error ?? "حدث خطأ في تحميل قائمة المنتجات");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "خطأ في تحميل قائمة المنتجات");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل قائمة المنتجات. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private async Task LoadReportAsync()
    {
        if (cmbProduct.SelectedValue is not int productId || productId == 0)
        {
            clsDialogs.Warning("يرجى اختيار منتج");
            return;
        }

        // RULE-N169 — حكم نطاق التاريخ من بيته الواحد clsValidation، والرفض بصيغة عربية واحدة.
        if (!clsValidation.IsDateRangeValid(dtpFrom.Value, dtpTo.Value))
        {
            clsDialogs.Warning(clsValidation.DateRangeErrorMessage);
            return;
        }

        try
        {
            ShowLoading(true);
            // نهاية اليوم شاملة: ReportRepository.cs:196 يقارن CreatedAt (طابع زمني كامل) بـ to.Value.Date.AddDays(1).AddTicks(-1)، فتمرير .Date وحدها يُسقط حركات اليوم الأخير (MockReportDesktopService.cs:259-260 يقارن بالتاريخ فقط فلا يتأثر).
            var result = await _serviceProvider.GetRequiredService<IReportDesktopService>().GetMovementsAsync(productId, null, dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value));
            if (result.IsSuccess)
            {
                // نوع الحركة يُعرض عربياً دائماً (RULE-N28): ما يصل عربياً (بيانات التجربة) يبقى كما هو،
                // واسم التعداد الإنجليزي القادم من طبقة البيانات يُترجم عبر خريطة واحدة في هذه الشاشة.
                var data = result.Value
                    .Select(x => x with { MovementType = MovementTypeLabel(x.MovementType) })
                    .ToList();
                _bindingSource.DataSource = data;

                // عمود «قبل» غير مرتبط بالبيانات — يُملأ بقيمة رقمية بعد الربط حتى يخرج في التصدير
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.DataBoundItem is ProductMovementReportDto item)
                        row.Cells[colBefore.Name].Value = item.QuantityAfter - item.QuantityChange;
                }

                FormatGrid();
                UpdateSummary(data);
            }
            else
            {
                clsDialogs.Error(result.Error ?? "حدث خطأ");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل تقرير حركة المنتج");
            clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل البيانات. تم تسجيل التفاصيل للدعم الفني.");
        }
        finally
        {
            ShowLoading(false);
        }
    }

    /// <summary>
    /// اسم نوع الحركة بالعربية — النص العربي (بيانات التجربة والحالات المترجَمة) يُعاد كما هو،
    /// واسم التعداد الإنجليزي القادم من طبقة البيانات يُترجم بمفردات تقرير المخزون نفسها.
    /// <para>
    /// <b>البيت الصحيح لهذه الخريطة طبقة البيانات</b> (مالكها: مالك تقرير حركة المنتج في المرحلة ب)،
    /// وتُعلَن هنا مرة واحدة فقط لأن لا بيت مشترك لأسماء أنواع الحركة في <c>Helpers/</c> —
    /// وشاشة عربية لا تعرض نصاً إنجليزياً (<c>RULE-N28</c>). تُنقل إلى بيتها عند إنشائه (<c>RULE-N170</c>).
    /// </para>
    /// </summary>
    private static string MovementTypeLabel(string? movementType) => movementType switch
    {
        null or "" => string.Empty,
        "Purchase" => "مشتريات",
        "PurchaseReturn" => "مرتجع مشتريات",
        "Sale" => "مبيعات",
        "SaleReturn" => "مرتجع مبيعات",
        "TransferOut" => "تحويل صادر",
        "TransferIn" => "تحويل وارد",
        "Count" => "جرد",
        "Adjustment" => "تسوية",
        "Damage" => "تالف",
        "OpeningBalance" => "رصيد افتتاحي",
        "InternalIssue" => "صرف داخلي",
        "InternalReceipt" => "استلام داخلي",
        _ => movementType ?? string.Empty
    };

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

    /// <summary>
    /// RULE-N169 — تنسيق أعمدة التقرير من بيته الواحد <see cref="GridFormatHelper"/>: الشاشة تعلن
    /// <b>الدلالة</b> لا نص التنسيق، فتأتي منازل الكميات وصيغة التاريخ من <c>clsFormat</c> الحيّ
    /// ويقرأها <c>ExportHelper</c> عند التصدير.
    /// <para>
    /// أسماء الأعمدة هي <c>Name</c> المعلن في المصمّم لا اسم حقل البيانات (<c>RULE-N179</c>)، والعمود
    /// غير الموجود يُتجاهل بصمت. ولا يلمس الـHelper أي سلوك بنيوي في الشبكة (<c>RULE-N52</c>).
    /// </para>
    /// </summary>
    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;

        // كميات: التغير · «قبل» (غير مرتبط بالبيانات، يملؤه LoadReportAsync) · بعد
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Quantity, "colQtyChange", "colBefore", "colAfter");

        // التاريخ: صيغة يوم/شهر/سنة الحيّة — مطابقة لقيمة المصمّم "dd/MM/yyyy"
        GridFormatHelper.ApplyColumn(dgvReport, "colDate", GridFormatKind.DateDayMonthYear);
    }

    private async Task ExportToExcelAsync()
    {
        if (_bindingSource.DataSource is not List<ProductMovementReportDto> data || data.Count == 0)
        {
            clsDialogs.Warning("لا توجد بيانات للتصدير");
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
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تصدير تقرير حركة المنتج إلى Excel");
            clsDialogs.Error("حدث خطأ أثناء التصدير. تم تسجيل التفاصيل للدعم الفني.");
        }
    }

    private void ShowLoading(bool show) => ReportUiStandardizer.ShowLoading(this, loadingPanel, show);
}
