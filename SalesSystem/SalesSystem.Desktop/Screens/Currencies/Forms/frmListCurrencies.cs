using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.Currencies.Forms;

public partial class frmListCurrencies : Form
{
    private readonly IServiceProvider _serviceProvider;
    private List<CurrencyDto> _allCurrencies = new();
    private List<CurrencyDto> _filteredCurrencies = new();

    public frmListCurrencies(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // ═══════════════════════════════════════════════════════
    //  Grid Setup
    // ═══════════════════════════════════════════════════════

    private void _SetupCurrencyGridColumns()
    {
        dgvCurrencies.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",      Header = "الرقم",    Fill = 5,  Min = 60  },
            new { Name = "Code",    Header = "الكود",    Fill = 8,  Min = 80  },
            new { Name = "Name",    Header = "الاسم",    Fill = 18, Min = 160 },
            new { Name = "SubUnit", Header = "الوحدة الفرعية", Fill = 12, Min = 120 },
            new { Name = "IsLocal", Header = "محلي",     Fill = 6,  Min = 70  },
        };

        foreach (var col in columns)
        {
            dgvCurrencies.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    private void _SetupRateGridColumns()
    {
        dgvRates.Columns.Clear();

        var columns = new[]
        {
            new { Name = "Id",           Header = "الرقم",        Fill = 5,  Min = 50  },
            new { Name = "CurrencyName", Header = "العملة",       Fill = 12, Min = 120 },
            new { Name = "Rate",         Header = "سعر الصرف",    Fill = 10, Min = 100 },
            new { Name = "FromDate",     Header = "من تاريخ",     Fill = 10, Min = 100 },
            new { Name = "ToDate",       Header = "إلى تاريخ",    Fill = 10, Min = 100 },
            new { Name = "IsCurrent",    Header = "الحالي",       Fill = 6,  Min = 70  },
            new { Name = "MinRate",      Header = "أدنى سعر",     Fill = 8,  Min = 80  },
            new { Name = "MaxRate",      Header = "أعلى سعر",     Fill = 8,  Min = 80  },
        };

        foreach (var col in columns)
        {
            dgvRates.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = col.Name,
                HeaderText = col.Header,
                DataPropertyName = col.Name,
                MinimumWidth = col.Min,
                FillWeight = col.Fill,
                ReadOnly = true,
            });
        }
    }

    // ═══════════════════════════════════════════════════════
    //  Data Loading
    // ═══════════════════════════════════════════════════════

    private async void _RefreshCurrencies()
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var api = _serviceProvider.GetRequiredService<ICurrencyApiService>();
            var result = await api.GetAllAsync();

