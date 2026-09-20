namespace SalesSystem.Desktop.Screens.CashVouchers.Forms;

partial class frmCashVoucherAdvances
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cmsAdvances = new ContextMenuStrip(components);
        viewToolStripMenuItem = new ToolStripMenuItem();
        pnlHeader = new Panel();
        lblMockBadge = new Label();
        lblMode = new Label();
        lblAccent = new Label();
        pnlStats = new Panel();
        flowCards = new FlowLayoutPanel();
        cardAdvances = new Panel();
        cardAdvancesTitle = new Label();
        cardAdvancesValue = new Label();
        cardAdvancesAccent = new Label();
        cardTrusts = new Panel();
        cardTrustsTitle = new Label();
        cardTrustsValue = new Label();
        cardTrustsAccent = new Label();
        cardEmployees = new Panel();
        cardEmployeesTitle = new Label();
        cardEmployeesValue = new Label();
        cardEmployeesAccent = new Label();
        pnlFilter = new Panel();
        flowFilter = new FlowLayoutPanel();
        lblSearch = new Label();
        txtSearch = new TextBox();
        lblStatus = new Label();
        cbStatus = new ComboBox();
        pnlFooter = new Panel();
        lblFooterLine = new Label();
        btnAdd = new Button();
        btnClose = new Button();
        label1 = new Label();
        lblRecordsCount = new Label();
        dgvAdvances = new DataGridView();
        cmsAdvances.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlStats.SuspendLayout();
        flowCards.SuspendLayout();
        cardAdvances.SuspendLayout();
        cardTrusts.SuspendLayout();
        cardEmployees.SuspendLayout();
        pnlFilter.SuspendLayout();
        flowFilter.SuspendLayout();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAdvances).BeginInit();
        SuspendLayout();
        // 
        // cmsAdvances
        // 
        cmsAdvances.BackColor = Color.White;
        cmsAdvances.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsAdvances.ImageScalingSize = new Size(24, 24);
        cmsAdvances.Items.AddRange(new ToolStripItem[] { viewToolStripMenuItem });
        cmsAdvances.Name = "cmsAdvances";
        cmsAdvances.RightToLeft = RightToLeft.Yes;
        cmsAdvances.Size = new Size(160, 36);
        cmsAdvances.Opening += cmsAdvances_Opening;
        // 
        // viewToolStripMenuItem
        // 
        viewToolStripMenuItem.Name = "viewToolStripMenuItem";
        viewToolStripMenuItem.Size = new Size(159, 32);
        viewToolStripMenuItem.Text = "عرض التفاصيل";
        viewToolStripMenuItem.Click += viewToolStripMenuItem_Click;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(33, 43, 54);
        pnlHeader.Controls.Add(lblMockBadge);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Controls.Add(lblAccent);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(620, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMockBadge
        // 
        lblMockBadge.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblMockBadge.AutoSize = true;
        lblMockBadge.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMockBadge.ForeColor = Color.FromArgb(241, 196, 15);
        lblMockBadge.Location = new Point(12, 17);
        lblMockBadge.Name = "lblMockBadge";
        lblMockBadge.Size = new Size(248, 22);
        lblMockBadge.TabIndex = 1;
        lblMockBadge.Text = "وضع التجربة (Mock) — بيانات وهمية";
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(120, 9);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(380, 38);
        lblMode.TabIndex = 2;
        lblMode.Text = "سلف وعهد الموظفين";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAccent
        // 
        lblAccent.BackColor = Color.FromArgb(0, 120, 215);
        lblAccent.Dock = DockStyle.Bottom;
        lblAccent.Height = 4;
        lblAccent.Name = "lblAccent";
        lblAccent.TabIndex = 3;
        // 
        // pnlStats
        // 
        pnlStats.BackColor = Color.WhiteSmoke;
        pnlStats.Controls.Add(flowCards);
        pnlStats.Dock = DockStyle.Top;
        pnlStats.Name = "pnlStats";
        pnlStats.Size = new Size(620, 126);
        pnlStats.TabIndex = 1;
        // 
        // flowCards
        // 
        flowCards.Controls.Add(cardAdvances);
        flowCards.Controls.Add(cardTrusts);
        flowCards.Controls.Add(cardEmployees);
        flowCards.Dock = DockStyle.Fill;
        flowCards.FlowDirection = FlowDirection.RightToLeft;
        flowCards.Name = "flowCards";
        flowCards.Padding = new Padding(12, 10, 12, 10);
        // 
        // cardAdvances
        // 
        cardAdvances.BackColor = Color.White;
        cardAdvances.Controls.Add(cardAdvancesTitle);
        cardAdvances.Controls.Add(cardAdvancesValue);
        cardAdvances.Controls.Add(cardAdvancesAccent);
        cardAdvances.Margin = new Padding(8, 9, 8, 9);
        cardAdvances.Name = "cardAdvances";
        cardAdvances.Size = new Size(182, 88);
        cardAdvances.TabIndex = 0;
        // 
        // cardAdvancesTitle
        // 
        cardAdvancesTitle.Dock = DockStyle.Top;
        cardAdvancesTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardAdvancesTitle.ForeColor = Color.FromArgb(127, 140, 141);
        cardAdvancesTitle.Height = 36;
        cardAdvancesTitle.Name = "cardAdvancesTitle";
        cardAdvancesTitle.TabIndex = 0;
        cardAdvancesTitle.Text = "إجمالي السلف المستحقة";
        cardAdvancesTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cardAdvancesValue
        // 
        cardAdvancesValue.Dock = DockStyle.Fill;
        cardAdvancesValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardAdvancesValue.ForeColor = Color.FromArgb(231, 76, 60);
        cardAdvancesValue.Name = "cardAdvancesValue";
        cardAdvancesValue.TabIndex = 1;
        cardAdvancesValue.Text = "0 YER";
        cardAdvancesValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cardAdvancesAccent
        // 
        cardAdvancesAccent.BackColor = Color.FromArgb(231, 76, 60);
        cardAdvancesAccent.Dock = DockStyle.Bottom;
        cardAdvancesAccent.Height = 5;
        cardAdvancesAccent.Name = "cardAdvancesAccent";
        cardAdvancesAccent.TabIndex = 2;
        // 
        // cardTrusts
        // 
        cardTrusts.BackColor = Color.White;
        cardTrusts.Controls.Add(cardTrustsTitle);
        cardTrusts.Controls.Add(cardTrustsValue);
        cardTrusts.Controls.Add(cardTrustsAccent);
        cardTrusts.Margin = new Padding(8, 9, 8, 9);
        cardTrusts.Name = "cardTrusts";
        cardTrusts.Size = new Size(182, 88);
        cardTrusts.TabIndex = 1;
        // 
        // cardTrustsTitle
        // 
        cardTrustsTitle.Dock = DockStyle.Top;
        cardTrustsTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardTrustsTitle.ForeColor = Color.FromArgb(127, 140, 141);
        cardTrustsTitle.Height = 36;
        cardTrustsTitle.Name = "cardTrustsTitle";
        cardTrustsTitle.TabIndex = 0;
        cardTrustsTitle.Text = "إجمالي العهد المفتوحة";
        cardTrustsTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cardTrustsValue
        // 
        cardTrustsValue.Dock = DockStyle.Fill;
        cardTrustsValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardTrustsValue.ForeColor = Color.FromArgb(52, 152, 219);
        cardTrustsValue.Name = "cardTrustsValue";
        cardTrustsValue.TabIndex = 1;
        cardTrustsValue.Text = "0 YER";
        cardTrustsValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cardTrustsAccent
        // 
        cardTrustsAccent.BackColor = Color.FromArgb(52, 152, 219);
        cardTrustsAccent.Dock = DockStyle.Bottom;
        cardTrustsAccent.Height = 5;
        cardTrustsAccent.Name = "cardTrustsAccent";
        cardTrustsAccent.TabIndex = 2;
        // 
        // cardEmployees
        // 
        cardEmployees.BackColor = Color.White;
        cardEmployees.Controls.Add(cardEmployeesTitle);
        cardEmployees.Controls.Add(cardEmployeesValue);
        cardEmployees.Controls.Add(cardEmployeesAccent);
        cardEmployees.Margin = new Padding(8, 9, 8, 9);
        cardEmployees.Name = "cardEmployees";
        cardEmployees.Size = new Size(182, 88);
        cardEmployees.TabIndex = 2;
        // 
        // cardEmployeesTitle
        // 
        cardEmployeesTitle.Dock = DockStyle.Top;
        cardEmployeesTitle.Font = new Font("Tahoma", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardEmployeesTitle.ForeColor = Color.FromArgb(127, 140, 141);
        cardEmployeesTitle.Height = 36;
        cardEmployeesTitle.Name = "cardEmployeesTitle";
        cardEmployeesTitle.TabIndex = 0;
        cardEmployeesTitle.Text = "عدد الموظفين";
        cardEmployeesTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cardEmployeesValue
        // 
        cardEmployeesValue.Dock = DockStyle.Fill;
        cardEmployeesValue.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        cardEmployeesValue.ForeColor = Color.FromArgb(46, 204, 113);
        cardEmployeesValue.Name = "cardEmployeesValue";
        cardEmployeesValue.TabIndex = 1;
        cardEmployeesValue.Text = "0";
        cardEmployeesValue.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // cardEmployeesAccent
        // 
        cardEmployeesAccent.BackColor = Color.FromArgb(46, 204, 113);
        cardEmployeesAccent.Dock = DockStyle.Bottom;
        cardEmployeesAccent.Height = 5;
        cardEmployeesAccent.Name = "cardEmployeesAccent";
        cardEmployeesAccent.TabIndex = 2;
        // 
        // pnlFilter
        // 
        pnlFilter.BackColor = Color.White;
        pnlFilter.Controls.Add(flowFilter);
        pnlFilter.Dock = DockStyle.Top;
        pnlFilter.Name = "pnlFilter";
        pnlFilter.Size = new Size(620, 58);
        pnlFilter.TabIndex = 2;
        // 
        // flowFilter
        // 
        flowFilter.Controls.Add(lblSearch);
        flowFilter.Controls.Add(txtSearch);
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
        lblSearch.Size = new Size(85, 24);
        lblSearch.TabIndex = 0;
        lblSearch.Text = "بحث بالاسم:";
        // 
        // txtSearch
        // 
        txtSearch.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtSearch.Location = new Point(98, 13);
        txtSearch.Margin = new Padding(14, 12, 4, 12);
        txtSearch.Name = "txtSearch";
        txtSearch.RightToLeft = RightToLeft.Yes;
        txtSearch.Size = new Size(220, 30);
        txtSearch.TabIndex = 1;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
        lblStatus.Location = new Point(332, 18);
        lblStatus.Margin = new Padding(6, 14, 4, 14);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(60, 24);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "الحالة:";
        // 
        // cbStatus
        // 
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbStatus.Items.AddRange(new object[] { "الكل", "سلفة", "عهدة" });
        cbStatus.Location = new Point(402, 14);
        cbStatus.Margin = new Padding(14, 14, 4, 14);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(110, 30);
        cbStatus.TabIndex = 3;
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
        pnlFooter.Size = new Size(620, 58);
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
        btnAdd.Location = new Point(448, 10);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(160, 40);
        btnAdd.TabIndex = 1;
        btnAdd.Text = "+ إضافة سلفة/عهدة";
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
        btnClose.Size = new Size(110, 40);
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
        label1.Location = new Point(470, 14);
        label1.Margin = new Padding(4, 0, 4, 0);
        label1.Name = "label1";
        label1.Size = new Size(115, 24);
        label1.TabIndex = 5;
        label1.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(52, 73, 94);
        lblRecordsCount.Location = new Point(405, 14);
        lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(45, 24);
        lblRecordsCount.TabIndex = 0;
        lblRecordsCount.Text = "[0]";
        // 
        // dgvAdvances
        // 
        dgvAdvances.AllowUserToAddRows = false;
        dgvAdvances.AllowUserToDeleteRows = false;
        dgvAdvances.AllowUserToResizeRows = false;
        dgvAdvances.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvAdvances.BackgroundColor = Color.FromArgb(30, 30, 30);
        dgvAdvances.BorderStyle = BorderStyle.None;
        dgvAdvances.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvAdvances.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvAdvances.ColumnHeadersHeight = 44;
        dgvAdvances.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvAdvances.ContextMenuStrip = cmsAdvances;
        dgvAdvances.Dock = DockStyle.Fill;
        dgvAdvances.EnableHeadersVisualStyles = false;
        dgvAdvances.GridColor = Color.FromArgb(64, 64, 64);
        dgvAdvances.MultiSelect = false;
        dgvAdvances.Name = "dgvAdvances";
        dgvAdvances.ReadOnly = true;
        dgvAdvances.RowHeadersVisible = false;
        dgvAdvances.RowTemplate.Height = 36;
        dgvAdvances.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAdvances.Size = new Size(620, 322);
        dgvAdvances.TabIndex = 4;
        dgvAdvances.TabStop = false;
        dgvAdvances.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 43, 54);
        dgvAdvances.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvAdvances.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dgvAdvances.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvAdvances.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(33, 43, 54);
        dgvAdvances.DefaultCellStyle.BackColor = Color.FromArgb(44, 44, 44);
        dgvAdvances.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
        dgvAdvances.DefaultCellStyle.Font = new Font("Tahoma", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dgvAdvances.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
        dgvAdvances.DefaultCellStyle.SelectionForeColor = Color.White;
        dgvAdvances.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        dgvAdvances.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 38, 38);
        dgvAdvances.CellFormatting += dgvAdvances_CellFormatting;
        dgvAdvances.DoubleClick += dgvAdvances_DoubleClick;
        // 
        // frmCashVoucherAdvances
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(620, 620);
        Controls.Add(dgvAdvances);
        Controls.Add(pnlFooter);
        Controls.Add(pnlFilter);
        Controls.Add(pnlStats);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmCashVoucherAdvances";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "سلف وعهد الموظفين";
        Load += frmCashVoucherAdvances_Load;
        cmsAdvances.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlStats.ResumeLayout(false);
        flowCards.ResumeLayout(false);
        cardAdvances.ResumeLayout(false);
        cardTrusts.ResumeLayout(false);
        cardEmployees.ResumeLayout(false);
        pnlFilter.ResumeLayout(false);
        flowFilter.ResumeLayout(false);
        flowFilter.PerformLayout();
        pnlFooter.ResumeLayout(false);
        pnlFooter.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvAdvances).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ContextMenuStrip cmsAdvances;
    private ToolStripMenuItem viewToolStripMenuItem;
    private Panel pnlHeader;
    private Label lblMockBadge;
    private Label lblMode;
    private Label lblAccent;
    private Panel pnlStats;
    private FlowLayoutPanel flowCards;
    private Panel cardAdvances;
    private Label cardAdvancesTitle;
    private Label cardAdvancesValue;
    private Label cardAdvancesAccent;
    private Panel cardTrusts;
    private Label cardTrustsTitle;
    private Label cardTrustsValue;
    private Label cardTrustsAccent;
    private Panel cardEmployees;
    private Label cardEmployeesTitle;
    private Label cardEmployeesValue;
    private Label cardEmployeesAccent;
    private Panel pnlFilter;
    private FlowLayoutPanel flowFilter;
    private Label lblSearch;
    private TextBox txtSearch;
    private Label lblStatus;
    private ComboBox cbStatus;
    private Panel pnlFooter;
    private Label lblFooterLine;
    private Button btnAdd;
    private Button btnClose;
    private Label label1;
    private Label lblRecordsCount;
    private DataGridView dgvAdvances;
}