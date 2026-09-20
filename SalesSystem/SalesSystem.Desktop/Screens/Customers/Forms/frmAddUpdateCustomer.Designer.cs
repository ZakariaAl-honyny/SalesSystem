namespace SalesSystem.Desktop.Screens.Customers.Forms
{
    partial class frmAddUpdateCustomer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            lblName = new Label();
            txtName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            lblOpeningBalance = new Label();
            txtOpeningBalance = new SalesSystem.Desktop.Controls.Common.MoneyTextBox();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode = new Label();
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(155, 8);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(250, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إضافة عميل جديد";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(65, 50);
            lblName.Name = "lblName";
            lblName.Size = new Size(148, 29);
            lblName.TabIndex = 12;
            lblName.Text = "اسم العميل :";
            // 
            // txtName
            // 
            txtName.Location = new Point(223, 47);
            txtName.Name = "txtName";
            txtName.Size = new Size(250, 36);
            txtName.TabIndex = 11;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(79, 90);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(134, 29);
            lblPhone.TabIndex = 10;
            lblPhone.Text = "رقم الهاتف :";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(223, 87);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(250, 36);
            txtPhone.TabIndex = 9;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(25, 134);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(188, 29);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "البريد الإلكتروني :";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(223, 127);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(250, 36);
            txtEmail.TabIndex = 7;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(118, 170);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(95, 29);
            lblAddress.TabIndex = 6;
            lblAddress.Text = "العنوان :";
            // 
            // txtAddress
            // 
            txtAddress.Location = new Point(223, 167);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(250, 36);
            txtAddress.TabIndex = 5;
            // 
            // lblOpeningBalance
            // 
            lblOpeningBalance.AutoSize = true;
            lblOpeningBalance.Location = new Point(20, 211);
            lblOpeningBalance.Name = "lblOpeningBalance";
            lblOpeningBalance.Size = new Size(193, 29);
            lblOpeningBalance.TabIndex = 4;
            lblOpeningBalance.Text = "الرصيد الافتتاحي :";
            // 
            // txtOpeningBalance
            // 
            txtOpeningBalance.Location = new Point(223, 207);
            txtOpeningBalance.Name = "txtOpeningBalance";
            txtOpeningBalance.Size = new Size(100, 36);
            txtOpeningBalance.TabIndex = 3;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(329, 210);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(88, 33);
            chkIsActive.TabIndex = 2;
            chkIsActive.Text = "نشط";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatAppearance.BorderColor = Color.Black;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(223, 265);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(129, 35);
            btnSave.TabIndex = 1;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderColor = Color.Black;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(358, 265);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(115, 35);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmAddUpdateCustomer
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(565, 340);
            Controls.Add(lblMode);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkIsActive);
            Controls.Add(txtOpeningBalance);
            Controls.Add(lblOpeningBalance);
            Controls.Add(txtAddress);
            Controls.Add(lblAddress);
            Controls.Add(txtEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtPhone);
            Controls.Add(lblPhone);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateCustomer";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Load += frmAddUpdateCustomer_Load;
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblMode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblOpeningBalance;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtAddress;
        private SalesSystem.Desktop.Controls.Common.MoneyTextBox txtOpeningBalance;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
