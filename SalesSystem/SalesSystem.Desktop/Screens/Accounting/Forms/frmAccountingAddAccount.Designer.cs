namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmAccountingAddAccount
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
            grpDetails = new GroupBox();
            lblHint = new Label();
            lblSuggestedCode = new Label();
            lblCodeLabel = new Label();
            chkAllowPosting = new CheckBox();
            cmbLevel = new ComboBox();
            lblLevelLabel = new Label();
            cmbNature = new ComboBox();
            lblNatureLabel = new Label();
            cmbParent = new ComboBox();
            lblParentLabel = new Label();
            txtAccountName = new TextBox();
            lblNameLabel = new Label();
            lblExample = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            errorProvider = new ErrorProvider(components);
            grpDetails.SuspendLayout();
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
            lblTitle.Size = new Size(718, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إضافة حساب";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpDetails
            // 
            grpDetails.Controls.Add(lblHint);
            grpDetails.Controls.Add(lblSuggestedCode);
            grpDetails.Controls.Add(lblCodeLabel);
            grpDetails.Controls.Add(chkAllowPosting);
            grpDetails.Controls.Add(cmbLevel);
            grpDetails.Controls.Add(lblLevelLabel);
            grpDetails.Controls.Add(cmbNature);
            grpDetails.Controls.Add(lblNatureLabel);
            grpDetails.Controls.Add(cmbParent);
            grpDetails.Controls.Add(lblParentLabel);
            grpDetails.Controls.Add(txtAccountName);
            grpDetails.Controls.Add(lblNameLabel);
            grpDetails.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpDetails.Location = new Point(42, 51);
            grpDetails.Name = "grpDetails";
            grpDetails.RightToLeft = RightToLeft.Yes;
            grpDetails.Size = new Size(611, 402);
            grpDetails.TabIndex = 1;
            grpDetails.TabStop = false;
            grpDetails.Text = "بيانات الحساب";
            // 
            // lblHint
            // 
            lblHint.Font = new Font("Tahoma", 9.5F);
            lblHint.ForeColor = Color.DimGray;
            lblHint.Location = new Point(12, 286);
            lblHint.Name = "lblHint";
            lblHint.RightToLeft = RightToLeft.Yes;
            lblHint.Size = new Size(472, 40);
            lblHint.TabIndex = 11;
            lblHint.Text = "يتحدث الكود المقترح تلقائياً عند تغيير الحساب الأب، ويمكن تعديل طبيعة الحساب يدوياً حسب الحاجة.";
            // 
            // lblSuggestedCode
            // 
            lblSuggestedCode.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSuggestedCode.ForeColor = Color.Firebrick;
            lblSuggestedCode.Location = new Point(12, 246);
            lblSuggestedCode.Name = "lblSuggestedCode";
            lblSuggestedCode.Size = new Size(356, 26);
            lblSuggestedCode.TabIndex = 10;
            lblSuggestedCode.Text = "1101-06";
            lblSuggestedCode.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCodeLabel
            // 
            lblCodeLabel.AutoSize = true;
            lblCodeLabel.Font = new Font("Tahoma", 10F);
            lblCodeLabel.Location = new Point(380, 248);
            lblCodeLabel.Name = "lblCodeLabel";
            lblCodeLabel.Size = new Size(123, 24);
            lblCodeLabel.TabIndex = 9;
            lblCodeLabel.Text = "الكود المقترح:";
            // 
            // chkAllowPosting
            // 
            chkAllowPosting.AutoSize = true;
            chkAllowPosting.Checked = true;
            chkAllowPosting.CheckState = CheckState.Checked;
            chkAllowPosting.Font = new Font("Tahoma", 10F);
            chkAllowPosting.Location = new Point(12, 204);
            chkAllowPosting.Name = "chkAllowPosting";
            chkAllowPosting.RightToLeft = RightToLeft.Yes;
            chkAllowPosting.Size = new Size(162, 28);
            chkAllowPosting.TabIndex = 8;
            chkAllowPosting.Text = "يسمح بالترحيل";
            chkAllowPosting.UseVisualStyleBackColor = true;
            // 
            // cmbLevel
            // 
            cmbLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLevel.Font = new Font("Tahoma", 10F);
            cmbLevel.FormattingEnabled = true;
            cmbLevel.Location = new Point(12, 160);
            cmbLevel.Name = "cmbLevel";
            cmbLevel.RightToLeft = RightToLeft.Yes;
            cmbLevel.Size = new Size(356, 32);
            cmbLevel.TabIndex = 7;
            // 
            // lblLevelLabel
            // 
            lblLevelLabel.AutoSize = true;
            lblLevelLabel.Font = new Font("Tahoma", 10F);
            lblLevelLabel.Location = new Point(380, 162);
            lblLevelLabel.Name = "lblLevelLabel";
            lblLevelLabel.Size = new Size(94, 24);
            lblLevelLabel.TabIndex = 6;
            lblLevelLabel.Text = "المستوى:";
            // 
            // cmbNature
            // 
            cmbNature.Font = new Font("Tahoma", 10F);
            cmbNature.FormattingEnabled = true;
            cmbNature.Location = new Point(12, 116);
            cmbNature.Name = "cmbNature";
            cmbNature.RightToLeft = RightToLeft.Yes;
            cmbNature.Size = new Size(356, 32);
            cmbNature.TabIndex = 5;
            // 
            // lblNatureLabel
            // 
            lblNatureLabel.AutoSize = true;
            lblNatureLabel.Font = new Font("Tahoma", 10F);
            lblNatureLabel.Location = new Point(380, 118);
            lblNatureLabel.Name = "lblNatureLabel";
            lblNatureLabel.Size = new Size(139, 24);
            lblNatureLabel.TabIndex = 4;
            lblNatureLabel.Text = "طبيعة الحساب:";
            // 
            // cmbParent
            // 
            cmbParent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParent.Font = new Font("Tahoma", 10F);
            cmbParent.FormattingEnabled = true;
            cmbParent.Location = new Point(12, 72);
            cmbParent.Name = "cmbParent";
            cmbParent.RightToLeft = RightToLeft.Yes;
            cmbParent.Size = new Size(356, 32);
            cmbParent.TabIndex = 3;
            cmbParent.SelectedIndexChanged += cmbParent_SelectedIndexChanged;
            // 
            // lblParentLabel
            // 
            lblParentLabel.AutoSize = true;
            lblParentLabel.Font = new Font("Tahoma", 10F);
            lblParentLabel.Location = new Point(380, 74);
            lblParentLabel.Name = "lblParentLabel";
            lblParentLabel.Size = new Size(124, 24);
            lblParentLabel.TabIndex = 2;
            lblParentLabel.Text = "الحساب الأب:";
            // 
            // txtAccountName
            // 
            txtAccountName.Font = new Font("Tahoma", 10F);
            txtAccountName.Location = new Point(12, 28);
            txtAccountName.Name = "txtAccountName";
            txtAccountName.RightToLeft = RightToLeft.Yes;
            txtAccountName.Size = new Size(356, 32);
            txtAccountName.TabIndex = 1;
            // 
            // lblNameLabel
            // 
            lblNameLabel.AutoSize = true;
            lblNameLabel.Font = new Font("Tahoma", 10F);
            lblNameLabel.Location = new Point(380, 30);
            lblNameLabel.Name = "lblNameLabel";
            lblNameLabel.Size = new Size(208, 24);
            lblNameLabel.TabIndex = 0;
            lblNameLabel.Text = "اسم الحساب (مطلوب):";
            // 
            // lblExample
            // 
            lblExample.Font = new Font("Tahoma", 9.5F);
            lblExample.ForeColor = Color.DimGray;
            lblExample.Location = new Point(87, 456);
            lblExample.Name = "lblExample";
            lblExample.RightToLeft = RightToLeft.Yes;
            lblExample.Size = new Size(496, 36);
            lblExample.TabIndex = 12;
            lblExample.Text = "مثال: اختيار الحساب الأب 1101 (النقد) يقترح الكود 1101-06 تلقائياً.";
            lblExample.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(398, 505);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 44);
            btnSave.TabIndex = 13;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(12, 505);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 44);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            errorProvider.RightToLeft = true;
            // 
            // frmAccountingAddAccount
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(718, 603);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(lblExample);
            Controls.Add(grpDetails);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAccountingAddAccount";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة حساب";
            grpDetails.ResumeLayout(false);
            grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblNameLabel;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.Label lblParentLabel;
        private System.Windows.Forms.ComboBox cmbParent;
        private System.Windows.Forms.Label lblNatureLabel;
        private System.Windows.Forms.ComboBox cmbNature;
        private System.Windows.Forms.Label lblLevelLabel;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.CheckBox chkAllowPosting;
        private System.Windows.Forms.Label lblCodeLabel;
        private System.Windows.Forms.Label lblSuggestedCode;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}