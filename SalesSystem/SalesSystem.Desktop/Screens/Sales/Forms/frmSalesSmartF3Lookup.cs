namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة البحث الذكي عن المنتج [F3] — نسخة أولية (UI-first prototype) ببيانات تجريبية
/// داخل الذاكرة (Mock) وبدون أي اعتماد على API أو قاعدة بيانات.
/// تُعرض كـ modal عبر ShowDialog()، ويرجع المنتج/الوحدة المختارة عبر خصائص القراءة فقط.
/// </summary>
public partial class frmSalesSmartF3Lookup : Form
{
    // ═══════════════════════════════════════════════
    // نماذج البيانات التجريبية (Mock — بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════
    private sealed class MockUnit
    {
        public string Name { get; set; } = "";
        public decimal Factor { get; set; } = 1m;
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
    }

    private sealed class MockProduct
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public List<MockUnit> Units { get; set; } = new();
        public decimal BasePrice => Units.Count > 0 ? Units[0].Price : 0m;
        public decimal BaseStock => Units.Count > 0 ? Units[0].Stock : 0m;
    }

    // ═══════════════════════════════════════════════
    // الحالة الداخلية
    // ═══════════════════════════════════════════════
    private readonly List<MockProduct> _allProducts = new();
    private readonly List<MockProduct> _visibleProducts = new();
    private MockProduct? _current;

    // ═══════════════════════════════════════════════
    // نتائج الاختيار — تُقرأ من المتصل بعد ShowDialog()
    // ═══════════════════════════════════════════════
    public int? SelectedProductId { get; private set; }
    public string? SelectedProductName { get; private set; }
    public string? SelectedProductCode { get; private set; }
    public string? SelectedUnitName { get; private set; }
    public decimal SelectedUnitFactor { get; private set; }
    public decimal SelectedUnitPrice { get; private set; }

    public bool IsSelected => SelectedProductId.HasValue;

    public frmSalesSmartF3Lookup(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _ = serviceProvider;
        InitializeComponent();
        flpUnits.Resize += flpUnits_Resize;
        BuildMockData();
        SetupGrid();
        ApplyFilter();
    }

    /// <summary>تركيز تلقائي على حقل البحث عند فتح النافذة.</summary>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        txtSearch.Focus();
        txtSearch.SelectAll();
    }

    // ═══════════════════════════════════════════════
    // بيانات تجريبية (8 أصناف) — أسعار وكميات decimal
    // ═══════════════════════════════════════════════
    private void BuildMockData()
    {
        _allProducts.Clear();

        MockProduct Make(int id, string code, string name, string category,
                         params (string Unit, decimal Factor, decimal Price, decimal Stock)[] units)
        {
            return new MockProduct
            {
                Id = id,
                Code = code,
                Name = name,
                Category = category,
                Units = units.Select(u => new MockUnit
                {
                    Name = u.Unit, Factor = u.Factor, Price = u.Price, Stock = u.Stock
                }).ToList()
            };
        }

        _allProducts.AddRange(new[]
        {
            Make(1, "102030", "شاي ليبتون 250غ", "مشروبات",
                ("قطعة", 1m, 500m, 150m), ("كرتون", 12m, 5600m, 10m)),
            Make(2, "102031", "زيت نباتي 1 لتر", "زيوت",
                ("زجاجة", 1m, 1500m, 80m), ("كرتون", 6m, 8400m, 15m)),
            Make(3, "102032", "أرز بسمتي 5 كغ", "حبوب",
                ("كيس", 1m, 3500m, 60m), ("كيس كبير", 2m, 6800m, 20m), ("جوال", 25m, 82500m, 4m)),
            Make(4, "102033", "سكر أبيض 1 كغ", "حبوب",
                ("كيس", 1m, 800m, 200m), ("كيس كبير", 10m, 7800m, 30m)),
            Make(5, "102034", "دقيق فاخر 2 كغ", "حبوب",
                ("كيس", 1m, 1200m, 120m), ("كيس كبير", 4m, 4600m, 25m)),
            Make(6, "102035", "معجون طماطم 400غ", "معلبات",
                ("علبة", 1m, 600m, 90m), ("كرتون", 24m, 13500m, 8m)),
            Make(7, "102036", "تونة معلبة 185غ", "معلبات",
                ("علبة", 1m, 1000m, 45m), ("كرتون", 24m, 22800m, 6m)),
            Make(8, "102037", "حليب مجفف 900غ", "ألبان",
                ("علبة", 1m, 4500m, 30m), ("كرتون", 12m, 52000m, 5m), ("ربع كرتون", 3m, 13200m, 10m))
        });
    }

    // ═══════════════════════════════════════════════
    // الشبكة اليسرى — قائمة المنتجات
    // ═══════════════════════════════════════════════
    private void SetupGrid()
    {
        dgvProducts.AutoGenerateColumns = false;
        dgvProducts.Columns.Clear();

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColName", HeaderText = "الاسم", DataPropertyName = "Name",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            MinimumWidth = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCode", HeaderText = "الكود", DataPropertyName = "Code", Width = 80,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCategory", HeaderText = "التصنيف", DataPropertyName = "Category", Width = 80,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColPrice", HeaderText = "السعر", DataPropertyName = "BasePrice", Width = 80,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColStock", HeaderText = "الرصيد", DataPropertyName = "BaseStock", Width = 70,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "0.###", Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
    }

    /// <summary>تصفية حسب الاسم / الكود / الباركود / التصنيف (محاكاة DataView.RowFilter).</summary>
    private void ApplyFilter()
    {
        string q = txtSearch.Text.Trim();
        var query = _allProducts.AsEnumerable();

        if (q.Length > 0)
        {
            query = query.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                                  || p.Code.Contains(q, StringComparison.OrdinalIgnoreCase)
                                  || p.Category.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        _visibleProducts.Clear();
        _visibleProducts.AddRange(query);

        dgvProducts.DataSource = null;
        dgvProducts.DataSource = _visibleProducts;
        SelectFirstRow();
    }

    private void SelectFirstRow()
    {
        if (_visibleProducts.Count > 0)
        {
            dgvProducts.ClearSelection();
            dgvProducts.Rows[0].Selected = true;
            if (dgvProducts.CurrentRow?.DataBoundItem is MockProduct p)
                FillUnitPane(p);
        }
        else
        {
            ClearUnitPane();
        }
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e) => ApplyFilter();

    private void dgvProducts_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvProducts.CurrentRow?.DataBoundItem is MockProduct p)
            FillUnitPane(p);
    }

    private void dgvProducts_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            btnInsertBase.PerformClick();
    }

    // ═══════════════════════════════════════════════
    // اللوحة اليمنى — وحدات المنتج المختار
    // ═══════════════════════════════════════════════
    private void FillUnitPane(MockProduct p)
    {
        _current = p;

        flpUnits.SuspendLayout();
        flpUnits.Controls.Clear();
        foreach (var u in p.Units)
            flpUnits.Controls.Add(CreateUnitRow(p, u));
        flpUnits.ResumeLayout(true);

        UpdateTotalStock();
    }

    private void ClearUnitPane()
    {
        _current = null;
        flpUnits.Controls.Clear();
        lblTotalStock.Text = "إجمالي الرصيد: —";
    }

    /// <summary>
    /// صف وحدة: الوحدة | معامل التحويل | السعر | زر إدراج.
    /// زر الإدراج يحمل (المنتج، الوحدة) في خاصية Tag ليربط النقر بالسطر الصحيح.
    /// </summary>
    private Panel CreateUnitRow(MockProduct p, MockUnit u)
    {
        var row = new Panel
        {
            Width = Math.Max(flpUnits.ClientSize.Width - 8, 200),
            Height = 48,
            Margin = new Padding(0, 0, 0, 6),
            Padding = new Padding(6, 4, 6, 4),
            BackColor = Color.White
        };
        row.Paint += (s, e) => ControlPaint.DrawBorder(
            e.Graphics, row.ClientRectangle, Color.FromArgb(222, 226, 230), ButtonBorderStyle.Solid);

        var tbl = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1
        };
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84F));

        var lblUnitName = new Label
        {
            Dock = DockStyle.Fill,
            Text = u.Name,
            Font = new Font("Tahoma", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(33, 43, 54),
            TextAlign = ContentAlignment.MiddleLeft
        };
        var lblFactor = new Label
        {
            Dock = DockStyle.Fill,
            Text = $"معامل: × {u.Factor:0.##}",
            Font = new Font("Tahoma", 9F, FontStyle.Regular),
            ForeColor = Color.FromArgb(80, 90, 100),
            TextAlign = ContentAlignment.MiddleLeft
        };
        var lblPrice = new Label
        {
            Dock = DockStyle.Fill,
            Text = u.Price.ToString("N2"),
            Font = new Font("Tahoma", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(21, 87, 36),
            TextAlign = ContentAlignment.MiddleRight
        };
        var btnInsert = new Button
        {
            Text = "إدراج",
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Popup,
            BackColor = Color.FromArgb(13, 110, 253),
            ForeColor = Color.White,
            Font = new Font("Tahoma", 9F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            Tag = (p, u)
        };
        btnInsert.Click += btnInsertUnit_Click;

        tbl.Controls.Add(lblUnitName, 0, 0);
        tbl.Controls.Add(lblFactor, 1, 0);
        tbl.Controls.Add(lblPrice, 2, 0);
        tbl.Controls.Add(btnInsert, 3, 0);

        row.Controls.Add(tbl);
        return row;
    }

    /// <summary>عند تغيير عرض اللوحة تتمدد صفوف الوحدات لملء العرض.</summary>
    private void flpUnits_Resize(object? sender, EventArgs e)
    {
        foreach (Control c in flpUnits.Controls)
            c.Width = Math.Max(flpUnits.ClientSize.Width - 8, 200);
    }

    /// <summary>إجمالي الرصيد محسوباً بالوحدة الأساسية (رصيد كل وحدة × معاملها).</summary>
    private void UpdateTotalStock()
    {
        if (_current == null)
        {
            lblTotalStock.Text = "إجمالي الرصيد: —";
            return;
        }

        decimal total = _current.Units.Sum(u => u.Stock * u.Factor);
        lblTotalStock.Text = $"إجمالي الرصيد (بالوحدة الأساسية): {total:0.###}";
    }

    // ═══════════════════════════════════════════════
    // الإدراج والإغلاق
    // ═══════════════════════════════════════════════
    private void btnInsertUnit_Click(object? sender, EventArgs e)
    {
        if (sender is Button b && b.Tag is (MockProduct p, MockUnit u))
            InsertAndClose(p, u);
    }

    private void btnInsertBase_Click(object? sender, EventArgs e)
    {
        if (_current == null)
        {
            MessageBox.Show("يرجى اختيار منتج من القائمة أولاً.", "اختيار المنتج",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var baseUnit = _current.Units.FirstOrDefault()
            ?? new MockUnit { Name = "قطعة", Factor = 1m, Price = _current.BasePrice };
        InsertAndClose(_current, baseUnit);
    }

    private void InsertAndClose(MockProduct p, MockUnit u)
    {
        // في النسخة النهائية يتم إرسال السطر المحدد عبر EventBus / حدث DataBack.
        SelectedProductId = p.Id;
        SelectedProductName = p.Name;
        SelectedProductCode = p.Code;
        SelectedUnitName = u.Name;
        SelectedUnitFactor = u.Factor;
        SelectedUnitPrice = u.Price;

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void frmSalesSmartF3Lookup_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.F3:
                txtSearch.Focus();
                txtSearch.SelectAll();
                e.Handled = true;
                break;
            case Keys.Enter:
                btnInsertBase.PerformClick();
                e.Handled = true;
                break;
            case Keys.Escape:
                btnClose.PerformClick();
                e.Handled = true;
                break;
        }
    }
}