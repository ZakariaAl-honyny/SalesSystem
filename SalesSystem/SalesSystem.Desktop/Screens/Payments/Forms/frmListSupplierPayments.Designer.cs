namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmListSupplierPayments
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
            pnlToolbar = new Panel();
            flpToolbar = new FlowLayoutPanel();
            btnAddNew = new Button();
            btnRefresh = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnRangeToday = new Button();
            btnRangeMonth = new Button();
            btnRange30 = new Button();
            lblQuickSearch = new Label();
            txtSearch = new TextBox();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            pnlGrid = new Panel();
            lblEmptyState = new Label();
            dgvSupplierPayments = new DataGridView();
            pnlSummary = new Panel();
            flpSummary = new FlowLayoutPanel();
            lblSummary = new Label();
            lblRecordsCountTitle = new Label();
            lblRecordsCount = new Label();
            pnlFooter = new Panel();
            btnClose = new Button();
            cmsSupplierPayments = new ContextMenuStrip(components);
            tsmiRefresh = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmiEdit = new ToolStripMenuItem();
            tsmiAdd = new ToolStripMenuItem();
            toolTip1 = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSupplierPayments).BeginInit();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            cmsSupplierPayments.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(8, 0, 8, 0);
            pnlHeader.Size = new Size(1200, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = System.Windows.Forms.DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(8, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1184, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "المدفوعات للموردين";
            lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(8, 6, 8, 6);
            pnlToolbar.Size = new Size(1200, 56);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
            flpToolbar.Controls.Add(btnAddNew);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            flpToolbar.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flpToolbar.Location = new Point(8, 6);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Size = new Size(1184, 44);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnAddNew
            // 
            btnAddNew.BackColor = Color.FromArgb(40, 167, 69);
            btnAddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAddNew.ForeColor = Color.White;
            btnAddNew.Location = new Point(1064, 0);
            btnAddNew.Margin = new Padding(4, 0, 4, 0);
            btnAddNew.Name = "btnAddNew";
            btnAddNew.Size = new Size(116, 38);
            btnAddNew.TabIndex = 0;
            btnAddNew.Text = "+ صرف جديد";
            btnAddNew.UseVisualStyleBackColor = false;
            btnAddNew.Click += btnAddNew_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
            btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(940, 0);
            btnRefresh.Margin = new Padding(4, 0, 4, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(116, 38);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // pnlFilters
            // 
            pnlFilters.BackColor = Color.FromArgb(235, 240, 235);
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            pnlFilters.Location = new Point(0, 112);
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
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(lblQuickSearch);
            gbFilter.Controls.Add(txtSearch);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(6, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            gbFilter.Size = new Size(1188, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(14, 82, 52);
            label2.Location = new Point(1058, 24);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(100, 26);
            label2.TabIndex = 0;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cbFilterBy.BackColor = Color.White;
            cbFilterBy.Font = new Font("Tahoma", 9F);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "المورد", "رقم السند", "البيان", "المبلغ" });
            cbFilterBy.Location = new Point(872, 22);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(180, 26);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = Color.White;
            txtFilterValue.Font = new Font("Tahoma", 9F);
            txtFilterValue.Location = new Point(660, 21);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            txtFilterValue.Size = new Size(204, 26);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyDown += txtFilterValue_KeyDown;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(14, 82, 52);
            label3.Location = new Point(600, 24);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(56, 26);
            label3.TabIndex = 3;
            label3.Text = "من:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Tahoma", 9F);
            dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(444, 22);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(152, 25);
            dtpFrom.TabIndex = 4;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(14, 82, 52);
            label4.Location = new Point(384, 24);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(56, 26);
            label4.TabIndex = 5;
            label4.Text = "إلى:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Tahoma", 9F);
            dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpTo.Location = new Point(228, 22);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(152, 25);
            dtpTo.TabIndex = 6;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeToday.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeToday.FlatAppearance.BorderSize = 1;
            btnRangeToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeToday.Location = new Point(1058, 64);
            btnRangeToday.Margin = new Padding(0);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 7;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeMonth.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeMonth.FlatAppearance.BorderSize = 1;
            btnRangeMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeMonth.Location = new Point(924, 64);
            btnRangeMonth.Margin = new Padding(0);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(128, 30);
            btnRangeMonth.TabIndex = 8;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.FromArgb(248, 249, 250);
            btnRange30.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRange30.FlatAppearance.BorderSize = 1;
            btnRange30.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.ForeColor = Color.FromArgb(33, 37, 41);
            btnRange30.Location = new Point(784, 64);
            btnRange30.Margin = new Padding(0);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(134, 30);
            btnRange30.TabIndex = 9;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // lblQuickSearch
            // 
            lblQuickSearch.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblQuickSearch.ForeColor = Color.FromArgb(14, 82, 52);
            lblQuickSearch.Location = new Point(684, 66);
            lblQuickSearch.Margin = new Padding(0);
            lblQuickSearch.Name = "lblQuickSearch";
            lblQuickSearch.Size = new Size(96, 26);
            lblQuickSearch.TabIndex = 10;
            lblQuickSearch.Text = "بحث سريع:";
            lblQuickSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.White;
            txtSearch.Font = new Font("Tahoma", 9F);
            txtSearch.Location = new Point(474, 64);
            txtSearch.Margin = new Padding(0);
            txtSearch.Name = "txtSearch";
            txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            txtSearch.Size = new Size(204, 26);
            txtSearch.TabIndex = 11;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnClearFilters
            // 
            // RULE-N148: «مسح التصفية» — داخل gbFilter، قيم حرفية مطابقة لما يُطبَّق وقت التشغيل (RULE-N134).
            // الصف الثاني، يسار كتلة البحث السريع بفاصل 8 بكسل — لا يحرّك أي عنصر قائم (RULE-N125).
            btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(321, 64);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(145, 32);
            btnClearFilters.TabIndex = 13;
            btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
            toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
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
            btnToggleFilters.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 104);
            btnToggleFilters.TabIndex = 12;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = Color.FromArgb(235, 240, 235);
            pnlGrid.Controls.Add(lblEmptyState);
            pnlGrid.Controls.Add(dgvSupplierPayments);
            pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlGrid.Location = new Point(0, 224);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(6, 4, 6, 4);
            pnlGrid.Size = new Size(1200, 415);
            pnlGrid.TabIndex = 3;
            // 
            // lblEmptyState
            // 
            lblEmptyState.BackColor = Color.Transparent;
            lblEmptyState.Dock = System.Windows.Forms.DockStyle.Fill;
            lblEmptyState.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblEmptyState.ForeColor = Color.FromArgb(14, 82, 52);
            lblEmptyState.Location = new Point(6, 4);
            lblEmptyState.Margin = new Padding(0);
            lblEmptyState.Name = "lblEmptyState";
            lblEmptyState.Size = new Size(1188, 407);
            lblEmptyState.TabIndex = 1;
            lblEmptyState.Text = "لا توجد بيانات لعرضها";
            lblEmptyState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblEmptyState.Visible = false;
            // 
            // dgvSupplierPayments
            // 
            dgvSupplierPayments.AllowUserToAddRows = false;
            dgvSupplierPayments.AllowUserToDeleteRows = false;
            dgvSupplierPayments.AllowUserToOrderColumns = true;
            dgvSupplierPayments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvSupplierPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSupplierPayments.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvSupplierPayments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvSupplierPayments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvSupplierPayments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSupplierPayments.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvSupplierPayments.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvSupplierPayments.ColumnHeadersHeight = 45;
            dgvSupplierPayments.ContextMenuStrip = cmsSupplierPayments;
            dgvSupplierPayments.DefaultCellStyle.BackColor = Color.White;
            dgvSupplierPayments.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvSupplierPayments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvSupplierPayments.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvSupplierPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvSupplierPayments.EnableHeadersVisualStyles = false;
            dgvSupplierPayments.GridColor = Color.FromArgb(205, 225, 205);
            dgvSupplierPayments.Location = new Point(6, 4);
            dgvSupplierPayments.MultiSelect = false;
            dgvSupplierPayments.Name = "dgvSupplierPayments";
            dgvSupplierPayments.ReadOnly = true;
            dgvSupplierPayments.RowHeadersVisible = false;
            dgvSupplierPayments.RowHeadersWidth = 62;
            dgvSupplierPayments.RowTemplate.Height = 32;
            dgvSupplierPayments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvSupplierPayments.Size = new Size(1188, 407);
            dgvSupplierPayments.TabIndex = 0;
            dgvSupplierPayments.TabStop = false;
            dgvSupplierPayments.CellDoubleClick += dgvSupplierPayments_CellDoubleClick;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
            pnlSummary.Controls.Add(flpSummary);
            pnlSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 639);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1200, 32);
            pnlSummary.TabIndex = 5;
            // 
            // flpSummary
            // 
            flpSummary.BackColor = Color.FromArgb(235, 240, 235);
            flpSummary.Controls.Add(lblSummary);
            flpSummary.Controls.Add(lblRecordsCountTitle);
            flpSummary.Controls.Add(lblRecordsCount);
            flpSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            flpSummary.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flpSummary.Location = new Point(0, 0);
            flpSummary.Margin = new Padding(0);
            flpSummary.Name = "flpSummary";
            flpSummary.Padding = new Padding(10, 4, 10, 4);
            flpSummary.Size = new Size(1200, 32);
            flpSummary.TabIndex = 0;
            flpSummary.WrapContents = false;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
            lblSummary.Location = new Point(982, 4);
            lblSummary.Margin = new Padding(4, 0, 12, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(206, 22);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00";
            // 
            // lblRecordsCountTitle
            // 
            lblRecordsCountTitle.AutoSize = true;
            lblRecordsCountTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblRecordsCountTitle.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCountTitle.Location = new Point(903, 4);
            lblRecordsCountTitle.Margin = new Padding(4, 0, 4, 0);
            lblRecordsCountTitle.Name = "lblRecordsCountTitle";
            lblRecordsCountTitle.Size = new Size(51, 22);
            lblRecordsCountTitle.TabIndex = 1;
            lblRecordsCountTitle.Text = "العدد:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCount.Location = new Point(855, 4);
            lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(30, 22);
            lblRecordsCount.TabIndex = 2;
            lblRecordsCount.Text = "0";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(btnClose);
            pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 671);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Size = new Size(1200, 32);
            pnlFooter.TabIndex = 4;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(12, 0);
            btnClose.Margin = new Padding(0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(180, 32);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // cmsSupplierPayments
            // 
            cmsSupplierPayments.ImageScalingSize = new Size(20, 20);
            cmsSupplierPayments.Items.AddRange(new ToolStripItem[] { tsmiRefresh, toolStripSeparator1, tsmiEdit, tsmiAdd });
            cmsSupplierPayments.Name = "cmsSupplierPayments";
            cmsSupplierPayments.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            cmsSupplierPayments.Size = new Size(161, 88);
            cmsSupplierPayments.Opening += cmsSupplierPayments_Opening;
            // 
            // tsmiRefresh
            // 
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.Size = new Size(160, 26);
            tsmiRefresh.Text = "تحديث";
            tsmiRefresh.Click += tsmiRefresh_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(157, 6);
            // 
            // tsmiEdit
            // 
            tsmiEdit.Name = "tsmiEdit";
            tsmiEdit.Size = new Size(160, 26);
            tsmiEdit.Text = "عرض السند";
            tsmiEdit.Click += tsmiEdit_Click;
            // 
            // tsmiAdd
            // 
            tsmiAdd.Name = "tsmiAdd";
            tsmiAdd.Size = new Size(160, 26);
            tsmiAdd.Text = "صرف جديد";
            tsmiAdd.Click += tsmiAdd_Click;
            // 
            // frmListSupplierPayments
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            ClientSize = new Size(1200, 703);
            Controls.Add(pnlGrid);
            // RULE-N135: الملخص فوق شريط الأسفل (pnlFooter يأخذ الحافة السفلية لأنه يُضاف أخيراً).
            Controls.Add(pnlSummary);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "frmListSupplierPayments";
            RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "المدفوعات للموردين";
            Load += frmListSupplierPayments_Load;
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSupplierPayments).EndInit();
            pnlSummary.ResumeLayout(false);
            flpSummary.ResumeLayout(false);
            flpSummary.PerformLayout();
            pnlFooter.ResumeLayout(false);
            cmsSupplierPayments.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblMode;
        private Panel pnlToolbar;
        private FlowLayoutPanel flpToolbar;
        private Button btnAddNew;
        private Button btnRefresh;
        private Panel pnlFilters;
        private GroupBox gbFilter;
        private Label label2;
        private ComboBox cbFilterBy;
        private TextBox txtFilterValue;
        private Label label3;
        private DateTimePicker dtpFrom;
        private Label label4;
        private DateTimePicker dtpTo;
        private Button btnRangeToday;
        private Button btnRangeMonth;
        private Button btnRange30;
        private Label lblQuickSearch;
        private TextBox txtSearch;
        private Button btnClearFilters;
        private Button btnToggleFilters;
        private Panel pnlGrid;
        private Label lblEmptyState;
        private DataGridView dgvSupplierPayments;
        private Panel pnlSummary;
        private FlowLayoutPanel flpSummary;
        private Label lblSummary;
        private Label lblRecordsCountTitle;
        private Label lblRecordsCount;
        private Panel pnlFooter;
        private Button btnClose;
        private ContextMenuStrip cmsSupplierPayments;
        private ToolStripMenuItem tsmiRefresh;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiEdit;
        private ToolStripMenuItem tsmiAdd;
        private ToolTip toolTip1;
    }
}
