namespace SalesSystem.Desktop.Screens.Returns.Forms
{
    partial class frmListReturns
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            pnlHeader = new Panel();
            lblMode = new Label();
            pnlToolbar = new Panel();
            flpToolbar = new FlowLayoutPanel();
            btnAdd = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            flpFilterRow2 = new FlowLayoutPanel();
            btnRangeToday = new Button();
            btnRangeMonth = new Button();
            btnRange30 = new Button();
            chkOnlyCancelled = new CheckBox();
            btnClearFilters = new Button();
            flpFilterRow1 = new FlowLayoutPanel();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label1 = new Label();
            cbStatus = new ComboBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnToggleFilters = new Button();
            pnlSummary = new Panel();
            flpSummary = new FlowLayoutPanel();
            lblSummary = new Label();
            lblCountCaption = new Label();
            lblRecordsCount = new Label();
            tabReturns = new TabControl();
            tpSales = new TabPage();
            lblNoSales = new Label();
            dgvSales = new DataGridView();
            ctxMenu = new ContextMenuStrip(components);
            tsmiOpen = new ToolStripMenuItem();
            tsSep1 = new ToolStripSeparator();
            tsmiRefresh = new ToolStripMenuItem();
            tpPurchases = new TabPage();
            lblNoPurchases = new Label();
            dgvPurchases = new DataGridView();
            toolTip = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            flpFilterRow2.SuspendLayout();
            flpFilterRow1.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            tabReturns.SuspendLayout();
            tpSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            ctxMenu.SuspendLayout();
            tpPurchases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(16, 150, 96);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1564, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(16, 150, 96);
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1564, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "إدارة المرتجعات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1564, 88);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = Color.White;
            flpToolbar.Controls.Add(btnAdd);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(8);
            flpToolbar.Size = new Size(1564, 88);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(40, 167, 69);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(1442, 8);
            btnAdd.Margin = new Padding(6, 0, 6, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 60);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "+ جديد";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1330, 8);
            btnRefresh.Margin = new Padding(6, 0, 6, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 60);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1218, 8);
            btnClose.Margin = new Padding(6, 0, 6, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 60);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = Color.White;
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 144);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1564, 112);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = Color.White;
            gbFilter.Controls.Add(flpFilterRow2);
            gbFilter.Controls.Add(flpFilterRow1);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10, 6, 10, 6);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1412, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // flpFilterRow2
            // 
            flpFilterRow2.Controls.Add(btnRangeToday);
            flpFilterRow2.Controls.Add(btnRangeMonth);
            flpFilterRow2.Controls.Add(btnRange30);
            flpFilterRow2.Controls.Add(chkOnlyCancelled);
            flpFilterRow2.Controls.Add(btnClearFilters);
            flpFilterRow2.Dock = DockStyle.Fill;
            flpFilterRow2.Location = new Point(10, 66);
            flpFilterRow2.Margin = new Padding(0);
            flpFilterRow2.Name = "flpFilterRow2";
            flpFilterRow2.Size = new Size(1392, 32);
            flpFilterRow2.TabIndex = 1;
            flpFilterRow2.WrapContents = false;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeToday.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeToday.FlatStyle = FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeToday.Location = new Point(1297, 0);
            btnRangeToday.Margin = new Padding(4, 0, 4, 0);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(91, 32);
            btnRangeToday.TabIndex = 0;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeMonth.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeMonth.FlatStyle = FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeMonth.Location = new Point(1170, 0);
            btnRangeMonth.Margin = new Padding(4, 0, 4, 0);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(119, 32);
            btnRangeMonth.TabIndex = 1;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.FromArgb(248, 249, 250);
            btnRange30.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRange30.FlatStyle = FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.ForeColor = Color.FromArgb(33, 37, 41);
            btnRange30.Location = new Point(1032, 0);
            btnRange30.Margin = new Padding(4, 0, 4, 0);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(130, 32);
            btnRange30.TabIndex = 2;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // chkOnlyCancelled
            // 
            chkOnlyCancelled.AutoSize = true;
            chkOnlyCancelled.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            chkOnlyCancelled.ForeColor = Color.FromArgb(33, 37, 41);
            chkOnlyCancelled.Location = new Point(889, 6);
            chkOnlyCancelled.Margin = new Padding(4, 6, 4, 0);
            chkOnlyCancelled.Name = "chkOnlyCancelled";
            chkOnlyCancelled.Size = new Size(135, 26);
            chkOnlyCancelled.TabIndex = 3;
            chkOnlyCancelled.Text = "الملغاة فقط";
            chkOnlyCancelled.UseVisualStyleBackColor = false;
            chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
            // 
            // btnClearFilters
            // 
            btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(676, 0);
            btnClearFilters.Margin = new Padding(16, 0, 4, 0);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(193, 32);
            btnClearFilters.TabIndex = 4;
            btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
            toolTip.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            // 
            // flpFilterRow1
            // 
            flpFilterRow1.Controls.Add(label2);
            flpFilterRow1.Controls.Add(cbFilterBy);
            flpFilterRow1.Controls.Add(txtFilterValue);
            flpFilterRow1.Controls.Add(label1);
            flpFilterRow1.Controls.Add(cbStatus);
            flpFilterRow1.Controls.Add(label3);
            flpFilterRow1.Controls.Add(dtpFrom);
            flpFilterRow1.Controls.Add(label4);
            flpFilterRow1.Controls.Add(dtpTo);
            flpFilterRow1.Dock = DockStyle.Top;
            flpFilterRow1.Location = new Point(10, 28);
            flpFilterRow1.Margin = new Padding(0);
            flpFilterRow1.Name = "flpFilterRow1";
            flpFilterRow1.Size = new Size(1392, 38);
            flpFilterRow1.TabIndex = 0;
            flpFilterRow1.WrapContents = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(33, 37, 41);
            label2.Location = new Point(1300, 8);
            label2.Margin = new Padding(6, 8, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(86, 22);
            label2.TabIndex = 0;
            label2.Text = "تصفية بـ:";
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = Color.White;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 10F);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "العميل/المورد", "رقم الفاتورة", "البيان", "المبلغ" });
            cbFilterBy.Location = new Point(1082, 4);
            cbFilterBy.Margin = new Padding(4, 4, 6, 4);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(210, 32);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = Color.White;
            txtFilterValue.Font = new Font("Tahoma", 10F);
            txtFilterValue.Location = new Point(832, 6);
            txtFilterValue.Margin = new Padding(4, 6, 4, 4);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(240, 32);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyDown += txtFilterValue_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(33, 37, 41);
            label1.Location = new Point(751, 8);
            label1.Margin = new Padding(10, 8, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(67, 22);
            label1.TabIndex = 3;
            label1.Text = "الحالة:";
            // 
            // cbStatus
            // 
            cbStatus.BackColor = Color.White;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 10F);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(599, 4);
            cbStatus.Margin = new Padding(4, 4, 6, 4);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(144, 32);
            cbStatus.TabIndex = 4;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(33, 37, 41);
            label3.Location = new Point(540, 8);
            label3.Margin = new Padding(10, 8, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(43, 22);
            label3.TabIndex = 5;
            label3.Text = "من:";
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = Color.White;
            dtpFrom.Font = new Font("Tahoma", 10F);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(390, 5);
            dtpFrom.Margin = new Padding(4, 5, 6, 4);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(142, 32);
            dtpFrom.TabIndex = 6;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(33, 37, 41);
            label4.Location = new Point(325, 8);
            label4.Margin = new Padding(10, 8, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(49, 22);
            label4.TabIndex = 7;
            label4.Text = "إلى:";
            // 
            // dtpTo
            // 
            dtpTo.BackColor = Color.White;
            dtpTo.Font = new Font("Tahoma", 10F);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(175, 5);
            dtpTo.Margin = new Padding(4, 5, 6, 4);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(142, 32);
            dtpTo.TabIndex = 8;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // btnToggleFilters
            // 
            btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
            btnToggleFilters.Cursor = Cursors.Hand;
            btnToggleFilters.Dock = DockStyle.Left;
            btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnToggleFilters.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 104);
            btnToggleFilters.TabIndex = 9;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.White;
            pnlSummary.Controls.Add(flpSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 741);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1564, 32);
            pnlSummary.TabIndex = 4;
            // 
            // flpSummary
            // 
            flpSummary.Controls.Add(lblSummary);
            flpSummary.Controls.Add(lblCountCaption);
            flpSummary.Controls.Add(lblRecordsCount);
            flpSummary.Dock = DockStyle.Fill;
            flpSummary.Location = new Point(0, 0);
            flpSummary.Margin = new Padding(0);
            flpSummary.Name = "flpSummary";
            flpSummary.Padding = new Padding(10, 5, 10, 4);
            flpSummary.Size = new Size(1564, 32);
            flpSummary.TabIndex = 0;
            flpSummary.WrapContents = false;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(33, 37, 41);
            lblSummary.Location = new Point(1380, 5);
            lblSummary.Margin = new Padding(4, 0, 12, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(160, 22);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0";
            // 
            // lblCountCaption
            // 
            lblCountCaption.AutoSize = true;
            lblCountCaption.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblCountCaption.ForeColor = Color.FromArgb(33, 37, 41);
            lblCountCaption.Location = new Point(1223, 5);
            lblCountCaption.Margin = new Padding(4, 0, 4, 0);
            lblCountCaption.Name = "lblCountCaption";
            lblCountCaption.Size = new Size(141, 22);
            lblCountCaption.TabIndex = 1;
            lblCountCaption.Text = "عدد المرتجعات:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordsCount.ForeColor = Color.FromArgb(33, 37, 41);
            lblRecordsCount.Location = new Point(1192, 5);
            lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(23, 24);
            lblRecordsCount.TabIndex = 2;
            lblRecordsCount.Text = "0";
            // 
            // tabReturns
            // 
            tabReturns.Controls.Add(tpSales);
            tabReturns.Controls.Add(tpPurchases);
            tabReturns.Dock = DockStyle.Fill;
            tabReturns.Font = new Font("Tahoma", 10F);
            tabReturns.Location = new Point(0, 256);
            tabReturns.Name = "tabReturns";
            tabReturns.RightToLeft = RightToLeft.Yes;
            tabReturns.RightToLeftLayout = true;
            tabReturns.SelectedIndex = 0;
            tabReturns.Size = new Size(1564, 485);
            tabReturns.TabIndex = 3;
            // 
            // tpSales
            // 
            tpSales.BackColor = Color.White;
            tpSales.Controls.Add(lblNoSales);
            tpSales.Controls.Add(dgvSales);
            tpSales.Location = new Point(4, 33);
            tpSales.Name = "tpSales";
            tpSales.Padding = new Padding(3);
            tpSales.Size = new Size(1556, 448);
            tpSales.TabIndex = 0;
            tpSales.Text = "مرتجعـات المبيعات";
            // 
            // lblNoSales
            // 
            lblNoSales.Dock = DockStyle.Fill;
            lblNoSales.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblNoSales.ForeColor = Color.FromArgb(33, 37, 41);
            lblNoSales.Location = new Point(3, 3);
            lblNoSales.Name = "lblNoSales";
            lblNoSales.Size = new Size(1550, 442);
            lblNoSales.TabIndex = 1;
            lblNoSales.Text = "لا توجد بيانات";
            lblNoSales.TextAlign = ContentAlignment.MiddleCenter;
            lblNoSales.Visible = false;
            // 
            // dgvSales
            // 
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
            dgvSales.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.BackgroundColor = Color.White;
            dgvSales.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvSales.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvSales.ColumnHeadersHeight = 45;
            dgvSales.ContextMenuStrip = ctxMenu;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 243, 205);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvSales.DefaultCellStyle = dataGridViewCellStyle3;
            dgvSales.Dock = DockStyle.Fill;
            dgvSales.EnableHeadersVisualStyles = false;
            dgvSales.GridColor = Color.FromArgb(222, 226, 230);
            dgvSales.Location = new Point(3, 3);
            dgvSales.MultiSelect = false;
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.RowHeadersWidth = 62;
            dgvSales.RowTemplate.Height = 32;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new Size(1550, 442);
            dgvSales.TabIndex = 0;
            dgvSales.TabStop = false;
            dgvSales.DoubleClick += dgvSales_DoubleClick;
            // 
            // ctxMenu
            // 
            ctxMenu.ImageScalingSize = new Size(24, 24);
            ctxMenu.Items.AddRange(new ToolStripItem[] { tsmiOpen, tsSep1, tsmiRefresh });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.Size = new Size(190, 74);
            // 
            // tsmiOpen
            // 
            tsmiOpen.Name = "tsmiOpen";
            tsmiOpen.Size = new Size(189, 32);
            tsmiOpen.Text = "فتح / تعديل";
            tsmiOpen.Click += tsmiOpen_Click;
            // 
            // tsSep1
            // 
            tsSep1.Name = "tsSep1";
            tsSep1.Size = new Size(186, 6);
            // 
            // tsmiRefresh
            // 
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.Size = new Size(189, 32);
            tsmiRefresh.Text = "تحديث القائمة";
            tsmiRefresh.Click += tsmiRefresh_Click;
            // 
            // tpPurchases
            // 
            tpPurchases.BackColor = Color.White;
            tpPurchases.Controls.Add(lblNoPurchases);
            tpPurchases.Controls.Add(dgvPurchases);
            tpPurchases.Location = new Point(4, 33);
            tpPurchases.Name = "tpPurchases";
            tpPurchases.Padding = new Padding(3);
            tpPurchases.Size = new Size(1556, 448);
            tpPurchases.TabIndex = 1;
            tpPurchases.Text = "مرتجعـات المشتريات";
            // 
            // lblNoPurchases
            // 
            lblNoPurchases.Dock = DockStyle.Fill;
            lblNoPurchases.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblNoPurchases.ForeColor = Color.FromArgb(33, 37, 41);
            lblNoPurchases.Location = new Point(3, 3);
            lblNoPurchases.Name = "lblNoPurchases";
            lblNoPurchases.Size = new Size(1550, 442);
            lblNoPurchases.TabIndex = 1;
            lblNoPurchases.Text = "لا توجد بيانات";
            lblNoPurchases.TextAlign = ContentAlignment.MiddleCenter;
            lblNoPurchases.Visible = false;
            // 
            // dgvPurchases
            // 
            dgvPurchases.AllowUserToAddRows = false;
            dgvPurchases.AllowUserToDeleteRows = false;
            dgvPurchases.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(248, 249, 250);
            dgvPurchases.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPurchases.BackgroundColor = Color.White;
            dgvPurchases.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle5.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvPurchases.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvPurchases.ColumnHeadersHeight = 45;
            dgvPurchases.ContextMenuStrip = ctxMenu;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(255, 243, 205);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvPurchases.DefaultCellStyle = dataGridViewCellStyle6;
            dgvPurchases.Dock = DockStyle.Fill;
            dgvPurchases.EnableHeadersVisualStyles = false;
            dgvPurchases.GridColor = Color.FromArgb(222, 226, 230);
            dgvPurchases.Location = new Point(3, 3);
            dgvPurchases.MultiSelect = false;
            dgvPurchases.Name = "dgvPurchases";
            dgvPurchases.ReadOnly = true;
            dgvPurchases.RowHeadersVisible = false;
            dgvPurchases.RowHeadersWidth = 62;
            dgvPurchases.RowTemplate.Height = 32;
            dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPurchases.Size = new Size(1550, 442);
            dgvPurchases.TabIndex = 0;
            dgvPurchases.TabStop = false;
            dgvPurchases.DoubleClick += dgvPurchases_DoubleClick;
            // 
            // frmListReturns
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(1564, 773);
            Controls.Add(tabReturns);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "frmListReturns";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة المرتجعات";
            Load += frmListReturns_Load;
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            flpFilterRow2.ResumeLayout(false);
            flpFilterRow2.PerformLayout();
            flpFilterRow1.ResumeLayout(false);
            flpFilterRow1.PerformLayout();
            pnlSummary.ResumeLayout(false);
            flpSummary.ResumeLayout(false);
            flpSummary.PerformLayout();
            tabReturns.ResumeLayout(false);
            tpSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ctxMenu.ResumeLayout(false);
            tpPurchases.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.FlowLayoutPanel flpFilterRow1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.FlowLayoutPanel flpFilterRow2;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.CheckBox chkOnlyCancelled;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblCountCaption;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.TabControl tabReturns;
        private System.Windows.Forms.TabPage tpSales;
        private System.Windows.Forms.Label lblNoSales;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.TabPage tpPurchases;
        private System.Windows.Forms.Label lblNoPurchases;
        private System.Windows.Forms.DataGridView dgvPurchases;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefresh;
    }
}
