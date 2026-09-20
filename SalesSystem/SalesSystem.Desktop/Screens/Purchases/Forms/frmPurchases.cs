using Serilog;
using System.Globalization;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// شاشة المشتريات — نموذج تجريبي (UI-first prototype) يعمل ببيانات وهمية في الذاكرة
/// بدون اتصال بالخادم أو قاعدة البيانات. يغطي: فاتورة مشتريات جديدة، سجل المشتريات، الموردون.
/// </summary>
public partial class frmPurchases : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── Mock data (UI-first prototype — no backend) ──────────────────────────
    private readonly List<MockSupplier> _suppliers = new();
    private readonly List<MockProduct> _products = new();
    private readonly List<MockLine> _invoiceLines = new();
    private readonly List<MockInvoice> _invoices = new();
    private int _nextInvoiceNumber = 7;
    private decimal _grandTotal;
    private decimal _paidAmount;

    private const decimal TaxRate = 0.15m; // 15% ضريبة

    public frmPurchases(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
    }

    // ── Form Events ──────────────────────────────────────────────────────────

    private void frmPurchases_Load(object? sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            PopulateMockData();
            PopulateLookups();
            SetupGrids();
            RefreshLinesGrid();
            RecalculateTotals();
            ApplyInvoiceFilter();
            PopulateSuppliersGrid();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ في تحميل نموذج المشتريات");
            MessageBox.Show("حدث خطأ في تحميل البيانات. يرجى إعادة المحاولة.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ── Mock Data (in-memory) ────────────────────────────────────────────────

    private void PopulateMockData()
    {
        // الموردون (6) — أرصدة بالريال اليمني
        _suppliers.AddRange(new[]
        {
            new MockSupplier { Id = 1, Name = "شركة النور للتجارة", Phone = "771234567", Balance = 1250000.00m, IsActive = true },
            new MockSupplier { Id = 2, Name = "مؤسسة اليمن الحديثة", Phone = "772345678", Balance = 875500.00m, IsActive = true },
            new MockSupplier { Id = 3, Name = "شركة الأمانة للاستيراد", Phone = "773456789", Balance = 0.00m, IsActive = true },
            new MockSupplier { Id = 4, Name = "مؤسسة برهان للتوزيع", Phone = "774567890", Balance = 1650250.75m, IsActive = true },
            new MockSupplier { Id = 5, Name = "شركة الشرق الأوسط", Phone = "775678901", Balance = 420000.00m, IsActive = false },
            new MockSupplier { Id = 6, Name = "مؤسسة الصفا للمواد الغذائية", Phone = "776789012", Balance = 2100000.50m, IsActive = true },
        });

        // الأصناف (8) — التكلفة الشرائية
        _products.AddRange(new[]
        {
            new MockProduct { Name = "سكر أبيض 50 كجم", Unit = "كيس", Cost = 12500.00m },
            new MockProduct { Name = "دقيق قمح فاخر 25 كجم", Unit = "كيس", Cost = 9800.00m },
            new MockProduct { Name = "أرز بسمتي 10 كجم", Unit = "كيس", Cost = 7250.00m },
            new MockProduct { Name = "زيت ذرة 5 لتر", Unit = "عبوة", Cost = 6400.00m },
            new MockProduct { Name = "حليب بودرة 2.5 كجم", Unit = "علبة", Cost = 4950.00m },
            new MockProduct { Name = "شاي أخضر 1 كجم", Unit = "علبة", Cost = 3800.00m },
            new MockProduct { Name = "ملح طعام 1 كجم", Unit = "كيس", Cost = 450.00m },
            new MockProduct { Name = "مكرونة 400 جم", Unit = "عبوة", Cost = 320.00m },
        });

        // أصناف الفاتورة الجديدة (4 أسطر تجريبية)
        _invoiceLines.AddRange(new[]
        {
            new MockLine { ProductName = "سكر أبيض 50 كجم", Unit = "كيس", Quantity = 20m, UnitCost = 12500.00m, Discount = 0m, BatchNo = "B-2025-001", ExpiryDate = new DateTime(2027, 12, 31) },
            new MockLine { ProductName = "دقيق قمح فاخر 25 كجم", Unit = "كيس", Quantity = 15m, UnitCost = 9800.00m, Discount = 1000.00m, BatchNo = "B-2025-002", ExpiryDate = new DateTime(2026, 11, 30) },
            new MockLine { ProductName = "أرز بسمتي 10 كجم", Unit = "كيس", Quantity = 10m, UnitCost = 7250.00m, Discount = 0m, BatchNo = "B-2025-003", ExpiryDate = new DateTime(2028, 6, 30) },
            new MockLine { ProductName = "زيت ذرة 5 لتر", Unit = "عبوة", Quantity = 24m, UnitCost = 6400.00m, Discount = 0m, BatchNo = "B-2025-004", ExpiryDate = new DateTime(2027, 9, 30) },
        });

        // فواتير المشتريات (6) — عبر الحالات الثلاث (مسودة / مرحل / ملغي)
        _invoices.AddRange(new[]
        {
            new MockInvoice { InvoiceNo = "PUR-2025-0001", SupplierName = "شركة النور للتجارة", Date = new DateTime(2025, 9, 2), PaymentType = "نقدي", Status = 2, Total = 850000.00m, Paid = 850000.00m },
            new MockInvoice { InvoiceNo = "PUR-2025-0002", SupplierName = "مؤسسة اليمن الحديثة", Date = new DateTime(2025, 9, 5), PaymentType = "آجل", Status = 2, Total = 1250000.00m, Paid = 500000.00m },
            new MockInvoice { InvoiceNo = "PUR-2025-0003", SupplierName = "شركة الأمانة للاستيراد", Date = new DateTime(2025, 9, 10), PaymentType = "نقدي", Status = 2, Total = 436500.00m, Paid = 436500.00m },
            new MockInvoice { InvoiceNo = "PUR-2025-0004", SupplierName = "مؤسسة برهان للتوزيع", Date = new DateTime(2025, 9, 14), PaymentType = "دفع مجزأ", Status = 2, Total = 714415.00m, Paid = 214415.00m },
            new MockInvoice { InvoiceNo = "PUR-2025-0005", SupplierName = "شركة النور للتجارة", Date = new DateTime(2025, 9, 16), PaymentType = "آجل", Status = 3, Total = 320000.00m, Paid = 0m },
            new MockInvoice { InvoiceNo = "PUR-2025-0006", SupplierName = "مؤسسة الصفا للمواد الغذائية", Date = new DateTime(2025, 9, 18), PaymentType = "نقدي", Status = 1, Total = 1980000.00m, Paid = 0m },
        });
    }

    // ── Lookups & Setup ──────────────────────────────────────────────────────

    private void PopulateLookups()
    {
        cmbSupplier.Items.Clear();
        foreach (var s in _suppliers)
            cmbSupplier.Items.Add(s.Name);
        if (cmbSupplier.Items.Count > 0) cmbSupplier.SelectedIndex = 0;

        cmbWarehouse.Items.AddRange(new object[] { "المستودع الرئيسي", "مستودع الجملة", "مستودع التجزئة" });
        cmbWarehouse.SelectedIndex = 0;

        cmbPaymentType.Items.AddRange(new object[] { "نقدي", "آجل", "دفع مجزأ" });
        cmbPaymentType.SelectedIndex = 0;

        cbPayTypeFilter.Items.AddRange(new object[] { "الكل", "نقدي", "آجل", "دفع مجزأ" });
        cbPayTypeFilter.SelectedIndex = 0;

        txtInvoiceNo.Text = "جديد";
        dtpInvoiceDate.Value = DateTime.Now;
        dtpFrom.Value = DateTime.Today.AddDays(-30);
        dtpTo.Value = DateTime.Today;
        txtPaid.Text = "400000";

        lblFooterHint.Text =
            $"وضع التجربة (Mock) — {_suppliers.Count} مورد و{_products.Count} صنفًا ببيانات وهمية، بدون اتصال بالخادم";
    }

    private void SetupGrids()
    {
        // ── جدول أصناف الفاتورة (8 أعمدة) ──
        dgvItems.AutoGenerateColumns = false;
        dgvItems.Columns.Clear();
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColProduct", HeaderText = "اسم الصنف", Width = 220 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColUnit", HeaderText = "الوحدة", Width = 75 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColQty", HeaderText = "الكمية", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColCost", HeaderText = "التكلفة", Width = 105, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDisc", HeaderText = "الخصم", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColBatch", HeaderText = "رقم التشغيلة", Width = 130 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColExpiry", HeaderText = "تاريخ الصلاحية", Width = 120 });
        dgvItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColTotal", HeaderText = "الإجمالي", Width = 130,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                ForeColor = Color.Blue,
                Font = new Font(dgvItems.Font, FontStyle.Bold)
            }
        });

        // ── جدول سجل المشتريات (6 أعمدة) ──
        dgvRecords.AutoGenerateColumns = false;
        dgvRecords.Columns.Clear();
        dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColInvNo", HeaderText = "رقم الفاتورة", Width = 140 });
        dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColSupplier", HeaderText = "المورد", Width = 280 });
        dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColDate", HeaderText = "التاريخ", Width = 120 });
        dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColTotal", HeaderText = "الإجمالي", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPaid", HeaderText = "المدفوع", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColStatus", HeaderText = "الحالة", Width = 100 });
        dgvRecords.CellFormatting += dgvRecords_CellFormatting;

        // ── جدول الموردين (5 أعمدة) ──
        dgvSuppliers.AutoGenerateColumns = false;
        dgvSuppliers.Columns.Clear();
        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColId", HeaderText = "الرقم", Width = 100 });
        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColName", HeaderText = "الاسم", Width = 320 });
        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColPhone", HeaderText = "الهاتف", Width = 160 });
        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColBalance", HeaderText = "الرصيد", Width = 160, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColActive", HeaderText = "الحالة", Width = 100 });
        dgvSuppliers.CellFormatting += dgvSuppliers_CellFormatting;
    }

    // ── Tab 1: New Invoice ───────────────────────────────────────────────────

    private void RefreshLinesGrid()
    {
        dgvItems.Rows.Clear();
        foreach (var line in _invoiceLines)
        {
            dgvItems.Rows.Add(
                line.ProductName,
                line.Unit,
                line.Quantity.ToString("N3"),
                line.UnitCost.ToString("N2"),
                line.Discount.ToString("N2"),
                line.BatchNo,
                line.ExpiryDate?.ToString("yyyy/MM/dd") ?? "",
                line.LineTotal.ToString("N2"));
        }
    }

    private void RecalculateTotals()
    {
        decimal subTotal = _invoiceLines.Sum(l => l.LineTotal);
        decimal discount = _invoiceLines.Sum(l => l.Discount);
        decimal tax = subTotal * TaxRate;
        _grandTotal = subTotal - discount + tax;

        lblSubTotalVal.Text = subTotal.ToString("N2");
        lblDiscountVal.Text = discount.ToString("N2");
        lblTaxVal.Text = tax.ToString("N2");
        lblGrandTotalVal.Text = _grandTotal.ToString("N2");
        RecalculateRemaining();
    }

    private void RecalculateRemaining()
    {
        if (!decimal.TryParse(txtPaid.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out _paidAmount))
            _paidAmount = 0m;

        decimal remaining = _grandTotal - _paidAmount;
        lblRemainingVal.Text = remaining.ToString("N2");
        lblRemainingVal.ForeColor = remaining > 0 ? Color.Red : Color.Green;
    }

    private void txtPaid_TextChanged(object? sender, EventArgs e) => RecalculateRemaining();

    private void btnSavePost_Click(object? sender, EventArgs e)
    {
        try
        {
            if (cmbSupplier.SelectedIndex < 0 || cmbWarehouse.SelectedIndex < 0)
            {
                MessageBox.Show("يرجى اختيار المورد والمستودع قبل الحفظ.", "بيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_invoiceLines.Count == 0)
            {
                MessageBox.Show("يجب إضافة صنف واحد على الأقل للفاتورة.", "بيانات ناقصة",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (decimal.TryParse(txtPaid.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out _paidAmount) == false)
                _paidAmount = 0m;

            // إضافة الفاتورة الجديدة إلى السجل (حالة: مرحل) — تجريبياً بدون خادم
            var invoice = new MockInvoice
            {
                InvoiceNo = $"PUR-2025-{_nextInvoiceNumber:0000}",
                SupplierName = cmbSupplier.Text,
                Date = dtpInvoiceDate.Value,
                PaymentType = cmbPaymentType.Text,
                Status = 2, // مرحل
                Total = _grandTotal,
                Paid = _paidAmount
            };
            _invoices.Add(invoice);
            _nextInvoiceNumber++;

            ApplyInvoiceFilter();

            MessageBox.Show($"تم حفظ وترحيل فاتورة المشتريات ({invoice.InvoiceNo}) بنجاح — وضع تجريبي.",
                "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ResetNewInvoiceTab();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "حدث خطأ أثناء حفظ فاتورة المشتريات (تجريبي)");
            MessageBox.Show("حدث خطأ أثناء الحفظ. تم تسجيل التفاصيل للدعم الفني.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ResetNewInvoiceTab()
    {
        txtInvoiceNo.Text = "جديد";
        dtpInvoiceDate.Value = DateTime.Now;
        cmbPaymentType.SelectedIndex = 0;
        txtPaid.Text = "0";
        _invoiceLines.Clear();
        RefreshLinesGrid();
        RecalculateTotals();
        dgvItems.Refresh();
    }

    // ── Tab 2: Purchase History ──────────────────────────────────────────────

    private void ApplyInvoiceFilter()
    {
        string search = txtSearch.Text.Trim();
        string payType = cbPayTypeFilter.Text;
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        var filtered = _invoices.Where(inv =>
            (string.IsNullOrEmpty(search) || inv.InvoiceNo.Contains(search) || inv.SupplierName.Contains(search)) &&
            (payType == "الكل" || inv.PaymentType == payType) &&
            inv.Date.Date >= from && inv.Date.Date <= to).ToList();

        dgvRecords.Rows.Clear();
        foreach (var inv in filtered)
        {
            dgvRecords.Rows.Add(
                inv.InvoiceNo,
                inv.SupplierName,
                inv.Date.ToString("yyyy/MM/dd"),
                inv.Total.ToString("N2"),
                inv.Paid.ToString("N2"),
                GetStatusText(inv.Status));
        }

        lblHistoryCount.Text = $"عدد السجلات: {filtered.Count}";
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e) => ApplyInvoiceFilter();

    private void cbPayTypeFilter_SelectedIndexChanged(object? sender, EventArgs e) => ApplyInvoiceFilter();

    private void dtpFrom_ValueChanged(object? sender, EventArgs e) => ApplyInvoiceFilter();

    private void dtpTo_ValueChanged(object? sender, EventArgs e) => ApplyInvoiceFilter();

    private void dgvRecords_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex == 5 && e.Value != null)
        {
            e.CellStyle.ForeColor = e.Value.ToString() switch
            {
                "مرحل" => Color.Green,
                "ملغي" => Color.Red,
                "مسودة" => Color.Blue,
                _ => Color.Black
            };
            e.CellStyle.Font = new Font(dgvRecords.Font, FontStyle.Bold);
        }
    }

    // ── Tab 3: Suppliers ─────────────────────────────────────────────────────

    private void PopulateSuppliersGrid()
    {
        dgvSuppliers.Rows.Clear();
        foreach (var s in _suppliers)
        {
            dgvSuppliers.Rows.Add(
                s.Id,
                s.Name,
                s.Phone,
                s.Balance.ToString("N2"),
                s.IsActive ? "نشط" : "موقوف");
        }

        lblSuppliersCount.Text = $"عدد الموردين: {_suppliers.Count}";
    }

    private void btnAddSupplier_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("سيتم فتح نافذة «إضافة مورد جديد» هنا — وضع تجريبي.",
            "إضافة مورد", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void dgvSuppliers_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex == 4 && e.Value != null)
        {
            e.CellStyle.ForeColor = e.Value.ToString() switch
            {
                "نشط" => Color.Green,
                "موقوف" => Color.Red,
                _ => Color.Black
            };
            e.CellStyle.Font = new Font(dgvSuppliers.Font, FontStyle.Bold);
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string GetStatusText(int status) => status switch
    {
        1 => "مسودة",
        2 => "مرحل",
        3 => "ملغي",
        _ => "غير معروف"
    };

    // ── Mock view models (UI shape only) ─────────────────────────────────────

    private sealed class MockSupplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }

    private sealed class MockProduct
    {
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
        public decimal Cost { get; set; }
    }

    private sealed class MockLine
    {
        public string ProductName { get; set; } = "";
        public string Unit { get; set; } = "";
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Discount { get; set; }
        public string BatchNo { get; set; } = "";
        public DateTime? ExpiryDate { get; set; }

        // LineTotal = (Quantity * UnitCost) - DiscountAmount
        public decimal LineTotal => (Quantity * UnitCost) - Discount;
    }

    private sealed class MockInvoice
    {
        public string InvoiceNo { get; set; } = "";
        public string SupplierName { get; set; } = "";
        public DateTime Date { get; set; }
        public string PaymentType { get; set; } = "";
        public int Status { get; set; } // 1 مسودة، 2 مرحل، 3 ملغي
        public decimal Total { get; set; }
        public decimal Paid { get; set; }
    }
}