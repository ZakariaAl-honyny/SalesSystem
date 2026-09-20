namespace SalesSystem.Desktop.Screens.Banks.Forms;

partial class frmAddUpdateBank
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        components?.Dispose();base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        errorProvider = new ErrorProvider(components);
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        lblMode = new Label();
        label1 = new Label();
        txtName = new TextBox();
        label2 = new Label();
        txtAccountNumber = new TextBox();
        label3 = new Label();
        txtIban = new TextBox();
        btnSave = new Button();
        btnClose = new Button();
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
        lblMode.Location = new Point(211, 15);
        lblMode.Name = "lblMode";
        lblMode.Size = new Size(247, 34);
        lblMode.TabIndex = 30;
        lblMode.Text = "إضافة بنك جديد";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(47, 74);
        label1.Name = "label1";
        label1.Size = new Size(125, 24);
        label1.TabIndex = 0;
        label1.Text = "اسم البنك *";
        // 
        // txtName
        // 
        txtName.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtName.Location = new Point(178, 71);
        txtName.Name = "txtName";
        txtName.RightToLeft = RightToLeft.Yes;
        txtName.Size = new Size(350, 32);
        txtName.TabIndex = 1;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label2.Location = new Point(44, 124);
        label2.Name = "label2";
        label2.Size = new Size(128, 24);
        label2.TabIndex = 2;
        label2.Text = "رقم الحساب";
        // 
        // txtAccountNumber
        // 
        txtAccountNumber.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtAccountNumber.Location = new Point(178, 121);
        txtAccountNumber.Name = "txtAccountNumber";
        txtAccountNumber.RightToLeft = RightToLeft.Yes;
        txtAccountNumber.Size = new Size(350, 32);
        txtAccountNumber.TabIndex = 2;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label3.Location = new Point(104, 174);
        label3.Name = "label3";
        label3.Size = new Size(68, 24);
        label3.TabIndex = 4;
        label3.Text = "الآيبان";
        // 
        // txtIban
        // 
        txtIban.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtIban.Location = new Point(178, 171);
        txtIban.Name = "txtIban";
        txtIban.RightToLeft = RightToLeft.Yes;
        txtIban.Size = new Size(350, 32);
        txtIban.TabIndex = 3;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.BackColor = Color.White;
        btnSave.FlatAppearance.BorderColor = Color.Black;
        btnSave.FlatAppearance.BorderSize = 2;
        btnSave.FlatStyle = FlatStyle.Popup;
        btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnSave.Location = new Point(420, 290);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(120, 45);
        btnSave.TabIndex = 4;
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
        btnClose.Location = new Point(12, 290);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(120, 45);
        btnClose.TabIndex = 5;
        btnClose.Text = "إلغاء";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;
        // 
        // frmAddUpdateBank
        // 
        AcceptButton = btnSave;
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.WhiteSmoke;
        CancelButton = btnClose;
        ClientSize = new Size(600, 360);
        Controls.Add(lblMode);
        Controls.Add(label1);
        Controls.Add(txtName);
        Controls.Add(label2);
        Controls.Add(txtAccountNumber);
        Controls.Add(label3);
        Controls.Add(txtIban);
        Controls.Add(btnSave);
        Controls.Add(btnClose);
        Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAddUpdateBank";
        RightToLeft = RightToLeft.Yes;
        RightToLeftLayout = true;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "إضافة بنك جديد";
        // errorProvider
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        errorProvider.RightToLeft = true;
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.ErrorProvider errorProvider;
    private Label lblMode;
    private Label label1;
    private TextBox txtName;
    private Label label2;
    private TextBox txtAccountNumber;
    private Label label3;
    private TextBox txtIban;
    private Button btnSave;
    private Button btnClose;
}
