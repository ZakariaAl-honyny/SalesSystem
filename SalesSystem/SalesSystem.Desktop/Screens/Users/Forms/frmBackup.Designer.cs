namespace SalesSystem.Desktop.Screens.Users.Forms;

partial class frmBackup
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        pnlHeader = new Panel();
        lblSubtitle = new Label();
        lblMode = new Label();
        pnlToolbar = new Panel();
        gbActions = new GroupBox();
        flpActions = new FlowLayoutPanel();
        btnCreateBackup = new Button();
        btnRestoreBackup = new Button();
        btnRefresh = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        label2 = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        label1 = new Label();
        dtpFrom = new DateTimePicker();
        label3 = new Label();
        dtpTo = new DateTimePicker();
        btnRangeToday = new Button();
        btnRangeMonth = new Button();
        btnRange30 = new Button();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlGrid = new Panel();
        lblEmpty = new Label();
        gbBackups = new GroupBox();
        dgvBackups = new DataGridView();
        colFileName = new DataGridViewTextBoxColumn();
        colCreatedAt = new DataGridViewTextBoxColumn();
        cmsBackups = new ContextMenuStrip(components);
        createBackupToolStripMenuItem = new ToolStripMenuItem();
        restoreToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        refreshToolStripMenuItem = new ToolStripMenuItem();
        pnlSummary = new Panel();
        gbSummary = new GroupBox();
        lblSummary = new Label();
        pnlFooter = new Panel();
        lblStatus = new Label();
        flpFooter = new FlowLayoutPanel();
        btnClose = new Button();
        toolTip = new ToolTip(components);
        pnlHeader.SuspendLayout();
        pnlToolbar.SuspendLayout();
        gbActions.SuspendLayout();
        flpActions.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlGrid.SuspendLayout();
        gbBackups.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBackups).BeginInit();
        cmsBackups.SuspendLayout();
        pnlSummary.SuspendLayout();
        gbSummary.SuspendLayout();
        pnlFooter.SuspendLayout();
        flpFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1100, 76);
        pnlHeader.TabIndex = 0;
        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoSize = false;
        lblSubtitle.BackColor = Color.FromArgb(14, 165, 103);
        lblSubtitle.Dock = DockStyle.Top;
        lblSubtitle.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblSubtitle.ForeColor = Color.White;
        lblSubtitle.Location = new Point(0, 40);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Padding = new Padding(12, 0, 12, 0);
        lblSubtitle.Size = new Size(1100, 36);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "ينشئ نسخة كاملة من قاعدة البيانات ويستعيدها — الاستعادة تستبدل البيانات الحالية بالكامل.";
        lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1100, 40);
        lblMode.TabIndex = 0;
        lblMode.Text = "النسخ الاحتياطي";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
        pnlToolbar.Controls.Add(gbActions);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 76);
        pnlToolbar.Margin = new Padding(0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Padding = new Padding(8);
        pnlToolbar.Size = new Size(1100, 100);
        pnlToolbar.TabIndex = 1;
        // 
        // gbActions
        // 
        gbActions.BackColor = Color.FromArgb(235, 245, 235);
        gbActions.Controls.Add(flpActions);
        gbActions.Dock = DockStyle.Fill;
        gbActions.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbActions.ForeColor = Color.FromArgb(14, 82, 52);
        gbActions.Location = new Point(8, 8);
        gbActions.Name = "gbActions";
        gbActions.Padding = new Padding(8);
        gbActions.Size = new Size(1084, 84);
        gbActions.TabIndex = 0;
        gbActions.TabStop = false;
        gbActions.Text = "أوامر النسخ الاحتياطي";
        // 
        // flpActions
        // 
        flpActions.BackColor = Color.FromArgb(235, 245, 235);
        flpActions.Controls.Add(btnCreateBackup);
        flpActions.Controls.Add(btnRestoreBackup);
        flpActions.Controls.Add(btnRefresh);
        flpActions.Dock = DockStyle.Fill;
        flpActions.FlowDirection = FlowDirection.LeftToRight;
        flpActions.Location = new Point(8, 33);
        flpActions.Name = "flpActions";
        flpActions.Padding = new Padding(6, 2, 6, 2);
        flpActions.Size = new Size(1068, 43);
        flpActions.TabIndex = 0;
        flpActions.WrapContents = false;
        // 
        // btnCreateBackup
        // 
        btnCreateBackup.BackColor = Color.FromArgb(25, 135, 84);
        btnCreateBackup.Cursor = Cursors.Hand;
        btnCreateBackup.FlatAppearance.BorderSize = 0;
        btnCreateBackup.FlatStyle = FlatStyle.Flat;
        btnCreateBackup.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnCreateBackup.ForeColor = Color.White;
        btnCreateBackup.Location = new Point(6, 2);
        btnCreateBackup.Margin = new Padding(0, 0, 8, 0);
        btnCreateBackup.Name = "btnCreateBackup";
        btnCreateBackup.Size = new Size(210, 36);
        btnCreateBackup.TabIndex = 0;
        btnCreateBackup.Text = "إنشاء نسخة احتياطية الآن";
        toolTip.SetToolTip(btnCreateBackup, "إنشاء نسخة كاملة الآن (F2)");
        btnCreateBackup.UseVisualStyleBackColor = false;
        btnCreateBackup.Click += btnCreateBackup_Click;
        // 
        // btnRestoreBackup
        // 
        btnRestoreBackup.BackColor = Color.FromArgb(220, 53, 69);
        btnRestoreBackup.Cursor = Cursors.Hand;
        btnRestoreBackup.FlatAppearance.BorderSize = 0;
        btnRestoreBackup.FlatStyle = FlatStyle.Flat;
        btnRestoreBackup.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRestoreBackup.ForeColor = Color.White;
        btnRestoreBackup.Location = new Point(226, 2);
        btnRestoreBackup.Margin = new Padding(0, 0, 8, 0);
        btnRestoreBackup.Name = "btnRestoreBackup";
        btnRestoreBackup.Size = new Size(210, 36);
        btnRestoreBackup.TabIndex = 1;
        btnRestoreBackup.Text = "استعادة النسخة المحددة";
        toolTip.SetToolTip(btnRestoreBackup, "استعادة النسخة المحددة — تستبدل كل البيانات الحالية (F4)");
        btnRestoreBackup.UseVisualStyleBackColor = false;
        btnRestoreBackup.Click += btnRestoreBackup_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(446, 2);
        btnRefresh.Margin = new Padding(0, 0, 8, 0);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(130, 36);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "تحديث (F5)";
        toolTip.SetToolTip(btnRefresh, "إعادة تحميل كل بيانات الشاشة وإرجاع التصفية إلى الافتراضي (F5)");
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 176);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1100, 120);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(label2);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(label1);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(label3);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbFilter.ForeColor = Color.FromArgb(14, 82, 52);
        gbFilter.Location = new Point(136, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(8);
        gbFilter.RightToLeft = RightToLeft.Yes;
        gbFilter.Size = new Size(958, 112);
        gbFilter.TabIndex = 0;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.ForeColor = Color.FromArgb(14, 82, 52);
        label2.Location = new Point(838, 30);
        label2.Name = "label2";
        label2.Size = new Size(74, 22);
        label2.TabIndex = 0;
        label2.Text = "تصفية بـ:";
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbFilterBy.Items.AddRange(new object[] { "اسم الملف", "تاريخ النسخة" });
        cbFilterBy.Location = new Point(680, 26);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(150, 30);
        cbFilterBy.TabIndex = 1;
        toolTip.SetToolTip(cbFilterBy, "عمود التصفية: اسم الملف أو تاريخ النسخة");
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.BorderStyle = BorderStyle.FixedSingle;
        txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtFilterValue.Location = new Point(404, 26);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.Size = new Size(264, 31);
        txtFilterValue.TabIndex = 2;
        toolTip.SetToolTip(txtFilterValue, "تصفية فورية أثناء الكتابة — Esc يمسح التصفية");
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        txtFilterValue.KeyDown += txtFilterValue_KeyDown;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.ForeColor = Color.FromArgb(14, 82, 52);
        label1.Location = new Point(356, 30);
        label1.Name = "label1";
        label1.Size = new Size(34, 22);
        label1.TabIndex = 3;
        label1.Text = "من:";
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = Color.White;
        dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(216, 26);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(132, 29);
        dtpFrom.TabIndex = 4;
        toolTip.SetToolTip(dtpFrom, "بداية نطاق تاريخ النسخة");
        dtpFrom.ValueChanged += dtpRange_ValueChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.ForeColor = Color.FromArgb(14, 82, 52);
        label3.Location = new Point(168, 30);
        label3.Name = "label3";
        label3.Size = new Size(36, 22);
        label3.TabIndex = 5;
        label3.Text = "إلى:";
        // 
        // dtpTo
        // 
        dtpTo.BackColor = Color.White;
        dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(30, 26);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(130, 29);
        dtpTo.TabIndex = 6;
        toolTip.SetToolTip(dtpTo, "نهاية نطاق تاريخ النسخة");
        dtpTo.ValueChanged += dtpRange_ValueChanged;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = Color.FromArgb(23, 162, 184);
        btnRangeToday.Cursor = Cursors.Hand;
        btnRangeToday.FlatAppearance.BorderSize = 0;
        btnRangeToday.FlatStyle = FlatStyle.Flat;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRangeToday.ForeColor = Color.White;
        btnRangeToday.Location = new Point(808, 68);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(118, 30);
        btnRangeToday.TabIndex = 7;
        btnRangeToday.Text = "اليوم";
        toolTip.SetToolTip(btnRangeToday, "نسخ اليوم فقط");
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.Click += btnRangeToday_Click;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = Color.FromArgb(23, 162, 184);
        btnRangeMonth.Cursor = Cursors.Hand;
        btnRangeMonth.FlatAppearance.BorderSize = 0;
        btnRangeMonth.FlatStyle = FlatStyle.Flat;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRangeMonth.ForeColor = Color.White;
        btnRangeMonth.Location = new Point(682, 68);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(118, 30);
        btnRangeMonth.TabIndex = 8;
        btnRangeMonth.Text = "هذا الشهر";
        toolTip.SetToolTip(btnRangeMonth, "نسخ الشهر الحالي حتى اليوم");
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // btnRange30
        // 
        btnRange30.BackColor = Color.FromArgb(23, 162, 184);
        btnRange30.Cursor = Cursors.Hand;
        btnRange30.FlatAppearance.BorderSize = 0;
        btnRange30.FlatStyle = FlatStyle.Flat;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRange30.ForeColor = Color.White;
        btnRange30.Location = new Point(556, 68);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(118, 30);
        btnRange30.TabIndex = 9;
        btnRange30.Text = "آخر 30 يوماً";
        toolTip.SetToolTip(btnRange30, "نسخ آخر 30 يوماً");
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
        // 
        // btnClearFilters
        // 
        btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClearFilters.ForeColor = Color.White;
        btnClearFilters.Location = new Point(408, 68);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(140, 30);
        btnClearFilters.TabIndex = 10;
        btnClearFilters.Text = "مسح التصفية";
        toolTip.SetToolTip(btnClearFilters, "يمسح أدوات التصفية فقط بلا إعادة تحميل");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
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
        btnToggleFilters.Size = new Size(130, 112);
        btnToggleFilters.TabIndex = 1;
        btnToggleFilters.Text = "إخفاء التصفية";
        toolTip.SetToolTip(btnToggleFilters, "إخفاء/إظهار لوحة التصفية — F9");
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(170, 220, 170);
        pnlGrid.Controls.Add(lblEmpty);
        pnlGrid.Controls.Add(gbBackups);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 296);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(10);
        pnlGrid.Size = new Size(1100, 358);
        pnlGrid.TabIndex = 3;
        // 
        // lblEmpty
        // 
        lblEmpty.BackColor = Color.FromArgb(220, 240, 220);
        lblEmpty.Dock = DockStyle.Fill;
        lblEmpty.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEmpty.ForeColor = Color.FromArgb(14, 82, 52);
        lblEmpty.Location = new Point(10, 10);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Size = new Size(1080, 338);
        lblEmpty.TabIndex = 0;
        lblEmpty.Text = "لا توجد نسخ احتياطية بعد — اضغط «إنشاء نسخة احتياطية الآن» لإنشاء أول نسخة.";
        lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
        lblEmpty.Visible = false;
        // 
        // gbBackups
        // 
        gbBackups.BackColor = Color.FromArgb(235, 245, 235);
        gbBackups.Controls.Add(dgvBackups);
        gbBackups.Dock = DockStyle.Fill;
        gbBackups.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbBackups.ForeColor = Color.FromArgb(14, 82, 52);
        gbBackups.Location = new Point(10, 10);
        gbBackups.Name = "gbBackups";
        gbBackups.Padding = new Padding(10);
        gbBackups.Size = new Size(1080, 338);
        gbBackups.TabIndex = 1;
        gbBackups.TabStop = false;
        gbBackups.Text = "النسخ الاحتياطية المتاحة";
        // 
        // dgvBackups
        // 
        dgvBackups.AllowUserToAddRows = false;
        dgvBackups.AllowUserToDeleteRows = false;
        dgvBackups.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 252, 245);
        dgvBackups.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvBackups.AutoGenerateColumns = false;
        dgvBackups.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBackups.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvBackups.BorderStyle = BorderStyle.None;
        dgvBackups.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        dataGridViewCellStyle2.ForeColor = Color.White;
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(14, 165, 103);
        dataGridViewCellStyle2.SelectionForeColor = Color.White;
        dgvBackups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvBackups.ColumnHeadersHeight = 45;
        dgvBackups.Columns.AddRange(new DataGridViewColumn[] { colFileName, colCreatedAt });
        dgvBackups.ContextMenuStrip = cmsBackups;
        dataGridViewCellStyle3.BackColor = Color.White;
        dataGridViewCellStyle3.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 255, 180);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
        dgvBackups.DefaultCellStyle = dataGridViewCellStyle3;
        dgvBackups.Dock = DockStyle.Fill;
        dgvBackups.EnableHeadersVisualStyles = false;
        dgvBackups.GridColor = Color.FromArgb(205, 225, 205);
        dgvBackups.Location = new Point(10, 33);
        dgvBackups.Margin = new Padding(0);
        dgvBackups.MultiSelect = false;
        dgvBackups.Name = "dgvBackups";
        dgvBackups.ReadOnly = true;
        dgvBackups.RightToLeft = RightToLeft.Yes;
        dgvBackups.RowHeadersVisible = false;
        dgvBackups.RowTemplate.Height = 32;
        dgvBackups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBackups.Size = new Size(1060, 295);
        dgvBackups.TabIndex = 0;
        dgvBackups.CellDoubleClick += dgvBackups_CellDoubleClick;
        dgvBackups.CellFormatting += dgvBackups_CellFormatting;
        dgvBackups.SelectionChanged += dgvBackups_SelectionChanged;
        // 
        // colFileName
        // 
        colFileName.DataPropertyName = "FileName";
        colFileName.FillWeight = 60F;
        colFileName.HeaderText = "اسم الملف";
        colFileName.MinimumWidth = 300;
        colFileName.Name = "colFileName";
        colFileName.ReadOnly = true;
        colFileName.SortMode = DataGridViewColumnSortMode.Automatic;
        // 
        // colCreatedAt
        // 
        colCreatedAt.DataPropertyName = "CreatedAt";
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
        colCreatedAt.DefaultCellStyle = dataGridViewCellStyle4;
        colCreatedAt.FillWeight = 40F;
        colCreatedAt.HeaderText = "تاريخ النسخة";
        colCreatedAt.MinimumWidth = 220;
        colCreatedAt.Name = "colCreatedAt";
        colCreatedAt.ReadOnly = true;
        colCreatedAt.SortMode = DataGridViewColumnSortMode.Automatic;
        // 
        // cmsBackups
        // 
        cmsBackups.BackColor = Color.White;
        cmsBackups.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsBackups.ImageScalingSize = new Size(24, 24);
        cmsBackups.Items.AddRange(new ToolStripItem[] { createBackupToolStripMenuItem, restoreToolStripMenuItem, toolStripSeparator1, refreshToolStripMenuItem });
        cmsBackups.Name = "cmsBackups";
        cmsBackups.RightToLeft = RightToLeft.Yes;
        cmsBackups.Size = new Size(226, 100);
        cmsBackups.Opening += cmsBackups_Opening;
        // 
        // createBackupToolStripMenuItem
        // 
        createBackupToolStripMenuItem.Name = "createBackupToolStripMenuItem";
        createBackupToolStripMenuItem.Size = new Size(225, 30);
        createBackupToolStripMenuItem.Text = "إنشاء نسخة احتياطية";
        createBackupToolStripMenuItem.Click += createBackupToolStripMenuItem_Click;
        // 
        // restoreToolStripMenuItem
        // 
        restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
        restoreToolStripMenuItem.Size = new Size(225, 30);
        restoreToolStripMenuItem.Text = "استعادة النسخة المحددة";
        restoreToolStripMenuItem.Click += restoreToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(222, 6);
        // 
        // refreshToolStripMenuItem
        // 
        refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
        refreshToolStripMenuItem.Size = new Size(225, 30);
        refreshToolStripMenuItem.Text = "تحديث";
        refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(gbSummary);
        pnlSummary.Dock = DockStyle.Bottom;
        pnlSummary.Location = new Point(0, 654);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Padding = new Padding(6, 4, 6, 4);
        pnlSummary.Size = new Size(1100, 62);
        pnlSummary.TabIndex = 4;
        // 
        // gbSummary
        // 
        gbSummary.BackColor = Color.FromArgb(235, 245, 235);
        gbSummary.Controls.Add(lblSummary);
        gbSummary.Dock = DockStyle.Fill;
        gbSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbSummary.ForeColor = Color.FromArgb(14, 82, 52);
        gbSummary.Location = new Point(6, 4);
        gbSummary.Name = "gbSummary";
        gbSummary.Padding = new Padding(8, 0, 8, 0);
        gbSummary.Size = new Size(1088, 54);
        gbSummary.TabIndex = 0;
        gbSummary.TabStop = false;
        gbSummary.Text = "الإجماليات";
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(8, 20);
        lblSummary.Name = "lblSummary";
        lblSummary.Size = new Size(1072, 26);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "عدد النسخ: 0 · آخر نسخة: —";
        lblSummary.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(flpFooter);
        pnlFooter.Controls.Add(lblStatus);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 716);
        pnlFooter.Margin = new Padding(0);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(8);
        pnlFooter.Size = new Size(1100, 60);
        pnlFooter.TabIndex = 5;
        // 
        // lblStatus
        // 
        lblStatus.BackColor = Color.FromArgb(235, 240, 235);
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStatus.ForeColor = Color.FromArgb(14, 82, 52);
        lblStatus.Location = new Point(8, 8);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(6, 0, 6, 0);
        lblStatus.Size = new Size(904, 44);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // flpFooter
        // 
        flpFooter.BackColor = Color.FromArgb(235, 240, 235);
        flpFooter.Controls.Add(btnClose);
        flpFooter.Dock = DockStyle.Right;
        flpFooter.FlowDirection = FlowDirection.LeftToRight;
        flpFooter.Location = new Point(912, 8);
        flpFooter.Name = "flpFooter";
        flpFooter.Padding = new Padding(8, 2, 8, 2);
        flpFooter.Size = new Size(180, 44);
        flpFooter.TabIndex = 1;
        flpFooter.WrapContents = false;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.Cursor = Cursors.Hand;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(8, 2);
        btnClose.Margin = new Padding(0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 40);
        btnClose.TabIndex = 0;
        btnClose.Text = "إغلاق (Esc)";
        toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmBackup
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(1100, 776);
        Controls.Add(pnlGrid);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFooter);
        Controls.Add(pnlFilters);
        Controls.Add(pnlToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = true;
        MinimumSize = new Size(980, 620);
        Name = "frmBackup";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "النسخ الاحتياطي";
        Load += frmBackup_Load;
        pnlHeader.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        gbActions.ResumeLayout(false);
        flpActions.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlGrid.ResumeLayout(false);
        gbBackups.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvBackups).EndInit();
        cmsBackups.ResumeLayout(false);
        pnlSummary.ResumeLayout(false);
        gbSummary.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        flpFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel pnlHeader;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Panel pnlToolbar;
    private System.Windows.Forms.GroupBox gbActions;
    private System.Windows.Forms.FlowLayoutPanel flpActions;
    private System.Windows.Forms.Button btnCreateBackup;
    private System.Windows.Forms.Button btnRestoreBackup;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Panel pnlFilters;
    private System.Windows.Forms.GroupBox gbFilter;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbFilterBy;
    private System.Windows.Forms.TextBox txtFilterValue;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.DateTimePicker dtpFrom;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.DateTimePicker dtpTo;
    private System.Windows.Forms.Button btnRangeToday;
    private System.Windows.Forms.Button btnRangeMonth;
    private System.Windows.Forms.Button btnRange30;
    private System.Windows.Forms.Button btnClearFilters;
    private System.Windows.Forms.Button btnToggleFilters;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.Label lblEmpty;
    private System.Windows.Forms.GroupBox gbBackups;
    private System.Windows.Forms.DataGridView dgvBackups;
    private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
    private System.Windows.Forms.ContextMenuStrip cmsBackups;
    private System.Windows.Forms.ToolStripMenuItem createBackupToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    private System.Windows.Forms.Panel pnlSummary;
    private System.Windows.Forms.GroupBox gbSummary;
    private System.Windows.Forms.Label lblSummary;
    private System.Windows.Forms.Panel pnlFooter;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.FlowLayoutPanel flpFooter;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.ToolTip toolTip;
}
