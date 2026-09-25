namespace SalesSystem.Desktop.Screens.Accounting.Forms;

partial class frmJournalEntries
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

        // ── Controls ───────────────────────────────────────────────────
        pnlHeader = new Panel();
        lblMockBadge = new Label();
        lblMode = new Label();
        lblAccent = new Label();

        pnlToolbar = new Panel();
        btnAddNew = new Button();
        btnRefresh = new Button();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblRecordsLabel = new Label();
        lblRecordsCount = new Label();

        dgvEntries = new DataGridView();

        splitContainer = new SplitContainer();
        dgvLines = new DataGridView();

        pnlDetailTotals = new Panel();
        lblDetailDebitCaption = new Label();
        lblDetailDebit = new Label();
        lblDetailCreditCaption = new Label();
        lblDetailCredit = new Label();
        lblDetailStatus = new Label();

        pnlFooter = new Panel();
        btnClose = new Button();
        lblFooterLine = new Label();

        cmsEntries = new ContextMenuStrip(components);
        tsmiRefresh = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        tsmiViewDetails = new ToolStripMenuItem();
        tsmiEdit = new ToolStripMenuItem();
        tsmiAddNew = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        tsmiPost = new ToolStripMenuItem();
        tsmiCancel = new ToolStripMenuItem();

        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvEntries).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
        pnlDetailTotals.SuspendLayout();
        pnlFooter.SuspendLayout();
        cmsEntries.SuspendLayout();
        SuspendLayout();

        // ═══════════════════════════════════════════════════════════════
        // رأس النموذج — Header Panel
        // ═══════════════════════════════════════════════════════════════

        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Controls.Add(lblAccent);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1200, 66);
        pnlHeader.TabIndex = 0;

        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(12, 24);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(290, 22);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";

        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(420, 12);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(360, 40);
        lblMode.TabIndex = 1;
        lblMode.Text = "القيود اليومية";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;

        lblAccent.BackColor = Color.FromArgb(25, 135, 84);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Location = new Point(0, 62);
        lblAccent.Name = "lblAccent";
        lblAccent.Size = new Size(1200, 4);
        lblAccent.TabIndex = 3;

        // ═══════════════════════════════════════════════════════════════
        // شريط الأدوات — Toolbar
        // ═══════════════════════════════════════════════════════════════

        pnlToolbar.BackColor = Color.White;
        pnlToolbar.Controls.Add(btnAddNew);
        pnlToolbar.Controls.Add(btnRefresh);
        pnlToolbar.Controls.Add(label2);
        pnlToolbar.Controls.Add(cbFilterBy);
        pnlToolbar.Controls.Add(txtFilterValue);
        pnlToolbar.Controls.Add(lblRecordsLabel);
        pnlToolbar.Controls.Add(lblRecordsCount);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 66);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1200, 60);
        pnlToolbar.TabIndex = 1;

        // btnAddNew
        btnAddNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAddNew.BackColor = Color.FromArgb(25, 135, 84);
        btnAddNew.Cursor = Cursors.Hand;
        btnAddNew.FlatAppearance.BorderSize = 0;
        btnAddNew.FlatStyle = FlatStyle.Flat;
        btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        btnAddNew.ForeColor = Color.White;
        btnAddNew.Location = new Point(1090, 10);
        btnAddNew.Name = "btnAddNew";
        btnAddNew.Size = new Size(100, 40);
        btnAddNew.TabIndex = 0;
        btnAddNew.Text = "+ جديد";
        btnAddNew.UseVisualStyleBackColor = false;
        btnAddNew.Click += btnAddNew_Click;

        // btnRefresh
        btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRefresh.BackColor = Color.White;
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 9F);
        btnRefresh.ForeColor = Color.FromArgb(52, 73, 94);
        btnRefresh.Location = new Point(990, 10);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(90, 40);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;

        // label2
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(52, 73, 94);
        label2.Location = new Point(12, 18);
        label2.Name = "label2";
        label2.Size = new Size(80, 24);
        label2.TabIndex = 2;
        label2.Text = "البحث:";

        // cbFilterBy
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.Font = new Font("Tahoma", 10F);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم القيد", "البيان", "نوع القيد", "الحالة" });
        cbFilterBy.Location = new Point(100, 14);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(180, 32);
        cbFilterBy.TabIndex = 3;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;

        // txtFilterValue
        txtFilterValue.Font = new Font("Tahoma", 10F);
        txtFilterValue.Location = new Point(290, 14);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.Size = new Size(350, 32);
        txtFilterValue.TabIndex = 4;
        txtFilterValue.Visible = false;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyPress += txtFilterValue_KeyPress;

        // lblRecordsLabel
        lblRecordsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsLabel.AutoSize = true;
        lblRecordsLabel.Font = new Font("Tahoma", 9F);
        lblRecordsLabel.ForeColor = Color.FromArgb(108, 117, 125);
        lblRecordsLabel.Location = new Point(920, 36);
        lblRecordsLabel.Name = "lblRecordsLabel";
        lblRecordsLabel.Size = new Size(50, 18);
        lblRecordsLabel.TabIndex = 5;
        lblRecordsLabel.Text = "العدد:";

        // lblRecordsCount
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblRecordsCount.ForeColor = Color.FromArgb(52, 73, 94);
        lblRecordsCount.Location = new Point(870, 36);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(30, 18);
        lblRecordsCount.TabIndex = 6;
        lblRecordsCount.Text = "[0]";

        // ═══════════════════════════════════════════════════════════════
        // الشبكة الرئيسية + شبكة البنود — SplitContainer
        // ═══════════════════════════════════════════════════════════════

        splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        splitContainer.Location = new Point(12, 132);
        splitContainer.Name = "splitContainer";
        splitContainer.Orientation = Orientation.Horizontal;
        splitContainer.Size = new Size(1176, 530);
        splitContainer.SplitterDistance = 300;
        splitContainer.TabIndex = 2;

        // ── الأعلى: القيود ─────────────────────────────────────────────

        dgvEntries.AllowUserToAddRows = false;
        dgvEntries.AllowUserToDeleteRows = false;
        dgvEntries.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvEntries.BackgroundColor = Color.White;
        dgvEntries.BorderStyle = BorderStyle.None;
        dgvEntries.ColumnHeadersHeight = 45;
        dgvEntries.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvEntries.ContextMenuStrip = cmsEntries;
        dgvEntries.Dock = DockStyle.Fill;
        dgvEntries.MultiSelect = false;
        dgvEntries.Name = "dgvEntries";
        dgvEntries.ReadOnly = true;
        dgvEntries.RowHeadersVisible = false;
        dgvEntries.RowTemplate.Height = 30;
        dgvEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEntries.TabIndex = 0;
        dgvEntries.SelectionChanged += dgvEntries_SelectionChanged;
        dgvEntries.DoubleClick += dgvEntries_DoubleClick;

        splitContainer.Panel1.Controls.Add(dgvEntries);

        // ── الأسفل: البنود + المجاميع ─────────────────────────────────

        dgvLines.AllowUserToAddRows = false;
        dgvLines.AllowUserToDeleteRows = false;
        dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvLines.BackgroundColor = Color.White;
        dgvLines.BorderStyle = BorderStyle.None;
        dgvLines.ColumnHeadersHeight = 40;
        dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvLines.Dock = DockStyle.Fill;
        dgvLines.MultiSelect = false;
        dgvLines.Name = "dgvLines";
        dgvLines.ReadOnly = true;
        dgvLines.RowHeadersVisible = false;
        dgvLines.RowTemplate.Height = 28;
        dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvLines.TabIndex = 0;

        pnlDetailTotals.BackColor = Color.FromArgb(248, 249, 250);
        pnlDetailTotals.Controls.Add(lblDetailDebitCaption);
        pnlDetailTotals.Controls.Add(lblDetailDebit);
        pnlDetailTotals.Controls.Add(lblDetailCreditCaption);
        pnlDetailTotals.Controls.Add(lblDetailCredit);
        pnlDetailTotals.Controls.Add(lblDetailStatus);
        pnlDetailTotals.Dock = DockStyle.Bottom;
        pnlDetailTotals.Location = new Point(0, 180);
        pnlDetailTotals.Name = "pnlDetailTotals";
        pnlDetailTotals.Size = new Size(1176, 46);
        pnlDetailTotals.TabIndex = 1;

        lblDetailDebitCaption.AutoSize = true;
        lblDetailDebitCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDetailDebitCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblDetailDebitCaption.Location = new Point(12, 12);
        lblDetailDebitCaption.Name = "lblDetailDebitCaption";
        lblDetailDebitCaption.Size = new Size(95, 24);
        lblDetailDebitCaption.TabIndex = 0;
        lblDetailDebitCaption.Text = "مجموع المدين:";

        lblDetailDebit.AutoSize = true;
        lblDetailDebit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDetailDebit.ForeColor = Color.FromArgb(52, 73, 94);
        lblDetailDebit.Location = new Point(115, 12);
        lblDetailDebit.Name = "lblDetailDebit";
        lblDetailDebit.Size = new Size(45, 24);
        lblDetailDebit.TabIndex = 1;
        lblDetailDebit.Text = "0.00";

        lblDetailCreditCaption.AutoSize = true;
        lblDetailCreditCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDetailCreditCaption.ForeColor = Color.FromArgb(108, 117, 125);
        lblDetailCreditCaption.Location = new Point(220, 12);
        lblDetailCreditCaption.Name = "lblDetailCreditCaption";
        lblDetailCreditCaption.Size = new Size(97, 24);
        lblDetailCreditCaption.TabIndex = 2;
        lblDetailCreditCaption.Text = "مجموع الدائن:";

        lblDetailCredit.AutoSize = true;
        lblDetailCredit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDetailCredit.ForeColor = Color.FromArgb(52, 73, 94);
        lblDetailCredit.Location = new Point(325, 12);
        lblDetailCredit.Name = "lblDetailCredit";
        lblDetailCredit.Size = new Size(45, 24);
        lblDetailCredit.TabIndex = 3;
        lblDetailCredit.Text = "0.00";

        lblDetailStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblDetailStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold);
        lblDetailStatus.ForeColor = Color.FromArgb(25, 135, 84);
        lblDetailStatus.Location = new Point(800, 12);
        lblDetailStatus.Name = "lblDetailStatus";
        lblDetailStatus.Size = new Size(364, 24);
        lblDetailStatus.TabIndex = 4;
        lblDetailStatus.Text = "";
        lblDetailStatus.TextAlign = ContentAlignment.MiddleRight;

        splitContainer.Panel2.Controls.Add(dgvLines);
        splitContainer.Panel2.Controls.Add(pnlDetailTotals);

        // ═══════════════════════════════════════════════════════════════
        // ذيل النموذج — Footer
        // ═══════════════════════════════════════════════════════════════

        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 668);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1200, 55);
        pnlFooter.TabIndex = 3;

        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 11F, FontStyle.Bold);
        btnClose.ForeColor = Color.FromArgb(52, 73, 94);
        btnClose.Location = new Point(12, 8);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 40);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;

        lblFooterLine.BackColor = Color.FromArgb(222, 226, 230);
        lblFooterLine.Dock = DockStyle.Top;
        lblFooterLine.Location = new Point(0, 0);
        lblFooterLine.Name = "lblFooterLine";
        lblFooterLine.Size = new Size(1200, 2);
        lblFooterLine.TabIndex = 1;

        // ═══════════════════════════════════════════════════════════════
        // قائمة السياق — Context Menu
        // ═══════════════════════════════════════════════════════════════

        cmsEntries.BackColor = Color.White;
        cmsEntries.Font = new Font("Tahoma", 10F);
        cmsEntries.ImageScalingSize = new Size(24, 24);
        cmsEntries.Items.AddRange(new ToolStripItem[]
        {
            tsmiRefresh, toolStripSeparator1,
            tsmiViewDetails, tsmiEdit, tsmiAddNew, toolStripSeparator2,
            tsmiPost, tsmiCancel
        });
        cmsEntries.Name = "cmsEntries";
        cmsEntries.RightToLeft = RightToLeft.Yes;
        cmsEntries.Size = new Size(220, 200);
        cmsEntries.Opening += cmsEntries_Opening;

        tsmiRefresh.Name = "tsmiRefresh";
        tsmiRefresh.ShortcutKeys = Keys.Control | Keys.R;
        tsmiRefresh.Size = new Size(219, 30);
        tsmiRefresh.Text = "&تحديث";
        tsmiRefresh.Click += tsmiRefresh_Click;

        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(216, 6);

        tsmiViewDetails.Name = "tsmiViewDetails";
        tsmiViewDetails.Size = new Size(219, 30);
        tsmiViewDetails.Text = "&عرض التفاصيل";
        tsmiViewDetails.Click += tsmiViewDetails_Click;

        tsmiEdit.Name = "tsmiEdit";
        tsmiEdit.Size = new Size(219, 30);
        tsmiEdit.Text = "&تعديل";
        tsmiEdit.Click += tsmiEdit_Click;

        tsmiAddNew.Name = "tsmiAddNew";
        tsmiAddNew.Size = new Size(219, 30);
        tsmiAddNew.Text = "&إضافة قيد جديد";
        tsmiAddNew.Click += tsmiAddNew_Click;

        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(216, 6);

        tsmiPost.Name = "tsmiPost";
        tsmiPost.Size = new Size(219, 30);
        tsmiPost.Text = "&ترحيل";
        tsmiPost.Click += tsmiPost_Click;

        tsmiCancel.Name = "tsmiCancel";
        tsmiCancel.Size = new Size(219, 30);
        tsmiCancel.Text = "&إلغاء";
        tsmiCancel.Click += tsmiCancel_Click;

        // ═══════════════════════════════════════════════════════════════
        // النموذج — Form
        // ═══════════════════════════════════════════════════════════════

        AcceptButton = btnAddNew;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1200, 723);
        Controls.Add(splitContainer);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmJournalEntries";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "القيود اليومية";
        Load += frmJournalEntries_Load;

        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvEntries).EndInit();
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
        pnlDetailTotals.ResumeLayout(false);
        pnlDetailTotals.PerformLayout();
        pnlFooter.ResumeLayout(false);
        cmsEntries.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    // ── Header ────────────────────────────────────────────
    private Panel pnlHeader;
    private Label lblMockBadge;
    private Label lblMode;
    private Label lblAccent;

    // ── Toolbar ───────────────────────────────────────────
    private Panel pnlToolbar;
    private Button btnAddNew;
    private Button btnRefresh;
    private Label label2;
    private ComboBox cbFilterBy;
    private TextBox txtFilterValue;
    private Label lblRecordsLabel;
    private Label lblRecordsCount;

    // ── Grids (split) ─────────────────────────────────────
    private SplitContainer splitContainer;
    private DataGridView dgvEntries;
    private DataGridView dgvLines;

    // ── Detail Totals ─────────────────────────────────────
    private Panel pnlDetailTotals;
    private Label lblDetailDebitCaption;
    private Label lblDetailDebit;
    private Label lblDetailCreditCaption;
    private Label lblDetailCredit;
    private Label lblDetailStatus;

    // ── Footer ────────────────────────────────────────────
    private Panel pnlFooter;
    private Button btnClose;
    private Label lblFooterLine;

    // ── Context Menu ──────────────────────────────────────
    private ContextMenuStrip cmsEntries;
    private ToolStripMenuItem tsmiRefresh;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem tsmiViewDetails;
    private ToolStripMenuItem tsmiEdit;
    private ToolStripMenuItem tsmiAddNew;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem tsmiPost;
    private ToolStripMenuItem tsmiCancel;
}
