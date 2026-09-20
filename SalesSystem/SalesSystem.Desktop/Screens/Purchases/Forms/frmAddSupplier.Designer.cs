namespace SalesSystem.Desktop.Screens.Purchases.Forms
{
    partial class frmAddSupplier
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
            this.components = new System.ComponentModel.Container();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblTaxNumber = new System.Windows.Forms.Label();
            this.txtTaxNumber = new System.Windows.Forms.TextBox();
            this.lblCreditLimit = new System.Windows.Forms.Label();
            this.nudCreditLimit = new System.Windows.Forms.NumericUpDown();
            this.lblAccount = new System.Windows.Forms.Label();
            this.lblAccountInfo = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudCreditLimit)).BeginInit();
            this.SuspendLayout();

            // lblTitle — شريط العنوان (Dock=Top)
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Height = 52;
            this.lblTitle.BackColor = System.Drawing.Color.Firebrick;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "إضافة مورد";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            int labelX = 20, inputX = 150, y = 70, spacing = 42;

            // 1) الاسم (مطلوب)
            this.lblName.Text = "الاسم *";
            this.lblName.Location = new System.Drawing.Point(labelX, y);
            this.lblName.Name = "lblName";
            this.txtName.Location = new System.Drawing.Point(inputX, y);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(280, 27);

            y += spacing;

            // 2) الهاتف (مطلوب)
            this.lblPhone.Text = "الهاتف *";
            this.lblPhone.Location = new System.Drawing.Point(labelX, y);
            this.lblPhone.Name = "lblPhone";
            this.txtPhone.Location = new System.Drawing.Point(inputX, y);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(280, 27);

            y += spacing;

            // 3) البريد الإلكتروني
            this.lblEmail.Text = "البريد الإلكتروني";
            this.lblEmail.Location = new System.Drawing.Point(labelX, y);
            this.lblEmail.Name = "lblEmail";
            this.txtEmail.Location = new System.Drawing.Point(inputX, y);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(280, 27);

            y += spacing;

            // 4) العنوان
            this.lblAddress.Text = "العنوان";
            this.lblAddress.Location = new System.Drawing.Point(labelX, y);
            this.lblAddress.Name = "lblAddress";
            this.txtAddress.Location = new System.Drawing.Point(inputX, y);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(280, 27);

            y += spacing;

            // 5) الرقم الضريبي
            this.lblTaxNumber.Text = "الرقم الضريبي";
            this.lblTaxNumber.Location = new System.Drawing.Point(labelX, y);
            this.lblTaxNumber.Name = "lblTaxNumber";
            this.txtTaxNumber.Location = new System.Drawing.Point(inputX, y);
            this.txtTaxNumber.Name = "txtTaxNumber";
            this.txtTaxNumber.Size = new System.Drawing.Size(280, 27);

            y += spacing;

            // 6) حد الائتمان (NumericUpDown — منزلتان عشريتان)
            this.lblCreditLimit.Text = "حد الائتمان";
            this.lblCreditLimit.Location = new System.Drawing.Point(labelX, y);
            this.lblCreditLimit.Name = "lblCreditLimit";
            this.nudCreditLimit.DecimalPlaces = 2;
            this.nudCreditLimit.Increment = 100;
            this.nudCreditLimit.Location = new System.Drawing.Point(inputX, y);
            this.nudCreditLimit.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            this.nudCreditLimit.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.nudCreditLimit.Name = "nudCreditLimit";
            this.nudCreditLimit.Size = new System.Drawing.Size(200, 27);
            this.nudCreditLimit.TabIndex = 5;
            this.nudCreditLimit.ThousandsSeparator = true;
            this.nudCreditLimit.Value = new decimal(new int[] { 0, 0, 0, 0 });

            y += spacing;

            // 7) الحساب — تُنشأ تلقائياً (تسمية توضيحية للقراءة فقط)
            this.lblAccount.Text = "الحساب";
            this.lblAccount.Location = new System.Drawing.Point(labelX, y);
            this.lblAccount.Name = "lblAccount";
            this.lblAccountInfo.AutoSize = false;
            this.lblAccountInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblAccountInfo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Italic);
            this.lblAccountInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblAccountInfo.Location = new System.Drawing.Point(inputX, y + 23);
            this.lblAccountInfo.Name = "lblAccountInfo";
            this.lblAccountInfo.Size = new System.Drawing.Size(350, 40);
            this.lblAccountInfo.Text = "سيُنشأ حساب تلقائياً تحت 1103 ذمم موردين";

            y += 78;

            // 8) ملاحظات (متعدد الأسطر)
            this.lblNotes.Text = "ملاحظات";
            this.lblNotes.Location = new System.Drawing.Point(labelX, y);
            this.lblNotes.Name = "lblNotes";
            this.txtNotes.AcceptsReturn = true;
            this.txtNotes.Location = new System.Drawing.Point(inputX, y);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(350, 85);

            y += 105;

            // ── Footer: حفظ + إلغاء ──
            this.btnSave.Text = "حفظ";
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(390, y);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 38);
            this.btnSave.TabIndex = 6;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Text = "إلغاء";
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCancel.FlatAppearance.BorderSize = 2;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(270, y);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 38);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ── Form ──
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(520, 600);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitle,
                lblName, txtName,
                lblPhone, txtPhone,
                lblEmail, txtEmail,
                lblAddress, txtAddress,
                lblTaxNumber, txtTaxNumber,
                lblCreditLimit, nudCreditLimit,
                lblAccount, lblAccountInfo,
                lblNotes, txtNotes,
                btnSave, btnCancel
            });
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddSupplier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إضافة مورد";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;

            ((System.ComponentModel.ISupportInitialize)(this.nudCreditLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName, lblPhone, lblEmail, lblAddress, lblTaxNumber, lblCreditLimit, lblAccount, lblAccountInfo, lblNotes;
        private System.Windows.Forms.TextBox txtName, txtPhone, txtEmail, txtAddress, txtTaxNumber, txtNotes;
        private System.Windows.Forms.NumericUpDown nudCreditLimit;
        private System.Windows.Forms.Button btnSave, btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}