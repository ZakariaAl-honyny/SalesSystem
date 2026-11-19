using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesSystem.Desktop.Configuration;
using SalesSystem.Desktop.Forms;
using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Services.Direct;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Services.Mock;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.Suppliers;
using SalesSystem.Desktop.Printing;
using SalesSystem.Desktop.Printing.Core;
using SalesSystem.Desktop.Screens.CustomerReceipts.Forms;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.Desktop.Screens.Units.Forms;
using SalesSystem.Desktop.Screens.Sales.Forms;
using SalesSystem.Desktop.Screens.POS.Forms;
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
using SalesSystem.Desktop.Screens.Settings.Forms;
using SalesSystem.Desktop.Screens.Tools.Forms;
using Microsoft.EntityFrameworkCore;
using SalesSystem.DTO.Common;
using SalesSystem.DataAccess;
using SalesSystem.DataAccess.Services;
using SalesSystem.Business.Interfaces;
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

                    // --- I*DesktopService implementations (2-Tier) ---
                    // Mock / Real service toggle (services that have both implementations)
                    if (appSettings.UseMockData)
                    {
                        services.AddTransient<IAuthDesktopService, MockAuthDesktopService>();
                        services.AddTransient<IBackupDesktopService, MockBackupDesktopService>();
                        services.AddTransient<ICashBoxDesktopService, MockCashBoxDesktopService>();
                        services.AddTransient<ICashBoxReportDesktopService, MockCashBoxReportDesktopService>();
                        services.AddTransient<ICategoryDesktopService, MockCategoryDesktopService>();
                        services.AddTransient<ICustomerDesktopService, MockCustomerDesktopService>();
                        services.AddTransient<ICustomerReceiptDesktopService, MockCustomerReceiptDesktopService>();
                        services.AddTransient<IDashboardDesktopService, MockDashboardDesktopService>();
                        services.AddTransient<IExpenseDesktopService, MockExpenseDesktopService>();
                        services.AddTransient<IFinancialReportDesktopService, MockFinancialReportDesktopService>();
                        services.AddTransient<IInventoryAdjustmentDesktopService, MockInventoryAdjustmentDesktopService>();
                        services.AddTransient<IInventoryDesktopService, MockInventoryDesktopService>();
                        services.AddTransient<IInventoryBatchDesktopService, MockInventoryBatchDesktopService>();
                        services.AddTransient<IProductDesktopService, MockProductDesktopService>();
                        services.AddTransient<IPurchaseInvoiceDesktopService, MockPurchaseInvoiceDesktopService>();
                        services.AddTransient<IPurchaseReportDesktopService, MockPurchaseReportDesktopService>();
                        services.AddTransient<IPurchaseReturnDesktopService, MockPurchaseReturnDesktopService>();
                        services.AddTransient<IReportDesktopService, MockReportDesktopService>();
                        services.AddTransient<ISalesInvoiceDesktopService, MockSalesInvoiceDesktopService>();
                        services.AddTransient<ISalesReportDesktopService, MockSalesReportDesktopService>();
                        services.AddTransient<ISalesReturnDesktopService, MockSalesReturnDesktopService>();
                        services.AddTransient<ISettingsDesktopService, MockSettingsDesktopService>();
                        // المرحلة أ — محاضر الجرد الفعلي: Mock فقط حتى موافقة المالك ثم Direct*Service
                        // في المرحلة ب (RULE-N165/RULE-N166/RULE-N168 — لا تسجيل Direct هنا).
                        services.AddTransient<IStocktakingDesktopService, MockStocktakingDesktopService>();
                        services.AddTransient<ISupplierDesktopService, MockSupplierDesktopService>();
                        services.AddTransient<ISupplierPaymentDesktopService, MockSupplierPaymentDesktopService>();
                        // ألوان النظام وإعدادات التنسيق — وضع التجربة: قيم في الذاكرة فقط (RULE-N168).
                        services.AddTransient<IThemeColorDesktopService, MockThemeColorDesktopService>();
                        services.AddTransient<IFormatSettingsDesktopService, MockFormatSettingsDesktopService>();
                        services.AddTransient<IUnitDesktopService, MockUnitDesktopService>();
                        services.AddTransient<IUserDesktopService, MockUserDesktopService>();
                        services.AddTransient<IWarehouseDesktopService, MockWarehouseDesktopService>();
                        // المرحلة أ — سندات التحويل المخزني: Mock فقط حتى موافقة المالك ثم Direct*Service
                        // في المرحلة ب (RULE-N165/RULE-N166/RULE-N168 — لا تسجيل Direct هنا).
                        services.AddTransient<IWarehouseTransferDesktopService, MockWarehouseTransferDesktopService>();
                        services.AddScoped<BizInterfaces.IAuthService, MockAuthService>();
                    }
                    else
                    {
                        services.AddTransient<IAuthDesktopService, DirectAuthService>();
                        services.AddTransient<IBackupDesktopService, DirectBackupService>();
                        services.AddTransient<ICashBoxDesktopService, DirectCashBoxService>();
                        services.AddTransient<ICashBoxReportDesktopService, DirectCashBoxReportService>();
                        services.AddTransient<ICategoryDesktopService, DirectCategoryService>();
                        services.AddTransient<ICustomerDesktopService, DirectCustomerService>();
                        services.AddTransient<ICustomerReceiptDesktopService, DirectCustomerReceiptService>();
                        services.AddTransient<IDashboardDesktopService, DirectDashboardService>();
                        services.AddTransient<IExpenseDesktopService, DirectExpenseService>();
                        services.AddTransient<IFinancialReportDesktopService, DirectFinancialReportService>();
                        services.AddTransient<IInventoryAdjustmentDesktopService, DirectInventoryAdjustmentService>();
                        services.AddTransient<IInventoryDesktopService, DirectInventoryService>();
                        services.AddTransient<IInventoryBatchDesktopService, DirectInventoryBatchService>();
                        services.AddTransient<IProductDesktopService, DirectProductService>();
                        services.AddTransient<IPurchaseInvoiceDesktopService, DirectPurchaseInvoiceService>();
                        services.AddTransient<IPurchaseReportDesktopService, DirectPurchaseReportService>();
                        services.AddTransient<IPurchaseReturnDesktopService, DirectPurchaseReturnService>();
                        services.AddTransient<IReportDesktopService, DirectReportService>();
                        services.AddTransient<ISalesInvoiceDesktopService, DirectSalesInvoiceService>();
                        services.AddTransient<ISalesReportDesktopService, DirectSalesReportService>();
                        services.AddTransient<ISalesReturnDesktopService, DirectSalesReturnService>();
                        services.AddTransient<ISettingsDesktopService, DirectSettingsService>();
                        services.AddTransient<ISupplierDesktopService, DirectSupplierService>();
                        services.AddTransient<ISupplierPaymentDesktopService, DirectSupplierPaymentService>();
                        // ألوان النظام وإعدادات التنسيق — الوضع الحقيقي: تقرأ من SystemSettings وتُطبَّق عند الإقلاع.
                        services.AddTransient<IThemeColorDesktopService, DirectThemeColorService>();
                        services.AddTransient<IFormatSettingsDesktopService, DirectFormatSettingsService>();
                        services.AddTransient<IUnitDesktopService, DirectUnitService>();
                        services.AddTransient<IUserDesktopService, DirectUserService>();
                        services.AddTransient<IWarehouseDesktopService, DirectWarehouseService>();
                        services.AddScoped<BizInterfaces.IAuthService, BizServices.AuthService>();
                    }

                    // DataAccess (2-Tier Direct)
                    services.AddDbContext<SalesDbContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddScoped<IUnitOfWork, UnitOfWork>();
                    // الذاكرة المؤقتة + مستودع إعدادات النظام: تحتاجهما خدمات الباك-إند (ProductService ·
                    // CashBoxService · SalesService · StoreSettingsService …) ومسار تحميل الألوان/التنسيق عند الإقلاع.
                    services.AddMemoryCache();
                    services.AddScoped<SalesSystem.Business.Interfaces.Repositories.ISystemSettingsRepository,
                                       SalesSystem.DataAccess.Repositories.SystemSettingsRepository>();

                    // Startup initializer — applies migrations + seeds the database ONCE at startup, real mode only
                    services.AddScoped<SalesSystem.Business.Infrastructure.IApplicationInitializer,
                                       SalesSystem.DataAccess.Initialization.DatabaseInitializer>();

                    // Infrastructure Services (implemented in DataAccess)
                    services.AddScoped<BizInterfaces.IBackupService, BackupService>();
                    services.AddScoped<BizInterfaces.IConnectionStringProtector, ConnectionStringProtector>();
                    services.AddSingleton<BizInterfaces.ILocalImageStorageService, LocalImageStorageService>();
                    services.AddScoped<BizInterfaces.ILogService, LogService>();
                    services.AddScoped<BizInterfaces.IReportExportService, ReportExportService>();
                    // مستودع التقارير وخدمات الطباعة (تنفيذها في DataAccess) — كانت غير مسجَّلة، فيفشل
                    // الوضع الحقيقي عند حلّ IReportService و SalesService برسالة «Unable to resolve service» (RULE-N10).
                    services.AddScoped<SalesSystem.Business.Interfaces.Repositories.IReportRepository,
                                       SalesSystem.DataAccess.Repositories.ReportRepository>();
                    services.AddScoped<SalesSystem.Business.Printing.IPrintService,
                                       SalesSystem.DataAccess.Printing.PrintService>();
                    // InvoicePrintDtoBuilder تبعية PrintDataService (بلا DbContext ⇒ Singleton).
                    services.AddSingleton<SalesSystem.Business.Printing.InvoicePrintDtoBuilder>();
                    services.AddScoped<SalesSystem.Business.Printing.Contracts.IPrintDataService,
                                       SalesSystem.DataAccess.Printing.PrintDataService>();

                    // Business Services
                    services.AddScoped<BizInterfaces.IAuditLogService, BizServices.AuditLogService>();
                    services.AddScoped<BizInterfaces.IBarcodeLookupService, BizServices.BarcodeLookupService>();
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
                    services.AddScoped<BizInterfaces.IProductCategoryService, BizServices.ProductCategoryService>();
                    services.AddScoped<BizInterfaces.IProductCostService, BizServices.ProductCostService>();
                    services.AddScoped<BizInterfaces.IProductImportService, BizServices.ProductImportService>();
                    services.AddScoped<BizInterfaces.IProductService, BizServices.ProductService>();
                    services.AddScoped<BizInterfaces.IProductUnitService, BizServices.ProductUnitService>();
                    services.AddScoped<BizInterfaces.IPurchaseReportService, BizServices.PurchaseReportService>();
                    services.AddScoped<BizInterfaces.IPurchaseReturnService, BizServices.PurchaseReturnService>();
                    services.AddScoped<BizInterfaces.IPurchaseService, BizServices.PurchaseService>();
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
                    // TASK-007 — audit identity source (username text, no Users FK)
                    services.AddSingleton<SalesSystem.Common.ICurrentUserProvider, DesktopCurrentUserProvider>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<INotificationService, NotificationService>();
                    
                    // Printing Services
                    services.AddTransient<IInvoicePrinter, InvoicePrinter>();
                    services.AddTransient<IReceiptPrinter, ReceiptPrinter>();

                    // UI Components - Essential Forms
                    services.AddTransient<frmLogin>();
                    services.AddTransient<frmMain>();
                    services.AddTransient<frmAddUpdateSalesInvoice>();
                    services.AddTransient<frmAddUpdatePurchaseInvoice>();
                    // المرتجعات (TASK-017 / TASK-018): قائمة واحدة `frmListReturns` + محرّران موحّدان —
                    // `frmAddUpdateSalesInvoice` بوضع «مرتجع بيع» و`frmAddUpdatePurchaseInvoice` بوضع «مرتجع شراء»
                    // ⇒ لا محرّر منفصل لأي مرتجع.
                    services.AddTransient<frmListReturns>();
                    services.AddTransient<frmCRUDWarehouseMaster>();
                    services.AddTransient<frmListSales>();
                    services.AddTransient<frmListPurchases>();

                    // TASK-016 — منتقيا الفواتير: قائمة الفواتير + بياناتها الكاملة (رأس + سطور) لبناء مرتجع.
                    // الفاتورة الكاملة تأتي من GetByIdAsync في العقد القائم — لا مسار بيانات جديد.
                    services.AddTransient<frmSalesInvoiceSelection>();
                    // Fully-qualified: الشاشتان في مجلدين مختلفين — والاسم صريح لا يعتمد على using.
                    services.AddTransient<SalesSystem.Desktop.Screens.Purchases.Forms.frmPurchaseInvoiceSelection>();
                    
                    // Module Controls — NEW Forms
                    services.AddTransient<frmDashboard>();
                    services.AddTransient<SalesSystem.Desktop.Screens.Dashboard.Controls.ctrlDashboard>(); // RULE-N67
                    services.AddTransient<frmCRUDCProductMaster>();
                    // Master forms (navigation targets from frmMain)
                    services.AddTransient<frmCRUDCategoryMaster>();
                    services.AddTransient<frmCRUDUnitMaster>();
                    services.AddTransient<frmCRUDCustomerMaster>();
                    services.AddTransient<frmShowCustomerCard>();
                    services.AddTransient<frmFindCustomer>();
                    services.AddTransient<frmCRUDCSupplierMaster>();
                    services.AddTransient<frmInventoryList>();
                    services.AddTransient<frmListPayments>();
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
                    services.AddTransient<frmIncomeStatementReport>();
                    services.AddTransient<frmCashFlowReport>();
                    services.AddTransient<frmAccountStatementReport>();
                    services.AddTransient<frmAgingReport>();
                    services.AddTransient<frmCashBoxSummaryReport>();
                    services.AddTransient<frmDailyClosureReport>();
                    // Users/Admin Forms
                    services.AddTransient<frmPasswordChange>();
                    services.AddTransient<frmBackup>();
                    // إعدادات — شاشة ألوان النظام (تُفتح من msbtnSystemColors في frmMain)
                    services.AddTransient<frmChangeSystemColorsSettings>();
                    services.AddTransient<frmFormatingSetting>();
                    // Master forms — replace the removed frmList* / frmAddUpdate* screens
                    services.AddTransient<frmCRUDUserMaster>();

                    // Inventory Module - New Forms
                    services.AddTransient<frmListInventoryAdjustments>();
                    services.AddTransient<frmAddUpdateInventoryAdjustment>();
                    services.AddTransient<frmListInventoryTransactions>();
                    services.AddTransient<frmListInventoryBatches>();
                    services.AddTransient<frmOpeningStock>();

                    // CashBoxes Module
                    services.AddTransient<frmCRUDCashBoxMaster>();
                    services.AddTransient<frmCashBoxTransactions>();

                    // Expenses Module — F-004 (D-01/D-03): شاشة Master مدمجة بدل frmList* + frmAddUpdate*
                    services.AddTransient<frmCRUDCExpensesMaster>();

                    // CustomerReceipts Module — F-004 (D-01/D-03): شاشة Master مدمجة بدل frmList* + frmAddUpdate*
                    services.AddTransient<frmCRUDCustomerReceiptsMaster>();

                    // Supplier Payments Module
                    services.AddTransient<frmListSupplierPayments>();

                    // Payment Dialogs
                    services.AddTransient<frmSupplierPaymentDialog>();

                    // Product Management Forms
                    services.AddTransient<frmCRUDCProductUnitMaster>();
                    // Fully-qualified: two frmProductSelection types exist.
                    // Legacy picker (Products.Forms) — used by frmOpeningStock / frmAddUpdateSalesInvoice.
                    services.AddTransient<SalesSystem.Desktop.Products.Forms.frmProductSelection>();

                    // Newly-built screens (wired into frmMain MenuStrip)
                    services.AddTransient<frmPOS>();

                    // Newly-built module hub screens (wired into frmMain MenuStrip)
                    services.AddTransient<frmInventory>();

                    // ─────────────────────────────────────────────────────────────────────
                    // شاشات موجودة في Screens/** ولم تكن مُسجَّلة في الحاوية (RULE-N10).
                    // RULE-N02: تُفتح من الحاوية فقط — لا إنشاء يدوي لأي نموذج في أي شاشة.
                    // ─────────────────────────────────────────────────────────────────────
                    // الأصناف
                    services.AddTransient<frmImportProductsExcel>();
                    // المخزون — تحويل · جرد · دفعات · باركود (وملفات الطباعة التابعة لها)
                    services.AddTransient<frmWarehouseTransferNew>();
                    services.AddTransient<frmWarehouseTransferPrint>();
                    services.AddTransient<frmStocktakingNew>();
                    services.AddTransient<frmStocktakingPrint>();
                    services.AddTransient<frmStocktakingPrintAdjustment>();
                    services.AddTransient<frmStocktakingPrintCard>();
                    services.AddTransient<frmInventoryFifo>();
                    services.AddTransient<frmInventoryBarcode>();
                    // المشتريات — حوارات سطور/خصم/تفاصيل وطباعة الفاتورة
                    services.AddTransient<frmPurchaseInvoicePrint>();
                    services.AddTransient<frmPurchaseItem>();
                    services.AddTransient<frmPurchaseItemInfo>();
                    services.AddTransient<frmPurchaseDiscount>();
                    services.AddTransient<frmPurchaseOtherDetails>();
                    // العملاء والموردون — اختيار وكشف حساب
                    services.AddTransient<frmCustomerSelection>();
                    services.AddTransient<frmCustomerStatement>();
                    services.AddTransient<frmSupplierSelection>();
                    services.AddTransient<frmSupplierStatement>();
                    // الأدوات
                    services.AddTransient<frmCalculator>();
                })
                .Build();

            Program.ServiceProvider = host.Services;

            // ترخيص QuestPDF (Community) — يُهيَّأ مرة واحدة قبل أي توليد PDF.
            // أي فشل يُسجَّل فقط ولا يمنع الإقلاع.
            try
            {
                SalesSystem.DataAccess.Printing.PrintingBootstrapper.Initialize();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "QuestPDF printing bootstrap failed; printing may be unavailable");
            }

            // واقعي فقط: تطبيق الهجرات وزرع البيانات الأساسية قبل شاشة الدخول.
            // فشل التهيئة = توقّف مُغلق (fail-closed): لا شاشة دخول على قاعدة بيانات غير مهيّأة (RULE-N166).
            var appSettings = host.Services.GetRequiredService<AppSettings>();
            if (!appSettings.UseMockData)
            {
                bool initFailed;

                try
                {
                    using var initScope = host.Services.CreateScope();
                    var initializer = initScope.ServiceProvider
                        .GetRequiredService<SalesSystem.Business.Infrastructure.IApplicationInitializer>();
                    var initResult = initializer.InitializeAsync().GetAwaiter().GetResult();

                    initFailed = !initResult.IsSuccess;

                    if (initFailed)
                        Log.Error("Database initialization failed: {Error}", initResult.Error);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Unexpected failure during database initialization");
                    initFailed = true;
                }

                // لا شاشة دخول بلا قاعدة بيانات جاهزة: رسالة عربية واحدة ثم خروج نظيف
                // (السبب الحقيقي يبقى في السجل فقط — لا يُعرض نص استثناء للمستخدم).
                if (initFailed)
                {
                    clsDialogs.Error(
                        "تعذر تجهيز قاعدة البيانات (تطبيق التحديثات وزرع البيانات الأساسية)، " +
                        "ولذلك سيُغلق النظام الآن ولن تُفتح شاشة الدخول.\n" +
                        "تحقق من اتصال قاعدة البيانات وصلاحية المستخدم ثم أعد تشغيل النظام. " +
                        "التفاصيل مسجَّلة في ملف السجل.",
                        "تعذر تجهيز قاعدة البيانات");

                    Environment.ExitCode = 1;   // فشل الإقلاع — رمز خروج غير صفري
                    return;                     // قبل إنشاء/عرض أي نموذج — و« finally » يُفرِّغ سجل Serilog
                }

                // قراءة الألوان وإعدادات التنسيق المحفوظة في SystemSettings وتطبيقها قبل إنشاء أي نموذج.
                // صفوف غائبة (أول تشغيل) أو أي فشل = متابعة بالقيم الافتراضية بلا إزعاج المستخدم ولا تعطيل الإقلاع.
                try
                {
                    using var prefScope = host.Services.CreateScope();

                    var themeService = prefScope.ServiceProvider.GetRequiredService<IThemeColorDesktopService>();
                    var themeResult = themeService.GetAsync().GetAwaiter().GetResult();
                    if (themeResult.IsSuccess && themeResult.Value is not null)
                        ThemePalette.TryApplyStored(themeResult.Value.Colors);

                    var formatService = prefScope.ServiceProvider.GetRequiredService<IFormatSettingsDesktopService>();
                    var formatResult = formatService.GetAsync().GetAwaiter().GetResult();
                    if (formatResult.IsSuccess && formatResult.Value is not null)
                        clsFormat.TryApplyAll(formatResult.Value.Values);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to load persisted theme/format settings; continuing with defaults");
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
            clsDialogs.Error("حدث خطأ فادح أثناء بدء التشغيل. تم تسجيل التفاصيل في ملف السجل. أغلق التطبيق وأعد المحاولة.", "خطأ فادح");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void GlobalUIExceptionHandler(object sender, ThreadExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Unhandled UI Thread Exception");
        clsDialogs.Error("عذراً، حدث خطأ غير متوقع في واجهة المستخدم. تم تسجيل التفاصيل للدعم الفني.", "خطأ بالنظام");
    }

    private static void GlobalBackgroundExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        Log.Error(ex, "Unhandled Background Exception");
        clsDialogs.Error("عذراً، حدث خطأ غير متوقع في العمليات الخلفية.", "خطأ بالنظام");
    }
}
