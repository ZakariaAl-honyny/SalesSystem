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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblInvoiceInfo = new Label();
            lblMode = new Label();
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
            pnlProducts = new Panel();
            flpProducts = new FlowLayoutPanel();
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
            pnlTotals = new Panel();
            flpTotals = new FlowLayoutPanel();
            lblSubTotal = new Label();
            lblDiscountTotal = new Label();
            lblNetTotal = new Label();
            flpInvoiceDiscount = new FlowLayoutPanel();
            lblInvoiceDiscountCaption = new Label();
            numInvoiceDiscount = new NumericUpDown();
            lblTafqit = new Label();
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
            pnlActions = new Panel();
            flpActions = new FlowLayoutPanel();
            btnSave = new Button();
            btnNew = new Button();
            btnClose = new Button();
            toolTip = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlTop.SuspendLayout();
            flpTopBar.SuspendLayout();
            flpBarcode.SuspendLayout();
            flpProductSearch.SuspendLayout();
            flpCategory.SuspendLayout();
            pnlProducts.SuspendLayout();
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
            pnlTotals.SuspendLayout();
            flpTotals.SuspendLayout();
            flpInvoiceDiscount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).BeginInit();
            pnlPayment.SuspendLayout();
            flpPayment.SuspendLayout();
            flpCustomer.SuspendLayout();
            flpWarehouse.SuspendLayout();
            flpCashBox.SuspendLayout();
            flpPaid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPaid).BeginInit();
            pnlActions.SuspendLayout();
            flpActions.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
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
            lblInvoiceInfo.Dock = DockStyle.Fill;
            lblInvoiceInfo.Font = new Font("Tahoma", 9F);
            lblInvoiceInfo.Location = new Point(8, 36);
            lblInvoiceInfo.Name = "lblInvoiceInfo";
            lblInvoiceInfo.Size = new Size(1264, 16);
            lblInvoiceInfo.TabIndex = 1;
            lblInvoiceInfo.Text = "التاريخ: —   |   المخزن: —   |   الصندوق: —";
            lblInvoiceInfo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblMode
            // 
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblMode.Location = new Point(8, 6);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1264, 30);
            lblMode.TabIndex = 0;
            lblMode.Text = "نقطة البيع — فاتورة جديدة";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(flpTopBar);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 56);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(8, 6, 8, 6);
            pnlTop.Size = new Size(1280, 76);
            pnlTop.TabIndex = 1;
            // 
            // flpTopBar
            // 
            flpTopBar.Controls.Add(flpBarcode);
            flpTopBar.Controls.Add(flpProductSearch);
            flpTopBar.Controls.Add(flpCategory);
            flpTopBar.Controls.Add(btnRefreshCatalog);
            flpTopBar.Dock = DockStyle.Fill;
            flpTopBar.FlowDirection = FlowDirection.RightToLeft;
            flpTopBar.Location = new Point(8, 6);
            flpTopBar.Name = "flpTopBar";
            flpTopBar.Size = new Size(1264, 64);
            flpTopBar.TabIndex = 0;
            flpTopBar.WrapContents = false;
            // 
            // flpBarcode
            // 
            flpBarcode.AutoSize = true;
            flpBarcode.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpBarcode.Controls.Add(lblBarcodeCaption);
            flpBarcode.Controls.Add(txtBarcode);
            flpBarcode.FlowDirection = FlowDirection.RightToLeft;
            flpBarcode.Location = new Point(856, 4);
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
            txtBarcode.Font = new Font("Tahoma", 15F, FontStyle.Bold);
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
            flpProductSearch.Controls.Add(lblProductSearchCaption);
            flpProductSearch.Controls.Add(txtProductSearch);
            flpProductSearch.FlowDirection = FlowDirection.RightToLeft;
            flpProductSearch.Location = new Point(536, 4);
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
            txtProductSearch.Font = new Font("Tahoma", 11F);
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
            flpCategory.Controls.Add(lblCategoryCaption);
            flpCategory.Controls.Add(cbCategory);
            flpCategory.FlowDirection = FlowDirection.RightToLeft;
            flpCategory.Location = new Point(316, 4);
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
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategory.Font = new Font("Tahoma", 10F);
            cbCategory.FormattingEnabled = true;
            cbCategory.Location = new Point(6, 3);
            cbCategory.Margin = new Padding(4, 3, 4, 3);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(156, 30);
            cbCategory.TabIndex = 1;
            // 
            // btnRefreshCatalog
            // 
            btnRefreshCatalog.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefreshCatalog.Location = new Point(166, 4);
            btnRefreshCatalog.Margin = new Padding(4);
            btnRefreshCatalog.Name = "btnRefreshCatalog";
            btnRefreshCatalog.Size = new Size(164, 40);
            btnRefreshCatalog.TabIndex = 3;
            btnRefreshCatalog.Text = "تحديث الأصناف [F5]";
            btnRefreshCatalog.UseVisualStyleBackColor = true;
            // 
            // pnlProducts
            // 
            pnlProducts.AutoScroll = true;
            pnlProducts.Controls.Add(flpProducts);
            pnlProducts.Dock = DockStyle.Right;
            pnlProducts.Location = new Point(900, 132);
            pnlProducts.Name = "pnlProducts";
            pnlProducts.Padding = new Padding(4);
            pnlProducts.Size = new Size(380, 460);
            pnlProducts.TabIndex = 2;
            // 
            // flpProducts
            // 
            flpProducts.AutoSize = true;
            flpProducts.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpProducts.Dock = DockStyle.Top;
            flpProducts.FlowDirection = FlowDirection.TopDown;
            flpProducts.Location = new Point(4, 4);
            flpProducts.Name = "flpProducts";
            flpProducts.Padding = new Padding(2);
            flpProducts.Size = new Size(372, 0);
            flpProducts.TabIndex = 0;
            flpProducts.WrapContents = false;
            // 
            // pnlCart
            // 
            pnlCart.Controls.Add(dgvCart);
            pnlCart.Dock = DockStyle.Fill;
            pnlCart.Location = new Point(0, 132);
            pnlCart.Name = "pnlCart";
            pnlCart.Size = new Size(900, 460);
            pnlCart.TabIndex = 3;
            // 
            // dgvCart
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AllowUserToResizeRows = false;
            dgvCart.AutoGenerateColumns = false;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.ColumnHeadersHeight = 42;
            dgvCart.Columns.AddRange(new DataGridViewColumn[] { colLineNo, colProduct, colBarcode, colQuantity, colUnitPrice, colDiscount, colTotal });
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.Location = new Point(0, 0);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.ReadOnly = false;
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowTemplate.Height = 32;
            dgvCart.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCart.Size = new Size(900, 460);
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
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N3";
            colQuantity.DefaultCellStyle = dataGridViewCellStyle2;
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
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            colUnitPrice.DefaultCellStyle = dataGridViewCellStyle3;
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
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            colDiscount.DefaultCellStyle = dataGridViewCellStyle4;
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
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            colTotal.DefaultCellStyle = dataGridViewCellStyle5;
            colTotal.FillWeight = 16F;
            colTotal.HeaderText = "الإجمالي";
            colTotal.MinimumWidth = 90;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            colTotal.Width = 120;
            // 
            // pnlLine
            // 
            pnlLine.Controls.Add(flpLine);
            pnlLine.Dock = DockStyle.Bottom;
            pnlLine.Location = new Point(0, 592);
            pnlLine.Name = "pnlLine";
            pnlLine.Padding = new Padding(8, 6, 8, 6);
            pnlLine.Size = new Size(1280, 62);
            pnlLine.TabIndex = 4;
            // 
            // flpLine
            // 
            flpLine.Controls.Add(lblSelectedProduct);
            flpLine.Controls.Add(flpQty);
            flpLine.Controls.Add(flpPrice);
            flpLine.Controls.Add(flpLineDiscount);
            flpLine.Controls.Add(btnApplyLine);
            flpLine.Controls.Add(btnRemoveLine);
            flpLine.Dock = DockStyle.Fill;
            flpLine.FlowDirection = FlowDirection.RightToLeft;
            flpLine.Location = new Point(8, 6);
            flpLine.Name = "flpLine";
            flpLine.Size = new Size(1264, 50);
            flpLine.TabIndex = 0;
            flpLine.WrapContents = false;
            // 
            // lblSelectedProduct
            // 
            lblSelectedProduct.AutoEllipsis = true;
            lblSelectedProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSelectedProduct.Location = new Point(1014, 4);
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
            flpQty.Controls.Add(lblQtyCaption);
            flpQty.Controls.Add(numQty);
            flpQty.FlowDirection = FlowDirection.RightToLeft;
            flpQty.Location = new Point(868, 4);
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
            numQty.DecimalPlaces = 3;
            numQty.Font = new Font("Tahoma", 10F);
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
            flpPrice.Controls.Add(lblPriceCaption);
            flpPrice.Controls.Add(numUnitPrice);
            flpPrice.FlowDirection = FlowDirection.RightToLeft;
            flpPrice.Location = new Point(716, 4);
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
            numUnitPrice.DecimalPlaces = 2;
            numUnitPrice.Font = new Font("Tahoma", 10F);
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
            flpLineDiscount.Controls.Add(lblLineDiscountCaption);
            flpLineDiscount.Controls.Add(numLineDiscount);
            flpLineDiscount.FlowDirection = FlowDirection.RightToLeft;
            flpLineDiscount.Location = new Point(564, 4);
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
            numLineDiscount.DecimalPlaces = 2;
            numLineDiscount.Font = new Font("Tahoma", 10F);
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
            btnApplyLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnApplyLine.Location = new Point(456, 4);
            btnApplyLine.Margin = new Padding(4);
            btnApplyLine.Name = "btnApplyLine";
            btnApplyLine.Size = new Size(100, 40);
            btnApplyLine.TabIndex = 4;
            btnApplyLine.Text = "تطبيق";
            btnApplyLine.UseVisualStyleBackColor = true;
            // 
            // btnRemoveLine
            // 
            btnRemoveLine.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRemoveLine.Location = new Point(324, 4);
            btnRemoveLine.Margin = new Padding(4);
            btnRemoveLine.Name = "btnRemoveLine";
            btnRemoveLine.Size = new Size(156, 40);
            btnRemoveLine.TabIndex = 5;
            btnRemoveLine.Text = "حذف السطر [Del]";
            btnRemoveLine.UseVisualStyleBackColor = true;
            // 
            // pnlTotals
            // 
            pnlTotals.Controls.Add(flpTotals);
            pnlTotals.Dock = DockStyle.Bottom;
            pnlTotals.Location = new Point(0, 654);
            pnlTotals.Name = "pnlTotals";
            pnlTotals.Padding = new Padding(8, 4, 8, 4);
            pnlTotals.Size = new Size(1280, 58);
            pnlTotals.TabIndex = 5;
            // 
            // flpTotals
            // 
            flpTotals.Controls.Add(lblSubTotal);
            flpTotals.Controls.Add(lblDiscountTotal);
            flpTotals.Controls.Add(lblNetTotal);
            flpTotals.Controls.Add(flpInvoiceDiscount);
            flpTotals.Controls.Add(lblTafqit);
            flpTotals.Dock = DockStyle.Fill;
            flpTotals.FlowDirection = FlowDirection.RightToLeft;
            flpTotals.Location = new Point(8, 4);
            flpTotals.Name = "flpTotals";
            flpTotals.Size = new Size(1264, 50);
            flpTotals.TabIndex = 0;
            flpTotals.WrapContents = false;
            // 
            // lblSubTotal
            // 
            lblSubTotal.AutoSize = true;
            lblSubTotal.Font = new Font("Tahoma", 10F);
            lblSubTotal.Location = new Point(1110, 6);
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
            lblDiscountTotal.Location = new Point(946, 6);
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
            lblNetTotal.Location = new Point(772, 4);
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
            flpInvoiceDiscount.Controls.Add(lblInvoiceDiscountCaption);
            flpInvoiceDiscount.Controls.Add(numInvoiceDiscount);
            flpInvoiceDiscount.FlowDirection = FlowDirection.RightToLeft;
            flpInvoiceDiscount.Location = new Point(606, 4);
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
            numInvoiceDiscount.DecimalPlaces = 2;
            numInvoiceDiscount.Font = new Font("Tahoma", 10F);
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
            lblTafqit.Location = new Point(168, 4);
            lblTafqit.Margin = new Padding(4);
            lblTafqit.Name = "lblTafqit";
            lblTafqit.Padding = new Padding(4, 0, 4, 0);
            lblTafqit.Size = new Size(430, 34);
            lblTafqit.TabIndex = 4;
            lblTafqit.Text = "—";
            lblTafqit.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlPayment
            // 
            pnlPayment.Controls.Add(flpPayment);
            pnlPayment.Dock = DockStyle.Bottom;
            pnlPayment.Location = new Point(0, 712);
            pnlPayment.Name = "pnlPayment";
            pnlPayment.Padding = new Padding(8, 4, 8, 4);
            pnlPayment.Size = new Size(1280, 84);
            pnlPayment.TabIndex = 6;
            // 
            // flpPayment
            // 
            flpPayment.Controls.Add(flpCustomer);
            flpPayment.Controls.Add(flpWarehouse);
            flpPayment.Controls.Add(flpCashBox);
            flpPayment.Controls.Add(rbtnCash);
            flpPayment.Controls.Add(rbtnCredit);
            flpPayment.Controls.Add(flpPaid);
            flpPayment.Controls.Add(lblRemaining);
            flpPayment.Dock = DockStyle.Fill;
            flpPayment.FlowDirection = FlowDirection.RightToLeft;
            flpPayment.Location = new Point(8, 4);
            flpPayment.Name = "flpPayment";
            flpPayment.Size = new Size(1264, 76);
            flpPayment.TabIndex = 0;
            // 
            // flpCustomer
            // 
            flpCustomer.AutoSize = true;
            flpCustomer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpCustomer.Controls.Add(lblCustomerCaption);
            flpCustomer.Controls.Add(cbCustomer);
            flpCustomer.FlowDirection = FlowDirection.RightToLeft;
            flpCustomer.Location = new Point(972, 4);
            flpCustomer.Margin = new Padding(4);
            flpCustomer.Name = "flpCustomer";
            flpCustomer.Size = new Size(288, 36);
            flpCustomer.TabIndex = 0;
            flpCustomer.WrapContents = false;
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.AutoSize = true;
            lblCustomerCaption.Font = new Font("Tahoma", 10F);
            lblCustomerCaption.Location = new Point(238, 6);
            lblCustomerCaption.Margin = new Padding(0, 6, 4, 0);
            lblCustomerCaption.Name = "lblCustomerCaption";
            lblCustomerCaption.Size = new Size(46, 24);
            lblCustomerCaption.TabIndex = 0;
            lblCustomerCaption.Text = "العميل:";
            lblCustomerCaption.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCustomer
            // 
            cbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCustomer.Font = new Font("Tahoma", 10F);
            cbCustomer.FormattingEnabled = true;
            cbCustomer.Location = new Point(4, 3);
            cbCustomer.Margin = new Padding(4, 3, 4, 3);
            cbCustomer.Name = "cbCustomer";
            cbCustomer.Size = new Size(200, 30);
            cbCustomer.TabIndex = 1;
            // 
            // flpWarehouse
            // 
            flpWarehouse.AutoSize = true;
            flpWarehouse.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpWarehouse.Controls.Add(lblWarehouseCaption);
            flpWarehouse.Controls.Add(cbWarehouse);
            flpWarehouse.FlowDirection = FlowDirection.RightToLeft;
            flpWarehouse.Location = new Point(748, 4);
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
            cbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWarehouse.Font = new Font("Tahoma", 10F);
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
            flpCashBox.Controls.Add(lblCashBoxCaption);
            flpCashBox.Controls.Add(cbCashBox);
            flpCashBox.FlowDirection = FlowDirection.RightToLeft;
            flpCashBox.Location = new Point(532, 4);
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
            cbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCashBox.Font = new Font("Tahoma", 10F);
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
            rbtnCash.Checked = true;
            rbtnCash.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbtnCash.Location = new Point(446, 4);
            rbtnCash.Margin = new Padding(4);
            rbtnCash.Name = "rbtnCash";
            rbtnCash.Size = new Size(78, 28);
            rbtnCash.TabIndex = 3;
            rbtnCash.TabStop = true;
            rbtnCash.Text = "نقدي";
            rbtnCash.UseVisualStyleBackColor = true;
            // 
            // rbtnCredit
            // 
            rbtnCredit.AutoSize = true;
            rbtnCredit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbtnCredit.Location = new Point(366, 4);
            rbtnCredit.Margin = new Padding(4);
            rbtnCredit.Name = "rbtnCredit";
            rbtnCredit.Size = new Size(72, 28);
            rbtnCredit.TabIndex = 4;
            rbtnCredit.Text = "آجل";
            rbtnCredit.UseVisualStyleBackColor = true;
            // 
            // flpPaid
            // 
            flpPaid.AutoSize = true;
            flpPaid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpPaid.Controls.Add(lblPaidCaption);
            flpPaid.Controls.Add(numPaid);
            flpPaid.FlowDirection = FlowDirection.RightToLeft;
            flpPaid.Location = new Point(224, 4);
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
            numPaid.DecimalPlaces = 2;
            numPaid.Font = new Font("Tahoma", 10F);
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
            lblRemaining.Location = new Point(4, 4);
            lblRemaining.Margin = new Padding(4);
            lblRemaining.Name = "lblRemaining";
            lblRemaining.Size = new Size(216, 34);
            lblRemaining.TabIndex = 6;
            lblRemaining.Text = "الباقي: 0.00";
            lblRemaining.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlActions
            // 
            pnlActions.Controls.Add(flpActions);
            pnlActions.Dock = DockStyle.Bottom;
            pnlActions.Location = new Point(0, 796);
            pnlActions.Name = "pnlActions";
            pnlActions.Padding = new Padding(8, 8, 8, 8);
            pnlActions.Size = new Size(1280, 64);
            pnlActions.TabIndex = 7;
            // 
            // flpActions
            // 
            flpActions.Controls.Add(btnSave);
            flpActions.Controls.Add(btnNew);
            flpActions.Controls.Add(btnClose);
            flpActions.Dock = DockStyle.Fill;
            flpActions.FlowDirection = FlowDirection.RightToLeft;
            flpActions.Location = new Point(8, 8);
            flpActions.Name = "flpActions";
            flpActions.Size = new Size(1264, 48);
            flpActions.TabIndex = 0;
            flpActions.WrapContents = false;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            btnSave.Location = new Point(1056, 0);
            btnSave.Margin = new Padding(6, 0, 6, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(202, 46);
            btnSave.TabIndex = 0;
            btnSave.Text = "حفظ وترحيل [F4]";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            btnNew.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            btnNew.Location = new Point(896, 0);
            btnNew.Margin = new Padding(6, 0, 6, 0);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(148, 46);
            btnNew.TabIndex = 1;
            btnNew.Text = "جديد [F2]";
            btnNew.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            btnClose.Location = new Point(758, 0);
            btnClose.Margin = new Padding(6, 0, 6, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(126, 46);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق [Esc]";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // frmPOS
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1280, 860);
            Controls.Add(pnlCart);
            Controls.Add(pnlLine);
            Controls.Add(pnlTotals);
            Controls.Add(pnlPayment);
            Controls.Add(pnlActions);
            Controls.Add(pnlProducts);
            Controls.Add(pnlTop);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F);
            KeyPreview = true;
            MinimumSize = new Size(1024, 720);
            Name = "frmPOS";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "نقطة البيع";
            pnlHeader.ResumeLayout(false);
            pnlTop.ResumeLayout(false);
            flpTopBar.ResumeLayout(false);
            flpTopBar.PerformLayout();
            flpBarcode.ResumeLayout(false);
            flpBarcode.PerformLayout();
            flpProductSearch.ResumeLayout(false);
            flpProductSearch.PerformLayout();
            flpCategory.ResumeLayout(false);
            flpCategory.PerformLayout();
            pnlProducts.ResumeLayout(false);
            pnlProducts.PerformLayout();
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
            pnlTotals.ResumeLayout(false);
            flpTotals.ResumeLayout(false);
            flpTotals.PerformLayout();
            flpInvoiceDiscount.ResumeLayout(false);
            flpInvoiceDiscount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numInvoiceDiscount).EndInit();
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
            pnlActions.ResumeLayout(false);
            flpActions.ResumeLayout(false);
            flpActions.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblInvoiceInfo;
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
        private System.Windows.Forms.Panel pnlProducts;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
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
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.FlowLayoutPanel flpTotals;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblDiscountTotal;
        private System.Windows.Forms.Label lblNetTotal;
        private System.Windows.Forms.FlowLayoutPanel flpInvoiceDiscount;
        private System.Windows.Forms.Label lblInvoiceDiscountCaption;
        private System.Windows.Forms.NumericUpDown numInvoiceDiscount;
        private System.Windows.Forms.Label lblTafqit;
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
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
