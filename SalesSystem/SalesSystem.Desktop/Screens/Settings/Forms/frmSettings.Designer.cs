namespace SalesSystem.Desktop.Screens.Settings.Forms
{
    partial class frmSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMode = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.numDefaultWarehouse = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultWarehouse = new System.Windows.Forms.Label();
            this.numDefaultBranch = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultBranch = new System.Windows.Forms.Label();
            this.numDefaultPurchaseTax = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultPurchaseTax = new System.Windows.Forms.Label();
            this.numDefaultSalesTax = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultSalesTax = new System.Windows.Forms.Label();
            this.chkEnableNotifications = new System.Windows.Forms.CheckBox();
            this.chkEnableAttachments = new System.Windows.Forms.CheckBox();
            this.chkAllowNegativeCash = new System.Windows.Forms.CheckBox();
            this.chkAutoCreateJE = new System.Windows.Forms.CheckBox();
            this.tpInventory = new System.Windows.Forms.TabPage();
            this.numStockAlertDays = new System.Windows.Forms.NumericUpDown();
            this.lblStockAlertDays = new System.Windows.Forms.Label();
            this.chkRequireExpiryOnPurchase = new System.Windows.Forms.CheckBox();
            this.chkRequireBatchOnPurchase = new System.Windows.Forms.CheckBox();
            this.chkEnableFefo = new System.Windows.Forms.CheckBox();
            this.chkAllowNegativeStock = new System.Windows.Forms.CheckBox();
            this.tpSales = new System.Windows.Forms.TabPage();
            this.chkAutoPrintAfterPosting = new System.Windows.Forms.CheckBox();
            this.chkShowExpiryInInvoices = new System.Windows.Forms.CheckBox();
            this.chkHideTaxInSales = new System.Windows.Forms.CheckBox();
            this.chkShowProfitInInvoice = new System.Windows.Forms.CheckBox();
            this.chkAllowDrafts = new System.Windows.Forms.CheckBox();
            this.chkAutoPostInvoices = new System.Windows.Forms.CheckBox();
            this.chkAllowBelowCostSale = new System.Windows.Forms.CheckBox();
            this.chkPreventBelowRetailPrice = new System.Windows.Forms.CheckBox();
            this.numDefaultCashCustomerId = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultCashCustomerId = new System.Windows.Forms.Label();
            this.tpPurchases = new System.Windows.Forms.TabPage();
            this.chkHideTaxInPurchases = new System.Windows.Forms.CheckBox();
            this.chkPurchaseAutoPost = new System.Windows.Forms.CheckBox();
            this.numDefaultCashSupplierId = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultCashSupplierId = new System.Windows.Forms.Label();
            this.tpBarcode = new System.Windows.Forms.TabPage();
            this.chkAllowDuplicateBarcode = new System.Windows.Forms.CheckBox();
            this.chkAutoGenerateBarcode = new System.Windows.Forms.CheckBox();
            this.tpPrint = new System.Windows.Forms.TabPage();
            this.chkPrintCompanyAddress = new System.Windows.Forms.CheckBox();
            this.chkPrintQRCode = new System.Windows.Forms.CheckBox();
            this.chkPrintBarcode = new System.Windows.Forms.CheckBox();
            this.chkPrintSignature = new System.Windows.Forms.CheckBox();
            this.chkShowBalanceOnPrint = new System.Windows.Forms.CheckBox();
            this.chkShowLogo = new System.Windows.Forms.CheckBox();
            this.numPrintCopies = new System.Windows.Forms.NumericUpDown();
            this.lblPrintCopies = new System.Windows.Forms.Label();
            this.txtPaperSize = new System.Windows.Forms.TextBox();
            this.lblPaperSize = new System.Windows.Forms.Label();
            this.txtFooterNote = new System.Windows.Forms.TextBox();
            this.lblFooterNote = new System.Windows.Forms.Label();
            this.txtStoreTaxNumber = new System.Windows.Forms.TextBox();
            this.lblStoreTaxNumber = new System.Windows.Forms.Label();
            this.txtLogoPath = new System.Windows.Forms.TextBox();
            this.lblLogoPath = new System.Windows.Forms.Label();
            this.txtA4PrinterName = new System.Windows.Forms.TextBox();
            this.lblA4PrinterName = new System.Windows.Forms.Label();
            this.txtThermalPrinterName = new System.Windows.Forms.TextBox();
            this.lblThermalPrinterName = new System.Windows.Forms.Label();
            this.tpNotifications = new System.Windows.Forms.TabPage();
            this.numExpiryAlertDays = new System.Windows.Forms.NumericUpDown();
            this.lblExpiryAlertDays = new System.Windows.Forms.Label();
            this.chkCreditLimitAlert = new System.Windows.Forms.CheckBox();
            this.chkExpiryAlert = new System.Windows.Forms.CheckBox();
            this.chkLowStockAlert = new System.Windows.Forms.CheckBox();
            this.tpAccounting = new System.Windows.Forms.TabPage();
            this.chkAccountingAutoJE = new System.Windows.Forms.CheckBox();
            this.tpBackup = new System.Windows.Forms.TabPage();
            this.btnBrowseBackupPath = new System.Windows.Forms.Button();
            this.txtBackupPath = new System.Windows.Forms.TextBox();
            this.lblBackupPath = new System.Windows.Forms.Label();
            this.numBackupRetentionDays = new System.Windows.Forms.NumericUpDown();
            this.lblBackupRetentionDays = new System.Windows.Forms.Label();
            this.txtBackupScheduleTime = new System.Windows.Forms.TextBox();
            this.lblBackupScheduleTime = new System.Windows.Forms.Label();
            this.btnCreateBackup = new System.Windows.Forms.Button();
            this.lstBackupFiles = new System.Windows.Forms.ListBox();
            this.lblBackupFiles = new System.Windows.Forms.Label();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.openFileDialogLogo = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabSettings.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultWarehouse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultBranch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultPurchaseTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultSalesTax)).BeginInit();
            this.tpInventory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStockAlertDays)).BeginInit();
            this.tpSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultCashCustomerId)).BeginInit();
            this.tpPurchases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultCashSupplierId)).BeginInit();
            this.tpBarcode.SuspendLayout();
            this.tpPrint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrintCopies)).BeginInit();
            this.tpNotifications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExpiryAlertDays)).BeginInit();
            this.tpAccounting.SuspendLayout();
            this.tpBackup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackupRetentionDays)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMode.AutoSize = true;
            this.lblMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMode.Location = new System.Drawing.Point(315, 15);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(248, 34);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "إعدادات النظام";
            // 
            // tabSettings
            // 
            this.tabSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSettings.Controls.Add(this.tpGeneral);
            this.tabSettings.Controls.Add(this.tpInventory);
            this.tabSettings.Controls.Add(this.tpSales);
            this.tabSettings.Controls.Add(this.tpPurchases);
            this.tabSettings.Controls.Add(this.tpBarcode);
            this.tabSettings.Controls.Add(this.tpPrint);
            this.tabSettings.Controls.Add(this.tpNotifications);
            this.tabSettings.Controls.Add(this.tpAccounting);
            this.tabSettings.Controls.Add(this.tpBackup);
            this.tabSettings.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tabSettings.Location = new System.Drawing.Point(12, 50);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabSettings.RightToLeftLayout = true;
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(876, 510);
            this.tabSettings.TabIndex = 1;
            // 
            // tpGeneral
            // 
            this.tpGeneral.BackColor = System.Drawing.Color.White;
            this.tpGeneral.Controls.Add(this.numDefaultWarehouse);
            this.tpGeneral.Controls.Add(this.lblDefaultWarehouse);
            this.tpGeneral.Controls.Add(this.numDefaultBranch);
            this.tpGeneral.Controls.Add(this.lblDefaultBranch);
            this.tpGeneral.Controls.Add(this.numDefaultPurchaseTax);
            this.tpGeneral.Controls.Add(this.lblDefaultPurchaseTax);
            this.tpGeneral.Controls.Add(this.numDefaultSalesTax);
            this.tpGeneral.Controls.Add(this.lblDefaultSalesTax);
            this.tpGeneral.Controls.Add(this.chkEnableNotifications);
            this.tpGeneral.Controls.Add(this.chkEnableAttachments);
            this.tpGeneral.Controls.Add(this.chkAllowNegativeCash);
            this.tpGeneral.Controls.Add(this.chkAutoCreateJE);
            this.tpGeneral.Location = new System.Drawing.Point(4, 28);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(10);
            this.tpGeneral.Size = new System.Drawing.Size(868, 478);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "عام";
            // 
            // numDefaultWarehouse
            // 
            this.numDefaultWarehouse.Location = new System.Drawing.Point(20, 220);
            this.numDefaultWarehouse.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numDefaultWarehouse.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDefaultWarehouse.Name = "numDefaultWarehouse";
            this.numDefaultWarehouse.Size = new System.Drawing.Size(100, 26);
            this.numDefaultWarehouse.TabIndex = 11;
            this.numDefaultWarehouse.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDefaultWarehouse
            // 
            this.lblDefaultWarehouse.AutoSize = true;
            this.lblDefaultWarehouse.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultWarehouse.Location = new System.Drawing.Point(250, 222);
            this.lblDefaultWarehouse.Name = "lblDefaultWarehouse";
            this.lblDefaultWarehouse.Size = new System.Drawing.Size(114, 18);
            this.lblDefaultWarehouse.TabIndex = 10;
            this.lblDefaultWarehouse.Text = "المستودع الافتراضي:";
            // 
            // numDefaultBranch
            // 
            this.numDefaultBranch.Location = new System.Drawing.Point(20, 180);
            this.numDefaultBranch.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numDefaultBranch.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDefaultBranch.Name = "numDefaultBranch";
            this.numDefaultBranch.Size = new System.Drawing.Size(100, 26);
            this.numDefaultBranch.TabIndex = 9;
            this.numDefaultBranch.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDefaultBranch
            // 
            this.lblDefaultBranch.AutoSize = true;
            this.lblDefaultBranch.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultBranch.Location = new System.Drawing.Point(250, 182);
            this.lblDefaultBranch.Name = "lblDefaultBranch";
            this.lblDefaultBranch.Size = new System.Drawing.Size(103, 18);
            this.lblDefaultBranch.TabIndex = 8;
            this.lblDefaultBranch.Text = "الفرع الافتراضي:";
            // 
            // numDefaultPurchaseTax
            // 
            this.numDefaultPurchaseTax.Location = new System.Drawing.Point(20, 140);
            this.numDefaultPurchaseTax.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numDefaultPurchaseTax.Name = "numDefaultPurchaseTax";
            this.numDefaultPurchaseTax.Size = new System.Drawing.Size(100, 26);
            this.numDefaultPurchaseTax.TabIndex = 7;
            // 
            // lblDefaultPurchaseTax
            // 
            this.lblDefaultPurchaseTax.AutoSize = true;
            this.lblDefaultPurchaseTax.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultPurchaseTax.Location = new System.Drawing.Point(250, 142);
            this.lblDefaultPurchaseTax.Name = "lblDefaultPurchaseTax";
            this.lblDefaultPurchaseTax.Size = new System.Drawing.Size(119, 18);
            this.lblDefaultPurchaseTax.TabIndex = 6;
            this.lblDefaultPurchaseTax.Text = "الضريبة الافتراضية - مشتريات:";
            // 
            // numDefaultSalesTax
            // 
            this.numDefaultSalesTax.Location = new System.Drawing.Point(20, 100);
            this.numDefaultSalesTax.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numDefaultSalesTax.Name = "numDefaultSalesTax";
            this.numDefaultSalesTax.Size = new System.Drawing.Size(100, 26);
            this.numDefaultSalesTax.TabIndex = 5;
            // 
            // lblDefaultSalesTax
            // 
            this.lblDefaultSalesTax.AutoSize = true;
            this.lblDefaultSalesTax.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultSalesTax.Location = new System.Drawing.Point(250, 102);
            this.lblDefaultSalesTax.Name = "lblDefaultSalesTax";
            this.lblDefaultSalesTax.Size = new System.Drawing.Size(116, 18);
            this.lblDefaultSalesTax.TabIndex = 4;
            this.lblDefaultSalesTax.Text = "الضريبة الافتراضية - مبيعات:";
            // 
            // chkEnableNotifications
            // 
            this.chkEnableNotifications.AutoSize = true;
            this.chkEnableNotifications.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkEnableNotifications.Location = new System.Drawing.Point(540, 220);
            this.chkEnableNotifications.Name = "chkEnableNotifications";
            this.chkEnableNotifications.Size = new System.Drawing.Size(127, 22);
            this.chkEnableNotifications.TabIndex = 3;
            this.chkEnableNotifications.Text = "تفعيل الإشعارات";
            this.chkEnableNotifications.UseVisualStyleBackColor = true;
            // 
            // chkEnableAttachments
            // 
            this.chkEnableAttachments.AutoSize = true;
            this.chkEnableAttachments.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkEnableAttachments.Location = new System.Drawing.Point(540, 180);
            this.chkEnableAttachments.Name = "chkEnableAttachments";
            this.chkEnableAttachments.Size = new System.Drawing.Size(145, 22);
            this.chkEnableAttachments.TabIndex = 2;
            this.chkEnableAttachments.Text = "تفعيل المرفقات";
            this.chkEnableAttachments.UseVisualStyleBackColor = true;
            // 
            // chkAllowNegativeCash
            // 
            this.chkAllowNegativeCash.AutoSize = true;
            this.chkAllowNegativeCash.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAllowNegativeCash.Location = new System.Drawing.Point(540, 140);
            this.chkAllowNegativeCash.Name = "chkAllowNegativeCash";
            this.chkAllowNegativeCash.Size = new System.Drawing.Size(183, 22);
            this.chkAllowNegativeCash.TabIndex = 1;
            this.chkAllowNegativeCash.Text = "السماح برصيد نقدي سالب";
            this.chkAllowNegativeCash.UseVisualStyleBackColor = true;
            // 
            // chkAutoCreateJE
            // 
            this.chkAutoCreateJE.AutoSize = true;
            this.chkAutoCreateJE.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAutoCreateJE.Location = new System.Drawing.Point(540, 100);
            this.chkAutoCreateJE.Name = "chkAutoCreateJE";
            this.chkAutoCreateJE.Size = new System.Drawing.Size(172, 22);
            this.chkAutoCreateJE.TabIndex = 0;
            this.chkAutoCreateJE.Text = "إنشاء قيود يومية تلقائياً";
            this.chkAutoCreateJE.UseVisualStyleBackColor = true;
            // 
            // tpInventory
            // 
            this.tpInventory.BackColor = System.Drawing.Color.White;
            this.tpInventory.Controls.Add(this.numStockAlertDays);
            this.tpInventory.Controls.Add(this.lblStockAlertDays);
            this.tpInventory.Controls.Add(this.chkRequireExpiryOnPurchase);
            this.tpInventory.Controls.Add(this.chkRequireBatchOnPurchase);
            this.tpInventory.Controls.Add(this.chkEnableFefo);
            this.tpInventory.Controls.Add(this.chkAllowNegativeStock);
            this.tpInventory.Location = new System.Drawing.Point(4, 28);
            this.tpInventory.Name = "tpInventory";
            this.tpInventory.Padding = new System.Windows.Forms.Padding(10);
            this.tpInventory.Size = new System.Drawing.Size(868, 478);
            this.tpInventory.TabIndex = 1;
            this.tpInventory.Text = "المخزون";
            // 
            // numStockAlertDays
            // 
            this.numStockAlertDays.Location = new System.Drawing.Point(20, 170);
            this.numStockAlertDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            this.numStockAlertDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numStockAlertDays.Name = "numStockAlertDays";
            this.numStockAlertDays.Size = new System.Drawing.Size(100, 26);
            this.numStockAlertDays.TabIndex = 5;
            this.numStockAlertDays.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // lblStockAlertDays
            // 
            this.lblStockAlertDays.AutoSize = true;
            this.lblStockAlertDays.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblStockAlertDays.Location = new System.Drawing.Point(250, 172);
            this.lblStockAlertDays.Name = "lblStockAlertDays";
            this.lblStockAlertDays.Size = new System.Drawing.Size(74, 18);
            this.lblStockAlertDays.TabIndex = 4;
            this.lblStockAlertDays.Text = "تنبيه المخزون (أيام):";
            // 
            // chkRequireExpiryOnPurchase
            // 
            this.chkRequireExpiryOnPurchase.AutoSize = true;
            this.chkRequireExpiryOnPurchase.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkRequireExpiryOnPurchase.Location = new System.Drawing.Point(20, 130);
            this.chkRequireExpiryOnPurchase.Name = "chkRequireExpiryOnPurchase";
            this.chkRequireExpiryOnPurchase.Size = new System.Drawing.Size(193, 22);
            this.chkRequireExpiryOnPurchase.TabIndex = 3;
            this.chkRequireExpiryOnPurchase.Text = "إلزام إدخال تاريخ الصلاحية عند الشراء";
            this.chkRequireExpiryOnPurchase.UseVisualStyleBackColor = true;
            // 
            // chkRequireBatchOnPurchase
            // 
            this.chkRequireBatchOnPurchase.AutoSize = true;
            this.chkRequireBatchOnPurchase.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkRequireBatchOnPurchase.Location = new System.Drawing.Point(20, 90);
            this.chkRequireBatchOnPurchase.Name = "chkRequireBatchOnPurchase";
            this.chkRequireBatchOnPurchase.Size = new System.Drawing.Size(199, 22);
            this.chkRequireBatchOnPurchase.TabIndex = 2;
            this.chkRequireBatchOnPurchase.Text = "إلزام إدخال رقم الدفعة عند الشراء";
            this.chkRequireBatchOnPurchase.UseVisualStyleBackColor = true;
            // 
            // chkEnableFefo
            // 
            this.chkEnableFefo.AutoSize = true;
            this.chkEnableFefo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkEnableFefo.Location = new System.Drawing.Point(20, 50);
            this.chkEnableFefo.Name = "chkEnableFefo";
            this.chkEnableFefo.Size = new System.Drawing.Size(246, 22);
            this.chkEnableFefo.TabIndex = 1;
            this.chkEnableFefo.Text = "تفعيل نظام الصرف حسب تاريخ الصلاحية (FEFO)";
            this.chkEnableFefo.UseVisualStyleBackColor = true;
            // 
            // chkAllowNegativeStock
            // 
            this.chkAllowNegativeStock.AutoSize = true;
            this.chkAllowNegativeStock.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAllowNegativeStock.Location = new System.Drawing.Point(20, 20);
            this.chkAllowNegativeStock.Name = "chkAllowNegativeStock";
            this.chkAllowNegativeStock.Size = new System.Drawing.Size(177, 22);
            this.chkAllowNegativeStock.TabIndex = 0;
            this.chkAllowNegativeStock.Text = "السماح بمخزون سالب";
            this.chkAllowNegativeStock.UseVisualStyleBackColor = true;
            // 
            // tpSales
            // 
            this.tpSales.BackColor = System.Drawing.Color.White;
            this.tpSales.Controls.Add(this.chkAutoPrintAfterPosting);
            this.tpSales.Controls.Add(this.chkShowExpiryInInvoices);
            this.tpSales.Controls.Add(this.chkHideTaxInSales);
            this.tpSales.Controls.Add(this.chkShowProfitInInvoice);
            this.tpSales.Controls.Add(this.chkAllowDrafts);
            this.tpSales.Controls.Add(this.chkAutoPostInvoices);
            this.tpSales.Controls.Add(this.chkAllowBelowCostSale);
            this.tpSales.Controls.Add(this.chkPreventBelowRetailPrice);
            this.tpSales.Controls.Add(this.numDefaultCashCustomerId);
            this.tpSales.Controls.Add(this.lblDefaultCashCustomerId);
            this.tpSales.Location = new System.Drawing.Point(4, 28);
            this.tpSales.Name = "tpSales";
            this.tpSales.Padding = new System.Windows.Forms.Padding(10);
            this.tpSales.Size = new System.Drawing.Size(868, 478);
            this.tpSales.TabIndex = 2;
            this.tpSales.Text = "المبيعات";
            // 
            // chkAutoPrintAfterPosting
            // 
            this.chkAutoPrintAfterPosting.AutoSize = true;
            this.chkAutoPrintAfterPosting.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAutoPrintAfterPosting.Location = new System.Drawing.Point(20, 220);
            this.chkAutoPrintAfterPosting.Name = "chkAutoPrintAfterPosting";
            this.chkAutoPrintAfterPosting.Size = new System.Drawing.Size(173, 22);
            this.chkAutoPrintAfterPosting.TabIndex = 9;
            this.chkAutoPrintAfterPosting.Text = "طباعة تلقائية بعد الترحيل";
            this.chkAutoPrintAfterPosting.UseVisualStyleBackColor = true;
            // 
            // chkShowExpiryInInvoices
            // 
            this.chkShowExpiryInInvoices.AutoSize = true;
            this.chkShowExpiryInInvoices.Checked = true;
            this.chkShowExpiryInInvoices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowExpiryInInvoices.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkShowExpiryInInvoices.Location = new System.Drawing.Point(20, 190);
            this.chkShowExpiryInInvoices.Name = "chkShowExpiryInInvoices";
            this.chkShowExpiryInInvoices.Size = new System.Drawing.Size(184, 22);
            this.chkShowExpiryInInvoices.TabIndex = 8;
            this.chkShowExpiryInInvoices.Text = "إظهار تاريخ الصلاحية في الفواتير";
            this.chkShowExpiryInInvoices.UseVisualStyleBackColor = true;
            // 
            // chkHideTaxInSales
            // 
            this.chkHideTaxInSales.AutoSize = true;
            this.chkHideTaxInSales.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkHideTaxInSales.Location = new System.Drawing.Point(20, 160);
            this.chkHideTaxInSales.Name = "chkHideTaxInSales";
            this.chkHideTaxInSales.Size = new System.Drawing.Size(155, 22);
            this.chkHideTaxInSales.TabIndex = 7;
            this.chkHideTaxInSales.Text = "إخفاء الضريبة في المبيعات";
            this.chkHideTaxInSales.UseVisualStyleBackColor = true;
            // 
            // chkShowProfitInInvoice
            // 
            this.chkShowProfitInInvoice.AutoSize = true;
            this.chkShowProfitInInvoice.Checked = true;
            this.chkShowProfitInInvoice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowProfitInInvoice.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkShowProfitInInvoice.Location = new System.Drawing.Point(20, 130);
            this.chkShowProfitInInvoice.Name = "chkShowProfitInInvoice";
            this.chkShowProfitInInvoice.Size = new System.Drawing.Size(161, 22);
            this.chkShowProfitInInvoice.TabIndex = 6;
            this.chkShowProfitInInvoice.Text = "إظهار الربح في الفاتورة";
            this.chkShowProfitInInvoice.UseVisualStyleBackColor = true;
            // 
            // chkAllowDrafts
            // 
            this.chkAllowDrafts.AutoSize = true;
            this.chkAllowDrafts.Checked = true;
            this.chkAllowDrafts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowDrafts.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAllowDrafts.Location = new System.Drawing.Point(20, 100);
            this.chkAllowDrafts.Name = "chkAllowDrafts";
            this.chkAllowDrafts.Size = new System.Drawing.Size(143, 22);
            this.chkAllowDrafts.TabIndex = 5;
            this.chkAllowDrafts.Text = "السماح بالمسودات";
            this.chkAllowDrafts.UseVisualStyleBackColor = true;
            // 
            // chkAutoPostInvoices
            // 
            this.chkAutoPostInvoices.AutoSize = true;
            this.chkAutoPostInvoices.Checked = true;
            this.chkAutoPostInvoices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoPostInvoices.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAutoPostInvoices.Location = new System.Drawing.Point(20, 70);
            this.chkAutoPostInvoices.Name = "chkAutoPostInvoices";
            this.chkAutoPostInvoices.Size = new System.Drawing.Size(176, 22);
            this.chkAutoPostInvoices.TabIndex = 4;
            this.chkAutoPostInvoices.Text = "ترحيل الفواتير تلقائياً";
            this.chkAutoPostInvoices.UseVisualStyleBackColor = true;
            // 
            // chkAllowBelowCostSale
            // 
            this.chkAllowBelowCostSale.AutoSize = true;
            this.chkAllowBelowCostSale.Checked = true;
            this.chkAllowBelowCostSale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowBelowCostSale.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAllowBelowCostSale.Location = new System.Drawing.Point(460, 20);
            this.chkAllowBelowCostSale.Name = "chkAllowBelowCostSale";
            this.chkAllowBelowCostSale.Size = new System.Drawing.Size(197, 22);
            this.chkAllowBelowCostSale.TabIndex = 3;
            this.chkAllowBelowCostSale.Text = "السماح بالبيع بأقل من التكلفة";
            this.chkAllowBelowCostSale.UseVisualStyleBackColor = true;
            // 
            // chkPreventBelowRetailPrice
            // 
            this.chkPreventBelowRetailPrice.AutoSize = true;
            this.chkPreventBelowRetailPrice.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkPreventBelowRetailPrice.Location = new System.Drawing.Point(460, 50);
            this.chkPreventBelowRetailPrice.Name = "chkPreventBelowRetailPrice";
            this.chkPreventBelowRetailPrice.Size = new System.Drawing.Size(230, 22);
            this.chkPreventBelowRetailPrice.TabIndex = 2;
            this.chkPreventBelowRetailPrice.Text = "منع البيع بأقل من السعر الرسمي";
            this.chkPreventBelowRetailPrice.UseVisualStyleBackColor = true;
            // 
            // numDefaultCashCustomerId
            // 
            this.numDefaultCashCustomerId.Location = new System.Drawing.Point(20, 30);
            this.numDefaultCashCustomerId.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numDefaultCashCustomerId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDefaultCashCustomerId.Name = "numDefaultCashCustomerId";
            this.numDefaultCashCustomerId.Size = new System.Drawing.Size(100, 26);
            this.numDefaultCashCustomerId.TabIndex = 1;
            this.numDefaultCashCustomerId.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDefaultCashCustomerId
            // 
            this.lblDefaultCashCustomerId.AutoSize = true;
            this.lblDefaultCashCustomerId.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultCashCustomerId.Location = new System.Drawing.Point(250, 32);
            this.lblDefaultCashCustomerId.Name = "lblDefaultCashCustomerId";
            this.lblDefaultCashCustomerId.Size = new System.Drawing.Size(135, 18);
            this.lblDefaultCashCustomerId.TabIndex = 0;
            this.lblDefaultCashCustomerId.Text = "العميل النقدي الافتراضي:";
            // 
            // tpPurchases
            // 
            this.tpPurchases.BackColor = System.Drawing.Color.White;
            this.tpPurchases.Controls.Add(this.chkHideTaxInPurchases);
            this.tpPurchases.Controls.Add(this.chkPurchaseAutoPost);
            this.tpPurchases.Controls.Add(this.numDefaultCashSupplierId);
            this.tpPurchases.Controls.Add(this.lblDefaultCashSupplierId);
            this.tpPurchases.Location = new System.Drawing.Point(4, 28);
            this.tpPurchases.Name = "tpPurchases";
            this.tpPurchases.Padding = new System.Windows.Forms.Padding(10);
            this.tpPurchases.Size = new System.Drawing.Size(868, 478);
            this.tpPurchases.TabIndex = 3;
            this.tpPurchases.Text = "المشتريات";
            // 
            // chkHideTaxInPurchases
            // 
            this.chkHideTaxInPurchases.AutoSize = true;
            this.chkHideTaxInPurchases.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkHideTaxInPurchases.Location = new System.Drawing.Point(20, 90);
            this.chkHideTaxInPurchases.Name = "chkHideTaxInPurchases";
            this.chkHideTaxInPurchases.Size = new System.Drawing.Size(182, 22);
            this.chkHideTaxInPurchases.TabIndex = 3;
            this.chkHideTaxInPurchases.Text = "إخفاء الضريبة في المشتريات";
            this.chkHideTaxInPurchases.UseVisualStyleBackColor = true;
            // 
            // chkPurchaseAutoPost
            // 
            this.chkPurchaseAutoPost.AutoSize = true;
            this.chkPurchaseAutoPost.Checked = true;
            this.chkPurchaseAutoPost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPurchaseAutoPost.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkPurchaseAutoPost.Location = new System.Drawing.Point(20, 50);
            this.chkPurchaseAutoPost.Name = "chkPurchaseAutoPost";
            this.chkPurchaseAutoPost.Size = new System.Drawing.Size(184, 22);
            this.chkPurchaseAutoPost.TabIndex = 2;
            this.chkPurchaseAutoPost.Text = "ترحيل فواتير المشتريات تلقائياً";
            this.chkPurchaseAutoPost.UseVisualStyleBackColor = true;
            // 
            // numDefaultCashSupplierId
            // 
            this.numDefaultCashSupplierId.Location = new System.Drawing.Point(20, 20);
            this.numDefaultCashSupplierId.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numDefaultCashSupplierId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDefaultCashSupplierId.Name = "numDefaultCashSupplierId";
            this.numDefaultCashSupplierId.Size = new System.Drawing.Size(100, 26);
            this.numDefaultCashSupplierId.TabIndex = 1;
            this.numDefaultCashSupplierId.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblDefaultCashSupplierId
            // 
            this.lblDefaultCashSupplierId.AutoSize = true;
            this.lblDefaultCashSupplierId.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDefaultCashSupplierId.Location = new System.Drawing.Point(250, 22);
            this.lblDefaultCashSupplierId.Name = "lblDefaultCashSupplierId";
            this.lblDefaultCashSupplierId.Size = new System.Drawing.Size(159, 18);
            this.lblDefaultCashSupplierId.TabIndex = 0;
            this.lblDefaultCashSupplierId.Text = "المورد النقدي الافتراضي:";
            // 
            // tpBarcode
            // 
            this.tpBarcode.BackColor = System.Drawing.Color.White;
            this.tpBarcode.Controls.Add(this.chkAllowDuplicateBarcode);
            this.tpBarcode.Controls.Add(this.chkAutoGenerateBarcode);
            this.tpBarcode.Location = new System.Drawing.Point(4, 28);
            this.tpBarcode.Name = "tpBarcode";
            this.tpBarcode.Padding = new System.Windows.Forms.Padding(10);
            this.tpBarcode.Size = new System.Drawing.Size(868, 478);
            this.tpBarcode.TabIndex = 4;
            this.tpBarcode.Text = "الباركود";
            // 
            // chkAllowDuplicateBarcode
            // 
            this.chkAllowDuplicateBarcode.AutoSize = true;
            this.chkAllowDuplicateBarcode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAllowDuplicateBarcode.Location = new System.Drawing.Point(20, 60);
            this.chkAllowDuplicateBarcode.Name = "chkAllowDuplicateBarcode";
            this.chkAllowDuplicateBarcode.Size = new System.Drawing.Size(186, 22);
            this.chkAllowDuplicateBarcode.TabIndex = 1;
            this.chkAllowDuplicateBarcode.Text = "السماح بتكرار الباركود";
            this.chkAllowDuplicateBarcode.UseVisualStyleBackColor = true;
            // 
            // chkAutoGenerateBarcode
            // 
            this.chkAutoGenerateBarcode.AutoSize = true;
            this.chkAutoGenerateBarcode.Checked = true;
            this.chkAutoGenerateBarcode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoGenerateBarcode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAutoGenerateBarcode.Location = new System.Drawing.Point(20, 20);
            this.chkAutoGenerateBarcode.Name = "chkAutoGenerateBarcode";
            this.chkAutoGenerateBarcode.Size = new System.Drawing.Size(185, 22);
            this.chkAutoGenerateBarcode.TabIndex = 0;
            this.chkAutoGenerateBarcode.Text = "توليد الباركود تلقائياً";
            this.chkAutoGenerateBarcode.UseVisualStyleBackColor = true;
            // 
            // tpPrint
            // 
            this.tpPrint.BackColor = System.Drawing.Color.White;
            this.tpPrint.AutoScroll = true;
            this.tpPrint.Controls.Add(this.chkPrintCompanyAddress);
            this.tpPrint.Controls.Add(this.chkPrintQRCode);
            this.tpPrint.Controls.Add(this.chkPrintBarcode);
            this.tpPrint.Controls.Add(this.chkPrintSignature);
            this.tpPrint.Controls.Add(this.chkShowBalanceOnPrint);
            this.tpPrint.Controls.Add(this.chkShowLogo);
            this.tpPrint.Controls.Add(this.numPrintCopies);
            this.tpPrint.Controls.Add(this.lblPrintCopies);
            this.tpPrint.Controls.Add(this.txtPaperSize);
            this.tpPrint.Controls.Add(this.lblPaperSize);
            this.tpPrint.Controls.Add(this.txtFooterNote);
            this.tpPrint.Controls.Add(this.lblFooterNote);
            this.tpPrint.Controls.Add(this.txtStoreTaxNumber);
            this.tpPrint.Controls.Add(this.lblStoreTaxNumber);
            this.tpPrint.Controls.Add(this.txtLogoPath);
            this.tpPrint.Controls.Add(this.lblLogoPath);
            this.tpPrint.Controls.Add(this.txtA4PrinterName);
            this.tpPrint.Controls.Add(this.lblA4PrinterName);
            this.tpPrint.Controls.Add(this.txtThermalPrinterName);
            this.tpPrint.Controls.Add(this.lblThermalPrinterName);
            this.tpPrint.Location = new System.Drawing.Point(4, 28);
            this.tpPrint.Name = "tpPrint";
            this.tpPrint.Padding = new System.Windows.Forms.Padding(10);
            this.tpPrint.Size = new System.Drawing.Size(868, 478);
            this.tpPrint.TabIndex = 5;
            this.tpPrint.Text = "الطباعة";
            // 
            // chkPrintCompanyAddress
            // 
            this.chkPrintCompanyAddress.AutoSize = true;
            this.chkPrintCompanyAddress.Checked = true;
            this.chkPrintCompanyAddress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrintCompanyAddress.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkPrintCompanyAddress.Location = new System.Drawing.Point(460, 280);
            this.chkPrintCompanyAddress.Name = "chkPrintCompanyAddress";
            this.chkPrintCompanyAddress.Size = new System.Drawing.Size(137, 22);
            this.chkPrintCompanyAddress.TabIndex = 19;
            this.chkPrintCompanyAddress.Text = "طباعة عنوان الشركة";
            this.chkPrintCompanyAddress.UseVisualStyleBackColor = true;
            // 
            // chkPrintQRCode
            // 
            this.chkPrintQRCode.AutoSize = true;
            this.chkPrintQRCode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkPrintQRCode.Location = new System.Drawing.Point(460, 250);
            this.chkPrintQRCode.Name = "chkPrintQRCode";
            this.chkPrintQRCode.Size = new System.Drawing.Size(119, 22);
            this.chkPrintQRCode.TabIndex = 18;
            this.chkPrintQRCode.Text = "طباعة رمز QR";
            this.chkPrintQRCode.UseVisualStyleBackColor = true;
            // 
            // chkPrintBarcode
            // 
            this.chkPrintBarcode.AutoSize = true;
            this.chkPrintBarcode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkPrintBarcode.Location = new System.Drawing.Point(460, 220);
            this.chkPrintBarcode.Name = "chkPrintBarcode";
            this.chkPrintBarcode.Size = new System.Drawing.Size(137, 22);
            this.chkPrintBarcode.TabIndex = 17;
            this.chkPrintBarcode.Text = "طباعة الباركود";
            this.chkPrintBarcode.UseVisualStyleBackColor = true;
            // 
            // chkPrintSignature
            // 
            this.chkPrintSignature.AutoSize = true;
            this.chkPrintSignature.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkPrintSignature.Location = new System.Drawing.Point(460, 190);
            this.chkPrintSignature.Name = "chkPrintSignature";
            this.chkPrintSignature.Size = new System.Drawing.Size(117, 22);
            this.chkPrintSignature.TabIndex = 16;
            this.chkPrintSignature.Text = "طباعة التوقيع";
            this.chkPrintSignature.UseVisualStyleBackColor = true;
            // 
            // chkShowBalanceOnPrint
            // 
            this.chkShowBalanceOnPrint.AutoSize = true;
            this.chkShowBalanceOnPrint.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkShowBalanceOnPrint.Location = new System.Drawing.Point(460, 160);
            this.chkShowBalanceOnPrint.Name = "chkShowBalanceOnPrint";
            this.chkShowBalanceOnPrint.Size = new System.Drawing.Size(145, 22);
            this.chkShowBalanceOnPrint.TabIndex = 15;
            this.chkShowBalanceOnPrint.Text = "إظهار الرصيد عند الطباعة";
            this.chkShowBalanceOnPrint.UseVisualStyleBackColor = true;
            // 
            // chkShowLogo
            // 
            this.chkShowLogo.AutoSize = true;
            this.chkShowLogo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkShowLogo.Location = new System.Drawing.Point(460, 130);
            this.chkShowLogo.Name = "chkShowLogo";
            this.chkShowLogo.Size = new System.Drawing.Size(117, 22);
            this.chkShowLogo.TabIndex = 14;
            this.chkShowLogo.Text = "إظهار الشعار";
            this.chkShowLogo.UseVisualStyleBackColor = true;
            // 
            // numPrintCopies
            // 
            this.numPrintCopies.Location = new System.Drawing.Point(20, 280);
            this.numPrintCopies.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            this.numPrintCopies.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPrintCopies.Name = "numPrintCopies";
            this.numPrintCopies.Size = new System.Drawing.Size(100, 26);
            this.numPrintCopies.TabIndex = 13;
            this.numPrintCopies.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblPrintCopies
            // 
            this.lblPrintCopies.AutoSize = true;
            this.lblPrintCopies.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPrintCopies.Location = new System.Drawing.Point(250, 282);
            this.lblPrintCopies.Name = "lblPrintCopies";
            this.lblPrintCopies.Size = new System.Drawing.Size(92, 18);
            this.lblPrintCopies.TabIndex = 12;
            this.lblPrintCopies.Text = "عدد النسخ:";
            // 
            // txtPaperSize
            // 
            this.txtPaperSize.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtPaperSize.Location = new System.Drawing.Point(20, 240);
            this.txtPaperSize.Name = "txtPaperSize";
            this.txtPaperSize.Size = new System.Drawing.Size(200, 26);
            this.txtPaperSize.TabIndex = 11;
            this.txtPaperSize.Text = "A4";
            // 
            // lblPaperSize
            // 
            this.lblPaperSize.AutoSize = true;
            this.lblPaperSize.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPaperSize.Location = new System.Drawing.Point(250, 243);
            this.lblPaperSize.Name = "lblPaperSize";
            this.lblPaperSize.Size = new System.Drawing.Size(91, 18);
            this.lblPaperSize.TabIndex = 10;
            this.lblPaperSize.Text = "حجم الورق:";
            // 
            // txtFooterNote
            // 
            this.txtFooterNote.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtFooterNote.Location = new System.Drawing.Point(20, 200);
            this.txtFooterNote.Name = "txtFooterNote";
            this.txtFooterNote.Size = new System.Drawing.Size(400, 26);
            this.txtFooterNote.TabIndex = 9;
            // 
            // lblFooterNote
            // 
            this.lblFooterNote.AutoSize = true;
            this.lblFooterNote.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblFooterNote.Location = new System.Drawing.Point(460, 85);
            this.lblFooterNote.Name = "lblFooterNote";
            this.lblFooterNote.Size = new System.Drawing.Size(105, 18);
            this.lblFooterNote.TabIndex = 8;
            this.lblFooterNote.Text = "ملاحظة التذييل:";
            // 
            // txtStoreTaxNumber
            // 
            this.txtStoreTaxNumber.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtStoreTaxNumber.Location = new System.Drawing.Point(20, 160);
            this.txtStoreTaxNumber.Name = "txtStoreTaxNumber";
            this.txtStoreTaxNumber.Size = new System.Drawing.Size(400, 26);
            this.txtStoreTaxNumber.TabIndex = 7;
            // 
            // lblStoreTaxNumber
            // 
            this.lblStoreTaxNumber.AutoSize = true;
            this.lblStoreTaxNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblStoreTaxNumber.Location = new System.Drawing.Point(460, 45);
            this.lblStoreTaxNumber.Name = "lblStoreTaxNumber";
            this.lblStoreTaxNumber.Size = new System.Drawing.Size(102, 18);
            this.lblStoreTaxNumber.TabIndex = 6;
            this.lblStoreTaxNumber.Text = "الرقم الضريبي للطباعة:";
            // 
            // txtLogoPath
            // 
            this.txtLogoPath.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtLogoPath.Location = new System.Drawing.Point(20, 120);
            this.txtLogoPath.Name = "txtLogoPath";
            this.txtLogoPath.Size = new System.Drawing.Size(400, 26);
            this.txtLogoPath.TabIndex = 5;
            // 
            // lblLogoPath
            // 
            this.lblLogoPath.AutoSize = true;
            this.lblLogoPath.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblLogoPath.Location = new System.Drawing.Point(460, 10);
            this.lblLogoPath.Name = "lblLogoPath";
            this.lblLogoPath.Size = new System.Drawing.Size(87, 18);
            this.lblLogoPath.TabIndex = 4;
            this.lblLogoPath.Text = "مسار الشعار:";
            // 
            // txtA4PrinterName
            // 
            this.txtA4PrinterName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtA4PrinterName.Location = new System.Drawing.Point(20, 80);
            this.txtA4PrinterName.Name = "txtA4PrinterName";
            this.txtA4PrinterName.Size = new System.Drawing.Size(400, 26);
            this.txtA4PrinterName.TabIndex = 3;
            // 
            // lblA4PrinterName
            // 
            this.lblA4PrinterName.AutoSize = true;
            this.lblA4PrinterName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblA4PrinterName.Location = new System.Drawing.Point(460, 55);
            this.lblA4PrinterName.Name = "lblA4PrinterName";
            this.lblA4PrinterName.Size = new System.Drawing.Size(157, 18);
            this.lblA4PrinterName.TabIndex = 2;
            this.lblA4PrinterName.Text = "طابعة A4 (فواتير):";
            // 
            // txtThermalPrinterName
            // 
            this.txtThermalPrinterName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtThermalPrinterName.Location = new System.Drawing.Point(20, 40);
            this.txtThermalPrinterName.Name = "txtThermalPrinterName";
            this.txtThermalPrinterName.Size = new System.Drawing.Size(400, 26);
            this.txtThermalPrinterName.TabIndex = 1;
            // 
            // lblThermalPrinterName
            // 
            this.lblThermalPrinterName.AutoSize = true;
            this.lblThermalPrinterName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblThermalPrinterName.Location = new System.Drawing.Point(460, 15);
            this.lblThermalPrinterName.Name = "lblThermalPrinterName";
            this.lblThermalPrinterName.Size = new System.Drawing.Size(204, 18);
            this.lblThermalPrinterName.TabIndex = 0;
            this.lblThermalPrinterName.Text = "طابعة حرارية (إيصالات):";
            // 
            // tpNotifications
            // 
            this.tpNotifications.BackColor = System.Drawing.Color.White;
            this.tpNotifications.Controls.Add(this.numExpiryAlertDays);
            this.tpNotifications.Controls.Add(this.lblExpiryAlertDays);
            this.tpNotifications.Controls.Add(this.chkCreditLimitAlert);
            this.tpNotifications.Controls.Add(this.chkExpiryAlert);
            this.tpNotifications.Controls.Add(this.chkLowStockAlert);
            this.tpNotifications.Location = new System.Drawing.Point(4, 28);
            this.tpNotifications.Name = "tpNotifications";
            this.tpNotifications.Padding = new System.Windows.Forms.Padding(10);
            this.tpNotifications.Size = new System.Drawing.Size(868, 478);
            this.tpNotifications.TabIndex = 6;
            this.tpNotifications.Text = "التنبيهات";
            // 
            // numExpiryAlertDays
            // 
            this.numExpiryAlertDays.Location = new System.Drawing.Point(20, 120);
            this.numExpiryAlertDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            this.numExpiryAlertDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numExpiryAlertDays.Name = "numExpiryAlertDays";
            this.numExpiryAlertDays.Size = new System.Drawing.Size(100, 26);
            this.numExpiryAlertDays.TabIndex = 4;
            this.numExpiryAlertDays.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // lblExpiryAlertDays
            // 
            this.lblExpiryAlertDays.AutoSize = true;
            this.lblExpiryAlertDays.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblExpiryAlertDays.Location = new System.Drawing.Point(250, 122);
            this.lblExpiryAlertDays.Name = "lblExpiryAlertDays";
            this.lblExpiryAlertDays.Size = new System.Drawing.Size(121, 18);
            this.lblExpiryAlertDays.TabIndex = 3;
            this.lblExpiryAlertDays.Text = "تنبيه انتهاء الصلاحية (أيام):";
            // 
            // chkCreditLimitAlert
            // 
            this.chkCreditLimitAlert.AutoSize = true;
            this.chkCreditLimitAlert.Checked = true;
            this.chkCreditLimitAlert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreditLimitAlert.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkCreditLimitAlert.Location = new System.Drawing.Point(20, 80);
            this.chkCreditLimitAlert.Name = "chkCreditLimitAlert";
            this.chkCreditLimitAlert.Size = new System.Drawing.Size(185, 22);
            this.chkCreditLimitAlert.TabIndex = 2;
            this.chkCreditLimitAlert.Text = "تنبيه تجاوز الحد الائتماني";
            this.chkCreditLimitAlert.UseVisualStyleBackColor = true;
            // 
            // chkExpiryAlert
            // 
            this.chkExpiryAlert.AutoSize = true;
            this.chkExpiryAlert.Checked = true;
            this.chkExpiryAlert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExpiryAlert.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkExpiryAlert.Location = new System.Drawing.Point(20, 50);
            this.chkExpiryAlert.Name = "chkExpiryAlert";
            this.chkExpiryAlert.Size = new System.Drawing.Size(201, 22);
            this.chkExpiryAlert.TabIndex = 1;
            this.chkExpiryAlert.Text = "تنبيه انتهاء صلاحية المنتجات";
            this.chkExpiryAlert.UseVisualStyleBackColor = true;
            // 
            // chkLowStockAlert
            // 
            this.chkLowStockAlert.AutoSize = true;
            this.chkLowStockAlert.Checked = true;
            this.chkLowStockAlert.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLowStockAlert.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkLowStockAlert.Location = new System.Drawing.Point(20, 20);
            this.chkLowStockAlert.Name = "chkLowStockAlert";
            this.chkLowStockAlert.Size = new System.Drawing.Size(174, 22);
            this.chkLowStockAlert.TabIndex = 0;
            this.chkLowStockAlert.Text = "تنبيه انخفاض المخزون";
            this.chkLowStockAlert.UseVisualStyleBackColor = true;
            // 
            // tpAccounting
            // 
            this.tpAccounting.BackColor = System.Drawing.Color.White;
            this.tpAccounting.Controls.Add(this.chkAccountingAutoJE);
            this.tpAccounting.Location = new System.Drawing.Point(4, 28);
            this.tpAccounting.Name = "tpAccounting";
            this.tpAccounting.Padding = new System.Windows.Forms.Padding(10);
            this.tpAccounting.Size = new System.Drawing.Size(868, 478);
            this.tpAccounting.TabIndex = 7;
            this.tpAccounting.Text = "محاسبة";
            // 
            // chkAccountingAutoJE
            // 
            this.chkAccountingAutoJE.AutoSize = true;
            this.chkAccountingAutoJE.Checked = true;
            this.chkAccountingAutoJE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAccountingAutoJE.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkAccountingAutoJE.Location = new System.Drawing.Point(20, 20);
            this.chkAccountingAutoJE.Name = "chkAccountingAutoJE";
            this.chkAccountingAutoJE.Size = new System.Drawing.Size(172, 22);
            this.chkAccountingAutoJE.TabIndex = 0;
            this.chkAccountingAutoJE.Text = "إنشاء قيود يومية تلقائياً";
            this.chkAccountingAutoJE.UseVisualStyleBackColor = true;
            // 
            // tpBackup
            // 
            this.tpBackup.BackColor = System.Drawing.Color.White;
            this.tpBackup.Controls.Add(this.btnRestoreBackup);
            this.tpBackup.Controls.Add(this.lblBackupFiles);
            this.tpBackup.Controls.Add(this.lstBackupFiles);
            this.tpBackup.Controls.Add(this.btnCreateBackup);
            this.tpBackup.Controls.Add(this.btnBrowseBackupPath);
            this.tpBackup.Controls.Add(this.txtBackupPath);
            this.tpBackup.Controls.Add(this.lblBackupPath);
            this.tpBackup.Controls.Add(this.numBackupRetentionDays);
            this.tpBackup.Controls.Add(this.lblBackupRetentionDays);
            this.tpBackup.Controls.Add(this.txtBackupScheduleTime);
            this.tpBackup.Controls.Add(this.lblBackupScheduleTime);
            this.tpBackup.Location = new System.Drawing.Point(4, 28);
            this.tpBackup.Name = "tpBackup";
            this.tpBackup.Padding = new System.Windows.Forms.Padding(10);
            this.tpBackup.Size = new System.Drawing.Size(868, 478);
            this.tpBackup.TabIndex = 8;
            this.tpBackup.Text = "نسخ احتياطي";
            // 
            // btnBrowseBackupPath
            // 
            this.btnBrowseBackupPath.BackColor = System.Drawing.Color.White;
            this.btnBrowseBackupPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowseBackupPath.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnBrowseBackupPath.Location = new System.Drawing.Point(440, 20);
            this.btnBrowseBackupPath.Name = "btnBrowseBackupPath";
            this.btnBrowseBackupPath.Size = new System.Drawing.Size(100, 28);
            this.btnBrowseBackupPath.TabIndex = 8;
            this.btnBrowseBackupPath.Text = "استعراض";
            this.btnBrowseBackupPath.UseVisualStyleBackColor = false;
            this.btnBrowseBackupPath.Click += new System.EventHandler(this.btnBrowseBackupPath_Click);
            // 
            // txtBackupPath
            // 
            this.txtBackupPath.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtBackupPath.Location = new System.Drawing.Point(20, 20);
            this.txtBackupPath.Name = "txtBackupPath";
            this.txtBackupPath.Size = new System.Drawing.Size(410, 26);
            this.txtBackupPath.TabIndex = 7;
            // 
            // lblBackupPath
            // 
            this.lblBackupPath.AutoSize = true;
            this.lblBackupPath.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblBackupPath.Location = new System.Drawing.Point(560, 23);
            this.lblBackupPath.Name = "lblBackupPath";
            this.lblBackupPath.Size = new System.Drawing.Size(99, 18);
            this.lblBackupPath.TabIndex = 6;
            this.lblBackupPath.Text = "مسار النسخ الاحتياطي:";
            // 
            // numBackupRetentionDays
            // 
            this.numBackupRetentionDays.Location = new System.Drawing.Point(20, 65);
            this.numBackupRetentionDays.Maximum = new decimal(new int[] { 365, 0, 0, 0 });
            this.numBackupRetentionDays.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numBackupRetentionDays.Name = "numBackupRetentionDays";
            this.numBackupRetentionDays.Size = new System.Drawing.Size(100, 26);
            this.numBackupRetentionDays.TabIndex = 5;
            this.numBackupRetentionDays.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // lblBackupRetentionDays
            // 
            this.lblBackupRetentionDays.AutoSize = true;
            this.lblBackupRetentionDays.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblBackupRetentionDays.Location = new System.Drawing.Point(560, 67);
            this.lblBackupRetentionDays.Name = "lblBackupRetentionDays";
            this.lblBackupRetentionDays.Size = new System.Drawing.Size(169, 18);
            this.lblBackupRetentionDays.TabIndex = 4;
            this.lblBackupRetentionDays.Text = "الاحتفاظ بالنسخ (أيام):";
            // 
            // txtBackupScheduleTime
            // 
            this.txtBackupScheduleTime.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtBackupScheduleTime.Location = new System.Drawing.Point(20, 110);
            this.txtBackupScheduleTime.Name = "txtBackupScheduleTime";
            this.txtBackupScheduleTime.Size = new System.Drawing.Size(100, 26);
            this.txtBackupScheduleTime.TabIndex = 3;
            this.txtBackupScheduleTime.Text = "02:00";
            // 
            // lblBackupScheduleTime
            // 
            this.lblBackupScheduleTime.AutoSize = true;
            this.lblBackupScheduleTime.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblBackupScheduleTime.Location = new System.Drawing.Point(560, 113);
            this.lblBackupScheduleTime.Name = "lblBackupScheduleTime";
            this.lblBackupScheduleTime.Size = new System.Drawing.Size(140, 18);
            this.lblBackupScheduleTime.TabIndex = 2;
            this.lblBackupScheduleTime.Text = "وقت النسخ المجدول:";
            // 
            // btnCreateBackup
            // 
            this.btnCreateBackup.BackColor = System.Drawing.Color.White;
            this.btnCreateBackup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreateBackup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnCreateBackup.ForeColor = System.Drawing.Color.Black;
            this.btnCreateBackup.Location = new System.Drawing.Point(20, 160);
            this.btnCreateBackup.Name = "btnCreateBackup";
            this.btnCreateBackup.Size = new System.Drawing.Size(150, 40);
            this.btnCreateBackup.TabIndex = 9;
            this.btnCreateBackup.Text = "إنشاء نسخة احتياطية";
            this.btnCreateBackup.UseVisualStyleBackColor = false;
            this.btnCreateBackup.Click += new System.EventHandler(this.btnCreateBackup_Click);
            // 
            // lstBackupFiles
            // 
            this.lstBackupFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBackupFiles.FormattingEnabled = true;
            this.lstBackupFiles.ItemHeight = 18;
            this.lstBackupFiles.Location = new System.Drawing.Point(20, 230);
            this.lstBackupFiles.Name = "lstBackupFiles";
            this.lstBackupFiles.Size = new System.Drawing.Size(630, 220);
            this.lstBackupFiles.TabIndex = 10;
            // 
            // lblBackupFiles
            // 
            this.lblBackupFiles.AutoSize = true;
            this.lblBackupFiles.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblBackupFiles.Location = new System.Drawing.Point(560, 210);
            this.lblBackupFiles.Name = "lblBackupFiles";
            this.lblBackupFiles.Size = new System.Drawing.Size(100, 18);
            this.lblBackupFiles.TabIndex = 11;
            this.lblBackupFiles.Text = "النسخ الاحتياطية:";
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.BackColor = System.Drawing.Color.White;
            this.btnRestoreBackup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRestoreBackup.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnRestoreBackup.ForeColor = System.Drawing.Color.Black;
            this.btnRestoreBackup.Location = new System.Drawing.Point(190, 160);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(150, 40);
            this.btnRestoreBackup.TabIndex = 12;
            this.btnRestoreBackup.Text = "استعادة النسخة الاحتياطية";
            this.btnRestoreBackup.UseVisualStyleBackColor = false;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(240, 570);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 42);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "حفظ التغييرات";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnClose.Location = new System.Drawing.Point(12, 570);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 36);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // openFileDialogLogo
            // 
            this.openFileDialogLogo.Filter = "ملفات الصور|*.png;*.jpg;*.jpeg;*.bmp|كل الملفات|*.*";
            this.openFileDialogLogo.Title = "اختيار شعار للطباعة";
            // 
            // frmSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabSettings);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إعدادات النظام";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tabSettings.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultWarehouse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultBranch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultPurchaseTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultSalesTax)).EndInit();
            this.tpInventory.ResumeLayout(false);
            this.tpInventory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStockAlertDays)).EndInit();
            this.tpSales.ResumeLayout(false);
            this.tpSales.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultCashCustomerId)).EndInit();
            this.tpPurchases.ResumeLayout(false);
            this.tpPurchases.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultCashSupplierId)).EndInit();
            this.tpBarcode.ResumeLayout(false);
            this.tpBarcode.PerformLayout();
            this.tpPrint.ResumeLayout(false);
            this.tpPrint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrintCopies)).EndInit();
            this.tpNotifications.ResumeLayout(false);
            this.tpNotifications.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExpiryAlertDays)).EndInit();
            this.tpAccounting.ResumeLayout(false);
            this.tpAccounting.PerformLayout();
            this.tpBackup.ResumeLayout(false);
            this.tpBackup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackupRetentionDays)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpInventory;
        private System.Windows.Forms.TabPage tpSales;
        private System.Windows.Forms.TabPage tpPurchases;
        private System.Windows.Forms.TabPage tpBarcode;
        private System.Windows.Forms.TabPage tpPrint;
        private System.Windows.Forms.TabPage tpNotifications;
        private System.Windows.Forms.TabPage tpAccounting;
        private System.Windows.Forms.TabPage tpBackup;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;

        // General tab
        private System.Windows.Forms.CheckBox chkAutoCreateJE;
        private System.Windows.Forms.CheckBox chkAllowNegativeCash;
        private System.Windows.Forms.CheckBox chkEnableAttachments;
        private System.Windows.Forms.CheckBox chkEnableNotifications;
        private System.Windows.Forms.NumericUpDown numDefaultSalesTax;
        private System.Windows.Forms.Label lblDefaultSalesTax;
        private System.Windows.Forms.NumericUpDown numDefaultPurchaseTax;
        private System.Windows.Forms.Label lblDefaultPurchaseTax;
        private System.Windows.Forms.NumericUpDown numDefaultBranch;
        private System.Windows.Forms.Label lblDefaultBranch;
        private System.Windows.Forms.NumericUpDown numDefaultWarehouse;
        private System.Windows.Forms.Label lblDefaultWarehouse;

        // Inventory tab
        private System.Windows.Forms.CheckBox chkAllowNegativeStock;
        private System.Windows.Forms.CheckBox chkEnableFefo;
        private System.Windows.Forms.CheckBox chkRequireBatchOnPurchase;
        private System.Windows.Forms.CheckBox chkRequireExpiryOnPurchase;
        private System.Windows.Forms.NumericUpDown numStockAlertDays;
        private System.Windows.Forms.Label lblStockAlertDays;

        // Sales tab
        private System.Windows.Forms.NumericUpDown numDefaultCashCustomerId;
        private System.Windows.Forms.Label lblDefaultCashCustomerId;
        private System.Windows.Forms.CheckBox chkPreventBelowRetailPrice;
        private System.Windows.Forms.CheckBox chkAllowBelowCostSale;
        private System.Windows.Forms.CheckBox chkAutoPostInvoices;
        private System.Windows.Forms.CheckBox chkAllowDrafts;
        private System.Windows.Forms.CheckBox chkShowProfitInInvoice;
        private System.Windows.Forms.CheckBox chkHideTaxInSales;
        private System.Windows.Forms.CheckBox chkShowExpiryInInvoices;
        private System.Windows.Forms.CheckBox chkAutoPrintAfterPosting;

        // Purchases tab
        private System.Windows.Forms.NumericUpDown numDefaultCashSupplierId;
        private System.Windows.Forms.Label lblDefaultCashSupplierId;
        private System.Windows.Forms.CheckBox chkPurchaseAutoPost;
        private System.Windows.Forms.CheckBox chkHideTaxInPurchases;

        // Barcode tab
        private System.Windows.Forms.CheckBox chkAutoGenerateBarcode;
        private System.Windows.Forms.CheckBox chkAllowDuplicateBarcode;

        // Print tab
        private System.Windows.Forms.TextBox txtThermalPrinterName;
        private System.Windows.Forms.Label lblThermalPrinterName;
        private System.Windows.Forms.TextBox txtA4PrinterName;
        private System.Windows.Forms.Label lblA4PrinterName;
        private System.Windows.Forms.TextBox txtLogoPath;
        private System.Windows.Forms.Label lblLogoPath;
        private System.Windows.Forms.TextBox txtStoreTaxNumber;
        private System.Windows.Forms.Label lblStoreTaxNumber;
        private System.Windows.Forms.TextBox txtFooterNote;
        private System.Windows.Forms.Label lblFooterNote;
        private System.Windows.Forms.TextBox txtPaperSize;
        private System.Windows.Forms.Label lblPaperSize;
        private System.Windows.Forms.NumericUpDown numPrintCopies;
        private System.Windows.Forms.Label lblPrintCopies;
        private System.Windows.Forms.CheckBox chkShowLogo;
        private System.Windows.Forms.CheckBox chkShowBalanceOnPrint;
        private System.Windows.Forms.CheckBox chkPrintSignature;
        private System.Windows.Forms.CheckBox chkPrintBarcode;
        private System.Windows.Forms.CheckBox chkPrintQRCode;
        private System.Windows.Forms.CheckBox chkPrintCompanyAddress;

        // Notifications tab
        private System.Windows.Forms.CheckBox chkLowStockAlert;
        private System.Windows.Forms.CheckBox chkExpiryAlert;
        private System.Windows.Forms.CheckBox chkCreditLimitAlert;
        private System.Windows.Forms.NumericUpDown numExpiryAlertDays;
        private System.Windows.Forms.Label lblExpiryAlertDays;

        // Accounting tab
        private System.Windows.Forms.CheckBox chkAccountingAutoJE;

        // Backup tab
        private System.Windows.Forms.TextBox txtBackupPath;
        private System.Windows.Forms.Label lblBackupPath;
        private System.Windows.Forms.NumericUpDown numBackupRetentionDays;
        private System.Windows.Forms.Label lblBackupRetentionDays;
        private System.Windows.Forms.TextBox txtBackupScheduleTime;
        private System.Windows.Forms.Label lblBackupScheduleTime;
        private System.Windows.Forms.Button btnBrowseBackupPath;
        private System.Windows.Forms.Button btnCreateBackup;
        private System.Windows.Forms.ListBox lstBackupFiles;
        private System.Windows.Forms.Label lblBackupFiles;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.OpenFileDialog openFileDialogLogo;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}
