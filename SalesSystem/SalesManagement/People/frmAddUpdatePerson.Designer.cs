namespace SalesManagement.People
{
    partial class frmAddUpdatePerson
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdatePerson));
            label1 = new Label();
            lblPersonID = new Label();
            lblMode = new Label();
            errorProvider1 = new ErrorProvider(components);
            openFileDialog1 = new OpenFileDialog();
            pbPersonImage = new PictureBox();
            txtLastName = new TextBox();
            txtThirdName = new TextBox();
            txtSecondName = new TextBox();
            label10 = new Label();
            txtFirstName = new TextBox();
            label13 = new Label();
            txtEmail = new TextBox();
            label2 = new Label();
            btnClose = new Button();
            label12 = new Label();
            label11 = new Label();
            label9 = new Label();
            txtAddress = new TextBox();
            label8 = new Label();
            txtPhone = new TextBox();
            rbFemale = new RadioButton();
            llRemoveImage = new LinkLabel();
            llOpenFileDialog = new LinkLabel();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            dtpDateOfBirth = new DateTimePicker();
            cbxCountry = new ComboBox();
            rbMale = new RadioButton();
            btnSave = new Button();
            txtNationalNo = new TextBox();
            label3 = new Label();
            btnSaveAndClose = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPersonImage).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(86, 93);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(126, 24);
            label1.TabIndex = 0;
            label1.Text = "رقم المعرف:";
            label1.UseWaitCursor = true;
            // 
            // lblPersonID
            // 
            lblPersonID.Anchor = AnchorStyles.Top;
            lblPersonID.AutoSize = true;
            lblPersonID.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblPersonID.Location = new Point(220, 93);
            lblPersonID.Margin = new Padding(4);
            lblPersonID.Name = "lblPersonID";
            lblPersonID.Size = new Size(51, 24);
            lblPersonID.TabIndex = 7;
            lblPersonID.Text = "N/A";
            lblPersonID.UseWaitCursor = true;
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(497, 8);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(311, 39);
            lblMode.TabIndex = 29;
            lblMode.Text = "إضافة  شخص جديد";
            lblMode.UseWaitCursor = true;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // pbPersonImage
            // 
            pbPersonImage.Anchor = AnchorStyles.Top;
            pbPersonImage.BorderStyle = BorderStyle.FixedSingle;
            pbPersonImage.Image = Properties.Resources.Male_512;
            pbPersonImage.Location = new Point(1027, 229);
            pbPersonImage.Margin = new Padding(4);
            pbPersonImage.Name = "pbPersonImage";
            pbPersonImage.Size = new Size(261, 250);
            pbPersonImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbPersonImage.TabIndex = 147;
            pbPersonImage.TabStop = false;
            // 
            // txtLastName
            // 
            txtLastName.Anchor = AnchorStyles.Top;
            txtLastName.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtLastName.Location = new Point(1027, 170);
            txtLastName.Margin = new Padding(4);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(261, 32);
            txtLastName.TabIndex = 121;
            txtLastName.KeyPress += txtFilterValue_KeyPress;
            txtLastName.Validating += _ValidateEmptyTextBox;
            // 
            // txtThirdName
            // 
            txtThirdName.Anchor = AnchorStyles.Top;
            txtThirdName.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtThirdName.Location = new Point(758, 170);
            txtThirdName.Margin = new Padding(4);
            txtThirdName.Name = "txtThirdName";
            txtThirdName.Size = new Size(261, 32);
            txtThirdName.TabIndex = 120;
            txtThirdName.KeyPress += txtFilterValue_KeyPress;
            // 
            // txtSecondName
            // 
            txtSecondName.Anchor = AnchorStyles.Top;
            txtSecondName.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtSecondName.Location = new Point(489, 171);
            txtSecondName.Margin = new Padding(4);
            txtSecondName.Name = "txtSecondName";
            txtSecondName.Size = new Size(261, 32);
            txtSecondName.TabIndex = 119;
            txtSecondName.KeyPress += txtFilterValue_KeyPress;
            txtSecondName.Validating += _ValidateEmptyTextBox;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top;
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label10.Location = new Point(130, 351);
            label10.Margin = new Padding(4);
            label10.Name = "label10";
            label10.Size = new Size(81, 24);
            label10.TabIndex = 141;
            label10.Text = "الإيميل:";
            // 
            // txtFirstName
            // 
            txtFirstName.Anchor = AnchorStyles.Top;
            txtFirstName.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtFirstName.Location = new Point(220, 170);
            txtFirstName.Margin = new Padding(4);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(261, 32);
            txtFirstName.TabIndex = 118;
            txtFirstName.KeyPress += txtFilterValue_KeyPress;
            txtFirstName.Validating += _ValidateEmptyTextBox;
            // 
            // label13
            // 
            label13.Anchor = AnchorStyles.Top;
            label13.AutoSize = true;
            label13.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label13.Location = new Point(130, 290);
            label13.Margin = new Padding(4);
            label13.Name = "label13";
            label13.Size = new Size(82, 24);
            label13.TabIndex = 145;
            label13.Text = "الجنس:";
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Top;
            txtEmail.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtEmail.Location = new Point(223, 347);
            txtEmail.Margin = new Padding(4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(261, 32);
            txtEmail.TabIndex = 127;
            txtEmail.Validating += txtEmail_Validating;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(138, 174);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(74, 24);
            label2.TabIndex = 144;
            label2.Text = "الأسم:";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(606, 557);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(185, 61);
            btnClose.TabIndex = 133;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // label12
            // 
            label12.Anchor = AnchorStyles.Top;
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label12.Location = new Point(677, 351);
            label12.Margin = new Padding(4);
            label12.Name = "label12";
            label12.Size = new Size(73, 24);
            label12.TabIndex = 143;
            label12.Text = "الدولة:";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top;
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label11.Location = new Point(621, 233);
            label11.Margin = new Padding(4);
            label11.Name = "label11";
            label11.Size = new Size(129, 24);
            label11.TabIndex = 142;
            label11.Text = "تاريخ الميلاد:";
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top;
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label9.Location = new Point(129, 411);
            label9.Margin = new Padding(4);
            label9.Name = "label9";
            label9.Size = new Size(83, 24);
            label9.TabIndex = 140;
            label9.Text = "العنوان:";
            // 
            // txtAddress
            // 
            txtAddress.Anchor = AnchorStyles.Top;
            txtAddress.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtAddress.Location = new Point(220, 397);
            txtAddress.Margin = new Padding(4);
            txtAddress.Multiline = true;
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(799, 152);
            txtAddress.TabIndex = 129;
            txtAddress.KeyPress += txtFilterValue_KeyPress;
            txtAddress.Validating += _ValidateEmptyTextBox;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top;
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label8.Location = new Point(632, 292);
            label8.Margin = new Padding(4);
            label8.Name = "label8";
            label8.Size = new Size(118, 24);
            label8.TabIndex = 139;
            label8.Text = "رقم الهاتف:";
            // 
            // txtPhone
            // 
            txtPhone.Anchor = AnchorStyles.Top;
            txtPhone.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtPhone.Location = new Point(758, 288);
            txtPhone.Margin = new Padding(4);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(261, 32);
            txtPhone.TabIndex = 126;
            txtPhone.KeyPress += txtFilterValue_KeyPress;
            txtPhone.Validating += _ValidateEmptyTextBox;
            // 
            // rbFemale
            // 
            rbFemale.Anchor = AnchorStyles.Top;
            rbFemale.AutoSize = true;
            rbFemale.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbFemale.Location = new Point(306, 290);
            rbFemale.Margin = new Padding(4);
            rbFemale.Name = "rbFemale";
            rbFemale.Size = new Size(78, 28);
            rbFemale.TabIndex = 125;
            rbFemale.TabStop = true;
            rbFemale.Text = "أنثى";
            rbFemale.UseVisualStyleBackColor = true;
            // 
            // llRemoveImage
            // 
            llRemoveImage.Anchor = AnchorStyles.Top;
            llRemoveImage.AutoSize = true;
            llRemoveImage.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            llRemoveImage.Location = new Point(1094, 519);
            llRemoveImage.Margin = new Padding(4);
            llRemoveImage.Name = "llRemoveImage";
            llRemoveImage.Size = new Size(127, 24);
            llRemoveImage.TabIndex = 131;
            llRemoveImage.TabStop = true;
            llRemoveImage.Text = "خذف الصورة";
            llRemoveImage.Visible = false;
            llRemoveImage.LinkClicked += llRemoveImage_LinkClicked;
            // 
            // llOpenFileDialog
            // 
            llOpenFileDialog.Anchor = AnchorStyles.Top;
            llOpenFileDialog.AutoSize = true;
            llOpenFileDialog.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            llOpenFileDialog.Location = new Point(1105, 487);
            llOpenFileDialog.Margin = new Padding(4);
            llOpenFileDialog.Name = "llOpenFileDialog";
            llOpenFileDialog.Size = new Size(105, 24);
            llOpenFileDialog.TabIndex = 130;
            llOpenFileDialog.TabStop = true;
            llOpenFileDialog.Text = "أختر صورة";
            llOpenFileDialog.LinkClicked += llOpenFileDialog_LinkClicked;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top;
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label7.Location = new Point(1127, 137);
            label7.Margin = new Padding(4);
            label7.Name = "label7";
            label7.Size = new Size(60, 24);
            label7.TabIndex = 138;
            label7.Text = "الرابع";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top;
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label6.Location = new Point(855, 138);
            label6.Margin = new Padding(4);
            label6.Name = "label6";
            label6.Size = new Size(66, 24);
            label6.TabIndex = 137;
            label6.Text = "الثالث";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label5.Location = new Point(585, 139);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(68, 24);
            label5.TabIndex = 136;
            label5.Text = "الثاني";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label4.Location = new Point(328, 139);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(56, 24);
            label4.TabIndex = 135;
            label4.Text = "الاول";
            // 
            // dtpDateOfBirth
            // 
            dtpDateOfBirth.Anchor = AnchorStyles.Top;
            dtpDateOfBirth.CustomFormat = "dd/MM/yyyy";
            dtpDateOfBirth.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dtpDateOfBirth.Format = DateTimePickerFormat.Custom;
            dtpDateOfBirth.Location = new Point(758, 229);
            dtpDateOfBirth.Margin = new Padding(4);
            dtpDateOfBirth.Name = "dtpDateOfBirth";
            dtpDateOfBirth.Size = new Size(261, 32);
            dtpDateOfBirth.TabIndex = 123;
            // 
            // cbxCountry
            // 
            cbxCountry.Anchor = AnchorStyles.Top;
            cbxCountry.BackColor = SystemColors.Control;
            cbxCountry.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCountry.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            cbxCountry.FormattingEnabled = true;
            cbxCountry.Location = new Point(758, 347);
            cbxCountry.Margin = new Padding(4);
            cbxCountry.Name = "cbxCountry";
            cbxCountry.Size = new Size(261, 32);
            cbxCountry.TabIndex = 128;
            // 
            // rbMale
            // 
            rbMale.Anchor = AnchorStyles.Top;
            rbMale.AutoSize = true;
            rbMale.Checked = true;
            rbMale.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            rbMale.Location = new Point(223, 290);
            rbMale.Margin = new Padding(4);
            rbMale.Name = "rbMale";
            rbMale.Size = new Size(67, 28);
            rbMale.TabIndex = 124;
            rbMale.TabStop = true;
            rbMale.Text = "ذكر";
            rbMale.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = (Image)resources.GetObject("btnSave.Image");
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(413, 557);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 132;
            btnSave.Text = "إضافة";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // txtNationalNo
            // 
            txtNationalNo.Anchor = AnchorStyles.Top;
            txtNationalNo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            txtNationalNo.Location = new Point(220, 229);
            txtNationalNo.Margin = new Padding(4);
            txtNationalNo.Name = "txtNationalNo";
            txtNationalNo.Size = new Size(261, 32);
            txtNationalNo.TabIndex = 122;
            txtNationalNo.KeyPress += txtFilterValue_KeyPress;
            txtNationalNo.Validating += txtNationalNo_Validating;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label3.Location = new Point(69, 233);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(143, 24);
            label3.TabIndex = 134;
            label3.Text = "الرقم الوطني:";
            // 
            // btnSaveAndClose
            // 
            btnSaveAndClose.Anchor = AnchorStyles.Top;
            btnSaveAndClose.BackColor = Color.White;
            btnSaveAndClose.Enabled = false;
            btnSaveAndClose.FlatStyle = FlatStyle.Popup;
            btnSaveAndClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveAndClose.Image = Properties.Resources.Save_32;
            btnSaveAndClose.ImageAlign = ContentAlignment.MiddleRight;
            btnSaveAndClose.Location = new Point(220, 557);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 157;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdatePerson
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1312, 650);
            Controls.Add(btnSaveAndClose);
            Controls.Add(pbPersonImage);
            Controls.Add(txtLastName);
            Controls.Add(txtThirdName);
            Controls.Add(txtSecondName);
            Controls.Add(label10);
            Controls.Add(txtFirstName);
            Controls.Add(label13);
            Controls.Add(txtEmail);
            Controls.Add(label2);
            Controls.Add(btnClose);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(label9);
            Controls.Add(txtAddress);
            Controls.Add(label8);
            Controls.Add(txtPhone);
            Controls.Add(rbFemale);
            Controls.Add(llRemoveImage);
            Controls.Add(llOpenFileDialog);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(dtpDateOfBirth);
            Controls.Add(cbxCountry);
            Controls.Add(rbMale);
            Controls.Add(btnSave);
            Controls.Add(txtNationalNo);
            Controls.Add(label3);
            Controls.Add(lblMode);
            Controls.Add(lblPersonID);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "frmAddUpdatePerson";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة وتعديل بيانات الشخص";
            FormClosing += frmAddUpdatePerson_FormClosing;
            Load += frmAddUpdatePerson_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPersonImage).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPersonID;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private PictureBox pbPersonImage;
        private TextBox txtLastName;
        private TextBox txtThirdName;
        private TextBox txtSecondName;
        private Label label10;
        private TextBox txtFirstName;
        private Label label13;
        private TextBox txtEmail;
        private Label label2;
        private Button btnClose;
        private Label label12;
        private Label label11;
        private Label label9;
        private TextBox txtAddress;
        private Label label8;
        private TextBox txtPhone;
        private RadioButton rbFemale;
        private LinkLabel llRemoveImage;
        private LinkLabel llOpenFileDialog;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private DateTimePicker dtpDateOfBirth;
        private ComboBox cbxCountry;
        private RadioButton rbMale;
        private Button btnSave;
        private TextBox txtNationalNo;
        private Label label3;
        private Button btnSaveAndClose;
    }
}