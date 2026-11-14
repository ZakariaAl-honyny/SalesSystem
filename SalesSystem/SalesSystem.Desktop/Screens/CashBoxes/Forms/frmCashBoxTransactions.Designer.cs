namespace SalesSystem.Desktop.Screens.CashBoxes.Forms;

partial class frmCashBoxTransactions
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
        pnlHeader = new Panel();
        lblMode = new Label();
        gbToolbar = new GroupBox();
        flpToolbar = new FlowLayoutPanel();
        btnRefresh = new Button();
        btnClose = new Button();
        pnlFilters = new Panel();
        gbFilter = new GroupBox();
        lblCashBox = new Label();
        cmbCashBox = new ComboBox();
        lblFilterBy = new Label();
        cbFilterBy = new ComboBox();
        txtFilterValue = new TextBox();
        lblStatus = new Label();
        cbStatus = new ComboBox();
        lblFrom = new Label();
        dtpFrom = new DateTimePicker();
        lblTo = new Label();
        dtpTo = new DateTimePicker();
        btnRangeToday = new Button();
        btnRangeMonth = new Button();
        btnRange30 = new Button();
        chkOnlyCancelled = new CheckBox();
        chkShowInactive = new CheckBox();
        btnClearFilters = new Button();
        btnToggleFilters = new Button();
        pnlSummary = new Panel();
        lblSummary = new Label();
        lblRecordsCount = new Label();
        pnlGrid = new Panel();
        dgvTransactions = new DataGridView();
        cmsTransactions = new ContextMenuStrip(components);
        tsmiOpenDocument = new ToolStripMenuItem();
        tsmiExport = new ToolStripMenuItem();
        tsmiRefresh = new ToolStripMenuItem();
        tsSep1 = new ToolStripSeparator();
        lblEmpty = new Label();
        pnlFooter = new Panel();
        lblHint = new Label();
        toolTip1 = new ToolTip(components);
        pnlHeader.SuspendLayout();
        gbToolbar.SuspendLayout();
        flpToolbar.SuspendLayout();
        pnlFilters.SuspendLayout();
        gbFilter.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlGrid.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
        cmsTransactions.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
        pnlHeader.Controls.Add(lblMode);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(1320, 50);
        pnlHeader.TabIndex = 0;
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Fill;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Margin = new Padding(0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(1320, 50);
        lblMode.TabIndex = 0;
        lblMode.Text = "حركات الصندوق";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbToolbar
        // 
        gbToolbar.BackColor = Color.FromArgb(235, 245, 235);
        gbToolbar.Controls.Add(flpToolbar);
        gbToolbar.Dock = DockStyle.Top;
        gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
        gbToolbar.Location = new Point(0, 50);
        gbToolbar.Margin = new Padding(0);
        gbToolbar.Name = "gbToolbar";
        gbToolbar.Padding = new Padding(8, 4, 8, 4);
        gbToolbar.Size = new Size(1320, 76);
        gbToolbar.TabIndex = 1;
        gbToolbar.TabStop = false;
        gbToolbar.Text = "أوامر الشاشة";
        // 
        // flpToolbar
        // 
        flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
        flpToolbar.Controls.Add(btnRefresh);
        flpToolbar.Controls.Add(btnClose);
        flpToolbar.Dock = DockStyle.Fill;
        flpToolbar.FlowDirection = FlowDirection.RightToLeft;
        flpToolbar.Location = new Point(8, 24);
        flpToolbar.Margin = new Padding(0);
        flpToolbar.Name = "flpToolbar";
        flpToolbar.Padding = new Padding(6, 0, 6, 0);
        flpToolbar.Size = new Size(1304, 40);
        flpToolbar.TabIndex = 0;
        flpToolbar.WrapContents = false;
        // 
        // btnRefresh
        // 
        btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = Color.White;
        btnRefresh.Location = new Point(1152, 4);
        btnRefresh.Margin = new Padding(4);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(140, 34);
        btnRefresh.TabIndex = 0;
        btnRefresh.Text = "تحديث (F5)";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(220, 53, 69);
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(1004, 4);
        btnClose.Margin = new Padding(4);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(140, 34);
        btnClose.TabIndex = 1;
        btnClose.Text = "إغلاق (Esc)";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // pnlFilters
        // 
        pnlFilters.BackColor = Color.FromArgb(235, 245, 235);
        pnlFilters.Controls.Add(gbFilter);
        pnlFilters.Controls.Add(btnToggleFilters);
        pnlFilters.Dock = DockStyle.Top;
        pnlFilters.Location = new Point(0, 126);
        pnlFilters.Margin = new Padding(0);
        pnlFilters.Name = "pnlFilters";
        pnlFilters.Padding = new Padding(6, 4, 6, 4);
        pnlFilters.Size = new Size(1320, 112);
        pnlFilters.TabIndex = 2;
        // 
        // gbFilter
        // 
        gbFilter.BackColor = Color.FromArgb(235, 245, 235);
        gbFilter.Controls.Add(lblCashBox);
        gbFilter.Controls.Add(cmbCashBox);
        gbFilter.Controls.Add(lblFilterBy);
        gbFilter.Controls.Add(cbFilterBy);
        gbFilter.Controls.Add(txtFilterValue);
        gbFilter.Controls.Add(lblStatus);
        gbFilter.Controls.Add(cbStatus);
        gbFilter.Controls.Add(lblFrom);
        gbFilter.Controls.Add(dtpFrom);
        gbFilter.Controls.Add(lblTo);
        gbFilter.Controls.Add(dtpTo);
        gbFilter.Controls.Add(btnRangeToday);
        gbFilter.Controls.Add(btnRangeMonth);
        gbFilter.Controls.Add(btnRange30);
        gbFilter.Controls.Add(chkOnlyCancelled);
        gbFilter.Controls.Add(chkShowInactive);
        gbFilter.Controls.Add(btnClearFilters);
        gbFilter.Dock = DockStyle.Fill;
        gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbFilter.ForeColor = Color.FromArgb(14, 82, 52);
        gbFilter.Location = new Point(152, 4);
        gbFilter.Margin = new Padding(0);
        gbFilter.Name = "gbFilter";
        gbFilter.Padding = new Padding(10);
        gbFilter.Size = new Size(1162, 104);
        gbFilter.TabIndex = 1;
        gbFilter.TabStop = false;
        gbFilter.Text = "بحث وتصفية";
        // 
        // lblCashBox
        // 
        lblCashBox.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCashBox.ForeColor = Color.FromArgb(14, 82, 52);
        lblCashBox.Location = new Point(1050, 32);
        lblCashBox.Margin = new Padding(0);
        lblCashBox.Name = "lblCashBox";
        lblCashBox.Size = new Size(85, 26);
        lblCashBox.TabIndex = 0;
        lblCashBox.Text = "الصندوق:";
        lblCashBox.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cmbCashBox
        // 
        cmbCashBox.BackColor = Color.White;
        cmbCashBox.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCashBox.FlatStyle = FlatStyle.Flat;
        cmbCashBox.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmbCashBox.FormattingEnabled = true;
        cmbCashBox.Location = new Point(855, 28);
        cmbCashBox.Margin = new Padding(0);
        cmbCashBox.Name = "cmbCashBox";
        cmbCashBox.Size = new Size(185, 32);
        cmbCashBox.TabIndex = 1;
        cmbCashBox.SelectedIndexChanged += cmbCashBox_SelectedIndexChanged;
        // 
        // lblFilterBy
        // 
        lblFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFilterBy.ForeColor = Color.FromArgb(14, 82, 52);
        lblFilterBy.Location = new Point(758, 32);
        lblFilterBy.Margin = new Padding(0);
        lblFilterBy.Name = "lblFilterBy";
        lblFilterBy.Size = new Size(90, 26);
        lblFilterBy.TabIndex = 2;
        lblFilterBy.Text = "تصفية بـ:";
        lblFilterBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbFilterBy
        // 
        cbFilterBy.BackColor = Color.White;
        cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
        cbFilterBy.FlatStyle = FlatStyle.Flat;
        cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbFilterBy.FormattingEnabled = true;
        cbFilterBy.Items.AddRange(new object[] { "كل الحقول", "رقم المستند", "البيان", "النوع" });
        cbFilterBy.Location = new Point(600, 28);
        cbFilterBy.Margin = new Padding(0);
        cbFilterBy.Name = "cbFilterBy";
        cbFilterBy.Size = new Size(150, 32);
        cbFilterBy.TabIndex = 3;
        cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
        // 
        // txtFilterValue
        // 
        txtFilterValue.BackColor = Color.White;
        txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtFilterValue.Location = new Point(385, 28);
        txtFilterValue.Margin = new Padding(0);
        txtFilterValue.Name = "txtFilterValue";
        txtFilterValue.PlaceholderText = "اكتب قيمة التصفية...";
        txtFilterValue.Size = new Size(205, 32);
        txtFilterValue.TabIndex = 4;
        txtFilterValue.TextChanged += txtFilterValue_TextChanged;
        // 
        // lblStatus
        // 
        lblStatus.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblStatus.ForeColor = Color.FromArgb(14, 82, 52);
        lblStatus.Location = new Point(292, 32);
        lblStatus.Margin = new Padding(0);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(85, 26);
        lblStatus.TabIndex = 5;
        lblStatus.Text = "الحالة:";
        lblStatus.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cbStatus
        // 
        cbStatus.BackColor = Color.White;
        cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbStatus.FlatStyle = FlatStyle.Flat;
        cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbStatus.FormattingEnabled = true;
        cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "مرحّل", "ملغى" });
        cbStatus.Location = new Point(165, 28);
        cbStatus.Margin = new Padding(0);
        cbStatus.Name = "cbStatus";
        cbStatus.Size = new Size(120, 32);
        cbStatus.TabIndex = 6;
        cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        // 
        // lblFrom
        // 
        lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblFrom.ForeColor = Color.FromArgb(14, 82, 52);
        lblFrom.Location = new Point(1068, 64);
        lblFrom.Margin = new Padding(0);
        lblFrom.Name = "lblFrom";
        lblFrom.Size = new Size(60, 26);
        lblFrom.TabIndex = 7;
        lblFrom.Text = "من:";
        lblFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFrom
        // 
        dtpFrom.BackColor = Color.White;
        dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpFrom.Format = DateTimePickerFormat.Short;
        dtpFrom.Location = new Point(905, 60);
        dtpFrom.Margin = new Padding(0);
        dtpFrom.Name = "dtpFrom";
        dtpFrom.Size = new Size(155, 32);
        dtpFrom.TabIndex = 8;
        dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        // 
        // lblTo
        // 
        lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTo.ForeColor = Color.FromArgb(14, 82, 52);
        lblTo.Location = new Point(845, 64);
        lblTo.Margin = new Padding(0);
        lblTo.Name = "lblTo";
        lblTo.Size = new Size(52, 26);
        lblTo.TabIndex = 9;
        lblTo.Text = "إلى:";
        lblTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpTo
        // 
        dtpTo.BackColor = Color.White;
        dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        dtpTo.Format = DateTimePickerFormat.Short;
        dtpTo.Location = new Point(682, 60);
        dtpTo.Margin = new Padding(0);
        dtpTo.Name = "dtpTo";
        dtpTo.Size = new Size(155, 32);
        dtpTo.TabIndex = 10;
        dtpTo.ValueChanged += dtpTo_ValueChanged;
        // 
        // btnRangeToday
        // 
        btnRangeToday.BackColor = Color.FromArgb(248, 249, 250);
        btnRangeToday.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRangeToday.FlatAppearance.BorderSize = 1;
        btnRangeToday.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        btnRangeToday.FlatStyle = FlatStyle.Popup;
        btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRangeToday.ForeColor = Color.FromArgb(33, 37, 41);
        btnRangeToday.Location = new Point(578, 58);
        btnRangeToday.Margin = new Padding(0);
        btnRangeToday.Name = "btnRangeToday";
        btnRangeToday.Size = new Size(96, 34);
        btnRangeToday.TabIndex = 11;
        btnRangeToday.Text = "اليوم";
        btnRangeToday.UseVisualStyleBackColor = false;
        btnRangeToday.Click += btnRangeToday_Click;
        // 
        // btnRangeMonth
        // 
        btnRangeMonth.BackColor = Color.FromArgb(248, 249, 250);
        btnRangeMonth.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRangeMonth.FlatAppearance.BorderSize = 1;
        btnRangeMonth.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        btnRangeMonth.FlatStyle = FlatStyle.Popup;
        btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRangeMonth.ForeColor = Color.FromArgb(33, 37, 41);
        btnRangeMonth.Location = new Point(448, 58);
        btnRangeMonth.Margin = new Padding(0);
        btnRangeMonth.Name = "btnRangeMonth";
        btnRangeMonth.Size = new Size(122, 34);
        btnRangeMonth.TabIndex = 12;
        btnRangeMonth.Text = "هذا الشهر";
        btnRangeMonth.UseVisualStyleBackColor = false;
        btnRangeMonth.Click += btnRangeMonth_Click;
        // 
        // btnRange30
        // 
        btnRange30.BackColor = Color.FromArgb(248, 249, 250);
        btnRange30.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
        btnRange30.FlatAppearance.BorderSize = 1;
        btnRange30.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
        btnRange30.FlatStyle = FlatStyle.Popup;
        btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnRange30.ForeColor = Color.FromArgb(33, 37, 41);
        btnRange30.Location = new Point(316, 58);
        btnRange30.Margin = new Padding(0);
        btnRange30.Name = "btnRange30";
        btnRange30.Size = new Size(124, 34);
        btnRange30.TabIndex = 13;
        btnRange30.Text = "آخر 30 يوماً";
        btnRange30.UseVisualStyleBackColor = false;
        btnRange30.Click += btnRange30_Click;
        // 
        // chkOnlyCancelled
        // 
        chkOnlyCancelled.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        chkOnlyCancelled.ForeColor = Color.FromArgb(14, 82, 52);
        chkOnlyCancelled.Location = new Point(158, 62);
        chkOnlyCancelled.Margin = new Padding(0);
        chkOnlyCancelled.Name = "chkOnlyCancelled";
        chkOnlyCancelled.Size = new Size(150, 28);
        chkOnlyCancelled.TabIndex = 14;
        chkOnlyCancelled.Text = "الملغاة فقط";
        chkOnlyCancelled.UseVisualStyleBackColor = false;
        chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
        // 
        // chkShowInactive
        // 
        chkShowInactive.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        chkShowInactive.ForeColor = Color.FromArgb(14, 82, 52);
        chkShowInactive.Location = new Point(12, 62);
        chkShowInactive.Margin = new Padding(0);
        chkShowInactive.Name = "chkShowInactive";
        chkShowInactive.Size = new Size(140, 28);
        chkShowInactive.TabIndex = 15;
        chkShowInactive.Text = "عرض غير النشط";
        chkShowInactive.UseVisualStyleBackColor = false;
        chkShowInactive.CheckedChanged += chkShowInactive_CheckedChanged;
        // 
        // btnClearFilters
        // 
        // RULE-N148: «مسح التصفية» يمسح أدوات التصفية فقط بلا إعادة تحميل (RULE-N134 — القيم حرفية).
        // الموضع: نهاية الصف الأول من اليمين إلى اليسار (RULE-N127) داخل padding الصندوق (10) ومحاذٍ لعمود
        // chkShowInactive في الصف الثاني (x = 12)، وبفاصل 8px عن cbStatus (x = 165) — لا تقاطع مع أي عنصر.
        btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
        btnClearFilters.Cursor = Cursors.Hand;
        btnClearFilters.Enabled = false;
        btnClearFilters.FlatAppearance.BorderSize = 0;
        btnClearFilters.FlatStyle = FlatStyle.Flat;
        btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
        btnClearFilters.ForeColor = Color.White;
        btnClearFilters.Location = new Point(12, 28);
        btnClearFilters.Margin = new Padding(0);
        btnClearFilters.Name = "btnClearFilters";
        btnClearFilters.Size = new Size(145, 32);
        btnClearFilters.TabIndex = 16;
        btnClearFilters.Text = "🧹 مسح التصفية";
        toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
        btnClearFilters.UseVisualStyleBackColor = false;
        btnClearFilters.Click += btnClearFilters_Click;
        // 
        // btnToggleFilters
        // 
        // RULE-N145: زر «إخفاء/إظهار التصفية» مُعلَن في المصمّم بقيم حرفية مطابقة لـ
        // ThemeHelper.Secondary + FilterPanelHelper حتى يظهر في المصمّم كما يظهر وقت التشغيل.
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
        btnToggleFilters.Margin = new Padding(0);
        btnToggleFilters.Name = "btnToggleFilters";
        btnToggleFilters.Size = new Size(140, 104);
        btnToggleFilters.TabIndex = 2;
        btnToggleFilters.Text = "إخفاء التصفية";
        btnToggleFilters.UseVisualStyleBackColor = false;
        // 
        // pnlSummary
        // 
        pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
        pnlSummary.Controls.Add(lblSummary);
        pnlSummary.Controls.Add(lblRecordsCount);
        pnlSummary.Dock = DockStyle.Top;
        pnlSummary.Location = new Point(0, 238);
        pnlSummary.Margin = new Padding(0);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.Size = new Size(1320, 40);
        pnlSummary.TabIndex = 3;
        // 
        // lblSummary
        // 
        lblSummary.Dock = DockStyle.Fill;
        lblSummary.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
        lblSummary.Location = new Point(90, 0);
        lblSummary.Margin = new Padding(0);
        lblSummary.Name = "lblSummary";
        lblSummary.Padding = new Padding(10, 0, 10, 0);
        lblSummary.Size = new Size(1230, 40);
        lblSummary.TabIndex = 0;
        lblSummary.Text = "عدد الحركات: 0 — الوارد: 0.00 · الصادر: 0.00 · الرصيد: 0.00";
        lblSummary.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblRecordsCount
        // 
        lblRecordsCount.Dock = DockStyle.Left;
        lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
        lblRecordsCount.Location = new Point(0, 0);
        lblRecordsCount.Margin = new Padding(0);
        lblRecordsCount.Name = "lblRecordsCount";
        lblRecordsCount.Size = new Size(90, 40);
        lblRecordsCount.TabIndex = 1;
        lblRecordsCount.Text = "0";
        lblRecordsCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlGrid
        // 
        pnlGrid.BackColor = Color.FromArgb(220, 240, 220);
        pnlGrid.Controls.Add(dgvTransactions);
        pnlGrid.Controls.Add(lblEmpty);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 278);
        pnlGrid.Margin = new Padding(0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(6);
        pnlGrid.Size = new Size(1320, 408);
        pnlGrid.TabIndex = 4;
        // 
        // dgvTransactions
        // 
        dgvTransactions.AllowUserToAddRows = false;
        dgvTransactions.AllowUserToDeleteRows = false;
        dgvTransactions.AllowUserToResizeRows = false;
        dgvTransactions.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(245, 252, 245)
        };
        dgvTransactions.AutoGenerateColumns = false;
        dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvTransactions.BackgroundColor = Color.FromArgb(220, 240, 220);
        dgvTransactions.BorderStyle = BorderStyle.None;
        dgvTransactions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        dgvTransactions.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleCenter,
            BackColor = Color.FromArgb(14, 165, 103),
            Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0),
            ForeColor = Color.White,
            SelectionBackColor = Color.FromArgb(14, 165, 103),
            WrapMode = DataGridViewTriState.True
        };
        dgvTransactions.ColumnHeadersHeight = 45;
        dgvTransactions.ContextMenuStrip = cmsTransactions;
        dgvTransactions.DefaultCellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleCenter,
            BackColor = Color.White,
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0),
            ForeColor = Color.FromArgb(33, 37, 41),
            SelectionBackColor = Color.FromArgb(255, 255, 180),
            SelectionForeColor = Color.FromArgb(33, 37, 41),
            WrapMode = DataGridViewTriState.False
        };
        dgvTransactions.Dock = DockStyle.Fill;
        dgvTransactions.EnableHeadersVisualStyles = false;
        dgvTransactions.GridColor = Color.FromArgb(205, 225, 205);
        dgvTransactions.Location = new Point(6, 6);
        dgvTransactions.Margin = new Padding(0);
        dgvTransactions.MultiSelect = false;
        dgvTransactions.Name = "dgvTransactions";
        dgvTransactions.ReadOnly = true;
        dgvTransactions.RowHeadersVisible = false;
        dgvTransactions.RowHeadersWidth = 62;
        dgvTransactions.RowTemplate.Height = 32;
        dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTransactions.Size = new Size(1308, 396);
        dgvTransactions.TabIndex = 0;
        dgvTransactions.ShowCellToolTips = true;
        dgvTransactions.KeyDown += dgvTransactions_KeyDown;
        // 
        // cmsTransactions
        // 
        cmsTransactions.BackColor = Color.White;
        cmsTransactions.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cmsTransactions.ImageScalingSize = new Size(24, 24);
        cmsTransactions.Items.AddRange(new ToolStripItem[] { tsmiOpenDocument, tsSep1, tsmiExport, tsmiRefresh });
        cmsTransactions.Name = "cmsTransactions";
        cmsTransactions.RightToLeft = RightToLeft.Yes;
        cmsTransactions.Size = new Size(180, 130);
        cmsTransactions.Opening += cmsTransactions_Opening;
        // 
        // tsmiOpenDocument
        // 
        tsmiOpenDocument.Name = "tsmiOpenDocument";
        tsmiOpenDocument.ShortcutKeyDisplayString = "Enter";
        tsmiOpenDocument.Size = new Size(179, 30);
        tsmiOpenDocument.Text = "فتح المستند";
        tsmiOpenDocument.Click += tsmiOpenDocument_Click;
        // 
        // tsSep1
        // 
        tsSep1.Name = "tsSep1";
        tsSep1.Size = new Size(176, 6);
        // 
        // tsmiExport
        // 
        tsmiExport.Name = "tsmiExport";
        tsmiExport.Size = new Size(179, 30);
        tsmiExport.Text = "تصدير إلى Excel";
        tsmiExport.Click += tsmiExport_Click;
        // 
        // tsmiRefresh
        // 
        tsmiRefresh.Name = "tsmiRefresh";
        tsmiRefresh.ShortcutKeyDisplayString = "F5";
        tsmiRefresh.Size = new Size(179, 30);
        tsmiRefresh.Text = "تحديث";
        tsmiRefresh.Click += tsmiRefresh_Click;
        // 
        // lblEmpty
        // 
        lblEmpty.BackColor = Color.FromArgb(220, 240, 220);
        lblEmpty.Dock = DockStyle.Fill;
        lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblEmpty.ForeColor = Color.FromArgb(14, 82, 52);
        lblEmpty.Location = new Point(6, 6);
        lblEmpty.Margin = new Padding(0);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Size = new Size(1308, 396);
        lblEmpty.TabIndex = 1;
        lblEmpty.Text = "لا توجد بيانات للعرض";
        lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
        lblEmpty.Visible = false;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(lblHint);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 694);
        pnlFooter.Margin = new Padding(0);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(1320, 34);
        pnlFooter.TabIndex = 5;
        // 
        // lblHint
        // 
        lblHint.Dock = DockStyle.Fill;
        lblHint.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblHint.ForeColor = Color.FromArgb(14, 82, 52);
        lblHint.Location = new Point(0, 0);
        lblHint.Margin = new Padding(0);
        lblHint.Name = "lblHint";
        lblHint.Padding = new Padding(10, 0, 10, 0);
        lblHint.Size = new Size(1320, 34);
        lblHint.TabIndex = 0;
        lblHint.Text = "F5 تحديث · F9 إخفاء/إظهار التصفية · Enter فتح المستند · Esc مسح التصفية/إغلاق";
        lblHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // toolTip1
        // 
        toolTip1.AutomaticDelay = 300;
        toolTip1.BackColor = Color.White;
        toolTip1.ForeColor = Color.Black;
        // 
        // frmCashBoxTransactions
        // 
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(1320, 728);
        Controls.Add(pnlGrid);
        Controls.Add(pnlFooter);
        Controls.Add(pnlSummary);
        Controls.Add(pnlFilters);
        Controls.Add(gbToolbar);
        Controls.Add(pnlHeader);
        Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        KeyPreview = true;
        MinimumSize = new Size(1100, 640);
        Name = "frmCashBoxTransactions";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "حركات الصندوق";
        Load += frmCashBoxTransactions_Load;
        pnlHeader.ResumeLayout(false);
        gbToolbar.ResumeLayout(false);
        flpToolbar.ResumeLayout(false);
        pnlFilters.ResumeLayout(false);
        gbFilter.ResumeLayout(false);
        gbFilter.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
        cmsTransactions.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Panel pnlHeader;
    private Label lblMode;
    private GroupBox gbToolbar;
    private FlowLayoutPanel flpToolbar;
    private Button btnRefresh;
    private Button btnClose;
    private Panel pnlFilters;
    private GroupBox gbFilter;
    private Label lblCashBox;
    private ComboBox cmbCashBox;
    private Label lblFilterBy;
    private ComboBox cbFilterBy;
    private TextBox txtFilterValue;
    private Label lblStatus;
    private ComboBox cbStatus;
    private Label lblFrom;
    private DateTimePicker dtpFrom;
    private Label lblTo;
    private DateTimePicker dtpTo;
    private Button btnRangeToday;
    private Button btnRangeMonth;
    private Button btnRange30;
    private CheckBox chkOnlyCancelled;
    private CheckBox chkShowInactive;
    private Button btnClearFilters;
    private Button btnToggleFilters;
    private Panel pnlSummary;
    private Label lblSummary;
    private Label lblRecordsCount;
    private Panel pnlGrid;
    private DataGridView dgvTransactions;
    private ContextMenuStrip cmsTransactions;
    private ToolStripMenuItem tsmiOpenDocument;
    private ToolStripMenuItem tsmiExport;
    private ToolStripMenuItem tsmiRefresh;
    private ToolStripSeparator tsSep1;
    private Label lblEmpty;
    private Panel pnlFooter;
    private Label lblHint;
    private ToolTip toolTip1;
}
