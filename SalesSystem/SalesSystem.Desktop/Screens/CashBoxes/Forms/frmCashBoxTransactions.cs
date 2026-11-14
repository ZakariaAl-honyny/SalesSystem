using System.ComponentModel;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.CustomerReceipts.Forms;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Screens.Expenses.Forms;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.DTO.Responses;

namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

/// <summary>
/// شاشة حركات الصندوق (قائمة) — WinForms كلاسيكي، ومنطق الشاشة في المعالجات (RULE-N109).
/// البنية (§2.34): العنوان ← أوامر الشاشة ← بحث وتصفية ← الملخص ← الشبكة ← شريط الأسفل.
/// التصفية كاملة داخل <c>gbFilter</c> وفورية (RULE-N56…N63)، والبحث النصي عبر
/// <see cref="RowFilterHelper"/> على أعمدة معروفة فقط (RULE-N24/N60).
/// </summary>
public partial class frmCashBoxTransactions : Form
{
    /// <summary>نوع المستند — يحدد شاشة الفتح ولا يُقارن بنص عربي (RULE-N60).</summary>
    private const string DocKindReceipt = "Receipt";
    private const string DocKindExpense = "Expense";

    /// <summary>اتجاه الحركة: 1 = وارد إلى الصندوق · -1 = صادر من الصندوق.</summary>
    private const int DirectionIn = 1;
    private const int DirectionOut = -1;

    private const string MovementTypeIn = "وارد";
    private const string MovementTypeOut = "صادر";

    private readonly IServiceProvider _serviceProvider;

    /// <summary>جدول الحركات الموحّد (سندات القبض واردًا + المصروفات صادرًا) — مصدر التصفية والملخص.</summary>
    private DataTable? _movements;

    /// <summary>يمنع التصفية الفورية أثناء التهيئة الأولية (RULE-N59).</summary>
    private bool _isInitializing = true;

    private List<CashBoxDto> _cashBoxes = new();

    /// <summary>
    /// النطاق الزمني الافتراضي (بداية الشهر الحالي ← اليوم) — يُلتقط عند الإنشاء ليعيده
    /// «مسح التصفية» بلا إعادة تحميل، ولتُقارن به حالة تفعيل الزر (RULE-N148).
    /// </summary>
    private readonly DateTime _defaultFrom;
    private readonly DateTime _defaultTo;

    /// <summary>RULE-N04: الخدمات عبر خصائص مطبوعة النوع.</summary>
    private ICashBoxDesktopService CashBoxService => _serviceProvider.GetRequiredService<ICashBoxDesktopService>();
    private ICustomerReceiptDesktopService ReceiptService => _serviceProvider.GetRequiredService<ICustomerReceiptDesktopService>();
    private IExpenseDesktopService ExpenseService => _serviceProvider.GetRequiredService<IExpenseDesktopService>();

    public frmCashBoxTransactions(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        // RULE-N148: النطاق الافتراضي (بداية الشهر الحالي ← اليوم) يُلتقط عند الإنشاء،
        // ليعيده «مسح التصفية» بلا إعادة تحميل ولتُقارن به حالة تفعيل الزر.
        var today = DateTime.Today;
        _defaultFrom = new DateTime(today.Year, today.Month, 1);
        _defaultTo = today;

        // RULE-N95 — الستة من KeyboardHelper معاً في المُنشئ.
        KeyboardHelper.WireEnterAsTab(this);                                        // RULE-N40
        KeyboardHelper.WireSelectAllOnFocus(this);                                  // RULE-N41
        KeyboardHelper.WireFocusHighlight(this, Color.FromArgb(0xD6, 0xE4, 0xF0));  // RULE-N46
        KeyboardHelper.WireArrowNavigation(this);                                   // RULE-N45
        KeyboardHelper.WireButtonDisabledVisual(btnRefresh, btnClose, btnClearFilters); // RULE-N43
        KeyboardHelper.WireGlobalShortcuts(this,                                    // RULE-N47
            (Keys.F5, () => btnRefresh.PerformClick()),
            (Keys.Escape, _GlobalEscape));

        _SetupToolTips();
        _SetupGridColumns();
        _ClearFilterInputs();
        _UpdateClearFiltersState();

        // RULE-N147 — Enter = الزر الأساسي: شاشة قوائم/مستندات بلا آلة حالة إدخال (RULE-N101) وبلا
        // زر «جديد» ⇒ الأساسي «تحديث». Enter على الشبكة يبقى «فتح المستند» (سلوك قائم — §2.21 بند 20).
        KeyboardHelper.WireModeEnter(this, () => dgvTransactions.ContainsFocus ? null : btnRefresh);
        AcceptButton = btnRefresh;

        // RULE-N63 — يتبنّى زر btnToggleFilters المُعلَن في المصمّم ويوصّل F9 والحالة المحفوظة.
        FilterPanelHelper.Attach(this, _serviceProvider);
    }

