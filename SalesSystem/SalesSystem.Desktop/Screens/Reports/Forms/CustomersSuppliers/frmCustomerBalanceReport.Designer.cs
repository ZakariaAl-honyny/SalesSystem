namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmCustomerBalanceReport
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();
        base.Dispose(disposing);
    }

    // ── الهيكل المرجعي RULE-N79(أ) — مبنيّ في المصمّم لا وقت التشغيل ─────────────
    // أسماء الحاويات هنا مطابقة حرفياً لثوابت معيار واجهة التقارير
    // (pnlReportRoot / pnlHeader / pnlToolbar / pnlFilters / pnlGrid / pnlSummary /
    // pnlFooter / gbToolbar / gbFilter / gbGrid / gbSummary / flpToolbar /
    // flpFilterControls / flpSummary / flpFooter)، فيعود BuildReportShell فوراً عند
    // وجود pnlReportRoot وتبقى بقية الأدوات تجد عناصرها بالاسم ⇒ المصمّم = وقت التشغيل
    // (RULE-N134 + §2.34). ولا يُشير أي سطر هنا إلى Helpers (RULE-N128).
    private TableLayoutPanel pnlReportRoot;
    private Panel pnlHeader;
    private Label lblMode;
    private Panel pnlToolbar;
    private GroupBox gbToolbar;
    private FlowLayoutPanel flpToolbar;
    private Panel pnlFilters;
    private GroupBox gbFilter;
    private FlowLayoutPanel flpFilterControls;
    private Panel pnlGrid;
    private GroupBox gbGrid;
    private Panel pnlSummary;
    private GroupBox gbSummary;
    private FlowLayoutPanel flpSummary;
    private Panel pnlFooter;
    private FlowLayoutPanel flpFooter;

    private DataGridView dgvReport;
    private Button btnRefresh;
    private Button btnExport;
    private Button btnClose;
    private Label label1;
    private Label lblRecordsCount;
    private Label lblSummary;
    private Label lblCustomer;
    private ComboBox cmbCustomer;
    private Panel loadingPanel;
    // RULE-N146 — شريط تنقل التقارير مُعلَن في المصمّم كما يظهر وقت التشغيل
    private Panel pnlReportNav;
    private FlowLayoutPanel flpReportNav;
    private Label lblNavCaption;
    private Button btnNavAllReports;
    private Label ll;

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        pnlReportRoot = new TableLayoutPanel();
        pnlHeader = new Panel();
        lblMode = new Label();
        pnlToolbar = new Panel();
        gbToolbar = new GroupBox();
        flpToolbar = new FlowLayoutPanel();
        btnRefresh = new Button();
        btnExport = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        flpFilterControls = new FlowLayoutPanel();
        lblCustomer = new Label();
        cmbCustomer = new ComboBox();
        pnlGrid = new Panel();
        gbGrid = new GroupBox();
        dgvReport = new DataGridView();
        pnlSummary = new Panel();
        gbSummary = new GroupBox();
        flpSummary = new FlowLayoutPanel();
        label1 = new Label();
        lblRecordsCount = new Label();
        lblSummary = new Label();
        pnlFooter = new Panel();
        flpFooter = new FlowLayoutPanel();
        btnClose = new Button();
        loadingPanel = new Panel();
        pnlReportNav = new Panel();
        flpReportNav = new FlowLayoutPanel();
        lblNavCaption = new Label();
        btnNavAllReports = new Button();
        ll = new Label();
        pnlReportRoot.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        gbToolbar.SuspendLayout();
        flpToolbar.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        flpFilterControls.SuspendLayout();
        pnlGrid.SuspendLayout();
        gbGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReport).BeginInit();
        pnlSummary.SuspendLayout();
        gbSummary.SuspendLayout();
        flpSummary.SuspendLayout();
        pnlFooter.SuspendLayout();
        flpFooter.SuspendLayout();
        loadingPanel.SuspendLayout();
        pnlReportNav.SuspendLayout();
        flpReportNav.SuspendLayout();
        SuspendLayout();
        // 
        // pnlReportRoot
        // 
        pnlReportRoot.BackColor = System.Drawing.Color.White;
        pnlReportRoot.ColumnCount = 1;
        pnlReportRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlReportRoot.Controls.Add(pnlHeader, 0, 0);
        pnlReportRoot.Controls.Add(pnlToolbar, 0, 1);
        pnlReportRoot.Controls.Add(pnlFilters, 0, 2);
        pnlReportRoot.Controls.Add(pnlGrid, 0, 3);
        pnlReportRoot.Controls.Add(pnlSummary, 0, 4);
        pnlReportRoot.Controls.Add(pnlFooter, 0, 5);
        pnlReportRoot.Dock = DockStyle.Fill;
        pnlReportRoot.Location = new Point(0, 0);
        pnlReportRoot.Margin = new Padding(0);
        pnlReportRoot.Name = "pnlReportRoot";
        pnlReportRoot.RowCount = 6;
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        pnlReportRoot.Size = new Size(1200, 680);
        pnlReportRoot.TabIndex = 0;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = System.Drawing.Color.White;
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Fill;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1200, 46);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = System.Drawing.Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1200, 46);
        lblMode.TabIndex = 30;
        lblMode.Text = "كشف حساب عملاء";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = System.Drawing.Color.White;
        pnlToolbar.Controls.Add(gbToolbar);
        pnlToolbar.Dock = DockStyle.Fill;
        pnlToolbar.Location = new Point(0, 46);
        pnlToolbar.Margin = new Padding(0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1200, 82);
        pnlToolbar.TabIndex = 1;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = System.Drawing.Color.White;
        gbToolbar.Controls.Add(flpToolbar);
        gbToolbar.Dock = DockStyle.Fill;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbToolbar.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbToolbar.Location = new Point(0, 0);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8, 2, 8, 4);
        gbToolbar.Size = new Size(1200, 82);
        gbToolbar.TabIndex = 0;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر التقرير";
        // 
        // flpToolbar
        // 
        flpToolbar.AutoScroll = true;
        flpToolbar.BackColor = System.Drawing.Color.White;
        flpToolbar.Controls.Add(btnRefresh);
        flpToolbar.Controls.Add(btnExport);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.RightToLeft;
        flpToolbar.Location = new Point(8, 27);
        flpToolbar.Name = "flpToolbar";
        flpToolbar.Padding = new Padding(8, 8, 8, 4);
        flpToolbar.Size = new Size(1184, 51);
        flpToolbar.TabIndex = 0;
        flpToolbar.WrapContents = false;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = System.Drawing.Color.White;
        btnRefresh.Location = new Point(1031, 12);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(176, 36);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "تحديث التقرير";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnExport
        // 
        btnExport.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
        btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        btnExport.FlatStyle = FlatStyle.Flat;
        btnExport.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnExport.ForeColor = System.Drawing.Color.White;
        btnExport.Location = new Point(847, 12);
        btnExport.Margin = new Padding(4);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(176, 36);
        btnExport.TabIndex = 2;
        btnExport.Text = "تصدير Excel";
        btnExport.UseVisualStyleBackColor = false;
        btnExport.Click += btnExport_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = System.Drawing.Color.White;
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Fill;
        pnlFilters.Location = new Point(0, 128);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Size = new Size(1200, 96);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = System.Drawing.Color.White;
        gbFilter.Controls.Add(flpFilterControls);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbFilter.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbFilter.Location = new Point(0, 0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.Size = new Size(1200, 96);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // flpFilterControls
        // 
        flpFilterControls.BackColor = System.Drawing.Color.White;
        flpFilterControls.Controls.Add(lblCustomer);
        flpFilterControls.Controls.Add(cmbCustomer);
        flpFilterControls.Dock = DockStyle.Fill;
        flpFilterControls.FlowDirection = FlowDirection.RightToLeft;
        flpFilterControls.Location = new Point(8, 33);
        flpFilterControls.Name = "flpFilterControls";
        flpFilterControls.Padding = new Padding(0, 4, 0, 0);
        flpFilterControls.Size = new Size(1184, 55);
        flpFilterControls.TabIndex = 0;
        flpFilterControls.WrapContents = true;
        // 
        // lblCustomer
        // 
        lblCustomer.AutoSize = true;
        lblCustomer.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCustomer.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblCustomer.Location = new Point(1118, 4);
        lblCustomer.Name = "lblCustomer";
        lblCustomer.Size = new Size(60, 24);
        lblCustomer.TabIndex = 3;
        lblCustomer.Text = "العميل:";
        lblCustomer.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbCustomer
        // 
        cmbCustomer.BackColor = System.Drawing.Color.White;
        cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCustomer.FlatStyle = FlatStyle.Flat;
        cmbCustomer.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCustomer.Location = new Point(914, 7);
        cmbCustomer.Name = "cmbCustomer";
        cmbCustomer.Size = new Size(200, 32);
        cmbCustomer.TabIndex = 4;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = System.Drawing.Color.White;
        pnlGrid.Controls.Add(gbGrid);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 224);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1200, 332);
        pnlGrid.TabIndex = 3;
        // 
        // gbGrid
        // 
        gbGrid.BackColor = System.Drawing.Color.White;
        gbGrid.Controls.Add(dgvReport);
        gbGrid.Dock = DockStyle.Fill;
        gbGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbGrid.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbGrid.Location = new Point(8, 8);
        gbGrid.Name = "gbGrid";
        gbGrid.Padding = new Padding(8, 2, 8, 4);
        gbGrid.Size = new Size(1184, 316);
        gbGrid.TabIndex = 0;
        gbGrid.TabStop = false;
        gbGrid.Text = "بيانات التقرير";
        // 
        // dgvReport
        // 
        dgvReport.AllowUserToAddRows = false;
        dgvReport.AllowUserToDeleteRows = false;
        dgvReport.AllowUserToOrderColumns = true;
        dgvReport.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
        dgvReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReport.BackgroundColor = System.Drawing.Color.White;
        dgvReport.BorderStyle = BorderStyle.None;
        dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
        dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvReport.ColumnHeadersHeight = 45;
        dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(243), Convert.ToInt32(205));
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvReport.DefaultCellStyle = dataGridViewCellStyle3;
        dgvReport.Dock = DockStyle.Fill;
        dgvReport.EnableHeadersVisualStyles = false;
        dgvReport.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(222), Convert.ToInt32(226), Convert.ToInt32(230));
        dgvReport.Location = new Point(8, 27);
        dgvReport.MultiSelect = false;
        dgvReport.Name = "dgvReport";
        dgvReport.ReadOnly = true;
        dgvReport.RowHeadersVisible = false;
        dgvReport.RowHeadersWidth = 62;
        dgvReport.RowTemplate.Height = 32;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.Size = new Size(1168, 285);
        dgvReport.TabIndex = 7;
        dgvReport.TabStop = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = System.Drawing.Color.White;
        pnlSummary.Controls.Add(gbSummary);
        pnlSummary.Dock = DockStyle.Fill;
        pnlSummary.Location = new Point(0, 556);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1200, 68);
        pnlSummary.TabIndex = 4;
        // 
        // gbSummary
        // 
        gbSummary.BackColor = System.Drawing.Color.White;
        gbSummary.Controls.Add(flpSummary);
        gbSummary.Dock = DockStyle.Fill;
        gbSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        gbSummary.Location = new Point(0, 0);
        gbSummary.Name = "gbSummary";
        gbSummary.Padding = new Padding(8, 2, 8, 4);
        gbSummary.Size = new Size(1200, 68);
        gbSummary.TabIndex = 0;
        gbSummary.TabStop = false;
        gbSummary.Text = "الإجماليات";
        // 
        // flpSummary
        // 
        flpSummary.BackColor = System.Drawing.Color.White;
        flpSummary.Controls.Add(label1);
        flpSummary.Controls.Add(lblRecordsCount);
        flpSummary.Controls.Add(lblSummary);
        flpSummary.Dock = DockStyle.Fill;
        flpSummary.FlowDirection = FlowDirection.RightToLeft;
        flpSummary.Location = new Point(8, 27);
        flpSummary.Name = "flpSummary";
        flpSummary.Padding = new Padding(8, 4, 8, 4);
        flpSummary.Size = new Size(1184, 37);
        flpSummary.TabIndex = 0;
        flpSummary.WrapContents = false;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        label1.Location = new Point(8, 10);
        label1.Margin = new Padding(10, 6, 0, 0);
        label1.Name = "label1";
        label1.Size = new Size(145, 24);
        label1.TabIndex = 9;
        label1.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblRecordsCount.Location = new Point(163, 10);
        lblRecordsCount.Margin = new Padding(10, 6, 0, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(23, 24);
        lblRecordsCount.TabIndex = 10;
        lblRecordsCount.Text = "0";
        // 
        // lblSummary
        // 
        lblSummary.AutoSize = true;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblSummary.Location = new Point(196, 10);
        lblSummary.Margin = new Padding(10, 6, 0, 0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(58, 24);
        lblSummary.TabIndex = 8;
        lblSummary.Text = "جاهز";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = System.Drawing.Color.White;
        pnlFooter.Controls.Add(flpFooter);
        pnlFooter.Dock = DockStyle.Fill;
        pnlFooter.Location = new Point(0, 624);
        pnlFooter.Margin = new Padding(0);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1200, 56);
        pnlFooter.TabIndex = 5;
        // 
        // flpFooter
        // 
        flpFooter.AutoScroll = true;
        flpFooter.BackColor = System.Drawing.Color.White;
        flpFooter.Controls.Add(btnClose);
        flpFooter.Dock = DockStyle.Fill;
        flpFooter.FlowDirection = FlowDirection.RightToLeft;
        flpFooter.Location = new Point(0, 0);
        flpFooter.Name = "flpFooter";
        flpFooter.Padding = new Padding(8, 6, 8, 6);
        flpFooter.Size = new Size(1200, 56);
        flpFooter.TabIndex = 0;
        flpFooter.WrapContents = false;
        // 
        // btnClose
        // 
        btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = System.Drawing.Color.White;
        btnClose.Location = new Point(1040, 10);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 36);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // loadingPanel
        // 
        loadingPanel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(100), Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(255));
        loadingPanel.Controls.Add(ll);
        loadingPanel.Dock = DockStyle.Fill;
        loadingPanel.Location = new Point(0, 0);
        loadingPanel.Name = "loadingPanel";
        loadingPanel.Size = new Size(1200, 680);
        loadingPanel.TabIndex = 31;
        loadingPanel.Visible = false;
        // 
        // ll
        // 
        ll.AutoSize = true;
        ll.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        ll.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        ll.Location = new Point(507, 320);
        ll.Name = "ll";
        ll.Size = new Size(168, 29);
        ll.TabIndex = 0;
        ll.Text = "جارٍ التحميل...";
        // 
        // pnlReportNav
        // 
        pnlReportNav.BackColor = System.Drawing.Color.White;
        pnlReportNav.Controls.Add(flpReportNav);
        pnlReportNav.Controls.Add(lblNavCaption);
        pnlReportNav.Dock = DockStyle.Bottom;
        pnlReportNav.Height = 58;
        pnlReportNav.Location = new Point(0, 622);
        pnlReportNav.Name = "pnlReportNav";
        pnlReportNav.RightToLeft = RightToLeft.Yes;
        pnlReportNav.Size = new Size(1200, 58);
        pnlReportNav.TabIndex = 32;
        // 
        // flpReportNav
        // 
        flpReportNav.AutoScroll = true;
        flpReportNav.BackColor = System.Drawing.Color.White;
        flpReportNav.Controls.Add(btnNavAllReports);
        flpReportNav.Dock = DockStyle.Fill;
        flpReportNav.FlowDirection = FlowDirection.RightToLeft;
        flpReportNav.Location = new Point(0, 0);
        flpReportNav.Margin = new Padding(0);
        flpReportNav.Name = "flpReportNav";
        flpReportNav.Size = new Size(1050, 58);
        flpReportNav.TabIndex = 0;
        flpReportNav.WrapContents = false;
        // 
        // btnNavAllReports
        // 
        btnNavAllReports.BackColor = System.Drawing.Color.White;
        btnNavAllReports.FlatStyle = FlatStyle.Popup;
        btnNavAllReports.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnNavAllReports.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        btnNavAllReports.Location = new Point(896, 4);
        btnNavAllReports.Margin = new Padding(4);
        btnNavAllReports.Name = "btnNavAllReports";
        btnNavAllReports.RightToLeft = RightToLeft.Yes;
        btnNavAllReports.Size = new Size(150, 40);
        btnNavAllReports.TabIndex = 0;
        btnNavAllReports.Text = "كل التقارير ▾";
        btnNavAllReports.UseVisualStyleBackColor = false;
        // 
        // lblNavCaption
        // 
        lblNavCaption.BackColor = System.Drawing.Color.White;
        lblNavCaption.Dock = DockStyle.Right;
        lblNavCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        lblNavCaption.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
        lblNavCaption.Location = new Point(1050, 0);
        lblNavCaption.Name = "lblNavCaption";
        lblNavCaption.Size = new Size(150, 58);
        lblNavCaption.TabIndex = 1;
        lblNavCaption.Text = "تقارير هذه المجموعة:";
        lblNavCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // frmCustomerBalanceReport
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
        CancelButton = btnClose;
        ClientSize = new Size(1200, 738);
        Controls.Add(pnlReportRoot);
        Controls.Add(loadingPanel);
        Controls.Add(pnlReportNav);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmCustomerBalanceReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "كشف حساب عملاء";
        pnlReportRoot.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        flpToolbar.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        flpFilterControls.ResumeLayout(false);
        flpFilterControls.PerformLayout();
        pnlGrid.ResumeLayout(false);
        gbGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvReport).EndInit();
        pnlSummary.ResumeLayout(false);
        gbSummary.ResumeLayout(false);
        flpSummary.ResumeLayout(false);
        flpSummary.PerformLayout();
        pnlFooter.ResumeLayout(false);
        flpFooter.ResumeLayout(false);
        loadingPanel.ResumeLayout(false);
        loadingPanel.PerformLayout();
        flpReportNav.ResumeLayout(false);
        flpReportNav.PerformLayout();
        pnlReportNav.ResumeLayout(false);
        ResumeLayout(false);
    }
}
