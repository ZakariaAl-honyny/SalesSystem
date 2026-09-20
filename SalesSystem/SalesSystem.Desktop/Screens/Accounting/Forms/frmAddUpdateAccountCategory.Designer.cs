namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmAddUpdateAccountCategory
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
            lblCategoryId = new Label();
            label2 = new Label();
            txtName = new TextBox();
            label3 = new Label();
            txtDescription = new TextBox();
            btnSave = new Button();
            btnClose = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();

            // lblMode
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.FromArgb(64, 64, 64);
            lblMode.Location = new Point(14, 14);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(340, 33);
            lblMode.TabIndex = 0;
            lblMode.Text = "إضافة تصنيف حسابات جديد";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;

            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F);
            label1.Location = new Point(440, 70);
            label1.Name = "label1";
            label1.Size = new Size(70, 21);
            label1.TabIndex = 1;
            label1.Text = "المعرف:";

            // lblCategoryId
            lblCategoryId.AutoSize = true;
            lblCategoryId.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblCategoryId.ForeColor = Color.FromArgb(64, 64, 64);
            lblCategoryId.Location = new Point(400, 70);
            lblCategoryId.Name = "lblCategoryId";
            lblCategoryId.Size = new Size(34, 21);
            lblCategoryId.TabIndex = 2;
            lblCategoryId.Text = "---";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F);
            label2.Location = new Point(500, 120);
            label2.Name = "label2";
            label2.Size = new Size(57, 21);
            label2.TabIndex = 3;
            label2.Text = "الاسم:";

            // txtName
            txtName.Font = new Font("Tahoma", 10F);
            txtName.Location = new Point(200, 117);
            txtName.Name = "txtName";
            txtName.RightToLeft = RightToLeft.Yes;
            txtName.Size = new Size(294, 28);
            txtName.TabIndex = 4;
            txtName.Validating += txtName_Validating;

            // label3
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F);
            label3.Location = new Point(490, 170);
            label3.Name = "label3";
            label3.Size = new Size(57, 21);
            label3.TabIndex = 5;
            label3.Text = "الوصف:";

            // txtDescription
            txtDescription.Font = new Font("Tahoma", 10F);
            txtDescription.Location = new Point(200, 167);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.RightToLeft = RightToLeft.Yes;
            txtDescription.Size = new Size(284, 80);
            txtDescription.TabIndex = 6;

            // btnSave
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(330, 280);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(152, 40);
            btnSave.TabIndex = 7;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // btnClose
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(12, 280);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(80, 40);
            btnClose.TabIndex = 8;
            btnClose.Text = "إلغاء";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // errorProvider
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.RightToLeft = true;

            // frmAccountCategoryEditor
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(584, 336);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(txtDescription);
            Controls.Add(label3);
            Controls.Add(txtName);
            Controls.Add(label2);
            Controls.Add(lblCategoryId);
            Controls.Add(label1);
            Controls.Add(lblMode);
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmAccountCategoryEditor";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة تصنيف حسابات جديد";
            Load += frmAccountCategoryEditor_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCategoryId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
