using System.Drawing.Drawing2D;

namespace SalesSystem.Desktop.Screens.Sales.Forms;

/// <summary>
/// نافذة «تحويل عرض سعر / تكرار فاتورة» — UI-first prototype ببيانات تجريبية داخل الذاكرة فقط
/// (بدون API / قاعدة بيانات). تُعرض كـ modal عبر ShowDialog().
/// في النسخة النهائية تُجلب عروض الأسعار المعتمدة والفواتير السابقة من الـ API
/// ويتم التحويل / التكرار عبر خدمة فاتورة البيع مع الحفاظ على المستند المصدر.
/// </summary>
public partial class frmSalesImportConvert : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════

    /// <summary>حالة عرض السعر: معتمد (أخضر) / بانتظار التعميد (كهرماني).</summary>
    private enum QuotationStatus
    {
        Approved,          // معتمد — جاهز للتحويل
        PendingApproval    // بانتظار التعميد
    }

    /// <summary>عرض سعر تجريبي. الإجمالي بالريال اليمني (YER).</summary>
    private sealed class MockQuotation
    {
        public string Number { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public DateTime Date { get; set; }
        public int ItemCount { get; set; }          // عدد الأصناف
        public decimal Total { get; set; }          // الإجمالي (decimal — ريال يمني)
        public QuotationStatus Status { get; set; }
        public string StatusText => Status == QuotationStatus.Approved ? "معتمد" : "بانتظار التعميد";
    }

    /// <summary>فاتورة سابقة تجريبية للتكرار. الإجمالي بالريال اليمني (YER).</summary>
    private sealed class MockInvoice
    {
        public string Number { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public DateTime Date { get; set; }
        public decimal Total { get; set; }          // الإجمالي (decimal — ريال يمني)
    }

    private readonly IServiceProvider _serviceProvider;

    private readonly List<MockQuotation> _quotations = new();
    private readonly List<MockInvoice> _invoices = new();

    // ═══════════════════════════════════════════════
    // نتيجة النافذة للمتصل (نموذج أولي)
    // ═══════════════════════════════════════════════
    /// <summary>true عند تنفيذ «تحويل واستيراد» لعرض سعر معتمد.</summary>
    public bool IsConvertAction { get; private set; }

    /// <summary>true عند تنفيذ «تكرار» لفاتورة سابقة.</summary>
    public bool IsDuplicateAction { get; private set; }

    /// <summary>رقم عرض السعر المحوّل (فارغ عند تكرار فاتورة).</summary>
    public string SelectedQuotationNumber { get; private set; } = "";

    /// <summary>رقم الفاتورة المكرّرة (فارغ عند تحويل عرض سعر).</summary>
    public string SelectedInvoiceNumber { get; private set; } = "";

    /// <summary>اسم العميل المرتبط بالعملية المنفذة.</summary>
    public string SelectedCustomerName { get; private set; } = "";

    public frmSalesImportConvert(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _serviceProvider = serviceProvider;
        InitializeComponent();
        BuildMockData();
        SetupQuotationGrid();
        SetupInvoiceGrid();
        UpdateCountLabel();
    }

    // ═══════════════════════════════════════════════
    // بناء البيانات التجريبية
    // ═══════════════════════════════════════════════
    /// <summary>
    /// 4 عروض أسعار (معتمد / بانتظار التعميد) + 4 فواتير سابقة،
    /// بإجماليات بالريال اليمني (YER) وأسماء عملاء متنوعة.
    /// </summary>
    private void BuildMockData()
    {
        _quotations.Clear();
        _invoices.Clear();

        _quotations.AddRange(new[]
        {
            new MockQuotation { Number = "عرض-2026-001", CustomerName = "أحمد محمد علي",              Date = new DateTime(2026, 9, 14), ItemCount = 3, Total = 125000m, Status = QuotationStatus.Approved },
            new MockQuotation { Number = "عرض-2026-002", CustomerName = "شركة النور للتجارة",          Date = new DateTime(2026, 9, 16), ItemCount = 7, Total = 486500m, Status = QuotationStatus.PendingApproval },
            new MockQuotation { Number = "عرض-2026-003", CustomerName = "مؤسسة اليمن الحديثة",         Date = new DateTime(2026, 9, 18), ItemCount = 5, Total = 96250m,  Status = QuotationStatus.Approved },
            new MockQuotation { Number = "عرض-2026-004", CustomerName = "محمد عبدالرحمن السالمي",      Date = new DateTime(2026, 9, 19), ItemCount = 2, Total = 24800m,  Status = QuotationStatus.PendingApproval }
        });

        _invoices.AddRange(new[]
        {
            new MockInvoice { Number = "ف-2026-1201", CustomerName = "أحمد محمد علي",              Date = new DateTime(2026, 9, 5),  Total = 183750m },
            new MockInvoice { Number = "ف-2026-1202", CustomerName = "محلات البدر للمواد الغذائية", Date = new DateTime(2026, 9, 8),  Total = 96400m  },
            new MockInvoice { Number = "ف-2026-1203", CustomerName = "شركة النور للتجارة",          Date = new DateTime(2026, 9, 11), Total = 421900m },
            new MockInvoice { Number = "ف-2026-1204", CustomerName = "عبدالله حسن المقطري",         Date = new DateTime(2026, 9, 13), Total = 58300m  }
        });
    }

    // ═══════════════════════════════════════════════
    // إعداد الشبكات
    // ═══════════════════════════════════════════════
    private void SetupQuotationGrid()
    {
        dgvQuotations.AutoGenerateColumns = false;
        dgvQuotations.Columns.Clear();

        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColQuotNumber", HeaderText = "رقم العرض", DataPropertyName = "Number", Width = 115,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColQuotCustomer", HeaderText = "العميل", DataPropertyName = "CustomerName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 140
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColQuotDate", HeaderText = "التاريخ", DataPropertyName = "Date", Width = 95,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "yyyy/MM/dd",
                Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColQuotItems", HeaderText = "عدد الأصناف", DataPropertyName = "ItemCount", Width = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColQuotTotal", HeaderText = "الإجمالي", DataPropertyName = "Total", Width = 115,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 110, 253)
            }
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColQuotStatus", HeaderText = "الحالة", DataPropertyName = "StatusText", Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvQuotations.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "ColQuotConvert", HeaderText = "تحويل", Width = 120,
            UseColumnTextForButtonValue = true, Text = "تحويل واستيراد",
            FlatStyle = FlatStyle.Flat,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(25, 135, 84),
                SelectionForeColor = Color.White
            }
        });

        dgvQuotations.DataSource = _quotations;
    }

    private void SetupInvoiceGrid()
    {
        dgvInvoices.AutoGenerateColumns = false;
        dgvInvoices.Columns.Clear();

        dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColInvNumber", HeaderText = "رقم الفاتورة", DataPropertyName = "Number", Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColInvCustomer", HeaderText = "العميل", DataPropertyName = "CustomerName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 160
        });
        dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColInvDate", HeaderText = "التاريخ", DataPropertyName = "Date", Width = 110,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "yyyy/MM/dd",
                Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        });
        dgvInvoices.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColInvTotal", HeaderText = "الإجمالي", DataPropertyName = "Total", Width = 130,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2",
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Font = new Font("Tahoma", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 110, 253)
            }
        });
        dgvInvoices.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "ColInvDuplicate", HeaderText = "تكرار", Width = 95,
            UseColumnTextForButtonValue = true, Text = "تكرار",
            FlatStyle = FlatStyle.Flat,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                SelectionBackColor = Color.FromArgb(25, 135, 84),
                SelectionForeColor = Color.White
            }
        });

        dgvInvoices.DataSource = _invoices;
    }

    // ═══════════════════════════════════════════════
    // شارة الحالة — معتمد (أخضر) / بانتظار التعميد (كهرماني)
    // ═══════════════════════════════════════════════
    private static (string Text, Color Fg, Color Bg) GetStatusBadgeInfo(QuotationStatus status)
    {
        if (status == QuotationStatus.Approved)
            return ("معتمد", Color.FromArgb(21, 87, 36), Color.FromArgb(222, 243, 231));       // أخضر
        return ("بانتظار التعميد", Color.FromArgb(122, 79, 0), Color.FromArgb(255, 244, 204)); // كهرماني
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

    private void dgvQuotations_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        int statusCol = dgvQuotations.Columns["ColQuotStatus"]?.Index ?? -1;
        if (e.RowIndex < 0 || e.ColumnIndex != statusCol)
            return;

        if (dgvQuotations.Rows[e.RowIndex].DataBoundItem is not MockQuotation quotation)
            return;

        // خلفية الخلية (تشمل تظليل التحديد عند اختيار الصف)
        e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

        var (badgeText, fg, bg) = GetStatusBadgeInfo(quotation.Status);

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

    private void dgvQuotations_CellToolTipTextNeeded(object? sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (dgvQuotations.Rows[e.RowIndex].DataBoundItem is not MockQuotation quotation)
            return;

        if (e.ColumnIndex == dgvQuotations.Columns["ColQuotStatus"].Index)
            e.ToolTipText = quotation.Status == QuotationStatus.Approved
                ? "معتمد — جاهز للتحويل إلى فاتورة بيع"
                : "بانتظار التعميد — يُحول بعد اعتماده من المسؤول";
    }

    // ═══════════════════════════════════════════════
    // تحويل عرض سعر إلى فاتورة
    // ═══════════════════════════════════════════════
    private void dgvQuotations_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != dgvQuotations.Columns["ColQuotConvert"].Index)
            return;

        if (dgvQuotations.Rows[e.RowIndex].DataBoundItem is MockQuotation quotation)
            TryConvert(quotation);
    }

    private void dgvQuotations_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        // النقر المزدوج على الصف = تحويل (زر «تحويل واستيراد» يعمل بالنقرة الواحدة)
        if (e.RowIndex < 0 || e.ColumnIndex == dgvQuotations.Columns["ColQuotConvert"].Index)
            return;

        if (dgvQuotations.Rows[e.RowIndex].DataBoundItem is MockQuotation quotation)
            TryConvert(quotation);
    }

    private void TryConvert(MockQuotation quotation)
    {
        // 1) تحذير عند تحويل عرض غير معتمد بعد
        if (quotation.Status == QuotationStatus.PendingApproval)
        {
            string warnMsg =
                $"عرض السعر «{quotation.Number}» لم يُعتمد بعد (بانتظار التعميد)." +
                Environment.NewLine + "هل تريد المتابعة بالتحويل على الرغم من ذلك؟";

            var warn = MessageBox.Show(warnMsg, "تحويل عرض سعر",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (warn != DialogResult.Yes) return;
        }

        // 2) تأكيد التحويل
        string confirmMsg =
            $"هل تريد تحويل عرض السعر «{quotation.Number}» إلى فاتورة بيع للعميل «{quotation.CustomerName}»؟" +
            Environment.NewLine +
            $"عدد الأصناف: {quotation.ItemCount} — الإجمالي: {quotation.Total:N2} ريال يمني";

        var confirm = MessageBox.Show(confirmMsg, "تحويل عرض سعر",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        // 3) تسجيل النتيجة وإغلاق الحوار (وضع تجريبي)
        IsConvertAction = true;
        SelectedQuotationNumber = quotation.Number;
        SelectedCustomerName = quotation.CustomerName;

        string doneMsg =
            $"تم تحويل عرض السعر «{quotation.Number}» إلى فاتورة بيع بنجاح (وضع تجريبي)." +
            Environment.NewLine +
            "سيتم تعبئة أصناف الفاتورة مع الحفاظ على الوحدات والأسعار والخصومات من المستند المصدر.";

        MessageBox.Show(doneMsg, "تحويل عرض سعر",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    // ═══════════════════════════════════════════════
    // تكرار فاتورة سابقة
    // ═══════════════════════════════════════════════
    private void dgvInvoices_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != dgvInvoices.Columns["ColInvDuplicate"].Index)
            return;

        if (dgvInvoices.Rows[e.RowIndex].DataBoundItem is MockInvoice invoice)
            TryDuplicate(invoice);
    }

    private void dgvInvoices_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        // النقر المزدوج على الصف = تكرار (زر «تكرار» يعمل بالنقرة الواحدة)
        if (e.RowIndex < 0 || e.ColumnIndex == dgvInvoices.Columns["ColInvDuplicate"].Index)
            return;

        if (dgvInvoices.Rows[e.RowIndex].DataBoundItem is MockInvoice invoice)
            TryDuplicate(invoice);
    }

    private void TryDuplicate(MockInvoice invoice)
    {
        string confirmMsg =
            $"هل تريد تكرار الفاتورة «{invoice.Number}» للعميل «{invoice.CustomerName}»؟" +
            Environment.NewLine +
            $"الإجمالي: {invoice.Total:N2} ريال يمني — سيتم إنشاء فاتورة جديدة بنفس الأصناف.";

        var confirm = MessageBox.Show(confirmMsg, "تكرار فاتورة سابقة",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        // تسجيل النتيجة وإغلاق الحوار (وضع تجريبي)
        IsDuplicateAction = true;
        SelectedInvoiceNumber = invoice.Number;
        SelectedCustomerName = invoice.CustomerName;

        string doneMsg =
            $"تم تكرار الفاتورة «{invoice.Number}» بنجاح (وضع تجريبي)." +
            Environment.NewLine +
            "سيتم إنشاء فاتورة جديدة بنفس الأصناف والأسعار والخصومات.";

        MessageBox.Show(doneMsg, "تكرار فاتورة سابقة",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    // ═══════════════════════════════════════════════
    // عداد التبويب الحالي + اختصارات لوحة المفاتيح
    // ═══════════════════════════════════════════════
    private void tabMain_SelectedIndexChanged(object? sender, EventArgs e) => UpdateCountLabel();

    private void UpdateCountLabel()
    {
        lblCount.Text = tabMain.SelectedIndex == 0
            ? $"عروض الأسعار: {_quotations.Count}"
            : $"الفواتير السابقة: {_invoices.Count}";
    }

    private void frmSalesImportConvert_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.Escape:
                btnClose.PerformClick(); // إغلاق (Cancel)
                e.Handled = true;
                break;

            case Keys.Enter:
                // Enter ينفذ إجراء الصف الحالي حسب التبويب النشط
                if (tabMain.SelectedIndex == 0)
                {
                    if (dgvQuotations.CurrentRow?.DataBoundItem is MockQuotation quotation)
                    {
                        TryConvert(quotation);
                        e.Handled = true;
                    }
                }
                else if (dgvInvoices.CurrentRow?.DataBoundItem is MockInvoice invoice)
                {
                    TryDuplicate(invoice);
                    e.Handled = true;
                }
                break;
        }
    }
}