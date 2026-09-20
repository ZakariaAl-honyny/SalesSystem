namespace SalesSystem.Desktop.Screens.Accounting.Forms
{
    partial class frmAddUpdateAccount
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
            lblAccountId = new Label();
            label2 = new Label();
            txtAccountCode = new TextBox();
            label3 = new Label();
            txtNameAr = new TextBox();
            label4 = new Label();
            txtNameEn = new TextBox();
            label5 = new Label();
            cmbNature = new ComboBox();
            label6 = new Label();
            cmbLevel = new ComboBox();
            chkIsSystem = new CheckBox();
            chkAllowTransactions = new CheckBox();
            label7 = new Label();
            txtDescription = new TextBox();
            label8 = new Label();
            txtNotes = new TextBox();
            label9 = new Label();
            txtColorCode = new TextBox();
            btnSave = new Button();
            btnClose = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();

            // lblMode
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(250, 15);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(180, 34);
            lblMode.TabIndex = 0;
            lblMode.Text = "إضافة حساب جديد";

            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 10F);
            label1.Location = new Point(440, 60);
            label1.Name = "label1";
            label1.Size = new Size(70, 21);
            label1.TabIndex = 1;
            label1.Text = "المعرف:";

            // lblAccountId
            lblAccountId.AutoSize = true;
            lblAccountId.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblAccountId.ForeColor = Color.FromArgb(64, 64, 64);
            lblAccountId.Location = new Point(400, 60);
            lblAccountId.Name = "lblAccountId";
            lblAccountId.Size = new Size(34, 21);
            lblAccountId.TabIndex = 2;
            lblAccountId.Text = "---";

            // label2
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F);
            label2.Location = new Point(300, 60);
            label2.Name = "label2";
            label2.Size = new Size(86, 21);
            label2.TabIndex = 3;
            label2.Text = "كود الحساب:";

            // txtAccountCode
            txtAccountCode.Font = new Font("Tahoma", 10F);
            txtAccountCode.Location = new Point(140, 57);
            txtAccountCode.Name = "txtAccountCode";
            txtAccountCode.ReadOnly = true;
            txtAccountCode.RightToLeft = RightToLeft.Yes;
            txtAccountCode.Size = new Size(154, 28);
            txtAccountCode.TabIndex = 4;
            txtAccountCode.Text = "(تلقائي)";

            // label3
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F);
            label3.Location = new Point(520, 100);
            label3.Name = "label3";
            label3.Size = new Size(121, 21);
            label3.TabIndex = 5;
            label3.Text = "اسم الحساب (عربي):";

            // txtNameAr
            txtNameAr.Font = new Font("Tahoma", 10F);
            txtNameAr.Location = new Point(140, 97);
            txtNameAr.Name = "txtNameAr";
            txtNameAr.RightToLeft = RightToLeft.Yes;
            txtNameAr.Size = new Size(374, 28);
            txtNameAr.TabIndex = 6;
            txtNameAr.Validating += txtNameAr_Validating;

            // label4
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F);
            label4.Location = new Point(530, 140);
            label4.Name = "label4";
            label4.Size = new Size(111, 21);
            label4.TabIndex = 7;
            label4.Text = "الاسم (إنجليزي):";

            // txtNameEn
            txtNameEn.Font = new Font("Tahoma", 10F);
            txtNameEn.Location = new Point(140, 137);
            txtNameEn.Name = "txtNameEn";
            txtNameEn.RightToLeft = RightToLeft.Yes;
            txtNameEn.Size = new Size(384, 28);
            txtNameEn.TabIndex = 8;
            txtNameEn.Validating += txtNameEn_Validating;

            // label5
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F);
            label5.Location = new Point(570, 180);
            label5.Name = "label5";
            label5.Size = new Size(57, 21);
            label5.TabIndex = 9;
            label5.Text = "الطبيعة:";

            // cmbNature
            cmbNature.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNature.Font = new Font("Tahoma", 10F);
            cmbNature.FormattingEnabled = true;
            cmbNature.Location = new Point(400, 177);
            cmbNature.Name = "cmbNature";
            cmbNature.RightToLeft = RightToLeft.Yes;
            cmbNature.Size = new Size(164, 28);
            cmbNature.TabIndex = 10;

            // label6
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 10F);
            label6.Location = new Point(570, 220);
            label6.Name = "label6";
            label6.Size = new Size(57, 21);
            label6.TabIndex = 11;
            label6.Text = "المستوى:";

            // cmbLevel
            cmbLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLevel.Font = new Font("Tahoma", 10F);
            cmbLevel.FormattingEnabled = true;
            cmbLevel.Location = new Point(400, 217);
            cmbLevel.Name = "cmbLevel";
            cmbLevel.RightToLeft = RightToLeft.Yes;
            cmbLevel.Size = new Size(164, 28);
            cmbLevel.TabIndex = 12;

            // chkIsSystem
            chkIsSystem.AutoSize = true;
            chkIsSystem.Font = new Font("Tahoma", 10F);
            chkIsSystem.Location = new Point(520, 260);
            chkIsSystem.Name = "chkIsSystem";
            chkIsSystem.RightToLeft = RightToLeft.Yes;
            chkIsSystem.Size = new Size(109, 25);
            chkIsSystem.TabIndex = 13;
            chkIsSystem.Text = "حساب نظامي";
            chkIsSystem.UseVisualStyleBackColor = true;

            // chkAllowTransactions
            chkAllowTransactions.AutoSize = true;
            chkAllowTransactions.Font = new Font("Tahoma", 10F);
            chkAllowTransactions.Location = new Point(340, 260);
            chkAllowTransactions.Name = "chkAllowTransactions";
            chkAllowTransactions.RightToLeft = RightToLeft.Yes;
            chkAllowTransactions.Size = new Size(140, 25);
            chkAllowTransactions.TabIndex = 14;
            chkAllowTransactions.Text = "يسمح بالحركات";
            chkAllowTransactions.UseVisualStyleBackColor = true;

            // label7
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 10F);
            label7.Location = new Point(590, 300);
            label7.Name = "label7";
            label7.Size = new Size(46, 21);
            label7.TabIndex = 15;
            label7.Text = "الوصف:";

            // txtDescription
            txtDescription.Font = new Font("Tahoma", 10F);
            txtDescription.Location = new Point(140, 297);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.RightToLeft = RightToLeft.Yes;
            txtDescription.Size = new Size(444, 60);
            txtDescription.TabIndex = 16;

            // label8
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 10F);
            label8.Location = new Point(590, 380);
            label8.Name = "label8";
            label8.Size = new Size(51, 21);
            label8.TabIndex = 17;
            label8.Text = "ملاحظات:";

            // txtNotes
            txtNotes.Font = new Font("Tahoma", 10F);
            txtNotes.Location = new Point(140, 377);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.RightToLeft = RightToLeft.Yes;
            txtNotes.Size = new Size(444, 60);
            txtNotes.TabIndex = 18;

            // label9
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 10F);
            label9.Location = new Point(400, 460);
            label9.Name = "label9";
            label9.Size = new Size(86, 21);
            label9.TabIndex = 19;
            label9.Text = "رمز اللون:";

            // txtColorCode
            txtColorCode.Font = new Font("Tahoma", 10F);
            txtColorCode.Location = new Point(250, 457);
            txtColorCode.Name = "txtColorCode";
            txtColorCode.RightToLeft = RightToLeft.Yes;
            txtColorCode.Size = new Size(140, 28);
            txtColorCode.TabIndex = 20;
            txtColorCode.Validating += txtColorCode_Validating;

            // btnSave
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(480, 520);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(152, 40);
            btnSave.TabIndex = 21;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;

            // btnClose
            btnClose.BackColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.Location = new Point(12, 520);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(80, 40);
            btnClose.TabIndex = 22;
            btnClose.Text = "إلغاء";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;

            // errorProvider
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.RightToLeft = true;

            // frmAccountEditor
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(684, 576);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(txtColorCode);
            Controls.Add(label9);
            Controls.Add(txtNotes);
            Controls.Add(label8);
            Controls.Add(txtDescription);
            Controls.Add(label7);
            Controls.Add(chkAllowTransactions);
            Controls.Add(chkIsSystem);
            Controls.Add(cmbLevel);
            Controls.Add(label6);
            Controls.Add(cmbNature);
            Controls.Add(label5);
            Controls.Add(txtNameEn);
            Controls.Add(label4);
            Controls.Add(txtNameAr);
            Controls.Add(label3);
            Controls.Add(txtAccountCode);
            Controls.Add(label2);
            Controls.Add(lblAccountId);
            Controls.Add(label1);
            Controls.Add(lblMode);
            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmAccountEditor";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة حساب جديد";
            Load += frmAccountEditor_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAccountId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAccountCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNameAr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNameEn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbNature;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.CheckBox chkIsSystem;
        private System.Windows.Forms.CheckBox chkAllowTransactions;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtColorCode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
