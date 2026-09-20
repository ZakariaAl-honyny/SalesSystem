namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

partial class frmCashVoucherView
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
        lblTypeBadge = new Label();
        lblAccent = new Label();
        lblStatus = new Label();
        pnlDetails = new Panel();
        lblContraValue = new Label();
        lblContraLabel = new Label();
        lblCostCenterValue = new Label();
        lblCostCenterLabel = new Label();
        lblPartyTypeValue = new Label();
        lblPartyTypeLabel = new Label();
        lblTreasuryValue = new Label();
        lblTreasuryLabel = new Label();
        lblMethodValue = new Label();
        lblMethodLabel = new Label();
        lblTypeValue = new Label();
        lblTypeLabel = new Label();
        lblDateValue = new Label();
        lblDateLabel = new Label();
        lblVoucherNoValue = new Label();
        lblVoucherNoLabel = new Label();
        lblAmountValue = new Label();
        lblAmountLabel = new Label();
        pnlTafqit = new Panel();
        lblTafqitText = new Label();
        lblTafqitLabel = new Label();
        pnlNotes = new Panel();
        lblNotesText = new Label();
        lblNotesLabel = new Label();
        pnlFooter = new Panel();
        btnClose = new Button();
        btnVoid = new Button();
        btnPrint = new Button();
        lblFooterLine = new Label();
        pnlHeader.SuspendLayout();
        pnlDetails.SuspendLayout();
        pnlTafqit.SuspendLayout();
        pnlNotes.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblTypeBadge);
        pnlHeader.Controls.Add(lblAccent);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(520, 66);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.Anchor = AnchorStyles.Top;
        lblTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTitle.ForeColor = Color.White;
        lblTitle.Location = new Point(220, 10);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(160, 40);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "عرض سند";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTypeBadge
        // 
        lblTypeBadge.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblTypeBadge.BackColor = Color.FromArgb(46, 204, 113);
        lblTypeBadge.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTypeBadge.ForeColor = Color.White;
        lblTypeBadge.Location = new Point(12, 16);
        lblTypeBadge.Name = "lblTypeBadge";
        lblTypeBadge.Size = new Size(76, 30);
        lblTypeBadge.TabIndex = 2;
        lblTypeBadge.Text = "قبض";
        lblTypeBadge.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(383, 24);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(125, 22);
        lblMockBadge.TabIndex = 3;
        lblMockBadge.Text = "وضع التجربة (Mock)";
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(46, 204, 113);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 4;
        // 
        // lblStatus
        // 
        lblStatus.BackColor = Color.FromArgb(25, 135, 84);
        lblStatus.Font = new Font("Tahoma", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStatus.ForeColor = Color.White;
        lblStatus.Location = new Point(20, 76);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(480, 34);
        lblStatus.TabIndex = 5;
        lblStatus.Text = "الحالة: مرحل";
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlDetails
        // 
        pnlDetails.BackColor = Color.White;
        pnlDetails.BorderStyle = BorderStyle.FixedSingle;
        pnlDetails.Controls.Add(lblContraValue);
        pnlDetails.Controls.Add(lblContraLabel);
        pnlDetails.Controls.Add(lblCostCenterValue);
        pnlDetails.Controls.Add(lblCostCenterLabel);
        pnlDetails.Controls.Add(lblPartyTypeValue);
        pnlDetails.Controls.Add(lblPartyTypeLabel);
        pnlDetails.Controls.Add(lblTreasuryValue);
        pnlDetails.Controls.Add(lblTreasuryLabel);
        pnlDetails.Controls.Add(lblMethodValue);
        pnlDetails.Controls.Add(lblMethodLabel);
        pnlDetails.Controls.Add(lblTypeValue);
        pnlDetails.Controls.Add(lblTypeLabel);
        pnlDetails.Controls.Add(lblDateValue);
        pnlDetails.Controls.Add(lblDateLabel);
        pnlDetails.Controls.Add(lblVoucherNoValue);
        pnlDetails.Controls.Add(lblVoucherNoLabel);
        pnlDetails.Location = new Point(20, 118);
        pnlDetails.Name = "pnlDetails";
        pnlDetails.RightToLeft = RightToLeft.Yes;
        pnlDetails.Size = new Size(480, 240);
        pnlDetails.TabIndex = 6;
        // 
        // lblVoucherNoLabel
        // 
        lblVoucherNoLabel.AutoSize = true;
        lblVoucherNoLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblVoucherNoLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblVoucherNoLabel.Location = new Point(12, 14);
        lblVoucherNoLabel.Name = "lblVoucherNoLabel";
        lblVoucherNoLabel.Size = new Size(86, 24);
        lblVoucherNoLabel.TabIndex = 7;
        lblVoucherNoLabel.Text = "رقم السند";
        lblVoucherNoLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblVoucherNoValue
        // 
        lblVoucherNoValue.AutoEllipsis = true;
        lblVoucherNoValue.BackColor = Color.White;
        lblVoucherNoValue.BorderStyle = BorderStyle.FixedSingle;
        lblVoucherNoValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblVoucherNoValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblVoucherNoValue.Location = new Point(128, 12);
        lblVoucherNoValue.Name = "lblVoucherNoValue";
        lblVoucherNoValue.Size = new Size(120, 32);
        lblVoucherNoValue.TabIndex = 8;
        lblVoucherNoValue.Text = "1003";
        lblVoucherNoValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDateLabel
        // 
        lblDateLabel.AutoSize = true;
        lblDateLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDateLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblDateLabel.Location = new Point(240, 14);
        lblDateLabel.Name = "lblDateLabel";
        lblDateLabel.Size = new Size(56, 24);
        lblDateLabel.TabIndex = 9;
        lblDateLabel.Text = "التاريخ";
        lblDateLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDateValue
        // 
        lblDateValue.AutoEllipsis = true;
        lblDateValue.BackColor = Color.White;
        lblDateValue.BorderStyle = BorderStyle.FixedSingle;
        lblDateValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblDateValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblDateValue.Location = new Point(356, 12);
        lblDateValue.Name = "lblDateValue";
        lblDateValue.Size = new Size(116, 32);
        lblDateValue.TabIndex = 10;
        lblDateValue.Text = "15/09/2026";
        lblDateValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTypeLabel
        // 
        lblTypeLabel.AutoSize = true;
        lblTypeLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTypeLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTypeLabel.Location = new Point(12, 60);
        lblTypeLabel.Name = "lblTypeLabel";
        lblTypeLabel.Size = new Size(50, 24);
        lblTypeLabel.TabIndex = 11;
        lblTypeLabel.Text = "النوع";
        lblTypeLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTypeValue
        // 
        lblTypeValue.AutoEllipsis = true;
        lblTypeValue.BackColor = Color.White;
        lblTypeValue.BorderStyle = BorderStyle.FixedSingle;
        lblTypeValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblTypeValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblTypeValue.Location = new Point(128, 58);
        lblTypeValue.Name = "lblTypeValue";
        lblTypeValue.Size = new Size(120, 32);
        lblTypeValue.TabIndex = 12;
        lblTypeValue.Text = "قبض";
        lblTypeValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblMethodLabel
        // 
        lblMethodLabel.AutoSize = true;
        lblMethodLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMethodLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblMethodLabel.Location = new Point(240, 60);
        lblMethodLabel.Name = "lblMethodLabel";
        lblMethodLabel.Size = new Size(96, 24);
        lblMethodLabel.TabIndex = 13;
        lblMethodLabel.Text = "طريقة الدفع";
        lblMethodLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblMethodValue
        // 
        lblMethodValue.AutoEllipsis = true;
        lblMethodValue.BackColor = Color.White;
        lblMethodValue.BorderStyle = BorderStyle.FixedSingle;
        lblMethodValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblMethodValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblMethodValue.Location = new Point(356, 58);
        lblMethodValue.Name = "lblMethodValue";
        lblMethodValue.Size = new Size(116, 32);
        lblMethodValue.TabIndex = 14;
        lblMethodValue.Text = "نقداً";
        lblMethodValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTreasuryLabel
        // 
        lblTreasuryLabel.AutoSize = true;
        lblTreasuryLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTreasuryLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTreasuryLabel.Location = new Point(12, 106);
        lblTreasuryLabel.Name = "lblTreasuryLabel";
        lblTreasuryLabel.Size = new Size(113, 24);
        lblTreasuryLabel.TabIndex = 15;
        lblTreasuryLabel.Text = "حساب الخزينة";
        lblTreasuryLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTreasuryValue
        // 
        lblTreasuryValue.AutoEllipsis = true;
        lblTreasuryValue.BackColor = Color.White;
        lblTreasuryValue.BorderStyle = BorderStyle.FixedSingle;
        lblTreasuryValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblTreasuryValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblTreasuryValue.Location = new Point(128, 104);
        lblTreasuryValue.Name = "lblTreasuryValue";
        lblTreasuryValue.Size = new Size(340, 32);
        lblTreasuryValue.TabIndex = 16;
        lblTreasuryValue.Text = "الصندوق الرئيسي (111)";
        lblTreasuryValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblPartyTypeLabel
        // 
        lblPartyTypeLabel.AutoSize = true;
        lblPartyTypeLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPartyTypeLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblPartyTypeLabel.Location = new Point(12, 152);
        lblPartyTypeLabel.Name = "lblPartyTypeLabel";
        lblPartyTypeLabel.Size = new Size(80, 24);
        lblPartyTypeLabel.TabIndex = 17;
        lblPartyTypeLabel.Text = "نوع الطرف";
        lblPartyTypeLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblPartyTypeValue
        // 
        lblPartyTypeValue.AutoEllipsis = true;
        lblPartyTypeValue.BackColor = Color.White;
        lblPartyTypeValue.BorderStyle = BorderStyle.FixedSingle;
        lblPartyTypeValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblPartyTypeValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblPartyTypeValue.Location = new Point(128, 150);
        lblPartyTypeValue.Name = "lblPartyTypeValue";
        lblPartyTypeValue.Size = new Size(120, 32);
        lblPartyTypeValue.TabIndex = 18;
        lblPartyTypeValue.Text = "عميل";
        lblPartyTypeValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCostCenterLabel
        // 
        lblCostCenterLabel.AutoSize = true;
        lblCostCenterLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCostCenterLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblCostCenterLabel.Location = new Point(240, 152);
        lblCostCenterLabel.Name = "lblCostCenterLabel";
        lblCostCenterLabel.Size = new Size(105, 24);
        lblCostCenterLabel.TabIndex = 19;
        lblCostCenterLabel.Text = "مركز التكلفة";
        lblCostCenterLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCostCenterValue
        // 
        lblCostCenterValue.AutoEllipsis = true;
        lblCostCenterValue.BackColor = Color.White;
        lblCostCenterValue.BorderStyle = BorderStyle.FixedSingle;
        lblCostCenterValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblCostCenterValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblCostCenterValue.Location = new Point(356, 150);
        lblCostCenterValue.Name = "lblCostCenterValue";
        lblCostCenterValue.Size = new Size(116, 32);
        lblCostCenterValue.TabIndex = 20;
        lblCostCenterValue.Text = "CC-01";
        lblCostCenterValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblContraLabel
        // 
        lblContraLabel.AutoSize = true;
        lblContraLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblContraLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblContraLabel.Location = new Point(12, 198);
        lblContraLabel.Name = "lblContraLabel";
        lblContraLabel.Size = new Size(105, 24);
        lblContraLabel.TabIndex = 21;
        lblContraLabel.Text = "الحساب المقابل";
        lblContraLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblContraValue
        // 
        lblContraValue.AutoEllipsis = true;
        lblContraValue.BackColor = Color.White;
        lblContraValue.BorderStyle = BorderStyle.FixedSingle;
        lblContraValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblContraValue.ForeColor = Color.FromArgb(33, 37, 41);
        lblContraValue.Location = new Point(128, 196);
        lblContraValue.Name = "lblContraValue";
        lblContraValue.Size = new Size(340, 32);
        lblContraValue.TabIndex = 22;
        lblContraValue.Text = "2102 - ذمم عملاء";
        lblContraValue.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblAmountLabel
        // 
        lblAmountLabel.AutoSize = true;
        lblAmountLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblAmountLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblAmountLabel.Location = new Point(20, 372);
        lblAmountLabel.Name = "lblAmountLabel";
        lblAmountLabel.Size = new Size(59, 24);
        lblAmountLabel.TabIndex = 23;
        lblAmountLabel.Text = "المبلغ";
        lblAmountLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblAmountValue
        // 
        lblAmountValue.BackColor = Color.White;
        lblAmountValue.BorderStyle = BorderStyle.FixedSingle;
        lblAmountValue.Font = new Font("Tahoma", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblAmountValue.ForeColor = Color.FromArgb(46, 204, 113);
        lblAmountValue.Location = new Point(128, 368);
        lblAmountValue.Name = "lblAmountValue";
        lblAmountValue.Size = new Size(372, 40);
        lblAmountValue.TabIndex = 24;
        lblAmountValue.Text = "150,000 ريال يمني";
        lblAmountValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTafqitLabel
        // 
        lblTafqitLabel.AutoSize = true;
        lblTafqitLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTafqitLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblTafqitLabel.Location = new Point(20, 424);
        lblTafqitLabel.Name = "lblTafqitLabel";
        lblTafqitLabel.Size = new Size(59, 24);
        lblTafqitLabel.TabIndex = 25;
        lblTafqitLabel.Text = "التفقيط";
        lblTafqitLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlTafqit
        // 
        pnlTafqit.BackColor = Color.White;
        pnlTafqit.BorderStyle = BorderStyle.FixedSingle;
        pnlTafqit.Controls.Add(lblTafqitText);
        pnlTafqit.Location = new Point(128, 420);
        pnlTafqit.Name = "pnlTafqit";
        pnlTafqit.Size = new Size(372, 48);
        pnlTafqit.TabIndex = 26;
        // 
        // lblTafqitText
        // 
        lblTafqitText.Dock = DockStyle.Fill;
        lblTafqitText.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTafqitText.ForeColor = Color.FromArgb(46, 204, 113);
        lblTafqitText.Location = new Point(0, 0);
        lblTafqitText.Name = "lblTafqitText";
        lblTafqitText.RightToLeft = RightToLeft.Yes;
        lblTafqitText.Size = new Size(370, 46);
        lblTafqitText.TabIndex = 0;
        lblTafqitText.Text = "مائة وخمسون ألف ريال يمني فقط";
        lblTafqitText.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblNotesLabel
        // 
        lblNotesLabel.AutoSize = true;
        lblNotesLabel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNotesLabel.ForeColor = Color.FromArgb(52, 73, 94);
        lblNotesLabel.Location = new Point(20, 478);
        lblNotesLabel.Name = "lblNotesLabel";
        lblNotesLabel.Size = new Size(59, 24);
        lblNotesLabel.TabIndex = 27;
        lblNotesLabel.Text = "البيان";
        lblNotesLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlNotes
        // 
        pnlNotes.BackColor = Color.White;
        pnlNotes.BorderStyle = BorderStyle.FixedSingle;
        pnlNotes.Controls.Add(lblNotesText);
        pnlNotes.Location = new Point(128, 474);
        pnlNotes.Name = "pnlNotes";
        pnlNotes.Size = new Size(372, 72);
        pnlNotes.TabIndex = 28;
        // 
        // lblNotesText
        // 
        lblNotesText.Dock = DockStyle.Fill;
        lblNotesText.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblNotesText.ForeColor = Color.FromArgb(33, 37, 41);
        lblNotesText.Location = new Point(0, 0);
        lblNotesText.Name = "lblNotesText";
        lblNotesText.RightToLeft = RightToLeft.Yes;
        lblNotesText.Size = new Size(370, 70);
        lblNotesText.TabIndex = 0;
        lblNotesText.Text = "سداد دفعة نقدية من العميل أحمد محمد عن فاتورة مبيعات رقم INV-2026-0142.";
        lblNotesText.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(btnVoid);
        pnlFooter.Controls.Add(btnPrint);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(520, 66);
        pnlFooter.TabIndex = 29;
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
        btnPrint.Location = new Point(370, 12);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(122, 42);
        btnPrint.TabIndex = 30;
        btnPrint.Text = "طباعة";
        btnPrint.UseVisualStyleBackColor = false;
        btnPrint.Click += btnPrint_Click;
        // 
        // btnVoid
        // 
        btnVoid.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnVoid.BackColor = Color.FromArgb(220, 53, 69);
        btnVoid.FlatAppearance.BorderSize = 0;
        btnVoid.FlatStyle = FlatStyle.Flat;
        btnVoid.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnVoid.ForeColor = Color.White;
        btnVoid.Location = new Point(232, 12);
        btnVoid.Name = "btnVoid";
        btnVoid.Size = new Size(130, 42);
        btnVoid.TabIndex = 31;
        btnVoid.Text = "إلغاء السند";
        btnVoid.UseVisualStyleBackColor = false;
        btnVoid.Click += btnVoid_Click;
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
        btnClose.TabIndex = 32;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmCashVoucherView
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(520, 620);
        Controls.Add(pnlFooter);
        Controls.Add(pnlNotes);
        Controls.Add(lblNotesLabel);
        Controls.Add(pnlTafqit);
        Controls.Add(lblTafqitLabel);
        Controls.Add(lblAmountValue);
        Controls.Add(lblAmountLabel);
        Controls.Add(pnlDetails);
        Controls.Add(lblStatus);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCashVoucherView";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "عرض سند";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlDetails.ResumeLayout(false);
        pnlDetails.PerformLayout();
        pnlTafqit.ResumeLayout(false);
        pnlNotes.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblTitle;
    private Label lblTypeBadge;
    private Label lblMockBadge;
    private Label lblAccent;
    private Label lblStatus;
    private Panel pnlDetails;
    private Label lblVoucherNoLabel;
    private Label lblVoucherNoValue;
    private Label lblDateLabel;
    private Label lblDateValue;
    private Label lblTypeLabel;
    private Label lblTypeValue;
    private Label lblMethodLabel;
    private Label lblMethodValue;
    private Label lblTreasuryLabel;
    private Label lblTreasuryValue;
    private Label lblPartyTypeLabel;
    private Label lblPartyTypeValue;
    private Label lblContraLabel;
    private Label lblContraValue;
    private Label lblCostCenterLabel;
    private Label lblCostCenterValue;
    private Label lblAmountLabel;
    private Label lblAmountValue;
    private Label lblTafqitLabel;
    private Panel pnlTafqit;
    private Label lblTafqitText;
    private Label lblNotesLabel;
    private Panel pnlNotes;
    private Label lblNotesText;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnPrint;
    private Button btnVoid;
    private Button btnClose;
}