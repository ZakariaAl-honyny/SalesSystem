using System.Drawing.Drawing2D;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// نافذة طباعة محضر الجرد (معاينة الطباعة) — نموذج واجهة (UI Shape) يعمل ببيانات
/// وهمية في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض معاينة ورقية لمحضر الجرد الفعلي داخل بطاقة تشبه المحضر المطبوع: اسم المتجر،
/// عنوان «محضر جرد فعلي»، رقم المحضر، التاريخ، المستودع، لجنة الجرد
/// (رئيس / عضو / أمين)، عدد الأصناف، إجمالي الفائض، إجمالي العجز، ثم فاصل متقطع
/// وتذييل توقيعات اللجنة.
/// زر الطباعة يعرض رسالة نجاح فقط (محاكاة). تُعرض النافذة عبر ShowDialog().
/// </summary>
public partial class frmStocktakingPrint : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── بيانات محضر الجرد الوهمية (Mock) ──────────────────────────────────────

    private sealed class StocktakingSession
    {
        public required string DocNo { get; init; }            // رقم المحضر
        public required DateTime Date { get; init; }           // التاريخ
        public required string Warehouse { get; init; }        // المستودع
        public required string CommitteeHead { get; init; }    // رئيس لجنة الجرد
        public required string CommitteeMember { get; init; }  // عضو اللجنة
        public required string Storekeeper { get; init; }      // أمين المخزن
        public required int ItemCount { get; init; }           // عدد الأصناف
        public required decimal TotalSurplus { get; init; }    // إجمالي الفائض
        public required decimal TotalDeficit { get; init; }    // إجمالي العجز
    }

    // محضر جرد رقم INV-2026-001 — المستودع الرئيسي — لجنة: أحمد / خالد / سارة
    private readonly StocktakingSession _session = new()
    {
        DocNo = "INV-2026-001",
        Date = new DateTime(2026, 9, 21),
        Warehouse = "المستودع الرئيسي",
        CommitteeHead = "أحمد محمد",
        CommitteeMember = "خالد عبدالله",
        Storekeeper = "سارة علي",
        ItemCount = 8,
        TotalSurplus = 12.750m,
        TotalDeficit = 5.250m,
    };

    public frmStocktakingPrint(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnPrint.PerformClick()));
        _BindSession();
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

        // عنوان البطاقة ورقم المحضر: نص مميّز + شريط تمييز من لوحة الثيم فقط (RULE-N54).
        lblAccent.BackColor = GreenThemeHelper.TextStrong;
        lblStocktakingTitle.ForeColor = GreenThemeHelper.TextStrong;
        lblDocNoValue.ForeColor = GreenThemeHelper.TextStrong;

        GreenThemeHelper.ApplyHeader(lblTitle);   // RULE-N53
        GreenThemeHelper.Apply(this);             // ← الأخير دائماً (RULE-N50)
    }

    // ── ربط بيانات المحضر ببطاقة المعاينة ─────────────────────────────────────

    private void _BindSession()
    {
        lblDocNoValue.Text = _session.DocNo;
        lblDateValue.Text = clsFormat.DateToDayMonthYear(_session.Date);
        lblWarehouseValue.Text = _session.Warehouse;
        lblCommitteeHeadValue.Text = _session.CommitteeHead;
        lblCommitteeMemberValue.Text = _session.CommitteeMember;
        lblStorekeeperValue.Text = _session.Storekeeper;
        lblItemCountValue.Text = $"{clsFormat.Number(_session.ItemCount)} أصناف";
        lblSurplusValue.Text = clsFormat.Quantity(_session.TotalSurplus);
        lblDeficitValue.Text = clsFormat.Quantity(_session.TotalDeficit);
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            clsDialogs.Success(
                $"تمت طباعة محضر الجرد رقم {_session.DocNo} بنجاح.\n" +
                $"المستودع: {_session.Warehouse} — لجنة الجرد: {_session.CommitteeHead} / {_session.CommitteeMember} / {_session.Storekeeper}\n" +
                $"عدد الأصناف: {clsFormat.Number(_session.ItemCount)} — إجمالي الفائض: {clsFormat.Quantity(_session.TotalSurplus)} — إجمالي العجز: {clsFormat.Quantity(_session.TotalDeficit)}\n" +
                "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)");
        }
        catch (Exception ex)
        {
            // RULE-N29: التفاصيل للسجل فقط، والمستخدم يرى رسالة عربية.
            Serilog.Log.Error(ex, "تعذرت طباعة محضر الجرد {DocNo}", _session.DocNo);
            clsDialogs.Error("تعذر طباعة محضر الجرد. يرجى إعادة المحاولة.");
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── الفاصل المتقطع أسفل بطاقة المحضر ───────────────────────────────────────

    private void pnlDashedSeparator_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(GreenThemeHelper.GridLine, 2) { DashStyle = DashStyle.Dash };
        int y = pnlDashedSeparator.Height / 2;
        e.Graphics.DrawLine(pen, 6, y, pnlDashedSeparator.Width - 6, y);
    }
}