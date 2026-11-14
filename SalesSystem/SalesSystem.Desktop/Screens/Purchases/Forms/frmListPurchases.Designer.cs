namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmListPurchases
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
            label5 = new Label();
            cbStatus = new ComboBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            chkOnlyDue = new CheckBox();
            chkOnlyCancelled = new CheckBox();
            btnRangeToday = new Button();
            btnRange30 = new Button();
            btnRangeMonth = new Button();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            pnlSummary = new Panel();
            lblSummary = new Label();
            label1 = new Label();
            lblRecordsCount = new Label();
            pnlGrid = new Panel();
            emptyState = new Label();
            dgvPurchases = new DataGridView();
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
            pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).BeginInit();
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
            lblMode.Text = "(( دليل فواتير الشراء )) لائحة الفواتير";
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
            btnAdd.Text = "فاتورة مشتريات";
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
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.FromArgb(248, 249, 250);
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.FromArgb(33, 37, 41);
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
            pnlFilters.Size = new Size(1584, 112);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = Color.FromArgb(235, 245, 235);
            gbFilter.Controls.Add(label5);
            gbFilter.Controls.Add(cbStatus);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(chkOnlyDue);
            gbFilter.Controls.Add(chkOnlyCancelled);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gbFilter.Location = new Point(6, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1572, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label5
            // 
            label5.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(14, 82, 52);
            label5.Location = new Point(1420, 26);
            label5.Margin = new Padding(0);
            label5.Name = "label5";
            label5.Size = new Size(84, 22);
            label5.TabIndex = 0;
            label5.Text = "الحالة:";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbStatus.FormattingEnabled = true;
            cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "مرحل", "ملغي" });
            cbStatus.Location = new Point(1290, 22);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(124, 26);
            cbStatus.TabIndex = 1;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(14, 82, 52);
            label3.Location = new Point(1230, 26);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(52, 22);
            label3.TabIndex = 2;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(1070, 22);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(154, 26);
            dtpFrom.TabIndex = 3;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(14, 82, 52);
            label4.Location = new Point(1010, 26);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(52, 22);
            label4.TabIndex = 4;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(850, 22);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(154, 26);
            dtpTo.TabIndex = 5;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(14, 82, 52);
            label2.Location = new Point(750, 26);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(100, 22);
            label2.TabIndex = 6;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيئ", "المورد", "رقم الفاتورة", "البيان", "المبلغ" });
            cbFilterBy.Location = new Point(560, 22);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(184, 26);
            cbFilterBy.TabIndex = 7;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFilterValue.Location = new Point(352, 23);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.Size = new Size(200, 24);
            txtFilterValue.TabIndex = 8;
            txtFilterValue.Visible = false;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // chkOnlyDue
            // 
            chkOnlyDue.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkOnlyDue.ForeColor = Color.FromArgb(14, 82, 52);
            chkOnlyDue.Location = new Point(180, 62);
            chkOnlyDue.Margin = new Padding(0);
            chkOnlyDue.Name = "chkOnlyDue";
            chkOnlyDue.Size = new Size(160, 24);
            chkOnlyDue.TabIndex = 9;
            chkOnlyDue.Text = "غير المسددة فقط";
            chkOnlyDue.UseVisualStyleBackColor = false;
            chkOnlyDue.CheckedChanged += chkOnlyDue_CheckedChanged;
            // 
            // chkOnlyCancelled
            // 
            chkOnlyCancelled.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkOnlyCancelled.ForeColor = Color.FromArgb(14, 82, 52);
            chkOnlyCancelled.Location = new Point(352, 62);
            chkOnlyCancelled.Margin = new Padding(0);
            chkOnlyCancelled.Name = "chkOnlyCancelled";
            chkOnlyCancelled.Size = new Size(170, 24);
            chkOnlyCancelled.TabIndex = 10;
            chkOnlyCancelled.Text = "الملغاة فقط";
            chkOnlyCancelled.UseVisualStyleBackColor = false;
            chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.FromArgb(108, 117, 125);
            btnRangeToday.FlatStyle = FlatStyle.Flat;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeToday.ForeColor = Color.White;
            btnRangeToday.Location = new Point(1124, 64);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 11;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.FromArgb(108, 117, 125);
            btnRange30.FlatStyle = FlatStyle.Flat;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRange30.ForeColor = Color.White;
            btnRange30.Location = new Point(987, 64);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(128, 30);
            btnRange30.TabIndex = 12;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.FromArgb(108, 117, 125);
            btnRangeMonth.FlatStyle = FlatStyle.Flat;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRangeMonth.ForeColor = Color.White;
            btnRangeMonth.Location = new Point(850, 64);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(100, 30);
            btnRangeMonth.TabIndex = 13;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnClearFilters
            // 
            // RULE-N148 / RULE-N134: مُعلَن في المصمّم بقيم حرفية مطابقة لثوابت الثيم
            // (ActionNeutralBackground = 108,117,125) — معطّل ما لم توجد تصفية نشطة.
            btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(27, 62);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(145, 32);
            btnClearFilters.TabIndex = 15;
            btnClearFilters.Text = "🧹 مسح التصفية";
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
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 104);
            btnToggleFilters.TabIndex = 14;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Controls.Add(label1);
            pnlSummary.Controls.Add(lblRecordsCount);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 829);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Padding = new Padding(8, 0, 8, 0);
            pnlSummary.Size = new Size(1584, 32);
            pnlSummary.TabIndex = 4;
            // 
            // lblSummary
            // 
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
            lblSummary.Location = new Point(164, 0);
            lblSummary.Margin = new Padding(0, 0, 8, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(1412, 32);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — الصافي: 0.00 · المدفوع: 0.00 · المتبقي: 0.00";
            lblSummary.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Right;
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(14, 82, 52);
            label1.Location = new Point(68, 0);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(96, 32);
            label1.TabIndex = 1;
            label1.Text = "عدد الفواتير:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Dock = DockStyle.Right;
            lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCount.Location = new Point(8, 0);
            lblRecordsCount.Margin = new Padding(0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(60, 32);
            lblRecordsCount.TabIndex = 2;
            lblRecordsCount.Text = "0";
            lblRecordsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = Color.FromArgb(235, 240, 235);
            pnlGrid.Controls.Add(emptyState);
            pnlGrid.Controls.Add(dgvPurchases);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 232);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Size = new Size(1584, 597);
            pnlGrid.TabIndex = 3;
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
            emptyState.Size = new Size(1584, 597);
            emptyState.TabIndex = 1;
            emptyState.Text = "لا توجد فواتير مطابقة للمعايير الحالية";
            emptyState.TextAlign = ContentAlignment.MiddleCenter;
            emptyState.Visible = false;
            // 
            // dgvPurchases
            // 
            dgvPurchases.AllowUserToAddRows = false;
            dgvPurchases.AllowUserToDeleteRows = false;
            dgvPurchases.AllowUserToOrderColumns = true;
            dgvPurchases.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvPurchases.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvPurchases.BorderStyle = BorderStyle.None;
            dgvPurchases.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvPurchases.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPurchases.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvPurchases.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvPurchases.ColumnHeadersHeight = 45;
            dgvPurchases.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPurchases.ContextMenuStrip = ctxMenu;
            dgvPurchases.DefaultCellStyle.BackColor = Color.White;
            dgvPurchases.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvPurchases.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvPurchases.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvPurchases.Dock = DockStyle.Fill;
            dgvPurchases.EnableHeadersVisualStyles = false;
            dgvPurchases.GridColor = Color.FromArgb(205, 225, 205);
            dgvPurchases.Location = new Point(0, 0);
            dgvPurchases.Margin = new Padding(0);
            dgvPurchases.MultiSelect = false;
            dgvPurchases.Name = "dgvPurchases";
            dgvPurchases.ReadOnly = true;
            dgvPurchases.RowHeadersVisible = false;
            dgvPurchases.RowHeadersWidth = 51;
            dgvPurchases.RowTemplate.Height = 32;
            dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPurchases.Size = new Size(1584, 597);
            dgvPurchases.TabIndex = 0;
            dgvPurchases.TabStop = false;
            dgvPurchases.CellFormatting += dgvPurchases_CellFormatting;
            dgvPurchases.SelectionChanged += dgvPurchases_SelectionChanged;
            dgvPurchases.DoubleClick += dgvPurchases_DoubleClick;
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
            // frmListPurchases
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1584, 861);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.Sizable;
            KeyPreview = true;
            MinimumSize = new Size(1200, 700);
            Name = "frmListPurchases";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "فواتير الشراء";
            Load += frmListPurchases_Load;
            pnlHeader.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            flpToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlSummary.ResumeLayout(false);
            pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPurchases).EndInit();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtFilterValue;
        private System.Windows.Forms.CheckBox chkOnlyDue;
        private System.Windows.Forms.CheckBox chkOnlyCancelled;
        private System.Windows.Forms.Button btnRangeToday;
        private System.Windows.Forms.Button btnRange30;
        private System.Windows.Forms.Button btnRangeMonth;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.Button btnToggleFilters;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.Label emptyState;
        private System.Windows.Forms.DataGridView dgvPurchases;
        private System.Windows.Forms.ContextMenuStrip ctxMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem tsmiPost;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancel;
        private System.Windows.Forms.ToolStripSeparator tsSep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyInvoiceNo;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
