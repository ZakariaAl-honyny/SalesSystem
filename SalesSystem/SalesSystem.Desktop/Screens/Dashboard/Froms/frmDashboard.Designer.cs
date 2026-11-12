namespace SalesSystem.Desktop.Screens.Dashboard.Forms
{
    partial class frmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblMode = new Label();
            pnlHost = new Panel();
            btnClose = new Button();
            SuspendLayout();
            // 
            // lblMode — شريط عنوان الشاشة (RULE-N27 / RULE-N53)
            // 
            lblMode.Dock = DockStyle.Top;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.Height = 44;
            lblMode.Name = "lblMode";
            lblMode.TabIndex = 0;
            lblMode.Text = "لوحة المعلومات";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlHost — يحوي ctrlDashboard بـ Dock=Fill
            // 
            pnlHost.Dock = DockStyle.Fill;
            pnlHost.Name = "pnlHost";
            pnlHost.TabIndex = 1;
            // 
            // btnClose
            // 
            btnClose.Dock = DockStyle.Bottom;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Height = 52;
            btnClose.Name = "btnClose";
            btnClose.TabIndex = 2;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // frmDashboard
            // 
            AcceptButton = btnClose;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1200, 620);
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
    }
}
