namespace SalesSystem.Desktop.Screens.Payments.Forms
{
    partial class frmListPayments
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
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            lblMode = new Label();
            pnlHeader = new Panel();
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
            chkOnlyCancelled = new CheckBox();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            lblCustomerEmpty = new Label();
            lblSupplierEmpty = new Label();
            pnlSummary = new Panel();
            lblSummary = new Label();
            toolTip1 = new ToolTip(components);
            tabPayments = new TabControl();
            tpCustomerPayments = new TabPage();
            dgvCustomerPayments = new DataGridView();
            cmsCustomerPayments = new ContextMenuStrip(components);
            tsmiCustomerRefresh = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmiCustomerEdit = new ToolStripMenuItem();
            tsmiCustomerAdd = new ToolStripMenuItem();
            pnlCustomerTop = new Panel();
            btnAddCustomerPayment = new Button();
            lblCustomerCount = new Label();
            lblCustomerCountTitle = new Label();
            tpSupplierPayments = new TabPage();
            dgvSupplierPayments = new DataGridView();
            cmsSupplierPayments = new ContextMenuStrip(components);
            tsmiSupplierRefresh = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            tsmiSupplierEdit = new ToolStripMenuItem();
            tsmiSupplierAdd = new ToolStripMenuItem();
            pnlSupplierTop = new Panel();
            btnAddSupplierPayment = new Button();
            lblSupplierCount = new Label();
            lblSupplierCountTitle = new Label();
            pnlHeader.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            tabPayments.SuspendLayout();
            tpCustomerPayments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCustomerPayments).BeginInit();
            cmsCustomerPayments.SuspendLayout();
            pnlCustomerTop.SuspendLayout();
            tpSupplierPayments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSupplierPayments).BeginInit();
            cmsSupplierPayments.SuspendLayout();
            pnlSupplierTop.SuspendLayout();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(16, 150, 96);
            lblMode.Dock = DockStyle.Fill;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1070, 56);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة المقبوضات والمدفوعات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(16, 150, 96);
            pnlHeader.Controls.Add(btnRefresh);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1200, 56);
            pnlHeader.TabIndex = 30;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(23, 162, 184);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(924, 11);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 35);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "🔄 تحديث";
            toolTip1.SetToolTip(btnRefresh, "تحديث كل البيانات وإرجاع الشاشة إلى الافتراضي — F5");
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1070, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(130, 56);
            btnClose.TabIndex = 0;
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
            pnlFilters.Location = new Point(0, 56);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1200, 112);
            pnlFilters.TabIndex = 20;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = Color.White;
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
            gbFilter.Controls.Add(chkOnlyCancelled);
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
            gbFilter.Enter += gbFilter_Enter;
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(33, 37, 41);
            label2.Location = new Point(942, 24);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(96, 26);
            label2.TabIndex = 0;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = Color.White;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Location = new Point(746, 22);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(176, 30);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = Color.White;
            txtFilterValue.Font = new Font("Tahoma", 9F);
            txtFilterValue.Location = new Point(490, 23);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(236, 29);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyDown += txtFilterValue_KeyDown;
            // 
            // label1
            // 
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(33, 37, 41);
            label1.Location = new Point(942, 63);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(72, 26);
            label1.TabIndex = 3;
            label1.Text = "الحالة:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.BackColor = Color.White;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 9F);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(812, 61);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(124, 30);
            cbStatus.TabIndex = 4;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(33, 37, 41);
            label3.Location = new Point(418, 24);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(52, 26);
            label3.TabIndex = 5;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = Color.White;
            dtpFrom.Font = new Font("Tahoma", 9F);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(252, 23);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(146, 29);
            dtpFrom.TabIndex = 6;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(33, 37, 41);
            label4.Location = new Point(180, 24);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(52, 26);
            label4.TabIndex = 7;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.BackColor = Color.White;
            dtpTo.Font = new Font("Tahoma", 9F);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(14, 23);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(146, 29);
            dtpTo.TabIndex = 8;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeToday.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeToday.FlatStyle = FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeToday.Location = new Point(12, 60);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 32);
            btnRangeToday.TabIndex = 9;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.FromArgb(248, 249, 250);
            btnRange30.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRange30.FlatStyle = FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.ForeColor = Color.FromArgb(33, 37, 41);
            btnRange30.Location = new Point(118, 60);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(138, 32);
            btnRange30.TabIndex = 10;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.FromArgb(248, 249, 250);
            btnRangeMonth.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRangeMonth.FlatStyle = FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.ForeColor = Color.FromArgb(33, 37, 41);
            btnRangeMonth.Location = new Point(262, 60);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(130, 32);
            btnRangeMonth.TabIndex = 11;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // chkOnlyCancelled
            // 
            chkOnlyCancelled.Font = new Font("Tahoma", 9F);
            chkOnlyCancelled.ForeColor = Color.FromArgb(33, 37, 41);
            chkOnlyCancelled.Location = new Point(398, 63);
            chkOnlyCancelled.Margin = new Padding(0);
            chkOnlyCancelled.Name = "chkOnlyCancelled";
            chkOnlyCancelled.Size = new Size(170, 26);
            chkOnlyCancelled.TabIndex = 12;
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
            btnClearFilters.Location = new Point(574, 60);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(232, 32);
            btnClearFilters.TabIndex = 13;
            btnClearFilters.Text = "\U0001f9f9 مسح التصفية";
            toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
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
            btnToggleFilters.TabIndex = 13;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // lblCustomerEmpty
            // 
            lblCustomerEmpty.BackColor = Color.Transparent;
            lblCustomerEmpty.Dock = DockStyle.Fill;
            lblCustomerEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblCustomerEmpty.ForeColor = Color.FromArgb(33, 37, 41);
            lblCustomerEmpty.Location = new Point(3, 43);
            lblCustomerEmpty.Name = "lblCustomerEmpty";
            lblCustomerEmpty.Size = new Size(1186, 422);
            lblCustomerEmpty.TabIndex = 0;
            lblCustomerEmpty.Text = "لا توجد مقبوضات مطابقة للمعايير الحالية";
            lblCustomerEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblCustomerEmpty.Visible = false;
            // 
            // lblSupplierEmpty
            // 
            lblSupplierEmpty.BackColor = Color.Transparent;
            lblSupplierEmpty.Dock = DockStyle.Fill;
            lblSupplierEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblSupplierEmpty.ForeColor = Color.FromArgb(33, 37, 41);
            lblSupplierEmpty.Location = new Point(3, 43);
            lblSupplierEmpty.Name = "lblSupplierEmpty";
            lblSupplierEmpty.Size = new Size(1186, 422);
            lblSupplierEmpty.TabIndex = 0;
            lblSupplierEmpty.Text = "لا توجد مدفوعات مطابقة للمعايير الحالية";
            lblSupplierEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblSupplierEmpty.Visible = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.White;
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 671);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1200, 32);
            pnlSummary.TabIndex = 10;
            // 
            // lblSummary
            // 
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(33, 37, 41);
            lblSummary.Location = new Point(0, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(1200, 32);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — الإجمالي: 0.00";
            lblSummary.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tabPayments
            // 
            tabPayments.Controls.Add(tpCustomerPayments);
            tabPayments.Controls.Add(tpSupplierPayments);
            tabPayments.Dock = DockStyle.Fill;
            tabPayments.Font = new Font("Tahoma", 9F);
            tabPayments.Location = new Point(0, 168);
            tabPayments.Name = "tabPayments";
            tabPayments.RightToLeft = RightToLeft.Yes;
            tabPayments.RightToLeftLayout = true;
            tabPayments.SelectedIndex = 0;
            tabPayments.Size = new Size(1200, 503);
            tabPayments.TabIndex = 1;
            tabPayments.SelectedIndexChanged += tabPayments_SelectedIndexChanged;
            // 
            // tpCustomerPayments
            // 
            tpCustomerPayments.BackColor = Color.White;
            tpCustomerPayments.Controls.Add(lblCustomerEmpty);
            tpCustomerPayments.Controls.Add(dgvCustomerPayments);
            tpCustomerPayments.Controls.Add(pnlCustomerTop);
            tpCustomerPayments.Location = new Point(4, 31);
            tpCustomerPayments.Name = "tpCustomerPayments";
            tpCustomerPayments.Padding = new Padding(3);
            tpCustomerPayments.Size = new Size(1192, 468);
            tpCustomerPayments.TabIndex = 0;
            tpCustomerPayments.Text = "مقبوضات العملاء";
            // 
            // dgvCustomerPayments
            // 
            dgvCustomerPayments.AllowUserToAddRows = false;
            dgvCustomerPayments.AllowUserToDeleteRows = false;
            dgvCustomerPayments.AllowUserToOrderColumns = true;
            dgvCustomerPayments.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(248, 249, 250);
            dgvCustomerPayments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            dgvCustomerPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCustomerPayments.BackgroundColor = Color.White;
            dgvCustomerPayments.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle8.Font = new Font("Tahoma", 9F);
            dataGridViewCellStyle8.ForeColor = Color.White;
            dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle8.SelectionForeColor = Color.White;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dgvCustomerPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dgvCustomerPayments.ColumnHeadersHeight = 45;
            dgvCustomerPayments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomerPayments.ContextMenuStrip = cmsCustomerPayments;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.White;
            dataGridViewCellStyle9.Font = new Font("Tahoma", 9F);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(255, 243, 205);
            dataGridViewCellStyle9.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
            dgvCustomerPayments.DefaultCellStyle = dataGridViewCellStyle9;
            dgvCustomerPayments.Dock = DockStyle.Fill;
            dgvCustomerPayments.EnableHeadersVisualStyles = false;
            dgvCustomerPayments.GridColor = Color.FromArgb(222, 226, 230);
            dgvCustomerPayments.Location = new Point(3, 43);
            dgvCustomerPayments.MultiSelect = false;
            dgvCustomerPayments.Name = "dgvCustomerPayments";
            dgvCustomerPayments.ReadOnly = true;
            dgvCustomerPayments.RowHeadersVisible = false;
            dgvCustomerPayments.RowHeadersWidth = 51;
            dgvCustomerPayments.RowTemplate.Height = 32;
            dgvCustomerPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomerPayments.Size = new Size(1186, 422);
            dgvCustomerPayments.TabIndex = 1;
            dgvCustomerPayments.DoubleClick += dgvCustomerPayments_DoubleClick;
            // 
            // cmsCustomerPayments
            // 
            cmsCustomerPayments.ImageScalingSize = new Size(20, 20);
            cmsCustomerPayments.Items.AddRange(new ToolStripItem[] { tsmiCustomerRefresh, toolStripSeparator1, tsmiCustomerEdit, tsmiCustomerAdd });
            cmsCustomerPayments.Name = "cmsCustomerPayments";
            cmsCustomerPayments.RightToLeft = RightToLeft.Yes;
            cmsCustomerPayments.Size = new Size(164, 106);
            cmsCustomerPayments.Opening += cmsCustomerPayments_Opening;
            // 
            // tsmiCustomerRefresh
            // 
            tsmiCustomerRefresh.Name = "tsmiCustomerRefresh";
            tsmiCustomerRefresh.Size = new Size(163, 32);
            tsmiCustomerRefresh.Text = "تحديث";
            tsmiCustomerRefresh.Click += tsmiCustomerRefresh_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(160, 6);
            // 
            // tsmiCustomerEdit
            // 
            tsmiCustomerEdit.Name = "tsmiCustomerEdit";
            tsmiCustomerEdit.Size = new Size(163, 32);
            tsmiCustomerEdit.Text = "فتح السند";
            tsmiCustomerEdit.Click += tsmiCustomerEdit_Click;
            // 
            // tsmiCustomerAdd
            // 
            tsmiCustomerAdd.Name = "tsmiCustomerAdd";
            tsmiCustomerAdd.Size = new Size(163, 32);
            tsmiCustomerAdd.Text = "قبض جديد";
            tsmiCustomerAdd.Click += tsmiCustomerAdd_Click;
            // 
            // pnlCustomerTop
            // 
            pnlCustomerTop.BackColor = Color.White;
            pnlCustomerTop.Controls.Add(btnAddCustomerPayment);
            pnlCustomerTop.Controls.Add(lblCustomerCount);
            pnlCustomerTop.Controls.Add(lblCustomerCountTitle);
            pnlCustomerTop.Dock = DockStyle.Top;
            pnlCustomerTop.Location = new Point(3, 3);
            pnlCustomerTop.Name = "pnlCustomerTop";
            pnlCustomerTop.Size = new Size(1186, 40);
            pnlCustomerTop.TabIndex = 0;
            // 
            // btnAddCustomerPayment
            // 
            btnAddCustomerPayment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddCustomerPayment.BackColor = Color.FromArgb(40, 167, 69);
            btnAddCustomerPayment.FlatStyle = FlatStyle.Flat;
            btnAddCustomerPayment.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnAddCustomerPayment.ForeColor = Color.White;
            btnAddCustomerPayment.Location = new Point(1906, 5);
            btnAddCustomerPayment.Name = "btnAddCustomerPayment";
            btnAddCustomerPayment.Size = new Size(106, 30);
            btnAddCustomerPayment.TabIndex = 2;
            btnAddCustomerPayment.Text = "قبض جديد";
            btnAddCustomerPayment.UseVisualStyleBackColor = false;
            btnAddCustomerPayment.Click += btnAddCustomerPayment_Click;
            // 
            // lblCustomerCount
            // 
            lblCustomerCount.AutoSize = true;
            lblCustomerCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblCustomerCount.ForeColor = Color.FromArgb(33, 37, 41);
            lblCustomerCount.Location = new Point(50, 10);
            lblCustomerCount.Name = "lblCustomerCount";
            lblCustomerCount.Size = new Size(21, 22);
            lblCustomerCount.TabIndex = 1;
            lblCustomerCount.Text = "0";
            // 
            // lblCustomerCountTitle
            // 
            lblCustomerCountTitle.AutoSize = true;
            lblCustomerCountTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblCustomerCountTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblCustomerCountTitle.Location = new Point(10, 10);
            lblCustomerCountTitle.Name = "lblCustomerCountTitle";
            lblCustomerCountTitle.Size = new Size(60, 22);
            lblCustomerCountTitle.TabIndex = 0;
            lblCustomerCountTitle.Text = "العدد:";
            // 
            // tpSupplierPayments
            // 
            tpSupplierPayments.BackColor = Color.White;
            tpSupplierPayments.Controls.Add(lblSupplierEmpty);
            tpSupplierPayments.Controls.Add(dgvSupplierPayments);
            tpSupplierPayments.Controls.Add(pnlSupplierTop);
            tpSupplierPayments.Location = new Point(4, 31);
            tpSupplierPayments.Name = "tpSupplierPayments";
            tpSupplierPayments.Padding = new Padding(3);
            tpSupplierPayments.Size = new Size(1192, 468);
            tpSupplierPayments.TabIndex = 1;
            tpSupplierPayments.Text = "مدفوعات الموردين";
            // 
            // dgvSupplierPayments
            // 
            dgvSupplierPayments.AllowUserToAddRows = false;
            dgvSupplierPayments.AllowUserToDeleteRows = false;
            dgvSupplierPayments.AllowUserToOrderColumns = true;
            dgvSupplierPayments.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
            dgvSupplierPayments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvSupplierPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSupplierPayments.BackgroundColor = Color.White;
            dgvSupplierPayments.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.Font = new Font("Tahoma", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvSupplierPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvSupplierPayments.ColumnHeadersHeight = 45;
            dgvSupplierPayments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSupplierPayments.ContextMenuStrip = cmsSupplierPayments;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 243, 205);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvSupplierPayments.DefaultCellStyle = dataGridViewCellStyle3;
            dgvSupplierPayments.Dock = DockStyle.Fill;
            dgvSupplierPayments.EnableHeadersVisualStyles = false;
            dgvSupplierPayments.GridColor = Color.FromArgb(222, 226, 230);
            dgvSupplierPayments.Location = new Point(3, 43);
            dgvSupplierPayments.MultiSelect = false;
            dgvSupplierPayments.Name = "dgvSupplierPayments";
            dgvSupplierPayments.ReadOnly = true;
            dgvSupplierPayments.RowHeadersVisible = false;
            dgvSupplierPayments.RowHeadersWidth = 51;
            dgvSupplierPayments.RowTemplate.Height = 32;
            dgvSupplierPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSupplierPayments.Size = new Size(1186, 422);
            dgvSupplierPayments.TabIndex = 1;
            dgvSupplierPayments.DoubleClick += dgvSupplierPayments_DoubleClick;
            // 
            // cmsSupplierPayments
            // 
            cmsSupplierPayments.ImageScalingSize = new Size(20, 20);
            cmsSupplierPayments.Items.AddRange(new ToolStripItem[] { tsmiSupplierRefresh, toolStripSeparator2, tsmiSupplierEdit, tsmiSupplierAdd });
            cmsSupplierPayments.Name = "cmsSupplierPayments";
            cmsSupplierPayments.RightToLeft = RightToLeft.Yes;
            cmsSupplierPayments.Size = new Size(166, 106);
            cmsSupplierPayments.Opening += cmsSupplierPayments_Opening;
            // 
            // tsmiSupplierRefresh
            // 
            tsmiSupplierRefresh.Name = "tsmiSupplierRefresh";
            tsmiSupplierRefresh.Size = new Size(165, 32);
            tsmiSupplierRefresh.Text = "تحديث";
            tsmiSupplierRefresh.Click += tsmiSupplierRefresh_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(162, 6);
            // 
            // tsmiSupplierEdit
            // 
            tsmiSupplierEdit.Name = "tsmiSupplierEdit";
            tsmiSupplierEdit.Size = new Size(165, 32);
            tsmiSupplierEdit.Text = "فتح السند";
            tsmiSupplierEdit.Click += tsmiSupplierEdit_Click;
            // 
            // tsmiSupplierAdd
            // 
            tsmiSupplierAdd.Name = "tsmiSupplierAdd";
            tsmiSupplierAdd.Size = new Size(165, 32);
            tsmiSupplierAdd.Text = "صرف جديد";
            tsmiSupplierAdd.Click += tsmiSupplierAdd_Click;
            // 
            // pnlSupplierTop
            // 
            pnlSupplierTop.BackColor = Color.White;
            pnlSupplierTop.Controls.Add(btnAddSupplierPayment);
            pnlSupplierTop.Controls.Add(lblSupplierCount);
            pnlSupplierTop.Controls.Add(lblSupplierCountTitle);
            pnlSupplierTop.Dock = DockStyle.Top;
            pnlSupplierTop.Location = new Point(3, 3);
            pnlSupplierTop.Name = "pnlSupplierTop";
            pnlSupplierTop.Size = new Size(1186, 40);
            pnlSupplierTop.TabIndex = 0;
            // 
            // btnAddSupplierPayment
            // 
            btnAddSupplierPayment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddSupplierPayment.BackColor = Color.FromArgb(40, 167, 69);
            btnAddSupplierPayment.FlatStyle = FlatStyle.Flat;
            btnAddSupplierPayment.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnAddSupplierPayment.ForeColor = Color.White;
            btnAddSupplierPayment.Location = new Point(1906, 5);
            btnAddSupplierPayment.Name = "btnAddSupplierPayment";
            btnAddSupplierPayment.Size = new Size(106, 30);
            btnAddSupplierPayment.TabIndex = 2;
            btnAddSupplierPayment.Text = "صرف جديد";
            btnAddSupplierPayment.UseVisualStyleBackColor = false;
            btnAddSupplierPayment.Click += btnAddSupplierPayment_Click;
            // 
            // lblSupplierCount
            // 
            lblSupplierCount.AutoSize = true;
            lblSupplierCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSupplierCount.ForeColor = Color.FromArgb(33, 37, 41);
            lblSupplierCount.Location = new Point(50, 10);
            lblSupplierCount.Name = "lblSupplierCount";
            lblSupplierCount.Size = new Size(21, 22);
            lblSupplierCount.TabIndex = 1;
            lblSupplierCount.Text = "0";
            // 
            // lblSupplierCountTitle
            // 
            lblSupplierCountTitle.AutoSize = true;
            lblSupplierCountTitle.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSupplierCountTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblSupplierCountTitle.Location = new Point(10, 10);
            lblSupplierCountTitle.Name = "lblSupplierCountTitle";
            lblSupplierCountTitle.Size = new Size(60, 22);
            lblSupplierCountTitle.TabIndex = 0;
            lblSupplierCountTitle.Text = "العدد:";
            // 
            // frmListPayments
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(1200, 703);
            Controls.Add(tabPayments);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListPayments";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة المقبوضات والمدفوعات";
            Load += frmListPayments_Load;
            pnlHeader.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlSummary.ResumeLayout(false);
            tabPayments.ResumeLayout(false);
            tpCustomerPayments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCustomerPayments).EndInit();
            cmsCustomerPayments.ResumeLayout(false);
            pnlCustomerTop.ResumeLayout(false);
            pnlCustomerTop.PerformLayout();
            tpSupplierPayments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSupplierPayments).EndInit();
            cmsSupplierPayments.ResumeLayout(false);
            pnlSupplierTop.ResumeLayout(false);
            pnlSupplierTop.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.CheckBox chkOnlyCancelled;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Label lblCustomerEmpty;
        private System.Windows.Forms.Label lblSupplierEmpty;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabPayments;
        private System.Windows.Forms.TabPage tpCustomerPayments;
        private System.Windows.Forms.TabPage tpSupplierPayments;
        private System.Windows.Forms.DataGridView dgvCustomerPayments;
        private System.Windows.Forms.Panel pnlCustomerTop;
        private System.Windows.Forms.Button btnAddCustomerPayment;
        private System.Windows.Forms.Label lblCustomerCount;
        private System.Windows.Forms.Label lblCustomerCountTitle;
        private System.Windows.Forms.DataGridView dgvSupplierPayments;
        private System.Windows.Forms.Panel pnlSupplierTop;
        private System.Windows.Forms.Button btnAddSupplierPayment;
        private System.Windows.Forms.Label lblSupplierCount;
        private System.Windows.Forms.Label lblSupplierCountTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ContextMenuStrip cmsCustomerPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiCustomerAdd;
        private System.Windows.Forms.ContextMenuStrip cmsSupplierPayments;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiSupplierAdd;
    }
}
