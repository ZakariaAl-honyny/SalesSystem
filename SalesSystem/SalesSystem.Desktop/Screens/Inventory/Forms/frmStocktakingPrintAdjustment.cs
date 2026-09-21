using System.Drawing.Drawing2D;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// نافذة طباعة سند تسوية جرد (معاينة الطباعة) — نموذج واجهة (UI Shape) يعمل ببيانات
/// وهمية في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض معاينة ورقية للسند داخل بطاقة تشبه السند المطبوع: اسم المتجر، عنوان
/// «سند تسوية جرد»، رقم السند، التاريخ، المستودع، سبب التسوية (تلف / انتهاء صلاحية /
/// فائض / عجز)، عدد الأصناف، وإجمالي قيمة التسوية بارزاً، ثم فاصل متقطع
/// وتذييل توقيعات (أمين المخزن / المحاسب / مدير المخزون).
/// زر الطباعة يعرض رسالة نجاح فقط (محاكاة). تُعرض النافذة عبر ShowDialog().
/// </summary>
public partial class frmStocktakingPrintAdjustment : Form
{
    private readonly IServiceProvider _serviceProvider;

    // لون سندات تسوية الجرد (نيلي) — يميزها عن محضر الجرد (تركواز) وسند التحويل (برتقالي)
    private static readonly Color AdjustmentColor = Color.FromArgb(108, 92, 231);

    // لون سبب التسوية «تلف» (أحمر) — إشارة إلى نقص/تلف يخفض قيمة المخزون
    private static readonly Color DamageRed = Color.FromArgb(220, 53, 69);

    // ── بيانات سند التسوية الوهمية (Mock) ──────────────────────────────────────

    private sealed class AdjustmentVoucher
    {
        public required string VoucherNo { get; init; }    // رقم السند
        public required DateTime Date { get; init; }       // التاريخ
        public required string Warehouse { get; init; }    // المستودع
        public required string Reason { get; init; }       // سبب التسوية (تلف/انتهاء صلاحية/فائض/عجز)
        public required int ItemCount { get; init; }       // عدد الأصناف
        public required decimal TotalValue { get; init; }  // إجمالي قيمة التسوية (ريال يمني)
    }

    // سند تسوية رقم ADJ-2026-001 — المستودع الرئيسي — سبب: تلف — بقيمة 8,400 ريال يمني
    private readonly AdjustmentVoucher _voucher = new()
    {
        VoucherNo = "ADJ-2026-001",
        Date = new DateTime(2026, 9, 21),
        Warehouse = "المستودع الرئيسي",
        Reason = "تلف",
        ItemCount = 4,
        TotalValue = 8400m,
    };

    public frmStocktakingPrintAdjustment(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnPrint.PerformClick()));
        _BindVoucher();
    }

    // ── ربط بيانات سند التسوية ببطاقة المعاينة ─────────────────────────────────

    private void _BindVoucher()
    {
        lblAdjustmentTitle.ForeColor = AdjustmentColor;
        lblAccent.BackColor = AdjustmentColor;

        lblVoucherNoValue.Text = _voucher.VoucherNo;
        lblVoucherNoValue.ForeColor = AdjustmentColor;
        lblDateValue.Text = _voucher.Date.ToString("dd/MM/yyyy");
        lblWarehouseValue.Text = _voucher.Warehouse;
        lblReasonValue.Text = _voucher.Reason;
        lblReasonValue.ForeColor = DamageRed;
        lblItemCountValue.Text = $"{_voucher.ItemCount} أصناف";
        lblTotalValue.Text = _voucher.TotalValue.ToString("N0") + " ريال يمني";
        lblTotalValue.ForeColor = AdjustmentColor;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            $"تمت طباعة سند تسوية الجرد رقم {_voucher.VoucherNo} بنجاح.\n" +
            $"المستودع: {_voucher.Warehouse} — سبب التسوية: {_voucher.Reason}\n" +
            $"عدد الأصناف: {_voucher.ItemCount} — إجمالي قيمة التسوية: {_voucher.TotalValue:N0} ريال يمني\n" +
            "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)",
            "تمت الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── الفاصل المتقطع أسفل بطاقة السند ────────────────────────────────────────

    private void pnlDashedSeparator_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(Color.FromArgb(190, 195, 200), 2) { DashStyle = DashStyle.Dash };
        int y = pnlDashedSeparator.Height / 2;
        e.Graphics.DrawLine(pen, 6, y, pnlDashedSeparator.Width - 6, y);
    }
}