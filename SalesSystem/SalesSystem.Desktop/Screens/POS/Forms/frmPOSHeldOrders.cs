namespace SalesSystem.Desktop.Screens.POS.Forms;

/// <summary>
/// نافذة «الفواتير المعلقة» [F4] — UI-first prototype ببيانات محلية في الذاكرة فقط (بدون API / قاعدة بيانات).
/// تُعرض كـ modal عبر ShowDialog(). في النسخة النهائية تُجلب الفواتير المعلقة من ISalesInvoiceApiService
/// وتُعاد أصنافها إلى سلة شاشة POS عند الاسترجاع.
/// </summary>
public partial class frmPOSHeldOrders : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════

    /// <summary>فاتورة معلقة تجريبية. الإجمالي بالريال اليمني (YER).</summary>
    private sealed class MockHeldOrder
    {
        public int Id { get; init; }
        public string CustomerName { get; init; } = "";
        public string HeldAt { get; init; } = "";   // وقت التعليق (ساعة:دقيقة ص/م)
        public int ItemCount { get; init; }         // عدد الأصناف
        public decimal Total { get; init; }         // الإجمالي (decimal — ريال يمني)
    }

    private readonly IServiceProvider _serviceProvider;

    private readonly List<MockHeldOrder> _allOrders = new();
    private readonly List<MockHeldOrder> _visibleOrders = new();

    /// <summary>
    /// محاكاة حالة السلة الحالية في شاشة POS — عند الربط الحقيقي تُقرأ من frmPOS.
    /// true = السلة تحتوي أصنافاً (يظهر تحذير قبل استبدالها بالفاتورة المعلقة).
    /// </summary>
    public bool CartHasItems { get; set; } = true;

    public frmPOSHeldOrders(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _serviceProvider = serviceProvider;
        InitializeComponent();
        BuildMockData();
        SetupGrid();
        ApplyFilter();
        SelectFirstRow();
    }

    /// <summary>
    /// 4 فواتير معلقة تجريبية بأسماء عملاء وأوقات وأعداد أصناف وإجماليات مختلفة بالريال اليمني.
    /// </summary>
    private void BuildMockData()
    {
        _allOrders.Clear();

        MockHeldOrder Make(int id, string customer, string heldAt, int items, decimal total)
            => new MockHeldOrder
            {
                Id = id, CustomerName = customer, HeldAt = heldAt, ItemCount = items, Total = total
            };

        _allOrders.AddRange(new[]
        {
            Make(1, "محمد أحمد الشرعبي", "10:24 ص", 5, 48250m),
            Make(2, "عميل نقدي",          "09:58 ص", 3, 14300m),
            Make(3, "فاطمة علي",          "09:15 ص", 8, 96750m),
            Make(4, "علي حسن",            "08:40 ص", 2, 8300m),
        });
    }

    private void SetupGrid()
    {
        dgvHeldOrders.AutoGenerateColumns = false;
        dgvHeldOrders.Columns.Clear();

        dgvHeldOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCustomer", HeaderText = "العميل", DataPropertyName = "CustomerName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 150
        });
        dgvHeldOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColTime", HeaderText = "الوقت", DataPropertyName = "HeldAt", Width = 95,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvHeldOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColItems", HeaderText = "عدد الأصناف", DataPropertyName = "ItemCount", Width = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvHeldOrders.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "Total", Width = 130,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 110, 253)
            }
        });
        dgvHeldOrders.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "ColRestore", HeaderText = "استرجاع", Width = 100,
            UseColumnTextForButtonValue = true, Text = "استرجاع",
            FlatStyle = FlatStyle.Flat,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(25, 135, 84),
                SelectionForeColor = Color.White
            }
        });
    }

    /// <summary>تصفية الشبكة حسب اسم العميل (بحث بالعميل).</summary>
    private void ApplyFilter()
    {
        string q = txtSearch.Text.Trim();

        var query = _allOrders.AsEnumerable();
        if (q.Length > 0)
        {
            query = query.Where(o => o.CustomerName.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        _visibleOrders.Clear();
        _visibleOrders.AddRange(query);

        dgvHeldOrders.DataSource = null;
        dgvHeldOrders.DataSource = _visibleOrders;
        lblCount.Text = $"عدد الفواتير المعلقة: {_visibleOrders.Count}";
        SelectFirstRow();
    }

    private void SelectFirstRow()
    {
        if (_visibleOrders.Count > 0)
            dgvHeldOrders.CurrentCell = dgvHeldOrders.Rows[0].Cells[0];
    }

    // ═══════════════════════════════════════════════
    // استرجاع فاتورة معلقة
    // ═══════════════════════════════════════════════
    private void dgvHeldOrders_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != dgvHeldOrders.Columns["ColRestore"].Index)
            return;

        if (dgvHeldOrders.Rows[e.RowIndex].DataBoundItem is MockHeldOrder order)
            TryRestore(order);
    }

    private void dgvHeldOrders_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        // النقر المزدوج على الصف = استرجاع (زر «استرجاع» يعمل بالنقرة الواحدة)
        if (e.RowIndex < 0 || e.ColumnIndex == dgvHeldOrders.Columns["ColRestore"].Index)
            return;

        if (dgvHeldOrders.Rows[e.RowIndex].DataBoundItem is MockHeldOrder order)
            TryRestore(order);
    }

    private void TryRestore(MockHeldOrder order)
    {
        // 1) تحذير عند وجود أصناف في السلة الحالية — قبل استبدالها بالفاتورة المعلقة
        if (CartHasItems)
        {
            string warnMsg =
                $"السلة الحالية تحتوي أصنافاً وسيتم استبدالها بالفاتورة المعلقة للعميل «{order.CustomerName}»." +
                Environment.NewLine + "هل تريد المتابعة؟";

            var warn = MessageBox.Show(warnMsg, "استرجاع فاتورة معلقة",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (warn != DialogResult.Yes) return;
        }

        // 2) تأكيد الاسترجاع
        string confirmMsg =
            $"هل تريد استرجاع الفاتورة المعلقة للعميل «{order.CustomerName}»؟" +
            Environment.NewLine +
            $"عدد الأصناف: {order.ItemCount} — الإجمالي: {order.Total:N2} ريال يمني";

        var confirm = MessageBox.Show(confirmMsg, "استرجاع فاتورة معلقة",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        // 3) محاكاة الاسترجاع: إزالة الفاتورة من القائمة وتحديث العداد
        _allOrders.Remove(order);
        ApplyFilter();

        string doneMsg =
            $"تم استرجاع الفاتورة المعلقة للعميل «{order.CustomerName}» إلى السلة بنجاح (وضع تجريبي).";

        MessageBox.Show(doneMsg, "استرجاع فاتورة معلقة",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        // في النسخة النهائية: تُعاد أصناف الفاتورة إلى سلة frmPOS ثم يُغلق الحوار.
        DialogResult = DialogResult.OK;
        Close();
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e) => ApplyFilter();

    private void frmPOSHeldOrders_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Escape:
                btnClose.PerformClick(); // إغلاق (Cancel)
                e.Handled = true;
                break;

            case Keys.Enter:
                if (txtSearch.Focused) break; // داخل حقل البحث نترك المعالجة الافتراضية
                if (dgvHeldOrders.CurrentRow?.DataBoundItem is MockHeldOrder order)
                {
                    TryRestore(order);
                    e.Handled = true;
                }
                break;

            case Keys.F2:
                txtSearch.Focus();
                txtSearch.SelectAll();
                e.Handled = true;
                break;
        }
    }
}