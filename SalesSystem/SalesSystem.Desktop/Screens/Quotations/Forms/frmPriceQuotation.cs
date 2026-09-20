using System.Drawing.Drawing2D;

namespace SalesSystem.Desktop.Screens.Quotations.Forms;

/// <summary>
/// نافذة «عروض الأسعار» — UI-first prototype ببيانات تجريبية داخل الذاكرة فقط
/// (بدون API / قاعدة بيانات). تُعرض كـ modal عبر ShowDialog().
/// في النسخة النهائية تُجلب عروض الأسعار من الـ API وتُحوَّل المعتمدة منها
/// إلى فاتورة بيع عبر خدمة فواتير البيع مع الحفاظ على المستند المصدر.
/// </summary>
public partial class frmPriceQuotation : Form
{
    // ═══════════════════════════════════════════════
    // بيانات تجريبية — Mock (بدون API / قاعدة بيانات)
    // ═══════════════════════════════════════════════

    /// <summary>حالة عرض السعر: معتمد (أخضر) / بانتظار التعميد (كهرماني).</summary>
    private enum QuotationStatus
    {
        Approved,          // معتمد — جاهز للتحويل إلى فاتورة
        PendingApproval    // بانتظار التعميد
    }

    /// <summary>عرض سعر تجريبي. الإجمالي decimal بالريال اليمني (YER).</summary>
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

    private readonly IServiceProvider _serviceProvider;

    /// <summary>القائمة الرئيسية الكاملة — تُستخدم لإعادة التصفية دون فقدان البيانات.</summary>
    private readonly List<MockQuotation> _allQuotations = new();

    /// <summary>الصفوف المعروضة حالياً في الشبكة بعد التصفية.</summary>
    private List<MockQuotation> _visibleQuotations = new();

    public frmPriceQuotation(IServiceProvider serviceProvider)
    {
        // نمط DI إلزامي لكل النماذج — الخدمات تُحل عند ربط الواجهة بالخلفية الفعلية.
        _serviceProvider = serviceProvider;
        InitializeComponent();
        BuildMockData();
        SetupGridColumns();

        // نطاق التاريخ الافتراضي: أول الشهر الحالي → اليوم
        dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpTo.Value = DateTime.Today;

        cbStatus.SelectedIndex = 0; // الكل
        ApplyFilters();
    }

    // ═══════════════════════════════════════════════
    // بناء البيانات التجريبية
    // ═══════════════════════════════════════════════
    /// <summary>
    /// 6 عروض أسعار موزعة بين (معتمد / بانتظار التعميد) بإجماليات
    /// بالريال اليمني (YER) وأسماء عملاء متنوعة. التواريخ نسبية لليوم
    /// الحالي كي تبقى ضمن نطاق «من/إلى» الافتراضي في أي وقت تشغيل.
    /// </summary>
    private void BuildMockData()
    {
        _allQuotations.Clear();
        DateTime today = DateTime.Today;

        _allQuotations.AddRange(new[]
        {
            new MockQuotation { Number = "عرض-2026-005", CustomerName = "أحمد محمد علي",              Date = today.AddDays(-19), ItemCount = 3, Total = 125000m,  Status = QuotationStatus.Approved },
            new MockQuotation { Number = "عرض-2026-006", CustomerName = "شركة النور للتجارة",          Date = today.AddDays(-16), ItemCount = 7, Total = 486500m,  Status = QuotationStatus.PendingApproval },
            new MockQuotation { Number = "عرض-2026-007", CustomerName = "مؤسسة اليمن الحديثة",         Date = today.AddDays(-11), ItemCount = 5, Total = 96250m,   Status = QuotationStatus.Approved },
            new MockQuotation { Number = "عرض-2026-008", CustomerName = "محمد عبدالرحمن السالمي",      Date = today.AddDays(-9),  ItemCount = 2, Total = 24800m,   Status = QuotationStatus.PendingApproval },
            new MockQuotation { Number = "عرض-2026-009", CustomerName = "محلات البدر للمواد الغذائية", Date = today.AddDays(-6),  ItemCount = 4, Total = 318400m,  Status = QuotationStatus.Approved },
            new MockQuotation { Number = "عرض-2026-010", CustomerName = "عبدالله حسن المقطري",         Date = today.AddDays(-3),  ItemCount = 6, Total = 573200m,  Status = QuotationStatus.PendingApproval },
        });
    }

