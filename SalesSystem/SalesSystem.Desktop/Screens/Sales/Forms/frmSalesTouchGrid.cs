namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// شبكة لمس — فاتورة بيع سريعة (UI-first prototype).
/// نسخة أولى ببيانات تجريبية داخل الذاكرة (Mock) بدون أي اعتماد على API أو قاعدة بيانات.
/// تُعرض كـ modal عبر ShowDialog().
///
/// التخطيط:
///   يمين (55%): شبكة أصناف قابلة للمس — صف أزرار تصنيفات + بطاقات أصناف (اسم + كود + رصيد + سعر).
///   يسار (45%): سلة الفاتورة — بطاقات أسطر (اسم + سعر×كمية + أزرار −/+ + إجمالي + حذف) + إجمالي جارٍ + زر حفظ.
/// الضغط على بطاقة صنف يضيف +1 للسلة ويدمج تلقائياً مع السطر الموجود (بمفتاح Id).
/// </summary>
public partial class frmSalesTouchGrid : Form
{
    // ═══════════════════════════════════════════════
    // نماذج Mock داخلية — بدون API / قاعدة بيانات
    // ═══════════════════════════════════════════════
    private sealed class MockProduct
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Stock { get; set; }         // RULE-002: كميات decimal
        public decimal Price { get; set; }         // RULE-001: نقود decimal(18,2)

