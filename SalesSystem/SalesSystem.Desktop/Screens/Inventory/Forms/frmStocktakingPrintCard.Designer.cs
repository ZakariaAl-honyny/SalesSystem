namespace SalesSystem.Desktop.Screens.Inventory.Forms;

partial class frmStocktakingPrintCard
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
        lblCardTitle = new Label();
        tblFields = new TableLayoutPanel();
        lblBalanceValue = new Label();
        lblBalanceLabel = new Label();
        lblWarehouseValue = new Label();
        lblWarehouseLabel = new Label();
        lblBarcodeValue = new Label();
        lblBarcodeLabel = new Label();
        lblProductNameValue = new Label();
        lblProductNameLabel = new Label();
        lblMovementsTitle = new Label();
        dgvMovements = new DataGridView();
        colDate = new DataGridViewTextBoxColumn();
        colType = new DataGridViewTextBoxColumn();
        colInQty = new DataGridViewTextBoxColumn();
        colOutQty = new DataGridViewTextBoxColumn();
        colBalance = new DataGridViewTextBoxColumn();
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
        ((System.ComponentModel.ISupportInitialize)dgvMovements).BeginInit();
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
        pnlHeader.Size = new Size(480, 46);
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
        lblTitle.Size = new Size(480, 42);
        lblTitle.TabIndex = 1;
        lblTitle.Text = "طباعة كرت صنف";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(241), Convert.ToInt32(196), Convert.ToInt32(15));
        lblMockBadge.Location = new Point(356, 13);
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
        pnlPreviewCard.Controls.Add(dgvMovements);
        pnlPreviewCard.Controls.Add(lblMovementsTitle);
        pnlPreviewCard.Controls.Add(tblFields);
        pnlPreviewCard.Controls.Add(pnlCardHeader);
        pnlPreviewCard.Location = new Point(20, 56);
        pnlPreviewCard.Name = "pnlPreviewCard";
        pnlPreviewCard.RightToLeft = RightToLeft.Yes;
        pnlPreviewCard.Size = new Size(440, 424);
        pnlPreviewCard.TabIndex = 1;
        // 
        // pnlCardHeader
        // 
        pnlCardHeader.BackColor = System.Drawing.Color.White;
        pnlCardHeader.Controls.Add(lblCardTitle);
        pnlCardHeader.Controls.Add(lblStoreName);
        pnlCardHeader.Location = new Point(0, 0);
        pnlCardHeader.Name = "pnlCardHeader";
        pnlCardHeader.Size = new Size(438, 74);
        pnlCardHeader.TabIndex = 0;
        // 
        // lblStoreName
        // 
        lblStoreName.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStoreName.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblStoreName.Location = new Point(0, 0);
        lblStoreName.Name = "lblStoreName";
        lblStoreName.Size = new Size(438, 30);
        lblStoreName.TabIndex = 0;
        lblStoreName.Text = "متجر النور التجاري";
        lblStoreName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblCardTitle
        // 
        lblCardTitle.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCardTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblCardTitle.Location = new Point(0, 30);
        lblCardTitle.Name = "lblCardTitle";
        lblCardTitle.Size = new Size(438, 40);
        lblCardTitle.TabIndex = 1;
        lblCardTitle.Text = "كرت حركة صنف";
        lblCardTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tblFields
        // 
        tblFields.BackColor = System.Drawing.Color.White;
        tblFields.ColumnCount = 2;
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblFields.Controls.Add(lblBalanceValue, 1, 3);
        tblFields.Controls.Add(lblBalanceLabel, 0, 3);
        tblFields.Controls.Add(lblWarehouseValue, 1, 2);
        tblFields.Controls.Add(lblWarehouseLabel, 0, 2);
        tblFields.Controls.Add(lblBarcodeValue, 1, 1);
        tblFields.Controls.Add(lblBarcodeLabel, 0, 1);
        tblFields.Controls.Add(lblProductNameValue, 1, 0);
        tblFields.Controls.Add(lblProductNameLabel, 0, 0);
        tblFields.Location = new Point(0, 74);
        tblFields.Name = "tblFields";
        tblFields.Padding = new Padding(12, 4, 12, 4);
        tblFields.RightToLeft = RightToLeft.Yes;
        tblFields.RowCount = 4;
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        tblFields.Size = new Size(438, 112);
        tblFields.TabIndex = 1;
        // 
        // lblProductNameLabel
        // 
        lblProductNameLabel.AutoSize = true;
        lblProductNameLabel.Dock = DockStyle.Fill;
        lblProductNameLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblProductNameLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblProductNameLabel.Location = new Point(15, 7);
        lblProductNameLabel.Name = "lblProductNameLabel";
        lblProductNameLabel.Size = new Size(104, 20);
        lblProductNameLabel.TabIndex = 0;
        lblProductNameLabel.Text = "الصنف";
        lblProductNameLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblProductNameValue
        // 
        lblProductNameValue.AutoEllipsis = true;
        lblProductNameValue.AutoSize = true;
        lblProductNameValue.Dock = DockStyle.Fill;
        lblProductNameValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblProductNameValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblProductNameValue.Location = new Point(125, 7);
        lblProductNameValue.Name = "lblProductNameValue";
        lblProductNameValue.Size = new Size(298, 20);
        lblProductNameValue.TabIndex = 1;
        lblProductNameValue.Text = "زيت نباتي 1 لتر";
        lblProductNameValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblBarcodeLabel
        // 
        lblBarcodeLabel.AutoSize = true;
        lblBarcodeLabel.Dock = DockStyle.Fill;
        lblBarcodeLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBarcodeLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblBarcodeLabel.Location = new Point(15, 33);
        lblBarcodeLabel.Name = "lblBarcodeLabel";
        lblBarcodeLabel.Size = new Size(104, 20);
        lblBarcodeLabel.TabIndex = 2;
        lblBarcodeLabel.Text = "الباركود";
        lblBarcodeLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblBarcodeValue
        // 
        lblBarcodeValue.AutoEllipsis = true;
        lblBarcodeValue.AutoSize = true;
        lblBarcodeValue.Dock = DockStyle.Fill;
        lblBarcodeValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblBarcodeValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblBarcodeValue.Location = new Point(125, 33);
        lblBarcodeValue.Name = "lblBarcodeValue";
        lblBarcodeValue.Size = new Size(298, 20);
        lblBarcodeValue.TabIndex = 3;
        lblBarcodeValue.Text = "6281001234567";
        lblBarcodeValue.TextAlign = ContentAlignment.MiddleLeft;
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
        lblWarehouseValue.AutoSize = true;
        lblWarehouseValue.Dock = DockStyle.Fill;
        lblWarehouseValue.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblWarehouseValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblWarehouseValue.Location = new Point(125, 59);
        lblWarehouseValue.Name = "lblWarehouseValue";
        lblWarehouseValue.Size = new Size(298, 20);
        lblWarehouseValue.TabIndex = 5;
        lblWarehouseValue.Text = "المستودع الرئيسي";
        lblWarehouseValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblBalanceLabel
        // 
        lblBalanceLabel.AutoSize = true;
        lblBalanceLabel.Dock = DockStyle.Fill;
        lblBalanceLabel.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBalanceLabel.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(73), Convert.ToInt32(94));
        lblBalanceLabel.Location = new Point(15, 85);
        lblBalanceLabel.Name = "lblBalanceLabel";
        lblBalanceLabel.Size = new Size(104, 20);
        lblBalanceLabel.TabIndex = 6;
        lblBalanceLabel.Text = "الرصيد الحالي";
        lblBalanceLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblBalanceValue
        // 
        lblBalanceValue.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(250), Convert.ToInt32(240));
        lblBalanceValue.BorderStyle = BorderStyle.FixedSingle;
        lblBalanceValue.Dock = DockStyle.Fill;
        lblBalanceValue.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBalanceValue.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(25), Convert.ToInt32(135), Convert.ToInt32(84));
        lblBalanceValue.Location = new Point(125, 85);
        lblBalanceValue.Name = "lblBalanceValue";
        lblBalanceValue.Size = new Size(298, 20);
        lblBalanceValue.TabIndex = 7;
        lblBalanceValue.Text = "157.500";
        lblBalanceValue.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblMovementsTitle
        // 
        lblMovementsTitle.BackColor = System.Drawing.Color.White;
        lblMovementsTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMovementsTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblMovementsTitle.Location = new Point(0, 186);
        lblMovementsTitle.Name = "lblMovementsTitle";
        lblMovementsTitle.Size = new Size(438, 26);
        lblMovementsTitle.TabIndex = 2;
        lblMovementsTitle.Text = "حركات الصنف";
        lblMovementsTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dgvMovements
        // 
        // RULE-N134/N140 — الثيم الأخضر: قيم حرفية مطابقة لما تُطبّقه الشبكة وقت التشغيل.
        // الارتفاعات (32 للرأس / 25 للصف) تبقى كما هي ليدخل عدد الحركات كاملاً في كرت المعاينة.
        dgvMovements.AllowUserToAddRows = false;
        dgvMovements.AllowUserToDeleteRows = false;
        dgvMovements.AllowUserToResizeColumns = false;
        dgvMovements.AllowUserToResizeRows = false;
        dgvMovements.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        dgvMovements.AutoGenerateColumns = false;
        dgvMovements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvMovements.BackgroundColor = System.Drawing.Color.White;
        dgvMovements.BorderStyle = BorderStyle.None;
        dgvMovements.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvMovements.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvMovements.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(58), Convert.ToInt32(64));
        dgvMovements.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dgvMovements.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
        dgvMovements.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(52), Convert.ToInt32(58), Convert.ToInt32(64));
        dgvMovements.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
        dgvMovements.ColumnHeadersHeight = 32;
        dgvMovements.Columns.AddRange(new DataGridViewColumn[] { colDate, colType, colInQty, colOutQty, colBalance });
        dgvMovements.DefaultCellStyle.BackColor = System.Drawing.Color.White;
        dgvMovements.DefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dgvMovements.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dgvMovements.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(243), Convert.ToInt32(205));
        dgvMovements.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dgvMovements.EnableHeadersVisualStyles = false;
        dgvMovements.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
        dgvMovements.Location = new Point(0, 212);
        dgvMovements.MultiSelect = false;
        dgvMovements.Name = "dgvMovements";
        dgvMovements.ReadOnly = true;
        dgvMovements.RightToLeft = RightToLeft.Yes;
        dgvMovements.RowHeadersVisible = false;
        dgvMovements.RowTemplate.Height = 25;
        dgvMovements.ScrollBars = ScrollBars.None;
        dgvMovements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvMovements.Size = new Size(438, 158);
        dgvMovements.TabIndex = 3;
        dgvMovements.TabStop = false;
        // 
        // colDate
        // 
        colDate.DataPropertyName = "Date";
        colDate.DefaultCellStyle.Format = "dd/MM/yyyy";
        colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colDate.FillWeight = 10F;
        colDate.HeaderText = "التاريخ";
        colDate.MinimumWidth = 60;
        colDate.Name = "colDate";
        colDate.ReadOnly = true;
        // 
        // colType
        // 
        colType.DataPropertyName = "Type";
        colType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colType.FillWeight = 22F;
        colType.HeaderText = "النوع";
        colType.MinimumWidth = 120;
        colType.Name = "colType";
        colType.ReadOnly = true;
        // 
        // colInQty
        // 
        colInQty.DataPropertyName = "InQty";
        colInQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colInQty.DefaultCellStyle.Format = "N3";
        colInQty.FillWeight = 10F;
        colInQty.HeaderText = "الوارد";
        colInQty.MinimumWidth = 60;
        colInQty.Name = "colInQty";
        colInQty.ReadOnly = true;
        // 
        // colOutQty
        // 
        colOutQty.DataPropertyName = "OutQty";
        colOutQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colOutQty.DefaultCellStyle.Format = "N3";
        colOutQty.FillWeight = 10F;
        colOutQty.HeaderText = "الصادر";
        colOutQty.MinimumWidth = 60;
        colOutQty.Name = "colOutQty";
        colOutQty.ReadOnly = true;
        // 
        // colBalance
        // 
        colBalance.DataPropertyName = "Balance";
        colBalance.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colBalance.DefaultCellStyle.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        colBalance.DefaultCellStyle.Format = "N3";
        colBalance.FillWeight = 12F;
        colBalance.HeaderText = "الرصيد";
        colBalance.MinimumWidth = 80;
        colBalance.Name = "colBalance";
        colBalance.ReadOnly = true;
        // 
        // pnlDashedSeparator
        // 
        pnlDashedSeparator.BackColor = System.Drawing.Color.White;
        pnlDashedSeparator.Location = new Point(0, 370);
        pnlDashedSeparator.Name = "pnlDashedSeparator";
        pnlDashedSeparator.Size = new Size(438, 14);
        pnlDashedSeparator.TabIndex = 4;
        pnlDashedSeparator.Paint += pnlDashedSeparator_Paint;
        // 
        // lblCardFooter
        // 
        lblCardFooter.BackColor = System.Drawing.Color.White;
        lblCardFooter.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCardFooter.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        lblCardFooter.Location = new Point(0, 384);
        lblCardFooter.Name = "lblCardFooter";
        lblCardFooter.Size = new Size(438, 40);
        lblCardFooter.TabIndex = 5;
        lblCardFooter.Text = "توقيع أمين المستودع: ....................    توقيع مدقق المخزون: ....................";
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
        pnlFooter.Size = new Size(480, 66);
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
        btnPrint.Location = new Point(328, 12);
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
        // frmStocktakingPrintCard
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
        CancelButton = btnClose;
        ClientSize = new Size(480, 560);
        Controls.Add(pnlFooter);
        Controls.Add(pnlPreviewCard);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmStocktakingPrintCard";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "طباعة كرت صنف";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlPreviewCard.ResumeLayout(false);
        pnlCardHeader.ResumeLayout(false);
        tblFields.ResumeLayout(false);
        tblFields.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvMovements).EndInit();
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
    private Label lblCardTitle;
    private TableLayoutPanel tblFields;
    private Label lblProductNameLabel;
    private Label lblProductNameValue;
    private Label lblBarcodeLabel;
    private Label lblBarcodeValue;
    private Label lblWarehouseLabel;
    private Label lblWarehouseValue;
    private Label lblBalanceLabel;
    private Label lblBalanceValue;
    private Label lblMovementsTitle;
    private DataGridView dgvMovements;
    private DataGridViewTextBoxColumn colDate;
    private DataGridViewTextBoxColumn colType;
    private DataGridViewTextBoxColumn colInQty;
    private DataGridViewTextBoxColumn colOutQty;
    private DataGridViewTextBoxColumn colBalance;
    private Panel pnlDashedSeparator;
    private Label lblCardFooter;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnPrint;
    private Button btnClose;
}
