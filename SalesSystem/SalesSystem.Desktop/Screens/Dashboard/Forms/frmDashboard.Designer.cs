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
            // lblMode — شريط عنوان الشاشة (RULE-N27 / RULE-N53)
            // 
            lblMode.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(16), Convert.ToInt32(150), Convert.ToInt32(96));
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = System.Drawing.Color.White;
            lblMode.Height = 44;
            lblMode.Name = "lblMode";
            lblMode.TabIndex = 0;
            lblMode.Text = "لوحة المعلومات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlHost — يحوي ctrlDashboard بـ Dock=Fill
            // 
            pnlHost.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
            pnlHost.Dock = DockStyle.Fill;
            pnlHost.Name = "pnlHost";
            pnlHost.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(108), Convert.ToInt32(117), Convert.ToInt32(125));
            btnClose.Cursor = Cursors.Hand;
            btnClose.Dock = DockStyle.Bottom;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.ForeColor = System.Drawing.Color.White;
            btnClose.Height = 52;
            btnClose.Name = "btnClose";
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
            BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(240), Convert.ToInt32(242), Convert.ToInt32(245));
            CancelButton = btnClose;
            ClientSize = new Size(1200, 620);
            // ترتيب الإرساء (RULE-N125): Fill أولاً ثم Bottom ثم Top — يُرسى آخر المُضاف أولاً
            Controls.Add(pnlHost);
            Controls.Add(btnClose);
            Controls.Add(lblMode);
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
