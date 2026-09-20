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

    // لون تدرج المشتريات (أزرق) — تمييز لوني لفاتورة المشتريات
    private static readonly Color PurchaseColor = Color.FromArgb(23, 87, 129);

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
    }

    // ── ربط بيانات الفاتورة ببطاقة المعاينة ─────────────────────────────────────

    private void _BindInvoice()
    {
        lblAccent.BackColor = PurchaseColor;
        lblInvoiceTitle.ForeColor = PurchaseColor;

        lblInvoiceNoValue.Text = _invoice.InvoiceNo;
        lblDateValue.Text = _invoice.Date.ToString("dd/MM/yyyy");
        lblSupplierValue.Text = _invoice.Supplier;
        lblWarehouseValue.Text = _invoice.Warehouse;

        lblSubTotalValue.Text = _invoice.SubTotal.ToString("N2") + " ريال يمني";
        lblDiscountValue.Text = "- " + _invoice.Discount.ToString("N2") + " ريال يمني";
        lblTaxValue.Text = _invoice.Tax.ToString("N2") + " ريال يمني";
        lblTotalValue.Text = _invoice.Total.ToString("N0") + " ريال يمني";
        lblTotalValue.ForeColor = PurchaseColor;
        lblPaidValue.Text = _invoice.Paid.ToString("N2") + " ريال يمني";
        lblRemainingValue.Text = _invoice.Remaining.ToString("N2") + " ريال يمني";
        lblTafqitValue.Text = _invoice.Tafqit;
        lblTafqitValue.ForeColor = PurchaseColor;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            $"تمت طباعة فاتورة المشتريات رقم {_invoice.InvoiceNo} بنجاح.\n" +
            $"المورد: {_invoice.Supplier} — الإجمالي: {_invoice.Total:N0} ريال يمني\n" +
            "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)",
            "تمت الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);
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