namespace SalesSystem.Desktop.Screens.Purchases.Forms;

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
        pnlTitle = new Panel();
        lblTitle = new Label();
        pnlProductInfo = new Panel();
        tblInfo = new TableLayoutPanel();
        lblProductName = new Label();
        lblBarcodeCaption = new Label();
        lblCategoryCaption = new Label();
        lblStockCaption = new Label();
        lblBarcodeValue = new Label();
        lblCategoryValue = new Label();
        lblStockValue = new Label();
        pnlCosts = new Panel();
        lblCostsCaption = new Label();
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
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnClose = new Button();
        lblRecordsCaption = new Label();
        lblRecordsCount = new Label();
        dgvHistory = new DataGridView();
        pnlTitle.SuspendLayout();
        pnlProductInfo.SuspendLayout();
        tblInfo.SuspendLayout();
        pnlCosts.SuspendLayout();
        flowCostCards.SuspendLayout();
        cardAvg.SuspendLayout();
        cardLast.SuspendLayout();
        cardMin.SuspendLayout();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
        SuspendLayout();
        // 
        // pnlTitle
        // 
        pnlTitle.Controls.Add(lblTitle);
        pnlTitle.Dock = DockStyle.Top;
        pnlTitle.Name = "pnlTitle";
        pnlTitle.Size = new Size(600, 46);
        pnlTitle.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.BackColor = Color.Firebrick;
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = Color.White;
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "معلومات صنف";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlProductInfo
        // 
        pnlProductInfo.BackColor = Color.White;
        pnlProductInfo.Controls.Add(tblInfo);
        pnlProductInfo.Dock = DockStyle.Top;
        pnlProductInfo.Name = "pnlProductInfo";
        pnlProductInfo.Padding = new Padding(14, 6, 14, 6);
        pnlProductInfo.Size = new Size(600, 84);
        pnlProductInfo.TabIndex = 1;
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
        tblInfo.Name = "tblInfo";
        tblInfo.RowCount = 3;
        tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        tblInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        // 
        // lblProductName
        // 
        lblProductName.ColumnSpan = 3;
        lblProductName.Dock = DockStyle.Fill;
        lblProductName.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblProductName.ForeColor = Color.Navy;
        lblProductName.Name = "lblProductName";
        lblProductName.Text = "—";
        lblProductName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblBarcodeCaption
        // 
        lblBarcodeCaption.Dock = DockStyle.Fill;
        lblBarcodeCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBarcodeCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblBarcodeCaption.Name = "lblBarcodeCaption";
        lblBarcodeCaption.Text = "الباركود:";
        lblBarcodeCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblCategoryCaption
        // 
        lblCategoryCaption.Dock = DockStyle.Fill;
        lblCategoryCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCategoryCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblCategoryCaption.Name = "lblCategoryCaption";
        lblCategoryCaption.Text = "التصنيف:";
        lblCategoryCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblStockCaption
        // 
        lblStockCaption.Dock = DockStyle.Fill;
        lblStockCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStockCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblStockCaption.Name = "lblStockCaption";
        lblStockCaption.Text = "الرصيد الحالي:";
        lblStockCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblBarcodeValue
        // 
        lblBarcodeValue.Dock = DockStyle.Fill;
        lblBarcodeValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBarcodeValue.ForeColor = Color.FromArgb(52, 73, 94);
        lblBarcodeValue.Name = "lblBarcodeValue";
        lblBarcodeValue.Text = "—";
        lblBarcodeValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblCategoryValue
        // 
        lblCategoryValue.Dock = DockStyle.Fill;
        lblCategoryValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCategoryValue.ForeColor = Color.FromArgb(52, 73, 94);
        lblCategoryValue.Name = "lblCategoryValue";
        lblCategoryValue.Text = "—";
        lblCategoryValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblStockValue
        // 
        lblStockValue.Dock = DockStyle.Fill;
        lblStockValue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStockValue.ForeColor = Color.ForestGreen;
        lblStockValue.Name = "lblStockValue";
        lblStockValue.Text = "—";
        lblStockValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlCosts
        // 
        pnlCosts.BackColor = Color.WhiteSmoke;
        pnlCosts.Controls.Add(lblCostsCaption);
        pnlCosts.Controls.Add(flowCostCards);
        pnlCosts.Dock = DockStyle.Top;
        pnlCosts.Name = "pnlCosts";
        pnlCosts.Size = new Size(600, 132);
        pnlCosts.TabIndex = 2;
        // 
        // lblCostsCaption
        // 
        lblCostsCaption.Dock = DockStyle.Top;
        lblCostsCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCostsCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblCostsCaption.Height = 26;
        lblCostsCaption.Name = "lblCostsCaption";
        lblCostsCaption.Text = "أسعار الشراء المقترحة";
        lblCostsCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // flowCostCards
        // 
        flowCostCards.Controls.Add(cardAvg);
        flowCostCards.Controls.Add(cardLast);
        flowCostCards.Controls.Add(cardMin);
        flowCostCards.Dock = DockStyle.Fill;
        flowCostCards.FlowDirection = FlowDirection.RightToLeft;
        flowCostCards.Name = "flowCostCards";
        flowCostCards.Padding = new Padding(10, 4, 10, 6);
        flowCostCards.WrapContents = false;
        // 
        // cardAvg
        // 
        cardAvg.BackColor = Color.White;
        cardAvg.Controls.Add(cardAvgAccent);
        cardAvg.Controls.Add(cardAvgTitle);
        cardAvg.Controls.Add(cardAvgValue);
        cardAvg.Margin = new Padding(8, 6, 8, 6);
        cardAvg.Name = "cardAvg";
        cardAvg.Padding = new Padding(10, 6, 10, 6);
        cardAvg.Size = new Size(158, 86);
        cardAvg.TabIndex = 0;
        // 
        // cardAvgAccent
        // 
        cardAvgAccent.BackColor = Color.FromArgb(41, 128, 185);
        cardAvgAccent.Dock = DockStyle.Bottom;
        cardAvgAccent.Height = 4;
        cardAvgAccent.Name = "cardAvgAccent";
        cardAvgAccent.TabIndex = 0;
        // 
        // cardAvgTitle
        // 
        cardAvgTitle.AutoSize = true;
        cardAvgTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardAvgTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardAvgTitle.Location = new Point(10, 6);
        cardAvgTitle.Name = "cardAvgTitle";
        cardAvgTitle.Size = new Size(120, 23);
        cardAvgTitle.TabIndex = 1;
        cardAvgTitle.Text = "متوسط التكلفة";
        // 
        // cardAvgValue
        // 
        cardAvgValue.AutoSize = true;
        cardAvgValue.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardAvgValue.ForeColor = Color.FromArgb(41, 128, 185);
        cardAvgValue.Location = new Point(10, 34);
        cardAvgValue.Name = "cardAvgValue";
        cardAvgValue.Size = new Size(65, 36);
        cardAvgValue.TabIndex = 2;
        cardAvgValue.Text = "0.00";
        // 
        // cardLast
        // 
        cardLast.BackColor = Color.White;
        cardLast.Controls.Add(cardLastAccent);
        cardLast.Controls.Add(cardLastTitle);
        cardLast.Controls.Add(cardLastValue);
        cardLast.Margin = new Padding(8, 6, 8, 6);
        cardLast.Name = "cardLast";
        cardLast.Padding = new Padding(10, 6, 10, 6);
        cardLast.Size = new Size(158, 86);
        cardLast.TabIndex = 1;
        // 
        // cardLastAccent
        // 
        cardLastAccent.BackColor = Color.FromArgb(39, 174, 96);
        cardLastAccent.Dock = DockStyle.Bottom;
        cardLastAccent.Height = 4;
        cardLastAccent.Name = "cardLastAccent";
        cardLastAccent.TabIndex = 0;
        // 
        // cardLastTitle
        // 
        cardLastTitle.AutoSize = true;
        cardLastTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardLastTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardLastTitle.Location = new Point(10, 6);
        cardLastTitle.Name = "cardLastTitle";
        cardLastTitle.Size = new Size(120, 23);
        cardLastTitle.TabIndex = 1;
        cardLastTitle.Text = "آخر سعر شراء";
        // 
        // cardLastValue
        // 
        cardLastValue.AutoSize = true;
        cardLastValue.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardLastValue.ForeColor = Color.FromArgb(39, 174, 96);
        cardLastValue.Location = new Point(10, 34);
        cardLastValue.Name = "cardLastValue";
        cardLastValue.Size = new Size(65, 36);
        cardLastValue.TabIndex = 2;
        cardLastValue.Text = "0.00";
        // 
        // cardMin
        // 
        cardMin.BackColor = Color.White;
        cardMin.Controls.Add(cardMinAccent);
        cardMin.Controls.Add(cardMinTitle);
        cardMin.Controls.Add(cardMinValue);
        cardMin.Margin = new Padding(8, 6, 8, 6);
        cardMin.Name = "cardMin";
        cardMin.Padding = new Padding(10, 6, 10, 6);
        cardMin.Size = new Size(158, 86);
        cardMin.TabIndex = 2;
        // 
        // cardMinAccent
        // 
        cardMinAccent.BackColor = Color.FromArgb(243, 156, 18);
        cardMinAccent.Dock = DockStyle.Bottom;
        cardMinAccent.Height = 4;
        cardMinAccent.Name = "cardMinAccent";
        cardMinAccent.TabIndex = 0;
        // 
        // cardMinTitle
        // 
        cardMinTitle.AutoSize = true;
        cardMinTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardMinTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardMinTitle.Location = new Point(10, 6);
        cardMinTitle.Name = "cardMinTitle";
        cardMinTitle.Size = new Size(120, 23);
        cardMinTitle.TabIndex = 1;
        cardMinTitle.Text = "أدنى سعر شراء";
        // 
        // cardMinValue
        // 
        cardMinValue.AutoSize = true;
        cardMinValue.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardMinValue.ForeColor = Color.FromArgb(243, 156, 18);
        cardMinValue.Location = new Point(10, 34);
        cardMinValue.Name = "cardMinValue";
        cardMinValue.Size = new Size(65, 36);
        cardMinValue.TabIndex = 2;
        cardMinValue.Text = "0.00";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(lblRecordsCaption);
        pnlFooter.Controls.Add(lblRecordsCount);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(600, 56);
        pnlFooter.TabIndex = 3;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 2;
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.TabIndex = 3;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnClose.FlatAppearance.BorderSize = 1;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.FromArgb(52, 73, 94);
        btnClose.Location = new Point(12, 9);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(110, 38);
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
        lblRecordsCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblRecordsCaption.Location = new Point(180, 16);
        lblRecordsCaption.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCaption.Name = "lblRecordsCaption";
        lblRecordsCaption.Size = new Size(158, 24);
        lblRecordsCaption.TabIndex = 1;
        lblRecordsCaption.Text = "عدد عمليات الشراء:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(52, 73, 94);
        lblRecordsCount.Location = new Point(400, 16);
        lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(30, 24);
        lblRecordsCount.TabIndex = 2;
        lblRecordsCount.Text = "0";
        // 
        // dgvHistory
        // 
        dgvHistory.AllowUserToAddRows = false;
        dgvHistory.AllowUserToDeleteRows = false;
        dgvHistory.AllowUserToResizeRows = false;
        dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvHistory.BackgroundColor = Color.White;
        dgvHistory.BorderStyle = BorderStyle.None;
        dgvHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvHistory.ColumnHeadersHeight = 44;
        dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvHistory.Dock = DockStyle.Fill;
        dgvHistory.EnableHeadersVisualStyles = false;
        dgvHistory.GridColor = Color.FromArgb(222, 226, 230);
        dgvHistory.MultiSelect = false;
        dgvHistory.Name = "dgvHistory";
        dgvHistory.ReadOnly = true;
        dgvHistory.RowHeadersVisible = false;
        dgvHistory.RowTemplate.Height = 32;
        dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvHistory.Size = new Size(600, 242);
        dgvHistory.TabIndex = 4;
        dgvHistory.TabStop = false;
        dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 43, 54);
        dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvHistory.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(33, 43, 54);
        dgvHistory.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvHistory.DefaultCellStyle.BackColor = Color.White;
        dgvHistory.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
        dgvHistory.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dgvHistory.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
        dgvHistory.DefaultCellStyle.SelectionForeColor = Color.White;
        dgvHistory.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        dgvHistory.CellFormatting += dgvHistory_CellFormatting;
        // 
        // frmPurchaseItemInfo
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(600, 560);
        Controls.Add(dgvHistory);
        Controls.Add(pnlFooter);
        Controls.Add(pnlCosts);
        Controls.Add(pnlProductInfo);
        Controls.Add(pnlTitle);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmPurchaseItemInfo";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "معلومات صنف";
        pnlTitle.ResumeLayout(false);
        pnlProductInfo.ResumeLayout(false);
        tblInfo.ResumeLayout(false);
        pnlCosts.ResumeLayout(false);
        flowCostCards.ResumeLayout(false);
        cardAvg.ResumeLayout(false);
        cardAvg.PerformLayout();
        cardLast.ResumeLayout(false);
        cardLast.PerformLayout();
        cardMin.ResumeLayout(false);
        cardMin.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlTitle;
    private Label lblTitle;
    private Panel pnlProductInfo;
    private TableLayoutPanel tblInfo;
    private Label lblProductName;
    private Label lblBarcodeCaption;
    private Label lblCategoryCaption;
    private Label lblStockCaption;
    private Label lblBarcodeValue;
    private Label lblCategoryValue;
    private Label lblStockValue;
    private Panel pnlCosts;
    private Label lblCostsCaption;
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
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnClose;
    private Label lblRecordsCaption;
    private Label lblRecordsCount;
    private DataGridView dgvHistory;
}