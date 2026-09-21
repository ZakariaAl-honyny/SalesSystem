namespace SalesSystem.Desktop.Screens.Accounting.Forms;

partial class frmJournalEntryNew
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlHeader = new Panel();
        lblMockBadge = new Label();
        lblMode = new Label();
        lblAccent = new Label();
        labelEntryNumber = new Label();
        txtEntryNumber = new TextBox();
        labelDate = new Label();
        dtpDate = new DateTimePicker();
        labelDescription = new Label();
        txtDescription = new TextBox();
        dgvLines = new DataGridView();
        cmsGrid = new ContextMenuStrip(components);
        tsmiDeleteRow = new ToolStripMenuItem();
        pnlBalance = new Panel();
        lblBalanceStatus = new Label();
        lblDiff = new Label();
        lblDiffCaption = new Label();
        lblTotalCredit = new Label();
        lblTotalCreditCaption = new Label();
        lblTotalDebit = new Label();
        lblTotalDebitCaption = new Label();
        pnlFooter = new Panel();
        btnCancel = new Button();
        btnPost = new Button();
        btnSaveDraft = new Button();
        lblFooterLine = new Label();
        pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        cmsGrid.SuspendLayout();
        pnlBalance.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Controls.Add(lblAccent);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(920, 66);
        pnlHeader.TabIndex = 0;
        // 
        // lblMockBadge
        // 
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(12, 24);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(317, 22);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(310, 12);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(300, 40);
        lblMode.TabIndex = 1;
        lblMode.Text = "قيد يومي جديد";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(25, 135, 84);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Location = new Point(0, 62);
        lblAccent.Name = "lblAccent";
        lblAccent.Size = new Size(920, 4);
        lblAccent.TabIndex = 3;
        // 
        // labelEntryNumber
        // 
        labelEntryNumber.AutoSize = true;
        labelEntryNumber.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelEntryNumber.ForeColor = Color.FromArgb(52, 73, 94);
        labelEntryNumber.Location = new Point(20, 88);
        labelEntryNumber.Name = "labelEntryNumber";
        labelEntryNumber.Size = new Size(102, 24);
        labelEntryNumber.TabIndex = 0;
        labelEntryNumber.Text = "رقم القيد:";
        // 
        // txtEntryNumber
        // 
        txtEntryNumber.BackColor = SystemColors.Control;
        txtEntryNumber.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtEntryNumber.Location = new Point(130, 84);
        txtEntryNumber.Name = "txtEntryNumber";
        txtEntryNumber.ReadOnly = true;
        txtEntryNumber.RightToLeft = RightToLeft.Yes;
        txtEntryNumber.Size = new Size(120, 32);
        txtEntryNumber.TabIndex = 1;
        txtEntryNumber.Text = "جديد";
        // 
        // labelDate
        // 
        labelDate.AutoSize = true;
        labelDate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelDate.ForeColor = Color.FromArgb(52, 73, 94);
        labelDate.Location = new Point(300, 88);
        labelDate.Name = "labelDate";
        labelDate.Size = new Size(77, 24);
        labelDate.TabIndex = 2;
        labelDate.Text = "التاريخ:";
        // 
        // dtpDate
        // 
        dtpDate.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpDate.Format = DateTimePickerFormat.Short;
        dtpDate.Location = new Point(390, 84);
        dtpDate.Name = "dtpDate";
        dtpDate.RightToLeft = RightToLeft.Yes;
        dtpDate.Size = new Size(150, 32);
        dtpDate.TabIndex = 3;
        // 
        // labelDescription
        // 
        labelDescription.AutoSize = true;
        labelDescription.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        labelDescription.ForeColor = Color.FromArgb(52, 73, 94);
        labelDescription.Location = new Point(20, 136);
        labelDescription.Name = "labelDescription";
        labelDescription.Size = new Size(69, 24);
        labelDescription.TabIndex = 4;
        labelDescription.Text = "البيان:";
        // 
        // txtDescription
        // 
        txtDescription.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtDescription.Location = new Point(130, 132);
        txtDescription.Name = "txtDescription";
        txtDescription.RightToLeft = RightToLeft.Yes;
        txtDescription.Size = new Size(430, 32);
        txtDescription.TabIndex = 5;
        // 
        // dgvLines
        // 
        dgvLines.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.White;
        dgvLines.ColumnHeadersHeight = 40;
        dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvLines.ContextMenuStrip = cmsGrid;
        dgvLines.Location = new Point(20, 180);
        dgvLines.Name = "dgvLines";
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowHeadersWidth = 62;
        dgvLines.RowTemplate.Height = 28;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.Size = new Size(880, 270);
        dgvLines.TabIndex = 6;
        dgvLines.CellValidating += dgvLines_CellValidating;
        dgvLines.CellValueChanged += dgvLines_CellValueChanged;
        dgvLines.CurrentCellDirtyStateChanged += dgvLines_CurrentCellDirtyStateChanged;
        dgvLines.DataError += dgvLines_DataError;
        // 
        // cmsGrid
        // 
        cmsGrid.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsGrid.ImageScalingSize = new Size(24, 24);
        cmsGrid.Items.AddRange(new ToolStripItem[] { tsmiDeleteRow });
        cmsGrid.Name = "cmsGrid";
        cmsGrid.RightToLeft = RightToLeft.Yes;
        cmsGrid.Size = new Size(257, 34);
        // 
        // tsmiDeleteRow
        // 
        tsmiDeleteRow.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        tsmiDeleteRow.Name = "tsmiDeleteRow";
        tsmiDeleteRow.Size = new Size(256, 30);
        tsmiDeleteRow.Text = "حذف السطر المحدد";
        tsmiDeleteRow.Click += tsmiDeleteRow_Click;
        // 
        // pnlBalance
        // 
        pnlBalance.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlBalance.BackColor = Color.White;
        pnlBalance.BorderStyle = BorderStyle.FixedSingle;
        pnlBalance.Controls.Add(lblBalanceStatus);
        pnlBalance.Controls.Add(lblDiff);
        pnlBalance.Controls.Add(lblDiffCaption);
        pnlBalance.Controls.Add(lblTotalCredit);
        pnlBalance.Controls.Add(lblTotalCreditCaption);
        pnlBalance.Controls.Add(lblTotalDebit);
        pnlBalance.Controls.Add(lblTotalDebitCaption);
        pnlBalance.Location = new Point(20, 458);
        pnlBalance.Name = "pnlBalance";
        pnlBalance.Size = new Size(880, 71);
        pnlBalance.TabIndex = 7;
        // 
        // lblBalanceStatus
        // 
        lblBalanceStatus.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblBalanceStatus.ForeColor = Color.FromArgb(25, 135, 84);
        lblBalanceStatus.Location = new Point(12, 32);
        lblBalanceStatus.Name = "lblBalanceStatus";
        lblBalanceStatus.Size = new Size(696, 37);
        lblBalanceStatus.TabIndex = 6;
        lblBalanceStatus.Text = "متوازن ✓";
        lblBalanceStatus.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDiff
        // 
        lblDiff.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDiff.ForeColor = Color.FromArgb(52, 73, 94);
        lblDiff.Location = new Point(652, 8);
        lblDiff.Name = "lblDiff";
        lblDiff.Size = new Size(56, 24);
        lblDiff.TabIndex = 5;
        lblDiff.Text = "0.00";
        lblDiff.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblDiffCaption
        // 
        lblDiffCaption.AutoSize = true;
        lblDiffCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblDiffCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblDiffCaption.Location = new Point(600, 8);
        lblDiffCaption.Name = "lblDiffCaption";
        lblDiffCaption.Size = new Size(71, 24);
        lblDiffCaption.TabIndex = 4;
        lblDiffCaption.Text = "الفرق:";
        // 
        // lblTotalCredit
        // 
        lblTotalCredit.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalCredit.ForeColor = Color.FromArgb(52, 73, 94);
        lblTotalCredit.Location = new Point(438, 8);
        lblTotalCredit.Name = "lblTotalCredit";
        lblTotalCredit.Size = new Size(120, 24);
        lblTotalCredit.TabIndex = 3;
        lblTotalCredit.Text = "0.00";
        lblTotalCredit.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTotalCreditCaption
        // 
        lblTotalCreditCaption.AutoSize = true;
        lblTotalCreditCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalCreditCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblTotalCreditCaption.Location = new Point(310, 8);
        lblTotalCreditCaption.Name = "lblTotalCreditCaption";
        lblTotalCreditCaption.Size = new Size(143, 24);
        lblTotalCreditCaption.TabIndex = 2;
        lblTotalCreditCaption.Text = "مجموع الدائن:";
        // 
        // lblTotalDebit
        // 
        lblTotalDebit.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalDebit.ForeColor = Color.FromArgb(52, 73, 94);
        lblTotalDebit.Location = new Point(140, 8);
        lblTotalDebit.Name = "lblTotalDebit";
        lblTotalDebit.Size = new Size(120, 24);
        lblTotalDebit.TabIndex = 1;
        lblTotalDebit.Text = "0.00";
        lblTotalDebit.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblTotalDebitCaption
        // 
        lblTotalDebitCaption.AutoSize = true;
        lblTotalDebitCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotalDebitCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblTotalDebitCaption.Location = new Point(12, 8);
        lblTotalDebitCaption.Name = "lblTotalDebitCaption";
        lblTotalDebitCaption.Size = new Size(150, 24);
        lblTotalDebitCaption.TabIndex = 0;
        lblTotalDebitCaption.Text = "مجموع المدين:";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnCancel);
        pnlFooter.Controls.Add(btnPost);
        pnlFooter.Controls.Add(btnSaveDraft);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 554);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(920, 66);
        pnlFooter.TabIndex = 8;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.White;
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCancel.ForeColor = Color.FromArgb(52, 73, 94);
        btnCancel.Location = new Point(12, 12);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(110, 42);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "إلغاء";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Click += btnCancel_Click;
        // 
        // btnPost
        // 
        btnPost.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPost.BackColor = Color.FromArgb(25, 135, 84);
        btnPost.Cursor = Cursors.Hand;
        btnPost.FlatAppearance.BorderSize = 0;
        btnPost.FlatStyle = FlatStyle.Flat;
        btnPost.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnPost.ForeColor = Color.White;
        btnPost.Location = new Point(680, 12);
        btnPost.Name = "btnPost";
        btnPost.Size = new Size(170, 42);
        btnPost.TabIndex = 10;
        btnPost.Text = "ترحيل";
        btnPost.UseVisualStyleBackColor = false;
        btnPost.Click += btnPost_Click;
        // 
        // btnSaveDraft
        // 
        btnSaveDraft.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSaveDraft.BackColor = Color.FromArgb(13, 110, 253);
        btnSaveDraft.Cursor = Cursors.Hand;
        btnSaveDraft.FlatAppearance.BorderSize = 0;
        btnSaveDraft.FlatStyle = FlatStyle.Flat;
        btnSaveDraft.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSaveDraft.ForeColor = Color.White;
        btnSaveDraft.Location = new Point(490, 12);
        btnSaveDraft.Name = "btnSaveDraft";
        btnSaveDraft.Size = new Size(170, 42);
        btnSaveDraft.TabIndex = 9;
        btnSaveDraft.Text = "حفظ كمسودة";
        btnSaveDraft.UseVisualStyleBackColor = false;
        btnSaveDraft.Click += btnSaveDraft_Click;
        // 
        // lblFooterLine
        // 
        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Location = new Point(0, 0);
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.Size = new Size(920, 2);
        lblFooterLine.TabIndex = 0;
        // 
        // frmJournalEntryNew
        // 
        AcceptButton = btnPost;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnCancel;
        ClientSize = new Size(920, 620);
        Controls.Add(dgvLines);
        Controls.Add(pnlBalance);
        Controls.Add(labelDescription);
        Controls.Add(txtDescription);
        Controls.Add(labelDate);
        Controls.Add(dtpDate);
        Controls.Add(labelEntryNumber);
        Controls.Add(txtEntryNumber);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmJournalEntryNew";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "قيد يومي جديد";
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        cmsGrid.ResumeLayout(false);
        pnlBalance.ResumeLayout(false);
        pnlBalance.PerformLayout();
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMode;
    private Label lblMockBadge;
    private Label lblAccent;
    private Label labelEntryNumber;
    private TextBox txtEntryNumber;
    private Label labelDate;
    private DateTimePicker dtpDate;
    private Label labelDescription;
    private TextBox txtDescription;
    private DataGridView dgvLines;
    private DataGridViewComboBoxColumn colAccount;
    private DataGridViewTextBoxColumn colDebit;
    private DataGridViewTextBoxColumn colCredit;
    private DataGridViewComboBoxColumn colCostCenter;
    private DataGridViewTextBoxColumn colNotes;
    private Panel pnlBalance;
    private Label lblTotalDebitCaption;
    private Label lblTotalDebit;
    private Label lblTotalCreditCaption;
    private Label lblTotalCredit;
    private Label lblDiffCaption;
    private Label lblDiff;
    private Label lblBalanceStatus;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnSaveDraft;
    private Button btnPost;
    private Button btnCancel;
    private ContextMenuStrip cmsGrid;
    private ToolStripMenuItem tsmiDeleteRow;
}