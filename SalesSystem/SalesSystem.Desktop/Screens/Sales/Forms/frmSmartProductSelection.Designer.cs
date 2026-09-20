namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmSmartProductSelection
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
            pnlHeader = new Panel();
            tblHeader = new TableLayoutPanel();
            lblTitle = new Label();
            lblWarehouseInfo = new Label();
            lblEscHint = new Label();
            tblMain = new TableLayoutPanel();
            pnlEdit = new Panel();
            pnlEditHeader = new Panel();
            lblEditTitle = new Label();
            pnlEditButtons = new Panel();
            flpEditButtons = new FlowLayoutPanel();
            btnInsertClose = new Button();
            btnInsertContinue = new Button();
            btnClose = new Button();
            pnlProducts = new Panel();
            txtSearch = new TextBox();
            flpCategories = new FlowLayoutPanel();
            dgvProducts = new DataGridView();
            pnlNetTotalCard = new Panel();
            tblNetCard = new TableLayoutPanel();
            lblNetTotalCaption = new Label();
            lblNetTotalVal = new Label();
            tblEdit = new TableLayoutPanel();
            pnlBanner = new Panel();
            lblBannerName = new Label();
            lblBannerBarcode = new Label();
            lblBannerStock = new Label();
            lblCostLabel = new Label();
            lblCostVal = new Label();
            lblDefaultPriceLabel = new Label();
            lblDefaultPriceVal = new Label();
            lblMinPriceLabel = new Label();
            lblMinPriceVal = new Label();
            lblPriceWarning = new Label();
            lblUnitLabel = new Label();
            cmbUnit = new ComboBox();
            lblFactorLabel = new Label();
            lblFactorVal = new Label();
            lblSalePriceLabel = new Label();
            numSalePrice = new NumericUpDown();
            lblQuantityLabel = new Label();
            numQuantity = new NumericUpDown();
            flpPresets = new FlowLayoutPanel();
            btnQty1 = new Button();
            btnQty5 = new Button();
            btnQty10 = new Button();
            btnQty12 = new Button();
            btnQty24 = new Button();
            btnQty48 = new Button();
            lblDiscountTypeLabel = new Label();
            cmbDiscountType = new ComboBox();
            lblDiscountValueLabel = new Label();
            numDiscountValue = new NumericUpDown();
            lblNetDiscountLabel = new Label();
            lblNetDiscountVal = new Label();
            lblExpiryLabel = new Label();
            dtpExpiry = new DateTimePicker();
            lblBatchLabel = new Label();
            txtBatch = new TextBox();
            pnlHeader.SuspendLayout();
            tblHeader.SuspendLayout();
            tblMain.SuspendLayout();
            pnlEdit.SuspendLayout();
            pnlEditHeader.SuspendLayout();
            pnlEditButtons.SuspendLayout();
            flpEditButtons.SuspendLayout();
            pnlProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            pnlNetTotalCard.SuspendLayout();
            tblNetCard.SuspendLayout();
            tblEdit.SuspendLayout();
            pnlBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSalePrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numQuantity).BeginInit();
            flpPresets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDiscountValue).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
            pnlHeader.Controls.Add(tblHeader);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 72;
            // 
            // tblHeader
            // 
            tblHeader.ColumnCount = 3;
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tblHeader.Controls.Add(lblTitle, 0, 0);
            tblHeader.Controls.Add(lblWarehouseInfo, 1, 0);
            tblHeader.Controls.Add(lblEscHint, 2, 0);
            tblHeader.Dock = DockStyle.Fill;
            tblHeader.RowCount = 1;
            tblHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Text = "اختيار المنتج [F3]";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblWarehouseInfo
            // 
            lblWarehouseInfo.Dock = DockStyle.Fill;
            lblWarehouseInfo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblWarehouseInfo.ForeColor = Color.FromArgb(180, 200, 220);
            lblWarehouseInfo.Text = "المخزن النشط: المخزن الرئيسي";
            lblWarehouseInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEscHint
            // 
            lblEscHint.Dock = DockStyle.Fill;
            lblEscHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEscHint.ForeColor = Color.FromArgb(150, 170, 190);
            lblEscHint.Text = "Esc = إغلاق";
            lblEscHint.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tblMain
            // 
            tblMain.ColumnCount = 2;
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tblMain.Controls.Add(pnlEdit, 0, 0);
            tblMain.Controls.Add(pnlProducts, 1, 0);
            tblMain.Dock = DockStyle.Fill;
            tblMain.RowCount = 1;
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // pnlEdit
            // 
            pnlEdit.BackColor = Color.White;
            pnlEdit.Controls.Add(tblEdit);
            pnlEdit.Controls.Add(pnlEditButtons);
            pnlEdit.Controls.Add(pnlEditHeader);
            pnlEdit.Dock = DockStyle.Fill;
            pnlEdit.Padding = new Padding(8);
            // 
            // pnlEditHeader
            // 
            pnlEditHeader.BackColor = Color.FromArgb(233, 236, 239);
            pnlEditHeader.Controls.Add(lblEditTitle);
            pnlEditHeader.Dock = DockStyle.Top;
            pnlEditHeader.Height = 36;
            // 
            // lblEditTitle
            // 
            lblEditTitle.Dock = DockStyle.Fill;
            lblEditTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEditTitle.ForeColor = Color.FromArgb(33, 43, 54);
            lblEditTitle.Text = "تعديل البيانات المقترحة";
            lblEditTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlEditButtons
            // 
            pnlEditButtons.BackColor = Color.FromArgb(248, 249, 250);
            pnlEditButtons.Controls.Add(flpEditButtons);
            pnlEditButtons.Dock = DockStyle.Bottom;
            pnlEditButtons.Height = 62;
            // 
            // flpEditButtons
            // 
            flpEditButtons.Controls.Add(btnInsertClose);
            flpEditButtons.Controls.Add(btnInsertContinue);
            flpEditButtons.Controls.Add(btnClose);
            flpEditButtons.Dock = DockStyle.Fill;
            flpEditButtons.FlowDirection = FlowDirection.RightToLeft;
            flpEditButtons.Padding = new Padding(4, 10, 4, 8);
            // 
            // btnInsertClose
            // 
            btnInsertClose.BackColor = Color.FromArgb(25, 135, 84);
            btnInsertClose.FlatStyle = FlatStyle.Popup;
            btnInsertClose.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnInsertClose.ForeColor = Color.White;
            btnInsertClose.Height = 40;
            btnInsertClose.Margin = new Padding(0, 0, 6, 0);
            btnInsertClose.Text = "إدراج وإغلاق [Ctrl+Enter]";
            btnInsertClose.Width = 190;
            btnInsertClose.Click += btnInsertClose_Click;
            // 
            // btnInsertContinue
            // 
            btnInsertContinue.BackColor = Color.FromArgb(13, 110, 253);
            btnInsertContinue.FlatStyle = FlatStyle.Popup;
            btnInsertContinue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnInsertContinue.ForeColor = Color.White;
            btnInsertContinue.Height = 40;
            btnInsertContinue.Margin = new Padding(0, 0, 6, 0);
            btnInsertContinue.Text = "إدراج ومتابعة [F8]";
            btnInsertContinue.Width = 180;
            btnInsertContinue.Click += btnInsertContinue_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.FromArgb(33, 43, 54);
            btnClose.Height = 40;
            btnClose.Margin = new Padding(0, 0, 6, 0);
            btnClose.Text = "إغلاق";
            btnClose.Width = 90;
            btnClose.Click += btnClose_Click;
            // 
            // pnlProducts
            // 
            pnlProducts.BackColor = Color.White;
            pnlProducts.Controls.Add(dgvProducts);
            pnlProducts.Controls.Add(flpCategories);
            pnlProducts.Controls.Add(txtSearch);
            pnlProducts.Dock = DockStyle.Fill;
            pnlProducts.Padding = new Padding(8);
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Top;
            txtSearch.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Height = 36;
            txtSearch.PlaceholderText = "بحث بالاسم / الكود / الباركود...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // flpCategories
            // 
            flpCategories.Dock = DockStyle.Top;
            flpCategories.FlowDirection = FlowDirection.RightToLeft;
            flpCategories.Height = 42;
            flpCategories.Padding = new Padding(0, 4, 0, 4);
            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.BorderStyle = BorderStyle.Fixed3D;
            dgvProducts.ColumnHeadersHeight = 44;
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.RowTemplate.Height = 34;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.CellClick += dgvProducts_CellClick;
            dgvProducts.CellDoubleClick += dgvProducts_CellDoubleClick;
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;
            // 
            // pnlNetTotalCard
            // 
            pnlNetTotalCard.BackColor = Color.FromArgb(211, 240, 222);
            pnlNetTotalCard.Controls.Add(tblNetCard);
            pnlNetTotalCard.Dock = DockStyle.Fill;
            pnlNetTotalCard.Height = 52;
            pnlNetTotalCard.Padding = new Padding(8, 4, 8, 4);
            // 
            // tblNetCard
            // 
            tblNetCard.ColumnCount = 2;
            tblNetCard.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tblNetCard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblNetCard.Controls.Add(lblNetTotalCaption, 0, 0);
            tblNetCard.Controls.Add(lblNetTotalVal, 1, 0);
            tblNetCard.Dock = DockStyle.Fill;
            tblNetCard.RowCount = 1;
            tblNetCard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblNetTotalCaption
            // 
            lblNetTotalCaption.Dock = DockStyle.Fill;
            lblNetTotalCaption.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetTotalCaption.ForeColor = Color.FromArgb(21, 87, 36);
            lblNetTotalCaption.Text = "الإجمالي الصافي:";
            lblNetTotalCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetTotalVal
            // 
            lblNetTotalVal.Dock = DockStyle.Fill;
            lblNetTotalVal.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetTotalVal.ForeColor = Color.FromArgb(21, 87, 36);
            lblNetTotalVal.Text = "0.00";
            lblNetTotalVal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tblEdit
            // 
            tblEdit.ColumnCount = 2;
            tblEdit.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tblEdit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblEdit.Controls.Add(pnlBanner, 0, 0);
            tblEdit.SetColumnSpan(pnlBanner, 2);
            tblEdit.Controls.Add(lblCostLabel, 0, 1);
            tblEdit.Controls.Add(lblCostVal, 1, 1);
            tblEdit.Controls.Add(lblDefaultPriceLabel, 0, 2);
            tblEdit.Controls.Add(lblDefaultPriceVal, 1, 2);
            tblEdit.Controls.Add(lblMinPriceLabel, 0, 3);
            tblEdit.Controls.Add(lblMinPriceVal, 1, 3);
            tblEdit.Controls.Add(lblPriceWarning, 0, 4);
            tblEdit.SetColumnSpan(lblPriceWarning, 2);
            tblEdit.Controls.Add(lblUnitLabel, 0, 5);
            tblEdit.Controls.Add(cmbUnit, 1, 5);
            tblEdit.Controls.Add(lblFactorLabel, 0, 6);
            tblEdit.Controls.Add(lblFactorVal, 1, 6);
            tblEdit.Controls.Add(lblSalePriceLabel, 0, 7);
            tblEdit.Controls.Add(numSalePrice, 1, 7);
            tblEdit.Controls.Add(lblQuantityLabel, 0, 8);
            tblEdit.Controls.Add(numQuantity, 1, 8);
            tblEdit.Controls.Add(flpPresets, 0, 9);
            tblEdit.SetColumnSpan(flpPresets, 2);
            tblEdit.Controls.Add(lblDiscountTypeLabel, 0, 10);
            tblEdit.Controls.Add(cmbDiscountType, 1, 10);
            tblEdit.Controls.Add(lblDiscountValueLabel, 0, 11);
            tblEdit.Controls.Add(numDiscountValue, 1, 11);
            tblEdit.Controls.Add(lblNetDiscountLabel, 0, 12);
            tblEdit.Controls.Add(lblNetDiscountVal, 1, 12);
            tblEdit.Controls.Add(lblExpiryLabel, 0, 13);
            tblEdit.Controls.Add(dtpExpiry, 1, 13);
            tblEdit.Controls.Add(lblBatchLabel, 0, 14);
            tblEdit.Controls.Add(txtBatch, 1, 14);
            tblEdit.Controls.Add(pnlNetTotalCard, 0, 16);
            tblEdit.SetColumnSpan(pnlNetTotalCard, 2);
            tblEdit.Dock = DockStyle.Fill;
            tblEdit.RowCount = 17;
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblEdit.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            // 
            // pnlBanner
            // 
            pnlBanner.BackColor = Color.FromArgb(232, 244, 253);
            pnlBanner.Controls.Add(lblBannerStock);
            pnlBanner.Controls.Add(lblBannerBarcode);
            pnlBanner.Controls.Add(lblBannerName);
            pnlBanner.Dock = DockStyle.Fill;
            pnlBanner.Padding = new Padding(8, 4, 8, 4);
            // 
            // lblBannerName
            // 
            lblBannerName.Dock = DockStyle.Top;
            lblBannerName.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBannerName.ForeColor = Color.FromArgb(13, 60, 120);
            lblBannerName.Height = 26;
            lblBannerName.Text = "اسم المنتج";
            lblBannerName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBannerBarcode
            // 
            lblBannerBarcode.Dock = DockStyle.Top;
            lblBannerBarcode.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBannerBarcode.ForeColor = Color.FromArgb(60, 90, 120);
            lblBannerBarcode.Height = 16;
            lblBannerBarcode.Text = "الباركود: —";
            lblBannerBarcode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBannerStock
            // 
            lblBannerStock.Dock = DockStyle.Top;
            lblBannerStock.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBannerStock.ForeColor = Color.FromArgb(21, 87, 36);
            lblBannerStock.Height = 16;
            lblBannerStock.Text = "الرصيد: —";
            lblBannerStock.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCostLabel
            // 
            lblCostLabel.Dock = DockStyle.Fill;
            lblCostLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCostLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblCostLabel.Text = "التكلفة:";
            lblCostLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCostVal
            // 
            lblCostVal.Dock = DockStyle.Fill;
            lblCostVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCostVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblCostVal.Text = "0.00";
            lblCostVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDefaultPriceLabel
            // 
            lblDefaultPriceLabel.Dock = DockStyle.Fill;
            lblDefaultPriceLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDefaultPriceLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblDefaultPriceLabel.Text = "السعر الافتراضي:";
            lblDefaultPriceLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDefaultPriceVal
            // 
            lblDefaultPriceVal.Dock = DockStyle.Fill;
            lblDefaultPriceVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDefaultPriceVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblDefaultPriceVal.Text = "0.00";
            lblDefaultPriceVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMinPriceLabel
            // 
            lblMinPriceLabel.Dock = DockStyle.Fill;
            lblMinPriceLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMinPriceLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblMinPriceLabel.Text = "أدنى سعر مسموح:";
            lblMinPriceLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMinPriceVal
            // 
            lblMinPriceVal.Dock = DockStyle.Fill;
            lblMinPriceVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMinPriceVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblMinPriceVal.Text = "0.00";
            lblMinPriceVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPriceWarning
            // 
            lblPriceWarning.Dock = DockStyle.Fill;
            lblPriceWarning.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPriceWarning.ForeColor = Color.FromArgb(220, 53, 69);
            lblPriceWarning.Text = "تنبيه: سعر البيع أقل من التكلفة";
            lblPriceWarning.TextAlign = ContentAlignment.MiddleLeft;
            lblPriceWarning.Visible = false;
            // 
            // lblUnitLabel
            // 
            lblUnitLabel.Dock = DockStyle.Fill;
            lblUnitLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUnitLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblUnitLabel.Text = "الوحدة:";
            lblUnitLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbUnit
            // 
            cmbUnit.Dock = DockStyle.Fill;
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
            // 
            // lblFactorLabel
            // 
            lblFactorLabel.Dock = DockStyle.Fill;
            lblFactorLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFactorLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblFactorLabel.Text = "معامل التحويل:";
            lblFactorLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFactorVal
            // 
            lblFactorVal.Dock = DockStyle.Fill;
            lblFactorVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFactorVal.ForeColor = Color.FromArgb(33, 43, 54);
            lblFactorVal.Text = "1";
            lblFactorVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSalePriceLabel
            // 
            lblSalePriceLabel.Dock = DockStyle.Fill;
            lblSalePriceLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSalePriceLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblSalePriceLabel.Text = "سعر البيع:";
            lblSalePriceLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numSalePrice
            // 
            numSalePrice.DecimalPlaces = 2;
            numSalePrice.Dock = DockStyle.Fill;
            numSalePrice.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numSalePrice.Maximum = 9999999;
            numSalePrice.ThousandsSeparator = true;
            numSalePrice.ValueChanged += numSalePrice_ValueChanged;
            // 
            // lblQuantityLabel
            // 
            lblQuantityLabel.Dock = DockStyle.Fill;
            lblQuantityLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuantityLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblQuantityLabel.Text = "الكمية:";
            lblQuantityLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numQuantity
            // 
            numQuantity.DecimalPlaces = 3;
            numQuantity.Dock = DockStyle.Fill;
            numQuantity.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numQuantity.Maximum = 999999;
            numQuantity.Minimum = 0.001m;
            numQuantity.Value = 1;
            numQuantity.ValueChanged += numQuantity_ValueChanged;
            // 
            // flpPresets
            // 
            flpPresets.Controls.Add(btnQty1);
            flpPresets.Controls.Add(btnQty5);
            flpPresets.Controls.Add(btnQty10);
            flpPresets.Controls.Add(btnQty12);
            flpPresets.Controls.Add(btnQty24);
            flpPresets.Controls.Add(btnQty48);
            flpPresets.Dock = DockStyle.Fill;
            flpPresets.FlowDirection = FlowDirection.RightToLeft;
            flpPresets.Padding = new Padding(0, 3, 0, 3);
            // 
            // btnQty1
            // 
            btnQty1.BackColor = Color.FromArgb(248, 249, 250);
            btnQty1.FlatStyle = FlatStyle.Popup;
            btnQty1.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQty1.Height = 30;
            btnQty1.Margin = new Padding(0, 0, 4, 0);
            btnQty1.Text = "1";
            btnQty1.Width = 42;
            btnQty1.Click += btnQtyPreset_Click;
            // 
            // btnQty5
            // 
            btnQty5.BackColor = Color.FromArgb(248, 249, 250);
            btnQty5.FlatStyle = FlatStyle.Popup;
            btnQty5.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQty5.Height = 30;
            btnQty5.Margin = new Padding(0, 0, 4, 0);
            btnQty5.Text = "5";
            btnQty5.Width = 42;
            btnQty5.Click += btnQtyPreset_Click;
            // 
            // btnQty10
            // 
            btnQty10.BackColor = Color.FromArgb(248, 249, 250);
            btnQty10.FlatStyle = FlatStyle.Popup;
            btnQty10.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQty10.Height = 30;
            btnQty10.Margin = new Padding(0, 0, 4, 0);
            btnQty10.Text = "10";
            btnQty10.Width = 42;
            btnQty10.Click += btnQtyPreset_Click;
            // 
            // btnQty12
            // 
            btnQty12.BackColor = Color.FromArgb(248, 249, 250);
            btnQty12.FlatStyle = FlatStyle.Popup;
            btnQty12.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQty12.Height = 30;
            btnQty12.Margin = new Padding(0, 0, 4, 0);
            btnQty12.Text = "12 (درزن)";
            btnQty12.Width = 84;
            btnQty12.Click += btnQtyPreset_Click;
            // 
            // btnQty24
            // 
            btnQty24.BackColor = Color.FromArgb(248, 249, 250);
            btnQty24.FlatStyle = FlatStyle.Popup;
            btnQty24.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQty24.Height = 30;
            btnQty24.Margin = new Padding(0, 0, 4, 0);
            btnQty24.Text = "24 (كرتون)";
            btnQty24.Width = 90;
            btnQty24.Click += btnQtyPreset_Click;
            // 
            // btnQty48
            // 
            btnQty48.BackColor = Color.FromArgb(248, 249, 250);
            btnQty48.FlatStyle = FlatStyle.Popup;
            btnQty48.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQty48.Height = 30;
            btnQty48.Margin = new Padding(0, 0, 4, 0);
            btnQty48.Text = "48";
            btnQty48.Width = 42;
            btnQty48.Click += btnQtyPreset_Click;
            // 
            // lblDiscountTypeLabel
            // 
            lblDiscountTypeLabel.Dock = DockStyle.Fill;
            lblDiscountTypeLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDiscountTypeLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblDiscountTypeLabel.Text = "نوع الخصم:";
            lblDiscountTypeLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbDiscountType
            // 
            cmbDiscountType.Dock = DockStyle.Fill;
            cmbDiscountType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDiscountType.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbDiscountType.Items.AddRange(new object[] { "مبلغ", "نسبة %" });
            cmbDiscountType.SelectedIndexChanged += cmbDiscountType_SelectedIndexChanged;
            // 
            // lblDiscountValueLabel
            // 
            lblDiscountValueLabel.Dock = DockStyle.Fill;
            lblDiscountValueLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDiscountValueLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblDiscountValueLabel.Text = "قيمة الخصم:";
            lblDiscountValueLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numDiscountValue
            // 
            numDiscountValue.DecimalPlaces = 2;
            numDiscountValue.Dock = DockStyle.Fill;
            numDiscountValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            numDiscountValue.Maximum = 9999999;
            numDiscountValue.ThousandsSeparator = true;
            numDiscountValue.ValueChanged += numDiscountValue_ValueChanged;
            // 
            // lblNetDiscountLabel
            // 
            lblNetDiscountLabel.Dock = DockStyle.Fill;
            lblNetDiscountLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNetDiscountLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblNetDiscountLabel.Text = "صافي الخصم:";
            lblNetDiscountLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNetDiscountVal
            // 
            lblNetDiscountVal.Dock = DockStyle.Fill;
            lblNetDiscountVal.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNetDiscountVal.ForeColor = Color.FromArgb(21, 87, 36);
            lblNetDiscountVal.Text = "0.00";
            lblNetDiscountVal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblExpiryLabel
            // 
            lblExpiryLabel.Dock = DockStyle.Fill;
            lblExpiryLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblExpiryLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblExpiryLabel.Text = "تاريخ الصلاحية:";
            lblExpiryLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpExpiry
            // 
            dtpExpiry.Dock = DockStyle.Fill;
            dtpExpiry.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpExpiry.Format = DateTimePickerFormat.Short;
            // 
            // lblBatchLabel
            // 
            lblBatchLabel.Dock = DockStyle.Fill;
            lblBatchLabel.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBatchLabel.ForeColor = Color.FromArgb(80, 90, 100);
            lblBatchLabel.Text = "رقم التشغيلة (Batch):";
            lblBatchLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtBatch
            // 
            txtBatch.Dock = DockStyle.Fill;
            txtBatch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBatch.PlaceholderText = "اختياري";
            // 
            pnlHeader.ResumeLayout(false);
            tblHeader.ResumeLayout(false);
            tblMain.ResumeLayout(false);
            pnlEdit.ResumeLayout(false);
            pnlEditHeader.ResumeLayout(false);
            pnlEditButtons.ResumeLayout(false);
            flpEditButtons.ResumeLayout(false);
            pnlProducts.ResumeLayout(false);
            pnlProducts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            pnlNetTotalCard.ResumeLayout(false);
            tblNetCard.ResumeLayout(false);
            tblEdit.ResumeLayout(false);
            pnlBanner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numSalePrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numQuantity).EndInit();
            flpPresets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numDiscountValue).EndInit();
            // 
            // frmSmartProductSelection
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1100, 720);
            Controls.Add(tblMain);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSmartProductSelection";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "اختيار المنتج [F3]";
            KeyDown += frmSmartProductSelection_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.TableLayoutPanel tblHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWarehouseInfo;
        private System.Windows.Forms.Label lblEscHint;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.Panel pnlEdit;
        private System.Windows.Forms.Panel pnlEditHeader;
        private System.Windows.Forms.Label lblEditTitle;
        private System.Windows.Forms.Panel pnlEditButtons;
        private System.Windows.Forms.FlowLayoutPanel flpEditButtons;
        private System.Windows.Forms.Button btnInsertClose;
        private System.Windows.Forms.Button btnInsertContinue;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlProducts;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Panel pnlNetTotalCard;
        private System.Windows.Forms.TableLayoutPanel tblNetCard;
        private System.Windows.Forms.Label lblNetTotalCaption;
        private System.Windows.Forms.Label lblNetTotalVal;
        private System.Windows.Forms.TableLayoutPanel tblEdit;
        private System.Windows.Forms.Panel pnlBanner;
        private System.Windows.Forms.Label lblBannerName;
        private System.Windows.Forms.Label lblBannerBarcode;
        private System.Windows.Forms.Label lblBannerStock;
        private System.Windows.Forms.Label lblCostLabel;
        private System.Windows.Forms.Label lblCostVal;
        private System.Windows.Forms.Label lblDefaultPriceLabel;
        private System.Windows.Forms.Label lblDefaultPriceVal;
        private System.Windows.Forms.Label lblMinPriceLabel;
        private System.Windows.Forms.Label lblMinPriceVal;
        private System.Windows.Forms.Label lblPriceWarning;
        private System.Windows.Forms.Label lblUnitLabel;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.Label lblFactorLabel;
        private System.Windows.Forms.Label lblFactorVal;
        private System.Windows.Forms.Label lblSalePriceLabel;
        private System.Windows.Forms.NumericUpDown numSalePrice;
        private System.Windows.Forms.Label lblQuantityLabel;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.FlowLayoutPanel flpPresets;
        private System.Windows.Forms.Button btnQty1;
        private System.Windows.Forms.Button btnQty5;
        private System.Windows.Forms.Button btnQty10;
        private System.Windows.Forms.Button btnQty12;
        private System.Windows.Forms.Button btnQty24;
        private System.Windows.Forms.Button btnQty48;
        private System.Windows.Forms.Label lblDiscountTypeLabel;
        private System.Windows.Forms.ComboBox cmbDiscountType;
        private System.Windows.Forms.Label lblDiscountValueLabel;
        private System.Windows.Forms.NumericUpDown numDiscountValue;
        private System.Windows.Forms.Label lblNetDiscountLabel;
        private System.Windows.Forms.Label lblNetDiscountVal;
        private System.Windows.Forms.Label lblExpiryLabel;
        private System.Windows.Forms.DateTimePicker dtpExpiry;
        private System.Windows.Forms.Label lblBatchLabel;
        private System.Windows.Forms.TextBox txtBatch;
    }
}