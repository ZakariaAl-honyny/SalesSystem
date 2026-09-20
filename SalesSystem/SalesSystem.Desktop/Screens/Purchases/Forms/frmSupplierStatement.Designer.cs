namespace SalesSystem.Desktop.Screens.Purchases.Forms;

partial class frmSupplierStatement
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
        pnlHeader = new Panel();
        lblMockBadge = new Label();
        lblMode = new Label();
        pnlSupplierInfo = new Panel();
        flowSupplier = new FlowLayoutPanel();
        lblSupplierCaption = new Label();
        lblSupplierName = new Label();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        pnlStats = new Panel();
        flowCards = new FlowLayoutPanel();
        cardPurchases = new Panel();
        cardPurchasesAccent = new Label();
        cardPurchasesTitle = new Label();
        cardPurchasesValue = new Label();
        cardPayments = new Panel();
        cardPaymentsAccent = new Label();
        cardPaymentsTitle = new Label();
        cardPaymentsValue = new Label();
        cardBalance = new Panel();
        cardBalanceAccent = new Label();
        cardBalanceTitle = new Label();
        cardBalanceValue = new Label();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnClose = new Button();
        label1 = new Label();
        lblRecordsCount = new Label();
        dgvStatement = new DataGridView();
        pnlHeader.SuspendLayout();
        pnlSupplierInfo.SuspendLayout();
        flowSupplier.SuspendLayout();
        pnlStats.SuspendLayout();
        flowCards.SuspendLayout();
        cardPurchases.SuspendLayout();
        cardPayments.SuspendLayout();
        cardBalance.SuspendLayout();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStatement).BeginInit();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(720, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(14, 19);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(248, 22);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(260, 8);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(200, 40);
        lblMode.TabIndex = 1;
        lblMode.Text = "كشف حساب مورد";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlSupplierInfo
        // 
        pnlSupplierInfo.BackColor = Color.White;
        pnlSupplierInfo.Controls.Add(flowSupplier);
        pnlSupplierInfo.Dock = DockStyle.Top;
        pnlSupplierInfo.Name = "pnlSupplierInfo";
        pnlSupplierInfo.Size = new Size(720, 54);
        pnlSupplierInfo.TabIndex = 1;
        // 
        // flowSupplier
        // 
        flowSupplier.Controls.Add(lblSupplierCaption);
        flowSupplier.Controls.Add(lblSupplierName);
        flowSupplier.Controls.Add(lblFrom);
        flowSupplier.Controls.Add(dtpFrom);
        flowSupplier.Controls.Add(lblTo);
        flowSupplier.Controls.Add(dtpTo);
        flowSupplier.Dock = DockStyle.Fill;
        flowSupplier.FlowDirection = FlowDirection.RightToLeft;
        flowSupplier.Name = "flowSupplier";
        flowSupplier.Padding = new Padding(10, 2, 10, 2);
        flowSupplier.WrapContents = false;
        // 
        // lblSupplierCaption
        // 
        lblSupplierCaption.AutoSize = true;
        lblSupplierCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSupplierCaption.ForeColor = Color.FromArgb(52, 73, 94);
        lblSupplierCaption.Location = new Point(3, 15);
        lblSupplierCaption.Margin = new Padding(4, 12, 2, 12);
        lblSupplierCaption.Name = "lblSupplierCaption";
        lblSupplierCaption.Size = new Size(55, 24);
        lblSupplierCaption.TabIndex = 0;
        lblSupplierCaption.Text = "المورد:";
        // 
        // lblSupplierName
        // 
        lblSupplierName.AutoSize = true;
        lblSupplierName.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSupplierName.ForeColor = Color.FromArgb(41, 128, 185);
        lblSupplierName.Location = new Point(74, 15);
        lblSupplierName.Margin = new Padding(20, 12, 2, 12);
        lblSupplierName.Name = "lblSupplierName";
        lblSupplierName.Size = new Size(163, 24);
        lblSupplierName.TabIndex = 1;
        lblSupplierName.Text = "شركة النور للتجارة";
        // 
        // lblFrom
        // 
        lblFrom.AutoSize = true;
        lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFrom.ForeColor = Color.FromArgb(52, 73, 94);
        lblFrom.Location = new Point(253, 15);
        lblFrom.Margin = new Padding(20, 12, 2, 12);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(42, 24);
        lblFrom.TabIndex = 2;
        lblFrom.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(309, 12);
        dtpFrom.Margin = new Padding(4, 12, 2, 12);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(120, 30);
        dtpFrom.TabIndex = 3;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.AutoSize = true;
        lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTo.ForeColor = Color.FromArgb(52, 73, 94);
        lblTo.Location = new Point(445, 15);
        lblTo.Margin = new Padding(16, 12, 2, 12);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(40, 24);
        lblTo.TabIndex = 4;
        lblTo.Text = "إلى:";
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(499, 12);
        dtpTo.Margin = new Padding(4, 12, 2, 12);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(120, 30);
        dtpTo.TabIndex = 5;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // pnlStats
        // 
        pnlStats.BackColor = Color.WhiteSmoke;
        pnlStats.Controls.Add(flowCards);
        pnlStats.Dock = DockStyle.Top;
        pnlStats.Name = "pnlStats";
        pnlStats.Size = new Size(720, 122);
        pnlStats.TabIndex = 2;
        // 
        // flowCards
        // 
        flowCards.Controls.Add(cardPurchases);
        flowCards.Controls.Add(cardPayments);
        flowCards.Controls.Add(cardBalance);
        flowCards.Dock = DockStyle.Fill;
        flowCards.FlowDirection = FlowDirection.RightToLeft;
        flowCards.Name = "flowCards";
        flowCards.Padding = new Padding(10, 6, 10, 6);
        // 
        // cardPurchases
        // 
        cardPurchases.BackColor = Color.White;
        cardPurchases.Controls.Add(cardPurchasesAccent);
        cardPurchases.Controls.Add(cardPurchasesTitle);
        cardPurchases.Controls.Add(cardPurchasesValue);
        cardPurchases.Margin = new Padding(12, 8, 12, 8);
        cardPurchases.Name = "cardPurchases";
        cardPurchases.Padding = new Padding(12, 6, 12, 6);
        cardPurchases.Size = new Size(200, 82);
        cardPurchases.TabIndex = 0;
        // 
        // cardPurchasesAccent
        // 
        cardPurchasesAccent.BackColor = Color.FromArgb(41, 128, 185);
        cardPurchasesAccent.Dock = DockStyle.Bottom;
        cardPurchasesAccent.Height = 4;
        cardPurchasesAccent.Name = "cardPurchasesAccent";
        cardPurchasesAccent.TabIndex = 0;
        // 
        // cardPurchasesTitle
        // 
        cardPurchasesTitle.AutoSize = true;
        cardPurchasesTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardPurchasesTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardPurchasesTitle.Location = new Point(12, 6);
        cardPurchasesTitle.Name = "cardPurchasesTitle";
        cardPurchasesTitle.Size = new Size(176, 24);
        cardPurchasesTitle.TabIndex = 1;
        cardPurchasesTitle.Text = "إجمالي المشتريات (مدين)";
        // 
        // cardPurchasesValue
        // 
        cardPurchasesValue.AutoSize = true;
        cardPurchasesValue.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardPurchasesValue.ForeColor = Color.FromArgb(41, 128, 185);
        cardPurchasesValue.Location = new Point(12, 34);
        cardPurchasesValue.Name = "cardPurchasesValue";
        cardPurchasesValue.Size = new Size(78, 39);
        cardPurchasesValue.TabIndex = 2;
        cardPurchasesValue.Text = "0.00";
        // 
        // cardPayments
        // 
        cardPayments.BackColor = Color.White;
        cardPayments.Controls.Add(cardPaymentsAccent);
        cardPayments.Controls.Add(cardPaymentsTitle);
        cardPayments.Controls.Add(cardPaymentsValue);
        cardPayments.Margin = new Padding(12, 8, 12, 8);
        cardPayments.Name = "cardPayments";
        cardPayments.Padding = new Padding(12, 6, 12, 6);
        cardPayments.Size = new Size(200, 82);
        cardPayments.TabIndex = 1;
        // 
        // cardPaymentsAccent
        // 
        cardPaymentsAccent.BackColor = Color.FromArgb(39, 174, 96);
        cardPaymentsAccent.Dock = DockStyle.Bottom;
        cardPaymentsAccent.Height = 4;
        cardPaymentsAccent.Name = "cardPaymentsAccent";
        cardPaymentsAccent.TabIndex = 0;
        // 
        // cardPaymentsTitle
        // 
        cardPaymentsTitle.AutoSize = true;
        cardPaymentsTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardPaymentsTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardPaymentsTitle.Location = new Point(12, 6);
        cardPaymentsTitle.Name = "cardPaymentsTitle";
        cardPaymentsTitle.Size = new Size(176, 24);
        cardPaymentsTitle.TabIndex = 1;
        cardPaymentsTitle.Text = "إجمالي المدفوعات (دائن)";
        // 
        // cardPaymentsValue
        // 
        cardPaymentsValue.AutoSize = true;
        cardPaymentsValue.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardPaymentsValue.ForeColor = Color.FromArgb(39, 174, 96);
        cardPaymentsValue.Location = new Point(12, 34);
        cardPaymentsValue.Name = "cardPaymentsValue";
        cardPaymentsValue.Size = new Size(78, 39);
        cardPaymentsValue.TabIndex = 2;
        cardPaymentsValue.Text = "0.00";
        // 
        // cardBalance
        // 
        cardBalance.BackColor = Color.White;
        cardBalance.Controls.Add(cardBalanceAccent);
        cardBalance.Controls.Add(cardBalanceTitle);
        cardBalance.Controls.Add(cardBalanceValue);
        cardBalance.Margin = new Padding(12, 8, 12, 8);
        cardBalance.Name = "cardBalance";
        cardBalance.Padding = new Padding(12, 6, 12, 6);
        cardBalance.Size = new Size(200, 82);
        cardBalance.TabIndex = 2;
        // 
        // cardBalanceAccent
        // 
        cardBalanceAccent.BackColor = Color.FromArgb(243, 156, 18);
        cardBalanceAccent.Dock = DockStyle.Bottom;
        cardBalanceAccent.Height = 4;
        cardBalanceAccent.Name = "cardBalanceAccent";
        cardBalanceAccent.TabIndex = 0;
        // 
        // cardBalanceTitle
        // 
        cardBalanceTitle.AutoSize = true;
        cardBalanceTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardBalanceTitle.ForeColor = Color.FromArgb(108, 117, 125);
        cardBalanceTitle.Location = new Point(12, 6);
        cardBalanceTitle.Name = "cardBalanceTitle";
        cardBalanceTitle.Size = new Size(130, 24);
        cardBalanceTitle.TabIndex = 1;
        cardBalanceTitle.Text = "الرصيد المتبقي";
        // 
        // cardBalanceValue
        // 
        cardBalanceValue.AutoSize = true;
        cardBalanceValue.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardBalanceValue.ForeColor = Color.FromArgb(52, 73, 94);
        cardBalanceValue.Location = new Point(12, 34);
        cardBalanceValue.Name = "cardBalanceValue";
        cardBalanceValue.Size = new Size(78, 39);
        cardBalanceValue.TabIndex = 2;
        cardBalanceValue.Text = "0.00";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(label1);
        pnlFooter.Controls.Add(lblRecordsCount);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(720, 54);
        pnlFooter.TabIndex = 3;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 2;
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.TabIndex = 4;
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
        btnClose.Location = new Point(12, 8);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(110, 38);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.ForeColor = Color.FromArgb(108, 117, 125);
        label1.Location = new Point(205, 15);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(115, 24);
        label1.TabIndex = 2;
        label1.Text = "عدد الحركات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(52, 73, 94);
        lblRecordsCount.Location = new Point(425, 15);
        lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(30, 24);
        lblRecordsCount.TabIndex = 3;
        lblRecordsCount.Text = "0";
        // 
        // dgvStatement
        // 
        dgvStatement.AllowUserToAddRows = false;
        dgvStatement.AllowUserToDeleteRows = false;
        dgvStatement.AllowUserToResizeRows = false;
        dgvStatement.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStatement.BackgroundColor = Color.White;
        dgvStatement.BorderStyle = BorderStyle.None;
        dgvStatement.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvStatement.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvStatement.ColumnHeadersHeight = 44;
        dgvStatement.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvStatement.Dock = DockStyle.Fill;
        dgvStatement.EnableHeadersVisualStyles = false;
        dgvStatement.GridColor = Color.FromArgb(222, 226, 230);
        dgvStatement.MultiSelect = false;
        dgvStatement.Name = "dgvStatement";
        dgvStatement.ReadOnly = true;
        dgvStatement.RowHeadersVisible = false;
        dgvStatement.RowTemplate.Height = 32;
        dgvStatement.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStatement.Size = new Size(720, 314);
        dgvStatement.TabIndex = 4;
        dgvStatement.TabStop = false;
        dgvStatement.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 43, 54);
        dgvStatement.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvStatement.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dgvStatement.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvStatement.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(33, 43, 54);
        dgvStatement.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvStatement.DefaultCellStyle.BackColor = Color.White;
        dgvStatement.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
        dgvStatement.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dgvStatement.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
        dgvStatement.DefaultCellStyle.SelectionForeColor = Color.White;
        dgvStatement.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        dgvStatement.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        dgvStatement.CellFormatting += dgvStatement_CellFormatting;
        // 
        // frmSupplierStatement
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(720, 600);
        Controls.Add(dgvStatement);
        Controls.Add(pnlFooter);
        Controls.Add(pnlStats);
        Controls.Add(pnlSupplierInfo);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmSupplierStatement";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "كشف حساب مورد";
        Load += frmSupplierStatement_Load;
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlSupplierInfo.ResumeLayout(false);
        flowSupplier.ResumeLayout(false);
        flowSupplier.PerformLayout();
        pnlStats.ResumeLayout(false);
        flowCards.ResumeLayout(false);
        cardPurchases.ResumeLayout(false);
        cardPurchases.PerformLayout();
        cardPayments.ResumeLayout(false);
        cardPayments.PerformLayout();
        cardBalance.ResumeLayout(false);
        cardBalance.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvStatement).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMockBadge;
    private Label lblMode;
    private Panel pnlSupplierInfo;
    private FlowLayoutPanel flowSupplier;
    private Label lblSupplierCaption;
    private Label lblSupplierName;
    private Label lblFrom;
    private DateTimePicker dtpFrom;
    private Label lblTo;
    private DateTimePicker dtpTo;
    private Panel pnlStats;
    private FlowLayoutPanel flowCards;
    private Panel cardPurchases;
    private Label cardPurchasesAccent;
    private Label cardPurchasesTitle;
    private Label cardPurchasesValue;
    private Panel cardPayments;
    private Label cardPaymentsAccent;
    private Label cardPaymentsTitle;
    private Label cardPaymentsValue;
    private Panel cardBalance;
    private Label cardBalanceAccent;
    private Label cardBalanceTitle;
    private Label cardBalanceValue;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnClose;
    private Label label1;
    private Label lblRecordsCount;
    private DataGridView dgvStatement;
}