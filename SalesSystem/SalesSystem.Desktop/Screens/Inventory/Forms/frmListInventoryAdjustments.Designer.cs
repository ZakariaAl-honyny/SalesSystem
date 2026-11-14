namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmListInventoryAdjustments
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
            contextMenuStrip = new ContextMenuStrip(components);
            tsmiView = new ToolStripMenuItem();
            tsmiPost = new ToolStripMenuItem();
            tsmiCancel = new ToolStripMenuItem();
            dgvAdjustments = new DataGridView();
            pnlHeader = new Panel();
            lblMode = new Label();
            gbCommands = new GroupBox();
            pnlToolbar = new Panel();
            btnRefresh = new Button();
            btnCancelAdjustment = new Button();
            btnPost = new Button();
            btnView = new Button();
            btnAdd = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            btnToggleFilters = new Button();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label1 = new Label();
            cbStatus = new ComboBox();
            btnRangeMonth = new Button();
            btnRange30 = new Button();
            btnRangeToday = new Button();
            chkIncludeCancelled = new CheckBox();
            chkOnlyCancelled = new CheckBox();
            btnClearFilters = new Button();
            pnlGrid = new GroupBox();
            lblEmpty = new Label();
            pnlSummary = new GroupBox();
            lblSummary = new Label();
            lblRecordsCount = new Label();
            toolTip1 = new ToolTip(components);
            contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAdjustments).BeginInit();
            pnlHeader.SuspendLayout();
            gbCommands.SuspendLayout();
            pnlToolbar.SuspendLayout();
            pnlFilters.SuspendLayout();
            gbFilter.SuspendLayout();
            pnlGrid.SuspendLayout();
            pnlSummary.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Font = new Font("Tahoma", 10F);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { tsmiView, tsmiPost, tsmiCancel });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.RightToLeft = RightToLeft.Yes;
            contextMenuStrip.Size = new Size(180, 100);
            // 
            // tsmiView
            // 
            tsmiView.Name = "tsmiView";
            tsmiView.Size = new Size(179, 30);
            tsmiView.Text = "عرض التفاصيل";
            tsmiView.Click += tsmiView_Click;
            // 
            // tsmiPost
            // 
            tsmiPost.Name = "tsmiPost";
            tsmiPost.Size = new Size(179, 30);
            tsmiPost.Text = "ترحيل";
            tsmiPost.Click += tsmiPost_Click;
            // 
            // tsmiCancel
            // 
            tsmiCancel.Name = "tsmiCancel";
            tsmiCancel.Size = new Size(179, 30);
            tsmiCancel.Text = "إلغاء";
            tsmiCancel.Click += tsmiCancel_Click;
            // 
            // dgvAdjustments
            // 
            // RULE-N134: نفس ألوان GreenThemeHelper حرفياً في المصمّم ووقت التشغيل.
            dgvAdjustments.AllowUserToAddRows = false;
            dgvAdjustments.AllowUserToDeleteRows = false;
            dgvAdjustments.AllowUserToOrderColumns = true;
            dgvAdjustments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvAdjustments.AutoGenerateColumns = false;
            dgvAdjustments.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvAdjustments.BorderStyle = BorderStyle.None;
            dgvAdjustments.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvAdjustments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvAdjustments.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgvAdjustments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAdjustments.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvAdjustments.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvAdjustments.ColumnHeadersHeight = 45;
            dgvAdjustments.ContextMenuStrip = contextMenuStrip;
            dgvAdjustments.DefaultCellStyle.BackColor = Color.White;
            dgvAdjustments.DefaultCellStyle.Font = new Font("Tahoma", 10F);
            dgvAdjustments.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvAdjustments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvAdjustments.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvAdjustments.Dock = DockStyle.Fill;
            dgvAdjustments.EnableHeadersVisualStyles = false;
            dgvAdjustments.GridColor = Color.FromArgb(205, 225, 205);
            dgvAdjustments.Location = new Point(6, 6);
            dgvAdjustments.Margin = new Padding(0);
            dgvAdjustments.MultiSelect = false;
            dgvAdjustments.Name = "dgvAdjustments";
            dgvAdjustments.ReadOnly = true;
            dgvAdjustments.RowHeadersVisible = false;
            dgvAdjustments.RowHeadersWidth = 62;
            dgvAdjustments.RowTemplate.Height = 32;
            dgvAdjustments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdjustments.Size = new Size(1552, 489);
            dgvAdjustments.TabIndex = 0;
            dgvAdjustments.TabStop = false;
            dgvAdjustments.CellDoubleClick += dgvAdjustments_CellDoubleClick;
            dgvAdjustments.MouseDown += dgvAdjustments_MouseDown;
            dgvAdjustments.SelectionChanged += dgvAdjustments_SelectionChanged;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(14, 165, 103);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(4);
            pnlHeader.Size = new Size(1564, 56);
            pnlHeader.TabIndex = 0;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(4, 4);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1556, 48);
            lblMode.TabIndex = 0;
            lblMode.Text = "تسويات المخزون";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbCommands — «أوامر الشاشة»: صندوق عنوان حول شريط الأزرار (RULE-N136)
            // 
            gbCommands.BackColor = Color.FromArgb(235, 240, 235);
            gbCommands.Controls.Add(pnlToolbar);
            gbCommands.Dock = DockStyle.Top;
            gbCommands.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbCommands.Location = new Point(0, 56);
            gbCommands.Margin = new Padding(0);
            gbCommands.Name = "gbCommands";
            gbCommands.Size = new Size(1564, 82);
            gbCommands.TabIndex = 1;
            gbCommands.TabStop = false;
            gbCommands.Text = "أوامر الشاشة";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(235, 240, 235);
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnCancelAdjustment);
            pnlToolbar.Controls.Add(btnPost);
            pnlToolbar.Controls.Add(btnView);
            pnlToolbar.Controls.Add(btnAdd);
            pnlToolbar.Controls.Add(btnClose);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(3, 22);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1558, 57);
            pnlToolbar.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(802, 5);
            btnRefresh.Margin = new Padding(0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 46);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnCancelAdjustment
            // 
            btnCancelAdjustment.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancelAdjustment.BackColor = Color.White;
            btnCancelAdjustment.FlatStyle = FlatStyle.Popup;
            btnCancelAdjustment.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancelAdjustment.Location = new Point(932, 5);
            btnCancelAdjustment.Margin = new Padding(0);
            btnCancelAdjustment.Name = "btnCancelAdjustment";
            btnCancelAdjustment.Size = new Size(120, 46);
            btnCancelAdjustment.TabIndex = 4;
            btnCancelAdjustment.Text = "إلغاء الترحيل";
            btnCancelAdjustment.UseVisualStyleBackColor = false;
            btnCancelAdjustment.Click += btnCancel_Click;
            // 
            // btnPost
            // 
            btnPost.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPost.BackColor = Color.White;
            btnPost.FlatStyle = FlatStyle.Popup;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPost.Location = new Point(1062, 5);
            btnPost.Margin = new Padding(0);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(120, 46);
            btnPost.TabIndex = 3;
            btnPost.Text = "ترحيل";
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnView
            // 
            btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnView.BackColor = Color.White;
            btnView.FlatStyle = FlatStyle.Popup;
            btnView.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnView.Location = new Point(1192, 5);
            btnView.Margin = new Padding(0);
            btnView.Name = "btnView";
            btnView.Size = new Size(120, 46);
            btnView.TabIndex = 2;
            btnView.Text = "عرض";
            btnView.UseVisualStyleBackColor = false;
            btnView.Click += btnView_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1322, 5);
            btnAdd.Margin = new Padding(0);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 46);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "تسوية جديدة";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(1452, 5);
            btnClose.Margin = new Padding(0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 46);
            btnClose.TabIndex = 0;
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
            pnlFilters.Location = new Point(0, 138);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1564, 112);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = Color.FromArgb(235, 245, 235);
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label1);
            gbFilter.Controls.Add(cbStatus);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(chkIncludeCancelled);
            gbFilter.Controls.Add(chkOnlyCancelled);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(6, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1412, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // btnToggleFilters — RULE-N145/RULE-N63: يُعلَن في المصمّم ويتبنّاه FilterPanelHelper
            // 
            btnToggleFilters.BackColor = Color.FromArgb(248, 249, 250);
            btnToggleFilters.Cursor = Cursors.Hand;
            btnToggleFilters.Dock = DockStyle.Left;
            btnToggleFilters.FlatAppearance.BorderColor = Color.FromArgb(222, 226, 230);
            btnToggleFilters.FlatAppearance.BorderSize = 1;
            btnToggleFilters.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnToggleFilters.FlatStyle = FlatStyle.Popup;
            btnToggleFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnToggleFilters.ForeColor = Color.FromArgb(33, 37, 41);
            btnToggleFilters.Location = new Point(6, 4);
            btnToggleFilters.Name = "btnToggleFilters";
            btnToggleFilters.Size = new Size(140, 104);
            btnToggleFilters.TabIndex = 20;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.Location = new Point(728, 28);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(85, 26);
            label2.TabIndex = 6;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.BackColor = Color.White;
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيء", "رقم التسوية", "المستودع", "البيان" });
            cbFilterBy.Location = new Point(558, 27);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(160, 26);
            cbFilterBy.TabIndex = 7;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = Color.White;
            txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            txtFilterValue.Location = new Point(330, 27);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.RightToLeft = RightToLeft.Yes;
            txtFilterValue.Size = new Size(220, 26);
            txtFilterValue.TabIndex = 8;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyDown += txtFilterValue_KeyDown;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // label4
            // 
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.Location = new Point(1003, 28);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(48, 26);
            label4.TabIndex = 4;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.BackColor = Color.White;
            dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(865, 27);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(130, 26);
            dtpTo.TabIndex = 5;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.Location = new Point(811, 28);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(48, 26);
            label3.TabIndex = 2;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = Color.White;
            dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(673, 27);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(130, 26);
            dtpFrom.TabIndex = 3;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label1
            // 
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label1.Location = new Point(1315, 28);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(70, 26);
            label1.TabIndex = 0;
            label1.Text = "الحالة:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.BackColor = Color.White;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbStatus.FormattingEnabled = true;
            cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "مرحّل", "ملغى" });
            cbStatus.Location = new Point(1187, 27);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(120, 26);
            cbStatus.TabIndex = 1;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.White;
            btnRangeMonth.FlatStyle = FlatStyle.Popup;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.Location = new Point(1115, 64);
            btnRangeMonth.Margin = new Padding(0);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(110, 30);
            btnRangeMonth.TabIndex = 9;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.White;
            btnRange30.FlatStyle = FlatStyle.Popup;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.Location = new Point(1235, 64);
            btnRange30.Margin = new Padding(0);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(125, 30);
            btnRange30.TabIndex = 10;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.White;
            btnRangeToday.FlatStyle = FlatStyle.Popup;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.Location = new Point(1315, 64);
            btnRangeToday.Margin = new Padding(0);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(70, 30);
            btnRangeToday.TabIndex = 11;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // chkIncludeCancelled
            // 
            chkIncludeCancelled.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkIncludeCancelled.Location = new Point(949, 66);
            chkIncludeCancelled.Margin = new Padding(0);
            chkIncludeCancelled.Name = "chkIncludeCancelled";
            chkIncludeCancelled.Size = new Size(150, 26);
            chkIncludeCancelled.TabIndex = 12;
            chkIncludeCancelled.Text = "عرض الملغاة";
            chkIncludeCancelled.UseVisualStyleBackColor = false;
            chkIncludeCancelled.CheckedChanged += chkIncludeCancelled_CheckedChanged;
            // 
            // chkOnlyCancelled
            // 
            chkOnlyCancelled.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkOnlyCancelled.Location = new Point(794, 66);
            chkOnlyCancelled.Margin = new Padding(0);
            chkOnlyCancelled.Name = "chkOnlyCancelled";
            chkOnlyCancelled.Size = new Size(140, 26);
            chkOnlyCancelled.TabIndex = 13;
            chkOnlyCancelled.Text = "الملغاة فقط";
            chkOnlyCancelled.UseVisualStyleBackColor = false;
            chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
            // 
            // btnClearFilters — RULE-N148: «مسح التصفية» داخل gbFilter ولا يعيد تحميل البيانات
            // 
            btnClearFilters.BackColor = Color.FromArgb(108, 117, 125);
            btnClearFilters.Cursor = Cursors.Hand;
            btnClearFilters.Enabled = false;
            btnClearFilters.FlatAppearance.BorderSize = 0;
            btnClearFilters.FlatStyle = FlatStyle.Flat;
            btnClearFilters.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnClearFilters.ForeColor = Color.White;
            btnClearFilters.Location = new Point(630, 63);
            btnClearFilters.Margin = new Padding(0);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(145, 32);
            btnClearFilters.TabIndex = 14;
            btnClearFilters.Text = "🧹 مسح التصفية";
            toolTip1.SetToolTip(btnClearFilters, "مسح كل أدوات التصفية (Esc) — لا يعيد تحميل البيانات");
            btnClearFilters.UseVisualStyleBackColor = false;
            btnClearFilters.Click += btnClearFilters_Click;
            // 
            // pnlGrid — «البيانات»: صندوق عنوان حول الشبكة (RULE-N136)
            // 
            pnlGrid.BackColor = Color.FromArgb(235, 245, 235);
            pnlGrid.Controls.Add(lblEmpty);
            pnlGrid.Controls.Add(dgvAdjustments);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            pnlGrid.Location = new Point(0, 250);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(6);
            pnlGrid.Size = new Size(1564, 481);
            pnlGrid.TabIndex = 3;
            pnlGrid.TabStop = false;
            pnlGrid.Text = "البيانات";
            // 
            // lblEmpty
            // 
            lblEmpty.BackColor = Color.Transparent;
            lblEmpty.Dock = DockStyle.Fill;
            lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblEmpty.ForeColor = Color.FromArgb(14, 82, 52);
            lblEmpty.Location = new Point(6, 6);
            lblEmpty.Margin = new Padding(0);
            lblEmpty.Name = "lblEmpty";
            lblEmpty.Size = new Size(1552, 489);
            lblEmpty.TabIndex = 1;
            lblEmpty.Text = "لا توجد بيانات للعرض";
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Visible = false;
            // 
            // pnlSummary — «الإجماليات» (RULE-N136)
            // 
            pnlSummary.BackColor = Color.FromArgb(235, 240, 235);
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Controls.Add(lblRecordsCount);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            pnlSummary.Location = new Point(0, 731);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Padding = new Padding(8, 18, 8, 4);
            pnlSummary.Size = new Size(1564, 52);
            pnlSummary.TabIndex = 4;
            pnlSummary.TabStop = false;
            pnlSummary.Text = "الإجماليات";
            // 
            // lblSummary
            // 
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(14, 82, 52);
            lblSummary.Location = new Point(90, 18);
            lblSummary.Margin = new Padding(0);
            lblSummary.Name = "lblSummary";
            lblSummary.Padding = new Padding(8, 0, 8, 0);
            lblSummary.Size = new Size(1474, 26);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — مسودات: 0 · مُرحَّلة: 0 · ملغاة: 0";
            lblSummary.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Dock = DockStyle.Right;
            lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblRecordsCount.ForeColor = Color.FromArgb(14, 82, 52);
            lblRecordsCount.Location = new Point(1474, 18);
            lblRecordsCount.Margin = new Padding(0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(90, 26);
            lblRecordsCount.TabIndex = 1;
            lblRecordsCount.Text = "0";
            lblRecordsCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 300;
            toolTip1.BackColor = Color.White;
            toolTip1.ForeColor = Color.Black;
            // 
            // frmListInventoryAdjustments
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(gbCommands);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListInventoryAdjustments";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تسويات المخزون";
            Load += frmListInventoryAdjustments_Load;
            contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAdjustments).EndInit();
            pnlHeader.ResumeLayout(false);
            gbCommands.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlGrid.ResumeLayout(false);
            pnlSummary.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        #region Fields

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem tsmiView;
        private ToolStripMenuItem tsmiPost;
        private ToolStripMenuItem tsmiCancel;
        private DataGridView dgvAdjustments;
        private Panel pnlHeader;
        private Label lblMode;
        private GroupBox gbCommands;
        private Panel pnlToolbar;
        private Button btnRefresh;
        private Button btnCancelAdjustment;
        private Button btnPost;
        private Button btnView;
        private Button btnAdd;
        private Button btnClose;
        private Panel pnlFilters;
        private GroupBox gbFilter;
        private Button btnToggleFilters;
        private Label label2;
        private ComboBox cbFilterBy;
        private TextBox txtFilterValue;
        private Label label4;
        private DateTimePicker dtpTo;
        private Label label3;
        private DateTimePicker dtpFrom;
        private Label label1;
        private ComboBox cbStatus;
        private Button btnRangeMonth;
        private Button btnRange30;
        private Button btnRangeToday;
        private CheckBox chkIncludeCancelled;
        private CheckBox chkOnlyCancelled;
        private Button btnClearFilters;
        private GroupBox pnlGrid;
        private Label lblEmpty;
        private GroupBox pnlSummary;
        private Label lblSummary;
        private Label lblRecordsCount;
        private ToolTip toolTip1;

        #endregion
    }
}
