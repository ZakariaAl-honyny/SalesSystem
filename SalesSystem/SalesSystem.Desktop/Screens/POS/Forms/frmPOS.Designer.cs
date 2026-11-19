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
            pnlProducts = new Panel();
            flpProducts = new FlowLayoutPanel();
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
            pnlProducts.SuspendLayout();
            pnlLine.SuspendLayout();
            flpLine.SuspendLayout();
            flpQty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            flpPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUnitPrice).BeginInit();
            flpLineDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLineDiscount).BeginInit();
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
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Controls.Add(lblInvoiceInfo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(8, 6, 8, 4);
            pnlHeader.Size = new Size(1322, 71);
            pnlHeader.TabIndex = 0;
            // 
            // lblInvoiceInfo
            // 
            lblInvoiceInfo.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            lblInvoiceInfo.Dock = DockStyle.Bottom;
            lblInvoiceInfo.Font = new Font("Tahoma", 9F);
            lblInvoiceInfo.ForeColor = System.Drawing.Color.White;
            lblInvoiceInfo.Location = new Point(8, 36);
            lblInvoiceInfo.Name = "lblInvoiceInfo";
            lblInvoiceInfo.Size = new Size(1306, 31);
            lblInvoiceInfo.TabIndex = 1;
            lblInvoiceInfo.Text = "التاريخ: —   |   المخزن: —   |   الصندوق: —";
            lblInvoiceInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMode
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Location = new Point(8, 6);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1306, 30);
            lblMode.TabIndex = 0;
            lblMode.Text = "نقطة البيع — فاتورة جديدة";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbSalesCommands
            // 
            gbSalesCommands.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbSalesCommands.Controls.Add(pnlTop);
            gbSalesCommands.Dock = DockStyle.Top;
            gbSalesCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbSalesCommands.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            gbSalesCommands.Location = new Point(0, 71);
            gbSalesCommands.Name = "gbSalesCommands";
            gbSalesCommands.Padding = new Padding(6, 22, 6, 6);
            gbSalesCommands.Size = new Size(1322, 108);
            gbSalesCommands.TabIndex = 1;
            gbSalesCommands.TabStop = false;
            gbSalesCommands.Text = "أوامر البيع";
            // 
            // pnlTop
            // 
            pnlTop.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlTop.Controls.Add(flpTopBar);
            pnlTop.Dock = DockStyle.Fill;
            pnlTop.Location = new Point(6, 47);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(6, 4, 6, 4);
            pnlTop.Size = new Size(1310, 55);
            pnlTop.TabIndex = 0;
            // 
            // flpTopBar
            // 
            flpTopBar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpTopBar.Controls.Add(flpBarcode);
            flpTopBar.Controls.Add(flpProductSearch);
            flpTopBar.Controls.Add(flpCategory);
            flpTopBar.Controls.Add(btnRefreshCatalog);
            flpTopBar.Dock = DockStyle.Fill;
            flpTopBar.Location = new Point(6, 4);
            flpTopBar.Name = "flpTopBar";
            flpTopBar.Size = new Size(1298, 47);
            flpTopBar.TabIndex = 0;
            flpTopBar.WrapContents = false;
            // 
            // flpBarcode
            // 
            flpBarcode.AutoSize = true;
            flpBarcode.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpBarcode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpBarcode.Controls.Add(txtBarcode);
            flpBarcode.Controls.Add(lblBarcodeCaption);
            flpBarcode.FlowDirection = FlowDirection.LeftToRight;
            flpBarcode.Location = new Point(878, 4);
            flpBarcode.Margin = new Padding(4);
            flpBarcode.Name = "flpBarcode";
            flpBarcode.Size = new Size(416, 42);
            flpBarcode.TabIndex = 0;
            flpBarcode.WrapContents = false;
            // 
            // lblBarcodeCaption
            // 
            lblBarcodeCaption.AutoSize = true;
            lblBarcodeCaption.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblBarcodeCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblBarcodeCaption.Location = new Point(318, 10);
            lblBarcodeCaption.Margin = new Padding(0, 10, 4, 0);
            lblBarcodeCaption.Name = "lblBarcodeCaption";
            lblBarcodeCaption.Size = new Size(98, 27);
            lblBarcodeCaption.TabIndex = 0;
            lblBarcodeCaption.Text = "الباركود:";
            lblBarcodeCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtBarcode
            // 
            txtBarcode.BackColor = System.Drawing.Color.White;
            txtBarcode.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtBarcode.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            txtBarcode.Location = new Point(4, 3);
            txtBarcode.Margin = new Padding(4, 3, 4, 3);
            txtBarcode.Name = "txtBarcode";
            txtBarcode.PlaceholderText = "امسح الباركود ثم اضغط Enter";
            txtBarcode.Size = new Size(306, 36);
            txtBarcode.TabIndex = 1;
            txtBarcode.TextAlign = HorizontalAlignment.Center;
            // 
            // flpProductSearch
            // 
            flpProductSearch.AutoSize = true;
            flpProductSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpProductSearch.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpProductSearch.Controls.Add(txtProductSearch);
            flpProductSearch.Controls.Add(lblProductSearchCaption);
            flpProductSearch.FlowDirection = FlowDirection.LeftToRight;
            flpProductSearch.Location = new Point(506, 4);
            flpProductSearch.Margin = new Padding(4);
            flpProductSearch.Name = "flpProductSearch";
            flpProductSearch.Size = new Size(364, 40);
            flpProductSearch.TabIndex = 1;
            flpProductSearch.WrapContents = false;
            // 
            // lblProductSearchCaption
            // 
            lblProductSearchCaption.AutoSize = true;
            lblProductSearchCaption.Font = new Font("Tahoma", 10F);
            lblProductSearchCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblProductSearchCaption.Location = new Point(240, 8);
            lblProductSearchCaption.Margin = new Padding(0, 8, 4, 0);
            lblProductSearchCaption.Name = "lblProductSearchCaption";
            lblProductSearchCaption.Size = new Size(124, 24);
            lblProductSearchCaption.TabIndex = 0;
            lblProductSearchCaption.Text = "بحث الأصناف:";
            lblProductSearchCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtProductSearch
            // 
            txtProductSearch.BackColor = System.Drawing.Color.White;
            txtProductSearch.Font = new Font("Tahoma", 11F);
            txtProductSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            txtProductSearch.Location = new Point(4, 3);
            txtProductSearch.Margin = new Padding(4, 3, 4, 3);
            txtProductSearch.Name = "txtProductSearch";
            txtProductSearch.PlaceholderText = "اسم الصنف أو الباركود";
            txtProductSearch.Size = new Size(228, 34);
            txtProductSearch.TabIndex = 1;
            // 
            // flpCategory
            // 
            flpCategory.AutoSize = true;
            flpCategory.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCategory.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpCategory.Controls.Add(cbCategory);
            flpCategory.Controls.Add(lblCategoryCaption);
            flpCategory.FlowDirection = FlowDirection.LeftToRight;
            flpCategory.Location = new Point(248, 4);
            flpCategory.Margin = new Padding(4);
            flpCategory.Name = "flpCategory";
            flpCategory.Size = new Size(250, 38);
            flpCategory.TabIndex = 2;
            flpCategory.WrapContents = false;
            // 
            // lblCategoryCaption
            // 
            lblCategoryCaption.AutoSize = true;
            lblCategoryCaption.Font = new Font("Tahoma", 10F);
            lblCategoryCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblCategoryCaption.Location = new Point(168, 8);
            lblCategoryCaption.Margin = new Padding(0, 8, 4, 0);
            lblCategoryCaption.Name = "lblCategoryCaption";
            lblCategoryCaption.Size = new Size(82, 24);
            lblCategoryCaption.TabIndex = 0;
            lblCategoryCaption.Text = "التصنيف:";
            lblCategoryCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCategory
            // 
            cbCategory.BackColor = System.Drawing.Color.White;
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategory.FlatStyle = FlatStyle.Flat;
            cbCategory.Font = new Font("Tahoma", 10F);
            cbCategory.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cbCategory.FormattingEnabled = true;
            cbCategory.Location = new Point(4, 3);
            cbCategory.Margin = new Padding(4, 3, 4, 3);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(156, 32);
            cbCategory.TabIndex = 1;
            // 
            // btnRefreshCatalog
            // 
            btnRefreshCatalog.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnRefreshCatalog.Cursor = Cursors.Hand;
            btnRefreshCatalog.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnRefreshCatalog.FlatStyle = FlatStyle.Flat;
            btnRefreshCatalog.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefreshCatalog.ForeColor = System.Drawing.Color.White;
            btnRefreshCatalog.Location = new Point(98, 4);
            btnRefreshCatalog.Margin = new Padding(4);
            btnRefreshCatalog.Name = "btnRefreshCatalog";
            btnRefreshCatalog.Size = new Size(142, 40);
            btnRefreshCatalog.TabIndex = 3;
            btnRefreshCatalog.Text = "تحديث الأصناف [F5]";
            btnRefreshCatalog.UseVisualStyleBackColor = false;
            // 
            // gbCustomerPayment
            // 
            gbCustomerPayment.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbCustomerPayment.Controls.Add(pnlPayment);
            gbCustomerPayment.Dock = DockStyle.Top;
            gbCustomerPayment.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCustomerPayment.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            gbCustomerPayment.Location = new Point(0, 179);
            gbCustomerPayment.Name = "gbCustomerPayment";
            gbCustomerPayment.Padding = new Padding(6, 22, 6, 6);
            gbCustomerPayment.Size = new Size(1322, 96);
            gbCustomerPayment.TabIndex = 2;
            gbCustomerPayment.TabStop = false;
            gbCustomerPayment.Text = "العميل والدفع";
            // 
            // pnlPayment
            // 
            pnlPayment.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlPayment.Controls.Add(flpPayment);
            pnlPayment.Dock = DockStyle.Fill;
            pnlPayment.Location = new Point(6, 47);
            pnlPayment.Name = "pnlPayment";
            pnlPayment.Padding = new Padding(6, 4, 6, 4);
            pnlPayment.Size = new Size(1310, 43);
            pnlPayment.TabIndex = 0;
            // 
            // flpPayment
            // 
            flpPayment.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpPayment.Controls.Add(flpCustomer);
            flpPayment.Controls.Add(flpWarehouse);
            flpPayment.Controls.Add(flpCashBox);
            flpPayment.Controls.Add(rbtnCash);
            flpPayment.Controls.Add(rbtnCredit);
            flpPayment.Controls.Add(flpPaid);
            flpPayment.Controls.Add(lblRemaining);
            flpPayment.Dock = DockStyle.Fill;
            flpPayment.Location = new Point(6, 4);
            flpPayment.Name = "flpPayment";
            flpPayment.Size = new Size(1298, 35);
            flpPayment.TabIndex = 0;
            flpPayment.WrapContents = false;
            // 
            // flpCustomer
            // 
            flpCustomer.AutoSize = true;
            flpCustomer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCustomer.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpCustomer.Controls.Add(cbCustomer);
            flpCustomer.Controls.Add(lblCustomerCaption);
            flpCustomer.FlowDirection = FlowDirection.LeftToRight;
            flpCustomer.Location = new Point(1031, 4);
            flpCustomer.Margin = new Padding(4);
            flpCustomer.Name = "flpCustomer";
            flpCustomer.Size = new Size(263, 38);
            flpCustomer.TabIndex = 0;
            flpCustomer.WrapContents = false;
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.AutoSize = true;
            lblCustomerCaption.Font = new Font("Tahoma", 10F);
            lblCustomerCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblCustomerCaption.Location = new Point(192, 6);
            lblCustomerCaption.Margin = new Padding(0, 6, 4, 0);
            lblCustomerCaption.Name = "lblCustomerCaption";
            lblCustomerCaption.Size = new Size(71, 24);
            lblCustomerCaption.TabIndex = 0;
            lblCustomerCaption.Text = "العميل:";
            lblCustomerCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCustomer
            // 
            cbCustomer.BackColor = System.Drawing.Color.White;
            cbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCustomer.FlatStyle = FlatStyle.Flat;
            cbCustomer.Font = new Font("Tahoma", 10F);
            cbCustomer.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cbCustomer.FormattingEnabled = true;
            cbCustomer.Location = new Point(4, 3);
            cbCustomer.Margin = new Padding(4, 3, 4, 3);
            cbCustomer.Name = "cbCustomer";
            cbCustomer.Size = new Size(180, 32);
            cbCustomer.TabIndex = 1;
            // 
            // flpWarehouse
            // 
            flpWarehouse.AutoSize = true;
            flpWarehouse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpWarehouse.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpWarehouse.Controls.Add(cbWarehouse);
            flpWarehouse.Controls.Add(lblWarehouseCaption);
            flpWarehouse.FlowDirection = FlowDirection.LeftToRight;
            flpWarehouse.Location = new Point(796, 4);
            flpWarehouse.Margin = new Padding(4);
            flpWarehouse.Name = "flpWarehouse";
            flpWarehouse.Size = new Size(227, 38);
            flpWarehouse.TabIndex = 1;
            flpWarehouse.WrapContents = false;
            // 
            // lblWarehouseCaption
            // 
            lblWarehouseCaption.AutoSize = true;
            lblWarehouseCaption.Font = new Font("Tahoma", 10F);
            lblWarehouseCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblWarehouseCaption.Location = new Point(152, 6);
            lblWarehouseCaption.Margin = new Padding(0, 6, 4, 0);
            lblWarehouseCaption.Name = "lblWarehouseCaption";
            lblWarehouseCaption.Size = new Size(75, 24);
            lblWarehouseCaption.TabIndex = 0;
            lblWarehouseCaption.Text = "المخزن:";
            lblWarehouseCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbWarehouse
            // 
            cbWarehouse.BackColor = System.Drawing.Color.White;
            cbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWarehouse.FlatStyle = FlatStyle.Flat;
            cbWarehouse.Font = new Font("Tahoma", 10F);
            cbWarehouse.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cbWarehouse.FormattingEnabled = true;
            cbWarehouse.Location = new Point(4, 3);
            cbWarehouse.Margin = new Padding(4, 3, 4, 3);
            cbWarehouse.Name = "cbWarehouse";
            cbWarehouse.Size = new Size(140, 32);
            cbWarehouse.TabIndex = 1;
            // 
            // flpCashBox
            // 
            flpCashBox.AutoSize = true;
            flpCashBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCashBox.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpCashBox.Controls.Add(cbCashBox);
            flpCashBox.Controls.Add(lblCashBoxCaption);
            flpCashBox.FlowDirection = FlowDirection.LeftToRight;
            flpCashBox.Location = new Point(560, 4);
            flpCashBox.Margin = new Padding(4);
            flpCashBox.Name = "flpCashBox";
            flpCashBox.Size = new Size(228, 38);
            flpCashBox.TabIndex = 2;
            flpCashBox.WrapContents = false;
            // 
            // lblCashBoxCaption
            // 
            lblCashBoxCaption.AutoSize = true;
            lblCashBoxCaption.Font = new Font("Tahoma", 10F);
            lblCashBoxCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblCashBoxCaption.Location = new Point(142, 6);
            lblCashBoxCaption.Margin = new Padding(0, 6, 4, 0);
            lblCashBoxCaption.Name = "lblCashBoxCaption";
            lblCashBoxCaption.Size = new Size(86, 24);
            lblCashBoxCaption.TabIndex = 0;
            lblCashBoxCaption.Text = "الصندوق:";
            lblCashBoxCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCashBox
            // 
            cbCashBox.BackColor = System.Drawing.Color.White;
            cbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCashBox.FlatStyle = FlatStyle.Flat;
            cbCashBox.Font = new Font("Tahoma", 10F);
            cbCashBox.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            cbCashBox.FormattingEnabled = true;
            cbCashBox.Location = new Point(4, 3);
            cbCashBox.Margin = new Padding(4, 3, 4, 3);
            cbCashBox.Name = "cbCashBox";
            cbCashBox.Size = new Size(130, 32);
            cbCashBox.TabIndex = 1;
            // 
            // rbtnCash
            // 
            rbtnCash.AutoSize = true;
            rbtnCash.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            rbtnCash.Checked = true;
            rbtnCash.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbtnCash.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            rbtnCash.Location = new Point(468, 4);
            rbtnCash.Margin = new Padding(4);
            rbtnCash.Name = "rbtnCash";
            rbtnCash.Size = new Size(84, 28);
            rbtnCash.TabIndex = 3;
            rbtnCash.TabStop = true;
            rbtnCash.Text = "نقدي";
            rbtnCash.UseVisualStyleBackColor = false;
            // 
            // rbtnCredit
            // 
            rbtnCredit.AutoSize = true;
            rbtnCredit.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            rbtnCredit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbtnCredit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            rbtnCredit.Location = new Point(388, 4);
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
            flpPaid.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpPaid.Controls.Add(numPaid);
            flpPaid.Controls.Add(lblPaidCaption);
            flpPaid.FlowDirection = FlowDirection.LeftToRight;
            flpPaid.Location = new Point(208, 4);
            flpPaid.Margin = new Padding(4);
            flpPaid.Name = "flpPaid";
            flpPaid.Size = new Size(172, 38);
            flpPaid.TabIndex = 5;
            flpPaid.WrapContents = false;
            // 
            // lblPaidCaption
            // 
            lblPaidCaption.AutoSize = true;
            lblPaidCaption.Font = new Font("Tahoma", 10F);
            lblPaidCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblPaidCaption.Location = new Point(92, 6);
            lblPaidCaption.Margin = new Padding(0, 6, 4, 0);
            lblPaidCaption.Name = "lblPaidCaption";
            lblPaidCaption.Size = new Size(80, 24);
            lblPaidCaption.TabIndex = 0;
            lblPaidCaption.Text = "المدفوع:";
            lblPaidCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numPaid
            // 
            numPaid.BackColor = System.Drawing.Color.White;
            numPaid.DecimalPlaces = 2;
            numPaid.Font = new Font("Tahoma", 10F);
            numPaid.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            numPaid.Location = new Point(4, 3);
            numPaid.Margin = new Padding(4, 3, 4, 3);
            numPaid.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numPaid.Name = "numPaid";
            numPaid.Size = new Size(80, 32);
            numPaid.TabIndex = 1;
            numPaid.TextAlign = HorizontalAlignment.Center;
            // 
            // lblRemaining
            // 
            lblRemaining.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            lblRemaining.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblRemaining.Location = new Point(54, 4);
            lblRemaining.Margin = new Padding(4);
            lblRemaining.Name = "lblRemaining";
            lblRemaining.Size = new Size(146, 34);
            lblRemaining.TabIndex = 6;
            lblRemaining.Text = "الباقي: 0.00";
            lblRemaining.TextAlign = ContentAlignment.MiddleRight;
            // 
            // gbCart
            // 
            gbCart.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbCart.Controls.Add(pnlCart);
            gbCart.Controls.Add(pnlProducts);
            gbCart.Controls.Add(pnlLine);
            gbCart.Dock = DockStyle.Fill;
            gbCart.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCart.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            gbCart.Location = new Point(0, 275);
            gbCart.Name = "gbCart";
            gbCart.Padding = new Padding(6, 22, 6, 6);
            gbCart.Size = new Size(1322, 481);
            gbCart.TabIndex = 3;
            gbCart.TabStop = false;
            gbCart.Text = "سلة الأصناف";
            // 
            // pnlCart
            // 
            pnlCart.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlCart.Controls.Add(dgvCart);
            pnlCart.Dock = DockStyle.Fill;
            pnlCart.Location = new Point(6, 47);
            pnlCart.Name = "pnlCart";
            pnlCart.Size = new Size(930, 366);
            pnlCart.TabIndex = 0;
            // 
            // dgvCart
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvCart.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCart.ColumnHeadersHeight = 45;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCart.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvCart.Location = new Point(0, 0);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowHeadersWidth = 62;
            dgvCart.RowTemplate.Height = 32;
            dgvCart.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCart.Size = new Size(930, 366);
            dgvCart.TabIndex = 0;
            // 
            // pnlProducts
            // 
            pnlProducts.AutoScroll = true;
            pnlProducts.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlProducts.Controls.Add(flpProducts);
            pnlProducts.Dock = DockStyle.Right;
            pnlProducts.Location = new Point(936, 47);
            pnlProducts.Name = "pnlProducts";
            pnlProducts.Padding = new Padding(4);
            pnlProducts.Size = new Size(380, 366);
            pnlProducts.TabIndex = 1;
            // 
            // flpProducts
            // 
            flpProducts.AutoSize = true;
            flpProducts.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpProducts.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpProducts.Dock = DockStyle.Top;
            flpProducts.FlowDirection = FlowDirection.TopDown;
            flpProducts.Location = new Point(4, 4);
            flpProducts.Name = "flpProducts";
            flpProducts.Padding = new Padding(2);
            flpProducts.Size = new Size(372, 4);
            flpProducts.TabIndex = 0;
            flpProducts.WrapContents = false;
            // 
            // pnlLine
            // 
            pnlLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlLine.Controls.Add(flpLine);
            pnlLine.Dock = DockStyle.Bottom;
            pnlLine.Location = new Point(6, 413);
            pnlLine.Name = "pnlLine";
            pnlLine.Padding = new Padding(6, 4, 6, 4);
            pnlLine.Size = new Size(1310, 62);
            pnlLine.TabIndex = 2;
            // 
            // flpLine
            // 
            flpLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpLine.Controls.Add(flpQty);
            flpLine.Controls.Add(flpPrice);
            flpLine.Controls.Add(flpLineDiscount);
            flpLine.Controls.Add(btnApplyLine);
            flpLine.Controls.Add(btnRemoveLine);
            flpLine.Dock = DockStyle.Fill;
            flpLine.Location = new Point(6, 4);
            flpLine.Name = "flpLine";
            flpLine.Size = new Size(1298, 54);
            flpLine.TabIndex = 0;
            flpLine.WrapContents = false;
            // 
            // lblSelectedProduct
            // 
            lblSelectedProduct.AutoEllipsis = true;
            lblSelectedProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSelectedProduct.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblSelectedProduct.Location = new Point(4, 4);
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
            flpQty.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpQty.Controls.Add(lblSelectedProduct);
            flpQty.Controls.Add(numQty);
            flpQty.Controls.Add(lblQtyCaption);
            flpQty.FlowDirection = FlowDirection.LeftToRight;
            flpQty.Location = new Point(875, 4);
            flpQty.Margin = new Padding(4);
            flpQty.Name = "flpQty";
            flpQty.Size = new Size(419, 46);
            flpQty.TabIndex = 1;
            flpQty.WrapContents = false;
            // 
            // lblQtyCaption
            // 
            lblQtyCaption.AutoSize = true;
            lblQtyCaption.Font = new Font("Tahoma", 10F);
            lblQtyCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblQtyCaption.Location = new Point(350, 6);
            lblQtyCaption.Margin = new Padding(0, 6, 4, 0);
            lblQtyCaption.Name = "lblQtyCaption";
            lblQtyCaption.Size = new Size(69, 24);
            lblQtyCaption.TabIndex = 0;
            lblQtyCaption.Text = "الكمية:";
            lblQtyCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numQty
            // 
            numQty.BackColor = System.Drawing.Color.White;
            numQty.DecimalPlaces = 3;
            numQty.Font = new Font("Tahoma", 10F);
            numQty.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            numQty.Location = new Point(258, 3);
            numQty.Margin = new Padding(4, 3, 4, 3);
            numQty.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(84, 32);
            numQty.TabIndex = 1;
            numQty.TextAlign = HorizontalAlignment.Center;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // flpPrice
            // 
            flpPrice.AutoSize = true;
            flpPrice.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPrice.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpPrice.Controls.Add(numUnitPrice);
            flpPrice.Controls.Add(lblPriceCaption);
            flpPrice.FlowDirection = FlowDirection.LeftToRight;
            flpPrice.Location = new Point(700, 4);
            flpPrice.Margin = new Padding(4);
            flpPrice.Name = "flpPrice";
            flpPrice.Size = new Size(167, 38);
            flpPrice.TabIndex = 2;
            flpPrice.WrapContents = false;
            // 
            // lblPriceCaption
            // 
            lblPriceCaption.AutoSize = true;
            lblPriceCaption.Font = new Font("Tahoma", 10F);
            lblPriceCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblPriceCaption.Location = new Point(100, 6);
            lblPriceCaption.Margin = new Padding(0, 6, 4, 0);
            lblPriceCaption.Name = "lblPriceCaption";
            lblPriceCaption.Size = new Size(67, 24);
            lblPriceCaption.TabIndex = 0;
            lblPriceCaption.Text = "السعر:";
            lblPriceCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numUnitPrice
            // 
            numUnitPrice.BackColor = System.Drawing.Color.White;
            numUnitPrice.DecimalPlaces = 2;
            numUnitPrice.Font = new Font("Tahoma", 10F);
            numUnitPrice.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            numUnitPrice.Location = new Point(4, 3);
            numUnitPrice.Margin = new Padding(4, 3, 4, 3);
            numUnitPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numUnitPrice.Name = "numUnitPrice";
            numUnitPrice.Size = new Size(88, 32);
            numUnitPrice.TabIndex = 1;
            numUnitPrice.TextAlign = HorizontalAlignment.Center;
            // 
            // flpLineDiscount
            // 
            flpLineDiscount.AutoSize = true;
            flpLineDiscount.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpLineDiscount.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpLineDiscount.Controls.Add(numLineDiscount);
            flpLineDiscount.Controls.Add(lblLineDiscountCaption);
            flpLineDiscount.FlowDirection = FlowDirection.LeftToRight;
            flpLineDiscount.Location = new Point(521, 4);
            flpLineDiscount.Margin = new Padding(4);
            flpLineDiscount.Name = "flpLineDiscount";
            flpLineDiscount.Size = new Size(171, 38);
            flpLineDiscount.TabIndex = 3;
            flpLineDiscount.WrapContents = false;
            // 
            // lblLineDiscountCaption
            // 
            lblLineDiscountCaption.AutoSize = true;
            lblLineDiscountCaption.Font = new Font("Tahoma", 10F);
            lblLineDiscountCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblLineDiscountCaption.Location = new Point(100, 6);
            lblLineDiscountCaption.Margin = new Padding(0, 6, 4, 0);
            lblLineDiscountCaption.Name = "lblLineDiscountCaption";
            lblLineDiscountCaption.Size = new Size(71, 24);
            lblLineDiscountCaption.TabIndex = 0;
            lblLineDiscountCaption.Text = "الخصم:";
            lblLineDiscountCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numLineDiscount
            // 
            numLineDiscount.BackColor = System.Drawing.Color.White;
            numLineDiscount.DecimalPlaces = 2;
            numLineDiscount.Font = new Font("Tahoma", 10F);
            numLineDiscount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            numLineDiscount.Location = new Point(4, 3);
            numLineDiscount.Margin = new Padding(4, 3, 4, 3);
            numLineDiscount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numLineDiscount.Name = "numLineDiscount";
            numLineDiscount.Size = new Size(88, 32);
            numLineDiscount.TabIndex = 1;
            numLineDiscount.TextAlign = HorizontalAlignment.Center;
            // 
            // btnApplyLine
            // 
            btnApplyLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
            btnApplyLine.Cursor = Cursors.Hand;
            btnApplyLine.FlatAppearance.BorderSize = 0;
            btnApplyLine.FlatStyle = FlatStyle.Flat;
            btnApplyLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnApplyLine.ForeColor = System.Drawing.Color.White;
            btnApplyLine.Location = new Point(413, 4);
            btnApplyLine.Margin = new Padding(4);
            btnApplyLine.Name = "btnApplyLine";
            btnApplyLine.Size = new Size(100, 40);
            btnApplyLine.TabIndex = 4;
            btnApplyLine.Text = "تطبيق";
            btnApplyLine.UseVisualStyleBackColor = false;
            // 
            // btnRemoveLine
            // 
            btnRemoveLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
            btnRemoveLine.Cursor = Cursors.Hand;
            btnRemoveLine.FlatAppearance.BorderSize = 0;
            btnRemoveLine.FlatStyle = FlatStyle.Flat;
            btnRemoveLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRemoveLine.ForeColor = System.Drawing.Color.White;
            btnRemoveLine.Location = new Point(248, 4);
            btnRemoveLine.Margin = new Padding(4);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new Size(157, 40);
            btnRemoveLine.TabIndex = 5;
            btnRemoveLine.Text = "حذف السطر [Del]";
            btnRemoveLine.UseVisualStyleBackColor = false;
            // 
            // gbTotals
            // 
            gbTotals.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
            gbTotals.Controls.Add(pnlTotals);
            gbTotals.Dock = DockStyle.Bottom;
            gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbTotals.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            gbTotals.Location = new Point(0, 756);
            gbTotals.Name = "gbTotals";
            gbTotals.Padding = new Padding(6, 22, 6, 6);
            gbTotals.Size = new Size(1322, 76);
            gbTotals.TabIndex = 4;
            gbTotals.TabStop = false;
            gbTotals.Text = "الإجماليات";
            // 
            // pnlTotals
            // 
            pnlTotals.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlTotals.Controls.Add(flpTotals);
            pnlTotals.Location = new Point(6, 22);
            pnlTotals.Name = "pnlTotals";
            pnlTotals.Padding = new Padding(6, 2, 6, 2);
            pnlTotals.Size = new Size(1268, 48);
            pnlTotals.TabIndex = 0;
            // 
            // flpTotals
            // 
            flpTotals.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpTotals.Controls.Add(lblSubTotal);
            flpTotals.Controls.Add(lblDiscountTotal);
            flpTotals.Controls.Add(lblNetTotal);
            flpTotals.Controls.Add(flpInvoiceDiscount);
            flpTotals.Controls.Add(lblTafqit);
            flpTotals.Dock = DockStyle.Fill;
            flpTotals.FlowDirection = FlowDirection.LeftToRight;
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
            lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblSubTotal.Location = new Point(12, 6);
            lblSubTotal.Margin = new Padding(4, 6, 12, 0);
            lblSubTotal.Name = "lblSubTotal";
            lblSubTotal.Size = new Size(199, 24);
            lblSubTotal.TabIndex = 0;
            lblSubTotal.Text = "المجموع الفرعي: 0.00";
            lblSubTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDiscountTotal
            // 
            lblDiscountTotal.AutoSize = true;
            lblDiscountTotal.Font = new Font("Tahoma", 10F);
            lblDiscountTotal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblDiscountTotal.Location = new Point(227, 6);
            lblDiscountTotal.Margin = new Padding(4, 6, 12, 0);
            lblDiscountTotal.Name = "lblDiscountTotal";
            lblDiscountTotal.Size = new Size(184, 24);
            lblDiscountTotal.TabIndex = 1;
            lblDiscountTotal.Text = "إجمالي الخصم: 0.00";
            lblDiscountTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblNetTotal
            // 
            lblNetTotal.AutoSize = true;
            lblNetTotal.Font = new Font("Tahoma", 13F, FontStyle.Bold);
            lblNetTotal.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblNetTotal.Location = new Point(427, 4);
            lblNetTotal.Margin = new Padding(4, 4, 12, 0);
            lblNetTotal.Name = "lblNetTotal";
            lblNetTotal.Size = new Size(186, 31);
            lblNetTotal.TabIndex = 2;
            lblNetTotal.Text = "الصافي: 0.00";
            lblNetTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // flpInvoiceDiscount
            // 
            flpInvoiceDiscount.AutoSize = true;
            flpInvoiceDiscount.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpInvoiceDiscount.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpInvoiceDiscount.Controls.Add(numInvoiceDiscount);
            flpInvoiceDiscount.Controls.Add(lblInvoiceDiscountCaption);
            flpInvoiceDiscount.FlowDirection = FlowDirection.LeftToRight;
            flpInvoiceDiscount.Location = new Point(621, 4);
            flpInvoiceDiscount.Margin = new Padding(4);
            flpInvoiceDiscount.Name = "flpInvoiceDiscount";
            flpInvoiceDiscount.Size = new Size(235, 38);
            flpInvoiceDiscount.TabIndex = 3;
            flpInvoiceDiscount.WrapContents = false;
            // 
            // lblInvoiceDiscountCaption
            // 
            lblInvoiceDiscountCaption.AutoSize = true;
            lblInvoiceDiscountCaption.Font = new Font("Tahoma", 10F);
            lblInvoiceDiscountCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblInvoiceDiscountCaption.Location = new Point(112, 6);
            lblInvoiceDiscountCaption.Margin = new Padding(0, 6, 4, 0);
            lblInvoiceDiscountCaption.Name = "lblInvoiceDiscountCaption";
            lblInvoiceDiscountCaption.Size = new Size(123, 24);
            lblInvoiceDiscountCaption.TabIndex = 0;
            lblInvoiceDiscountCaption.Text = "خصم الفاتورة:";
            lblInvoiceDiscountCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // numInvoiceDiscount
            // 
            numInvoiceDiscount.BackColor = System.Drawing.Color.White;
            numInvoiceDiscount.DecimalPlaces = 2;
            numInvoiceDiscount.Font = new Font("Tahoma", 10F);
            numInvoiceDiscount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            numInvoiceDiscount.Location = new Point(4, 3);
            numInvoiceDiscount.Margin = new Padding(4, 3, 4, 3);
            numInvoiceDiscount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numInvoiceDiscount.Name = "numInvoiceDiscount";
            numInvoiceDiscount.Size = new Size(100, 32);
            numInvoiceDiscount.TabIndex = 1;
            numInvoiceDiscount.TextAlign = HorizontalAlignment.Center;
            // 
            // lblTafqit
            // 
            lblTafqit.AutoEllipsis = true;
            lblTafqit.BorderStyle = BorderStyle.FixedSingle;
            lblTafqit.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblTafqit.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblTafqit.Location = new Point(864, 4);
            lblTafqit.Margin = new Padding(4);
            lblTafqit.Name = "lblTafqit";
            lblTafqit.Padding = new Padding(4, 0, 4, 0);
            lblTafqit.Size = new Size(388, 34);
            lblTafqit.TabIndex = 4;
            lblTafqit.Text = "—";
            lblTafqit.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlActions
            // 
            pnlActions.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlActions.Controls.Add(flpActions);
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Location = new Point(0, 832);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(8);
            pnlActions.Size = new Size(1322, 68);
            pnlActions.TabIndex = 5;
            // 
            // flpActions
            // 
            flpActions.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpActions.Controls.Add(btnSave);
            flpActions.Controls.Add(btnNew);
            flpActions.Controls.Add(btnClose);
            flpActions.Dock = DockStyle.Fill;
            flpActions.Location = new Point(8, 8);
            flpActions.Name = "flpActions";
            flpActions.Size = new Size(1306, 52);
            flpActions.TabIndex = 0;
            flpActions.WrapContents = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Location = new Point(1069, 0);
            btnSave.Margin = new Padding(6, 0, 6, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(231, 46);
            btnSave.TabIndex = 0;
            btnSave.Text = "حفظ وترحيل [F4]";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // btnNew
            // 
            btnNew.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
            btnNew.Cursor = Cursors.Hand;
            btnNew.FlatAppearance.BorderSize = 0;
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNew.ForeColor = System.Drawing.Color.White;
            btnNew.Location = new Point(909, 0);
            btnNew.Margin = new Padding(6, 0, 6, 0);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(148, 46);
            btnNew.TabIndex = 1;
            btnNew.Text = "جديد [F2]";
            btnNew.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new Point(749, 0);
            btnClose.Margin = new Padding(6, 0, 6, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(148, 46);
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
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            ClientSize = new Size(1322, 900);
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
            pnlProducts.ResumeLayout(false);
            pnlProducts.PerformLayout();
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
            gbTotals.ResumeLayout(false);
            pnlTotals.ResumeLayout(false);
            flpTotals.ResumeLayout(false);
            flpTotals.PerformLayout();
            flpInvoiceDiscount.ResumeLayout(false);
            flpInvoiceDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).EndInit();
            pnlActions.ResumeLayout(false);
            flpActions.ResumeLayout(false);
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
