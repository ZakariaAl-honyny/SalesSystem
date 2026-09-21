namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmStocktakingPrintAdjustment
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
        lblAdjustmentTitle = new Label();
        lblStoreName = new Label();
        tblFields = new TableLayoutPanel();
        lblVoucherNoValue = new Label();
        lblVoucherNoLabel = new Label();
        lblDateValue = new Label();
        lblDateLabel = new Label();
        lblWarehouseValue = new Label();
        lblWarehouseLabel = new Label();
        lblReasonValue = new Label();
        lblReasonLabel = new Label();
        lblItemCountValue = new Label();
        lblItemCountLabel = new Label();
        lblTotalValue = new Label();
        lblTotalLabel = new Label();
        pnlDashedSeparator = new Panel();
        lblCardFooter = new Label();
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
        pnlHeader.Size = new Size(440, 46);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(440, 42);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "طباعة سند تسوية جرد";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(316, 13);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(112, 21);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock)";
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(108, 92, 231);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 3;
        // 
        // pnlPreviewCard
        // 
        pnlPreviewCard.BackColor = Color.White;
        pnlPreviewCard.BorderStyle = BorderStyle.FixedSingle;
        pnlPreviewCard.Controls.Add(pnlCardHeader);
        pnlPreviewCard.Controls.Add(tblFields);
        pnlPreviewCard.Controls.Add(pnlDashedSeparator);
        pnlPreviewCard.Controls.Add(lblCardFooter);
        pnlPreviewCard.Location = new Point(20, 56);
        pnlPreviewCard.Name = "pnlPreviewCard";
        pnlPreviewCard.RightToLeft = RightToLeft.Yes;
        pnlPreviewCard.Size = new Size(400, 386);
        pnlPreviewCard.TabIndex = 1;
        // 
        // pnlCardHeader
        // 
        pnlCardHeader.BackColor = Color.White;
        pnlCardHeader.Controls.Add(lblAdjustmentTitle);
        pnlCardHeader.Controls.Add(lblStoreName);
        pnlCardHeader.Location = new Point(0, 0);
        pnlCardHeader.Name = "pnlCardHeader";
        pnlCardHeader.Size = new Size(398, 74);
        pnlCardHeader.TabIndex = 0;
        // 
        // lblStoreName
        // 
        lblStoreName.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStoreName.ForeColor = Color.FromArgb(23, 87, 129);
        lblStoreName.Location = new Point(0, 0);
        lblStoreName.Name = "lblStoreName";
        lblStoreName.Size = new Size(398, 30);
        lblStoreName.TabIndex = 0;
        lblStoreName.Text = "متجر النور التجاري";
        lblStoreName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAdjustmentTitle
        // 
        lblAdjustmentTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblAdjustmentTitle.ForeColor = Color.FromArgb(108, 92, 231);
        lblAdjustmentTitle.Location = new Point(0, 30);
        lblAdjustmentTitle.Name = "lblAdjustmentTitle";
        lblAdjustmentTitle.Size = new Size(398, 40);
        lblAdjustmentTitle.TabIndex = 1;
        lblAdjustmentTitle.Text = "سند تسوية جرد";
        lblAdjustmentTitle.TextAlign = ContentAlignment.MiddleCenter;
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
        tblFields.Controls.Add(lblWarehouseValue, 1, 2);
        tblFields.Controls.Add(lblWarehouseLabel, 0, 2);
        tblFields.Controls.Add(lblReasonValue, 1, 3);
        tblFields.Controls.Add(lblReasonLabel, 0, 3);
        tblFields.Controls.Add(lblItemCountValue, 1, 4);
        tblFields.Controls.Add(lblItemCountLabel, 0, 4);
        tblFields.Controls.Add(lblTotalValue, 1, 5);
        tblFields.Controls.Add(lblTotalLabel, 0, 5);
        tblFields.Location = new Point(0, 74);
        tblFields.Name = "tblFields";
        tblFields.Padding = new Padding(12, 4, 12, 4);
        tblFields.RightToLeft = RightToLeft.Yes;
        tblFields.RowCount = 6;
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        tblFields.Size = new Size(398, 190);
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
        lblVoucherNoValue.AutoEllipsis = true;
        lblVoucherNoValue.AutoSize = true;
        lblVoucherNoValue.Dock = DockStyle.Fill;
        lblVoucherNoValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblVoucherNoValue.ForeColor = Color.FromArgb(108, 92, 231);
        lblVoucherNoValue.Location = new Point(125, 7);
        lblVoucherNoValue.Name = "lblVoucherNoValue";
        lblVoucherNoValue.Size = new Size(258, 20);
        lblVoucherNoValue.TabIndex = 1;
        lblVoucherNoValue.Text = "ADJ-2026-001";
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
        lblDateValue.Size = new Size(258, 20);
        lblDateValue.TabIndex = 3;
        lblDateValue.Text = "21/09/2026";
        lblDateValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblWarehouseLabel
        // 
        lblWarehouseLabel.AutoSize = true;
        lblWarehouseLabel.Dock = DockStyle.Fill;
        lblWarehouseLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblWarehouseLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblWarehouseLabel.Location = new Point(15, 59);
        lblWarehouseLabel.Name = "lblWarehouseLabel";
        lblWarehouseLabel.Size = new Size(104, 20);
        lblWarehouseLabel.TabIndex = 4;
        lblWarehouseLabel.Text = "المستودع";
        lblWarehouseLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblWarehouseValue
        // 
        lblWarehouseValue.AutoEllipsis = true;
        lblWarehouseValue.Dock = DockStyle.Fill;
        lblWarehouseValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblWarehouseValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblWarehouseValue.Location = new Point(125, 59);
        lblWarehouseValue.Name = "lblWarehouseValue";
        lblWarehouseValue.Size = new Size(258, 20);
        lblWarehouseValue.TabIndex = 5;
        lblWarehouseValue.Text = "المستودع الرئيسي";
        lblWarehouseValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblReasonLabel
        // 
        lblReasonLabel.AutoSize = true;
        lblReasonLabel.Dock = DockStyle.Fill;
        lblReasonLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblReasonLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblReasonLabel.Location = new Point(15, 85);
        lblReasonLabel.Name = "lblReasonLabel";
        lblReasonLabel.Size = new Size(104, 24);
        lblReasonLabel.TabIndex = 6;
        lblReasonLabel.Text = "سبب التسوية";
        lblReasonLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblReasonValue
        // 
        lblReasonValue.AutoEllipsis = true;
        lblReasonValue.Dock = DockStyle.Fill;
        lblReasonValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblReasonValue.ForeColor = Color.FromArgb(220, 53, 69);
        lblReasonValue.Location = new Point(125, 85);
        lblReasonValue.Name = "lblReasonValue";
        lblReasonValue.Size = new Size(258, 24);
        lblReasonValue.TabIndex = 7;
        lblReasonValue.Text = "تلف";
        lblReasonValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblItemCountLabel
        // 
        lblItemCountLabel.AutoSize = true;
        lblItemCountLabel.Dock = DockStyle.Fill;
        lblItemCountLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblItemCountLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblItemCountLabel.Location = new Point(15, 115);
        lblItemCountLabel.Name = "lblItemCountLabel";
        lblItemCountLabel.Size = new Size(104, 20);
        lblItemCountLabel.TabIndex = 8;
        lblItemCountLabel.Text = "عدد الأصناف";
        lblItemCountLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblItemCountValue
        // 
        lblItemCountValue.AutoEllipsis = true;
        lblItemCountValue.Dock = DockStyle.Fill;
        lblItemCountValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblItemCountValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblItemCountValue.Location = new Point(125, 115);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(258, 20);
        lblItemCountValue.TabIndex = 9;
        lblItemCountValue.Text = "4 أصناف";
        lblItemCountValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalLabel
        // 
        lblTotalLabel.AutoSize = true;
        lblTotalLabel.Dock = DockStyle.Fill;
        lblTotalLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTotalLabel.Location = new Point(15, 141);
        lblTotalLabel.Name = "lblTotalLabel";
        lblTotalLabel.Size = new Size(104, 42);
        lblTotalLabel.TabIndex = 10;
        lblTotalLabel.Text = "إجمالي قيمة التسوية";
        lblTotalLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTotalValue
        // 
        lblTotalValue.BackColor = Color.FromArgb(244, 242, 255);
        lblTotalValue.BorderStyle = BorderStyle.FixedSingle;
        lblTotalValue.Dock = DockStyle.Fill;
        lblTotalValue.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalValue.ForeColor = Color.FromArgb(108, 92, 231);
        lblTotalValue.Location = new Point(125, 141);
        lblTotalValue.Name = "lblTotalValue";
        lblTotalValue.Size = new Size(258, 42);
        lblTotalValue.TabIndex = 11;
        lblTotalValue.Text = "8,400 ريال يمني";
        lblTotalValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlDashedSeparator
        // 
        pnlDashedSeparator.BackColor = Color.White;
        pnlDashedSeparator.Location = new Point(0, 264);
        pnlDashedSeparator.Name = "pnlDashedSeparator";
        pnlDashedSeparator.Size = new Size(398, 14);
        pnlDashedSeparator.TabIndex = 2;
        pnlDashedSeparator.Paint += pnlDashedSeparator_Paint;
        // 
        // lblCardFooter
        // 
        lblCardFooter.BackColor = Color.White;
        lblCardFooter.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCardFooter.ForeColor = Color.FromArgb(108, 117, 125);
        lblCardFooter.Location = new Point(0, 278);
        lblCardFooter.Name = "lblCardFooter";
        lblCardFooter.Size = new Size(398, 108);
        lblCardFooter.TabIndex = 3;
        lblCardFooter.Text = "توقيع أمين المخزن: ....................    توقيع المحاسب: ....................\r\nتوقيع مدير المخزون: ....................\r\n\r\nشكراً لتعاملكم معنا";
        lblCardFooter.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(btnPrint);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(440, 66);
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
        btnPrint.Location = new Point(288, 12);
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
        // frmStocktakingPrintAdjustment
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(440, 520);
        Controls.Add(pnlFooter);
        Controls.Add(pnlPreviewCard);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmStocktakingPrintAdjustment";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طباعة سند تسوية جرد";
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
    private Label lblAdjustmentTitle;
    private TableLayoutPanel tblFields;
    private Label lblVoucherNoLabel;
    private Label lblVoucherNoValue;
    private Label lblDateLabel;
    private Label lblDateValue;
    private Label lblWarehouseLabel;
    private Label lblWarehouseValue;
    private Label lblReasonLabel;
    private Label lblReasonValue;
    private Label lblItemCountLabel;
    private Label lblItemCountValue;
    private Label lblTotalLabel;
    private Label lblTotalValue;
    private Panel pnlDashedSeparator;
    private Label lblCardFooter;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnPrint;
    private Button btnClose;
}