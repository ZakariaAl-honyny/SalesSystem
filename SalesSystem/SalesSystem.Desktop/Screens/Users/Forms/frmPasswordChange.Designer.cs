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
        lblCurrentPassword = new Label();
        txtCurrentPassword = new TextBox();
        label1 = new Label();
        txtNewPassword = new TextBox();
        label2 = new Label();
        txtConfirmPassword = new TextBox();
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
        gbPassword.Controls.Add(lblCurrentPassword);
        gbPassword.Controls.Add(txtCurrentPassword);
        gbPassword.Controls.Add(label1);
        gbPassword.Controls.Add(txtNewPassword);
        gbPassword.Controls.Add(label2);
        gbPassword.Controls.Add(txtConfirmPassword);
        gbPassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        gbPassword.ForeColor = Color.FromArgb(14, 82, 52);
        gbPassword.Location = new Point(12, 58);
        gbPassword.Name = "gbPassword";
        gbPassword.Padding = new Padding(8);
        gbPassword.Size = new Size(596, 200);
        gbPassword.TabIndex = 1;
        gbPassword.TabStop = false;
        gbPassword.Text = "بيانات كلمة المرور";
        // 
        // lblCurrentPassword
        // 
        lblCurrentPassword.AutoSize = true;
        lblCurrentPassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblCurrentPassword.Location = new Point(390, 40);
        lblCurrentPassword.Name = "lblCurrentPassword";
        lblCurrentPassword.Size = new Size(186, 24);
        lblCurrentPassword.TabIndex = 0;
        lblCurrentPassword.Text = "كلمة المرور الحالية:";
        lblCurrentPassword.Visible = false;
        // 
        // txtCurrentPassword
        // 
        txtCurrentPassword.BackColor = Color.White;
        txtCurrentPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtCurrentPassword.Location = new Point(94, 38);
        txtCurrentPassword.Name = "txtCurrentPassword";
        txtCurrentPassword.PasswordChar = '*';
        txtCurrentPassword.RightToLeft = RightToLeft.Yes;
        txtCurrentPassword.Size = new Size(286, 32);
        txtCurrentPassword.TabIndex = 1;
        txtCurrentPassword.Visible = false;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(390, 98);
        label1.Name = "label1";
        label1.Size = new Size(193, 24);
        label1.TabIndex = 2;
        label1.Text = "كلمة المرور الجديدة:";
        // 
        // txtNewPassword
        // 
        txtNewPassword.BackColor = Color.White;
        txtNewPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtNewPassword.Location = new Point(94, 96);
        txtNewPassword.Name = "txtNewPassword";
        txtNewPassword.PasswordChar = '*';
        txtNewPassword.RightToLeft = RightToLeft.Yes;
        txtNewPassword.Size = new Size(286, 32);
        txtNewPassword.TabIndex = 3;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(390, 156);
        label2.Name = "label2";
        label2.Size = new Size(168, 24);
        label2.TabIndex = 4;
        label2.Text = "تأكيد كلمة المرور:";
        // 
        // txtConfirmPassword
        // 
        txtConfirmPassword.BackColor = Color.White;
        txtConfirmPassword.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtConfirmPassword.Location = new Point(94, 154);
        txtConfirmPassword.Name = "txtConfirmPassword";
        txtConfirmPassword.PasswordChar = '*';
        txtConfirmPassword.RightToLeft = RightToLeft.Yes;
        txtConfirmPassword.Size = new Size(286, 32);
        txtConfirmPassword.TabIndex = 5;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(235, 240, 235);
        pnlFooter.Controls.Add(btnSave);
        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 272);
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
        ClientSize = new Size(620, 340);
        Controls.Add(gbPassword);
        Controls.Add(pnlFooter);
        Controls.Add(lblMode);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
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
    private Label lblCurrentPassword;
    private TextBox txtCurrentPassword;
    private Label label1;
    private TextBox txtNewPassword;
    private Label label2;
    private TextBox txtConfirmPassword;
    private Panel pnlFooter;
    private Button btnSave;
    private Button btnClose;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