    // ── الثيم والتلميحات ────────────────────────────────────────────────────────

    /// <summary>
    /// الثيم الأخضر (RULE-N50…RULE-N53): يبدأ بـ ThemeHelper وينتهي بـ GreenThemeHelper.Apply
    /// **كآخر سطر**، والقيم النهائية مطابقة حرفياً لِما في *.Designer.cs (RULE-N134/N140…N144).
    /// </summary>
    private void ApplyUiTheme()
    {
        ThemeHelper.ApplyMainFormStyle(this);
        ThemeHelper.ApplyButtonStyle(btnRefresh, ThemeHelper.ButtonType.Neutral);
        ThemeHelper.ApplyButtonStyle(btnClose, ThemeHelper.ButtonType.Danger);

        // يُعاد ضبط ما فرضه ThemeHelper (AutoSize/خط Segoe UI) إلى قيم المصمّم.
        foreach (var btn in new[] { btnRefresh, btnClose })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btn.Size = new Size(140, 34);
        }

        // أزرار النطاقات السريعة داخل gbFilter — نمط ثانوي صغير (RULE-N57).
        ThemeHelper.ApplyButtonStyle(btnRangeToday, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRangeMonth, ThemeHelper.ButtonType.Secondary);
        ThemeHelper.ApplyButtonStyle(btnRange30, ThemeHelper.ButtonType.Secondary);

        foreach (var btn in new[] { btnRangeToday, btnRangeMonth, btnRange30 })
        {
            btn.AutoSize = false;
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Popup;
            btn.MinimumSize = Size.Empty;
            btn.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btn.Height = 34;
        }

        btnRangeToday.Width = 96;
        btnRangeMonth.Width = 122;
        btnRange30.Width = 124;

        GreenThemeHelper.ApplyHeader(lblMode);                  // RULE-N53
        GreenThemeHelper.ApplyGrid(dgvTransactions);            // RULE-N51/N52
        lblSummary.ForeColor = GreenThemeHelper.TextStrong;
        lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;

        // RULE-N22 — شبكة عرض: السلوك تعلنه الشاشة نفسها (ApplyGrid لا يلمسه).
        dgvTransactions.ReadOnly = true;
        dgvTransactions.AllowUserToAddRows = false;
        dgvTransactions.AllowUserToDeleteRows = false;
        dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTransactions.MultiSelect = false;

