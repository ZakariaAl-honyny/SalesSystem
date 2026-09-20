using SalesSystem.DTO;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.Returns;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Returns.Forms;

public partial class frmListReturns : Form
{
    private readonly IServiceProvider _serviceProvider;
    private ISalesReturnApiService _salesReturnService => _serviceProvider.GetRequiredService<ISalesReturnApiService>();
    private IPurchaseReturnApiService _purchaseReturnService => _serviceProvider.GetRequiredService<IPurchaseReturnApiService>();

    // ── Data Stores ──────────────────────────────────────────────────────────
    private List<SalesReturnDto> _allSalesReturns = new();
    private List<SalesReturnDto> _filteredSalesReturns = new();
    private List<PurchaseReturnDto> _allPurchaseReturns = new();
    private List<PurchaseReturnDto> _filteredPurchaseReturns = new();

    public frmListReturns(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Data Loading ─────────────────────────────────────────────────────────

    private async void _RefreshReturns()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var salesResult = await _salesReturnService.GetAllAsync();
            if (salesResult.IsSuccess && salesResult.Value != null)
            {
                _allSalesReturns = salesResult.Value.ToList();
            }
            else
            {
                if (!salesResult.IsSuccess)
                {
                    MessageBox.Show(salesResult.Error ?? "حدث خطأ في تحميل مرتجعات المبيعات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _allSalesReturns = new List<SalesReturnDto>();
            }

            var purchaseResult = await _purchaseReturnService.GetAllAsync();
            if (purchaseResult.IsSuccess && purchaseResult.Value != null)
            {
                _allPurchaseReturns = purchaseResult.Value.ToList();
            }
            else
            {
                if (!purchaseResult.IsSuccess)
                {
                    MessageBox.Show(purchaseResult.Error ?? "حدث خطأ في تحميل مرتجعات المشتريات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                _allPurchaseReturns = new List<PurchaseReturnDto>();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("حدث خطأ أثناء تحميل المرتجعات: " + ex.Message,
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _allSalesReturns = new List<SalesReturnDto>();
            _allPurchaseReturns = new List<PurchaseReturnDto>();
        }
        finally
        {
            _filteredSalesReturns = _allSalesReturns.ToList();
            _filteredPurchaseReturns = _allPurchaseReturns.ToList();
            _PopulateSalesGrid();
            _PopulatePurchaseGrid();
            _UpdateRecordsCount();
            Cursor = Cursors.Default;
        }
    }

    private void _PopulateSalesGrid()
    {
        dgvSales.Rows.Clear();

        foreach (var r in _filteredSalesReturns)
        {
            dgvSales.Rows.Add(
                r.ReturnNo,
                r.ReturnDate.ToString("yyyy-MM-dd"),
                r.CustomerName ?? "عميل نقدي",
                r.TotalAmount.ToString("N2"),
                r.StatusDisplay
            );
        }
    }

    private void _PopulatePurchaseGrid()
    {
        dgvPurchases.Rows.Clear();

        foreach (var r in _filteredPurchaseReturns)
        {
            dgvPurchases.Rows.Add(
                r.ReturnNo,
                r.ReturnDate.ToString("yyyy-MM-dd"),
                r.SupplierName,
                r.TotalAmount.ToString("N2"),
                r.StatusDisplay
            );
        }
    }

    private void _ReloadForm()
    {
        _SetupSalesGrid();
        _SetupPurchaseGrid();
        _RefreshReturns();
    }

    private void _UpdateRecordsCount()
    {
        int total = _filteredSalesReturns.Count + _filteredPurchaseReturns.Count;
        lblRecordsCount.Text = total.ToString();
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupSalesGrid()
    {
        dgvSales.Columns.Clear();

        dgvSales.Columns.Add("ReturnNo", "رقم المرتجع");
        dgvSales.Columns.Add("ReturnDate", "التاريخ");
        dgvSales.Columns.Add("CustomerName", "العميل");
        dgvSales.Columns.Add("TotalAmount", "الإجمالي");
        dgvSales.Columns.Add("StatusDisplay", "الحالة");

        if (dgvSales.Rows.Count > 0)
        {
            dgvSales.Columns[0].Width = 130;
            dgvSales.Columns[1].Width = 110;
            dgvSales.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSales.Columns[3].Width = 110;
            dgvSales.Columns[4].Width = 90;
        }
    }

    private void _SetupPurchaseGrid()
    {
        dgvPurchases.Columns.Clear();

        dgvPurchases.Columns.Add("ReturnNo", "رقم المرتجع");
        dgvPurchases.Columns.Add("ReturnDate", "التاريخ");
        dgvPurchases.Columns.Add("SupplierName", "المورد");
        dgvPurchases.Columns.Add("TotalAmount", "الإجمالي");
        dgvPurchases.Columns.Add("StatusDisplay", "الحالة");

        if (dgvPurchases.Rows.Count > 0)
        {
            dgvPurchases.Columns[0].Width = 130;
            dgvPurchases.Columns[1].Width = 110;
            dgvPurchases.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPurchases.Columns[3].Width = 110;
            dgvPurchases.Columns[4].Width = 90;
        }
    }

    // ── Form Events ──────────────────────────────────────────────────────────

    private void frmListReturns_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── Toolbar Events ─────────────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        int tabIndex = tabReturns.SelectedIndex;
        if (tabIndex == 0)
            OpenSalesReturnForm(null);
        else
            OpenPurchaseReturnForm(null);
    }

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    // ── Grid Events ─────────────────────────────────────────────────────────

    private void dgvSales_DoubleClick(object sender, EventArgs e)
    {
        if (dgvSales.CurrentRow == null) return;
        int index = dgvSales.CurrentRow.Index;
        if (index < 0 || index >= _filteredSalesReturns.Count) return;
        OpenSalesReturnForm(_filteredSalesReturns[index]);
    }

    private void dgvPurchases_DoubleClick(object sender, EventArgs e)
    {
        if (dgvPurchases.CurrentRow == null) return;
        int index = dgvPurchases.CurrentRow.Index;
        if (index < 0 || index >= _filteredPurchaseReturns.Count) return;
        OpenPurchaseReturnForm(_filteredPurchaseReturns[index]);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private void OpenSalesReturnForm(SalesReturnDto? dto)
    {
        var form = _serviceProvider.GetRequiredService<frmSalesReturn>();
        form.LoadData(dto);
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }

    private void OpenPurchaseReturnForm(PurchaseReturnDto? dto)
    {
        var form = _serviceProvider.GetRequiredService<frmPurchaseReturn>();
        form.LoadData(dto);
        clsGlobal._OpenForm(form);
        _ReloadForm();
    }
}
