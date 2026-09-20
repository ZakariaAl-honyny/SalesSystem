using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// تعديل داخلي — فاتورة بيع (Inline Grid) — نموذج واجهة (UI Shape) ببيانات تجريبية فقط.
/// بدون أي اتصال بـ API أو قاعدة بيانات؛ يُعرض modal عبر ShowDialog().
///
/// التخطيط:
///   1) شريط عنوان (Dock=Top): «تعديل داخلي — فاتورة بيع».
///   2) ترويسة الفاتورة (Dock=Top): رقم الفاتورة (للقراءة فقط) + العميل (ComboBox + شارة الرصيد)
///      + نوع الدفع (نقدي/آجل) + ملاحظات عامة.
///   3) شبكة تحرير داخلي (Dock=Fill): الوحدة (ComboBox) | اسم الصنف | الكمية (N3) | السعر (N2)
///      | الخصم (N2) | الإجمالي (محسوب — للقراءة فقط) | ملاحظات السطر.
///      الإجمالي = (الكمية × السعر) − الخصم — يُعاد حسابه فورياً عند تعديل أي خلية (RULE-001/002: نقود وكميات decimal).
///   4) قائمة سياقية على الشبكة: تعديل كمية السطر (Enter) / إضافة خصم مباشر / حذف السطر (Delete) / إدراج سطر جديد أسفل.
///   5) تذييل (Dock=Bottom): عدد الأسطر + الإجمالي المبدئي + إجمالي الخصم + الصافي النهائي + اعتماد/إغلاق.
/// </summary>
public partial class frmSalesInlineGrid : Form
{
    // ═══════════════════════════════════════════════
    // نماذج Mock داخلية — بدون API / قاعدة بيانات
    // ═══════════════════════════════════════════════
    private sealed class MockProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
        public decimal Price { get; set; }   // RULE-001: نقود decimal(18,2)
    }

    private sealed class MockCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Balance { get; set; } // رصيد مستحق — RULE-001
    }

    // الوحدات المتاحة في عمود «الوحدة» (DataGridViewComboBoxColumn)
    private static readonly string[] Units =
        { "قطعة", "علبة", "كرتونة", "كيس", "لتر", "كغ", "زجاجة" };

    private readonly IServiceProvider _serviceProvider;
    private readonly List<MockProduct> _products = new();
    private readonly List<MockCustomer> _customers = new();
    private DataTable _linesTable = new();

    public frmSalesInlineGrid(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج (RULE-N03) — الخدمات تُحل عند الربط بالخلفية الفعلية.
        _serviceProvider = serviceProvider;
        InitializeComponent();
        BuildMockData();
        SetupGrid();
        PopulateHeader();
        SeedInvoiceLines();
        UpdateTotals();
    }

    // ═══════════════════════════════════════════════
    // البيانات التجريبية — 6 أصناف + 4 عملاء
    // ═══════════════════════════════════════════════
    private void BuildMockData()
    {
        _products.Clear();
        _products.AddRange(new[]
        {
            new MockProduct { Id = 1, Name = "أرز بسمتي 5 كغ",        Unit = "كيس",     Price = 3500.00m },
            new MockProduct { Id = 2, Name = "سكر أبيض 1 كغ",         Unit = "كيس",     Price = 800.00m  },
            new MockProduct { Id = 3, Name = "زيت عباد الشمس 1 لتر",  Unit = "زجاجة",   Price = 1500.00m },
            new MockProduct { Id = 4, Name = "معجون طماطم 400غ",      Unit = "علبة",    Price = 600.00m  },
            new MockProduct { Id = 5, Name = "دقيق فاخر 2 كغ",        Unit = "كيس",     Price = 1200.00m },
            new MockProduct { Id = 6, Name = "شاي أخضر 250غ",         Unit = "علبة",    Price = 500.00m  }
        });

        _customers.Clear();
        _customers.AddRange(new[]
        {
            new MockCustomer { Id = 1, Name = "عميل نقدي عام",            Balance = 0.00m      },
            new MockCustomer { Id = 2, Name = "شركة الأمانة للتوريدات",   Balance = 25000.00m  },
            new MockCustomer { Id = 3, Name = "مؤسسة النور التجارية",     Balance = 8750.50m   },
            new MockCustomer { Id = 4, Name = "محل الركن الأخضر",         Balance = 0.00m      }
        });
    }

    // ═══════════════════════════════════════════════
    // إعداد الشبكة — أعمدة تحرير داخلي مرتبطة بـ DataTable
    // ═══════════════════════════════════════════════
    private void SetupGrid()
    {
        _linesTable = new DataTable();
        _linesTable.Columns.Add("Unit", typeof(string));
        _linesTable.Columns.Add("ItemName", typeof(string));
        _linesTable.Columns.Add("Quantity", typeof(decimal));   // RULE-002
        _linesTable.Columns.Add("Price", typeof(decimal));      // RULE-001
        _linesTable.Columns.Add("Discount", typeof(decimal));   // RULE-001
        _linesTable.Columns.Add("LineTotal", typeof(decimal));  // محسوب
        _linesTable.Columns.Add("Notes", typeof(string));

        dgvLines.AutoGenerateColumns = false;
        dgvLines.Columns.Clear();

        var colUnit = new DataGridViewComboBoxColumn
        {
            Name = "colUnit",
            HeaderText = "الوحدة",
            DataPropertyName = "Unit",
            FlatStyle = FlatStyle.Flat,
            DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
            FillWeight = 10F,
            MinimumWidth = 70
        };
        colUnit.Items.AddRange(Units);
        dgvLines.Columns.Add(colUnit);

        dgvLines.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colItemName",
            HeaderText = "اسم الصنف",
            DataPropertyName = "ItemName",
            FillWeight = 24F,
            MinimumWidth = 180,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
        });

        dgvLines.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colQuantity",
            HeaderText = "الكمية",
            DataPropertyName = "Quantity",
            FillWeight = 10F,
            MinimumWidth = 80,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N3",
                Alignment = DataGridViewContentAlignment.MiddleRight
            }
        });

        dgvLines.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colPrice",
            HeaderText = "السعر",
            DataPropertyName = "Price",
            FillWeight = 13F,
            MinimumWidth = 90,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight
            }
        });

        dgvLines.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colDiscount",
            HeaderText = "الخصم",
            DataPropertyName = "Discount",
            FillWeight = 13F,
            MinimumWidth = 90,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight
            }
        });

        dgvLines.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colLineTotal",
            HeaderText = "الإجمالي",
            DataPropertyName = "LineTotal",
            FillWeight = 13F,
            MinimumWidth = 100,
            ReadOnly = true, // عمود محسوب — للقراءة فقط
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(21, 87, 36)
            }
        });

        dgvLines.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colNotes",
            HeaderText = "ملاحظات السطر",
            DataPropertyName = "Notes",
            FillWeight = 17F,
            MinimumWidth = 140,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
        });

        dgvLines.DataSource = _linesTable;
    }

    // ═══════════════════════════════════════════════
    // ترويسة الفاتورة
    // ═══════════════════════════════════════════════
    private void PopulateHeader()
    {
        txtInvoiceNo.Text = "1024"; // رقم فاتورة تجريبي — للقراءة فقط

        cbCustomer.DataSource = _customers;
        cbCustomer.DisplayMember = nameof(MockCustomer.Name);
        cbCustomer.SelectedIndex = 0;

        cbPaymentType.SelectedIndex = 0; // نقدي

        UpdateBalanceBadge();
    }

    private void UpdateBalanceBadge()
    {
        var customer = cbCustomer.SelectedItem as MockCustomer;
        if (customer == null) return;

        lblBalance.Text = $"الرصيد: {customer.Balance:N2}";
        bool hasDebt = customer.Balance > 0m;
        lblBalance.BackColor = hasDebt ? Color.FromArgb(253, 237, 237) : Color.FromArgb(233, 236, 239);
        lblBalance.ForeColor = hasDebt ? Color.FromArgb(220, 53, 69) : Color.FromArgb(80, 90, 100);
    }

    private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        => UpdateBalanceBadge();

    // ═══════════════════════════════════════════════
    // ملء الفاتورة بأسطر تجريبية (6 أصناف) + سطر فارغ للإضافة
    // ═══════════════════════════════════════════════
    private void SeedInvoiceLines()
    {
        _linesTable.Rows.Clear();

        var qtyByProduct = new Dictionary<int, decimal>
        {
            { 1, 2m }, { 2, 5m }, { 3, 3m }, { 4, 10m }, { 5, 4m }, { 6, 6m }
        };

        foreach (var p in _products)
        {
            decimal qty = qtyByProduct.GetValueOrDefault(p.Id, 1m);
            decimal discount = p.Id == 1 ? 100.00m : 0.00m; // خصم تجريبي على أول سطر
            decimal lineTotal = (qty * p.Price) - discount;  // RULE-002: (كمية × سعر) − خصم

            _linesTable.Rows.Add(p.Unit, p.Name, qty, p.Price, discount, lineTotal, "");
        }
    }

    // ═══════════════════════════════════════════════
    // الحسابات الحية — الإجمالي + ملخص التذييل
    // ═══════════════════════════════════════════════
    private void RecalculateRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= dgvLines.Rows.Count) return;

        var row = dgvLines.Rows[rowIndex];
        decimal qty = ToDecimal(row.Cells["colQuantity"].Value);
        decimal price = ToDecimal(row.Cells["colPrice"].Value);
        decimal discount = ToDecimal(row.Cells["colDiscount"].Value);
        decimal lineTotal = (qty * price) - discount; // RULE-002

        row.Cells["colLineTotal"].Value = lineTotal;
    }

    private void dgvLines_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvLines.Columns[e.ColumnIndex].Name == "colLineTotal") return; // تجنب إعادة الدخول

        RecalculateRow(e.RowIndex);
        UpdateTotals();
    }

    private (int Lines, decimal Gross, decimal Discount, decimal Net) ComputeTotals()
    {
        int lines = 0;
        decimal gross = 0m, discount = 0m, net = 0m;

        foreach (DataGridViewRow row in dgvLines.Rows)
        {
            if (row.IsNewRow) continue; // صف الإضافة الفارغ لا يُحسب

            string item = row.Cells["colItemName"].Value as string;
            if (string.IsNullOrWhiteSpace(item)) continue;

            decimal qty = ToDecimal(row.Cells["colQuantity"].Value);
            decimal price = ToDecimal(row.Cells["colPrice"].Value);
            decimal disc = ToDecimal(row.Cells["colDiscount"].Value);

            lines++;
            gross += qty * price;
            discount += disc;
            net += (qty * price) - disc;
        }

        return (lines, gross, discount, net);
    }

    private void UpdateTotals()
    {
        var totals = ComputeTotals();

        lblLinesValue.Text = totals.Lines.ToString();
        lblGrossValue.Text = totals.Gross.ToString("N2");
        lblDiscountValue.Text = totals.Discount.ToString("N2");
        lblNetValue.Text = totals.Net.ToString("N2");
    }

    // ═══════════════════════════════════════════════
    // القائمة السياقية — عمليات السطر
    // ═══════════════════════════════════════════════
    private void cmsLines_Opening(object sender, CancelEventArgs e)
    {
        var row = dgvLines.CurrentRow;
        bool hasRow = row != null;
        bool hasLine = hasRow && !row.IsNewRow;

        miEditQty.Enabled = hasRow;
        miAddDiscount.Enabled = hasLine;
        miDeleteLine.Enabled = hasLine;
        miInsertBelow.Enabled = true;
    }

    private void dgvLines_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        // النقر بالزر الأيمن يحدد السطر قبل فتح القائمة السياقية
        if (e.Button == MouseButtons.Right &&
            e.RowIndex >= 0 && e.RowIndex < dgvLines.Rows.Count)
        {
            dgvLines.CurrentCell = dgvLines.Rows[e.RowIndex].Cells[e.ColumnIndex >= 0 ? e.ColumnIndex : 0];
        }
    }

    private void miEditQty_Click(object sender, EventArgs e)
        => EditQuantityOfCurrentLine();

    private void miAddDiscount_Click(object sender, EventArgs e)
        => AddDirectDiscountToCurrentLine();

    private void miDeleteLine_Click(object sender, EventArgs e)
        => DeleteCurrentLine();

    private void miInsertBelow_Click(object sender, EventArgs e)
        => InsertLineBelowCurrent();

    private void EditQuantityOfCurrentLine()
    {
        var row = dgvLines.CurrentRow;
        if (row == null) return;

        decimal current = ToDecimal(row.Cells["colQuantity"].Value);
        using var prompt = new NumericPrompt("تعديل كمية السطر", "الكمية:", current);

        if (prompt.ShowDialog(this) != DialogResult.OK) return;

        if (prompt.Value <= 0m)
        {
            MessageBox.Show("يجب أن تكون الكمية أكبر من صفر.", "قيمة غير صالحة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        row.Cells["colQuantity"].Value = prompt.Value;
        RecalculateRow(row.Index);
        UpdateTotals();
    }

    private void AddDirectDiscountToCurrentLine()
    {
        var row = dgvLines.CurrentRow;
        if (row == null || row.IsNewRow) return;

        decimal current = ToDecimal(row.Cells["colDiscount"].Value);
        using var prompt = new NumericPrompt("إضافة خصم مباشر", "مبلغ الخصم الإضافي:", 0m);

        if (prompt.ShowDialog(this) != DialogResult.OK) return;

        if (prompt.Value < 0m)
        {
            MessageBox.Show("لا يمكن إدخال خصم سالب.", "قيمة غير صالحة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        row.Cells["colDiscount"].Value = current + prompt.Value;
        RecalculateRow(row.Index);
        UpdateTotals();
    }

    private void DeleteCurrentLine()
    {
        var row = dgvLines.CurrentRow;
        if (row == null || row.IsNewRow) return;

        string item = row.Cells["colItemName"].Value as string ?? "";
        if (MessageBox.Show($"هل تريد حذف السطر «{item}»؟", "حذف السطر",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        dgvLines.EndEdit();
        _linesTable.Rows[row.Index].Delete();
        UpdateTotals();
    }

    private void InsertLineBelowCurrent()
    {
        dgvLines.EndEdit();

        var row = dgvLines.CurrentRow;
        int insertAt = _linesTable.Rows.Count;
        if (row != null && !row.IsNewRow) insertAt = row.Index + 1;

        DataRow newRow = _linesTable.NewRow();
        newRow["Unit"] = Units[0];   // قطعة
        newRow["ItemName"] = "";
        newRow["Quantity"] = 1m;
        newRow["Price"] = 0m;
        newRow["Discount"] = 0m;
        newRow["LineTotal"] = 0m;
        newRow["Notes"] = "";

        _linesTable.Rows.InsertAt(newRow, insertAt);
        UpdateTotals();

        if (insertAt < dgvLines.Rows.Count)
        {
            dgvLines.CurrentCell = dgvLines.Rows[insertAt].Cells["colItemName"];
            dgvLines.BeginEdit(true);
        }
    }

    // ═══════════════════════════════════════════════
    // أحداث لوحة المفاتيح — Enter / Delete / Esc
    // ═══════════════════════════════════════════════
    private void dgvLines_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            if (!dgvLines.IsCurrentCellInEditMode)
            {
                e.Handled = true;
                EditQuantityOfCurrentLine();
            }
            // أثناء التحرير: Enter الافتراضي يُثبت الخلية وينتقل للسطر التالي
        }
        else if (e.KeyCode == Keys.Delete && !dgvLines.IsCurrentCellInEditMode)
        {
            e.Handled = true;
            DeleteCurrentLine();
        }
    }

    private void frmSalesInlineGrid_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Escape)
        {
            DialogResult = DialogResult.Cancel;
            Close();
            e.Handled = true;
        }
    }

    // ═══════════════════════════════════════════════
    // الصف الفارغ للإضافة — قيم افتراضية
    // ═══════════════════════════════════════════════
    private void dgvLines_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
    {
        e.Row.Cells["colUnit"].Value = Units[0];
        e.Row.Cells["colQuantity"].Value = 1m;
        e.Row.Cells["colPrice"].Value = 0m;
        e.Row.Cells["colDiscount"].Value = 0m;
        e.Row.Cells["colLineTotal"].Value = 0m;
    }

    // ═══════════════════════════════════════════════
    // اعتماد / إغلاق
    // ═══════════════════════════════════════════════
    private async void btnSave_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            var totals = ComputeTotals();

            if (totals.Lines == 0)
            {
                MessageBox.Show("لا توجد أسطر صالحة في الفاتورة — أضف صنفاً واحداً على الأقل.",
                    "اعتماد الفاتورة", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string customer = (cbCustomer.SelectedItem as MockCustomer)?.Name ?? "";
            string paymentType = cbPaymentType.SelectedItem?.ToString() ?? "";

            // محاكاة زمن الاستجابة — في النسخة النهائية يتم استدعاء API عبر ServiceHelper/DI
            await Task.Delay(400);

            MessageBox.Show(
                $"تم اعتماد الفاتورة رقم {txtInvoiceNo.Text} تجريبياً (وضع Mock).\n\n" +
                $"العميل: {customer}\nنوع الدفع: {paymentType}\n" +
                $"عدد الأسطر: {totals.Lines}\nالإجمالي المبدئي: {totals.Gross:N2}\n" +
                $"إجمالي الخصم: {totals.Discount:N2}\nالصافي النهائي: {totals.Net:N2}",
                "اعتماد الفاتورة", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch
        {
            // لا نعرض تفاصيل الاستثناء للمستخدم (رسائل المستخدم عامة وبالعربية)
            MessageBox.Show("حدث خطأ غير متوقع أثناء اعتماد الفاتورة. يرجى المحاولة مرة أخرى.",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ═══════════════════════════════════════════════
    // مساعدات
    // ═══════════════════════════════════════════════
    private static decimal ToDecimal(object? value)
    {
        if (value is decimal d) return d;
        if (value is null) return 0m;

        string s = value.ToString() ?? "";
        if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out var a)) return a;
        if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var b)) return b;
        return 0m;
    }

    /// <summary>حوار إدخال رقمي بسيط (Mock) لتعديل الكمية أو إضافة الخصم المباشر.</summary>
    private sealed class NumericPrompt : Form
    {
        private readonly TextBox _txt;

        public decimal Value { get; private set; }

        public NumericPrompt(string title, string label, decimal initial)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(360, 148);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;

            var lbl = new Label
            {
                Text = label,
                AutoSize = true,
                Location = new Point(12, 12),
                Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0)
            };

            _txt = new TextBox
            {
                Location = new Point(12, 40),
                Width = 336,
                Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Text = initial.ToString("0.###", CultureInfo.InvariantCulture),
                TextAlign = HorizontalAlignment.Center
            };

            var btnOk = new Button
            {
                Text = "موافق",
                DialogResult = DialogResult.OK,
                Location = new Point(200, 86),
                Width = 90,
                Height = 34,
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Popup,
                Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Cursor = Cursors.Hand
            };

            var btnCancel = new Button
            {
                Text = "إلغاء",
                DialogResult = DialogResult.Cancel,
                Location = new Point(104, 86),
                Width = 90,
                Height = 34,
                BackColor = Color.FromArgb(233, 236, 239),
                FlatStyle = FlatStyle.Popup,
                Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Cursor = Cursors.Hand
            };

            Controls.Add(lbl);
            Controls.Add(_txt);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            Shown += (s, e) =>
            {
                _txt.SelectAll();
                _txt.Focus();
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (!decimal.TryParse(_txt.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var v) &&
                    !decimal.TryParse(_txt.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                {
                    MessageBox.Show("الرجاء إدخال قيمة رقمية صحيحة.", "قيمة غير صالحة",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                Value = v;
            }
            base.OnFormClosing(e);
        }
    }
}