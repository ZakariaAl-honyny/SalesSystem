using System.Data;
using SalesSystem.Desktop.Helpers;
using Serilog;

namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    /// <summary>
    /// شاشة المخزون الرئيسية (نظرة عامة): أرصدة الأصناف · محاضر الجرد · التحويلات المخزنية.
    /// <para>
    /// المعيار المطبَّق: §2.21 + §2.22 (gbFilter + pnlSummary + btnToggleFilters + btnClearFilters معلنة في المصمّم)
    /// + §2.18 (RULE-N147: Enter = الزر الأساسي) + §2.27 (الستة من KeyboardHelper)
    /// + §2.34 (عنوان ← أوامر ← تصفية ← محتوى ← إجماليات) + §2.35 (المصمّم = وقت التشغيل).
    /// </para>
    /// <para>
    /// البيانات تجريبية في الذاكرة (UI-First) — لا وصول لأي طبقة بيانات من الشاشة.
    /// التصفية فورية (RULE-N59) بلا زر بحث، وبناء <c>DataView.RowFilter</c> يمرّ عبر
    /// <see cref="RowFilterHelper"/> على أعمدة معروفة فقط (RULE-N24/RULE-N60).
    /// </para>
    /// </summary>
    public partial class frmInventory : Form
    {
        /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
        private static readonly Color FocusColor = Color.FromArgb(0xD6, 0xE4, 0xF0);

        // ── ثوابت الحالات (RULE-N60: تُقارن بثوابت داخل الكود، لا بنص المستخدم) ─────
        private const string StockOut = "نفد";
        private const string StockLow = "محدود";
        private const string StockAvailable = "متوفر";

        private const string CountDraft = "مسودة";
        private const string CountDone = "مكتمل";
        private const string CountPosted = "مرحّل";

        private const string TransferDraft = "مسودة";
        private const string TransferTransit = "قيد النقل";
        private const string TransferReceived = "مستلم";

        private readonly IServiceProvider _serviceProvider;

        /// <summary>يبدأ true حتى لا تنطلق تصفية قبل اكتمال تهيئة أدوات التصفية.</summary>
        private bool _isInitializing = true;

        /// <summary>القيم الافتراضية لأدوات التصفية كما تُفتح بها الشاشة — مرجع «مسح التصفية» (RULE-N148).</summary>
        private DateTime _defaultFrom;
        private DateTime _defaultTo;

        private DataTable _productsTable = new();
        private DataView _productsView = new();
        private DataTable _countsTable = new();
        private DataView _countsView = new();
        private DataTable _transfersTable = new();
        private DataView _transfersView = new();

        // ── Status badge colors ─────────────────────────────────────────────────────

        private static readonly Dictionary<string, Color> _countStatusColors = new()
        {
            { CountDraft, Color.SlateGray },
            { CountDone, Color.SteelBlue },
            { CountPosted, Color.ForestGreen },
        };

        private static readonly Dictionary<string, Color> _transferStatusColors = new()
        {
            { TransferDraft, Color.SlateGray },
            { TransferTransit, Color.Goldenrod },
            { TransferReceived, Color.ForestGreen },
        };

        // ── Mock data models (UI-first prototype — no backend) ────────────────────

        private sealed record MockProduct(
            string Code, string Barcode, string Name, string Unit,
            string Warehouse, decimal Stock, decimal ReorderLevel, decimal Cost, bool IsActive);

        private sealed record MockCount(
            string CountNo, DateTime Date, string Warehouse, string Committee, string Status);

        private sealed record MockTransfer(
            string TransferNo, DateTime Date, string From, string To, string Driver, string Status);

        public frmInventory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();

            // القيم الافتراضية لأدوات التصفية — تُلتقط مرة واحدة عند البناء (RULE-N148).
            _CaptureFilterDefaults();

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

            // RULE-N147 — Enter = الزر الأساسي (لا يوجد «جديد» في شاشة عرض ⇒ «تحديث»).
            KeyboardHelper.WireModeEnter(this, () => btnRefresh);
            AcceptButton = btnRefresh;

            _SetupToolTips();

            ApplyUiTheme();   // RULE-N50 — ينتهي بالثيم الأخضر

            // RULE-N63 — طيّ/إظهار صندوق التصفية، آخر سطر في المُنشئ.
            FilterPanelHelper.Attach(this, _serviceProvider);
        }

        // ── الثيم ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// الثيم الأخضر (RULE-N50…RULE-N53): يبدأ بـ ThemeHelper وينتهي بـ
        /// <c>GreenThemeHelper.Apply(this)</c> **كآخر سطر**، والقيم النهائية مطابقة حرفياً
        /// لما أُعلن في <c>frmInventory.Designer.cs</c> (RULE-N134/N140…N144).
        /// </summary>
        private void ApplyUiTheme()
        {
            ThemeHelper.ApplyMainFormStyle(this);   // يفرض أبيض/Segoe UI ⇒ يردّه الثيم الأخضر أدناه

            GreenThemeHelper.ApplyHeader(lblMode);                    // RULE-N53
            GreenThemeHelper.ApplyGrid(dgvProducts);                  // RULE-N51/N52
            GreenThemeHelper.ApplyGrid(dgvCounts);
            GreenThemeHelper.ApplyGrid(dgvTransfers);
            lblSummary.ForeColor = GreenThemeHelper.TextStrong;       // RULE-N53

            // RULE-N22: سلوك الشبكات تعلنه الشاشة نفسها — ApplyGrid لا يلمسه (RULE-N52).
            foreach (var grid in new[] { dgvProducts, dgvCounts, dgvTransfers })
            {
                grid.ReadOnly = true;
                grid.AllowUserToAddRows = false;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.MultiSelect = false;
            }

            GreenThemeHelper.Apply(this);   // ← الأخير دائماً (RULE-N50)
        }

        /// <summary>تلميحات الأزرار وأدوات التصفية مع اختصاراتها (RULE-N47).</summary>
        private void _SetupToolTips()
        {
            toolTip1.SetToolTip(btnRefresh, "تحديث بيانات المخزون (F5)");
            toolTip1.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
            toolTip1.SetToolTip(cbFilterBy, "اختر الحقل الذي تُصفّي به");
            toolTip1.SetToolTip(txtFilterValue, "اكتب قيمة التصفية — التصفية فورية (Esc يمسحها)");
            toolTip1.SetToolTip(cbStatus, "تصفية بحالة السطر في تبويب المحتوى الحالي");
            toolTip1.SetToolTip(btnRangeToday, "نطاق اليوم (لمحاضر الجرد والتحويلات)");
            toolTip1.SetToolTip(btnRangeMonth, "من بداية الشهر الحالي حتى اليوم");
            toolTip1.SetToolTip(btnRange30, "آخر 30 يوماً");
            toolTip1.SetToolTip(chkShowInactive, "إظهار الأصناف غير النشطة");
            toolTip1.SetToolTip(chkOnlyOutOfStock, "إظهار الأصناف النافدة فقط (الرصيد ≤ 0)");
        }

        // ── Mock Data Builders ─────────────────────────────────────────────────────

        private void _BuildProductsTable()
        {
            _productsTable = new DataTable();
            _productsTable.Columns.Add("Code", typeof(string));
            _productsTable.Columns.Add("Barcode", typeof(string));
            _productsTable.Columns.Add("Name", typeof(string));
            _productsTable.Columns.Add("Unit", typeof(string));
            _productsTable.Columns.Add("WarehouseName", typeof(string));
            _productsTable.Columns.Add("Stock", typeof(decimal));
            _productsTable.Columns.Add("ReorderLevel", typeof(decimal));
            _productsTable.Columns.Add("Cost", typeof(decimal));
            _productsTable.Columns.Add("StockStatus", typeof(string));
            _productsTable.Columns.Add("IsActive", typeof(bool));

            var products = new[]
            {
                new MockProduct("P-1001", "6251001234567", "سكر أبيض 1 كجم",           "كيس",     "الرئيسي", 0m,    20m,  32.50m,  true),
                new MockProduct("P-1002", "6251001234568", "شاي أوراق سائب 250 جم",     "عبوة",    "الرئيسي", 15m,   25m,  18.75m,  true),
                new MockProduct("P-1003", "6251001234569", "زيت عباد الشمس 1 لتر",      "زجاجة",   "المعرض",  120m,  40m,  46.00m,  true),
                new MockProduct("P-1004", "6251001234570", "أرز بسمتي 5 كجم",           "كيس",     "الرئيسي", 8m,    30m,  145.00m, true),
                new MockProduct("P-1005", "6251001234571", "دقيق أبيض فاخر 2 كجم",      "كيس",     "الرئيسي", 60m,   50m,  28.90m,  true),
                new MockProduct("P-1006", "6251001234572", "حليب مجفف 2 كجم",           "علبة",    "المعرض",  0m,    15m,  95.00m,  true),
                new MockProduct("P-1007", "6251001234573", "قهوة سريعة الذوبان 100 جم", "برطمان",  "الرئيسي", 45m,   20m,  22.40m,  true),
                new MockProduct("P-1008", "6251001234574", "معجون طماطم 400 جم",        "علبة",    "المعرض",  18m,   35m,  12.75m,  true),
                new MockProduct("P-1009", "6251001234575", "صابون سائل لليدين 500 مل",  "زجاجة",   "الرئيسي", 210m,  50m,  15.20m,  true),
                new MockProduct("P-1010", "6251001234576", "مناديل ورقية 200 منديل",    "علبة",    "المعرض",  4m,    25m,  8.60m,   true),
                new MockProduct("P-1011", "6251001234577", "عصير مانجو 1 لتر (موقوف)",  "كرتون",   "المعرض",  33m,   20m,  11.40m,  false),
            };

            foreach (var p in products)
            {
                string status = p.Stock <= 0
                    ? StockOut
                    : p.Stock <= p.ReorderLevel ? StockLow : StockAvailable;

                _productsTable.Rows.Add(
                    p.Code, p.Barcode, p.Name, p.Unit, p.Warehouse,
                    p.Stock, p.ReorderLevel, p.Cost, status, p.IsActive);
            }
        }

        private void _BuildCountsTable()
        {
            _countsTable = new DataTable();
            _countsTable.Columns.Add("CountNo", typeof(string));
            _countsTable.Columns.Add("CountDate", typeof(DateTime));
            _countsTable.Columns.Add("WarehouseName", typeof(string));
            _countsTable.Columns.Add("Committee", typeof(string));
            _countsTable.Columns.Add("Status", typeof(string));

            var counts = new[]
            {
                new MockCount("INV-2026-001", new DateTime(2026, 9, 5),  "الرئيسي", "رئيس: أحمد محمود / عضو: خالد سعيد", "مرحّل"),
                new MockCount("INV-2026-002", new DateTime(2026, 9, 12), "المعرض",  "رئيس: سامي حسن / عضو: عمر يوسف",    "مكتمل"),
                new MockCount("INV-2026-003", new DateTime(2026, 9, 18), "الرئيسي", "رئيس: أحمد محمود / عضو: ليلى إبراهيم", "مسودة"),
            };

            foreach (var c in counts)
            {
                _countsTable.Rows.Add(c.CountNo, c.Date, c.Warehouse, c.Committee, c.Status);
            }
        }

        private void _BuildTransfersTable()
        {
            _transfersTable = new DataTable();
            _transfersTable.Columns.Add("TransferNo", typeof(string));
            _transfersTable.Columns.Add("TransferDate", typeof(DateTime));
            _transfersTable.Columns.Add("FromWarehouse", typeof(string));
            _transfersTable.Columns.Add("ToWarehouse", typeof(string));
            _transfersTable.Columns.Add("DriverName", typeof(string));
            _transfersTable.Columns.Add("Status", typeof(string));

            var transfers = new[]
            {
                new MockTransfer("TR-2026-001", new DateTime(2026, 9, 8),  "الرئيسي", "المعرض",  "محمد علي", "مستلم"),
                new MockTransfer("TR-2026-002", new DateTime(2026, 9, 11), "المعرض",  "الرئيسي", "حسن فؤاد", "قيد النقل"),
                new MockTransfer("TR-2026-003", new DateTime(2026, 9, 15), "الرئيسي", "المعرض",  "محمد علي", "مستلم"),
                new MockTransfer("TR-2026-004", new DateTime(2026, 9, 19), "المعرض",  "الرئيسي", "—",        "مسودة"),
            };

            foreach (var t in transfers)
            {
                _transfersTable.Rows.Add(t.TransferNo, t.Date, t.From, t.To, t.Driver, t.Status);
            }
        }

        // ── Grid Setup ─────────────────────────────────────────────────────────────

        private void _SetupProductsGrid()
        {
            dgvProducts.Columns.Clear();

            var columns = new[]
            {
                new { Name = "Code",          Header = "الكود",       Fill = 7,  Min = 70, Format = (string?)null },
                new { Name = "Name",          Header = "الاسم",       Fill = 16, Min = 150, Format = (string?)null },
                new { Name = "Unit",          Header = "الوحدة",      Fill = 7,  Min = 70, Format = (string?)null },
                new { Name = "WarehouseName", Header = "المستودع",    Fill = 9,  Min = 90, Format = (string?)null },
                new { Name = "Stock",         Header = "الرصيد",      Fill = 7,  Min = 70, Format = "N3" },
                new { Name = "ReorderLevel",  Header = "حد الطلب",    Fill = 7,  Min = 70, Format = "N3" },
                new { Name = "Cost",          Header = "التكلفة",     Fill = 8,  Min = 80, Format = "N2" },
                new { Name = "StockStatus",   Header = "حالة الرصيد", Fill = 9,  Min = 90, Format = (string?)null },
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

                dgvProducts.Columns.Add(column);
            }
        }

        private void _SetupCountsGrid()
        {
            dgvCounts.Columns.Clear();

            var columns = new[]
            {
                new { Name = "CountNo",       Header = "رقم المحضر", Fill = 10, Min = 110, Format = (string?)null },
                new { Name = "CountDate",     Header = "التاريخ",     Fill = 10, Min = 100, Format = (string?)"yyyy/MM/dd" },
                new { Name = "WarehouseName", Header = "المستودع",    Fill = 12, Min = 100, Format = (string?)null },
                new { Name = "Committee",     Header = "لجنة الجرد",  Fill = 34, Min = 260, Format = (string?)null },
                new { Name = "Status",        Header = "الحالة",      Fill = 10, Min = 80,  Format = (string?)null },
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

                dgvCounts.Columns.Add(column);
            }
        }

        private void _SetupTransfersGrid()
        {
            dgvTransfers.Columns.Clear();

            var columns = new[]
            {
                new { Name = "TransferNo",    Header = "رقم السند",  Fill = 10, Min = 110, Format = (string?)null },
                new { Name = "TransferDate",  Header = "التاريخ",    Fill = 10, Min = 100, Format = (string?)"yyyy/MM/dd" },
                new { Name = "FromWarehouse", Header = "من مستودع",  Fill = 12, Min = 110, Format = (string?)null },
                new { Name = "ToWarehouse",   Header = "إلى مستودع", Fill = 12, Min = 110, Format = (string?)null },
                new { Name = "DriverName",    Header = "السائق",     Fill = 12, Min = 100, Format = (string?)null },
                new { Name = "Status",        Header = "الحالة",     Fill = 10, Min = 80,  Format = (string?)null },
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

                dgvTransfers.Columns.Add(column);
            }
        }

        // ── Data Binding ───────────────────────────────────────────────────────────

        private void _BindGrids()
        {
            _productsView = new DataView(_productsTable);
            dgvProducts.DataSource = _productsView;

            _countsView = new DataView(_countsTable);
            dgvCounts.DataSource = _countsView;

            _transfersView = new DataView(_transfersTable);
            dgvTransfers.DataSource = _transfersView;
        }

        // ── Filtering (RULE-N56…N61) ───────────────────────────────────────────────

        /// <summary>الشبكة الظاهرة في التبويب النشط — مصدر التصفية والنقر المزدوج.</summary>
        private DataGridView ActiveGrid => tabMain.SelectedIndex switch
        {
            1 => dgvCounts,
            2 => dgvTransfers,
            _ => dgvProducts
        };

        /// <summary>يطبّق التصفية على جدول التبويب النشط — التصفية فورية بلا زر بحث (RULE-N59).</summary>
        private void _ApplyActiveFilter()
        {
            if (_isInitializing || IsDisposed) return;

            switch (tabMain.SelectedIndex)
            {
                case 1:
                    _ApplyCountsFilter();
                    break;
                case 2:
                    _ApplyTransfersFilter();
                    break;
                default:
                    _ApplyProductsFilter();
                    break;
            }

            _UpdateSummary();
            _UpdateClearFiltersState();   // RULE-N148 — الزر مفعّل فقط عند وجود تصفية نشطة
        }

        private void _ApplyProductsFilter()
        {
            if (_productsView.Table == null) return;

            var parts = new List<string>();

            string search = txtFilterValue.Text.Trim();
            if (search.Length > 0)
            {
                parts.Add(cbFilterBy.SelectedIndex switch
                {
                    1 => RowFilterHelper.Like("Name", search),
                    2 => RowFilterHelper.Like("Code", search),
                    3 => RowFilterHelper.Like("Barcode", search),
                    _ => "(" + RowFilterHelper.Like("Name", search)
                              + " OR " + RowFilterHelper.Like("Code", search)
                              + " OR " + RowFilterHelper.Like("Barcode", search) + ")",
                });
            }

            if (!chkShowInactive.Checked)
                parts.Add("IsActive = true");

            if (chkOnlyOutOfStock.Checked)
                parts.Add("Stock <= 0");

            string? status = _SelectedProductsStatus();
            if (status is not null)
                parts.Add($"StockStatus = '{RowFilterHelper.Escape(status)}'");

            _SetRowFilter(_productsView, parts);
        }

        private void _ApplyCountsFilter()
        {
            if (_countsView.Table == null) return;

            var parts = new List<string>();

            string search = txtFilterValue.Text.Trim();
            if (search.Length > 0)
            {
                parts.Add(cbFilterBy.SelectedIndex switch
                {
                    1 => RowFilterHelper.Like("CountNo", search),
                    2 => RowFilterHelper.Like("WarehouseName", search),
                    3 => RowFilterHelper.Like("Committee", search),
                    _ => "(" + RowFilterHelper.Like("CountNo", search)
                              + " OR " + RowFilterHelper.Like("WarehouseName", search)
                              + " OR " + RowFilterHelper.Like("Committee", search) + ")",
                });
            }

            string? status = _SelectedCountsStatus();
            if (status is not null)
                parts.Add($"Status = '{RowFilterHelper.Escape(status)}'");

            _AddDateRange(parts, "CountDate");

            _SetRowFilter(_countsView, parts);
        }

        private void _ApplyTransfersFilter()
        {
            if (_transfersView.Table == null) return;

            var parts = new List<string>();

            string search = txtFilterValue.Text.Trim();
            if (search.Length > 0)
            {
                parts.Add(cbFilterBy.SelectedIndex switch
                {
                    1 => RowFilterHelper.Like("TransferNo", search),
                    2 => RowFilterHelper.Like("FromWarehouse", search),
                    3 => RowFilterHelper.Like("ToWarehouse", search),
                    4 => RowFilterHelper.Like("DriverName", search),
                    _ => "(" + RowFilterHelper.Like("TransferNo", search)
                              + " OR " + RowFilterHelper.Like("FromWarehouse", search)
                              + " OR " + RowFilterHelper.Like("ToWarehouse", search)
                              + " OR " + RowFilterHelper.Like("DriverName", search) + ")",
                });
            }

            string? status = _SelectedTransfersStatus();
            if (status is not null)
                parts.Add($"Status = '{RowFilterHelper.Escape(status)}'");

            _AddDateRange(parts, "TransferDate");

            _SetRowFilter(_transfersView, parts);
        }

        /// <summary>يضيف نطاق التاريخ (النطاق المعكوس يُبادَل طرفاه بدل إسقاط النتائج).</summary>
        private void _AddDateRange(List<string> parts, string dateColumn)
        {
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;
            if (from > to)
                (from, to) = (to, from);

            parts.Add($"{dateColumn} >= #{from:yyyy-MM-dd}#");
            parts.Add($"{dateColumn} <= #{to:yyyy-MM-dd}#");
        }

        /// <summary>إن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).</summary>
        private static void _SetRowFilter(DataView view, List<string> parts)
        {
            try
            {
                view.RowFilter = parts.Count > 0 ? string.Join(" AND ", parts) : string.Empty;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر تطبيق تصفية شاشة المخزون — أُبقيت التصفية السابقة كما هي");
            }
        }

        /// <summary>قيمة حالة الرصيد المقابلة لفهرس الاختيار (RULE-N60: لا مقارنة نصية).</summary>
        private string? _SelectedProductsStatus() => cbStatus.SelectedIndex switch
        {
            1 => StockAvailable,
            2 => StockLow,
            3 => StockOut,
            _ => null
        };

        private string? _SelectedCountsStatus() => cbStatus.SelectedIndex switch
        {
            1 => CountDraft,
            2 => CountDone,
            3 => CountPosted,
            _ => null
        };

        private string? _SelectedTransfersStatus() => cbStatus.SelectedIndex switch
        {
            1 => TransferDraft,
            2 => TransferTransit,
            3 => TransferReceived,
            _ => null
        };

        // ── Summary (RULE-N61) ─────────────────────────────────────────────────────

        private void _UpdateSummary()
        {
            switch (tabMain.SelectedIndex)
            {
                case 1:
                    lblSummary.Text = "محاضر الجرد — " + _Summarize(
                        _countsView, "Status",
                        (CountDraft, "مسودة"), (CountDone, "مكتمل"), (CountPosted, "مرحّل"));
                    break;
                case 2:
                    lblSummary.Text = "التحويلات المخزنية — " + _Summarize(
                        _transfersView, "Status",
                        (TransferDraft, "مسودة"), (TransferTransit, "قيد النقل"), (TransferReceived, "مستلم"));
                    break;
                default:
                    lblSummary.Text = _SummarizeProducts();
                    break;
            }

            _UpdateEmptyLabels();
        }

        /// <summary>ملخص شبكة بحالة نصية: المعروض/الكل + عدّاد كل حالة.</summary>
        private static string _Summarize(
            DataView view, string statusColumn,
            params (string Status, string Caption)[] statuses)
        {
            var counts = new int[statuses.Length];

            foreach (DataRowView row in view)
            {
                string value = row[statusColumn]?.ToString() ?? string.Empty;

                for (int i = 0; i < statuses.Length; i++)
                {
                    if (string.Equals(value, statuses[i].Status, StringComparison.Ordinal))
                    {
                        counts[i]++;
                        break;
                    }
                }
            }

            var builder = new System.Text.StringBuilder();
            builder.Append($"المعروض: {view.Count}");

            for (int i = 0; i < statuses.Length; i++)
                builder.Append($" · {statuses[i].Caption}: {counts[i]}");

            return builder.ToString();
        }

        /// <summary>ملخص الأرصدة: النافد/المحدود/المتوفر + قيمة المخزون (N2).</summary>
        private string _SummarizeProducts()
        {
            int outOfStock = 0, low = 0, available = 0;
            decimal stockValue = 0m;

            foreach (DataRowView row in _productsView)
            {
                string status = row["StockStatus"]?.ToString() ?? string.Empty;

                if (string.Equals(status, StockOut, StringComparison.Ordinal)) outOfStock++;
                else if (string.Equals(status, StockLow, StringComparison.Ordinal)) low++;
                else if (string.Equals(status, StockAvailable, StringComparison.Ordinal)) available++;

                decimal stock = row["Stock"] is decimal s ? s : 0m;
                decimal cost = row["Cost"] is decimal c ? c : 0m;
                stockValue += stock * cost;
            }

            return $"المعروض: {_productsView.Count} من {_productsTable.Rows.Count}"
                 + $" · نفد: {outOfStock} · محدود: {low} · متوفر: {available}"
                 + $" · قيمة المخزون: {stockValue:N2}";
        }

        /// <summary>سطر «لا توجد بيانات» لكل تبويب (RULE-N22/N23).</summary>
        private void _UpdateEmptyLabels()
        {
            _ToggleEmpty(lblEmptyProducts, _productsView.Count);
            _ToggleEmpty(lblEmptyCounts, _countsView.Count);
            _ToggleEmpty(lblEmptyTransfers, _transfersView.Count);
        }

        private static void _ToggleEmpty(Label label, int count)
        {
            label.Visible = count == 0;

            if (label.Visible)
                label.BringToFront();
        }

        // ── Filter Choices (per tab) ───────────────────────────────────────────────

        /// <summary>يعيد بناء خيارات «تصفية بـ» و«الحالة» بحسب التبويب النشط.</summary>
        private void _LoadFilterChoices()
        {
            bool wasInitializing = _isInitializing;
            _isInitializing = true;

            try
            {
                cbFilterBy.Items.Clear();
                cbStatus.Items.Clear();

                switch (tabMain.SelectedIndex)
                {
                    case 1:
                        cbFilterBy.Items.AddRange(new object[]
                        {
                            "الكل (رقم/مستودع/لجنة)", "رقم المحضر", "المستودع", "لجنة الجرد"
                        });
                        cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "مكتمل", "مرحّل" });
                        break;

                    case 2:
                        cbFilterBy.Items.AddRange(new object[]
                        {
                            "الكل (رقم/مستودعات/سائق)", "رقم السند", "من مستودع", "إلى مستودع", "السائق"
                        });
                        cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "قيد النقل", "مستلم" });
                        break;

                    default:
                        cbFilterBy.Items.AddRange(new object[]
                        {
                            "الكل (اسم/كود/باركود)", "الاسم", "الكود", "الباركود"
                        });
                        cbStatus.Items.AddRange(new object[] { "الكل", "متوفر", "محدود", "نفد" });
                        break;
                }

                cbFilterBy.SelectedIndex = 0;
                cbStatus.SelectedIndex = 0;
            }
            finally
            {
                _isInitializing = wasInitializing;
            }
        }

        /// <summary>نطاق التاريخ لا يُطبَّق على تبويب الأرصدة ⇒ يُعطَّل بوضوح بدل أن يبقى ميتاً.</summary>
        private void _ApplyDateFilterAvailability()
        {
            bool hasDates = tabMain.SelectedIndex != 0;

            dtpFrom.Enabled = hasDates;
            dtpTo.Enabled = hasDates;
            btnRangeToday.Enabled = hasDates;
            btnRangeMonth.Enabled = hasDates;
            btnRange30.Enabled = hasDates;
        }

        // ── Status Badge Helpers ───────────────────────────────────────────────────

        private static void _ApplyStatusBadge(DataGridView dgv, DataGridViewCellFormattingEventArgs e, Dictionary<string, Color> colors)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (!colors.TryGetValue(e.Value.ToString() ?? string.Empty, out var color)) return;

            e.CellStyle.BackColor = color;
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // ── Global Shortcuts ──────────────────────────────────────────────────────

        /// <summary>Esc: مسح كل التصفية إن كان المؤشر داخل صندوقها، وإلا إغلاق الشاشة (RULE-N47).</summary>
        private void _GlobalEscape()
        {
            if (gbFilter.ContainsFocus)
            {
                _ClearAllFilters();
                return;
            }

            Close();
        }

        /// <summary>
        /// «مسح التصفية» (RULE-N148) — يمسح **أدوات التصفية فقط** ويعيد الشبكة إلى وضعها الافتراضي،
        /// بلا إعادة تحميل ولا لمس البيانات. «تحديث» (F5) هو المسؤول عن إعادة تحميل كل شيء.
        /// </summary>
        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            _ClearAllFilters();
            txtFilterValue.Focus();
        }

        /// <summary>تفريغ أدوات التصفية وحدها بلا تطبيق ولا تحميل — تُستهلك من «مسح» و«تحديث».</summary>
        private void _ClearFilterInputs()
        {
            bool wasInitializing = _isInitializing;
            _isInitializing = true;
            try
            {
                txtFilterValue.Clear();

                if (cbFilterBy.Items.Count > 0) cbFilterBy.SelectedIndex = 0;
                if (cbStatus.Items.Count > 0) cbStatus.SelectedIndex = 0;

                chkShowInactive.Checked = false;
                chkOnlyOutOfStock.Checked = false;

                dtpFrom.Value = _defaultFrom;
                dtpTo.Value = _defaultTo;
            }
            finally
            {
                _isInitializing = wasInitializing;
            }
        }

        /// <summary>يمسح أدوات التصفية ثم يُعيد تطبيقها فقط — لا إعادة تحميل للبيانات (RULE-N148).</summary>
        private void _ClearAllFilters()
        {
            _ClearFilterInputs();
            _ApplyActiveFilter();
            txtFilterValue.Focus();
        }

        /// <summary>RULE-N43: زر «مسح التصفية» مفعّل فقط عند اختلاف أي أداة عن قيمتها الافتراضية.</summary>
        private void _UpdateClearFiltersState()
        {
            btnClearFilters.Enabled =
                !string.IsNullOrWhiteSpace(txtFilterValue.Text)
                || cbFilterBy.SelectedIndex > 0
                || cbStatus.SelectedIndex > 0
                || chkShowInactive.Checked
                || chkOnlyOutOfStock.Checked
                || dtpFrom.Value.Date != _defaultFrom
                || dtpTo.Value.Date != _defaultTo;
        }

        /// <summary>يلتقط القيم الافتراضية لأدوات التصفية — عند البناء وبعد اكتمال التحميل.</summary>
        private void _CaptureFilterDefaults()
        {
            _defaultFrom = dtpFrom.Value.Date;
            _defaultTo = dtpTo.Value.Date;
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

            _ApplyActiveFilter();
        }

        /// <summary>«هذا الشهر» = من أول الشهر الحالي حتى اليوم — تُستهلك عند التحميل فقط (RULE-N148).</summary>
        private void _SetDateRangeThisMonth()
        {
            var today = DateTime.Today;
            dtpFrom.Value = new DateTime(today.Year, today.Month, 1);
            dtpTo.Value = today;
        }

        // ── Drill-down ─────────────────────────────────────────────────────────────

        /// <summary>تصفية فورية على قيمة الخلية (النقر المزدوج أو عنصر قائمة السياق).</summary>
        private void _DrillDown(DataGridView grid, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string? value = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                MessageBox.Show("الخلية المحددة لا تحمل قيمة تُصفّى بها.", "تصفية",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            _ApplyActiveFilter();
        }

        private void miDrillDown_Click(object sender, EventArgs e)
        {
            var grid = ActiveGrid;
            var cell = grid.CurrentCell;

            if (cell is null)
            {
                MessageBox.Show("يرجى تحديد سطر في الشبكة أولاً.", "تصفية",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _DrillDown(grid, new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex));
        }

        private void miRefresh_Click(object sender, EventArgs e)
            => btnRefresh.PerformClick();

        private void miClearFilters_Click(object sender, EventArgs e)
            => _ClearAllFilters();

        // ── Form Events ────────────────────────────────────────────────────────────

        private void frmInventory_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _isInitializing = true;

            try
            {
                _BuildProductsTable();
                _BuildCountsTable();
                _BuildTransfersTable();

                _SetupProductsGrid();
                _SetupCountsGrid();
                _SetupTransfersGrid();

                _BindGrids();
                _LoadFilterChoices();
                _SetDateRangeThisMonth();
                _CaptureFilterDefaults();   // الافتراضي = ما فُتحت به الشاشة (RULE-N148)
                _ApplyDateFilterAvailability();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "خطأ أثناء تحميل بيانات المخزون التجريبية");
                MessageBox.Show("حدث خطأ أثناء تحميل بيانات المخزون.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isInitializing = false;
                Cursor = Cursors.Default;
            }

            _ApplyActiveFilter();
            _UpdateClearFiltersState();   // RULE-N148 — لقطة أولى بعد اكتمال التحميل
            txtFilterValue.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        // ── Toolbar ────────────────────────────────────────────────────────────────

        /// <summary>
        /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى الافتراضي (RULE-N148):
        /// أدوات التصفية ← الدلائل (المستودعات) ← شبكة الأرصدة ← الملخّص والتحديد.
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _isInitializing = true;

            try
            {
                _ClearFilterInputs();        // التصفية تعود للافتراضي

                _BuildProductsTable();       // إعادة تحميل البيانات (بدل الدلائل والشبكة)
                _BuildCountsTable();
                _BuildTransfersTable();

                _BindGrids();                // إعادة الربط ⇒ الشبكة تعود بلا تحديد
                _LoadFilterChoices();        // خيارات «تصفية بـ» و«الحالة» للتبويب النشط
                _ApplyDateFilterAvailability();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر تحديث بيانات المخزون");
                MessageBox.Show("تعذر تحديث بيانات المخزون. يرجى إعادة المحاولة.", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isInitializing = false;
                Cursor = Cursors.Default;
            }

            _ApplyActiveFilter();            // التصفية الافتراضية + الملخّص + حالة زر المسح
            ActiveGrid.ClearSelection();     // إعادة التحديد إلى الافتراضي
            txtFilterValue.Focus();
        }

        // ── Filter Events (instant — RULE-N59) ─────────────────────────────────────

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
            => _ApplyActiveFilter();

        /// <summary>Enter يطبّق التصفية فوراً (وهي مطبَّقة أصلاً عند كل كتابة).</summary>
        private void txtFilterValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None) return;

            _ApplyActiveFilter();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
            => _ApplyActiveFilter();

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
            => _ApplyActiveFilter();

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
            => _ApplyActiveFilter();

        private void dtpTo_ValueChanged(object sender, EventArgs e)
            => _ApplyActiveFilter();

        private void chkShowInactive_CheckedChanged(object sender, EventArgs e)
            => _ApplyActiveFilter();

        private void chkOnlyOutOfStock_CheckedChanged(object sender, EventArgs e)
            => _ApplyActiveFilter();

        private void btnRangeToday_Click(object sender, EventArgs e)
            => _SetDateRange(DateTime.Today, DateTime.Today);

        private void btnRangeMonth_Click(object sender, EventArgs e)
            => _SetDateRange(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);

        private void btnRange30_Click(object sender, EventArgs e)
            => _SetDateRange(DateTime.Today.AddDays(-29), DateTime.Today);

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;

            _LoadFilterChoices();
            _ApplyDateFilterAvailability();
            _ApplyActiveFilter();
        }

        // ── Grid Events ────────────────────────────────────────────────────────────

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            => _DrillDown(dgvProducts, e);

        private void dgvCounts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            => _DrillDown(dgvCounts, e);

        private void dgvTransfers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            => _DrillDown(dgvTransfers, e);

        private void dgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            var statusColumn = dgvProducts.Columns["StockStatus"];
            if (statusColumn == null || e.ColumnIndex != statusColumn.Index) return;

            Color? color = e.Value.ToString() switch
            {
                StockOut => (Color?)Color.Firebrick,
                StockLow => (Color?)Color.Goldenrod,
                StockAvailable => (Color?)Color.ForestGreen,
                _ => null,
            };

            if (color is null) return;

            e.CellStyle.BackColor = color.Value;
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.Font = new Font(dgvProducts.Font, FontStyle.Bold);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void dgvCounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => _ApplyStatusBadge(dgvCounts, e, _countStatusColors);

        private void dgvTransfers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => _ApplyStatusBadge(dgvTransfers, e, _transferStatusColors);
    }
}
