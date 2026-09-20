using SalesManagement.People.Controls;

namespace SalesManagement.Users
{
    partial class frmAddUpdateUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateUser));
            errorProvider1 = new ErrorProvider(components);
            lblTitle = new Label();
            chkIsActive = new CheckBox();
            btnPrevEmployeeInfo = new Button();
            pictureBox11 = new PictureBox();
            lblUserID = new Label();
            tpLoginInfo = new TabPage();
            label1 = new Label();
            label10 = new Label();
            txtUserName = new TextBox();
            label13 = new Label();
            txtConfirmPassword = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtPassword = new TextBox();
            pictureBox7 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox2 = new PictureBox();
            btnClose = new Button();
            btnNext = new Button();
            tcUserInfo = new TabControl();
            tpEmployeeInfo = new TabPage();
            ctrlEmployeeCardWithFilter1 = new SalesManagement.Employees.Controls.ctrlEmployeeCardWithFilter();
            btnNextLoginInfo = new Button();
            btnSave = new Button();
            btnSaveAndClose = new Button();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).BeginInit();
            tpLoginInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            tcUserInfo.SuspendLayout();
            tpEmployeeInfo.SuspendLayout();
            SuspendLayout();
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.WhiteSmoke;
            lblTitle.FlatStyle = FlatStyle.Flat;
            lblTitle.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.Firebrick;
            lblTitle.Location = new Point(395, 8);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(367, 39);
            lblTitle.TabIndex = 34;
            lblTitle.Text = "إضافة / تعديل مستخدم";
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.Font = new Font("Tahoma", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkIsActive.Location = new Point(770, 299);
            chkIsActive.Margin = new Padding(4);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(81, 28);
            chkIsActive.TabIndex = 3;
            chkIsActive.Text = "فعال";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // btnPrevEmployeeInfo
            // 
            btnPrevEmployeeInfo.AutoSize = true;
            btnPrevEmployeeInfo.BackColor = Color.White;
            btnPrevEmployeeInfo.FlatStyle = FlatStyle.Popup;
            btnPrevEmployeeInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnPrevEmployeeInfo.Image = Properties.Resources.Next_32;
            btnPrevEmployeeInfo.ImageAlign = ContentAlignment.MiddleLeft;
            btnPrevEmployeeInfo.Location = new Point(818, 712);
            btnPrevEmployeeInfo.Margin = new Padding(4);
            btnPrevEmployeeInfo.Name = "btnPrevEmployeeInfo";
            btnPrevEmployeeInfo.Size = new Size(307, 63);
            btnPrevEmployeeInfo.TabIndex = 4;
            btnPrevEmployeeInfo.Text = "العودة لأختيار مستخدم أخر";
            btnPrevEmployeeInfo.TextAlign = ContentAlignment.MiddleRight;
            btnPrevEmployeeInfo.UseVisualStyleBackColor = false;
            btnPrevEmployeeInfo.Click += btnPrevEmployeeInfo_Click;
            // 
            // pictureBox11
            // 
            pictureBox11.Image = Properties.Resources.Number_32;
            pictureBox11.Location = new Point(804, 40);
            pictureBox11.Margin = new Padding(4);
            pictureBox11.Name = "pictureBox11";
            pictureBox11.Size = new Size(47, 51);
            pictureBox11.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox11.TabIndex = 123;
            pictureBox11.TabStop = false;
            // 
            // lblUserID
            // 
            lblUserID.AutoSize = true;
            lblUserID.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            lblUserID.Location = new Point(745, 53);
            lblUserID.Margin = new Padding(4);
            lblUserID.Name = "lblUserID";
            lblUserID.Size = new Size(51, 24);
            lblUserID.TabIndex = 122;
            lblUserID.Text = "N/A";
            // 
            // tpLoginInfo
            // 
            tpLoginInfo.BackColor = Color.WhiteSmoke;
            tpLoginInfo.Controls.Add(chkIsActive);
            tpLoginInfo.Controls.Add(btnPrevEmployeeInfo);
            tpLoginInfo.Controls.Add(pictureBox11);
            tpLoginInfo.Controls.Add(lblUserID);
            tpLoginInfo.Controls.Add(label1);
            tpLoginInfo.Controls.Add(label10);
            tpLoginInfo.Controls.Add(txtUserName);
            tpLoginInfo.Controls.Add(label13);
            tpLoginInfo.Controls.Add(txtConfirmPassword);
            tpLoginInfo.Controls.Add(label2);
            tpLoginInfo.Controls.Add(label3);
            tpLoginInfo.Controls.Add(txtPassword);
            tpLoginInfo.Controls.Add(pictureBox7);
            tpLoginInfo.Controls.Add(pictureBox3);
            tpLoginInfo.Controls.Add(pictureBox2);
            tpLoginInfo.Location = new Point(4, 28);
            tpLoginInfo.Margin = new Padding(3, 4, 3, 4);
            tpLoginInfo.Name = "tpLoginInfo";
            tpLoginInfo.Padding = new Padding(3, 4, 3, 4);
            tpLoginInfo.Size = new Size(1132, 783);
            tpLoginInfo.TabIndex = 1;
            tpLoginInfo.Text = "بيانات المستخدم";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Flat;
            label1.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label1.Location = new Point(859, 53);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(140, 24);
            label1.TabIndex = 121;
            label1.Text = "الرقم المعرف:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label10.Location = new Point(859, 230);
            label10.Margin = new Padding(4);
            label10.Name = "label10";
            label10.Size = new Size(165, 24);
            label10.TabIndex = 115;
            label10.Text = "تأكيد كلمة السر:";
            // 
            // txtUserName
            // 
            txtUserName.Font = new Font("Tahoma", 10F);
            txtUserName.Location = new Point(535, 108);
            txtUserName.Margin = new Padding(4);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(261, 32);
            txtUserName.TabIndex = 0;
            txtUserName.Validating += txtUserName_Validating;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label13.Location = new Point(859, 299);
            label13.Margin = new Padding(4);
            label13.Name = "label13";
            label13.Size = new Size(132, 24);
            label13.TabIndex = 117;
            label13.Text = "هل الحساب:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Tahoma", 10F);
            txtConfirmPassword.Location = new Point(535, 226);
            txtConfirmPassword.Margin = new Padding(4);
            txtConfirmPassword.MaxLength = 50;
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(261, 32);
            txtConfirmPassword.TabIndex = 2;
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.Validating += txtConfirmPassword_Validating;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(859, 112);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(164, 24);
            label2.TabIndex = 116;
            label2.Text = "أسم المستخدم:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label3.Location = new Point(859, 171);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(114, 24);
            label3.TabIndex = 114;
            label3.Text = "كلمة السر:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Tahoma", 10F);
            txtPassword.Location = new Point(535, 167);
            txtPassword.Margin = new Padding(4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(261, 32);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Validating += txtPassword_Validating;
            // 
            // pictureBox7
            // 
            pictureBox7.Image = Properties.Resources.Password_32;
            pictureBox7.Location = new Point(804, 217);
            pictureBox7.Margin = new Padding(4);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(47, 51);
            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox7.TabIndex = 120;
            pictureBox7.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Password_32;
            pictureBox3.Location = new Point(804, 158);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 119;
            pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Person_32;
            pictureBox2.Location = new Point(804, 99);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(47, 51);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 118;
            pictureBox2.TabStop = false;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top;
            btnClose.AutoSize = true;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(398, 874);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(185, 61);
            btnClose.TabIndex = 7;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNext.AutoSize = true;
            btnNext.BackColor = Color.White;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNext.Image = Properties.Resources.Logo_Final;
            btnNext.ImageAlign = ContentAlignment.MiddleRight;
            btnNext.Location = new Point(8427, 4640);
            btnNext.Margin = new Padding(3, 4, 3, 4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(1342, 1063);
            btnNext.TabIndex = 33;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = false;
            // 
            // tcUserInfo
            // 
            tcUserInfo.Anchor = AnchorStyles.Top;
            tcUserInfo.Controls.Add(tpEmployeeInfo);
            tcUserInfo.Controls.Add(tpLoginInfo);
            tcUserInfo.Font = new Font("Tahoma", 8F);
            tcUserInfo.Location = new Point(8, 55);
            tcUserInfo.Margin = new Padding(4);
            tcUserInfo.Name = "tcUserInfo";
            tcUserInfo.RightToLeftLayout = true;
            tcUserInfo.SelectedIndex = 0;
            tcUserInfo.Size = new Size(1140, 815);
            tcUserInfo.TabIndex = 31;
            // 
            // tpEmployeeInfo
            // 
            tpEmployeeInfo.BackColor = Color.WhiteSmoke;
            tpEmployeeInfo.Controls.Add(ctrlEmployeeCardWithFilter1);
            tpEmployeeInfo.Controls.Add(btnNextLoginInfo);
            tpEmployeeInfo.Controls.Add(btnNext);
            tpEmployeeInfo.Location = new Point(4, 28);
            tpEmployeeInfo.Margin = new Padding(4);
            tpEmployeeInfo.Name = "tpEmployeeInfo";
            tpEmployeeInfo.Padding = new Padding(3, 4, 3, 4);
            tpEmployeeInfo.Size = new Size(1132, 783);
            tpEmployeeInfo.TabIndex = 0;
            tpEmployeeInfo.Text = "بينات الموظف";
            // 
            // ctrlEmployeeCardWithFilter1
            // 
            ctrlEmployeeCardWithFilter1.BackColor = Color.WhiteSmoke;
            ctrlEmployeeCardWithFilter1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ctrlEmployeeCardWithFilter1.Location = new Point(0, 0);
            ctrlEmployeeCardWithFilter1.Margin = new Padding(0);
            ctrlEmployeeCardWithFilter1.Name = "ctrlEmployeeCardWithFilter1";
            ctrlEmployeeCardWithFilter1.RightToLeft = RightToLeft.Yes;
            ctrlEmployeeCardWithFilter1.Size = new Size(1134, 698);
            ctrlEmployeeCardWithFilter1.TabIndex = 34;
            // 
            // btnNextLoginInfo
            // 
            btnNextLoginInfo.AutoSize = true;
            btnNextLoginInfo.BackColor = Color.White;
            btnNextLoginInfo.FlatStyle = FlatStyle.Popup;
            btnNextLoginInfo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnNextLoginInfo.Image = Properties.Resources.Prev_32;
            btnNextLoginInfo.ImageAlign = ContentAlignment.MiddleRight;
            btnNextLoginInfo.Location = new Point(11, 708);
            btnNextLoginInfo.Margin = new Padding(4);
            btnNextLoginInfo.Name = "btnNextLoginInfo";
            btnNextLoginInfo.Size = new Size(378, 63);
            btnNextLoginInfo.TabIndex = 1;
            btnNextLoginInfo.Text = "الانتقال إلى أدخل بينات المستخدم";
            btnNextLoginInfo.TextAlign = ContentAlignment.MiddleLeft;
            btnNextLoginInfo.UseVisualStyleBackColor = false;
            btnNextLoginInfo.Click += btnNextLoginInfo_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.AutoSize = true;
            btnSave.BackColor = Color.White;
            btnSave.Enabled = false;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = (Image)resources.GetObject("btnSave.Image");
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(205, 874);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 6;
            btnSave.Text = "إضافة";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
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
            btnSaveAndClose.Location = new Point(12, 874);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 5;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateUser
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1153, 945);
            Controls.Add(btnSaveAndClose);
            Controls.Add(lblTitle);
            Controls.Add(btnClose);
            Controls.Add(tcUserInfo);
            Controls.Add(btnSave);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            Name = "frmAddUpdateUser";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل مستخدم";
            Activated += frmAddUpdateUser_Activated;
            FormClosing += frmAddUpdateUser_FormClosing;
            Load += frmAddUpdateUser_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox11).EndInit();
            tpLoginInfo.ResumeLayout(false);
            tpLoginInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            tcUserInfo.ResumeLayout(false);
            tpEmployeeInfo.ResumeLayout(false);
            tpEmployeeInfo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private ErrorProvider errorProvider1;
        private Label lblTitle;
        private Button btnClose;
        private TabControl tcUserInfo;
        private TabPage tpEmployeeInfo;
        private Button btnNextLoginInfo;
        private Button btnNext;
        private TabPage tpLoginInfo;
        private CheckBox chkIsActive;
        private Button btnPrevEmployeeInfo;
        private PictureBox pictureBox11;
        private Label lblUserID;
        private Label label1;
        private Label label10;
        private TextBox txtUserName;
        private Label label13;
        private TextBox txtConfirmPassword;
        private Label label2;
        private Label label3;
        private TextBox txtPassword;
        private PictureBox pictureBox7;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private Button btnSave;
        private Button btnSaveAndClose;
        private Employees.Controls.ctrlEmployeeCardWithFilter ctrlEmployeeCardWithFilter1;
    }
}