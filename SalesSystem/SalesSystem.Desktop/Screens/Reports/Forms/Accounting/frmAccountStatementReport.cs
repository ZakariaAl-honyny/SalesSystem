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

public partial class frmAccountStatementReport : Form, IExportableReport
{
    public DataGridView GetDataGridView() => dgvReport;
    public string GetReportName() => "كشف حساب الطرف";

    private readonly IServiceProvider _serviceProvider;
    private readonly BindingSource _bindingSource = new();
    private readonly ToolTip _summaryTip = new() { InitialDelay = 300, AutoPopDelay = 8000 };
    private readonly ToolTip _buttonTip = new() { InitialDelay = 300, AutoPopDelay = 8000 };
    private readonly ErrorProvider _errorProvider = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink, RightToLeft = true };

    public frmAccountStatementReport(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        components ??= new System.ComponentModel.Container();
        components.Add(_summaryTip);
        components.Add(_buttonTip);
        components.Add(_errorProvider);
        dgvReport.ReadOnly = true;
        dgvReport.AllowUserToAddRows = false;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.AutoGenerateColumns = true;
        dgvReport.DataSource = _bindingSource;
        _errorProvider.ContainerControl = this;
        cmbPartyType.SelectedIndex = 0;
        nudPartyId.Minimum = 0;
        nudPartyId.Value = 0;
        _buttonTip.SetToolTip(btnRefresh, "تحديث (F5)");
        _buttonTip.SetToolTip(btnExport, "تصدير إلى Excel (Ctrl+E)");
        _buttonTip.SetToolTip(btnClose, "إغلاق (Esc)");
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
            if (cmbPartyType.SelectedIndex < 0)
            {
                clsDialogs.Warning("اختر نوع الطرف (عميل أو مورد) أولاً.");
                return;
            }
            var partyId = (int)nudPartyId.Value;
            if (partyId <= 0)
            {
                _errorProvider.SetError(nudPartyId, "أدخل رقم الطرف أولاً");
                lblSummary.Text = "أدخل رقم الطرف أولاً";
                clsDialogs.Warning("أدخل رقم الطرف أولاً.");
                return;
            }
            IReadOnlyList<AccountStatementDto>? list = null;
            if (cmbPartyType.SelectedIndex == 0)
            {
                // نهاية اليوم شاملة: FinancialReportService.cs:659 يقارن InvoiceDate (طابع زمني كامل) بـ to مباشرةً بلا تطبيع.
                var result = await _serviceProvider.GetRequiredService<IFinancialReportDesktopService>().GetAccountStatementAsync(partyId, dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value), default);
                if (result.IsSuccess) list = result.Value;
                else { clsDialogs.Error(result.Error!); return; }
            }
            else
            {
                // نهاية اليوم شاملة: FinancialReportService.cs:612,714 يأخذ DateOnly من to مباشرةً، و:530 يقارن VoucherDate بطابع زمني كامل — فنهاية آخر لحظة في اليوم صحيحة للثلاثة.
                var result = await _serviceProvider.GetRequiredService<IFinancialReportDesktopService>().GetSupplierStatementAsync(partyId, dtpFrom.Value.Date, clsValidation.EndOfDay(dtpTo.Value), default);
                if (result.IsSuccess) list = result.Value;
                else { clsDialogs.Error(result.Error!); return; }
            }
            if (list != null)
            {
                _bindingSource.DataSource = list.ToList();
                UpdateSummary(list.ToList());
                FormatGrid();
                _errorProvider.SetError(nudPartyId, string.Empty);
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تحميل كشف حساب الطرف"); clsDialogs.Error("حدث خطأ غير متوقع أثناء تحميل التقرير."); }
        finally { ShowLoading(false); }
    }

    private void UpdateSummary(List<AccountStatementDto> list)
    {
        if (list.Count == 0)
        {
            lblSummary.Text = "لا توجد بيانات";
            lblRecordsCount.Text = "عدد الحركات: 0";
            _summaryTip.SetToolTip(lblSummary, string.Empty);
            return;
        }

        var debit = list.Sum(x => x.Debit);
        var credit = list.Sum(x => x.Credit);

        // الرصيد الختامي = رصيد آخر حركة كما تحسبه الخدمة (عميل: مدين−دائن · مورد: دائن−مدين).
        // لا تُكرَّر قاعدة إشارة طبيعة الحساب داخل الشاشة، وإلا خالف الملخّص شبكة السطور في كشف المورد
        // (RULE-N169 — الملخّص من نفس مصدر أرقام الشبكة لا من قاعدة ثانية).
        var balance = list[^1].Balance;

        lblRecordsCount.Text = $"عدد الحركات: {clsFormat.Number(list.Count)}";
        lblSummary.Text = $"مدين: {clsFormat.Money(debit)}    |    دائن: {clsFormat.Money(credit)}    |    الرصيد: {clsFormat.Money(balance)}";

        try { _summaryTip.SetToolTip(lblSummary, "الرصيد تفقيطاً: " + ArabicNumberToWords.ToWords(balance)); }
        catch (Exception ex) { Log.Error(ex, "خطأ في تفقيط رصيد كشف حساب الطرف"); }
    }

    private void FormatGrid()
    {
        if (dgvReport.Columns.Count == 0) return;
        SetHeader("Date", "التاريخ"); SetHeader("Description", "البيان"); SetHeader("ReferenceNumber", "رقم المرجع");
        SetHeader("Debit", "مدين"); SetHeader("Credit", "دائن"); SetHeader("Balance", "الرصيد");
        GridFormatHelper.ApplyColumns(dgvReport, GridFormatKind.Money, "Debit", "Credit", "Balance");
        // عمود التاريخ من البيت الواحد أيضاً — وإلا عُرض بصيغة ثقافة الجهاز لا بصيغة clsFormat (RULE-N169).
        GridFormatHelper.ApplyColumn(dgvReport, "Date", GridFormatKind.DateShort);
    }

    private void SetHeader(string col, string text) => ReportUiStandardizer.SetHeader(dgvReport, col, text);

    private Task ExportToExcelAsync()
    {
        if (dgvReport.Rows.Count == 0) { clsDialogs.Warning("لا توجد بيانات للتصدير"); return Task.CompletedTask; }
        try
        {
            var dlg = new SaveFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", FileName = $"كشف_حساب_{DateTime.Now:yyyyMMdd}.xlsx" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ExportHelper.ExportToExcel(dgvReport, "كشف حساب", dlg.FileName);
                clsDialogs.Success("تم تصدير التقرير بنجاح");
            }
        }
        catch (Exception ex) { Log.Error(ex, "خطأ في تصدير كشف حساب"); clsDialogs.Error("حدث خطأ أثناء التصدير."); }
        return Task.CompletedTask;
    }

    private void ShowLoading(bool show) => ReportUiStandardizer.ShowLoading(this, loadingPanel, show);
}

