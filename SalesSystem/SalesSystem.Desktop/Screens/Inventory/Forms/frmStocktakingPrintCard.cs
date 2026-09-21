using System.Drawing.Drawing2D;
using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Inventory.Forms;

/// <summary>
/// نافذة طباعة كرت صنف (معاينة الطباعة) — نموذج واجهة (UI Shape) يعمل ببيانات
/// وهمية في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// تعرض معاينة ورقية لكرت حركة صنف داخل بطاقة تشبه الكرت المطبوع: اسم المتجر،
/// عنوان «كرت حركة صنف»، بيانات الصنف (الصنف / الباركود / المستودع / الرصيد الحالي)،
/// جدول حركات الصنف (التاريخ / النوع / الوارد / الصادر / الرصيد) برصيد جارٍ،
/// ثم فاصل متقطع وتذييل توقيعات (أمين المستودع / مدقق المخزون).
/// زر الطباعة يعرض رسالة نجاح فقط (محاكاة). تُعرض النافذة عبر ShowDialog().
/// </summary>
public partial class frmStocktakingPrintCard : Form
{
    private readonly IServiceProvider _serviceProvider;

    // لون كروت الأصناف (أزرق) — يميزها عن محاضر الجرد (تركواز) وسندات التحويل (برتقالي)
    private static readonly Color CardColor = Color.FromArgb(41, 128, 185);
    private static readonly Color BalanceGreen = Color.FromArgb(25, 135, 84);

    // ── بيانات كرت الصنف الوهمية (Mock) ──────────────────────────────────────

    private sealed class ProductCard
    {
        public required string ProductName { get; init; }     // اسم الصنف
        public required string Barcode { get; init; }         // الباركود
        public required string Warehouse { get; init; }       // المستودع
        public required decimal CurrentBalance { get; init; } // الرصيد الحالي (decimal 18,3)
    }

    private sealed class Movement
    {
        public required DateTime Date { get; init; }      // التاريخ
        public required string Type { get; init; }        // النوع (وارد/صادر)
        public required decimal InQty { get; init; }      // الوارد
        public required decimal OutQty { get; init; }     // الصادر
        public required decimal Balance { get; init; }    // الرصيد الجاري
    }

    // كرت صنف «زيت نباتي 1 لتر» — المستودع الرئيسي — الرصيد الحالي 157.500
    private readonly ProductCard _card = new()
    {
        ProductName = "زيت نباتي 1 لتر",
        Barcode = "6281001234567",
        Warehouse = "المستودع الرئيسي",
        CurrentBalance = 157.500m,
    };

    // 5 حركات (وارد/صادر) برصيد جارٍ — آخر رصيد يطابق الرصيد الحالي في الكرت
    private readonly List<Movement> _movements = new()
    {
        new() { Date = new DateTime(2026, 9, 1), Type = "رصيد افتتاحي", InQty = 100.000m, OutQty = 0m, Balance = 100.000m },
        new() { Date = new DateTime(2026, 9, 5), Type = "مشتريات (وارد)", InQty = 80.000m, OutQty = 0m, Balance = 180.000m },
        new() { Date = new DateTime(2026, 9, 10), Type = "مبيعات (صادر)", InQty = 0m, OutQty = 25.000m, Balance = 155.000m },
        new() { Date = new DateTime(2026, 9, 15), Type = "مردودات مبيعات (وارد)", InQty = 12.500m, OutQty = 0m, Balance = 167.500m },
        new() { Date = new DateTime(2026, 9, 18), Type = "مبيعات (صادر)", InQty = 0m, OutQty = 10.000m, Balance = 157.500m },
    };

    public frmStocktakingPrintCard(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnPrint.PerformClick()));
        _BindCard();
        _BindMovements();
    }

    // ── ربط بيانات الكرت ببطاقة المعاينة ─────────────────────────────────────

    private void _BindCard()
    {
        lblCardTitle.ForeColor = CardColor;
        lblAccent.BackColor = CardColor;

        lblProductNameValue.Text = _card.ProductName;
        lblBarcodeValue.Text = _card.Barcode;
        lblWarehouseValue.Text = _card.Warehouse;
        lblBalanceValue.Text = _card.CurrentBalance.ToString("N3");
    }

    private void _BindMovements()
    {
        dgvMovements.DataSource = null;
        dgvMovements.DataSource = _movements;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            $"تمت طباعة كرت صنف «{_card.ProductName}» بنجاح.\n" +
            $"الباركود: {_card.Barcode} — المستودع: {_card.Warehouse}\n" +
            $"الرصيد الحالي: {_card.CurrentBalance:N3} — عدد الحركات: {_movements.Count}\n" +
            "(وضع تجريبي — تمت معاينة الطباعة دون اتصال بالطابعة)",
            "تمت الطباعة", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    // ── الفاصل المتقطع أسفل بطاقة الكرت ───────────────────────────────────────

    private void pnlDashedSeparator_Paint(object? sender, PaintEventArgs e)
    {
        using var pen = new Pen(Color.FromArgb(190, 195, 200), 2) { DashStyle = DashStyle.Dash };
        int y = pnlDashedSeparator.Height / 2;
        e.Graphics.DrawLine(pen, 6, y, pnlDashedSeparator.Width - 6, y);
    }
}