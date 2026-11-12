using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.Forms;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Services.Direct;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Services.Mock;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.People;
using SalesSystem.Desktop.People.Controls;
using SalesSystem.Desktop.People.Forms;
using SalesSystem.Desktop.Suppliers;
using SalesSystem.Desktop.Printing;
using SalesSystem.Desktop.Printing.Core;
using SalesSystem.Desktop.CustomerReceipts.Forms;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.Desktop.Screens.Units.Forms;
using SalesSystem.Desktop.Screens.Sales.Forms;
using SalesSystem.Desktop.Screens.POS.Forms;
using SalesSystem.Desktop.Screens.Employees.Forms;
using SalesSystem.Desktop.Screens.Products.Forms;
using SalesSystem.Desktop.Screens.Returns.Forms;
using SalesSystem.Desktop.Screens.Purchases.Forms;
using SalesSystem.Desktop.Screens.Customers.Forms;
using SalesSystem.Desktop.Screens.Categories.Forms;
using SalesSystem.Desktop.Screens.Payments.Forms;
using SalesSystem.Desktop.Screens.Expenses.Forms;
using SalesSystem.Desktop.Screens.CashBoxes.Forms;
using SalesSystem.Desktop.Screens.Warehouses.Forms;
using SalesSystem.Desktop.Screens.Inventory.Forms;
using SalesSystem.Desktop.Suppliers.Forms;
using SalesSystem.Desktop.Screens;
using SalesSystem.Desktop.Screens.Dashboard.Forms;
using SalesSystem.Desktop.Screens.Reports.Forms;
using Microsoft.EntityFrameworkCore;
using SalesSystem.DTO.Common;
using SalesSystem.DataAccess;
using SalesSystem.DataAccess.Services;
using SalesSystem.Business.Interfaces;
using SalesSystem.Business.Accounting.Services;
using BizInterfaces = SalesSystem.Business.Interfaces.Services;
using BizServices = SalesSystem.Business.Services;

namespace SalesSystem.Desktop;

