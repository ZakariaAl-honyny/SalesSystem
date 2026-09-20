namespace SalesSystem.Desktop.Screens.Inventory.Forms
{
    partial class frmAddUpdateInventoryAdjustment
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            errorProvider = new ErrorProvider(components);
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            lblMode = new Label();
            groupHeader = new GroupBox();
            lblAdjustmentNo = new Label();
            txtAdjustmentNo = new TextBox();
            lblWarehouse = new Label();
            cmbWarehouse = new ComboBox();
            lblAdjustmentType = new Label();
            cmbAdjustmentType = new ComboBox();
            lblReason = new Label();
            txtReason = new TextBox();
            groupLines = new GroupBox();
            dgvLines = new DataGridView();
            lblLinesCount = new Label();
            label1 = new Label();
            btnSaveDraft = new Button();
            btnPost = new Button();
            btnClose = new Button();

            groupHeader.SuspendLayout();
            groupLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(620, 15);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(260, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "تسوية مخزون";

            // groupHeader
            groupHeader.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            groupHeader.Location = new Point(12, 65);
            groupHeader.Name = "groupHeader";
            groupHeader.RightToLeft = RightToLeft.Yes;
            groupHeader.Size = new Size(1540, 120);
            groupHeader.TabIndex = 0;
            groupHeader.TabStop = false;
            groupHeader.Text = "معلومات التسوية";

            // lblAdjustmentNo
            lblAdjustmentNo.AutoSize = true;
            lblAdjustmentNo.Font = new Font("Tahoma", 10F);
            lblAdjustmentNo.Location = new Point(1370, 30);
            lblAdjustmentNo.Name = "lblAdjustmentNo";
            lblAdjustmentNo.Size = new Size(120, 24);
            lblAdjustmentNo.TabIndex = 0;
            lblAdjustmentNo.Text = "رقم المستند:";

            // txtAdjustmentNo
            txtAdjustmentNo.Font = new Font("Tahoma", 10F);
            txtAdjustmentNo.Location = new Point(1190, 27);
            txtAdjustmentNo.Name = "txtAdjustmentNo";
            txtAdjustmentNo.ReadOnly = true;
            txtAdjustmentNo.Size = new Size(150, 32);
            txtAdjustmentNo.TabIndex = 1;

            // lblWarehouse
            lblWarehouse.AutoSize = true;
            lblWarehouse.Font = new Font("Tahoma", 10F);
            lblWarehouse.Location = new Point(1050, 30);
            lblWarehouse.Name = "lblWarehouse";
            lblWarehouse.Size = new Size(100, 24);
            lblWarehouse.TabIndex = 2;
            lblWarehouse.Text = "المستودع:";

            // cmbWarehouse
            cmbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouse.Font = new Font("Tahoma", 10F);
            cmbWarehouse.Location = new Point(820, 27);
            cmbWarehouse.Name = "cmbWarehouse";
            cmbWarehouse.Size = new Size(200, 32);
            cmbWarehouse.TabIndex = 3;

            // lblAdjustmentType
            lblAdjustmentType.AutoSize = true;
            lblAdjustmentType.Font = new Font("Tahoma", 10F);
            lblAdjustmentType.Location = new Point(680, 30);
            lblAdjustmentType.Name = "lblAdjustmentType";
            lblAdjustmentType.Size = new Size(110, 24);
            lblAdjustmentType.TabIndex = 4;
            lblAdjustmentType.Text = "نوع التسوية:";

            // cmbAdjustmentType
            cmbAdjustmentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAdjustmentType.Font = new Font("Tahoma", 10F);
            cmbAdjustmentType.Location = new Point(450, 27);
            cmbAdjustmentType.Name = "cmbAdjustmentType";
            cmbAdjustmentType.Size = new Size(200, 32);
            cmbAdjustmentType.TabIndex = 5;

            // lblReason
            lblReason.AutoSize = true;
            lblReason.Font = new Font("Tahoma", 10F);
            lblReason.Location = new Point(340, 75);
            lblReason.Name = "lblReason";
            lblReason.Size = new Size(70, 24);
            lblReason.TabIndex = 6;
            lblReason.Text = "السبب:";

            // txtReason
            txtReason.Font = new Font("Tahoma", 10F);
            txtReason.Location = new Point(15, 72);
            txtReason.Name = "txtReason";
            txtReason.Size = new Size(310, 32);
            txtReason.TabIndex = 7;

            // groupLines
            groupLines.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            groupLines.Location = new Point(12, 195);
            groupLines.Name = "groupLines";
            groupLines.RightToLeft = RightToLeft.Yes;
            groupLines.Size = new Size(1540, 470);
            groupLines.TabIndex = 1;
            groupLines.TabStop = false;
            groupLines.Text = "البنود";

            // dgvLines
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AllowUserToOrderColumns = true;
            dgvLines.BackgroundColor = SystemColors.Window;
            dgvLines.BorderStyle = BorderStyle.Fixed3D;
            dgvLines.ColumnHeadersHeight = 50;
            dgvLines.Location = new Point(10, 35);
            dgvLines.MultiSelect = false;
            dgvLines.Name = "dgvLines";
            dgvLines.ReadOnly = true;
            dgvLines.RowHeadersWidth = 62;
            dgvLines.RowTemplate.Height = 28;
            dgvLines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLines.Size = new Size(1515, 400);
            dgvLines.TabIndex = 0;

            // lblLinesCount
            lblLinesCount.AutoSize = true;
            lblLinesCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblLinesCount.Location = new Point(60, 440);
            lblLinesCount.Name = "lblLinesCount";
            lblLinesCount.Size = new Size(28, 24);
            lblLinesCount.Text = "0";

            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(10, 440);
            label1.Name = "label1";
            label1.Size = new Size(54, 24);
            label1.Text = "عدد البنود:";

            // btnSaveDraft
            btnSaveDraft.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSaveDraft.BackColor = Color.White;
            btnSaveDraft.FlatStyle = FlatStyle.Popup;
            btnSaveDraft.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveDraft.Location = new Point(140, 710);
            btnSaveDraft.Name = "btnSaveDraft";
            btnSaveDraft.Size = new Size(140, 50);
            btnSaveDraft.TabIndex = 10;
            btnSaveDraft.Text = "حفظ مسودة";
            btnSaveDraft.UseVisualStyleBackColor = false;
            btnSaveDraft.Click += btnSaveDraft_Click;

            // btnPost
            btnPost.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPost.BackColor = Color.White;
            btnPost.FlatStyle = FlatStyle.Popup;
            btnPost.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPost.Location = new Point(290, 710);
            btnPost.Name = "btnPost";
            btnPost.Size = new Size(140, 50);
            btnPost.TabIndex = 11;
            btnPost.Text = "ترحيل";
            btnPost.UseVisualStyleBackColor = false;
            btnPost.Click += btnPost_Click;

            // btnClose
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 710);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(120, 50);
            btnClose.TabIndex = 12;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // frmInventoryAdjustmentEditor
            AcceptButton = btnPost;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1564, 783);
            Controls.Add(btnPost);
            Controls.Add(btnSaveDraft);
            Controls.Add(btnClose);
            Controls.Add(lblMode);
            Controls.Add(groupHeader);
            Controls.Add(groupLines);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmInventoryAdjustmentEditor";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تسوية مخزون جديدة";
            Load += frmInventoryAdjustmentEditor_Load;

            groupHeader.ResumeLayout(false);
            groupHeader.PerformLayout();
            groupLines.ResumeLayout(false);
            groupLines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #region Fields
        private Label lblMode;
        private GroupBox groupHeader;
        private Label lblAdjustmentNo;
        private TextBox txtAdjustmentNo;
        private Label lblWarehouse;
        private ComboBox cmbWarehouse;
        private Label lblAdjustmentType;
        private ComboBox cmbAdjustmentType;
        private Label lblReason;
        private TextBox txtReason;
        private GroupBox groupLines;
        private DataGridView dgvLines;
        private Label lblLinesCount;
        private Label label1;
        private Button btnSaveDraft;
        private Button btnPost;
        private Button btnClose;
        private ErrorProvider errorProvider;
        #endregion
    }
}
