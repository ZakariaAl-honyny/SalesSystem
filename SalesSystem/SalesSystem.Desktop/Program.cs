using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.Forms;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Controls.Users;
using SalesSystem.Desktop.Services.Http;
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
using SalesSystem.Desktop.Screens.Roles.Forms;
using SalesSystem.Desktop.Screens.Taxes.Forms;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.Desktop.Screens.StockTransfers.Forms;
using SalesSystem.Desktop.Screens.Units.Forms;
using SalesSystem.Desktop.Screens.Sales.Forms;
using SalesSystem.Desktop.Screens.POS.Forms;
using SalesSystem.Desktop.Screens.Employees.Forms;
using SalesSystem.Desktop.Screens.Products.Forms;
using SalesSystem.Desktop.Screens.Returns.Forms;
using SalesSystem.Desktop.Screens.Purchases.Forms;
using SalesSystem.Desktop.Screens.PaymentVouchers.Forms;
using SalesSystem.Desktop.Screens.Customers.Forms;
using SalesSystem.Desktop.Screens.Categories.Forms;
using SalesSystem.Desktop.Screens.Payments.Forms;
using SalesSystem.Desktop.Screens.Expenses.Forms;
using SalesSystem.Desktop.Screens.CashBoxes.Forms;
using SalesSystem.Desktop.Screens.CashVouchers.Forms;
using SalesSystem.Desktop.Screens.Accounting.Forms;
using SalesSystem.Desktop.Screens.Banks.Forms;
using SalesSystem.Desktop.Screens.Warehouses.Forms;
using SalesSystem.Desktop.Screens.Inventory.Forms;
using SalesSystem.Desktop.Suppliers.Forms;
using SalesSystem.Desktop.Screens;
using SalesSystem.Desktop.Screens.Settings.Forms;
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
                    // Configuration
                    var apiSettings = context.Configuration.GetSection("ApiSettings").Get<ApiSettings>() 
                                     ?? new ApiSettings();
                    services.AddSingleton(apiSettings);

                    // App Settings (Mock Mode Toggle)
                    var appSettings = context.Configuration.GetSection("AppSettings").Get<AppSettings>() ?? new AppSettings();
                    AppSettings.Current = appSettings;
                    services.AddSingleton(appSettings);

                    // Http Services
                    services.AddTransient<AuthTokenHandler>();
                    
                    services.AddHttpClient<HttpClientService>(client =>
                    {
                        client.BaseAddress = new Uri(apiSettings.BaseUrl);
                    }).AddHttpMessageHandler<AuthTokenHandler>();

                    // API Services (Inject HttpClientService)
                    services.AddTransient<IAuthApiService, AuthApiService>();
                    services.AddTransient<ICategoryApiService, CategoryApiService>();
                    services.AddTransient<IUnitApiService, UnitApiService>();
                    services.AddTransient<ISupplierApiService, SupplierApiService>();
                    services.AddTransient<IPurchaseInvoiceApiService, PurchaseInvoiceApiService>();
                    services.AddTransient<ISalesReturnApiService, SalesReturnApiService>();
                    services.AddTransient<IPurchaseReturnApiService, PurchaseReturnApiService>();
                    services.AddTransient<IStockTransferApiService, StockTransferApiService>();
                    services.AddTransient<ICustomerPaymentApiService, CustomerPaymentApiService>();
                    services.AddTransient<ISupplierPaymentApiService, SupplierPaymentApiService>();
                    services.AddTransient<IReportApiService, ReportApiService>();
                    services.AddTransient<IDashboardApiService, DashboardApiService>();
                    services.AddTransient<IInventoryApiService, InventoryApiService>();
                    services.AddTransient<IInventoryAdjustmentApiService, InventoryAdjustmentApiService>();
                    services.AddTransient<IInventoryCountApiService, InventoryCountApiService>();
                    services.AddTransient<IInventoryBatchApiService, InventoryBatchApiService>();
                    services.AddTransient<IProductPriceApiService, ProductPriceApiService>();
                    services.AddTransient<ICompanySettingsApiService, CompanySettingsApiService>();
                    services.AddTransient<INotificationApiService, NotificationApiService>();
                    services.AddTransient<IBackupApiService, BackupApiService>();
                    services.AddTransient<IUserApiService, UserApiService>();
                    services.AddTransient<IPersonApiService, PersonApiService>();
                    services.AddTransient<IAccountApiService, AccountApiService>();
                    services.AddTransient<IJournalEntryApiService, JournalEntryApiService>();
                    services.AddTransient<ISystemAccountMappingApiService, SystemAccountMappingApiService>();
                    services.AddTransient<IAccountCategoryApiService, AccountCategoryApiService>();
                    // Report API Services
                    services.AddTransient<IFinancialReportApiService, FinancialReportApiService>();
                    services.AddTransient<ISalesReportApiService, SalesReportApiService>();
                    services.AddTransient<IPurchaseReportApiService, PurchaseReportApiService>();
                    services.AddTransient<ICashBoxReportApiService, CashBoxReportApiService>();
                    services.AddTransient<IUserReportApiService, UserReportApiService>();
                    // Admin/Settings API Services
                    services.AddTransient<IRoleApiService, RoleApiService>();
                    services.AddTransient<IUserSessionApiService, UserSessionApiService>();
                    services.AddTransient<IDocumentSequenceApiService, DocumentSequenceApiService>();
                    services.AddTransient<IAuditLogApiService, AuditLogApiService>();
                    services.AddTransient<ILogsApiService, LogsApiService>();

                    // Module API Services
                    services.AddTransient<IBranchApiService, BranchApiService>();
                    services.AddTransient<ICashBoxApiService, CashBoxApiService>();
                    services.AddTransient<IBankApiService, BankApiService>();
                    services.AddTransient<IExpenseApiService, ExpenseApiService>();
                    services.AddTransient<ICustomerReceiptApiService, CustomerReceiptApiService>();
                    services.AddTransient<IPaymentVoucherApiService, PaymentVoucherApiService>();

                    // Mock / Real service toggle (6 services)
                    if (appSettings.UseMockData)
                    {
                        services.AddTransient<ISalesInvoiceApiService, MockSalesInvoiceApiService>();
                        services.AddTransient<ICustomerApiService, MockCustomerApiService>();
                        services.AddTransient<IProductApiService, MockProductApiService>();
                        services.AddTransient<IWarehouseApiService, MockWarehouseApiService>();
                        services.AddTransient<ISettingsApiService, MockSettingsApiService>();
                        services.AddTransient<ITaxesApiService, MockTaxesApiService>();
                        services.AddScoped<BizInterfaces.IAuthService, MockAuthService>();
                    }
                    else
                    {
                        services.AddTransient<ISalesInvoiceApiService, SalesInvoiceApiService>();
                        services.AddTransient<ICustomerApiService, CustomerApiService>();
                        services.AddTransient<IProductApiService, ProductApiService>();
                        services.AddTransient<IWarehouseApiService, WarehouseApiService>();
                        services.AddTransient<ISettingsApiService, SettingsApiService>();
                        services.AddTransient<ITaxesApiService, TaxesApiService>();
                        services.AddScoped<BizInterfaces.IAuthService, BizServices.AuthService>();
                    }

                    // DataAccess (2-Tier Direct)
                    services.AddDbContext<SalesDbContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddScoped<IUnitOfWork, UnitOfWork>();

                    var jwtSettings = context.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
                                     ?? new JwtSettings();
                    services.AddSingleton(jwtSettings);

                    // Infrastructure Services (implemented in DataAccess)
                    services.AddScoped<BizInterfaces.IBackupService, BackupService>();
                    services.AddScoped<BizInterfaces.IConnectionStringProtector, ConnectionStringProtector>();
                    services.AddScoped<BizInterfaces.IJwtTokenGenerator, JwtTokenGenerator>();
                    services.AddSingleton<BizInterfaces.ILocalImageStorageService, LocalImageStorageService>();
                    services.AddScoped<BizInterfaces.ILogService, LogService>();
                    services.AddScoped<BizInterfaces.IReportExportService, ReportExportService>();

                    // Accounting Services
                    services.AddScoped<BizInterfaces.IAccountingIntegrationService, AccountingIntegrationService>();
                    services.AddScoped<BizInterfaces.IAnnualClosingService, AnnualClosingService>();
                    services.AddScoped<BizInterfaces.IFiscalYearService, FiscalYearService>();
                    services.AddScoped<BizInterfaces.IJournalEntryNumberGenerator, JournalEntryNumberGenerator>();
                    services.AddScoped<BizInterfaces.IJournalEntryService, JournalEntryService>();
                    services.AddScoped<BizInterfaces.ISystemAccountService, SystemAccountService>();

                    // Business Services
                    services.AddScoped<BizInterfaces.IAccountCategoryService, BizServices.AccountCategoryService>();
                    services.AddScoped<BizInterfaces.IAccountCodeGeneratorService, BizServices.AccountCodeGeneratorService>();
                    services.AddScoped<BizInterfaces.IAccountLinkService, BizServices.AccountLinkService>();
                    services.AddScoped<BizInterfaces.IAccountService, BizServices.AccountService>();
                    services.AddScoped<BizInterfaces.IAssemblyService, BizServices.AssemblyService>();
                    services.AddScoped<BizInterfaces.IAttachmentService, BizServices.AttachmentService>();
                    services.AddScoped<BizInterfaces.IAuditLogService, BizServices.AuditLogService>();
                    services.AddScoped<BizInterfaces.IBankService, BizServices.BankService>();
                    services.AddScoped<BizInterfaces.IBarcodeLookupService, BizServices.BarcodeLookupService>();
                    services.AddScoped<BizInterfaces.IBranchService, BizServices.BranchService>();
                    services.AddScoped<BizInterfaces.ICashBoxReportService, BizServices.CashBoxReportService>();
                    services.AddScoped<BizInterfaces.ICashBoxService, BizServices.CashBoxService>();
                    services.AddScoped<BizInterfaces.ICompanySettingsService, BizServices.CompanySettingsService>();
                    services.AddScoped<BizInterfaces.ICustomerContactService, BizServices.CustomerContactService>();
                    services.AddScoped<BizInterfaces.ICustomerReceiptService, BizServices.CustomerReceiptService>();
                    services.AddScoped<BizInterfaces.ICustomerService, BizServices.CustomerService>();
                    services.AddScoped<BizInterfaces.IDocumentSequenceService, BizServices.DocumentSequenceService>();
                    services.AddScoped<BizInterfaces.IExpenseService, BizServices.ExpenseService>();
                    services.AddScoped<BizInterfaces.IFifoAllocationService, BizServices.FifoAllocationService>();
                    services.AddScoped<BizInterfaces.IFinancialReportService, BizServices.FinancialReportService>();
                    services.AddScoped<BizInterfaces.IInventoryAdjustmentService, BizServices.InventoryAdjustmentService>();
                    services.AddScoped<BizInterfaces.IInventoryBatchService, BizServices.InventoryBatchService>();
                    services.AddScoped<BizInterfaces.IInventoryCountService, BizServices.InventoryCountService>();
                    services.AddScoped<BizInterfaces.IInventoryService, BizServices.InventoryService>();
                    services.AddScoped<BizInterfaces.IMinStockAlertService, BizServices.MinStockAlertService>();
                    services.AddScoped<BizInterfaces.INotificationService, BizServices.NotificationService>();
                    services.AddScoped<BizInterfaces.IPaymentVoucherService, BizServices.PaymentVoucherService>();
                    services.AddScoped<BizInterfaces.IPermissionService, BizServices.PermissionService>();
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
                    services.AddScoped<BizInterfaces.IRoleService, BizServices.RoleService>();
                    services.AddScoped<BizInterfaces.ISalesReportService, BizServices.SalesReportService>();
                    services.AddScoped<BizInterfaces.ISalesReturnService, BizServices.SalesReturnService>();
                    services.AddScoped<BizInterfaces.ISalesService, BizServices.SalesService>();
                    services.AddScoped<BizInterfaces.ISessionManagementService, BizServices.SessionManagementService>();
                    services.AddScoped<BizInterfaces.IStoreSettingsService, BizServices.StoreSettingsService>();
                    services.AddScoped<BizInterfaces.ISupplierContactService, BizServices.SupplierContactService>();
                    services.AddScoped<BizInterfaces.ISupplierPaymentApplicationService, BizServices.SupplierPaymentApplicationService>();
                    services.AddScoped<BizInterfaces.ISupplierPaymentService, BizServices.SupplierPaymentService>();
                    services.AddScoped<BizInterfaces.ISupplierService, BizServices.SupplierService>();
                    services.AddScoped<BizInterfaces.ITaxService, BizServices.TaxService>();
                    services.AddScoped<BizInterfaces.IUnitService, BizServices.UnitService>();
                    services.AddScoped<BizInterfaces.IUpdateProductPricingService, BizServices.UpdateProductPricingService>();
                    services.AddScoped<BizInterfaces.IUserReportService, BizServices.UserReportService>();
                    services.AddScoped<BizInterfaces.IUserService, BizServices.UserService>();
                    services.AddScoped<BizInterfaces.IWarehouseService, BizServices.WarehouseService>();
                    services.AddScoped<BizInterfaces.IWarehouseTransferService, BizServices.WarehouseTransferService>();

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
                    services.AddTransient<frmAddUpdateCategory>();
                    services.AddTransient<frmAddUpdateUnit>();
                    services.AddTransient<frmAddUpdateWarehouse>();
                    services.AddTransient<frmAddUpdateCustomer>();
                    services.AddTransient<frmAddUpdateSupplier>();
                    services.AddTransient<frmAddUpdateProduct>();
                    services.AddTransient<frmSalesInvoice>();
                    services.AddTransient<frmPurchaseInvoice>();
                    services.AddTransient<frmSalesReturn>();
                    services.AddTransient<frmPurchaseReturn>();
                    services.AddTransient<frmStockTransfer>();
                    services.AddTransient<frmListPeople>();
                    services.AddTransient<frmAddUpdatePerson>();
                    services.AddTransient<frmShowPersonInfo>();
                    services.AddTransient<frmFindPerson>();
                    services.AddTransient<frmListReturns>();
                    services.AddTransient<frmListWarehouses>();
                    services.AddTransient<frmListUnits>();
                    services.AddTransient<frmListSales>();
                    services.AddTransient<frmListPurchases>();
                    
                    // Module Controls — NEW Forms
                    services.AddTransient<frmDashboard>();
                    services.AddTransient<frmListProducts>();
                    services.AddTransient<frmListCustomers>();
                    services.AddTransient<frmListSuppliers>();
                    services.AddTransient<frmInventoryList>();
                    services.AddTransient<frmListStockTransfers>();
                    services.AddTransient<frmListPayments>();
                    services.AddTransient<frmSettings>();
                    services.AddTransient<frmCompanySettings>();
                    services.AddTransient<frmNotifications>();
                    services.AddTransient<frmUpdateCheck>();
                    services.AddTransient<frmUsers>();
                    services.AddTransient<frmReports>();
                    services.AddTransient<frmSalesReport>();
                    services.AddTransient<frmPurchasesReport>();
                    services.AddTransient<frmInventoryReport>();
                    services.AddTransient<frmCustomerBalanceReport>();
                    services.AddTransient<frmSupplierBalanceReport>();
                    services.AddTransient<frmProductMovementReport>();
                    services.AddTransient<frmLowStockReport>();
                    // Accounting Forms
                    services.AddTransient<frmListJournalEntries>();
                    services.AddTransient<frmAddUpdateJournalEntry>();
                    services.AddTransient<frmListAccounts>();
                    services.AddTransient<frmAddUpdateAccount>();
                    services.AddTransient<frmSystemAccountMappings>();
                    services.AddTransient<frmListAccountCategories>();
                    services.AddTransient<frmListCategories>();
                    services.AddTransient<frmAddUpdateAccountCategory>();
                    // Users/Admin Forms
                    services.AddTransient<frmAddUpdateUser>();
                    services.AddTransient<frmPasswordChange>();
                    services.AddTransient<frmBackup>();
                    services.AddTransient<frmListUsers>();
                    services.AddTransient<frmListRoles>();
                    services.AddTransient<frmAddUpdateRole>();
                    services.AddTransient<frmAuditLog>();
                    services.AddTransient<frmSystemLogs>();
                    services.AddTransient<frmUserSessions>();
                    services.AddTransient<frmDocumentSequences>();
                    services.AddTransient<frmListTaxes>();
                    services.AddTransient<frmAddUpdateTax>();
                    services.AddTransient<frmListAccountCategories>();
                    services.AddTransient<frmAddUpdateAccountCategory>();

                    // Inventory Module - New Forms
                    services.AddTransient<frmListInventoryAdjustments>();
                    services.AddTransient<frmAddUpdateInventoryAdjustment>();
                    services.AddTransient<frmListInventoryCounts>();
                    services.AddTransient<frmInventoryCountEditor>();
                    services.AddTransient<frmListInventoryTransactions>();
                    services.AddTransient<frmListInventoryBatches>();
                    services.AddTransient<frmOpeningStock>();

                    // CashBoxes Module
                    services.AddTransient<frmListCashBoxes>();
                    services.AddTransient<frmAddUpdateCashBox>();
                    services.AddTransient<frmCashTransfer>();
                    services.AddTransient<frmCashBoxTransactions>();
                    services.AddTransient<frmDailyClosure>();

                    // Banks Module
                    services.AddTransient<frmListBanks>();
                    services.AddTransient<frmAddUpdateBank>();

                    // Expenses Module
                    services.AddTransient<frmListExpenses>();
                    services.AddTransient<frmAddUpdateExpense>();

                    // CustomerReceipts Module
                    services.AddTransient<frmListCustomerReceipts>();
                    services.AddTransient<frmAddUpdateCustomerReceipt>();

                    // PaymentVouchers Module
                    services.AddTransient<frmListPaymentVouchers>();
                    services.AddTransient<frmAddUpdatePaymentVoucher>();

                    // Supplier Payments Module
                    services.AddTransient<frmListSupplierPayments>();
                    services.AddTransient<frmAddUpdateSupplierPayment>();

                    // Payment Dialogs
                    services.AddTransient<frmCustomerPaymentDialog>();
                    services.AddTransient<frmSupplierPaymentDialog>();

                    // Product Management Forms
                    services.AddTransient<frmProductPrices>();
                    services.AddTransient<frmProductUnitsList>();
                    services.AddTransient<frmAddUpdateProductUnit>();
                    services.AddTransient<frmAddUpdateProductCategory>();
                    // Fully-qualified: two frmProductSelection types exist.
                    // Old API-backed picker (Products.Forms) — used by frmOpeningStock / frmAddUpdateSalesInvoice.
                    services.AddTransient<SalesSystem.Desktop.Products.Forms.frmProductSelection>();
                    // New UI-first prototype (Sales.Forms) — wired to the main menu.
                    services.AddTransient<SalesSystem.Desktop.Screens.Sales.Forms.frmSmartProductSelection>();

                    // Receipt Vouchers
                    services.AddTransient<frmListReceiptVouchers>();

                    // Newly-built screens (wired into frmMain MenuStrip)
                    services.AddTransient<frmPOS>();
                    services.AddTransient<frmListEmployees>();
                    services.AddTransient<frmAddUpdateEmployee>();
                    services.AddTransient<frmDiscountExpenses>();
                    services.AddTransient<frmCurrencySelection>();
                    services.AddTransient<frmCustomerPicker>();

                    // المبيعات — أدوات الفاتورة الإضافية (جديد)
                    services.AddTransient<frmSalesProfitBreakdown>();
                    services.AddTransient<frmPOSHeldOrders>();
                    services.AddTransient<frmSalesImportConvert>();
                    services.AddTransient<frmReceiptPrint>();
                    services.AddTransient<frmSalesSmartF3Lookup>();
                    // استراتيجيات الإدخال
                    services.AddTransient<frmSalesContinuousScanner>();
                    services.AddTransient<frmSalesTouchGrid>();
                    services.AddTransient<frmSalesRapidBar>();
                    services.AddTransient<frmSalesInlineGrid>();

                    // Newly-built module hub screens (wired into frmMain MenuStrip)
                    services.AddTransient<frmCashVouchers>();
                    services.AddTransient<frmPurchases>();
                    services.AddTransient<frmInventory>();
                    services.AddTransient<frmAccounting>();
                    services.AddTransient<frmReportsDashboard>();
                })
                .Build();

            Program.ServiceProvider = host.Services;

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
