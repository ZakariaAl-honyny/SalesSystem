namespace SalesSystem.Desktop.Screens.Sales.Forms
{
    partial class frmListSales
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
            btnAdd = new Button();
            btnView = new Button();
            btnPost = new Button();
            btnCancel = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            label1 = new Label();
            cbStatus = new ComboBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            chkOnlyDue = new CheckBox();
            btnRangeToday = new Button();
            btnRangeMonth = new Button();
            btnRange30 = new Button();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            pnlSummary = new Panel();
            flpSummary = new FlowLayoutPanel();
            lblCountCaption = new Label();
            lblRecordsCount = new Label();
            lblTotalCaption = new Label();
            lblTotalAmount = new Label();
            lblPaidCaption = new Label();
            lblTotalPaid = new Label();
            lblDueCaption = new Label();
            lblTotalDue = new Label();
            pnlFooter = new Panel();
            lblHint = new Label();
            pnlGrid = new Panel();
            emptyState = new Label();
            dgvSales = new DataGridView();
            colInvoiceNo = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colCustomer = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colPaid = new DataGridViewTextBoxColumn();
            colDue = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colNotes = new DataGridViewTextBoxColumn();
            ctxMenu = new ContextMenuStrip(components);
            tsmiView = new ToolStripMenuItem();
            tsmiPost = new ToolStripMenuItem();
            tsmiCancel = new ToolStripMenuItem();
            tsSep1 = new ToolStripSeparator();
            tsmiCopyInvoiceNo = new ToolStripMenuItem();
            toolTip1 = new ToolTip(components);
            pnlHeader.SuspendLayout();
            pnlToolbar.SuspendLayout();
            flpToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlSummary.SuspendLayout();
            flpSummary.SuspendLayout();
            pnlFooter.SuspendLayout();
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            ctxMenu.SuspendLayout();
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
            pnlHeader.Size = new Size(1584, 56);
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
            lblMode.Size = new Size(1584, 56);
            lblMode.TabIndex = 0;
            lblMode.Text = "(( دليل فواتير البيع )) لائحة الفواتير";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
            pnlToolbar.Controls.Add(flpToolbar);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1584, 64);
            pnlToolbar.TabIndex = 1;
            // 
            // flpToolbar
            // 
            flpToolbar.BackColor = Color.FromArgb(235, 240, 235);
            flpToolbar.Controls.Add(btnAdd);
            flpToolbar.Controls.Add(btnView);
            flpToolbar.Controls.Add(btnPost);
            flpToolbar.Controls.Add(btnCancel);
            flpToolbar.Controls.Add(btnRefresh);
            flpToolbar.Controls.Add(btnClose);
            flpToolbar.Dock = DockStyle.Fill;
            flpToolbar.Location = new Point(0, 0);
            flpToolbar.Margin = new Padding(0);
            flpToolbar.Name = "flpToolbar";
            flpToolbar.Padding = new Padding(10);
            flpToolbar.Size = new Size(1584, 64);
            flpToolbar.TabIndex = 0;
            flpToolbar.WrapContents = false;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(13, 110, 253);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(1421, 13);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(140, 40);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "فاتورة جديدة";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnView
            // 
            btnView.BackColor = Color.FromArgb(248, 249, 250);
            btnView.Enabled = false;
            btnView.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnView.FlatAppearance.BorderSize = 1;
            btnView.FlatStyle = FlatStyle.Flat;
            btnView.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnView.ForeColor = Color.FromArgb(33, 37, 41);
            btnView.Location = new Point(1275, 13);
            btnView.Name = "btnView";
            btnView.Size = new Size(140, 40);
            btnView.TabIndex = 1;
            btnView.Text = "عرض / تعديل";
            btnView.UseVisualStyleBackColor = false;
            btnView.Click += btnView_Click;
            // 
            // btnPost
            // 
            btnPost.BackColor = Color.FromArgb(25, 135, 84);
            btnPost.Enabled = false;
            btnPost.FlatStyle = FlatStyle.Flat;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPost.ForeColor = Color.White;
            btnPost.Location = new Point(1129, 13);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(140, 40);
            btnPost.TabIndex = 2;
            btnPost.Text = "ترحيل";
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(220, 53, 69);
            btnCancel.Enabled = false;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(983, 13);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(140, 40);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "إلغاء الفاتورة";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(108, 117, 125);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(837, 13);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 40);
            btnRefresh.TabIndex = 4;
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
            btnClose.Location = new Point(691, 13);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(140, 40);
            btnClose.TabIndex = 5;
            btnClose.Text = "خروج";
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
            pnlFilters.Size = new Size(1584, 110);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = Color.FromArgb(235, 245, 235);
            gbFilter.Controls.Add(label1);
            gbFilter.Controls.Add(cbStatus);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(chkOnlyDue);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbFilter.Location = new Point(6, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(10);
            gbFilter.Size = new Size(1572, 102);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label1
            // 
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(14, 82, 52);
            label1.Location = new Point(1420, 34);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(82, 26);
            label1.TabIndex = 0;
            label1.Text = "الحالة:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(1290, 30);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(124, 32);
            cbStatus.TabIndex = 1;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(14, 82, 52);
            label3.Location = new Point(1230, 34);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(60, 26);
            label3.TabIndex = 2;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(1070, 30);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(154, 32);
            dtpFrom.TabIndex = 3;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(14, 82, 52);
            label4.Location = new Point(1010, 34);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(60, 26);
            label4.TabIndex = 4;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(850, 30);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(154, 32);
            dtpTo.TabIndex = 5;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(14, 82, 52);
            label2.Location = new Point(750, 34);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(100, 26);
            label2.TabIndex = 6;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "رقم الفاتورة", "اسم العميل", "الحالة" });
            cbFilterBy.Location = new Point(560, 30);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(184, 32);
            cbFilterBy.TabIndex = 7;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(352, 30);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(200, 32);
            txtFilterValue.TabIndex = 8;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // chkOnlyDue
            // 
            chkOnlyDue.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkOnlyDue.ForeColor = Color.FromArgb(14, 82, 52);
            chkOnlyDue.Location = new Point(180, 32);
            chkOnlyDue.Margin = new Padding(0);
            chkOnlyDue.Name = "chkOnlyDue";
            chkOnlyDue.Size = new Size(160, 28);
            chkOnlyDue.TabIndex = 9;
            chkOnlyDue.Text = "غير المسددة فقط";
            chkOnlyDue.UseVisualStyleBackColor = false;
            chkOnlyDue.CheckedChanged += chkOnlyDue_CheckedChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.FromArgb(108, 117, 125);
            btnRangeToday.FlatStyle = FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeToday.ForeColor = Color.White;
            btnRangeToday.Location = new Point(1124, 65);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 10;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.FromArgb(108, 117, 125);
            btnRangeMonth.FlatStyle = FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeMonth.ForeColor = Color.White;
            btnRangeMonth.Location = new Point(850, 65);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(100, 30);
            btnRangeMonth.TabIndex = 11;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.FromArgb(108, 117, 125);
            btnRange30.FlatStyle = FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRange30.ForeColor = Color.White;
            btnRange30.Location = new Point(987, 65);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(128, 30);
            btnRange30.TabIndex = 12;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // btnClearFilters
            // 
            // RULE-N148: «مسح التصفية» — يمسح أدوات التصفية وحدها بلا إعادة تحميل،
            // ومُعلَن في المصمّم بقيم حرفية مطابقة لِما يُطبَّق وقت التشغيل (RULE-N134/RULE-N145):
            // ActionNeutralBackground (108,117,125) + نص أبيض + Tahoma 9 Bold + معطّل افتراضياً.
            btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(20, 64);
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
            // RULE-N145: زر «إخفاء/إظهار التصفية» مُعلَن في المصمّم بقيم حرفية مطابقة لما يُطبَّق
            // وقت التشغيل (نمط الزر الثانوي + عرض 140 + Tahoma 9 Bold)؛ والسلوك
            // (النقرة · F9 · التلميح · الحالة المحفوظة) يُوصِّله FilterPanelHelper.Attach.
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
            btnToggleFilters.Size = new Size(140, 102);
            btnToggleFilters.TabIndex = 13;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
            pnlSummary.Controls.Add(flpSummary);
            // RULE-N135: الإجماليات بعد المحتوى (أسفل الشبكة) وقبل شريط الأسفل.
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 230);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1584, 46);
            pnlSummary.TabIndex = 3;
            // 
            // flpSummary
            // 
            flpSummary.BackColor = Color.FromArgb(235, 240, 235);
            flpSummary.Controls.Add(lblCountCaption);
            flpSummary.Controls.Add(lblRecordsCount);
            flpSummary.Controls.Add(lblTotalCaption);
            flpSummary.Controls.Add(lblTotalAmount);
            flpSummary.Controls.Add(lblPaidCaption);
            flpSummary.Controls.Add(lblTotalPaid);
            flpSummary.Controls.Add(lblDueCaption);
            flpSummary.Controls.Add(lblTotalDue);
            flpSummary.Dock = DockStyle.Fill;
            flpSummary.Location = new Point(0, 0);
            flpSummary.Margin = new Padding(0);
            flpSummary.Name = "flpSummary";
            flpSummary.Padding = new Padding(10, 8, 10, 8);
            flpSummary.Size = new Size(1584, 46);
            flpSummary.TabIndex = 0;
            flpSummary.WrapContents = false;
            // 
            // lblCountCaption
            // 
            lblCountCaption.AutoSize = true;
            lblCountCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCountCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblCountCaption.Location = new Point(1434, 8);
            lblCountCaption.Margin = new Padding(4, 0, 4, 0);
            lblCountCaption.Name = "lblCountCaption";
            lblCountCaption.Size = new Size(126, 24);
            lblCountCaption.TabIndex = 0;
            lblCountCaption.Text = "عدد الفواتير:";
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.AutoSize = true;
            lblRecordsCount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCount.Location = new Point(1403, 8);
            lblRecordsCount.Margin = new Padding(4, 0, 24, 0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(23, 24);
            lblRecordsCount.TabIndex = 1;
            lblRecordsCount.Text = "0";
            // 
            // lblTotalCaption
            // 
            lblTotalCaption.AutoSize = true;
            lblTotalCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalCaption.Location = new Point(1273, 8);
            lblTotalCaption.Margin = new Padding(4, 0, 4, 0);
            lblTotalCaption.Name = "lblTotalCaption";
            lblTotalCaption.Size = new Size(102, 24);
            lblTotalCaption.TabIndex = 2;
            lblTotalCaption.Text = "الإجمالي:";
            // 
            // lblTotalAmount
            // 
            lblTotalAmount.AutoSize = true;
            lblTotalAmount.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalAmount.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalAmount.Location = new Point(1210, 8);
            lblTotalAmount.Margin = new Padding(4, 0, 24, 0);
            lblTotalAmount.Name = "lblTotalAmount";
            lblTotalAmount.Size = new Size(55, 24);
            lblTotalAmount.TabIndex = 3;
            lblTotalAmount.Text = "0.00";
            // 
            // lblPaidCaption
            // 
            lblPaidCaption.AutoSize = true;
            lblPaidCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPaidCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblPaidCaption.Location = new Point(1091, 8);
            lblPaidCaption.Margin = new Padding(4, 0, 4, 0);
            lblPaidCaption.Name = "lblPaidCaption";
            lblPaidCaption.Size = new Size(91, 24);
            lblPaidCaption.TabIndex = 4;
            lblPaidCaption.Text = "المدفوع:";
            // 
            // lblTotalPaid
            // 
            lblTotalPaid.AutoSize = true;
            lblTotalPaid.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalPaid.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalPaid.Location = new Point(1028, 8);
            lblTotalPaid.Margin = new Padding(4, 0, 24, 0);
            lblTotalPaid.Name = "lblTotalPaid";
            lblTotalPaid.Size = new Size(55, 24);
            lblTotalPaid.TabIndex = 5;
            lblTotalPaid.Text = "0.00";
            // 
            // lblDueCaption
            // 
            lblDueCaption.AutoSize = true;
            lblDueCaption.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDueCaption.ForeColor = Color.FromArgb(14, 82, 52);
            lblDueCaption.Location = new Point(907, 8);
            lblDueCaption.Margin = new Padding(4, 0, 4, 0);
            lblDueCaption.Name = "lblDueCaption";
            lblDueCaption.Size = new Size(93, 24);
            lblDueCaption.TabIndex = 6;
            lblDueCaption.Text = "المتبقي:";
            // 
            // lblTotalDue
            // 
            lblTotalDue.AutoSize = true;
            lblTotalDue.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalDue.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalDue.Location = new Point(844, 8);
            lblTotalDue.Margin = new Padding(4, 0, 4, 0);
            lblTotalDue.Name = "lblTotalDue";
            lblTotalDue.Size = new Size(55, 24);
            lblTotalDue.TabIndex = 7;
            lblTotalDue.Text = "0.00";
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
            pnlFooter.Controls.Add(lblHint);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 803);
            pnlFooter.Margin = new Padding(0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(10, 0, 10, 0);
            pnlFooter.Size = new Size(1584, 58);
            pnlFooter.TabIndex = 5;
            // 
            // lblHint
            // 
            lblHint.Dock = DockStyle.Fill;
            lblHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHint.ForeColor = Color.FromArgb(14, 82, 52);
            lblHint.Location = new Point(10, 0);
            lblHint.Margin = new Padding(0);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(1564, 58);
            lblHint.TabIndex = 0;
            lblHint.Text = "F2 جديد · F3 عرض/تعديل · F4 ترحيل · Delete إلغاء · F5 تحديث · Enter = التالي";
            lblHint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = Color.FromArgb(235, 240, 235);
            pnlGrid.Controls.Add(emptyState);
            pnlGrid.Controls.Add(dgvSales);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 276);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Size = new Size(1584, 527);
            pnlGrid.TabIndex = 4;
            // 
            // emptyState
            // 
            emptyState.BackColor = Color.Transparent;
            emptyState.Dock = DockStyle.Fill;
            emptyState.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            emptyState.ForeColor = Color.FromArgb(14, 82, 52);
            emptyState.Location = new Point(0, 0);
            emptyState.Margin = new Padding(0);
            emptyState.Name = "emptyState";
            emptyState.Size = new Size(1584, 527);
            emptyState.TabIndex = 1;
            emptyState.Text = "لا توجد فواتير مطابقة للمعايير الحالية";
            emptyState.TextAlign = ContentAlignment.MiddleCenter;
            emptyState.Visible = false;
            // 
            // dgvSales
            // 
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AllowUserToOrderColumns = true;
            dgvSales.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvSales.BorderStyle = BorderStyle.None;
            dgvSales.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvSales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSales.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvSales.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvSales.ColumnHeadersHeight = 45;
            dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSales.DefaultCellStyle.BackColor = Color.White;
            dgvSales.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvSales.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvSales.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvSales.EnableHeadersVisualStyles = false;
            dgvSales.GridColor = Color.FromArgb(205, 225, 205);
            dgvSales.Columns.AddRange(new DataGridViewColumn[] { colInvoiceNo, colDate, colCustomer, colTotal, colPaid, colDue, colStatus, colNotes });
            dgvSales.ContextMenuStrip = ctxMenu;
            dgvSales.Dock = DockStyle.Fill;
            dgvSales.Location = new Point(0, 0);
            dgvSales.Margin = new Padding(0);
            dgvSales.MultiSelect = false;
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersVisible = false;
            dgvSales.RowHeadersWidth = 51;
            dgvSales.RowTemplate.Height = 32;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new Size(1584, 527);
            dgvSales.TabIndex = 0;
            dgvSales.TabStop = false;
            dgvSales.CellFormatting += dgvSales_CellFormatting;
            dgvSales.SelectionChanged += dgvSales_SelectionChanged;
            dgvSales.DoubleClick += dgvSales_DoubleClick;
            // 
            // colInvoiceNo
            // 
            colInvoiceNo.FillWeight = 8F;
            colInvoiceNo.HeaderText = "رقم الفاتورة";
            colInvoiceNo.MinimumWidth = 80;
            colInvoiceNo.Name = "colInvoiceNo";
            colInvoiceNo.ReadOnly = true;
            // 
            // colDate
            // 
            colDate.FillWeight = 10F;
            colDate.HeaderText = "التاريخ";
            colDate.MinimumWidth = 90;
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
            // 
            // colCustomer
            // 
            colCustomer.FillWeight = 22F;
            colCustomer.HeaderText = "العميل";
            colCustomer.MinimumWidth = 140;
            colCustomer.Name = "colCustomer";
            colCustomer.ReadOnly = true;
            // 
            // colTotal
            // 
            colTotal.FillWeight = 12F;
            colTotal.HeaderText = "الإجمالي";
            colTotal.MinimumWidth = 100;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            colTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPaid
            // 
            colPaid.FillWeight = 12F;
            colPaid.HeaderText = "المدفوع";
            colPaid.MinimumWidth = 100;
            colPaid.Name = "colPaid";
            colPaid.ReadOnly = true;
            colPaid.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colDue
            // 
            colDue.FillWeight = 12F;
            colDue.HeaderText = "المتبقي";
            colDue.MinimumWidth = 100;
            colDue.Name = "colDue";
            colDue.ReadOnly = true;
            colDue.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colStatus
            // 
            colStatus.FillWeight = 8F;
            colStatus.HeaderText = "الحالة";
            colStatus.MinimumWidth = 80;
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colNotes
            // 
            colNotes.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colNotes.FillWeight = 16F;
            colNotes.HeaderText = "ملاحظات";
            colNotes.MinimumWidth = 120;
            colNotes.Name = "colNotes";
            colNotes.ReadOnly = true;
            colNotes.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // ctxMenu
            // 
            ctxMenu.ImageScalingSize = new Size(24, 24);
            ctxMenu.Items.AddRange(new ToolStripItem[] { tsmiView, tsmiPost, tsmiCancel, tsSep1, tsmiCopyInvoiceNo });
            ctxMenu.Name = "ctxMenu";
            ctxMenu.Size = new Size(242, 138);
            ctxMenu.Opening += ctxMenu_Opening;
            // 
            // tsmiView
            // 
            tsmiView.Name = "tsmiView";
            tsmiView.ShortcutKeyDisplayString = "F3";
            tsmiView.Size = new Size(241, 32);
            tsmiView.Text = "عرض / تعديل";
            tsmiView.Click += tsmiView_Click;
            // 
            // tsmiPost
            // 
            tsmiPost.Name = "tsmiPost";
            tsmiPost.ShortcutKeyDisplayString = "F4";
            tsmiPost.Size = new Size(241, 32);
            tsmiPost.Text = "ترحيل";
            tsmiPost.Click += tsmiPost_Click;
            // 
            // tsmiCancel
            // 
            tsmiCancel.Name = "tsmiCancel";
            tsmiCancel.ShortcutKeyDisplayString = "Delete";
            tsmiCancel.Size = new Size(241, 32);
            tsmiCancel.Text = "إلغاء الفاتورة";
            tsmiCancel.Click += tsmiCancel_Click;
            // 
            // tsSep1
            // 
            tsSep1.Name = "tsSep1";
            tsSep1.Size = new Size(238, 6);
            // 
            // tsmiCopyInvoiceNo
            // 
            tsmiCopyInvoiceNo.Name = "tsmiCopyInvoiceNo";
            tsmiCopyInvoiceNo.Size = new Size(241, 32);
            tsmiCopyInvoiceNo.Text = "نسخ رقم الفاتورة";
            tsmiCopyInvoiceNo.Click += tsmiCopyInvoiceNo_Click;
            // 
            // frmListSales
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            ClientSize = new Size(1584, 861);
            Controls.Add(pnlGrid);
            // RULE-N135: الإجماليات قبل شريط الأسفل ⇒ يُضاف pnlSummary قبل pnlFooter
            // (يُرسى آخر عنصر مُضاف أولاً، فيأخذ pnlFooter الحافة السفلية ويجلس الملخص فوقه).
            Controls.Add(pnlSummary);
            Controls.Add(pnlFooter);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            KeyPreview = true;
            MinimumSize = new Size(1200, 700);
            Name = "frmListSales";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "دليل فواتير البيع";
            Load += frmListSales_Load;
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlSummary.ResumeLayout(false);
            flpSummary.ResumeLayout(false);
            flpSummary.PerformLayout();
            pnlFooter.ResumeLayout(false);
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ctxMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.CheckBox chkOnlyDue;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Forms.Label lblCountCaption;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label lblTotalCaption;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblPaidCaption;
        private System.Windows.Forms.Label lblTotalPaid;
        private System.Windows.Forms.Label lblDueCaption;
        private System.Windows.Forms.Label lblTotalDue;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;
        private System.Windows.Forms.Label emptyState;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem tsmiPost;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancel;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyInvoiceNo;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
