namespace SalesSystem.Desktop.Screens.Settings.Forms
{
    partial class frmNotifications
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
            this.components = new System.ComponentModel.Container();
            this.lblMode = new System.Windows.Forms.Label();
            this.dgvNotifications = new System.Windows.Forms.DataGridView();
            this.lblRecordsCount = new System.Windows.Forms.Label();
            this.lblRecordsCountTitle = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnMarkAsRead = new System.Windows.Forms.Button();
            this.btnMarkAllAsRead = new System.Windows.Forms.Button();
            this.chkUnreadOnly = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmsNotifications = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiMarkAsRead = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMarkAllAsRead = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotifications)).BeginInit();
            this.cmsNotifications.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMode.AutoSize = true;
            this.lblMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMode.Location = new System.Drawing.Point(360, 15);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(180, 39);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "الإشعارات";
            // 
            // dgvNotifications
            // 
            this.dgvNotifications.AllowUserToAddRows = false;
            this.dgvNotifications.AllowUserToDeleteRows = false;
            this.dgvNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNotifications.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNotifications.BackgroundColor = System.Drawing.Color.White;
            this.dgvNotifications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotifications.ContextMenuStrip = this.cmsNotifications;
            this.dgvNotifications.Location = new System.Drawing.Point(12, 90);
            this.dgvNotifications.MultiSelect = false;
            this.dgvNotifications.Name = "dgvNotifications";
            this.dgvNotifications.ReadOnly = true;
            this.dgvNotifications.RowHeadersVisible = false;
            this.dgvNotifications.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNotifications.Size = new System.Drawing.Size(876, 440);
            this.dgvNotifications.TabIndex = 1;
            // 
            // lblRecordsCount
            // 
            this.lblRecordsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecordsCount.AutoSize = true;
            this.lblRecordsCount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecordsCount.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblRecordsCount.Location = new System.Drawing.Point(57, 545);
            this.lblRecordsCount.Name = "lblRecordsCount";
            this.lblRecordsCount.Size = new System.Drawing.Size(17, 18);
            this.lblRecordsCount.TabIndex = 3;
            this.lblRecordsCount.Text = "0";
            // 
            // lblRecordsCountTitle
            // 
            this.lblRecordsCountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecordsCountTitle.AutoSize = true;
            this.lblRecordsCountTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecordsCountTitle.Location = new System.Drawing.Point(12, 545);
            this.lblRecordsCountTitle.Name = "lblRecordsCountTitle";
            this.lblRecordsCountTitle.Size = new System.Drawing.Size(49, 18);
            this.lblRecordsCountTitle.TabIndex = 2;
            this.lblRecordsCountTitle.Text = "عدد :";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefresh.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnRefresh.Location = new System.Drawing.Point(12, 55);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 30);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnMarkAsRead
            // 
            this.btnMarkAsRead.BackColor = System.Drawing.Color.White;
            this.btnMarkAsRead.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMarkAsRead.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnMarkAsRead.Location = new System.Drawing.Point(108, 55);
            this.btnMarkAsRead.Name = "btnMarkAsRead";
            this.btnMarkAsRead.Size = new System.Drawing.Size(140, 30);
            this.btnMarkAsRead.TabIndex = 5;
            this.btnMarkAsRead.Text = "تحديد كمقروء";
            this.btnMarkAsRead.UseVisualStyleBackColor = false;
            this.btnMarkAsRead.Click += new System.EventHandler(this.btnMarkAsRead_Click);
            // 
            // btnMarkAllAsRead
            // 
            this.btnMarkAllAsRead.BackColor = System.Drawing.Color.White;
            this.btnMarkAllAsRead.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMarkAllAsRead.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnMarkAllAsRead.Location = new System.Drawing.Point(254, 55);
            this.btnMarkAllAsRead.Name = "btnMarkAllAsRead";
            this.btnMarkAllAsRead.Size = new System.Drawing.Size(160, 30);
            this.btnMarkAllAsRead.TabIndex = 6;
            this.btnMarkAllAsRead.Text = "تحديد الكل كمقروء";
            this.btnMarkAllAsRead.UseVisualStyleBackColor = false;
            this.btnMarkAllAsRead.Click += new System.EventHandler(this.btnMarkAllAsRead_Click);
            // 
            // chkUnreadOnly
            // 
            this.chkUnreadOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkUnreadOnly.AutoSize = true;
            this.chkUnreadOnly.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkUnreadOnly.Location = new System.Drawing.Point(750, 60);
            this.chkUnreadOnly.Name = "chkUnreadOnly";
            this.chkUnreadOnly.Size = new System.Drawing.Size(138, 22);
            this.chkUnreadOnly.TabIndex = 7;
            this.chkUnreadOnly.Text = "غير المقروءة فقط";
            this.chkUnreadOnly.UseVisualStyleBackColor = true;
            this.chkUnreadOnly.CheckedChanged += new System.EventHandler(this.chkUnreadOnly_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnClose.Location = new System.Drawing.Point(12, 580);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 36);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmsNotifications
            // 
            this.cmsNotifications.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRefresh,
            this.toolStripSeparator1,
            this.tsmiMarkAsRead,
            this.tsmiMarkAllAsRead});
            this.cmsNotifications.Name = "cmsNotifications";
            this.cmsNotifications.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmsNotifications.Size = new System.Drawing.Size(221, 82);
            // 
            // tsmiRefresh
            // 
            this.tsmiRefresh.Name = "tsmiRefresh";
            this.tsmiRefresh.Size = new System.Drawing.Size(220, 26);
            this.tsmiRefresh.Text = "تحديث";
            this.tsmiRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // tsmiMarkAsRead
            // 
            this.tsmiMarkAsRead.Name = "tsmiMarkAsRead";
            this.tsmiMarkAsRead.Size = new System.Drawing.Size(220, 26);
            this.tsmiMarkAsRead.Text = "تحديد كمقروء";
            this.tsmiMarkAsRead.Click += new System.EventHandler(this.btnMarkAsRead_Click);
            // 
            // tsmiMarkAllAsRead
            // 
            this.tsmiMarkAllAsRead.Name = "tsmiMarkAllAsRead";
            this.tsmiMarkAllAsRead.Size = new System.Drawing.Size(220, 26);
            this.tsmiMarkAllAsRead.Text = "تحديد الكل كمقروء";
            this.tsmiMarkAllAsRead.Click += new System.EventHandler(this.btnMarkAllAsRead_Click);
            // 
            // frmNotifications
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(900, 630);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkUnreadOnly);
            this.Controls.Add(this.btnMarkAllAsRead);
            this.Controls.Add(this.btnMarkAsRead);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblRecordsCount);
            this.Controls.Add(this.lblRecordsCountTitle);
            this.Controls.Add(this.dgvNotifications);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNotifications";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "الإشعارات";
            this.Load += new System.EventHandler(this.frmNotifications_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotifications)).EndInit();
            this.cmsNotifications.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.DataGridView dgvNotifications;
        private System.Windows.Forms.Label lblRecordsCount;
        private System.Windows.Forms.Label lblRecordsCountTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnMarkAsRead;
        private System.Windows.Forms.Button btnMarkAllAsRead;
        private System.Windows.Forms.CheckBox chkUnreadOnly;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ContextMenuStrip cmsNotifications;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarkAsRead;
        private System.Windows.Forms.ToolStripMenuItem tsmiMarkAllAsRead;
    }
}
