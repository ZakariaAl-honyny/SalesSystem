using System.Drawing.Drawing2D;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة اختيار العميل — UI-first prototype ببيانات تجريبية داخل الذاكرة (بدون API / قاعدة بيانات).
/// تُعرض كـ modal عبر ShowDialog(). في النسخة النهائية يُجلب العملاء من ICustomerApiService.
/// </summary>
public partial class frmCustomerPicker : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════
    /// <summary>
    /// عميل تجريبي. الرصيد: موجب = "له دفعة سابقة" (أخضر) — سالب = "عليه للمحل" (أحمر) — صفر = "حساب خالص" (رمادي).
    /// </summary>
    private sealed class MockCustomer
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public decimal Balance { get; set; }
    }

    /// <summary>زر بحدود متقطعة (Dashed) — لعنصر الاختيار السريع "عميل نقدي عام".</summary>
    private sealed class DashedButton : Button
    {
        public DashedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.FromArgb(255, 244, 204);   // كهرماني فاتح
            ForeColor = Color.FromArgb(122, 79, 0);      // بني داكن
            Font = new Font("Tahoma", 9.5F, FontStyle.Bold);
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(224, 168, 15), 1.5F) { DashStyle = DashStyle.Dash };
            e.Graphics.DrawRectangle(pen, new Rectangle(2, 2, Width - 5, Height - 5));
        }
    }

    private readonly List<MockCustomer> _allCustomers = new();
    private readonly List<MockCustomer> _visibleCustomers = new();
    private MockCustomer? _selected;
    private bool _cashCustomerSelected;

    /// <summary>معرّف العميل المختار (0 عند اختيار "عميل نقدي عام").</summary>
    public int SelectedCustomerId => _selected?.Id ?? 0;

    /// <summary>اسم العميل المختار.</summary>
    public string SelectedCustomerName => _selected?.Name ?? "";

    /// <summary>رصيد العميل المختار.</summary>
    public decimal SelectedCustomerBalance => _selected?.Balance ?? 0m;

    /// <summary>true عند الضغط على "عميل نقدي عام" (فاتورة نقدية بدون عميل محدد).</summary>
    public bool IsCashCustomerSelected => _cashCustomerSelected;

    public frmCustomerPicker(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _ = serviceProvider;
        InitializeComponent();
        BuildMockData();
        SetupGrid();
        ApplyFilter();
        SelectFirstRow();
    }

    /// <summary>
    /// 6 عملاء تجريبيين بأرصدة متفاوتة (له / عليه / خالص) لإظهار ألوان شارات الرصيد.
    /// </summary>
    private void BuildMockData()
    {
        _allCustomers.Clear();

        MockCustomer Make(int id, string code, string name, string phone, string address, decimal balance)
            => new MockCustomer
            {
                Id = id, Code = code, Name = name, Phone = phone, Address = address, Balance = balance
            };

        _allCustomers.AddRange(new[]
        {
            Make(1, "101", "أحمد محمد علي",              "777123456", "شارع الزبيري - صنعاء",   12500m),   // له دفعة سابقة
            Make(2, "102", "شركة النور للتجارة",          "777987654", "شارع الستين - صنعاء",  -85000m),   // عليه للمحل
            Make(3, "103", "مؤسسة اليمن الحديثة",         "733456789", "شارع تعز - عدن",        3750m),   // له دفعة سابقة
            Make(4, "104", "محمد عبدالرحمن السالمي",      "771234567", "حجة - ميدي",           -45000m),   // عليه للمحل
            Make(5, "105", "عبدالله حسن المقطري",         "778889990", "ذمار - شارع جمال",         0m),   // حساب خالص
            Make(6, "106", "محلات البدر للمواد الغذائية", "775556667", "إب - شارع الحرية",          0m)    // حساب خالص
        });
    }

    private void SetupGrid()
    {
        dgvCustomers.AutoGenerateColumns = false;
        dgvCustomers.Columns.Clear();

        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCode", HeaderText = "الكود", DataPropertyName = "Code", Width = 50,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColName", HeaderText = "الاسم", DataPropertyName = "Name", Width = 150
        });
        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColPhone", HeaderText = "الهاتف", DataPropertyName = "Phone", Width = 90,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColAddress", HeaderText = "العنوان", DataPropertyName = "Address",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 90
        });
        dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColBalance", HeaderText = "الرصيد", DataPropertyName = "Balance", Width = 120
        });
    }

    /// <summary>تصفية الشبكة حسب البحث بالاسم / الكود / الهاتف (محاكاة DataView.RowFilter).</summary>
    private void ApplyFilter()
    {
        string q = txtSearch.Text.Trim();
        _selected = null;

        var query = _allCustomers.AsEnumerable();
        if (q.Length > 0)
        {
            query = query.Where(c => c.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                                  || c.Code.Contains(q, StringComparison.OrdinalIgnoreCase)
                                  || c.Phone.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        _visibleCustomers.Clear();
        _visibleCustomers.AddRange(query);

        dgvCustomers.DataSource = null;
        dgvCustomers.DataSource = _visibleCustomers;
        lblCount.Text = $"عدد العملاء: {_visibleCustomers.Count}";
        SelectFirstRow();
    }

    private void SelectFirstRow()
    {
        if (_visibleCustomers.Count > 0)
            dgvCustomers.CurrentCell = dgvCustomers.Rows[0].Cells[0];
    }

    // ═══════════════════════════════════════════════
    // شارة الرصيد — عليه للمحل (أحمر) / له دفعة سابقة (أخضر) / حساب خالص (رمادي)
    // ═══════════════════════════════════════════════
    private static (string Text, Color Fg, Color Bg) GetBadgeInfo(decimal balance)
    {
        if (balance < 0)
            return ("عليه للمحل", Color.FromArgb(176, 32, 50), Color.FromArgb(253, 233, 236));
        if (balance > 0)
            return ("له دفعة سابقة", Color.FromArgb(21, 87, 36), Color.FromArgb(222, 243, 231));
        return ("حساب خالص", Color.FromArgb(73, 80, 87), Color.FromArgb(233, 236, 239));
    }

    private static GraphicsPath CreateRoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private void dgvCustomers_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        int balanceCol = dgvCustomers.Columns["ColBalance"]?.Index ?? -1;
        if (e.RowIndex < 0 || e.ColumnIndex != balanceCol)
            return;

        if (dgvCustomers.Rows[e.RowIndex].DataBoundItem is not MockCustomer customer)
            return;

        // خلفية الخلية (تشمل تظليل التحديد عند اختيار الصف)
        e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

        var (badgeText, fg, bg) = GetBadgeInfo(customer.Balance);

        using var font = new Font("Tahoma", 9F, FontStyle.Bold);
        var textSize = TextRenderer.MeasureText(badgeText, font);
        const int padX = 8;
        const int padY = 5;
        int pillWidth = textSize.Width + padX * 2;
        int pillHeight = Math.Max(24, textSize.Height + padY * 2);
        int pillX = e.CellBounds.X + (e.CellBounds.Width - pillWidth) / 2;
        int pillY = e.CellBounds.Y + (e.CellBounds.Height - pillHeight) / 2;

        using var path = CreateRoundedRect(new Rectangle(pillX, pillY, pillWidth, pillHeight), pillHeight / 2);
        using var brush = new SolidBrush(bg);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.FillPath(brush, path);

        TextRenderer.DrawText(e.Graphics, badgeText, font,
            new Rectangle(pillX, pillY, pillWidth, pillHeight), fg,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

        e.Handled = true;
    }

    private void dgvCustomers_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (dgvCustomers.Rows[e.RowIndex].DataBoundItem is not MockCustomer customer)
            return;

        if (e.ColumnIndex == dgvCustomers.Columns["ColBalance"].Index)
            e.ToolTipText = $"الرصيد: {customer.Balance:N2}";
    }

    // ═══════════════════════════════════════════════
    // التحديد والإغلاق
    // ═══════════════════════════════════════════════
    private void dgvCustomers_SelectionChanged(object? sender, EventArgs e)
    {
        _selected = dgvCustomers.CurrentRow?.DataBoundItem as MockCustomer;
    }

    private void dgvCustomers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            SelectAndClose();
    }

    private void SelectAndClose()
    {
        if (dgvCustomers.CurrentRow?.DataBoundItem is not MockCustomer customer)
        {
            MessageBox.Show("يرجى اختيار عميل من القائمة أولاً.", "اختيار العميل",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _selected = customer;
        DialogResult = DialogResult.OK;
        Close();
    }

    /// <summary>اختيار سريع: فاتورة نقدية عامة بدون عميل محدد.</summary>
    private void btnCashCustomer_Click(object? sender, EventArgs e)
    {
        _cashCustomerSelected = true;
        _selected = null;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnOk_Click(object? sender, EventArgs e) => SelectAndClose();

    private void txtSearch_TextChanged(object? sender, EventArgs e) => ApplyFilter();

    private void frmCustomerPicker_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Escape:
                btnCancel.PerformClick();
                e.Handled = true;
                break;
            case Keys.Enter:
                SelectAndClose();
                e.Handled = true;
                break;
            case Keys.F2:
                txtSearch.Focus();
                txtSearch.SelectAll();
                e.Handled = true;
                break;
        }
    }
}