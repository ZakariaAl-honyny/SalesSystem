namespace SalesSystem.Desktop.Screens
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            gbLogin = new GroupBox();
            lblUser = new Label();
            cmbUsers = new ComboBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblMockHint = new Label();
            btnLogin = new Button();
            lblError = new Label();
            errorProvider1 = new ErrorProvider(components);
            gbLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(14, 165, 103);
            lblTitle.Font = new Font("Tahoma", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 50);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(686, 67);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "برنامج إدارة المبيعات";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbLogin
            // 
            gbLogin.BackColor = Color.FromArgb(235, 245, 235);
            gbLogin.Controls.Add(lblUser);
            gbLogin.Controls.Add(cmbUsers);
            gbLogin.Controls.Add(lblPassword);
            gbLogin.Controls.Add(txtPassword);
            gbLogin.Controls.Add(lblMockHint);
            gbLogin.Controls.Add(btnLogin);
            gbLogin.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            gbLogin.ForeColor = Color.FromArgb(14, 82, 52);
            gbLogin.Location = new Point(58, 132);
            gbLogin.Margin = new Padding(4, 5, 4, 5);
            gbLogin.Name = "gbLogin";
            gbLogin.Padding = new Padding(8);
            gbLogin.Size = new Size(570, 236);
            gbLogin.TabIndex = 1;
            gbLogin.TabStop = false;
            gbLogin.Text = "تسجيل الدخول";
            // 
            // lblUser
            // 
            lblUser.Font = new Font("Tahoma", 10F);
            lblUser.ForeColor = Color.FromArgb(14, 82, 52);
            lblUser.Location = new Point(44, 40);
            lblUser.Margin = new Padding(4, 0, 4, 0);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(170, 30);
            lblUser.TabIndex = 0;
            lblUser.Text = "المستخدم:";
            lblUser.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cmbUsers
            // 
            cmbUsers.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            cmbUsers.BackColor = Color.White;
            cmbUsers.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUsers.FlatStyle = FlatStyle.Flat;
            cmbUsers.Font = new Font("Tahoma", 10F);
            cmbUsers.Location = new Point(222, 36);
            cmbUsers.Margin = new Padding(4, 5, 4, 5);
            cmbUsers.Name = "cmbUsers";
            cmbUsers.Size = new Size(300, 32);
            cmbUsers.TabIndex = 1;
            cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            // 
            // lblPassword
            // 
            lblPassword.Font = new Font("Tahoma", 10F);
            lblPassword.ForeColor = Color.FromArgb(14, 82, 52);
            lblPassword.Location = new Point(44, 92);
            lblPassword.Margin = new Padding(4, 0, 4, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(170, 30);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "كلمة المرور:";
            lblPassword.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            txtPassword.BackColor = Color.White;
            txtPassword.Font = new Font("Tahoma", 10F);
            txtPassword.Location = new Point(222, 88);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.MaxLength = 128;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(300, 32);
            txtPassword.TabIndex = 3;
            txtPassword.TextAlign = HorizontalAlignment.Center;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.KeyPress += txtPassword_KeyPress;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // lblMockHint
            // 
            lblMockHint.Font = new Font("Tahoma", 9F);
            lblMockHint.ForeColor = Color.FromArgb(14, 82, 52);
            lblMockHint.Location = new Point(44, 134);
            lblMockHint.Margin = new Padding(4, 0, 4, 0);
            lblMockHint.Name = "lblMockHint";
            lblMockHint.Size = new Size(478, 24);
            lblMockHint.TabIndex = 4;
            lblMockHint.Text = "وضع التجربة: كلمة المرور لكل الحسابات هي 123456 — لا قاعدة بيانات.";
            lblMockHint.TextAlign = ContentAlignment.MiddleCenter;
            lblMockHint.Visible = false;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(14, 165, 103);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 103);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(222, 168);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(300, 46);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "دخول";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // lblError
            // 
            lblError.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblError.ForeColor = Color.FromArgb(220, 53, 69);
            lblError.Location = new Point(0, 382);
            lblError.Margin = new Padding(4, 0, 4, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(686, 46);
            lblError.TabIndex = 2;
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.Visible = false;
            // 
            // frmLogin
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(170, 220, 170);
            ClientSize = new Size(686, 470);
            Controls.Add(lblError);
            Controls.Add(gbLogin);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmLogin";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تسجيل الدخول";
            errorProvider1.ContainerControl = this;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            gbLogin.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox gbLogin;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox cmbUsers;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblMockHint;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
