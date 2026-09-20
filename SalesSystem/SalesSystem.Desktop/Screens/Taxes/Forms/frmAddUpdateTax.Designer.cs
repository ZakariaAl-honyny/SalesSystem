namespace SalesSystem.Desktop.Screens.Taxes.Forms
{
    partial class frmAddUpdateTax
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
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.lblMode = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblRate = new System.Windows.Forms.Label();
            this.numRate = new System.Windows.Forms.NumericUpDown();
            this.lblTaxType = new System.Windows.Forms.Label();
            this.cboTaxType = new System.Windows.Forms.ComboBox();
            this.chkIsDefault = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.BackColor = System.Drawing.Color.DarkSlateGray;
            this.lblMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMode.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold);
            this.lblMode.ForeColor = System.Drawing.Color.White;
            this.lblMode.Location = new System.Drawing.Point(0, 0);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(420, 42);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "إضافة ضريبة جديدة";
            this.lblMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblName.Location = new System.Drawing.Point(330, 65);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(68, 18);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "اسم الضريبة:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtName.Location = new System.Drawing.Point(40, 62);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(280, 26);
            this.txtName.TabIndex = 2;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCode.Location = new System.Drawing.Point(332, 102);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(66, 18);
            this.lblCode.TabIndex = 3;
            this.lblCode.Text = "الرمز:";
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtCode.Location = new System.Drawing.Point(40, 99);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(120, 26);
            this.txtCode.TabIndex = 4;
            // 
            // lblRate
            // 
            this.lblRate.AutoSize = true;
            this.lblRate.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRate.Location = new System.Drawing.Point(332, 140);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(51, 18);
            this.lblRate.TabIndex = 5;
            this.lblRate.Text = "النسبة:";
            // 
            // numRate
            // 
            this.numRate.DecimalPlaces = 2;
            this.numRate.Font = new System.Drawing.Font("Tahoma", 9F);
            this.numRate.Location = new System.Drawing.Point(40, 137);
            this.numRate.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numRate.Name = "numRate";
            this.numRate.Size = new System.Drawing.Size(120, 26);
            this.numRate.TabIndex = 6;
            // 
            // lblTaxType
            // 
            this.lblTaxType.AutoSize = true;
            this.lblTaxType.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTaxType.Location = new System.Drawing.Point(332, 178);
            this.lblTaxType.Name = "lblTaxType";
            this.lblTaxType.Size = new System.Drawing.Size(49, 18);
            this.lblTaxType.TabIndex = 7;
            this.lblTaxType.Text = "النوع:";
            // 
            // cboTaxType
            // 
            this.cboTaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTaxType.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cboTaxType.FormattingEnabled = true;
            this.cboTaxType.Items.AddRange(new object[] { "نسبة مئوية", "مبلغ ثابت" });
            this.cboTaxType.Location = new System.Drawing.Point(40, 175);
            this.cboTaxType.Name = "cboTaxType";
            this.cboTaxType.Size = new System.Drawing.Size(280, 26);
            this.cboTaxType.TabIndex = 8;
            // 
            // chkIsDefault
            // 
            this.chkIsDefault.AutoSize = true;
            this.chkIsDefault.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkIsDefault.Location = new System.Drawing.Point(170, 217);
            this.chkIsDefault.Name = "chkIsDefault";
            this.chkIsDefault.Size = new System.Drawing.Size(130, 22);
            this.chkIsDefault.TabIndex = 9;
            this.chkIsDefault.Text = "افتراضي";
            this.chkIsDefault.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnSave.Location = new System.Drawing.Point(220, 255);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "حفظ";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.btnCancel.Location = new System.Drawing.Point(70, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "إلغاء";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmTaxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(420, 310);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkIsDefault);
            this.Controls.Add(this.cboTaxType);
            this.Controls.Add(this.lblTaxType);
            this.Controls.Add(this.numRate);
            this.Controls.Add(this.lblRate);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblMode);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTaxEditor";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إضافة ضريبة جديدة";
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).EndInit();
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblRate;
        private System.Windows.Forms.NumericUpDown numRate;
        private System.Windows.Forms.Label lblTaxType;
        private System.Windows.Forms.ComboBox cboTaxType;
        private System.Windows.Forms.CheckBox chkIsDefault;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
