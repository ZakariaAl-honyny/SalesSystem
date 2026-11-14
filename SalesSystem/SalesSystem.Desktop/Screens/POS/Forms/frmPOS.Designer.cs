namespace SalesSystem.Desktop.Screens.POS.Forms
{
    partial class frmPOS
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblInvoiceInfo = new Label();
            lblMode = new Label();
            gbSalesCommands = new GroupBox();
            pnlTop = new Panel();
            flpTopBar = new FlowLayoutPanel();
            flpBarcode = new FlowLayoutPanel();
            lblBarcodeCaption = new Label();
            txtBarcode = new TextBox();
            flpProductSearch = new FlowLayoutPanel();
            lblProductSearchCaption = new Label();
            txtProductSearch = new TextBox();
            flpCategory = new FlowLayoutPanel();
            lblCategoryCaption = new Label();
            cbCategory = new ComboBox();
            btnRefreshCatalog = new Button();
            gbCustomerPayment = new GroupBox();
            pnlPayment = new Panel();
            flpPayment = new FlowLayoutPanel();
            flpCustomer = new FlowLayoutPanel();
            lblCustomerCaption = new Label();
            cbCustomer = new ComboBox();
            flpWarehouse = new FlowLayoutPanel();
            lblWarehouseCaption = new Label();
            cbWarehouse = new ComboBox();
            flpCashBox = new FlowLayoutPanel();
            lblCashBoxCaption = new Label();
            cbCashBox = new ComboBox();
            rbtnCash = new RadioButton();
            rbtnCredit = new RadioButton();
            flpPaid = new FlowLayoutPanel();
            lblPaidCaption = new Label();
            numPaid = new NumericUpDown();
            lblRemaining = new Label();
            gbCart = new GroupBox();
            pnlCart = new Panel();
            dgvCart = new DataGridView();
            colLineNo = new DataGridViewTextBoxColumn();
            colProduct = new DataGridViewTextBoxColumn();
            colBarcode = new DataGridViewTextBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colUnitPrice = new DataGridViewTextBoxColumn();
            colDiscount = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            pnlLine = new Panel();
            flpLine = new FlowLayoutPanel();
            lblSelectedProduct = new Label();
            flpQty = new FlowLayoutPanel();
            lblQtyCaption = new Label();
            numQty = new NumericUpDown();
            flpPrice = new FlowLayoutPanel();
            lblPriceCaption = new Label();
            numUnitPrice = new NumericUpDown();
            flpLineDiscount = new FlowLayoutPanel();
            lblLineDiscountCaption = new Label();
            numLineDiscount = new NumericUpDown();
            btnApplyLine = new Button();
            btnRemoveLine = new Button();
            pnlProducts = new Panel();
            flpProducts = new FlowLayoutPanel();
            gbTotals = new GroupBox();
            pnlTotals = new Panel();
            flpTotals = new FlowLayoutPanel();
            lblSubTotal = new Label();
            lblDiscountTotal = new Label();
            lblNetTotal = new Label();
            flpInvoiceDiscount = new FlowLayoutPanel();
            lblInvoiceDiscountCaption = new Label();
            numInvoiceDiscount = new NumericUpDown();
            lblTafqit = new Label();
            pnlActions = new Panel();
            flpActions = new FlowLayoutPanel();
            btnSave = new Button();
            btnNew = new Button();
            btnClose = new Button();
            toolTip = new ToolTip(components);
            errorProvider = new ErrorProvider(components);
            pnlHeader.SuspendLayout();
            gbSalesCommands.SuspendLayout();
            pnlTop.SuspendLayout();
            flpTopBar.SuspendLayout();
            flpBarcode.SuspendLayout();
            flpProductSearch.SuspendLayout();
            flpCategory.SuspendLayout();
            gbCustomerPayment.SuspendLayout();
            pnlPayment.SuspendLayout();
            flpPayment.SuspendLayout();
            flpCustomer.SuspendLayout();
            flpWarehouse.SuspendLayout();
            flpCashBox.SuspendLayout();
            flpPaid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPaid).BeginInit();
            gbCart.SuspendLayout();
            pnlCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            pnlLine.SuspendLayout();
            flpLine.SuspendLayout();
            flpQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            flpPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUnitPrice).BeginInit();
            flpLineDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLineDiscount).BeginInit();
            pnlProducts.SuspendLayout();
            gbTotals.SuspendLayout();
            pnlTotals.SuspendLayout();
            flpTotals.SuspendLayout();
            flpInvoiceDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).BeginInit();
            pnlActions.SuspendLayout();
            flpActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader — شريط العنوان (RULE-N135 البند 1)
            // 
            pnlHeader.BackColor = Color.FromArgb(170, 220, 170);
            pnlHeader.Controls.Add(lblInvoiceInfo);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(8, 6, 8, 4);
            pnlHeader.Size = new Size(1280, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblInvoiceInfo
            // 
            lblInvoiceInfo.BackColor = Color.FromArgb(14, 165, 103);
            lblInvoiceInfo.Dock = DockStyle.Fill;
            lblInvoiceInfo.Font = new Font("Tahoma", 9F);
            lblInvoiceInfo.ForeColor = Color.White;
            lblInvoiceInfo.Location = new Point(8, 36);
            lblInvoiceInfo.Name = "lblInvoiceInfo";
            lblInvoiceInfo.Size = new Size(1264, 16);
            lblInvoiceInfo.TabIndex = 1;
            lblInvoiceInfo.Text = "التاريخ: —   |   المخزن: —   |   الصندوق: —";
            lblInvoiceInfo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(8, 6);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1264, 30);
            lblMode.TabIndex = 0;
            lblMode.Text = "نقطة البيع — فاتورة جديدة";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbSalesCommands — أوامر البيع: باركود + بحث + تصنيف + تحديث (RULE-N135 البند 2)
            // 
            gbSalesCommands.BackColor = Color.FromArgb(235, 245, 235);
            gbSalesCommands.Controls.Add(pnlTop);
            gbSalesCommands.Dock = DockStyle.Top;
            gbSalesCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbSalesCommands.ForeColor = Color.FromArgb(14, 82, 52);
            gbSalesCommands.Location = new Point(0, 56);
            gbSalesCommands.Name = "gbSalesCommands";
            gbSalesCommands.Padding = new Padding(6, 22, 6, 6);
            gbSalesCommands.Size = new Size(1280, 108);
            gbSalesCommands.TabIndex = 1;
            gbSalesCommands.TabStop = false;
            gbSalesCommands.Text = "أوامر البيع";
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(235, 240, 235);
            pnlTop.Controls.Add(flpTopBar);
            pnlTop.Dock = DockStyle.Fill;
            pnlTop.Location = new Point(6, 22);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(6, 4, 6, 4);
            pnlTop.Size = new Size(1268, 80);
            pnlTop.TabIndex = 0;
            // 
            // flpTopBar
            // 
            flpTopBar.BackColor = Color.FromArgb(235, 240, 235);
            flpTopBar.Controls.Add(flpBarcode);
            flpTopBar.Controls.Add(flpProductSearch);
            flpTopBar.Controls.Add(flpCategory);
            flpTopBar.Controls.Add(btnRefreshCatalog);
            flpTopBar.Dock = DockStyle.Fill;
            flpTopBar.FlowDirection = FlowDirection.RightToLeft;
            flpTopBar.Location = new Point(6, 4);
            flpTopBar.Name = "flpTopBar";
            flpTopBar.Size = new Size(1256, 72);
            flpTopBar.TabIndex = 0;
            flpTopBar.WrapContents = false;
            // 
            // flpBarcode
            // 
            flpBarcode.AutoSize = true;
            flpBarcode.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpBarcode.BackColor = Color.FromArgb(235, 240, 235);
            flpBarcode.Controls.Add(lblBarcodeCaption);
            flpBarcode.Controls.Add(txtBarcode);
            flpBarcode.FlowDirection = FlowDirection.RightToLeft;
            flpBarcode.Location = new Point(848, 4);
            flpBarcode.Margin = new Padding(4);
            flpBarcode.Name = "flpBarcode";
            flpBarcode.Size = new Size(404, 44);
            flpBarcode.TabIndex = 0;
            flpBarcode.WrapContents = false;
            // 
            // lblBarcodeCaption
            // 
            lblBarcodeCaption.AutoSize = true;
            lblBarcodeCaption.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblBarcodeCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblBarcodeCaption.Location = new Point(316, 10);
            lblBarcodeCaption.Margin = new Padding(0, 10, 4, 0);
            lblBarcodeCaption.Name = "lblBarcodeCaption";
            lblBarcodeCaption.Size = new Size(84, 26);
            lblBarcodeCaption.TabIndex = 0;
            lblBarcodeCaption.Text = "الباركود:";
            lblBarcodeCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtBarcode
            // 
            txtBarcode.BackColor = Color.White;
            txtBarcode.Font = new Font("Tahoma", 15F, FontStyle.Bold);
            txtBarcode.ForeColor = Color.FromArgb(33, 37, 41);
            txtBarcode.Location = new Point(6, 3);
            txtBarcode.Margin = new Padding(4, 3, 4, 3);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "امسح الباركود ثم اضغط Enter";
            txtBarcode.Size = new Size(306, 38);
            txtBarcode.TabIndex = 1;
            txtBarcode.TextAlign = HorizontalAlignment.Center;
            // 
            // flpProductSearch
            // 
            flpProductSearch.AutoSize = true;
            flpProductSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpProductSearch.BackColor = Color.FromArgb(235, 240, 235);
            flpProductSearch.Controls.Add(lblProductSearchCaption);
            flpProductSearch.Controls.Add(txtProductSearch);
            flpProductSearch.FlowDirection = FlowDirection.RightToLeft;
            flpProductSearch.Location = new Point(528, 4);
            flpProductSearch.Margin = new Padding(4);
            flpProductSearch.Name = "flpProductSearch";
            flpProductSearch.Size = new Size(312, 42);
            flpProductSearch.TabIndex = 1;
            flpProductSearch.WrapContents = false;
            // 
            // lblProductSearchCaption
            // 
            lblProductSearchCaption.AutoSize = true;
            lblProductSearchCaption.Font = new Font("Tahoma", 10F);
            lblProductSearchCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblProductSearchCaption.Location = new Point(238, 8);
            lblProductSearchCaption.Margin = new Padding(0, 8, 4, 0);
            lblProductSearchCaption.Name = "lblProductSearchCaption";
            lblProductSearchCaption.Size = new Size(70, 24);
            lblProductSearchCaption.TabIndex = 0;
            lblProductSearchCaption.Text = "بحث الأصناف:";
            lblProductSearchCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtProductSearch
            // 
            txtProductSearch.BackColor = Color.White;
            txtProductSearch.Font = new Font("Tahoma", 11F);
            txtProductSearch.ForeColor = Color.FromArgb(33, 37, 41);
            txtProductSearch.Location = new Point(6, 3);
            txtProductSearch.Margin = new Padding(4, 3, 4, 3);
            txtProductSearch.Name = "txtProductSearch";
            txtProductSearch.PlaceholderText = "اسم الصنف أو الباركود";
            txtProductSearch.Size = new Size(228, 30);
            txtProductSearch.TabIndex = 1;
            // 
            // flpCategory
            // 
            flpCategory.AutoSize = true;
            flpCategory.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCategory.BackColor = Color.FromArgb(235, 240, 235);
            flpCategory.Controls.Add(lblCategoryCaption);
            flpCategory.Controls.Add(cbCategory);
            flpCategory.FlowDirection = FlowDirection.RightToLeft;
            flpCategory.Location = new Point(308, 4);
            flpCategory.Margin = new Padding(4);
            flpCategory.Name = "flpCategory";
            flpCategory.Size = new Size(212, 42);
            flpCategory.TabIndex = 2;
            flpCategory.WrapContents = false;
            // 
            // lblCategoryCaption
            // 
            lblCategoryCaption.AutoSize = true;
            lblCategoryCaption.Font = new Font("Tahoma", 10F);
            lblCategoryCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblCategoryCaption.Location = new Point(166, 8);
            lblCategoryCaption.Margin = new Padding(0, 8, 4, 0);
            lblCategoryCaption.Name = "lblCategoryCaption";
            lblCategoryCaption.Size = new Size(42, 24);
            lblCategoryCaption.TabIndex = 0;
            lblCategoryCaption.Text = "التصنيف:";
            lblCategoryCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCategory
            // 
            cbCategory.BackColor = Color.White;
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategory.FlatStyle = FlatStyle.Flat;
            cbCategory.Font = new Font("Tahoma", 10F);
            cbCategory.ForeColor = Color.FromArgb(33, 37, 41);
            cbCategory.FormattingEnabled = true;
            cbCategory.Location = new Point(6, 3);
            cbCategory.Margin = new Padding(4, 3, 4, 3);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(156, 30);
            cbCategory.TabIndex = 1;
            // 
            // btnRefreshCatalog
            // 
            btnRefreshCatalog.BackColor = Color.FromArgb(248, 249, 250);
            btnRefreshCatalog.Cursor = Cursors.Hand;
            btnRefreshCatalog.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRefreshCatalog.FlatAppearance.BorderSize = 1;
            btnRefreshCatalog.FlatStyle = FlatStyle.Flat;
            btnRefreshCatalog.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefreshCatalog.ForeColor = Color.FromArgb(33, 37, 41);
            btnRefreshCatalog.Location = new Point(158, 4);
            btnRefreshCatalog.Margin = new Padding(4);
            btnRefreshCatalog.Name = "btnRefreshCatalog";
            btnRefreshCatalog.Size = new Size(142, 40);
            btnRefreshCatalog.TabIndex = 3;
            btnRefreshCatalog.Text = "تحديث الأصناف [F5]";
            btnRefreshCatalog.UseVisualStyleBackColor = false;
            // 
            // gbCustomerPayment — بيانات العميل/المخزن/الصندوق/الدفع (RULE-N135 البند 3)
            // 
            gbCustomerPayment.BackColor = Color.FromArgb(235, 245, 235);
            gbCustomerPayment.Controls.Add(pnlPayment);
            gbCustomerPayment.Dock = DockStyle.Top;
            gbCustomerPayment.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCustomerPayment.ForeColor = Color.FromArgb(14, 82, 52);
            gbCustomerPayment.Location = new Point(0, 164);
            gbCustomerPayment.Name = "gbCustomerPayment";
            gbCustomerPayment.Padding = new Padding(6, 22, 6, 6);
            gbCustomerPayment.Size = new Size(1280, 96);
            gbCustomerPayment.TabIndex = 2;
            gbCustomerPayment.TabStop = false;
            gbCustomerPayment.Text = "العميل والدفع";
            // 
            // pnlPayment
            // 
            pnlPayment.BackColor = Color.FromArgb(235, 240, 235);
            pnlPayment.Controls.Add(flpPayment);
            pnlPayment.Dock = DockStyle.Fill;
            pnlPayment.Location = new Point(6, 22);
            pnlPayment.Name = "pnlPayment";
            pnlPayment.Padding = new Padding(6, 4, 6, 4);
            pnlPayment.Size = new Size(1268, 68);
            pnlPayment.TabIndex = 0;
            // 
            // flpPayment
            // 
            flpPayment.BackColor = Color.FromArgb(235, 240, 235);
            flpPayment.Controls.Add(flpCustomer);
            flpPayment.Controls.Add(flpWarehouse);
            flpPayment.Controls.Add(flpCashBox);
            flpPayment.Controls.Add(rbtnCash);
            flpPayment.Controls.Add(rbtnCredit);
            flpPayment.Controls.Add(flpPaid);
            flpPayment.Controls.Add(lblRemaining);
            flpPayment.Dock = DockStyle.Fill;
            flpPayment.FlowDirection = FlowDirection.RightToLeft;
            flpPayment.Location = new Point(6, 4);
            flpPayment.Name = "flpPayment";
            flpPayment.Size = new Size(1256, 60);
            flpPayment.TabIndex = 0;
            flpPayment.WrapContents = false;
            // 
            // flpCustomer
            // 
            flpCustomer.AutoSize = true;
            flpCustomer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCustomer.BackColor = Color.FromArgb(235, 240, 235);
            flpCustomer.Controls.Add(lblCustomerCaption);
            flpCustomer.Controls.Add(cbCustomer);
            flpCustomer.FlowDirection = FlowDirection.RightToLeft;
            flpCustomer.Location = new Point(984, 4);
            flpCustomer.Margin = new Padding(4);
            flpCustomer.Name = "flpCustomer";
            flpCustomer.Size = new Size(268, 36);
            flpCustomer.TabIndex = 0;
            flpCustomer.WrapContents = false;
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.AutoSize = true;
            lblCustomerCaption.Font = new Font("Tahoma", 10F);
            lblCustomerCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblCustomerCaption.Location = new Point(218, 6);
            lblCustomerCaption.Margin = new Padding(0, 6, 4, 0);
            lblCustomerCaption.Name = "lblCustomerCaption";
            lblCustomerCaption.Size = new Size(46, 24);
            lblCustomerCaption.TabIndex = 0;
            lblCustomerCaption.Text = "العميل:";
            lblCustomerCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCustomer
            // 
            cbCustomer.BackColor = Color.White;
            cbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCustomer.FlatStyle = FlatStyle.Flat;
            cbCustomer.Font = new Font("Tahoma", 10F);
            cbCustomer.ForeColor = Color.FromArgb(33, 37, 41);
            cbCustomer.FormattingEnabled = true;
            cbCustomer.Location = new Point(4, 3);
            cbCustomer.Margin = new Padding(4, 3, 4, 3);
            cbCustomer.Name = "cbCustomer";
            cbCustomer.Size = new Size(180, 30);
            cbCustomer.TabIndex = 1;
            // 
            // flpWarehouse
            // 
            flpWarehouse.AutoSize = true;
            flpWarehouse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpWarehouse.BackColor = Color.FromArgb(235, 240, 235);
            flpWarehouse.Controls.Add(lblWarehouseCaption);
            flpWarehouse.Controls.Add(cbWarehouse);
            flpWarehouse.FlowDirection = FlowDirection.RightToLeft;
            flpWarehouse.Location = new Point(756, 4);
            flpWarehouse.Margin = new Padding(4);
            flpWarehouse.Name = "flpWarehouse";
            flpWarehouse.Size = new Size(220, 36);
            flpWarehouse.TabIndex = 1;
            flpWarehouse.WrapContents = false;
            // 
            // lblWarehouseCaption
            // 
            lblWarehouseCaption.AutoSize = true;
            lblWarehouseCaption.Font = new Font("Tahoma", 10F);
            lblWarehouseCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblWarehouseCaption.Location = new Point(174, 6);
            lblWarehouseCaption.Margin = new Padding(0, 6, 4, 0);
            lblWarehouseCaption.Name = "lblWarehouseCaption";
            lblWarehouseCaption.Size = new Size(42, 24);
            lblWarehouseCaption.TabIndex = 0;
            lblWarehouseCaption.Text = "المخزن:";
            lblWarehouseCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbWarehouse
            // 
            cbWarehouse.BackColor = Color.White;
            cbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWarehouse.FlatStyle = FlatStyle.Flat;
            cbWarehouse.Font = new Font("Tahoma", 10F);
            cbWarehouse.ForeColor = Color.FromArgb(33, 37, 41);
            cbWarehouse.FormattingEnabled = true;
            cbWarehouse.Location = new Point(4, 3);
            cbWarehouse.Margin = new Padding(4, 3, 4, 3);
            cbWarehouse.Name = "cbWarehouse";
            cbWarehouse.Size = new Size(140, 30);
            cbWarehouse.TabIndex = 1;
            // 
            // flpCashBox
            // 
            flpCashBox.AutoSize = true;
            flpCashBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCashBox.BackColor = Color.FromArgb(235, 240, 235);
            flpCashBox.Controls.Add(lblCashBoxCaption);
            flpCashBox.Controls.Add(cbCashBox);
            flpCashBox.FlowDirection = FlowDirection.RightToLeft;
            flpCashBox.Location = new Point(536, 4);
            flpCashBox.Margin = new Padding(4);
            flpCashBox.Name = "flpCashBox";
            flpCashBox.Size = new Size(212, 36);
            flpCashBox.TabIndex = 2;
            flpCashBox.WrapContents = false;
            // 
            // lblCashBoxCaption
            // 
            lblCashBoxCaption.AutoSize = true;
            lblCashBoxCaption.Font = new Font("Tahoma", 10F);
            lblCashBoxCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblCashBoxCaption.Location = new Point(166, 6);
            lblCashBoxCaption.Margin = new Padding(0, 6, 4, 0);
            lblCashBoxCaption.Name = "lblCashBoxCaption";
            lblCashBoxCaption.Size = new Size(50, 24);
            lblCashBoxCaption.TabIndex = 0;
            lblCashBoxCaption.Text = "الصندوق:";
            lblCashBoxCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCashBox
            // 
            cbCashBox.BackColor = Color.White;
            cbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCashBox.FlatStyle = FlatStyle.Flat;
            cbCashBox.Font = new Font("Tahoma", 10F);
            cbCashBox.ForeColor = Color.FromArgb(33, 37, 41);
            cbCashBox.FormattingEnabled = true;
            cbCashBox.Location = new Point(4, 3);
            cbCashBox.Margin = new Padding(4, 3, 4, 3);
            cbCashBox.Name = "cbCashBox";
            cbCashBox.Size = new Size(130, 30);
            cbCashBox.TabIndex = 1;
            // 
            // rbtnCash
            // 
            rbtnCash.AutoSize = true;
            rbtnCash.BackColor = Color.FromArgb(235, 240, 235);
            rbtnCash.Checked = true;
            rbtnCash.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbtnCash.ForeColor = Color.FromArgb(14, 82, 52);
            rbtnCash.Location = new Point(450, 4);
            rbtnCash.Margin = new Padding(4);
            rbtnCash.Name = "rbtnCash";
            rbtnCash.Size = new Size(78, 28);
            rbtnCash.TabIndex = 3;
            rbtnCash.TabStop = true;
            rbtnCash.Text = "نقدي";
            rbtnCash.UseVisualStyleBackColor = false;
            // 
            // rbtnCredit
            // 
            rbtnCredit.AutoSize = true;
            rbtnCredit.BackColor = Color.FromArgb(235, 240, 235);
            rbtnCredit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbtnCredit.ForeColor = Color.FromArgb(14, 82, 52);
            rbtnCredit.Location = new Point(370, 4);
            rbtnCredit.Margin = new Padding(4);
            rbtnCredit.Name = "rbtnCredit";
            rbtnCredit.Size = new Size(72, 28);
            rbtnCredit.TabIndex = 4;
            rbtnCredit.Text = "آجل";
            rbtnCredit.UseVisualStyleBackColor = false;
            // 
            // flpPaid
            // 
            flpPaid.AutoSize = true;
            flpPaid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPaid.BackColor = Color.FromArgb(235, 240, 235);
            flpPaid.Controls.Add(lblPaidCaption);
            flpPaid.Controls.Add(numPaid);
            flpPaid.FlowDirection = FlowDirection.RightToLeft;
            flpPaid.Location = new Point(228, 4);
            flpPaid.Margin = new Padding(4);
            flpPaid.Name = "flpPaid";
            flpPaid.Size = new Size(134, 36);
            flpPaid.TabIndex = 5;
            flpPaid.WrapContents = false;
            // 
            // lblPaidCaption
            // 
            lblPaidCaption.AutoSize = true;
            lblPaidCaption.Font = new Font("Tahoma", 10F);
            lblPaidCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblPaidCaption.Location = new Point(88, 6);
            lblPaidCaption.Margin = new Padding(0, 6, 4, 0);
            lblPaidCaption.Name = "lblPaidCaption";
            lblPaidCaption.Size = new Size(42, 24);
            lblPaidCaption.TabIndex = 0;
            lblPaidCaption.Text = "المدفوع:";
            lblPaidCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numPaid
            // 
            numPaid.BackColor = Color.White;
            numPaid.DecimalPlaces = 2;
            numPaid.Font = new Font("Tahoma", 10F);
            numPaid.ForeColor = Color.FromArgb(33, 37, 41);
            numPaid.Location = new Point(4, 3);
            numPaid.Margin = new Padding(4, 3, 4, 3);
            numPaid.Maximum = 100000000M;
            numPaid.Name = "numPaid";
            numPaid.Size = new Size(80, 30);
            numPaid.TabIndex = 1;
            numPaid.TextAlign = HorizontalAlignment.Center;
            // 
            // lblRemaining
            // 
            lblRemaining.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblRemaining.ForeColor = Color.FromArgb(14, 82, 52);
            lblRemaining.Location = new Point(20, 4);
            lblRemaining.Margin = new Padding(4);
            lblRemaining.Name = "lblRemaining";
            lblRemaining.Size = new Size(200, 34);
            lblRemaining.TabIndex = 6;
            lblRemaining.Text = "الباقي: 0.00";
            lblRemaining.TextAlign = ContentAlignment.MiddleRight;
            // 
            // gbCart — سلة الأصناف + محرر السطر (RULE-N135 البند 4)
            // 
            gbCart.BackColor = Color.FromArgb(235, 245, 235);
            gbCart.Controls.Add(pnlCart);
            gbCart.Controls.Add(pnlProducts);
            gbCart.Controls.Add(pnlLine);
            gbCart.Dock = DockStyle.Fill;
            gbCart.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCart.ForeColor = Color.FromArgb(14, 82, 52);
            gbCart.Location = new Point(0, 260);
            gbCart.Name = "gbCart";
            gbCart.Padding = new Padding(6, 22, 6, 6);
            gbCart.Size = new Size(1280, 496);
            gbCart.TabIndex = 3;
            gbCart.TabStop = false;
            gbCart.Text = "سلة الأصناف";
            // 
            // pnlCart
            // 
            pnlCart.BackColor = Color.FromArgb(235, 240, 235);
            pnlCart.Controls.Add(dgvCart);
            pnlCart.Dock = DockStyle.Fill;
            pnlCart.Location = new Point(6, 22);
            pnlCart.Name = "pnlCart";
            pnlCart.Size = new Size(888, 406);
            pnlCart.TabIndex = 0;
            // 
            // dgvCart — RULE-N22a / RULE-N83: شبكة سلة قابلة للتحرير داخل الخلية
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AllowUserToResizeRows = false;
            dgvCart.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvCart.AutoGenerateColumns = false;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvCart.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCart.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvCart.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvCart.ColumnHeadersHeight = 45;
            dgvCart.Columns.AddRange(new DataGridViewColumn[] { colLineNo, colProduct, colBarcode, colQuantity, colUnitPrice, colDiscount, colTotal });
            dgvCart.DefaultCellStyle.BackColor = Color.White;
            dgvCart.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvCart.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvCart.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvCart.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.GridColor = Color.FromArgb(205, 225, 205);
            dgvCart.Location = new Point(0, 0);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = false;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowTemplate.Height = 32;
            dgvCart.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCart.Size = new Size(888, 406);
            dgvCart.TabIndex = 0;
            // 
            // colLineNo
            // 
            colLineNo.DataPropertyName = "LineNo";
            colLineNo.FillWeight = 6F;
            colLineNo.HeaderText = "#";
            colLineNo.MinimumWidth = 40;
            colLineNo.Name = "colLineNo";
            colLineNo.ReadOnly = true;
            colLineNo.Width = 40;
            // 
            // colProduct
            // 
            colProduct.DataPropertyName = "DisplayName";
            colProduct.FillWeight = 36F;
            colProduct.HeaderText = "الصنف";
            colProduct.MinimumWidth = 140;
            colProduct.Name = "colProduct";
            colProduct.ReadOnly = true;
            colProduct.Width = 240;
            // 
            // colBarcode
            // 
            colBarcode.DataPropertyName = "Barcode";
            colBarcode.FillWeight = 16F;
            colBarcode.HeaderText = "الباركود";
            colBarcode.MinimumWidth = 90;
            colBarcode.Name = "colBarcode";
            colBarcode.ReadOnly = true;
            colBarcode.Width = 120;
            // 
            // colQuantity
            // 
            colQuantity.DataPropertyName = "Quantity";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N3";
            colQuantity.DefaultCellStyle = dataGridViewCellStyle1;
            colQuantity.FillWeight = 12F;
            colQuantity.HeaderText = "الكمية";
            colQuantity.MinimumWidth = 70;
            colQuantity.Name = "colQuantity";
            colQuantity.ReadOnly = false;
            colQuantity.Width = 90;
            // 
            // colUnitPrice
            // 
            colUnitPrice.DataPropertyName = "UnitPrice";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            colUnitPrice.DefaultCellStyle = dataGridViewCellStyle2;
            colUnitPrice.FillWeight = 14F;
            colUnitPrice.HeaderText = "سعر الوحدة";
            colUnitPrice.MinimumWidth = 90;
            colUnitPrice.Name = "colUnitPrice";
            colUnitPrice.ReadOnly = false;
            colUnitPrice.Width = 110;
            // 
            // colDiscount
            // 
            colDiscount.DataPropertyName = "Discount";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            colDiscount.DefaultCellStyle = dataGridViewCellStyle3;
            colDiscount.FillWeight = 12F;
            colDiscount.HeaderText = "الخصم";
            colDiscount.MinimumWidth = 70;
            colDiscount.Name = "colDiscount";
            colDiscount.ReadOnly = false;
            colDiscount.Width = 90;
            // 
            // colTotal
            // 
            colTotal.DataPropertyName = "LineTotal";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            colTotal.DefaultCellStyle = dataGridViewCellStyle4;
            colTotal.FillWeight = 16F;
            colTotal.HeaderText = "الإجمالي";
            colTotal.MinimumWidth = 90;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            colTotal.Width = 120;
            // 
            // pnlLine — محرر السطر المحدد (داخل كتلة السلة)
            // 
            pnlLine.BackColor = Color.FromArgb(235, 240, 235);
            pnlLine.Controls.Add(flpLine);
            pnlLine.Dock = DockStyle.Bottom;
            pnlLine.Location = new Point(6, 428);
            pnlLine.Name = "pnlLine";
            pnlLine.Padding = new Padding(6, 4, 6, 4);
            pnlLine.Size = new Size(1268, 62);
            pnlLine.TabIndex = 2;
            // 
            // flpLine
            // 
            flpLine.BackColor = Color.FromArgb(235, 240, 235);
            flpLine.Controls.Add(lblSelectedProduct);
            flpLine.Controls.Add(flpQty);
            flpLine.Controls.Add(flpPrice);
            flpLine.Controls.Add(flpLineDiscount);
            flpLine.Controls.Add(btnApplyLine);
            flpLine.Controls.Add(btnRemoveLine);
            flpLine.Dock = DockStyle.Fill;
            flpLine.FlowDirection = FlowDirection.RightToLeft;
            flpLine.Location = new Point(6, 4);
            flpLine.Name = "flpLine";
            flpLine.Size = new Size(1256, 54);
            flpLine.TabIndex = 0;
            flpLine.WrapContents = false;
            // 
            // lblSelectedProduct
            // 
            lblSelectedProduct.AutoEllipsis = true;
            lblSelectedProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSelectedProduct.ForeColor = Color.FromArgb(14, 82, 52);
            lblSelectedProduct.Location = new Point(1006, 4);
            lblSelectedProduct.Margin = new Padding(4);
            lblSelectedProduct.Name = "lblSelectedProduct";
            lblSelectedProduct.Size = new Size(246, 38);
            lblSelectedProduct.TabIndex = 0;
            lblSelectedProduct.Text = "لم يُحدد سطر";
            lblSelectedProduct.TextAlign = ContentAlignment.MiddleRight;
            // 
            // flpQty
            // 
            flpQty.AutoSize = true;
            flpQty.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpQty.BackColor = Color.FromArgb(235, 240, 235);
            flpQty.Controls.Add(lblQtyCaption);
            flpQty.Controls.Add(numQty);
            flpQty.FlowDirection = FlowDirection.RightToLeft;
            flpQty.Location = new Point(860, 4);
            flpQty.Margin = new Padding(4);
            flpQty.Name = "flpQty";
            flpQty.Size = new Size(138, 36);
            flpQty.TabIndex = 1;
            flpQty.WrapContents = false;
            // 
            // lblQtyCaption
            // 
            lblQtyCaption.AutoSize = true;
            lblQtyCaption.Font = new Font("Tahoma", 10F);
            lblQtyCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblQtyCaption.Location = new Point(92, 6);
            lblQtyCaption.Margin = new Padding(0, 6, 4, 0);
            lblQtyCaption.Name = "lblQtyCaption";
            lblQtyCaption.Size = new Size(42, 24);
            lblQtyCaption.TabIndex = 0;
            lblQtyCaption.Text = "الكمية:";
            lblQtyCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numQty
            // 
            numQty.BackColor = Color.White;
            numQty.DecimalPlaces = 3;
            numQty.Font = new Font("Tahoma", 10F);
            numQty.ForeColor = Color.FromArgb(33, 37, 41);
            numQty.Location = new Point(4, 3);
            numQty.Margin = new Padding(4, 3, 4, 3);
            numQty.Maximum = 1000000M;
            numQty.Name = "numQty";
            numQty.Size = new Size(84, 30);
            numQty.TabIndex = 1;
            numQty.TextAlign = HorizontalAlignment.Center;
            numQty.Value = 1M;
            // 
            // flpPrice
            // 
            flpPrice.AutoSize = true;
            flpPrice.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPrice.BackColor = Color.FromArgb(235, 240, 235);
            flpPrice.Controls.Add(lblPriceCaption);
            flpPrice.Controls.Add(numUnitPrice);
            flpPrice.FlowDirection = FlowDirection.RightToLeft;
            flpPrice.Location = new Point(708, 4);
            flpPrice.Margin = new Padding(4);
            flpPrice.Name = "flpPrice";
            flpPrice.Size = new Size(144, 36);
            flpPrice.TabIndex = 2;
            flpPrice.WrapContents = false;
            // 
            // lblPriceCaption
            // 
            lblPriceCaption.AutoSize = true;
            lblPriceCaption.Font = new Font("Tahoma", 10F);
            lblPriceCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblPriceCaption.Location = new Point(96, 6);
            lblPriceCaption.Margin = new Padding(0, 6, 4, 0);
            lblPriceCaption.Name = "lblPriceCaption";
            lblPriceCaption.Size = new Size(44, 24);
            lblPriceCaption.TabIndex = 0;
            lblPriceCaption.Text = "السعر:";
            lblPriceCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numUnitPrice
            // 
            numUnitPrice.BackColor = Color.White;
            numUnitPrice.DecimalPlaces = 2;
            numUnitPrice.Font = new Font("Tahoma", 10F);
            numUnitPrice.ForeColor = Color.FromArgb(33, 37, 41);
            numUnitPrice.Location = new Point(4, 3);
            numUnitPrice.Margin = new Padding(4, 3, 4, 3);
            numUnitPrice.Maximum = 100000000M;
            numUnitPrice.Name = "numUnitPrice";
            numUnitPrice.Size = new Size(88, 30);
            numUnitPrice.TabIndex = 1;
            numUnitPrice.TextAlign = HorizontalAlignment.Center;
            // 
            // flpLineDiscount
            // 
            flpLineDiscount.AutoSize = true;
            flpLineDiscount.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpLineDiscount.BackColor = Color.FromArgb(235, 240, 235);
            flpLineDiscount.Controls.Add(lblLineDiscountCaption);
            flpLineDiscount.Controls.Add(numLineDiscount);
            flpLineDiscount.FlowDirection = FlowDirection.RightToLeft;
            flpLineDiscount.Location = new Point(556, 4);
            flpLineDiscount.Margin = new Padding(4);
            flpLineDiscount.Name = "flpLineDiscount";
            flpLineDiscount.Size = new Size(144, 36);
            flpLineDiscount.TabIndex = 3;
            flpLineDiscount.WrapContents = false;
            // 
            // lblLineDiscountCaption
            // 
            lblLineDiscountCaption.AutoSize = true;
            lblLineDiscountCaption.Font = new Font("Tahoma", 10F);
            lblLineDiscountCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblLineDiscountCaption.Location = new Point(96, 6);
            lblLineDiscountCaption.Margin = new Padding(0, 6, 4, 0);
            lblLineDiscountCaption.Name = "lblLineDiscountCaption";
            lblLineDiscountCaption.Size = new Size(44, 24);
            lblLineDiscountCaption.TabIndex = 0;
            lblLineDiscountCaption.Text = "الخصم:";
            lblLineDiscountCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numLineDiscount
            // 
            numLineDiscount.BackColor = Color.White;
            numLineDiscount.DecimalPlaces = 2;
            numLineDiscount.Font = new Font("Tahoma", 10F);
            numLineDiscount.ForeColor = Color.FromArgb(33, 37, 41);
            numLineDiscount.Location = new Point(4, 3);
            numLineDiscount.Margin = new Padding(4, 3, 4, 3);
            numLineDiscount.Maximum = 100000000M;
            numLineDiscount.Name = "numLineDiscount";
            numLineDiscount.Size = new Size(88, 30);
            numLineDiscount.TabIndex = 1;
            numLineDiscount.TextAlign = HorizontalAlignment.Center;
            // 
            // btnApplyLine
            // 
            btnApplyLine.BackColor = Color.FromArgb(13, 110, 253);
            btnApplyLine.Cursor = Cursors.Hand;
            btnApplyLine.FlatAppearance.BorderSize = 0;
            btnApplyLine.FlatStyle = FlatStyle.Flat;
            btnApplyLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnApplyLine.ForeColor = Color.White;
            btnApplyLine.Location = new Point(448, 4);
            btnApplyLine.Margin = new Padding(4);
            btnApplyLine.Name = "btnApplyLine";
            btnApplyLine.Size = new Size(100, 40);
            btnApplyLine.TabIndex = 4;
            btnApplyLine.Text = "تطبيق";
            btnApplyLine.UseVisualStyleBackColor = false;
            // 
            // btnRemoveLine
            // 
            btnRemoveLine.BackColor = Color.FromArgb(220, 53, 69);
            btnRemoveLine.Cursor = Cursors.Hand;
            btnRemoveLine.FlatAppearance.BorderSize = 0;
            btnRemoveLine.FlatStyle = FlatStyle.Flat;
            btnRemoveLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRemoveLine.ForeColor = Color.White;
            btnRemoveLine.Location = new Point(312, 4);
            btnRemoveLine.Margin = new Padding(4);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new Size(128, 40);
            btnRemoveLine.TabIndex = 5;
            btnRemoveLine.Text = "حذف السطر [Del]";
            btnRemoveLine.UseVisualStyleBackColor = false;
            // 
            // pnlProducts — الاختيار السريع للأصناف (RULE-N81)
            // 
            pnlProducts.AutoScroll = true;
            pnlProducts.BackColor = Color.FromArgb(235, 240, 235);
            pnlProducts.Controls.Add(flpProducts);
            pnlProducts.Dock = DockStyle.Right;
            pnlProducts.Location = new Point(894, 22);
            pnlProducts.Name = "pnlProducts";
            pnlProducts.Padding = new Padding(4);
            pnlProducts.Size = new Size(380, 406);
            pnlProducts.TabIndex = 1;
            // 
            // flpProducts
            // 
            flpProducts.AutoSize = true;
            flpProducts.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpProducts.BackColor = Color.FromArgb(235, 240, 235);
            flpProducts.Dock = DockStyle.Top;
            flpProducts.FlowDirection = FlowDirection.TopDown;
            flpProducts.Location = new Point(4, 4);
            flpProducts.Name = "flpProducts";
            flpProducts.Padding = new Padding(2);
            flpProducts.Size = new Size(372, 0);
            flpProducts.TabIndex = 0;
            flpProducts.WrapContents = false;
            // 
            // gbTotals — الإجماليات (RULE-N135 البند 5)
            // 
            gbTotals.BackColor = Color.FromArgb(235, 245, 235);
            gbTotals.Controls.Add(pnlTotals);
            gbTotals.Dock = DockStyle.Bottom;
            gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbTotals.ForeColor = Color.FromArgb(14, 82, 52);
            gbTotals.Location = new Point(0, 756);
            gbTotals.Name = "gbTotals";
            gbTotals.Padding = new Padding(6, 22, 6, 6);
            gbTotals.Size = new Size(1280, 76);
            gbTotals.TabIndex = 4;
            gbTotals.TabStop = false;
            gbTotals.Text = "الإجماليات";
            // 
            // pnlTotals
            // 
            pnlTotals.BackColor = Color.FromArgb(235, 240, 235);
            pnlTotals.Controls.Add(flpTotals);
            pnlTotals.Dock = DockStyle.Fill;
            pnlTotals.Location = new Point(6, 22);
            pnlTotals.Name = "pnlTotals";
            pnlTotals.Padding = new Padding(6, 2, 6, 2);
            pnlTotals.Size = new Size(1268, 48);
            pnlTotals.TabIndex = 0;
            // 
            // flpTotals
            // 
            flpTotals.BackColor = Color.FromArgb(235, 240, 235);
            flpTotals.Controls.Add(lblSubTotal);
            flpTotals.Controls.Add(lblDiscountTotal);
            flpTotals.Controls.Add(lblNetTotal);
            flpTotals.Controls.Add(flpInvoiceDiscount);
            flpTotals.Controls.Add(lblTafqit);
            flpTotals.Dock = DockStyle.Fill;
            flpTotals.FlowDirection = FlowDirection.RightToLeft;
            flpTotals.Location = new Point(6, 2);
            flpTotals.Name = "flpTotals";
            flpTotals.Size = new Size(1256, 44);
            flpTotals.TabIndex = 0;
            flpTotals.WrapContents = false;
            // 
            // lblSubTotal
            // 
            lblSubTotal.AutoSize = true;
            lblSubTotal.Font = new Font("Tahoma", 10F);
            lblSubTotal.ForeColor = Color.FromArgb(14, 82, 52);
            lblSubTotal.Location = new Point(1108, 6);
            lblSubTotal.Margin = new Padding(4, 6, 12, 0);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new Size(144, 24);
            lblSubTotal.TabIndex = 0;
            lblSubTotal.Text = "المجموع الفرعي: 0.00";
            lblSubTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDiscountTotal
            // 
            lblDiscountTotal.AutoSize = true;
            lblDiscountTotal.Font = new Font("Tahoma", 10F);
            lblDiscountTotal.ForeColor = Color.FromArgb(14, 82, 52);
            lblDiscountTotal.Location = new Point(944, 6);
            lblDiscountTotal.Margin = new Padding(4, 6, 12, 0);
            lblDiscountTotal.Name = "lblDiscountTotal";
            lblDiscountTotal.Size = new Size(148, 24);
            lblDiscountTotal.TabIndex = 1;
            lblDiscountTotal.Text = "إجمالي الخصم: 0.00";
            lblDiscountTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetTotal
            // 
            lblNetTotal.AutoSize = true;
            lblNetTotal.Font = new Font("Tahoma", 13F, FontStyle.Bold);
            lblNetTotal.ForeColor = Color.FromArgb(14, 82, 52);
            lblNetTotal.Location = new Point(770, 4);
            lblNetTotal.Margin = new Padding(4, 4, 12, 0);
            lblNetTotal.Name = "lblNetTotal";
            lblNetTotal.Size = new Size(158, 30);
            lblNetTotal.TabIndex = 2;
            lblNetTotal.Text = "الصافي: 0.00";
            lblNetTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // flpInvoiceDiscount
            // 
            flpInvoiceDiscount.AutoSize = true;
            flpInvoiceDiscount.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpInvoiceDiscount.BackColor = Color.FromArgb(235, 240, 235);
            flpInvoiceDiscount.Controls.Add(lblInvoiceDiscountCaption);
            flpInvoiceDiscount.Controls.Add(numInvoiceDiscount);
            flpInvoiceDiscount.FlowDirection = FlowDirection.RightToLeft;
            flpInvoiceDiscount.Location = new Point(604, 4);
            flpInvoiceDiscount.Margin = new Padding(4);
            flpInvoiceDiscount.Name = "flpInvoiceDiscount";
            flpInvoiceDiscount.Size = new Size(158, 36);
            flpInvoiceDiscount.TabIndex = 3;
            flpInvoiceDiscount.WrapContents = false;
            // 
            // lblInvoiceDiscountCaption
            // 
            lblInvoiceDiscountCaption.AutoSize = true;
            lblInvoiceDiscountCaption.Font = new Font("Tahoma", 10F);
            lblInvoiceDiscountCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblInvoiceDiscountCaption.Location = new Point(108, 6);
            lblInvoiceDiscountCaption.Margin = new Padding(0, 6, 4, 0);
            lblInvoiceDiscountCaption.Name = "lblInvoiceDiscountCaption";
            lblInvoiceDiscountCaption.Size = new Size(46, 24);
            lblInvoiceDiscountCaption.TabIndex = 0;
            lblInvoiceDiscountCaption.Text = "خصم الفاتورة:";
            lblInvoiceDiscountCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numInvoiceDiscount
            // 
            numInvoiceDiscount.BackColor = Color.White;
            numInvoiceDiscount.DecimalPlaces = 2;
            numInvoiceDiscount.Font = new Font("Tahoma", 10F);
            numInvoiceDiscount.ForeColor = Color.FromArgb(33, 37, 41);
            numInvoiceDiscount.Location = new Point(4, 3);
            numInvoiceDiscount.Margin = new Padding(4, 3, 4, 3);
            numInvoiceDiscount.Maximum = 100000000M;
            numInvoiceDiscount.Name = "numInvoiceDiscount";
            numInvoiceDiscount.Size = new Size(100, 30);
            numInvoiceDiscount.TabIndex = 1;
            numInvoiceDiscount.TextAlign = HorizontalAlignment.Center;
            // 
            // lblTafqit
            // 
            lblTafqit.AutoEllipsis = true;
            lblTafqit.BorderStyle = BorderStyle.FixedSingle;
            lblTafqit.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblTafqit.ForeColor = Color.FromArgb(14, 82, 52);
            lblTafqit.Location = new Point(166, 4);
            lblTafqit.Margin = new Padding(4);
            lblTafqit.Name = "lblTafqit";
            lblTafqit.Padding = new Padding(4, 0, 4, 0);
            lblTafqit.Size = new Size(430, 34);
            lblTafqit.TabIndex = 4;
            lblTafqit.Text = "—";
            lblTafqit.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlActions — شريط سفلي (RULE-N135 البند 6 — مستثنى من الصناديق)
            // 
            pnlActions.BackColor = Color.FromArgb(235, 240, 235);
            pnlActions.Controls.Add(flpActions);
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Location = new Point(0, 832);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(8, 8, 8, 8);
            pnlActions.Size = new Size(1280, 68);
            pnlActions.TabIndex = 5;
            // 
            // flpActions
            // 
            flpActions.BackColor = Color.FromArgb(235, 240, 235);
            flpActions.Controls.Add(btnSave);
            flpActions.Controls.Add(btnNew);
            flpActions.Controls.Add(btnClose);
            flpActions.Dock = DockStyle.Fill;
            flpActions.FlowDirection = FlowDirection.RightToLeft;
            flpActions.Location = new Point(8, 8);
            flpActions.Name = "flpActions";
            flpActions.Size = new Size(1264, 52);
            flpActions.TabIndex = 0;
            flpActions.WrapContents = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(25, 135, 84);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(1056, 0);
            btnSave.Margin = new Padding(6, 0, 6, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(202, 46);
            btnSave.TabIndex = 0;
            btnSave.Text = "حفظ وترحيل [F4]";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.FromArgb(13, 110, 253);
            btnNew.Cursor = Cursors.Hand;
            btnNew.FlatAppearance.BorderSize = 0;
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNew.ForeColor = Color.White;
            btnNew.Location = new Point(896, 0);
            btnNew.Margin = new Padding(6, 0, 6, 0);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(148, 46);
            btnNew.TabIndex = 1;
            btnNew.Text = "جديد [F2]";
            btnNew.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(758, 0);
            btnClose.Margin = new Padding(6, 0, 6, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(126, 46);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق [Esc]";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.RightToLeft = true;
            // 
            // frmPOS
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            ClientSize = new Size(1280, 900);
            Controls.Add(gbCart);
            Controls.Add(gbTotals);
            Controls.Add(pnlActions);
            Controls.Add(gbCustomerPayment);
            Controls.Add(gbSalesCommands);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F);
            KeyPreview = true;
            MinimumSize = new Size(1024, 800);
            Name = "frmPOS";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "نقطة البيع";
            pnlHeader.ResumeLayout(false);
            gbSalesCommands.ResumeLayout(false);
            pnlTop.ResumeLayout(false);
            flpTopBar.ResumeLayout(false);
            flpTopBar.PerformLayout();
            flpBarcode.ResumeLayout(false);
            flpBarcode.PerformLayout();
            flpProductSearch.ResumeLayout(false);
            flpProductSearch.PerformLayout();
            flpCategory.ResumeLayout(false);
            flpCategory.PerformLayout();
            gbCustomerPayment.ResumeLayout(false);
            pnlPayment.ResumeLayout(false);
            flpPayment.ResumeLayout(false);
            flpPayment.PerformLayout();
            flpCustomer.ResumeLayout(false);
            flpCustomer.PerformLayout();
            flpWarehouse.ResumeLayout(false);
            flpWarehouse.PerformLayout();
            flpCashBox.ResumeLayout(false);
            flpCashBox.PerformLayout();
            flpPaid.ResumeLayout(false);
            flpPaid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPaid).EndInit();
            gbCart.ResumeLayout(false);
            pnlCart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            pnlLine.ResumeLayout(false);
            flpLine.ResumeLayout(false);
            flpLine.PerformLayout();
            flpQty.ResumeLayout(false);
            flpQty.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            flpPrice.ResumeLayout(false);
            flpPrice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numUnitPrice).EndInit();
            flpLineDiscount.ResumeLayout(false);
            flpLineDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numLineDiscount).EndInit();
            pnlProducts.ResumeLayout(false);
            pnlProducts.PerformLayout();
            gbTotals.ResumeLayout(false);
            pnlTotals.ResumeLayout(false);
            flpTotals.ResumeLayout(false);
            flpTotals.PerformLayout();
            flpInvoiceDiscount.ResumeLayout(false);
            flpInvoiceDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).EndInit();
            pnlActions.ResumeLayout(false);
            flpActions.ResumeLayout(false);
            flpActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblInvoiceInfo;
        private System.Windows.Forms.GroupBox gbSalesCommands;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.FlowLayoutPanel flpTopBar;
        private System.Windows.Forms.FlowLayoutPanel flpBarcode;
        private System.Windows.Forms.Label lblBarcodeCaption;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.FlowLayoutPanel flpProductSearch;
        private System.Windows.Forms.Label lblProductSearchCaption;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.FlowLayoutPanel flpCategory;
        private System.Windows.Forms.Label lblCategoryCaption;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Button btnRefreshCatalog;
        private System.Windows.Forms.GroupBox gbCustomerPayment;
        private System.Windows.Forms.Panel pnlPayment;
        private System.Windows.Forms.FlowLayoutPanel flpPayment;
        private System.Windows.Forms.FlowLayoutPanel flpCustomer;
        private System.Windows.Forms.Label lblCustomerCaption;
        private System.Windows.Forms.ComboBox cbCustomer;
        private System.Windows.Forms.FlowLayoutPanel flpWarehouse;
        private System.Windows.Forms.Label lblWarehouseCaption;
        private System.Windows.Forms.ComboBox cbWarehouse;
        private System.Windows.Forms.FlowLayoutPanel flpCashBox;
        private System.Windows.Forms.Label lblCashBoxCaption;
        private System.Windows.Forms.ComboBox cbCashBox;
        private System.Windows.Forms.RadioButton rbtnCash;
        private System.Windows.Forms.RadioButton rbtnCredit;
        private System.Windows.Forms.FlowLayoutPanel flpPaid;
        private System.Windows.Forms.Label lblPaidCaption;
        private System.Windows.Forms.NumericUpDown numPaid;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.GroupBox gbCart;
        private System.Windows.Forms.Panel pnlCart;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.Panel pnlLine;
        private System.Windows.Forms.FlowLayoutPanel flpLine;
        private System.Windows.Forms.Label lblSelectedProduct;
        private System.Windows.Forms.FlowLayoutPanel flpQty;
        private System.Windows.Forms.Label lblQtyCaption;
        private System.Windows.Forms.NumericUpDown numQty;
        private System.Windows.Forms.FlowLayoutPanel flpPrice;
        private System.Windows.Forms.Label lblPriceCaption;
        private System.Windows.Forms.NumericUpDown numUnitPrice;
        private System.Windows.Forms.FlowLayoutPanel flpLineDiscount;
        private System.Windows.Forms.Label lblLineDiscountCaption;
        private System.Windows.Forms.NumericUpDown numLineDiscount;
        private System.Windows.Forms.Button btnApplyLine;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Panel pnlProducts;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.GroupBox gbTotals;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.FlowLayoutPanel flpTotals;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblDiscountTotal;
        private System.Windows.Forms.Label lblNetTotal;
        private System.Windows.Forms.FlowLayoutPanel flpInvoiceDiscount;
        private System.Windows.Forms.Label lblInvoiceDiscountCaption;
        private System.Windows.Forms.NumericUpDown numInvoiceDiscount;
        private System.Windows.Forms.Label lblTafqit;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
