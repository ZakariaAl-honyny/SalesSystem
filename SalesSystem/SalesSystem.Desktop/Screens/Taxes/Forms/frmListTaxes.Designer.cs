namespace SalesSystem.Desktop.Screens.Taxes.Forms
{
    partial class frmListTaxes
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
            this.lblMode = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkIncludeInactive = new System.Windows.Forms.CheckBox();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.dgvTaxes = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(505, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(190, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إدارة الضرائب";
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAdd.BackColor = Color.White;
            btnAdd.FlatAppearance.BorderColor = Color.Black;
            btnAdd.FlatAppearance.BorderSize = 2;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnAdd.Location = new Point(1103, 80);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 75);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "+ جديد";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Location = new Point(12, 650);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 61);
            btnClose.TabIndex = 0;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.Black;
            btnRefresh.FlatAppearance.BorderSize = 2;
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnRefresh.Location = new Point(1010, 80);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 75);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "تحديث";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnEdit
            // 
            btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.BackColor = Color.White;
            btnEdit.FlatAppearance.BorderColor = Color.Black;
            btnEdit.FlatAppearance.BorderSize = 2;
            btnEdit.FlatStyle = FlatStyle.Popup;
            btnEdit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnEdit.Location = new Point(920, 80);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(75, 75);
            btnEdit.TabIndex = 9;
            btnEdit.Text = "تعديل";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BackColor = Color.White;
            btnDelete.FlatAppearance.BorderColor = Color.Black;
            btnDelete.FlatAppearance.BorderSize = 2;
            btnDelete.FlatStyle = FlatStyle.Popup;
            btnDelete.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.Red;
            btnDelete.Location = new Point(830, 80);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 75);
            btnDelete.TabIndex = 10;
            btnDelete.Text = "حذف";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // chkIncludeInactive
            // 
            chkIncludeInactive.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            chkIncludeInactive.AutoSize = true;
            chkIncludeInactive.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            chkIncludeInactive.Location = new Point(21, 80);
            chkIncludeInactive.Name = "chkIncludeInactive";
            chkIncludeInactive.Size = new Size(200, 28);
            chkIncludeInactive.TabIndex = 3;
            chkIncludeInactive.Text = "عرض غير النشطة";
            chkIncludeInactive.UseVisualStyleBackColor = true;
            chkIncludeInactive.CheckedChanged += chkIncludeInactive_CheckedChanged;
            // 
            // lblRecordCount
            // 
            lblRecordCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblRecordCount.AutoSize = true;
            lblRecordCount.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblRecordCount.Location = new Point(1100, 665);
            lblRecordCount.Name = "lblRecordCount";
            lblRecordCount.Size = new Size(52, 24);
            lblRecordCount.TabIndex = 7;
            lblRecordCount.Text = "[0]";
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblSearch.Location = new Point(21, 115);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(130, 24);
            lblSearch.TabIndex = 6;
            lblSearch.Text = "البحث عن طريق:";
            // 
            // dgvTaxes
            // 
            dgvTaxes.AllowUserToAddRows = false;
            dgvTaxes.AllowUserToDeleteRows = false;
            dgvTaxes.AllowUserToOrderColumns = true;
            dgvTaxes.BackgroundColor = SystemColors.Window;
            dgvTaxes.BorderStyle = BorderStyle.Fixed3D;
            dgvTaxes.ColumnHeadersHeight = 50;
            dgvTaxes.Location = new Point(12, 160);
            dgvTaxes.MultiSelect = false;
            dgvTaxes.Name = "dgvTaxes";
            dgvTaxes.ReadOnly = true;
            dgvTaxes.RowHeadersWidth = 62;
            dgvTaxes.RowTemplate.Height = 28;
            dgvTaxes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaxes.Size = new Size(1176, 470);
            dgvTaxes.TabIndex = 8;
            dgvTaxes.TabStop = false;
            dgvTaxes.CellMouseDoubleClick += dgvTaxes_CellMouseDoubleClick;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Tahoma", 10F);
            txtSearch.Location = new Point(200, 111);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(500, 32);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // frmListTaxes
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1200, 723);
            Controls.Add(lblRecordCount);
            Controls.Add(lblSearch);
            Controls.Add(chkIncludeInactive);
            Controls.Add(txtSearch);
            Controls.Add(dgvTaxes);
            Controls.Add(lblMode);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "frmListTaxes";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة الضرائب";
            Load += frmListTaxes_Load;
            ((System.ComponentModel.ISupportInitialize)(this.dgvTaxes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkIncludeInactive;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.DataGridView dgvTaxes;
        private System.Windows.Forms.TextBox txtSearch;
    }
}
