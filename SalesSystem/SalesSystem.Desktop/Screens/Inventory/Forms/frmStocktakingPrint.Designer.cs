namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmStocktakingPrint
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
        lblStocktakingTitle = new Label();
        lblStoreName = new Label();
        tblFields = new TableLayoutPanel();
        lblDocNoValue = new Label();
        lblDocNoLabel = new Label();
        lblDateValue = new Label();
        lblDateLabel = new Label();
        lblWarehouseValue = new Label();
        lblWarehouseLabel = new Label();
        lblCommitteeHeadValue = new Label();
        lblCommitteeHeadLabel = new Label();
        lblCommitteeMemberValue = new Label();
        lblCommitteeMemberLabel = new Label();
        lblStorekeeperValue = new Label();
        lblStorekeeperLabel = new Label();
        lblItemCountValue = new Label();
        lblItemCountLabel = new Label();
        lblSurplusValue = new Label();
        lblSurplusLabel = new Label();
        lblDeficitValue = new Label();
        lblDeficitLabel = new Label();
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
        // RULE-N134/N140 — الثيم الأخضر: قيم حرفية مطابقة لثوابت وقت التشغيل (بلا أي مرجع لـ Helpers هنا).
        pnlHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
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
        lblTitle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = System.Drawing.Color.White;
        lblTitle.Location = new Point(0, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(440, 42);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "طباعة محضر جرد";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(241), Convert.ToInt32(196), Convert.ToInt32(15));
        lblMockBadge.Location = new Point(316, 13);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(112, 21);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock)";
        // 
        // lblAccent
        // 
        lblAccent.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 3;
        // 
        // pnlPreviewCard
        // 
        pnlPreviewCard.BackColor = System.Drawing.Color.White;
        pnlPreviewCard.BorderStyle = BorderStyle.FixedSingle;
        pnlPreviewCard.Controls.Add(lblCardFooter);
        pnlPreviewCard.Controls.Add(pnlDashedSeparator);
        pnlPreviewCard.Controls.Add(tblFields);
        pnlPreviewCard.Controls.Add(pnlCardHeader);
        pnlPreviewCard.Location = new Point(20, 56);
        pnlPreviewCard.Name = "pnlPreviewCard";
        pnlPreviewCard.RightToLeft = RightToLeft.Yes;
        pnlPreviewCard.Size = new Size(400, 390);
        pnlPreviewCard.TabIndex = 1;
        // 
        // pnlCardHeader
        // 
        pnlCardHeader.BackColor = System.Drawing.Color.White;
        pnlCardHeader.Controls.Add(lblStocktakingTitle);
        pnlCardHeader.Controls.Add(lblStoreName);
        pnlCardHeader.Location = new Point(0, 0);
        pnlCardHeader.Name = "pnlCardHeader";
        pnlCardHeader.Size = new Size(398, 74);
        pnlCardHeader.TabIndex = 0;
        // 
        // lblStoreName
        // 
        lblStoreName.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStoreName.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblStoreName.Location = new Point(0, 0);
        lblStoreName.Name = "lblStoreName";
        lblStoreName.Size = new Size(398, 30);
        lblStoreName.TabIndex = 0;
        lblStoreName.Text = "متجر النور التجاري";
        lblStoreName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblStocktakingTitle
        // 
        lblStocktakingTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStocktakingTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblStocktakingTitle.Location = new Point(0, 30);
        lblStocktakingTitle.Name = "lblStocktakingTitle";
        lblStocktakingTitle.Size = new Size(398, 40);
        lblStocktakingTitle.TabIndex = 1;
        lblStocktakingTitle.Text = "محضر جرد فعلي";
        lblStocktakingTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tblFields
        // 
        tblFields.BackColor = System.Drawing.Color.White;
        tblFields.ColumnCount = 2;
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblFields.Controls.Add(lblDocNoValue, 1, 0);
        tblFields.Controls.Add(lblDocNoLabel, 0, 0);
        tblFields.Controls.Add(lblDateValue, 1, 1);
        tblFields.Controls.Add(lblDateLabel, 0, 1);
        tblFields.Controls.Add(lblWarehouseValue, 1, 2);
        tblFields.Controls.Add(lblWarehouseLabel, 0, 2);
        tblFields.Controls.Add(lblCommitteeHeadValue, 1, 3);
        tblFields.Controls.Add(lblCommitteeHeadLabel, 0, 3);
        tblFields.Controls.Add(lblCommitteeMemberValue, 1, 4);
        tblFields.Controls.Add(lblCommitteeMemberLabel, 0, 4);
        tblFields.Controls.Add(lblStorekeeperValue, 1, 5);
        tblFields.Controls.Add(lblStorekeeperLabel, 0, 5);
        tblFields.Controls.Add(lblItemCountValue, 1, 6);
        tblFields.Controls.Add(lblItemCountLabel, 0, 6);
        tblFields.Controls.Add(lblSurplusValue, 1, 7);
        tblFields.Controls.Add(lblSurplusLabel, 0, 7);
        tblFields.Controls.Add(lblDeficitValue, 1, 8);
        tblFields.Controls.Add(lblDeficitLabel, 0, 8);
        tblFields.Location = new Point(0, 74);
        tblFields.Name = "tblFields";
        tblFields.Padding = new Padding(12, 4, 12, 4);
        tblFields.RightToLeft = RightToLeft.Yes;
        tblFields.RowCount = 9;
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.Size = new Size(398, 242);
        tblFields.TabIndex = 1;
        // 
        // lblDocNoLabel
        // 
        lblDocNoLabel.AutoSize = true;
        lblDocNoLabel.Dock = DockStyle.Fill;
        lblDocNoLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDocNoLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblDocNoLabel.Location = new Point(15, 7);
        lblDocNoLabel.Name = "lblDocNoLabel";
        lblDocNoLabel.Size = new Size(104, 20);
        lblDocNoLabel.TabIndex = 0;
        lblDocNoLabel.Text = "رقم المحضر";
        lblDocNoLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDocNoValue
        // 
        lblDocNoValue.AutoEllipsis = true;
        lblDocNoValue.AutoSize = true;
        lblDocNoValue.Dock = DockStyle.Fill;
        lblDocNoValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDocNoValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblDocNoValue.Location = new Point(125, 7);
        lblDocNoValue.Name = "lblDocNoValue";
        lblDocNoValue.Size = new Size(258, 20);
        lblDocNoValue.TabIndex = 1;
        lblDocNoValue.Text = "INV-2026-001";
        lblDocNoValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDateLabel
        // 
        lblDateLabel.AutoSize = true;
        lblDateLabel.Dock = DockStyle.Fill;
        lblDateLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDateLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
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
        lblDateValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
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
        lblWarehouseLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
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
        lblWarehouseValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblWarehouseValue.Location = new Point(125, 59);
        lblWarehouseValue.Name = "lblWarehouseValue";
        lblWarehouseValue.Size = new Size(258, 20);
        lblWarehouseValue.TabIndex = 5;
        lblWarehouseValue.Text = "المستودع الرئيسي";
        lblWarehouseValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblCommitteeHeadLabel
        // 
        lblCommitteeHeadLabel.AutoSize = true;
        lblCommitteeHeadLabel.Dock = DockStyle.Fill;
        lblCommitteeHeadLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCommitteeHeadLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblCommitteeHeadLabel.Location = new Point(15, 85);
        lblCommitteeHeadLabel.Name = "lblCommitteeHeadLabel";
        lblCommitteeHeadLabel.Size = new Size(104, 20);
        lblCommitteeHeadLabel.TabIndex = 6;
        lblCommitteeHeadLabel.Text = "رئيس لجنة الجرد";
        lblCommitteeHeadLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCommitteeHeadValue
        // 
        lblCommitteeHeadValue.AutoEllipsis = true;
        lblCommitteeHeadValue.Dock = DockStyle.Fill;
        lblCommitteeHeadValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblCommitteeHeadValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblCommitteeHeadValue.Location = new Point(125, 85);
        lblCommitteeHeadValue.Name = "lblCommitteeHeadValue";
        lblCommitteeHeadValue.Size = new Size(258, 20);
        lblCommitteeHeadValue.TabIndex = 7;
        lblCommitteeHeadValue.Text = "أحمد محمد";
        lblCommitteeHeadValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblCommitteeMemberLabel
        // 
        lblCommitteeMemberLabel.AutoSize = true;
        lblCommitteeMemberLabel.Dock = DockStyle.Fill;
        lblCommitteeMemberLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCommitteeMemberLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblCommitteeMemberLabel.Location = new Point(15, 111);
        lblCommitteeMemberLabel.Name = "lblCommitteeMemberLabel";
        lblCommitteeMemberLabel.Size = new Size(104, 20);
        lblCommitteeMemberLabel.TabIndex = 8;
        lblCommitteeMemberLabel.Text = "عضو اللجنة";
        lblCommitteeMemberLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCommitteeMemberValue
        // 
        lblCommitteeMemberValue.AutoEllipsis = true;
        lblCommitteeMemberValue.Dock = DockStyle.Fill;
        lblCommitteeMemberValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblCommitteeMemberValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblCommitteeMemberValue.Location = new Point(125, 111);
        lblCommitteeMemberValue.Name = "lblCommitteeMemberValue";
        lblCommitteeMemberValue.Size = new Size(258, 20);
        lblCommitteeMemberValue.TabIndex = 9;
        lblCommitteeMemberValue.Text = "خالد عبدالله";
        lblCommitteeMemberValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblStorekeeperLabel
        // 
        lblStorekeeperLabel.AutoSize = true;
        lblStorekeeperLabel.Dock = DockStyle.Fill;
        lblStorekeeperLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStorekeeperLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblStorekeeperLabel.Location = new Point(15, 137);
        lblStorekeeperLabel.Name = "lblStorekeeperLabel";
        lblStorekeeperLabel.Size = new Size(104, 20);
        lblStorekeeperLabel.TabIndex = 10;
        lblStorekeeperLabel.Text = "أمين المخزن";
        lblStorekeeperLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblStorekeeperValue
        // 
        lblStorekeeperValue.AutoEllipsis = true;
        lblStorekeeperValue.Dock = DockStyle.Fill;
        lblStorekeeperValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblStorekeeperValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblStorekeeperValue.Location = new Point(125, 137);
        lblStorekeeperValue.Name = "lblStorekeeperValue";
        lblStorekeeperValue.Size = new Size(258, 20);
        lblStorekeeperValue.TabIndex = 11;
        lblStorekeeperValue.Text = "سارة علي";
        lblStorekeeperValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblItemCountLabel
        // 
        lblItemCountLabel.AutoSize = true;
        lblItemCountLabel.Dock = DockStyle.Fill;
        lblItemCountLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblItemCountLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
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
        lblItemCountValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblItemCountValue.Location = new Point(125, 163);
        lblItemCountValue.Name = "lblItemCountValue";
        lblItemCountValue.Size = new Size(258, 20);
        lblItemCountValue.TabIndex = 13;
        lblItemCountValue.Text = "8 أصناف";
        lblItemCountValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblSurplusLabel
        // 
        lblSurplusLabel.AutoSize = true;
        lblSurplusLabel.Dock = DockStyle.Fill;
        lblSurplusLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSurplusLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblSurplusLabel.Location = new Point(15, 189);
        lblSurplusLabel.Name = "lblSurplusLabel";
        lblSurplusLabel.Size = new Size(104, 20);
        lblSurplusLabel.TabIndex = 14;
        lblSurplusLabel.Text = "إجمالي الفائض";
        lblSurplusLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblSurplusValue
        // 
        lblSurplusValue.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(250), Convert.ToInt32(240));
        lblSurplusValue.BorderStyle = BorderStyle.FixedSingle;
        lblSurplusValue.Dock = DockStyle.Fill;
        lblSurplusValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSurplusValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        lblSurplusValue.Location = new Point(125, 189);
        lblSurplusValue.Name = "lblSurplusValue";
        lblSurplusValue.Size = new Size(258, 20);
        lblSurplusValue.TabIndex = 15;
        lblSurplusValue.Text = "12.750";
        lblSurplusValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblDeficitLabel
        // 
        lblDeficitLabel.AutoSize = true;
        lblDeficitLabel.Dock = DockStyle.Fill;
        lblDeficitLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDeficitLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblDeficitLabel.Location = new Point(15, 215);
        lblDeficitLabel.Name = "lblDeficitLabel";
        lblDeficitLabel.Size = new Size(104, 20);
        lblDeficitLabel.TabIndex = 16;
        lblDeficitLabel.Text = "إجمالي العجز";
        lblDeficitLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDeficitValue
        // 
        lblDeficitValue.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(252), Convert.ToInt32(242), Convert.ToInt32(242));
        lblDeficitValue.BorderStyle = BorderStyle.FixedSingle;
        lblDeficitValue.Dock = DockStyle.Fill;
        lblDeficitValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDeficitValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(53), Convert.ToInt32(69));
        lblDeficitValue.Location = new Point(125, 215);
        lblDeficitValue.Name = "lblDeficitValue";
        lblDeficitValue.Size = new Size(258, 20);
        lblDeficitValue.TabIndex = 17;
        lblDeficitValue.Text = "5.250";
        lblDeficitValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlDashedSeparator
        // 
        pnlDashedSeparator.BackColor = System.Drawing.Color.White;
        pnlDashedSeparator.Location = new Point(0, 316);
        pnlDashedSeparator.Name = "pnlDashedSeparator";
        pnlDashedSeparator.Size = new Size(398, 14);
        pnlDashedSeparator.TabIndex = 2;
        pnlDashedSeparator.Paint += pnlDashedSeparator_Paint;
        // 
        // lblCardFooter
        // 
        lblCardFooter.BackColor = System.Drawing.Color.White;
        lblCardFooter.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCardFooter.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblCardFooter.Location = new Point(0, 330);
        lblCardFooter.Name = "lblCardFooter";
        lblCardFooter.Size = new Size(398, 60);
        lblCardFooter.TabIndex = 3;
        lblCardFooter.Text = "توقيع رئيس لجنة الجرد: ....................    توقيع عضو اللجنة: ....................\r\nتوقيع أمين المخزن: ....................";
        lblCardFooter.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = System.Drawing.Color.White;
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
        lblFooterLine.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Height = 2;
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.TabIndex = 0;
        // 
        // btnPrint
        // 
        btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPrint.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        btnPrint.FlatAppearance.BorderSize = 0;
        btnPrint.FlatStyle = FlatStyle.Flat;
        btnPrint.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnPrint.ForeColor = System.Drawing.Color.White;
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
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(12, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(100, 42);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmStocktakingPrint
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
        CancelButton = btnClose;
        ClientSize = new Size(440, 600);
        Controls.Add(pnlFooter);
        Controls.Add(pnlPreviewCard);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmStocktakingPrint";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طباعة محضر جرد";
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
    private Label lblStocktakingTitle;
    private TableLayoutPanel tblFields;
    private Label lblDocNoLabel;
    private Label lblDocNoValue;
    private Label lblDateLabel;
    private Label lblDateValue;
    private Label lblWarehouseLabel;
    private Label lblWarehouseValue;
    private Label lblCommitteeHeadLabel;
    private Label lblCommitteeHeadValue;
    private Label lblCommitteeMemberLabel;
    private Label lblCommitteeMemberValue;
    private Label lblStorekeeperLabel;
    private Label lblStorekeeperValue;
    private Label lblItemCountLabel;
    private Label lblItemCountValue;
    private Label lblSurplusLabel;
    private Label lblSurplusValue;
    private Label lblDeficitLabel;
    private Label lblDeficitValue;
    private Panel pnlDashedSeparator;
    private Label lblCardFooter;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnPrint;
    private Button btnClose;
}
