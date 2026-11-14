namespace SalesSystem.Desktop.Screens.Users.Forms;

partial class frmPasswordChange
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblMode = new Label();
        gbPassword = new GroupBox();
        lblHint = new Label();
        lblCurrentPassword = new Label();
        txtCurrentPassword = new TextBox();
        lblNewPassword = new Label();
        txtNewPassword = new TextBox();
        lblConfirmPassword = new Label();
        txtConfirmPassword = new TextBox();
        lblError = new Label();
        pnlFooter = new Panel();
        btnSave = new Button();
        btnClose = new Button();
        errorProvider = new ErrorProvider(components);
        gbPassword.SuspendLayout();
        pnlFooter.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.BackColor = Color.FromArgb(14, 165, 103);
        lblMode.Dock = DockStyle.Top;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.White;
        lblMode.Location = new Point(0, 0);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(620, 46);
        lblMode.TabIndex = 0;
        lblMode.Text = "تغيير كلمة المرور";
        lblMode.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // gbPassword
        // 
        gbPassword.BackColor = Color.FromArgb(235, 245, 235);
        gbPassword.Controls.Add(lblHint);
        gbPassword.Controls.Add(lblCurrentPassword);
        gbPassword.Controls.Add(txtCurrentPassword);
        gbPassword.Controls.Add(lblNewPassword);
        gbPassword.Controls.Add(txtNewPassword);
        gbPassword.Controls.Add(lblConfirmPassword);
        gbPassword.Controls.Add(txtConfirmPassword);
        gbPassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbPassword.ForeColor = Color.FromArgb(14, 82, 52);
        gbPassword.Location = new Point(12, 58);
        gbPassword.Name = "gbPassword";
        gbPassword.Padding = new Padding(8);
        gbPassword.Size = new Size(596, 232);
        gbPassword.TabIndex = 1;
        gbPassword.TabStop = false;
        gbPassword.Text = "بيانات كلمة المرور";
        // 
        // lblHint
        // 
        lblHint.AutoSize = true;
        lblHint.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        lblHint.ForeColor = Color.FromArgb(14, 82, 52);
        lblHint.Location = new Point(16, 28);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(316, 19);
        lblHint.TabIndex = 0;
        lblHint.Text = "يجب إدخال كلمة المرور الحالية لتأكيد التغيير.";
        lblHint.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCurrentPassword
        // 
        lblCurrentPassword.AutoSize = true;
        lblCurrentPassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCurrentPassword.Location = new Point(390, 66);
        lblCurrentPassword.Name = "lblCurrentPassword";
        lblCurrentPassword.Size = new Size(186, 24);
        lblCurrentPassword.TabIndex = 1;
        lblCurrentPassword.Text = "كلمة المرور الحالية:";
        // 
        // txtCurrentPassword
        // 
        txtCurrentPassword.BackColor = Color.White;
        txtCurrentPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtCurrentPassword.Location = new Point(94, 64);
        txtCurrentPassword.Name = "txtCurrentPassword";
        txtCurrentPassword.PasswordChar = '*';
        txtCurrentPassword.RightToLeft = RightToLeft.Yes;
        txtCurrentPassword.Size = new Size(286, 32);
        txtCurrentPassword.TabIndex = 2;
        txtCurrentPassword.TextChanged += _OnPasswordTextChanged;
        // 
        // lblNewPassword
        // 
        lblNewPassword.AutoSize = true;
        lblNewPassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblNewPassword.Location = new Point(390, 124);
        lblNewPassword.Name = "lblNewPassword";
        lblNewPassword.Size = new Size(193, 24);
        lblNewPassword.TabIndex = 3;
        lblNewPassword.Text = "كلمة المرور الجديدة:";
        // 
        // txtNewPassword
        // 
        txtNewPassword.BackColor = Color.White;
        txtNewPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNewPassword.Location = new Point(94, 122);
        txtNewPassword.Name = "txtNewPassword";
        txtNewPassword.PasswordChar = '*';
        txtNewPassword.RightToLeft = RightToLeft.Yes;
        txtNewPassword.Size = new Size(286, 32);
        txtNewPassword.TabIndex = 4;
        txtNewPassword.TextChanged += _OnPasswordTextChanged;
        // 
        // lblConfirmPassword
        // 
        lblConfirmPassword.AutoSize = true;
        lblConfirmPassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblConfirmPassword.Location = new Point(390, 182);
        lblConfirmPassword.Name = "lblConfirmPassword";
        lblConfirmPassword.Size = new Size(168, 24);
        lblConfirmPassword.TabIndex = 5;
        lblConfirmPassword.Text = "تأكيد كلمة المرور:";
        // 
        // txtConfirmPassword
        // 
        txtConfirmPassword.BackColor = Color.White;
        txtConfirmPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtConfirmPassword.Location = new Point(94, 180);
        txtConfirmPassword.Name = "txtConfirmPassword";
        txtConfirmPassword.PasswordChar = '*';
        txtConfirmPassword.RightToLeft = RightToLeft.Yes;
        txtConfirmPassword.Size = new Size(286, 32);
        txtConfirmPassword.TabIndex = 6;
        txtConfirmPassword.TextChanged += _OnPasswordTextChanged;
        // 
        // lblError
        // 
        lblError.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblError.ForeColor = Color.FromArgb(220, 53, 69);
        lblError.Location = new Point(12, 298);
        lblError.Name = "lblError";
        lblError.Size = new Size(596, 26);
        lblError.TabIndex = 3;
        lblError.TextAlign = ContentAlignment.MiddleCenter;
        lblError.Visible = false;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 342);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Padding = new Padding(10);
        pnlFooter.Size = new Size(620, 68);
        pnlFooter.TabIndex = 2;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(14, 165, 103);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(440, 12);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(160, 44);
        btnSave.TabIndex = 0;
        btnSave.Text = "حفظ (F2)";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.FromArgb(108, 117, 125);
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.White;
        btnClose.Location = new Point(270, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(160, 44);
        btnClose.TabIndex = 1;
        btnClose.Text = "إلغاء (Esc)";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // errorProvider
        // 
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.ContainerControl = this;
        // 
        // frmPasswordChange
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(170, 220, 170);
        CancelButton = btnClose;
        ClientSize = new Size(620, 410);
        Controls.Add(gbPassword);
        Controls.Add(lblError);
        Controls.Add(pnlFooter);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimumSize = new Size(620, 410);
        MinimizeBox = false;
        Name = "frmPasswordChange";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "تغيير كلمة المرور";
        Load += frmPasswordChange_Load;
        gbPassword.ResumeLayout(false);
        gbPassword.PerformLayout();
        pnlFooter.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private GroupBox gbPassword;
    private Label lblHint;
    private Label lblCurrentPassword;
    private TextBox txtCurrentPassword;
    private Label lblNewPassword;
    private TextBox txtNewPassword;
    private Label lblConfirmPassword;
    private TextBox txtConfirmPassword;
    private Label lblError;
    private Panel pnlFooter;
    private Button btnSave;
    private Button btnClose;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
