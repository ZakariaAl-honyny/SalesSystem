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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
            cmsSupplierPayments = new ContextMenuStrip(components);
            tsmiRefresh = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmiEdit = new ToolStripMenuItem();
            tsmiAdd = new ToolStripMenuItem();
            pnlSummary = new Panel();
            flpSummary = new FlowLayoutPanel();
            lblSummary = new Label();
            lblRecordsCountTitle = new Label();
            lblRecordsCount = new Label();
            btnClose = new Button();
            toolTip1 = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSupplierPayments).BeginInit();
            cmsSupplierPayments.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(8, 0, 8, 0);
            pnlHeader.Size = new Size(1200, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Location = new Point(8, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1184, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "المدفوعات للموردين";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Padding = new Padding(8, 6, 8, 6);
            pnlToolbar.Size = new Size(1200, 56);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpToolbar.Controls.Add(btnAddNew);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(8, 6);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Size = new Size(1184, 44);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnAddNew
            // 
            btnAddNew.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(13), Convert.ToInt32(110), Convert.ToInt32(253));
            btnAddNew.FlatStyle = FlatStyle.Flat;
            btnAddNew.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAddNew.ForeColor = System.Drawing.Color.White;
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
            btnRefresh.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(23), Convert.ToInt32(162), Convert.ToInt32(184));
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = System.Drawing.Color.White;
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
            pnlFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Controls.Add(btnToggleFilters);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 112);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1200, 112);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(245), Convert.ToInt32(235));
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
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1048, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            label2.Location = new Point(857, 24);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(100, 26);
            label2.TabIndex = 0;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = System.Drawing.Color.White;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "المورد", "رقم السند", "البيان", "المبلغ" });
            cbFilterBy.Location = new Point(671, 22);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(180, 30);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = System.Drawing.Color.White;
            txtFilterValue.Font = new Font("Tahoma", 9F);
            txtFilterValue.Location = new Point(459, 21);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.RightToLeft = RightToLeft.Yes;
            txtFilterValue.Size = new Size(204, 29);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyDown += txtFilterValue_KeyDown;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            label3.Location = new Point(399, 24);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(56, 26);
            label3.TabIndex = 3;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = System.Drawing.Color.White;
            dtpFrom.Font = new Font("Tahoma", 9F);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(243, 22);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(152, 29);
            dtpFrom.TabIndex = 4;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            label4.Location = new Point(183, 24);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(56, 26);
            label4.TabIndex = 5;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.BackColor = System.Drawing.Color.White;
            dtpTo.Font = new Font("Tahoma", 9F);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(27, 22);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(152, 29);
            dtpTo.TabIndex = 6;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            btnRangeToday.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnRangeToday.FlatStyle = FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
            btnRangeToday.Location = new Point(857, 64);
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
            btnRangeMonth.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            btnRangeMonth.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnRangeMonth.FlatStyle = FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
            btnRangeMonth.Location = new Point(723, 64);
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
            btnRange30.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            btnRange30.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnRange30.FlatStyle = FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
            btnRange30.Location = new Point(583, 64);
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
            lblQuickSearch.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblQuickSearch.Location = new Point(483, 66);
            lblQuickSearch.Margin = new Padding(0);
            lblQuickSearch.Name = "lblQuickSearch";
            lblQuickSearch.Size = new Size(96, 26);
            lblQuickSearch.TabIndex = 10;
            lblQuickSearch.Text = "بحث سريع:";
            lblQuickSearch.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = System.Drawing.Color.White;
            txtSearch.Font = new Font("Tahoma", 9F);
            txtSearch.Location = new Point(273, 64);
            txtSearch.Margin = new Padding(0);
            txtSearch.Name = "txtSearch";
            txtSearch.RightToLeft = RightToLeft.Yes;
            txtSearch.Size = new Size(204, 29);
            txtSearch.TabIndex = 11;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnClearFilters
            // 
            btnClearFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClearFilters.ForeColor = System.Drawing.Color.White;
            btnClearFilters.Location = new Point(65, 64);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(200, 32);
            btnClearFilters.TabIndex = 13;
            btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
            toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            // 
            // btnToggleFilters
            // 
            btnToggleFilters.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(248), Convert.ToInt32(249), Convert.ToInt32(250));
            btnToggleFilters.Cursor = Cursors.Hand;
            btnToggleFilters.Dock = DockStyle.Left;
            btnToggleFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            btnToggleFilters.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(240), Convert.ToInt32(240));
            btnToggleFilters.FlatStyle = FlatStyle.Flat;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(30), Convert.ToInt32(41), Convert.ToInt32(59));
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 104);
            btnToggleFilters.TabIndex = 12;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlGrid.Controls.Add(lblEmptyState);
            pnlGrid.Controls.Add(dgvSupplierPayments);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 224);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(6, 4, 6, 4);
            pnlGrid.Size = new Size(1200, 447);
            pnlGrid.TabIndex = 3;
            // 
            // lblEmptyState
            // 
            lblEmptyState.BackColor = System.Drawing.Color.Transparent;
            lblEmptyState.Dock = DockStyle.Fill;
            lblEmptyState.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblEmptyState.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblEmptyState.Location = new Point(6, 4);
            lblEmptyState.Margin = new Padding(0);
            lblEmptyState.Name = "lblEmptyState";
            lblEmptyState.Size = new Size(1188, 439);
            lblEmptyState.TabIndex = 1;
            lblEmptyState.Text = "لا توجد بيانات لعرضها";
            lblEmptyState.TextAlign = ContentAlignment.MiddleCenter;
            lblEmptyState.Visible = false;
            // 
            // dgvSupplierPayments
            // 
            dgvSupplierPayments.AllowUserToAddRows = false;
            dgvSupplierPayments.AllowUserToDeleteRows = false;
            dgvSupplierPayments.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(245), Convert.ToInt32(252), Convert.ToInt32(245));
            dgvSupplierPayments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSupplierPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSupplierPayments.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(220), Convert.ToInt32(240), Convert.ToInt32(220));
            dgvSupplierPayments.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(165), Convert.ToInt32(103));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvSupplierPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvSupplierPayments.ColumnHeadersHeight = 45;
            dgvSupplierPayments.ContextMenuStrip = cmsSupplierPayments;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(255), Convert.ToInt32(255), Convert.ToInt32(180));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvSupplierPayments.DefaultCellStyle = dataGridViewCellStyle3;
            dgvSupplierPayments.Dock = DockStyle.Fill;
            dgvSupplierPayments.EnableHeadersVisualStyles = false;
            dgvSupplierPayments.GridColor = System.Drawing.Color.FromArgb(Convert.ToInt32(205), Convert.ToInt32(225), Convert.ToInt32(205));
            dgvSupplierPayments.Location = new Point(6, 4);
            dgvSupplierPayments.MultiSelect = false;
            dgvSupplierPayments.Name = "dgvSupplierPayments";
            dgvSupplierPayments.ReadOnly = true;
            dgvSupplierPayments.RowHeadersVisible = false;
            dgvSupplierPayments.RowHeadersWidth = 62;
            dgvSupplierPayments.RowTemplate.Height = 32;
            dgvSupplierPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSupplierPayments.Size = new Size(1188, 439);
            dgvSupplierPayments.TabIndex = 0;
            dgvSupplierPayments.TabStop = false;
            dgvSupplierPayments.CellDoubleClick += dgvSupplierPayments_CellDoubleClick;
            // 
            // cmsSupplierPayments
            // 
            cmsSupplierPayments.ImageScalingSize = new Size(20, 20);
            cmsSupplierPayments.Items.AddRange(new ToolStripItem[] { tsmiRefresh, toolStripSeparator1, tsmiEdit, tsmiAdd });
            cmsSupplierPayments.Name = "cmsSupplierPayments";
            cmsSupplierPayments.RightToLeft = RightToLeft.Yes;
            cmsSupplierPayments.Size = new Size(172, 106);
            cmsSupplierPayments.Opening += cmsSupplierPayments_Opening;
            // 
            // tsmiRefresh
            // 
            tsmiRefresh.Name = "tsmiRefresh";
            tsmiRefresh.Size = new Size(171, 32);
            tsmiRefresh.Text = "تحديث";
            tsmiRefresh.Click += tsmiRefresh_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(168, 6);
            // 
            // tsmiEdit
            // 
            tsmiEdit.Name = "tsmiEdit";
            tsmiEdit.Size = new Size(171, 32);
            tsmiEdit.Text = "عرض السند";
            tsmiEdit.Click += tsmiEdit_Click;
            // 
            // tsmiAdd
            // 
            tsmiAdd.Name = "tsmiAdd";
            tsmiAdd.Size = new Size(171, 32);
            tsmiAdd.Text = "صرف جديد";
            tsmiAdd.Click += tsmiAdd_Click;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            pnlSummary.Controls.Add(flpSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 671);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1200, 32);
            pnlSummary.TabIndex = 5;
            // 
            // flpSummary
            // 
            flpSummary.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(235), Convert.ToInt32(240), Convert.ToInt32(235));
            flpSummary.Controls.Add(lblSummary);
            flpSummary.Controls.Add(lblRecordsCountTitle);
            flpSummary.Controls.Add(lblRecordsCount);
            flpSummary.Dock = DockStyle.Fill;
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
            lblSummary.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(14), Convert.ToInt32(82), Convert.ToInt32(52));
            lblSummary.Location = new Point(862, 4);
            lblSummary.Margin = new Padding(4, 0, 12, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(314, 22);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00";
            // 
            // lblRecordsCountTitle
            // 
            lblRecordsCountTitle.AutoSize = true;
            lblRecordsCountTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblRecordsCountTitle.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblRecordsCountTitle.Location = new Point(786, 4);
            lblRecordsCountTitle.Margin = new Padding(4, 0, 4, 0);
            lblRecordsCountTitle.Name = "lblRecordsCountTitle";
            lblRecordsCountTitle.Size = new Size(60, 22);
            lblRecordsCountTitle.TabIndex = 1;
            lblRecordsCountTitle.Text = "العدد:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(33), Convert.ToInt32(37), Convert.ToInt32(41));
            lblRecordsCount.Location = new Point(757, 4);
            lblRecordsCount.Margin = new Padding(4, 0, 4, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(21, 22);
            lblRecordsCount.TabIndex = 2;
            lblRecordsCount.Text = "0";
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Location = new Point(820, 0);
            btnClose.Margin = new Padding(0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(116, 38);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // frmListSupplierPayments
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(170), Convert.ToInt32(220), Convert.ToInt32(170));
            ClientSize = new Size(1200, 703);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "frmListSupplierPayments";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
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
            cmsSupplierPayments.ResumeLayout(false);
            pnlSummary.ResumeLayout(false);
            flpSummary.ResumeLayout(false);
            flpSummary.PerformLayout();
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
        private Button btnClose;
        private ContextMenuStrip cmsSupplierPayments;
        private ToolStripMenuItem tsmiRefresh;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiEdit;
        private ToolStripMenuItem tsmiAdd;
        private ToolTip toolTip1;
    }
}