    // ═══════════════════════════════════════════════
    // إعداد الشبكة
    // ═══════════════════════════════════════════════
    private void SetupGridColumns()
    {
        dgvQuotations.AutoGenerateColumns = false;
        dgvQuotations.Columns.Clear();

        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColNumber", HeaderText = "رقم العرض", DataPropertyName = "Number", Width = 115,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColCustomer", HeaderText = "العميل", DataPropertyName = "CustomerName",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 160
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColDate", HeaderText = "التاريخ", DataPropertyName = "Date", Width = 95,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "yyyy/MM/dd",
                Alignment = DataGridViewContentAlignment.MiddleCenter
            }
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColItems", HeaderText = "عدد الأصناف", DataPropertyName = "ItemCount", Width = 85,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvQuotations.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "ColTotal", HeaderText = "الإجمالي", DataPropertyName = "Total", Width = 115,
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
            Name = "ColStatus", HeaderText = "الحالة", DataPropertyName = "StatusText", Width = 110,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvQuotations.Columns.Add(new DataGridViewButtonColumn
        {
            Name = "ColConvert", HeaderText = "تحويل", Width = 130,
            UseColumnTextForButtonValue = true, Text = "تحويل إلى فاتورة",
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

    // ═══════════════════════════════════════════════
    // التصفية: بحث (بالعميل/الرقم) + الحالة + من/إلى التاريخ
    // ═══════════════════════════════════════════════
    private void ApplyFilters()
    {
        string search = txtSearch.Text.Trim();
        string statusFilter = cbStatus.SelectedItem as string ?? "الكل";
        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;

        _visibleQuotations = _allQuotations
            .Where(q =>
                (string.IsNullOrEmpty(search)
                 || q.Number.Contains(search, StringComparison.OrdinalIgnoreCase)
                 || q.CustomerName.Contains(search, StringComparison.OrdinalIgnoreCase))
                && (statusFilter == "الكل"
                    || (statusFilter == "معتمد" && q.Status == QuotationStatus.Approved)
                    || (statusFilter == "بانتظار التعميد" && q.Status == QuotationStatus.PendingApproval))
                && q.Date.Date >= from && q.Date.Date <= to)
            .ToList();

        dgvQuotations.DataSource = _visibleQuotations;
        lblRecordsCount.Text = _visibleQuotations.Count.ToString();
    }

    // ═══════════════════════════════════════════════
    // أحداث التصفية — بحث / حالة / نطاق تاريخ
    // ═══════════════════════════════════════════════
    private void txtSearch_TextChanged(object? sender, EventArgs e) => ApplyFilters();

    private void cbStatus_SelectedIndexChanged(object? sender, EventArgs e) => ApplyFilters();

    private void dtpFrom_ValueChanged(object? sender, EventArgs e) => ApplyFilters();

    private void dtpTo_ValueChanged(object? sender, EventArgs e) => ApplyFilters();

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
        int statusCol = dgvQuotations.Columns["ColStatus"]?.Index ?? -1;
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

        if (e.ColumnIndex == dgvQuotations.Columns["ColStatus"].Index)
            e.ToolTipText = quotation.Status == QuotationStatus.Approved
                ? "معتمد — جاهز للتحويل إلى فاتورة بيع"
                : "بانتظار التعميد — يُحول بعد اعتماده من المسؤول";
    }

    // ═══════════════════════════════════════════════
    // تحويل عرض سعر إلى فاتورة
    // ═══════════════════════════════════════════════
    private void dgvQuotations_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != dgvQuotations.Columns["ColConvert"].Index)
            return;

        if (dgvQuotations.Rows[e.RowIndex].DataBoundItem is MockQuotation quotation)
            TryConvertToInvoice(quotation);
    }

    private void dgvQuotations_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        // النقر المزدوج على الصف = تحويل (زر «تحويل إلى فاتورة» يعمل بالنقرة الواحدة)
        if (e.RowIndex < 0 || e.ColumnIndex == dgvQuotations.Columns["ColConvert"].Index)
            return;

        if (dgvQuotations.Rows[e.RowIndex].DataBoundItem is MockQuotation quotation)
            TryConvertToInvoice(quotation);
    }

    private void TryConvertToInvoice(MockQuotation quotation)
    {
        // 1) تحذير عند تحويل عرض غير معتمد بعد
        if (quotation.Status == QuotationStatus.PendingApproval)
        {
            string warnMsg =
                $"عرض السعر «{quotation.Number}» لم يُعتمد بعد (بانتظار التعميد)." +
                Environment.NewLine + "هل تريد المتابعة بالتحويل على الرغم من ذلك؟";

            var warn = MessageBox.Show(warnMsg, "تحويل إلى فاتورة",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (warn != DialogResult.Yes) return;
        }

        // 2) تأكيد التحويل
        string confirmMsg =
            $"هل تريد تحويل عرض السعر «{quotation.Number}» إلى فاتورة بيع للعميل «{quotation.CustomerName}»؟" +
            Environment.NewLine +
            $"عدد الأصناف: {quotation.ItemCount} — الإجمالي: {quotation.Total:N2} ريال يمني";

        var confirm = MessageBox.Show(confirmMsg, "تحويل إلى فاتورة",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        // 3) رسالة النجاح (وضع تجريبي — دون ربط بالخلفية)
        string doneMsg =
            $"تم تحويل عرض السعر «{quotation.Number}» إلى فاتورة بيع بنجاح (وضع تجريبي)." +
            Environment.NewLine +
            "سيتم تعبئة أصناف الفاتورة مع الحفاظ على الوحدات والأسعار والخصومات من المستند المصدر.";

        MessageBox.Show(doneMsg, "تحويل إلى فاتورة",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ═══════════════════════════════════════════════
    // أزرار الشريط السفلي
    // ═══════════════════════════════════════════════
    private void btnAdd_Click(object? sender, EventArgs e)
    {
        // نموذج أولي — يُفتح محرر إنشاء عرض سعر في النسخة النهائية عبر DI
        MessageBox.Show(
            "سيتم فتح نافذة إنشاء عرض سعر جديد في النسخة النهائية (وضع تجريبي).",
            "إضافة عرض سعر", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnClose_Click(object? sender, EventArgs e) => Close();

    // ═══════════════════════════════════════════════
    // اختصارات لوحة المفاتيح
    // ═══════════════════════════════════════════════
    private void frmPriceQuotation_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Escape)
        {
            btnClose.PerformClick(); // إغلاق (Cancel)
            e.Handled = true;
        }
    }
}