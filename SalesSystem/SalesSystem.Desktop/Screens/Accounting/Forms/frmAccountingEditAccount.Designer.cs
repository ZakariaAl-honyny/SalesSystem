namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmAccountingEditAccount
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
            lblNotesLabel = new Label();
            txtNotes = new TextBox();
            lblOpeningLabel = new Label();
            numOpeningBalance = new NumericUpDown();
            chkAllowPosting = new CheckBox();
            lblAccountCode = new Label();
            lblCodeLabel = new Label();
            cmbNature = new ComboBox();
            lblNatureLabel = new Label();
            cmbParent = new ComboBox();
            lblParentLabel = new Label();
            txtAccountName = new TextBox();
            lblNameLabel = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            errorProvider = new ErrorProvider(components);
            grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numOpeningBalance).BeginInit();
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
            lblTitle.Size = new Size(520, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "تعديل حساب";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpDetails
            // 
            grpDetails.Controls.Add(lblNotesLabel);
            grpDetails.Controls.Add(txtNotes);
            grpDetails.Controls.Add(lblOpeningLabel);
            grpDetails.Controls.Add(numOpeningBalance);
            grpDetails.Controls.Add(chkAllowPosting);
            grpDetails.Controls.Add(lblAccountCode);
            grpDetails.Controls.Add(lblCodeLabel);
            grpDetails.Controls.Add(cmbNature);
            grpDetails.Controls.Add(lblNatureLabel);
            grpDetails.Controls.Add(cmbParent);
            grpDetails.Controls.Add(lblParentLabel);
            grpDetails.Controls.Add(txtAccountName);
            grpDetails.Controls.Add(lblNameLabel);
            grpDetails.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            grpDetails.Location = new Point(12, 60);
            grpDetails.Name = "grpDetails";
            grpDetails.RightToLeft = RightToLeft.Yes;
            grpDetails.Size = new Size(496, 392);
            grpDetails.TabIndex = 1;
            grpDetails.TabStop = false;
            grpDetails.Text = "بيانات الحساب";
            // 
            // lblNameLabel
            // 
            lblNameLabel.AutoSize = true;
            lblNameLabel.Font = new Font("Tahoma", 10F);
            lblNameLabel.Location = new Point(380, 30);
            lblNameLabel.Name = "lblNameLabel";
            lblNameLabel.Size = new Size(87, 21);
            lblNameLabel.TabIndex = 0;
            lblNameLabel.Text = "اسم الحساب:";
            // 
            // txtAccountName
            // 
            txtAccountName.Font = new Font("Tahoma", 10F);
            txtAccountName.Location = new Point(12, 28);
            txtAccountName.Name = "txtAccountName";
            txtAccountName.RightToLeft = RightToLeft.Yes;
            txtAccountName.Size = new Size(356, 28);
            txtAccountName.TabIndex = 1;
            // 
            // lblParentLabel
            // 
            lblParentLabel.AutoSize = true;
            lblParentLabel.Font = new Font("Tahoma", 10F);
            lblParentLabel.Location = new Point(380, 74);
            lblParentLabel.Name = "lblParentLabel";
            lblParentLabel.Size = new Size(90, 21);
            lblParentLabel.TabIndex = 2;
            lblParentLabel.Text = "الحساب الأب:";
            // 
            // cmbParent
            // 
            cmbParent.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParent.Font = new Font("Tahoma", 10F);
            cmbParent.FormattingEnabled = true;
            cmbParent.Location = new Point(12, 72);
            cmbParent.Name = "cmbParent";
            cmbParent.RightToLeft = RightToLeft.Yes;
            cmbParent.Size = new Size(356, 28);
            cmbParent.TabIndex = 3;
            // 
            // lblNatureLabel
            // 
            lblNatureLabel.AutoSize = true;
            lblNatureLabel.Font = new Font("Tahoma", 10F);
            lblNatureLabel.Location = new Point(380, 118);
            lblNatureLabel.Name = "lblNatureLabel";
            lblNatureLabel.Size = new Size(100, 21);
            lblNatureLabel.TabIndex = 4;
            lblNatureLabel.Text = "طبيعة الحساب:";
            // 
            // cmbNature
            // 
            cmbNature.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNature.Font = new Font("Tahoma", 10F);
            cmbNature.FormattingEnabled = true;
            cmbNature.Location = new Point(12, 116);
            cmbNature.Name = "cmbNature";
            cmbNature.RightToLeft = RightToLeft.Yes;
            cmbNature.Size = new Size(356, 28);
            cmbNature.TabIndex = 5;
            // 
            // lblCodeLabel
            // 
            lblCodeLabel.AutoSize = true;
            lblCodeLabel.Font = new Font("Tahoma", 10F);
            lblCodeLabel.Location = new Point(380, 162);
            lblCodeLabel.Name = "lblCodeLabel";
            lblCodeLabel.Size = new Size(42, 21);
            lblCodeLabel.TabIndex = 6;
            lblCodeLabel.Text = "الكود:";
            // 
            // lblAccountCode
            // 
            lblAccountCode.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAccountCode.ForeColor = Color.Firebrick;
            lblAccountCode.Location = new Point(12, 160);
            lblAccountCode.Name = "lblAccountCode";
            lblAccountCode.Size = new Size(356, 26);
            lblAccountCode.TabIndex = 7;
            lblAccountCode.Text = "1101-01";
            lblAccountCode.TextAlign = ContentAlignment.MiddleLeft;
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
            chkAllowPosting.Size = new Size(132, 25);
            chkAllowPosting.TabIndex = 8;
            chkAllowPosting.Text = "يسمح بالترحيل";
            chkAllowPosting.UseVisualStyleBackColor = true;
            // 
            // lblOpeningLabel
            // 
            lblOpeningLabel.AutoSize = true;
            lblOpeningLabel.Font = new Font("Tahoma", 10F);
            lblOpeningLabel.Location = new Point(380, 250);
            lblOpeningLabel.Name = "lblOpeningLabel";
            lblOpeningLabel.Size = new Size(102, 21);
            lblOpeningLabel.TabIndex = 9;
            lblOpeningLabel.Text = "الرصيد الافتتاحي:";
            // 
            // numOpeningBalance
            // 
            numOpeningBalance.DecimalPlaces = 2;
            numOpeningBalance.Font = new Font("Tahoma", 10F);
            numOpeningBalance.Location = new Point(12, 246);
            numOpeningBalance.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numOpeningBalance.Name = "numOpeningBalance";
            numOpeningBalance.RightToLeft = RightToLeft.Yes;
            numOpeningBalance.Size = new Size(356, 28);
            numOpeningBalance.TabIndex = 10;
            numOpeningBalance.ThousandsSeparator = true;
            numOpeningBalance.Value = new decimal(new int[] { 250000, 0, 0, 0 });
            // 
            // lblNotesLabel
            // 
            lblNotesLabel.AutoSize = true;
            lblNotesLabel.Font = new Font("Tahoma", 10F);
            lblNotesLabel.Location = new Point(380, 294);
            lblNotesLabel.Name = "lblNotesLabel";
            lblNotesLabel.Size = new Size(61, 21);
            lblNotesLabel.TabIndex = 11;
            lblNotesLabel.Text = "ملاحظات:";
            // 
            // txtNotes
            // 
            txtNotes.Font = new Font("Tahoma", 10F);
            txtNotes.Location = new Point(12, 290);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.RightToLeft = RightToLeft.Yes;
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(356, 76);
            txtNotes.TabIndex = 12;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(398, 464);
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
            btnCancel.Location = new Point(12, 464);
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
            errorProvider.RightToLeft = true;
            // 
            // frmAccountingEditAccount
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(520, 520);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(grpDetails);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAccountingEditAccount";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "تعديل حساب";
            grpDetails.ResumeLayout(false);
            grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numOpeningBalance).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Label lblCodeLabel;
        private System.Windows.Forms.Label lblAccountCode;
        private System.Windows.Forms.CheckBox chkAllowPosting;
        private System.Windows.Forms.Label lblOpeningLabel;
        private System.Windows.Forms.NumericUpDown numOpeningBalance;
        private System.Windows.Forms.Label lblNotesLabel;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}