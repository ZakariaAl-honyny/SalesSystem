namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmAddUpdateInventoryAdjustment
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            errorProvider = new ErrorProvider(components);
            toolTip1 = new ToolTip(components);
            liveTip = new ToolTip(components);
            cmsLines = new ContextMenuStrip(components);
            miCopyCell = new ToolStripMenuItem();
            miRefresh = new ToolStripMenuItem();
            lblMode = new Label();
            gbToolbar = new GroupBox();
            btnSaveDraft = new Button();
            btnPost = new Button();
            btnClose = new Button();
            groupHeader = new GroupBox();
            lblAdjustmentNo = new Label();
            txtAdjustmentNo = new TextBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblAdjustmentType = new Label();
            cmbAdjustmentType = new ComboBox();
            lblReason = new Label();
            txtReason = new TextBox();
            gbTotals = new GroupBox();
            label1 = new Label();
            lblLinesCount = new Label();
            lblTotalsDiffTitle = new Label();
            lblTotalsDiff = new Label();
            lblTotalsValueTitle = new Label();
            txtTotalsValue = new TextBox();
            groupLines = new GroupBox();
            dgvLines = new DataGridView();
            lblEmpty = new Label();

            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            cmsLines.SuspendLayout();
            gbToolbar.SuspendLayout();
            groupHeader.SuspendLayout();
            gbTotals.SuspendLayout();
            groupLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();

            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.RightToLeft = true;
            // 
            // toolTip1
            // 
            toolTip1.AutomaticDelay = 300;
            toolTip1.BackColor = Color.White;
            toolTip1.ForeColor = Color.Black;
            // 
            // liveTip
            // 
            liveTip.AutomaticDelay = 300;
            liveTip.BackColor = Color.White;
            liveTip.ForeColor = Color.Black;
            // 
            // cmsLines
            // 
            cmsLines.Items.AddRange(new ToolStripItem[] { miCopyCell, miRefresh });
            cmsLines.Name = "cmsLines";
            cmsLines.RightToLeft = RightToLeft.Yes;
            cmsLines.Size = new Size(210, 56);
            // 
            // miCopyCell
            // 
            miCopyCell.Name = "miCopyCell";
            miCopyCell.Size = new Size(209, 26);
            miCopyCell.Text = "نسخ قيمة الخلية";
            miCopyCell.Click += miCopyCell_Click;
            // 
            // miRefresh
            // 
            miRefresh.Name = "miRefresh";
            miRefresh.Size = new Size(209, 26);
            miRefresh.Text = "تحديث البيانات (F5)";
            miRefresh.Click += miRefresh_Click;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(14, 165, 103);
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Margin = new Padding(0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1360, 50);
            lblMode.TabIndex = 0;
            lblMode.Text = "تسوية مخزون";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbToolbar
            // 
            gbToolbar.BackColor = Color.FromArgb(235, 245, 235);
            gbToolbar.Controls.Add(btnSaveDraft);
            gbToolbar.Controls.Add(btnPost);
            gbToolbar.Controls.Add(btnClose);
            gbToolbar.Dock = DockStyle.Top;
            gbToolbar.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbToolbar.ForeColor = Color.FromArgb(14, 82, 52);
            gbToolbar.Location = new Point(0, 50);
            gbToolbar.Margin = new Padding(0);
            gbToolbar.Name = "gbToolbar";
            gbToolbar.Padding = new Padding(8);
            gbToolbar.Size = new Size(1360, 64);
            gbToolbar.TabIndex = 1;
            gbToolbar.TabStop = false;
            gbToolbar.Text = "أوامر التسوية";
            // 
            // btnSaveDraft
            // 
            btnSaveDraft.BackColor = Color.FromArgb(23, 162, 184);
            btnSaveDraft.Cursor = Cursors.Hand;
            btnSaveDraft.FlatAppearance.BorderSize = 0;
            btnSaveDraft.FlatStyle = FlatStyle.Flat;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveDraft.ForeColor = Color.White;
            btnSaveDraft.Location = new Point(1180, 22);
            btnSaveDraft.Name = "btnSaveDraft";
            btnSaveDraft.Size = new Size(140, 34);
            btnSaveDraft.TabIndex = 0;
            btnSaveDraft.Text = "حفظ مسودة";
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;
            // 
            // btnPost
            // 
            btnPost.BackColor = Color.FromArgb(40, 167, 69);
            btnPost.Cursor = Cursors.Hand;
            btnPost.FlatAppearance.BorderSize = 0;
            btnPost.FlatStyle = FlatStyle.Flat;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPost.ForeColor = Color.White;
            btnPost.Location = new Point(1030, 22);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(140, 34);
            btnPost.TabIndex = 1;
            btnPost.Text = "ترحيل (F4)";
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.Cursor = Cursors.Hand;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(890, 22);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 34);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // groupHeader
            // 
            groupHeader.BackColor = Color.FromArgb(235, 245, 235);
            groupHeader.Controls.Add(lblAdjustmentNo);
            groupHeader.Controls.Add(txtAdjustmentNo);
            groupHeader.Controls.Add(lblWarehouse);
            groupHeader.Controls.Add(cmbWarehouse);
            groupHeader.Controls.Add(lblAdjustmentType);
            groupHeader.Controls.Add(cmbAdjustmentType);
            groupHeader.Controls.Add(lblReason);
            groupHeader.Controls.Add(txtReason);
            groupHeader.Dock = DockStyle.Top;
            groupHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            groupHeader.ForeColor = Color.FromArgb(14, 82, 52);
            groupHeader.Location = new Point(0, 114);
            groupHeader.Margin = new Padding(0);
            groupHeader.Name = "groupHeader";
            groupHeader.Padding = new Padding(8);
            groupHeader.RightToLeft = RightToLeft.Yes;
            groupHeader.Size = new Size(1360, 124);
            groupHeader.TabIndex = 2;
            groupHeader.TabStop = false;
            groupHeader.Text = "بيانات التسوية";
            // 
            // lblAdjustmentNo
            // 
            lblAdjustmentNo.AutoSize = true;
            lblAdjustmentNo.Font = new Font("Tahoma", 10F);
            lblAdjustmentNo.ForeColor = Color.FromArgb(14, 82, 52);
            lblAdjustmentNo.Location = new Point(1200, 26);
            lblAdjustmentNo.Name = "lblAdjustmentNo";
            lblAdjustmentNo.Size = new Size(120, 24);
            lblAdjustmentNo.TabIndex = 0;
            lblAdjustmentNo.Text = "رقم المستند:";
            // 
            // txtAdjustmentNo
            // 
            txtAdjustmentNo.BackColor = Color.White;
            txtAdjustmentNo.Font = new Font("Tahoma", 10F);
            txtAdjustmentNo.Location = new Point(1040, 23);
            txtAdjustmentNo.Name = "txtAdjustmentNo";
            txtAdjustmentNo.ReadOnly = true;
            txtAdjustmentNo.Size = new Size(150, 32);
            txtAdjustmentNo.TabIndex = 1;
            txtAdjustmentNo.TabStop = false;
            // 
            // lblWarehouse
            // 
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Tahoma", 10F);
            lblWarehouse.ForeColor = Color.FromArgb(14, 82, 52);
            lblWarehouse.Location = new Point(900, 26);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(100, 24);
            lblWarehouse.TabIndex = 2;
            lblWarehouse.Text = "المستودع:";
            // 
            // cmbWarehouse
            // 
            cmbWarehouse.BackColor = Color.White;
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.FlatStyle = FlatStyle.Flat;
            cmbWarehouse.Font = new Font("Tahoma", 10F);
            cmbWarehouse.FormattingEnabled = true;
            cmbWarehouse.Location = new Point(690, 23);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(200, 32);
            cmbWarehouse.TabIndex = 3;
            // 
            // lblAdjustmentType
            // 
            lblAdjustmentType.AutoSize = true;
            lblAdjustmentType.Font = new Font("Tahoma", 10F);
            lblAdjustmentType.ForeColor = Color.FromArgb(14, 82, 52);
            lblAdjustmentType.Location = new Point(556, 26);
            lblAdjustmentType.Name = "lblAdjustmentType";
            lblAdjustmentType.Size = new Size(120, 24);
            lblAdjustmentType.TabIndex = 4;
            lblAdjustmentType.Text = "نوع التسوية:";
            // 
            // cmbAdjustmentType
            // 
            cmbAdjustmentType.BackColor = Color.White;
            cmbAdjustmentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAdjustmentType.FlatStyle = FlatStyle.Flat;
            cmbAdjustmentType.Font = new Font("Tahoma", 10F);
            cmbAdjustmentType.FormattingEnabled = true;
            cmbAdjustmentType.Location = new Point(346, 23);
            cmbAdjustmentType.Name = "cmbAdjustmentType";
            cmbAdjustmentType.Size = new Size(200, 32);
            cmbAdjustmentType.TabIndex = 5;
            // 
            // lblReason
            // 
            lblReason.AutoSize = true;
            lblReason.Font = new Font("Tahoma", 10F);
            lblReason.ForeColor = Color.FromArgb(14, 82, 52);
            lblReason.Location = new Point(1200, 78);
            lblReason.Name = "lblReason";
            lblReason.Size = new Size(80, 24);
            lblReason.TabIndex = 6;
            lblReason.Text = "السبب:";
            // 
            // txtReason
            // 
            txtReason.BackColor = Color.White;
            txtReason.Font = new Font("Tahoma", 10F);
            txtReason.Location = new Point(860, 75);
            txtReason.Name = "txtReason";
            txtReason.Size = new Size(330, 32);
            txtReason.TabIndex = 7;
            // 
            // gbTotals
            // 
            gbTotals.BackColor = Color.FromArgb(235, 245, 235);
            gbTotals.Controls.Add(label1);
            gbTotals.Controls.Add(lblLinesCount);
            gbTotals.Controls.Add(lblTotalsDiffTitle);
            gbTotals.Controls.Add(lblTotalsDiff);
            gbTotals.Controls.Add(lblTotalsValueTitle);
            gbTotals.Controls.Add(txtTotalsValue);
            gbTotals.Dock = DockStyle.Bottom;
            gbTotals.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbTotals.ForeColor = Color.FromArgb(14, 82, 52);
            gbTotals.Location = new Point(0, 704);
            gbTotals.Margin = new Padding(0);
            gbTotals.Name = "gbTotals";
            gbTotals.Padding = new Padding(8);
            gbTotals.Size = new Size(1360, 76);
            gbTotals.TabIndex = 3;
            gbTotals.TabStop = false;
            gbTotals.Text = "الإجماليات";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(14, 82, 52);
            label1.Location = new Point(1180, 24);
            label1.Name = "label1";
            label1.Size = new Size(110, 24);
            label1.TabIndex = 0;
            label1.Text = "عدد البنود:";
            // 
            // lblLinesCount
            // 
            lblLinesCount.AutoSize = true;
            lblLinesCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblLinesCount.ForeColor = Color.FromArgb(14, 82, 52);
            lblLinesCount.Location = new Point(1100, 24);
            lblLinesCount.Name = "lblLinesCount";
            lblLinesCount.Size = new Size(28, 24);
            lblLinesCount.TabIndex = 1;
            lblLinesCount.Text = "0";
            // 
            // lblTotalsDiffTitle
            // 
            lblTotalsDiffTitle.AutoSize = true;
            lblTotalsDiffTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTotalsDiffTitle.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalsDiffTitle.Location = new Point(900, 24);
            lblTotalsDiffTitle.Name = "lblTotalsDiffTitle";
            lblTotalsDiffTitle.Size = new Size(180, 24);
            lblTotalsDiffTitle.TabIndex = 2;
            lblTotalsDiffTitle.Text = "إجمالي فرق الكمية:";
            // 
            // lblTotalsDiff
            // 
            lblTotalsDiff.AutoSize = true;
            lblTotalsDiff.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTotalsDiff.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalsDiff.Location = new Point(750, 24);
            lblTotalsDiff.Name = "lblTotalsDiff";
            lblTotalsDiff.Size = new Size(60, 24);
            lblTotalsDiff.TabIndex = 3;
            lblTotalsDiff.Text = "0.000";
            // 
            // lblTotalsValueTitle
            // 
            lblTotalsValueTitle.AutoSize = true;
            lblTotalsValueTitle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblTotalsValueTitle.ForeColor = Color.FromArgb(14, 82, 52);
            lblTotalsValueTitle.Location = new Point(600, 24);
            lblTotalsValueTitle.Name = "lblTotalsValueTitle";
            lblTotalsValueTitle.Size = new Size(110, 24);
            lblTotalsValueTitle.TabIndex = 4;
            lblTotalsValueTitle.Text = "قيمة الفرق:";
            // 
            // txtTotalsValue
            // 
            txtTotalsValue.BackColor = Color.White;
            txtTotalsValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtTotalsValue.Location = new Point(420, 21);
            txtTotalsValue.Name = "txtTotalsValue";
            txtTotalsValue.ReadOnly = true;
            txtTotalsValue.Size = new Size(170, 32);
            txtTotalsValue.TabIndex = 5;
            txtTotalsValue.TabStop = true;
            txtTotalsValue.Text = "0.00";
            txtTotalsValue.TextAlign = HorizontalAlignment.Right;
            // 
            // groupLines
            // 
            groupLines.BackColor = Color.FromArgb(235, 245, 235);
            groupLines.Controls.Add(dgvLines);
            groupLines.Controls.Add(lblEmpty);
            groupLines.Dock = DockStyle.Fill;
            groupLines.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            groupLines.ForeColor = Color.FromArgb(14, 82, 52);
            groupLines.Location = new Point(0, 238);
            groupLines.Margin = new Padding(0);
            groupLines.Name = "groupLines";
            groupLines.Padding = new Padding(8);
            groupLines.RightToLeft = RightToLeft.Yes;
            groupLines.Size = new Size(1360, 466);
            groupLines.TabIndex = 4;
            groupLines.TabStop = false;
            groupLines.Text = "أصناف التسوية";
            // 
            // dgvLines
            // 
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AllowUserToOrderColumns = true;
            dgvLines.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 252, 245);
            dgvLines.AutoGenerateColumns = false;
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.BackgroundColor = Color.FromArgb(220, 240, 220);
            dgvLines.BorderStyle = BorderStyle.None;
            dgvLines.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(14, 165, 103);
            dgvLines.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLines.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(14, 165, 103);
            dgvLines.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            dgvLines.ColumnHeadersHeight = 45;
            dgvLines.ContextMenuStrip = cmsLines;
            dgvLines.DefaultCellStyle.BackColor = Color.White;
            dgvLines.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvLines.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 180);
            dgvLines.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvLines.Dock = DockStyle.Fill;
            dgvLines.EnableHeadersVisualStyles = false;
            dgvLines.GridColor = Color.FromArgb(205, 225, 205);
            dgvLines.Location = new Point(8, 31);
            dgvLines.Margin = new Padding(0);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersVisible = false;
            dgvLines.RowHeadersWidth = 62;
            dgvLines.RowTemplate.Height = 32;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(1344, 427);
            dgvLines.TabIndex = 0;
            dgvLines.TabStop = false;
            // 
            // lblEmpty
            // 
            lblEmpty.BackColor = Color.Transparent;
            lblEmpty.Dock = DockStyle.Fill;
            lblEmpty.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            lblEmpty.ForeColor = Color.FromArgb(14, 82, 52);
            lblEmpty.Location = new Point(8, 31);
            lblEmpty.Margin = new Padding(0);
            lblEmpty.Name = "lblEmpty";
            lblEmpty.Size = new Size(1344, 427);
            lblEmpty.TabIndex = 1;
            lblEmpty.Text = "لا توجد بنود في هذه التسوية";
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Visible = false;
            // 
            // frmAddUpdateInventoryAdjustment
            // 
            AcceptButton = null;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            CancelButton = btnClose;
            ClientSize = new Size(1360, 780);
            Controls.Add(groupLines);
            Controls.Add(gbTotals);
            Controls.Add(groupHeader);
            Controls.Add(gbToolbar);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 10F, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmAddUpdateInventoryAdjustment";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تسوية مخزون جديدة";
            Load += frmInventoryAdjustmentEditor_Load;

            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            cmsLines.ResumeLayout(false);
            gbToolbar.ResumeLayout(false);
            groupHeader.ResumeLayout(false);
            groupHeader.PerformLayout();
            gbTotals.ResumeLayout(false);
            gbTotals.PerformLayout();
            groupLines.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
        }

        #region Fields
        private ErrorProvider errorProvider;
        private ToolTip toolTip1;
        private ToolTip liveTip;
        private ContextMenuStrip cmsLines;
        private ToolStripMenuItem miCopyCell;
        private ToolStripMenuItem miRefresh;
        private Label lblMode;
        private GroupBox gbToolbar;
        private Button btnSaveDraft;
        private Button btnPost;
        private Button btnClose;
        private GroupBox groupHeader;
        private Label lblAdjustmentNo;
        private TextBox txtAdjustmentNo;
        private Label lblWarehouse;
        private ComboBox cmbWarehouse;
        private Label lblAdjustmentType;
        private ComboBox cmbAdjustmentType;
        private Label lblReason;
        private TextBox txtReason;
        private GroupBox gbTotals;
        private Label label1;
        private Label lblLinesCount;
        private Label lblTotalsDiffTitle;
        private Label lblTotalsDiff;
        private Label lblTotalsValueTitle;
        private TextBox txtTotalsValue;
        private GroupBox groupLines;
        private DataGridView dgvLines;
        private Label lblEmpty;
        #endregion
    }
}
