namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmPurchaseItemInfo
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolTip = new ToolTip(components);
            cmsHistory = new ContextMenuStrip(components);
            tsmiExport = new ToolStripMenuItem();
            tsmiRefresh = new ToolStripMenuItem();
            pnlTitle = new Panel();
            lblTitle = new Label();
            gbProductInfo = new GroupBox();
            tblInfo = new TableLayoutPanel();
            lblProductName = new Label();
            lblBarcodeCaption = new Label();
            lblCategoryCaption = new Label();
            lblStockCaption = new Label();
            lblBarcodeValue = new Label();
            lblCategoryValue = new Label();
            lblStockValue = new Label();
            gbCosts = new GroupBox();
            flowCostCards = new FlowLayoutPanel();
            cardAvg = new Panel();
            cardAvgAccent = new Label();
            cardAvgTitle = new Label();
            cardAvgValue = new Label();
            cardLast = new Panel();
            cardLastAccent = new Label();
            cardLastTitle = new Label();
            cardLastValue = new Label();
            cardMin = new Panel();
            cardMinAccent = new Label();
            cardMinTitle = new Label();
            cardMinValue = new Label();
            gbHistory = new GroupBox();
            dgvHistory = new DataGridView();
            emptyState = new Label();
            pnlFooter = new Panel();
            lblFooterLine = new Label();
            btnClose = new Button();
            lblRecordsCaption = new Label();
            lblRecordsCount = new Label();
            cmsHistory.SuspendLayout();
            pnlTitle.SuspendLayout();
            gbProductInfo.SuspendLayout();
            tblInfo.SuspendLayout();
            gbCosts.SuspendLayout();
            flowCostCards.SuspendLayout();
            cardAvg.SuspendLayout();
            cardLast.SuspendLayout();
            cardMin.SuspendLayout();
            gbHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // cmsHistory
            // 
            cmsHistory.BackColor = Color.White;
            cmsHistory.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmsHistory.ImageScalingSize = new Size(24, 24);
            cmsHistory.Items.AddRange(new ToolStripItem[] { tsmiExport, tsmiRefresh });
            cmsHistory.Name = "cmsHistory";
            cmsHistory.RightToLeft = RightToLeft.Yes;
            cmsHistory.ShowImageMargin = false;
            cmsHistory.Size = new Size(210, 60);
            // 
            // tsmiExport
            // 
            tsmiExport.Name = "tsmiExport";
            tsmiExport.Size = new Size(209, 28);
            tsmiExport.Text = "تصدير سجل المشتريات إلى Excel";
            tsmiExport.Click += tsmiExport_Click;
            // 
            // tsmiRefresh
            // 
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.Size = new Size(209, 28);
            tsmiRefresh.Text = "تحديث البيانات";
            tsmiRefresh.Click += tsmiRefresh_Click;
            // 
            // pnlTitle
            // 
            pnlTitle.BackColor = Color.FromArgb(14, 165, 103);
            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Dock = DockStyle.Top;
            pnlTitle.Location = new Point(0, 0);
            pnlTitle.Margin = new Padding(0);
            pnlTitle.Name = "pnlTitle";
            pnlTitle.Size = new Size(700, 46);
            pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(14, 165, 103);
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(700, 46);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "معلومات صنف";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbProductInfo
            // 
            gbProductInfo.BackColor = Color.FromArgb(235, 245, 235);
            gbProductInfo.Controls.Add(tblInfo);
            gbProductInfo.Dock = DockStyle.Top;
            gbProductInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbProductInfo.ForeColor = Color.FromArgb(14, 82, 52);
            gbProductInfo.Location = new Point(0, 46);
            gbProductInfo.Name = "gbProductInfo";
            gbProductInfo.Padding = new Padding(12, 4, 12, 8);
            gbProductInfo.Size = new Size(700, 108);
            gbProductInfo.TabIndex = 1;
            gbProductInfo.TabStop = false;
            gbProductInfo.Text = "بيانات الصنف";
            // 
            // tblInfo
            // 
            tblInfo.ColumnCount = 3;
            tblInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tblInfo.Controls.Add(lblProductName, 0, 0);
            tblInfo.Controls.Add(lblBarcodeCaption, 0, 1);
            tblInfo.Controls.Add(lblCategoryCaption, 1, 1);
            tblInfo.Controls.Add(lblStockCaption, 2, 1);
            tblInfo.Controls.Add(lblBarcodeValue, 0, 2);
            tblInfo.Controls.Add(lblCategoryValue, 1, 2);
            tblInfo.Controls.Add(lblStockValue, 2, 2);
            tblInfo.Dock = DockStyle.Fill;
            tblInfo.Location = new Point(12, 20);
            tblInfo.Name = "tblInfo";
            tblInfo.RowCount = 3;
            tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tblInfo.Size = new Size(676, 80);
            tblInfo.TabIndex = 0;
            // 
            // lblProductName
            // 
            tblInfo.SetColumnSpan(lblProductName, 3);
            lblProductName.Dock = DockStyle.Fill;
            lblProductName.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblProductName.ForeColor = Color.FromArgb(14, 82, 52);
            lblProductName.Location = new Point(3, 0);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(670, 34);
            lblProductName.TabIndex = 0;
            lblProductName.Text = "—";
            lblProductName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBarcodeCaption
            // 
            lblBarcodeCaption.Dock = DockStyle.Fill;
            lblBarcodeCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblBarcodeCaption.Location = new Point(3, 34);
            lblBarcodeCaption.Name = "lblBarcodeCaption";
            lblBarcodeCaption.Size = new Size(219, 22);
            lblBarcodeCaption.TabIndex = 1;
            lblBarcodeCaption.Text = "الباركود:";
            lblBarcodeCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCategoryCaption
            // 
            lblCategoryCaption.Dock = DockStyle.Fill;
            lblCategoryCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategoryCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblCategoryCaption.Location = new Point(228, 34);
            lblCategoryCaption.Name = "lblCategoryCaption";
            lblCategoryCaption.Size = new Size(219, 22);
            lblCategoryCaption.TabIndex = 2;
            lblCategoryCaption.Text = "التصنيف:";
            lblCategoryCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStockCaption
            // 
            lblStockCaption.Dock = DockStyle.Fill;
            lblStockCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStockCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblStockCaption.Location = new Point(453, 34);
            lblStockCaption.Name = "lblStockCaption";
            lblStockCaption.Size = new Size(220, 22);
            lblStockCaption.TabIndex = 3;
            lblStockCaption.Text = "الرصيد الحالي:";
            lblStockCaption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBarcodeValue
            // 
            lblBarcodeValue.Dock = DockStyle.Fill;
            lblBarcodeValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarcodeValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblBarcodeValue.Location = new Point(3, 56);
            lblBarcodeValue.Name = "lblBarcodeValue";
            lblBarcodeValue.Size = new Size(219, 24);
            lblBarcodeValue.TabIndex = 4;
            lblBarcodeValue.Text = "—";
            lblBarcodeValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCategoryValue
            // 
            lblCategoryValue.Dock = DockStyle.Fill;
            lblCategoryValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategoryValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblCategoryValue.Location = new Point(228, 56);
            lblCategoryValue.Name = "lblCategoryValue";
            lblCategoryValue.Size = new Size(219, 24);
            lblCategoryValue.TabIndex = 5;
            lblCategoryValue.Text = "—";
            lblCategoryValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblStockValue
            // 
            lblStockValue.Dock = DockStyle.Fill;
            lblStockValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStockValue.ForeColor = Color.FromArgb(14, 82, 52);
            lblStockValue.Location = new Point(453, 56);
            lblStockValue.Name = "lblStockValue";
            lblStockValue.Size = new Size(220, 24);
            lblStockValue.TabIndex = 6;
            lblStockValue.Text = "—";
            lblStockValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbCosts
            // 
            gbCosts.BackColor = Color.FromArgb(235, 245, 235);
            gbCosts.Controls.Add(flowCostCards);
            gbCosts.Dock = DockStyle.Top;
            gbCosts.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbCosts.ForeColor = Color.FromArgb(14, 82, 52);
            gbCosts.Location = new Point(0, 154);
            gbCosts.Name = "gbCosts";
            gbCosts.Padding = new Padding(10, 4, 10, 6);
            gbCosts.Size = new Size(700, 148);
            gbCosts.TabIndex = 2;
            gbCosts.TabStop = false;
            gbCosts.Text = "أسعار الشراء المقترحة";
            // 
            // flowCostCards
            // 
            flowCostCards.BackColor = Color.FromArgb(235, 245, 235);
            flowCostCards.Controls.Add(cardAvg);
            flowCostCards.Controls.Add(cardLast);
            flowCostCards.Controls.Add(cardMin);
            flowCostCards.Dock = DockStyle.Fill;
            flowCostCards.FlowDirection = FlowDirection.RightToLeft;
            flowCostCards.Location = new Point(10, 20);
            flowCostCards.Name = "flowCostCards";
            flowCostCards.Padding = new Padding(10, 4, 10, 6);
            flowCostCards.Size = new Size(680, 122);
            flowCostCards.TabIndex = 0;
            flowCostCards.WrapContents = false;
            // 
            // cardAvg
            // 
            cardAvg.BackColor = Color.White;
            cardAvg.BorderStyle = BorderStyle.FixedSingle;
            cardAvg.Controls.Add(cardAvgAccent);
            cardAvg.Controls.Add(cardAvgTitle);
            cardAvg.Controls.Add(cardAvgValue);
            cardAvg.Location = new Point(476, 24);
            cardAvg.Margin = new Padding(8, 6, 8, 6);
            cardAvg.Name = "cardAvg";
            cardAvg.Padding = new Padding(10, 6, 10, 6);
            cardAvg.Size = new Size(186, 88);
            cardAvg.TabIndex = 0;
            // 
            // cardAvgAccent
            // 
            cardAvgAccent.BackColor = Color.FromArgb(23, 162, 184);
            cardAvgAccent.Dock = DockStyle.Bottom;
            cardAvgAccent.Location = new Point(10, 76);
            cardAvgAccent.Name = "cardAvgAccent";
            cardAvgAccent.Size = new Size(164, 4);
            cardAvgAccent.TabIndex = 0;
            // 
            // cardAvgTitle
            // 
            cardAvgTitle.AutoSize = true;
            cardAvgTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cardAvgTitle.ForeColor = Color.FromArgb(14, 82, 52);
            cardAvgTitle.Location = new Point(10, 6);
            cardAvgTitle.Name = "cardAvgTitle";
            cardAvgTitle.Size = new Size(110, 19);
            cardAvgTitle.TabIndex = 1;
            cardAvgTitle.Text = "متوسط التكلفة";
            // 
            // cardAvgValue
            // 
            cardAvgValue.AutoSize = true;
            cardAvgValue.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cardAvgValue.ForeColor = Color.FromArgb(14, 82, 52);
            cardAvgValue.Location = new Point(10, 34);
            cardAvgValue.Name = "cardAvgValue";
            cardAvgValue.Size = new Size(62, 30);
            cardAvgValue.TabIndex = 2;
            cardAvgValue.Text = "0.00";
            // 
            // cardLast
            // 
            cardLast.BackColor = Color.White;
            cardLast.BorderStyle = BorderStyle.FixedSingle;
            cardLast.Controls.Add(cardLastAccent);
            cardLast.Controls.Add(cardLastTitle);
            cardLast.Controls.Add(cardLastValue);
            cardLast.Location = new Point(274, 24);
            cardLast.Margin = new Padding(8, 6, 8, 6);
            cardLast.Name = "cardLast";
            cardLast.Padding = new Padding(10, 6, 10, 6);
            cardLast.Size = new Size(186, 88);
            cardLast.TabIndex = 1;
            // 
            // cardLastAccent
            // 
            cardLastAccent.BackColor = Color.FromArgb(14, 165, 103);
            cardLastAccent.Dock = DockStyle.Bottom;
            cardLastAccent.Location = new Point(10, 76);
            cardLastAccent.Name = "cardLastAccent";
            cardLastAccent.Size = new Size(164, 4);
            cardLastAccent.TabIndex = 0;
            // 
            // cardLastTitle
            // 
            cardLastTitle.AutoSize = true;
            cardLastTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cardLastTitle.ForeColor = Color.FromArgb(14, 82, 52);
            cardLastTitle.Location = new Point(10, 6);
            cardLastTitle.Name = "cardLastTitle";
            cardLastTitle.Size = new Size(110, 19);
            cardLastTitle.TabIndex = 1;
            cardLastTitle.Text = "آخر سعر شراء";
            // 
            // cardLastValue
            // 
            cardLastValue.AutoSize = true;
            cardLastValue.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cardLastValue.ForeColor = Color.FromArgb(14, 82, 52);
            cardLastValue.Location = new Point(10, 34);
            cardLastValue.Name = "cardLastValue";
            cardLastValue.Size = new Size(62, 30);
            cardLastValue.TabIndex = 2;
            cardLastValue.Text = "0.00";
            // 
            // cardMin
            // 
            cardMin.BackColor = Color.White;
            cardMin.BorderStyle = BorderStyle.FixedSingle;
            cardMin.Controls.Add(cardMinAccent);
            cardMin.Controls.Add(cardMinTitle);
            cardMin.Controls.Add(cardMinValue);
            cardMin.Location = new Point(72, 24);
            cardMin.Margin = new Padding(8, 6, 8, 6);
            cardMin.Name = "cardMin";
            cardMin.Padding = new Padding(10, 6, 10, 6);
            cardMin.Size = new Size(186, 88);
            cardMin.TabIndex = 2;
            // 
            // cardMinAccent
            // 
            cardMinAccent.BackColor = Color.FromArgb(108, 117, 125);
            cardMinAccent.Dock = DockStyle.Bottom;
            cardMinAccent.Location = new Point(10, 76);
            cardMinAccent.Name = "cardMinAccent";
            cardMinAccent.Size = new Size(164, 4);
            cardMinAccent.TabIndex = 0;
            // 
            // cardMinTitle
            // 
            cardMinTitle.AutoSize = true;
            cardMinTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cardMinTitle.ForeColor = Color.FromArgb(14, 82, 52);
            cardMinTitle.Location = new Point(10, 6);
            cardMinTitle.Name = "cardMinTitle";
            cardMinTitle.Size = new Size(110, 19);
            cardMinTitle.TabIndex = 1;
            cardMinTitle.Text = "أدنى سعر شراء";
            // 
            // cardMinValue
            // 
            cardMinValue.AutoSize = true;
            cardMinValue.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cardMinValue.ForeColor = Color.FromArgb(14, 82, 52);
            cardMinValue.Location = new Point(10, 34);
            cardMinValue.Name = "cardMinValue";
            cardMinValue.Size = new Size(62, 30);
            cardMinValue.TabIndex = 2;
            cardMinValue.Text = "0.00";
            // 
            // gbHistory
            // 
            gbHistory.BackColor = Color.FromArgb(235, 245, 235);
            gbHistory.Controls.Add(dgvHistory);
            gbHistory.Controls.Add(emptyState);
            gbHistory.Dock = DockStyle.Fill;
            gbHistory.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbHistory.ForeColor = Color.FromArgb(14, 82, 52);
            gbHistory.Location = new Point(0, 302);
            gbHistory.Name = "gbHistory";
            gbHistory.Padding = new Padding(10, 4, 10, 8);
            gbHistory.Size = new Size(700, 280);
            gbHistory.TabIndex = 3;
            gbHistory.TabStop = false;
            gbHistory.Text = "سجل المشتريات السابقة";
            // 
            // dgvHistory
            // 
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AllowUserToResizeRows = false;
            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistory.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvHistory.BorderStyle = BorderStyle.None;
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHistory.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvHistory.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvHistory.ColumnHeadersHeight = 45;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHistory.ContextMenuStrip = cmsHistory;
            dgvHistory.DefaultCellStyle.BackColor = Color.White;
            dgvHistory.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvHistory.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvHistory.Dock = DockStyle.Fill;
            dgvHistory.EnableHeadersVisualStyles = false;
            dgvHistory.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvHistory.GridColor = Color.FromArgb(205, 225, 205);
            dgvHistory.Location = new Point(10, 20);
            dgvHistory.Margin = new Padding(0);
            dgvHistory.MultiSelect = false;
            dgvHistory.Name = "dgvHistory";
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.RowTemplate.Height = 32;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.Size = new Size(680, 252);
            dgvHistory.TabIndex = 0;
            dgvHistory.TabStop = false;
            dgvHistory.CellDoubleClick += dgvHistory_CellDoubleClick;
            dgvHistory.CellFormatting += dgvHistory_CellFormatting;
            // 
            // emptyState
            // 
            emptyState.BackColor = Color.Transparent;
            emptyState.Dock = DockStyle.Fill;
            emptyState.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            emptyState.ForeColor = Color.FromArgb(14, 82, 52);
            emptyState.Location = new Point(10, 20);
            emptyState.Name = "emptyState";
            emptyState.Size = new Size(680, 252);
            emptyState.TabIndex = 1;
            emptyState.Text = "لا توجد مشتريات سابقة مسجّلة لهذا الصنف";
            emptyState.TextAlign = ContentAlignment.MiddleCenter;
            emptyState.Visible = false;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(btnClose);
            pnlFooter.Controls.Add(lblRecordsCount);
            pnlFooter.Controls.Add(lblRecordsCaption);
            pnlFooter.Controls.Add(lblFooterLine);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 582);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(700, 58);
            pnlFooter.TabIndex = 4;
            // 
            // lblFooterLine
            // 
            lblFooterLine.BackColor = Color.FromArgb(205, 225, 205);
            lblFooterLine.Dock = DockStyle.Top;
            lblFooterLine.Location = new Point(0, 0);
            lblFooterLine.Name = "lblFooterLine";
            lblFooterLine.Size = new Size(700, 2);
            lblFooterLine.TabIndex = 3;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(14, 11);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 38);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblRecordsCaption
            // 
            lblRecordsCaption.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCaption.AutoSize = true;
            lblRecordsCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCaption.Location = new Point(452, 18);
            lblRecordsCaption.Name = "lblRecordsCaption";
            lblRecordsCaption.Size = new Size(126, 21);
            lblRecordsCaption.TabIndex = 1;
            lblRecordsCaption.Text = "عدد عمليات الشراء:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCount.Location = new Point(590, 18);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(24, 21);
            lblRecordsCount.TabIndex = 2;
            lblRecordsCount.Text = "0";
            // 
            // frmPurchaseItemInfo
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnClose;
            ClientSize = new Size(700, 640);
            Controls.Add(gbHistory);
            Controls.Add(pnlFooter);
            Controls.Add(gbCosts);
            Controls.Add(gbProductInfo);
            Controls.Add(pnlTitle);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmPurchaseItemInfo";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterParent;
            Text = "معلومات صنف";
            cmsHistory.ResumeLayout(false);
            pnlTitle.ResumeLayout(false);
            gbProductInfo.ResumeLayout(false);
            tblInfo.ResumeLayout(false);
            gbCosts.ResumeLayout(false);
            flowCostCards.ResumeLayout(false);
            cardAvg.ResumeLayout(false);
            cardAvg.PerformLayout();
            cardLast.ResumeLayout(false);
            cardLast.PerformLayout();
            cardMin.ResumeLayout(false);
            cardMin.PerformLayout();
            gbHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            pnlFooter.ResumeLayout(false);
            pnlFooter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlTitle;
        private Label lblTitle;
        private GroupBox gbProductInfo;
        private TableLayoutPanel tblInfo;
        private Label lblProductName;
        private Label lblBarcodeCaption;
        private Label lblCategoryCaption;
        private Label lblStockCaption;
        private Label lblBarcodeValue;
        private Label lblCategoryValue;
        private Label lblStockValue;
        private GroupBox gbCosts;
        private FlowLayoutPanel flowCostCards;
        private Panel cardAvg;
        private Label cardAvgAccent;
        private Label cardAvgTitle;
        private Label cardAvgValue;
        private Panel cardLast;
        private Label cardLastAccent;
        private Label cardLastTitle;
        private Label cardLastValue;
        private Panel cardMin;
        private Label cardMinAccent;
        private Label cardMinTitle;
        private Label cardMinValue;
        private GroupBox gbHistory;
        private DataGridView dgvHistory;
        private Label emptyState;
        private Panel pnlFooter;
        private Label lblFooterLine;
        private Button btnClose;
        private Label lblRecordsCaption;
        private Label lblRecordsCount;
        private ContextMenuStrip cmsHistory;
        private ToolStripMenuItem tsmiExport;
        private ToolStripMenuItem tsmiRefresh;
        private ToolTip toolTip;
    }
}
