namespace SalesSystem.Desktop.Screens.Users.Forms;

partial class frmPasswordChange
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
        components = new System.ComponentModel.Container();
        lblMode = new Label();
        label1 = new Label();
        txtNewPassword = new TextBox();
        label2 = new Label();
        txtConfirmPassword = new TextBox();
        btnSave = new Button();
        btnClose = new Button();
        lblCurrentPassword = new Label();
        txtCurrentPassword = new TextBox();
        errorProvider = new ErrorProvider(components);
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // lblMode
        // 
        lblMode.Anchor = AnchorStyles.Top;
        lblMode.AutoSize = true;
        lblMode.BackColor = Color.WhiteSmoke;
        lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(146, 9);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(267, 39);
        lblMode.TabIndex = 30;
        lblMode.Text = "تغيير كلمة المرور";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(12, 109);
        label1.Name = "label1";
        label1.Size = new Size(193, 24);
        label1.TabIndex = 0;
        label1.Text = "كلمة المرور الجديدة";
        // 
        // txtNewPassword
        // 
        txtNewPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNewPassword.Location = new Point(211, 109);
        txtNewPassword.Name = "txtNewPassword";
        txtNewPassword.PasswordChar = '*';
        txtNewPassword.RightToLeft = RightToLeft.Yes;
        txtNewPassword.Size = new Size(266, 32);
        txtNewPassword.TabIndex = 1;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(12, 157);
        label2.Name = "label2";
        label2.Size = new Size(168, 24);
        label2.TabIndex = 2;
        label2.Text = "تأكيد كلمة المرور";
        // 
        // txtConfirmPassword
        // 
        txtConfirmPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtConfirmPassword.Location = new Point(211, 157);
        txtConfirmPassword.Name = "txtConfirmPassword";
        txtConfirmPassword.PasswordChar = '*';
        txtConfirmPassword.RightToLeft = RightToLeft.Yes;
        txtConfirmPassword.Size = new Size(266, 32);
        txtConfirmPassword.TabIndex = 3;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.White;
        btnSave.FlatAppearance.BorderColor = Color.Black;
        btnSave.FlatAppearance.BorderSize = 2;
        btnSave.FlatStyle = FlatStyle.Popup;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.Location = new Point(211, 201);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(140, 45);
        btnSave.TabIndex = 4;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(357, 201);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 5;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // lblCurrentPassword
        // 
        lblCurrentPassword.AutoSize = true;
        lblCurrentPassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCurrentPassword.Location = new Point(12, 68);
        lblCurrentPassword.Name = "lblCurrentPassword";
        lblCurrentPassword.Size = new Size(186, 24);
        lblCurrentPassword.TabIndex = 31;
        lblCurrentPassword.Text = "كلمة المرور الحالية";
        // 
        // txtCurrentPassword
        // 
        txtCurrentPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtCurrentPassword.Location = new Point(211, 68);
        txtCurrentPassword.Name = "txtCurrentPassword";
        txtCurrentPassword.PasswordChar = '*';
        txtCurrentPassword.RightToLeft = RightToLeft.Yes;
        txtCurrentPassword.Size = new Size(266, 32);
        txtCurrentPassword.TabIndex = 32;
        // 
        // frmPasswordChange
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(613, 275);
        Controls.Add(lblCurrentPassword);
        Controls.Add(txtCurrentPassword);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(txtNewPassword);
        Controls.Add(label2);
        Controls.Add(txtConfirmPassword);
        Controls.Add(btnSave);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmPasswordChange";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "تغيير كلمة المرور";
        Load += frmPasswordChange_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Label label1;
    private TextBox txtNewPassword;
    private Label label2;
    private TextBox txtConfirmPassword;
    private Button btnSave;
    private Button btnClose;
    private Label lblCurrentPassword;
    private TextBox txtCurrentPassword;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