static class Program
{
    /// <summary>
    /// Global service provider for resolving dependencies from non-DI-aware forms.
    /// </summary>
    public static IServiceProvider? ServiceProvider { get; private set; }
    [STAThread]
    static void Main()
    {
        // 1. Configure Serilog for AI-friendly error logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File("Logs/AI_Error_Log_.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "=== {Timestamp:yyyy-MM-dd HH:mm:ss} ==={NewLine}Message: {Message}{NewLine}Exception: {Exception}{NewLine}--------------------------------------------------{NewLine}")
            .CreateLogger();

        try
        {
            ApplicationConfiguration.Initialize();

            // 2. Global Exception Handlers
            Application.ThreadException += GlobalUIExceptionHandler;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += GlobalBackgroundExceptionHandler;

            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // App Settings (Mock Mode Toggle)
                    var appSettings = context.Configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();
                    AppSettings.Current = appSettings;
                    services.AddSingleton(appSettings);

                    // --- I*ApiService implementations (2-Tier) ---
                    // Mock-only by design (no BLL backend yet) — Employee module is intentionally kept:
                    services.AddTransient<IEmployeeApiService, MockEmployeeApiService>();

                    // Interfaces with NO mock — always Direct:
                    services.AddTransient<IAuthApiService, DirectAuthService>();
                    services.AddTransient<IBackupApiService, DirectBackupService>();
                    services.AddTransient<IBranchApiService, DirectBranchApiService>();
                    services.AddTransient<IFinancialReportApiService, DirectFinancialReportService>();
                    services.AddTransient<IPersonApiService, DirectPersonService>();
                    services.AddTransient<IReportApiService, DirectReportService>();
                    
                    // Mock / Real service toggle (services that have both implementations)
                    if (appSettings.UseMockData)
                    {
                        services.AddTransient<IAccountApiService, MockAccountApiService>();
                        services.AddTransient<ICashBoxApiService, MockCashBoxApiService>();
                        services.AddTransient<ICashBoxReportApiService, MockCashBoxReportApiService>();
                        services.AddTransient<ICategoryApiService, MockCategoryApiService>();
                        services.AddTransient<ICustomerApiService, MockCustomerApiService>();
                        services.AddTransient<ICustomerReceiptApiService, MockCustomerReceiptApiService>();
                        services.AddTransient<IDashboardApiService, MockDashboardApiService>();
                        services.AddTransient<IExpenseApiService, MockExpenseApiService>();
                        services.AddTransient<IInventoryAdjustmentApiService, MockInventoryAdjustmentApiService>();
                        services.AddTransient<IInventoryApiService, MockInventoryApiService>();
                        services.AddTransient<IInventoryBatchApiService, MockInventoryBatchApiService>();
                        services.AddTransient<IJournalEntryApiService, MockJournalEntryApiService>();
                        services.AddTransient<IProductApiService, MockProductApiService>();
                        services.AddTransient<IPurchaseInvoiceApiService, MockPurchaseInvoiceApiService>();
                        services.AddTransient<IPurchaseReportApiService, MockPurchaseReportApiService>();
                        services.AddTransient<IPurchaseReturnApiService, MockPurchaseReturnApiService>();
                        services.AddTransient<ISalesInvoiceApiService, MockSalesInvoiceApiService>();
                        services.AddTransient<ISalesReportApiService, MockSalesReportApiService>();
                        services.AddTransient<ISalesReturnApiService, MockSalesReturnApiService>();
                        services.AddTransient<ISettingsApiService, MockSettingsApiService>();
                        services.AddTransient<ISupplierApiService, MockSupplierApiService>();
                        services.AddTransient<ISupplierPaymentApiService, MockSupplierPaymentApiService>();
                        services.AddTransient<IUnitApiService, MockUnitApiService>();
                        services.AddTransient<IUserApiService, MockUserApiService>();
                        services.AddTransient<IWarehouseApiService, MockWarehouseApiService>();
                        services.AddScoped<BizInterfaces.IAuthService, MockAuthService>();
                    }
                    else
                    {
                        services.AddTransient<IAccountApiService, DirectAccountService>();
                        services.AddTransient<ICashBoxApiService, DirectCashBoxService>();
                        services.AddTransient<ICashBoxReportApiService, DirectCashBoxReportService>();
                        services.AddTransient<ICategoryApiService, DirectCategoryService>();
                        services.AddTransient<ICustomerApiService, DirectCustomerService>();
                        services.AddTransient<ICustomerReceiptApiService, DirectCustomerReceiptService>();
                        services.AddTransient<IDashboardApiService, DirectDashboardService>();
                        services.AddTransient<IExpenseApiService, DirectExpenseService>();
                        services.AddTransient<IInventoryAdjustmentApiService, DirectInventoryAdjustmentService>();
                        services.AddTransient<IInventoryApiService, DirectInventoryService>();
                        services.AddTransient<IInventoryBatchApiService, DirectInventoryBatchService>();
                        services.AddTransient<IJournalEntryApiService, DirectJournalEntryService>();
                        services.AddTransient<IProductApiService, DirectProductService>();
                        services.AddTransient<IPurchaseInvoiceApiService, DirectPurchaseInvoiceService>();
                        services.AddTransient<IPurchaseReportApiService, DirectPurchaseReportService>();
                        services.AddTransient<IPurchaseReturnApiService, DirectPurchaseReturnApiService>();
                        services.AddTransient<ISalesInvoiceApiService, DirectSalesInvoiceService>();
                        services.AddTransient<ISalesReportApiService, DirectSalesReportService>();
                        services.AddTransient<ISalesReturnApiService, DirectSalesReturnApiService>();
                        services.AddTransient<ISettingsApiService, DirectSettingsService>();
                        services.AddTransient<ISupplierApiService, DirectSupplierService>();
                        services.AddTransient<ISupplierPaymentApiService, DirectSupplierPaymentService>();
                        services.AddTransient<IUnitApiService, DirectUnitService>();
                        services.AddTransient<IUserApiService, DirectUserService>();
                        services.AddTransient<IWarehouseApiService, DirectWarehouseService>();
                        services.AddScoped<BizInterfaces.IAuthService, BizServices.AuthService>();
                    }

                    // DataAccess (2-Tier Direct)
                    services.AddDbContext<SalesDbContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddScoped<IUnitOfWork, UnitOfWork>();

                    // Startup initializer — applies migrations + seeds the database ONCE at startup, real mode only
                    services.AddScoped<SalesSystem.Business.Infrastructure.IApplicationInitializer,
                                       SalesSystem.DataAccess.Initialization.DatabaseInitializer>();

                    // Infrastructure Services (implemented in DataAccess)
                    services.AddScoped<BizInterfaces.IBackupService, BackupService>();
                    services.AddScoped<BizInterfaces.IConnectionStringProtector, ConnectionStringProtector>();
                    services.AddSingleton<BizInterfaces.ILocalImageStorageService, LocalImageStorageService>();
                    services.AddScoped<BizInterfaces.ILogService, LogService>();
                    services.AddScoped<BizInterfaces.IReportExportService, ReportExportService>();

                    // Accounting Services
                    services.AddScoped<BizInterfaces.IAccountingIntegrationService, AccountingIntegrationService>();
                    services.AddScoped<BizInterfaces.IJournalEntryNumberGenerator, JournalEntryNumberGenerator>();
                    services.AddScoped<BizInterfaces.IJournalEntryService, JournalEntryService>();
                    services.AddScoped<BizInterfaces.ISystemAccountService, SystemAccountService>();

                    // Business Services
                    services.AddScoped<BizInterfaces.IAccountCodeGeneratorService, BizServices.AccountCodeGeneratorService>();
                    services.AddScoped<BizInterfaces.IAccountLinkService, BizServices.AccountLinkService>();
                    services.AddScoped<BizInterfaces.IAccountService, BizServices.AccountService>();
                    services.AddScoped<BizInterfaces.IAuditLogService, BizServices.AuditLogService>();
                    services.AddScoped<BizInterfaces.IBarcodeLookupService, BizServices.BarcodeLookupService>();
                    services.AddScoped<BizInterfaces.IBranchService, BizServices.BranchService>();
                    services.AddScoped<BizInterfaces.ICashBoxReportService, BizServices.CashBoxReportService>();
                    services.AddScoped<BizInterfaces.ICashBoxService, BizServices.CashBoxService>();
                    services.AddScoped<BizInterfaces.ICompanySettingsService, BizServices.CompanySettingsService>();
                    services.AddScoped<BizInterfaces.ICustomerReceiptService, BizServices.CustomerReceiptService>();
                    services.AddScoped<BizInterfaces.ICustomerService, BizServices.CustomerService>();
                    services.AddScoped<BizInterfaces.IDocumentSequenceService, BizServices.DocumentSequenceService>();
                    services.AddScoped<BizInterfaces.IExpenseService, BizServices.ExpenseService>();
                    services.AddScoped<BizInterfaces.IFifoAllocationService, BizServices.FifoAllocationService>();
                    services.AddScoped<BizInterfaces.IFinancialReportService, BizServices.FinancialReportService>();
                    services.AddScoped<BizInterfaces.IInventoryAdjustmentService, BizServices.InventoryAdjustmentService>();
                    services.AddScoped<BizInterfaces.IInventoryBatchService, BizServices.InventoryBatchService>();
                    services.AddScoped<BizInterfaces.IInventoryService, BizServices.InventoryService>();
                    services.AddScoped<BizInterfaces.IPaymentVoucherService, BizServices.PaymentVoucherService>();
                    services.AddScoped<BizInterfaces.IPersonService, BizServices.PersonService>();
                    services.AddScoped<BizInterfaces.IProductCategoryService, BizServices.ProductCategoryService>();
                    services.AddScoped<BizInterfaces.IProductCostService, BizServices.ProductCostService>();
                    services.AddScoped<BizInterfaces.IProductImportService, BizServices.ProductImportService>();
                    services.AddScoped<BizServices.IProductPriceService, BizServices.ProductPriceService>();
                    services.AddScoped<BizInterfaces.IProductService, BizServices.ProductService>();
                    services.AddScoped<BizInterfaces.IProductUnitService, BizServices.ProductUnitService>();
                    services.AddScoped<BizInterfaces.IPurchaseReportService, BizServices.PurchaseReportService>();
                    services.AddScoped<BizInterfaces.IPurchaseReturnService, BizServices.PurchaseReturnService>();
                    services.AddScoped<BizInterfaces.IPurchaseService, BizServices.PurchaseService>();
                    services.AddScoped<BizInterfaces.IReceiptVoucherService, BizServices.ReceiptVoucherService>();
                    services.AddScoped<BizInterfaces.IReportService, BizServices.ReportService>();
                    services.AddScoped<BizInterfaces.ISalesReportService, BizServices.SalesReportService>();
                    services.AddScoped<BizInterfaces.ISalesReturnService, BizServices.SalesReturnService>();
                    services.AddScoped<BizInterfaces.ISalesService, BizServices.SalesService>();
                    services.AddScoped<BizInterfaces.IStoreSettingsService, BizServices.StoreSettingsService>();
                    services.AddScoped<BizInterfaces.ISupplierPaymentApplicationService, BizServices.SupplierPaymentApplicationService>();
                    services.AddScoped<BizInterfaces.ISupplierPaymentService, BizServices.SupplierPaymentService>();
                    services.AddScoped<BizInterfaces.ISupplierService, BizServices.SupplierService>();
                    services.AddScoped<BizInterfaces.IUnitService, BizServices.UnitService>();
                    services.AddScoped<BizInterfaces.IUpdateProductPricingService, BizServices.UpdateProductPricingService>();
                    services.AddScoped<BizInterfaces.IUserReportService, BizServices.UserReportService>();
                    services.AddScoped<BizInterfaces.IUserService, BizServices.UserService>();
                    services.AddScoped<BizInterfaces.IWarehouseService, BizServices.WarehouseService>();

                    // Core Infrastructure Services
                    services.AddSingleton<IEventBus, EventBus>();
                    services.AddSingleton<ISessionService, SessionService>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<INotificationService, NotificationService>();
                    services.AddSingleton<IDialogService, DialogService>();
                    
                    // Printing Services
                    services.AddTransient<IInvoicePrinter, InvoicePrinter>();
                    services.AddTransient<IReceiptPrinter, ReceiptPrinter>();

                    // UI Components - Essential Forms
                    services.AddTransient<frmLogin>();
                    services.AddTransient<frmMain>();
                    // MVP: مخفي — replaced by Master form
                    // services.AddTransient<frmAddUpdateCategory>();
                    // services.AddTransient<frmAddUpdateUnit>();
                    services.AddTransient<frmAddUpdateWarehouse>();
                    // MVP: مخفي — replaced by Master form
                    // services.AddTransient<frmAddUpdateCustomer>();
                    // services.AddTransient<frmAddUpdateSupplier>();
                    // services.AddTransient<frmAddUpdateProduct>();
                    services.AddTransient<frmAddUpdateSalesInvoice>();
                    services.AddTransient<frmPurchaseInvoice>();
                    services.AddTransient<frmSalesReturn>();
                    services.AddTransient<frmPurchaseReturn>();
                    services.AddTransient<frmListPeople>();
                    services.AddTransient<frmAddUpdatePerson>();
                    services.AddTransient<frmShowPersonInfo>();
                    services.AddTransient<frmFindPerson>();
                    services.AddTransient<frmListReturns>();
                    // MVP: مخفي — replaced by Master form
                    // services.AddTransient<frmListWarehouses>();
                    services.AddTransient<frmWarehouseMaster>();
                    // MVP: مخفي — replaced by Master form
                    // services.AddTransient<frmListUnits>();
                    services.AddTransient<frmListSales>();
                    services.AddTransient<frmListPurchases>();
                    
                    // Module Controls — NEW Forms
                    services.AddTransient<frmDashboard>();
                    services.AddTransient<SalesSystem.Desktop.Screens.Dashboard.Controls.ctrlDashboard>(); // RULE-N67
                    services.AddTransient<frmProductMaster>();
                    // Master forms (navigation targets from frmMain)
                    services.AddTransient<frmCategoryMaster>();
                    services.AddTransient<frmUnitMaster>();
                    services.AddTransient<frmCustomerMaster>();
                    services.AddTransient<frmSupplierMaster>();
                    // MVP: مخفي — replaced by Master form
                    // services.AddTransient<frmListProducts>();
                    // services.AddTransient<frmListCustomers>();
                    // services.AddTransient<frmListSuppliers>();
                    services.AddTransient<frmInventoryList>();
                    services.AddTransient<frmListPayments>();
                    // services.AddTransient<frmUpdateCheck>(); // V2: update-check removed
                    // reports (31 screens — RULE-N70/N71) — one screen = one report
                    services.AddTransient<frmSalesReport>();
                    services.AddTransient<frmPurchasesReport>();
                    services.AddTransient<frmInventoryReport>();
                    services.AddTransient<frmCustomerBalanceReport>();
                    services.AddTransient<frmSupplierBalanceReport>();
                    services.AddTransient<frmProductMovementReport>();
                    services.AddTransient<frmLowStockReport>();
                    services.AddTransient<frmDailySalesReport>();
                    services.AddTransient<frmSalesByCustomerReport>();
                    services.AddTransient<frmSalesByProductReport>();
                    services.AddTransient<frmSalesByCategoryReport>();
                    services.AddTransient<frmProfitByCustomerReport>();
                    services.AddTransient<frmProductProfitabilityReport>();
                    services.AddTransient<frmReturnsReport>();
                    services.AddTransient<frmPurchasesBySupplierReport>();
                    services.AddTransient<frmPurchasesByProductReport>();
                    services.AddTransient<frmStockBalanceReport>();
                    services.AddTransient<frmWarehouseMovementReport>();
                    services.AddTransient<frmDetailedStockLedgerReport>();
                    services.AddTransient<frmExpiredProductsReport>();
                    services.AddTransient<frmTrialBalanceReport>();
                    services.AddTransient<frmGeneralLedgerReport>();
                    services.AddTransient<frmIncomeStatementReport>();
                    services.AddTransient<frmBalanceSheetReport>();
                    services.AddTransient<frmCashFlowReport>();
                    services.AddTransient<frmWorkingCapitalReport>();
                    services.AddTransient<frmVatReport>();
                    services.AddTransient<frmAccountStatementReport>();
                    services.AddTransient<frmAgingReport>();
                    services.AddTransient<frmCashBoxSummaryReport>();
                    services.AddTransient<frmDailyClosureReport>();
                    // Users/Admin Forms
                    services.AddTransient<frmPasswordChange>();
                    services.AddTransient<frmBackup>();
                    // Master forms — replace the removed frmList* / frmAddUpdate* screens
                    services.AddTransient<frmUserMaster>();
                    services.AddTransient<SalesSystem.Desktop.Screens.Accounting.Forms.frmAccounts>();
                    services.AddTransient<SalesSystem.Desktop.Screens.Accounting.Forms.frmJournalEntries>();

                    // Inventory Module - New Forms
                    services.AddTransient<frmListInventoryAdjustments>();
                    services.AddTransient<frmAddUpdateInventoryAdjustment>();
                    services.AddTransient<frmListInventoryTransactions>();
                    services.AddTransient<frmListInventoryBatches>();
                    services.AddTransient<frmOpeningStock>();

                    // CashBoxes Module
                    // MVP: مخفي — replaced by Master form
                    // services.AddTransient<frmListCashBoxes>();
                    services.AddTransient<frmCashBoxMaster>();
                    services.AddTransient<frmAddUpdateCashBox>();
                    services.AddTransient<frmCashBoxTransactions>();

                    // Expenses Module
                    services.AddTransient<frmListExpenses>();
                    services.AddTransient<frmAddUpdateExpense>();

                    // CustomerReceipts Module
                    services.AddTransient<frmListCustomerReceipts>();
                    services.AddTransient<frmAddUpdateCustomerReceipt>();

                    // PaymentVouchers Module
                    // (removed with the hidden V2 payment-voucher screens)

                    // Supplier Payments Module
                    services.AddTransient<frmListSupplierPayments>();
                    services.AddTransient<frmAddUpdateSupplierPayment>();

                    // Payment Dialogs
                    services.AddTransient<frmSupplierPaymentDialog>();

                    // Product Management Forms
                    services.AddTransient<frmProductUnitMaster>();
                    // Fully-qualified: two frmProductSelection types exist.
                    // Old API-backed picker (Products.Forms) — used by frmOpeningStock / frmAddUpdateSalesInvoice.
                    services.AddTransient<SalesSystem.Desktop.Products.Forms.frmProductSelection>();

                    // Newly-built screens (wired into frmMain MenuStrip)
                    services.AddTransient<frmPOS>();
                    services.AddTransient<frmEmployeeMaster>();

                    // Newly-built module hub screens (wired into frmMain MenuStrip)
                    services.AddTransient<frmPurchases>();
                    services.AddTransient<frmInventory>();
                })
                .Build();

