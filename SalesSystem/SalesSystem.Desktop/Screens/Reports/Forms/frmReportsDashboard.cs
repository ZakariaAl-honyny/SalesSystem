using SalesSystem.Desktop.Helpers;

namespace SalesSystem.Desktop.Screens.Reports.Forms
{
    public partial class frmReportsDashboard : Form
    {
        private readonly IServiceProvider _serviceProvider;

        // ── Mock Data (in-memory, Arabic, decimal — UI Shape, no backend) ──────────

        // KPI cards (ريال يمني)
        private static readonly decimal _totalSales = 1_250_000m;
        private static readonly decimal _totalPurchases = 875_500m;
        private static readonly decimal _stockValue = 2_100_000m;
        private static readonly decimal _receivables = 420_000m;

        // Report list — كل التقارير محفوظة هنا، والعمود Mvp يحدد ما يُعرض للمستخدم
        // MVP: التقارير الستة الأساسية عليها true، والباقي مخفي (HIDE, never delete)
        private static readonly (string Category, string Report, bool Mvp)[] _reportGroups =
        {
            // ── التقارير الأساسية المعروضة (MVP) ─────────────────────────────────
            ("المبيعات", "تقرير المبيعات", true),              // MVP: أساسي 1
            ("المشتريات", "تقرير المشتريات", true),            // MVP: أساسي 2
            ("المخزون", "تقرير المخزون الحالي", true),         // MVP: أساسي 3
            ("المبيعات", "تقرير الأرباح", true),               // MVP: أساسي 4
            ("المحاسبة", "كشف حساب عميل", true),               // MVP: أساسي 5
            ("المحاسبة", "كشف حساب مورد", true),               // MVP: أساسي 6

            // ── تقارير محفوظة لكنها مخفية عن المستخدم ────────────────────────────
            ("المبيعات", "مبيعات حسب الصنف", false),           // MVP: مخفي
            ("المبيعات", "مبيعات حسب العميل", false),          // MVP: مخفي
            ("المبيعات", "مبيعات حسب التصنيف", false),         // MVP: مخفي
            ("المبيعات", "مبيعات يومية", false),               // MVP: مخفي
            ("المبيعات", "أرباح العملاء", false),              // MVP: مخفي
            ("المشتريات", "المشتريات حسب المورد", false),      // MVP: مخفي
            ("المشتريات", "المشتريات حسب الصنف", false),       // MVP: مخفي
            ("المخزون", "حركة الأصناف", false),                // MVP: مخفي
            ("المخزون", "أرصدة المخزون", false),               // MVP: مخفي
            ("المخزون", "انتهاء الصلاحية", false),             // MVP: مخفي
            ("المخزون", "دفتر المخزون التفصيلي", false),       // MVP: مخفي
            ("المخزون", "حركات المخازن", false),               // MVP: مخفي
            ("المحاسبة", "ميزان المراجعة", false),             // MVP: مخفي
            ("المحاسبة", "كشف الحساب (أرصدة)", false),         // MVP: مخفي
            ("المحاسبة", "قائمة الدخل", false),                // MVP: مخفي
            ("المحاسبة", "الميزانية العمومية", false),         // MVP: مخفي
            ("المحاسبة", "دفتر الأستاذ العام", false),         // MVP: مخفي
            ("المحاسبة", "التدفق النقدي", false),              // MVP: مخفي
            ("المحاسبة", "ضريبة القيمة المضافة", false),       // MVP: مخفي
            ("المحاسبة", "الأعمار (Aging)", false),            // MVP: مخفي
            ("الخزينة", "حركة الصندوق", false),                // MVP: مخفي
            ("الخزينة", "Z-Report", false),                    // MVP: مخفي
            ("الخزينة", "الإغلاق اليومي", false),              // MVP: مخفي
            ("أخرى", "تقرير المرتجعات", false),                // MVP: مخفي
        };

        public frmReportsDashboard(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            InitializeComponent();
            KeyboardHelper.WireEnterAsTab(this);
        }

        // ── Data Loading (mock, in-memory) ─────────────────────────────────────────

        private void frmReportsDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                _LoadKpiCards();
                _LoadReportTree();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل البيانات: " + ex.Message,
                    "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _LoadKpiCards()
        {
            cardSalesValue.Text = _totalSales.ToString("N2");
            cardPurchasesValue.Text = _totalPurchases.ToString("N2");
            cardStockValue.Text = _stockValue.ToString("N2");
            cardReceivablesValue.Text = _receivables.ToString("N2");
        }

        private void _LoadReportTree()
        {
            tvReports.BeginUpdate();
            tvReports.Nodes.Clear();

            // MVP: تُعرض التقارير التي عليها Mvp = true فقط، مجمّعة حسب التصنيف
            foreach (var group in _reportGroups.Where(r => r.Mvp).GroupBy(r => r.Category))
            {
                var categoryNode = new TreeNode(group.Key)
                {
                    NodeFont = new Font("Tahoma", 10F, FontStyle.Bold)
                };

                foreach (var report in group)
                {
                    categoryNode.Nodes.Add(new TreeNode(report.Report));
                }

                tvReports.Nodes.Add(categoryNode);
            }

            tvReports.ExpandAll();
            tvReports.EndUpdate();
        }

        // ── Tree Events ────────────────────────────────────────────────────────────

        private void tvReports_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            if (e.Node.Parent == null)
            {
                lblPreviewPlaceholder.Text = "اختر تقريراً من القائمة لعرضه هنا";
                return;
            }

            lblPreviewPlaceholder.Text = "تقرير: " + e.Node.Text +
                "\n\n(معاينة أولية — سيتم عرض التقرير هنا في النسخة الكاملة)";
        }

        // ── Form Events ────────────────────────────────────────────────────────────

        private void btnClose_Click(object sender, EventArgs e)
            => Close();
    }
}