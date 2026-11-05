using SalesSystem.Desktop.GlobalClasses;
using SalesSystem.Desktop.Services;
using SalesSystem.Desktop.Services.Interfaces;
using SalesSystem.Desktop.Products.Forms;
using SalesSystem.Desktop.CustomerReceipts.Forms;

using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Desktop.Screens.Users.Forms;
using SalesSystem.Desktop.Screens.Units.Forms;
using SalesSystem.Desktop.Screens.Sales.Forms;
using SalesSystem.Desktop.Screens.POS.Forms;
using SalesSystem.Desktop.Screens.Employees.Forms;
using SalesSystem.Desktop.Screens.Products.Forms;
using SalesSystem.Desktop.Screens.Purchases.Forms;
using SalesSystem.Desktop.Screens.Returns.Forms;
using SalesSystem.Desktop.Screens.Customers.Forms;
using SalesSystem.Desktop.Screens.Categories.Forms;
using SalesSystem.Desktop.Screens.Payments.Forms;
using SalesSystem.Desktop.Screens.Expenses.Forms;
using SalesSystem.Desktop.Screens.CashBoxes.Forms;
using SalesSystem.Desktop.Screens.Inventory.Forms;
using SalesSystem.Desktop.Screens.Warehouses.Forms;
using SalesSystem.Desktop.Suppliers.Forms;
using SalesSystem.Desktop.Screens.Reports.Forms;
using SalesSystem.Desktop.Helpers;
using SalesSystem.Desktop.Configuration;
 
namespace SalesSystem.Desktop.Screens;

public partial class frmMain : Form
{
    private readonly ISessionService _sessionService;
    private readonly IServiceProvider _serviceProvider;

    public frmMain(ISessionService sessionService, IServiceProvider serviceProvider)
    {
        _sessionService = sessionService;
        _serviceProvider = serviceProvider;
        InitializeComponent();

        KeyboardHelper.WireEnterAsTab(this);

        this.RightToLeft = RightToLeft.Yes;
        this.RightToLeftLayout = true;

        // MVP: إخفاء شاشات النسخة الثانية (V2) — الكود سليم ومحفوظ لإعادة التفعيل لاحقاً
        ApplyMvpMenuVisibility();
    }

    /// <summary>
    /// MVP (محل واحد / مستخدم واحد / فرع واحد): إخفاء عناصر القوائم المؤجلة إلى V2.
    /// </summary>
    private void ApplyMvpMenuVisibility()
    {
        // ── المنتجات ─────────────────────────────────────────────────────────
        msbtnProductCategories.Visible = false;     // MVP: مخفي (V2)

        // ── إضافة سريعة (أصبحت داخل شاشات الـ Master) ───────────────────────
        msbtnAddProduct.Visible = false;            // MVP: مخفي — add handled inside Master form
        msbtnAddCustomer.Visible = false;           // MVP: مخفي — add handled inside Master form
        msbtnAddSupplier.Visible = false;           // MVP: مخفي — add handled inside Master form

        // ── المبيعات ────────────────────────────────────────────────────────
        msbtnCurrencySelection.Visible = false;     // MVP: محذوف (وحدة العملات الوهمية) — يبقى مخفياً

        // ── الإعدادات ───────────────────────────────────────────────────────
        // الإعدادات العامة وإعدادات الشركة أُزيلتا — تبقى شاشتا النسخ الاحتياطي والتحديث فقط
        // (شاشتا النسخ الاحتياطي والتحديث مخفيتان في MVP — انظر msbtnUpdateCheck أدناه)
        msbtnSettings.Visible = false;
        msbtnCompanySettings.Visible = false;
        msbtnUpdateCheck.Visible = false; // V2
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        if (!_sessionService.IsAuthenticated)
        {
            Application.Exit();
            return;
        }

        // Store reference for clsGlobal._OpenForm()
        clsGlobal.frmMdiMain = this;

        // Set MdiClient background to WhiteSmoke
        var MdiContainerPage = this.Controls.OfType<MdiClient>().FirstOrDefault();
        if (MdiContainerPage != null)
            MdiContainerPage.BackColor = Color.WhiteSmoke;

        // MVP: إخفاء شاشات V2 (يُطبَّق أيضاً في المنشئ — إبقاء العناصر المؤجلة مخفية بعد التحميل)
        ApplyMvpMenuVisibility();

        // Mock-mode visual indicator
        if (AppSettings.Current.UseMockData)
        {
            this.Text += "  — وضع التجربة (Mock)";
        }
    }

    // ── المنتجات (Products) ───────────────────────────────────────────────────

