namespace SalesSystem.Desktop.Screens.Units.Forms
{
    partial class frmAddUpdateUnit
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
            lblName = new Label();
            txtName = new TextBox();
            lblSymbol = new Label();
            txtSymbol = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            chkIsActive = new CheckBox();
            lblMode = new Label();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(95, 113);
            lblName.Name = "lblName";
            lblName.Size = new Size(139, 29);
            lblName.TabIndex = 37;
            lblName.Text = "اسم الوحدة:";
            // 
            // txtName
            // 
            txtName.Location = new Point(240, 109);
            txtName.Name = "txtName";
            txtName.Size = new Size(290, 36);
            txtName.TabIndex = 36;
            // 
            // lblSymbol
            // 
            lblSymbol.AutoSize = true;
            lblSymbol.Location = new Point(111, 155);
            lblSymbol.Name = "lblSymbol";
            lblSymbol.Size = new Size(123, 29);
            lblSymbol.TabIndex = 35;
            lblSymbol.Text = "الرمز (رمز):";
            lblSymbol.Click += lblSymbol_Click;
            // 
            // txtSymbol
            // 
            txtSymbol.Location = new Point(240, 151);
            txtSymbol.Name = "txtSymbol";
            txtSymbol.Size = new Size(290, 36);
            txtSymbol.TabIndex = 34;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatAppearance.BorderColor = Color.Black;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(240, 276);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(163, 35);
            btnSave.TabIndex = 33;
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
            btnCancel.Location = new Point(409, 276);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(111, 35);
            btnCancel.TabIndex = 32;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(240, 204);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(88, 33);
            chkIsActive.TabIndex = 31;
            chkIsActive.Text = "نشط";
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(206, 9);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(288, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إضافة وحدة جديدة";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmAddUpdateUnit
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(670, 398);
            Controls.Add(lblMode);
            Controls.Add(chkIsActive);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtSymbol);
            Controls.Add(lblSymbol);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateUnit";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblSymbol;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}



