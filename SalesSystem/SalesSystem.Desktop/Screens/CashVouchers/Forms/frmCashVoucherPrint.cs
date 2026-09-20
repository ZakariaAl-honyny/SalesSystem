using System.Drawing.Drawing2D;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

/// <summary>
/// نافذة طباعة سند (معاينة الطباعة) — نموذج واجهة (UI Shape) يعمل ببيانات وهمية
/// في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض معاينة ورقية للسند داخل بطاقة تشبه السند المطبوع: اسم المتجر، نوع السند
/// (قبض/صرف)، رقم السند، التاريخ، الحساب، الجهة، المبلغ بارزاً مع التفقيط والبيان،
/// ثم فاصل متقطع وتذييل «شكراً لتعاملكم معنا».
/// زر الطباعة يعرض رسالة نجاح فقط (محاكاة). تُعرض النافذة عبر ShowDialog().
/// </summary>
public partial class frmCashVoucherPrint : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ألوان النوع: قبض (أخضر) / صرف (وردي) — نفس تدرجات باقي شاشات السندات
    private static readonly Color ReceiptColor = Color.FromArgb(46, 204, 113);
    private static readonly Color PaymentColor = Color.FromArgb(231, 76, 60);

    // ── بيانات السند الوهمية (Mock) ───────────────────────────────────────────

    private sealed class CashVoucher
    {
        public required string VoucherNo { get; init; }      // رقم السند
        public required string Type { get; init; }           // قبض / صرف
        public required DateTime Date { get; init; }         // التاريخ
        public required string Treasury { get; init; }       // حساب الخزينة (الصندوق)
        public required string Party { get; init; }          // الجهة (عميل / مورد)
        public required decimal Amount { get; init; }        // المبلغ (ريال يمني)
        public required string Tafqit { get; init; }         // التفقيط بالحروف
        public required string Notes { get; init; }          // البيان
    }

    // سند قبض رقم 1003 — أحمد محمد — الصندوق الرئيسي (111) — 150,000 ريال يمني
    private readonly CashVoucher _voucher = new()
    {
        VoucherNo = "1003",
        Type = "قبض",
        Date = new DateTime(2026, 9, 15),
        Treasury = "الصندوق الرئيسي (111)",
        Party = "أحمد محمد",
        Amount = 150000m,
        Tafqit = "مائة وخمسون ألف ريال يمني فقط",
        Notes = "سداد دفعة نقدية من العميل أحمد محمد عن فاتورة مبيعات رقم INV-2026-0142.",
    };

    public frmCashVoucherPrint(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnPrint.PerformClick()));
        _BindVoucher();
    }

    // ── ربط بيانات السند ببطاقة المعاينة ──────────────────────────────────────

    private void _BindVoucher()
    {
        bool isReceipt = _voucher.Type == "قبض";
        var typeColor = isReceipt ? ReceiptColor : PaymentColor;

        lblVoucherTitle.Text = isReceipt ? "سند قبض" : "سند صرف";
        lblVoucherTitle.ForeColor = typeColor;
        lblAccent.BackColor = typeColor;

        lblVoucherNoValue.Text = _voucher.VoucherNo;
        lblDateValue.Text = _voucher.Date.ToString("dd/MM/yyyy");
        lblAccountValue.Text = _voucher.Treasury;
        lblPartyValue.Text = _voucher.Party;

        lblAmountValue.Text = _voucher.Amount.ToString("N0") + " ريال يمني";
        lblAmountValue.ForeColor = typeColor;
        lblTafqitValue.Text = _voucher.Tafqit;
        lblTafqitValue.ForeColor = typeColor;
        lblNotesValue.Text = _voucher.Notes;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            $"تمت طباعة السند رقم {_voucher.VoucherNo} — {_voucher.Type} بنجاح.\n" +
            $"الجهة: {_voucher.Party} — المبلغ: {_voucher.Amount:N0} ريال يمني\n" +
            "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)",
            "تمت الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── الفاصل المتقطع أسفل بطاقة السند ───────────────────────────────────────

    private void pnlDashedSeparator_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(Color.FromArgb(190, 195, 200), 2) { DashStyle = DashStyle.Dash };
        int y = pnlDashedSeparator.Height / 2;
        e.Graphics.DrawLine(pen, 6, y, pnlDashedSeparator.Width - 6, y);
    }
}