using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.PurchaseOrders.Forms;

/// <summary>
/// شاشة أوامر الشراء — نموذج واجهة تجريبي (UI Shape) ببيانات تجريبية داخل الذاكرة،
/// بدون أي اتصال بالخلفية (API/قاعدة بيانات).
/// سيتم ربط هذه الشاشة بخدمة واجهة برمجية (IPurchaseOrderApiService) لاحقاً.
/// </summary>
public partial class frmPurchaseOrder : Form
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>قائمة الأوامر الكاملة (بيانات تجريبية في الذاكرة).</summary>
    private List<PurchaseOrderRow> _allOrders = new();

    /// <summary>الأوامر بعد تطبيق عوامل التصفية (بحث / حالة / فترة تاريخ).</summary>
    private List<PurchaseOrderRow> _filteredOrders = new();

    /// <summary>صف بيانات تجريبي لأمر شراء (يعكس لاحقاً PurchaseOrderDto).</summary>
    private sealed class PurchaseOrderRow
    {
        public string OrderNo { get; init; } = "";
        public string SupplierName { get; init; } = "";
        public DateTime OrderDate { get; init; }
        public int ItemsCount { get; init; }
        public decimal Total { get; init; }
        public string Status { get; init; } = "مفتوح"; // مفتوح / مستلم / ملغي
    }

    public frmPurchaseOrder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
    }

    // ── Mock Data Loading ──────────────────────────────────────────────────────

    /// <summary>تعبئة بيانات تجريبية داخل الذاكرة (6 أوامر عبر الحالات الثلاث).</summary>
    private void _LoadMockData()
    {
        DateTime today = DateTime.Today;

        _allOrders = new List<PurchaseOrderRow>
        {
            new PurchaseOrderRow
            {
                OrderNo = "PO-2026-001",
                SupplierName = "شركة النور للتجارة",
                OrderDate = today.AddDays(-3),
                ItemsCount = 3,
                Total = 12500.00m,
                Status = "مفتوح",
            },
            new PurchaseOrderRow
            {
                OrderNo = "PO-2026-002",
                SupplierName = "مؤسسة الشرق الأوسط",
                OrderDate = today.AddDays(-1),
                ItemsCount = 5,
                Total = 48750.50m,
                Status = "مفتوح",
            },
            new PurchaseOrderRow
            {
                OrderNo = "PO-2026-003",
                SupplierName = "مصنع الأماني للمواد الغذائية",
                OrderDate = today.AddDays(-9),
                ItemsCount = 8,
                Total = 96300.00m,
                Status = "مستلم",
            },
            new PurchaseOrderRow
            {
                OrderNo = "PO-2026-004",
                SupplierName = "شركة الخليج للإلكترونيات",
                OrderDate = today.AddDays(-11),
                ItemsCount = 2,
                Total = 15200.00m,
                Status = "مستلم",
            },
            new PurchaseOrderRow
            {
                OrderNo = "PO-2026-005",
                SupplierName = "مؤسسة الركن الأخضر",
                OrderDate = today.AddDays(-16),
                ItemsCount = 6,
                Total = 31850.75m,
                Status = "ملغي",
            },
            new PurchaseOrderRow
            {
                OrderNo = "PO-2026-006",
                SupplierName = "شركة الاتحاد للمواد الغذائية",
                OrderDate = today.AddDays(-5),
                ItemsCount = 4,
                Total = 22400.00m,
                Status = "مفتوح",
            },
        };
    }

    // ── Grid Setup ─────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvOrders.Columns.Clear();

        dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "OrderNo",
            HeaderText = "رقم الأمر",
            DataPropertyName = "OrderNo",
            FillWeight = 10,
            MinimumWidth = 90,
            ReadOnly = true,
        });
        dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "SupplierName",
            HeaderText = "المورد",
            DataPropertyName = "SupplierName",
            FillWeight = 20,
            MinimumWidth = 150,
            ReadOnly = true,
        });
        dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "OrderDate",
            HeaderText = "التاريخ",
            DataPropertyName = "OrderDate",
            FillWeight = 12,
            MinimumWidth = 100,
            ReadOnly = true,
        });
        dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ItemsCount",
            HeaderText = "عدد الأصناف",
            DataPropertyName = "ItemsCount",
            FillWeight = 10,
            MinimumWidth = 90,
            ReadOnly = true,
        });
        dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Total",
            HeaderText = "الإجمالي",
            DataPropertyName = "Total",
            FillWeight = 14,
            MinimumWidth = 110,
            ReadOnly = true,
        });
        dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Status",
            HeaderText = "الحالة",
            DataPropertyName = "Status",
            FillWeight = 10,
            MinimumWidth = 80,
            ReadOnly = true,
        });
        dgvOrders.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "Convert",
            HeaderText = "تحويل",
            Text = "تحويل إلى فاتورة مشتريات",
            UseColumnTextForButtonValue = true,
            FillWeight = 22,
            MinimumWidth = 180,
        });
    }

    // ── Filtering ──────────────────────────────────────────────────────────────

    private void _ApplyFilters()
    {
        // حماية: قبل اكتمال تهيئة المكونات (أحداث Load المبكرة).
        if (dgvOrders.Columns.Count == 0 || cbStatus.Items.Count == 0)
            return;

        IEnumerable<PurchaseOrderRow> query = _allOrders;

        // تصفية حسب الحالة
        string statusFilter = cbStatus.SelectedItem?.ToString() ?? "الكل";
        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "الكل")
            query = query.Where(o => o.Status == statusFilter);

        // تصفية حسب فترة التاريخ (من/إلى)
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        query = query.Where(o => o.OrderDate.Date >= from && o.OrderDate.Date <= to);

        // تصفية نصية (رقم الأمر / اسم المورد)
        string text = txtFilterValue.Text.Trim();
        if (!string.IsNullOrEmpty(text))
            query = query.Where(o =>
                o.OrderNo.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                o.SupplierName.Contains(text, StringComparison.OrdinalIgnoreCase));

        _filteredOrders = query.ToList();
        _PopulateGrid();
        lblRecordsCount.Text = _filteredOrders.Count.ToString();
    }

    private void _PopulateGrid()
    {
        dgvOrders.Rows.Clear();

        foreach (var order in _filteredOrders)
        {
            dgvOrders.Rows.Add(
                order.OrderNo,
                order.SupplierName,
                order.OrderDate.ToString("yyyy/MM/dd"),
                order.ItemsCount,
                order.Total.ToString("N2"),
                order.Status,
                string.Empty // عمود الزر (نص الزر ثابت من العمود)
            );
        }
    }

    private void _ReloadForm()
    {
        _SetupGridColumns();
        _ApplyFilters();
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private PurchaseOrderRow? _GetSelectedOrder()
    {
        if (dgvOrders.CurrentRow == null) return null;
        int rowIndex = dgvOrders.CurrentRow.Index;
        if (rowIndex < 0 || rowIndex >= _filteredOrders.Count) return null;
        return _filteredOrders[rowIndex];
    }

    // ── Form Events ────────────────────────────────────────────────────────────

    private void frmPurchaseOrder_Load(object sender, EventArgs e)
    {
        dtpFrom.Value = DateTime.Today.AddDays(-30);
        dtpTo.Value = DateTime.Today;
        cbStatus.SelectedIndex = 0;
        _LoadMockData();
        _SetupGridColumns();
        _ApplyFilters();
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── Filter Events ──────────────────────────────────────────────────────────

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void dtpTo_ValueChanged(object sender, EventArgs e)
        => _ApplyFilters();

    private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            e.Handled = true;
            return;
        }
    }

    // ── Grid Events ────────────────────────────────────────────────────────────

    private void dgvOrders_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        // تلوين عمود الحالة: مفتوح (أزرق) / مستلم (أخضر) / ملغي (أحمر)
        if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
        {
            e.CellStyle.ForeColor = e.Value.ToString() switch
            {
                "مستلم" => Color.Green,
                "ملغي" => Color.Red,
                _ => Color.Blue,
            };
            e.CellStyle.Font = new Font(dgvOrders.Font, FontStyle.Bold);
        }
    }

    private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        if (dgvOrders.Columns[e.ColumnIndex].Name == "Convert")
            _ConvertSelectedOrder();
    }

    // ── Toolbar / Button Events ────────────────────────────────────────────────

    private void btnAdd_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "شاشة إضافة أمر شراء جديد قيد التطوير — هذا نموذج واجهة تجريبي (Mock).",
            "إضافة أمر شراء",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void _ConvertSelectedOrder()
    {
        var order = _GetSelectedOrder();
        if (order == null) return;

        if (order.Status != "مفتوح")
        {
            MessageBox.Show(
                "يمكن تحويل الأوامر المفتوحة فقط إلى فاتورة مشتريات.",
                "تنبيه",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show(
                $"هل تريد تحويل أمر الشراء رقم {order.OrderNo} إلى فاتورة مشتريات؟",
                "تأكيد التحويل",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        MessageBox.Show(
            $"تم تحويل أمر الشراء رقم {order.OrderNo} إلى فاتورة مشتريات بنجاح.",
            "نجاح",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void convertToolStripMenuItem_Click(object sender, EventArgs e)
        => _ConvertSelectedOrder();

    private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        => _ReloadForm();

    private void cmsOrders_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        => cmsOrders.Enabled = dgvOrders.Rows.Count > 0 && dgvOrders.CurrentRow != null;
}