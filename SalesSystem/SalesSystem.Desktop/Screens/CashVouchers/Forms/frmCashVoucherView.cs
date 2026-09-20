using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

/// <summary>
/// نافذة عرض سند قبض / صرف — نموذج واجهة (UI Shape) يعمل ببيانات وهمية في الذاكرة
/// دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// واجهة للقراءة فقط تعرض تفاصيل السند كاملة (رقم، تاريخ، نوع، طريقة دفع، حسابات،
/// مبلغ مع التفقيط، بيان) مع الحالة، وتتيح الطباعة أو إلغاء السند.
/// </summary>
public partial class frmCashVoucherView : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ألوان النوع: قبض (أخضر) / صرف (وردي) — نفس تدرجات شاشة الإضافة
    private static readonly Color ReceiptColor = Color.FromArgb(46, 204, 113);
    private static readonly Color PaymentColor = Color.FromArgb(231, 76, 60);

    // ── بيانات السند الوهمية (Mock) ───────────────────────────────────────────

    private sealed class CashVoucher
    {
        public required string VoucherNo { get; init; }      // رقم السند
        public required string Type { get; init; }           // قبض / صرف
        public required DateTime Date { get; init; }         // التاريخ
        public required string Method { get; init; }         // طريقة الدفع
        public required string Treasury { get; init; }       // حساب الخزينة
        public required string PartyType { get; init; }      // نوع الطرف
        public required string ContraAccount { get; init; }  // الحساب المقابل
        public required string CostCenter { get; init; }     // مركز التكلفة
        public required decimal Amount { get; init; }        // المبلغ (ريال يمني)
        public required string Tafqit { get; init; }         // التفقيط بالحروف
        public required string Notes { get; init; }          // البيان
        public required string Status { get; init; }         // مرحل / مسودة / ملغي
    }

    // سند قبض رقم 1003 — العميل أحمد محمد — الصندوق الرئيسي (111) — 150,000 ريال — مرحل
    private readonly CashVoucher _voucher = new()
    {
        VoucherNo = "1003",
        Type = "قبض",
        Date = new DateTime(2026, 9, 15),
        Method = "نقداً",
        Treasury = "الصندوق الرئيسي (111)",
        PartyType = "عميل",
        ContraAccount = "2102 - ذمم عملاء",
        CostCenter = "CC-01",
        Amount = 150000m,
        Tafqit = "مائة وخمسون ألف ريال يمني فقط",
        Notes = "سداد دفعة نقدية من العميل أحمد محمد عن فاتورة مبيعات رقم INV-2026-0142.",
        Status = "مرحل",
    };

    public frmCashVoucherView(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        _BindVoucher();
    }

    // ── ربط بيانات السند بالواجهة ─────────────────────────────────────────────

    private void _BindVoucher()
    {
        bool isReceipt = _voucher.Type == "قبض";
        var typeColor = isReceipt ? ReceiptColor : PaymentColor;

        // شريط العنوان: شارة النوع (قبض أخضر / صرف وردي) + شريط التمييز
        lblTypeBadge.Text = _voucher.Type;
        lblTypeBadge.BackColor = typeColor;
        lblAccent.BackColor = typeColor;
        Text = isReceipt ? "عرض سند قبض" : "عرض سند صرف";

        // الحقول الأساسية
        lblVoucherNoValue.Text = _voucher.VoucherNo;
        lblDateValue.Text = _voucher.Date.ToString("dd/MM/yyyy");
        lblTypeValue.Text = _voucher.Type;
        lblTypeValue.ForeColor = typeColor;
        lblTypeValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMethodValue.Text = _voucher.Method;
        lblTreasuryValue.Text = _voucher.Treasury;
        lblPartyTypeValue.Text = _voucher.PartyType;
        lblContraValue.Text = _voucher.ContraAccount;
        lblCostCenterValue.Text = _voucher.CostCenter;

        // المبلغ (بارز) + التفقيط + البيان
        lblAmountValue.Text = _voucher.Amount.ToString("N0") + " ريال يمني";
        lblAmountValue.ForeColor = typeColor;
        lblTafqitText.Text = _voucher.Tafqit;
        lblTafqitText.ForeColor = typeColor;
        lblNotesText.Text = _voucher.Notes;

        // الحالة: مرحل (أخضر) / مسودة (كهرماني) / ملغي (رمادي)
        var (statusColor, statusText) = _voucher.Status switch
        {
            "مرحل"  => (Color.FromArgb(25, 135, 84),   "الحالة: مرحل"),
            "مسودة" => (Color.FromArgb(240, 173, 78),  "الحالة: مسودة"),
            _       => (Color.FromArgb(108, 117, 125), "الحالة: ملغي"),
        };
        lblStatus.BackColor = statusColor;
        lblStatus.Text = statusText;

        // لا يمكن إلغاء سند ملغي
        btnVoid.Enabled = _voucher.Status != "ملغي";
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void btnPrint_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            $"سيتم طباعة السند [{_voucher.VoucherNo}] — {_voucher.Type}\n" +
            $"المبلغ: {_voucher.Amount:N0} ريال يمني\n" +
            "(وضع تجريبي — ستتم الطباعة في النسخة الكاملة)",
            "طباعة السند", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnVoid_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(
                $"هل أنت متأكد من إلغاء السند [{_voucher.VoucherNo}]؟\n" +
                "لا يمكن التراجع عن هذا الإجراء.",
                "تأكيد الإلغاء",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        MessageBox.Show(
            "تم إلغاء السند بنجاح (وضع تجريبي).",
            "تم الإلغاء", MessageBoxButtons.OK, MessageBoxIcon.Information);

        btnVoid.Enabled = false;
        lblStatus.BackColor = Color.FromArgb(108, 117, 125);
        lblStatus.Text = "الحالة: ملغي";
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();
}