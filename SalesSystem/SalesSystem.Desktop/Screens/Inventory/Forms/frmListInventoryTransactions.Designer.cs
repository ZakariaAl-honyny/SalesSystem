namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmListInventoryTransactions
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose(); base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            contextMenuStrip = new ContextMenuStrip(components);
            tsmiView = new ToolStripMenuItem();
            dgvTransactions = new DataGridView();
            lblMode = new Label();
            pnlHeader = new Panel();
            gbCommands = new GroupBox();
            pnlToolbar = new Panel();
            btnRefresh = new Button();
            btnClose = new Button();
            pnlFilters = new Panel();
            gbFilter = new GroupBox();
            label2 = new Label();
            cbFilterBy = new ComboBox();
            txtFilterValue = new TextBox();
            label1 = new Label();
            cbMovementType = new ComboBox();
            label3 = new Label();
            dtpFrom = new DateTimePicker();
            label4 = new Label();
            dtpTo = new DateTimePicker();
            btnRangeToday = new Button();
            btnRangeMonth = new Button();
            btnRange30 = new Button();
            chkOnlyIn = new CheckBox();
            chkOnlyOut = new CheckBox();
            btnClearFilters = new Button();
            btnToggleFilters = new Button();
            pnlGrid = new GroupBox();
            lblEmpty = new Label();
            pnlSummary = new GroupBox();
            lblSummary = new Label();
            lblRecordsCount = new Label();
            toolTip1 = new ToolTip(components);
            contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
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
            contextMenuStrip.ImageScalingSize = new Size(24, 24);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { tsmiView });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.RightToLeft = RightToLeft.Yes;
            contextMenuStrip.Size = new Size(198, 36);
            // 
            // tsmiView
            // 
            tsmiView.Name = "tsmiView";
            tsmiView.Size = new Size(197, 32);
            tsmiView.Text = "عرض التفاصيل";
            tsmiView.Click += tsmiView_Click;
            // 
            // dgvTransactions
            // 
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 249, 250);
            dgvTransactions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTransactions.BackgroundColor = Color.White;
            dgvTransactions.BorderStyle = BorderStyle.None;
            dgvTransactions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvTransactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTransactions.ColumnHeadersHeight = 45;
            dgvTransactions.ContextMenuStrip = contextMenuStrip;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 243, 205);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTransactions.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTransactions.Dock = DockStyle.Fill;
            dgvTransactions.EnableHeadersVisualStyles = false;
            dgvTransactions.GridColor = Color.FromArgb(222, 226, 230);
            dgvTransactions.Location = new Point(6, 31);
            dgvTransactions.Margin = new Padding(0);
            dgvTransactions.MultiSelect = false;
            dgvTransactions.Name = "dgvTransactions";
            dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.RowHeadersWidth = 62;
            dgvTransactions.RowTemplate.Height = 32;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.Size = new Size(1552, 444);
            dgvTransactions.TabIndex = 0;
            dgvTransactions.TabStop = false;
            dgvTransactions.CellDoubleClick += dgvTransactions_CellDoubleClick;
            dgvTransactions.MouseDown += dgvTransactions_MouseDown;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(16, 150, 96);
            lblMode.Dock = DockStyle.Fill;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(4, 4);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1556, 48);
            lblMode.TabIndex = 0;
            lblMode.Text = "حركات المخزون";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(16, 150, 96);
            pnlHeader.Controls.Add(lblMode);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(4);
            pnlHeader.Size = new Size(1564, 56);
            pnlHeader.TabIndex = 0;
            // 
            // gbCommands
            // 
            gbCommands.BackColor = Color.White;
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
            pnlToolbar.BackColor = Color.White;
            pnlToolbar.Controls.Add(btnRefresh);
            pnlToolbar.Controls.Add(btnClose);
            pnlToolbar.Dock = DockStyle.Fill;
            pnlToolbar.Location = new Point(3, 28);
            pnlToolbar.Margin = new Padding(0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1558, 51);
            pnlToolbar.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(16, 150, 96);
            btnRefresh.FlatAppearance.BorderSize = 2;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(1412, 5);
            btnRefresh.Margin = new Padding(4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(130, 46);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(16, 150, 96);
            btnClose.FlatAppearance.BorderSize = 2;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(1268, 5);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(130, 46);
            btnClose.TabIndex = 1;
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
            pnlFilters.Location = new Point(0, 138);
            pnlFilters.Margin = new Padding(0);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(6, 4, 6, 4);
            pnlFilters.Size = new Size(1564, 112);
            pnlFilters.TabIndex = 2;
            // 
            // gbFilter
            // 
            gbFilter.BackColor = Color.White;
            gbFilter.Controls.Add(label2);
            gbFilter.Controls.Add(cbFilterBy);
            gbFilter.Controls.Add(txtFilterValue);
            gbFilter.Controls.Add(label1);
            gbFilter.Controls.Add(cbMovementType);
            gbFilter.Controls.Add(label3);
            gbFilter.Controls.Add(dtpFrom);
            gbFilter.Controls.Add(label4);
            gbFilter.Controls.Add(dtpTo);
            gbFilter.Controls.Add(btnRangeToday);
            gbFilter.Controls.Add(btnRangeMonth);
            gbFilter.Controls.Add(btnRange30);
            gbFilter.Controls.Add(chkOnlyIn);
            gbFilter.Controls.Add(chkOnlyOut);
            gbFilter.Controls.Add(btnClearFilters);
            gbFilter.Dock = DockStyle.Fill;
            gbFilter.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            gbFilter.Location = new Point(146, 4);
            gbFilter.Margin = new Padding(0);
            gbFilter.Name = "gbFilter";
            gbFilter.Padding = new Padding(8);
            gbFilter.RightToLeft = RightToLeft.Yes;
            gbFilter.Size = new Size(1412, 104);
            gbFilter.TabIndex = 0;
            gbFilter.TabStop = false;
            gbFilter.Text = "بحث وتصفية";
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label2.Location = new Point(1295, 26);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(85, 26);
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
            cbFilterBy.Items.AddRange(new object[] { "الصنف", "المستودع", "نوع الحركة", "رقم المرجع", "البيان" });
            cbFilterBy.Location = new Point(1119, 26);
            cbFilterBy.Margin = new Padding(0);
            cbFilterBy.Name = "cbFilterBy";
            cbFilterBy.Size = new Size(172, 30);
            cbFilterBy.TabIndex = 1;
            cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            // 
            // txtFilterValue
            // 
            txtFilterValue.BackColor = Color.White;
            txtFilterValue.Font = new Font("Tahoma", 9F);
            txtFilterValue.Location = new Point(891, 26);
            txtFilterValue.Margin = new Padding(0);
            txtFilterValue.Name = "txtFilterValue";
            txtFilterValue.RightToLeft = RightToLeft.Yes;
            txtFilterValue.Size = new Size(224, 29);
            txtFilterValue.TabIndex = 2;
            txtFilterValue.TextChanged += txtFilterValue_TextChanged;
            txtFilterValue.KeyPress += txtFilterValue_KeyPress;
            // 
            // label1
            // 
            label1.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label1.Location = new Point(793, 26);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(94, 26);
            label1.TabIndex = 3;
            label1.Text = "نوع الحركة:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbMovementType
            // 
            cbMovementType.BackColor = Color.White;
            cbMovementType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMovementType.FlatStyle = FlatStyle.Flat;
            cbMovementType.Font = new Font("Tahoma", 9F);
            cbMovementType.FormattingEnabled = true;
            cbMovementType.Items.AddRange(new object[] { "الكل", "مشتريات", "مرتجع مشتريات", "مبيعات", "مرتجع مبيعات", "تحويل خارج", "تحويل داخل", "جرد", "تسوية", "تلف", "رصيد افتتاحي", "صرف داخلي", "استلام داخلي" });
            cbMovementType.Location = new Point(639, 26);
            cbMovementType.Margin = new Padding(0);
            cbMovementType.Name = "cbMovementType";
            cbMovementType.Size = new Size(150, 30);
            cbMovementType.TabIndex = 4;
            cbMovementType.SelectedIndexChanged += cbMovementType_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label3.Location = new Point(587, 26);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(48, 26);
            label3.TabIndex = 5;
            label3.Text = "من:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            dtpFrom.BackColor = Color.White;
            dtpFrom.Font = new Font("Tahoma", 9F);
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(453, 26);
            dtpFrom.Margin = new Padding(0);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(130, 29);
            dtpFrom.TabIndex = 6;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // label4
            // 
            label4.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            label4.Location = new Point(401, 26);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(48, 26);
            label4.TabIndex = 7;
            label4.Text = "إلى:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            dtpTo.BackColor = Color.White;
            dtpTo.Font = new Font("Tahoma", 9F);
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(267, 26);
            dtpTo.Margin = new Padding(0);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(130, 29);
            dtpTo.TabIndex = 8;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // btnRangeToday
            // 
            btnRangeToday.BackColor = Color.White;
            btnRangeToday.FlatStyle = FlatStyle.Popup;
            btnRangeToday.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeToday.Location = new Point(1280, 62);
            btnRangeToday.Margin = new Padding(0);
            btnRangeToday.Name = "btnRangeToday";
            btnRangeToday.Size = new Size(100, 30);
            btnRangeToday.TabIndex = 9;
            btnRangeToday.Text = "اليوم";
            btnRangeToday.UseVisualStyleBackColor = false;
            btnRangeToday.Click += btnRangeToday_Click;
            // 
            // btnRangeMonth
            // 
            btnRangeMonth.BackColor = Color.White;
            btnRangeMonth.FlatStyle = FlatStyle.Popup;
            btnRangeMonth.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRangeMonth.Location = new Point(1166, 62);
            btnRangeMonth.Margin = new Padding(0);
            btnRangeMonth.Name = "btnRangeMonth";
            btnRangeMonth.Size = new Size(110, 30);
            btnRangeMonth.TabIndex = 10;
            btnRangeMonth.Text = "هذا الشهر";
            btnRangeMonth.UseVisualStyleBackColor = false;
            btnRangeMonth.Click += btnRangeMonth_Click;
            // 
            // btnRange30
            // 
            btnRange30.BackColor = Color.White;
            btnRange30.FlatStyle = FlatStyle.Popup;
            btnRange30.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            btnRange30.Location = new Point(1037, 62);
            btnRange30.Margin = new Padding(0);
            btnRange30.Name = "btnRange30";
            btnRange30.Size = new Size(125, 30);
            btnRange30.TabIndex = 11;
            btnRange30.Text = "آخر 30 يوماً";
            btnRange30.UseVisualStyleBackColor = false;
            btnRange30.Click += btnRange30_Click;
            // 
            // chkOnlyIn
            // 
            chkOnlyIn.Font = new Font("Tahoma", 9F);
            chkOnlyIn.Location = new Point(899, 64);
            chkOnlyIn.Margin = new Padding(0);
            chkOnlyIn.Name = "chkOnlyIn";
            chkOnlyIn.Size = new Size(134, 26);
            chkOnlyIn.TabIndex = 12;
            chkOnlyIn.Text = "الوارد فقط";
            chkOnlyIn.UseVisualStyleBackColor = false;
            chkOnlyIn.CheckedChanged += chkOnlyIn_CheckedChanged;
            // 
            // chkOnlyOut
            // 
            chkOnlyOut.Font = new Font("Tahoma", 9F);
            chkOnlyOut.Location = new Point(761, 64);
            chkOnlyOut.Margin = new Padding(0);
            chkOnlyOut.Name = "chkOnlyOut";
            chkOnlyOut.Size = new Size(134, 26);
            chkOnlyOut.TabIndex = 13;
            chkOnlyOut.Text = "الصادر فقط";
            chkOnlyOut.UseVisualStyleBackColor = false;
            chkOnlyOut.CheckedChanged += chkOnlyOut_CheckedChanged;
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
            btnClearFilters.Location = new Point(608, 62);
            btnClearFilters.Margin = new Padding(0);
            btnClearFilters.Name = "btnClearFilters";
            btnClearFilters.Size = new Size(145, 32);
            btnClearFilters.TabIndex = 21;
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
            btnToggleFilters.TabIndex = 20;
            btnToggleFilters.Text = "إخفاء التصفية";
            btnToggleFilters.UseVisualStyleBackColor = false;
            // 
            // pnlGrid
            // 
            pnlGrid.BackColor = Color.White;
            pnlGrid.Controls.Add(dgvTransactions);
            pnlGrid.Controls.Add(lblEmpty);
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
            lblEmpty.ForeColor = Color.FromArgb(33, 37, 41);
            lblEmpty.Location = new Point(6, 31);
            lblEmpty.Margin = new Padding(0);
            lblEmpty.Name = "lblEmpty";
            lblEmpty.Size = new Size(1552, 444);
            lblEmpty.TabIndex = 1;
            lblEmpty.Text = "لا توجد بيانات للعرض";
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Visible = false;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.White;
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
            lblSummary.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(33, 37, 41);
            lblSummary.Location = new Point(8, 15);
            lblSummary.Margin = new Padding(0);
            lblSummary.Name = "lblSummary";
            lblSummary.Padding = new Padding(8, 0, 8, 0);
            lblSummary.Size = new Size(1452, 33);
            lblSummary.TabIndex = 0;
            lblSummary.Text = "المعروض: 0 من 0 — وارد: 0.000 · صادر: 0.000";
            lblSummary.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRecordsCount
            // 
            lblRecordsCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            lblRecordsCount.ForeColor = Color.FromArgb(33, 37, 41);
            lblRecordsCount.Location = new Point(1460, 15);
            lblRecordsCount.Margin = new Padding(0);
            lblRecordsCount.Name = "lblRecordsCount";
            lblRecordsCount.Size = new Size(96, 33);
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
            // frmListInventoryTransactions
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(1564, 783);
            Controls.Add(pnlGrid);
            Controls.Add(pnlSummary);
            Controls.Add(pnlFilters);
            Controls.Add(gbCommands);
            Controls.Add(pnlHeader);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListInventoryTransactions";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "حركات المخزون";
            Load += frmListInventoryTransactions_Load;
            contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
            pnlHeader.ResumeLayout(false);
            gbCommands.ResumeLayout(false);
            pnlToolbar.ResumeLayout(false);
            pnlFilters.ResumeLayout(false);
            gbFilter.ResumeLayout(false);
            gbFilter.PerformLayout();
            pnlGrid.ResumeLayout(false);
            pnlSummary.ResumeLayout(false);
            ResumeLayout(false);
        }

        #region Fields
        private Label lblMode;
        private Panel pnlHeader;
        private GroupBox gbCommands;
        private Panel pnlToolbar;
        private Button btnRefresh;
        private Button btnClose;
        private Panel pnlFilters;
        private GroupBox gbFilter;
        private Button btnToggleFilters;
        private Label label2;
        private ComboBox cbFilterBy;
        private TextBox txtFilterValue;
        private Label label1;
        private ComboBox cbMovementType;
        private Label label3;
        private DateTimePicker dtpFrom;
        private Label label4;
        private DateTimePicker dtpTo;
        private Button btnRangeToday;
        private Button btnRangeMonth;
        private Button btnRange30;
        private CheckBox chkOnlyIn;
        private CheckBox chkOnlyOut;
        private Button btnClearFilters;
        private GroupBox pnlGrid;
        private Label lblEmpty;
        private GroupBox pnlSummary;
        private Label lblSummary;
        private Label lblRecordsCount;
        private ToolTip toolTip1;
        private DataGridView dgvTransactions;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem tsmiView;
        #endregion
    }
}
