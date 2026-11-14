using System.Drawing.Drawing2D;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// نافذة طباعة سند تحويل مخزني (معاينة الطباعة) — نموذج واجهة (UI Shape) يعمل ببيانات
/// وهمية في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض معاينة ورقية للسند داخل بطاقة تشبه السند المطبوع: اسم المتجر، عنوان
/// «سند تحويل مخزني»، رقم السند، التاريخ، من مستودع، إلى مستودع، السائق،
/// لوحة المركبة، عدد الأصناف، وملاحظة الاستلام «يُستلم في المستودع الوجهة»،
/// ثم فاصل متقطع وتذييل توقيعات (أمين المستودع / المستلم).
/// زر الطباعة يعرض رسالة نجاح فقط (محاكاة). تُعرض النافذة عبر ShowDialog().
/// </summary>
public partial class frmWarehouseTransferPrint : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── بيانات سند التحويل الوهمية (Mock) ──────────────────────────────────────

    private sealed class WarehouseTransfer
    {
        public required string TransferNo { get; init; }    // رقم السند
        public required DateTime Date { get; init; }        // التاريخ
        public required string FromWarehouse { get; init; } // من مستودع
        public required string ToWarehouse { get; init; }   // إلى مستودع
        public required string Driver { get; init; }        // السائق
        public required string VehiclePlate { get; init; }  // لوحة المركبة
        public required int ItemCount { get; init; }        // عدد الأصناف
        public required string Note { get; init; }          // ملاحظة
    }

    // سند تحويل رقم TR-2026-001 — من المستودع الرئيسي إلى مخزن المعرض — السائق محمد علي
    private readonly WarehouseTransfer _transfer = new()
    {
        TransferNo = "TR-2026-001",
        Date = new DateTime(2026, 9, 21),
        FromWarehouse = "المستودع الرئيسي",
        ToWarehouse = "مخزن المعرض",
        Driver = "محمد علي",
        VehiclePlate = "12345 صنعاء",
        ItemCount = 8,
        Note = "يُستلم في المستودع الوجهة",
    };

    public frmWarehouseTransferPrint(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnPrint.PerformClick()));
        _BindTransfer();
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

        // عنوان السند ورقمه وملاحظة الاستلام: نص مميّز من لوحة الثيم فقط (RULE-N54).
        lblAccent.BackColor = GreenThemeHelper.TextStrong;
        lblTransferTitle.ForeColor = GreenThemeHelper.TextStrong;
        lblTransferNoValue.ForeColor = GreenThemeHelper.TextStrong;
        lblNoteValue.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.ApplyHeader(lblTitle);   // RULE-N53
        GreenThemeHelper.Apply(this);             // ← الأخير دائماً (RULE-N50)
    }

    // ── ربط بيانات سند التحويل ببطاقة المعاينة ─────────────────────────────────

    private void _BindTransfer()
    {
        lblTransferNoValue.Text = _transfer.TransferNo;
        lblDateValue.Text = _transfer.Date.ToString("dd/MM/yyyy");
        lblFromWarehouseValue.Text = _transfer.FromWarehouse;
        lblToWarehouseValue.Text = _transfer.ToWarehouse;
        lblDriverValue.Text = _transfer.Driver;
        lblVehiclePlateValue.Text = _transfer.VehiclePlate;
        lblItemCountValue.Text = $"{_transfer.ItemCount} أصناف";
        lblNoteValue.Text = _transfer.Note;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            clsDialogs.Success(
                $"تمت طباعة سند التحويل رقم {_transfer.TransferNo} بنجاح.\n" +
                $"من: {_transfer.FromWarehouse} — إلى: {_transfer.ToWarehouse}\n" +
                $"السائق: {_transfer.Driver} — عدد الأصناف: {_transfer.ItemCount}\n" +
                "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)");
        }
        catch (Exception ex)
        {
            // RULE-N29: التفاصيل للسجل فقط، والمستخدم يرى رسالة عربية.
            Serilog.Log.Error(ex, "تعذرت طباعة سند التحويل {TransferNo}", _transfer.TransferNo);
            clsDialogs.Error("تعذر طباعة سند التحويل المخزني. يرجى إعادة المحاولة.");
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