        public bool OutOfStock => Stock <= 0m;
        public bool LimitedStock => !OutOfStock && Stock <= 10m;
        public string StockText => OutOfStock ? "نفذ" : LimitedStock ? "محدود" : "متوفر";
        public Color StockColor => OutOfStock
            ? Color.FromArgb(220, 53, 69)   // أحمر
            : LimitedStock
                ? Color.FromArgb(253, 126, 20) // برتقالي
                : Color.FromArgb(25, 135, 84);  // أخضر
    }

    /// <summary>سطر في سلة الفاتورة + مراجع عناصر الواجهة لتحديثها فورياً.</summary>
    private sealed class CartLine
    {
        public MockProduct Product { get; }
        public decimal Quantity { get; set; } = 1m;
        public decimal LineTotal => Quantity * Product.Price; // RULE-002: (كمية × سعر)

        public Panel Card = null!;
        public Label LblQty = null!;
        public Label LblPriceQty = null!;
        public Label LblTotal = null!;
        public Button BtnMinus = null!;

        public CartLine(MockProduct product) => Product = product;
    }

    // ═══════════════════════════════════════════════
    // الحالة الداخلية
    // ═══════════════════════════════════════════════
    private readonly List<MockProduct> _allProducts = new();
    private readonly Dictionary<int, CartLine> _cart = new();          // دمج تلقائي بمفتاح Id
    private readonly Dictionary<int, Panel> _productCards = new();
    private readonly List<Button> _categoryPills = new();
    private string _activeCategory = "الكل";

    public frmSalesTouchGrid(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج (RULE-N03) — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _ = serviceProvider;
        InitializeComponent();
        BuildMockData();
        PopulateCategoryPills();
        PopulateProductGrid();
        UpdateCartSummary();
    }

    // ═══════════════════════════════════════════════
    // بيانات تجريبية — 12 صنفاً عبر 4 تصنيفات
    // ═══════════════════════════════════════════════
    private void BuildMockData()
    {
        _allProducts.Clear();

        MockProduct Make(int id, string code, string name, string category, decimal stock, decimal price)
            => new MockProduct { Id = id, Code = code, Name = name, Category = category, Stock = stock, Price = price };

        _allProducts.AddRange(new[]
        {
            Make(1,  "1001", "أرز بسمتي 5 كغ",       "مواد غذائية", 60m,   3500m),
            Make(2,  "1002", "سكر أبيض 1 كغ",        "مواد غذائية", 200m,   800m),
            Make(3,  "1003", "زيت عباد الشمس 1 لتر", "مواد غذائية", 8m,    1500m), // محدود
            Make(4,  "1004", "معجون طماطم 400غ",     "مواد غذائية", 90m,    600m),
            Make(5,  "1005", "دقيق فاخر 2 كغ",       "مواد غذائية", 0m,    1200m), // نفذ
            Make(6,  "2001", "شاي أخضر 250غ",        "مشروبات",     150m,   500m),
            Make(7,  "2002", "مياه معدنية 1.5 لتر",  "مشروبات",     300m,   400m),
            Make(8,  "2003", "عصير برتقال 1 لتر",    "مشروبات",     12m,    900m), // محدود
            Make(9,  "2004", "حليب مجفف 900غ",       "مشروبات",     30m,   4500m),
            Make(10, "3001", "صابون غسيل 1 كغ",      "تنظيف",       75m,    700m),
            Make(11, "3002", "مناديل ورقية 150 ورقة", "تنظيف",      5m,     500m), // محدود
            Make(12, "4001", "شوكولاتة بالحليب 80غ", "أخرى",        40m,    350m)
        });
    }

    // ═══════════════════════════════════════════════
    // أزرار التصنيفات (الكل + مواد غذائية + مشروبات + تنظيف + أخرى)
    // ═══════════════════════════════════════════════
    private void PopulateCategoryPills()
    {
        flpCategories.Controls.Clear();
        _categoryPills.Clear();

        string[] categories = { "الكل", "مواد غذائية", "مشروبات", "تنظيف", "أخرى" };

        foreach (var cat in categories)
        {
            var pill = new Button
            {
                Text = cat,
                Tag = cat,
                AutoSize = true,
                Height = 34,
                Margin = new Padding(0, 0, 6, 0),
                FlatStyle = FlatStyle.Popup,
                Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(33, 43, 54),
                Padding = new Padding(12, 0, 12, 0),
                Cursor = Cursors.Hand
            };
            pill.Click += (s, e) => SelectCategory(cat);
            _categoryPills.Add(pill);
            flpCategories.Controls.Add(pill);
        }

        SelectCategory("الكل");
    }

    private void SelectCategory(string category)
    {
        _activeCategory = category;

        foreach (var pill in _categoryPills)
        {
            bool active = (string?)pill.Tag == category;
            pill.BackColor = active ? Color.FromArgb(13, 110, 253) : Color.FromArgb(248, 249, 250);
            pill.ForeColor = active ? Color.White : Color.FromArgb(33, 43, 54);
        }

        foreach (var kvp in _productCards)
        {
            var product = _allProducts.First(p => p.Id == kvp.Key);
            kvp.Value.Visible = _activeCategory == "الكل" || product.Category == _activeCategory;
        }
    }

    // ═══════════════════════════════════════════════
    // شبكة الأصناف — بطاقات قابلة للمس
    // ═══════════════════════════════════════════════
    private void PopulateProductGrid()
    {
        flpProducts.Controls.Clear();
        _productCards.Clear();

        foreach (var p in _allProducts)
        {
            var card = CreateProductCard(p);
            _productCards[p.Id] = card;
            flpProducts.Controls.Add(card);
        }
    }

    private Panel CreateProductCard(MockProduct p)
    {
        var card = new Panel
        {
            Width = 172,
            Height = 148,
            Margin = new Padding(8),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(6),
            Cursor = Cursors.Hand
        };

        var tbl = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 1,
            RowCount = 4
        };
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));  // اسم الصنف
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));  // الكود
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));  // رصيد badge
        tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // السعر

        var lblName = new Label
        {
            Dock = DockStyle.Fill,
            Text = p.Name,
            Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = p.OutOfStock ? Color.FromArgb(150, 155, 160) : Color.FromArgb(33, 43, 54),
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };

        var lblCode = new Label
        {
            Dock = DockStyle.Fill,
            Text = $"الكود: {p.Code}",
            Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0),
            ForeColor = Color.FromArgb(120, 130, 140),
            TextAlign = ContentAlignment.MiddleLeft
        };

        var lblStock = new Label
        {
            Dock = DockStyle.Fill,
            Text = p.OutOfStock ? "نفذ المخزون" : $"{p.StockText} • {p.Stock:0.###}",
            Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = Color.White,
            BackColor = p.StockColor,
            TextAlign = ContentAlignment.MiddleCenter,
            Padding = new Padding(0, 2, 0, 2)
        };

        var lblPrice = new Label
        {
            Dock = DockStyle.Fill,
            Text = $"السعر: {p.Price:N2}",
            Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = p.OutOfStock ? Color.FromArgb(150, 155, 160) : Color.FromArgb(13, 110, 253),
            TextAlign = ContentAlignment.MiddleRight
        };

        tbl.Controls.Add(lblName, 0, 0);
        tbl.Controls.Add(lblCode, 0, 1);
        tbl.Controls.Add(lblStock, 0, 2);
        tbl.Controls.Add(lblPrice, 0, 3);

        card.Controls.Add(tbl);

        // اللمس في أي مكان من البطاقة يضيف الصنف للسلة
        WireCardClick(card, p);
        WireCardClick(tbl, p);
        WireCardClick(lblName, p);
        WireCardClick(lblCode, p);
        WireCardClick(lblStock, p);
        WireCardClick(lblPrice, p);

        return card;
    }

    private static void WireCardClick(Control c, MockProduct p)
    {
        c.Click += (s, e) => AddToCart(p);
        c.Cursor = Cursors.Hand;
    }

    // ═══════════════════════════════════════════════
    // سلة الفاتورة — إضافة / دمج / تعديل / حذف
    // ═══════════════════════════════════════════════
    private void AddToCart(MockProduct product)
    {
        if (product.OutOfStock)
        {
            MessageBox.Show($"الصنف «{product.Name}» نفذ من المخزون ولا يمكن إضافته.",
                "نفذ المخزون", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_cart.TryGetValue(product.Id, out var existing))
        {
            ChangeQuantity(existing, +1); // دمج: +1 على السطر الموجود
        }
        else
        {
            var line = new CartLine(product);
            _cart[product.Id] = line;
            CreateCartCard(line);
            UpdateCartSummary();
        }
    }

    private void CreateCartCard(CartLine line)
    {
        var card = new Panel
        {
            Height = 108,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(6),
            Margin = new Padding(0, 0, 0, 8)
        };

        var tbl = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 1,
            RowCount = 3
        };
        tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));  // اسم الصنف
        tbl.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));  // سعر × كمية
        tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // أدوات التحكم

        // ── صف 0: اسم الصنف ──
        var lblName = new Label
        {
            Dock = DockStyle.Fill,
            Text = line.Product.Name,
            Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = Color.FromArgb(33, 43, 54),
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = true
        };

        // ── صف 1: سعر × كمية ──
        var lblPriceQty = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Tahoma", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0),
            ForeColor = Color.FromArgb(120, 130, 140),
            TextAlign = ContentAlignment.MiddleLeft
        };

        // ── صف 2: أدوات التحكم (stepper −/+ + إجمالي + حذف) ──
        var tblControls = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 2,
            RowCount = 1
        };
        tblControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        tblControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        tblControls.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // stepper — بترتيب RTL يظهر + يميناً ثم الكمية ثم − يساراً
        var flpStepper = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0)
        };

        var btnPlus = new Button
        {
            Text = "+",
            Width = 44,
            Height = 44,
            Margin = new Padding(0, 0, 4, 0),
            FlatStyle = FlatStyle.Popup,
            Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0),
            BackColor = Color.FromArgb(13, 110, 253),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        var lblQty = new Label
        {
            Text = "1",
            Width = 50,
            Height = 44,
            Margin = new Padding(0, 0, 4, 0),
            Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = Color.FromArgb(33, 43, 54),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var btnMinus = new Button
        {
            Text = "−",
            Width = 44,
            Height = 44,
            Margin = new Padding(0, 0, 4, 0),
            FlatStyle = FlatStyle.Popup,
            Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0),
            BackColor = Color.FromArgb(248, 249, 250),
            ForeColor = Color.FromArgb(33, 43, 54),
            Cursor = Cursors.Hand
        };
        flpStepper.Controls.Add(btnPlus);
        flpStepper.Controls.Add(lblQty);
        flpStepper.Controls.Add(btnMinus);

        // الجانب الأيسر: الإجمالي + حذف
        var tblRight = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            ColumnCount = 2,
            RowCount = 1
        };
        tblRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 74F));
        tblRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var lblTotal = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = Color.FromArgb(21, 87, 36),
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "0.00"
        };
        var btnDelete = new Button
        {
            Dock = DockStyle.Fill,
            Text = "حذف",
            FlatStyle = FlatStyle.Popup,
            Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0),
            BackColor = Color.FromArgb(220, 53, 69),
            ForeColor = Color.White,
            Cursor = Cursors.Hand
        };
        tblRight.Controls.Add(lblTotal, 0, 0);
        tblRight.Controls.Add(btnDelete, 1, 0);

        tblControls.Controls.Add(flpStepper, 0, 0);
        tblControls.Controls.Add(tblRight, 1, 0);

        tbl.Controls.Add(lblName, 0, 0);
        tbl.Controls.Add(lblPriceQty, 0, 1);
        tbl.Controls.Add(tblControls, 0, 2);

        card.Controls.Add(tbl);

        btnPlus.Click += (s, e) => ChangeQuantity(line, +1);
        btnMinus.Click += (s, e) => ChangeQuantity(line, -1);
        btnDelete.Click += (s, e) => RemoveFromCart(line);

        line.Card = card;
        line.LblQty = lblQty;
        line.LblPriceQty = lblPriceQty;
        line.LblTotal = lblTotal;
        line.BtnMinus = btnMinus;

        card.Width = CartCardWidth();
        flpCart.Controls.Add(card);
        flpCart.ScrollControlIntoView(card);

        UpdateLineLabels(line);
    }

    /// <summary>عرض بطاقة السطر ليتناسب مع عرض السلة (عند الإنشاء أو تغيير الحجم).</summary>
    private int CartCardWidth()
    {
        int w = flpCart.ClientSize.Width - 12; // هامش 8 يمين + 4 يسار
        return w < 240 ? 440 : w;              // قبل اكتمال التخطيط ClientSize قد يكون 0
    }

    private void flpCart_Resize(object? sender, EventArgs e)
    {
        foreach (Control c in flpCart.Controls)
            c.Width = CartCardWidth();
    }

    private void ChangeQuantity(CartLine line, int delta)
    {
        decimal newQty = line.Quantity + delta;

        // منع تجاوز الرصيد المتاح (عند الضغط +)
        if (delta > 0 && line.Product.Stock > 0 && newQty > line.Product.Stock)
        {
            newQty = line.Product.Stock;
            MessageBox.Show($"الكمية المطلوبة تتجاوز الرصيد المتاح (الرصيد: {line.Product.Stock:0.###}).",
                "رصيد غير كافٍ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        if (newQty < 1m) newQty = 1m;

        line.Quantity = newQty;
        UpdateLineLabels(line);
        UpdateCartSummary();
    }

    private void UpdateLineLabels(CartLine line)
    {
        line.LblQty.Text = line.Quantity.ToString("0.###");
        line.LblPriceQty.Text = $"{line.Product.Price:N2} × {line.Quantity:0.###}";
        line.LblTotal.Text = line.LineTotal.ToString("N2");
        line.BtnMinus.Enabled = line.Quantity > 1m;
    }

    private void RemoveFromCart(CartLine line)
    {
        flpCart.Controls.Remove(line.Card);
        line.Card.Dispose();
        _cart.Remove(line.Product.Id);
        UpdateCartSummary();
    }

    private void ClearCart()
    {
        foreach (var line in _cart.Values.ToList())
        {
            flpCart.Controls.Remove(line.Card);
            line.Card.Dispose();
        }
        _cart.Clear();
        UpdateCartSummary();
    }

    // ═══════════════════════════════════════════════
    // الملخص الجاري — عدد الأصناف + الإجمالي
    // ═══════════════════════════════════════════════
    private void UpdateCartSummary()
    {
        int count = _cart.Count;
        lblCartCount.Text = count == 0 ? "لا توجد أصناف" : $"{count} صنف";
        lblTotalVal.Text = _cart.Values.Sum(l => l.LineTotal).ToString("N2");
        btnSave.Enabled = count > 0;
    }

    // ═══════════════════════════════════════════════
    // حفظ باللمس — محاكاة استدعاء API في النسخة النهائية
    // ═══════════════════════════════════════════════
    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            if (_cart.Count == 0)
            {
                MessageBox.Show("سلة الفاتورة فارغة — أضف أصنافاً قبل الحفظ.",
                    "حفظ الفاتورة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = _cart.Values.Sum(l => l.LineTotal);

            // محاكاة زمن الاستجابة — في النسخة النهائية يتم استدعاء API عبر ServiceHelper/DI
            await Task.Delay(400);

            MessageBox.Show($"تم حفظ الفاتورة تجريبياً بإجمالي {total:N2} (وضع تجريبي — Mock).",
                "حفظ الفاتورة", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearCart();
        }
        catch
        {
            // لا نعرض تفاصيل الاستثناء للمستخدم (RULE: رسائل المستخدم عامة وبالعربية)
            MessageBox.Show("حدث خطأ غير متوقع أثناء حفظ الفاتورة. يرجى المحاولة مرة أخرى.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void frmSalesTouchGrid_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Escape)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            e.Handled = true;
        }
    }
}