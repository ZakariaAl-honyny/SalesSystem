namespace SalesSystem.Desktop.Suppliers.Forms
{
    partial class frmAddUpdateSupplier
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.errorProvider = new System.Windows.Forms.ErrorProvider();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblOpeningBalance = new System.Windows.Forms.Label();
            this.txtOpeningBalance = new SalesSystem.Desktop.Controls.Common.MoneyTextBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            // lblMode
            this.lblMode = new System.Windows.Forms.Label();
            this.lblMode.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblMode.AutoSize = true;
            this.lblMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblMode.ForeColor = System.Drawing.Color.Firebrick;
            this.lblMode.Location = new System.Drawing.Point(85, 8);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(250, 39);
            this.lblMode.TabIndex = 30;
            this.lblMode.Text = "إضافة مورد جديد";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            int labelX = 20, inputX = 130, y = 65, spacing = 40;

            this.lblName.Text = "��� ������:";
            this.lblName.Location = new System.Drawing.Point(labelX, y);
            this.txtName.Location = new System.Drawing.Point(inputX, y);
            this.txtName.Size = new System.Drawing.Size(250, 27);
            
            y += spacing;
            this.lblPhone.Text = "��� ������:";
            this.lblPhone.Location = new System.Drawing.Point(labelX, y);
            this.txtPhone.Location = new System.Drawing.Point(inputX, y);
            this.txtPhone.Size = new System.Drawing.Size(250, 27);

            y += spacing;
            this.lblEmail.Text = "������ ����������:";
            this.lblEmail.Location = new System.Drawing.Point(labelX, y);
            this.txtEmail.Location = new System.Drawing.Point(inputX, y);
            this.txtEmail.Size = new System.Drawing.Size(250, 27);

            y += spacing;
            this.lblAddress.Text = "�������:";
            this.lblAddress.Location = new System.Drawing.Point(labelX, y);
            this.txtAddress.Location = new System.Drawing.Point(inputX, y);
            this.txtAddress.Size = new System.Drawing.Size(250, 27);

            y += spacing;
            this.lblOpeningBalance.Text = "������ ���������:";
            this.lblOpeningBalance.Location = new System.Drawing.Point(labelX, y);
            this.txtOpeningBalance.Location = new System.Drawing.Point(inputX, y);
            this.txtOpeningBalance.Size = new System.Drawing.Size(100, 27);

            this.chkIsActive.Text = "���";
            this.chkIsActive.Location = new System.Drawing.Point(210, y);

            y += 60;
            this.btnSave.Text = "\u062d\u0641\u0638";
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(280, y);
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Text = "\u0625\u0644\u063a\u0627\u0621";
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCancel.FlatAppearance.BorderSize = 2;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(170, y);
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ClientSize = new System.Drawing.Size(420, y + 60);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblMode, lblName, txtName, lblPhone, txtPhone, lblEmail, txtEmail,
                lblAddress, txtAddress, lblOpeningBalance, txtOpeningBalance,
                chkIsActive, btnSave, btnCancel
            });
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SupplierEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblName, lblPhone, lblEmail, lblAddress, lblOpeningBalance;
        private System.Windows.Forms.TextBox txtName, txtPhone, txtEmail, txtAddress;
        private SalesSystem.Desktop.Controls.Common.MoneyTextBox txtOpeningBalance;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnSave, btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