            if (result.IsSuccess && result.Value != null)
            {
                _allCurrencies = result.Value.ToList();
            }
            else
            {
                _allCurrencies = new List<CurrencyDto>();
                MessageBox.Show(
                    result.Error ?? "فشل في تحميل بيانات العملات",
                    "خطأ في تحميل البيانات",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _filteredCurrencies = _allCurrencies.ToList();
            _PopulateCurrencyGrid();
            _UpdateRecordsCount();

            // حمّل أسعار الصرف لأول عملة إن وُجدت
            if (_filteredCurrencies.Count > 0)
                await _LoadRatesForCurrency(_filteredCurrencies[0].Id);
            else
                _ClearRatesGrid();
        }
        finally
        {
            btnRefresh.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    private async Task _LoadRatesForCurrency(int currencyId)
    {
        try
        {
            var api = _serviceProvider.GetRequiredService<ICurrencyApiService>();
            var result = await api.GetRatesAsync(currencyId);

            if (result.IsSuccess && result.Value != null)
            {
                _PopulateRateGrid(result.Value);
            }
            else
            {
                _ClearRatesGrid();
            }
        }
        catch
        {
            _ClearRatesGrid();
        }
    }

    // ═══════════════════════════════════════════════════════
    //  Grid Population
    // ═══════════════════════════════════════════════════════

    private void _PopulateCurrencyGrid()
    {
        dgvCurrencies.AutoGenerateColumns = false;
        dgvCurrencies.Rows.Clear();

        foreach (var c in _filteredCurrencies)
        {
            dgvCurrencies.Rows.Add(
                c.Id,
                c.Code,
                c.Name,
                c.SubUnit ?? "—",
                c.IsLocal ? "نعم" : "لا"
            );
        }
    }

    private void _PopulateRateGrid(IReadOnlyList<CurrencyRateDto> rates)
    {
        dgvRates.AutoGenerateColumns = false;
        dgvRates.Rows.Clear();

        foreach (var r in rates)
        {
            dgvRates.Rows.Add(
                r.Id,
                r.CurrencyName,
                r.Rate.ToString("N2"),
                r.FromDate.ToString("yyyy-MM-dd"),
                r.ToDate?.ToString("yyyy-MM-dd") ?? "—",
                r.IsCurrent ? "نعم" : "لا",
                r.MinRate?.ToString("N2") ?? "—",
                r.MaxRate?.ToString("N2") ?? "—"
            );
        }

        lblRatesTitle.Text = $"أسعار الصرف — {rates.FirstOrDefault()?.CurrencyName ?? ""}";
    }

    private void _ClearRatesGrid()
    {
        dgvRates.Rows.Clear();
        lblRatesTitle.Text = "أسعار الصرف";
    }

    // ═══════════════════════════════════════════════════════
    //  Helpers
    // ═══════════════════════════════════════════════════════

    private void _ReloadForm()
    {
        _SetupCurrencyGridColumns();
        _SetupRateGridColumns();
        _RefreshCurrencies();
    }

    private void _UpdateRecordsCount()
    {
        lblRecordsCount.Text = _filteredCurrencies.Count.ToString();
    }

    private int _GetSelectedCurrencyId()
    {
        if (dgvCurrencies.CurrentRow == null) return -1;
        return (int)dgvCurrencies.CurrentRow.Cells["Id"].Value;
    }

    // ═══════════════════════════════════════════════════════
    //  Event Handlers
    // ═══════════════════════════════════════════════════════

    private void frmListCurrencies_Load(object sender, EventArgs e)
    {
        _ReloadForm();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void btnRefresh_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        string filterText = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(filterText))
            _filteredCurrencies = _allCurrencies.ToList();
        else
            _filteredCurrencies = _allCurrencies.Where(c =>
                c.Name.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                c.Code.Contains(filterText, StringComparison.OrdinalIgnoreCase) ||
                (c.SubUnit ?? "").Contains(filterText, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        _PopulateCurrencyGrid();
        _UpdateRecordsCount();
    }

    private async void dgvCurrencies_SelectionChanged(object? sender, EventArgs e)
    {
        int id = _GetSelectedCurrencyId();
        if (id > 0)
            await _LoadRatesForCurrency(id);
        else
            _ClearRatesGrid();
    }

    private void dgvCurrencies_DoubleClick(object sender, EventArgs e)
    {
        // TODO: فتح نافذة التعديل عند توفر frmAddUpdateCurrency
        int id = _GetSelectedCurrencyId();
        if (id > 0)
        {
            MessageBox.Show(
                $"سيتم فتح نافذة تعديل العملة رقم {id} — قريباً",
                "قيد التطوير",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void btnAddNew_Click(object sender, EventArgs e)
    {
        // TODO: فتح نافذة إضافة عملة جديدة عند توفر frmAddUpdateCurrency
        MessageBox.Show(
            "ستتوفر نافذة إضافة عملة جديدة قريباً",
            "قيد التطوير",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void cmsCurrencies_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = dgvCurrencies.Rows.Count == 0 || dgvCurrencies.CurrentRow == null;
    }

    private void editToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int id = _GetSelectedCurrencyId();
        if (id > 0)
        {
            MessageBox.Show(
                $"سيتم فتح نافذة تعديل العملة رقم {id} — قريباً",
                "قيد التطوير",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
