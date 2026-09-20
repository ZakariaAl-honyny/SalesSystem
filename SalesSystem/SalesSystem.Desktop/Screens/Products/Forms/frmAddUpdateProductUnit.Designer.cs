namespace SalesSystem.Desktop.Products.Forms
{
    partial class frmAddUpdateProductUnit
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
            lblProduct = new Label();
            cmbProduct = new ComboBox();
            lblUnit = new Label();
            cmbUnit = new ComboBox();
            lblFactor = new Label();
            txtFactor = new TextBox();
            chkIsBaseUnit = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblMode = new Label();
            errorProvider = new ErrorProvider(components);
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lblProduct
            // 
            lblProduct.AutoSize = true;
            lblProduct.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblProduct.Location = new Point(161, 109);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(75, 24);
            lblProduct.TabIndex = 0;
            lblProduct.Text = "المنتج:";
            // 
            // cmbProduct
            // 
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.Enabled = false;
            cmbProduct.Font = new Font("Tahoma", 10F);
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Location = new Point(251, 106);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(280, 32);
            cmbProduct.TabIndex = 1;
            // 
            // lblUnit
            // 
            lblUnit.AutoSize = true;
            lblUnit.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblUnit.Location = new Point(161, 159);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(79, 24);
            lblUnit.TabIndex = 2;
            lblUnit.Text = "الوحدة:";
            // 
            // cmbUnit
            // 
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.Font = new Font("Tahoma", 10F);
            cmbUnit.FormattingEnabled = true;
            cmbUnit.Location = new Point(251, 156);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(280, 32);
            cmbUnit.TabIndex = 3;
            // 
            // lblFactor
            // 
            lblFactor.AutoSize = true;
            lblFactor.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblFactor.Location = new Point(161, 209);
            lblFactor.Name = "lblFactor";
            lblFactor.Size = new Size(152, 24);
            lblFactor.TabIndex = 4;
            lblFactor.Text = "معامل التحويل:";
            // 
            // txtFactor
            // 
            txtFactor.Font = new Font("Tahoma", 10F);
            txtFactor.Location = new Point(319, 205);
            txtFactor.Name = "txtFactor";
            txtFactor.Size = new Size(100, 32);
            txtFactor.TabIndex = 5;
            txtFactor.Text = "1";
            // 
            // chkIsBaseUnit
            // 
            chkIsBaseUnit.AutoSize = true;
            chkIsBaseUnit.Font = new Font("Tahoma", 10F);
            chkIsBaseUnit.Location = new Point(251, 259);
            chkIsBaseUnit.Name = "chkIsBaseUnit";
            chkIsBaseUnit.Size = new Size(176, 28);
            chkIsBaseUnit.TabIndex = 6;
            chkIsBaseUnit.Text = "الوحدة الأساسية";
            chkIsBaseUnit.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatAppearance.BorderColor = Color.Black;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Location = new Point(251, 309);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(156, 40);
            btnSave.TabIndex = 7;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.Black;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnCancel.Location = new Point(413, 309);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 40);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(251, 35);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(226, 39);
            lblMode.TabIndex = 31;
            lblMode.Text = "إضافة وحدة منتج";
            // 
            // frmAddUpdateProductUnit
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(718, 389);
            Controls.Add(lblMode);
            Controls.Add(lblProduct);
            Controls.Add(cmbProduct);
            Controls.Add(lblUnit);
            Controls.Add(cmbUnit);
            Controls.Add(lblFactor);
            Controls.Add(txtFactor);
            Controls.Add(chkIsBaseUnit);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateProductUnit";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة وحدة منتج";
            Load += frmProductUnitEditor_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.Label lblFactor;
        private System.Windows.Forms.TextBox txtFactor;
        private System.Windows.Forms.CheckBox chkIsBaseUnit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private Label lblMode;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
