namespace SalesSystem.Desktop.Screens
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            lblPin = new Label();
            txtPin = new TextBox();
            btnLogin = new Button();
            lblError = new Label();
            errorProvider1 = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Tahoma", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 50);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(686, 67);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "برنامج إدارة المبيعات";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPin
            // 
            lblPin.Location = new Point(114, 210);
            lblPin.Margin = new Padding(4, 0, 4, 0);
            lblPin.Name = "lblPin";
            lblPin.Size = new Size(143, 31);
            lblPin.TabIndex = 3;
            lblPin.Text = "رمز الدخول (PIN):";
            lblPin.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtPin
            // 
            txtPin.Location = new Point(257, 206);
            txtPin.Margin = new Padding(4, 5, 4, 5);
            txtPin.MaxLength = 12;
            txtPin.Name = "txtPin";
            txtPin.Size = new Size(284, 31);
            txtPin.TabIndex = 1;
            txtPin.TextAlign = HorizontalAlignment.Center;
            txtPin.UseSystemPasswordChar = true;
            txtPin.KeyPress += txtPin_KeyPress;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.White;
            btnLogin.FlatAppearance.BorderColor = Color.Black;
            btnLogin.FlatAppearance.BorderSize = 2;
            btnLogin.FlatStyle = FlatStyle.Popup;
            btnLogin.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.Location = new Point(257, 285);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(286, 68);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "دخول";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lblError
            // 
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(0, 380);
            lblError.Margin = new Padding(4, 0, 4, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(686, 50);
            lblError.TabIndex = 4;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;
            // 
            // LoginForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(686, 470);
            Controls.Add(lblError);
            Controls.Add(btnLogin);
            Controls.Add(txtPin);
            Controls.Add(lblPin);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "frmLogin";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تسجيل الدخول";
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
