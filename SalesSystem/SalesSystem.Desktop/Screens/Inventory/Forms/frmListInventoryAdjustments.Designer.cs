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
            pnlToolbar = new Panel();
            btnRefresh = new Button();
            btnCancelAdjustment = new Button();
            btnPost = new Button();
            btnView = new Button();
            btnAdd = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
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
            pnlGrid = new Panel();
            pnlSummary = new Panel();
            lblSummary = new Label();
            lblRecordsCount = new Label();
            toolTip1 = new ToolTip(components);
            contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAdjustments).BeginInit();
            pnlHeader.SuspendLayout();
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
            dgvAdjustments.AllowUserToAddRows = false;
            dgvAdjustments.AllowUserToDeleteRows = false;
            dgvAdjustments.AllowUserToOrderColumns = true;
            dgvAdjustments.AutoGenerateColumns = false;
            dgvAdjustments.BackgroundColor = SystemColors.Window;
            dgvAdjustments.BorderStyle = BorderStyle.Fixed3D;
            dgvAdjustments.ColumnHeadersHeight = 50;
            dgvAdjustments.Dock = DockStyle.Fill;
            dgvAdjustments.Location = new Point(6, 6);
            dgvAdjustments.Margin = new Padding(0);
            dgvAdjustments.MultiSelect = false;
            dgvAdjustments.Name = "dgvAdjustments";
            dgvAdjustments.ReadOnly = true;
            dgvAdjustments.RowHeadersWidth = 62;
            dgvAdjustments.RowTemplate.Height = 28;
            dgvAdjustments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAdjustments.Size = new Size(1552, 517);
            dgvAdjustments.TabIndex = 0;
            dgvAdjustments.TabStop = false;
            dgvAdjustments.CellDoubleClick += dgvAdjustments_CellDoubleClick;
            dgvAdjustments.MouseDown += dgvAdjustments_MouseDown;
            dgvAdjustments.SelectionChanged += dgvAdjustments_SelectionChanged;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.WhiteSmoke;
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
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(4, 4);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1556, 48);
            lblMode.TabIndex = 0;
            lblMode.Text = "تسويات المخزون";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.WhiteSmoke;
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnCancelAdjustment);
            pnlToolbar.Controls.Add(btnPost);
            pnlToolbar.Controls.Add(btnView);
            pnlToolbar.Controls.Add(btnAdd);
            pnlToolbar.Controls.Add(btnClose);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 56);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1564, 60);
            pnlToolbar.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(802, 7);
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
            btnCancelAdjustment.Location = new Point(932, 7);
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
            btnPost.Location = new Point(1062, 7);
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
            btnView.Location = new Point(1192, 7);
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
            btnAdd.Location = new Point(1322, 7);
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
            btnClose.Location = new Point(1452, 7);
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
            pnlFilters.BackColor = Color.WhiteSmoke;
            pnlFilters.Controls.Add(gbFilter);
            pnlFilters.Dock = DockStyle.Top;
            pnlFilters.Location = new Point(0, 116);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1564, 112);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
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
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(6, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1552, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.Location = new Point(868, 28);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(85, 26);
            label2.TabIndex = 6;
            label2.Text = "تصفية بـ:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbFilterBy
            // 
            cbFilterBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFilterBy.FlatStyle = FlatStyle.Flat;
            cbFilterBy.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbFilterBy.FormattingEnabled = true;
            cbFilterBy.Items.AddRange(new object[] { "لا شيء", "رقم التسوية", "المستودع", "البيان" });
            cbFilterBy.Location = new Point(698, 27);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(160, 26);
            cbFilterBy.TabIndex = 7;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            txtFilterValue.Location = new Point(470, 27);
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
            label4.Location = new Point(1143, 28);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(48, 26);
            label4.TabIndex = 4;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(1005, 27);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(130, 26);
            dtpTo.TabIndex = 5;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.Location = new Point(951, 28);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(48, 26);
            label3.TabIndex = 2;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(813, 27);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(130, 26);
            dtpFrom.TabIndex = 3;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label1
            // 
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label1.Location = new Point(1455, 28);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(70, 26);
            label1.TabIndex = 0;
            label1.Text = "الحالة:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.Flat;
            cbStatus.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            cbStatus.FormattingEnabled = true;
            cbStatus.Items.AddRange(new object[] { "الكل", "مسودة", "مرحّل", "ملغى" });
            cbStatus.Location = new Point(1327, 27);
            cbStatus.Margin = new Padding(0);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(120, 26);
            cbStatus.TabIndex = 1;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.FlatStyle = FlatStyle.Popup;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.Location = new Point(1255, 64);
            btnRangeMonth.Margin = new Padding(0);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(110, 30);
            btnRangeMonth.TabIndex = 9;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = true;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.FlatStyle = FlatStyle.Popup;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.Location = new Point(1375, 64);
            btnRange30.Margin = new Padding(0);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(125, 30);
            btnRange30.TabIndex = 10;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = true;
            btnRange30.Click += btnRange30_Click;
            // 
            // btnRangeToday
            // 
            btnRangeToday.FlatStyle = FlatStyle.Popup;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.Location = new Point(1455, 64);
            btnRangeToday.Margin = new Padding(0);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(70, 30);
            btnRangeToday.TabIndex = 11;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = true;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // chkIncludeCancelled
            // 
            chkIncludeCancelled.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkIncludeCancelled.Location = new Point(1089, 66);
            chkIncludeCancelled.Margin = new Padding(0);
            chkIncludeCancelled.Name = "chkIncludeCancelled";
            chkIncludeCancelled.Size = new Size(150, 26);
            chkIncludeCancelled.TabIndex = 12;
            chkIncludeCancelled.Text = "عرض الملغاة";
            chkIncludeCancelled.UseVisualStyleBackColor = true;
            chkIncludeCancelled.CheckedChanged += chkIncludeCancelled_CheckedChanged;
            // 
            // chkOnlyCancelled
            // 
            chkOnlyCancelled.Font = new Font("Tahoma", 9F, FontStyle.Regular);
            chkOnlyCancelled.Location = new Point(934, 66);
            chkOnlyCancelled.Margin = new Padding(0);
            chkOnlyCancelled.Name = "chkOnlyCancelled";
            chkOnlyCancelled.Size = new Size(140, 26);
            chkOnlyCancelled.TabIndex = 13;
            chkOnlyCancelled.Text = "الملغاة فقط";
            chkOnlyCancelled.UseVisualStyleBackColor = true;
            chkOnlyCancelled.CheckedChanged += chkOnlyCancelled_CheckedChanged;
            // 
            // pnlGrid
            // 
            pnlGrid.Controls.Add(dgvAdjustments);
            pnlGrid.Dock = DockStyle.Fill;
            pnlGrid.Location = new Point(0, 228);
            pnlGrid.Margin = new Padding(0);
            pnlGrid.Name = "pnlGrid";
            pnlGrid.Padding = new Padding(6);
            pnlGrid.Size = new Size(1564, 523);
            pnlGrid.TabIndex = 3;
            // 
            // pnlSummary
            // 
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Controls.Add(lblRecordsCount);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 751);
            pnlSummary.Margin = new Padding(0);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1564, 32);
            pnlSummary.TabIndex = 4;
            // 
            // lblSummary
            // 
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.Location = new Point(90, 0);
            lblSummary.Margin = new Padding(0);
            lblSummary.Name = "lblSummary";
            lblSummary.Padding = new Padding(8, 0, 8, 0);
            lblSummary.Size = new Size(1474, 32);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — مسودات: 0 · مُرحَّلة: 0 · ملغاة: 0";
            lblSummary.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Dock = DockStyle.Right;
            lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblRecordsCount.Location = new Point(1474, 0);
            lblRecordsCount.Margin = new Padding(0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(90, 32);
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
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(pnlToolbar);
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
        private Panel pnlToolbar;
        private Button btnRefresh;
        private Button btnCancelAdjustment;
        private Button btnPost;
        private Button btnView;
        private Button btnAdd;
        private Button btnClose;
        private Panel pnlFilters;
        private GroupBox gbFilter;
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
        private Panel pnlGrid;
        private Panel pnlSummary;
        private Label lblSummary;
        private Label lblRecordsCount;
        private ToolTip toolTip1;

        #endregion
    }
}
