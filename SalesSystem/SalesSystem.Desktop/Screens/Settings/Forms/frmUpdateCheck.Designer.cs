namespace SalesSystem.Desktop.Screens.Settings.Forms
{
    partial class frmUpdateCheck
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
            this.lblSystemName = new System.Windows.Forms.Label();
            this.lblSystemNameValue = new System.Windows.Forms.Label();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.lblCurrentVersionValue = new System.Windows.Forms.Label();
            this.lblLatestVersion = new System.Windows.Forms.Label();
            this.lblLatestVersionValue = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.btnCheckForUpdates = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.lblChangelog = new System.Windows.Forms.Label();
            this.txtChangelog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMode.AutoSize = true;
            this.lblMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMode.Location = new System.Drawing.Point(160, 15);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(268, 34);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "التحقق من التحديثات";
            // 
            // lblSystemName
            // 
            this.lblSystemName.AutoSize = true;
            this.lblSystemName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSystemName.Location = new System.Drawing.Point(460, 75);
            this.lblSystemName.Name = "lblSystemName";
            this.lblSystemName.Size = new System.Drawing.Size(116, 18);
            this.lblSystemName.TabIndex = 1;
            this.lblSystemName.Text = "اسم النظام:";
            // 
            // lblSystemNameValue
            // 
            this.lblSystemNameValue.AutoSize = true;
            this.lblSystemNameValue.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblSystemNameValue.Location = new System.Drawing.Point(60, 75);
            this.lblSystemNameValue.Name = "lblSystemNameValue";
            this.lblSystemNameValue.Size = new System.Drawing.Size(119, 18);
            this.lblSystemNameValue.TabIndex = 2;
            this.lblSystemNameValue.Text = "نظام إدارة المبيعات";
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblCurrentVersion.Location = new System.Drawing.Point(460, 110);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(106, 18);
            this.lblCurrentVersion.TabIndex = 3;
            this.lblCurrentVersion.Text = "الإصدار الحالي:";
            // 
            // lblCurrentVersionValue
            // 
            this.lblCurrentVersionValue.AutoSize = true;
            this.lblCurrentVersionValue.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCurrentVersionValue.Location = new System.Drawing.Point(60, 110);
            this.lblCurrentVersionValue.Name = "lblCurrentVersionValue";
            this.lblCurrentVersionValue.Size = new System.Drawing.Size(51, 18);
            this.lblCurrentVersionValue.TabIndex = 4;
            this.lblCurrentVersionValue.Text = "1.0.0.0";
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.AutoSize = true;
            this.lblLatestVersion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblLatestVersion.Location = new System.Drawing.Point(460, 145);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Size = new System.Drawing.Size(121, 18);
            this.lblLatestVersion.TabIndex = 5;
            this.lblLatestVersion.Text = "أحدث إصدار:";
            // 
            // lblLatestVersionValue
            // 
            this.lblLatestVersionValue.AutoSize = true;
            this.lblLatestVersionValue.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblLatestVersionValue.ForeColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.lblLatestVersionValue.Location = new System.Drawing.Point(60, 145);
            this.lblLatestVersionValue.Name = "lblLatestVersionValue";
            this.lblLatestVersionValue.Size = new System.Drawing.Size(99, 18);
            this.lblLatestVersionValue.TabIndex = 6;
            this.lblLatestVersionValue.Text = "--";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(460, 180);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(68, 18);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "الحالة:";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblStatusValue.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.lblStatusValue.Location = new System.Drawing.Point(60, 180);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(141, 18);
            this.lblStatusValue.TabIndex = 8;
            this.lblStatusValue.Text = "لم يتم الفحص بعد";
            // 
            // btnCheckForUpdates
            // 
            this.btnCheckForUpdates.BackColor = System.Drawing.Color.White;
            this.btnCheckForUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCheckForUpdates.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnCheckForUpdates.ForeColor = System.Drawing.Color.Black;
            this.btnCheckForUpdates.Location = new System.Drawing.Point(220, 225);
            this.btnCheckForUpdates.Name = "btnCheckForUpdates";
            this.btnCheckForUpdates.Size = new System.Drawing.Size(180, 42);
            this.btnCheckForUpdates.TabIndex = 9;
            this.btnCheckForUpdates.Text = "التحقق من التحديثات";
            this.btnCheckForUpdates.UseVisualStyleBackColor = false;
            this.btnCheckForUpdates.Click += new System.EventHandler(this.btnCheckForUpdates_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnClose.Location = new System.Drawing.Point(12, 480);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 36);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(60, 280);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(480, 25);
            this.progressBar.TabIndex = 11;
            this.progressBar.Visible = false;
            // 
            // lblProgressText
            // 
            this.lblProgressText.AutoSize = true;
            this.lblProgressText.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblProgressText.ForeColor = System.Drawing.Color.Gray;
            this.lblProgressText.Location = new System.Drawing.Point(60, 315);
            this.lblProgressText.Name = "lblProgressText";
            this.lblProgressText.Size = new System.Drawing.Size(0, 18);
            this.lblProgressText.TabIndex = 12;
            // 
            // lblChangelog
            // 
            this.lblChangelog.AutoSize = true;
            this.lblChangelog.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblChangelog.Location = new System.Drawing.Point(460, 350);
            this.lblChangelog.Name = "lblChangelog";
            this.lblChangelog.Size = new System.Drawing.Size(126, 18);
            this.lblChangelog.TabIndex = 13;
            this.lblChangelog.Text = "سجل التغييرات:";
            // 
            // txtChangelog
            // 
            this.txtChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChangelog.BackColor = System.Drawing.Color.White;
            this.txtChangelog.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtChangelog.Location = new System.Drawing.Point(60, 350);
            this.txtChangelog.Multiline = true;
            this.txtChangelog.Name = "txtChangelog";
            this.txtChangelog.ReadOnly = true;
            this.txtChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChangelog.Size = new System.Drawing.Size(390, 100);
            this.txtChangelog.TabIndex = 14;
            // 
            // frmUpdateCheck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(600, 530);
            this.Controls.Add(this.txtChangelog);
            this.Controls.Add(this.lblChangelog);
            this.Controls.Add(this.lblProgressText);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCheckForUpdates);
            this.Controls.Add(this.lblStatusValue);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblLatestVersionValue);
            this.Controls.Add(this.lblLatestVersion);
            this.Controls.Add(this.lblCurrentVersionValue);
            this.Controls.Add(this.lblCurrentVersion);
            this.Controls.Add(this.lblSystemNameValue);
            this.Controls.Add(this.lblSystemName);
            this.Controls.Add(this.lblMode);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateCheck";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "التحقق من التحديثات";
            this.Load += new System.EventHandler(this.frmUpdateCheck_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblSystemName;
        private System.Windows.Forms.Label lblSystemNameValue;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Label lblCurrentVersionValue;
        private System.Windows.Forms.Label lblLatestVersion;
        private System.Windows.Forms.Label lblLatestVersionValue;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Button btnCheckForUpdates;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgressText;
        private System.Windows.Forms.Label lblChangelog;
        private System.Windows.Forms.TextBox txtChangelog;
    }
}
