using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.DTO.Requests;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Screens.CashBoxes;
using SalesSystem.Desktop.Helpers;
 
namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

public partial class frmAddUpdateCashBox : Form
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ToolTip toolTip1;
    private int _editingId = -1;
    private bool _isEditMode => _editingId > 0;

    // الحساب الأب المحدد لصناديق النقد في شجرة الحسابات
    // 1101 = النقد والأصول النقدية (يُعدَّل حسب شجرة حساباتك الفعلية)
    private const string CashBoxParentAccountCode = "1101";
    private AccountDto? _parentAccount;

    public frmAddUpdateCashBox(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        this.Text = "إضافة صندوق نقدي جديد";
        lblMode.Text = "إضافة صندوق نقدي جديد";

        // تهيئة أدلة الإدخال (ToolTips)
        toolTip1 = new ToolTip();
        toolTip1.SetToolTip(txtName, "أدخل اسم الصندوق — هذا الحقل إلزامي");
        toolTip1.SetToolTip(cmbBranch, "الفرع التابع له الصندوق — اختياري، اتركه فارغاً للصندوق العام");
        toolTip1.SetToolTip(txtDescription, "ملاحظات إضافية عن الصندوق");
        toolTip1.SetToolTip(btnSave, "حفظ الصندوق وإنشاء الحساب المحاسبي تلقائياً");
        toolTip1.SetToolTip(btnClose, "إغلاق النافذة بدون حفظ");
        toolTip1.SetToolTip(rbNoInitialBalance, "الصندوق يبدأ برصيد صفر");
        toolTip1.SetToolTip(rbYesInitialBalance, "إضافة مبلغ ابتدائي للصندوق");
        toolTip1.SetToolTip(rbSourceOpening, "الرصيد مال خارج النظام (قيد افتتاحي)");
        toolTip1.SetToolTip(rbSourceTransfer, "الرصيد من صندوق آخر موجود بالنظام");
        toolTip1.SetToolTip(cmbSourceCashBox, "اختر الصندوق الذي سيُنقص منه المبلغ");

        // إخفاء لوحة التفاصيل المحاسبية حتى تُحمَّل البيانات
        pnlAccountingInfo.Visible = false;

        // ربط أحداث الـ RadioButtons
        rbYesInitialBalance.CheckedChanged += rbYesInitialBalance_CheckedChanged;
        rbNoInitialBalance.CheckedChanged += rbNoInitialBalance_CheckedChanged;
        rbSourceOpening.CheckedChanged += rbSourceOpening_CheckedChanged;
        rbSourceTransfer.CheckedChanged += rbSourceTransfer_CheckedChanged;

        // الحالة الابتدائية: لا رصيد ابتدائي
        pnlInitialBalanceDetails.Visible = false;
        pnlTransferDetails.Visible = false;
    }

    public frmAddUpdateCashBox(IServiceProvider serviceProvider, int id) : this(serviceProvider)
    {
        _editingId = id;
        this.Text = "تعديل الصندوق النقدي";
        lblMode.Text = "تعديل الصندوق النقدي";
    }

    // ══════════════════════════════════════════════════════
    //  تحميل بيانات الصندوق (وضع التعديل)
    // ══════════════════════════════════════════════════════
    private async Task LoadCashBoxAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().GetByIdAsync(_editingId);
            Cursor = Cursors.Default;

            if (result.IsSuccess && result.Value != null)
            {
                var cb = result.Value;
                txtName.Text = cb.Name;
                txtDescription.Text = cb.Description ?? "";

                // تعيين الفرع إن وُجد
                if (cb.BranchId.HasValue && cb.BranchId.Value > 0)
                    cmbBranch.SelectedValue = cb.BranchId.Value;
            }
            else
            {
                MessageBox.Show(result.Error ?? "فشل في تحميل بيانات الصندوق",
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        catch (Exception ex)
        {
            Cursor = Cursors.Default;
            MessageBox.Show($"خطأ في تحميل بيانات الصندوق: {ex.Message}",
                "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    // ══════════════════════════════════════════════════════
    //  تحميل قائمة الفروع من API
    // ══════════════════════════════════════════════════════
    private async Task LoadBranchesAsync()
    {
        try
        {
            var result = await _serviceProvider.GetRequiredService<IBranchApiService>().GetAllAsync(includeInactive: false);
            if (result.IsSuccess && result.Value != null)
            {
                var branches = result.Value.ToList();
                branches.Insert(0, new BranchDto(0, "-- اختر الفرع --", IsActive: false));
                cmbBranch.DataSource = branches;
                cmbBranch.DisplayMember = nameof(BranchDto.Name);
                cmbBranch.ValueMember = nameof(BranchDto.Id);
                cmbBranch.SelectedIndex = 0;
            }
            else
            {
                cmbBranch.Enabled = false;
            }
        }
        catch
        {
            cmbBranch.Enabled = false;
        }
    }

    // ══════════════════════════════════════════════════════
    //  تحميل قائمة الصناديق النقدية للتحويل منها
    // ══════════════════════════════════════════════════════
    private async Task LoadCashBoxesForTransferAsync()
    {
        try
        {
            var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().GetAllAsync();
            if (result.IsSuccess && result.Value != null)
            {
                var boxes = result.Value
                    .Where(b => b.IsActive)
                    .Select(b => new { b.Id, Display = $"{b.Name} ({b.AccountName ?? ""})" })
                    .ToList();
                cmbSourceCashBox.DataSource = boxes;
                cmbSourceCashBox.DisplayMember = "Display";
                cmbSourceCashBox.ValueMember = "Id";
            }
        }
        catch
        {
            // Silently fail — transfer option just won't work
        }
    }

    // ══════════════════════════════════════════════════════
    //  تحميل التفاصيل المحاسبية من API في الوقت الحقيقي
    // ══════════════════════════════════════════════════════
    private async Task LoadAccountingPreviewAsync()
    {
        // عرض حالة تحميل
        lblAccountCodeValue.Text = "⏳ جارٍ التحميل...";
        lblParentAccountValue.Text = "⏳ جارٍ التحميل...";
        lblAccountTypeValue.Text = "⏳ جارٍ التحميل...";
        lblAccountNoteValue.Text = "";
        pnlAccountingInfo.Visible = true;

        try
        {
            // جلب جميع الحسابات للبحث عن الحساب الأب
            var allResult = await _serviceProvider.GetRequiredService<IAccountApiService>().GetAllAsync(includeInactive: false);

            if (!allResult.IsSuccess || allResult.Value == null)
            {
                ShowAccountingError("تعذّر تحميل شجرة الحسابات");
                return;
            }

            var accounts = allResult.Value;

            // البحث عن الحساب الأب بالكود المحدد
            _parentAccount = accounts.FirstOrDefault(a =>
                a.AccountCode == CashBoxParentAccountCode);

            if (_parentAccount == null)
            {
                ShowAccountingError($"لم يُعثر على الحساب الأب ({CashBoxParentAccountCode}) في الشجرة");
                return;
            }

            // حساب رقم الحساب الجديد:
            // الأكواد التفصيلية (Level 4) تحت الأب = ParentCode + 4 أرقام
            var siblingCodes = accounts
                .Where(a => a.ParentId == _parentAccount.Id && a.Level == 4)
                .Select(a => a.AccountCode)
                .ToList();

            string nextCode = GenerateNextAccountCode(_parentAccount.AccountCode, siblingCodes);

            // بناء مسار الشجرة
            string treePath = BuildTreePath(accounts, _parentAccount);

            // عرض البيانات
            lblAccountCodeValue.Text = nextCode;
            lblParentAccountValue.Text = $"{_parentAccount.AccountCode} — {_parentAccount.NameAr}";
            lblAccountTypeValue.Text = $"أصول (Asset)";   // الصناديق دائماً Assets
            lblAccountNoteValue.Text = treePath + $" ← {nextCode} (جديد)";
        }
        catch (Exception ex)
        {
            ShowAccountingError($"خطأ: {ex.Message}");
        }
    }

    private void ShowAccountingError(string message)
    {
        lblAccountCodeValue.Text = "—";
        lblParentAccountValue.Text = "—";
        lblAccountTypeValue.Text = "—";
        lblAccountNoteValue.Text = $"⚠️ {message}";
        lblAccountNoteValue.ForeColor = Color.Firebrick;
    }

    // ══════════════════════════════════════════════════════
    //  توليد رقم الحساب التالي (نفس منطق السيرفر)
    // ══════════════════════════════════════════════════════
    private static string GenerateNextAccountCode(string parentCode, List<string> existingCodes)
    {
        // المستوى 4: ParentCode(4) + suffix(4) = 8 أرقام
        int suffixLength = 4;
        int maxSuffix = 0;

        foreach (var code in existingCodes)
        {
            if (code.StartsWith(parentCode) && code.Length == parentCode.Length + suffixLength)
            {
                if (int.TryParse(code[parentCode.Length..], out int suffix))
                    maxSuffix = Math.Max(maxSuffix, suffix);
            }
        }

        return parentCode + (maxSuffix + 1).ToString().PadLeft(suffixLength, '0');
    }

    // ══════════════════════════════════════════════════════
    //  بناء مسار الشجرة النصي للعرض
    // ══════════════════════════════════════════════════════
    private static string BuildTreePath(IReadOnlyList<AccountDto> accounts, AccountDto account)
    {
        var path = new List<string>();
        var current = account;

        while (current != null)
        {
            path.Insert(0, $"{current.AccountCode} ({current.NameAr})");
            current = current.ParentId.HasValue
                ? accounts.FirstOrDefault(a => a.Id == current.ParentId.Value)
                : null;
        }

        return string.Join(" ← ", path);
    }

    // ══════════════════════════════════════════════════════
    //  أحداث النموذج
    // ══════════════════════════════════════════════════════
    private async void frmCashBoxEditor_Load(object sender, EventArgs e)
    {
        try
        {
            await LoadBranchesAsync();
            await LoadCashBoxesForTransferAsync();

            if (_isEditMode)
                await LoadCashBoxAsync();
            else
                await LoadAccountingPreviewAsync();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "حدث خطأ غير متوقع أثناء تحميل النموذج");
            System.Windows.Forms.MessageBox.Show("حدث خطأ غير متوقع أثناء تحميل النموذج. يرجى إعادة المحاولة.", "خطأ",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }
    }

    // RadioButton: هل تريد رصيد ابتدائي؟
    private void rbYesInitialBalance_CheckedChanged(object sender, EventArgs e)
        => pnlInitialBalanceDetails.Visible = rbYesInitialBalance.Checked;

    private void rbNoInitialBalance_CheckedChanged(object sender, EventArgs e)
        => pnlInitialBalanceDetails.Visible = !rbNoInitialBalance.Checked;

    // RadioButton: مصدر المبلغ
    private void rbSourceOpening_CheckedChanged(object sender, EventArgs e)
        => pnlTransferDetails.Visible = false;

    private void rbSourceTransfer_CheckedChanged(object sender, EventArgs e)
    {
        pnlTransferDetails.Visible = rbSourceTransfer.Checked;
        if (rbSourceTransfer.Checked)
            UpdateTransferWarning();
    }

    private void UpdateTransferWarning()
    {
        if (cmbSourceCashBox.SelectedItem == null) return;
        // سيُحدَّث النص ديناميكياً عند اختيار الصندوق المصدر
        lblTransferWarning.Text = "⚠️ سيُنقص المبلغ المدخل من رصيد الصندوق المصدر";
        lblTransferWarning.Visible = true;
    }

    // ══════════════════════════════════════════════════════
    //  التحقق من صحة البيانات
    // ══════════════════════════════════════════════════════
    private bool _ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            errorProvider.SetError(txtName, "اسم الصندوق مطلوب.");
            isValid = false;
        }
        else
        {
            errorProvider.SetError(txtName, "");
        }

        if (rbYesInitialBalance.Checked)
        {
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                errorProvider.SetError(txtAmount, "المبلغ الابتدائي يجب أن يكون أكبر من صفر.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtAmount, "");
            }

            if (rbSourceTransfer.Checked && cmbSourceCashBox.SelectedItem == null)
            {
                errorProvider.SetError(cmbSourceCashBox, "يرجى اختيار الصندوق المصدر للتحويل.");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(cmbSourceCashBox, "");
            }
        }

        if (!isValid)
        {
            MessageBox.Show("يرجى تصحيح الأخطاء الموضحة قبل المتابعة.", "بيانات غير مكتملة",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return isValid;
    }

    // ══════════════════════════════════════════════════════
    //  حفظ
    // ══════════════════════════════════════════════════════
    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (!_ValidateForm()) return;

        Cursor = Cursors.WaitCursor;
        btnSave.Enabled = false;

        try
        {
            if (_isEditMode)
            {
                var request = new UpdateCashBoxRequest(
                    txtName.Text.Trim(),
                    string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim());

                var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().UpdateAsync(_editingId, request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم تحديث الصندوق بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في تحديث الصندوق",
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // الحصول على رقم الفرع المختار (اختياري)
                short? branchId = null;
                if (cmbBranch.SelectedValue is short selectedId && selectedId > 0)
                    branchId = selectedId;

                // تحديد الرصيد الابتدائي
                decimal openingBalance = 0;
                int? sourceCashBoxId = null;

                if (rbYesInitialBalance.Checked && decimal.TryParse(txtAmount.Text, out decimal amount) && amount > 0)
                {
                    openingBalance = amount;
                    if (rbSourceTransfer.Checked && cmbSourceCashBox.SelectedValue is int srcId && srcId > 0)
                        sourceCashBoxId = srcId;
                }

                var request = new CreateCashBoxRequest(
                    txtName.Text.Trim(),
                    null,   // AccountId — يُنشأ تلقائياً من السيرفر
                    string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                    branchId,
                    openingBalance,
                    sourceCashBoxId);

                var result = await _serviceProvider.GetRequiredService<ICashBoxApiService>().CreateAsync(request);
                if (result.IsSuccess)
                {
                    MessageBox.Show("تم إضافة الصندوق بنجاح.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Error ?? "فشل في إضافة الصندوق",
                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        finally
        {
            btnSave.Enabled = true;
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Sets the editor to Update mode for the given cash box ID.
    /// Called after DI resolution (parameterless constructor).
    /// Data loading happens in Form_Load.
    /// </summary>
    public void _SetUpdateMode(int id)
    {
        _editingId = id;
        this.Text = "تعديل الصندوق النقدي";
        lblMode.Text = "تعديل الصندوق النقدي";
    }

    private void btnClose_Click(object sender, EventArgs e) => Close();
}