namespace SalesSystem.Desktop.Products.Forms
{
    partial class frmAddUpdateProductCategory
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
            lblNameAr = new Label();
            txtNameAr = new TextBox();
            lblNameEn = new Label();
            txtNameEn = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblParentCategory = new Label();
            cmbParentCategory = new ComboBox();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            lblMode = new Label();
            errorProvider = new ErrorProvider(components);
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lblNameAr
            // 
            lblNameAr.AutoSize = true;
            lblNameAr.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNameAr.Location = new Point(115, 92);
            lblNameAr.Name = "lblNameAr";
            lblNameAr.Size = new Size(151, 24);
            lblNameAr.TabIndex = 0;
            lblNameAr.Text = "الاسم بالعربية:";
            // 
            // txtNameAr
            // 
            txtNameAr.Font = new Font("Tahoma", 10F);
            txtNameAr.Location = new Point(272, 89);
            txtNameAr.Name = "txtNameAr";
            txtNameAr.Size = new Size(321, 32);
            txtNameAr.TabIndex = 1;
            // 
            // lblNameEn
            // 
            lblNameEn.AutoSize = true;
            lblNameEn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNameEn.Location = new Point(90, 142);
            lblNameEn.Name = "lblNameEn";
            lblNameEn.Size = new Size(176, 24);
            lblNameEn.TabIndex = 2;
            lblNameEn.Text = "الاسم بالإنجليزية:";
            // 
            // txtNameEn
            // 
            txtNameEn.Font = new Font("Tahoma", 10F);
            txtNameEn.Location = new Point(272, 139);
            txtNameEn.Name = "txtNameEn";
            txtNameEn.Size = new Size(321, 32);
            txtNameEn.TabIndex = 3;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblDescription.Location = new Point(185, 192);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(81, 24);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "الوصف:";
            // 
            // txtDescription
            // 
            txtDescription.Font = new Font("Tahoma", 10F);
            txtDescription.Location = new Point(272, 189);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(321, 60);
            txtDescription.TabIndex = 5;
            // 
            // lblParentCategory
            // 
            lblParentCategory.AutoSize = true;
            lblParentCategory.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblParentCategory.Location = new Point(126, 267);
            lblParentCategory.Name = "lblParentCategory";
            lblParentCategory.Size = new Size(140, 24);
            lblParentCategory.TabIndex = 6;
            lblParentCategory.Text = "التصنيف الأب:";
            // 
            // cmbParentCategory
            // 
            cmbParentCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParentCategory.Font = new Font("Tahoma", 10F);
            cmbParentCategory.FormattingEnabled = true;
            cmbParentCategory.Location = new Point(272, 264);
            cmbParentCategory.Name = "cmbParentCategory";
            cmbParentCategory.Size = new Size(321, 32);
            cmbParentCategory.TabIndex = 7;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Font = new Font("Tahoma", 10F);
            chkIsActive.Location = new Point(272, 314);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(77, 28);
            chkIsActive.TabIndex = 8;
            chkIsActive.Text = "نشط";
            chkIsActive.TextAlign = ContentAlignment.MiddleRight;
            chkIsActive.Visible = false;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatAppearance.BorderColor = Color.Black;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Location = new Point(272, 362);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(176, 40);
            btnSave.TabIndex = 9;
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
            btnCancel.Location = new Point(454, 362);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(139, 40);
            btnCancel.TabIndex = 10;
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
            lblMode.Location = new Point(275, 20);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(260, 39);
            lblMode.TabIndex = 30;
            lblMode.Text = "إضافة تصنيف منتج";
            // 
            // frmAddUpdateProductCategory
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(792, 425);
            Controls.Add(lblMode);
            Controls.Add(lblNameAr);
            Controls.Add(txtNameAr);
            Controls.Add(lblNameEn);
            Controls.Add(txtNameEn);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(lblParentCategory);
            Controls.Add(cmbParentCategory);
            Controls.Add(chkIsActive);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Font = new Font("Tahoma", 12F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateProductCategory";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة تصنيف منتج";
            Load += frmProductCategoryEditor_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Label lblNameAr;
        private System.Windows.Forms.TextBox txtNameAr;
        private System.Windows.Forms.Label lblNameEn;
        private System.Windows.Forms.TextBox txtNameEn;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblParentCategory;
        private System.Windows.Forms.ComboBox cmbParentCategory;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Label lblMode;
    }
}
