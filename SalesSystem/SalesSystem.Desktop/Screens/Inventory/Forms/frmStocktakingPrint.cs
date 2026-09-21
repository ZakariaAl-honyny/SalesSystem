using System.Drawing.Drawing2D;
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

    // لون محاضر الجرد (تركواز) — يميزها عن سندات التحويل والقبض/الصرف
    private static readonly Color StocktakingColor = Color.FromArgb(13, 116, 119);

    // ألوان الفائض/العجز — مطابقة لشاشة الجرد الفعلي (frmStocktakingNew)
    private static readonly Color SurplusGreen = Color.FromArgb(25, 135, 84);
    private static readonly Color DeficitRed = Color.FromArgb(220, 53, 69);

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
    }

    // ── ربط بيانات المحضر ببطاقة المعاينة ─────────────────────────────────────

    private void _BindSession()
    {
        lblStocktakingTitle.ForeColor = StocktakingColor;
        lblAccent.BackColor = StocktakingColor;

        lblDocNoValue.Text = _session.DocNo;
        lblDocNoValue.ForeColor = StocktakingColor;
        lblDateValue.Text = _session.Date.ToString("dd/MM/yyyy");
        lblWarehouseValue.Text = _session.Warehouse;
        lblCommitteeHeadValue.Text = _session.CommitteeHead;
        lblCommitteeMemberValue.Text = _session.CommitteeMember;
        lblStorekeeperValue.Text = _session.Storekeeper;
        lblItemCountValue.Text = $"{_session.ItemCount} أصناف";
        lblSurplusValue.Text = _session.TotalSurplus.ToString("N3");
        lblDeficitValue.Text = _session.TotalDeficit.ToString("N3");
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            $"تمت طباعة محضر الجرد رقم {_session.DocNo} بنجاح.\n" +
            $"المستودع: {_session.Warehouse} — لجنة الجرد: {_session.CommitteeHead} / {_session.CommitteeMember} / {_session.Storekeeper}\n" +
            $"عدد الأصناف: {_session.ItemCount} — إجمالي الفائض: {_session.TotalSurplus:N3} — إجمالي العجز: {_session.TotalDeficit:N3}\n" +
            "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)",
            "تمت الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── الفاصل المتقطع أسفل بطاقة المحضر ───────────────────────────────────────

    private void pnlDashedSeparator_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(Color.FromArgb(190, 195, 200), 2) { DashStyle = DashStyle.Dash };
        int y = pnlDashedSeparator.Height / 2;
        e.Graphics.DrawLine(pen, 6, y, pnlDashedSeparator.Width - 6, y);
    }
}