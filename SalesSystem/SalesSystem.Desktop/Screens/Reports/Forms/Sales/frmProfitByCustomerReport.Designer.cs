namespace SalesSystem.Desktop.Screens.Reports.Forms;

partial class frmProfitByCustomerReport
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
    // وجود pnlReportRoot وتبقى بقية الأدوات تجد عناصرها بالاسم ⇒ المصمّم = وقت التشغيل (RULE-N134).
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
    private DateTimePicker dtpFrom;
    private DateTimePicker dtpTo;
    private Label label3;
    private Label label4;

    // RULE-N146 — شريط تنقل التقارير مُعلَن في المصمّم لا وقت التشغيل (RULE-N145)
    private Panel pnlReportNav;
    private Label lblNavCaption;
    private FlowLayoutPanel flpReportNav;
    private Button btnNavAllReports;

    // RULE-N145 — طبقة التحميل مُعلَنة في المصمّم (لا تُبنى وقت التشغيل)، ويبقى pnlReportNav آخر Controls.Add
    private Panel loadingPanel;
    private Label ll;

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
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
        label3 = new Label();
        dtpFrom = new DateTimePicker();
        label4 = new Label();
        dtpTo = new DateTimePicker();
        pnlGrid = new Panel();
        gbGrid = new GroupBox();
        dgvReport = new DataGridView();
        pnlSummary = new Panel();
        gbSummary = new GroupBox();
        flpSummary = new FlowLayoutPanel();
        label1 = new Label();
        lblRecordsCount = new Label();
        pnlFooter = new Panel();
        flpFooter = new FlowLayoutPanel();
        btnClose = new Button();
        loadingPanel = new Panel();
        ll = new Label();
        pnlReportNav = new Panel();
        lblNavCaption = new Label();
        flpReportNav = new FlowLayoutPanel();
        btnNavAllReports = new Button();
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
        pnlReportRoot.BackColor = Color.FromArgb(235, 240, 235);
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
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        pnlReportRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
        pnlReportRoot.Size = new Size(1200, 680);
        pnlReportRoot.TabIndex = 0;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(235, 240, 235);
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
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1200, 46);
        lblMode.TabIndex = 30;
        lblMode.Text = "الربح حسب العميل";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
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
        gbToolbar.BackColor = Color.FromArgb(235, 245, 235);
        gbToolbar.Controls.Add(flpToolbar);
        gbToolbar.Dock = DockStyle.Fill;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
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
        flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
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
        btnRefresh.BackColor = Color.FromArgb(14, 165, 103);
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 103);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(1031, 12);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(160, 36);
        btnRefresh.TabIndex = 5;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnExport
        // 
        btnExport.BackColor = Color.FromArgb(23, 162, 184);
        btnExport.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 103);
        btnExport.FlatStyle = FlatStyle.Flat;
        btnExport.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnExport.ForeColor = Color.White;
        btnExport.Location = new Point(863, 12);
        btnExport.Margin = new Padding(4);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(150, 36);
        btnExport.TabIndex = 6;
        btnExport.Text = "تصدير Excel";
        btnExport.UseVisualStyleBackColor = false;
        btnExport.Click += btnExport_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Dock = DockStyle.Fill;
        pnlFilters.Location = new Point(0, 128);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Size = new Size(1200, 120);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(flpFilterControls);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbFilter.ForeColor = Color.FromArgb(14, 82, 52);
        gbFilter.Location = new Point(0, 0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.Size = new Size(1200, 120);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // flpFilterControls
        // 
        flpFilterControls.BackColor = Color.FromArgb(235, 240, 235);
        flpFilterControls.Controls.Add(label3);
        flpFilterControls.Controls.Add(dtpFrom);
        flpFilterControls.Controls.Add(label4);
        flpFilterControls.Controls.Add(dtpTo);
        flpFilterControls.Dock = DockStyle.Fill;
        flpFilterControls.FlowDirection = FlowDirection.RightToLeft;
        flpFilterControls.Location = new Point(8, 33);
        flpFilterControls.Name = "flpFilterControls";
        flpFilterControls.Padding = new Padding(0, 4, 0, 0);
        flpFilterControls.Size = new Size(1184, 79);
        flpFilterControls.TabIndex = 0;
        flpFilterControls.WrapContents = true;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.ForeColor = Color.FromArgb(14, 82, 52);
        label3.Location = new Point(1118, 4);
        label3.Name = "label3";
        label3.Size = new Size(46, 24);
        label3.TabIndex = 9;
        label3.Text = "من:";
        label3.TextAlign = ContentAlignment.MiddleRight;
        label3.Click += label3_Click;
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = Color.White;
        dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(951, 7);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(163, 32);
        dtpFrom.TabIndex = 10;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.ForeColor = Color.FromArgb(14, 82, 52);
        label4.Location = new Point(891, 4);
        label4.Name = "label4";
        label4.Size = new Size(52, 24);
        label4.TabIndex = 11;
        label4.Text = "إلى:";
        label4.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpTo
        // 
        dtpTo.BackColor = Color.White;
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(719, 7);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(168, 32);
        dtpTo.TabIndex = 12;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(235, 240, 235);
        pnlGrid.Controls.Add(gbGrid);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 248);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(8);
        pnlGrid.Size = new Size(1200, 308);
        pnlGrid.TabIndex = 3;
        // 
        // gbGrid
        // 
        gbGrid.BackColor = Color.FromArgb(235, 245, 235);
        gbGrid.Controls.Add(dgvReport);
        gbGrid.Dock = DockStyle.Fill;
        gbGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbGrid.ForeColor = Color.FromArgb(14, 82, 52);
        gbGrid.Location = new Point(8, 8);
        gbGrid.Name = "gbGrid";
        gbGrid.Padding = new Padding(8, 2, 8, 4);
        gbGrid.Size = new Size(1184, 292);
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
        dataGridViewCellStyle4.BackColor = Color.FromArgb(245, 252, 245);
        dgvReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
        dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReport.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvReport.BorderStyle = BorderStyle.None;
        dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle5.BackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle5.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dataGridViewCellStyle5.ForeColor = Color.White;
        dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle5.SelectionForeColor = Color.White;
        dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
        dgvReport.ColumnHeadersHeight = 45;
        dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = Color.White;
        dataGridViewCellStyle6.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
        dgvReport.DefaultCellStyle = dataGridViewCellStyle6;
        dgvReport.Dock = DockStyle.Fill;
        dgvReport.EnableHeadersVisualStyles = false;
        dgvReport.GridColor = Color.FromArgb(205, 225, 205);
        dgvReport.Location = new Point(8, 27);
        dgvReport.MultiSelect = false;
        dgvReport.Name = "dgvReport";
        dgvReport.ReadOnly = true;
        dgvReport.RowHeadersVisible = false;
        dgvReport.RowHeadersWidth = 62;
        dgvReport.RowTemplate.Height = 32;
        dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReport.Size = new Size(1168, 261);
        dgvReport.TabIndex = 34;
        dgvReport.TabStop = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 245, 235);
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
        gbSummary.BackColor = Color.FromArgb(235, 245, 235);
        gbSummary.Controls.Add(flpSummary);
        gbSummary.Dock = DockStyle.Fill;
        gbSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbSummary.ForeColor = Color.FromArgb(14, 82, 52);
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
        flpSummary.BackColor = Color.FromArgb(235, 240, 235);
        flpSummary.Controls.Add(label1);
        flpSummary.Controls.Add(lblRecordsCount);
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
        label1.ForeColor = Color.FromArgb(14, 82, 52);
        label1.Location = new Point(8, 10);
        label1.Margin = new Padding(10, 6, 0, 0);
        label1.Name = "label1";
        label1.Size = new Size(145, 24);
        label1.TabIndex = 3;
        label1.Text = "عدد السجلات:";
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.AutoSize = true;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
        lblRecordsCount.Location = new Point(163, 10);
        lblRecordsCount.Margin = new Padding(10, 6, 0, 0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(23, 24);
        lblRecordsCount.TabIndex = 4;
        lblRecordsCount.Text = "0";
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
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
        flpFooter.BackColor = Color.FromArgb(235, 240, 235);
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
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 103);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
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
        loadingPanel.BackColor = Color.FromArgb(100, 255, 255, 255);
        loadingPanel.Controls.Add(ll);
        loadingPanel.Dock = DockStyle.Fill;
        loadingPanel.Location = new Point(0, 0);
        loadingPanel.Name = "loadingPanel";
        loadingPanel.Size = new Size(1200, 680);
        loadingPanel.TabIndex = 35;
        loadingPanel.Visible = false;
        // 
        // ll
        // 
        ll.AutoSize = true;
        ll.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        ll.ForeColor = Color.FromArgb(14, 82, 52);
        ll.Location = new Point(516, 320);
        ll.Name = "ll";
        ll.Size = new Size(168, 29);
        ll.TabIndex = 0;
        ll.Text = "جارٍ التحميل...";
        ll.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlReportNav
        // 
        pnlReportNav.BackColor = Color.FromArgb(235, 240, 235);
        pnlReportNav.Controls.Add(flpReportNav);
        pnlReportNav.Controls.Add(lblNavCaption);
        pnlReportNav.Dock = DockStyle.Bottom;
        pnlReportNav.Height = 58;
        pnlReportNav.Location = new Point(0, 680);
        pnlReportNav.Name = "pnlReportNav";
        pnlReportNav.RightToLeft = RightToLeft.Yes;
        pnlReportNav.Size = new Size(1200, 58);
        pnlReportNav.TabIndex = 32;
        // 
        // lblNavCaption
        // 
        lblNavCaption.BackColor = Color.FromArgb(235, 240, 235);
        lblNavCaption.Dock = DockStyle.Right;
        lblNavCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNavCaption.ForeColor = Color.FromArgb(14, 82, 52);
        lblNavCaption.Location = new Point(1050, 0);
        lblNavCaption.Name = "lblNavCaption";
        lblNavCaption.Size = new Size(150, 58);
        lblNavCaption.TabIndex = 1;
        lblNavCaption.Text = "تقارير هذه المجموعة:";
        lblNavCaption.TextAlign = ContentAlignment.MiddleCenter;
        lblNavCaption.Width = 150;
        // 
        // flpReportNav
        // 
        flpReportNav.AutoScroll = true;
        flpReportNav.BackColor = Color.FromArgb(235, 240, 235);
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
        btnNavAllReports.BackColor = Color.FromArgb(235, 245, 235);
        btnNavAllReports.FlatStyle = FlatStyle.Popup;
        btnNavAllReports.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnNavAllReports.ForeColor = Color.FromArgb(14, 82, 52);
        btnNavAllReports.Location = new Point(896, 4);
        btnNavAllReports.Margin = new Padding(4);
        btnNavAllReports.Name = "btnNavAllReports";
        btnNavAllReports.RightToLeft = RightToLeft.Yes;
        btnNavAllReports.Size = new Size(150, 40);
        btnNavAllReports.TabIndex = 0;
        btnNavAllReports.Text = "كل التقارير ▾";
        btnNavAllReports.UseVisualStyleBackColor = false;
        // 
        // frmProfitByCustomerReport
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(1200, 738);
        Controls.Add(pnlReportRoot);
        Controls.Add(loadingPanel);
        Controls.Add(pnlReportNav);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmProfitByCustomerReport";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "الربح حسب العميل";
        Load += frmProfitByCustomerReport_Load;
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
        pnlReportNav.ResumeLayout(false);
        pnlReportNav.PerformLayout();
        flpReportNav.ResumeLayout(false);
        ResumeLayout(false);
    }

    // RULE-N134 — أنماط خلايا الشبكة الحرفية (تعيش في المصمّم ليعرضها VS بنفس ألوان التشغيل).
    private DataGridViewCellStyle dataGridViewCellStyle1;
    private DataGridViewCellStyle dataGridViewCellStyle2;
    private DataGridViewCellStyle dataGridViewCellStyle3;
}
