namespace SalesSystem.Desktop.Screens.Categories.Forms
{
    partial class frmAddUpdateCategory
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
            lblDescription = new Label();
            txtDescription = new TextBox();
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
            lblName.Location = new Point(53, 91);
            lblName.Name = "lblName";
            lblName.Size = new Size(152, 29);
            lblName.TabIndex = 37;
            lblName.Text = "اسم التصنيف:";
            // 
            // txtName
            // 
            txtName.Location = new Point(211, 87);
            txtName.Name = "txtName";
            txtName.Size = new Size(352, 36);
            txtName.TabIndex = 36;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(120, 127);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(85, 29);
            lblDescription.TabIndex = 35;
            lblDescription.Text = "الوصف:";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(211, 127);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(352, 80);
            txtDescription.TabIndex = 34;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatAppearance.BorderColor = Color.Black;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(211, 263);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(187, 35);
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
            btnCancel.Location = new Point(404, 263);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(159, 35);
            btnCancel.TabIndex = 32;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Location = new Point(211, 213);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(88, 33);
            chkIsActive.TabIndex = 31;
            chkIsActive.Text = "نشط";
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.Dock = DockStyle.Top;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(765, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إضافة تصنيف جديد";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmAddUpdateCategory
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(765, 339);
            Controls.Add(lblMode);
            Controls.Add(chkIsActive);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtDescription);
            Controls.Add(lblDescription);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateCategory";
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
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}



