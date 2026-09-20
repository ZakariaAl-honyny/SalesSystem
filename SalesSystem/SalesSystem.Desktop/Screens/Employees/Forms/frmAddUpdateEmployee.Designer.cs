namespace SalesSystem.Desktop.Screens.Employees.Forms
{
    partial class frmAddUpdateEmployee
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do NOT modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            errorProvider = new ErrorProvider(components);
            lblMode = new Label();
            lblName = new Label();
            txtName = new TextBox();
            lblEmployeeNumber = new Label();
            txtEmployeeNumber = new TextBox();
            lblJobTitle = new Label();
            cmbJobTitle = new ComboBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblNationalId = new Label();
            txtNationalId = new TextBox();
            lblBaseSalary = new Label();
            txtBaseSalary = new TextBox();
            lblCommissionRate = new Label();
            txtCommissionRate = new TextBox();
            lblHireDate = new Label();
            dtpHireDate = new DateTimePicker();
            chkActive = new CheckBox();
            lblLinkedUser = new Label();
            cmbLinkedUser = new ComboBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // lblMode
            // 
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.Dock = DockStyle.Top;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(0, 0);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(700, 48);
            lblMode.TabIndex = 0;
            lblMode.Text = "إضافة موظف";
            lblMode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(40, 91);
            lblName.Name = "lblName";
            lblName.Size = new Size(74, 29);
            lblName.TabIndex = 1;
            lblName.Text = "الاسم:";
            // 
            // txtName
            // 
            txtName.Location = new Point(260, 87);
            txtName.Name = "txtName";
            txtName.Size = new Size(380, 36);
            txtName.TabIndex = 2;
            // 
            // lblEmployeeNumber
            // 
            lblEmployeeNumber.AutoSize = true;
            lblEmployeeNumber.Location = new Point(40, 131);
            lblEmployeeNumber.Name = "lblEmployeeNumber";
            lblEmployeeNumber.Size = new Size(116, 29);
            lblEmployeeNumber.TabIndex = 3;
            lblEmployeeNumber.Text = "رقم الموظف:";
            // 
            // txtEmployeeNumber
            // 
            txtEmployeeNumber.Location = new Point(260, 127);
            txtEmployeeNumber.Name = "txtEmployeeNumber";
            txtEmployeeNumber.Size = new Size(180, 36);
            txtEmployeeNumber.TabIndex = 4;
            txtEmployeeNumber.KeyPress += txtEmployeeNumber_KeyPress;
            // 
            // lblJobTitle
            // 
            lblJobTitle.AutoSize = true;
            lblJobTitle.Location = new Point(40, 171);
            lblJobTitle.Name = "lblJobTitle";
            lblJobTitle.Size = new Size(134, 29);
            lblJobTitle.TabIndex = 5;
            lblJobTitle.Text = "المسمى الوظيفي:";
            // 
            // cmbJobTitle
            // 
            cmbJobTitle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbJobTitle.FormattingEnabled = true;
            cmbJobTitle.Items.AddRange(new object[] { "بائع", "كاشير", "مندوب", "سائق", "عامل" });
            cmbJobTitle.Location = new Point(260, 167);
            cmbJobTitle.Name = "cmbJobTitle";
            cmbJobTitle.Size = new Size(260, 36);
            cmbJobTitle.TabIndex = 6;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(40, 211);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(70, 29);
            lblPhone.TabIndex = 7;
            lblPhone.Text = "الهاتف:";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(260, 207);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(260, 36);
            txtPhone.TabIndex = 8;
            // 
            // lblNationalId
            // 
            lblNationalId.AutoSize = true;
            lblNationalId.Location = new Point(40, 251);
            lblNationalId.Name = "lblNationalId";
            lblNationalId.Size = new Size(116, 29);
            lblNationalId.TabIndex = 9;
            lblNationalId.Text = "الرقم الوطني:";
            // 
            // txtNationalId
            // 
            txtNationalId.Location = new Point(260, 247);
            txtNationalId.Name = "txtNationalId";
            txtNationalId.Size = new Size(260, 36);
            txtNationalId.TabIndex = 10;
            // 
            // lblBaseSalary
            // 
            lblBaseSalary.AutoSize = true;
            lblBaseSalary.Location = new Point(40, 291);
            lblBaseSalary.Name = "lblBaseSalary";
            lblBaseSalary.Size = new Size(124, 29);
            lblBaseSalary.TabIndex = 11;
            lblBaseSalary.Text = "الراتب الأساسي:";
            // 
            // txtBaseSalary
            // 
            txtBaseSalary.Location = new Point(260, 287);
            txtBaseSalary.Name = "txtBaseSalary";
            txtBaseSalary.Size = new Size(180, 36);
            txtBaseSalary.TabIndex = 12;
            txtBaseSalary.KeyPress += txtDecimal_KeyPress;
            // 
            // lblCommissionRate
            // 
            lblCommissionRate.AutoSize = true;
            lblCommissionRate.Location = new Point(40, 331);
            lblCommissionRate.Name = "lblCommissionRate";
            lblCommissionRate.Size = new Size(126, 29);
            lblCommissionRate.TabIndex = 13;
            lblCommissionRate.Text = "نسبة العمولة:";
            // 
            // txtCommissionRate
            // 
            txtCommissionRate.Location = new Point(260, 327);
            txtCommissionRate.Name = "txtCommissionRate";
            txtCommissionRate.Size = new Size(180, 36);
            txtCommissionRate.TabIndex = 14;
            txtCommissionRate.KeyPress += txtDecimal_KeyPress;
            // 
            // lblHireDate
            // 
            lblHireDate.AutoSize = true;
            lblHireDate.Location = new Point(40, 371);
            lblHireDate.Name = "lblHireDate";
            lblHireDate.Size = new Size(116, 29);
            lblHireDate.TabIndex = 15;
            lblHireDate.Text = "تاريخ التعيين:";
            // 
            // dtpHireDate
            // 
            dtpHireDate.Format = DateTimePickerFormat.Short;
            dtpHireDate.Location = new Point(260, 367);
            dtpHireDate.Name = "dtpHireDate";
            dtpHireDate.Size = new Size(240, 36);
            dtpHireDate.TabIndex = 16;
            // 
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Location = new Point(260, 411);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(88, 33);
            chkActive.TabIndex = 17;
            chkActive.Text = "نشط";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // lblLinkedUser
            // 
            lblLinkedUser.AutoSize = true;
            lblLinkedUser.Location = new Point(40, 451);
            lblLinkedUser.Name = "lblLinkedUser";
            lblLinkedUser.Size = new Size(146, 29);
            lblLinkedUser.TabIndex = 18;
            lblLinkedUser.Text = "المستخدم المرتبط:";
            // 
            // cmbLinkedUser
            // 
            cmbLinkedUser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLinkedUser.FormattingEnabled = true;
            cmbLinkedUser.Items.AddRange(new object[] { "بدون", "أحمد الحميري", "فاطمة الشرعبي", "خالد العمراني", "سمير المقطري" });
            cmbLinkedUser.Location = new Point(260, 447);
            cmbLinkedUser.Name = "cmbLinkedUser";
            cmbLinkedUser.Size = new Size(320, 36);
            cmbLinkedUser.TabIndex = 19;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.Location = new Point(40, 491);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(82, 29);
            lblNotes.TabIndex = 20;
            lblNotes.Text = "ملاحظات:";
            // 
            // txtNotes
            // 
            txtNotes.Location = new Point(260, 487);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(380, 80);
            txtNotes.TabIndex = 21;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.White;
            btnSave.FlatAppearance.BorderColor = Color.Black;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(260, 600);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(190, 40);
            btnSave.TabIndex = 22;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderColor = Color.Black;
            btnCancel.FlatAppearance.BorderSize = 2;
            btnCancel.FlatStyle = FlatStyle.Popup;
            btnCancel.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(460, 600);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 40);
            btnCancel.TabIndex = 23;
            btnCancel.Text = "إلغاء";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmAddUpdateEmployee
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnCancel;
            ClientSize = new Size(700, 670);
            Controls.Add(lblMode);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtNotes);
            Controls.Add(lblNotes);
            Controls.Add(cmbLinkedUser);
            Controls.Add(lblLinkedUser);
            Controls.Add(chkActive);
            Controls.Add(dtpHireDate);
            Controls.Add(lblHireDate);
            Controls.Add(txtCommissionRate);
            Controls.Add(lblCommissionRate);
            Controls.Add(txtBaseSalary);
            Controls.Add(lblBaseSalary);
            Controls.Add(txtNationalId);
            Controls.Add(lblNationalId);
            Controls.Add(txtPhone);
            Controls.Add(lblPhone);
            Controls.Add(cmbJobTitle);
            Controls.Add(lblJobTitle);
            Controls.Add(txtEmployeeNumber);
            Controls.Add(lblEmployeeNumber);
            Controls.Add(txtName);
            Controls.Add(lblName);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmAddUpdateEmployee";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة موظف";
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblEmployeeNumber;
        private System.Windows.Forms.TextBox txtEmployeeNumber;
        private System.Windows.Forms.Label lblJobTitle;
        private System.Windows.Forms.ComboBox cmbJobTitle;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblNationalId;
        private System.Windows.Forms.TextBox txtNationalId;
        private System.Windows.Forms.Label lblBaseSalary;
        private System.Windows.Forms.TextBox txtBaseSalary;
        private System.Windows.Forms.Label lblCommissionRate;
        private System.Windows.Forms.TextBox txtCommissionRate;
        private System.Windows.Forms.Label lblHireDate;
        private System.Windows.Forms.DateTimePicker dtpHireDate;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblLinkedUser;
        private System.Windows.Forms.ComboBox cmbLinkedUser;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}