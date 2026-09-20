using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.CashBoxes;
using SalesSystem.Desktop.Helpers;
using SalesSystem.DTO.Common;

namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

public partial class frmDailyClosure : Form
{
    private readonly IServiceProvider _serviceProvider;

    public frmDailyClosure(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        dtpDate.Value = DateTime.Today;
    }

    private async void _LoadClosureData()
    {
        DateTime selectedDate = dtpDate.Value.Date;

        Cursor = Cursors.WaitCursor;

        var result = await _serviceProvider.GetRequiredService<ICashBoxReportApiService>().GetCashBoxSummaryAsync(selectedDate);

        Cursor = Cursors.Default;

        if (result.IsSuccess && result.Value != null)
        {
            var summaries = result.Value.ToList();

            decimal totalIncome = summaries.Sum(s => s.TotalIncome);
            decimal totalExpense = summaries.Sum(s => s.TotalExpense);
            decimal netBalance = summaries.Sum(s => s.NetBalance);

            txtTotalIncome.Text = totalIncome.ToString("N2");
            txtTotalExpense.Text = totalExpense.ToString("N2");
            txtNetBalance.Text = netBalance.ToString("N2");
        }
        else
        {
            txtTotalIncome.Text = "0.00";
            txtTotalExpense.Text = "0.00";
            txtNetBalance.Text = "0.00";

            if (result.Error != null)
            {
                MessageBox.Show(result.Error, "خطأ في تحميل البيانات",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void dtpDate_ValueChanged(object sender, EventArgs e)
    {
        _LoadClosureData();
    }

    private void frmDailyClosure_Load(object sender, EventArgs e)
    {
        _LoadClosureData();
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
        MessageBox.Show("سيتم تفعيل خاصية الطباعة قريباً.", "طباعة",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
