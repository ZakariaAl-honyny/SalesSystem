namespace SalesSystem.Desktop.Screens.Purchases.Forms;

partial class frmPurchaseInvoicePrint
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
        lblInvoiceTitle = new Label();
        tblFields = new TableLayoutPanel();
        lblInvoiceNoValue = new Label();
        lblInvoiceNoLabel = new Label();
        lblDateValue = new Label();
        lblDateLabel = new Label();
        lblSupplierValue = new Label();
        lblSupplierLabel = new Label();
        lblWarehouseValue = new Label();
        lblWarehouseLabel = new Label();
        lblSubTotalValue = new Label();
        lblSubTotalLabel = new Label();
        lblDiscountValue = new Label();
        lblDiscountLabel = new Label();
        lblTaxValue = new Label();
        lblTaxLabel = new Label();
        lblTotalValue = new Label();
        lblTotalLabel = new Label();
        lblPaidValue = new Label();
        lblPaidLabel = new Label();
        lblRemainingValue = new Label();
        lblRemainingLabel = new Label();
        lblTafqitValue = new Label();
        lblTafqitLabel = new Label();
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
        lblTitle.Text = "طباعة فاتورة مشتريات";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(312, 13);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(112, 21);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock)";
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(23, 87, 129);
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
        pnlPreviewCard.Size = new Size(400, 430);
        pnlPreviewCard.TabIndex = 1;
        // 
        // pnlCardHeader
        // 
        pnlCardHeader.BackColor = Color.White;
        pnlCardHeader.Controls.Add(lblInvoiceTitle);
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
        // lblInvoiceTitle
        // 
        lblInvoiceTitle.Dock = DockStyle.Top;
        lblInvoiceTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblInvoiceTitle.ForeColor = Color.FromArgb(23, 87, 129);
        lblInvoiceTitle.Location = new Point(0, 30);
        lblInvoiceTitle.Name = "lblInvoiceTitle";
        lblInvoiceTitle.Size = new Size(398, 40);
        lblInvoiceTitle.TabIndex = 1;
        lblInvoiceTitle.Text = "فاتورة مشتريات";
        lblInvoiceTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tblFields
        // 
        tblFields.BackColor = Color.White;
        tblFields.ColumnCount = 2;
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblFields.Controls.Add(lblInvoiceNoValue, 1, 0);
        tblFields.Controls.Add(lblInvoiceNoLabel, 0, 0);
        tblFields.Controls.Add(lblDateValue, 1, 1);
        tblFields.Controls.Add(lblDateLabel, 0, 1);
        tblFields.Controls.Add(lblSupplierValue, 1, 2);
        tblFields.Controls.Add(lblSupplierLabel, 0, 2);
        tblFields.Controls.Add(lblWarehouseValue, 1, 3);
        tblFields.Controls.Add(lblWarehouseLabel, 0, 3);
        tblFields.Controls.Add(lblSubTotalValue, 1, 4);
        tblFields.Controls.Add(lblSubTotalLabel, 0, 4);
        tblFields.Controls.Add(lblDiscountValue, 1, 5);
        tblFields.Controls.Add(lblDiscountLabel, 0, 5);
        tblFields.Controls.Add(lblTaxValue, 1, 6);
        tblFields.Controls.Add(lblTaxLabel, 0, 6);
        tblFields.Controls.Add(lblTotalValue, 1, 7);
        tblFields.Controls.Add(lblTotalLabel, 0, 7);
        tblFields.Controls.Add(lblPaidValue, 1, 8);
        tblFields.Controls.Add(lblPaidLabel, 0, 8);
        tblFields.Controls.Add(lblRemainingValue, 1, 9);
        tblFields.Controls.Add(lblRemainingLabel, 0, 9);
        tblFields.Controls.Add(lblTafqitValue, 1, 10);
        tblFields.Controls.Add(lblTafqitLabel, 0, 10);
        tblFields.Dock = DockStyle.Top;
        tblFields.Location = new Point(0, 74);
        tblFields.Name = "tblFields";
        tblFields.Padding = new Padding(12, 4, 12, 4);
        tblFields.RightToLeft = RightToLeft.Yes;
        tblFields.RowCount = 11;
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tblFields.Size = new Size(398, 308);
        tblFields.TabIndex = 1;
        // 
        // lblInvoiceNoLabel
        // 
        lblInvoiceNoLabel.AutoSize = true;
        lblInvoiceNoLabel.Dock = DockStyle.Fill;
        lblInvoiceNoLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblInvoiceNoLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblInvoiceNoLabel.Location = new Point(15, 7);
        lblInvoiceNoLabel.Name = "lblInvoiceNoLabel";
        lblInvoiceNoLabel.Size = new Size(104, 18);
        lblInvoiceNoLabel.TabIndex = 0;
        lblInvoiceNoLabel.Text = "رقم الفاتورة";
        lblInvoiceNoLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblInvoiceNoValue
        // 
        lblInvoiceNoValue.AutoSize = true;
        lblInvoiceNoValue.Dock = DockStyle.Fill;
        lblInvoiceNoValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblInvoiceNoValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblInvoiceNoValue.Location = new Point(125, 7);
        lblInvoiceNoValue.Name = "lblInvoiceNoValue";
        lblInvoiceNoValue.Size = new Size(258, 18);
        lblInvoiceNoValue.TabIndex = 1;
        lblInvoiceNoValue.Text = "5003";
        lblInvoiceNoValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDateLabel
        // 
        lblDateLabel.AutoSize = true;
        lblDateLabel.Dock = DockStyle.Fill;
        lblDateLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDateLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblDateLabel.Location = new Point(15, 31);
        lblDateLabel.Name = "lblDateLabel";
        lblDateLabel.Size = new Size(104, 18);
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
        lblDateValue.Location = new Point(125, 31);
        lblDateValue.Name = "lblDateValue";
        lblDateValue.Size = new Size(258, 18);
        lblDateValue.TabIndex = 3;
        lblDateValue.Text = "18/09/2026";
        lblDateValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblSupplierLabel
        // 
        lblSupplierLabel.AutoSize = true;
        lblSupplierLabel.Dock = DockStyle.Fill;
        lblSupplierLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSupplierLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblSupplierLabel.Location = new Point(15, 55);
        lblSupplierLabel.Name = "lblSupplierLabel";
        lblSupplierLabel.Size = new Size(104, 18);
        lblSupplierLabel.TabIndex = 4;
        lblSupplierLabel.Text = "المورد";
        lblSupplierLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblSupplierValue
        // 
        lblSupplierValue.AutoEllipsis = true;
        lblSupplierValue.Dock = DockStyle.Fill;
        lblSupplierValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblSupplierValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblSupplierValue.Location = new Point(125, 55);
        lblSupplierValue.Name = "lblSupplierValue";
        lblSupplierValue.Size = new Size(258, 18);
        lblSupplierValue.TabIndex = 5;
        lblSupplierValue.Text = "شركة النور للتجارة";
        lblSupplierValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblWarehouseLabel
        // 
        lblWarehouseLabel.AutoSize = true;
        lblWarehouseLabel.Dock = DockStyle.Fill;
        lblWarehouseLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblWarehouseLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblWarehouseLabel.Location = new Point(15, 79);
        lblWarehouseLabel.Name = "lblWarehouseLabel";
        lblWarehouseLabel.Size = new Size(104, 18);
        lblWarehouseLabel.TabIndex = 6;
        lblWarehouseLabel.Text = "المستودع";
        lblWarehouseLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblWarehouseValue
        // 
        lblWarehouseValue.AutoEllipsis = true;
        lblWarehouseValue.Dock = DockStyle.Fill;
        lblWarehouseValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblWarehouseValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblWarehouseValue.Location = new Point(125, 79);
        lblWarehouseValue.Name = "lblWarehouseValue";
        lblWarehouseValue.Size = new Size(258, 18);
        lblWarehouseValue.TabIndex = 7;
        lblWarehouseValue.Text = "المستودع الرئيسي (1)";
        lblWarehouseValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblSubTotalLabel
        // 
        lblSubTotalLabel.AutoSize = true;
        lblSubTotalLabel.Dock = DockStyle.Fill;
        lblSubTotalLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSubTotalLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblSubTotalLabel.Location = new Point(15, 103);
        lblSubTotalLabel.Name = "lblSubTotalLabel";
        lblSubTotalLabel.Size = new Size(104, 18);
        lblSubTotalLabel.TabIndex = 8;
        lblSubTotalLabel.Text = "المجموع الفرعي";
        lblSubTotalLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblSubTotalValue
        // 
        lblSubTotalValue.AutoSize = true;
        lblSubTotalValue.Dock = DockStyle.Fill;
        lblSubTotalValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblSubTotalValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblSubTotalValue.Location = new Point(125, 103);
        lblSubTotalValue.Name = "lblSubTotalValue";
        lblSubTotalValue.Size = new Size(258, 18);
        lblSubTotalValue.TabIndex = 9;
        lblSubTotalValue.Text = "650,000.00 ريال يمني";
        lblSubTotalValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDiscountLabel
        // 
        lblDiscountLabel.AutoSize = true;
        lblDiscountLabel.Dock = DockStyle.Fill;
        lblDiscountLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDiscountLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblDiscountLabel.Location = new Point(15, 127);
        lblDiscountLabel.Name = "lblDiscountLabel";
        lblDiscountLabel.Size = new Size(104, 18);
        lblDiscountLabel.TabIndex = 10;
        lblDiscountLabel.Text = "الخصم";
        lblDiscountLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDiscountValue
        // 
        lblDiscountValue.AutoSize = true;
        lblDiscountValue.Dock = DockStyle.Fill;
        lblDiscountValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblDiscountValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblDiscountValue.Location = new Point(125, 127);
        lblDiscountValue.Name = "lblDiscountValue";
        lblDiscountValue.Size = new Size(258, 18);
        lblDiscountValue.TabIndex = 11;
        lblDiscountValue.Text = "- 8,000.00 ريال يمني";
        lblDiscountValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTaxLabel
        // 
        lblTaxLabel.AutoSize = true;
        lblTaxLabel.Dock = DockStyle.Fill;
        lblTaxLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTaxLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTaxLabel.Location = new Point(15, 151);
        lblTaxLabel.Name = "lblTaxLabel";
        lblTaxLabel.Size = new Size(104, 18);
        lblTaxLabel.TabIndex = 12;
        lblTaxLabel.Text = "الضريبة";
        lblTaxLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTaxValue
        // 
        lblTaxValue.AutoSize = true;
        lblTaxValue.Dock = DockStyle.Fill;
        lblTaxValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblTaxValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblTaxValue.Location = new Point(125, 151);
        lblTaxValue.Name = "lblTaxValue";
        lblTaxValue.Size = new Size(258, 18);
        lblTaxValue.TabIndex = 13;
        lblTaxValue.Text = "72,415.00 ريال يمني";
        lblTaxValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTotalLabel
        // 
        lblTotalLabel.AutoSize = true;
        lblTotalLabel.Dock = DockStyle.Fill;
        lblTotalLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTotalLabel.Location = new Point(15, 175);
        lblTotalLabel.Name = "lblTotalLabel";
        lblTotalLabel.Size = new Size(104, 38);
        lblTotalLabel.TabIndex = 14;
        lblTotalLabel.Text = "الإجمالي";
        lblTotalLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTotalValue
        // 
        lblTotalValue.BackColor = Color.FromArgb(240, 247, 253);
        lblTotalValue.BorderStyle = BorderStyle.FixedSingle;
        lblTotalValue.Dock = DockStyle.Fill;
        lblTotalValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalValue.ForeColor = Color.FromArgb(23, 87, 129);
        lblTotalValue.Location = new Point(125, 175);
        lblTotalValue.Name = "lblTotalValue";
        lblTotalValue.Size = new Size(258, 38);
        lblTotalValue.TabIndex = 15;
        lblTotalValue.Text = "714,415 ريال يمني";
        lblTotalValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblPaidLabel
        // 
        lblPaidLabel.AutoSize = true;
        lblPaidLabel.Dock = DockStyle.Fill;
        lblPaidLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPaidLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblPaidLabel.Location = new Point(15, 219);
        lblPaidLabel.Name = "lblPaidLabel";
        lblPaidLabel.Size = new Size(104, 18);
        lblPaidLabel.TabIndex = 16;
        lblPaidLabel.Text = "المدفوع";
        lblPaidLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblPaidValue
        // 
        lblPaidValue.AutoSize = true;
        lblPaidValue.Dock = DockStyle.Fill;
        lblPaidValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblPaidValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblPaidValue.Location = new Point(125, 219);
        lblPaidValue.Name = "lblPaidValue";
        lblPaidValue.Size = new Size(258, 18);
        lblPaidValue.TabIndex = 17;
        lblPaidValue.Text = "500,000.00 ريال يمني";
        lblPaidValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblRemainingLabel
        // 
        lblRemainingLabel.AutoSize = true;
        lblRemainingLabel.Dock = DockStyle.Fill;
        lblRemainingLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRemainingLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblRemainingLabel.Location = new Point(15, 243);
        lblRemainingLabel.Name = "lblRemainingLabel";
        lblRemainingLabel.Size = new Size(104, 18);
        lblRemainingLabel.TabIndex = 18;
        lblRemainingLabel.Text = "الباقي";
        lblRemainingLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblRemainingValue
        // 
        lblRemainingValue.AutoSize = true;
        lblRemainingValue.Dock = DockStyle.Fill;
        lblRemainingValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblRemainingValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblRemainingValue.Location = new Point(125, 243);
        lblRemainingValue.Name = "lblRemainingValue";
        lblRemainingValue.Size = new Size(258, 18);
        lblRemainingValue.TabIndex = 19;
        lblRemainingValue.Text = "214,415.00 ريال يمني";
        lblRemainingValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTafqitLabel
        // 
        lblTafqitLabel.AutoSize = true;
        lblTafqitLabel.Dock = DockStyle.Fill;
        lblTafqitLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTafqitLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTafqitLabel.Location = new Point(15, 267);
        lblTafqitLabel.Name = "lblTafqitLabel";
        lblTafqitLabel.Size = new Size(104, 34);
        lblTafqitLabel.TabIndex = 20;
        lblTafqitLabel.Text = "التفقيط";
        lblTafqitLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTafqitValue
        // 
        lblTafqitValue.AutoEllipsis = true;
        lblTafqitValue.Dock = DockStyle.Fill;
        lblTafqitValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTafqitValue.ForeColor = Color.FromArgb(23, 87, 129);
        lblTafqitValue.Location = new Point(125, 267);
        lblTafqitValue.Name = "lblTafqitValue";
        lblTafqitValue.Size = new Size(258, 34);
        lblTafqitValue.TabIndex = 21;
        lblTafqitValue.Text = "سبعمائة وأربعة عشر ألفاً وأربعمائة وخمسة عشر ريالاً يمنياً فقط";
        lblTafqitValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlDashedSeparator
        // 
        pnlDashedSeparator.BackColor = Color.White;
        pnlDashedSeparator.Dock = DockStyle.Top;
        pnlDashedSeparator.Location = new Point(0, 382);
        pnlDashedSeparator.Name = "pnlDashedSeparator";
        pnlDashedSeparator.Size = new Size(398, 14);
        pnlDashedSeparator.TabIndex = 2;
        pnlDashedSeparator.Paint += pnlDashedSeparator_Paint;
        // 
        // lblThanks
        // 
        lblThanks.BackColor = Color.White;
        lblThanks.Dock = DockStyle.Bottom;
        lblThanks.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblThanks.ForeColor = Color.FromArgb(108, 117, 125);
        lblThanks.Location = new Point(0, 396);
        lblThanks.Name = "lblThanks";
        lblThanks.Size = new Size(398, 34);
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
        // frmPurchaseInvoicePrint
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(440, 580);
        Controls.Add(pnlFooter);
        Controls.Add(pnlPreviewCard);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmPurchaseInvoicePrint";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طباعة فاتورة مشتريات";
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
    private Label lblInvoiceTitle;
    private TableLayoutPanel tblFields;
    private Label lblInvoiceNoLabel;
    private Label lblInvoiceNoValue;
    private Label lblDateLabel;
    private Label lblDateValue;
    private Label lblSupplierLabel;
    private Label lblSupplierValue;
    private Label lblWarehouseLabel;
    private Label lblWarehouseValue;
    private Label lblSubTotalLabel;
    private Label lblSubTotalValue;
    private Label lblDiscountLabel;
    private Label lblDiscountValue;
    private Label lblTaxLabel;
    private Label lblTaxValue;
    private Label lblTotalLabel;
    private Label lblTotalValue;
    private Label lblPaidLabel;
    private Label lblPaidValue;
    private Label lblRemainingLabel;
    private Label lblRemainingValue;
    private Label lblTafqitLabel;
    private Label lblTafqitValue;
    private Panel pnlDashedSeparator;
    private Label lblThanks;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnPrint;
    private Button btnClose;
}