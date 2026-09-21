namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmWarehouseTransferPrint
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
        lblTransferTitle = new Label();
        tblFields = new TableLayoutPanel();
        lblTransferNoValue = new Label();
        lblTransferNoLabel = new Label();
        lblDateValue = new Label();
        lblDateLabel = new Label();
        lblFromWarehouseValue = new Label();
        lblFromWarehouseLabel = new Label();
        lblToWarehouseValue = new Label();
        lblToWarehouseLabel = new Label();
        lblDriverValue = new Label();
        lblDriverLabel = new Label();
        lblVehiclePlateValue = new Label();
        lblVehiclePlateLabel = new Label();
        lblItemCountValue = new Label();
        lblItemCountLabel = new Label();
        lblNoteValue = new Label();
        lblNoteLabel = new Label();
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
        lblTitle.Text = "طباعة سند تحويل مخزني";
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
        lblAccent.BackColor = Color.FromArgb(230, 126, 34);
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
        pnlPreviewCard.Controls.Add(lblCardFooter);
        pnlPreviewCard.Location = new Point(20, 56);
        pnlPreviewCard.Name = "pnlPreviewCard";
        pnlPreviewCard.RightToLeft = RightToLeft.Yes;
        pnlPreviewCard.Size = new Size(400, 366);
        pnlPreviewCard.TabIndex = 1;
        // 
        // pnlCardHeader
        // 
        pnlCardHeader.BackColor = Color.White;
        pnlCardHeader.Controls.Add(lblTransferTitle);
        pnlCardHeader.Controls.Add(lblStoreName);
        pnlCardHeader.Dock = DockStyle.Top;
        pnlCardHeader.Name = "pnlCardHeader";
        pnlCardHeader.Size = new Size(398, 74);
        pnlCardHeader.TabIndex = 0;
        // 
        // lblStoreName
        // 
        lblStoreName.Dock = DockStyle.Top;
        lblStoreName.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStoreName.ForeColor = Color.FromArgb(23, 87, 129);
        lblStoreName.Location = new Point(0, 0);
        lblStoreName.Name = "lblStoreName";
        lblStoreName.Size = new Size(398, 30);
        lblStoreName.TabIndex = 0;
        lblStoreName.Text = "متجر النور التجاري";
        lblStoreName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTransferTitle
        // 
        lblTransferTitle.Dock = DockStyle.Top;
        lblTransferTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTransferTitle.ForeColor = Color.FromArgb(230, 126, 34);
        lblTransferTitle.Location = new Point(0, 30);
        lblTransferTitle.Name = "lblTransferTitle";
        lblTransferTitle.Size = new Size(398, 40);
        lblTransferTitle.TabIndex = 1;
        lblTransferTitle.Text = "سند تحويل مخزني";
        lblTransferTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tblFields
        // 
        tblFields.BackColor = Color.White;
        tblFields.ColumnCount = 2;
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblFields.Controls.Add(lblTransferNoValue, 1, 0);
        tblFields.Controls.Add(lblTransferNoLabel, 0, 0);
        tblFields.Controls.Add(lblDateValue, 1, 1);
        tblFields.Controls.Add(lblDateLabel, 0, 1);
        tblFields.Controls.Add(lblFromWarehouseValue, 1, 2);
        tblFields.Controls.Add(lblFromWarehouseLabel, 0, 2);
        tblFields.Controls.Add(lblToWarehouseValue, 1, 3);
        tblFields.Controls.Add(lblToWarehouseLabel, 0, 3);
        tblFields.Controls.Add(lblDriverValue, 1, 4);
        tblFields.Controls.Add(lblDriverLabel, 0, 4);
        tblFields.Controls.Add(lblVehiclePlateValue, 1, 5);
        tblFields.Controls.Add(lblVehiclePlateLabel, 0, 5);
        tblFields.Controls.Add(lblItemCountValue, 1, 6);
        tblFields.Controls.Add(lblItemCountLabel, 0, 6);
        tblFields.Controls.Add(lblNoteValue, 1, 7);
        tblFields.Controls.Add(lblNoteLabel, 0, 7);
        tblFields.Dock = DockStyle.Top;
        tblFields.Location = new Point(0, 74);
        tblFields.Name = "tblFields";
        tblFields.Padding = new Padding(12, 4, 12, 4);
        tblFields.RightToLeft = RightToLeft.Yes;
        tblFields.RowCount = 8;
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        tblFields.Size = new Size(398, 238);
        tblFields.TabIndex = 1;
        // 
        // lblTransferNoLabel
        // 
        lblTransferNoLabel.AutoSize = true;
        lblTransferNoLabel.Dock = DockStyle.Fill;
        lblTransferNoLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTransferNoLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTransferNoLabel.Location = new Point(15, 7);
        lblTransferNoLabel.Name = "lblTransferNoLabel";
        lblTransferNoLabel.Size = new Size(104, 20);
        lblTransferNoLabel.TabIndex = 0;
        lblTransferNoLabel.Text = "رقم السند";
        lblTransferNoLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTransferNoValue
        // 
        lblTransferNoValue.AutoEllipsis = true;
        lblTransferNoValue.AutoSize = true;
        lblTransferNoValue.Dock = DockStyle.Fill;
        lblTransferNoValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTransferNoValue.ForeColor = Color.FromArgb(230, 126, 34);
        lblTransferNoValue.Location = new Point(125, 7);
        lblTransferNoValue.Name = "lblTransferNoValue";
        lblTransferNoValue.Size = new Size(258, 20);
        lblTransferNoValue.TabIndex = 1;
        lblTransferNoValue.Text = "TR-2026-001";
        lblTransferNoValue.TextAlign = ContentAlignment.MiddleLeft;
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
        // lblFromWarehouseLabel
        // 
        lblFromWarehouseLabel.AutoSize = true;
        lblFromWarehouseLabel.Dock = DockStyle.Fill;
        lblFromWarehouseLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFromWarehouseLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblFromWarehouseLabel.Location = new Point(15, 59);
        lblFromWarehouseLabel.Name = "lblFromWarehouseLabel";
        lblFromWarehouseLabel.Size = new Size(104, 20);
        lblFromWarehouseLabel.TabIndex = 4;
        lblFromWarehouseLabel.Text = "من مستودع";
        lblFromWarehouseLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblFromWarehouseValue
        // 
        lblFromWarehouseValue.AutoEllipsis = true;
        lblFromWarehouseValue.Dock = DockStyle.Fill;
        lblFromWarehouseValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblFromWarehouseValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblFromWarehouseValue.Location = new Point(125, 59);
        lblFromWarehouseValue.Name = "lblFromWarehouseValue";
        lblFromWarehouseValue.Size = new Size(258, 20);
        lblFromWarehouseValue.TabIndex = 5;
        lblFromWarehouseValue.Text = "المستودع الرئيسي";
        lblFromWarehouseValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblToWarehouseLabel
        // 
        lblToWarehouseLabel.AutoSize = true;
        lblToWarehouseLabel.Dock = DockStyle.Fill;
        lblToWarehouseLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblToWarehouseLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblToWarehouseLabel.Location = new Point(15, 85);
        lblToWarehouseLabel.Name = "lblToWarehouseLabel";
        lblToWarehouseLabel.Size = new Size(104, 20);
        lblToWarehouseLabel.TabIndex = 6;
        lblToWarehouseLabel.Text = "إلى مستودع";
        lblToWarehouseLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblToWarehouseValue
        // 
        lblToWarehouseValue.AutoEllipsis = true;
        lblToWarehouseValue.Dock = DockStyle.Fill;
        lblToWarehouseValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblToWarehouseValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblToWarehouseValue.Location = new Point(125, 85);
        lblToWarehouseValue.Name = "lblToWarehouseValue";
        lblToWarehouseValue.Size = new Size(258, 20);
        lblToWarehouseValue.TabIndex = 7;
        lblToWarehouseValue.Text = "مخزن المعرض";
        lblToWarehouseValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDriverLabel
        // 
        lblDriverLabel.AutoSize = true;
        lblDriverLabel.Dock = DockStyle.Fill;
        lblDriverLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDriverLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblDriverLabel.Location = new Point(15, 111);
        lblDriverLabel.Name = "lblDriverLabel";
        lblDriverLabel.Size = new Size(104, 20);
        lblDriverLabel.TabIndex = 8;
        lblDriverLabel.Text = "السائق";
        lblDriverLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDriverValue
        // 
        lblDriverValue.AutoEllipsis = true;
        lblDriverValue.Dock = DockStyle.Fill;
        lblDriverValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblDriverValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblDriverValue.Location = new Point(125, 111);
        lblDriverValue.Name = "lblDriverValue";
        lblDriverValue.Size = new Size(258, 20);
        lblDriverValue.TabIndex = 9;
        lblDriverValue.Text = "محمد علي";
        lblDriverValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblVehiclePlateLabel
        // 
        lblVehiclePlateLabel.AutoSize = true;
        lblVehiclePlateLabel.Dock = DockStyle.Fill;
        lblVehiclePlateLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblVehiclePlateLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblVehiclePlateLabel.Location = new Point(15, 137);
        lblVehiclePlateLabel.Name = "lblVehiclePlateLabel";
        lblVehiclePlateLabel.Size = new Size(104, 20);
        lblVehiclePlateLabel.TabIndex = 10;
        lblVehiclePlateLabel.Text = "لوحة المركبة";
        lblVehiclePlateLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblVehiclePlateValue
        // 
        lblVehiclePlateValue.AutoEllipsis = true;
        lblVehiclePlateValue.Dock = DockStyle.Fill;
        lblVehiclePlateValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblVehiclePlateValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblVehiclePlateValue.Location = new Point(125, 137);
        lblVehiclePlateValue.Name = "lblVehiclePlateValue";
        lblVehiclePlateValue.Size = new Size(258, 20);
        lblVehiclePlateValue.TabIndex = 11;
        lblVehiclePlateValue.Text = "12345 صنعاء";
        lblVehiclePlateValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblItemCountLabel
        // 
        lblItemCountLabel.AutoSize = true;
        lblItemCountLabel.Dock = DockStyle.Fill;
        lblItemCountLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblItemCountLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblItemCountLabel.Location = new Point(15, 163);
        lblItemCountLabel.Name = "lblItemCountLabel";
        lblItemCountLabel.Size = new Size(104, 20);
        lblItemCountLabel.TabIndex = 12;
        lblItemCountLabel.Text = "عدد الأصناف";
        lblItemCountLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblItemCountValue
        // 
        lblItemCountValue.AutoEllipsis = true;
        lblItemCountValue.Dock = DockStyle.Fill;
        lblItemCountValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblItemCountValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblItemCountValue.Location = new Point(125, 163);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(258, 20);
        lblItemCountValue.TabIndex = 13;
        lblItemCountValue.Text = "8 أصناف";
        lblItemCountValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblNoteLabel
        // 
        lblNoteLabel.AutoSize = true;
        lblNoteLabel.Dock = DockStyle.Fill;
        lblNoteLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNoteLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblNoteLabel.Location = new Point(15, 189);
        lblNoteLabel.Name = "lblNoteLabel";
        lblNoteLabel.Size = new Size(104, 50);
        lblNoteLabel.TabIndex = 14;
        lblNoteLabel.Text = "ملاحظة";
        lblNoteLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNoteValue
        // 
        lblNoteValue.BackColor = Color.FromArgb(255, 248, 240);
        lblNoteValue.BorderStyle = BorderStyle.FixedSingle;
        lblNoteValue.Dock = DockStyle.Fill;
        lblNoteValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNoteValue.ForeColor = Color.FromArgb(230, 126, 34);
        lblNoteValue.Location = new Point(125, 189);
        lblNoteValue.Name = "lblNoteValue";
        lblNoteValue.Size = new Size(258, 50);
        lblNoteValue.TabIndex = 15;
        lblNoteValue.Text = "يُستلم في المستودع الوجهة";
        lblNoteValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlDashedSeparator
        // 
        pnlDashedSeparator.BackColor = Color.White;
        pnlDashedSeparator.Dock = DockStyle.Top;
        pnlDashedSeparator.Location = new Point(0, 312);
        pnlDashedSeparator.Name = "pnlDashedSeparator";
        pnlDashedSeparator.Size = new Size(398, 14);
        pnlDashedSeparator.TabIndex = 2;
        pnlDashedSeparator.Paint += pnlDashedSeparator_Paint;
        // 
        // lblCardFooter
        // 
        lblCardFooter.BackColor = Color.White;
        lblCardFooter.Dock = DockStyle.Bottom;
        lblCardFooter.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCardFooter.ForeColor = Color.FromArgb(108, 117, 125);
        lblCardFooter.Location = new Point(0, 326);
        lblCardFooter.Name = "lblCardFooter";
        lblCardFooter.Size = new Size(398, 38);
        lblCardFooter.TabIndex = 3;
        lblCardFooter.Text = "توقيع أمين المستودع: ....................    توقيع المستلم: ....................";
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
        // frmWarehouseTransferPrint
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(440, 560);
        Controls.Add(pnlFooter);
        Controls.Add(pnlPreviewCard);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmWarehouseTransferPrint";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طباعة سند تحويل مخزني";
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
    private Label lblTransferTitle;
    private TableLayoutPanel tblFields;
    private Label lblTransferNoLabel;
    private Label lblTransferNoValue;
    private Label lblDateLabel;
    private Label lblDateValue;
    private Label lblFromWarehouseLabel;
    private Label lblFromWarehouseValue;
    private Label lblToWarehouseLabel;
    private Label lblToWarehouseValue;
    private Label lblDriverLabel;
    private Label lblDriverValue;
    private Label lblVehiclePlateLabel;
    private Label lblVehiclePlateValue;
    private Label lblItemCountLabel;
    private Label lblItemCountValue;
    private Label lblNoteLabel;
    private Label lblNoteValue;
    private Panel pnlDashedSeparator;
    private Label lblCardFooter;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnPrint;
    private Button btnClose;
}