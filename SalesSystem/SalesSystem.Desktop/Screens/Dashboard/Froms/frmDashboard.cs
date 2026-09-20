using SalesSystem.DTO;
using SalesSystem.DTO.Common;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Messaging.Messages;
using SalesSystem.Desktop.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Dashboard;
using SalesSystem.Desktop.Helpers;
 
namespace SalesSystem.Desktop.Screens.Dashboard.Forms
{
    public partial class frmDashboard : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private IDisposable? _sub1;
        private IDisposable? _sub2;
        private IDisposable? _sub3;

        public frmDashboard(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        InitializeComponent();
        KeyboardHelper.WireEnterAsTab(this);
        ApplyRoleVisibility();
        ApplyWelcomeText();
        }

        // ── Role-Based Visibility ────────────────────────────────────────────────

        private void ApplyRoleVisibility()
        {
            var session = GetSessionService();
            if (session.Current?.Role == SalesSystem.Common.Enums.UserRole.Cashier)
            {
                cardPurchases.Visible = false;
                cardPayables.Visible = false;
            }
        }

        private void ApplyWelcomeText()
        {
            var session = GetSessionService();
            lblWelcome.Text = "\u0623\u0647\u0644\u0627\u064B " + (session.Current?.FullName ?? "\u0627\u0644\u0645\u0633\u062A\u062E\u062F\u0645");
        }

        private ISessionService GetSessionService()
        {
            return _serviceProvider.GetRequiredService<ISessionService>();
        }

        // ── Data Loading ─────────────────────────────────────────────────────────

        private async void _RefreshDashboard()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var result = await _serviceProvider.GetRequiredService<IDashboardApiService>().GetSummaryAsync();

                if (result.IsSuccess && result.Value != null)
                {
                    var s = result.Value;
                    cardSalesValue.Text = s.TotalSalesToday.ToString("N2");
                    cardPurchasesValue.Text = s.TotalPurchasesToday.ToString("N2");
                    cardCustomersValue.Text = s.ActiveCustomersCount.ToString();
                    cardLowStockValue.Text = s.LowStockItemsCount.ToString();
                    cardReceivablesValue.Text = s.TotalReceivables.ToString("N2");
                    cardPayablesValue.Text = s.TotalPayables.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                // Silently handle — dashboard refresh failure is non-critical.
                // The next event or manual refresh will retry.
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // ── Form Events ──────────────────────────────────────────────────────────

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            var eventBus = _serviceProvider.GetRequiredService<IEventBus>();
            _sub1 = eventBus.Subscribe<SaleInvoiceChangedMessage>(
                async _ => _RefreshDashboard());
            _sub2 = eventBus.Subscribe<PurchaseInvoiceChangedMessage>(
                async _ => _RefreshDashboard());
            _sub3 = eventBus.Subscribe<StockChangedMessage>(
                async _ => _RefreshDashboard());

            _RefreshDashboard();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ── Cleanup ─────────────────────────────────────────────────────────────

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sub1?.Dispose();
                _sub2?.Dispose();
                _sub3?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
