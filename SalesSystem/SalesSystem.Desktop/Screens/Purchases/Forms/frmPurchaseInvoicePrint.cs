using System.Drawing.Drawing2D;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Purchases.Forms;

/// <summary>
/// نافذة طباعة فاتورة مشتريات (معاينة الطباعة) — نموذج واجهة (UI Shape) يعمل ببيانات
/// وهمية في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض معاينة ورقية للفاتورة داخل بطاقة تشبه الفاتورة المطبوعة: اسم المتجر، عنوان
/// «فاتورة مشتريات»، رقم الفاتورة، التاريخ، المورد، المستودع، المجموع الفرعي، الخصم،
/// الضريبة، الإجمالي بارزاً، المدفوع، الباقي، التفقيط، ثم فاصل متقطع وتذييل
/// «شكراً لتعاملكم معنا».
/// زر الطباعة يعرض رسالة نجاح فقط (محاكاة). تُعرض النافذة عبر ShowDialog().
/// </summary>
public partial class frmPurchaseInvoicePrint : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── بيانات الفاتورة الوهمية (Mock) ─────────────────────────────────────────

    private sealed class PurchaseInvoice
    {
        public required string InvoiceNo { get; init; }     // رقم الفاتورة
        public required DateTime Date { get; init; }        // التاريخ
        public required string Supplier { get; init; }      // المورد
        public required string Warehouse { get; init; }     // المستودع
        public required decimal SubTotal { get; init; }     // المجموع الفرعي
        public required decimal Discount { get; init; }     // الخصم
        public required decimal Tax { get; init; }          // الضريبة
        public required decimal Total { get; init; }        // الإجمالي
        public required decimal Paid { get; init; }         // المدفوع
        public required decimal Remaining { get; init; }    // الباقي
        public required string Tafqit { get; init; }        // التفقيط بالحروف
    }

    // فاتورة مشتريات رقم 5003 — شركة النور للتجارة — الإجمالي 714,415 ريال يمني
    private readonly PurchaseInvoice _invoice = new()
    {
        InvoiceNo = "5003",
        Date = new DateTime(2026, 9, 18),
        Supplier = "شركة النور للتجارة",
        Warehouse = "المستودع الرئيسي (1)",
        SubTotal = 650000m,
        Discount = 8000m,
        Tax = 72415m,
        Total = 714415m,
        Paid = 500000m,
        Remaining = 214415m,
        Tafqit = "سبعمائة وأربعة عشر ألفاً وأربعمائة وخمسة عشر ريالاً يمنياً فقط",
    };

    public frmPurchaseInvoicePrint(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnPrint.PerformClick()));
        _BindInvoice();
        ApplyUiTheme();   // RULE-N50 — الثيم الأخضر آخر خطوة في تهيئة المظهر
    }

    // ── الثيم الأخضر (RULE-N50…RULE-N54 / RULE-N134 / RULE-N140…N144) ──────────

    /// <summary>
    /// يبدأ بـ <see cref="ThemeHelper"/> وينتهي بـ <see cref="GreenThemeHelper.Apply(Form)"/>
    /// **كآخر سطر** — والقيم النهائية مطابقة حرفياً لما هو معلن في <c>*.Designer.cs</c>
    /// حتى يتساوى المصمّم ووقت التشغيل.
    /// </summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        ThemeHelper.ApplyButtonStyle(btnPrint, ThemeHelper.ButtonType.Success);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Neutral);

        // يُعاد ضبط ما فرضه ThemeHelper (خط Segoe UI/حجم مرن) إلى قيم المصمّم حرفياً.
        foreach (var btn in new[] { btnPrint, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        }
        btnPrint.Size = new Size(132, 42);
        btnPrint.ForeColor = Color.White;
        btnClose.Size = new Size(100, 42);
        btnClose.ForeColor = Color.White;

        // عنوان الفاتورة والإجمالي والتفقيط: نص مميّز من لوحة الثيم فقط (RULE-N54).
        lblAccent.BackColor = GreenThemeHelper.TextStrong;
        lblInvoiceTitle.ForeColor = GreenThemeHelper.TextStrong;
        lblTotalValue.ForeColor = GreenThemeHelper.TextStrong;
        lblTafqitValue.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.ApplyHeader(lblTitle);   // RULE-N53
        GreenThemeHelper.Apply(this);             // ← الأخير دائماً (RULE-N50)
    }

    // ── ربط بيانات الفاتورة ببطاقة المعاينة ─────────────────────────────────────

    private void _BindInvoice()
    {
        lblInvoiceNoValue.Text = _invoice.InvoiceNo;
        lblDateValue.Text = _invoice.Date.ToString("dd/MM/yyyy");
        lblSupplierValue.Text = _invoice.Supplier;
        lblWarehouseValue.Text = _invoice.Warehouse;

        lblSubTotalValue.Text = _invoice.SubTotal.ToString("N2") + " ريال يمني";
        lblDiscountValue.Text = "- " + _invoice.Discount.ToString("N2") + " ريال يمني";
        lblTaxValue.Text = _invoice.Tax.ToString("N2") + " ريال يمني";
        lblTotalValue.Text = _invoice.Total.ToString("N0") + " ريال يمني";
        lblPaidValue.Text = _invoice.Paid.ToString("N2") + " ريال يمني";
        lblRemainingValue.Text = _invoice.Remaining.ToString("N2") + " ريال يمني";
        lblTafqitValue.Text = _invoice.Tafqit;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            MessageBox.Show(
                $"تمت طباعة فاتورة المشتريات رقم {_invoice.InvoiceNo} بنجاح.\n" +
                $"المورد: {_invoice.Supplier} — الإجمالي: {_invoice.Total:N0} ريال يمني\n" +
                "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)",
                "تمت الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            // RULE-N29: التفاصيل للسجل فقط، والمستخدم يرى رسالة عربية.
            Serilog.Log.Error(ex, "تعذرت طباعة فاتورة المشتريات {InvoiceNo}", _invoice.InvoiceNo);
            MessageBox.Show("تعذر طباعة فاتورة المشتريات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── الفاصل المتقطع أسفل بطاقة الفاتورة ─────────────────────────────────────

    private void pnlDashedSeparator_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(Color.FromArgb(190, 195, 200), 2) { DashStyle = DashStyle.Dash };
        int y = pnlDashedSeparator.Height / 2;
        e.Graphics.DrawLine(pen, 6, y, pnlDashedSeparator.Width - 6, y);
    }
}