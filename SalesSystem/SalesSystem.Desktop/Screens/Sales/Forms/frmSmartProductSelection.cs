namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة اختيار المنتج [F3] — الواجهة الرئيسية لاختيار الصنف أثناء إدخال فاتورة البيع.
/// نسخة أولية (UI-first prototype) ببيانات تجريبية داخل الذاكرة، بدون أي اعتماد على API أو قاعدة بيانات.
/// تُعرض كـ modal عبر ShowDialog().
/// </summary>
public partial class frmSmartProductSelection : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════
    private sealed class MockProduct
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Stock { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public List<(string Name, decimal Factor)> Units { get; set; } = new();
    }

    private sealed class MockUnit
    {
        public string Name { get; set; } = "";
        public decimal Factor { get; set; } = 1m;
        public override string ToString() => Name;
    }

    private readonly List<MockProduct> _allProducts = new();
    private readonly List<MockProduct> _visibleProducts = new();
    private readonly List<Button> _categoryPills = new();
    private MockProduct? _current;
    private string _activeCategory = "الكل";

    public frmSmartProductSelection(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _ = serviceProvider;
        InitializeComponent();
        BuildMockData();
        SetupGrid();
        PopulateCategoryPills();
        ApplyFilter();
        SelectFirstRow();
    }

    /// <summary>
    /// بيانات تجريبية (10 أصناف) بنفس أسلوب MockData.cs في المشروع — أسعار وتكاليف decimal.
    /// </summary>
    private void BuildMockData()
    {
        _allProducts.Clear();

        MockProduct Make(int id, string code, string name, string category,
                         decimal stock, decimal price, decimal cost, params (string, decimal)[] units)
        {
            return new MockProduct
            {
                Id = id, Code = code, Name = name, Category = category,
                Stock = stock, Price = price, Cost = cost,
                Units = units.Select(u => (u.Item1, u.Item2)).ToList()
            };
        }

        _allProducts.AddRange(new[]
        {
            Make(1, "102030", "شاي ليبتون 250غ", "مشروبات", 150m, 500m, 350m, ("قطعة", 1m), ("كرتون", 12m)),
            Make(2, "102031", "زيت نباتي 1 لتر", "زيوت", 80m, 1500m, 1100m, ("زجاجة", 1m), ("كرتون", 6m)),
            Make(3, "102032", "أرز بسمتي 5 كغ", "حبوب", 60m, 3500m, 2800m, ("كيس", 1m), ("كيس كبير", 2m)),
            Make(4, "102033", "سكر أبيض 1 كغ", "حبوب", 200m, 800m, 620m, ("كيس", 1m), ("كيس كبير", 10m)),
            Make(5, "102034", "دقيق فاخر 2 كغ", "حبوب", 120m, 1200m, 950m, ("كيس", 1m), ("كيس كبير", 4m)),
            Make(6, "102035", "معجون طماطم 400غ", "معلبات", 90m, 600m, 420m, ("علبة", 1m), ("كرتون", 24m)),
            Make(7, "102036", "تونا معلبة 185غ", "معلبات", 45m, 1000m, 750m, ("علبة", 1m), ("كرتون", 24m)),
            Make(8, "102037", "حليب مجفف 900غ", "ألبان", 30m, 4500m, 3800m, ("علبة", 1m), ("كرتون", 12m)),
            Make(9, "102038", "مناديل ورقية 150 ورقة", "منظفات", 200m, 500m, 300m, ("علبة", 1m), ("كرتون", 12m)),
            Make(10, "102039", "صابون غسيل 1 كغ", "منظفات", 75m, 700m, 480m, ("قطعة", 1m), ("كرتون", 12m))
        });
    }

    private void SetupGrid()
    {
        dgvProducts.AutoGenerateColumns = false;
        dgvProducts.Columns.Clear();

        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCode", HeaderText = "كود/باركود", DataPropertyName = "Code", Width = 100
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColName", HeaderText = "اسم المنتج", DataPropertyName = "Name", Width = 200
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCategory", HeaderText = "تصنيف", DataPropertyName = "Category", Width = 90
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColStock", HeaderText = "رصيد", DataPropertyName = "Stock", Width = 70,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "0.###", Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColPrice", HeaderText = "سعر مقترح", DataPropertyName = "Price", Width = 90,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvProducts.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "ColAction", HeaderText = "اختيار", Text = "اختيار", UseColumnTextForButtonValue = true, Width = 80,
            FlatStyle = FlatStyle.Popup
        });
    }

    /// <summary>أزرار التصنيفات (الكل + 5 تصنيفات).</summary>
    private void PopulateCategoryPills()
    {
        flpCategories.Controls.Clear();
        _categoryPills.Clear();

        var categories = new List<string> { "الكل" };
        categories.AddRange(_allProducts.Select(p => p.Category).Distinct().OrderBy(c => c));

        foreach (var cat in categories)
        {
            var pill = new Button
            {
                Text = cat,
                Tag = cat,
                AutoSize = true,
                Height = 30,
                Margin = new Padding(0, 0, 6, 0),
                FlatStyle = FlatStyle.Popup,
                Font = new Font("Tahoma", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(33, 43, 54),
                Padding = new Padding(10, 0, 10, 0),
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
        ApplyFilter();
    }

    /// <summary>تصفية الشبكة حسب البحث والتصنيف (محاكاة DataView.RowFilter).</summary>
    private void ApplyFilter()
    {
        string q = txtSearch.Text.Trim();
        var query = _allProducts.AsEnumerable();

        if (_activeCategory != "الكل")
            query = query.Where(p => p.Category == _activeCategory);

        if (q.Length > 0)
            query = query.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                                  || p.Code.Contains(q, StringComparison.OrdinalIgnoreCase));

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
            FillRightPane(_visibleProducts[0]);
        }
        else
        {
            ClearRightPane();
        }
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e) => ApplyFilter();

    private void dgvProducts_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvProducts.CurrentRow?.DataBoundItem is MockProduct p)
            FillRightPane(p);
    }

    private void dgvProducts_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvProducts.Columns[e.ColumnIndex].Name == "ColAction")
            InsertAndClose();
    }

    private void dgvProducts_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            InsertAndClose();
    }

    // ═══════════════════════════════════════════════
    // اللوحة اليمنى — تعديل البيانات المقترحة
    // ═══════════════════════════════════════════════
    private void FillRightPane(MockProduct p)
    {
        _current = p;
        lblBannerName.Text = p.Name;
        lblBannerBarcode.Text = $"الباركود: {p.Code}";
        lblBannerStock.Text = $"الرصيد: {p.Stock:0.###}";
        lblCostVal.Text = p.Cost.ToString("N2");
        lblDefaultPriceVal.Text = p.Price.ToString("N2");
        lblMinPriceVal.Text = (p.Cost * 1.05m).ToString("N2"); // هامش أمان 5% فوق التكلفة

        cmbUnit.Items.Clear();
        foreach (var u in p.Units)
            cmbUnit.Items.Add(new MockUnit { Name = u.Name, Factor = u.Factor });
        cmbUnit.SelectedIndex = 0;

        numSalePrice.Value = Math.Min(p.Price, numSalePrice.Maximum);
        numQuantity.Value = 1;
        numDiscountValue.Value = 0;
        cmbDiscountType.SelectedIndex = 0;
        dtpExpiry.Value = DateTime.Today.AddMonths(6);
        txtBatch.Text = $"B-{p.Code}";

        UpdateFactorAndTotals();
    }

    private void ClearRightPane()
    {
        _current = null;
        lblBannerName.Text = "اسم المنتج";
        lblBannerBarcode.Text = "الباركود: —";
        lblBannerStock.Text = "الرصيد: —";
        lblCostVal.Text = "0.00";
        lblDefaultPriceVal.Text = "0.00";
        lblMinPriceVal.Text = "0.00";
        cmbUnit.Items.Clear();
        numSalePrice.Value = 0;
        numQuantity.Value = 1;
        numDiscountValue.Value = 0;
        lblNetDiscountVal.Text = "0.00";
        lblNetTotalVal.Text = "0.00";
        lblPriceWarning.Visible = false;
        txtBatch.Clear();
    }

    private void cmbUnit_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbUnit.SelectedItem is MockUnit u && _current != null)
        {
            lblFactorVal.Text = $"{u.Factor:0.##}";
            numSalePrice.Value = Math.Min(_current.Price * u.Factor, numSalePrice.Maximum);
        }
    }

    private void numSalePrice_ValueChanged(object? sender, EventArgs e) => UpdateFactorAndTotals();
    private void numQuantity_ValueChanged(object? sender, EventArgs e) => UpdateFactorAndTotals();
    private void numDiscountValue_ValueChanged(object? sender, EventArgs e) => UpdateFactorAndTotals();
    private void cmbDiscountType_SelectedIndexChanged(object? sender, EventArgs e) => UpdateFactorAndTotals();

    /// <summary>حساب فوري: صافي الخصم + الإجمالي الصافي + تحذير السعر الأقل من التكلفة.</summary>
    private void UpdateFactorAndTotals()
    {
        if (_current == null) return;

        decimal qty = numQuantity.Value;
        decimal unitPrice = numSalePrice.Value;
        decimal gross = qty * unitPrice;

        decimal discount = cmbDiscountType.SelectedIndex == 1
            ? gross * numDiscountValue.Value / 100m
            : numDiscountValue.Value;
        if (discount > gross) discount = gross;

        decimal net = gross - discount;

        lblNetDiscountVal.Text = discount.ToString("N2");
        lblNetTotalVal.Text = net.ToString("N2");

        bool belowCost = unitPrice < _current.Cost;
        lblPriceWarning.Visible = belowCost;
        lblPriceWarning.Text = belowCost
            ? $"تنبيه: سعر البيع ({unitPrice:N2}) أقل من التكلفة ({_current.Cost:N2})"
            : "تنبيه: سعر البيع أقل من التكلفة";
    }

    // ═══════════════════════════════════════════════
    // أزرار الكميات السريعة
    // ═══════════════════════════════════════════════
    private void btnQtyPreset_Click(object? sender, EventArgs e)
    {
        if (sender is Button b && int.TryParse(b.Text.Split(' ')[0], out int qty))
        {
            numQuantity.Value = Math.Min(qty, (int)numQuantity.Maximum);
            numQuantity.Focus();
        }
    }

    // ═══════════════════════════════════════════════
    // الإدراج والإغلاق / الإدراج والمتابعة
    // ═══════════════════════════════════════════════
    private void InsertAndClose()
    {
        if (_current == null)
        {
            MessageBox.Show("يرجى اختيار منتج من القائمة أولاً.", "اختيار المنتج",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // في النسخة النهائية يتم إرسال السطر المحدد عبر EventBus / حدث DataBack.
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnInsertClose_Click(object? sender, EventArgs e) => InsertAndClose();

    private void btnInsertContinue_Click(object? sender, EventArgs e)
    {
        if (_current == null)
        {
            MessageBox.Show("يرجى اختيار منتج من القائمة أولاً.", "اختيار المنتج",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show($"تم إدراج «{_current.Name}» × {numQuantity.Value:0.###} (وضع تجريبي).",
            "إدراج ومتابعة", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // محاكاة: إعادة تعيين الكمية لمواصلة الإدراج.
        numQuantity.Value = 1;
        txtSearch.Focus();
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void frmSmartProductSelection_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.F3:
                txtSearch.Focus();
                txtSearch.SelectAll();
                e.Handled = true;
                break;
            case Keys.Control | Keys.Enter:
                InsertAndClose();
                e.Handled = true;
                break;
            case Keys.F8:
                btnInsertContinue.PerformClick();
                e.Handled = true;
                break;
            case Keys.Escape:
                btnClose.PerformClick();
                e.Handled = true;
                break;
        }
    }
}