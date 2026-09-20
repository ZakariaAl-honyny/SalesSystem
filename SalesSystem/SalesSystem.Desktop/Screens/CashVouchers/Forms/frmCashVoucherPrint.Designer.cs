namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

partial class frmCashVoucherPrint
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        pnlHeader = new Panel();
        lblMockBadge = new Label();
        lblTitle = new Label();
        lblAccent = new Label();
        pnlPreviewCard = new Panel();
        pnlCardHeader = new Panel();
        lblStoreName = new Label();
        lblVoucherTitle = new Label();
        tblFields = new TableLayoutPanel();
        lblVoucherNoValue = new Label();
        lblVoucherNoLabel = new Label();
        lblDateValue = new Label();
        lblDateLabel = new Label();
        lblAccountValue = new Label();
        lblAccountLabel = new Label();
        lblPartyValue = new Label();
        lblPartyLabel = new Label();
        lblAmountValue = new Label();
        lblAmountLabel = new Label();
        lblTafqitValue = new Label();
        lblTafqitLabel = new Label();
        lblNotesValue = new Label();
        lblNotesLabel = new Label();
        pnlDashedSeparator = new Panel();
        lblThanks = new Label();
        pnlFooter = new Panel();
        btnClose = new Button();
        btnPrint = new Button();
        lblFooterLine = new Label();
        pnlHeader.SuspendLayout();
        pnlPreviewCard.SuspendLayout();
        pnlCardHeader.SuspendLayout();
        tblFields.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblAccent);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(420, 46);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(420, 42);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "طباعة سند";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(296, 13);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(112, 21);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock)";
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(46, 204, 113);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 3;
        // 
        // pnlPreviewCard
        // 
        pnlPreviewCard.BackColor = Color.White;
        pnlPreviewCard.BorderStyle = BorderStyle.FixedSingle;
        pnlPreviewCard.Controls.Add(tblFields);
        pnlPreviewCard.Controls.Add(pnlCardHeader);
        pnlPreviewCard.Controls.Add(pnlDashedSeparator);
        pnlPreviewCard.Controls.Add(lblThanks);
        pnlPreviewCard.Location = new Point(20, 56);
        pnlPreviewCard.Name = "pnlPreviewCard";
        pnlPreviewCard.RightToLeft = RightToLeft.Yes;
        pnlPreviewCard.Size = new Size(380, 390);
        pnlPreviewCard.TabIndex = 1;
        // 
        // pnlCardHeader
        // 
        pnlCardHeader.BackColor = Color.White;
        pnlCardHeader.Controls.Add(lblVoucherTitle);
        pnlCardHeader.Controls.Add(lblStoreName);
        pnlCardHeader.Dock = DockStyle.Top;
        pnlCardHeader.Name = "pnlCardHeader";
        pnlCardHeader.Size = new Size(378, 74);
        pnlCardHeader.TabIndex = 0;
        // 
        // lblStoreName
        // 
        lblStoreName.Dock = DockStyle.Top;
        lblStoreName.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStoreName.ForeColor = Color.FromArgb(23, 87, 129);
        lblStoreName.Location = new Point(0, 0);
        lblStoreName.Name = "lblStoreName";
        lblStoreName.Size = new Size(378, 30);
        lblStoreName.TabIndex = 0;
        lblStoreName.Text = "متجر النور التجاري";
        lblStoreName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblVoucherTitle
        // 
        lblVoucherTitle.Dock = DockStyle.Top;
        lblVoucherTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblVoucherTitle.ForeColor = Color.FromArgb(46, 204, 113);
        lblVoucherTitle.Location = new Point(0, 30);
        lblVoucherTitle.Name = "lblVoucherTitle";
        lblVoucherTitle.Size = new Size(378, 40);
        lblVoucherTitle.TabIndex = 1;
        lblVoucherTitle.Text = "سند قبض";
        lblVoucherTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tblFields
        // 
        tblFields.BackColor = Color.White;
        tblFields.ColumnCount = 2;
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblFields.Controls.Add(lblVoucherNoValue, 1, 0);
        tblFields.Controls.Add(lblVoucherNoLabel, 0, 0);
        tblFields.Controls.Add(lblDateValue, 1, 1);
        tblFields.Controls.Add(lblDateLabel, 0, 1);
        tblFields.Controls.Add(lblAccountValue, 1, 2);
        tblFields.Controls.Add(lblAccountLabel, 0, 2);
        tblFields.Controls.Add(lblPartyValue, 1, 3);
        tblFields.Controls.Add(lblPartyLabel, 0, 3);
        tblFields.Controls.Add(lblAmountValue, 1, 4);
        tblFields.Controls.Add(lblAmountLabel, 0, 4);
        tblFields.Controls.Add(lblTafqitValue, 1, 5);
        tblFields.Controls.Add(lblTafqitLabel, 0, 5);
        tblFields.Controls.Add(lblNotesValue, 1, 6);
        tblFields.Controls.Add(lblNotesLabel, 0, 6);
        tblFields.Dock = DockStyle.Top;
        tblFields.Location = new Point(0, 74);
        tblFields.Name = "tblFields";
        tblFields.Padding = new Padding(12, 4, 12, 4);
        tblFields.RightToLeft = RightToLeft.Yes;
        tblFields.RowCount = 7;
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
        tblFields.Size = new Size(378, 260);
        tblFields.TabIndex = 1;
        // 
        // lblVoucherNoLabel
        // 
        lblVoucherNoLabel.AutoSize = true;
        lblVoucherNoLabel.Dock = DockStyle.Fill;
        lblVoucherNoLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblVoucherNoLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblVoucherNoLabel.Location = new Point(15, 7);
        lblVoucherNoLabel.Name = "lblVoucherNoLabel";
        lblVoucherNoLabel.Size = new Size(104, 20);
        lblVoucherNoLabel.TabIndex = 0;
        lblVoucherNoLabel.Text = "رقم السند";
        lblVoucherNoLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblVoucherNoValue
        // 
        lblVoucherNoValue.AutoSize = true;
        lblVoucherNoValue.Dock = DockStyle.Fill;
        lblVoucherNoValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblVoucherNoValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblVoucherNoValue.Location = new Point(125, 7);
        lblVoucherNoValue.Name = "lblVoucherNoValue";
        lblVoucherNoValue.Size = new Size(238, 20);
        lblVoucherNoValue.TabIndex = 1;
        lblVoucherNoValue.Text = "1003";
        lblVoucherNoValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDateLabel
        // 
        lblDateLabel.AutoSize = true;
        lblDateLabel.Dock = DockStyle.Fill;
        lblDateLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDateLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblDateLabel.Location = new Point(15, 33);
        lblDateLabel.Name = "lblDateLabel";
        lblDateLabel.Size = new Size(104, 20);
        lblDateLabel.TabIndex = 2;
        lblDateLabel.Text = "التاريخ";
        lblDateLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDateValue
        // 
        lblDateValue.AutoSize = true;
        lblDateValue.Dock = DockStyle.Fill;
        lblDateValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblDateValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblDateValue.Location = new Point(125, 33);
        lblDateValue.Name = "lblDateValue";
        lblDateValue.Size = new Size(238, 20);
        lblDateValue.TabIndex = 3;
        lblDateValue.Text = "15/09/2026";
        lblDateValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAccountLabel
        // 
        lblAccountLabel.AutoSize = true;
        lblAccountLabel.Dock = DockStyle.Fill;
        lblAccountLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblAccountLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblAccountLabel.Location = new Point(15, 59);
        lblAccountLabel.Name = "lblAccountLabel";
        lblAccountLabel.Size = new Size(104, 20);
        lblAccountLabel.TabIndex = 4;
        lblAccountLabel.Text = "الحساب";
        lblAccountLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblAccountValue
        // 
        lblAccountValue.AutoEllipsis = true;
        lblAccountValue.Dock = DockStyle.Fill;
        lblAccountValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblAccountValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblAccountValue.Location = new Point(125, 59);
        lblAccountValue.Name = "lblAccountValue";
        lblAccountValue.Size = new Size(238, 20);
        lblAccountValue.TabIndex = 5;
        lblAccountValue.Text = "الصندوق الرئيسي (111)";
        lblAccountValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblPartyLabel
        // 
        lblPartyLabel.AutoSize = true;
        lblPartyLabel.Dock = DockStyle.Fill;
        lblPartyLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPartyLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblPartyLabel.Location = new Point(15, 85);
        lblPartyLabel.Name = "lblPartyLabel";
        lblPartyLabel.Size = new Size(104, 20);
        lblPartyLabel.TabIndex = 6;
        lblPartyLabel.Text = "الجهة";
        lblPartyLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblPartyValue
        // 
        lblPartyValue.AutoEllipsis = true;
        lblPartyValue.Dock = DockStyle.Fill;
        lblPartyValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblPartyValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblPartyValue.Location = new Point(125, 85);
        lblPartyValue.Name = "lblPartyValue";
        lblPartyValue.Size = new Size(238, 20);
        lblPartyValue.TabIndex = 7;
        lblPartyValue.Text = "أحمد محمد";
        lblPartyValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblAmountLabel
        // 
        lblAmountLabel.AutoSize = true;
        lblAmountLabel.Dock = DockStyle.Fill;
        lblAmountLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblAmountLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblAmountLabel.Location = new Point(15, 111);
        lblAmountLabel.Name = "lblAmountLabel";
        lblAmountLabel.Size = new Size(104, 42);
        lblAmountLabel.TabIndex = 8;
        lblAmountLabel.Text = "المبلغ";
        lblAmountLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblAmountValue
        // 
        lblAmountValue.BackColor = Color.FromArgb(245, 250, 247);
        lblAmountValue.BorderStyle = BorderStyle.FixedSingle;
        lblAmountValue.Dock = DockStyle.Fill;
        lblAmountValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblAmountValue.ForeColor = Color.FromArgb(46, 204, 113);
        lblAmountValue.Location = new Point(125, 111);
        lblAmountValue.Name = "lblAmountValue";
        lblAmountValue.Size = new Size(238, 42);
        lblAmountValue.TabIndex = 9;
        lblAmountValue.Text = "150,000 ريال يمني";
        lblAmountValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTafqitLabel
        // 
        lblTafqitLabel.AutoSize = true;
        lblTafqitLabel.Dock = DockStyle.Fill;
        lblTafqitLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTafqitLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTafqitLabel.Location = new Point(15, 159);
        lblTafqitLabel.Name = "lblTafqitLabel";
        lblTafqitLabel.Size = new Size(104, 26);
        lblTafqitLabel.TabIndex = 10;
        lblTafqitLabel.Text = "التفقيط";
        lblTafqitLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTafqitValue
        // 
        lblTafqitValue.AutoEllipsis = true;
        lblTafqitValue.Dock = DockStyle.Fill;
        lblTafqitValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTafqitValue.ForeColor = Color.FromArgb(46, 204, 113);
        lblTafqitValue.Location = new Point(125, 159);
        lblTafqitValue.Name = "lblTafqitValue";
        lblTafqitValue.Size = new Size(238, 26);
        lblTafqitValue.TabIndex = 11;
        lblTafqitValue.Text = "مائة وخمسون ألف ريال يمني فقط";
        lblTafqitValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNotesLabel
        // 
        lblNotesLabel.AutoSize = true;
        lblNotesLabel.Dock = DockStyle.Fill;
        lblNotesLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNotesLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblNotesLabel.Location = new Point(15, 191);
        lblNotesLabel.Name = "lblNotesLabel";
        lblNotesLabel.Size = new Size(104, 70);
        lblNotesLabel.TabIndex = 12;
        lblNotesLabel.Text = "البيان";
        lblNotesLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNotesValue
        // 
        lblNotesValue.Dock = DockStyle.Fill;
        lblNotesValue.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblNotesValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblNotesValue.Location = new Point(125, 191);
        lblNotesValue.Name = "lblNotesValue";
        lblNotesValue.Size = new Size(238, 70);
        lblNotesValue.TabIndex = 13;
        lblNotesValue.Text = "سداد دفعة نقدية من العميل أحمد محمد عن فاتورة مبيعات رقم INV-2026-0142.";
        lblNotesValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlDashedSeparator
        // 
        pnlDashedSeparator.BackColor = Color.White;
        pnlDashedSeparator.Dock = DockStyle.Top;
        pnlDashedSeparator.Location = new Point(0, 334);
        pnlDashedSeparator.Name = "pnlDashedSeparator";
        pnlDashedSeparator.Size = new Size(378, 14);
        pnlDashedSeparator.TabIndex = 2;
        pnlDashedSeparator.Paint += pnlDashedSeparator_Paint;
        // 
        // lblThanks
        // 
        lblThanks.BackColor = Color.White;
        lblThanks.Dock = DockStyle.Bottom;
        lblThanks.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblThanks.ForeColor = Color.FromArgb(108, 117, 125);
        lblThanks.Location = new Point(0, 354);
        lblThanks.Name = "lblThanks";
        lblThanks.Size = new Size(378, 34);
        lblThanks.TabIndex = 3;
        lblThanks.Text = "شكراً لتعاملكم معنا";
        lblThanks.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(btnPrint);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(420, 66);
        pnlFooter.TabIndex = 2;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 2;
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.TabIndex = 0;
        // 
        // btnPrint
        // 
        btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPrint.BackColor = Color.FromArgb(25, 135, 84);
        btnPrint.FlatAppearance.BorderSize = 0;
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnPrint.ForeColor = Color.White;
        btnPrint.Location = new Point(268, 12);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(132, 42);
        btnPrint.TabIndex = 1;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = false;
        btnPrint.Click += btnPrint_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnClose.FlatAppearance.BorderSize = 1;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.FromArgb(52, 73, 94);
        btnClose.Location = new Point(12, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(100, 42);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmCashVoucherPrint
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(420, 560);
        Controls.Add(pnlFooter);
        Controls.Add(pnlPreviewCard);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCashVoucherPrint";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طباعة سند";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlPreviewCard.ResumeLayout(false);
        pnlCardHeader.ResumeLayout(false);
        tblFields.ResumeLayout(false);
        tblFields.PerformLayout();
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblTitle;
    private Label lblMockBadge;
    private Label lblAccent;
    private Panel pnlPreviewCard;
    private Panel pnlCardHeader;
    private Label lblStoreName;
    private Label lblVoucherTitle;
    private TableLayoutPanel tblFields;
    private Label lblVoucherNoLabel;
    private Label lblVoucherNoValue;
    private Label lblDateLabel;
    private Label lblDateValue;
    private Label lblAccountLabel;
    private Label lblAccountValue;
    private Label lblPartyLabel;
    private Label lblPartyValue;
    private Label lblAmountLabel;
    private Label lblAmountValue;
    private Label lblTafqitLabel;
    private Label lblTafqitValue;
    private Label lblNotesLabel;
    private Label lblNotesValue;
    private Panel pnlDashedSeparator;
    private Label lblThanks;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnPrint;
    private Button btnClose;
}