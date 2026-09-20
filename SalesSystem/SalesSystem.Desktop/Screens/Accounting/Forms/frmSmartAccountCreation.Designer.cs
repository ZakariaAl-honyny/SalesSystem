namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmSmartAccountCreation
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
            lblTitle = new Label();
            grpTemplates = new GroupBox();
            rbCashbox = new RadioButton();
            rbBank = new RadioButton();
            rbWallet = new RadioButton();
            rbCustomer = new RadioButton();
            rbSupplier = new RadioButton();
            grpTemplateInfo = new GroupBox();
            lblReason = new Label();
            lblCodeValue = new Label();
            lblCodeLabel = new Label();
            lblParentValue = new Label();
            lblParentLabel = new Label();
            lblNatureValue = new Label();
            lblNatureLabel = new Label();
            lblNameLabel = new Label();
            txtName = new TextBox();
            btnCreate = new Button();
            btnCancel = new Button();
            errorProvider = new ErrorProvider(components);
            grpTemplates.SuspendLayout();
            grpTemplateInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(33, 43, 54);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(560, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إنشاء حساب ذكي";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpTemplates
            // 
            grpTemplates.Controls.Add(rbSupplier);
            grpTemplates.Controls.Add(rbCustomer);
            grpTemplates.Controls.Add(rbWallet);
            grpTemplates.Controls.Add(rbBank);
            grpTemplates.Controls.Add(rbCashbox);
            grpTemplates.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpTemplates.Location = new Point(12, 60);
            grpTemplates.Name = "grpTemplates";
            grpTemplates.RightToLeft = RightToLeft.Yes;
            grpTemplates.Size = new Size(536, 122);
            grpTemplates.TabIndex = 1;
            grpTemplates.TabStop = false;
            grpTemplates.Text = "اختر القالب:";
            // 
            // rbCashbox
            // 
            rbCashbox.AutoSize = true;
            rbCashbox.Checked = true;
            rbCashbox.Font = new Font("Tahoma", 10F);
            rbCashbox.Location = new Point(24, 32);
            rbCashbox.Name = "rbCashbox";
            rbCashbox.RightToLeft = RightToLeft.Yes;
            rbCashbox.Size = new Size(98, 25);
            rbCashbox.TabIndex = 0;
            rbCashbox.TabStop = true;
            rbCashbox.Text = "صندوق";
            rbCashbox.UseVisualStyleBackColor = true;
            // 
            // rbBank
            // 
            rbBank.AutoSize = true;
            rbBank.Font = new Font("Tahoma", 10F);
            rbBank.Location = new Point(208, 32);
            rbBank.Name = "rbBank";
            rbBank.RightToLeft = RightToLeft.Yes;
            rbBank.Size = new Size(82, 25);
            rbBank.TabIndex = 1;
            rbBank.Text = "بنك";
            rbBank.UseVisualStyleBackColor = true;
            // 
            // rbWallet
            // 
            rbWallet.AutoSize = true;
            rbWallet.Font = new Font("Tahoma", 10F);
            rbWallet.Location = new Point(392, 32);
            rbWallet.Name = "rbWallet";
            rbWallet.RightToLeft = RightToLeft.Yes;
            rbWallet.Size = new Size(106, 25);
            rbWallet.TabIndex = 2;
            rbWallet.Text = "محفظة";
            rbWallet.UseVisualStyleBackColor = true;
            // 
            // rbCustomer
            // 
            rbCustomer.AutoSize = true;
            rbCustomer.Font = new Font("Tahoma", 10F);
            rbCustomer.Location = new Point(24, 76);
            rbCustomer.Name = "rbCustomer";
            rbCustomer.RightToLeft = RightToLeft.Yes;
            rbCustomer.Size = new Size(96, 25);
            rbCustomer.TabIndex = 3;
            rbCustomer.Text = "عميل";
            rbCustomer.UseVisualStyleBackColor = true;
            // 
            // rbSupplier
            // 
            rbSupplier.AutoSize = true;
            rbSupplier.Font = new Font("Tahoma", 10F);
            rbSupplier.Location = new Point(208, 76);
            rbSupplier.Name = "rbSupplier";
            rbSupplier.RightToLeft = RightToLeft.Yes;
            rbSupplier.Size = new Size(102, 25);
            rbSupplier.TabIndex = 4;
            rbSupplier.Text = "مورد";
            rbSupplier.UseVisualStyleBackColor = true;
            // 
            // grpTemplateInfo
            // 
            grpTemplateInfo.Controls.Add(lblReason);
            grpTemplateInfo.Controls.Add(lblCodeValue);
            grpTemplateInfo.Controls.Add(lblCodeLabel);
            grpTemplateInfo.Controls.Add(lblParentValue);
            grpTemplateInfo.Controls.Add(lblParentLabel);
            grpTemplateInfo.Controls.Add(lblNatureValue);
            grpTemplateInfo.Controls.Add(lblNatureLabel);
            grpTemplateInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpTemplateInfo.Location = new Point(12, 196);
            grpTemplateInfo.Name = "grpTemplateInfo";
            grpTemplateInfo.RightToLeft = RightToLeft.Yes;
            grpTemplateInfo.Size = new Size(536, 152);
            grpTemplateInfo.TabIndex = 2;
            grpTemplateInfo.TabStop = false;
            grpTemplateInfo.Text = "معلومات القالب (قراءة فقط)";
            // 
            // lblNatureLabel
            // 
            lblNatureLabel.AutoSize = true;
            lblNatureLabel.Font = new Font("Tahoma", 10F);
            lblNatureLabel.Location = new Point(442, 30);
            lblNatureLabel.Name = "lblNatureLabel";
            lblNatureLabel.Size = new Size(80, 21);
            lblNatureLabel.TabIndex = 0;
            lblNatureLabel.Text = "طبيعة الحساب:";
            // 
            // lblNatureValue
            // 
            lblNatureValue.AutoSize = true;
            lblNatureValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblNatureValue.ForeColor = Color.FromArgb(64, 64, 64);
            lblNatureValue.Location = new Point(334, 30);
            lblNatureValue.Name = "lblNatureValue";
            lblNatureValue.Size = new Size(37, 21);
            lblNatureValue.TabIndex = 1;
            lblNatureValue.Text = "أصل";
            // 
            // lblParentLabel
            // 
            lblParentLabel.AutoSize = true;
            lblParentLabel.Font = new Font("Tahoma", 10F);
            lblParentLabel.Location = new Point(442, 60);
            lblParentLabel.Name = "lblParentLabel";
            lblParentLabel.Size = new Size(80, 21);
            lblParentLabel.TabIndex = 2;
            lblParentLabel.Text = "الحساب الأب:";
            // 
            // lblParentValue
            // 
            lblParentValue.AutoSize = true;
            lblParentValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblParentValue.ForeColor = Color.FromArgb(64, 64, 64);
            lblParentValue.Location = new Point(190, 60);
            lblParentValue.Name = "lblParentValue";
            lblParentValue.Size = new Size(198, 21);
            lblParentValue.TabIndex = 3;
            lblParentValue.Text = "1101 — النقد وما في حكمه";
            // 
            // lblCodeLabel
            // 
            lblCodeLabel.AutoSize = true;
            lblCodeLabel.Font = new Font("Tahoma", 10F);
            lblCodeLabel.Location = new Point(442, 90);
            lblCodeLabel.Name = "lblCodeLabel";
            lblCodeLabel.Size = new Size(80, 21);
            lblCodeLabel.TabIndex = 4;
            lblCodeLabel.Text = "الكود المقترح:";
            // 
            // lblCodeValue
            // 
            lblCodeValue.AutoSize = true;
            lblCodeValue.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblCodeValue.ForeColor = Color.Firebrick;
            lblCodeValue.Location = new Point(334, 90);
            lblCodeValue.Name = "lblCodeValue";
            lblCodeValue.Size = new Size(69, 21);
            lblCodeValue.TabIndex = 5;
            lblCodeValue.Text = "1101-05";
            // 
            // lblReason
            // 
            lblReason.Font = new Font("Tahoma", 9.5F);
            lblReason.ForeColor = Color.DimGray;
            lblReason.Location = new Point(14, 118);
            lblReason.Name = "lblReason";
            lblReason.RightToLeft = RightToLeft.Yes;
            lblReason.Size = new Size(508, 30);
            lblReason.TabIndex = 6;
            lblReason.Text = "الشرح: الصندوق: أصل يُستخدم لتتبع النقد المتوفر";
            // 
            // lblNameLabel
            // 
            lblNameLabel.AutoSize = true;
            lblNameLabel.Font = new Font("Tahoma", 10F);
            lblNameLabel.Location = new Point(484, 370);
            lblNameLabel.Name = "lblNameLabel";
            lblNameLabel.Size = new Size(64, 21);
            lblNameLabel.TabIndex = 3;
            lblNameLabel.Text = "اسم الحساب:";
            // 
            // txtName
            // 
            txtName.Font = new Font("Tahoma", 10F);
            txtName.Location = new Point(12, 400);
            txtName.Name = "txtName";
            txtName.RightToLeft = RightToLeft.Yes;
            txtName.Size = new Size(536, 28);
            txtName.TabIndex = 4;
            // 
            // btnCreate
            // 
            btnCreate.BackColor = Color.White;
            btnCreate.FlatStyle = FlatStyle.Popup;
            btnCreate.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreate.Location = new Point(398, 540);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(150, 44);
            btnCreate.TabIndex = 5;
            btnCreate.Text = "إنشاء";
            btnCreate.UseVisualStyleBackColor = false;
            btnCreate.Click += btnCreate_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(12, 540);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 44);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.RightToLeft = true;
            // 
            // frmSmartAccountCreation
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(560, 600);
            Controls.Add(btnCancel);
            Controls.Add(btnCreate);
            Controls.Add(txtName);
            Controls.Add(lblNameLabel);
            Controls.Add(grpTemplateInfo);
            Controls.Add(grpTemplates);
            Controls.Add(lblTitle);
            AcceptButton = btnCreate;
            CancelButton = btnCancel;
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSmartAccountCreation";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إنشاء حساب ذكي";
            grpTemplates.ResumeLayout(false);
            grpTemplates.PerformLayout();
            grpTemplateInfo.ResumeLayout(false);
            grpTemplateInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpTemplates;
        private System.Windows.Forms.RadioButton rbCashbox;
        private System.Windows.Forms.RadioButton rbBank;
        private System.Windows.Forms.RadioButton rbWallet;
        private System.Windows.Forms.RadioButton rbCustomer;
        private System.Windows.Forms.RadioButton rbSupplier;
        private System.Windows.Forms.GroupBox grpTemplateInfo;
        private System.Windows.Forms.Label lblNatureLabel;
        private System.Windows.Forms.Label lblNatureValue;
        private System.Windows.Forms.Label lblParentLabel;
        private System.Windows.Forms.Label lblParentValue;
        private System.Windows.Forms.Label lblCodeLabel;
        private System.Windows.Forms.Label lblCodeValue;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.Label lblNameLabel;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}