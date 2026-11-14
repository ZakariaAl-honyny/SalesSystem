using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    /// <summary>
    /// قائمة حركات المخزون (عرض فقط): التاريخ · المستودع · نوع الحركة · الكمية · الرصيد · المرجع.
    /// <para>
    /// المعيار المطبَّق: §2.21 + §2.22 (gbFilter + pnlSummary + btnToggleFilters معلن في المصمّم)
    /// + §2.27 (الستة من KeyboardHelper) + §2.34 + §2.35 (المصمّم = وقت التشغيل).
    /// </para>
    /// <para>
    /// التصفية فورية بلا زر بحث (RULE-N59)، وبناء <c>DataView.RowFilter</c> يمرّ عبر
    /// <see cref="RowFilterHelper"/> على أعمدة معروفة فقط (RULE-N24/RULE-N60).
    /// البيانات تُقرأ من <see cref="IReportDesktopService"/> فقط — لا وصول لأي طبقة أخرى (RULE-N05).
    /// </para>
    /// </summary>
    public partial class frmInventoryList : Form
    {
        /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
        private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

        private readonly IServiceProvider _serviceProvider;

        private DataTable _movementsTable = new();
        private DataView _movementsView = new();

        /// <summary>يبدأ true حتى لا تنطلق تصفية قبل اكتمال تحميل البيانات وتهيئة أدوات التصفية.</summary>
        private bool _isInitializing = true;

        /// <summary>RULE-N148: بداية النطاق الزمني الافتراضي — تُلتقط عند إنشاء الشاشة («مسح التصفية» يعيدها).</summary>
        private readonly DateTime _defaultFrom;

        /// <summary>RULE-N148: نهاية النطاق الزمني الافتراضي — تُلتقط عند إنشاء الشاشة («مسح التصفية» يعيدها).</summary>
        private readonly DateTime _defaultTo;

        public frmInventoryList(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();

            // النطاق الافتراضي المعتمد للشاشة: من أول الشهر الحالي حتى اليوم.
            var today = DateTime.Today;
            _defaultFrom = new DateTime(today.Year, today.Month, 1);
            _defaultTo = today;

            // RULE-N40…N46 — تنقل وتحديد وتظليل الحقل النشط وأسهم RTL.
            KeyboardHelper.WireEnterAsTab(this);
            KeyboardHelper.WireSelectAllOnFocus(this);
            KeyboardHelper.WireFocusHighlight(this, FocusColor);
            KeyboardHelper.WireArrowNavigation(this);

            // RULE-N43 — مظهر واضح للأزرار المعطّلة.
            KeyboardHelper.WireButtonDisabledVisual(btnRefresh, btnClose, btnClearFilters);

            // RULE-N47 — F5 تحديث · Esc مسح التصفية/إغلاق.
            KeyboardHelper.WireGlobalShortcuts(this,
                (Keys.F5, () => btnRefresh.PerformClick()),
                (Keys.Escape, _GlobalEscape));

            // RULE-N147: Enter = الزر الأساسي (شاشة عرض بلا «جديد» ⇒ «تحديث»).
            KeyboardHelper.WireModeEnter(this, _PrimaryActionButton);
            AcceptButton = _PrimaryActionButton();

            _SetupToolTips();

            ApplyUiTheme();   // RULE-N50 — ينتهي بالثيم الأخضر

            // RULE-N63 — طيّ/إظهار صندوق التصفية، آخر سطر في المُنشئ.
            FilterPanelHelper.Attach(this, _serviceProvider);
        }

        /// <summary>عقد الطبقة المكتبية لبيانات التقارير/الحركات (RULE-N04).</summary>
        private IReportDesktopService ReportService
            => _serviceProvider.GetRequiredService<IReportDesktopService>();

        /// <summary>
        /// الزر الأساسي للشاشة لاختصار Enter (RULE-N147): شاشة عرض بلا «جديد» ⇒ «تحديث».
        /// يُقرأ عند كل ضغطة Enter.
        /// </summary>
        private Button? _PrimaryActionButton() => btnRefresh;

        // ── الثيم ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// الثيم الأخضر (RULE-N50…RULE-N53): يبدأ بـ ThemeHelper وينتهي بـ
        /// <c>GreenThemeHelper.Apply(this)</c> **كآخر سطر**، والقيم النهائية مطابقة حرفياً
        /// لما أُعلن في <c>frmInventoryList.Designer.cs</c> (RULE-N134/N140…N144).
        /// </summary>
        private void ApplyUiTheme()
        {
            ThemeHelper.ApplyMainFormStyle(this);   // يفرض أبيض/Segoe UI ⇒ يردّه الثيم الأخضر أدناه

            GreenThemeHelper.ApplyHeader(lblMode);          // RULE-N53
            GreenThemeHelper.ApplyGrid(dgvMovements);       // RULE-N51/N52
            lblSummary.ForeColor = GreenThemeHelper.TextStrong;
            lblRecordsCount.ForeColor = GreenThemeHelper.TextStrong;
            label1.ForeColor = GreenThemeHelper.TextStrong;

            // RULE-N22: سلوك الشبكة تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
            dgvMovements.ReadOnly = true;
            dgvMovements.AllowUserToAddRows = false;
            dgvMovements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovements.MultiSelect = false;

            GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
        }

        /// <summary>تلميحات الأزرار وأدوات التصفية مع اختصاراتها (RULE-N47).</summary>
        private void _SetupToolTips()
        {
            toolTip1.SetToolTip(btnRefresh, "إعادة تحميل الحركات (F5)");
            toolTip1.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
            toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
            toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
            toolTip1.SetToolTip(cbStatus, "تصفية حسب اتجاه الحركة (وارد/صادر)");
            toolTip1.SetToolTip(btnRangeToday, "حركات اليوم فقط");
            toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
            toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
            toolTip1.SetToolTip(chkOnlyOutOfStock, "إظهار الحركات التي انتهى الرصيد بعدها إلى صفر أو أقل");
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupGridColumns()
        {
            dgvMovements.Columns.Clear();

            var columns = new[]
            {
                new { Name = "MovementDate",   Header = "التاريخ",        Fill = 12, Min = 150, Format = (string?)"yyyy/MM/dd HH:mm" },
                new { Name = "WarehouseName",  Header = "المستودع",       Fill = 12, Min = 130, Format = (string?)null },
                new { Name = "MovementType",   Header = "نوع الحركة",     Fill = 12, Min = 120, Format = (string?)null },
                new { Name = "QuantityChange", Header = "الكمية",         Fill = 10, Min = 100, Format = (string?)"N3" },
                new { Name = "QuantityAfter",  Header = "الرصيد",         Fill = 10, Min = 100, Format = (string?)"N3" },
                new { Name = "ReferenceNo",    Header = "رقم المرجع",     Fill = 10, Min = 120, Format = (string?)null },
            };

            foreach (var col in columns)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.Name,
                    MinimumWidth = col.Min,
                    FillWeight = col.Fill,
                    ReadOnly = true,
                };

                if (col.Format != null)
                    column.DefaultCellStyle.Format = col.Format;

                dgvMovements.Columns.Add(column);
            }
        }

        // ── Data Loading ───────────────────────────────────────────────────────────

        /// <summary>
        /// RULE-N25/N26: تحميل غير متزامن مع try/catch و WaitCursor واستعادته في finally.
        /// البيانات تُقرأ كاملة مرة واحدة ثم تُصفّى في الذاكرة لتبقى التصفية فورية (RULE-N59).
        /// </summary>
        private async Task _RefreshMovementsListAsync()
        {
            _movementsTable = new DataTable();
            _movementsTable.Columns.Add("MovementDate", typeof(DateTime));
            _movementsTable.Columns.Add("WarehouseName", typeof(string));
            _movementsTable.Columns.Add("MovementType", typeof(string));
            _movementsTable.Columns.Add("QuantityChange", typeof(decimal));
            _movementsTable.Columns.Add("QuantityAfter", typeof(decimal));
            _movementsTable.Columns.Add("ReferenceNo", typeof(string));

            var result = await ReportService.GetMovementsAsync();

            if (result.IsSuccess && result.Value != null)
            {
                foreach (var m in result.Value)
                {
                    _movementsTable.Rows.Add(
                        m.Date,
                        m.WarehouseName ?? string.Empty,
                        m.MovementType ?? string.Empty,
                        m.QuantityChange,
                        m.QuantityAfter,
                        m.ReferenceNo ?? string.Empty);
                }
            }
            else
            {
                clsDialogs.Error(result.Error ?? "تعذر تحميل بيانات حركات المخزون.");
            }

            _movementsView = new DataView(_movementsTable);
            dgvMovements.DataSource = _movementsView;

            _ApplyFilter();
        }

        private async void frmInventoryList_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            btnRefresh.Enabled = false;
            _isInitializing = true;

            try
            {
                _SetupGridColumns();
                _SetDateRangeThisMonth();
                _isInitializing = false;

                await _RefreshMovementsListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر تحميل قائمة حركات المخزون");
                clsDialogs.Error("تعذر تحميل بيانات الحركات. يرجى إعادة المحاولة.");
            }
            finally
            {
                _isInitializing = false;

                if (!IsDisposed)
                {
                    btnRefresh.Enabled = true;
                    Cursor = Cursors.Default;
                    _UpdateClearFiltersState();   // RULE-N43/N148 — الحالة الابتدائية للزر بعد التحميل
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            btnRefresh.Enabled = false;

            try
            {
                // RULE-N148: «تحديث» = مسح التصفية ← إعادة تحميل البيانات ← إعادة الملخص والاختيار للافتراضي.
                _ClearFilterInputs();

                await _RefreshMovementsListAsync();

                dgvMovements.ClearSelection();
                _UpdateClearFiltersState();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر تحديث قائمة حركات المخزون");
                clsDialogs.Error("تعذر تحديث بيانات الحركات. يرجى إعادة المحاولة.");
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

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        // ── التصفية الفورية (RULE-N56…N61) ─────────────────────────────────────────

        /// <summary>
        /// التصفية الفورية: الحقل المختار + اتجاه الحركة + النطاق الزمني + «الرصيد النافد فقط».
        /// لا يوجد زر بحث (RULE-N59)، وبناء الصيغة يمرّ عبر <see cref="RowFilterHelper"/>.
        /// </summary>
        private void _ApplyFilter()
        {
            if (_isInitializing || IsDisposed || _movementsView.Table == null) return;

            var parts = new List<string>();

            // 1) نص التصفية على الحقل المختار (RULE-N60: العمود من ثوابت الكود).
            string search = txtFilterValue.Text.Trim();
            if (search.Length > 0)
            {
                parts.Add(cbFilterBy.SelectedIndex switch
                {
                    1 => RowFilterHelper.Like("WarehouseName", search),
                    2 => RowFilterHelper.Like("ReferenceNo", search),
                    3 => RowFilterHelper.Like("MovementType", search),
                    _ => "(" + RowFilterHelper.Like("WarehouseName", search)
                              + " OR " + RowFilterHelper.Like("ReferenceNo", search)
                              + " OR " + RowFilterHelper.Like("MovementType", search) + ")",
                });
            }

            // 2) اتجاه الحركة — 0 = الكل، 1 = وارد، 2 = صادر.
            if (cbStatus.SelectedIndex == 1)
                parts.Add("QuantityChange > 0");
            else if (cbStatus.SelectedIndex == 2)
                parts.Add("QuantityChange < 0");

            // 3) النطاق الزمني — النطاق المعكوس يُبادَل طرفاه بدل إسقاط النتائج.
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;
            if (from > to)
                (from, to) = (to, from);

            parts.Add($"MovementDate >= #{from:yyyy-MM-dd}#");
            parts.Add($"MovementDate <= #{to:yyyy-MM-dd}#");

            // 4) الحركات التي انتهى الرصيد بعدها إلى صفر أو أقل.
            if (chkOnlyOutOfStock.Checked)
                parts.Add("QuantityAfter <= 0");

            // إن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).
            try
            {
                _movementsView.RowFilter = parts.Count > 0 ? string.Join(" AND ", parts) : string.Empty;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر تطبيق تصفية حركات المخزون — أُبقيت التصفية السابقة كما هي");
            }

            _UpdateSummary();
            _UpdateClearFiltersState();   // RULE-N43/N148 — الزر مفعّل فقط عند وجود تصفية نشطة
        }

        /// <summary>RULE-N61: عدد المعروض من الكل + إجمالي الوارد/الصادر (N3) + سطر «لا توجد بيانات».</summary>
        private void _UpdateSummary()
        {
            decimal totalIn = 0m;
            decimal totalOut = 0m;

            foreach (DataRowView row in _movementsView)
            {
                decimal change = row["QuantityChange"] is decimal value ? value : 0m;

                if (change > 0) totalIn += change;
                else totalOut += change;
            }

            lblRecordsCount.Text = _movementsView.Count.ToString();
            lblSummary.Text = $"المعروض: {_movementsView.Count} من {_movementsTable.Rows.Count}"
                            + $" — وارد: {totalIn:N3} · صادر: {Math.Abs(totalOut):N3}"
                            + $" · صافي: {(totalIn + totalOut):N3}";

            lblEmpty.Visible = _movementsView.Count == 0;

            if (lblEmpty.Visible)
                lblEmpty.BringToFront();
        }

        // ── أدوات التصفية ──────────────────────────────────────────────────────────

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

        /// <summary>«هذا الشهر» = من أول الشهر الحالي حتى اليوم (الافتراضي ومسح Esc).</summary>
        private void _SetDateRangeThisMonth()
        {
            dtpFrom.Value = _defaultFrom;
            dtpTo.Value = _defaultTo;
        }

        /// <summary>
        /// «مسح التصفية» (RULE-N148): يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
        /// بلا إعادة تحميل — «تحديث» هو المسؤول عن إعادة تحميل البيانات.
        /// </summary>
        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            _ClearAllFilters();
            txtFilterValue.Focus();
        }

        /// <summary>تفريغ أدوات التصفية وحدها إلى قيمها الافتراضية (بلا تطبيق ولا تحميل).</summary>
        private void _ClearFilterInputs()
        {
            bool wasInitializing = _isInitializing;
            _isInitializing = true;

            try
            {
                cbFilterBy.SelectedIndex = 0;
                cbStatus.SelectedIndex = 0;
                chkOnlyOutOfStock.Checked = false;
                txtFilterValue.Clear();
                dtpFrom.Value = _defaultFrom;
                dtpTo.Value = _defaultTo;
            }
            finally
            {
                _isInitializing = wasInitializing;
            }
        }

        /// <summary>Esc: مسح كل أدوات التصفية وإعادة النطاق إلى «هذا الشهر».</summary>
        private void _ClearAllFilters()
        {
            _ClearFilterInputs();

            _ApplyFilter();
            txtFilterValue.Focus();
        }

        /// <summary>RULE-N43/N148: زر «مسح التصفية» مفعّل فقط عند وجود تصفية مختلفة عن الافتراضي.</summary>
        private void _UpdateClearFiltersState()
        {
            if (btnClearFilters is null) return;

            btnClearFilters.Enabled =
                !string.IsNullOrWhiteSpace(txtFilterValue.Text)
                || cbFilterBy.SelectedIndex > 0
                || cbStatus.SelectedIndex > 0
                || chkOnlyOutOfStock.Checked
                || dtpFrom.Value.Date != _defaultFrom
                || dtpTo.Value.Date != _defaultTo;
        }

        /// <summary>RULE-N47 + RULE-N59: Esc داخل صندوق التصفية = مسحها، وخارجه = إغلاق الشاشة.</summary>
        private void _GlobalEscape()
        {
            if (gbFilter.ContainsFocus)
            {
                _ClearAllFilters();
                return;
            }

            Close();
        }

        // ── Drill-down ─────────────────────────────────────────────────────────────

        /// <summary>تصفية فورية على قيمة الخلية (النقر المزدوج أو عنصر قائمة السياق).</summary>
        private void _DrillDown(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string? value = dgvMovements.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                clsDialogs.Warning("الخلية المحددة لا تحمل قيمة تُصفّى بها.");
                return;
            }

            _isInitializing = true;
            try
            {
                txtFilterValue.Text = value;
            }
            finally
            {
                _isInitializing = false;
            }

            _ApplyFilter();
        }

        private void miDrillDown_Click(object sender, EventArgs e)
        {
            var cell = dgvMovements.CurrentCell;

            if (cell is null)
            {
                clsDialogs.Warning("يرجى تحديد سطر في الشبكة أولاً.");
                return;
            }

            _DrillDown(new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex));
        }

        private void miRefresh_Click(object sender, EventArgs e)
            => btnRefresh.PerformClick();

        private void miClearFilters_Click(object sender, EventArgs e)
            => _ClearAllFilters();

        // ── أحداث التصفية الفورية ──────────────────────────────────────────────────

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
            => _ApplyFilter();

        /// <summary>RULE-N59: Enter يطبّق التصفية فوراً (وهي مطبَّقة أصلاً عند كل كتابة).</summary>
        private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

            _ApplyFilter();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
            => _ApplyFilter();

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
            => _ApplyFilter();

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
            => _ApplyFilter();

        private void dtpTo_ValueChanged(object sender, EventArgs e)
            => _ApplyFilter();

        private void chkOnlyOutOfStock_CheckedChanged(object sender, EventArgs e)
            => _ApplyFilter();

        private void btnRangeToday_Click(object sender, EventArgs e)
            => _SetDateRange(DateTime.Today, DateTime.Today);

        private void btnRangeMonth_Click(object sender, EventArgs e)
            => _SetDateRange(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);

        private void btnRange30_Click(object sender, EventArgs e)
            => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

        // ── أحداث الشبكة ───────────────────────────────────────────────────────────

        private void dgvMovements_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            => _DrillDown(e);

        private void dgvMovements_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var changeColumn = dgvMovements.Columns["QuantityChange"];
            if (changeColumn == null || e.ColumnIndex != changeColumn.Index || e.Value is null) return;

            decimal qty = (decimal)e.Value;
            if (qty < 0)
            {
                e.CellStyle.ForeColor = Color.Red;
            }
            else if (qty > 0)
            {
                e.CellStyle.ForeColor = Color.Green;
            }
        }
    }
}
