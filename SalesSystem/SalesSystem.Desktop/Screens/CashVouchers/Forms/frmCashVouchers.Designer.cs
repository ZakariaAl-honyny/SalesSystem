namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

partial class frmCashVouchers
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsVouchers = new ContextMenuStrip(components);
        previewToolStripMenuItem = new ToolStripMenuItem();
        printToolStripMenuItem = new ToolStripMenuItem();
        viewToolStripMenuItem = new ToolStripMenuItem();
        cancelToolStripMenuItem = new ToolStripMenuItem();
        pnlHeader = new Panel();
        lblMode = new Label();
        lblMockBadge = new Label();
        pnlStats = new Panel();
        flowCards = new FlowLayoutPanel();
        cardReceipts = new Panel();
        cardReceiptsAccent = new Label();
        cardReceiptsTitle = new Label();
        cardReceiptsValue = new Label();
        cardPayments = new Panel();
        cardPaymentsAccent = new Label();
        cardPaymentsTitle = new Label();
        cardPaymentsValue = new Label();
        cardNet = new Panel();
        cardNetAccent = new Label();
        cardNetTitle = new Label();
        cardNetValue = new Label();
        pnlFilter = new Panel();
        flowFilter = new FlowLayoutPanel();
        lblSearch = new Label();
        txtSearch = new TextBox();
        lblType = new Label();
        cbType = new ComboBox();
        lblMethod = new Label();
        cbMethod = new ComboBox();
        lblStatus = new Label();
        cbStatus = new ComboBox();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnAdd = new Button();
        btnClose = new Button();
        label1 = new Label();
        lblRecordsCount = new Label();
        dgvVouchers = new DataGridView();
        cmsVouchers.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlStats.SuspendLayout();
        flowCards.SuspendLayout();
        cardReceipts.SuspendLayout();
        cardPayments.SuspendLayout();
        cardNet.SuspendLayout();
        pnlFilter.SuspendLayout();
        flowFilter.SuspendLayout();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVouchers).BeginInit();
        SuspendLayout();
        // 
        // cmsVouchers
        // 
        cmsVouchers.BackColor = Color.White;
        cmsVouchers.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsVouchers.ImageScalingSize = new Size(24, 24);
        cmsVouchers.Items.AddRange(new ToolStripItem[] { previewToolStripMenuItem, printToolStripMenuItem, viewToolStripMenuItem, cancelToolStripMenuItem });
        cmsVouchers.Name = "cmsVouchers";
        cmsVouchers.RightToLeft = RightToLeft.Yes;
        cmsVouchers.Size = new Size(220, 124);
        cmsVouchers.Opening += cmsVouchers_Opening;
        // 
        // previewToolStripMenuItem
        // 
        previewToolStripMenuItem.Name = "previewToolStripMenuItem";
        previewToolStripMenuItem.Size = new Size(219, 30);
        previewToolStripMenuItem.Text = "معاينة السند";
        previewToolStripMenuItem.Click += previewToolStripMenuItem_Click;
        // 
        // printToolStripMenuItem
        // 
        printToolStripMenuItem.Name = "printToolStripMenuItem";
        printToolStripMenuItem.Size = new Size(219, 30);
        printToolStripMenuItem.Text = "طباعة السند";
        printToolStripMenuItem.Click += printToolStripMenuItem_Click;
        // 
        // viewToolStripMenuItem
        // 
        viewToolStripMenuItem.Name = "viewToolStripMenuItem";
        viewToolStripMenuItem.Size = new Size(219, 30);
        viewToolStripMenuItem.Text = "عرض التفاصيل";
        viewToolStripMenuItem.Click += viewToolStripMenuItem_Click;
        // 
        // cancelToolStripMenuItem
        // 
        cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
        cancelToolStripMenuItem.Size = new Size(219, 30);
        cancelToolStripMenuItem.Text = "إلغاء السند";
        cancelToolStripMenuItem.Click += cancelToolStripMenuItem_Click;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1150, 60);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(345, 8);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(460, 44);
        lblMode.TabIndex = 1;
        lblMode.Text = "سندات القبض والصرف والخزينة";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(14, 21);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(248, 22);
        lblMockBadge.TabIndex = 2;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
        // 
        // pnlStats
        // 
        pnlStats.BackColor = Color.WhiteSmoke;
        pnlStats.Controls.Add(flowCards);
        pnlStats.Dock = DockStyle.Top;
        pnlStats.Name = "pnlStats";
        pnlStats.Size = new Size(1150, 128);
        pnlStats.TabIndex = 1;
        // 
        // flowCards
        // 
        flowCards.Controls.Add(cardReceipts);
        flowCards.Controls.Add(cardPayments);
        flowCards.Controls.Add(cardNet);
        flowCards.Dock = DockStyle.Fill;
        flowCards.FlowDirection = FlowDirection.RightToLeft;
        flowCards.Name = "flowCards";
        flowCards.Padding = new Padding(12, 10, 12, 10);
        // 
        // cardReceipts
        // 
        cardReceipts.BackColor = Color.White;
        cardReceipts.Controls.Add(cardReceiptsAccent);
        cardReceipts.Controls.Add(cardReceiptsTitle);
        cardReceipts.Controls.Add(cardReceiptsValue);
        cardReceipts.Margin = new Padding(14, 10, 14, 10);
        cardReceipts.Name = "cardReceipts";
        cardReceipts.Padding = new Padding(15, 8, 15, 8);
        cardReceipts.Size = new Size(340, 88);
        cardReceipts.TabIndex = 0;
        // 
        // cardReceiptsAccent
        // 
        cardReceiptsAccent.BackColor = Color.FromArgb(46, 204, 113);
        cardReceiptsAccent.Dock = DockStyle.Bottom;
        cardReceiptsAccent.Height = 5;
        cardReceiptsAccent.Name = "cardReceiptsAccent";
        cardReceiptsAccent.TabIndex = 0;
        // 
        // cardReceiptsTitle
        // 
        cardReceiptsTitle.AutoSize = true;
        cardReceiptsTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardReceiptsTitle.ForeColor = Color.FromArgb(127, 140, 141);
        cardReceiptsTitle.Location = new Point(15, 8);
        cardReceiptsTitle.Name = "cardReceiptsTitle";
        cardReceiptsTitle.Size = new Size(194, 27);
        cardReceiptsTitle.TabIndex = 1;
        cardReceiptsTitle.Text = "إجمالي سندات القبض";
        // 
        // cardReceiptsValue
        // 
        cardReceiptsValue.AutoSize = true;
        cardReceiptsValue.Font = new Font("Tahoma", 19F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardReceiptsValue.ForeColor = Color.FromArgb(46, 204, 113);
        cardReceiptsValue.Location = new Point(15, 38);
        cardReceiptsValue.Name = "cardReceiptsValue";
        cardReceiptsValue.Size = new Size(200, 46);
        cardReceiptsValue.TabIndex = 2;
        cardReceiptsValue.Text = "+ 0.00 YER";
        // 
        // cardPayments
        // 
        cardPayments.BackColor = Color.White;
        cardPayments.Controls.Add(cardPaymentsAccent);
        cardPayments.Controls.Add(cardPaymentsTitle);
        cardPayments.Controls.Add(cardPaymentsValue);
        cardPayments.Margin = new Padding(14, 10, 14, 10);
        cardPayments.Name = "cardPayments";
        cardPayments.Padding = new Padding(15, 8, 15, 8);
        cardPayments.Size = new Size(340, 88);
        cardPayments.TabIndex = 1;
        // 
        // cardPaymentsAccent
        // 
        cardPaymentsAccent.BackColor = Color.FromArgb(231, 76, 60);
        cardPaymentsAccent.Dock = DockStyle.Bottom;
        cardPaymentsAccent.Height = 5;
        cardPaymentsAccent.Name = "cardPaymentsAccent";
        cardPaymentsAccent.TabIndex = 0;
        // 
        // cardPaymentsTitle
        // 
        cardPaymentsTitle.AutoSize = true;
        cardPaymentsTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardPaymentsTitle.ForeColor = Color.FromArgb(127, 140, 141);
        cardPaymentsTitle.Location = new Point(15, 8);
        cardPaymentsTitle.Name = "cardPaymentsTitle";
        cardPaymentsTitle.Size = new Size(189, 27);
        cardPaymentsTitle.TabIndex = 1;
        cardPaymentsTitle.Text = "إجمالي سندات الصرف";
        // 
        // cardPaymentsValue
        // 
        cardPaymentsValue.AutoSize = true;
        cardPaymentsValue.Font = new Font("Tahoma", 19F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardPaymentsValue.ForeColor = Color.FromArgb(231, 76, 60);
        cardPaymentsValue.Location = new Point(15, 38);
        cardPaymentsValue.Name = "cardPaymentsValue";
        cardPaymentsValue.Size = new Size(200, 46);
        cardPaymentsValue.TabIndex = 2;
        cardPaymentsValue.Text = "− 0.00 YER";
        // 
        // cardNet
        // 
        cardNet.BackColor = Color.White;
        cardNet.Controls.Add(cardNetAccent);
        cardNet.Controls.Add(cardNetTitle);
        cardNet.Controls.Add(cardNetValue);
        cardNet.Margin = new Padding(14, 10, 14, 10);
        cardNet.Name = "cardNet";
        cardNet.Padding = new Padding(15, 8, 15, 8);
        cardNet.Size = new Size(340, 88);
        cardNet.TabIndex = 2;
        // 
        // cardNetAccent
        // 
        cardNetAccent.BackColor = Color.FromArgb(52, 152, 219);
        cardNetAccent.Dock = DockStyle.Bottom;
        cardNetAccent.Height = 5;
        cardNetAccent.Name = "cardNetAccent";
        cardNetAccent.TabIndex = 0;
        // 
        // cardNetTitle
        // 
        cardNetTitle.AutoSize = true;
        cardNetTitle.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardNetTitle.ForeColor = Color.FromArgb(127, 140, 141);
        cardNetTitle.Location = new Point(15, 8);
        cardNetTitle.Name = "cardNetTitle";
        cardNetTitle.Size = new Size(203, 27);
        cardNetTitle.TabIndex = 1;
        cardNetTitle.Text = "صافي التدفق النقدي";
        // 
        // cardNetValue
        // 
        cardNetValue.AutoSize = true;
        cardNetValue.Font = new Font("Tahoma", 19F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardNetValue.ForeColor = Color.FromArgb(52, 152, 219);
        cardNetValue.Location = new Point(15, 38);
        cardNetValue.Name = "cardNetValue";
        cardNetValue.Size = new Size(190, 46);
        cardNetValue.TabIndex = 2;
        cardNetValue.Text = "0.00 YER";
        // 
        // pnlFilter
        // 
        pnlFilter.BackColor = Color.White;
        pnlFilter.Controls.Add(flowFilter);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(1150, 64);
        pnlFilter.TabIndex = 2;
        // 
        // flowFilter
        // 
        flowFilter.Controls.Add(lblSearch);
        flowFilter.Controls.Add(txtSearch);
        flowFilter.Controls.Add(lblType);
        flowFilter.Controls.Add(cbType);
        flowFilter.Controls.Add(lblMethod);
        flowFilter.Controls.Add(cbMethod);
        flowFilter.Controls.Add(lblStatus);
        flowFilter.Controls.Add(cbStatus);
        flowFilter.Dock = DockStyle.Fill;
        flowFilter.FlowDirection = FlowDirection.RightToLeft;
        flowFilter.Name = "flowFilter";
        flowFilter.Padding = new Padding(10, 5, 10, 5);
        flowFilter.WrapContents = false;
        // 
        // lblSearch
        // 
        lblSearch.AutoSize = true;
        lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSearch.ForeColor = Color.FromArgb(52, 73, 94);
        lblSearch.Location = new Point(3, 18);
        lblSearch.Margin = new Padding(6, 14, 4, 14);
        lblSearch.Name = "lblSearch";
        lblSearch.Size = new Size(50, 24);
        lblSearch.TabIndex = 0;
        lblSearch.Text = "بحث:";
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtSearch.Location = new Point(63, 13);
        txtSearch.Margin = new Padding(14, 12, 4, 12);
        txtSearch.Name = "txtSearch";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(240, 30);
        txtSearch.TabIndex = 1;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // lblType
        // 
        lblType.AutoSize = true;
        lblType.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblType.ForeColor = Color.FromArgb(52, 73, 94);
        lblType.Location = new Point(317, 18);
        lblType.Margin = new Padding(6, 14, 4, 14);
        lblType.Name = "lblType";
        lblType.Size = new Size(52, 24);
        lblType.TabIndex = 2;
        lblType.Text = "النوع:";
        // 
        // cbType
        // 
        cbType.DropDownStyle = ComboBoxStyle.DropDownList;
        cbType.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbType.Items.AddRange(new object[] { "الكل", "قبض", "صرف", "تحويل" });
        cbType.Location = new Point(379, 14);
        cbType.Margin = new Padding(14, 14, 4, 14);
        cbType.Name = "cbType";
        cbType.Size = new Size(110, 30);
        cbType.TabIndex = 3;
        cbType.SelectedIndexChanged += cbType_SelectedIndexChanged;
        // 
        // lblMethod
        // 
        lblMethod.AutoSize = true;
        lblMethod.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMethod.ForeColor = Color.FromArgb(52, 73, 94);
        lblMethod.Location = new Point(503, 18);
        lblMethod.Margin = new Padding(6, 14, 4, 14);
        lblMethod.Name = "lblMethod";
        lblMethod.Size = new Size(66, 24);
        lblMethod.TabIndex = 4;
        lblMethod.Text = "الطريقة:";
        // 
        // cbMethod
        // 
        cbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
        cbMethod.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbMethod.Items.AddRange(new object[] { "الكل", "نقد", "بنك", "محفظة" });
        cbMethod.Location = new Point(583, 14);
        cbMethod.Margin = new Padding(14, 14, 4, 14);
        cbMethod.Name = "cbMethod";
        cbMethod.Size = new Size(110, 30);
        cbMethod.TabIndex = 5;
        cbMethod.SelectedIndexChanged += cbMethod_SelectedIndexChanged;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
        lblStatus.Location = new Point(707, 18);
        lblStatus.Margin = new Padding(6, 14, 4, 14);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(58, 24);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "الحالة:";
        // 
        // cbStatus
        // 
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbStatus.Items.AddRange(new object[] { "الكل", "مرحل", "مسودة", "ملغي" });
        cbStatus.Location = new Point(779, 14);
        cbStatus.Margin = new Padding(14, 14, 4, 14);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(110, 30);
        cbStatus.TabIndex = 7;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(248, 249, 250);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(btnAdd);
        pnlFooter.Controls.Add(label1);
        pnlFooter.Controls.Add(lblRecordsCount);
        pnlFooter.Controls.Add(lblFooterLine);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1150, 62);
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
        // btnAdd
        // 
        btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnAdd.BackColor = Color.FromArgb(25, 135, 84);
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.FlatStyle = FlatStyle.Flat;
        btnAdd.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnAdd.ForeColor = Color.White;
        btnAdd.Location = new Point(990, 10);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(150, 42);
        btnAdd.TabIndex = 1;
        btnAdd.Text = "+ إضافة سند";
        btnAdd.UseVisualStyleBackColor = false;
        btnAdd.Click += btnAdd_Click;
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
        btnClose.Location = new Point(12, 10);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(110, 42);
        btnClose.TabIndex = 2;
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
        label1.Location = new Point(850, 20);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(115, 24);
        label1.TabIndex = 3;
        label1.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(52, 73, 94);
        lblRecordsCount.Location = new Point(790, 20);
        lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(45, 24);
        lblRecordsCount.TabIndex = 0;
        lblRecordsCount.Text = "[0]";
        // 
        // dgvVouchers
        // 
        dgvVouchers.AllowUserToAddRows = false;
        dgvVouchers.AllowUserToDeleteRows = false;
        dgvVouchers.AllowUserToResizeRows = false;
        dgvVouchers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvVouchers.BackgroundColor = Color.FromArgb(30, 30, 30);
        dgvVouchers.BorderStyle = BorderStyle.None;
        dgvVouchers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvVouchers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvVouchers.ColumnHeadersHeight = 46;
        dgvVouchers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvVouchers.ContextMenuStrip = cmsVouchers;
        dgvVouchers.Dock = DockStyle.Fill;
        dgvVouchers.EnableHeadersVisualStyles = false;
        dgvVouchers.GridColor = Color.FromArgb(64, 64, 64);
        dgvVouchers.MultiSelect = false;
        dgvVouchers.Name = "dgvVouchers";
        dgvVouchers.ReadOnly = true;
        dgvVouchers.RowHeadersVisible = false;
        dgvVouchers.RowTemplate.Height = 36;
        dgvVouchers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvVouchers.Size = new Size(1150, 366);
        dgvVouchers.TabIndex = 4;
        dgvVouchers.TabStop = false;
        dgvVouchers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 43, 54);
        dgvVouchers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvVouchers.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dgvVouchers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvVouchers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(33, 43, 54);
        dgvVouchers.DefaultCellStyle.BackColor = Color.FromArgb(44, 44, 44);
        dgvVouchers.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
        dgvVouchers.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dgvVouchers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
        dgvVouchers.DefaultCellStyle.SelectionForeColor = Color.White;
        dgvVouchers.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        dgvVouchers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 38, 38);
        dgvVouchers.CellContentClick += dgvVouchers_CellContentClick;
        dgvVouchers.CellFormatting += dgvVouchers_CellFormatting;
        dgvVouchers.DoubleClick += dgvVouchers_DoubleClick;
        // 
        // frmCashVouchers
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(1150, 680);
        Controls.Add(dgvVouchers);
        Controls.Add(pnlFooter);
        Controls.Add(pnlFilter);
        Controls.Add(pnlStats);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmCashVouchers";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "سندات القبض والصرف والخزينة";
        Load += frmCashVouchers_Load;
        cmsVouchers.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlStats.ResumeLayout(false);
        flowCards.ResumeLayout(false);
        cardReceipts.ResumeLayout(false);
        cardReceipts.PerformLayout();
        cardPayments.ResumeLayout(false);
        cardPayments.PerformLayout();
        cardNet.ResumeLayout(false);
        cardNet.PerformLayout();
        pnlFilter.ResumeLayout(false);
        flowFilter.ResumeLayout(false);
        flowFilter.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvVouchers).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMode;
    private Label lblMockBadge;
    private Panel pnlStats;
    private FlowLayoutPanel flowCards;
    private Panel cardReceipts;
    private Label cardReceiptsAccent;
    private Label cardReceiptsTitle;
    private Label cardReceiptsValue;
    private Panel cardPayments;
    private Label cardPaymentsAccent;
    private Label cardPaymentsTitle;
    private Label cardPaymentsValue;
    private Panel cardNet;
    private Label cardNetAccent;
    private Label cardNetTitle;
    private Label cardNetValue;
    private Panel pnlFilter;
    private FlowLayoutPanel flowFilter;
    private Label lblSearch;
    private TextBox txtSearch;
    private Label lblType;
    private ComboBox cbType;
    private Label lblMethod;
    private ComboBox cbMethod;
    private Label lblStatus;
    private ComboBox cbStatus;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnAdd;
    private Button btnClose;
    private Label label1;
    private Label lblRecordsCount;
    private DataGridView dgvVouchers;
    private ContextMenuStrip cmsVouchers;
    private ToolStripMenuItem previewToolStripMenuItem;
    private ToolStripMenuItem printToolStripMenuItem;
    private ToolStripMenuItem viewToolStripMenuItem;
    private ToolStripMenuItem cancelToolStripMenuItem;
}