            Program.ServiceProvider = host.Services;

            // واقعي فقط: تطبيق الهجرات وزرع البيانات الأساسية قبل شاشة الدخول
            var appSettings = host.Services.GetRequiredService<AppSettings>();
            if (!appSettings.UseMockData)
            {
                try
                {
                    using var initScope = host.Services.CreateScope();
                    var initializer = initScope.ServiceProvider
                        .GetRequiredService<SalesSystem.Business.Infrastructure.IApplicationInitializer>();
                    var initResult = initializer.InitializeAsync().GetAwaiter().GetResult();
                    if (!initResult.IsSuccess)
                    {
                        Log.Error("Database initialization failed: {Error}", initResult.Error);
                        MessageBox.Show(initResult.Error ?? "تعذر تهيئة قاعدة البيانات.",
                            "تهيئة قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Unexpected failure during database initialization");
                    MessageBox.Show("تعذر تهيئة قاعدة البيانات. تحقق من الاتصال ثم أعد تشغيل النظام.",
                        "تهيئة قاعدة البيانات", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;


            var loginForm = serviceProvider.GetRequiredService<frmLogin>();
            Application.Run(loginForm);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            MessageBox.Show($"حدث خطأ كارثي أثناء تشغيل التطبيق. تم تسجيل التفاصيل في ملف Logs.\nالخطأ: {ex.Message}", "خطأ فادح", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void GlobalUIExceptionHandler(object sender, ThreadExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Unhandled UI Thread Exception");
        MessageBox.Show("عذراً، حدث خطأ غير متوقع في واجهة المستخدم. تم تسجيل التفاصيل للدعم الفني.", "خطأ بالنظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static void GlobalBackgroundExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        Log.Error(ex, "Unhandled Background Exception");
        MessageBox.Show("عذراً، حدث خطأ غير متوقع في العمليات الخلفية.", "خطأ بالنظام", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
