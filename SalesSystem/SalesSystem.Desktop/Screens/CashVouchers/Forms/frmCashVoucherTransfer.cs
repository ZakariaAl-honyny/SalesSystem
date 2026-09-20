using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

/// <summary>
/// نافذة تحويل بين الخزائن (سند تحويل) — نموذج واجهة (UI Shape) يعمل ببيانات وهمية
/// في الذاكرة دون أي اتصال بالخادم (وضع التجربة / UI-First).
/// قيد مزدوج متوازن: مدين إلى الحساب الوجهة + دائن من الحساب المصدر.
/// </summary>
public partial class frmCashVoucherTransfer : Form
{
    private readonly IServiceProvider _serviceProvider;

    // ── بيانات وهمية في الذاكرة (Mock) ────────────────────────────────────────

    private sealed class TreasuryAccount
    {
        public required string Code { get; init; }
        public required string Name { get; init; }
        public required decimal Balance { get; init; }

        public override string ToString() => $"{Name} {Code} (رصيد: {Balance:N0})";
    }

    private static readonly TreasuryAccount[] TreasuryAccounts =
    {
        new() { Code = "111", Name = "الصندوق الرئيسي", Balance = 250000m },
        new() { Code = "112", Name = "البنك",           Balance = 800000m },
        new() { Code = "113", Name = "المحفظة",         Balance = 95000m },
    };

    // ألوان القيد المزدوج: طبيعي (أزرق) / تحذير (أحمر)
    private static readonly Color NoteNormalBack = Color.FromArgb(235, 245, 252);
    private static readonly Color NoteNormalFore = Color.FromArgb(41, 128, 185);
    private static readonly Color NoteWarningBack = Color.FromArgb(253, 240, 238);
    private static readonly Color NoteWarningFore = Color.FromArgb(192, 57, 43);

    public frmCashVoucherTransfer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        KeyboardHelper.WireGlobalShortcuts(this, (Keys.F10, () => btnSave.PerformClick()));
        _LoadMockLookups();
    }

    // ── تحميل القوائم الوهمية ─────────────────────────────────────────────────

    private void _LoadMockLookups()
    {
        cmbFrom.Items.Clear();
        cmbTo.Items.Clear();

        foreach (var acc in TreasuryAccounts)
        {
            cmbFrom.Items.Add(acc.ToString());
            cmbTo.Items.Add(acc.ToString());
        }

        dtpDate.Value = DateTime.Today;
        cmbFrom.SelectedIndex = 0;   // الصندوق الرئيسي 111
        cmbTo.SelectedIndex = 1;     // البنك 112 — مختلف افتراضياً
        numAmount.Value = 0;

        _UpdateJournalNote();
    }

    private TreasuryAccount? _GetSelectedAccount(ComboBox combo)
    {
        if (combo.SelectedIndex < 0 || combo.SelectedIndex >= TreasuryAccounts.Length)
            return null;

        return TreasuryAccounts[combo.SelectedIndex];
    }

    // ── قيد مزدوج متوازن (يتحدّث مباشرة عند تغيير الحسابات) ───────────────────

    private void _UpdateJournalNote()
    {
        var from = _GetSelectedAccount(cmbFrom);
        var to = _GetSelectedAccount(cmbTo);

        if (from == null || to == null)
        {
            pnlNote.BackColor = NoteNormalBack;
            lblNote.ForeColor = NoteNormalFore;
            lblNote.Text = "قيد مزدوج متوازن: مدين إلى حساب + دائن من حساب";
            return;
        }

        if (from.Code == to.Code)
        {
            pnlNote.BackColor = NoteWarningBack;
            lblNote.ForeColor = NoteWarningFore;
            lblNote.Text = "تنبيه: من وإلى الحساب نفسه — التحويل غير صالح";
            return;
        }

        pnlNote.BackColor = NoteNormalBack;
        lblNote.ForeColor = NoteNormalFore;
        lblNote.Text =
            $"قيد مزدوج متوازن: مدين (إلى) {to.Name} {to.Code} + دائن (من) {from.Name} {from.Code}";
    }

    // ── التحقق من صحة البيانات ────────────────────────────────────────────────

    private bool _ValidateForm()
    {
        var from = _GetSelectedAccount(cmbFrom);
        var to = _GetSelectedAccount(cmbTo);

        if (from == null || to == null)
        {
            MessageBox.Show("يرجى اختيار حساب المصدر وحساب الوجهة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (from.Code == to.Code)
        {
            errorProvider.SetError(cmbTo, "لا يمكن التحويل من وإلى نفس الحساب.");
            MessageBox.Show("لا يمكن التحويل من وإلى نفس الحساب.\nيرجى اختيار حسابين مختلفين.",
                "تحويل غير صالح", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        errorProvider.SetError(cmbTo, "");

        if (numAmount.Value <= 0)
        {
            errorProvider.SetError(numAmount, "المبلغ يجب أن يكون أكبر من صفر.");
            MessageBox.Show("المبلغ يجب أن يكون أكبر من صفر.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (numAmount.Value > from.Balance)
        {
            errorProvider.SetError(numAmount, $"المبلغ يتجاوز رصيد الحساب المصدر ({from.Balance:N0}).");
            MessageBox.Show(
                $"المبلغ ({numAmount.Value:N2}) يتجاوز رصيد الحساب المصدر.\n" +
                $"رصيد {from.Name} {from.Code}: {from.Balance:N0}",
                "رصيد غير كافٍ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        errorProvider.SetError(numAmount, "");
        return true;
    }

    // ── أحداث الواجهة ─────────────────────────────────────────────────────────

    private void cmbFrom_SelectedIndexChanged(object sender, EventArgs e)
        => _UpdateJournalNote();

    private void cmbTo_SelectedIndexChanged(object sender, EventArgs e)
        => _UpdateJournalNote();

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        var from = _GetSelectedAccount(cmbFrom)!;
        var to = _GetSelectedAccount(cmbTo)!;

        MessageBox.Show(
            $"تم حفظ سند التحويل وترحيله بنجاح (وضع تجريبي).\n" +
            $"من: {from.Name} {from.Code} ← إلى: {to.Name} {to.Code}\n" +
            $"المبلغ: {numAmount.Value:N2} ريال يمني",
            "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
        => Close();
}