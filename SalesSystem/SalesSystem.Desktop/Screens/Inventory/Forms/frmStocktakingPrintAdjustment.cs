using System.Drawing.Drawing2D;
using SalesSystem.Desktop.GlobalClasses;
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

        // عنوان البطاقة ورقم السند وإجمالي التسوية: نص مميّز من لوحة الثيم فقط (RULE-N54).
        lblAccent.BackColor = GreenThemeHelper.TextStrong;
        lblAdjustmentTitle.ForeColor = GreenThemeHelper.TextStrong;
        lblVoucherNoValue.ForeColor = GreenThemeHelper.TextStrong;
        lblTotalValue.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.ApplyHeader(lblTitle);   // RULE-N53
        GreenThemeHelper.Apply(this);             // ← الأخير دائماً (RULE-N50)
    }

    // ── ربط بيانات سند التسوية ببطاقة المعاينة ─────────────────────────────────

    private void _BindVoucher()
    {
        lblVoucherNoValue.Text = _voucher.VoucherNo;
        lblDateValue.Text = _voucher.Date.ToString("dd/MM/yyyy");
        lblWarehouseValue.Text = _voucher.Warehouse;
        lblReasonValue.Text = _voucher.Reason;
        lblItemCountValue.Text = $"{_voucher.ItemCount} أصناف";
        lblTotalValue.Text = _voucher.TotalValue.ToString("N0") + " ريال يمني";
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            clsDialogs.Success(
                $"تمت طباعة سند تسوية الجرد رقم {_voucher.VoucherNo} بنجاح.\n" +
                $"المستودع: {_voucher.Warehouse} — سبب التسوية: {_voucher.Reason}\n" +
                $"عدد الأصناف: {_voucher.ItemCount} — إجمالي قيمة التسوية: {_voucher.TotalValue:N0} ريال يمني\n" +
                "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)");
        }
        catch (Exception ex)
        {
            // RULE-N29: التفاصيل للسجل فقط، والمستخدم يرى رسالة عربية.
            Serilog.Log.Error(ex, "تعذرت طباعة سند تسوية الجرد {VoucherNo}", _voucher.VoucherNo);
            clsDialogs.Error("تعذر طباعة سند تسوية الجرد. يرجى إعادة المحاولة.");
        }
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