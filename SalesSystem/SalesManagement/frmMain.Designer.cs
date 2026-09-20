using SalesManagement.Busniess;

namespace SalesManagement
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            msMainMenu = new MenuStrip();
            msBasis = new ToolStripMenuItem();
            msbtnManageItems = new ToolStripMenuItem();
            msbtnAddItem = new ToolStripMenuItem();
            msbtnUpdateItem = new ToolStripMenuItem();
            msbtnDeleteItem = new ToolStripMenuItem();
            msbtnSearchItem = new ToolStripMenuItem();
            msbtnManageSuppliers = new ToolStripMenuItem();
            msbtnAddSupplier = new ToolStripMenuItem();
            msbtnUpdateSupplier = new ToolStripMenuItem();
            msbtnDeleteSupplier = new ToolStripMenuItem();
            msbtnSearchSupplier = new ToolStripMenuItem();
            msbtnManageCustomers = new ToolStripMenuItem();
            msbtnAddCustomer = new ToolStripMenuItem();
            msbtnUpdateCustomer = new ToolStripMenuItem();
            msbtnDeleteCustomer = new ToolStripMenuItem();
            msbtnSearchCustomer = new ToolStripMenuItem();
            msbtnManageStores = new ToolStripMenuItem();
            msbtnAddStore = new ToolStripMenuItem();
            msbtnUpdateStore = new ToolStripMenuItem();
            msbtnDeleteStore = new ToolStripMenuItem();
            msbtnSearchStore = new ToolStripMenuItem();
            msbtnManageCategories = new ToolStripMenuItem();
            msbtnAddCategory = new ToolStripMenuItem();
            msbtnUpdateCategory = new ToolStripMenuItem();
            msbtnDeleteCategory = new ToolStripMenuItem();
            msbtnSearchCategory = new ToolStripMenuItem();
            msbtnManageUnits = new ToolStripMenuItem();
            msbtnAddUnit = new ToolStripMenuItem();
            msbtnUpdateUnit = new ToolStripMenuItem();
            msbtnDeleteUnit = new ToolStripMenuItem();
            msbtnSearchUnit = new ToolStripMenuItem();
            msbtnManageCompanies = new ToolStripMenuItem();
            msbtnAddCompany = new ToolStripMenuItem();
            msbtnUpdateCompany = new ToolStripMenuItem();
            msbtnDeleteCompany = new ToolStripMenuItem();
            msbtnSearchCompany = new ToolStripMenuItem();
            msbtnManageDelegates = new ToolStripMenuItem();
            msbtnAddDelegate = new ToolStripMenuItem();
            msbtnUpdateDelegate = new ToolStripMenuItem();
            msbtnDeleteDelegate = new ToolStripMenuItem();
            msbtnSearchDelegate = new ToolStripMenuItem();
            msbtnManageRevenues = new ToolStripMenuItem();
            msbtnAddRevenue = new ToolStripMenuItem();
            msbtnUpdateRevenue = new ToolStripMenuItem();
            msbtnDeleteRevenue = new ToolStripMenuItem();
            msbtnSearchRevenue = new ToolStripMenuItem();
            msbtnManageEmployees = new ToolStripMenuItem();
            msbtnAddEmployee = new ToolStripMenuItem();
            msbtnUpdateEmployee = new ToolStripMenuItem();
            msbtnDeleteEmployee = new ToolStripMenuItem();
            msbtnSearchEmployee = new ToolStripMenuItem();
            msStore = new ToolStripMenuItem();
            msbtnStartQty = new ToolStripMenuItem();
            msbtnInventory = new ToolStripMenuItem();
            msbtnNeeds = new ToolStripMenuItem();
            msbtnExpired = new ToolStripMenuItem();
            msbtnAvQty = new ToolStripMenuItem();
            msbtnEditQty = new ToolStripMenuItem();
            msbtnItemActions = new ToolStripMenuItem();
            msbtnPrintBarcode = new ToolStripMenuItem();
            msBuy = new ToolStripMenuItem();
            msbtnBuyInvoice = new ToolStripMenuItem();
            msbtnBuyReturn = new ToolStripMenuItem();
            msbtnBuyTotal = new ToolStripMenuItem();
            msbtnBuyTotalDetails = new ToolStripMenuItem();
            msbtnBuyRotation = new ToolStripMenuItem();
            msSale = new ToolStripMenuItem();
            msbtnSaleInvoice = new ToolStripMenuItem();
            msbtnSaleReturn = new ToolStripMenuItem();
            msbtnSaleTotal = new ToolStripMenuItem();
            msbtnSaleTotalDetails = new ToolStripMenuItem();
            msbtnSaleRotation = new ToolStripMenuItem();
            msRevenue = new ToolStripMenuItem();
            msbtnCashExpense = new ToolStripMenuItem();
            msbtnCashRevenueGet = new ToolStripMenuItem();
            msbtnCashRevenueReturn = new ToolStripMenuItem();
            msbtnCashRevenueActions = new ToolStripMenuItem();
            msbtnCashRevenueActionsByEmployee = new ToolStripMenuItem();
            msbtnCashRevenueTotal = new ToolStripMenuItem();
            msbtnCashRevenueTotalDetails = new ToolStripMenuItem();
            msProfit = new ToolStripMenuItem();
            msbtnProfit = new ToolStripMenuItem();
            msbtnProfitDetails = new ToolStripMenuItem();
            msbtnProfitWithoutRevenue = new ToolStripMenuItem();
            msbtnProfitBeforeRevenue = new ToolStripMenuItem();
            msbtnProfitWithoutCustmRevenue = new ToolStripMenuItem();
            msbtnProfitVirtual = new ToolStripMenuItem();
            msAccounting = new ToolStripMenuItem();
            msbtnAccountingTree = new ToolStripMenuItem();
            msbtnCreateAccountingTree = new ToolStripMenuItem();
            msbtnJournal = new ToolStripMenuItem();
            msbtnLedger = new ToolStripMenuItem();
            msbtnBalanceSheet = new ToolStripMenuItem();
            msData = new ToolStripMenuItem();
            msbtnImportExcel = new ToolStripMenuItem();
            msbtnImportAccess = new ToolStripMenuItem();
            msbtnExportExcel = new ToolStripMenuItem();
            msbtnExportAccess = new ToolStripMenuItem();
            msbtnTakeBackup = new ToolStripMenuItem();
            msbtnRestoreBackup = new ToolStripMenuItem();
            msReport = new ToolStripMenuItem();
            smbtnReportBasis = new ToolStripMenuItem();
            smbtnReportStore = new ToolStripMenuItem();
            smbtnReportBuy = new ToolStripMenuItem();
            smbtnReportSale = new ToolStripMenuItem();
            smbtnReportRevenueRevenue = new ToolStripMenuItem();
            smbtnReportProfitLoss = new ToolStripMenuItem();
            smbtnReportAccounting = new ToolStripMenuItem();
            smbtnReportProgramUse = new ToolStripMenuItem();
            msLangAndSettings = new ToolStripMenuItem();
            msbtnLang = new ToolStripMenuItem();
            msbtnBackgroundColors = new ToolStripMenuItem();
            msbtnUsers = new ToolStripMenuItem();
            msbtnPrivlleges = new ToolStripMenuItem();
            msbtnShowOpenForms = new ToolStripMenuItem();
            msAccoutSettings = new ToolStripMenuItem();
            msbtnCurrentUserInfo = new ToolStripMenuItem();
            msbtnChangePassword = new ToolStripMenuItem();
            msbtnLogout = new ToolStripMenuItem();
            msbtnAddExpense = new ToolStripMenuItem();
            msbtnUpdateExpense = new ToolStripMenuItem();
            msbtnDeleteExpense = new ToolStripMenuItem();
            msbtnSearchExpense = new ToolStripMenuItem();
            msExpense = new ToolStripMenuItem();
            msbtnCashExpenseReturn = new ToolStripMenuItem();
            msbtnCashExpenseActions = new ToolStripMenuItem();
            msbtnCashExpenseActionsByEmployee = new ToolStripMenuItem();
            msbtnCashExpenseTotal = new ToolStripMenuItem();
            msbtnCashExpenseTotalDetails = new ToolStripMenuItem();
            msbtnProfitWithoutCustmExpense = new ToolStripMenuItem();
            errorProvider1 = new ErrorProvider(components);
            msMainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // msMainMenu
            // 
            msMainMenu.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            msMainMenu.GripStyle = ToolStripGripStyle.Visible;
            msMainMenu.ImageScalingSize = new Size(24, 24);
            msMainMenu.Items.AddRange(new ToolStripItem[] { msBasis, msStore, msBuy, msSale, msRevenue, msProfit, msAccounting, msData, msReport, msLangAndSettings, msAccoutSettings });
            msMainMenu.Location = new Point(0, 0);
            msMainMenu.Name = "msMainMenu";
            msMainMenu.Size = new Size(1378, 37);
            msMainMenu.TabIndex = 0;
            msMainMenu.Text = "Main Menu";
            // 
            // msBasis
            // 
            msBasis.DropDownItems.AddRange(new ToolStripItem[] { msbtnManageItems, msbtnManageSuppliers, msbtnManageCustomers, msbtnManageStores, msbtnManageCategories, msbtnManageUnits, msbtnManageCompanies, msbtnManageDelegates, msbtnManageRevenues, msbtnManageEmployees });
            msBasis.Name = "msBasis";
            msBasis.Size = new Size(95, 33);
            msBasis.Text = "اساس";
            // 
            // msbtnManageItems
            // 
            msbtnManageItems.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddItem, msbtnUpdateItem, msbtnDeleteItem, msbtnSearchItem });
            msbtnManageItems.Name = "msbtnManageItems";
            msbtnManageItems.Size = new Size(324, 38);
            msbtnManageItems.Text = "إدارة الاصناف";
            msbtnManageItems.Click += msbtnManageItems_Click;
            // 
            // msbtnAddItem
            // 
            msbtnAddItem.Name = "msbtnAddItem";
            msbtnAddItem.Size = new Size(296, 38);
            msbtnAddItem.Text = "إضافة صنف جديد";
            msbtnAddItem.Click += msbtnAddItem_Click;
            // 
            // msbtnUpdateItem
            // 
            msbtnUpdateItem.Name = "msbtnUpdateItem";
            msbtnUpdateItem.Size = new Size(296, 38);
            msbtnUpdateItem.Text = "التعديل على صنف";
            msbtnUpdateItem.Click += msbtnUpdateItem_Click;
            // 
            // msbtnDeleteItem
            // 
            msbtnDeleteItem.Name = "msbtnDeleteItem";
            msbtnDeleteItem.Size = new Size(296, 38);
            msbtnDeleteItem.Text = "حذف صنف";
            msbtnDeleteItem.Click += msbtnDeleteItem_Click;
            // 
            // msbtnSearchItem
            // 
            msbtnSearchItem.Name = "msbtnSearchItem";
            msbtnSearchItem.Size = new Size(296, 38);
            msbtnSearchItem.Text = "البحث عن صنف";
            msbtnSearchItem.Click += msbtnSearchItem_Click;
            // 
            // msbtnManageSuppliers
            // 
            msbtnManageSuppliers.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddSupplier, msbtnUpdateSupplier, msbtnDeleteSupplier, msbtnSearchSupplier });
            msbtnManageSuppliers.Name = "msbtnManageSuppliers";
            msbtnManageSuppliers.Size = new Size(324, 38);
            msbtnManageSuppliers.Text = "إدارة الموردين";
            msbtnManageSuppliers.Click += msbtnManageSuppliers_Click;
            // 
            // msbtnAddSupplier
            // 
            msbtnAddSupplier.Name = "msbtnAddSupplier";
            msbtnAddSupplier.Size = new Size(291, 38);
            msbtnAddSupplier.Text = "إضافة مورد جديد";
            msbtnAddSupplier.Click += msbtnAddSupplier_Click;
            // 
            // msbtnUpdateSupplier
            // 
            msbtnUpdateSupplier.Name = "msbtnUpdateSupplier";
            msbtnUpdateSupplier.Size = new Size(291, 38);
            msbtnUpdateSupplier.Text = "التعديل على مورد";
            msbtnUpdateSupplier.Click += msbtnUpdateSupplier_Click;
            // 
            // msbtnDeleteSupplier
            // 
            msbtnDeleteSupplier.Name = "msbtnDeleteSupplier";
            msbtnDeleteSupplier.Size = new Size(291, 38);
            msbtnDeleteSupplier.Text = "حذف مورد";
            msbtnDeleteSupplier.Click += msbtnDeleteSupplier_Click;
            // 
            // msbtnSearchSupplier
            // 
            msbtnSearchSupplier.Name = "msbtnSearchSupplier";
            msbtnSearchSupplier.Size = new Size(291, 38);
            msbtnSearchSupplier.Text = "البحث عن مورد";
            msbtnSearchSupplier.Click += msbtnSearchSupplier_Click;
            // 
            // msbtnManageCustomers
            // 
            msbtnManageCustomers.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddCustomer, msbtnUpdateCustomer, msbtnDeleteCustomer, msbtnSearchCustomer });
            msbtnManageCustomers.Name = "msbtnManageCustomers";
            msbtnManageCustomers.Size = new Size(324, 38);
            msbtnManageCustomers.Text = "إدارة العملاء";
            msbtnManageCustomers.Click += msbtnManageCustomers_Click;
            // 
            // msbtnAddCustomer
            // 
            msbtnAddCustomer.Name = "msbtnAddCustomer";
            msbtnAddCustomer.Size = new Size(302, 38);
            msbtnAddCustomer.Text = "إضافة عميل جديد";
            msbtnAddCustomer.Click += msbtnAddCustomer_Click;
            // 
            // msbtnUpdateCustomer
            // 
            msbtnUpdateCustomer.Name = "msbtnUpdateCustomer";
            msbtnUpdateCustomer.Size = new Size(302, 38);
            msbtnUpdateCustomer.Text = "التعديل على عميل";
            msbtnUpdateCustomer.Click += msbtnUpdateCustomer_Click;
            // 
            // msbtnDeleteCustomer
            // 
            msbtnDeleteCustomer.Name = "msbtnDeleteCustomer";
            msbtnDeleteCustomer.Size = new Size(302, 38);
            msbtnDeleteCustomer.Text = "حذف العميل";
            msbtnDeleteCustomer.Click += msbtnDeleteCustomer_Click;
            // 
            // msbtnSearchCustomer
            // 
            msbtnSearchCustomer.Name = "msbtnSearchCustomer";
            msbtnSearchCustomer.Size = new Size(302, 38);
            msbtnSearchCustomer.Text = "البحث عن عميل";
            msbtnSearchCustomer.Click += msbtnSearchCustomer_Click;
            // 
            // msbtnManageStores
            // 
            msbtnManageStores.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddStore, msbtnUpdateStore, msbtnDeleteStore, msbtnSearchStore });
            msbtnManageStores.Name = "msbtnManageStores";
            msbtnManageStores.Size = new Size(324, 38);
            msbtnManageStores.Text = "إدارة المخازن";
            msbtnManageStores.Click += msbtnManageStores_Click;
            // 
            // msbtnAddStore
            // 
            msbtnAddStore.Name = "msbtnAddStore";
            msbtnAddStore.Size = new Size(302, 38);
            msbtnAddStore.Text = "إضافة مخزن جديد";
            msbtnAddStore.Click += msbtnAddStore_Click;
            // 
            // msbtnUpdateStore
            // 
            msbtnUpdateStore.Name = "msbtnUpdateStore";
            msbtnUpdateStore.Size = new Size(302, 38);
            msbtnUpdateStore.Text = "التعديل على مخزن";
            msbtnUpdateStore.Click += msbtnUpdateStore_Click;
            // 
            // msbtnDeleteStore
            // 
            msbtnDeleteStore.Name = "msbtnDeleteStore";
            msbtnDeleteStore.Size = new Size(302, 38);
            msbtnDeleteStore.Text = "حذف مخزن";
            msbtnDeleteStore.Click += msbtnDeleteStore_Click;
            // 
            // msbtnSearchStore
            // 
            msbtnSearchStore.Name = "msbtnSearchStore";
            msbtnSearchStore.Size = new Size(302, 38);
            msbtnSearchStore.Text = "البحث عن مخزن";
            msbtnSearchStore.Click += msbtnSearchStore_Click;
            // 
            // msbtnManageCategories
            // 
            msbtnManageCategories.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddCategory, msbtnUpdateCategory, msbtnDeleteCategory, msbtnSearchCategory });
            msbtnManageCategories.Name = "msbtnManageCategories";
            msbtnManageCategories.Size = new Size(324, 38);
            msbtnManageCategories.Text = "إدارة التصنيفات";
            msbtnManageCategories.Click += msbtnManageCategories_Click;
            // 
            // msbtnAddCategory
            // 
            msbtnAddCategory.Name = "msbtnAddCategory";
            msbtnAddCategory.Size = new Size(311, 38);
            msbtnAddCategory.Text = "إضافة تصنيف جديد";
            msbtnAddCategory.Click += msbtnAddCategory_Click;
            // 
            // msbtnUpdateCategory
            // 
            msbtnUpdateCategory.Name = "msbtnUpdateCategory";
            msbtnUpdateCategory.Size = new Size(311, 38);
            msbtnUpdateCategory.Text = "التعديل على تصنيف";
            msbtnUpdateCategory.Click += msbtnUpdateCategory_Click;
            // 
            // msbtnDeleteCategory
            // 
            msbtnDeleteCategory.Name = "msbtnDeleteCategory";
            msbtnDeleteCategory.Size = new Size(311, 38);
            msbtnDeleteCategory.Text = "حذف التصنيف";
            msbtnDeleteCategory.Click += msbtnDeleteCategory_Click;
            // 
            // msbtnSearchCategory
            // 
            msbtnSearchCategory.Name = "msbtnSearchCategory";
            msbtnSearchCategory.Size = new Size(311, 38);
            msbtnSearchCategory.Text = "البحث عن تصنيف";
            msbtnSearchCategory.Click += msbtnSearchCategory_Click;
            // 
            // msbtnManageUnits
            // 
            msbtnManageUnits.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddUnit, msbtnUpdateUnit, msbtnDeleteUnit, msbtnSearchUnit });
            msbtnManageUnits.Name = "msbtnManageUnits";
            msbtnManageUnits.Size = new Size(324, 38);
            msbtnManageUnits.Text = "إدارة الوحدات";
            msbtnManageUnits.Click += msbtnManageUnits_Click;
            // 
            // msbtnAddUnit
            // 
            msbtnAddUnit.Name = "msbtnAddUnit";
            msbtnAddUnit.Size = new Size(298, 38);
            msbtnAddUnit.Text = "إضافة وحدة جديد";
            msbtnAddUnit.Click += msbtnAddUnit_Click;
            // 
            // msbtnUpdateUnit
            // 
            msbtnUpdateUnit.Name = "msbtnUpdateUnit";
            msbtnUpdateUnit.Size = new Size(298, 38);
            msbtnUpdateUnit.Text = "التعديل على وحدة";
            msbtnUpdateUnit.Click += msbtnUpdateUnit_Click;
            // 
            // msbtnDeleteUnit
            // 
            msbtnDeleteUnit.Name = "msbtnDeleteUnit";
            msbtnDeleteUnit.Size = new Size(298, 38);
            msbtnDeleteUnit.Text = "حذف وحدة";
            msbtnDeleteUnit.Click += msbtnDeleteUnit_Click;
            // 
            // msbtnSearchUnit
            // 
            msbtnSearchUnit.Name = "msbtnSearchUnit";
            msbtnSearchUnit.Size = new Size(298, 38);
            msbtnSearchUnit.Text = "البحث عن وحدة";
            msbtnSearchUnit.Click += msbtnSearchUnit_Click;
            // 
            // msbtnManageCompanies
            // 
            msbtnManageCompanies.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddCompany, msbtnUpdateCompany, msbtnDeleteCompany, msbtnSearchCompany });
            msbtnManageCompanies.Name = "msbtnManageCompanies";
            msbtnManageCompanies.Size = new Size(324, 38);
            msbtnManageCompanies.Text = "إدارة شركات المنشاء";
            msbtnManageCompanies.Click += msbtnManageCompanies_Click;
            // 
            // msbtnAddCompany
            // 
            msbtnAddCompany.Name = "msbtnAddCompany";
            msbtnAddCompany.Size = new Size(308, 38);
            msbtnAddCompany.Text = "إضافة شركة جديد";
            msbtnAddCompany.Click += msbtnAddCompany_Click;
            // 
            // msbtnUpdateCompany
            // 
            msbtnUpdateCompany.Name = "msbtnUpdateCompany";
            msbtnUpdateCompany.Size = new Size(308, 38);
            msbtnUpdateCompany.Text = "التعديل على شركة";
            msbtnUpdateCompany.Click += msbtnUpdateCompany_Click;
            // 
            // msbtnDeleteCompany
            // 
            msbtnDeleteCompany.Name = "msbtnDeleteCompany";
            msbtnDeleteCompany.Size = new Size(308, 38);
            msbtnDeleteCompany.Text = "حذف الشركة";
            msbtnDeleteCompany.Click += msbtnDeleteCompany_Click;
            // 
            // msbtnSearchCompany
            // 
            msbtnSearchCompany.Name = "msbtnSearchCompany";
            msbtnSearchCompany.Size = new Size(308, 38);
            msbtnSearchCompany.Text = "البحث عن شركة";
            msbtnSearchCompany.Click += msbtnSearchCompany_Click;
            // 
            // msbtnManageDelegates
            // 
            msbtnManageDelegates.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddDelegate, msbtnUpdateDelegate, msbtnDeleteDelegate, msbtnSearchDelegate });
            msbtnManageDelegates.Name = "msbtnManageDelegates";
            msbtnManageDelegates.Size = new Size(324, 38);
            msbtnManageDelegates.Text = "إدارة المناديب";
            msbtnManageDelegates.Click += msbtnManageDelegates_Click;
            // 
            // msbtnAddDelegate
            // 
            msbtnAddDelegate.Name = "msbtnAddDelegate";
            msbtnAddDelegate.Size = new Size(310, 38);
            msbtnAddDelegate.Text = "إضافة مندوب جديد";
            msbtnAddDelegate.Click += msbtnAddDelegate_Click;
            // 
            // msbtnUpdateDelegate
            // 
            msbtnUpdateDelegate.Name = "msbtnUpdateDelegate";
            msbtnUpdateDelegate.Size = new Size(310, 38);
            msbtnUpdateDelegate.Text = "التعديل على مندوب";
            msbtnUpdateDelegate.Click += msbtnUpdateDelegate_Click;
            // 
            // msbtnDeleteDelegate
            // 
            msbtnDeleteDelegate.Name = "msbtnDeleteDelegate";
            msbtnDeleteDelegate.Size = new Size(310, 38);
            msbtnDeleteDelegate.Text = "حذف مندوب";
            msbtnDeleteDelegate.Click += msbtnDeleteDelegate_Click;
            // 
            // msbtnSearchDelegate
            // 
            msbtnSearchDelegate.Name = "msbtnSearchDelegate";
            msbtnSearchDelegate.Size = new Size(310, 38);
            msbtnSearchDelegate.Text = "البحث عن مندوب";
            msbtnSearchDelegate.Click += msbtnSearchDelegate_Click;
            // 
            // msbtnManageRevenues
            // 
            msbtnManageRevenues.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddRevenue, msbtnUpdateRevenue, msbtnDeleteRevenue, msbtnSearchRevenue });
            msbtnManageRevenues.Name = "msbtnManageRevenues";
            msbtnManageRevenues.Size = new Size(324, 38);
            msbtnManageRevenues.Text = "إدارة الايرادات";
            msbtnManageRevenues.Click += msbtnManageRevenues_Click;
            // 
            // msbtnAddRevenue
            // 
            msbtnAddRevenue.Name = "msbtnAddRevenue";
            msbtnAddRevenue.Size = new Size(288, 38);
            msbtnAddRevenue.Text = "إضافة ايراد جديد";
            msbtnAddRevenue.Click += msbtnAddRevenue_Click;
            // 
            // msbtnUpdateRevenue
            // 
            msbtnUpdateRevenue.Name = "msbtnUpdateRevenue";
            msbtnUpdateRevenue.Size = new Size(288, 38);
            msbtnUpdateRevenue.Text = "التعديل على ايراد";
            msbtnUpdateRevenue.Click += msbtnUpdateRevenue_Click;
            // 
            // msbtnDeleteRevenue
            // 
            msbtnDeleteRevenue.Name = "msbtnDeleteRevenue";
            msbtnDeleteRevenue.Size = new Size(288, 38);
            msbtnDeleteRevenue.Text = "حذف ايراد";
            msbtnDeleteRevenue.Click += msbtnDeleteRevenue_Click;
            // 
            // msbtnSearchRevenue
            // 
            msbtnSearchRevenue.Name = "msbtnSearchRevenue";
            msbtnSearchRevenue.Size = new Size(288, 38);
            msbtnSearchRevenue.Text = "البحث عن ايراد";
            msbtnSearchRevenue.Click += msbtnSearchRevenue_Click;
            // 
            // msbtnManageEmployees
            // 
            msbtnManageEmployees.DropDownItems.AddRange(new ToolStripItem[] { msbtnAddEmployee, msbtnUpdateEmployee, msbtnDeleteEmployee, msbtnSearchEmployee });
            msbtnManageEmployees.Name = "msbtnManageEmployees";
            msbtnManageEmployees.Size = new Size(324, 38);
            msbtnManageEmployees.Text = "إدارة الموظفين";
            msbtnManageEmployees.Click += msbtnManageEmployees_Click;
            // 
            // msbtnAddEmployee
            // 
            msbtnAddEmployee.Name = "msbtnAddEmployee";
            msbtnAddEmployee.Size = new Size(309, 38);
            msbtnAddEmployee.Text = "إضافة موظف جديد";
            msbtnAddEmployee.Click += msbtnAddEmployee_Click;
            // 
            // msbtnUpdateEmployee
            // 
            msbtnUpdateEmployee.Name = "msbtnUpdateEmployee";
            msbtnUpdateEmployee.Size = new Size(309, 38);
            msbtnUpdateEmployee.Text = "التعديل على موظف";
            msbtnUpdateEmployee.Click += msbtnUpdateEmployee_Click;
            // 
            // msbtnDeleteEmployee
            // 
            msbtnDeleteEmployee.Name = "msbtnDeleteEmployee";
            msbtnDeleteEmployee.Size = new Size(309, 38);
            msbtnDeleteEmployee.Text = "خذف الموظف";
            msbtnDeleteEmployee.Click += msbtnDeleteEmployee_Click;
            // 
            // msbtnSearchEmployee
            // 
            msbtnSearchEmployee.Name = "msbtnSearchEmployee";
            msbtnSearchEmployee.Size = new Size(309, 38);
            msbtnSearchEmployee.Text = "البحث عن موظف";
            msbtnSearchEmployee.Click += msbtnSearchEmployee_Click;
            // 
            // msStore
            // 
            msStore.DropDownItems.AddRange(new ToolStripItem[] { msbtnStartQty, msbtnInventory, msbtnNeeds, msbtnExpired, msbtnAvQty, msbtnEditQty, msbtnItemActions, msbtnPrintBarcode });
            msStore.Name = "msStore";
            msStore.Size = new Size(83, 33);
            msStore.Text = "مخزن";
            // 
            // msbtnStartQty
            // 
            msbtnStartQty.Name = "msbtnStartQty";
            msbtnStartQty.Size = new Size(288, 38);
            msbtnStartQty.Text = "الارصدة الافتتاحية";
            // 
            // msbtnInventory
            // 
            msbtnInventory.Name = "msbtnInventory";
            msbtnInventory.Size = new Size(288, 38);
            msbtnInventory.Text = "الجرد";
            // 
            // msbtnNeeds
            // 
            msbtnNeeds.Name = "msbtnNeeds";
            msbtnNeeds.Size = new Size(288, 38);
            msbtnNeeds.Text = "النواقص";
            // 
            // msbtnExpired
            // 
            msbtnExpired.Name = "msbtnExpired";
            msbtnExpired.Size = new Size(288, 38);
            msbtnExpired.Text = "تواريخ الصلاحية";
            // 
            // msbtnAvQty
            // 
            msbtnAvQty.Name = "msbtnAvQty";
            msbtnAvQty.Size = new Size(288, 38);
            msbtnAvQty.Text = "الكميات المتاحة";
            // 
            // msbtnEditQty
            // 
            msbtnEditQty.Name = "msbtnEditQty";
            msbtnEditQty.Size = new Size(288, 38);
            msbtnEditQty.Text = "تعديل الكميات";
            // 
            // msbtnItemActions
            // 
            msbtnItemActions.Name = "msbtnItemActions";
            msbtnItemActions.Size = new Size(288, 38);
            msbtnItemActions.Text = "حركات الصنف";
            // 
            // msbtnPrintBarcode
            // 
            msbtnPrintBarcode.Name = "msbtnPrintBarcode";
            msbtnPrintBarcode.Size = new Size(288, 38);
            msbtnPrintBarcode.Text = "طباعة الباركود";
            // 
            // msBuy
            // 
            msBuy.DropDownItems.AddRange(new ToolStripItem[] { msbtnBuyInvoice, msbtnBuyReturn, msbtnBuyTotal, msbtnBuyTotalDetails, msbtnBuyRotation });
            msBuy.Name = "msBuy";
            msBuy.Size = new Size(78, 33);
            msBuy.Text = "شراء";
            // 
            // msbtnBuyInvoice
            // 
            msbtnBuyInvoice.Name = "msbtnBuyInvoice";
            msbtnBuyInvoice.Size = new Size(380, 38);
            msbtnBuyInvoice.Text = "فاتورة شراء";
            // 
            // msbtnBuyReturn
            // 
            msbtnBuyReturn.Name = "msbtnBuyReturn";
            msbtnBuyReturn.Size = new Size(380, 38);
            msbtnBuyReturn.Text = "مرتجع شراء";
            // 
            // msbtnBuyTotal
            // 
            msbtnBuyTotal.Name = "msbtnBuyTotal";
            msbtnBuyTotal.Size = new Size(380, 38);
            msbtnBuyTotal.Text = "اجمالي المشتريات";
            // 
            // msbtnBuyTotalDetails
            // 
            msbtnBuyTotalDetails.Name = "msbtnBuyTotalDetails";
            msbtnBuyTotalDetails.Size = new Size(380, 38);
            msbtnBuyTotalDetails.Text = "تفاصيل اجمالي المشتريات";
            // 
            // msbtnBuyRotation
            // 
            msbtnBuyRotation.Name = "msbtnBuyRotation";
            msbtnBuyRotation.Size = new Size(380, 38);
            msbtnBuyRotation.Text = "معدل دوران الشراء";
            // 
            // msSale
            // 
            msSale.DropDownItems.AddRange(new ToolStripItem[] { msbtnSaleInvoice, msbtnSaleReturn, msbtnSaleTotal, msbtnSaleTotalDetails, msbtnSaleRotation });
            msSale.Name = "msSale";
            msSale.Size = new Size(58, 33);
            msSale.Text = "بيع";
            // 
            // msbtnSaleInvoice
            // 
            msbtnSaleInvoice.Name = "msbtnSaleInvoice";
            msbtnSaleInvoice.Size = new Size(358, 38);
            msbtnSaleInvoice.Text = "فاتورة بيع";
            // 
            // msbtnSaleReturn
            // 
            msbtnSaleReturn.Name = "msbtnSaleReturn";
            msbtnSaleReturn.Size = new Size(358, 38);
            msbtnSaleReturn.Text = "مرتجع بيع";
            // 
            // msbtnSaleTotal
            // 
            msbtnSaleTotal.Name = "msbtnSaleTotal";
            msbtnSaleTotal.Size = new Size(358, 38);
            msbtnSaleTotal.Text = "اجمالي المبيعات";
            // 
            // msbtnSaleTotalDetails
            // 
            msbtnSaleTotalDetails.Name = "msbtnSaleTotalDetails";
            msbtnSaleTotalDetails.Size = new Size(358, 38);
            msbtnSaleTotalDetails.Text = "تفاصيل اجمالي المبيعات";
            // 
            // msbtnSaleRotation
            // 
            msbtnSaleRotation.Name = "msbtnSaleRotation";
            msbtnSaleRotation.Size = new Size(358, 38);
            msbtnSaleRotation.Text = "معدل دوران البيع";
            // 
            // msRevenue
            // 
            msRevenue.DropDownItems.AddRange(new ToolStripItem[] { msbtnCashExpense, msbtnCashRevenueGet, msbtnCashRevenueReturn, msbtnCashRevenueActions, msbtnCashRevenueActionsByEmployee, msbtnCashRevenueTotal, msbtnCashRevenueTotalDetails });
            msRevenue.Name = "msRevenue";
            msRevenue.Size = new Size(69, 33);
            msRevenue.Text = "إيراد";
            // 
            // msbtnCashExpense
            // 
            msbtnCashExpense.Name = "msbtnCashExpense";
            msbtnCashExpense.Size = new Size(418, 38);
            msbtnCashExpense.Text = "صرف نقدية";
            // 
            // msbtnCashRevenueGet
            // 
            msbtnCashRevenueGet.Name = "msbtnCashRevenueGet";
            msbtnCashRevenueGet.Size = new Size(418, 38);
            msbtnCashRevenueGet.Text = "توريد نقدية";
            // 
            // msbtnCashRevenueReturn
            // 
            msbtnCashRevenueReturn.Name = "msbtnCashRevenueReturn";
            msbtnCashRevenueReturn.Size = new Size(418, 38);
            msbtnCashRevenueReturn.Text = "ارجاع توريد";
            // 
            // msbtnCashRevenueActions
            // 
            msbtnCashRevenueActions.Name = "msbtnCashRevenueActions";
            msbtnCashRevenueActions.Size = new Size(418, 38);
            msbtnCashRevenueActions.Text = "حركة التوريدات";
            // 
            // msbtnCashRevenueActionsByEmployee
            // 
            msbtnCashRevenueActionsByEmployee.Name = "msbtnCashRevenueActionsByEmployee";
            msbtnCashRevenueActionsByEmployee.Size = new Size(418, 38);
            msbtnCashRevenueActionsByEmployee.Text = "حركة التوريدات حسب الموظف";
            // 
            // msbtnCashRevenueTotal
            // 
            msbtnCashRevenueTotal.Name = "msbtnCashRevenueTotal";
            msbtnCashRevenueTotal.Size = new Size(418, 38);
            msbtnCashRevenueTotal.Text = "اجمالي الإرادات";
            // 
            // msbtnCashRevenueTotalDetails
            // 
            msbtnCashRevenueTotalDetails.Name = "msbtnCashRevenueTotalDetails";
            msbtnCashRevenueTotalDetails.Size = new Size(418, 38);
            msbtnCashRevenueTotalDetails.Text = "تفاصيل اجمالي الإرادات";
            // 
            // msProfit
            // 
            msProfit.DropDownItems.AddRange(new ToolStripItem[] { msbtnProfit, msbtnProfitDetails, msbtnProfitWithoutRevenue, msbtnProfitBeforeRevenue, msbtnProfitWithoutCustmRevenue, msbtnProfitVirtual });
            msProfit.Name = "msProfit";
            msProfit.Size = new Size(62, 33);
            msProfit.Text = "ربح";
            // 
            // msbtnProfit
            // 
            msbtnProfit.Name = "msbtnProfit";
            msbtnProfit.Size = new Size(323, 38);
            msbtnProfit.Text = "صافي الربح";
            // 
            // msbtnProfitDetails
            // 
            msbtnProfitDetails.Name = "msbtnProfitDetails";
            msbtnProfitDetails.Size = new Size(323, 38);
            msbtnProfitDetails.Text = "تفاصيل صافي الربح";
            // 
            // msbtnProfitWithoutRevenue
            // 
            msbtnProfitWithoutRevenue.Name = "msbtnProfitWithoutRevenue";
            msbtnProfitWithoutRevenue.Size = new Size(323, 38);
            msbtnProfitWithoutRevenue.Text = "الربح بدون الإرادات";
            // 
            // msbtnProfitBeforeRevenue
            // 
            msbtnProfitBeforeRevenue.Name = "msbtnProfitBeforeRevenue";
            msbtnProfitBeforeRevenue.Size = new Size(323, 38);
            msbtnProfitBeforeRevenue.Text = "الربح قبل المصروفات";
            // 
            // msbtnProfitWithoutCustmRevenue
            // 
            msbtnProfitWithoutCustmRevenue.Name = "msbtnProfitWithoutCustmRevenue";
            msbtnProfitWithoutCustmRevenue.Size = new Size(323, 38);
            msbtnProfitWithoutCustmRevenue.Text = "الربح بدون إيراد معين";
            // 
            // msbtnProfitVirtual
            // 
            msbtnProfitVirtual.Name = "msbtnProfitVirtual";
            msbtnProfitVirtual.Size = new Size(323, 38);
            msbtnProfitVirtual.Text = "افتراض ربح وهمي";
            // 
            // msAccounting
            // 
            msAccounting.DropDownItems.AddRange(new ToolStripItem[] { msbtnAccountingTree, msbtnCreateAccountingTree, msbtnJournal, msbtnLedger, msbtnBalanceSheet });
            msAccounting.Name = "msAccounting";
            msAccounting.Size = new Size(110, 33);
            msAccounting.Text = "حسابات";
            // 
            // msbtnAccountingTree
            // 
            msbtnAccountingTree.Name = "msbtnAccountingTree";
            msbtnAccountingTree.Size = new Size(327, 38);
            msbtnAccountingTree.Text = "فروع الحسابات";
            // 
            // msbtnCreateAccountingTree
            // 
            msbtnCreateAccountingTree.Name = "msbtnCreateAccountingTree";
            msbtnCreateAccountingTree.Size = new Size(327, 38);
            msbtnCreateAccountingTree.Text = "تكوين فروع الحسابات";
            // 
            // msbtnJournal
            // 
            msbtnJournal.Name = "msbtnJournal";
            msbtnJournal.Size = new Size(327, 38);
            msbtnJournal.Text = "دفتر اليومية";
            // 
            // msbtnLedger
            // 
            msbtnLedger.Name = "msbtnLedger";
            msbtnLedger.Size = new Size(327, 38);
            msbtnLedger.Text = "دفتر الأستاذ";
            // 
            // msbtnBalanceSheet
            // 
            msbtnBalanceSheet.Name = "msbtnBalanceSheet";
            msbtnBalanceSheet.Size = new Size(327, 38);
            msbtnBalanceSheet.Text = "الميزانية العمومية";
            // 
            // msData
            // 
            msData.DropDownItems.AddRange(new ToolStripItem[] { msbtnImportExcel, msbtnImportAccess, msbtnExportExcel, msbtnExportAccess, msbtnTakeBackup, msbtnRestoreBackup });
            msData.Name = "msData";
            msData.Size = new Size(84, 33);
            msData.Text = "بيانات";
            // 
            // msbtnImportExcel
            // 
            msbtnImportExcel.Name = "msbtnImportExcel";
            msbtnImportExcel.Size = new Size(372, 38);
            msbtnImportExcel.Text = "استيراد بيانات من اكسيل";
            // 
            // msbtnImportAccess
            // 
            msbtnImportAccess.Name = "msbtnImportAccess";
            msbtnImportAccess.Size = new Size(372, 38);
            msbtnImportAccess.Text = "استيراد بيانات من اكسس";
            // 
            // msbtnExportExcel
            // 
            msbtnExportExcel.Name = "msbtnExportExcel";
            msbtnExportExcel.Size = new Size(372, 38);
            msbtnExportExcel.Text = "تصدير بيانات إلى اكسيل";
            // 
            // msbtnExportAccess
            // 
            msbtnExportAccess.Name = "msbtnExportAccess";
            msbtnExportAccess.Size = new Size(372, 38);
            msbtnExportAccess.Text = "تصدير بيانات إلى اكسس";
            // 
            // msbtnTakeBackup
            // 
            msbtnTakeBackup.Name = "msbtnTakeBackup";
            msbtnTakeBackup.Size = new Size(372, 38);
            msbtnTakeBackup.Text = "اخذ نسخة احتياطية";
            // 
            // msbtnRestoreBackup
            // 
            msbtnRestoreBackup.Name = "msbtnRestoreBackup";
            msbtnRestoreBackup.Size = new Size(372, 38);
            msbtnRestoreBackup.Text = "استرجاع نسخة احتياطية";
            // 
            // msReport
            // 
            msReport.DropDownItems.AddRange(new ToolStripItem[] { smbtnReportBasis, smbtnReportStore, smbtnReportBuy, smbtnReportSale, smbtnReportRevenueRevenue, smbtnReportProfitLoss, smbtnReportAccounting, smbtnReportProgramUse });
            msReport.Name = "msReport";
            msReport.Size = new Size(81, 33);
            msReport.Text = "تقارير";
            // 
            // smbtnReportBasis
            // 
            smbtnReportBasis.Name = "smbtnReportBasis";
            smbtnReportBasis.Size = new Size(369, 38);
            smbtnReportBasis.Text = "تقارير الأساس";
            // 
            // smbtnReportStore
            // 
            smbtnReportStore.Name = "smbtnReportStore";
            smbtnReportStore.Size = new Size(369, 38);
            smbtnReportStore.Text = "تقارير المخازن";
            // 
            // smbtnReportBuy
            // 
            smbtnReportBuy.Name = "smbtnReportBuy";
            smbtnReportBuy.Size = new Size(369, 38);
            smbtnReportBuy.Text = "تقارير المشتريات";
            // 
            // smbtnReportSale
            // 
            smbtnReportSale.Name = "smbtnReportSale";
            smbtnReportSale.Size = new Size(369, 38);
            smbtnReportSale.Text = "تقارير المبيعات";
            // 
            // smbtnReportRevenueRevenue
            // 
            smbtnReportRevenueRevenue.Name = "smbtnReportRevenueRevenue";
            smbtnReportRevenueRevenue.Size = new Size(369, 38);
            smbtnReportRevenueRevenue.Text = "تقارير الإرادات والمصروفات";
            // 
            // smbtnReportProfitLoss
            // 
            smbtnReportProfitLoss.Name = "smbtnReportProfitLoss";
            smbtnReportProfitLoss.Size = new Size(369, 38);
            smbtnReportProfitLoss.Text = "تقارير الأرباح والخسائر";
            // 
            // smbtnReportAccounting
            // 
            smbtnReportAccounting.Name = "smbtnReportAccounting";
            smbtnReportAccounting.Size = new Size(369, 38);
            smbtnReportAccounting.Text = "تقارير الحسابات";
            // 
            // smbtnReportProgramUse
            // 
            smbtnReportProgramUse.Name = "smbtnReportProgramUse";
            smbtnReportProgramUse.Size = new Size(369, 38);
            smbtnReportProgramUse.Text = "تقارير استخدامات البرنامج";
            // 
            // msLangAndSettings
            // 
            msLangAndSettings.DropDownItems.AddRange(new ToolStripItem[] { msbtnLang, msbtnBackgroundColors, msbtnUsers, msbtnPrivlleges, msbtnShowOpenForms });
            msLangAndSettings.Name = "msLangAndSettings";
            msLangAndSettings.Size = new Size(192, 33);
            msLangAndSettings.Text = "اللغة و الإعدادات";
            // 
            // msbtnLang
            // 
            msbtnLang.Name = "msbtnLang";
            msbtnLang.Size = new Size(368, 38);
            msbtnLang.Text = "اللغة";
            // 
            // msbtnBackgroundColors
            // 
            msbtnBackgroundColors.Name = "msbtnBackgroundColors";
            msbtnBackgroundColors.Size = new Size(368, 38);
            msbtnBackgroundColors.Text = "لون الخلفية";
            // 
            // msbtnUsers
            // 
            msbtnUsers.Name = "msbtnUsers";
            msbtnUsers.Size = new Size(368, 38);
            msbtnUsers.Text = "المستخدمين";
            // 
            // msbtnPrivlleges
            // 
            msbtnPrivlleges.Name = "msbtnPrivlleges";
            msbtnPrivlleges.Size = new Size(368, 38);
            msbtnPrivlleges.Text = "الصلاحيات";
            // 
            // msbtnShowOpenForms
            // 
            msbtnShowOpenForms.Name = "msbtnShowOpenForms";
            msbtnShowOpenForms.Size = new Size(368, 38);
            msbtnShowOpenForms.Text = "عرض الشاشات المفتوحة";
            // 
            // msAccoutSettings
            // 
            msAccoutSettings.DropDownItems.AddRange(new ToolStripItem[] { msbtnCurrentUserInfo, msbtnChangePassword, msbtnLogout });
            msAccoutSettings.Name = "msAccoutSettings";
            msAccoutSettings.Size = new Size(192, 33);
            msAccoutSettings.Text = "اعدادات الحساب";
            // 
            // msbtnCurrentUserInfo
            // 
            msbtnCurrentUserInfo.Name = "msbtnCurrentUserInfo";
            msbtnCurrentUserInfo.Size = new Size(384, 38);
            msbtnCurrentUserInfo.Text = "معلومات المستخدم الحالي";
            // 
            // msbtnChangePassword
            // 
            msbtnChangePassword.Name = "msbtnChangePassword";
            msbtnChangePassword.Size = new Size(384, 38);
            msbtnChangePassword.Text = "تغيير كلمة السر";
            // 
            // msbtnLogout
            // 
            msbtnLogout.Name = "msbtnLogout";
            msbtnLogout.Size = new Size(384, 38);
            msbtnLogout.Text = "تسجيل الخروج";
            msbtnLogout.Click += msbtnLogout_Click;
            // 
            // msbtnAddExpense
            // 
            msbtnAddExpense.Name = "msbtnAddExpense";
            msbtnAddExpense.Size = new Size(32, 19);
            // 
            // msbtnUpdateExpense
            // 
            msbtnUpdateExpense.Name = "msbtnUpdateExpense";
            msbtnUpdateExpense.Size = new Size(32, 19);
            // 
            // msbtnDeleteExpense
            // 
            msbtnDeleteExpense.Name = "msbtnDeleteExpense";
            msbtnDeleteExpense.Size = new Size(32, 19);
            // 
            // msbtnSearchExpense
            // 
            msbtnSearchExpense.Name = "msbtnSearchExpense";
            msbtnSearchExpense.Size = new Size(32, 19);
            // 
            // msExpense
            // 
            msExpense.Name = "msExpense";
            msExpense.Size = new Size(32, 19);
            // 
            // msbtnCashExpenseReturn
            // 
            msbtnCashExpenseReturn.Name = "msbtnCashExpenseReturn";
            msbtnCashExpenseReturn.Size = new Size(32, 19);
            // 
            // msbtnCashExpenseActions
            // 
            msbtnCashExpenseActions.Name = "msbtnCashExpenseActions";
            msbtnCashExpenseActions.Size = new Size(32, 19);
            // 
            // msbtnCashExpenseActionsByEmployee
            // 
            msbtnCashExpenseActionsByEmployee.Name = "msbtnCashExpenseActionsByEmployee";
            msbtnCashExpenseActionsByEmployee.Size = new Size(32, 19);
            // 
            // msbtnCashExpenseTotal
            // 
            msbtnCashExpenseTotal.Name = "msbtnCashExpenseTotal";
            msbtnCashExpenseTotal.Size = new Size(32, 19);
            // 
            // msbtnCashExpenseTotalDetails
            // 
            msbtnCashExpenseTotalDetails.Name = "msbtnCashExpenseTotalDetails";
            msbtnCashExpenseTotalDetails.Size = new Size(32, 19);
            // 
            // msbtnProfitWithoutCustmExpense
            // 
            msbtnProfitWithoutCustmExpense.Name = "msbtnProfitWithoutCustmExpense";
            msbtnProfitWithoutCustmExpense.Size = new Size(32, 19);
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            errorProvider1.RightToLeft = true;
            // 
            // frmMain
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.LightYellow;
            ClientSize = new Size(1378, 944);
            Controls.Add(msMainMenu);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.ControlText;
            IsMdiContainer = true;
            MainMenuStrip = msMainMenu;
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmMain";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "الشاشه الرئيسيه";
            WindowState = FormWindowState.Maximized;
            Load += frmMain_Load;
            msMainMenu.ResumeLayout(false);
            msMainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip msMainMenu;
        private ToolStripMenuItem msBasis;
        private ToolStripMenuItem msStore;
        private ToolStripMenuItem msBuy;
        private ToolStripMenuItem msbtnManageItems;
        private ToolStripMenuItem msbtnManageSuppliers;
        private ToolStripMenuItem msbtnManageCustomers;
        private ToolStripMenuItem msbtnManageStores;
        private ToolStripMenuItem msbtnManageCategories;
        private ToolStripMenuItem msbtnManageUnits;
        private ToolStripMenuItem msbtnManageCompanies;
        private ToolStripMenuItem msbtnManageDelegates;
        private ToolStripMenuItem msbtnExpense;
        private ToolStripMenuItem msbtnManageRevenues;
        private ToolStripMenuItem msbtnStartQty;
        private ToolStripMenuItem msbtnInventory;
        private ToolStripMenuItem msbtnNeeds;
        private ToolStripMenuItem msbtnExpired;
        private ToolStripMenuItem msbtnAvQty;
        private ToolStripMenuItem msbtnEditQty;
        private ToolStripMenuItem msbtnItemActions;
        private ToolStripMenuItem msbtnPrintBarcode;
        private ToolStripMenuItem msbtnBuyInvoice;
        private ToolStripMenuItem msbtnBuyReturn;
        private ToolStripMenuItem msbtnBuyTotal;
        private ToolStripMenuItem msbtnBuyRotation;
        private ToolStripMenuItem msbtnBuyTotalDetails;
        private ToolStripMenuItem msSale;
        private ToolStripMenuItem msbtnSaleInvoice;
        private ToolStripMenuItem msbtnSaleReturn;
        private ToolStripMenuItem msbtnSaleTotal;
        private ToolStripMenuItem msbtnSaleTotalDetails;
        private ToolStripMenuItem msbtnSaleRotation;
        private ToolStripMenuItem msbtnManageEmployees;
        private ToolStripMenuItem msExpense;
        private ToolStripMenuItem msbtnCashExpense;
        private ToolStripMenuItem msbtnCashExpenseReturn;
        private ToolStripMenuItem msbtnCashExpenseActions;
        private ToolStripMenuItem msbtnCashExpenseActionsByEmployee;
        private ToolStripMenuItem msbtnCashExpenseTotal;
        private ToolStripMenuItem msbtnCashExpenseTotalDetails;
        private ToolStripMenuItem msAccoutSettings;
        private ToolStripMenuItem msbtnCurrentUserInfo;
        private ToolStripMenuItem msbtnChangePassword;
        private ToolStripMenuItem msbtnLogout;
        private ToolStripMenuItem msRevenue;
        private ToolStripMenuItem msbtnCashRevenueGet;
        private ToolStripMenuItem msbtnCashRevenueReturn;
        private ToolStripMenuItem msbtnCashRevenueActions;
        private ToolStripMenuItem msbtnCashRevenueActionsByEmployee;
        private ToolStripMenuItem msbtnCashRevenueTotal;
        private ToolStripMenuItem msbtnCashRevenueTotalDetails;
        private ToolStripMenuItem msProfit;
        private ToolStripMenuItem msbtnProfit;
        private ToolStripMenuItem msbtnProfitDetails;
        private ToolStripMenuItem msbtnProfitWithoutRevenue;
        private ToolStripMenuItem msbtnProfitBeforeRevenue;
        private ToolStripMenuItem msbtnProfitWithoutCustmExpense;
        private ToolStripMenuItem msbtnProfitWithoutCustmRevenue;
        private ToolStripMenuItem msbtnProfitVirtual;
        private ToolStripMenuItem msAccounting;
        private ToolStripMenuItem msbtnAccountingTree;
        private ToolStripMenuItem msbtnCreateAccountingTree;
        private ToolStripMenuItem msbtnJournal;
        private ToolStripMenuItem msbtnLedger;
        private ToolStripMenuItem msbtnBalanceSheet;
        private ToolStripMenuItem msbtnAddItem;
        private ToolStripMenuItem msbtnUpdateItem;
        private ToolStripMenuItem msbtnDeleteItem;
        private ToolStripMenuItem msbtnAddSupplier;
        private ToolStripMenuItem msbtnUpdateSupplier;
        private ToolStripMenuItem msbtnDeleteSupplier;
        private ToolStripMenuItem msbtnAddCustomer;
        private ToolStripMenuItem msbtnUpdateCustomer;
        private ToolStripMenuItem msbtnDeleteCustomer;
        private ToolStripMenuItem msbtnAddStore;
        private ToolStripMenuItem msbtnUpdateStore;
        private ToolStripMenuItem msbtnDeleteStore;
        private ToolStripMenuItem msbtnAddCategory;
        private ToolStripMenuItem msbtnUpdateCategory;
        private ToolStripMenuItem msbtnDeleteCategory;
        private ToolStripMenuItem msbtnAddUnit;
        private ToolStripMenuItem msbtnUpdateUnit;
        private ToolStripMenuItem msbtnDeleteUnit;
        private ToolStripMenuItem msbtnAddCompany;
        private ToolStripMenuItem msbtnUpdateCompany;
        private ToolStripMenuItem msbtnDeleteCompany;
        private ToolStripMenuItem msbtnAddDelegate;
        private ToolStripMenuItem msbtnUpdateDelegate;
        private ToolStripMenuItem msbtnDeleteDelegate;
        private ToolStripMenuItem msbtnAddExpense;
        private ToolStripMenuItem msbtnUpdateExpense;
        private ToolStripMenuItem msbtnDeleteExpense;
        private ToolStripMenuItem msbtnAddRevenue;
        private ToolStripMenuItem msbtnUpdateRevenue;
        private ToolStripMenuItem msbtnDeleteRevenue;
        private ToolStripMenuItem msbtnAddEmployee;
        private ToolStripMenuItem msbtnUpdateEmployee;
        private ToolStripMenuItem msbtnDeleteEmployee;
        private ToolStripMenuItem msData;
        private ToolStripMenuItem msbtnImportExcel;
        private ToolStripMenuItem msbtnImportAccess;
        private ToolStripMenuItem msbtnExportAccess;
        private ToolStripMenuItem msbtnTakeBackup;
        private ToolStripMenuItem msbtnRestoreBackup;
        private ToolStripMenuItem msbtnExportExcel;
        private ToolStripMenuItem msbtnSearchItem;
        private ToolStripMenuItem msbtnSearchSupplier;
        private ToolStripMenuItem msbtnSearchCustomer;
        private ToolStripMenuItem msbtnSearchStore;
        private ToolStripMenuItem msbtnSearchCategory;
        private ToolStripMenuItem msbtnSearchUnit;
        private ToolStripMenuItem msbtnSearchCompany;
        private ToolStripMenuItem msbtnSearchDelegate;
        private ToolStripMenuItem msbtnSearchExpense;
        private ToolStripMenuItem msbtnSearchRevenue;
        private ToolStripMenuItem msbtnSearchEmployee;
        private ToolStripMenuItem msReport;
        private ToolStripMenuItem smbtnReportBasis;
        private ToolStripMenuItem smbtnReportStore;
        private ToolStripMenuItem smbtnReportBuy;
        private ToolStripMenuItem smbtnReportSale;
        private ToolStripMenuItem smbtnReportRevenueRevenue;
        private ToolStripMenuItem smbtnReportProfitLoss;
        private ToolStripMenuItem smbtnReportAccounting;
        private ToolStripMenuItem smbtnReportProgramUse;
        private ToolStripMenuItem msLangAndSettings;
        private ToolStripMenuItem msbtnLang;
        private ToolStripMenuItem msbtnBackgroundColors;
        private ToolStripMenuItem msbtnUsers;
        private ToolStripMenuItem msbtnPrivlleges;
        private ToolStripMenuItem msbtnShowOpenForms;
        private ErrorProvider errorProvider1;
    }
}