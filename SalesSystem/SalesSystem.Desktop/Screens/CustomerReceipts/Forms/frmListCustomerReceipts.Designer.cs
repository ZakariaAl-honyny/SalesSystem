namespace SalesSystem.Desktop.CustomerReceipts.Forms;

partial class frmListCustomerReceipts
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
        cmsReceipts = new ContextMenuStrip(components);
        editToolStripMenuItem = new ToolStripMenuItem();
        postToolStripMenuItem = new ToolStripMenuItem();
        cancelToolStripMenuItem = new ToolStripMenuItem();
        toolTip1 = new ToolTip(components);
        pnlHeader = new Panel();
        lblMode = new Label();
        lblRecordsCount = new Label();
        lblCountCaption = new Label();
        pnlToolbar = new Panel();
        flpToolbar = new FlowLayoutPanel();
        btnAddNew = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        label1 = new Label();
        cbStatus = new ComboBox();
        label3 = new Label();
        dtpFrom = new DateTimePicker();
        label4 = new Label();
        dtpTo = new DateTimePicker();
        btnRangeToday = new Button();
        btnRange30 = new Button();
        btnRangeMonth = new Button();
        btnClearFilters = new Button();
        chkOnlyUnapplied = new CheckBox();
        btnToggleFilters = new Button();
        pnlSummary = new Panel();
        lblSummary = new Label();
        pnlGrid = new Panel();
        emptyState = new Label();
        dgvReceipts = new DataGridView();
        cmsReceipts.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        flpToolbar.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvReceipts).BeginInit();
        SuspendLayout();
        // 
        // cmsReceipts
        // 
        cmsReceipts.BackColor = Color.White;
        cmsReceipts.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsReceipts.ImageScalingSize = new Size(24, 24);
        cmsReceipts.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, postToolStripMenuItem, cancelToolStripMenuItem });
        cmsReceipts.Name = "cmsReceipts";
        cmsReceipts.RightToLeft = RightToLeft.Yes;
        cmsReceipts.Size = new Size(220, 98);
        cmsReceipts.Opening += cmsReceipts_Opening;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(219, 30);
        editToolStripMenuItem.Text = "تعديل السند";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // postToolStripMenuItem
        // 
        postToolStripMenuItem.Name = "postToolStripMenuItem";
        postToolStripMenuItem.Size = new Size(219, 30);
        postToolStripMenuItem.Text = "ترحيل";
        postToolStripMenuItem.Click += postToolStripMenuItem_Click;
        // 
        // cancelToolStripMenuItem
        // 
        cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
        cancelToolStripMenuItem.Size = new Size(219, 30);
        cancelToolStripMenuItem.Text = "إلغاء الترحيل";
        cancelToolStripMenuItem.Click += cancelToolStripMenuItem_Click;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Controls.Add(lblRecordsCount);
        pnlHeader.Controls.Add(lblCountCaption);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1200, 56);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Margin = new Padding(0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1020, 56);
        lblMode.TabIndex = 0;
        lblMode.Text = "إدارة سندات القبض";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.BackColor = Color.FromArgb(14, 165, 103);
        lblRecordsCount.Dock = DockStyle.Right;
        lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.White;
        lblRecordsCount.Location = new Point(1020, 0);
        lblRecordsCount.Margin = new Padding(0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(70, 56);
        lblRecordsCount.TabIndex = 2;
        lblRecordsCount.Text = "0";
        lblRecordsCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblCountCaption
        // 
        lblCountCaption.BackColor = Color.FromArgb(14, 165, 103);
        lblCountCaption.Dock = DockStyle.Right;
        lblCountCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCountCaption.ForeColor = Color.White;
        lblCountCaption.Location = new Point(1090, 0);
        lblCountCaption.Margin = new Padding(0);
        lblCountCaption.Name = "lblCountCaption";
        lblCountCaption.Size = new Size(110, 56);
        lblCountCaption.TabIndex = 1;
        lblCountCaption.Text = "عدد السجلات:";
        lblCountCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
        pnlToolbar.Controls.Add(flpToolbar);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 56);
        pnlToolbar.Margin = new Padding(0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1200, 64);
        pnlToolbar.TabIndex = 1;
        // 
        // flpToolbar
        // 
        flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
        flpToolbar.Controls.Add(btnAddNew);
        flpToolbar.Controls.Add(btnRefresh);
        flpToolbar.Controls.Add(btnClose);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.Location = new Point(0, 0);
        flpToolbar.Margin = new Padding(0);
        flpToolbar.Name = "flpToolbar";
        flpToolbar.Padding = new Padding(8, 12, 8, 12);
        flpToolbar.Size = new Size(1200, 64);
        flpToolbar.TabIndex = 0;
        flpToolbar.WrapContents = false;
        // 
        // btnAddNew
        // 
        btnAddNew.BackColor = Color.FromArgb(40, 167, 69);
        btnAddNew.FlatStyle = FlatStyle.Flat;
        btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnAddNew.ForeColor = Color.White;
        btnAddNew.Location = new Point(12, 16);
        btnAddNew.Margin = new Padding(4);
        btnAddNew.Name = "btnAddNew";
        btnAddNew.Size = new Size(130, 38);
        btnAddNew.TabIndex = 0;
        btnAddNew.Text = "+ سند جديد";
        btnAddNew.UseVisualStyleBackColor = false;
        btnAddNew.Click += btnAddNew_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(150, 16);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(120, 38);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "تحديث";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(274, 16);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 38);
        btnClose.TabIndex = 2;
        btnClose.Text = "إغلاق";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 120);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1200, 112);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(label3);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(label4);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Controls.Add(chkOnlyUnapplied);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbFilter.Location = new Point(6, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(10);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(1188, 104);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label2
        // 
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.ForeColor = Color.FromArgb(14, 82, 52);
        label2.Location = new Point(1050, 25);
        label2.Margin = new Padding(0);
        label2.Name = "label2";
        label2.Size = new Size(90, 22);
        label2.TabIndex = 0;
        label2.Text = "تصفية بـ:";
        label2.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "العميل", "رقم السند", "البيان", "المبلغ" });
        cbFilterBy.Location = new Point(910, 22);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(135, 26);
        cbFilterBy.TabIndex = 1;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtFilterValue.Location = new Point(690, 22);
        txtFilterValue.Margin = new Padding(0);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.RightToLeft = RightToLeft.Yes;
        txtFilterValue.Size = new Size(215, 26);
        txtFilterValue.TabIndex = 2;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        // 
        // label1
        // 
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.ForeColor = Color.FromArgb(14, 82, 52);
        label1.Location = new Point(608, 25);
        label1.Margin = new Padding(0);
        label1.Name = "label1";
        label1.Size = new Size(78, 22);
        label1.TabIndex = 3;
        label1.Text = "الحالة:";
        label1.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.BackColor = Color.White;
        cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbStatus.FormattingEnabled = true;
        cbStatus.Location = new Point(490, 22);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(113, 26);
        cbStatus.TabIndex = 4;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // label3
        // 
        label3.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.ForeColor = Color.FromArgb(14, 82, 52);
        label3.Location = new Point(432, 25);
        label3.Margin = new Padding(0);
        label3.Name = "label3";
        label3.Size = new Size(54, 22);
        label3.TabIndex = 5;
        label3.Text = "من:";
        label3.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFrom
        // 
        dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(298, 22);
        dtpFrom.Margin = new Padding(0);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(130, 26);
        dtpFrom.TabIndex = 6;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // label4
        // 
        label4.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.ForeColor = Color.FromArgb(14, 82, 52);
        label4.Location = new Point(240, 25);
        label4.Margin = new Padding(0);
        label4.Name = "label4";
        label4.Size = new Size(54, 22);
        label4.TabIndex = 7;
        label4.Text = "إلى:";
        label4.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpTo
        // 
        dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(106, 22);
        dtpTo.Margin = new Padding(0);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(130, 26);
        dtpTo.TabIndex = 8;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = Color.FromArgb(248, 249, 250);
        btnRangeToday.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRangeToday.FlatAppearance.BorderSize = 1;
        btnRangeToday.FlatStyle = FlatStyle.Flat;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRangeToday.ForeColor = Color.FromArgb(33, 37, 41);
        btnRangeToday.Location = new Point(1038, 62);
        btnRangeToday.Margin = new Padding(0);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(100, 30);
        btnRangeToday.TabIndex = 9;
        btnRangeToday.Text = "اليوم";
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.Click += btnRangeToday_Click;
        // 
        // btnRange30
        // 
        btnRange30.BackColor = Color.FromArgb(248, 249, 250);
        btnRange30.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRange30.FlatAppearance.BorderSize = 1;
        btnRange30.FlatStyle = FlatStyle.Flat;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRange30.ForeColor = Color.FromArgb(33, 37, 41);
        btnRange30.Location = new Point(928, 62);
        btnRange30.Margin = new Padding(0);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(105, 30);
        btnRange30.TabIndex = 10;
        btnRange30.Text = "آخر 30 يوماً";
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = Color.FromArgb(248, 249, 250);
        btnRangeMonth.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRangeMonth.FlatAppearance.BorderSize = 1;
        btnRangeMonth.FlatStyle = FlatStyle.Flat;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRangeMonth.ForeColor = Color.FromArgb(33, 37, 41);
        btnRangeMonth.Location = new Point(818, 62);
        btnRangeMonth.Margin = new Padding(0);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(105, 30);
        btnRangeMonth.TabIndex = 11;
        btnRangeMonth.Text = "هذا الشهر";
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // btnClearFilters
        // 
        // RULE-N148: يمسح أدوات التصفية فقط بلا إعادة تحميل البيانات — «تحديث» هو المسؤول عن التحميل.
        // RULE-N145: مُعلَن في المصمّم بقيم حرفية مطابقة لما يُطبَّق وقت التشغيل (RULE-N134).
        btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClearFilters.ForeColor = Color.White;
        btnClearFilters.Location = new Point(392, 61);
        btnClearFilters.Margin = new Padding(0);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(145, 32);
        btnClearFilters.TabIndex = 13;
        btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
        toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // chkOnlyUnapplied
        // 
        chkOnlyUnapplied.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        chkOnlyUnapplied.ForeColor = Color.FromArgb(14, 82, 52);
        chkOnlyUnapplied.Location = new Point(545, 65);
        chkOnlyUnapplied.Margin = new Padding(0);
        chkOnlyUnapplied.Name = "chkOnlyUnapplied";
        chkOnlyUnapplied.Size = new Size(255, 26);
        chkOnlyUnapplied.TabIndex = 12;
        chkOnlyUnapplied.Text = "غير المخصصة فقط";
        chkOnlyUnapplied.UseVisualStyleBackColor = false;
        chkOnlyUnapplied.CheckedChanged += chkOnlyUnapplied_CheckedChanged;
        // 
        // btnToggleFilters
        // 
        // RULE-N145: مُعلَن في المصمّم بقيم حرفية مطابقة لما يُطبَّق وقت التشغيل.
        // السلوك (النقرة · F9 · التلميح · الحالة المحفوظة) يُوصِّله FilterPanelHelper.Attach.
        btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
        btnToggleFilters.Cursor = Cursors.Hand;
        btnToggleFilters.Dock = DockStyle.Left;
        btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnToggleFilters.FlatAppearance.BorderSize = 1;
        btnToggleFilters.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        btnToggleFilters.FlatStyle = FlatStyle.Popup;
        btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
        btnToggleFilters.Location = new Point(6, 4);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 13;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 728);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Padding = new Padding(10, 0, 10, 0);
        pnlSummary.Size = new Size(1200, 32);
        pnlSummary.TabIndex = 4;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(10, 0);
        lblSummary.Margin = new Padding(0);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1180, 32);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00 · مرحّل: 0 · ملغى: 0";
        lblSummary.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(235, 240, 235);
        pnlGrid.Controls.Add(emptyState);
        pnlGrid.Controls.Add(dgvReceipts);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 232);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Size = new Size(1200, 496);
        pnlGrid.TabIndex = 3;
        // 
        // emptyState
        // 
        emptyState.BackColor = Color.Transparent;
        emptyState.Dock = DockStyle.Fill;
        emptyState.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        emptyState.ForeColor = Color.FromArgb(14, 82, 52);
        emptyState.Location = new Point(0, 0);
        emptyState.Margin = new Padding(0);
        emptyState.Name = "emptyState";
        emptyState.Size = new Size(1200, 496);
        emptyState.TabIndex = 1;
        emptyState.Text = "لا توجد سندات قبض مطابقة للمعايير الحالية";
        emptyState.TextAlign = ContentAlignment.MiddleCenter;
        emptyState.Visible = false;
        // 
        // dgvReceipts
        // 
        dgvReceipts.AllowUserToAddRows = false;
        dgvReceipts.AllowUserToDeleteRows = false;
        dgvReceipts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
        dgvReceipts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvReceipts.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvReceipts.BorderStyle = BorderStyle.None;
        dgvReceipts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
        dgvReceipts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvReceipts.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dgvReceipts.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        dgvReceipts.ColumnHeadersHeight = 45;
        dgvReceipts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvReceipts.ContextMenuStrip = cmsReceipts;
        dgvReceipts.DefaultCellStyle.BackColor = Color.White;
        dgvReceipts.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
        dgvReceipts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dgvReceipts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvReceipts.Dock = DockStyle.Fill;
        dgvReceipts.EnableHeadersVisualStyles = false;
        dgvReceipts.GridColor = Color.FromArgb(205, 225, 205);
        dgvReceipts.Location = new Point(0, 0);
        dgvReceipts.Margin = new Padding(0);
        dgvReceipts.MultiSelect = false;
        dgvReceipts.Name = "dgvReceipts";
        dgvReceipts.ReadOnly = true;
        dgvReceipts.RowHeadersVisible = false;
        dgvReceipts.RowHeadersWidth = 62;
        dgvReceipts.RowTemplate.Height = 32;
        dgvReceipts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvReceipts.Size = new Size(1200, 496);
        dgvReceipts.TabIndex = 0;
        dgvReceipts.TabStop = false;
        dgvReceipts.DoubleClick += dgvReceipts_DoubleClick;
        // 
        // frmListCustomerReceipts
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        ClientSize = new Size(1200, 760);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        Name = "frmListCustomerReceipts";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إدارة سندات القبض";
        Load += frmListCustomerReceipts_Load;
        cmsReceipts.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        flpToolbar.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvReceipts).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMode;
    private Label lblCountCaption;
    private Label lblRecordsCount;
    private Panel pnlToolbar;
    private FlowLayoutPanel flpToolbar;
    private Button btnAddNew;
    private Button btnRefresh;
    private Button btnClose;
    private Panel pnlFilters;
    private GroupBox gbFilter;
    private Label label2;
    private ComboBox cbFilterBy;
    private TextBox txtFilterValue;
    private Label label1;
    private ComboBox cbStatus;
    private Label label3;
    private DateTimePicker dtpFrom;
    private Label label4;
    private DateTimePicker dtpTo;
    private Button btnRangeToday;
    private Button btnRange30;
    private Button btnRangeMonth;
    private Button btnClearFilters;
    private CheckBox chkOnlyUnapplied;
    private Button btnToggleFilters;
    private Panel pnlSummary;
    private Label lblSummary;
    private Panel pnlGrid;
    private Label emptyState;
    private DataGridView dgvReceipts;
    private ContextMenuStrip cmsReceipts;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem postToolStripMenuItem;
    private ToolStripMenuItem cancelToolStripMenuItem;
    private ToolTip toolTip1;
}
