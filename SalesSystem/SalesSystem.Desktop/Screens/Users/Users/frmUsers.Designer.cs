namespace SalesSystem.Desktop.Controls.Users
{
    partial class frmUsers
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
            lblInfo = new Label();
            btnOpenList = new Button();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.DarkSlateGray;
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 10.8F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(568, 42);
            lblMode.TabIndex = 0;
            lblMode.Text = "إدارة المستخدمين";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Tahoma", 10F);
            lblInfo.ForeColor = Color.FromArgb(100, 100, 100);
            lblInfo.Location = new Point(45, 100);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(506, 24);
            lblInfo.TabIndex = 1;
            lblInfo.Text = "هذه الواجهة متاحة عبر شاشة قائمة المستخدمين المنفصلة.";
            // 
            // btnOpenList
            // 
            btnOpenList.BackColor = Color.White;
            btnOpenList.FlatStyle = FlatStyle.Popup;
            btnOpenList.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnOpenList.Location = new Point(198, 162);
            btnOpenList.Name = "btnOpenList";
            btnOpenList.Size = new Size(200, 42);
            btnOpenList.TabIndex = 2;
            btnOpenList.Text = "فتح قائمة المستخدمين";
            btnOpenList.UseVisualStyleBackColor = false;
            btnOpenList.Click += btnOpenList_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(12, 260);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(96, 36);
            btnClose.TabIndex = 3;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // frmUsers
            // 
            AutoScaleDimensions = new SizeF(13F, 29F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(568, 310);
            Controls.Add(btnClose);
            Controls.Add(btnOpenList);
            Controls.Add(lblInfo);
            Controls.Add(lblMode);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmUsers";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إدارة المستخدمين";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnOpenList;
        private System.Windows.Forms.Button btnClose;
    }
}
