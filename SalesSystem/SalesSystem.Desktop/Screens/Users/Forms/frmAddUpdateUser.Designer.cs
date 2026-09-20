namespace SalesSystem.Desktop.Screens.Users.Forms;

partial class frmAddUpdateUser
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
        lblUserId = new Label();
        label2 = new Label();
        txtUserName = new TextBox();
        label3 = new Label();
        cbRoleId = new ComboBox();
        label4 = new Label();
        cbBranchId = new ComboBox();
        chkIsActive = new CheckBox();
        chkMustChangePassword = new CheckBox();
        btnSave = new Button();
        btnClose = new Button();
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
        lblMode.FlatStyle = FlatStyle.Flat;
        lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblMode.ForeColor = Color.Firebrick;
        lblMode.Location = new Point(230, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(280, 34);
        lblMode.TabIndex = 30;
        lblMode.Text = "إضافة مستخدم جديد";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(76, 80);
        label1.Name = "label1";
        label1.Size = new Size(149, 24);
        label1.TabIndex = 0;
        label1.Text = "رقم المستخدم";
        // 
        // lblUserId
        // 
        lblUserId.AutoSize = true;
        lblUserId.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblUserId.Location = new Point(244, 80);
        lblUserId.Name = "lblUserId";
        lblUserId.Size = new Size(56, 24);
        lblUserId.TabIndex = 1;
        lblUserId.Text = "جديد";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(62, 122);
        label2.Name = "label2";
        label2.Size = new Size(176, 24);
        label2.TabIndex = 2;
        label2.Text = "اسم المستخدم *";
        // 
        // txtUserName
        // 
        txtUserName.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtUserName.Location = new Point(244, 122);
        txtUserName.Name = "txtUserName";
        txtUserName.RightToLeft = RightToLeft.Yes;
        txtUserName.Size = new Size(266, 32);
        txtUserName.TabIndex = 3;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(102, 170);
        label3.Name = "label3";
        label3.Size = new Size(74, 24);
        label3.TabIndex = 4;
        label3.Text = "الدور *";
        // 
        // cbRoleId
        // 
        cbRoleId.DropDownStyle = ComboBoxStyle.DropDownList;
        cbRoleId.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbRoleId.Location = new Point(244, 170);
        cbRoleId.Name = "cbRoleId";
        cbRoleId.RightToLeft = RightToLeft.Yes;
        cbRoleId.Size = new Size(266, 32);
        cbRoleId.TabIndex = 5;
        cbRoleId.SelectedIndexChanged += cbRoleId_SelectedIndexChanged;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label4.Location = new Point(102, 218);
        label4.Name = "label4";
        label4.Size = new Size(58, 24);
        label4.TabIndex = 6;
        label4.Text = "الفرع";
        // 
        // cbBranchId
        // 
        cbBranchId.DropDownStyle = ComboBoxStyle.DropDownList;
        cbBranchId.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        cbBranchId.Location = new Point(244, 218);
        cbBranchId.Name = "cbBranchId";
        cbBranchId.RightToLeft = RightToLeft.Yes;
        cbBranchId.Size = new Size(266, 32);
        cbBranchId.TabIndex = 7;
        // 
        // chkIsActive
        // 
        chkIsActive.AutoSize = true;
        chkIsActive.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        chkIsActive.Location = new Point(244, 266);
        chkIsActive.Name = "chkIsActive";
        chkIsActive.Size = new Size(83, 28);
        chkIsActive.TabIndex = 8;
        chkIsActive.Text = "نشط";
        chkIsActive.UseVisualStyleBackColor = true;
        // 
        // chkMustChangePassword
        // 
        chkMustChangePassword.AutoSize = true;
        chkMustChangePassword.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        chkMustChangePassword.Location = new Point(349, 266);
        chkMustChangePassword.Name = "chkMustChangePassword";
        chkMustChangePassword.Size = new Size(243, 28);
        chkMustChangePassword.TabIndex = 9;
        chkMustChangePassword.Text = "يجب تغيير كلمة المرور";
        chkMustChangePassword.UseVisualStyleBackColor = true;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.BackColor = Color.White;
        btnSave.FlatAppearance.BorderColor = Color.Black;
        btnSave.FlatAppearance.BorderSize = 2;
        btnSave.FlatStyle = FlatStyle.Popup;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.Location = new Point(370, 320);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(120, 45);
        btnSave.TabIndex = 10;
        btnSave.Text = "حفظ";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnClose.BackColor = Color.White;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.FlatAppearance.BorderColor = Color.Black;
        btnClose.FlatAppearance.BorderSize = 2;
        btnClose.FlatStyle = FlatStyle.Popup;
        btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnClose.Location = new Point(12, 320);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 11;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmAddUpdateUser
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(600, 400);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(lblUserId);
        Controls.Add(label2);
        Controls.Add(txtUserName);
        Controls.Add(label3);
        Controls.Add(cbRoleId);
        Controls.Add(label4);
        Controls.Add(cbBranchId);
        Controls.Add(chkIsActive);
        Controls.Add(chkMustChangePassword);
        Controls.Add(btnSave);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAddUpdateUser";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إضافة مستخدم جديد";
        Load += frmUserEditor_Load;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblMode;
    private Label label1;
    private Label lblUserId;
    private Label label2;
    private TextBox txtUserName;
    private Label label3;
    private ComboBox cbRoleId;
    private Label label4;
    private ComboBox cbBranchId;
    private CheckBox chkIsActive;
    private CheckBox chkMustChangePassword;
    private Button btnSave;
    private Button btnClose;
    private System.Windows.Forms.ErrorProvider errorProvider;
}
