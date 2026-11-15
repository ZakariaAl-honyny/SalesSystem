using System.Data;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services.Interfaces;
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
    /// <b>البيانات من العقود لا من الشاشة (RULE-N167 / RULE-N173):</b> لا تُبنى في الشاشة أي
    /// مجموعة صفوف مُختلقة — الأرصدة من <see cref="IInventoryDesktopService.GetStockByWarehouseAsync"/>،
    /// ومحاضر الجرد من <see cref="IStocktakingDesktopService.GetAllAsync"/>، والتحويلات من
    /// <see cref="IWarehouseTransferDesktopService.GetAllAsync"/> — فيرى المالك نفس ما تحفظه
    /// الشاشات والعقود، ويختفي أي اسم/حالة لا وجود لها في بيانات التجربة.
    /// </para>
    /// <para>
    /// وضع التجربة (<see cref="AppSettings.Current"/> · <c>UseMockData</c>) هو البيت الوحيد لاكتشاف الوضع؛
    /// وفي الوضع الحقيقي تبقى الشبكات فارغة ويُعرض إشعار صريح في شريط الملخص (لا صفوف ولا أرقام مُختلقة).
    /// التصفية فورية (RULE-N59) بلا زر بحث، وبناء <c>DataView.RowFilter</c> يمرّ عبر
    /// <see cref="RowFilterHelper"/> على أعمدة معروفة فقط (RULE-N24/RULE-N60).
    /// </para>
    /// </summary>
    public partial class frmInventory : Form
    {
        /// <summary>RULE-N46: لون الحقل النشط الموحّد.</summary>
        private static Color FocusColor => ThemePalette.DefaultFor(ThemePalette.SelectionRowColor);

        /// <summary>
        /// وضع التجربة: البيت الواحد لاكتشاف الوضع هو <see cref="AppSettings.Current"/> (<c>UseMockData</c>)
        /// — نفس ما يستهلكه <c>frmMain</c>؛ لا آلية ثانية. القراءة من العقود تقع في وضع التجربة فقط.
        /// </summary>
        private static bool IsMockMode => AppSettings.Current.UseMockData;

        /// <summary>
        /// إشعار الوضع الحقيقي — يُعرض في شريط الملخص وسطور «لا توجد بيانات» القائمة في المصمّم
        /// (RULE-N134/N145: بلا عنصر واجهة جديد).
        /// </summary>
        private const string MockOnlyNotice =
            "لا توجد بيانات: هذه الشاشة تُعرض في وضع التجربة فقط حتى اعتماد الباك-إند";

        /// <summary>نص سطر «لا توجد بيانات» الأصلي في المصمّم (وضع التجربة فقط).</summary>
        private const string EmptyStateText = "لا توجد بيانات للعرض";

        /// <summary>قيمة عمود «السائق» حين لا يحمل عقد التحويل حقل سائق (لا يُختلق اسم).</summary>
        private const string NotAvailableValue = "—";

        // ── ثوابت الحالات (RULE-N60: تُقارن بثوابت داخل الكود، لا بنص المستخدم) ─────
        private const string StockOut = "نفد";
        private const string StockLow = "محدود";
        private const string StockAvailable = "متوفر";

        // حالات المستند (محضر جرد · سند تحويل): نفس أسماء الحالة التي يُعيدها العقد (StatusName)
        private const string DocumentDraft = "مسودة";
        private const string DocumentPosted = "مرحّل";
        private const string DocumentCancelled = "ملغي";

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

        // ── شارات الحالة: ألوان الثيم الدلالية الموحّدة (RULE-N172 — لا لون حرفي في الشاشة) ─────
        // قاموس واحد للحالتين (محضر جرد · سند تحويل): الحالات الثلاث نفسها بنفس النصوص،
        // فلا تُكرَّر نسخة ثانية تتباعد قيمها عند أول تعديل (RULE-N169).

        private static readonly Dictionary<string, Color> _documentStatusColors = new()
        {
            { DocumentDraft, GreenThemeHelper.StatusNeutralStrong },
            { DocumentPosted, GreenThemeHelper.StatusSuccessStrong },
            { DocumentCancelled, GreenThemeHelper.StatusDangerStrong },
        };

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

        // ── الخدمات (RULE-N04 — تُحل من الحاوية، ولا تُنشأ يدوياً) ───────────────────

        /// <summary>الأرصدة المخزنية لكل (صنف، مخزن) — أرصدة الأصناف.</summary>
        private IInventoryDesktopService InventoryService
            => _serviceProvider.GetRequiredService<IInventoryDesktopService>();

        /// <summary>دليل المخازن — لازم لطلب رصيد كل مخزن (لا يُخترع معرّف مخزن في الشاشة).</summary>
        private IWarehouseDesktopService WarehouseService
            => _serviceProvider.GetRequiredService<IWarehouseDesktopService>();

        /// <summary>دليل الأصناف — حد الطلب والباركود وحالة التنشيط (لا تُشتق في الشاشة).</summary>
        private IProductDesktopService ProductService
            => _serviceProvider.GetRequiredService<IProductDesktopService>();

        /// <summary>محاضر الجرد الفعلي.</summary>
        private IStocktakingDesktopService StocktakingService
            => _serviceProvider.GetRequiredService<IStocktakingDesktopService>();

        /// <summary>سندات التحويل المخزني.</summary>
        private IWarehouseTransferDesktopService TransferService
            => _serviceProvider.GetRequiredService<IWarehouseTransferDesktopService>();

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

        // ── تحميل البيانات من العقود ────────────────────────────────────────────────

        /// <summary>
        /// أرصدة الأصناف: رصيد كل (صنف، مخزن) من <see cref="IInventoryDesktopService"/> + ما يكمل الصف
        /// من دليل الأصناف (حد الطلب · الباركود · التنشيط). لا صف واحد يُبنى خارج هذين المصدرين.
        /// </summary>
        private async Task _LoadProductsAsync()
        {
            _productsTable = _CreateProductsTable();

            // الوضع الحقيقي: الشبكة تبقى فارغة — لا تُبنى أي صفوف مُختلقة (RULE-N167).
            if (!IsMockMode) return;

            var productsResult = await ProductService.GetAllAsync(includeInactive: true);
            if (!productsResult.IsSuccess || productsResult.Value is null)
            {
                Log.Error("تعذر تحميل دليل الأصناف لأرصدة المخزون: {Error}", productsResult.Error);
                clsDialogs.Error("تعذر تحميل أرصدة الأصناف. يرجى إعادة المحاولة.");
                return;
            }

            var productsById = productsResult.Value.ToDictionary(p => p.Id);

            var warehousesResult = await WarehouseService.GetAllAsync(includeInactive: true);
            if (!warehousesResult.IsSuccess || warehousesResult.Value is null)
            {
                Log.Error("تعذر تحميل دليل المخازن لأرصدة المخزون: {Error}", warehousesResult.Error);
                clsDialogs.Error("تعذر تحميل أرصدة المخازن. يرجى إعادة المحاولة.");
                return;
            }

            foreach (var warehouse in warehousesResult.Value)
            {
                var stockResult = await InventoryService.GetStockByWarehouseAsync(warehouse.Id);

                if (!stockResult.IsSuccess || stockResult.Value is null)
                {
                    // مخزن تعذّر رصيده لا يُسقِط بقية المخازن — يُسجّل ويُتخطّى بلا صفوف مُختلقة.
                    Log.Error("تعذر تحميل رصيد المخزن {WarehouseId}: {Error}", warehouse.Id, stockResult.Error);
                    continue;
                }

                foreach (var stock in stockResult.Value)
                {
                    productsById.TryGetValue(stock.ProductId, out var product);

                    decimal reorderLevel = product?.ReorderLevel ?? 0m;

                    string status = stock.Quantity <= 0m
                        ? StockOut
                        : stock.Quantity <= reorderLevel ? StockLow : StockAvailable;

                    _productsTable.Rows.Add(
                        // «الكود» = المعرّف النصي المسجَّل للصنف (لا يحمل العقد حقلاً منفصلاً للكود،
                        // والباركود هو معرّفه النصي) — كان العمود يُملأ بفراغ دائم فتبقى تصفية «الكود» بلا نتائج.
                        product?.Barcode ?? string.Empty,
                        product?.Barcode ?? string.Empty,
                        string.IsNullOrWhiteSpace(stock.ProductName) ? product?.Name ?? string.Empty : stock.ProductName,
                        stock.UnitName ?? string.Empty,
                        stock.WarehouseName ?? string.Empty,
                        stock.Quantity,
                        reorderLevel,
                        stock.AvgCost,
                        status,
                        product?.IsActive ?? true);
                }
            }
        }

        /// <summary>محاضر الجرد: رقم المستند · التاريخ · المخزن · لجنة الجرد · الحالة — من عقد محاضر الجرد.</summary>
        private async Task _LoadCountsAsync()
        {
            _countsTable = _CreateCountsTable();

            if (!IsMockMode) return;

            var result = await StocktakingService.GetAllAsync(includeCancelled: true);
            if (!result.IsSuccess || result.Value is null)
            {
                Log.Error("تعذر تحميل محاضر الجرد: {Error}", result.Error);
                clsDialogs.Error("تعذر تحميل محاضر الجرد. يرجى إعادة المحاولة.");
                return;
            }

            foreach (var document in result.Value)
            {
                _countsTable.Rows.Add(
                    document.DocumentNo,
                    document.Date,
                    document.WarehouseName,
                    _CommitteeOf(document.CommitteeHead, document.CommitteeMember),
                    document.StatusName);
            }
        }

        /// <summary>سندات التحويل: رقم السند · التاريخ · من/إلى مخزن · السائق · الحالة — من عقد التحويل.</summary>
        private async Task _LoadTransfersAsync()
        {
            _transfersTable = _CreateTransfersTable();

            if (!IsMockMode) return;

            var result = await TransferService.GetAllAsync(includeCancelled: true);
            if (!result.IsSuccess || result.Value is null)
            {
                Log.Error("تعذر تحميل سندات التحويل المخزني: {Error}", result.Error);
                clsDialogs.Error("تعذر تحميل سندات التحويل المخزني. يرجى إعادة المحاولة.");
                return;
            }

            foreach (var document in result.Value)
            {
                _transfersTable.Rows.Add(
                    document.DocumentNo,
                    document.Date,
                    document.FromWarehouseName,
                    document.ToWarehouseName,
                    NotAvailableValue,   // عقد التحويل لا يحمل حقل سائق — لا يُختلق اسم
                    document.StatusName);
            }
        }

        /// <summary>نص لجنة الجرد من حقلي العقد (رئيس · عضو) — عرض فقط، لا اختلاق أعضاء.</summary>
        private static string _CommitteeOf(string? head, string? member)
        {
            var parts = new List<string>(2);

            if (!string.IsNullOrWhiteSpace(head))
                parts.Add($"رئيس: {head.Trim()}");

            if (!string.IsNullOrWhiteSpace(member))
                parts.Add($"عضو: {member.Trim()}");

            return parts.Count == 0 ? NotAvailableValue : string.Join(" / ", parts);
        }

        private static DataTable _CreateProductsTable()
        {
            var table = new DataTable();
            table.Columns.Add("Code", typeof(string));
            table.Columns.Add("Barcode", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Unit", typeof(string));
            table.Columns.Add("WarehouseName", typeof(string));
            table.Columns.Add("Stock", typeof(decimal));
            table.Columns.Add("ReorderLevel", typeof(decimal));
            table.Columns.Add("Cost", typeof(decimal));
            table.Columns.Add("StockStatus", typeof(string));
            table.Columns.Add("IsActive", typeof(bool));
            return table;
        }

        private static DataTable _CreateCountsTable()
        {
            var table = new DataTable();
            table.Columns.Add("CountNo", typeof(string));
            table.Columns.Add("CountDate", typeof(DateTime));
            table.Columns.Add("WarehouseName", typeof(string));
            table.Columns.Add("Committee", typeof(string));
            table.Columns.Add("Status", typeof(string));
            return table;
        }

        private static DataTable _CreateTransfersTable()
        {
            var table = new DataTable();
            table.Columns.Add("TransferNo", typeof(string));
            table.Columns.Add("TransferDate", typeof(DateTime));
            table.Columns.Add("FromWarehouse", typeof(string));
            table.Columns.Add("ToWarehouse", typeof(string));
            table.Columns.Add("DriverName", typeof(string));
            table.Columns.Add("Status", typeof(string));
            return table;
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
                new { Name = "Stock",         Header = "الرصيد",      Fill = 7,  Min = 70, Format = (string?)clsFormat.QuantityPattern },
                new { Name = "ReorderLevel",  Header = "حد الطلب",    Fill = 7,  Min = 70, Format = (string?)clsFormat.QuantityPattern },
                new { Name = "Cost",          Header = "التكلفة",     Fill = 8,  Min = 80, Format = (string?)clsFormat.MoneyPattern },
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
                new { Name = "CountDate",     Header = "التاريخ",     Fill = 10, Min = 100, Format = (string?)clsFormat.Get(clsFormat.DateShortPattern) },
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
                new { Name = "TransferDate",  Header = "التاريخ",    Fill = 10, Min = 100, Format = (string?)clsFormat.Get(clsFormat.DateShortPattern) },
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

            string search = txtFilterValue.Text.Trim();

            var parts = new List<string>
            {
                RowFilterHelper.Text(search, cbFilterBy.SelectedIndex switch
                {
                    1 => new[] { "Name" },
                    2 => new[] { "Code" },
                    3 => new[] { "Barcode" },
                    _ => new[] { "Name", "Code", "Barcode" },
                }),
            };

            if (!chkShowInactive.Checked)
                parts.Add("IsActive = true");

            if (chkOnlyOutOfStock.Checked)
                parts.Add("Stock <= 0");

            parts.Add(RowFilterHelper.EqualsText("StockStatus", _SelectedProductsStatus()));

            _SetRowFilter(_productsView, parts);
        }

        private void _ApplyCountsFilter()
        {
            if (_countsView.Table == null) return;

            var parts = new List<string>
            {
                RowFilterHelper.Text(txtFilterValue.Text.Trim(), cbFilterBy.SelectedIndex switch
                {
                    1 => new[] { "CountNo" },
                    2 => new[] { "WarehouseName" },
                    3 => new[] { "Committee" },
                    _ => new[] { "CountNo", "WarehouseName", "Committee" },
                }),
                RowFilterHelper.EqualsText("Status", _SelectedDocumentStatus()),
                RowFilterHelper.DateRange("CountDate", dtpFrom.Value, dtpTo.Value),
            };

            _SetRowFilter(_countsView, parts);
        }

        private void _ApplyTransfersFilter()
        {
            if (_transfersView.Table == null) return;

            var parts = new List<string>
            {
                RowFilterHelper.Text(txtFilterValue.Text.Trim(), cbFilterBy.SelectedIndex switch
                {
                    1 => new[] { "TransferNo" },
                    2 => new[] { "FromWarehouse" },
                    3 => new[] { "ToWarehouse" },
                    4 => new[] { "DriverName" },
                    _ => new[] { "TransferNo", "FromWarehouse", "ToWarehouse", "DriverName" },
                }),
                RowFilterHelper.EqualsText("Status", _SelectedDocumentStatus()),
                RowFilterHelper.DateRange("TransferDate", dtpFrom.Value, dtpTo.Value),
            };

            _SetRowFilter(_transfersView, parts);
        }

        /// <summary>إن فشل بناء الصيغة تُترك التصفية السابقة كما هي (لا سقوط إلى «بلا تصفية»).</summary>
        private static void _SetRowFilter(DataView view, List<string> parts)
        {
            try
            {
                view.RowFilter = RowFilterHelper.Build(parts);
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

        /// <summary>حالة المستند (محضر جرد أو سند تحويل) — 1 مسودة · 2 مرحّل · 3 ملغي كما يعيدها العقد.</summary>
        private string? _SelectedDocumentStatus() => cbStatus.SelectedIndex switch
        {
            1 => DocumentDraft,
            2 => DocumentPosted,
            3 => DocumentCancelled,
            _ => null
        };

        // ── Summary (RULE-N61) ─────────────────────────────────────────────────────

        private void _UpdateSummary()
        {
            // الوضع الحقيقي: لا بيانات مُختلقة ⇒ إشعار صريح في شريط الملخص القائم (RULE-N134/N145).
            if (!IsMockMode)
            {
                lblSummary.Text = MockOnlyNotice;
                _UpdateEmptyLabels();
                return;
            }

            switch (tabMain.SelectedIndex)
            {
                case 1:
                    lblSummary.Text = "محاضر الجرد — " + _Summarize(
                        _countsView, "Status",
                        (DocumentDraft, "مسودة"), (DocumentPosted, "مرحّل"), (DocumentCancelled, "ملغي"));
                    break;
                case 2:
                    lblSummary.Text = "التحويلات المخزنية — " + _Summarize(
                        _transfersView, "Status",
                        (DocumentDraft, "مسودة"), (DocumentPosted, "مرحّل"), (DocumentCancelled, "ملغي"));
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
            builder.Append($"المعروض: {clsFormat.Number(view.Count)}");

            for (int i = 0; i < statuses.Length; i++)
                builder.Append($" · {statuses[i].Caption}: {clsFormat.Number(counts[i])}");

            return builder.ToString();
        }

        /// <summary>ملخص الأرصدة: النافد/المحدود/المتوفر + قيمة المخزون (RULE-N179: من <c>clsFormat</c>).</summary>
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

            return $"المعروض: {clsFormat.Number(_productsView.Count)} من {clsFormat.Number(_productsTable.Rows.Count)}"
                 + $" · نفد: {clsFormat.Number(outOfStock)} · محدود: {clsFormat.Number(low)} · متوفر: {clsFormat.Number(available)}"
                 + $" · قيمة المخزون: {clsFormat.Money(stockValue)}";
        }

        /// <summary>
        /// سطر «لا توجد بيانات» لكل تبويب (RULE-N22/N23) — نصه يُبدَّل في الوضع الحقيقي إلى إشعار
        /// صريح يمنع قراءة الفراغ كـ«صفر رصيد حقيقي» (RULE-N167)، والعنصر نفسه قائم في المصمّم.
        /// </summary>
        private void _UpdateEmptyLabels()
        {
            string text = IsMockMode ? EmptyStateText : MockOnlyNotice;

            _ToggleEmpty(lblEmptyProducts, _productsView.Count, text);
            _ToggleEmpty(lblEmptyCounts, _countsView.Count, text);
            _ToggleEmpty(lblEmptyTransfers, _transfersView.Count, text);
        }

        private static void _ToggleEmpty(Label label, int count, string text)
        {
            label.Text = text;
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
                        cbStatus.Items.AddRange(new object[] { "الكل", DocumentDraft, DocumentPosted, DocumentCancelled });
                        break;

                    case 2:
                        cbFilterBy.Items.AddRange(new object[]
                        {
                            "الكل (رقم/مستودعات/سائق)", "رقم السند", "من مستودع", "إلى مستودع", "السائق"
                        });
                        cbStatus.Items.AddRange(new object[] { "الكل", DocumentDraft, DocumentPosted, DocumentCancelled });
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

            _ApplyActiveFilter();
        }

        private void miDrillDown_Click(object sender, EventArgs e)
        {
            var grid = ActiveGrid;
            var cell = grid.CurrentCell;

            if (cell is null)
            {
                clsDialogs.Warning("يرجى تحديد سطر في الشبكة أولاً.");
                return;
            }

            _DrillDown(grid, new DataGridViewCellEventArgs(cell.ColumnIndex, cell.RowIndex));
        }

        private void miRefresh_Click(object sender, EventArgs e)
            => btnRefresh.PerformClick();

        private void miClearFilters_Click(object sender, EventArgs e)
            => _ClearAllFilters();

        // ── Form Events ────────────────────────────────────────────────────────────

        /// <summary>RULE-N25/N26: معالج async void مع try/catch واستعادة المؤشر في finally.</summary>
        private async void frmInventory_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _isInitializing = true;

            try
            {
                await _LoadAllDataAsync();

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
                Log.Error(ex, "خطأ أثناء تحميل بيانات المخزون من العقود");
                clsDialogs.Error("حدث خطأ أثناء تحميل بيانات المخزون.");
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

        /// <summary>قراءة التبويبات الثلاثة من عقودها — مكان واحد يُستهلك من التحميل والتحديث.</summary>
        private async Task _LoadAllDataAsync()
        {
            await _LoadProductsAsync();
            await _LoadCountsAsync();
            await _LoadTransfersAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
            => Close();

        // ── Toolbar ────────────────────────────────────────────────────────────────

        /// <summary>
        /// «تحديث» (F5) — إعادة تحميل **كل** بيانات الشاشة وإرجاعها إلى الافتراضي (RULE-N148):
        /// أدوات التصفية ← الشبكات (من العقود) ← الملخّص والتحديد.
        /// </summary>
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            _isInitializing = true;

            try
            {
                _ClearFilterInputs();        // التصفية تعود للافتراضي

                await _LoadAllDataAsync();   // إعادة قراءة البيانات من العقود

                _BindGrids();                // إعادة الربط ⇒ الشبكة تعود بلا تحديد
                _LoadFilterChoices();        // خيارات «تصفية بـ» و«الحالة» للتبويب النشط
                _ApplyDateFilterAvailability();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "تعذر تحديث بيانات المخزون");
                clsDialogs.Error("تعذر تحديث بيانات المخزون. يرجى إعادة المحاولة.");
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
                StockOut => (Color?)GreenThemeHelper.StatusDangerStrong,
                StockLow => (Color?)GreenThemeHelper.StatusWarningStrong,
                StockAvailable => (Color?)GreenThemeHelper.StatusSuccessStrong,
                _ => null,
            };

            if (color is null) return;

            e.CellStyle.BackColor = color.Value;
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.Font = new Font(dgvProducts.Font, FontStyle.Bold);
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void dgvCounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => _ApplyStatusBadge(dgvCounts, e, _documentStatusColors);

        private void dgvTransfers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => _ApplyStatusBadge(dgvTransfers, e, _documentStatusColors);
    }
}
