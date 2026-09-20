using SalesManagement.Users.Controls;

namespace SalesManagement.Users
{
    partial class frmChangePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangePassword));
            lblMode = new Label();
            label10 = new Label();
            txtCurrentPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtNewPassword = new TextBox();
            pictureBox7 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox2 = new PictureBox();
            errorProvider1 = new ErrorProvider(components);
            btnSave = new Button();
            btnClose = new Button();
            ctrlUserCard1 = new ctrlUserCard();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // lblMode
            // 
            lblMode.Anchor = AnchorStyles.Top;
            lblMode.AutoSize = true;
            lblMode.BackColor = Color.WhiteSmoke;
            lblMode.FlatStyle = FlatStyle.Flat;
            lblMode.Font = new Font("Tahoma", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMode.ForeColor = Color.Firebrick;
            lblMode.Location = new Point(451, 8);
            lblMode.Margin = new Padding(4);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(250, 39);
            lblMode.TabIndex = 32;
            lblMode.Text = "تغيير كلمة السر";
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top;
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label10.Location = new Point(18, 890);
            label10.Margin = new Padding(4);
            label10.Name = "label10";
            label10.Size = new Size(241, 24);
            label10.TabIndex = 94;
            label10.Text = "تأكيد كلمة السر الجديدة:";
            // 
            // txtCurrentPassword
            // 
            txtCurrentPassword.Anchor = AnchorStyles.Top;
            txtCurrentPassword.Font = new Font("Tahoma", 10F);
            txtCurrentPassword.Location = new Point(322, 768);
            txtCurrentPassword.Margin = new Padding(4);
            txtCurrentPassword.Name = "txtCurrentPassword";
            txtCurrentPassword.Size = new Size(261, 32);
            txtCurrentPassword.TabIndex = 90;
            txtCurrentPassword.UseSystemPasswordChar = true;
            txtCurrentPassword.Validating += txtCurrentPassword_Validating;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Anchor = AnchorStyles.Top;
            txtConfirmPassword.Font = new Font("Tahoma", 10F);
            txtConfirmPassword.Location = new Point(322, 886);
            txtConfirmPassword.Margin = new Padding(4);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(261, 32);
            txtConfirmPassword.TabIndex = 92;
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.Validating += txtConfirmPassword_Validating;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label2.Location = new Point(76, 772);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(183, 24);
            label2.TabIndex = 95;
            label2.Text = "كلمة السر الحالية:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            label3.Location = new Point(69, 831);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(190, 24);
            label3.TabIndex = 93;
            label3.Text = "كلمة السر الجديدة:";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Anchor = AnchorStyles.Top;
            txtNewPassword.Font = new Font("Tahoma", 10F);
            txtNewPassword.Location = new Point(322, 827);
            txtNewPassword.Margin = new Padding(4);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(261, 32);
            txtNewPassword.TabIndex = 91;
            txtNewPassword.UseSystemPasswordChar = true;
            txtNewPassword.Validating += txtNewPassword_Validating;
            // 
            // pictureBox7
            // 
            pictureBox7.Image = Properties.Resources.Number_32;
            pictureBox7.Location = new Point(267, 877);
            pictureBox7.Margin = new Padding(4);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(47, 51);
            pictureBox7.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox7.TabIndex = 98;
            pictureBox7.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Anchor = AnchorStyles.Top;
            pictureBox3.Image = Properties.Resources.Number_32;
            pictureBox3.Location = new Point(267, 818);
            pictureBox3.Margin = new Padding(4);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(47, 51);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 97;
            pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top;
            pictureBox2.Image = Properties.Resources.Number_32;
            pictureBox2.Location = new Point(267, 759);
            pictureBox2.Margin = new Padding(4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(47, 51);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 96;
            pictureBox2.TabStop = false;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = (Image)resources.GetObject("btnSave.Image");
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(27, 936);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(181, 64);
            btnSave.TabIndex = 103;
            btnSave.Text = "حفظ";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top;
            btnClose.BackColor = Color.White;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatStyle = FlatStyle.Popup;
            btnClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnClose.Image = Properties.Resources.Close_32;
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(216, 936);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(181, 64);
            btnClose.TabIndex = 104;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // ctrlUserCard1
            // 
            ctrlUserCard1.Anchor = AnchorStyles.Top;
            ctrlUserCard1.BackColor = Color.WhiteSmoke;
            ctrlUserCard1.Location = new Point(8, 55);
            ctrlUserCard1.Margin = new Padding(4);
            ctrlUserCard1.Name = "ctrlUserCard1";
            ctrlUserCard1.Padding = new Padding(6);
            ctrlUserCard1.RightToLeft = RightToLeft.Yes;
            ctrlUserCard1.Size = new Size(1136, 705);
            ctrlUserCard1.TabIndex = 31;
            ctrlUserCard1.Load += ctrlUserCard1_Load;
            // 
            // frmChangePassword
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(1153, 1013);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(label10);
            Controls.Add(txtCurrentPassword);
            Controls.Add(txtConfirmPassword);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(txtNewPassword);
            Controls.Add(pictureBox7);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(lblMode);
            Controls.Add(ctrlUserCard1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmChangePassword";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "شاشة تغيير كلمة السر";
            Load += frmChangePassword_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private ctrlUserCard ctrlUserCard1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCurrentPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
    }
}