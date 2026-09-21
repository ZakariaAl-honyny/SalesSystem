using System.Drawing.Drawing2D;
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

    // لون سندات التحويل (برتقالي) — يميزها عن سندات القبض/الصرف
    private static readonly Color TransferColor = Color.FromArgb(230, 126, 34);

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
    }

    // ── ربط بيانات سند التحويل ببطاقة المعاينة ─────────────────────────────────

    private void _BindTransfer()
    {
        lblTransferTitle.ForeColor = TransferColor;
        lblAccent.BackColor = TransferColor;

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
        MessageBox.Show(
            $"تمت طباعة سند التحويل رقم {_transfer.TransferNo} بنجاح.\n" +
            $"من: {_transfer.FromWarehouse} — إلى: {_transfer.ToWarehouse}\n" +
            $"السائق: {_transfer.Driver} — عدد الأصناف: {_transfer.ItemCount}\n" +
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