    private void msbtnManageProducts_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmProductMaster>());

    private void msbtnAddProduct_Click(object sender, EventArgs e)
    {
        // MVP: مخفي — add handled inside Master form
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateProduct>());
    }

    private void msbtnManageCustomers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCustomerMaster>());

    private void msbtnAddCustomer_Click(object sender, EventArgs e)
    {
        // MVP: مخفي — add handled inside Master form
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateCustomer>());
    }

    private void msbtnManageSuppliers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSupplierMaster>());

    private void msbtnAddSupplier_Click(object sender, EventArgs e)
    {
        // MVP: مخفي — add handled inside Master form
        // clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmAddUpdateSupplier>());
    }

    private void msbtnManageCategories_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCategoryMaster>());

    private void msbtnManageUnits_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmUnitMaster>());

    private void msbtnProductCategories_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCategoryMaster>());

    // ── المخزون (Warehouse / Stock Transfers / Inventory) ────────────────────

    private void msbtnManageWarehouses_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmWarehouseMaster>());

    private void msbtnInventoryMovement_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmInventoryList>());

    private void msbtnInventoryAdjustments_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListInventoryAdjustments>());

    private void msbtnInventoryCounts_Click(object sender, EventArgs e)
    {
        // MVP: شاشة الجرد المخزني (V2) أُزيلت مع الوحدة — لا إجراء.
    }

    private void msbtnInventory_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmInventory>());

    // ── المشتريات (Purchases) ────────────────────────────────────────────────

    private void msbtnManagePurchases_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListPurchases>());

    private void msbtnPurchaseInvoice_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPurchaseInvoice>());

    private void msbtnPurchaseReturn_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPurchaseReturn>());

    private void msbtnPurchases_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPurchases>());

    // ── المبيعات (Sales) ─────────────────────────────────────────────────────

    private void msbtnManageSales_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListSales>());

    private void msbtnSalesInvoice_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSalesInvoice>());

    private void msbtnSalesReturn_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmSalesReturn>());

    // ── المبيعات — نقطة البيع وأدوات الفاتورة (جديد) ────────────────

    private void msbtnPOS_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmPOS>());

    private void msbtnCurrencySelection_Click(object sender, EventArgs e)
        => MessageBox.Show("وحدة العملات غير متوفرة في نسخة MVP الحالية؛ العملة الافتراضية هي الريال اليمني.",
            "العملات", MessageBoxButtons.OK, MessageBoxIcon.Information);

    // ── الخزينة (Treasury) ───────────────────────────────────────────────────

    private void msbtnManagePayments_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListPayments>());

    private void msbtnCashBoxes_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmCashBoxMaster>());

    private void msbtnExpenses_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListExpenses>());

    private void msbtnCustomerReceipts_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListCustomerReceipts>());

    private void msbtnSupplierPayments_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmListSupplierPayments>());

    // ── بيانات (Data / Users) ────────────────────────────────────────────────

    private void msbtnManageUsers_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmUserMaster>());

    private void msbtnEmployees_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmEmployeeMaster>());

    // ── تقارير (Reports) ─────────────────────────────────────────────────────

    private void msbtnReports_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmReports>());

    private void msbtnReportsDashboard_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmReportsDashboard>());

    // ── الإعدادات (Settings) ──────────────────────────────────────────────────
    // الإعدادات العامة وإعدادات الشركة أُزيلتا من الواجهة — المعالجات تبقى فارغة
    // فقط لأنهما مرتبطتان في ملف الـ Designer.

    private void msbtnSettings_Click(object sender, EventArgs e)
    {
        // أُزيلت شاشة الإعدادات العامة (Settings فقط: نسخ احتياطي + تحديث)
    }

    private void msbtnCompanySettings_Click(object sender, EventArgs e)
    {
        // أُزيلت شاشة إعدادات الشركة
    }

    private void msbtnUpdateCheck_Click(object sender, EventArgs e)
        { /* V2: update-check removed */ }

    private void msbtnBackup_Click(object sender, EventArgs e)
        => clsGlobal._OpenForm(_serviceProvider.GetRequiredService<frmBackup>());

    // ── إعدادات الحساب (Logout) ──────────────────────────────────────────────

    private void msbtnLogout_Click(object sender, EventArgs e)
    {
        _sessionService.SignOut();
        Application.Restart();
    }

    // ── ملف (File) ────────────────────────────────────────────────────────────

    private void msbtnChangePassword_Click(object sender, EventArgs e)
    {
        using var frm = _serviceProvider.GetRequiredService<frmPasswordChange>();
        frm.ShowDialog(this);
    }

    private void msbtnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
        //this.Close();
    }
}
