namespace SalesSystem.Desktop.Screens.Dashboard.Forms
{
    partial class frmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        // المصمّم = وقت التشغيل (RULE-N134 / RULE-N140 / RULE-N141):
        //   كل سطح هنا معلَن بقيمة حرفية مطابقة لثوابت GreenThemeHelper — لا SystemColors،
        //   ولا تفعيل UseVisualStyleBackColor على النموذج، ولا مرجع لأي عضو في Helpers/ (RULE-N128).
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            pnlHost = new Panel();
            btnClose = new Button();
            toolTip = new ToolTip(components);
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.FromArgb(16, 150, 96);
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.White;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(1275, 44);
            lblMode.TabIndex = 0;
            lblMode.Text = "لوحة المعلومات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlHost
            // 
            pnlHost.BackColor = Color.FromArgb(240, 242, 245);
            pnlHost.Dock = DockStyle.Fill;
            pnlHost.Location = new Point(0, 0);
            pnlHost.Name = "pnlHost";
            pnlHost.Size = new Size(1275, 655);
            pnlHost.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(108, 117, 125);
            btnClose.Cursor = Cursors.Hand;
            btnClose.Dock = DockStyle.Bottom;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(0, 603);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(1275, 52);
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق";
            toolTip.SetToolTip(btnClose, "إغلاق الشاشة (Esc)");
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // frmDashboard
            // 
            AcceptButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(240, 242, 245);
            CancelButton = btnClose;
            ClientSize = new Size(1275, 655);
            Controls.Add(btnClose);
            Controls.Add(lblMode);
            Controls.Add(pnlHost);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDashboard";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "لوحة المعلومات";
            ResumeLayout(false);
        }

        #endregion

        private Label lblMode;
        private Panel pnlHost;
        private Button btnClose;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
