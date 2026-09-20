namespace SalesSystem.Desktop.Controls.Users
{
    partial class frmDocumentSequences
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
            lblMode = new Label();
            btnRefresh = new Button();
            btnClose = new Button();
            btnSaveAll = new Button();
            lblRecordCount = new Label();
            dgvSequences = new DataGridView();
            txtSearch = new TextBox();
            lblSearch = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvSequences).BeginInit();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(260, 15);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(276, 39);
            lblMode.TabIndex = 0;
            lblMode.Text = "سلاسل المستندات";
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.Location = new Point(698, 94);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 30);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(31, 436);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnSaveAll
            // 
            btnSaveAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveAll.BackColor = Color.White;
            btnSaveAll.FlatStyle = FlatStyle.Popup;
            btnSaveAll.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSaveAll.Location = new Point(640, 450);
            btnSaveAll.Name = "btnSaveAll";
            btnSaveAll.Size = new Size(140, 40);
            btnSaveAll.TabIndex = 3;
            btnSaveAll.Text = "حفظ الكل";
            btnSaveAll.UseVisualStyleBackColor = false;
            btnSaveAll.Click += btnSaveAll_Click;
            // 
            // lblRecordCount
            // 
            lblRecordCount.AutoSize = true;
            lblRecordCount.Location = new Point(20, 100);
            lblRecordCount.Name = "lblRecordCount";
            lblRecordCount.Size = new Size(0, 29);
            lblRecordCount.TabIndex = 4;
            // 
            // dgvSequences
            // 
            dgvSequences.AllowUserToAddRows = false;
            dgvSequences.AllowUserToDeleteRows = false;
            dgvSequences.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSequences.BackgroundColor = Color.White;
            dgvSequences.ColumnHeadersHeight = 50;
            dgvSequences.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvSequences.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvSequences.Location = new Point(12, 130);
            dgvSequences.Name = "dgvSequences";
            dgvSequences.RowHeadersWidth = 51;
            dgvSequences.RowTemplate.Height = 28;
            dgvSequences.Size = new Size(776, 300);
            dgvSequences.TabIndex = 5;
            dgvSequences.CellEndEdit += dgvSequences_CellEndEdit;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch.Location = new Point(84, 88);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 36);
            txtSearch.TabIndex = 6;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 92);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(66, 29);
            lblSearch.TabIndex = 7;
            lblSearch.Text = "بحث:";
            // 
            // frmDocumentSequences
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(800, 500);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(dgvSequences);
            Controls.Add(lblRecordCount);
            Controls.Add(btnSaveAll);
            Controls.Add(btnClose);
            Controls.Add(btnRefresh);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDocumentSequences";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "سلاسل المستندات";
            Load += frmDocumentSequences_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSequences).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.DataGridView dgvSequences;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
    }
}
