namespace SalesManagement.Companies
{
    partial class frmAddUpdateCompany
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddUpdateCompany));
            pictureBox1 = new PictureBox();
            txtOtherDetails = new TextBox();
            label4 = new Label();
            txtCompanyName = new TextBox();
            txtCompanyID = new TextBox();
            label3 = new Label();
            label2 = new Label();
            lblTitle = new Label();
            errorProvider1 = new ErrorProvider(components);
            txtCompanyPhone = new TextBox();
            label1 = new Label();
            txtCompanyAddress = new TextBox();
            label5 = new Label();
            btnSave = new Button();
            btnClose = new Button();
            btnSaveAndClose = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top;
            pictureBox1.Location = new Point(425, 10);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(155, 150);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // txtOtherDetails
            // 
            txtOtherDetails.Anchor = AnchorStyles.Top;
            txtOtherDetails.Location = new Point(217, 494);
            txtOtherDetails.Margin = new Padding(4);
            txtOtherDetails.Multiline = true;
            txtOtherDetails.Name = "txtOtherDetails";
            txtOtherDetails.Size = new Size(571, 100);
            txtOtherDetails.TabIndex = 4;
            txtOtherDetails.KeyPress += txtFilterValue_KeyPress;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Location = new Point(37, 494);
            label4.Margin = new Padding(4);
            label4.Name = "label4";
            label4.Size = new Size(172, 29);
            label4.TabIndex = 5;
            label4.Text = "تفاصيل الشركة:";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtCompanyName
            // 
            txtCompanyName.Anchor = AnchorStyles.Top;
            txtCompanyName.Location = new Point(217, 298);
            txtCompanyName.Margin = new Padding(4);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new Size(571, 36);
            txtCompanyName.TabIndex = 1;
            txtCompanyName.KeyPress += txtFilterValue_KeyPress;
            txtCompanyName.Validating += _ValidateEmptyTextBox;
            // 
            // txtCompanyID
            // 
            txtCompanyID.Anchor = AnchorStyles.Top;
            txtCompanyID.BackColor = SystemColors.Window;
            txtCompanyID.Enabled = false;
            txtCompanyID.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCompanyID.Location = new Point(217, 254);
            txtCompanyID.Margin = new Padding(4);
            txtCompanyID.Name = "txtCompanyID";
            txtCompanyID.ReadOnly = true;
            txtCompanyID.Size = new Size(571, 36);
            txtCompanyID.TabIndex = 0;
            txtCompanyID.Text = "0";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Location = new Point(60, 302);
            label3.Margin = new Padding(4);
            label3.Name = "label3";
            label3.Size = new Size(149, 29);
            label3.TabIndex = 2;
            label3.Text = "أسم الشركة:";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(71, 257);
            label2.Margin = new Padding(4);
            label2.Name = "label2";
            label2.Size = new Size(138, 29);
            label2.TabIndex = 1;
            label2.Text = "رقم الشركة:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Anchor = AnchorStyles.Top;
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Tahoma", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(325, 163);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(355, 48);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "إضافة / تعديل شركة";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // txtCompanyPhone
            // 
            txtCompanyPhone.Anchor = AnchorStyles.Top;
            txtCompanyPhone.Location = new Point(217, 342);
            txtCompanyPhone.Margin = new Padding(4);
            txtCompanyPhone.Name = "txtCompanyPhone";
            txtCompanyPhone.Size = new Size(571, 36);
            txtCompanyPhone.TabIndex = 2;
            txtCompanyPhone.KeyPress += txtFilterValue_KeyPress;
            txtCompanyPhone.Validating += _ValidateEmptyTextBox;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(11, 346);
            label1.Margin = new Padding(4);
            label1.Name = "label1";
            label1.Size = new Size(198, 29);
            label1.TabIndex = 8;
            label1.Text = "رقم هاتف الشركة:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtCompanyAddress
            // 
            txtCompanyAddress.Anchor = AnchorStyles.Top;
            txtCompanyAddress.Location = new Point(217, 386);
            txtCompanyAddress.Margin = new Padding(4);
            txtCompanyAddress.Multiline = true;
            txtCompanyAddress.Name = "txtCompanyAddress";
            txtCompanyAddress.Size = new Size(571, 100);
            txtCompanyAddress.TabIndex = 3;
            txtCompanyAddress.KeyPress += txtFilterValue_KeyPress;
            txtCompanyAddress.Validating += _ValidateEmptyTextBox;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Location = new Point(51, 386);
            label5.Margin = new Padding(4);
            label5.Name = "label5";
            label5.Size = new Size(158, 29);
            label5.TabIndex = 10;
            label5.Text = "عنوان الشركة:";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top;
            btnSave.BackColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Popup;
            btnSave.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSave.Image = Properties.Resources.Save_32;
            btnSave.ImageAlign = ContentAlignment.MiddleRight;
            btnSave.Location = new Point(410, 602);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(185, 61);
            btnSave.TabIndex = 6;
            btnSave.Text = "إضافة";
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
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageAlign = ContentAlignment.MiddleRight;
            btnClose.Location = new Point(603, 602);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(185, 61);
            btnClose.TabIndex = 7;
            btnClose.Text = "إغلاق";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // btnSaveAndClose
            // 
            btnSaveAndClose.Anchor = AnchorStyles.Top;
            btnSaveAndClose.BackColor = Color.White;
            btnSaveAndClose.FlatStyle = FlatStyle.Popup;
            btnSaveAndClose.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            btnSaveAndClose.Image = Properties.Resources.Save_32;
            btnSaveAndClose.ImageAlign = ContentAlignment.MiddleRight;
            btnSaveAndClose.Location = new Point(217, 602);
            btnSaveAndClose.Margin = new Padding(4);
            btnSaveAndClose.Name = "btnSaveAndClose";
            btnSaveAndClose.Size = new Size(185, 61);
            btnSaveAndClose.TabIndex = 5;
            btnSaveAndClose.Text = "إضافة وإغلاق";
            btnSaveAndClose.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAndClose.UseVisualStyleBackColor = false;
            btnSaveAndClose.Click += btnSaveAndClose_Click;
            // 
            // frmAddUpdateCompany
            // 
            AcceptButton = btnSave;
            AutoScaleMode = AutoScaleMode.None;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            BackColor = Color.WhiteSmoke;
            CancelButton = btnClose;
            ClientSize = new Size(985, 701);
            Controls.Add(btnSaveAndClose);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
            Controls.Add(txtCompanyAddress);
            Controls.Add(label5);
            Controls.Add(txtCompanyPhone);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(txtCompanyName);
            Controls.Add(txtCompanyID);
            Controls.Add(txtOtherDetails);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(lblTitle);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            Name = "frmAddUpdateCompany";
            Padding = new Padding(4);
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "إضافة / تعديل شركة";
            FormClosing += frmAddUpdateCompany_FormClosing;
            Load += frmAddUpdateCompany_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtOtherDetails;
        private Label label4;
        private TextBox txtCompanyName;
        private TextBox txtCompanyID;
        private Label label3;
        private Label label2;
        private Label lblTitle;
        private PictureBox pictureBox1;
        private ErrorProvider errorProvider1;
        private TextBox txtCompanyAddress;
        private Label label5;
        private TextBox txtCompanyPhone;
        private Label label1;
        private Button btnSave;
        private Button btnClose;
        private Button btnSaveAndClose;
    }
}