        GreenThemeHelper.Apply(this);                           // ← الأخير دائماً (RULE-N50)
    }

    /// <summary>تلميحات الأزرار وأدوات التصفية مع اختصاراتها (RULE-N47).</summary>
    private void _SetupToolTips()
    {
        toolTip1.SetToolTip(btnRefresh, "تحديث حركات الصندوق (F5)");
        toolTip1.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        toolTip1.SetToolTip(cmbCashBox, "اختر الصندوق لعرض حركاته");
        toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
        toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
        toolTip1.SetToolTip(cbStatus, "تصفية بالحالة: مسودة / مرحّل / ملغى");
        toolTip1.SetToolTip(dtpFrom, "بداية النطاق الزمني");
        toolTip1.SetToolTip(dtpTo, "نهاية النطاق الزمني");
        toolTip1.SetToolTip(btnRangeToday, "حركات اليوم فقط");
        toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
        toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
        toolTip1.SetToolTip(chkOnlyCancelled, "إظهار الحركات الملغاة فقط");
        toolTip1.SetToolTip(chkShowInactive, "إظهار الحركات غير النشطة أيضاً");
        toolTip1.SetToolTip(dgvTransactions, "Enter يفتح المستند · زر الفأرة الأيمن قائمة الإجراءات");
    }

    // ── الشبكة ─────────────────────────────────────────────────────────────────

    private void _SetupGridColumns()
    {
        dgvTransactions.AutoGenerateColumns = false;
        dgvTransactions.Columns.Clear();

        _AddColumn("DocNo", "رقم المستند", 100, 8);
        _AddColumn("Type", "النوع", 80, 6);
        _AddColumn("Amount", "المبلغ", 100, 9, "N2");
        _AddColumn("RunningBalance", "الرصيد الجاري", 110, 9, "N2");
        _AddColumn("Description", "البيان", 200, 20);
        _AddColumn("StatusName", "الحالة", 90, 7);
        _AddColumn("MovementDate", "التاريخ", 110, 9, "yyyy-MM-dd");
    }

    private void _AddColumn(string property, string header, int minWidth, int fillWeight, string? format = null)
    {
        var column = new DataGridViewTextBoxColumn
        {
            Name = property,
            HeaderText = header,
            DataPropertyName = property,
            MinimumWidth = minWidth,
            FillWeight = fillWeight,
            ReadOnly = true,
        };

        if (format is not null)
            column.DefaultCellStyle.Format = format;

        dgvTransactions.Columns.Add(column);
    }

    // ── التحميل ────────────────────────────────────────────────────────────────

    private async void frmCashBoxTransactions_Load(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            ApplyUiTheme();   // RULE-N50 — الأخير في مسار التحميل
            await _LoadCashBoxesAsync();
            _UpdateClearFiltersState();   // RULE-N148 — حالة زر «مسح التصفية» بعد التحميل
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل شاشة حركات الصندوق");
            MessageBox.Show("تعذر تحميل الشاشة. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private async Task _LoadCashBoxesAsync()
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            // الصندوق المعروض يُحفظ قبل إعادة بناء القائمة ليعود بعد التحديث (RULE-N148).
            int previousCashBoxId = cmbCashBox.SelectedValue is int previous ? previous : 0;

            var result = await CashBoxService.GetAllAsync();

            if (result.IsSuccess && result.Value != null)
            {
                _cashBoxes = result.Value.Where(b => b.IsActive).ToList();

                _isInitializing = true;
                try
                {
                    cmbCashBox.DataSource = _cashBoxes.Select(b => new { b.Id, b.Name }).ToList();
                    cmbCashBox.DisplayMember = "Name";
                    cmbCashBox.ValueMember = "Id";

                    int index = 0;
                    if (previousCashBoxId > 0)
                    {
                        int found = _cashBoxes.FindIndex(b => b.Id == previousCashBoxId);
                        if (found >= 0) index = found;
                    }

                    cmbCashBox.SelectedIndex = _cashBoxes.Count > 0 ? index : -1;
                }
                finally
                {
                    _isInitializing = false;
                }

                if (cmbCashBox.SelectedValue is int cashBoxId && cashBoxId > 0)
                    await _LoadMovementsAsync(cashBoxId);
                else
                    _ShowEmptyState();
            }
            else
            {
                _ShowEmptyState();
                MessageBox.Show(result.Error ?? "فشل في تحميل قائمة الصناديق.", "خطأ في تحميل البيانات",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحميل صناديق شاشة الحركات");
            MessageBox.Show("تعذر تحميل قائمة الصناديق. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// يبني حركات الصندوق للعرض فقط من مستندات موجودة فعلاً: سندات القبض (وارد) والمصروفات (صادر).
    /// لا يوجد أي ترحيل أو حساب مالي هنا — العرض والتصفية فقط، والخدمات هي المصدر (RULE-N05).
    /// </summary>
    private async Task _LoadMovementsAsync(int cashBoxId)
    {
        Cursor = Cursors.WaitCursor;
        btnRefresh.Enabled = false;

        try
        {
            var table = CreateMovementsTable();

            var receiptsResult = await ReceiptService.GetAllAsync(includeInactive: true);
            if (receiptsResult.IsSuccess && receiptsResult.Value != null)
            {
                foreach (var receipt in receiptsResult.Value.Where(r => r.CashBoxId == cashBoxId))
                {
                    table.Rows.Add(
                        receipt.Id,
                        DocKindReceipt,
                        receipt.ReceiptNo.ToString(),
                        MovementTypeIn,
                        DirectionIn,
                        receipt.Amount,
                        0m,
                        string.IsNullOrWhiteSpace(receipt.Notes) ? receipt.CustomerName ?? string.Empty : receipt.Notes,
                        (int)receipt.Status,
                        receipt.StatusName ?? string.Empty,
                        receipt.ReceiptDate,
                        receipt.IsActive,
                        receipt.Status == 3);
                }
            }
            else
            {
                MessageBox.Show(receiptsResult.Error ?? "فشل في تحميل سندات القبض.", "خطأ في تحميل البيانات",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var expensesResult = await ExpenseService.GetAllAsync();
            if (expensesResult.IsSuccess && expensesResult.Value != null)
            {
                foreach (var expense in expensesResult.Value.Where(x => x.CashBoxId == cashBoxId))
                {
                    table.Rows.Add(
                        expense.Id,
                        DocKindExpense,
                        expense.ExpenseNo.ToString(),
                        MovementTypeOut,
                        DirectionOut,
                        expense.Amount,
                        0m,
                        string.IsNullOrWhiteSpace(expense.Notes) ? expense.ExpenseAccountName ?? string.Empty : expense.Notes,
                        (int)expense.Status,
                        expense.StatusName ?? string.Empty,
                        expense.ExpenseDate,
                        expense.IsActive,
                        expense.Status == 3);
                }
            }
            else
            {
                MessageBox.Show(expensesResult.Error ?? "فشل في تحميل المصروفات.", "خطأ في تحميل البيانات",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _movements = table;
            _ApplyFilter();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر بناء حركات الصندوق {CashBoxId}", cashBoxId);
            MessageBox.Show("تعذر تحميل حركات الصندوق. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                btnRefresh.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }

    /// <summary>جدول الحركات الموحّد — أعمدة ثابتة معروفة (RULE-N60).</summary>
    private static DataTable CreateMovementsTable()
    {
        var table = new DataTable();
        table.Columns.Add("DocId", typeof(int));
        table.Columns.Add("DocKind", typeof(string));
        table.Columns.Add("DocNo", typeof(string));
        table.Columns.Add("Type", typeof(string));
        table.Columns.Add("Direction", typeof(int));
        table.Columns.Add("Amount", typeof(decimal));
        table.Columns.Add("RunningBalance", typeof(decimal));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("Status", typeof(int));
        table.Columns.Add("StatusName", typeof(string));
        table.Columns.Add("MovementDate", typeof(DateTime));
        table.Columns.Add("IsActive", typeof(bool));
        table.Columns.Add("IsCancelled", typeof(bool));
        return table;
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى الافتراضي (RULE-N148):
    /// أدوات التصفية ← قائمة الصناديق ← الحركات ← الملخص والتحديد. ليس مسحاً للتصفية وحدها.
    /// </summary>
    private async Task _RefreshAsync()
    {
        if (IsDisposed) return;

        _ClearFilterInputs();          // التصفية تعود للافتراضي
        await _LoadCashBoxesAsync();   // قائمة الصناديق + الحركات (ويعيد التصفية والملخص من البيانات المُعاد تحميلها)
        _UpdateClearFiltersState();
    }

    // ── التصفية الفورية (RULE-N56…N61) ─────────────────────────────────────────

    /// <summary>
    /// يبني تعبير التصفية: النطاق الزمني + الحالة + الملغاة فقط + إخفاء غير النشط + نص البحث.
    /// اسم العمود ثابت، والقيمة النصية تُهرَّب عبر <see cref="RowFilterHelper.Like"/> (RULE-N24/N60).
    /// </summary>
    private string _BuildFilterExpression()
    {
        var conditions = new List<string>();

        DateTime from = dtpFrom.Value.Date;
        DateTime to = dtpTo.Value.Date;
        if (from > to) (from, to) = (to, from);

        conditions.Add($"MovementDate >= #{from:MM/dd/yyyy}#");
        conditions.Add($"MovementDate < #{to.AddDays(1):MM/dd/yyyy}#");

        if (cbStatus.SelectedIndex > 0)
            conditions.Add($"Status = {cbStatus.SelectedIndex}");

        if (chkOnlyCancelled.Checked)
            conditions.Add("IsCancelled = true");

        // الحركات الملغاة تُستثنى عادةً من «النشط»: عند طلب الملغاة لا نُسقطها بشرط النشاط.
        bool requestingCancelled = chkOnlyCancelled.Checked || cbStatus.SelectedIndex == 3;
        if (!chkShowInactive.Checked && !requestingCancelled)
            conditions.Add("IsActive = true");

        string text = txtFilterValue.Text.Trim();
        if (text.Length > 0)
        {
            conditions.Add(cbFilterBy.SelectedIndex switch
            {
                1 => RowFilterHelper.Like("DocNo", text),
                2 => RowFilterHelper.Like("Description", text),
                3 => RowFilterHelper.Like("Type", text),
                _ => "(" + RowFilterHelper.Like("DocNo", text)
                     + " OR " + RowFilterHelper.Like("Description", text)
                     + " OR " + RowFilterHelper.Like("Type", text)
                     + " OR " + RowFilterHelper.Like("StatusName", text) + ")",
            });
        }

        return string.Join(" AND ", conditions);
    }

    private void _ApplyFilter()
    {
        var table = _movements;
        if (table == null || dgvTransactions.IsDisposed)
        {
            _UpdateClearFiltersState();
            return;
        }

        var view = table.DefaultView;
        view.RowFilter = _BuildFilterExpression();
        view.Sort = "MovementDate ASC, DocNo ASC";

        _RecalculateRunningBalance(view);

        dgvTransactions.DataSource = view;

        decimal inflow = 0m;
        decimal outflow = 0m;

        foreach (DataRowView rowView in view)
        {
            DataRow row = rowView.Row;
            decimal amount = row["Amount"] is decimal value ? value : 0m;
            int direction = row["Direction"] is int dir ? dir : 0;

            if (direction == DirectionIn) inflow += amount;
            else if (direction == DirectionOut) outflow += amount;
        }

        lblRecordsCount.Text = view.Count.ToString();
        lblSummary.Text =
            $"المعروض: {view.Count} من {table.Rows.Count} — الوارد: {clsFormat.Money(inflow)} · الصادر: {clsFormat.Money(outflow)} · الرصيد: {clsFormat.Money(inflow - outflow)}";

        lblEmpty.Visible = view.Count == 0;
        if (lblEmpty.Visible) lblEmpty.BringToFront();

        _UpdateClearFiltersState();   // RULE-N148 — بعد كل تطبيق تصفية
    }

    /// <summary>يعيد الشاشة إلى «لا بيانات» بعد اختفاء الصندوق أو فشل تحميل الحركات.</summary>
    private void _ShowEmptyState()
    {
        _movements = null;

        if (dgvTransactions.IsDisposed) return;

        dgvTransactions.DataSource = null;
        lblRecordsCount.Text = "0";
        lblSummary.Text = "المعروض: 0 من 0 — الوارد: 0.00 · الصادر: 0.00 · الرصيد: 0.00";
        lblEmpty.Visible = true;
        lblEmpty.BringToFront();
    }

    /// <summary>يعيد احتساب الرصيد الجاري على المدى المعروض بالترتيب الزمني (RULE-001).</summary>
    private static void _RecalculateRunningBalance(DataView view)
    {
        decimal running = 0m;

        foreach (DataRowView rowView in view)
        {
            DataRow row = rowView.Row;
            decimal amount = row["Amount"] is decimal value ? value : 0m;
            int direction = row["Direction"] is int dir ? dir : 0;

            running += direction * amount;
            row["RunningBalance"] = running;
        }
    }

    // ── أدوات التصفية ──────────────────────────────────────────────────────────

    /// <summary>
    /// «مسح التصفية» — يمسح **أدوات التصفية فقط** بلا أي إعادة تحميل (RULE-N148):
    /// نص التصفية · «تصفية بـ» و«الحالة» إلى الفهرس الافتراضي · مربعات الاختيار · النطاق الزمني الافتراضي.
    /// </summary>
    private void btnClearFilters_Click(object sender, EventArgs e)
        => _ClearAllFilters();   // يمسح ثم يُعيد التركيز إلى حقل التصفية (RULE-N148)

    /// <summary>
    /// تفريغ أدوات التصفية وحدها بلا تطبيق ولا إعادة تحميل (RULE-N148).
    /// النطاق الزمني يعود إلى القيم الملتقطة عند إنشاء الشاشة، لا إلى «اليوم» المتحرك.
    /// </summary>
    private void _ClearFilterInputs()
    {
        // منع المعالجات من إعادة تطبيق التصفية أثناء إرجاع القيم إلى الافتراضي.
        _isInitializing = true;
        try
        {
            txtFilterValue.Clear();
            cbFilterBy.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            chkOnlyCancelled.Checked = false;
            chkShowInactive.Checked = false;
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }
        finally
        {
            _isInitializing = false;
        }
    }

    /// <summary>Esc: مسح كل التصفية إن كان المؤشر داخل صندوق التصفية، وإلا إغلاق الشاشة.</summary>
    private void _GlobalEscape()
    {
        if (gbFilter.ContainsFocus)
        {
            _ClearAllFilters();
            return;
        }

        Close();
    }

    /// <summary>يمسح كل أدوات التصفية ويعيد الشبكة إلى حالتها الافتراضية — بلا إعادة تحميل (RULE-N148).</summary>
    private void _ClearAllFilters()
    {
        _ClearFilterInputs();
        _ApplyFilter();
        _UpdateClearFiltersState();
        txtFilterValue.Focus();
    }

    /// <summary>RULE-N43 + RULE-N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية تختلف عن الافتراضي.</summary>
    private void _UpdateClearFiltersState()
    {
        if (btnClearFilters is null) return;

        btnClearFilters.Enabled =
            !string.IsNullOrWhiteSpace(txtFilterValue.Text)
            || cbFilterBy.SelectedIndex != 0
            || cbStatus.SelectedIndex != 0
            || chkOnlyCancelled.Checked
            || chkShowInactive.Checked
            || dtpFrom.Value.Date != _defaultFrom.Date
            || dtpTo.Value.Date != _defaultTo.Date;
    }

    private void _SetDateRange(DateTime from, DateTime to)
    {
        _isInitializing = true;
        try
        {
            dtpFrom.Value = from;
            dtpTo.Value = to;
        }
        finally
        {
            _isInitializing = false;
        }

        _ApplyFilter();
    }

    // ── الأحداث ────────────────────────────────────────────────────────────────

    private async void cmbCashBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;

        try
        {
            if (cmbCashBox.SelectedValue is int cashBoxId && cashBoxId > 0)
            {
                await _LoadMovementsAsync(cashBoxId);
                return;
            }

            _ShowEmptyState();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تبديل الصندوق في شاشة الحركات");
            MessageBox.Show("تعذر تحميل حركات الصندوق. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// «تحديث» (F5) — إعادة تحميل كاملة لبيانات الشاشة، لا مسحٌ للتصفية وحدها (RULE-N148).
    /// </summary>
    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        try
        {
            await _RefreshAsync();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تحديث حركات الصندوق");
            MessageBox.Show("تعذر تحديث البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
        => Close();

    private void txtFilterValue_TextChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpFrom_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void dtpTo_ValueChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void chkOnlyCancelled_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void chkShowInactive_CheckedChanged(object sender, EventArgs e)
    {
        if (_isInitializing) return;
        _ApplyFilter();
    }

    private void btnRangeToday_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today, DateTime.Today);

    private void btnRangeMonth_Click(object sender, EventArgs e)
    {
        var today = DateTime.Today;
        _SetDateRange(new DateTime(today.Year, today.Month, 1), today);
    }

    private void btnRange30_Click(object sender, EventArgs e)
        => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

    // ── قائمة السياق والإجراءات ────────────────────────────────────────────────

    private void cmsTransactions_Opening(object sender, CancelEventArgs e)
    {
        bool hasRow = _GetSelectedRow() is not null;
        bool hasCashBox = cmbCashBox.SelectedValue is int cashBoxId && cashBoxId > 0;

        tsmiOpenDocument.Enabled = hasRow;
        tsmiExport.Enabled = dgvTransactions.Rows.Count > 0;
        tsmiRefresh.Enabled = hasCashBox;
    }

    private void tsmiRefresh_Click(object sender, EventArgs e)
        => btnRefresh.PerformClick();   // مسار تحديث واحد (RULE-N148)

    private void tsmiExport_Click(object sender, EventArgs e)
        => _ExportToExcel();

    private void tsmiOpenDocument_Click(object sender, EventArgs e)
        => _OpenSelectedDocument();

    private void dgvTransactions_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

        _OpenSelectedDocument();
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    /// <summary>يفتح مستند الحركة المحددة (سند قبض أو مصروف) في شاشته النمطية (RULE-N09).</summary>
    private void _OpenSelectedDocument()
    {
        DataRowView? selected = _GetSelectedRow();
        if (selected is null) return;

        DataRow row = selected.Row;
        int docId = row["DocId"] is int id ? id : 0;
        string docKind = row["DocKind"] as string ?? string.Empty;

        if (docId <= 0) return;

        try
        {
            if (string.Equals(docKind, DocKindReceipt, StringComparison.Ordinal))
            {
                var form = _serviceProvider.GetRequiredService<frmAddUpdateCustomerReceipt>();
                form._SetUpdateMode(docId);
                form.ShowDialog(this);
            }
            else if (string.Equals(docKind, DocKindExpense, StringComparison.Ordinal))
            {
                var form = _serviceProvider.GetRequiredService<frmAddUpdateExpense>();
                form._SetUpdateMode(docId);
                form.ShowDialog(this);
            }
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر فتح مستند الحركة {DocKind}/{DocId}", docKind, docId);
            MessageBox.Show("تعذر فتح المستند. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        _ = _RefreshMovementsAsync();   // المستند قد تغيّر — تُعاد الحركات (بلا مسح تصفية ولا إعادة تحميل كامل)
    }

    /// <summary>إعادة قراءة حركات الصندوق المعروض فقط — تُستخدم بعد إغلاق مستند، بلا لمس أدوات التصفية.</summary>
    private async Task _RefreshMovementsAsync()
    {
        if (IsDisposed) return;

        if (cmbCashBox.SelectedValue is int cashBoxId && cashBoxId > 0)
            await _LoadMovementsAsync(cashBoxId);
        else
            _ShowEmptyState();
    }

    private DataRowView? _GetSelectedRow()
        => dgvTransactions.CurrentRow?.DataBoundItem as DataRowView;

    /// <summary>تصدير المعروض إلى Excel عبر الأداة الموحّدة (RULE-N112).</summary>
    private void _ExportToExcel()
    {
        try
        {
            if (dgvTransactions.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات معروضة للتصدير.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = $"حركات_الصندوق_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                Title = "تصدير حركات الصندوق إلى Excel"
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;
            ExportHelper.ExportToExcel(dgvTransactions, "حركات الصندوق", dialog.FileName);

            MessageBox.Show("تم تصدير البيانات بنجاح.", "تم التصدير",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "تعذر تصدير حركات الصندوق إلى Excel");
            MessageBox.Show("تعذر تصدير البيانات. يرجى إعادة المحاولة.", "خطأ",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }
}
